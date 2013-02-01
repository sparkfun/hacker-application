<%@ Register Tagprefix="Top" Tagname="Header" Src="/includes/header.ascx" %>
<%@ Register Tagprefix="Center" Tagname="Navigation" Src="/includes/nav.ascx" %>
<%@ Register Tagprefix="Bottom" Tagname="Footer" Src="/includes/footer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
	<head>
		<title>Grand Junction (2050 North Ave.), Colorado Stop 'n Save Store Details</title>
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
		<meta name="description" content="" />
		<meta name="keywords" content="" />
		<meta name="author" content="Sean Smith: Firestar Design Company" />
		<link rel="stylesheet" type="text/css" media="screen" href="/includes/screen.css" />
		<link rel="stylesheet" type="text/css" media="print" href="/includes/print.css" />
		<link rel="shortcut icon" type="image/ico" href="/favicon.ico" />
		<script language="JavaScript" src="/includes/rollover.js"></script>
		<script language="JavaScript" src="/includes/radrotator_client.js"></script>
		<script language="JavaScript1.2" src="/includes/slideshow.js"></script>
	</head>

	<body>
	
		<div id="body">
		
			<div id="header">
				
				<Top:Header id="ctlHeader" runat="server" />
				
			</div>
			
			<div id="left">
				
				<div id="leftSection">
				
					<h1>Grand Junction (2050 North Ave.)&nbsp;&nbsp;&nbsp;<a href="http://www.mapquest.com/maps/map.adp?address=2050%20North%20Ave&city=Grand%20Junction&state=CO&zipcode=81501&country=US&title=%3cspan%20style%3d%22margin%2dbottom%3a0px%3b%22%20class%3d%22adr%22%3e%3cb%20class%3d%22fn%20org%22%3eStop%20N%20Save%3a%3c%2fb%3e%3cspan%20style%3d%22margin%2dbottom%3a0px%3b%22%20class%3d%22nowrap%20phnum%20tel%20work%22%3e970%2d243%2d5726%3c%2fspan%3e%20%3cspan%20style%3d%22margin%2dbottom%3a0px%3b%22%20class%3d%22street%2daddress%22%3e2050%20North%20Ave%3c%2fspan%3e%20%3cspan%20style%3d%22display%3ainline%3bmargin%2dbottom%3a0px%3b%22%20class%3d%22locality%22%3eGrand%20Junction%3c%2fspan%3e%2c%20%3cspan%20style%3d%22display%3ainline%3bmargin%2dbottom%3a0px%3b%22%20class%3d%22region%22%3eCO%3c%2fspan%3e%20%3cspan%20style%3d%22display%3ainline%3bmargin%2dbottom%3a0px%3b%22%20class%3d%22postal%2dcode%22%3e81501%3c%2fspan%3e%2c%20%20%3cspan%20style%3d%22display%3ainline%3bmargin%2dbottom%3a0px%3b%22%20class%3d%22country%2dname%22%3eUS%3c%2fspan%3e%3c%2fspan%3e&cid=lfmaplink2&name=Stop%20N%20Save&dtype=s" target="_blank">View Map To Store</a></h1>
					
					<fieldset class="storeDetails">
						<legend>Address &amp; Contact Info</legend>
						<table>
							<tbody>
								<tr>
									<td>
										2050 North Ave.<br />
										Grand Junction, CO 81501
									</td>
									<td>
										Phone: 970-243-5726<br />
										Fax: 970-243-7818
									</td>
								</tr>
							</tbody>
						</table>
					</fieldset>
					
					<fieldset class="storeDetails">
						<legend>Store Photos</legend>
						<table>
							<tbody>
								<tr>
									<td>
										<img src="/graphics/stores/2/store-2-1-full.jpg" alt="Main Store Photo." width="385" height="256" border="0" name="slidePhotoDisplay1" />
									</td>
								</tr>
								<tr>
									<td>
										<p class="clickPhoto">Click thumbnail photo below to see larger photo.</p>
										<table id="storePhotos">
											<tbody>
												<tr>
													<td>
														<img src="/graphics/stores/2/store-2-1-thumb.jpg" alt="Clifton Store Photo 1." width="75" height="50" border="0" onclick="imgSlideOnOff('/graphics/stores/2/store-2-1-full.jpg', 'slidePhotoDisplay1')" />
													</td>
													<td>
														<img src="/graphics/stores/2/store-2-2-thumb.jpg" alt="Clifton Store Photo 2." width="75" height="50" border="0" onclick="imgSlideOnOff('/graphics/stores/2/store-2-2-full.jpg', 'slidePhotoDisplay1')" />
													</td>
													<td colspan="3">
														&nbsp;
													</td>
												</tr>
											</tbody>
										</table>
										
									</td>
								</tr>
							</tbody>
						</table>
					</fieldset>
					
					<fieldset class="storeDetails">
						<legend>Features</legend>
						<ul>
							<li>Quality Stop 'n Save Fuel</li>
							<li>3 cent cash discount on Fuel</li>
							<li>Touch Free Car Wash</li>
							<li>Vacuum</li>
							<li>ATM</li>
							<li>Prepaid Services</li>
							<li>Boyd's Gourmet Coffee and Cappuccino</li>
							<li>Gourmet Ice</li>
							<li>Car Care Products</li>
							<li>Free Air</li>
							<li>Gourmet Ice Frozen Beverage</li>
						</ul>
					</fieldset>
					
					<fieldset class="storeDetails">
						<legend>Store Hours</legend>
						<p>
							Monday-Saturday: 5:30AM to 10PM<br />
							Sunday: 6AM to 10PM
						</p>
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