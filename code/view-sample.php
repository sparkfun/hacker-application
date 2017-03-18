<?

if ($media) {
    $title = $media->title('seo');
    $desc = HTML::chars($media->description('seo'));
    $social_desc = HTML::chars($media->description('social'));
    $twitter_token = $user->token('twitter');
    $ctp_image = Arr::get($_GET, 'ctp_image');
    $image = $ctp_image ? $media->play_image() : $media->largest_image();
    $mobile = Request::user_agent('mobile');

    // Construct video_url for Facebook. Pointing to /embed_flash_autoplay
    // no longer seems to work. Was showing 'Download file' instead.
    $video_url = URL::base() . "player.swf?loader=" . URL::base() . "player/";
    $other_video_url = URL::base() . "player.swf?loader=" . URL::base() . "player/";
    if (isset($track_url))
        $video_url .= "&track_url=$track_url";
    else
        $video_url .= "&track_url=facebook.com";
    $video_url .= "&video_id=$media->hash&autoplay=true";
    $other_video_url .= "&video_id=$media->hash&autoplay=true";

    $secure_video_url = str_replace('http:','https:', $video_url);
    $twitter_player_url = str_replace('&autoplay=true', '', $secure_video_url);

    if (isset($track_url))
        $twitter_player_url = str_replace("&track_url=$track_url", '&track_url=twitter.com', $twitter_player_url);
    else
        $twitter_player_url = str_replace('&track_url=facebook.com', '&track_url=twitter.com', $twitter_player_url);

    // <video> url.
    // FIXME: Replace with $media->video_info() once HTML5 player is out.
    $video_file = FALSE;
    foreach (array('720', '480', '360') as $version) {
        $field = "v${version}p_file";
        if ($media->video->$field) {
            $video_file = $media->video->$field;
            break;
        }
    }
} else {
    $title = Kohana::config('site.name.title');
}

// Embed playlist.
if ($type == 'playlist') {
    $url = "/e/playlist/$playlist_id/$media->hash/core";

// Embed channel.
} else {
    $url = "/e/channel/$user->id/$media->hash";

    if ($type == 'mls')
        $url .= "?type=$type";
    else
        $url .= '?type=landing';

    if($autostart)
        $url .= '&autoplay=true';
}

?>

<!DOCTYPE html>
<html>

<head>
    <title><?= $title ?></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0">
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
    <? if (! $media || $type == 'mls' || $type == 'playlist' || $media->is('noindex')) { ?><meta name="robots" content="noindex" /><? } ?>

    <? if (isset($desc)) { ?><meta name="description" content="<?= $desc ?>" /><? } ?>

    <!-- Facebook -->
    <link rel="image_src" href="<?= $image ?>" />
    <meta property="og:title" content="<?= $title ?>" />
    <? if ($media->loaded()) { ?>
        <meta property="og:description" content="<?= $social_desc ?>" />
        <meta property="og:url" content="<?= $media->share_url(TRUE) ?>" />
    <? } ?>
    <meta property="og:site_name" content="<?= $user->group->name ?>" />
    <meta property="og:type" content="video.other" />
    <meta property="og:video" content="<?= $video_url ?>" />
    <meta property="og:video:type" content="application/x-shockwave-flash" />
    <meta property="og:video:secure_url" content="<?= $secure_video_url ?>" />
    <meta property="og:video:width" content="640" />
    <meta property="og:video:height" content="360" />
    <meta property="og:image" content="<?= $image ?>" />

    <!-- Twitter -->
    <meta name="twitter:card" content="player">
    <meta name="twitter:title" content="<?= $title ?>">
    <? if ($twitter_token) { ?>
        <meta name="twitter:site" content="<?= $twitter_token->label ?>">
    <? } ?>
    <meta name="twitter:description" content="<?= $social_desc ?>">
    <meta name="twitter:player" content="<?= $twitter_player_url ?>">
    <meta name="twitter:player:width" content="640">
    <meta name="twitter:player:height" content="360">
    <meta name="twitter:player:stream" content="<?= $twitter_player_url ?>">
    <meta name="twitter:player:stream:content_type" content="application/x-shockwave-flash">
    <meta name="twitter:image" content="<?= $image ?>">

    <!-- Others -->
    <meta name="medium" content="video" />
    <meta name="video_type" content="application/x-shockwave-flash" />
    <link rel="video_src" href="<?= $other_video_url ?>" />

   <link rel="stylesheet" href="/static/css/common.css">

    <? include Kohana::find_file('views', 'templates/pieces/jquery') ?>
    <script src="/static/js/lists.min.js"></script>
    <script src="/static/js/common.js"></script>
    <script src="/static/js/screenfull.js"></script>

    <link rel="stylesheet" href="/static/css/common.css">
    <link type="text/css" rel="stylesheet" href="/static/css/lists.css" />
    <link type="text/css" rel="stylesheet" href="/static/css/landing.css" />
    <? include Kohana::find_file('views', 'templates/pieces/font-awesome') ?>

