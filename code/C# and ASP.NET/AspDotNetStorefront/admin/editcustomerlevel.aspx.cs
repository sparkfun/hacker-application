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
	/// Summary description for editcustomerlevel
	/// </summary>
	public class editcustomerlevel : SkinBase
	{
		
		int CustomerLevelID;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			CustomerLevelID = 0;

			if(Common.QueryString("CustomerLevelID").Length != 0 && Common.QueryString("CustomerLevelID") != "0") 
			{
				Editing = true;
				CustomerLevelID = Localization.ParseUSInt(Common.QueryString("CustomerLevelID"));
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
					// see if this CustomerLevel already exists:
					rs = DB.GetRS("select count(Name) as N from CustomerLevel  " + DB.GetNoLock() + " where CustomerLevelID<>" + CustomerLevelID.ToString() + " and deleted=0 and lower(Name)=" + DB.SQuote(Common.Form("Name").ToLower()));
					rs.Read();
					N = DB.RSFieldInt(rs,"N");
					rs.Close();
					if(N != 0)
					{
						ErrorMsg = "<p><b><font color=red>ERROR:<br><br></font><blockquote>There is already another Customer Level with that description. Please <a href=\"javascript:history.back(-1);\">go back</a>.</b></blockquote></p>";
					}
				}
				else
				{
					// see if this Name is already there:
					rs = DB.GetRS("select count(CustomerLevelID) as N from CustomerLevel  " + DB.GetNoLock() + " where deleted=0 and lower(Name)=" + DB.SQuote(Common.Form("Name").ToLower()));
					rs.Read();
					N = DB.RSFieldInt(rs,"N");
					rs.Close();
					if(N != 0)
					{
						ErrorMsg = "<p><b><font color=red>ERROR:<br><br></font><blockquote>There is already another Customer Level with that description. Please <a href=\"javascript:history.back(-1);\">go back</a>.</b></blockquote></p>";
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
							sql.Append("insert into CustomerLevel(CustomerLevelGUID,Name,LevelDiscountPercent,LevelDiscountAmount,LevelHasFreeShipping,LevelAllowsQuantityDiscounts,LevelHasNoTax,LevelAllowsCoupons,LevelDiscountsApplyToExtendedPrices,LastUpdatedBy) values(");
							sql.Append(DB.SQuote(NewGUID) + ",");
							sql.Append(DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							sql.Append(Localization.SingleStringForDB(Common.FormUSSingle("LevelDiscountPercent")) + ",");
							sql.Append(Localization.CurrencyStringForDB(Common.FormUSDecimal("LevelDiscountAmount")) + ",");
							sql.Append(Common.Form("LevelHasFreeShipping") + ",");
							sql.Append(Common.Form("LevelAllowsQuantityDiscounts") + ",");
							sql.Append(Common.Form("LevelHasNoTax") + ",");
							sql.Append(Common.Form("LevelAllowsCoupons") + ",");
							sql.Append(Common.Form("LevelDiscountsApplyToExtendedPrices") + ",");
							sql.Append(thisCustomer._customerID);
							sql.Append(")");
							DB.ExecuteSQL(sql.ToString());

							rs = DB.GetRS("select CustomerLevelID from CustomerLevel  " + DB.GetNoLock() + " where deleted=0 and CustomerLevelGUID=" + DB.SQuote(NewGUID));
							rs.Read();
							CustomerLevelID = DB.RSFieldInt(rs,"CustomerLevelID");
							Editing = true;
							rs.Close();
							DataUpdated = true;
						}
						else
						{
							// ok to update:
							sql.Append("update CustomerLevel set ");
							sql.Append("Name=" + DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							sql.Append("LevelDiscountPercent=" + Common.Form("LevelDiscountPercent") + ",");
							sql.Append("LevelDiscountAmount=" + Common.Form("LevelDiscountAmount") + ",");
							sql.Append("LevelHasFreeShipping=" + Common.Form("LevelHasFreeShipping") + ",");
							sql.Append("LevelAllowsQuantityDiscounts=" + Common.Form("LevelAllowsQuantityDiscounts") + ",");
							sql.Append("LevelHasNoTax=" + Common.Form("LevelHasNoTax") + ",");
							sql.Append("LevelAllowsCoupons=" + Common.Form("LevelAllowsCoupons") + ",");
							sql.Append("LevelDiscountsApplyToExtendedPrices=" + Common.Form("LevelDiscountsApplyToExtendedPrices") + ",");
							sql.Append("LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
							sql.Append("LastUpdatedBy=" + thisCustomer._customerID.ToString() + " ");
							sql.Append("where CustomerLevelID=" + CustomerLevelID.ToString());
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
			SectionTitle = "<a href=\"CustomerLevels.aspx\">CustomerLevels</a> - Manage Customer Levels";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			IDataReader rs = DB.GetRS("select * from CustomerLevel  " + DB.GetNoLock() + " where CustomerLevelID=" + CustomerLevelID.ToString());
			if(rs.Read())
			{
				Editing = true;
			}
			if(DataUpdated)
			{
				writer.Write("<p align=\"left\"><b><font color=blue>(UPDATED)</font></b></p>\n");
			}


			if(ErrorMsg.Length != 0)
			{
				writer.Write("<p align=\"left\"><b><font color=red>" + ErrorMsg + "</font></b></p>\n");
			}


			if(ErrorMsg.Length == 0)
			{

				if(Editing)
				{
					writer.Write("<p align=\"left\"><b>Editing CustomerLevel: " + DB.RSField(rs,"Name") + " (ID=" + DB.RSFieldInt(rs,"CustomerLevelID").ToString() + ")</p></b>\n");
				}
				else
				{
					writer.Write("<p align=\"left\"><b>Adding New Customer Level:</p></b>\n");
				}

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function Form_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("submitonce(theForm);\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				writer.Write("<p align=\"left\">Please enter the following information about this Customer Level. Fields marked with an asterisk (*) are required. All other fields are optional.</p>\n");
				writer.Write("<p align=\"left\"><b><font color=blue>WARNING: You can easily define a pricing/level/extended pricing/quantity pricing/coupon scheme that NO ONE could possibly figure out how the actual pricing was computed. Our suggestion is to KEEP IT SIMPLE. If you use Discount Percents for levels, DON'T USE extended pricing, and TURN OFF coupons for the level, etc... If you want to define extended pricing for each product variant, then set ALL level discounts to 0, disallow coupons, etc... NetStore will ALWAYS compute A price for each product and the entire order, but YOU may not be able to explain how the price was arrived at to your customers. KEEP IT SIMPLE!</font></b></p>\n");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\">\n");
				writer.Write("<form enctype=\"multipart/form-data\" action=\"editCustomerLevel.aspx?CustomerLevelID=" + CustomerLevelID.ToString() + "&edit=" + Editing.ToString() + "\" method=\"post\" id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
				writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"100%\" colspan=\"3\" align=\"left\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Description:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"Name\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Name")) , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"Name_vldt\" value=\"[req][blankalert=Please enter a description of this Customer Level, e.g. Reseller]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("<small>Enter the name for this level, e.g. Reseller, Wholesale, Platinum, Gold, Preferred, etc...best to KEEP IT SHORT!.</small>");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Level Discount Percent:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"LevelDiscountPercent\" value=\"" + Common.IIF(Editing , Localization.SingleStringForDB(DB.RSFieldSingle(rs,"LevelDiscountPercent")), "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"LevelDiscountPercent_vldt\" value=\"[req][blankalert=Please enter the Customer Level discount percentage, or 0]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("<small>(Enter 0, or a percentage like 5 or 7.5)</small> <small>if non-zero, this each product price will be discounted by this amount for customers of this level. If Level Discounts also apply to extended prices is also checked, this discount percentage will ALSO be applied to any level specific extended prices. Generally, if you are going to the trouble of setting up extended pricing schemes, this percentage should be 0, and the extended prices should be the actual level price. If you don't want go to all the work of setting up extended prices for each product, this percentage discount is the best way to go to offer reseller/wholsale pricing to certain customers.</small>");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Level Discount Amount:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"LevelDiscountAmount\" value=\"" + Common.IIF(Editing , Localization.CurrencyStringForDB(DB.RSFieldDecimal(rs,"LevelDiscountAmount")), "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"LevelDiscountAmount_vldt\" value=\"[req][blankalert=Please enter the Customer Level discount amount in your currency (e.g. dollars), or 0]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("<small>(Enter 0, or a dollar amount like 2.50 or 10.00)</small> <small>if non-zero, this amount will be subtracted from the entire order total, after all other criteria/discounts etc are applied. It will be unusual to apply this kind of level discount, but... you can.</small>");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Level Includes Free Shipping:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"LevelHasFreeShipping\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"LevelHasFreeShipping") , " checked " , "") , "") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"LevelHasFreeShipping\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"LevelHasFreeShipping") , "" , " checked ") , " checked ") + ">\n");
				writer.Write("                	</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("<small>if yes, then all customer orders for this level will have $0 shipping charges on their order.</small>");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Level Allows Quantity Discounts:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"LevelAllowsQuantityDiscounts\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"LevelAllowsQuantityDiscounts") , " checked " , "") , "") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"LevelAllowsQuantityDiscounts\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"LevelAllowsQuantityDiscounts") , "" , " checked ") , " checked ") + ">\n");
				writer.Write("                	</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("<small>if yes, then all quantity discount tables (specified elsewhere) are applied on a product by product basis, based on quantity. Discounts are applied after extended price, or level discounted price is applied.</small>");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Level Has No Tax On Orders:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"LevelHasNoTax\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"LevelHasNoTax") , " checked " , "") , "") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"LevelHasNoTax\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"LevelHasNoTax") , "" , " checked ") , " checked ") + ">\n");
				writer.Write("                	</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("<small>if yes, then all customer orders for this level will have $0 tax charged, regardless of their address.</small>");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Level Allows Coupons On Orders:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"LevelAllowsCoupons\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"LevelAllowsCoupons") , " checked " , "") , "") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"LevelAllowsCoupons\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"LevelAllowsCoupons") , "" , " checked ") , " checked ") + ">\n");
				writer.Write("                	</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("<small>if yes, then if the customer enters a valid coupon, that coupon discount specificiations will be applied to the ENTIRE ORDER, ON TOP of any level extended prices and level discounts. If no, then these customers are not allowed to enter coupon codes.</small>");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Level Discounts Also Apply To Extended Prices:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"LevelDiscountsApplyToExtendedPrices\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"LevelDiscountsApplyToExtendedPrices") , " checked " , "") , "") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"LevelDiscountsApplyToExtendedPrices\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"LevelDiscountsApplyToExtendedPrices") , "" , " checked ") , " checked ") + ">\n");
				writer.Write("                	</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("<small>if yes, then level discount amount and percents will be applied ON TOP of level extended prices. If no, then if an extended price exists for a product, that price is used as is.</small>");
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
