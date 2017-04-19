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
	/// Summary description for TwoCheckout.
	/// </summary>
	public class TwoCheckout
	{
		public TwoCheckout() {}

		static public String CaptureOrder(int OrderNumber)
		{
			String result = "CAPTURE METHOD NOT SUPPORTED FOR 2CHECKOUT";
			return result;
		}

		static public String VoidOrder(int OrderNumber)
		{
			String result = "VOID METHOD NOT SUPPORTED FOR 2CHECKOUT";
			return result;
		}

		static public String RefundOrder(int OrderNumber)
		{
			String result = "REFUND METHOD NOT SUPPORTED FOR 2CHECKOUT";
			return result;
		}
		
		static public String ProcessCard(int OrderNumber, ShoppingCart cart, String CardName, String CardType, String CardNumber, String CardExtraCode, String CardExpirationMonth, String CardExpirationYear, Decimal OrderTotal, bool useLiveTransactions, Address Billing, Address Shipping, String CAVV, String ECI, String XID, out bool PaymentCleared, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut)
		{
			String result = "OK";
			PaymentCleared = true;
			AuthorizationCode = String.Empty;
			AuthorizationResult = String.Empty;
			AuthorizationTransID = String.Empty;
			AVSResult = String.Empty;
			TransactionCommandOut = String.Empty;
			return result;
		}

	}
}
