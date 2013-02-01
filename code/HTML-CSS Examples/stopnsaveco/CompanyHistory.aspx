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
				
					<h1>Company History</h1>
					
					<img src="/graphics/30th-cup-logo-small.gif" class="float-left" alt="30th Anniversary Stop 'n Save Logo." width="120" height="121" border="0" />
					
					<p>Feather Petroleum Company has the distinction of being the first gas station in Grand Junction to also sell groceries.  The first location, called Stop 'n Save, opened at First &amp; Rood.  Feather Petroleum currently operates seventeen Stop 'n Save stores in Colorado, spanning eight counties.</p>

					<p>Feather Petroleum has been family owned and operated since it was founded by Larry Feather, Stan Medsker and Andy Smith in 1977.  Larry retired in 2004 after serving as president for 27 years.  The company's president, Kent Frieling and Kathy Schoenfeld continue to operate the company as second generation owners.  The company employs over 150 people and attributes its longevity to its talented and dedicated management team, office staff, store managers and employees.</p>

					<p>Stop 'n Save continues to offer the latest in convenience by offering &quot;pay at the pump&quot;; in store scanning for speed and price accuracy; a Stop 'n Save gift card and mall card program; prepaid phone and cellular; pizza, sub and breakfast sandwiches; car washes; drive-thru windows at some Western Slope stores and quality Conoco, Phillips 66 and Timezone gasoline.  Most importantly, Stop 'n Save maintains a <em>&quot;Customers First&quot;</em> focus, realizing our primary mission is speed and convenience.</p>

					<p>Feather Petroleum Co. is proud of its long history of providing convenience to the people of Colorado and looks forward to many more years of service.</p>
					
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