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
	<title>Raleigh Arts Links</title>
	
	<meta name="Description" content="Arts links for Raleigh, Cary and Wake County, North Carolina.">

	<meta name="Keywords" content="Raleigh, Cary, Wake County, City Links, Arts Links">

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
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate2.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-link-directory.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Raleigh Arts Links</h1>
					
				<p>Here are some helpful links pertaining to arts in the Raleigh, Cary and Apex area.</p>
					
				<ul class="whylist">
					<li class="why">
						<a href="http://www.unc.edu/arts/" target="_blank">Arts Carolina - University of NC - Chapel Hill</a>
					</li>
					<li class="why">
						<a href="http://www.alltelpavilion.com/" target="_blank">Alltel Pavilion at Walnut Creek</a>
					</li>
					<li class="why">
						<a href="http://carolinaunion.unc.edu/cuab/" target="_blank">Carolina Union - University of NC</a>
					</li>
					<li class="why">
						<a href="http://www.carolinaballet.com/" target="_blank">Carolina Ballet</a>
					</li>
					<li class="why">
						<a href="http://www.carolinatheatre.org/" target="_blank">Carolina Theater (Durham)</a>
					</li>
					<li class="why">
						<a href="http://www.concertsingers.org/" target="_blank">Concert Singers of Cary</a>
					</li>
					<li class="why">
						<a href="http://www.durhamsavoyards.org/" target="_blank">Durham Savoyards</a>
					</li>
					<li class="why">
						<a href="http://www.ncmasterchorale.org/" target="_blank">NC Master Chorale (Raleigh Oratorio Society)</a>
					</li>
					<li class="why">
						<a href="http://www.ncarts.org/" target="_blank">North Carolina Arts Council</a>
					</li>
					<li class="why">
						<a href="http://www.ncdcr.gov/" target="_blank">North Carolina Dept. of Cultural Resources</a>
					</li>
					<li class="why">
						<a href="http://www.ncmoa.org/" target="_blank">North Carolina Museum of Art</a>
					</li>
					<li class="why">
						<a href="http://www.nctheatre.com" target="_blank">North Carolina Theater (Raleigh)</a>
					</li>
					<li class="why">
						<a href="http://www.ncsymphony.org/" target="_blank">North Carolina Symphony</a>
					</li>
					<li class="why">
						<a href="http://www.pinecone.org/" target="_blank">Pinecone (Traditional Music)</a>
					</li>
					<li class="why">
						<a href="http://www.raleighboychoir.org/" target="_blank">Raleigh Boy Choir</a>
					</li>
					<li class="why">
						<a href="http://raleighflutes.org/" target="_blank">Raleigh Flute Association</a>
					</li>
					<li class="why">
						<a href="http://www.raleighlittletheatre.org/" target="_blank">Raleigh Little Theatre</a>
					</li>
					<li class="why">
						<a href="http://www.rr.org/" target="_blank">Raleigh Ringers (Handbell Choir)</a>
					</li>
					<li class="why">
						<a href="http://www.theatreinthepark.com/" target="_blank">Theatre in the Park (Raleigh)</a>
					</li>
					<li class="why">
						<a href="http://www.trianglebrass.org/" target="_blank">Triangle Brass Band</a>
					</li>
					<li class="why">
						<a href="http://www.trianglewind.org/" target="_blank">Triangle Wind Ensemble</a>
					</li>
				</ul>
				
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
