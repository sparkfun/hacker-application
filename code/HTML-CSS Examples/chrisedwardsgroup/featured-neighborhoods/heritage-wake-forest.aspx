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
	<title>Heritage Wake Forest Neighborhood of Raleigh, North Carolina - Chris Edwards RE/MAX United</title>
	
	<meta name="Description" content="Description of the Heritage Wake Forest neighborhood in Raleigh, North Carolina.">

	<meta name="Keywords" content="Raleigh, Heritage Wake Forest, North Carolina">

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
						<td><img src="/images/left-pix/heritage-forest-sign.jpg" alt="" width="290" height="210" border="0"><img src="/images/right-pix/rt-tours.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Heritage Wake Forest Neighborhood Profile</h1>
						
						<table border="0" class="faq" summary="Whitehart Neighborhood">
							<tr>
				    <td>
									City: Wake Forest, North Carolina
								</td>
							</tr>
							<tr>
								<td>
									Address: Off Rogers Road &amp; Heritage Lake Road
								</td>
							</tr>
						</table>
							
						<h2>Neighborhood Facts:</h2>
						
						<ul class="whylist">
							<li class="why">
								Age range is 2005-New
							</li>
							<li class="why">
								Price range is $250,000-$750,000
							</li>
						</ul>
						
						<p>Heritage Wake Forest is emerging as one of the Triangle's premier golf communities. The creators of Heritage Wake Forest are all proven professionals who share a common desire to create a community unlike any other and to design and build homes with as much lasting value as the &quot;heritage&quot; that surrounds it.</p>  

						<p>With 120 acres set aside for soccer fields, softball fields, walking &amp; biking trails, parks, and greenways, Heritage Wake Forest is just the place for maintaining your active lifestyle. And with the joint efforts of both the Town and County, these amenities will become the center for future growth and a better way of life for you and your family.</p>
						
						<p>The Heritage Golf Course is a challenging, yet player friendly &quot;American Links.&quot; They are proud to have been Voted &quot;North Carolina's Best New Course&quot; in 2002 and Rated 4 1/2 Stars in 2004 both by North Carolina Magazine.</p> 
						
						<p>World class architect Bob Moore with JMP Golf Design Group, designed the course to embrace the natural characteristics of the land, winding through the Carolina pines with elevation changes, creeks, lakes and pot bunkers.</p>
						
						<p>They are a Semi-Private Golf Club and offer numerous amenities including a beautiful clubhouse, fully merchandised golf shop, PGA staffed golf shop, Banquet Room with full catering and the American Links Bar and Grill. We also offer private member-only locker rooms, lounge and club storage.</p> 
						
						<p>For your convenience, The Heritage Club offers one of the area's finest practice facilities, including full driving range with target greens and private teaching tee, practice bunker and chipping green, and an 8000 square foot putting green.</p>
						
						<p>Click the link below for more information on <strong>Heritage Wake Forest</strong>:</p>
						
						<p><a href="http://www.heritagewakeforest.com/" target="_blank">http://www.heritagewakeforest.com/</a></p>
						
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
