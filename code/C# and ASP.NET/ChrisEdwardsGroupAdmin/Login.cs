using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroupAdmin
{
	/// <summary>
	/// Summary description for Login.
	/// </summary>
	public class Login : Main
	{
		public TextBox txtUserName;
		public TextBox txtPassword;
		public Label lblError;

		public Login()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public void Page_Load(Object Source, EventArgs E)
		{

		}

		public void DoLogin(Object source, EventArgs e)
		{
			string userName = txtUserName.Text.ToString();
			string password = txtPassword.Text.ToString();
			bool blnCookie = false;

			if (FormsAuthentication.Authenticate(userName, password))
			{
				FormsAuthentication.RedirectFromLoginPage(userName, blnCookie);
			}
			else
			{
				lblError.Visible = true;
				lblError.Text = "* You must enter a valid UserName and Password.";
			}
		}
	}
}
