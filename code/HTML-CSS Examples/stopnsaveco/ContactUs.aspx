<%@ Page EnableViewState="True" Inherits="StopNSave.Website.ContactForm" %>

<%@ Register TagPrefix="vam" Namespace="PeterBlum.VAM" Assembly="PeterBlum.VAM" %>
<%@ Register Tagprefix="Top" Tagname="Header" Src="/includes/header.ascx" %>
<%@ Register Tagprefix="Center" Tagname="Navigation" Src="/includes/nav.ascx" %>
<%@ Register Tagprefix="Bottom" Tagname="Footer" Src="/includes/footer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
	<head>
		<title>Contact Stop 'n Save</title>
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
		<meta name="description" content="" />
		<meta name="keywords" content="" />
		<meta name="author" content="Sean Smith: Firestar Design Company" />
		<link rel="stylesheet" type="text/css" media="screen" href="/includes/screen.css" />
		<link rel="stylesheet" type="text/css" media="screen" href="/includes/forms.css" />
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
				
					<div id="companyContact">
						<strong>Feather Petroleum Company</strong><br />
						2492 Industrial Blvd.<br />
						Grand Junction, CO 81505<br />
						970-242-5205
					</div>
				
					<h1>Contact Us</h1>
					
					<p>Please send us your inquiry and we will get back to you as quickly as we can, usually within 24 hours or less.</p>
		
					<form runat="server">
													
					<div id="htmErrors" Visible="false" class="errorsummary" runat="server" />
								
					<vam:ValidationSummary id="valSummary" runat="server" HeaderText="Please Correct These Errors:" HyperLinkToFields="True" AutoUpdate="True"></vam:ValidationSummary>
							
					<table class="forms">
						<tbody>
							<tr>
								<th>
									First Name: <span class="required">(required)</span>
								</th>
								<th>
									Last Name: <span class="required">(required)</span>
								</th>
							</tr>
							<tr>
								<td>
									<asp:TextBox id="txtFirstName" MaxLength="30" Columns="20" runat="server" />
									<vam:RequiredTextValidator id="rfvFirstName" runat="server" ControlIDToEvaluate="txtFirstName" ShowRequiredFieldMarker="True" ErrorMessage="First Name is required." />
								</td>
								<td>
									<asp:TextBox id="txtLastName" MaxLength="30" Columns="20" runat="server" />
									<vam:RequiredTextValidator id="rtvLastName" runat="server" ControlIDToEvaluate="txtLastName" ShowRequiredFieldMarker="True" ErrorMessage="Last Name is required." />
								</td>
							</tr>
							<tr>
								<th>
									E-mail Address: <span class="required">(required)</span>
								</th>
								<th>
									Telephone Number:
								</th>
							</tr>
							<tr>
								<td>
									<asp:TextBox id="txtEmail" MaxLength="100" Columns="25" runat="server" />
									<vam:RequiredTextValidator id="rtvEmail" runat="server" ControlIDToEvaluate="txtEmail" ShowRequiredFieldMarker="True" ErrorMessage="E-mail Address is required." />
									<vam:EmailAddressValidator id="eavEmail" runat="server" ControlIDToEvaluate="txtEmail" ErrorMessage="Invalid E-mail Address." />
								</td>
								<td>
									<vam:FilteredTextBox id="txtTelephone" runat="server" LettersUppercase="false" LettersLowercase="false" Digits="true" Space="true" MaxLength="60" Columns="20" OtherCharacters="+()-" />
								</td>
							</tr>
							<tr>
								<th colspan="2">
									Comments:
								</th>
							</tr>
							<tr>
								<td colspan="2">
									<asp:TextBox id="txtComments" Columns="62" Rows="5" TextMode="MultiLine" runat="server"/>
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<p class="privacystatement">
										We take your privacy very seriously.  Your e-mail address, telephone number, and personal information will <strong>not</strong> be given to any third party, will <strong>not</strong> be added to an e-mail list unless desired and will <strong>not</strong> be used in any way other than to provide you with feedback and answers to your questions.
										</p>
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<vam:Button id="submitButton" CssClass="buttons" Text="Send Contact Form" CausesValidation="True" OnClick="Submit_OnClick" runat="server" />&nbsp;&nbsp;<input type="reset" value="Cancel">
								</td>
							</tr>
						</tbody>
					</table>
					
					</form>
					
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