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
	/// Summary description for variants.
	/// </summary>
	public class variants : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "<a href=\"products.aspx\">Manage Products</a> - Add/Edit Variants";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			int ProductID = Common.QueryStringUSInt("ProductID");
			if(ProductID == 0)
			{
				Response.Redirect("products.aspx");
			}

			String ProductName = Common.GetProductName(ProductID);
			String ProductSKU = Common.GetProductSKU(ProductID);

			bool ProductUsesAdvancedInventoryMgmt = Common.ProductUsesAdvancedInventoryMgmt(ProductID);

			if(Common.QueryString("DeleteID").Length != 0)
			{
				// delete the mfg:
				DB.ExecuteSQL("update productvariant set deleted=1, LastUpdatedBy=" + thisCustomer._customerID.ToString() + " where VariantID=" + Common.QueryString("DeleteID"));
				Common.ClearCache();
			}

			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				for(int i = 0; i<=Request.Form.Count-1; i++)
				{
					if(Request.Form.Keys[i].IndexOf("DisplayOrder_") != -1)
					{
						String[] keys = Request.Form.Keys[i].Split('_');
						int VariantID = Localization.ParseUSInt(keys[1]);
						int DispOrd = 1;
						try
						{
							DispOrd = Localization.ParseUSInt(Request.Form[Request.Form.Keys[i]]);
						}
						catch {}
						DB.ExecuteSQL("update productvariant set DisplayOrder=" + DispOrd.ToString() + " where VariantID=" + VariantID.ToString());
					}
				}
			}
			
			writer.Write("<p align=\"left\"<b>Editing Variants for Product: <a href=\"editproduct.aspx?productid=" + ProductID.ToString() + "\">" + ProductName + "</a> (Product SKU=" + ProductSKU + ", ProductID=" + ProductID.ToString() + ")</b></p>\n");

			DataSet ds = DB.GetDS("select * from productvariant  " + DB.GetNoLock() + " where deleted=0 and ProductID=" + ProductID.ToString() + " order by displayorder,name",false,System.DateTime.Now.AddDays(1));

			writer.Write("<form id=\"Form1\" name=\"Form1\" method=\"POST\" action=\"variants.aspx?productid=" + ProductID.ToString() + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td><b>ID</b></td>\n");
			writer.Write("      <td><b>Variant</b></td>\n");
			writer.Write("      <td><b>Variant SKU Suffix</b></td>\n");
			writer.Write("      <td><b>Full SKU</b></td>\n");
			writer.Write("      <td><b>Price</b></td>\n");
			writer.Write("      <td><b>Sale Price</b></td>\n");
			if(ProductUsesAdvancedInventoryMgmt)
			{
				writer.Write("      <td align=\"center\"><b>Inventory</b></td>\n");
			}
			writer.Write("      <td align=\"center\"><b>Display Order</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Edit</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Move</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete</b></td>\n");
			writer.Write("    </tr>\n");
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
				writer.Write("      <td >" + DB.RowFieldInt(row,"VariantID").ToString() + "</td>\n");
				writer.Write("      <td >");
				String Image1URL = Common.LookupImage("Variant",DB.RowFieldInt(row,"VariantID"),"icon",_siteID);
				if(Image1URL.Length != 0)
				{

					writer.Write("<a href=\"editVariant.aspx?Variantid=" + DB.RowFieldInt(row,"VariantID").ToString() + "\">");
					writer.Write("<img src=\"" + Image1URL + "\" height=\"35\" border=\"0\" align=\"absmiddle\">");
					writer.Write("</a>&nbsp;\n");
				}
				writer.Write("<a href=\"editVariant.aspx?productid=" + ProductID.ToString() + "&Variantid=" + DB.RowFieldInt(row,"VariantID").ToString() + "\">" + Common.IIF(DB.RowField(row,"Name").Length == 0 , "(Blank)" , DB.RowField(row,"Name")) + "</a>");
				writer.Write("</td>\n");
				writer.Write("      <td >" + DB.RowField(row,"SKUSuffix") + "</td>\n");
				writer.Write("      <td >" + Common.GetProductSKU(ProductID) + DB.RowField(row,"SKUSuffix") + "</td>\n");
				writer.Write("      <td >" + Localization.CurrencyStringForDisplay( DB.RowFieldSingle(row,"Price")) + "</td>\n");
				writer.Write("      <td >" + Common.IIF(DB.RowFieldDecimal(row,"SalePrice") != System.Decimal.Zero , Localization.CurrencyStringForDisplay(DB.RowFieldDecimal(row,"SalePrice")) , "&nbsp;") + "</td>\n");
				if(ProductUsesAdvancedInventoryMgmt)
				{
					writer.Write("      <td  align=\"center\"><a href=\"editinventory.aspx?productid=" + ProductID.ToString() + "&variantid=" + DB.RowFieldInt(row,"VariantID").ToString() + "\">Click Here</a></td>\n");
				}
				writer.Write("      <td align=\"center\"><input size=2 type=\"text\" name=\"DisplayOrder_" + DB.RowFieldInt(row,"VariantID").ToString() + "\" value=\"" + DB.RowFieldInt(row,"DisplayOrder").ToString() + "\"></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Edit\" name=\"Edit_" + DB.RowFieldInt(row,"VariantID").ToString() + "\" onClick=\"self.location='editVariant.aspx?productid=" + ProductID.ToString() + "&Variantid=" + DB.RowFieldInt(row,"VariantID").ToString() + "'\"></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Move\" name=\"Move_" + DB.RowFieldInt(row,"VariantID").ToString() + "\" onClick=\"self.location='moveVariant.aspx?productid=" + ProductID.ToString() + "&Variantid=" + DB.RowFieldInt(row,"VariantID").ToString() + "'\"></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Delete\" name=\"Delete_" + DB.RowFieldInt(row,"VariantID").ToString() + "\" onClick=\"DeleteVariant(" + DB.RowFieldInt(row,"VariantID").ToString() + ")\"></td>\n");
				writer.Write("    </tr>\n");
			}
			ds.Dispose();
			writer.Write("    <tr>\n");
			writer.Write("      <td colspan=\"" + Common.IIF(ProductUsesAdvancedInventoryMgmt , 7 , 6).ToString() + "\" align=\"left\"></td>\n");
			writer.Write("      <td align=\"center\" bgcolor=\"" + Common.AppConfig("LightCellColor") + "\"><input type=\"submit\" value=\"Update\" name=\"Submit\"></td>\n");
			writer.Write("      <td colspan=\"3\"></td>\n");
			writer.Write("    </tr>\n");
			writer.Write("    <tr>\n");
			writer.Write("      <td colspan=\"10\" height=5></td>\n");
			writer.Write("    </tr>\n");
			writer.Write("  </table>\n");
			writer.Write(" <input type=\"button\" value=\"Add New Variant\" name=\"AddNew\" onClick=\"self.location='editVariant.aspx?productid=" + ProductID.ToString() + "';\">\n");
			writer.Write("</form>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteVariant(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete Variant: ' + id))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'Variants.aspx?productid=" + ProductID.ToString() + "&deleteid=' + id;\n");
			writer.Write("}\n");
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
