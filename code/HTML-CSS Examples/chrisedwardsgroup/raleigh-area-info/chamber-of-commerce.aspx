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
	<title>North Carolina Chambers of Commerce</title>
	
	<meta name="Description" content="Chamber of Commerce links for Raleigh, Cary and Wake County, North Carolina.">

	<meta name="Keywords" content="Raleigh, Cary, Wake County, Chamber of Commerce">

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
			
				<h1>North Carolina Chambers of Commerce</h1>
					
				<p>Here are links to Chamber of Commerce websites in North Carolina.</p>
				
				<ul class="whylist">
					<li class="why">
						<a href="http://www.apexchamber.com/" target="_blank">Apex Chamber of Commerce</a>
					</li>
					<li class="why">
						<a href="http://www.carychamber.com/" target="_blank">Cary Chamber of Commerce</a>
					</li>
					<li class="why">
						<a href="http://www.carolinachamber.org/" target="_blank">Chapel Hill Chamber of Commerce</a>
					</li>
					<li class="why">
						<a href="http://www.durhamchamber.org/" target="_blank">Durham Chamber of Commerce</a>
					</li>
					<li class="why">
						<a href="http://www.fuquay-varina.com/" target="_blank">Fuquay-Varina  Chamber of Commerce</a>
					</li>
					<li class="why">
						<a href="http://www.garnerchamber.com/" target="_blank">Garner Chamber of Commerce</a>
					</li>
					<li class="why">
						<a href="http://www.hollyspringschamber.org/" target="_blank">Holly Springs Chamber of Commerce</a>
					</li>
					<li class="why">
						<a href="http://www.hollyspringsnc.org/" target="_blank">Holly Springs Economic Development Council</a>
					</li>
					<li class="why">
						<a href="http://www.knightdalechamber.com/" target="_blank">Knightdale Chamber of Commerce</a>
					</li>
					<li class="why">
						<a href="http://www.morrisvillenc.com/" target="_blank">Morrisville Chamber of Commerce</a>
					</li>
					<li class="why">
						<a href="http://www.raleighchamber.org/" target="_blank">Greater Raleigh Chamber of Commerce</a>
					</li>
					<li class="why">
						<a href="http://www.raleighcvb.org/" target="_blank">Greater Raleigh Convention &amp; Visitors Bureau</a>
					</li>
					<li class="why">
						<a href="http://www.ncchamber.net/mx/hm.asp?id=home" target="_blank">North Carolina Chamber</a>
					</li>
					<li class="why">
						<a href="http://www.wakeforestchamber.org/" target="_blank">Wake Forest Chamber of Commerce</a>
					</li>
					<li class="why">
						<a href="http://www.wendellchamber.com/" target="_blank">Wendell Chamber of Commerce</a>
					</li>
					<li class="why">
						<a href="http://www.zebulonchamber.com/" target="_blank">Zebulon Chamber of Commerce</a>
					</li>
				</ul>
				
				<p><strong>Link Trade</strong> - Would you like to exchange links with us?  We accept high quality link partners.  All link partners will be evaluated based on relative content and quality. We require a reciprocal link on a comparable website page. To setup a reciprocal link <a href="mailto:webmaster@chrisedwardsgroup.com">e-mail the webmaster</a>.</p>
				
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
