// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for dyop.
	/// </summary>
	public class dyop : SkinBase
	{
		int CategoryID;
		int SectionID;
		String CategoryName;
		String SectionName;
		bool RequiresReg;
		int PackID;
		decimal PresetPackPrice;
		String PresetPackProducts;
		String ProductName;
		String ProductDescription;
		String FileDescription;
		String ProductPicture;
		String LargePic;
		bool HasLargePic;
		bool SpecsInline;
		String SpecTitle;
		decimal BasePrice;
		int Points;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)

			PackID = Common.QueryStringUSInt("PackID");
			
			
			if(PackID == 0)
			{
				Response.Redirect("default.aspx");
			}
			if(Common.ProductHasBeenDeleted(PackID))
			{
				Response.Redirect(SE.MakeDriverLink("ProductNotFound"));
			}

			CategoryID = Common.QueryStringUSInt("CategoryID");
			SectionID = Common.QueryStringUSInt("SectionID");
			if(CategoryID == 0 && SectionID == 0)
			{
				// no category or section passed in, pick first one that this product is mapped to:
				String tmpS = Common.GetProductCategories(PackID,true);
				if(tmpS.Length != 0)
				{
					String[] catIDs = tmpS.Split(',');
					CategoryID = Localization.ParseUSInt(catIDs[0]);
				}
				else
				{
					String tmpS2 = Common.GetProductSections(PackID,true);
					if(tmpS2.Length != 0)
					{
						String[] secIDs = tmpS2.Split(',');
						SectionID = Localization.ParseUSInt(secIDs[0]);
					}
				}
			}
			CategoryName = Common.GetCategoryName(CategoryID);
			SectionName = Common.GetSectionName(SectionID);

			IDataReader rs = DB.GetRS("select product.*,productvariant.price,productvariant.points,productvariant.saleprice from product  " + DB.GetNoLock() + ", productvariant " + DB.GetNoLock() + " where product.productid=productvariant.productid and productvariant.deleted=0 and productvariant.published=1 and product.ProductID=" + PackID.ToString());
			if(!rs.Read())
			{
				rs.Close();
				Response.Redirect("default.aspx");
			}

			SpecsInline = DB.RSFieldBool(rs,"SpecsInline");
			SpecTitle = DB.RSField(rs,"SpecTitle");
			Points = DB.RSFieldInt(rs,"Points");
			
			ProductName = DB.RSField(rs,"Name");
			ProductDescription = DB.RSField(rs,"Description");
			FileDescription = new ProductDescriptionFile(PackID,thisCustomer._localeSetting,_siteID)._contents;
			if(FileDescription.Length != 0)
			{
				ProductDescription += "<br>" + FileDescription;
			}
			ProductPicture = Common.LookupImage("Product",PackID,"medium",_siteID);
			LargePic = Common.LookupImage("Product",PackID,"large",_siteID);
			HasLargePic = (LargePic.Length != 0);
			if(ProductPicture.Length == 0)
			{
				ProductPicture = Common.LookupImage("Product",PackID,"icon",_siteID);
			}
			if(ProductPicture.Length == 0)
			{
				ProductPicture = Common.AppConfig("NoPicture");
			}
			RequiresReg = DB.RSFieldBool(rs,"RequiresRegistration");
			
			BasePrice = System.Decimal.Zero;
			if(DB.RSFieldDecimal(rs,"SalePrice") != System.Decimal.Zero)
			{
				BasePrice = DB.RSFieldDecimal(rs,"SalePrice");
			}
			else
			{
				BasePrice = DB.RSFieldDecimal(rs,"Price");
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
			if(RequiresReg && thisCustomer._isAnon)
			{
				writer.Write("<br><br><br><br><b>You must be a registered user to view this product!</b><br><br><br><a href=\"signin.aspx?returnurl=showproduct.aspx?" + Server.HtmlEncode(Server.UrlEncode(Common.ServerVariables("QUERY_STRING"))) + "\">Click Here</a> to sign-in.");
			}
			else
			{
				PresetPackPrice = System.Decimal.Zero;
				PresetPackProducts = String.Empty;
				Common.PresetPack(thisCustomer, PackID, false, out PresetPackPrice, out PresetPackProducts);
				
				int ShowProductID = Common.QueryStringUSInt("ProductID");
				int ShowCategoryID = Common.QueryStringUSInt("ShowCategoryID");

				int CustomerLevelID = thisCustomer._customerLevelID;
				bool CustomerLevelAllowsQuantityDiscounts = Common.CustomerLevelAllowsQuantityDiscounts(CustomerLevelID);
				String CustomerLevelName = Common.GetCustomerLevelName(CustomerLevelID);
			
				int PackSize = Common.GetPackSize(PackID);
				String PackSizePrompt = PackSize.ToString();
				String PackTypePrompt = Common.GetProductName(PackID);
				if(!PackTypePrompt.EndsWith("Pack"))
				{
					PackTypePrompt += " Pack";
				}
				String PackTypeImage = Common.LookupImage("product",PackID,"medium",_siteID);
				if(PackTypeImage.Length == 0)
				{
					PackTypeImage = Common.LookupImage("Product",PackID,"icon",_siteID);
				}
				if(PackTypeImage.Length == 0)
				{
					PackTypeImage = Common.AppConfig("NoPicture");
				}

				Common.LogEvent(thisCustomer._customerID,10, PackID.ToString());
				String LargeImage = String.Empty;
				if(HasLargePic)
				{
					LargeImage = LargePic;
					int LargePicW = Common.GetImageWidth(LargeImage);
					int LargePicH = Common.GetImageHeight(LargeImage);
					writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
					writer.Write("function popup(url)\n");
					writer.Write("	{\n");
					writer.Write("	window.open('popup.aspx?src=' + url,'LargerImage" + PackID.ToString() + Common.GetRandomNumber(1,100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no,copyhistory=no,width=" + LargePicW.ToString() + ",height=" + LargePicH.ToString() + ",left=0,top=0');\n");
					writer.Write("	return (true);\n");
					writer.Write("	}\n");
					writer.Write("</script>\n");
				}

				ProductSpecFile pspec = new ProductSpecFile(PackID,thisCustomer._localeSetting,_siteID);
				bool ProductHasSpecs = (pspec._contents.Length != 0);
				IDataReader rs = DB.GetRS("Select SpecsInline,SpecTitle from product where ProductID=" + PackID.ToString());
				rs.Read();
				bool SpecsInline = DB.RSFieldBool(rs,"SpecsInline");
				String SpecTitle = DB.RSField(rs,"SpecTitle");
				rs.Close();
				String SpecLink = Common.IIF(SpecsInline , "#Specs" , pspec._url);
				if(SpecTitle.Length == 0)
				{
					SpecTitle = Common.AppConfig("DefaultSpecTitle");
				}

				int ActiveDIDID = Common.LookupActiveProductQuantityDiscountID(PackID);
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

				CustomCart cart = new CustomCart(thisCustomer._customerID,PackID,_siteID);
				int NumItemsInPack = CustomCart.NumItems(thisCustomer._customerID,PackID);

				writer.Write("<form method=\"GET\" action=\"addtocart.aspx\" onSubmit=\"\">\n");
				writer.Write("<input type=\"hidden\" name=\"ProductID\" value=\"" + PackID.ToString() + "\">\n");
				writer.Write("<input type=\"hidden\" name=\"VariantID\" value=\"" + Common.GetProductsFirstVariantID(PackID).ToString() + "\">\n");
				writer.Write("<input type=\"hidden\" name=\"ReturnURL\" value=\"" + Common.GetThisPageName(false) + "?" + Common.ServerVariables("QUERY_STRING") + "\">\n");
			
				writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"4\" width=\"100%\">");
				writer.Write("<tr>");
				writer.Write("<td align=\"center\" valign=\"top\" width=\"40%\">");
				String ProdPic = Common.LookupImage("Product",PackID,"medium",_siteID);
				if(ProdPic.Length == 0)
				{
					ProdPic = Common.LookupImage("Product",PackID,"icon",_siteID);
				}
				if(ProdPic.Length == 0)
				{
					ProdPic = Common.AppConfig("NoPicture");
				}

				if(HasLargePic) 
				{
					writer.Write("<img style=\"cursor:hand;\" onClick=\"popup('" + Server.UrlEncode(LargePic) + "')\" alt=\"Click here to view larger image\" style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + ProdPic + "\">");
				}
				else
				{
					writer.Write("<img style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + ProdPic + "\">");
				}
				writer.Write("<br>");
				writer.Write("");
				if(HasLargePic) 
				{
					writer.Write("<a href=\"javascript:void(0);\" onClick=\"javascript:popup('" + Server.UrlEncode(LargePic) + "');\">View Larger Image</a><br>");
				}
				writer.Write("</td>");
				writer.Write("<td align=\"left\" valign=\"top\">");

				writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
				writer.Write("<tr><td align=\"right\">");
				int NumProducts = 0;
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
					int PreviousProductID = Common.GetPreviousProduct(PackID,CategoryID,SectionID,0,0,false,true,true);
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
					int NextProductID = Common.GetNextProduct(PackID,CategoryID,SectionID,0,0,false,true,true);
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
				writer.Write("<tr valign=\"top\"><td>");
				writer.Write("<font class=\"ProductNameText\">");
				writer.Write(ProductName);
				writer.Write("</font>&nbsp;&nbsp;");
				if(ProductHasSpecs) // don't put specs inline for a pack
				{
					writer.Write("&nbsp;&nbsp;<small>(<a href=\"" + SpecLink + "\" " + Common.IIF(SpecsInline , "" , "target=\"_blank\"") + ">" + SpecTitle + "</a>)</small>");
				}
				writer.Write("<br>");
				if(ActiveDID)
				{
					String S1 = String.Empty;
					writer.Write("<br><small>This product qualifies for quantity discount pricing. (<a href=\"javascript:void(0);\" style=\"cursor: normal;\" onMouseover=\"ddrivetip(" + DB.SQuote(Common.AppConfig("quantitydiscount") + "<br><br>" + Common.GetQuantityDiscountDisplayTable(ActiveDIDID)) + ",'" + Common.AppConfig("LightCellColor") + "', 300)\" onMouseout=\"hideddrivetip()\">what's this?</a>)</small>");
				}
				if(Common.AppConfigBool("ShowEMailProductToFriend"))
				{
					String S1 = String.Empty;
					writer.Write("<br><small><img src=\"skins/skin_" + _siteID.ToString() + "/images/mailicon.gif\" border=\"0\" align=\"absmiddle\">&nbsp;<a href=\"emailproduct.aspx?productid=" + PackID.ToString() + "&categoryid=" + CategoryID.ToString() + "&sectionid=" + SectionID.ToString() + "\">E-mail this product to a friend</a></small><br><br>");
				}
				writer.Write("<div align=\"left\">");
				writer.Write(ProductDescription);
				writer.Write("<br>&nbsp;<div align=\"left\">"); 

				if(PackSize == 0)
				{
					decimal BaseDisplayPrice = BasePrice;
					if(PresetPackPrice != System.Decimal.Zero)
					{
						BaseDisplayPrice += PresetPackPrice;
					}
					writer.Write("<b>Base " + PackTypePrompt + " Price: " + Localization.CurrencyStringForDisplay(BaseDisplayPrice) + "</b> " + Common.IIF(thisCustomer._customerLevelID != 0 && Common.AppConfigBool("MicroPay.ShowPointsWithPrices"), "(" + Points.ToString() + " Points)", "") + "<br>");
					decimal PackPriceDelta = Common.PackPriceDelta(thisCustomer._customerID,thisCustomer._customerLevelID,PackID,0);
					writer.Write("<b>Final " + PackTypePrompt + " Price: " + Localization.CurrencyStringForDisplay(BasePrice+PackPriceDelta) + "</b><br>");
				}
				else
				{
					writer.Write("<b>" + PackTypePrompt + " Price: " + Localization.CurrencyStringForDisplay(BasePrice) + "</b> " + Common.IIF(thisCustomer._customerLevelID != 0 && Common.AppConfigBool("MicroPay.ShowPointsWithPrices"), "(" + Points.ToString() + " Points)", "") + "<br>");
				}

				writer.Write("</div>");
				writer.Write("</td></tr>");
				writer.Write("<tr valign=\"top\"><td height=\"10\"></td></tr>");
				writer.Write("<tr valign=\"top\"><td height=\"20\"><hr size=\"1\" color=\"#666666\"></td></tr>");

				writer.Write("<tr><td>");
				if(PackSize == 0)
				{
					writer.Write("Mix and match any items (any products, colors or sizes) into a single pack. Use the style browser below to choose products and add them to your " + PackTypePrompt.ToLower() + ". When you have at least one item in to your " + PackTypePrompt.ToLower() + ", you can then add the " + PackTypePrompt.ToLower() + " to your " + Common.AppConfig("CartPrompt").ToLower() + " and checkout.<br><br>\n");
				}
				else
				{
					writer.Write("Mix and match any <b>" + PackSizePrompt.ToLower() + "</b> items (any products, colors or sizes) into a single pack. Use the style browser below to choose products and add them to your " + PackTypePrompt.ToLower() + ". Your " + PackTypePrompt.ToLower() + " must contain " + PackSizePrompt.ToLower() + " products. When you have " + PackSizePrompt.ToLower() + " products in to your " + PackTypePrompt.ToLower() + ", you can then add the " + PackTypePrompt.ToLower() + " to your " + Common.AppConfig("CartPrompt").ToLower() + " and checkout.<br><br>\n");
				}

				// ###############################
			
				writer.Write("        <table cellSpacing=\"1\" width=\"100%\" cellpadding=\"0\" border=\"0\">\n");
				writer.Write("          <tbody>\n");
				writer.Write("            <tr>\n");
				if(NumItemsInPack > 0 && (PackSize==0 || NumItemsInPack == PackSize))
				{
					writer.Write("              <td colspan=\"5\"><b>Your " + PackTypePrompt.ToLower() + " is full and ready for purchase!</b></td>\n");
				}
				else
				{
					if(NumItemsInPack == 0)
					{
						writer.Write("              <td colspan=\"5\"><b>Your " + PackTypePrompt.ToLower() + " is currently empty. Use the style browser to add items.</b></td>\n");
					}
					else
					{
						writer.Write("              <td colspan=\"5\"><b>Your " + PackTypePrompt.ToLower() + " currently contains the following <span id=\"NumItemsInPack\">" + NumItemsInPack.ToString() + "</span> items:</b></td>\n");
					}
				}
				writer.Write("            </tr>\n");
				writer.Write("            <tr>\n");
				writer.Write("              <td width=\"60%\" height=\"18\" class=\"DarkCell\"><font class=\"dyop_hdr\">&nbsp;Item</font></b></td>\n");
				writer.Write("              <td width=\"10%\" height=\"18\" class=\"DarkCell\" align=\"center\"><font class=\"dyop_hdr\">" + Common.AppConfig("ColorOptionPrompt") + "</font></b></td>\n");
				writer.Write("              <td width=\"10%\" height=\"18\" class=\"DarkCell\" align=\"center\"><font class=\"dyop_hdr\">" + Common.AppConfig("SizeOptionPrompt") + "</font></b></td>\n");
				writer.Write("              <td width=\"10%\" height=\"18\" class=\"DarkCell\" align=\"center\"><font class=\"dyop_hdr\">Quantity</font></b></td>\n");
				writer.Write("              <td width=\"10%\" height=\"18\" class=\"DarkCell\" align=\"center\"><font class=\"dyop_hdr\">&nbsp;</font></b></td>\n");
				writer.Write("            </tr>\n");

				String tmpP = "," + PresetPackProducts + ",";
				for(int i = 0; i < cart._cartItems.Count; i++)
				{
					bool ProductIsPreset = (tmpP.IndexOf("," + ((CustomItem)cart._cartItems[i]).productID.ToString() + ",") != -1);
					if(i > 0)
					{
						writer.Write("            <tr>\n");
						writer.Write("              <td colSpan=\"5\" height=\"5\" valign=\"middle\"><img src=\"skins/skin_" + _siteID.ToString() + "/images/spacer_pack.gif\" height=\"1\" width=\"100%\"></td>\n");
						writer.Write("            </tr>\n");
						writer.Write("            <tr>\n");
					}
					writer.Write("              <td valign=\"middle\" align=\"left\"><font class=\"dyop_sm\">&nbsp;<a href=\"javascript:void(0);\" onClick=\"javascript:document.frames['sb'].location='sb.aspx?type=" + Common.QueryString("type") + "&packid=" + PackID.ToString() + "&showproductid=" + ((CustomItem)cart._cartItems[i]).productID.ToString() + "&categoryid=" + CategoryID.ToString() + "&sectionid=" + SectionID.ToString() + "&isfull=" + (PackSize != 0 && NumItemsInPack >= PackSize).ToString() + "';\">" + ((CustomItem)cart._cartItems[i]).productName + "</a></font></td>\n");
					writer.Write("              <td valign=\"middle\" align=\"center\"><font class=\"dyop_sm\">" + ((CustomItem)cart._cartItems[i]).chosenColor + "</font></td>\n");
					writer.Write("              <td valign=\"middle\" align=\"center\"><font class=\"dyop_sm\">" + ((CustomItem)cart._cartItems[i]).chosenSize + "</font></td>\n");
					String QString = ((CustomItem)cart._cartItems[i]).quantity.ToString();
					writer.Write("              <td valign=\"middle\" align=\"right\"><font class=\"dyop_sm\">" + QString + "&nbsp;");
					if(ProductIsPreset)
					{
						writer.Write("&nbsp;");
					}
					else
					{
						writer.Write("<map name=\"FPMapUpDown_" + ((CustomItem)cart._cartItems[i]).CustomCartRecordID.ToString() + "\">\n");
						if(PackSize != 0 && NumItemsInPack > PackSize)
						{
							writer.Write("<area alt=\"Your " + PackTypePrompt.ToLower() + " is already full\" href=\"javascript:void(0);\" onClick=\"alert('Your " + PackTypePrompt.ToLower() + " already contains " + PackSize.ToString() + " items. Please decrease other item quantities first, if you want to add more of this item');\" shape=\"rect\" coords=\"0, 0, 15, 8\">\n");
						}
						else
						{
							writer.Write("<area alt=\"Increase Quantity\" href=\"dyop_quan.aspx?type=" + Common.QueryString("type") + "&packid=" + PackID.ToString() + "&productid=" + ShowProductID.ToString() + "&categoryid=" + ShowCategoryID.ToString() + "&sectionid=" + SectionID.ToString() + "&cartid=" + ((CustomItem)cart._cartItems[i]).CustomCartRecordID.ToString() + "&quan=1\" shape=\"rect\" coords=\"0, 0, 15, 8\">\n");
						}
						writer.Write("<area alt=\"Decrease Quantity\" href=\"dyop_quan.aspx?type=" + Common.QueryString("type") + "&packid=" + PackID.ToString() + "&productid=" + ShowProductID.ToString() + "&categoryid=" + ShowCategoryID.ToString() + "&sectionid=" + SectionID.ToString() + "&cartid=" + ((CustomItem)cart._cartItems[i]).CustomCartRecordID.ToString() + "&quan=-1\" shape=\"rect\" coords=\"0, 9, 15, 15\">\n");
						writer.Write("</map><img align=\"absmiddle\" border=\"0\" src=\"skins/skin_" + _siteID.ToString() + "/images/updown.gif\" usemap=\"#FPMapUpDown_" + ((CustomItem)cart._cartItems[i]).CustomCartRecordID.ToString() + "\" width=\"15\" height=\"15\">\n");
						writer.Write("&nbsp;\n");
						writer.Write("</font>\n");
					}
					writer.Write("</td>");
					writer.Write("              <td valign=\"middle\" align=\"center\">");
					if(ProductIsPreset)
					{
						writer.Write("Preset");
					}
					else
					{
						writer.Write("<img style=\"cursor: hand;\" onclick=\"if(confirm('Remove this item from your " + PackTypePrompt.ToLower() + "?')) self.location='dyop_delete.aspx?type=" + Common.QueryString("type") + "&packid=" + PackID.ToString() + "&productid=" + ShowProductID.ToString() + "&categoryid=" + ShowCategoryID.ToString() + "&sectionid=" + SectionID.ToString() + "&deleteid=" + ((CustomItem)cart._cartItems[i]).CustomCartRecordID.ToString() + "';\" height=\"16\" alt=\"Delete this item from your " + PackTypePrompt.ToLower() + "\" src=\"skins/skin_" + _siteID.ToString() + "/images/delete.gif\" width=\"32\" align=\"absMiddle\" border=\"0\">");
					}
					writer.Write("</td>\n");
					writer.Write("            </tr>\n");
				}

				writer.Write("             <tr>\n");
				writer.Write("              <td colSpan=\"5\" height=\"5\" valign=\"middle\"><img src=\"skins/skin_" + _siteID.ToString() + "/images/spacer_pack_double.gif\" height=\"3\" width=\"100%\"></td>\n");
				writer.Write("            </tr>\n");
				writer.Write("           <tr>\n");
				writer.Write("              <td valign=\"middle\" align=\"left\"><font class=\"dyop_sm\">&nbsp;</font></td>\n");
				writer.Write("              <td valign=\"middle\" align=\"center\"><font class=\"dyop_sm\"></font></td>\n");
				writer.Write("              <td valign=\"middle\" align=\"center\"><font class=\"dyop_sm\"><b>Total</b></font></td>\n");
				writer.Write("              <td valign=\"middle\" align=\"center\" ><font class=\"dyop_sm\"><b>" + NumItemsInPack.ToString() + "</b></font></td>\n");
				writer.Write("              <td></td>\n");
				writer.Write("            </tr>\n");
				writer.Write("          </tbody>\n");
				writer.Write("        </table>\n");

				if(PackSize == 0)
				{
					if(NumItemsInPack > 0)
					{
						writer.Write("<div align=\"right\"><input type=\"submit\" class=\"ReadyToPurchaseButton\" value=\"Your Pack is Ready For Purchase\" id=\"PurchasePack\" name=\"PurchasePack\"></div>");
					}
					else
					{
						writer.Write("<b>When you have added items to your " + PackTypePrompt.ToLower() + ", you will be able to add it to your " + Common.AppConfig("CartPrompt").ToLower() + "</b>");
					}
				}
				else
				{
					if(NumItemsInPack < PackSize)
					{
						writer.Write("<b>When your " + PackTypePrompt.ToLower() + " is full, you will be able to add it to your " + Common.AppConfig("CartPrompt").ToLower() + "</b>");
					}
					else if(NumItemsInPack > PackSize)
					{
						writer.Write("<font color=red><b>Your " + PackTypePrompt.ToLower() + " contains more than " + PackSizePrompt + " items, please remove some items</b></font>");
					}
					else
					{
						writer.Write("<div align=\"right\"><input type=\"submit\" class=\"ReadyToPurchaseButton\" value=\"Your Pack Is Ready To Purchase\" id=\"PurchasePack\" name=\"PurchasePack\"></div>");
					}
				}

				// ###############################

				writer.Write("</td>\n");
				writer.Write("</tr>\n");
				writer.Write("</table>\n");

				writer.Write("</table>");

				writer.Write("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");

				writer.Write("    <tr>\n");
				writer.Write("      <td width=\"100%\" colspan=\"2\">\n");
				writer.Write("      <table style=\"BORDER-RIGHT: #444444 0px solid; BORDER-TOP: #444444 0px solid; BORDER-LEFT: #444444 0px solid; BORDER-BOTTOM: #444444 0px solid\" cellPadding=\"2\" width=\"100%\" border=\"0\" cellspacing?0?>\n");
				writer.Write("        <tbody>\n");
				writer.Write("          <tr>\n");
				writer.Write("            <td vAlign=\"top\" align=\"left\"><img src=\"skins/Skin_" + _siteID.ToString() + "/images/stylebrowser.gif\" border=\"0\"><br>\n");
				writer.Write("              <table style=\"BORDER-RIGHT: #444444 1px solid; BORDER-TOP: #444444 1px solid; BORDER-LEFT: #444444 1px solid; BORDER-BOTTOM: #444444 1px solid\" cellSpacing=\"0\" cellPadding=\"4\" width=\"100%\" border=\"0\">\n");
				writer.Write("                <tbody>\n");
				writer.Write("                  <tr>\n");
				writer.Write("                    <td vAlign=\"top\" align=\"left\">\n");

				// style browser:
				int PBH = Common.AppConfigUSInt("ProductBrowserHeight");
				if(PBH == 0)
				{
					PBH = 500; // pixels
				}
				writer.Write("<iframe height=\"" + PBH.ToString() + "\" id=\"sb\" name=\"sb\" src=\"sb.aspx?packid=" + PackID.ToString() + "&productid=" + ShowProductID.ToString() + "&categoryid=" + ShowCategoryID.ToString() + "&sectionid=" + SectionID.ToString() + "&isfull=" + (PackSize != 0 && NumItemsInPack >= PackSize).ToString() + "\" scrolling=\"no\" marginwidth=\"0\" marginheight=\"0\" frameborder=\"0\" vspace=\"0\" hspace=\"0\" style=\"width:100%; display:block\" ></iframe>\n");

				writer.Write("                    </td>\n");
				writer.Write("                  </tr>\n");
				writer.Write("                </tbody>\n");
				writer.Write("              </table>\n");
				writer.Write("            </td>\n");
				writer.Write("          </tr>\n");
				writer.Write("        </tbody>\n");
				writer.Write("      </table>\n");
				writer.Write("      &nbsp;\n");
				writer.Write("      </td>\n");
				writer.Write("    </tr>\n");
				writer.Write("  </table>\n");
				writer.Write("</div>\n");
				writer.Write("</form>\n");
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
