// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
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
	/// Summary description for editproduct
	/// </summary>
	public class editproduct : SkinBase
	{
		
		int NumCats;
		int NumSecs;
		int NumAffs;
		int NumCL;
		int MaxCatMaps;
		int MM;
		int ProductID;
		ProductDescriptionFile pdesc;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			NumCats = DB.GetSqlN("Select count(*) as N from category  " + DB.GetNoLock() + " where deleted=0");
			NumSecs = DB.GetSqlN("Select count(*) as N from [section]  " + DB.GetNoLock() + " where deleted=0");
			NumAffs = DB.GetSqlN("Select count(*) as N from affiliate  " + DB.GetNoLock() + " where deleted=0");
			NumCL = DB.GetSqlN("Select count(*) as N from customerlevel  " + DB.GetNoLock() + " where deleted=0");
			MaxCatMaps = Common.AppConfigUSInt("MaxCatMaps");
			if(MaxCatMaps == 0)
			{
				MaxCatMaps = 5;
			}
			MM = Common.Min(Common.Max(Common.Max(NumCats,NumSecs),NumAffs),MaxCatMaps);
			ProductID = 0;
			
			if(Common.QueryString("ProductID").Length != 0 && Common.QueryString("ProductID") != "0") 
			{
				Editing = true;
				ProductID = Localization.ParseUSInt(Common.QueryString("ProductID"));
			} 
			else 
			{
				Editing = false;
			}
			
			IDataReader rs;
			
			pdesc = new ProductDescriptionFile(ProductID,thisCustomer._localeSetting,_siteID);

			//int N = 0;
			if(Common.QueryString("DeleteDescriptionFile").ToUpper() == "TRUE")
			{
				System.IO.File.Delete(pdesc._fn);
				Response.Redirect("editproduct.aspx?productid=" + ProductID.ToString());
			}

			if(Common.QueryString("DeleteSpecFile").ToUpper() == "TRUE")
			{
				ProductSpecFile pspec = new ProductSpecFile(ProductID,_siteID);
				System.IO.File.Delete(pspec._fn);
				Response.Redirect("editproduct.aspx?productid=" + ProductID.ToString());
			}

			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{

				if(ErrorMsg.Length == 0)
				{
			
					//try
					//{
						StringBuilder sql = new StringBuilder(2500);
						String LargeProductImage = Common.LookupImage("Product",ProductID,"large",_siteID);
						if(!Editing)
						{
							// ok to add them:
							String NewGUID = DB.GetNewGUID();
							sql.Append("insert into product(ProductGUID,Name,ProductTypeID,Summary,Description,ColorOptionPrompt,SizeOptionPrompt,RequiresTextOption,TextOptionPrompt,TextOptionMaxLength,FroogleDescription,RelatedProducts,UpsellProducts,UpsellProductDiscountPercentage,RequiresProducts,SEKeywords,SEDescription,SETitle,SENoScript,SEAltText,SKU,ColWidth,ProductDisplayFormatID,ManufacturerID,ManufacturerPartNumber,SalesPromptID,SpecTitle,SpecCall,IsFeatured,IsFeaturedTeaser,VariantShown,Published,ShowBuyButton,IsCallToOrder,HidePriceUntilCart,ShowInProductBrowser,ExcludeFromPriceFeeds,IsAKit,IsAPack,PackSize,UseAdvancedInventoryMgmt,TrackInventoryBySize,TrackInventoryByColor,RequiresRegistration,SpecsInline,MiscText,SwatchImageMap,QuantityDiscountID,LastUpdatedBy) values(");
							sql.Append(DB.SQuote(NewGUID) + ",");
							sql.Append(DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							sql.Append(Common.Form("ProductTypeID").ToString() + ",");
							if(Common.Form("Summary").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("Summary")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("Description").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("Description")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("ColorOptionPrompt").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("ColorOptionPrompt")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("SizeOptionPrompt").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("SizeOptionPrompt")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							sql.Append(Common.FormUSInt("RequiresTextOption").ToString() + ",");
							if(Common.Form("TextOptionPrompt").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("TextOptionPrompt")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("TextOptionMaxLength").Length != 0)
							{
								sql.Append(Common.FormUSInt("TextOptionMaxLength").ToString() + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("FroogleDescription").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("FroogleDescription")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("RelatedProducts").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("RelatedProducts")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("UpsellProducts").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("UpsellProducts")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							sql.Append(Localization.SingleStringForDB(Common.FormUSSingle("UpsellProductDiscountPercentage")) + ",");
							if(Common.Form("RequiresProducts").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("RequiresProducts")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("SEKeywords").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("SEKeywords")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("SEDescription").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("SEDescription")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("SETitle").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("SETitle")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("SENoScript").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("SENoScript")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("SEAltText").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("SEAltText")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("SKU").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("SKU")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							sql.Append(Common.IIF(Common.Form("ColWidth").Length == 0 , Common.AppConfig("Admin_DefaultProductColWidth") , Common.Form("ColWidth")) + ",");
							sql.Append(Common.Form("ProductDisplayFormatID") + ",");
							sql.Append(Common.Form("ManufacturerID") + ",");
							sql.Append(DB.SQuote(Common.Form("ManufacturerPartNumber")) + ",");
							sql.Append(Common.Form("SalesPromptID") + ",");
							if(Common.Form("SpecTitle").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("SpecTitle")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("SpecCall").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("SpecCall")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							sql.Append("0,"); //Common.Form("IsFeatured") + ",");
							if(Common.Form("IsFeaturedTeaser").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("IsFeaturedTeaser")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							sql.Append(Common.Form("VariantShown") + ",");
							sql.Append(Common.FormUSInt("Published").ToString() + ",");
							sql.Append(Common.FormUSInt("ShowBuyButton").ToString() + ",");
							sql.Append(Common.FormUSInt("IsCallToOrder").ToString() + ",");
							sql.Append(Common.FormUSInt("HidePriceUntilCart").ToString() + ",");
							sql.Append(Common.FormUSInt("ShowInProductBrowser").ToString() + ",");
							sql.Append(Common.FormUSInt("ExcludeFromPriceFeeds").ToString() + ",");
							sql.Append(Common.FormUSInt("IsAKit").ToString() + ",");
							sql.Append(Common.FormUSInt("IsAPack").ToString() + ",");
							sql.Append(Common.FormUSInt("PackSize").ToString() + ",");
							if(Common.FormUSInt("IsAKit") == 1 || Common.FormUSInt("IsAPack") == 1)
							{
								sql.Append("0,"); // cannot use advanced inventory mgmt
								sql.Append("0,"); // cannot use advanced inventory mgmt
								sql.Append("0,"); // cannot use advanced inventory mgmt
							}
							else
							{
								sql.Append(Common.FormUSInt("UseAdvancedInventoryMgmt").ToString() + ",");
								sql.Append(Common.FormUSInt("TrackInventoryBySize").ToString() + ",");
								sql.Append(Common.FormUSInt("TrackInventoryByColor").ToString() + ",");
							}
							sql.Append(Common.FormUSInt("RequiresRegistration").ToString() + ",");
							sql.Append(Common.FormUSInt("SpecsInline").ToString() + ",");
							if(Common.Form("MiscText").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("MiscText")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("SwatchImageMap").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("SwatchImageMap")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							sql.Append(Common.Form("QuantityDiscountID") + ",");
							sql.Append(thisCustomer._customerID);
							sql.Append(")");
							DB.ExecuteSQL(sql.ToString());

							rs = DB.GetRS("select ProductID from product  " + DB.GetNoLock() + " where deleted=0 and ProductGUID=" + DB.SQuote(NewGUID));
							rs.Read();
							ProductID = DB.RSFieldInt(rs,"ProductID");
							Editing = true;
							rs.Close();
							DataUpdated = true;

							// ARE WE ADDING A SIMPLE PRODUCT, IF SO, CREATE THE DEFAULT VARIANT
							if(Common.Form("Price").Length != 0)
							{
								// ok to add:
								NewGUID = DB.GetNewGUID();
								sql.Remove(0,sql.Length);
								sql.Append("insert into productvariant(VariantGUID,ProductID,Price,SalePrice,Weight,Dimensions,Inventory,Published,Colors,ColorSKUModifiers,Sizes,SizeSKUModifiers,LastUpdatedBy) values(");
								sql.Append(DB.SQuote(NewGUID) + ",");
								sql.Append(ProductID.ToString() + ",");
								sql.Append(Common.Form("Price") + ",");
								sql.Append(Common.IIF(Common.Form("SalePrice").Length != 0 , Common.Form("SalePrice") , "NULL") + ",");
								sql.Append(Common.IIF(Common.Form("Weight").Length != 0 , Common.Form("Weight") , "NULL") + ",");
								sql.Append(DB.SQuote(Common.Form("Dimensions")) + ",");
								sql.Append(Common.IIF(Common.Form("Inventory").Length != 0 , Common.Form("Inventory") , Common.AppConfig("Admin_DefaultInventory")) + ",");
								sql.Append("1,");
								sql.Append(DB.SQuote(Common.Form("Colors")) + ",");
								sql.Append(DB.SQuote(Common.Form("ColorSKUModifiers")) + ",");
								sql.Append(DB.SQuote(Common.Form("Sizes")) + ",");
								sql.Append(DB.SQuote(Common.Form("SizeSKUModifiers")) + ",");
								sql.Append(thisCustomer._customerID);
								sql.Append(")");
								DB.ExecuteSQL(sql.ToString());
							}
						}
						else
						{
							// ok to update:
							sql.Append("update product set ");
							sql.Append("Name=" + DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							sql.Append("ProductTypeID=" + Common.Form("ProductTypeID") + ",");
							if(Common.Form("Summary").Length != 0)
							{
								sql.Append("Summary=" + DB.SQuote(Common.Form("Summary")) + ",");
							}
							else
							{
								sql.Append("Summary=NULL,");
							}
							if(Common.Form("Description").Length != 0)
							{
								sql.Append("Description=" + DB.SQuote(Common.Form("Description")) + ",");
							}
							else
							{
								sql.Append("Description=NULL,");
							}
							if(Common.Form("ColorOptionPrompt").Length != 0)
							{
								sql.Append("ColorOptionPrompt=" + DB.SQuote(Common.Form("ColorOptionPrompt")) + ",");
							}
							else
							{
								sql.Append("ColorOptionPrompt=NULL,");
							}
							if(Common.Form("SizeOptionPrompt").Length != 0)
							{
								sql.Append("SizeOptionPrompt=" + DB.SQuote(Common.Form("SizeOptionPrompt")) + ",");
							}
							else
							{
								sql.Append("SizeOptionPrompt=NULL,");
							}
							sql.Append("RequiresTextOption=" + Common.FormUSInt("RequiresTextOption") + ",");
							if(Common.Form("TextOptionPrompt").Length != 0)
							{
								sql.Append("TextOptionPrompt=" + DB.SQuote(Common.Form("TextOptionPrompt")) + ",");
							}
							else
							{
								sql.Append("TextOptionPrompt=NULL,");
							}
							if(Common.Form("TextOptionMaxLength").Length != 0)
							{
								sql.Append("TextOptionMaxLength=" + Common.FormUSInt("TextOptionMaxLength").ToString() + ",");
							}
							else
							{
								sql.Append("TextOptionMaxLength=NULL,");
							}
							if(Common.Form("FroogleDescription").Length != 0)
							{
								sql.Append("FroogleDescription=" + DB.SQuote(Common.Form("FroogleDescription")) + ",");
							}
							else
							{
								sql.Append("FroogleDescription=NULL,");
							}
							if(Common.Form("RelatedProducts").Length != 0)
							{
								sql.Append("RelatedProducts=" + DB.SQuote(Common.Form("RelatedProducts")) + ",");
							}
							else
							{
								sql.Append("RelatedProducts=NULL,");
							}
							if(Common.Form("UpsellProducts").Length != 0)
							{
								sql.Append("UpsellProducts=" + DB.SQuote(Common.Form("UpsellProducts")) + ",");
							}
							else
							{
								sql.Append("UpsellProducts=NULL,");
							}
							sql.Append("UpsellProductDiscountPercentage=" + Localization.SingleStringForDB(Common.FormUSSingle("UpsellProductDiscountPercentage")) + ",");
							if(Common.Form("RequiresProducts").Length != 0)
							{
								sql.Append("RequiresProducts=" + DB.SQuote(Common.Form("RequiresProducts")) + ",");
							}
							else
							{
								sql.Append("RequiresProducts=NULL,");
							}
							if(Common.Form("SEKeywords").Length != 0)
							{
								sql.Append("SEKeywords=" + DB.SQuote(Common.Form("SEKeywords")) + ",");
							}
							else
							{
								sql.Append("SEKeywords=NULL,");
							}
							if(Common.Form("SEDescription").Length != 0)
							{
								sql.Append("SEDescription=" + DB.SQuote(Common.Form("SEDescription")) + ",");
							}
							else
							{
								sql.Append("SEDescription=NULL,");
							}
							if(Common.Form("SETitle").Length != 0)
							{
								sql.Append("SETitle=" + DB.SQuote(Common.Form("SETitle")) + ",");
							}
							else
							{
								sql.Append("SETitle=NULL,");
							}
							if(Common.Form("SENoScript").Length != 0)
							{
								sql.Append("SENoScript=" + DB.SQuote(Common.Form("SENoScript")) + ",");
							}
							else
							{
								sql.Append("SENoScript=NULL,");
							}
							if(Common.Form("SEAltText").Length != 0)
							{
								sql.Append("SEAltText=" + DB.SQuote(Common.Form("SEAltText")) + ",");
							}
							else
							{
								sql.Append("SEAltText=NULL,");
							}
							if(Common.Form("SKU").Length != 0)
							{
								sql.Append("SKU=" + DB.SQuote(Common.Form("SKU")) + ",");
							}
							else
							{
								sql.Append("SKU=NULL,");
							}
							sql.Append("ColWidth=" + Common.IIF(Common.Form("ColWidth").Length == 0 , Common.AppConfig("Admin_DefaultProductColWidth") , Common.Form("ColWidth")) + ",");
							sql.Append("ProductDisplayFormatID=" + Common.Form("ProductDisplayFormatID") + ",");
							sql.Append("ManufacturerID=" + Common.Form("ManufacturerID") + ",");
							sql.Append("ManufacturerPartNumber=" + DB.SQuote(Common.Form("ManufacturerPartNumber")) + ",");
							sql.Append("SalesPromptID=" + Common.Form("SalesPromptID") + ",");
							if(Common.Form("SpecTitle").Length != 0)
							{
								sql.Append("SpecTitle=" + DB.SQuote(Common.Form("SpecTitle")) + ",");
							}
							else
							{
								sql.Append("SpecTitle=NULL,");
							}
							if(Common.Form("SpecCall").Length != 0)
							{
								sql.Append("SpecCall=" + DB.SQuote(Common.Form("SpecCall")) + ",");
							}
							else
							{
								sql.Append("SpecCall=NULL,");
							}
							sql.Append("IsFeatured=0,"); //Common.Form("IsFeatured") + ",");
							if(Common.Form("IsFeaturedTeaser").Length != 0)
							{
								sql.Append("IsFeaturedTeaser=" + DB.SQuote(Common.Form("IsFeaturedTeaser")) + ",");
							}
							else
							{
								sql.Append("IsFeaturedTeaser=NULL,");
							}
							sql.Append("VariantShown=" + Common.Form("VariantShown") + ",");
							sql.Append("Published=" + Common.Form("Published") + ",");
							sql.Append("ShowBuyButton=" + Common.Form("ShowBuyButton") + ",");
							sql.Append("IsCallToOrder=" + Common.Form("IsCallToOrder") + ",");
							sql.Append("HidePriceUntilCart=" + Common.Form("HidePriceUntilCart") + ",");
							sql.Append("ShowInProductBrowser=" + Common.Form("ShowInProductBrowser") + ",");
							sql.Append("ExcludeFromPriceFeeds=" + Common.Form("ExcludeFromPriceFeeds") + ",");
							sql.Append("IsAKit=" + Common.Form("IsAKit") + ",");
							sql.Append("IsAPack=" + Common.Form("IsAPack") + ",");
							sql.Append("PackSize=" + Common.FormUSInt("PackSize").ToString() + ",");
							if(Common.FormUSInt("IsAKit") == 1 || Common.FormUSInt("IsAPack") == 1)
							{
								sql.Append("UseAdvancedInventoryMgmt=0,"); // cannot use advanced inventory mgmt
								sql.Append("TrackInventoryBySize=0,");
								sql.Append("TrackInventoryByColor=0,");
							}
							else
							{
								sql.Append("UseAdvancedInventoryMgmt=" + Common.Form("UseAdvancedInventoryMgmt") + ",");
								sql.Append("TrackInventoryBySize=" + Common.Form("TrackInventoryBySize") + ",");
								sql.Append("TrackInventoryByColor=" + Common.Form("TrackInventoryByColor") + ",");
							}
							sql.Append("RequiresRegistration=" + Common.Form("RequiresRegistration") + ",");
							sql.Append("SpecsInline=" + Common.Form("SpecsInline") + ",");
							if(Common.Form("MiscText").Length != 0)
							{
								sql.Append("MiscText=" + DB.SQuote(Common.Form("MiscText")) + ",");
							}
							else
							{
								sql.Append("MiscText=NULL,");
							}
							if(Common.Form("SwatchImageMap").Length != 0)
							{
								sql.Append("SwatchImageMap=" + DB.SQuote(Common.Form("SwatchImageMap")) + ",");
							}
							else
							{
								sql.Append("SwatchImageMap=NULL,");
							}
							sql.Append("QuantityDiscountID=" + Common.Form("QuantityDiscountID") + ",");
							sql.Append("LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
							sql.Append("LastUpdatedBy=" + thisCustomer._customerID.ToString() + " ");
							sql.Append("where ProductID=" + ProductID.ToString());
							DB.ExecuteSQL(sql.ToString());
							DataUpdated = true;
							Editing = true;
						}

						// handle image uploaded:
						String FN = ProductID.ToString();
						if(Common.AppConfigBool("UseSKUForProductImageName"))
						{
							String SKU = Common.Form("SKU").Trim();
							if(SKU.Length != 0)
							{
								FN = SKU;
							}
						}
						try
						{
							String Image1 = String.Empty;
							HttpPostedFile Image1File = Request.Files["Image1"];
							if(Image1File.ContentLength != 0)
							{
								// delete any current image file first
								try
								{
									System.IO.File.Delete(Common.GetImagePath("Product","icon",true) + FN + ".jpg");
									System.IO.File.Delete(Common.GetImagePath("Product","icon",true) + FN + ".gif");
									System.IO.File.Delete(Common.GetImagePath("Product","icon",true) + FN + ".png");
								}
								catch
								{}

								String s = Image1File.ContentType;
								switch(Image1File.ContentType)
								{
									case "image/gif":
										Image1 = Common.GetImagePath("Product","icon",true) + FN + ".gif";
										Image1File.SaveAs(Image1);
										break;
									case "image/x-png":
										Image1 = Common.GetImagePath("Product","icon",true) + FN + ".png";
										Image1File.SaveAs(Image1);
										break;
									case "image/jpeg":
									case "image/pjpeg":
										Image1 = Common.GetImagePath("Product","icon",true) + FN + ".jpg";
										Image1File.SaveAs(Image1);
										break;
								}
							}

							String Image2 = String.Empty;
							HttpPostedFile Image2File = Request.Files["Image2"];
							if(Image2File.ContentLength != 0)
							{
								// delete any current image file first
								try
								{
									System.IO.File.Delete(Common.GetImagePath("Product","medium",true) + FN + ".jpg");
									System.IO.File.Delete(Common.GetImagePath("Product","medium",true) + FN + ".gif");
									System.IO.File.Delete(Common.GetImagePath("Product","medium",true) + FN + ".png");
								}
								catch
								{}

								String s = Image2File.ContentType;
								switch(Image2File.ContentType)
								{
									case "image/gif":
										Image2 = Common.GetImagePath("Product","medium",true) + FN + ".gif";
										Image2File.SaveAs(Image2);
										break;
									case "image/x-png":
										Image2 = Common.GetImagePath("Product","medium",true) + FN + ".png";
										Image2File.SaveAs(Image2);
										break;
									case "image/jpeg":
									case "image/pjpeg":
										Image2 = Common.GetImagePath("Product","medium",true) + FN + ".jpg";
										Image2File.SaveAs(Image2);
										break;
								}
							}

							String Image3 = String.Empty;
							HttpPostedFile Image3File = Request.Files["Image3"];
							if(Image3File.ContentLength != 0)
							{
								// delete any current image file first
								try
								{
									System.IO.File.Delete(Common.GetImagePath("Product","large",true) + FN + ".jpg");
									System.IO.File.Delete(Common.GetImagePath("Product","large",true) + FN + ".gif");
									System.IO.File.Delete(Common.GetImagePath("Product","large",true) + FN + ".png");
								}
								catch
								{}

								String s = Image3File.ContentType;
								switch(Image3File.ContentType)
								{
									case "image/gif":
										Image3 = Common.GetImagePath("Product","large",true) + FN + ".gif";
										Image3File.SaveAs(Image3);
										break;
									case "image/x-png":
										Image3 = Common.GetImagePath("Product","large",true) + FN + ".png";
										Image3File.SaveAs(Image3);
										break;
									case "image/jpeg":
									case "image/pjpeg":
										Image3 = Common.GetImagePath("Product","large",true) + FN + ".jpg";
										Image3File.SaveAs(Image3);
										break;
								}
							}

							// color swatch image
							String Image4 = String.Empty;
							HttpPostedFile Image4File = Request.Files["Image4"];
							if(Image4File.ContentLength != 0)
							{
								// delete any current image file first
								try
								{
									System.IO.File.Delete(Common.GetImagePath("Product","swatch",true) + FN + ".jpg");
									System.IO.File.Delete(Common.GetImagePath("Product","swatch",true) + FN + ".gif");
									System.IO.File.Delete(Common.GetImagePath("Product","swatch",true) + FN + ".png");
								}
								catch
								{}

								String s = Image4File.ContentType;
								switch(Image4File.ContentType)
								{
									case "image/gif":
										Image4 = Common.GetImagePath("Product","swatch",true) + FN + ".gif";
										Image4File.SaveAs(Image4);
										break;
									case "image/x-png":
										Image4 = Common.GetImagePath("Product","swatch",true) + FN + ".png";
										Image4File.SaveAs(Image4);
										break;
									case "image/jpeg":
									case "image/pjpeg":
										Image4 = Common.GetImagePath("Product","swatch",true) + FN + ".jpg";
										Image4File.SaveAs(Image4);
										break;
								}
							}

						}
						catch(Exception ex)
						{
							ErrorMsg = Common.GetExceptionDetail(ex,"<br>");
						}


						// UPDATE CATEGORY MAPPINGS:
						if(DataUpdated)
						{
							DB.ExecuteSQL("delete from productcategory where productid=" + ProductID.ToString());
							String CMap = Common.Form("CategoryMap");
							if(CMap.Length != 0)
							{
								String[] CMapArray = CMap.Split(',');
								foreach(String CatID in CMapArray)
								{
									DB.ExecuteSQL("insert into productcategory(productid,categoryid) values(" + ProductID.ToString() + "," + CatID.ToString() + ")");
									if(DB.GetSqlN("select count(*) as N from categorydisplayorder  " + DB.GetNoLock() + " where productid=" + ProductID.ToString() + " and CategoryID=" + CatID) == 0)
									{
										// create default mapping:
										DB.ExecuteSQL("insert into categorydisplayorder(productid,categoryid,displayorder) values(" + ProductID.ToString() + "," + CatID.ToString() + ",1)");
									}
								}
							}
							DB.ExecuteSQL("delete from categorydisplayorder where productid=" + ProductID.ToString() + " and CategoryID not in (select CategoryID from ProductCategory  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString() + ")");
						}

						// UPDATE SECTION MAPPINGS:
						if(DataUpdated)
						{
							DB.ExecuteSQL("delete from productsection where productid=" + ProductID.ToString());
							String SMap = Common.Form("SectionMap");
							if(SMap.Length != 0)
							{
								String[] SMapArray = SMap.Split(',');
								foreach(String SecID in SMapArray)
								{
									DB.ExecuteSQL("insert into productsection(productid,sectionid) values(" + ProductID.ToString() + "," + SecID.ToString() + ")");
									if(DB.GetSqlN("select count(*) as N from sectiondisplayorder  " + DB.GetNoLock() + " where productid=" + ProductID.ToString() + " and SectionID=" + SecID) == 0)
									{
										// create default mapping:
										DB.ExecuteSQL("insert into sectiondisplayorder(productid,sectionid,displayorder) values(" + ProductID.ToString() + "," + SecID.ToString() + ",1)");
									}
								}
							}
							DB.ExecuteSQL("delete from sectiondisplayorder where productid=" + ProductID.ToString() + " and SectionID not in (select SectionID from ProductSection  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString() + ")");
						}

						// UPDATE AFFILIATE MAPPINGS:
						if(DataUpdated)
						{
							DB.ExecuteSQL("delete from productaffiliate where productid=" + ProductID.ToString());
							String CMap = Common.Form("AffiliateMap");
							if(CMap.Length != 0)
							{
								String[] CMapArray = CMap.Split(',');
								foreach(String AffID in CMapArray)
								{
									DB.ExecuteSQL("insert into productaffiliate(productid,affiliateid) values(" + ProductID.ToString() + "," + AffID.ToString() + ")");
								}
							}
						}

						// UPDATE CUSTOMER LEVEL MAPPINGS:
						if(DataUpdated)
						{
							DB.ExecuteSQL("delete from productcustomerlevel where productid=" + ProductID.ToString());
							String CMap = Common.Form("CustomerLevelMap");
							if(CMap.Length != 0)
							{
								String[] CMapArray = CMap.Split(',');
								foreach(String CLID in CMapArray)
								{
									DB.ExecuteSQL("insert into productcustomerlevel(productid,customerlevelid) values(" + ProductID.ToString() + "," + CLID.ToString() + ")");
								}
							}
						}

						String LargeImage1 = Common.GetImagePath("Product","large",true) + FN + ".gif";
						String LargeImage2 = Common.GetImagePath("Product","large",true) + FN + ".jpg";
						String LargeImage3 = Common.GetImagePath("Product","large",true) + FN + ".png";
					//}
					//catch(Exception ex)
					//{
					//	ErrorMsg = "<p><b>ERROR: " + Common.GetExceptionDetail(ex,"<br>") + "<br><br></b></p>";
					//}
				}

			}
			SectionTitle = "<a href=\"products.aspx\">Products</a> - Manage Products";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			bool ProductUsesAdvancedInventoryMgmt = Common.ProductUsesAdvancedInventoryMgmt(ProductID);
			bool IsAKit = Common.IsAKit(ProductID);
			bool IsAPack = Common.IsAPack(ProductID);
			if(IsAKit || IsAPack)
			{
				ProductUsesAdvancedInventoryMgmt = false;
			}
			ProductSpecFile pspec = new ProductSpecFile(ProductID,_siteID);

			IDataReader rs = DB.GetRS("select * from Product  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString());
			if(rs.Read())
			{
				Editing = true;
			}
			
			if(ErrorMsg.Length != 0)
			{
				writer.Write("<p><b><font color=red>" + ErrorMsg + "</font></b></p>\n");
			}

			if(DataUpdated)
			{
				writer.Write("<p align=\"left\"><b><font color=blue>(UPDATED)</font></b></p>\n");
			}

			int ManufacturerID = Common.QueryStringUSInt("ManufacturerID");
			int CategoryID = Common.QueryStringUSInt("CategoryID");
			int SectionID = Common.QueryStringUSInt("SectionID");

			if(CategoryID == 0 && SectionID == 0)
			{
				CategoryID = Common.CookieUSInt("CategoryID");
				if(CategoryID == 0)
				{
					SectionID = Common.CookieUSInt("SectionID");
				}
			}

			String ProductCategories = Common.GetProductCategories(ProductID,false);
			String ProductSections = Common.GetProductSections(ProductID,false);
			String ProductAffiliates = Common.GetProductAffiliates(ProductID,false);
			String ProductCustomerLevels = Common.GetProductCustomerLevels(ProductID,false);
			String[] Cats = ProductCategories.Split(',');
			String[] Secs = ProductSections.Split(',');
			String[] Affs = ProductAffiliates.Split(',');
			String[] CLs = ProductCustomerLevels.Split(',');

			if(ErrorMsg.Length == 0)
			{

				if(Editing)
				{
					writer.Write("<p align=\"left\"><b>Editing Product: " + DB.RSField(rs,"Name") + " (Product SKU=" + DB.RSField(rs,"SKU") + ", ProductID=" + DB.RSFieldInt(rs,"ProductID").ToString() + ")&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"variants.aspx?productid=" + ProductID.ToString() + "\">Add/Edit Variants</a>");
					if(ProductUsesAdvancedInventoryMgmt)
					{
						writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"editinventory.aspx?productid=" + ProductID.ToString() + "\">Inventory</a>");
					}
					writer.Write("</b>");
					if(CategoryID == 0 && SectionID == 0)
					{
						if(ProductCategories.Length > 0)
						{
							CategoryID = Localization.ParseUSInt(Cats[0]);
						}
					}
					int NumProducts = Common.IIF(CategoryID != 0 , Common.GetNumCategoryProducts(CategoryID,true,true) , Common.GetNumSectionProducts(SectionID,true,true));
					writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;");
					if(NumProducts > 1)
					{
						int PreviousProductID = Common.GetPreviousProduct(ProductID,CategoryID,SectionID,0,0,false,true,true);
						if(CategoryID != 0)
						{
							writer.Write("<a class=\"ProductNavLink\" href=\"editproduct.aspx?productid=" + PreviousProductID.ToString() + "&categoryID=" + CategoryID.ToString() + "\">previous product</a>&nbsp;|&nbsp;");
						}
						else
						{
							writer.Write("<a class=\"ProductNavLink\" href=\"editproduct.aspx?productid=" + PreviousProductID.ToString() + "&SectionID=" + SectionID.ToString() + "\">previous product</a>&nbsp;|&nbsp;");
						}
					}
					writer.Write("<a class=\"ProductNavLink\" href=\"products.aspx?categoryid=" + CategoryID.ToString() + "&sectionID=" + SectionID.ToString() + "\">up</a>");
					if(NumProducts > 1)
					{
						int NextProductID = Common.GetNextProduct(ProductID,CategoryID,SectionID,0,0,false,true,true);
						if(CategoryID != 0)
						{
							writer.Write("&nbsp;|&nbsp;<a class=\"ProductNavLink\" href=\"editproduct.aspx?productid=" + NextProductID.ToString() + "&categoryID=" + CategoryID.ToString() + "\">next product</a><br>&nbsp");
						}
						else
						{
							writer.Write("&nbsp;|&nbsp;<a class=\"ProductNavLink\" href=\"editproduct.aspx?productid=" + NextProductID.ToString() + "&sectionID=" + SectionID.ToString() + "\">next product</a><br>&nbsp");
						}
					}
					writer.Write("</p>\n");
				}
				else
				{
					writer.Write("<p align=\"left\"><b>Adding New Product:</p></b>\n");
				}

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function Form_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("submitonce(theForm);\n");
				writer.Write("if (theForm.SalesPromptID.selectedIndex < 1)\n");
				writer.Write("{\n");
				writer.Write("alert(\"Please select a sales prompt to use.\");\n");
				writer.Write("theForm.SalesPromptID.focus();\n");
				writer.Write("submitenabled(theForm);\n");
				writer.Write("return (false);\n");
				writer.Write("    }\n");
				writer.Write("if (theForm.ProductTypeID.selectedIndex < 1)\n");
				writer.Write("{\n");
				writer.Write("alert(\"Please select a product type.\");\n");
				writer.Write("theForm.ProductTypeID.focus();\n");
				writer.Write("submitenabled(theForm);\n");
				writer.Write("return (false);\n");
				writer.Write("    }\n");
				writer.Write("if (theForm.ManufacturerID.selectedIndex < 1)\n");
				writer.Write("{\n");
				writer.Write("alert(\"Please select a manufacturer.\");\n");
				writer.Write("theForm.ManufacturerID.focus();\n");
				writer.Write("submitenabled(theForm);\n");
				writer.Write("return (false);\n");
				writer.Write("    }\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				writer.Write("<p>Please enter the following information about this product. Fields marked with an asterisk (*) are required. All other fields are optional.</p>\n");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\">\n");
				writer.Write("<form enctype=\"multipart/form-data\" action=\"editproduct.aspx?ProductID=" + ProductID.ToString() + "&edit=" + Editing.ToString() + "&manufacturerid=" + Common.QueryString("ManufacturerID") + "&categoryid=" + CategoryID.ToString() + "\" method=\"post\" id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
				writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"100%\" colspan=\"2\" align=\"left\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("<tr>\n");
				writer.Write("<td></td><td align=\"left\"><br>\n");
				if(Editing) 
				{
					writer.Write("<input type=\"submit\" value=\"Update\" name=\"submit\">\n");
					writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"reset\" class=\"CPButton\" value=\"Reset\" name=\"reset\">\n");
				} 
				else 
				{
					writer.Write("<input type=\"submit\" value=\"Add New\" name=\"submit\">\n");
				}
				writer.Write("        </td>\n");
				writer.Write("      </tr>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Product Name:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				String PName = String.Empty;
				if(Editing)
				{
					PName = Server.HtmlEncode(DB.RSField(rs,"Name"));
				}
				writer.Write("                	<input maxLength=\"100\" size=\"50\" name=\"Name\" value=\"" + Common.IIF(Editing , PName , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"Name_vldt\" value=\"[req][blankalert=Please enter the product name]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Product Type:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("<select size=\"1\" name=\"ProductTypeID\">\n");
				writer.Write(" <OPTION VALUE=\"0\">SELECT ONE</option>\n");
				IDataReader rsst = DB.GetRS("select * from ProductType  " + DB.GetNoLock() + " where deleted=0 order by name");
				while(rsst.Read())
				{
					writer.Write("<option value=\"" + DB.RSFieldInt(rsst,"ProductTypeID").ToString() + "\"");
					if(Editing) 
					{
						if(DB.RSFieldInt(rs,"ProductTypeID") == DB.RSFieldInt(rsst,"ProductTypeID"))
						{
							writer.Write(" selected");
						}
					}
					else
					{
						if(DB.RSFieldInt(rsst,"ProductTypeID") == Common.AppConfigUSInt("Admin_DefaultProductTypeID"))
						{
							writer.Write(" selected");
						}
					}
					writer.Write(">" + DB.RSField(rsst,"Name") + "</option>");
				}
				rsst.Close();
				writer.Write("</select>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Manufacturer:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("<select size=\"1\" name=\"ManufacturerID\">\n");
				writer.Write(" <OPTION VALUE=\"0\" " + Common.IIF(!Editing && ManufacturerID==0 , " selected " , "") + ">SELECT ONE</option>\n");
				rsst = DB.GetRS("select * from Manufacturer  " + DB.GetNoLock() + " where deleted=0 order by name");
				while(rsst.Read())
				{
					writer.Write("<option value=\"" + DB.RSFieldInt(rsst,"ManufacturerID").ToString() + "\"");
					if(Editing) 
					{
						if(DB.RSFieldInt(rs,"ManufacturerID") == DB.RSFieldInt(rsst,"ManufacturerID"))
						{
							writer.Write(" selected");
						}
					}
					else
					{
						if(ManufacturerID == DB.RSFieldInt(rsst,"ManufacturerID"))
						{
							writer.Write(" selected");
						}
					}
					writer.Write(">" + DB.RSField(rsst,"Name") + "</option>");
				}
				rsst.Close();
				writer.Write("</select>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">SKU:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"50\" size=\"50\" name=\"SKU\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"SKU")) , "") + "\">\n");
				//writer.Write("                	<input type=\"hidden\" name=\"SKU_vldt\" value=\"[req][blankalert=Please enter the product SKU]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Manufacturer Part #:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"50\" size=\"50\" name=\"ManufacturerPartNumber\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"ManufacturerPartNumber")) , "") + "\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\" bgcolor=\"" + Common.IIF(Editing && !DB.RSFieldBool(rs,"Published") , "#Fe5888" , "FFFFFF") + "\">*Published:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\" bgcolor=\"" + Common.IIF(Editing && !DB.RSFieldBool(rs,"Published") , "#Fe5888" , "FFFFFF") + "\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"Published\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"Published") , " checked " , "") , " checked ") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"Published\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"Published") , "" , " checked ") , "") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Show Buy Button:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\" >\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"ShowBuyButton\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"ShowBuyButton") , " checked " , "") , " checked ") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"ShowBuyButton\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"ShowBuyButton") , "" , " checked ") , "") + ">\n");
				writer.Write(" <small>if no, the add to cart button will not be shown for this product</small>");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Is Call To Order:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\" >\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"IsCallToOrder\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"IsCallToOrder") , " checked " , "") , "") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"IsCallToOrder\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"IsCallToOrder") , "" , " checked ") , " checked ") + ">\n");
				writer.Write(" <small>if no, CALL TO ORDER will be shown for this product, instead of the add to cart form/button</small>");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Hide Price Until Cart:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\" >\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"HidePriceUntilCart\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"HidePriceUntilCart") , " checked " , "") , "") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"HidePriceUntilCart\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"HidePriceUntilCart") , "" , " checked ") , " checked ") + ">\n");
				writer.Write(" <small>if yes, customer must add product to cart in order to see the price.</small>");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Show In Product Browser:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\" >\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"ShowInProductBrowser\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"ShowInProductBrowser") , " checked " , "") , " checked ") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"ShowInProductBrowser\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"ShowInProductBrowser") , "" , " checked ") , "") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Exclude From Froogle/PriceGrabber Feeds:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\" >\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"ExcludeFromPriceFeeds\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"ExcludeFromPriceFeeds") , " checked " , "") , " checked ") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"ExcludeFromPriceFeeds\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"ExcludeFromPriceFeeds") , "" , " checked ") , "") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Is A Kit:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"IsAKit\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"IsAKit") , " checked " , "") , "") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"IsAKit\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"IsAKit") , "" , " checked ") , " checked ") + ">\n");
				writer.Write(Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"IsAKit") , "<a href=\"editkit.aspx?productid=" + DB.RSFieldInt(rs,"ProductID").ToString() + "\">Edit Kit</a>" , "") , ""));
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Is A Pack:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"IsAPack\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"IsAPack") , " checked " , "") , "") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"IsAPack\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"IsAPack") , "" , " checked ") , " checked ") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Pack Size:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"10\" size=\"10\" name=\"PackSize\" value=\"" + Common.IIF(Editing , DB.RSFieldInt(rs,"PackSize").ToString() , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"PackSize_vldt\" value=\"[number][invalidalert=Please enter the pack size, e.g. 12]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				if(IsAKit || IsAPack)
				{
					writer.Write("<input type=\"hidden\" name=\"UseAdvancedInventoryMgmt\" value=\"0\">");
					writer.Write("<input type=\"hidden\" name=\"TrackInventoryBySize\" value=\"0\">");
					writer.Write("<input type=\"hidden\" name=\"TrackInventoryByColor\" value=\"0\">");
				}
				else
				{
					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td align=\"right\" valign=\"middle\">*Use Advanced Inventory Mgmt:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"UseAdvancedInventoryMgmt\" value=\"1\" " + Common.IIF(Editing ,Common.IIF(DB.RSFieldBool(rs,"UseAdvancedInventoryMgmt") , " checked " , "") , "") + ">\n");
					writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"UseAdvancedInventoryMgmt\" value=\"0\" " + Common.IIF(Editing ,Common.IIF(DB.RSFieldBool(rs,"UseAdvancedInventoryMgmt") , "" , " checked ") , " checked ") + ">\n");
					writer.Write("                </td>\n");
					writer.Write("              </tr>\n");

					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td align=\"right\" valign=\"middle\">*Track Inventory By Size:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"TrackInventoryBySize\" value=\"1\" " + Common.IIF(Editing ,Common.IIF(DB.RSFieldBool(rs,"TrackInventoryBySize") , " checked " , "") , "") + ">\n");
					writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"TrackInventoryBySize\" value=\"0\" " + Common.IIF(Editing ,Common.IIF(DB.RSFieldBool(rs,"TrackInventoryBySize") , "" , " checked ") , " checked ") + ">\n");
					writer.Write("                </td>\n");
					writer.Write("              </tr>\n");

					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td align=\"right\" valign=\"middle\">*Track Inventory By Color:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"TrackInventoryByColor\" value=\"1\" " + Common.IIF(Editing ,Common.IIF(DB.RSFieldBool(rs,"TrackInventoryByColor") , " checked " , "") , "") + ">\n");
					writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"TrackInventoryByColor\" value=\"0\" " + Common.IIF(Editing ,Common.IIF(DB.RSFieldBool(rs,"TrackInventoryByColor") , "" , " checked ") , " checked ") + ">\n");
					writer.Write("                </td>\n");
					writer.Write("              </tr>\n");

					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Color Option Prompt:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("                	<input maxLength=\"50\" size=\"20\" name=\"ColorOptionPrompt\" value=\"" + Common.IIF(Editing , DB.RSField(rs,"ColorOptionPrompt") , "") + "\">\n");
					writer.Write("                	</td>\n");
					writer.Write("              </tr>\n");
					
					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Size Option Prompt:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("                	<input maxLength=\"50\" size=\"20\" name=\"SizeOptionPrompt\" value=\"" + Common.IIF(Editing , DB.RSField(rs,"SizeOptionPrompt") , "") + "\">\n");
					writer.Write("                	</td>\n");
					writer.Write("              </tr>\n");
					
					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td align=\"right\" valign=\"middle\">*Requires Text Field:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"RequiresTextOption\" value=\"1\" " + Common.IIF(Editing ,Common.IIF(DB.RSFieldBool(rs,"RequiresTextOption") , " checked " , "") , "") + ">\n");
					writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"RequiresTextOption\" value=\"0\" " + Common.IIF(Editing ,Common.IIF(DB.RSFieldBool(rs,"RequiresTextOption") , "" , " checked ") , " checked ") + ">\n");
					writer.Write("                </td>\n");
					writer.Write("              </tr>\n");

					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Text Field Prompt:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("                	<input maxLength=\"50\" size=\"20\" name=\"TextOptionPrompt\" value=\"" + Common.IIF(Editing , DB.RSField(rs,"TextOptionPrompt") , "") + "\">\n");
					writer.Write("                	</td>\n");
					writer.Write("              </tr>\n");

					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Text Option Max Length:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("                	<input maxLength=\"5\" size=\"4\" name=\"TextOptionMaxLength\" value=\"" + Common.IIF(Editing , DB.RSFieldInt(rs,"TextOptionMaxLength").ToString() , "") + "\"> (# of characters allowed for this text option)\n");
					writer.Write("                	</td>\n");
					writer.Write("              </tr>\n");

				}

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Requires Registration To View:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"RequiresRegistration\" value=\"1\" " + Common.IIF(Editing ,Common.IIF(DB.RSFieldBool(rs,"RequiresRegistration") , " checked " , "") , "") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"RequiresRegistration\" value=\"0\" " + Common.IIF(Editing ,Common.IIF(DB.RSFieldBool(rs,"RequiresRegistration") , "" , " checked ") , " checked ") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				if(IsAKit || IsAPack)
				{
					writer.Write("<input type=\"hidden\" name=\"ProductDisplayFormatID\" value=\"1\">");
				}
				else
				{
					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td align=\"right\" valign=\"middle\">*Display Format:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("<select size=\"1\" name=\"ProductDisplayFormatID\">\n");
					rsst = DB.GetRS("select * from ProductDisplayFormat  " + DB.GetNoLock() + " where deleted=0 order by name");
					while(rsst.Read())
					{
						writer.Write("<option value=\"" + DB.RSFieldInt(rsst,"ProductDisplayFormatID").ToString() + "\"");
						if(Editing) 
						{
							if(DB.RSFieldInt(rs,"ProductDisplayFormatID") == DB.RSFieldInt(rsst,"ProductDisplayFormatID"))
							{
								writer.Write(" selected");
							}
						}
						else
						{
							if(DB.RSFieldInt(rsst,"ProductDisplayFormatID") == Common.AppConfigUSInt("Admin_DefaultProductDisplayFormatID"))
							{
								writer.Write(" selected");
							}
						}
						writer.Write(">" + DB.RSField(rsst,"Name") + "</option>");
					}
					rsst.Close();
					writer.Write("</select>\n");
					writer.Write("                </td>\n");
					writer.Write("              </tr>\n");
				}

				writer.Write("<tr valign=\"middle\">\n");
				writer.Write("<td align=\"right\" valign=\"middle\">Quantity Discount Table:&nbsp;&nbsp;</td>\n");
				writer.Write("<td align=\"left\">\n");
				writer.Write("<select size=\"1\" name=\"QuantityDiscountID\">\n");
				writer.Write("<option value=\"0\">None</option>");
				rsst = DB.GetRS("select * from QuantityDiscount  " + DB.GetNoLock() + " order by name");
				while(rsst.Read())
				{
					writer.Write("<option value=\"" + DB.RSFieldInt(rsst,"QuantityDiscountID").ToString() + "\"");
					if(Editing) 
					{
						if(DB.RSFieldInt(rs,"QuantityDiscountID") == DB.RSFieldInt(rsst,"QuantityDiscountID"))
						{
							writer.Write(" selected");
						}
					}
					writer.Write(">" + DB.RSField(rsst,"Name") + "</option>");
				}
				rsst.Close();
				writer.Write("</select>\n");
				writer.Write("</td>\n");
				writer.Write("</tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">Grid Column Width:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"2\" size=\"2\" name=\"ColWidth\" value=\"" + Common.IIF(Editing ,DB.RSFieldInt(rs,"ColWidth").ToString() , "4") + "\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Category(s):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");

				writer.Write("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\n");
				writer.Write("<tr>");
				writer.Write("<td align=\"left\" valign=\"top\">" + GetCategoryList(ProductID, ProductCategories, 0,1) + "</td>");
				writer.Write("<td align=\"right\" valign=\"top\">" + Common.AppConfig("SectionPromptPlural") + ":&nbsp;&nbsp;</td>");
				writer.Write("<td align=\"left\" valign=\"top\">" + GetSectionList(ProductID, ProductSections, 0,1) + "</td>");
				writer.Write("<td align=\"right\" valign=\"top\">Affiliate(s):&nbsp;&nbsp;</td>");
				writer.Write("<td align=\"left\" valign=\"top\">" + GetAffiliateList(ProductID, ProductAffiliates, 0,1) + "</td>");
				writer.Write("<td align=\"right\" valign=\"top\">Customer Level(s):&nbsp;&nbsp;</td>");
				writer.Write("<td align=\"left\" valign=\"top\">" + GetCustomerLevelList(ProductID, ProductCustomerLevels, 0,1) + "</td>");
				writer.Write("</tr>");
				writer.Write("</table>");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Summary (HTML OK):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<textarea style=\"height: 30em; width: 100%;\" cols=\"" + Common.AppConfig("Admin_TextareaWidth") + "\" rows=\"" + Common.AppConfig("Admin_TextareaHeightSmall") + "\" id=\"Summary\" name=\"Summary\">" + Common.IIF(Editing ,Server.HtmlEncode(DB.RSField(rs,"Summary")) , "") + "</textarea>");
				//writer.Write("<a href=\"autofill.aspx?productid=" + ProductID.ToString() + "\" target=\"_blank\">AutoFill Variants</a>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Description (HTML OK):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				if(Editing && pdesc._contents.Length != 0)
				{
					writer.Write("<b>From File: " + pdesc._url + "</b> &nbsp;&nbsp;\n");
					writer.Write("<input type=\"button\" value=\"Delete\" name=\"DeleteDescriptionFile_" + ProductID.ToString() + "\" onClick=\"DeleteDescriptionFile()\">");
					writer.Write("<div style=\"border-style: dashed; border-width: 1px;\">\n");
					writer.Write(pdesc._contents);
					writer.Write("</div>\n");
				}
				//else
				//{
				writer.Write("                	<textarea style=\"height: 60em; width: 100%;\" cols=\"" + Common.AppConfig("Admin_TextareaWidth") + "\" rows=\"" + Common.AppConfig("Admin_TextareaHeightl") + "\" id=\"Description\" name=\"Description\">" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Description")) , "") + "</textarea>\n");
				//}
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Froogle Description (NO HTML):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<textarea style=\"height: 30em; width: 100%;\" cols=\"" + Common.AppConfig("Admin_TextareaWidth") + "\" rows=\"" + Common.AppConfig("Admin_TextareaHeightSmall") + "\" id=\"FroogleDescription\" name=\"FroogleDescription\">" + Common.IIF(Editing ,Server.HtmlEncode(DB.RSField(rs,"FroogleDescription")) , "") + "</textarea>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Related Products:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"255\" size=\"100\" name=\"RelatedProducts\" value=\"" + Common.IIF(Editing ,DB.RSField(rs,"RelatedProducts") , "") + "\"> (enter related PRODUCT IDs, NOT names, e.g. 42,13,150)\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Upsell Products:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"255\" size=\"100\" name=\"UpsellProducts\" value=\"" + Common.IIF(Editing ,DB.RSField(rs,"UpsellProducts") , "") + "\"> (enter upsell PRODUCT IDs, NOT names, e.g. 42,13,150)\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Upsell Product Discount Percent:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"UpsellProductDiscountPercentage\" value=\"" + Common.IIF(Editing , Localization.SingleStringForDB(DB.RSFieldSingle(rs,"UpsellProductDiscountPercentage")), "") + "\"><small>(Enter 0, or a percentage like 5 or 7.5)</small>\n");
				writer.Write("                	<input type=\"hidden\" name=\"UpsellProductDiscountPercentage_vldt\" value=\"[number][invalidalert=Please enter a valid percentage amount, e.g. 10.0]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Requires Products:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"255\" size=\"100\" name=\"RequiresProducts\" value=\"" + Common.IIF(Editing ,DB.RSField(rs,"RequiresProducts") , "") + "\"> (enter PRODUCT IDs, NOT names, that MUST be in the cart if this product is also in the cart. The store will add these to the customer cart automatically if they are not present when this product is added. e.g. 42,13,150)\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Search Engine Page Title:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"100\" name=\"SETitle\" value=\"" + Common.IIF(Editing ,DB.RSField(rs,"SETitle") , "") + "\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Search Engine Keywords:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"1000\" size=\"100\" name=\"SEKeywords\" value=\"" + Common.IIF(Editing ,DB.RSField(rs,"SEKeywords") , "") + "\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Search Engine Description:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"255\" size=\"100\" name=\"SEDescription\" value=\"" + Common.IIF(Editing ,DB.RSField(rs,"SEDescription") , "") + "\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");


				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Search Engine NoScript:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<textarea name=\"SENoScript\" rows=\"10\" cols=\"50\">" + Common.IIF(Editing , DB.RSField(rs,"SENoScript") , "") + "</textarea>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Search Engine AltText:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"50\" size=\"50\" name=\"SEAltText\" value=\"" + Common.IIF(Editing , DB.RSField(rs,"SEAltText") , "") + "\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*'On Sale' Prompt:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("<select size=\"1\" name=\"SalesPromptID\">\n");
				writer.Write(" <OPTION VALUE=\"0\">SELECT ONE</option>\n");
				rsst = DB.GetRS("select * from salesprompt  " + DB.GetNoLock() + " where deleted=0 order by name");
				while(rsst.Read())
				{
					writer.Write("<option value=\"" + DB.RSFieldInt(rsst,"SalesPromptID").ToString() + "\"");
					if(Editing) 
					{
						if(DB.RSFieldInt(rs,"SalesPromptID") == DB.RSFieldInt(rsst,"SalesPromptID"))
						{
							writer.Write(" selected");
						}
					}
					else
					{
						if(DB.RSFieldInt(rsst,"SalesPromptID") == Common.AppConfigUSInt("Admin_DefaultSalesPromptID"))
						{
							writer.Write(" selected");
						}
					}
					writer.Write(">" + DB.RSField(rsst,"Name") + "</option>");
				}
				rsst.Close();
				writer.Write("</select>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Spec Title:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"50\" name=\"SpecTitle\" value=\"" + Common.IIF(Editing ,Server.HtmlEncode(DB.RSField(rs,"SpecTitle")) , "") + "\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Spec Call:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"1000\" size=\"50\" name=\"SpecCall\" value=\"" + Common.IIF(Editing ,DB.RSField(rs,"SpecCall") , "") + "\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Specs Inline:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"SpecsInline\" value=\"1\" " + Common.IIF(Editing ,Common.IIF(DB.RSFieldBool(rs,"SpecsInline") , " checked " , "") , Common.IIF(Common.AppConfigBool("Admin_SpecsInlineByDefault") , " checked " , "")) + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"SpecsInline\" value=\"0\" " + Common.IIF(Editing ,Common.IIF(DB.RSFieldBool(rs,"SpecsInline") , "" , " checked ") , Common.IIF(Common.AppConfigBool("Admin_SpecsInlineByDefault") , "" , " checked ")) + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				//				writer.Write("              <tr valign=\"middle\">\n");
				//				writer.Write("                <td align=\"right\" valign=\"middle\">Is Featured Now:&nbsp;&nbsp;</td>\n");
				//				writer.Write("                <td align=\"left\">\n");
				//				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"IsFeatured\" value=\"1\" " + Common.IIF(Editing ,(DB.RSFieldBool(rs,"IsFeatured") , " checked " , "") , "") + ">\n");
				//				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"IsFeatured\" value=\"0\" " + Common.IIF(Editing ,(DB.RSFieldBool(rs,"IsFeatured") , "" , " checked ") , " checked ") + ">\n");
				//				writer.Write("                </td>\n");
				//				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Is Featured Teaser:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"500\" size=\"50\" name=\"IsFeaturedTeaser\" value=\"" + Common.IIF(Editing ,DB.RSField(rs,"IsFeaturedTeaser") , "") + "\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				if(IsAKit || IsAPack)
				{
					writer.Write("<input type=\"hidden\" name=\"VariantShown\" value=\"0\">");
				}
				else
				{
					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Variant Shown:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("                	<input maxLength=\"100\" size=\"50\" name=\"VariantShown\" value=\"" + Common.IIF(Editing ,DB.RSFieldInt(rs,"VariantShown").ToString() , "1") + "\">\n");
					writer.Write("                	<input type=\"hidden\" name=\"VariantShown_vldt\" value=\"[number][invalidalert=Please enter the variant number to show for this product as a default, e.g. 1]\">\n");
					writer.Write("                	</td>\n");
					writer.Write("              </tr>\n");
				}

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Misc Text (HTML OK):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<textarea cols=\"" + Common.AppConfig("Admin_TextareaWidth") + "\" rows=\"" + Common.AppConfig("Admin_TextareaHeightSmall") + "\" name=\"MiscText\">" + Common.IIF(Editing ,Server.HtmlEncode(DB.RSField(rs,"MiscText")) , "") + "</textarea>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("  <tr>\n");
				writer.Write("    <td valign=\"top\" align=\"right\">Icon:\n");
				writer.Write("</td>\n");
				writer.Write("    <td valign=\"top\" align=\"left\">");
				writer.Write("    <input type=\"file\" name=\"Image1\" size=\"50\" value=\"" + Common.IIF(Editing ,"" , "") + "\">\n");
				String Image1URL = Common.LookupImage("Product",ProductID,"icon",_siteID);
				if(Editing && Image1URL.Length != 0)
				{
					writer.Write("<a href=\"javascript:void(0);\" onClick=\"DeleteIcon('icon','Pic1');\">Click here</a> to delete the current image<br>\n");
					writer.Write("<br><img id=\"Pic1\" name=\"Pic1\" border=\"0\" src=\"" + Image1URL + "?" + Common.GetRandomNumber(1,1000000).ToString() + "\">\n");
				}
				writer.Write("</td>\n");
				writer.Write(" </tr>\n");

				writer.Write("  <tr>\n");
				writer.Write("    <td valign=\"top\" align=\"right\">Medium Pic:\n");
				writer.Write("</td>\n");
				writer.Write("    <td valign=\"top\" align=\"left\">");
				writer.Write("    <input type=\"file\" name=\"Image2\" size=\"50\" value=\"" + Common.IIF(Editing ,"" , "") + "\">\n");
				String Image2URL = Common.LookupImage("Product",ProductID,"medium",_siteID);
				if(Image2URL.Length != 0)
				{
					writer.Write("<a href=\"javascript:void(0);\" onClick=\"DeleteIcon('medium','Pic2');\">Click here</a> to delete the current image<br>\n");
					if(Common.GetProductsFirstVariantID(ProductID) != 0)
					{
						writer.Write("&nbsp;&nbsp;<a href=\"productimagemgr.aspx?productid=" + ProductID.ToString() + "\">Multi-Image Manager</a>");
					}
					writer.Write("<br><img id=\"Pic2\" name=\"Pic2\" border=\"0\" src=\"" + Image2URL + "?" + Common.GetRandomNumber(1,1000000).ToString() + "\">\n");
				}
				writer.Write("</td>\n");
				writer.Write(" </tr>\n");

				writer.Write("  <tr>\n");
				writer.Write("    <td valign=\"top\" align=\"right\">Large Pic:\n");
				writer.Write("</td>\n");
				writer.Write("    <td valign=\"top\" align=\"left\">");
				writer.Write("    <input type=\"file\" name=\"Image3\" size=\"50\" value=\"" + Common.IIF(Editing ,"" , "") + "\">\n");
				String Image3URL = Common.LookupImage("Product",ProductID,"large",_siteID);
				if(Image3URL.Length != 0)
				{
					writer.Write("<a href=\"javascript:void(0);\" onClick=\"DeleteIcon('large','Pic3');\">Click here</a> to delete the current image<br>\n");
					writer.Write("<br><img id=\"Pic3\" name=\"Pic3\" border=\"0\" src=\"" + Image3URL + "?" + Common.GetRandomNumber(1,1000000).ToString() + "\">\n");
				}
				writer.Write("</td>\n");
				writer.Write(" </tr>\n");

				writer.Write("  <tr>\n");
				writer.Write("    <td valign=\"top\" align=\"right\">Color Swatch Pic:\n");
				writer.Write("</td>\n");
				writer.Write("    <td valign=\"top\" align=\"left\">");
				writer.Write("    <input type=\"file\" name=\"Image4\" size=\"50\" value=\"" + Common.IIF(Editing ,"" , "") + "\">\n");
				String Image4URL = Common.LookupImage("Product",ProductID,"swatch",_siteID);
				if(Image4URL.Length != 0)
				{
					if(Image4URL.IndexOf("nopicture") == -1)
					{
						writer.Write("<a href=\"javascript:void(0);\" onClick=\"DeleteIcon('swatch','Pic2');\">Click here</a> to delete the current image<br>\n");
					}
					writer.Write("<br><img id=\"Pic2\" name=\"Pic2\" border=\"0\" src=\"" + Image4URL + "?" + Common.GetRandomNumber(1,1000000).ToString() + "\">\n");
				}
				writer.Write("</td>\n");
				writer.Write(" </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Swatch Image Map:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<textarea cols=\"" + Common.AppConfig("Admin_TextareaWidth") + "\" rows=\"" + Common.AppConfig("Admin_TextareaHeightSmall") + "\" name=\"SwatchImageMap\">" + Common.IIF(Editing ,DB.RSField(rs,"SwatchImageMap") , "") + "</textarea>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("<tr>\n");
				writer.Write("<td></td><td align=\"left\"><br>\n");
				if(Editing) 
				{
					writer.Write("<input type=\"submit\" value=\"Update\" name=\"submit\">\n");
					writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"reset\" class=\"CPButton\" value=\"Reset\" name=\"reset\">\n");
				} 
				else 
				{
					writer.Write("<input type=\"submit\" value=\"Add New\" name=\"submit\">\n");
				}
				writer.Write("        </td>\n");
				writer.Write("      </tr>\n");

				if(Editing && pspec._contents.Length != 0)
				{
					writer.Write("<tr valign=\"middle\">\n");
					writer.Write("<td align=\"right\" valign=\"top\">Specifications:&nbsp;&nbsp;</td>\n");
					writer.Write("<td align=\"left\">\n");
					writer.Write("<b>From File: " + pspec._fn + "</b> &nbsp;&nbsp;\n");
					writer.Write("<input type=\"button\" value=\"Delete\" name=\"DeleteSpecFile_" + ProductID.ToString() + "\" onClick=\"DeleteSpecFile()\">");
					writer.Write("<div style=\"border-style: dashed; border-width: 1px;\">\n");
					writer.Write(pspec._contents);
					writer.Write("</div>\n");
					writer.Write("</td>\n");
					writer.Write("</tr>\n");
				}



				if(!Editing)
				{
					writer.Write("<tr><td colspan=\"2\"><br><hr size=1><b>IF YOU ARE ADDING A SIMPLE PRODUCT, OR PACK, OR KIT YOU MUST ENTER AT LEAST THE PRICE BELOW AND USE THE BOTTOM SUBMIT BUTTON ON THIS PAGE!! IF YOU NEED A COMPLEX PRODUCT (ONE THAT HAS MULTIPLE VARIANTS), LEAVE ALL FIELDS BELOW BLANK AND USE THE SUBMIT BUTTON ABOVE, AND THEN ENTER PRODUCT VARIANTS <font color=blue>AFTER</font> YOU HAVE ADDED THE MAIN PRODUCT!</b><br></td></tr>\n");

					// SIMPLE VARIANT STUFF:
					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Price:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("                	<input maxLength=\"10\" size=\"10\" name=\"Price\" value=\"" + Common.IIF(Editing ,DB.RSFieldDecimal(rs,"Price").ToString() , "") + "\">\n");
					writer.Write("                	<input type=\"hidden\" name=\"Price_vldt\" value=\"[number][blankalert=Please enter the variant price][invalidalert=Please enter a valid dollar amount, e.g. 10.00 without the leading $ sign!]\">\n");
					writer.Write("                	</td>\n");
					writer.Write("              </tr>\n");

					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Sale Price:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("                	<input maxLength=\"10\" size=\"10\" name=\"SalePrice\" value=\"" + Common.IIF(Editing ,DB.RSFieldDecimal(rs,"SalePrice").ToString() , "") + "\">\n");
					writer.Write("                	<input type=\"hidden\" name=\"SalePrice_vldt\" value=\"[number][invalidalert=Please enter a valid dollar amount, e.g. 10.00 without the leading $ sign!]\">\n");
					writer.Write("                	</td>\n");
					writer.Write("              </tr>\n");

					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Weight:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("                	<input maxLength=\"10\" size=\"10\" name=\"Weight\" value=\"" + Common.IIF(Editing ,DB.RSFieldSingle(rs,"Weight").ToString() , "") + "\"> <small>(in " + Localization.WeightUnits() + ")</small>\n");
					writer.Write("                	<input type=\"hidden\" name=\"Weight_vldt\" value=\"[number][invalidalert=Please enter the weight of this item in " + Localization.WeightUnits() + ", e.g. 2.5]\">\n");
					writer.Write("                	</td>\n");
					writer.Write("              </tr>\n");

					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Dimensions:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("                	<input maxLength=\"50\" size=\"50\" name=\"Dimensions\" value=\"" + Common.IIF(Editing ,DB.RSField(rs,"Dimensions") , "") + "\">\n");
					writer.Write("                	</td>\n");
					writer.Write("              </tr>\n");

					if(!ProductUsesAdvancedInventoryMgmt)
					{
						writer.Write("              <tr valign=\"middle\">\n");
						writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Current Inventory:&nbsp;&nbsp;</td>\n");
						writer.Write("                <td align=\"left\">\n");
						writer.Write("                	<input maxLength=\"10\" size=\"10\" name=\"Inventory\" value=\"" + Common.IIF(Editing ,DB.RSFieldInt(rs,"Inventory").ToString() , "") + "\">\n");
						writer.Write("                	<input type=\"hidden\" name=\"Inventory_vldt\" value=\"[number][invalidalert=Please enter the current inventory in stock for this item, e.g. 100]\">\n");
						writer.Write("                	</td>\n");
						writer.Write("              </tr>\n");
					}

					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Colors:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("                	<input maxLength=\"5000\" size=\"50\" name=\"Colors\" value=\"" + Common.IIF(Editing ,DB.RSField(rs,"Colors") , "") + "\">&nbsp;<small>(Separate colors by commas)</small>\n");
					writer.Write("                	</td>\n");
					writer.Write("              </tr>\n");

					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Color SKU Modifiers:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("                	<input maxLength=\"5000\" size=\"50\" name=\"ColorSKUModifiers\" value=\"" + Common.IIF(Editing ,DB.RSField(rs,"ColorSKUModifiers") , "") + "\">&nbsp;<small>(Separate skus by commas to match colors)</small>\n");
					writer.Write("                	</td>\n");
					writer.Write("              </tr>\n");

					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Sizes:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("                	<input maxLength=\"5000\" size=\"50\" name=\"Sizes\" value=\"" + Common.IIF(Editing ,DB.RSField(rs,"Sizes") , "") + "\">&nbsp;<small>(Separate sizes by commas)</small>\n");
					writer.Write("                	</td>\n");
					writer.Write("              </tr>\n");

					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Size SKU Modifiers:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("                	<input maxLength=\"5000\" size=\"50\" name=\"SizeSKUModifiers\" value=\"" + Common.IIF(Editing ,DB.RSField(rs,"SizeSKUModifiers") , "") + "\">&nbsp;<small>(Separate skus by commas to match sizes)</small>\n");
					writer.Write("                	</td>\n");
					writer.Write("              </tr>\n");

					writer.Write("<tr>\n");
					writer.Write("<td></td><td align=\"left\"><br>\n");
					writer.Write("<input type=\"submit\" value=\"Add New (Simple Product, or Kit, Or Pack)\" name=\"submit\">\n");
					writer.Write("</td>\n");
					writer.Write("</tr>\n");


				
				}

				writer.Write("</form>\n");
				writer.Write("  </table>\n");

				writer.Write(Common.GenerateHtmlEditor("Description"));
				//writer.Write(Common.GenerateHtmlEditor("MiscText"));
				writer.Write(Common.GenerateHtmlEditor("Summary"));

				writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
				writer.Write("function DeleteIcon(size,name)\n");
				writer.Write("{\n");
				writer.Write("window.open('deleteicon.aspx?ProductID=" + ProductID.ToString() + "&FormImageName=' + name + '&size=' + size,\"Admin_ML\",\"height=250,width=440,top=10,left=20,status=no,toolbar=no,menubar=no,scrollbars=yes,location=no\")\n");
				writer.Write("}\n");
				writer.Write("function DeleteDescriptionFile()\n");
				writer.Write("{\n");
				writer.Write("if(confirm('Are you sure you want to delete the description file for this product?'))\n");
				writer.Write("{\n");
				writer.Write("self.location = 'editproduct.aspx?productid=" + ProductID.ToString() + "&deletedescriptionfile=true';\n");
				writer.Write("}\n");
				writer.Write("}\n");
				writer.Write("function DeleteSpecFile()\n");
				writer.Write("{\n");
				writer.Write("if(confirm('Are you sure you want to delete the spec file for this product?'))\n");
				writer.Write("{\n");
				writer.Write("self.location = 'editproduct.aspx?productid=" + ProductID.ToString() + "&deletespecfile=true';\n");
				writer.Write("}\n");
				writer.Write("}\n");
				writer.Write("</SCRIPT>\n");
			}
			rs.Close();
		}

		static public String GetCategoryList(int ProductID, String ProductCategories, int ForParentCategoryID, int level)
		{
			StringBuilder tmpS = new StringBuilder(5000);
			String sql = String.Empty;
			if(ForParentCategoryID == 0)
			{
				sql = "select * from category  " + DB.GetNoLock() + " where (parentcategoryid=0 or ParentCategoryID IS NULL) and deleted=0 order by DisplayOrder,Name";
			}
			else
			{
				sql = "select * from category  " + DB.GetNoLock() + " where parentcategoryid=" + ForParentCategoryID.ToString() + " and deleted=0 order by DisplayOrder,Name";
			}
			IDataReader rs = DB.GetRS(sql);

			String Indent = String.Empty;
			for(int i = 1; i < level; i++)
			{
				Indent += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
			}
			while(rs.Read())
			{
				bool ProductIsMappedToThisCategory = (("," + ProductCategories + ",").IndexOf("," + DB.RSFieldInt(rs,"CategoryID").ToString() + ",") != -1);
				tmpS.Append("<input type=\"checkbox\" name=\"CategoryMap\" value=\"" +  DB.RSFieldInt(rs,"CategoryID").ToString() + "\" " + Common.IIF(ProductIsMappedToThisCategory , " checked " , "") + ">" + Common.IIF(level == 1 , "<b>" , "") + Indent + DB.RSField(rs,"name") + Common.IIF(level == 1 , "</b>" , "") + "<br>\n");
				if(Common.CategoryHasSubs(DB.RSFieldInt(rs,"CategoryID")))
				{
					tmpS.Append(GetCategoryList(ProductID, ProductCategories, DB.RSFieldInt(rs,"CategoryID"),level+1));
				}
			}
			rs.Close();
			return tmpS.ToString();
		}

		static public String GetSectionList(int ProductID, String ProductSections, int ForParentSectionID, int level)
		{
			StringBuilder tmpS = new StringBuilder(5000);
			String sql = String.Empty;
			if(ForParentSectionID == 0)
			{
				sql = "select * from [section]  " + DB.GetNoLock() + " where (parentSectionid=0 or ParentSectionID IS NULL) and published<>0 and deleted=0 order by DisplayOrder,Name";
			}
			else
			{
				sql = "select * from [section]  " + DB.GetNoLock() + " where parentSectionid=" + ForParentSectionID.ToString() + " and published<>0 and deleted=0 order by DisplayOrder,Name";
			}
			IDataReader rs = DB.GetRS(sql);

			String Indent = String.Empty;
			for(int i = 1; i < level; i++)
			{
				Indent += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
			}
			while(rs.Read())
			{
				bool ProductIsMappedToThisSection = (("," + ProductSections + ",").IndexOf("," + DB.RSFieldInt(rs,"SectionID").ToString() + ",") != -1);
				tmpS.Append("<input type=\"checkbox\" name=\"SectionMap\" value=\"" +  DB.RSFieldInt(rs,"SectionID").ToString() + "\" " + Common.IIF(ProductIsMappedToThisSection , " checked " , "") + ">" + Common.IIF(level == 1 , "<b>" , "") + Indent + DB.RSField(rs,"name") + Common.IIF(level == 1 , "</b>" , "") + "<br>\n");
				if(Common.SectionHasSubs(DB.RSFieldInt(rs,"SectionID")))
				{
					tmpS.Append(GetSectionList(ProductID, ProductSections, DB.RSFieldInt(rs,"SectionID"),level+1));
				}
			}
			rs.Close();
			return tmpS.ToString();
		}

		static public String GetAffiliateList(int ProductID, String ProductAffiliates, int ForParentAffiliateID, int level)
		{
			StringBuilder tmpS = new StringBuilder(5000);
			String sql = "select * from Affiliate  " + DB.GetNoLock() + " where deleted=0 order by lastname,firstname";
			IDataReader rs = DB.GetRS(sql);

			String Indent = String.Empty;
			for(int i = 1; i < level; i++)
			{
				Indent += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
			}
			while(rs.Read())
			{
				bool ProductIsMappedToThisAffiliate = (("," + ProductAffiliates + ",").IndexOf("," + DB.RSFieldInt(rs,"AffiliateID").ToString() + ",") != -1);
				String AffNM = (DB.RSField(rs,"firstname") + " " + DB.RSField(rs,"LastName")).Trim();
				if(AffNM.Length == 0)
				{
					AffNM = DB.RSField(rs,"Company");
				}
				if(AffNM.Length == 0)
				{
					AffNM = "Affiliate " + DB.RSFieldInt(rs,"AffiliateID").ToString();
				}
				tmpS.Append("<input type=\"checkbox\" name=\"AffiliateMap\" value=\"" +  DB.RSFieldInt(rs,"AffiliateID").ToString() + "\" " + Common.IIF(ProductIsMappedToThisAffiliate , " checked " , "") + ">" + Common.IIF(level == 1 , "<b>" , "") + Indent + AffNM + Common.IIF(level == 1 , "</b>" , "") + "<br>\n");
			}
			rs.Close();
			return tmpS.ToString();
		}

		static public String GetCustomerLevelList(int ProductID, String ProductCustomerLevels, int ForParentCustomerLevelID, int level)
		{

			StringBuilder tmpS = new StringBuilder(5000);
			String sql = "select * from CustomerLevel  " + DB.GetNoLock() + " where deleted=0 order by customerlevelid";
			IDataReader rs = DB.GetRS(sql);

			String Indent = String.Empty;
			for(int i = 1; i < level; i++)
			{
				Indent += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
			}
			while(rs.Read())
			{
				bool ProductIsMappedToThisCustomerLevel = (("," + ProductCustomerLevels + ",").IndexOf("," + DB.RSFieldInt(rs,"CustomerLevelID").ToString() + ",") != -1);
				String AffNM = DB.RSField(rs,"name").Trim();
				if(AffNM.Length == 0)
				{
					AffNM = "CustomerLevel " + DB.RSFieldInt(rs,"CustomerLevelID").ToString();
				}
				tmpS.Append("<input type=\"checkbox\" name=\"CustomerLevelMap\" value=\"" +  DB.RSFieldInt(rs,"CustomerLevelID").ToString() + "\" " + Common.IIF(ProductIsMappedToThisCustomerLevel , " checked " , "") + ">" + Common.IIF(level == 1 , "<b>" , "") + Indent + AffNM + Common.IIF(level == 1 , "</b>" , "") + "<br>\n");
			}
			rs.Close();
			return tmpS.ToString();
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
