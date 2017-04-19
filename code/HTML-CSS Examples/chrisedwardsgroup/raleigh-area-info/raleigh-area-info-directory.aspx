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
	<title>Raleigh Area Information - Helpful Links Pertaining to North Carolina</title>
	
	<meta name="Description" content="Directory of links for Raleigh, Cary, Apex and Wake County, North Carolina.">

	<meta name="Keywords" content="Raleigh, Cary, Apex, Wake County, Links, Link Directory">

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
			
				<h1>Raleigh Area Information</h1>
					
				<p>Here are some helpful links pertaining to information in Raleigh, Cary, Apex and surrounding towns. Included is some information about Wake County and other items of interest in the beautiful Triangle Area of North Carolina!</p>
				
				<table class="directory" summary="Directory of website links for the Raleigh, Cary and Apex, North Carolina area.">
					<tr>
						<td>
							<p><a href="arts.aspx"><strong>Arts</strong></a></p>
							<p>Links to Raleigh Area Art Galleries and Entertainment Information</p>
						</td>
						<td>
							<p><a href="chamber-of-commerce.aspx"><strong>Chambers of Commerce</strong></a></p>
							<p>Links to Chambers of Commerce for Raleigh, Cary, Apex and surrounding towns</p>
						</td>
					</tr>
					<tr>
						<td>
							<p><a href="city-county-town-govt.aspx"><strong>City and County Governments</strong></a></p>
							<p>Links to Raleigh Area City and County Governments</p>
						</td>
						<td>
							<p><a href="clubs.aspx"><strong>Clubs and Interest Groups</strong></a></p>
							<p>Links to Raleigh Area Clubs, Historical Sites and Interest Groups</p>
						</td>
						
					</tr>
					<tr>
						<td>
							<p><a href="vehicle-driver.aspx"><strong>Department of Motor Vehicles</strong></a></p>
							<p>Links and Resources to Vehicle Registration and Drivers License Information with the North Carolina Department of Motor Vehicles</p>
						</td>
						<td>
							<p><a href="education.aspx"><strong>Education</strong></a></p>
							<p>Links and Resources to Raleigh Area public and private schools</p>
						</td>
					</tr>
					<tr>
						<td>
							<p><a href="employment.aspx"><strong>Employment</strong></a></p>
							<p>Links and Resources for finding jobs in Raleigh, NC and the Triangle Area</p>
						</td><td>
							<p><a href="entertainment.aspx"><strong>Entertainment and Newspapers</strong></a></p>
							<p>Links to entertainment and newspapers in the Raleigh, Cary and Apex region</p>
						</td>
					</tr>
					<tr>
						<td>
							<p><a href="golf.aspx"><strong>Golf Courses</strong></a></p>
							<p>Links and Addresses of Golf Courses in the surrounding area.</p>
						</td>
						<td>
							<p><a href="raleigh-hotel-motel.aspx"><strong>Hotels & Motels</strong></a></p>
							<p>Links and addresses to Hotels and Motels in the Raleigh-Durham-Chapel Hill Area.</p>
						</td>
						
					</tr>
					<tr>
						<td>
							<p><a href="historical-sites-raleigh-north-carolina.aspx"><strong>Historical Sites</strong></a></p>
							<p>Links and addresses to Historical Sites near Raleigh, Chapel Hill and Durham.</p>
						</td>
						<td>
							<p><a href="internet-web.aspx"><strong>Internet/Web</strong></a></p>
							<p>Links to Internet and website related information in the Apex, Cary, Durham, Raleigh and Wake Forest Area.</p>
						</td>
						
					</tr>
					<tr>
						<td>
							<p><a href="parks-triangle.aspx"><strong>Parks and Recreation</strong></a></p>
							<p>Links to Parks and Lakes in the Apex, Cary, Durham, Raleigh and Wake Forest Area</p>
						</td>
						<td>
							<p><a href="museums.aspx"><strong>Museums</strong></a></p>
							<p>Links to Museums in the Apex, Cary, Durham, Raleigh and Wake Forest Area</p>
						</td>
						
					</tr>
					<tr>
						<td>
							<p><a href="restaurants.aspx"><strong>Restaurants</strong></a></p>
							<p>Links to some of the fine restaurants in the surrounding area</p>
						</td>
						<td>
							<p><a href="state-govt.aspx"><strong>State Government</strong></a></p>
							<p>Links and Resources to North Carolina State Government</p>
						</td>
					</tr>
					<tr>
						<td>
							<p><a href="sports.aspx"><strong>Sports</strong></a></p>
							<p>Links to some of the sports teams and events in the surrounding area</p>
						</td>
						<td>
							&nbsp;
						</td>
					</tr>
				</table>
				
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
