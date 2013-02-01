<%@ Register Tagprefix="Top" Tagname="Header" Src="/includes/header.ascx" %>
<%@ Register Tagprefix="Center" Tagname="Navigation" Src="/includes/nav.ascx" %>
<%@ Register Tagprefix="Bottom" Tagname="Footer" Src="/includes/footer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
	<head>
		<title>Pay by Touch</title>
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
				
					<h1>Pay By Touch</h1>
					
					<h2 class="subheading">What is Pay By Touch?</h2>
					
					<img src="/graphics/pay_by_touch_logo.gif" class="float-left" alt="Pay By Touch Logo." width="150" height="53" border="0" />
					
					<p>Pay By Touch is a free service that allows you to pay for purchases and access loyalty discounts simply by placing your finger on a sensor when you check out. Your finger links you, and only you, to your accounts – completely eliminating the need to carry cards, checks or cash.</p>

					<p>
						Find out more about Pay By Touch at:&nbsp;&nbsp;<a href="http://www.paybytouch.com/" target="_blank">http://www.paybytouch.com/</a>
					</p>

					<table>
						<tbody>
							<tr>
								<td>
									<img src="/graphics/img_icon_safe.gif" alt="" width="34" height="34" border="0" />
								</td>
								<td>
									<h3>It's Safe</h3>
									<p>Your finger is unique to you, which means only you can access your financial accounts. The Pay By Touch service helps protect you from physical or identity theft. Because there’s nothing to carry, there’s nothing to be lost or stolen.</p>
								</td>
							</tr>
							<tr>
								<td>
									<img src="/graphics/img_icon_fast.gif" alt="" width="34" height="34" border="0" />
								</td>
								<td>
									<h3>It's Fast and Easy</h3>
									<p>Enroll once and use it anywhere! No writing checks. No cards to swipe. No fumbling with cash. No need to show your ID.</p>
								</td>
							</tr>
							<tr>
								<td>
									<img src="/graphics/img_icon_private.gif" alt="" width="34" height="34" border="0" />
								</td>
								<td>
									<h3>It's Private</h3>
									<p>Because you don’t have to present your cards, check or ID when you pay, no one can see your account information. Your information is securely stored and will not be sold to third parties.</p>
								</td>
							</tr>
							<tr>
								<td>
									<img src="/graphics/img_icon_free.gif" alt="" width="34" height="34" border="0" />
								</td>
								<td>
									<h3>It's Free</h3>
									<p>The Pay By Touch service is always free and there are no hidden fees.</p>
								</td>
							</tr>
						</tbody>
					</table>
					
					<p>
						Find out more about Pay By Touch at:&nbsp;&nbsp;<a href="http://www.paybytouch.com/" target="_blank">http://www.paybytouch.com/</a>
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