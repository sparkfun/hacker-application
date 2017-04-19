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
	<title>Five Points Neighborhood of Cary, North Carolina - Chris Edwards RE/MAX United</title>
	
	<meta name="Description" content="Description of the Five Points neighborhood in Raleigh, North Carolina.">

	<meta name="Keywords" content="Raleigh, Five Points, North Carolina">

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
			
				<h1>Five Points Neighborhood Profile</h1>
				
						<!-- to be added 
						
						<table border="0" class="faq" summary="Five Points Neighborhood">
							<tr>
				    			<td>
									City: West Cary, North Carolina
								</td>
							</tr>
							<tr>
								<td>
									Address: Highway 55 and Edgemore Avenue in West Cary
								</td>
							</tr>
						</table>
						
						-->
							
						<h2>Neighborhood Facts:</h2>
						
						<ul class="whylist">
							<li class="why">
								Price range starts in the mid 300's
							</li>
							<li class="why">
								A Very pleasant composite of styles and different sized homes!
							</li>
						</ul>
						
						<p>The suburban neighborhoods that comprise the Five Points Neighborhoods were part of an extremely important planning movement that had captured the imagination of the Progressive Reformers of Raleigh. In line with their desire for a new, simple, efficient lifestyle that was symbolized by the new bungalow houses which became popular in the 1920s, these suburban neighborhoods were planned communities with services that epitomized efficiency as well as providing escape from unhealthy and hectic urban life.</p>

						<p>Life is still pretty simple and efficient in Five Points. Over the years, the neighborhood has attracted a number of young, first-time homeowners. It's a short drive to downtown, reasonably convenient to the interstate, and has a nice block of Restaurants and Bars.</p>
						
						<p>Greater Five Points is comprised of the neighborhoods of Bloomsbury, Hayes Barton, Roanoke Park and Vanguard Park. Hayes Barton was, in the early days, considered the most desirable of these neighborhoods, the work of the landscape architect Earle Sumner Draper, who designed some of the first greenbelt buffers in the country and was also well known for his design of mill villages.</p>
						
						<p>The homes in the Five Points neighborhoods are a hodge-podge of styles and sizes that nonetheless come together as a very pleasant composite. Today, not many first time homebuyers can afford the area with prices starting in the high $300's for a small, bungalow home that may need significant renovation</p>
						
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
