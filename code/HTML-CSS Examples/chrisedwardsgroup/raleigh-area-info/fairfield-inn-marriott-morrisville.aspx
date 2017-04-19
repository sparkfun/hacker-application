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
	<title>Fairfield Inn By Marriott, Morrisville</title>
	
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
							<td><img src="/images/hotels-motels/fairfield-inn-marriott-exte.jpg" alt="" width="190" height="143" border="0" class="padding" /></td>
							<td><img src="/images/hotels-motels/fairfield-inn-marriott-room.jpg" alt="" width="190" height="143" border="0" class="padding" /></td>
						</tr>
					</tbody>
				</table>
				
				<table border="0">
					<tbody>
						<tr>
							<td><p><strong>Fairfield Inn By Marriott</strong><img src="../images/hotels-motels/fairfield-inn-marriott-logo.jpg" alt="" width="78" height="51" border="0" class="floatright"><br />Raleigh Airport/ Rtp. 2750 Slater Road Morrisville,<br />North Carolina 27560 US<br /><strong>Phone:</strong> (919) 468-2660<br /><strong>Website:</strong><br /><a href="http://marriott.com/property/propertypage/rdufa" target="_blank">http://marriott.com/property/propertypage</a></p></td>
						</tr>
					</tbody>
				</table>
				
				<p><strong>Description:</strong><br />Marriott's award winning economy hotel lodging chain features clean, convenient, quality accommodations and the peace of mind you get from Marriott consistency - all for a great value. The Fairfield Inn Raleigh Airport is 1 mile from Gateway Office Park, two miles from SAS Institute, one mile from GTE, three miles from Nortel, four miles from Carter Finley Stadium and State Fairgrounds and nine miles from downtown. For more detailed location information, directions and an area map, please see Area Info.</p>
				
				<p>Click on the link below to view in PDF format:</p>
				<ul class="whylist">
					<li class="why">
						 <a href="hotel-motel/fairfield-inn-by-marriott-raleigh-airport-rtp.pdf" target="_blank">Print PDF</a>
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
