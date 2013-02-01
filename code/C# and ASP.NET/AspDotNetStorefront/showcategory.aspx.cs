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
	/// Summary description for showcategory.
	/// </summary>
	public class showcategory : SkinBase
	{
		int CategoryID;
		int SectionID;
		int ProductTypeID;
		int ManufacturerID;
		String CategoryName;
		String CategoryDescription;
		String CategoryPicture;
		bool SortByLooks;
		bool AllowSectionFiltering;
		bool AllowManufacturerFiltering;
		bool AllowProductTypeFiltering;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			CategoryID = Common.QueryStringUSInt("CategoryID");

			String SEName = Common.QueryString("SEName");
			if(CategoryID == 0 && SEName.Length != 0)
			{
				// mapping from static url, try to find category id:
				CategoryID = SE.LookupSECategory(SEName);
				if(CategoryID == 0)
				{
					// no match:
					//Response.Redirect("default.aspx");
				}
			}

			if(CategoryID == 0)
			{
				//Response.Redirect("default.aspx");
			}


			SectionID = Common.QueryStringUSInt("SectionID");
			ProductTypeID = Common.QueryStringUSInt("ProductTypeID");
			ManufacturerID = Common.QueryStringUSInt("ManufacturerID");

			if(Common.QueryString("SectionID").Length == 0)
			{
				if(Common.QueryString("ResetFilter").Length == 0 && Common.AppConfigBool("PersistFilters") && Common.Cookie("SectionID",true).Length != 0)
				{
					SectionID = Common.CookieUSInt("SectionID");
				}
			}

			if(Common.QueryString("ProductTypeID").Length == 0)
			{
				if(Common.QueryString("ResetFilter").Length == 0 && Common.AppConfigBool("PersistFilters") && Common.Cookie("ProductTypeID",true).Length != 0)
				{
					ProductTypeID = Common.CookieUSInt("ProductTypeID");
				}
				if(ProductTypeID != 0 && !Common.ProductTypeHasVisibleProducts(ProductTypeID))
				{
					ProductTypeID = 0;
				}
			}

			if(Common.QueryString("ManufacturerID").Length == 0)
			{
				if(Common.QueryString("ResetFilter").Length == 0 && Common.AppConfigBool("PersistFilters") && Common.Cookie("ManufacturerID",true).Length != 0)
				{
					ManufacturerID = Common.CookieUSInt("ManufacturerID");
				}
			}

			if(Common.QueryString("ResetFilter").Length != 0)
			{
				SectionID = 0;
				ManufacturerID = 0;
				ProductTypeID = 0;
			}

			if(Common.AppConfigBool("PersistFilters"))
			{
				Common.SetCookie("SectionID",SectionID.ToString(),new TimeSpan(365,0,0,0,0));
				Common.SetCookie("ManufacturerID",ManufacturerID.ToString(),new TimeSpan(365,0,0,0,0));
				Common.SetCookie("ProductTypeID",ProductTypeID.ToString(),new TimeSpan(365,0,0,0,0));
			}

			IDataReader rs = DB.GetRS("select * from category  " + DB.GetNoLock() + " where published=1 and deleted=0 and categoryid=" + CategoryID.ToString());
			if(!rs.Read())
			{
				rs.Close();
				Response.Redirect("default.aspx");
			}
			
			CategoryName = DB.RSField(rs,"Name");
			CategoryDescription = DB.RSField(rs,"Description");
			String FileDescription = new DescriptionFile("category",CategoryID,thisCustomer._localeSetting,_siteID)._contents;
			if (FileDescription.Length != 0)
			{
				CategoryDescription += "<div align=\"left\">" + FileDescription + "</div>";
			}
			CategoryPicture = Common.LookupImage("Category",CategoryID,"",_siteID);
			SortByLooks = DB.RSFieldBool(rs,"SortByLooks");
			AllowSectionFiltering = DB.RSFieldBool(rs,"AllowSectionFiltering");
			AllowManufacturerFiltering = DB.RSFieldBool(rs,"AllowManufacturerFiltering");
			AllowProductTypeFiltering = DB.RSFieldBool(rs,"AllowProductTypeFiltering");

			if(DB.RSField(rs,"SETitle").Length == 0)
			{
				base._SETitle = Common.AppConfig("StoreName") + " - " + CategoryName;
			}
			else
			{
				base._SETitle = DB.RSField(rs,"SETitle");
			}
			if(DB.RSField(rs,"SEDescription").Length == 0)
			{
				base._SEDescription = CategoryName;
			}
			else
			{
				base._SEDescription = DB.RSField(rs,"SEDescription");
			}
			if(DB.RSField(rs,"SEKeywords").Length == 0)
			{
				base._SEKeywords = CategoryName;
			}
			else
			{
				base._SEKeywords = DB.RSField(rs,"SEKeywords");
			}
			base._SENoScript = DB.RSField(rs,"SENoScript");

			rs.Close();

			if(!AllowSectionFiltering)
			{
				SectionID = 0; // override, category setting has highest input
			}
			
			if(!AllowManufacturerFiltering)
			{
				ManufacturerID = 0; // override, category setting has highest input
			}
			
			if(!AllowProductTypeFiltering)
			{
				ProductTypeID = 0; // override, category setting has highest input
			}
			
			SectionTitle = "<span class=\"SectionTitleText\">";
			int pid = Common.GetParentCategory(CategoryID);
			while(pid != 0)
			{
				SectionTitle = "<a class=\"SectionTitleText\" href=\"" + SE.MakeCategoryLink(pid,"") + "\">" + Common.GetCategoryName(pid) + "</a> - " + SectionTitle;
				pid = Common.GetParentCategory(pid);
			}
			SectionTitle += CategoryName;

			if(SectionID != 0)
			{
				SectionTitle += "&nbsp;-&nbsp;" + Common.GetSectionBreadcrumb(SectionID);
			}

			SectionTitle += "</span>";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			Common.LogEvent(thisCustomer._customerID,9,CategoryID.ToString());
			if(CategoryID == Common.AppConfigUSInt("IsFeaturedCategoryID"))
			{
				Response.Redirect("showisfeaturedcategory.aspx");
			}

			int DisplayFormatID = 1;
			IDataReader rsu = DB.GetRS("select CategoryDisplayFormatID from Category where CategoryID=" + CategoryID.ToString());
			if(rsu.Read())
			{
				DisplayFormatID = DB.RSFieldInt(rsu,"CategoryDisplayFormatID");
			}
			rsu.Close();
			
			bool empty = true;
			String sql = String.Empty;
			if(DisplayFormatID == 1 || DisplayFormatID == 2 || DisplayFormatID == 3 || DisplayFormatID == 4 || DisplayFormatID == 6)
			{
				sql = "SELECT C.CategoryID, C.Name AS CategoryName, C.ColWidth, C.DisplayPrefix, C.SortByLooks, C.CategoryDisplayFormatID, C.SEName as CSEName, C.SENoScript as CSENoScript, C.SEAltText as CSEAltText, P.SENoScript as PSENoScript, P.SEAltText as PSEAltText, P.ProductID, P.Name, P.SEName as PSEName, P.ProductTypeID, P.Summary, P.Description, P.SEKeywords, P.SEDescription, P.SKU, P.ManufacturerID, M.Name AS ManufacturerName, P.ManufacturerPartNumber, P.SalesPromptID, P.SpecTitle, P.SpecCall, P.IsFeatured, P.VariantShown, P.Published, P.MiscText, P.Looks, P.Notes, P.HidePriceUntilCart, SalesPrompt.Name as sDescription ";
				sql += " FROM (((((Category C  " + DB.GetNoLock() + " INNER JOIN ProductCategory PC  " + DB.GetNoLock() + " ON C.CategoryID = PC.CategoryID) INNER JOIN Product P  " + DB.GetNoLock() + " ON PC.ProductID = P.ProductID) INNER JOIN Manufacturer M  " + DB.GetNoLock() + " ON P.ManufacturerID = M.ManufacturerID) left outer join CategoryDisplayOrder DO  " + DB.GetNoLock() + " on p.productid=DO.productid) left outer join SalesPrompt " + DB.GetNoLock() + " on P.SalesPromptID=SalesPrompt.SalesPromptID)";
				sql += " WHERE DO.categoryid=" + CategoryID.ToString() + " and (P.Published = 1) AND (P.Deleted = 0) and PC.categoryid=" + CategoryID.ToString();
			}
			else
			{
				// need price and all variants
				sql = "SELECT C.CategoryID, C.Name AS CategoryName, C.ColWidth, C.DisplayPrefix, C.SortByLooks, C.CategoryDisplayFormatID, C.SEName as CSEName, C.SENoScript as CSENoScript, C.SEAltText as CSEAltText, P.SENoScript as PSENoScript, P.SEAltText as PSEAltText, P.ProductID, P.Name, P.SEName as PSEName, P.ProductTypeID, P.Summary, P.Description, P.SEKeywords, P.SEDescription, P.SKU, P.ManufacturerID, M.Name AS ManufacturerName, P.ManufacturerPartNumber, P.SalesPromptID, P.SpecTitle, P.SpecCall, P.IsFeatured, P.VariantShown, P.Published, P.MiscText, P.Looks, P.Notes, P.HidePriceUntilCart, P.Deleted, PV.VariantID, PV.Price, PV.SalePrice, PV.Name as VariantName, SalesPrompt.Name as sDescription ";
				sql += " FROM ((((((Category C  " + DB.GetNoLock() + " INNER JOIN ProductCategory PC  " + DB.GetNoLock() + " ON C.CategoryID = PC.CategoryID) INNER JOIN Product P  " + DB.GetNoLock() + " ON PC.ProductID = P.ProductID) INNER JOIN Manufacturer M  " + DB.GetNoLock() + " ON P.ManufacturerID = M.ManufacturerID) left outer join CategoryDisplayOrder DO  " + DB.GetNoLock() + " on p.productid=DO.productid) left outer join ProductVariant PV " + DB.GetNoLock() + " on P.ProductID=PV.ProductID) left outer join SalesPrompt " + DB.GetNoLock() + " on P.SalesPromptID=SalesPrompt.SalesPromptID)";
				sql += " WHERE DO.categoryid=" + CategoryID.ToString() + " and (P.Published = 1) AND (P.Deleted = 0) and (PV.Deleted=0) and PC.categoryid=" + CategoryID.ToString();
			}
			
			if(SectionID != 0)
			{
				sql += " and p.productid in (select distinct productid from productsection  " + DB.GetNoLock() + " where sectionid=" + SectionID.ToString() + ") ";
			}
			if(ManufacturerID != 0)
			{
				sql += " and p.manufacturerID =" + ManufacturerID.ToString();
			}
			if(ProductTypeID != 0)
			{
				sql += " and p.ProductTypeID=" + ProductTypeID.ToString();
			}
			if(Common.AppConfigBool("FilterProductsByAffiliate"))
			{
				sql += " and P.ProductID in (select distinct productid from productaffiliate  " + DB.GetNoLock() + " where affiliateid=" + thisCustomer._affiliateID.ToString() + ")";
			}
			if(Common.AppConfigBool("FilterProductsByCustomerLevel"))
			{
				String FilterOperator = Common.IIF(Common.AppConfigBool("FilterByCustomerLevelIsAscending"),"<=","=");
				sql += " and (P.ProductID in (select distinct productid from productCustomerLevel  " + DB.GetNoLock() + " where CustomerLevelid" + FilterOperator + thisCustomer._customerLevelID.ToString() + ") ";
				if(Common.AppConfigBool("FilterByCustomerLevel0SeesUnmappedProducts"))
				{
					// allow customer level 0 to see any product that is not specifically mapped to any customer levels
					sql += " or P.ProductID not in (select productid from productcustomerlevel)";
				}
				sql += ")";
			}
			if(SortByLooks)
			{
				sql += " order by looks desc, DO.displayorder, p.name";
			}
			else
			{
				sql += " order by DO.displayorder, p.name";
			}

			DataSet ds = DB.GetDS(sql,false,System.DateTime.Now.AddHours(1));
			empty = (ds.Tables[0].Rows.Count == 0);

			if(Common.AppConfigBool("ForceCategoryHeaderDisplay") || CategoryDescription.Length != 0)
			{
				writer.Write("<p align=\"left\">\n");
				int MaxWidth = Common.AppConfigNativeInt("MaxIconWidth");
				if(MaxWidth == 0)
				{
					MaxWidth = 125;
				}
				int MaxHeight = Common.AppConfigNativeInt("MaxIconHeight");
				if(MaxHeight == 0)
				{
					MaxHeight = 125;
				}
				if(CategoryPicture.Length != 0)
				{
					int w = Common.GetImageWidth(CategoryPicture);
					int h = Common.GetImageHeight(CategoryPicture);
					if(w > MaxWidth)
					{
						writer.Write("<img align=\"left\" src=\"" + CategoryPicture + "\" width=\"" + MaxWidth.ToString() + "\" border=\"0\">");
					}
					else if(h > MaxHeight)
					{
						writer.Write("<img align=\"left\" src=\"" + CategoryPicture + "\" height=\"" + MaxHeight + "\" border=\"0\">");
					}
					else
					{
						writer.Write("<img align=\"left\" src=\"" + CategoryPicture + "\" border=\"0\">");
					}
				}
				writer.Write("<b>" + CategoryName + "</b>");
				if(CategoryDescription.Length != 0)
				{
					writer.Write(": " + CategoryDescription);
				}
				writer.Write("</p>\n");
			}

//			if(CategoryID != Common.AppConfigUSInt("IsFeaturedCategoryID"))
//			{
//				writer.Write("<p align=\"justify\">" + Common.AppConfig("CategoryIntroText").Replace("%CAT%","<b>" + CategoryName + "</b>") + "</p>");
//			}

			if(AllowSectionFiltering || AllowManufacturerFiltering || AllowProductTypeFiltering)
			{
				writer.Write("<form id=\"FilterForm\" name=\"FilterForm\" method=\"GET\" action=\"showcategory.aspx" + "\">\n");
				writer.Write("<input type=\"hidden\" name=\"CategoryID\" value=\"" + CategoryID.ToString() + "\">\n");
				DataSet dsst;
				writer.Write("Filter <b>" + CategoryName + "</b> Products By:");
				if(AllowSectionFiltering)
				{
					writer.Write("&nbsp;&nbsp;" + Common.AppConfig("SectionPromptSingular") + ": ");
					writer.Write("<select onChange=\"document.FilterForm.submit()\" style=\"font-size: 9px;\" size=\"1\" name=\"SectionID\">\n");
					writer.Write("<OPTION VALUE=\"0\" " + Common.IIF(SectionID == 0 , " selected " , "") + ">All " + Common.AppConfig("SectionPromptPlural") + "</option>\n");
					String SecSel = Common.GetSectionSelectList(0,String.Empty,0);
					// mark current Section:
					SecSel = SecSel.Replace("<option value=\"" + SectionID.ToString() + "\">","<option value=\"" + SectionID.ToString() + "\" selected>");
					writer.Write(SecSel);
					writer.Write("</select>\n");
				}
				if(AllowManufacturerFiltering)
				{
					writer.Write("&nbsp;&nbsp;Manufacturer: <select size=\"1\" name=\"ManufacturerID\" onChange=\"document.FilterForm.submit();\">\n");
					writer.Write("<OPTION VALUE=\"0\" " + Common.IIF(ManufacturerID == 0 , " selected " , "") + ">All Manufacturers</option>\n");
					dsst = DB.GetDS("select * from Manufacturer  " + DB.GetNoLock() + " where deleted=0 order by displayorder,name",false,System.DateTime.Now.AddHours(3));
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
				}
				if(AllowProductTypeFiltering)
				{
					writer.Write("&nbsp;&nbsp;Product Type: <select size=\"1\" name=\"ProductTypeID\" onChange=\"document.FilterForm.submit();\">\n");
					writer.Write("<OPTION VALUE=\"0\" " + Common.IIF(ProductTypeID == 0 , " selected " , "") + ">All Product Types</option>\n");
					dsst = DB.GetDS("select * from ProductType  " + DB.GetNoLock() + " where deleted=0 order by name",false,System.DateTime.Now.AddHours(3));
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
				}
				writer.Write("</form>\n");
			}
			bool CatHasSubs = Common.CategoryHasSubs(CategoryID);
			
			if(CatHasSubs)
			{
				// write out the subcats first!
				IDataReader rssub = DB.GetRS("select * from category  " + DB.GetNoLock() + " where deleted=0 and published<>0 and parentcategoryid=" + CategoryID.ToString() + " order by displayorder,name");
				bool emptysubs = true;
				if(rssub.Read())
				{
					emptysubs = false;
				}
				// GRID FORMAT:
				int ItemNumber = 1;
				int ItemsPerRow = Common.AppConfigUSInt("DefaultCategoryColWidth");
				if(!emptysubs)	
				{
					try
					{
						int tmpItemsPerRow = 0;
						if(!empty)
						{
							tmpItemsPerRow = Localization.ParseUSInt(ds.Tables[0].Rows[0]["ColWidth"].ToString());
						}
						if (tmpItemsPerRow != 0)
						{
							ItemsPerRow = tmpItemsPerRow;
						}
					}
					catch {} // ignore empty ds}
				}
				if(ItemsPerRow == 0)
				{
					ItemsPerRow = 4;
				}
				//writer.Write("<tr><td colspan=\"" + ItemsPerRow.ToString() + "\">" + CategoryDescription + "</td></tr>");
				//writer.Write("<tr><td colspan=\"" + ItemsPerRow.ToString() + "\"></td></tr>");

				if(!emptysubs)
				{
					//writer.Write("<br clear=\"all\"><hr size=1>\n");
					if(!Common.AppConfigBool("ShowSubcatsInGrid"))
					{
						writer.Write("<p align=\"left\"><b>This category contains the following subcategories:</b>\n");
						do
						{
							writer.Write("&nbsp;&nbsp;&nbsp;<img src=\"skins/skin_" + _siteID.ToString() + "/images/redarrow.gif\" align=\"absmiddle\">&nbsp;<a href=\"" + SE.MakeCategoryLink(DB.RSFieldInt(rssub,"CategoryID"),DB.RSField(rssub,"SEName")) + "\">");
							writer.Write(DB.RSField(rssub,"Name") + "</a>");
						} while(rssub.Read());
						writer.Write("</p>\n");
						rssub.Close();
					}
					else
					{

						//writer.Write("<p align=\"left\"><b>This category contains the following subcategories:</b></p>\n");
						writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"4\" width=\"100%\">");
						do
						{
							if(ItemNumber == 1)
							{
								writer.Write("<tr>");
							}
							if(ItemNumber == ItemsPerRow+1)
							{
								writer.Write("</tr><tr><td colspan=\"" + ItemsPerRow.ToString() + "\" height=\"8\"></td></tr>");
								ItemNumber=1;
							}
							writer.Write("<td width=\"" + ((int)(100/ItemsPerRow)).ToString() + "%\" align=\"center\" valign=\"top\">");
							String ImgUrl = Common.LookupImage("Category",DB.RSFieldInt(rssub,"CategoryID"),"icon",_siteID);
							if(ImgUrl.Length == 0)
							{
								ImgUrl = Common.AppConfig("NoPicture");
							}
							if(ImgUrl.Length != 0)
							{
								writer.Write("<img style=\"cursor: hand;\" alt=\"" + DB.RSField(rssub,"SEAltText") + "\" onClick=\"self.location='" + SE.MakeCategoryLink(DB.RSFieldInt(rssub,"CategoryID"),DB.RSField(rssub,"SEName")) + "';\" style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + ImgUrl + "\">");
								writer.Write("<br>");
							}
							writer.Write("<a href=\"" + SE.MakeCategoryLink(DB.RSFieldInt(rssub,"CategoryID"),DB.RSField(rssub,"SEName")) + "\">");
							writer.Write(DB.RSField(rssub,"Name") + "</a>");
							writer.Write("</td>");
							ItemNumber++;
						} while (rssub.Read());
						rssub.Close();
						for(int i = ItemNumber; i<=ItemsPerRow; i++)
						{
							writer.Write("<td>&nbsp;</td>");
						}
						writer.Write("</tr>");
						writer.Write("</table>");
					}
				}
				rssub.Close();
			}

			if(empty)
			{
				if(Common.AppConfigBool("ForceCategoryHeaderDisplay") || CategoryDescription.Length != 0)
				{
					if(!CatHasSubs)
					{
						writer.Write("<br clear=\"all\">\n");
					}
				}
				if(CatHasSubs && !Common.AppConfigBool("ShowSubcatsInGrid"))
				{
					writer.Write("<p align=\"center\"><b><br><br><br>Please select a subcategory above</b></p>\n");
				}
				else
				{
					if(AllowSectionFiltering || AllowManufacturerFiltering || AllowProductTypeFiltering)
					{
						Topic t = new Topic("EmptyCategoryText",thisCustomer._localeSetting,_siteID);
						writer.Write(t._contents);
					}
					else
					{
						if(!CatHasSubs)
						{
							writer.Write("<p align=\"center\"><b><br><br><br>No products found.");
							writer.Write("&nbsp;Please check back later or <a href=\"" + SE.MakeDriverLink("Contact") + "\">contact us</a>.<br><br>We are in the process of building our online store.");
							writer.Write("</b></p>\n");
						}
					}
				}
			}
			else
			{
				DataRow row1 = ds.Tables[0].Rows[0];
				if(Common.AppConfigBool("ForceCategoryHeaderDisplay") || CategoryDescription.Length != 0)
				{
					writer.Write("<br clear=\"all\"><hr size=1>\n");
				}
				
				int PageSize = Common.AppConfigUSInt("CategoryGridPageSize");
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
							Response.Redirect("showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + (PageNum-1).ToString());
						}
					}
				}
				int StartRow = (PageSize*(PageNum-1)) + 1;
				int StopRow = StartRow + PageSize -1 ;
				if(StopRow > NumRows)
				{
					StopRow = NumRows;
				}
				int rowidx = 1; // 1 based!
				int ItemNumber = 1;
				int ItemsPerRow = Common.AppConfigUSInt("DefaultCategoryColWidth");
				if(!empty)	
				{
					int tmpItemsPerRow = DB.RowFieldInt(row1,"ColWidth");
					if (tmpItemsPerRow != 0)
					{
						ItemsPerRow = tmpItemsPerRow;
					}
				}
				if(ItemsPerRow == 0)
				{
					ItemsPerRow = 4;
				}

				int DispFormatID = DB.RowFieldInt(row1,"CategoryDisplayFormatID");
				if(DispFormatID == 1 || DispFormatID == 2 || DispFormatID == 3 || DispFormatID == 7 || DispFormatID == 8)
				{
					if(pagingOn && NumRows >= PageSize && (NumPages > 1 || Common.QueryString("show") == "all"))
					{
						if(Common.AppConfig("PageNumberFormat").ToUpper() == "NEXT_PREV")
						{
							writer.Write("<p class=\"PageNumber\" align=\"left\">Showing items " + StartRow.ToString() + "-" + StopRow.ToString() + " of " + NumRows.ToString());
							if(NumPages > 1)
							{
								writer.Write(" (");
								if(PageNum > 1)
								{
									//writer.Write("<a href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=1\">First Page</a>");
									//writer.Write(" | ");
									writer.Write("<a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + (PageNum-1).ToString() + "\">Previous Page</a>");
								}
								if(PageNum > 1 && PageNum < NumPages)
								{
									writer.Write(" | ");
								}
								if(PageNum < NumPages)
								{
									writer.Write("<a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + (PageNum+1).ToString() + "\">Next Page</a>");
									//writer.Write(" | ");
									//writer.Write("<a href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + NumPages.ToString() + "\">Last Page</a>");
								}
								writer.Write(")");
								writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;Click <a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&show=all\">here</a> to see all items");
							}
						}
						else
						{
							writer.Write("<p class=\"PageNumber\" align=\"left\">");
							if(Common.QueryString("show") == "all")
							{
								writer.Write("Click <a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=1\">here</a> to return to paging");
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
										writer.Write("<a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + u.ToString() + "\">" + u.ToString() + "</a>&nbsp;&nbsp;");
									}
								}
								writer.Write("&nbsp;&nbsp;<a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&show=all\">all</a>");
							}
						}
						writer.Write("</p>\n");
					}
				}
				switch(DispFormatID)
				{

					case 1:
						// GRID FORMAT:
						writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"4\" width=\"100%\">");
						//writer.Write("<tr><td colspan=\"" + ItemsPerRow.ToString() + "\">" + CategoryDescription + "</td></tr>");
						//writer.Write("<tr><td colspan=\"" + ItemsPerRow.ToString() + "\"></td></tr>");

						if(empty)
						{
							Topic t = new Topic("EmptyCategoryText",thisCustomer._localeSetting,_siteID);
							writer.Write(t._contents);
						}
						else
						{
							foreach(DataRow row in ds.Tables[0].Rows)
							{
								if(rowidx >= StartRow && rowidx <= StopRow)
								{
									if(ItemNumber == 1)
									{
										writer.Write("<tr>");
									}
									if(ItemNumber == ItemsPerRow+1)
									{
										writer.Write("</tr><tr><td colspan=\"" + ItemsPerRow.ToString() + "\" height=\"8\"></td></tr>");
										ItemNumber=1;
									}

									writer.Write("<td width=\"" + ((int)(100/ItemsPerRow)).ToString() + "%\" align=\"center\" valign=\"bottom\">");
									String ImgUrl = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",_siteID);
									if(ImgUrl.Length == 0)
									{
										ImgUrl = Common.AppConfig("NoPicture");
									}
									if(ImgUrl.Length != 0)
									{
//										Single w = (Single)Common.GetImageWidth(ImgUrl);
//										Single h = (Single)Common.GetImageHeight(ImgUrl);
//										Single finalW = w;
//										Single finalH = h;
//										if(w > 250)
//										{
//											finalH = h * 250/w;
//											finalW = 250;
//										}
//										if(finalH > 250)
//										{
//											finalW = finalW * 250/finalH;
//											finalH = 250;
//										}
										writer.Write("<img style=\"cursor: hand;\" alt=\"" + DB.RowField(row,"PSEAltText") + "\" onClick=\"self.location='" + SE.MakeProductAndCategoryLink(DB.RowFieldInt(row,"ProductID"),CategoryID,DB.RowField(row,"PSEName")) + "';\" style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + ImgUrl + "\">");
										writer.Write("<br><br>");
									}
									writer.Write("<a href=\"" + SE.MakeProductAndCategoryLink(DB.RowFieldInt(row,"ProductID"),CategoryID,DB.RowField(row,"PSEName")) + "\">");
									writer.Write(DB.RowField(row,"Name") + "</a>");

									if(Common.AppConfigBool("ShowPriceInCategoryGrid"))
									{
										writer.Write("<br>");
										bool IsOnSale = false;
										int VariantID = Common.GetFirstProductVariant(DB.RowFieldInt(row,"ProductID"));
										int CustomerLevelID = thisCustomer._customerLevelID;
										decimal YourPR = Common.DetermineLevelPrice(VariantID,CustomerLevelID,out IsOnSale);
										decimal RegularPR = Common.GetVariantPrice(VariantID);
										int Points = Common.GetVariantPoints(VariantID);
										if(CustomerLevelID == 0)
										{
											writer.Write("<font class=\"ShowPriceRegularPrompt\">" + Common.AppConfig("ShowPriceRegularPrompt") + ": " + Localization.CurrencyStringForDisplay(RegularPR) + "</font> " + Common.IIF(thisCustomer._customerLevelID != 0 && Common.AppConfigBool("MicroPay.ShowPointsWithPrices"), "(" + Points.ToString() + " Points)", ""));
										}
										else
										{
											writer.Write("<font class=\"ShowPriceExtendedPrompt\">" + Common.AppConfig("ShowPriceExtendedPrompt") + ": " + Localization.CurrencyStringForDisplay(YourPR) + "</font> " + Common.IIF(thisCustomer._customerLevelID != 0 && Common.AppConfigBool("MicroPay.ShowPointsWithPrices"), "(" + Points.ToString() + " Points)", ""));
										}
										if(IsOnSale)
										{
											writer.Write("<br><font class=\"ShowPriceSalePrompt\">" + Common.AppConfig("ShowPriceSalePrompt") + ": " + Localization.CurrencyStringForDisplay(YourPR) + "</font>");
										}
									}
									
									writer.Write("</td>");
									ItemNumber++;
								}
								rowidx++;
							}
							for(int i = ItemNumber; i<=ItemsPerRow; i++)
							{
								writer.Write("<td>&nbsp;</td>");
							}
							writer.Write("</tr>");
						}
						writer.Write("</table>");
						break;
			
					case 2:
						// TABLE - EXPANDED FORMAT:
						
						if(pagingOn && NumRows >= PageSize && (NumPages > 1 || Common.QueryString("show") == "all"))
						{
							if(Common.AppConfig("PageNumberFormat").ToUpper() == "NEXT_PREV")
							{
								writer.Write("<p class=\"PageNumber\" align=\"left\">Showing items " + StartRow.ToString() + "-" + StopRow.ToString() + " of " + NumRows.ToString());
								if(NumPages > 1)
								{
									writer.Write(" (");
									if(PageNum > 1)
									{
										//writer.Write("<a href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=1\">First Page</a>");
										//writer.Write(" | ");
										writer.Write("<a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + (PageNum-1).ToString() + "\">Previous Page</a>");
									}
									if(PageNum > 1 && PageNum < NumPages)
									{
										writer.Write(" | ");
									}
									if(PageNum < NumPages)
									{
										writer.Write("<a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + (PageNum+1).ToString() + "\">Next Page</a>");
										//writer.Write(" | ");
										//writer.Write("<a href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + NumPages.ToString() + "\">Last Page</a>");
									}
									writer.Write(")");
									writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;Click <a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&show=all\">here</a> to see all items");
								}
							}
							else
							{
								writer.Write("<p class=\"PageNumber\" align=\"left\">");
								if(Common.QueryString("show") == "all")
								{
									writer.Write("Click <a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=1\">here</a> to return to paging");
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
											writer.Write("<a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + u.ToString() + "\">" + u.ToString() + "</a>&nbsp;&nbsp;");
										}
									}
									writer.Write("&nbsp;&nbsp;<a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&show=all\">all</a>");
								}
							}
							writer.Write("</p>\n");
						}

						foreach(DataRow row in ds.Tables[0].Rows)
						{
							if(rowidx >= StartRow && rowidx <= StopRow)
							{
								WriteProductBar(writer,row,DB.RowFieldInt(row,"ProductID"),CategoryID,_siteID);
							}
							rowidx++;
						}

						if(pagingOn && NumRows >= PageSize && (NumPages > 1 || Common.QueryString("show") == "all"))
						{
							if(Common.AppConfig("PageNumberFormat").ToUpper() == "NEXT_PREV")
							{
								writer.Write("<p class=\"PageNumber\" align=\"left\">Showing items " + StartRow.ToString() + "-" + StopRow.ToString() + " of " + NumRows.ToString());
								if(NumPages > 1)
								{
									writer.Write(" (");
									if(PageNum > 1)
									{
										//writer.Write("<a href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=1\">First Page</a>");
										//writer.Write(" | ");
										writer.Write("<a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + (PageNum-1).ToString() + "\">Previous Page</a>");
									}
									if(PageNum > 1 && PageNum < NumPages)
									{
										writer.Write(" | ");
									}
									if(PageNum < NumPages)
									{
										writer.Write("<a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + (PageNum+1).ToString() + "\">Next Page</a>");
										//writer.Write(" | ");
										//writer.Write("<a href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + NumPages.ToString() + "\">Last Page</a>");
									}
									writer.Write(")");
									writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;Click <a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&show=all\">here</a> to see all items");
								}
							}
							else
							{
								writer.Write("<p class=\"PageNumber\" align=\"left\">");
								if(Common.QueryString("show") == "all")
								{
									writer.Write("Click <a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=1\">here</a> to return to paging");
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
											writer.Write("<a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + u.ToString() + "\">" + u.ToString() + "</a>&nbsp;&nbsp;");
										}
									}
									writer.Write("&nbsp;&nbsp;<a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&show=all\">all</a>");
								}
							}
							writer.Write("</p>\n");
						}

						break;
					case 3:
						// TABLE - CONDENSED FORMAT:
						writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
						writer.Write("    <tr class=\"DarkCell\">\n");
						writer.Write("      <td><font class=\"CondensedDarkCellText\"><b>Photo</b></font></td>\n");
						writer.Write("      <td><font class=\"CondensedDarkCellText\"><b>Product</b></font></td>\n");
						writer.Write("      <td><font class=\"CondensedDarkCellText\"><b>Manufacturer</b></font></td>\n");
						writer.Write("      <td><font class=\"CondensedDarkCellText\"><b>SKU</b></font></td>\n");
						writer.Write("      <td><font class=\"CondensedDarkCellText\"><b>More Info</b></font></td>\n");
						writer.Write("    </tr>\n");
						int rownum = 1;
						foreach(DataRow row in ds.Tables[0].Rows)
						{
							writer.Write("<tr " + Common.IIF(rownum % 2 == 0 , "class=\"LightCell\"" , "") + ">\n");

							writer.Write("<td valign=\"middle\" align=\"center\">");

							String Image1URL = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",_siteID);
							if(Image1URL.Length == 0)
							{
								Image1URL = Common.AppConfig("NoPictureIcon");
							}
							int HT = Common.AppConfigUSInt("CondensedTablePictureHeight");
							if(HT == 0)
							{
								HT = 50;
							}

							writer.Write("<img style=\"cursor: hand;\" alt=\"" + DB.RowField(row,"PSEAltText") + "\" OnClick=\"self.location='" + SE.MakeProductAndCategoryLink(DB.RowFieldInt(row,"ProductID"),CategoryID,DB.RowField(row,"PSEName"))  + "';\" style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + Image1URL + "\" height=\"" + HT.ToString() + "\" align=\"absmiddle\">");
							//writer.Write("</a>");
							writer.Write("&nbsp;\n");
							writer.Write("</td>");
					
							writer.Write("<td>");
							writer.Write("<a href=\"" + SE.MakeProductAndCategoryLink(DB.RowFieldInt(row,"ProductID"),CategoryID,DB.RowField(row,"PSEName")) + "\">");
							writer.Write(DB.RowField(row,"Name"));
							writer.Write("</a>");
							writer.Write("</td>\n");
					
							writer.Write("<td>" + DB.RowField(row,"ManufacturerName") + "</td>\n");
							writer.Write("<td>" + DB.RowField(row,"SKU") +  "</td>\n");

							writer.Write("<td>");
							writer.Write("<a href=\"" + SE.MakeProductAndCategoryLink(DB.RowFieldInt(row,"ProductID"),CategoryID,DB.RowField(row,"PSEName"))  + "\">");
							writer.Write("<img style=\"cursor:hand;\" src=\"skins/skin_" + _siteID.ToString() + "/images/moreinfo.gif\" border=\"0\" align=\"absmiddle\">");
							writer.Write("</a>&nbsp;\n");
							writer.Write("</td>\n");
							writer.Write("</tr>\n");
							rownum++;
						};
						writer.Write("</table>");
						break;
					case 4:
						// SIMPLE PRODUCT LIST
						int PageSize2 = Common.AppConfigUSInt("CategoryGridPageSize");
						if(PageSize2 == 0)
						{
							PageSize2 = 20;
						}
						writer.Write(Common.GetSimpleProductListBox("Category",false,thisCustomer,"showcategory.aspx", PageSize2, _siteID,CategoryID,SectionID,ManufacturerID,ProductTypeID,false,false,""));
						break;
					case 5:
						// TABLE ORDER FORMAT:
						SectionID = 0;
						writer.Write("<div align=\"left\">\n");
						writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
						writer.Write("function TableOrderForm_Validator(theForm)\n");
						writer.Write("	{\n");
						writer.Write("	return (true);\n");
						writer.Write("	}\n");
						writer.Write("</script>\n");

						//writer.Write("<form method=\"POST\" name=\"TableOrderForm\" id=\"TableOrderForm\" action=\"tableorder_process.aspx\" onsubmit=\"return validateForm(this) && TableOrderForm_Validator(this)\" >\n");
						writer.Write("<form method=\"POST\" name=\"TableOrderForm\" id=\"TableOrderForm\" action=\"tableorder_process.aspx\" >\n");

						sql = "SELECT     TOP 100 PERCENT CategoryDisplayOrder.DisplayOrder, ProductVariant.VariantID, ProductVariant.Name AS VariantName, ProductVariant.SKUSuffix, ProductVariant.Description AS VariantDescription, ProductVariant.Price, ProductVariant.SalePrice, ProductVariant.MSRP, ProductVariant.Cost, ProductVariant.Inventory, ProductVariant.DisplayOrder AS VariantDisplayOrder, ProductVariant.Colors, ProductVariant.ColorSKUModifiers, ProductVariant.Sizes, ProductVariant.SizeSKUModifiers, ProductVariant.Deleted AS VariatnDeleted, ProductVariant.Published AS VariantPublished, CategoryDisplayOrder.CategoryID, Product.ProductID, Product.ProductGUID, Product.Name, Product.ProductTypeID, Product.Summary, Product.Description, Product.SKU, Product.ManufacturerID, Product.ManufacturerPartNumber, Product.SalesPromptID, Product.Published, Product.RequiresRegistration, Product.Deleted, Product.IsAKit, Product.IsAPack, Product.ShowBuyButton, Product.HidePriceUntilCart, Product.SizeOptionPrompt, Product.ColorOptionPrompt,SalesPrompt.name as SDescription ";
						sql += " FROM (((Product  " + DB.GetNoLock() + " INNER JOIN ProductVariant  " + DB.GetNoLock() + " ON Product.ProductID = ProductVariant.ProductID) INNER JOIN CategoryDisplayOrder  " + DB.GetNoLock() + " ON Product.ProductID = CategoryDisplayOrder.ProductID) inner join SalesPrompt " + DB.GetNoLock() + " on product.salespromptid=salesprompt.salespromptid) WHERE (ProductVariant.Published = 1) AND (ProductVariant.Deleted = 0) AND (CategoryDisplayOrder.CategoryID = " + CategoryID.ToString() + ") AND (Product.HidePriceUntilCart = 0) AND (Product.ShowBuyButton = 1) AND (Product.IsAPack = 0) AND (Product.IsAKit = 0) AND (Product.Deleted = 0) AND (Product.RequiresRegistration = 0 OR Product.RequiresRegistration IS NULL) AND (Product.Published = 1) ";


