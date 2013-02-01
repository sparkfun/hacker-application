// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for lostpassword.
	/// </summary>
	public class lostpassword : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
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
				bool ErrorDuringSend = false;
				try
				{
					Common.SendMail(Common.AppConfig("StoreName") + " Password","Your password is: " + PWD,false,FromEMail,FromEMail,EMail,EMail,"",Common.AppConfig("MailMe_Server"));
				}
				catch
				{
					ErrorDuringSend = true;
				}
				if(ErrorDuringSend)
				{
					Response.Redirect("signin.aspx?errormsg=" + Server.UrlEncode("Unable to send e-mail"));
				}
				Response.Redirect("signin.aspx?notemsg=Your+Password+Has+Been+Sent.");
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
