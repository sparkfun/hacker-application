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
	/// Summary description for galleries.
	/// </summary>
	public class galleries : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Manage Galleries";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(Common.QueryString("DeleteID").Length != 0)
			{
				// delete the mfg:
				DB.ExecuteSQL("update gallery set deleted=1, LastUpdatedBy=" + thisCustomer._customerID.ToString() + " where GalleryID=" + Common.QueryString("DeleteID"));
				Common.ClearCache();

				// delete any images:
				try
				{
					System.IO.File.Delete(Common.GetImagePath("Gallery","",true) + Common.QueryStringUSInt("DeleteID").ToString() + ".jpg");
					System.IO.File.Delete(Common.GetImagePath("Gallery","",true) + Common.QueryStringUSInt("DeleteID").ToString() + ".gif");
					System.IO.File.Delete(Common.GetImagePath("Gallery","",true) + Common.QueryStringUSInt("DeleteID").ToString() + ".png");
				}
				catch {}

				// delete the gallery directory also!
				String GalleryDirName = Common.GetGalleryDir(Common.QueryStringUSInt("DeleteID"));
				String SFP = HttpContext.Current.Server.MapPath("../images/spacer.gif").Replace("images\\spacer.gif","images\\gallery") + "\\" + GalleryDirName;
				try
				{
					if(Directory.Exists(SFP))
					{
						String[] files_jpg = Directory.GetFiles(SFP,"*.jpg");
						foreach(String file in files_jpg)
						{
							System.IO.File.Delete(file);
						}
						String[] files_gif = Directory.GetFiles(SFP,"*.gif");
						foreach(String file in files_gif)
						{
							System.IO.File.Delete(file);
						}
						String[] files_png = Directory.GetFiles(SFP,"*.png");
						foreach(String file in files_png)
						{
							System.IO.File.Delete(file);
						}
						Directory.Delete(SFP);
					}
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
						int GalleryID = Localization.ParseUSInt(keys[1]);
						int DispOrd = 1;
						try
						{
							DispOrd = Localization.ParseUSInt(Request.Form[Request.Form.Keys[i]]);
						}
						catch {}
						DB.ExecuteSQL("update gallery set DisplayOrder=" + DispOrd.ToString() + " where GalleryID=" + GalleryID.ToString());
					}
				}
			}
			
			DataSet ds = DB.GetDS("select * from gallery  " + DB.GetNoLock() + " where deleted=0 order by displayorder,name",false,System.DateTime.Now.AddDays(1));
			writer.Write("<form method=\"POST\" action=\"galleries.aspx\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td ><b>ID</b></td>\n");
			writer.Write("      <td ><b>Gallery Name</b></td>\n");
			writer.Write("      <td ><b>Gallery URL</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Display Order</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Edit</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Manage Images</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete</b></td>\n");
			writer.Write("    </tr>\n");
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
				writer.Write("      <td>" + DB.RowFieldInt(row,"GalleryID").ToString() + "</td>\n");
				writer.Write("      <td >");
				String Image1URL = Common.LookupImage("Gallery",DB.RowFieldInt(row,"GalleryID"),"",_siteID);
				if(Image1URL.Length != 0)
				{
					writer.Write("<a href=\"editGallery.aspx?Galleryid=" + DB.RowFieldInt(row,"GalleryID").ToString() + "\">");
					writer.Write("<img src=\"" + Image1URL + "?" + Common.GetRandomNumber(1,1000000).ToString() + "\" height=\"35\" border=\"0\" align=\"absmiddle\">");
					writer.Write("</a>&nbsp;\n");
				}
				writer.Write("      <a href=\"editGallery.aspx?Galleryid=" + DB.RowFieldInt(row,"GalleryID").ToString() + "\">");
				writer.Write(DB.RowField(row,"Name"));
				writer.Write("</a>");
				writer.Write("</td>\n");
				writer.Write("      <td><a target=\"_blank\" href=\"../showgallery.aspx?galleryid=" + DB.RowFieldInt(row,"GalleryID").ToString() + "\">showgallery.aspx?galleryid=" + DB.RowFieldInt(row,"GalleryID").ToString() + "</a></td>\n");
				writer.Write("      <td align=\"center\"><input size=2 type=\"text\" name=\"DisplayOrder_" + DB.RowFieldInt(row,"GalleryID").ToString() + "\" value=\"" + DB.RowFieldInt(row,"DisplayOrder").ToString() + "\"></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Edit\" name=\"Edit_" + DB.RowFieldInt(row,"GalleryID").ToString() + "\" onClick=\"self.location='editgallery.aspx?galleryid=" + DB.RowFieldInt(row,"GalleryID").ToString() + "'\"></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Add/Delete Gallery Images\" name=\"ManageImages_" + DB.RowFieldInt(row,"GalleryID").ToString() + "\" onClick=\"self.location='galleryimages.aspx?galleryid=" + DB.RowFieldInt(row,"GalleryID").ToString() + "'\"></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Delete\" name=\"Delete_" + DB.RowFieldInt(row,"GalleryID").ToString() + "\" onClick=\"DeleteGallery(" + DB.RowFieldInt(row,"GalleryID").ToString() + ")\"></td>\n");
				writer.Write("    </tr>\n");
			}
			ds.Dispose();
			writer.Write("    <tr>\n");
			writer.Write("      <td colspan=\"4\" align=\"left\"></td>\n");
			writer.Write("      <td align=\"center\" bgcolor=\"" + Common.AppConfig("LightCellColor") + "\"><input type=\"submit\" value=\"Update\" name=\"Submit\"></td>\n");
			writer.Write("      <td colspan=\"2\"></td>\n");
			writer.Write("    </tr>\n");
			writer.Write("    <tr>\n");
			writer.Write("      <td colspan=\"7\" height=5></td>\n");
			writer.Write("    </tr>\n");
			writer.Write("  </table>\n");
			writer.Write("<p align=\"left\"><input type=\"button\" value=\"Add New Gallery\" name=\"AddNew\" onClick=\"self.location='editgallery.aspx';\"><p/>");
			writer.Write("</form>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteGallery(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete gallery: ' + id))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'galleries.aspx?deleteid=' + id;\n");
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
