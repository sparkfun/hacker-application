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
	<title>Oakwood Neighborhood of Cary, North Carolina - Chris Edwards RE/MAX United</title>
	
	<meta name="Description" content="Description of the Oakwood neighborhood in Raleigh, North Carolina.">

	<meta name="Keywords" content="Raleigh, Oakwood, North Carolina">

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
						<td><img src="/images/left-pix/oakwood-sign.jpg" alt="" width="290" height="210" border="0"><img src="/images/right-pix/rt-tours.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Oakwood Neighborhood Profile</h1>
						
						<table border="0" class="faq" summary="Oakwood Neighborhood">
							<tr>
				    <td>
									City: Raleigh, North Carolina
								</td>
							</tr>
							<!-- to be added 
							<tr>
								<td>
									Address: 
								</td>
							</tr>
							-->
						</table>
							
						<h2>Neighborhood Facts:</h2>
						
						<ul class="whylist">
							<li class="why">
								The prime, historic district!
							</li>
							<li class="why">
								One of Raleigh's major tourist attractions.
							</li>
						</ul>
						
						<p>The diamond of downtown Raleigh, Oakwood is the prime, historic district! What makes Oakwood so desireable? First, the amazing array of historic homes, many of which have been completely updated to the fullest extent! Next, Oakwood is in the heart of the current revitalization of downtown Raleigh that includes the Blount street project, Fayetteville Avenue and Seaboard Station. Oakwood is truly one of the most authentic, historic neighborhoods in North Carolina!</p>  

						<p>When Oakwood was initially developed in the post-Civil War years, it was considered to be out in the boonies - dug out of the woods and a hike into town. Today the same turf is effectively downtown. Regardless, many of the Victorian-style homes appear much as they did at the time.</p>
						
						<p>Oakwood saw some lean years through most of the last century, as people moved to the suburbs and many of the homes were neglected. In the early '70s, though, a spruce-up was underway and the neighborhood is today one of the loveliest in the city. In 1974, it became Raleigh's first designated historic district, and was the first to be listed in the National Register. In addition to the homes, there's a Confederate cemetery with some very cool, very elaborate headstones. The North Carolina Museum of History is a good source for more on Oakwood.</p>
						
						<p>The announcement in 1972 of plans for a major thoroughfare through the heart of Oakwood united residents, and the Society for the Preservation of Historic Oakwood was formed. The thoroughfare plan was ultimately thwarted and neighborhood revitalization continues. Oakwood is now one of Raleigh's major tourist attractions. It is a tangible reminder of Southern urban life during the 19th and early 20th centuries, but is also a vital modern community</p>
						
						<p>Oakwood Historic District, directly east of the <a href="http://www.cr.nps.gov/nr/travel/raleigh/exe.htm" target="_blank">Executive Mansion</a>, is roughly bounded by Person St. on the west, Franklin St. on the north, Watauga and Linden sts. on the east, and Edenton and Morson sts. on the south. Walking tour maps are available at the Raleigh Capital Area Visitor Services, located in the lobby of the North Carolina Museum of History at 5 E. Edenton St. For further information visit the neighborhood's website, which includes information on the Annual Garden Tour and December's Candlelight Tour. Oakwood Cemetery (<a href="http://www.historicoakwood.com/" target="_blank">www.historicoakwood.com</a>) is open to visitors daily, from 8:00am to 5:00pm in the winter, and until 6:00pm the rest of the year.</p> 

						<p>
							Historic Oakwood Official Site:<br />
							<a href="http://www.historicoakwood.org/" target="_blank">http://www.historicoakwood.org/</a>
						</p>
						
						<p>
							Historic Oakwood Cemetery:<br />
							<a href="http://www.historicoakwoodcemetery.com/" target="_blank">http://www.historicoakwoodcemetery.com/</a>
						</p>
						
						<h2>Please feel free to <a href="/contact.aspx">contact</a> Chris for more information about the sale of a home in a particular area or subdivision.</h2>
				
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
