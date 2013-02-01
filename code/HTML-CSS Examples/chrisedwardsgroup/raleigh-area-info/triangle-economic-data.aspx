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
	<title>Triangle, North Carolina Economic Data. Chris Edwards RE/MAX United</title>
	
	<meta name="Description" content="Description of Triangle, North Carolina economic data.">

	<meta name="Keywords" content="Raleigh, Durham, Chapel Hill, Triangle, North Carolina, Economic Data">

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
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate3.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-triangle-info.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1 class="body">Triangle Economic Data</h1>
					
				<table border="0" class="profile">
					<tbody>
						<tr>
							<th colspan="2">RALEIGH HOUSING (Corporate Limits)</th>
						</tr>
						<tr>
							<td>Owner-occupied</td>
							<td>52.3%</td>
						</tr>
						<tr>
							<td>Renter-occupied</td>
							<td>47.7%</td>
						</tr>
						<tr>
							<td>Total Units (July, 2003)</td>
							<td>139,259</td>
						</tr>
						<tr>
							<td>Median Contract Rent</td>
							<td>$763</td>
						</tr>
						<tr>
							<td>Average Home Sale Price</td>
							<td>$234,157</td>
						</tr>
						<tr>
							<td class="profilebody" colspan="2">Raleigh % of Wake County sales: 60 percent (Source: ACCRA 4th Qtr. 2002)</td>
						</tr>
					</tbody>
				</table>
				
				<table border="0" class="profile">
					<tbody>
						<tr>
							<th colspan="2">INCOME</th>
						</tr>
						<tr>
							<td>Wake Co. Per Capita (2000)</td>
							<td>$36,581</td>
						</tr>
						<tr>
							<td>State Per Capita (2000)</td>
							<td>$26,882</td>
						</tr>
						<tr>
							<td>MSA Median Family</td>
							<td>$71,300</td>
						</tr>
						<tr>
							<td class="profilebody" colspan="2">(MSA = Metropolitan Statistical Area for Raleigh-Durham Region)</td>
						</tr>
					</tbody>
				</table>
				
				<table border="0" class="profile">
					<tbody>
						<tr>
							<th colspan="2">EMPLOYMENT (ESC 4th Quarter 2001)</th>
						</tr>
						<tr>
							<td>Manufacturing</td>
							<td>27,571 (7.2%)</td>
						</tr>
						<tr>
							<td>Mining & Construction</td>
							<td>27,911 (7.3%)</td>
						</tr>
						<tr>
							<td>Transportation, Communications & Public Utilities</td>
							<td>21,586 (5.7%)</td>
						</tr>
						<tr>
							<td>Wholesale and Retail Trade</td>
							<td>92,759 (24.3%)</td>
						</tr>
						<tr>
							<td>Finance, Insurance, & Real Estate</td>
							<td>20,270 (5.3%)</td>
						</tr>
						<tr>
							<td>Services/Other</td>
							<td>120,629 (31.7%)</td>
						</tr>
						<tr>
							<td>Government</td>
							<td>70,336 (18.5%)</td>
						</tr>
						<tr>
							<td>TOTAL</td>
							<td>381,062</td>
						</tr>
					</tbody>
				</table>
				
				<table border="0" class="profile">
					<tbody>
						<tr>
							<th colspan="3">Average Annual Unemployment Rates</th>
						</tr>
						<tr>
							<td>&nbsp;</td>
							<td class="profilebody3"><span class="bold">2003</span></td>
							<td class="profilebody3"><span class="bold">5 Yr. Average</span></td>
						</tr>
						<tr>
							<td>Raleigh</td>
							<td class="profilebody3">5.0%</td>
							<td class="profilebody3">2.7%</td>
						</tr>
						<tr>
							<td>Wake County</td>
							<td class="profilebody3">5.0%</td>
							<td class="profilebody3">2.6%</td>
						</tr>
						<tr>
							<td>North Carolina</td>
							<td class="profilebody3">6.6%</td>
							<td class="profilebody3">4.3%</td>
						</tr>
						<tr>
							<td>United States</td>
							<td class="profilebody3">5.8%</td>
							<td class="profilebody3">4.6%</td>
						</tr>
						<tr>
							<td>Estimated Raleigh Employment</td>
							<td class="profilebody3">1995</td>
							<td class="profilebody3">180,676</td>
						</tr>
						<tr>
							<td>Projected Raleigh Employment</td>
							<td class="profilebody3">2025</td>
							<td class="profilebody3">324,833</td>
						</tr>
					</tbody>
				</table>
				
				<table border="0" class="profile">
					<tbody>
						<tr>
							<th colspan="2">TRANSPORTATION - Airport and Airlines</th>
						</tr>
						<tr>
							<td colspan="2"><span class="bold">Raleigh-Durham International Airport (2002)</span></td>
						</tr>
						<tr>
							<td colspan="2">8.4 million passengers incoming & departing</td>
						</tr>
						<tr>
							<td colspan="2">Passengers (daily) 23,215</td>
						</tr>
						<tr>
							<td colspan="2">123,000 tons of cargo handled yearly</td>
						</tr>
						<tr>
							<td colspan="2">Take offs and landings (yearly) 80,765</td>
						</tr>
						<tr>
							<td colspan="2">Cargo tonnage handled (daily) 336</td>
						</tr>
						<tr>
							<td colspan="2"><span class="bold">Major Airlines</span></td>
						</tr>
						<tr>
							<td>American Airlines</td>
							<td>Northwest</td>
						</tr>
						<tr>
							<td>Air Canada</td>
							<td>Southwest</td>
						</tr>
						<tr>
							<td>Air Tran</td>
							<td>TWA</td>
						</tr>
						<tr>
							<td>Canadian Regional</td>
							<td>USAir</td>
						</tr>
						<tr>
							<td>Continental</td>
							<td>United</td>
						</tr>
						<tr>
							<td>Delta</td>
							<td>Skyway</td>
						</tr>
						<tr>
							<td colspan="2"><span class="bold">18 Commuter Airlines</span></td>
						</tr>
					</tbody>
				</table>
				
				<table border="0" class="profile">
					<tbody>
						<tr>
							<th colspan="2">TRANSPORTATION - Bus, Rail and Transit Authority</th>
						</tr>
						<tr>
							<td colspan="2">Amtrak - 111,698 passengers yearly</td>
						</tr>
						<tr>
							<td colspan="2">6 trains daily serving Raleigh</td>
						</tr>
						<tr>
							<td colspan="2">CSX Railroad</td>
						</tr>
						<tr>
							<td colspan="2">Norfolk Southern Railroad</td>
						</tr>
						<tr>
							<td colspan="2"><strong>Capital Area Transit (CAT) - City Bus System</strong></td>
						</tr>
						<tr>
							<td>43 buses</td>
							<td>15 connector buses</td>
						</tr>
						<tr>
							<td>18 fixed routes</td>
							<td>9 connector routes</td>
						</tr>
						<tr>
							<td colspan="2">3.4 million riders per year</td>
						</tr>
						<tr>
							<td colspan="2"><strong>Triangle Transit Authority - Regional Bus System</strong></td>
						</tr>
						<tr>
							<td>29 buses</td>
							<td>Approx. 34,100 riders per year</td>
						</tr>
					</tbody>
				</table>
				
				<table border="0" class="profile">
					<tbody>
						<tr>
							<th colspan="2">DEVELOPMENT</th>
						</tr>
						<tr>
							<td colspan="2">Value of Construction Authorized - 2002</td>
						</tr>
						<tr>
							<td>Raleigh</td>
							<td>$1,001,613,936</td>
						</tr>
						<tr>
							<td>Wake County</td>
							<td>N/A</td>
						</tr>
						<tr>
							<td colspan="2">Residential Lots  or Units Approved - 2002</td>
						</tr>
						<tr>
							<td>Raleigh</td>
							<td>4,236</td>
						</tr>
						<tr>
							<td>5 Year Average</td>
							<td>3,826</td>
						</tr>
						<tr>
							<td colspan="2">Commercial Building Permit Value - 2002</td>
						</tr>
						<tr>
							<td>Raleigh</td>
							<td>$86 Million</td>
						</tr>
						<tr>
							<td>5 Year Average</td>
							<td>$179 Million</td>
						</tr>
					</tbody>
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
