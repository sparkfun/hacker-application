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
	/// Summary description for checkoutpaypal.
	/// </summary>
	public class checkoutpaypal : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Order - PayPal:";
			RequireSecurePage();
			RequiresLogin(Common.GetThisPageName(false) + "?" + Common.ServerVariables("QUERY_STRING"));
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			//			IDataReader rs = DB.GetRS("select * from customer where customerid=" + thisCustomer._customerID.ToString());
			//			rs.Read();
			//
			//      rs.Close();
      
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


			writer.Write("<font color=blue size=2><b>Click the button shown below to go to PayPal and make your payment. AFTER you make your payment on the PayPal site, you must click their \"Continue\" button. You will see this \"Continue\" button on the PayPal after you have made your payment. Clicking on that \"Continue\" button will then bring you back here so we can complete your order!</b></font><br><br>");
			writer.Write("<div align=\"center\">\n");
			writer.Write("<br>Click below to go to PayPal and complete your purchase:<br><br>\n");
			writer.Write("<form target=\"_top\" action=\"" + Common.AppConfig("PayPal_Live_Server") + "\" method=\"post\">\n");
			writer.Write("<input type=\"hidden\" name=\"cmd\" value=\"_ext-enter\">\n");
			writer.Write("<input type=\"hidden\" name=\"redirect_cmd\" value=\"_xclick\">\n");
			writer.Write("<input type=\"hidden\" name=\"business\" value=\"" + Common.AppConfig("PayPal_Business") + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"item_name\" value=\"" + Common.AppConfig("StoreName") + " Purchase\">\n");
			writer.Write("<input type=\"hidden\" name=\"item_number\" value=\"" + ((CartItem)cart._cartItems[0]).SKU + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"amount\" value=\"" + Localization.CurrencyStringForGateway((cart.Total(true))) + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"rm\" value=\"2\">\n");
			writer.Write("<input type=\"hidden\" name=\"return\" value=\"" + Common.GetStoreHTTPLocation(true) + Common.AppConfig("PayPal_ReturnOKURL") + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"cancel_return\" value=\"" + Common.GetStoreHTTPLocation(true) + Common.AppConfig("PayPal_ReturnCancelURL") + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"no_note\" value=\"1\">\n");
			writer.Write("<input type=\"hidden\" name=\"quantity\" value=\"1\">\n"); // always set to one. we are sending the order Total to PayPal
			writer.Write("<input type=\"hidden\" name=\"cs\" value=\"1\">\n");
			writer.Write("<input type=\"hidden\" name=\"custom\" value=\"" + thisCustomer._customerID.ToString() + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"currency_code\" value=\"" + Localization.StoreCurrency() + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"lc\" value=\"US\">\n");
			writer.Write("<input type=\"hidden\" name=\"first_name\" value=\"" + cart._billingAddress.FirstName + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"last_name\" value=\"" + cart._billingAddress.LastName + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"address1\" value=\"" + cart._billingAddress.Address1 + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"address2\" value=\"" + cart._billingAddress.Address2 + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"city\" value=\"" + cart._billingAddress.City + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"state\" value=\"" + cart._billingAddress.State + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"zip\" value=\"" + cart._billingAddress.Zip + "\">\n");
			try
			{
				String ph = Common.MakeProperPhoneFormat(cart._billingAddress.Phone);
				writer.Write("<input type=\"hidden\" name=\"night_phone_a\" value=\"" + ph.Substring(1,3) + "\">\n");
				writer.Write("<input type=\"hidden\" name=\"night_phone_b\" value=\"" + ph.Substring(4,3) + "\">\n");
				writer.Write("<input type=\"hidden\" name=\"night_phone_c\" value=\"" + ph.Substring(7,4) + "\">\n");
				writer.Write("<input type=\"hidden\" name=\"day_phone_a\" value=\"" + ph.Substring(1,3) + "\">\n");
				writer.Write("<input type=\"hidden\" name=\"day_phone_b\" value=\"" + ph.Substring(4,3) + "\">\n");
				writer.Write("<input type=\"hidden\" name=\"day_phone_c\" value=\"" + ph.Substring(7,4) + "\">\n");
			}
			catch {}
			writer.Write("&nbsp;&nbsp;<input align=\"absmiddle\" type=\"image\" src=\"skins/Skin_" + _siteID.ToString() + "/images/paypal.gif\" border=\"0\" name=\"submit\" alt=\"Make payments with PayPal - it's fast, free and secure!\">\n");
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
