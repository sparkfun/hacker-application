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
	<title>Why Hire Chris Edwards? Testimonial Letter</title>
	
	<meta name="Description" content="Why Hire Chris Edwards?">

	<meta name="Keywords" content="Raleigh, Cary, Apex, Triangle, Durham, Chapel Hill, North Carolina">

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
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate1.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-welcome-triangle.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1 class="body">
					Why Hire Chris Edwards?
				</h1>
									
				<p>July 4, 2008<br />
				Chris Edwards<br />
				Chris Edwards Group<br />
				RE/MAX United Raleigh-Cary<br />
				51 Kilmayne Dr., Ste. 100<br />
				Cary, NC 27511<br /></p>
				
				<p>Dear Chris,</p>
				
				<p>I hope that you and your family are doing well personally and professionally.</p> 
				
				<p>As I recall our conversations during the home buying process, I would be remiss if I didn't express my gratitude for all the help you provided me. As a first time homebuyer, there were a lot of questions surrounding the process which I was unsure about. As you know, I interviewed several real estate agents prior to starting our working relationship using a wide array of sources.  After this intense process, it was clear that your professionalism, expertise, organizational and personal skills stood out from the other candidates.</p>

				<p>Throughout the home buying process, you were always extremely reliable and pointed out several options for each situation that came up. It certainly not only made me feel like I was making the right decisions, but the right informed decisions. However, the best compliment I can provide is simply that regardless of the time, you were always available. Due to the fact that I was relocating due to a position change and was from out of state, I did not have the time to talk during normal business hours. The fact that you took the time to speak/meet with me whether it was midnight, during the weekend, after business hours, or even while you were on vacation speaks volumes about your dedication to your profession.</p>

				<p>As I settle into my new house, it is extremely comforting to know that I made the right decision for myself. This feeling would not have been possible had it not been for your assistance and guidance. Anyone that is looking for the best real estate agent in the area should not look further than you.  If you should ever need a reference for one of your future clients, please do not hesitate to contact me. I look forward to continuing our friendship for many years to come.</p>

				<p>Cordially,</p>
				
				<p>Carlos Cruz<br />
				CAC</p>
				
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
