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
	<title>Real Estate Agents Link Directory</title>
	
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
			
				<h1>Real Estate Link Directory</h1>
					
				<p>Here are some helpful links pertaining to Real Estate and Real Estate agents across the United States.</p>
				
				<table border="0" class="profile">
					<tr><th colspan="3">Real Estate Agents</th></tr>
					<tr>
						<td class="school"><a href="california-real-estate.aspx">California</a></td>
						<td class="school"><a href="colorado-real-estate.aspx">Colorado</a></td>
						<td class="school"><a href="florida-real-estate.aspx">Florida</a></td>
					</tr>
					<tr>
						<td class="school"><a href="georgia-real-estate.aspx">Georgia</a></td>
						<td class="school"><a href="idaho-real-estate.aspx">Idaho</a></td>
						<td class="school"><a href="illinois-real-estate.aspx">Illinois</a></td>
					</tr>
					<tr>
						<td class="school"><a href="iowa-real-estate.aspx">Iowa</a></td>
						<td class="school"><a href="kentucky-real-estate.aspx">Kentucky</a></td>
						<td class="school"><a href="massachusetts-real-estate.aspx">Massachusetts</a></td>
					</tr>
					<tr>
						<td class="school"><a href="michigan-real-estate.aspx">Michigan</a></td>
						<td class="school"><a href="montana-real-estate.aspx">Montana</a></td>
						<td class="school"><a href="new-york-real-estate.aspx">New York</a></td>
					</tr>
					<tr>
						<td class="school"><a href="north-carolina-real-estate.aspx">North Carolina</a></td>
						<td class="school"><a href="south-carolina-real-estate.aspx">South Carolina</a></td>
						<td class="school"><a href="texas-real-estate.aspx">Texas</a></td>
					</tr>
					<tr>
						<td class="school"><a href="virginia-real-estate.aspx">Virginia</a></td>
						<td class="school" colspan="2"><a href="washington-real-estate.aspx">Washington</a></td>
					</tr>
				</table>
				
				<table border="0" class="profile">
					<tr><th colspan="2">Real Estate Information</th></tr>
					<tr>
						<td><a href="real-estate-consumer-information.aspx">Consumer Real Estate Information</a></td>
						<td><a href="real-estate-investing-information.aspx">Investing Information</a></td>
					</tr>
					<tr>
						<td><a href="real-estate-credit-information.aspx">Credit Information</a></td>
						<td><a href="real-estate-mortgage-information.aspx">Mortgage Information</a></td>
					</tr>
				</table>
				
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
