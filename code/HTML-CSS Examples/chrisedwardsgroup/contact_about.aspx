<%@ Page Debug="False" Trace="False" EnableViewState="True" Inherits="ChrisEdwards.ContactAboutListing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
	"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
	<title>Contact Chris Edwards About Listing</title>

	<link rel="shortcut icon" type="image/ico" href="/EdwardsIcon.ico" />
	<link rel="stylesheet" href="/includes/ce.css" type="text/css">
	<link rel="stylesheet" href="/includes/forms.css" type="text/css">
	<script type="text/javascript">
    
      var _gaq = _gaq || [];
      _gaq.push(['_setAccount', 'UA-2016415-2']);
      _gaq.push(['_trackPageview']);
    
      (function() {
        var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
        ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
      })();
    
    </script>
</head>

<body>

<form runat="server">

	<table class="emailfriend">
		<tbody>
			<tr>
				<th colspan="2" class="formheading">
					Contact About Listing MLS#: <asp:Literal id="ltlTitleMLS" runat="server" />
				</th>
			</tr>
			<tr>
				<td class="emailafriendcell">
					First Name: *
				</td>
				<td class="emailafriendcell">
					<asp:TextBox id="txtFirstName" MaxLength="50" Columns="35" runat="server" />
					<asp:RequiredFieldValidator id="rfvFirstName" ControlToValidate="txtFirstName" ErrorMessage="You must fill in the First Name field." Text="*" runat="server" />
				</td>
			</tr>
			<tr>
				<td class="emailafriendcell">
					Last Name: *
				</td>
				<td class="emailafriendcell">
					<asp:TextBox id="txtLastName" MaxLength="50" Columns="35" runat="server" />
					<asp:RequiredFieldValidator id="rfvLastName" ControlToValidate="txtLastName" ErrorMessage="You must fill in the Last Name field." Text="*" runat="server" />
				</td>
			</tr>
			<tr>
				<td class="emailafriendcell">
					Telephone: *
				</td>
				<td class="emailafriendcell">
					(<asp:TextBox id="txtTelephone1" Columns="1" MaxLength="3" runat="server" />) -
					<asp:TextBox id="txtTelephone2" Columns="1" MaxLength="3" runat="server" /> -
					<asp:TextBox id="txtTelephone3" Columns="2" MaxLength="4" runat="server" />
					<asp:RequiredFieldValidator id="rfvTelephone1" ControlToValidate="txtTelephone1" ErrorMessage="You must fill in a three digit area code." Text="*" runat="server" />
					<asp:RequiredFieldValidator id="rfvTelephone2" ControlToValidate="txtTelephone2" ErrorMessage="You must fill in a three digit telephone number." Text="*" runat="server" />
					<asp:RequiredFieldValidator id="rfvTelephone3" ControlToValidate="txtTelephone3" ErrorMessage="You must fill in a four digit telephone number." Text="*" runat="server" />
					<asp:RegularExpressionValidator id="revTelephone1" ControlToValidate="txtTelephone1" ValidationExpression="\d{3}" ErrorMessage="You must fill in a valid telephone number in the Telephone Number field." Text="*" runat="server" />
					<asp:RegularExpressionValidator id="revTelephone2" ControlToValidate="txtTelephone2" ValidationExpression="\d{3}" ErrorMessage="You must fill in a valid telephone number in the Telephone Number field." Text="*" runat="server" />
					<asp:RegularExpressionValidator id="revTelephone3" ControlToValidate="txtTelephone3" ValidationExpression="\d{4}" ErrorMessage="You must fill in a valid telephone number in the Telephone Number field." Text="*" runat="server" />
				</td>
			</tr>
			<tr>
				<td class="emailafriendcell">
					E-mail Address: *
				</td>
				<td class="emailafriendcell">
					<asp:TextBox id="txtEmail" MaxLength="50" Columns="35" runat="server" />
					<asp:RequiredFieldValidator id="rfvEmail" ControlToValidate="txtEmail" ErrorMessage="You must fill-in the E-Mail Address field." Text="*" runat="server" />
					<asp:RegularExpressionValidator id="revEmail" ControlToValidate="txtEmail" ValidationExpression="[\w\x2E\x2D]{2,}\x40{1}[\w\x2E\x2D]{2,}\x2E{1}[\w\x2E\x2D]{2,}" ErrorMessage="You must fill in a valid e-mail address in the E-mail Address field." Text="*" runat="server" />
				</td>
			</tr>
			<tr>
				<td class="emailafriendcell" colspan="2">
					Comments:
				</td>
			</tr>
			<tr>
				<td class="emailafriendcell" colspan="2">
					<asp:TextBox id="txtCommentsField" Columns="35" Rows="5" TextMode="Multiline" CssClass="tahoma11px" runat="server" />
				</td>
			</tr>
		</tbody>
	</table>
				
<table class="submitbuttons">
	<tbody>
		<tr>
			<td>
				<asp:Button id="submitButton" Text="Send Contact" CssClass="buttons" CausesValidation="True" OnClick="EmailContact_Click" runat="server" />&nbsp;&nbsp;<input type="reset" class="buttons" value="Cancel">
			</td>
		</tr>
	</tbody>
</table>
				
<asp:ValidationSummary id="vsmSummary" DisplayMode="BulletList" EnableClientScript="True" ShowSummary="False" ShowMessageBox="True" HeaderText="Please correct the following errors on the page:" CssClass="errorsummary" runat="server" />
				
<div id="hgcStatus" Visible="false" class="statusmessage" runat="server" />

</form>

<p class="closewindow"><a href="" onClick="window.close();">Close Window</a></p>

</body>
</html>
