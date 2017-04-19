using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroupAdmin
{
	/// <summary>
	/// Summary description for TopLogin.
	/// </summary>
	public class TopLogin : UserControl
	{
		public Label lblUser;

		public TopLogin()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public void Page_Load(Object source, EventArgs e)
		{
			if (Page.User.Identity.IsAuthenticated)
			{
				lblUser.Text = Page.User.Identity.Name.ToString();
			}
			else
			{
				// Sign Out User
				FormsAuthentication.SignOut();

				// Redirect to Login Page
				Response.Clear();
				Response.Redirect(Request.UrlReferrer.ToString());
			}
		}

		public void DoSignOut(Object source, EventArgs e)
		{
			FormsAuthentication.SignOut();

			Response.Clear();
			Response.Redirect(Request.UrlReferrer.ToString());
		}
	}
}
