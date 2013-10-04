<!DOCTYPE html>

<html>

<head>
<meta charset="UTF-8">
<meta name="viewport" content="initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=0" />
<title>Jenny Ferenc</title>

<?php

// scan pics directory for pictures

$pics = array();
$pic_types = array('jpg', 'jpeg', 'gif', 'png');

//http://zaemis.blogspot.com/2012/07/php-recursive-directory-traversal.html
// added $filter param
function getDirectoryList($dir, $filter) {
    $dirList = array();
	$fileList = array();

    if ($dfp = opendir($dir)) {
        while (($entry = readdir($dfp)) !== false) {
            if ($entry[0] != ".") { // catches dot dirs and hidden files
                $path = "$dir/$entry";
                if (is_file($path)) {
					// only files that match filter
					if (in_array(substr(strtolower($entry), strrpos($entry,".") + 1), $filter)) {
                    	$fileList[] = $entry;
					}
                } elseif (is_dir($path)) {
                    $dirList[$entry] = getDirectoryList($path, $filter);
                }
            }
        }
        closedir($dfp);

		// shuffle for different first picture every time
        //uksort($dirList, "strnatcmp");
        //natsort($fileList);
		shuffle($fileList);
    }

    return $dirList + $fileList;
}

$pics = getDirectoryList('pics', $pic_types);

// first pic must be Default 0, JS assumes this
// images are resized based on url (1600 max width here)
$first_pic = '/pics/1600/Default/'.$pics['Default'][0];

?>

<style type="text/css" media="screen">
html {
	background: #000;
	background-repeat: no-repeat;
	background-position: center center;
	background-attachment: fixed;
	background-image: url('<?php echo $first_pic; ?>');
	-webkit-background-size: cover;
	-moz-background-size: cover;
	-o-background-size: cover;
	background-size: cover;
	color: #fff;
	font-family: "Gill Sans", Futura, Calibri, sans-serif;
	font-size: 100%;
	text-align: left;
	padding: 0;
	margin: 0;
	height: 100%;
	width: 100%;
}
body {
	width: 100%;
	height: 100%;
	padding: 0;
	margin: 0;
	position: absolute;
	top: 0;
	left: 0;
	right: 0;
	bottom: 0;
}
.image {
	opacity: 0;
	position: absolute;
	top: 0;
	left: 0;
	right: 0;
	bottom: 0;
	background-repeat: no-repeat;
	background-position: center center;
	background-attachment: fixed;
	-webkit-background-size: cover;
	-moz-background-size: cover;
	-o-background-size: cover;
	background-size: cover;
	-moz-transition: opacity 0.5s ease-in-out;
	-webkit-transition: opacity 0.5s ease-in-out;
	-o-transition: opacity 0.5s ease-in-out;
	transition: opacity 0.5s ease-in-out;
	/* force hardware acceleration */
	-moz-transform: translate3d(0, 0, 0);
	-webkit-transform: translate3d(0, 0, 0);
	-o-transform: translate3d(0, 0, 0);
	transform: translate3d(0, 0, 0);
	z-index: 2;
}
.image.start {
	opacity: 1;
}
#nav {
	margin: 1% 2%;
	padding: 0;
	position: relative;
	z-index: 10;
	overflow: auto;
}
.panel-wrap {
	position: relative;
	margin: 1% 2%;
}
.panel {
	position: absolute;
	top: 0;
	left: 0;
	z-index: 15;
	width: 36em;
	max-width: 100%;
	padding: 1.5em;
	-webkit-box-sizing: border-box; /* Safari/Chrome, other WebKit */
	-moz-box-sizing: border-box;    /* Firefox, other Gecko */
	box-sizing: border-box;         /* Opera/IE 8+ */
	background: rgb(40,40,40); /* for browsers that don't support rgba */
	background: rgba(40,40,40,0.8);
	color: #fff;
	border-radius: 5em;
	border-top-right-radius: 0.5em;
	border-bottom-left-radius: 0.5em;
	overflow: auto;
	box-shadow: 0.3em 0.3em 0.3em rgba(0,0,0,0.2);
}
#about .headshot {
	float: left;
	width: 35%;
}
#about .title, #about .links {
	width: 58%;
	float: right;
}
.headshot img {
	border-radius: 0.5em;
	border-top-left-radius: 4em;
}
.links ul, #keys ul {
	list-style-type: none;
	padding: 0;
	margin: 0;
}
.links li {
	width: auto;
	margin: 0 2% 2% 0;
	float: left;
}
.links a, .links span {
	color: #fff;
	background-color:#7af;
	padding: 0.2em;
	border-radius: 0.1em;
	display: block;
	width: auto;
}
.links a:hover, .links a:active, .links a:focus {
	background-color: #ce0;
}

