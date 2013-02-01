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
	<title>Eagle Ridge Golf Club in Raleigh, NC: Raleigh - Cary - Apex - Research Triangle Park - Wake County - Triangle, NC Real Estate Agent Chris Edwards</title>
	
	<meta name="Description" content="Eagle Ridge Golf Club in Raleigh, NC by Rich Vinesett with RE/MAX United in Cary, North Carolina.">

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
			
				<h1>Eagle Ridge Golf Club in Raleigh, NC by Rich Vinesett</h1>
				
				<p>Eagle Ridge Golf Club is located on the South side of Raleigh and Wake County, North Carlolina - partially in Garner. It boasts 18 beautiful holes on rolling hills, alongside creeks, over lakes, and underneath the North Carolina sky. Hole number 15 is a doozy of par 3 over a gaping drop off that serves as a ball magnet; don't be deceived. It may only be 200 yards but it plays like 250, so whip out that driver, especially if you're facing the wind.</p>

				<p>The greens at Eagle Ridge are always manicured - not too fast, not too slow, usually just right. The course was designed by Tom Kite and Bob Cupp and was opened for play in 2000.  There is a driving range and a putting green.</p>

				<p>Eagle Ridge Golf Community consists of homes ranging from $180's to the $500's - some are spec homes from builders such as KB Homes and K Hovnanian and some are custom built. There is also a community swimming pool, tennis courts, sidewalks, and lots of family fun.</p>

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
