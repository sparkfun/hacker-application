using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroupAdmin
{
	/// <summary>
	/// Summary description for PasswordHash.
	/// </summary>
	public class PasswordHash : Page
	{
		public RadioButtonList rblPasswordFormat;
		public TextBox txtPassword;
		public HtmlGenericControl outMessage;

		public PasswordHash()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public void Create_Hash(Object source, EventArgs e)
		{
			string hashString, formatString, passwordString;

			// get the selected format
			formatString = rblPasswordFormat.SelectedItem.Value.ToString();
			// get the password
			passwordString = txtPassword.Text.ToString();

			// create hash
			hashString = FormsAuthentication.HashPasswordForStoringInConfigFile(passwordString, formatString);

			// display the result
			outMessage.InnerHtml = formatString + " Password Hash is: " + hashString;

		}
	}
}
