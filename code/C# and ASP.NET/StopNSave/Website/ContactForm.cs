using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Quiksoft.EasyMail.SMTP;

namespace StopNSave.Website
{
	/// <summary>
	/// Website contact form.
	/// </summary>
	public class ContactForm : Main
	{
		public HtmlGenericControl htmErrors;
		public TextBox txtFirstName;
		public TextBox txtLastName;
		public TextBox txtEmail;
		public TextBox txtTelephone;
		public TextBox txtComments;

		public void Submit_OnClick(Object sender, EventArgs e)
		{
			// Send Contact Form
			SendMail_ContactForm();
		}

		public void SendMail_ContactForm()
		{
			// Convert web form variables to local variables
			// so that they are easy to work with

			// First Name
			string firstName;
			firstName = txtFirstName.Text.ToString();

			// Last Name
			string lastName;
			lastName = txtLastName.Text.ToString();

			// E-mail Address
			string email;
			email = txtEmail.Text.ToString();
			
			// Telephone number.
			string telephone = txtTelephone.Text.ToString();

			// Comments
			string comments;
			comments = txtComments.Text.ToString();

			// Create the EmailMessage object
			EmailMessage em = new EmailMessage();
  
			// Specify From Address and Display Name
			em.From.Email = email;
			em.From.Name = firstName + " " + lastName;
 
			// Check to see what whether the server is set to live
			// or testing mode
			if (ServerStatus == SmtpServerStatus.Live)
			{
				// Add Feather Petroleum Recipient
				em.Recipients.Add("general@featherpetro.com", "Feather Petroleum", RecipientType.To);
			}
 
			// Add Bill Smith Recipient
			em.Recipients.Add("info@firestardesign.com", "Bill Smith", RecipientType.BCC);

			// Add Sean Smith Recipient
			em.Recipients.Add("sean@firestardesign.com", "Sean Smith", RecipientType.BCC);
 
			// Specify the subject
			em.Subject = "Stop 'n Save Website - Contact Form.";

			string bodyText;

			// Start E-mail Body
			bodyText = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">" + "\r";
			bodyText += "<html>" + "\r";
			bodyText += "<head>" + "\r";
			bodyText += "<title>" + "\r";
			bodyText += "Contact Form" + "\r";
			bodyText += "</title>" + "\r";
			bodyText += "<link rel=\"stylesheet\" href=\"http://featherpetro.com/includes/email.css\" type=\"text/css\">" + "\r";
			bodyText += "</head>" + "\r";
			bodyText += "<body>" + "\r";

			// Start Heading
			bodyText += "<h1>" + "\r";
			bodyText += "Contact Form" + "\r";
			bodyText += "</h1>" + "\r";

			// Start Table
			bodyText += "<table>" + "\r";
			bodyText += "<tbody>" + "\r";
			
			// Start Personal Information Section
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\" class=\"formheading\">" + "\r";
			bodyText += "Contact Form" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start First Name, Last Name
			bodyText += "<tr>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "First Name:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Last Name:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += firstName + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += lastName + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";
			
			// Start E-mail Address, Phone Number
			bodyText += "<tr>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "E-mail Address:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Phone Number:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += "<a href=\"mailto:" + email +
				"\">" + email + "</a>" + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += telephone + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Comments
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\">" + "\r";
			bodyText += "Comments:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td colspan=\"2\">" + "\r";
			bodyText += comments + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// End Table
			bodyText += "</tbody>" + "\r";
			bodyText += "</table>" + "\r";

			// End HTML Body
			bodyText += "</body>" + "\r";
			bodyText += "</html>" + "\r";

			// Add an HTML body part
			em.BodyParts.Add(bodyText, BodyPartFormat.HTML);
 
			// Create a new SMTP object
			SMTP smtp = new SMTP();

			// Add a new SMTP server to the SMTPServers collection and set 
			// the status mode to live or testing
			smtp.SMTPServers.Add(GetMailServer(ServerStatus));

			try
			{
				// Send the message
				smtp.Send(em);

				// If successful redirect to thank you page
				Response.Redirect("/Confirmation.aspx", true);
			}
			catch (Exception objError)
			{
				// Display errors
				htmErrors.Visible = true;
				htmErrors.InnerText = "Error Message: " + objError.Message
					+ " Error Source: " + objError.Source;
				return;
			}
		}
	}
}