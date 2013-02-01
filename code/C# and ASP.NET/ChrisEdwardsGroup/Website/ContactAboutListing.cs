using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Quiksoft.EasyMail.SMTP;

namespace ChrisEdwardsGroup.Website
{
	/// <summary>
	/// Summary description for ContactAboutListing.
	/// </summary>
	public class ContactAboutListing : Main
	{
		public Literal ltlTitleMLS;
		public TextBox txtFirstName;
		public TextBox txtLastName;
		public TextBox txtTelephone1;
		public TextBox txtTelephone2;
		public TextBox txtTelephone3;
		public TextBox txtEmail;
		public TextBox txtCommentsField;
		public HtmlGenericControl hgcStatus;

		public void Page_Load(Object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				Bind();
			}
		}

		private void Bind()
		{
			string mlsID = Request.QueryString["MLSID"];
			ltlTitleMLS.Text = mlsID;
		}

		public void EmailContact_Click(Object sender, EventArgs e)
		{
			// Convert Variables to Strings
			// For Easy Reference

			// MLS
			string mlsID;
			mlsID = Request.QueryString["MLSID"];

			// Telephone1
			string tel1;
			tel1 = txtTelephone1.Text.ToString();

			// Telephone2
			string tel2;
			tel2 = txtTelephone2.Text.ToString();

			// Telephone3
			string tel3;
			tel3 = txtTelephone3.Text.ToString();

			// Full Telephone
			string telephone;
			telephone = "(" + tel1 + ") " + 
				tel2 + "-" + tel3;

			// First Name
			string firstName;
			firstName = txtFirstName.Text.ToString();

			// Last Name
			string lastName;
			lastName = txtLastName.Text.ToString();

			// Email
			string email;
			email = txtEmail.Text.ToString();

			// Comments
			string comments;
			comments = txtCommentsField.Text.ToString();

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
			emailMessage.Subject = "Chris Edwards Group Website Lead.";

			string bodyText;

			// e-mail body
			bodyText = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\">";
			bodyText += "<html>";
			bodyText += "<head>";
			bodyText += "<title>Chris Edwards Group Website Lead</title>";
			bodyText += "<link rel=\"stylesheet\" href=\"http://www.chrisedwardsgroup.com/includes/email.css\" type=\"text/css\">" + "\r";
			bodyText += "</head>";
			bodyText += "<body>";

			// Start Message
			bodyText += "<h1>" + "\r";
			bodyText += "Chris Edwards Group Website Lead" + "\r";
			bodyText += "</h1>" + "\r";

			// Start Body Heading
			bodyText += "<table>";
			bodyText += "<tbody>";
			bodyText += "<tr>";
			bodyText += "<th>";
			bodyText += "In Reference to Listing MLS#: " +
				"<a href=\"http://www.chrisedwardsgroup.com/" +
				"raleigh-cary-real-estate/single-family-home.aspx?MLSID=" + mlsID + "\">" + 
				mlsID + "</a>";
			bodyText += "</th>";
			bodyText += "</tr>";
			bodyText += "</tbody>";
			bodyText += "</table>";
			
			// Start Lead Information
			bodyText += "<table cellpadding=\"5\">";
			bodyText += "<tbody>";
			bodyText += "<tr>";
			bodyText += "<td>";
			bodyText += "First Name:";
			bodyText += "</td>";
			bodyText += "<td>";
			bodyText += firstName;
			bodyText += "</td>";
			bodyText += "</tr>";
			bodyText += "<tr>";
			bodyText += "<td>";
			bodyText += "Last Name:";
			bodyText += "</td>";
			bodyText += "<td>";
			bodyText += lastName;
			bodyText += "</td>";
			bodyText += "</tr>";
			bodyText += "<tr>";
			bodyText += "<td>";
			bodyText += "Telephone Number:";
			bodyText += "</td>";
			bodyText += "<td>";
			bodyText += telephone;
			bodyText += "</td>";
			bodyText += "</tr>";
			bodyText += "<tr>";
			bodyText += "<td>";
			bodyText += "E-mail Address:";
			bodyText += "</td>";
			bodyText += "<td>";
			bodyText += "<a href=\"mailto:" + email +
				"\">" + email + "</a>"; 
			bodyText += "</td>";
			bodyText += "</tr>";
			bodyText += "<tr>";
			bodyText += "<td colspan=\"2\">";
			bodyText += "Comments:";
			bodyText += "</td>";
			bodyText += "</tr>";
			bodyText += "<tr>";
			bodyText += "<td colspan=\"2\">";
			bodyText += comments;
			bodyText += "</td>";
			bodyText += "</tr>";
			bodyText += "</tbody>";
			bodyText += "</table>";
			bodyText += "</body>";
			bodyText += "</html>";

			// Add an HTML body part
			emailMessage.BodyParts.Add(bodyText, BodyPartFormat.HTML);
 
			// Create the SMTP object using the constructor to specify the mail server
			SMTP smtp = new SMTP("mail.chrisedwardsgroup.com");

			try
			{
				// Send the message
				smtp.Send(emailMessage);

				// Display confirmation message
				hgcStatus.Visible = true;
				hgcStatus.InnerText = "Contact Request Sent Successfully!";
			}
			catch (Exception objError)
			{
				// Catch Errors and Display
				hgcStatus.Visible = true;
				hgcStatus.InnerText = "Error While Sending Message: " + 
					objError.Message + objError.Source;
				return;
			}
		}
	}
}