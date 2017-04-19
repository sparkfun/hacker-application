// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Web;
using System.Collections;
using System.IO;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for editgallery
	/// </summary>
	public class editgallery : SkinBase
	{
		
		int GalleryID;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			GalleryID = 0;

			if(Common.QueryString("GalleryID").Length != 0 && Common.QueryString("GalleryID") != "0") 
			{
				Editing = true;
				GalleryID = Localization.ParseUSInt(Common.QueryString("GalleryID"));
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
					// see if this gallery already exists:
					rs = DB.GetRS("select count(name) as N from gallery  " + DB.GetNoLock() + " where GalleryID<>" + GalleryID.ToString() + " and deleted=0 and lower(Name)=" + DB.SQuote(Common.Form("Name").ToLower()));
					rs.Read();
					N = DB.RSFieldInt(rs,"N");
					rs.Close();
					if(N != 0)
					{
						ErrorMsg = "<p><b><font color=red>ERROR:<br><br></font><blockquote>There is already another gallery with that name. Please <a href=\"javascript:history.back(-1);\">go back</a>.</b></blockquote></p>";
					}
				}
				else
				{
					// see if this name is already there:
					rs = DB.GetRS("select count(name) as N from Gallery  " + DB.GetNoLock() + " where deleted=0 and lower(Name)=" + DB.SQuote(Common.Form("Name").ToLower()));
					rs.Read();
					N = DB.RSFieldInt(rs,"N");
					rs.Close();
					if(N != 0)
					{
						ErrorMsg = "<p><b><font color=red>ERROR:<br><br></font><blockquote>There is already another gallery with that name. Please <a href=\"javascript:history.back(-1);\">go back</a>.</b></blockquote></p>";
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
							sql.Append("insert into gallery(GalleryGUID,Name,DirName,Description,LastUpdatedBy) values(");
							sql.Append(DB.SQuote(NewGUID) + ",");
							sql.Append(DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							sql.Append(DB.SQuote(Common.Left(Common.Form("DirName"),100)) + ",");
							if(Common.Form("Description").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("Description")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							sql.Append(thisCustomer._customerID);
							sql.Append(")");
							DB.ExecuteSQL(sql.ToString());

							rs = DB.GetRS("select GalleryID from gallery  " + DB.GetNoLock() + " where deleted=0 and GalleryGUID=" + DB.SQuote(NewGUID));
							rs.Read();
							GalleryID = DB.RSFieldInt(rs,"GalleryID");
							Editing = true;
							rs.Close();
							DataUpdated = true;
							// BUILD THE GALLERY DIRECTORY:
							String SFP = HttpContext.Current.Server.MapPath("../images/spacer.gif").Replace("images\\spacer.gif","images\\gallery") + "\\" + Common.GetGalleryDir(GalleryID);
							try
							{
								if(!Directory.Exists(SFP))
								{
									DirectoryInfo di = Directory.CreateDirectory(SFP);
								}
							}
							catch {}
						}
						else
						{
							// ok to update:
							sql.Append("update gallery set ");
							sql.Append("Name=" + DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							if(Common.Form("Description").Length != 0)
							{
								sql.Append("Description=" + DB.SQuote(Common.Form("Description")) + ",");
							}
							else
							{
								sql.Append("Description=NULL,");
							}
							sql.Append("LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
							sql.Append("LastUpdatedBy=" + thisCustomer._customerID.ToString() + " ");
							sql.Append("where GalleryID=" + GalleryID.ToString());
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
								System.IO.File.Delete(Common.GetImagePath("Gallery","",true) + GalleryID.ToString() + ".jpg");
								System.IO.File.Delete(Common.GetImagePath("Gallery","",true) + GalleryID.ToString() + ".gif");
								System.IO.File.Delete(Common.GetImagePath("Gallery","",true) + GalleryID.ToString() + ".png");
							}
							catch
							{}

							String s = Image1File.ContentType;
							switch(Image1File.ContentType)
							{
								case "image/gif":
									Image1 = Common.GetImagePath("Gallery","",true) + GalleryID.ToString() + ".gif";
									Image1File.SaveAs(Image1);
									break;
								case "image/x-png":
									Image1 = Common.GetImagePath("Gallery","",true) + GalleryID.ToString() + ".png";
									Image1File.SaveAs(Image1);
									break;
								case "image/jpeg":
								case "image/pjpeg":
									Image1 = Common.GetImagePath("Gallery","",true) + GalleryID.ToString() + ".jpg";
									Image1File.SaveAs(Image1);
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
			SectionTitle = "<a href=\"galleries.aspx\">Galleries</a> - Manage Galleries" + Common.IIF(DataUpdated , " (Updated)" , "");
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			IDataReader rs = DB.GetRS("select * from Gallery  " + DB.GetNoLock() + " where GalleryID=" + GalleryID.ToString());
			if(rs.Read())
			{
				Editing = true;
			}
			
			if(ErrorMsg.Length != 0)
			{
				writer.Write("<p><b><font color=red>" + ErrorMsg + "</font></b></p>\n");
			}


			if(ErrorMsg.Length == 0)
			{

				if(Editing)
				{
					writer.Write("<b>Editing Gallery: " + DB.RSField(rs,"Name") + " (ID=" + DB.RSFieldInt(rs,"GalleryID").ToString() + ")<br><br></b>\n");
				}
				else
				{
					writer.Write("<b>Adding New Gallery:<br><br></b>\n");
				}

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function Form_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("submitonce(theForm);\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");

				if(!Editing)
				{
					writer.Write("function MakeSafeName(theForm)\n");
					writer.Write("{\n");
					writer.Write("var cn = document.Form1.Name.value;\n");
					writer.Write("var cn2 = stripCharsNotInBag(cn,\"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_1234567890\");\n");
					writer.Write("document.Form1.DirName.value = cn2;\n");
					writer.Write("document.Form1.DirName.focus();\n");
					writer.Write("return (true);\n");
					writer.Write("}\n");
				}

				writer.Write("</script>\n");

				writer.Write("<p>Please enter the following information about this gallery (brand). Fields marked with an asterisk (*) are required. All other fields are optional.</p>\n");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\">\n");
				writer.Write("<form enctype=\"multipart/form-data\" action=\"editgallery.aspx?GalleryID=" + GalleryID.ToString() + "&edit=" + Editing.ToString() + "\" method=\"post\" id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
				writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"100%\" colspan=\"2\" align=\"left\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Gallery Name:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"Name\" " + Common.IIF(Editing , "" , "onBlur=\"MakeSafeName(this);\"") + " value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Name")) , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"Name_vldt\" value=\"[req][blankalert=Please enter the gallery name]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Gallery Directory:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				if(!Editing)
				{
					writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"DirName\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"DirName")) , "") + "\">\n");
					writer.Write("                	<input type=\"hidden\" name=\"DirName_vldt\" value=\"[req][blankalert=Please enter the gallery directory. This MUST be a valid Windows directory name, a-z, A-Z, 9-0, and _ characters ONLY!]\">\n");
				}
				else
				{
					writer.Write(DB.RSField(rs,"DirName"));
				}
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");


				writer.Write("    <td valign=\"top\" align=\"right\">Icon:\n");
				writer.Write("</td>\n");
				writer.Write("    <td valign=\"top\" align=\"left\">");
				writer.Write("    <input type=\"file\" name=\"Image1\" size=\"30\" value=\"" + Common.IIF(Editing , "" , "") + "\">\n");
				String Image1URL = Common.LookupImage("Gallery",GalleryID,"",_siteID);
				if(Image1URL.Length != 0)
				{
					writer.Write("<a href=\"javascript:void(0);\" onClick=\"DeleteIcon();\">Click here</a> to delete the current image<br>\n");
					writer.Write("<br><img id=\"GalleryPic\" name=\"GalleryPic\" border=\"0\" src=\"" + Image1URL + "?" + Common.GetRandomNumber(1,1000000).ToString() + "\">\n");
				}
				writer.Write("</td>\n");
				writer.Write(" </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Description (HTML OK):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<textarea style=\"height: 60em; width: 100%;\" cols=\"" + Common.AppConfig("Admin_TextareaWidth") + "\" rows=\"" + Common.AppConfig("Admin_TextareaHeightl") + "\" id=\"Description\" name=\"Description\">" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Description")) , "") + "</textarea>\n");
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

				writer.Write(Common.GenerateHtmlEditor("Description"));

				writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
				writer.Write("function DeleteIcon()\n");
				writer.Write("{\n");
				writer.Write("window.open('deleteicon.aspx?GalleryID=" + GalleryID.ToString() + "&FormImageName=GalleryPic',\"AspDotNetStorefrontAdmin_ML\",\"height=250,width=440,top=10,left=20,status=no,toolbar=no,menubar=no,scrollbars=yes,location=no\")\n");
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
