<%@ Page Debug="False" EnableViewState="False" Inherits="ChrisEdwardsGroup.Website.PhotoDetails" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<html>
<head>
	<title>MLS#<asp:Literal id="ltlMLS" runat="server" /> Enlarged Photo and Description</title>
	
	<link rel="stylesheet" href="/includes/listings.css" type="text/css">
	<script type="text/javascript">
    
      var _gaq = _gaq || [];
      _gaq.push(['_setAccount', 'UA-2016415-2']);
      _gaq.push(['_trackPageview']);
    
      (function() {
        var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
        ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
      })();
    
    </script>
</head>

<body>

<asp:Label id="lblOutError" Visible="False" runat="server" />

<div>
	
	<asp:Image id="imgPicturePathFull" runat="server" />
	
	<h3 class="photocomhead">
		Photo Comments:
	</h3>
	
	<p class="photocomments">
		<asp:Label id="lblPictureComments" runat="server" />
	</p>
	
	<p class="closewindow">
		<a href="" onClick="window.close();">Close Window</a>
	</p>

</div>

</body>
</html>
