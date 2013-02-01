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
	<title>Cary, North Carolina City Profile.</title>
	
	<meta name="Description" content="Cary, North Carolina City Profile.">

	<meta name="Keywords" content="Cary, North Carolina, Chris Edwards, RE/MAX United">

	<meta name="Robots" content="all">

	<link rel="shortcut icon" type="image/ico" href="/EdwardsIcon.ico" />
	<link rel="stylesheet" href="/includes/ce.css" type="text/css">
	<link rel="stylesheet" href="/includes/forms.css" type="text/css">
	<script language="JavaScript1.2" src="/includes/rollover.js"></script>
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
						<td><img src="/images/left-pix/cary-sign.jpg" alt="" width="290" height="210" border="0"><img src="/images/right-pix/rt-tours.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1 class="body">Cary, North Carolina Profile</h1>
				
					<p>Many folks ask me what has led to Cary's tremendous popularity. There are several factors that have made Cary such a desirable place to live. First, is its overall quality of life due to the meticulous and clean environment. When you drive through Cary, you immediately notice the beautiful landscape along the roadside and throughout the city. You will not see billboards or junky, abandon strip malls. Because Cary has really just exploded in the last 10 years, the shopping and most of the neighborhoods are newer. Cary's strict zoning and building regulations keeps the quality of the area very high!</p>

					<a href="http://tours.tourfactory.com/tours/tour.asp?t=547134" target="_blank" onmouseover="imgOnOff('img1','/images/cary-vtROLL.jpg');" onmouseout="imgOnOff('img1','/images/cary-vt.jpg');"><img name="img1" src="/images/cary-vt.jpg" width="250" height="70" border="1" class="floatright2" /></a>
	
					<p>Cary's proximity to Raleigh, Research Triangle Park and lots of shopping make it a very convenient place to live. It's commitment to academics and high test scores also play a major role in its popularity!</p>
					
					<p>Keep in mind that because Cary has so much to offer, its home prices reflect that as well. Even though the prices are higher than it's neighboring communities like Apex &amp; Holly Springs, the value is there.</p>
					
					<p>Cary has led the area in growth, income, sales and many other areas. The average family income in Cary is the highest in the area, and they have leveraged that into an area with great schools, well-managed neighborhoods, and an orderly and well-planned layout.</p>
					
					<p>Cary has much to offer for entertainment, from its many golf courses to the Lazy Daze Arts and Crafts Festival, possibly the largest of its kind in the southeast.</p>
					
					<p>Cary was established in 1750, and became a stop on the railroad in the 1850's. Amtrak still maintains its station there today. The growth spurred by the railroad putting them 'on the map' helped Cary to add the amenities it needed, including a post office and a school. The community was absorbed into Wake County in 1871. The people who were managing Cary's growth envisioned it as a center for learning, and established Cary Academy, which became the state's first public school in 1907.</p> 
					
					<p>Cary Academy has since established a relationship with SAS Software, a major software company headquartered in Cary. The school boasts over 600 students in grades 6-12, and over 700 computers, making it a top choice for college prep-minded students.</p>
	
					<p>If you're looking for a vibrant, fast moving environment, then Cary is where you want to be. Cary is a residential and commercial center with over 110, 000 people, many of who work in the Raleigh/Durham/RTP area.</p>

					<p class="center">
						<a href="http://www.agentpreview.com/agents/united-states/north-carolina/cary.htm" target="_blank"><img src="/images/120x60view_my_profile.gif" alt="Cary Real Estate Agent." width="120" height="60" border="0" /></a>
					</p>

					<!-- removed
					
					<p class="text">Homes in Cary start at $120,000 in Park Village, and range up to $750,000 in neighborhoods such as Somerset.</p>

					-->

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
