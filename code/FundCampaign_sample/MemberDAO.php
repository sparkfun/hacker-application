<?php
namespace FundCampaign\app\components\Models;

/**
 * MemberDAO
 * @author William Barstad
 * @package FundCampaign\app\components\Models
 *
 * Notes:   -- Members are authenticated by email and password
 *
 * TODO:    Test all filter_var calls.
 *
 */
class MemberDAO extends DAO
{
    /**
     * @param $fcdsn - Database parameters
     */
    public function __construct($fcdsn)
    {
        parent::__construct($fcdsn);
    }

    /**
     * @method procMember
     * @param $em - email
     * @param $p1 - pwd 1
     * @param null $fn - first name
     * @param null $ln - last name
     * @param null $p2 - pwd 2
     * @return Member
     * Description:  Process member, decide by parameters whether to login or create new register. Too clever...?
     */
    public function procMember($em, $p1, $fn = NULL, $ln = NULL, $p2 = NULL)
    {
        // Login
        if (!$p2 && !$fn && !$ln) {
            try {
                return $this->Login($em, $p1);
            } catch (\Exception $e) {
                die($e->getMessage());
            }
        // New member registration
        } else {
            try {
                return $this->newRegistration($fn, $ln, $em, $p1, $p2);
            } catch (\Exception $e) {
                die($e->getMessage());
            }
        }
    }

    /**
     * @method checkMember
     * @param $em
     * @return bool
     * Description: Check for existence of member.
     */
    protected function checkMember($em)
    {
        if (filter_var(trim($em), FILTER_VALIDATE_EMAIL)) {
            $stmt = $this->da->prepare("SELECT email FROM users WHERE email = ?");
            $stmt->bind_param("s", $em);
            $stmt->execute();
            $stmt->close();

            return $stmt->num_rows == 1 ? true : false;
        } else {
            throw new \Exception("checkMember: email not valid.");
        }
    }

    /**
     * @method login
     * @param $em
     * @param $p
     * @return Member
     * @throws \InvalidArgumentException
     * @throws \Exception
     * Description: Authenticates passed params, registers and returns the member
     */
    protected function login($em, $p)
    {
        if ($p == filter_var($p, FILTER_SANITIZE_STRING)) {
            if (filter_var($em, FILTER_VALIDATE_EMAIL)) {

                //prepare the statment
                $stmt = $this->da->prepare("SELECT userid FROM users WHERE email = ? AND password=PASSWORD(?)");
                $stmt->bind_param("ss", $em, md5($p));

                //execute the statement and store the result for analysis
                $stmt->execute();
                $stmt->bind_result($m);
                $stmt->fetch();
                $stmt->close();

                if ($stmt->num_rows != 1) {
                    throw new \Exception("Login failed.");
                } else {
                    return $this->getMember($m);
                }
            }
        }
    }

    /**
     * @method newRegistration
     * @param $fn
     * @param $ln
     * @param $em
     * @param $p1
     * @param $p2
     * @return Member
     * @throws \Exception
     * Description: Register a new member.
     *
     * TODO: Add email confirmation function.
     */
    protected function newRegistration($fn, $ln, $em, $p1, $p2)
    {
        if (filter_var(trim($em), FILTER_VALIDATE_EMAIL) && $this->validPasswords(trim($p1), trim($p2))) {

            $fn = filter_var(trim($fn), FILTER_SANITIZE_STRING);
            $ln = filter_var(trim($ln), FILTER_SANITIZE_STRING);

            // TODO: Have the database generate userids.

            //prepare the statment
            $sql = "INSERT INTO users (firstname, lastname, email, password, registrationdate) ";
            $sql .= "VALUES (?, ?, ?, PASSWORD(?), NOW())";
            $stmt = $this->da->prepare($sql);
            $stmt->bind_param("ssss", $fn, $ln, $em, md5($p1));
            $stmt->execute();
            $stmt->close();

            if ($stmt->num_rows != 1) {
                throw new \Exception("Registration failed.");
            } else {
                return $this->getMemberByEmail($em);
            }
        }
    }

