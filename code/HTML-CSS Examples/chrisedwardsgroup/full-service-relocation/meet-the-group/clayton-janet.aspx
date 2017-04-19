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
	<title>Meet the Chris Edwards Group - Janet Clayton - Full Service Relocation to Raleigh, Cary and Apex, NC</title>
	
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
			
				<h1>Meet Janet Clayton, Commercial Real Estate Advisor</h1>
				
				<img src="/images/group-photos/janet-clayton.jpg" alt="" width="144" height="200" border="0" id="photo" />
				
				<a href="http://www.tlgcre.com/about.html" target="_blank"><img src="/images/group-photos/grubb-ellis-logo.jpg" alt="" width="327" height="75" border="0" id="photo" /></a>
				
				<table id="address">
					<tr>
						<td>
							<strong>Janet Clayton</strong><br />
							Real Estate Advisor<br />
							Grubb &amp; Ellis TLG<br />
							1511 Sunday Drive, Suite 200<br />
							Raleigh, NC 27607<br /><br />
							
							<strong>Contact information for Janet Clayton</strong><br />
							<a href="mailto:Janet.clayton@tlgcre.com">Janet.clayton@tlgcre.com</a><br />
							Direct: 919-420-1581<br />
							Cell: 919-306-0405<br />
							Main: 919-785-3434<br />
							Fax: 919-785-0802<br />
							<a href="http://www.tlgcre.com/" target="_blank">www.TLGCRE.com</a> Independently Owned and Operated
						</td>
					</tr>
				</table>
				
				<strong><p>Relationships Built On Results</p></strong>
				
				<p>Janet L. Clayton, MBA, is a North Carolina licensed real estate broker with Grubb &amp; Ellis|Thomas Linderman Graham. She specializes in tenant and buyer representation for individuals and organizations seeking office, flex and industrial space in the Triangle region (Raleigh, Durham, Chapel Hill). Her extensive business experience provides a valuable benefit to her clients in the identification and analysis of leasehold and purchase opportunities.</p>

				<p>Having Janet as our commercial partner give us the ability to better serve more clientele who also need to relocate their business to the Triangle area.</p>
				
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
