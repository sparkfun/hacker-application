<%@ Register Tagprefix="Top" Tagname="FeaturedProperty" Src="/includes/featured.ascx" %>
<%@ Register Tagprefix="Top" Tagname="TopLinks" Src="/includes/toplinks.ascx" %>
<%@ Register Tagprefix="Top" Tagname="LeftLinks" Src="/includes/leftlinks.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Address" Src="/includes/address.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Neighborhoods" Src="/includes/neighborhoods-group.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Logos" Src="/includes/logos.ascx" %>
<%@ Register Tagprefix="Bottom" Tagname="Footer" Src="/includes/footer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
	"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
	<title>Meet the Chris Edwards Group - David N. Bryan, P.A. - Full Service Relocation to Raleigh, Cary and Apex, NC</title>
	
	<meta name="Description" content="Meet the Chris Edwards Group - Full Service Relocation to Raleigh, Cary and Apex, NC.">

	<meta name="Keywords" content="Meet the Chris Edwards Group, Raleigh Relocation, North Carolina Relocation, Full Service Relocation">

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
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate4.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-relocation.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Meet David N. Bryan, P.A.</h1>
				
				<img src="../../images/group-photos/david-n-bryan.jpg" alt="" width="180" height="230" border="0" id="photo">
				
				<table id="address">
					<tr>
						<td>
							<strong>David N. Bryan</strong><br />
							Attorney at Law<br />
							Post Office Box 1238<br />
							Holly Springs, NC 27540<br /><br />
							
							<strong>Contact Information for David N. Bryan</strong><br />
							Telephone: (919)552-9995<br />
							Fax: (919)552-9996<br />
							E-Mail: <a href="mailto:davidbryan@dnblaw.com">davidbryan@dnblaw.com</a><br />
							Website: <a href="http://www.dnblaw.com/pages/793541/index.htm" target="_blank">www.dnblaw.com</a>
						</td>
					</tr>
				</table>
				
				<p>Buying a home may be the biggest single investment of your lifetime. A life's savings may be invested in this one venture.  As a buyer of real estate in North Carolina you will need an attorney to represent you before, during and after the actual time spent at the settlement table.</p>

				<p>Selling a home also involves many issues that may not be as obvious. A seller must be able to deliver good, marketable title to the buyer and a failure to fulfill this obligation can result in years of costly litigation down the road.</p>
				
				<p>My firm focuses its law practice on representing buyers and sellers of residential and commercial real estate in Cary, Apex, Holly Springs, Fuquay Varina and greater Wake County, North Carolina. We work closely with our clients - whether they are the buyer or seller - to ensure that real estate documents are in order and that the real estate closing takes place without stress or unwelcome surprises.</p>
				
				<p>In addition to our real estate services, my firm works with individuals looking to start a business, buy a business or transfer an existing business. Many of my clients who are moving into a new community are also looking for new business opportunities.  Further, we also assist new families to the area set up estate plans and provide free reviews of existing wills from other states.</p>
				
				<p>As a native of North Carolina and a resident of this area, I work closely with my clients to make sure that the transition into their new community - both as neighbors and entrepreneurs - is pleasant and rewarding.</p>
				
				<p>Call (919) 552-9995 or E-Mail <a href="mailto:davidbryan@dnblaw.com">davidbryan@dnblaw.com</a> for Additional Information or to Schedule a Closing or Appointment.</p>
				
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
