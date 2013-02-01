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
	/// Summary description for prices.
	/// </summary>
	public class prices : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Update Prices";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			int CategoryID = Common.QueryStringUSInt("CategoryID");
			int SectionID = Common.QueryStringUSInt("SectionID");
			int ProductTypeID = Common.QueryStringUSInt("ProductTypeID");
			int ManufacturerID = Common.QueryStringUSInt("ManufacturerID");

			if(Common.QueryString("CategoryID").Length == 0)
			{
				CategoryID = Common.CookieUSInt("CategoryID");
			}
			if(Common.QueryString("SectionID").Length == 0)
			{
				SectionID = Common.CookieUSInt("SectionID");
			}
			if(Common.QueryString("ProductTypeID").Length == 0)
			{
				ProductTypeID = Common.CookieUSInt("ProductTypeID");
			}
			if(Common.QueryString("ManufacturerID").Length == 0)
			{
				ManufacturerID = Common.CookieUSInt("ManufacturerID");
			}

			Common.SetCookie("CategoryID",CategoryID.ToString(),new TimeSpan(365,0,0,0,0));
			Common.SetCookie("SectionID",SectionID.ToString(),new TimeSpan(365,0,0,0,0));
			Common.SetCookie("ProductTypeID",ProductTypeID.ToString(),new TimeSpan(365,0,0,0,0));
			Common.SetCookie("ManufacturerID",ManufacturerID.ToString(),new TimeSpan(365,0,0,0,0));

			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				for(int i = 0; i<=Request.Form.Count-1; i++)
				{
					if(Request.Form.Keys[i].IndexOf("XPrice_") != -1 && Request.Form.Keys[i].IndexOf("_vldt") == -1)
					{
						String[] keys = Request.Form.Keys[i].Split('_');
						int VariantID = Localization.ParseUSInt(keys[1]);
						decimal Price = System.Decimal.Zero;
						try
						{
							if(Common.Form("XPrice_" + VariantID.ToString()).Length != 0)
							{
								Price = Common.FormUSDecimal("XPrice_" + VariantID.ToString());
							}
							DB.ExecuteSQL("update ProductVariant set Price=" + Common.IIF(Price != System.Decimal.Zero , Localization.CurrencyStringForDB(Price) , "NULL") + ", LastUpdatedBy=" + thisCustomer._customerID.ToString() + ", LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + "  where VariantID=" + VariantID.ToString());
						}
						catch {}
					}
					if(Request.Form.Keys[i].IndexOf("YPrice") != -1 && Request.Form.Keys[i].IndexOf("_vldt") == -1)
					{
						String[] keys = Request.Form.Keys[i].Split('_');
						int VariantID = Localization.ParseUSInt(keys[1]);
						decimal SalePrice = System.Decimal.Zero;
						try
						{
							if(Common.Form("YPrice_" + VariantID.ToString()).Length != 0)
							{
								SalePrice = Common.FormUSDecimal("YPrice_" + VariantID.ToString());
							}
							DB.ExecuteSQL("update ProductVariant set SalePrice=" + Common.IIF(SalePrice != System.Decimal.Zero , Localization.CurrencyStringForDB(SalePrice) , "NULL") + ", LastUpdatedBy=" + thisCustomer._customerID.ToString() + ", LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + " where VariantID=" + VariantID.ToString());
						}
						catch {}
					}
				}
			}
			
			writer.Write("<a href=\"prices.aspx?categoryid=0&sectionid=0&producttypeid=0&manufacturerid=0\">RESET FILTERS</a><br>");
			writer.Write("<form id=\"FilterForm\" name=\"FilterForm\" method=\"GET\" action=\"prices.aspx\">\n");

			writer.Write(Common.AppConfig("CategoryPromptSingular") + ": ");
			writer.Write("<select onChange=\"document.FilterForm.submit()\" style=\"font-size: 9px;\" size=\"1\" name=\"CategoryID\">\n");
			writer.Write("<OPTION VALUE=\"0\" " + Common.IIF(CategoryID == 0 , " selected " , "") + ">All " + Common.AppConfig("CategoryPromptPlural") + "</option>\n");
			String CatSel = Common.GetCategorySelectList(0,String.Empty,0);
			// mark current Category:
			CatSel = CatSel.Replace("<option value=\"" + CategoryID.ToString() + "\">","<option value=\"" + CategoryID.ToString() + "\" selected>");
			writer.Write(CatSel);
			writer.Write("</select>\n");

			writer.Write("&nbsp;&nbsp;");

			writer.Write(Common.AppConfig("SectionPromptSingular") + ": ");
			writer.Write("<select onChange=\"document.FilterForm.submit()\" style=\"font-size: 9px;\" size=\"1\" name=\"SectionID\">\n");
			writer.Write("<OPTION VALUE=\"0\" " + Common.IIF(SectionID==0 , " selected " , "") + ">All " + Common.AppConfig("SectionPromptPlural") + "</option>\n");
			String SecSel = Common.GetSectionSelectList(0,String.Empty,0);
			// mark current Section:
			SecSel = SecSel.Replace("<option value=\"" + SectionID.ToString() + "\">","<option value=\"" + SectionID.ToString() + "\" selected>");
			writer.Write(SecSel);
			writer.Write("</select>\n");

			writer.Write("&nbsp;&nbsp;");

			writer.Write("Manufacturer: <select size=\"1\" name=\"ManufacturerID\" onChange=\"document.FilterForm.submit();\">\n");
			writer.Write("<OPTION VALUE=\"0\" " + Common.IIF(ManufacturerID==0 , " selected " , "") + ">All Manufacturers</option>\n");
			DataSet dsst = DB.GetDS("select * from Manufacturer  " + DB.GetNoLock() + " where deleted=0 order by displayorder,name",false,System.DateTime.Now.AddHours(1));
			foreach(DataRow row in dsst.Tables[0].Rows)
			{
				writer.Write("<option value=\"" + DB.RowFieldInt(row,"ManufacturerID").ToString() + "\"");
				if(DB.RowFieldInt(row,"ManufacturerID") == ManufacturerID )
				{
					writer.Write(" selected");
				}
				writer.Write(">" + DB.RowField(row,"Name") + "</option>");
			}
			dsst.Dispose();
			writer.Write("</select>\n");

			writer.Write("&nbsp;&nbsp;");

			writer.Write("Product Type: <select size=\"1\" name=\"ProductTypeID\" onChange=\"document.FilterForm.submit();\">\n");
			writer.Write("<OPTION VALUE=\"0\" " + Common.IIF(ProductTypeID==0 , " selected " , "") + ">All Product Types</option>\n");
			dsst = DB.GetDS("select * from ProductType  " + DB.GetNoLock() + " where deleted=0 order by name",false,System.DateTime.Now.AddHours(1));
			foreach(DataRow row in dsst.Tables[0].Rows)
			{
				writer.Write("<option value=\"" + DB.RowFieldInt(row,"ProductTypeID").ToString() + "\"");
				if(DB.RowFieldInt(row,"ProductTypeID") == ProductTypeID )
				{
					writer.Write(" selected");
				}
				writer.Write(">" + DB.RowField(row,"Name") + "</option>");
			}
			dsst.Dispose();
			writer.Write("</select>\n");
			
			writer.Write("</form>\n");

			String sql = "SELECT Product.ProductID, Product.Name, Product.SKU, Product.ManufacturerID, Product.ManufacturerPartNumber, Product.Published, Product.Deleted, ProductVariant.VariantID, ProductVariant.Name AS VName, ProductVariant.SKUSuffix, ProductVariant.ManufacturerPartNumber AS VManufacturerPartNumber, ProductVariant.Price, ProductVariant.SalePrice, ProductVariant.Deleted AS VDeleted, ProductVariant.Published AS VPublished ";
			sql += " FROM Product  " + DB.GetNoLock() + " LEFT OUTER JOIN ProductVariant  " + DB.GetNoLock() + " ON Product.ProductID = ProductVariant.ProductID ";
			sql += " WHERE Product.Published=1 AND Product.Deleted=0 AND ProductVariant.Published=1 AND ProductVariant.Deleted=0 ";
			if(CategoryID != 0)
			{
				sql += " and product.ProductID in (select distinct productid from productcategory  " + DB.GetNoLock() + " where categoryid=" + CategoryID.ToString() + ")";
			}
			if(SectionID != 0)
			{
				sql += " and product.ProductID in (select distinct productid from productsection  " + DB.GetNoLock() + " where SectionID=" + SectionID.ToString() + ")";
			}
			if(ProductTypeID != 0)
			{
				sql += " and product.ProductTypeID=" + ProductTypeID.ToString();
			}
			if(ManufacturerID != 0)
			{
				sql += " and product.ManufacturerID=" + ManufacturerID.ToString();
			}
			sql += " order by product.name,productvariant.name";
			DataSet ds = DB.GetDS(sql,false);

			int NumRows = ds.Tables[0].Rows.Count;
			int PageSize = 30;
			int PageNum = Common.QueryStringUSInt("PageNum");
			if(PageNum == 0)
			{
				PageNum = 1;
			}
			if(Common.QueryString("show") == "all")
			{
				PageSize = 1000000;
				PageNum = 1;
			}
			int NumPages = (NumRows/PageSize) + Common.IIF(NumRows % PageSize == 0 , 0 , 1);
			if(PageNum > NumPages)
			{
				if(NumRows > 0)
				{
					Response.Redirect("prices.aspx?categoryid=" + CategoryID.ToString() + "&producttypeid=" + ProductTypeID.ToString() + "&manufacturerID=" + ManufacturerID.ToString() + "&pagenum=" + (PageNum-1).ToString());
				}
			}
			int StartRow = (PageSize*(PageNum-1)) + 1;
			int StopRow = StartRow + PageSize -1 ;
			if(StopRow > NumRows)
			{
				StopRow = NumRows;
			}

			String QueryParms = "categoryid=" + CategoryID.ToString() + "&sectionid=" + SectionID.ToString() + "&manufacturerid=" + ManufacturerID.ToString() + "&producttypeid=" + ProductTypeID.ToString();
			
			bool PagingOn = true;
			if(PagingOn && NumRows >= PageSize)
			{
				if(Common.AppConfig("PageNumberFormat").ToUpper() == "NEXT_PREV")
				{
					writer.Write("<p class=\"PageNumber\" align=\"left\">Showing items " + StartRow.ToString() + "-" + StopRow.ToString() + " of " + NumRows.ToString());
					if(NumPages > 1)
					{
						writer.Write(" (");
						if(PageNum > 1)
						{
							writer.Write("<a href=\"prices.aspx?" + QueryParms + "&pagenum=1\">First Page</a>");
							writer.Write(" | ");
							writer.Write("<a class=\"PageNumber\" href=\"prices.aspx?" + QueryParms + "&pagenum=" + (PageNum-1).ToString() + "\">Previous Page</a>");
						}
						if(PageNum > 1 && PageNum < NumPages)
						{
							writer.Write(" | ");
						}
						if(PageNum < NumPages)
						{
							writer.Write("<a class=\"PageNumber\" href=\"prices.aspx?" + QueryParms + "&pagenum=" + (PageNum+1).ToString() + "\">Next Page</a>");
							writer.Write(" | ");
							writer.Write("<a href=\"prices.aspx?" + QueryParms + "&pagenum=" + NumPages.ToString() + "\">Last Page</a>");
						}
						writer.Write(")");
						writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;Click <a class=\"PageNumber\" href=\"prices.aspx?" + QueryParms + "&show=all\">here</a> to see all items");
					}
				}
				else
				{
					writer.Write("<p class=\"PageNumber\" align=\"left\">");
					if(Common.QueryString("show") == "all")
					{
						writer.Write("Click <a class=\"PageNumber\" href=\"prices.aspx?" + QueryParms + "&pagenum=1\">here</a> to turn paging back on.");
					}
					else
					{
						writer.Write("Page: ");
						for(int u = 1; u <= NumPages; u++)
						{
							if(u % 35 == 0)
							{
								writer.Write("<br>");
							}
							if(u == PageNum)
							{
								writer.Write(u.ToString() + "&nbsp;&nbsp;");
							}
							else
							{
								writer.Write("<a class=\"PageNumber\" href=\"prices.aspx?" + QueryParms + "&pagenum=" + u.ToString() + "\">" + u.ToString() + "</a>&nbsp;&nbsp;");
							}
						}
						writer.Write("&nbsp;&nbsp;<a class=\"PageNumber\" href=\"prices.aspx?" + QueryParms + "&show=all\">all</a>");
					}
				}
				writer.Write("</p>\n");
			}

			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function PriceForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");

			writer.Write("<form id=\"PriceForm\" name=\"PriceForm\" method=\"POST\" action=\"prices.aspx?categoryid=" + CategoryID.ToString() + "&producttypeid=" + ProductTypeID.ToString() + "&manufacturerID=" + ManufacturerID.ToString() + "&pagenum=" + PageNum.ToString() + "\" onsubmit=\"return (validateForm(this) && PriceForm_Validator(this))\" >\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td><b>Product ID</b></td>\n");
			writer.Write("      <td><b>Variant ID</b></td>\n");
			writer.Write("      <td><b>Product</b></td>\n");
			writer.Write("      <td><b>SKU</b></td>\n");
			writer.Write("      <td><b>Price</b></td>\n");
			writer.Write("      <td><b>SalePrice</b></td>\n");
			writer.Write("    </tr>\n");
			for(int rowN = StartRow; rowN <= StopRow; rowN++)
			{
				DataRow row = ds.Tables[0].Rows[rowN-1];
				writer.Write("<tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
				writer.Write("<td >" + DB.RowFieldInt(row,"ProductID").ToString() + "</td>\n");
				writer.Write("<td >" + DB.RowFieldInt(row,"VariantID").ToString() + "</td>\n");
				writer.Write("<td >");

				String Image1URL = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",_siteID);
				if(Image1URL.Length != 0)
				{

					writer.Write("<a href=\"editProduct.aspx?Productid=" + DB.RowFieldInt(row,"ProductID").ToString() + "\">");
					writer.Write("<img src=\"" + Image1URL + "\" height=\"35\" border=\"0\" align=\"absmiddle\">");
					writer.Write("</a>&nbsp;\n");
				}
				writer.Write("<a href=\"editProduct.aspx?Productid=" + DB.RowFieldInt(row,"ProductID").ToString() + "\">");
				writer.Write(DB.RowField(row,"Name"));
				if(DB.RowField(row,"VName").Length != 0)
				{
					writer.Write(" - ");
				}
				writer.Write(DB.RowField(row,"VName"));
				writer.Write("</a>");

				writer.Write("</a>");
				writer.Write("</td>\n");
				writer.Write("<td>" + DB.RowField(row,"SKU") +  DB.RowField(row,"SKUSuffix") + "</td>\n");
				writer.Write("<td>");
				writer.Write("<input name=\"XPrice_" + DB.RowFieldInt(row,"VariantID").ToString() + "\" type=\"text\" size=\"10\" value=\"" + Localization.CurrencyStringForDB( DB.RowFieldDecimal(row,"Price")) + "\">");
				writer.Write("<input name=\"XPrice_" + DB.RowFieldInt(row,"VariantID").ToString() + "_vldt\" type=\"hidden\" value=\"[number][invalidalert=please enter a valid dollar amount]\">");
				writer.Write("</td>\n");
				writer.Write("<td>");
				writer.Write("<input name=\"YPrice_" + DB.RowFieldInt(row,"VariantID").ToString() + "\" type=\"text\" size=\"10\" value=\"" + Common.IIF(DB.RowFieldDecimal(row,"SalePrice") != System.Decimal.Zero , Localization.CurrencyStringForDB( DB.RowFieldDecimal(row,"SalePrice")) , "") + "\">");
				writer.Write("<input name=\"YPrice_" + DB.RowFieldInt(row,"VariantID").ToString() + "_vldt\" type=\"hidden\" value=\"[number][invalidalert=please enter a valid dollar amount]\">");
				writer.Write("</td>\n");
				writer.Write("</tr>\n");
			}
			ds.Dispose();
			writer.Write("</table>\n");
			writer.Write("<p align=\"right\"><input type=\"submit\" value=\"Update\" name=\"Submit\"></p>\n");
			writer.Write("</form>\n");
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
