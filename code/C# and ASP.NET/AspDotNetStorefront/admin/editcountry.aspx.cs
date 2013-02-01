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
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for editcountry
	/// </summary>
	public class editcountry : SkinBase
	{
		
		int CountryID;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			CountryID = 0;
			if(Common.QueryString("CountryID").Length != 0 && Common.QueryString("CountryID") != "0") 
			{
				Editing = true;
				CountryID = Localization.ParseUSInt(Common.QueryString("CountryID"));
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
					if(!Editing)
					{
						// ok to add them:
						String NewGUID = DB.GetNewGUID();
						sql.Append("insert into Country(CountryGUID,Name,TwoLetterISOCode,ThreeLetterISOCode,NumericISOCode,LastUpdatedBy) values(");
						sql.Append(DB.SQuote(NewGUID) + ",");
						sql.Append(DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
						sql.Append(DB.SQuote(Common.Left(Common.Form("TwoLetterISOCode"),2)) + ",");
						sql.Append(DB.SQuote(Common.Left(Common.Form("ThreeLetterISOCode"),3)) + ",");
						sql.Append(DB.SQuote(Common.Left(Common.Form("NumericISOCode"),3)) + ",");
						sql.Append(thisCustomer._customerID);
						sql.Append(")");
						DB.ExecuteSQL(sql.ToString());

						rs = DB.GetRS("select CountryID from Country  " + DB.GetNoLock() + " where CountryGUID=" + DB.SQuote(NewGUID));
						rs.Read();
						CountryID = DB.RSFieldInt(rs,"CountryID");
						Editing = true;
						rs.Close();
						DataUpdated = true;
					}
					else
					{
						// ok to update:
						sql.Append("update Country set ");
						sql.Append("Name=" + DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
						sql.Append("TwoLetterISOCode=" + DB.SQuote(Common.Left(Common.Form("TwoLetterISOCode"),2)) + ",");
						sql.Append("ThreeLetterISOCode=" + DB.SQuote(Common.Left(Common.Form("ThreeLetterISOCode"),3)) + ",");
						sql.Append("NumericISOCode=" + DB.SQuote(Common.Left(Common.Form("NumericISOCode"),3)) + ",");
						sql.Append("LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
						sql.Append("LastUpdatedBy=" + thisCustomer._customerID.ToString() + " ");
						sql.Append("where CountryID=" + CountryID.ToString());
						DB.ExecuteSQL(sql.ToString());
						DataUpdated = true;
						Editing = true;
					}
				//}
				//catch(Exception ex)
				//{
				//	ErrorMsg = "<p><b>ERROR: " + Common.GetExceptionDetail(ex,"<br>") + "<br><br></b></p>";
				//}

			}
			SectionTitle = "<a href=\"countries.aspx\">Countries</a> - Manage Countries";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			IDataReader rs = DB.GetRS("select * from Country  " + DB.GetNoLock() + " where CountryID=" + CountryID.ToString());
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

				writer.Write("<div align=\"left\">");
				if(Editing)
				{
					writer.Write("<b>Editing Country: " + DB.RSField(rs,"Name") + " (ID=" + DB.RSFieldInt(rs,"CountryID").ToString() + ")<br><br></b>\n");
				}
				else
				{
					writer.Write("<b>Adding New Country:<br><br></b>\n");
				}

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function Form_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("submitonce(theForm);\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				writer.Write("<p>Please enter the following information about this Country. Fields marked with an asterisk (*) are required. All other fields are optional.</p>\n");
				writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\">\n");
				writer.Write("<form enctype=\"multipart/form-data\" action=\"editCountry.aspx?CountryID=" + CountryID.ToString() + "&edit=" + Editing.ToString() + "\" method=\"post\" id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
				writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"100%\" colspan=\"2\" align=\"left\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Name:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"Name\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Name")) , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"Name_vldt\" value=\"[req][blankalert=Please enter the Country name]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Two Letter ISO Code:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"2\" size=\"5\" name=\"TwoLetterISOCode\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"TwoLetterISOCode")) , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"TwoLetterISOCode_vldt\" value=\"[req][blankalert=Please enter the Country's Two Letter ISO Code]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Three Letter ISO Code:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"3\" size=\"5\" name=\"ThreeLetterISOCode\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"ThreeLetterISOCode")) , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"ThreeLetterISOCode_vldt\" value=\"[req][blankalert=Please enter the Country's Three Letter ISO Code]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Numeric ISO Code:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"5\" size=\"5\" name=\"NumericISOCode\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"NumericISOCode")) , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"NumericISOCode_vldt\" value=\"[req][blankalert=Please enter the Country's Numeric ISO Code]\">\n");
				writer.Write("                	</td>\n");
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
				writer.Write("</div>");
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