#menu {
	padding: 3em;
	width: 46em;
}
#menu a {
	display: block;
	width: auto;
	max-width: 31%;
	float: left;
	background: #000;
	padding: 0.5em;
	margin: 0 0 2% 2%;
	-webkit-box-sizing: border-box; /* Safari/Chrome, other WebKit */
	-moz-box-sizing: border-box;    /* Firefox, other Gecko */
	box-sizing: border-box;         /* Opera/IE 8+ */
}
#menu a:hover, #menu a:active, #menu a:focus {
	background: #ce0;
}

#keys {
	padding: 3em;
	width: 24em;
}
#keys code {
	color: #ce0;
	font-weight: bold;
	display: block;
	width: 40%;
	float: left;
	text-align: right;
	margin-right: 1em;
	font-size: 1em;
	font-family: Courier, monospace;
}

#nav li {margin: 0 0.5em 0.5em 0;}
#nav a, #nav span {
	background: none;
	color: #fff;
	text-shadow: 0.1em 0.1em 0.2em #000;
	font-weight: bold;
}

#nav #caption {
	font-style: italic;
	font-weight: normal;
	padding-left: 2em;
}

h1 {
	font-size: 2em;
	line-height: 1.5em;
	margin: 0 0 0.8em 0;
	padding: 0;
	font-weight: bold;
}
p {
	line-height: 1.5em;
	margin: 0 0 1.75em 0;
	padding: 0;
}
a {
	color: #8bf;
	text-decoration: none;
	outline: none;
	-moz-transition: 0.3s;
	-ms-transition: 0.3s;
	-webkit-transition: 0.3s;
	-o-transition: 0.3s;
	transition: 0.3s;
}
a:hover, a:active, a:focus {
	color: #ce0;
}
img {
	border: 0;
	max-width: 100%;
	vertical-align: bottom;
}

.panel {
	-moz-transition: 0.3s;
	-ms-transition: 0.3s;
	-webkit-transition: 0.3s;
	-o-transition: 0.3s;
	transition: 0.3s;
	/* start hidden move off left */
	-moz-transform: translate3d(-110%, 0, 0);
	-webkit-transform: translate3d(-110%, 0, 0);
	-o-transform: translate3d(-110%, 0, 0);
	transform: translate3d(-110%, 0, 0);
}
.panel.show {
	-moz-transform: translate3d(0, 0, 0);
	-webkit-transform: translate3d(0, 0, 0);
	-o-transform: translate3d(0, 0, 0);
	transform: translate3d(0, 0, 0);
}

@media (min-width: 60em) {
	.panel-wrap {font-size: 1.25em;}
}

@media (max-width: 30em) {
	
	.panel-wrap {
		text-align: center;
		font-size: 1em;
	}
	h1 {font-size: 1.5em;}
	.panel {
		border-radius: 0.5em !important;
		padding: 1em !important;
	}
	#about .title {
		width: 100%;
		float: none;
		margin-bottom: 2em;
	}
	#about .headshot, #about .links {
		width: 50%;
		float: left;
	}
	#about .links {
		width: 45%;
		float: right;
		text-align: left;
	}
	#about .headshot img {border-radius: 0.5em;}
	#about .links li, #about .links li.kiva {
		display: block;
		width: 100%;
	}
}

</style>

