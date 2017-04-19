using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroupAdmin
{
	/// <summary>
	/// Summary description for AddCommercialRealEstate.
	/// </summary>
	public class AddCommercialRealEstate : Main
	{
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
		public TextBox txtRPAnnualTaxes;
		public TextBox txtRPAnnualTaxYear;
		public TextBox txtRPScheduleNumber;
		public TextBox txtPPAnnualTaxes;
		public TextBox txtPPAnnualTaxYear;
		public TextBox txtPPScheduleNumber;
		public TextBox txtPropertyDescription;
		public TextBox txtBuildingLayoutDescription;
		public TextBox txtFixturesFurnitureEquipment;
		public TextBox txtFrontage;
		public TextBox txtParking;
		public TextBox txtTotalSquareFt;
		public TextBox txtLandSize;
		public TextBox txtYearBuilt;
		public TextBox txtYearRemodeled;
		public DropDownList ddlStyle;
		public DropDownList ddlFoundation;
		public DropDownList ddlConstruction;
		public DropDownList ddlRoof;
		public DropDownList ddlHeating;
		public DropDownList ddlElectricityProvider;
		public TextBox txtDomWaterProvider;
		public DropDownList ddlNaturalGasProvider;
		public RadioButtonList rblSewerSeptic;
		public DropDownList ddlPossession;
		public DropDownList ddlTerms;
		public TextBox txtFeatures;
		public TextBox txtInclusions;
		public TextBox txtExclusions;
		public TextBox txtDisclosures;
		public TextBox txtMapDirections;

		public AddCommercialRealEstate()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public void Page_Load(Object sender, EventArgs e)
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
			string strVirtualPath = Request.ApplicationPath + "/config/commercial_re.xml";

			// Read XML Config File into DataSet
			ds.ReadXml(Request.MapPath(strVirtualPath), XmlReadMode.InferSchema);

			// DataBind "Style" DropDownList
			ddlStyle.DataSource = ds.Tables["Style"].DefaultView;
			ddlStyle.DataValueField = "StyleValue";
			ddlStyle.DataTextField = "StyleValue";
			ddlStyle.DataBind();

			// Add ListItem For "Style" DropDownList
			ListItem selectStyle = new ListItem("-- Select Style --", "");
			// Insert ListItem Into "Style" DropDownList
			ddlStyle.Items.Insert(0, selectStyle);
			// Set SelectedIndex
			ddlStyle.SelectedIndex = 0;

			// DataBind "Foundation" DropDownList
			ddlFoundation.DataSource = ds.Tables["Foundation"].DefaultView;
			ddlFoundation.DataValueField = "FoundationValue";
			ddlFoundation.DataTextField = "FoundationValue";
			ddlFoundation.DataBind();

			// Add ListItem For "Foundation" DropDownList
			ListItem selectFoundation = new ListItem("-- Select Foundation --", "");
			// Insert ListItem Into "Foundation" DropDownList
			ddlFoundation.Items.Insert(0, selectFoundation);
			// Set SelectedIndex
			ddlFoundation.SelectedIndex = 0;

			// DataBind "Construction" DropDownList
			ddlConstruction.DataSource = ds.Tables["Construction"].DefaultView;
			ddlConstruction.DataValueField = "ConstructionValue";
			ddlConstruction.DataTextField = "ConstructionValue";
			ddlConstruction.DataBind();

			// Add ListItem For "Construction" DropDownList
			ListItem selectConstruction = new ListItem("-- Select Construction --", "");
			// Insert ListItem Into "Construction" DropDownList
			ddlConstruction.Items.Insert(0, selectConstruction);
			// Set SelectedIndex
			ddlConstruction.SelectedIndex = 0;

			// DataBind "Roof" DropDownList
			ddlRoof.DataSource = ds.Tables["Roof"].DefaultView;
			ddlRoof.DataValueField = "RoofValue";
			ddlRoof.DataTextField = "RoofValue";
			ddlRoof.DataBind();

			// Add ListItem For "Roof" DropDownList
			ListItem selectRoof = new ListItem("-- Select Roof --", "");
			// Insert ListItem Into "Roof" DropDownList
			ddlRoof.Items.Insert(0, selectRoof);
			// Set SelectedIndex
			ddlRoof.SelectedIndex = 0;

			// DataBind "Heating" DropDownList
			ddlHeating.DataSource = ds.Tables["Heating"].DefaultView;
			ddlHeating.DataValueField = "HeatingValue";
			ddlHeating.DataTextField = "HeatingValue";
			ddlHeating.DataBind();

			// Add ListItem For "Heating" DropDownList
			ListItem selectHeating = new ListItem("-- Select Heating --", "");
			// Insert ListItem Into "Heating" DropDownList
			ddlHeating.Items.Insert(0, selectHeating);
			// Set SelectedIndex
			ddlHeating.SelectedIndex = 0;

			// DataBind "ElectricityProvider" DropDownList
			ddlElectricityProvider.DataSource = ds.Tables["ElectricityProvider"].DefaultView;
			ddlElectricityProvider.DataValueField = "ElectricityProviderValue";
			ddlElectricityProvider.DataTextField = "ElectricityProviderValue";
			ddlElectricityProvider.DataBind();

			// Add ListItem For "ElectricityProvider" DropDownList
			ListItem selectElectricityProvider = new ListItem("-- Select Electricity Provider --", "");
			// Insert ListItem Into "ElectricityProvider" DropDownList
			ddlElectricityProvider.Items.Insert(0, selectElectricityProvider);
			// Set SelectedIndex
			ddlElectricityProvider.SelectedIndex = 0;

			// DataBind "NaturalGasProvider" DropDownList
			ddlNaturalGasProvider.DataSource = ds.Tables["NaturalGasProvider"].DefaultView;
			ddlNaturalGasProvider.DataValueField = "NaturalGasProviderValue";
			ddlNaturalGasProvider.DataTextField = "NaturalGasProviderValue";
			ddlNaturalGasProvider.DataBind();

			// Add ListItem For "NaturalGasProvider" DropDownList
			ListItem selectNaturalGasProvider = new ListItem("-- Select Natural Gas Provider --", "");
			// Insert ListItem Into "NaturalGasProvider" DropDownList
			ddlNaturalGasProvider.Items.Insert(0, selectNaturalGasProvider);
			// Set SelectedIndex
			ddlNaturalGasProvider.SelectedIndex = 0;

			// DataBind "Possession" DropDownList
			ddlPossession.DataSource = ds.Tables["Possession"].DefaultView;
			ddlPossession.DataValueField = "PossessionValue";
			ddlPossession.DataTextField = "PossessionValue";
			ddlPossession.DataBind();

			// Add ListItem For "Possession" DropDownList
			ListItem selectPossession = new ListItem("-- Select Possession --", "");
			// Insert ListItem Into "Possession" DropDownList
			ddlPossession.Items.Insert(0, selectPossession);
			// Set SelectedIndex
			ddlPossession.SelectedIndex = 0;

			// DataBind "Terms" DropDownList
			ddlTerms.DataSource = ds.Tables["Terms"].DefaultView;
			ddlTerms.DataValueField = "TermsValue";
			ddlTerms.DataTextField = "TermsValue";
			ddlTerms.DataBind();

			// Add ListItem For "Terms" DropDownList
			ListItem selectTerms = new ListItem("-- Select Terms --", "");
			// Insert ListItem Into "Terms" DropDownList
			ddlTerms.Items.Insert(0, selectTerms);
			// Set SelectedIndex
			ddlTerms.SelectedIndex = 0;
		}

		public void AddCommercialRealEstate_Click(Object Source, EventArgs E)
		{
			// Create Connection String
			string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);
			
			// Create SqlCommand Object
			SqlCommand objCommand = new SqlCommand("sp_insert_commercial_re", objConnection);
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

			if (CheckTextBox(txtRPAnnualTaxes))
			{
				//Add "RPAnnualTaxes" Parameter
				objParam = objCommand.Parameters.Add("@RPAnnualTaxes", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtRPAnnualTaxes.Text);
			}

			if (CheckTextBox(txtRPAnnualTaxYear))
			{
				//Add "RPAnnualTaxYear" Parameter
				objParam = objCommand.Parameters.Add("@RPAnnualTaxYear", SqlDbType.Char, 4); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtRPAnnualTaxYear.Text);
			}

			if (CheckTextBox(txtRPScheduleNumber))
			{
				//Add "RPScheduleNumber" Parameter
				objParam = objCommand.Parameters.Add("@RPScheduleNumber", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtRPScheduleNumber.Text);
			}

			if (CheckTextBox(txtPPAnnualTaxes))
			{
				//Add "PPAnnualTaxes" Parameter
				objParam = objCommand.Parameters.Add("@PPAnnualTaxes", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtPPAnnualTaxes.Text);
			}

			if (CheckTextBox(txtPPAnnualTaxYear))
			{
				// Add "PPAnnualTaxYear" Parameter
				objParam = objCommand.Parameters.Add("@PPAnnualTaxYear", SqlDbType.Char, 4); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtPPAnnualTaxYear.Text);
			}

			if (CheckTextBox(txtPPScheduleNumber))
			{
				// Add "PPScheduleNumber" Parameter
				objParam = objCommand.Parameters.Add("@PPScheduleNumber", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtPPScheduleNumber.Text);
			}

			// Add "PropertyDescription" Parameter
			objParam = objCommand.Parameters.Add("@PropertyDescription", SqlDbType.VarChar, 500); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtPropertyDescription.Text);

			if (CheckTextBox(txtBuildingLayoutDescription))
			{
				// Add "BuildingLayoutDescription" Parameter
				objParam = objCommand.Parameters.Add("@BuildingLayoutDescription", SqlDbType.VarChar, 500); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtBuildingLayoutDescription.Text);
			}

			// Add "FixturesFurnitureEquipment" Parameter
			objParam = objCommand.Parameters.Add("@FixturesFurnitureEquipment", SqlDbType.VarChar, 500); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtFixturesFurnitureEquipment.Text);

			if (CheckTextBox(txtFrontage))
			{
				// Add "Frontage" Parameter
				objParam = objCommand.Parameters.Add("@Frontage", SqlDbType.VarChar, 250); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtFrontage.Text);
			}

			if (CheckTextBox(txtParking))
			{
				// Add "Parking" Parameter
				objParam = objCommand.Parameters.Add("@Parking", SqlDbType.VarChar, 250); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtParking.Text);
			}

			// Add "TotalSquareFt" Parameter
			objParam = objCommand.Parameters.Add("@TotalSquareFt", SqlDbType.Float); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToDouble(txtTotalSquareFt.Text);

			// Add "LandSize" Parameter
			objParam = objCommand.Parameters.Add("@LandSize", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtLandSize.Text);

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

			// Add "Style" Parameter
			objParam = objCommand.Parameters.Add("@Style", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddlStyle.SelectedItem.Value);

			// Add "Foundation" Parameter
			objParam = objCommand.Parameters.Add("@Foundation", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddlFoundation.SelectedItem.Value);

			// Add "Construction" Parameter
			objParam = objCommand.Parameters.Add("@Construction", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddlConstruction.SelectedItem.Value);

			// Add "Roof" Parameter
			objParam = objCommand.Parameters.Add("@Roof", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddlRoof.SelectedItem.Value);

			// Add "Heating" Parameter
			objParam = objCommand.Parameters.Add("@Heating", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddlHeating.SelectedItem.Value);

			// Add "ElectricityProvider" Parameter
			objParam = objCommand.Parameters.Add("@ElectricityProvider", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddlElectricityProvider.SelectedItem.Value);

			// Add "DomWaterProvider" Parameter
			objParam = objCommand.Parameters.Add("@DomWaterProvider", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtDomWaterProvider.Text);

			// Add "NaturalGasProvider" Parameter
			objParam = objCommand.Parameters.Add("@NaturalGasProvider", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddlNaturalGasProvider.SelectedItem.Value);

			// Add "SewerSeptic" Parameter
			objParam = objCommand.Parameters.Add("@SewerSeptic", SqlDbType.VarChar, 20);
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(rblSewerSeptic.SelectedItem.Value);

			// Add "Possession" Parameter
			objParam = objCommand.Parameters.Add("@Possession", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddlPossession.SelectedItem.Text);

			// Add "Terms" Parameter
			objParam = objCommand.Parameters.Add("@Terms", SqlDbType.VarChar, 30); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddlTerms.SelectedItem.Text);

			// Add "Features" Parameter
			objParam = objCommand.Parameters.Add("@Features", SqlDbType.VarChar, 750); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtFeatures.Text);

			// Add "Inclusions" Parameter
			objParam = objCommand.Parameters.Add("@Inclusions", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtInclusions.Text);

			// Add "Exclusions" Parameter
			objParam = objCommand.Parameters.Add("@Exclusions", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtExclusions.Text);

			if (CheckTextBox(txtDisclosures))
			{
				// Add "Disclosures" Parameter
				objParam = objCommand.Parameters.Add("@Disclosures", SqlDbType.VarChar, 250); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtDisclosures.Text);
			}

			if (CheckTextBox(txtMapDirections))
			{
				// Add "MapDirections" Parameter
				objParam = objCommand.Parameters.Add("@MapDirections", SqlDbType.VarChar, 250); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtMapDirections.Text);
			}

			// Add Database Record
			// Catch and Report Any Errors
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
			Response.Redirect("manage_commercial_re.aspx");
		}
	}
}
