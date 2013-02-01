//#define YOURPAY
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

#if YOURPAY
using LinkPointTransaction;
#endif

namespace AspDotNetStorefrontGateways
{


	/// <summary>
	/// Summary description for YourPay.
	/// </summary>
	public class YourPay
	{
    
		static public String CaptureOrder(int OrderNumber)
		{
			String result = "YOURPAY COM COMPONENTS NOT INSTALLED ON SERVER OR STOREFRONT NOT COMPILED WITH YOURPAY CODE TURNED ON";

#if YOURPAY

			DB.ExecuteSQL("update orders set CaptureTXCommand=NULL, CaptureTXResult=NULL where OrderNumber=" + OrderNumber.ToString());
			bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
			String TransID = String.Empty;
			String CardNumber = String.Empty;
			String CardExpirationMonth = String.Empty;
			String CardExpirationYear = String.Empty;
			Decimal OrderTotal = System.Decimal.Zero;
			IDataReader rs = DB.GetRS("select * from orders where OrderNumber=" + OrderNumber.ToString());
			if(rs.Read())
			{
				TransID = DB.RSField(rs,"AuthorizationPNREF");
				CardNumber = Common.UnmungeString(DB.RSField(rs,"CardNumber"));;
				CardExpirationMonth = DB.RSField(rs,"CardExpirationMonth");
				CardExpirationYear = DB.RSField(rs,"CardExpirationYear");
				OrderTotal = DB.RSFieldDecimal(rs,"OrderTotal");
			}
			if(CardExpirationYear.Length > 2)
			{
				CardExpirationYear = CardExpirationYear.Substring(2,2);
			}
			rs.Close();

			if(TransID.Length == 0 || TransID == "0")
			{
				result = "Invalid or Empty Transaction ID";
			}
			else
			{
				LPOrderPart order = LPOrderFactory.createOrderPart("order");
				LPOrderPart op = LPOrderFactory.createOrderPart();
				
				op.put("ordertype","POSTAUTH");
				order.addPart("orderoptions", op ); 
	
				op.clear();
				op.put("configfile",Common.AppConfig("YOURPAY_CONFIGFILE"));
				order.addPart("merchantinfo", op );

				// Build creditcard
				op.clear();
				op.put("cardnumber",CardNumber);
				op.put("cardexpmonth",CardExpirationMonth);
				op.put("cardexpyear",CardExpirationYear);
				// add creditcard to order
				order.addPart("creditcard", op );

				// Build payment
				op.clear();
				op.put("chargetotal",Localization.CurrencyStringForGateway(OrderTotal));
				// add payment to order
				order.addPart("payment", op );

				// Add oid
				op.clear();
				op.put("oid",TransID);
				// add transactiondetails to order
				order.addPart("transactiondetails", op );

				LinkPointTxn LPTxn = new LinkPointTxn();

				// get outgoing XML from the 'order' object
				string outXml = order.toXML();

				String KeyFile = HttpContext.Current.Server.MapPath(Common.IIF(Common.IsAdminSite, "../", "") + Common.AppConfig("YOURPAY_KEYFILE"));

				DB.ExecuteSQL("update orders set CaptureTXCommand=" + DB.SQuote(outXml) + " where OrderNumber=" + OrderNumber.ToString());

				// Call LPTxn
				String Linkpoint_Host = Common.IIF(Common.AppConfigBool("UseLiveTransactions") , Common.AppConfig("YOURPAY_LIVE_SERVER") , Common.AppConfig("YOURPAY_TEST_SERVER"));
				string rawResponseString = LPTxn.send(KeyFile,Linkpoint_Host,Common.AppConfigUSInt("YOURPAY_PORT"), outXml);
				DB.ExecuteSQL("update orders set CaptureTXResult=" + DB.SQuote(rawResponseString)  + " where OrderNumber=" + OrderNumber.ToString());
			
				// response holders
				String R_Approved = ParseTag("r_approved",rawResponseString);
				String R_Code = ParseTag("r_code",rawResponseString);
				String R_Error = ParseTag("r_error",rawResponseString);
				String R_Message = ParseTag("r_message",rawResponseString);

				if(R_Approved == "APPROVED")
				{
					result = "OK";
				}
				else
				{
					result = R_Error;
				}
			}
#endif
			return result;
		}

