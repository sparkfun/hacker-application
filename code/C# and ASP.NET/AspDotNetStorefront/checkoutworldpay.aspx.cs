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

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for checkoutworldpay.
	/// </summary>
	public class checkoutworldpay : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Order - WorldPay:";
			RequireSecurePage();
			RequiresLogin(Common.GetThisPageName(false) + "?" + Common.ServerVariables("QUERY_STRING"));
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(Common.QueryString("ErrorMsg").Length != 0)
			{
				writer.Write("<p><b><font class=\"error\"><b>UNABLE TO PROCESS TRANSACTION: " + Server.HtmlEncode(Common.QueryString("ErrorMsg")) + "</b></font></center></p>");
			}
			
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

			writer.Write("<div align=\"center\">\n");
			writer.Write("<br>Click on the WorldPay button below to go to the WorldPay site and complete your purchase. After you finish making the payment, you will be returned back to this site.<br><br>\n");
			writer.Write("<form action=\"" + Common.AppConfig("WorldPay_Live_Server") + "\" method=\"post\">\n");
			writer.Write("<input type=\"hidden\" name=\"instId\" value=\"" + Common.AppConfig("WorldPay_InstallationID") + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"cartId\" value=\"" + cart._thisCustomer._customerID.ToString() + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"amount\" value=\"" + Localization.CurrencyStringForGateway((cart.Total(true))) + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"currency\" value=\"" + Localization.StoreCurrency() + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"desc\" value=\"" + Common.AppConfig("StoreName") + " Purchase\">\n");
			writer.Write("<input type=\"hidden\" name=\"MC_callback\" value=\"" + Common.GetStoreHTTPLocation(true) + Common.AppConfig("WorldPay_ReturnURL") + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"authMode\" value=\"" + Common.IIF(Common.TransactionMode() == "AUTH" , "E" , "A") + "\">\n");
			
			writer.Write("<input type=\"hidden\" name=\"name\" value=\"" + (cart._billingAddress.FirstName + " " + cart._billingAddress.LastName) + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"address\" value=\"" + cart._billingAddress.Address1 + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"postcode\" value=\"" + cart._billingAddress.Zip + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"country\" value=\"" + Common.GetCountryTwoLetterISOCode(cart._billingAddress.Country) + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"tel\" value=\"" + cart._billingAddress.Phone + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"email\" value=\"" + cart._billingAddress.Email + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"lang\" value=\"" + Common.AppConfig("WorldPay_LanguageLocale") + "\">\n");

			if(Common.AppConfigBool("WorldPay_FixContact"))
			{
				writer.Write("<input type=\"hidden\" name=\"fixContact\" value=\"true\">\n");
			}

			if(Common.AppConfigBool("WorldPay_HideContact"))
			{
				writer.Write("<input type=\"hidden\" name=\"hideContact\" value=\"true\">\n");
			}

			if(Common.AppConfigBool("WorldPay_TestMode"))
			{
				writer.Write("<input type=\"hidden\" name=\"testMode\" value=\"" + Common.AppConfig("WorldPay_TestModeCode") + "\">\n");
			}


			writer.Write("&nbsp;&nbsp;<input align=\"absmiddle\" type=\"image\" src=\"images/worldpay.gif\" border=\"0\" name=\"submit\" alt=\"Make payment via WorldPay!\">\n");
			writer.Write("</form>\n");
			writer.Write("<font color=red size=4><b>After you go to the WorldPay site (by clicking on the button above), you must click the \"Complete Your Order\" link that we provide on the WorldPay site. You will do this after you have made your WorldPay payment. Clicking on the \"Complete Your Order\" button will then bring you back here so we can complete your order and issue your receipt!</b></font><br><br>");
			writer.Write("</div>\n");

			// ORDER SUMMARY:
			writer.Write("<b>Review your order below to make sure it's correct.<br>Then click on the WorldPay button above.</b></p>");
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
