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
	<title>Oaklyn Neighborhood of Raleigh, North Carolina.</title>
	
	<meta name="Description" content="Description of the Oaklyn neighborhood in Raleigh, North Carolina.">

	<meta name="Keywords" content="Oaklyn, Raleigh, North Carolina">

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
						<td><img src="/images/left-pix/oaklyn-sign.jpg" alt="" width="290" height="210" border="0"><img src="/images/right-pix/rt-tours.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Oaklyn Neighborhood Profile</h1>
						
						<table border="0" class="faq" summary="Oaklyn Neighborhood">
							<tr>
				    <td>
									City: Raleigh, North Carolina
								</td>
							</tr>
							<tr>
								<td>
									Address: Off Graham Newton Road Between Penny &amp; 1010.
								</td>
							</tr>
							<tr>
								<td>
									<a href="http://www.mapquest.com/maps/map.adp?location=tY%2fA2v3yyuBemWcCbE4t6CDXuTdnMrm%2fpoJwcHEY6G7oyMdLaHpS6T1nPU53ICbO6TxUyx%2bNdhUrEwZP5r2iUTkMXk53RHZq3DupkmKnnuKmcyvW1L5qQQ%3d%3d&address=graham%20newton%20and%20oaklyn%20springs&city=raleigh&state=nc&zipcode=27606&country=US&addtohistory=&submit=Get%20Map" target="_blank">Map link to neighborhood</a> (click link)
								</td>
							</tr>
						</table>
							
						<h2>Neighborhood Facts:</h2>
						
						<ul class="whylist">
							<li class="why">
								39 homes in subdivision
							</li>
							<li class="why">
								Age range is 1994 - 2000
							</li>
							<li class="why">
								Price range is $300,000 - $400,000
							</li>
							<li class="why">
								Convenient to Raleigh &amp; Cary!
							</li>
							<li class="why">
								Spacious Lot Sizes
							</li>
						</ul>
						
						<p>Oaklyn is a small subdivision made up of only 39 homes, and is a unique spot in Southern Wake County! The prices range across the $300's based on location and square footage. Oaklyn is a good value for those wanting a nice home on a large lot under $400,000. Home styles are more traditional and were built in the mid to late 1990's. Oaklyn is adjacent to the Whitehall neighborhood.</p>

						<p>The area where Oaklyn, <a href="whitehart.aspx">Whitehart</a> &amp; <a href="heatherstone.aspx">Heatherstone</a> are located is booming with prime development and land is getting to be harder to find and more expensive. Most new homes are starting above $500,000 and are on small, non-private lots. This makes neighborhoods like Whitehart, Oaklyn &amp; Heatherstone even more sought after!</p>
						
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
