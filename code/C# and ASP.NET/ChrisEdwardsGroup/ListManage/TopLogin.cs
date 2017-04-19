using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroup.ListManage
{
	/// <summary>
	/// Summary description for TopLogin.
	/// </summary>
	public class TopLogin : UserControl
	{
		public Label lblUser;

		public void Page_Load(Object sender, EventArgs e)
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

		public void DoSignOut(Object sender, EventArgs e)
		{
			FormsAuthentication.SignOut();

			Response.Clear();
			Response.Redirect(Request.UrlReferrer.ToString());
		}
	}
}