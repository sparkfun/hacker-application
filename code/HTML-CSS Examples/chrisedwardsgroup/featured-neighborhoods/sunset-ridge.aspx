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
	<title>Sunset Ridge Neighborhood of Cary, North Carolina - Chris Edwards RE/MAX United</title>
	
	<meta name="Description" content="Description of the Sunset Ridge neighborhood in Raleigh, North Carolina.">

	<meta name="Keywords" content="Raleigh, Sunset Ridge, North Carolina">

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
						<td><img src="/images/left-pix/sunset-ridge-logo.jpg" alt="" width="290" height="210" border="0"><img src="/images/right-pix/rt-tours.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Sunset Ridge Neighborhood Profile</h1>
						
						<table border="0" class="faq" summary="Riggsbee Farm Neighborhood">
							<tr>
				    			<td>
									City: Holly Springs, North Carolina
								</td>
							</tr>
							<tr>
								<td>
									Address: Holly Springs Road &amp; Linksland
								</td>
							</tr>
						</table>
							
						<h2>Neighborhood Facts:</h2>
						
						<ul class="whylist">
							<li class="why">
								Age range is from the early 90's through today
							</li>
							<li class="why">
								Price range is $400,000 - $700,000
							</li>
							<li class="why">
								Between Cary &amp; Fuquay Varina
							</li>
						</ul>
						
						<p>Welcome to one of my favorite Triangle communities, Sunset Ridge! The Sunset Ridge area has the ability to offer a multitude of wonderful amenities that I find most buyers are looking for. These amenities include; lots of custom housing choices, great schools, including the brand new Holly Springs High School, close to shopping and unbeatable recreational facilities!</p>
						
						<p>Many buyers have chosen Sunset Ridge simply because of its outstanding value. Located between Cary &amp; Fuquay Varina, Holly Springs is just minutes from Raleigh but many buyers have thought the extra distance was more than worth it!</p> 
						
						<p>Sunset Ridge began in the early 90's and wonderful custom homes are still being built today. The neighborhood also features homes right on Devils Ridge Golf Coarse that many have felt were priced very affordably! Nice townhomes and bungalow homes can be found for those looking for more affordability and less maintenance.</p>  
						
						<p>The town of Holly Springs features many nice communities but Sunset Ridge has been the cornerstone of the area! Click on the links below to lean more about Devils Ridge Country Club and the Swim &amp; Tennis Facilities.</p> 
						
						<p>Homes in Sunset Ridge currently average in the $350-450,000 range.</p> 
						
						<p>Devils Ridge Country Club Site:</p>
						
						<p><a href="http://www.devilsridgecc.com/about/contact_information.htm" target="_blank">http://www.devilsridgecc.com/about/contact_information.htm</a></p>

						<p>Sunset Ridge Tennis &amp; Swim Club site:</p>

						<p><a href="http://www.sunsetridgeclub.com/" target="_blank">http://www.sunsetridgeclub.com/</a></p>
						
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
