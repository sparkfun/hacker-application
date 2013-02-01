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
	/// Summary description for techtalk.
	/// </summary>
	public class techtalk : SkinBase
	{
		String Contents;
		String fn;
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			fn = Server.MapPath("skins/Skin_" + _siteID.ToString() + "/" + Common.QueryString("Topic") + ".htm");
			Contents = Common.ReadFile(fn,true);
			Contents = Contents.Replace("%SITEID%",_siteID.ToString());
			SectionTitle = "Tech Talk"; //Common.ExtractToken(Contents,"<title>","</title>");
			if(SectionTitle.Length == 0)
			{
				SectionTitle = Server.HtmlEncode(Common.QueryString("topic")).ToUpper();
			}
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(Contents.Length == 0)
			{
				writer.Write("<img src=\"images/spacer.gif\" border=\"0\" height=\"100\" width=\"1\"><br>\n");
				writer.Write("<p align=\"center\"><font class=\"big\"><b>This page is currently empty. Please check back again for an update.</b></font></p>");
			}
			else
			{	
				writer.Write("\n");
				writer.Write("<!-- READ FROM FILE: " + fn + " -->");
				writer.Write("\n");
				writer.Write(Common.ExtractBody(Contents));
				writer.Write("\n");
				writer.Write("<!-- END OF FILE: " + fn + " -->");
				writer.Write("\n");
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
