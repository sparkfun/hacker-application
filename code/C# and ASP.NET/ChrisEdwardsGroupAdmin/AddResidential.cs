using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroupAdmin
{
	public class AddResidential : Main
	{
		public DropDownList ddlShowProperty;
		public HtmlGenericControl hgcErrors;
		public TextBox txtMLS;
		public TextBox txtAltMLS;
		public TextBox txtPrice;
		public TextBox txtOwner;
		public DropDownList ddlAgentID1;
		public DropDownList ddlAgentID2;
		public TextBox txtTagline;
		public TextBox txtAddress1;
		public TextBox txtAddress2;
		public DropDownList ddlCityID;
		public TextBox txtSubdivision;
		public TextBox txtAnnualTaxes;
		public TextBox txtAnnualTaxYear;
		public TextBox txtScheduleNumber;
		public TextBox txtBedrooms;
		public TextBox txtBaths;
		public TextBox txtSquareFt;
		public TextBox txtYearBuilt;
		public TextBox txtYearRemodeled;
		public TextBox txtParcelSize;
		public DropDownList ddlStyle;
		public DropDownList ddlFoundation;
		public DropDownList	ddlConstruction;
		public DropDownList ddlRoof;
		public DropDownList ddlGarage;
		public RadioButtonList rblPatio;
		public RadioButtonList rblDeck;
		public RadioButtonList rblFenced;
		public TextBox txtFencingDescription;
		public DropDownList ddlHeating;
		public RadioButtonList rblFireplace;
		public RadioButtonList rblWoodstove;
		public DropDownList ddlElectricityProvider;
		public TextBox txtElectricityMonthlyCost;
		public TextBox txtDomWaterProvider;
		public TextBox txtDomWaterMonthlyCost;
		public TextBox txtIrrWaterProvider;
		public TextBox txtIrrWaterShares;
		public TextBox txtIrrWaterMonthlyCost;
		public RadioButtonList rblSewer;
		public TextBox txtFencing;
		public TextBox txtKitchenDim;
		public TextBox txtLivingRoomDim;
		public TextBox txtDiningRoomDim;
		public TextBox txtFamilyRoomDim;
		public TextBox txtMasterBedDim;
		public TextBox txtBedroom2Dim;
		public TextBox txtBedroom3Dim;
		public TextBox txtBedroom4Dim;
		public TextBox txtBathroom1Dim;
		public TextBox txtBathroom2Dim;
		public TextBox txtBathroom3Dim;
		public TextBox txtBathroom4Dim;
		public TextBox txtBasementDim;
		public TextBox txtGarageDim;
		public TextBox txtPatioDim;
		public TextBox txtDeckDim;
		public TextBox txtShedDim;
		public TextBox txtOfficeDim;
		public TextBox txtMediaRoomDim;
		public TextBox txtLaundryRoomDim;
		public TextBox txtSunroomDim;
		public DropDownList ddlPossession;
		public TextBox txtEarnestMoney;
		public TextBox txtFeaturesDescription;
		public TextBox txtInclusionsDescription;
		public TextBox txtExclusionsDescription;
		public TextBox txtOutbuildingsDescription;
		public TextBox txtDisclosuresDescription;
		public TextBox txtMapDirections;

		public void Page_Load(Object Source, EventArgs E)
		{
			if (!Page.IsPostBack)
			{
				Bind();
			}
		}

		private void Bind()
		{
			// Create Connection String
			string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);

			// Create SqlCommand Object
			SqlCommand objCommand = new SqlCommand("sp_select_cities", objConnection);
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create SqlDataAdapter Object
			SqlDataAdapter sda = new SqlDataAdapter();

			// Create DataSet Object
			DataSet ds = new DataSet();

			try
			{
				sda.SelectCommand = objCommand;
				sda.Fill(ds, "Cities");
			}
			catch (Exception objError)
			{
				// display error details
				hgcErrors.Visible = true;
				hgcErrors.InnerText = "<b>* Error while accessing data</b>.<br />"
					+ objError.Message + "<br />" + objError.Source;
				return;  //  and stop execution
			}

			// DataBind "CityID" DropDownList
			ddlCityID.DataSource = ds.Tables["Cities"];
			ddlCityID.DataValueField = "CityID";
			ddlCityID.DataTextField = "CityName";
			ddlCityID.DataBind();

			// Create ListItem for "CityID" DropDownList
			ListItem selectCity = new ListItem("-- Select City --", "");
			// Insert ListItem Into "CityID" DropDownList
			ddlCityID.Items.Insert(0, selectCity);
			
			// Change Command Text to Fill Agents DataSet
			objCommand.CommandText = "sp_select_agents";

			try
			{
				sda.SelectCommand = objCommand;
				sda.Fill(ds, "Associates");
			}
			catch (Exception objError)
			{
				// display error details
				hgcErrors.Visible = true;
				hgcErrors.InnerText = "<b>* Error while accessing data</b>.<br />"
					+ objError.Message + "<br />" + objError.Source;
				return;  //  and stop execution
			}

			// DataBind "AgentID1" DropDownList
			ddlAgentID1.DataSource = ds.Tables["Associates"];
			ddlAgentID1.DataValueField = "AgentID";
			ddlAgentID1.DataTextField = "FullName";
			ddlAgentID1.DataBind();

			// Create ListItem for "AgentID1" DropDownList
			ListItem selectAgent = new ListItem("-- Select Agent --", "");
			// Insert ListItem into "AgentID1" DropDownList
			ddlAgentID1.Items.Insert(0, selectAgent);

			// DataBind "AgentID2" DropDownList
			ddlAgentID2.DataSource = ds.Tables["Associates"];
			ddlAgentID2.DataValueField = "AgentID";
			ddlAgentID2.DataTextField = "FullName";
			ddlAgentID2.DataBind();

			// Create ListItem for "AgentID2" DropDownList
			ListItem noneAgent = new ListItem("-- N/A --", "");
			// Insert ListItem into "AgentID2" DropDownList
			ddlAgentID2.Items.Insert(0, noneAgent);

			// Create String to Connect to Residential XML Config File
			string strVirtualPath = Request.ApplicationPath + "/config/residential_dropdown.xml";

			// Read XML Config File into DataSet
			ds.ReadXml(Request.MapPath(strVirtualPath), XmlReadMode.InferSchema);

			// DataBind "Style" DropDownList
			ddlStyle.DataSource = ds.Tables["Style"].DefaultView;
			ddlStyle.DataValueField = "StyleValue";
			ddlStyle.DataTextField = "StyleText";
			ddlStyle.DataBind();

			// DataBind "Foundation" DropDownList
			ddlFoundation.DataSource = ds.Tables["Foundation"].DefaultView;
			ddlFoundation.DataValueField = "FoundationValue";
			ddlFoundation.DataTextField = "FoundationText";
			ddlFoundation.DataBind();

			// DataBind "Construction" DropDownList
			ddlConstruction.DataSource = ds.Tables["Construction"].DefaultView;
			ddlConstruction.DataValueField = "ConstructionValue";
			ddlConstruction.DataTextField = "ConstructionText";
			ddlConstruction.DataBind();

			// DataBind "Roof" DropDownList
			ddlRoof.DataSource = ds.Tables["Roof"].DefaultView;
			ddlRoof.DataValueField = "RoofValue";
			ddlRoof.DataTextField = "RoofText";
			ddlRoof.DataBind();

			// DataBind "Garage" DropDownList
			ddlGarage.DataSource = ds.Tables["Garage"].DefaultView;
			ddlGarage.DataValueField = "GarageValue";
			ddlGarage.DataTextField = "GarageText";
			ddlGarage.DataBind();

			// DataBind "Heating" DropDownList
			ddlHeating.DataSource = ds.Tables["Heating"].DefaultView;
			ddlHeating.DataValueField = "HeatingValue";
			ddlHeating.DataTextField = "HeatingText";
			ddlHeating.DataBind();

			// DataBind "Possession" DropDownList
			ddlPossession.DataSource = ds.Tables["Possession"].DefaultView;
			ddlPossession.DataValueField = "PossessionValue";
			ddlPossession.DataTextField = "PossessionText";
			ddlPossession.DataBind();

			// DataBind "ElectricityProvider" DropDownList
			ddlElectricityProvider.DataSource = ds.Tables["ElectricityProviders"].DefaultView;
			ddlElectricityProvider.DataValueField = "ElectricityProviderValue";
			ddlElectricityProvider.DataTextField = "ElectricityProviderText";
			ddlElectricityProvider.DataBind();
		}

		public void AddResidential_Click(Object Source, EventArgs E)
		{
			// Create Connection String
			string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);
			
			// Create SqlCommand Object
			SqlCommand objCommand = new SqlCommand("sp_insert_residential", objConnection);
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create SqlParameter Object
			SqlParameter objParam;

			// Add "MLS" Parameter
			objParam = objCommand.Parameters.Add("@MLS", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(txtMLS.Text);

			if (CheckTextBox(txtAltMLS))
			{
				//Add "AltMLS" Parameter
				objParam = objCommand.Parameters.Add("@AltMLS", SqlDbType.Int); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToInt32(txtAltMLS.Text);
			}

			//Add "Price" Parameter
			objParam = objCommand.Parameters.Add("@Price", SqlDbType.Money); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToDouble(txtPrice.Text);

			//Add "Owner" Parameter
			objParam = objCommand.Parameters.Add("@Owner", SqlDbType.VarChar, 35); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtOwner.Text);

			//Add "AgentID1" Parameter
			objParam = objCommand.Parameters.Add("@AgentID1", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(ddlAgentID1.SelectedItem.Value);

			if (CheckDropDownList(ddlAgentID2))
			{
				//Add "AgentID2" Parameter
				objParam = objCommand.Parameters.Add("@AgentID2", SqlDbType.Int); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToInt32(ddlAgentID2.SelectedItem.Value);
			}

			if (CheckTextBox(txtTagline))
			{
				//Add "Tagline" Parameter
				objParam = objCommand.Parameters.Add("@Tagline", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtTagline.Text);
			}
		
			//Add "Address1" Parameter
			objParam = objCommand.Parameters.Add("@Address1", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtAddress1.Text);
		
			if (CheckTextBox(txtAddress2))
			{
				//Add "Address2" Parameter
				objParam = objCommand.Parameters.Add("@Address2", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtAddress2.Text);
			}

			//Add "CityID" Parameter
			objParam = objCommand.Parameters.Add("@CityID", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(ddlCityID.SelectedItem.Value);

			if (CheckTextBox(txtSubdivision))
			{
				//Add "Subdivision" Parameter
				objParam = objCommand.Parameters.Add("@Subdivision", SqlDbType.VarChar, 60); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtSubdivision.Text);
			}

			if (CheckTextBox(txtAnnualTaxes))
			{
				//Add "AnnualTaxes" Parameter
				objParam = objCommand.Parameters.Add("@AnnualTaxes", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtAnnualTaxes.Text);
			}

			if (CheckTextBox(txtAnnualTaxYear))
			{
				//Add "AnnualTaxYear" Parameter
				objParam = objCommand.Parameters.Add("@AnnualTaxYear", SqlDbType.Char, 4); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtAnnualTaxYear.Text);
			}

			if (CheckTextBox(txtScheduleNumber))
			{
				//Add "ScheduleNumber" Parameter
				objParam = objCommand.Parameters.Add("@ScheduleNumber", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtScheduleNumber.Text);
			}

			//Add "Bedrooms" Parameter
			objParam = objCommand.Parameters.Add("@Bedrooms", SqlDbType.Float); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToDouble(txtBedrooms.Text);

			//Add "Baths" Parameter
			objParam = objCommand.Parameters.Add("@Baths", SqlDbType.Float); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToDouble(txtBaths.Text);

			// Add "SquareFt" Parameter
			objParam = objCommand.Parameters.Add("@SquareFt", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(txtSquareFt.Text);

			// Add "YearBuilt" Parameter
			objParam = objCommand.Parameters.Add("@YearBuilt", SqlDbType.Char, 4); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtYearBuilt.Text);

			if (CheckTextBox(txtYearRemodeled))
			{
				//Add "YearRemodeled" Parameter
				objParam = objCommand.Parameters.Add("@YearRemodeled", SqlDbType.Char, 4); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtYearRemodeled.Text);
			}

			//Add "ParcelSize" Parameter
			objParam = objCommand.Parameters.Add("@ParcelSize", SqlDbType.VarChar, 30); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtParcelSize.Text);

			//Add "Style" Parameter
			objParam = objCommand.Parameters.Add("@Style", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddlStyle.SelectedItem.Value);

			//Add "Foundation" Parameter
			objParam = objCommand.Parameters.Add("@Foundation", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddlFoundation.SelectedItem.Value);

			//Add "Construction" Parameter
			objParam = objCommand.Parameters.Add("@Construction", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddlConstruction.SelectedItem.Value);

			//Add "Roof" Parameter
			objParam = objCommand.Parameters.Add("@Roof", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddlRoof.SelectedItem.Value);

			//Add "Garage" Parameter
			objParam = objCommand.Parameters.Add("@Garage", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddlGarage.SelectedItem.Value);

			//Add "Patio" Parameter
			objParam = objCommand.Parameters.Add("@Patio", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt16(rblPatio.SelectedItem.Value);

			//Add "Deck" Parameter
			objParam = objCommand.Parameters.Add("@Deck", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt16(rblDeck.SelectedItem.Value);

			//Add "Fenced" Parameter
			objParam = objCommand.Parameters.Add("@Fenced", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt16(rblFenced.SelectedItem.Value);

			if (CheckTextBox(txtFencingDescription))
			{
				//Add "FencingDescription" Parameter
				objParam = objCommand.Parameters.Add("@FencingDescription", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtFencingDescription.Text);
			}

			//Add "Heating" Parameter
			objParam = objCommand.Parameters.Add("@Heating", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddlHeating.SelectedItem.Value);

			//Add "Fireplace" Parameter
			objParam = objCommand.Parameters.Add("@Fireplace", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt16(rblFireplace.SelectedItem.Value);

			//Add "Woodstove" Parameter
			objParam = objCommand.Parameters.Add("@Woodstove", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt16(rblWoodstove.SelectedItem.Value);

			//Add "ElectricityProvider" Parameter
			objParam = objCommand.Parameters.Add("@ElectricityProvider", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddlElectricityProvider.SelectedItem.Value);

			if (CheckTextBox(txtElectricityMonthlyCost))
			{
				//Add "ElectricityMonthlyCost" Parameter
				objParam = objCommand.Parameters.Add("@ElectricityMonthlyCost", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDecimal(txtElectricityMonthlyCost.Text);
			}

			//Add "DomWaterProvider" Parameter
			objParam = objCommand.Parameters.Add("@DomWaterProvider", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtDomWaterProvider.Text);

			if (CheckTextBox(txtDomWaterMonthlyCost))
			{
				//Add "DomMonthlyWaterCost" Parameter
				objParam = objCommand.Parameters.Add("@DomWaterMonthlyCost", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtDomWaterMonthlyCost.Text);
			}

			if (CheckTextBox(txtIrrWaterProvider))
			{
				//Add "IrrWaterProvider" Parameter
				objParam = objCommand.Parameters.Add("@IrrWaterProvider", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtIrrWaterProvider.Text);
			}

			if (CheckTextBox(txtIrrWaterShares))
			{
				//Add "IrrWaterShares" Parameter
				objParam = objCommand.Parameters.Add("@IrrWaterShares", SqlDbType.Decimal); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDecimal(txtIrrWaterShares.Text);
			}

			if (CheckTextBox(txtIrrWaterMonthlyCost))
			{
				//Add "IrrWaterCost" Parameter
				objParam = objCommand.Parameters.Add("@IrrWaterMonthlyCost", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtIrrWaterMonthlyCost.Text);
			}

			//Add "Sewer" Parameter
			objParam = objCommand.Parameters.Add("@Sewer", SqlDbType.Char, 6);
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(rblSewer.SelectedItem.Value);
		
			if (CheckTextBox(txtKitchenDim))
			{
				//Add "KitchenDim" Parameter
				objParam = objCommand.Parameters.Add("@KitchenDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtKitchenDim.Text);
			}

			if (CheckTextBox(txtLivingRoomDim))
			{
				//Add "LivingRoomDim" Parameter
				objParam = objCommand.Parameters.Add("@LivingRoomDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtLivingRoomDim.Text);
			}

			if (CheckTextBox(txtDiningRoomDim))
			{
				//Add "DiningRoomDim" Parameter
				objParam = objCommand.Parameters.Add("@DiningRoomDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtDiningRoomDim.Text);
			}

			if (CheckTextBox(txtFamilyRoomDim))
			{
				//Add "FamilyRoomDim" Parameter
				objParam = objCommand.Parameters.Add("@FamilyRoomDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtFamilyRoomDim.Text);
			}

			if (CheckTextBox(txtMasterBedDim))
			{
				//Add "MasterBedDim" Parameter
				objParam = objCommand.Parameters.Add("@MasterBedDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtMasterBedDim.Text);
			}

			if (CheckTextBox(txtBedroom2Dim))
			{
				//Add "Bedroom2Dim" Parameter
				objParam = objCommand.Parameters.Add("@Bedroom2Dim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtBedroom2Dim.Text);
			}

			if (CheckTextBox(txtBedroom3Dim))
			{
				//Add "Bedroom3Dim" Parameter
				objParam = objCommand.Parameters.Add("@Bedroom3Dim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtBedroom3Dim.Text);
			}

			if (CheckTextBox(txtBedroom4Dim))
			{
				//Add "Bedroom4Dim" Parameter
				objParam = objCommand.Parameters.Add("@Bedroom4Dim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtBedroom4Dim.Text);
			}

			if (CheckTextBox(txtBathroom1Dim))
			{
				//Add "Bathroom1Dim" Parameter
				objParam = objCommand.Parameters.Add("@Bathroom1Dim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtBathroom1Dim.Text);
			}

			if (CheckTextBox(txtBathroom2Dim))
			{
				//Add "Bathroom2Dim" Parameter
				objParam = objCommand.Parameters.Add("@Bathroom2Dim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtBathroom2Dim.Text);
			}

			if (CheckTextBox(txtBathroom3Dim))
			{
				//Add "Bathroom3Dim" Parameter
				objParam = objCommand.Parameters.Add("@Bathroom3Dim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtBathroom3Dim.Text);
			}

			if (CheckTextBox(txtBathroom4Dim))
			{
				//Add "Bathroom4Dim" Parameter
				objParam = objCommand.Parameters.Add("@Bathroom4Dim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtBathroom4Dim.Text);
			}

			if (CheckTextBox(txtBasementDim))
			{
				//Add "BasementDim" Parameter
				objParam = objCommand.Parameters.Add("@BasementDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtBasementDim.Text);
			}

			if (CheckTextBox(txtGarageDim))
			{
				//Add "GarageDim" Parameter
				objParam = objCommand.Parameters.Add("@GarageDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtGarageDim.Text);
			}

			if (CheckTextBox(txtPatioDim))
			{
				//Add "PatioDim" Parameter
				objParam = objCommand.Parameters.Add("@PatioDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtPatioDim.Text);
			}

			if (CheckTextBox(txtDeckDim))
			{
				//Add "DeckDim" Parameter
				objParam = objCommand.Parameters.Add("@DeckDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtDeckDim.Text);
			}

			if (CheckTextBox(txtShedDim))
			{
				//Add "ShedDim" Parameter
				objParam = objCommand.Parameters.Add("@ShedDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtShedDim.Text);
			}

			if (CheckTextBox(txtOfficeDim))
			{
				//Add "OfficeDim" Parameter
				objParam = objCommand.Parameters.Add("@OfficeDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtOfficeDim.Text);
			}

			if (CheckTextBox(txtMediaRoomDim))
			{
				//Add "MediaRoomDim" Parameter
				objParam = objCommand.Parameters.Add("@MediaRoomDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtMediaRoomDim.Text);
			}

			if (CheckTextBox(txtLaundryRoomDim))
			{
				//Add "LaundryDim" Parameter
				objParam = objCommand.Parameters.Add("@LaundryRoomDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtLaundryRoomDim.Text);
			}

			if (CheckTextBox(txtSunroomDim))
			{
				//Add "LaundryDim" Parameter
				objParam = objCommand.Parameters.Add("@SunroomDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtSunroomDim.Text);
			}

			//Add "Possession" Parameter
			objParam = objCommand.Parameters.Add("@Possession", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddlPossession.SelectedItem.Text);

			//Add "EarnestMoney" Parameter
			objParam = objCommand.Parameters.Add("@EarnestMoney", SqlDbType.Money); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToDouble(txtEarnestMoney.Text);

			//Add "FeaturesDescription" Parameter
			objParam = objCommand.Parameters.Add("@FeaturesDescription", SqlDbType.VarChar, 750); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtFeaturesDescription.Text);

			//Add "InclusionsDescription" Parameter
			objParam = objCommand.Parameters.Add("@InclusionsDescription", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtInclusionsDescription.Text);

			//Add "ExclusionsDescription" Parameter
			objParam = objCommand.Parameters.Add("@ExclusionsDescription", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtExclusionsDescription.Text);

			if (CheckTextBox(txtOutbuildingsDescription))
			{
				//Add "OutbuildingsDescription" Parameter
				objParam = objCommand.Parameters.Add("@OutbuildingsDescription", SqlDbType.VarChar, 250); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtOutbuildingsDescription.Text);
			}

			if (CheckTextBox(txtDisclosuresDescription))
			{
				//Add "DisclosuresDescription" Parameter
				objParam = objCommand.Parameters.Add("@DisclosuresDescription", SqlDbType.VarChar, 250); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtDisclosuresDescription.Text);
			}

			if (CheckTextBox(txtMapDirections))
			{
				// Add "MapDirections" Parameter
				objParam = objCommand.Parameters.Add("@MapDirections", SqlDbType.VarChar, 250); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtMapDirections.Text);
			}

			// Add Database Record
			try
			{
				objConnection.Open();
				objCommand.ExecuteNonQuery();
				objConnection.Close();
			}
			catch (SqlException Ex)
			{
				if (Ex.Number == 2627)
				{
					hgcErrors.Visible = true;
					hgcErrors.InnerHtml = "Summary of Errors:";
					hgcErrors.InnerHtml += "<ul>" + "\n";
					hgcErrors.InnerHtml += "<li>The MLS number of the property" + 
						" you are entering already exists in the database.</li>" + "\n";
					hgcErrors.InnerHtml += "</ul>" + "\n";
					return;
				}
				else
				{
					hgcErrors.Visible = true;
					hgcErrors.InnerHtml = GetSqlExceptionDump(Ex);
					return;
				}
			}
			catch (Exception objError)
			{
				hgcErrors.Visible = true;
				hgcErrors.InnerHtml = "Summary of Errors:";
				hgcErrors.InnerHtml += "<ul>" + "\n";
				hgcErrors.InnerHtml += "<li>Error Message:" + objError.Message +
				"</li>" + "\n";
				hgcErrors.InnerHtml += "<li>Error Source:" + objError.Source +
				"</li>" + "\n";
				hgcErrors.InnerHtml += "</ul>" + "\n";
				return;
			}

			// Save MLS Value to Variable
			int mlsID;
			mlsID = Convert.ToInt32(txtMLS.Text);

			// Create Graphics Directory
			string dirPath;
			dirPath = @"D:\websites\chrisedwardsgroup\listings\" + mlsID.ToString() + @"\";

			// Check to See If Directory Exists
			// If Directory Does Not Exist Add New Directory
			if (!Directory.Exists(dirPath))
			{
				Directory.CreateDirectory(dirPath);
			}

			// Initialize MLS Session Cookie
			Session["mlsID"] = mlsID;

			// Redirect to Residential Management Page
			Response.Redirect("manage_residential.aspx");
		}
	}

}
