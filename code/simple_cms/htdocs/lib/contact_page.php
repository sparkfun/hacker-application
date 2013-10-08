<?php

class contact_page
{
    protected $app;
    protected $email_to         = CONTACT_TO;
    protected $email_subject    = CONTACT_SUBJECT;
    protected $post_params      = array();

    public function __construct($app, $post_params)
    {
        $this->app = $app;
        $this->post_params = $post_params;
    }

    public function process_page()
    {
        return $this->app['twig']->render('contact_page_template.html', $this->post_params);
    }

    public function process_post()
    {
        if ($validation_errors = $this->validate_parameters())
            return $validation_errors;

        if (mail($this->email_to, $this->email_subject . ' from ' . $this->post_params['contact_name'], $this->post_params['contact_message'], 'From: ' . $this->post_params['contact_name'] . '<' . $this->post_params['contact_email'] . '>'))
            return false;
        else
            return 'Something went wrong while sending your message.  Please contact us at: <a href="mailto:' . $this->get_email_to() . '">' . $this->get_email_to() . '</a>';
    }

    protected function validate_parameters()
    {
        $error_text = '';

        if (!$this->post_params['contact_name'])
            $error_text .= 'Please fill out your name. ';

        if (!$this->post_params['contact_email'])
            $error_text .= 'Please fill out your email address. ';
        elseif (!filter_var($this->post_params['contact_email'], FILTER_VALIDATE_EMAIL))
            $error_text .= 'The email address you provided is invalid. ';

        if (!$this->post_params['contact_message'])
            $error_text .= 'Please fill out a message.';

        return $error_text;
    }

    protected function get_email_to()
    {
        return $this->email_to;
    }
}
?>
