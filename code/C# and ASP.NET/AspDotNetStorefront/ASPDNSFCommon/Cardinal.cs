using System;
using System.Web;
using System.Web.SessionState;
using CardinalCommerce;

namespace AspDotNetStorefrontCommon
{
	/// <summary>
	/// Summary description for Cardinal.
	/// </summary>
	public class Cardinal
	{
		public Cardinal() {}

		// returns true if no errors, and card is enrolled:
		static public bool PreChargeLookup(String CardNumber, int CardExpirationYear, int CardExpirationMonth, int OrderNumber, decimal OrderTotal, String OrderDescription, out String ACSUrl, out String Payload, out String TransactionID, out String CardinalLookupResult)
		{
			CardinalCommerce.CentinelRequest ccRequest = new CardinalCommerce.CentinelRequest();
			CardinalCommerce.CentinelResponse ccResponse = new CardinalCommerce.CentinelResponse();

			// ==================================================================================
			// Construct the cmpi_lookup message
			// ==================================================================================

			ccRequest.add("MsgType", Common.AppConfig("CardinalCommerce.Centinel.MsgType.Lookup"));
			ccRequest.add("Version", Common.AppConfig("CardinalCommerce.Centinel.Version.Lookup"));
			ccRequest.add("MerchantId", Common.AppConfig("CardinalCommerce.Centinel.MerchantID"));
			ccRequest.add("ProcessorId", Common.AppConfig("CardinalCommerce.Centinel.ProcessorID"));
			ccRequest.add("PAN", CardNumber);
			ccRequest.add("PANExpr", CardExpirationYear.ToString().PadLeft(4,'0').Substring(2,2) + CardExpirationMonth.ToString().PadLeft(2,'0'));
			ccRequest.add("OrderNumber", OrderNumber.ToString());
			ccRequest.add("OrderDesc", OrderDescription);
			ccRequest.add("PurchaseAmount", Localization.CurrencyStringForGateway(OrderTotal)); 
			ccRequest.add("RawAmount", Localization.CurrencyStringForGateway(OrderTotal).Replace(",","").Replace(".","").Replace("$",""));
			ccRequest.add("PurchaseCurrency", Localization.StoreCurrencyNumericCode());
			ccRequest.add("UserAgent", Common.ServerVariables("HTTP_USER_AGENT"));
			//if(Recurring == "Y")
			//{
			//	ccRequest.add("Recurring", "Y");
			//	ccRequest.add("RecurringFrequency", RecurringFrequency);
			//	ccRequest.add("RecurringEnd", RecurringEnd);
			//}
			//else
			//{
			ccRequest.add("Recurring", "N");
			//}

			int NumAttempts = Common.AppConfigUSInt("CardinalCommerce.Centinel.NumRetries");
			if(NumAttempts == 0)
			{
				NumAttempts = 1;
			}
			bool CallWasOk = false;
			for(int i = 1; i <= NumAttempts; i++)
			{
				CallWasOk = true;
				try
				{
					String URL = Common.AppConfig("CardinalCommerce.Centinel.TransactionUrl.Test");
					if(Common.AppConfigBool("CardinalCommerce.Centinel.IsLive"))
					{
						URL = Common.AppConfig("CardinalCommerce.Centinel.TransactionUrl.Live");
					}
					ccResponse = ccRequest.sendHTTP(URL, Common.AppConfigUSInt("CardinalCommerce.Centinel.MapsTimeout"));
				}
				catch
				{
					CallWasOk = false;
				}
				if(CallWasOk)
				{
					break;
				}
			}

			Payload = String.Empty;
			ACSUrl = String.Empty;
			TransactionID = String.Empty;
			if(CallWasOk)
			{
				String errorNo = ccResponse.getValue("ErrorNo");
				String errorDesc = ccResponse.getValue("ErrorDesc");
				String enrolled = ccResponse.getValue("Enrolled");
				Payload = ccResponse.getValue("Payload");
				ACSUrl = ccResponse.getValue("ACSUrl");
				TransactionID = ccResponse.getValue("TransactionId");

				CardinalLookupResult = ccResponse.getUnparsedResponse();

				ccRequest = null;
				ccResponse = null;

				//======================================================================================
				// Assert that there was no error code returned and the Cardholder is enrolled in the
				// Payment Authentication Program prior to starting the Authentication process.
				//======================================================================================

				if(errorNo == "0" && enrolled == "Y")
				{
					return true;
				}
				return false;
			}
			ccRequest = null;
			ccResponse = null;
			CardinalLookupResult = "Cardinal Lookup Call Failed";
			return false;
		}

