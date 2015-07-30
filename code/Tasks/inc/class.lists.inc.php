<?php

/**
 * Handles list interactions within the app
 *
 * PHP version 5
 *
 * @author Jason Lengstorf
 * @author Chris Coyier
 * @copyright 2009 Chris Coyier and Jason Lengstorf
 * @license   http://www.opensource.org/licenses/mit-license.html  MIT License
 *
 */
class ColoredListsItems
{
    /**
     * The database object
     *
     * @var object
     */
    private $_db;

    /**
     * Checks for a database object and creates one if none is found
     *
     * @param object $db
     * @return void
     */
    public function __construct($db=NULL)
    {
        if(is_object($db))
        {
            $this->_db = $db;
        }
        else
        {
            $dsn = "mysql:host=".DB_HOST.";dbname=".DB_NAME;
            $this->_db = new PDO($dsn, DB_USER, DB_PASS);
        }
    }

    /**
     * Loads all list items associated with a user ID
     *
     * This function both outputs <li> tags with list items and returns an
     * array with the list ID, list URL, and the order number for a new item.
     *
     * @return array    an array containing list ID, list URL, and next order
     */
    public function loadListItemsByUser()
    {
        $sql = "SELECT
                    list_items.ListID, ListText, ListItemID, ListItemColor,
                    ListItemDone, ListURL
                FROM list_items
                LEFT JOIN lists
                USING (ListID)
                WHERE list_items.ListID=(
                    SELECT lists.ListID
                    FROM lists
                    WHERE lists.UserID=(
                        SELECT users.UserID
                        FROM users
                        WHERE users.Username=:user
                    )
                )
                ORDER BY ListItemPosition";
        if($stmt = $this->_db->prepare($sql))
        {
            $stmt->bindParam(':user', $_SESSION['Username'], PDO::PARAM_STR);
            $stmt->execute();
            $order = 0;
            while($row = $stmt->fetch())
            {
                $LID = $row['ListID'];
                $URL = $row['ListURL'];
                echo $this->formatListItems($row,   $order);
            }
            $stmt->closeCursor();

            // If there aren't any list items saved, no list ID is returned
            if(!isset($LID))
            {
                $sql = "SELECT ListID, ListURL
                        FROM lists
                        WHERE UserID = (
                            SELECT UserID
                            FROM users
                            WHERE Username=:user
                        )";
                if($stmt = $this->_db->prepare($sql))
                {
                    $stmt->bindParam(':user', $_SESSION['Username'], PDO::PARAM_STR);
                    $stmt->execute();
                    $row = $stmt->fetch();
                    $LID = $row['ListID'];
                    $URL = $row['ListURL'];
                    $stmt->closeCursor();
                }
            }
        }
        else
        {
            echo "tttt<li> Something went wrong. ", $db->errorInfo, "</li>n";
        }

        return array($LID, $URL, $order);
    }

    /**
     * Outputs all list items corresponding to a particular list ID
     *
     * @return void
     */
    public function loadListItemsByListId()
    {
        $sql = "SELECT ListText, ListItemID, ListItemColor, ListItemDone
                FROM list_items
                WHERE ListID=(
                    SELECT ListID
                    FROM lists
                    WHERE ListURL=:list
                )
                ORDER BY ListItemPosition";
        if($stmt = $this->_db->prepare($sql)) {
            $stmt->bindParam(':list', $_GET['list'], PDO::PARAM_STR);
            $stmt->execute();
            $order = 1;
            while($row = $stmt->fetch())
            {
                echo $this->formatListItems($row, $order);
                  $order;
            }
            $stmt->closeCursor();
        } else {
            echo "<li> Something went wrong. ", $db->error, "</li>";
        }
    }

    /**
     * Generates HTML markup for each list item
     *
     * @param array $row    an array of the current item's attributes
     * @param int $order    the position of the current list item
     * @return string       the formatted HTML string
     */
    private function formatListItems($row, $order)
    {
        $c = $this->getColorClass($row['ListItemColor']);
        if($row['ListItemDone']==1)
        {
            $d = '<img class="crossout" src="/assets/images/crossout.png" '
                . 'style="width: 100%; display: block;"/>';
        }
        else
        {
            $d = NULL;
        }

        // If not logged in, manually append the <span> tag to each item
        if(!isset($_SESSION['LoggedIn'])||$_SESSION['LoggedIn']!=1)
        {
            $ss = "<span>";
            $se = "</span>";
        }
        else
        {
            $ss = NULL;
            $se = NULL;
        }

        return "<li id=" . $row['ListItemID'] . " rel=" . $order . " "
            . "class=" . $c . " color=" . $row['ListItemColor'] . ">$ss"
            . htmlentities(strip_tags($row['ListText'])) . $d
            . "$se</li>";
    }

    /**
     * Returns the CSS class that determines color for the list item
     *
     * @param int $color    the color code of an item
     * @return string       the corresponding CSS class for the color code
     */
    private function getColorClass($color)
    {
        switch($color)
        {
            case 1:
                return 'color-blue';
            case 2:
                return 'color-yellow';
            case 3:
                return 'color-red';
            default:
                return 'color-green';
        }
    }

