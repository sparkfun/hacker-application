using System;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroup.ListManage
{
	/// <summary>
	/// Summary description for Login.
	/// </summary>
	public class Login : Main
	{
		public TextBox txtUserName;
		public TextBox txtPassword;
		public Label lblError;

		public void DoLogin(Object sender, EventArgs e)
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