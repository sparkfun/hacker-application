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
	<title>Meet the Chris Edwards Group - Alex Macklin - Full Service Relocation to Raleigh, Cary and Apex, NC</title>
	
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
			
				<h1>Meet Alex Macklin, Crescent State Bank</h1>
				
				<img src="/images/group-photos/alex-macklin.jpg" alt="" width="211" height="222" border="0" id="photo" />
				
				<a href="http://www.crescentstatebank.com/" target="_blank"><img src="/images/group-photos/crescent-bank-logo.jpg" alt="" width="264" height="80" border="0" id="photo" /></a>
				
				<table id="address">
					<tr>
						<td>
							<strong>Alex Macklin</strong><br />
							VP Mortgage Services<br />
							Crescent State Bank<br />
							700 Holly Springs Road<br />
							Holly Springs, NC 27540<br /><br />
							
							<strong>Contact Information for Alex Macklin</strong><br />
							<a href="mailto:amacklin@crescentstatebank.com">amacklin@crescentstatebank.com</a><br />
							919-552-6590<br />
							919-552-7677<br />
						</td>
					</tr>
				</table>
				
				<p>Alex Macklin is our &quot;hometown bank&quot; mortgage consultant! You’ll enjoy solid and ultra professional service by letting Alex handle your mortgage needs.</p>

				<p>Alex is the Vice President of the in the Residential Mortgage Lending Division at Crescent State Bank and has over 12 years of banking experience which has given him the tools to provide the polished service and communication needed to give every customer a smooth transaction!</p>
				
				<p>His office is located in Holly Springs but he also can offer mortgage services in most of the continental United States. He strives to be the best in the business through commitment, effort, and rising to the challenges placed before him.</p>
				
				<p>When you choose to do business with Alex Macklin &amp; Crescent State Bank you can expect to receive:</p>
				
				<ul class="list">
					<li>Friendly assistance with a customer service attitude</li>
					<li>Professional processing and underwriting team with over 20 years of experience in residential lending</li>
					<li>Competitive Rates</li>
					<li>Many different loan programs to fit the client’s needs</li>
					<li>A full service branch to provide complete financial assistance that most mortgage brokers can’t offer</li>
				</ul>
				
				<p>Call Alex today for the service you deserve and require!</p>
				
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
