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
	<title>Devils Ridge Golf Club in Holly Springs, NC: Raleigh - Cary - Apex - Research Triangle Park - Wake County - Triangle, NC Real Estate Agent Chris Edwards</title>
	
	<meta name="Description" content="Devils Ridge Golf Club in Holly Springs, NC by Rich Vinesett with RE/MAX United in Cary, North Carolina.">

	<meta name="Keywords" content="">

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
			
				<h1>Devils Ridge Golf Club in Holly Springs, NC by Rich Vinesett</h1>
				
				<p>Devils Ridge Golf Club is nestled inside of a lovely community in Holly Springs, North Carolina called Sunset Ridge. Do you like to play target golf? If the answer is yes, schedule a tee time at Devils Ridge today. The fairways are tight but lush. There are lots of sharp doglegs and not a lot of straight holes with the exception of the beloved "Dolly Partin" hole number 15; you'll have to play there to see what I mean. The greens are cut tight and will role straight and true, so if you miss to the right, it's because you pushed it. If you play a mean slice, there are several holes that will suit you fine. If you can't hit a slice or a draw, take extra balls.</p>

				<p>Sunset Ridge Community dates back to the early 90's, a time when most Wake County locals didn't know what Holly Springs was, but it has homes as new as 2008. Sunset Ridge is somewhat of a pioneer subdivision to a town that was ranked the 22nd best place to live by money magazine in 2007. Sunset Ridge has two huge swimming pools, tennis courts, basketball hoops, soccer fields, walking trails, sidewalks, schools, established trees, .5 acre and bigger lots, custom detached homes, townhomes, and condos. The prices range from 90k up in to the millions. And I must say that Sunset Ridge in Holly Springs North Carolina is one of my favorites!!!</p>

				<p>See Rich's comments and information on other golf course communities in Raleigh, Cary, Apex, Holly Springs, Fuquay Varina, Wake Forest, Durham, Chapel Hill, Clayton, Pittsboro, and Hillsborough North Carolina.</p>

				<p>Or visit individual golf course communities such as:  Bentwinds, Brier Creek, Chapel Ridge, Crooked Creek, Eagle Ridge Hasentree, Governors Club Hedingham, Heritage Wake Forest, Lochmere, Knights Play, Macgregor Downs, North Ridge, Preserve at Jordan Lake, Preston, River Ridge, Sunset Ridge, The Neuse Twelve Oaks, Wakefield or Wildwood Green.</p>
				
				<h2><a href="/full-service-relocation/meet-the-group/vinesett-rich.aspx">Back to Meet Rich Vinesett, Chris Edwards Group Co-Associate</a></h2>
				
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
