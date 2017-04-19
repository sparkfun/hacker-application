using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroupAdmin
{
	public class AddVacantLand : Main
	{
		public HtmlGenericControl hgcErrors;
		public TextBox txtMLS;
		public TextBox txtAltMLS;
		public TextBox txtPrice;
		public DropDownList ddlAgentID1;
		public DropDownList ddlAgentID2;
		public TextBox txtOwner;
		public TextBox txtTagline;
		public TextBox txtAddress1;
		public TextBox txtAddress2;
		public DropDownList ddlCityID;
		public TextBox txtSubdivision;
		public TextBox txtAnnualTaxes;
		public TextBox txtAnnualTaxYear;
		public TextBox txtScheduleNumber;
		public TextBox txtAssessments;
		public TextBox txtParcelSize;
		public RadioButtonList rblCovenants;
		public RadioButtonList rblFenced;
		public TextBox txtFencingDescription;
		public TextBox txtAccessDescription;
		public TextBox txtTopography;
		public TextBox txtEasements;
		public RadioButtonList rblDomWaterAvailable;
		public TextBox txtDomWaterProvider;
		public RadioButtonList rblIrrWaterAvailable;
		public TextBox txtIrrWaterProvider;
		public TextBox txtIrrWaterDescription;
		public TextBox txtMineralRightsDescription;
		public RadioButtonList rblElectricityAvailable;
		public DropDownList ddlElectricityProvider;
		public RadioButtonList rblNaturalGasAvailable;
		public DropDownList ddlNaturalGasProvider;
		public RadioButtonList rblSewerInstalled;
		public RadioButtonList rblSewer;
		public CheckBox chbPropane;
		public CheckBox chbTelephone;
		public DropDownList ddlPossession;
		public TextBox txtEarnestMoney;
		public TextBox txtFeaturesDescription;
		public TextBox txtDisclosuresDescription;
		public TextBox txtMapDirections;
		
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
			string strVirtualPath = Request.ApplicationPath + "/config/vacantland_dropdown.xml";

			// Read XML Config File into DataSet
			ds.ReadXml(Request.MapPath(strVirtualPath), XmlReadMode.InferSchema);

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

			// Select "No" Value on "Covenants" RadioButtonList
			rblCovenants.SelectedIndex = 1;

			// Select "No" Value on "Fenced" RadioButtonList
			rblFenced.SelectedIndex = 1;

			// Select "No" Value on "DomWaterAvailable" RadioButtonList
			rblDomWaterAvailable.SelectedIndex = 1;

			// Select "No" Value on "IrrWaterAvailable" RadioButtonList
			rblIrrWaterAvailable.SelectedIndex = 1;

			// Select "No" Value on "ElectricityAvailable" RadioButtonList
			rblElectricityAvailable.SelectedIndex = 1;

			// Select "No" Value on "NaturalGasAvailable" RadioButtonList
			rblNaturalGasAvailable.SelectedIndex = 1;

			// Select "No" Value on "SewerInstalled" RadioButtonList
			rblSewerInstalled.SelectedIndex = 1;

			// Select "Not Installed" Value on "Sewer" RadioButtonList
			rblSewer.SelectedIndex = 2;
		}

		public void AddVacantLand_Click(Object Source, EventArgs E)
		{
			// Create Connection String
			string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);

			// Create SqlCommand
			SqlCommand objCommand = new SqlCommand("sp_insert_vacantland", objConnection);
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create SqlParameter
			SqlParameter objParam;

			// Add "MLS" Parameter
			objParam = objCommand.Parameters.Add("@MLS", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(txtMLS.Text);

			if (CheckTextBox(txtAltMLS))
			{
				// Add "AltMLS" Parameter
				objParam = objCommand.Parameters.Add("@AltMLS", SqlDbType.Int); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToInt32(txtAltMLS.Text);
			}

			// Add "Price" Parameter
			objParam = objCommand.Parameters.Add("@Price", SqlDbType.Money); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToDouble(txtPrice.Text);

			// Add "AgentID1" Parameter
			objParam = objCommand.Parameters.Add("@AgentID1", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(ddlAgentID1.SelectedItem.Value);

			if (CheckDropDownList(ddlAgentID2))
			{
				// Add "AgentID2" Parameter
				objParam = objCommand.Parameters.Add("@AgentID2", SqlDbType.Int); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToInt32(ddlAgentID2.SelectedItem.Value);
			}

			// Add "Owner" Parameter
			objParam = objCommand.Parameters.Add("@Owner", SqlDbType.VarChar, 35); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtOwner.Text);

			// Add "Tagline" Parameter
			objParam = objCommand.Parameters.Add("@Tagline", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtTagline.Text);
		
			// Add "Address1" Parameter
			objParam = objCommand.Parameters.Add("@Address1", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtAddress1.Text);
		
			if (CheckTextBox(txtAddress2))
			{
				// Add "Address2" Parameter
				objParam = objCommand.Parameters.Add("@Address2", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtAddress2.Text);
			}

			// Add "CityID" Parameter
			objParam = objCommand.Parameters.Add("@CityID", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(ddlCityID.SelectedItem.Value);

			if (CheckTextBox(txtSubdivision))
			{
				// Add "Subdivision" Parameter
				objParam = objCommand.Parameters.Add("@Subdivision", SqlDbType.VarChar, 60); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtSubdivision.Text);
			}

			if (CheckTextBox(txtAnnualTaxes))
			{
				// Add "AnnualTaxes" Parameter
				objParam = objCommand.Parameters.Add("@AnnualTaxes", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtAnnualTaxes.Text);
			}

			if (CheckTextBox(txtAnnualTaxYear))
			{
				// Add "AnnualTaxYear" Parameter
				objParam = objCommand.Parameters.Add("@AnnualTaxYear", SqlDbType.Char, 4); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtAnnualTaxYear.Text);
			}

			if (CheckTextBox(txtScheduleNumber))
			{
				// Add "ScheduleNumber" Parameter
				objParam = objCommand.Parameters.Add("@ScheduleNumber", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtScheduleNumber.Text);
			}

			if (CheckTextBox(txtAssessments))
			{
				// Add "Assessments" Parameter
				objParam = objCommand.Parameters.Add("@Assessments", SqlDbType.VarChar, 100); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtAssessments.Text);
			}

			// Add "ParcelSize" Parameter
			objParam = objCommand.Parameters.Add("@ParcelSize", SqlDbType.VarChar, 30); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtParcelSize.Text);

			// Add "Covenants" Parameter
			objParam = objCommand.Parameters.Add("@Covenants", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt16(rblCovenants.SelectedItem.Value);

			// Add "Fenced" Parameter
			objParam = objCommand.Parameters.Add("@Fenced", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt16(rblFenced.SelectedItem.Value);

			// Add "FencingDescription" Parameter
			objParam = objCommand.Parameters.Add("@FencingDescription", SqlDbType.VarChar, 150); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtFencingDescription.Text);

			// Add "AccessDescription" Parameter
			objParam = objCommand.Parameters.Add("@AccessDescription", SqlDbType.VarChar, 150); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtAccessDescription.Text);

			if (CheckTextBox(txtTopography))
			{
				// Add "Topography" Parameter
				objParam = objCommand.Parameters.Add("@Topography", SqlDbType.VarChar, 150); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtTopography.Text);
			}

			// Add "Easements" Parameter
			objParam = objCommand.Parameters.Add("@Easements", SqlDbType.VarChar, 60); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtEasements.Text);

			// Add "DomWaterAvailable" Parameter
			objParam = objCommand.Parameters.Add("@DomWaterAvailable", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt16(rblDomWaterAvailable.SelectedItem.Value);

			if (CheckTextBox(txtDomWaterProvider))
			{
				// Add "DomWaterProvider" Parameter
				objParam = objCommand.Parameters.Add("@DomWaterProvider", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtDomWaterProvider.Text);
			}

			// Add "IrrWaterAvailable" Parameter
			objParam = objCommand.Parameters.Add("@IrrWaterAvailable", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt16(rblIrrWaterAvailable.SelectedItem.Value);

			if (CheckTextBox(txtIrrWaterProvider))
			{
				//Add "IrrWaterProvider" Parameter
				objParam = objCommand.Parameters.Add("@IrrWaterProvider", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtIrrWaterProvider.Text);
			}

			if (CheckTextBox(txtIrrWaterDescription))
			{
				//Add "IrrWaterDescription" Parameter
				objParam = objCommand.Parameters.Add("@IrrWaterDescription", SqlDbType.VarChar, 150); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtIrrWaterDescription.Text);
			}

			if (CheckTextBox(txtMineralRightsDescription))
			{
				// Add "MineralRightsDescription" Parameter
				objParam = objCommand.Parameters.Add("@MineralRightsDescription", SqlDbType.VarChar, 150); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtMineralRightsDescription.Text);
			}

			// Add "ElectricityAvailable" Parameter
			objParam = objCommand.Parameters.Add("@ElectricityAvailable", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt16(rblElectricityAvailable.SelectedItem.Value);

			if (CheckDropDownList(ddlElectricityProvider))
			{
				// Add "ElectricityProvider" Parameter
				objParam = objCommand.Parameters.Add("@ElectricityProvider", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(ddlElectricityProvider.SelectedItem.Value);
			}

			// Add "NaturalGasAvailable" Parameter
			objParam = objCommand.Parameters.Add("@NaturalGasAvailable", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt16(rblNaturalGasAvailable.SelectedItem.Value);

			if (CheckDropDownList(ddlNaturalGasProvider))
			{
				// Add "NaturalGasProvider" Parameter
				objParam = objCommand.Parameters.Add("@NaturalGasProvider", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(ddlNaturalGasProvider.SelectedItem.Value);
			}

			// Add "SewerInstalled" Parameter
			objParam = objCommand.Parameters.Add("@SewerInstalled", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt16(rblSewerInstalled.SelectedItem.Value);

			// Add "Sewer" Parameter
			objParam = objCommand.Parameters.Add("@Sewer", SqlDbType.VarChar, 20); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(rblSewer.SelectedItem.Value);

			// Add "Propane" Parameter
			objParam = objCommand.Parameters.Add("@Propane", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = chbPropane.Checked;

			//Add "Telephone" Parameter
			objParam = objCommand.Parameters.Add("@Telephone", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = chbTelephone.Checked;

			// Add "Possession" Parameter
			objParam = objCommand.Parameters.Add("@Possession", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddlPossession.SelectedItem.Value);

			// Add "EarnestMoney" Parameter
			objParam = objCommand.Parameters.Add("@EarnestMoney", SqlDbType.Money); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToDouble(txtEarnestMoney.Text);

			// Add "FeaturesDescription" Parameter
			objParam = objCommand.Parameters.Add("@FeaturesDescription", SqlDbType.VarChar, 750); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtFeaturesDescription.Text);

			if (CheckTextBox(txtDisclosuresDescription))
			{
				// Add "DisclosuresDescription" Parameter
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
			Response.Redirect("manage_vacant_land.aspx");
		}
	}

}
