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
	/// Summary description for editvariant
	/// </summary>
	public class editvariant : SkinBase
	{
		
		int ProductID;
		int VariantID;
		bool ProductUsesAdvancedInventoryMgmt;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			ProductID = Common.QueryStringUSInt("ProductID");
			VariantID = 0;
			
			if(Common.QueryString("VariantID").Length != 0 && Common.QueryString("VariantID") != "0") 
			{
				Editing = true;
				VariantID = Localization.ParseUSInt(Common.QueryString("VariantID"));
			} 
			else 
			{
				Editing = false;
			}
			if(ProductID == 0)
			{
				ProductID = Common.GetVariantProductID(VariantID);
			}
			
			ProductUsesAdvancedInventoryMgmt = Common.ProductUsesAdvancedInventoryMgmt(ProductID);
			
			IDataReader rs;
			
			//int N = 0;
			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
//				try
				//{
					decimal Price = System.Decimal.Zero;
					decimal SalePrice = System.Decimal.Zero;
					//Single KitPrice = System.Decimal.Zero;
					decimal MSRP = System.Decimal.Zero;
					decimal Cost = System.Decimal.Zero;
					decimal ShippingCost = System.Decimal.Zero;
					int Points = 0;
					int MinimumQuantity = 0;
					if(Common.Form("Price").Length != 0)
					{
						Price = Common.FormUSDecimal("Price");
					}
					if(Common.Form("SalePrice").Length != 0)
					{
						SalePrice = Common.FormUSDecimal("SalePrice");
					}

					if(Common.Form("MSRP").Length != 0)
					{
						MSRP = Common.FormUSDecimal("MSRP");
					}
					if(Common.Form("Cost").Length != 0)
					{
						Cost = Common.FormUSDecimal("Cost");
					}
					if(Common.Form("ShippingCost").Length != 0)
					{
						ShippingCost = Common.FormUSDecimal("ShippingCost");
					}
					if(Common.AppConfig("MicroPay.PointsPerDollar").Length != 0)
					{
						Single PointsPerDollar = Common.AppConfigUSSingle("MicroPay.PointsPerDollar");
						if(PointsPerDollar != 0.0F)
						{
							Points = (int)(Price * (decimal)PointsPerDollar);
						}
					}
					else if(Common.Form("Points").Length != 0)
					{
						Points = Common.FormUSInt("Points");
					}
					if(Common.Form("MinimumQuantity").Length != 0)
					{
						MinimumQuantity = Common.FormUSInt("MinimumQuantity");
					}

					StringBuilder sql = new StringBuilder(2500);
					if(!Editing)
					{
						// ok to add:
						String NewGUID = DB.GetNewGUID();
						sql.Append("insert into productvariant(VariantGUID,ProductID,Name,Description,RestrictedQuantities,FroogleDescription,Price,SalePrice,MSRP,Cost,Points,MinimumQuantity,ShippingCost,SKUSuffix,ManufacturerPartNumber,Weight,Dimensions,Inventory,SubscriptionMonths,Published,IsRecurring,RecurringInterval,RecurringIntervalType,Colors,ColorSKUModifiers,Sizes,SizeSKUModifiers,QuantityDiscountID,IsTaxable,IsShipSeparately,IsDownload,IsSecureAttachment,DownloadLocation,LastUpdatedBy) values(");
						sql.Append(DB.SQuote(NewGUID) + ",");
						sql.Append(ProductID.ToString() + ",");
						sql.Append(DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
						if(Common.Form("Description").Length != 0)
						{
							sql.Append(DB.SQuote(Common.Form("Description")) + ",");
						}
						else
						{
							sql.Append("NULL,");
						}
						if(Common.Form("RestrictedQuantities").Length != 0)
						{
							sql.Append(DB.SQuote(Common.Form("RestrictedQuantities")) + ",");
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
						sql.Append(Localization.DecimalStringForDB(Price) + ",");
						sql.Append(Common.IIF(SalePrice != System.Decimal.Zero , Localization.DecimalStringForDB(SalePrice) , "NULL") + ",");
						sql.Append(Common.IIF(MSRP != System.Decimal.Zero , Localization.DecimalStringForDB(MSRP) , "NULL") + ",");
						sql.Append(Common.IIF(Cost != System.Decimal.Zero , Localization.DecimalStringForDB(Cost) , "NULL") + ",");
						sql.Append(Localization.IntStringForDB(Points) + ",");
						sql.Append(Common.IIF(MinimumQuantity != 0 , Localization.IntStringForDB(MinimumQuantity) , "NULL") + ",");
						sql.Append(Common.IIF(ShippingCost != System.Decimal.Zero , Localization.DecimalStringForDB(ShippingCost) , "NULL") + ",");
						if(Common.Form("SKUSuffix").Length != 0)
						{
							sql.Append(DB.SQuote(Common.Form("SKUSuffix")) + ",");
						}
						else
						{
							sql.Append("NULL,");
						}
						if(Common.Form("ManufacturerPartNumber").Length != 0)
						{
							sql.Append(DB.SQuote(Common.Form("ManufacturerPartNumber")) + ",");
						}
						else
						{
							sql.Append("NULL,");
						}
						sql.Append(Common.IIF(Common.FormUSSingle("Weight") != 0.0F , Localization.SingleStringForDB(Common.FormUSSingle("Weight")) , "NULL") + ",");
						if(Common.Form("Dimensions").Length != 0)
						{
							sql.Append(DB.SQuote(Common.Form("Dimensions")) + ",");
						}
						else
						{
							sql.Append("NULL,");
						}
						sql.Append(Common.IIF(Common.Form("Inventory").Length != 0 , Common.Form("Inventory") , "1000000") + ",");
						sql.Append(Common.FormUSInt("SubscriptionMonths").ToString() + ",");
						sql.Append(Common.FormUSInt("Published").ToString() + ",");
						sql.Append(Common.FormUSInt("IsRecurring").ToString() + ",");
						sql.Append(Common.FormUSInt("RecurringInterval").ToString() + ",");
						sql.Append(Common.FormUSInt("RecurringIntervalType").ToString() + ",");
						sql.Append(DB.SQuote(Common.Form("Colors").Replace(", ",",").Replace(" ,",",")) + ",");
						sql.Append(DB.SQuote(Common.Form("ColorSKUModifiers").Replace(", ",",").Replace(" ,",",")) + ",");
						sql.Append(DB.SQuote(Common.Form("Sizes").Replace(", ",",").Replace(" ,",",")) + ",");
						sql.Append(DB.SQuote(Common.Form("SizeSKUModifiers").Replace(", ",",").Replace(" ,",",")) + ",");
						//sql.Append(DB.SQuote(Common.Form("Sizes2").Replace(", ",",").Replace(" ,",",")) + ",");
						//sql.Append(DB.SQuote(Common.Form("SizeSKUModifiers2").Replace(", ",",").Replace(" ,",",")) + ",");
						sql.Append(Common.Form("QuantityDiscountID") + ",");
						sql.Append(Common.Form("IsTaxable") + ",");
						sql.Append(Common.Form("IsShipSeparately") + ",");
						sql.Append(Common.Form("IsDownload") + ",");
						sql.Append(Common.Form("IsSecureAttachment") + ",");
						String DLoc = Common.Form("DownloadLocation");
						if(DLoc.StartsWith("/"))
						{
							DLoc = DLoc.Substring(1,DLoc.Length - 1); // remove leading / char!
						}
						sql.Append(DB.SQuote(DLoc) + ",");
						sql.Append(thisCustomer._customerID);
						sql.Append(")");
						DB.ExecuteSQL(sql.ToString());

						rs = DB.GetRS("select VariantID from productvariant  " + DB.GetNoLock() + " where deleted=0 and VariantGUID=" + DB.SQuote(NewGUID));
						rs.Read();
						VariantID = DB.RSFieldInt(rs,"VariantID");
						Editing = true;
						rs.Close();
						DataUpdated = true;
					}
					else
					{
						// ok to update:
						sql.Append("update productvariant set ");
						sql.Append("ProductID=" + ProductID.ToString() + ",");
						sql.Append("Name=" + DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
						if(Common.Form("Description").Length != 0)
						{
							sql.Append("Description=" + DB.SQuote(Common.Form("Description")) + ",");
						}
						else
						{
							sql.Append("Description=NULL,");
						}
						if(Common.Form("RestrictedQuantities").Length != 0)
						{
							sql.Append("RestrictedQuantities=" + DB.SQuote(Common.Form("RestrictedQuantities")) + ",");
						}
						else
						{
							sql.Append("RestrictedQuantities=NULL,");
						}
						if(Common.Form("FroogleDescription").Length != 0)
						{
							sql.Append("FroogleDescription=" + DB.SQuote(Common.Form("FroogleDescription")) + ",");
						}
						else
						{
							sql.Append("FroogleDescription=NULL,");
						}
						sql.Append("Price=" + Localization.DecimalStringForDB(Price) + ",");
						sql.Append("SalePrice=" + Common.IIF(SalePrice != System.Decimal.Zero , Localization.DecimalStringForDB(SalePrice) , "NULL") + ",");
						sql.Append("MSRP=" + Common.IIF(MSRP != System.Decimal.Zero , Localization.DecimalStringForDB(MSRP) , "NULL") + ",");
						sql.Append("Cost=" + Common.IIF(Cost != System.Decimal.Zero , Localization.DecimalStringForDB(Cost) , "NULL") + ",");
						sql.Append("Points=" + Localization.IntStringForDB(Points) + ",");
						sql.Append("MinimumQuantity=" + Common.IIF(MinimumQuantity != 0 , Localization.IntStringForDB(MinimumQuantity) , "NULL") + ",");
						sql.Append("ShippingCost=" + Common.IIF(ShippingCost != System.Decimal.Zero , Localization.DecimalStringForDB(ShippingCost) , "NULL") + ",");
						if(Common.Form("SKUSuffix").Length != 0)
						{
							sql.Append("SKUSuffix=" + DB.SQuote(Common.Form("SKUSuffix")) + ",");
						}
						else
						{
							sql.Append("SKUSuffix=NULL,");
						}
						if(Common.Form("ManufacturerPartNumber").Length != 0)
						{
							sql.Append("ManufacturerPartNumber=" + DB.SQuote(Common.Form("ManufacturerPartNumber")) + ",");
						}
						else
						{
							sql.Append("ManufacturerPartNumber=NULL,");
						}
						sql.Append("Weight=" + Common.IIF(Common.FormUSSingle("Weight") != 0.0F , Localization.SingleStringForDB(Common.FormUSSingle("Weight")) , "NULL") + ",");
						if(Common.Form("Dimensions").Length != 0)
						{
							sql.Append("Dimensions=" + DB.SQuote(Common.Form("Dimensions")) + ",");
						}
						else
						{
							sql.Append("Dimensions=NULL,");
						}
						sql.Append("Inventory=" + Common.IIF(Common.Form("Inventory").Length != 0 , Common.Form("Inventory") , "1000000") + ",");
						sql.Append("SubscriptionMonths=" + Common.FormUSInt("SubscriptionMonths").ToString() + ",");
						sql.Append("Published=" + Common.FormUSInt("Published").ToString() + ",");
						sql.Append("IsRecurring=" + Common.FormUSInt("IsRecurring").ToString() + ",");
						sql.Append("RecurringInterval=" + Common.FormUSInt("RecurringInterval").ToString() + ",");
						sql.Append("RecurringIntervalType=" + Common.FormUSInt("RecurringIntervalType").ToString() + ",");
						sql.Append("Colors=" + DB.SQuote(Common.Form("Colors").Replace(", ",",").Replace(" ,",",")) + ",");
						sql.Append("ColorSKUModifiers=" + DB.SQuote(Common.Form("ColorSKUModifiers").Replace(", ",",").Replace(" ,",",")) + ",");
						sql.Append("Sizes=" + DB.SQuote(Common.Form("Sizes").Replace(", ",",").Replace(" ,",",")) + ",");
						sql.Append("SizeSKUModifiers=" + DB.SQuote(Common.Form("SizeSKUModifiers").Replace(", ",",").Replace(" ,",",")) + ",");
						//sql.Append("Sizes2=" + DB.SQuote(Common.Form("Sizes2").Replace(", ",",").Replace(" ,",",")) + ",");
						//sql.Append("SizeSKUModifiers2=" + DB.SQuote(Common.Form("SizeSKUModifiers2").Replace(", ",",").Replace(" ,",",")) + ",");
						sql.Append("QuantityDiscountID=" + Common.Form("QuantityDiscountID") + ",");
						sql.Append("IsTaxable=" + Common.Form("IsTaxable") + ",");
						sql.Append("IsShipSeparately=" + Common.Form("IsShipSeparately") + ",");
						sql.Append("IsDownload=" + Common.Form("IsDownload") + ",");
						sql.Append("IsSecureAttachment=" + Common.Form("IsSecureAttachment") + ",");
						String DLoc = Common.Form("DownloadLocation");
						if(DLoc.StartsWith("/"))
						{
							DLoc = DLoc.Substring(1,DLoc.Length - 1); // remove leading / char!
						}
						sql.Append("DownloadLocation=" + DB.SQuote(DLoc) + ",");
						sql.Append("LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
						sql.Append("LastUpdatedBy=" + thisCustomer._customerID.ToString() + " ");
						sql.Append("where VariantID=" + VariantID.ToString());
						DB.ExecuteSQL(sql.ToString());
						DataUpdated = true;
						Editing = true;
					}

					// handle image uploaded:
					try
					{
						String Image1 = String.Empty;
						HttpPostedFile Image1File = Request.Files["Image1"];
						if(Image1File.ContentLength != 0)
						{
							// delete any current image file first
							try
							{
								System.IO.File.Delete(Common.GetImagePath("Variant","icon",true) + VariantID.ToString() + ".jpg");
								System.IO.File.Delete(Common.GetImagePath("Variant","icon",true) + VariantID.ToString() + ".gif");
								System.IO.File.Delete(Common.GetImagePath("Variant","icon",true) + VariantID.ToString() + ".png");
							}
							catch
							{}

							String s = Image1File.ContentType;
							switch(Image1File.ContentType)
							{
								case "image/gif":
									Image1 = Common.GetImagePath("Variant","icon",true) + VariantID.ToString() + ".gif";
									Image1File.SaveAs(Image1);
									break;
								case "image/x-png":
									Image1 = Common.GetImagePath("Variant","icon",true) + VariantID.ToString() + ".png";
									Image1File.SaveAs(Image1);
									break;
								case "image/jpeg":
								case "image/pjpeg":
									Image1 = Common.GetImagePath("Variant","icon",true) + VariantID.ToString() + ".jpg";
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
								System.IO.File.Delete(Common.GetImagePath("Variant","medium",true) + VariantID.ToString() + ".jpg");
								System.IO.File.Delete(Common.GetImagePath("Variant","medium",true) + VariantID.ToString() + ".gif");
								System.IO.File.Delete(Common.GetImagePath("Variant","medium",true) + VariantID.ToString() + ".png");
							}
							catch
							{}

							String s = Image2File.ContentType;
							switch(Image2File.ContentType)
							{
								case "image/gif":
									Image2 = Common.GetImagePath("Variant","medium",true) + VariantID.ToString() + ".gif";
									Image2File.SaveAs(Image2);
									break;
								case "image/x-png":
									Image2 = Common.GetImagePath("Variant","medium",true) + VariantID.ToString() + ".png";
									Image2File.SaveAs(Image2);
									break;
								case "image/jpeg":
								case "image/pjpeg":
									Image2 = Common.GetImagePath("Variant","medium",true) + VariantID.ToString() + ".jpg";
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
								System.IO.File.Delete(Common.GetImagePath("Variant","large",true) + VariantID.ToString() + ".jpg");
								System.IO.File.Delete(Common.GetImagePath("Variant","large",true) + VariantID.ToString() + ".gif");
								System.IO.File.Delete(Common.GetImagePath("Variant","large",true) + VariantID.ToString() + ".png");
							}
							catch
							{}

							String s = Image3File.ContentType;
							switch(Image3File.ContentType)
							{
								case "image/gif":
									Image3 = Common.GetImagePath("Variant","large",true) + VariantID.ToString() + ".gif";
									Image3File.SaveAs(Image3);
									break;
								case "image/x-png":
									Image3 = Common.GetImagePath("Product","large",true) + VariantID.ToString() + ".png";
									Image3File.SaveAs(Image3);
									break;
								case "image/jpeg":
								case "image/pjpeg":
									Image3 = Common.GetImagePath("Variant","large",true) + VariantID.ToString() + ".jpg";
									Image3File.SaveAs(Image3);
									break;
							}
						}


					}
					catch(Exception ex)
					{
						ErrorMsg = Common.GetExceptionDetail(ex,"<br>");
					}

					String LargeImage1 = Common.GetImagePath("Variant","large",true) + VariantID.ToString() + ".gif";
					String LargeImage2 = Common.GetImagePath("Variant","large",true) + VariantID.ToString() + ".jpg";
					String LargeImage3 = Common.GetImagePath("Variant","large",true) + VariantID.ToString() + ".png";

//				}
//				catch(Exception ex)
//				{
//					ErrorMsg = "<p><b>ERROR: " + Common.GetExceptionDetail(ex,"<br>") + "<br><br></b></p>";
//				}
			}
			SectionTitle = "<a href=\"variants.aspx?productid=" + ProductID.ToString() + "\">Variants</a> - Manage Variants";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			IDataReader rs = DB.GetRS("select PV.*,P.ColorOptionPrompt,P.SizeOptionPrompt from Product P " + DB.GetNoLock() + " , productvariant PV  " + DB.GetNoLock() + " where PV.ProductID=P.ProductID and VariantID=" + VariantID.ToString());
			if(rs.Read())
			{
				Editing = true;
			}

			String ColorOptionPrompt = DB.RSField(rs,"ColorOptionPrompt");
			String SizeOptionPrompt = DB.RSField(rs,"SizeOptionPrompt");
			if(ColorOptionPrompt.Length == 0)
			{
				ColorOptionPrompt = Common.AppConfig("ColorOptionPrompt");
			}
			if(SizeOptionPrompt.Length == 0)
			{
				SizeOptionPrompt = Common.AppConfig("SizeOptionPrompt");
			}
			
			if(ErrorMsg.Length != 0)
			{
				writer.Write("<p align=\"left\"><b><font color=red>" + ErrorMsg + "</font></b></p>\n");
			}
			if(DataUpdated)
			{
				writer.Write("<p align=\"left\"><b><font color=blue>(UPDATED)</font></b></p>\n");
			}


			if(ErrorMsg.Length == 0)
			{

				writer.Write("<p align=\"left\"><b>Within Product: <a href=\"editproduct.aspx?productid=" + ProductID.ToString() + "\">" + Common.GetProductName(ProductID) + "</a> (Product SKU=" + Common.GetProductSKU(ProductID) + ", ProductID=" + ProductID.ToString() + ")</b</p>\n");
				if(Editing)
				{
					writer.Write("<p align=\"left\"><b>Editing Variant: " + DB.RSField(rs,"Name") + " (Variant SKUSuffix=" + DB.RSField(rs,"SKUSuffix") + ", VariantID=" + DB.RSFieldInt(rs,"VariantID").ToString() + ")</b></p>\n");
				}
				else
				{
					writer.Write("<p align=\"left\"><b>Adding New Variant:</p></b>\n");
				}

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function Form_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("submitonce(theForm);\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				writer.Write("<p align=\"left\">Please enter the following information about this variant. Fields marked with an asterisk (*) are required. All other fields are optional.</p>\n");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\">\n");
				writer.Write("<form enctype=\"multipart/form-data\" action=\"editvariant.aspx?productid=" + ProductID.ToString() + "&VariantID=" + VariantID.ToString() + "&edit=" + Editing.ToString() + "\" method=\"post\" id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
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
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Variant Name:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"Name\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Name")) , "") + "\">\n");
				//writer.Write("                	<input type=\"hidden\" name=\"Name_vldt\" value=\"[req][blankalert=Please enter the variant name]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">SKU Suffix:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"50\" size=\"30\" name=\"SKUSuffix\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"SKUSuffix")) , "") + "\">\n");
				//writer.Write("                	<input type=\"hidden\" name=\"SKUSuffix_vldt\" value=\"[req][blankalert=Please enter the variant SKU Suffix]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Manufacturer Part #:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"50\" size=\"30\" name=\"ManufacturerPartNumber\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"ManufacturerPartNumber")) , "") + "\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\" bgcolor=\"" + Common.IIF(Editing && !DB.RSFieldBool(rs,"Published") , "#Fe5888" , "FFFFFF") + "\">*Published:&nbsp;</td>\n");
				writer.Write("                <td align=\"left\" bgcolor=\"" + Common.IIF(Editing && !DB.RSFieldBool(rs,"Published") , "#Fe5888" , "FFFFFF") + "\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"Published\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"Published") , " checked " , "") , " checked ") + ">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"Published\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"Published") , "" , " checked ") , "") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Is Recurring:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\" >\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"IsRecurring\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"IsRecurring") , " checked " , "") , "") + ">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"IsRecurring\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"IsRecurring") , "" , " checked ") , " checked ") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Recurring Interval:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"10\" size=\"10\" name=\"RecurringInterval\" value=\"" + Common.IIF(Editing , DB.RSFieldInt(rs,"RecurringInterval").ToString() , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"RecurringInterval_vldt\" value=\"[number][invalidalert=Please enter the recurring interval length for of this variant, as an integer, e.g. 1]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Recurring Interval Type:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\" >\n");
				writer.Write("Days&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"RecurringIntervalType\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldInt(rs,"RecurringIntervalType") == 1 , " checked " , "") , "") + ">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;\n");
				writer.Write("Weeks&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"RecurringIntervalType\" value=\"2\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldInt(rs,"RecurringIntervalType") == 2 , " checked " , "") , "") + ">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;\n");
				writer.Write("Months&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"RecurringIntervalType\" value=\"3\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldInt(rs,"RecurringIntervalType") == 3 , " checked " , "") , " checked ") + ">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;\n");
				writer.Write("Years&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"RecurringIntervalType\" value=\"4\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldInt(rs,"RecurringIntervalType") == 4 , " checked " , "") , "") + ">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Description (HTML OK):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<textarea style=\"height: 60em; width: 100%;\" cols=\"" + Common.AppConfig("Admin_TextareaWidth") + "\" rows=\"" + Common.AppConfig("Admin_TextareaHeightl") + "\" id=\"Description\" name=\"Description\">" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Description")) , "") + "</textarea>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Froogle Description (NO HTML):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<textarea style=\"height: 60em; width: 100%;\" cols=\"" + Common.AppConfig("Admin_TextareaWidth") + "\" rows=\"" + Common.AppConfig("Admin_TextareaHeightl") + "\" id=\"FroogleDescription\" name=\"FroogleDescription\">" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"FroogleDescription")) , "") + "</textarea>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				int NumCustomerLevels = DB.GetSqlN("select count(*) as N from CustomerLevel  " + DB.GetNoLock() + " where deleted=0");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Restricted Quantities:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"255\" size=\"100\" name=\"RestrictedQuantities\" value=\"" + Common.IIF(Editing ,DB.RSField(rs,"RestrictedQuantities") , "") + "\"> (quantities allowed, e.g. 5, 10, 15, 20, 25)\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Minimum Quantity:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"10\" size=\"10\" name=\"MinimumQuantity\" value=\"" + Common.IIF(Editing , Common.IIF(DB.RSFieldInt(rs,"MinimumQuantity") != 0 , DB.RSFieldInt(rs,"MinimumQuantity").ToString() , "") , "") + "\"> (leave blank for no mininimum)\n");
				writer.Write("                	<input type=\"hidden\" name=\"MinimumQuantity_vldt\" value=\"[number][invalidalert=Please enter a valid integer number, e.g. 250!]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Price:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"10\" size=\"10\" name=\"Price\" value=\"" + Common.IIF(Editing , (Localization.CurrencyStringForDB( DB.RSFieldDecimal(rs,"Price"))) , "") + "\"> (in format x.xx)");
				if(NumCustomerLevels > 0)
				{
					writer.Write("&nbsp;&nbsp;<a href=\"editextendedprices.aspx?ProductID=" + ProductID.ToString() + "&VariantID=" + VariantID.ToString() + "\">Define Extended Prices</a> <small>(Defined By Customer Level)</small>\n");
				}
				writer.Write("                	<input type=\"hidden\" name=\"Price_vldt\" value=\"[req][number][blankalert=Please enter the variant price][invalidalert=Please enter a valid dollar amount, e.g. 10.00 without the leading $ sign!]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Sale Price:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"10\" size=\"10\" name=\"SalePrice\" value=\"" + Common.IIF(Editing , Common.IIF(DB.RSFieldDecimal(rs,"SalePrice") != System.Decimal.Zero , Localization.CurrencyStringForDB( DB.RSFieldDecimal(rs,"SalePrice")) , "") , "") + "\"> (in format x.xx)\n");
				writer.Write("                	<input type=\"hidden\" name=\"SalePrice_vldt\" value=\"[number][invalidalert=Please enter a valid dollar amount, e.g. 10.00 without the leading $ sign!]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">MSRP:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"10\" size=\"10\" name=\"MSRP\" value=\"" + Common.IIF(Editing , Common.IIF(DB.RSFieldDecimal(rs,"MSRP") != System.Decimal.Zero , Localization.CurrencyStringForDB( DB.RSFieldDecimal(rs,"MSRP")) , "") , "") + "\"> (in format x.xx)\n");
				writer.Write("                	<input type=\"hidden\" name=\"MSRP_vldt\" value=\"[number][invalidalert=Please enter a valid dollar amount, e.g. 10.00 without the leading $ sign!]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Actual Cost:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"10\" size=\"10\" name=\"Cost\" value=\"" + Common.IIF(Editing , Common.IIF(DB.RSFieldDecimal(rs,"Cost") != System.Decimal.Zero , Localization.CurrencyStringForDB( DB.RSFieldDecimal(rs,"Cost")) , "") , "") + "\"> (in format x.xx, never displayed on site)\n");
				writer.Write("                	<input type=\"hidden\" name=\"Cost_vldt\" value=\"[number][invalidalert=Please enter a valid dollar amount, e.g. 10.00 without the leading $ sign!]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Points:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"10\" size=\"10\" name=\"Points\" value=\"" + Common.IIF(Editing , Common.IIF(DB.RSFieldInt(rs,"Points") != 0 , DB.RSFieldInt(rs,"Points").ToString() , "") , "") + "\"> (simple integer, only used if using Points Program)\n");
				writer.Write("                	<input type=\"hidden\" name=\"Points_vldt\" value=\"[number][invalidalert=Please enter a valid integer number, e.g. 250!]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Shipping Cost:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"10\" size=\"10\" name=\"ShippingCost\" value=\"" + Common.IIF(Editing , Common.IIF(DB.RSFieldDecimal(rs,"ShippingCost") != System.Decimal.Zero , Localization.CurrencyStringForDB( DB.RSFieldDecimal(rs,"ShippingCost")) , "") , "") + "\">  (in format x.xx, and only used if using individual item shipping costs)\n");
				writer.Write("                	<input type=\"hidden\" name=\"ShippingCost_vldt\" value=\"[number][invalidalert=Please enter a valid dollar amount, e.g. 10.00 without the leading $ sign!]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("<tr valign=\"middle\">\n");
				writer.Write("<td align=\"right\" valign=\"middle\">Quantity Discount Table:&nbsp;&nbsp;</td>\n");
				writer.Write("<td align=\"left\">\n");
				writer.Write("<select size=\"1\" name=\"QuantityDiscountID\">\n");
				writer.Write("<option value=\"0\">None</option>");
				IDataReader rsst = DB.GetRS("select * from QuantityDiscount  " + DB.GetNoLock() + " order by name");
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
				writer.Write("                <td align=\"right\" valign=\"middle\">*Is Taxable:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"IsTaxable\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"IsTaxable") , " checked " , "") , " checked ") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"IsTaxable\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"IsTaxable") , "" , " checked ") , "") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Is Ship Separately:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"IsShipSeparately\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"IsShipSeparately") , " checked " , "") , "") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"IsShipSeparately\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"IsShipSeparately") , "" , " checked ") , " checked ") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Is Download:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"IsDownload\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"IsDownload") , " checked " , "") , "  ") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"IsDownload\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"IsDownload") , "" , " checked ") , " checked ") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Download Location:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"1000\" size=\"100\" name=\"DownloadLocation\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"DownloadLocation")) , "") + "\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">Is Secure Attachment:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"IsSecureAttachment\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"IsSecureAttachment") , " checked " , "") , "  ") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"IsSecureAttachment\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"IsSecureAttachment") , "" , " checked ") , " checked ") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Weight:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"10\" size=\"10\" name=\"Weight\" value=\"" + Common.IIF(Editing , Localization.SingleStringForDB(DB.RSFieldSingle(rs,"Weight")) , "") + "\"> <small>(in format x.xx, in " + Localization.WeightUnits() + ")</small>\n");
				writer.Write("                	<input type=\"hidden\" name=\"Weight_vldt\" value=\"[number][invalidalert=Please enter the weight of this item in " + Localization.WeightUnits() + ", e.g. 2.5]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Dimensions:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"50\" size=\"30\" name=\"Dimensions\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Dimensions")) , "") + "\"> MUST be in format: N.NN x N.NN x N.NN, Height x Width x Depth, in inches, e.g. 4.5 x 7.8 x 2\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				if(ProductUsesAdvancedInventoryMgmt)
				{
					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Manage Inventory:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("                	<a href=\"editinventory.aspx?productid=" + ProductID.ToString() + "&variantid=" + VariantID.ToString() + "\">Click Here</a>\n");
					writer.Write("                	</td>\n");
					writer.Write("              </tr>\n");
				}
				else
				{
					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Current Inventory:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("                	<input maxLength=\"10\" size=\"10\" name=\"Inventory\" value=\"" + Common.IIF(Editing , DB.RSFieldInt(rs,"Inventory").ToString() , "") + "\">\n");
					writer.Write("                	<input type=\"hidden\" name=\"Inventory_vldt\" value=\"[number][invalidalert=Please enter the current inventory in stock for this item, e.g. 100]\">\n");
					writer.Write("                	</td>\n");
					writer.Write("              </tr>\n");
				}

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Subscription Months:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"10\" size=\"10\" name=\"SubscriptionMonths\" value=\"" + Common.IIF(Editing , DB.RSFieldInt(rs,"SubscriptionMonths").ToString() , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"SubscriptionMonths_vldt\" value=\"[number][invalidalert=Please enter the number of months a customers subscription would be extended by the purchase of this variant]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");


				writer.Write("  <tr>\n");
				writer.Write("    <td valign=\"top\" align=\"right\">Icon:\n");
				writer.Write("</td>\n");
				writer.Write("    <td valign=\"top\" align=\"left\">");
				writer.Write("    <input type=\"file\" name=\"Image1\" size=\"30\" value=\"" + Common.IIF(Editing , "" , "") + "\">\n");
				String Image1URL = Common.LookupImage("Variant",VariantID,"icon",_siteID);
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
				writer.Write("    <input type=\"file\" name=\"Image2\" size=\"30\" value=\"" + Common.IIF(Editing , "" , "") + "\">\n");
				String Image2URL = Common.LookupImage("Variant",VariantID,"medium",_siteID);
				if(Image2URL.Length != 0)
				{
					writer.Write("<a href=\"javascript:void(0);\" onClick=\"DeleteIcon('medium','Pic2');\">Click here</a> to delete the current image<br>\n");
					writer.Write("<br><img id=\"Pic2\" name=\"Pic2\" border=\"0\" src=\"" + Image2URL + "?" + Common.GetRandomNumber(1,1000000).ToString() + "\">\n");
				}
				writer.Write("</td>\n");
				writer.Write(" </tr>\n");

				writer.Write("  <tr>\n");
				writer.Write("    <td valign=\"top\" align=\"right\">Large Pic:\n");
				writer.Write("</td>\n");
				writer.Write("    <td valign=\"top\" align=\"left\">");
				writer.Write("    <input type=\"file\" name=\"Image3\" size=\"30\" value=\"" + Common.IIF(Editing , "" , "") + "\">\n");
				String Image3URL = Common.LookupImage("Variant",VariantID,"large",_siteID);
				if(Image3URL.Length != 0)
				{
					writer.Write("<a href=\"javascript:void(0);\" onClick=\"DeleteIcon('large','Pic3');\">Click here</a> to delete the current image<br>\n");
					writer.Write("<br><img id=\"Pic3\" name=\"Pic3\" border=\"0\" src=\"" + Image3URL + "?" + Common.GetRandomNumber(1,1000000).ToString() + "\">\n");
				}
				writer.Write("</td>\n");
				writer.Write(" </tr>\n");

				// COLORS & SIZES:
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">" + ColorOptionPrompt + "(s):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"750\" size=\"50\" name=\"Colors\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Colors")) , "") + "\">&nbsp;<small>(Separate " + ColorOptionPrompt.ToLower() + "(s) by commas)</small>\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">" + ColorOptionPrompt + " SKU Modifiers:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"750\" size=\"50\" name=\"ColorSKUModifiers\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"ColorSKUModifiers")) , "") + "\">&nbsp;<small>(Separate skus by commas)</small>\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">" + SizeOptionPrompt + "(s):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"750\" size=\"50\" name=\"Sizes\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Sizes")) , "") + "\">&nbsp;<small>(Separate " + SizeOptionPrompt.ToLower() + "(s) by commas)</small>\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">" + SizeOptionPrompt + " SKU Modifiers:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"750\" size=\"50\" name=\"SizeSKUModifiers\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"SizeSKUModifiers")) , "") + "\">&nbsp;<small>(Separate skus by commas)</small>\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

