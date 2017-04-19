// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Xml;
using System.Configuration;
using System.Web.SessionState;
using System.Web.Caching;
using System.Web.Handlers;
using System.Web.Hosting;
using System.Web.Mail;
using System.Web.Security;
using System.Web.UI;
using System.Web.Util;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for products.
	/// </summary>
	public class products : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Manage Products";
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

			if(Common.QueryString("DeleteID").Length != 0)
			{
				DB.ExecuteSQL("delete from ShoppingCart where productid=" + Common.QueryString("DeleteID"));
				DB.ExecuteSQL("delete from kitcart where productid=" + Common.QueryString("DeleteID"));
				DB.ExecuteSQL("delete from customcart where productid=" + Common.QueryString("DeleteID"));
				DB.ExecuteSQL("update Product set deleted=1, LastUpdatedBy=" + thisCustomer._customerID.ToString() + " where ProductID=" + Common.QueryString("DeleteID"));
				Common.ClearCache();
			}
		
			writer.Write("<a href=\"products.aspx?categoryid=0&sectionid=0&producttypeid=0&manufacturerid=0\">RESET FILTERS</a><br>");
			writer.Write("<form id=\"FilterForm\" name=\"FilterForm\" method=\"GET\" action=\"products.aspx\">\n");

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



			String sql = String.Empty;
			if(CategoryID != 0 && SectionID != 0)
			{
				// section has priority over category for DO
				sql = "select p.*,do.DisplayOrder from Product P " + DB.GetNoLock() + " left outer join sectiondisplayorder DO  " + DB.GetNoLock() + " on p.productid=DO.productid where deleted=0 and do.sectionID=" + SectionID.ToString() + " " + Common.IIF(SectionID != 0 , " and p.ProductID in (select distinct productid from productsection  " + DB.GetNoLock() + " where SectionID=" + SectionID.ToString() + ")" , "") + Common.IIF(CategoryID != 0 , " and p.ProductID in (select distinct productid from productcategory  " + DB.GetNoLock() + " where categoryid=" + CategoryID.ToString() + ")" , "") + Common.IIF(ProductTypeID != 0 , " and ProductTypeID=" + ProductTypeID.ToString() , "") + Common.IIF(ManufacturerID != 0 , " and ManufacturerID=" + ManufacturerID.ToString() , "") + " order by do.displayorder,p.name";
			}
			if(CategoryID != 0 && SectionID == 0)
			{
				sql = "select p.*,do.DisplayOrder from Product P  " + DB.GetNoLock() + " left outer join categorydisplayorder DO  " + DB.GetNoLock() + " on p.productid=DO.productid where deleted=0 and do.categoryID=" + CategoryID.ToString() + " " + Common.IIF(CategoryID != 0 , " and p.ProductID in (select distinct productid from productcategory  " + DB.GetNoLock() + " where categoryid=" + CategoryID.ToString() + ")" , "") + Common.IIF(ProductTypeID != 0 , " and ProductTypeID=" + ProductTypeID.ToString() , "") + Common.IIF(ManufacturerID != 0 , " and ManufacturerID=" + ManufacturerID.ToString() , "") + " order by do.displayorder,p.name";
			}
			if(CategoryID == 0 && SectionID != 0)
			{
				sql = "select p.*,do.DisplayOrder from Product P  " + DB.GetNoLock() + " left outer join sectiondisplayorder DO  " + DB.GetNoLock() + " on p.productid=DO.productid where deleted=0 and do.sectionID=" + SectionID.ToString() + " " + Common.IIF(SectionID != 0 , " and p.ProductID in (select distinct productid from productsection " + DB.GetNoLock() + " where SectionID=" + SectionID.ToString() + ")" , "") + Common.IIF(ProductTypeID != 0 , " and ProductTypeID=" + ProductTypeID.ToString() , "") + Common.IIF(ManufacturerID != 0 , " and ManufacturerID=" + ManufacturerID.ToString() , "") + " order by do.displayorder,p.name";
			}
			if(CategoryID == 0 && SectionID == 0)
			{
				sql = "select p.* from Product P  " + DB.GetNoLock() + " where deleted=0 " + Common.IIF(ProductTypeID != 0 , " and ProductTypeID=" + ProductTypeID.ToString() , "") + Common.IIF(ManufacturerID != 0 , " and ManufacturerID=" + ManufacturerID.ToString() , "") + " order by p.name";
			}
			DataSet ds = DB.GetDS(sql,false,System.DateTime.Now.AddDays(1));

			String QueryParms = "categoryid=" + CategoryID.ToString() + "&sectionid=" + SectionID.ToString() + "&manufacturerid=" + ManufacturerID.ToString() + "&producttypeid=" + ProductTypeID.ToString();
			// --------------------------

			int PageSize = Common.AppConfigUSInt("Admin_ProductPageSize");
			if(PageSize == 0)
			{
				PageSize = 20;
			}
			bool pagingOn = true;
			if(PageSize == 0)
			{
				pagingOn = false;
				PageSize = 1000000;
			}
			int NumRows = ds.Tables[0].Rows.Count;
			int PageNum = Common.QueryStringUSInt("PageNum");
			if(PageNum == 0)
			{
				PageNum = 1;
			}
			if(Common.QueryString("show") == "all")
			{
				PageSize = NumRows;
				PageNum = 1;
			}
			int NumPages = (NumRows/PageSize) + Common.IIF(NumRows % PageSize == 0 , 0 , 1);
			if(pagingOn)
			{
				if(PageNum > NumPages)
				{
					if(NumRows > 0)
					{
						Response.Redirect("products.aspx?" + QueryParms + "&pagenum=" + (PageNum-1).ToString());
					}
				}
			}
			int StartRow = (PageSize*(PageNum-1)) + 1;
			int StopRow = StartRow + PageSize -1 ;
			if(StopRow > NumRows)
			{
				StopRow = NumRows;
			}
				
			//writer.Write("<p align=\"left\">Number of rows=" + ds.Tables[0].Rows.Count + "</p>\n");
			if(pagingOn && NumRows >= PageSize)
			{
				if(Common.AppConfig("PageNumberFormat").ToUpper() == "NEXT_PREV")
				{
					writer.Write("<p class=\"PageNumber\" align=\"left\">Showing items " + StartRow.ToString() + "-" + StopRow.ToString() + " of " + NumRows.ToString());
					if(NumPages > 1)
					{
						writer.Write(" (");
						if(PageNum > 1)
						{
							writer.Write("<a href=\"products.aspx?" + QueryParms + "&pagenum=1\">First Page</a>");
							writer.Write(" | ");
							writer.Write("<a class=\"PageNumber\" href=\"products.aspx?" + QueryParms + "&pagenum=" + (PageNum-1).ToString() + "\">Previous Page</a>");
						}
						if(PageNum > 1 && PageNum < NumPages)
						{
							writer.Write(" | ");
						}
						if(PageNum < NumPages)
						{
							writer.Write("<a class=\"PageNumber\" href=\"products.aspx?" + QueryParms + "&pagenum=" + (PageNum+1).ToString() + "\">Next Page</a>");
							writer.Write(" | ");
							writer.Write("<a href=\"products.aspx?" + QueryParms + "&pagenum=" + NumPages.ToString() + "\">Last Page</a>");
						}
						writer.Write(")");
						writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;Click <a class=\"PageNumber\" href=\"products.aspx?" + QueryParms + "&show=all\">here</a> to see all items");
					}
				}
				else
				{
					writer.Write("<p class=\"PageNumber\" align=\"left\">");
					if(Common.QueryString("show") == "all")
					{
						writer.Write("Click <a class=\"PageNumber\" href=\"products.aspx?" + QueryParms + "&pagenum=1\">here</a> to turn paging back on.");
					}
					else
					{
						writer.Write("Page: ");
						for(int u = 1; u <= NumPages; u++)
						{
							if(u % 15 == 0)
							{
								writer.Write("<br>");
							}
							if(u == PageNum)
							{
								writer.Write(u.ToString() + "&nbsp;&nbsp;");
							}
							else
							{
								writer.Write("<a class=\"PageNumber\" href=\"products.aspx?" + QueryParms + "&pagenum=" + u.ToString() + "\">" + u.ToString() + "</a>&nbsp;&nbsp;");
							}
						}
						writer.Write("&nbsp;&nbsp;<a class=\"PageNumber\" href=\"products.aspx?" + QueryParms + "&show=all\">all</a>");
					}
				}
				writer.Write("</p>\n");
			}
			
			// --------------------------

			writer.Write("<form id=\"Form1\" name=\"Form1\" method=\"POST\" action=\"products.aspx?categoryid=" + CategoryID.ToString() + "&producttypeid=" + ProductTypeID.ToString() + "&manufacturerID=" + ManufacturerID.ToString() + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("<p align=\"left\"><input type=\"button\" value=\"Add New Product\" name=\"AddNew\" onClick=\"self.location='editProduct.aspx?categoryid=" + CategoryID.ToString() + "&sectionid=" + SectionID.ToString() + "&manufacturerID=" + ManufacturerID.ToString() + "';\"></p>");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td><b>ID</b></td>\n");
			writer.Write("      <td><b>Product</b></td>\n");
			writer.Write("      <td><b>SKU</b></td>\n");
			writer.Write("      <td><b>Mfg Part #</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Inventory</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Edit</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Variants</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Ratings</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete</b></td>\n");
			writer.Write("    </tr>\n");
			
			int rowi = 1; // 1 based!
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				if(rowi >= StartRow && rowi <= StopRow)
				{
					writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
					writer.Write("      <td >" + DB.RowFieldInt(row,"ProductID").ToString() + "</td>\n");
					writer.Write("      <td >");

					String Image1URL = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",_siteID);
					if(Image1URL.Length == 0)
					{
						Image1URL = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"medium",_siteID);
						if(Image1URL.Length == 0)
						{
							Image1URL = "../" + Common.AppConfig("NoPictureIcon");
						}
					}

					writer.Write("<a href=\"editProduct.aspx?Productid=" + DB.RowFieldInt(row,"ProductID").ToString() + "&categoryid=" + CategoryID.ToString() + "&sectionID=" + SectionID.ToString() + "\">");
					writer.Write("<img src=\"" + Image1URL + "\" height=\"25\" border=\"0\" align=\"absmiddle\">");
					writer.Write("</a>&nbsp;\n");
					writer.Write("<a href=\"editProduct.aspx?Productid=" + DB.RowFieldInt(row,"ProductID").ToString() + "&categoryid=" + CategoryID.ToString() + "&sectionID=" + SectionID.ToString() + "\">");
					writer.Write(DB.RowField(row,"Name"));
					writer.Write("</a>");

					writer.Write("</a>");
					writer.Write("</td>\n");
					writer.Write("      <td >" + DB.RowField(row,"SKU") + "</td>\n");
					writer.Write("      <td >" + DB.RowField(row,"ManufacturerPartNumber") + "</td>\n");
					if(Common.ProductUsesAdvancedInventoryMgmt(DB.RowFieldInt(row,"ProductID")))
					{
						writer.Write("<td align=\"center\"><a href=\"editinventory.aspx?productid=" + DB.RowFieldInt(row,"ProductID").ToString() + "\">Inventory</a></td>\n");
					}
					else
					{
						writer.Write("<td align=\"center\">N/A</td>");
					}
					writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Edit\" name=\"Edit_" + DB.RowFieldInt(row,"ProductID").ToString() + "\" onClick=\"self.location='editProduct.aspx?Productid=" + DB.RowFieldInt(row,"ProductID").ToString() + "&categoryid=" + CategoryID.ToString() + "&sectionID=" + SectionID.ToString() + "'\"></td>\n");
					int NumVariants = DB.GetSqlN("select count(*) as N from productvariant  " + DB.GetNoLock() + " where deleted=0 and productid=" + DB.RowFieldInt(row,"ProductID").ToString());
					writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Variants (" + NumVariants.ToString() + ")\" name=\"Variants_" + DB.RowFieldInt(row,"ProductID").ToString() + "\" onClick=\"self.location='" + Common.IIF(NumVariants == 1 , "variants.aspx?Productid=" + DB.RowFieldInt(row,"ProductID").ToString() , "variants.aspx?Productid=" + DB.RowFieldInt(row,"ProductID").ToString()) + "'\"></td>\n");
					int NumRatings = DB.GetSqlN("select count(*) as N from rating  " + DB.GetNoLock() + " where productid=" + DB.RowFieldInt(row,"ProductID").ToString());
					writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Ratings (" + NumRatings.ToString() + ")\" name=\"Ratings_" + DB.RowFieldInt(row,"ProductID").ToString() + "\" onClick=\"self.location='productratings.aspx?Productid=" + DB.RowFieldInt(row,"ProductID").ToString() + "'\"></td>\n");
					writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Delete\" name=\"Delete_" + DB.RowFieldInt(row,"ProductID").ToString() + "\" onClick=\"DeleteProduct(" + DB.RowFieldInt(row,"ProductID").ToString() + ")\"></td>");
					writer.Write("    </tr>\n");
				}
				rowi++;
			}
			ds.Dispose();
			writer.Write("  </table>\n");
			writer.Write("<p align=\"left\"><input type=\"button\" value=\"Add New Product\" name=\"AddNew\" onClick=\"self.location='editProduct.aspx?categoryid=" + CategoryID.ToString() + "&sectionid=" + SectionID.ToString() + "&manufacturerID=" + ManufacturerID.ToString() + "';\"></p>");
			writer.Write("</form>\n");

			// ------------------------

			if(pagingOn && NumRows >= PageSize)
			{
				if(Common.AppConfig("PageNumberFormat").ToUpper() == "NEXT_PREV")
				{
					writer.Write("<p class=\"PageNumber\" align=\"left\">Showing items " + StartRow.ToString() + "-" + StopRow.ToString() + " of " + NumRows.ToString());
					if(NumPages > 1)
					{
						writer.Write(" (");
						if(PageNum > 1)
						{
							writer.Write("<a href=\"products.aspx?" + QueryParms + "&pagenum=1\">First Page</a>");
							writer.Write(" | ");
							writer.Write("<a class=\"PageNumber\" href=\"products.aspx?" + QueryParms + "&pagenum=" + (PageNum-1).ToString() + "\">Previous Page</a>");
						}
						if(PageNum > 1 && PageNum < NumPages)
						{
							writer.Write(" | ");
						}
						if(PageNum < NumPages)
						{
							writer.Write("<a class=\"PageNumber\" href=\"products.aspx?" + QueryParms + "&pagenum=" + (PageNum+1).ToString() + "\">Next Page</a>");
							writer.Write(" | ");
							writer.Write("<a href=\"products.aspx?" + QueryParms + "&pagenum=" + NumPages.ToString() + "\">Last Page</a>");
						}
						writer.Write(")");
						writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;Click <a class=\"PageNumber\" href=\"products.aspx?" + QueryParms + "&show=all\">here</a> to see all items");
					}
				}
				else
				{
					writer.Write("<p class=\"PageNumber\" align=\"left\">");
					if(Common.QueryString("show") == "all")
					{
						writer.Write("Click <a class=\"PageNumber\" href=\"products.aspx?" + QueryParms + "&pagenum=1\">here</a> to turn paging back on.");
					}
					else
					{
						writer.Write("Page: ");
						for(int u = 1; u <= NumPages; u++)
						{
							if(u % 15 == 0)
							{
								writer.Write("<br>");
							}
							if(u == PageNum)
							{
								writer.Write(u.ToString() + "&nbsp;&nbsp;");
							}
							else
							{
								writer.Write("<a class=\"PageNumber\" href=\"products.aspx?" + QueryParms + "&pagenum=" + u.ToString() + "\">" + u.ToString() + "</a>&nbsp;&nbsp;");
							}
						}
						writer.Write("&nbsp;&nbsp;<a class=\"PageNumber\" href=\"products.aspx?" + QueryParms + "&show=all\">all</a>");
					}
				}
				writer.Write("</p>\n");
			}

			// ------------------------

			writer.Write("</center></b>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteProduct(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete Product: ' + id))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'Products.aspx?deleteid=' + id;\n");
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
