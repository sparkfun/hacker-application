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
	/// Summary description for recentadditions.
	/// </summary>
	public class recentadditions : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = Common.AppConfig("RecentAdditionsTitle");
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			String Intro = Common.AppConfig("RecentAdditionsIntro");
			if(Intro.Length != 0)
			{
				writer.Write("<p align=\"left\">" + Intro + "</p>");
			}

			bool anyFound = false;
			int ShowN = Common.AppConfigUSInt("RecentAdditionsN");
			if(ShowN == 0)
			{
				ShowN = 10;
			}
			int NumDays = Common.AppConfigUSInt("RecentAdditionsNumDays");
			if(NumDays == 0)
			{
				NumDays = 30;
			}
			String sql = "select top " + ShowN.ToString() + " Product.ProductID, Product.SKU, ProductVariant.SKUSuffix, Product.Name, Product.SEName, ProductVariant.Name as VariantName from Product " + DB.GetNoLock() + " , ProductVariant  " + DB.GetNoLock() + " where Product.ProductID=ProductVariant.ProductID and Product.Deleted=0 and Product.Published<>0 and ProductVariant.Published<>0 and ProductVariant.Deleted=0  and Product.CreatedOn>=" + DB.DateQuote(Localization.ToNativeShortDateString(System.DateTime.Now.AddDays(-NumDays))) + " order by Product.CreatedOn desc, Product.Name";
			DataSet ds = DB.GetDS(sql,false);
			if(ds.Tables[0].Rows.Count > 0)
			{
				writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
				writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
				writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/recent.gif\" border=\"0\"><br>");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
				writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

				bool ShowSales = false; 
				bool ShowPics = Common.AppConfigBool("RecentAdditionsShowPics");
				int ShowPicsHeight = Common.AppConfigUSInt("RecentAdditionsShowPicsHeight");
				writer.Write("  <table border=\"0\" cellpadding=\"0\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
				writer.Write("    <tr>\n");
				//writer.Write("      <td align=\"left\"><b>Rank</b></td>\n");
				writer.Write("      <td align=\"left\"><b>Name</b></td>\n");
				if(ShowSales)
				{
					writer.Write("      <td align=\"center\"><b>Sales</b></td>\n");
				}
				writer.Write("      <td align=\"center\"><b>SKU</b></td>\n");
				writer.Write("      <td align=\"center\"><b>"  + Common.AppConfig("CategoryPromptSingular") + "</b></td>\n");
				writer.Write("    </tr>\n");
				writer.Write("<tr><td colspan=\"" + Common.IIF(ShowSales , "4" , "3") + "\" height=\"4\" width=\"100%\"><img src=\"images/spacer.gif\" height=\"1\" width=\"1\"></td></tr>");
				//int i = 1;
				foreach(DataRow row in ds.Tables[0].Rows)
				{
					String url = SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName"));
					writer.Write("<tr>");
					//writer.Write("<td valign=\"middle\" align=\"left\" >" + i++ + ".</td>");
					writer.Write("<td valign=\"middle\" align=\"left\" >");
					if(ShowPics)
					{
						String ImgUrl = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",_siteID);
						if(ImgUrl.Length == 0)
						{
							ImgUrl = Common.AppConfig("NoPicture");
						}
						if(ImgUrl.Length != 0)
						{
							writer.Write("<br><img " + Common.IIF(ShowPicsHeight != 0 , "height=\"" + ShowPicsHeight.ToString() + "\"" , "") + " align=\"absmiddle\" onClick=\"self.location='" + url + "';\" style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + ImgUrl + "?" + Common.GetRandomNumber(1,100000).ToString() + "\">\n");
						}
					}
					writer.Write("<a href=\"" + url + "\">" + DB.RowField(row,"Name") + Common.IIF(DB.RowField(row,"VariantName").Length != 0 , " - " + DB.RowField(row,"VariantName") , "") + "</a>");
					writer.Write("</td>");
					if(ShowSales)
					{
						writer.Write("<td valign=\"middle\" align=\"center\" >" + DB.RowFieldInt(row,"NumSales").ToString() + "</td>");
					}
					writer.Write("<td valign=\"middle\" align=\"center\">" + Common.MakeProperProductSKU(DB.RowField(row,"SKU"), DB.RowField(row,"SkuSuffix"),"","") + "</td>");
					String Cats = Common.GetProductCategories(DB.RowFieldInt(row,"ProductID"),false);
					if(Cats.Length != 0)
					{
						String[] CatIDs = Cats.Split(',');
						writer.Write("<td align=\"center\">");
						bool firstCat = true;
						foreach(String s in CatIDs)
						{
							if(!firstCat)
							{
								writer.Write(", ");
							}
							writer.Write("<a href=\"showcategory.aspx?categoryid=" + s + "&resetfilter=true\">" + Common.GetCategoryName(Localization.ParseUSInt(s)).Trim() + "</a>");
							firstCat = false;
						}
						writer.Write("</td>\n");
					}
					else
					{
						writer.Write("<td align=\"center\">");
						writer.Write("&nbsp;");
						writer.Write("</td>\n");
					}
					writer.Write("</tr>\n");
					writer.Write("<tr><td colspan=\"" + Common.IIF(ShowSales , "4" , "3") + "\" height=\"1\" width=\"100%\" class=\"LightCell\"><img src=\"images/spacer.gif\" height=\"1\" width=\"1\"></td></tr>");
					anyFound = true;
				}
				writer.Write("<tr><td colspan=\"" + Common.IIF(ShowSales , "4" , "3") + "\">&nbsp;</td></tr>\n");
				writer.Write("</table>\n");

				writer.Write("</td></tr>\n");
				writer.Write("</table>\n");
				writer.Write("</td></tr>\n");
				writer.Write("</table>\n");
			}
			ds.Dispose();

			if(!anyFound)
			{
				writer.Write("<p align=\"left\"><b>There are no new products in the last " + NumDays.ToString() + " days...</b></p>\n");
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
