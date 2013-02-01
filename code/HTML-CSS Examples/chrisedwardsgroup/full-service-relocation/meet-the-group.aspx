<%@ Register Tagprefix="Top" Tagname="FeaturedProperty" Src="/includes/featured.ascx" %>
<%@ Register Tagprefix="Top" Tagname="TopLinks" Src="/includes/toplinks.ascx" %>
<%@ Register Tagprefix="Top" Tagname="LeftLinks" Src="/includes/leftlinks.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Address" Src="/includes/address.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Neighborhoods" Src="/includes/neighborhoods-group.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Logos" Src="/includes/logos.ascx" %>
<%@ Register Tagprefix="Bottom" Tagname="Footer" Src="/includes/footer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
	"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
	<title>Meet the Chris Edwards Group - Full Service Relocation to Raleigh, Cary and Apex, NC</title>
	
	<meta name="Description" content="Meet the Chris Edwards Group - Full Service Relocation to Raleigh, Cary and Apex, NC.">

	<meta name="Keywords" content="Meet the Chris Edwards Group, Raleigh Relocation, North Carolina Relocation, Full Service Relocation">

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
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate4.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-relocation.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1>Meet the Chris Edwards Group</h1>

				<p>I could not provide the kind of premium service that makes up Full Service Relocation without the solid partnerships of the people listed below.</p>
				
				<p>I have created the &quot;group&quot; concept to take most of the stress and time out of finding good vendors to make your relocation a smooth one.</p>
				
				<p>The &quot;partners&quot; listed below are part of the group due to their same commitment to providing premium service when it comes to every client's relocation. These partners are only being referred due to their outstanding track record with my business and dedication to every client's needs.</p>
				
				<p>Please feel free to contact any of these partners below to ask questions or receive more helpful information. Click on any name below to open a complete profile for each group member.</p>

				<p><strong>CEG Co-Associate</strong></p>
				
				<p><a href="meet-the-group/vinesett-rich.aspx">Rich Vinesett - RE/MAX United</a></p>
				
				<p><strong>Commercial Real Estate Partner</strong></p>
				
				<p><a href="meet-the-group/clayton-janet.aspx">Janet Clayton - Grubb &amp; Ellis TLG</a></p>
				
				<p><strong>Mortgage &amp; Lending</strong></p>
				
				<p><a href="meet-the-group/moore-jim.aspx">Jim Moore - First Captial (Wells Fargo Products)</a></p>
				
				<p><a href="meet-the-group/taylor-mike.aspx">Mike Taylor - Charter Funding</a></p>
				
				<p><a href="meet-the-group/macklin-alex.aspx">Alex Macklin - Crescent State Bank</a></p>
				
				<p><strong>Moving &amp; Storage Services</strong></p>
				
				<p><a href="meet-the-group/mather-greg.aspx">Greg Mather - Mather Brothers / Wheaton Van Lines</a></p>
				
				<p><a href="meet-the-group/lennon-evan.aspx">Evan Lennon - Carolina PODS #15 (Raleigh)</a></p>
				
				<p><strong>Home Owner's Insurance</strong></p>
				
				<p><a href="meet-the-group/farless-tim.aspx">Tim Farless - State Farm Insurance</a></p>
				
				<p><a href="meet-the-group/bagherpour-cameron.aspx">Cameron Bagherpour - Allstate Insurance</a></p>
				
				<p><strong>Long &amp; Short Term Apartments</strong></p>
				
				<p><a href="meet-the-group/jessica-becky.aspx">Jessica &amp; Becky - Equity Corporate Housing</a></p>
				
				<p><a href="meet-the-group/burkle-diane.aspx">Diane Burkle - The Park in Cary</a></p>
				
				<p><strong>Investing</strong></p>
				
				<p><a href="meet-the-group/cobb-jill.aspx">Jill Cobb - Edward Jones</a></p>
				
				<p><strong>Real Estate Attorney</strong></p>
				
				<p><a href="meet-the-group/bryan-david.aspx">David N. Bryan, P.A.</a></p>
				
				<p><a href="meet-the-group/richardson-jonathan.aspx">Law Office of Jonathan Richardson</a></p>
				
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
