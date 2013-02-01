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
	<title>Real Estate Agents Link Directory - Florida</title>
	
	<meta name="Description" content="Florida Real Estate agents Real Estate link directory.">

	<meta name="Keywords" content="Florida Real Estate Links, Real Estate Agents">

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
			
				<h1>Real Estate Agents Link Directory - Florida</h1>
					
				<p>Here are some helpful links pertaining to Real Estate and Real Estate agents in Florida.</p>
				
				<h2>
					Florida
				</h2>
				
				<ul class="whylist">
					<li class="why">
						<a href="http://www.sellsfloridawaterfront.com/" target="_blank">Fort Myers Florida Real Estate</a> - The team of Christopher Ingram and Scott Hills are committed to providing the best and most comprehensive Fort Myers Florida Real Estate services available today.
					</li>
					<li class="why">
						<a href="http://www.buyerbrokerorlando.com/" target="_blank">Orlando Real Estate Broker Brenda DeArmond</a> - Specializing in Orlando real estate buyer/broker representation.
					</li>
					<li class="why">
						<a href="http://www.flgulfhomes.com/" target="_blank">Punta Gorda Florida Real Estate</a> - Bob and Johanne Wallace of RE/MAX Harbor Realty specializing in Punta Gorda and Port Charlotte Florida Real Estate.
					</li>
					<li class="why">
						<a href="http://floridasungroup.com/" target="_blank">Cape Coral Florida Real Estate Broker</a> - Lots and Homes for sale and buy, Vacation Rentals, Property Management.
					</li>
					<li class="why">
						<a href="http://www.gitta.com/" target="_blank">Orlando Florida Real Estate</a> - Central Florida relocation specialist Gitta Urbainczyk.
					</li>
					<li class="why">
						<a href="http://www.buyerbrokertampa.com/" target="_blank">Tampa Florida Real Estate Brenda DeArmond</a> - Specializing in Tampa Bay - Clearwater - St. Petersburg - Greater Gulf Coast Florida.
					</li>
					<li class="why">
						<a href="http://www.miamilodgerealty.com/" target="_blank">Miami Beach Real Estate</a> - Miami real estate services from real estate agents specializing in Miami Beach and Miami- houses, villas, condos and commercial real estate.
					</li>
					<li class="why">
						<a href="http://www.thebestfloridarealestate.com/" target="_blank">South Florida Preconstruction Homes</a> - We provide you with the best platform to buy and sell preconstruction homes in South Florida. If you wish to have your details changed, we would be interested in modification as per your requirements.
					</li>
					<li class="why">
						<a href="http://www.realestatefloridakeys.com/" title="Florida Keys Real Estate" target="_blank">Florida Keys Real Estate</a> - Coldwell Banker Schmitt Real Estate Florida Keys is the most trusted name for real estate in Florida Keys.  We offer the best deals for finest residential & commercial real estate in the entire Florida Keys from Key West to Key Largo.
					</li>
				</ul>
				
				<p><a href="real-estate.aspx">Back To Real Estate Link Directory</a></p>
				
				<p><strong>Link Trade</strong> - Would you like to exchange links with us?  We accept high quality link partners.  All link partners will be evaluated based on relative content and quality.  We require a reciprocal link on a comparable website page.  To setup a reciprocal link <a href="mailto:webmaster@chrisedwardsgroup.com">e-mail the webmaster</a>.</p>

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
