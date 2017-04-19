<%@ Register Tagprefix="Top" Tagname="Header" Src="/includes/header.ascx" %>
<%@ Register Tagprefix="Center" Tagname="Navigation" Src="/includes/nav.ascx" %>
<%@ Register Tagprefix="Bottom" Tagname="Footer" Src="/includes/footer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
	<head>
		<title>Customers First Statement</title>
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
					
					<h2>Customers First Statement</h2>
					
					<dl class="custFirst">
						<dt>C</dt>
						<dd>Customer is the boss</dd>
						<dt>U</dt>
						<dd>Upbeat, enthusiastic attitude</dd>
						<dt>S</dt>
						<dd>See Through the customers eyes</dd>
						<dt>T</dt>
						<dd>Treat every customer with respect and dignity</dd>
						<dt>O</dt>
						<dd>Observe the condition of the store-keep it clean</dd>
						<dt>M</dt>
						<dd>Memorize the customers names</dd>
						<dt>E</dt>
						<dd>Eye contact - let every customer know that you see them</dd>
						<dt>R</dt>
						<dd>Respond to the needs of the customer - offer assistance</dd>
						<dt>S</dt>
						<dd>Stock and front the shelves - know your products</dd>
					</dl>
						
						<br />
					
					<dl class="custFirst">
						<dt>F</dt>
						<dd>Fast, friendly service</dd>
						<dt>I</dt>
						<dd>Image - always look your best</dd>
						<dt>R</dt>
						<dd>Resolve customer complaints in a flexible, positive manner</dd>
						<dt>S</dt>
						<dd>Suggest additional sales - ring sales accurately</dd>
						<dt>T</dt>
						<dd>Thank every customer and ask them to return</dd>
					</dl>
					
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