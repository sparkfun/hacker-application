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
	<title>Raleigh Museums Links</title>
	
	<meta name="Description" content="Museum links for Raleigh, Cary and Wake County, North Carolina.">

	<meta name="Keywords" content="Museums, Raleigh, Cary, Wake County, City Links, Museum Links">

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
			
				<h1>Raleigh Museum Links</h1>
					
				<p>Here are some helpful links pertaining to Museums in the Raleigh, Cary and Apex area.</p>
				
				<ul class="whylist">
					<li class="why">
						<a href="http://www.ackland.org" target="_blank">Ackland Art Museum</a>
					</li>
					<li class="why">
						<a href="http://www.americandancefestival.org/" target="_blank">American Dance and Music Festival</a>
					</li>
					<li class="why">
						<a href="http://www.avagardner.org" target="_blank">Ava Gardner Museum</a>
					</li>
					<li class="why">
						<a href="http://www.camnc.org" target="_blank">Contemporary Art Museum</a>
					</li>
					<li class="why">
						<a href="http://www.duke.edu/web/duma" target="_blank">Duke University Museum of Art</a>
					</li>
					<li class="why">
						<a href="http://www.moreheadplanetarium.org/" target="_blank">Morehead Panetarium</a>
					</li>
					<li class="why">
						<a href="http://www.nccu.edu/artmuseum/" target="_blank">North Carolina Central University Art Museum</a>
					</li>
					<li class="why">
						<a href="http://www.ncartmuseum.org" target="_blank">North Carolina Museum of Art</a>
					</li>
					<li class="why">
						<a href="http://ncmuseumofhistory.org/" target="_blank">North Carolina Museum of History</a>
					</li>
					<li class="why">
						<a href="http://www.herald-sun.com/ncmls" target="_blank">North Carolina Museum of Life &amp; Science</a>	
					</li>
					<li class="why">
						<a href="http://www.naturalsciences.org" target="_blank">North Carolina Museum of Natural Sciences</a>	
					</li>
					<li class="why">
						<a href="http://www.raleighcitymuseum.org/" target="_blank">Raleigh City Museum</a>	
					</li>
				</ul>
				
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
