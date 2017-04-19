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
	/// Summary description for showproduct.
	/// </summary>
	public class showproduct : SkinBase
	{
		int ProductID;
		String CategoryName;
		String SectionName;
		int CategoryID;
		int SectionID;
		bool IsAKit;
		bool IsAPack;
		bool RequiresReg;
		int ProductDisplayFormatID;
		String ProductName;
		String ProductDescription;
		String ProductPicture;
		String LargePic;
		bool HasLargePic;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			ProductID = Common.QueryStringUSInt("ProductID");
			String SEName = Common.QueryString("SEName");
			if(ProductID == 0 && SEName.Length != 0)
			{
				// mapping from static url, try to find product id:
				ProductID = SE.LookupSEProduct(SEName);
				if(ProductID == 0)
				{
					// no match:
					Response.Redirect("default.aspx");
				}
			}
			if(ProductID == 0)
			{
				Response.Redirect("default.aspx");
			}
			if(Common.ProductHasBeenDeleted(ProductID))
			{
				Response.Redirect(SE.MakeDriverLink("ProductNotFound"));
			}

			IDataReader rs = DB.GetRS("select * from product  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString());
			if(!rs.Read())
			{
				rs.Close();
				Response.Redirect("default.aspx");
			}

			IsAKit = DB.RSFieldBool(rs,"IsAKit");
			IsAPack = DB.RSFieldBool(rs,"IsAPack");
			RequiresReg = DB.RSFieldBool(rs,"RequiresRegistration");
			ProductDisplayFormatID = DB.RSFieldInt(rs,"ProductDisplayFormatID");
			ProductName = DB.RSField(rs,"Name");
			ProductDescription = DB.RSField(rs,"Description"); //.Replace("\n","<br>");
			//V4_0
			String FileDescription = new ProductDescriptionFile(ProductID,thisCustomer._localeSetting,_siteID)._contents;
			if(FileDescription.Length != 0)
			{
				ProductDescription += "<div align=\"left\">" + FileDescription + "</div>";
			}
			ProductPicture = Common.LookupImage("Product",ProductID,"medium",_siteID);
			LargePic = Common.LookupImage("Product",ProductID,"large",_siteID);
			HasLargePic = (LargePic.Length != 0);
			if(ProductPicture.Length == 0)
			{
				ProductPicture = Common.LookupImage("Product",ProductID,"icon",_siteID);
			}
			if(ProductPicture.Length == 0)
			{
				ProductPicture = Common.AppConfig("NoPicture");
			}

			if(DB.RSField(rs,"SETitle").Length == 0)
			{
				base._SETitle = Common.AppConfig("StoreName") + " - " + ProductName;
			}
			else
			{
				base._SETitle = DB.RSField(rs,"SETitle");
			}
			if(DB.RSField(rs,"SEDescription").Length == 0)
			{
				base._SEDescription = ProductName;
			}
			else
			{
				base._SEDescription = DB.RSField(rs,"SEDescription");
			}
			if(DB.RSField(rs,"SEKeywords").Length == 0)
			{
				base._SEKeywords = ProductName;
			}
			else
			{
				base._SEKeywords = DB.RSField(rs,"SEKeywords");
			}
			base._SENoScript = DB.RSField(rs,"SENoScript");

			rs.Close();
			if(IsAPack)
			{
				Response.Redirect("dyop.aspx?packid=" + ProductID.ToString());
			}

			CategoryID = Common.QueryStringUSInt("CategoryID");
			SectionID = Common.QueryStringUSInt("SectionID");
			if(CategoryID == 0 && SectionID == 0)
			{
				// no category or section passed in, pick first one that this product is mapped to:
				String tmpS = Common.GetProductCategories(ProductID,false);
				if(tmpS.Length != 0)
				{
					String[] catIDs = tmpS.Split(',');
					CategoryID = Localization.ParseUSInt(catIDs[0]);
				}
			}
			CategoryName = Common.GetCategoryName(CategoryID);
			SectionName = Common.GetSectionName(SectionID);

			SectionTitle = "<span class=\"SectionTitleText\"><img src=\"skins/skin_" + _siteID.ToString() + "/images/downarrow.gif\" align=\"absmiddle\" border=\"0\"> ";
			if(CategoryID != 0)
			{
				int pid = Common.GetParentCategory(CategoryID);
				while(pid != 0)
				{
					SectionTitle = "<a class=\"SectionTitleText\" href=\"" + SE.MakeCategoryLink(pid,"") + "\">" + Common.GetCategoryName(pid) + "</a> - " + SectionTitle;
					pid = Common.GetParentCategory(pid);
				}
				SectionTitle += "<a class=\"SectionTitleText\" href=\"" + SE.MakeCategoryLink(CategoryID,"") + "\">" + CategoryName + "</a> - ";
			}
			else
			{
				int pid = Common.GetParentSection(SectionID);
				while(pid != 0)
				{
					SectionTitle += "<a class=\"SectionTitleText\" href=\"" + SE.MakeSectionLink(pid,"") + "\">" + Common.GetSectionName(pid) + "</a> - ";
					pid = Common.GetParentSection(pid);
				}
				SectionTitle += "<a class=\"SectionTitleText\" href=\"" + SE.MakeSectionLink(SectionID,"") + "\">" + SectionName + "</a> - ";
			}
			SectionTitle += ProductName;
			SectionTitle += "</span>";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			Common.LogEvent(thisCustomer._customerID,10, ProductID.ToString());
			if(Common.Form("IsKitSubmit") == "true")
			{
				thisCustomer.RequireCustomerRecord();
				Common.ProcessKitForm(thisCustomer,ProductID);
			}
			if(RequiresReg && thisCustomer._isAnon)
			{
				writer.Write("<br><br><br><br><b>You must be a registered user to view this product!</b><br><br><br><a href=\"signin.aspx?returnurl=showproduct.aspx?" + Server.HtmlEncode(Server.UrlEncode(Common.ServerVariables("QUERY_STRING"))) + "\">Click Here</a> to sign-in.");
			}
			else
			{

				DB.ExecuteSQL("update product set Looks=Looks+1 where ProductID=" + ProductID.ToString());
				String sql = String.Empty;
				if(IsAKit)
				{
					sql = "SELECT TOP 100 PERCENT Product.*, KitGroup.KitGroupID AS KitGroupID, KitGroup.Name AS GroupName, KitGroup.DisplayOrder AS GroupDisplayOrder, KitGroup.KitGroupTypeID AS KitGroupTypeID, KitGroup.Description as GroupDescription, KitGroup.IsRequired AS GroupIsRequired, ProductVariant.Price, ProductVariant.Points, Product.HidePriceUntilCart, ProductVariant.SalePrice FROM (Product  " + DB.GetNoLock() + " INNER JOIN KitGroup  " + DB.GetNoLock() + " ON Product.ProductID = KitGroup.ProductID) inner join productvariant  " + DB.GetNoLock() + " on product.productid=productvariant.productid where Product.Deleted=0 and Product.Published<>0 and ProductVariant.deleted=0 and ProductVariant.Published<>0 and Product.ProductID=" + ProductID.ToString() + " ORDER BY KitGroup.DisplayOrder, KitGroup.Name";
				}
				else
				{
					sql = "SELECT SalesPrompt.name as SDescription, Product.MiscText, Product.SwatchImageMap, Product.ProductID, Product.IsCallToOrder, Product.ProductTypeID, Product.RelatedProducts, Product.UpsellProducts, Product.UpsellProductDiscountPercentage, Product.HidePriceUntilCart, Product.Name, Product.SEName, Product.SpecTitle, Product.SpecsInline, Product.SpecCall, Product.ProductDisplayFormatID, Product.ColWidth, Product.Summary, Product.Description, Product.SEKeywords, Product.SEDescription, Product.SKU, Product.ManufacturerID, Product.ManufacturerPartNumber, Product.Published, Product.Deleted, ProductVariant.VariantID, ProductVariant.Name AS VariantName, ProductVariant.Colors, ProductVariant.Sizes, ProductVariant.Dimensions, ProductVariant.Weight, ProductVariant.Inventory, ProductVariant.SKUSuffix, ProductVariant.ManufacturerPartNumber AS VManufacturerPartNumber, ProductVariant.Price, ProductVariant.Points, ProductVariant.SalePrice, ProductVariant.Deleted AS VariantDeleted, ProductVariant.Description as VariantDescription, ProductVariant.Published AS VariantPublished, Manufacturer.Name AS ManufacturerName, Manufacturer.SEName as ManufacturerSEName FROM  ((Product  " + DB.GetNoLock() + " INNER JOIN Manufacturer  " + DB.GetNoLock() + " ON Product.ManufacturerID = Manufacturer.ManufacturerID) left outer join salesprompt  " + DB.GetNoLock() + " on product.salespromptid=salesprompt.salespromptid) LEFT OUTER JOIN ProductVariant  " + DB.GetNoLock() + " ON Product.ProductID = ProductVariant.ProductID WHERE product.published=1 and Product.Deleted=0 and product.productid=" + ProductID.ToString() + " AND (ProductVariant.Published=1 or ProductVariant.Published IS NULL) AND (ProductVariant.Deleted=0 or ProductVariant.Deleted IS NULL) ORDER by ProductVariant.DisplayOrder, ProductVariant.Name";
				}
				IDataReader rs = DB.GetRS(sql);
				bool empty = true;
				String RelatedProducts = String.Empty;
				String UpsellProducts = String.Empty;
				if(rs.Read())
				{
					empty = false;
					RelatedProducts = DB.RSField(rs,"RelatedProducts".Trim());
					UpsellProducts = DB.RSField(rs,"UpsellProducts".Trim());
				}

				// setup multi-image gallery:
				String SwatchPic = Common.LookupImage("Product",ProductID,"swatch",_siteID);
				String SwatchImageMap = String.Empty;
				if(!empty)
				{
					SwatchImageMap = DB.RSField(rs,"SwatchImageMap");
				}

				ProductImageGallery ImgGal = null;
				String ImgGalCacheName = "ImgGal_" + ProductID.ToString() + "_" + _siteID.ToString() + "_" + thisCustomer._localeSetting;
				if(Common.AppConfigBool("CacheMenus"))
				{
					ImgGal = (ProductImageGallery)HttpContext.Current.Cache.Get(ImgGalCacheName);
				}
				if(ImgGal == null)
				{
					ImgGal = new ProductImageGallery(ProductID,_siteID,thisCustomer._localeSetting);
				}
				if(Common.AppConfigBool("CacheMenus"))
				{
					HttpContext.Current.Cache.Insert(ImgGalCacheName,ImgGal,null,System.DateTime.Now.AddMinutes(60),TimeSpan.Zero);
				}
				writer.Write(ImgGal._imgDHTML);

				if(!empty)
				{

					String LargeImage = String.Empty;
					if(HasLargePic)
					{
						LargeImage = LargePic;
						int LargePicW = Common.GetImageWidth(LargeImage);
						int LargePicH = Common.GetImageHeight(LargeImage);
						writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
						writer.Write("function popupimg(url)\n");
						writer.Write("	{\n");
						writer.Write("	window.open('popup.aspx?src=' + url,'LargerImage" + ProductID.ToString() + Common.GetRandomNumber(1,100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no,copyhistory=no,width=" + LargePicW.ToString() + ",height=" + LargePicH.ToString() + ",left=0,top=0');\n");
						writer.Write("	return (true);\n");
						writer.Write("	}\n");
						writer.Write("</script>\n");
					}

					ProductSpecFile pspec = new ProductSpecFile(ProductID,thisCustomer._localeSetting,_siteID);
					bool ProductHasSpecs = (pspec._contents.Length != 0);
					bool SpecsInline = DB.RSFieldBool(rs,"SpecsInline");
					String SpecTitle = DB.RSField(rs,"SpecTitle");
					String SpecLink = Common.IIF(SpecsInline , "#Specs" , pspec._url);
					if(SpecTitle.Length == 0)
					{
						SpecTitle = Common.AppConfig("DefaultSpecTitle");
					}

					int CustomerLevelID = thisCustomer._customerLevelID;
					bool CustomerLevelAllowsQuantityDiscounts = Common.CustomerLevelAllowsQuantityDiscounts(CustomerLevelID);
					String CustomerLevelName = Common.GetCustomerLevelName(CustomerLevelID);

					String ProdPic = String.Empty;
					String MainProductSKU = String.Empty;
					int ActiveDIDID = Common.LookupActiveProductQuantityDiscountID(ProductID);
					bool ActiveDID = (ActiveDIDID != 0);
					if(!CustomerLevelAllowsQuantityDiscounts)
					{
						ActiveDIDID = 0;
						ActiveDID = false;
					}
					if(ActiveDID)
					{
						writer.Write(Common.ReadFile("tip2.js",false));
					}

					int NumProducts = 0;
					if(IsAKit)
					{
						// DISPLAY KIT:
						thisCustomer.RequireCustomerRecord();
						writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"4\" width=\"100%\">");
						writer.Write("<tr>");
						writer.Write("<td align=\"center\" valign=\"top\" width=\"40%\">");
						ProdPic = Common.LookupImage("Product",ProductID,"medium",_siteID);
						if(ProdPic.Length == 0)
						{
							ProdPic = Common.LookupImage("Product",ProductID,"icon",_siteID);
						}
						if(ProdPic.Length == 0)
						{
							ProdPic = Common.AppConfig("NoPicture");
						}
						if(HasLargePic) 
						{
							writer.Write("<img id=\"ProductPic" + ProductID.ToString() + "\" name=\"ProductPic" + ProductID.ToString() + "\" style=\"cursor:hand;\" onClick=\"popupimg('" + LargePic + "')\" alt=\"Click here to view larger image\" style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + ProdPic + "\">");
						}
						else
						{
							writer.Write("<img id=\"ProductPic" + ProductID.ToString() + "\" name=\"ProductPic" + ProductID.ToString() + "\" style=\"" + Common.AppConfig("ImageFrameStyle").Replace("cursor: hand;","") + "\" src=\"" + ProdPic + "\">");
						}
						if(ImgGal._imgGalIcons.Length != 0)
						{
							writer.Write("<br><br>");
							writer.Write(ImgGal._imgGalIcons);
							if(SwatchPic.Length != 0)
							{
								writer.Write(SwatchImageMap);
								writer.Write("<br><img src=\"images/spacer.gif\" width=\"1\" height=\"4\"><br><img style=\"cursor: hand;\" src=\"" + SwatchPic + "\" usemap=\"#SwatchMap\" border=\"0\">");
							}
						}
						if(HasLargePic)
						{
							writer.Write("<br><img src=\"images/spacer.gif\" width=\"1\" height=\"4\"><br>");
							writer.Write("<div align=\"center\"><a href=\"javascript:void(0);\" onClick=\"javascript:popupimg('" + LargePic + "');\"><img src=\"skins/skin_" + _siteID.ToString() + "/images/showlarger.gif\" align=\"absmiddle\" border=\"0\" alt=\"Show Larger Picture\"></a></div><br>");
						}
						//writer.Write("<br>SKU Shown: " + Common.MakeProperProductSKU(DB.RSField(rs,"SKU"),DB.RSField(rs,"SKUSuffix"),"",""));
						writer.Write("</td>");
						writer.Write("<td align=\"left\" valign=\"top\">");

						writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");

						if(!Common.AppConfigBool("HideProductNextPrevLinks"))
						{
							writer.Write("<tr><td align=\"right\">");
							NumProducts = 0;
							if(CategoryID != 0)
							{
								NumProducts = Common.GetNumCategoryProducts(CategoryID,true,true);
							}
							else
							{
								NumProducts = Common.GetNumSectionProducts(SectionID,true,true);
							}
							if(NumProducts > 1)
							{
								int PreviousProductID = Common.GetPreviousProduct(ProductID,CategoryID,SectionID,0,0,false,true,true);
								if(CategoryID != 0)
								{
									writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndCategoryLink(PreviousProductID,CategoryID,"") + "\">previous</a>&nbsp;|&nbsp;");
								}
								else
								{
									writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndSectionLink(PreviousProductID,SectionID,"") + "\">previous</a>&nbsp;|&nbsp;");
								}
							}
							if(CategoryID != 0)
							{
								writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeCategoryLink(CategoryID,"") + "\">up</a>");
							}
							else
							{
								writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeSectionLink(SectionID,"") + "\">up</a>");
							}
							if(NumProducts > 1)
							{
								int NextProductID = Common.GetNextProduct(ProductID,CategoryID,SectionID,0,0,false,true,true);
								if(CategoryID != 0)
								{
									writer.Write("&nbsp;|&nbsp;<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndCategoryLink(NextProductID,CategoryID,"") + "\">next</a><br>&nbsp;");
								}
								else
								{
									writer.Write("&nbsp;|&nbsp;<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndSectionLink(NextProductID,SectionID,"") + "\">next</a><br>&nbsp;");
								}
							}
							writer.Write("</td></tr>\n");
						}
						writer.Write("<tr valign=\"top\"><td>");
						writer.Write("<font class=\"ProductNameText\">");
						writer.Write(DB.RSField(rs,"Name"));
						writer.Write("</font>&nbsp;&nbsp;");
						if(ProductHasSpecs)
						{
							writer.Write("&nbsp;&nbsp;<small>(<a href=\"" + SpecLink + "\" " + Common.IIF(SpecsInline , "" , "target=\"_blank\"") + ">" + SpecTitle + "</a>)</small>");
						}
						writer.Write("<br>");
						if(ActiveDID)
						{
							String S1 = String.Empty;
							writer.Write("<br><small>This product qualifies for quantity discount pricing. (<a href=\"javascript:void(0);\" style=\"cursor: normal;\" onMouseover=\"ddrivetip(" + Common.SQuote(Common.AppConfig("quantitydiscount") + "<br><br>" + Common.GetQuantityDiscountDisplayTable(ActiveDIDID)) + ",'" + Common.AppConfig("LightCellColor") + "', 300)\" onMouseout=\"hideddrivetip()\">what's this?</a>)</small>");
						}
						if(Common.AppConfigBool("ShowEMailProductToFriend"))
						{
							String S1 = String.Empty;
							writer.Write("<br><small><img src=\"skins/skin_" + _siteID.ToString() + "/images/mailicon.gif\" border=\"0\" align=\"absmiddle\">&nbsp;<a href=\"emailproduct.aspx?productid=" + ProductID.ToString() + "&categoryid=" + CategoryID.ToString() + "\">E-mail this product to a friend</a></small><br><br>");
						}
						writer.Write("<div align=\"left\">");
						writer.Write(ProductDescription);
						writer.Write("</div>");

						if(Common.AppConfigBool("ShowInventoryTable"))
						{
							writer.Write("<div align=\"left\"><br>");
							writer.Write(Common.GetInventoryTable(ProductID,Common.GetProductsFirstVariantID(ProductID),thisCustomer._isAdminUser));
							writer.Write("</div>");
						}

						writer.Write("</td></tr>");
						writer.Write("<tr valign=\"top\"><td height=\"10\"></td></tr>");
						writer.Write("</table>");
						writer.Write("</td></tr></table>");
						writer.Write("<hr size=\"1\" color=\"#666666\">");

						writer.Write("<br clear=all>");
						writer.Write(Common.GetJSPopupRoutines());
						writer.Write("<div align=\"left\" width=\"100%\">\n");
						decimal BasePrice = System.Decimal.Zero;
						if(DB.RSFieldDecimal(rs,"SalePrice") != System.Decimal.Zero)
						{
							BasePrice = DB.RSFieldDecimal(rs,"SalePrice");
						}
						else
						{
							BasePrice = DB.RSFieldDecimal(rs,"Price");
						}

						writer.Write("<div align=\"left\">");
						bool KitIsComplete = Common.KitContainsAllRequiredItems(thisCustomer._customerID,ProductID,0);
						if(!DB.RSFieldBool(rs,"HidePriceUntilCart"))
						{
							int Points = DB.RSFieldInt(rs,"Points");
							writer.Write("<b>Base Price: " + Localization.CurrencyStringForDisplay(BasePrice) + "</b> " + Common.IIF(thisCustomer._customerLevelID != 0 && Common.AppConfigBool("MicroPay.ShowPointsWithPrices"), "(" + Points.ToString() + " Points)", "") + "<br>");
							decimal KitPriceDelta = Common.KitPriceDelta(thisCustomer._customerID,ProductID,0);
							if(CustomerLevelID == 0)
							{
								writer.Write("<b>Customized Price: " + Localization.CurrencyStringForDisplay(BasePrice+KitPriceDelta) + "</b><br>");
							}
							else
							{
								decimal PR = BasePrice + KitPriceDelta;
								Single LevelDiscountPercent = (Single)Common.GetCustomerLevelDiscountPercent(CustomerLevelID); //0.0F;
								if(LevelDiscountPercent != 0.0F)
								{
									PR = PR * (decimal)(1.00F - (LevelDiscountPercent/100.0F));
								}
								writer.Write("<b><strike>Customized Price: " + Localization.CurrencyStringForDisplay(BasePrice+KitPriceDelta) + "</strike></b><br>");
								writer.Write("<b>" + CustomerLevelName + " Customized Price: " + Localization.CurrencyStringForDisplay(PR) + "</b><br>");
							}
						}

						if(KitIsComplete)
						{
							writer.Write("<p><b>Your Kit Is Ready To Purchase!</b>");
							writer.Write(Common.GetAddToCartForm(false,Common.AppConfigBool("ShowWishButtons"),ProductID,Common.GetFirstProductVariant(ProductID),_siteID,1,!ImgGal.IsEmpty()));
							writer.Write("</p>");
						}
						else
						{
							writer.Write("<p><b>Your kit selections are not yet complete, please select additional required items below...</b></p>");
						}
						writer.Write("<p></p>"); // spacer
						writer.Write("</div>");

						writer.Write("<form method=\"post\" action=\"" + SE.MakeProductLink(ProductID,"") + "\">");
						writer.Write("<input type=\"hidden\" name=\"IsKitSubmit\" value=\"true\">");
						writer.Write("<table width=\"100%\" cellpadding=\"6\" cellspacing=\"0\" border=\"0\">");
						int groupnumber = 0;
						bool HidePrice = false;
						do
						{
							int ThisGroupID = DB.RSFieldInt(rs,"KitGroupID");
							groupnumber++;
							String bgcolor = Common.IIF(groupnumber % 2 == 0, "bgcolor=\"#FFFFFF\"", "class=\"LightCell\"");
							writer.Write("<tr " + bgcolor + " align=\"left\">");
							writer.Write("<td align=\"left\">");
							writer.Write("<b>" + Common.IIF(DB.RSFieldBool(rs,"GroupIsRequired"),"*","") + DB.RSField(rs,"GroupName") + "</b>&nbsp;&nbsp;");
							if(DB.RSField(rs,"GroupDescription").Length != 0)
							{
								writer.Write("<a href=\"javascript:void(0);\" onClick=\"popupkitgroupwh('Kit%20Information','" + DB.RSFieldInt(rs,"KitGroupID").ToString() + "',300,400,'yes')\" style=\"cursor:hand;\"><img alt=\"Click here for more information about this option...\" src=\"skins/skin_" + _siteID.ToString() + "/images/helpcircle.gif\" border=\"0\" align=\"absmiddle\"></a>");
							}
							writer.Write("</td>");
							writer.Write("<td width=10></td>");
							writer.Write("<td align=\"right\">");
							writer.Write("</td>");
							writer.Write("</tr>");
							// show the group items:
							IDataReader rsi = DB.GetRS("select * from KitItem  " + DB.GetNoLock() + " where KitGroupID=" + ThisGroupID.ToString() + " order by displayorder,name");
							bool itemsfound = false;
							bool anyDescs = false;
							bool CustomerHasSelectedAnyItemsInThisGroup = Common.KitContainsAnyGroupItems(thisCustomer._customerID,ProductID,0,ThisGroupID);
							int ix = 1;
							switch(DB.RSFieldInt(rs,"KitGroupTypeID"))
							{
								case 1: // Single Select Dropdown  List
									writer.Write("<tr " + bgcolor + " align=\"left\">");
									writer.Write("<td valign=\"top\" align=\"left\">");
									while(rsi.Read())
									{
										if(ix == 1)
										{
											HidePrice = DB.RSFieldBool(rs,"HidePriceUntilCart");
											writer.Write("<select size=\"1\" name=\"KitGroupID_" + ThisGroupID.ToString() + "\">");
										}
										itemsfound = true;
										String IName = Server.HtmlEncode(DB.RSField(rsi,"Name"));
										if(!DB.RSFieldBool(rs,"HidePriceUntilCart"))
										{
											decimal PR = DB.RSFieldDecimal(rsi,"PriceDelta");
											if(PR < System.Decimal.Zero)
											{
												IName += " - Subtract " + Localization.CurrencyStringForDisplay(-PR);
											}
											else if(PR > System.Decimal.Zero)
											{
												IName += " - Add " + Localization.CurrencyStringForDisplay(PR);
											}
										}
										String IsSelected = String.Empty;
										if(!CustomerHasSelectedAnyItemsInThisGroup)
										{
											if(DB.RSFieldBool(rsi,"IsDefault"))
											{
												IsSelected = " selected ";
											}
										}
										else
										{
											if(Common.KitContainsItem(thisCustomer._customerID,ProductID,0,DB.RSFieldInt(rsi,"KitItemID")))
											{
												IsSelected = " selected ";
											}
										}
										writer.Write("<option value=\"" + DB.RSFieldInt(rsi,"KitItemID").ToString() + "\" " + IsSelected + ">" + IName + "</option>");
										if(DB.RSField(rsi,"Description").Length != 0)
										{
											anyDescs = true;
										}
										ix++;
									}
									if(ix > 1)
									{
										writer.Write("</select>");
									}
									writer.Write("</td>");
									writer.Write("<td width=10></td>");
									writer.Write("<td valign=\"top\" align=\"right\">");
									if(DB.RSField(rs,"GroupDescription").Length != 0 && anyDescs)
									{
										writer.Write("<a href=\"javascript:void(0);\" style=\"cursor:hand;\" onClick=\"popupkitgroupwh('Kit%20Information','" + ThisGroupID.ToString() + "',300,400,'yes')\"><img alt=\"Click here for more information about this option...\" src=\"skins/skin_" + _siteID.ToString() + "/images/moreinfo.gif\" border=\"0\" align=\"absmiddle\"></a>");
									}
									writer.Write("</td>");
									writer.Write("</tr>");
									break;
								case 2: // Single Select Radio List
									while(rsi.Read())
									{
										HidePrice = DB.RSFieldBool(rs,"HidePriceUntilCart");
										writer.Write("<tr " + bgcolor + " align=\"left\">");
										writer.Write("<td valign=\"top\" align=\"left\">");
										itemsfound = true;
										String IName = Server.HtmlEncode(DB.RSField(rsi,"Name"));
										if(!DB.RSFieldBool(rs,"HidePriceUntilCart"))
										{
											decimal PR = DB.RSFieldDecimal(rsi,"PriceDelta");
											if(PR < System.Decimal.Zero)
											{
												IName += " - Subtract " + Localization.CurrencyStringForDisplay(-PR);
											}
											else if(PR > System.Decimal.Zero)
											{
												IName += " - Add " + Localization.CurrencyStringForDisplay(PR);
											}
										}
										String IsSelected = String.Empty;
										if(!CustomerHasSelectedAnyItemsInThisGroup)
										{
											if(DB.RSFieldBool(rsi,"IsDefault"))
											{
												IsSelected = " checked ";
											}
										}
										else
										{
											if(Common.KitContainsItem(thisCustomer._customerID,ProductID,0,DB.RSFieldInt(rsi,"KitItemID")))
											{
												IsSelected = " checked ";
											}
										}
										writer.Write("<input type=\"radio\" name=\"KitGroupID_" + ThisGroupID.ToString() + "\" value=\"" + DB.RSFieldInt(rsi,"KitItemID").ToString() + "\" " + IsSelected + ">" + IName);
										writer.Write("</td>");
										writer.Write("<td width=10></td>");
										writer.Write("<td align=\"right\">");
										if(DB.RSField(rsi,"Description").Length != 0)
										{
											writer.Write("<a href=\"javascript:void(0);\" style=\"cursor:hand;\" onClick=\"popupkititemwh('Kit%20Information','" + DB.RSFieldInt(rsi,"KitItemID").ToString() + "',300,400,'yes')\"><img alt=\"Click here for more information about this option...\" src=\"skins/skin_" + _siteID.ToString() + "/images/moreinfo.gif\" border=\"0\" align=\"absmiddle\"></a>");
										}
									}
									writer.Write("</td>");
									writer.Write("</tr>");
									break;
								case 3: // Multi Select Checkbox
									while(rsi.Read())
									{
										HidePrice = DB.RSFieldBool(rs,"HidePriceUntilCart");
										writer.Write("<tr " + bgcolor + " align=\"left\">");
										writer.Write("<td valign=\"top\" align=\"left\">");
										itemsfound = true;
										String IName = Server.HtmlEncode(DB.RSField(rsi,"Name"));
										if(!DB.RSFieldBool(rs,"HidePriceUntilCart"))
										{
											decimal PR = DB.RSFieldDecimal(rsi,"PriceDelta");
											if(PR < System.Decimal.Zero)
											{
												IName += " - Subtract " + Localization.CurrencyStringForDisplay(-PR);
											}
											else if(PR > System.Decimal.Zero)
											{
												IName += " - Add " + Localization.CurrencyStringForDisplay(PR);
											}
										}
										String IsSelected = String.Empty;
										if(!CustomerHasSelectedAnyItemsInThisGroup)
										{
											//if(DB.RSFieldBool(rsi,"IsDefault"))
											//{
											//	IsSelected = " checked ";
											//}
										}
										else
										{
											if(Common.KitContainsItem(thisCustomer._customerID,ProductID,0,DB.RSFieldInt(rsi,"KitItemID")))
											{
												IsSelected = " checked ";
											}
										}
										writer.Write("<input type=\"checkbox\" name=\"KitGroupID_" + ThisGroupID.ToString() + "\" value=\"" + DB.RSFieldInt(rsi,"KitItemID").ToString() + "\" " + IsSelected + ">" + IName);
										writer.Write("</td>");
										writer.Write("<td width=10></td>");
										writer.Write("<td align=\"right\">");
										if(DB.RSField(rsi,"Description").Length != 0)
										{
											writer.Write("<a href=\"javascript:void(0);\" style=\"cursor:hand;\" onClick=\"popupkititemwh('Kit%20Information','" + DB.RSFieldInt(rsi,"KitItemID").ToString() + "',300,400,'yes')\"><img alt=\"Click here for more information about this option...\" src=\"skins/skin_" + _siteID.ToString() + "/images/moreinfo.gif\" border=\"0\" align=\"absmiddle\"></a>");
										}
									}
									writer.Write("</td>");
									writer.Write("</tr>");
									break;
							}
							rsi.Close();
							if(!itemsfound)
							{
								writer.Write("<tr " + bgcolor + " align=\"left\">");
								writer.Write("<td align=\"left\">");
								writer.Write("(no " + DB.RSField(rs,"GroupName").ToLower() + " items have been configured yet...)");
								writer.Write("</td>");
								writer.Write("<td width=10></td>");
								writer.Write("<td align=\"right\">");
								writer.Write("</td>");
								writer.Write("</tr>");}
						} while(rs.Read());
						writer.Write("</table>");
						if(!HidePrice)
						{
							writer.Write("<input type=\"submit\" value=\"Update Kit Price\">");
						}
						else
						{
							writer.Write("<input type=\"submit\" value=\"Update Kit\">");
						}
						writer.Write("</form>");
						writer.Write("</div>\n");
					}
					else
					{
						switch(ProductDisplayFormatID)
						{
							case 1:
								// RIGHT BAR FORMAT:
								writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"4\" width=\"100%\">");
								writer.Write("<tr>");
								writer.Write("<td align=\"center\" valign=\"top\" width=\"40%\">");
								ProdPic = Common.LookupImage("Product",ProductID,"medium",_siteID);
								if(ProdPic.Length == 0)
								{
									ProdPic = Common.LookupImage("Product",ProductID,"icon",_siteID);
								}
								if(ProdPic.Length == 0)
								{
									ProdPic = Common.AppConfig("NoPicture");
								}
								if(HasLargePic) 
								{
									writer.Write("<img id=\"ProductPic" + ProductID.ToString() + "\" name=\"ProductPic" + ProductID.ToString() + "\" style=\"cursor:hand;\" onClick=\"popupimg('" + LargePic + "')\" alt=\"Click here to view larger image\" style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + ProdPic + "\">");
								}
								else
								{
									writer.Write("<img id=\"ProductPic" + ProductID.ToString() + "\" name=\"ProductPic" + ProductID.ToString() + "\" style=\"" + Common.AppConfig("ImageFrameStyle").Replace("cursor: hand;","") + "\" src=\"" + ProdPic + "\">");
								}
								if(ImgGal._imgGalIcons.Length != 0)
								{
									writer.Write("<br><br>");
									writer.Write(ImgGal._imgGalIcons);
									if(SwatchPic.Length != 0)
									{
										writer.Write(SwatchImageMap);
										writer.Write("<br><img src=\"images/spacer.gif\" width=\"1\" height=\"4\"><br><img style=\"cursor: hand;\" src=\"" + SwatchPic + "\" usemap=\"#SwatchMap\" border=\"0\">");
									}
								}
								if(HasLargePic)
								{
									writer.Write("<br><img src=\"images/spacer.gif\" width=\"1\" height=\"4\"><br>");
									writer.Write("<div align=\"center\"><a href=\"javascript:void(0);\" onClick=\"javascript:popupimg('" + LargePic + "');\"><img src=\"skins/skin_" + _siteID.ToString() + "/images/showlarger.gif\" align=\"absmiddle\" border=\"0\" alt=\"Show Larger Picture\"></a></div><br>");
								}
								//writer.Write("<br>SKU Shown: " + Common.MakeProperProductSKU(DB.RSField(rs,"SKU"),DB.RSField(rs,"SKUSuffix"),"",""));
								writer.Write("</td>");
								writer.Write("<td align=\"left\" valign=\"top\">");

								writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
								if(!Common.AppConfigBool("HideProductNextPrevLinks"))
								{
									writer.Write("<tr><td align=\"right\">");
									NumProducts = 0;
									if(CategoryID != 0)
									{
										NumProducts = Common.GetNumCategoryProducts(CategoryID,true,true);
									}
									else
									{
										NumProducts = Common.GetNumSectionProducts(SectionID,true,true);
									}
									if(NumProducts > 1)
									{
										int PreviousProductID = Common.GetPreviousProduct(ProductID,CategoryID,SectionID,0,0,false,true,true);
										if(CategoryID != 0)
										{
											writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndCategoryLink(PreviousProductID,CategoryID,"") + "\">previous</a>&nbsp;|&nbsp;");
										}
										else
										{
											writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndSectionLink(PreviousProductID,SectionID,"") + "\">previous</a>&nbsp;|&nbsp;");
										}
									}
									if(CategoryID != 0)
									{
										writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeCategoryLink(CategoryID,"") + "\">up</a>");
									}
									else
									{
										writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeSectionLink(SectionID,"") + "\">up</a>");
									}
									if(NumProducts > 1)
									{
										int NextProductID = Common.GetNextProduct(ProductID,CategoryID,SectionID,0,0,false,true,true);
										if(CategoryID != 0)
										{
											writer.Write("&nbsp;|&nbsp;<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndCategoryLink(NextProductID,CategoryID,"") + "\">next</a><br>&nbsp;");
										}
										else
										{
											writer.Write("&nbsp;|&nbsp;<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndSectionLink(NextProductID,SectionID,"") + "\">next</a><br>&nbsp;");
										}
									}
									writer.Write("</td></tr>\n");
								}
								writer.Write("<tr valign=\"top\"><td>");
								writer.Write("<font class=\"ProductNameText\">");
								writer.Write(DB.RSField(rs,"Name"));
								writer.Write("</font>&nbsp;&nbsp;");
								if(ProductHasSpecs)
								{
									writer.Write("&nbsp;&nbsp;<small>(<a href=\"" + SpecLink + "\" " + Common.IIF(SpecsInline , "" , "target=\"_blank\"") + ">" + SpecTitle + "</a>)</small>");
								}
								writer.Write("<br>");
								if(ActiveDID)
								{
									String S1 = String.Empty;
									writer.Write("<br><small>This product qualifies for quantity discount pricing. (<a href=\"javascript:void(0);\" style=\"cursor: normal;\" onMouseover=\"ddrivetip(" + Common.SQuote(Common.AppConfig("quantitydiscount") + "<br><br>" + Common.GetQuantityDiscountDisplayTable(ActiveDIDID)) + ",'" + Common.AppConfig("LightCellColor") + "', 300)\" onMouseout=\"hideddrivetip()\">what's this?</a>)</small>");
								}
								if(Common.AppConfigBool("ShowEMailProductToFriend"))
								{
									String S1 = String.Empty;
									writer.Write("<br><small><img src=\"skins/skin_" + _siteID.ToString() + "/images/mailicon.gif\" border=\"0\" align=\"absmiddle\">&nbsp;<a href=\"emailproduct.aspx?productid=" + ProductID.ToString() + "&categoryid=" + CategoryID.ToString() + "\">E-mail this product to a friend</a></small><br><br>");
								}
								writer.Write("<div align=\"left\">");
								writer.Write(ProductDescription);
								writer.Write("</div>");
							
								if(Common.AppConfigBool("ShowInventoryTable") && Common.ProductUsesAdvancedInventoryMgmt(ProductID))
								{
									writer.Write("<div align=\"left\"><br>");
									writer.Write(Common.GetInventoryTable(ProductID,Common.GetProductsFirstVariantID(ProductID),thisCustomer._isAdminUser));
									writer.Write("</div>");
								}
							
								writer.Write("</td></tr>");
								writer.Write("<tr valign=\"top\"><td height=\"10\"></td></tr>");
								writer.Write("<tr valign=\"top\"><td height=\"20\"><hr size=\"1\" color=\"#666666\"></td></tr>");
								MainProductSKU = String.Empty;
								if(!empty)
								{
									do
									{
										writer.Write("<tr valign=\"top\"><td>");
										MainProductSKU = DB.RSField(rs,"SKU");
										writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
										String PName = String.Empty;
										if(Common.AppConfigBool("ShowFullNameInRightBar"))
										{
											if(DB.RSField(rs,"VariantName").Length == 0)
											{
												PName = ProductName;
											}
											else
											{
												PName = ProductName + " -<br>" + DB.RSField(rs,"VariantName");
											}
										}
										else
										{
											if(DB.RSField(rs,"VariantName").Length == 0)
											{
												PName = ProductName;
											}
											else
											{
												PName = DB.RSField(rs,"VariantName");
											}
										}

										writer.Write("<tr valign=\"top\"><td colspan=\"2\"><b>" + PName + "</b></td></tr>");
										writer.Write("<tr valign=\"top\"><td width=\"50%\">SKU:</td><td>" + Common.MakeProperProductSKU(DB.RSField(rs,"SKU"),DB.RSField(rs,"SkuSuffix"),"","") + "</td></tr>");
										if(DB.RSField(rs,"VariantDescription").Length != 0)
										{
											writer.Write("<tr valign=\"top\"><td width=\"50%\">Description:</td><td>" + DB.RSField(rs,"VariantDescription") + "</td></tr>");
										}
										if(DB.RSField(rs,"Dimensions").Length != 0)
										{
											writer.Write("<tr valign=\"top\"><td width=\"50%\">Dimensions:</td><td>" + DB.RSField(rs,"Dimensions") + "</td></tr>");
										}
										if(DB.RSFieldSingle(rs,"Weight") != 0.0F)
										{
											writer.Write("<tr valign=\"top\"><td width=\"50%\">Weight:</td><td>" + DB.RSFieldSingle(rs,"Weight").ToString() + " " + Localization.WeightUnits() + ".</td></tr>");
										}
										if(Common.AppConfigBool("ShowInventoryTable"))
										{
											writer.Write("<tr valign=\"top\"><td width=\"50%\">In Stock:</td><td>");
											writer.Write(Common.GetInventoryTable(ProductID,DB.RSFieldInt(rs,"VariantID"),thisCustomer._isAdminUser));
											writer.Write("</td></tr>");
										}

										if(!DB.RSFieldBool(rs,"HidePriceUntilCart"))
										{
											int Points = DB.RSFieldInt(rs,"Points");
											if(CustomerLevelID == 0)
											{
												// show consumer pricing (i.e. customer level 0):
												if(DB.RSFieldDecimal(rs,"SalePrice") != System.Decimal.Zero)
												{
													writer.Write("<tr valign=\"top\"><td width=\"50%\"><strike><b>Price:</b></strike></td><td><strike><b>" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</b></strike></td></tr>");
													writer.Write("<tr valign=\"top\"><td width=\"50%\"><b><font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + DB.RSField(rs,"sDescription") + ":</font></b></td><td><b><font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"SalePrice")) + "</font></b>" + Common.IIF(thisCustomer._customerLevelID != 0 && Common.AppConfigBool("MicroPay.ShowPointsWithPrices"), "(" + Points.ToString() + " Points)", "") + "</td></tr>");
												}
												else
												{
													writer.Write("<tr valign=\"top\"><td width=\"50%\"><b>Price:</b></td><td><b>" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</b>" + Common.IIF(thisCustomer._customerLevelID != 0 && Common.AppConfigBool("MicroPay.ShowPointsWithPrices"), "(" + Points.ToString() + " Points)", "")+ "</td></tr>");
												}
											}
											else
											{
												// calculate and show "level" pricing:
												bool IsOnSale = false;
												decimal LevelPrice = Common.DetermineLevelPrice(DB.RSFieldInt(rs,"VariantID"),CustomerLevelID,out IsOnSale);
												writer.Write("<tr valign=\"top\"><td width=\"50%\"><b>Regular Price:</b></td><td><b>" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</b></td></tr>");
												writer.Write("<tr valign=\"top\"><td width=\"50%\"><b><font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + CustomerLevelName + " Price:</font></b></td><td><b><font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + Localization.CurrencyStringForDisplay( LevelPrice) + "</font></b>");
												writer.Write("&nbsp;" + Common.IIF(thisCustomer._customerLevelID !=0 && Common.AppConfigBool("MicroPay.ShowPointsWithPrices"), "(" + Points.ToString() + " Points)", "") + "&nbsp;");
												writer.Write("</td></tr>");
											}
										}

										writer.Write("<tr valign=\"top\"><td colspan=2 align=left>" + Common.GetAddToCartForm(false,true,ProductID,DB.RSFieldInt(rs,"VariantID"),_siteID,ProductDisplayFormatID,!ImgGal.IsEmpty()) + "</td></tr>");
										writer.Write("</table><br><br>");
										writer.Write("</td></tr>");
									} while (rs.Read());
								}
								else
								{
									writer.Write("<tr><td><b>This product is currently empty. Please check back soon for updates...</b></td></tr>");
								}

								writer.Write("</table></td></tr>");

								writer.Write("</table>");
								break;

							case 2:
								// GRID FORMAT:
								writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\">\n");
								writer.Write("<tr><td align=\"left\" valign=\"top\">");

								ProdPic = Common.LookupImage("Product",ProductID,"medium",_siteID);
								if(ProdPic.Length == 0)
								{
									ProdPic = Common.LookupImage("Product",ProductID,"icon",_siteID);
								}
								if(ProdPic.Length == 0)
								{
									ProdPic = Common.AppConfig("NoPicture");
								}
								if(HasLargePic) 
								{
									writer.Write("<img id=\"ProductPic" + ProductID.ToString() + "\" name=\"ProductPic" + ProductID.ToString() + "\" style=\"cursor:hand;\" onClick=\"popupimg('" + LargePic + "')\" alt=\"Click here to view larger image\" style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + ProdPic + "\">");
								}
								else
								{
									writer.Write("<img id=\"ProductPic" + ProductID.ToString() + "\" name=\"ProductPic" + ProductID.ToString() + "\" style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + ProdPic + "\">");
								}
								if(ImgGal._imgGalIcons.Length != 0)
								{
									writer.Write("<br><br>");
									writer.Write(ImgGal._imgGalIcons);
									if(SwatchPic.Length != 0)
									{
										writer.Write(SwatchImageMap);
										writer.Write("<br><img src=\"images/spacer.gif\" width=\"1\" height=\"4\"><br><img style=\"cursor: hand;\" src=\"" + SwatchPic + "\" usemap=\"#SwatchMap\" border=\"0\">");
									}
								}
								if(HasLargePic)
								{
									writer.Write("<br><img src=\"images/spacer.gif\" width=\"1\" height=\"4\"><br>");
									writer.Write("<div align=\"center\"><a href=\"javascript:void(0);\" onClick=\"javascript:popupimg('" + LargePic + "');\"><img src=\"skins/skin_" + _siteID.ToString() + "/images/showlarger.gif\" align=\"absmiddle\" border=\"0\" alt=\"Show Larger Picture\"></a></div><br>");
								}
							
								writer.Write("</td><td width=\"100%\" align=\"left\" valign=\"top\">");
								
								if(!Common.AppConfigBool("HideProductNextPrevLinks"))
								{
									writer.Write("<table width=\"100%\"><tr><td align=\"right\">");
									NumProducts = 0;
									if(CategoryID != 0)
									{
										NumProducts = Common.GetNumCategoryProducts(CategoryID,true,true);
									}
									else
									{
										NumProducts = Common.GetNumSectionProducts(SectionID,true,true);
									}
									if(NumProducts > 1)
									{
										int PreviousProductID = Common.GetPreviousProduct(ProductID,CategoryID,SectionID,0,0,false,true,true);
										if(CategoryID != 0)
										{
											writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndCategoryLink(PreviousProductID,CategoryID,"") + "\">previous</a>&nbsp;|&nbsp;");
										}
										else
										{
											writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndSectionLink(PreviousProductID,SectionID,"") + "\">previous</a>&nbsp;|&nbsp;");
										}
									}
									if(CategoryID != 0)
									{
										writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeCategoryLink(CategoryID,"") + "\">up</a>");
									}
									else
									{
										writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeSectionLink(SectionID,"") + "\">up</a>");
									}
									if(NumProducts > 1)
									{
										int NextProductID = Common.GetNextProduct(ProductID,CategoryID,SectionID,0,0,false,true,true);
										if(CategoryID != 0)
										{
											writer.Write("&nbsp;|&nbsp;<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndCategoryLink(NextProductID,CategoryID,"") + "\">next</a><br>&nbsp;");
										}
										else
										{
											writer.Write("&nbsp;|&nbsp;<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndSectionLink(NextProductID,SectionID,"") + "\">next</a><br>&nbsp;");
										}
									}
									writer.Write("</td></tr></table>\n");
								}
								
								writer.Write("<font class=\"ProductNameText\">");
								writer.Write(DB.RSField(rs,"Name"));
								writer.Write("</font>&nbsp;&nbsp;");
								if(ProductHasSpecs)
								{
									writer.Write("&nbsp;&nbsp;<small>(<a href=\"" + SpecLink + "\" " + Common.IIF(SpecsInline , "" , "target=\"_blank\"") + ">" + SpecTitle + "</a>)</small>");
								}
								writer.Write("<br>");
								if(ActiveDID)
								{
									String S1 = String.Empty;
									writer.Write("<br><small>This product qualifies for quantity discount pricing. (<a href=\"javascript:void(0);\" style=\"cursor: normal;\" onMouseover=\"ddrivetip(" + Common.SQuote(Common.AppConfig("quantitydiscount") + "<br><br>" + Common.GetQuantityDiscountDisplayTable(ActiveDIDID)) + ",'" + Common.AppConfig("LightCellColor") + "', 300)\" onMouseout=\"hideddrivetip()\">what's this?</a>)</small>");
								}
								if(Common.AppConfigBool("ShowEMailProductToFriend"))
								{
									String S1 = String.Empty;
									writer.Write("<br><small><img src=\"skins/skin_" + _siteID.ToString() + "/images/mailicon.gif\" border=\"0\" align=\"absmiddle\">&nbsp;<a href=\"emailproduct.aspx?productid=" + ProductID.ToString() + "&categoryid=" + CategoryID.ToString() + "\">E-mail this product to a friend</a></small><br><br>");
								}
								writer.Write("<div align=\"left\">");
								writer.Write(ProductDescription);
								writer.Write("</div>");
							
								if(Common.AppConfigBool("ShowInventoryTable") && Common.ProductUsesAdvancedInventoryMgmt(ProductID))
								{
									writer.Write("<div align=\"left\"><br>");
									writer.Write(Common.GetInventoryTable(ProductID,Common.GetProductsFirstVariantID(ProductID),thisCustomer._isAdminUser));
									writer.Write("</div>");
								}
							
								writer.Write("</td></tr></table>\n");

								writer.Write("<br><hr size=1><br>");

								int ItemNumber = 1;
								int ItemsPerRow = Common.AppConfigUSInt("DefaultProductColWidth");
								if(!empty)	
								{
									int tmpItemsPerRow = DB.RSFieldInt(rs,"ColWidth");
									if (tmpItemsPerRow != 0)
									{
										ItemsPerRow = tmpItemsPerRow;
									}
								}
								writer.Write("<table border=\"0\" cellpadding=\"4\" cellspacing=\"0\" width=\"100%\">");
								if(empty)
								{
									writer.Write("<tr><td colspan=\"" + ItemsPerRow.ToString() + "\"><b>This product is currently empty. Please check back soon for updates...</b></td></tr>");
								}
								else
								{
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
										writer.Write("<td width=\"" + (100/ItemsPerRow).ToString() + "%\" height=\"70\" align=\"center\" valign=\"top\">");
										String ImgUrl = Common.LookupImage("Variant",DB.RSFieldInt(rs,"VariantID"),"icon",_siteID);
										if(ImgUrl.Length == 0)
										{
											ImgUrl = Common.AppConfig("NoPicture");
										}
										if(ImgUrl.Length != 0)
										{
											writer.Write("<img style=\"" + Common.AppConfig("ImageFrameStyle").Replace("cursor: hand;","") + "\" src=\"" + ImgUrl + "\">");
											writer.Write("<br>");
										}
										String PName = String.Empty;
										if(Common.AppConfigBool("ShowFullNameInGrid"))
										{
											if(DB.RSField(rs,"VariantName").Length == 0)
											{
												PName = ProductName;
											}
											else
											{
												PName = ProductName + " -<br>" + DB.RSField(rs,"VariantName");
											}
										}
										else
										{
											if(DB.RSField(rs,"VariantName").Length == 0)
											{
												PName = ProductName;
											}
											else
											{
												PName = DB.RSField(rs,"VariantName");
											}
										}
										writer.Write("<b>" + PName + "</b><br>");
										writer.Write("<font class=\"SmallGridText\">SKU: " + Common.MakeProperProductSKU(DB.RSField(rs,"SKU"),DB.RSField(rs,"SkuSuffix"),"","") +  "</font><br>\n");
										if(DB.RSField(rs,"VariantDescription").Length != 0)
										{
											writer.Write("<font class=\"SmallGridText\">" + DB.RSField(rs,"VariantDescription") + "</font><br>\n");
										}
										if(DB.RSFieldSingle(rs,"Weight") != 0.0F)
										{
											writer.Write("<font class=\"SmallGridText\">" + DB.RSFieldSingle(rs,"Weight").ToString() + "</font><br>\n");
										}
										if(DB.RSField(rs,"Dimensions").Length != 0)
										{
											writer.Write(DB.RSField(rs,"Dimensions") + "<br>\n");
										}

										if(!DB.RSFieldBool(rs,"HidePriceUntilCart"))
										{
											int Points = DB.RSFieldInt(rs,"Points");
											if(CustomerLevelID == 0)
											{
												//show consumer pricing (e.g. level 0)
												String PriceString = "<b>" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</b>";
												if(DB.RSFieldDecimal(rs,"SalePrice") != System.Decimal.Zero)
												{
													PriceString = "<strike><b>Price:&nbsp;" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</b></strike>";
													PriceString += "<br><b><font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + DB.RSField(rs,"sDescription") + ":</font>&nbsp;<font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"SalePrice")) + "</font></b>";
													PriceString += Common.IIF(thisCustomer._customerLevelID != 0 && Common.AppConfigBool("MicroPay.ShowPointsWithPrices"), "(" + Points.ToString() + " Points)", "");
												}
												writer.Write(PriceString + "<br>\n");
											}
											else
											{
												// show level pricing:
												bool IsOnSale = false;
												decimal LevelPrice = Common.DetermineLevelPrice(DB.RSFieldInt(rs,"VariantID"),CustomerLevelID, out IsOnSale);
												String PriceString = "<b>Regular Price:&nbsp;" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</b>";
												PriceString += "<br><b><font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + CustomerLevelName + " Price:</font>&nbsp;<font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + Localization.CurrencyStringForDisplay( LevelPrice) + "</font></b>";
												PriceString += Common.IIF(thisCustomer._customerLevelID != 0 && Common.AppConfigBool("MicroPay.ShowPointsWithPrices"), "(" + Points.ToString() + " Points)", "");
												writer.Write(PriceString + "<br>\n");
											}
										}
										writer.Write(Common.GetAddToCartForm(false,true,ProductID,DB.RSFieldInt(rs,"VariantID"),_siteID,ProductDisplayFormatID,!ImgGal.IsEmpty()));

										writer.Write("</td>");
										ItemNumber++;
									} while (rs.Read());
									for(int i = ItemNumber; i<=ItemsPerRow; i++)
									{
										writer.Write("<td>&nbsp;</td>");
									}
									writer.Write("</tr>");
								}
								writer.Write("</table>");
								break;

							case 3:
								// TABLE - EXPANDED FORMAT:
								writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\">\n");
								writer.Write("<tr><td align=\"left\" valign=\"top\">");

								ProdPic = Common.LookupImage("Product",ProductID,"medium",_siteID);
								if(ProdPic.Length == 0)
								{
									ProdPic = Common.LookupImage("Product",ProductID,"icon",_siteID);
								}
								if(ProdPic.Length == 0)
								{
									ProdPic = Common.AppConfig("NoPicture");
								}
								if(HasLargePic) 
								{
									writer.Write("<img id=\"ProductPic" + ProductID.ToString() + "\" name=\"ProductPic" + ProductID.ToString() + "\" style=\"cursor:hand;\" onClick=\"popupimg('" + LargePic + "')\" alt=\"Click here to view larger image\" style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + ProdPic + "\">");
								}
								else
								{
									writer.Write("<img id=\"ProductPic" + ProductID.ToString() + "\" name=\"ProductPic" + ProductID.ToString() + "\" style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + ProdPic + "\">");
								}
								if(ImgGal._imgGalIcons.Length != 0)
								{
									writer.Write("<br><br>");
									writer.Write(ImgGal._imgGalIcons);
									if(SwatchPic.Length != 0)
									{
										writer.Write(SwatchImageMap);
										writer.Write("<br><img src=\"images/spacer.gif\" width=\"1\" height=\"4\"><br><img style=\"cursor: hand;\" src=\"" + SwatchPic + "\" usemap=\"#SwatchMap\" border=\"0\">");
									}
								}
								if(HasLargePic)
								{
									writer.Write("<br><img src=\"images/spacer.gif\" width=\"1\" height=\"4\"><br>");
									writer.Write("<div align=\"center\"><a href=\"javascript:void(0);\" onClick=\"javascript:popupimg('" + LargePic + "');\"><img src=\"skins/skin_" + _siteID.ToString() + "/images/showlarger.gif\" align=\"absmiddle\" border=\"0\" alt=\"Show Larger Picture\"></a></div><br>");
								}
							
								writer.Write("</td><td width=\"100%\" align=\"left\" valign=\"top\">");
								
																if(!Common.AppConfigBool("HideProductNextPrevLinks"))
								{
									writer.Write("<table width=\"100%\"><tr><td align=\"right\">");
									NumProducts = 0;
									if(CategoryID != 0)
									{
										NumProducts = Common.GetNumCategoryProducts(CategoryID,true,true);
									}
									else
									{
										NumProducts = Common.GetNumSectionProducts(SectionID,true,true);
									}
									if(NumProducts > 1)
									{
										int PreviousProductID = Common.GetPreviousProduct(ProductID,CategoryID,SectionID,0,0,false,true,true);
										if(CategoryID != 0)
										{
											writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndCategoryLink(PreviousProductID,CategoryID,"") + "\">previous</a>&nbsp;|&nbsp;");
										}
										else
										{
											writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndSectionLink(PreviousProductID,SectionID,"") + "\">previous</a>&nbsp;|&nbsp;");
										}
									}
									if(CategoryID != 0)
									{
										writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeCategoryLink(CategoryID,"") + "\">up</a>");
									}
									else
									{
										writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeSectionLink(SectionID,"") + "\">up</a>");
									}
									if(NumProducts > 1)
									{
										int NextProductID = Common.GetNextProduct(ProductID,CategoryID,SectionID,0,0,false,true,true);
										if(CategoryID != 0)
										{
											writer.Write("&nbsp;|&nbsp;<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndCategoryLink(NextProductID,CategoryID,"") + "\">next</a><br>&nbsp;");
										}
										else
										{
											writer.Write("&nbsp;|&nbsp;<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndSectionLink(NextProductID,SectionID,"") + "\">next</a><br>&nbsp;");
										}
									}
									writer.Write("</td></tr></table>\n");
								}
								
								writer.Write("<font class=\"ProductNameText\">");
								writer.Write(DB.RSField(rs,"Name"));
								writer.Write("</font>&nbsp;&nbsp;");
								if(ProductHasSpecs)
								{
									writer.Write("&nbsp;&nbsp;<small>(<a href=\"" + SpecLink + "\" " + Common.IIF(SpecsInline , "" , "target=\"_blank\"") + ">" + SpecTitle + "</a>)</small>");
								}
								writer.Write("<br>");
								if(ActiveDID)
								{
									String S1 = String.Empty;
									writer.Write("<br><small>This product qualifies for quantity discount pricing. (<a href=\"javascript:void(0);\" style=\"cursor: normal;\" onMouseover=\"ddrivetip(" + Common.SQuote(Common.AppConfig("quantitydiscount") + "<br><br>" + Common.GetQuantityDiscountDisplayTable(ActiveDIDID)) + ",'" + Common.AppConfig("LightCellColor") + "', 300)\" onMouseout=\"hideddrivetip()\">what's this?</a>)</small>");
								}
								if(Common.AppConfigBool("ShowEMailProductToFriend"))
								{
									String S1 = String.Empty;
									writer.Write("<br><small><img src=\"skins/skin_" + _siteID.ToString() + "/images/mailicon.gif\" border=\"0\" align=\"absmiddle\">&nbsp;<a href=\"emailproduct.aspx?productid=" + ProductID.ToString() + "&categoryid=" + CategoryID.ToString() + "\">E-mail this product to a friend</a></small><br><br>");
								}
								writer.Write("<div align=\"left\">");
								writer.Write(ProductDescription);
								writer.Write("</div>");
							
								if(Common.AppConfigBool("ShowInventoryTable") && Common.ProductUsesAdvancedInventoryMgmt(ProductID))
								{
									writer.Write("<div align=\"left\"><br>");
									writer.Write(Common.GetInventoryTable(ProductID,Common.GetProductsFirstVariantID(ProductID),thisCustomer._isAdminUser));
									writer.Write("</div>");
								}
							
								writer.Write("</td></tr></table>\n");

								writer.Write("<br><hr size=1><br>");

								if(!empty)
								{
									do
									{
										WriteVariantBar(writer, thisCustomer,rs,DB.RSFieldInt(rs,"VariantID"),ProductID,_siteID);
									} while(rs.Read());
								}
								else
								{
									writer.Write("<b>This product is currently empty. Please check back soon for updates...</b>");
								}
								break;

							case 4:
								// TABLE - CONDENSED FORMAT:
								IDataReader rsx = DB.GetRS("select count(*) as N from productvariant  " + DB.GetNoLock() + " where deleted=0 and published=1 and description IS NOT NULL and productid=" + ProductID.ToString());
								rsx.Read();
								bool SomeDescriptions = (DB.RSFieldInt(rsx,"N") != 0);
								rsx.Close();
								writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\">\n");
								writer.Write("<tr><td align=\"left\" valign=\"top\">");

								ProdPic = Common.LookupImage("Product",ProductID,"medium",_siteID);
								if(ProdPic.Length == 0)
								{
									ProdPic = Common.LookupImage("Product",ProductID,"icon",_siteID);
								}
								if(ProdPic.Length == 0)
								{
									ProdPic = Common.AppConfig("NoPicture");
								}
								if(HasLargePic) 
								{
									writer.Write("<img style=\"cursor:hand;\" onClick=\"popupimg('" + LargePic + "')\" alt=\"Click here to view larger image\" style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + ProdPic + "\">");
								}
								else
								{
									writer.Write("<img style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + ProdPic + "\">");
								}
								writer.Write("<br>");
								writer.Write("");
								if(HasLargePic)
								{
									writer.Write("<div align=\"center\"><a href=\"javascript:void(0);\" onClick=\"javascript:popupimg('" + LargePic + "');\"><img src=\"skins/skin_" + _siteID.ToString() + "/images/showlarger.gif\" align=\"absmiddle\" border=\"0\" alt=\"Show Larger Picture\"></a></div><br>");
								}
							
								writer.Write("</td><td width=\"100%\" align=\"left\" valign=\"top\">");
								
																if(!Common.AppConfigBool("HideProductNextPrevLinks"))
								{
									writer.Write("<table width=\"100%\"><tr><td align=\"right\">");
									NumProducts = 0;
									if(CategoryID != 0)
									{
										NumProducts = Common.GetNumCategoryProducts(CategoryID,true,true);
									}
									else
									{
										NumProducts = Common.GetNumSectionProducts(SectionID,true,true);
									}
									if(NumProducts > 1)
									{
										int PreviousProductID = Common.GetPreviousProduct(ProductID,CategoryID,SectionID,0,0,false,true,true);
										if(CategoryID != 0)
										{
											writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndCategoryLink(PreviousProductID,CategoryID,"") + "\">previous</a>&nbsp;|&nbsp;");
										}
										else
										{
											writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndSectionLink(PreviousProductID,SectionID,"") + "\">previous</a>&nbsp;|&nbsp;");
										}
									}
									if(CategoryID != 0)
									{
										writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeCategoryLink(CategoryID,"") + "\">up</a>");
									}
									else
									{
										writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeSectionLink(SectionID,"") + "\">up</a>");
									}
									if(NumProducts > 1)
									{
										int NextProductID = Common.GetNextProduct(ProductID,CategoryID,SectionID,0,0,false,true,true);
										if(CategoryID != 0)
										{
											writer.Write("&nbsp;|&nbsp;<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndCategoryLink(NextProductID,CategoryID,"") + "\">next</a><br>&nbsp;");
										}
										else
										{
											writer.Write("&nbsp;|&nbsp;<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndSectionLink(NextProductID,SectionID,"") + "\">next</a><br>&nbsp;");
										}
									}
									writer.Write("</td></tr></table>\n");
								}
								
								writer.Write("<font class=\"ProductNameText\">");
								writer.Write(DB.RSField(rs,"Name"));
								writer.Write("</font>&nbsp;&nbsp;");
								if(ProductHasSpecs)
								{
									writer.Write("&nbsp;&nbsp;<small>(<a href=\"" + SpecLink + "\" " + Common.IIF(SpecsInline , "" , "target=\"_blank\"") + ">" + SpecTitle + "</a>)</small>");
								}
								writer.Write("<br>");
								if(ActiveDID)
								{
									String S1 = String.Empty;
									writer.Write("<br><small>This product qualifies for quantity discount pricing. (<a href=\"javascript:void(0);\" style=\"cursor: normal;\" onMouseover=\"ddrivetip(" + Common.SQuote(Common.AppConfig("quantitydiscount") + "<br><br>" + Common.GetQuantityDiscountDisplayTable(ActiveDIDID)) + ",'" + Common.AppConfig("LightCellColor") + "', 300)\" onMouseout=\"hideddrivetip()\">what's this?</a>)</small>");
								}
								if(Common.AppConfigBool("ShowEMailProductToFriend"))
								{
									String S1 = String.Empty;
									writer.Write("<br><small><img src=\"skins/skin_" + _siteID.ToString() + "/images/mailicon.gif\" border=\"0\" align=\"absmiddle\">&nbsp;<a href=\"emailproduct.aspx?productid=" + ProductID.ToString() + "&categoryid=" + CategoryID.ToString() + "\">E-mail this product to a friend</a></small><br><br>");
								}
								writer.Write("<div align=\"left\">");
								writer.Write(ProductDescription);
								writer.Write("</div>");
							
								if(Common.AppConfigBool("ShowInventoryTable") && Common.ProductUsesAdvancedInventoryMgmt(ProductID))
								{
									writer.Write("<div align=\"left\"><br>");
									writer.Write(Common.GetInventoryTable(ProductID,Common.GetProductsFirstVariantID(ProductID),thisCustomer._isAdminUser));
									writer.Write("</div>");
								}
							
								writer.Write("</td></tr></table>\n");

								writer.Write("<br>");
								//writer.Write("<br><hr size=1><br>");

								if(!empty)
								{
									writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
									writer.Write("    <tr class=\"DarkCell\">\n");
									writer.Write("      <td width=\"10%\" align=\"center\"><font class=\"CondensedDarkCellText\"><b>Photo</b></font></td>\n");
									writer.Write("      <td width=\"15%\" align=\"left\"><font class=\"CondensedDarkCellText\"><b>Product</b></font></td>\n");
									writer.Write("      <td width=\"10%\" align=\"center\"><font class=\"CondensedDarkCellText\"><b>SKU</b></font></td>\n");
									if(Common.AppConfigBool("ShowDescriptionInTableCondensed") && SomeDescriptions)
									{
										writer.Write("      <td width=\"30%\" align=\"left\"><font class=\"CondensedDarkCellText\"><b>Description</b></font></td>\n");
									}
									if(Common.AppConfigBool("ShowWeightInTableCondensed"))
									{
										writer.Write("      <td width=\"30%\" align=\"center\"><font class=\"CondensedDarkCellText\"><b>Weight</b></font></td>\n");
									}
									if(Common.AppConfigBool("ShowDimensionsInTableCondensed"))
									{
										writer.Write("      <td align=\"center\"><font class=\"CondensedDarkCellText\"><b>Dimensions</b></font></td>\n");
									}
									if(!DB.RSFieldBool(rs,"HidePriceUntilCart"))
									{
										writer.Write("      <td width=\"10%\" align=\"center\"><font class=\"CondensedDarkCellText\"><b>Price</b></font></td>\n");
									}
									writer.Write("      <td width=\"20%\" align=\"center\"><font class=\"CondensedDarkCellText\"><b>Order</b></font></td>\n");
									writer.Write("    </tr>\n");
									int row = 1;
									do
									{
										writer.Write("<tr " + Common.IIF(row % 2 == 0 , "class=\"LightCell\"" , "") + ">\n");

										writer.Write("<td valign=\"middle\" align=\"center\"><font class=\"CondensedVariantText\">");
										String Image1URL = Common.LookupImage("Variant",DB.RSFieldInt(rs,"VariantID"),"icon",_siteID);
										int HT = Common.AppConfigUSInt("CondensedTablePictureHeight");
										if(HT == 0)
										{
											HT = 50;
										}
										if(Image1URL.Length == 0)
										{
											Image1URL = Common.AppConfig("NoPictureIcon");
										}
										writer.Write("<img style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + Image1URL + "\" height=\"" + HT.ToString() + "\" align=\"absmiddle\">");
										writer.Write("</td>");
					
										String PName = String.Empty;
										if(Common.AppConfigBool("ShowFullNameInTableCondensed"))
										{
											if(DB.RSField(rs,"VariantName").Length == 0)
											{
												PName = ProductName;
											}
											else
											{
												PName = ProductName + " - " + DB.RSField(rs,"VariantName");
											}
										}
										else
										{
											if(DB.RSField(rs,"VariantName").Length == 0)
											{
												PName = ProductName;
											}
											else
											{
												PName = DB.RSField(rs,"VariantName");
											}
										}
										writer.Write("<td valign=\"middle\" align=\"left\"><font class=\"CondensedVariantText\">" + PName + "</font></td>\n");
										writer.Write("<td valign=\"middle\" align=\"center\"><font class=\"CondensedVariantText\">" + Common.MakeProperProductSKU(DB.RSField(rs,"SKU"),DB.RSField(rs,"SkuSuffix"),"","") +  "</font></td>\n");
										if(Common.AppConfigBool("ShowDescriptionInTableCondensed") && SomeDescriptions)
										{
											writer.Write("<td valign=\"middle\" align=\"left\"><font class=\"CondensedVariantText\">" + DB.RSField(rs,"VariantDescription") + "</font></td>\n");
										}
										if(Common.AppConfigBool("ShowWeightInTableCondensed"))
										{
											writer.Write("<td valign=\"middle\" align=\"center\"><font class=\"CondensedVariantText\">" + DB.RSFieldSingle(rs,"Weight").ToString() + "</font></td>\n");
										}
										if(Common.AppConfigBool("ShowDimensionsInTableCondensed"))
										{
											writer.Write("<td valign=\"middle\" align=\"center\"><font class=\"CondensedVariantText\">" + DB.RSField(rs,"Dimensions") + "</font></td>\n");
										}

										if(!DB.RSFieldBool(rs,"HidePriceUntilCart"))
										{
											int Points = DB.RSFieldInt(rs,"Points");
											if(CustomerLevelID == 0)
											{
												// show consumer pricing (e.g. level 0):
												String PriceString = "<b>" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</b>";
												if(DB.RSFieldDecimal(rs,"SalePrice") != System.Decimal.Zero)
												{
													PriceString = "<strike><b>Price:&nbsp;" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</b></strike>";
													PriceString += "<br><b><font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + DB.RSField(rs,"sDescription") + ":</font>&nbsp;<font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"SalePrice")) + "</font></b>";
													PriceString += Common.IIF(thisCustomer._customerLevelID != 0 && Common.AppConfigBool("MicroPay.ShowPointsWithPrices"), "(" + Points.ToString() + " Points)", "");
												}
												writer.Write("<td valign=\"middle\" align=\"center\"><font class=\"CondensedVariantText\">" + PriceString + "</font></td>\n");
											}
											else
											{
												// show level pricing:
												bool IsOnSale = false;
												decimal LevelPrice = Common.DetermineLevelPrice(DB.RSFieldInt(rs,"VariantID"),CustomerLevelID, out IsOnSale);
												String PriceString = "<strike><b>Regular Price:&nbsp;" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</b></strike>";
												PriceString += "<br><b><font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + CustomerLevelName + " Price:</font>&nbsp;<font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + Localization.CurrencyStringForDisplay( LevelPrice) + "</font></b>";
												PriceString += Common.IIF(thisCustomer._customerLevelID != 0 && Common.AppConfigBool("MicroPay.ShowPointsWithPrices"), "(" + Points.ToString() + " Points)", "");
												writer.Write("<td valign=\"middle\" align=\"center\"><font class=\"CondensedVariantText\">" + PriceString + "</font></td>\n");
											}
										}
										writer.Write("<td valign=\"middle\" align=\"center\">" + Common.GetAddToCartForm(false,true,ProductID,DB.RSFieldInt(rs,"VariantID"),_siteID,ProductDisplayFormatID,!ImgGal.IsEmpty()) + "</td>");
										writer.Write("</tr>\n");
										row++;
									} while(rs.Read());
									writer.Write("</table>");
								}
								else
								{
									writer.Write("<b>This product is currently empty. Please check back soon for updates...</b>");
								}
								break;
							case 5:
								// SINGLE VARIANT FORMAT (NOTE: ONLY WORKS IF PRODUCT HAS ONE VARIANT!)
								writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"4\" width=\"100%\">");
								writer.Write("<tr>");
								writer.Write("<td align=\"center\" valign=\"top\" width=\"40%\">");
								ProdPic = Common.LookupImage("Product",ProductID,"medium",_siteID);
								if(ProdPic.Length == 0)
								{
									ProdPic = Common.LookupImage("Product",ProductID,"icon",_siteID);
								}
								if(ProdPic.Length == 0)
								{
									ProdPic = Common.AppConfig("NoPicture");
								}
								if(HasLargePic) 
								{
									writer.Write("<img id=\"ProductPic" + ProductID.ToString() + "\" name=\"ProductPic" + ProductID.ToString() + "\" style=\"cursor:hand;\" onClick=\"popupimg('" + LargePic + "')\" alt=\"Click here to view larger image\" style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + ProdPic + "\">");
								}
								else
								{
									writer.Write("<img id=\"ProductPic" + ProductID.ToString() + "\" name=\"ProductPic" + ProductID.ToString() + "\" style=\"" + Common.AppConfig("ImageFrameStyle").Replace("cursor: hand;","") + "\" src=\"" + ProdPic + "\">");
								}
								if(ImgGal._imgGalIcons.Length != 0)
								{
									writer.Write("<br><br>");
									writer.Write(ImgGal._imgGalIcons);
									if(SwatchPic.Length != 0)
									{
										writer.Write(SwatchImageMap);
										writer.Write("<br><img src=\"images/spacer.gif\" width=\"1\" height=\"4\"><br><img style=\"cursor: hand;\" src=\"" + SwatchPic + "\" usemap=\"#SwatchMap\" border=\"0\">");
									}
								}
								if(HasLargePic)
								{
									writer.Write("<br><img src=\"images/spacer.gif\" width=\"1\" height=\"4\"><br>");
									writer.Write("<div align=\"center\"><a href=\"javascript:void(0);\" onClick=\"javascript:popupimg('" + LargePic + "');\"><img src=\"skins/skin_" + _siteID.ToString() + "/images/showlarger.gif\" align=\"absmiddle\" border=\"0\" alt=\"Show Larger Picture\"></a></div><br>");
								}
								//writer.Write("<br>SKU Shown: " + Common.MakeProperProductSKU(DB.RSField(rs,"SKU"),DB.RSField(rs,"SKUSuffix"),"",""));
								writer.Write("</td>");
								writer.Write("<td align=\"left\" valign=\"top\">");

								writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");

								if(!Common.AppConfigBool("HideProductNextPrevLinks"))
								{
									writer.Write("<tr><td align=\"right\">");
									NumProducts = 0;
									if(CategoryID != 0)
									{
										NumProducts = Common.GetNumCategoryProducts(CategoryID,true,true);
									}
									else
									{
										NumProducts = Common.GetNumSectionProducts(SectionID,true,true);
									}
									if(NumProducts > 1)
									{
										int PreviousProductID = Common.GetPreviousProduct(ProductID,CategoryID,SectionID,0,0,false,true,true);
										if(CategoryID != 0)
										{
											writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndCategoryLink(PreviousProductID,CategoryID,"") + "\">previous</a>&nbsp;|&nbsp;");
										}
										else
										{
											writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndSectionLink(PreviousProductID,SectionID,"") + "\">previous</a>&nbsp;|&nbsp;");
										}
									}
									if(CategoryID != 0)
									{
										writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeCategoryLink(CategoryID,"") + "\">up</a>");
									}
									else
									{
										writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeSectionLink(SectionID,"") + "\">up</a>");
									}
									if(NumProducts > 1)
									{
										int NextProductID = Common.GetNextProduct(ProductID,CategoryID,SectionID,0,0,false,true,true);
										if(CategoryID != 0)
										{
											writer.Write("&nbsp;|&nbsp;<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndCategoryLink(NextProductID,CategoryID,"") + "\">next</a><br>&nbsp;");
										}
										else
										{
											writer.Write("&nbsp;|&nbsp;<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndSectionLink(NextProductID,SectionID,"") + "\">next</a><br>&nbsp;");
										}
									}
									writer.Write("</td></tr>\n");
								}

								writer.Write("<tr valign=\"top\"><td><font class=\"ProductNameText\">");
								writer.Write((DB.RSField(rs,"Name") + " " + DB.RSField(rs,"VariantName")).Trim());
								writer.Write("</font>&nbsp;&nbsp;");
								if(ProductHasSpecs)
								{
									writer.Write("&nbsp;&nbsp;<small>(<a href=\"" + SpecLink + "\" " + Common.IIF(SpecsInline , "" , "target=\"_blank\"") + ">" + SpecTitle + "</a>)</small>");
								}
								writer.Write("<br>");
								if(ActiveDID)
								{
									String S1 = String.Empty;
									writer.Write("<br>");
									writer.Write("<small>This product qualifies for quantity discount pricing. (<a href=\"javascript:void(0);\" style=\"cursor: normal;\" onMouseover=\"ddrivetip(" + Common.SQuote(Common.AppConfig("quantitydiscount") + "<br><br>" + Common.GetQuantityDiscountDisplayTable(ActiveDIDID)) + ",'" + Common.AppConfig("LightCellColor") + "', 300)\" onMouseout=\"hideddrivetip()\">what's this?</a>)</small>");
								}
								if(Common.AppConfigBool("ShowEMailProductToFriend"))
								{
									String S1 = String.Empty;
									writer.Write("<br><small><img src=\"skins/skin_" + _siteID.ToString() + "/images/mailicon.gif\" border=\"0\" align=\"absmiddle\">&nbsp;<a href=\"emailproduct.aspx?productid=" + ProductID.ToString() + "&categoryid=" + CategoryID.ToString() + "\">E-mail this product to a friend</a></small><br><br>");
								}
								writer.Write("</td></tr>");
								writer.Write("<tr valign=\"top\"><td>");
								writer.Write("<div align=\"left\">");
								writer.Write(ProductDescription);
								writer.Write("</div>");
							
								if(Common.AppConfigBool("ShowInventoryTable"))
								{
									writer.Write("<div align=\"left\"><br>");
									writer.Write(Common.GetInventoryTable(ProductID,Common.GetProductsFirstVariantID(ProductID),thisCustomer._isAdminUser));
									writer.Write("</div>");
								}
							
								writer.Write("</td></tr>");
								writer.Write("<tr valign=\"top\"><td height=10></td></tr>");

								writer.Write("<tr valign=\"top\"><td>");

								writer.Write("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\n");
								writer.Write("<tr valign=\"top\"><td width=150><b>SKU:</b></td><td>" + Common.MakeProperProductSKU(DB.RSField(rs,"SKU"),DB.RSField(rs,"SkuSuffix"),"","") + " (" + ProductID.ToString() + ")</td></tr>");
								writer.Write("<tr valign=\"top\"><td colspan=2 height=10></td></tr>");
								if(DB.RSField(rs,"Dimensions").Length != 0)
								{
									writer.Write("<tr valign=\"top\"><td width=150><b>Dimensions:</b></td><td>" +  DB.RSField(rs,"Dimensions") + "</td></tr>");
									writer.Write("<tr valign=\"top\"><td colspan=2 height=10></td></tr>");
								}
								if(DB.RSFieldSingle(rs,"Weight") != 0.0F)
								{
									writer.Write("<tr valign=\"top\"><td width=150><b>Weight:</b></td><td>" +  DB.RSFieldSingle(rs,"Weight").ToString() + "</td></tr>");
									writer.Write("<tr valign=\"top\"><td colspan=2 height=10></td></tr>");
								}

								writer.Write("<tr valign=\"top\"><td colspan=2 height=10></td></tr>");
								if(!DB.RSFieldBool(rs,"HidePriceUntilCart"))
								{
									int Points = DB.RSFieldInt(rs,"Points");
									if(CustomerLevelID == 0)
									{
										// show consumer (e.g. level 0) pricing:
										if(DB.RSFieldDecimal(rs,"SalePrice") != System.Decimal.Zero)
										{
											writer.Write("<tr valign=\"top\"><td width=150><b>Price:</b></td><td><b><strike>" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</strike>&nbsp;&nbsp;&nbsp;<font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + DB.RSField(rs,"sDescription") + ":</font> <font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"SalePrice")) + "</font></b>" + Common.IIF(thisCustomer._customerLevelID != 0 && Common.AppConfigBool("MicroPay.ShowPointsWithPrices"), "(" + Points.ToString() + " Points)", "") + "</td></tr>");
											writer.Write("<tr valign=\"top\"><td colspan=2 height=10></td></tr>");
										}
										else
										{
											writer.Write("<tr valign=\"top\"><td width=150><b>Price:</b></td><td><b>" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</b>" + Common.IIF(thisCustomer._customerLevelID != 0 && Common.AppConfigBool("MicroPay.ShowPointsWithPrices"), "(" + Points.ToString() + " Points)", "") + "</td></tr>");
											writer.Write("<tr valign=\"top\"><td colspan=2 height=10></td></tr>");
										}
									}
									else
									{
										// show level pricing:
										bool IsOnSale = false;
										decimal LevelPrice = Common.DetermineLevelPrice(DB.RSFieldInt(rs,"VariantID"),CustomerLevelID, out IsOnSale);
										writer.Write("<tr valign=\"top\"><td width=150><b>Regular Price:</b></td><td><b>" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</b></td></tr>");
										writer.Write("<tr valign=\"top\"><td colspan=2 height=10></td></tr>");
										writer.Write("<tr valign=\"top\"><td width=150><b><font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + CustomerLevelName + " Price:</font></b></td><td><b><font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + Localization.CurrencyStringForDisplay( LevelPrice) + "</font></b>");
										writer.Write("&nbsp;" + Common.IIF(thisCustomer._customerLevelID !=0 && Common.AppConfigBool("MicroPay.ShowPointsWithPrices"), "(" + Points.ToString() + " Points)", "") + "&nbsp;");
										writer.Write("</td></tr>");
										writer.Write("<tr valign=\"top\"><td colspan=2 height=10></td></tr>");
									}
								}
								//if(DB.RSField(rs,"Colors").Length != 0)
								//{
								//	writer.Write("<tr valign=\"top\"><td width=150><b>Colors:</b></td><td>" +  DB.RSField(rs,"Colors") + "</td></tr>");
								//	writer.Write("<tr valign=\"top\"><td colspan=2 height=10></td></tr>");
								//}
								//if(DB.RSField(rs,"Sizes").Length != 0)
								//{
								//	writer.Write("<tr valign=\"top\"><td width=150><b>Sizes:</b></td><td>" +  DB.RSField(rs,"Sizes") + "</td></tr>");
								//	writer.Write("<tr valign=\"top\"><td colspan=2 height=10></td></tr>");
								//}
								writer.Write("</table>");

								writer.Write("</td></tr>");

								writer.Write("<tr valign=\"top\"><td>" + Common.GetAddToCartForm(false,true,ProductID,DB.RSFieldInt(rs,"VariantID"),_siteID,ProductDisplayFormatID,!ImgGal.IsEmpty()) + "</td></tr>");
								writer.Write("</table>");
								writer.Write("</td></tr>");
								writer.Write("</table>");
								break;
							case 6:
								// SINGLE VARIANT FORMAT 2 (NOTE: ONLY WORKS IF PRODUCT HAS ONE VARIANT!)
								writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"4\" width=\"100%\">");
								writer.Write("<tr>");
								writer.Write("<td align=\"center\" valign=\"top\" width=\"40%\">");
								ProdPic = Common.LookupImage("Product",ProductID,"medium",_siteID);
								if(ProdPic.Length == 0)
								{
									ProdPic = Common.LookupImage("Product",ProductID,"icon",_siteID);
								}
								if(ProdPic.Length == 0)
								{
									ProdPic = Common.AppConfig("NoPicture");
								}
								HasLargePic = Common.FileExists(Common.LookupImage("Product",ProductID,"large",_siteID));
								if(HasLargePic) 
								{
									writer.Write("<img id=\"ProductPic" + ProductID.ToString() + "\" name=\"ProductPic" + ProductID.ToString() + "\" style=\"cursor:hand;\" onClick=\"popupimg('" + LargePic + "')\" alt=\"Click here to view larger image\" style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + ProdPic + "\">");
								}
								else
								{
									writer.Write("<img id=\"ProductPic" + ProductID.ToString() + "\" name=\"ProductPic" + ProductID.ToString() + "\" style=\"" + Common.AppConfig("ImageFrameStyle").Replace("cursor: hand;","") + "\" src=\"" + ProdPic + "\">");
								}
								if(ImgGal._imgGalIcons.Length != 0)
								{
									writer.Write("<br><br>");
									writer.Write(ImgGal._imgGalIcons);
									if(SwatchPic.Length != 0)
									{
										writer.Write(SwatchImageMap);
										writer.Write("<br><img src=\"images/spacer.gif\" width=\"1\" height=\"4\"><br><img style=\"cursor: hand;\" src=\"" + SwatchPic + "\" usemap=\"#SwatchMap\" border=\"0\">");
									}
								}
								if(HasLargePic)
								{
									writer.Write("<br><img src=\"images/spacer.gif\" width=\"1\" height=\"4\"><br>");
									writer.Write("<div align=\"center\"><a href=\"javascript:void(0);\" onClick=\"javascript:popupimg('" + LargePic + "');\"><img src=\"skins/skin_" + _siteID.ToString() + "/images/showlarger.gif\" align=\"absmiddle\" border=\"0\" alt=\"Show Larger Picture\"></a></div><br>");
								}
								//writer.Write("<br>SKU Shown: " + Common.MakeProperProductSKU(DB.RSField(rs,"SKU"),DB.RSField(rs,"SKUSuffix"),"",""));
								writer.Write("</td>");
								writer.Write("<td align=\"left\" valign=\"top\">");

								writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");

								if(!Common.AppConfigBool("HideProductNextPrevLinks"))
								{
									writer.Write("<tr><td align=\"right\">");
									NumProducts = 0;
									if(CategoryID != 0)
									{
										NumProducts = Common.GetNumCategoryProducts(CategoryID,true,true);
									}
									else
									{
										NumProducts = Common.GetNumSectionProducts(SectionID,true,true);
									}
									if(NumProducts > 1)
									{
										int PreviousProductID = Common.GetPreviousProduct(ProductID,CategoryID,SectionID,0,0,false,true,true);
										if(CategoryID != 0)
										{
											writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndCategoryLink(PreviousProductID,CategoryID,"") + "\">previous</a>&nbsp;|&nbsp;");
										}
										else
										{
											writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndSectionLink(PreviousProductID,SectionID,"") + "\">previous</a>&nbsp;|&nbsp;");
										}
									}
									if(CategoryID != 0)
									{
										writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeCategoryLink(CategoryID,"") + "\">up</a>");
									}
									else
									{
										writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeSectionLink(SectionID,"") + "\">up</a>");
									}
									if(NumProducts > 1)
									{
										int NextProductID = Common.GetNextProduct(ProductID,CategoryID,SectionID,0,0,false,true,true);
										if(CategoryID != 0)
										{
											writer.Write("&nbsp;|&nbsp;<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndCategoryLink(NextProductID,CategoryID,"") + "\">next</a><br>&nbsp;");
										}
										else
										{
											writer.Write("&nbsp;|&nbsp;<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndSectionLink(NextProductID,SectionID,"") + "\">next</a><br>&nbsp;");
										}
									}
									writer.Write("</td></tr>\n");
								}

								writer.Write("<tr valign=\"top\"><td><font class=\"ProductNameText\">");
								writer.Write((DB.RSField(rs,"Name") + " " + DB.RSField(rs,"VariantName")).Trim());
								writer.Write("</font>&nbsp;&nbsp;");
								if(ProductHasSpecs)
								{
									writer.Write("&nbsp;&nbsp;<small>(<a href=\"" + SpecLink + "\" " + Common.IIF(SpecsInline , "" , "target=\"_blank\"") + ">" + SpecTitle + "</a>)</small>");
								}
								writer.Write("<br>");
								writer.Write("<span class=\"ProductSKUText\">" + DB.RSField(rs,"SKU") + "</span><br>");
								writer.Write("<br>");
								if(Common.AppConfigBool("ShowEMailProductToFriend"))
								{
									String S1 = String.Empty;
									writer.Write("<small><img src=\"skins/skin_" + _siteID.ToString() + "/images/mailicon.gif\" border=\"0\" align=\"absmiddle\">&nbsp;<a href=\"emailproduct.aspx?productid=" + ProductID.ToString() + "&categoryid=" + CategoryID.ToString() + "\">E-mail this product to a friend</a></small><br><br>");
								}
								writer.Write("</td></tr>");

								writer.Write("<tr valign=\"top\"><td align=\"left\" valign=\"top\">");
								writer.Write(ProductDescription);
								writer.Write("</td></tr>");

								if(Common.AppConfigBool("ShowInventoryTable"))
								{
									writer.Write("<tr valign=\"top\"><td align=\"left\" valign=\"top\"><br>");
									writer.Write(Common.GetInventoryTable(ProductID,Common.GetProductsFirstVariantID(ProductID),thisCustomer._isAdminUser));
									writer.Write("</td></tr>");
								}
							
								writer.Write("<tr valign=\"top\"><td><br>");
								if(!DB.RSFieldBool(rs,"HidePriceUntilCart"))
								{
									int Points = DB.RSFieldInt(rs,"Points");
									if(CustomerLevelID == 0)
									{
										// show consumer (e.g. level 0) pricing:
										if(DB.RSFieldDecimal(rs,"SalePrice") != System.Decimal.Zero)
										{
											writer.Write("<b>Price:</b> <b><strike>" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</strike>&nbsp;&nbsp;&nbsp;<font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + DB.RSField(rs,"sDescription") + ":</font> <font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"SalePrice")) + "</font></b>" + Common.IIF(thisCustomer._customerLevelID != 0 && Common.AppConfigBool("MicroPay.ShowPointsWithPrices"), "(" + Points.ToString() + " Points)", "") + "<br>");
										}
										else
										{
											writer.Write("<b>Price:</b> <b>" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</b>" + Common.IIF(thisCustomer._customerLevelID != 0 && Common.AppConfigBool("MicroPay.ShowPointsWithPrices"), "(" + Points.ToString() + " Points)", "") + "<br>");
										}
									}
									else
									{
										// show level pricing:
										bool IsOnSale = false;
										decimal LevelPrice = Common.DetermineLevelPrice(DB.RSFieldInt(rs,"VariantID"),CustomerLevelID, out IsOnSale);
										writer.Write("<b>Regular Price:</b> <b>" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</b><br>");
										writer.Write("<b><font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + CustomerLevelName + " Price:</font></b> <b><font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + Localization.CurrencyStringForDisplay( LevelPrice) + "</font></b>");
										writer.Write("&nbsp;" + Common.IIF(thisCustomer._customerLevelID !=0 && Common.AppConfigBool("MicroPay.ShowPointsWithPrices"), "(" + Points.ToString() + " Points)", "") + "&nbsp;");
										writer.Write("<br>");
									}
								}
								writer.Write("</td></tr>");

								writer.Write("<tr valign=\"top\"><td>" + Common.GetAddToCartForm(false,true,ProductID,DB.RSFieldInt(rs,"VariantID"),_siteID,ProductDisplayFormatID,!ImgGal.IsEmpty()) + "</td></tr>");
	
								writer.Write("</table>");
								writer.Write("</td></tr>");
								writer.Write("</table>");
								break;
						}
					}
					if(UpsellProducts.Length != 0)
					{
						try
						{
							// people type weird things in the upsell box field, so ignore any "issues"...no other good solution at the moment:
							String S = Common.GetUpsellProductsBoxExpanded(ProductID,100,true,String.Empty,Common.AppConfig("RelatedProductsFormat").ToUpper() == "GRID",_siteID,thisCustomer._customerLevelID);
							if(S.Length != 0)
							{
								writer.Write("<br clear=\"all\">");
								writer.Write(S);
							}
						}
						catch {}
					}
					if(RelatedProducts.Length != 0)
					{
						try
						{
							// people type weird things in the related box field, so ignore any "issues"...no other good solution at the moment:
							String S = Common.GetRelatedProductsBoxExpanded(ProductID,100,true,String.Empty,Common.AppConfig("RelatedProductsFormat").ToUpper() == "GRID",_siteID);
							if(S.Length != 0)
							{
								writer.Write("<br clear=\"all\">");
								writer.Write(S);
							}
						}
						catch {}
					}
					rs.Close();

					if(ProductHasSpecs && SpecsInline)
					{
						writer.Write("<hr size=1>\n");
						writer.Write("<p><a name=\"Specs\"><b>" + SpecTitle + "</b></a></p>\n");
						try
						{
							writer.Write(pspec._contents);
						}
						catch (Exception ex)
						{
							writer.Write("<font color=red>" + Common.GetExceptionDetail(ex,"<br>") + "</font>");
						}
					}

					if(Common.AppConfigBool("RatingsEnabled"))
					{
						writer.Write(Ratings.Display(thisCustomer,ProductID,CategoryID,_siteID));
					}
				}
				else
				{
					Response.Write("This product has no variants! All products must have at least one variant, even if it is mostly empty, and just has a price. Please refer to the manual.");
				}

			}
		}

		void WriteVariantBar(System.Web.UI.HtmlTextWriter writer, Customer thisCustomer, IDataReader rs, int VariantID, int ProductID, int SiteID)
		{
			String PName = String.Empty;
			if(Common.AppConfigBool("ShowFullNameInTableExpanded"))
			{
				if(DB.RSField(rs,"VariantName").Length == 0)
				{
					PName = DB.RSField(rs,"Name");
				}
				else
				{
					PName = DB.RSField(rs,"Name") + " - " + DB.RSField(rs,"VariantName");
				}
			}
			else
			{
				if(DB.RSField(rs,"VariantName").Length == 0)
				{
					PName = DB.RSField(rs,"Name");
				}
				else
				{
					PName = DB.RSField(rs,"VariantName");
				}
			}

			writer.Write("			<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" bgcolor=\"#FFFFFF\" >\n");
			writer.Write("			<tr>\n");
			writer.Write("				<td colspan=\"4\" align=\"left\" valign=\"middle\" height=\"20\" class=\"DarkCell\">\n");
			writer.Write("					&nbsp;<img src=\"skins/skin_" + _siteID.ToString() + "/images/whitearrow.gif\" align=\"absmiddle\">&nbsp;<font style=\"font-size: 15px; font-weight:bold; color:white;\">" + PName + "</font></a>\n");
			writer.Write("				</td>\n");
			writer.Write("			</tr>\n");
			writer.Write("			<tr>\n");
			writer.Write("				<td width=\"2%\" class=\"GreyCell\"><img src=\"/images/spacer.gif\" width=\"5\" height=\"1\"></td>\n");
			writer.Write("				<td width=\"30%\" align=\"center\" valign=\"top\" class=\"GreyCell\">\n");
			String ImgUrl = Common.LookupImage("Variant",DB.RSFieldInt(rs,"VariantID"),"medium",_siteID);
			if(ImgUrl.Length != 0)
			{
				writer.Write("<img id=\"ProductPic" + ProductID.ToString() + "\" name=\"ProductPic" + ProductID.ToString() + "\" style=\"" + Common.AppConfig("ImageFrameStyle").Replace("cursor: hand;","") + "\" src=\"" + ImgUrl + "\">\n");
			}
			writer.Write("				</td>\n");
			writer.Write("				<td width=\"8%\" class=\"GreyCell\">\n");
			writer.Write("					<img src=\"/images/spacer.gif\" width=\"10\" height=\"1\">\n");
			writer.Write("				</td>\n");
			writer.Write("				<td width=\"60%\" valign=\"top\" align=\"left\" class=\"GreyCell\">\n");
			writer.Write("					<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" class=\"GreyCell\">\n");
			writer.Write("						<tr>\n");
			writer.Write("							<td width=\"30%\" align=\"left\" valign=\"top\">SKU:</td>\n");
			writer.Write("							<td width=\"70%\" align=\"left\" valign=\"top\">" + Common.MakeProperProductSKU(DB.RSField(rs,"SKU"),DB.RSField(rs,"SkuSuffix"),"","") + "</td>\n");
			writer.Write("						</tr>\n");
			if(DB.RSField(rs,"VariantDescription").Length != 0)
			{
				writer.Write("						<tr>\n");
				writer.Write("							<td width=\"30%\" align=\"left\" valign=\"top\">Description:</td>\n");
				writer.Write("							<td width=\"70%\" align=\"left\" valign=\"top\">" + DB.RSField(rs,"VariantDescription") + "</td>\n");
				writer.Write("						</tr>\n");
			}
			if(DB.RSField(rs,"Dimensions").Length != 0)
			{
				writer.Write("						<tr>\n");
				writer.Write("							<td width=\"30%\" align=\"left\" valign=\"top\">Dimensions:</td>\n");
				writer.Write("							<td width=\"70%\" align=\"left\" valign=\"top\">" + DB.RSField(rs,"Dimensions") + "</td>\n");
				writer.Write("						</tr>\n");
			}
			if(DB.RSFieldSingle(rs,"Weight") != 0.0F)
			{
				writer.Write("						<tr>\n");
				writer.Write("							<td width=\"30%\" align=\"left\" valign=\"top\">Weight:</td>\n");
				writer.Write("							<td width=\"70%\" align=\"left\" valign=\"top\">" + DB.RSFieldSingle(rs,"Weight").ToString() + "</td>\n");
				writer.Write("						</tr>\n");
			}
			int CustomerLevelID = thisCustomer._customerLevelID;
			if(!DB.RSFieldBool(rs,"HidePriceUntilCart"))
			{
				if(CustomerLevelID == 0)
				{
					// show consumer pricing (i.e. customer level 0):
					if(DB.RSFieldDecimal(rs,"SalePrice") != System.Decimal.Zero)
					{
						writer.Write("<tr>");
						writer.Write("<td width=\"30%\" align=\"left\" valign=\"top\"><strike><b>Price:</b></strike></td>");
						writer.Write("<td width=\"70%\" align=\"left\" valign=\"top\"><strike><b>" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</b></strike></td>");
						writer.Write("</tr>");
						writer.Write("<tr>");
						writer.Write("<td width=\"30%\" align=\"left\" valign=\"top\"><b><font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + DB.RSField(rs,"sDescription") + ":</font></b></td>");
						writer.Write("<td width=\"70%\" align=\"left\" valign=\"top\"><b><font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"SalePrice")) + "</font></b></td>");
						writer.Write("</tr>");
					}
					else
					{
						writer.Write("<tr>");
						writer.Write("<td width=\"30%\" align=\"left\" valign=\"top\"><b>Price:</b></td>");
						writer.Write("<td width=\"70%\" align=\"left\" valign=\"top\"><b>" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</b></td>");
						writer.Write("</tr>");
					}
				}
				else
				{
					// calculate and show "level" pricing:
					String CustomerLevelName = Common.GetCustomerLevelName(CustomerLevelID);
					bool IsOnSale = false;
					decimal LevelPrice = Common.DetermineLevelPrice(VariantID,CustomerLevelID, out IsOnSale);
					int Points = DB.RSFieldInt(rs,"Points");
					writer.Write("<tr>");
					writer.Write("<td width=\"30%\" align=\"left\" valign=\"top\"><b>Regular Price:</b></td>");
					writer.Write("<td width=\"70%\" align=\"left\" valign=\"top\"><b>" + Localization.CurrencyStringForDisplay( DB.RSFieldDecimal(rs,"Price")) + "</b></td>");
					writer.Write("</tr>");
					writer.Write("<tr>");
					writer.Write("<td width=\"30%\" align=\"left\" valign=\"top\"><b><font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + CustomerLevelName + " Price:</font></b></td>");
					writer.Write("<td width=\"70%\" align=\"left\" valign=\"top\"><b><font color=" + Common.AppConfig("OnSaleForTextColor") + ">" + Localization.CurrencyStringForDisplay( LevelPrice) + "</font></b>");
					writer.Write("&nbsp;" + Common.IIF(thisCustomer._customerLevelID !=0 && Common.AppConfigBool("MicroPay.ShowPointsWithPrices"), "(" + Points.ToString() + " Points)", "") + "&nbsp;");
					writer.Write("</td>");
					writer.Write("</tr>");
				}
			}
			if(Common.AppConfigBool("ShowInventoryTable"))
			{
				writer.Write("						<tr>\n");
				writer.Write("							<td width=\"30%\" align=\"left\" valign=\"top\">In Stock:</td>\n");
				writer.Write("							<td width=\"70%\" align=\"left\" valign=\"top\">");
				writer.Write(Common.GetInventoryTable(ProductID,VariantID,thisCustomer._isAdminUser));
				writer.Write("</td>\n");
				writer.Write("						</tr>\n");
			}
			writer.Write("						<tr>\n");
			//writer.Write("							<td width=\"30%\" align=\"left\" valign=\"top\"></td>\n");
			//writer.Write("							<td width=\"70%\" align=\"left\" valign=\"top\">" + Common.GetAddToCartForm(CategoryID,ProductID,DB.RSFieldInt(rs,"VariantID"),thisSite._siteID,2) + "</td>\n");
			writer.Write("							<td width=\"100%\" align=\"left\" colspan=2 valign=\"top\">" + Common.GetAddToCartForm(false,true,ProductID,DB.RSFieldInt(rs,"VariantID"),SiteID,2,false) + "</td>\n");
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
