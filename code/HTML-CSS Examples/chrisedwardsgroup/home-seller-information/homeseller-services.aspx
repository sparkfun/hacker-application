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
	<title>Homeseller Services offered by Chris Edwards RE/MAX United Cary, North Carolina.</title>
	
	<meta name="Description" content="Description of homeseller services offered by Chris Edwards with RE/MAX United in Cary, North Carolina.">

	<meta name="Keywords" content="Homeseller Services, Chris Edwards, RE/MAX United, Cary, North Carolina">

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
			
				<h1>
					Homeseller Services<br />
					Why use my Professional Services?
				</h1>
				
				<p><strong>I will ........</strong></p>
				
				<p>*Help you establish the best selling price for your home in today's market by preparing a comparative market analysis.</p>
				
				<p>*Be a valuable source of information to keep you up to date on market conditions and the competition. This ensures you have the market knowledge to make the choices that allow your home to sell for maximum value and not become "shop-worn"!</p>
				
				<p>*Help you prepare your home for showing to make it most attractive to buyers. This helps ensure buyers will be excited about your home and be willing to pay top dollar!</p>
				
				<p>*Provide an extensive marketing and advertising program to expose your home to the most buyers possible. This creates the activity necessary to bring top dollar!</p>

				<p>*Place your home on the Multiple List System and on the Internet, and make your home available to the out of town buyers using the services of our own and several other relocation companies.</p>
				
				<p>*Match your home to potential....qualified....buyers. Your time will not be wasted by lookers and people who cannot afford to buy your home.</p>
				
				<p>*Represent you during negotiations to achieve the best possible price and terms.</p>
				
				<p>*Help arrange financing for the buyer to ensure the sale proceeds.</p>
				
				<p>*Handle all details from listing to closing. Up to 50 different people, from appraisers to surveyors will be involved in the transaction. We coordinate the process and arrange all appointments and inspections to ensure a smooth sale that stays together!</p>
				
				<p>*Have information about your new destination sent to you and coordinate the buying process if you are planning to buy another home anywhere in the world.</p>
				
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
