<%@ Page Language="C#" Debug="False" Inherits="ChrisEdwardsGroup.ListManage.DatabaseStats" %>

<%@ Register Tagprefix="Top" Tagname="Bar" Src="includes/topbar.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<html>
	<head>
		<title>Chris Edwards Group - Listings Management: Database Statistics</title>
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
						<td class="pagetitle">Listings Management: Database Statistics</td>
					</tr>
				</table>
			</td>
	</tr>
	<tr>
    <td colspan="3" class="innercontent">
				
					<asp:Label id="lblOutError" visible="false" runat="server"/>
							
							<table border="1" cellpadding="3" class="homepagetop">
								<tr>
									<th class="sectionhead" colspan="2">Database Statistics:</th>
								</tr>
							</table>
							
							<table border="1" cellpadding="6" class="dbasestats">
								<tr>
									<th colspan="2" class="dbasehead">Property Listings</th>
								</tr>
								<tr>
									<td class="dbaseitem">Business (no Real Estate)</td>
									<td class="dbaseitemralign"><asp:Literal id="litBusinessNoREQty" runat="server"/></td>
								</tr>
								<tr>
									<td class="dbaseitem">Business with Real Estate</td>
									<td class="dbaseitemralign"><asp:Literal id="litBusinessWithREQty" runat="server"/></td>
								</tr>
								<tr>
									<td class="dbaseitem">Commercial Real Estate</td>
									<td class="dbaseitemralign"><asp:Literal id="litCommercialREQty" runat="server"/></td>
								</tr>
								<tr>
									<td class="dbaseitem">Farm &amp; Ranch Properties</td>
									<td class="dbaseitemralign"><asp:Literal id="litFarmRanchQty" runat="server"/></td>
								</tr>
								<tr>
									<td class="dbaseitem">Income Producing Property</td>
									<td class="dbaseitemralign"><asp:Literal id="litIncomeProducingPropQty" runat="server"/></td>
								</tr>
								<tr>
									<td class="dbaseitem">Residential Properties</td>
									<td class="dbaseitemralign"><asp:Literal id="litResidentialQty" runat="server"/></td>
								</tr>
								
								<tr>
									<td class="dbaseitem">Vacant Land</td>
									<td class="dbaseitemralign"><asp:Literal id="litVacantLandQty" runat="server"/></td>
								</tr>
								<tr>
									<td class="dbaseitem">Total:</td>
									<td class="dbaseitemralign"><asp:Literal id="litTotalQty" runat="server"/></td>
								</tr>
							</table>
							
							<table border="1" cellpadding="6" class="dbasestats">
								<tr>
									<th colspan="2" class="dbasehead">Pictures</th>
								</tr>
								<tr>
									<td class="dbaseitem">Total Pictures:</td>
									<td class="dbaseitemralign"><asp:Literal id="litTotalPicQty" runat="server"/></td>
								</tr>
								<tr>
									<td class="dbaseitem">Average Number of Pictures Per Listing:</td>
									<td class="dbaseitemralign"><asp:Literal id="litPicPerListingAvg" runat="server"/></td>
								</tr>
							</table>
							
				</td>
</tr>
</table>

</form>

	</body>
</html>
