// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for lostpassword.
	/// </summary>
	public class lostpassword : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");
			
			Customer thisCustomer = new Customer();

			String Message = String.Empty;
			String PWD = String.Empty;

			//String FromName = "Support";
			String FromEMail = Common.AppConfig("MailMe_FromAddress");
			String EMail = Common.Form("EMail");
			String sql = String.Empty;

			if(EMail.Length != 0 )
			{
				sql = "select email,[password] from customer  " + DB.GetNoLock() + " where deleted=0 and email=" + DB.SQuote(EMail.ToLower());
			}

			IDataReader rs = DB.GetRS(sql);
			if(rs.Read())
			{
				//SEC4
				PWD = DB.RSField(rs,"Password");
				if (Common.AppConfigBool("EncryptPassword"))
				{
					PWD = Common.UnmungeString(PWD);  
					if (PWD.StartsWith("Error")) //bad decryption may be in clear
					{
						PWD = DB.RSField(rs,"Password");
					}
				}
			}
			rs.Close();

			if(PWD.Length != 0)
			{
				try
				{
					Common.SendMail(Common.AppConfig("StoreName") + " Password","Your password is: " + PWD,false,FromEMail,FromEMail,EMail,EMail,"",Common.AppConfig("MailMe_Server"));
					Response.Redirect("signin.aspx?notemsg=Your+Password+Has+Been+Sent.");
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
					Message = "There is no registered user with that e-mail address!";
				}
				Response.Redirect("signin.aspx?errormsg=" + Server.UrlEncode(Message));
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
