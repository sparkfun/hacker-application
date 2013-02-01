<%@ Register Tagprefix="Top" Tagname="Header" Src="/includes/header.ascx" %>
<%@ Register Tagprefix="Center" Tagname="Navigation" Src="/includes/nav.ascx" %>
<%@ Register Tagprefix="Bottom" Tagname="Footer" Src="/includes/footer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
	<head>
		<title>Our Menu</title>
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
				
					<h1>Our Menu</h1>
					
					<img src="/graphics/fruits.jpg" class="float-right" alt="" width="166" height="250" border="0" />
					
					<p>With today's busy schedules and lifestyles it's sometimes a challenge to find quality food while on the run. Here at Stop 'n Save you can be assured that with every visit at anytime of day you will find a variety of freshly prepared items to satisfy and delight your taste buds.</p>
					
					<p>So whether you need that jump start in the morning with a fresh Breakfast Sandwich and pastry or Sub Sandwich and  Salad for lunch or even take home a fresh made to order Pizza, Stop 'n Save delivers quality you can count on.</p>
					
					<p>
						*Variety and selection may vary between locations.
					</p>

					<fieldset class="menu">
						<legend>Pizza</legend>
						<img src="/graphics/pizza-group.jpg" alt="Pizza." width="410" height="83" border="0" />
					</fieldset>
					
					<fieldset class="menu">
						<legend>Cookies and Pastries</legend>
						<img src="/graphics/sweets-group.jpg" alt="Cookies, Donuts and Pastries." width="410" height="83" border="0" />
					</fieldset>
					
					<fieldset class="menu">
						<legend>Subs, Hamburgers and Hot Dogs</legend>
						<img src="/graphics/subs-hamburgers-group.jpg" alt="Subs, Hamburgers and Hot Dogs." width="410" height="83" border="0" />
					</fieldset>
					
					<fieldset class="menu">
						<legend>Soups and Salads</legend>
						<img src="/graphics/soups-salads-group.jpg" alt="Soups and Salads." width="410" height="83" border="0" />
					</fieldset>
					
					<fieldset class="menu">
						<legend>Breakfast Stuff</legend>
						<img src="/graphics/breakfast-sands-group.jpg" alt="Breakfast Sandwiches and Burritos." width="410" height="83" border="0" />
					</fieldset>
					
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