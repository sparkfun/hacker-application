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
	<title>Danbury at Regency Neighborhood of Raleigh, North Carolina - Chris Edwards RE/MAX United</title>
	
	<meta name="Description" content="Description of the Danbury at Regency neighborhood in Raleigh, North Carolina.">

	<meta name="Keywords" content="Raleigh, Danbury at Regency, North Carolina">

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
						<td><img src="/images/left-pix/danbury-sign.jpg" alt="" width="290" height="210" border="0"><img src="/images/right-pix/rt-tours.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Danbury at Regency Neighborhood Profile</h1>
						
						<table border="0" class="faq" summary="Danbury Neighborhood">
							<tr>
				    <td>
									City: Cary, North Carolina
								</td>
							</tr>
							<tr>
								<td>
									Address: Located at Penny Road and Killingsworth
								</td>
							</tr>
						</table>
							
						<h2>Neighborhood Facts:</h2>
						
						<ul class="whylist">
							<li class="why">
								Age range is 1997-2002
							</li>
							<li class="why">
								Price range is $450,000-$500,000
							</li>
							<li class="why">
								Excellent Cary Location!
							</li>
							<li class="why">
								Next to Regency Park
							</li>
						</ul>
						
						<p>The neighborhoods that make up Regency are some of the most popular on the east side of Cary. They are; Danbury, Cambridge, Wyndfall, Kensington, Barrington &amp; The Estates at Regency.</p>
						
						<p>Danbury is at the first level of pricing, but still has the location and amenities that the Regency Park Area is famous for! You'll enjoy the a community pool, close proximity of restaurants, wonderful shopping, highway access, Regency Lake &amp; Amphitheater and the natural setting that surrounds the Regency area.</p>
						
						<p>Click the link below for more information on <strong>Regency</strong>:</p>
						
						<p><a href="http://www.regencycommunities.com/outside_frame.asp" target="_blank">http://www.regencycommunities.com/outside_frame.asp</a></p>

						<p>Regency Riptides Swimteam:</p>
						
						<p><a href="http://www.regencyriptides.com/" target="_blank">http://www.regencyriptides.com/</a></p>
						
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
