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
	<title>Raleigh, North Carolina Relocation Program - Full Service Relocation to Raleigh, Cary and Apex, NC</title>
	
	<meta name="Description" content="Find out the benefits of full service Raleigh, North Carolina relocation services offered by the Chris Edwards Group.">

	<meta name="Keywords" content="Raleigh Relocation, North Carolina Relocation, Full Service Relocation">

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
			
				<h1>Raleigh, North Carolina Relocation Program</h1>
									
				<a href="meet-the-group.aspx"><img src="/images/logos/logo-btn.jpg" alt="Meet the Chris Edwards Group." width="158" height="190" border="0" class="logorelo" /></a>

				<p>Congratulations on your decision to relocate to the Triangle Area of North Carolina, which includes the towns of <strong>Apex</strong>, <strong>Cary</strong>, <strong>Chapel Hill</strong>, <strong>Clayton</strong>, <strong>Durham</strong>, <strong>Fuquay-Varina</strong>, <strong>Holly Springs</strong>, <strong>Garner</strong>, <strong>Knightdale</strong>, <strong>Morrisville</strong>, <strong>Raleigh</strong>, <strong>Wake Forest</strong> and <strong>Willow Springs</strong>.</p>

				<p>If you are just considering the Raleigh area of North Carolina, let us be your resource to receive all the helpful information you need to make a final decision!</p>
				
				<p>Our full service relocation service provides you with the following support:</p>

				<ul class="whylist">
					<li class="why">
						A complete relocation packet that includes a Triangle relocation guide, maps, listings for your Area of interest and other helpful area information. <a href="triangle-relocation.aspx">Get your free, comprehensive Triangle relocation packet now!</a>
					</li>
					<li class="why">
						&quot;My Home Search Profile&quot; that automatically emails you homes that fit your specific criteria. <a href="/raleigh-cary-real-estate/raleigh-cary-home-search.aspx">Set up your Home Search Profile now</a>!
					</li>
					<li class="why">
						Professional assistance to locate and purchase your ideal home.
					</li>
					<li class="why">
						Coordination with one of our preferred lenders to take care of all your financing needs.
					</li>
					<li class="why">
						Home Inspection coordination.
					</li>
					<li class="why">
						Home owners insurance coordination.
					</li>
					<li class="why">
						Moving &amp; Storage coordination.
					</li>
					<li class="why">
						Closing attorney coordination.
					</li>
					<li class="why">
						Excellent assistance to get your current home sold. This can be locally or anywhere in North America! We have an unbeatable marketing plan designed to attract the maximum amount of buyers!
					</li>
					<li class="why">
						Short or long term housing needs.
					</li>
					<li class="why">
						A utility connection service - <a href="http://www.allconnect.com/pages/mission.html" target="_blank">All Connect</a>.
					</li>
				</ul>
				
				<p class="bold">Our main goal is to make your transition a smooth one.<br />Why Full Service Relocation?</p>

				<ul class="whylist">
					<li class="why">Chris Edwards is a Certified Relocation Specialist.</li>
					<li class="why">Excellent track record of satisfied clients.</li>
					<li class="why">Established connections with proven vendors who provide excellent service.</li>
					<li class="why">One main point of contact to lead you to the services you need.</li>
					<li class="why">A relocation system in place to take the worries out of your transition.</li>
				</ul>
					
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
