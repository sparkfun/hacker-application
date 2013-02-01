using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Quiksoft.EasyMail.SMTP;
using REMS.Configuration;

namespace ChrisEdwardsGroup.Website
{
	/// <summary>
	/// Summary description for Contact.
	/// </summary>
	public class Contact : Main
	{
		public HtmlGenericControl htmErrors;
		public TextBox txtFirstName;
		public TextBox txtLastName;
		public TextBox txtEmail;
		public TextBox txtPhoneNumber1;
		public TextBox txtPhoneNumber2;
		public TextBox txtPhoneNumber3;
		public TextBox txtComments;

        // Create new WebFormsConfiguration instance.
        private WebFormConfiguration wfc = new WebFormConfiguration();

		public void Submit_OnClick(Object sender, EventArgs e)
		{
			// Send Contact Form
			Send_ContactForm();
		}

		public void Send_ContactForm()
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

			// Telephone #1
			string telephone1;
			telephone1 = txtPhoneNumber1.Text.ToString();

			// Telephone #2
			string telephone2;
			telephone2 = txtPhoneNumber2.Text.ToString();

			// Telephone #3
			string telephone3;
			telephone3 = txtPhoneNumber3.Text.ToString();

			// Full Telephone
			string fullTelephone;
			fullTelephone = "(" + telephone1 + ") " + telephone2 +
				"-" + telephone3;

			// Comments
			string comments;
			comments = txtComments.Text.ToString();

            // Create a new EmailConfiguration instance.
            EmailConfiguration emConfig = new EmailConfiguration();

            // Initialize a new EmailMessage instance by
            // loading up the recipients from the
            // EmailConfiguration class.
            EmailMessage em = emConfig.LoadRecipients();
  
			// Specify From Address and Display Name
			em.From.Email = email;
			em.From.Name = firstName + " " + lastName;
 
			// Specify the subject
			em.Subject = "Chris Edwards Group Contact Form.";

			string bodyText;

			// Start E-mail Body
			bodyText = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">" + "\r";
			bodyText += "<html>" + "\r";
			bodyText += "<head>" + "\r";
			bodyText += "<title>" + "\r";
			bodyText += "Contact Form" + "\r";
			bodyText += "</title>" + "\r";
            bodyText += "<link rel=\"stylesheet\" href=\"http://" + wfc.DomainName +
                wfc.IncludesPath + "/" + wfc.CSSEmail + "\"" + " type=\"text/css\">" + "\r";
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
			bodyText += fullTelephone + "\r";
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

            // Add a new SMTP server to the SMTPServers collection
            // and set it using the ActiveSMTPServer property
            // of the e-mail configuration.
            smtp.SMTPServers.Add(emConfig.ActiveSMTPServer);

            try
            {
                // Send the message
                smtp.Send(em);

                // If successful redirect to thank you page
                Response.Redirect(wfc.ConfirmPage, true);
            }
            catch (SMTPAuthenticationException authEx)
            {
                // Display errors
                htmErrors.Visible = true;
                htmErrors.InnerText = "SMTP Authentication Error: " + authEx.Message
                    + " Error Source: " + authEx.Source + " Error Code: " + authEx.ErrorCode + ".";
                htmErrors.InnerText += "SMTP Information: " + emConfig.ActiveSMTPServer.Name;
                return;
            }
            catch (Exception objError)
            {
                // Display errors
                htmErrors.Visible = true;
                htmErrors.InnerText = "Error Message: " + objError.Message
                    + " Error Source: " + objError.Source;
                htmErrors.InnerText += "SMTP Information: " + emConfig.ActiveSMTPServer.Name;
                return;
            }
		}
	}
}