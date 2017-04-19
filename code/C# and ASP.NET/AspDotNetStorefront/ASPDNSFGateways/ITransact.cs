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
	/// Summary description for ITransact.
	/// </summary>
	public class ITransact
	{
		public ITransact() {}

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
      
			transactionCommand.Append(              "<?xml version=\"1.0\"?>");
			transactionCommand.Append(              "  <ITransactInterface>");
			transactionCommand.Append(              "    <VendorIdentification>");
			transactionCommand.Append(String.Format("      <VendorId>{0}</VendorId>",Common.AppConfig("ITransact.Vendor_ID").Trim()));
			transactionCommand.Append(String.Format("      <VendorPassword>{0}</VendorPassword>",Common.AppConfig("ITransact.Password").Trim()));
			transactionCommand.Append(String.Format("      <HomePage>{0}</HomePage>",Common.AppConfig("LiveServer").Trim()));
			transactionCommand.Append(              "    </VendorIdentification>");
			transactionCommand.Append(              "    <PostAuthTransaction>");
			transactionCommand.Append(String.Format("      <OperationXID>{0}</OperationXID>",TransID));
			transactionCommand.Append(String.Format("      <Total>{0}</Total>",Localization.CurrencyStringForGateway(OrderTotal)));
			transactionCommand.Append(              "      <TransactionControl>");
			//Capture in Storefront doesn't currently support a message to go with the PostAuth email TBD?
			transactionCommand.Append(              "        <SendCustomerEmail>TRUE</SendCustomerEmail>");
			transactionCommand.Append(              "        <EmailText>");
			transactionCommand.Append(              "          <EmailTextItem></EmailTextItem>");
			transactionCommand.Append(              "          <EmailTextItem></EmailTextItem>");
			transactionCommand.Append(              "        </EmailText>");
			transactionCommand.Append(              "      </TransactionControl>");
			transactionCommand.Append(              "    </PostAuthTransaction>");
			transactionCommand.Append(              "  </ITransactInterface>");

			// Is the command good XML
			XmlDocument cmdXml = new XmlDocument();
			try
			{
				cmdXml.LoadXml(transactionCommand.ToString());
			}
			catch
			{
				DB.ExecuteSQL("update orders set CaptureTXCommand=" + DB.SQuote(transactionCommand.ToString()) + " where OrderNumber=" + OrderNumber.ToString());
				return "Capture Command is not valid XML.";
			}

			//Have good Xml. Lets make it pretty
			transactionCommand.Length = 0; //Clear the builder
			transactionCommand.Append(Common.FormatXml(cmdXml));

			DB.ExecuteSQL("update orders set CaptureTXCommand=" + DB.SQuote(transactionCommand.ToString()) + " where OrderNumber=" + OrderNumber.ToString());

			if(TransID.Length == 0 || TransID == "9999999999") // 9999999999 is a test transaction
			{
				result = "Invalid or Empty Transaction ID";
			}
			else
			{
				try
				{
					ASCIIEncoding encoding = new ASCIIEncoding();
					byte[]  data = encoding.GetBytes(transactionCommand.ToString());

					// Prepare web request...
					String AuthServer = Common.AppConfig("ITransact.VoidRefund_Server");
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

					String replyCode = String.Empty;
					String authResponse = String.Empty;

					XmlDocument responseXml = new XmlDocument();
					try
					{
						//Make sure it's good XML
						responseXml.LoadXml(rawResponseString.Trim());
						//Have good Xml. Lets make it pretty
						rawResponseString = Common.FormatXml(responseXml);

						replyCode = responseXml.SelectSingleNode("//Status").InnerText;
						authResponse = responseXml.SelectSingleNode("//ErrorCategory").InnerText +" "+responseXml.SelectSingleNode("//ErrorMessage").InnerText;
					}
					catch
					{
						authResponse="GARBLED RESPONSE FROM THE GATEWAY";
					}

					DB.ExecuteSQL("update orders set CaptureTXResult=" + DB.SQuote(rawResponseString)  + " where OrderNumber=" + OrderNumber.ToString());
					if(replyCode.ToUpper() == "OK") // They sometimes return OK or ok so ignore case
					{
						result = "OK";
					}
					else
					{
						result = authResponse;
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
			String CaptureTXResult = String.Empty;
      
			Decimal OrderTotal = System.Decimal.Zero;
			IDataReader rs = DB.GetRS("select * from orders where OrderNumber=" + OrderNumber.ToString());
			if(rs.Read())
			{
				TransID = DB.RSField(rs,"AuthorizationPNREF");
				OrderTotal = DB.RSFieldDecimal(rs,"OrderTotal");

				// You can only Void a PostAuth transaction so you may need the Transaction ID for the Capture Transaction 
				CaptureTXResult = DB.RSField(rs,"CaptureTXResult");
			}
			rs.Close();      

			//If there was a Capture TXResult then Void the Capture TX instead
			if (CaptureTXResult.Length != 0)
			{
				try
				{
					XmlDocument captureXml = new XmlDocument();
					captureXml.LoadXml(CaptureTXResult);
					// Get the Transaction ID of the Capture Transaction instead
					TransID = captureXml.SelectSingleNode("//XID").InnerText;
				}
				catch
				{
					result = String.Format("Invalid Capture Transaction ID: {0}",TransID);
				}
			}
            
			transactionCommand.Append(              "<?xml version=\"1.0\"?>");
			transactionCommand.Append(              "  <ITransactInterface>");
			transactionCommand.Append(              "    <VendorIdentification>");
			transactionCommand.Append(String.Format("      <VendorId>{0}</VendorId>",Common.AppConfig("ITransact.Vendor_ID").Trim()));
			transactionCommand.Append(String.Format("      <VendorPassword>{0}</VendorPassword>",Common.AppConfig("ITransact.Password").Trim()));
			transactionCommand.Append(String.Format("      <HomePage>{0}</HomePage>",Common.AppConfig("LiveServer").Trim()));
			transactionCommand.Append(              "    </VendorIdentification>");
			transactionCommand.Append(              "    <VoidTransaction>");
			transactionCommand.Append(String.Format("      <OperationXID>{0}</OperationXID>",TransID));
			transactionCommand.Append(              "      <TransactionControl>");
			//Void in Storefront doesn't currently support a message to go with the void email TBD?
			transactionCommand.Append(              "        <SendCustomerEmail>TRUE</SendCustomerEmail>");
			transactionCommand.Append(              "        <EmailText>");
			transactionCommand.Append(              "          <EmailTextItem></EmailTextItem>");
			transactionCommand.Append(              "          <EmailTextItem></EmailTextItem>");
			transactionCommand.Append(              "        </EmailText>");
			transactionCommand.Append(              "      </TransactionControl>");
			transactionCommand.Append(              "    </VoidTransaction>");
			transactionCommand.Append(              "  </ITransactInterface>");


			// Is the command good XML
			XmlDocument cmdXml = new XmlDocument();
			try
			{
				cmdXml.LoadXml(transactionCommand.ToString());
			}
			catch
			{
				//Save bad version for inspection
				DB.ExecuteSQL("update orders set VoidTXCommand=" + DB.SQuote(transactionCommand.ToString()) + " where OrderNumber=" + OrderNumber.ToString());
				return "Void Command is not valid XML.";
			}

			//Have good Xml. Lets make it pretty
			transactionCommand.Length = 0; //Clear the builder
			transactionCommand.Append(Common.FormatXml(cmdXml));
			//Save reformatted version 

			DB.ExecuteSQL("update orders set VoidTXCommand=" + DB.SQuote(transactionCommand.ToString()) + " where OrderNumber=" + OrderNumber.ToString());

			if(TransID.Length == 0 || TransID == "9999999999") // 9999999999 is a test transaction
			{
				result = String.Format("Invalid or Empty Transaction ID: {0}",TransID);
			}
			else
			{
				try
				{
					ASCIIEncoding encoding = new ASCIIEncoding();
					byte[]  data = encoding.GetBytes(transactionCommand.ToString());

					// Prepare web request...
					String AuthServer = Common.AppConfig("ITransact.VoidRefund_Server");
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

					String replyCode = String.Empty;
					String authResponse = String.Empty;

					XmlDocument responseXml = new XmlDocument();
					try
					{
						//Make sure it's good XML
						responseXml.LoadXml(rawResponseString.Trim());
						//Have good Xml. Lets make it pretty
						rawResponseString = Common.FormatXml(responseXml);

						replyCode = responseXml.SelectSingleNode("//Status").InnerText;
						authResponse = responseXml.SelectSingleNode("//ErrorCategory").InnerText +" "+responseXml.SelectSingleNode("//ErrorMessage").InnerText;
					}
					catch
					{
						authResponse="GARBLED RESPONSE FROM THE GATEWAY";
					}

					DB.ExecuteSQL("update orders set VoidTXResult=" + DB.SQuote(rawResponseString)  + " where OrderNumber=" + OrderNumber.ToString());

					if(replyCode.ToUpper() == "OK") // They sometimes return OK or ok so ignore case
					{
						result = "OK";
					}
					else
					{
						result = authResponse;
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
      
			transactionCommand.Append(              "<?xml version=\"1.0\"?>");
			transactionCommand.Append(              "  <ITransactInterface>");
			transactionCommand.Append(              "    <VendorIdentification>");
			transactionCommand.Append(String.Format("      <VendorId>{0}</VendorId>",Common.AppConfig("ITransact.Vendor_ID").Trim()));
			transactionCommand.Append(String.Format("      <VendorPassword>{0}</VendorPassword>",Common.AppConfig("ITransact.Password").Trim()));
			transactionCommand.Append(String.Format("      <HomePage>{0}</HomePage>",Common.AppConfig("LiveServer").Trim()));
			transactionCommand.Append(              "    </VendorIdentification>");
			transactionCommand.Append(              "    <TranCredTransaction>");
			transactionCommand.Append(String.Format("      <OperationXID>{0}</OperationXID>",TransID));
			transactionCommand.Append(String.Format("      <Total>{0:###0.00}</Total>",OrderTotal));
			transactionCommand.Append(              "      <TransactionControl>");
      
			//Credit in Storefront doesn't currently support a message to go with the void email TBD?
			transactionCommand.Append(              "        <SendCustomerEmail>TRUE</SendCustomerEmail>");
			transactionCommand.Append(              "        <EmailText>");
			transactionCommand.Append(              "          <EmailTextItem></EmailTextItem>");
			transactionCommand.Append(              "          <EmailTextItem></EmailTextItem>");
			transactionCommand.Append(              "        </EmailText>");
			transactionCommand.Append(              "      </TransactionControl>");
			transactionCommand.Append(              "    </TranCredTransaction>");
			transactionCommand.Append(              "  </ITransactInterface>");

			// Is the command good XML
			XmlDocument cmdXml = new XmlDocument();
			try
			{
				cmdXml.LoadXml(transactionCommand.ToString());
			}
			catch
			{
				DB.ExecuteSQL("update orders set RefundTXCommand=" + DB.SQuote(transactionCommand.ToString()) + " where OrderNumber=" + OrderNumber.ToString());
				return "Refund Command is not valid XML.";
			}

			//Have good Xml. Lets make it pretty
			transactionCommand.Length = 0; //Clear the builder
			transactionCommand.Append(Common.FormatXml(cmdXml));

			DB.ExecuteSQL("update orders set RefundTXCommand=" + DB.SQuote(transactionCommand.ToString()) + " where OrderNumber=" + OrderNumber.ToString());

			if(TransID.Length == 0 || TransID == "9999999999") // 9999999999 is a test transaction
			{
				result = String.Format("Invalid or Empty Transaction ID: {0}",TransID);
			}
			else
			{
				try
				{
					ASCIIEncoding encoding = new ASCIIEncoding();
					byte[]  data = encoding.GetBytes(transactionCommand.ToString());

					// Prepare web request...
					String AuthServer = Common.AppConfig("ITransact.VoidRefund_Server");
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

					String replyCode = String.Empty;
					String authResponse = String.Empty;

					XmlDocument responseXml = new XmlDocument();
					try
					{
						//Make sure it's good XML
						responseXml.LoadXml(rawResponseString.Trim());
						//Have good Xml. Lets make it pretty
						rawResponseString = Common.FormatXml(responseXml);
 
						replyCode = responseXml.SelectSingleNode("//Status").InnerText;
						authResponse = responseXml.SelectSingleNode("//ErrorCategory").InnerText +" "+responseXml.SelectSingleNode("//ErrorMessage").InnerText;
					}
					catch
					{
						authResponse="GARBLED RESPONSE FROM THE GATEWAY";
					}

					DB.ExecuteSQL("update orders set RefundTXResult=" + DB.SQuote(rawResponseString)  + " where OrderNumber=" + OrderNumber.ToString());
					if(replyCode.ToUpper() == "OK") // They sometimes return OK or ok so ignore case
					{
						result = "OK";
					}
					else
					{
						result = authResponse;
					}
				}
				catch
				{
					result = "NO RESPONSE FROM GATEWAY!";
				}
			}
			return result;
		}
    
		static public String ProcessCard(int OrderNumber, ShoppingCart cart, String CardName, String CardType, String CardNumber, String CardExtraCode, String CardExpirationMonth, String CardExpirationYear, Decimal OrderTotal, bool useLiveTransactions, Address Billing, Address Shipping, String CAVV, String ECI, String XID, out bool PaymentCleared, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut)
		{
			String result = "OK";

			ASCIIEncoding encoding = new ASCIIEncoding();
			StringBuilder transactionCommand = new StringBuilder(5000);

			transactionCommand.Append(              "<?xml version=\"1.0\"?>");
			transactionCommand.Append(              "<SaleRequest>");
			transactionCommand.Append(              "  <CustomerData>");
			transactionCommand.Append(String.Format("    <Email>{0}</Email>",Common.XmlEncode(cart._email)));

			transactionCommand.Append(              "    <BillingAddress>");
			//Substitute the ITransact.Test_FirstName if not a live transaction
			transactionCommand.Append(String.Format("      <FirstName>{0}</FirstName>",Common.IIF(useLiveTransactions,Common.XmlEncode(cart._billingAddress.FirstName),Common.AppConfig("ITransact.Test_FirstName"))));
			transactionCommand.Append(String.Format("      <LastName>{0}</LastName>",Common.XmlEncode(cart._billingAddress.LastName)));
			transactionCommand.Append(String.Format("       <Address1>{0}</Address1>",Common.XmlEncode(cart._billingAddress.Address1)));
      
			if (cart._billingAddress.Address2.Length != 0)
				transactionCommand.Append(String.Format("       <Address2>{0}</Address2>",Common.XmlEncode(cart._billingAddress.Address2)));
      
			transactionCommand.Append(String.Format("      <City>{0}</City>",Common.XmlEncode(cart._billingAddress.City)));
			transactionCommand.Append(String.Format("      <State>{0}</State>",Common.XmlEncode(cart._billingAddress.State)));
			transactionCommand.Append(String.Format("      <Zip>{0}</Zip>",Common.XmlEncode(cart._billingAddress.Zip)));
			transactionCommand.Append(String.Format("      <Country>{0}</Country>",Common.XmlEncode(cart._billingAddress.Country)));
			transactionCommand.Append(String.Format("      <Phone>{0}</Phone>",Common.XmlEncode(cart._billingAddress.Phone)));
			transactionCommand.Append(              "    </BillingAddress>");
      
			transactionCommand.Append(              "    <ShippingAddress>");
			transactionCommand.Append(String.Format("      <FirstName>{0}</FirstName>",Common.XmlEncode(cart._shippingAddress.FirstName)));
			transactionCommand.Append(String.Format("      <LastName>{0}</LastName>",Common.XmlEncode(cart._shippingAddress.LastName)));
			transactionCommand.Append(String.Format("       <Address1>{0}</Address1>",Common.XmlEncode(cart._shippingAddress.Address1)));

			if (cart._shippingAddress.Address2.Length != 0)
				transactionCommand.Append(String.Format("       <Address2>{0}</Address2>",Common.XmlEncode(cart._shippingAddress.Address2)));
      
			transactionCommand.Append(String.Format("      <City>{0}</City>",Common.XmlEncode(cart._shippingAddress.City)));
			transactionCommand.Append(String.Format("      <State>{0}</State>",Common.XmlEncode(cart._shippingAddress.State)));
			transactionCommand.Append(String.Format("      <Zip>{0}</Zip>",Common.XmlEncode(cart._shippingAddress.Zip)));
			transactionCommand.Append(String.Format("      <Country>{0}</Country>",Common.XmlEncode(cart._shippingAddress.Country)));
			transactionCommand.Append(String.Format("      <Phone>{0}</Phone>",Common.XmlEncode(cart._shippingAddress.Phone)));
			transactionCommand.Append(              "    </ShippingAddress>");
      
			transactionCommand.Append(              "    <AccountInfo>");
      
			transactionCommand.Append(              "      <CardInfo>");
			transactionCommand.Append(String.Format("      <CCNum>{0}</CCNum>",CardNumber));
			transactionCommand.Append(String.Format("      <CCMo>{0}</CCMo>",CardExpirationMonth.PadLeft(2,'0')));
			transactionCommand.Append(String.Format("      <CCYr>{0}</CCYr>",CardExpirationYear.ToString()));
    
      if (CardExtraCode.Trim().Length == 0)
      {
        transactionCommand.Append("<CVV2Number />");
        transactionCommand.Append(            "      <CVV2Illegible>1</CVV2Illegible>");
      }
      else
      {
        transactionCommand.Append(String.Format("      <CVV2Number>{0}</CVV2Number>",CardExtraCode));
      }
      
      transactionCommand.Append(              "      </CardInfo>");
      
			transactionCommand.Append(              "    </AccountInfo>");
			transactionCommand.Append(              "  </CustomerData>");

			transactionCommand.Append(              "  <TransactionData>");

			transactionCommand.Append(String.Format("      <VendorId>{0}</VendorId>",Common.AppConfig("ITransact.Vendor_ID").Trim()));
			transactionCommand.Append(String.Format("      <VendorPassword>{0}</VendorPassword>",Common.AppConfig("ITransact.Password").Trim()));
			transactionCommand.Append(String.Format("      <HomePage>{0}</HomePage>",Common.AppConfig("LiveServer").Trim()));
      
			// Add Preauth tag if auth only
			if (Common.TransactionMode() == "AUTH")
			{
				transactionCommand.Append(              "      <Preauth />");
			}
      
			// Email message text for the ITransact receipt since it's only a total.
			transactionCommand.Append(              "      <EmailText>");
			transactionCommand.Append(              "        <EmailTextItem>Detailed receipt will be sent in a separate email.</EmailTextItem>");
			transactionCommand.Append(              "      </EmailText>");

			transactionCommand.Append(              "    <OrderItems>");

			decimal productTotal = System.Decimal.Zero;

			foreach (CartItem item in cart._cartItems)
			{
				transactionCommand.Append(              "      <Item>");
				transactionCommand.Append(String.Format("        <Description>{0}</Description>",item.productName));
				transactionCommand.Append(String.Format("        <Cost>{0}</Cost>",Localization.CurrencyStringForGateway(item.price)));
				transactionCommand.Append(String.Format("        <Qty>{0}</Qty>",item.quantity));
				transactionCommand.Append(              "      </Item>");
				productTotal += (item.price * item.quantity);
			}

			productTotal += (cart.TaxTotal(true));
			productTotal += (cart.ShippingTotal(true));

			if (productTotal != OrderTotal)
			{
				transactionCommand.Append(              "      <Item>");
				transactionCommand.Append(              "        <Description>Discounts and Coupons - You saved</Description>");
				transactionCommand.Append(String.Format("        <Cost>{0}</Cost>",Localization.CurrencyStringForGateway(OrderTotal-productTotal)));
				transactionCommand.Append(              "        <Qty>1</Qty>");
				transactionCommand.Append(              "      </Item>");
			}

			if (cart.TaxTotal(true) != 0)
			{
				transactionCommand.Append(              "      <Item>");
				transactionCommand.Append(              "        <Description>Tax</Description>");
				transactionCommand.Append(String.Format("        <Cost>{0}</Cost>",Localization.CurrencyStringForGateway(cart.TaxTotal(true))));
				transactionCommand.Append(              "        <Qty>1</Qty>");
				transactionCommand.Append(              "      </Item>");
			}
      
			if (cart.ShippingTotal(true) != 0)
			{
				transactionCommand.Append(              "      <Item>");
				transactionCommand.Append(              "        <Description>Shipping</Description>");
				transactionCommand.Append(String.Format("        <Cost>{0}</Cost>",Localization.CurrencyStringForGateway(cart.ShippingTotal(true))));
				transactionCommand.Append(              "        <Qty>1</Qty>");
				transactionCommand.Append(              "      </Item>");
			}
      
			//     
			//      transactionCommand.Append(              "      <Item>");
			//      transactionCommand.Append(String.Format("        <Description>{0} - Order# {1}</Description>",Common.AppConfig("StoreName"),OrderNumber));
			//      transactionCommand.Append(String.Format("        <Cost>{0}</Cost>",Localization.CurrencyStringForGateway(OrderTotal)));
			//      transactionCommand.Append(              "        <Qty>1</Qty>");
			//      transactionCommand.Append(              "      </Item>");

			transactionCommand.Append(              "    </OrderItems>");

			transactionCommand.Append(              "  </TransactionData>");
			transactionCommand.Append(              "</SaleRequest>");

			// Is the command good XML
			XmlDocument cmdXml = new XmlDocument();
			try
			{
				cmdXml.LoadXml(transactionCommand.ToString());
			}
			catch
			{
			}

			//Have good Xml. Lets make it pretty
			transactionCommand.Length = 0; //Clear the builder
			transactionCommand.Append(Common.FormatXml(cmdXml));

			byte[]  data = encoding.GetBytes(transactionCommand.ToString());

			// Prepare web request...
			String AuthServer = Common.AppConfig("ITransact.Sale_Server"); // Use the Sale Transaction Server

			WebResponse myResponse;
			String rawResponseString = String.Empty;

			try
			{
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
				myResponse = myRequest.GetResponse();
				StreamReader sr = new StreamReader(myResponse.GetResponseStream());
				rawResponseString = sr.ReadToEnd();
				// Close and clean up the StreamReader
				sr.Close();
				//Close the Response
				myResponse.Close();
			}
			catch
			{
				result = "NO RESPONSE FROM GATEWAY!";
			}

			// rawResponseString now has gateway response
      
			AVSResult = String.Empty;

			String replyCode = String.Empty;
			String approvalCode = String.Empty;
			String authResponse = String.Empty;
			String TransID = String.Empty;
      
			XmlDocument responseXml = new XmlDocument();
      
			try
			{
				responseXml.LoadXml(rawResponseString);
				//Good Xml make it pretty
				rawResponseString = Common.FormatXml(responseXml);

				replyCode = responseXml.SelectSingleNode("//Status").InnerText;
				approvalCode = responseXml.SelectSingleNode("//AuthCode").InnerText;
				authResponse = responseXml.SelectSingleNode("//ErrorCategory").InnerText +" "+responseXml.SelectSingleNode("//ErrorMessage").InnerText;
				TransID = responseXml.SelectSingleNode("//XID").InnerText;
				AVSResult = responseXml.SelectSingleNode("//AVSResponse").InnerText;
			}
			catch
			{
				authResponse="GARBLED RESPONSE FROM THE GATEWAY";
			}

			PaymentCleared = false;
			AuthorizationCode = approvalCode;
			AuthorizationResult = rawResponseString;
			AuthorizationTransID = TransID;
			TransactionCommandOut = transactionCommand.ToString();
      
			if(replyCode.ToUpper() == "OK") // They sometimes return OK or ok so ignore case
			{
				PaymentCleared = true;
			}
			else if(Common.AppConfigBool("StoreFailedTransactionsAsOrders"))
			{
				// store the order as if it was ok, send the receipt, but mark the order as FAILED:
				// note, must return OK out of this routine, so rest of order processing continues on
				result = "OK";
				String CC = String.Empty;
				if(Common.AppConfigBool("StoreCCInDB"))
				{
					CC = Common.MungeString(CardNumber);
				}
				else
				{
					CC = "XXXXXXXXXXXX";
				}
				String sql2 = "update orders set " +
					"PaymentGateway=" + DB.SQuote("ITransact") + ", " +
					"AuthorizationResult=" + DB.SQuote("FAILED: " + rawResponseString) + ", " +
					"AuthorizationCode=" + DB.SQuote(approvalCode) + ", " + 
					"AuthorizationPNREF=NULL, " + 
					"TransactionCommand=" + DB.SQuote(transactionCommand.ToString().Replace(CardNumber,CC)) +
					" where OrderNumber=" + OrderNumber.ToString();
				DB.ExecuteSQL(sql2);
			}
			else
			{
				result = authResponse;
				if(result.Length == 0)
				{
					result = "Unspecified Error";
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
				String sql = "insert into FailedTransaction(CustomerID,OrderDate,PaymentGateway,TransactionCommand,TransactionResult) values(" + cart._thisCustomer._customerID.ToString() + "," + DB.DateQuote(Localization.ToNativeShortDateString(System.DateTime.Now)) + "," + DB.SQuote("ITRANSACT") + "," + DB.SQuote(transactionCommand.ToString().Replace(CardNumber,CC)) + "," + DB.SQuote(rawResponseString) + ")";
				DB.ExecuteSQL(sql);
			}
			return result;
		}

		static public String ProcessECheck(int OrderNumber, ShoppingCart cart, String eCheckBankABACode,String eCheckBankAccountNumber,String eCheckBankAccountType,String eCheckBankName,String eCheckBankAccountName, Decimal OrderTotal, Address Billing, Address Shipping, out bool PaymentCleared, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut)
		{
			String result = "OK";

			bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");

			ASCIIEncoding encoding = new ASCIIEncoding();
			StringBuilder transactionCommand = new StringBuilder(5000);

			transactionCommand.Append(              "<?xml version=\"1.0\"?>");
			transactionCommand.Append(              "<SaleRequest>");
			transactionCommand.Append(              "  <CustomerData>");
			transactionCommand.Append(String.Format("    <Email>{0}</Email>",Common.XmlEncode(cart._email)));

			transactionCommand.Append(              "    <BillingAddress>");
			//Substitute the ITransact.Test_FirstName if not a live transaction
			transactionCommand.Append(String.Format("      <FirstName>{0}</FirstName>",Common.IIF(useLiveTransactions,Common.XmlEncode(cart._billingAddress.FirstName),Common.AppConfig("ITransact.Test_FirstName"))));
			transactionCommand.Append(String.Format("      <LastName>{0}</LastName>",Common.XmlEncode(cart._billingAddress.LastName)));
			transactionCommand.Append(String.Format("       <Address1>{0}</Address1>",Common.XmlEncode(cart._billingAddress.Address1)));
      
			if (cart._billingAddress.Address2.Length != 0)
				transactionCommand.Append(String.Format("       <Address2>{0}</Address2>",Common.XmlEncode(cart._billingAddress.Address2)));
      
			transactionCommand.Append(String.Format("      <City>{0}</City>",Common.XmlEncode(cart._billingAddress.City)));
			transactionCommand.Append(String.Format("      <State>{0}</State>",Common.XmlEncode(cart._billingAddress.State)));
			transactionCommand.Append(String.Format("      <Zip>{0}</Zip>",Common.XmlEncode(cart._billingAddress.Zip)));
			transactionCommand.Append(String.Format("      <Country>{0}</Country>",Common.XmlEncode(cart._billingAddress.Country)));
			transactionCommand.Append(String.Format("      <Phone>{0}</Phone>",Common.XmlEncode(cart._billingAddress.Phone)));
			transactionCommand.Append(              "    </BillingAddress>");
      
			transactionCommand.Append(              "    <ShippingAddress>");
			transactionCommand.Append(String.Format("      <FirstName>{0}</FirstName>",Common.XmlEncode(cart._shippingAddress.FirstName)));
			transactionCommand.Append(String.Format("      <LastName>{0}</LastName>",Common.XmlEncode(cart._shippingAddress.LastName)));
			transactionCommand.Append(String.Format("       <Address1>{0}</Address1>",Common.XmlEncode(cart._shippingAddress.Address1)));

			if (cart._shippingAddress.Address2.Length != 0)
				transactionCommand.Append(String.Format("       <Address2>{0}</Address2>",Common.XmlEncode(cart._shippingAddress.Address2)));
      
			transactionCommand.Append(String.Format("      <City>{0}</City>",Common.XmlEncode(cart._shippingAddress.City)));
			transactionCommand.Append(String.Format("      <State>{0}</State>",Common.XmlEncode(cart._shippingAddress.State)));
			transactionCommand.Append(String.Format("      <Zip>{0}</Zip>",Common.XmlEncode(cart._shippingAddress.Zip)));
			transactionCommand.Append(String.Format("      <Country>{0}</Country>",Common.XmlEncode(cart._shippingAddress.Country)));
			transactionCommand.Append(String.Format("      <Phone>{0}</Phone>",Common.XmlEncode(cart._shippingAddress.Phone)));
			transactionCommand.Append(              "    </ShippingAddress>");
      
			transactionCommand.Append(              "    <AccountInfo>");
      
			//Translate the account type
			string AccountType = String.Empty;
			switch (eCheckBankAccountType)
			{
				case "CHECKING" : AccountType = "personal"; break;
				case "SAVINGS" : AccountType = "personal"; break;
				case "BUSINESS CHECKING" : AccountType = "business"; break;
				default : AccountType = "personal"; break;
			}

			transactionCommand.Append(              "      <CheckInfo>");
			transactionCommand.Append(String.Format("      <ABA>{0}</ABA>",eCheckBankABACode));
			transactionCommand.Append(String.Format("      <Account>{0}</Account>",eCheckBankAccountNumber));
			transactionCommand.Append(String.Format("      <AccountType>{0}</AccountType>",AccountType));
			transactionCommand.Append(String.Format("      <CheckMemo>{0} - Order# {1}</CheckMemo>",Common.AppConfig("StoreName"),OrderNumber));
			transactionCommand.Append(              "      </CheckInfo>");
      
			transactionCommand.Append(              "    </AccountInfo>");
			transactionCommand.Append(              "  </CustomerData>");

			transactionCommand.Append(              "  <TransactionData>");

			transactionCommand.Append(String.Format("      <VendorId>{0}</VendorId>",Common.AppConfig("ITransact.Vendor_ID").Trim()));
			transactionCommand.Append(String.Format("      <VendorPassword>{0}</VendorPassword>",Common.AppConfig("ITransact.Password").Trim()));
			transactionCommand.Append(String.Format("      <HomePage>{0}</HomePage>",Common.AppConfig("LiveServer").Trim()));
      
      
			// Email message text for the ITransact receipt since it's only a total.
			transactionCommand.Append(              "      <EmailText>");
			transactionCommand.Append(              "        <EmailTextItem>Detailed receipt will be sent in a separate email.</EmailTextItem>");
			transactionCommand.Append(              "      </EmailText>");

			transactionCommand.Append(              "    <OrderItems>");

			decimal productTotal = System.Decimal.Zero;

			foreach (CartItem item in cart._cartItems)
			{
				transactionCommand.Append(              "      <Item>");
				transactionCommand.Append(String.Format("        <Description>{0}</Description>",item.productName));
				transactionCommand.Append(String.Format("        <Cost>{0}</Cost>",Localization.CurrencyStringForGateway(item.price)));
				transactionCommand.Append(String.Format("        <Qty>{0}</Qty>",item.quantity));
				transactionCommand.Append(              "      </Item>");
				productTotal += (item.price * item.quantity);
			}

			productTotal += (cart.TaxTotal(true));
			productTotal += (cart.ShippingTotal(true));

			if (productTotal != OrderTotal)
			{
				transactionCommand.Append(              "      <Item>");
				transactionCommand.Append(              "        <Description>Discounts and Coupons - You saved</Description>");
				transactionCommand.Append(String.Format("        <Cost>{0}</Cost>",Localization.CurrencyStringForGateway(OrderTotal-productTotal)));
				transactionCommand.Append(              "        <Qty>1</Qty>");
				transactionCommand.Append(              "      </Item>");
			}

			if (cart.TaxTotal(true) != 0)
			{
				transactionCommand.Append(              "      <Item>");
				transactionCommand.Append(              "        <Description>Tax</Description>");
				transactionCommand.Append(String.Format("        <Cost>{0}</Cost>",Localization.CurrencyStringForGateway(cart.TaxTotal(true))));
				transactionCommand.Append(              "        <Qty>1</Qty>");
				transactionCommand.Append(              "      </Item>");
			}
      
			if (cart.ShippingTotal(true) != 0)
			{
				transactionCommand.Append(              "      <Item>");
				transactionCommand.Append(              "        <Description>Shipping</Description>");
				transactionCommand.Append(String.Format("        <Cost>{0}</Cost>",Localization.CurrencyStringForGateway(cart.ShippingTotal(true))));
				transactionCommand.Append(              "        <Qty>1</Qty>");
				transactionCommand.Append(              "      </Item>");
			}
      
			//     
			//      transactionCommand.Append(              "      <Item>");
			//      transactionCommand.Append(String.Format("        <Description>{0} - Order# {1}</Description>",Common.AppConfig("StoreName"),OrderNumber));
			//      transactionCommand.Append(String.Format("        <Cost>{0}</Cost>",Localization.CurrencyStringForGateway(OrderTotal)));
			//      transactionCommand.Append(              "        <Qty>1</Qty>");
			//      transactionCommand.Append(              "      </Item>");

			transactionCommand.Append(              "    </OrderItems>");

			transactionCommand.Append(              "  </TransactionData>");
			transactionCommand.Append(              "</SaleRequest>");

			// Is the command good XML
			XmlDocument cmdXml = new XmlDocument();
			try
			{
				cmdXml.LoadXml(transactionCommand.ToString());
			}
			catch
			{
			}

			//Have good Xml. Lets make it pretty
			transactionCommand.Length = 0; //Clear the builder
			transactionCommand.Append(Common.FormatXml(cmdXml));

			byte[]  data = encoding.GetBytes(transactionCommand.ToString());

			// Prepare web request...
			String AuthServer = Common.AppConfig("ITransact.Sale_Server"); // Use the Sale Transaction Server

			WebResponse myResponse;
			String rawResponseString = String.Empty;

			try
			{
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
				myResponse = myRequest.GetResponse();
				StreamReader sr = new StreamReader(myResponse.GetResponseStream());
				rawResponseString = sr.ReadToEnd();
				// Close and clean up the StreamReader
				sr.Close();
				//Close the Response
				myResponse.Close();
			}
			catch
			{
				result = "NO RESPONSE FROM GATEWAY!";
			}

			// rawResponseString now has gateway response
      
			AVSResult = String.Empty;

			String sql = String.Empty;
			String replyCode = String.Empty;
			String approvalCode = String.Empty;
			String authResponse = String.Empty;
			String TransID = String.Empty;
      
			XmlDocument responseXml = new XmlDocument();
      
			try
			{
				responseXml.LoadXml(rawResponseString);
				//Good Xml make it pretty
				rawResponseString = Common.FormatXml(responseXml);

				replyCode = responseXml.SelectSingleNode("//Status").InnerText;
				approvalCode = responseXml.SelectSingleNode("//AuthCode").InnerText;
				authResponse = responseXml.SelectSingleNode("//ErrorCategory").InnerText +" "+responseXml.SelectSingleNode("//ErrorMessage").InnerText;
				TransID = responseXml.SelectSingleNode("//XID").InnerText;
				AVSResult = responseXml.SelectSingleNode("//AVSResponse").InnerText;
			}
			catch
			{
				authResponse="GARBLED RESPONSE FROM THE GATEWAY";
			}

			PaymentCleared = false;
			AuthorizationCode = approvalCode;
			AuthorizationResult = rawResponseString;
			AuthorizationTransID = TransID;
			TransactionCommandOut = transactionCommand.ToString();
      
			if(replyCode.ToUpper() == "OK") // They sometimes return OK or ok so ignore case
			{
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
				sql = "insert into FailedTransaction(CustomerID,OrderDate,PaymentGateway,TransactionCommand,TransactionResult) values(" + cart._thisCustomer._customerID.ToString() + "," + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + "," + DB.SQuote("ITRANSACT") + "," + DB.SQuote(transactionCommand.ToString()) + "," + DB.SQuote(rawResponseString) + ")";
				DB.ExecuteSQL(sql);
			}
			return result;
		}

 
	}
}
