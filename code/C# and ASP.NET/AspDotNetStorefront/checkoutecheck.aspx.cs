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
	/// Summary description for checkoutecheck.
	/// </summary>
	public class checkoutecheck : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Order - eCheck Information:";
			RequireSecurePage();
			RequiresLogin(Common.GetThisPageName(false) + "?" + Common.ServerVariables("QUERY_STRING"));
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{

			Topic t = new Topic("CheckoutECheckPageHeader",thisCustomer._localeSetting,_siteID);
			writer.Write(t._contents);

			if(thisCustomer._isAdminUser)
			{
				writer.Write("<p><font color=\"blue\"><b>PAYMENT GATEWAY: " + Common.AppConfig("PaymentGateway") + "&nbsp;&nbsp;&nbsp;&nbsp;TRANSACTION MODE: " + Common.AppConfig("TransactionMode") + "&nbsp;&nbsp;&nbsp;&nbsp;GATEWAY MODE: " + Common.IIF(Common.AppConfigBool("UseLiveTransactions"), "LIVE", "TEST") + "</b></font></center></p>");
			}
			else
			{
				if(!Common.AppConfigBool("UseLiveTransactions"))
				{
					writer.Write("<p><font color=\"blue\"><b>NOTE: USING TEST TRANSACTIONS</b></font></center></p>");
				}
			}
			
			if(Common.QueryString("ErrorMsg").Length != 0)
			{
				writer.Write("<p><font class=\"error\"><b>UNABLE TO PROCESS TRANSACTION: " + Server.HtmlEncode(Common.QueryString("ErrorMsg")) + "</b></font></center></p>");
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

			//			writer.Write(Common.ReadFile("tip2.js",false));

			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function CheckOutECheckForm_Validator(theForm)\n");
			writer.Write("	{\n");
			writer.Write("	submitonce(theForm);\n");
			if(Common.AppConfigBool("RequireTermsAndConditionsAtCheckout"))
			{
				writer.Write("	if (!theForm.TermsAndConditionsRead.checked)\n");
				writer.Write("	{\n");
				writer.Write("		alert(\"Please indicate your acceptance of the Terms and Conditions before proceeding. If you need assistance, please contact us.\");\n");
				writer.Write("		theForm.TermsAndConditionsRead.focus();\n");
				writer.Write("		submitenabled(theForm);\n");
				writer.Write("		return (false);\n");
				writer.Write("    }\n");
			}
			writer.Write("	return (true);\n");
			writer.Write("	}\n");
			writer.Write("</script>\n");

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


			if( (Common.AppConfig("PaymentGateway").Trim().ToUpper() == "AUTHORIZENET" || Common.AppConfig("PaymentGateway").Trim().ToUpper() == "EPROCESSINGNETWORK" || Common.AppConfig("PaymentGateway").Trim().ToUpper() == "ITRANSACT") && Common.AppConfig("TransactionMode").Trim().ToUpper() == "AUTH CAPTURE")
			{

				writer.Write("<form method=\"POST\" action=\"checkoutecheck_process.aspx\" name=\"CheckOutECheckForm\" id=\"CheckOutECheckForm\" onsubmit=\"return (validateForm(this) && CheckOutECheckForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">");
				writer.Write("  <div align=\"left\">");

				writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
				writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
				writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/echeckinfo.gif\" border=\"0\"><br>");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
				writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
		
				writer.Write("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"100%\"><b>Please Enter Your E-Check Bank Account Information Below:</b></td>");
				writer.Write("      </tr>");
				writer.Write("      <tr><td width=\"100%\"><hr></td></tr>");
				writer.Write("      <tr><td width=\"100%\" height=\"10\"></td></tr>");
				writer.Write("    </table>");

				Address BillingAddress = new Address();
				BillingAddress.LoadByCustomer(thisCustomer._customerID,AddressTypes.Billing);
        
				//If blank assume the Accoutn First and Last name
				if (BillingAddress.ECheckBankAccountName.Length == 0)
				{
					BillingAddress.ECheckBankAccountName = String.Format("{0} {1}",thisCustomer._firstName,thisCustomer._lastName);
				}

				writer.Write(BillingAddress.InputECheckHTML(true));
        
      
				if(Common.AppConfigBool("RequireTermsAndConditionsAtCheckout"))
				{
					writer.Write("<table align=\"center\" width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" style=\"border-style: dashed; border-width: 1px; border-color: #000000; \"><tr><td class=\"LightCell\">");
					writer.Write(" <tr><td>");
					writer.Write("  <input type=\"checkbox\" name=\"TermsAndConditionsRead\">&nbsp;");
					Topic t2 = new Topic("checkouttermsandconditions",thisCustomer._localeSetting,_siteID);
					writer.Write(t2._contents);
					writer.Write("  </td></tr>\n");
					writer.Write("</table>");
					writer.Write(" <tr><td colspan=\"2\" height=\"25\"></td></tr>");
				}
				writer.Write("  <tr>");
				writer.Write("    <td width=\"100%\" colspan=\"2\" align=\"center\">");
				writer.Write("     <input type=\"submit\" value=\"Submit Order\" name=\"Continue\">");
				writer.Write("	  </td>");
				writer.Write("  </tr>");
				writer.Write("	<tr><td colspan=\"2\" height=\"15\"></td></tr>");
				writer.Write("  <tr>");
				writer.Write("    <td width=\"100%\" colspan=\"2\" align=\"center\"><p align=\"center\">");
				writer.Write("       <b>Review your order below to make sure it's correct.<br>Then click 'Submit Order' only once to place your order.</b></p>");
				writer.Write("      <p align=\"justify\">NOTE: When you submit this order, it may appear as if the server is not responding. The server will actually be processing your e-check payment in real time; the next page cannot be sent to you until the order is fully processed. The process normally takes about 30 seconds, but it may take longer during certain times of the day. To avoid double charge, please DO NOT abort your submission until you have received a final writer from the server.</p>");
				writer.Write("    </td>");
				writer.Write("  </tr>");
				writer.Write("</table>");
				writer.Write("  </tr>");
				writer.Write("</table>");
				writer.Write("</div>");
				writer.Write("</form>");
				writer.Write("");
				writer.Write("<br>");

				//				writer.Write("</td></tr>\n");
				//				writer.Write("</table>\n");
				//				writer.Write("</td></tr>\n");
				//				writer.Write("</table>\n");

				// ORDER SUMMARY:
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
			else
			{
				writer.Write("<b><font color=red>ECHECKS ARE ONLY SUPPORTED WITH AUTHORIZE.NET or ITRANSACT GATEWAY IN AUTH CAPTURE MODE</font></b>");
			}

			Topic t3 = new Topic("CheckoutECheckPageFooter",thisCustomer._localeSetting,_siteID);
			writer.Write(t3._contents);
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
