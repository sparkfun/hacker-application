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
	<title>Raleigh Clubs and Interest Groups Links</title>
	
	<meta name="Description" content="Clubs and Interest Groups links for Raleigh, Cary and Wake County, North Carolina.">

	<meta name="Keywords" content="Clubs, Interest Groups, Raleigh, Cary, Wake County, City Links">

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
			
				<h1>Raleigh Clubs and Interest Groups Links</h1>
					
				<p>Here are some helpful links pertaining to Clubs, Historical Sites and Interest Groups in the Raleigh, Cary and Apex area.</p>
				
				<ul class="whylist">
					<li class="why">
						<a href="http://www.bassclub.com/" target="_blank">Bassmasters Fishing Club</a>
					</li>
					<li class="why">
						<a href="http://www.friends-of-ireland.org/" target="_blank">Friends of Ireland</a>
					</li>
					<li class="why">
						<a href="http://www.ah.dcr.state.nc.us/sections/hs/default.htm" target="_blank">Historical Sites</a>
					</li>
					<li class="why">
						<a href="http://rtpnet.org/ncfats/" target="_blank">NC FATS Mountain Biking Club</a>
					</li>
					<li class="why">
						<a href="http://www.newcomersclubofraleigh.org" target="_blank">Newcomers Club of Raleigh</a>
					</li>
					<li class="why">
						<a href="http://rtpnet.org/rars/" target="_blank">Raleigh Amateur Radio Club</a>
					</li>
					<li class="why">
						<a href="http://rtpnet.org/~rac/" target="_blank">Raleigh Astronomy Club</a>
					</li>
					<li class="why">
						<a href="http://www.raleighbridgeclub.org" target="_blank">Raleigh Bridge Club</a>
					</li>
					<li class="why">
						<a href="http://www.jlraleigh.com/" target="_blank">Raleigh Junior League (Service Organization)</a>
					</li>
					<li class="why">
						<a href="http://www.trainWeb.org/nrmrc/" target="_blank">Raleigh Model Railroad Club</a>
					</li>
					<li class="why">
						<a href="http://radicr.home.mindspring.com/" target="_blank">Raleigh Rose Society</a>
					</li>
					<li class="why">
						<a href="http://jollyroger.com/windsurf/" target="_blank">Triangle Windsurfing & Boardsailing Club</a>
					</li>
					<li class="why">
						<a href="http://www.trifl.org/" target="_blank">TriFL--Triangle Ferret Lovers Club</a>
					</li>
					<li class="why">
						<a href="http://www.trianglewoodturners.com/" target="_blank">Triangle Woodturners</a>
					</li>
					<li class="why">
						<a href="http://www.rootsweb.com/~ncwcgs/" target="_blank">Wake County Genealogical Society</a>
					</li>
					<li class="why">
					<a href="http://rtpnet.org/wakelwv/" target="_blank">Wake County League of Women Voters</a>	
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
