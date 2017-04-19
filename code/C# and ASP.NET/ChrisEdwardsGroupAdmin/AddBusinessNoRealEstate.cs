using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroupAdmin
{
	public class AddBusinessNoRealEstate : Main
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
		public TextBox txtBusinessName;
		public TextBox txtBusinessDescription;
		public TextBox txtBusinessAssetsIncluded;
		public TextBox txtTraining;
		public CheckBox chbNonCompeteAgreementYesNo;
		public TextBox txtNonCompeteAgreementDescription;
		public TextBox txtInventory;
		public CheckBox chbLeaseIncludedYesNo;
		public TextBox txtLeaseDescription;
		public CheckBox chbBusinessAssetsDLA;
		public CheckBox chbFixturesFurnitureEquipmentDLA;
		public CheckBox chbInventoryDLA;
		public CheckBox chbLicenseInfoDLA;
		public CheckBox chbBusinessAccountsSDA;
		public CheckBox chbProfitLossInfoSDA;
		public DropDownList ddlPossession;
		public DropDownList ddlTerms;
		public TextBox txtDisclosures;
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
			string strVirtualPath = Request.ApplicationPath + "/config/business_with_re.xml";

			// Read XML Config File into DataSet
			ds.ReadXml(Request.MapPath(strVirtualPath), XmlReadMode.InferSchema);

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

		public void AddBusinessNoRealEstate_Click(Object Source, EventArgs E)
		{
			// Create Connection String
			string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);
			
			// Create SqlCommand Object
			SqlCommand objCommand = new SqlCommand("sp_insert_business_no_re", objConnection);
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
				//Add "PPAnnualTaxYear" Parameter
				objParam = objCommand.Parameters.Add("@PPAnnualTaxYear", SqlDbType.Char, 4); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtPPAnnualTaxYear.Text);
			}

			if (CheckTextBox(txtPPScheduleNumber))
			{
				//Add "PPScheduleNumber" Parameter
				objParam = objCommand.Parameters.Add("@PPScheduleNumber", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtPPScheduleNumber.Text);
			}

			// Add "BusinessName" Parameter
			objParam = objCommand.Parameters.Add("@BusinessName", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtBusinessName.Text);

			// Add "BusinessDescription" Parameter
			objParam = objCommand.Parameters.Add("@BusinessDescription", SqlDbType.VarChar, 500); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtBusinessDescription.Text);

			// Add "BusinessAssetsIncluded" Parameter
			objParam = objCommand.Parameters.Add("@BusinessAssetsIncluded", SqlDbType.VarChar, 500); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtBusinessAssetsIncluded.Text);

			// Add "Training" Parameter
			objParam = objCommand.Parameters.Add("@Training", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtTraining.Text);

			// Add "NonCompeteAgreementYesNo" Parameter
			objParam = objCommand.Parameters.Add("@NonCompeteAgreementYesNo", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = chbNonCompeteAgreementYesNo.Checked;

			// Check "NonCompeteAgreementDescription" TextBox
			if (CheckTextBox(txtNonCompeteAgreementDescription))
			{
				// If Not NULL Or Empty
				// Add "NonCompeteAgreementDescription" Parameter
				objParam = objCommand.Parameters.Add("@NonCompeteAgreementDescription", SqlDbType.VarChar, 250); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtNonCompeteAgreementDescription.Text);
			}

			// Add "Inventory" Parameter
			objParam = objCommand.Parameters.Add("@Inventory", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtInventory.Text);

			// Add "LeaseIncludedYesNo" Parameter
			objParam = objCommand.Parameters.Add("@LeaseIncludedYesNo", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = chbLeaseIncludedYesNo.Checked;

			// Check "LeaseDescription" TextBox
			if (CheckTextBox(txtLeaseDescription))
			{
				// If Not NULL Or Empty
				// Add "LeaseDescription" Parameter
				objParam = objCommand.Parameters.Add("@LeaseDescription", SqlDbType.VarChar, 500); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtLeaseDescription.Text);
			}

			// Add "BusinessAssetsDLA" Parameter
			objParam = objCommand.Parameters.Add("@BusinessAssetsDLA", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = chbBusinessAssetsDLA.Checked;

			// Add "FixturesFurnitureEquipmentDLA" Parameter
			objParam = objCommand.Parameters.Add("@FixturesFurnitureEquipmentDLA", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = chbFixturesFurnitureEquipmentDLA.Checked;

			// Add "InventoryDLA" Parameter
			objParam = objCommand.Parameters.Add("@InventoryDLA", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = chbInventoryDLA.Checked;

			// Add "LicenseInfoDLA" Parameter
			objParam = objCommand.Parameters.Add("@LicenseInfoDLA", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = chbLicenseInfoDLA.Checked;

			// Add "BusinessAccountsSDA" Parameter
			objParam = objCommand.Parameters.Add("@BusinessAccountsSDA", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = chbBusinessAccountsSDA.Checked;

			// Add "ProfitLossInfoSDA" Parameter
			objParam = objCommand.Parameters.Add("@ProfitLossInfoSDA", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = chbProfitLossInfoSDA.Checked;

			// Add "Possession" Parameter
			objParam = objCommand.Parameters.Add("@Possession", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddlPossession.SelectedItem.Text);

			// Add "Terms" Parameter
			objParam = objCommand.Parameters.Add("@Terms", SqlDbType.VarChar, 30); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddlTerms.SelectedItem.Text);

			// Check "DisclosuresDescription" TextBox
			if (CheckTextBox(txtDisclosures))
			{
				// If Not NULL Or Empty
				// Add "DisclosuresDescription" Parameter
				objParam = objCommand.Parameters.Add("@Disclosures", SqlDbType.VarChar, 250); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtDisclosures.Text);
			}

			// Check "MapDirections" TextBox
			if (CheckTextBox(txtMapDirections))
			{
				// If Not NULL Or Empty
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
			Response.Redirect("manage_business_no_re.aspx");
		}
	}

}
