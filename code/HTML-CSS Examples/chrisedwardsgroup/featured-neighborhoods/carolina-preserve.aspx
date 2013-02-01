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
	<title>Carolina Preserve at Amberly in Cary, North Carolina - Chris Edwards RE/MAX United</title>
	
	<meta name="Description" content="Description of the Carolina Preserve at Amberly in Cary, North Carolina.">

	<meta name="Keywords" content="Carolina Preserve, Amberly, Cary, North Carolina">

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
						<td><img src="/images/left-pix/preserve-sign.jpg" alt="" width="290" height="210" border="0"><img src="/images/right-pix/rt-tours.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Carolina Preserve at Amberly Profile</h1>
				
						<!-- to be added 
						
						<table border="0" class="faq" summary="Carolina Preserve at Amberly">
							<tr>
				    			<td>
									City: Cary, North Carolina
								</td>
							</tr>
							<tr>
								<td>
									Address: 
								</td>
							</tr>
						</table>
						
						-->
							
						<h2>Neighborhood Facts:</h2>
						
						<ul class="whylist">
							<li class="why">
								Your own personal retreat.
							</li>
							<li class="why">
								A dynamic environment filled with new opportunities to learn, grow and experience!
							</li>
						</ul>

						<p>Carolina Preserve by Del Webb at <a href="amberly.aspx">Amberly</a> will offer a lifestyle beyond anything in the area. Being one of the few 55+, active adult communities makes Carolina Preserve a sought after place for the many adults &amp; retirees coming to North Carolina! With their incredible amenity package and staff available to plan community events, you will find Carolina Preserve a perfect place to live.</p> 
						
						<p>In Carolina Preserve, the residents will have exclusive use of a 30,000 square foot clubhouse. The upper level will include a state-of-the-art fitness area containing strength and cardiovascular equipment, a heated indoor pool with locker rooms, a social lounge area, library, and several multipurpose rooms including a stadium kitchen. Take the stairs or the elevator to the lower level to enjoy the Varsity Room, a perfect place for residents to cheer on local teams during ACC Tournaments and other sporting events. The lower level will also include an aerobics/yoga studio, a multi-purpose room, and a small kitchen for serving parties indoors or out. Included in outdoor activities are a scenic walking trail system, tennis courts, Bocce courts, chipping and putting green, outdoor pool, and covered outdoor pavilion overlooking our 15-acre lake.</p> 
						
						<p>Carolina Preserve is located in Amberly - the Triangle's premier master planned community. Amberly is designed to be both your own personal retreat and a dynamic environment filled with new opportunities to learn, grow and experience. The 12,000 square foot Residents Club is a stylish, multi-use center. It will feature a fitness center, event lawn, amphitheatre, Jr. Olympic pool, kid's pool, and adult pool. Amberly will also feature a Village Center offering small shops at the center of the community, and a Town Center with additional shopping, entertainment, restaurants and office space. Being a Carolina Preserve resident offers you the best of both worlds, access to your exclusive Del Webb clubhouse to enjoy with your peers, and access to the Amberly clubhouse.</p>
						
						<p>Directions to Carolina Preserve:</p>
						
						<p>From I40 east/ Durham &amp; Chapel Hill: Take Hwy 55 (exit 278) toward NC-54/Apex. Turn right onto NC55. Turn slight right onto S. Alston Ave. Turn right onto Green Level Durham Rd. Turn right onto Carpenter Fire Station Rd. From I40 West and Raleigh: Take exit 285, Aviation Pkwy toward Morrisville/RDU International Airport. Turn left onto Aviation Pkwy. Aviation Pkwy becomes Morrisville Carpenter Rd. Turn right onto Carpenter Fire Station, right on Yates Home Rd.</p>

						
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
