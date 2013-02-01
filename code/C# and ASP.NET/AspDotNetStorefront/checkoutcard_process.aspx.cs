// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using AspDotNetStorefrontCommon;
using AspDotNetStorefrontGateways;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for checkoutcard_process.
	/// </summary>
	public class checkoutcard_process : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");

			Customer thisCustomer = new Customer();
			thisCustomer.RequireCustomerRecord();

			if(ShoppingCart.CartIsEmpty(thisCustomer._customerID,CartTypeEnum.ShoppingCart))
			{
				Response.Redirect("ShoppingCart.aspx");
			}

			//Get the current card billing information
			Address BillingAddress = new Address();
			BillingAddress.LoadByCustomer(thisCustomer._customerID,AddressTypes.Billing);

			String CardName = Common.Form("CardName");
			String CardNumber = Common.Form("CardNumber").Trim().Replace(" ","");
			String CardExtraCode = Common.Form("CardExtraCode").Trim().Replace(" ","");
			String CardType = Common.Form("CardType").Trim().Replace(" ","");
			String CardExpirationMonth = Common.Form("CardExpirationMonth").Trim().Replace(" ","");
			String CardExpirationYear = Common.Form("CardExpirationYear").Trim().Replace(" ","");

			if (CardNumber.StartsWith("***")) CardNumber = BillingAddress.CardNumber; // Still obscured so use the original
			if (CardExtraCode.StartsWith("***")) CardExtraCode = BillingAddress.CardExtraCode; // Still obscured so use the original
 
			if(CardNumber.Length == 0 || (!Common.AppConfigBool("CardExtraCodeIsOptional") && CardExtraCode.Length == 0) || CardName.Length == 0 || CardExpirationMonth.Length == 0 || CardExpirationYear.Length == 0)
			{
				Response.Redirect("checkoutcard.aspx?errormsg=" + Server.UrlEncode("No Credit Card Number Found, Please Retry."));
			}
			
			ShoppingCart cart = new ShoppingCart(1,thisCustomer,CartTypeEnum.ShoppingCart,0,false);
			int OrderNumber = Common.GetNextOrderNumber();

			// remember their info:

			BillingAddress.CardName = CardName;
			BillingAddress.CardType = CardType;
			BillingAddress.CardNumber = CardNumber;
			BillingAddress.CardExtraCode = String.Empty; // we are NOT allowed to store this value!
			BillingAddress.CardExpirationMonth = CardExpirationMonth;
			BillingAddress.CardExpirationYear = CardExpirationYear;
			BillingAddress.UpdateDB();

			//
			//			String CSql = "update Customer set CardType=" + DB.SQuote(CardType) + ", CardName=" + DB.SQuote(CardName) + ", CardNumber=" + DB.SQuote(Common.MungeString(CardNumber)) + ", CardExpirationMonth=" + DB.SQuote(CardExpirationMonth) + ", CardExpirationYear=" + DB.SQuote(CardExpirationYear) + " where CustomerID=" + thisCustomer._customerID.ToString();
			//			DB.ExecuteSQL(CSql);

			// try create the order record, check for status of TX though:

			if(Common.AppConfigBool("CardinalCommerce.Centinel.Enabled") && (CardType.Trim().ToUpper() == "VISA" || CardType.Trim().ToUpper() == "MASTERCARD"))
			{
				// use cardinal pre-auth fraud screening:
				String ACSUrl = String.Empty;
				String Payload = String.Empty;
				String TransactionID = String.Empty;
				String CardinalLookupResult = String.Empty;
				if(Cardinal.PreChargeLookup(CardNumber,Localization.ParseUSInt(CardExpirationYear),Localization.ParseUSInt(CardExpirationMonth),OrderNumber,cart.Total(true),"",out ACSUrl, out Payload, out TransactionID, out CardinalLookupResult))
				{
					// redirect to intermediary page which gets card password from user:

					CustomerSession sess = new CustomerSession(thisCustomer._customerID);
					sess.SetVal("Cardinal.LookupResult",CardinalLookupResult,System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
					sess.SetVal("Cardinal.ACSUrl",ACSUrl,System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
					sess.SetVal("Cardinal.Payload",Payload,System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
					sess.SetVal("Cardinal.TransactionID",TransactionID,System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
					sess.SetVal("Cardinal.OrderNumber",OrderNumber,System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
					sess.SetVal("CardName",CardName,System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
					sess.SetVal("CardNumber",Common.MungeString(CardNumber),System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
					sess.SetVal("CardExtraCode",CardExtraCode,System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
					sess.SetVal("CardType",CardType,System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
					sess.SetVal("CardExpirationMonth",CardExpirationMonth,System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
					sess.SetVal("CardExpirationYear",CardExpirationYear,System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
					sess =  null;
		
					Response.Redirect("cardinalform.aspx");
					// this will eventually come "back" to us in cardinal_process.aspx
				}
				else
				{
					CustomerSession sess = new CustomerSession(thisCustomer._customerID);
					sess.SetVal("Cardinal.LookupResult",CardinalLookupResult,System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
					// user not enrolled or cardinal gateway returned error, so process card normally, using already created order #:
					String status = Gateway.MakeOrder(cart,"Credit Card",String.Empty,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderNumber,String.Empty, String.Empty, String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty);
					if(status != "OK")
					{
						sess =  null;
						Response.Redirect("checkoutcard.aspx?error=1&errormsg=" + status);
					}
					DB.ExecuteSQL("update orders set CardinalLookupResult=" + DB.SQuote(sess.Session("Cardinal.LookupResult")) + " where OrderNumber=" + OrderNumber.ToString());
					sess =  null;
					Response.Redirect("orderconfirmation.aspx?ordernumber=" + OrderNumber.ToString() + "&paymentmethod=Credit+Card");
				}
			}
			else
			{
				// try create the order record, check for status of TX though:
				String status = Gateway.MakeOrder(cart,"Credit Card",String.Empty,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderNumber,String.Empty, String.Empty, String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty);
				if(status != "OK")
				{
					Response.Redirect("checkoutcard.aspx?error=1&errormsg=" + Server.UrlEncode(status));
				}
				Response.Redirect("orderconfirmation.aspx?ordernumber=" + OrderNumber.ToString() + "&paymentmethod=Credit+Card");

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
