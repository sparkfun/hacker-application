<?php
session_start();

require "controller.php";
$controller = new Controller();
$locations = $controller->showLocations();
$locationData = pg_fetch_all($locations);

?>
<!DOCTYPE html>
<html>
    <head>
        <meta charset="utf-8"></meta>
        <meta name="viewport", content="width=device-width, initial-scale=1.0"></meta>
        <meta http-equiv="X-UA-Compatible" content="IE=edge"></meta>
        <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" integrity="sha384-wvfXpqpZZVQGK6TAh5PVlGOfQNHSoD2xbE+QkPxCAFlNEevoEH3Sl0sibVcOQVnN" crossorigin="anonymous">
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/normalize/5.0.0/normalize.min.css" type="text/css" >
        <link rel="stylesheet" href="./styles.css" type="text/css">
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
        <link href="https://fonts.googleapis.com/css?family=Rubik" rel="stylesheet">
        <title>PHP Weather App</title>
    </head>
    <body>
        <div class="mainContainer">
            <nav class="nav">
                <i class="fa fa-cloud" aria-hidden="true"></i>
                <h1 class="formItem largeText">Weather App</h1>
                <form action="requestHandler.php" method="post">
                    <input type="text" name="address" placeholder="Enter City, State"/>
                    <input class="btn" type="submit" value="Get Conditions"/>
                </form>
            </nav>
            <div class="mainContent">
                <div id="locationStream" class="locationStream">
                    <?php if(!empty($locationData)) : ?>
                        <p class="smallText">Click to see current weather</p>
                        <?php
                        foreach($locationData as $prop):
                        ?>
                            <div class="locationItem">
                                <a href="requestHandler.php?findById=<?php echo $prop["id"] ?>">
                                    <p class="smallText"><?php echo $prop["city"] . "," . $prop["state"] ?></p>
                                    <a href="requestHandler.php?remove=true&item=<?php echo $prop["id"] ?>">
                                        <i class="fa fa-times delete-item" aria-hidden="true"></i>
                                    </a>
                                    <input type="hidden" name="id" id="id" value="<?php echo $prop["id"] ?>" />
                                </a>
                            </div>
                        <?php endforeach; ?>
                    <?php else : ?>
                        <p class="smallText">No Locations Yet</p>
                    <?php endif; ?>
                </div>
                <div class="locationDetails">
                    <?php if(isset($_SESSION["conditions"])) : ?>
                        <p class="smallText">Current Conditions</p>
                        <div class="conditons">
                            <p class="mediumText"><?php echo "Current weather for: " . $_SESSION["conditions"]["city"] . ", " . $_SESSION["conditions"]["state"]?></p>
                            <p class="mediumText"><?php echo "Current Temp: " . $_SESSION["conditions"]["conditions"]->apparentTemperature ?></p>
                            <p class="mediumText"><?php echo "Dew Point: " . $_SESSION["conditions"]["conditions"]->dewPoint ?></p>
                            <p class="mediumText"><?php echo "Pressure: " . $_SESSION["conditions"]["conditions"]->pressure ?></p>
                            <p class="mediumText"><?php echo "Wind Speed: " . $_SESSION["conditions"]["conditions"]->windSpeed ?></p>
                        </div>
                    <?php else : ?>
                        <p class="smallText">No Location Specified</p>
                    <?php endif; ?>
                </div>
            </div>
        </div>
    </body>
</html>