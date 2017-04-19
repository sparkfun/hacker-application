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
	<title>Research Triangle Park Employers - Raleigh, Cary, Apex, Research Triangle Park, Wake County, Triangle, NC Real Estate Agent Chris Edwards</title>
	
	<meta name="Description" content="Research Triangle Park Employers in the Raleigh, Cary, Apex, Research Triangle Park, Wake County, Triangle, NC area.">

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
			
				<h1>Research Triangle Park Employers</h1>
				<p>(Ranked by number of employees in RTP)</p>
				
				<ol>
					<li>IBM</li>
					<li>Glaxo Smith Kline, PLC</li>
					<li>Cisco Systems</li>
					<li>Nortel Networks</li>
					<li>RTI International</li>
					<li>U.S. Environmental Protection Agency</li>
					<li>National Institute of Environmental Health Services</li>
					<li>Sony Ericsson Mobile Communications</li>
					<li>Biogen IDEC</li>
					<li>Network Appliance</li>
					<li>BASF Corporation Agriculture Product Center</li>
					<li>Diosynth Biotechnology</li>
					<li>Bayer Cropscience</li>
					<li>Talecris Biotherapeutics</li>
					<li>EISAI, Inc.</li>
					<li>Dupont Electronic Technologies</li>
					<li>Credit Suisse</li>
					<li>Underwriters Laboratories, Inc.</li>
					<li>The UNC Center for Public Television</li>
					<li>BD Technologies</li>
					<li>EMC Corporation</li>
					<li>Syngenta Biotechnology, Inc.</li>
					<li>Sumitomo Electric Lightwave Corp.</li>
					<li>Triangle Transit Authority</li>
					<li>Lenovo</li>
				</ol>
				
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
