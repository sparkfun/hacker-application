using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Quiksoft.EasyMail.SMTP;

namespace StopNSave.Website
{
	/// <summary>
	/// Summary description for Main.
	/// </summary>
	public class Main : Page
	{
		public static SmtpServerStatus ServerStatus;
		
		static Main()
		{
			// Set the server status to live or testing
			// so that e-mail forms can be tested correctly.
			string serverStatus = ConfigurationSettings.AppSettings["ServerStatus"];
			if (serverStatus == "Live")
			{
				// Set server status to live
				ServerStatus = SmtpServerStatus.Live;
			}
			else
			{
				// Set server status to testing
				ServerStatus = SmtpServerStatus.Testing;
			}
		}
		
		public enum SmtpServerStatus
		{
			Testing,
			Live
		}

		protected enum FormType
		{
			HomeSearch = 1,
			ContactForm = 2
		}

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

		protected bool IsEmpty(string s)
		{
			if (s == null || s == "")
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		protected bool IsNull(string s)
		{
			if (s == null || s == "")
			{
				return true;
			}
			else
			{
				return false;
			}	
		}

		protected string ConvertTrueFalse(bool formValue)
		{
			if (formValue == true)
			{
				return "Yes";
			}
			else
			{
				return "No";
			}
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
						cbl.Items[i].Value.ToString() + 
						"</li>" + "\r");
				}
			}

			// End unordered list
			sb.Append("</ul>" + "\r");

			// Return string builder
			return sb;
		}

		public bool CheckTextBox(TextBox textBoxControl)
		{
			string textBox;
			textBox = textBoxControl.Text.Trim();

			if (textBox == null || textBox == "")
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		
		public SMTPServer GetMailServer(SmtpServerStatus status)
		{
			// Create SMTPServer object
			SMTPServer mailServ = new SMTPServer();

			// If the status is set to testing
			if (status == SmtpServerStatus.Testing)
			{
				// Set the mail server value to Bresnan
				mailServ.Name = "mail.bresnan.net";
				return mailServ;
			}
			else
			{
				// Set the mail server value to the live server value
				mailServ.AuthMode = SMTPAuthMode.AuthLogin;
				mailServ.Account = "webmaster";
				mailServ.Password = "Ow^>(lD+9";
				mailServ.Name = "mail.firestardesign.com";
				return mailServ;
			}
		}

		protected void SendAutoResponse(string email, string name, FormType type)
		{
			// Create an new Quiksoft e-mail message
			EmailMessage em = new EmailMessage();
  
			//  Add a from name and e-mail
			em.From.Email = "marsha@meetmarsha.com";
			em.From.Name = "Marsha Bryan";

			// Add autoresponder recipient
			em.Recipients.Add(email, name, RecipientType.To);

			// Add Marsha Bryan Recipient
			em.Recipients.Add("marsha@meetmarsha.com", "Marsha Bryan", RecipientType.To);

			// Add Sean Smith Recipient
			em.Recipients.Add("webmaster@meetmarsha.com", "Sean Smith", RecipientType.BCC);
 
			// Fill-in subject or default subject from form
			em.Subject = "Automatic Response From The Marsha Bryan Team.";

			// Template path
			string templatePath;

			// Determine which template to use
			switch(type)       
			{         
				case FormType.HomeSearch:
					// Set path to e-mail template
					templatePath = "/templates/" + "HomeSearch.html";
					break;
				case FormType.ContactForm:         
					// Set path to e-mail template
					templatePath = "/templates/" + "ContactForm.html";
					break;
				default:      
					// Set path to e-mail template
					templatePath = "/templates/" + "ContactForm.html";            
					break;
			}

			// Import body from HTML template
			em.ImportBody(Request.MapPath(templatePath), BodyPartFormat.HTML, ImportBodyFlags.None);

			// Create new DataTable columns to represent the fields
			// that will be replaced in each e-mail template
			DataTable emailRecipients = new DataTable();
			emailRecipients.Columns.Add("Name");

			// Create new DataRow and insert values collected
			// from form and query string
			DataRow newRow = emailRecipients.NewRow();
			newRow["Name"] = name;

			// Add new row to emailRecipients datatable
			emailRecipients.Rows.Add(newRow);
 
			// Create a new SMTP object
			SMTP smtp = new SMTP();

			// Add a new SMTP server to the SMTPServers collection
			smtp.SMTPServers.Add(GetMailServer(ServerStatus));

			// Try to send the message and catch any errors
			try
			{
				// Send the message using the bulk merge feature using
				// the emailRecipients DataTable as the template
				// and setting defaults of 0 for MessagesPerSecond
				// and Millisecond pause between connections
				smtp.SendBulkMerge(em, emailRecipients, 0, 0);
			}
			catch (Exception ex)
			{
				// Get Exception message
				string exMessage = GetExceptionDump(ex);

				// Display exception message during error handling
				Debug.Fail(exMessage);
			}
		}

		protected string GetExceptionDump(Exception ex)
		{
			// Create StringBuilder to hold error message
			StringBuilder sb = new StringBuilder("\n");

			// Return error message
			sb.Append("<p>Exception Details:</p>" + "\n");
			sb.Append("<ul>" + "\n");
			sb.Append(" <li>Error Message: " + ex.Message + "</li>" + "\n");
			sb.Append(" <li>Error Source: " + ex.Source + "</li>" + "\n");
			sb.Append(" <li>Stack Trace: " + ex.StackTrace + "</li>" + "\n");
			sb.Append("</ul>" + "\n");

			return sb.ToString();
		}
	}
}