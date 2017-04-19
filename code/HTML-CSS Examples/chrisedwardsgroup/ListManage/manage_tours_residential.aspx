<%@ Page Language="C#" Debug="False" Inherits="ChrisEdwardsGroup.ListManage.ManageTours" %>

<%@ Register Tagprefix="Top" Tagname="Bar" Src="includes/topbar.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<html>
	<head>
		<title>Chris Edwards Group - Listings Management - Manage Tours: Residential Property</title>
			<link rel="stylesheet" href="includes/listing_management.css" type="text/css">
			<script language="JavaScript" src="includes/manage_error.js"></script>
			<script language="JavaScript1.2" src="includes/menu.js"></script>
			<script language="JavaScript1.2" src="includes/stm_script.js"></script>
	</head>

	<body>
	
	<form enctype="multipart/form-data" runat="server">
	
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
						<td class="pagetitle" colspan="3">Manage Tours: Residential Property</td>
					</tr>
					<tr>
						<td class="quicklisting">Quick Info:</td>
						<td class="quicklistinginfo">MLS: <asp:Label id="lblMLS" runat="server" /></td>
						<td class="quicklistinginfo">Total Tours: <asp:Label id="lblTotalTours" runat="server" /></td>
					</tr>
				</table>
			</td>
	</tr>
	<tr>
    <td colspan="3" class="innercontent">
					
					<asp:Label id="lblOutError" visible="false" runat="server" />
							
							<table border="0" class="imagehead">
								<tbody>
									<tr>
										<td class="imagehead">
											<a href="manage_residential.aspx"><img src="graphics/manage_bar_tour_01.gif" alt="Manage Property" width="132" height="24" border="0"></a><a href="manage_pictures_residential.aspx"><img src="graphics/manage_bar_tour_02.gif" alt="Manage Pictures" width="132" height="24" border="0"></a><a href="manage_tours_residential.aspx"><img src="graphics/manage_bar_tour_03.gif" alt="Manage Tours" width="145" height="24" border="0"></a></td>
									</tr>
								</tbody>
							</table>
							
							<table class="managephotos" cellpadding="5">
								<thead>
									<tr>
										<th class="sectionheadpics" colspan="3">Add New Tours</th>
									</tr>
								</thead>
								<tbody>
									<tr>
										<td>Virtual Tour Link:</td>
										<td>
											<asp:TextBox id="txtTourUrl" MaxLength="80" Columns="60" runat="server" />
											<asp:RequiredFieldValidator id="rfvTourUrl" ControlToValidate="txtTourUrl" ErrorMessage="You must fill in the Virtual Tour Link field." Text="*" runat="server" />
										</td>
										<td>
											<asp:Button id="btnAddUrl" CssClass="buttons" Text="Add Tour Link" CausesValidation="True" OnClick="AddTourUrl_Click" runat="server" />
										</td>
									</tr>
								</tbody>
							</table>
													
							<table class="managephotos" cellpadding="5">
								<thead>
									<tr>
										<th class="sectionheadpics" colspan="2">Manage Tours</th>
									</tr>
								</thead>
								<tbody>
									<tr>
										<td>
										
											<asp:DataList id="dtlManageTours"
							     CellPadding="5"
												CssClass="managepictures"
							     DataKeyField="TourID"
							     ExtractTemplateRows="True"
							     GridLines="Both"
							     ShowFooter="False"
							     ShowHeader="True"
							     OnCancelCommand="ManageTours_CancelCommand"
												OnDeleteCommand="ManageTours_DeleteCommand"
							     OnEditCommand="ManageTours_EditCommand"
												OnItemCreated="ManageTours_ItemCreated"
							     OnUpdateCommand="ManageTours_UpdateCommand"
							     runat="server">
											
										<EditItemStyle BackColor="Khaki" />
							  	<HeaderStyle CssClass="sectionheadpics" />
							
							   <HeaderTemplate>
							   	<asp:Table runat="server">
														<asp:TableRow>
											   		<asp:TableCell>Tour Link:</asp:TableCell>
																<asp:TableCell>&nbsp;</asp:TableCell>
											  	</asp:TableRow>
											</asp:Table>
							   </HeaderTemplate>
										
							   <ItemTemplate>
							      <asp:Table runat="server">
														<asp:TableRow>
																<asp:TableCell CssClass="linkwidth">
																	<asp:HyperLink id="hlTourUrl" Target="_blank" runat="server" />
																</asp:TableCell>
																<asp:TableCell CssClass="editbackground">
																	<asp:LinkButton id="lbtnEdit" Text="Edit" CommandName="edit" CausesValidation="False" runat="server" /><br /><br />
																	<asp:LinkButton id="lbtnDelete" Text="Delete" CommandName="delete" CausesValidation="False" runat="server" />
																</asp:TableCell>
											  	</asp:TableRow>
													</asp:Table>
							   </ItemTemplate>
										
							   <EditItemTemplate>
							      <asp:Table runat="server">
														<asp:TableRow>
															<asp:TableCell CssClass="commentswidth">
																<asp:TextBox id="txtTourUrl" MaxLength="80" Columns="80" runat="server" />
															</asp:TableCell>
																<asp:TableCell CssClass="editbackground">
																	<asp:LinkButton id="lbtnUpdate" Text="OK" CommandName="update" CausesValidation="False" runat="server" /><br /><br />
																	<asp:LinkButton id="lbtnCancel" Text="Cancel" CommandName="cancel" CausesValidation="False" runat="server" />
																</asp:TableCell>
											  	</asp:TableRow>
													</asp:Table>
							   </EditItemTemplate>
										
							</asp:DataList>
							
				</td>
</tr>
</table>

	</form>
	</body>
</html>
