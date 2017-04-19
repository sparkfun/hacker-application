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
	/// Summary description for custombid.
	/// </summary>
	public class custombid : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "THANK YOU";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			String Body = "From: " + Common.Form("Name") + "<br>Organization: " + Common.Form("Organization") + "<br>E-Mail: " + Common.Form("Email") + "<br>Phone: " + Common.Form("Phone") + "<br><br>Request:<br><br>" + Common.Form("Summary").Replace("\n","<br>");
			try
			{
				Common.SendMail("AspDotNetStorefront: Custom Bid Request",Body,true,Common.AppConfig("MailMe_FromAddress"),Common.AppConfig("MailMe_FromName"),Common.AppConfig("MailMe_ToAddress"),Common.AppConfig("MailMe_ToName"),"",Common.AppConfig("MailMe_Server"));
			}
			catch {}
			writer.Write("<br><br><br><blockquote><b>Your custom bid request has been received.</b><br><br>Someone will contact you shortly! Thanks for the opportunity to earn your business.</blockquote>");
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
