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
	<title>Sports in North Carolina</title>
	
	<meta name="Description" content="Links to North Carolina sports websites.">

	<meta name="Keywords" content="North Carolina, Links, Sports">

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
			
				<h1>Sports in North Carolina</h1>
					
				<p>Here are some helpful links pertaining to Sports in North Carolina.</p>
				
				<h2>
					Professional Sports Teams
				</h2>
				
				<ul class="whylist">
					<li class="why">
						<a href="http://www.caneshockey.com/" target="_blank">Carolina Hurricanes</a> (NHL) - Raleigh, NC
					</li>
					<li class="why">
						<a href="http://www.panthers.com/" target="_blank">Carolina Panthers</a> (NFL) - Charlotte, NC
					</li>
					<li class="why">
						<a href="http://www.cobrasfootball.com/" target="_blank">Carolina Cobras</a> (Arena Football) - Raleigh, NC
					</li>
					<li class="why">
						<a href="http://www.dbulls.com/" target="_blank">Durham Bulls</a> (AAA Baseball) - Durham, NC
					</li>
					<li class="why">
						<a href="http://www.gomudcats.com/" target="_blank">Carolina Mudcats</a> (SE League Baseball) - Zebulon, NC
					</li>
					<li class="why">
						<a href="http://www.wusa.com/" target="_blank">Carolina Courage</a> (Women's Soccer) - Raleigh, NC
					</li>
				</ul>
				
				<h2>
					Collegiate Teams
				</h2>
				
				<ul class="whylist">
					<li class="why">
						<a href="http://theacc.ocsn.com/" target="_blank">ACC - Atlantic Coast Conference</a>
					</li>
					<li class="why">
						<a href="http://goduke.ocsn.com/" target="_blank">Duke Sports Infonet</a> - Durham, NC
					</li>
					<li class="why">
						<a href="http://gopack.ocsn.com/" target="_blank">NCSU Athletics</a> - Raleigh, NC
					</li>
					<li class="why">
						<a href="http://tarheelblue.ocsn.com/" target="_blank">University of NC Athletics</a> - Chapel Hill, NC
					</li>
				</ul>
				
				<h2>
					Local Clubs and Teams
				</h2>
				
				<ul class="whylist">
					<li class="why">
						<a href="http://www.brookhillsteeplechase.com/" target="_blank">Brookhill Steeplechase</a>
					</li>
					<li class="why">
						<a href="http://www.caslnc.com/" target="_blank">Capital Area Soccer League</a>
					</li>
					<li class="why">
						<a href="http://www.teamdiscovery.com/Diamonds/" target="_blank">Carolina Diamonds (Fastpitch Softball)</a>
					</li>
					<li class="why">
						<a href="http://www.sports-nc.com/" target="_blank">Directory of North Carolina Sports Organizations</a>
					</li>
					<li class="why">
						<a href="http://www.ncbikeclub.org/" target="_blank">North Carolina Bicycle Club (NCBC)</a>
					</li>
					<li class="why">
						<a href="http://www.nctrailblazers.org/default.asp" target="_blank">North Carolina Trailblazers (Amateur Women's Hockey)</a>
					</li>
					<li class="why">
						<a href="http://www.skinorthcarolina.com/" target="_blank">Ski North Carolina</a>
					</li>
					<li class="why">
						<a href="http://jollyroger.com/windsurf/" target="_blank">Triangle Windsurfing &amp; Boardsailing Club</a>
					</li>
				</ul>
				
				<h2>
					Speedway
				</h2>
				
				<ul class="whylist">
					<li class="why">
						<a href="http://www.charlottemotorspeedway.com/" target="_blank">Charlotte Motor Speedway</a>
					</li>
					<li class="why">
						<a href="http://www.northcarolinaspeedway.com/" target="_blank">The North Carolina Speedway</a>
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
