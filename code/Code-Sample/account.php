<?php
    include_once "common/base.php";
    if(isset($_SESSION['LoggedIn']) && $_SESSION['LoggedIn']==1):
        $pageTitle = "Your Account";
        include_once "common/header.php";
        include_once 'inc/class.users.inc.php';
        $users = new ColoredListsUsers($db);

        if(isset($_GET['email']) && $_GET['email']=="changed")
        {
            echo "<div class='message good'>Your email address "
                . "has been changed.</div>";
        }
        else if(isset($_GET['email']) && $_GET['email']=="failed")
        {
            echo "<div class='message bad'>There was an error "
                . "changing your email address.</div>";
        }

        if(isset($_GET['password']) && $_GET['password']=="changed")
        {
            echo "<div class='message good'>Your password "
                . "has been changed.</div>";
        }
        elseif(isset($_GET['password']) && $_GET['password']=="nomatch")
        {
            echo "<div class='message bad'>The two passwords "
                . "did not match. Try again!</div>";
        }

        if(isset($_GET['delete']) && $_GET['delete']=="failed")
        {
            echo "<div class='message bad'>There was an error "
                . "deleting your account.</div>";
        }

        list($userID, $v) = $users->retrieveAccountInfo();
?>

        <h2>Your Account</h2>
        <form method="post" action="db-interaction/users.php">
            <div>
                <input type="hidden" name="userid"
                    value="<?php echo $userID ?>" />
                <input type="hidden" name="action"
                    value="changeemail" />
                <input type="text" name="username" id="username" />
                <label for="username">Change Email Address</label>
                <br /><br />
                <input type="submit" name="change-email-submit"
                    id="change-email-submit" value="Change Email"
                    class="button" />
            </div>
        </form><br /><br />

        <form method="post" action="db-interaction/users.php"
            id="change-password-form">
            <div>
                <input type="hidden" name="user-id"
                    value="<?php echo $userID ?>" />
                <input type="hidden" name="v"
                    value="<?php echo $v ?>" />
                <input type="hidden" name="action"
                    value="changepassword" />
                <input type="password"
                    name="p" id="new-password" />
                <label for="password">New Password</label>
                <br /><br />
                <input type="password" name="r"
                    id="repeat-new-password" />
                <label for="password">Repeat New Password</label>
                <br /><br />
                <input type="submit" name="change-password-submit"
                    id="change-password-submit" value="Change Password"
                    class="button" />
            </div>
        </form>
        <hr />

        <form method="post" action="deleteaccount.php"
            id="delete-account-form">
            <div>
                <input type="hidden" name="user-id"
                    value="<?php echo $userID ?>" />
                <input type="submit"
                    name="delete-account-submit" id="delete-account-submit"
                    value="Delete Account?" class="button" />
            </div>
        </form>

<?php
    else:
        header("Location: /");
        exit;
    endif;
?>

<div class="clear"></div>

<?php
    include_once "common/sidebar.php";
    include_once "common/footer.php";
?>