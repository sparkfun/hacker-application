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
	<title>Lochmere Golf Club in Cary, NC: Raleigh - Cary - Apex - Research Triangle Park - Wake County - Triangle, NC Real Estate Agent Chris Edwards</title>
	
	<meta name="Description" content="Lochmere Golf Club in Cary, NC by Rich Vinesett with RE/MAX United in Cary, North Carolina.">

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
			
				<h1>Lochmere Golf Club in Cary, NC by Rich Vinesett</h1>
				
				<p>Lochmere Golf Club is the only semi-private golf course in Cary, North Carolina. It features an 18 hole golf course, putting green, chipping green, and driving range. Watch out for the water-there us a creek that runs through and along most of Lochmere's winding fairways and there are also several ponds throughout the course, so take a couple of extra balls when you play there. Also, the greens are often times very shaggy so expect to miss the putts you think you'll make and make the putts you think you'll miss. Lochmere golf course was built in 1985 by Gene Hamm.</p>

				<p>Lochmere community itself is very established with several lakes and towering shade trees, swimming pools, tennis courts, walking trails, bike paths, and friendly people. You can get a townhome in Lochmere in the $200's or a detached single-family home for $300k and on into the millions.</p>

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
