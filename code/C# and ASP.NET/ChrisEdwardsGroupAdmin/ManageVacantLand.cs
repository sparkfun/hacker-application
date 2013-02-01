using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroupAdmin
{
	public class ManageVacantLand : Main
	{
		public Label lblListingDateTime;
		public Label lblLastEditDateTime;
		public HtmlGenericControl hgcErrors;
		public DataList dlVacantLand;

		public void Page_Load(Object Source, EventArgs e)
		{
			// Check "mlsID" Session
			// If Null or Empty Redirect to Home Page
			if (Session["mlsID"] == null || Session["mlsID"].ToString() == "")
			{
				Response.Redirect("default.aspx", true);
			}

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
			SqlCommand objCommand = new SqlCommand();

			// Set SqlCommand Object Properties
			objCommand.Connection = objConnection;
			objCommand.CommandTimeout = 30;
			objCommand.CommandText = "sp_select_vacantland";
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create SqlParameter Object
			SqlParameter objParam;

			//Add "MLS" Parameter
			objParam = objCommand.Parameters.Add("@MLS", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(Session["mlsID"]);

			// Create SqlDataAdapter Object
			SqlDataAdapter sda = new SqlDataAdapter(objCommand);

			// Create DataSet Object
			DataSet ds = new DataSet();

			// Fill DataSet
			sda.Fill(ds, "VacantLand");

			// Fill ListingDateTime Label
			DateTime dtListingDateTime = Convert.ToDateTime(ds.Tables["VacantLand"].Rows[0]["ListingDateTime"]);
			lblListingDateTime.Text = dtListingDateTime.ToString("D");

			// Get Variable Type
			string getType;
			getType = ds.Tables["VacantLand"].Rows[0]["LastEditDateTime"].GetType().ToString();

			// Check to see if LastEditDateTime field is null
			// If not DataBind LastEditDateTime label
			if (getType != "System.DBNull")
			{
				DateTime dtNow = DateTime.Now;

				// Fill LastEditDateTime Label
				DateTime dtLastEdit = Convert.ToDateTime(ds.Tables["VacantLand"].Rows[0]["LastEditDateTime"]);
				lblLastEditDateTime.Text = dtLastEdit.ToString("D");

				if (dtNow.Date == dtLastEdit.Date)
				{
					lblLastEditDateTime.CssClass = "red";
				}
			}

			// DataBind() Vacant Land DataList
			dlVacantLand.DataSource = ds.Tables["VacantLand"].DefaultView;
			dlVacantLand.DataBind();
		}

		public void VacantLand_ItemCreated(Object sender, DataListItemEventArgs e)
		{
			ListItemType lit = e.Item.ItemType;

			if (lit == ListItemType.EditItem)
			{
				// Get the item data as a DataRowView object
				DataRowView drv = (DataRowView)e.Item.DataItem;

				// Create Connection String
				string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

				// Create SqlConnection Object
				SqlConnection objConnection = new SqlConnection(strConnect);

				// Create SqlCommand Object
				SqlCommand objCommand = new SqlCommand();

				// Set SqlCommand Object Properties
				objCommand.Connection = objConnection;
				objCommand.CommandTimeout = 30;
				objCommand.CommandText = "sp_select_agents";
				objCommand.CommandType = CommandType.StoredProcedure;

				// Create SqlDataAdapter Object
				SqlDataAdapter sda = new SqlDataAdapter(objCommand);

				// Create DataSet Object
				DataSet ds = new DataSet();

				// Fill Agents Table
				sda.Fill(ds, "Agents");

				// Retrieve Agent1 DropDownList
				DropDownList ddAgent1;
				ddAgent1 = (DropDownList)e.Item.FindControl("ddlAgentID1");
				// DataBind() AgentID1 DropDownList
				ddAgent1.DataSource = ds.Tables["Agents"].DefaultView;
				ddAgent1.DataTextField = "FullName";
				ddAgent1.DataValueField = "AgentID";
				ddAgent1.DataBind();

				// Retrieve Agent2 DropDownList
				DropDownList ddAgent2;
				ddAgent2 = (DropDownList)e.Item.FindControl("ddlAgentID2");
				// DataBind() AgentID2 DropDownList
				ddAgent2.DataSource = ds.Tables["Agents"].DefaultView;
				ddAgent2.DataTextField = "FullName";
				ddAgent2.DataValueField = "AgentID";
				ddAgent2.DataBind();

				// Set CommandText to get Cities Data
				objCommand.CommandText = "sp_select_cities";
				objCommand.CommandType = CommandType.StoredProcedure;

				// Set SqlDataAdapter SelectCommand to SqlCommand
				sda.SelectCommand = objCommand;

				// Fill Cities Table
				sda.Fill(ds, "Cities");

				// Retrieve Cities DropDownList
				DropDownList ddCities;
				ddCities = (DropDownList)e.Item.FindControl("ddlCityID");
				// DataBind() Cities DropDownList
				ddCities.DataSource = ds.Tables["Cities"].DefaultView;
				ddCities.DataTextField = "CityName";
				ddCities.DataValueField = "CityID";
				ddCities.DataBind();

				// Create connection string for XML config file
				string strVirtualPath = Request.ApplicationPath + "/config/vacantland_dropdown.xml";

				// Read XML into DataSet
				ds.ReadXml(Request.MapPath(strVirtualPath), XmlReadMode.InferSchema);		

				// Retrieve "ElectricityProvider" DropDownList
				DropDownList ddElectricityProvider;
				ddElectricityProvider = (DropDownList)e.Item.FindControl("ddlElectricityProvider");
				// DataBind() "ElectricityProvider" DropDownList
				ddElectricityProvider.DataSource = ds.Tables["ElectricityProvider"].DefaultView;
				ddElectricityProvider.DataValueField = "ElectricityProviderValue";
				ddElectricityProvider.DataTextField = "ElectricityProviderValue";
				ddElectricityProvider.DataBind();

				// Retrieve "NaturalGasProvider" DropDownList
				DropDownList ddNaturalGasProvider;
				ddNaturalGasProvider = (DropDownList)e.Item.FindControl("ddlNaturalGasProvider");
				// DataBind() "NaturalGasProvider" DropDownList
				ddNaturalGasProvider.DataSource = ds.Tables["NaturalGasProvider"].DefaultView;
				ddNaturalGasProvider.DataValueField = "NaturalGasProviderValue";
				ddNaturalGasProvider.DataTextField = "NaturalGasProviderValue";
				ddNaturalGasProvider.DataBind();

				// Retrieve "Possession" DropDownList
				DropDownList ddPossession;
				ddPossession = (DropDownList)e.Item.FindControl("ddlPossession");
				// DataBind() "Possession" DropDownList
				ddPossession.DataSource = ds.Tables["Possession"].DefaultView;
				ddPossession.DataValueField = "PossessionValue";
				ddPossession.DataTextField = "PossessionValue";
				ddPossession.DataBind();

				// Add ListItem For "Possession" DropDownList
				ListItem selectPossession = new ListItem("-- Select Possession --", "");
				// Insert ListItem Into "Possession" DropDownList
				ddPossession.Items.Insert(0, selectPossession);
			}
		}

		public void VacantLand_Edit(Object sender, DataListCommandEventArgs e)
		{
			dlVacantLand.EditItemIndex = e.Item.ItemIndex;
			hgcErrors.Visible = false;
			Bind();
		}

		public void VacantLand_Cancel(Object sender, DataListCommandEventArgs e)
		{
			dlVacantLand.EditItemIndex = -1;
			hgcErrors.Visible = false;
			Bind();
		}

		public void VacantLand_Delete(Object sender, DataListCommandEventArgs e)
		{
			// Get "mlsID" from cookie
			int mlsID = (int)Session["mlsID"];

			// Set Directory Path
			string dirPath;
			dirPath = @"D:\websites\chrisedwardsgroup\listings\" + mlsID.ToString() + @"\";

			// Check to see if directory exists
			// If not delete directory
			if (Directory.Exists(dirPath))
			{
				Directory.Delete(dirPath, true);
			}

			// Create Connection String
			string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);

			// Create SqlCommand Object
			SqlCommand objCommand = new SqlCommand("sp_delete_vacantland", objConnection);
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create SqlParameter Object
			SqlParameter objParam;

			// Add MLS Parameter
			objParam = objCommand.Parameters.Add("@MLS", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = mlsID;

			// Remove Database Record
			// Catch Any Errors
			try
			{
				objConnection.Open();
				objCommand.ExecuteNonQuery();
				objConnection.Close();

				// Redirect to residential browse
				Response.Redirect("browse_vacant_land.aspx", true);
			}
			catch (SqlException Ex)
			{
				hgcErrors.Visible = true;
				hgcErrors.InnerHtml = GetSqlExceptionDump(Ex);
				return;
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
		}

		public void VacantLand_Update(Object sender, DataListCommandEventArgs e)
		{
			// Create Connection String
			string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);

			// Create SqlCommand Object
			SqlCommand objCommand = new SqlCommand("sp_update_vacantland", objConnection);
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create SqlParameter Object
			SqlParameter objParam;

			// Retrieve MLS TextBox
			TextBox tbMLS;
			tbMLS = (TextBox)e.Item.FindControl("txtMLS");
			// Add MLS Parameter
			objParam = objCommand.Parameters.Add("@MLS", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(tbMLS.Text);

			// Retrieve AltMLS TextBox
			TextBox tbAltMLS;
			tbAltMLS = (TextBox)e.Item.FindControl("txtAltMLS");
			// Check AltMLS TextBox
			if (CheckTextBox(tbAltMLS))
			{
				// Add AltMLS Parameter
				objParam = objCommand.Parameters.Add("@AltMLS", SqlDbType.Int); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToInt32(tbAltMLS.Text);
			}

			// Retrieve Price TextBox
			TextBox tbPrice;
			tbPrice = (TextBox)e.Item.FindControl("txtPrice");
			// Add Price Parameter
			objParam = objCommand.Parameters.Add("@Price", SqlDbType.Money); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToDouble(tbPrice.Text);

			// Retrieve Owner TextBox
			TextBox tbOwner;
			tbOwner = (TextBox)e.Item.FindControl("txtOwner");
			//Add Owner Parameter
			objParam = objCommand.Parameters.Add("@Owner", SqlDbType.VarChar, 35); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbOwner.Text);

			// Retrieve AgentID1 DropDownList
			DropDownList ddAgent1;
			ddAgent1 = (DropDownList)e.Item.FindControl("ddlAgentID1");
			// Add AgentID1 Parameter
			objParam = objCommand.Parameters.Add("@AgentID1", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(ddAgent1.SelectedItem.Value);

			// Retrieve AgentID2 DropDownList
			DropDownList ddAgent2;
			ddAgent2 = (DropDownList)e.Item.FindControl("ddlAgentID2");
			// Check AgentID2 DropDownList
			if (CheckDropDownList(ddAgent2))
			{
				// Add AgentID2 Parameter
				objParam = objCommand.Parameters.Add("@AgentID2", SqlDbType.Int); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToInt32(ddAgent2.SelectedItem.Value);
			}

			// Retrieve Tagline TextBox
			TextBox tbTagline;
			tbTagline = (TextBox)e.Item.FindControl("txtTagline");
			//Add "Tagline" Parameter
			objParam = objCommand.Parameters.Add("@Tagline", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbTagline.Text);

			// Retrieve Address1 TextBox
			TextBox tbAddress1;
			tbAddress1 = (TextBox)e.Item.FindControl("txtAddress1");
			//Add "Address1" Parameter
			objParam = objCommand.Parameters.Add("@Address1", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbAddress1.Text);

			// Retrieve Address2 TextBox
			TextBox tbAddress2;
			tbAddress2 = (TextBox)e.Item.FindControl("txtAddress2");
			// Check Address2 TextBox
			if (CheckTextBox(tbAddress2))
			{
				// Add Address2 Parameter
				objParam = objCommand.Parameters.Add("@Address2", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbAddress2.Text);
			}

			// Retrieve CityID DropDownList
			DropDownList ddCityID;
			ddCityID = (DropDownList)e.Item.FindControl("ddlCityID");
			//Add "CityID" Parameter
			objParam = objCommand.Parameters.Add("@CityID", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(ddCityID.SelectedItem.Value);

			// Retrieve Subdivision TextBox
			TextBox tbSubdivision;
			tbSubdivision = (TextBox)e.Item.FindControl("txtSubdivision");
			// Check Subdivision TextBox
			if (CheckTextBox(tbSubdivision))
			{
				// Add Subdivision Parameter
				objParam = objCommand.Parameters.Add("@Subdivision", SqlDbType.VarChar, 60); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbSubdivision.Text);
			}

			// Retrieve the AnnualTaxes TextBox
			TextBox tbAnnualTaxes;
			tbAnnualTaxes = (TextBox)e.Item.FindControl("txtAnnualTaxes");
			// Check AnnualTaxes TextBox
			if (CheckTextBox(tbAnnualTaxes))
			{
				// Add AnnualTaxes Parameter
				objParam = objCommand.Parameters.Add("@AnnualTaxes", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbAnnualTaxes.Text);
			}

			// Retrieve the AnnualTaxYear TextBox
			TextBox tbAnnualTaxYear;
			tbAnnualTaxYear = (TextBox)e.Item.FindControl("txtAnnualTaxYear");
			// Check AnnualTaxYear TextBox
			if (CheckTextBox(tbAnnualTaxYear))
			{
				// Add AnnualTaxYear Parameter
				objParam = objCommand.Parameters.Add("@AnnualTaxYear", SqlDbType.Char, 4); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbAnnualTaxYear.Text);
			}

			// Retrieve "ScheduleNumber" TextBox
			TextBox tbScheduleNumber;
			tbScheduleNumber = (TextBox)e.Item.FindControl("txtScheduleNumber");
			// Check "ScheduleNumber" TextBox
			if (CheckTextBox(tbScheduleNumber))
			{
				//Add "ScheduleNumber" Parameter
				objParam = objCommand.Parameters.Add("@ScheduleNumber", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbScheduleNumber.Text);
			}

			// Retrieve "Assessments" TextBox
			TextBox tbAssessments;
			tbAssessments = (TextBox)e.Item.FindControl("txtAssessments");
			// Check "Assessments" TextBox
			if (CheckTextBox(tbAssessments))
			{
				// Add "Assessments" Parameter
				objParam = objCommand.Parameters.Add("@Assessments", SqlDbType.VarChar, 100);
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbAssessments.Text);
			}

			// Retrieve "ParcelSize" TextBox
			TextBox tbParcelSize;
			tbParcelSize = (TextBox)e.Item.FindControl("txtParcelSize");
			// Add "ParcelSize" Parameter
			objParam = objCommand.Parameters.Add("@ParcelSize", SqlDbType.VarChar, 30); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = (SqlString)(tbParcelSize.Text);

			// Retrieve "Covenants" RadioButtonList
			RadioButtonList rbCovenants;
			rbCovenants = (RadioButtonList)e.Item.FindControl("rblCovenants");
			// Create SqlString to Hold Temp Value
			SqlString covenantsTemp;
			covenantsTemp = rbCovenants.SelectedItem.Value;
			// Convert SqlString to SqlBoolean
			SqlBoolean covenants;
			covenants = covenantsTemp.ToSqlBoolean();
			// Add "Covenants" Parameter
			objParam = objCommand.Parameters.Add("@Covenants", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = covenants;

			// Retrieve "Fenced" RadioButtonList
			RadioButtonList rbFenced;
			rbFenced = (RadioButtonList)e.Item.FindControl("rblFenced");
			// Create SqlString to Hold Temp Value
			SqlString fencedTemp;
			fencedTemp = rbFenced.SelectedItem.Value;
			// Convert SqlString to SqlBoolean
			SqlBoolean fenced;
			fenced = fencedTemp.ToSqlBoolean();
			// Add "Fenced" Parameter
			objParam = objCommand.Parameters.Add("@Fenced", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = fenced;

			// Retrieve "FencingDescription" TextBox
			TextBox tbFencingDescription;
			tbFencingDescription = (TextBox)e.Item.FindControl("txtFencingDescription");
			// Add "FencingDescription" Parameter
			objParam = objCommand.Parameters.Add("@FencingDescription", SqlDbType.VarChar, 150); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = (SqlString)(tbFencingDescription.Text);

			// Retrieve "AccessDescription" TextBox
			TextBox tbAccessDescription;
			tbAccessDescription = (TextBox)e.Item.FindControl("txtAccessDescription");
			// Add "AccessDescription" Parameter
			objParam = objCommand.Parameters.Add("@AccessDescription", SqlDbType.VarChar, 150); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = (SqlString)(tbAccessDescription.Text);

			// Retrieve "Topography" TextBox
			TextBox tbTopography;
			tbTopography = (TextBox)e.Item.FindControl("txtTopography");
			// Check "Topography" TextBox
			if (CheckTextBox(tbTopography))
			{
				// Add "Topography" Parameter
				objParam = objCommand.Parameters.Add("@Topography", SqlDbType.VarChar, 150); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = (SqlString)(tbTopography.Text);
			}

			// Retrieve "Easements" TextBox
			TextBox tbEasements;
			tbEasements = (TextBox)e.Item.FindControl("txtEasements");
			// Check "Easements" TextBox
			if (CheckTextBox(tbEasements))
			{
				// Add "Easements" Parameter
				objParam = objCommand.Parameters.Add("@Easements", SqlDbType.VarChar, 80); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = (SqlString)(tbEasements.Text);
			}

			// Retrieve "DomWaterAvailable" RadioButtonList
			RadioButtonList rbDomWaterAvailable;
			rbDomWaterAvailable = (RadioButtonList)e.Item.FindControl("rblDomWaterAvailable");
			// Create SqlString to Hold Temp Value
			SqlString domWaterAvailableTemp;
			domWaterAvailableTemp = rbDomWaterAvailable.SelectedItem.Value;
			// Convert SqlString to SqlBoolean
			SqlBoolean domWaterAvailable;
			domWaterAvailable = domWaterAvailableTemp.ToSqlBoolean();
			// Add "DomWaterAvailable" Parameter
			objParam = objCommand.Parameters.Add("@DomWaterAvailable", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = domWaterAvailable;

			// Retrieve "DomWaterProvider" TextBox
			TextBox tbDomWaterProvider;
			tbDomWaterProvider = (TextBox)e.Item.FindControl("txtDomWaterProvider");
			// Check "DomWaterProvider" TextBox
			if (CheckTextBox(tbDomWaterProvider))
			{
				// Add "DomWaterProvider" Parameter
				objParam = objCommand.Parameters.Add("@DomWaterProvider", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = (SqlString)(tbDomWaterProvider.Text);
			}

			// Retrieve "IrrWaterAvailable" RadioButtonList
			RadioButtonList rbIrrWaterAvailable;
			rbIrrWaterAvailable = (RadioButtonList)e.Item.FindControl("rblIrrWaterAvailable");
			// Create SqlString to Hold Temp Value
			SqlString irrWaterAvailableTemp;
			irrWaterAvailableTemp = rbIrrWaterAvailable.SelectedItem.Value;
			// Convert SqlString to SqlBoolean
			SqlBoolean irrWaterAvailable;
			irrWaterAvailable = irrWaterAvailableTemp.ToSqlBoolean();
			// Add "IrrWaterAvailable" Parameter
			objParam = objCommand.Parameters.Add("@IrrWaterAvailable", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = irrWaterAvailable;

			// Retrieve "IrrWaterProvider" TextBox
			TextBox tbIrrWaterProvider;
			tbIrrWaterProvider = (TextBox)e.Item.FindControl("txtIrrWaterProvider");
			// Check "IrrWaterProvider" TextBox
			if (CheckTextBox(tbIrrWaterProvider))
			{
				// Add "IrrWaterProvider" Parameter
				objParam = objCommand.Parameters.Add("@IrrWaterProvider", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = (SqlString)(tbIrrWaterProvider.Text);
			}

			// Retrieve "IrrWaterDescription" TextBox
			TextBox tbIrrWaterDescription;
			tbIrrWaterDescription = (TextBox)e.Item.FindControl("txtIrrWaterDescription");
			// Check "IrrWaterDescription" TextBox
			if (CheckTextBox(tbIrrWaterDescription))
			{
				// Add "IrrWaterDescription" Parameter
				objParam = objCommand.Parameters.Add("@IrrWaterDescription", SqlDbType.VarChar, 150); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = (SqlString)(tbIrrWaterDescription.Text);
			}

			// Retrieve "MineralRightsDescription" TextBox
			TextBox tbMineralRightsDescription;
			tbMineralRightsDescription = (TextBox)e.Item.FindControl("txtMineralRightsDescription");
			// Check "MineralRightsDescription" TextBox
			if (CheckTextBox(tbMineralRightsDescription))
			{
				// Add "MineralRightsDescription" Parameter
				objParam = objCommand.Parameters.Add("@MineralRightsDescription", SqlDbType.VarChar, 150); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = (SqlString)(tbMineralRightsDescription.Text);
			}

			// Retrieve "ElectricityAvailable" RadioButtonList
			RadioButtonList rbElectricityAvailable;
			rbElectricityAvailable = (RadioButtonList)e.Item.FindControl("rblElectricityAvailable");
			// Create SqlString to Hold Temp Value
			SqlString electricityAvailableTemp;
			electricityAvailableTemp = rbElectricityAvailable.SelectedItem.Value;
			// Convert SqlString to SqlBoolean
			SqlBoolean electricityAvailable;
			electricityAvailable = electricityAvailableTemp.ToSqlBoolean();
			// Add "ElectricityAvailable" Parameter
			objParam = objCommand.Parameters.Add("@ElectricityAvailable", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = electricityAvailable;

			// Retrieve "ElectricityProvider" DropDownList
			DropDownList ddElectricityProvider;
			ddElectricityProvider = (DropDownList)e.Item.FindControl("ddlElectricityProvider");
			// Check "ElectricityProvider" DropDownList
			if (CheckDropDownList(ddElectricityProvider))
			{
				// Add "ElectricityProvider" Parameter
				objParam = objCommand.Parameters.Add("@ElectricityProvider", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(ddElectricityProvider.SelectedItem.Value);
			}

			// Retrieve "NaturalGasAvailable" RadioButtonList
			RadioButtonList rbNaturalGasAvailable;
			rbNaturalGasAvailable = (RadioButtonList)e.Item.FindControl("rblNaturalGasAvailable");
			// Create SqlString to Hold Temp Value
			SqlString naturalGasAvailableTemp;
			naturalGasAvailableTemp = rbNaturalGasAvailable.SelectedItem.Value;
			// Convert SqlString to SqlBoolean
			SqlBoolean naturalGasAvailable;
			naturalGasAvailable = naturalGasAvailableTemp.ToSqlBoolean();
			// Add "NaturalGasAvailable" Parameter
			objParam = objCommand.Parameters.Add("@NaturalGasAvailable", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = naturalGasAvailable;

			// Retrieve "NaturalGasProvider" DropDownList
			DropDownList ddNaturalGasProvider;
			ddNaturalGasProvider = (DropDownList)e.Item.FindControl("ddlNaturalGasProvider");
			// Check "NaturalGasProvider" DropDownList
			if (CheckDropDownList(ddNaturalGasProvider))
			{
				// Add "NaturalGasProvider" Parameter
				objParam = objCommand.Parameters.Add("@NaturalGasProvider", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(ddNaturalGasProvider.SelectedItem.Value);
			}

			// Retrieve "SewerInstalled" RadioButtonList
			RadioButtonList rbSewerInstalled;
			rbSewerInstalled = (RadioButtonList)e.Item.FindControl("rblSewerInstalled");
			// Create SqlString to Hold Temp Value
			SqlString sewerInstalledTemp;
			sewerInstalledTemp = rbSewerInstalled.SelectedItem.Value;
			// Convert SqlString to SqlBoolean
			SqlBoolean sewerInstalled;
			sewerInstalled = sewerInstalledTemp.ToSqlBoolean();
			// Add "SewerInstalled" Parameter
			objParam = objCommand.Parameters.Add("@SewerInstalled", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = sewerInstalled;

			// Retrieve "Sewer" RadioButtonList
			RadioButtonList rbSewer;
			rbSewer = (RadioButtonList)e.Item.FindControl("rblSewer");
			// Add "Sewer" Parameter
			objParam = objCommand.Parameters.Add("@Sewer", SqlDbType.VarChar, 20); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = (SqlString)(rbSewer.SelectedItem.Value);

			// Retrieve "Propane" CheckBox
			CheckBox chbPropane;
			chbPropane = (CheckBox)e.Item.FindControl("chbPropane");
			// Add "Propane" Parameter
			objParam = objCommand.Parameters.Add("@Propane", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = (SqlBoolean)(chbPropane.Checked);

			// Retrieve "Telephone" CheckBox
			CheckBox chbTelephone;
			chbTelephone = (CheckBox)e.Item.FindControl("chbTelephone");
			// Add "Telephone" Parameter
			objParam = objCommand.Parameters.Add("@Telephone", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = (SqlBoolean)(chbTelephone.Checked);

			// Retrieve "Possession" DropDownList
			DropDownList ddPossession;
			ddPossession = (DropDownList)e.Item.FindControl("ddlPossession");
			// Add "Possession" Parameter
			objParam = objCommand.Parameters.Add("@Possession", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = (SqlString)(ddPossession.SelectedItem.Value);

			// Retrieve "EarnestMoney" TextBox
			TextBox tbEarnestMoney;
			tbEarnestMoney = (TextBox)e.Item.FindControl("txtEarnestMoney");
			// Create SqlString to Hold Temp Value
			SqlString tempEarnestMoney;
			tempEarnestMoney = tbEarnestMoney.Text;
			// Convert SqlString to SqlMoney Value
			SqlMoney earnestMoney;
			earnestMoney = tempEarnestMoney.ToSqlMoney();
			// Add "EarnestMoney" Parameter
			objParam = objCommand.Parameters.Add("@EarnestMoney", SqlDbType.Money); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = earnestMoney;

			// Retrieve "FeaturesDescription" TextBox
			TextBox tbFeaturesDescription;
			tbFeaturesDescription = (TextBox)e.Item.FindControl("txtFeaturesDescription");
			// Add "FeaturesDescription" Parameter
			objParam = objCommand.Parameters.Add("@FeaturesDescription", SqlDbType.VarChar, 750); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = (SqlString)(tbFeaturesDescription.Text);

			// Retrieve "DisclosuresDescription" TextBox
			TextBox tbDisclosuresDescription;
			tbDisclosuresDescription = (TextBox)e.Item.FindControl("txtDisclosuresDescription");
			// Check "DisclosuresDescription" TextBox
			if (CheckTextBox(tbDisclosuresDescription))
			{
				// Add "DisclosuresDescription" Parameter
				objParam = objCommand.Parameters.Add("@DisclosuresDescription", SqlDbType.VarChar, 250); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = (SqlString)(tbDisclosuresDescription.Text);
			}

			// Retrieve "MapDirections" TextBox
			TextBox tbMapDirections;
			tbMapDirections = (TextBox)e.Item.FindControl("txtMapDirections");
			// Check "MapDirections" TextBox
			if (CheckTextBox(tbMapDirections))
			{
				// Add "MapDirections" Parameter
				objParam = objCommand.Parameters.Add("@MapDirections", SqlDbType.VarChar, 250); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = (SqlString)(tbMapDirections.Text);
			}

			// Update Database Record
			// Catch Any Errors and Display
			try
			{
				objConnection.Open();
				objCommand.ExecuteNonQuery();
				objConnection.Close();
			}
			catch (SqlException Ex)
			{
				hgcErrors.Visible = true;
				hgcErrors.InnerHtml = GetSqlExceptionDump(Ex);
				return;
			}
			catch (Exception objError)
			{
				hgcErrors.Visible = true;
				hgcErrors.InnerHtml = "Summary of Errors:";
				hgcErrors.InnerHtml += "<ul>" + "\n";
				hgcErrors.InnerHtml += "<li>Error Message: " + objError.Message +
					"</li>" + "\n";
				hgcErrors.InnerHtml += "<li>Error Source: " + objError.Source +
					"</li>" + "\n";
				hgcErrors.InnerHtml += "</ul>" + "\n";
				return;
			}

			// Set DataList EditItemIndex To -1
			dlVacantLand.EditItemIndex = -1;

			// Set MLS Cookie
			Session["mlsID"] = Convert.ToInt32(tbMLS.Text);

			// Call Bind() Function
			Bind();
		}

		public void VacantLand_DataBound(Object sender, DataListItemEventArgs e) 
		{
			ListItemType lit = e.Item.ItemType;

			if (lit == ListItemType.Item)
			{
				// Get the item data as a DataRowView object
				DataRowView drv = (DataRowView)e.Item.DataItem;

				// Retrieve "MLS" TableCell
				TableCell tcMLS;
				tcMLS = (TableCell)e.Item.FindControl("tcMLS");
				// DataBind "MLS" TableCell
				tcMLS.Text = drv["MLS"].ToString();

				// Retrieve "AltMLS" TableCell
				TableCell tcAltMLS;
				tcAltMLS = (TableCell)e.Item.FindControl("tcAltMLS");
				// DataBind "AltMLS" TableCell
				tcAltMLS.Text = ConvertToString(drv["AltMLS"]);

				// Retrieve "Price" TableCell
				TableCell tcPrice;
				tcPrice = (TableCell)e.Item.FindControl("tcPrice");
				// DataBind "Price" TableCell
				tcPrice.Text = ConvertToMoney(drv["Price"]);

				// Retrieve "Agent1" TableCell
				TableCell tcAgent1;
				tcAgent1 = (TableCell)e.Item.FindControl("tcAgent1");
				// DataBind "Agent1" TableCell
				tcAgent1.Text = drv["Agent1"].ToString();

				// Retrieve "Agent2" TableCell
				TableCell tcAgent2;
				tcAgent2 = (TableCell)e.Item.FindControl("tcAgent2");
				// DataBind "Agent2" TableCell
				tcAgent2.Text = ConvertToString(drv["Agent2"]);

				// Retrieve "Owner" TableCell
				TableCell tcOwner;
				tcOwner = (TableCell)e.Item.FindControl("tcOwner");
				// DataBind "Owner" TableCell
				tcOwner.Text = drv["Owner"].ToString();

				// Retrieve "Tagline" TableCell
				TableCell tcTagline;
				tcTagline = (TableCell)e.Item.FindControl("tcTagline");
				// DataBind "Tagline" TableCell
				tcTagline.Text = drv["Tagline"].ToString();

				// Retrieve "Address1" TableCell
				TableCell tcAddress1;
				tcAddress1 = (TableCell)e.Item.FindControl("tcAddress1");
				// DataBind "Address1" TableCell
				tcAddress1.Text = drv["Address1"].ToString();

				// Retrieve "Address2" TableCell
				TableCell tcAddress2;
				tcAddress2 = (TableCell)e.Item.FindControl("tcAddress2");
				// DataBind "Address2" TableCell
				tcAddress2.Text = ConvertToString(drv["Address2"]);

				// Retrieve "City" TableCell
				TableCell tcCity;
				tcCity = (TableCell)e.Item.FindControl("tcCity");
				// DataBind "City" TableCell
				tcCity.Text = drv["CityName"].ToString();

				// Retrieve "Subdivision" TableCell
				TableCell tcSubdivision;
				tcSubdivision = (TableCell)e.Item.FindControl("tcSubdivision");
				// DataBind "Subdivision" TableCell
				tcSubdivision.Text = ConvertToString(drv["Subdivision"]);

				// Retrieve "AnnualTaxes" TableCell
				TableCell tcAnnualTaxes;
				tcAnnualTaxes = (TableCell)e.Item.FindControl("tcAnnualTaxes");
				// DataBind "AnnualTaxes" TableCell
				tcAnnualTaxes.Text = ConvertToMoney(drv["AnnualTaxes"]);

				// Retrieve "AnnualTaxYear" TableCell
				TableCell tcAnnualTaxYear;
				tcAnnualTaxYear = (TableCell)e.Item.FindControl("tcAnnualTaxYear");
				// DataBind "AnnualTaxYear" TableCell
				tcAnnualTaxYear.Text = ConvertToString(drv["AnnualTaxYear"]);

				// Retrieve "ScheduleNumber" TableCell
				TableCell tcScheduleNumber;
				tcScheduleNumber = (TableCell)e.Item.FindControl("tcScheduleNumber");
				// DataBind "ScheduleNumber" TableCell
				tcScheduleNumber.Text = ConvertToString(drv["ScheduleNumber"]);

				// Retrieve "Assessments" TableCell
				TableCell tcAssessments;
				tcAssessments = (TableCell)e.Item.FindControl("tcAssessments");
				// DataBind "Assessments" TableCell
				tcAssessments.Text = ConvertToString(drv["Assessments"]);

				// Retrieve "ParcelSize" TableCell
				TableCell tcParcelSize;
				tcParcelSize = (TableCell)e.Item.FindControl("tcParcelSize");
				// DataBind "ParcelSize" TableCell
				tcParcelSize.Text = drv["ParcelSize"].ToString();

				// Retrieve "Covenants" TableCell
				TableCell tcCovenants;
				tcCovenants = (TableCell)e.Item.FindControl("tcCovenants");
				// DataBind "Covenants" TableCell
				tcCovenants.Text = ConvertToYesNo(drv["Covenants"]);

				// Retrieve "Fenced" TableCell
				TableCell tcFenced;
				tcFenced = (TableCell)e.Item.FindControl("tcFenced");
				// DataBind "Fenced" TableCell
				tcFenced.Text = ConvertToYesNo(drv["Fenced"]);

				// Retrieve "FencingDescription" TableCell
				TableCell tcFencingDescription;
				tcFencingDescription = (TableCell)e.Item.FindControl("tcFencingDescription");
				// DataBind "FencingDescription" TableCell
				tcFencingDescription.Text = drv["FencingDescription"].ToString();

				// Retrieve "AccessDescription" TableCell
				TableCell tcAccessDescription;
				tcAccessDescription = (TableCell)e.Item.FindControl("tcAccessDescription");
				// DataBind "AccessDescription" TableCell
				tcAccessDescription.Text = drv["AccessDescription"].ToString();

				// Retrieve "Topography" TableCell
				TableCell tcTopography;
				tcTopography = (TableCell)e.Item.FindControl("tcTopography");
				// DataBind "Topography" TableCell
				tcTopography.Text = ConvertToString(drv["Topography"]);

				// Retrieve "Easements" TableCell
				TableCell tcEasements;
				tcEasements = (TableCell)e.Item.FindControl("tcEasements");
				// DataBind "Easements" TableCell
				tcEasements.Text = ConvertToString(drv["Easements"]);

				// Retrieve "DomWaterAvailable" TableCell
				TableCell tcDomWaterAvailable;
				tcDomWaterAvailable = (TableCell)e.Item.FindControl("tcDomWaterAvailable");
				// DataBind "DomWaterAvailable" TableCell
				tcDomWaterAvailable.Text = ConvertToYesNo(drv["DomWaterAvailable"]);

				// Retrieve "DomWaterProvider" TableCell
				TableCell tcDomWaterProvider;
				tcDomWaterProvider = (TableCell)e.Item.FindControl("tcDomWaterProvider");
				// DataBind "DomWaterProvider" TableCell
				tcDomWaterProvider.Text = ConvertToString(drv["DomWaterProvider"]);

				// Retrieve "IrrWaterAvailable" TableCell
				TableCell tcIrrWaterAvailable;
				tcIrrWaterAvailable = (TableCell)e.Item.FindControl("tcIrrWaterAvailable");
				// DataBind "IrrWaterAvailable" TableCell
				tcIrrWaterAvailable.Text = ConvertToYesNo(drv["IrrWaterAvailable"]);

				// Retrieve "IrrWaterProvider" TableCell
				TableCell tcIrrWaterProvider;
				tcIrrWaterProvider = (TableCell)e.Item.FindControl("tcIrrWaterProvider");
				// DataBind "IrrWaterProvider" TableCell
				tcIrrWaterProvider.Text = ConvertToString(drv["IrrWaterProvider"]);

				// Retrieve "IrrWaterDescription" TableCell
				TableCell tcIrrWaterDescription;
				tcIrrWaterDescription = (TableCell)e.Item.FindControl("tcIrrWaterDescription");
				// DataBind "IrrWaterDescription" TableCell
				tcIrrWaterDescription.Text = ConvertToString(drv["IrrWaterDescription"]);

				// Retrieve "MineralRightsDescription" TableCell
				TableCell tcMineralRightsDescription;
				tcMineralRightsDescription = (TableCell)e.Item.FindControl("tcMineralRightsDescription");
				// DataBind "MineralRightsDescription" TableCell
				tcMineralRightsDescription.Text = ConvertToString(drv["MineralRightsDescription"]);

				// Retrieve "ElectricityAvailable" TableCell
				TableCell tcElectricityAvailable;
				tcElectricityAvailable = (TableCell)e.Item.FindControl("tcElectricityAvailable");
				// DataBind "ElectricityAvailable" TableCell
				tcElectricityAvailable.Text = ConvertToYesNo(drv["ElectricityAvailable"]);

				// Retrieve "ElectricityProvider" TableCell
				TableCell tcElectricityProvider;
				tcElectricityProvider = (TableCell)e.Item.FindControl("tcElectricityProvider");
				// DataBind "ElectricityProvider" TableCell
				tcElectricityProvider.Text = ConvertToString(drv["ElectricityProvider"]);

				// Retrieve "NaturalGasAvailable" TableCell
				TableCell tcNaturalGasAvailable;
				tcNaturalGasAvailable = (TableCell)e.Item.FindControl("tcNaturalGasAvailable");
				// DataBind "NaturalGasAvailable" TableCell
				tcNaturalGasAvailable.Text = ConvertToYesNo(drv["NaturalGasAvailable"]);

				// Retrieve "NaturalGasProvider" TableCell
				TableCell tcNaturalGasProvider;
				tcNaturalGasProvider = (TableCell)e.Item.FindControl("tcNaturalGasProvider");
				// DataBind "NaturalGasProvider" TableCell
				tcNaturalGasProvider.Text = ConvertToString(drv["NaturalGasProvider"]);

				// Retrieve "SewerInstalled" TableCell
				TableCell tcSewerInstalled;
				tcSewerInstalled = (TableCell)e.Item.FindControl("tcSewerInstalled");
				// DataBind "SewerInstalled" TableCell
				tcSewerInstalled.Text = ConvertToYesNo(drv["SewerInstalled"]);

				// Retrieve "Sewer" TableCell
				TableCell tcSewer;
				tcSewer = (TableCell)e.Item.FindControl("tcSewer");
				// DataBind "Sewer" TableCell
				tcSewer.Text = drv["Sewer"].ToString();

				// Retrieve "Propane" TableCell
				TableCell tcPropane;
				tcPropane = (TableCell)e.Item.FindControl("tcPropane");
				// DataBind "Propane" TableCell
				tcPropane.Text = ConvertToYesNo(drv["Propane"]);

				// Retrieve "Telephone" TableCell
				TableCell tcTelephone;
				tcTelephone = (TableCell)e.Item.FindControl("tcTelephone");
				// DataBind "Telephone" TableCell
				tcTelephone.Text = ConvertToYesNo(drv["Telephone"]);

				// Retrieve "Possession" TableCell
				TableCell tcPossession;
				tcPossession = (TableCell)e.Item.FindControl("tcPossession");
				// DataBind "Possession" TableCell
				tcPossession.Text = drv["Possession"].ToString();

				// Retrieve "EarnestMoney" TableCell
				TableCell tcEarnestMoney;
				tcEarnestMoney = (TableCell)e.Item.FindControl("tcEarnestMoney");
				// DataBind "EarnestMoney" TableCell
				tcEarnestMoney.Text = ConvertToMoney(drv["EarnestMoney"]);

				// Retrieve "FeaturesDescription" TableCell
				TableCell tcFeaturesDescription;
				tcFeaturesDescription = (TableCell)e.Item.FindControl("tcFeaturesDescription");
				// DataBind "FeaturesDescription" TableCell
				tcFeaturesDescription.Text = drv["FeaturesDescription"].ToString();

				// Retrieve "DisclosuresDescription" TableCell
				TableCell tcDisclosuresDescription;
				tcDisclosuresDescription = (TableCell)e.Item.FindControl("tcDisclosuresDescription");
				// DataBind "DisclosuresDescription" TableCell
				tcDisclosuresDescription.Text = ConvertToString(drv["DisclosuresDescription"]);

				// Retrieve "MapDirections" TableCell
				TableCell tcMapDirections;
				tcMapDirections = (TableCell)e.Item.FindControl("tcMapDirections");
				// DataBind "MapDirections" TableCell
				tcMapDirections.Text = ConvertToString(drv["MapDirections"]);
			}

			if (lit == ListItemType.EditItem)
			{
				// Get the item data as a DataRowView object
				DataRowView drv = (DataRowView)e.Item.DataItem;

				// Retrieve MLS TextBox
				TextBox tbMLS;
				tbMLS = (TextBox)e.Item.FindControl("txtMLS");
				// Fill MLS TextBox
				tbMLS.Text = drv["MLS"].ToString();
				// Disable MLS TextBox
				tbMLS.Enabled = false;

				// Retrieve AltMLS TextBox
				TextBox tbAltMLS;
				tbAltMLS = (TextBox)e.Item.FindControl("txtAltMLS");
				// Fill AltMLS TextBox
				tbAltMLS.Text = drv["AltMLS"].ToString();

				// Retrieve Price TextBox
				TextBox tbPrice;
				tbPrice = (TextBox)e.Item.FindControl("txtPrice");
				// Fill Price TextBox
				tbPrice.Text = ConvertToNumeric(drv["Price"]);

				// Retrieve "Agent1" DropDownList
				DropDownList ddAgent1;
				ddAgent1 = (DropDownList)e.Item.FindControl("ddlAgentID1");
				// Try selecting "Agent1" value
				// Catch if value is not valid and set top value
				try
				{
					// Select "Agent1" DropDownList Choice
					ddAgent1.Items.FindByValue(drv["AgentID1"].ToString()).Selected = true;
				}
				catch
				{
					// Select "Agent1" DropDownList Choice
					ddAgent1.SelectedIndex = 0;
				}

				// Retrieve "Agent2" DropDownList
				DropDownList ddAgent2;
				ddAgent2 = (DropDownList)e.Item.FindControl("ddlAgentID2");
				// Create ListItem for "Agent2" DropDownList
				// Insert ListItem into "Agent2" DropDownList
				ListItem noneAgent = new ListItem("-- N/A --", "");
				ddAgent2.Items.Insert(0, noneAgent);
				// Try selecting "Agent2" value
				// Catch if value is not valid and set top value
				try
				{
					// Select "Agent2" DropDownList Choice
					ddAgent2.Items.FindByValue(drv["AgentID2"].ToString()).Selected = true;
				}
				catch
				{
					// Select "Agent2" DropDownList Choice
					ddAgent2.SelectedIndex = 0;
				}

				// Retrieve Owner TextBox
				TextBox tbOwner;
				tbOwner = (TextBox)e.Item.FindControl("txtOwner");
				// Fill Owner TextBox
				tbOwner.Text = drv["Owner"].ToString();

				// Retrieve Tagline TextBox
				TextBox tbTagline;
				tbTagline = (TextBox)e.Item.FindControl("txtTagline");
				// Fill Tagline TextBox
				tbTagline.Text = drv["Tagline"].ToString();

				// Retrieve Address1 TextBox
				TextBox tbAddress1;
				tbAddress1 = (TextBox)e.Item.FindControl("txtAddress1");
				// Fill Address1 TextBox
				tbAddress1.Text = drv["Address1"].ToString();

				// Retrieve Address2 TextBox
				TextBox tbAddress2;
				tbAddress2 = (TextBox)e.Item.FindControl("txtAddress2");
				// Fill Address2 TextBox
				tbAddress2.Text = drv["Address2"].ToString();

				// Retrieve "Cities" DropDownList
				DropDownList ddCities;
				ddCities = (DropDownList)e.Item.FindControl("ddlCityID");
				// Try selecting "Cities" value
				// Catch if value is not valid and set top value
				try
				{
					// Select "Cities" DropDownList Choice
					ddCities.Items.FindByValue(drv["CityID"].ToString()).Selected = true;
				}
				catch
				{
					// Select "Cities" DropDownList Choice
					ddCities.SelectedIndex = 0;
				}

				// Retrieve "Subdivision" TextBox
				TextBox tbSubdivision;
				tbSubdivision = (TextBox)e.Item.FindControl("txtSubdivision");
				// Fill "Subdivision" TextBox
				tbSubdivision.Text = drv["Subdivision"].ToString();

				// Retrieve "AnnualTaxes" TextBox
				TextBox tbAnnualTaxes;
				tbAnnualTaxes = (TextBox)e.Item.FindControl("txtAnnualTaxes");
				// Fill "AnnualTaxes" TextBox
				tbAnnualTaxes.Text = ConvertToNumeric(drv["AnnualTaxes"]);	
				
				// Retrieve "AnnualTaxYear" TextBox
				TextBox tbAnnualTaxYear;
				tbAnnualTaxYear = (TextBox)e.Item.FindControl("txtAnnualTaxYear");
				// Fill "AnnualTaxYear" TextBox
				tbAnnualTaxYear.Text = drv["AnnualTaxYear"].ToString();

				// Retrieve "ScheduleNumber" TextBox
				TextBox tbScheduleNumber;
				tbScheduleNumber = (TextBox)e.Item.FindControl("txtScheduleNumber");
				// Fill "ScheduleNumber" TextBox
				tbScheduleNumber.Text = drv["ScheduleNumber"].ToString();

				// Retrieve "Assessments" TextBox
				TextBox tbAssessments;
				tbAssessments = (TextBox)e.Item.FindControl("txtAssessments");
				// Fill "Assessments" TextBox
				tbAssessments.Text = drv["Assessments"].ToString();

				// Retrieve "ParcelSize" TextBox
				TextBox tbParcelSize;
				tbParcelSize = (TextBox)e.Item.FindControl("txtParcelSize");
				// Fill "ParcelSize" TextBox
				tbParcelSize.Text = drv["ParcelSize"].ToString();

				// Retrieve "Covenants" RadioButtonList
				RadioButtonList rbCovenants;
				rbCovenants = (RadioButtonList)e.Item.FindControl("rblCovenants");
				// Convert "Covenants" to Boolean
				bool blnCovenants = Convert.ToBoolean(drv["Covenants"]);
				// Select "Covenants" RadioButtonList Choice
				if (blnCovenants == true)
				{
					rbCovenants.Items[0].Selected = true;
				}
				else
				{
					rbCovenants.Items[1].Selected = true;
				}

				// Retrieve "Fenced" RadioButtonList
				RadioButtonList rbFenced;
				rbFenced = (RadioButtonList)e.Item.FindControl("rblFenced");
				// Convert "Fenced" to Boolean
				bool blnFenced = Convert.ToBoolean(drv["Fenced"]);
				// Select "Fenced" RadioButtonList Choice
				if (blnFenced == true)
				{
					rbFenced.Items[0].Selected = true;
				}
				else
				{
					rbFenced.Items[1].Selected = true;
				}

				// Retrieve "FencingDescription" TextBox
				TextBox tbFencingDescription;
				tbFencingDescription = (TextBox)e.Item.FindControl("txtFencingDescription");
				// Fill "FencingDescription" TextBox
				tbFencingDescription.Text = drv["FencingDescription"].ToString();

				// Retrieve "AccessDescription" TextBox
				TextBox tbAccessDescription;
				tbAccessDescription = (TextBox)e.Item.FindControl("txtAccessDescription");
				// Fill "AccessDescription" TextBox
				tbAccessDescription.Text = drv["AccessDescription"].ToString();

				// Retrieve "Topography" TextBox
				TextBox tbTopography;
				tbTopography = (TextBox)e.Item.FindControl("txtTopography");
				// Fill "Topography" TextBox
				tbTopography.Text = drv["Topography"].ToString();

				// Retrieve "Easements" TextBox
				TextBox tbEasements;
				tbEasements = (TextBox)e.Item.FindControl("txtEasements");
				// Fill "Easements" TextBox
				tbEasements.Text = drv["Easements"].ToString();

				// Retrieve "DomWaterAvailable" RadioButtonList
				RadioButtonList rbDomWaterAvailable;
				rbDomWaterAvailable = (RadioButtonList)e.Item.FindControl("rblDomWaterAvailable");
				// Convert "DomWaterAvailable" to Boolean
				bool blnDomWaterAvailable = Convert.ToBoolean(drv["DomWaterAvailable"]);
				// Select "DomWaterAvailable" RadioButtonList Choice
				if (blnDomWaterAvailable == true)
				{
					rbDomWaterAvailable.Items[0].Selected = true;
				}
				else
				{
					rbDomWaterAvailable.Items[1].Selected = true;
				}

				// Retrieve "DomWaterProvider" TextBox
				TextBox tbDomWaterProvider;
				tbDomWaterProvider = (TextBox)e.Item.FindControl("txtDomWaterProvider");
				// Fill "DomWaterProvider" TextBox
				tbDomWaterProvider.Text = drv["DomWaterProvider"].ToString();

				// Retrieve "IrrWaterAvailable" RadioButtonList
				RadioButtonList rbIrrWaterAvailable;
				rbIrrWaterAvailable = (RadioButtonList)e.Item.FindControl("rblIrrWaterAvailable");
				// Convert "IrrWaterAvailable" to Boolean
				bool blnIrrWaterAvailable = Convert.ToBoolean(drv["IrrWaterAvailable"]);
				// Select "IrrWaterAvailable" RadioButtonList Choice
				if (blnIrrWaterAvailable == true)
				{
					rbIrrWaterAvailable.Items[0].Selected = true;
				}
				else
				{
					rbIrrWaterAvailable.Items[1].Selected = true;
				}

				// Retrieve "IrrWaterProvider" TextBox
				TextBox tbIrrWaterProvider;
				tbIrrWaterProvider = (TextBox)e.Item.FindControl("txtIrrWaterProvider");
				// Fill "IrrWaterProvider" TextBox
				tbIrrWaterProvider.Text = drv["IrrWaterProvider"].ToString();

				// Retrieve "IrrWaterDescription" TextBox
				TextBox tbIrrWaterDescription;
				tbIrrWaterDescription = (TextBox)e.Item.FindControl("txtIrrWaterDescription");
				// Fill "IrrWaterDescription" TextBox
				tbIrrWaterDescription.Text = drv["IrrWaterDescription"].ToString();

				// Retrieve "MineralRightsDescription" TextBox
				TextBox tbMineralRightsDescription;
				tbMineralRightsDescription = (TextBox)e.Item.FindControl("txtMineralRightsDescription");
				// Fill "MineralRightsDescription" TextBox
				tbMineralRightsDescription.Text = drv["MineralRightsDescription"].ToString();

				// Retrieve "ElectricityAvailable" RadioButtonList
				RadioButtonList rbElectricityAvailable;
				rbElectricityAvailable = (RadioButtonList)e.Item.FindControl("rblElectricityAvailable");
				// Convert "ElectricityAvailable" to Boolean
				bool blnElectricityAvailable = Convert.ToBoolean(drv["ElectricityAvailable"]);
				// Select "ElectricityAvailable" RadioButtonList Choice
				if (blnElectricityAvailable == true)
				{
					rbElectricityAvailable.Items[0].Selected = true;
				}
				else
				{
					rbElectricityAvailable.Items[1].Selected = true;
				}

				// Retrieve "ElectricityProvider" DropDownList
				DropDownList ddElectricityProvider;
				ddElectricityProvider = (DropDownList)e.Item.FindControl("ddlElectricityProvider");
				
				// Create ListItem For "ElectricityProvider" DropDownList
				ListItem selectElectricityProvider = new ListItem("-- Select Electricity Provider --", "");
				// Insert ListItem Into "ElectricityProvider" DropDownList
				ddElectricityProvider.Items.Insert(0, selectElectricityProvider);

				// Try selecting "ElectricityProvider" value
				// Catch if value is not valid and set top value
				try
				{
					// Select "ElectricityProvider" DropDownList Choice
					ddElectricityProvider.Items.FindByText(drv["ElectricityProvider"].ToString()).Selected = true;
				}
				catch
				{
					// Select "ElectricityProvider" DropDownList Choice
					ddElectricityProvider.SelectedIndex = 0;
				}

				// Retrieve "NaturalGasAvailable" RadioButtonList
				RadioButtonList rbNaturalGasAvailable;
				rbNaturalGasAvailable = (RadioButtonList)e.Item.FindControl("rblNaturalGasAvailable");

				// Convert "NaturalGasAvailable" to Boolean
				bool blnNaturalGasAvailable = Convert.ToBoolean(drv["NaturalGasAvailable"]);
				// Select "NaturalGasAvailable" RadioButtonList Choice
				if (blnNaturalGasAvailable == true)
				{
					rbNaturalGasAvailable.Items[0].Selected = true;
				}
				else
				{
					rbNaturalGasAvailable.Items[1].Selected = true;
				}

				// Retrieve "NaturalGasProvider" DropDownList
				DropDownList ddNaturalGasProvider;
				ddNaturalGasProvider = (DropDownList)e.Item.FindControl("ddlNaturalGasProvider");

				// Create ListItem For "NaturalGasProvider" DropDownList
				ListItem selectNaturalGasProvider = new ListItem("-- Select Natural Gas Provider --", "");
				// Insert ListItem Into "NaturalGasProvider" DropDownList
				ddNaturalGasProvider.Items.Insert(0, selectNaturalGasProvider);

				// Try selecting "NaturalGasProvider" value
				// Catch if value is not valid and set top value
				try
				{
					// Select "NaturalGasProvider" DropDownList Choice
					ddNaturalGasProvider.Items.FindByText(drv["NaturalGasProvider"].ToString()).Selected = true;
				}
				catch
				{
					// Select "NaturalGasProvider" DropDownList Choice
					ddNaturalGasProvider.SelectedIndex = 0;
				}

				// Retrieve "SewerInstalled" RadioButtonList
				RadioButtonList rbSewerInstalled;
				rbSewerInstalled = (RadioButtonList)e.Item.FindControl("rblSewerInstalled");
				// Convert "SewerInstalled" to Boolean
				bool blnSewerInstalled = Convert.ToBoolean(drv["SewerInstalled"]);
				// Select "SewerInstalled" RadioButtonList Choice
				if (blnSewerInstalled == true)
				{
					rbSewerInstalled.Items[0].Selected = true;
				}
				else
				{
					rbSewerInstalled.Items[1].Selected = true;
				}

				// Retrieve "Sewer" RadioButtonList
				RadioButtonList rblSewer;
				rblSewer = (RadioButtonList)e.Item.FindControl("rblSewer");
				// Try selecting "Sewer" value
				// Catch if value is not valid and set top value
				try
				{
					// Select "Sewer" RadioButtonList Choice
					rblSewer.Items.FindByText(drv["Sewer"].ToString()).Selected = true;
				}
				catch
				{
					// Select "Sewer" RadioButtonList Choice
					rblSewer.SelectedIndex = 2;
				}

				// Retrieve "Propane" CheckBox
				CheckBox cbPropane;
				cbPropane = (CheckBox)e.Item.FindControl("chbPropane");
				// Fill "Propane" CheckBox
				cbPropane.Checked = Convert.ToBoolean(drv["Propane"]);

				// Retrieve "Telephone" CheckBox
				CheckBox cbTelephone;
				cbTelephone = (CheckBox)e.Item.FindControl("chbTelephone");
				// Fill "Telephone" CheckBox
				cbTelephone.Checked = Convert.ToBoolean(drv["Telephone"]);

				// Retrieve "Possession" DropDownList
				DropDownList ddPossession;
				ddPossession = (DropDownList)e.Item.FindControl("ddlPossession");
				
				// Create ListItem For "Possession" DropDownList
				ListItem selectPossession = new ListItem("-- Select Possession --", "");
				// Insert ListItem Into "Possession" DropDownList
				ddPossession.Items.Insert(0, selectPossession);

				// Try selecting "Possession" value
				// Catch if value is not valid and set top value
				try
				{
					// Select "Possession" DropDownList Choice
					ddPossession.Items.FindByText(drv["Possession"].ToString()).Selected = true;
				}
				catch
				{
					// Select "Possession" DropDownList Choice
					ddPossession.SelectedIndex = 0;
				}

				// Retrieve "EarnestMoney" TextBox
				TextBox tbEarnestMoney;
				tbEarnestMoney = (TextBox)e.Item.FindControl("txtEarnestMoney");
				// Fill "EarnestMoney" TextBox
				tbEarnestMoney.Text = ConvertToNumeric(drv["EarnestMoney"]);

				// Retrieve "FeaturesDescription" TextBox
				TextBox tbFeaturesDescription;
				tbFeaturesDescription = (TextBox)e.Item.FindControl("txtFeaturesDescription");
				// Fill "FeaturesDescription" TextBox
				tbFeaturesDescription.Text = drv["FeaturesDescription"].ToString();

				// Retrieve "DisclosuresDescription" TextBox
				TextBox tbDisclosuresDescription;
				tbDisclosuresDescription = (TextBox)e.Item.FindControl("txtDisclosuresDescription");
				// Fill "DisclosuresDescription" TextBox
				tbDisclosuresDescription.Text = drv["DisclosuresDescription"].ToString();

				// Retrieve "MapDirections" TextBox
				TextBox tbMapDirections;
				tbMapDirections = (TextBox)e.Item.FindControl("txtMapDirections");
				// Fill "MapDirections" TextBox
				tbMapDirections.Text = drv["MapDirections"].ToString();
			}
		}
	}
}