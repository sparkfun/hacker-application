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
	<title>Moore Square Historic District, North Carolina - Chris Edwards RE/MAX United</title>
	
	<meta name="Description" content="Description of the Moore Square Historic District in Raleigh, North Carolina.">

	<meta name="Keywords" content="Raleigh, Moore Square Historic District, North Carolina">

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
			
				<h1>Moore Square Historic District Profile</h1>
				
						<!-- to be added 
						
						<table border="0" class="faq" summary="Moore Square Neighborhood">
							<tr>
				    			<td>
									City: Holly Springs, North Carolina
								</td>
							</tr>
							<tr>
								<td>
									Address: Holly Springs Road &amp; Linksland
								</td>
							</tr>
						</table>
						
						-->
							
						<h2>Neighborhood Facts:</h2>
						
						<ul class="whylist">
							<li class="why">
								Age the late 1800s
							</li>
							<li class="why">
								Visit the giant copper acorn!
							</li>
						</ul>

						<p>No houses here, but the Moore Square Historic District is home to one of two parks that are still around from the original city plan mapped out in 1792. Today Moore Square is best known as the site of both the Artsplosure festival and the giant copper acorn. Sure, you can go to Raleigh without visiting the giant copper acorn. Suit yourself. Seems a bit churlish, though. Too good to have your picture taken with a giant copper acorn? The giant copper acorn weighs a half ton. The significance of the giant copper acorn is that Raleigh is sometimes called the &quot;City of Giant Copper Oaks.&quot; Well, actually it's just the &quot;City of Oaks&quot; that Raleigh is sometimes called. Visit the giant copper acorn.</p>
						
						<p>In the late 1800s, Moore Square developed into one of the city's primary commercial hubs. City Market arrived in 1914, and in the 1920s, with post-Civil War segregation now firmly entrenched, the area became known as &quot;Black Main Street.&quot;</p>
						
						<p>By the later part of the 20th century, though, suburbanization had taken a toll on the Moore Square area. But revitalization efforts - most significantly the redevelopment of City Market and the arrival of Exploris - have resulted in a return of activity to the area.</p>
						
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
