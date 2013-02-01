// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// $Header: /v5.0/AspDotNetStorefront/engine.aspx.cs 4     4/07/05 11:06a Administrator $
// $Header: /v5.0/AspDotNetStorefront/engine.aspx.cs 4     4/07/05 11:06a Administrator $
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for Engine.
	/// </summary>
	public class Engine : SkinBase
	{

		XmlPackage p;

		private void Page_Load(object sender, System.EventArgs e)
		{
			p = new XmlPackage(Common.QueryString("XmlPackage"), ThisCustomer,SiteID);
			SectionTitle = p.SectionTitle;
		}
		
		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
      writer.Write(p.TransformString());
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
