// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for emailproduct.
	/// </summary>
	public class emailproduct : SkinBase
	{
		int ProductID;
		int CategoryID;
		bool RequiresReg;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			ProductID = Common.QueryStringUSInt("ProductID");
			if(ProductID == 0)
			{
				Response.Redirect("default.aspx");
			}
			if(Common.ProductHasBeenDeleted(ProductID))
			{
				Response.Redirect(SE.MakeDriverLink("ProductNotFound"));
			}

			IDataReader rs = DB.GetRS("select * from product " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString());
			if(!rs.Read())
			{
				rs.Close();
				Response.Redirect("default.aspx");
			}
			RequiresReg = DB.RSFieldBool(rs,"RequiresRegistration");
			int ProductDisplayFormatID = DB.RSFieldInt(rs,"ProductDisplayFormatID");
			String ProductName = DB.RSField(rs,"Name");
			String ProductDescription = DB.RSField(rs,"Description"); //.Replace("\n","<br>");
			String FileDescription = new ProductDescriptionFile(ProductID,thisCustomer._localeSetting,_siteID)._contents;
			if(FileDescription.Length != 0)
			{
				ProductDescription += "<div align=\"left\">" + FileDescription + "</div>";
			}
			rs.Close();

			CategoryID = Common.QueryStringUSInt("CategoryID");
			if(CategoryID == 0)
			{
				// no category passed in, pick first one that this product is mapped to:
				String tmpS = Common.GetProductCategories(ProductID,false);
				if(tmpS.Length != 0)
				{
					String[] catIDs = tmpS.Split(',');
					CategoryID = Localization.ParseUSInt(catIDs[0]);
				}
			}
			String CategoryName = Common.GetCategoryName(CategoryID);

			SectionTitle = String.Empty;
			int pid = Common.GetParentCategory(CategoryID);
			while(pid != 0)
			{
				SectionTitle = "<a class=\"SectionTitleText\" href=\"" + SE.MakeCategoryLink(pid,"") + "\">" + Common.GetCategoryName(pid) + "</a> - " + SectionTitle;
				pid = Common.GetParentCategory(pid);
			}
			SectionTitle += "<a class=\"SectionTitleText\" href=\"" + SE.MakeCategoryLink(CategoryID,"") + "\">" + CategoryName + "</a> - ";
			SectionTitle += ProductName;
			SectionTitle += "</span>";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			Common.LogEvent(thisCustomer._customerID,10, ProductID.ToString());
			if(RequiresReg && thisCustomer._isAnon)
			{
				writer.Write("<br><br><br><br><b>You must be a registered user to view this product!</b><br><br><br><a href=\"signin.aspx?returnurl=showproduct.aspx?" + Server.HtmlEncode(Server.UrlEncode(Common.ServerVariables("QUERY_STRING"))) + "\">Click Here</a> to sign-in.");
			}
			else
			{
				DB.ExecuteSQL("update product set Looks=Looks+1 where ProductID=" + ProductID.ToString());
				String sql = "SELECT SalesPrompt.name as SDescription, Product.ProductID, Product.Name, Product.SEName, Product.SpecTitle, Product.SpecsInline, Product.SpecCall, Product.ProductDisplayFormatID, Product.ColWidth, Product.Summary, Product.Description, Product.SEKeywords, Product.SEDescription, Product.SKU, Product.ManufacturerID, Product.ManufacturerPartNumber, Product.Published, Product.Deleted, ProductVariant.VariantID, ProductVariant.Name AS VariantName, ProductVariant.Colors, ProductVariant.Sizes, ProductVariant.Inventory, ProductVariant.SKUSuffix, ProductVariant.ManufacturerPartNumber AS VManufacturerPartNumber, ProductVariant.Price, ProductVariant.SalePrice, ProductVariant.Deleted AS VariantDeleted, ProductVariant.Description as VariantDescription, ProductVariant.Published AS VariantPublished, Manufacturer.Name AS ManufacturerName, Manufacturer.SEName as ManufacturerSEName FROM  ((Product  " + DB.GetNoLock() + " INNER JOIN Manufacturer  " + DB.GetNoLock() + " ON Product.ManufacturerID = Manufacturer.ManufacturerID) left outer join salesprompt  " + DB.GetNoLock() + " on product.salespromptid=salesprompt.salespromptid) LEFT OUTER JOIN ProductVariant  " + DB.GetNoLock() + " ON Product.ProductID = ProductVariant.ProductID WHERE Product.Published=1 AND Product.Deleted=0 and product.productid=" + ProductID.ToString() + " AND ProductVariant.Published=1 AND ProductVariant.Deleted=0 ORDER by ProductVariant.DisplayOrder, ProductVariant.Name";
				IDataReader rs = DB.GetRS(sql);
				rs.Read();

				String ProdPic = String.Empty;
				String SEName = DB.RSField(rs,"SEName");

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
				writer.Write("<img border=\"0\" src=\"" + ProdPic + "\">");
				writer.Write("<br>");
				writer.Write("");
				writer.Write("</td>");
				writer.Write("<td align=\"left\" valign=\"top\">");

				writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");

				writer.Write("<tr><td align=\"right\">");
				writer.Write("<a class=\"ProductNavLink\" href=\"" + SE.MakeProductAndCategoryLink(ProductID,CategoryID,SEName) + "\">back to product page</a>");
				writer.Write("</td></tr>\n");
				writer.Write("<tr valign=\"top\"><td height=10></td></tr>");

				writer.Write("<tr valign=\"top\"><td><font class=\"ProductNameText\">");
				writer.Write("E-Mail To A Friend: " + (DB.RSField(rs,"Name") + " " + DB.RSField(rs,"VariantName")).Trim());
				writer.Write("</font>&nbsp;&nbsp;");
				writer.Write("<br><br>");
				writer.Write("</td></tr>");
				writer.Write("<tr valign=\"top\"><td align=\"left\" valign=\"top\">\n");


				if(Common.Form("ToAddress").Length != 0)
				{
					// we have a submit:
					String FromAddress = Common.Form("FromAddress");
					String ToAddress = Common.Form("ToAddress");
					String BotAddress = Common.AppConfig("ReceiptEMailFrom");
					String Subject = Common.AppConfig("StoreName") + " - " + (DB.RSField(rs,"Name") + " " + DB.RSField(rs,"VariantName")).Trim();
					String ProductURL = Common.GetStoreHTTPLocation(false) + SE.MakeProductAndCategoryLink(ProductID,CategoryID,SEName);
					StringBuilder Body = new StringBuilder(5000);
					Body.Append("<html><head><title>" + Subject + "</title></head><body>");
					Body.Append("This message has been sent to you from " + FromAddress + "<br><br>");
					Body.Append("Message from sender: <br>");
					Body.Append(Server.HtmlEncode(Common.Form("Message")));
					Body.Append("<br><br>");
					Body.Append("<b>They invited you to view the " + (DB.RSField(rs,"Name") + " " + DB.RSField(rs,"VariantName")).Trim() + " Product.</b><br><br>To view the product page, click on the URL given below.<br>");
					Body.Append("URL: <a href=\"" + ProductURL + "\">" + ProductURL + "</a>");
					Body.Append("<br><br>");
					ProdPic = Common.LookupImage("Product",ProductID,"medium",_siteID);
					if(ProdPic.Length == 0)
					{
						ProdPic = Common.LookupImage("Product",ProductID,"icon",_siteID);
					}
					if(ProdPic.Length != 0)
					{
						Body.Append("<a href=\"" + ProductURL + "\"><img align=\"left\" border=\"0\" src=\"" + Common.GetStoreHTTPLocation(false) + ProdPic + "\"></a>");
					}
					Body.Append(DB.RSField(rs,"Description") + "<br><br clear=\"all\">");
					Body.Append("<p>" + Common.AppConfig("StoreName") + "<br>");
					Body.Append("<a href=\"" + Common.GetStoreHTTPLocation(false) + "\">" + Common.GetStoreHTTPLocation(false) + "</a></p>");
					Body.Append("</body></html>");
					try
					{
						Common.SendMail(Subject, Body.ToString(), true, BotAddress,BotAddress,ToAddress,ToAddress,"",Common.AppConfig("MailMe_Server"));
						writer.Write("<b>Your e-mail has been sent.<br><br></b>");
					}
					catch(Exception ex)
					{
						writer.Write("<b>Your e-mail could not be sent. Reason: " + Common.GetExceptionDetail(ex,"<br>") + "<br><br></b>");
					}
					writer.Write("<a href=\"" + SE.MakeProductAndCategoryLink(ProductID,CategoryID,SEName) + "\"><b>Click here to go back to the product page</b></a>");
				}
				
				else
				{
					writer.Write("<form method=\"POST\" action=\"emailproduct.aspx?productid=" + ProductID.ToString() + "&categoryid=" + CategoryID.ToString() + "\" onsubmit=\"return validateForm(this)\" name=\"EMailForm\">\n");
					writer.Write("<b>E-mail this product information to:</b><br>\n");
					writer.Write("<small>(Enter the email address of the recipient below)</small><br>\n");
					writer.Write("<input maxLength=\"50\" size=\"40\" value=\"\" name=\"ToAddress\"><br>\n");
					writer.Write("<input type=\"hidden\" name=\"ToAddress_vldt\" value=\"[req][email][blankalert=Please enter the e-mail address of who you want this sent to][invalidalert=Please enter a valid e-mail address for the recipient]\"><br>\n");
					writer.Write("<p><b>Add your own personal message:</b><br>\n");
					writer.Write("<textarea name=\"Message\" rows=\"7\" wrap=\"virtual\" cols=\"40\" size=\"50\">This is my personal message</textarea></p>\n");
					writer.Write("<p><b>Enter your email address:</b><br>\n");
					writer.Write("<input maxLength=\"50\" size=\"40\" value=\"" + Common.IIF(thisCustomer._isAnon , "" , thisCustomer._email) + "\" name=\"FromAddress\"><br>\n");
					writer.Write("<input type=\"hidden\" name=\"FromAddress_vldt\" value=\"[req][email][blankalert=Please enter your e-mail address so the recipient knows who this is from][invalidalert=Please enter a valid e-mail for yourself]\"><br>\n");
					writer.Write("<font color=\"#cc0000\">Note:</font> Your email address is used <i>only</i> to let the recipient know who sent the email and in case of transmission error. Neither your address nor the recipient's address will be used for any other purpose.</p>\n");
					writer.Write("<p><input type=\"submit\" value=\"Send\"> <input type=\"reset\" value=\"Clear\"></p>\n");
					writer.Write("</form>\n");
				}

				writer.Write("</td></tr>");

				writer.Write("</table>");
				writer.Write("</td></tr>");
				writer.Write("</table>");

				rs.Close();

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
