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
	<title>Meet the Chris Edwards Group - Greg &amp; Mike Mather - Full Service Relocation to Raleigh, Cary and Apex, NC</title>
	
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
			
				<h1>Meet Greg &amp; Mike Mather, Mather Brothers / Wheaton Moving</h1>
				
				<img src="/images/group-photos/mather-bros.jpg" alt="" width="200" height="194" border="0" id="photo" />
				
				<a href="http://www.matherbrothers.com/" target="_blank"><img src="/images/group-photos/mather-bros-logo.jpg" alt="" width="202" height="131" border="0" id="photo" /></a>
				
				<table id="address">
					<tr>
						<td>
							<strong>Greg &amp; Mike Mather</strong><br />
							Mather Brothers / Wheaton Moving<br />
							1532-B West Garner Road<br />
							Garner, NC 27529<br /><br />
							
							<strong>Contact information for Mather Brothers</strong><br />
							<a href="mailto:greg.mather@matherbrothers.com">greg.mather@matherbrothers.com</a><br />
							<a href="http://www.matherbrothers.com/" target="_blank">www.matherbrothers.com</a><br />
							Office: 919-662-7800<br />
							Toll Free: 800-851-0287
						</td>
					</tr>
				</table>
				
				<p>Greg and Mike Mather grew up in the moving industry. In 1984 at 14 and 15 years old they took their first jobs working for their uncles who were drivers for a nation-wide moving &amp; storage company. They learned from the beginning that moving families is more than just an impersonal service and that customers must trust their movers with everything they have worked their entire lives for. They realized then that the key to success in this difficult industry was to show their customers that they were skilled in their trade, fair to deal with, care about their reputation and the quality of their service.</p>

				<p>In 1998 Greg and Mike founded their own moving and storage company based on principles they learned early on. They chose the name Mather Brothers Moving Company so that customers would know who they were doing business with. In an effort to make their customers feel comfortable they hire people who are competent, professional, experienced and who show personal pride in the services they provide. They work each day to develop close bonds with their crews since they are the ones who represent the company in each customer’s home.</p>

				<p>Greg &amp; Mike Mather have received awards for outstanding customer service, claims prevention, outstanding achievement in sales and together have received Wheaton Van Lines Quality Agent Award for the years 2000, 2001, 2002 and are on target to receive this prestigious award again in 2003. Through a hands on approach and philosophy, Mather Brothers Moving Company has experienced incredible growth during a time of recession. With a quality first mindset, we hire only top quality employees. Our staff of trained professionals strives for one hundred percent customer satisfaction. Through our dedication to the customer, we are becoming one of Wheaton World Wide Moving’s top quality agents.</p>

				<img src="/images/group-photos/mather-truck.jpg" alt="" width="457" height="131" border="0" id="photo" />

				<p>Let the Mather Brothers provide you with a professional and reliable moving experience to the Triangle!</p>
				
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
