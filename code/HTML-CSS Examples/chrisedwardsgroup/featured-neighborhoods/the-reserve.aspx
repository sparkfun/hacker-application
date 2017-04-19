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
	<title>The Reserve Neighborhood of Cary, North Carolina - Chris Edwards RE/MAX United</title>
	
	<meta name="Description" content="Description of the The Reserve neighborhood in Raleigh, North Carolina.">

	<meta name="Keywords" content="Raleigh, The Reserve, North Carolina">

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
						<td><img src="/images/left-pix/the-reserve-sign.jpg" alt="" width="290" height="210" border="0"><img src="/images/right-pix/rt-tours.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>The Reserve Neighborhood Profile</h1>
						
						<table border="0" class="faq" summary="Riggsbee Farm Neighborhood">
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
							
						<h2>Neighborhood Facts:</h2>
						
						<ul class="whylist">
							<li class="why">
								Age range is 1996-2003
							</li>
							<li class="why">
								Price range is from the mid $400's to the mid $600's
							</li>
							<li class="why">
								Located Right Off Highway 55
							</li>
							<li class="why">
								Not to be Confused With &quot;The Reserve&quot; in Chapel Hill, NC
							</li>
						</ul>
						
						<p>The Reserve is truly one of my personal favorites in West Cary! If you are looking a nice home on a larger lot, this is the place! The prices range from the mid $400's to the mid $600's and the layout of the neighborhood is meticulous! Age range is from 1996 to 2003, and you won't find any new build neighborhoods that compare!</p>

						<p>The entrance is located right off Highway 55, north of High House Rd. The Reserve is the next step up from Wellsley and also features a community pool.</p>
						
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
