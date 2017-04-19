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
	/// Summary description for editstaff
	/// </summary>
	public class editstaff : SkinBase
	{
		
		int StaffID;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			StaffID = 0;

			if(Common.QueryString("StaffID").Length != 0 && Common.QueryString("StaffID") != "0") 
			{
				Editing = true;
				StaffID = Localization.ParseUSInt(Common.QueryString("StaffID"));
			} 
			else 
			{
				Editing = false;
			}
			
			IDataReader rs;
			
			int N = 0;
			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{

				if(Editing)
				{
					// see if this staff already exists:
					rs = DB.GetRS("select count(name) as N from staff  " + DB.GetNoLock() + " where StaffID<>" + StaffID.ToString() + " and deleted=0 and lower(Name)=" + DB.SQuote(Common.Form("Name").ToLower()));
					rs.Read();
					N = DB.RSFieldInt(rs,"N");
					rs.Close();
					if(N != 0)
					{
						ErrorMsg = "<p><b><font color=red>ERROR:<br><br></font><blockquote>There is already another staff member with that name. Please <a href=\"javascript:history.back(-1);\">go back</a>.</b></blockquote></p>";
					}
				}
				else
				{
					// see if this name is already there:
					rs = DB.GetRS("select count(name) as N from Staff  " + DB.GetNoLock() + " where deleted=0 and lower(Name)=" + DB.SQuote(Common.Form("Name").ToLower()));
					rs.Read();
					N = DB.RSFieldInt(rs,"N");
					rs.Close();
					if(N != 0)
					{
						ErrorMsg = "<p><b><font color=red>ERROR:<br><br></font><blockquote>There is already another staff member with that name. Please <a href=\"javascript:history.back(-1);\">go back</a>.</b></blockquote></p>";
					}
				}

				if(ErrorMsg.Length == 0)
				{
					try
					{
						StringBuilder sql = new StringBuilder(2500);
						if(!Editing)
						{
							// ok to add them:
							String NewGUID = DB.GetNewGUID();
							sql.Append("insert into staff(StaffGUID,Name,Published,Title,Phone,FAX,Email,LastUpdatedBy) values(");
							sql.Append(DB.SQuote(NewGUID) + ",");
							sql.Append(DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							sql.Append(Common.Form("Published") + ",");
							if(Common.Form("Title").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("Title")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("Phone").Length != 0)
							{
								sql.Append(DB.SQuote(Common.MakeProperPhoneFormat(Common.Form("Phone"))) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("FAX").Length != 0)
							{
								sql.Append(DB.SQuote(Common.MakeProperPhoneFormat(Common.Form("FAX"))) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("EMail").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("EMail")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							sql.Append(thisCustomer._customerID);
							sql.Append(")");
							DB.ExecuteSQL(sql.ToString());

							rs = DB.GetRS("select StaffID from staff  " + DB.GetNoLock() + " where deleted=0 and StaffGUID=" + DB.SQuote(NewGUID));
							rs.Read();
							StaffID = DB.RSFieldInt(rs,"StaffID");
							Editing = true;
							rs.Close();
							DataUpdated = true;

							sql.Remove(0,sql.Length);
							sql.Append("update staff set ");
							if(Common.Form("Bio").Length != 0)
							{
								sql.Append("Bio=" + DB.SQuote(Common.Form("Bio")));
							}
							else
							{
								sql.Append("Bio=NULL");
							}
							sql.Append(" where StaffID=" + StaffID.ToString());
							DB.ExecuteSQL(sql.ToString());
						}
						else
						{
							// ok to update:
							sql.Append("update staff set ");
							sql.Append("Name=" + DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							sql.Append("Published=" + Common.Form("Published") + ",");
							if(Common.Form("Title").Length != 0)
							{
								sql.Append("Title=" + DB.SQuote(Common.Form("Title")) + ",");
							}
							else
							{
								sql.Append("Title=NULL,");
							}
							if(Common.Form("Phone").Length != 0)
							{
								sql.Append("Phone=" + DB.SQuote(Common.MakeProperPhoneFormat(Common.Form("Phone"))) + ",");
							}
							else
							{
								sql.Append("Phone=NULL,");
							}
							if(Common.Form("FAX").Length != 0)
							{
								sql.Append("FAX=" + DB.SQuote(Common.MakeProperPhoneFormat(Common.Form("FAX"))) + ",");
							}
							else
							{
								sql.Append("FAX=NULL,");
							}
							if(Common.Form("Email").Length != 0)
							{
								sql.Append("Email=" + DB.SQuote(Common.Left(Common.Form("EMail"),100)) + ",");
							}
							else
							{
								sql.Append("Email=NULL,");
							}
							sql.Append("LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
							sql.Append("LastUpdatedBy=" + thisCustomer._customerID.ToString() + " ");
							sql.Append("where StaffID=" + StaffID.ToString());
							DB.ExecuteSQL(sql.ToString());


							sql.Remove(0,sql.Length);
							sql.Append("update staff set ");
							if(Common.Form("Bio").Length != 0)
							{
								sql.Append("Bio=" + DB.SQuote(Common.Form("Bio")));
							}
							else
							{
								sql.Append("Bio=NULL");
							}
							sql.Append(" where StaffID=" + StaffID.ToString());
							DB.ExecuteSQL(sql.ToString());


							DataUpdated = true;
							Editing = true;
						}
					}
					catch(Exception ex)
					{
						ErrorMsg = "<p><b>ERROR: " + Common.GetExceptionDetail(ex,"<br>") + "<br><br></b></p>";
					}

					// handle image uploaded:
					try
					{
						String Image1 = String.Empty;
						HttpPostedFile Image1File = Request.Files["Image1"];
						if(Image1File.ContentLength != 0)
						{
							// delete any current image file first
							try
							{
								System.IO.File.Delete(Common.GetImagePath("Staff","icon",true) + StaffID.ToString() + ".jpg");
								System.IO.File.Delete(Common.GetImagePath("Staff","icon",true) + StaffID.ToString() + ".gif");
								System.IO.File.Delete(Common.GetImagePath("Staff","icon",true) + StaffID.ToString() + ".png");
							}
							catch
							{}

							String s = Image1File.ContentType;
							switch(Image1File.ContentType)
							{
								case "image/gif":
									Image1 = Common.GetImagePath("Staff","icon",true) + StaffID.ToString() + ".gif";
									Image1File.SaveAs(Image1);
									break;
								case "image/x-png":
									Image1 = Common.GetImagePath("Staff","icon",true) + StaffID.ToString() + ".png";
									Image1File.SaveAs(Image1);
									break;
								case "image/jpeg":
								case "image/pjpeg":
									Image1 = Common.GetImagePath("Staff","icon",true) + StaffID.ToString() + ".jpg";
									Image1File.SaveAs(Image1);
									break;
							}
						}

						String Image2 = String.Empty;
						HttpPostedFile Image2File = Request.Files["Image2"];
						if(Image2File.ContentLength != 0)
						{
							// delete any current image file first
							try
							{
								System.IO.File.Delete(Common.GetImagePath("Staff","medium",true) + StaffID.ToString() + ".jpg");
								System.IO.File.Delete(Common.GetImagePath("Staff","medium",true) + StaffID.ToString() + ".gif");
								System.IO.File.Delete(Common.GetImagePath("Staff","medium",true) + StaffID.ToString() + ".png");
							}
							catch
							{}

							String s = Image2File.ContentType;
							switch(Image2File.ContentType)
							{
								case "image/gif":
									Image2 = Common.GetImagePath("Staff","medium",true) + StaffID.ToString() + ".gif";
									Image2File.SaveAs(Image2);
									break;
								case "image/x-png":
									Image2 = Common.GetImagePath("Staff","medium",true) + StaffID.ToString() + ".png";
									Image2File.SaveAs(Image2);
									break;
								case "image/jpeg":
								case "image/pjpeg":
									Image2 = Common.GetImagePath("Staff","medium",true) + StaffID.ToString() + ".jpg";
									Image2File.SaveAs(Image2);
									break;
							}
						}
					}
					catch(Exception ex)
					{
						ErrorMsg = Common.GetExceptionDetail(ex,"<br>");
					}
				}

			}
			SectionTitle = "<a href=\"staff.aspx\">Staff</a> - Manage Staff";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			IDataReader rs = DB.GetRS("select * from Staff  " + DB.GetNoLock() + " where StaffID=" + StaffID.ToString());
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
					writer.Write("<b>Editing Staff: " + DB.RSField(rs,"Name") + " (ID=" + DB.RSFieldInt(rs,"StaffID").ToString() + ")<br><br></b>\n");
				}
				else
				{
					writer.Write("<b>Adding New Staff:<br><br></b>\n");
				}

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function Form_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("submitonce(theForm);\n");
				//			writer.Write("  if (theForm.State.selectedIndex < 1)\n");
				//			writer.Write("  {\n");
				//			writer.Write("    	alert(\"Please select the state.\");\n");
				//			writer.Write("    theForm.State.focus();\n");
				//			writer.Write("    	return (false);\n");
				//			writer.Write("  }\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				writer.Write("<p>Please enter the following information about this staff member. Fields marked with an asterisk (*) are required. All other fields are optional.</p>\n");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\">\n");
				writer.Write("<form enctype=\"multipart/form-data\" action=\"editstaff.aspx?StaffID=" + StaffID.ToString() + "&edit=" + Editing.ToString() + "\" method=\"post\" id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
				writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
				
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
				writer.Write("                <td width=\"100%\" colspan=\"2\" align=\"left\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Name:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"Name\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Name")) , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"Name_vldt\" value=\"[req][blankalert=Please enter the staff name]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Published:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"Published\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"Published") , " checked " , " checked ") , " checked ") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"Published\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"Published") , "" , " checked ") , "") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Title:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"Title\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Title")) , "") + "\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");


				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">E-Mail Address:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"35\" name=\"EMail\" value=\"" + Common.IIF(Editing , DB.RSField(rs,"EMail") , Common.QueryString("Email")) + "\">\n");
				writer.Write("                    <input type=\"hidden\" name=\"EMail_vldt\" value=\"[invalidalert=Please enter a valid e-mail address]\">\n");
				writer.Write("               	</td>\n");
				writer.Write("              </tr>\n");
				writer.Write("              </tr>\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">Phone:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"35\" size=\"35\" name=\"Phone\" value=\"" + Common.IIF(Editing , Common.GetPhoneDisplayFormat(DB.RSField(rs,"Phone")) , "") + "\">&nbsp;&nbsp;<small>(optional, including area code)</small>\n");
				writer.Write("                    <input type=\"hidden\" name=\"Phone_vldt\" value=\"[invalidalert=Please enter a valid phone number with areacode, e.g. (480) 555-1212]\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">Fax:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"35\" size=\"35\" name=\"FAX\" value=\"" + Common.IIF(Editing , Common.GetPhoneDisplayFormat(DB.RSField(rs,"Fax")) , "") + "\">&nbsp;&nbsp;<small>(optional, including area code)</small>\n");
				writer.Write("                    <input type=\"hidden\" name=\"FAX_vldt\" value=\"[invalidalert=Please enter a valid FAX number with areacode, e.g. (480) 555-1212]\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("  <tr>\n");
				writer.Write("    <td valign=\"top\" align=\"right\">Icon:\n");
				writer.Write("</td>\n");
				writer.Write("    <td valign=\"top\" align=\"left\">");
				writer.Write("    <input type=\"file\" name=\"Image1\" size=\"50\" value=\"" + Common.IIF(Editing , "" , "") + "\">\n");
				String Image1URL = Common.LookupImage("Staff",StaffID,"icon",_siteID);
				if(Editing && Image1URL.Length != 0)
				{
					writer.Write("<a href=\"javascript:void(0);\" onClick=\"DeleteIcon('icon','Pic1');\">Click here</a> to delete the current image<br>\n");
					writer.Write("<br><img id=\"Pic1\" name=\"Pic1\" border=\"0\" src=\"" + Image1URL + "\">\n");
				}
				writer.Write("</td>\n");
				writer.Write(" </tr>\n");

				writer.Write("  <tr>\n");
				writer.Write("    <td valign=\"top\" align=\"right\">Medium Pic:\n");
				writer.Write("</td>\n");
				writer.Write("    <td valign=\"top\" align=\"left\">");
				writer.Write("    <input type=\"file\" name=\"Image2\" size=\"50\" value=\"" + Common.IIF(Editing , "" , "") + "\">\n");
				String Image2URL = Common.LookupImage("Staff",StaffID,"medium",_siteID);
				if(Image2URL.Length != 0)
				{
					writer.Write("<a href=\"javascript:void(0);\" onClick=\"DeleteIcon('medium','Pic2');\">Click here</a> to delete the current image<br>\n");
					writer.Write("<br><img id=\"Pic2\" name=\"Pic2\" border=\"0\" src=\"" + Image2URL + "\">\n");
				}
				writer.Write("</td>\n");
				writer.Write(" </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Bio (HTML OK):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<textarea cols=\"" + Common.AppConfig("Admin_TextareaWidth") + "\" rows=\"" + Common.AppConfig("Admin_TextareaHeight") + "\" name=\"Bio\">" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Bio")) , "") + "</textarea>\n");
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

				writer.Write(Common.GenerateHtmlEditor("Bio"));

				writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
				writer.Write("function DeleteIcon()\n");
				writer.Write("{\n");
				writer.Write("window.open('deleteicon.aspx?StaffID=" + StaffID.ToString() + "&FormImageName=MfgPic',\"AspDotNetStorefrontAdmin_ML\",\"height=250,width=440,top=10,left=20,status=no,toolbar=no,menubar=no,scrollbars=yes,location=no\")\n");
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
