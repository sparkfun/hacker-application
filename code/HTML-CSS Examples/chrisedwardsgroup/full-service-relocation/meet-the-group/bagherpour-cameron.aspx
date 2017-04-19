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
	<title>Meet the Chris Edwards Group - Cameron Bagherpour - Full Service Relocation to Raleigh, Cary and Apex, NC</title>
	
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
			
				<h1>Meet Cameron Bagherpour, Allstate Insurance Company</h1>
				
				<img src="/images/group-photos/cameron-bagherpour.jpg" alt="" width="133" height="200" border="0" id="photo" />
				
				<a href="http://www.allstate.com/LandingPages/Auto/Q2_Auto_A.aspx?src=GOO&Campaign=222220000002430&CMP=KNC-Google-G03&HBX_PK=Allstate+insurance&HBX_OU=50" target="_blank"><img src="/images/group-photos/allstate-logo.jpg" alt="" width="150" height="112" border="0" id="photo" /></a>
				
				<table id="address">
					<tr>
						<td>
							<strong>Cameron Bagherpour</strong><br />
							Agent / Owner<br />
							Allstate Insurance Company<br />
							150 Cornerstone Drive, Suite 102<br />
							Cary, NC 27519<br /><br />
							
							<strong>Contact information for Cameron Bagherpour:</strong><br />
							<a href="mailto:cameronb@allstate.com">cameronb@allstate.com</a><br />
							<a href="http://agent.allstate.com/cameronb/Welcome/" target="_blank">http://agent.allstate.com/cameronb/</a><br />
							Office: 919-460-0606<br />
							Cell: 919-434-9001<br />
							Fax: 919-460-0212
						</td>
					</tr>
				</table>
			
				<p>As an Allstate Agent in Cary, Cameron knows many local families. His knowledge and understanding of the people in this community help provide customers with an outstanding level of service. He looks forward to helping families like yours protect the things that are important - your family, home, car, boat, and more.</p>
	
				<p>Cameron can provide assistance with; auto, home, business &amp; life insurance. In addition, he can also provide financial services like annuities &amp; IRA’s</p>
				
				<p>The Allstate office of Cameron Bagherpour is conveniently located in the Preston area of Cary, NC. (the corner of High House and Davis Drive)</p>

				<p><a href="http://agent.allstate.com/cameronb/AgencyInfo/MyBackGround.ASPX" target="_blank">Click here to receive an on-line quote</a></p>
				
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
