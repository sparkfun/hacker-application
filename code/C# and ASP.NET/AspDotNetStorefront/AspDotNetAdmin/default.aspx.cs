// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Web;
using System.Text;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for _default.
	/// </summary>
	public class _default : SkinBase
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(Common.ServerVariables("HTTP_HOST").ToLower().IndexOf(Common.AppConfig("LiveServer").ToLower()) != -1 && Common.ServerVariables("HTTP_HOST").ToLower().IndexOf("www") == -1)
			{
				if(Common.AppConfigBool("RedirectLiveToWWW"))
				{
					Response.Redirect("http://www." + Common.AppConfig("LiveServer").ToLower() + "/" + Common.GetAdminDir() + "/default.aspx?" + Common.ServerVariables("QUERY_STRING"));
				}
			}
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			base.RequireSecurePage();
			if(Common.AppConfigBool("ShowTreeInAdmin"))
			{
				base.SetTemplate("main_with_tree.htm");
			}
			else
			{
				base.SetTemplate("main.htm");
			}
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
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
