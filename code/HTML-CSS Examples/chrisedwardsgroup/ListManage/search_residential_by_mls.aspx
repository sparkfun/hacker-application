<%@ Page Language="C#" Debug="False" Inherits="ChrisEdwardsGroup.ListManage.SearchResidentialByMLS" %>

<%@ Register Tagprefix="Top" Tagname="Bar" Src="includes/topbar.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<html>
	<head>
		<title>Chris Edwards Group - Listing Management - Search Residential By MLS</title>
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
						<td class="pagetitle">Search Listings: Residential By MLS</td>
					</tr>
				</table>
			</td>
	</tr>
	<tr>
    <td colspan="3" class="innercontent">
				
				<div id="hgcErrors" Visible="false" class="errorsummary" runat="server" />
				
				<asp:ValidationSummary id="vsmSearchRes" DisplayMode="BulletList" EnableClientScript="True" ShowSummary="True" ShowMessageBox="True" HeaderText="Summary of Errors:" CssClass="errorsummary" runat="server" />
				
				<asp:Table id="tblSearchResidential" CssClass="searchbox" runat="server">
					<asp:TableRow>
		   	<asp:TableCell CssClass="searchhead">MLS#:</asp:TableCell>
						<asp:TableCell CssClass="searchhead">Alt. MLS#:</asp:TableCell>
						<asp:TableCell CssClass="searchhead">&nbsp;</asp:TableCell>
		  	</asp:TableRow>
					<asp:TableRow>
		   	<asp:TableCell CssClass="padleftinput">
							<asp:TextBox id="txtMLS" MaxLength="6" Columns="5" runat="server" />
							<asp:RequiredFieldValidator id="rfvMLS" ControlToValidate="txtMLS" ErrorMessage="You must fill in the MLS field." Text="*" runat="server" />
							<asp:RegularExpressionValidator id="RegExtxtMLS" ControlToValidate="txtMLS" ValidationExpression="\d{5,6}" Display="Static" EnableClientScript="true" ErrorMessage="You must enter a valid MLS number." Text="*" runat="server"/>
						</asp:TableCell>
						<asp:TableCell CssClass="padleftinput">
							<asp:TextBox id="txtAltMLS" MaxLength="6" Columns="5" runat="server" />
							<asp:RegularExpressionValidator id="RegExtxtAltMLS" ControlToValidate="txtAltMLS" ValidationExpression="\d{5,6}" Display="Static" EnableClientScript="True" ErrorMessage="You must enter a valid Alternate MLS number." Text="*" runat="server"/>
						</asp:TableCell>
						<asp:TableCell CssClass="padleftinput">
							<asp:Button id="searchButton" CssClass="buttons" Text="Search by MLS" CausesValidation="True" OnClick="SearchResidentialByMLS_Click" runat="server" />
						</asp:TableCell>
		  	</asp:TableRow>
				</asp:Table>
				
				<asp:Table id="tblRecordCount" Visible="False" runat="server">
					<asp:TableRow>
						<asp:TableCell id="tcRecordCount" CssClass="recordcount" />
		  	</asp:TableRow>
				</asp:Table>

						<asp:DataGrid id="dtgSearchResByMLS"
									AllowPaging="False"
									AllowSorting="False"
									AutoGenerateColumns="False"
									CellPadding="7"
									GridLines="Both"
									BorderColor="Black"
									ShowFooter="False"
									ShowHeader="True"
									PageSize="6"
									OnItemCommand="ItemCommand_OnClick"
									OnItemCreated="SearchResidentialByMLS_ItemCreated"
									DataKeyField="MLS"
									Visible="False"
									runat="server">
									
							  <AlternatingItemStyle BackColor="LightGoldenrodYellow" />
							  <HeaderStyle CssClass="browseheader" />
									<PagerStyle Mode="NumericPages" PageButtonCount="6" />

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