		static public String VoidOrder(int OrderNumber)
		{
			String result = "YOURPAY COM COMPONENTS NOT INSTALLED ON SERVER OR STOREFRONT NOT COMPILED WITH YOURPAY CODE TURNED ON";
#if YOURPAY

			DB.ExecuteSQL("update orders set VoidTXCommand=NULL, VoidTXResult=NULL where OrderNumber=" + OrderNumber.ToString());
			bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
			String TransID = String.Empty;
			String CardNumber = String.Empty;
			String CardExpirationMonth = String.Empty;
			String CardExpirationYear = String.Empty;
			Decimal OrderTotal = System.Decimal.Zero;
			IDataReader rs = DB.GetRS("select * from orders where OrderNumber=" + OrderNumber.ToString());
			if(rs.Read())
			{
				TransID = DB.RSField(rs,"AuthorizationPNREF");
				CardNumber = Common.UnmungeString(DB.RSField(rs,"CardNumber"));;
				CardExpirationMonth = DB.RSField(rs,"CardExpirationMonth");
				CardExpirationYear = DB.RSField(rs,"CardExpirationYear");
				OrderTotal = DB.RSFieldDecimal(rs,"OrderTotal");
			}
			if(CardExpirationYear.Length > 2)
			{
				CardExpirationYear = CardExpirationYear.Substring(2,2);
			}
			rs.Close();
			
			if(TransID.Length == 0 || TransID == "0")
			{
				result = "Invalid or Empty Transaction ID";
			}
			else
			{
				LPOrderPart order = LPOrderFactory.createOrderPart("order");
				LPOrderPart op = LPOrderFactory.createOrderPart();
				
				op.put("ordertype","VOID");
				order.addPart("orderoptions", op ); 
	
				op.clear();
				op.put("configfile",Common.AppConfig("YOURPAY_CONFIGFILE"));
				order.addPart("merchantinfo", op );

				// Build creditcard
				op.clear();
				op.put("cardnumber",CardNumber);
				op.put("cardexpmonth",CardExpirationMonth);
				op.put("cardexpyear",CardExpirationYear);
				// add creditcard to order
				order.addPart("creditcard", op );

				// Build payment
				op.clear();
				op.put("chargetotal",Localization.CurrencyStringForGateway(OrderTotal));
				// add payment to order
				order.addPart("payment", op );

				// Add oid
				op.clear();
				op.put("oid",TransID);
				// add transactiondetails to order
				order.addPart("transactiondetails", op );

				LinkPointTxn LPTxn = new LinkPointTxn();

				// get outgoing XML from the 'order' object
				string outXml = order.toXML();

				String KeyFile = HttpContext.Current.Server.MapPath(Common.IIF(Common.IsAdminSite, "../", "") + Common.AppConfig("YOURPAY_KEYFILE"));

				DB.ExecuteSQL("update orders set VoidTXCommand=" + DB.SQuote(outXml) + " where OrderNumber=" + OrderNumber.ToString());

				// Call LPTxn
				String Linkpoint_Host = Common.IIF(Common.AppConfigBool("UseLiveTransactions") , Common.AppConfig("YOURPAY_LIVE_SERVER") , Common.AppConfig("YOURPAY_TEST_SERVER"));
				string rawResponseString = LPTxn.send(KeyFile,Linkpoint_Host,Common.AppConfigUSInt("YOURPAY_PORT"), outXml);
				DB.ExecuteSQL("update orders set VoidTXResult=" + DB.SQuote(rawResponseString)  + " where OrderNumber=" + OrderNumber.ToString());
			
				// response holders
				String R_Approved = ParseTag("r_approved",rawResponseString);
				String R_Code = ParseTag("r_code",rawResponseString);
				String R_Error = ParseTag("r_error",rawResponseString);
				String R_Message = ParseTag("r_message",rawResponseString);

				if(R_Approved == "APPROVED")
				{
					result = "OK";
				}
				else
				{
					result = R_Error;
				}
			}
#endif
			return result;
		}

