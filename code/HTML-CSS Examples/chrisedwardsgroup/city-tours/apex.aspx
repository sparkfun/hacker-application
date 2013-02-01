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
	<title>Apex, North Carolina City Profile.</title>
	
	<meta name="Description" content="Apex, North Carolina City Profile.">

	<meta name="Keywords" content="Apex, North Carolina, Chris Edwards, RE/MAX United">

	<meta name="Robots" content="all">

	<link rel="shortcut icon" type="image/ico" href="/EdwardsIcon.ico" />
	<link rel="stylesheet" href="/includes/ce.css" type="text/css">
	<link rel="stylesheet" href="/includes/forms.css" type="text/css">
	<script language="JavaScript1.2" src="/includes/rollover.js"></script>
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
						<td><img src="/images/left-pix/apex-sign.jpg" alt="" width="290" height="210" border="0"><img src="/images/right-pix/rt-tours.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1 class="body">Apex, North Carolina Profile</h1>

				<a href="http://tours.tourfactory.com/tours/tour.asp?t=547130" target="_blank" onmouseover="imgOnOff('img1','/images/apex-vtROLL.jpg');" onmouseout="imgOnOff('img1','/images/apex-vt.jpg');"><img name="img1" src="/images/apex-vt.jpg" width="250" height="70" border="1" class="floatright2" /></a>
				
				<p>You'll find Apex to be an interesting area that has so much to offer! The thing you'll quickly notice and have to understand about Apex is that it circumvents several other cities like Cary, Raleigh and even Holly Springs. This gives Apex the advantage of offering so much variety in housing, shopping, good schools and convenience!</p>

				<p>The crossroads of 55 &amp; 64 in Apex is exploding with quality shopping and will eventually meet the new 540 bypass. Many say that when this takes place, that side of Apex will be one of the most convenient places in the Triangle!</p>
				
				<p>The downtown district, anchored by Salem Street, is on the National Register of Historic Places and today the buildings house specialty shops, restaurants, and more. The town's efforts to retain its historic and small town appeal, in spite of monumental growth have paid off: It was named &quot;Best Small Town in North Carolina&quot; by Business North Carolina magazine.</p>
				
				<p>Conveniently located near to Jordan Lake and RTP, Apex offers a reasonable commute, easy access to recreational facilities and a new, state-of-the-art library!</p>  
				
				<p>Settler's came to this area as early as 1867, but the town was incorporated as another stop on the railroad in 1873. Named Apex because, fittingly, the land here was the highest elevation on the Chatham Railroad.</p>
				
				<p class="text">Apex offers many choices in housing, but remains very much a homes, rather than a multifamily area. Here you can choose from among historic (and historic replica) houses and farms, or town homes and apartments. With a current population of only 22,500 (expected to double in the next decade), it has much to offer the folk who may work in the city, but prefer a more small-town setting for their home life. Demand is spurring more development, and more than 25 new neighborhoods are under construction at this time. Not surprising, since Apex offers excellent schools and a brand new, state-of-the-art regional library.</p>

				<p class="center">
					<a href="http://www.agentpreview.com/agents/united-states/north-carolina/apex.htm" target="_blank"><img src="/images/120x60view_my_profile.gif" alt="Apex Real Estate Agent." width="120" height="60" border="0" /></a>
				</p>

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
