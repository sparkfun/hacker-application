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
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontGateways
{

	public class MyPolicy : ICertificatePolicy 
	{
		public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem) 
		{
			return true;
		}
	}

	/// <summary>
	/// Summary description for PayFuse.
	/// </summary>
	public class PayFuse
	{
		public PayFuse() {}

		static public String CaptureOrder(int OrderNumber)
		{
			String result = "OK";

			StringBuilder transactionCommand = new StringBuilder(5000);
			DB.ExecuteSQL("update orders set CaptureTXCommand=NULL, CaptureTXResult=NULL where OrderNumber=" + OrderNumber.ToString());
			bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
			String TransID = String.Empty;
			Decimal OrderTotal = System.Decimal.Zero;
			IDataReader rs = DB.GetRS("select * from orders where OrderNumber=" + OrderNumber.ToString());
			if(rs.Read())
			{
				TransID = DB.RSField(rs,"AuthorizationPNREF");
				OrderTotal = DB.RSFieldDecimal(rs,"OrderTotal");
			}
			rs.Close();			
			
			transactionCommand.Append("<EngineDocList>");
			transactionCommand.Append("<DocVersion>1.0</DocVersion>");
			transactionCommand.Append("<EngineDoc>");
			transactionCommand.Append("<ContentType>OrderFormDoc</ContentType>");
			transactionCommand.Append("<User>");
			transactionCommand.Append("<Name>" + Common.AppConfig("PayFuse.UserID") + "</Name>");
			transactionCommand.Append("<Password>" + Common.AppConfig("PayFuse.Password") + "</Password>");
			transactionCommand.Append("<Alias>" + Common.AppConfig("PayFuse.Alias") + "</Alias>");
			transactionCommand.Append("</User>");
			transactionCommand.Append("<Instructions>");
			transactionCommand.Append("<Pipeline>Payment</Pipeline>");
			transactionCommand.Append("</Instructions>");
			transactionCommand.Append("<OrderFormDoc>");
			transactionCommand.Append("<Mode>" + Common.IIF(Common.AppConfigBool("UseLiveTransactions") , "P" , "Y") + "</Mode>");
			transactionCommand.Append("<Id>" + TransID.ToString() + "</Id>");
			transactionCommand.Append("<Transaction>");
			transactionCommand.Append("<Type>PostAuth</Type>");
			transactionCommand.Append("<CurrentTotals>");
			transactionCommand.Append("<Totals>");
			transactionCommand.Append("<Total DataType=\"Money\" Currency=\"" + Localization.StoreCurrencyNumericCode() + "\">" + Localization.CurrencyStringForGateway(OrderTotal) + "</Total>");
			transactionCommand.Append("</Totals>");
			transactionCommand.Append("</CurrentTotals>");
			transactionCommand.Append("</Transaction>");
			transactionCommand.Append("</OrderFormDoc>");
			transactionCommand.Append("</EngineDoc>");
			transactionCommand.Append("</EngineDocList>");

			DB.ExecuteSQL("update orders set CaptureTXCommand=" + DB.SQuote(transactionCommand.ToString()) + " where OrderNumber=" + OrderNumber.ToString());

			if(TransID.Length == 0 || TransID == "0")
			{
				result = "Invalid or Empty Transaction ID";
			}
			else
			{

				try
				{
					ASCIIEncoding encoding = new ASCIIEncoding();
					byte[]  data = encoding.GetBytes("XmlPostVar=" + HttpContext.Current.Server.UrlEncode(transactionCommand.ToString()));

					// Prepare web request...
					String AuthServer = Common.IIF(useLiveTransactions , Common.AppConfig("PayFuse.LIVE_SERVER") , Common.AppConfig("PayFuse.TEST_SERVER"));
					HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
					myRequest.ContentType = "text/xml;charset=\"utf-8\"";
					myRequest.Accept = "text/xml";
					myRequest.Method = "POST";
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

					XmlDocument responseXml = new XmlDocument();
					String replyCode  = String.Empty;
					String authResponseMsg = String.Empty;
					try
					{
						//Make sure it's good XML
						responseXml.LoadXml(rawResponseString.Trim());
						//Have good Xml. Lets make it pretty
						rawResponseString = Common.FormatXml(responseXml);
					}
					catch
					{
						authResponseMsg = "GARBLED RESPONSE FROM THE GATEWAY";
					}

					if(authResponseMsg.Length == 0)
					{
						try
						{
							replyCode = responseXml.SelectSingleNode("//CcErrCode").InnerText;
							authResponseMsg = responseXml.SelectSingleNode("//CcReturnMsg").InnerText + " " + responseXml.SelectSingleNode("//Notice").InnerText;
						}
						catch
						{
							authResponseMsg = "Could not find CcErrCode In Gateway Response";
						}
					}

					DB.ExecuteSQL("update orders set CaptureTXResult=" + DB.SQuote(rawResponseString)  + " where OrderNumber=" + OrderNumber.ToString());
					if(replyCode == "1")
					{
						result = "OK";
					}
					else
					{
						result = authResponseMsg;
					}
				}
				catch
				{
					result = "NO RESPONSE FROM GATEWAY!";
				}
			}
			return result;
		}

		static public String VoidOrder(int OrderNumber)
		{
			String result = "OK";

			StringBuilder transactionCommand = new StringBuilder(5000);
			DB.ExecuteSQL("update orders set VoidTXCommand=NULL, VoidTXResult=NULL where OrderNumber=" + OrderNumber.ToString());
			bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
			String TransID = String.Empty;
			Decimal OrderTotal = System.Decimal.Zero;
			IDataReader rs = DB.GetRS("select * from orders where OrderNumber=" + OrderNumber.ToString());
			if(rs.Read())
			{
				TransID = DB.RSField(rs,"AuthorizationPNREF");
				OrderTotal = DB.RSFieldDecimal(rs,"OrderTotal");
			}
			rs.Close();			
			
			transactionCommand.Append("<EngineDocList>");
			transactionCommand.Append("<DocVersion>1.0</DocVersion>");
			transactionCommand.Append("<EngineDoc>");
			transactionCommand.Append("<ContentType>OrderFormDoc</ContentType>");
			transactionCommand.Append("<User>");
			transactionCommand.Append("<Name>" + Common.AppConfig("PayFuse.UserID") + "</Name>");
			transactionCommand.Append("<Password>" + Common.AppConfig("PayFuse.Password") + "</Password>");
			transactionCommand.Append("<Alias>" + Common.AppConfig("PayFuse.Alias") + "</Alias>");
			transactionCommand.Append("</User>");
			transactionCommand.Append("<Instructions>");
			transactionCommand.Append("<Pipeline>Payment</Pipeline>");
			transactionCommand.Append("</Instructions>");
			transactionCommand.Append("<OrderFormDoc>");
			transactionCommand.Append("<Mode>" + Common.IIF(Common.AppConfigBool("UseLiveTransactions") , "P" , "Y") + "</Mode>");
			transactionCommand.Append("<Id>" + TransID.ToString() + "</Id>");
			transactionCommand.Append("<Transaction>");
			transactionCommand.Append("<Type>Void</Type>");
			transactionCommand.Append("</Transaction>");
			transactionCommand.Append("</OrderFormDoc>");
			transactionCommand.Append("</EngineDoc>");
			transactionCommand.Append("</EngineDocList>");

			DB.ExecuteSQL("update orders set VoidTXCommand=" + DB.SQuote(transactionCommand.ToString()) + " where OrderNumber=" + OrderNumber.ToString());

			if(TransID.Length == 0 || TransID == "0")
			{
				result = "Invalid or Empty Transaction ID";
			}
			else
			{

				try
				{
					ASCIIEncoding encoding = new ASCIIEncoding();
					byte[]  data = encoding.GetBytes("XmlPostVar=" + HttpContext.Current.Server.UrlEncode(transactionCommand.ToString()));

					// Prepare web request...
					String AuthServer = Common.IIF(useLiveTransactions , Common.AppConfig("PayFuse.LIVE_SERVER") , Common.AppConfig("PayFuse.TEST_SERVER"));
					HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
					myRequest.ContentType = "text/xml;charset=\"utf-8\"";
					myRequest.Accept = "text/xml";
					myRequest.Method = "POST";
					//myRequest.Headers.Add ("MIME-Version", "1.0");
					//myRequest.Headers.Add ("Request-number", "1");
					//myRequest.Headers.Add ("Content-transfer-encoding", "text");
					//myRequest.Headers.Add ("Document-type", "Request");
					//myRequest.ContentType = "text/XML";
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

					XmlDocument responseXml = new XmlDocument();
					String replyCode  = String.Empty;
					String authResponseMsg = String.Empty;
					try
					{
						//Make sure it's good XML
						responseXml.LoadXml(rawResponseString.Trim());
						//Have good Xml. Lets make it pretty
						rawResponseString = Common.FormatXml(responseXml);
					}
					catch
					{
						authResponseMsg = "GARBLED RESPONSE FROM THE GATEWAY";
					}

					if(authResponseMsg.Length == 0)
					{
						try
						{
							replyCode = responseXml.SelectSingleNode("//CcErrCode").InnerText;
							authResponseMsg = responseXml.SelectSingleNode("//CcReturnMsg").InnerText + " " + responseXml.SelectSingleNode("//Notice").InnerText;
						}
						catch
						{
							authResponseMsg = "Could not find CcErrCode In Gateway Response";
						}
					}

					DB.ExecuteSQL("update orders set VoidTXResult=" + DB.SQuote(rawResponseString)  + " where OrderNumber=" + OrderNumber.ToString());
					if(replyCode == "1")
					{
						result = "OK";
					}
					else
					{
						result = authResponseMsg;
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
			String result = "OK";

			StringBuilder transactionCommand = new StringBuilder(5000);
			DB.ExecuteSQL("update orders set RefundTXCommand=NULL, RefundTXResult=NULL where OrderNumber=" + OrderNumber.ToString());
			bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
			String TransID = String.Empty;
			Decimal OrderTotal = System.Decimal.Zero;
			IDataReader rs = DB.GetRS("select * from orders where OrderNumber=" + OrderNumber.ToString());
			if(rs.Read())
			{
				TransID = DB.RSField(rs,"AuthorizationPNREF");
				OrderTotal = DB.RSFieldDecimal(rs,"OrderTotal");
			}
			rs.Close();			
			
			transactionCommand.Append("<EngineDocList>");
			transactionCommand.Append("<DocVersion>1.0</DocVersion>");
			transactionCommand.Append("<EngineDoc>");
			transactionCommand.Append("<ContentType>OrderFormDoc</ContentType>");
			transactionCommand.Append("<User>");
			transactionCommand.Append("<Name>" + Common.AppConfig("PayFuse.UserID") + "</Name>");
			transactionCommand.Append("<Password>" + Common.AppConfig("PayFuse.Password") + "</Password>");
			transactionCommand.Append("<Alias>" + Common.AppConfig("PayFuse.Alias") + "</Alias>");
			transactionCommand.Append("</User>");
			transactionCommand.Append("<Instructions>");
			transactionCommand.Append("<Pipeline>Payment</Pipeline>");
			transactionCommand.Append("</Instructions>");
			transactionCommand.Append("<OrderFormDoc>");
			transactionCommand.Append("<Mode>" + Common.IIF(Common.AppConfigBool("UseLiveTransactions") , "P" , "Y") + "</Mode>");
			transactionCommand.Append("<Id>" + TransID.ToString() + "</Id>");
			transactionCommand.Append("<Transaction>");
			transactionCommand.Append("<Type>Credit</Type>");
			transactionCommand.Append("<CurrentTotals>");
			transactionCommand.Append("<Totals>");
			transactionCommand.Append("<Total DataType=\"Money\" Currency=\"" + Localization.StoreCurrencyNumericCode() + "\">" + Localization.CurrencyStringForGateway(OrderTotal) + "</Total>");
			transactionCommand.Append("</Totals>");
			transactionCommand.Append("</CurrentTotals>");
			transactionCommand.Append("</Transaction>");
			transactionCommand.Append("</OrderFormDoc>");
			transactionCommand.Append("</EngineDoc>");
			transactionCommand.Append("</EngineDocList>");

			DB.ExecuteSQL("update orders set RefundTXCommand=" + DB.SQuote(transactionCommand.ToString()) + " where OrderNumber=" + OrderNumber.ToString());

			if(TransID.Length == 0 || TransID == "0")
			{
				result = "Invalid or Empty Transaction ID";
			}
			else
			{

				try
				{
					ASCIIEncoding encoding = new ASCIIEncoding();
					byte[]  data = encoding.GetBytes("XmlPostVar=" + HttpContext.Current.Server.UrlEncode(transactionCommand.ToString()));

					// Prepare web request...
					String AuthServer = Common.IIF(useLiveTransactions , Common.AppConfig("PayFuse.LIVE_SERVER") , Common.AppConfig("PayFuse.TEST_SERVER"));
					HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
					myRequest.ContentType = "text/xml;charset=\"utf-8\"";
					myRequest.Accept = "text/xml";
					myRequest.Method = "POST";
					//myRequest.Headers.Add ("MIME-Version", "1.0");
					//myRequest.Headers.Add ("Request-number", "1");
					//myRequest.Headers.Add ("Content-transfer-encoding", "text");
					//myRequest.Headers.Add ("Document-type", "Request");
					//myRequest.ContentType = "text/XML";
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

					XmlDocument responseXml = new XmlDocument();
					String replyCode  = String.Empty;
					String authResponseMsg = String.Empty;
					try
					{
						//Make sure it's good XML
						responseXml.LoadXml(rawResponseString.Trim());
						//Have good Xml. Lets make it pretty
						rawResponseString = Common.FormatXml(responseXml);
					}
					catch
					{
						authResponseMsg = "GARBLED RESPONSE FROM THE GATEWAY";
					}

					if(authResponseMsg.Length == 0)
					{
						try
						{
							replyCode = responseXml.SelectSingleNode("//CcErrCode").InnerText;
							authResponseMsg = responseXml.SelectSingleNode("//CcReturnMsg").InnerText + " " + responseXml.SelectSingleNode("//Notice").InnerText;
						}
						catch
						{
							authResponseMsg = "Could not find CcErrCode In Gateway Response";
						}
					}

					DB.ExecuteSQL("update orders set RefundTXResult=" + DB.SQuote(rawResponseString)  + " where OrderNumber=" + OrderNumber.ToString());
					if(replyCode == "1")
					{
						result = "OK";
					}
					else
					{
						result = authResponseMsg;
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

			AuthorizationCode = String.Empty;
			AuthorizationResult = String.Empty;
			AuthorizationTransID = String.Empty;
			AVSResult = String.Empty;
			PaymentCleared = false;
			TransactionCommandOut = String.Empty;

			transactionCommand.Append("<EngineDocList>");
			transactionCommand.Append("<DocVersion>1.0</DocVersion>");
			transactionCommand.Append("<EngineDoc>");
			transactionCommand.Append("<ContentType>OrderFormDoc</ContentType>");
			transactionCommand.Append("<User>");
			transactionCommand.Append("<Name>" + Common.AppConfig("PayFuse.UserID") + "</Name>");
			transactionCommand.Append("<Password>" + Common.AppConfig("PayFuse.Password") + "</Password>");
			transactionCommand.Append("<Alias>" + Common.AppConfig("PayFuse.Alias") + "</Alias>");
			transactionCommand.Append("</User>");
			transactionCommand.Append("<Instructions>");
			transactionCommand.Append("<Pipeline>Payment</Pipeline>");
			transactionCommand.Append("</Instructions>");
			transactionCommand.Append("<OrderFormDoc>");
			transactionCommand.Append("<Mode>" + Common.IIF(Common.AppConfigBool("UseLiveTransactions") , "P" , "Y") + "</Mode>");
			transactionCommand.Append("<Comments>Order Number: " + OrderNumber.ToString() + ", CustomerID=" + cart._thisCustomer._customerID.ToString() + "</Comments>");
			transactionCommand.Append("<Consumer>");
			transactionCommand.Append("<Email>" + Common.XmlEncode(cart._email) + "</Email>");
			transactionCommand.Append("<PaymentMech>");
			transactionCommand.Append("<CreditCard>");
			transactionCommand.Append("<Number>" + CardNumber + "</Number>");
			transactionCommand.Append("<Expires DataType=\"ExpirationDate\" Locale=\"" + Localization.StoreCurrencyNumericCode() + "\">" + CardExpirationMonth.PadLeft(2,'0') + "/" + CardExpirationYear.ToString().Substring(2,2) + "</Expires>");
			transactionCommand.Append("<Cvv2Val>" + CardExtraCode + "</Cvv2Val>");
			transactionCommand.Append("<Cvv2Indicator>1</Cvv2Indicator>");
			transactionCommand.Append("</CreditCard>");
			transactionCommand.Append("</PaymentMech>");
			transactionCommand.Append("<BillTo>");
			transactionCommand.Append("<Location>");
			transactionCommand.Append("<TelVoice>" + Common.XmlEncode(cart._billingAddress.Phone) + "</TelVoice>");
			transactionCommand.Append("<TelFax/>");
			transactionCommand.Append("<Address>");
			transactionCommand.Append("<Name>" + Common.XmlEncode((cart._billingAddress.FirstName + " " + cart._billingAddress.LastName)) + "</Name>");
			transactionCommand.Append("<Street1>" + Common.XmlEncode(cart._billingAddress.Address1) + "</Street1>");
			transactionCommand.Append("<Street2>" + Common.XmlEncode(cart._billingAddress.Address2) + "</Street2>");
			transactionCommand.Append("<City>" + Common.XmlEncode(cart._billingAddress.City) + "</City>");
			transactionCommand.Append("<StateProv>" + Common.XmlEncode(cart._billingAddress.State) + "</StateProv>");
			transactionCommand.Append("<PostalCode>" + Common.XmlEncode(cart._billingAddress.Zip) + "</PostalCode>");
			transactionCommand.Append("<Country>" + Common.XmlEncode(Localization.StoreCurrencyNumericCode()) + "</Country>");
			transactionCommand.Append("<Company>" + Common.XmlEncode(cart._billingAddress.Company) + "</Company>");
			transactionCommand.Append("</Address>");
			transactionCommand.Append("</Location>");
			transactionCommand.Append("</BillTo>");
			transactionCommand.Append("</Consumer>");
			transactionCommand.Append("<Transaction>");
			transactionCommand.Append("<Type>" + Common.IIF(Common.TransactionMode() == "AUTH", "PreAuth", "Auth") + "</Type>");
			transactionCommand.Append("<CurrentTotals>");
			transactionCommand.Append("<Totals>");
			transactionCommand.Append("<Total DataType=\"Money\" Currency=\"" + Localization.StoreCurrencyNumericCode() + "\">" + Localization.CurrencyStringForGateway(OrderTotal).Replace(".","").Replace("$","").Replace(",","") + "</Total>");
			transactionCommand.Append("</Totals>");
			transactionCommand.Append("</CurrentTotals>");
			transactionCommand.Append("</Transaction>");
			transactionCommand.Append("</OrderFormDoc>");
			transactionCommand.Append("</EngineDoc>");
			transactionCommand.Append("</EngineDocList>");

			// Is the command good XML
			XmlDocument cmdXml = new XmlDocument();
			try
			{
				cmdXml.LoadXml(transactionCommand.ToString());
			}
			catch
			{
				return "Transaction command XML is not valid.";
			}
			//Have good Xml. Lets make it pretty
			transactionCommand.Length = 0; //Clear the builder
			transactionCommand.Append(Common.FormatXml(cmdXml));
			cmdXml = null;


			byte[]  data = encoding.GetBytes("XmlPostVar=" + HttpContext.Current.Server.UrlEncode(transactionCommand.ToString()));

			// Prepare web request...
			//System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();
			String AuthServer = Common.IIF(useLiveTransactions , Common.AppConfig("PayFuse.LIVE_SERVER") , Common.AppConfig("PayFuse.TEST_SERVER"));
			HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
			if(!useLiveTransactions)
			{
				// must provide a port:
			}
			myRequest.ContentType = "text/xml;charset=\"utf-8\"";
			myRequest.Method = "POST";
			myRequest.Headers.Add ("Content-transfer-encoding", "text");
			myRequest.Headers.Add ("Document-type", "Request");
			myRequest.ContentType = "text/XML";
			myRequest.ContentLength = data.Length;
			Stream newStream = myRequest.GetRequestStream();
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

			XmlDocument responseXml = new XmlDocument();
			String replyCode  = String.Empty;
			String authResponseMsg = String.Empty;
			try
			{
				//Make sure it's good XML
				responseXml.LoadXml(rawResponseString.Trim());
				//Have good Xml. Lets make it pretty
				rawResponseString = Common.FormatXml(responseXml);
			}
			catch
			{
				authResponseMsg = "GARBLED RESPONSE FROM THE GATEWAY";
			}

			PaymentCleared = false;
			AuthorizationResult = rawResponseString;
			
			if(authResponseMsg.Length == 0)
			{
				try
				{
					replyCode = responseXml.SelectSingleNode("//CcErrCode").InnerText;
					authResponseMsg = responseXml.SelectSingleNode("//CcReturnMsg").InnerText + " " + responseXml.SelectSingleNode("//Notice").InnerText;
				}
				catch
				{
					authResponseMsg = "Could not find CcErrCode In Gateway Response";
				}
			}

			try
			{
				AuthorizationCode = responseXml.SelectSingleNode("//AuthCode").InnerText;
			}
			catch {}

			try
			{
				AuthorizationTransID = responseXml.SelectSingleNode("//TransactionId").InnerText;
			}
			catch {}

			AVSResult = String.Empty; // TBD
			TransactionCommandOut = transactionCommand.ToString();
			
			if(replyCode == "1")
			{
				result = "OK";
				PaymentCleared = true;
			}
			else
			{
				result = authResponseMsg;
//				if(replyCode.Length != 0)
//				{
//					result = "Error: " + replyCode + ", Reason=";
//					switch(replyCode)
//					{
//						case "2008":
//							result += "Invalid MerchantID";
//							break;
//						case "1052":
//							result += "Declined";
//							break;
//						case "1057":
//							result += "Declined";
//							break;
//						case "1058":
//							result += "Declined";
//							break;
//						case "50":
//							result += "Declined";
//							break;
//						case "1053":
//							result += "Declined";
//							break;
//						case "1002":
//							result += "Invalid Amount";
//							break;
//						case "1017":
//							result += "Invalid Card Number";
//							break;
//						case "1050":
//							result += "Insufficient Funds";
//							break;
//						case "1051":
//							result += "Card Is Expired";
//							break;
//						case "1059":
//							result += "Insufficient Funds";
//							break;
//						case "1060":
//							result += "Insufficient Funds";
//							break;
//						case "1013":
//							result += "Invalid Date";
//							break;
//						default:
//							break;
//					}
//				}
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
				String sql = "insert into FailedTransaction(CustomerID,OrderDate,PaymentGateway,TransactionCommand,TransactionResult) values(" + cart._thisCustomer._customerID.ToString() + "," + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + "," + DB.SQuote("PAYFUSE") + "," + DB.SQuote(transactionCommand.ToString().Replace(CardNumber,CC)) + "," + DB.SQuote(rawResponseString) + ")";
				DB.ExecuteSQL(sql);
			}
			return result;
		}

	}
}
