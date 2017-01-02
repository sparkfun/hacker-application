<?php


// Hook into WordPress to allow creation of the SiteWideSearch table.
add_action('init', 'BaseSiteWideSearch::installSearchTable');

// Generating the search table can be a little expensive, so we should up the limit just to be safe.
set_time_limit(60);

/**
 * BaseSiteWideSearch allows querying of a specialized db table (created with this class)
 * via full text search.
 * This class must be extended and cannot be used directly.
 * According to best practices, an object should be
 * instantiated (purely static class/methods are an anti-pattern) from the child class,
 * but the nature of this functionality and the way it hooks up to WordPress doesn't really warrant it.
 * IMPORTANT! You must use this functionality in tangent with TacoWordPress or it will not work.
 * @link https://github.com/tacowordpress/tacowordpress
 */
Abstract Class BaseSiteWideSearch {

  public static $results = null;

  public static $default_site_wide_search_results_limit = 20;

  // Table name in the database. Must be MyISAM
  public static $table_name = 'sitewidesearch';

  /**
   * Get only the fields specified in the getAllowedJSONFields method
   * @param collection $results
   * @return array
   */
  private static function getOnlyJSONFields($results) {

    $child_class = get_called_class();
    $json_fields = $child_class::getAllowedJSONFields();
    $array_filtered_fields = array();
    $inc = 0;
    foreach($results as $result) {
      foreach($json_fields as $field) {
        $array_filtered_fields[$inc][$field] = $result->get($field);
      }
      // todo: move this into child class
      if(in_array('permalink', $json_fields)
        && in_array('ID', $json_fields)) {
          $array_filtered_fields[$inc]['permalink'] = get_permalink($result->get('ID'));
      }
      if(in_array('post_content', $json_fields)) {
        $array_filtered_fields[$inc]['post_content'] = get_excerpt($result->get('post_content'), 0, 80);
      }
      if(in_array('post_title', $json_fields)) {
        $array_filtered_fields[$inc]['post_title'] = get_excerpt($result->get('post_title'), 0, 50);
      }
      if(in_array('post_name', $json_fields)) {
        $array_filtered_fields[$inc]['post_name'] = $result->get('post_title');
      }
      $inc++;
    }
    return $array_filtered_fields;
  }


  /**
   * If any of the configuration from the extended class has changed,
   * regenerate the search table.
   * @return void
   */
  public static function checkForConfigChanges() {
    $child_class = get_called_class();
    global $wpdb;

    if(!$wpdb->query("SELECT * FROM sitewidesearch")) {
      $child_class::regenerateSearchTable(true);
    }

    if(!get_option('sitewidesearch_config')) {
      add_option(
        'sitewidesearch_config',
        serialize($child_class::getSearchableFields())
      );
    }

    $config = unserialize(get_option('sitewidesearch_config'));

    if($config !== $child_class::getSearchableFields()) {
      update_option(
        'sitewidesearch_config',
        serialize($child_class::getSearchableFields())
      );
      self::regenerateSearchTable(true);
    }
  }


  /**
   * Regenerate the search table
   * @param boolean $from_scratch (expensive)
   * @return void
   */
  public static function regenerateSearchTable($from_scratch=false) {
    if($from_scratch) {
      self::regenerateSearchTableFromScratch();
    }
    global $wpdb;
    $records = $wpdb->get_results("SELECT * FROM sitewidesearch");
    foreach($records as $r) {
      self::postModified($r->post_id, true);
    }
  }


  /**
   * Regenerate the search table from scratch
   * WARNING - This is expensive (use a cron job after hours)
   * @return void
   */
  public static function regenerateSearchTableFromScratch() {
    global $wpdb;
    $post_types = self::getPostTypes();
    unset($post_types['default']);

    $post_types = join(
      ',',
      array_map(function($s) { return "'$s'"; }, $post_types)
    );

    $query = sprintf(
      "SELECT ID FROM %sposts
      WHERE post_type IN (%s)
      AND post_status = 'publish'
      ",
      $wpdb->prefix,
      $post_types
    );

    $records = array_map(
      function($o) {
        return $o->ID;
      },
      $wpdb->get_results($query, OBJECT)
    );

    foreach($records as $post_id) {
      self::postModified($post_id, true);
    }
  }


  /**
   * Create a search table if none exists
   * The fields meta_1 – meta_10 correspond to the
   * points (ranking) given in the extended class.
   * @return void
   */
  public static function installSearchTable() {
    global $wpdb;

    $table_name = self::$table_name;

    $test_string = sprintf("show tables like '%s'", $table_name);

    if($wpdb->get_var($test_string) ==  $table_name) return;

    $sql = sprintf(
      "CREATE TABLE %s (
        `post_id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
        `post_type` text,
        `term_ids` longtext,
        `post_title` text,
        `post_name` varchar(200),
        `terms` longtext,
        `taxonomies` longtext,
        `meta_1` longtext,
        `meta_2` longtext,
        `meta_3` longtext,
        `meta_4` longtext,
        `meta_5` longtext,
        `meta_6` longtext,
        `meta_7` longtext,
        `meta_8` longtext,
        `meta_9` longtext,
        `meta_10`  longtext,
        PRIMARY KEY (`post_id`)
      ) ENGINE=MyISAM;",
      $table_name
    );
    $wpdb->query($sql);
    self::regenerateSearchTable(true);
  }


  /**
   * Watch if a post has been modified. If so, create a WordPress hook to
   * modify the search table.
   * @return void
   */
  public static function watchPostStatuses() {
    $child_class = get_called_class();
    $post_types = array_filter(
      array_keys($child_class::getSearchableFields())
    );

    $post_statuses = array(
      'new',
      'publish',
      'pending',
      'draft',
      'auto-draft',
      'future',
      'private',
      'inherit',
      'trash',
    );

    foreach($post_types as $post_type) {
      foreach($post_statuses as $status) {
        add_action(
          sprintf('%s_%s', $status, $post_type),
          sprintf('%s::postModified', $child_class)
        );
      }
    }
  }


  /**
   * If a post has been modified/created,
   * update or insert into the search table.
   * @param  object $post The post object that has been created or modified
   * @param  boolean $force_update No matter what, update/create the post in the search table.
   * @return void
   */
  public function postModified($post, $force_update=false) {
    global $wpdb;
    $table_name = self::$table_name;

    $post_wp_object = get_post($post);
    if(is_null($post_wp_object)) return;

    if(in_array($post_wp_object->post_status, array('trash', 'pending', 'private', 'draft', 'auto-draft', 'inherit'))
      && !$force_update) {
      self::postDeleted($post_wp_object->ID);
      return;
    }

    $taco_post = \Taco\Post\Factory::create(get_post($post_wp_object));

    $child_class = get_called_class();

    $post_types_fields = $child_class::getSearchableFields();
    $post_type = $taco_post->post_type;
    $fields_weights = $post_types_fields[$post_type];
    $weighted_meta = array(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

    foreach($weighted_meta as $weight) {
      $weighted_meta['meta_'.$weight] = array_flip($fields_weights)[$weight];
    }

    $weighted_meta = array_filter($weighted_meta, function($item) use ($taco_post) {
      //if(in_array($item, array('post_title', 'post_name'))) return false;
      if(!$item && $item !== 0) return true;
      if(!is_numeric($item)) return true;
    });

    unset($weighted_meta['meta_0']);

    $weighted_meta = array_map(function($item) use ($taco_post) {
      if(!strlen($item)) return;
      return $taco_post->get($item);
    }, $weighted_meta);


    $tax_terms = wp_get_post_terms($taco_post->ID, get_taxonomies());
    $term_ids = join(', ', Collection::pluck($tax_terms, 'term_id'));
    $term_names = join(', ', Collection::pluck($tax_terms, 'name'));
    $taxonomies = join(', ', array_unique(Collection::pluck($tax_terms, 'taxonomy')));

    $row_exists = $wpdb->query(
      sprintf(
        "SELECT post_id from sitewidesearch where post_id = %d",
        $taco_post->ID
      )
    );

    $action = ($row_exists) ? 'update' : 'insert';

    $data = array_merge(
      array(
        'post_id' => $taco_post->ID,
        'post_type' => $taco_post->post_type,
        'post_title' => $taco_post->post_title,
        'post_name' => $taco_post->post_name,
        'terms' => $term_names,
        'term_ids' => $term_ids,
        'taxonomies' => $taxonomies,
      ),
      $weighted_meta
    );

    $meta_sprint = array_fill(1, 10, '%s');

    if($action === 'insert') {
      $wpdb->insert(
        $table_name,
        $data,
        array_merge(
          array('%d', '%s', '%s', '%s', '%s', '%s', '%s'),
          $meta_sprint
        )
      );
    }

    if($action === 'update') {
      $wpdb->update(
        $table_name,
        $data,
        array('post_id' => $taco_post->ID),
        array_merge(
          array('%d', '%s', '%s', '%s', '%s', '%s', '%s'),
          $meta_sprint
        )
      );
    }
  }


  /**
   * Delete a post in the search table.
   * @param int $id the post ID
   * @return void
   */
  public function postDeleted($id) {
    global $wpdb;

    $sql = sprintf(
      "DELETE FROM `sitewidesearch` WHERE `post_id` IN ('%d');",
      $id
    );

    $wpdb->query($sql);
  }


  /**
   * Get a joined array of post types (will eventually allow for fields and points)
   * @param array $array
   * @return string
   */
  private static function getQueryFromCriteria($array) {
    $array_post_types = array();
    foreach($array as $post_type) {
      $array_post_types[] = "'$post_type'";
    }
    return join(",", $array_post_types);
  }


  /**
   * Get Taco Posts from an array of ids
   * @param array $array_ids
   * @return collection
   */
  private static function convertToTacoPosts($array_ids) {
    return \Taco\Post\Factory::createMultiple($array_ids);
  }


  /**
   * Set and get default ranking fields here
   * @return array
   */
  public function getDefaultRankingFields() {
    return array(
      'post_title'   => 5,
      'post_name' => 4,
      'post_content' => 1,
      'terms' => 3
    );
  }


  /**
   * Get an array of field => values without post_types
   * @return array
   */
  public static function getAllFields() {
    $child_class = get_called_class();
    $post_type_fields = $child_class::getSearchableFields();
    $new_array = array();
    foreach($post_type_fields as $post_type => $fields) {
      foreach($fields as $k => $v) {
        $new_array[$k] = $v;
      }
    }
    return $new_array;
  }


  /**
   * Get an associative array of key (field name) => value (points) fields
   * @return array
   */
  public function getRankingFields() {
    $default_fields = self::getDefaultRankingFields();
    $intersected = array_intersect_key(self::getAllFields(), $default_fields);
    return ($intersected + $default_fields);
  }


  /**
   * Construct a query part for post fields
   * @param string $keywords
   * @return array
   */
  private function constructPostFieldsQuery($keywords) {
    $query = array();
    $field_points = self::getRankingFields();
    foreach($field_points as $key => $points) {
      $query[] = sprintf(
        "(%d * (MATCH (%s) AGAINST ('%s' IN BOOLEAN MODE))) \r\n",
        $points,
        $key,
        $keywords
      );
    }
    return join(' + ', $query);
  }


  /**
   * Unset default fields from array of fields
   * @param array $fields
   * @return array
   */
  private static function unsetDefaults($post_type_fields) {
    $default_fields = self::getDefaultRankingFields();
    foreach($default_fields as $k => $v) {
      foreach($post_type_fields as $post_type => $fields) {
        if(!Arr::iterable($post_type_fields[$post_type])) continue;
        unset($post_type_fields[$post_type][$k]);
      }
    }
    return $post_type_fields;
  }


  /**
   * Construct query parts for postmeta
   * @param string $keywords
   * @return array
   */
  private static function constructPostMetaQuery($keywords) {
    $child_class = get_called_class();
    $post_type_fields = self::unsetDefaults($child_class::getSearchableFields());
    $query_1 = array();
    $query_2 = array();
    $inc = 0;
    global $wpdb;
    $wp_db_prefix = $wpdb->prefix;
    foreach($post_type_fields as $post_type => $fields) {
      $post_type_query = (strlen($post_type))
          ? sprintf("and post_type = '%s'", $post_type)
          : '';

      foreach($fields as $key => $points) {
        $field_key_query =  sprintf("AND pm%d.meta_key = '%s'", $inc, $key);
        if(!$post_type && !in_array($key, array_keys(self::getDefaultRankingFields()))) {
          throw new Exception("Fields defined must be a default field or belong to a custom post type.", 1);
        }
        $query_1[] = sprintf(
          "(%d * (MATCH (pm%d.meta_value) AGAINST ('%s' IN BOOLEAN MODE))) \r\n",
          $points,
          $inc,
          $keywords
        );
        $query_2[] = sprintf(
          "LEFT JOIN %spostmeta AS pm%d ON pm%d.post_id = %sposts.ID %s %s \r\n",
          $wp_db_prefix,
          $inc,
          $inc,
          $wp_db_prefix,
          $post_type_query,
          $field_key_query
        );
        $inc++;
      }
    }
    return array(
      join(' + ', $query_1),
      join($query_2)
    );
  }


  /**
   * Returns the post_types defined in getSearchableFields()
   * @return array
   */
  public static function getPostTypes() {
    $child_class = get_called_class();
    $searchable_fields = $child_class::getSearchableFields();
    unset($searchable_fields['default']);
    return array_filter(array_keys($searchable_fields));
  }


  /**
   * Get the query part for meta points
   * @param  string $keywords
   * @return string MySQL query (part)
   */
  public static function getMetaPointsQuery($keywords='') {
    $query = array();
    for($i = 1; $i <= 10; $i++) {
      $query[]  = sprintf(
        "+ (%d * (MATCH (meta_%d) AGAINST ('%s' IN BOOLEAN MODE)))",
        $i,
        $i,
        $keywords
      );
    }
    return join("\r\n ", $query);
  }


  /**
   * Get the default query part for meta points
   * @param string $keywords
   * @param array $type_values An array of of $field => $value
   * @return string MySQL query (part)
   */
  public static function getDefaultPointsQuery($keywords='', $type_values) {
    $query = array();
    $defaults = array('post_title', 'post_name', 'terms', 'taxonomies');
    foreach($type_values as $field => $value) {
      if(!in_array($field, $defaults)) continue;
      $query[]  = sprintf(
        "+ (%d * (MATCH (%s) AGAINST ('%s' IN BOOLEAN MODE)))",
        $value,
        $field,
        $keywords
      );
    }
    return join("\r\n ", $query);
  }


  /**
   * Get the query part for terms
   * @param array $terms
   * @return string MySQL query (part)
   */
  public static function getTermsQuery($terms) {
    $query = array();
    foreach($terms as $term_id) {
      $query[] = sprintf('OR locate(%d, term_ids)', $term_id);
    }
    return join("\r\n ", $query);
  }


  /**
   * Get the query part for full text
   * @param string $keywords
   * @return string MySQL query (part)
   */
  public static function getFullTextQuery($keywords) {

    if(!strlen($keywords)) return '';

    $child_class = get_called_class();
    $post_type_field_values = $child_class::getSearchableFields();
    $query = array(',');
    $query[] = 'CASE';
    foreach($post_type_field_values as $post_type => $type_values) {

      if($post_type === 'default' ||
        !\AppLibrary\Arr::iterable($type_values)) continue;

      $query[] = sprintf('WHEN post_type=\'%s\' then', $post_type);
      $query[] = 'MAX(';
      $query[] = self::getDefaultPointsQuery($keywords, $type_values);
      $query[] = self::getMetaPointsQuery($keywords);
      $query[] = ')';
    }
    $query[] = 'ELSE';
    $query[] = self::getDefaultPointsQuery($keywords, $post_type_field_values['default']);
    $query[] = self::getMetaPointsQuery($keywords);
    $query[] = 'END as score';

    return join("\r\n ", $query);
  }


  /**
   * Get
   * @param string $keywords text that should be queried (full text)
   * @param array $terms taxonomy terms
   * @param boolean $to_json true returns JSON
   * @return array or JSON
   */
  public static function getSearchResults($keywords, $terms=[], $to_json=false) {
    global $wpdb;
    $post_types = self::getPostTypes();
    unset($post_types['default']);
    $post_types = join(
      ',',
      array_map(function($s) { return "'$s'"; }, self::getPostTypes())
    );

    $child_class = get_called_class();
    $search_results_limit = (isset($child_class::$site_wide_search_results_limit))
      ? $child_class::$site_wide_search_results_limit
      : self::$default_site_wide_search_results_limit;

    $query = sprintf(
      "SELECT
      post_id
      %s
      FROM sitewidesearch
      WHERE 1 = 1
      AND post_type in (%s)
      %s
      GROUP BY post_id
      %s
      DESC
      LIMIT %d",
      self::getFullTextQuery($keywords),
      $post_types,
      self::getTermsQuery($terms),
      (strlen($keywords))
        ? 'HAVING score > 0 ORDER BY score'
        : '',
      $search_results_limit
    );

    $results = $wpdb->get_results($query);

    if(!\AppLibrary\Arr::iterable($results)) return array();

    if(!$to_json) {
      self::$results = self::convertToTacoPosts(\AppLibrary\Collection::pluck($results, 'post_id'));
      return self::$results;
    }
    $results_taco = self::convertToTacoPosts(\AppLibrary\Collection::pluck($results, 'post_id'));
    self::$results = $results;

    $results_stripped = self::getOnlyJSONFields($results_taco);
    echo json_encode($results_stripped);
    exit;

  }


  /**
   * Get a collection of results (Taco Post Type Objects) from the database
   * @param string $keywords
   * @param bool $to_json
   * @return collection
   */
  public static function getSearchResultsOld($keywords, $to_json = false, $limit = 100, $offset = 0) {
    if(!strlen($keywords)) return array();
    $keywords = Str::machine($keywords, ' ');
    $child_class = get_called_class();
    $post_types = self::getQueryFromCriteria($child_class::getPostTypes());
    $array_query_postmeta = self::constructPostMetaQuery($keywords);

    global $wpdb;
    $wp_db_prefix = $wpdb->prefix;
    $query = sprintf("
      select
      ID,
      max(%s
      %s) as score
      FROM %sposts
      %s
      WHERE 1 = 1
      AND post_type in (%s)
      %s
      AND post_status = 'publish'
      GROUP BY ID
      HAVING score > 0
      ORDER BY score
      DESC
      LIMIT %d
      OFFSET %d",
      $array_query_postmeta[0] . '+',
      self::constructPostFieldsQuery($keywords),
      $wp_db_prefix,
      $array_query_postmeta[1],
      $post_types,
      $array_query_postmeta[2],
      $limit,
      $offset
    );
    $results = $wpdb->get_results($query);

    if(!Arr::iterable($results)) return array();

    if(!$to_json) {
      self::$results = self::convertToTacoPosts(Collection::pluck($results, 'ID'));
      return self::$results;
    }
    $results_taco = self::convertToTacoPosts(Collection::pluck($results, 'ID'));
    self::$results = $results;

    $results_stripped = self::getOnlyJSONFields($results_taco);
    echo json_encode($results_stripped);
    exit;
  }


  /**
   * get a collection of results categorized by post type
   */
  public static function getOrganizedByPostType($results=null, $limit=4) {
    if(!Arr::iterable($results)) {
      if(!Arr::iterable(self::$results)) return array();
    }
    $results = (Arr::iterable($results))
      ? $results
      : self::$results;
    $child_class = get_called_class();

    $post_types = array_flip(array_filter(
      array_keys($child_class::getSearchableFields())
    ));

    foreach($post_types as $k => $v) {
      $post_types[$k] = array();
    }

    foreach(self::$results as $result) {
      if(count($post_types[$result->post_type]) == $limit) continue;
      $post_types[$result->post_type][] = $result;
    }
    return array_filter($post_types);
  }
}