		static public String PreChargeAuthenticate(int OrderNumber, String PaRes, String TransactionID, out String PAResStatus, out String SignatureVerification, out String ErrorNo, out String ErrorDesc, out String CardinalAuthenticateResult)
		{
			CardinalCommerce.CentinelRequest ccRequest = new CardinalCommerce.CentinelRequest();
			CardinalCommerce.CentinelResponse ccResponse = new CardinalCommerce.CentinelResponse();

			ErrorNo = String.Empty;
			ErrorDesc = String.Empty;
			PAResStatus = String.Empty;
			SignatureVerification = String.Empty;


			if(PaRes.Length == 0 || TransactionID.Length == 0)
			{
				CardinalAuthenticateResult = "Insufficient Input Data";
				return "Error Invoking Cardinal Authentication - Session May Have Expired";
			}
			else
			{

				// ==================================================================================
				// Construct the cmpi_authenticate message
				// ==================================================================================

				ccRequest.add("MsgType", Common.AppConfig("CardinalCommerce.Centinel.MsgType.Authenticate")); //cmpi_authenticate
				ccRequest.add("Version", Common.AppConfig("CardinalCommerce.Centinel.Version.Authenticate"));
				ccRequest.add("MerchantId", Common.AppConfig("CardinalCommerce.Centinel.MerchantID"));
				ccRequest.add("ProcessorId", Common.AppConfig("CardinalCommerce.Centinel.ProcessorID")); 
				ccRequest.add("TransactionId", TransactionID);
				ccRequest.add("PAResPayload", HttpContext.Current.Server.HtmlEncode(PaRes));

				int NumAttempts = Common.AppConfigUSInt("CardinalCommerce.Centinel.NumRetries");
				if(NumAttempts == 0)
				{
					NumAttempts = 1;
				}
				bool CallWasOk = false;
				for(int i = 1; i <= NumAttempts; i++)
				{
					CallWasOk = true;
					try
					{
						String URL = Common.AppConfig("CardinalCommerce.Centinel.TransactionUrl.Test");
						if(Common.AppConfigBool("CardinalCommerce.Centinel.IsLive"))
						{
							URL = Common.AppConfig("CardinalCommerce.Centinel.TransactionUrl.Live");
						}
						ccResponse = ccRequest.sendHTTP(URL, Common.AppConfigUSInt("CardinalCommerce.Centinel.MapsTimeout"));
					}
					catch
					{
						CallWasOk = false;
					}
					if(CallWasOk)
					{
						break;
					}
				}

				if(CallWasOk)
				{
					ErrorNo = ccResponse.getValue("ErrorNo");
					ErrorDesc = ccResponse.getValue("ErrorDesc");
					String cavv = ccResponse.getValue("Cavv");
					String xid = ccResponse.getValue("Xid");
					PAResStatus = ccResponse.getValue("PAResStatus");
					SignatureVerification = ccResponse.getValue("SignatureVerification");
					String eciflag = ccResponse.getValue("EciFlag");

					//=====================================================================================
					// ************************************************************************************
					//				** Important Note **
					// ************************************************************************************
					//
					// Here you should persist the authentication results to your commerce system. A production
					// integration should, at a minimum, write the PAResStatus, Cavv, EciFlag, Xid to a database
					// for use when sending the authorization message to your gateway provider.
					//
					// Be sure not to simply //pass// the authentication results around from page to page, since
					// the values could be easily spoofed if that technique is used.
					//
					//=====================================================================================

					CardinalAuthenticateResult = ccResponse.getUnparsedResponse();
					String tmpS = ccResponse.getUnparsedResponse();
					ccRequest = null;
					ccResponse = null;
					return tmpS;
				}
				ccRequest = null;
				ccResponse = null;
				CardinalAuthenticateResult = "Cardinal Authenticate Call Failed";
				return "Error Invoking Cardinal Authentication - Please Retry Credit Card Entry";
			}
		}
	}
}
