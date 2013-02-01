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
	<title>Raleigh Hotels and Motels</title>
	
	<meta name="Description" content="Hotels and Motel links for Raleigh, Cary and Wake County, North Carolina.">

	<meta name="Keywords" content="Hotels, Motels, Raleigh, Cary, Wake County, City Links">

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
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate2.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-link-directory.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Raleigh Hotel and Motel Links</h1>
					
				<p>Here are some helpful links to Hotels and Motels in the Raleigh-Durham-Chapel Hill Area.</p>
				
				<p><strong>Downtown - Capitol Area</strong></p>
				<ul class="whylist">
					<li class="why">
						 <a href="clarion-hotel-capitol.aspx">Clarion Hotel State Capitol</a>
					</li>
					<li class="why">
						 <a href="sheraton-hotel-capitol.aspx">Sheraton Hotel Capitol Center</a>
					</li>
				</ul>
				
				<p><strong>Crabtree Valley Mall</strong></p>
				<ul class="whylist">
					<li class="why">
						 <a href="candlewood-hotel-suites.aspx">Candlewood Suites Raleigh/Crabtree Hotel</a>
					</li>
					<li class="why">
						 <a href="embassy-hotel-suites.aspx">Embassy Suites Hotel&reg; Raleigh/Crabtree Valley</a>
					</li>
					<li class="why">
							<a href="fairfield-inn.aspx">Fairfield Inn By Marriott Raleigh Crabtree</a>
					</li>
					<li class="why">
							<a href="hampton-inn.aspx">Hampton Inn Raleigh-Crabtree Valley</a>
					</li>
					<li class="why">
							<a href="holiday-inn.aspx">Holiday Inn Raleigh Crabtree Valley NC</a>
					</li>
				</ul>
				
				<p><strong>NC State University</strong></p>
				<ul class="whylist">
					<li class="why">
							<a href="holiday-inn-brownstone.aspx">Holiday Inn Brownstone Dt Raleigh NC</a>
					</li>
				</ul>
				
				<p><strong>Morrisville-Durham</strong></p>
				<ul class="whylist">
					<li class="why">
							<a href="courtyard-marriott-morrisville.aspx">Courtyard By Marriott Raleigh Durham Airport</a>
					</li>
					<li class="why">
							<a href="fairfield-inn-marriott-morrisville.aspx">Fairfield Inn By Marriott Raleigh Airport/Rtp</a>
					</li>
					<li class="why">
							<a href="residence-inn-marriott-morrisville.aspx">Residence Inn By Marriott Raleigh Durham Airport</a>
					</li>
					<li class="why">
							<a href="holiday-inn-express-morrisville.aspx">Holiday Inn Express Raleigh-Durham Airport NC</a>
					</li>
					<li class="why">
							<a href="la-quinta-inn-morrisville.aspx">La Quinta Inn &amp; Suites Raleigh-Durham Intl Airport</a>
					</li>
					<li class="why">
							<a href="microtel-inn-morrisville.aspx">Microtel Inn</a>
					</li>
				</ul>
				
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