//						sql = "SELECT Product.ProductID, Product.HidePriceUntilCart, Product.IsCallToOrder, Product.Summary, Product.Description, Product.ProductTypeID, Product.RelatedProducts, Product.HidePriceUntilCart, Product.Name, Product.SEName, Product.SpecTitle, Product.SpecsInline, Product.SpecCall, Product.ProductDisplayFormatID, Product.ColWidth, Product.Summary, Product.Description, Product.SEKeywords, Product.SEDescription, Product.SKU, Product.ManufacturerPartNumber, Product.Published, Product.Deleted, ProductVariant.VariantID, ProductVariant.FroogleDescription, ProductVariant.Name AS VariantName, ProductVariant.Colors, ProductVariant.Sizes, ProductVariant.Dimensions, ProductVariant.Weight, ProductVariant.Inventory, ProductVariant.SKUSuffix, ProductVariant.ManufacturerPartNumber AS VManufacturerPartNumber, ProductVariant.Price, ProductVariant.SalePrice, ProductVariant.Deleted AS VariantDeleted, ProductVariant.Description as VariantDescription, ProductVariant.Published AS VariantPublished, ProductVariant.price, ProductVariant.SalePrice FROM  Product " + DB.GetNoLock() + " ,ProductVariant  " + DB.GetNoLock() + " ";
//						if(CategoryID != 0)
//						{
//							sql += ",categorydisplayorder";
//						}
//						sql += " where Product.ProductID = ProductVariant.ProductID ";
//						if(CategoryID != 0)
//						{
//							sql += " and product.productid=categorydisplayorder.productid and categorydisplayorder.CategoryID=" + CategoryID.ToString();
//						}
//						if(SectionID != 0)
//						{
//							sql += " and product.productid=sectiondisplayorder.productid and sectiondisplayorder.sectionid=" + SectionID.ToString();
//						}
//						sql += " and (Product.RequiresRegistration IS NULL or Product.RequiresRegistration=0) and product.published=1 and Product.Deleted=0 and ProductVariant.Published=1 AND ProductVariant.Deleted=0 and Product.IsAKit=0 and Product.IsAPack=0 ";
//						if(CategoryID != 0)
//						{
//							sql += " and product.productid in (select distinct productid from productcategory  " + DB.GetNoLock() + " where CategoryID=" + CategoryID.ToString() + ")";
//							sql += " and categorydisplayorder.productid in (select distinct productid from productcategory  " + DB.GetNoLock() + " where CategoryID=" + CategoryID.ToString() + ")";
//						}
						if(SectionID != 0)
						{
							sql += " and product.productid in (select distinct productid from productsection  " + DB.GetNoLock() + " where SectionID=" + SectionID.ToString() + ")";
							//sql += " and sectiondisplayorder.productid in (select distinct productid from productsection  " + DB.GetNoLock() + " where SectionID=" + SectionID.ToString() + ")";
						}
						if(ManufacturerID != 0)
						{
							sql += " and product.ManufacturerID=" + ManufacturerID.ToString();
						}
