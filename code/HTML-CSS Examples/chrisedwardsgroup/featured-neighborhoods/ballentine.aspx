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
	<title>Ballentine Neighborhood of Cary, North Carolina - Chris Edwards RE/MAX United</title>
	
	<meta name="Description" content="Description of the Ballentine neighborhood in Raleigh, North Carolina.">

	<meta name="Keywords" content="Raleigh, Ballentine, North Carolina">

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
						<td><img src="/images/left-pix/ballentine-sign.jpg" alt="" width="290" height="210" border="0"><img src="/images/right-pix/rt-tours.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Ballentine Neighborhood Profile</h1>
						
						<table border="0" class="faq" summary="Barrington &amp; Regency Park Estates Neighborhood">
							<tr>
				   				<td>
									City: Fuquay Varina, North Carolina
								</td>
							</tr>
							<tr>
				   				<td>
									Map:<br />
									<img src="/images/ballentine-map.jpg" alt="" width="225" height="189" border="0">
								</td>
							</tr>
						</table>
							
						<h2>Neighborhood Facts:</h2>
						
						<ul class="whylist">
							<li class="why">
								Age range is 2000-2006
							</li>
							<li class="why">
								Price range is $200,000 - $300,000
							</li>
							<li class="why">
								Located off Sunset Lake Road in Fuquay Varina
							</li>
						</ul>
						
						<p>Ballentine is one of our favorite Fuquay Varina neighborhoods due to the variety of homes and convenient location to lots of amenities! This charming community features pool &amp; clubhouse and it's own elementary school right in the neighborhood! </p>

						<p>Ballentine offers mostly re-sale homes from several different builders, which means you will find of wide selection of home sizes, styles and prices! A left turn on Sunset Lake Road gets you to Downtown Fuquay and HWY 401, which are full of shopping & restaurants!</p>

						<p><strong>Ballentine Neighborhood Information:</strong></p>
						
						<p>
							<a href="http://ballentine.wcpss.net/" target="_blank">Ballentine Elementary School</a> (grades KG-05)<br />
							1600 McLaurin Lane<br />
							Fuquay-Varina, NC 27526<br />
							(919) 557-1120 
						</p>
						
						<p>
							<a href="http://www.wcpss.net/school-directory/424.html" target="_blank">Fuquay Varina Middle School</a> (grades 06-08)<br />
							104 N Woodrow St.<br />
							Fuquay-Varina, NC 27526-2045<br />
							(919) 557-2727 
						</p>
						
						<p>
							<a href="http://fvhs.wcpss.net/" target="_blank">Fuquay Varina High School</a> (grades 09-12)<br />
							201 Bengal Blvd.<br />
							Fuquay-Varina, NC 27526-1603<br />
							(919) 557-2511 
						</p>
						
						<p><strong>Utilities:</strong></p>
						
						<p>
							Electric: Progress Energy<br />
							(919) 508-5400
						</p>
						
						<p>
							Gas: PSNC<br />
							(877) 766-2427 
						</p>
						
						<p>
							Phone:Sprint<br />
							(800) 786-6272 
						</p>
						
						<p>
							Cable/Internet: Road-Runner/Time Warner <br />
							(919) 585-4892
						</p>
						
						<p>
							Water/Trash/Recycling:Town of Fuquay<br />
							(919) 552-1405 
						</p>
						
						<p></p>
						
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