		static public String RefundOrder(int OrderNumber)
		{
			String result = "YOURPAY COM COMPONENTS NOT INSTALLED ON SERVER OR STOREFRONT NOT COMPILED WITH YOURPAY CODE TURNED ON";
#if YOURPAY

			DB.ExecuteSQL("update orders set RefundTXCommand=NULL, RefundTXResult=NULL where OrderNumber=" + OrderNumber.ToString());
			bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
			String TransID = String.Empty;
			String CardNumber = String.Empty;
			String CardExpirationMonth = String.Empty;
			String CardExpirationYear = String.Empty;
			Decimal OrderTotal = System.Decimal.Zero;
			IDataReader rs = DB.GetRS("select * from orders where OrderNumber=" + OrderNumber.ToString());
			if(rs.Read())
			{
				TransID = DB.RSField(rs,"AuthorizationPNREF");
				CardNumber = Common.UnmungeString(DB.RSField(rs,"CardNumber"));;
				CardExpirationMonth = DB.RSField(rs,"CardExpirationMonth");
				CardExpirationYear = DB.RSField(rs,"CardExpirationYear");
				OrderTotal = DB.RSFieldDecimal(rs,"OrderTotal");
			}
			if(CardExpirationYear.Length > 2)
			{
				CardExpirationYear = CardExpirationYear.Substring(2,2);
			}
			rs.Close();
			
			if(TransID.Length == 0 || TransID == "0")
			{
				result = "Invalid or Empty Transaction ID";
			}
			else
			{
				LPOrderPart order = LPOrderFactory.createOrderPart("order");
				LPOrderPart op = LPOrderFactory.createOrderPart();
				
				op.put("ordertype","CREDIT");
				order.addPart("orderoptions", op ); 
	
				op.clear();
				op.put("configfile",Common.AppConfig("YOURPAY_CONFIGFILE"));
				order.addPart("merchantinfo", op );

				// Build creditcard
				op.clear();
				op.put("cardnumber",CardNumber);
				op.put("cardexpmonth",CardExpirationMonth);
				op.put("cardexpyear",CardExpirationYear);
				// add creditcard to order
				order.addPart("creditcard", op );

				// Build payment
				op.clear();
				op.put("chargetotal",Localization.CurrencyStringForGateway(OrderTotal));
				// add payment to order
				order.addPart("payment", op );

				// Add oid
				op.clear();
				op.put("oid",TransID);
				// add transactiondetails to order
				order.addPart("transactiondetails", op );

				LinkPointTxn LPTxn = new LinkPointTxn();

				// get outgoing XML from the 'order' object
				string outXml = order.toXML();

				String KeyFile = HttpContext.Current.Server.MapPath(Common.IIF(Common.IsAdminSite, "../", "") + Common.AppConfig("YOURPAY_KEYFILE"));

				DB.ExecuteSQL("update orders set RefundTXCommand=" + DB.SQuote(outXml) + " where OrderNumber=" + OrderNumber.ToString());

				// Call LPTxn
				String Linkpoint_Host = Common.IIF(Common.AppConfigBool("UseLiveTransactions") , Common.AppConfig("YOURPAY_LIVE_SERVER") , Common.AppConfig("YOURPAY_TEST_SERVER"));
				string rawResponseString = LPTxn.send(KeyFile,Linkpoint_Host,Common.AppConfigUSInt("YOURPAY_PORT"), outXml);
				DB.ExecuteSQL("update orders set RefundTXResult=" + DB.SQuote(rawResponseString)  + " where OrderNumber=" + OrderNumber.ToString());
			
				// response holders
				String R_Approved = ParseTag("r_approved",rawResponseString);
				String R_Code = ParseTag("r_code",rawResponseString);
				String R_Error = ParseTag("r_error",rawResponseString);
				String R_Message = ParseTag("r_message",rawResponseString);

				if(R_Approved == "APPROVED")
				{
					result = "OK";
				}
				else
				{
					result = R_Error;
				}
			}
#endif
			return result;
		}
		
		
	
