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
	<title>Property Disclosure Information for the Seller.</title>
	
	<meta name="Description" content="Property disclosure information for home sellers.">

	<meta name="Keywords" content="Property Disclosure, Home Seller">

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
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate4.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-seller.jpg" alt="Contact Chris Edwards for Home Seller Assistance in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Property Disclosure for the Seller</h1>
					
				<p>North Carolina General Statute 47E requires you to fill out a property disclosure statement when you sell your property. You can get this statement form at any RE/MAX UNITED office, or from Chris Edwards.</p>

				<p>Click on the "Property Disclosure Statement" link to review the information needed and download a copy of the form in "PDF" format before you put your home on the market for sale: <a href="http://www.ncrec.state.nc.us/forms/rec422.pdf" class="rollblack" target="_blank">Property Disclosure Statement</a></p>

				<p>You must provide a disclosure statement to all potential purchasers in connection with the sale (unless the tenant is already occupying or intends to occupy the dwelling), even if you are selling your home on your own. Except in cases of:</p>
				
				<ul class="whylist">
					<li class="why">New constructed, never occupied homes</li>
					<li class="why">Transfers from one co-owner to another</li>
					<li class="why">Transfers to a spouse or persons in the lineal line</li>
					<li class="why">Transfers between spouses as a result of a divorce</li>
					<li class="why">Transfers pursuant to a court order, foreclosures, bankruptcy, or by trustees or estate administrators</li>
					<li class="why">Lease with option to purchase contract where the lessee occupies or intends to occupy the dwelling</li>
					<li class="why">Transfers between parties when both parties agree in advance not to complete a residential property disclosure statement</li>
				</ul>
				
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
