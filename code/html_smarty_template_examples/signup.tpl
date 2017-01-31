<div class="row">
    <div class="col-xs-12">
        <p class="sign-up-header">{$translator->translate("{$page}.header")}</p>
    </div>
</div>
<div class="row">
    <div class="col-xs-12">
        <p class="sign-up-subheader">{$translator->translate("{$page}.subheader")}</p>
    </div>
</div>
<div class="sign-up">
    <div class="row">
        <div class="col-xs-12">
            <p class="sign-up-section-header">{$translator->translate("{$page}.email_header")}</p>
        </div>
    </div>
    <form id="sign-up-form" method="post" action="/map">
        <div class="row">
            <div class="col-xs-12">
                <span class="error">Required</span>
                <input id="email" class="sign-up-input" type="text" name="email" value="Email Address" maxlength="100">
                
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <span class="error">Required</span>
                <input id="email-confirm" class="sign-up-input" type="text" name="email_confirm" value="Confirm Email" maxlength="100">
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <p class="sign-up-section-header about">{$translator->translate("{$page}.about_header")}</p>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <span class="error"></span>
                <input id="firstname" class="sign-up-input" type="text" name="firstname" value="First Name" maxlength="50">
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <span class="error"></span>
                <input id="lastname" class="sign-up-input" type="text" name="lastname" value="Last Name" maxlength="80">
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <input id="zipcode" class="sign-up-input" type="text" name="zipcode" value="Zip Code" maxlength="5">
                <!-- <input id="interest" class="sign-up-input" type="text" name="interest" value="Primary Area of Interest"> -->
                <select id="interest" class="sign-up-input" name="interest">
                    {foreach from=$translator->translate("arr_interest") item=interest key=id}
                    <option value="{$id}" >{$interest}</option>
                    {/foreach}
                </select>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <button id="sign-up-submit" class="btn btn-signup" type="submit">{$translator->translate("{$page}.sign_up_button.text")}<span class="arrow-right"></span></button>
            </div>
        </div>
        <input id="source" class="sign-up-input" type="hidden" name="source" value="{$source}">
        <input id="post_to_client" class="post_to_client" type="hidden" name="post_to_client" value="{$post_to_client}">
    </form>
</div>
