using System;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Quiksoft.EasyMail.SMTP;

namespace ChrisEdwardsGroup.Website
{
	/// <summary>
	/// Home search form.
	/// </summary>
	public class HomeSearch : Main
	{
		public HtmlGenericControl htmErrors;
		public DropDownList ddlMinPrice;
		public DropDownList ddlMaxPrice;
		public CheckBoxList chblAreaCity;
		public CheckBoxList chblNeighborhoodCommunity;
		public TextBox txtAppxLotSize;
		public TextBox txtAppxSqFt;
		public TextBox txtMinBedrooms;
		public TextBox txtMinBathrooms;
		public CheckBoxList chblHomeType;
		public CheckBoxList chblHomeStyle;
		public CheckBoxList chblBasementType;
		public CheckBoxList chblParking;
		public CheckBoxList chblAmenities;
		public RadioButtonList rblHomeCondition;
		public RadioButtonList rblReasonForMove;
		public RadioButtonList rblMoveInTimeframe;
		public RadioButtonList rblHomeLooking;
		public CheckBoxList chblShedule;
		public RadioButtonList rblScheduleMeeting;
		public TextBox txtComments;
		public TextBox txtFirstName;
		public TextBox txtLastName;
		public TextBox txtEmail;
		public TextBox txtTelephone;
		public DropDownList ddlContactMethod;
		public TextBox txtBestTimeContact;

		public void Page_Load(Object sender, EventArgs e)
		{
			// Check for Page PostBack
			if (!Page.IsPostBack)
			{
				Bind();
			}
		}

		private void Bind()
		{
			// Create string to hold path to price range config file
			string priceRangePath = Request.ApplicationPath + "includes/price_range2.xml";

			// Create DataSet Object
			DataSet ds = new DataSet();

			// Read XML Config File into DataSet
			ds.ReadXml(Request.MapPath(priceRangePath), XmlReadMode.InferSchema);

			// DataBind "MinPrice" DropDownList
			ddlMinPrice.DataSource = ds.Tables["MinRange"].DefaultView;
			ddlMinPrice.DataValueField = "MinRangeValue";
			ddlMinPrice.DataTextField = "MinRangeText";
			ddlMinPrice.DataBind();

			// Create minimum price list item
			ListItem minPrice = new ListItem("Select Minimum Price...", "0");
			// Insert list item as first item in list
			ddlMinPrice.Items.Insert(0, minPrice);
			// Select first item
			ddlMinPrice.SelectedIndex = 16;

			// DataBind "MaxPrice" DropDownList
			ddlMaxPrice.DataSource = ds.Tables["MaxRange"].DefaultView;
			ddlMaxPrice.DataValueField = "MaxRangeValue";
			ddlMaxPrice.DataTextField = "MaxRangeText";
			ddlMaxPrice.DataBind();

			// Create max price list item
			ListItem maxPrice = new ListItem("Select Maximum Price...", "0");
			// Insert list item as first item in list
			ddlMaxPrice.Items.Insert(0, maxPrice);
			// Select first item
			ddlMaxPrice.SelectedIndex = 20;

			// Create string to hold path to city config file
			string citiesPath = Request.ApplicationPath + "includes/cities.xml";

			// Read XML Config File into DataSet
			ds.ReadXml(Request.MapPath(citiesPath), XmlReadMode.InferSchema);

			// DataBind area/city CheckBoxList
			chblAreaCity.DataSource = ds.Tables["Cities"].DefaultView;
			chblAreaCity.DataTextField = "City";
			chblAreaCity.DataValueField = "City";
			chblAreaCity.DataBind();

			// Create string to hold path to neighborhoods config file
			string neighborhoodsPath = Request.ApplicationPath + "includes/neighborhoods.xml";

			// Read XML Config File into DataSet
			ds.ReadXml(Request.MapPath(neighborhoodsPath), XmlReadMode.InferSchema);

			// DataBind neighborhood/community CheckBoxList
			chblNeighborhoodCommunity.DataSource = ds.Tables["Neighborhoods"].DefaultView;
			chblNeighborhoodCommunity.DataTextField = "Neighborhood";
			chblNeighborhoodCommunity.DataValueField = "Neighborhood";
			chblNeighborhoodCommunity.DataBind();
		}

