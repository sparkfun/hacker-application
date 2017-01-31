<div class="header">
    <div class="row">
        <div class="col-xs-6 col-sm-5 col-sm-offset-1 col-md-offset-1 col-lg-2 col-lg-offset-1">
            <a href="{if $is_mobile}{$translator->translate("header_logo.mobile.href")}{else}{$translator->translate("header_logo.desktop.href")}{/if}" target="_blank">
                <img class="logo-small" src="/img/logo-mobile.png"/>
                <img class="logo-large" src="/img/logo-desktop.png"/>
            </a>
        </div>
        <div class="col-xs-6 col-sm-4 col-sm-offset-1 col-md-3 col-md-offset-2 col-lg-2 col-lg-offset-5">
            <p class="mobile">{$translator->translate("{$page}.nav.mobile.header")}</p>
            <div class="desktop">
                <p>{$translator->translate("{$page}.nav.desktop.header")}</p>
                {assign var="subheader" value=$translator->translate("{$page}.nav.desktop.subheader")}
                {if isset($subheader)}
                <p class="subheader">{$subheader}</p>
                {/if}
            </div>
        </div>
    </div>
</div>