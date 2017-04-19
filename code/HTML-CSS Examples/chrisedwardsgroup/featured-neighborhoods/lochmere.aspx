<%@ Register Tagprefix="Top" Tagname="FeaturedProperty" Src="/includes/featured.ascx" %>
<%@ Register Tagprefix="Top" Tagname="TopLinks" Src="/includes/toplinks.ascx" %>
<%@ Register Tagprefix="Top" Tagname="LeftLinks" Src="/includes/leftlinks.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Address" Src="/includes/address.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Neighborhoods" Src="/includes/neighborhoods.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Logos" Src="/includes/logos.ascx" %>
<%@ Register Tagprefix="Bottom" Tagname="Footer" Src="/includes/footer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
	"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
	<title>Lochmere Neighborhood of Cary, North Carolina - Chris Edwards RE/MAX United</title>
	
	<meta name="Description" content="Description of the Lochmere neighborhood in Raleigh, North Carolina.">

	<meta name="Keywords" content="Raleigh, Lochmere, North Carolina">

	<meta name="Robots" content="all">

	<link rel="shortcut icon" type="image/ico" href="/EdwardsIcon.ico" />
	<link rel="stylesheet" href="/includes/ce.css" type="text/css">
	<link rel="stylesheet" href="/includes/forms.css" type="text/css">
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

<!-- start header and toplinks -->
<Top:TopLinks id="ctlTopLinks" runat="server" />
<!-- end header and toplinks -->
	
	<tr>
		
		<!-- start left link column -->
		<Top:LeftLinks id="ctlLeftLinks" runat="server" />
		<!-- end left link column -->
		
		<!-- start right column -->
		<td id="right">
			
			<!-- start flash & featured listing -->
				<table border="0">
					<tr>
					
						<!-- start flash or photo -->
						<td><img src="/images/left-pix/lochmere-club-sign.jpg" alt="" width="290" height="210" border="0"><img src="/images/right-pix/rt-tours.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
						<!-- end flash or photo -->
						
						<!-- featured listing -->
						<td>
							
							<Top:FeaturedProperty id="featProperty" runat="server" />
						
						</td>
						<!-- end listing -->
						
					</tr>
				</table>
				
			<!-- end flash & featured listing -->
			
			<!-- start text -->
			
				<h1>Lochmere Neighborhood Profile</h1>
						
						<table border="0" class="faq" summary="Lochmere Neighborhood">
							<tr>
				   				 <td>
									City: Cary, North Carolina
								</td>
							</tr>
							<tr>
								<td>
									Address: Kildaire Farm Road &amp; Lochmere Drive
								</td>
							</tr>
						</table>
							
						<h2>Neighborhood Facts:</h2>
						
						<ul class="whylist">
							<li class="why">
								Age range is 1980's-1990's
							</li>
							<li class="why">
								Price range is $400,000 - 1 million
							</li>
							<li class="why">
								Prime Location in East Cary!
							</li>
						</ul>
						
						<p>How would you like to have a home in a park?</p> 
						
						<p>That's pretty much the feel that you'll find in Lochmere. This planned community is easily one of Cary's most popular neighborhoods, and for good reason. The mature landscape featuring lots of trees and foliage throughout provides a natural setting laced with walking trails, community pool, lake, tennis &amp; volleyball courts and even a bike lane along the neighborhood roadside. If you like to be active, this is the place!</p> 
						
						<p>You'll find a wide variety of homes from a townhome in Glen Echo in the high $100's to an awesome lakefront home hitting the seven-figure mark! For the most part, you'll find the prices in the $300's and $400's for homes built in the mid 1980's to early 1990's. The one challenge I find that people have with Lochmere is that many of the homes are dated and do not have the open floorplan of more modern homes.</p>
						
						<p>One of the other great features is Lochmere's prime location in east Cary. Three premium grocery stores, including a Whole Foods, lots of restaurant choices and Western Wake Hospital are just a few of the close amenities! If you at photo of the sign above you'll see a nice shopping area off in the distance, which features gourmet coffee &amp; wine shops along with a restaurant.</p>

						<p>See the other profile for <a href="lochmere-highlands.aspx">Lochmere Highlands</a>. There are some slight differences in neighborhoods.</p>

						<p>Lochmere also features a private golf course and country club. Click the link below for more information.</p>

						<p><a href="http://www.lochmere.com/" target="_blank">http://www.lochmere.com/</a></p>
						
						<h2>Please feel free to <a href="/contact.aspx">contact</a> Chris for more information about the sale of a home in a particular area or subdivision.</h2>
				
				<!-- end text -->
			
				<!-- start address -->
				<Middle:Address id="ctlAddress" runat="server" />
				<!-- end address -->
				
				<!-- start neighborhoods -->
				<Middle:Neighborhoods id="ctlNeighborhoods" runat="server" />
				<!-- end neighborhoods -->
				
		</td>
		<!-- end right column -->
	
	</tr>
</table>

<!-- start logos -->
<Middle:Logos id="ctlLogos" runat="server" />
<!-- end logos -->

<!-- start footer -->
<Bottom:Footer id="ctlFooter" runat="server" />
<!-- end footer -->

</body>
</html>