<!--[if lt IE 10]>
<style type="text/css" media="screen">
/* old IE doesn't support transform translate -- boo */
.panel {
	display: none;
}
.panel.show {
	display: block;
}
</style>
<![endif]-->

</head>

<body>
	
<div id="nav" class="links">
<ul>
<li><a href="#about">About</a></li>
<li><a href="#menu">Menu</a></li>
<li><a href="#keys">Keys</a></li>
<li><span id="caption"></span></li>
</ul>
</div><!-- end #nav -->

<div class="panel-wrap">	

<div id="about" class="panel show">
	
<div class="title">
<h1>Jenny Ferenc</h1>
<p>Web Developer in Boulder, Colorado<br />
<a href="mailto:jenny.ferenc@gmail.com">jenny.ferenc@gmail.com</a></p>
</div>

<div class="headshot">
<img src="jenny-3.jpg" alt="Jenny Ferenc" />
</div>

<div class="links">
<ul>
<li><a href="http://www.facebook.com/jennyferenc"><img src="facebook-128.png" alt="Facebook" width="40" /></a></li>
<li><a href="https://plus.google.com/108871894618245590659?rel=author"><img src="googleplus-128.png" alt="Google+" width="40" /></a></li>
<li><a href="http://www.linkedin.com/in/jennyferenc"><img src="linkedin-128.png" alt="LinkedIn" width="40" /></a></li>
<li class="kiva"><a href="http://www.kiva.org/lender/jennyf"><img src="kiva-128.png" alt="Kiva" width="72" /></a></li>
</ul>
</div>

</div><!-- end #about -->

<div id="menu" class="panel">
<?php

foreach ($pics as $key => $val) {
	echo '<a href="#'.$key.'" data-dir="'.$key.'"><img src="/pics/160x160-c/'.$key.'/'.$val[0].'" /></a>';
}

?>
</div><!-- end #menu -->

<div id="keys" class="panel">
<ul>
<li><code>Right Arrow</code> Next picture</li>
<li><code>Left Arrow</code> Previous picture</li>
<li><code>Down Arrow</code> Next album</li>
<li><code>Up Arrow</code> Previous album</li>
<li><code>1 - 9</code> Album number</li>
<li><code>A</code> About page</li>
<li><code>M</code> Menu page</li>
<li><code>K</code> Keys page</li>
<li><code>H</code> Hide all pages</li>
<li><code>X</code> Start over</li>
</ul>
</div><!-- end #keys -->

</div><!-- end .panelwrap -->

<!-- Google Analytics -->
<script>
(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
(i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
})(window,document,'script','//www.google-analytics.com/analytics.js','ga');

ga('create', 'UA-20433710-1');
ga('send', 'pageview');

</script>
<!-- End Google Analytics -->

<script src="//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
<script src="/jquery.hammer.min.js"></script>
<script type="text/javascript">

var pics = <?php echo json_encode($pics) ?>;

