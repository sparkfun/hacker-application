<%@ Register Tagprefix="Top" Tagname="FeaturedProperty" Src="/includes/featured.ascx" %>
<%@ Register Tagprefix="Top" Tagname="TopLinks" Src="/includes/toplinks.ascx" %>
<%@ Register Tagprefix="Top" Tagname="LeftLinks" Src="/includes/leftlinks.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Neighborhoods" Src="/includes/neighborhoods.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Logos" Src="/includes/logos.ascx" %>
<%@ Register Tagprefix="Bottom" Tagname="Footer" Src="/includes/footer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
	"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
	<title>NAME HERE Neighborhood of Cary, North Carolina - Chris Edwards RE/MAX United</title>
	
	<meta name="Description" content="Description of the NAME HERE neighborhood in Raleigh, North Carolina.">

	<meta name="Keywords" content="Raleigh, NAME HERE, North Carolina">

	<meta name="Robots" content="all">

	<link rel="stylesheet" href="/includes/ce.css" type="text/css">
	<link rel="stylesheet" href="/includes/forms.css" type="text/css">
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
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate2.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-tours.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>NAME HERE Neighborhood Profile</h1>
						
						<table border="0" class="faq" summary="Barrington &amp; Regency Park Estates Neighborhood">
							<tr>
				    <td>
									City: , North Carolina
								</td>
							</tr>
							<tr>
								<td>
									Address: 
								</td>
							</tr>
							<tr>
								<td>
									<a href="http://maps.google.com/maps?q=100+Killingsworth,+Cary,+NC+27511&spn=0.030520,0.054957&iwloc=A&hl=en" target="_blank">Map link to neighborhood</a> (click link)
								</td>
							</tr>
						</table>
							
						<h2>Neighborhood Facts:</h2>
						
						<ul class="whylist">
							<li class="why">
								Age range is 1993-2001
							</li>
							<li class="why">
								Price range is $750,000-$2.500,000
							</li>
							<li class="why">
								Excellent Cary Location!
							</li>
							<li class="why">
								Next to Regency Park
							</li>
						</ul>
						
						<p></p>

						<p></p>

						<p></p>
						
						<h2>Please feel free to <a href="/contact.aspx">contact</a> Chris for more information about the sale of a home in a particular area or subdivision.</h2>
				
				<!-- end text -->
			
				<!-- start address -->
				<table border="0" id="address">
					<tr><td colspan="2"><img src="/images/logos/eprocertbanner.gif" width="312" height="40" alt="" /></td></tr>
					<tr>
						<td>
							<strong>Chris Edwards Group</strong><br />
							RE/MAX United Raleigh-Cary<br />
							51 Kilmayne Dr., Ste. 100<br />
							Cary, NC 27511<br /><br />
							
							<strong>Toll Free: (888) 828-0288</strong><br />
							<strong>Office: (919)469-6540</strong><br />
							FAX: (919) 469-8444<br />
							Email: <a href="mailto:chris@chrisedwardsgroup.com">chris@chrisedwardsgroup.com</a>
						</td>
						
						<td><img src="/images/edwards-logo-lg.jpg" alt="" width="174" height="150" border="0"></td>
						
					</tr>
				</table>
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
