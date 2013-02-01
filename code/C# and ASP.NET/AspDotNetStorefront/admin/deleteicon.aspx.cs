// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.IO;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for deleteicon.
	/// </summary>
	public class deleteicon : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");

			Customer thisCustomer = new Customer();
			String FormImageName = Common.QueryString("FormImageName");

			if(thisCustomer._isAdminUser)
			{
				int ManufacturerID = Common.QueryStringUSInt("ManufacturerID");
				int OrderOptionID = Common.QueryStringUSInt("OrderOptionID");
				int ProductID = Common.QueryStringUSInt("ProductID");
				int VariantID = Common.QueryStringUSInt("VariantID");
				int CategoryID = Common.QueryStringUSInt("CategoryID");
				int SectionID = Common.QueryStringUSInt("SectionID");
				int SubcategoryID = Common.QueryStringUSInt("SubcategoryID");
				int PartnerID = Common.QueryStringUSInt("PartnerID");
				int StaffID = Common.QueryStringUSInt("StaffID");
				int GalleryID = Common.QueryStringUSInt("GalleryID");
				String Size = Common.QueryString("Size").ToLower();

				Response.Write("<html><head><title>Delete Icon</title></head><body>\n");

				String ImagePath = String.Empty;
				try
				{
					if(ManufacturerID != 0)
					{
						System.IO.File.Delete(Common.GetImagePath("Manufacturer","",true) + ManufacturerID.ToString() + ".jpg");
						System.IO.File.Delete(Common.GetImagePath("Manufacturer","",true) + ManufacturerID.ToString() + ".gif");
						System.IO.File.Delete(Common.GetImagePath("Manufacturer","",true) + ManufacturerID.ToString() + ".png");
					}
					if(OrderOptionID != 0)
					{
						System.IO.File.Delete(Common.GetImagePath("OrderOption","",true) + OrderOptionID.ToString() + ".jpg");
						System.IO.File.Delete(Common.GetImagePath("OrderOption","",true) + OrderOptionID.ToString() + ".gif");
						System.IO.File.Delete(Common.GetImagePath("OrderOption","",true) + OrderOptionID.ToString() + ".png");
					}

					if(CategoryID != 0)
					{
						System.IO.File.Delete(Common.GetImagePath("Category","",true) + CategoryID.ToString() + ".jpg");
						System.IO.File.Delete(Common.GetImagePath("Category","",true) + CategoryID.ToString() + ".gif");
						System.IO.File.Delete(Common.GetImagePath("Category","",true) + CategoryID.ToString() + ".png");
					}

			
					if(SectionID != 0)
					{
						System.IO.File.Delete(Common.GetImagePath("Section","",true) + SectionID.ToString() + ".jpg");
						System.IO.File.Delete(Common.GetImagePath("Section","",true) + SectionID.ToString() + ".gif");
						System.IO.File.Delete(Common.GetImagePath("Section","",true) + SectionID.ToString() + ".png");
					}

			
					if(ProductID != 0)
					{

						String FN = ProductID.ToString();
						if(Common.AppConfigBool("UseSKUForProductImageName"))
						{
							IDataReader rs = DB.GetRS("select SKU from product  " + DB.GetNoLock() + " where productid=" + ProductID.ToString());
							if(rs.Read())
							{
								String SKU = DB.RSField(rs,"SKU").Trim();
								if(SKU.Length != 0)
								{
									FN = SKU;
								}
							}
							rs.Close();
						}
						Response.Write("Deleting File: " + Common.GetImagePath("Product",Size,true) + FN + ".jpg" + "<br>");

						System.IO.File.Delete(Common.GetImagePath("Product",Size,true) + FN + ".jpg");
						System.IO.File.Delete(Common.GetImagePath("Product",Size,true) + FN + ".gif");
						System.IO.File.Delete(Common.GetImagePath("Product",Size,true) + FN + ".png");
					}

					if(VariantID != 0)
					{
						System.IO.File.Delete(Common.GetImagePath("Variant",Size,true) + VariantID.ToString() + ".jpg");
						System.IO.File.Delete(Common.GetImagePath("Variant",Size,true) + VariantID.ToString() + ".gif");
						System.IO.File.Delete(Common.GetImagePath("Variant",Size,true) + VariantID.ToString() + ".png");
					}
					if(PartnerID != 0)
					{
						System.IO.File.Delete(Common.GetImagePath("Partner",Size,true) + PartnerID.ToString() + ".jpg");
						System.IO.File.Delete(Common.GetImagePath("Partner",Size,true) + PartnerID.ToString() + ".gif");
						System.IO.File.Delete(Common.GetImagePath("Partner",Size,true) + PartnerID.ToString() + ".png");
					}
					if(StaffID != 0)
					{
						System.IO.File.Delete(Common.GetImagePath("Staff",Size,true) + StaffID.ToString() + ".jpg");
						System.IO.File.Delete(Common.GetImagePath("Staff",Size,true) + StaffID.ToString() + ".gif");
						System.IO.File.Delete(Common.GetImagePath("Staff",Size,true) + StaffID.ToString() + ".png");
					}
					if(GalleryID != 0)
					{
						System.IO.File.Delete(Common.GetImagePath("Gallery",Size,true) + GalleryID.ToString() + ".jpg");
						System.IO.File.Delete(Common.GetImagePath("Gallery",Size,true) + GalleryID.ToString() + ".gif");
						System.IO.File.Delete(Common.GetImagePath("Gallery",Size,true) + GalleryID.ToString() + ".png");
					}

				}
				catch {}
				Response.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				Response.Write("opener.document.getElementById('" + FormImageName + "').src = '../images/spacer.gif';\n");
				Response.Write("self.close();\n");
				Response.Write("</script>\n");
			}
			else
			{
				Response.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				Response.Write("self.close();\n");
				Response.Write("</script>\n");
			}

			Response.Write("</body></html>\n");
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
