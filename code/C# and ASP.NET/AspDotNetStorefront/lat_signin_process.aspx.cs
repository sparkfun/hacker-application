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
	/// Summary description for lat_signin_process.
	/// </summary>
	public class lat_signin_process : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			base._disableLeftAndRightCols = true; // page gets two wide
			SectionTitle = "<a href=\"lat_account.aspx\">" + Common.AppConfig("AffiliateProgramName") + "</a> Sign In";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			String EMailField = Common.Form("EMail").ToLower();
			String PasswordField = Common.Form("Password");

			//SEC4
			//Try it both ways
			bool ClearLogin = false;
			bool EncryptLogin = false;
        
			IDataReader rs = DB.GetRS(String.Format("select * from affiliate {0} where deleted=0 and email={1} and [password]={2}",DB.GetNoLock(),DB.SQuote(EMailField),DB.SQuote(PasswordField)));
			ClearLogin = rs.Read();
			if (!ClearLogin)
			{
				rs = DB.GetRS(String.Format("select * from affiliate {0} where deleted=0 and email={1} and [password]={2}",DB.GetNoLock(),DB.SQuote(EMailField),DB.SQuote(Common.MungeString(PasswordField))));
				EncryptLogin = rs.Read();  
			}

			if (ClearLogin && Common.AppConfigBool("EncryptPassword"))
			{
				//Logged in but the password was not encrypted so encrypt it.
				DB.ExecuteSQL(String.Format("update affiliate set [password]={0} where deleted=0 and email={1}",DB.SQuote(Common.MungeString(PasswordField)),DB.SQuote(EMailField)));
			}

			if (EncryptLogin && (!Common.AppConfigBool("EncryptPassword")))
			{
				//Logged in but encrypted when it shouldn't be so update to clear.
				DB.ExecuteSQL(String.Format("update affiliate set [password]={0} where deleted=0 and email={1}",DB.SQuote(PasswordField),DB.SQuote(EMailField)));
			}

			if (!(ClearLogin || EncryptLogin))
			{
				rs.Close();
				Response.Redirect("lat_signin.aspx?errormsg=Invalid+Login");
			}

			int AffiliateID = DB.RSFieldInt(rs,"AffiliateID");

			// we've got a good login:
			Common.SetSessionCookie("LATAffiliateID",AffiliateID.ToString());
			
			String AffiliateGUID = DB.RSFieldGUID(rs,"AffiliateGUID");
			rs.Close();

			writer.Write("<img src=\"../images/spacer.gif\" width=\"1\" height=\"100\"><br><b><center>");
			writer.Write(Common.AppConfig("AffiliateProgramName") + " sign-in complete, please wait...");
			writer.Write("<br><img src=\"../images/spacer.gif\" width=\"1\" height=\"100\"><br>");
			writer.Write("</center></b>");

			String ReturnURL = Server.HtmlDecode(Common.Form("ReturnURL"));
			if(ReturnURL.Length == 0)
			{
				ReturnURL = "lat_account.aspx";
			}
			Response.AddHeader("REFRESH","1; URL=" + Server.UrlDecode(ReturnURL));

			//writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			//writer.Write("self.location='" + Server.HtmlEncode(Server.UrlEncode(ReturnURL)) + "';\n");
			//writer.Write("</SCRIPT>\n");
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
