<%@ Page Debug="False" EnableViewState="True" Inherits="ChrisEdwardsGroup.Website.EmailAFriend" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN"
	"http://www.w3.org/TR/html4/loose.dtd">

<html>
<head>
	<title>Email to a Friend</title>
	
	<link rel="stylesheet" href="/includes/listings.css" type="text/css">
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
				
		<table class="popupform">
			<tbody>
				<tr>
					<th colspan="2" class="formheading">
						Email to a Friend
					</th>
				</tr>
				<tr>
					<td>
						To: *
					</td>
					<td>
						<asp:textbox id="txtToField" maxlength="60" columns="35" runat="server" />
						<asp:requiredfieldvalidator id="rfvToField" controltovalidate="txtToField" errormessage="You must fill-in the To: field." text="*" runat="server" />
						<asp:regularexpressionvalidator id="revToField" controltovalidate="txtToField" validationexpression="[\w\x2E\x2D]{2,}\x40{1}[\w\x2E\x2D]{2,}\x2E{1}[\w\x2E\x2D]{2,}" errormessage="You must fill in a valid e-mail address in the To: field." text="*" runat="server" />
					</td>
				</tr>
				<tr>
					<td>
						CC:
					</td>
					<td>
						<asp:textbox id="txtCCField" maxlength="60" columns="35" runat="server" />
						<asp:regularexpressionvalidator id="revCCField" controltovalidate="txtCCField" validationexpression="[\w\x2E\x2D]{2,}\x40{1}[\w\x2E\x2D]{2,}\x2E{1}[\w\x2E\x2D]{2,}" errormessage="You must fill in a valid e-mail address in the CC: field." text="*" runat="server" />
					</td>
				</tr>
				<tr>
					<td>
						From: *
					</td>
					<td>
						<asp:textbox id="txtFromField" maxlength="60" columns="35" runat="server" />
						<asp:requiredfieldvalidator id="rfvFromField" controltovalidate="txtFromField" errormessage="You must fill-in the From: field." text="*" runat="server" />
						<asp:regularexpressionvalidator id="revFromField" controltovalidate="txtFromField" validationexpression="[\w\x2E\x2D]{2,}\x40{1}[\w\x2E\x2D]{2,}\x2E{1}[\w\x2E\x2D]{2,}" errormessage="You must fill in a valid e-mail address in the From: field." text="*" runat="server" />
					</td>
				</tr>
				<tr>
					<td>
						Subject:
					</td>
					<td>
						<asp:textbox id="txtSubjectField" maxlength="60" columns="45" runat="server" />
						<asp:requiredfieldvalidator id="rfvSubjectField" controltovalidate="txtSubjectField" errormessage="You must fill-in the Subject: field." text="*" runat="server" />
					</td>
				</tr>
				<tr>
					<td colspan="2">
						Comments:
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<asp:textbox id="txtCommentsField" columns="35" rows="4" textmode="Multiline" cssclass="tahoma11px" runat="server" />
					</td>
				</tr>
			</tbody>
		</table>
		
		<table>
			<tbody>
				<tr>
					<td>
						<asp:Button id="submitButton" Text="Send to Friend" CssClass="buttons" CausesValidation="True" OnClick="EmailContact_Click" runat="server" />&nbsp;&nbsp;<input type="reset" class="buttons" value="Cancel">
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
