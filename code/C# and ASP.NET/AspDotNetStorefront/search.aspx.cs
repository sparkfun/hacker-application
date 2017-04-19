// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.AspDotNetStorefront.com
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
			Topic t = new Topic("SearchPageHeader",thisCustomer._localeSetting,_siteID);
			writer.Write(t._contents);
			String SearchTerm = Common.QueryString("SearchTerm").Trim();

			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function SearchForm2_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("	submitonce(theForm);\n");
			writer.Write("  if (theForm.SearchTerm.value.length < " + Common.AppConfig("MinSearchStringLength") + ")\n");
			writer.Write("  {\n");
			writer.Write("    alert('Please enter at least " + Common.AppConfig("MinSearchStringLength") + " characters in the Search For field.');\n");
			writer.Write("    theForm.SearchTerm.focus();\n");
			writer.Write("	submitenabled(theForm);\n");
			writer.Write("    return (false);\n");
			writer.Write("  }\n");
			writer.Write("  return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");

			writer.Write("<form method=\"GET\" action=\"search.aspx\" onsubmit=\"return validateForm(this)\" name=\"SearchForm2\">\n");
			writer.Write("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\n");
			writer.Write("      <tr align=\"left\">\n");
			writer.Write("        <td width=\"100%\" colspan=\"2\"><b><font color=\"red\">" + Server.HtmlEncode(Common.QueryString("ErrorMsg")) + "</font></b>\n");
			writer.Write("          <br>&nbsp;&nbsp;What are you searching for?\n");
			writer.Write("          <input type=\"text\" name=\"SearchTerm\" size=\"25\" maxlength=\"70\" value=\"" + Server.HtmlEncode(Common.QueryString("SearchTerm")) + "\">\n");
			writer.Write("          <input type=\"hidden\" name=\"SearchTerm_vldt\" value=\"[req][len=" + Common.AppConfig("MinSearchStringLength") + "][blankalert=Please enter something to search for!]\">\n");
			writer.Write("          &nbsp;<input type=\"submit\" value=\"Search\" name=\"B1\"></td>\n");
			writer.Write("      </tr>\n");
			writer.Write("    </table>\n");
			writer.Write("</form>\n");

			String st = Common.QueryString("SearchTerm").Trim();
			if(st.Length != 0)
			{
				DB.ExecuteSQL("insert into SearchLog(SearchTerm,CustomerID) values(" + DB.SQuote(Common.Ellipses(st,97,true)) + "," + thisCustomer._customerID.ToString() + ")");
				String stlike = "%" + st + "%";
				String stquoted = DB.SQuote(stlike);

				bool anyFound = false;

				if(Common.AppConfigBool("Search_ShowCategoriesInResults"))
				{
					// MATCHING CATEGORIES:
					DataSet ds = DB.GetDS("select * from category  " + DB.GetNoLock() + " where deleted=0 and published<>0 and categoryid in (select distinct categoryid from productcategory  " + DB.GetNoLock() + " where productid in (select distinct productid from product  " + DB.GetNoLock() + " where deleted=0 and published<>0)) and category.name like " + stquoted + " and published<>0 and deleted=0 order by displayorder,name",false);
					if(ds.Tables[0].Rows.Count > 0)
					{
						writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
						writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
						writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/matchingcategories.gif\" border=\"0\"><br>");
						writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
						writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
						foreach(DataRow row in ds.Tables[0].Rows)
						{
							writer.Write(Common.GetCategoryBreadcrumb(DB.RowFieldInt(row,"CategoryID")));
							anyFound = true;
						}
						writer.Write("</td></tr>\n");
						writer.Write("</table>\n");
						writer.Write("</td></tr>\n");
						writer.Write("</table>\n");
					}
					ds.Dispose();
				}

				if(Common.AppConfigBool("Search_ShowSectionsInResults"))
				{
					// MATCHING SECTIONS:
					DataSet ds = DB.GetDS("select * from [section]  " + DB.GetNoLock() + " where deleted=0 and published<>0 and sectionid in (select distinct sectionid from productsection  " + DB.GetNoLock() + " where productid in (select distinct productid from product  " + DB.GetNoLock() + " where deleted=0 and published<>0)) and [section].name like " + stquoted + " and published<>0 and deleted=0 order by displayorder,name",false);
					if(ds.Tables[0].Rows.Count > 0)
					{
						writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
						writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
						writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/matchingsections.gif\" border=\"0\"><br>");
						writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
						writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
						foreach(DataRow row in ds.Tables[0].Rows)
						{
							writer.Write(Common.GetSectionBreadcrumb(DB.RowFieldInt(row,"SectionID")));
							anyFound = true;
						}
						writer.Write("</td></tr>\n");
						writer.Write("</table>\n");
						writer.Write("</td></tr>\n");
						writer.Write("</table>\n");
					}
					ds.Dispose();
				}

				if(Common.AppConfigBool("Search_ShowManufacturersInResults"))
				{
					// MATCHING MANUFACTURERS:
					DataSet ds = DB.GetDS("select * from manufacturer  " + DB.GetNoLock() + " where deleted=0 and name like " + stquoted + " and deleted=0",false);
					if(ds.Tables[0].Rows.Count > 0)
					{

						writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
						writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
						writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/matchingmanufacturers.gif\" border=\"0\"><br>");
						writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
						writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
						foreach(DataRow row in ds.Tables[0].Rows)
						{
							writer.Write("<a href=\"" + SE.MakeManufacturerLink(DB.RowFieldInt(row,"ManufacturerID"),DB.RowField(row,"SEName")) + "\">" + Common.HighlightTerm(DB.RowField(row,"Name"),st) + "</a><br>\n");
							anyFound = true;
						}
						writer.Write("</td></tr>\n");
						writer.Write("</table>\n");
						writer.Write("</td></tr>\n");
						writer.Write("</table>\n");
					}
					ds.Dispose();
				}

				if(Common.AppConfigBool("Search_ShowProductsInResults"))
				{
					// MATCHING PRODUCTS:
					int ShowPicsHeight = Common.AppConfigUSInt("SearchAdv_ShowPicsHeight");
					String[] stArray;
					if(st.IndexOf(" ") != -1)
					{
						stArray = (st + "|" + st.Replace(" ","|")).Split('|');
					}
					else
					{
						stArray = st.Split('|');
					}
					foreach(String thisS in stArray)
					{
						String thisSQuoted = DB.SQuote("%" + thisS + "%");
						String sql = "set concat_null_yields_null off; SELECT distinct Product.ProductID, Product.ManufacturerID, Product.Name, Product.SKU, ProductVariant.VariantID, ProductVariant.Name as VariantName, ProductVariant.SKUSuffix ";
						sql += " FROM Product  " + DB.GetNoLock() + " inner join ProductVariant  " + DB.GetNoLock() + " on Product.ProductID = ProductVariant.ProductID where ";
						sql += " Product.Published=1 AND Product.Deleted=0 AND ProductVariant.Published=1 AND ProductVariant.Deleted=0 and product.manufacturerid in (select manufacturerid from manufacturer  " + DB.GetNoLock() + " where deleted=0) ";
						if(SearchTerm.Length != 0)
						{
							sql += " and (";
							sql += " product.name like " + thisSQuoted;
							sql += " or product.sku like " + thisSQuoted;
							sql += " or product.manufacturerpartnumber like " + thisSQuoted;
							sql += " or productvariant.name like " + thisSQuoted;
							sql += " or productvariant.manufacturerpartnumber like " + thisSQuoted;
							sql += " or product.sku+productvariant.skusuffix like " + thisSQuoted;
							sql += ")";
						}
						if(Common.AppConfigBool("FilterProductsByAffiliate"))
						{
							sql += " and Product.ProductID in (select distinct productid from productaffiliate  " + DB.GetNoLock() + " where affiliateid=" + thisCustomer._affiliateID.ToString() + ") and Product.Published=1 AND Product.Deleted=0 AND ProductVariant.Published=1 AND ProductVariant.Deleted=0 and product.manufacturerid in (select manufacturerid from manufacturer  " + DB.GetNoLock() + " where deleted=0) and (product.productid in (select distinct productid from productcategory  " + DB.GetNoLock() + " where categoryid in (select distinct categoryid from category  " + DB.GetNoLock() + " where published<>0 and deleted=0)) or product.productid in (select distinct productid from productsection  " + DB.GetNoLock() + " where sectionid in (select distinct sectionid from [section]  " + DB.GetNoLock() + " where published<>0 and deleted=0)))";
						}
						if(Common.AppConfigBool("FilterProductsByCustomerLevel"))
						{
							String FilterOperator = Common.IIF(Common.AppConfigBool("FilterByCustomerLevelIsAscending"),"<=","=");
							sql += " and (Product.ProductID in (select distinct productid from productCustomerLevel  " + DB.GetNoLock() + " where CustomerLevelid" + FilterOperator + thisCustomer._customerLevelID.ToString() + ") and Product.Published=1 AND Product.Deleted=0 AND ProductVariant.Published=1 AND ProductVariant.Deleted=0 and product.manufacturerid in (select manufacturerid from manufacturer  " + DB.GetNoLock() + " where deleted=0) and (product.productid in (select distinct productid from productcategory  " + DB.GetNoLock() + " where categoryid in (select distinct categoryid from category  " + DB.GetNoLock() + " where published<>0 and deleted=0)) or product.productid in (select distinct productid from productsection  " + DB.GetNoLock() + " where sectionid in (select distinct sectionid from [section]  " + DB.GetNoLock() + " where published<>0 and deleted=0))) ";
							if(Common.AppConfigBool("FilterByCustomerLevel0SeesUnmappedProducts"))
							{
								// allow customer level 0 to see any product that is not specifically mapped to any customer levels
								sql += " or Product.ProductID not in (select productid from productcustomerlevel)";
							}
							sql += ")";
						}
						DataSet ds = DB.GetDS(sql,false);
						if(ds.Tables[0].Rows.Count > 0)
						{
							writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
							writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
							writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/matchingproducts.gif\" align=\"bottom\" border=\"0\">&nbsp;&nbsp;For: " + thisS + "<br>");
							writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
							writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

							writer.Write("  <table border=\"0\" cellpadding=\"0\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
							writer.Write("    <tr>\n");
							writer.Write("      <td align=\"left\"><b>Name</b></td>\n");
							writer.Write("      <td align=\"center\"><b>SKU</b></td>\n");
							writer.Write("      <td align=\"center\"><b>"  + Common.AppConfig("CategoryPromptSingular") + "</b></td>\n");
							if(Common.AppConfigBool("Search_ShowManufacturersInResults"))
							{
								writer.Write("      <td align=\"center\"><b>Manufacturer</b></td>\n");
							}
							writer.Write("    </tr>\n");
							foreach(DataRow row in ds.Tables[0].Rows)
							{
								String url = SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),String.Empty);
								writer.Write("<tr>");
								writer.Write("<td valign=\"middle\" align=\"left\" >");
								//								if(ShowPics == 1)
								//								{
								//									String ImgUrl = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon");
								//									if(ImgUrl.Length == 0)
								//									{
								//										ImgUrl = Common.AppConfig("NoPicture");
								//									}
								//									if(ImgUrl.Length != 0)
								//									{
								//										writer.Write("<br><img " + Common.IIF(ShowPicsHeight != 0 , "height=\"" + ShowPicsHeight.ToString() + "\"" , "") + " align=\"absmiddle\" onClick=\"self.location='" + url + "';\" style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + ImgUrl + "?" + Common.GetRandomNumber(1,100000).ToString() + "\">\n");
								//									}
								//								}
								writer.Write("<a href=\"" + url + "\">" + DB.RowField(row,"Name") + Common.IIF(DB.RowField(row,"VariantName").Length != 0 , " - " + DB.RowField(row,"VariantName") , "") + "</a>");
								writer.Write("</td>");
								writer.Write("<td align=\"center\">" + Common.HighlightTerm(Common.MakeProperProductSKU(DB.RowField(row,"SKU"),DB.RowField(row,"SKUSuffix"),"",""),st) + "</td>");
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
								if(Common.AppConfigBool("Search_ShowManufacturersInResults"))
								{
									writer.Write("<td align=\"center\"><a href=\"" + SE.MakeManufacturerLink(DB.RowFieldInt(row,"ManufacturerID"),DB.RowField(row,"SEName")) + "\">" + Common.HighlightTerm(Common.GetManufacturerName(DB.RowFieldInt(row,"ManufacturerID")),st) + "</a></td>");
								}
								writer.Write("</tr>\n");
								writer.Write("<tr><td colspan=\"" + Common.IIF(Common.AppConfigBool("Search_ShowManufacturersInResults") , "5" , "4") + "\" height=\"1\" width=\"100%\" class=\"LightCell\"><img src=\"images/spacer.gif\" height=\"1\" width=\"1\"></td></tr>");
								anyFound = true;
							}
							writer.Write("<tr><td colspan=\"" + Common.IIF(Common.AppConfigBool("Search_ShowManufacturersInResults") , "5" , "4") + "\">&nbsp;</td></tr>\n");
							writer.Write("</table>\n");

							writer.Write("</td></tr>\n");
							writer.Write("</table>\n");
							writer.Write("</td></tr>\n");
							writer.Write("</table>\n");
						}
						ds.Dispose();
					}
				}

				if(!anyFound)
				{
					writer.Write("<p align=\"left\"><b>Your search did not result in any matches</b></p>\n");
				}

			}

			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("document.SearchForm2.SearchTerm.focus();\n");
			writer.Write("</script>\n");
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
