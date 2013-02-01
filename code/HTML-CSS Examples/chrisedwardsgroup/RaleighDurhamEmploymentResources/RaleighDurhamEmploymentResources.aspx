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
	<title>Raleigh/Durham Employment Resources - Raleigh, Cary, Apex, Research Triangle Park, Wake County, Triangle, NC Real Estate Agent Chris Edwards</title>
	
	<meta name="Description" content="Employment Resources in the Raleigh, Cary, Apex, Research Triangle Park, Wake County, Triangle, NC area.">

	<meta name="Keywords" content="">

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
			
				<h1>Welcome to our Raleigh/Durham Employment Resource!</h1>
				
				<table class="employment" border="0">
					<tr>
						<td><img src="EmploymentGraphics/EmploymentLogo.jpg" alt="" width="112" height="109" border="0" /></td>
						<td><a href="http://www.raleighjobs.com/" target="_blank"><img src="EmploymentGraphics/ReleighJobs.jpg" alt="" width="322" height="119" border="0" /></a></td>
					</tr>
					<tr>
						<td colspan="2"><a href="http://triangle.bizjournals.com/triangle/jobs/" target="_blank"><img src="EmploymentGraphics/TriangleBusinessJournal.jpg" alt="" width="300" height="60" border="0" /></a></td>
					</tr>
				</table>
				
				<a href="http://www.bizjournals.com/bookoflists/triangle/" target="_blank"><img src="EmploymentGraphics/BookOfLists.jpg" alt="" width="120" height="167" border="0" class="floatright2"></a><p>Because we specialize in relocation, we continually get asked if we have contacts and comprehensive resources for employment in the Raleigh/Cary/Durham area.</p>

				<p>A great place to start! Click the picture to the right to order a current copy of the annual &quot;Book of Lists&quot; from the Triangle Business Journal. This guide is loaded with business names and contact information for almost every field!</p>

				<p>A must for your job hunt!</p>
				
				<p>Below you will find a list of helpful links to assist with your search. We've complied some of the best available resources to make your Raleigh employment search more efficient!</p>
				
				<table class="employment" border="0">
					<tr>
						<td></td>
					</tr>
				</table>
				
				<h2><a href="RaleighCaryDurhamExecutiveRecruiters.aspx">Raleigh's Top Executive Recruiter List!</a></h2>
				
				<h2><a href="RaleighCaryDurhamEmploymentServices.aspx">Raleigh Employment Service Company List</a></h2>
				
				<h2><a href="RaleighCaryDurhamTechnicalStaffing.aspx">Raleigh / Durham Technical Staffing Companies</a></h2>
				
				<h2><a href="http://www.raleighjobs.com/" target="_blank">Raleigh Job Search Links</a></h2>
				
				<h2><a href="http://jobs.careerbuilder.com/?lr=cbmc_no&amp;siteid=cbmc_no002" target="_blank">News &amp; Observer Job Search Tool</a></h2>
				
				<h2><a href="http://triangle.bizjournals.com/triangle/jobs/" target="_blank">Triangle Business Journal Job Search Tool</a></h2>
				
				<h2><a href="https://www.bizjournals.com/subscription/subscribe.html" target="_blank">Subscribe to the Triangle Business Journal</a></h2>
				
				<h2><a href="http://www.bls.gov/eag/eag.nc_raleigh_msa.htm" target="_blank">The US Department of Labor Bureau of Statics for Raleigh-Cary, NC</a></h2>
				
				<h2><a href="ResearchTriangleParkEmployers.aspx">Top Research Triangle Park Employers</a></h2>
				
				<h2><a href="CaryNCMajorEmployers.aspx">Cary, NC top Employer List</a></h2>
				
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
