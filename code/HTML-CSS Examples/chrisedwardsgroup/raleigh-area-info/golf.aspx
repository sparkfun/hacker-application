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
	<title>Golf Courses in North Carolina</title>
	
	<meta name="Description" content="Links to golf courses in North Carolina.">

	<meta name="Keywords" content="Golf, North Carolina, Links, Sports">

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
			
				<h1>Golf Courses in North Carolina</h1>
					
				<p>Here are some helpful links and addresses to Golf Courses in North Carolina.</p>
				
				<h2>Apex</h2>
				
				<ul class="whylist">
					<li class="why">
						<a href="http://www.knightsplay.com/home.html" target="_blank">Knights Play Golf Center</a>, 2512 Ten Ten Road, Apex, NC 919-303-4653
					</li>
				</ul>
				
				<h2>
					Cary
				</h2>
				
				<ul class="whylist">
					<li class="why">
						<a href="http://www.lochmere.com/" target="_blank">Lochmere Golf Course</a>, 2511 Kildaire Farm Road, Cary, NC 919-851-0611
					</li>
				</ul>
				
				<h2>
					Chapel Hill
				</h2>
				
				<ul class="whylist">
					<li class="why">
						<a href="http://tarheelblue.ocsn.com/finley/unc-finley.html" target="_blank">Finley Golf Course</a>, Finley Golf Course Road, Chapel Hill, NC 919-962-2349
					</li>
					<li class="why">
						<a href="http://www.governorsclub.com/" target="_blank">Governor's Club Golf Course</a>, 10100 Governors Drive, Chapel Hill, NC 800 925-0085
					</li>
					<li class="why">
						Twin Lake Golf Course, 305 Tenney Circle, Chapel Hill, NC 919-933-1024
					</li>
				</ul>
				
				<h2>
					Clayton
				</h2>
				
				<ul class="whylist">
					<li class="why">
						<a href="http://www.neusegolf.com/" target="_blank">The Neuse Golf Course</a>, 918 Birkdale Drive, Clayton, NC 919-550-0550
					</li>
					<li class="why">
						Pine Hollow Golf Course, 3900 Garner Road, Clayton, NC 919-553-4554
					</li>
					<li class="why">
						<a href="http://www.fredsmithcompany.com/" target="_blank">Riverwood</a>, 400 Riverwood Drive, Clayton, NC 919-550-1919
					</li>
				</ul>
				
				<h2>
					Durham
				</h2>
				
				<ul class="whylist">
					<li class="why">
						Croasdaile Country Club, 3800 Farm Gate Ave, Durham, NC 919-383-1591
					</li>
					<li class="why">
						<a href="http://www.washingtondukeinn.com/golfclub.html" target="_blank">Duke University Golf Course</a>, 3001 Cameron Blvd., Durham, NC 919-681-6161
					</li>
					<li class="why">
						Falls Village Golf, 115 Falls Village Ln., Durham, NC 919-596-4653
					</li>
					<li class="why">
						<a href="http://www.hillandalegolf.com/" target="_blank">Hillandale Golf Course</a>, PO Box 2786, Durham, NC 919-286-4211
					</li>
					<li class="why">
						Hope Valley Country Club &amp; Golf Course, 3803 Dover Road, Durham, NC 919-489-6565
					</li>
					<li class="why">
						Lumley Road, Lumley Road, Durham, NC 919-596-2401
					</li>
					<li class="why">
						The Crossings at Grove Park, 4023 Wake Forest Hwy, Durham, NC 919-596-7298
					</li>
					<li class="why">
						<a href="http://www.treyburncc.com/" target="_blank">Treyburn Country Club</a>, 1 Old Trail, Durham, NC 919-620-0184
					</li>
					<li class="why">
						Willowhaven Country Club, 253 Country Club Drive, Durham, NC 919-383-5511
					</li>
				</ul>
				
				<h2>
					Fuquay-Varina
				</h2>
				
				<ul class="whylist">
					<li class="why">
						<a href="http://www.bentwinds.org/" target="_blank">Bentwinds Golf and Country Club</a>, 6536 Dornoch Place, Fuquay-Varina, NC 919-552-5656
					</li>
					<li class="why">
						<a href="http://www.playcrookedcreek.com/" target="_blank">Crooked Creek Golf Club</a>, 4621 Shady Greens Drive, Fuquay-Varina, NC 919-557-7529
					</li>
					<li class="why">
						Hidden Valley Golf Course, 7900 Highway 55 South, Willow Spring, NC 919-639-4071
					</li>
				</ul>
				
				<h2>
					Garner
				</h2>
				
				<ul class="whylist">
					<li class="why">
						Eagle Crest Golf Club, 4400 Auburn Church Road, Garner, NC 919-772-6104
					</li>
					<li class="why">
						<a href="http://www.ergc.biz/" target="_blank">Eagle Ridge Golf Club</a>, 575 Competition Road, Garner, NC 919-661-6300
					</li>
					<li class="why">
						<a href="http://www.meadowbrookgolfclub.com/" target="_blank">Meadowbrook Country Club</a>, 8202 White Oak Road, Garner, NC-919 772-1636
					</li>
				</ul>
				
				<h2>
					Holly Springs
				</h2>
				
				<ul class="whylist">
					<li class="why">
						<a href="http://www.devilsridgecc.com/" target="_blank">Devil's Ridge Golf Club</a>, 5107 Linksland Drive, Holly Springs, NC 919 557-6100
					</li>
				</ul>
				
				<h2>
					Raleigh
				</h2>
				
				<ul class="whylist">
					<li class="why">
						Cheviot Hills Golf Course, 7301 Capital Blvd., Raleigh, NC 919-876-9920
					</li>
					<li class="why">
						Hedingham Golf Club, 4801 Harbour Towne Drive, Raleigh, NC 919-250-3030
					</li>
					<li class="why">
						<a href="http://www.heritagewakeforest.com/golf/" target="_blank">Heritage Golf Club</a>, 1250 Heritage Club Avenue, Wake Forest, NC 919-453-2020
					</li>
					<li class="why">
						Raleigh Golf Association, 1527 Tryon Road, Raleigh, NC 919-772-998
					</li>
					<li class="why">
						<a href="http://www.golfriverridge.com/" target="_blank">River Ridge Golf Club</a>, 3224 Auburn-Knightsdale Road, Raleigh, NC 919-661-8374
					</li>
					<li class="why">
						<a href="http://www.wildwoodgreen.com/" target="_blank">Wildwood Green Golf Club</a>, 3000 Bally Bunion Way, Raleigh, NC 919-846-8376
					</li>
					<li class="why">
						<a href="http://www.wil-margolf.com/" target="_blank">Wil-Mar Golf Club</a>, 2300 Old Milburnie Road, Raleigh, NC 919-266-1800
					</li>
				</ul>
				
				<h2>
					Wake Forest
				</h2>
				
				<ul class="whylist">
					<li class="why">
						<a href="http://www.brevofieldgolf.com/" target="_blank">Brevofield Golf Links</a>, 13625 Camp Kanata Road, Wake Forest, NC 919 556-3715
					</li>
					<li class="why">
						Paschal Golf Club, 555 Stadium Drive, Wake Forest, NC 919-556-5861
					</li>
					<li class="why">
						<a href="http://www.wakeforestgolfclub.com/" target="_blank">Wake Forest Golf Club</a>, 13239 Capital Blvd., Wake Forest, NC 919-556-3416
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
