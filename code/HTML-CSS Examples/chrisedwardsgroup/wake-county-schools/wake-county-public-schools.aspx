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
	<title>Wake County Public Schools Directory</title>
	
	<meta name="Description" content="Directory of links for Wake County Public Schools, North Carolina.">

	<meta name="Keywords" content="Wake County Public Schools, Raleigh, Cary, Apex, Wake County, Links, Link Directory">

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
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate3.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-schools.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Wake County Public Schools Directory</h1>
					
				<p class="bold">Here are some helpful links pertaining to information about the Wake County Public Schools.</p>
				
				<table class="directory" summary="Wake County Public Schools Directory.">
					<tr>
						<td>
							<p><a href="http://www.wcpss.net/" target="_blank"><strong>Wake County Public Schools Website</strong></a></p>
							<p>Link to main website for the Wake County Public Schools</p>
						</td>
						<td>
							<p><a href="http://www.wcpss.net/magnet/index.html" target="_blank"><strong>Wake County Magnet Programs</strong></a></p>
							<p>Link to website for the Magnet Programs in Wake County Public Schools</p>
						</td>
					</tr>
					<tr>
						<td>
							<p><a href="http://www.wcpss.net/newcomer/getting-started/registration/" target="_blank"><strong>Wake County Public Schools Registration</strong></a></p>
							<p>Link to website for Wake County Public School Registration</p>
						</td>
						<td>
							<p><a href="http://www.wcpss.net/history/index.html" target="_blank"><strong>History of Wake County Public Schools</strong></a></p>
							<p>Link to website for the history of Wake County Public Schools</p>
						</td>
					</tr>
					<tr>
						<td>
							<p><a href="wake-county-school-profile-2002.aspx"><strong>School Profiles - 2001-2002</strong></a></p>
							<p>Links to PDF documents of profiles for schools in the Wake County Public School system</p>
						</td>
						<td>
							<p><a href="wake-county-school-profile-2003.aspx"><strong>School Profiles - 2002-2003</strong></a></p>
							<p>Links to PDF documents of profiles for schools in the Wake County Public School system</p>
						</td>
					</tr>
					<tr>
						<td>
							<p><a href="wake-county-school-profile-2005.aspx"><strong>School Profiles - 2004-2005</strong></a></p>
							<p>Links to PDF documents of profiles for schools in the Wake County Public School system</p>
						</td>
						<td>
							<p><a href="wake-county-school-profile-2006.aspx"><strong>School Profiles - 2006-2007</strong></a></p>
							<p>Links to PDF documents of profiles for schools in the Wake County Public School system</p>
						</td>
					</tr>
				</table>
				
				<p><span class="bold">Link Trade</span> - Would you like to exchange links with us?  We accept high quality link partners.  All link partners will be evaluated based on relative content and quality.		We require a reciprocal link on a comparable website page.  To setup a reciprocal link <a href="mailto:webmaster@chrisedwardsgroup.com">e-mail the webmaster</a>.</p>
					
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
