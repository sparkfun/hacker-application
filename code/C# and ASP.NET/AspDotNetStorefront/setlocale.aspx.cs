// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for setlocale.
	/// </summary>
	public class setlocale : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Set Locale";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			String LocaleSetting = Common.QueryString("LocaleSetting");
			if(LocaleSetting.Length == 0)
			{
				LocaleSetting = Localization.GetWebConfigLocale();
			}
			thisCustomer.SetLocale(LocaleSetting);

			writer.Write("<img src=\"../images/spacer.gif\" width=\"1\" height=\"100\"><br><b><center>");
			writer.Write("Setting Locale To: " + Common.GetLocaleSettingDescription(LocaleSetting) + "<br><br>Please wait...");
			writer.Write("<br><img src=\"../images/spacer.gif\" width=\"1\" height=\"100\"><br>");
			writer.Write("</center></b>");

			string ReturnURL = Common.QueryString("ReturnURL");
			if(ReturnURL.IndexOf("setlocale.aspx") != -1)
			{
				ReturnURL = String.Empty;
			}

			if (ReturnURL.Length == 0)
			{
				ReturnURL = "default.aspx";
			}
			Response.AddHeader("REFRESH","1; URL=" + Server.UrlDecode(ReturnURL));
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
