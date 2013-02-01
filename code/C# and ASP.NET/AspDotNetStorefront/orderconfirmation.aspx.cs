//#define SECUREATTACHMENT
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
	/// Summary description for orderconfirmation.
	/// </summary>
	public class orderconfirmation : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Order - Confirmation:";
			RequireSecurePage();
			RequiresLogin(Common.GetThisPageName(false) + "?" + Common.ServerVariables("QUERY_STRING"));
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			String PaymentMethod = Common.QueryString("PaymentMethod");

			int CustomerID = thisCustomer._customerID;
			int OrderNumber = Common.QueryStringUSInt("OrderNumber");

			CustomerSession sess = new CustomerSession(CustomerID);
			bool AlreadyConfirmed = false;
			if(sess.Session("Order_" + OrderNumber.ToString() + "_CONFIRMED").Length != 0)
			{
				AlreadyConfirmed = true;
			}

			if(!AlreadyConfirmed)
			{
				DB.ExecuteSQL("update customer set OrderNotes=NULL where customerid=" + CustomerID.ToString());
			}

			String ReceiptURL = "receipt.aspx?ordernumber=" + OrderNumber.ToString() + "&customerid=" + CustomerID.ToString();
			IDataReader rs = DB.GetRS("select * from orders where customerid=" + CustomerID.ToString() + " and ordernumber=" + OrderNumber.ToString());
			Order order = new Order(OrderNumber);
			writer.Write("<center>");
			String StoreName = Common.AppConfig("StoreName");
			bool UseLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
			if(rs.Read())
			{
				if(!AlreadyConfirmed)
				{
					Common.SendOrderEMail(thisCustomer,OrderNumber,false,PaymentMethod,true);
					writer.Write("\n");
					String SS = String.Empty;
					if(Common.AppConfigBool("IncludeGoogleTrackingCode"))
					{
						Topic GoogleTrackingCode = new Topic("GoogleTrackingCode");
						if(GoogleTrackingCode._contents.Length != 0)
						{
							writer.Write(GoogleTrackingCode._contents);
							writer.Write("\n");
						}
					}
					if(Common.AppConfigBool("IncludeOvertureTrackingCode"))
					{
						Topic OvertureTrackingCode = new Topic("OvertureTrackingCode");
						if(OvertureTrackingCode._contents.Length != 0)
						{
							writer.Write(OvertureTrackingCode._contents);
							writer.Write("\n");
						}
					}
				}
			
				Topic t = new Topic("ConfirmationTracking",thisCustomer._localeSetting,_siteID);
				String ConfirmationTracking = t._contents;
				if(ConfirmationTracking.Length != 0)
				{
					writer.Write(ConfirmationTracking);
				}
				switch(PaymentMethod.Replace(" ","").Trim().ToUpper())
				{
					case "CREDITCARD":
						writer.Write("<br><br><font class=\"ProductNameText\">ORDER RECEIVED</font><br><br><b>Thank you for your order.</b><br><br>");
						break;
					case "ECHECK":
						writer.Write("<br><br><font class=\"ProductNameText\">ORDER RECEIVED</font><br><br><b>Thank you for your order.</b><br><br>");
						break;
					case "MICROPAY":
						writer.Write("<br><br><font class=\"ProductNameText\">" + Common.AppConfig("Micropay.Prompt").ToUpper() + " ORDER RECEIVED</font><br><br>");
						writer.Write("<b>Your " + Common.AppConfig("Micropay.Prompt") + " balance is: " + Localization.CurrencyStringForDisplay(Common.GetMicroPayBalance(CustomerID)) + "</b><br><br><b>Thank you for your order.</b><br><br>");
						break;
					case "PURCHASEORDER":
						writer.Write("<br><br><font class=\"ProductNameText\">PURCHASE ORDER RECEIVED</font><br><br><b>Thank you for your order. Someone will contact you shortly.</b><br><br>");
						break;
					case "PAYPAL":
						writer.Write("<br><br><font class=\"ProductNameText\">PAYPAL ORDER RECEIVED</font><br><br><b>Thank you for your order.</b><br><br>");
						break;
					case "REQUESTQUOTE":
						writer.Write("<br><br><font class=\"ProductNameText\">REQUEST FOR QUOTE RECEIVED</font><br><br><b>Thank you for your custom quote request. Someone will contact you shortly.</b><br><br>");
						break;
					case "CHECK":
						writer.Write("<br><br><font class=\"ProductNameText\">PENDING ORDER RECEIVED (CHECK or MONEY ORDER)</font><br><br><b>Thank you for your order.<br><br>");
						String SS = String.Empty;
						writer.Write("<span class=\"CheckInstructions\">\n");
						Topic t2 = new Topic("checkinstructions",thisCustomer._localeSetting,_siteID);
						writer.Write(t2._contents);
						writer.Write("</span>");
						break;
				}
				writer.Write("<br><br><b>PLEASE PRINT THIS PAGE FOR YOUR RECORDS</b><br><br>");
				writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;Your Order Number is: <b>" + OrderNumber.ToString() + "</b><br>");
				writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;Your Customer ID is: <b>" + CustomerID.ToString() + "</b><br><br>");
				writer.Write("An e-mail confirmation will be sent to: " + DB.RSField(rs,"Email") + ".<br><br><br>");
				// we have now changed the following to send the receipt in all cases, so the customer has a record of their order:
				//if(order._paymentClearedOn.Length != 0 && order._paymentGateway != "MANUAL" && PaymentMethod.Replace(" ","").Trim().ToUpper() != "CHECK")
				//{
				writer.Write("For a printable receipt, <a target=\"_blank\" href=\"" + ReceiptURL + "\"> click here</a>.<br><br>");
				//}

				// however, do NOT send download e-mail unless payment has cleared:
				if(order._paymentClearedOn != System.DateTime.MinValue && order.HasDownloadComponents() && (PaymentMethod.Replace(" ","").Trim().ToUpper() == "PAYPAL" || PaymentMethod.Replace(" ","").Trim().ToUpper() == "ECHECK" || PaymentMethod.Replace(" ","").Trim().ToUpper() == "MICROPAY" || (PaymentMethod.Replace(" ","").Trim().ToUpper() == "CREDITCARD" && order._paymentGateway != "MANUAL")))
				{
					writer.Write("Your order has downloadable components.<br>A separate e-mail will be sent to you with download instructions.<br>The download items are listed below for convenience:<br><br>");
					writer.Write(order.GetDownloadList(false));
				}
			}
			else
			{
				writer.Write("<br><br><br><br><br>");
				writer.Write("No order could be found in the database...Please contact us for more information.");
				writer.Write("<br><br><br><br><br>");
			}
			writer.Write("</center>");
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
