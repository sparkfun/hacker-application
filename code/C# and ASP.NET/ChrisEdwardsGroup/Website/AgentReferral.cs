using System;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Quiksoft.EasyMail.SMTP;

namespace ChrisEdwardsGroup.Website
{
	/// <summary>
	/// Form for other agents to refer clients.
	/// </summary>
	public class AgentReferral : Main
	{
		public HtmlGenericControl htmErrors;
		public TextBox txtClientFirstName;
		public TextBox txtClientLastName;
		public TextBox txtClientAddress1;
		public TextBox txtClientAddress2;
		public TextBox txtClientCity;
		public TextBox txtClientState;
		public TextBox txtClientZipCode;
		public TextBox txtClientEmail;
		public TextBox txtClientTelephone;
		public TextBox txtOtherClientInformation;
		public DropDownList ddlMinPrice;
		public DropDownList ddlMaxPrice;
		public CheckBoxList chblAreaCity;
		public CheckBoxList chblNeighborhoodCommunity;
		public TextBox txtAppxLotSize;
		public TextBox txtAppxSqFt;
		public TextBox txtMinBedrooms;
		public TextBox txtMinBathrooms;
		public TextBox txtOtherFeaturesAmenities;
		public RadioButtonList rblHomeCondition;
		public RadioButtonList rblReasonForMove;
		public RadioButtonList rblMoveInTimeframe;
		public RadioButtonList rblHomeLooking;
		public TextBox txtRealtorFirstName;
		public TextBox txtRealtorLastName;
		public TextBox txtRealtorCompanyName;
		public TextBox txtRealtorAddress1;
		public TextBox txtRealtorAddress2;
		public TextBox txtRealtorCity;
		public TextBox txtRealtorState;
		public TextBox txtRealtorZipCode;
		public TextBox txtRealtorEmail;
		public TextBox txtRealtorTelephone;
		protected DropDownList ddlRequestedFee;
		public TextBox txtOtherComments;

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
			// Create DataSet Object
			DataSet ds = new DataSet();

			// Create String to Connect to Residential XML Config File
			string strVirtualPath = Request.ApplicationPath + "includes/price_range2.xml";

			// Read XML Config File into DataSet
			ds.ReadXml(Request.MapPath(strVirtualPath), XmlReadMode.InferSchema);

			// DataBind "MinPrice" DropDownList
			ddlMinPrice.DataSource = ds.Tables["MinRange"].DefaultView;
			ddlMinPrice.DataValueField = "MinRangeValue";
			ddlMinPrice.DataTextField = "MinRangeText";
			ddlMinPrice.DataBind();

			// Select First Choice
			ddlMinPrice.SelectedIndex = 14;

			// DataBind "MaxPrice" DropDownList
			ddlMaxPrice.DataSource = ds.Tables["MaxRange"].DefaultView;
			ddlMaxPrice.DataValueField = "MaxRangeValue";
			ddlMaxPrice.DataTextField = "MaxRangeText";
			ddlMaxPrice.DataBind();

			// Select Choice
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
			// Send Realtor Referral E-mail
			SendMail_RealtorReferral();
		}

