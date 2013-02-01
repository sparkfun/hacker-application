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
	<title>Homebuyer Services directory - Chris Edwards RE/MAX United Cary, North Carolina.</title>
	
	<meta name="Description" content="Homebuyer Services directory - Chris Edwards with RE/MAX United in Cary, North Carolina.">

	<meta name="Keywords" content="hoem buyer, Homebuyer Services, Homebuyer, Chris Edwards, RE/MAX United, Cary, North Carolina">

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
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate2.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-buyer.jpg" alt="Contact Chris Edwards for Home Buyer Assistance in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Home Buyer Assistance and Information</h1>
				
				<p>Buying a home will probably be the largest financial investment of your life. Buying a home is a major transaction. Why chance potential financial problems, legal technicalities and the emotional stress created by searching for a new home. Focus your time and energy on making the decision of what to buy. Use the <strong>free Buyer Agent services</strong> of the Chris Edwards Group to locate and secure the Real Estate you desire.</p>

				<p>Please review the information in the pages listed below to find out more about the many ways the Chris Edwards Group can assist you in buying or selling a home or real estate.</p>
				
				<table border="0" class="directory" summary="Featured Raleigh, North Carolina Cities including Apex, Cary, Chapel Hill, Clayton, Durham, Fuquay-Varina, Holly Springs, Garner, Knightdale, Morrisville, Raleigh, Research Triangle Park, Wake Forest, Willow Springs.">
					
					<tr>
						<td>
							<p><a href="homebuyer-services.aspx"><strong>Home Buyer Services</strong></a></p>
							<p>Why use my Professional Services? ...</p>
						</td>
						<td>
							<p><a href="buyers-agent.aspx"><strong>Buyer's Agent</strong></a></p>
							<p>Why Use A Buyer's Agent? ...</p>
						</td>
					</tr>
					
					<tr>
						<td>
							<p><a href="buyer-faq.aspx"><strong>Frequently Asked Questions</strong></a></p>
							<p>Buyers Frequently Asked Questions ...</p>
						</td>
						<td>
							<p><a href="home-inspection.aspx"><strong>Home Inspection</strong></a></p>
							<p>Ok, so you've found your dream home ...</p>
						</td>
					</tr>
					
					<tr>
						<td>
							<p><a href="home-warranties-buyer.aspx"><strong>Home Warranties (Buyer)</strong></a></p>
							<p>Home Warranties Advice for Buyers ...</p>
						</td>
						<td>
							<p><a href="homeowners-insurance.aspx"><strong>Homeowner's Insurance</strong></a></p>
							<p>Homeowner's insurance protects you and your house ...</p>
						</td>
					</tr>
					
					<tr>
						<td>
							<p><a href="multiple-offers-buyer.aspx"><strong>Multiple Offers to Buyer</strong></a></p>
							<p>What happens when there are multiple offers to buy a house? ...</p>
						</td>
						<td>
							<p><a href="property-disclosure-buyer.aspx"><strong>Property Disclosure Buyer</strong></a></p>
							<p>North Carolina General Statute 47E requires a seller to fill out ...</p>
						</td>
					</tr>
					
					<tr>
						<td>
							<p><a href="property-taxes.aspx"><strong>Property Taxes</strong></a></p>
							<p>Here's some general information about property taxes in North Carolina ...</p>
						</td>
						<td>
							&nbsp;
						</td>
					</tr>
					
				</table>
				
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
