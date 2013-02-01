// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using AspDotNetStorefrontCommon;
using AspDotNetStorefrontGateways;


namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for cardinal_process.
	/// </summary>
	public class cardinal_process : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");
			Response.Cache.SetAllowResponseInBrowserHistory(false);

			Customer thisCustomer = new Customer();
			thisCustomer.RequireCustomerRecord();

			int CustomerID = thisCustomer._customerID;
			CustomerSession sess = new CustomerSession(CustomerID);

			String Payload = sess.Session("Cardinal.Payload");
			String PaRes = Common.Form("PaRes");
			String TransactionID = sess.Session("Cardinal.TransactionID");
			int OrderNumber = sess.SessionUSInt("Cardinal.OrderNumber");

			String ReturnURL = String.Empty;

			if(ShoppingCart.CartIsEmpty(CustomerID,CartTypeEnum.ShoppingCart))
			{
				ReturnURL = "ShoppingCart.aspx";
			}

			if(ReturnURL.Length == 0)
			{
				if(OrderNumber == 0)
				{
					ReturnURL = "checkoutcard.aspx?error=1&errormsg=" + Server.UrlEncode("Session Expired. Please retry credit card entry");
				}
			}

			if(ReturnURL.Length == 0)
			{
				if(Payload.Length == 0 || TransactionID.Length == 0)
				{
					ReturnURL = "checkoutcard.aspx?error=1&errormsg=" + Server.UrlEncode("Bank verification was incomplete or canceled. Please retry credit card entry");
				}
			}

			String PAResStatus = String.Empty;
			String SignatureVerification = String.Empty;
			String ErrorNo = String.Empty;
			String ErrorDesc = String.Empty;

			if(ReturnURL.Length == 0)
			{
				String CardinalAuthenticateResult = String.Empty;
				String AuthResult = Cardinal.PreChargeAuthenticate(OrderNumber,PaRes,TransactionID,out PAResStatus, out SignatureVerification, out ErrorNo, out ErrorDesc, out CardinalAuthenticateResult);
				sess.SetVal("CardinalAuthenticateResult",CardinalAuthenticateResult,System.DateTime.Now.AddMinutes(Common.SessionTimeout()));

				//=====================================================================================
				// Determine if the Authentication was Successful or Error
				//
				// Please consult the documentation regarding the handling of each response scenario.
				//
				// If the Authentication results (PAResStatus) is a Y or A, and the SignatureVerification is Y, then
				// the Payer Authentication was successful. The Authorization Message should be processed,
				// and the User taken to a Order Confirmation location.
				//
				// If the Authentication results were not successful (PAResStatus = N), or
				// the ErrorNo was NOT //0// then the Consumer should be redirected, and prompted for another
				// form of payment.
				//
				// If the Authentication results were not successful (PAResStatus = U) and the ErrorNo = //0//
				// then authorization message should be processed. In this case the merchant will retain
				// liability for this transaction if it is sent to authorization.
				//
				// Note that it is also important that you account for cases when your flow logic can account
				// for error cases, and the flow can be broken after //N// number of attempts
				//=====================================================================================

				// handle success cases:
				if(((PAResStatus == "Y" || PAResStatus == "A") && SignatureVerification == "Y") || (PAResStatus == "U" && ErrorNo == "0"))
				{
					ShoppingCart cart = new ShoppingCart(1,thisCustomer,CartTypeEnum.ShoppingCart,0,false);

					// GET CAVV FROM authenticate call result:
					String CAVV = Common.ExtractToken(sess.Session("Cardinal.AuthenticateResult"),"<Cavv>","</Cavv>");
					String ECI = Common.ExtractToken(sess.Session("Cardinal.AuthenticateResult"),"<EciFlag>","</EciFlag>");
					String XID = Common.ExtractToken(sess.Session("Cardinal.AuthenticateResult"),"<Xid>","</Xid>");

					// use cardnumber from session (becauase StoreCCInDB may be false, cannot rely on it being in the order table)
					String status = Gateway.MakeOrder(cart,"Credit Card",String.Empty,sess.Session("CardName"),sess.Session("CardType"),Common.UnmungeString(sess.Session("CardNumber")),sess.Session("CardExtraCode"),sess.Session("CardExpirationMonth"),sess.Session("CardExpirationYear"),OrderNumber, CAVV, ECI, XID,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty);
					if(status != "OK")
					{
						ReturnURL = "checkoutcard.aspx?error=1&errormsg=" + status;
					}
					else
					{
						// store cardinal call results for posterity:
						DB.ExecuteSQL("update orders set CardinalLookupResult=" + DB.SQuote(sess.Session("CardinalLookupResult")) + ", CardinalAuthenticateResult=" + DB.SQuote(sess.Session("CardinalAuthenticateResult")) + " where OrderNumber=" + OrderNumber.ToString());
						ReturnURL = "orderconfirmation.aspx?ordernumber=" + OrderNumber.ToString() + "&paymentmethod=Credit+Card";
					}
				}

				// handle failure:
				if(PAResStatus == "N" || ErrorNo != "0")
				{
					ReturnURL = "checkoutcard.aspx?error=1&errormsg=" + Server.UrlEncode("We were unable to verify your credit card. Please retry your credit card or choose a different payment type.");
				}


				// handle failure:
				if(PAResStatus == "N" || ErrorNo != "0")
				{
					ReturnURL = "checkoutcard.aspx?error=1&errormsg=" + Server.UrlEncode("We were unable to verify your credit card. Please retry your credit card or choose a different payment type.");
				}
			}

			if(ReturnURL.Length == 0)
			{
				ReturnURL = "checkoutcard.aspx?error=1&errormsg=" + Server.UrlEncode("Unknown Result. Message=" + ErrorDesc + ". Please retry your credit card.");
			}
			sess = null;

			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");
			Response.Write("<html><head><title>Cardinal Process</title></head><body>");
			Response.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			Response.Write("top.location='" + ReturnURL + "';\n");
			Response.Write("</SCRIPT>\n");
			Response.Write("<div align=\"center\">If this page does not refresh automatically, <a href=\"" + ReturnURL + "\" target=\"_top\">Click Here</a> to finish your order</div>");
			Response.Write("</body></html>");


		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
