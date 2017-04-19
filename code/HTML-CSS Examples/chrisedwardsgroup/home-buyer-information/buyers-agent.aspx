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
	<title>Why Use a Buyers Agent? Advice from Chris Edwards RE/MAX United Cary, North Carolina.</title>

	<meta name="Description" content="Why Use a Buyers Agent? Advice from Chris Edwards with RE/MAX United in Cary, North Carolina.">

	<meta name="Keywords" content="Buyers Agent, Chris Edwards, RE/MAX United, Cary, North Carolina">

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
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate2.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-buyer.jpg" alt="Contact Chris Edwards for Home Buyer Assistance in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Why Use A Buyer's Agent?</h1>
									
				<table border="0" class="faq">
					<tbody>
					
						<tr><td class="question">Q: Why Use A Buyer's Agent?</td></tr>
						<tr><td class="answer">A: Buying a home will probably be the largest financial investment of your life. Buying a home is a major transaction. Why chance potential financial problems, legal technicalities and the emotional stress created by searching for a new home. Focus your time and energy on making the decision of what to buy. Use the free Buyer Agent services of the Chris Edwards Group to locate and secure the Real Estate you desire.</td></tr>
						<tr><td>&nbsp;</td></tr>
						
						<tr><td class="question">Q: Can You Find Your Dream House Without Help?</td></tr>
						<tr><td class="answer">A: Of course you can. It is possible that you will stumble onto the perfect property the first five minutes you begin looking. The questions you should really be asking are ...</td></tr>
						<tr><td>&nbsp;</td></tr>
						
						<tr><td class="question">Q: Do you have endless time and energy?</td></tr>
						<tr><td class="question">Q: Do you know a lot of Realtors?</td></tr>
						<tr><td class="question">Q: Do you want to spend a large part of each day looking at newly available properties?</td></tr>
						<tr><td class="answer">A: If you answer "NO!" to any of these questions, you need to harness the experience and determination of Chris Edwards to help you find and purchase your next home. Whether you are a first time buyer or a seasoned homeowner, Chris can make the task of locating your next home easier and less stressful.</td></tr>
						<tr><td>&nbsp;</td></tr>
						
						<tr><td class="question">Q: Do you think you will have to pay extra for a Buyer's Agent?</td></tr>
						<tr><td class="answer">A: The answer is no. A Buyer's Agent is paid from the fees collected by the Seller's Agent for selling the property. What you get is an experienced professional at your side throughout the process to make sure the details are completed.</td></tr>
						<tr><td>&nbsp;</td></tr>
						
						<tr><td class="question">Working as your personal Buyer's Agent, Chris Edwards will:</td></tr>
						<tr><td class="answer">
							<ul>
								<li>Look for and find homes that fit your requirements.</li>
								<li>Help pre-qualify you for a mortgage and find properties in your price range.</li>
								<li>View properties and troubleshoot problems before you visit or make an offer.</li>
								<li>Offer Advice on how much a property is worth.</li>
								<li>Negotiate and present any offers on your behalf.</li>
								<li>Refer you to experts, such as attorneys, inspectors, mortgage lenders and movers.</li>
							</ul>
						</td></tr>
						<tr><td>&nbsp;</td></tr>
					</tbody>
				</table>
						
				<p>Give Chris a call or drop him an <a href="mailto:chris@chrisedwardsgroup.com">email</a> today and let him start looking for your new home!</p>
				
					
					
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
