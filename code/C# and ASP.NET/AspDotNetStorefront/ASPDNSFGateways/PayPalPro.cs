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
	/// Summary description for PayPalPro.
	/// </summary>
	public class PayPalPro
	{
		public PayPalPro() {}

		static public String CaptureOrder(int OrderNumber)
		{
			HttpContext.Current.Session["GatewayMsg"] = String.Empty;
			String result = "OK";

			DB.ExecuteSQL("update orders set CaptureTXCommand=NULL, CaptureTXResult=NULL where OrderNumber=" + OrderNumber.ToString());
			bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
			String TransID = String.Empty;
			IDataReader rs = DB.GetRS("select * from orders  " + DB.GetNoLock() + " where OrderNumber=" + OrderNumber.ToString());
			if(rs.Read())
			{
				TransID = DB.RSField(rs,"AuthorizationPNREF");
			}
			rs.Close();			
			
			ASCIIEncoding encoding = new ASCIIEncoding();
			StringBuilder transactionCommand = new StringBuilder(5000);
			transactionCommand.Append("x_type=PRIOR_AUTH_CAPTURE"); 

			String X_Login = Common.AppConfig("PAYPALPRO_X_LOGIN");
			if(X_Login.Trim().ToUpper() == "REGISTRY")
			{
				WindowsRegistry reg = new WindowsRegistry(Common.AppConfig("EncryptKey.RegistryLocation"));
				X_Login = reg.Read("PAYPALPRO_X_LOGIN");
				reg = null;
			}

			String X_TranKey = Common.AppConfig("PAYPALPRO_X_TRAN_KEY");
			if(X_TranKey.Trim().ToUpper() == "REGISTRY")
			{
				WindowsRegistry reg = new WindowsRegistry(Common.AppConfig("EncryptKey.RegistryLocation"));
				X_TranKey = reg.Read("PAYPALPRO_X_TRAN_KEY");
				reg = null;
			}

			transactionCommand.Append("&x_login=" + X_Login);
			transactionCommand.Append("&x_tran_key=" + X_TranKey);
			transactionCommand.Append("&x_version=" + Common.AppConfig("PAYPALPRO_X_VERSION"));
			transactionCommand.Append("&x_test_request=" + Common.IIF(useLiveTransactions , "FALSE" , "TRUE"));
			transactionCommand.Append("&x_method=" + Common.AppConfig("PAYPALPRO_X_METHOD"));
			transactionCommand.Append("&x_delim_data=" + Common.AppConfig("PAYPALPRO_X_DELIM_DATA"));
			transactionCommand.Append("&x_delim_char=" + Common.AppConfig("PAYPALPRO_X_DELIM_CHAR"));
			transactionCommand.Append("&x_encap_char=" + Common.AppConfig("PAYPALPRO_X_ENCAP_CHAR"));
			transactionCommand.Append("&x_relay_response=" + Common.AppConfig("PAYPALPRO_X_RELAY_RESPONSE"));
			transactionCommand.Append("&x_customer_ip=" + Common.ServerVariables("REMOTE_ADDR"));
			transactionCommand.Append("&x_trans_id=" + TransID);

			DB.ExecuteSQL("update orders set CaptureTXCommand=" + DB.SQuote(transactionCommand.ToString()) + " where OrderNumber=" + OrderNumber.ToString());

			//			if(TransID.Length == 0 || TransID == "0")
			//			{
			//				result = "Invalid or Empty Transaction ID";
			//			}
			//			else
			//			{

			try
			{
				byte[]  data = encoding.GetBytes(transactionCommand.ToString());

				// Prepare web request...
				String AuthServer = Common.IIF(useLiveTransactions , Common.AppConfig("PAYPALPRO_LIVE_SERVER") , Common.AppConfig("PAYPALPRO_TEST_SERVER"));
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
				String rawResponseString = String.Empty;
				try
				{
					myResponse = myRequest.GetResponse();
					using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()) )
					{
						rawResponseString = sr.ReadToEnd();
						// Close and clean up the StreamReader
						sr.Close();
					}
					myResponse.Close();
				}
				catch(Exception ex)
				{
					HttpContext.Current.Session["GatewayMsg"] = Common.Session("GatewayMsg") + ex.Message;
					rawResponseString = "0|||Error Calling PayPalPro Payment Gateway||||||||";
				}

				// rawResponseString now has gateway response
				String[] statusArray = rawResponseString.Split(Common.AppConfig("PAYPALPRO_X_DELIM_CHAR").ToCharArray());
				// this seems to be a new item where auth.net is returing quotes around each parameter, so strip them out:
				for(int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
				{
					statusArray[i] = statusArray[i].Trim('\"');
				}
				
				String sql = String.Empty;
				String replyCode = statusArray[0];

				DB.ExecuteSQL("update orders set CaptureTXResult=" + DB.SQuote(rawResponseString)  + " where OrderNumber=" + OrderNumber.ToString());
				if(replyCode == "1")
				{
					result = "OK";
				}
				else
				{
					result = statusArray[3];
				}
			}
			catch(Exception ex)
			{
				HttpContext.Current.Session["GatewayMsg"] = Common.Session("GatewayMsg") + ex.Message;
				result = "NO RESPONSE FROM GATEWAY!";
			}
			//}
			return result;
		}

		static public String VoidOrder(int OrderNumber)
		{
			HttpContext.Current.Session["GatewayMsg"] = String.Empty;
			String result = "OK";

			DB.ExecuteSQL("update orders set VoidTXCommand=NULL, VoidTXResult=NULL where OrderNumber=" + OrderNumber.ToString());
			bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
			String TransID = String.Empty;
			IDataReader rs = DB.GetRS("select * from orders  " + DB.GetNoLock() + " where OrderNumber=" + OrderNumber.ToString());
			if(rs.Read())
			{
				TransID = DB.RSField(rs,"AuthorizationPNREF");
			}
			rs.Close();

			String X_Login = Common.AppConfig("PAYPALPRO_X_LOGIN");
			if(X_Login.Trim().ToUpper() == "REGISTRY")
			{
				WindowsRegistry reg = new WindowsRegistry(Common.AppConfig("EncryptKey.RegistryLocation"));
				X_Login = reg.Read("PAYPALPRO_X_LOGIN");
				reg = null;
			}

			String X_TranKey = Common.AppConfig("PAYPALPRO_X_TRAN_KEY");
			if(X_TranKey.Trim().ToUpper() == "REGISTRY")
			{
				WindowsRegistry reg = new WindowsRegistry(Common.AppConfig("EncryptKey.RegistryLocation"));
				X_TranKey = reg.Read("PAYPALPRO_X_TRAN_KEY");
				reg = null;
			}


			ASCIIEncoding encoding = new ASCIIEncoding();
			StringBuilder transactionCommand = new StringBuilder(5000);
			transactionCommand.Append("x_type=VOID"); 
			transactionCommand.Append("&x_login=" + X_Login);
			transactionCommand.Append("&x_tran_key=" + X_TranKey);
			transactionCommand.Append("&x_version=" + Common.AppConfig("PAYPALPRO_X_VERSION"));
			transactionCommand.Append("&x_test_request=" + Common.IIF(useLiveTransactions , "FALSE" , "TRUE"));
			transactionCommand.Append("&x_method=" + Common.AppConfig("PAYPALPRO_X_METHOD"));
			transactionCommand.Append("&x_delim_data=" + Common.AppConfig("PAYPALPRO_X_DELIM_DATA"));
			transactionCommand.Append("&x_delim_char=" + Common.AppConfig("PAYPALPRO_X_DELIM_CHAR"));
			transactionCommand.Append("&x_encap_char=" + Common.AppConfig("PAYPALPRO_X_ENCAP_CHAR"));
			transactionCommand.Append("&x_relay_response=" + Common.AppConfig("PAYPALPRO_X_RELAY_RESPONSE"));
			transactionCommand.Append("&x_customer_ip=" + Common.ServerVariables("REMOTE_ADDR"));
			transactionCommand.Append("&x_trans_id=" + TransID);

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
					String AuthServer = Common.IIF(useLiveTransactions , Common.AppConfig("PAYPALPRO_LIVE_SERVER") , Common.AppConfig("PAYPALPRO_TEST_SERVER"));
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
					String rawResponseString = String.Empty;
					try
					{
						myResponse = myRequest.GetResponse();
						using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()) )
						{
							rawResponseString = sr.ReadToEnd();
							// Close and clean up the StreamReader
							sr.Close();
						}
						myResponse.Close();
					}
					catch(Exception ex)
					{
						HttpContext.Current.Session["GatewayMsg"] = Common.Session("GatewayMsg") + ex.Message;
						rawResponseString = "0|||Error Calling Authorize.Net Payment Gateway||||||||";
					}

					// rawResponseString now has gateway response
					String[] statusArray = rawResponseString.Split(Common.AppConfig("PAYPALPRO_X_DELIM_CHAR").ToCharArray());
					// this seems to be a new item where auth.net is returing quotes around each parameter, so strip them out:
					for(int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
					{
						statusArray[i] = statusArray[i].Trim('\"');
					}

					String sql = String.Empty;
					String replyCode = statusArray[0];

					DB.ExecuteSQL("update orders set VoidTXResult=" + DB.SQuote(rawResponseString)  + " where OrderNumber=" + OrderNumber.ToString());
					if(replyCode == "1")
					{
						result = "OK";
					}
					else
					{
						result = statusArray[3];
					}
				}
				catch(Exception ex)
				{
					HttpContext.Current.Session["GatewayMsg"] = Common.Session("GatewayMsg") + ex.Message;
					result = "NO RESPONSE FROM GATEWAY!";
				}

			}
			return result;
		}

		static public String RefundOrder(int OrderNumber)
		{
			HttpContext.Current.Session["GatewayMsg"] = String.Empty;
			String result = "OK";

			DB.ExecuteSQL("update orders set RefundTXCommand=NULL, RefundTXResult=NULL where OrderNumber=" + OrderNumber.ToString());
			bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
			String TransID = String.Empty;
			String CardNumber = String.Empty;
			Decimal OrderTotal = System.Decimal.Zero;
			IDataReader rs = DB.GetRS("select * from orders  " + DB.GetNoLock() + " where OrderNumber=" + OrderNumber.ToString());
			if(rs.Read())
			{
				TransID = DB.RSField(rs,"AuthorizationPNREF");
				CardNumber = Common.UnmungeString(DB.RSField(rs,"CardNumber"));
				OrderTotal = DB.RSFieldDecimal(rs,"OrderTotal");
			}
			rs.Close();

			String X_Login = Common.AppConfig("PAYPALPRO_X_LOGIN");
			if(X_Login.Trim().ToUpper() == "REGISTRY")
			{
				WindowsRegistry reg = new WindowsRegistry(Common.AppConfig("EncryptKey.RegistryLocation"));
				X_Login = reg.Read("PAYPALPRO_X_LOGIN");
				reg = null;
			}

			String X_TranKey = Common.AppConfig("PAYPALPRO_X_TRAN_KEY");
			if(X_TranKey.Trim().ToUpper() == "REGISTRY")
			{
				WindowsRegistry reg = new WindowsRegistry(Common.AppConfig("EncryptKey.RegistryLocation"));
				X_TranKey = reg.Read("PAYPALPRO_X_TRAN_KEY");
				reg = null;
			}



			ASCIIEncoding encoding = new ASCIIEncoding();
			StringBuilder transactionCommand = new StringBuilder(5000);
			transactionCommand.Append("x_type=CREDIT"); 
			transactionCommand.Append("&x_login=" + X_Login);
			transactionCommand.Append("&x_tran_key=" + X_TranKey);
			transactionCommand.Append("&x_version=" + Common.AppConfig("PAYPALPRO_X_VERSION"));
			transactionCommand.Append("&x_test_request=" + Common.IIF(useLiveTransactions , "FALSE" , "TRUE"));
			transactionCommand.Append("&x_method=" + Common.AppConfig("PAYPALPRO_X_METHOD"));
			transactionCommand.Append("&x_delim_data=" + Common.AppConfig("PAYPALPRO_X_DELIM_DATA"));
			transactionCommand.Append("&x_delim_char=" + Common.AppConfig("PAYPALPRO_X_DELIM_CHAR"));
			transactionCommand.Append("&x_encap_char=" + Common.AppConfig("PAYPALPRO_X_ENCAP_CHAR"));
			transactionCommand.Append("&x_relay_response=" + Common.AppConfig("PAYPALPRO_X_RELAY_RESPONSE"));
			transactionCommand.Append("&x_trans_id=" + TransID);
			transactionCommand.Append("&x_amount=" + Localization.CurrencyStringForGateway(OrderTotal));
			transactionCommand.Append("&x_customer_ip=" + Common.ServerVariables("REMOTE_ADDR"));
			transactionCommand.Append("&x_card_num=" + CardNumber);

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
					String AuthServer = Common.IIF(useLiveTransactions , Common.AppConfig("PAYPALPRO_LIVE_SERVER") , Common.AppConfig("PAYPALPRO_TEST_SERVER"));
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
					String rawResponseString = String.Empty;
					try
					{
						myResponse = myRequest.GetResponse();
						using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()) )
						{
							rawResponseString = sr.ReadToEnd();
							// Close and clean up the StreamReader
							sr.Close();
						}
						myResponse.Close();
					}
					catch(Exception ex)
					{
						HttpContext.Current.Session["GatewayMsg"] = Common.Session("GatewayMsg") + ex.Message;
						rawResponseString = "0|||Error Calling Authorize.Net Payment Gateway||||||||";
					}

					// rawResponseString now has gateway response
					String[] statusArray = rawResponseString.Split(Common.AppConfig("PAYPALPRO_X_DELIM_CHAR").ToCharArray());
					// this seems to be a new item where auth.net is returing quotes around each parameter, so strip them out:
					for(int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
					{
						statusArray[i] = statusArray[i].Trim('\"');
					}

					String sql = String.Empty;
					String replyCode = statusArray[0];

					DB.ExecuteSQL("update orders set RefundTXResult=" + DB.SQuote(rawResponseString)  + " where OrderNumber=" + OrderNumber.ToString());
					if(replyCode == "1")
					{
						result = "OK";
					}
					else
					{
						result = statusArray[3];
					}
				}
				catch(Exception ex)
				{
					HttpContext.Current.Session["GatewayMsg"] = Common.Session("GatewayMsg") + ex.Message;
					result = "NO RESPONSE FROM GATEWAY!";
				}
			}
			return result;
		}

		static public String ProcessCard(int OrderNumber, ShoppingCart cart, String CardName, String CardType, String CardNumber, String CardExtraCode, String CardExpirationMonth, String CardExpirationYear, Decimal OrderTotal, bool useLiveTransactions, Address Billing, Address Shipping, String CAVV, String ECI, String XID, out bool PaymentCleared, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut)
		{
			HttpContext.Current.Session["GatewayMsg"] = String.Empty;
			String result = "OK";

			String X_Login = Common.AppConfig("PAYPALPRO_X_LOGIN");
			if(X_Login.Trim().ToUpper() == "REGISTRY")
			{
				WindowsRegistry reg = new WindowsRegistry(Common.AppConfig("EncryptKey.RegistryLocation"));
				X_Login = reg.Read("PAYPALPRO_X_LOGIN");
				reg = null;
			}

			String X_TranKey = Common.AppConfig("PAYPALPRO_X_TRAN_KEY");
			if(X_TranKey.Trim().ToUpper() == "REGISTRY")
			{
				WindowsRegistry reg = new WindowsRegistry(Common.AppConfig("EncryptKey.RegistryLocation"));
				X_TranKey = reg.Read("PAYPALPRO_X_TRAN_KEY");
				reg = null;
			}

			ASCIIEncoding encoding = new ASCIIEncoding();
			StringBuilder transactionCommand = new StringBuilder(5000);

			transactionCommand.Append("x_type=" + Common.IIF(Common.TransactionMode() == "AUTH" , "AUTH_ONLY" , "AUTH_CAPTURE")); //Common.AppConfig("PAYPALPRO_X_TYPE"));

			transactionCommand.Append("&x_login=" + X_Login);
			transactionCommand.Append("&x_tran_key=" + X_TranKey);
			transactionCommand.Append("&x_version=" + Common.AppConfig("PAYPALPRO_X_VERSION"));
			transactionCommand.Append("&x_test_request=" + Common.IIF(useLiveTransactions , "FALSE" , "TRUE"));
			transactionCommand.Append("&x_merchant_email=" + Common.AppConfig("PAYPALPRO_X_EMAIL"));
			transactionCommand.Append("&x_description=" + Common.AppConfig("StoreName") + " Order " + OrderNumber.ToString());

			transactionCommand.Append("&x_method=" + Common.AppConfig("PAYPALPRO_X_METHOD"));

			transactionCommand.Append("&x_delim_data=" + Common.AppConfig("PAYPALPRO_X_DELIM_DATA"));
			transactionCommand.Append("&x_delim_char=" + Common.AppConfig("PAYPALPRO_X_DELIM_CHAR"));
			transactionCommand.Append("&x_encap_char=" + Common.AppConfig("PAYPALPRO_X_ENCAP_CHAR"));
			transactionCommand.Append("&x_relay_response=" + Common.AppConfig("PAYPALPRO_X_RELAY_RESPONSE"));
			
			transactionCommand.Append("&x_email_customer=" + Common.AppConfig("PAYPALPRO_X_EMAIL_CUSTOMER"));
			transactionCommand.Append("&x_recurring_billing=" + Common.AppConfig("PAYPALPRO_X_RECURRING_BILLING"));

			transactionCommand.Append("&x_amount=" + Localization.CurrencyStringForGateway(OrderTotal));
			transactionCommand.Append("&x_card_num=" + CardNumber);
			transactionCommand.Append("&x_card_code=" + CardExtraCode);
			transactionCommand.Append("&x_exp_date=" + CardExpirationMonth.PadLeft(2,'0') + "/" + CardExpirationYear);
			transactionCommand.Append("&x_phone=" + cart._billingAddress.Phone);
			transactionCommand.Append("&x_fax=");
			transactionCommand.Append("&x_customer_tax_id=");
			transactionCommand.Append("&x_cust_id=" + cart._thisCustomer._customerID.ToString());
			transactionCommand.Append("&x_invoice_num=" + OrderNumber.ToString());
			transactionCommand.Append("&x_email=" + Billing.Email);
			transactionCommand.Append("&x_customer_ip=" + Common.ServerVariables("REMOTE_ADDR"));

			transactionCommand.Append("&x_first_name=" + Billing.FirstName);
			transactionCommand.Append("&x_last_name=" + Billing.LastName);
			transactionCommand.Append("&x_company=" + Billing.Company);
			transactionCommand.Append("&x_address=" + Billing.Address1);
			transactionCommand.Append("&x_city=" + Billing.City);
			transactionCommand.Append("&x_state=" + Billing.State);
			transactionCommand.Append("&x_zip=" + Billing.Zip);
			transactionCommand.Append("&x_country=" + Billing.Country);

			transactionCommand.Append("&x_ship_to_first_name=" + Shipping.FirstName);
			transactionCommand.Append("&x_ship_to_last_name=" + Shipping.LastName);
			transactionCommand.Append("&x_ship_to_company=" + Shipping.Company);
			transactionCommand.Append("&x_ship_to_address=" + Shipping.Address1);
			transactionCommand.Append("&x_ship_to_city=" + Shipping.City);
			transactionCommand.Append("&x_ship_to_state=" + Shipping.State);
			transactionCommand.Append("&x_ship_to_zip=" + Shipping.Zip);
			transactionCommand.Append("&x_ship_to_country=" + Shipping.Country);


			transactionCommand.Append("&x_customer_ip=" + Common.ServerVariables("REMOTE_ADDR"));

			if(CAVV.Length != 0 || ECI.Length != 0)
			{
				transactionCommand.Append("&x_authentication_indicator=" + ECI);
				transactionCommand.Append("&x_cardholder_authentication_value=" + CAVV);
			}

			HttpContext.Current.Session["GatewayMsg"] = Common.Session("GatewayMsg") + ", TXCmd=" + transactionCommand.ToString();
				
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
				String AuthServer = Common.IIF(useLiveTransactions , Common.AppConfig("PAYPALPRO_LIVE_SERVER") , Common.AppConfig("PAYPALPRO_TEST_SERVER"));
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
				String rawResponseString = String.Empty;
				try
				{
					myResponse = myRequest.GetResponse();
					using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()) )
					{
						rawResponseString = sr.ReadToEnd();
						// Close and clean up the StreamReader
						sr.Close();
					}
					myResponse.Close();
				}
				catch(Exception ex)
				{
					HttpContext.Current.Session["GatewayMsg"] = Common.Session("GatewayMsg") + ex.Message;
					rawResponseString = "0|||Error Calling Authorize.NetPayment Gateway||||||||";
				}

				HttpContext.Current.Session["GatewayMsg"] = Common.Session("GatewayMsg") + ", TXResponse=" + rawResponseString;
				
				// rawResponseString now has gateway response
				String[] statusArray = rawResponseString.Split(Common.AppConfig("PAYPALPRO_X_DELIM_CHAR").ToCharArray());
				// this seems to be a new item where auth.net is returing quotes around each parameter, so strip them out:
				for(int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
				{
					statusArray[i] = statusArray[i].Trim('\"');
				}

				String sql = String.Empty;
				String replyCode = statusArray[0];
				String responseCode = statusArray[2];
				String approvalCode = statusArray[4];
				String authResponse = statusArray[3];
				String TransID = statusArray[6];

				PaymentCleared = false;
				AuthorizationCode = statusArray[4];
				AuthorizationResult = rawResponseString;
				AuthorizationTransID = statusArray[6];
				AVSResult = statusArray[5];
				TransactionCommandOut = transactionCommand.ToString();

				if(replyCode == "1")
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
					else
					{
						result = result.Replace("account","card");
						result = result.Replace("Account","Card");
						result = result.Replace("ACCOUNT","CARD");
					}
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
					sql = "insert into FailedTransaction(CustomerID,OrderDate,PaymentGateway,TransactionCommand,TransactionResult) values(" + cart._thisCustomer._customerID.ToString() + "," + DB.DateQuote(Localization.ToNativeShortDateString(System.DateTime.Now)) + "," + DB.SQuote("PayPalPro") + "," + DB.SQuote(transactionCommand.ToString().Replace(CardNumber + "",CC)) + "," + DB.SQuote(rawResponseString) + ")";
					DB.ExecuteSQL(sql);
				}
			}
			catch(Exception ex)
			{
				HttpContext.Current.Session["GatewayMsg"] = Common.Session("GatewayMsg") + ex.Message;
				result = "Error calling Authorize.net gateway. Please retry your order in a few minutes or select another checkout payment option.";
			}
			return result;
		}


		static public String ProcessECheck(int OrderNumber, ShoppingCart cart, String eCheckBankABACode,String eCheckBankAccountNumber,String eCheckBankAccountType,String eCheckBankName,String eCheckBankAccountName, Decimal OrderTotal, Address Billing, Address Shipping, out bool PaymentCleared, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut)
		{
			HttpContext.Current.Session["GatewayMsg"] = String.Empty;
			String result = "OK";
			bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");

			ASCIIEncoding encoding = new ASCIIEncoding();
			StringBuilder transactionCommand = new StringBuilder(5000);


			transactionCommand.Append("x_method=ECHECK");
			transactionCommand.Append("&x_type=AUTH_CAPTURE"); // eCHECKS only support AUTH_CAPTURE
			transactionCommand.Append("&x_echeck_type=WEB");

			transactionCommand.Append("&x_login=" + Common.AppConfig("PAYPALPRO_X_LOGIN"));
			transactionCommand.Append("&x_tran_key=" + Common.AppConfig("PAYPALPRO_X_TRAN_KEY"));
			transactionCommand.Append("&x_version=" + Common.AppConfig("PAYPALPRO_X_VERSION"));
			transactionCommand.Append("&x_merchant_email=" + Common.AppConfig("MailMe_ToAddress"));
			transactionCommand.Append("&x_description=" + Common.AppConfig("StoreName") + " Order " + OrderNumber.ToString());

			transactionCommand.Append("&x_delim_data=" + Common.AppConfig("PAYPALPRO_X_DELIM_DATA"));
			transactionCommand.Append("&x_delim_char=" + Common.AppConfig("PAYPALPRO_X_DELIM_CHAR"));
			transactionCommand.Append("&x_encap_char=" + Common.AppConfig("PAYPALPRO_X_ENCAP_CHAR"));
			transactionCommand.Append("&x_relay_response=" + Common.AppConfig("PAYPALPRO_X_RELAY_RESPONSE"));
			
			transactionCommand.Append("&x_email_customer=" + Common.AppConfig("PAYPALPRO_X_EMAIL_CUSTOMER"));
			transactionCommand.Append("&x_recurring_billing=NO"); // for echecks

			transactionCommand.Append("&x_amount=" + Localization.CurrencyStringForGateway(OrderTotal));
			transactionCommand.Append("&x_bank_aba_code=" + eCheckBankABACode);
			transactionCommand.Append("&x_bank_acct_num=" + eCheckBankAccountNumber);
			transactionCommand.Append("&x_bank_acct_type=" + eCheckBankAccountType);
			transactionCommand.Append("&x_bank_name=" + eCheckBankName);
			transactionCommand.Append("&x_bank_acct_name=" + eCheckBankAccountName);
			transactionCommand.Append("&x_customer_organization_type=" + Common.IIF(eCheckBankAccountType == "BUSINESS CHECKING", "B", "I"));

			transactionCommand.Append("&x_phone=" + cart._billingAddress.Phone);
			transactionCommand.Append("&x_fax=");
			transactionCommand.Append("&x_customer_tax_id=");
			transactionCommand.Append("&x_cust_id=" + cart._thisCustomer._customerID.ToString());
			transactionCommand.Append("&x_invoice_num=" + OrderNumber.ToString());
			transactionCommand.Append("&x_email=" + Billing.Email);
			transactionCommand.Append("&x_customer_ip=" + Common.ServerVariables("REMOTE_ADDR"));

			transactionCommand.Append("&x_first_name=" + Billing.FirstName);
			transactionCommand.Append("&x_last_name=" + Billing.LastName);
			transactionCommand.Append("&x_company=" + Billing.Company);
			transactionCommand.Append("&x_address=" + Billing.Address1);
			transactionCommand.Append("&x_city=" + Billing.City);
			transactionCommand.Append("&x_state=" + Billing.State);
			transactionCommand.Append("&x_zip=" + Billing.Zip);
			transactionCommand.Append("&x_country=" + Billing.Country);


			transactionCommand.Append("&x_ship_to_first_name=" + Shipping.FirstName);
			transactionCommand.Append("&x_ship_to_last_name=" + Shipping.LastName);
			transactionCommand.Append("&x_ship_to_company=" + Shipping.Company);
			transactionCommand.Append("&x_ship_to_address=" + Shipping.Address1);
			transactionCommand.Append("&x_ship_to_city=" + Shipping.City);
			transactionCommand.Append("&x_ship_to_state=" + Shipping.State);
			transactionCommand.Append("&x_ship_to_zip=" + Shipping.Zip);
			transactionCommand.Append("&x_ship_to_country=" + Shipping.Country);


			transactionCommand.Append("&x_customer_ip=" + Common.ServerVariables("REMOTE_ADDR"));

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
				String AuthServer = Common.IIF(useLiveTransactions , Common.AppConfig("PAYPALPRO_LIVE_SERVER") , Common.AppConfig("PAYPALPRO_TEST_SERVER"));
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
				String rawResponseString = String.Empty;
				try
				{
					myResponse = myRequest.GetResponse();
					using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()) )
					{
						rawResponseString = sr.ReadToEnd();
						// Close and clean up the StreamReader
						sr.Close();
					}
					myResponse.Close();
				}
				catch(Exception ex)
				{
					HttpContext.Current.Session["GatewayMsg"] = Common.Session("GatewayMsg") + ex.Message;
					rawResponseString = "0|||Error Calling Authorize.NetPayment Gateway||||||||";
				}

				// rawResponseString now has gateway response
				String[] statusArray = rawResponseString.Split(Common.AppConfig("PAYPALPRO_X_DELIM_CHAR").ToCharArray());
				// this seems to be a new item where auth.net is returing quotes around each parameter, so strip them out:
				for(int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
				{
					statusArray[i] = statusArray[i].Trim('\"');
				}

				String sql = String.Empty;
				String replyCode = statusArray[0];
				String responseCode = statusArray[2];
				String approvalCode = statusArray[4];
				String authResponse = statusArray[3];
				String TransID = statusArray[6];

				PaymentCleared = false;
				AuthorizationCode = statusArray[4];
				AuthorizationResult = rawResponseString;
				AuthorizationTransID = statusArray[6];
				AVSResult = statusArray[5];
				TransactionCommandOut = transactionCommand.ToString();

				if(replyCode == "1")
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
					// record failed TX:
					sql = "insert into FailedTransaction(CustomerID,OrderDate,PaymentGateway,TransactionCommand,TransactionResult) values(" + cart._thisCustomer._customerID.ToString() + "," + DB.DateQuote(Localization.ToNativeShortDateString(System.DateTime.Now)) + "," + DB.SQuote("PayPalPro") + "," + DB.SQuote(transactionCommand.ToString()) + "," + DB.SQuote(rawResponseString) + ")";
					DB.ExecuteSQL(sql);
				}
			}
			catch(Exception ex)
			{
				HttpContext.Current.Session["GatewayMsg"] = Common.Session("GatewayMsg") + ex.Message;
				result = "Error calling Authorize.net gateway. Please retry your order in a few minutes or select another checkout payment option.";
			}
			return result;
		}

	
	}
}
