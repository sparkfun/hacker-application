// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for Driver.
	/// </summary>
	public class Driver : SkinBase
	{

		Topic t;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			t = new Topic(Common.QueryString("topic"),thisCustomer._localeSetting,_siteID);
			SectionTitle = t._sectionTitle;
		}
		
		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(t._pwd.Length != 0)
			{
				if(Common.Form("Password") == t._pwd)
				{
					Session["Topic" + t._topicName] = "VALIDATED";
				}
			}

			if(t._pwd.Length != 0 && Common.Session("Topic" + t._topicName) != "VALIDATED")
			{
				writer.Write("<form method=\"POST\" action=\"driver.aspx?topic=" + Server.UrlEncode(t._topicName) + "\">\n");
				writer.Write("  <p><b>Validation is required to view this topic.</b></p>\n");
				writer.Write("  <p>Please enter the topic password: <input type=\"text\" name=\"Password\" size=\"20\" maxlength=\"100\"><input type=\"submit\" value=\"Submit\" name=\"B1\"></p>\n");
				writer.Write("</form>\n");
			}
			else
			{

				if(t._contents.Length == 0)
				{
					writer.Write("<img src=\"images/spacer.gif\" border=\"0\" height=\"100\" width=\"1\"><br>\n");
					writer.Write("<p align=\"center\"><font class=\"big\"><b>This page is currently empty. Please check back again for an update.</b></font></p>");
				}
				else
				{	
					writer.Write("\n");
					writer.Write("<!-- READ FROM " + Common.IIF(t._fromDB , "DB", "FILE: " + t._fn) + ": " + " -->");
					writer.Write("\n");
					writer.Write(t._contents);
					writer.Write("\n");
					writer.Write("<!-- END OF " + Common.IIF(t._fromDB , "DB", "FILE: " + t._fn) + ": " + " -->");
					writer.Write("\n");
				}
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