		public void Submit_OnClick(Object sender, EventArgs e)
		{
			// Send Home Search E-mail
			Send_HomeSearch();
		}

		public void Send_HomeSearch()
		{
			// Convert web form variables to local variables
			// so that they are easy to work with

			// First Name
			string firstName = txtFirstName.Text.Trim().ToString();

			// Last Name
			string lastName = txtLastName.Text.Trim().ToString();

			// E-mail Address
			string email = txtEmail.Text.Trim().ToString();

			string telephone = txtTelephone.Text.Trim().ToString();

			// Preferred contact method
			string contactMethod = ddlContactMethod.SelectedValue.ToString();

			// Best time to contact
			string bestTimeContact = txtBestTimeContact.Text.Trim().ToString();

			// Minimum purchase price
			double minPurchasePrice = Convert.ToDouble(ddlMinPrice.SelectedItem.Value.ToString());

			// Maximum purchase price
			double maxPurchasePrice = Convert.ToDouble(ddlMaxPrice.SelectedItem.Value.ToString());

			// Area/City
			StringBuilder areaCity;
			areaCity = CreateBulletList(chblAreaCity);

			// Neighborhood/Community
			StringBuilder neighComm;
			neighComm = CreateBulletList(chblNeighborhoodCommunity);

			// Approx. Lot Size
			string approxLotSize;
			approxLotSize = txtAppxLotSize.Text.ToString();

			// Approx. Sq. Ft.
			string approxSqFt = txtAppxSqFt.Text.ToString();

			// Min. Bedrooms
			string minBeds = txtMinBedrooms.Text.ToString();

			// Min. Bathrooms
			string minBaths = txtMinBathrooms.Text.ToString();

			// Home Type
			StringBuilder homeType;
			homeType = CreateBulletList(chblHomeType);

			// Home Style
			StringBuilder homeStyle;
			homeStyle = CreateBulletList(chblHomeStyle);

			// Basement Type
			StringBuilder basementType;
			basementType = CreateBulletList(chblBasementType);

			// Parking
			StringBuilder parking;
			parking = CreateBulletList(chblParking);

			// Amenities
			StringBuilder amenities;
			amenities = CreateBulletList(chblAmenities);

			// Retrieve home condition
			string homeCondition = rblHomeCondition.SelectedItem.Value.ToString();

			// Retrieve reason for move
			string reasonForMove = rblReasonForMove.SelectedItem.Value.ToString();

			// Retrieve move-in timeframe
			string moveInTimeframe = rblMoveInTimeframe.SelectedItem.Value.ToString();

			// How Long Looking
			string howLongLooking = rblHomeLooking.SelectedItem.Value.ToString();
			
			// Schedule
			StringBuilder schedule = CreateBulletList(chblShedule);
			
			// How Long Looking
			string scheduleMeeting = rblScheduleMeeting.SelectedItem.Value.ToString();

			// Comments
			string comments = txtComments.Text.Trim().ToString();

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
			emailMessage.Subject = "Raleigh-Cary Home Search.";

			string bodyText;

			// Start E-mail Body
			bodyText = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">" + "\r";
			bodyText += "<html>" + "\r";
			bodyText += "<head>" + "\r";
			bodyText += "<title>" + "\r";
			bodyText += "Raleigh-Cary Home Search" + "\r";
			bodyText += "</title>" + "\r";
			bodyText += "<link rel=\"stylesheet\" href=\"http://www.chrisedwardsgroup.com/includes/email.css\" type=\"text/css\">" + "\r";
			bodyText += "</head>" + "\r";
			bodyText += "<body>" + "\r";

			// Start Heading
			bodyText += "<h1>" + "\r";
			bodyText += "Raleigh-Cary Home Search" + "\r";
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
			bodyText += telephone + "\r";
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

			// Start Home Search Information Section
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\" class=\"formheading\">" + "\r";
			bodyText += "What Type of Home Are You Looking For?" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Minimum Price, Maximum Purchase Price
			bodyText += "<tr>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Minimum Purchase Price:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Maximum Purchase Price:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += minPurchasePrice.ToString("c") + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += maxPurchasePrice.ToString("c") + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Area/City, Community/Subdivision
			bodyText += "<tr>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Area/City:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Community/Subdivision:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += areaCity + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += neighComm + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Approx. Lot Size, Approx. Approx. Sq. Ft.
			bodyText += "<tr>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Approximate Lot Size:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Approximate Square Footage:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += approxLotSize + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += approxSqFt + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Min. Bedrooms, Min. Bathrooms
			bodyText += "<tr>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Minimum Number of Bedrooms:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Minimum Number of Bathrooms:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += minBeds + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += minBaths + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Home Type , Home Style
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

			// Start Home Condition, Reason For Move
			bodyText += "<tr>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Home Condition:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Reason For Move:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += homeCondition + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += reasonForMove + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Move-In Timeframe, How Long Looking
			bodyText += "<tr>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Move-In Timeframe:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "How Long Have You Been Looking:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += moveInTimeframe + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += howLongLooking + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";
			
			// Start Schedule
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\">" + "\r";
			bodyText += "I Would Like To Schedule:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td colspan=\"2\">" + "\r";
			bodyText += schedule + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";
			
			// Start Schedule Meeting
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\">" + "\r";
			bodyText += "When Would You Like To Schedule A Meeting?:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td colspan=\"2\">" + "\r";
			bodyText += scheduleMeeting + "\r";
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
				Response.Redirect("raleigh-cary-home-search-thanks.aspx", true);
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
			string firstName = txtFirstName.Text.Trim().ToString();

			// Last Name
			string lastName = txtLastName.Text.Trim().ToString();

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
			emailMessage.Subject = firstName + ", are you considering a move to North Carolina?";

			string bodyText;

			// Start E-mail Body
			bodyText = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">" + "\r";
			bodyText += "<html>" + "\r";
			bodyText += "<head>" + "\r";
			bodyText += "<title>" + "\r";
			bodyText += firstName + ", are you considering a move to North Carolina?" + "\r";
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
			bodyText += "Thank you for coming to my site to assist with your home search in North Carolina!" + "\r";
			bodyText += "</p>" + "\r";

			// Start Paragraph
			bodyText += "<p>" + "\r";
			bodyText += "If you have decided to move to this area or just considering a move, we are here to help. I am happy to provide complete relocation service to ensure your move is a smooth one. Detailed home search, buyer representation, excellent mover connections and lending assistance are just some of the ways we can greatly help! So many benefits at <strong>NO</strong> cost to you!" + "\r";
			bodyText += "</p>" + "\r";

			// Start Paragraph
			bodyText += "<p>" + "\r";
			bodyText += "I have started a personal home profile for you based on the information you already sent. Please take a moment to let us know how we can best assist you by responding to the questions below. This will help us to better serve you and understand your needs." + "\r";
			bodyText += "</p>" + "\r";

			// Start Ordered List
			bodyText += "<ol>" + "\r";
			bodyText += "<li>What brings you to the Raleigh area?</li>" + "\r";
			bodyText += "<li>Will a work commute time be an important factor?</li>" + "\r";
			bodyText += "<li>What is your timeframe to move to the area?</li>" + "\r";
			bodyText += "</ol>" + "\r";

			// Start Paragraph
			bodyText += "<p>" + "\r";
			bodyText += "I have developed one of the best real estate web sites in North Carolina that is focused on helping you in many ways with your move to this great area! Feel to call us with any questions you may have at <strong>1-888-828-0288</strong>! I hope to get the opportunity to help you with your move to this great area!" + "\r";
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
			bodyText += "<a href=\"http://www.chrisedwardsgroup.com/\"><img src=\"http://www.chrisedwardsgroup.com/images/chris-edwards-card.jpg\" width=\"350\" height=\"200\" border=\"1\" alt=\"Chris Edwards RE/MAX United\" /></a>";
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