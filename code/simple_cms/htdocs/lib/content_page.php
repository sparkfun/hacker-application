<?php

class content_page
{
    private $app;
    private $page_tag = 'home';
    private $page;
    private $is_admin = false;
    private $alert_array = array();

    public function __construct($app)
    {
        $this->app = $app;

        $user = $this->app['session']->get('user');

        if (isset($user['username']))
            $this->is_admin = true;
    }

    public function show_page($post_params = array())
    {
        // Get class_content if page_class is set and class exists.
        if ($this->page['page_class'] && class_exists($this->page['page_class']))
        {
            $page_class_obj = new $this->page['page_class']($this->app, $post_params);
            $this->page['class_content'] = $page_class_obj->process_page();
        }
        else
            $this->page['class_content'] = null;

        $this->page['nav_array']    = $this->get_nav_array();
        $this->page['alert_array']  = $this->alert_array;
        $this->page['is_admin']     = $this->is_admin;

        return $this->page;
    }

    public function process_post_action($post_params)
    {
        // Verify user is an administrator and check for edit_page flag.
        if ($this->is_admin and isset($post_params['edit_page']))
        {
            if ($this->update_page($post_params))
                $this->set_alert('success', 'Updated', 'Your page content has been updated.');
            else
                $this->set_alert('error', 'Oops', 'Something went wrong with updating your page.');
            $this->set_page_tag($this->page_tag);
        }
        elseif ($this->page['page_class'] && class_exists($this->page['page_class']))
        {
            // Run a specific method in the page class to process post.
            $page_class_obj = new $this->page['page_class']($this->app, $post_params);
            if ($status = $page_class_obj->process_post())
                $this->set_alert('error', 'Oops', $status);
            else
                $this->set_alert('success', 'Message sent', 'Your message has been sent, thank you!');
        }
    }

    public function set_page_tag($page_tag)
    {
        $this->page_tag = $page_tag;

        if (!$this->page = $this->get_page())
            $this->set_page_tag('404');
    }

    // types: success, error, info, block.
    protected function set_alert($type, $slug, $text)
    {
        $this->alert_array[] = array(
            'type' => $type,
            'slug' => $slug,
            'text' => $text
        );
    }

    protected function get_page()
    {
        $sql = "SELECT
                    p.page_name,
                    p.title,
                    p.content,
                    p.page_class,
                    p.page_tag
                FROM pages p
                WHERE
                    p.page_tag = :page_tag;";
        $params = array('page_tag' => $this->page_tag);

        return $this->app['db']->fetchAssoc($sql, $params);
    }

    protected function update_page($page_attributes)
    {
        $sql = "UPDATE pages
                SET
                    page_name   = :page_name,
                    title       = :title,
                    content     = :content
                WHERE
                    page_tag    = :page_tag;";
        $params = array(
            'page_name' => $page_attributes['page_name'],
            'title'     => $page_attributes['title'],
            'content'   => $page_attributes['content'],
            'page_tag'  => $page_attributes['page_tag']
        );

        $pdo = $this->app['db']->prepare($sql);

        return $pdo->execute($params);
    }

    protected function get_nav_array()
    {
        $sql = "SELECT
                    p.page_id,
                    p.page_name,
                    p.page_tag,
                    IF(p2.nav_parent IS NULL, 0, COUNT(*)) AS sub_page_count
                FROM pages p
                LEFT JOIN pages p2 ON p.page_id = p2.nav_parent
                WHERE
                    p.enabled = 1
                    AND p.in_nav = 1
                    AND p.nav_parent = 0
                GROUP BY
                    p.page_id
                ORDER BY
                    p.page_order ASC, p.page_name ASC;";

        $nav['tabs'] = $this->app['db']->fetchAll($sql);

        $sql = "SELECT
                    p.nav_parent,
                    p.page_name,
                    p.page_tag
                FROM pages p
                WHERE
                    p.enabled = 1
                    AND p.in_nav = 1
                    AND p.nav_parent > 0
                ORDER BY
                    p.page_order ASC, p.page_name ASC;";

        $sub_nav_items = $this->app['db']->fetchAll($sql);

        foreach ($sub_nav_items as $key => $value)
        {
            $nav['sub_nav'][$value['nav_parent']][] = $value;
        }

        return $nav;
    }
}

?>
