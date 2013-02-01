// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for displayorder.
	/// </summary>
	public class displayorder : SkinBase
	{
		
		int CategoryID;
		int SectionID;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			CategoryID = Common.QueryStringUSInt("CategoryID");
			SectionID = Common.QueryStringUSInt("SectionID");

			if(CategoryID == 0 && SectionID == 0)
			{
				Response.Redirect("default.aspx");
			}

			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				if(CategoryID != 0)
				{
					DB.ExecuteSQL("delete from CategoryDisplayOrder where CategoryID=" + CategoryID.ToString());
				}
				if(SectionID != 0)
				{
					DB.ExecuteSQL("delete from SectionDisplayOrder where SectionID=" + SectionID.ToString());
				}

				for(int i = 0; i<=Request.Form.Count-1; i++)
				{
					if(Request.Form.Keys[i].IndexOf("DisplayOrder_") != -1)
					{
						String[] keys = Request.Form.Keys[i].Split('_');
						int ProductID = Localization.ParseUSInt(keys[1]);
						int DispOrd = 1;
						try
						{
							DispOrd = Localization.ParseUSInt(Request.Form[Request.Form.Keys[i]]);
						}
						catch {}
						if(CategoryID != 0)
						{
							DB.ExecuteSQL("insert into CategoryDisplayOrder(CategoryID,ProductID,DisplayOrder) values(" + CategoryID.ToString() + "," + ProductID.ToString() + "," + DispOrd.ToString() + ")");
						}
						if(SectionID != 0)
						{
							DB.ExecuteSQL("insert into SectionDisplayOrder(SectionID,ProductID,DisplayOrder) values(" + SectionID.ToString() + "," + ProductID.ToString() + "," + DispOrd.ToString() + ")");
						}
					}
				}
			}
			SectionTitle = "Set Display Order";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write("<p>TEST TEST TEST</p>");
			String sql = String.Empty;
			String prompt = String.Empty;
			if(CategoryID != 0)
			{
				sql = "select Product.*,DisplayOrder from Product  " + DB.GetNoLock() + " left outer join CategoryDisplayOrder  " + DB.GetNoLock() + " on product.productid=CategoryDisplayOrder.productid where CategoryDisplayOrder.categoryid=" + CategoryID.ToString() + " and deleted=0 " + Common.IIF(CategoryID != 0 , " and product.ProductID in (select distinct productid from productcategory  " + DB.GetNoLock() + " where categoryid=" + CategoryID.ToString() + ")" , "") + Common.IIF(SectionID != 0 , " and product.ProductID in (select distinct productid from productsection  " + DB.GetNoLock() + " where SectionID=" + SectionID.ToString() + ")" , "") + " order by displayorder,name";
				prompt = "Setting Product Display Order for Category: " + Common.GetCategoryName(CategoryID);
			}
			else
			{
				sql = "select Product.*,DisplayOrder from Product  " + DB.GetNoLock() + " left outer join SectionDisplayOrder  " + DB.GetNoLock() + " on product.productid=SectionDisplayOrder.productid where SectionDisplayOrder.SectionID=" + SectionID.ToString() + " and deleted=0 " + Common.IIF(CategoryID != 0 , " and product.ProductID in (select distinct productid from productcategory  " + DB.GetNoLock() + " where categoryid=" + CategoryID.ToString() + ")" , "") + Common.IIF(SectionID != 0 , " and product.ProductID in (select distinct productid from productsection  " + DB.GetNoLock() + " where SectionID=" + SectionID.ToString() + ")" , "") + " order by displayorder,name";
				prompt = "Setting Product Display Order for " + Common.AppConfig("SectionPromptSingular") + ": " + Common.GetSectionName(SectionID);
			}
			DataSet ds = DB.GetDS(sql,false,System.DateTime.Now.AddDays(1));

			writer.Write("<p><b>" + prompt + "</b></p>");

			writer.Write("<form id=\"Form1\" name=\"Form1\" method=\"POST\" action=\"displayorder.aspx?categoryid=" + CategoryID.ToString() + "&sectionid=" + SectionID.ToString() + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td><b>ID</b></td>\n");
			writer.Write("      <td><b>Product</b></td>\n");
			writer.Write("      <td><b>SKU</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Display Order</b></td>\n");
			writer.Write("    </tr>\n");
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
				writer.Write("      <td >" + DB.RowFieldInt(row,"ProductID").ToString() + "</td>\n");
				writer.Write("      <td >");

				String Image1URL = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",_siteID);
				if(Image1URL.Length == 0)
				{
					Image1URL = "../" + Common.AppConfig("NoPictureIcon");
				}

				writer.Write("<a href=\"editProduct.aspx?Productid=" + DB.RowFieldInt(row,"ProductID").ToString() + "\">");
				writer.Write("<img src=\"" + Image1URL + "\" height=\"25\" border=\"0\" align=\"absmiddle\">");
				writer.Write("</a>&nbsp;\n");
				writer.Write("<a href=\"editProduct.aspx?Productid=" + DB.RowFieldInt(row,"ProductID").ToString() + "\">");
				writer.Write(DB.RowField(row,"Name"));
				writer.Write("</a>");

				writer.Write("</a>");
				writer.Write("</td>\n");
				writer.Write("      <td >" + DB.RowField(row,"SKU") + "</td>\n");
				writer.Write("      <td align=\"center\"><input size=2 type=\"text\" name=\"DisplayOrder_" + DB.RowFieldInt(row,"ProductID").ToString() + "\" value=\"" + Common.IIF(DB.RowFieldInt(row,"DisplayOrder") == 0 , "1" , DB.RowField(row,"DisplayOrder")) + "\"></td>\n");
				writer.Write("    </tr>\n");
			}
			ds.Dispose();
			writer.Write("    <tr>\n");
			writer.Write("      <td colspan=\"3\" align=\"left\"></td>\n");
			writer.Write("      <td align=\"center\" bgcolor=\"" + Common.AppConfig("LightCellColor") + "\"><input type=\"submit\" value=\"Update\" name=\"Submit\"></td>\n");
			writer.Write("    </tr>\n");
			writer.Write("    <tr>\n");
			writer.Write("      <td colspan=\"4\" height=5></td>\n");
			writer.Write("    </tr>\n");
			writer.Write("  </table>\n");
			writer.Write("</form>\n");

			writer.Write("</center></b>\n");
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
