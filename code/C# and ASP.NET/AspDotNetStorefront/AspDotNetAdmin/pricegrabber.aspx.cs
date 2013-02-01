// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for pricegrabber.
	/// </summary>
	public class pricegrabber : System.Web.UI.Page
	{

		private decimal GetWeightCost(Single wt)
		{
			if(wt == 0.0F)
			{
				return System.Decimal.Zero;
			}
			DataSet ds = DB.GetDS("select min(ShippingCharge) as ShippingCharge,LowValue,HighValue from shippingbyweight  " + DB.GetNoLock() + " group by LowValue,HighValue order by LowValue ",true);
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				Single LowV = DB.RowFieldSingle(row,"LowValue");
				Single HighV = DB.RowFieldSingle(row,"HighValue");
				if(wt == 0.0F)
				{
					ds.Dispose();
					return System.Decimal.Zero;
				}
				if(wt >= LowV && wt <= HighV)
				{
					ds.Dispose();
					return DB.RowFieldDecimal(row,"ShippingCharge");
				}

			}
			ds.Dispose();
			return System.Decimal.Zero;
		}

		private String PGField(String s)
		{
			s = s.Replace("\"","\"\"");
			if(s.IndexOf(",") != -1)
			{
				s = "\"" + s + "\"";
			}
			return s;
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");
			Server.ScriptTimeout = 1000;
			
			Customer thisCustomer = new Customer();
			
			Response.Write("<html><head><title>PriceGrabber Feed</title></head><body>\n");

			String FN = Server.MapPath("../images/pricegrabber.csv");

			int PGAffId = Common.GetAffiliateID("PriceGrabber");
			String PGTrackingSuffix = String.Empty;
			if(PGAffId != 0)
			{
				PGTrackingSuffix = "&affiliateid=" + PGAffId.ToString();
			}

			if(thisCustomer._isAdminUser)
			{
				//				if(Common.QueryString("submit").ToLower() == "true")
				//				{
				//					try
				//					{
				//						KCommon.Net.FTP.Session ftp = new Session();
				//						int Port = Common.AppConfigUSInt("PriceGrabber_FTPPort");
				//						if(Port == 0)
				//						{
				//							Port = 21;
				//						}
				//						ftp.Port = Port;
				//						ftp.Server = Common.AppConfig("PriceGrabber_FTPServer");
				//						Response.Write("FTP Connecting...");
				//						Response.Flush();
				//						ftp.Connect(Common.AppConfig("PriceGrabber	_Username"),Common.AppConfig("PriceGrabber_Password"));
				//						Response.Write("ok<br>");
				//						Response.Flush();
				//						if(ftp.IsConnected)
				//						{
				//							Response.Write("FTP Uploading File: " + FN + "...");
				//							Response.Flush();
				//							ftp.CurrentDirectory.PutFile(FN);
				//							Response.Write("ok<br>");
				//							Response.Flush();
				//						}
				//						Response.Write("FTP Closing...");
				//						Response.Flush();
				//						ftp.Close();
				//						Response.Write("ok<br>");
				//						Response.Flush();
				//						Response.Write("Upload Completed<br><br>\n");
				//						Response.Write("<a href=\"javascript:self.close();\">Close</a>");
				//					}
				//					catch(Exception ex)
				//					{
				//						Response.Write("<font color=red><b>" + Common.GetExceptionDetail(ex,"<br>") + "</b></font><br><br>");
				//					}
				//				}
				//				else if(Common.AppConfig("PriceGrabber_Username").Length == 0 || Common.AppConfig("PriceGrabber_Password").Length == 0)
				//				{
				//					Response.Write("You must sign-up for a PriceGrabber account first. Then enter your username and password in AppConfig:PriceGrabber_Username and AppConfig:PriceGrabber_Password variables respectively!<br>\n");
				//				}
				//				else
				//				{

				StringBuilder tmpS = new StringBuilder(50000);
				String sql = "SELECT SalesPrompt.name as SDescription, Product.ProductID, Product.IsCallToOrder, Product.ProductTypeID, Product.RelatedProducts, Product.HidePriceUntilCart, Product.Name, Product.SEName, Product.SpecTitle, Product.SpecsInline, Product.SpecCall, Product.ProductDisplayFormatID, Product.ColWidth, Product.Summary, Product.Description, Product.SEKeywords, Product.SEDescription, Product.SKU, Product.ManufacturerID, Product.ManufacturerPartNumber, Product.Published, Product.Deleted, ProductVariant.VariantID, ProductVariant.FroogleDescription, ProductVariant.Name AS VariantName, ProductVariant.Colors, ProductVariant.Sizes, ProductVariant.Dimensions, ProductVariant.Weight, ProductVariant.Inventory, ProductVariant.SKUSuffix, ProductVariant.ManufacturerPartNumber AS VManufacturerPartNumber, ProductVariant.Price, ProductVariant.SalePrice, ProductVariant.Deleted AS VariantDeleted, ProductVariant.Description as VariantDescription, ProductVariant.Published AS VariantPublished, Manufacturer.Name AS ManufacturerName, Manufacturer.SEName as ManufacturerSEName FROM  ((Product  " + DB.GetNoLock() + " INNER JOIN Manufacturer  " + DB.GetNoLock() + " ON Product.ManufacturerID = Manufacturer.ManufacturerID) left outer join salesprompt  " + DB.GetNoLock() + " on product.salespromptid=salesprompt.salespromptid) LEFT OUTER JOIN ProductVariant  " + DB.GetNoLock() + " ON Product.ProductID = ProductVariant.ProductID WHERE product.published=1 and Product.Deleted=0 and ProductVariant.Published=1 AND ProductVariant.Deleted=0 and (product.excludefrompricefeeds IS NULL or product.excludefrompricefeeds=0) ORDER by Product.Name, ProductVariant.DisplayOrder, ProductVariant.Name";
				IDataReader rs = DB.GetRS(sql);
				String StoreURL = Common.GetStoreHTTPLocation(false);
				while(rs.Read())
				{
					String ImgURL = Common.LookupImage("Variant",DB.RSFieldInt(rs,"VariantID"),"medium",thisCustomer._skinID);
					if(ImgURL.Length == 0)
					{
						ImgURL = Common.LookupImage("Product",DB.RSFieldInt(rs,"productID"),"medium",thisCustomer._skinID);
					}
					if(ImgURL.Length != 0)
					{
						ImgURL = StoreURL + ImgURL; // per google, if no image, leave empty
					}
					ImgURL = ImgURL.Replace("../images","/images");
					String Price = Localization.CurrencyStringForDB(DB.RSFieldDecimal(rs,"Price")); // google only allows US currencies
					if(DB.RSFieldDecimal(rs,"SalePrice") != System.Decimal.Zero)
					{
						Price = Localization.CurrencyStringForDB(DB.RSFieldDecimal(rs,"SalePrice")); // google only allows US currencies
					}
					String CatName = Common.GetFirstProductCategory(DB.RSFieldInt(rs,"ProductID"),false).Replace("\t","").Replace("\n","").Replace("\r","");
					String PName = DB.RSField(rs,"Name");
					if(DB.RSField(rs,"VariantName").Length != 0)
					{
						PName = PName + " - " + DB.RSField(rs,"VariantName");
					}
					PName = Server.HtmlEncode(Common.Left(PName,80).Replace("\t","").Replace("\n","").Replace("\r",""));
					String PDesc = String.Empty;
					if(DB.RSField(rs,"FroogleDescription").Length != 0)
					{
						PDesc = DB.RSField(rs,"FroogleDescription");
					}
					else if(Common.AppConfigBool("Froogle_UseDescriptionsIfFroogleDescEmpty"))
					{
						PDesc = DB.RSField(rs,"Description");
						if(PDesc.Length == 0)
						{
							PDesc = DB.RSField(rs,"Description");
						}
					}
					PDesc = Server.HtmlEncode(Common.Left(PDesc,1000).Replace("\t","").Replace("\n","").Replace("\r",""));
					String ID = (DB.RSFieldInt(rs,"ProductID")*100000).ToString() + (DB.RSFieldInt(rs,"VariantID")*100000).ToString(); // trying to generate unique ids that won't change
					tmpS.Append(PName);
					tmpS.Append("|");
					tmpS.Append(Common.GetManufacturerName(DB.RSFieldInt(rs,"ManufacturerID")));
					tmpS.Append("|");
					String MfgPartNo = DB.RSField(rs,"ManufacturerPartNumber");
					if(MfgPartNo.Length == 0)
					{
						MfgPartNo = DB.RSField(rs,"ManufacturerPartNumber");
					}
					tmpS.Append(PGField(MfgPartNo));
					tmpS.Append("|");
					tmpS.Append(PGField(Price.Replace("$","").Replace(",","")));
					tmpS.Append("|");
					tmpS.Append("yes"); // availability
					tmpS.Append("|");
					tmpS.Append(StoreURL + "showproduct.aspx?productid=" + DB.RSFieldInt(rs,"ProductID").ToString() + PGTrackingSuffix);
					tmpS.Append("|");
					decimal ShpCost = DB.RSFieldDecimal(rs,"ShippingCost");
					Single Wt = DB.RSFieldSingle(rs,"Weight");
					decimal WtCst = GetWeightCost(Wt);
					tmpS.Append(PGField(Localization.DecimalStringForDB(WtCst))); // shipping costs
					tmpS.Append("|");
					tmpS.Append("New"); // product condition
					tmpS.Append("|");
					tmpS.Append("\n");
					// + "\t" + PName + "\t" + PDesc + "\t" + ImgURL + "\t" + CatName + "\t" + Price + "\t" + ID + "\r\n");
				}
				rs.Close();

				Common.WriteFile(FN,tmpS.ToString());

				Response.Write("Your pricegrabber feed data is shown below.");
				Response.Write("<br><br>");
				Response.Write("This file has been saved as: " + Common.GetStoreHTTPLocation(false) + "images/pricegrabber.csv");
				Response.Write("<br><br>");
				Response.Write("Feed Data:<br>\n");
				Response.Write("<hr size=1>");
				Response.Write(tmpS.ToString().Replace("\n","<br>"));
				Response.Write("<hr size=1>");
				//				}
			}
			else
			{
				Response.Write("Permission denied");
			}

			Response.Write("</body></html>\n");
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
