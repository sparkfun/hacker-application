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
	<title>Home Warranty Advice for Buyers from Chris Edwards with RE/MAX United Cary, North Carolina.</title>
	
	<meta name="Description" content="Home warranty advice for buyers from Chris Edwards with RE/MAX United in Cary, North Carolina.">

	<meta name="Keywords" content="Home Warranty, Chris Edwards, RE/MAX United, Cary, North Carolina">

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
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate2.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-buyer.jpg" alt="Contact Chris Edwards for Home Buyer Assistance in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Home Warranties Advice for Buyers</h1>
									
				<p>A home warranty policy protects you from paying for repairs or replacements caused by the unexpected failure of a major system or appliance. You'll pay a small price for this warranty at closing. And get a policy that protects you for a full year.</p>
				
				<p>Your home warranty provides:</p>
				
				<p><img src="/images/bullet_hl.gif" alt="" width="13" height="8" border="0">Assurance and financial protection that you won't have to pay for any unexpected major repairs.</p>
				
				<p><img src="/images/bullet_hl.gif" alt="" width="13" height="8" border="0">The convenience of being able to call the home warranty service 24/7 to schedule a repair if something does break down (rather than having to wait for a repair shop to open).</p>
				
				<p>A good way to avoid unexpected repairs, in addition to a home inspection, is to:</p>
				
				<p><img src="/images/bullet_hl.gif" alt="" width="13" height="8" border="0">Visit the home at different times of day (listen for traffic and noise).</p>
				
				<p><img src="/images/bullet_hl.gif" alt="" width="13" height="8" border="0">Flush all toilets.</p>
				
				<p><img src="/images/bullet_hl.gif" alt="" width="13" height="8" border="0">Turn on lights.</p>
				
				<p><img src="/images/bullet_hl.gif" alt="" width="13" height="8" border="0">Run the water.</p>
				
				<p>Contact the city to check for permits to be sure all parts of the house meet safety code standards.</p>
				
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
