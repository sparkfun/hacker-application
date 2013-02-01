//#define IONGATE
// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontGateways
{
	/// <summary>
	/// Summary description for IonGate.
	/// </summary>
	public class IonGate
	{
		public IonGate() {}

		static public String CaptureOrder(int OrderNumber)
		{
			String result = "IONGATE COM COMPONENTS NOT INSTALLED ON SERVER OR STOREFRONT NOT COMPILED WITH IONGATE CODE TURNED ON";
#if IONGATE
			result = "NOT IMPLEMENTED YET";
#endif
			return result;
		}

		static public String VoidOrder(int OrderNumber)
		{
			String result = "IONGATE COM COMPONENTS NOT INSTALLED ON SERVER OR STOREFRONT NOT COMPILED WITH IONGATE CODE TURNED ON";
#if IONGATE
			result = "NOT IMPLEMENTED YET";
#endif
			return result;
		}

		static public String RefundOrder(int OrderNumber)
		{
			String result = "IONGATE COM COMPONENTS NOT INSTALLED ON SERVER OR STOREFRONT NOT COMPILED WITH IONGATE CODE TURNED ON";
#if IONGATE
			result = "NOT IMPLEMENTED YET";
#endif
			return result;
		}
		
		static public String ProcessCard(int OrderNumber, ShoppingCart cart, String CardName, String CardType, String CardNumber, String CardExtraCode, String CardExpirationMonth, String CardExpirationYear, Decimal OrderTotal, bool useLiveTransactions, Address Billing,  Address Shipping, String CAVV, String ECI, String XID, out bool PaymentCleared, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut)
		{
			String result = "IONGATE COM COMPONENTS NOT INSTALLED ON SERVER OR STOREFRONT NOT COMPILED WITH IONGATE CODE TURNED ON";
			PaymentCleared = false;
			AuthorizationCode = String.Empty;
			AuthorizationResult = String.Empty;
			AuthorizationTransID = String.Empty;
			AVSResult = String.Empty;
			TransactionCommandOut = String.Empty;
#if IONGATE
		String sql;
		IONConnect.AuthorizeClass ionGate = new IONConnect.AuthorizeClass();
		String transactionCommand;
		String RawResponseString;
		String ionStatus;
		String ionReplyCode = String.Empty;
		String ionResponseCode = String.Empty;
		String ionAuthResponse = String.Empty;
		String ionApprovalCode = String.Empty;
		String OrderTotalString;
		String ionLogin = Common.IIF(useLiveTransactions , Common.AppConfig("IonGateLogin") , Common.AppConfig("IonGateLoginTest"));

		if(!useLiveTransactions)
		{
			OrderTotal = 1.0M;
		}
		OrderTotalString = Localization.CurrencyStringForGateway(OrderTotal);

		// TBD: how the heck do you specify the transactionmode for iongate??? their site sucks!

		transactionCommand = "Login=" + ionLogin + "|" + 
			"Amount=" + OrderTotalString + "|" + 
			"CardType=" + CardType.ToUpper() + "|" + 
			"CardNum=" + CardNumber + "|" + 
			"CardName=" + CardName + "|" + 
			"Address=" + Billing.address1 + "|" + 
			"Address2=" + Billing.address2 + "|" + 
			"City=" + Billing.city + "|" + 
			"State=" + Billing.state + "|" + 
			"Zip=" + Billing.zip + "|" + 
			"Phone=" + Billing.phone + "|" + 
			"InvoiceNo=" + OrderNumber.ToString() + "|" + 
			"Description=" + OrderNumber.ToString() + "|" + 
			"Expires=" + CardExpirationMonth + CardExpirationYear.Substring(CardExpirationYear.Length-2,2);

		ionStatus = ionGate.Post(transactionCommand);
		RawResponseString = ionStatus;
		String[] ionStatusArray = ionStatus.Split("|".ToCharArray());
		for(int i = ionStatusArray.GetLowerBound(0); i <= ionStatusArray.GetUpperBound(0); i++)
		{
			String[] lasKeyPair = ionStatusArray[i].Split("=".ToCharArray());
			switch(lasKeyPair[0].ToLower())
			{
				case "replycode":
					ionReplyCode = lasKeyPair[1];
					break;
				case "responsecode":
					ionResponseCode = lasKeyPair[1];
					break;
				case "authresponse":
					ionAuthResponse = lasKeyPair[1];
					break;
				case "approvalcode":
					ionApprovalCode = lasKeyPair[1];
					break;
			}
		}
		
		PaymentCleared = false;
		AuthorizationCode = ionApprovalCode;
		AuthorizationResult = RawResponseString;
		AuthorizationTransID = String.Empty; // TBD
		AVSResult = String.Empty; // TBD
		TransactionCommandOut = transactionCommand.ToString();
		
		if(ionResponseCode == "AA" && ionReplyCode == "000")
		{
			result = "OK";
			PaymentCleared = true;
		}
		else
		{
			result = ionAuthResponse;
			if(result.Length == 0)
			{
				result = "Unspecified Error";
			}
			result = result.Replace("account","card");
			result = result.Replace("Account","Card");
			result = result.Replace("ACCOUNT","CARD");
			// record failed TX:
			String CC = String.Empty;
			if(Common.AppConfigBool("StoreCCInDB"))
			{
				CC = Common.MungeString(CardNumber);
			}
			else
			{
				CC = "XXXXXXXXXXXX";
			}
			sql = "insert into FailedTransaction(CustomerID,OrderDate,PaymentGateway,TransactionCommand,TransactionResult) values(" + cart._thisCustomer._customerID.ToString() + "," + DB.DateQuote(Localization.ToNativeShortDateString(System.DateTime.Now)) + "," + DB.SQuote("IONGATE") + "," + DB.SQuote(transactionCommand.Replace(CardNumber,CC) + "," + DB.SQuote(RawResponseString) + ")";
			DB.ExecuteSQL(sql);
		}
#endif
		return result;
		}
	}
}
