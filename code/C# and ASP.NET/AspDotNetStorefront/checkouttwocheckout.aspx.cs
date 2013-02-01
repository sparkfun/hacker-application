// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using AspDotNetStorefrontCommon;
using AspDotNetStorefrontGateways;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for checkouttwocheckout.
	/// </summary>
	public class checkouttwocheckout : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad();
			SectionTitle = "Order - 2Checkout:";
			RequireSecurePage();
			RequiresLogin(Common.GetThisPageName(false) + "?" + Common.ServerVariables("QUERY_STRING"));
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write("\n");

			ShoppingCart cart = new ShoppingCart(_siteID,thisCustomer,CartTypeEnum.ShoppingCart,0,false);
			// shouldn't have gotten to this point if their cart is empty, so check to make sure:
			if(cart.IsEmpty())
			{
				Response.Redirect("ShoppingCart.aspx");
			}

			// ** UPDATE SHIPPING INFO, ADDRESSES COULD HAVE CHANGED!
			Common.ShippingCalculationEnum ShipCalcID = Common.GetActiveShippingCalculationID();
			int ShippingMethodID = thisCustomer.ShippingMethodID;
			String ShippingMethod = String.Empty;
			if(ShipCalcID == Common.ShippingCalculationEnum.UseRealTimeRates)
			{
				ShippingMethodID = 0;
				cart.RecheckShippingRates(); // recalc shipping rates, because user is going to checkout, if going back to cart, no need to do this, as cart does it on display after load
				ShippingMethod = cart._shippingMethod;
			}
			else if(ShipCalcID == Common.ShippingCalculationEnum.CalculateShippingByWeightAndZone)
			{
				ShippingMethodID = 0;
				ShippingMethod = "Ship To Zone";
			}
			else
			{
				ShippingMethod = Common.GetShippingMethodName(ShippingMethodID);
			}
			// ** END UPDATE SHIPPING INFO, ADDRESSES COULD HAVE CHANGED!

			// this is an undocumented and unsupported feature:
			if(Common.AppConfigBool("ShortCircuitCheckoutOn0DollarOrder") && cart.Total(true) == System.Decimal.Zero)
			{
				// try create the order record, check for status of TX though:
				int OrderNumber = Common.GetNextOrderNumber();
				String status = Gateway.MakeOrder(cart,"Bypass Gateway",String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,OrderNumber,String.Empty, String.Empty, String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty);
				if(status != "OK")
				{
					Response.Redirect("ShoppingCart.aspx?errormsg=" + Server.UrlEncode(status));
				}
				Response.Redirect("orderconfirmation.aspx?ordernumber=" + OrderNumber.ToString() + "&paymentmethod=Credit+Card");
			}
			// end undocumented and unsupported feature

			bool useLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
			
			writer.Write("<font color=blue size=2><b>Click the button shown below to go to 2Checkout and make your payment. AFTER you make your payment on the 2Checkout site, you must click their &quot;Continue&quot; button. You will see this &quot;Continue&quot; button on the 2Checkout site after you have made your payment. Clicking on that &quot;Continue&quot; button will then bring you back here so we can complete your order!</b></font><br><br>");
			writer.Write("<div align=\"center\">\n");
			writer.Write("<br>Click below to go to 2Checkout and complete your purchase:<br><br>\n");
			writer.Write("<form target=\"_top\" action=\"" + Common.AppConfig("2CHECKOUT_Live_Server") + "\" method=\"post\">\n");

			writer.Write("<input type=\"hidden\" name=\"x_login\" value=\"" + Common.AppConfig("2CHECKOUT_VendorID") + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"x_amount\" value=\"" + Localization.CurrencyStringForGateway((cart.Total(true))) + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"x_invoice_num\" value=\"" + Common.GetNewGUID() + "\">\n"); // yuk...we don't know what the order nubmer will be...
			writer.Write("<input type=\"hidden\" name=\"x_receipt_link_url\" value=\"" + Common.GetStoreHTTPLocation(true) + "twocheckout_return.aspx\">\n");
			writer.Write("<input type=\"hidden\" name=\"x_return_url\" value=\"" + Common.GetStoreHTTPLocation(true) + "twocheckout_return.aspx\">\n");
			writer.Write("<input type=\"hidden\" name=\"x_return\" value=\"" + Common.GetStoreHTTPLocation(true) + "twocheckout_return.aspx\">\n");
			if(!useLiveTransactions)
			{
				writer.Write("<input type=\"hidden\" name=\"demo\" value=\"Y\">\n");
			}
			writer.Write("<input type=\"hidden\" name=\"x_First_Name\" value=\"" + cart._billingAddress.FirstName + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"x_Last_Name\" value=\"" + cart._billingAddress.LastName + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"x_Address\" value=\"" + cart._billingAddress.Address1 + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"x_City\" value=\"" + cart._billingAddress.City + "\">\n"); // 2checkout docs are unclear as to the name of this parm
			writer.Write("<input type=\"hidden\" name=\"x_State\" value=\"" + cart._billingAddress.State + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"x_Zip\" value=\"" + cart._billingAddress.Zip + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"x_Country\" value=\"" + cart._billingAddress.Country + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"x_Email\" value=\"" + cart._billingAddress.Email + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"x_Phone\" value=\"" + cart._billingAddress.Phone + "\">\n");

			writer.Write("<input type=\"hidden\" name=\"city\" value=\"" + cart._billingAddress.City + "\">\n");
			writer.Write("&nbsp;&nbsp;<input type=\"submit\" value=\"Make payments with 2Checkout\">\n");
			writer.Write("</form>\n");
			writer.Write("</div>\n");

			// ORDER SUMMARY:
			writer.Write("<b>Review your order below to make sure it's correct.</b></p>");
			writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/orderinfo.gif\" align=\"absbottom\" border=\"0\"> To edit your order, <a href=\"ShoppingCart.aspx\">click here</a>.<br>");
			writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

			writer.Write("\n");
			writer.Write(cart.Display(true,_siteID,thisCustomer._isAnon));
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");

			cart = null;
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