<style>
    body {
        font-size: 12px;
        font-family: HelveticaNeue, 'Helvetica Light', Arial, sans-serif;
        margin: 0;
    }
    #landing {
        display: block;
        margin: 0 auto;
        max-width: 960px;
    }

    #masthead {
        color: #999;
        background: #202020;
        height: 87px;
        position: fixed;
        width: 100%;
        z-index: 30;
    }
    .left-card {
        font-family: HelveticaNeue, 'Helvetica Light', Arial, sans-serif;
        border-right: 1px solid white;
        width: 30%;
        height: 67px;
        box-sizing: content-box;
        padding: 10px;
        float: left;
    }
    .left-card #photo {
        border-radius: 28px;
        border: 2px solid #ddd;
        height: 50px;
        width: 50px;
        box-sizing: content-box;
        margin-top: 7px;
    }
    .left-card .right {
        text-align: right;
        color: white;
        font-size: 18px;
        font-weight: 300;
        line-height: 20px;
        display: inline-block;
    }
    .title-role {
        font-size: 14px;
        padding-top: 12px;
    }
    .right-card {
        font-family: HelveticaNeue, 'Helvetica Light', Arial, sans-serif;
        float: left;
        padding: 50px 0 0 10px;
    }
    .right-card span {
        color: white;
        font-size: 14px;
    }
    .center {
        margin: 0 auto;
        width: 945px;
    }
    .left img {
        /*margin: -22px 0 0 5px; */
        position: absolute;
        bottom: 0;
    }
    .redbutton {
        border: 0;
        margin-right: 3px;
        margin-top: 55px;
        padding: 4px 0;
    }

    #content {
        background-color: #000;
        height: 539px;
        max-width: 960px;
        margin: 0 auto;
    }
    .mobile-playlists-menu-bar-accommodater{
        position: absolute;
        width: 100%;
        height: 43px;
        background: #202020;
        z-index: 0;
        /*background-image: -webkit-gradient(linear,left top,left bottom,from( #3c3c3c ),to( #111 ));
        background-image: -webkit-linear-gradient( #3c3c3c,#111 );
        background-image: -moz-linear-gradient( #3c3c3c,#111 );
        background-image: -ms-linear-gradient( #3c3c3c,#111 );
        background-image: -o-linear-gradient( #3c3c3c,#111 );
        background-image: linear-gradient( #3c3c3c,#111 );*/
    }

    #details #playlist{
        margin: 0 -14px;
    }
    #list .item img.thumb, #list .item div.thumb,
    #list .false-item img.thumb, #list .false-item div.thumb{
        border: 1px solid #ddd;
    }

    /*Landing page content is meant to be displayed within element at one resolution*/
    /*So lets overwrite our media queries here to keep styles consistent*/
    .item + .inline-player-container{
        height: 434px;
    }
    .item .image:hover ~ .title,
    .item .title:hover{
        margin-top: -193px;
    }
    .overflow-deterrent #inline-player{
        width: 590px;
        height: 332px;
        left: 0;
    }
    .inline-player-container .overflow-deterrent{
        height: 332px;
        width: 895px;
    }
    #container #menu-screen,
    #container #menu-screen + .wellcomemat-branding-container{
        /*right: -290px !important;*/
        color: white;
        background: #202020;
    }
    #container #menu-screen + .wellcomemat-branding-container{
        bottom: 0;
    }
    @media (max-width: 947px) {  /* 2 columns*/
        .inline-player-container .overflow-deterrent{
            height: 332px;
            width: 590px;
        }
    }
    @media (min-width: 947px) {  /* 3 columns*/
        .inline-player-container .unfold-escape{
            left: 889px;
        }
    }
    /*^^override media queries here to keep styles consistent^^*/
    .header #search-mobile,
    #all-content .mobile-playlists-menu-bar{
        background-image: -webkit-gradient(linear,left top,left bottom,from( #202020 ),to( #202020 ));
        background-image: -webkit-linear-gradient( #202020,#202020 );
        background-image: -moz-linear-gradient( #202020,#202020 );
        background-image: -ms-linear-gradient( #202020,#202020 );
        background-image: -o-linear-gradient( #202020,#202020 );
        background-image: linear-gradient( #202020,#202020 );
    }
    #content{
        position: relative;
        top: -130px;
        background: #202020;
    }
<? if ( $mobile ) { ?>
    #content{
        top: -162px; /* must match mobile height of div above #content */
    }
<? } ?>
    #mobile-playlists #content{
        top: 3px;
    }
    .sub-body #mobile-playlists::before{
        width: 188px;
    }
    div #details{
        position: relative;
        top: 88px;
        padding-bottom: 100px;
    }
</style>
</head>
<body>

<? include Kohana::find_file('views', 'templates/pieces/google-tag-manager') ?>

<? if ( $mobile ) { ?>
    <div style="background-color: #202020; height: 162px"></div>
<? } else { ?>
    <div style="background-color: #202020; height: 88px"></div>
<? } ?>

<div id="content">

<? if ( $mobile ) { ?>
    <div class="mobile-playlists-menu-bar-accommodater"> </div>
<? } ?>
    <? if ($media) { ?>
        <div id="landing">
            <!-- type: <?= $type ?>, url: <?= $url ?> -->

            <?= Request::factory($url)->execute() ?>
        </div>

        <video controls poster="http://thumbnails.wellcomemat.com/<?= $media->largest_image_filename() ?>" style="display: none">
            <source src="<?= $media->video->http_url() . $video_file ?>" type="video/mp4" />
        </video>
    <? } else { ?>
        <div id="landing" style="color: #fff; font-size: 17px; text-align: center"><?= "This video does not exist!" ?></div>
    <? } ?>
</div>

</body>
</html>
