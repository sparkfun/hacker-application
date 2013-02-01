//#define VERISIGN
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
	/// Summary description for Verisign.
	/// </summary>
	public class Verisign
	{
		public Verisign()	{}

		static public String CaptureOrder(int OrderNumber)
		{
			String result = "VERISIGN COM COMPONENTS NOT INSTALLED ON SERVER OR STOREFRONT NOT COMPILED WITH VERISIGN CODE TURNED ON";

#if VERISIGN

			HttpContext.Current.Session["GatewayMsg"] = String.Empty;
			result = "OK";
			PFProCOMLib.PNComClass vsnGate = new PFProCOMLib.PNComClass();

			DB.ExecuteSQL("update orders set CaptureTXCommand=NULL, CaptureTXResult=NULL where OrderNumber=" + OrderNumber.ToString());
			bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
			String TransID = String.Empty;
			IDataReader rs = DB.GetRS("select * from orders  " + DB.GetNoLock() + " where OrderNumber=" + OrderNumber.ToString());
			if(rs.Read())
			{
				TransID = DB.RSField(rs,"AuthorizationPNREF");
			}
			rs.Close();			
			
			StringBuilder transactionCommand = new StringBuilder(5000);

			transactionCommand.Append("TRXTYPE=D&TENDER=C&COMMENT1=" + Common.AppConfig("StoreName") + " Capture");
			transactionCommand.Append("&PWD=" + Common.AppConfig("Verisign_PWD"));
			transactionCommand.Append("&USER=" + Common.AppConfig("Verisign_USER"));
			transactionCommand.Append("&VENDOR=" + Common.AppConfig("Verisign_VENDOR"));
			transactionCommand.Append("&PARTNER=" + Common.AppConfig("Verisign_PARTNER"));
			transactionCommand.Append("&ORIGID=" + TransID);

			DB.ExecuteSQL("update orders set CaptureTXCommand=" + DB.SQuote(transactionCommand.ToString()) + " where OrderNumber=" + OrderNumber.ToString());

			try
			{
				int Ctx1;
				if(Common.AppConfigBool("UseLiveTransactions"))
				{
					Ctx1 = vsnGate.CreateContext("payflow.verisign.com",443,30,String.Empty,0,String.Empty,String.Empty);
				}
				else
				{
					Ctx1 = vsnGate.CreateContext("test-payflow.verisign.com",443,30,String.Empty,0,String.Empty,String.Empty);
				}
				String rawResponseString = vsnGate.SubmitTransaction(Ctx1, transactionCommand.ToString(), transactionCommand.Length);
				vsnGate.DestroyContext (Ctx1);

				String[] statusArray = rawResponseString.Split('&');
				String replyCode = String.Empty;
				String replyMsg = String.Empty;
				String PNREF = String.Empty;
				for(int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
				{
					String[] lasKeyPair = statusArray[i].Split('=');
					switch(lasKeyPair[0].ToLower())
					{
						case "result":
							replyCode = lasKeyPair[1];
							break;
						case "pnref":
							PNREF = lasKeyPair[1];
							break;
						case "respmsg":
							replyMsg = lasKeyPair[1];
							break;
					}
				}

				DB.ExecuteSQL("update orders set CaptureTXResult=" + DB.SQuote(rawResponseString)  + " where OrderNumber=" + OrderNumber.ToString());
				if(replyCode == "0")
				{
					result = "OK";
				}
				else
				{
					result = replyMsg;
				}
			}
			catch(Exception ex)
			{
				HttpContext.Current.Session["GatewayMsg"] = Common.Session("GatewayMsg") + ex.Message;
				result = "NO RESPONSE FROM GATEWAY!";
			}
			//}
#endif
			return result;
		}

		static public String VoidOrder(int OrderNumber)
		{
			String result = "VERISIGN COM COMPONENTS NOT INSTALLED ON SERVER OR STOREFRONT NOT COMPILED WITH VERISIGN CODE TURNED ON";
#if VERISIGN

			HttpContext.Current.Session["GatewayMsg"] = String.Empty;
			result = "OK";
			PFProCOMLib.PNComClass vsnGate = new PFProCOMLib.PNComClass();

			DB.ExecuteSQL("update orders set VoidTXCommand=NULL, VoidTXResult=NULL where OrderNumber=" + OrderNumber.ToString());
			bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
			String TransID = String.Empty;
			IDataReader rs = DB.GetRS("select * from orders  " + DB.GetNoLock() + " where OrderNumber=" + OrderNumber.ToString());
			if(rs.Read())
			{
				TransID = DB.RSField(rs,"AuthorizationPNREF");
			}
			rs.Close();

			StringBuilder transactionCommand = new StringBuilder(5000);

			transactionCommand.Append("TRXTYPE=V&TENDER=C&COMMENT1=" + Common.AppConfig("StoreName") + " Void");
			transactionCommand.Append("&PWD=" + Common.AppConfig("Verisign_PWD"));
			transactionCommand.Append("&USER=" + Common.AppConfig("Verisign_USER"));
			transactionCommand.Append("&VENDOR=" + Common.AppConfig("Verisign_VENDOR"));
			transactionCommand.Append("&PARTNER=" + Common.AppConfig("Verisign_PARTNER"));
			transactionCommand.Append("&ORIGID=" + TransID);

			DB.ExecuteSQL("update orders set VoidTXCommand=" + DB.SQuote(transactionCommand.ToString()) + " where OrderNumber=" + OrderNumber.ToString());

			if(TransID.Length == 0 || TransID == "0")
			{
				result = "Invalid or Empty Transaction ID";
			}
			else
			{
				try
				{
					int Ctx1;
					if(Common.AppConfigBool("UseLiveTransactions"))
					{
						Ctx1 = vsnGate.CreateContext("payflow.verisign.com",443,30,String.Empty,0,String.Empty,String.Empty);
					}
					else
					{
						Ctx1 = vsnGate.CreateContext("test-payflow.verisign.com",443,30,String.Empty,0,String.Empty,String.Empty);
					}
					String rawResponseString = vsnGate.SubmitTransaction(Ctx1, transactionCommand.ToString(), transactionCommand.Length);
					vsnGate.DestroyContext (Ctx1);

					String[] statusArray = rawResponseString.Split('&');
					String replyCode = String.Empty;
					String replyMsg = String.Empty;
					String PNREF = String.Empty;
					for(int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
					{
						String[] lasKeyPair = statusArray[i].Split('=');
						switch(lasKeyPair[0].ToLower())
						{
							case "result":
								replyCode = lasKeyPair[1];
								break;
							case "pnref":
								PNREF = lasKeyPair[1];
								break;
							case "respmsg":
								replyMsg = lasKeyPair[1];
								break;
						}
					}
					
					DB.ExecuteSQL("update orders set VoidTXResult=" + DB.SQuote(rawResponseString)  + " where OrderNumber=" + OrderNumber.ToString());
					if(replyCode == "0")
					{
						result = "OK";
					}
					else
					{
						result = replyMsg;
					}
				}
				catch(Exception ex)
				{
					HttpContext.Current.Session["GatewayMsg"] = Common.Session("GatewayMsg") + ex.Message;
					result = "NO RESPONSE FROM GATEWAY!";
				}

			}
#endif
			return result;
		}

		static public String RefundOrder(int OrderNumber)
		{
			String result = "VERISIGN COM COMPONENTS NOT INSTALLED ON SERVER OR STOREFRONT NOT COMPILED WITH VERISIGN CODE TURNED ON";
#if VERISIGN

			HttpContext.Current.Session["GatewayMsg"] = String.Empty;
			result = "OK";
			PFProCOMLib.PNComClass vsnGate = new PFProCOMLib.PNComClass();

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

			StringBuilder transactionCommand = new StringBuilder(5000);

			transactionCommand.Append("TRXTYPE=C&TENDER=C&COMMENT1=" + Common.AppConfig("StoreName") + " Refund");
			transactionCommand.Append("&PWD=" + Common.AppConfig("Verisign_PWD"));
			transactionCommand.Append("&USER=" + Common.AppConfig("Verisign_USER"));
			transactionCommand.Append("&VENDOR=" + Common.AppConfig("Verisign_VENDOR"));
			transactionCommand.Append("&PARTNER=" + Common.AppConfig("Verisign_PARTNER"));
			transactionCommand.Append("&ORIGID=" + TransID);

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
					int Ctx1;
					if(Common.AppConfigBool("UseLiveTransactions"))
					{
						Ctx1 = vsnGate.CreateContext("payflow.verisign.com",443,30,String.Empty,0,String.Empty,String.Empty);
					}
					else
					{
						Ctx1 = vsnGate.CreateContext("test-payflow.verisign.com",443,30,String.Empty,0,String.Empty,String.Empty);
					}
					String rawResponseString = vsnGate.SubmitTransaction(Ctx1, transactionCommand.ToString(), transactionCommand.Length);
					vsnGate.DestroyContext (Ctx1);

					String[] statusArray = rawResponseString.Split('&');
					String replyCode = String.Empty;
					String replyMsg = String.Empty;
					String PNREF = String.Empty;
					for(int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
					{
						String[] lasKeyPair = statusArray[i].Split('=');
						switch(lasKeyPair[0].ToLower())
						{
							case "result":
								replyCode = lasKeyPair[1];
								break;
							case "pnref":
								PNREF = lasKeyPair[1];
								break;
							case "respmsg":
								replyMsg = lasKeyPair[1];
								break;
						}
					}

					DB.ExecuteSQL("update orders set RefundTXResult=" + DB.SQuote(rawResponseString)  + " where OrderNumber=" + OrderNumber.ToString());
					if(replyCode == "0")
					{
						result = "OK";
					}
					else
					{
						result = replyMsg;
					}
				}
				catch(Exception ex)
				{
					HttpContext.Current.Session["GatewayMsg"] = Common.Session("GatewayMsg") + ex.Message;
					result = "NO RESPONSE FROM GATEWAY!";
				}
			}
#endif
			return result;
		}
		
		// processes card in real time:
		static public String ProcessCard(int OrderNumber, ShoppingCart cart,String CardName, String CardType, String CardNumber, String CardExtraCode, String CardExpirationMonth, String CardExpirationYear, Decimal OrderTotal, bool useLiveTransactions, Address Billing,  Address Shipping, String CAVV, String ECI, String XID, out bool PaymentCleared, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut)
		{
			String result = "VERISIGN COM COMPONENTS NOT INSTALLED ON SERVER OR STOREFRONT NOT COMPILED WITH VERISIGN CODE TURNED ON";
			PaymentCleared = false;
			AuthorizationCode = String.Empty;
			AuthorizationResult = String.Empty;
			AuthorizationTransID = String.Empty;
			AVSResult = String.Empty;
			TransactionCommandOut = String.Empty;
#if VERISIGN
			String sql;
			PFProCOMLib.PNComClass vsnGate = new PFProCOMLib.PNComClass();

			StringBuilder transactionCommand = new StringBuilder(5000);
			String rawResponseString;
			String replyCode = String.Empty;
			String responseCode = String.Empty;
			String authResponse = String.Empty;
			String approvalCode = String.Empty;
			String orderTotalString;

			if(!useLiveTransactions)
			{
				OrderTotal = 1.0M;
			}
			orderTotalString = Localization.CurrencyStringForGateway(OrderTotal);

			transactionCommand.Append("TRXTYPE=" + Common.IIF(Common.TransactionMode() == "AUTH" , "A", "S") + "&TENDER=C&ZIP=" + cart._shippingAddress.Zip + "&COMMENT1=" + "Order " + OrderNumber + "&COMMENT2=" + "CustomerID " + cart._thisCustomer.CustomerID);
			transactionCommand.Append("&PWD=" + Common.AppConfig("Verisign_PWD"));
			transactionCommand.Append("&USER=" + Common.AppConfig("Verisign_USER"));
			transactionCommand.Append("&VENDOR=" + Common.AppConfig("Verisign_VENDOR"));
			transactionCommand.Append("&PARTNER=" + Common.AppConfig("Verisign_PARTNER"));

			//set the amount 
			transactionCommand.Append("&AMT=" + orderTotalString);
			
			transactionCommand.Append("&ACCT=" + CardNumber);
			//set the expiration date form the HTML form
			transactionCommand.Append("&EXPDATE=" + CardExpirationMonth.PadLeft(2,'0') + CardExpirationYear.Substring(2,2));

			//set the CSC code:
			if (CardExtraCode.Trim().Length != 0)
			{
				transactionCommand.Append("&CSC2MATCH=" + CardExtraCode);
				transactionCommand.Append("&CVV2=" + CardExtraCode);
			}
			
			transactionCommand.Append("&SHIPTOSTREET=" + Shipping.Address1);
			transactionCommand.Append("&SHIPTOCITY=" + Shipping.City);
			transactionCommand.Append("&SHIPTOSTATE=" + Shipping.State );
			transactionCommand.Append("&SHIPTOZIP=" + Shipping.Zip);
			transactionCommand.Append("&SHIPTOCOUNTRY=" + Shipping.Country); //Verisign documentation says it's SHIPTOCOUNTRY but support says it's COUNTRYCODE which is the one that worked for me
			transactionCommand.Append("&COUNTRYCODE=" + Shipping.Country); //Verisign documentation says it's SHIPTOCOUNTRY but support says it's COUNTRYCODE which is the one that worked for me
			transactionCommand.Append("&STREET=" + Billing.Address1);
			transactionCommand.Append("&CITY=" + Billing.City);
			transactionCommand.Append("&STATE=" + Billing.State);
			transactionCommand.Append("&ZIP=" + Billing.Zip);
			transactionCommand.Append("&COUNTRY=" + Billing.Country);
			transactionCommand.Append("&CUSTIP=" + cart._thisCustomer.LastIPAddress);
			transactionCommand.Append("&EMAIL=" + Billing.Email);

			if(CAVV.Length != 0)
			{
				transactionCommand.Append("&CAVV[" + CAVV.Length.ToString() + "]=" + CAVV);
				transactionCommand.Append("&ECI=" + ECI);
				transactionCommand.Append("&XID=" + XID);
			}

			int Ctx1;
			if(Common.AppConfigBool("UseLiveTransactions"))
			{
				Ctx1 = vsnGate.CreateContext("payflow.verisign.com",443,30,String.Empty,0,String.Empty,String.Empty);
			}
			else
			{
				Ctx1 = vsnGate.CreateContext("test-payflow.verisign.com",443,30,String.Empty,0,String.Empty,String.Empty);
			}
			String curString = vsnGate.SubmitTransaction(Ctx1, transactionCommand.ToString(), transactionCommand.Length);
			rawResponseString = curString;
			vsnGate.DestroyContext (Ctx1);

			bool AVSOk = true;
			String AVSAddr = String.Empty;
			String AVSZip = String.Empty;

			String[] statusArray = curString.Split('&');
			for(int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
			{
				String[] lasKeyPair = statusArray[i].Split('=');
				switch(lasKeyPair[0].ToLower())
				{
					case "result":
						replyCode = lasKeyPair[1];
						break;
					case "pnref":
						responseCode = lasKeyPair[1];
						break;
					case "respmsg":
						authResponse = lasKeyPair[1];
						break;
					case "authcode":
						approvalCode = lasKeyPair[1];
						break;
					case "avsaddr":
						AVSAddr = lasKeyPair[1];
						break;
					case "avszip":
						AVSZip = lasKeyPair[1];
						break;
				}
			}

			// ok, how to handle this? Bank doesn't decline based on AVS info, so we can't either...as the card has already been charged!

			//			if(Common.AppConfigBool("Verisign_Verify_Addresses"))
			//			{
			//				AVSOk = false;
			//				if(AVSAddr == "Y" || AVSZip == "Y")
			//				{
			//					AVSOk = true;
			//				}
			//			}
			
			PaymentCleared = false;
			AuthorizationCode = approvalCode;
			AuthorizationResult = rawResponseString;
			AuthorizationTransID = responseCode;
			AVSResult =  String.Empty; // TBD
			TransactionCommandOut = transactionCommand.ToString();
			
			if(replyCode == "0" && AVSOk)
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
				String CC = String.Empty;
				if(Common.AppConfigBool("StoreCCInDB"))
				{
					CC = Common.MungeString(CardNumber);
				}
				else
				{
					CC = "XXXXXXXXXXXX";
				}
				sql = "insert into FailedTransaction(CustomerID,OrderDate,PaymentGateway,TransactionCommand,TransactionResult) values(" + cart._thisCustomer._customerID.ToString() + "," + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + "," + DB.SQuote("VERISIGN") + "," + DB.SQuote(transactionCommand.ToString().Replace(CardNumber,CC)) + "," + DB.SQuote(rawResponseString) + ")";
				DB.ExecuteSQL(sql);
			}
#endif
			return result;
		}		
	}
}
