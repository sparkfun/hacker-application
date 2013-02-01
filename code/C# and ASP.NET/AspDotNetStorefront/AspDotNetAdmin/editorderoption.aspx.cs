// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.AspDotNetStorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.IO;
using System.Web;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for editorderoption
	/// </summary>
	public class editorderoption : SkinBase
	{
		
		int OrderOptionID;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			OrderOptionID = 0;
			
			if(Common.QueryString("OrderOptionID").Length != 0 && Common.QueryString("OrderOptionID") != "0") 
			{
				Editing = true;
				OrderOptionID = Localization.ParseUSInt(Common.QueryString("OrderOptionID"));
			} 
			else 
			{
				Editing = false;
			}

			
			IDataReader rs;
			
			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				//try
				//{
				StringBuilder sql = new StringBuilder(2500);
				decimal Cost = System.Decimal.Zero;
				if(Common.Form("Cost").Length != 0)
				{
					Cost = Common.FormUSDecimal("Cost");
				}
				if(!Editing)
				{
					// ok to add them:
					String NewGUID = DB.GetNewGUID();
					sql.Append("insert into OrderOption(OrderOptionGUID,Name,Description,Cost,LastUpdatedBy) values(");
					sql.Append(DB.SQuote(NewGUID) + ",");
					sql.Append(DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
					if(Common.Form("Description").Length != 0)
					{
						sql.Append(DB.SQuote(Common.Form("Description")) + ",");
					}
					else
					{
						sql.Append("NULL,");
					}
					sql.Append(Common.IIF(Cost != System.Decimal.Zero , Localization.DecimalStringForDB(Cost) , "0.0") + ",");
					sql.Append(thisCustomer._customerID);
					sql.Append(")");
					DB.ExecuteSQL(sql.ToString());

					rs = DB.GetRS("select OrderOptionID from OrderOption  " + DB.GetNoLock() + " where OrderOptionGUID=" + DB.SQuote(NewGUID));
					rs.Read();
					OrderOptionID = DB.RSFieldInt(rs,"OrderOptionID");
					Editing = true;
					rs.Close();
					DataUpdated = true;
				}
				else
				{
					// ok to update:
					sql.Append("update OrderOption set ");
					sql.Append("Name=" + DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
					if(Common.Form("Description").Length != 0)
					{
						sql.Append("Description=" + DB.SQuote(Common.Form("Description")) + ",");
					}
					else
					{
						sql.Append("Description=NULL,");
					}
					sql.Append("Cost=" + Common.IIF(Cost != System.Decimal.Zero , Localization.DecimalStringForDB(Cost) , "0.0") + ",");
					sql.Append("LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
					sql.Append("LastUpdatedBy=" + thisCustomer._customerID.ToString() + " ");
					sql.Append("where OrderOptionID=" + OrderOptionID.ToString());
					DB.ExecuteSQL(sql.ToString());
					DataUpdated = true;
					Editing = true;
				}
				//}
				//catch(Exception ex)
				//{
				//	ErrorMsg = "<p><b>ERROR: " + Common.GetExceptionDetail(ex,"<br>") + "<br><br></b></p>";
				//}
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
							System.IO.File.Delete(Common.GetImagePath("OrderOption","",true) + OrderOptionID.ToString() + ".jpg");
							System.IO.File.Delete(Common.GetImagePath("OrderOption","",true) + OrderOptionID.ToString() + ".gif");
							System.IO.File.Delete(Common.GetImagePath("OrderOption","",true) + OrderOptionID.ToString() + ".png");
						}
						catch
						{}

						String s = Image1File.ContentType;
						switch(Image1File.ContentType)
						{
							case "image/gif":
								Image1 = Common.GetImagePath("OrderOption","",true) + OrderOptionID.ToString() + ".gif";
								Image1File.SaveAs(Image1);
								break;
							case "image/x-png":
								Image1 = Common.GetImagePath("OrderOption","",true) + OrderOptionID.ToString() + ".png";
								Image1File.SaveAs(Image1);
								break;
							case "image/jpeg":
							case "image/pjpeg":
								Image1 = Common.GetImagePath("OrderOption","",true) + OrderOptionID.ToString() + ".jpg";
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
			SectionTitle = "<a href=\"OrderOptions.aspx\">OrderOptions</a> - Manage OrderOptions" + Common.IIF(DataUpdated , " (Updated)" , "");
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			IDataReader rs = DB.GetRS("select * from OrderOption  " + DB.GetNoLock() + " where OrderOptionID=" + OrderOptionID.ToString());
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
					writer.Write("<b>Editing OrderOption: " + DB.RSField(rs,"Name") + " (ID=" + DB.RSFieldInt(rs,"OrderOptionID").ToString() + ")<br><br></b>\n");
				}
				else
				{
					writer.Write("<b>Adding New OrderOption:<br><br></b>\n");
				}

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function Form_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("submitonce(theForm);\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				writer.Write("<p>Please enter the following information about this orderoption. Fields marked with an asterisk (*) are required. All other fields are optional.</p>\n");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\">\n");
				writer.Write("<form enctype=\"multipart/form-data\" action=\"editorderoption.aspx?OrderOptionID=" + OrderOptionID.ToString() + "&edit=" + Editing.ToString() + "\" method=\"post\" id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
				writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"100%\" colspan=\"2\" align=\"left\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Order Option Name:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"Name\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Name")) , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"Name_vldt\" value=\"[req][blankalert=Please enter the order option name]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Description:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<textarea cols=\"60\" rows=\"20\" id=\"Description\" name=\"Description\">" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Description")) , "") + "</textarea>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Cost:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"10\" size=\"10\" name=\"Cost\" value=\"" + Common.IIF(Editing , Common.IIF(DB.RSFieldDecimal(rs,"Cost") != System.Decimal.Zero , Localization.CurrencyStringForDB( DB.RSFieldDecimal(rs,"Cost")) , "") , "") + "\"> (in x.xx format)\n");
				writer.Write("                	<input type=\"hidden\" name=\"Cost_vldt\" value=\"[number][invalidalert=Please enter a valid dollar amount, e.g. 10.00 without the leading $ sign!]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("  <tr>\n");
				writer.Write("    <td valign=\"top\" align=\"right\">Icon:\n");
				writer.Write("</td>\n");
				writer.Write("    <td valign=\"top\" align=\"left\">");
				writer.Write("    <input type=\"file\" name=\"Image1\" size=\"30\" value=\"" + Common.IIF(Editing , "" , "") + "\">\n");
				String Image1URL = Common.LookupImage("OrderOption",OrderOptionID,"",_siteID);
				if(Image1URL.Length != 0)
				{
					writer.Write("<a href=\"javascript:void(0);\" onClick=\"DeleteIcon();\">Click here</a> to delete the current image<br>\n");
					writer.Write("<br><img id=\"CatPic\" name=\"CatPic\" border=\"0\" src=\"" + Image1URL + "?" + Common.GetRandomNumber(1,1000000).ToString() + "\">\n");
				}
				writer.Write("</td>\n");
				writer.Write(" </tr>\n");
				
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
			}
			rs.Close();

			writer.Write(Common.GenerateHtmlEditor("Description"));

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteIcon()\n");
			writer.Write("{\n");
			writer.Write("window.open('deleteicon.aspx?OrderOptionID=" + OrderOptionID.ToString() + "&FormImageName=CatPic',\"ASPDNSFAdmin_ML\",\"height=250,width=440,top=10,left=20,status=no,toolbar=no,menubar=no,scrollbars=yes,location=no\")\n");
			writer.Write("}\n");
			writer.Write("</SCRIPT>\n");
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
