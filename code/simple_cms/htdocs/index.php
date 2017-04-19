<?php

 error_reporting(E_ALL);
 ini_set("display_errors", 1);

require_once '../config/config.php';
require_once __DIR__.'/../vendor/silex/vendor/autoload.php';
require_once 'lib/contact_page.php';
require_once 'lib/content_page.php';

use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\HttpKernel\HttpKernelInterface;

$app = new Silex\Application();

$app->register(new Silex\Provider\TwigServiceProvider(), array(
    'twig.path' => __DIR__.'/templates',
    'cache' => false,
    'debug' => false
));
$app->register(new Silex\Provider\SessionServiceProvider());
$app->register(new Silex\Provider\DoctrineServiceProvider(), array('db.options' => $config['db_info']) );

$app->get('/', function() use ($app) {
    $subRequest = Request::create('/home', 'GET');
    return $app->handle($subRequest, HttpKernelInterface::SUB_REQUEST);
});

$app->post('/', function(Request $request) use ($app) {
    $subRequest = Request::create('/home', 'POST', $request->request->all());
    return $app->handle($subRequest, HttpKernelInterface::SUB_REQUEST);
});

$app->get('/login', function () use ($app) {
    $username = $app['request']->server->get('PHP_AUTH_USER', false);
    $password = $app['request']->server->get('PHP_AUTH_PW');

    if (ADMIN_USER === $username && ADMIN_PASS === $password) {
        $app['session']->set('user', array('username' => $username));
        return $app->redirect('home');
    }

    $response = new Response();
    $response->headers->set('WWW-Authenticate', sprintf('Basic realm="%s"', 'site_login'));
    $response->setStatusCode(401, 'Please sign in.');
    return $response;
});

$app->get('/logout', function () use ($app) {
    $app['session']->clear();
    return $app->redirect('home');
});


$app->get('/{page_tag}', function($page_tag) use ($app) {
    $content = new content_page($app);
    $content->set_page_tag($page_tag);
    return $app['twig']->render('healing_path_base_template.html', $content->show_page());
});

$app->post('/{page_tag}', function(Request $request, $page_tag) use ($app) {
    $content = new content_page($app);
    $content->set_page_tag($page_tag);
    $content->process_post_action($request->request->all());
    return $app['twig']->render('healing_path_base_template.html', $content->show_page($request->request->all()));
});

$app->run();
?>
