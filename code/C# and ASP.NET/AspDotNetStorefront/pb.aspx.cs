using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for pb.
	/// </summary>
	public class pb : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");
			
			Customer thisCustomer = new Customer();
			
			Response.Write("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0//EN\" \"http://www.w3.org/TR/REC-html40/strict.dtd\">\n");
			Response.Write("<html>\n");
			Response.Write("<head>\n");
			Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1252\">\n");
			Response.Write("<title>Product Browser</title>\n");
			Response.Write("<link rel=\"stylesheet\" href=\"skins/Skin_" + thisCustomer._skinID.ToString() + "/style.css\" type=\"text/css\">\n");
			Response.Write("<script type=\"text/javascript\" src=\"formValidate.js\"></script>\n");
			Response.Write("</head>\n");
			Response.Write("<body style=\"margin: 0px;\" bottommargin=\"0\" leftmargin=\"0\" marginheight=\"0\" marginwidth=\"0\" rightmargin=\"0\" topmargin=\"0\" bgcolor=\"#FFFFFF\" %ONBLUR% onLoad=\"self.focus()\">\n");
			Response.Write("<!-- PAGE INVOCATION: '%INVOCATION%' -->\n");

			int PackID = Common.QueryStringUSInt("PackID");
			int ProductID = Common.QueryStringUSInt("ProductID");
			int CategoryID = Common.QueryStringUSInt("CategoryID");
			int SectionID = Common.QueryStringUSInt("SectionID");

			if(ProductID != 0)
			{
				if(CategoryID == 0)
				{
					try 
					{
						CategoryID = Localization.ParseUSInt(Common.GetProductCategories(PackID,true).Split(',')[0]);
					}
					catch
					{}
				}
			}
			else
			{
				if(CategoryID == 0)
				{
					IDataReader rsc = DB.GetRS("Select top 1 CategoryID from category  " + DB.GetNoLock() + " where deleted=0 and published=1 and categoryid in (select distinct(categoryid) from productcategory  " + DB.GetNoLock() + " where productid in (select productid from product  " + DB.GetNoLock() + " where IsAPack=0 and IsAKit=0 and ShowInProductBrowser=1 and deleted=0 and published=1)) and ShowInProductBrowser=1 order by DisplayOrder");
					if(rsc.Read())
					{
						CategoryID = DB.RSFieldInt(rsc,"CategoryID");
					}
					rsc.Close();
				}
				ProductID = Common.GetFirstProduct(CategoryID,false,false);
			}

			if(Common.ProductHasBeenDeleted(ProductID))
			{
				Response.Redirect(SE.MakeDriverLink("ProductNotFound"));
			}

			IDataReader rs = DB.GetRS("select * from product  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString());
			if(!rs.Read())
			{
				rs.Close();
				Response.Redirect(SE.MakeDriverLink("ProductNotFound"));
			}
			bool RequiresReg = DB.RSFieldBool(rs,"RequiresRegistration");
			int ProductDisplayFormatID = DB.RSFieldInt(rs,"ProductDisplayFormatID");
			String ProductName = DB.RSField(rs,"Name");
			String ProductDescription = DB.RSField(rs,"Summary"); //.Replace("\n","<br>");
			String FileDescription = new ProductDescriptionFile(ProductID,thisCustomer._localeSetting,thisCustomer._skinID)._contents;
			if(FileDescription.Length != 0)
			{
				ProductDescription += "<br>" + FileDescription;
			}
			String ProductPicture = Common.LookupImage("Product",ProductID,"medium",thisCustomer._skinID);
			bool HasLargePic = (Common.LookupImage("Product",ProductID,"large",thisCustomer._skinID).Length != 0);
			if(ProductPicture.Length == 0)
			{
				ProductPicture = Common.LookupImage("Product",ProductID,"icon",thisCustomer._skinID);
			}
			if(ProductPicture.Length == 0)
			{
				ProductPicture = Common.AppConfig("NoPicture");
			}
			rs.Close();

			String CategoryName = Common.GetCategoryName(CategoryID);

			if(RequiresReg && thisCustomer._isAnon)
			{
				Response.Write("<b>You must be a registered user to view this product!</b>");
			}
			else
			{
				DB.ExecuteSQL("update product set Looks=Looks+1 where ProductID=" + ProductID.ToString());
				String sql = "SELECT SalesPrompt.name as SDescription, Product.SwatchImageMap, Product.ProductID, Product.IsCallToOrder, Product.HidePriceUntilCart, Product.ProductTypeID, Product.RelatedProducts, Product.Name, Product.SEName, Product.SpecTitle, Product.SpecsInline, Product.SpecCall, Product.ProductDisplayFormatID, Product.ColWidth, Product.Summary, Product.Description, Product.SEKeywords, Product.SEDescription, Product.SKU, Product.ManufacturerID, Product.ManufacturerPartNumber, Product.Published, Product.Deleted, ProductVariant.VariantID, ProductVariant.Name AS VariantName, ProductVariant.Colors, ProductVariant.Sizes, ProductVariant.Dimensions, ProductVariant.Weight, ProductVariant.Inventory, ProductVariant.SKUSuffix, ProductVariant.ManufacturerPartNumber AS VManufacturerPartNumber, ProductVariant.Price, ProductVariant.SalePrice, ProductVariant.Deleted AS VariantDeleted, ProductVariant.Description as VariantDescription, ProductVariant.Published AS VariantPublished, Manufacturer.Name AS ManufacturerName, Manufacturer.SEName as ManufacturerSEName FROM  ((Product  " + DB.GetNoLock() + " INNER JOIN Manufacturer  " + DB.GetNoLock() + " ON Product.ManufacturerID = Manufacturer.ManufacturerID) left outer join salesprompt  " + DB.GetNoLock() + " on product.salespromptid=salesprompt.salespromptid) LEFT OUTER JOIN ProductVariant  " + DB.GetNoLock() + " ON Product.ProductID = ProductVariant.ProductID WHERE product.published=1 and Product.Deleted=0 and product.productid=" + ProductID.ToString() + " and product.isapack<>1 and product.isakit<>1 AND (ProductVariant.Published=1 or ProductVariant.Published IS NULL) AND (ProductVariant.Deleted=0 or ProductVariant.Deleted IS NULL) ORDER by ProductVariant.DisplayOrder, ProductVariant.Name";
				rs = DB.GetRS(sql);
				bool empty = true;
				String RelatedProducts = String.Empty;
				if(rs.Read())
				{
					empty = false;
					RelatedProducts = DB.RSField(rs,"RelatedProducts");
				}

				String LargeImage = String.Empty;
				int LargePicW = 0;
				int LargePicH = 0;
				if(HasLargePic)
				{
					LargeImage = Common.LookupImage("Product",ProductID,"large",thisCustomer._skinID);
					LargePicW = Common.GetImageWidth(LargeImage);
					LargePicH = Common.GetImageHeight(LargeImage);
				}
				Response.Write(Common.GetJSPopupRoutines());

				ProductSpecFile pspec = new ProductSpecFile(PackID,thisCustomer._localeSetting,thisCustomer._skinID);
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
				int NumProductsInCategory = 0;
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
					Response.Write(Common.ReadFile("tip2.js",false));
				}

				// setup multi-image gallery:
				String SwatchPic = Common.LookupImage("Product",ProductID,"swatch",thisCustomer._skinID);
				String SwatchImageMap = String.Empty;
				if(!empty)
				{
					SwatchImageMap = DB.RSField(rs,"SwatchImageMap");
				}

				ProductImageGallery ImgGal = null;
				String ImgGalCacheName = "ImgGal_" + ProductID.ToString() + "_" + thisCustomer._skinID.ToString() + "_" + thisCustomer._localeSetting;
				if(Common.AppConfigBool("CacheMenus"))
				{
					ImgGal = (ProductImageGallery)HttpContext.Current.Cache.Get(ImgGalCacheName);
				}
				if(ImgGal == null)
				{
					ImgGal = new ProductImageGallery(ProductID,thisCustomer._skinID,thisCustomer._localeSetting);
				}
				if(Common.AppConfigBool("CacheMenus"))
				{
					HttpContext.Current.Cache.Insert(ImgGalCacheName,ImgGal,null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
				}
				Response.Write(ImgGal._imgDHTML);

				// RIGHT BAR FORMAT:
				Response.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"4\" width=\"100%\">");
				Response.Write("<tr>");
				Response.Write("<td align=\"center\" valign=\"top\" width=\"40%\">");
				ProdPic = Common.LookupImage("Product",ProductID,"medium",thisCustomer._skinID);
				if(ProdPic.Length == 0)
				{
					ProdPic = Common.LookupImage("Product",ProductID,"icon",thisCustomer._skinID);
				}
				if(ProdPic.Length == 0)
				{
					ProdPic = Common.AppConfig("NoPicture");
				}
				int ProdPicW = 0;
				if(ProdPic.Length != 0)
				{
					ProdPicW = Common.GetImageWidth(ProdPic);
				}
				if(ProdPicW > 300)
				{
					ProdPicW = 300;
				}
				if(HasLargePic) 
				{
					Response.Write("<img id=\"ProductPic" + ProductID.ToString() + "\" name=\"ProductPic" + ProductID.ToString() + "\" width=\"" + ProdPicW.ToString() + "\" style=\"cursor:hand;\" onClick=\"popupwh('" + DB.RSField(rs,"Name").Replace("'","").Replace("\"","") + " Large Pic','" + Common.LookupImage("Product",ProductID,"large",thisCustomer._skinID) + "'," + LargePicW.ToString() + "," + (LargePicH+50).ToString() + ")\" alt=\"Click here to view larger image\" style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + ProdPic + "\">");
				}
				else
				{
					Response.Write("<img id=\"ProductPic" + ProductID.ToString() + "\" name=\"ProductPic" + ProductID.ToString() + "\" width=\"" + ProdPicW.ToString() + "\" style=\"" + Common.AppConfig("ImageFrameStyle").Replace("cursor: hand","cursor: normal") + "\" src=\"" + ProdPic + "\">");
				}
				if(ImgGal._imgGalIcons.Length != 0)
				{
					Response.Write("<br><br>");
					Response.Write(ImgGal._imgGalIcons);
					if(SwatchPic.Length != 0)
					{
						Response.Write(SwatchImageMap);
						Response.Write("<br><img src=\"images/spacer.gif\" width=\"1\" height=\"4\"><br><img style=\"cursor: hand;\" src=\"" + SwatchPic + "\" usemap=\"#SwatchMap\" border=\"0\">");
					}
				}
				if(HasLargePic) 
				{
					Response.Write("<br><img src=\"images/spacer.gif\" width=\"1\" height=\"4\"><br>");
					Response.Write("<a href=\"javascript:void(0);\" onClick=\"javascript:popup('" + DB.RSField(rs,"Name").Replace("'","").Replace("\"","") + " Large Pic','" + Common.LookupImage("Product",ProductID,"large",thisCustomer._skinID) + "'," + LargePicW.ToString() + "," + (LargePicH+50).ToString() + ");\"><img alt=\"Click here to view larger image\" src=\"skins/skin_" + thisCustomer._skinID.ToString() + "/images/showlarger.gif\" border=\"0\" align=\"absmiddle\"></a>");
				}

				Response.Write("</td>");
				Response.Write("<td align=\"left\" valign=\"top\">");

				Response.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
				Response.Write("<tr valign=\"top\"><td>");

				Response.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
				Response.Write("<tr><td valign=\"middle\" align=\"left\">");
				Response.Write("<font class=\"ProductNameText\">" + DB.RSField(rs,"Name") + "</font>");
				Response.Write("</td><td valign=\"middle\" align=\"right\">");

				NumProductsInCategory = Common.GetNumCategoryProducts(CategoryID,false,false);
				if(NumProductsInCategory > 1)
				{
					int PreviousProductID = Common.GetPreviousProduct(ProductID,CategoryID,0,0,0,false,false,false);
					Response.Write("<a class=\"ProductNavLink\" href=\"pb.aspx?type=" + Common.QueryString("type") + "&packid=" + PackID.ToString() + "&productid=" + PreviousProductID.ToString() + "&categoryid=" + CategoryID.ToString()+ "&sectionid=" + SectionID.ToString() + "&isfull=" + Common.QueryString("isfull") + "\"><img align=\"absmiddle\" src=\"skins/skin_" + thisCustomer._skinID.ToString() + "/images/previous.gif\" border=\"0\" alt=\"previous product in this style\"></a>&nbsp;&nbsp;");
				}
				if(NumProductsInCategory > 1)
				{
					int NextProductID = Common.GetNextProduct(ProductID,CategoryID,0,0,0,false,false,false);
					Response.Write("&nbsp;&nbsp;<a class=\"ProductNavLink\" href=\"pb.aspx?type=" + Common.QueryString("type") + "&packid=" + PackID.ToString() + "&productid=" + NextProductID.ToString() + "&categoryid=" + CategoryID.ToString() + "&sectionid=" + SectionID.ToString() + "&isfull=" + Common.QueryString("isfull") + "\"><img align=\"absmiddle\" src=\"skins/skin_" + thisCustomer._skinID.ToString() + "/images/next.gif\" border=\"0\" alt=\"next product in this style\"></a><br>&nbsp;");
				}
				Response.Write("</td></tr>");
				Response.Write("</table>");

				Response.Write("</td></tr>");

				Response.Write("<tr><td>");
				Response.Write("<br>");
				if(ActiveDID)
				{
					String S1 = String.Empty;
					Response.Write("<br><small>This product qualifies for quantity discount pricing. (<a href=\"javascript:void(0);\" style=\"cursor: normal;\" onMouseover=\"ddrivetip(" + DB.SQuote(Common.AppConfig("quantitydiscount") + "<br><br>" + Common.GetQuantityDiscountDisplayTable(ActiveDIDID)) + ",'" + Common.AppConfig("LightCellColor") + "', 300)\" onMouseout=\"hideddrivetip()\">what's this?</a>)</small>");
				}
				Response.Write("<div align=\"left\">");
				Response.Write(ProductDescription);
				Response.Write("</div>");
				Response.Write("</td></tr>");
				Response.Write("<tr valign=\"top\"><td height=\"10\"></td></tr>");
				MainProductSKU = String.Empty;
				if(!empty)
				{
					do
					{
						Response.Write("<tr valign=\"top\"><td>");
						MainProductSKU = DB.RSField(rs,"SKU");
						Response.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
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

						Response.Write("<tr valign=\"top\"><td>SKU:</td><td align=\"left\">" + Common.MakeProperProductSKU(DB.RSField(rs,"SKU"),DB.RSField(rs,"SkuSuffix"),"","") + "</td></tr>");
						if(DB.RSField(rs,"VariantDescription").Length != 0)
						{
							Response.Write("<tr valign=\"top\"><td width=\"50%\">Description:</td><td>" + DB.RSField(rs,"VariantDescription") + "</td></tr>");
						}
						
						if(Common.AppConfigBool("ShowInventoryTable") && Common.ProductUsesAdvancedInventoryMgmt(ProductID))
						{
							Response.Write("<tr valign=\"top\"><td width=\"100%\" align=\"left\" colspan=\"2\"><br>");
							Response.Write(Common.GetInventoryTable(ProductID,Common.GetProductsFirstVariantID(ProductID),thisCustomer._isAdminUser));
							Response.Write("</td></tr>");
						}
						
						Response.Write("<tr valign=\"top\"><td width=\"100%\" colspan=2>");

						if(Common.QueryString("IsFull").ToLower() == "true")
						{
							Response.Write("<br><b><font color=\"blue\">Your pack is currently full. To add this item, you must first remove other items above.</font></b>");
						}
						else
						{
							Response.Write(Common.GetAddToCartForm(true,false,ProductID,DB.RSFieldInt(rs,"VariantID"),1,ProductDisplayFormatID,!ImgGal.IsEmpty()));
						}
	
						Response.Write("</td></tr>");
						Response.Write("</table>");
						Response.Write("</td></tr>");
					} while (rs.Read());
				}
				else
				{
					Response.Write("<tr><td><b>This product is currently empty. Please check back soon for updates...</b></td></tr>");
				}

				Response.Write("</table></td></tr>");

				Response.Write("</table>");

				rs.Close();

				Response.Write("<script Language=\"JavaScript\">\n");
				Response.Write("function SendAddToCustomForm(theForm)\n");
				Response.Write("{\n");
				Response.Write("	top.location='dyop_addtocart.aspx?type=" + Common.QueryString("type") + "&packid=" + PackID.ToString() + "&categoryid=" + CategoryID.ToString() + "&sectionid=" + SectionID.ToString() + "&productid=' + theForm.ProductID.value + '&variantid=' + theForm.VariantID.value + '&quantity=' + theForm.Quantity.value + '&color=' + theForm.Color.value + '&size=' + theForm.Size.value;\n");
				Response.Write("}\n");
				Response.Write("</script>\n");

			}
			Response.Write("</body>\n");
			Response.Write("</html>\n");
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
