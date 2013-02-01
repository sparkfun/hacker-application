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
	<title>Garner, North Carolina City Profile.</title>
	
	<meta name="Description" content="Garner, North Carolina City Profile.">

	<meta name="Keywords" content="Garner, North Carolina, Chris Edwards, RE/MAX United">

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
						<td><img src="/images/left-pix/garner-sign.jpg" alt="" width="290" height="210" border="0"><img src="/images/right-pix/rt-tours.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1 class="body">Garner, North Carolina Profile</h1>
				
					<p>Garner is located just south of Raleigh and is a thriving community that is becoming more of a Triangle Destination! Like Morrisville, many businesses are finding Garner to be an advantageous place to headquarter or have a location. Being so close to Raleigh as well as majors roadways like; I-40, 401, 70 and 50 has placed Garner in a winning position! Plenty of new shopping and retail also add to the enticement of relocating to the Garner area!</p>

					<p>Garner has become a &quot;diamond in the rough&quot; for many homebuyers. There are many new and resale home choices with a more affordable price tag, not to mention that you are still close to Downtown Raleigh!</p>
					
					<p>Since development has been steady, but not explosive over the last decade, Garner has had time to plan for essential facilities and services to accommodate the growth that's now coming its way, such as the Garner Senior Center or the newly developed Lake Benson Park.</p>
					
					<p>Today, Garner is the place to find all the opportunities that go along with economic growth and all the benefits of city living in a less taxing, more gracious living and business environment. Even with this growth, you'll still notice the quality, small town feel!</p>
					
				<p>Garner is located just south of Raleigh and is a unique and inviting combination of the best of the old and the most innovative of the new.</p>

				<p>Nowhere else in the state does the solid bedrock of family-oriented tradition and the progressive catalyst of new ideas and new technology meet and mingle. A truly fascinating harmony is found in our community of over 18,000 people.</p>

				<p>That harmony has drawn thousands of new residents to Garner during the last 20 years as workers and leaders in business, industry, government and research look for an affordable, traditional home for their families. With Garner's convenient access to one of the nation's premier metropolitan areas, major transportation arteries, numerous universities, and the world famous Research Triangle Park, Garner has become a popular place to live and do business in recent years.</p>

				<p>Because Garner has a small-town heritage, it's the kind of place where neighbors meet at their mailboxes and talk over local issues - where everyone turns out for the Friday night high school football games or the annual Independence Day Festival.</p>

				<p>Garner is minutes away from enjoying an exhibit at the State Art Museum, rooting for one of the local Atlantic Coast Conference football teams, enjoying an evening's entertainment with a live performance by the North Carolina Symphony, or laughing with one of the country' s hottest comedians.</p>
				
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
