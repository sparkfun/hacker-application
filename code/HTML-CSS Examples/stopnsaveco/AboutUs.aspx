<%@ Register Tagprefix="Top" Tagname="Header" Src="/includes/header.ascx" %>
<%@ Register Tagprefix="Center" Tagname="Navigation" Src="/includes/nav.ascx" %>
<%@ Register Tagprefix="Bottom" Tagname="Footer" Src="/includes/footer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
	<head>
		<title>About Stop 'n Save</title>
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
				
					<h1>About Stop 'n Save</h1>
					
					<p>Feather Petroleum Company is a locally owned company which owns and operates eighteen Stop 'n Save convenience stores throughout nine counties in Colorado, six of which are located in Grand Junction, Colorado.</p>

					<p>We invite you to &quot;stop in&quot; at any of our stores and see the difference our commitment to quality and fast service makes.</p>

					<p>We Sell Quality <a href="http://p66conoco76.conocophillips.com/" target="_blank">Conoco/Phillips 66</a> and Timezone Gasoline</p>
					
					<h1>What Makes Stop 'n Save Better?</h1>
					
					<h2 class="subheading">It all comes back to the &quot;Golden Rule&quot;:  How do you like to be treated?</h2>
				
					<p>That is why we pride ourselves in the Cleanliness of our stores and the Customer Service we offer. We want to exceed your expectations with every visit. Starting with a greeting at the door, our full attention and assistance while you shop and fast courteous service when at the sales counter.</p>

					<p>We made that commitment to you.  To offer the best Customer Service you have ever experienced in the cleanest stores.</p>
					
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