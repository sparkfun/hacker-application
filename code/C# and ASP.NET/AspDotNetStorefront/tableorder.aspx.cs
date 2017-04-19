// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for tableorder.
	/// </summary>
	public class tableorder : SkinBase
	{

		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "<span class=\"SectionTitleText\">Multiple Items Add Form:</span>";

		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			int CategoryID = Common.QueryStringUSInt("CategoryID");
			int SectionID = Common.QueryStringUSInt("SectionID");
			int ManufacturerID = Common.QueryStringUSInt("ManufacturerID");

			writer.Write("<div align=\"left\">\n");
			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function TableOrderForm_Validator(theForm)\n");
			writer.Write("	{\n");
			writer.Write("	return (true);\n");
			writer.Write("	}\n");
			writer.Write("</script>\n");

			//writer.Write("<form method=\"POST\" name=\"TableOrderForm\" id=\"TableOrderForm\" action=\"tableorder_process.aspx\" onsubmit=\"return validateForm(this) && TableOrderForm_Validator(this)\" >\n");
			writer.Write("<form method=\"POST\" name=\"TableOrderForm\" id=\"TableOrderForm\" action=\"tableorder_process.aspx\" >\n");
			String sql = "SELECT Product.ProductID, Product.HidePriceUntilCart, Product.IsCallToOrder, Product.Description, Product.ProductTypeID, Product.RelatedProducts, Product.HidePriceUntilCart, Product.Name, Product.SEName, Product.SpecTitle, Product.SpecsInline, Product.SpecCall, Product.ProductDisplayFormatID, Product.ColWidth, Product.Summary, Product.Description, Product.SEKeywords, Product.SEDescription, Product.SKU, Product.ManufacturerPartNumber, Product.Published, Product.Deleted, ProductVariant.VariantID, ProductVariant.FroogleDescription, ProductVariant.Name AS VariantName, ProductVariant.Colors, ProductVariant.Sizes, ProductVariant.Dimensions, ProductVariant.Weight, ProductVariant.Inventory, ProductVariant.SKUSuffix, ProductVariant.ManufacturerPartNumber AS VManufacturerPartNumber, ProductVariant.Price, ProductVariant.SalePrice, ProductVariant.Deleted AS VariantDeleted, ProductVariant.Description as VariantDescription, ProductVariant.Published AS VariantPublished, ProductVariant.price, ProductVariant.SalePrice FROM  Product " + DB.GetNoLock() + " ,ProductVariant  " + DB.GetNoLock();
			if(CategoryID != 0)
			{
				sql += ",categorydisplayorder";
			}
			if(SectionID != 0)
			{
				sql += ",sectiondisplayorder";
			}
			sql += " where Product.ProductID = ProductVariant.ProductID ";
			if(CategoryID != 0)
			{
				sql += " and product.productid=categorydisplayorder.productid and categorydisplayorder.CategoryID=" + CategoryID.ToString();
			}
			if(SectionID != 0)
			{
				sql += " and product.productid=sectiondisplayorder.productid and sectiondisplayorder.sectionid=" + SectionID.ToString();
			}
			sql += " and (Product.RequiresRegistration IS NULL or Product.RequiresRegistration=0) and product.published=1 and Product.Deleted=0 and ProductVariant.Published=1 AND ProductVariant.Deleted=0 and Product.IsAKit=0 and Product.IsAPack=0 ";
			if(CategoryID != 0)
			{
				sql += " and product.productid in (select distinct productid from productcategory  " + DB.GetNoLock() + " where CategoryID=" + CategoryID.ToString() + ")";
			}
			if(SectionID != 0)
			{
				sql += " and product.productid in (select distinct productid from productsection  " + DB.GetNoLock() + " where SectionID=" + SectionID.ToString() + ")";
			}
			if(ManufacturerID != 0)
			{
				sql += " and product.ManufacturerID=" + ManufacturerID.ToString();
			}
			if(Common.AppConfigBool("FilterProductsByAffiliate"))
			{
				sql += " and Product.ProductID in (select distinct productid from productaffiliate  " + DB.GetNoLock() + " where affiliateid=" + thisCustomer._affiliateID.ToString() + ")";
			}
			if(Common.AppConfigBool("FilterProductsByCustomerLevel"))
			{
				String FilterOperator = Common.IIF(Common.AppConfigBool("FilterByCustomerLevelIsAscending"),"<=","=");
				sql += " and (Product.ProductID in (select distinct productid from productCustomerLevel  " + DB.GetNoLock() + " where CustomerLevelid" + FilterOperator + thisCustomer._customerLevelID.ToString() + ") ";
				if(Common.AppConfigBool("FilterByCustomerLevel0SeesUnmappedProducts"))
				{
					// allow customer level 0 to see any product that is not specifically mapped to any customer levels
					sql += " or Product.ProductID not in (select productid from productcustomerlevel)";
				}
				sql += ")";
			}
			if(CategoryID != 0)
			{
				sql += " order by categorydisplayorder.displayorder, product.name, ProductVariant.DisplayOrder, ProductVariant.Name";
			}
			else if(SectionID != 0)
			{
				sql += " order by sectiondisplayorder.displayorder, product.name, ProductVariant.DisplayOrder, ProductVariant.Name";
			}
			else
			{
				sql += " ORDER by Product.Name, ProductVariant.DisplayOrder, ProductVariant.Name";
			}
			//writer.Write("SQL=" + sql + "<br>");
			IDataReader rs = DB.GetRS(sql);

			writer.Write("<div align=\"left\">This page lets you easily add <b>multiple</b> items to your cart all at one time.<br><br>For each style, you can enter a quantity by color/size that you want to add to your " + Common.AppConfig("CartPrompt").ToLower() + " and then click on the <b>\"" + Common.AppConfig("CartButtonPrompt") + "\"</b> button at the bottom of the page.<br>&nbsp;</div>");
			writer.Write("<table cellpadding=\"4\" border=\"0\" cellspacing=\"0\">");

			int CustomerLevelID = thisCustomer._customerLevelID;
			bool CustomerLevelAllowsQuantityDiscounts = Common.CustomerLevelAllowsQuantityDiscounts(CustomerLevelID);
			String CustomerLevelName = Common.GetCustomerLevelName(CustomerLevelID);

			bool anyFound = false;
			while(rs.Read())
			{
				anyFound = true;

				int ProductID = DB.RSFieldInt(rs,"ProductID");
				int ActiveDIDID = Common.LookupActiveProductQuantityDiscountID(ProductID);
				bool ActiveDID = (ActiveDIDID != 0);
				if(!CustomerLevelAllowsQuantityDiscounts)
				{
					ActiveDIDID = 0;
					ActiveDID = false;
				}
				
				String ProductName = DB.RSField(rs,"Name") + Common.IIF(DB.RSField(rs,"VariantName").Length != 0, " - " + DB.RSField(rs,"VariantName"), "");
				String ProductDescription = DB.RSField(rs,"Description"); //.Replace("\n","<br>");
				String FileDescription = new ProductDescriptionFile(ProductID,thisCustomer._localeSetting,_siteID)._contents;
				if(FileDescription.Length != 0)
				{
					ProductDescription += "<div align=\"left\">" + FileDescription + "</div>";
				}
				String ProductPicture = Common.LookupImage("Product",ProductID,"medium",_siteID);
				String LargePic = Common.LookupImage("Product",ProductID,"large",_siteID);
				bool HasLargePic = (LargePic.Length != 0);
				if(ProductPicture.Length == 0)
				{
					ProductPicture = Common.LookupImage("Product",ProductID,"icon",_siteID);
				}
				if(ProductPicture.Length == 0)
				{
					ProductPicture = Common.AppConfig("NoPicture");
				}

				String LargeImage = String.Empty;
				if(HasLargePic)
				{
					LargeImage = LargePic;
					int LargePicW = Common.GetImageWidth(LargeImage);
					int LargePicH = Common.GetImageHeight(LargeImage);
					writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
					writer.Write("function popup" + ProductID.ToString() + "_" + DB.RSFieldInt(rs,"VariantID").ToString() + "(url)\n");
					writer.Write("	{\n");
					writer.Write("	window.open('popup.aspx?src=' + url,'LargerImage" + ProductID.ToString() + Common.GetRandomNumber(1,100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no,copyhistory=no,width=" + LargePicW.ToString() + ",height=" + LargePicH.ToString() + ",left=0,top=0');\n");
					writer.Write("	return (true);\n");
					writer.Write("	}\n");
					writer.Write("</script>\n");
				}


				writer.Write("<tr>");
				writer.Write("<td align=\"center\" valign=\"top\" >");
				if(ProductPicture.Length != 0)
				{
					writer.Write("<img " + Common.IIF(HasLargePic,"style=\"cursor: hand;\" alt=\"View Larger Image\" onClick=\"javascript:popup" + ProductID.ToString() + "_" + DB.RSFieldInt(rs,"VariantID").ToString() + "('" + Server.UrlEncode(LargePic) + "');\"","") + " src=\"" + ProductPicture + "?" + Common.GetRandomNumber(1,100000).ToString() + "\">");
					writer.Write("<br>");
					writer.Write("");
					if(HasLargePic) 
					{
						writer.Write("<a href=\"javascript:void(0);\" onClick=\"javascript:popup" + ProductID.ToString() + "_" + DB.RSFieldInt(rs,"VariantID").ToString() + "('" + Server.UrlEncode(LargePic) + "');\">View Larger Image</a><br>");
					}
				}

				writer.Write("</td>");
				writer.Write("<td align=\"left\" valign=\"top\" >");

				writer.Write("<span class=\"ProductNameText\">" + ProductName + "</span><br><br>\n");
				writer.Write("<div align=\"left\">");
				writer.Write("SKU: " + Common.MakeProperProductSKU(DB.RSField(rs,"SKU"),DB.RSField(rs,"SKUSuffix"),"",""));
				writer.Write("</div>");
				writer.Write("<br>");

				writer.Write("<div align=\"left\">");
				writer.Write(ProductDescription);
				writer.Write("</div>");
				
				writer.Write("<div align=\"left\">");
				if(!DB.RSFieldBool(rs,"HidePriceUntilCart"))
				{
					writer.Write("<table>");
					if(CustomerLevelID == 0)
					{
						// show consumer (e.g. level 0) pricing:
						if(DB.RSFieldDecimal(rs,"SalePrice") != System.Decimal.Zero)
						{
							writer.Write("<tr valign=\"top\"><td width=150><b>Price:</b></td><td><b><strike>" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</strike>&nbsp;&nbsp;&nbsp;<font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + DB.RSField(rs,"sDescription") + ":</font> <font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"SalePrice")) + "</font></b></td></tr>");
							writer.Write("<tr valign=\"top\"><td colspan=2 height=10></td></tr>");
						}
						else
						{
							writer.Write("<tr valign=\"top\"><td width=150><b>Price:</b></td><td><b>" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</b></td></tr>");
							writer.Write("<tr valign=\"top\"><td colspan=2 height=10></td></tr>");
						}
					}
					else
					{
						// show level pricing:
						bool IsOnSale = false;
						decimal LevelPrice = Common.DetermineLevelPrice(DB.RSFieldInt(rs,"VariantID"),CustomerLevelID, out IsOnSale);
						writer.Write("<tr valign=\"top\"><td width=150><b>Regular Price:</b></td><td><b>" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</b></td></tr>");
						writer.Write("<tr valign=\"top\"><td colspan=2 height=10></td></tr>");
						writer.Write("<tr valign=\"top\"><td width=150><b><font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + CustomerLevelName + " Price:</font></b></td><td><b><font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + Localization.CurrencyStringForDisplay( LevelPrice) + "</font></b></td></tr>");
						writer.Write("<tr valign=\"top\"><td colspan=2 height=10></td></tr>");
					}
					writer.Write("</table>");
				}
				writer.Write("</div>");

				
				String Sizes = DB.RSField(rs,"Sizes");
				String Colors = DB.RSField(rs,"Colors");

				String[] ColorsSplit = Colors.Split(',');
				String[] SizesSplit = Sizes.Split(',');
			
				writer.Write("<table border=\"0\" cellpadding=\"1\" cellspacing=\"1\" style=\"border-style: solid; border-color: #EEEEEE; border-width: 1px;\">\n");
				writer.Write("<tr>\n");
				writer.Write("<td valign=\"middle\" align=\"left\"><b>Color/Size</b></td>\n");
				for(int i = SizesSplit.GetLowerBound(0); i <= SizesSplit.GetUpperBound(0); i++)
				{
					writer.Write("<td valign=\"middle\" align=\"center\">" + SizesSplit[i].Trim() + "</td>\n");
				}
				writer.Write("</tr>\n");
				for(int i = ColorsSplit.GetLowerBound(0); i <= ColorsSplit.GetUpperBound(0); i++)
				{
					String bgcolor = Common.IIF(i % 2 == 0 , "#FFFFFF" , "#EEEEEE");
					writer.Write("<tr>\n");
					writer.Write("<td valign=\"middle\" align=\"right\" bgcolor=\"" + bgcolor + "\">" + ColorsSplit[i].Trim() + "</td>\n");
					for(int j = SizesSplit.GetLowerBound(0); j <= SizesSplit.GetUpperBound(0); j++)
					{
						writer.Write("<td valign=\"middle\" align=\"center\" bgcolor=\"" + bgcolor + "\">");
						String FldName = DB.RSFieldInt(rs,"ProductID").ToString() + "_" + DB.RSFieldInt(rs,"VariantID").ToString() + "_" + i.ToString() + "_" + j.ToString();
						//writer.Write(FldName);
						writer.Write("<input name=\"Qty_" + FldName + "\" type=\"text\" size=\"3\" maxlength=\"3\">");
						//writer.Write("<input name=\"Qty_" + FldName + "_vldt\" type=\"hidden\" value=\"[number][invalidalert=Please enter a single number, e.g. 1, 4, 10]\">");
						writer.Write("</td>\n");
					}
					writer.Write("</tr>\n");
				}
				writer.Write("</table>\n");
				writer.Write("</td>");
				writer.Write("</tr>");
				writer.Write("<tr><td colspan=2 height=15><hr size=1></td></tr>");
			}
			rs.Close();
			writer.Write("</table>");

			writer.Write("<br><br>");
			if(anyFound)
			{
				writer.Write("<center><input type=\"Submit\" value=\"" + Common.AppConfig("CartButtonPrompt") + "\"></center>");
			}
			else
			{
				writer.Write("<center><b>No Products Found</b></center>");
			}
			writer.Write("</form>");
			writer.Write("</div>");
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
