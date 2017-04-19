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
	/// Summary description for receipt.
	/// </summary>
	public class receipt : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Customer thisCustomer = new Customer(Common.QueryStringUSInt("CustomerID"),true);
			if(thisCustomer._isAnon)
			{
				// you must be logged in to view receipts:
				Response.Redirect("signin.aspx?returnurl=receipt.aspx?" + Server.UrlEncode(Common.ServerVariables("QUERY_STRING")));
			}

			Order ord = new Order(Common.QueryStringUSInt("OrderNumber"));
			if(ord._isEmpty)
			{
				// no order to show:
				Response.Redirect(SE.MakeDriverLink("ordernotfound"));
			}
			//if(thisCustomer._customerID != ord._customerID && !thisCustomer._isAdminUser)
			//{
			//	Response.Redirect(SE.MakeDriverLink("ordernotfound"));
			//}
			// ok to view:
			String PWD = Common.QueryString("PWD");
			bool nocc = (Common.QueryString("nocc").ToLower() == "true");
			Response.Write(ord.Receipt(thisCustomer._customerID,Common.GetActiveReceiptTemplate(ord._siteID),PWD,nocc));
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
