<%@ Page EnableViewState="True" Inherits="ChrisEdwardsGroup.Website.AgentReferral" %>

<%@ Register TagPrefix="vam" Namespace="PeterBlum.VAM" Assembly="PeterBlum.VAM" %>
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
	<title>Agent Referral Form</title>
	
	<meta name="Description" content="Referral form for referring Real Estate clients to Chris Edwards in Raleigh, North Carolina.">

	<meta name="Keywords" content="agent,agent referral,realtor,realtor referral,north carolina">

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
					Agent Referral Form
				</h1>
				
				<p class="text">
					Realtors, do you have clients wanting to buy or sell Real Estate in the Raleigh, North Carolina area? Refer your clients using this handy online form!
				</p>
				
				<p class="text">
					In exchange for your referral you will receive the following:
				</p>
				
				<ul class="referral">
					<li>
						Your realtor referral fee will be paid promptly upon completion.
					</li>
					<li>
						Your valued clients will be respected.  They will be cared for from arrival to moving day and beyond.
					</li>
					<li>
						You will be informed throughout every part of the transaction.
					</li>
				</ul>
				
				<form runat="server">
									
				<div id="htmErrors" Visible="false" class="errorsummary" runat="server" />
				
				<vam:ValidationSummary id="valSummary" runat="server" HeaderText="Please Correct the Following Errors:" HyperLinkToFields="True" AutoUpdate="True" />

				<table class="forms">
					<tbody>
						<tr>
							<th colspan="2" class="formhead">
								Client Information
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
								<asp:TextBox id="txtClientFirstName" MaxLength="30" Columns="30" runat="server" />
								<vam:RequiredTextValidator id="rfvClientFirstName" runat="server" ControlIDToEvaluate="txtClientFirstName" ShowRequiredFieldMarker="True" ErrorMessage="First Name is required." />
							</td>
							<td class="homesearch">
								<asp:TextBox id="txtClientLastName" MaxLength="30" Columns="30" runat="server" />
								<vam:RequiredTextValidator id="rfvClientLastName" runat="server" ControlIDToEvaluate="txtClientLastName" ShowRequiredFieldMarker="True" ErrorMessage="Last Name is required." />
							</td>
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
								<asp:TextBox id="txtClientAddress1" MaxLength="50" Columns="30" runat="server" />
							</td>
							<td class="homesearch">
								<asp:TextBox id="txtClientAddress2" MaxLength="50" Columns="30" runat="server" />
							</td>
						</tr>
						<tr>
							<th class="homesearchhead" colspan="2">
								City:
							</th>
						</tr>
						<tr>
							<td class="homesearch" colspan="2">
								<asp:TextBox id="txtClientCity" MaxLength="50" Columns="40" runat="server" />
							</td>
						</tr>
						<tr>
							<th class="homesearchhead">
								State:
							</th>
							<th class="homesearchhead">
								Zip Code:
							</th>
						</tr>
						<tr>
							<td class="homesearch">
								<asp:TextBox id="txtClientState" MaxLength="50" Columns="25" runat="server" />
							</td>
							<td class="homesearch">
								<asp:TextBox id="txtClientZipCode" MaxLength="10" Columns="12" runat="server" />
							</td>
						</tr>
						<tr>
							<th class="homesearchhead">
								E-mail Address:
							</th>
							<th class="homesearchhead">
								Telephone Number: <span class="italic">(required)</span>
							</th>
						</tr>
						<tr>
							<td class="homesearch">
								<asp:TextBox id="txtClientEmail" MaxLength="30" Columns="30" runat="server" />
								<vam:EmailAddressValidator id="revClientEmail" runat="server" ControlIDToEvaluate="txtClientEmail" ErrorMessage="Invalid E-mail Address." />
							</td>
							<td class="homesearch">
								<vam:FilteredTextBox id="txtClientTelephone" runat="server" LettersUppercase="false" LettersLowercase="false" Digits="true" Space="true" MaxLength="60" Columns="30" OtherCharacters="+()-" />
								<vam:RequiredTextValidator id="rtvClientTelephone" runat="server" ControlIDToEvaluate="txtClientTelephone" ShowRequiredFieldMarker="True" ErrorMessage="Telephone is required." />
							</td>
						</tr>
						<tr>
							<th colspan="2" class="homesearchhead">
								Other Client Information:
							</th>
						</tr>
						<tr>
							<td class="homesearch" colspan="2">
								<asp:TextBox id="txtOtherClientInformation" Columns="45" Rows="5" TextMode="MultiLine" runat="server"/>
						</td>
						</tr>
					</tbody>
				</table>
				
				<table class="forms">
					<tbody>
						<tr>
							<th colspan="2" class="formhead">
								What Type of Home Is Your Client Looking For?
							</th>
						</tr>
						<tr>
							<th class="homesearchhead">
								Minimum Purchase Price:
							</th>
							<th class="homesearchhead">
								Maximum Purchase Price:
							</th>
						</tr>
						<tr>
							<td class="homesearch">
								<asp:DropDownList id="ddlMinPrice" CssClass="dropdownlist" runat="server" />
							</td>
							<td class="homesearch">
								<asp:DropDownList id="ddlMaxPrice" CssClass="dropdownlist" runat="server" />
							</td>
						</tr>
						<tr>
							<th colspan="2" class="homesearchhead">
								Area/City:
							</th>
						</tr>
						<tr>
							<td class="homesearch" colspan="2">
								<asp:CheckBoxList id="chblAreaCity" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="3" TextAlign="Right" runat="server" />
							</td>
						</tr>
						<tr>
							<th colspan="2" class="homesearchhead">
								Neighborhood/Community:
							</th>
						</tr>
						<tr>
							<td class="homesearch" colspan="2">
								<asp:CheckBoxList id="chblNeighborhoodCommunity" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="3" TextAlign="Right" runat="server" />
							</td>
						</tr>
						<tr>
							<th class="homesearchhead">
								Approximate Lot Size:
							</th>
							<th class="homesearchhead">
								Approximate Square Footage:
							</th>
						</tr>
						<tr>
							<td class="homesearch">
								<asp:TextBox id="txtAppxLotSize" MaxLength="30" Columns="20" runat="server" />
							</td>
							<td class="homesearch">
								<asp:TextBox id="txtAppxSqFt" MaxLength="20" Columns="15" runat="server" />
							</td>
						</tr>
						<tr>
							<th class="homesearchhead">
								Minimum Number of Bedrooms:
							</th>
							<th class="homesearchhead">
								Minimum Number of Bathrooms:
							</th>
						</tr>
						<tr>
							<td class="homesearch">
								<asp:TextBox id="txtMinBedrooms" MaxLength="6" Columns="6" runat="server" />
							</td>
							<td class="homesearch">
								<asp:TextBox id="txtMinBathrooms" MaxLength="6" Columns="6" runat="server" />
							</td>
						</tr>
						<tr>
							<th colspan="2" class="homesearchhead">
								Other Features and Amenities:
							</th>
						</tr>
						<tr>
							<td class="homesearch" colspan="2">
								<asp:TextBox id="txtOtherFeaturesAmenities" Columns="45" Rows="5" TextMode="MultiLine" runat="server"/>
						</td>
						</tr>
						<tr>
							<th colspan="2" class="homesearchhead">
								Home Condition:
							</th>
						</tr>
						<tr>
							<td class="homesearch" colspan="2">
								<asp:RadioButtonList id="rblHomeCondition" RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Table" TextAlign="Right" runat="server">
									<asp:ListItem Text="Fixer Upper" Value="Fixer Upper" />
									<asp:ListItem Text="Average" Value="Average" Selected="True" />
									<asp:ListItem Text="Excellent" Value="Excellent" />
								</asp:RadioButtonList>
							</td>
						</tr>
						<tr>
							<th colspan="2" class="homesearchhead">
								Reason For Move:
							</th>
						</tr>
						<tr>
							<td class="homesearch" colspan="2">
								<asp:RadioButtonList id="rblReasonForMove" RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Table" TextAlign="Right" runat="server">
									<asp:ListItem Text="Relocating" Value="Relocating" Selected="True" />
									<asp:ListItem Text="Buying First Home" Value="Buying First Home" />
									<asp:ListItem Text="Moving Up" Value="Moving Up" />
									<asp:ListItem Text="Moving Down" Value="Moving Down" />
									<asp:ListItem Text="Retiring" Value="Retiring" />
								</asp:RadioButtonList>
							</td>
						</tr>
						<tr>
							<th colspan="2" class="homesearchhead">
								Move-In Timeframe:
							</th>
						</tr>
						<tr>
							<td class="homesearch" colspan="2">
								<asp:RadioButtonList id="rblMoveInTimeframe" RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Table" TextAlign="Right" runat="server">
									<asp:ListItem Text="One Month" Value="One Month" />
									<asp:ListItem Text="Two Months" Value="Two Months" Selected="True" />
									<asp:ListItem Text="Three Months" Value="Three Months" />
									<asp:ListItem Text="Six Months" Value="Six Months" />
									<asp:ListItem Text="One Year" Value="One Year" />
								</asp:RadioButtonList>
							</td>
						</tr>
						<tr>
							<th colspan="2" class="homesearchhead">
								How Long Have You Been Looking:
							</th>
						</tr>
						<tr>
							<td class="homesearch" colspan="2">
								<asp:RadioButtonList id="rblHomeLooking" RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Table" TextAlign="Right" runat="server">
									<asp:ListItem Text="Just Started Looking" Value="Just Started Looking" />
									<asp:ListItem Text="One Month" Value="One Month" Selected="True" />
									<asp:ListItem Text="Two Months" Value="Two Months" />
									<asp:ListItem Text="Three Months" Value="Three Months" />
									<asp:ListItem Text="Six Months" Value="Six Months" />
									<asp:ListItem Text="One Year" Value="One Year" />
								</asp:RadioButtonList>
							</td>
						</tr>
					</tbody>
				</table>
				
				<table class="forms">
					<tbody>
						<tr>
							<th colspan="2" class="formhead">
								Your Realtor Information
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
								<asp:TextBox id="txtRealtorFirstName" MaxLength="30" Columns="30" runat="server" />
								<vam:RequiredTextValidator id="rtvRealtorFirstName" runat="server" ControlIDToEvaluate="txtRealtorFirstName" ShowRequiredFieldMarker="True" ErrorMessage="First Name is required." />
							</td>
							<td class="homesearch">
								<asp:TextBox id="txtRealtorLastName" MaxLength="30" Columns="30" runat="server" />
								<vam:RequiredTextValidator id="rtvRealtorLastName" runat="server" ControlIDToEvaluate="txtRealtorLastName" ShowRequiredFieldMarker="True" ErrorMessage="Last Name is required." />
							</td>
						</tr>
						<tr>
							<th class="homesearchhead" colspan="2">
								Company Name:
							</th>
						</tr>
						<tr>
							<td class="homesearch" colspan="2">
								<asp:TextBox id="txtRealtorCompanyName" MaxLength="60" Columns="50" runat="server" />
							</td>
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
								<asp:TextBox id="txtRealtorAddress1" MaxLength="50" Columns="30" runat="server" />
							</td>
							<td class="homesearch">
								<asp:TextBox id="txtRealtorAddress2" MaxLength="50" Columns="30" runat="server" />
							</td>
						</tr>
						<tr>
							<th class="homesearchhead" colspan="2">
								City:
							</th>
						</tr>
						<tr>
							<td class="homesearch" colspan="2">
								<asp:TextBox id="txtRealtorCity" MaxLength="50" Columns="40" runat="server" />
							</td>
						</tr>
						<tr>
							<th class="homesearchhead">
								State:
							</th>
							<th class="homesearchhead">
								Zip Code:
							</th>
						</tr>
						<tr>
							<td class="homesearch">
								<asp:TextBox id="txtRealtorState" MaxLength="50" Columns="25" runat="server" />
							</td>
							<td class="homesearch">
								<asp:TextBox id="txtRealtorZipCode" MaxLength="10" Columns="12" runat="server" />
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
								<asp:TextBox id="txtRealtorEmail" MaxLength="30" Columns="30" runat="server" />
								<vam:RequiredTextValidator id="rtvRealtorEmail" runat="server" ControlIDToEvaluate="txtRealtorEmail" ShowRequiredFieldMarker="True" ErrorMessage="E-mail Address is required." />
								<vam:EmailAddressValidator id="eavRealtorEmail" runat="server" ControlIDToEvaluate="txtRealtorEmail" ErrorMessage="Invalid E-mail Address." />
							</td>
							<td class="homesearch">
								<vam:FilteredTextBox id="txtRealtorTelephone" runat="server" LettersUppercase="false" LettersLowercase="false" Digits="true" Space="true" MaxLength="60" Columns="30" OtherCharacters="+()-" />
								<vam:RequiredTextValidator id="rtvRealtorTelephone" runat="server" ControlIDToEvaluate="txtRealtorTelephone" ShowRequiredFieldMarker="True" ErrorMessage="Telephone is required." />
							</td>
						</tr>
						<tr>
							<th class="homesearchhead" colspan="2">
								Requested Referral Fee:
							</th>
						</tr>
						<tr>
							<td class="homesearch" colspan="2">
								<asp:DropDownList id="ddlRequestedFee" runat="server">
								   <asp:ListItem Value="20%" selected="true">20%</asp:ListItem>
								   <asp:ListItem Value="25%">25%</asp:ListItem>
								   <asp:ListItem Value="Other">Other</asp:ListItem>
								</asp:DropDownList>
							</td>
						</tr>
						<tr>
							<th class="homesearchhead" colspan="2">
								Other Comments:
							</th>
						</tr>
						<tr>
							<td class="homesearch" colspan="2">
								<asp:TextBox id="txtOtherComments" Columns="45" Rows="5" TextMode="MultiLine" runat="server"/>
							</td>
						</tr>
						<tr>
							<td class="homesearch" colspan="2">
								We take your privacy and your client's privacy very seriously.  E-mail addresses, telephone numbers, and personal information will <span class="bold">not</span> be given to any third party, will <span class="bold">not</span> be added to an e-mail list unless desired and will <span class="bold">not</span> be used in any way other than to provide service to your client in the North Carolina Real Estate market.
							</td>
						</tr>
						<tr>
							<td class="homesearch" colspan="2">
								<vam:Button id="submitButton" Text="Send Client Referral" CausesValidation="True" OnClick="Submit_OnClick" runat="server" />&nbsp;&nbsp;<input type="reset" value="Cancel">
							</td>
						</tr>
					</tbody>
				</table>
				
				</form>
						
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
