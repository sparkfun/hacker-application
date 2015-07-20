<?php
require_once 'main.inc.php';

/*****
 * Example Login Page
 *
 * This is a login page that I wrote for an internal company website. It shows
 * some general php code with some nice styled html combined. I also added some
 * random javascipt at the bottom to show you some of that.
 *
 * Thank you for your consideration!
 *****/

$error = null;

// Program Yubikey to this url
// https://zeus.greekgods.com/user/login.php?y=

if (isset($_REQUEST['y'])) {
    if (isset($_REQUEST['redirect'])) {
        header('Location: '.BASE_URL.'/user/twofactor.php?y='.$_REQUEST['y'].'&redirect='.$_REQUEST['redirect']);
    } else {
        header('Location: '.BASE_URL.'/user/twofactor.php?y='.$_REQUEST['y']);
    }
    exit();
}

function bad_login() {
    global $db;

    $whitelist_ips = array(
        '70.99.121.178',
        '192.168.1.1',
        '127.0.0.1',
    );

    if (in_array($_SERVER['REMOTE_ADDR'], $whitelist_ips)) {
        return;
    }

    $insert = true;
    $data = array(
        'addy'   => $_SERVER['REMOTE_ADDR'],
        'count'  => 0,
        'expire' => date('Y-m-d H:i:s', strtotime("+60 minute")),
    );

    $sql = "SELECT * FROM iplockout WHERE addy = :addy";
    $request = $db->prepare($sql);

    $request->bindValue(":addy", $data['addy']);

    if ($request->execute()) {
        if ($row = $request->fetch(PDO::FETCH_ASSOC)) {
            if (is_array($row) && count($row)) {
                $insert = false;
                $data = $row;
            }
        }
    }

    $data['count'] += 1;
    if ($data['count'] >= 3) {
        $data['expire'] = date('Y-m-d H:i:s', strtotime("+15 minute"));
    }

    if ($insert) {
        $sql = "INSERT INTO iplockout (addy, count, expire) VALUES (:addy, :count, :expire)";
    } else {
        $sql = "UPDATE iplockout SET count = :count, expire = :expire WHERE addy = :addy";
    }
    $request = $db->prepare($sql);

    $request->bindValue(":count",  $data['count']);
    $request->bindValue(":expire", $data['expire']);
    $request->bindValue(":addy",   $data['addy']);

    $request->execute();
}

