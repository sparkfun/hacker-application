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
using System.Web;
using System.IO;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for manufacturers.
	/// </summary>
	public class manufacturers : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Manage Manufacturers (Brands)";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(Common.QueryString("DeleteID").Length != 0)
			{
				// delete the mfg:
				DB.ExecuteSQL("update manufacturer set deleted=1, LastUpdatedBy=" + thisCustomer._customerID.ToString() + " where ManufacturerID=" + Common.QueryString("DeleteID"));
				Common.ClearCache();
				// delete any images:
				try
				{
					System.IO.File.Delete(Common.GetImagePath("Manufacturer","icon",true) + Common.QueryStringUSInt("DeleteID").ToString() + ".jpg");
					System.IO.File.Delete(Common.GetImagePath("Manufacturer","icon",true) + Common.QueryStringUSInt("DeleteID").ToString() + ".gif");
					System.IO.File.Delete(Common.GetImagePath("Manufacturer","icon",true) + Common.QueryStringUSInt("DeleteID").ToString() + ".png");
					System.IO.File.Delete(Common.GetImagePath("Manufacturer","medium",true) + Common.QueryStringUSInt("DeleteID").ToString() + ".jpg");
					System.IO.File.Delete(Common.GetImagePath("Manufacturer","medium",true) + Common.QueryStringUSInt("DeleteID").ToString() + ".gif");
					System.IO.File.Delete(Common.GetImagePath("Manufacturer","medium",true) + Common.QueryStringUSInt("DeleteID").ToString() + ".png");
					System.IO.File.Delete(Common.GetImagePath("Manufacturer","large",true) + Common.QueryStringUSInt("DeleteID").ToString() + ".jpg");
					System.IO.File.Delete(Common.GetImagePath("Manufacturer","large",true) + Common.QueryStringUSInt("DeleteID").ToString() + ".gif");
					System.IO.File.Delete(Common.GetImagePath("Manufacturer","large",true) + Common.QueryStringUSInt("DeleteID").ToString() + ".png");
				}
				catch {}
			}

			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				for(int i = 0; i<=Request.Form.Count-1; i++)
				{
					if(Request.Form.Keys[i].IndexOf("DisplayOrder_") != -1)
					{
						String[] keys = Request.Form.Keys[i].Split('_');
						int ManufacturerID = Localization.ParseUSInt(keys[1]);
						int DispOrd = 1;
						try
						{
							DispOrd = Localization.ParseUSInt(Request.Form[Request.Form.Keys[i]]);
						}
						catch {}
						DB.ExecuteSQL("update manufacturer set DisplayOrder=" + DispOrd.ToString() + " where ManufacturerID=" + ManufacturerID.ToString());
					}
				}
			}
			
			DataSet ds = DB.GetDS("select * from manufacturer  " + DB.GetNoLock() + " where deleted=0 order by displayorder,name",false,System.DateTime.Now.AddDays(1));
			writer.Write("<form method=\"POST\" action=\"manufacturers.aspx\">\n");
			writer.Write("<p align=\"left\"><input type=\"button\" value=\"Add New Manufacturer\" name=\"AddNew\" onClick=\"self.location='editmanufacturer.aspx';\"><p>");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td ><b>ID</b></td>\n");
			writer.Write("      <td ><b>Manufacturer</b></td>\n");
			writer.Write("      <td ><b>Web Site</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Display Order</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Edit</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Products</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete</b></td>\n");
			writer.Write("    </tr>\n");
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
				writer.Write("      <td>" + DB.RowFieldInt(row,"ManufacturerID").ToString() + "</td>\n");
				writer.Write("      <td >");
				String Image1URL = Common.LookupImage("Manufacturer",DB.RowFieldInt(row,"ManufacturerID"),"",_siteID);
				if(Image1URL.Length != 0)
				{
					writer.Write("<a href=\"editManufacturer.aspx?Manufacturerid=" + DB.RowFieldInt(row,"ManufacturerID").ToString() + "\">");
					writer.Write("<img src=\"" + Image1URL + "\" height=\"35\" border=\"0\" align=\"absmiddle\">");
					writer.Write("</a>&nbsp;\n");
				}
				writer.Write("      <a href=\"editManufacturer.aspx?Manufacturerid=" + DB.RowFieldInt(row,"ManufacturerID").ToString() + "\">");
				writer.Write(DB.RowField(row,"Name"));
				writer.Write("</a>");
				writer.Write("</td>\n");
				writer.Write("<td align=\"left\">");
				if(DB.RowField(row,"URL").Length != 0)
				{
					writer.Write("<a href=\"" + DB.RowField(row,"URL") + "\" target=\"_blank\">");
					writer.Write(DB.RowField(row,"URL"));
					writer.Write("</a>");
				}
				writer.Write("</td>\n");
				writer.Write("      <td align=\"center\"><input size=2 type=\"text\" name=\"DisplayOrder_" + DB.RowFieldInt(row,"ManufacturerID").ToString() + "\" value=\"" + DB.RowFieldInt(row,"DisplayOrder").ToString() + "\"></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Edit\" name=\"Edit_" + DB.RowFieldInt(row,"ManufacturerID").ToString() + "\" onClick=\"self.location='editmanufacturer.aspx?manufacturerid=" + DB.RowFieldInt(row,"ManufacturerID").ToString() + "'\"></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Products\" name=\"Products_" + DB.RowFieldInt(row,"ManufacturerID").ToString() + "\" onClick=\"self.location='products.aspx?manufacturerid=" + DB.RowFieldInt(row,"ManufacturerID").ToString() + "&producttypeid=0'\"></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Delete\" name=\"Delete_" + DB.RowFieldInt(row,"ManufacturerID").ToString() + "\" onClick=\"DeleteManufacturer(" + DB.RowFieldInt(row,"ManufacturerID").ToString() + ")\"></td>\n");
				writer.Write("    </tr>\n");
			}
			ds.Dispose();
			writer.Write("    <tr>\n");
			writer.Write("      <td colspan=\"3\" align=\"left\"></td>\n");
			writer.Write("      <td align=\"center\" bgcolor=\"" + Common.AppConfig("LightCellColor") + "\"><input type=\"submit\" value=\"Update\" name=\"Submit\"></td>\n");
			writer.Write("      <td colspan=\"3\"></td>\n");
			writer.Write("    </tr>\n");
			writer.Write("    <tr>\n");
			writer.Write("      <td colspan=\"7\" height=5></td>\n");
			writer.Write("    </tr>\n");
			writer.Write("  </table>\n");
			writer.Write("<p align=\"left\"><input type=\"button\" value=\"Add New Manufacturer\" name=\"AddNew\" onClick=\"self.location='editmanufacturer.aspx';\"><p>");
			writer.Write("</form>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteManufacturer(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete manufacturer: ' + id))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'manufacturers.aspx?deleteid=' + id;\n");
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
