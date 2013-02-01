// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using AspDotNetStorefrontCommon;
using AspDotNetStorefrontGateways;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for worldpayreturn.
	/// </summary>
	public class worldpayreturn : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");

			if(Common.Form("TransStatus") != "Y")
			{
				String ReturnURL = Common.GetStoreHTTPLocation(true) + "ShoppingCart.aspx";
				if(Common.AppConfigBool("WorldPay_OnCancelAutoRedirectToCart"))
				{
					Response.AddHeader("REFRESH","1; URL=" + ReturnURL);
				}
				Response.Write("<html><head><title>WorldPay Checkout Canceled - Please Wait</title></head><body>");
				if(!Common.AppConfigBool("WorldPay_OnCancelAutoRedirectToCart"))
				{
					Topic t = new Topic("WorldPayCancel");
					Response.Write(t._contents);
					Response.Write("<p align=\"left\"><b>Your order checkout was canceled. <a href=\"" + ReturnURL + "\">Click here to return to your " + Common.AppConfig("CartPrompt").ToLower() + "</a></b></p>");
				}
				if(Common.AppConfigBool("WorldPay_OnCancelAutoRedirectToCart"))
				{
					Response.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
					Response.Write("self.location='" + ReturnURL + "';\n");
					Response.Write("</SCRIPT>\n");
				}
				Response.Write("</body></html>");
			}
			else
			{
				int CustomerID = Common.FormNativeInt("CartID");
				Customer thisCustomer = new Customer(CustomerID,true);
				
				// need these later in processcard, don't like passing via session, but it should be safe, and is easiest thing to do
				// worldpay structure requires this, so it can work like our other payment gateways
				CustomerSession sess = new CustomerSession(CustomerID);
				sess.SetVal("WorldPay_CartID",Common.Form("CartID"),System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
				sess.SetVal("WorldPay_TransID",Common.Form("TransID"),System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
				sess.SetVal("WorldPay_FuturePayID",Common.Form("FuturePayID"),System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
				sess.SetVal("WorldPay_TransStatus",Common.Form("TransStatus"),System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
				sess.SetVal("WorldPay_TransTime",Common.Form("TransTime"),System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
				sess.SetVal("WorldPay_AuthAmount",Common.Form("AuthAmount"),System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
				sess.SetVal("WorldPay_AuthCurrency",Common.Form("AuthCurrency"),System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
				sess.SetVal("WorldPay_RawAuthMessage",Common.Form("RawAuthMessage"),System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
				sess.SetVal("WorldPay_RawAuthCode",Common.Form("RawAuthCode"),System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
				sess.SetVal("WorldPay_CallbackPW",Common.Form("CallbackPW"),System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
				sess.SetVal("WorldPay_CardType",Common.Form("CardType"),System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
				sess.SetVal("WorldPay_CountryMatch",Common.Form("CountryMatch"),System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
				sess.SetVal("WorldPay_AVS",Common.Form("AVS"),System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
				sess = null;
				
				if(CustomerID != 0)
				{

					// MakeOrder ALWAYS Returns OK, because WorldPay will never return without a C for cancel or Y for success, and the C was handled above

					int OrderNumber = Common.GetNextOrderNumber();
					ShoppingCart cart = new ShoppingCart(1,thisCustomer,CartTypeEnum.ShoppingCart,0,false);
					String Msg = String.Empty;
					try
					{
						String status = Gateway.MakeOrder(cart,"Credit Card",String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,OrderNumber,String.Empty,String.Empty, String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty);
					}
					catch (Exception ex)
					{
						Msg = Common.GetExceptionDetail(ex,"<br>");
					}
				
					String ReturnURL = Common.GetStoreHTTPLocation(true) + "orderconfirmation.aspx?ordernumber=" + OrderNumber.ToString() + "&paymentmethod=Credit+Card";
					
					Response.AddHeader("REFRESH","1; URL=" + ReturnURL);
					Response.Write("<html><head><title>WorldPay Checkout Successful - Please Wait</title></head><body>");
					if(Msg.Length != 0)
					{
						Response.Write("ERROR: " + Msg + "<br><br>");
					}
					Topic t = new Topic("WorldPaySuccess",thisCustomer._localeSetting);
					Response.Write(t._contents);
					Response.Write("<p align=\"left\"><b>Your payment was successful. <a href=\"" + ReturnURL + "\">Click here to COMPLETE YOUR ORDER.</a></b></p>");
					Response.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
					Response.Write("self.location='" + ReturnURL + "';\n");
					Response.Write("</SCRIPT>\n");
					Response.Write("</body></html>");
				}
				else
				{
					Response.Write("<html><head><title>WorldPay Checkout Errort</title></head><body>");
					Response.Write("No Customer ID Returned");
					Response.Write("</body></html>");
				}
			}
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
