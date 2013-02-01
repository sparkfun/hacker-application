// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Security;
using System.Text;
using System.Web.SessionState;
using System.IO;
using System.Net;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontGateways
{
	/// <summary>
	/// Summary description for EFSNet.
	/// </summary>
	public class EFSNet
	{
		public EFSNet() {}

		static public String CaptureOrder(int OrderNumber)
		{
			String result = "NOT IMPLEMENTED YET";
			return result;
		}

		static public String VoidOrder(int OrderNumber)
		{
			String result = "NOT IMPLEMENTED YET";
			return result;
		}

		static public String RefundOrder(int OrderNumber)
		{
			String result = "NOT IMPLEMENTED YET";
			return result;
		}
		
		static public String ProcessCard(int OrderNumber, ShoppingCart cart, String CardName, String CardType, String CardNumber, String CardExtraCode, String CardExpirationMonth, String CardExpirationYear, Decimal OrderTotal, bool useLiveTransactions, Address Billing,  Address Shipping, String CAVV, String ECI, String XID, out bool PaymentCleared, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut)
		{
			String result = "OK";

			ASCIIEncoding encoding = new ASCIIEncoding();
			StringBuilder transactionCommand = new StringBuilder(5000);

			transactionCommand.Append("Method=" + Common.IIF(Common.TransactionMode() == "AUTH", "CreditCardAuthorize", "CreditCardCharge")); //Common.AppConfig("EFSNET_METHOD"));
			transactionCommand.Append("&StoreID=" + Common.IIF(useLiveTransactions , Common.AppConfig("EFSNET_LIVE_STOREID") , Common.AppConfig("EFSNET_TEST_STOREID")));
			transactionCommand.Append("&StoreKey=" + Common.IIF(useLiveTransactions , Common.AppConfig("EFSNET_LIVE_STOREKEY") , Common.AppConfig("EFSNET_TEST_STOREKEY")));
			transactionCommand.Append("&ApplicationID=" + HttpContext.Current.Server.UrlEncode(Common.AppConfig("StoreName") + " v" + Common.AppConfig("StoreVersion")));
			transactionCommand.Append("&AccountNumber=" + CardNumber);
			transactionCommand.Append("&ReferenceNumber=" + OrderNumber.ToString());
			transactionCommand.Append("&ExpirationMonth=" + CardExpirationMonth.PadLeft(2,'0'));
			transactionCommand.Append("&ExpirationYear=" + CardExpirationYear.Substring(2,2));
			transactionCommand.Append("&CardVerificationValue=" + CardExtraCode);
			transactionCommand.Append("&TransactionAmount=" + Localization.CurrencyStringForGateway(OrderTotal));
			if(Common.AppConfigBool("EFSNET_VERIFY_ADDRESSES"))
			{
				transactionCommand.Append("&BillingName=" + (Billing.FirstName + " " + Billing.LastName).Trim());
				transactionCommand.Append("&BillingAddress=" + HttpContext.Current.Server.UrlEncode(Billing.Address1));
				transactionCommand.Append("&BillingCity=" + HttpContext.Current.Server.UrlEncode(Billing.City));
				transactionCommand.Append("&BillingState=" + Billing.State);
				transactionCommand.Append("&BillingPostalCode=" + Billing.Zip);
				transactionCommand.Append("&BillingCountry=" + HttpContext.Current.Server.UrlEncode(Billing.Country));
				transactionCommand.Append("&BillingPhone=" + Billing.Phone);
			}

			String AuthServer = Common.IIF(useLiveTransactions , Common.AppConfig("EFSNET_LIVE_SERVER") , Common.AppConfig("EFSNET_TEST_SERVER"));
			String rawResponseString = Common.AspHTTP(AuthServer + "?" + transactionCommand.ToString());

			String[] statusArray = rawResponseString.Split("&".ToCharArray());
			String responseCode = String.Empty;
			String resultMessage = String.Empty;
			String resultCode = String.Empty;
			String transID = String.Empty;
			for(int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
			{
				String[] lasKeyPair = statusArray[i].Split("=".ToCharArray());
				switch(lasKeyPair[0].ToLower())
				{
					case "responsecode":
						responseCode = lasKeyPair[1];
						break;
					case "transactionid":
						transID = lasKeyPair[1];
						break;
					case "resultmessage":
						resultMessage = lasKeyPair[1];
						break;
					case "resultcode":
						resultCode = lasKeyPair[1];
						break;
				}
			}

			PaymentCleared = false;
			AuthorizationCode = resultCode;
			AuthorizationResult = rawResponseString;
			AuthorizationTransID = transID;
			AVSResult = String.Empty; // TBD
			TransactionCommandOut = transactionCommand.ToString();
			
			if(responseCode == "0")
			{
				result = "OK";
				PaymentCleared = true;
			}
			else
			{
				result = resultMessage;
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
				String sql3 = "insert into FailedTransaction(CustomerID,OrderDate,PaymentGateway,TransactionCommand,TransactionResult) values(" + cart._thisCustomer._customerID.ToString() + "," + DB.DateQuote(Localization.ToNativeShortDateString(System.DateTime.Now)) + "," + DB.SQuote("EFSNET") + "," + DB.SQuote(transactionCommand.ToString().Replace(CardNumber,CC)) + "," + DB.SQuote(rawResponseString) + ")";
				DB.ExecuteSQL(sql3);
			}
			return result;
		}

	}
}
