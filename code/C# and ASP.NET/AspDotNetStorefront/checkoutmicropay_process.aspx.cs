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
	/// Summary description for checkoutmicropay_process.
	/// </summary>
	public class checkoutmicropay_process : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
      Response.CacheControl="private";
      Response.Expires=0;
      Response.AddHeader("pragma", "no-cache");
			
      Customer thisCustomer = new Customer();
      thisCustomer.RequireCustomerRecord();

      if(ShoppingCart.CartIsEmpty(thisCustomer._customerID,CartTypeEnum.ShoppingCart))
      {
        Response.Redirect("ShoppingCart.aspx");
      }

      ShoppingCart cart = new ShoppingCart(1,thisCustomer,CartTypeEnum.ShoppingCart,0,false);
      
      int OrderNumber = Common.GetNextOrderNumber();

			// try create the order record, check for status of TX though:
			String status = Gateway.MakeOrder(cart,"MICROPAY",String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,OrderNumber,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty);
			if(status != "OK")
			{
				Response.Redirect("checkoutmicropay.aspx?error=1&errormsg=" + Server.UrlEncode(status));
			}
			Response.Redirect("orderconfirmation.aspx?ordernumber=" + OrderNumber.ToString() + "&paymentmethod=MICROPAY");

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
