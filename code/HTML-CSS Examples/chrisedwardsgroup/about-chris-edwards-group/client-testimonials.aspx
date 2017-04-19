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
	<title>Client Testimonials for Chris Edwards RE/MAX United Cary, North Carolina.</title>
	
	<meta name="Description" content="Client testimonials for Chris Edwards with RE/MAX United in Cary, North Carolina.">

	<meta name="Keywords" content="Testimonials, Client Testimonials, Chris Edwards, Chris Edwards Group, Cary, North Carolina">

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
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate5.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-about.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
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
			
				<h1 class="body">Client Testimonials</h1>
					
				<p class="testimonial">&quot;His professional and innovative approach brought many prospective buyers into our home immediately, resulting in a quick sale!&quot;</p>
				
				<p class="indent">- Pat & Dick Genthner</p>
				
				<p class="testimonial">&quot;In the past we have dealt with many realtors and you by far exceed them all! You are most professional in all areas of customer service!&quot;</p>
				
				<p class="indent">- Paul Casavant / Rick Williams</p>
				
				<p class="testimonial">&quot;You were more than just a realtor selling a house, you were the one person that remained the voice of reason and calm and held our hand through the entire process. You returned every call, answered every question no matter how trivial, you listened and stuck with us through tears and disappointment.&quot;</p>
				
				<p class="testimonial">&quot;In the end it was you who ultimately found a way and turned what was a dire situation into a dream come true. We know that had it been any other realtor, we would not be sitting in this house on New Years Day!&quot;</p>
				
				<p class="indent">- David & Cindy Noordeloos</p>
				
				<p class="testimonial">&quot;The tour & image you created for our home no doubt made the difference in selling our home so quickly! Thank you for your dedication and going above and beyond the norm!&quot;</p>
				
				<p class="indent">- Jay & Marcy Jones</p>
				
				<p class="testimonial">&quot;Thank you so much for all that you did for us this year!</p>
				
				<p class="testimonial">You made our move here go so smoothly and we couldn't done it without you!&quot;</p>
				
				<p class="indent">- Doug & Cara Goodwin</p>
				
				<p class="testimonial">&quot;We were very impressed with your expertise and professionalism, especially in using the latest internet presentation and marketing techniques.&quot;</p>
				
				<p class="testimonial">&quot;It gave us a great deal of confidence and comfort given the present difficult market conditions. We could not be more pleased with your work and we would highly recommend you to anyone listing a home in the area!&quot;</p>
				
				<p class="indent">- Jim & Sylvia Gildenvan</p>
				
				<p class="testimonial">&quot;We have nothing but good things to say about Chris. He made everything very easy for us, answered every question and was always reachable and prompt.&quot;</p>
				
				<p class="indent">- Anil & Heeral Dedhia</p>
				
				<p class="testimonial">&quot;Chris is  great! He is honest, professional and fun.  We would definitely recommend him.&quot;</p>
				
				<p class="indent">- Steve & Alicia Pelton</p>
				
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