function YPics(pics_arr) {

	// TODO: pass target elements on init instead of hard coding body/caption here
	this.body = $('body');
	this.caption = $('#caption');
	this.dirs_arr = [];
	this.pics_arr;
	this.dir_name = '';
	this.dir_id = 0;
	this.next_dir_id = 0;
	this.prev_dir_id = 0;
	this.pic_id = 0;
	this.next_pic_id = 0;
	this.prev_pic_id = 0;
	this.prev_img;
	this.cur_img;
	this.cur_panel = '';
	this.to;
	this.delay = 6000; // 60 second auto timer
	this.css_fade = 600; // 0.5s css transition plus slop to prevent overlapping css fade
	this.hammertime;

	var self = this;

	this.init = function() {
	
		self.pics_arr = pics_arr;
	
		// find all directory names
		for (i in self.pics_arr) {
			self.dirs_arr.push(i);
		}
		
		// start with first default pic, don't change to it since we're already there
		self.setDir('Default', true);
		
		var to_show = '#about';
		if (window.location.hash) {
			to_show = window.location.hash;
		}
		self.showPanel(to_show);
	
		// start auto transition timer
		self.to = window.setTimeout(self.nextPic, self.delay);
		
		$('#nav a').on('click', function() { return self.showPanel($(this).attr('href')); });
		$('#menu a').on('click', self.chooseDir);
		
		// init touch handler
		self.hammertime = self.body.hammer();
		
		// swipe/keyboard nav events
		self.enableSwipe();
		
		// preload next and prev images
		self.preloadNP();
	};
	
	this.enableSwipe = function() {
		// keyboard navigation events
		$(document).on('keydown', self.keydownCallback);
		// swipe navigation events
		self.hammertime.on("swipe", self.swipeCallback);
	};
	
	this.disableSwipe = function() {
		// keyboard navigation events
		$(document).off('keydown', self.keydownCallback);
		// swipe navigation events
		self.hammertime.off("swipe", self.swipeCallback);
	};
	
	// TODO: show/hide panel functionality should probably be a separate object from the picture stuff
	this.showPanel = function(id) {
		$('.panel').removeClass('show');
		if ($(id).length && self.cur_panel != id) {
			$(id).addClass('show');
			self.cur_panel = id;
		} else {
			self.cur_panel = '#hide';
		}
		window.location.hash = self.cur_panel;
		// trigger google analytics pageview
		ga('send', {
			'hitType': 'pageview',
			'page': '/' + self.cur_panel,
			'title': self.cur_panel
		});
		return false;
	};
	
	this.swipeCallback = function(e) {
		// prevent scroll
		e.gesture.preventDefault();
		if (e.gesture.direction == 'left') {
			self.nextPic();
		} else if (e.gesture.direction == 'right') {
			self.prevPic();
		} else if (e.gesture.direction == 'up') {
			self.nextDir();
		} else if (e.gesture.direction == 'down') {
			self.prevDir();
		}
		return false;
	};

	this.keydownCallback = function(e) {
		if (e.keyCode == 37) { // left arrow, next pic
	    	self.prevPic();
			return false;
	    } else if (e.keyCode == 39) { // right arrow, prev pic
			self.nextPic();
			return false;
		} else if (e.keyCode == 38) { // up arrow, prev directory
			self.prevDir();
			return false;
		} else if (e.keyCode == 40) { // down arrow, next directory
			self.nextDir();
			return false;
		} else if (e.keyCode == 88) { // x, return to default
			self.showPanel('#about');
			self.setDir('Default');
			return false;
		} else if (e.keyCode >= 49 && e.keyCode <= 57) { // 1-9, albums
	    	self.showPanel('#hide');
			self.setDir(e.keyCode - 49);
			return false;
		} else if (e.keyCode == 65) { // a, about
	    	self.showPanel('#about');
			return false;
		} else if (e.keyCode == 72) { // h, hide
	    	self.showPanel('#hide');
			return false;
		} else if (e.keyCode == 75) { // k, keys
	    	self.showPanel('#keys');
			return false;
		} else if (e.keyCode == 77) { // m, menu
	    	self.showPanel('#menu');
			return false;
		}
		return true;
	};
	
	this.chooseDir = function(e) {
		self.showPanel('#hide');
		self.setDir($(this).attr('data-dir'));
		return false;
	};

	this.nextDir = function() {
		self.setDir(self.dir_id + 1);
	};

	this.prevDir = function() {
		self.setDir(self.dir_id - 1);
	};

	this.nextPic = function() {
		self.setPic(self.pic_id + 1);
	};

	this.prevPic = function() {
		self.setPic(self.pic_id - 1);
	};
	
	this.setCaption = function() {
		var caption = '';
		if (self.dir_name != 'Default') {
			caption = self.dir_name.replace('_', ' ');
		}
		self.caption.html(caption);
	};

	this.setDir = function(id, noChange, pid) {
		if (id.substring && self.dirs_arr.indexOf(id) > -1) { // string, exists in array
			self.dir_name = id;
			self.dir_id = self.dirs_arr.indexOf(id);
		} else { // number array index
			if (id < 0) { // wrap to end
				id = self.dirs_arr.length - 1;
			} else if (id >= self.dirs_arr.length) { // wrap to beginning
				id = 0;
			}
			self.dir_name = self.dirs_arr[id];
			self.dir_id = id;
		}
		// Set next dir
		self.next_dir_id = self.dir_id + 1;
		if (self.next_dir_id >= self.dirs_arr.length) {
			self.next_dir_id = 0;
		}
		// Set prev dir
		self.prev_dir_id = self.dir_id - 1;
		if (self.prev_dir_id < 0) {
			self.prev_dir_id = self.dirs_arr.length - 1;
		}
		self.setCaption();
		if (pid == 'e') { // last pic in dir
			pid = self.pics_arr[self.dir_name].length - 1;
		} else if (pid !== parseInt(pid)) { // not integer
			pid = 0;
		}
		self.setPic(pid, noChange);
		return self.dir_name;
	};

	this.setPic = function(id, noChange) {
		if (!noChange) {
			// disable swipe, key shortcuts
			self.disableSwipe();
			// stop auto timer
			window.clearTimeout(self.to);
		}
		// check id is in bounds
		if (id !== parseInt(id)) { // not integer
			id = 0;
		} else if (id < 0) {
			//id = self.pics_arr[self.dir_name].length - 1;
			self.setDir(self.dir_id - 1, noChange, 'e');
			return false;
		} else if (id >= self.pics_arr[self.dir_name].length) {
			//id = 0;
			self.setDir(self.dir_id + 1, noChange, 0);
			return false;
		}
		// Set next pic
		self.next_pic_id = id + 1;
		if (self.next_pic_id >= self.pics_arr[self.dir_name].length) {
			self.next_pic_id = 0;
		}
		// Set prev pic
		self.prev_pic_id = id - 1;
		if (self.prev_pic_id < 0) {
			self.prev_pic_id = self.pics_arr[self.dir_name].length - 1;
		}
		// Finally change the pic
		self.pic_id = id;
		if (!noChange) {
			self.changeBg();
		}
		return self.pic_id;
	};

	this.picUrl = function(id, dir_id) {
		var dir_name;
		if (dir_id) {
			dir_name = self.dirs_arr[dir_id];
		} else {
			dir_name = self.dir_name
		}
		var url = '/pics/1600/' + dir_name + '/' + self.pics_arr[dir_name][id];
		return url;
	};

	this.preloadNP = function() {
		imgPreloadNext = new Image();
		imgPreloadNext.src = self.picUrl(self.next_pic_id);

		imgPreloadPrev = new Image();
		imgPreloadPrev.src = self.picUrl(self.prev_pic_id);
		
		imgPreloadNextD = new Image();
		imgPreloadNextD.src = self.picUrl(0, self.next_dir_id);

		imgPreloadPrevD = new Image();
		imgPreloadPrevD.src = self.picUrl(0, self.prev_dir_id);
	};
	
	this.picLoaded = function() {
		// remove last img
		if (self.prev_img) {
			self.prev_img.remove();
		}
		// now img transition done, restart auto timer
		self.to = window.setTimeout(self.nextPic, self.delay);
		// swipe/keyboard navigation events
		self.enableSwipe();
	};
	
	this.start = function() {
		self.cur_img.addClass('start'); // start transition
		self.to = window.setTimeout(self.picLoaded, self.css_fade); // css transition time
	};

	this.changeBg = function() {

		// preload background image before switching
		var imgPreload = new Image();

		imgPreload.onload = function () {
			self.prev_img = self.cur_img;
			// create new img container
			self.cur_img = $("<div></div>");
			self.cur_img.addClass('image');
			self.cur_img.css('background-image', 'url(' + this.src + ')');
			self.body.append(self.cur_img);
			self.to = window.setTimeout(self.start, 20); // delay or else css transition doesn't run
		};

		imgPreload.src = self.picUrl(self.pic_id);

		// preload next and prev images
		self.preloadNP();
	};

	this.init();

} // end YPics
	
$(document).ready(function() {
	var yp = new YPics(pics);
});

</script>

</body>

</html>