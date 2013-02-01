<%@ Page Language="C#" Debug="False" Inherits="ChrisEdwardsGroup.ListManage.AddAgent" %>

<%@ Register Tagprefix="Top" Tagname="Bar" Src="includes/topbar.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<html>
	<head>
		<title>Chris Edwards Group - Listing Management: Add Agent</title>
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
						<td class="pagetitle">Add Agent</td>
					</tr>
				</table>
			</td>
	</tr>
	<tr>
    <td colspan="3" class="innercontent">
				
				<asp:Label id="lblStatus" visible="false" runat="server"/>
							
				<table border="0" cellpadding="3">
					<tbody>
						<tr>
							<th class="sectionhead" colspan="2">Personal Information</th>
						</tr>
						<tr>
							<th>First Name:</th>
							<th>Last Name:</th>
						</tr>
						<tr>
							<td class="padleftinput">
								<asp:TextBox id="txtFirstName" MaxLength="30" Columns="30" runat="server" />
								<asp:RequiredFieldValidator id="RFVtxtFirstName" ControlToValidate="txtFirstName" ErrorMessage="*" runat="server" />
							</td>
							<td class="padleftinput">
								<asp:TextBox id="txtLastName" MaxLength="30" Columns="30" runat="server" />
								<asp:RequiredFieldValidator id="RFVtxtLastName" ControlToValidate="txtLastName" ErrorMessage="*" runat="server" />
							</td>
						</tr>
					</tbody>
				</table>
						
				<table border="0" cellpadding="3">
					<tbody>
						<tr>
							<th class="sectionhead" colspan="2">Contact Information</th>
						</tr>
						<tr>
							<th>Office Telephone:</th>
							<th>Home Telphone:</th>
						</tr>
						<tr>
							<td class="padleftinput">
								<asp:TextBox id="txtOfficeTelephone" MaxLength="14" Columns="14" runat="server" />
								<asp:RequiredFieldValidator id="RFVtxtOfficeTelephone" ControlToValidate="txtOfficeTelephone" ErrorMessage="*" runat="server" />
							</td>
							<td class="padleftinput">
								<asp:TextBox id="txtHomeTelephone" MaxLength="14" Columns="14" runat="server" />
							</td>
						</tr>
						<tr>
							<th>Toll-Free Telephone:</th>
							<th>Fax:</th>
						</tr>
						<tr>
							<td class="padleftinput">
								<asp:TextBox id="txtTollFreeTelephone" MaxLength="14" Columns="14" runat="server" />
							</td>
							<td class="padleftinput">
								<asp:TextBox id="txtFax" MaxLength="14" Columns="14" runat="server" />
							</td>
						</tr>
						<tr>
							<th colspan="2">E-mail Address:</th>
						</tr>
						<tr>
							<td class="padleftinput" colspan="2">
								<asp:TextBox id="txtEmail" MaxLength="50" Columns="30" runat="server" />
								<asp:RequiredFieldValidator id="RFVtxtEmail" ControlToValidate="txtEmail" ErrorMessage="*" runat="server" />
							</td>
						</tr>
					</tbody>
				</table>
				
					<table border="0" cellpadding="3">
					<tbody>
						<tr>
							<th class="sectionhead" colspan="2">Website Information</th>
						</tr>
						<tr>
							<th>Website Address:</th>
						</tr>
						<tr>
							<td class="padleftinput">
								<asp:TextBox id="txtWebsiteURL" MaxLength="100" Columns="75" runat="server" />
							</td>
						</tr>
					</tbody>
				</table>
				
				<table>
					<tbody>
						<tr>
							<td><asp:Button id="submitButton" CssClass="buttons" Text="Add Agent" CausesValidation="true" OnClick="AddAgent_Click" runat="server"/>&nbsp;&nbsp;<input type="reset" class="buttons"></td>
						</tr>
					</tbody>
				</table>
			
				</td>
			</tr>
		</table>
		
				</td>
</tr>
</table>

	</form>
	</body>
</html>
