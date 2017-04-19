<%@ Page Language="C#" Debug="False" Inherits="ChrisEdwardsGroup.ListManage.AddResidential" %>

<%@ Register Tagprefix="Top" Tagname="Bar" Src="includes/topbar.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<html>
	<head>
		<title>Chris Edwards Group - Listing Management - Add Residential Property</title>
			<link rel="stylesheet" href="includes/listing_management.css" type="text/css">
			<script language="JavaScript" src="includes/manage_error.js"></script>
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
						<td class="pagetitle">Add Listing: Residential Property</td>
					</tr>
				</table>
			</td>
	</tr>
	<tr>
    <td colspan="3" class="innercontent">
				
					<div id="hgcErrors" Visible="false" class="errorsummary" runat="server" />
							
							<asp:ValidationSummary id="vsmResidential" 
     							DisplayMode="BulletList" 
     							EnableClientScript="True"
     							ShowSummary="True"
     							ShowMessageBox="True"                        
     							HeaderText="Summary of Errors:"
												CssClass="errorsummary"
     							runat="server" />

								<asp:Table id="tblResListing" runat="server">
								
									<asp:TableRow>
						   	<asp:TableCell CssClass="sectionhead" ColumnSpan="3">Property Information</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead">MLS#:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Alt. MLS#:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Price:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
						   	<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtMLS" MaxLength="10" Columns="10" runat="server" />
												<asp:RequiredFieldValidator id="RFVtxtMLS" ControlToValidate="txtMLS" ErrorMessage="You must fill in the MLS field." Text="*" runat="server" />
												<asp:RegularExpressionValidator id="RegExtxtMLS" ControlToValidate="txtMLS" ValidationExpression="^(\w|\d){5,}$" Display="Static" EnableClientScript="true" ErrorMessage="You must enter a valid MLS number." Text="*" runat="server"/>
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtAltMLS" MaxLength="10" Columns="10" runat="server" />
												<asp:RegularExpressionValidator id="RegExtxtAltMLS" ControlToValidate="txtAltMLS" ValidationExpression="^(\w|\d){5,}$" Display="Static" EnableClientScript="True" ErrorMessage="You must enter a valid Alternate MLS number." Text="*" runat="server"/>
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											$<asp:TextBox id="txtPrice" MaxLength="17" Columns="16" runat="server" />
												<asp:RequiredFieldValidator id="RFVtxtPrice" ControlToValidate="txtPrice" ErrorMessage="You must fill in the Price field." Text="*" runat="server" />
												<asp:RangeValidator id="RNGtxtPrice" ControlToValidate="txtPrice" MinimumValue="1" MaximumValue="99999999999999999" Type="Currency" EnableClientScript="True" ErrorMessage="You must enter a valid Price." Text="*" runat="server"/>
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead">Listing Agent #1:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Listing Agent #2:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">&nbsp;</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
						   		<asp:TableCell CssClass="padleftinput">
												<asp:DropDownList id="ddlAgentID1" class="dropdown" runat="server" />
												<asp:RequiredFieldValidator id="RFVddlAgentID1" ControlToValidate="ddlAgentID1" ErrorMessage="You must make a selection from the Agent #1 drop-down list." Text="*" runat="server" />
											</asp:TableCell>
											<asp:TableCell CssClass="padleftinput">
												<asp:DropDownList id="ddlAgentID2" class="dropdown" runat="server" />
											</asp:TableCell>
											<asp:TableCell CssClass="padleftinput">
												&nbsp;
											</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
										<asp:TableCell CssClass="cellhead">Owner:</asp:TableCell>
						   	<asp:TableCell CssClass="cellhead" ColumnSpan="2">Tagline:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtOwner" MaxLength="35" Columns="30" runat="server" />
												<asp:RequiredFieldValidator id="RFVtxtOwner" ControlToValidate="txtOwner" ErrorMessage="You must fill in the Owner field." Text="*" runat="server" />
										</asp:TableCell>
						   	<asp:TableCell CssClass="padleftinput" ColumnSpan="2">
											<asp:TextBox id="txtTagline" MaxLength="50" Columns="48" runat="server" />
											<asp:RequiredFieldValidator id="RFVtxtTagline" ControlToValidate="txtTagline" ErrorMessage="You must fill in the Tagline field." Text="*" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="sectionhead" ColumnSpan="3">Address</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead">Address #1:</asp:TableCell>
											<asp:TableCell CssClass="cellhead" ColumnSpan="2">Address #2:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtAddress1" MaxLength="50" Columns="30" runat="server" />
												<asp:RequiredFieldValidator id="RFVtxtAddress1" ControlToValidate="txtAddress1" ErrorMessage="You must fill in the Address #1 field." Text="*" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput" ColumnSpan="2">
											<asp:TextBox id="txtAddress2" MaxLength="50" Columns="35" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead">City:</asp:TableCell>
											<asp:TableCell CssClass="cellhead" ColumnSpan="2">Subdivision:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
						   		<asp:TableCell CssClass="padleftinput">
												<asp:DropDownList id="ddlCityID" class="dropdown" runat="server" />
												<asp:RequiredFieldValidator id="RFVddlCityID" ControlToValidate="ddlCityID" ErrorMessage="You must make a selection from the City drop-down list." Text="*" runat="server" />
											</asp:TableCell>
											<asp:TableCell CssClass="padleftinput" ColumnSpan="2">
											<asp:TextBox id="txtSubdivision" MaxLength="60" Columns="45" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="sectionhead" ColumnSpan="3">Annual Taxes</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead">Taxes:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Tax Year:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Schedule Number:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
						   	<asp:TableCell CssClass="padleftinput">
											$<asp:TextBox id="txtAnnualTaxes" MaxLength="10" Columns="8" runat="server" />
											<asp:RangeValidator id="RNGtxtAnnualTaxes" ControlToValidate="txtAnnualTaxes" MinimumValue="1" MaximumValue="9999999999" Type="Currency" EnableClientScript="true" ErrorMessage="You must fill in a valid Annual Taxes amount." Text="*" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtAnnualTaxYear" MaxLength="4" Columns="1" runat="server" />
											<asp:RangeValidator id="RNGtxtAnnualTaxYear" ControlToValidate="txtAnnualTaxYear" MinimumValue="1990" MaximumValue="2100" Type="Integer" EnableClientScript="true" ErrorMessage="You must fill in a valid Annual Tax Year." Text="*" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtScheduleNumber" MaxLength="30" Columns="25" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
					  			<asp:TableCell CssClass="cellhead" ColumnSpan="3">Assessments:</asp:TableCell>
							 	</asp:TableRow>
									<asp:TableRow>
							  	<asp:TableCell CssClass="padleftinput" ColumnSpan="3">
											<asp:TextBox id="txtAssessments" MaxLength="100" Columns="75" runat="server" />
										</asp:TableCell>
							 	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="sectionhead" ColumnSpan="3">Property Features</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead">Bedrooms:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Baths:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Square Feet:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
						   	<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtBedrooms" MaxLength="6" Columns="3" runat="server" />
											<asp:RequiredFieldValidator id="RFVtxtBedrooms" ControlToValidate="txtBedrooms" ErrorMessage="You must fill in the Bedrooms field." Text="*" runat="server" />
											<asp:RangeValidator id="RNGtxtBedrooms" ControlToValidate="txtBedrooms" MinimumValue="0.25" MaximumValue="999.99" Type="Double" EnableClientScript="true" ErrorMessage="You must fill in a valid Bedrooms value." Text="*" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtBaths" MaxLength="6" Columns="3" runat="server" />
											<asp:RequiredFieldValidator id="RFVtxtBaths" ControlToValidate="txtBaths" ErrorMessage="You must fill in the Baths field." Text="*" runat="server" />
											<asp:RangeValidator id="RNGtxtBaths" ControlToValidate="txtBaths" MinimumValue="0.25" MaximumValue="999.99" Type="Double" EnableClientScript="True" ErrorMessage="You must fill in a valid Baths value." Text="*" runat="server"/>
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtSquareFt" MaxLength="8" Columns="8" runat="server" />
											<asp:RequiredFieldValidator id="RFVtxtSquareFt" ControlToValidate="txtSquareFt" ErrorMessage="You must fill in the Square Feet field." Text="*" runat="server" />
											<asp:RangeValidator id="RNGtxtSquareFt" ControlToValidate="txtSquareFt" MinimumValue="1" MaximumValue="99999999" Type="Integer" EnableClientScript="true" ErrorMessage="You must fill in a valid Square Feet value." Text="*" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   	<asp:TableCell CssClass="cellhead">Year Built:</asp:TableCell>
										<asp:TableCell CssClass="cellhead">Year Remodeled:</asp:TableCell>
										<asp:TableCell CssClass="cellhead">Parcel Size:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
						   	<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtYearBuilt" MaxLength="4" Columns="1" runat="server" />
											<asp:RequiredFieldValidator id="RFVtxtYearBuilt" ControlToValidate="txtYearBuilt" ErrorMessage="You must fill in the Year Built field." Text="*" runat="server" />
											<asp:RangeValidator id="RNGtxtYearBuilt" ControlToValidate="txtYearBuilt" MinimumValue="1600" MaximumValue="2100" Type="Integer" EnableClientScript="True" ErrorMessage="You must fill in a valid Year Built value." Text="*" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtYearRemodeled" MaxLength="4" Columns="1" runat="server" />
											<asp:RangeValidator id="RNGtxtYearRemodeled" ControlToValidate="txtYearRemodeled" MinimumValue="1600" MaximumValue="2100" Type="Integer" EnableClientScript="True" ErrorMessage="You must fill in a valid Year Remodeled value." Text="*" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtParcelSize" MaxLength="30" Columns="30" runat="server" />
											<asp:RequiredFieldValidator id="RFVtxtParcelSize" ControlToValidate="txtParcelSize" ErrorMessage="You must fill in the Parcel Size field." Text="*" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
										<asp:TableCell CssClass="cellhead">Style:</asp:TableCell>
						   	<asp:TableCell CssClass="cellhead">Foundation:</asp:TableCell>
										<asp:TableCell CssClass="cellhead">Construction:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput">
											<asp:DropDownList id="ddlStyle" class="dropdown" runat="server" />
											<asp:RequiredFieldValidator id="RFVddlStyle" ControlToValidate="ddlStyle" ErrorMessage="You must make a selection from the Style drop-down list." Text="*" runat="server" />
										</asp:TableCell>
						   	<asp:TableCell CssClass="padleftinput">
											<asp:DropDownList id="ddlFoundation" class="dropdown" runat="server" />
											<asp:RequiredFieldValidator id="RFVddlFoundation" ControlToValidate="ddlFoundation" ErrorMessage="You must make a selection from the Foundation drop-down list." Text="*" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:DropDownList id="ddlConstruction" class="dropdown" runat="server" />	
											<asp:RequiredFieldValidator id="RFVddlConstruction" ControlToValidate="ddlConstruction" ErrorMessage="You must make a selection from the Construction drop-down list." Text="*" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>						
										<asp:TableCell CssClass="cellhead">Roof:</asp:TableCell>
						   	<asp:TableCell CssClass="cellhead" ColumnSpan="2">Garage:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput">
											<asp:DropDownList id="ddlRoof" class="dropdown" runat="server" />
											<asp:RequiredFieldValidator id="RFVddlRoof" ControlToValidate="ddlRoof" ErrorMessage="You must make a selection from the Roof drop-down list." Text="*" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput" ColumnSpan="2">
											<asp:DropDownList id="ddlGarage" class="dropdown" runat="server" />
											<asp:RequiredFieldValidator id="RFVddlGarage" ControlToValidate="ddlGarage" ErrorMessage="You must make a selection from the Garage drop-down list." Text="*" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
										<asp:TableCell CssClass="cellhead">Patio:</asp:TableCell>
										<asp:TableCell CssClass="cellhead" ColumnSpan="2">Deck:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput">
											<asp:RadioButtonList id="rblPatio" RepeatDirection="Horizontal" RepeatLayout="Flow" TextAlign="Right" runat="server">
												<asp:ListItem Text="Yes" Value="1" />
												<asp:ListItem Text="No" Value="0" Selected="True" />
											</asp:RadioButtonList>
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput" ColumnSpan="2">
											<asp:RadioButtonList id="rblDeck" RepeatDirection="Horizontal" RepeatLayout="Flow" TextAlign="Right" runat="server">
												<asp:ListItem Text="Yes" Value="1" />
												<asp:ListItem Text="No" Value="0" Selected="True" />
											</asp:RadioButtonList>
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
										<asp:TableCell CssClass="cellhead">Fenced:</asp:TableCell>
										<asp:TableCell CssClass="cellhead" ColumnSpan="2">Fencing Description:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput">
											<asp:RadioButtonList id="rblFenced" RepeatDirection="Horizontal" RepeatLayout="Flow" TextAlign="Right" runat="server">
												<asp:ListItem Text="Yes" Value="1" />
												<asp:ListItem Text="No" Value="0" Selected="True" />
											</asp:RadioButtonList>
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput" ColumnSpan="2">
											<asp:TextBox id="txtFencingDescription" MaxLength="50" Columns="45" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="sectionhead" ColumnSpan="3">Utilities</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead">Heating:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Fireplace:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Woodstove/Pellet Stove:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput">
											<asp:DropDownList id="ddlHeating" class="dropdown" runat="server" />
											<asp:RequiredFieldValidator id="RFVddlHeating" ControlToValidate="ddlHeating" ErrorMessage="You must make a selection from the Heating drop-down list." Text="*" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:RadioButtonList id="rblFireplace" RepeatDirection="Horizontal" RepeatLayout="Flow" TextAlign="Right" runat="server">
												<asp:ListItem Text="Yes" Value="1" />
												<asp:ListItem Text="No" Value="0" Selected="True" />
											</asp:RadioButtonList>
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:RadioButtonList id="rblWoodstove" RepeatDirection="Horizontal" RepeatLayout="Flow" TextAlign="Right" runat="server">
												<asp:ListItem Text="Yes" Value="1" />
												<asp:ListItem Text="No" Value="0" Selected="True" />
											</asp:RadioButtonList>
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead">Electricity Provider:</asp:TableCell>
											<asp:TableCell CssClass="cellhead" ColumnSpan="2">Electricity Monthly Cost:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput">
											<asp:DropDownList id="ddlElectricityProvider" class="dropdown" runat="server" />
											<asp:RequiredFieldValidator id="RFVddlElectricityProvider" ControlToValidate="ddlElectricityProvider" ErrorMessage="You must make a selection from the Electricity Provider drop-down list." Text="*" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput" ColumnSpan="2">
												$<asp:TextBox id="txtElectricityMonthlyCost" MaxLength="10" Columns="8" runat="server" />
												<asp:RangeValidator id="RNGtxtElectricityMonthlyCost" ControlToValidate="txtElectricityMonthlyCost" MinimumValue="1" MaximumValue="9999999999" Type="Currency" EnableClientScript="True" ErrorMessage="You must fill in a valid Electricity Monthly Cost value." Text="*" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead">Domestic Water Provider:</asp:TableCell>
											<asp:TableCell CssClass="cellhead" ColumnSpan="2">Domestic Water Monthly Cost:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtDomWaterProvider" MaxLength="50" Columns="30" runat="server" />
											<asp:RequiredFieldValidator id="RFVtxtDomWaterProvider" ControlToValidate="txtDomWaterProvider" ErrorMessage="You must fill in the Domestic Water Provider field." Text="*" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput" ColumnSpan="2">
											$<asp:TextBox id="txtDomWaterMonthlyCost" MaxLength="10" Columns="8" runat="server" />
											<asp:RangeValidator id="RNGtxtDomWaterMonthlyCost" ControlToValidate="txtDomWaterMonthlyCost" MinimumValue="1" MaximumValue="9999999999" Type="Currency" EnableClientScript="True" ErrorMessage="You must fill in a valid Domestic Water Monthly Cost value." Text="*" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead">Irrigation Water Provider:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Irrigation Shares:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Irrigation Monthly Cost:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtIrrWaterProvider" MaxLength="50" Columns="30" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtIrrWaterShares" MaxLength="8" Columns="6" runat="server" />
											<asp:RangeValidator id="RNGtxtIrrWaterShares" ControlToValidate="txtIrrWaterShares" MinimumValue="0.10" MaximumValue="99999999" Type="Double" EnableClientScript="True" ErrorMessage="You must fill in a valid Irrigation Water Shares value." Text="*" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											$<asp:TextBox id="txtIrrWaterMonthlyCost" MaxLength="10" Columns="8" runat="server" />
											<asp:RangeValidator id="RNGtxtIrrWaterMonthlyCost" ControlToValidate="txtIrrWaterMonthlyCost" MinimumValue="1" MaximumValue="9999999999" Type="Currency" EnableClientScript="True" ErrorMessage="You must fill in a valid Irrigation Water Monthly Cost value." Text="*" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead" ColumnSpan="3">Sewer:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
						   	<asp:TableCell CssClass="padleftinput" ColumnSpan="3">
											<asp:RadioButtonList id="rblSewer" RepeatDirection="Horizontal" RepeatLayout="Flow" TextAlign="Right" runat="server">
												<asp:ListItem Text="Sewer" Value="Sewer" Selected="True" />
												<asp:ListItem Text="Septic" Value="Septic" />
											</asp:RadioButtonList>
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="sectionhead" ColumnSpan="3">Room Dimensions</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead">Kitchen:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Living Room:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Dining Room:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtKitchenDim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtLivingRoomDim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtDiningRoomDim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead" ColumnSpan="3">Family Room:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput" ColumnSpan="3">
											<asp:TextBox id="txtFamilyRoomDim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="sectionhead" ColumnSpan="3">Bedroom Dimensions</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead">Master Bedroom:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Bedroom #2:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Bedroom #3:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtMasterBedDim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtBedroom2Dim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtBedroom3Dim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead" ColumnSpan="3">Bedroom #4:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput" ColumnSpan="3">
											<asp:TextBox id="txtBedroom4Dim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="sectionhead" ColumnSpan="3">Bathroom Dimensions</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead">Bathroom #1:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Bathroom #2:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Bathroom #3:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtBathroom1Dim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtBathroom2Dim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtBathroom3Dim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead" ColumnSpan="3">Bathroom #4:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput" ColumnSpan="3">
											<asp:TextBox id="txtBathroom4Dim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="sectionhead" ColumnSpan="3">Other Dimensions</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead">Basement Dimensions:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Garage Dimensions:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Patio Dimensions:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtBasementDim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtGarageDim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtPatioDim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead">Deck Dimensions:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Shed Dimensions:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Office Dimensions:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtDeckDim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtShedDim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtOfficeDim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
										<asp:TableCell CssClass="cellhead">Media Room Dimensions:</asp:TableCell>
										<asp:TableCell CssClass="cellhead">Laundry Room Dimensions:</asp:TableCell>
										<asp:TableCell CssClass="cellhead">Sunroom Dimensions:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtMediaRoomDim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtLaundryRoomDim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											<asp:TextBox id="txtSunroomDim" MaxLength="30" Columns="20" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   	<asp:TableCell CssClass="sectionhead" ColumnSpan="3">Possession/Earnest Money</asp:TableCell>

						  	</asp:TableRow>
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead" ColumnSpan="2">Possession:</asp:TableCell>
											<asp:TableCell CssClass="cellhead">Earnest Money:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput" ColumnSpan="2">
											<asp:DropDownList id="ddlPossession" class="dropdown" runat="server" />
											<asp:RequiredFieldValidator id="RFVddlPossession" ControlToValidate="ddlPossession" ErrorMessage="You must make a selection from the Possession drop-down list." Text="*" runat="server" />
										</asp:TableCell>
										<asp:TableCell CssClass="padleftinput">
											$<asp:TextBox id="txtEarnestMoney" MaxLength="10" Columns="8" runat="server" />
											<asp:RequiredFieldValidator id="RFVtxtEarnestMoney" ControlToValidate="txtEarnestMoney" ErrorMessage="You must fill in the Earnest Money field." Text="*" runat="server" />
											<asp:RangeValidator id="RNGtxtEarnestMoney" ControlToValidate="txtEarnestMoney" MinimumValue="1" MaximumValue="9999999999" Type="Currency" EnableClientScript="True" ErrorMessage="You must fill in a valid Earnest Money value." Text="*" runat="server" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="sectionhead" ColumnSpan="3">Description</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead" ColumnSpan="3">Features Description:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput" ColumnSpan="3">
											<asp:TextBox id="txtFeaturesDescription" Columns="75" Rows="4" TextMode="Multiline" CssClass="textarea8pt" runat="server"/>
											<asp:RequiredFieldValidator id="RFVtxtFeaturesDescription" ControlToValidate="txtFeaturesDescription" ErrorMessage="You must fill in the Features Description field." Text="*" runat="server" />
											<asp:CustomValidator runat="server" ControlToValidate="txtFeaturesDescription" OnServerValidate="txtDescription_Validate750" ClientValidationFunction="txtDescription_ClientValidate750" ErrorMessage="The Features Description field may only contain 750 characters or less." Text="*" Display="Static" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead" ColumnSpan="3">Inclusions Description:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput" ColumnSpan="3">
											<asp:TextBox id="txtInclusionsDescription" Columns="75" Rows="4" TextMode="Multiline" CssClass="textarea8pt" runat="server"/>
											<asp:RequiredFieldValidator id="RFVtxtInclusionsDescription" ControlToValidate="txtInclusionsDescription" ErrorMessage="You must fill in the Inclusions Description field." Text="*" runat="server" />
											<asp:CustomValidator runat="server" ControlToValidate="txtInclusionsDescription" OnServerValidate="txtDescription_Validate250" ClientValidationFunction="txtDescription_ClientValidate250" ErrorMessage="The Inclusions Description field may only contain 250 characters or less." Text="*" Display="Static" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead" ColumnSpan="3">Exclusions Description:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput" ColumnSpan="3">
											<asp:TextBox id="txtExclusionsDescription" Columns="75" Rows="4" TextMode="Multiline" CssClass="textarea8pt" runat="server"/>
											<asp:RequiredFieldValidator id="RFVtxtExclusionsDescription" ControlToValidate="txtExclusionsDescription" ErrorMessage="You must fill in the Exclusions Description field." Text="*" runat="server" />
											<asp:CustomValidator runat="server" ControlToValidate="txtExclusionsDescription" OnServerValidate="txtDescription_Validate250" ClientValidationFunction="txtDescription_ClientValidate250" ErrorMessage="The Exclusions Description field may only contain 250 characters or less." Display="Static" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead" ColumnSpan="3">Outbuildings Description:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput" ColumnSpan="3">
											<asp:TextBox id="txtOutbuildingsDescription" Columns="75" Rows="4" TextMode="Multiline" CssClass="textarea8pt" runat="server"/>
											<asp:CustomValidator runat="server" ControlToValidate="txtOutbuildingsDescription" OnServerValidate="txtDescription_Validate250" ClientValidationFunction="txtDescription_ClientValidate250" ErrorMessage="The Outbuildings Description field may only contain 250 characters or less." Text="*" Display="Static" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead" ColumnSpan="3">Disclosures Description:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput" ColumnSpan="3">
											<asp:TextBox id="txtDisclosuresDescription" Columns="75" Rows="4" TextMode="Multiline" CssClass="textarea8pt" runat="server"/>
											<asp:CustomValidator runat="server" ControlToValidate="txtDisclosuresDescription" OnServerValidate="txtDescription_Validate250" ClientValidationFunction="txtDescription_ClientValidate250" ErrorMessage="The Disclosures Description field may only contain 250 characters or less." Text="*" Display="Static" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="sectionhead" ColumnSpan="3">Directions</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
						   		<asp:TableCell CssClass="cellhead" ColumnSpan="3">Map Directions to Property:</asp:TableCell>
						  	</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell CssClass="padleftinput" ColumnSpan="3">
											<asp:TextBox id="txtMapDirections" Columns="75" Rows="4" TextMode="Multiline" CssClass="textarea8pt" runat="server"/>
											<asp:CustomValidator runat="server" ControlToValidate="txtMapDirections" OnServerValidate="txtDescription_Validate250" ClientValidationFunction="txtDescription_ClientValidate250" ErrorMessage="The Map Directions field may only contain 250 characters or less." Display="Static" />
										</asp:TableCell>
						  	</asp:TableRow>
									
									<asp:TableRow>
						   		<asp:TableCell CssClass="padleftinput" ColumnSpan="3">
												<asp:Button id="submitButton" CssClass="buttons" Text="Add Residential Property" CausesValidation="True" OnClick="AddResidential_Click" runat="server" />&nbsp;&nbsp;<input type="reset" class="buttons" value="Clear Form">
											</asp:TableCell>
						  	</asp:TableRow>
									
								</asp:Table>
				</td>
</tr>
</table>

</form>
	
	</body>
</html>
