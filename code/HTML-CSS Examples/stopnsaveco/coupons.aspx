<%@ Register Tagprefix="Top" Tagname="Header" Src="/includes/header.ascx" %>
<%@ Register Tagprefix="Center" Tagname="Navigation" Src="/includes/nav.ascx" %>
<%@ Register Tagprefix="Bottom" Tagname="Footer" Src="/includes/footer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
	<head>
		<title>Free Coupons from Stop 'n Save</title>
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
				
					<h1>Free Coupons</h1>
					
					<table id="Coupons">
						<tbody>
							<tr>	
								<td class="couponText">
									<p>
										<strong>Summer Coupons</strong><br />
										Print and Present Coupon(s) at Store<br />
										Expires: See Coupon For Details
									</p>
								</td>
								<td>
									<p class="reportDownload">
										<a href="coupons/MonthlyCoupon.pdf" target="_blank">Click here to download or print</a> Coupon Set #1.
									</p>
								</td>
							</tr>
							<tr>	
								<td class="couponText">
									<p>
										&nbsp;
									</p>
								</td>
								<td>
									<p class="reportDownload">
										<a href="coupons/MonthlyCoupon2.pdf" target="_blank">Click here to download or print</a> Coupon Set #2.
									</p>
								</td>
							</tr>
							<tr>	
								<td class="couponText">
									<p>
										&nbsp;
									</p>
								</td>
								<td>
									<p class="reportDownload">
										<a href="coupons/MonthlyCoupon3.pdf" target="_blank">Click here to download or print</a> Coupon Set #3.
									</p>
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