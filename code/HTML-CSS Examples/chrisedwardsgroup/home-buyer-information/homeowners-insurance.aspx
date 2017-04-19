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
	<title>Homeowner's Insurance Advice from Chris Edwards with RE/MAX United Cary, North Carolina.</title>
	
	<meta name="Description" content="Homeowner's insurance advice from Chris Edwards with RE/MAX United in Cary, North Carolina.">

	<meta name="Keywords" content="Homeowner's Insurance, Chris Edwards, RE/MAX United, Cary, North Carolina">

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
			
				<h1>Homeowner's Insurance</h1>
									
				<p>Homeowner's insurance protects you and your house against losses from fire, theft, liability, vandalism, water damage, wind damage, tornadoes, and loss of use. You can buy earthquake and flood insurance separately.</p>
				
				<p><img src="/images/bullet_hl.gif" alt="" width="13" height="8" border="0">A standard policy requires coverage for at least 80% of the value of your home, excluding land and the foundation. It will usually insure your personal property at actual cash value.</p>
				
				<p><img src="/images/bullet_hl.gif" alt="" width="13" height="8" border="0">A broad-form policy covers additional things like broken glass and smoke damage.</p>
				
				<p><img src="/images/bullet_hl.gif" alt="" width="13" height="8" border="0">An all-risk policy covers even more. For example: ice-damaged roofs.</p>
				
				<p><img src="/images/bullet_hl.gif" alt="" width="13" height="8" border="0">You can also purchase optional guaranteed replacement cost coverage. This will pay to rebuild your home and replace its contents with no depreciation.</p>
				
				<p><em>Reducing Premiums</em> - If cost is an issue, you can lower your premiums:</p>
				
				<p><img src="/images/bullet_hl.gif" alt="" width="13" height="8" border="0">With discounts for smoke alarms, fire extinguishers, deadbolt locks, and whole-house alarm systems</p>
				
				<p><img src="/images/bullet_hl.gif" alt="" width="13" height="8" border="0">With a higher deductible</p>
				
				<p><img src="/images/bullet_hl.gif" alt="" width="13" height="8" border="0">By combining your auto and homeowner's policies</p>
				
				<p>Be sure to discuss your options with your insurance agent. Chris Edwards has experience with insurance agents and can make a referral, just <a href="mailto:chris@chrisedwardsgroup.com">click here</a> and ask Chris the name.</p>
								
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
