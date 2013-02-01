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
	/// Summary description for NetBilling.
	/// </summary>
	public class NetBilling
	{
		public NetBilling() {}

		static public String CaptureOrder(int OrderNumber)
		{
			String result = "OK";

			DB.ExecuteSQL("update orders set CaptureTXCommand=NULL, CaptureTXResult=NULL where OrderNumber=" + OrderNumber.ToString());
			bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
			String TransID = String.Empty;
			String CardNumber = String.Empty;
			Decimal OrderTotal = System.Decimal.Zero;
			IDataReader rs = DB.GetRS("select * from orders where OrderNumber=" + OrderNumber.ToString());
			if(rs.Read())
			{
				TransID = DB.RSField(rs,"AuthorizationPNREF");
				CardNumber = Common.UnmungeString(DB.RSField(rs,"CardNumber"));
				OrderTotal = DB.RSFieldDecimal(rs,"OrderTotal");
			}
			rs.Close();

			ASCIIEncoding encoding = new ASCIIEncoding();
			StringBuilder transactionCommand = new StringBuilder(5000);

			transactionCommand.Append("tran_type=D");
			transactionCommand.Append("&account_id=" + Common.AppConfig("NetBilling.Account_ID"));
			transactionCommand.Append("&orig_id=" + TransID);

			DB.ExecuteSQL("update orders set CaptureTXCommand=" + DB.SQuote(transactionCommand.ToString()) + " where OrderNumber=" + OrderNumber.ToString());

			if(TransID.Length == 0 || TransID == "0")
			{
				result = "Invalid or Empty Transaction ID";
			}
			else if(CardNumber.Length == 0)
			{
				result = "Credit Card Number Not Found or Empty";
			}
			else
			{
				try
				{

					byte[]  data = encoding.GetBytes(transactionCommand.ToString());

					// Prepare web request...
					String AuthServer = Common.IIF(useLiveTransactions , Common.AppConfig("NetBilling.LIVE_SERVER") , Common.AppConfig("NetBilling.TEST_SERVER"));
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
					String[] statusArray = rawResponseString.Split('&');
					String resultCode = String.Empty;
					String replyMsg = String.Empty;
					for(int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
					{
						String[] lasKeyPair = statusArray[i].Split('=');
						switch(lasKeyPair[0].ToLower())
						{
							case "status_code":
								resultCode = lasKeyPair[1];
								break;
							case "auth_msg":
								replyMsg = lasKeyPair[1];
								break;
						}
					}
					String sql = String.Empty;

					DB.ExecuteSQL("update orders set CaptureTXResult=" + DB.SQuote(rawResponseString)  + " where OrderNumber=" + OrderNumber.ToString());
					if(resultCode != "0" && resultCode != "F" && resultCode != "D")
					{
						result = "OK";
					}
					else
					{
						result = replyMsg;
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
			String result = "VOID not supported by NetBilling Gateway";
			return result;
		}

		static public String RefundOrder(int OrderNumber)
		{
			String result = "OK";

			DB.ExecuteSQL("update orders set RefundTXCommand=NULL, RefundTXResult=NULL where OrderNumber=" + OrderNumber.ToString());
			bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
			String TransID = String.Empty;
			String CardNumber = String.Empty;
			Decimal OrderTotal = System.Decimal.Zero;
			IDataReader rs = DB.GetRS("select * from orders where OrderNumber=" + OrderNumber.ToString());
			if(rs.Read())
			{
				TransID = DB.RSField(rs,"AuthorizationPNREF");
				CardNumber = Common.UnmungeString(DB.RSField(rs,"CardNumber"));
				OrderTotal = DB.RSFieldDecimal(rs,"OrderTotal");
			}
			rs.Close();

			ASCIIEncoding encoding = new ASCIIEncoding();
			StringBuilder transactionCommand = new StringBuilder(5000);

			transactionCommand.Append("tran_type=R");
			transactionCommand.Append("&account_id=" + Common.AppConfig("NetBilling.Account_ID"));
			transactionCommand.Append("&orig_id=" + TransID);

			DB.ExecuteSQL("update orders set RefundTXCommand=" + DB.SQuote(transactionCommand.ToString()) + " where OrderNumber=" + OrderNumber.ToString());

			if(TransID.Length == 0 || TransID == "0")
			{
				result = "Invalid or Empty Transaction ID";
			}
			else if(CardNumber.Length == 0)
			{
				result = "Credit Card Number Not Found or Empty";
			}
			else
			{
				try
				{

					byte[]  data = encoding.GetBytes(transactionCommand.ToString());

					// Prepare web request...
					String AuthServer = Common.IIF(useLiveTransactions , Common.AppConfig("NetBilling.LIVE_SERVER") , Common.AppConfig("NetBilling.TEST_SERVER"));
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
					String[] statusArray = rawResponseString.Split('&');
					String resultCode = String.Empty;
					String replyMsg = String.Empty;
					for(int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
					{
						String[] lasKeyPair = statusArray[i].Split('=');
						switch(lasKeyPair[0].ToLower())
						{
							case "status_code":
								resultCode = lasKeyPair[1];
								break;
							case "auth_msg":
								replyMsg = lasKeyPair[1];
								break;
						}
					}
					String sql = String.Empty;

					DB.ExecuteSQL("update orders set RefundTXResult=" + DB.SQuote(rawResponseString)  + " where OrderNumber=" + OrderNumber.ToString());
					if(resultCode != "0" && resultCode != "F" && resultCode != "D")
					{
						result = "OK";
					}
					else
					{
						result = replyMsg;
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

			transactionCommand.Append("tran_type=" + Common.IIF(Common.TransactionMode() == "AUTH" , "A" , "S"));

			transactionCommand.Append("&account_id=" + Common.AppConfig("NetBilling.Account_ID"));
			transactionCommand.Append("&site_tag=" + Common.AppConfig("NetBilling.Site_Tag"));
			transactionCommand.Append("&pay_type=" + Common.AppConfig("NetBilling.Pay_Type"));

			transactionCommand.Append("&amount=" + Localization.CurrencyStringForGateway(OrderTotal));
			transactionCommand.Append("&card_number=" + CardNumber);
			transactionCommand.Append("&card_cvv2=" + CardExtraCode);
			transactionCommand.Append("&card_expire=" + CardExpirationMonth.PadLeft(2,'0') + CardExpirationYear.Substring(2,2));
			transactionCommand.Append("&user_data=" + cart._thisCustomer._customerID.ToString());
			transactionCommand.Append("&description=" + OrderNumber.ToString());
			transactionCommand.Append("&cust_phone=" + cart._billingAddress.Phone);

			if(CAVV.Length != 0 || ECI.Length != 0)
			{
				transactionCommand.Append("&3ds_cavv=" + CAVV);
				transactionCommand.Append("&3ds_xid=" + ECI);
			}


			if(Common.AppConfigBool("NetBilling.VERIFY_ADDRESSES"))
			{
				transactionCommand.Append("&bill_name1=" + Billing.FirstName);
				transactionCommand.Append("&bill_name2=" + Billing.LastName);
				transactionCommand.Append("&bill_street=" + Billing.Address1);
				transactionCommand.Append("&bill_city=" + Billing.City);
				transactionCommand.Append("&bill_state=" + Billing.State);
				transactionCommand.Append("&bill_zip=" + Billing.Zip);
				transactionCommand.Append("&bill_country=" + Billing.Country);
			}

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
				String AuthServer = Common.IIF(useLiveTransactions , Common.AppConfig("NetBilling.LIVE_SERVER") , Common.AppConfig("NetBilling.TEST_SERVER"));
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

				bool AVSOk = true;

				String[] statusArray = rawResponseString.Split('&');
				String resultCode = String.Empty;
				for(int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
				{
					String[] lasKeyPair = statusArray[i].Split('=');
					switch(lasKeyPair[0].ToLower())
					{
						case "trans_id":
							AuthorizationTransID = lasKeyPair[1];
							break;
						case "status_code":
							resultCode = lasKeyPair[1];
							break;
						case "auth_msg":
							AuthorizationResult = lasKeyPair[1];
							break;
						case "auth_code":
							AuthorizationCode = lasKeyPair[1];
							break;
						case "avs_code":
							AVSResult = lasKeyPair[1];
							break;
					}
				}

				PaymentCleared = false;
				//AuthorizationResult = rawResponseString;
				TransactionCommandOut = transactionCommand.ToString();
			
				if((resultCode != "0" && resultCode != "F" && resultCode != "D") && AVSOk)
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
					String sql = "insert into FailedTransaction(CustomerID,OrderDate,PaymentGateway,TransactionCommand,TransactionResult) values(" + cart._thisCustomer._customerID.ToString() + "," + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + "," + DB.SQuote("NETBILLING") + "," + DB.SQuote(transactionCommand.ToString().Replace(CardNumber,CC)) + "," + DB.SQuote(rawResponseString) + ")";
					DB.ExecuteSQL(sql);
				}
			}
			catch(Exception ex)
			{
				String s = ex.Message;
				result = "Error calling NetBilling gateway(" + s + "). Please retry your order in a few minutes";
			}
			return result;
		}

	}
}
