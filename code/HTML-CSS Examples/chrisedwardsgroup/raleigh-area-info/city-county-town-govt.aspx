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
	<title>North Carolina City, Town and Government</title>
	
	<meta name="Description" content="City, county and town links for Wake County, North Carolina.">

	<meta name="Keywords" content="Raleigh, Cary, Wake County, City Links, County Links, Town Links">

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
			
				<h1>North Carolina City, Town and Government</h1>
					
				<p>Here are some helpful links to city, town and government websites in North Carolina.</p>
				
				<h2>Wake County</h2>
				
				<ul class="whylist">
					<li class="why">
						<a href="http://www.ci.apex.nc.us/" target="_blank">Town of Apex</a>
					</li>
					<li class="why">
						<a href="http://www.townofcary.org/" target="_blank">Town of Cary</a>
					</li>
					<li class="why">
						<a href="http://www.ci.chapel-hill.nc.us/" target="_blank">Town of Chapel Hill</a>
					</li>
					<li class="why">
						<a href="http://www.ci.durham.nc.us/" target="_blank">City of Durham</a>
					</li>
					<li class="why">
						<a href="http://www.fuquay-varina.org/" target="_blank">Town of Fuquay-Varina</a>
					</li>
					<li class="why">
						<a href="http://www.garner.nc.us/" target="_blank">Town of Garner</a>
					</li>
					<li class="why">
						<a href="http://www.hollyspringschamber.org/" target="_blank">Town of Holly Springs</a>
					</li>
					<li class="why">
						<a href="http://www.ci.knightdale.nc.us/" target="_blank">Town of Knightdale</a>
					</li>
					<li class="why">
						<a href="http://www.ci.morrisville.nc.us/" target="_blank">Town of Morrisville</a>
					</li>
					<li class="why">
						<a href="http://www.raleigh-nc.org/" target="_blank">City of Raleigh</a>
					</li>
					<li class="why">
						<a href="http://www.wakeforestnc.com/" target="_blank">Town of Wake Forest</a>
					</li>
					<li class="why">
						<a href="http://www.townofwendell.com/" target="_blank">Town of Wendell</a>
					</li>
					<li class="why">
						<a href="http://www.ci.zebulon.nc.us/" target="_blank">Town of Zebulon</a>
					</li>
					<li class="why">
						<a href="http://www.wakegov.com/" target="_blank">Wake County Government</a>
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
