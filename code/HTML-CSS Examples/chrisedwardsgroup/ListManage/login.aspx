<%@ Page Language="C#" Debug="False" Inherits="ChrisEdwardsGroup.ListManage.Login" %>

<!doctype html public "-//W3C//DTD HTML 4.01 Transitional//EN"
	"http://www.w3.org/TR/html4/loose.dtd">

<html>
	<head>
		<title>Chris Edwards Group - Listing Management Portal Login</title>
			<link rel="stylesheet" href="includes/login.css" type="text/css">
	</head>

	<body>
	
	<form runat="server">
	
	<h2 class="heading">Chris Edwards Group <br />
		<span class="font12pt">Management Portal</span></h2>
	
	<table class="login" cellpadding="16" border="3">
		<tbody>
		<tr>
			<td>User Name:</td>
			<td>
				<asp:TextBox id="txtUserName" Columns="25" MaxLength="15" runat="server" />
				<asp:RequiredFieldValidator id="RFVtxtUserName" ControlToValidate="txtUserName" Text="*" ErrorMessage="You must enter a User Name." runat="server" />
			</td>
		</tr>
		<tr>
			<td>Password:</td>
			<td>
				<asp:TextBox id="txtPassword" Columns="25" MaxLength="15" TextMode="Password" runat="server" />
				<asp:RequiredFieldValidator id="RFVtxtPassword" ControlToValidate="txtPassword" Text="*" ErrorMessage="You must enter a password." runat="server" />
			</td>
		</tr>
		<tr>
			<td colspan="2" class="center">
				<asp:Button id="btnLogin"
     Text="Login"
     CausesValidation="true"
     OnClick="DoLogin"
     runat="server"/>&nbsp;&nbsp;<input type="reset" id="btnReset" value="Reset" runat="server" />
			</td>
		</tr>
	</tbody>
	</table>
	
	
	<asp:ValidationSummary 
     id="valErrors" 
     DisplayMode="BulletList" 
     EnableClientScript="true"
     ShowSummary="true"
     ShowMessageBox="false"                        
     HeaderText="Summary of errors:"
					CssClass="errors"
     runat="server"/>
					
	<asp:Label id="lblError" visible="false" CssClass="errors" runat="server"/>
	
	

</form>

	</body>
</html>
