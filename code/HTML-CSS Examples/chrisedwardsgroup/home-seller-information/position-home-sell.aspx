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
	<title>Position Your Home to Sell.</title>
	
	<meta name="Description" content="Tips and tricks to position your home to sell.">

	<meta name="Keywords" content="Home Sales, Home Seller, Sell Home">

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
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate4.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-seller.jpg" alt="Contact Chris Edwards for Home Seller Assistance in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Position Your Home To Sell</h1>
									
				<p><strong>Outside</strong></p>
				
				<ul class="whylist">
					<li class="why">Take care of your lawn (mow, edge, rake, landscape) and trim your shrubs.</li>
					<li class="why">Clean and organize your porch.</li>
					<li class="why">Paint or clean the front door.</li>
					<li class="why">Rake leaves, shovel walks</li>
				</ul>
				
				<p><strong>Inside</strong></p>
				
				<ul class="whylist">
					<li class="why">Clean all rooms and make them shine.</li>
					<li class="why">Redecorate to cover scuffed woodwork or fading paint.</li>
					<li class="why">Check faucets, lights, light bulbs.</li>
					<li class="why">Repair sticky doors and cabinets.</li>
					<li class="why">Eliminate safety hazards (slippery rugs, low hanging objects).</li>
					<li class="why">De-clutter and organize storage areas: attic, basement, closets.</li>
					<li class="why">Repair damaged or ugly caulking in bathroom.</li>
					<li class="why">Put out your best linens, towels, window treatments.</li>
					<li class="why">Get rid of excess furnishings, decorations, belongings.</li>
					<li class="why">Turn on lights at night, open shades in day.</li>
					<li class="why">Keep people and pets away!</li>
					<li class="why">Turn off radios and TVs.</li>
				</ul>
				
				<p><strong>And remember, if you are present during a showing:</strong></p>
				
				<ul class="whylist">
					<li class="why">Do not apologize for anything in your home.</li>
					<li class="why">Do not offer to include furnishings or other items in the sale.</li>
					<li class="why">Do not get involved in discussions about price and terms.</li>
				</ul>
				
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
