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

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for setssl.
	/// </summary>
	public class setssl : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			if(Common.AppConfigBool("UseSSL"))
			{
				if(Common.OnLiveServer() && Common.ServerVariables("SERVER_PORT_SECURE") != "1")
				{
					Response.Redirect("https://" + Common.ServerVariables("HTTP_HOST") + Common.GetThisPageName(true) + "?" + Common.ServerVariables("QUERY_STRING"));
				}
			}
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Welcome to " + Common.AppConfig("StoreName");
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(thisCustomer.IsAdminSuperUser)
			{
				if(Common.QueryString("set").ToLower() == "true")
				{
					Common.SetAppConfig("UseSSL","true",false);
					Common.SetAppConfig("RedirectLiveToWWW","true",false);
					writer.Write("SSL ON");
				}
				else
				{
					Common.SetAppConfig("UseSSL","false",false);
					Common.SetAppConfig("RedirectLiveToWWW","false",false);
					writer.Write("SSL OFF");
				}
				Common.ClearCache();
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
