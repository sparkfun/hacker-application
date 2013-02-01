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
	<title>Wake Forest, North Carolina City Profile.</title>
	
	<meta name="Description" content="Wake Forest, North Carolina City Profile.">

	<meta name="Keywords" content="Wake Forest, North Carolina, Chris Edwards, RE/MAX United">

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
						<td><img src="/images/left-pix/wake-forest-sign.jpg" alt="" width="290" height="210" border="0"><img src="/images/right-pix/rt-tours.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1 class="body">Wake Forest, North Carolina Profile</h1>
				
					<p>Welcome to the historic and charming town of Wake Forest, North Carolina! The recent completion of the 540 outer loop has opened up new doors to this nice town by providing much better accessibility and convenience!</p>

					<p>Along with the addition of the 540 loop came some of the Triangle's most popular, planned communities, Wakefield Plantation & Heritage at Wake Forest! Wakefield Plantation is an exceptional all-inclusive 2,200-acre community with breathtaking natural beauty. Residents can enjoy an active lifestyle with world-class golf plus tennis and swimming, playgrounds and clubhouse activities for year-round enjoyment. Visit our single-family homes, townhomes, villas, and custom designed estates. Experience the impressive fairways and stunning beauty of our renowned private golf course. With so much to offer, Wakefield Plantation is the perfect place to live, work and play!</p>
					
					<p>Heritage at Wake Forest also provides similar amentias and even more new, housing options since the development is not as established as Wakefield Plantation.</p>
					
					<p>Outside of these larger, planned communities, there are many other custom home neighborhoods in Wake Forest that provide larger lots and more privacy than other surrounding areas.</p>
					
					<p>The one challenge that still remains in Wake Forest is getting in and out of the area. Even though 540 has made it somewhat more accessible, dealing with highway 1 and 1A can still be challenging at many times of the day outside of just rush hour.</p>
					
					<p>Many folks think that Wake Forest University is located in Wake Forest, NC, when in actuality it moved in 1957 to the town of Winston-Salem North Carolina. Southeastern Baptist Seminary now occupies the beautiful campus where the college used to sit.</p>

				<p>This link has all of the demo information</p>
				
				<p><a href="http://www.wakeforestchamber.org/images/special/demographics/demo_inc_profile.pdf" target="_blank">http://www.wakeforestchamber.org/images/special/demographics/demo_inc_profile.pdf</a></p>

				<p class="center">
					<a href="http://www.agentpreview.com/agents/united-states/north-carolina/wake-forest.htm" target="_blank"><img src="/images/120x60view_my_profile.gif" alt="Wake Forest Real Estate Agent." width="120" height="60" border="0" /></a>
				</p>
				
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
