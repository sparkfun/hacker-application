<!DOCTYPE html>
<html>
<head>
    <title>{translate to="{$page}.meta.title"}</title>

    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <!-- Standard meta tags -->
    {foreach from=$translator->translate("{$page}.meta.tags") item=content key=name}
        <meta name="{$name}" content="{$content}" />
    {/foreach}

    <!-- OG meta tags -->
    {foreach from=$translator->translate("{$page}.meta.og") item=content key=property}
        <meta property="og:{$property}" content="{$content}" />
    {/foreach}

    <link rel="canonical" href="{$translator->translate("{$page}.canonical")}" />
    
    <!-- <link href="/css/reset.css" rel="stylesheet" type="text/css" /> -->
    <link href="/libcss/bootstrap/3.1.1/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="/css/structure.css" rel="stylesheet" type="text/css" />
    <!-- <link href="//cloud.typography.com/6239252/712222/css/fonts.css" type="text/css" rel="stylesheet"> -->

    {foreach $objPageConfig->getCSS() as $strStyle}
        {$strStyle}
    {/foreach}
    
    <!--[if IE]>
    <link href="/css/ie.css" rel="stylesheet" type="text/css" />
    <![endif]-->
    
    <!--[if lt IE 9]>
    <link href="/css/ie-old.css" rel="stylesheet" type="text/css" />
    <script src="/js/html5shiv.min.js"></script>
    <script src="/js/respond.min.js"></script>
    <![endif]-->

    <script src="https://immanalytics.com" async></script>
</head>
<body class="male">
{$gtm}
{$mp_pixel}

{$objPageConfig->getTemplate('header')}
<div class="container">
{foreach $objPageConfig->getSections() as $objSectionConfig}
    {$objSectionConfig->render()}
{/foreach}

<div class="overlay"></div>
<div class="browser-fail-overlay"></div>
<div class="browser-fail">
    <div class="browser-fail-content">
        <h5>Your browser is out of date.</h5>

        <p>To get the best possible experience using our website we recommend that you upgrade to a newer version or another web browser.<br />
            A list of popular web browsers can be found below.</p>

        <p class="browser-rec">Click on an icon below to get to the download page</p>

        <p class="browser-links cf">
            <span class="chrome"><a href="https://www.google.com/chrome">Chrome</a></span>
            <span class="firefox"><a href="http://www.mozilla.com/en-US/firefox/all.html">Firefox</a></span>
            <span class="ie"><a href="http://www.microsoft.com/windows/internet-explorer/worldwide-sites.aspx">Internet Explorer</a></span>
            <span class="safari"><a href="http://apple.com/safari/download/">Safari</a></span>
        </p>

    </div>
</div>

</div>

<script src="/js/jquery-1.11.2.js"></script>
<script src="/js/jquery-ui-1.11.2.min.js"></script>

<script src="/js/signup.js"></script>
{if (isset($ext_js))}
    {$ext_js}
{/if}

{foreach $objPageConfig->getJS() as $strJS}
    {$strJS}
{/foreach}

{$pixel}

</body>
</html>
