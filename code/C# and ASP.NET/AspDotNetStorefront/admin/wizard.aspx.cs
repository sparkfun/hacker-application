// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for wizard.
	/// </summary>
	public class wizard : SkinBase
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Configuration Wizard";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(!thisCustomer.IsAdminSuperUser)
			{
				writer.Write("Insufficient Permission!");
			}
			else
			{

				if(Common.Form("IsSubmit") == "true")
				{
					// save the config settings:
					Common.SetAppConfig("RTShipping.OriginZip",Common.Form("OriginZip"),false);
					Common.SetAppConfig("LiveServer",Common.Form("LiveServer"),false);
					Common.SetAppConfig("MailMe_FromAddress",Common.Form("StoreEMail"),false);
					Common.SetAppConfig("MailMe_FromName",Common.Form("StoreEMailName"),false);
					Common.SetAppConfig("MailMe_ToAddress",Common.Form("StoreEMail"),false);
					Common.SetAppConfig("MailMe_ToName",Common.Form("StoreEMailName"),false);
					Common.SetAppConfig("MailMe_Server","mail." + Common.Form("LiveServer"),false);
					Common.SetAppConfig("GotOrderEMailFrom",Common.Form("StoreEMail"),false);
					Common.SetAppConfig("GotOrderEMailFromName",Common.Form("StoreEMailName"),false);
					Common.SetAppConfig("GotOrderEMailTo",Common.Form("StoreEMail"),false);
					Common.SetAppConfig("ReceiptEMailFrom",Common.Form("StoreEMail"),false);
					Common.SetAppConfig("ReceiptEMailFromName",Common.Form("StoreEMailName"),false);
					
					Common.SetAppConfig("UseSSL",Common.Form("UseSSL"),false);
					Common.SetAppConfig("UseLiveTransactions",Common.Form("UseLiveTransactions"),false);

					Common.SetAppConfig("TransactionMode",Common.Form("TransactionMode"),false);

					Common.SetAppConfig("Localization.StoreCurrency",Common.Form("StoreCurrency"),false);
					Common.SetAppConfig("Localization.StoreCurrencyNumericCode",Common.Form("StoreCurrencyNumericCode"),false);

					Common.SetAppConfig("StoreName",Common.Form("StoreName"),false);
					Common.SetAppConfig("SE_MetaTitle",Common.Form("StoreName"),false);
					Common.SetAppConfig("Dispatch_SiteName",Common.Form("StoreName"),false);

					Common.SetAppConfig("PaymentMethods",Common.Form("PaymentMethods"),false);
					Common.SetAppConfig("PaymentGateway",Common.Form("PaymentGateway"),false);

					if(Common.AppConfig("EncryptKey") == "WIZARD")
					{
						Common.SetAppConfig("EncryptKey",Common.GetRandomNumber(1000,1000000).ToString() + Common.GetRandomNumber(1000,1000000).ToString() + Common.GetRandomNumber(1000,1000000).ToString(),false);
					}

					if(Common.AppConfig("OrderShowCCPwd") == "WIZARD")
					{
						Common.SetAppConfig("OrderShowCCPwd",Common.GetRandomNumber(1000,1000000).ToString() + Common.GetRandomNumber(1000,1000000).ToString() + Common.GetRandomNumber(1000,1000000).ToString(),false);
					}

					Common.SetAppConfig("WizardRun","true",false);

					Common.ClearCache();
					Response.Redirect("wizard.aspx"); // to make sure new login is now visible

				}

				IDataReader rs = DB.GetRS("select * from Customer where Customerid=" + thisCustomer._customerID.ToString());
				rs.Read();

				if(Common.QueryString("msg").Length > 0)
				{
					writer.Write("<p align=\"left\"><b><font color=\"blue\">The Account Information Has Been Updated</font></b></p>");
				}

				if(Common.QueryString("errormsg").Length > 0)
				{
					writer.Write("<p align=\"left\"><b><font color=\"red\">" + Server.HtmlEncode(Common.QueryString("ErrorMsg")) + "</font></b></p>");
				}

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function WizardForm_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("  return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				String act = "wizard.aspx";
				writer.Write("<form method=\"POST\" action=\"" + act + "\" name=\"WizardForm\" id=\"WizardForm\")\" onSubmit=\"return (validateForm(this) && WizardForm_Validator(this))\">");
				writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">");

				writer.Write("    <table border=\"0\" cellpadding=\"4\" cellspacing=\"0\" width=\"100%\">");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"100%\" colspan=\"2\"><b>This wizard can help you configure your store's primary configuration variables after first installation.<br><br></b></td>");
				writer.Write("      </tr>");

				writer.Write("      <tr>");
				writer.Write("        <td  align=\"right\">*Store Name: </td>");
				writer.Write("        <td >");
				writer.Write("        <input type=\"text\" name=\"StoreName\" size=\"37\" maxlength=\"100\" value=\"" + Common.AppConfig("StoreName") + "\"> (required, Enter the name of your store, e.g. ACME Widgets)");
				writer.Write("        <input type=\"hidden\" name=\"StoreName_vldt\" value=\"[req][blankalert=Please enter the name of your store, e.g. ACME Widgets]\">\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");

				writer.Write("      <tr>");
				writer.Write("        <td  align=\"right\">*Store Domain: </td>");
				writer.Write("        <td >");
				writer.Write("        <input type=\"text\" name=\"LiveServer\" size=\"37\" maxlength=\"100\" value=\"" + Common.AppConfig("LiveServer") + "\"> (required, enter the domain name of the production store site, with no www, e.g. yourdomain.com)");
				writer.Write("        <input type=\"hidden\" name=\"LiveServer_vldt\" value=\"[req][blankalert=Please enter the domain that your live store will be running on, e.g. yourstoredomain.com]\">\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");

				writer.Write("      <tr>");
				writer.Write("        <td  align=\"right\">*Store E-Mail Address: </td>");
				writer.Write("        <td >");
				writer.Write("        <input type=\"text\" name=\"StoreEMail\" size=\"37\" maxlength=\"100\" value=\"" + Common.AppConfig("MailMe_FromAddress") + "\"> (required, enter the e-mail address that the store should use to send order receipts, e.g. sales@yourdomain.com)");
				writer.Write("        <input type=\"hidden\" name=\"StoreEMail_vldt\" value=\"[req][blankalert=Please enter the store e-mail address, e.g. sales@yourdomain.com]\">\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");

				writer.Write("      <tr>");
				writer.Write("        <td  align=\"right\">*Store E-Mail Name: </td>");
				writer.Write("        <td >");
				writer.Write("        <input type=\"text\" name=\"StoreEMailName\" size=\"37\" maxlength=\"100\" value=\"" + Common.AppConfig("MailMe_FromName") + "\"> (required, enter the friendly name for the store e-mail address, e.g. Sales)");
				writer.Write("        <input type=\"hidden\" name=\"StoreEMailName_vldt\" value=\"[req][blankalert=Please enter the store e-mail name, e.g. Sales]\">\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");
			
				writer.Write("      <tr>");
				writer.Write("        <td  align=\"right\">Store Zip Code: </td>");
				writer.Write("        <td >");
				writer.Write("        <input type=\"text\" name=\"OriginZip\" size=\"6\" maxlength=\"5\" value=\"" + Common.AppConfig("RTShipping.OriginZip") + "\"> (if using Real Time Shipping, the store needs to know your shipment source zip code)");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				
				writer.Write("      <tr>");
				writer.Write("        <td  align=\"right\">Transaction Mode: </td>");
				writer.Write("        <td >");
				writer.Write("        <select size=\"1\" name=\"TransactionMode\"><option " + Common.IIF(Common.TransactionMode() == "AUTH"," selected ","") + ">AUTH</option><option" + Common.IIF(Common.TransactionMode() == "AUTH CAPTURE"," selected ","") + ">AUTH CAPTURE</option></select> (AUTH = Authorize Orders Only. AUTH CAPTURE = Authorize AND Capture Orders in Real Time)");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				
				writer.Write("      <tr>");
				writer.Write("        <td  align=\"right\">Store Currency: </td>");
				writer.Write("        <td >");
				writer.Write("        <input type=\"text\" name=\"StoreCurrency\" size=\"3\" maxlength=\"3\" value=\"" + Localization.StoreCurrency() + "\"> (Your store master currency, this is the ISO 4217 Standard Code, e.g. USD)");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				
				writer.Write("      <tr>");
				writer.Write("        <td  align=\"right\">Store Currency Numeric Code: </td>");
				writer.Write("        <td >");
				writer.Write("        <input type=\"text\" name=\"StoreCurrencyNumericCode\" size=\"4\" maxlength=\"4\" value=\"" + Localization.StoreCurrencyNumericCode() + "\"> (Your store master currency numeric code, this is the ISO 4217 Standard Numeric Code, e.g. 840)");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				
				writer.Write("      <tr>");
				writer.Write("        <td valign=\"top\" align=\"right\">Payment Methods Accepted: </td>");
				writer.Write("        <td >");
				writer.Write("Credit Card: <input type=\"checkbox\" name=\"PaymentMethods\" value=\"Credit Card\"" + Common.IIF(Common.AppConfig("PaymentMethods").ToUpper().IndexOf("CREDIT CARD") != -1," checked ","") + "><br>\n");
				writer.Write("PayPal: <input type=\"checkbox\" name=\"PaymentMethods\" value=\"PayPal\"" + Common.IIF(Common.AppConfig("PaymentMethods").ToUpper().IndexOf("PAYPAL") != -1," checked ","") + "><br>\n");
				writer.Write("Request For Quotes: <input type=\"checkbox\" name=\"PaymentMethods\" value=\"Request Quote\"" + Common.IIF(Common.AppConfig("PaymentMethods").ToUpper().IndexOf("REQUEST QUOTE") != -1," checked ","") + "><br>\n");
				writer.Write("Purchase Orders: <input type=\"checkbox\" name=\"PaymentMethods\" value=\"Purchase Order\"" + Common.IIF(Common.AppConfig("PaymentMethods").ToUpper().IndexOf("PURCHASE ORDER") != -1," checked ","") + "><br>\n");
				writer.Write("Checks: <input type=\"checkbox\" name=\"PaymentMethods\" value=\"Check\"" + Common.IIF(Common.AppConfig("PaymentMethods").ToUpper().IndexOf("CHECK") != -1," checked ","") + "><br>\n");
				writer.Write("eChecks: <input type=\"checkbox\" name=\"PaymentMethods\" value=\"eCheck\"" + Common.IIF(Common.AppConfig("PaymentMethods").ToUpper().IndexOf("ECHECK") != -1," checked ","") + ">\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				
				writer.Write("      <tr>");
				writer.Write("        <td valign=\"top\" align=\"right\">Payment Gateway: </td>");
				writer.Write("        <td >");
				writer.Write("MANUAL: <input type=\"radio\" name=\"PaymentGateway\" value=\"MANUAL\"" + Common.IIF(Common.AppConfig("PaymentGateway").ToUpper().IndexOf("MANUAL") != -1," checked ","") + "> (No gateway, cards are not charged in real time)<br>\n");
				writer.Write("Authorize.net: <input type=\"radio\" name=\"PaymentGateway\" value=\"AUTHORIZENET\"" + Common.IIF(Common.AppConfig("PaymentGateway").ToUpper().IndexOf("AUTHORIZENET") != -1," checked ","") + "><br>\n");
				writer.Write("2Checkout: <input type=\"radio\" name=\"PaymentGateway\" value=\"2CHECKOUT\"" + Common.IIF(Common.AppConfig("PaymentGateway").ToUpper().IndexOf("2CHECKOUT") != -1," checked ","") + "><br>\n");
				writer.Write("eProcessingNetwork: <input type=\"radio\" name=\"PaymentGateway\" value=\"EPROCESSINGNETWORK\"" + Common.IIF(Common.AppConfig("PaymentGateway").ToUpper().IndexOf("EPROCESSINGNETWORK") != -1," checked ","") + "><br>\n");
				writer.Write("Cybersource: <input type=\"radio\" name=\"PaymentGateway\" value=\"CYBERSOURCE\"" + Common.IIF(Common.AppConfig("PaymentGateway").ToUpper().IndexOf("CYBERSOURCE") != -1," checked ","") + "><br>\n");
				writer.Write("EFSNET: <input type=\"radio\" name=\"PaymentGateway\" value=\"EFSNET\"" + Common.IIF(Common.AppConfig("PaymentGateway").ToUpper().IndexOf("EFSNET") != -1," checked ","") + "><br>\n");
				writer.Write("Iongate: <input type=\"radio\" name=\"PaymentGateway\" value=\"IONGATE\"" + Common.IIF(Common.AppConfig("PaymentGateway").ToUpper().IndexOf("IONGATE") != -1," checked ","") + "><br>\n");
				writer.Write("ITransact: <input type=\"radio\" name=\"PaymentGateway\" value=\"ITRANSACT\"" + Common.IIF(Common.AppConfig("PaymentGateway").ToUpper().IndexOf("ITRANSACT") != -1," checked ","") + "><br>\n");
				writer.Write("Linkpoint: <input type=\"radio\" name=\"PaymentGateway\" value=\"LINKPOINT\"" + Common.IIF(Common.AppConfig("PaymentGateway").ToUpper().IndexOf("LINKPOINT") != -1," checked ","") + "><br>\n");
				writer.Write("NetBilling: <input type=\"radio\" name=\"PaymentGateway\" value=\"NETBILLING\"" + Common.IIF(Common.AppConfig("PaymentGateway").ToUpper().IndexOf("NETBILLING") != -1," checked ","") + "><br>\n");
				writer.Write("Paymentech: <input type=\"radio\" name=\"PaymentGateway\" value=\"PAYMENTECH\"" + Common.IIF(Common.AppConfig("PaymentGateway").ToUpper().IndexOf("PAYMENTECH") != -1," checked ","") + "><br>\n");
				writer.Write("PayFuse: <input type=\"radio\" name=\"PaymentGateway\" value=\"PAYFUSE\"" + Common.IIF(Common.AppConfig("PaymentGateway").ToUpper().IndexOf("PAYFUSE") != -1," checked ","") + "><br>\n");
				writer.Write("PayPal: <input type=\"radio\" name=\"PaymentGateway\" value=\"PAYPAL\"" + Common.IIF(Common.AppConfig("PaymentGateway").ToUpper().IndexOf("PAYPAL") != -1," checked ","") + "><br>\n");
				writer.Write("Transaction Central: <input type=\"radio\" name=\"PaymentGateway\" value=\"TRANSACTIONCENTRAL\"" + Common.IIF(Common.AppConfig("PaymentGateway").ToUpper().IndexOf("TRANSACTIONCENTRAL") != -1," checked ","") + "> (same as MerchantAnywhere)<br>\n");
				writer.Write("Verisign PayFlo PRO: <input type=\"radio\" name=\"PaymentGateway\" value=\"VERISIGN\"" + Common.IIF(Common.AppConfig("PaymentGateway").ToUpper().IndexOf("VERISIGN") != -1," checked ","") + "><br>\n");
				writer.Write("YourPay: <input type=\"radio\" name=\"PaymentGateway\" value=\"YOURPAY\"" + Common.IIF(Common.AppConfig("PaymentGateway").ToUpper().IndexOf("YOURPAY") != -1," checked ","") + "><br>\n");
				writer.Write("WORLDPAY JUNIOR: <input type=\"radio\" name=\"PaymentGateway\" value=\"WORLDPAY JUNIOR\"" + Common.IIF(Common.AppConfig("PaymentGateway").ToUpper().IndexOf("WORLDPAY JUNIOR") != -1," checked ","") + "><br>\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");

				writer.Write("      <tr>");
				writer.Write("        <td valign=\"middle\" align=\"right\">Use Live Transactions: </td>");
				writer.Write("        <td >");
				writer.Write("No: <input type=\"radio\" name=\"UseLiveTransactions\" value=\"false\"" + Common.IIF(!Common.AppConfigBool("UseLiveTransactions"), " checked ","") + "> (Store Test Mode)<img src=\"images/spacer.gif\" width=\"50\" height=\"1\">\n");
				writer.Write("Yes: <input type=\"radio\" name=\"UseLiveTransactions\" value=\"true\"" + Common.IIF(Common.AppConfigBool("UseLiveTransactions"), " checked ","") + "> (Live Mode)<br>\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");

				writer.Write("      <tr>");
				writer.Write("        <td valign=\"middle\" align=\"right\">Use SSL: </td>");
				writer.Write("        <td >");
				writer.Write("No: <input type=\"radio\" name=\"UseSSL\" value=\"false\"" + Common.IIF(!Common.AppConfigBool("UseSSL"), " checked ","") + "> (No SSL)<img src=\"images/spacer.gif\" width=\"50\" height=\"1\">\n");
				writer.Write("Yes: <input type=\"radio\" name=\"UseSSL\" value=\"true\"" + Common.IIF(Common.AppConfigBool("UseSSL"), " checked ","") + "> (You MUST have your SSL cert installed BEFORE checking this yes!!)<br>\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");

				writer.Write("</table>\n");
				writer.Write("<br><p align=\"center\"><input type=\"submit\" value=\"Update Settings\" name=\"Continue\"></p>");
				writer.Write("</form>");

				rs.Close();
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
