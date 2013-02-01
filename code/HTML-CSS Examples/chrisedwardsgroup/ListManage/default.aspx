<%@ Page Language="C#" Debug="False" Inherits="ChrisEdwardsGroup.ListManage.Main" %>

<%@ Register Tagprefix="Top" Tagname="Bar" Src="includes/topbar.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<html>
	<head>
		<title>Chris Edwards Group - Listings Management: Home Page</title>
			<link rel="stylesheet" href="includes/listing_management.css" type="text/css">
			<script language="JavaScript1.2" src="includes/menu.js"></script>
			<script language="JavaScript1.2" src="includes/stm_script.js"></script>
	</head>

	<body>
	
	<form runat="server">
	
	<table cellspacing="0" cellpadding="0" border="1" class="main">
		<tr>
			<Top:Bar id="ctlTopBar" runat="server" />
		</tr>
		<tr>
  	<td rowspan="2" class="leftbar">
				<script language="JavaScript1.2" src="includes/stm_menu.js"></script>
			</td>
   <td colspan="3">
				<table border="0" class="backgroundtitleqnav">
					<tr>
						<td class="pagetitle">Listings Management: Home Page</td>
					</tr>
				</table>
			</td>
	</tr>
	<tr>
    <td colspan="3" class="innercontent">
							
							<table border="1" cellpadding="3" class="homepagetop">
								<tr>
									<th class="sectionhead" colspan="2">Home Page:</th>
								</tr>
							</table>
							
							<table border="1" cellpadding="3" class="newsannounce">
								<tr>
									<th class="newsannouncetop">News and Announcements:</th>
								</tr>
							</table>
			
				</td>
</tr>
</table>

</form>

	</body>
</html>
