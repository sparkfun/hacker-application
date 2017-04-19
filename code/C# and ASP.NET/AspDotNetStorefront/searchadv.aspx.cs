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
	/// Summary description for searchadv.
	/// </summary>
	public class searchadv : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Advanced Search";
			if(DB.GetDBProvider() == "MSACCESS") // adv search not supported for access db
			{
				Response.Redirect("search.aspx" + "?" + Common.ServerVariables("QUERY_STRING"));
			}
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			Topic t = new Topic("SearchPageHeader",thisCustomer._localeSetting,_siteID);
			writer.Write(t._contents);


			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function SearchForm2_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("  return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");

			String SearchTerm = Common.QueryString("SearchTerm").Trim();
			String SKU = Common.QueryString("SKU");
			String ManufacturerPartNumber = Common.QueryString("ManufacturerPartNumber");
			int ManufacturerID = Common.QueryStringUSInt("ManufacturerID");
			int CategoryID = Common.QueryStringUSInt("CategoryID");
			int SectionID = Common.QueryStringUSInt("SectionID");
			int ProductTypeID = Common.QueryStringUSInt("ProductTypeID");
			int ShowPics = Common.QueryStringUSInt("ShowPics");
			int SearchDescriptions = Common.QueryStringUSInt("SearchDescriptions");
			String MinPrice = Common.QueryString("MinPrice");
			String MaxPrice = Common.QueryString("MaxPrice");
			int NumPT = DB.GetSqlN("select count(*) as N from producttype  " + DB.GetNoLock() + " where deleted=0");
			int NumCats = DB.GetSqlN("select count(*) as N from category  " + DB.GetNoLock() + " where published<>0 and deleted=0");
			int NumSecs = DB.GetSqlN("select count(*) as N from [section]  " + DB.GetNoLock() + " where published<>0 and deleted=0");
			int NumMfgs = DB.GetSqlN("select count(*) as N from manufacturer  " + DB.GetNoLock() + " where deleted=0");

			if(SearchTerm.Length != 0)
			{
				DB.ExecuteSQL("insert into SearchLog(SearchTerm,CustomerID) values(" + DB.SQuote(Common.Ellipses(SearchTerm,97,true)) + "," + thisCustomer._customerID.ToString() + ")");
			}


			writer.Write("<form method=\"GET\" action=\"searchadv.aspx\" onsubmit=\"return validateForm(this)\" name=\"AdvSearchForm\">\n");
			writer.Write("<p>Use the settings below to indicate what you are searching for:</p>\n");
			writer.Write("<table width=\"100%\" cellpadding=\"1\" cellspacing=\"0\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("              <tr valign=\"middle\">\n");
			writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Search For:&nbsp;&nbsp;</td>\n");
			writer.Write("                <td align=\"left\">\n");
			writer.Write("                	<input maxLength=\"100\" size=\"50\" name=\"SearchTerm\" value=\"" + SearchTerm + "\">");
			writer.Write("                	</td>\n");
			writer.Write("              </tr>\n");
			writer.Write("              <tr valign=\"middle\">\n");
			writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Search Descriptions:&nbsp;&nbsp;</td>\n");
			writer.Write("                <td align=\"left\">\n");
			writer.Write("					Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"SearchDescriptions\" value=\"1\" " + Common.IIF(SearchDescriptions == 1 , " checked " , "") + ">\n");
			writer.Write("					No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"SearchDescriptions\" value=\"0\" " + Common.IIF(SearchDescriptions == 0 , " checked " , "") + ">\n");
			writer.Write("                	</td>\n");
			writer.Write("              </tr>\n");

			if(Common.AppConfigBool("SearchAdv_ShowProductType") && NumPT > 2)
			{
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">Product Type:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("<select size=\"1\" name=\"ProductTypeID\">\n");
				writer.Write(" <OPTION VALUE=\"0\">--ANY--</option>\n");
				IDataReader rsstx = DB.GetRS("select * from ProductType  " + DB.GetNoLock() + " where deleted=0 order by name");
				while(rsstx.Read())
				{
					writer.Write("<option value=\"" + DB.RSFieldInt(rsstx,"ProductTypeID").ToString() + "\"");
					if(ProductTypeID == DB.RSFieldInt(rsstx,"ProductTypeID"))
					{
						writer.Write(" selected");
					}
					writer.Write(">" + DB.RSField(rsstx,"Name") + "</option>");
				}
				rsstx.Close();
				writer.Write("</select>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
			}

			if(Common.AppConfigBool("SearchAdv_ShowCategory") && NumCats > 2)
			{
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">" + Common.AppConfig("CategoryPromptSingular") + ":&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("<select onChange=\"document.FilterForm.submit()\" style=\"font-size: 9px;\" size=\"1\" name=\"CategoryID\">\n");
				writer.Write("<OPTION VALUE=\"0\" " + Common.IIF(CategoryID == 0 , " selected " , "") + ">All " + Common.AppConfig("CategoryPromptPlural") + "</option>\n");
				String CatSel = Common.GetCategorySelectList(0,String.Empty,0);
				// mark current Category:
				CatSel = CatSel.Replace("<option value=\"" + CategoryID.ToString() + "\">","<option value=\"" + CategoryID.ToString() + "\" selected>");
				writer.Write(CatSel);
				writer.Write("</select>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
			}

			if(Common.AppConfigBool("SearchAdv_ShowSection") && NumSecs > 2)
			{
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">" + Common.AppConfig("SectionPromptSingular") + ":&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("<select onChange=\"document.FilterForm.submit()\" style=\"font-size: 9px;\" size=\"1\" name=\"SectionID\">\n");
				writer.Write("<OPTION VALUE=\"0\" " + Common.IIF(SectionID == 0 , " selected " , "") + ">All " + Common.AppConfig("SectionPromptPlural") + "</option>\n");
				String SecSel = Common.GetSectionSelectList(0,String.Empty,0);
				// mark current Section:
				SecSel = SecSel.Replace("<option value=\"" + SectionID.ToString() + "\">","<option value=\"" + SectionID.ToString() + "\" selected>");
				writer.Write(SecSel);
				writer.Write("</select>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
			}

			if(Common.AppConfigBool("SearchAdv_ShowManufacturer") && NumMfgs > 2)
			{
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">Manufacturer:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("<select size=\"1\" name=\"ManufacturerID\">\n");
				writer.Write(" <OPTION VALUE=\"0\">--ANY--</option>\n");
				IDataReader rsst2 = DB.GetRS("select * from Manufacturer  " + DB.GetNoLock() + " where deleted=0 order by name");
				while(rsst2.Read())
				{
					writer.Write("<option value=\"" + DB.RSFieldInt(rsst2,"ManufacturerID").ToString() + "\"");
					if(ManufacturerID == DB.RSFieldInt(rsst2,"ManufacturerID"))
					{
						writer.Write(" selected");
					}
					writer.Write(">" + DB.RSField(rsst2,"Name") + "</option>");
				}
				rsst2.Close();
				writer.Write("</select>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
			}

			if(Common.AppConfigBool("SearchAdv_ShowSKU"))
			{
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">SKU:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"50\" size=\"50\" name=\"SKU\" value=\"" + SKU + "\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");
			}

			//			if(Common.AppConfigBool("SearchAdv_ShowManufacturerPartNumber"))
			//			{
			//				writer.Write("              <tr valign=\"middle\">\n");
			//				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Manufacturer Part #:&nbsp;&nbsp;</td>\n");
			//				writer.Write("                <td align=\"left\">\n");
			//				writer.Write("                	<input maxLength=\"50\" size=\"50\" name=\"ManufacturerPartNumber\" value=\"" + ManufacturerPartNumber + "\">\n");
			//				writer.Write("                	</td>\n");
			//				writer.Write("              </tr>\n");
			//			}

			writer.Write("              <tr valign=\"middle\">\n");
			writer.Write("                <td align=\"right\" valign=\"middle\">Show Pics:&nbsp;&nbsp;</td>\n");
			writer.Write("                <td align=\"left\">\n");
			writer.Write("					Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"ShowPics\" value=\"1\" " + Common.IIF(ShowPics == 1 , " checked " , "") + ">\n");
			writer.Write("					No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"ShowPics\" value=\"0\" " + Common.IIF(ShowPics == 0 , " checked " , "") + ">\n");
			writer.Write("                </td>\n");
			writer.Write("              </tr>\n");

			if(Common.AppConfigBool("SearchAdv_ShowPriceRange"))
			{
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Min Price:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"10\" size=\"10\" name=\"MinPrice\" value=\"" + MinPrice + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"MinPrice_vldt\" value=\"[number][invalidalert=Please enter a valid currency amount, e.g. 10.00 without the leading $ sign!]\">\n");
				writer.Write("                &nbsp;&nbsp;&nbsp;Max Price:&nbsp;&nbsp;\n");
				writer.Write("                	<input maxLength=\"10\" size=\"10\" name=\"MaxPrice\" value=\"" + MaxPrice + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"MaxPrice_vldt\" value=\"[number][invalidalert=Please enter a valid currency amount, e.g. 10.00 without the leading $ sign!]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");
			}

			writer.Write("<tr>\n");
			writer.Write("<td></td><td align=\"left\"><br>\n");
			writer.Write("<input type=\"submit\" value=\"Search\" name=\"submit\">\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("</form>\n");

			String st = Common.QueryString("SearchTerm").Trim();
			bool IsSubmit = (Common.QueryString("IsSubmit").ToLower() == "true");
			if(IsSubmit)
			{
				String stlike = "%" + st + "%";
				String stquoted = DB.SQuote(stlike);

				bool anyFound = false;

				// MATCHING CATEGORIES:
				if(Common.AppConfigBool("Search_ShowCategoriesInResults") && NumCats > 0 && SearchTerm.Length != 0)
				{
					String sql = "select * from category  " + DB.GetNoLock() + " where deleted=0 and published<>0 and category.name like " + stquoted + " and published<>0 and deleted=0 ";
					if(CategoryID != 0)
					{
						sql += " and CategoryID=" + CategoryID.ToString();
					}
					sql += " order by displayorder,name";
					DataSet ds = DB.GetDS(sql,false);
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

				// MATCHING SECTIONS:
				if(Common.AppConfigBool("Search_ShowSectionsInResults") && NumSecs > 0 && SearchTerm.Length != 0)
				{
					String sql = "select * from [section]  " + DB.GetNoLock() + " where deleted=0 and published<>0 and name like " + stquoted;
					if(SectionID != 0)
					{
						sql += " and sectionID=" + SectionID.ToString();
					}
					sql += " order by displayorder,name";
					DataSet ds = DB.GetDS(sql,false);
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

				// MATCHING MANUFACTURERS:
				if(Common.AppConfigBool("Search_ShowManufacturersInResults") && NumMfgs > 0 && SearchTerm.Length != 0)
				{
					String sql = "select * from manufacturer  " + DB.GetNoLock() + " where deleted=0 and name like " + stquoted;
					if(ManufacturerID != 0)
					{
						sql += " and ManufacturerID=" + ManufacturerID.ToString();
					}
					DataSet ds = DB.GetDS(sql,false);
					if(ds.Tables[0].Rows.Count > 0)
					{

						writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
						writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
						writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/matchingmanufacturers.gif\" border=\"0\"><br>");
						writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
						writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
						foreach(DataRow row in ds.Tables[0].Rows)
						{
							writer.Write("<a href=\"" + SE.MakeManufacturerLink(DB.RowFieldInt(row,"ManufacturerID"),String.Empty) + "\">" + Common.HighlightTerm(DB.RowField(row,"Name"),st) + "</a><br>\n");
							anyFound = true;
						}
						writer.Write("</td></tr>\n");
						writer.Write("</table>\n");
						writer.Write("</td></tr>\n");
						writer.Write("</table>\n");
					}
					ds.Dispose();
				}

				if(Common.AppConfigBool("Search_ShowProductsInResults") && (SearchTerm.Length != 0 || CategoryID != 0 || SectionID != 0 || ManufacturerID != 0 || ProductTypeID != 0))
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
						sql += " Product.Published=1 AND Product.Deleted=0 AND ProductVariant.Published=1 AND ProductVariant.Deleted=0 and product.manufacturerid in (select manufacturerid from manufacturer  " + DB.GetNoLock() + " where deleted=0)  ";
						if(SearchTerm.Length != 0)
						{
							sql += " and (";
							sql += " product.name like " + thisSQuoted;
							sql += " or product.sku like " + thisSQuoted;
							sql += " or product.manufacturerpartnumber like " + thisSQuoted;
							sql += " or productvariant.name like " + thisSQuoted;
							sql += " or productvariant.manufacturerpartnumber like " + thisSQuoted;
							sql += " or product.sku+productvariant.skusuffix like " + thisSQuoted;
							if(SearchDescriptions == 1)
							{
								sql += " or product.description like " + thisSQuoted;
								sql += " or product.summary like " + thisSQuoted;
							}
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
						if(MinPrice.Length != 0)
						{
							sql += " and productvariant.price >= " + MinPrice;
						}
						if(MaxPrice.Length != 0)
						{
							sql += " and productvariant.price <= " + MaxPrice;
						}
						if(CategoryID != 0)
						{
							sql += " and product.productid in (select productid from productcategory  " + DB.GetNoLock() + " where categoryid=" + CategoryID.ToString() + ")";

						}
						if(SectionID != 0)
						{
							sql += " and product.productid in (select productid from productsection  " + DB.GetNoLock() + " where sectionid=" + SectionID.ToString() + ")";

						}
						if(ProductTypeID != 0)
						{
							sql += " and product.producttypeid=" + ProductTypeID.ToString();
						}
						if(ManufacturerID != 0)
						{
							sql += " and product.ManufacturerID=" + ManufacturerID.ToString();
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
								if(ShowPics == 1)
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
									writer.Write("<td align=\"center\"><a href=\"" + SE.MakeManufacturerLink(DB.RowFieldInt(row,"ManufacturerID"),String.Empty) + "\">" + Common.HighlightTerm(Common.GetManufacturerName(DB.RowFieldInt(row,"ManufacturerID")),st) + "</a></td>");
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
			writer.Write("document.AdvSearchForm.SearchTerm.focus();\n");
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
