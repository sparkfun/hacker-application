// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using AspDotNetStorefrontCommon;
using KCommon.Net.FTP;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for froogle.
	/// </summary>
	public class froogle : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");
			Server.ScriptTimeout = 1000;
			
			Customer thisCustomer = new Customer();
			
			Response.Write("<html><head><title>Froogle Feed</title></head><body>\n");

			int FroogleAffId = Common.GetAffiliateID("Froogle");
			String FroogleTrackingSuffix = String.Empty;
			if(FroogleAffId != 0)
			{
				FroogleTrackingSuffix = "&affiliateid=" + FroogleAffId.ToString();
			}


			String FN = Server.MapPath("../images/") + Common.AppConfig("Froogle_Username") + ".txt";
			if(Common.AppConfig("Froogle_Filename").Length != 0)
			{
				FN = Server.MapPath("../images/") + Common.AppConfig("Froogle_Filename");
			}
			if(thisCustomer._isAdminUser)
			{
				if(Common.QueryString("submit").ToLower() == "true")
				{
					try
					{
						KCommon.Net.FTP.Session ftp = new Session();
						int Port = Common.AppConfigUSInt("Froogle_FTPPort");
						if(Port == 0)
						{
							Port = 21;
						}
						ftp.Port = Port;
					String Srv = Common.AppConfig("Froogle_FTPServer").Replace("ftp://","").Replace("FTP://","").Replace("ftp:\\\\","").Replace("FTP:\\\\","");
						ftp.Server = Srv;
						Response.Write("FTP Connecting...");
						Response.Flush();
						ftp.Connect(Common.AppConfig("Froogle_Username"),Common.AppConfig("Froogle_Password"));
						Response.Write("ok<br>");
						Response.Flush();
						if(ftp.IsConnected)
						{
							Response.Write("FTP Uploading File: " + FN + "...");
							Response.Flush();
							ftp.CurrentDirectory.PutFile(FN);
							Response.Write("ok<br>");
							Response.Flush();
						}
						Response.Write("FTP Closing...");
						Response.Flush();
						ftp.Close();
						Response.Write("ok<br>");
						Response.Flush();
						Response.Write("Upload Completed<br><br>\n");
					}
					catch(Exception ex)
					{
						Response.Write("<font color=red><b>" + Common.GetExceptionDetail(ex,"<br>") + "</b></font><br><br>");
					}
				}
				else if(Common.AppConfig("Froogle_Username").Length == 0 || Common.AppConfig("Froogle_Password").Length == 0)
				{
					Response.Write("You must sign-up for a froogle feed account with google first. Then enter your username and password in AppConfig:Froogle_Username and AppConfig:Froogle_Password variables respectively!<br>\n");
				}
				else
				{

					StringBuilder tmpS = new StringBuilder(50000);
					tmpS.Append("product_url\tname\tmanufacturer_id\tdescription\timage_url\tcategory\tprice\toffer_id\r\n");

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
						PName = Server.HtmlEncode(Common.Left(PName,80).Replace("\t","").Replace("\n","").Replace("\r","")); // per google
						String PDesc = String.Empty;
						if(DB.RSField(rs,"FroogleDescription").Length != 0)
						{
							PDesc = DB.RSField(rs,"FroogleDescription");
						}
						else if(Common.AppConfigBool("Froogle_UseDescriptionsIfFroogleDescEmpty"))
						{
							PDesc = DB.RSField(rs,"VariantDescription");
							if(PDesc.Length == 0)
							{
								PDesc = DB.RSField(rs,"Description");
							}
						}
						PDesc = Server.HtmlEncode(Common.Left(PDesc,1000).Replace("\t","").Replace("\n","").Replace("\r","")); // per google
						String ID = (DB.RSFieldInt(rs,"ProductID")*100000).ToString() + (DB.RSFieldInt(rs,"VariantID")*100000).ToString(); // trying to generate unique ids that won't change
						tmpS.Append(StoreURL + "showproduct.aspx?productid=" + DB.RSFieldInt(rs,"ProductID").ToString() + FroogleTrackingSuffix);
						tmpS.Append("\t" + PName + "\t" + Common.MakeProperProductSKU(DB.RSField(rs,"ManufacturerPartNumber"),DB.RSField(rs,"VManufacturerPartNUmber"),String.Empty,String.Empty) + "\t" + PDesc + "\t" + ImgURL + "\t" + CatName + "\t" + Price + "\t" + ID.ToString() + "\r\n");
					}
					rs.Close();

					Common.WriteFile(FN,tmpS.ToString());

					Response.Write("Your froogle feed data is shown below. If satisifed with this data, <a href=\"froogle.aspx?submit=true\">Click here</a> to upload this data to Froogle.<br><br>");
					Response.Write("Feed Data:<br>\n");
					Response.Write("<hr size=1>");
					Response.Write(tmpS.ToString().Replace("\n","<br>"));
					Response.Write("<hr size=1>");
				}
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
