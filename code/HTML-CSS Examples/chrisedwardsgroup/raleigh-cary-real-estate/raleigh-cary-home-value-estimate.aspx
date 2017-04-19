<%@ Page Debug="False" Trace="False" EnableViewState="True" Inherits="ChrisEdwardsGroup.Website.HomeValueEstimate" %>

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
	<title>Free Home Value Estimate - Chris Edwards Group Raleigh-Cary Real Estate</title>
	
	<meta name="Description" content="Receive a free home value estimate.  Find out how much your Raleigh, Cary, Apex, North Carolina home is worth.">

	<meta name="Keywords" content="Home Value Estimate, Raleigh, Cary, Apex, North Carolina, Home Worth, Home Values">

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
					Free Home Value Estimate
				</h1>
				
				<p>
					If you would like an <em>accurate</em> and <em>comprehensive</em> personal analysis of your home's value, fill out the form below and submit it now for your free Detailed Home Value Analysis.
				</p>
					
				<p>
					How much are homes in the <span class="bold">Raleigh-Cary, North Carolina</span> area home selling for? Please provide us with some detailed information about your home below and we will tell you.
				</p>
					
				<p>
					Note: Before relying on this evaluation for marketing or other purposes, a physical inspection of the property should be arranged to confirm the accuracy of the results.  The more information that you provide, the more accurate your home evaluation will be.
				</p>
					
					<form runat="server">
										
						<div id="htmErrors" Visible="false" class="errorsummary" runat="server" />
							
						<asp:ValidationSummary id="vsmSummary" DisplayMode="BulletList" EnableClientScript="True" ShowSummary="True" ShowMessageBox="True" HeaderText="Please correct the following errors on the page:" CssClass="errorsummary" runat="server" />
					
					<table class="forms">
						<tbody>
							<tr>
								<th colspan="2" class="formhead">
									Tell Us More About Your Raleigh Area Home
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
								<th class="homesearchhead">
									Lot Size:
								</th>
								<th class="homesearchhead">
									Square Footage:
								</th>
							</tr>
							<tr>
								<td class="homesearch">
									<asp:TextBox id="txtLotSize" MaxLength="50" Columns="20" runat="server" />
								</td>
								<td class="homesearch">
									<asp:TextBox id="txtSqFt" MaxLength="50" Columns="20" runat="server" />
								</td>
							</tr>
							<tr>
								<th class="homesearchhead">
									Number of Bedrooms:
								</th>
								<th class="homesearchhead">
									Number of Bathrooms:
								</th>
							</tr>
							<tr>
								<td class="homesearch">
									<asp:TextBox id="txtBedrooms" MaxLength="6" Columns="6" runat="server" />
								</td>
								<td class="homesearch">
									<asp:TextBox id="txtBathrooms" MaxLength="6" Columns="6" runat="server" />
								</td>
							</tr>
							<tr>
								<th class="homesearchhead">
									Year Built:
								</th>
								<th class="homesearchhead">
									Neighborhood:
								</th>
							</tr>
							<tr>
								<td class="homesearch">
									<asp:TextBox id="txtYearBuilt" MaxLength="6" Columns="6" runat="server" />
								</td>
								<td class="homesearch">
									<asp:TextBox id="txtNeighborhood" MaxLength="30" Columns="30" runat="server" />
								</td>
							</tr>
							<tr>
								<th colspan="2" class="homesearchhead">
									Home Type:
								</th>
							</tr>
							<tr>
								<td class="homesearch" colspan="2">
									<asp:RadioButtonList id="rblHomeType" RepeatColumns="2" RepeatDirection="Horizontal" RepeatLayout="Table" TextAlign="Right" runat="server">
									<asp:ListItem value="Single Family" Selected="True">
										Single Family
									</asp:ListItem>
									<asp:ListItem value="Multi Family">
										Multi Family
									</asp:ListItem>
									<asp:ListItem value="Townhouse">
										Townhouse
									</asp:ListItem>
									<asp:ListItem value="Condo">
										Condo
									</asp:ListItem>
								</asp:RadioButtonList>
							</td>
							</tr>
							<tr>
								<th colspan="2" class="homesearchhead">
									Home Style:
								</th>
							</tr>
							<tr>
								<td class="homesearch" colspan="2">
									<asp:RadioButtonList id="rblHomeStyle" RepeatColumns="2" RepeatDirection="Horizontal" RepeatLayout="Table" TextAlign="Right" runat="server">
									<asp:ListItem value="Ranch" Selected="True">
										Ranch
									</asp:ListItem>
									<asp:ListItem value="Bungalow">
										Bungalow
									</asp:ListItem>
									<asp:ListItem value="Two Story">
										Two Story
									</asp:ListItem>
									<asp:ListItem value="Two and a Half Story">
										Two a Half Story
									</asp:ListItem>
									<asp:ListItem value="One and a Half Story">
										One and a Half Story
									</asp:ListItem>
									<asp:ListItem value="Bilevel">
										Bilevel
									</asp:ListItem>
									<asp:ListItem value="Split Level - 3 Levels">
										Split Level - 3 Levels
									</asp:ListItem>
									<asp:ListItem value="Split Level - 4 or More Levels">
										Split Level - 4 or More Levels
									</asp:ListItem>
									<asp:ListItem value="Other">
										Other
									</asp:ListItem>
								</asp:RadioButtonList>
							</td>
							</tr>
							<tr>
								<th colspan="2" class="homesearchhead">
									Basement Type:
								</th>
							</tr>
							<tr>
								<td class="homesearch" colspan="2">
									<asp:RadioButtonList id="rblBasementType" RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Table" TextAlign="Right" runat="server">
									<asp:ListItem value="Full" Selected="True">
										Full
									</asp:ListItem>
									<asp:ListItem value="Part">
										Part
									</asp:ListItem>
									<asp:ListItem value="Crawl">
										Crawl
									</asp:ListItem>
									<asp:ListItem value="Slab">
										Slab
									</asp:ListItem>
									<asp:ListItem value="None">
										None
									</asp:ListItem>
								</asp:RadioButtonList>
							</td>
							</tr>
							<tr>
								<th colspan="2" class="homesearchhead">
									Parking:
								</th>
							</tr>
							<tr>
								<td class="homesearch" colspan="2">
									<asp:RadioButtonList id="rblParking" RepeatColumns="2" RepeatDirection="Horizontal" RepeatLayout="Table" TextAlign="Right" runat="server">
									<asp:ListItem value="Driveway" Selected="True">
										Driveway
									</asp:ListItem>
									<asp:ListItem value="Parking Pad">
										Parking Pad
									</asp:ListItem>
									<asp:ListItem value="Single Detached Garage">
										Single Detached Garage
									</asp:ListItem>
									<asp:ListItem value="Double Detached Garage">
										Double Detached Garage
									</asp:ListItem>
									<asp:ListItem value="Single Attached Garage">
										Single Attached Garage
									</asp:ListItem>
									<asp:ListItem value="Double Attached Garage">
										Double Attached Garage
									</asp:ListItem>
									<asp:ListItem value="Multiple Car Garage">
										Multiple Car Garage
									</asp:ListItem>
									<asp:ListItem value="Underground">
										Underground
									</asp:ListItem>
								</asp:RadioButtonList>
							</td>
							</tr>
							<tr>
								<th colspan="2" class="homesearchhead">
									Amenities:
								</th>
							</tr>
							<tr>
								<td class="homesearch" colspan="2">
									<asp:CheckBoxList id="chblAmenities" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="3" TextAlign="Right" runat="server">
									<asp:ListItem value="Dining Room">
										Dining Room
									</asp:ListItem>
									<asp:ListItem value="Family Room">
										Family Room
									</asp:ListItem>
									<asp:ListItem value="Rec Room">
										Rec Room
									</asp:ListItem>
									<asp:ListItem value="Deck">
										Deck
									</asp:ListItem>
									<asp:ListItem value="Central Air">
										Central Air
									</asp:ListItem>
									<asp:ListItem value="New Siding">
										New Siding
									</asp:ListItem>
									<asp:ListItem value="Freshly Painted">
										Freshly Painted
									</asp:ListItem>
									<asp:ListItem value="New Flooring">
										New Flooring
									</asp:ListItem>
									<asp:ListItem value="New Roof">
										New Roof
									</asp:ListItem>
									<asp:ListItem value="New Kitchen Cabinets">
										New Kitchen Cabinets
									</asp:ListItem>
									<asp:ListItem value="Upgraded Bathroom">
										Upgraded Bathroom
									</asp:ListItem>
									<asp:ListItem value="New Windows">
										New Windows
									</asp:ListItem>
									<asp:ListItem value="Whirlpool Bath">
										Whirlpool Bath
									</asp:ListItem>
									<asp:ListItem value="Fenced Yard">
										Fenced Yard
									</asp:ListItem>
									<asp:ListItem value="Fireplace">
										Fireplace
									</asp:ListItem>
								</asp:CheckBoxList>
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
										<asp:ListItem Text="Fixer Upper" Value="Fixer Upper" Selected="True" />
										<asp:ListItem Text="Average" Value="Average" />
										<asp:ListItem Text="Excellent" Value="Excellent" />
									</asp:RadioButtonList>
								</td>
							</tr>
							<tr>
								<th colspan="2" class="homesearchhead">
									Reason For Home Evaluation:
								</th>
							</tr>
							<tr>
								<td class="homesearch" colspan="2">
									<asp:RadioButtonList id="rblReasonForEvaluation" RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Table" TextAlign="Right" runat="server">
										<asp:ListItem Text="Transfer" Value="Transfer" Selected="True" />
										<asp:ListItem Text="Moving Up" Value="Moving Up" />
										<asp:ListItem Text="Moving Down" Value="Moving Down" />
										<asp:ListItem Text="Retiring" Value="Retiring" />
										<asp:ListItem Text="Other Reason" Value="Other Reason" />
									</asp:RadioButtonList>
								</td>
							</tr>
							<tr>
								<th colspan="2" class="homesearchhead">
									How Soon Are You Moving?:
								</th>
							</tr>
							<tr>
								<td class="homesearch" colspan="2">
									<asp:RadioButtonList id="rblHowSoonMoving" RepeatColumns="2" RepeatDirection="Horizontal" RepeatLayout="Table" TextAlign="Right" runat="server">
										<asp:ListItem Text="Within Three Months" Value="Within Three Months" Selected="True" />
										<asp:ListItem Text="Within Six Months" Value="Within Six Months" />
										<asp:ListItem Text="Within One Year" Value="Within One Year" />
										<asp:ListItem Text="Within the Next Five Years" Value="Within the Next Five Years" />
									</asp:RadioButtonList>
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
									<vam:RequiredTextValidator id="rfvFirstName" runat="server" ControlIDToEvaluate="txtFirstName" ShowRequiredFieldMarker="True" ErrorMessage="First Name is required." />
								</td>
								<td class="homesearch">
									<asp:TextBox id="txtLastName" MaxLength="30" Columns="30" runat="server" />
									<VAM:RequiredTextValidator id="rtvLastName" runat="server" ControlIDToEvaluate="txtLastName" ShowRequiredFieldMarker="True" ErrorMessage="Last Name is required." />
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
									<asp:TextBox id="txtEmail" MaxLength="60" Columns="30" runat="server" />
									<vam:RequiredTextValidator id="rtvEmail" runat="server" ControlIDToEvaluate="txtEmail" ShowRequiredFieldMarker="True" ErrorMessage="E-mail Address is required." />
									<vam:EmailAddressValidator id="eavEmail" runat="server" ControlIDToEvaluate="txtEmail" ErrorMessage="Invalid E-mail Address." />
								</td>
								<td class="homesearch">
									( <asp:TextBox id="txtPhoneNumber1" MaxLength="3" Columns="1" runat="server" /> )
										<asp:TextBox id="txtPhoneNumber2" MaxLength="3" Columns="1" runat="server" /> -
										<asp:TextBox id="txtPhoneNumber3" MaxLength="4" Columns="2" runat="server" />
										<vam:RequiredTextValidator id="rtvPhoneNumber1" runat="server" ControlIDToEvaluate="txtPhoneNumber1" ShowRequiredFieldMarker="False" ErrorMessage="Area code is required." />
										<vam:RequiredTextValidator id="rtvPhoneNumber2" runat="server" ControlIDToEvaluate="txtPhoneNumber2" ShowRequiredFieldMarker="False" ErrorMessage="Prefix is required." />
										<vam:RequiredTextValidator id="rtvPhoneNumber3" runat="server" ControlIDToEvaluate="txtPhoneNumber3" ShowRequiredFieldMarker="False" ErrorMessage="Line number is required." />
										<vam:CombinedErrorMessages id="cemTelephone" ShowRequiredFieldMarker="True" runat="server">
											<Validators>
												<vam:ValidatorControlConnection ControlID="rtvPhoneNumber1" />
												<vam:ValidatorControlConnection ControlID="rtvPhoneNumber2" />
												<vam:ValidatorControlConnection ControlID="rtvPhoneNumber3" />
											</Validators>
										</vam:CombinedErrorMessages>
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
								<td class="homesearch" colspan="2">
									We take your privacy very seriously.  Your e-mail address, telephone number, and personal information will <span class="bold">not</span> be given to any third party, will <span class="bold">not</span> be added to an e-mail list unless desired and will <span class="bold">not</span> be used in any way other than to provide you with up-to-date information about the Raleigh real estate market.
								</td>
							</tr>
							<tr>
								<td class="homesearch" colspan="2">
									<asp:Button id="submitButton" Text="Evaluate My Home" CausesValidation="True" OnClick="Submit_OnClick" runat="server" />&nbsp;&nbsp;<input type="reset" value="Cancel">
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
