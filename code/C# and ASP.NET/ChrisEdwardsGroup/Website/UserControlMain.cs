using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Quiksoft.EasyMail.SMTP;

namespace ChrisEdwardsGroup.Website
{
	/// <summary>
	/// Summary description for UserControlMain.
	/// </summary>
	public class UserControlMain : UserControl
	{
		public string GetSqlExceptionDump(SqlException Ex)
		{
			StringBuilder sb = new StringBuilder("\n");
			sb.Append("Database Error Details:" + "\n");
			sb.Append(" Class: " + Ex.Class + "\n");
			sb.Append(" Error Number: " + Ex.Number + "\n");
			sb.Append(" Errors: " + Ex.Errors + "\n");
			sb.Append(" Help Link: " + Ex.HelpLink + "\n");
			sb.Append(" Inner Exception: " + Ex.InnerException + "\n");
			sb.Append(" Line Number: " + Ex.LineNumber + "\n");
			sb.Append(" Message: " + Ex.Message + "\n");
			sb.Append(" Procedure: " + Ex.Procedure + "\n");
			sb.Append(" Server: " + Ex.Server + "\n");
			sb.Append(" Source: " + Ex.Source + "\n");
			sb.Append(" Stack Trace: " + Ex.StackTrace + "\n");
			sb.Append(" State: " + Ex.State + "\n");
			sb.Append(" Target Site: " + Ex.TargetSite + "\n");

			return sb.ToString();
		}

		public StringBuilder CreateBulletList(CheckBoxList cbl)
		{
			// Create new string builder to hold item list
			StringBuilder sb = new StringBuilder("");
			
			// Start unordered list
			sb.Append("<ul>" + "\r");

			// Add list items to unordered list
			for (int i = 0; i < cbl.Items.Count; i++)
			{
				if (cbl.Items[i].Selected)
				{
					sb.Append("<li>" +
						cbl.Items[i].Value + 
						"</li>" + "\r");
				}
			}

			// End unordered list
			sb.Append("</ul>" + "\r");

			// Return string builder
			return sb;
		}

		public SMTPServer GetMailServer()
		{
			// Create the smtp mail server account with login information
			SMTPServer mailServ = new SMTPServer();
			mailServ.AuthMode = SMTPAuthMode.AuthLogin;
			mailServ.Account = "webmaster";
			mailServ.Password = "firestar1";
			mailServ.Name = "mail.chrisedwardsgroup.com";
			return mailServ;
		}

		public string GetDbConnectionString()
		{
		    if (ConfigurationSettings.AppSettings != null) return ConfigurationSettings.AppSettings["SqlConnectString"];
		    return "";
		}
	}
}