//						if(CategoryID != 0)
//						{
//							sql += " order by categorydisplayorder.displayorder, product.name, ProductVariant.DisplayOrder, ProductVariant.Name";
//						}
//						else if(SectionID != 0)
//						{
//							sql += " order by sectiondisplayorder.displayorder, product.name, ProductVariant.DisplayOrder, ProductVariant.Name";
//						}
//						else
//						{
//							sql += " ORDER by Product.Name, ProductVariant.DisplayOrder, ProductVariant.Name";
//						}

						sql += " ORDER BY CategoryDisplayOrder.DisplayOrder, ProductVariant.DisplayOrder";
						//writer.Write("SQL=" + sql + "<br>");
						DataSet ds2 = DB.GetDS(sql,false);

						writer.Write("<div align=\"left\">This page lets you easily add <b>multiple</b> items to your cart all at one time.<br><br>For each style, you can enter a quantity by color/size that you want to add to your " + Common.AppConfig("CartPrompt").ToLower() + " and then click on the <b>\"" + Common.AppConfig("CartButtonPrompt") + "\"</b> button at the bottom of the page.<br>&nbsp;</div>");
						writer.Write("<table cellpadding=\"4\" border=\"0\" cellspacing=\"0\">");

						bool CustomerLevelAllowsQuantityDiscounts = Common.CustomerLevelAllowsQuantityDiscounts(thisCustomer._customerLevelID);

						bool anyFound = false;
						foreach(DataRow row in ds2.Tables[0].Rows)
						{
							anyFound = true;

							int ProductID = DB.RowFieldInt(row,"ProductID");
							int ActiveDIDID = Common.LookupActiveProductQuantityDiscountID(ProductID);
							bool ActiveDID = (ActiveDIDID != 0);
							if(!CustomerLevelAllowsQuantityDiscounts)
							{
								ActiveDIDID = 0;
								ActiveDID = false;
							}
				
							String ProductName = DB.RowField(row,"Name") + Common.IIF(DB.RowField(row,"VariantName").Length != 0, " - " + DB.RowField(row,"VariantName"), "");
							String ProductDescription = DB.RowField(row,"Description"); //.Replace("\n","<br>");
							if(Common.AppConfigBool("ShowSummaryInTableOrderFormat"))
							{
								ProductDescription = DB.RowField(row,"Summary");
							}
							String FileDescription = new ProductDescriptionFile(ProductID,thisCustomer._localeSetting,_siteID)._contents;
							if(FileDescription.Length != 0)
							{
								ProductDescription += "<br>" + FileDescription;
							}
							String ProductPicture = Common.LookupImage("Product",ProductID,"medium",_siteID);
							String LargePic = Common.LookupImage("Product",ProductID,"large",_siteID);
							bool HasLargePic = (LargePic.Length != 0);
							if(ProductPicture.Length == 0)
							{
								ProductPicture = Common.LookupImage("Product",ProductID,"icon",_siteID);
							}
							if(ProductPicture.Length == 0)
							{
								ProductPicture = Common.AppConfig("NoPicture");
							}

							String LargeImage = String.Empty;
							if(HasLargePic)
							{
								LargeImage = LargePic;
								int LargePicW = Common.GetImageWidth(LargeImage);
								int LargePicH = Common.GetImageHeight(LargeImage);
								writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
								writer.Write("function popup" + ProductID.ToString() + "_" + DB.RowFieldInt(row,"VariantID").ToString() + "(url)\n");
								writer.Write("	{\n");
								writer.Write("	window.open('popup.aspx?src=' + url,'LargerImage" + ProductID.ToString() + Common.GetRandomNumber(1,100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no,copyhistory=no,width=" + LargePicW.ToString() + ",height=" + LargePicH.ToString() + ",left=0,top=0');\n");
								writer.Write("	return (true);\n");
								writer.Write("	}\n");
								writer.Write("</script>\n");
							}


							writer.Write("<tr>");
							writer.Write("<td align=\"center\" valign=\"top\" >");
							if(ProductPicture.Length != 0)
							{
								writer.Write("<img " + Common.IIF(HasLargePic,"style=\"cursor: hand;\" alt=\"View Larger Image\" onClick=\"javascript:popup" + ProductID.ToString() + "_" + DB.RowFieldInt(row,"VariantID").ToString() + "('" + Server.UrlEncode(LargePic) + "');\"","") + " src=\"" + ProductPicture + "?" + Common.GetRandomNumber(1,100000).ToString() + "\">");
								writer.Write("<br>");
								writer.Write("");
								if(HasLargePic) 
								{
									writer.Write("<a href=\"javascript:void(0);\" onClick=\"javascript:popup" + ProductID.ToString() + "_" + DB.RowFieldInt(row,"VariantID").ToString() + "('" + Server.UrlEncode(LargePic) + "');\">View Larger Image</a><br>");
								}
							}

							writer.Write("</td>");
							writer.Write("<td align=\"left\" valign=\"top\" >");

							writer.Write("<span class=\"ProductNameText\">" + ProductName + "</span><br><br>\n");
							writer.Write("<div align=\"left\">");
							writer.Write("SKU: " + Common.MakeProperProductSKU(DB.RowField(row,"SKU"),DB.RowField(row,"SKUSuffix"),"",""));
							writer.Write("</div>");
							writer.Write("<br>");

							writer.Write("<div align=\"left\">");
							writer.Write(ProductDescription);
							writer.Write("</div>");
				
							writer.Write("<div align=\"left\">");
							if(!DB.RowFieldBool(row,"HidePriceUntilCart"))
							{
								writer.Write("<table>");
								if(thisCustomer._customerLevelID == 0)
								{
									// show consumer (e.g. level 0) pricing:
									if(DB.RowFieldDecimal(row,"SalePrice") != System.Decimal.Zero)
									{
										writer.Write("<tr valign=\"top\"><td width=150><b>Price:</b></td><td><b><strike>" + Localization.CurrencyStringForDisplay( DB.RowFieldSingle(row,"Price")) + "</strike>&nbsp;&nbsp;&nbsp;<font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + DB.RowField(row,"sDescription") + ":</font> <font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + Localization.CurrencyStringForDisplay( DB.RowFieldSingle(row,"SalePrice")) + "</font></b></td></tr>");
										writer.Write("<tr valign=\"top\"><td colspan=2 height=10></td></tr>");
									}
									else
									{
										writer.Write("<tr valign=\"top\"><td width=150><b>Price:</b></td><td><b>" + Localization.CurrencyStringForDisplay( DB.RowFieldSingle(row,"Price")) + "</b></td></tr>");
										writer.Write("<tr valign=\"top\"><td colspan=2 height=10></td></tr>");
									}
								}
								else
								{
									// show level pricing:
									bool IsOnSale = false;
									decimal LevelPrice = Common.DetermineLevelPrice(DB.RowFieldInt(row,"VariantID"),thisCustomer._customerLevelID, out IsOnSale);
									writer.Write("<tr valign=\"top\"><td width=150><b>Regular Price:</b></td><td><b>" + Localization.CurrencyStringForDisplay( DB.RowFieldSingle(row,"Price")) + "</b></td></tr>");
									writer.Write("<tr valign=\"top\"><td colspan=2 height=10></td></tr>");
									writer.Write("<tr valign=\"top\"><td width=150><b><font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + thisCustomer._customerLevelName + " Price:</font></b></td><td><b><font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + Localization.CurrencyStringForDisplay( LevelPrice) + "</font></b></td></tr>");
									writer.Write("<tr valign=\"top\"><td colspan=2 height=10></td></tr>");
								}
								writer.Write("</table>");
							}
							writer.Write("</div>");

				
							String Sizes = DB.RowField(row,"Sizes");
							String Colors = DB.RowField(row,"Colors");

							String[] ColorsSplit = Colors.Split(',');
							String[] SizesSplit = Sizes.Split(',');
			
							writer.Write("<table border=\"0\" cellpadding=\"1\" cellspacing=\"1\" style=\"border-style: solid; border-color: #EEEEEE; border-width: 1px;\">\n");
							writer.Write("<tr>\n");
							writer.Write("<td valign=\"middle\" align=\"left\"><b>Color/Size</b></td>\n");
							for(int i = SizesSplit.GetLowerBound(0); i <= SizesSplit.GetUpperBound(0); i++)
							{
								writer.Write("<td valign=\"middle\" align=\"center\">" + SizesSplit[i].Trim() + "</td>\n");
							}
							writer.Write("</tr>\n");
							for(int i = ColorsSplit.GetLowerBound(0); i <= ColorsSplit.GetUpperBound(0); i++)
							{
								writer.Write("<tr>\n");
								writer.Write("<td valign=\"middle\" align=\"right\" >" + ColorsSplit[i].Trim() + "</td>\n");
								for(int j = SizesSplit.GetLowerBound(0); j <= SizesSplit.GetUpperBound(0); j++)
								{
									writer.Write("<td valign=\"middle\" align=\"center\">");
									String FldName = DB.RowFieldInt(row,"ProductID").ToString() + "_" + DB.RowFieldInt(row,"VariantID").ToString() + "_" + i.ToString() + "_" + j.ToString();
									//writer.Write(FldName);
									writer.Write("<input name=\"Qty_" + FldName + "\" type=\"text\" size=\"3\" maxlength=\"3\">");
									//writer.Write("<input name=\"Qty_" + FldName + "_vldt\" type=\"hidden\" value=\"[number][invalidalert=Please enter a single number, e.g. 1, 4, 10]\">");
									writer.Write("</td>\n");
								}
								writer.Write("</tr>\n");
							}
							writer.Write("</table>\n");
							writer.Write("</td>");
							writer.Write("</tr>");
							writer.Write("<tr><td colspan=2 height=15><hr size=1></td></tr>");
						}
						ds2.Dispose();
						writer.Write("</table>");

						writer.Write("<br><br>");
						if(anyFound)
						{
							writer.Write("<center><input type=\"Submit\" value=\"" + Common.AppConfig("CartButtonPrompt") + "\"></center>");
						}
						else
						{
							writer.Write("<center><b>No Products Found</b></center>");
						}
						writer.Write("</form>");
						writer.Write("</div>");
						break;
					case 6:
						// ONE LINE PRODUCT LIST
						int PageSize3 = Common.AppConfigUSInt("CategoryGridPageSize");
						if(PageSize3 == 0)
						{
							PageSize3 = 40;
						}
						writer.Write(Common.GetSimpleProductListBox("Category", true,thisCustomer,"showcategory.aspx", PageSize3, _siteID,CategoryID,SectionID,ManufacturerID,ProductTypeID,false,false,""));
						break;
					case 7:
						// TABLE EXPANDED 2:
						SectionID = 0;
						writer.Write("<div align=\"left\">\n");
						sql = "SELECT TOP 100 PERCENT CategoryDisplayOrder.DisplayOrder, ProductVariant.VariantID, ProductVariant.Name AS VariantName, ProductVariant.SKUSuffix, ProductVariant.Description AS VariantDescription, ProductVariant.Price, ProductVariant.SalePrice, ProductVariant.MSRP, ProductVariant.Cost, ProductVariant.Inventory, ProductVariant.DisplayOrder AS VariantDisplayOrder, ProductVariant.Colors, ProductVariant.ColorSKUModifiers, ProductVariant.Sizes, ProductVariant.SizeSKUModifiers, ProductVariant.Deleted AS VariatnDeleted, ProductVariant.Published AS VariantPublished, CategoryDisplayOrder.CategoryID, Product.ProductID, Product.ProductGUID, Product.Name, Product.ProductTypeID, Product.Summary, Product.Description, Product.SKU, Product.ManufacturerID, Product.ManufacturerPartNumber, Product.SalesPromptID, Product.Published, Product.RequiresRegistration, Product.Deleted, Product.IsAKit, Product.IsAPack, Product.ShowBuyButton, Product.HidePriceUntilCart, Product.SizeOptionPrompt, Product.MiscText as ProductMiscText, Product.ColorOptionPrompt,SalesPrompt.name as SDescription ";
						sql += " FROM (((Product " + DB.GetNoLock() + " INNER JOIN ProductVariant  " + DB.GetNoLock() + " ON Product.ProductID = ProductVariant.ProductID) INNER JOIN CategoryDisplayOrder  " + DB.GetNoLock() + " ON Product.ProductID = CategoryDisplayOrder.ProductID) inner join SalesPrompt " + DB.GetNoLock() + " on product.salespromptid=salesprompt.salespromptid) WHERE (ProductVariant.Published = 1) AND (ProductVariant.Deleted = 0) AND (CategoryDisplayOrder.CategoryID = " + CategoryID.ToString() + ") AND (Product.HidePriceUntilCart = 0) AND (Product.ShowBuyButton = 1) AND (Product.IsAPack = 0) AND (Product.IsAKit = 0) AND (Product.Deleted = 0) AND (Product.RequiresRegistration = 0 OR Product.RequiresRegistration IS NULL) AND (Product.Published = 1) ";

						if(SectionID != 0)
						{
							sql += " and product.productid in (select distinct productid from productsection  " + DB.GetNoLock() + " where SectionID=" + SectionID.ToString() + ")";
						}
						if(ManufacturerID != 0)
						{
							sql += " and product.ManufacturerID=" + ManufacturerID.ToString();
						}

						sql += " ORDER BY CategoryDisplayOrder.DisplayOrder, ProductVariant.DisplayOrder";
						ds2 = DB.GetDS(sql,false);

						writer.Write("<table cellpadding=\"4\" border=\"0\" cellspacing=\"0\">");

						CustomerLevelAllowsQuantityDiscounts = Common.CustomerLevelAllowsQuantityDiscounts(thisCustomer._customerLevelID);

						anyFound = false;
						foreach(DataRow row in ds2.Tables[0].Rows)
						{
							anyFound = true;

							int ProductID = DB.RowFieldInt(row,"ProductID");
							int ActiveDIDID = Common.LookupActiveProductQuantityDiscountID(ProductID);
							bool ActiveDID = (ActiveDIDID != 0);
							if(!CustomerLevelAllowsQuantityDiscounts)
							{
								ActiveDIDID = 0;
								ActiveDID = false;
							}
				
							String ProductName = DB.RowField(row,"Name") + Common.IIF(DB.RowField(row,"VariantName").Length != 0, " - " + DB.RowField(row,"VariantName"), "");
							String ProductDescription = DB.RowField(row,"Description"); //.Replace("\n","<br>");
							if(Common.AppConfigBool("ShowSummaryInTableOrderFormat"))
							{
								ProductDescription = DB.RowField(row,"Summary");
							}
							String FileDescription = new ProductDescriptionFile(ProductID,thisCustomer._localeSetting,_siteID)._contents;
							if(FileDescription.Length != 0)
							{
								ProductDescription += "<br>" + FileDescription;
							}
							String ProductPicture = Common.LookupImage("Variant",DB.RowFieldInt(row,"VariantID"),"icon",_siteID);
							if(ProductPicture.IndexOf("nopicture") != -1)
							{
								ProductPicture = Common.LookupImage("Product",ProductID,"icon",_siteID);
							}
							String LargePic = Common.LookupImage("Product",ProductID,"large",_siteID);
							bool HasLargePic = (LargePic.Length != 0);

							String LargeImage = String.Empty;
							if(HasLargePic)
							{
								LargeImage = LargePic;
								int LargePicW = Common.GetImageWidth(LargeImage);
								int LargePicH = Common.GetImageHeight(LargeImage);
								writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
								writer.Write("function popup" + ProductID.ToString() + "_" + DB.RowFieldInt(row,"VariantID").ToString() + "(url)\n");
								writer.Write("	{\n");
								writer.Write("	window.open('popup.aspx?src=' + url,'LargerImage" + ProductID.ToString() + Common.GetRandomNumber(1,100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no,copyhistory=no,width=" + LargePicW.ToString() + ",height=" + LargePicH.ToString() + ",left=0,top=0');\n");
								writer.Write("	return (true);\n");
								writer.Write("	}\n");
								writer.Write("</script>\n");
							}

							writer.Write("<tr>");
							writer.Write("<td align=\"center\" valign=\"top\" >");
							if(ProductPicture.Length != 0)
							{
								writer.Write("<img " + Common.IIF(HasLargePic,"style=\"cursor: hand;\" alt=\"View Larger Image\" onClick=\"javascript:popup" + ProductID.ToString() + "_" + DB.RowFieldInt(row,"VariantID").ToString() + "('" + Server.UrlEncode(LargePic) + "');\"","") + " src=\"" + ProductPicture + "?" + Common.GetRandomNumber(1,100000).ToString() + "\">");
								writer.Write("<br>");
								writer.Write("");
								if(HasLargePic) 
								{
									writer.Write("<a href=\"javascript:void(0);\" onClick=\"javascript:popup" + ProductID.ToString() + "_" + DB.RowFieldInt(row,"VariantID").ToString() + "('" + Server.UrlEncode(LargePic) + "');\">View Larger Image</a><br>");
								}
							}

							writer.Write("</td>");
							writer.Write("<td align=\"left\" valign=\"top\" >");

							writer.Write("<span class=\"ProductNameText\"><a href=\"" + SE.MakeProductAndCategoryLink(ProductID,CategoryID,String.Empty) + "\">" + ProductName + "</a></span><br>\n");
							
							writer.Write("<div align=\"left\">");
							writer.Write(ProductDescription);
							writer.Write("</div>");
							writer.Write("<br>");
							
							writer.Write("SKU: " + Common.MakeProperProductSKU(DB.RowField(row,"SKU"),DB.RowField(row,"SKUSuffix"),"","") + "<br>");
							if(DB.RowField(row,"ProductMiscText").ToUpper() == "SHOW INVENTORY")
							{
								writer.Write("Stock On Hand: " + DB.RowFieldInt(row,"Inventory").ToString() + "<br>");
							}
							else
							{
								writer.Write("In Stock: " + Common.IIF(DB.RowFieldInt(row,"Inventory") > 0, "Yes", "No") + "<br>");
							}
				
							if(!DB.RowFieldBool(row,"HidePriceUntilCart"))
							{
								if(thisCustomer._customerLevelID == 0)
								{
									// show consumer (e.g. level 0) pricing:
									if(DB.RowFieldDecimal(row,"SalePrice") != System.Decimal.Zero)
									{
										writer.Write("<strike>Price: " + Localization.CurrencyStringForDisplay( DB.RowFieldSingle(row,"Price")) + "</strike><br>");
										writer.Write("<font color=\"" + Common.AppConfig("OnSaleForTextColor") + "\">" + DB.RowField(row,"sDescription") + ": " + Localization.CurrencyStringForDisplay( DB.RowFieldSingle(row,"SalePrice")) + "</font><br>");
									}
									else
									{
										writer.Write("Price: " + Localization.CurrencyStringForDisplay( DB.RowFieldSingle(row,"Price")) + "<br>");
									}
								}
								else
								{
									// show level pricing:
									bool IsOnSale = false;
									decimal LevelPrice = Common.DetermineLevelPrice(DB.RowFieldInt(row,"VariantID"),thisCustomer._customerLevelID, out IsOnSale);
									writer.Write("Regular Price: " + Localization.CurrencyStringForDisplay( DB.RowFieldSingle(row,"Price")) + "<br>");
									writer.Write("<font color=\"" + Common.AppConfig("OnSaleForTextColor") + "\">" + thisCustomer._customerLevelName + " Price: " + Localization.CurrencyStringForDisplay( LevelPrice) + "<br></font>");
								}
							}

							String FldName = DB.RowFieldInt(row,"ProductID").ToString() + "_" + DB.RowFieldInt(row,"VariantID").ToString();
							writer.Write("<br>");
							writer.Write(Common.GetAddToCartForm(false,false,DB.RowFieldInt(row,"ProductID"),DB.RowFieldInt(row,"VariantID"),_siteID,DB.RowFieldInt(row1,"CategoryDisplayFormatID"),false));
							//writer.Write("Quantity: <input name=\"Qty_" + FldName + "\" type=\"text\" size=\"3\" maxlength=\"3\">");

							writer.Write("</td>");
							writer.Write("</tr>");
							writer.Write("<tr><td colspan=2 height=15><hr size=1></td></tr>");
						}
						ds2.Dispose();
						writer.Write("</table>");

						writer.Write("<br><br>");
						if(!anyFound)
						{
							writer.Write("<center><b>No Products Found</b></center>");
						}
						writer.Write("</div>");
						break;
					case 8:
						// TABLE - CONDENSED 2:
						writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
						writer.Write("    <tr class=\"DarkCell\">\n");
						writer.Write("      <td><font class=\"CondensedDarkCellText\"><b>Photo</b></font></td>\n");
						writer.Write("      <td><font class=\"CondensedDarkCellText\"><b>Product</b></font></td>\n");
						writer.Write("      <td><font class=\"CondensedDarkCellText\"><b>Manufacturer</b></font></td>\n");
						writer.Write("      <td><font class=\"CondensedDarkCellText\"><b>Price</b></font></td>\n");
						writer.Write("      <td><font class=\"CondensedDarkCellText\"><b>More Info</b></font></td>\n");
						writer.Write("    </tr>\n");
						int rownum2 = 1;
						foreach(DataRow row in ds.Tables[0].Rows)
						{
							writer.Write("<tr " + Common.IIF(rownum2 % 2 == 0 , "class=\"LightCell\"" , "") + ">\n");

							writer.Write("<td valign=\"middle\" align=\"center\">");

							String Image1URL = Common.LookupImage("Variant",DB.RowFieldInt(row,"VariantID"),"icon",_siteID);
							if(Image1URL.IndexOf("nopicture.") != -1)
							{
								Image1URL = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",_siteID);
							}
							if(Image1URL.Length == 0)
							{
								Image1URL = Common.AppConfig("NoPictureIcon");
							}
							int HT = Common.AppConfigUSInt("CondensedTablePictureHeight");
							if(HT == 0)
							{
								HT = 50;
							}

							writer.Write("<img style=\"cursor: hand;\" alt=\"" + DB.RowField(row,"PSEAltText") + "\" OnClick=\"self.location='" + SE.MakeProductAndCategoryLink(DB.RowFieldInt(row,"ProductID"),CategoryID,DB.RowField(row,"PSEName"))  + "';\" style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + Image1URL + "\" height=\"" + HT.ToString() + "\" align=\"absmiddle\">");
							//writer.Write("</a>");
							writer.Write("&nbsp;\n");
							writer.Write("</td>");
					
							writer.Write("<td>");
							writer.Write("<a href=\"" + SE.MakeProductAndCategoryLink(DB.RowFieldInt(row,"ProductID"),CategoryID,DB.RowField(row,"PSEName")) + "\">");
							writer.Write(Common.MakeProperProductName(DB.RowField(row,"Name"),DB.RowField(row,"VariantName")));
							writer.Write("</a>");
							writer.Write("</td>\n");
					
							writer.Write("<td>" + DB.RowField(row,"ManufacturerName") + "</td>\n");
							writer.Write("<td>");

							if(!DB.RowFieldBool(row,"HidePriceUntilCart"))
							{
								if(thisCustomer._customerLevelID == 0)
								{
									// show consumer (e.g. level 0) pricing:
									if(DB.RowFieldDecimal(row,"SalePrice") != System.Decimal.Zero)
									{
										writer.Write("<strike>Price: " + Localization.CurrencyStringForDisplay( DB.RowFieldSingle(row,"Price")) + "</strike><br>");
										writer.Write("<font color=\"" + Common.AppConfig("OnSaleForTextColor") + "\">" + DB.RowField(row,"sDescription") + ": " + Localization.CurrencyStringForDisplay( DB.RowFieldSingle(row,"SalePrice")) + "</font><br>");
									}
									else
									{
										writer.Write("Price: " + Localization.CurrencyStringForDisplay( DB.RowFieldSingle(row,"Price")) + "<br>");
									}
								}
								else
								{
									// show level pricing:
									bool IsOnSale = false;
									decimal LevelPrice = Common.DetermineLevelPrice(DB.RowFieldInt(row,"VariantID"),thisCustomer._customerLevelID, out IsOnSale);
									writer.Write("Regular Price: " + Localization.CurrencyStringForDisplay( DB.RowFieldSingle(row,"Price")) + "<br>");
									writer.Write("<font color=\"" + Common.AppConfig("OnSaleForTextColor") + "\">" + thisCustomer._customerLevelName + " Price: " + Localization.CurrencyStringForDisplay( LevelPrice) + "<br></font>");
								}
							}
							else
							{
								writer.Write("&nbsp;");
							}

							writer.Write("</td>\n");

							writer.Write("<td>");
							writer.Write("<a href=\"" + SE.MakeProductAndCategoryLink(DB.RowFieldInt(row,"ProductID"),CategoryID,DB.RowField(row,"PSEName"))  + "\">");
							writer.Write("<img style=\"cursor:hand;\" src=\"skins/skin_" + _siteID.ToString() + "/images/moreinfo.gif\" border=\"0\" align=\"absmiddle\">");
							writer.Write("</a>&nbsp;\n");
							writer.Write("</td>\n");
							writer.Write("</tr>\n");
							rownum2++;
						};
						writer.Write("</table>");
						break;
				}
				if(DispFormatID == 1 || DispFormatID == 2 || DispFormatID == 3 || DispFormatID == 7 || DispFormatID == 8)
				{
					if(pagingOn && NumRows >= PageSize && (NumPages > 1 || Common.QueryString("show") == "all"))
					{
						if(Common.AppConfig("PageNumberFormat").ToUpper() == "NEXT_PREV")
						{
							writer.Write("<p class=\"PageNumber\" align=\"left\">Showing items " + StartRow.ToString() + "-" + StopRow.ToString() + " of " + NumRows.ToString());
							if(NumPages > 1)
							{
								writer.Write(" (");
								if(PageNum > 1)
								{
									//writer.Write("<a href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=1\">First Page</a>");
									//writer.Write(" | ");
									writer.Write("<a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + (PageNum-1).ToString() + "\">Previous Page</a>");
								}
								if(PageNum > 1 && PageNum < NumPages)
								{
									writer.Write(" | ");
								}
								if(PageNum < NumPages)
								{
									writer.Write("<a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + (PageNum+1).ToString() + "\">Next Page</a>");
									//writer.Write(" | ");
									//writer.Write("<a href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + NumPages.ToString() + "\">Last Page</a>");
								}
								writer.Write(")");
								writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;Click <a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&show=all\">here</a> to see all items");
							}
						}
						else
						{
							writer.Write("<p class=\"PageNumber\" align=\"left\">");
							if(Common.QueryString("show") == "all")
							{
								writer.Write("Click <a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=1\">here</a> to return to paging");
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
										writer.Write("<a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + u.ToString() + "\">" + u.ToString() + "</a>&nbsp;&nbsp;");
									}
								}
								writer.Write("&nbsp;&nbsp;<a class=\"PageNumber\" href=\"showcategory.aspx?categoryid=" + CategoryID.ToString() + "&show=all\">all</a>");
							}
						}
						writer.Write("</p>\n");
					}
				}
			}
			ds.Dispose();
		}

		void WriteProductBar(System.Web.UI.HtmlTextWriter writer, DataRow row, int ProductID, int CategoryID, int SiteID)
		{
			String url = SE.MakeProductAndCategoryLink(DB.RowFieldInt(row,"ProductID"),CategoryID,DB.RowField(row,"PSEName"));
			writer.Write("			<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" >\n");
			writer.Write("			<tr>\n");
			writer.Write("				<td colspan=\"4\" align=\"left\" valign=\"middle\" height=\"20\" class=\"DarkCell\">\n");
			writer.Write("					&nbsp;<img src=\"skins/skin_" + _siteID.ToString() + "/images/whitearrow.gif\" align=\"absmiddle\">&nbsp;<a class=\"DarkCellText\" href=\"" + url + "\"><font style=\"font-size: 15px; font-weight:bold;\">" + DB.RowField(row,"Name") + " (" + DB.RowField(row,"ManufacturerName") + ")</font></a>\n");
			writer.Write("				</td>\n");
			writer.Write("			</tr>\n");
			writer.Write("			<tr>\n");
			writer.Write("				<td width=\"2%\" class=\"GreyCell\"><img src=\"/images/spacer.gif\" width=\"5\" height=\"1\">\n");
			writer.Write("				</td>\n");
			writer.Write("				<td width=\"30%\" align=\"center\" valign=\"top\" class=\"GreyCell\">\n");
			String ImgUrl = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",_siteID);
			if(ImgUrl.Length == 0)
			{
				ImgUrl = Common.AppConfig("NoPicture");
			}
			if(ImgUrl.Length != 0)
			{
				writer.Write("<br><img style=\"cursor: hand;\" alt=\"" + DB.RowField(row,"PSEAltText") + "\" onClick=\"self.location='" + url + "';\" style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + ImgUrl + "?" + Common.GetRandomNumber(1,100000).ToString() + "\">\n");
			}
			writer.Write("				</td>\n");
			writer.Write("				<td width=\"8%\" class=\"GreyCell\">\n");
			writer.Write("					<img src=\"/images/spacer.gif\" width=\"30\" height=\"1\">\n");
			writer.Write("				</td>\n");
			writer.Write("				<td width=\"60%\" valign=\"top\" align=\"left\" class=\"GreyCell\">\n");
			writer.Write("					<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" class=\"GreyCell\">\n");
			writer.Write("						<tr>\n");
			writer.Write("							<td width=\"20%\" align=\"left\" valign=\"top\">Description:</td>\n");
			writer.Write("							<td width=\"80%\" align=\"left\" valign=\"top\">" + DB.RowField(row,"Description") + "</td>\n");
			writer.Write("						</tr>\n");
			writer.Write("						<tr>\n");
			writer.Write("							<td width=\"20%\" align=\"left\" valign=\"top\">Base SKU:</td>\n");
			writer.Write("							<td width=\"80%\" align=\"left\" valign=\"top\">" + DB.RowField(row,"SKU") + "</td>\n");
			writer.Write("						</tr>\n");
			writer.Write("						<tr>\n");
			writer.Write("							<td colspan=\"2\" width=\"100%\" align=\"right\" valign=\"bottom\"><a href=\"" + url + "\"><img style=\"cursor:hand;\" src=\"skins/skin_" + _siteID.ToString() + "/images/moreinfo.gif\" border=\"0\"></a></td>\n");
			writer.Write("						</tr>\n");
			writer.Write("					</table>\n");
			writer.Write("				</td>\n");
			writer.Write("			</tr>\n");
			writer.Write("			</table>\n");
			//writer.Write("			<br><br>\n");
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
