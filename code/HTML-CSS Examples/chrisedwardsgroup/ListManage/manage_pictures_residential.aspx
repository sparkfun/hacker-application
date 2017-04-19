<%@ Page Language="C#" Debug="False" Inherits="ChrisEdwardsGroup.ListManage.ManagePictures" %>

<%@ Register Tagprefix="Top" Tagname="Bar" Src="includes/topbar.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<html>
	<head>
		<title>Chris Edwards Group - Listings Management - Manage Pictures: Residential Property</title>
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
						<td class="pagetitle" colspan="3">Manage Pictures: Residential Property</td>
					</tr>
					<tr>
						<td class="quicklisting">Quick Info:</td>
						<td class="quicklistinginfo">MLS: <asp:Label id="lblMLS" runat="server" /></td>
						<td class="quicklistinginfo">Total Pictures: <asp:Label id="lblTotalPics" runat="server" /></td>
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
										<td class="imagehead"><a href="manage_residential.aspx"><img src="graphics/manage_bar_pic_01.gif" alt="Manage Property" width="132" height="24" border="0"></a><a href="manage_pictures_residential.aspx"><img src="graphics/manage_bar_pic_02.gif" alt="Manage Pictures" width="145" height="24" border="0"></a><a href="manage_tours_residential.aspx"><img src="graphics/manage_bar_pic_03.gif" alt="Manage Tours" width="132" height="24" border="0"></a></td>
									</tr>
								</tbody>
							</table>
							
							<table class="managephotos" cellpadding="5">
								<thead>
									<tr>
										<th class="sectionheadpics" colspan="3">Add New Pictures</th>
									</tr>
								</thead>
								<tbody>
									<tr>
										<td>Select File to Upload:</td>
										<td><input id="iptFile" type="file" size="35" accept="image/pjpeg,image/gif" runat="server" /></td>
										<td>
											<input type="button" id="Button1" value="Upload Picture" OnServerClick="UploadPicture_Click" runat="server" />
										</td>
									</tr>
								</tbody>
							</table>
													
							<table class="managephotos" cellpadding="5">
								<thead>
									<tr>
										<th class="sectionheadpics" colspan="2">Manage Pictures</th>
									</tr>
								</thead>
								<tbody>
									<tr>
										<td>
										
											<asp:DataList id="dtlManagePictures"
							     CellPadding="5"
												CssClass="managepictures"
							     DataKeyField="PictureID"
							     ExtractTemplateRows="True"
							     GridLines="Both"
							     ShowFooter="False"
							     ShowHeader="True"
							     OnCancelCommand="ManagePics_CancelCommand"
												OnDeleteCommand="ManagePics_DeleteCommand"
							     OnEditCommand="ManagePics_EditCommand"
							     OnItemCreated="ManagePics_ItemCreated"
							     OnUpdateCommand="ManagePics_UpdateCommand"
							     runat="server">
							
							   <AlternatingItemStyle BackColor="LightGoldenrodYellow" />
										<EditItemStyle BackColor="Khaki" />
							  	<HeaderStyle CssClass="sectionheadpics" />
							
							   <HeaderTemplate>
							   	<asp:Table runat="server">
														<asp:TableRow>
											   		<asp:TableCell CssClass="picturewidth">Thumbnail Picture:</asp:TableCell>
																<asp:TableCell CssClass="commentswidth">Picture Comments:</asp:TableCell>
																<asp:TableCell CssClass="defaultpicwidth">Main Picture?:</asp:TableCell>
																<asp:TableCell>&nbsp;</asp:TableCell>
											  	</asp:TableRow>
											</asp:Table>
							   </HeaderTemplate>
										
							   <ItemTemplate>
							      <asp:Table runat="server">
														<asp:TableRow>
											   		<asp:TableCell CssClass="picturewidth">
																	<asp:Image id="imgPicturePathThumb" runat="server" />
																</asp:TableCell>
																<asp:TableCell id="tcPictureComments" CssClass="commentswidth" />
																<asp:TableCell id="tcDefaultPicture" CssClass="defaultpicwidth" />
																<asp:TableCell CssClass="editbackground">
																	<asp:LinkButton id="lbtnEdit" Text="Edit" CommandName="edit" runat="server" /><br><br>
																	<asp:LinkButton id="lbtnDelete" Text="Delete" CommandName="delete" runat="server" />
																</asp:TableCell>
											  	</asp:TableRow>
													</asp:Table>
							   </ItemTemplate>
										
							   <AlternatingItemTemplate>
							   	<asp:Table runat="server">
												<asp:TableRow>
									   		<asp:TableCell CssClass="picturewidth">
															<asp:Image id="imgPicturePathThumb" runat="server" />
														</asp:TableCell>
														<asp:TableCell id="tcPictureComments" CssClass="commentswidth" />
														<asp:TableCell id="tcDefaultPicture" CssClass="defaultpicwidth" />
														<asp:TableCell CssClass="editbackground">
															<asp:LinkButton id="lbtnEdit" Text="Edit" CommandName="edit" runat="server" /><br><br>
															<asp:LinkButton id="lbtnDelete" Text="Delete" CommandName="delete" runat="server" />
														</asp:TableCell>
									  	</asp:TableRow>
													</asp:Table>
							   </AlternatingItemTemplate>
										
							   <EditItemTemplate>
							      <asp:Table runat="server">
														<asp:TableRow>
											   	<asp:TableCell CssClass="picturewidth">
																<asp:Image id="imgPicturePathThumb" runat="server" />
															</asp:TableCell>
															<asp:TableCell CssClass="commentswidth">
																<asp:TextBox id="txtPictureComments" Columns="55" Rows="4" TextMode="Multiline" CssClass="textarea8pt" runat="server" />
															</asp:TableCell>
															<asp:TableCell CssClass="defaultpicwidth">
																	<asp:CheckBox id="chbDefaultPicture" runat="server" />
																</asp:TableCell>
																<asp:TableCell CssClass="editbackground">
																	<asp:LinkButton id="lbtnUpdate" Text="OK" CommandName="update" runat="server" />&nbsp;<asp:LinkButton id="lbtnCancel" Text="Cancel" CommandName="cancel" runat="server" />
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
