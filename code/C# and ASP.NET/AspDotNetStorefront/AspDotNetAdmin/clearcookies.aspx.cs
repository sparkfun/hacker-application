// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Data;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for clearcookies.
	/// </summary>
	public class clearcookies : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");
			Response.Write("<html><head><title>Clear Cookies</title>");
			Response.Write("</head><body bgcolor=\"FFFFFF\">");
			for(int i = 0; i < Request.Cookies.Count; i++)
			{
				String cookie = Request.Cookies.Keys[i];
				Response.Write("Clearing cookie \"" + cookie + "\"...<br>");
				Response.Cookies[cookie].Expires = System.DateTime.Now.AddDays(-1);
			}

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
