using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Quiksoft.EasyMail.SMTP;

namespace ChrisEdwardsGroup.Website
{
	/// <summary>
	/// Summary description for EmailAFriend.
	/// </summary>
	public class EmailAFriend : Main
	{
		public HtmlGenericControl hgcStatus;
		public TextBox txtToField;
		public TextBox txtCCField;
		public TextBox txtFromField;
		public TextBox txtSubjectField;
		public TextBox txtCommentsField;

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

			txtSubjectField.Text = "You've gotta see this house! (MLS# " +
				mlsID + ")";
		}

		public void EmailContact_Click(Object sender, EventArgs e)
		{
			string mlsID = Request.QueryString["MLSID"];
			string toField = txtToField.Text.ToString();
			string ccField = txtCCField.Text.ToString();
			string fromField = txtFromField.Text.ToString();
			string subjectField = txtSubjectField.Text.ToString();
			string commentsField = txtCommentsField.Text.ToString();

			// Create the EmailMessage object
			EmailMessage emailMessage = new EmailMessage();
  
			// Specify From Address and Display Name
			emailMessage.From.Email = "chris@chrisedwardsgroup.com";
			emailMessage.From.Name = "Chris Edwards";

			// Add To Recipient
			emailMessage.Recipients.Add(toField, toField, RecipientType.To);

			// If CC E-mail Address Is Not Null
			if (!IsNull(ccField))
			{
				// Add CC Recipient
				emailMessage.Recipients.Add(ccField, ccField, RecipientType.CC);
			}

			// Add Chris Edwards Recipient
			emailMessage.Recipients.Add("chris@chrisedwardsgroup.com", "Chris Edwards", RecipientType.BCC);

			// Add Bill Smith Recipient
			emailMessage.Recipients.Add("info@firestardesign.com", "Bill Smith", RecipientType.BCC);

			// Add Sean Smith Recipient
			emailMessage.Recipients.Add("sean@firestardesign.com", "Sean Smith", RecipientType.BCC);
 
			// Specify the subject
			emailMessage.Subject = "Chris Edwards Group E-mail A Friend.";

			string bodyText;

			// Start message body
			bodyText = "<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\">" + "\r";
			bodyText += "<html>" + "\r";
			bodyText += "<head>" + "\r";
			bodyText += "<title>E-mail a Friend</title>" + "\r";
			bodyText += "<link rel=\"stylesheet\" href=\"http://www.chrisedwardsgroup.com/includes/email.css\" type=\"text/css\">" + "\r";
			bodyText += "</head>" + "\r";
			bodyText += "<body>" + "\r";

			//Start heading
			bodyText += "<h1>" + "\r";
			bodyText += "E-mail a Friend" + "\r";
			bodyText += "</h1>" + "\r";

			bodyText += "<table>" + "\r";
			bodyText += "<tbody>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += "To:" + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += "<a href=\"mailto:" + toField +
				"\">" + toField + "</a>";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Check to see if CC is Emtpy or NULL
			// If not write HTML to Body
			if (!IsNull(ccField))
			{
				bodyText += "<tr>" + "\r";
				bodyText += "<td>" + "\r";
				bodyText += "CC:" + "\r";
				bodyText += "</td>" + "\r";
				bodyText += "<td>" + "\r";
				bodyText += "<a href=\"mailto:" + ccField +
					"\">" + ccField + "</a>";
				bodyText += "</td>" + "\r";
				bodyText += "</tr>" + "\r";
			}
			// Start From Section
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += "From:" + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += "<a href=\"mailto:" + fromField +
				"\">" + fromField + "</a>";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";
			// Start Subject Section
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += "Subject:" + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += subjectField + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";
			// Start Body Section
			bodyText += "<tr>" + "\r";
			bodyText += "<td colspan=\"2\">" + "\r";
			bodyText += commentsField + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";
			// Start Link Section
			bodyText += "<tr>" + "\r";
			bodyText += "<td colspan=\"2\">" + "\r";
			bodyText += "<a href=\"http://www.chrisedwardsgroup.com/" +
				"raleigh-cary-real-estate/single-family-home.aspx?MLSID=" + mlsID + "\"" +
				" target=\"_blank\">" + "Click here to view property MLS#" +
				mlsID + "</a>" + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";
			// Start Salutation Section
			bodyText += "<tr>" + "\r";
			bodyText += "<td colspan=\"2\">" + "\r";
			bodyText += "Sincerely," + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td colspan=\"2\">" + "\r";
			bodyText += "<a href=\"mailto:" + fromField +
				"\">" + fromField + "</a>";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "</tbody>" + "\r";
			bodyText += "</table>" + "\r";

			// End HTML body
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

				// Display confirmation message
				hgcStatus.Visible = true;
				hgcStatus.InnerText = "E-mail A Friend Sent Successfully!";
			}
			catch (Exception objError)
			{
				// Display error message
				hgcStatus.Visible = true;
				hgcStatus.InnerText = "Error While Sending Message: " + 
					objError.Message + objError.Source;
				return;
			}
		}
	}
}