    /**
     * @method getMember
     * @param int $m
     * @return Member
     * @throws \Exception
     * Description: Returns a Member object for the passed memberID
     */
    public function getMember($m)
    {
        if (filter_var($m, FILTER_VALIDATE_INT)) {

            //prepare the statement
            $stmt = $this->da->prepare("SELECT username, firstname, lastname, email FROM users WHERE userid = ?");
            $stmt->bind_param("i", $m);

            //execute the statement and bind the results into local variables
            $stmt->execute();
            $stmt->bind_result($u, $fn, $ln, $em);
            $stmt->fetch();
            $stmt->close();

            //if there exists one and only one member with the passed userid, create and populate a new Member object
            if ($stmt->num_rows == 1) {
                $member = new Member($m);
                if ($member) {
                    $member->setUsername($u);
                    $member->setFirstName($fn);
                    $member->setLastName($ln);
                    $member->setEmail($em);
                    return $member;
                } else {
                    throw new \Exception("MemberDAO: Member creation failed.");
                }
            } else {
                throw new \Exception("MemberDAO: There was an internal error (172).");
            }
        }
    }

    public function getMemberByEmail($em)
    {
        if (filter_var(trim($em), FILTER_VALIDATE_EMAIL)) {

            // Prepare the statement.
            $stmt = $this->da->prepare("SELECT userid FROM users WHERE email = ?");
            $stmt->bind_param("s", $em);

            // Execute the statement and bind the results into local variables.
            $stmt->execute();
            $stmt->bind_result($m);
            $stmt->fetch();
            $stmt->close();

            // If there exists one and only one member with the passed memberID call getMember.
            if ($stmt->num_rows == 1) {
                return $this->getMember($m);
            } else if ($stmt->num_rows == 0) {
                throw new \Exception("User does not exist.");
            } else {
                throw new \Exception("MemberDAO: There was an internal error (194).");
            }
        }
    }

    // TODO: Create getMembers returning an array of member objects based on an array of userids.

    /**
     * @method memberForgotPwd
     * @param $em
     * @return mixed
     * @throws \Exception
     * Description: Generate temporary password and send it to user.
     *
     * TODO: Workflow note -- If user does not exist, offer new registration to the user.
     */
    public function memberForgotPwd($em)
    {
        // Check for member in the database.
        if ($this->checkMember($em)) {

            // Generate temporary password.
            $temppass = substr(uniqid(rand(), 1), 3, 10);

            // Update database with temp password.
            $stmt = $this->da->prepare("UPDATE users SET password=PASSWORD(?) WHERE email=?");
            $stmt->bind_param("ss", md5($temppass), $em);
            $stmt->execute();
            $stmt->close();



            // Send temp password to user via email.
            $body = "At your request, your forgotten password has been removed";
            $body .= " and this temporary password has been generated: " . $temppass . "\n";
            $body .= "Log in to <a href='http://fundcampaign.com'>FundCampaign.com</a> to change your password.";

            mail($em, 'FC Account', $body, 'From: cp@fundcampaign.com');

            // Notify Member
            // TODO: Move changed password notification to presentation layer?

            echo "<h3>Your password has been changed. Soon you will receive a temporary password by email.";
            echo "Login with the temporary password, then change your password using the \"Change Password\" function.</h3>";

        } else {
            throw new \Exception("User does not exist");
        }
    }

    /**
     * @method validPasswords
     * @param $p1 - first pwd field
     * @param $p2 - second pwd field
     * @return bool
     * Description: Check and match passwords for new registration.
     */
    private function validPasswords($p1, $p2)
    {
        // TODO: validPasswords requires testing.
        // TODO: Allow passwords to contain special chars

        $err = 'Please enter an alphanumeric value for each password field between 4-20 characters.';

        if (($p1 != filter_var($p1, FILTER_SANITIZE_STRING)) || ($p2 != filter_var($p2, FILTER_SANITIZE_STRING))) {
            throw new \InvalidArgumentException($err);
        } else if (!preg_match("/^[a-zA-Z0-9]{4,20}$/", $p1) || !preg_match("/^[a-zA-Z0-9]{4,20}$/", $p2)) {
            throw new \InvalidArgumentException($err);
        } else if ($p1 != $p2) {
            throw new \InvalidArgumentException('Passwords do not match.');
        } else {
            return true;
        }
    }
}