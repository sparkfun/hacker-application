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
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for editshippingzone
	/// </summary>
	public class editshippingzone : SkinBase
	{
		
		int ShippingZoneID;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			ShippingZoneID = 0;

			if(Common.QueryString("ShippingZoneID").Length != 0 && Common.QueryString("ShippingZoneID") != "0") 
			{
				Editing = true;
				ShippingZoneID = Localization.ParseUSInt(Common.QueryString("ShippingZoneID"));
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
					// see if this shippingZone already exists:
					rs = DB.GetRS("select count(Name) as N from shippingZone  " + DB.GetNoLock() + " where ShippingZoneID<>" + ShippingZoneID.ToString() + " and deleted=0 and lower(Name)=" + DB.SQuote(Common.Form("Name").ToLower()));
					rs.Read();
					N = DB.RSFieldInt(rs,"N");
					rs.Close();
					if(N != 0)
					{
						ErrorMsg = "<p><b><font color=red>ERROR:<br><br></font><blockquote>There is already another shipping Zone with that name. Please <a href=\"javascript:history.back(-1);\">go back</a>.</b></blockquote></p>";
					}
				}
				else
				{
					// see if this name is already there:
					rs = DB.GetRS("select count(ShippingZoneID) as N from ShippingZone  " + DB.GetNoLock() + " where deleted=0 and lower(Name)=" + DB.SQuote(Common.Form("Name").ToLower()));
					rs.Read();
					N = DB.RSFieldInt(rs,"N");
					rs.Close();
					if(N != 0)
					{
						ErrorMsg = "<p><b><font color=red>ERROR:<br><br></font><blockquote>There is already another shipping Zone with that name. Please <a href=\"javascript:history.back(-1);\">go back</a>.</b></blockquote></p>";
					}
				}

				if(ErrorMsg.Length == 0)
				{
					try
					{
						StringBuilder sql = new StringBuilder(2500);
						if(!Editing)
						{
							// ok to add:
							String NewGUID = DB.GetNewGUID();
							sql.Append("insert into shippingZone(ShippingZoneGUID,Name,ZipCodes,LastUpdatedBy) values(");
							sql.Append(DB.SQuote(NewGUID) + ",");
							sql.Append(DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							sql.Append(DB.SQuote(Common.Form("ZipCodes")) + ",");
							sql.Append(thisCustomer._customerID);
							sql.Append(")");
							DB.ExecuteSQL(sql.ToString());

							rs = DB.GetRS("select ShippingZoneID from shippingZone  " + DB.GetNoLock() + " where deleted=0 and ShippingZoneGUID=" + DB.SQuote(NewGUID));
							rs.Read();
							ShippingZoneID = DB.RSFieldInt(rs,"ShippingZoneID");
							Editing = true;
							rs.Close();
							DataUpdated = true;
						}
						else
						{
							// ok to update:
							sql.Append("update shippingZone set ");
							sql.Append("Name=" + DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							sql.Append("ZipCodes=" + DB.SQuote(Common.Form("ZipCodes")) + ",");
							sql.Append("LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
							sql.Append("LastUpdatedBy=" + thisCustomer._customerID.ToString() + " ");
							sql.Append("where ShippingZoneID=" + ShippingZoneID.ToString());
							DB.ExecuteSQL(sql.ToString());
							DataUpdated = true;
							Editing = true;
						}
					}
					catch(Exception ex)
					{
						ErrorMsg = "<p align=\"left\"><b>ERROR: " + Common.GetExceptionDetail(ex,"<br>") + "<br><br></b></p>";
					}
				}

			}
			SectionTitle = "<a href=\"shippingZones.aspx\">Shipping Zones</a> - Manage Shipping Zones";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			IDataReader rs = DB.GetRS("select * from ShippingZone  " + DB.GetNoLock() + " where deleted=0 and ShippingZoneID=" + ShippingZoneID.ToString());
			Editing = false;
			if(rs.Read())
			{
				Editing = true;
			}
			
			if(ErrorMsg.Length != 0)
			{
				writer.Write("<p align=\"left\"><b><font color=red>" + ErrorMsg + "</font></b></p>\n");
			}
			if(DataUpdated)
			{
				writer.Write("<p align=\"left\"><b><font color=blue>(UPDATED)</font></b></p>\n");
			}

			if(ErrorMsg.Length == 0)
			{

				if(Editing)
				{
					writer.Write("<p align=\"left\"><b>Editing Shipping Zone: " + DB.RSField(rs,"Name") + " (ID=" + DB.RSFieldInt(rs,"ShippingZoneID").ToString() + ")</p></b>\n");
				}
				else
				{
					writer.Write("<p align=\"left\"><b>Adding New Shipping Zone:</p></b>\n");
				}

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function ShippingZoneForm_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("submitonce(theForm);\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				writer.Write("<p align=\"left\">Please enter the following information about this zone. Fields marked with an asterisk (*) are required. All other fields are optional.</p>\n");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\">\n");
				writer.Write("<form action=\"editshippingZone.aspx?ShippingZoneID=" + ShippingZoneID.ToString() + "&edit=" + Editing.ToString() + "\" Method=\"post\" id=\"ShippingZoneForm\" name=\"ShippingZoneForm\" onsubmit=\"return (validateForm(this) && ShippingZoneForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
				writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"100%\" colspan=\"2\" align=\"left\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Name:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"Name\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Name")) , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"Name_vldt\" value=\"[req][blankalert=Please enter the shipping Zone name]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"top\">*ZipCodes:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Enter the target zip code prefixes to match against, e.g. 850, 851, 004-005, etc.<br>Use a comma separate list of 3 digit prefixes.<br>Ranges can be entered as NNN-MMM.<br>");
				writer.Write("                	<textarea id=\"ZipCodes\" name=\"ZipCodes\" cols=\"" + Common.AppConfig("Admin_TextareaWidth") + "\" rows=\"" + Common.AppConfig("Admin_TextareaHeightSmall") + "\">" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"ZipCodes")) , "") + "</textarea>\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("<tr>\n");
				writer.Write("<td></td><td align=\"left\"><br>\n");
				if(Editing)
				{
					writer.Write("<input type=\"submit\" value=\"Update\" name=\"submit\">\n");
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