function login() {
    global $db;

    if (!isset($_REQUEST['user']) && !isset($_REQUEST['pass'])) {
        return null;
    }

    if (trim($_REQUEST['user']) == '' || trim($_REQUEST['pass']) == '') {
        return 'You must supply a user name and a password!';
    }

    if (!isset($session)) {
        // Remove expired IP address lockouts
        $sql = "DELETE FROM iplockout WHERE expire < GETDATE()";
        $db->query($sql);

        // Check IP address to see if its currently blocked
        $sql = "SELECT * FROM iplockout WHERE count >= 3 AND addy = :addy";
        $request = $db->prepare($sql);

        $request->bindValue(":addy", $_SERVER['REMOTE_ADDR']);

        if ($request->execute()) {
            $row = $request->fetch(PDO::FETCH_ASSOC);
            if (is_array($row) && count($row) > 0) {
                $min = ceil(abs(strtotime($row['expire']) - time()) / 60);

                return 'Your IP address is currently locked out. <br> Please try again in '.$min.' minutes!';
            }
        }

        // Add domain name if the user did not provide it
        $email = trim($_REQUEST['user']);
        $domain = '@greekgods.com';
        if (!(substr($_REQUEST['user'], strlen($domain)*-1) == $domain)) {
            $email = $email.'@greekgods.com';
        }

        // Remove domain from user name
        $user = str_replace('@greekgods.com', '', $email);

        $sql = "SELECT UID, Name, Email, Type, YubikeyID FROM users WHERE active = 1 AND Email = :email";
        $request = $db->prepare($sql);

        $request->bindValue(":email", $email);

        if ($request->execute()) {
            if ($row = $request->fetch(PDO::FETCH_ASSOC)) {
                $userData = $row;

                // Remove domain from user name
                $userData['User'] = str_replace('@greekgods.com', '', $userData['Email']);

            } else {
                bad_login();
                return 'User name or password is incorrect! <!-- (0x0) -->';
            }
        } else {
            bad_login();
            return 'User name or password is incorrect! <!-- (0x1) -->';
        }

        // Check Password
        if (ldap_check($user, $_REQUEST['pass']) == false) {
            bad_login();
            return 'User name or password is incorrect! <!-- (0x2) -->';
        }

        if ($userData['YubikeyID'] != null) {
            // Generate Hash
            session_regenerate_id();
            $hash = session_id();

            // Save hash to database
            $sql = "UPDATE Users SET Password = :hash WHERE Email = :email";
            $request = $db->prepare($sql);

            $request->bindValue(":hash", $hash);
            $request->bindValue(":email", $email);

            if (!$request->execute()) {
                return 'Unable to save HASH to the database! <!-- (0x3) -->';
            }

            if (isset($_REQUEST['redirect'])) {
                header('Location: '.BASE_URL.'/user/twofactor.php?hash='.$hash.'&redirect='.$_REQUEST['redirect']);
            } else {
                header('Location: '.BASE_URL.'/user/twofactor.php?hash='.$hash);
            }
            exit();
        }

        // Remove old session
        //if (!remove_session($userData['UID'])) {
        //    return 'Unable to remove old sessions, please contact support!';
        //}

        $sessionid = null;

        // Check for existing session
        $sql = "SELECT hash FROM Session WHERE user_id = :user_id";
        $request = $db->prepare($sql);

        $request->bindValue(":user_id", $userData['UID']);

        if ($request->execute()) {
            if ($row = $request->fetch(PDO::FETCH_ASSOC)) {
                $sessionid = trim($row['hash']);
            }
        }

        if ($sessionid) {
            // Active Session found, restoring
            session_id($sessionid);

            $params = session_get_cookie_params();

            setcookie(
                session_name(),
                session_id(),
                time()+$params["lifetime"],
                $params["path"],
                COOKIE_DOMAIN,
                $params["secure"],
                $params["httponly"]
            );

        } else {
            // No Active Session, Start a new one

            // Get Session ID
            $hash = session_id();

            if ($hash == '') {
                return 'Session HASH is empty!';
            }

            // Start a session
            $sql = "INSERT INTO session (user_id, hash, last_activity) VALUES (:user_id, :hash, :date)";
            $request = $db->prepare($sql);

            $request->bindValue(":user_id", $userData['UID']);
            $request->bindValue(":hash", $hash);
            $request->bindValue(":date", date('Y-m-d H:i:s'));

            if (!$request->execute()) {
                $error = $request->errorInfo();
                return 'Unable to start session, please contact support! <!-- '.$error[2].' -->';
            }
        }

        // Set ASP cookie
        $data = array();
        $data[] = 'Name='     .rawurlencode($userData['Name']);
        $data[] = 'Email='    .rawurlencode($userData['Email']);
        $data[] = 'ID='       .rawurlencode($userData['UID']);
        $data[] = 'AgentType='.rawurlencode($userData['Type']);

        $cookie_data = implode('&', $data);

        setrawcookie(
            'Admin',
            $cookie_data,
            strtotime(date(
                'Y-m-d 02:00:00',
                strtotime("+1 day")
            )),
            '/',
            COOKIE_DOMAIN
        );

        // Redirect to main page
        if (isset($_REQUEST['redirect'])) {
            header('Location: '.BASE_URL.$_REQUEST['redirect']);
        } else {
            header('Location: '.BASE_URL.'/');
        }
        exit();
    }
}

function logout() {
    remove_session();
    return "You have been logged out!";
}

if (isset($_GET['logout'])) {
    $message = logout();
} else {
    $error = login();
}

if (isset($_REQUEST['message'])) {
    $message = $_REQUEST['message'];
}
if (isset($_REQUEST['error'])) {
    $error = $_REQUEST['error'];
}

