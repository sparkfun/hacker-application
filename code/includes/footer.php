
	</div><!-- Close .container -->
<?php
	if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id') {
		echo "<footer>";
	} else {
		echo "<footer class='beta'>";
	}
?>
		<p class='footerLinks'><ul>
			<li><a href='/index'>Home</a></li>
			<li><a href='/about'>About</a></li>
			<li><a href='/terms'>Terms</a></li>
			<li><a href='/privacy'>Privacy</a></li>
			<li><a href='/developers'>Developers</a></li>
			<li><a href='/careers'>Careers</a></li>
			<li><a href='/contact'>Contact</a></li>
		</ul></p>
		<p>Copyright &copy; 2013. <a href="http://www.hirestarts.com">HireStarts, LLC.</a> All Rights Reserved.</p>
	</footer>
</div><!-- Close #wrapper -->

<!-- Javascripts -->
<script type='text/javascript' src='https://ajax.googleapis.com/ajax/libs/jqueryui/1/jquery-ui.min.js'></script>
<script type='text/javascript' src='/libs/tinymce/jscripts/tiny_mce/tiny_mce.js'></script>
<script type='text/javascript' src='/js/script.js'></script> 
<script type='text/javascript' src='/js/bootstrap.js'></script> 
<script type='text/javascript' src='/js/html5shiv.js'></script>
<script type='text/javascript' src='/js/jquery.masonry.js'></script> 
<script type='text/javascript' src='/js/tags.js'></script>
 
	<?php
	if(!(isset($_SESSION['profile']))) {
	?>
		<script type='text/javascript'>
				$(window).load(function(){
					$('#profileComplete').modal('show');
				});	
		</script>
	<?php
	}	
	?>
<script type="text/javascript">

$(document).ready(function() {
	$('#photoInput').hide();
});
$('#newPhoto').mousedown(function() {
	if (!$(this).is(':checked')) {
		$('#photoInput').show();
		$(this).trigger("change");
	} else {
		$('#photoInput').hide();
	}
});



	


$(window).load(function() {
	//init tinymce
	tinyMCE.init({
		theme : "advanced",
		mode: "exact",
		elements : "format",
		theme_advanced_resizing : true,
		content_css : "styles/styles.css",
		theme_advanced_toolbar_location : "top",
		theme_advanced_buttons1 : "bold,italic,underline,strikethrough,separator,"
		+ "justifyleft,justifycenter,justifyright,justifyfull,formatselect,"
		+ "bullist,numlist,outdent,indent",
		theme_advanced_buttons2 : "link,unlink,anchor,image,separator,"
		+"undo,redo,cleanup,code,separator,sub,sup,charmap",
		theme_advanced_buttons3 : ""
	});
	
	//enable tags on skills
	$('#skills').inputosaurus({
		width : '350px',
		autoCompleteSource : '',
		change : function(ev){
			$('#skills').val(ev.target.value);
		}
	});
	//enable tags on interests
	$('#interests').inputosaurus({
		width : '350px',
		autoCompleteSource : '',
		change : function(ev){
			$('#interests').val(ev.target.value);
		}
	});
	//enable tags on keywords
	$('#keywords').inputosaurus({
		width : '350px',
		autoCompleteSource : '',
		change : function(ev){
			$('#keywords').val(ev.target.value);
		}
	});

	var loading = false;
	var $container = $('.blockContainer'); 
			
	$container.imagesLoaded( function(){
		$container.masonry({
			itemSelector : '.masonryBlock',
			isFitWidth: true,
			isAnimated: true,   
			isResizable: true,
		});
	});
});

$(document).ready(function () {
	//init tooltips
	$("[rel=tooltip]").tooltip({
		placement: 'bottom',
	});
	$("[rel=tooltip-left]").tooltip({
		placement: 'left',
	});
	
	//init dropdown links
   $('.dropdown-menu li a').on('click', function() {
		window.location = $(this).attr("href");
	});
	
	supports_input_placeholder();
	
});

// detects if the "placeholder" attribute is available and enforces it if it is
function supports_input_placeholder() {
    var i = document.createElement('input');
    return 'placeholder' in i;
}

if(!supports_input_placeholder()) {
    var fields = document.getElementsByTagName('INPUT');
    for(var i=0; i < fields.length; i++) {
		if(fields[i].hasAttribute('placeholder')) {
			fields[i].defaultValue = fields[i].getAttribute('placeholder');
			fields[i].onfocus = function() { if(this.value == this.defaultValue) this.value = ''; }
			fields[i].onblur = function() { if(this.value == '') this.value = this.defaultValue; }
		}
    }
}

</script>

</body>

</html>