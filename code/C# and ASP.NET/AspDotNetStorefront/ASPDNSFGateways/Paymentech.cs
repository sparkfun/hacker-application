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
	/// Summary description for Paymentech.
	/// </summary>
	public class Paymentech
	{
		public Paymentech() {}

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

			transactionCommand.Append("<Request>\n");
			transactionCommand.Append("<AC>\n");
			transactionCommand.Append("<CommonData>\n");
			transactionCommand.Append("<CommonMandatory AuthOverrideInd=\"N\" LangInd=\"00\" CardHolderAttendanceInd=\"01\" HcsTcsInd=\"T\" TxCatg=\"7\" MessageType=\"" + Common.IIF(Common.TransactionMode() == "AUTH", "A", "AC") + "\" Version=\"2\" TzCode=\"" + Common.AppConfig("PAYMENTECH_MERCHANT_TZCODE") + "\">\n");
			transactionCommand.Append("<AccountNum AccountTypeInd=\"91\">" + CardNumber + "</AccountNum>\n");
			transactionCommand.Append("<POSDetails POSEntryMode=\"01\"/>\n");
			transactionCommand.Append("<MerchantID>" + Common.AppConfig("PAYMENTECH_MERCHANT_ID") + "</MerchantID>\n");
			transactionCommand.Append("<TerminalID TermEntCapInd=\"05\" CATInfoInd=\"06\" TermLocInd=\"01\" CardPresentInd=\"N\" POSConditionCode=\"59\" AttendedTermDataInd=\"01\">" + Common.AppConfig("PAYMENTECH_MERCHANT_TERMINAL_ID") + "</TerminalID>\n");
			transactionCommand.Append("<BIN>" + Common.AppConfig("PAYMENTECH_BIN") + "</BIN>\n");
			transactionCommand.Append("<OrderID>" + OrderNumber.ToString().PadRight(16,'0') + "</OrderID>\n");
			transactionCommand.Append("<AmountDetails>\n");
			transactionCommand.Append("<Amount>" + Localization.CurrencyStringForGateway(OrderTotal).Replace(",","").Replace(".","").PadLeft(12,'0') + "</Amount>\n");
			transactionCommand.Append("</AmountDetails>\n");
			transactionCommand.Append("<TxTypeCommon TxTypeID=\"G\"/>\n");
			transactionCommand.Append("<Currency CurrencyCode=\"" + Localization.StoreCurrencyNumericCode() + "\" CurrencyExponent=\"2\"/>\n");
			transactionCommand.Append("<CardPresence>\n");
			transactionCommand.Append("<CardNP>\n");
			transactionCommand.Append("<Exp>" + CardExpirationMonth.PadLeft(2,'0') + CardExpirationYear.ToString().Substring(2,2) + "</Exp>\n");
			transactionCommand.Append("</CardNP>\n");
			transactionCommand.Append("</CardPresence>\n");
			transactionCommand.Append("<TxDateTime/>\n");
			transactionCommand.Append("</CommonMandatory>\n");
			transactionCommand.Append("<CommonOptional>\n");
			transactionCommand.Append("<Comments>CustomerID: " + cart._thisCustomer._customerID.ToString() + "</Comments>\n");
			if(!Common.AppConfigBool("CardExtraCodeIsOptional"))
			{
				transactionCommand.Append("<CardSecVal>" + CardExtraCode + "</CardSecVal>\n");
			}
			transactionCommand.Append("<ECommerceData ECSecurityInd=\"07\">\n");
			transactionCommand.Append("<ECOrderNum>" + OrderNumber.ToString().PadRight(16,'0') + "</ECOrderNum>\n");
			transactionCommand.Append("</ECommerceData>\n");
			transactionCommand.Append("</CommonOptional>\n");
			transactionCommand.Append("</CommonData>\n");
			transactionCommand.Append("<Auth>\n");
			transactionCommand.Append("<AuthMandatory FormatInd=\"H\"/>\n");
			if(Common.AppConfigBool("PAYMENTECH_VERIFY_ADDRESSES"))
			{
				transactionCommand.Append("<AuthOptional>\n");
				transactionCommand.Append("<AVSextended>\n");
				transactionCommand.Append("<AVSname>" + (Billing.FirstName + " " + Billing.LastName) + "</AVSname>\n");
				transactionCommand.Append("<AVSaddress1>" + Billing.Address1 + "</AVSaddress1>\n");
				transactionCommand.Append("<AVSaddress2>" + Billing.Suite + "</AVSaddress2>\n");
				transactionCommand.Append("<AVScity>" + Billing.City + "</AVScity>\n");
				transactionCommand.Append("<AVSstate>" + Billing.State + "</AVSstate>\n");
				transactionCommand.Append("<AVSzip>" + Billing.Zip + "</AVSzip>\n");
				transactionCommand.Append("</AVSextended>\n");
				transactionCommand.Append("</AuthOptional>\n");
			}
			transactionCommand.Append("</Auth>\n");
			transactionCommand.Append("<Cap>\n");
			transactionCommand.Append("<CapMandatory>\n");
			transactionCommand.Append("<EntryDataSrc>02</EntryDataSrc>\n");
			transactionCommand.Append("</CapMandatory>\n");
			transactionCommand.Append("<CapOptional/>\n");
			transactionCommand.Append("</Cap>\n");
			transactionCommand.Append("</AC>\n");
			transactionCommand.Append("</Request>\n");

			byte[]  data = encoding.GetBytes(transactionCommand.ToString());

			// Prepare web request...
			String AuthServer = Common.IIF(useLiveTransactions , Common.AppConfig("PAYMENTECH_LIVE_SERVER") , Common.AppConfig("PAYMENTECH_TEST_SERVER"));
			HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
			myRequest.Headers.Add ("MIME-Version", "1.0");
			myRequest.Headers.Add ("Request-number", "1");
			myRequest.Headers.Add ("Content-transfer-encoding", "text");
			myRequest.Headers.Add ("Document-type", "Request");
			myRequest.ContentType = "application/PTI22";
			myRequest.ContentLength = data.Length;
			myRequest.Method = "POST";
			Stream newStream=myRequest.GetRequestStream();
			// Send the data.
			newStream.Write(data,0,data.Length);
			newStream.Close();
			// get the response
			WebResponse myResponse;
			myResponse = myRequest.GetResponse();
			String rawResponseString = String.Empty;
			using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()) )
			{
				rawResponseString = sr.ReadToEnd();
				// Close and clean up the StreamReader
				sr.Close();
			}
			myResponse.Close();

			// rawResponseString now has gateway response

			String sql = String.Empty;
			String replyCode = Common.ExtractToken(rawResponseString,"<ApprovalStatus>","</ApprovalStatus>");
			String responseCode = Common.ExtractToken(rawResponseString,"<RespCode>","</RespCode>");
			String approvalCode = Common.ExtractToken(rawResponseString,"<AuthCode>","</AuthCode>");
			String authResponse = Common.ExtractToken(rawResponseString,"<StatusMsg>","</StatusMsg>");
			String TransID = Common.ExtractToken(rawResponseString,"<TxRefNum>","</TxRefNum>");

			int idx = authResponse.IndexOf(">");
			if(idx != -1)
			{
				// pick only text out:
				authResponse = authResponse.Substring(idx+1,authResponse.Length - idx - 1);
			}

			PaymentCleared = false;
			AuthorizationCode = approvalCode;
			AuthorizationResult = rawResponseString;
			AuthorizationTransID = TransID;
			AVSResult = String.Empty; // TBD
			TransactionCommandOut = transactionCommand.ToString();
			
			if(replyCode == "1")
			{
				result = "OK";
				PaymentCleared = true;
			}
			else if(replyCode == "0")
			{
				String CC = String.Empty;
				if(Common.AppConfigBool("StoreCCInDB"))
				{
					CC = Common.MungeString(CardNumber);
				}
				else
				{
					CC = "XXXXXXXXXXXX";
				}
				sql = "insert into FailedTransaction(CustomerID,OrderDate,PaymentGateway,TransactionCommand,TransactionResult) values(" + cart._thisCustomer._customerID.ToString() + "," + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + "," + DB.SQuote("PAYMENTECH") + "," + DB.SQuote(transactionCommand.ToString().Replace(CardNumber,CC)) + "," + DB.SQuote(rawResponseString) + ")";
				DB.ExecuteSQL(sql);
				result = "DECLINED";
			}
			else
			{
				result = "System Error: " + authResponse;
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
				sql = "insert into FailedTransaction(CustomerID,OrderDate,PaymentGateway,TransactionCommand,TransactionResult) values(" + cart._thisCustomer._customerID.ToString() + "," + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + "," + DB.SQuote("PAYMENTECH") + "," + DB.SQuote(transactionCommand.ToString().Replace(CardNumber,CC)) + "," + DB.SQuote(rawResponseString) + ")";
				DB.ExecuteSQL(sql);
			}
			return result;
		}

	}
}
