// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Web;
using System.IO;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for editcategory
	/// </summary>
	public class editcategory : SkinBase
	{
		
		int CategoryID;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			CategoryID = 0;
			if(Common.QueryString("CategoryID").Length != 0 && Common.QueryString("CategoryID") != "0") 
			{
				Editing = true;
				CategoryID = Common.QueryStringUSInt("CategoryID");
			} 
			else 
			{
				Editing = false;
			}

			IDataReader rs;
			
			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				if(ErrorMsg.Length == 0)
				{
//					try
//					{
						StringBuilder sql = new StringBuilder(2500);
						if(!Editing)
						{
							// ok to add them:
							String NewGUID = DB.GetNewGUID();
							sql.Append("insert into category(CategoryGUID,Name,ParentCategoryID,Summary,Description,SEKeywords,SEDescription,SETitle,SENoScript,SEAltText,Published,ShowInProductBrowser,AllowSectionFiltering,AllowManufacturerFiltering,AllowProductTypeFiltering,ColWidth,CategoryDisplayFormatID,SortByLooks,QuantityDiscountID,LastUpdatedBy) values(");
							sql.Append(DB.SQuote(NewGUID) + ",");
							sql.Append(DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							sql.Append(Common.Form("ParentCategoryID") + ",");
							sql.Append(DB.SQuote(Common.Form("Summary")) + ",");
							sql.Append(DB.SQuote(Common.Form("Description")) + ",");
							if(Common.Form("SEKeywords").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("SEKeywords")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("SEDescription").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("SEDescription")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("SETitle").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("SETitle")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("SENoScript").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("SENoScript")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("SEAltText").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("SEAltText")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							sql.Append(Common.Form("Published") + ",");
							sql.Append(Common.Form("ShowInProductBrowser") + ",");
							sql.Append(Common.Form("AllowSectionFiltering") + ",");
							sql.Append(Common.Form("AllowManufacturerFiltering") + ",");
							sql.Append(Common.Form("AllowProductTypeFiltering") + ",");
							sql.Append(Common.IIF(Common.Form("ColWidth").Length == 0 , Common.AppConfig("Admin_DefaultCategoryColWidth") , Common.Form("ColWidth")) + ",");
							sql.Append(Common.Form("CategoryDisplayFormatID") + ",");
							sql.Append(Common.Form("SortByLooks") + ",");
							sql.Append(Common.Form("QuantityDiscountID") + ",");
							sql.Append(thisCustomer._customerID.ToString());
							sql.Append(")");
							DB.ExecuteSQL(sql.ToString());

							rs = DB.GetRS("select CategoryID from category  " + DB.GetNoLock() + " where deleted=0 and CategoryGUID=" + DB.SQuote(NewGUID));
							rs.Read();
							CategoryID = DB.RSFieldInt(rs,"CategoryID");
							Editing = true;
							rs.Close();
							DataUpdated = true;
						}
						else
						{
							// ok to update:
							sql.Append("update category set ");
							sql.Append("Name=" + DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							sql.Append("ParentCategoryID=" + Common.Form("ParentCategoryID") + ",");
							sql.Append("Summary=" + DB.SQuote(Common.Form("Summary")) + ",");
							sql.Append("Description=" + DB.SQuote(Common.Form("Description")) + ",");
							if(Common.Form("SEKeywords").Length != 0)
							{
								sql.Append("SEKeywords=" + DB.SQuote(Common.Form("SEKeywords")) + ",");
							}
							else
							{
								sql.Append("SEKeywords=NULL,");
							}
							if(Common.Form("SEDescription").Length != 0)
							{
								sql.Append("SEDescription=" + DB.SQuote(Common.Form("SEDescription")) + ",");
							}
							else
							{
								sql.Append("SEDescription=NULL,");
							}
							if(Common.Form("SETitle").Length != 0)
							{
								sql.Append("SETitle=" + DB.SQuote(Common.Form("SETitle")) + ",");
							}
							else
							{
								sql.Append("SETitle=NULL,");
							}
							if(Common.Form("SENoScript").Length != 0)
							{
								sql.Append("SENoScript=" + DB.SQuote(Common.Form("SENoScript")) + ",");
							}
							else
							{
								sql.Append("SENoScript=NULL,");
							}
							if(Common.Form("SEAltText").Length != 0)
							{
								sql.Append("SEAltText=" + DB.SQuote(Common.Form("SEAltText")) + ",");
							}
							else
							{
								sql.Append("SEAltText=NULL,");
							}
							sql.Append("Published=" + Common.Form("Published") + ",");
							sql.Append("ShowInProductBrowser=" + Common.Form("ShowInProductBrowser") + ",");
							sql.Append("AllowSectionFiltering=" + Common.Form("AllowSectionFiltering") + ",");
							sql.Append("AllowManufacturerFiltering=" + Common.Form("AllowManufacturerFiltering") + ",");
							sql.Append("AllowProductTypeFiltering=" + Common.Form("AllowProductTypeFiltering") + ",");
							sql.Append("ColWidth=" + Common.IIF(Common.Form("ColWidth").Length == 0 , Common.AppConfig("Admin_DefaultCategoryColWidth") , Common.Form("ColWidth")) + ",");
							sql.Append("CategoryDisplayFormatID=" + Common.Form("CategoryDisplayFormatID") + ",");
							sql.Append("SortByLooks=" + Common.Form("SortByLooks") + ",");
							sql.Append("QuantityDiscountID=" + Common.Form("QuantityDiscountID") + ",");
							sql.Append("LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
							sql.Append("LastUpdatedBy=" + thisCustomer._customerID.ToString() + " ");
							sql.Append("where CategoryID=" + CategoryID.ToString());
							DB.ExecuteSQL(sql.ToString());
							DataUpdated = true;
							Editing = true;
						}
//					}
//					catch(Exception ex)
//					{
//						ErrorMsg = "<p><b>ERROR: " + Common.GetExceptionDetail(ex,"<br>") + "<br><br></b></p>";
//					}
//
//					// handle image uploaded:
//					try
//					{
						String Image1 = String.Empty;
						HttpPostedFile Image1File = Request.Files["Image1"];
						if(Image1File.ContentLength != 0)
						{
							// delete any current image file first
							try
							{
								System.IO.File.Delete(Common.GetImagePath("Category","",true) + CategoryID.ToString() + ".jpg");
								System.IO.File.Delete(Common.GetImagePath("Category","",true) + CategoryID.ToString() + ".gif");
								System.IO.File.Delete(Common.GetImagePath("Category","",true) + CategoryID.ToString() + ".png");
							}
							catch
							{}

							String s = Image1File.ContentType;
							switch(Image1File.ContentType)
							{
								case "image/gif":
									Image1 = Common.GetImagePath("Category","",true) + CategoryID.ToString() + ".gif";
									Image1File.SaveAs(Image1);
									break;
								case "image/x-png":
									Image1 = Common.GetImagePath("Category","",true) + CategoryID.ToString() + ".png";
									Image1File.SaveAs(Image1);
									break;
								case "image/jpeg":
								case "image/pjpeg":
									Image1 = Common.GetImagePath("Category","",true) + CategoryID.ToString() + ".jpg";
									Image1File.SaveAs(Image1);
									break;
							}
						}
//					}
//					catch(Exception ex)
//					{
//						ErrorMsg = Common.GetExceptionDetail(ex,"<br>");
//					}
				}

			}
			SectionTitle = "<a href=\"categories.aspx\">" + Common.AppConfig("CategoryPromptPlural") + "</a> - Manage " + Common.AppConfig("CategoryPromptPlural");
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			IDataReader rs = DB.GetRS("select * from Category  " + DB.GetNoLock() + " where CategoryID=" + CategoryID.ToString());
			if(rs.Read())
			{
				Editing = true;
			}
			
			if(ErrorMsg.Length != 0)
			{
				writer.Write("<p><b><font color=red>" + ErrorMsg + "</font></b></p>\n");
			}
			if(DataUpdated)
			{
				writer.Write("<p align=\"left\"><b><font color=blue>(UPDATED)</font></b></p>\n");
			}


			if(ErrorMsg.Length == 0)
			{

				if(Editing)
				{
					writer.Write("<b>Editing " + Common.AppConfig("CategoryPromptSingular") + ": " + DB.RSField(rs,"Name") + " (ID=" + DB.RSFieldInt(rs,"CategoryID").ToString() + ")<br><br></b>\n");

					writer.Write("<p align=\"left\"><b>Editing " + Common.AppConfig("CategoryPromptSingular") + ": " + DB.RSField(rs,"Name") + " (ID=" + DB.RSFieldInt(rs,"CategoryID").ToString() + "</b>");
					int NumCategories = DB.GetSqlN("select count(*) as N from category  " + DB.GetNoLock() + " where deleted=0");
					writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;");
					if(NumCategories > 1)
					{
						int PreviousID = Common.GetPreviousCategory(CategoryID);
						writer.Write("<a class=\"ProductNavLink\" href=\"editcategory.aspx?categoryID=" + PreviousID.ToString() + "\">previous " + Common.AppConfig("CategoryPromptSingular").ToLower() + "</a>&nbsp;|&nbsp;");
					}
					writer.Write("<a class=\"ProductNavLink\" href=\"categories.aspx\">up</a>");
					if(NumCategories > 1)
					{
						int NextID = Common.GetNextCategory(CategoryID);
						writer.Write("&nbsp;|&nbsp;<a class=\"ProductNavLink\" href=\"editcategory.aspx?categoryID=" + NextID.ToString() + "\">next " + Common.AppConfig("CategoryPromptSingular").ToLower() + "</a>&nbsp");
					}
					writer.Write("&nbsp;&nbsp;&nbsp;|&nbsp;<a class=\"ProductNavLink\" href=\"products.aspx?categoryid=" + CategoryID.ToString() + "\">Show Products</a>");
					writer.Write("&nbsp;&nbsp;&nbsp;|&nbsp;<a class=\"ProductNavLink\" href=\"displayorder.aspx?categoryid=" + CategoryID.ToString() + "\">Set Product Display Orders</a>");
					writer.Write("<br>");
					writer.Write("</p>\n");
				}
				else
				{
					writer.Write("<b>Adding New " + Common.AppConfig("CategoryPromptSingular") + ":<br><br></b>\n");
				}

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function Form_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("submitonce(theForm);\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				writer.Write("<p>Please enter the following information about this " + Common.AppConfig("CategoryPromptSingular").ToLower() + ". Fields marked with an asterisk (*) are required. All other fields are optional.</p>\n");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\">\n");
				writer.Write("<form enctype=\"multipart/form-data\" action=\"editcategory.aspx?CategoryID=" + CategoryID.ToString() + "&edit=" + Editing.ToString() + "\" method=\"post\" id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
				writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"100%\" colspan=\"2\" align=\"left\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("<tr>\n");
				writer.Write("<td></td><td align=\"left\"><br>\n");
				if(Editing) 
				{
					writer.Write("<input type=\"submit\" value=\"Update\" name=\"submit\">\n");
					writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"reset\" class=\"CPButton\" value=\"Reset\" name=\"reset\">\n");
				} 
				else 
				{
					writer.Write("<input type=\"submit\" value=\"Add New\" name=\"submit\">\n");
				}
				writer.Write("        </td>\n");
				writer.Write("      </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*" + Common.AppConfig("CategoryPromptSingular") + " Name:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"Name\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Name")) , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"Name_vldt\" value=\"[req][blankalert=Please enter the " + Common.AppConfig("CategoryPromptSingular").ToLower() + " name]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\" bgcolor=\"" + Common.IIF(Editing && !DB.RSFieldBool(rs,"Published") , "#Fe5888" , "FFFFFF") + "\">*Published:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\" bgcolor=\"" + Common.IIF(Editing && !DB.RSFieldBool(rs,"Published") , "#Fe5888" , "FFFFFF") + "\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"Published\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"Published") , " checked " , "") , " checked ") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"Published\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"Published") , "" , " checked ") , "") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Show In Product Browser:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\" >\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"ShowInProductBrowser\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"ShowInProductBrowser") , " checked " , "") , " checked ") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"ShowInProductBrowser\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"ShowInProductBrowser") , "" , " checked ") , "") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Allow " + Common.AppConfig("SectionPromptSingular") + " Filtering:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"AllowSectionFiltering\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"AllowSectionFiltering") , " checked " , " checked ") , "") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"AllowSectionFiltering\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"AllowSectionFiltering") , "" , " checked ") , " checked ") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Allow Manufacturer Filtering:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"AllowManufacturerFiltering\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"AllowManufacturerFiltering") , " checked " , " checked ") , "") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"AllowManufacturerFiltering\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"AllowManufacturerFiltering") , "" , " checked ") , " checked ") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Allow ProductType Filtering:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"AllowProductTypeFiltering\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"AllowProductTypeFiltering") , " checked " , "") , "") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"AllowProductTypeFiltering\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"AllowProductTypeFiltering") , "" , " checked ") , " checked ") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">Parent " + Common.AppConfig("CategoryPromptSingular") + ":&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");

				writer.Write("<select size=\"1\" name=\"ParentCategoryID\">\n");
				writer.Write(" <OPTION VALUE=\"0\" " + Common.IIF(!Editing , " selected " , "") + ">--ROOT LEVEL--</option>\n");
				String CatSel = Common.GetCategorySelectList(0,String.Empty,CategoryID);
				// mark current parent:
				CatSel = CatSel.Replace("<option value=\"" + DB.RSFieldInt(rs,"ParentCategoryID").ToString() + "\">","<option value=\"" + DB.RSFieldInt(rs,"ParentCategoryID").ToString() + "\" selected>");
				writer.Write(CatSel);
				writer.Write("</select>\n");
				
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Display Format:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("<select size=\"1\" name=\"CategoryDisplayFormatID\">\n");
				IDataReader rsst = DB.GetRS("select * from CategoryDisplayFormat  " + DB.GetNoLock() + " where deleted=0 order by name");
				while(rsst.Read())
				{
					writer.Write("<option value=\"" + DB.RSFieldInt(rsst,"CategoryDisplayFormatID").ToString() + "\"");
					if(Editing) 
					{
						if(DB.RSFieldInt(rs,"CategoryDisplayFormatID") == DB.RSFieldInt(rsst,"CategoryDisplayFormatID"))
						{
							writer.Write(" selected");
						}
					}
					else
					{
						if(DB.RSFieldInt(rsst,"CategoryDisplayFormatID") == Common.AppConfigUSInt("Admin_DefaultCategoryDisplayFormatID"))
						{
							writer.Write(" selected");
						}
					}
					writer.Write(">" + DB.RSField(rsst,"Name") + "</option>");
				}
				rsst.Close();
				writer.Write("</select>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("<tr valign=\"middle\">\n");
				writer.Write("<td align=\"right\" valign=\"middle\">Quantity Discount Table:&nbsp;&nbsp;</td>\n");
				writer.Write("<td align=\"left\">\n");
				writer.Write("<select size=\"1\" name=\"QuantityDiscountID\">\n");
				writer.Write("<option value=\"0\">None</option>");
				rsst = DB.GetRS("select * from QuantityDiscount  " + DB.GetNoLock() + " order by name");
				while(rsst.Read())
				{
					writer.Write("<option value=\"" + DB.RSFieldInt(rsst,"QuantityDiscountID").ToString() + "\"");
					if(Editing) 
					{
						if(DB.RSFieldInt(rs,"QuantityDiscountID") == DB.RSFieldInt(rsst,"QuantityDiscountID"))
						{
							writer.Write(" selected");
						}
					}
					writer.Write(">" + DB.RSField(rsst,"Name") + "</option>");
				}
				rsst.Close();
				writer.Write("</select>\n");
				writer.Write("</td>\n");
				writer.Write("</tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">Grid Column Width:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"2\" size=\"2\" name=\"ColWidth\" value=\"" + Common.IIF(Editing , DB.RSFieldInt(rs,"ColWidth").ToString() , "4") + "\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">Order Products By Looks:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"SortByLooks\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"SortByLooks") , " checked " , "") , "") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"SortByLooks\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"SortByLooks") , "" , " checked ") , " checked ") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");


				writer.Write("  <tr>\n");
				writer.Write("    <td valign=\"top\" align=\"right\">Icon:\n");
				writer.Write("</td>\n");
				writer.Write("    <td valign=\"top\" align=\"left\">");
				writer.Write("    <input type=\"file\" name=\"Image1\" size=\"30\" value=\"" + Common.IIF(Editing , "" , "") + "\">\n");
				String Image1URL = Common.LookupImage("Category",CategoryID,"",_siteID);
				if(Image1URL.Length != 0)
				{
					writer.Write("<a href=\"javascript:void(0);\" onClick=\"DeleteIcon();\">Click here</a> to delete the current image<br>\n");
					writer.Write("<br><img id=\"CatPic\" name=\"CatPic\" border=\"0\" src=\"" + Image1URL + "?" + Common.GetRandomNumber(1,1000000).ToString() + "\">\n");
				}
				writer.Write("</td>\n");
				writer.Write(" </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Summary (HTML OK):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<textarea style=\"height: 30em; width: 100%;\" cols=\"" + Common.AppConfig("Admin_TextareaWidth") + "\" rows=\"" + Common.AppConfig("Admin_TextareaHeightSmall") + "\" id=\"Summary\" name=\"Summary\">" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Summary")) , "") + "</textarea>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Description (HTML OK):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<textarea style=\"height: 60em; width: 100%;\" cols=\"" + Common.AppConfig("Admin_TextareaWidth") + "\" rows=\"" + Common.AppConfig("Admin_TextareaHeightl") + "\" id=\"Description\" name=\"Description\">" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Description")) , "") + "</textarea>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Search Engine Page Title:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"100\" name=\"SETitle\" value=\"" + Common.IIF(Editing , DB.RSField(rs,"SETitle") , "") + "\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Search Engine Keywords:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"1000\" size=\"100\" name=\"SEKeywords\" value=\"" + Common.IIF(Editing , DB.RSField(rs,"SEKeywords") , "") + "\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Search Engine Description:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"255\" size=\"100\" name=\"SEDescription\" value=\"" + Common.IIF(Editing , DB.RSField(rs,"SEDescription") , "") + "\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Search Engine NoScript:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<textarea name=\"SENoScript\" rows=\"10\" cols=\"50\">" + Common.IIF(Editing , DB.RSField(rs,"SENoScript") , "") + "</textarea>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Search Engine AltText:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"50\" size=\"50\" name=\"SEAltText\" value=\"" + Common.IIF(Editing , DB.RSField(rs,"SEAltText") , "") + "\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("<tr>\n");
				writer.Write("<td></td><td align=\"left\"><br>\n");
				if(Editing) 
				{
					writer.Write("<input type=\"submit\" value=\"Update\" name=\"submit\">\n");
					writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"reset\" class=\"CPButton\" value=\"Reset\" name=\"reset\">\n");
				} 
				else 
				{
					writer.Write("<input type=\"submit\" value=\"Add New\" name=\"submit\">\n");
				}
				writer.Write("        </td>\n");
				writer.Write("      </tr>\n");
				writer.Write("</form>\n");
				writer.Write("  </table>\n");

				writer.Write(Common.GenerateHtmlEditor("Summary"));
				writer.Write(Common.GenerateHtmlEditor("Description"));

				writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
				writer.Write("function DeleteIcon()\n");
				writer.Write("{\n");
				writer.Write("window.open('deleteicon.aspx?CategoryID=" + CategoryID.ToString() + "&FormImageName=CatPic',\"AspDotNetStorefrontAdmin_ML\",\"height=250,width=440,top=10,left=20,status=no,toolbar=no,menubar=no,scrollbars=yes,location=no\")\n");
				writer.Write("}\n");
				writer.Write("</SCRIPT>\n");

			}
			rs.Close();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	}
}
