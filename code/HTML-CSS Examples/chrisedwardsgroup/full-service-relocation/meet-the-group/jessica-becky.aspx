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
	<title>Meet the Chris Edwards Group - Jessica Reese and Becky Arguello - Full Service Relocation to Raleigh, Cary and Apex, NC</title>
	
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
			
				<h1>Meet Jessica Reese and Becky Arguello, Corporate Sales Representatives</h1>
				
				<img src="/images/group-photos/corporate-building.jpg" alt="" width="163" height="212" border="0" id="photo" />
				
				<a href="http://www.equitycorporatehousing.com/" target="_blank"><img src="/images/group-photos/corporate-housing-logo.jpg" alt="" width="166" height="212" border="0" id="photo" /></a>
				
				<table id="address">
					<tr>
						<td>
							<strong>Jessica Reese (or) Becky Arguello</strong><br />
							Corporate Sales Representatives<br />
							Equity Corporate Housing<br />
							100 Northwoods Village Drive<br />
							Cary, NC 27513<br /><br />
							
							<strong>Contact information for Jessica Reese</strong><br />
							<a href="mailto:jreese@eqrworld.com">jreese@eqrworld.com</a><br />
							<a href="http://www.equitycorporatehousing.com/" target="_blank">www.equitycorporatehousing.com</a><br />
							Office: 919.468.5611<br />
							Toll Free: 800.533.2370<br />
							Fax: 919.468.5499
						</td>
					</tr>
				</table> 


				<p>&quot;Making Life Easy&quot;</p>
				
				<p>Equity Corporate Housing&reg; is a division of Equity Residential&reg;, the largest owner of apartments in the United States and America's Choice for Corporate Housing&reg; They have access to more than 200,000 units that Equity Residential owns and manages, as well as thousands more around the world. This allows them the flexibility of offering furnished, unfurnished, short-term and long-term options.</p>
				
				<p>At Equity Corporate Housing, they specialize in making your extended stay worry-free. Our spacious corporate apartments provide much more room than a hotel, at a more affordable price. Whether you are booking for a large group move, relocating an individual or setting up a furnished apartment on your own, Equity Corporate Housing has a solution for you.</p>
				
				<p>Everything they do is designed to make your extended stay happy, worry-free, and, above all, easy. If you are looking for the right corporate apartment, they offer many choices that are conveniently located and offer all the amenities you need. Their corporate apartments provide much more room than a hotel, at a much more affordable price.</p> 
				
				<p>If you are responsible for arranging temporary housing for your company’s employees, we can make your life easy, too. We offer one point of contact for fulfilling all your temporary housing needs, plus a host of benefits such as direct billing available to companies that participate in our National Accounts program. You get peace of mind from knowing that your employees will experience our consistent and high standards for quality, maintenance and service wherever they stay.</p>
				
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
