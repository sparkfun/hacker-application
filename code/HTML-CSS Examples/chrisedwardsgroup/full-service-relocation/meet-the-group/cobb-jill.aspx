<%@ Register Tagprefix="Top" Tagname="FeaturedProperty" Src="/includes/featured.ascx" %>
<%@ Register Tagprefix="Top" Tagname="TopLinks" Src="/includes/toplinks.ascx" %>
<%@ Register Tagprefix="Top" Tagname="LeftLinks" Src="/includes/leftlinks.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Address" Src="/includes/address.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Neighborhoods" Src="/includes/neighborhoods-group.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Logos" Src="/includes/logos.ascx" %>
<%@ Register Tagprefix="Bottom" Tagname="Footer" Src="/includes/footer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
	"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
	<title>Meet the Chris Edwards Group - Jill Cobb - Full Service Relocation to Raleigh, Cary and Apex, NC</title>
	
	<meta name="Description" content="Meet the Chris Edwards Group - Full Service Relocation to Raleigh, Cary and Apex, NC.">

	<meta name="Keywords" content="Meet the Chris Edwards Group, Raleigh Relocation, North Carolina Relocation, Full Service Relocation">

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
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate4.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-relocation.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Meet Jill A. Cobb, Edward Jones Investments</h1>
				
				<a href="http://www.edwardjones.com/cgi/getHTML.cgi?page=/USA/home/index.html" target="_blank"><img src="/images/group-photos/edward-jones-logo.jpg" alt="" width="220" height="44" border="0" id="photo" /></a>
				
				<table id="address">
					<tr>
						<td>
							<strong>Jill A. Cobb</strong><br />
							Investment Representative<br />
							Edward Jones Investments<br />
							919 Kildaire Farm Road<br />
							Cary, NC 27511<br /><br />
							
							<strong>Contact information for Jill Cobb:</strong><br />
							Office: 919-465-7466<br />
							Office: 1-866-465-7468<br />
							Fax: 1-866-360-7860<br />
							E-mail: <a href="mailto:Jill.Cobb@edwardjones.com">Jill.Cobb@edwardjones.com</a><br />
							Jill's Personal Web Page:<br />
							<a href="http://www.edwardjones.com/cgi/getHTML.cgi?page=/USA/IR/market/index.html&CIRN=712724" target="_blank">http://www.edwardjones.com/cgi/</a>
						</td>
					</tr>
				</table>
				
				<p>At Edward Jones, we believe in doing business face to face.  We want to get to know your unique financial objectives so we can develop a strategy to help you achieve your long-term financial goals.  So, whether you want to plan for retirement, save for college, reduce your tax bills or just acquire extra income to meet monthly expenses, you can expect personalize service and information on investing in quality investments for the long-term.  Contact me today to discuss your financial goals.</p>
				
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
