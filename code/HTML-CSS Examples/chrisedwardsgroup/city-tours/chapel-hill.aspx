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
	<title>Chapel Hill, North Carolina City Profile.</title>
	
	<meta name="Description" content="Chapel Hill, North Carolina City Profile.">

	<meta name="Keywords" content="Chapel Hill, North Carolina, Chris Edwards, RE/MAX United">

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
						<td><img src="/images/left-pix/chapel-hill-sign.jpg" alt="" width="290" height="210" border="0"><img src="/images/right-pix/rt-tours.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1 class="body">Chapel Hill, North Carolina Profile</h1>
				
				<p>Welcome to <strong>Chapel Hill</strong> and <strong>Carrboro</strong>, nestled in the rolling, wooded Piedmont of North Carolina. These two towns are ideally located three hours from the Atlantic coast and three hours from the Blue Ridge Mountains, allowing residents and visitors to enjoy a variety of recreational activities.</p>

				<p>As one of the three "points" of The Research Triangle, Chapel Hill, along with Raleigh and Durham, continually receives accolades for being a top location to live and do business. Many folks ask us what some of the main differences are with Chapel Hill vs. Raleigh. Besides its western location and outstanding collegiate environment, Chapel Hill does have a hillier terrain with lots of beautiful trees! There just aren't many more attractive places!</p>

				<p>The Town of Chapel Hill is located principally in Orange County and slightly in Durham County in the north central portion of North Carolina on the Piedmont Plateau, approximately equidistant between Washington, DC, and Atlanta, Georgia. The area's topography is characterized by rolling hills.</p> 

				<p>The Town, which was incorporated in 1819, presently covers an area of 21.1 square miles and has a population of 52,440 according to the latest estimate issued by the State of North Carolina for July 2002. The Town is the home of the University of North Carolina at Chapel Hill. the nation's oldest public university, established in 1789. Today, the University enjoys a reputation as one of the best public universities in the United States. </p>

				<p>Today, Chapel Hill is anything but a typical southern town. In fact, some say it's really not southern at all, with so many residents coming from other parts of the country. The community is diverse, consisting of professors, students, business people and retirees from all over the world, not to mention the people who are native to the city.</p>

				<p>In addition to the prominence of the nation's first state university, another attractive feature of downtown Chapel Hill is Franklin Street, named after Benjamin Franklin. This is the most vibrant downtown in the state. Day and night, the streets are filled with people enjoying a multitude of shopping, dining and entertainment options.</p>

				<p class="center">
					<a href="http://www.agentpreview.com/agents/united-states/north-carolina/chapel-hill.htm" target="_blank"><img src="/images/120x60view_my_profile.gif" alt="Chapel Hill Real Estate Agent." width="120" height="60" border="0" /></a>
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
