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
	<title>Meet the Chris Edwards Group - Jim Moore - Full Service Relocation to Raleigh, Cary and Apex, NC</title>
	
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
			
				<h1>Meet Jim Moore, Home Mortgage Consultant</h1>
				
				<img src="/images/group-photos/moore-jim.jpg" alt="" width="183" height="210" border="0" id="photo" />
				
				<img src="/images/group-photos/capital-mortgage-logo.jpg" alt="" width="162" height="127" border="0" id="photo" />
				
				<table id="address">
					<tr>
						<td>
							<strong>Jim Moore</strong><br />
							Home Mortgage Consultant<br />
							First Capital Mortgage*, LLC<br />
							51 Kilmayne Drive<br />
							Cary, NC 27511<br /><br />
							
							*an affiliate of:<br />
							Wells Fargo Home Mortgage<br />
							MAC M5533-011<br /><br />
							
							<strong>Contact Information for Jim Moore</strong><br />
							<a href="mailto:Jim.r.moore@wellsfargo.com">Jim.r.moore@wellsfargo.com</a><br />
							Office: 919-469-6558<br />
							Cell: 919-523-9979<br />
							Fax: 919-465-0552<br />
							Apply for a loan now at:<br />
							<a href="http://www.homeloans.com/jim-moore1" target="_blank">http://www.homeloans.com/jim-moore1</a>
						</td>
					</tr>
				</table>
				
				<p>Jim Moore and his staff represent Wells Fargo Products and offer a broad range of programs to help you quickly and easily settle into your new home. First Capital Mortgage, LLC is an affiliate of Wells Fargo Home Mortgage and is conveniently located here at RE/MAX United.</p>

				<p>Ask Jim about a &quot;Tax Smart&quot; mortgage. They never charge an origination fee, or &quot;junk&quot; fees! Competitive rates, low cost &amp; no cost closing cost mortgages are available.</p>

				<p>Call Jim today for a free pre-qualification consultation or pre-approval to give you the needed documentation to make a solid offer!</p>
				
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