		static protected string ParseTag(string tag, string rsp )
		{
			StringBuilder sb = new StringBuilder(256);
			sb.AppendFormat("<{0}>",tag);
			int len = sb.Length; 
			int idxSt=-1;
			int idxEnd=-1; 
			idxSt = rsp.IndexOf(sb.ToString());
			if( idxSt == -1)
			{ 
				return "";
			}
			idxSt += len;
			sb.Remove(0,len);
			sb.AppendFormat("</{0}>",tag);
			idxEnd = rsp.IndexOf(sb.ToString(),idxSt);
			if( idxEnd == -1)
			{
				return "";
			}
			return rsp.Substring(idxSt,idxEnd-idxSt);
		}

		public YourPay()	{}

		// processes card in real time:
		static public String ProcessCard(int OrderNumber, ShoppingCart cart,String CardName, String CardType, String CardNumber, String CardExtraCode, String CardExpirationMonth, String CardExpirationYear, Decimal OrderTotal, bool useLiveTransactions, Address Billing,  Address Shipping, String CAVV, String ECI, String XID, out bool PaymentCleared, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut)
		{
			String result = "YOURPAY COM COMPONENTS NOT INSTALLED ON SERVER OR STOREFRONT NOT COMPILED WITH YOURPAY CODE TURNED ON";
			PaymentCleared = false;
			AuthorizationCode = String.Empty;
			AuthorizationResult = String.Empty;
			AuthorizationTransID = String.Empty;
			AVSResult = String.Empty;
			TransactionCommandOut = String.Empty;
#if YOURPAY

			// create order
			LPOrderPart order = LPOrderFactory.createOrderPart("order");
			// create a part we will use to build the order
			LPOrderPart op = LPOrderFactory.createOrderPart();
				
			// Build 'orderoptions'
			op.put("ordertype",Common.IIF(Common.TransactionMode() == "AUTH", "PREAUTH" , "SALE"));
			// add 'orderoptions to order
			order.addPart("orderoptions", op ); 
	
			// Build 'merchantinfo'
			op.clear();
			op.put("configfile",Common.AppConfig("YOURPAY_CONFIGFILE"));
			// add 'merchantinfo to order
			order.addPart("merchantinfo", op );

			// Build 'billing'
			// Required for AVS. If not provided, 
			// transactions will downgrade.
			op.clear();
			op.put("name",CardName);
			op.put("company",cart._billingAddress.Company);
			op.put("address1",cart._billingAddress.Address1);
			op.put("address2",cart._billingAddress.Address2);
			op.put("city",cart._billingAddress.City);
			op.put("state",cart._billingAddress.State);
			// Required for AVS. If not provided, 
			// transactions will downgrade.	
			String addrnum = String.Empty;
			int ix = cart._billingAddress.Address1.IndexOf(" ");
			if(ix > 0)
			{
				addrnum = cart._billingAddress.Address1.Substring(0,ix);
			}
			if(Common.AppConfigBool("YOURPAY_Verify_Addresses") && addrnum.Length != 0)
			{
				op.put("zip",cart._billingAddress.Zip);
				op.put("addrnum",addrnum);
			}
			op.put("country",cart._billingAddress.Country);
			op.put("phone",cart._billingAddress.Phone);
			op.put("email",cart._billingAddress.Email);
			// add 'billing to order
			order.addPart("billing", op );

			// Build 'creditcard'
			op.clear();
			op.put("cardnumber",CardNumber);
			op.put("cardexpmonth",CardExpirationMonth);
			String ExpYear = CardExpirationYear;
			if(CardExpirationYear.Length > 2)
			{
				ExpYear = CardExpirationYear.Substring(2,2);
			}
			op.put("cardexpyear",ExpYear);
			op.put("cvmvalue",CardExtraCode.PadLeft(3,'0'));
			op.put("cvmindicator",Common.IIF(CardExtraCode.Length != 0 , "provided" , "not_provided"));
			// add 'creditcard to order
			order.addPart("creditcard", op );

			// Build 'payment'
			op.clear();
			op.put("chargetotal",Localization.CurrencyStringForGateway(OrderTotal));
			// add 'payment to order
			order.addPart("payment", op );	

			// add notes 	
			op.clear();
			op.put("comments","CustomerID=" + cart._thisCustomer._customerID.ToString() + ", OrderNumber=" + OrderNumber.ToString());
			order.addPart("notes",op);

			// create transaction object	
			LinkPointTxn LPTxn = new LinkPointTxn();

			// get outgoing XML from the 'order' object
			string outXml = order.toXML();

			String KeyFile = HttpContext.Current.Server.MapPath(Common.IIF(Common.IsAdminSite, "../", "") + Common.AppConfig("YOURPAY_KEYFILE"));

			// Call LPTxn
			String Linkpoint_Host = Common.IIF(Common.AppConfigBool("UseLiveTransactions") , Common.AppConfig("YOURPAY_LIVE_SERVER") , Common.AppConfig("YOURPAY_TEST_SERVER"));
			string rawResponseString = LPTxn.send(KeyFile,Linkpoint_Host,Common.AppConfigUSInt("YOURPAY_PORT"), outXml);

			// response holders
			String R_Time = ParseTag("r_time",rawResponseString);
			String R_Ref = ParseTag("r_ref",rawResponseString);
			String R_Approved = ParseTag("r_approved",rawResponseString);
			String R_Code = ParseTag("r_code",rawResponseString);
			String R_Authresr = ParseTag("r_authresronse",rawResponseString);
			String R_Error = ParseTag("r_error",rawResponseString);
			String R_OrderNum = ParseTag("r_ordernum",rawResponseString);
			String R_Message = ParseTag("r_message",rawResponseString);
			String R_Score = ParseTag("r_score",rawResponseString);
			String R_TDate = ParseTag("r_tdate",rawResponseString);
			String R_AVS = ParseTag("r_avs",rawResponseString);
			String R_Tax = ParseTag("r_tax",rawResponseString);
			String R_Shipping = ParseTag("r_shipping",rawResponseString);
			String R_FraudCode = ParseTag("r_fraudCode",rawResponseString);
			String R_ESD = ParseTag("esd",rawResponseString);		
			
			String sql = String.Empty;

			PaymentCleared = false;
			AuthorizationCode = R_Code;
			AuthorizationResult = rawResponseString;
			AuthorizationTransID = R_OrderNum;
			AVSResult = R_AVS;
			TransactionCommandOut = outXml.ToString();
			
			if(R_Approved.ToUpper() == "APPROVED")
			{
				result = "OK";
				PaymentCleared = true;
			}
			else
			{
				result = R_Error;
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
				sql = "insert into FailedTransaction(CustomerID,OrderDate,PaymentGateway,TransactionCommand,TransactionResult) values(" + cart._thisCustomer._customerID.ToString() + "," + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + "," + DB.SQuote("LINKPOINT") + "," + DB.SQuote(outXml.Replace(CardNumber,CC)) + "," + DB.SQuote(rawResponseString) + ")";
				DB.ExecuteSQL(sql);
			}
#endif
			return result;
		}		
	}
}
