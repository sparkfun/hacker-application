// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Web.Mail;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for notification.
	/// </summary>
	public class notification : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");

			int OrderNumber = Common.QueryStringUSInt("OrderNumber");
			int CustomerID = Common.QueryStringUSInt("CustomerID");

			Response.Write("<html><head><title>" + Common.AppConfig("StoreName") + " Order Notification, Order #" + OrderNumber.ToString() + "</title></head><body>");
			Response.Write("<b>" + Common.AppConfig("StoreName") + " Order Notification, Order #" + OrderNumber.ToString() + "</b><br>");
			Response.Write("<b>Order Date: " + Localization.ToNativeDateTimeString(DateTime.Now) + "<br><br>");

			String ReceiptURL = Common.IIF(Common.AppConfigBool("UseSSL") , "https://" , "http://") + Common.ServerVariables("HTTP_HOST") + "/receipt.aspx?ordernumber=" + OrderNumber.ToString() + "&customerid=" + CustomerID.ToString();
			String XMLURL = Common.IIF(Common.AppConfigBool("UseSSL") , "https://" , "http://") + Common.ServerVariables("HTTP_HOST") + "/" + Common.AppConfig("AdminDir") + "/orderXML.aspx?ordernumber=" + OrderNumber.ToString() + "&customerid=" + CustomerID.ToString();

			Response.Write("For Order Receipt: <a href=\"" + ReceiptURL + "\">click here</a><br>");
			Response.Write("For Order XML: <a href=\"" + XMLURL + "\">click here</a><br>");
			Response.Write("</body></html>");

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
