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
	/// Summary description for search.
	/// </summary>
	public class search : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Search";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function SearchForm2_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("  if (theForm.SearchTerm.value.length < 3)\n");
			writer.Write("  {\n");
			writer.Write("    alert('Please enter at least 3 characters in the Search For field.');\n");
			writer.Write("    theForm.SearchTerm.focus();\n");
			writer.Write("    return (false);\n");
			writer.Write("  }\n");
			writer.Write("  return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");

			writer.Write("<form method=\"GET\" action=\"search.aspx\" onsubmit=\"return (validateForm(this) && SearchForm2_Validator(this))\" name=\"SearchForm2\">\n");
			writer.Write("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\n");
			writer.Write("      <tr align=\"left\">\n");
			writer.Write("        <td width=\"100%\" colspan=\"2\"><b><font color=\"#FF0000\">" + Common.QueryString("ErrorMsg") + "</font></b>\n");
			writer.Write("          Please enter the search text. This can be part of a product name, sku, or description, etc.:</td>\n");
			writer.Write("      </tr>\n");
			writer.Write("      <tr>\n");
			writer.Write("        <td height=\"10\" width=\"25%\"></td>\n");
			writer.Write("        <td height=\"10\" width=\"75%\"></td>\n");
			writer.Write("      </tr>\n");
			writer.Write("      <tr align=\"left\">\n");
			writer.Write("        <td width=\"25%\">Search For Word(s):</td>\n");
			writer.Write("        <td width=\"75%\">\n");
			writer.Write("          <input type=\"text\" name=\"SearchTerm\" size=\"25\" maxlength=\"70\" value=\"" + Server.HtmlEncode(Common.QueryString("SearchTerm")) + "\">\n");
			writer.Write("          <input type=\"hidden\" name=\"SearchTerm_vldt\" value=\"[req][blankalert=Please enter something to search for!]\">\n");
			writer.Write("          &nbsp;<input type=\"submit\" value=\"Search\" name=\"B1\"></td>\n");
			writer.Write("      </tr>\n");
			//			writer.Write("      <tr align=\"left\">\n");
			//			writer.Write("        <td width=\"25%\">Show Photo Icons:</td>\n");
			//			writer.Write("        <td width=\"75%\"><input type=\"checkbox\" name=\"ShowPics\" value=\"ON\"" + (Common.QueryString("ShowPics").Length != 0 , " checked" , "") + "></td>\n");
			//			writer.Write("      </tr>\n");
			//			writer.Write("      <tr align=\"left\">\n");
			//			writer.Write("        <td width=\"25%\">Search All Photo Variants:</td>\n");
			//			writer.Write("        <td width=\"75%\"><input type=\"checkbox\" name=\"SearchVariants\" value=\"ON\"" + (Common.QueryString("SearchVariants").Length != 0 , " checked" , "") + "></td>\n");
			//			writer.Write("      </tr>\n");
			//			writer.Write("      <tr align=\"left\">\n");
			//			writer.Write("        <td width=\"25%\">Search Type:</td>\n");
			//			writer.Write("        <td width=\"75%\"><select size=\"1\" name=\"MatchType\">\n");
			//			writer.Write("    <option" + (Common.QueryString("MatchType") == "Any Words" || Common.QueryString("MatchType").Length == 0 , " selected" , "") + ">Any Words</option>\n");
			//			writer.Write("    <option" + (Common.QueryString("MatchType") == "Exact Match" , " selected" , "") + ">Exact Match</option>\n");
			//			writer.Write("  </select></td>\n");
			//			writer.Write("      </tr>\n");
			writer.Write("    </table>\n");
			writer.Write("</form>\n");



			String st = Common.QueryString("SearchTerm").Trim();
			if(st.Length != 0)
			{
				String stlike = "%" + st + "%";
				String stquoted = DB.SQuote(stlike);


				// MATCHING CATEGORIES:
				bool anyFound = false;
				IDataReader rs = DB.GetRS("select * from category  " + DB.GetNoLock() + " where category.name like " + stquoted + " and published<>0 and deleted=0 order by displayorder,name");

				writer.Write("<table cellpadding=\"2\" cellspacing=\"0\" border=\"0\" width=\"100%\">\n");
				writer.Write("<tr><td style=\"filter:progid:DXImageTransform.Microsoft.Gradient(startColorStr='#FFFFFF', endColorStr='#6487DB', gradientType='1')\"><b>" + Common.AppConfig("CategoryPromptPlural").ToUpper() + " MATCHING: '" + st.ToUpper() + "'</b></font></td></tr>\n");
				while(rs.Read())
				{
					writer.Write("<tr><td>" + Common.GetCategoryBreadcrumb(DB.RSFieldInt(rs,"CategoryID")) + "</td></tr>");
					anyFound = true;
				}
				rs.Close();
				if(!anyFound)
				{
					writer.Write("<tr><td>No matches found</td></tr>\n");
				}
				writer.Write("<tr><td>&nbsp;</td></tr>\n");
				writer.Write("</table>\n");

				// MATCHING SECTIONS:
				anyFound = false;
				rs = DB.GetRS("select * from [Section]  " + DB.GetNoLock() + " where name like " + stquoted + " and published<>0 and deleted=0 order by displayorder,name");

				writer.Write("<table cellpadding=\"2\" cellspacing=\"0\" border=\"0\" width=\"100%\">\n");
				writer.Write("<tr><td style=\"filter:progid:DXImageTransform.Microsoft.Gradient(startColorStr='#FFFFFF', endColorStr='#6487DB', gradientType='1')\"><b>" + Common.AppConfig("SectionPromptPlural").ToUpper() + " MATCHING: '" + st.ToUpper() + "'</b></font></td></tr>\n");
				while(rs.Read())
				{
					writer.Write("<tr><td>" + Common.GetSectionBreadcrumb(DB.RSFieldInt(rs,"SectionID")) + "</td></tr>");
					anyFound = true;
				}
				rs.Close();
				if(!anyFound)
				{
					writer.Write("<tr><td>No matches found</td></tr>\n");
				}
				writer.Write("<tr><td>&nbsp;</td></tr>\n");
				writer.Write("</table>\n");

				// MATCHING MANUFACTURERS:
				anyFound = false;
				rs = DB.GetRS("select * from manufacturer  " + DB.GetNoLock() + " where name like " + stquoted + " and deleted=0");

				writer.Write("<table cellpadding=\"2\" cellspacing=\"0\" border=\"0\" width=\"100%\">\n");
				writer.Write("<tr><td style=\"filter:progid:DXImageTransform.Microsoft.Gradient(startColorStr='#FFFFFF', endColorStr='#6487DB', gradientType='1')\"><b>MANUFACTURERS MATCHING: '" + st.ToUpper() + "'</b></font></td></tr>\n");
				while(rs.Read())
				{
					writer.Write("<tr><td><a href=\"editmanufacturer.aspx?manufacturerid=" + DB.RSFieldInt(rs,"ManufacturerID").ToString() + "\">" + Common.HighlightTerm(DB.RSField(rs,"Name"),st) + "</a></td></tr>\n");
					anyFound = true;
				}
				rs.Close();
				if(!anyFound)
				{
					writer.Write("<tr><td>No matches found</td></tr>\n");
				}
				writer.Write("<tr><td>&nbsp;</td></tr>\n");
				writer.Write("</table>\n");

				// MATCHING PRODUCTS:
				anyFound = false;
					String sql = "SET CONCAT_NULL_YIELDS_NULL OFF;SELECT Product.ProductID, Product.Name, Product.SEName, product.summary, product.description, productvariant.description as variantdescription, product.sku+productvariant.skusuffix as FullSKU, Product.SKU, Product.ManufacturerID, Product.ManufacturerPartNumber, Product.Published, Product.Deleted, ProductVariant.VariantID, ProductVariant.Name AS VariantName, ProductVariant.SKUSuffix, ProductVariant.ManufacturerPartNumber AS VariantManufacturerPartNumber, ProductVariant.Price, ProductVariant.SalePrice, ProductVariant.Deleted AS VariantDeleted, ProductVariant.Published AS VariantPublished, Manufacturer.Name AS ManufacturerName, Manufacturer.SEName as ManufacturerSEName, category.name as CategoryName, category.SEName as CategorySEName, Category.CategoryID FROM (((Product  " + DB.GetNoLock() + " INNER JOIN Manufacturer  " + DB.GetNoLock() + " ON Product.ManufacturerID = Manufacturer.ManufacturerID) LEFT OUTER JOIN ProductVariant  " + DB.GetNoLock() + " ON Product.ProductID = ProductVariant.ProductID) left outer join productcategory  " + DB.GetNoLock() + " on product.productid=productcategory.productid) left outer join category  " + DB.GetNoLock() + " on productcategory.categoryid=category.categoryid WHERE Product.Published=1 AND Product.Deleted=0 AND ProductVariant.Published=1";
				sql += " and (";
				sql += " product.name like " + DB.SQuote("%" + st + "%");
				sql += " or product.manufacturerpartnumber like " + DB.SQuote("%" + st + "%");
				sql += " or productvariant.name like " + DB.SQuote("%" + st + "%");
				sql += " or productvariant.manufacturerpartnumber like " + DB.SQuote("%" + st + "%");
				sql += " or product.sku+productvariant.skusuffix like " + DB.SQuote("%" + st + "%");
				sql += ")";
				rs = DB.GetRS(sql);

				writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");

				writer.Write("<tr><td style=\"filter:progid:DXImageTransform.Microsoft.Gradient(startColorStr='#FFFFFF', endColorStr='#6487DB', gradientType='1')\" colspan=\"5\"><b>PRODUCTS MATCHING: '" + st.ToUpper() + "'</b></font></td></tr>\n");
				
				while(rs.Read())
				{
					if(!anyFound)
					{
						writer.Write("    <tr>\n");
						writer.Write("      <td align=\"left\"><font style=\"CondensedVariantText\"><b>Product</b></font></td>\n");
						writer.Write("      <td align=\"left\"><font style=\"CondensedVariantText\"><b>Variant</b></font></td>\n");
						writer.Write("      <td align=\"center\"><font style=\"CondensedVariantText\"><b>SKU</b></font></td>\n");
						writer.Write("      <td align=\"center\"><font style=\"CondensedVariantText\"><b>Category</b></font></td>\n");
						writer.Write("      <td align=\"center\"><font style=\"CondensedVariantText\"><b>Manufacturer</b></font></td>\n");
						writer.Write("    </tr>\n");
						//writer.Write("<tr><td colspan=\"4\">&nbsp;</td></tr>\n");
					}
					writer.Write("<tr>");
					writer.Write("<td align=\"left\" ><a href=\"editproduct.aspx?productid=" + DB.RSFieldInt(rs,"ProductID").ToString() + "\">" + Common.HighlightTerm(DB.RSField(rs,"Name"),st) + "</a></td>");
					writer.Write("<td align=\"left\" ><a href=\"editvariant.aspx?productid=" + DB.RSFieldInt(rs,"ProductID").ToString() + "&variantid=" + DB.RSFieldInt(rs,"VariantID").ToString() + "\">" + Common.HighlightTerm(DB.RSField(rs,"VariantName"),st) + "</a></td>");
					writer.Write("<td align=\"center\" >" + Common.HighlightTerm(DB.RSField(rs,"FullSKU"),st) + "</td>");
					writer.Write("<td align=\"center\" ><a href=\"editcategory.aspx?categoryid=" + DB.RSFieldInt(rs,"CategoryID").ToString() + "\">" + Common.HighlightTerm(DB.RSField(rs,"CategoryName"),st) + "</a></td>");
					writer.Write("<td align=\"center\" ><a href=\"editmanufacturer.aspx?manufacturerid=" + DB.RSFieldInt(rs,"ManufacturerID").ToString() + "\">" + Common.HighlightTerm(DB.RSField(rs,"ManufacturerName"),st) + "</a></td>");
					writer.Write("</tr>\n");
					anyFound = true;
				}
				rs.Close();
				if(!anyFound)
				{
					writer.Write("<tr><td colspan=\"4\">No matches found</td></tr>\n");
				}
				writer.Write("<tr><td colspan=\"4\">&nbsp;</td></tr>\n");
				writer.Write("</table>\n");

				// MATCHING PRODUCTS:
				anyFound = false;
				int pid = 0;
				try
				{
					pid = Localization.ParseUSInt(st);
					rs = DB.GetRS("select * from product  " + DB.GetNoLock() + " where productid=" + pid.ToString());

					writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");

					writer.Write("<tr><td style=\"filter:progid:DXImageTransform.Microsoft.Gradient(startColorStr='#FFFFFF', endColorStr='#6487DB', gradientType='1')\" colspan=\"4\"><b>PRODUCT IDS MATCHING: '" + st.ToUpper() + "'</b></font></td></tr>\n");
				
					while(rs.Read())
					{
						if(!anyFound)
						{
							writer.Write("    <tr>\n");
							writer.Write("      <td align=\"left\"><font style=\"CondensedVariantText\"><b>Product</b></font></td>\n");
							writer.Write("      <td align=\"left\"><font style=\"CondensedVariantText\"><b>Variant</b></font></td>\n");
							writer.Write("      <td align=\"center\"><font style=\"CondensedVariantText\"><b>SKU</b></font></td>\n");
							writer.Write("      <td align=\"center\"><font style=\"CondensedVariantText\"><b>Category</b></font></td>\n");
							writer.Write("      <td align=\"center\"><font style=\"CondensedVariantText\"><b>Manufacturer</b></font></td>\n");
							writer.Write("    </tr>\n");
							//writer.Write("<tr><td colspan=\"4\">&nbsp;</td></tr>\n");
						}
						writer.Write("<tr>");
						writer.Write("<td align=\"left\"><a href=\"editproduct.aspx?productid=" + DB.RSFieldInt(rs,"ProductID").ToString() + "\">" + Common.HighlightTerm(DB.RSField(rs,"Name"),st) + "</a></td>");
						writer.Write("<td align=\"left\"><a href=\"editvariant.aspx?productid=" + DB.RSFieldInt(rs,"ProductID").ToString() + "&variantid=" + DB.RSFieldInt(rs,"VariantID").ToString() + "\">" + Common.HighlightTerm(DB.RSField(rs,"VariantName"),st) + "</a></td>");
						writer.Write("<td align=\"center\">" + Common.HighlightTerm(DB.RSField(rs,"FullSKU"),st) + "</td>");
						writer.Write("<td align=\"center\"><a href=\"editcategory.aspx?categoryid=" + DB.RSFieldInt(rs,"CategoryID").ToString() + "\">" + Common.HighlightTerm(DB.RSField(rs,"CategoryName"),st) + "</a></td>");
						writer.Write("<td align=\"center\"><a href=\"editmanufacturer.aspx?manufacturerid=" + DB.RSFieldInt(rs,"ManufacturerID").ToString() + "\">" + Common.HighlightTerm(DB.RSField(rs,"ManufacturerName"),st) + "</a></td>");
						writer.Write("</tr>\n");
						anyFound = true;
					}
					rs.Close();
					if(!anyFound)
					{
						writer.Write("<tr><td colspan=\"4\">No matches found</td></tr>\n");
					}
					writer.Write("<tr><td colspan=\"4\">&nbsp;</td></tr>\n");
					writer.Write("</table>\n");
				}
				catch {}

			}
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
