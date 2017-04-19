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
	<title>Real Estate Agents Link Directory - North Carolina</title>
	
	<meta name="Description" content="Real Estate agents Real Estate link directory.">

	<meta name="Keywords" content="Real Estate Links, Real Estate Agents">

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
			
				<h1>Real Estate Agents Link Directory - North Carolina</h1>
					
				<p>Here are some helpful links pertaining to Real Estate and Real Estate agents in North Carolina.</p>
				
				<h2>
					North Carolina
				</h2>
				
				<ul class="whylist">
					<li class="why">
						<a href="http://www.copelandrealestatenc.com/" target="_blank">Beaufort, NC Real Estate Crystal Coast Homes</a> - Copeland Real Estate offers Real Estate with offices on the magnificent Crystal Coast in Beaufort and Morehead City, North Carolina.
					</li>
					<li class="why">
						<a href="http://www.greybeardrealty.com/" target="_blank">Asheville, NC Vacation Rentals</a> - Asheville NC, Black Mountains, Montreat, Western North Carolina vacation rentals.
					</li>
					<li class="why">
						<a href="http://www.moreheadcityinfo.com/" target="_blank">A Resident's view of Morehead City and the Crystal Coast, NC</a> - My goal is to build a site to provide as much information as possible about the Crystal Coast area, that would be of interest to anyone moving to, visiting or just desires local information.
					</li>
					<li class="why">
						<a href="http://www.sloanerealty.com/" target="_blank">Ocean Isle Beach Vacation Rentals</a> - Ocean Isle Beach Vacation Rentals, Residential Real Estate, Commercial Real Estate and New Home Communities offered by Coldwell Banker Sloane Realty.
					</li>
					<li class="why">
						<a href="http://www.sloanevacations.com/" target="_blank">Ocean Isle Beach Rentals</a> - Ocean Isle Vacation Beach Rentals offered by Coldwell Banker Sloane Realty, for planning the perfect North Carolina Vacation. 
					</li>
					<li class="why">
						<a href="http://www.sloanecommercial.com/" target="_blank">Brunswick County Commercial Real Estate</a> - Our Company Offers Commercial Real Estate with a Division that continues to be the leader in Commercial Real Estate in Brunswick County.
					</li>
					<li class="why">
						<a href="http://www.sloanenewhomes.com/" target="_blank">Brunswick County New Home</a> - Coldwell Banker Sloane Realty's New Homes division was founded to provide comprehensive marketing and sales solutions to new home builders and developers.
					</li>
					<li class="why">
						<a href="http://www.cbsloane.com/" target="_blank">Ocean Isle Beach Real Estate</a> - Brunswick Islands third generation, family business that has been the leader of real estate in the South Brunswick Islands for 50 years.
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
