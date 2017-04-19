<%@ Register Tagprefix="Top" Tagname="FeaturedProperty" Src="/includes/featured.ascx" %>
<%@ Register Tagprefix="Top" Tagname="TopLinks" Src="/includes/toplinks.ascx" %>
<%@ Register Tagprefix="Top" Tagname="LeftLinks" Src="/includes/leftlinks.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Hotels" Src="/includes/hotels.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Address" Src="/includes/address.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Neighborhoods" Src="/includes/neighborhoods.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Logos" Src="/includes/logos.ascx" %>
<%@ Register Tagprefix="Bottom" Tagname="Footer" Src="/includes/footer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
	"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
	<title>Raleigh/ Morrisville Microtel Inn</title>
	
	<meta name="Description" content="Hotels and Motel links for Raleigh, Cary and Wake County, North Carolina.">

	<meta name="Keywords" content="Hotels, Motels, Raleigh, Cary, Wake County">

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
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate2.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-link-directory.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Raleigh Hotels, Morrisville - Durham:</h1>
				
				<table border="0">
					<tbody>
						<tr>
							<td><img src="/images/hotels-motels/microtel-inn-exterior.jpg" alt="" width="190" height="143" border="0" class="padding" /></td>
							<td><img src="/images/hotels-motels/microtel-inn-room.jpg" alt="" width="190" height="143" border="0" class="padding" /></td>
						</tr>
					</tbody>
				</table>
				
				<table border="0">
					<tbody>
						<tr>
							<td><p><strong>Microtel Inn</strong><img src="/images/hotels-motels/microtel-inn-logo.jpg" alt="" width="70" height="51" border="0" class="floatright"><br />104 Factory Shops RD Morrisville,<br />North Carolina 27560 US<br /><strong>Phone:</strong> (888) 771-7171<br /><strong>Website:</strong><br /><a href="http://www.microtelinn.com/reservations/locationdetail.asp?facid=25" target="_blank">http://www.microtelinn.com/reservations/locationdetail</a></p></td>
						</tr>
					</tbody>
				</table>
				
				<p><strong>Description:</strong><br />Amenities include: 98 Guest Rooms 3 Floors/ Air Conditioning Connecting Rooms/ Cribs - Desk and Chair/ Hair Dryer - Ice Machine/ Iron/Ironing Boards - Microfitness Center/ Non Smoking Rooms - Safe Deposit Boxes/ Cable Tv W/Remote - Wake Up System/ Elevator/ Available in Limited Rooms Inquire Upon Check in - In Room Microwave - In Room Refrigerator<br />Off Site Facility: Exercise Room - Health Club/Priviledge</p>
				
				<p>Click on the link below to view in PDF format:</p>
				<ul class="whylist">
					<li class="why">
						 <a href="hotel-motel/microtel-inn-durham-nc.pdf" target="_blank">Print PDF</a>
					</li>
				</ul>
				
				<!-- start Hotels -->
				<Middle:Hotels id="ctlHotels" runat="server" />
				<!-- end Hotels -->
				
				<p><strong>Link Trade</strong> - Would you like to exchange links with us?  We accept high quality link partners.  All link partners will be evaluated based on relative content and quality.  We require a reciprocal link on a comparable website page.  To setup a reciprocal link <a href="mailto:webmaster@chrisedwardsgroup.com">e-mail the webmaster</a>.</p>
				
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