//				writer.Write("              <tr valign=\"middle\">\n");
//				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">" + SizeOptionPrompt + " 2(s):&nbsp;&nbsp;</td>\n");
//				writer.Write("                <td align=\"left\">\n");
//				writer.Write("                	<input maxLength=\"750\" size=\"50\" name=\"Sizes2\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Sizes2")) , "") + "\">&nbsp;<small>(Separate " + SizeOptionPrompt.ToLower() + "(s) by commas)</small>\n");
//				writer.Write("                	</td>\n");
//				writer.Write("              </tr>\n");
//
//				writer.Write("              <tr valign=\"middle\">\n");
//				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">" + SizeOptionPrompt + " SKU Modifiers 2:&nbsp;&nbsp;</td>\n");
//				writer.Write("                <td align=\"left\">\n");
//				writer.Write("                	<input maxLength=\"750\" size=\"50\" name=\"SizeSKUModifiers2\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"SizeSKUModifiers2")) , "") + "\">&nbsp;<small>(Separate skus by commas)</small>\n");
//				writer.Write("                	</td>\n");
//				writer.Write("              </tr>\n");

				// size/color tables for display purposes only:
				if(Editing && (DB.RSField(rs,"Colors").Length != 0 || DB.RSField(rs,"ColorSKUModifiers").Length != 0 || DB.RSField(rs,"Sizes").Length != 0 || DB.RSField(rs,"SizeSKUModifiers").Length != 0))
				{
					writer.Write("<tr valign=\"left\"><td colspan=\"2\" height=\"10\"></td></tr>\n");
					writer.Write("<tr valign=\"left\">\n");
					writer.Write("<td width=\"25%\" align=\"right\" valign=\"top\">" + ColorOptionPrompt + "/" + SizeOptionPrompt + " Tables:&nbsp;&nbsp;<br><small>(summary)</small></td>\n");
					writer.Write("<td align=\"left\">\n");

					String[] Colors = DB.RSField(rs,"Colors").Split(',');
					String[] Sizes = DB.RSField(rs,"Sizes").Split(',');
					String[] ColorSKUModifiers = DB.RSField(rs,"ColorSKUModifiers").Split(',');
					String[] SizeSKUModifiers = DB.RSField(rs,"SizeSKUModifiers").Split(',');

					for(int i = Colors.GetLowerBound(0); i <= Colors.GetUpperBound(0); i++)
					{
						Colors[i] = Colors[i].Trim();
					}

					for(int i = Sizes.GetLowerBound(0); i <= Sizes.GetUpperBound(0); i++)
					{
						Sizes[i] = Sizes[i].Trim();
					}

					for(int i = ColorSKUModifiers.GetLowerBound(0); i <= ColorSKUModifiers.GetUpperBound(0); i++)
					{
						ColorSKUModifiers[i] = ColorSKUModifiers[i].Trim();
					}

					for(int i = SizeSKUModifiers.GetLowerBound(0); i <= SizeSKUModifiers.GetUpperBound(0); i++)
					{
						SizeSKUModifiers[i] = SizeSKUModifiers[i].Trim();
					}

					int ColorCols = Colors.GetUpperBound(0);
					int SizeCols = Sizes.GetUpperBound(0);
					ColorCols = Math.Max(ColorCols,ColorSKUModifiers.GetUpperBound(0));
					SizeCols = Math.Max(SizeCols,SizeSKUModifiers.GetUpperBound(0));

					if(DB.RSField(rs,"Colors").Length != 0 || DB.RSField(rs,"ColorSKUModifiers").Length != 0)
					{
						writer.Write("<table cellpadding=\"2\" cellspacing=\"0\" border=\"1\">\n");
						writer.Write("<tr>\n");
						writer.Write("<td><b>" + ColorOptionPrompt + "</b></td>\n");
						for(int i = 0; i <= ColorCols; i++)
						{
							String ColorVal = String.Empty;
							try
							{
								ColorVal = Colors[i];
							}
							catch{}
							if(ColorVal.Length == 0)
							{
								ColorVal = "&nbsp;";
							}
							writer.Write("<td align=\"center\">" + ColorVal + "</td>\n");
						}
						writer.Write("<tr>\n");
						writer.Write("<tr>\n");
						writer.Write("<td><b>SKU Modifier</b></td>\n");
						for(int i = 0; i <= ColorCols; i++)
						{
							String SKUVal = String.Empty;
							try
							{
								SKUVal = ColorSKUModifiers[i];
							}
							catch{}
							if(SKUVal.Length == 0)
							{
								SKUVal = "&nbsp;";
							}
							writer.Write("<td align=\"center\">" + SKUVal + "</td>\n");
						}
						writer.Write("<tr>\n");
						writer.Write("</table>\n");
						writer.Write("<br><br>");
					}


					if(DB.RSField(rs,"Sizes").Length != 0 || DB.RSField(rs,"SizeSKUModifiers").Length != 0)
					{
						writer.Write("<table cellpadding=\"2\" cellspacing=\"0\" border=\"1\">\n");
						writer.Write("<tr>\n");
						writer.Write("<td><b>" + SizeOptionPrompt + "</b></td>\n");
						for(int i = 0; i <= SizeCols; i++)
						{
							String SizeVal = String.Empty;
							try
							{
								SizeVal = Sizes[i];
							}
							catch{}
							if(SizeVal.Length == 0)
							{
								SizeVal = "&nbsp;";
							}
							writer.Write("<td align=\"center\">" + SizeVal + "</td>\n");
						}
						writer.Write("<tr>\n");
						writer.Write("<tr>\n");
						writer.Write("<td><b>SKU Modifier</b></td>\n");
						for(int i = 0; i <= SizeCols; i++)
						{
							String SKUVal = String.Empty;
							try
							{
								SKUVal = SizeSKUModifiers[i];
							}
							catch{}
							if(SKUVal.Length == 0)
							{
								SKUVal = "&nbsp;";
							}
							writer.Write("<td align=\"center\">" + SKUVal + "</td>\n");
						}
						writer.Write("<tr>\n");
						writer.Write("</table>\n");
					}

					writer.Write("</td>\n");
					writer.Write("</tr>\n");
				}


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
				writer.Write("</form>\n");
				writer.Write("  </table>\n");

				writer.Write(Common.GenerateHtmlEditor("Description"));

				writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
				writer.Write("function DeleteIcon(size,name)\n");
				writer.Write("{\n");
				writer.Write("window.open('deleteicon.aspx?VariantID=" + VariantID.ToString() + "&FormImageName=' + name + '&size=' + size,\"alikaussadmin_ML\",\"height=250,width=440,top=10,left=20,status=no,toolbar=no,menubar=no,scrollbars=yes,location=no\")\n");
				writer.Write("}\n");
				writer.Write("</SCRIPT>\n");

			}
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
