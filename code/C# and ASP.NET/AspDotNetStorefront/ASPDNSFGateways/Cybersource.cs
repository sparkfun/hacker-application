//#define CYBERSOURCE
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
using System.Data;
using AspDotNetStorefrontCommon;

#if CYBERSOURCE
using System.Web.Services;
using CyberSource.Soap;
using CyberSource.Soap.CyberSourceWS;
#endif

using System.Collections;

namespace AspDotNetStorefrontGateways
{
	/// <summary>
	/// Summary description for Cybersource.
	/// </summary>
	public class Cybersource
	{
		public Cybersource() {}

		static public String CaptureOrder(int OrderNumber)
		{
			String result = "CAPTURE NOT IMPLEMENTED WITH CYBERSOURCE GATEWAY";
#if CYBERSOURCE
#endif
			return result;
		}

		static public String VoidOrder(int OrderNumber)
		{
			String result = "VOID NOT IMPLEMENTED WITH CYBERSOURCE GATEWAY";
#if CYBERSOURCE
#endif
			return result;
		}

		static public String RefundOrder(int OrderNumber)
		{
			String result = "REFUND NOT IMPLEMENTED WITH CYBERSOURCE GATEWAY";
#if CYBERSOURCE
#endif
			return result;
		}
		
		static public String ProcessCard(int OrderNumber, ShoppingCart cart, String CardName, String CardType, String CardNumber, String CardExtraCode, String CardExpirationMonth, String CardExpirationYear, Decimal OrderTotal, bool useLiveTransactions, Address Billing,  Address Shipping, String CAVV, String ECI, String XID, out bool PaymentCleared, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut)
		{
			String result = "GATEWAY NOT COMPILED INTO STOREFRONT";
			PaymentCleared = false;
			AVSResult = "N/A";
			AuthorizationResult = "N/A";
			AuthorizationCode = "N/A";
			AuthorizationTransID = "N/A";
			TransactionCommandOut = "N/A";
#if CYBERSOURCE
			result = "OK";

			RequestMessage request = new RequestMessage();

			request.clientApplication = "AspDotNetStorefront";
			request.clientApplicationVersion = Common.AppConfig("StoreVersion");
			request.clientApplicationUser = Common.Session("CustomerID");

			request.merchantReferenceCode = "Order #: " + OrderNumber.ToString() + " " + Localization.ToNativeDateTimeString(System.DateTime.Now));

			request.merchantID = Common.AppConfig("Cybersource.MerchantID");
			request.ccAuthService = new CCAuthService();
			request.ccAuthService.run = "true";
			if(Common.TransactionMode() == "AUTH CAPTURE")
			{
				request.ccCaptureService = new CCCaptureService();
				request.ccCaptureService.run = "true";
			}

			BillTo billTo = new BillTo();
			billTo.firstName = cart._billingAddress.firstName;
			billTo.lastName = cart._billingAddress.lastName;
			billTo.company = cart._billingAddress.company;
			billTo.street1 = cart._billingAddress.address1;
			billTo.street2 = cart._billingAddress.address2;
			billTo.city = cart._billingAddress.city;
			billTo.state = cart._billingAddress.state;
			billTo.postalCode = cart._billingAddress.zip;
			billTo.country = cart._billingAddress.country;
			request.billTo = billTo;

			ShipTo ShipTo = new ShipTo();
			ShipTo.firstName = cart._shippingAddress.firstName;
			ShipTo.lastName = cart._shippingAddress.lastName;
			ShipTo.company = cart._shippingAddress.company;
			ShipTo.street1 = cart._shippingAddress.address1;
			ShipTo.street2 = cart._shippingAddress.address2;
			ShipTo.city = cart._shippingAddress.city;
			ShipTo.state = cart._shippingAddress.state;
			ShipTo.postalCode = cart._shippingAddress.zip;
			ShipTo.country = cart._shippingAddress.country;
			request.shipTo = ShipTo;

			Card card = new Card();
			card.accountNumber = CardNumber;
			card.cvIndicator = "true";
			card.cvNumber = CardExtraCode;
			card.expirationMonth = CardExpirationMonth;
			card.expirationYear = CardExpirationYear;
			request.card = card;

			request.item = new Item[1];
			Item item = new Item();
			item.id = "0";
			item.unitPrice = Localization.CurrencyStringForGateway(OrderTotal);
			request.item[0] = item;

			result = "ERROR";

			try 
			{
				ReplyMessage reply = Client.RunTransaction( request );
				if(reply.decision == "ACCEPT" || reply.decision == "REVIEW")
				{
					result = "OK";
					PaymentCleared = true;
					if(Common.TransactionMode() == "AUTH CAPTURE")
					{
						AVSResult = reply.ccAuthReply.avsCode;
						AuthorizationResult = reply.ccCaptureReply.reasonCode;
						AuthorizationCode = reply.ccAuthReply.authorizationCode;
						AuthorizationTransID = reply.requestID;
						TransactionCommandOut = String.Empty;
					}
					else
					{
						AVSResult = reply.ccAuthReply.avsCode;
						AuthorizationResult = reply.reasonCode;
						AuthorizationCode = reply.ccAuthReply.authorizationCode;
						AuthorizationTransID = reply.requestID;
						TransactionCommandOut = String.Empty;
					}
				}
				else
				{
					result = "Your transaction was NOT approved, reason code: " + reply.reasonCode + ".";
					switch (reply.reasonCode) 
					{
						case "101": 
							result += " The request is missing one or more required fields.";
							break;
						case "102": 
							result += " One or more fields in the request contains invalid data.";
							break;
						case "104": 
							result += " The merchantReferenceCode sent with this authorization request matches the merchantReferenceCode of another authorization request that you sent in the last 15 minutes.";
							break;
						case "150": 
							result += " Error: General system failure. Possible action: Wait a few minutes and resend the request.";
							break;
						case "151": 
							result += " Error: The request was received but there was a server timeout.";
							break;
						case "152": 
							result += " Error: The request was received, but a service did not finish running in time.";
							break;
						case "201": 
							result += " The issuing bank has questions about the request. You do not receive an authorization code programmatically, but you might receive one verbally by calling the processor.";
							break;
						case "202": 
							result += " CARD HAS EXPIRED.";
							break;
						case "203": 
							result += " CARD WAS DECLINED.";
							break;
						case "204": 
							result += " INSUFFICIENT FUNDS. Please use a different card or select another form of payment.";
							break;
						case "205": 
							result += " INVALID CARD.";
							break;
						case "207": 
							result += " BANK IS UNVAVAILBLE.";
							break;
						case "208": 
							result += " CARD IS NOT ACTIVE.";
							break;
						case "210": 
							result += " CARD IS AT LIMIT.";
							break;
						case "211": 
							result += " INVALID CARD VERIFICATION CODE";
							break;
						case "231": 
							result += " INVALID ACCOUNT NUMBER";
							break;
						default:
							break;
					}
				}
			} 
			catch (Exception ex) 
			{
				result = ex.Message; //ProcessReply(reply);
			}
#endif

			return result;
		}

	}
}
