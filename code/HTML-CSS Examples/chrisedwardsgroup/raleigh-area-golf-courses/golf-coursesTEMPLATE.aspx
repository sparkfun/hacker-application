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
	<title>Raleigh, North Carolina Public and Semi-Private Golf Courses: Raleigh - Cary - Apex - Research Triangle Park - Wake County - Triangle, NC Real Estate Agent Chris Edwards</title>
	
	<meta name="Description" content="Raleigh, North Carolina Public and Semi-Private Golf Courses by Chris Edwards with RE/MAX United in Cary, North Carolina.">

	<meta name="Keywords" content="">

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
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate4.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-link-directory.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Raleigh, North Carolina Public and Semi-Private Golf Courses</h1>
				
				<table id="recruiters">
					<tbody>
						
						<tr>
							<th>Course</th>
							<th>Par</th>
							<th>Designer(s)</th>
							<th>Yards</th>
							<th>Built</th>
						</tr>
						
						<tr>
							<td class="course">
								<strong></strong><br />
								<br />
								<br />
								
							</td>
							<td class="other"></td>
							<td class="designer"><br /></td>
							<td class="other"></td>
							<td class="other"></td>
						</tr>
						
						<tr>
							<td class="course">
								<strong></strong><br />
								<br />
								<br />
								
							</td>
							<td class="other"></td>
							<td class="designer"><br /></td>
							<td class="other"></td>
							<td class="other"></td>
						</tr>
						
						<tr>
							<td class="course">
								<strong></strong><br />
								<br />
								<br />
								
							</td>
							<td class="other"></td>
							<td class="designer"><br /></td>
							<td class="other"></td>
							<td class="other"></td>
						</tr>
						
						<tr>
							<td class="course">
								<strong></strong><br />
								<br />
								<br />
								
							</td>
							<td class="other"></td>
							<td class="designer"><br /></td>
							<td class="other"></td>
							<td class="other"></td>
						</tr>
						
						<tr>
							<td class="course">
								<strong></strong><br />
								<br />
								<br />
								
							</td>
							<td class="other"></td>
							<td class="designer"><br /></td>
							<td class="other"></td>
							<td class="other"></td>
						</tr>
						
					</tbody>
				</table>
				
				<h2><a href="/full-service-relocation/meet-the-group/vinesett-rich.aspx">Meet Rich Vinesett, CEG Co-Associate</a><br />
				Relocation &amp; Golf Community Specialist!</h2>
				
				<p>Rich brings fun, care, and professional service to each client that comes to him for real estate service in the Triangle! He was born and raised in North Carolina and is proud to assist you with a home sale or purchase in a golf community in <strong>Raleigh, Cary, Apex, Holly Springs, The Research Triangle Park area, Wake Forest, Garner, Willow Spring, Chapel Hill, Durham, Knightdale &amp; Clayton, North Carolina</strong>.</p>
				
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
