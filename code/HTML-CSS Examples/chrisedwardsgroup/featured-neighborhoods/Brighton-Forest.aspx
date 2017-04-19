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
	<title>Brighton Forest Neighborhood, North Carolina - Chris Edwards RE/MAX United</title>
	
	<meta name="Description" content="Description of the Brighton Forest Neighborhood, North Carolina.">

	<meta name="Keywords" content="Brighton Forest, Cary, North Carolina">

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
						<td><img src="/images/left-pix/BrightonForest-sign.jpg" alt="" width="290" height="210" border="0"><img src="/images/right-pix/rt-tours.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Brighton Forest Neighborhood Profile</h1>

				<p>Brighton Forest is a unique neighborhood of custom homes on the border of Apex &amp; Fuquay Varina, North Carolina! This is truly a rare find to purchase a quality, custom home on a generous lot in the $300's and still close to nice amenities! </p>

				<p><strong>The Brighton Forest provides:</strong></p>
				
				<ul>
					<li>Custom built homes from the $200's to low $400's depending on size, upgrades and section of the neighborhood.</li>
					<li>The two sections of Brighton Forest are Wynwood &amp; Oakhurst, Oakhurst being the more expensive section of the neighborhood.</li>
					<li>A group of six, custom home builders</li>
					<li>Neighborhood completion is projected at 392 homes</li>
					<li>A nice community pool with bath house, waterslide &amp; playground</li>
					<li>More private, wooded lots than most neighborhoods in the area!</li>
				</ul>
				
				<h2>Click here to see our featured <a href="/raleigh-cary-real-estate/single-family-home.aspx?MLSID=1675396">Brighton Forest Listings</a>!</h2>
						
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