?><!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">

        <title>ZEUS - Log In</title>

        <!-- CSS Files -->
        <link rel="stylesheet" href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.1/css/bootstrap.css">
        <link rel="stylesheet" href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.1/css/bootstrap-theme.css">

        <!-- JavaScript Libraries -->
        <script src="//cdnjs.cloudflare.com/ajax/libs/html5shiv/3.7.2/html5shiv.js"></script>
        <script src="//cdnjs.cloudflare.com/ajax/libs/html5shiv/3.7.2/html5shiv-printshiv.js"></script>

        <style>
            main {
                font-family: 'Century Gothic', CenturyGothic, AppleGothic, sans-serif;
                text-align: center;
            }
            #title {
                font-size: 120px;
                padding-top: 80px;
            }
            #tagline {
                font-size: 30px;
                padding-top: 180px;
                color: #747474;
            }
            .upper {
                background-color: #003366;
                color: #fff;
            }
            #login {
                width: 300px;
                margin: auto;
                margin-bottom: 60px;
            }
        </style>
    </head>

    <body>
        <main>
            <section class="upper">
                <div id="title">ZEUS</div>
                <?php
                if (isset($message)) {
                    echo '<div class="alert alert-success" role="alert" style="width: 300px; margin-left: auto; margin-right: auto; margin-bottom: 30px;">'.$message.'</div>';
                }
                if (isset($error)) {
                    echo '<div class="alert alert-danger" role="alert" style="width: 300px; margin-left: auto; margin-right: auto; margin-bottom: 30px;">'.$error.'</div>';
                }
                ?>
                <div id="login">
                    <form method="post" action="<?=basename(__FILE__)?>" autocomplete="off">
                        <div class="form-group">
                            <input type="text" class="form-control" id="user" name="user" autocomplete="off" placeholder="Username" style="text-align: center;">
                        </div>
                        <div class="form-group">
                            <input type="password" class="form-control" id="pass" name="pass" autocomplete="off" placeholder="Password" style="text-align: center;">
                        </div>
                        <?php
                            if (isset($_REQUEST['redirect'])) echo '<input type="hidden" name="redirect" value="'.$_REQUEST['redirect'].'">';
                        ?>
                        <button type="submit" class="btn btn-link" style="color: #fff;" id="login">Login</button>
                    </form>
                </div>
            </section>
            <section class="lower">
                <div id="tagline">Change the world, one thunder bolt at a time.</div>
            </section>
        </main>
    </body>

    <script type="text/javascript">
        // Some random javascript I wrote so I can show you I do that too!
        function save_agent(self) {
            var rowid = $(".agent.selectedrow").data('rowid');
            var agentid = $(".agent.selectedrow").data('agentid');
            var key = $(self).data('key');
            var value = $(self).val();

            $.ajax({
                dataType: "json",
                url: "ajax.php",
                data: { action: "saveagent", agentid: agentid, key: key, value: value },
                success: function(result) {
                    if (result.status == 0) {
                        if (key == "Relationship") {
                            agentdata[rowid].relationship = (value == 'N/A' ? null : value);
                        }
                        if (key == "Fish") {
                            agentdata[rowid].fish = value;
                        }
                        if (key == "Target") {
                            agentdata[rowid].target = (value == 'Yes' ? true : false);
                        }

                        showAgentStatus(agentdata[rowid]);
                    }
                }
            });
        }

        // I found this nifty way to check if a field has changed at all, this
        // includes copy/pate, text input, click, or drag drop. This allows for
        // fields on the page to be auto saved via ajax.
        // NOTE: This also works in IE 8+
        $("#gen-call-notes").bind("input propertychange", function (evt) {
            var self = this;
            // If it's the propertychange event, make sure it's the value that changed.
            if (window.event && event.type == "propertychange" && event.propertyName != "value")
                return;

            has_changed();
        });

        // So every time a value has been changed it calls this function to
        // start a timer (and restarts it if needed to reduce unnecessary ajax
        // calls to the server.
        function has_changed() {
            changed = true;

            $("#lastsave").html('<span style="color: #FF0000; font-weight: bold;">NOT SAVED</span>');

            clearTimeout(window.changetimer);
            window.changetimer = setTimeout(save_page, 1000);
        }

        // If the user has not changed any thing else on the page for the last
        // second, it sends all the data off to a ajax page to save in the
        // background.
        function save_page() {
            var fields = {};

            fields.dateofcall    = $("#date-of-call").val();
            fields.meatgreet     = $("#meatgreet").val();
            fields.meatgreetcost = $("#meatgreetcost").val();
            fields.gencallnotes  = $("#gen-call-notes").val();

            // I like writing out jquery ajax requests in this format since it
            // allows greater control over the ajax options.
            $.ajax({
                type: "post",
                dataType: "json",
                url: "ajax.php",
                data: { action: "save", fields: fields, agents: agentdata },
                success: function(result) {
                    if (result.status == 0) {
                        var d = new Date();
                        var txt = '';

                        // Date
                        txt += (d.getMonth()+1)+'/'+d.getDate()+'/'+d.getFullYear();
                        txt += ' ';

                        // Time
                        var hours = d.getHours();
                        var minutes = d.getMinutes();
                        var ampm = hours >= 12 ? 'pm' : 'am';
                        hours = hours % 12;
                        hours = hours ? hours : 12; // the hour '0' should be '12'
                        minutes = minutes < 10 ? '0'+minutes : minutes;
                        txt += hours + ':' + minutes + ' ' + ampm;

                        // Show date and time
                        $("#lastsave").html('<span style="color: #C8C8C8;">Last saved at '+txt+'</span>');

                        changed = false;
                    }
                }
            });
        }
    </script>

</html>
