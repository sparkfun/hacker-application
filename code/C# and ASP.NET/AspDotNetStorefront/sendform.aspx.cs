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
	/// Summary description for sendform.
	/// </summary>
	public class sendform : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Contacting Us";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			try
			{
				// send form to store administrator:
				String FormContents = Common.GetFormInput(true,"<br>");
				Common.SendMail("Contact Inquiry From Website", FormContents + Common.AppConfig("MailFooter"), true, Common.AppConfig("GotOrderEMailFrom"),Common.AppConfig("GotOrderEMailFromName"),Common.AppConfig("GotOrderEMailTo"),Common.AppConfig("GotOrderEMailTo"),"",Common.AppConfig("MailMe_Server"));
				writer.Write("<p align=\"left\">Thanks.<br><br>Your message has been received, and someone will contact you shortly.</p>");
			}
			catch(Exception ex)
			{
				writer.Write("<p align=\"left\"><font color=red><b>Unable to send message, reason: " + Common.GetExceptionDetail(ex,"<br>") + "</b></font></p>");
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
