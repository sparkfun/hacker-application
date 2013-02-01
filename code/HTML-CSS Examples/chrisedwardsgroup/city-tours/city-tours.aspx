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
	<title>Raleigh Area - Featured City Tours</title>
	
	<meta name="Description" content="Search for homes and Real Estate in Apex, Cary, Chapel Hill, Clayton, Durham, Fuquay-Varina, Holly Springs, Garner, Knightdale, Morrisville, Raleigh, Research Triangle Park, Wake Forest and Willow Springs, North Carolina.">

	<meta name="Keywords" content="Apex, Cary, Chapel Hill, Clayton, Durham, Fuquay-Varina, Holly Springs, Garner, Knightdale, Morrisville, Raleigh, Research Triangle Park, Wake Forest, Willow Springs, North Carolina, Home Search, Real Estate">

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
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate2.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-tours.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Featured Raleigh Area City Tours</h1>
				
				<p>The Triangle area of North Carolina is primarily made up of the areas of Raleigh, Durham, Chapel Hill and surrounding communities. <strong>Research Triangle Park (RTP)</strong> is located in about the center of this area.</p>

				<p>Composed of over 7,000 acres of North Carolina pine forest with about 1,100 acres for development, RTP currently houses more than 100 research and development facilities which employ over 38,500 Triangle area residents.</p>

				<p>Major universities at <a href="http://www.ncsu.edu/" target="_blank"><strong>Raleigh (North Carolina State)</strong></a>, <a href="http://www.unc.edu/" target="_blank"><strong>Chapel Hill (University of North Carolina)</strong></a>, <a href="http://www.duke.edu/" target="_blank"><strong>Durham (Duke University)</strong></a> and <a href="http://www.wfu.edu/" target="_blank"><strong>Winston-Salem (Wake Forest)</strong></a> help foster an unparalled environment of education, research and industry. The following cities and communities are located either within that area or nearby, and help create one of the most dynamic and vital areas of the United States.</p>
				
				<table border="0" class="directory" summary="Featured Raleigh, North Carolina Cities including Apex, Cary, Chapel Hill, Clayton, Durham, Fuquay-Varina, Holly Springs, Garner, Knightdale, Morrisville, Raleigh, Research Triangle Park, Wake Forest, Willow Springs.">
					
					<tr>
						<td>
							<p><a href="apex.aspx"><strong>Apex</strong></a></p>
							<p>Settler's came to this area as early as 1867, but the town was incorporated as another stop on the ...</p>
						</td>
						<td>
							<p><a href="cary.aspx"><strong>Cary</strong></a></p>
							<p>Cary was established in 1750, and became a stop on the railroad in the 1850's ...</p>
						</td>
					</tr>
					
					<tr>
						<td>
							<p><a href="chapel-hill.aspx"><strong>Chapel Hill</strong></a></p>
							<p>Chapel Hill, home of UNC, is located northwest of Raleigh and Research Triangle Park ...</p>
						</td>
						<td>
							<p><a href="clayton.aspx"><strong>Clayton</strong></a></p>
							<p>Like Fuquay Varina, Clayton has become a very popular relocation spot because it ...</p>
						</td>
					</tr>
					
					<tr>
						<td>
							<p><a href="durham.aspx"><strong>Durham</strong></a></p>
							<p>Home of Duke University, Durham is known as the "City of Medicine" ...</p>
						</td>
						<td>
							<p><a href="fuquay-varina.aspx"><strong>Fuquay-Varina</strong></a></p>
							<p>Fuquay-Varina, reflects the dual heritage of two communities and the story ...</p>
						</td>
					</tr>
					
					<tr>
						<td>
							<p><a href="garner.aspx"><strong>Garner</strong></a></p>
							<p>Garner is located just south of Raleigh and is a unique and inviting ...</p>
						</td>
						<td>
							<p><a href="holly-springs.aspx"><strong>Holly Springs</strong></a></p>
							<p>Holly Springs offers a small-town atmosphere while balancing projected future growth ...</p>
						</td>
					</tr>
					
					<tr>
						<td>
							<p><a href="knightdale.aspx"><strong>Knightdale</strong></a></p>
							<p>In February of 1701, John Lawson was hired by the King of England to explore the interior of what was ...</p>
						</td>
						<td>
							<p><a href="morrisville.aspx"><strong>Morrisville</strong></a></p>
							<p>The Town of Morrisville consists of approximately 10 square miles in area ...</p>
						</td>
					</tr>
					
					<tr>
						<td>
							<p><a href="raleigh.aspx"><strong>Raleigh</strong></a></p>
							<p>Nestled among the native long-leaf pines in the heart of the Tar Heel State lies ...</p>
						</td>
						<td>
							<p><a href="triangle-park.aspx"><strong>Research Triangle Park</strong></a></p>
							<p>Research Triangle Park (RTP) is a public/private, planned research park, created in 1959 ...</p>
						</td>
					</tr>
					
					<tr>
						<td>
							<p><a href="wake-forest.aspx"><strong>Wake Forest</strong></a></p>
							<p>Welcome to the historic and charming town of Wake Forest, North Carolina ..</p>
						</td>
						<td>
							&nbsp;
						</td>
					</tr>
					
				</table>
				
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
