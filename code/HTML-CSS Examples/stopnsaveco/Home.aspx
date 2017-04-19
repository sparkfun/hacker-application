<%@ Register Tagprefix="Top" Tagname="Header" Src="/includes/header.ascx" %>
<%@ Register Tagprefix="Center" Tagname="Navigation" Src="/includes/nav.ascx" %>
<%@ Register Tagprefix="Bottom" Tagname="Footer" Src="/includes/footer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
	<head>
		<title>Stop 'n Save Gas &amp; Convenience Stores: Customers First!</title>
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
		<meta name="description" content="Convenience and gas stores located all throughout Colorado." />
		<meta name="keywords" content="Stop 'n Save,convenience store,feather petroleum,gas,gas station,Colorado" />
		<meta name="author" content="Sean Smith: Firestar Design Company" />
		<link rel="stylesheet" type="text/css" media="screen" href="/includes/screen.css" />
		<link rel="stylesheet" type="text/css" media="print" href="/includes/print.css" />
		<link rel="shortcut icon" type="image/ico" href="/favicon.ico" />
		<script language="JavaScript" src="/includes/rollover.js"></script>
	</head>

	<body id="contact">
	
		<div id="body">
		
			<div id="header">
				
				<Top:Header id="ctlHeader" runat="server" />
				
			</div>
			
			<div id="left">
				
				<div id="leftSection">
				
					<a href="foods-beverages/Menu.aspx"><img src="graphics/summer-deals.jpg" alt="Summer Deals 2009" width="380" height="300" border="0" /></a>

					
					<table>
						<tbody>
							<tr>
								<td class="leftCol">
									
									<h2>Latest News</h2>
									
									<p><strong>Rock Jam</strong> - Rock Jam Tickets now on sale at all Stop 'n Save locations.</p>
									
									<p>Stop 'n Save is proud to announce that they will be the ticket outlet for the Colorado Pork and Hops Challenge. The event will be held at Lincoln Park in Grand Junction - September 18 and 19, 2009. Check back for more details.</p>
									
									<p>Stop 'n Save Gift Cards make great gifts, available at all locations.</p>
									
									<img src="/graphics/GiftCard.jpg" alt="Stop 'n Save Gift Cards make great gifts, available at all locations." width="220" height="151" border="0" />
									
								</td>
								<td>
								
									<div id="dealsSection">
										<h2>Hot Summer Specials</h2>
										
										<dl class="hotDeals">
											
											<dt>Aquafina, 12 pack</dt>
											<dd>16.9 ounce, $5.99</dd>
											
											<dt>Monster Energy Drink</dt>
											<dd>16 ounce, $1.79</dd>
											
											<dt>PowerAde 32 ounce</dt>
											<dd>2 for $1.69</dd>
											
											<dt>Sobe Life Water</dt>
											<dd>20 ounce, $1.29</dd>
											
											<dt>Coke, 14 ounce</dt>
											<dd>$.59</dd>
											
										</dl>
										
									</div>
									
									<p>Price may vary at some locations.</p>
									
									<h2>How to Find Our Stores</h2>
									
									<p>We've got maps to each of our stores so that you'll never have trouble finding us.</p>

									<p>Find one of our <a href="/stores/Stores-Home.aspx">store locations</a> now.</p>
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