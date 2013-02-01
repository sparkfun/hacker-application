<%@ Register Tagprefix="Top" Tagname="Header" Src="/includes/header.ascx" %>
<%@ Register Tagprefix="Center" Tagname="Navigation" Src="/includes/nav.ascx" %>
<%@ Register Tagprefix="Bottom" Tagname="Footer" Src="/includes/footer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
	<head>
		<title>Fuel Home Page</title>
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

	<body>
	
		<div id="body">
		
			<div id="header">
				
				<Top:Header id="ctlHeader" runat="server" />
				
			</div>
			
			<div id="left">
				
				<div id="leftSection">
				
					<h1>Our Fuel</h1>
					
					<p><strong>Stop 'n Save</strong> is pleased to sell quality Phillips 66 and Conoco PROclean gasoline products at its branded locations.  In 2004, ConocoPhillips PROclean products were recognized as being a &quot;Top Tier Detergent Gasoline&quot;, which is critical to the performance of your car.</p>

					<!-- promo photo here -->
					
					<!-- promo photo here -->
					
					<h2>What do Quality PROclean Gasolines do for my car?</h2>
					
					<p>Using Quality PROclean Gasolines will maximize your car's performance by helping to:</p>
					
					<ul>
						<li>Maximize miles per gallon</li>
						<li>Make engines up to 35 percent cleaner after five tanks</li>
						<li>Reduce vehicle emissions and pollution</li>
						<li>Protect against wear and tear</li>
						<li>Add power and acceleration</li>
					</ul>
					
					<h2>How does a detergent additive program work?</h2>
					
					<p>A detergent additive program works by cleaning your car's fuel system and intake valves. When your engine is running clean, your car can perform at its best.</p>

					<h2>What is &quot;Top Tier&quot; and why is it important?</h2>
					
					<p>TOP TIER Detergent Gasoline is the premier standard for gasoline performance. Six of the world's top automakers, <strong>BMW, General Motors, Honda, Toyota, Volkswagen and Audi</strong> recognize that the current EPA minimum detergent requirements do not go far enough to ensure optimal engine performance.</p>

					<p>Since the minimum additive performance standards were first established by EPA in 1995, most gasoline marketers have actually reduced the concentration level of detergent additive in their gasoline by up to 50%.  As a result, the ability of a vehicle to maintain stringent Tier 2 emission standards have been hampered, leading to engine deposits which can have a big impact on in-use emissions and driver satisfaction.  These automakers have raised the bar. TOP TIER Detergent Gasoline help drivers avoid lower quality gasoline which can leave deposits on critical engine parts, which reduces engine performance.  That's something both drivers and automakers want to avoid.</p>
					
					<p>For more information on our gasoline products, please browse the following sites:</p>
					
					<p>
						Find out more about quality ConocoPhillips gasoline at:<br /><br />
						<a href="http://p66conoco76.conocophillips.com/" target="_blank">http://p66conoco76.conocophillips.com/</a>
					</p>
					
					<p>
						Find out more about PROclean at:<br /><br />
						<a href="http://p66conoco76.conocophillips.com/proclean.aspx" target="_blank">http://p66conoco76.conocophillips.com/proclean.aspx</a>
					</p>
					
					<p>
						Find out more about TOP TIER gasoline:<br /><br />
						<a href="http://www.toptiergas.com/" target="_blank">http://www.toptiergas.com/</a>
					</p>
					
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