    /**
     * Adds a list item to the database
     *
     * @return mixed    ID of the new item on success, error message on failure
     */
    public function addListItem()
    {
        $list = $_POST['list'];
        $text = strip_tags(urldecode(trim($_POST['text'])), WHITELIST);
        $pos = $_POST['pos'];

        $sql = "INSERT INTO list_items
                    (ListID, ListText, ListItemPosition, ListItemColor)
                VALUES (:list, :text, :pos, 1)";
        try
        {
            $stmt = $this->_db->prepare($sql);
            $stmt->bindParam(':list', $list, PDO::PARAM_INT);
            $stmt->bindParam(':text', $text, PDO::PARAM_STR);
            $stmt->bindParam(':pos', $pos, PDO::PARAM_INT);
            $stmt->execute();
            $stmt->closeCursor();

            return $this->_db->lastInsertId();
        }
        catch(PDOException $e)
        {
            return $e->getMessage();
        }
    }

    /**
     * Changes the order of a list's items
     *
     * @return string    a message indicating the number of affected items
     */
    public function changeListItemPosition()
    {
        $listid = (int) $_POST['currentListID'];
        $startPos = (int) $_POST['startPos'];
        $currentPos = (int) $_POST['currentPos'];
        $direction = $_POST['direction'];

        if($direction == 'up')
        {
            /*
             * This query modifies all items with a position between the item's
             * original position and the position it was moved to. If the
             * change makes the item's position greater than the item's
             * starting position, then the query sets its position to the new
             * position. Otherwise, the position is simply incremented.
             */
            $sql = "UPDATE list_items
                    SET ListItemPosition=(
                        CASE
                            WHEN ListItemPosition 1>$startPos THEN $currentPos
                            ELSE ListItemPosition 1
                        END)
                    WHERE ListID=$listid
                    AND ListItemPosition BETWEEN $currentPos AND $startPos";
        }
        else
        {
            /*
             * Same as above, except item positions are decremented, and if the
             * item's changed position is less than the starting position, its
             * position is set to the new position.
             */
            $sql = "UPDATE list_items
                    SET ListItemPosition=(
                        CASE
                            WHEN ListItemPosition-1<$startPos THEN $currentPos
                            ELSE ListItemPosition-1
                        END)
                    WHERE ListID=$listid
                    AND ListItemPosition BETWEEN $startPos AND $currentPos";
        }

        $rows = $this->_db->exec($sql);
        echo "Query executed successfully. ",
            "Affected rows: $rows";
    }

    /**
     * Changes the color code of a list item
     *
     * @return mixed    returns TRUE on success, error message on failure
     */
    public function changeListItemColor()
    {
        $sql = "UPDATE list_items
                SET ListItemColor=:color
                WHERE ListItemID=:item
                LIMIT 1";
        try
        {
            $stmt = $this->_db->prepare($sql);
            $stmt->bindParam(':color', $_POST['color'], PDO::PARAM_INT);
            $stmt->bindParam(':item', $_POST['id'], PDO::PARAM_INT);
            $stmt->execute();
            $stmt->closeCursor();
            return TRUE;
        } catch(PDOException $e) {
            return $e->getMessage();
        }
    }

    /**
     * Updates the text for a list item
     *
     * @return string    Sanitized saved text on success, error message on fail
     */
    public function updateListItem()
    {
        $listItemID = $_POST["listItemID"];
        $newValue = strip_tags(urldecode(trim($_POST["value"])), WHITELIST);

        $sql = "UPDATE list_items
                SET ListText=:text
                WHERE ListItemID=:id
                LIMIT 1";
        if($stmt = $this->_db->prepare($sql)) {
            $stmt->bindParam(':text', $newValue, PDO::PARAM_STR);
            $stmt->bindParam(':id', $listItemID, PDO::PARAM_INT);
            $stmt->execute();
            $stmt->closeCursor();

            echo $newValue;
        } else {
            echo "Error saving, sorry about that!";
        }
    }

    /**
     * Changes the ListItemDone state of an item
     *
     * @return mixed    returns TRUE on success, error message on failure
     */
    public function toggleListItemDone()
    {
        $sql = "UPDATE list_items
                SET ListItemDone=:done
                WHERE ListItemID=:item
                LIMIT 1";
        try
        {
            $stmt = $this->_db->prepare($sql);
            $stmt->bindParam(':done', $_POST['done'], PDO::PARAM_INT);
            $stmt->bindParam(':item', $_POST['id'], PDO::PARAM_INT);
            $stmt->execute();
            $stmt->closeCursor();
            return TRUE;
        } catch(PDOException $e) {
            return $e->getMessage();
        }
    }

    /**
     * Removes a list item from the database
     *
     * @return string    message indicating success or failure
     */
    public function deleteListItem()
    {
        $list = $_POST['list'];
        $item = $_POST['id'];

        $sql = "DELETE FROM list_items
                WHERE ListItemID=:item
                AND ListID=:list
                LIMIT 1";
        try
        {
            $stmt = $this->_db->prepare($sql);
            $stmt->bindParam(':item', $item, PDO::PARAM_INT);
            $stmt->bindParam(':list', $list, PDO::PARAM_INT);
            $stmt->execute();
            $stmt->closeCursor();

            $sql = "UPDATE list_items
                    SET ListItemPosition=ListItemPosition-1
                    WHERE ListID=:list
                    AND ListItemPosition>:pos";
            try
            {
                $stmt = $this->_db->prepare($sql);
                $stmt->bindParam(':list', $list, PDO::PARAM_INT);
                $stmt->bindParam(':pos', $_POST['pos'], PDO::PARAM_INT);
                $stmt->execute();
                $stmt->closeCursor();
                return "Success!";
            }
            catch(PDOException $e)
            {
                return $e->getMessage();
            }
        }
        catch(Exception $e)
        {
            return $e->getMessage();
        }
    }

}

?>