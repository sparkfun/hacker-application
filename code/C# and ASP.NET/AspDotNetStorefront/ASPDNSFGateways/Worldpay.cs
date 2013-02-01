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
	/// Summary description for Worldpay.
	/// </summary>
	public class Worldpay
	{
		public Worldpay() {}

		static public String CaptureOrder(int OrderNumber)
		{
			String result = "CAPTURE METHOD NOT SUPPORTED FOR WORLDPAY JUNIOR";
			return result;
		}

		static public String VoidOrder(int OrderNumber)
		{
			String result = "VOID METHOD NOT SUPPORTED FOR WORLDPAY JUNIOR";
			return result;
		}

		static public String RefundOrder(int OrderNumber)
		{
			String result = "REFUND METHOD NOT SUPPORTED FOR WORLDPAY JUNIOR";
			return result;
		}
		
		static public String ProcessCard(int OrderNumber, ShoppingCart cart, String CardName, String CardType, String CardNumber, String CardExtraCode, String CardExpirationMonth, String CardExpirationYear, Decimal OrderTotal, bool useLiveTransactions, Address Billing,  Address Shipping, String CAVV, String ECI, String XID, out bool PaymentCleared, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut)
		{
			String result = "OK";

			CustomerSession sess = new CustomerSession(Common.SessionUSInt("CustomerID"));
			String rawResponseString = 
				"TransID=" + sess.Session("WorldPay_TransID") + 
				"&FuturePayID=" + sess.Session("WorldPay_FuturePayID") + 
				"&TransStatus=" + sess.Session("WorldPay_TransStatus") + 
				"&TransTime=" + sess.Session("WorldPay_TransTime") + 
				"&AuthAmount=" + sess.Session("WorldPay_AuthAmount") + 
				"&AuthCurrency=" + sess.Session("WorldPay_AuthCurrency") + 
				"&RawAuthMessage=" + sess.Session("WorldPay_RawAuthMessage") + 
				"&RawAuthCode=" + sess.Session("WorldPay_RawAuthCode") + 
				"&CallbackPW=" + sess.Session("WorldPay_CallbackPW") + 
				"&CardType=" + sess.Session("WorldPay_CardType") + 
				"&CountryMatch=" + sess.Session("WorldPay_CountryMatch") + 
				"&AVS=" + sess.Session("WorldPay_AVS");

			String sql = String.Empty;
			String replyCode = sess.Session("WorldPay_TransStatus");
			String responseCode = sess.Session("WorldPay_RawAuthCode");
			String approvalCode = sess.Session("WorldPay_RawAuthCode");
			String authResponse = sess.Session("WorldPay_RawAuthMessage");
			String TransID = sess.Session("WorldPay_TransID");
			
			PaymentCleared = false;
			AuthorizationCode = approvalCode;
			AuthorizationResult = rawResponseString;
			AuthorizationTransID = TransID;
			AVSResult = sess.Session("WorldPay_AVS");
			TransactionCommandOut = String.Empty;

			bool AVSOk = true;
//			if(Common.AppConfigBool("WorldPay_RequireAVSMatch"))
//			{
//				AVSOk = false;
//				String AVS = Common.Session("WorldPay_AVS");
//				if(AVS.Length == 4)
//				{
//					AVSOk = (AVS[0] == '2') && ((AVS[1] == '2' || AVS[1] == '8') || (AVS[2] == '2' || AVS[2] == '8')) && (AVS[3] == '2' || AVS[3] == '8');
//				}
//			}


			
			if(replyCode == "Y" && AVSOk)
			{
				result = "OK";
				PaymentCleared = true;

			}
			else
			{
				result = authResponse;
				if(result.Length == 0)
				{
					result = "Unspecified Error";
				}
				result = result.Replace("account","card");
				result = result.Replace("Account","Card");
				result = result.Replace("ACCOUNT","CARD");
				// record failed TX:
				sql = "insert into FailedTransaction(CustomerID,OrderDate,PaymentGateway,TransactionCommand,TransactionResult) values(" + cart._thisCustomer._customerID.ToString() + "," + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + "," + DB.SQuote("WORLDPAY JUNIOR") + "," + DB.SQuote("N/A") + "," + DB.SQuote(rawResponseString) + ")";
				DB.ExecuteSQL(sql);
			}
			sess = null;
			return result;
		}

	}
}
