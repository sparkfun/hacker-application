using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Quiksoft.EasyMail.SMTP;

namespace ChrisEdwardsGroup.Website
{
	/// <summary>
	/// Summary description for RelocationPacket.
	/// </summary>
	public class RelocationPacket : Main
	{
		public HtmlGenericControl htmErrors;
		public TextBox txtFirstName;
		public TextBox txtLastName;
		public TextBox txtAddress1;
		public TextBox txtAddress2;
		public TextBox txtCity;
		public TextBox txtState;
		public TextBox txtZipCode;
		public TextBox txtEmail;
		public TextBox txtPhoneNumber1;
		public TextBox txtPhoneNumber2;
		public TextBox txtPhoneNumber3;
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

			// Address1
			string address1;
			address1 = txtAddress1.Text.ToString();

			// Address2
			string address2;
			address2 = txtAddress2.Text.ToString();

			// City
			string city;
			city = txtCity.Text.ToString();

			// State
			string state;
			state = txtState.Text.ToString();

			// Zip Code
			string zipCode;
			zipCode = txtZipCode.Text.ToString();

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
			emailMessage.Subject = "Triangle Relocation Packet Request.";

			string bodyText;

			// Start E-mail Body
			bodyText = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">" + "\r";
			bodyText += "<html>" + "\r";
			bodyText += "<head>" + "\r";
			bodyText += "<title>" + "\r";
			bodyText += "Triangle Relocation Packet Request" + "\r";
			bodyText += "</title>" + "\r";
			bodyText += "<link rel=\"stylesheet\" href=\"http://www.chrisedwardsgroup.com/includes/email.css\" type=\"text/css\">" + "\r";
			bodyText += "</head>" + "\r";
			bodyText += "<body>" + "\r";

			// Start Heading
			bodyText += "<h1>" + "\r";
			bodyText += "Triangle Relocation Packet Request" + "\r";
			bodyText += "</h1>" + "\r";

			// Start Table
			bodyText += "<table>" + "\r";
			bodyText += "<tbody>" + "\r";
			
			// Start Personal Information Section
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\" class=\"formheading\">" + "\r";
			bodyText += "Free Relocation Packet Request" + "\r";
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

			// Start City, State
			bodyText += "<tr>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "City:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "State:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += city + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += state + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start ZipCode
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\">" + "\r";
			bodyText += "Zip Code:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td colspan=\"2\">" + "\r";
			bodyText += zipCode + "\r";
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
			emailMessage.BodyParts.Add(bodyText, BodyPartFormat.HTML);
 
			// Create the smtp object
			SMTP smtp = new SMTP();
			smtp.SMTPServers.Add(GetMailServer());

