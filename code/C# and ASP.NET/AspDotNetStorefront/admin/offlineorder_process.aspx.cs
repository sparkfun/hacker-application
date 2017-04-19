// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for offlineorder_process.
	/// </summary>
	public class offlineorder_process : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Enter Offline Order - Process Status";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			StringBuilder sql = new StringBuilder(5000);
			String orderGUID = DB.GetNewGUID();

			int NewOrderNumber = Common.GetNextOrderNumber();
			sql.Append("insert into Orders(OrderNumber,OrderGUID,OrderDate,StoreVersion,CustomerServiceNotes,SiteID,CustomerID,TaxRate,OrderNotes,PaymentMethod,LastName,FirstName,EMail,Phone,BillingLastName,BillingFirstName,BillingCompany,BillingAddress1,BillingAddress2,BillingSuite,BillingCity,BillingState,BillingZip,BillingCountry,BillingPhone,ShippingLastName,ShippingFirstName,ShippingCompany,ShippingAddress1,ShippingAddress2,ShippingSuite,ShippingCity,ShippingState,ShippingZip,ShippingCountry,ShippingPhone,OrderSubtotal,OrderTax,OrderShippingCosts,OrderTotal,ShippedOn,RegisterDate,ReceiptEmailSentOn,PaymentClearedOn,Deleted,TransactionCommand) values (");
			sql.Append( NewOrderNumber.ToString() + ",");
			sql.Append( DB.SQuote(orderGUID) + ",");
			sql.Append( DB.SQuote(Common.Form("OrderDate")) + ",");
			sql.Append( DB.SQuote(Common.AppConfig("StoreVersion")) + ",");
			sql.Append( DB.SQuote(Common.AppConfig("OfflineOrder")) + ",");
			sql.Append( Common.AppConfigUSInt("DefaultSkinID").ToString() + ","); // SiteID
			sql.Append( "0,"); // customerid
			sql.Append( Localization.SingleStringForDB(Common.GetStateTaxRate(Common.Form("ShippingState"))) + ",");
			sql.Append( DB.SQuote(Common.Form("OrderNotes")) + ",");
			sql.Append( DB.SQuote(Common.Form("PaymentMethod")) + ",");
			sql.Append( DB.SQuote(Common.Form("LastName")) + ",");
			sql.Append( DB.SQuote(Common.Form("FirstName")) + ",");
			sql.Append( DB.SQuote(Common.Form("EMail")) + ",");
			sql.Append( DB.SQuote(Common.Form("ShippingPhone")) + ",");
			sql.Append( DB.SQuote(Common.Form("BillingLastName")) + ",");
			sql.Append( DB.SQuote(Common.Form("BillingFirstName")) + ",");
			sql.Append( DB.SQuote(Common.Form("BillingCompany")) + ",");
			sql.Append( DB.SQuote(Common.Form("BillingAddress1")) + ",");
			sql.Append( DB.SQuote(Common.Form("BillingAddress2")) + ",");
			sql.Append( DB.SQuote(Common.Form("BillingSuite")) + ",");
			sql.Append( DB.SQuote(Common.Form("BillingCity")) + ",");
			sql.Append( DB.SQuote(Common.Form("BillingState")) + ",");
			sql.Append( DB.SQuote(Common.Form("BillingZip")) + ",");
			sql.Append( DB.SQuote(Common.Form("BillingCountry")) + ",");
			sql.Append( DB.SQuote(Common.Form("BillingPhone")) + ",");
			sql.Append( DB.SQuote(Common.Form("ShippingLastName")) + ",");
			sql.Append( DB.SQuote(Common.Form("ShippingFirstName")) + ",");
			sql.Append( DB.SQuote(Common.Form("ShippingCompany")) + ",");
			sql.Append( DB.SQuote(Common.Form("ShippingAddress1")) + ",");
			sql.Append( DB.SQuote(Common.Form("ShippingAddress2")) + ",");
			sql.Append( DB.SQuote(Common.Form("ShippingSuite")) + ",");
			sql.Append( DB.SQuote(Common.Form("ShippingCity")) + ",");
			sql.Append( DB.SQuote(Common.Form("ShippingState")) + ",");
			sql.Append( DB.SQuote(Common.Form("ShippingZip")) + ",");
			sql.Append( DB.SQuote(Common.Form("ShippingCountry")) + ",");
			sql.Append( DB.SQuote(Common.Form("ShippingPhone")) + ",");
			sql.Append( Localization.CurrencyStringForDB(Common.FormUSDecimal("Subtotal")) + ",");
			sql.Append( Localization.CurrencyStringForDB(Common.FormUSDecimal("Tax")) + ",");
			sql.Append( Localization.CurrencyStringForDB(Common.FormUSDecimal("Shipping")) + ",");
			sql.Append( Localization.CurrencyStringForDB(Common.FormUSDecimal("Total")) + ",");
			sql.Append( DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
			sql.Append( DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
			sql.Append( DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
			sql.Append( DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
			sql.Append( "0,"); // deleted
			sql.Append( DB.SQuote("OFFLINE ORDER") + ")");  // must update later if TX is ok!

			DB.ExecuteSQL(sql.ToString());

			// add order items:
			for(int i = 1; i <= 5; i++)
			{
				String Description = Common.Form("Description_" + i.ToString());
				String SKU = Common.Form("SKU_" + i.ToString());
				int Quantity = Common.FormUSInt("Quantity_" + i.ToString());
				if(Quantity == 0)
				{
					Quantity = 1;
				}
				String Price = Common.Form("Price_" + i.ToString());
				if(Description.Length != 0)
				{
					StringBuilder sql2 = new StringBuilder(5000);
					sql2.Append("insert into Orders_ShoppingCart(OrderNumber,ShoppingCartRecID,CustomerID,ProductID,VariantID,Quantity,OrderedProductDescription,OrderedProductSKU,OrderedProductPrice,OrderedProductRegularPrice,IsTaxable) values(");
					sql2.Append(NewOrderNumber.ToString() + ",");
					sql2.Append("0,0,0,0,"); // ShoppingCartRecID,CustomerID,ProductID,VariantID
					sql2.Append(Quantity.ToString() + ",");
					sql2.Append(DB.SQuote(Description) + ",");
					sql2.Append(DB.SQuote(SKU) + ",");
					sql2.Append(Price.ToString().Replace("$","") + ",");
					sql2.Append(Price.ToString().Replace("$","") + ",");
					sql2.Append("1" + ")");
					DB.ExecuteSQL(sql2.ToString());
				}
			}

			// send receipt (if e-mail specified)
			String MailServer = Common.AppConfig("MailMe_Server");
			if(MailServer.Length != 0 && MailServer.ToUpper() != "TBD")
			{
				Order order = new Order(NewOrderNumber);
				String SubjectReceipt = String.Empty;
				SubjectReceipt = Common.AppConfig("StoreName") + " Receipt";
				if(order._paymentMethod.ToUpper() == "REQUEST QUOTE")
				{
					SubjectReceipt += " (REQUEST FOR QUOTE)";
				}
				try
				{
					Common.SendMail(SubjectReceipt, order.Receipt(thisCustomer._customerID,Common.GetActiveReceiptTemplate(_siteID),"",true) + Common.AppConfig("MailFooter"), true, Common.AppConfig("ReceiptEMailFrom"),Common.AppConfig("ReceiptEMailFromName"),order._email,order._email,"",MailServer);
				}
				catch {}
			}

			String ReceiptURL = "../receipt.aspx?ordernumber=" + NewOrderNumber.ToString() + "&customerid=0&pwd=" + HttpContext.Current.Server.UrlEncode(Common.AppConfig("OrderShowCCPwd"));

			writer.Write("<div align=\"center\">\n");
			writer.Write("<p align=\"center\"><h3><b>ORDER #" + NewOrderNumber.ToString() + " CREATED</b></h3></p>\n");
			writer.Write("<br><br><p align=\"center\"><a href=\"" + ReceiptURL + "\" target=\"_blank\"><b>View Receipt</b></a></p>\n");
			writer.Write("<br><br><p align=\"center\"><a href=\"offlineorder.aspx\"><b>Enter Another Order</b></a></p>\n");
			writer.Write("</div>");
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
