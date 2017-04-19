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
	/// Summary description for lat_lostpassword.
	/// </summary>
	public class lat_lostpassword : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");

			String Message = String.Empty;
			String PWD = String.Empty;

			//String FromName = "Support";
			String FromEMail = Common.AppConfig("AffiliateEMailAddress");
			String EMail = Common.Form("EMail");
			String sql = String.Empty;

			if(EMail.Length != 0 )
			{
				sql = "select email,[password] from Affiliate  " + DB.GetNoLock() + " where deleted=0 and email=" + Common.SQuote(EMail.ToLower());
			}

			IDataReader rs = DB.GetRS(sql);
			if(rs.Read())
			{
				PWD = DB.RSField(rs,"Password");
			}
			rs.Close();

			if(PWD.Length != 0)
			{
				try
				{
				Common.SendMail("" + Common.AppConfig("AffiliateProgramName") + " Password","Your " + Common.AppConfig("AffiliateProgramName") + " account password is: " + PWD,false,FromEMail,FromEMail,EMail,EMail,"",Common.AppConfig("MailMe_Server"));
				Response.Redirect("lat_signin.aspx?notemsg=" + Server.UrlEncode("Your " + Common.AppConfig("AffiliateProgramName") + " Account Password Has Been Sent."));
				}
				catch
				{
					Response.Redirect("signin.aspx?errormsg=" + Server.UrlEncode("Unable to send e-mail"));
				}
			}
			else
			{
				if(EMail.Length != 0)
				{
					Message = "There is no registered " + Common.AppConfig("AffiliateProgramName") + " member with that e-mail address!";
				}
				Response.Redirect("lat_signin.aspx?errormsg=" + Server.UrlEncode(Message));
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
