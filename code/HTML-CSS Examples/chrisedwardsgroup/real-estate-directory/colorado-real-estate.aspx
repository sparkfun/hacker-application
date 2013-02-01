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
	<title>Real Estate Agents Link Directory - Colorado</title>
	
	<meta name="Description" content="Real Estate agents Real Estate link directory.">

	<meta name="Keywords" content="Real Estate Links, Real Estate Agents">

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
			
				<h1>Real Estate Agents Link Directory - Colorado</h1>
					
				<p>Here are some helpful links pertaining to Real Estate and Real Estate agents in Colorado.</p>
				
				<h2>
					Colorado
				</h2>
				
				<ul class="whylist">
					<li class="why">
						<a href="http://www.cedaredge-realestate.com/" target="_blank">Western Colorado Cedaredge Real Estate</a> - Marsha Bryan with RE/MAX Mountain West in Cedaredge, Colorado.
					</li>
					<li class="why">
						<a href="http://www.gregstratman.com/" target="_blank">Cedaredge, CO Real Estate and Homes</a> - Greg Stratman with RE/MAX Mountain West in Cedaredge, Colorado.
					</li>
					<li class="why">
						<a href="http://www.grand-junction-homes.com/" target="_blank">Grand Junction Real Estate</a> - Jeptha Sheene with RE/MAX 4000, Inc. in Grand Junction, Colorado.
					</li>
					<li class="why">
						<a href="http://www.rmwrealestate.com/" target="_blank">Western Colorado Real Estate</a> - RE/MAX Mountain West with Offices in Carbondale, Cedaredge, Delta, Glenwood Springs, and Paonia.
					</li>
					<li class="why">
		    <a href="http://www.crabtreeproperties.com/" target="_blank">Grand Lake and Grand County Colorado Homes</a> - Contact Crabtree Properties, a full-service Grand Lake Real Estate office active in Grand County since 1969, for homes, land or other property in Grand Lake, Winter Park, Fraser, Granby, Silver Creek, Sol Vista and Hot Sulphur Springs.
		   </li>
					<li class="why">
						<a href="http://www.grandlakeresortproperties.com/" target="_blank">Grand Lake Real Estate</a> - RE/MAX in Grand Lake. Full service Real Estate in Grand Lake, Granby and Winter Park, Colorado.
					</li>
				</ul>
				
				<p><a href="real-estate.aspx">Back To Real Estate Link Directory</a></p>
				
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
