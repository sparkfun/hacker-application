 <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
        <title>Have You Been Hit by a Double Whammy?</title>
        <script
            src="https://code.jquery.com/jquery-3.1.1.min.js"
            integrity="sha256-hVVnYaiADRTO2PzUGmuLJr8BLUSjGIZsDYGmIJLv2b8="
            crossorigin="anonymous"></script>
        <script type="text/javascript" src="includes/main.js"></script>
        <script type="text/javascript" src="includes/toolbarLib.js"></script>
        <script type="text/javascript" src="includes/symptomLib.js"></script>
        <script
          src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"
          integrity="sha256-VazP97ZCwtekAsvgPBSUwPFKdrwD3unUfSGVYrahUqU="
          crossorigin="anonymous"></script>
        
        <!-- Adding SDK to this view as well, you may want it for the iframe sizing (below) -->
        <script src="//connect.facebook.net/en_US/sdk.js"></script>
        <link href='//fonts.googleapis.com/css?family=Open+Sans' rel='stylesheet' type='text/css'>
        <link href='//fonts.googleapis.com/css?family=Shadows+Into+Light+Two' rel='stylesheet' type='text/css'>
        <link href="includes/styles.css" rel="stylesheet" type="text/css" /> 
    </head>

<?php
$url = "https://yourURLHere.com";
//Browser detection - stops IE under version 9 from proceeding
if(preg_match('/(?i)msie [1-8]/',$_SERVER['HTTP_USER_AGENT'])) {
    // if IE<=8
    echo "We're sorry but your browser version is not supported. Please upgrade or try a different version.";
	exit;
}

/*
 * Writes image to filesystem, add your own DB call here if you wish to save to a database
 */
if ($_POST['savetogallery']) {
	
	$imagedata = $_REQUEST['urldata'];
	
	// you can use the PNG data returnd here 
	// to save to filesystem, save to database and share on social media.
	// For example: 
	/*
	$imagedata = str_replace('data:image/jpeg;base64,', '', $imagedata);
	$imagedata = base64_decode($imagedata);
	
	$im = imagecreatefromstring($imagedata);
	$timestamp = time();
	imagesavealpha($im, true);
	if ($im !== false) {
		$newfile = "creations/avatar" . $timestamp . ".jpg";
		$dbname = "avatar".$timestamp.".jpg";
		$fhandler = fopen($newfile, 'w+'); //create if not exists, truncate to 0 length
		$write = fwrite($fhandler, $imagedata); //write image data
		$close = fclose($fhandler); //close stream
		imagedestroy($im);
	}
	// end save image to filesystem
	
	// Add to database
	// Your database logic here :) 
	*/
?>
<body>
    <div id="frame">
    <h1>Ready to Share Your Double Whammy Avatar</h1>
    <div id="finalize">
            
            <span id="fb-publish"><img src="images/sharefb.jpg" style="cursor:pointer" border="0" /></span>
            
            <script type="text/javascript">
            (function() {
                // Need to add the version here, due to the new SDK. See https://developers.facebook.com/docs/apps/upgrading/
                FB.init({
                    appId: 'YourAppId', cookie: true, status: true, xfbml: true, oauth: true, version : 'v2.1'
                });
                var fbShare = function() {
                    FB.ui({
                        method: "feed",
                        display: "iframe",
                        link: "<?php echo($url)?>",
                        caption: "<?php echo($url)?>",
                        description: "Psoriatic Arthritis (PsA) can feel like a double whammy.  Help raise awareness of PsA by creating a Double Whammy avatar.",
                        picture: "<?php echo($url)?>/creations<? echo($newfile)?>"
                    });
                };
                $("#fb-publish").click(function() {
                    // Share UI does not require login here, it can handle it on it's own
                    fbShare();                
                });
            })();
            </script>
            
            
        </div>
        <div class="clear"></div>
   		<p>Sample of image/png base 64 data that is being generated.  <br />
        This can be output as an image file, saved in the database and/or shared on social media: </p>
        <textarea cols="100" rows="25"><?php echo($imagedata)?></textarea>
        
        <p><a href="index.php"><img src="images/start-over.jpg" style="cursor:pointer" border="0" /></a></p>

         
    </div>
    </body>
    </html>
    <?php
        exit();
    } else {
    ?>
    
    <script type="text/javascript">
    var objects = new Array();
    var symptom_icons = new Array();
    
    $( document ).ready(function() {
        init();
    });
    
    
    // This can be included to manually set the height of the iframe on facebook. 
	// Facebook is weird about sizing things, so this is needed more often than not to solve the scroll bar issue
    FB.init({        
        appId: 'YourAppIdHere',  
        version    : 'v2.1',
        status: true, // check login status
        cookie: true, // enable cookies to allow the server to access the session
        xfbml: true // parse XFBML
    });
    FB.Canvas.setSize({ height:784 });
    
    </script>
    
    <body>
    
    <div id="frame">
    
    <img src="images/PSAcallout.png" align="left" />
    <a href="https://www.facebook.com/video.php?v=752955758071122&set=vb.367781396588562&type=3&theater" target="_blank">
    	<img src="images/see-tv-spot.png" align="right"  border="0" />
    </a>
    <div class="clear"></div>
    <h1>Have You or Someone You Know Been Hit by a Double Whammy?</h1>
    <h2>Create an Avatar and Help Raise Awareness for Psoriatic Arthritis (PsA)</h2>
    <div id="_symptoms">
        <p class="orange">Choose your Double Whammy</p>
        <p class="instructions">PsA is a double whammy, like getting struck by lightning then hit with a ton of bricks. <strong>DRAG</strong> the whammies below to show where skin symptoms and joint pain affect you. </p>
        
        <div id="symptoms"></div>
    </div>
    
    <img id="figure" src="images/doublewhammy.jpg" style="display:none" />
    <div id="container">
    <div id="droppable1" class="droppable"></div>
        <div id="droppable2" class="droppable"></div>
        <div id="droppable3" class="droppable"></div>
        <div id="droppable4" class="droppable"></div>
        <div id="droppable5" class="droppable"></div>
        <div id="droppable11" class="droppable"></div>
        <div id="droppable12" class="droppable"></div>
        <div id="droppable13" class="droppable"></div>
        <div id="droppable14" class="droppable"></div>
        <div id="droppable15" class="droppable"></div>
        
        <div id="_canvas">
            <canvas id="canvas"></canvas>
        </div>
    </div>
    
    <div id="toolbar"><p class="orange">Accessorize Your Avatar</p>
    <p class="instructions"><strong>CLICK</strong> the accessories below to add your personality and style<br /> to your avatar.</p>
    <div class="spacer"></div>
    </div>
    
    <div id="finalize">
        <p class="orange">You're almost ready to share!</p>
        <p>Click here to finalize and share your Double Whammy avatar </p>
        
        <form method="post" action="" onsubmit="return mergeCanvas()">
        <input type="hidden" name="urldata" id="urldata" />
        <input type="hidden" name="savetogallery" value="1" />
        <input type="submit" id="submit" value="Save and Share" onclick="mergeCanvas()" />
        
        </form>
      </div>
    </div>
    
    <div class="clear"></div>
    
    <img id="sample" style="display:none" />
    <?php } ?>
</body>
</html>