<%@ Page Debug="False" Trace="False" EnableViewState="True" Inherits="ChrisEdwardsGroup.Website.HowMuchSell" %>

<%@ Register Tagprefix="Top" Tagname="FeaturedProperty" Src="/includes/featured.ascx" %>
<%@ Register Tagprefix="Top" Tagname="TopLinks" Src="/includes/toplinks.ascx" %>
<%@ Register Tagprefix="Top" Tagname="LeftLinks" Src="/includes/leftlinks.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Address" Src="/includes/address.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Neighborhoods" Src="/includes/neighborhoods.ascx" %>
<%@ Register Tagprefix="Middle" Tagname="Logos" Src="/includes/logos.ascx" %>
<%@ Register Tagprefix="Bottom" Tagname="Footer" Src="/includes/footer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
	"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
	<title>How Much Did the House On Our Street in Raleigh, North Carolina Sell For?</title>
	
	<meta name="Description" content="Find out how much the house on your street in Raleigh, North Carolina sold for.">

	<meta name="Keywords" content="Raleigh Homes, Raleigh Home Prices, Raleigh Real Estate Market">

	<meta name="Robots" content="all">

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

<!-- start header and toplinks -->
<Top:TopLinks id="ctlTopLinks" runat="server" />
<!-- end header and toplinks -->
	
	<tr>
		
		<!-- start left link column -->
		<Top:LeftLinks id="ctlLeftLinks" runat="server" />
		<!-- end left link column -->
		
		<!-- start right column -->
		<td id="right">
			
			<!-- start flash & featured listing -->
				<table border="0">
					<tr>
						
						<!-- start flash or photo -->
						<td><asp:AdRotator id="adrArray1" AdvertisementFile="/includes/rotate1.xml" height="210" width="290" BorderWidth="0" runat="server"/><img src="/images/right-pix/rt-welcome-triangle.jpg" alt="Contact Chris Edwards for Real Estate in the Triangle Area of Raleigh, Cary and Apex North Carolina" width="290" height="210" border="0"></td>
						<!-- end flash or photo -->
						
						<!-- featured listing -->
						<td>
							
							<Top:FeaturedProperty id="featProperty" runat="server" />
						
						</td>
						<!-- end listing -->
						
					</tr>
				</table>
				
			<!-- end flash & featured listing -->
			
			<!-- start text -->
			
				<h1 class="body">
					A House Just Sold On Our Street in the Raleigh Area.<br /><em>How Much Did They Get For It?</em>
				</h1>
					
					<p>
						How much did the house on our street in Raleigh, North Carolina sell for? Submit the address and any other details you have and we will find out! 
					</p>
					
					<form runat="server">
										
						<div id="htmErrors" Visible="false" class="errorsummary" runat="server" />
							
						<asp:ValidationSummary id="vsmSummary" DisplayMode="BulletList" EnableClientScript="True" ShowSummary="True" ShowMessageBox="True" HeaderText="Please correct the following errors on the page:" CssClass="errorsummary" runat="server" />
					
					<table class="forms">
						<tbody>
							<tr>
								<th colspan="2" class="formhead">
									Home Information
								</th>
							</tr>
							<tr>
								<th class="homesearchhead">
									Address 1:
								</th>
								<th class="homesearchhead">
									Address 2:
								</th>
							</tr>
							<tr>
								<td class="homesearch">
									<asp:TextBox id="txtAddress1" MaxLength="50" Columns="30" runat="server" />
								</td>
								<td class="homesearch">
									<asp:TextBox id="txtAddress2" MaxLength="50" Columns="30" runat="server" />
								</td>
							</tr>
							<tr>
								<th class="homesearchhead">
									City:
								</th>
								<th class="homesearchhead">
									Zip Code:
								</th>
							</tr>
							<tr>
								<td class="homesearch">
									<asp:TextBox id="txtCity" MaxLength="50" Columns="30" runat="server" />
								</td>
								<td class="homesearch">
									<asp:TextBox id="txtZipCode" MaxLength="10" Columns="12" runat="server" />
								</td>
							</tr>
							<tr>
								<th colspan="2" class="homesearchhead">
									Other Information:
								</th>
							</tr>
							<tr>
								<td class="homesearch" colspan="2">
									<asp:TextBox id="txtOtherInformation" Columns="45" Rows="5" TextMode="MultiLine" runat="server"/>
							</td>
							</tr>
						</tbody>
					</table>
							
					<table class="forms">
						<tbody>
							<tr>
								<th colspan="2" class="formhead">
									Personal Information
								</th>
							</tr>
							<tr>
								<th class="homesearchhead">
									First Name: <span class="italic">(required)</span>
								</th>
								<th class="homesearchhead">
									Last Name: <span class="italic">(required)</span>
								</th>
							</tr>
							<tr>
								<td class="homesearch">
									<asp:TextBox id="txtFirstName" MaxLength="30" Columns="30" runat="server" />
									<asp:RequiredFieldValidator id="rfvFirstName" ControlToValidate="txtFirstName" ErrorMessage="You must fill in the First Name field." Text="*" runat="server" />
								</td>
								<td class="homesearch">
									<asp:TextBox id="txtLastName" MaxLength="30" Columns="30" runat="server" />
									<asp:RequiredFieldValidator id="rfvLastName" ControlToValidate="txtLastName" ErrorMessage="You must fill in the Last Name field." Text="*" runat="server" />
								</td>
							</tr>
							<tr>
								<th class="homesearchhead">
									E-mail Address: <span class="italic">(required)</span>
								</th>
								<th class="homesearchhead">
									Telephone Number: <span class="italic">(required)</span>
								</th>
							</tr>
							<tr>
								<td class="homesearch">
									<asp:TextBox id="txtEmail" MaxLength="30" Columns="30" runat="server" />
									<asp:RequiredFieldValidator id="rfvEmail" ControlToValidate="txtEmail" ErrorMessage="You must fill in the E-mail field." Text="*" runat="server" />
									<asp:RegularExpressionValidator id="revEmail" ControlToValidate="txtEmail" ValidationExpression="[\w\x2E\x2D]{2,}\x40{1}[\w\x2E\x2D]{2,}\x2E{1}[\w\x2E\x2D]{2,}" ErrorMessage="You must fill in a valid e-mail address in the E-mail Address field." Text="*" runat="server" />
								</td>
								<td class="homesearch">
									( <asp:TextBox id="txtPhoneNumber1" MaxLength="3" Columns="3" runat="server" /> )
											<asp:TextBox id="txtPhoneNumber2" MaxLength="3" Columns="3" runat="server" /> -
											<asp:TextBox id="txtPhoneNumber3" MaxLength="4" Columns="4" runat="server" />
											<asp:RequiredFieldValidator id="rfvPhoneNumber1" ControlToValidate="txtPhoneNumber1" ErrorMessage="You must fill in a three digit area code." Text="*" runat="server" />
											<asp:RequiredFieldValidator id="rfvPhoneNumber2" ControlToValidate="txtPhoneNumber2" ErrorMessage="You must fill in a three digit telephone number." Text="*" runat="server" />
											<asp:RequiredFieldValidator id="rfvPhoneNumber3" ControlToValidate="txtPhoneNumber3" ErrorMessage="You must fill in a four digit telephone number." Text="*" runat="server" />
											<asp:RegularExpressionValidator id="revPhoneNumber1" ControlToValidate="txtPhoneNumber1" ValidationExpression="\d{3}" ErrorMessage="You must fill in a valid telephone number." Text="*" runat="server" />
											<asp:RegularExpressionValidator id="revPhoneNumber2" ControlToValidate="txtPhoneNumber2" ValidationExpression="\d{3}" ErrorMessage="You must fill in a valid telephone number." Text="*" runat="server" />
											<asp:RegularExpressionValidator id="revPhoneNumber3" ControlToValidate="txtPhoneNumber3" ValidationExpression="\d{4}" ErrorMessage="You must fill in a valid telephone number." Text="*" runat="server" />
								</td>
							</tr>
							<tr>
								<td class="homesearch" colspan="2">
									We take your privacy very seriously.  Your e-mail address, telephone number, and personal information will <span class="bold">not</span> be given to any third party, will <span class="bold">not</span> be added to an e-mail list unless desired and will <span class="bold">not</span> be used in any way other than to provide you with up-to-date information about the Raleigh real estate market.
								</td>
							</tr>
							<tr>
								<td class="homesearch" colspan="2">
									<asp:Button id="submitButton" Text="Send Home Information" CausesValidation="True" OnClick="Submit_OnClick" runat="server" />&nbsp;&nbsp;<input type="reset" value="Cancel">
								</td>
							</tr>
						</tbody>
					</table>
					
					</form>
				</h1>
								
				
				
				<!-- end text -->
			
				<!-- start address -->
				<Middle:Address id="ctlAddress" runat="server" />
				<!-- end address -->
				
				<!-- start neighborhoods -->
				<Middle:Neighborhoods id="ctlNeighborhoods" runat="server" />
				<!-- end neighborhoods -->
				
		</td>
		<!-- end right column -->
	
	</tr>
</table>

<!-- start logos -->
<Middle:Logos id="ctlLogos" runat="server" />
<!-- end logos -->

<!-- start footer -->
<Bottom:Footer id="ctlFooter" runat="server" />
<!-- end footer -->

</body>
</html>
