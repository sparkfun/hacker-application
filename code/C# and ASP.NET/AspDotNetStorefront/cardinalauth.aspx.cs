using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for cardinalauth.
	/// </summary>
	public class cardinalauth : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");
			Customer thisCustomer = new Customer();
			thisCustomer.RequireCustomerRecord();
			
			//=====================================================================================\n");
			//= Easy Connect - Cardinal Commerce (http://www.cardinalcommerce.com)\n");
			//= ecauth.aspx\n");
			//=\n");
			//= Usage\n");
			//=		Form used to POST the payer authentication request to the Card Issuer Servers.\n");
			//=		The Card Issuer Servers will in turn display the payer authentication window\n");
			//=		to the consumer within this location.\n");
			//=\n");
			//=		Note that the form field names below are case sensitive. For additional information\n");
			//=		please see the integration documentation.\n");
			//=\n");
			//=====================================================================================\n");
			Response.Cache.SetAllowResponseInBrowserHistory(false);
			int CustomerID = thisCustomer._customerID;
			CustomerSession sess = new CustomerSession(CustomerID);
			
			if(sess.Session("Cardinal.ACSURL").Length == 0)
			{
				Response.Write("<HTML>\n");
				Response.Write("<BODY>\n");
				Response.Write("<center>Your session has expired. Please go to the shopping cart page to retry your checkout.</center>\n");
				Response.Write("</BODY>\n");
				Response.Write("</HTML>\n");
			}
			else
			{
				Response.Write("<HTML>\n");
				Response.Write("<BODY onLoad=\"document.frmLaunchACS.submit();\">\n");
				Response.Write("<center>\n");
				//=====================================================================================\n");
				// The Inline Authentication window must be a minimum of 410 pixel width by\n");
				// 400 pixel height.\n");
				//=====================================================================================\n");
				Response.Write("<FORM name=\"frmLaunchACS\" method=\"Post\" action=\"" + sess.Session("Cardinal.ACSURL") + "\">\n");
				Response.Write("<noscript>\n");
				Response.Write("	<br><br>\n");
				Response.Write("	<center>\n");
				Response.Write("	<font color=\"red\">\n");
				Response.Write("	<h1>Processing your Payer Authentication Transaction</h1>\n");
				Response.Write("	<h2>JavaScript is currently disabled or is not supported by your browser.<br></h2>\n");
				Response.Write("	<h3>Please click Submit to continue the processing of your transaction.</h3>\n");
				Response.Write("	</font>\n");
				Response.Write("	<input type=\"submit\" value=\"Submit\">\n");
				Response.Write("	</center>\n");
				Response.Write("</noscript>\n");
				Response.Write("<input type=hidden name=\"PaReq\" value=\"" + sess.Session("Cardinal.Payload") + "\">\n");
				Response.Write("<input type=hidden name=\"TermUrl\" value=\"" + Common.GetStoreHTTPLocation(true) + Common.AppConfig("CardinalCommerce.Centinel.TermURL") + "\">\n");
				Response.Write("<input type=hidden name=\"MD\" value=\"None\">\n");
				Response.Write("</FORM>\n");
				Response.Write("</center>\n");
				Response.Write("</BODY>\n");
				Response.Write("</HTML>\n");
			}
			sess = null;
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
