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
	<title>About Chris Edwards RE/MAX United Cary, North Carolina.</title>
	
	<meta name="Description" content="Profile of Chris Edwards with RE/MAX United in Cary, North Carolina.">

	<meta name="Keywords" content="Chris Edwards, RE/MAX United, Cary, North Carolina">

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
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate5.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-about.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>About Chris Edwards - Business Profile</h1>
								
				<p>The Chris Edwards Group takes a different approach in the market! In addition to providing excellent, reliable real estate service, we take a very proactive role in getting your home sold, or finding you your dream home!</p>
				
				<p><strong>Selling your home:</strong></p>
				
				<p>The traditional means of putting a sign in the yard and an ad in the paper might have been enough in the past, but not in today's competitive marketplace! You need a detailed marketing plan to reach the maximum amount of buyers, as well as a real estate company that will invest in your success.</p>
				
				<p><strong>Image Marketing:</strong></p>
				
				<p>Our detailed plan consists of creating a unique image for your home on the web, quality color flyers & other innovative features that will attract potential buyers to your home. <!-- Click on the links below to see an example of our image flyer, the finest virtual tour technology and a web commercial that could be created for your home.-->Click on the link to view a typical <a href="http://www.visualtour.com/show.asp?T=438460" target="_blank">Virtual Tour Example</a>. In the near future we will have examples of the other services listed available here. Until then, contact Chris at 888-828-0288 or via <a href="mailto:chris@chrisedwardsgroup.com">email</a> and he will make arrangements for you to receive them right away.</a></p>
				
				<p class="bold">
					<a href="flyer-example.pdf" target="_blank">Flyer Example</a><br /> 
					<a href="http://www.visualtour.com/show.asp?T=438460" target="_blank">Virtual Tour Example</a><br />
					<a href="/flash/welcome-to-raleigh-cary-apex.swf" target="_blank">Web Commercial Example</a>
				</p>
				
				<p><strong>Buying a Home:</strong></p>
				
				<p>We are Relocation Certified! As buyers agents, we make the process smooth and efficient by providing the research and support needed to help you make a confident offer! Stress and uncertainty will be replaced with excitement and confidence! </p>
				
				<p><strong>Power of the Group!</strong></p>
				
				<p>Whether you are a native or new to the area, our referral network will provide the contacts needed to get you through the process. Resources such as lending, moving, storage, extended stay hotel & utility hook-up are just a few of the contacts we can provide to take any hassle out of your relocation!</p>
				
				<p>Don't settle for the same old marketing and services .....</p>
				
				<p><strong>Give yourself the &quot;Home Advantage&quot; with the Chris Edwards Group!</strong></p>
				
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
