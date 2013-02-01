using System;
using System.Data;
using System.Web;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for splash.
	/// </summary>
	public class splash : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(thisCustomer._isAdminUser)
			{
				if(Common.AppConfigBool("Admin_ShowDefaultContents"))
				{
					writer.Write("Store Version: " + Common.AppConfig("StoreVersion") + "<br><br>");
				
					writer.Write("Active Locale Setting: " + thisCustomer._localeSetting + "<br>");
					writer.Write("Master Locale Setting: " + Localization.GetWebConfigLocale() + "<br><br>");
				
					writer.Write("Payment Gateway: " + Common.AppConfig("PaymentGateway") + "<br>");
					writer.Write("Gateway Mode: " + Common.IIF(Common.AppConfigBool("UseLiveTransactions"), "LIVE", "TEST") + "<br>");
					writer.Write("Transaction Mode: " + Common.AppConfig("TransactionMode") + "<br>");
					writer.Write("Payment Methods: " + Common.AppConfig("PaymentMethods") + "<br>");
					writer.Write(Common.AppConfig("MicroPay.Prompt") + " Enabled: " + Common.AppConfigBool("Micropay.Enabled").ToString() + "<br>");
					writer.Write("Cardinal Enabled: " + Common.AppConfigBool("CardinalCommerce.Centinel.Enabled").ToString() + "<br>");
					if(Common.AppConfigBool("CardinalCommerce.Centinel.Enabled"))
					{
						writer.Write("Cardinal Is Live: " + Common.AppConfigBool("CardinalCommerce.Centinel.IsLive").ToString() + "<br>");
					}
					writer.Write("<br>");
					writer.Write("DB Provider: " + DB.GetDBProvider() + "<br>");
				
					if(thisCustomer.IsAdminSuperUser)
					{
						writer.Write("DBConn: " + DB.GetDBConn() + "<br><br>");
					}
					else
					{
						writer.Write("<br>");
					}
				
					writer.Write("<a href=\"wizard.aspx\">Run Configuration Wizard</a> - This can help you set variables the first time you setup your store<br><br>");
			
					if(Common.AppConfigBool("UseSSL"))
					{
						writer.Write("<a href=\"setssl.aspx?set=false\">Turn SSL Off</a><br><br>");
					}
					else
					{
						writer.Write("<a href=\"setssl.aspx?set=true\">Turn SSL On</a> - Set your store to use SSL (You must have your SSL cert already installed)<br><br>");
					}

					writer.Write("<a href=\"appconfig.aspx?searchfor=mail\">E-Mail Settings</a><br><br>");

					writer.Write("<a href=\"appconfig.aspx?searchfor=" + Common.AppConfig("PaymentGateway") + "\">Gateway Settings</a><br>");

				}
				else
				{
					writer.Write("<img src=\"images/spacer.gif\" height=\"500\" width=\"1\">");
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
