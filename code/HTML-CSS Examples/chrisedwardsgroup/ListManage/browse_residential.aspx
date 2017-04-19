<%@ Page Language="C#" Debug="False" Inherits="ChrisEdwardsGroup.ListManage.BrowseResidential" %>

<%@ Register Tagprefix="Top" Tagname="Bar" Src="includes/topbar.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<html>
	<head>
		<title>Chris Edwards Group - Listing Management - Browse Residential Property</title>
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
						<td class="pagetitle">Browse Listings: Residential Property</td>
					</tr>
				</table>
			</td>
	</tr>
	<tr>
    <td colspan="3" class="innercontent">
				
					<asp:Label id="lblOutError" visible="false" runat="server" />
						
						<table>
							<tbody>
								<tr>
									<td class="recordcount"><asp:Label id="lblRecordCount" visible="True" CssClass="recordcount" runat="server" /></td>
								</tr>
							</tbody>
						</table>

						<asp:DataGrid id="dtgBrowse"
									AllowPaging="True"
									AllowSorting="False"
									AutoGenerateColumns="False"
									CellPadding="7"
									GridLines="Both"
									BorderColor="Black"
									ShowFooter="False"
									ShowHeader="True"
									PageSize="8"
				     OnPageIndexChanged="PageIndexChanged_OnClick"
									OnItemCommand="ItemCommand_OnClick"
									OnItemCreated="BrowseResidential_ItemCreated"
									DataKeyField="MLS"
									runat="server">
									
							  <AlternatingItemStyle BackColor="LightGoldenrodYellow" />
							  <HeaderStyle CssClass="browseheader" />
									<PagerStyle Mode="NumericPages" PageButtonCount="16" />

									<Columns>
										<asp:BoundColumn runat="server" DataField="MLS" HeaderText="MLS" />	
										<asp:BoundColumn runat="server" DataField="ManagementPage" HeaderText="Management Page" Visible="False" />								
										<asp:BoundColumn runat="server" DataField="Price" HeaderText="Price" DataFormatString="{0:C}" SortExpression="Price, City" />
										<asp:BoundColumn runat="server" DataField="Address" HeaderText="Address" />
										<asp:BoundColumn runat="server" DataField="City" HeaderText="City" SortExpression="City, Price" />
										<asp:BoundColumn runat="server" DataField="Owner" HeaderText="Owner" SortExpression="Owner" />
										<asp:BoundColumn runat="server" DataField="Agent" HeaderText="Listing Agent" />
										<asp:ButtonColumn runat="server" Text="Edit" CommandName="editinfo">
											<itemstyle backcolor="LightSteelBlue" font-bold="true" />
										</asp:ButtonColumn>
									</Columns>
																									
								</asp:DataGrid>
								
				</td>
</tr>
</table>

</form>

	</body>
</html>
