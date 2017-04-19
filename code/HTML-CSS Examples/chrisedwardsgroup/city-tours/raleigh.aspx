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
	<title>Raleigh, North Carolina City Profile.</title>
	
	<meta name="Description" content="Raleigh, North Carolina City Profile.">

	<meta name="Keywords" content="Raleigh, North Carolina, Chris Edwards, RE/MAX United">

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
						<td><img src="/images/left-pix/raleigh_sign.jpg" alt="" width="290" height="210" border="0"><img src="/images/right-pix/rt-tours.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1 class="body">Raleigh, North Carolina Profile - The Time is Now!</h1>

					<p>What was once a mild capitol city mostly made up of government buildings is quickly getting an exciting facelift that is continuing to gain national attention and excitement for the outstanding things to come!</p>
					
					<p>The downtown is a work in progress with a goal to become an exciting metro with new shopping, convention center, premium hotels and tremendous growth in commercial relocation! The quality growth can already by seen in our new, memorial auditorium where you'll find top entertainment and Broadway shows!</p>
					
					<p>The town will still boast it's historic areas like Oakwood, which is a charming community that features historic homes in high demand! Super communities like Bedford at Falls River in North Raleigh offer many newer housing options and incredible amenities!</p>

					<p>(<a href="../featured-neighborhoods/oakwood.aspx">Oakwood</a> &amp; <a href="../featured-neighborhoods/bedford-falls-river.aspx">Bedford</a> neighborhood profiles - click here)</p>
					 
					<p>Premium academics on all levels just gives Raleigh even more advantages. The place to be if you're a die-hard college sports fan!</p>
					 
					<p>Growth of this quality and magnitude will continue to place Raleigh as one of the top cities to relocate to in the United States! More and more we are also finding people making Raleigh the place to come to for retirement. High cost of living, bad weather and general rat race has caused many retirees to make the Raleigh area their new city of choice!</p>
					
					<p>In a nutshell, the main reason people move to Raleigh are; the wonderful economy and employment opportunities, great place to start a business & raise a family, the beautiful landscape, world-class education options and overall quality of life.</p>
					
					<p>Perhaps our best spokespeople are our residents. A recent survey showed an astounding 96 percent of newcomers with children said they would move to the area again if they had it to do all over. And unlike most metro areas in the US, Raleigh and the Triangle have all the amenities of big-city life without the high cost of living.</p>

					<p class="center">
						<a href="http://www.agentpreview.com/agents/united-states/north-carolina/raleigh.htm" target="_blank"><img src="/images/120x60view_my_profile.gif" alt="Raleigh Real Estate Agent." width="120" height="60" border="0" /></a>
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
