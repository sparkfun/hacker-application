using System;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Quiksoft.EasyMail.SMTP;

namespace ChrisEdwardsGroup.Website
{
	/// <summary>
	/// Request free home value estimate form.
	/// </summary>
	public class HomeValueEstimate : Main
	{
		public HtmlGenericControl htmErrors;
		public TextBox txtAddress1;
		public TextBox txtAddress2;
		public TextBox txtCity;
		public TextBox txtZipCode;
		public TextBox txtLotSize;
		public TextBox txtSqFt;
		public TextBox txtBedrooms;
		public TextBox txtBathrooms;
		public TextBox txtYearBuilt;
		public TextBox txtNeighborhood;
		public RadioButtonList rblHomeType;
		public RadioButtonList rblHomeStyle;
		public RadioButtonList rblBasementType;
		public RadioButtonList rblParking;
		public CheckBoxList chblAmenities;
		public TextBox txtOtherFeaturesAmenities;
		public RadioButtonList rblHomeCondition;
		public RadioButtonList rblReasonForEvaluation;
		public RadioButtonList rblHowSoonMoving;
		public TextBox txtFirstName;
		public TextBox txtLastName;
		public TextBox txtEmail;
		public TextBox txtPhoneNumber1;
		public TextBox txtPhoneNumber2;
		public TextBox txtPhoneNumber3;
		public DropDownList ddlContactMethod;
		public TextBox txtBestTimeContact;

		public void Submit_OnClick(Object sender, EventArgs e)
		{
			// Send Home Worth E-mail
			SendMail_HomeWorth();
		}

		public void SendMail_HomeWorth()
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
			
			// Preferred contact method
			string contactMethod = ddlContactMethod.SelectedValue.ToString();

			// Best time to contact
			string bestTimeContact = txtBestTimeContact.Text.Trim().ToString();

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

			// Lot Size
			string lotSize;
			lotSize = txtLotSize.Text.ToString();

			// Sq. Ft.
			string sqFt;
			sqFt = txtSqFt.Text.ToString();

			// Bedrooms
			string beds;
			beds = txtBedrooms.Text.ToString();

			// Bathrooms
			string baths;
			baths = txtBathrooms.Text.ToString();

			// Year Built
			string yearBuilt;
			yearBuilt = txtYearBuilt.Text.ToString();

			// Neighborhood
			string neighborhood;
			neighborhood = txtNeighborhood.Text.ToString();

			// Home Type
			string homeType;
			homeType = rblHomeType.SelectedItem.Value.ToString();

			// Home Style
			string homeStyle;
			homeStyle = rblHomeStyle.SelectedItem.Value.ToString();

			// Basement Type
			string basementType;
			basementType = rblBasementType.SelectedItem.Value.ToString();

			// Parking
			string parking;
			parking = rblParking.SelectedItem.Value.ToString();

			// Amenities
			StringBuilder amenities = new StringBuilder("");
			amenities.Append("<ul>" + "\r");
			for (int i = 0; i < chblAmenities.Items.Count; i++)
			{
				if (chblAmenities.Items[i].Selected)
				{
					amenities.Append("<li>" +
						chblAmenities.Items[i].Value.ToString() + 
						"</li>" + "\r");
				}
			}
			amenities.Append("</ul>" + "\r");

			// Other Features and Amenities
			string otherFeatAmen;
			otherFeatAmen = txtOtherFeaturesAmenities.Text.ToString();

			// Home Condition
			string homeCondition;
			homeCondition = rblHomeCondition.SelectedItem.Value.ToString();

			// Reason For Evaluation
			string reasonForEvaluation;
			reasonForEvaluation = rblReasonForEvaluation.SelectedItem.Value.ToString();

			// How Soon Moving
			string howSoonMoving;
			howSoonMoving = rblHowSoonMoving.SelectedItem.Value.ToString();

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
			emailMessage.Subject = "Chris Edwards Group - Free Home Value Estimate";

			string bodyText;

			// Start E-mail Body
			bodyText = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">" + "\r";
			bodyText += "<html>" + "\r";
			bodyText += "<head>" + "\r";
			bodyText += "<title>" + "\r";
			bodyText += "Chris Edwards Group - Free Home Value Estimate" + "\r";
			bodyText += "</title>" + "\r";
			bodyText += "<link rel=\"stylesheet\" href=\"http://www.chrisedwardsgroup.com/includes/email.css\" type=\"text/css\">" + "\r";
			bodyText += "</head>" + "\r";
			bodyText += "<body>" + "\r";

			// Start Heading
			bodyText += "<h1>" + "\r";
			bodyText += "Free Home Value Estimate" + "\r";
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
			
			// Start preferred contact method, best time to contact
			bodyText += "<tr>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Preferred Method of Contact:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Best Time to Contact:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += contactMethod + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += bestTimeContact + "\r";
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
			bodyText += "Tell Us More About Your Raleigh-Cary Area Home" + "\r";
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

			// Start Lot Size, Square Footage
			bodyText += "<tr>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Lot Size:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Square Footage:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += lotSize + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += sqFt + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Number of Bedrooms, Number of Bathrooms
			bodyText += "<tr>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Number of Bedrooms:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Number of Bathrooms:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += beds + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += baths + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Year Built, Neighborhood
			bodyText += "<tr>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Year Built:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Neighborhood:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += yearBuilt + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += neighborhood + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Home Type, Home Style
			bodyText += "<tr>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Home Type:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Home Style:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += homeType + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += homeStyle + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Basement Type, Parking
			bodyText += "<tr>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Basement Type:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Parking:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += basementType + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += parking + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Amenities
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\">" + "\r";
			bodyText += "Amenities:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td colspan=\"2\">" + "\r";
			bodyText += amenities + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Other Features and Amenities
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\">" + "\r";
			bodyText += "Other Features and Amenities:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td colspan=\"2\">" + "\r";
			bodyText += otherFeatAmen + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Home Condition, Reason For Move
			bodyText += "<tr>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Home Condition:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Reason For Home Evaluation:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += homeCondition + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += reasonForEvaluation + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start How Soon Are You Moving
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\">" + "\r";
			bodyText += "How Soon Are You Moving?:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td colspan=\"2\">" + "\r";
			bodyText += howSoonMoving + "\r";
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

				// If successful redirect to thank you page
				Response.Redirect("raleigh-cary-home-value-estimate-thanks.aspx", true);
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