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
	<title>Bentwinds Country Club in Fuquay Varina, NC: Raleigh - Cary - Apex - Research Triangle Park - Wake County - Triangle, NC Real Estate Agent Chris Edwards</title>
	
	<meta name="Description" content="Bentwinds Country Club in Fuquay Varina, NC by Rich Vinesett with RE/MAX United in Cary, North Carolina.">

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
			
				<h1>Bentwinds Country Club in Fuquay Varina, NC by Rich Vinesett</h1>
				
				<p>Bentwinds Country Club in Fuquay Varina, North Carolina is rugged but elegant. I have a love hate relationship for this course because I have yet to play a good game there, but I can't wait to play it again. One of my insurance partners, Ray Beaird with The Young Group of Fuquay is a member out there. If you get the pleasure to play there, look him up - he'll be smoking a cigar while chasing down his 300+ yard drives. It's a good ol' boy network at Bentwinds. If that's what your looking for, Bentwinds is your club.</p>

				<p>They save the toughest hole for last at Bentwinds Country Club. If you can hit a 300+ yard drive, you can clear the water, otherwise, tee off with your 180 club, and then lay up on the other side of the water. For me, it's 3 shots+ to the green guaranteed. The water is so wide, it beats me every time - kinda like Tin cup. There is a driving range and a practice green.</p>

				<p>Benwinds Community has homes as old as 1980 and as new as 2007. The lots are large because good ol' boys need space. There are walking trails, tennis courts, swimming pools, lakes, and plenty of Southern Hospitality. The trees in the neighborhood are established and towering, so if you're looking for a cleared postage stamp lot, look elsewhere. The homes in Bentwind's range from the 400's and up to approximately $1Million.</p>

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
