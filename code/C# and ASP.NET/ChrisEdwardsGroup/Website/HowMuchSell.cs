using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Quiksoft.EasyMail.SMTP;

namespace ChrisEdwardsGroup.Website
{
	/// <summary>
	/// Summary description for HomeSearch.
	/// </summary>
	public class HowMuchSell : Main
	{
		public HtmlGenericControl htmErrors;
		public TextBox txtAddress1;
		public TextBox txtAddress2;
		public TextBox txtCity;
		public TextBox txtZipCode;
		public TextBox txtOtherInformation;
		public TextBox txtFirstName;
		public TextBox txtLastName;
		public TextBox txtEmail;
		public TextBox txtPhoneNumber1;
		public TextBox txtPhoneNumber2;
		public TextBox txtPhoneNumber3;

		public void Submit_OnClick(Object sender, EventArgs e)
		{
			// Send How Much Sell E-mail
			SendMail_HowMuchSell();
		}

		public void SendMail_HowMuchSell()
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

			// Address #1
			string address1;
			address1 = txtAddress1.Text.ToString();

			// Address #2
			string address2;
			address2 = txtAddress2.Text.ToString();

			// City
			string city;
			city = txtCity.Text.ToString();

			// Zip Code
			string zipCode;
			zipCode = txtZipCode.Text.ToString();

			// Other Information
			string otherInfo;
			otherInfo = txtOtherInformation.Text.ToString();

			// Create the EmailMessage object
			EmailMessage emailMessage = new EmailMessage();
  
			// Specify From Address and Display Name
			emailMessage.From.Email = email;
			emailMessage.From.Name = firstName + " " + lastName;
 
			// Add Chris Edwards Recipient
			emailMessage.Recipients.Add("chris@chrisedwardsgroup.com", "Chris Edwards", RecipientType.To);
 
			// Add Bill Smith Recipient
			emailMessage.Recipients.Add("info@firestardesign.com", "Bill Smith", RecipientType.BCC);

			// Add Sean Smith Recipient
			emailMessage.Recipients.Add("sean@firestardesign.com", "Sean Smith", RecipientType.BCC);
 
			// Specify the subject
			emailMessage.Subject = "How Much Did They Get For It?";

			string bodyText;

			// Start E-mail Body
			bodyText = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">" + "\r";
			bodyText += "<html>" + "\r";
			bodyText += "<head>" + "\r";
			bodyText += "<title>" + "\r";
			bodyText += "How Much Did They Get For It?" + "\r";
			bodyText += "</title>" + "\r";
			bodyText += "<link rel=\"stylesheet\" href=\"http://www.chrisedwardsgroup.com/includes/email.css\" type=\"text/css\">" + "\r";
			bodyText += "</head>" + "\r";
			bodyText += "<body>" + "\r";

			// Start Heading
			bodyText += "<h1>" + "\r";
			bodyText += "How Much Did They Get For It?" + "\r";
			bodyText += "</h1>" + "\r";

			// Start Table
			bodyText += "<table>" + "\r";
			bodyText += "<tbody>" + "\r";
			
			// Start Personal Information Section
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\" class=\"formheading\">" + "\r";
			bodyText += "Personal Information" + "\r";
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

			// End Table
			bodyText += "</tbody>" + "\r";
			bodyText += "</table>" + "\r";

			// Start Table
			bodyText += "<table>" + "\r";
			bodyText += "<tbody>" + "\r";

			// Start Tell Us More Section
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\" class=\"formheading\">" + "\r";
			bodyText += "Home Information" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Address1, Address2
			bodyText += "<tr>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Address 1:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Address 2:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += address1 + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += address2 + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start City, Zip Code
			bodyText += "<tr>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "City:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Zip Code:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += city + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += zipCode + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Other Information
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\">" + "\r";
			bodyText += "Other Information:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td colspan=\"2\">" + "\r";
			bodyText += otherInfo + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// End Table
			bodyText += "</tbody>" + "\r";
			bodyText += "</table>" + "\r";

			// End HTML Body
			bodyText += "</body>" + "\r";
			bodyText += "</html>" + "\r";

			// Add an HTML body part
			emailMessage.BodyParts.Add(bodyText, BodyPartFormat.HTML);
 
			// Create the SMTP object using the constructor to specify the mail server
			SMTP smtp = new SMTP("mail.chrisedwardsgroup.com");

			try
			{
				// Send the message
				smtp.Send(emailMessage);

				// If successful redirect to thank you page
				Response.Redirect("how-much-sell-raleigh-cary-house-thanks.aspx", true);
			}
			catch (Exception objError)
			{
				// Display errors
				htmErrors.Visible = true;
				htmErrors.InnerText = "Error while accessing data: " + objError.Message
					+ " Error Source: " + objError.Source;
				return;
			}
		}
	}
}