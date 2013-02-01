<%@ Register Tagprefix="Top" Tagname="Header" Src="/includes/header.ascx" %>
<%@ Register Tagprefix="Center" Tagname="Navigation" Src="/includes/nav.ascx" %>
<%@ Register Tagprefix="Bottom" Tagname="Footer" Src="/includes/footer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
	<head>
		<title>Latest Stop 'n Save News</title>
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
		<meta name="description" content="" />
		<meta name="keywords" content="" />
		<meta name="author" content="Sean Smith: Firestar Design Company" />
		<link rel="stylesheet" type="text/css" media="screen" href="/includes/screen.css" />
		<link rel="stylesheet" type="text/css" media="print" href="/includes/print.css" />
		<link rel="shortcut icon" type="image/ico" href="/favicon.ico" />
		<script language="JavaScript" src="/includes/rollover.js"></script>
		<script language="JavaScript" src="/includes/radrotator_client.js"></script>
	</head>

	<body id="contact">
	
		<div id="body">
		
			<div id="header">
				
				<Top:Header id="ctlHeader" runat="server" />
				
			</div>
			
			<div id="left">
				
				<div id="leftSection">
				
					<h2>Latest News</h2>
					
					<p><strong>Moving Dirt</strong></p>

					<p>Store #22, Located near 104th and Chambers in Commerce City, north of Denver. Ground breaking this Spring with an opening date of Fall 2009.</p>
					
					<p><a href="http://countryjamgjco.countryjam.com/" target="_blank"><strong>Country Jam</strong></a> - Country Jam Tickets now on sale at all Stop 'n Save locations.</p>
									
					<p><a href="http://www.sunlightmtn.com/" target="_blank"><strong>Ski Sunlight Super Deal</strong></a> - Stop at any participating Stop 'n Save location and when you purchase 8 gallons of fuel or more, receive a voucher good for a half price Lift Ticket to Sunlight Mountain Resort with the purchase of one at full price.</p>
					
					<p><a href="http://skifreecolorado.com/home/" target="_blank"><strong>Ski Free Colorado</strong></a> - If you would like to ski free with Phillips 66, stop by a participating Stop 'n Save and purchase a minimum of 10 gallons of quality gasoline, diesel or E85 and receive inside the store a Ski Free voucher good for a free adult lift ticket with the purchase of a full price adult lift ticket. Some restrictions may apply.</p>
					
					<a href="http://skifreecolorado.com/home/" target="_blank"><img src="/graphics/2009SkipromoSM.jpg" alt="2009 Colorado Ski Promotion" width="220" height="182" border="0" /></a>
	
					<h2>Makes A Great Gift Any Time of Year!</h2>
	
					<img src="/graphics/GiftCard.jpg" alt="Stop 'n Save Gift Cards make great gifts, available at all locations." width="220" height="151" border="0">
					
					<p>Stop 'n Save Gift Cards make great gifts, available at all locations.</p>
					
				</div>
				
			</div>
			
			<div id="right">
				
				<div id="priNav">
				
					<Center:Navigation id="ctlNav" runat="server" />
				
				</div>
			
			</div>
			
			<div id="footer">
			
				<Bottom:Footer id="ctlFooter" runat="server" />

			</div>
		
		</div>

	</body>
</html>