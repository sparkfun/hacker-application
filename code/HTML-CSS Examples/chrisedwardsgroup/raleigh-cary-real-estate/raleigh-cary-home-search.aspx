<%@ Page Debug="True" Trace="False" EnableViewState="True" Inherits="ChrisEdwardsGroup.Website.HomeSearch" %>

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
	<title>Raleigh, Cary and Apex, North Carolina Real Estate Home Search</title>
	
	<meta name="Description" content="Search for homes and Real Estate in Apex, Cary, Chapel Hill, Clayton, Durham, Fuquay-Varina, Holly Springs, Garner, Knightdale, Morrisville, Raleigh, Research Triangle Park, Wake Forest and Willow Springs, North Carolina.">

	<meta name="Keywords" content="Apex, Cary, Chapel Hill, Clayton, Durham, Fuquay-Varina, Holly Springs, Garner, Knightdale, Morrisville, Raleigh, Research Triangle Park, Wake Forest, Willow Springs, North Carolina, Home Search, Real Estate">

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
							Raleigh, Cary, Apex &amp; Holly Springs North Carolina Home Search
						</h1>
						
						<p class="text">
							Welcome to our home search gateway system, the easiest way to receive the best homes that fit your criteria in <span class="bold">Raleigh, North Carolina</span> and surrounding areas. The gateway is a unique, helpful system that will do the home search work for you!
						</p>
						
						<p class="text">
							Please complete the home search profile below with the most accurate information you can give us. We will set up a custom home search gateway for you based on the detailed criteria you provide in the boxes below to search through over 14,000+ properties in the Triangle MLS system. I will email you whenever new properties hit the market that fit your criteria become available in the region that includes: <span class="bold">Raleigh, Cary, Apex, Morrisville, Holly Springs, Fuquay-Varina, Wake Forest, Garner and Chapel Hill, North Carolina</span>. 
						</p>
						
						<p class="text">
							The gateway is also interactive and allows you to save listings as &quot;Favorites.&quot; We also have access to the &quot;favorites&quot; file, and this will give us an even better idea of what you like. The Home Search Gateway is an easy &amp; efficient way to be educated on what our Raleigh/Cary Real Estate market has to offer.     
						</p>
						
						<p class="note">
							The more information that you provide below, the more accurate your home search will be. Thank you for giving us the opportunity to help you!
						</p>
						
						<!-- start raleigh-cary home search form -->
						<form runat="server">
											
						<div id="htmErrors" Visible="false" class="VAMValSummary" runat="server" />
							
						<vam:ValidationSummary id="valSummary" runat="server" HeaderText="Please Correct the Following Errors:" HyperLinkToFields="True" AutoUpdate="True" />
						
						<table class="forms">
							<tbody>
								<tr>
									<th colspan="2" class="formhead">
										What Type of Home Are You Looking For?
									</th>
								</tr>
								<tr>
									<th>
										Minimum Purchase Price:
									</th>
									<th>
										Maximum Purchase Price:
									</th>
								</tr>
								<tr>
									<td>
										<asp:DropDownList id="ddlMinPrice" CssClass="dropdownlists" runat="server" />
									</td>
									<td>
										<asp:DropDownList id="ddlMaxPrice" CssClass="dropdownlists" runat="server" />
									</td>
								</tr>
								<tr>
									<th colspan="2">
										Area/City:
									</th>
								</tr>
								<tr>
									<td colspan="2">
										<asp:CheckBoxList id="chblAreaCity" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="3" TextAlign="Right" runat="server" />
								</td>
								</tr>
								<tr>
									<th colspan="2">
										Neighborhood/Community:
									</th>
								</tr>
								<tr>
									<td colspan="2">
										<asp:CheckBoxList id="chblNeighborhoodCommunity" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="3" TextAlign="Right" runat="server" />
								</td>
								</tr>
								<tr>
									<th>
										Approximate Lot Size:
									</th>
									<th>
										Approximate Square Footage:
									</th>
								</tr>
								<tr>
									<td>
										<asp:TextBox id="txtAppxLotSize" MaxLength="30" Columns="20" runat="server" />
									</td>
									<td>
										<asp:TextBox id="txtAppxSqFt" MaxLength="20" Columns="15" runat="server" />
									</td>
								</tr>
								<tr>
									<th>
										Minimum Number of Bedrooms:
									</th>
									<th>
										Minimum Number of Bathrooms:
									</th>
								</tr>
								<tr>
									<td>
										<asp:TextBox id="txtMinBedrooms" MaxLength="6" Columns="6" runat="server" />
									</td>
									<td>
										<asp:TextBox id="txtMinBathrooms" MaxLength="6" Columns="6" runat="server" />
									</td>
								</tr>
								<tr>
									<th colspan="2">
										Home Type:
									</th>
								</tr>
								<tr>
									<td colspan="2">
										<asp:CheckBoxList id="chblHomeType" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="2" TextAlign="Right" runat="server">
										<asp:ListItem value="Single Family">
											Single Family
										</asp:ListItem>
										<asp:ListItem value="Multi Family/Investment">
											Multi Family/Investment
										</asp:ListItem>
										<asp:ListItem value="Townhouse">
											Townhouse
										</asp:ListItem>
										<asp:ListItem value="Condo">
											Condo
										</asp:ListItem>
									</asp:CheckBoxList>
								</td>
								</tr>
								<tr>
									<th colspan="2">
										Home Condition:
									</th>
								</tr>
								<tr>
									<td colspan="2">
										<asp:RadioButtonList id="rblHomeCondition" RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Table" TextAlign="Right" runat="server">
											<asp:ListItem Text="Fixer Upper" Value="Fixer Upper" />
											<asp:ListItem Text="Average" Value="Average" />
											<asp:ListItem Text="Excellent" Value="Excellent" />
										</asp:RadioButtonList>
									</td>
								</tr>
								<tr>
									<th colspan="2">
										Reason For Move:
									</th>
								</tr>
								<tr>
									<td colspan="2">
										<asp:RadioButtonList id="rblReasonForMove" RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Table" TextAlign="Right" runat="server">
											<asp:ListItem Text="New Job" Value="New Job" />
											<asp:ListItem Text="Relocating" Value="Relocating" />
											<asp:ListItem Text="Buying First Home" Value="Buying First Home" />
											<asp:ListItem Text="Moving Up" Value="Moving Up" />
											<asp:ListItem Text="Moving Down" Value="Moving Down" />
											<asp:ListItem Text="Retiring" Value="Retiring" />
										</asp:RadioButtonList>
									</td>
								</tr>
								<tr>
									<th colspan="2">
										Move-In Timeframe:
									</th>
								</tr>
								<tr>
									<td colspan="2">
										<asp:RadioButtonList id="rblMoveInTimeframe" RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Table" TextAlign="Right" runat="server">
											<asp:ListItem Text="One Month" Value="One Month" />
											<asp:ListItem Text="Two Months" Value="Two Months" />
											<asp:ListItem Text="Three Months" Value="Three Months" />
											<asp:ListItem Text="Six Months" Value="Six Months" />
											<asp:ListItem Text="One Year" Value="One Year" />
										</asp:RadioButtonList>
									</td>
								</tr>
								<tr>
									<th colspan="2">
										How Long Have You Been Looking:
									</th>
								</tr>
								<tr>
									<td colspan="2">
										<asp:RadioButtonList id="rblHomeLooking" RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Table" TextAlign="Right" runat="server">
											<asp:ListItem Text="Just Started Looking" Value="Just Started Looking" />
											<asp:ListItem Text="One Month" Value="One Month" />
											<asp:ListItem Text="Two Months" Value="Two Months" />
											<asp:ListItem Text="Three Months" Value="Three Months" />
											<asp:ListItem Text="Six Months" Value="Six Months" />
											<asp:ListItem Text="One Year" Value="One Year" />
										</asp:RadioButtonList>
									</td>
								</tr>
								<tr>
									<th colspan="2">
										I Would Like to Schedule: (Check all that apply)
									</th>
								</tr>
								<tr>
									<td colspan="2">
										<asp:CheckBoxList id="chblShedule" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="2" TextAlign="Right" runat="server">
											<asp:ListItem value="N/A">
												N/A
											</asp:ListItem>
											<asp:ListItem value="A Tour of Homes">
												A Tour of Homes
											</asp:ListItem>
											<asp:ListItem value="A Tour of Neighborhoods">
												A Tour of Neighborhoods
											</asp:ListItem>
											<asp:ListItem value="A Tour of Cities">
												A Tour of Cities
											</asp:ListItem>
											<asp:ListItem value="Home Buying Consultation">
												Home Buying Consultation
											</asp:ListItem>
											<asp:ListItem value="Home Selling Consultation">
												Home Selling Consultation
											</asp:ListItem>
										</asp:CheckBoxList>
									</td>
								</tr>
								<tr>
									<th colspan="2">
										When Would You Like To Schedule A Meeting?:
									</th>
								</tr>
								<tr>
									<td colspan="2">
										<asp:RadioButtonList id="rblScheduleMeeting" RepeatColumns="2" RepeatDirection="Horizontal" RepeatLayout="Table" TextAlign="Right" runat="server">
											<asp:ListItem Text="N/A" Value="N/A" />
											<asp:ListItem Text="This Week" Value="This Week" />
											<asp:ListItem Text="As Soon As Possible" Value="As Soon As Possible" />
											<asp:ListItem Text="This Month" Value="This Month" />
											<asp:ListItem Text="At A Later Date" Value="At A Later Date" />
											<asp:ListItem Text="Contact Me To Schedule" Value="Contact Me To Schedule" />
										</asp:RadioButtonList>
									</td>
								</tr>
								<tr>
									<th colspan="2">
										Comments or Other Desired Criteria:
									</th>
								</tr>
								<tr>
									<td colspan="2">
										<asp:TextBox id="txtComments" Columns="45" Rows="5" TextMode="MultiLine" runat="server"/>
								</td>
								</tr>
							</tbody>
						</table>
						
						<!-- start personal information form section -->
						<table class="forms">
							<tbody>
								<tr>
									<th colspan="2" class="formhead">
										Personal Information
									</th>
								</tr>
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
										<asp:TextBox id="txtFirstName" MaxLength="30" Columns="30" runat="server" />
										<VAM:RequiredTextValidator id="rfvFirstName" runat="server" ControlIDToEvaluate="txtFirstName" ShowRequiredFieldMarker="True" ErrorMessage="First Name is required.">
											<ErrorFormatterContainer>
												<vam:TextErrorFormatter></vam:TextErrorFormatter>
											</ErrorFormatterContainer>
										</VAM:RequiredTextValidator>
									</td>
									<td>
										<asp:TextBox id="txtLastName" MaxLength="30" Columns="30" runat="server" />
										<VAM:RequiredTextValidator id="rtvLastName" runat="server" ControlIDToEvaluate="txtLastName" ShowRequiredFieldMarker="True" ErrorMessage="Last Name is required.">
											<ErrorFormatterContainer>
												<vam:TextErrorFormatter></vam:TextErrorFormatter>
											</ErrorFormatterContainer>
										</VAM:RequiredTextValidator>
									</td>
								</tr>
								<tr>
									<th>
										E-Mail Address: <span class="required">(required)</span>
									</th>
									<th>
										Telephone Number: <span class="required">(required)</span>
									</th>
								</tr>
								<tr>
									<td>
										<asp:TextBox id="txtEmail" MaxLength="60" Columns="30" runat="server" />
										<VAM:RequiredTextValidator id="rtvEmail" runat="server" ControlIDToEvaluate="txtEmail" ShowRequiredFieldMarker="True" ErrorMessage="E-mail Address is required." />
										<VAM:EmailAddressValidator id="eavEmail" runat="server" ControlIDToEvaluate="txtEmail" ErrorMessage="Invalid E-mail Address." />
									</td>
									<td>
										<vam:FilteredTextBox id="txtTelephone" runat="server" LettersUppercase="false" LettersLowercase="false" Digits="true" Space="true" MaxLength="60" Columns="30" OtherCharacters="+()-" />
										<vam:RequiredTextValidator id="rtvTelephone" runat="server" ControlIDToEvaluate="txtTelephone" ShowRequiredFieldMarker="True" ErrorMessage="Telephone is required." />
									</td>
								</tr>
								<tr>
									<th>
										Preferred Contact Method: <span class="required">(required)</span>
									</th>
									<th>
										Best Time to Contact:
									</th>
								</tr>
								<tr>
									<td>
										<asp:DropDownList id="ddlContactMethod" cssclass="dropdownlists" runat="server">
											<asp:ListItem value="">
								   	Select Method...
								   </asp:ListItem>
								   <asp:ListItem value="E-mail">
								   	E-mail
								   </asp:ListItem>
											<asp:ListItem value="Telephone">
								   	Telephone
								   </asp:ListItem>
											<asp:ListItem value="E-mail or Telephone">
								   	E-mail or Telephone
								   </asp:ListItem>
										</asp:DropDownList>
										<vam:RequiredTextValidator id="rtvContactMethod" runat="server" ControlIDToEvaluate="ddlContactMethod" ShowRequiredFieldMarker="True" ErrorMessage="Preferred Contact Method is required." />
									</td>
									<td>
										<asp:TextBox id="txtBestTimeContact" MaxLength="60" Columns="30" runat="server" />
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<p class="privacystatement">
											We take your privacy very seriously.  Your e-mail address, telephone number, and personal information will <span class="bold">not</span> be given to any third party, will <span class="bold">not</span> be added to an e-mail list unless desired and will <span class="bold">not</span> be used in any way other than to provide you with up-to-date information about the <span class="bold">Raleigh Real Estate</span> market.
											</p>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<vam:Button id="submitButton" Text="Send Home Search" CausesValidation="True" OnClick="Submit_OnClick" runat="server" />&nbsp;&nbsp;<input type="reset" value="Cancel">
									</td>
								</tr>
							</tbody>
						</table>
						<!-- end personal information form section -->
						
						</form>
						<!-- end raleigh-cary home search form -->
						
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