			try
			{
				// Send the message
				smtp.Send(emailMessage);

				// Send autoresponder message
				Send_AutoResponder();

				// If successful redirect to thank you page
				Response.Redirect("triangle-relocation-thanks.aspx", true);
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

		public void Send_AutoResponder()
		{
			// Convert web form variables to local variables
			// so that they are easy to work with

			// First Name
			string firstName = txtFirstName.Text.ToString();

			// Last Name
			string lastName = txtLastName.Text.ToString();

			// Full Name
			string fullName = firstName + " " + lastName;

			// E-mail Address
			string email;
			email = txtEmail.Text.ToString();

			// Create the EmailMessage object
			EmailMessage emailMessage = new EmailMessage();
  
			// Specify From Address and From Name
			emailMessage.From.Email = "chris@chrisedwardsgroup.com";
			emailMessage.From.Name = "Chris Edwards";

			// Add Autoresponder Recipient
			emailMessage.Recipients.Add(email, fullName, RecipientType.To);
 
//			// Add Chris Edwards Recipient
//			emailMessage.Recipients.Add("chris@chrisedwardsgroup.com", "Chris Edwards", RecipientType.BCC);
//
//			// Add Sean Smith Recipient
//			emailMessage.Recipients.Add("sean@firestardesign.com", "Sean Smith", RecipientType.BCC);
 
			// Specify the subject
			emailMessage.Subject = "Relocation packet request received.  Thank you.";

			string bodyText;

			// Start E-mail Body
			bodyText = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">" + "\r";
			bodyText += "<html>" + "\r";
			bodyText += "<head>" + "\r";
			bodyText += "<title>" + "\r";
			bodyText += "Relocation packet request received.  Thank you." + "\r";
			bodyText += "</title>" + "\r";
			bodyText += "<link rel=\"stylesheet\" href=\"http://www.chrisedwardsgroup.com/includes/email.css\" type=\"text/css\">" + "\r";
			bodyText += "</head>" + "\r";
			bodyText += "<body>" + "\r";

			// Start Paragraph
			bodyText += "<p>" + "\r";
			bodyText += "Hello " + firstName + "," + "\r";
			bodyText += "</p>" + "\r";

			// Start Paragraph
			bodyText += "<p>" + "\r";
			bodyText += "Thank you for requesting a relocation packet from the Chris Edwards Group! I have created this resource not only to assist with learning a new area or locating a new home, but to be a complete resource to take most of the tasks and worry out of your hands!" + "\r";
			bodyText += "</p>" + "\r";

			// Start Paragraph
			bodyText += "<p>" + "\r";
			bodyText += "If you have decided to move this area or just considering a move, we are here to help. I provide a complete relocation service to ensure your move is a smooth one. Home searching, buyer agency representation, excellent mover connections and lending assistance are just some of the ways we can greatly help! So many benefits at <strong>NO</strong> cost to you!" + "\r";
			bodyText += "</p>" + "\r";

			// Start Paragraph
			bodyText += "<p>" + "\r";
			bodyText += "Please take a moment to let us know how we can best assist you by responding to the questions below. This will help us to build a personal, detailed profile for your move." + "\r";
			bodyText += "</p>" + "\r";

			// Start Ordered List
			bodyText += "<ol>" + "\r";
			bodyText += "<li>What brings you to the Raleigh/Cary area?</li>" + "\r";
			bodyText += "<li>Would you be relocating with a job?</li>" + "\r";
			bodyText += "<li>What is your time frame to move?</li>" + "\r";
			bodyText += "</ol>" + "\r";

			// Start Paragraph
			bodyText += "<p>" + "\r";
			bodyText += "I have developed one of the best real estate web sites in North Carolina that is focused on helping you in many ways with your move to this great area! We continue to make enhancements to provide more information and better area visuals to assist people getting ready to make the move. Feel to call us with any questions you may have at <strong>1-888-828-0288</strong>!  We hope to get the opportunity to serve you!" + "\r";
			bodyText += "</p>" + "\r";

			// Start Paragraph
			bodyText += "<p>" + "\r";
			bodyText += "Sincerely," + "\r";
			bodyText += "</p>" + "\r";

			// Start Paragraph
			bodyText += "<p>" + "\r";
			bodyText += "Chris Edwards<br />";
			bodyText += "Broker Associate, ePro<br />";
			bodyText += "RE/MAX United / CEG<br />";
			bodyText += "<a href=\"http://www.chrisedwardsgroup.com/\">www.ChrisEdwardsGroup.com</a><br />";
			bodyText += "1-888-828-0288";
			bodyText += "</p>" + "\r";

			// Start Business Card Image
			bodyText += "<p>" + "\r";
			bodyText += "<a href=\"http://www.chrisedwardsgroup.com/\"><img src=\"http://www.chrisedwardsgroup.com/graphics/chris-edwards-card.jpg\" width=\"350\" height=\"200\" border=\"1\" alt=\"Chris Edwards RE/MAX United\" /></a>";
			bodyText += "</p>" + "\r";

			// End HTML Body
			bodyText += "</body>" + "\r";
			bodyText += "</html>" + "\r";

			// Add an HTML body part
			emailMessage.BodyParts.Add(bodyText, BodyPartFormat.HTML);
 
			// Create the smtp object
			SMTP smtp = new SMTP();
			smtp.SMTPServers.Add(GetMailServer());

			try
			{
				// Send the message
				smtp.Send(emailMessage);

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