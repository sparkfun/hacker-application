// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for lat_signout.
	/// </summary>
	public class lat_signout : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			base._disableLeftAndRightCols = true; // page gets two wide
			SectionTitle = Common.AppConfig("AffiliateProgramName") + " Signout";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			Common.SetSessionCookie("LATAffiliateID",String.Empty);
			writer.Write("<img src=\"../images/spacer.gif\" width=\"1\" height=\"100\"><br><b><center>");
			writer.Write(Common.AppConfig("AffiliateProgramName") + " sign-out complete, please wait...");
			writer.Write("<br><img src=\"../images/spacer.gif\" width=\"1\" height=\"100\"><br>");
			writer.Write("</center></b>");
			Response.AddHeader("REFRESH","1; URL=t-affiliate.aspx");
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
