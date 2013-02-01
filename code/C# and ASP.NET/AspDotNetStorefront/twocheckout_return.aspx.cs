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
	/// Summary description for twocheckout_return.
	/// </summary>
	public class twocheckout_return : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");

			if(Common.Form("credit_card_processed") == "Y" || Common.Form("credit_card_processed") == "K")
			{
				Customer thisCustomer = new Customer();
				ShoppingCart cart = new ShoppingCart(1,thisCustomer,CartTypeEnum.ShoppingCart,0,false);
				int OrderNumber = Common.GetNextOrderNumber();
				String status = Gateway.MakeOrder(cart,"2CHECKOUT",String.Empty,String.Empty,"2CHECKOUT",String.Empty,String.Empty,String.Empty,String.Empty,OrderNumber,String.Empty,String.Empty, String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty);
				Response.Redirect("orderconfirmation.aspx?ordernumber=" + OrderNumber.ToString() + "&paymentmethod=Credit+Card");
			}
			
			// error or not approved:
			Response.Redirect("shoppingcart.aspx?errormsg=" + Server.UrlEncode("Billing was canceled or not approved. Please retry your order."));
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
