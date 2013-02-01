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
	/// Summary description for storewide.
	/// </summary>
	public class storewide : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Store-Wide Actions";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			bool SalesPromptUpdated = false;
			bool SpecTitleUpdated = false;
			bool InvalidateAllUserCookiesUpdated = false;
			bool SpecsInlineUpdated = false;
			bool AllowQuantityDiscountUpdated = false;
			bool ClearAllShoppingCartsUpdated = false;
			bool SalesPriceUpdated = false;
			bool PurgeOldAnonsUpdated = false;
			bool DeleteOrphanedOrdersUpdated = false;

			if(Common.Form("IsSubmitSalesPrompt").ToUpper() == "TRUE")
			{
				if(Localization.ParseUSInt(Common.Form("SalesPromptID")) != 0)
				{
					DB.ExecuteSQL("Update product set SalesPromptID=" + Common.Form("SalesPromptID") + " where 1=1 " + Common.IIF(Common.Form("CategoryID") != "0" , " and productid in (select distinct productid from productcategory  " + DB.GetNoLock() + " where categoryid=" + Common.Form("CategoryID") + ")" , "") + Common.IIF(Common.Form("SectionID") != "0" , " and productid in (select distinct productid from productsection  " + DB.GetNoLock() + " where SectionID=" + Common.Form("SectionID") + ")" , "") + Common.IIF(Common.Form("ManufacturerID") != "0" , " and productid in (select distinct productid from product  " + DB.GetNoLock() + " where manufacturerid=" + Common.Form("ManufacturerID") + ")" , ""));
					SalesPromptUpdated = true;
				}
			}

			if(Common.Form("IsSubmitInvalidateAllUserCookies").ToUpper() == "TRUE")
			{
				DB.ExecuteSQL("update customer set CustomerGUID=newid()");
				InvalidateAllUserCookiesUpdated = true;
			}

			if(Common.Form("IsSubmitSpecTitle").ToUpper() == "TRUE")
			{
				DB.ExecuteSQL("Update product set SpecTitle=" + DB.SQuote(Common.Form("SpecTitle")));
				SpecTitleUpdated = true;
			}

			if(Common.Form("IsSubmitClearAllShoppingCarts").ToUpper() == "TRUE")
			{
				// do NOT delete recurring items.
				DB.ExecuteLongTimeSQL("delete from kitcart where CartType<>" + ((int)CartTypeEnum.RecurringCart).ToString(),1000);
				DB.ExecuteLongTimeSQL("delete from customcart where CartType<>" + ((int)CartTypeEnum.RecurringCart).ToString(),1000);
				DB.ExecuteLongTimeSQL("delete from ShoppingCart where CartType<>" + ((int)CartTypeEnum.RecurringCart).ToString(),1000);
				ClearAllShoppingCartsUpdated = true;
			}

			if(Common.Form("IsSubmitSpecsInline").ToUpper() == "TRUE")
			{
				if(Common.Form("SpecsInline").Length != 0)
				{
					DB.ExecuteSQL("Update product set SpecsInline=" + Common.Form("SpecsInline"));
					SpecsInlineUpdated = true;
				}
			}

			if(Common.Form("IsSubmitAllowQuantityDiscount").ToUpper() == "TRUE")
			{
				DB.ExecuteSQL("Update product set QuantityDiscountID=" + Common.Form("QuantityDiscountID"));
				DB.ExecuteSQL("Update productvariant set QuantityDiscountID=" + Common.Form("QuantityDiscountID"));
				AllowQuantityDiscountUpdated = true;
			}

			if(Common.Form("IsSubmitDeleteOrphanedOrders").ToUpper() == "TRUE")
			{
				Common.AdminDeleteOrphanedOrders();
				DeleteOrphanedOrdersUpdated = true;
			}

			if(Common.Form("IsSubmitSalesPrice").ToUpper() == "TRUE")
			{
				DB.ExecuteSQL("Update productvariant set SalePrice=" + Common.IIF(Common.Form("DiscountPercent")=="0" || Common.Form("DiscountPercent")=="0.0" || Common.Form("DiscountPercent")=="0.00" , "NULL" , "Price*(1-(" + Common.Form("DiscountPercent") + "/100.0))") + " where 1=1 " + Common.IIF(Common.Form("CategoryID") != "0" , " and productid in (select distinct productid from productcategory  " + DB.GetNoLock() + " where categoryid=" + Common.Form("CategoryID") + ")" , "") + Common.IIF(Common.Form("SectionID") != "0" , " and productid in (select distinct productid from productsection  " + DB.GetNoLock() + " where SectionID=" + Common.Form("SectionID") + ")" , "") + Common.IIF(Common.Form("ManufacturerID") != "0" , " and productid in (select distinct productid from product  " + DB.GetNoLock() + " where manufacturerid=" + Common.Form("ManufacturerID") + ")" , ""));
				SalesPriceUpdated = true;
			}

			if(Common.Form("IsSubmitPurgeOldAnons").ToUpper() == "TRUE")
			{
				// delete anons who are 1 month old with no items in their shopping cart:
				DB.ExecuteLongTimeSQL("delete from customer where email like " + DB.SQuote("Anon_%") + " and customerid not in (select distinct customerid from ShoppingCart " + DB.GetNoLock() + " ) and customerid not in (select distinct customerid from kitcart " + DB.GetNoLock() + " ) and customerid not in (select distinct customerid from customcart " + DB.GetNoLock() + " ) and customerid not in (select distinct customerid from orders " + DB.GetNoLock() + " ) and customerid not in (select distinct customerid from rating " + DB.GetNoLock() + " ) and customerid not in (select distinct ratingcustomerid from ratingcommenthelpfulness " + DB.GetNoLock() + " ) and customerid not in (select distinct votingcustomerid from ratingcommenthelpfulness " + DB.GetNoLock() + " ) and customerid not in (select distinct customerid from pollvotingrecord " + DB.GetNoLock() + " )",1000);
				PurgeOldAnonsUpdated = true;
			}

			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");

			writer.Write("function SalesPromptForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("if (theForm.SalesPromptID.selectedIndex < 1)\n");
			writer.Write("{\n");
			writer.Write("alert(\"Please select a sales prompt to use.\");\n");
			writer.Write("theForm.SalesPromptID.focus();\n");
			writer.Write("submitenabled(theForm);\n");
			writer.Write("return (false);\n");
			writer.Write("    }\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");

			writer.Write("function SpecTitleForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");

			writer.Write("function InvalidateAllUserCookiesForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");

			writer.Write("function SpecsInlineForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");

			writer.Write("function ClearAllShoppingCartsForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");

			writer.Write("function AllowQuantityDiscountForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");

			writer.Write("function PurgeOldAnonsForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");

			writer.Write("function DeleteOrphanedOrdersForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");

			writer.Write("</script>\n");

			// SALES PROMPT:

			writer.Write("<form action=\"storewide.aspx\" method=\"post\" id=\"SalesPromptForm\" name=\"SalesPromptForm\" onsubmit=\"return (validateForm(this) && SalesPromptForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmitSalesPrompt\" value=\"true\">\n");

			writer.Write("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">\n");
			writer.Write("<tr valign=\"middle\">\n");
			writer.Write("<td width=\"300\" align=\"right\" valign=\"top\">Set 'On Sale' Prompt:&nbsp;&nbsp;</td>\n");
			writer.Write("<td align=\"left\">\n");
			writer.Write("<select size=\"1\" name=\"SalesPromptID\">\n");
			writer.Write(" <OPTION VALUE=\"0\" selected>SELECT ONE</option>\n");
			IDataReader rsst = DB.GetRS("select * from salesprompt  " + DB.GetNoLock() + " where deleted=0 order by name");
			while(rsst.Read())
			{
				writer.Write("<option value=\"" + DB.RSFieldInt(rsst,"SalesPromptID").ToString() + "\"");
				writer.Write(">" + DB.RSField(rsst,"Name") + "</option>");
			}
			rsst.Close();
			writer.Write("</select>\n");

			writer.Write("<br>For " + Common.AppConfig("CategoryPromptSingular") + ": ");
			writer.Write("<select onChange=\"document.FilterForm.submit()\" style=\"font-size: 9px;\" size=\"1\" name=\"CategoryID\">\n");
			writer.Write("<OPTION VALUE=\"0\">All " + Common.AppConfig("CategoryPromptPlural") + "</option>\n");
			String CatSel = Common.GetCategorySelectList(0,String.Empty,0);
			writer.Write(CatSel);
			writer.Write("</select>\n");

			writer.Write("<br>For " + Common.AppConfig("SectionPromptSingular") + ": ");
			writer.Write("<select onChange=\"document.FilterForm.submit()\" style=\"font-size: 9px;\" size=\"1\" name=\"SectionID\">\n");
			writer.Write("<OPTION VALUE=\"0\">All " + Common.AppConfig("SectionPromptPlural") + "</option>\n");
			String SecSel = Common.GetSectionSelectList(0,String.Empty,0);
			writer.Write(SecSel);
			writer.Write("</select>\n");

			writer.Write("<br>For " + "For Manufacturer: <select size=\"1\" name=\"ManufacturerID\" onChange=\"document.FilterForm.submit();\">\n");
			writer.Write("<OPTION VALUE=\"0\">All Manufacturers</option>\n");
			DataSet dsst = DB.GetDS("select * from Manufacturer  " + DB.GetNoLock() + " where deleted=0 order by displayorder,name",false,System.DateTime.Now.AddHours(1));
			foreach(DataRow row in dsst.Tables[0].Rows)
			{
				writer.Write("<option value=\"" + DB.RowFieldInt(row,"ManufacturerID").ToString() + "\"");
				writer.Write(">" + DB.RowField(row,"Name") + "</option>");
			}
			dsst.Dispose();
			writer.Write("</select>\n");

			if(SalesPromptUpdated)
			{
				writer.Write("&nbsp;&nbsp;<b><font color=blue>(UPDATED)</b></font>");
			}
			writer.Write("<br><input type=\"Submit\" value=\"Submit\" name=\"Submit\">\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("</form>\n");

			writer.Write("<hr size=1>\n");

			// SPEC TITLE:
			writer.Write("<form action=\"storewide.aspx\" method=\"post\" id=\"SpecTitleForm\" name=\"SpecTitleForm\" onsubmit=\"return (validateForm(this) && SpecTitleForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmitSpecTitle\" value=\"true\">\n");
			writer.Write("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">\n");
			writer.Write("              <tr valign=\"middle\">\n");
			writer.Write("                <td width=\"300\" align=\"right\" valign=\"middle\">*Spec Title For All Products:&nbsp;&nbsp;</td>\n");
			writer.Write("                <td align=\"left\">\n");
			writer.Write("                	<input maxLength=\"50\" size=\"30\" name=\"SpecTitle\">\n");
			if(SpecTitleUpdated)
			{
				writer.Write("&nbsp;&nbsp;<b><font color=blue>(UPDATED)</b></font>");
			}
			writer.Write("&nbsp;&nbsp;<input type=\"submit\" value=\"Submit\" name=\"submit\">\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("</form>\n");

			writer.Write("<hr size=1>\n");
			
			// SPECS INLINE:
			writer.Write("<form action=\"storewide.aspx\" method=\"post\" id=\"SpecsInlineForm\" name=\"SpecsInlineForm\" onsubmit=\"return (validateForm(this) && SpecsInlineForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmitSpecsInline\" value=\"true\">\n");
			writer.Write("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">\n");
			writer.Write("              <tr valign=\"middle\">\n");
			writer.Write("                <td width=\"300\" align=\"right\" valign=\"middle\">*Specs Inline For All Products:&nbsp;&nbsp;</td>\n");
			writer.Write("                <td align=\"left\">\n");

			
			
			writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"SpecsInline\" value=\"1\">\n");
			writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"SpecsInline\" value=\"0\">\n");
			if(SpecsInlineUpdated)
			{
				writer.Write("&nbsp;&nbsp;<b><font color=blue>(UPDATED)</b></font>");
			}
			writer.Write("&nbsp;&nbsp;<input type=\"submit\" value=\"Submit\" name=\"submit\">\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("</form>\n");

			writer.Write("<hr size=1>\n");
			
			// Quantity discount:
			writer.Write("<form action=\"storewide.aspx\" method=\"post\" id=\"AllowQuantityDiscountForm\" name=\"AllowQuantityDiscountForm\" onsubmit=\"return (validateForm(this) && AllowQuantityDiscountForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmitAllowQuantityDiscount\" value=\"true\">\n");
			writer.Write("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">\n");
			writer.Write("<tr valign=\"middle\">\n");
			writer.Write("<td width=\"300\" align=\"right\" valign=\"middle\">*Quantity Discount Table to be used for ALL Products & Variants:&nbsp;&nbsp;</td>\n");
			writer.Write("<td align=\"left\">\n");
			
			writer.Write("<select size=\"1\" name=\"QuantityDiscountID\">\n");
			writer.Write(" <OPTION VALUE=\"0\" selected>Set to None</option>\n");
			rsst = DB.GetRS("select * from quantitydiscount  " + DB.GetNoLock() + " order by name");
			while(rsst.Read())
			{
				writer.Write("<option value=\"" + DB.RSFieldInt(rsst,"QuantityDiscountID").ToString() + "\"");
				writer.Write(">" + DB.RSField(rsst,"Name") + "</option>");
			}
			rsst.Close();
			writer.Write("</select>\n");
			if(AllowQuantityDiscountUpdated)
			{
				writer.Write("&nbsp;&nbsp;<b><font color=blue>(UPDATED)</b></font>");
			}
			writer.Write("&nbsp;&nbsp;<input type=\"submit\" value=\"Submit\" name=\"submit\">\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("</form>\n");

			writer.Write("<hr size=1>\n");
			
			// SET SALES PRICE:
			writer.Write("<form action=\"storewide.aspx\" method=\"post\" id=\"SalesPriceForm\" name=\"SalesPriceForm\" onsubmit=\"return (validateForm(this) && SalesPriceForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmitSalesPrice\" value=\"true\">\n");
			writer.Write("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">\n");
			writer.Write("              <tr valign=\"top\">\n");
			writer.Write("                <td width=\"300\" align=\"right\" valign=\"top\">Set Sales Discount Percentage:&nbsp;&nbsp;</td>\n");
			writer.Write("                <td align=\"left\">\n");
			writer.Write("                	*Discount Percentage: <input maxLength=\"10\" size=\"10\" name=\"DiscountPercent\"> (in x.xx format)\n");
			writer.Write("                	<input type=\"hidden\" name=\"DiscountPercent_vldt\" value=\"[req][number][blankalert=Please enter the sales price discount, as a percentage, e.g. 10][invalidalert=Please enter a valid percentage discount, e.g. 10]\">\n");

			writer.Write("<br>For " + Common.AppConfig("CategoryPromptSingular") + ": ");
			writer.Write("<select onChange=\"document.FilterForm.submit()\" style=\"font-size: 9px;\" size=\"1\" name=\"CategoryID\">\n");
			writer.Write("<OPTION VALUE=\"0\">All " + Common.AppConfig("CategoryPromptPlural") + "</option>\n");
			writer.Write(CatSel);
			writer.Write("</select>\n");

			writer.Write("<br>For " + Common.AppConfig("SectionPromptSingular") + ": ");
			writer.Write("<select onChange=\"document.FilterForm.submit()\" style=\"font-size: 9px;\" size=\"1\" name=\"SectionID\">\n");
			writer.Write("<OPTION VALUE=\"0\">All " + Common.AppConfig("SectionPromptPlural") + "</option>\n");
			writer.Write(SecSel);
			writer.Write("</select>\n");

			writer.Write("<br>For " + "For Manufacturer: <select size=\"1\" name=\"ManufacturerID\" onChange=\"document.FilterForm.submit();\">\n");
			writer.Write("<OPTION VALUE=\"0\">All Manufacturers</option>\n");
			dsst = DB.GetDS("select * from Manufacturer  " + DB.GetNoLock() + " where deleted=0 order by displayorder,name",false,System.DateTime.Now.AddHours(1));
			foreach(DataRow row in dsst.Tables[0].Rows)
			{
				writer.Write("<option value=\"" + DB.RowFieldInt(row,"ManufacturerID").ToString() + "\"");
				writer.Write(">" + DB.RowField(row,"Name") + "</option>");
			}
			dsst.Dispose();
			writer.Write("</select>\n");

			if(SalesPriceUpdated)
			{
				writer.Write("<br><b><font color=blue>(UPDATED)</b></font>");
			}
			writer.Write("<br><input type=\"submit\" value=\"Submit\" name=\"submit\">\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("</form>\n");

			writer.Write("<hr size=1>\n");

			// PURGE OLD ANONS:
			writer.Write("<form action=\"storewide.aspx\" method=\"post\" id=\"PurgeOldAnonsForm\" name=\"PurgeOldAnonsForm\" onsubmit=\"return (validateForm(this) && PurgeOldAnonsForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmitPurgeOldAnons\" value=\"true\">\n");
			writer.Write("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">\n");
			writer.Write("              <tr valign=\"middle\">\n");
			writer.Write("                <td width=\"300\" align=\"right\" valign=\"middle\">Purge Old Anons:&nbsp;&nbsp;</td>\n");
			writer.Write("                <td align=\"left\">\n");
			if(PurgeOldAnonsUpdated)
			{
				writer.Write("&nbsp;&nbsp;<b><font color=blue>(UPDATED)</b></font>");
			}
			writer.Write("&nbsp;&nbsp;<input type=\"submit\" value=\"Submit\" name=\"submit\">\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("</form>\n");

			writer.Write("<hr size=1>\n");

			// CLEAR ALL SHOPPING CARTS:
			writer.Write("<form action=\"storewide.aspx\" method=\"post\" id=\"ClearAllShoppingCartsForm\" name=\"ClearAllShoppingCartsForm\" onsubmit=\"return (validateForm(this) && ClearAllShoppingCartsForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmitClearAllShoppingCarts\" value=\"true\">\n");
			writer.Write("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">\n");
			writer.Write("              <tr valign=\"middle\">\n");
			writer.Write("                <td width=\"300\" align=\"right\" valign=\"middle\">Clear All Shopping Carts (except Recurring Items):&nbsp;&nbsp;</td>\n");
			writer.Write("                <td align=\"left\">\n");
			if(ClearAllShoppingCartsUpdated)
			{
				writer.Write("&nbsp;&nbsp;<b><font color=blue>(UPDATED)</b></font>");
			}
			writer.Write("&nbsp;&nbsp;<input type=\"submit\" value=\"Submit\" name=\"submit\">\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("</form>\n");

			writer.Write("<hr size=1>\n");

			// DELETE ORPHANED ORDERS:
			writer.Write("<form action=\"storewide.aspx\" method=\"post\" id=\"DeleteOrphanedOrdersForm\" name=\"DeleteOrphanedOrdersForm\" onsubmit=\"return (validateForm(this) && DeleteOrphanedOrdersForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmitDeleteOrphanedOrders\" value=\"true\">\n");
			writer.Write("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">\n");
			writer.Write("              <tr valign=\"middle\">\n");
			writer.Write("                <td width=\"300\" align=\"right\" valign=\"middle\">Delete Orphaned Orders:&nbsp;&nbsp;</td>\n");
			writer.Write("                <td align=\"left\">\n");
			if(DeleteOrphanedOrdersUpdated)
			{
				writer.Write("&nbsp;&nbsp;<b><font color=blue>(UPDATED)</b></font>");
			}
			writer.Write("&nbsp;&nbsp;<input type=\"submit\" value=\"Submit\" name=\"submit\">\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("</form>\n");

			writer.Write("<hr size=1>\n");

			// INVALIDATE ALL USER COOKIES:
			writer.Write("<form action=\"storewide.aspx\" method=\"post\" id=\"InvalidateAllUserCookiesForm\" name=\"InvalidateAllUserCookiesForm\" onsubmit=\"return (validateForm(this) && InvalidateAllUserCookiesForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmitInvalidateAllUserCookies\" value=\"true\">\n");
			writer.Write("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">\n");
			writer.Write("              <tr valign=\"middle\">\n");
			writer.Write("                <td width=\"300\" align=\"right\" valign=\"middle\">Invalidate All User Cookies:&nbsp;&nbsp;</td>\n");
			writer.Write("                <td align=\"left\">\n");
			if(InvalidateAllUserCookiesUpdated)
			{
				writer.Write("&nbsp;&nbsp;<b><font color=blue>(UPDATED)</b></font>");
			}
			writer.Write("&nbsp;&nbsp;<input type=\"submit\" value=\"Submit\" name=\"submit\">\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("</form>\n");

			writer.Write("<hr size=1>\n");

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
