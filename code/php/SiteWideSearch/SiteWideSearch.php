<?php

// Hook into WordPress to allow creation and modification of the SiteWideSearch table.
add_action('init', 'SiteWideSearch::watchPostStatuses');
add_action('init', 'SiteWideSearch::checkForConfigChanges');

/**
 * SiteWideSearch is a class that has to extend the base class of "BaseSiteWideSearch.
 * Most of the methods here are for configuration
 * purposes otherwise they are overrides of the parent class.
 * Please see "/core/BaseSiteWideSearch.php" for a full explanation of what these classes do.
 * IMPORTANT! You must use this functionality in tangent with TacoWordPress or it will not work.
 * @link https://github.com/tacowordpress/tacowordpress
 */
class SiteWideSearch extends SiteWideSearch {

  public static $site_wide_search_results_limit = 20;


  /**
   * Get a multi-dimensional array containing keys of post types
   *  and values of field => points
   * Each post type has fields that can be factored in the search results ranking system.
   * Giving a field (e.g., "post_content" of "page" in the array below)
   * a range of 1-10 will set the rank for that field.
   * The rank is essentially points that will be added up to determine the score for each search result.
   * @return array
   */
  public static function getSearchableFields() {
    return array(
      'page' => array(
        'post_content' => 2,
        'post_title' => 4,
        'post_name' => 6
      ),
      'post' => array(
        'post_content' => 1,
        'post_title' => 4,
        'post_name' => 6
      ),
      'resource' => array(
        'post_title' => 2,
        'post_name' => 4,
        'post_content' => 3,
        'resource_authors' => 7,
        'terms' => 3,
        'taxonomies' => 2
      ),
      'state-ac' => array(
        'post_title' => 4,
        'post_content' => 1,
        'post_name' => 5,
        'terms' => 3,
        'taxonomies' => 2
      ),
      'issue' => array(
        'post_title' => 4,
        'post_content' => 1,
        'post_name' => 6,
        'terms' => 3,
        'taxonomies' => 2
      ),
      'staff-member' => array(
        'post_title' => 4,
        'post_content' => 1,
        'post_name' => 6
      ),
      // override the ranking system for default fields
      'default' => array(
        'post_title' => 4,
        'post_content' => 3,
        'post_name' => 3,
        'post_content' => 1,
        'terms' => 3,
        'taxonomies' => 2
      ),
    );
  }


  /**
   * Assign allowed fields that will be used for getting data via JSON
   * @return array
   */
  public static function getAllowedJSONFields() {
    return array(
      'post_title',
      'post_content',
      'post_date',
      'permalink',
      'ID'
    );
  }
}
