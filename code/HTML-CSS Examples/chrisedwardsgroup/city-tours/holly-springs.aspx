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
	<title>Holly Springs, North Carolina City Profile.</title>
	
	<meta name="Description" content="Holly Springs, North Carolina City Profile.">

	<meta name="Keywords" content="Holly Springs, North Carolina, Chris Edwards, RE/MAX United">

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
						<td><img src="/images/left-pix/holly-springs_sign.jpg" alt="" width="290" height="210" border="0"><img src="/images/right-pix/rt-tours.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1 class="body">Holly Springs, North Carolina Profile</h1>
				
					<p>Nestled among Apex, Cary and Fuquay-Varina in Southwestern Wake County, Holly Springs offers a small-town atmosphere while balancing projected future growth. More and more Holly Springs is having some of the same growth and quality characteristics that Cary did when it started it's big growth streak in the 1990's.</p>

					<p>Holly Springs offers a beautiful living environment that's becoming more convenient all the time! Roadwork continues to make the area more accessible to daily commuters, including the new 55 bypass, widening of highway 1 and future extension of the 540!</p>
					
					<p>The real attraction that buyers have found in Holly Springs is that they enjoy  a similar quality of life to Cary but for less money. Many people have felt that a few minutes more in commute has been well worth it!</p>
					
					<p>While the Town welcomes growth, leaders also are determined to control the quality and placement of new developments while preserving open space and creating public areas. One of the recent focuses has been on encouraging commercial development in downtown Holly Springs.</p>
					
					<p>&quot;We're looking at downtown as a center for development of places that will be destination points, places that will generate traffic to draw people here,&quot; said Holly Springs Town Manager Carl Dean. &quot;We want to make it a place where people want to come and shop.&quot;</p>
					 
					<p>Part of ensuring a successful downtown was building Town Hall in the heart of Holly Springs. On Main Street, Town Hall is a center of constant activity. Opened in 2003, the 35,000 square-foot, two-story brick building was designed in an architectural style reminiscent of the 19th century when Holly Springs was founded. A cupola with a large clock that faces Main Street and an outdoor plaza with a fountain behind the building are just two of the building's features. In the lobby, above a display case maintained by the Holly Springs Historical Preservation Society, hangs a historic oil painting of George Washington, dated to the 1700s. The painting hung in 1876 in Carpenter Hall in Philadelphia.</p>
					
					<p>Whether it is the Town's balance of commercial and residential development, its reasonable land prices coupled with its proximity to urban centers, or its small-town charm, new residents and businesses continue to be attracted to Holly Springs.</p>
					
					<p>Hardly a weekend passes without a Town-sponsored family activity in a local park or downtown, whether it's a free movie or concert during the warmer months or a seasonal event such as the annual Easter Egg Hunt or the Happy Holly Days Parade.</p>
					
					<p>One of the most popular communities in Holly Springs is Sunset Ridge. Sunset Ridge is home of the Devils Ridge Golf Coarse & country club. The area also provides an outstanding pool facility with water slide and water park for the little ones. A mix of scenic beauty, charming home styles and great amentias make Sunset Ridge one of the most sought after places to live in the Triangle!</p>
					
					<p>Indeed, the community seems ever occupied with providing for future generations. The Town currently has two elementary schools and one middle school and the new Holly Springs High School. A library and cultural arts center are scheduled to open in downtown Holly Springs in 2006 to offer additional opportunities for youth and adults alike.</p>
					
				<p><a href="http://www.hollyspringsnc.us/about/map/mappath.pdf" target="_blank">Click here for a map of of Holly Springs and its location within Wake County.</a></p>
				
					<p>Distance to Triangle Destinations</p>
					
					<ul>
					<li>Raleigh: 15 miles</li>
					<li>Cary: 6 miles</li>
					<li>Durham: 25 miles</li>
					<li>Chapel Hill: 30 miles</li>
					<li>Research Triangle Park: 18 miles</li>
					<li>Raleigh-Durham International Airport: 23 miles</li>
					</ul>
					
				<p class="center">
					<a href="http://www.agentpreview.com/agents/united-states/north-carolina/holly-springs.htm" target="_blank"><img src="/images/120x60view_my_profile.gif" alt="Holly Springs Real Estate Agent." width="120" height="60" border="0" /></a>
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