		public void SendMail_RealtorReferral()
		{
			// Convert web form variables to local variables
			// so that they are easy to work with

			// Client First Name
			string clientFirstName;
			clientFirstName = txtClientFirstName.Text.ToString();

			// Client Last Name
			string clientLastName;
			clientLastName = txtClientLastName.Text.ToString();

			// Client Address #1
			string clientAddress1;
			clientAddress1 = txtClientAddress1.Text.ToString();

			// Client Address #2
			string clientAddress2;
			clientAddress2 = txtClientAddress2.Text.ToString();

			// Client City
			string clientCity;
			clientCity = txtClientCity.Text.ToString();

			// Client State
			string clientState;
			clientState = txtClientState.Text.ToString();

			// Client Zip Code
			string clientZipCode;
			clientZipCode = txtClientZipCode.Text.ToString();

			// Client E-mail Address
			string clientEmail;
			clientEmail = txtClientEmail.Text.ToString();

			// Client Telephone
			string clientTelephone = txtClientTelephone.Text.Trim().ToString();

			// Other Client Information
			string otherClientInformation;
			otherClientInformation = txtOtherClientInformation.Text.ToString();

			// Minimum Purchase Price
			double minPurchasePrice;
			minPurchasePrice = Convert.ToDouble(ddlMinPrice.SelectedItem.Value.ToString());

			// Maximum Purchase Price
			double maxPurchasePrice;
			maxPurchasePrice = Convert.ToDouble(ddlMaxPrice.SelectedItem.Value.ToString());

			// Area/City
			StringBuilder areaCity = CreateBulletList(chblAreaCity);

			// Neighborhood/Community
			StringBuilder neighComm = CreateBulletList(chblNeighborhoodCommunity);

			// Approx. Lot Size
			string approxLotSize;
			approxLotSize = txtAppxLotSize.Text.ToString();

			// Approx. Sq. Ft.
			string approxSqFt;
			approxSqFt = txtAppxSqFt.Text.ToString();

			// Min. Bedrooms
			string minBeds;
			minBeds = txtMinBedrooms.Text.ToString();

			// Min. Bathrooms
			string minBaths;
			minBaths = txtMinBathrooms.Text.ToString();

			// Other Features and Amenities
			string otherFeatAmen;
			otherFeatAmen = txtOtherFeaturesAmenities.Text.ToString();

			// Home Condition
			string homeCondition;
			homeCondition = rblHomeCondition.SelectedItem.Value.ToString();

			// Reason For Move
			string reasonForMove;
			reasonForMove = rblReasonForMove.SelectedItem.Value.ToString();

			// Move-In Timeframe
			string moveInTimeframe;
			moveInTimeframe = rblMoveInTimeframe.SelectedItem.Value.ToString();

			// How Long Looking
			string howLongLooking;
			howLongLooking = rblHomeLooking.SelectedItem.Value.ToString();

			// Realtor First Name
			string realtorFirstName;
			realtorFirstName = txtRealtorFirstName.Text.ToString();

			// Realtor Last Name
			string realtorLastName;
			realtorLastName = txtRealtorLastName.Text.ToString();

			// Realtor Company
			string realtorCompany;
			realtorCompany = txtRealtorCompanyName.Text.ToString();

			// Realtor Address #1
			string realtorAddress1;
			realtorAddress1 = txtRealtorAddress1.Text.ToString();

			// Realtor Address #2
			string realtorAddress2;
			realtorAddress2 = txtRealtorAddress2.Text.ToString();

			// Realtor City
			string realtorCity;
			realtorCity = txtRealtorCity.Text.ToString();

			// Realtor State
			string realtorState;
			realtorState = txtRealtorState.Text.ToString();

			// Realtor Zip Code
			string realtorZipCode;
			realtorZipCode = txtRealtorZipCode.Text.ToString();

			// Realtor E-mail Address
			string realtorEmail;
			realtorEmail = txtRealtorEmail.Text.ToString();

			// Realtor Telephone
			string realtorTelephone = txtRealtorTelephone.Text.Trim().ToString();
			
			// Request Realtor Fee
			string requestedFee = ddlRequestedFee.SelectedItem.Value;

			// Other Comments
			string otherComments;
			otherComments = txtOtherComments.Text.ToString();

			// Create the EmailMessage object
			EmailMessage em = new EmailMessage();
  
			// Specify From Address and Display Name
			em.From.Email = realtorEmail;
			em.From.Name = realtorFirstName + " " + realtorLastName;

			// Add Chris Edwards Recipient
			em.Recipients.Add("chris@chrisedwardsgroup.com", "Chris Edwards", RecipientType.To);
 
			// Add Bill Smith Recipient
			em.Recipients.Add("info@firestardesign.com", "Bill Smith", RecipientType.BCC);

			// Add Sean Smith Recipient
			em.Recipients.Add("sean@firestardesign.com", "Sean Smith", RecipientType.BCC);
 
			// Specify the subject
			em.Subject = "Chris Edwards Agent Referral Form.";

			string bodyText;

			// Start E-mail Body
			bodyText = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">" + "\r";
			bodyText += "<html>" + "\r";
			bodyText += "<head>" + "\r";
			bodyText += "<title>" + "\r";
			bodyText += "Agent Referral Form" + "\r";
			bodyText += "</title>" + "\r";
			bodyText += "<link rel=\"stylesheet\" href=\"http://www.chrisedwardsgroup.com/includes/email.css\" type=\"text/css\">" + "\r";
			bodyText += "</head>" + "\r";
			bodyText += "<body>" + "\r";

			// Start Heading
			bodyText += "<h1>" + "\r";
			bodyText += "Agent Referral Form" + "\r";
			bodyText += "</h1>" + "\r";

			// Start Table
			bodyText += "<table>" + "\r";
			bodyText += "<tbody>" + "\r";
			
			// Start Client Information Section
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\" class=\"formheading\">" + "\r";
			bodyText += "Client Information" + "\r";
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
			bodyText += clientFirstName + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += clientLastName + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Address #1, Address #2
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
			bodyText += clientAddress1 + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += clientAddress2 + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start City
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\">" + "\r";
			bodyText += "City:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td colspan=\"2\">" + "\r";
			bodyText += clientCity + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start State, Zip Code
			bodyText += "<tr>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "State:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Zip Code:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += clientState + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += clientZipCode + "\r";
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
			bodyText += "<a href=\"mailto:" + clientEmail +
				"\">" + clientEmail + "</a>" + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += clientTelephone + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Other Client Information
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\">" + "\r";
			bodyText += "Other Client Information:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td colspan=\"2\">" + "\r";
			bodyText += otherClientInformation + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// End Table
			bodyText += "</tbody>" + "\r";
			bodyText += "</table>" + "\r";

			// Start Table
			bodyText += "<table>" + "\r";
			bodyText += "<tbody>" + "\r";

			// Start Client Home Search Information Section
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\" class=\"formheading\">" + "\r";
			bodyText += "What Type of Home Is Your Client Looking For?" + "\r";
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

			// End Table
			bodyText += "</tbody>" + "\r";
			bodyText += "</table>" + "\r";

			// Start Table
			bodyText += "<table>" + "\r";
			bodyText += "<tbody>" + "\r";
			
			// Start Realtor Information Section
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\" class=\"formheading\">" + "\r";
			bodyText += "Your Realtor Information" + "\r";
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
			bodyText += realtorFirstName + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += realtorLastName + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Company Information
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\">" + "\r";
			bodyText += "Company:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td colspan=\"2\">" + "\r";
			bodyText += realtorCompany + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Address #1, Address #2
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
			bodyText += realtorAddress1 + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += realtorAddress2 + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start City
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\">" + "\r";
			bodyText += "City:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td colspan=\"2\">" + "\r";
			bodyText += realtorCity + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start State, Zip Code
			bodyText += "<tr>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "State:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "<th>" + "\r";
			bodyText += "Zip Code:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += realtorState + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += realtorZipCode + "\r";
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
			bodyText += "<a href=\"mailto:" + realtorEmail +
				"\">" + realtorEmail + "</a>" + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "<td>" + "\r";
			bodyText += realtorTelephone + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";
			
			// Start Requested Realtor Fee
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\">" + "\r";
			bodyText += "Requested Referral Fee:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td colspan=\"2\">" + "\r";
			bodyText += requestedFee + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// Start Other Comments
			bodyText += "<tr>" + "\r";
			bodyText += "<th colspan=\"2\">" + "\r";
			bodyText += "Other Comments:" + "\r";
			bodyText += "</th>" + "\r";
			bodyText += "</tr>" + "\r";
			bodyText += "<tr>" + "\r";
			bodyText += "<td colspan=\"2\">" + "\r";
			bodyText += otherComments + "\r";
			bodyText += "</td>" + "\r";
			bodyText += "</tr>" + "\r";

			// End Table
			bodyText += "</tbody>" + "\r";
			bodyText += "</table>" + "\r";

			// Start Table
			bodyText += "<table>" + "\r";
			bodyText += "<tbody>" + "\r";

			// End HTML Body
			bodyText += "</body>" + "\r";
			bodyText += "</html>" + "\r";

			// Add an HTML body part
			em.BodyParts.Add(bodyText, BodyPartFormat.HTML);
 
			// Create the SMTP object and set the mail server.
			SMTP smtp = new SMTP();
			smtp.SMTPServers.Add(GetMailServer());

			try
			{
				// Send the message
				smtp.Send(em);

				// If successful redirect to thank you page
				Response.Redirect("realtor-referral-thanks.aspx", true);
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