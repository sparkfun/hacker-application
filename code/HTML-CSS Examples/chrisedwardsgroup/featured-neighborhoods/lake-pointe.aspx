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
	<title>Lochmere Lake Pointe Neighborhood of Cary, North Carolina.</title>
	
	<meta name="Description" content="Description of the Lake Pointe neighborhood in Cary, North Carolina.">

	<meta name="Keywords" content="Lake Pointe, Cary, North Carolina">

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
						<td><img src="/images/left-pix/lakepointe-sign.jpg" alt="" width="290" height="210" border="0"><img src="/images/right-pix/rt-tours.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Lochmere Lake Pointe Neighborhood Profile</h1>
						
						<table border="0" class="faq" summary="Lake Pointe Neighborhood">
							<tr>
				    <td>
									City: Cary, North Carolina
								</td>
							</tr>
							<tr>
								<td>
									Address: Tryon Rd to Summerpoint, Right on Loch Pointe Drive
								</td>
							</tr>
							<tr>
								<td>
									<a href="http://www.mapquest.com/maps/map.adp?location=j1EYoQCVn843VhPgqmldrYPbv9sITLqxXmd8gBe2A4v3jMxolBqiIxtnDUiBt2rMgSRrltMgZ2ke21jy0D3qTF79nT3HQMsboDnBu8o5zmg%3d&address=loch%20pointe&city=cary&state=nc&zipcode=27511&country=US&addtohistory=&submit=Get%20Map" target="_blank">Map link to neighborhood</a> (click link)
								</td>
							</tr>
						</table>
							
						<h2>
							Neighborhood Facts:
						</h2>
						
						<ul class="whylist">
							<li class="why">
								19 homes in subdivision
							</li>
							<li class="why">
								Age range is 1996 - 1998
							</li>
							<li class="why">
								Price range is $436,000 - $499,000
							</li>
							<li class="why">
								Large, custom homes
							</li>
							<li class="why">
								Great Cary Location!
							</li>
						</ul>
						
						<p>Lake Pointe is a one street section of the popular Lochmere subdivisions in Cary.  Like Summer Pointe, Lake Pointe is a newer section of Lochmere offering custom, estate homes with a premium Cary location. Lochmere Lake Pointe is minutes from Western Wake Hospital, plenty of shopping and restaurants and major highways!</p>


						
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
