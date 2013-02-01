<%@ Register Tagprefix="Top" Tagname="Header" Src="/includes/header.ascx" %>
<%@ Register Tagprefix="Center" Tagname="Navigation" Src="/includes/nav.ascx" %>
<%@ Register Tagprefix="Bottom" Tagname="Footer" Src="/includes/footer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
	<head>
		<title>Stop 'n Save Convenience Stores: Fountain Drinks and Beverages</title>
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
				
					<h1>Fountain Drinks and Beverages</h1>
					
					<h2 class="subheading">Experience a Wave of Refreshment</h2>
					
					<p>We offer a full range of Coke and Pepsi products at our fountains guaranteed to quench your thirst.</p>
					
					<table id="foods-beverages">
						<tbody>
							<tr>
								<td class="product-image">
									<img src="/graphics/soda-small.jpg" alt="Fountain Drinks." width="85" height="178" border="0" />
								</td>
								<td>
									<h3>Soda Fountain</h3>
									
									<p>Self-serve fountain drinks for a &quot;Wave of Refreshment&quot;</p>
									
									<ul>
										<li>Mountain Dew</li>
										<li>Diet Mountain Dew</li>
										<li>Coke</li>
										<li>Diet Coke</li>
										<li>Sierra Mist</li>
										<li>Dr. Pepper</li>
										<li>Diet Dr. Pepper</li>
										<li>Mug Root Beer</li>
									</ul>
								</td>
							</tr>
							<tr>
								<td class="product-image">
									<img src="/graphics/soda-clear-small.jpg" alt="Soda Drinks." width="85" height="161" border="0" />
								</td>
								<td>
									<h3>Other Drinks</h3>
									
									<p>Fresh brewed drinks and fruit beverages.</p>
									
									<ul>
										<li>Boyd's Iced Tea - Brewed Fresh All Day</li>
										<li>Tropicana Lemonade</li>
									</ul>
								</td>
							</tr>
						</tbody>
					</table>
					
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