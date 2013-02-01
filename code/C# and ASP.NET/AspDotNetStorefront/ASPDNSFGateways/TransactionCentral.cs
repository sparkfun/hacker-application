// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using System.Security;
using System.Text;
using System.Web.SessionState;
using System.IO;
using System.Net;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontGateways
{
	/// <summary>
	/// Summary description for TRANSACTIONCENTRAL.
	/// </summary>
	public class TransactionCentral
	{
		public TransactionCentral() {}

		static public String CaptureOrder(int OrderNumber)
		{
			String result = "CAPTURE IS NOT SUPPORTED FOR TRANSACTION CENTRAL GATEWAY";
			return result;
		}

		static public String VoidOrder(int OrderNumber)
		{
			String result = "OK";

			DB.ExecuteSQL("update orders set VoidTXCommand=NULL, VoidTXResult=NULL where OrderNumber=" + OrderNumber.ToString());
			bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
			String TransID = String.Empty;
			IDataReader rs = DB.GetRS("select * from orders where OrderNumber=" + OrderNumber.ToString());
			Decimal OrderTotal = System.Decimal.Zero;
			if(rs.Read())
			{
				TransID = DB.RSField(rs,"AuthorizationPNREF");
				OrderTotal = DB.RSFieldDecimal(rs,"OrderTotal");
			}
			rs.Close();

			ASCIIEncoding encoding = new ASCIIEncoding();
			StringBuilder transactionCommand = new StringBuilder(5000);

			transactionCommand.Append("MerchantID=" + Common.AppConfig("TRANSACTIONCENTRAL_MERCHANTID"));
			transactionCommand.Append("&TransID=" + TransID.ToString());
			transactionCommand.Append("&CreditAmount=" + Localization.CurrencyStringForGateway(OrderTotal));
			transactionCommand.Append("&CCRURL="); // we want immediate postback

			DB.ExecuteSQL("update orders set VoidTXCommand=" + DB.SQuote(transactionCommand.ToString()) + " where OrderNumber=" + OrderNumber.ToString());

			if(TransID.Length == 0 || TransID == "0")
			{
				result = "Invalid or Empty Transaction ID";
			}
			else
			{
				try
				{
					byte[]  data = encoding.GetBytes(transactionCommand.ToString());

					// Prepare web request...
					String AuthServer = Common.AppConfig("TRANSACTIONCENTRAL_VOID_SERVER");
					HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
					myRequest.Method = "POST";
					myRequest.ContentType="application/x-www-form-urlencoded";
					myRequest.ContentLength = data.Length;
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
					String[] statusArray = Common.ExtractBody(rawResponseString).Split('&');

					String TransIDResponse = String.Empty;
					String TransType = String.Empty;
					String Auth = String.Empty;
					String Notes = String.Empty;

					foreach(String s in statusArray)
					{
						String[] Keys = s.Split('=');
						switch(Keys[0].Trim().Replace("\t","").ToUpper())
						{
							case "TRANSID":
								TransIDResponse = Keys[1];
								break;
							case "AUTH":
								Auth = Keys[1];
								break;
							case "TRANSTYPE":
								TransType = Keys[1];
								break;
							case "NOTES":
								Notes = Keys[1];
								break;
						}
					}

					DB.ExecuteSQL("update orders set VoidTXResult=" + DB.SQuote(rawResponseString)  + " where OrderNumber=" + OrderNumber.ToString());

					if(Auth.Trim().ToUpper() != "DECLINED")
					{
						result = "OK";
					}
					else
					{
						result = Notes;
					}
				}
				catch
				{
					result = "NO RESPONSE FROM GATEWAY!";
				}

			}
			return result;
		}

		static public String RefundOrder(int OrderNumber)
		{
			// they seem to use the identical calls for both void & refund (seems strange to us)
			String result = "OK";

			DB.ExecuteSQL("update orders set RefundTXCommand=NULL, RefundTXResult=NULL where OrderNumber=" + OrderNumber.ToString());
			bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
			String TransID = String.Empty;
			IDataReader rs = DB.GetRS("select * from orders where OrderNumber=" + OrderNumber.ToString());
			Decimal OrderTotal = System.Decimal.Zero;
			if(rs.Read())
			{
				TransID = DB.RSField(rs,"AuthorizationPNREF");
				OrderTotal = DB.RSFieldDecimal(rs,"OrderTotal");
			}
			rs.Close();

			ASCIIEncoding encoding = new ASCIIEncoding();
			StringBuilder transactionCommand = new StringBuilder(5000);

			transactionCommand.Append("MerchantID=" + Common.AppConfig("TRANSACTIONCENTRAL_MERCHANTID"));
			transactionCommand.Append("&TransID=" + TransID.ToString());
			transactionCommand.Append("&CreditAmount=" + Localization.CurrencyStringForGateway(OrderTotal));
			transactionCommand.Append("&CCRURL="); // we want immediate postback

			DB.ExecuteSQL("update orders set RefundTXCommand=" + DB.SQuote(transactionCommand.ToString()) + " where OrderNumber=" + OrderNumber.ToString());

			if(TransID.Length == 0 || TransID == "0")
			{
				result = "Invalid or Empty Transaction ID";
			}
			else
			{
				try
				{
					byte[]  data = encoding.GetBytes(transactionCommand.ToString());

					// Prepare web request...
					String AuthServer = Common.AppConfig("TRANSACTIONCENTRAL_VOID_SERVER");
					HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
					myRequest.Method = "POST";
					myRequest.ContentType="application/x-www-form-urlencoded";
					myRequest.ContentLength = data.Length;
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
					String[] statusArray = Common.ExtractBody(rawResponseString).Split('&');

					String TransIDResponse = String.Empty;
					String TransType = String.Empty;
					String Auth = String.Empty;
					String Notes = String.Empty;

					foreach(String s in statusArray)
					{
						String[] Keys = s.Split('=');
						switch(Keys[0].Trim().Replace("\t","").ToUpper())
						{
							case "TRANSID":
								TransIDResponse = Keys[1];
								break;
							case "AUTH":
								Auth = Keys[1];
								break;
							case "TRANSTYPE":
								TransType = Keys[1];
								break;
							case "NOTES":
								Notes = Keys[1];
								break;
						}
					}

					DB.ExecuteSQL("update orders set RefundTXResult=" + DB.SQuote(rawResponseString)  + " where OrderNumber=" + OrderNumber.ToString());

					if(Auth.Trim().ToUpper() != "DECLINED")
					{
						result = "OK";
					}
					else
					{
						result = Notes;
					}
				}
				catch
				{
					result = "NO RESPONSE FROM GATEWAY!";
				}
			}
			return result;
		}

		static public String ProcessCard(int OrderNumber, ShoppingCart cart, String CardName, String CardType, String CardNumber, String CardExtraCode, String CardExpirationMonth, String CardExpirationYear, Decimal OrderTotal, bool useLiveTransactions, Address Billing,  Address Shipping, String CAVV, String ECI, String XID, out bool PaymentCleared, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut)
		{
			String result = "OK";

			ASCIIEncoding encoding = new ASCIIEncoding();
			StringBuilder transactionCommand = new StringBuilder(5000);

			transactionCommand.Append("MerchantID=" + Common.AppConfig("TRANSACTIONCENTRAL_MERCHANTID"));
			transactionCommand.Append("&Amount=" + Localization.CurrencyStringForGateway(OrderTotal));
			transactionCommand.Append("&REFID=" + OrderNumber.ToString());
			transactionCommand.Append("&AccountNo=" + CardNumber);
			transactionCommand.Append("&CCMonth=" + CardExpirationMonth.PadLeft(2,'0'));
			transactionCommand.Append("&CCYear=" + CardExpirationYear);
			transactionCommand.Append("&NameOnAccount=" + HttpContext.Current.Server.UrlEncode((Billing.FirstName + Billing.LastName)));
			transactionCommand.Append("&AVSADDR=" + HttpContext.Current.Server.UrlEncode(Billing.Address1));
			transactionCommand.Append("&AVSZIP=" + Billing.Zip);
			transactionCommand.Append("&CCRURL="); // we want immediate postback
			transactionCommand.Append("&CVV2=" + CardExtraCode);
			transactionCommand.Append("&USER1=" + cart._thisCustomer._customerID.ToString());
			transactionCommand.Append("&USER2=" + OrderNumber.ToString());
			transactionCommand.Append("&USER3=" + HttpContext.Current.Server.UrlEncode(Common.AppConfig("StoreName")));


			byte[]  data = encoding.GetBytes(transactionCommand.ToString());

			// Prepare web request...
			PaymentCleared = false;
			AuthorizationCode = String.Empty;
			AuthorizationResult = String.Empty;
			AuthorizationTransID = String.Empty;
			AVSResult = String.Empty;
			TransactionCommandOut = String.Empty;
			try
			{
				String AuthServer = Common.IIF(useLiveTransactions , Common.AppConfig("TRANSACTIONCENTRAL_LIVE_SERVER") , Common.AppConfig("TRANSACTIONCENTRAL_TEST_SERVER"));
				HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
				myRequest.Method = "POST";
				myRequest.ContentType="application/x-www-form-urlencoded";
				myRequest.ContentLength = data.Length;
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

				PaymentCleared = false;
				AuthorizationCode = String.Empty;
				AuthorizationResult = String.Empty;
				AuthorizationTransID = String.Empty;
				AVSResult = String.Empty;
				TransactionCommandOut = transactionCommand.ToString();

				// rawResponseString now has gateway response
				String[] statusArray = Common.ExtractBody(rawResponseString).Split('&');

				foreach(String s in statusArray)
				{
					String[] Keys = s.Split('=');
					switch(Keys[0].Trim().Replace("\t","").ToUpper())
					{
						case "TRANSID":
							AuthorizationTransID = Keys[1];
							break;
						case "REFNO":
							break;
						case "AUTH":
							AuthorizationCode = Keys[1];
							break;
						case "AVSCODE":
							AVSResult = Keys[1];
							break;
						case "CVV2RESPONSEMSG":
							// not used
							break;
						case "NOTES":
							AuthorizationResult = Keys[1];
							break;
					}
				}

				if(AuthorizationCode.Trim().ToUpper() != "DECLINED")
				{
					result = "OK";
					PaymentCleared = true;
				}
				else
				{
					result = AuthorizationResult;
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
					String sql = "insert into FailedTransaction(CustomerID,OrderDate,PaymentGateway,TransactionCommand,TransactionResult) values(" + cart._thisCustomer._customerID.ToString() + "," + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + "," + DB.SQuote("TRANSACTIONCENTRAL") + "," + DB.SQuote(transactionCommand.ToString().Replace(CardNumber,CC)) + "," + DB.SQuote(rawResponseString) + ")";
					DB.ExecuteSQL(sql);
				}
			}
			catch
			{
				result = "Error calling TransactionCentral gateway. Please retry your order in a few minutes";
			}
			return result;
		}

	}
}
