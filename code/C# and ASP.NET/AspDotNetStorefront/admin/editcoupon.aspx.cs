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
	/// Summary description for editcoupon
	/// </summary>
	public class editcoupon : SkinBase
	{
		
		int CouponID;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			CouponID = 0;

			if(Common.QueryString("CouponID").Length != 0 && Common.QueryString("CouponID") != "0") 
			{
				Editing = true;
				CouponID = Localization.ParseUSInt(Common.QueryString("CouponID"));
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
					// see if this coupon already exists:
					rs = DB.GetRS("select count(CouponCode) as N from coupon  " + DB.GetNoLock() + " where CouponID<>" + CouponID.ToString() + " and deleted=0 and lower(CouponCode)=" + DB.SQuote(Common.Form("CouponCode").ToLower()));
					rs.Read();
					N = DB.RSFieldInt(rs,"N");
					rs.Close();
					if(N != 0)
					{
						ErrorMsg = "<p><b><font color=red>ERROR:<br><br></font><blockquote>There is already another coupon with that code. Please <a href=\"javascript:history.back(-1);\">go back</a>.</b></blockquote></p>";
					}
				}
				else
				{
					// see if this name is already there:
					rs = DB.GetRS("select count(CouponID) as N from Coupon  " + DB.GetNoLock() + " where deleted=0 and lower(CouponCode)=" + DB.SQuote(Common.Form("CouponCode").ToLower()));
					rs.Read();
					N = DB.RSFieldInt(rs,"N");
					rs.Close();
					if(N != 0)
					{
						ErrorMsg = "<p><b><font color=red>ERROR:<br><br></font><blockquote>There is already another coupon with that name. Please <a href=\"javascript:history.back(-1);\">go back</a>.</b></blockquote></p>";
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
							sql.Append("insert into coupon(CouponGUID,CouponCode,ExpirationDate,Description,DiscountPercent,DiscountAmount,DiscountIncludesFreeShipping,ExpiresOnFirstUseByAnyCustomer,ExpiresAfterNUses,NumUses,ExpiresAfterOneUsageByEachCustomer,ValidForCustomers,ValidForProducts,ValidForManufacturers,RequiresMinimumOrderAmount,ValidForCategories,ValidForSections,LastUpdatedBy) values(");
							sql.Append(DB.SQuote(NewGUID) + ",");
							sql.Append(DB.SQuote(Common.Left(Common.Form("CouponCode"),100)) + ",");
							sql.Append(DB.SQuote(Common.Form("ExpirationDate")) + ",");
							sql.Append(DB.SQuote(Common.Form("Description")) + ",");
							sql.Append(Localization.SingleStringForDB(Common.FormUSSingle("DiscountPercent")) + ",");
							sql.Append(Localization.CurrencyStringForDB(Common.FormUSDecimal("DiscountAmount")) + ",");
							sql.Append(Common.FormUSInt("DiscountIncludesFreeShipping").ToString() + ",");
							sql.Append(Common.FormUSInt("ExpiresOnFirstUseByAnyCustomer").ToString() + ",");
							sql.Append(Common.FormUSInt("ExpiresAfterNUses").ToString() + ",");
							sql.Append("0,");
							sql.Append(Common.FormUSInt("ExpiresAfterOneUsageByEachCustomer").ToString() + ",");
							sql.Append(DB.SQuote(Common.Form("ValidForCustomers")) + ",");
							sql.Append(DB.SQuote(Common.Form("ValidForProducts")) + ",");
							sql.Append(DB.SQuote(Common.Form("ValidForManufacturers")) + ",");
							sql.Append(Localization.CurrencyStringForDB(Common.FormUSDecimal("RequiresMinimumOrderAmount")) + ",");
							sql.Append(DB.SQuote(Common.Form("ValidForCategories")) + ",");
							sql.Append(DB.SQuote(Common.Form("ValidForSections")) + ",");
							
							sql.Append(thisCustomer._customerID);
							sql.Append(")");
							DB.ExecuteSQL(sql.ToString());

							rs = DB.GetRS("select CouponID from coupon  " + DB.GetNoLock() + " where deleted=0 and CouponGUID=" + DB.SQuote(NewGUID));
							rs.Read();
							CouponID = DB.RSFieldInt(rs,"CouponID");
							Editing = true;
							rs.Close();
							DataUpdated = true;
						}
						else
						{
							// ok to update:
							sql.Append("update coupon set ");
							sql.Append("CouponCode=" + DB.SQuote(Common.Left(Common.Form("CouponCode"),100)) + ",");
							sql.Append("ExpirationDate=" + DB.SQuote(Common.Left(Common.Form("ExpirationDate"),100)) + ",");
							sql.Append("Description=" + DB.SQuote(Common.Form("Description")) + ",");
							sql.Append("DiscountPercent=" + Localization.SingleStringForDB(Common.FormUSSingle("DiscountPercent")) + ",");
							sql.Append("DiscountAmount=" + Localization.CurrencyStringForDB(Common.FormUSDecimal("DiscountAmount")) + ",");
							sql.Append("DiscountIncludesFreeShipping=" + Common.FormUSInt("DiscountIncludesFreeShipping").ToString() + ",");
							sql.Append("ExpiresOnFirstUseByAnyCustomer=" + Common.FormUSInt("ExpiresOnFirstUseByAnyCustomer").ToString() + ",");
							sql.Append("ExpiresAfterNUses=" + Common.FormUSInt("ExpiresAfterNUses").ToString() + ",");
							sql.Append("ExpiresAfterOneUsageByEachCustomer=" + Common.FormUSInt("ExpiresAfterOneUsageByEachCustomer").ToString() + ",");
							sql.Append("ValidForCustomers=" + DB.SQuote(Common.Form("ValidForCustomers")) + ",");
							sql.Append("ValidForProducts=" + DB.SQuote(Common.Form("ValidForProducts")) + ",");
							sql.Append("ValidForManufacturers=" + DB.SQuote(Common.Form("ValidForManufacturers")) + ",");
							sql.Append("RequiresMinimumOrderAmount=" + Localization.CurrencyStringForDB(Common.FormUSDecimal("RequiresMinimumOrderAmount")) + ",");
							sql.Append("ValidForCategories=" + DB.SQuote(Common.Form("ValidForCategories")) + ",");
							sql.Append("ValidForSections=" + DB.SQuote(Common.Form("ValidForSections")) + ",");
							sql.Append("LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
							sql.Append("LastUpdatedBy=" + thisCustomer._customerID.ToString() + " ");
							sql.Append("where CouponID=" + CouponID.ToString());
							DB.ExecuteSQL(sql.ToString());
							DataUpdated = true;
							Editing = true;
						}
					}
					catch(Exception ex)
					{
						ErrorMsg = "<p><b>ERROR: " + Common.GetExceptionDetail(ex,"<br>") + "<br><br></b></p>";
					}

				}

			}
			SectionTitle = "<a href=\"coupons.aspx\">Coupons</a> - Manage Coupons";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			IDataReader rs = DB.GetRS("select * from Coupon  " + DB.GetNoLock() + " where CouponID=" + CouponID.ToString());
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

			writer.Write("  <!-- calendar stylesheet -->\n");
			writer.Write("  <link rel=\"stylesheet\" type=\"text/css\" media=\"all\" href=\"jscalendar/calendar-win2k-cold-1.css\" title=\"win2k-cold-1\" />\n");
			writer.Write("\n");
			writer.Write("  <!-- main calendar program -->\n");
			writer.Write("  <script type=\"text/javascript\" src=\"jscalendar/calendar.js\"></script>\n");
			writer.Write("\n");
			writer.Write("  <!-- language for the calendar -->\n");
			writer.Write("  <script type=\"text/javascript\" src=\"jscalendar/lang/" + Localization.JSCalendarLanguageFile() + "\"></script>\n");
			writer.Write("\n");
			writer.Write("  <!-- the following script defines the Calendar.setup helper function, which makes\n");
			writer.Write("       adding a calendar a matter of 1 or 2 lines of code. -->\n");
			writer.Write("  <script type=\"text/javascript\" src=\"jscalendar/calendar-setup.js\"></script>\n");
			
			if(ErrorMsg.Length == 0)
			{

				writer.Write("<div align=\"left\">");
				if(Editing)
				{
					writer.Write("<b>Editing Coupon: " + DB.RSField(rs,"Name") + " (ID=" + DB.RSFieldInt(rs,"CouponID").ToString() + ")<br><br></b>\n");
				}
				else
				{
					writer.Write("<b>Adding New Coupon:<br><br></b>\n");
				}

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function Form_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("submitonce(theForm);\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				writer.Write("<p>Please enter the following information about this coupon. Fields marked with an asterisk (*) are required. All other fields are optional.</p>\n");
				writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\">\n");
				writer.Write("<form enctype=\"multipart/form-data\" action=\"editcoupon.aspx?CouponID=" + CouponID.ToString() + "&edit=" + Editing.ToString() + "\" method=\"post\" id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
				writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"100%\" colspan=\"2\" align=\"left\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Coupon Code:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"CouponCode\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"CouponCode")) , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"CouponCode_vldt\" value=\"[req][blankalert=Please enter the coupon code]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Expiration Date:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"ExpirationDate\" value=\"" + Common.IIF(Editing , Localization.ToNativeShortDateString(DB.RSFieldDateTime(rs,"ExpirationDate")) , Localization.ToNativeShortDateString(System.DateTime.Now.AddMonths(1))) + "\">&nbsp;<img src=\"skins/skin_" + _siteID.ToString() + "/images/calendar.gif\" style=\"cursor:hand;\" align=\"absmiddle\" id=\"f_trigger_s\">&nbsp;<small>(" + Localization.ShortDateFormat() + ")</small>\n");
				writer.Write("                	<input type=\"hidden\" name=\"ExpirationDate_vldt\" value=\"[req][blankalert=Please enter the expiration date (e.g. " + Localization.ShortDateFormat() + ")]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Description (HTML OK):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<textarea cols=\"70\" rows=\"5\" id=\"Description\" name=\"Description\">" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Description")) , "") + "</textarea>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Discount Percent:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"DiscountPercent\" value=\"" + Common.IIF(Editing , Localization.SingleStringForDB(DB.RSFieldSingle(rs,"DiscountPercent")), "") + "\"><small>(Enter 0, or a percentage like 5 or 7.5)</small>\n");
				writer.Write("                	<input type=\"hidden\" name=\"DiscountPercent_vldt\" value=\"[req][blankalert=Please enter the coupon discount percentage, or 0]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Discount Amount:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"DiscountAmount\" value=\"" + Common.IIF(Editing , Localization.CurrencyStringForDB(DB.RSFieldDecimal(rs,"DiscountAmount")), "") + "\"><small>(Enter 0, or a dollar amount like 2.50 or 10.00)</small>\n");
				writer.Write("                	<input type=\"hidden\" name=\"DiscountAmount_vldt\" value=\"[req][blankalert=Please enter the coupon discount amount in your currency (e.g. dollars), or 0]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Discount Includes Free Shipping:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"DiscountIncludesFreeShipping\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"DiscountIncludesFreeShipping") , " checked " , "") , "") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"DiscountIncludesFreeShipping\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"DiscountIncludesFreeShipping") , "" , " checked ") , " checked ") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");


				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Expires On First Usage By Any Customer:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"ExpiresOnFirstUseByAnyCustomer\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"ExpiresOnFirstUseByAnyCustomer") , " checked " , "") , "") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"ExpiresOnFirstUseByAnyCustomer\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"ExpiresOnFirstUseByAnyCustomer") , "" , " checked ") , " checked ") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");


				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Expires After One Usage By Each Customer:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"ExpiresAfterOneUsageByEachCustomer\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"ExpiresAfterOneUsageByEachCustomer") , " checked " , "") , "") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"ExpiresAfterOneUsageByEachCustomer\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"ExpiresAfterOneUsageByEachCustomer") , "" , " checked ") , " checked ") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Expires After N Uses:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"ExpiresAfterNUses\" value=\"" + Common.IIF(Editing , DB.RSFieldInt(rs,"ExpiresAfterNUses").ToString() , "0") + "\"><small>(Enter the # of times this coupon may be used by any/all customers, or 0 if unrestricted)</small>\n");
				writer.Write("                	<input type=\"hidden\" name=\"ExpiresAfterNUses_vldt\" value=\"[number][blankalert=Please enter the # of times this coupon may be used, by any/all customers, or 0]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\"># Uses:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write(Common.IIF(Editing , DB.RSFieldInt(rs,"NumUses").ToString() , "N/A"));
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Requires Minimum Order Amount:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"RequiresMinimumOrderAmount\" value=\"" + Common.IIF(Editing , Localization.CurrencyStringForDB(DB.RSFieldDecimal(rs,"RequiresMinimumOrderAmount")) , "0") + "\"><small>(If the coupon can only be used on orders that exceed a certain amount, enter that amount. Otherwise, leave this field blank or enter 0.0)</small>\n");
				writer.Write("                	<input type=\"hidden\" name=\"RequiresMinimumOrderAmount_vldt\" value=\"[number][blankalert=Please enter the minimum order amount in your currency (e.g. dollars), or 0]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Valid For Customer(s):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"ValidForCustomers\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"ValidForCustomers")) , "") + "\"> (enter the customer id(s) for which this coupon is valid, or leave blank to allow any customer to use it. Enter customer id's separated by a comma, e.g. 12343, 12344, 12345, etc...)\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Valid For Product(s):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"ValidForProducts\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"ValidForProducts")) , "") + "\"> (enter the product id(s) for which this coupon is valid, or leave blank to allow it to work on any product. Enter product id's separated by a comma, e.g. 40, 41, 42, etc...)\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Valid For Category(s):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"ValidForCategories\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"ValidForCategories")) , "") + "\"> (enter the category id(s) for which this coupon is valid, or leave blank to allow it to work on any category. Enter category id's separated by a comma, e.g. 1,2,3, etc...)\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Valid For Section(s):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"ValidForSections\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"ValidForSections")) , "") + "\"> (enter the section id(s) for which this coupon is valid, or leave blank to allow it to work on any section. Enter section id's separated by a comma, e.g. 1,2,3, etc...)\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Valid For Manufacturer(s):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"ValidForManufacturers\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"ValidForManufacturers")) , "") + "\"> (enter the manufacturer id(s) for which this coupon is valid, or leave blank to allow it to work on any manufacturer. Enter manufacturer id's separated by a comma, e.g. 1,2,3, etc...\n");
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
				writer.Write("\n<script type=\"text/javascript\">\n");
				writer.Write("    Calendar.setup({\n");
				writer.Write("        inputField     :    \"ExpirationDate\",      // id of the input field\n");
				writer.Write("        ifFormat       :    \"" + Localization.JSCalendarDateFormatSpec() + "\",       // format of the input field\n");
				writer.Write("        showsTime      :    false,            // will display a time selector\n");
				writer.Write("        button         :    \"f_trigger_s\",   // trigger for the calendar (button ID)\n");
				writer.Write("        singleClick    :    true            // double-click mode\n");
				writer.Write("    });\n");
				writer.Write("</script>\n");
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
