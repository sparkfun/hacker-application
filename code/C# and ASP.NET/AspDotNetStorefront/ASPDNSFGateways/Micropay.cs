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
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontGateways
{
  /// <summary>
  /// Summary description for Micropay.
  /// </summary>
  public class Micropay
  {
    public Micropay() {}

    static public String CaptureOrder(int OrderNumber)
    {
      String result = "OK";

      DB.ExecuteSQL(String.Format("update orders set CaptureTXCommand=NULL, CaptureTXResult=NULL where OrderNumber={0}",OrderNumber));
      bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
      
      Decimal OrderTotal = System.Decimal.Zero;
      Decimal mpBalance = System.Decimal.Zero;

      int CustomerID = 0;

      IDataReader rs = DB.GetRS(String.Format("select * from orders where OrderNumber={0}",OrderNumber));
      if(rs.Read())
      {
        OrderTotal = DB.RSFieldDecimal(rs,"OrderTotal");
        CustomerID = DB.RSFieldInt(rs,"CustomerID");
        mpBalance = Common.GetMicroPayBalance(CustomerID);
      }
      rs.Close();      

      DB.ExecuteSQL(String.Format("update customer set MicroPayBalance={0} where CustomerID={1}",(mpBalance - OrderTotal),CustomerID));
      DB.ExecuteSQL(String.Format("update orders set CaptureTXCommand='Capture MicroPay {0}' where OrderNumber={1}", OrderTotal, OrderNumber));
      DB.ExecuteSQL(String.Format("update orders set CaptureTXResult='MicroPay Balance {0} => {1}' where OrderNumber={2}",mpBalance, (mpBalance - OrderTotal), OrderNumber));
      
      return result;
    }

    static public String VoidOrder(int OrderNumber)
    {
      String result = "OK";

      DB.ExecuteSQL(String.Format("update orders set VoidTXCommand=NULL, VoidTXResult=NULL where OrderNumber={0}",OrderNumber));
      bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
      
      Decimal OrderTotal = System.Decimal.Zero;
      Decimal mpBalance = System.Decimal.Zero;

      int CustomerID = 0;

      IDataReader rs = DB.GetRS(String.Format("select * from orders where OrderNumber={0}",OrderNumber));
      if(rs.Read())
      {

        OrderTotal = DB.RSFieldDecimal(rs,"OrderTotal");
        CustomerID = DB.RSFieldInt(rs,"CustomerID");
        mpBalance = Common.GetMicroPayBalance(CustomerID);
      }
      rs.Close();      

      DB.ExecuteSQL(String.Format("update customer set MicroPayBalance={0} where CustomerID={1}",(mpBalance + OrderTotal),CustomerID));
      DB.ExecuteSQL(String.Format("update orders set VoidTXCommand='Void MicroPay {0}' where OrderNumber={1}", OrderTotal, OrderNumber));
      DB.ExecuteSQL(String.Format("update orders set VoidTXResult='MicroPay Balance {0} => {1}' where OrderNumber={2}",mpBalance, (OrderTotal + mpBalance), OrderNumber));
      
      return result;
    }

    static public String RefundOrder(int OrderNumber)
    {
      String result = "OK";

      DB.ExecuteSQL(String.Format("update orders set RefundTXCommand=NULL, RefundTXResult=NULL where OrderNumber={0}",OrderNumber));
      bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
      
      Decimal OrderTotal = System.Decimal.Zero;
      Decimal mpBalance = System.Decimal.Zero;

      int CustomerID = 0;

      IDataReader rs = DB.GetRS(String.Format("select * from orders where OrderNumber={0}",OrderNumber));
      if(rs.Read())
      {
        OrderTotal = DB.RSFieldDecimal(rs,"OrderTotal");
        CustomerID = DB.RSFieldInt(rs,"CustomerID");
        mpBalance = Common.GetMicroPayBalance(CustomerID);
      }
      rs.Close();      

      DB.ExecuteSQL(String.Format("update customer set MicroPayBalance={0} where CustomerID={1}",(mpBalance + OrderTotal),CustomerID));
      DB.ExecuteSQL(String.Format("update orders set RefundTXCommand='Refund to MicroPay {0}' where OrderNumber={1}", OrderTotal, OrderNumber));
      DB.ExecuteSQL(String.Format("update orders set RefundTXResult='MicroPay Balance {0} => {1}' where OrderNumber={2}",mpBalance, (OrderTotal + mpBalance), OrderNumber));
      
      return result;
    }

    static public String ProcessMicropay(int OrderNumber, ShoppingCart cart, Decimal OrderTotal, Address Billing, out bool PaymentCleared, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut)
    {
      String result = "OK";
      String sql = String.Empty;

      bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
      //First do they have enough money?
      decimal mpBalance = Common.GetMicroPayBalance(cart._thisCustomer._customerID);
      if ( OrderTotal > mpBalance)
      {
        result = "INSUFFICIENT " + Common.AppConfig("Micropay.Prompt").ToUpper() + " BALANCE FOR THIS ORDER";
      }
      else
      {
        //Update balance if in AUTH CAPTURE mode
        if (Common.AppConfig("TransactionMode").ToUpper().Trim() == "AUTH CAPTURE")
        {
          DB.ExecuteSQL(String.Format("update customer set MicroPayBalance={0} where CustomerID={1}",(mpBalance - OrderTotal),cart._thisCustomer._customerID));
        }
      }      

      PaymentCleared = false;
      AVSResult = "N/A";
      AuthorizationCode = "MicroPay";
      AuthorizationResult = result;
      AuthorizationTransID = "MicroPay";
      TransactionCommandOut = "Pay by MicroPay";
      
      if(result.ToUpper() == "OK") 
      {
        PaymentCleared = true;
      }
      else 
      {
        // record failed TX:
        sql = "insert into FailedTransaction(CustomerID,OrderDate,PaymentGateway,TransactionCommand,TransactionResult) values(" + cart._thisCustomer._customerID.ToString() + "," + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + "," + DB.SQuote("MICROPAY") + "," + DB.SQuote(TransactionCommandOut) + "," + DB.SQuote(result) + ")";
        DB.ExecuteSQL(sql);
      }
      return result;
    }
    
  }
}

