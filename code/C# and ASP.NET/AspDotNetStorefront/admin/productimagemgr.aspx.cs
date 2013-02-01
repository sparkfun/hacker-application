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
	/// Summary description for productimagemgr
	/// </summary>
	public class productimagemgr : SkinBase
	{
		
		int ProductID;
		int VariantID;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)

			ProductID = Common.QueryStringUSInt("ProductID");
			VariantID = Common.QueryStringUSInt("VariantID");
			if(VariantID == 0)
			{
				VariantID = Common.GetFirstProductVariant(ProductID);
			}
			
			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
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
				try
				{
					for(int i = 0; i<=Request.Form.Count-1; i++)
					{
						String FieldName = Request.Form.Keys[i];
						if(FieldName.IndexOf("Key_") != -1)
						{
							String KeyVal = Common.Form(FieldName);
							// this field should be processed
							String[] KeyValSplit = KeyVal.Split('|');
							int TheFieldID = Localization.ParseUSInt(KeyValSplit[0]);
							int TheProductID = Localization.ParseUSInt(KeyValSplit[1]);
							int TheVariantID = Localization.ParseUSInt(KeyValSplit[2]);
							String ImageNumber = Common.CleanSizeColorOption(KeyValSplit[3]);
							String Color = Common.CleanSizeColorOption(KeyValSplit[4]);
							String SafeColor = Common.MakeSafeFilesystemName(Color);
							bool DeleteIt = (Common.Form("Delete_" + TheFieldID.ToString()).Length != 0);
							if(DeleteIt)
							{
								System.IO.File.Delete(Common.GetImagePath("Product","medium",true) + FN + "_" + ImageNumber.ToLower() + "_" + SafeColor + ".jpg");
								System.IO.File.Delete(Common.GetImagePath("Product","medium",true) + FN + "_" + ImageNumber.ToLower() + "_" + SafeColor + ".gif");
								System.IO.File.Delete(Common.GetImagePath("Product","medium",true) + FN + "_" + ImageNumber.ToLower() + "_" + SafeColor + ".png");
							}

							String Image2 = String.Empty;
							HttpPostedFile Image2File = Request.Files["Image" + TheFieldID.ToString()];
							if(Image2File.ContentLength != 0)
							{
								// delete any current image file first
								try
								{
									System.IO.File.Delete(Common.GetImagePath("Product","medium",true) + FN + "_" + ImageNumber.ToLower() + "_" + SafeColor + ".jpg");
									System.IO.File.Delete(Common.GetImagePath("Product","medium",true) + FN + "_" + ImageNumber.ToLower() + "_" + SafeColor + ".gif");
									System.IO.File.Delete(Common.GetImagePath("Product","medium",true) + FN + "_" + ImageNumber.ToLower() + "_" + SafeColor + ".png");
								}
								catch
								{}

								String s = Image2File.ContentType;
								switch(Image2File.ContentType)
								{
									case "image/gif":
										Image2 = Common.GetImagePath("Product","medium",true) + FN + "_" + ImageNumber.ToLower() + "_" + SafeColor + ".gif";
										Image2File.SaveAs(Image2);
										break;
									case "image/x-png":
										Image2 = Common.GetImagePath("Product","medium",true) + FN + "_" + ImageNumber.ToLower() + "_" + SafeColor +  ".png";
										Image2File.SaveAs(Image2);
										break;
									case "image/jpeg":
									case "image/pjpeg":
										Image2 = Common.GetImagePath("Product","medium",true) + FN + "_" + ImageNumber.ToLower() + "_" + SafeColor +  ".jpg";
										Image2File.SaveAs(Image2);
										break;
								}
							}
						}
					}
				}
				catch(Exception ex)
				{
					ErrorMsg += Common.GetExceptionDetail(ex,"<br>");
				}
			}
			SectionTitle = "<a href=\"editproduct.aspx?productid=" + ProductID.ToString() + "\">Products</a> - Multiple Image Manager";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
		
			if(ErrorMsg.Length != 0)
			{
				writer.Write("<p align=\"left\"><b><font color=red>" + ErrorMsg + "</font></b></p>\n");
			}
			if(DataUpdated)
			{
				writer.Write("<p align=\"left\"><b><font color=blue>(UPDATED)</font></b></p>\n");
			}

			IDataReader rs = DB.GetRS("select * from productvariant  " + DB.GetNoLock() + " where VariantID=" + VariantID.ToString());
			if(!rs.Read())
			{
				rs.Close();
				Response.Redirect("splash.aspx"); // should not happen, but...
			}

			String ProductName = Common.GetProductName(ProductID);
			String ProductSKU = Common.GetProductSKU(ProductID);
			String VariantName = Common.GetVariantName(VariantID);
			String VariantSKU = Common.GetVariantSKUSuffix(VariantID);
			
			String ImageNumbers = "1,2,3,4,5,6,7,8,9,10";
			String Colors = "," +  DB.RSField(rs,"Colors"); // add an "empty" color to the first entry, to allow an image to be specified for "no color selected"
			String[] ColorsSplit = Colors.Split(',');
			String[] ImageNumbersSplit = ImageNumbers.Split(',');

			writer.Write("<p align=\"left\"><b>PRODUCT: <a href=\"editproduct.aspx?productid=" + ProductID.ToString() + "\">" + ProductName + " (ProductID=" + ProductID.ToString() + ")</a></b></p>");
			writer.Write("<p align=\"left\">Manage (medium) images for this product by image # and color. You can have up to 10 images for a product, and an image for each color, so this forms a 2 dimensional grid if images: image number x color. Each slot can have a separate picture. You should also load the medium image pic on the editproduct page...that image is used by default for most page displays. These images are only used on the product page, when the user actively selects a different image number icon and/or color selection.</p>\n");

			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function MultiImageForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");

			writer.Write("<div align=\"left\">");
			writer.Write("<form enctype=\"multipart/form-data\" action=\"productimagemgr.aspx?productid=" + ProductID.ToString() + "&VariantID=" + VariantID.ToString() + "\" method=\"post\" id=\"MultiImageForm\" name=\"MultiImageForm\" onsubmit=\"return (validateForm(this) && MultiImageForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");

			writer.Write("<table border=\"0\" cellspacing=\"4\" cellpadding=\"4\" border=\"1\">\n");
			writer.Write("<tr>\n");
			writer.Write("<td valign=\"middle\" align=\"right\"><b>Color\\Image#</b></td>\n");
			for(int i = ImageNumbersSplit.GetLowerBound(0); i <= ImageNumbersSplit.GetUpperBound(0); i++)
			{
				writer.Write("<td valign=\"middle\" align=\"center\"><b>" + Common.CleanSizeColorOption(ImageNumbersSplit[i]) + "</b></td>\n");
			}
			writer.Write("</tr>\n");
			int FormFieldID = 1000; // arbitrary number
			for(int i = ColorsSplit.GetLowerBound(0); i <= ColorsSplit.GetUpperBound(0); i++)
			{
				writer.Write("<tr>\n");
				writer.Write("<td valign=\"middle\" align=\"right\"><b>" + Common.IIF(ColorsSplit[i].Length == 0, "(No Color Selected)",Common.CleanSizeColorOption(ColorsSplit[i])) + "</b></td>\n");
				for(int j = ImageNumbersSplit.GetLowerBound(0); j <= ImageNumbersSplit.GetUpperBound(0); j++)
				{
					writer.Write("<td valign=\"bottom\" align=\"center\" bgcolor=\"#EEEEEE\">");
					int ImgWidth = Common.AppConfigNativeInt("Admin.MultiGalleryImageWidth");
					writer.Write("<img " + Common.IIF(ImgWidth != 0, "width=\"" + ImgWidth.ToString() + "\"","") + " src=\"" + Common.LookupProductImageByNumberAndColor(ProductID,_siteID,thisCustomer._localeSetting,Localization.ParseUSInt(ImageNumbersSplit[j]),ColorsSplit[i]) + "\"><br>");
					writer.Write("<input style=\"font-size: 9px;\" type=\"file\" name=\"Image" + FormFieldID.ToString() + "\" size=\"24\" value=\"\"><br>\n");
					writer.Write("<input type=\"checkbox\" name=\"Delete_" + FormFieldID.ToString() + "\"> delete");
					writer.Write("<input type=\"hidden\" name=\"Key_" + FormFieldID.ToString() + "\" value=\"" + FormFieldID.ToString() + "|" + ProductID.ToString() + "|" + VariantID.ToString() + "|" + Common.CleanSizeColorOption(ImageNumbersSplit[j]) + "|" + Common.CleanSizeColorOption(ColorsSplit[i]) + "\">");
					FormFieldID++;
					writer.Write("</td>\n");
				}
				writer.Write("</tr>\n");
			}

			writer.Write("</table>\n");
			writer.Write("<p align=\"left\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"submit\" value=\"Update\" name=\"submit\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"reset\" value=\"Reset\" name=\"reset\"></p>\n");
			writer.Write("</form>\n");
			writer.Write("</div>");
			rs.Close();
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
