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
	/// <summary>
	/// Summary description for ManageCommercialRealEstate.
	/// </summary>
	public class ManageCommercialRealEstate : Main
	{
		public Label lblListingDateTime;
		public Label lblLastEditDateTime;
		public HtmlGenericControl hgcErrors;
		public DataList dlCommercialRE;

		public ManageCommercialRealEstate()
		{
			//
			// TODO: Add constructor logic here
			//
		}

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
			objCommand.CommandText = "sp_select_commercial_re";
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
			sda.Fill(ds, "CommercialRE");

			// Fill ListingDateTime Label
			DateTime dtListingDateTime = Convert.ToDateTime(ds.Tables["CommercialRE"].Rows[0]["ListingDateTime"]);
			lblListingDateTime.Text = dtListingDateTime.ToString("D");

			// Get Variable Type
			string getType;
			getType = ds.Tables["CommercialRE"].Rows[0]["LastEditDateTime"].GetType().ToString();

			// Check to see if LastEditDateTime field is null
			// If not DataBind LastEditDateTime label
			if (getType != "System.DBNull")
			{
				DateTime dtNow = DateTime.Now;

				// Fill LastEditDateTime Label
				DateTime dtLastEdit = Convert.ToDateTime(ds.Tables["CommercialRE"].Rows[0]["LastEditDateTime"]);
				lblLastEditDateTime.Text = dtLastEdit.ToString("D");

				if (dtNow.Date == dtLastEdit.Date)
				{
					lblLastEditDateTime.CssClass = "red";
				}
			}

			// DataBind() Residential DataList
			dlCommercialRE.DataSource = ds.Tables["CommercialRE"].DefaultView;
			dlCommercialRE.DataBind();
		}

		public void CommercialRE_ItemCreated(Object sender, DataListItemEventArgs e)
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

				// Create String to Connect to Residential XML Config File
				string strVirtualPath = Request.ApplicationPath + "/config/commercial_re.xml";

				// Read XML Config File into DataSet
				ds.ReadXml(Request.MapPath(strVirtualPath), XmlReadMode.InferSchema);

				// Retrieve "Style" DropDownList
				DropDownList ddlStyle;
				ddlStyle = (DropDownList)e.Item.FindControl("ddlStyle");

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

				// Retrieve "Foundation" DropDownList
				DropDownList ddlFoundation;
				ddlFoundation = (DropDownList)e.Item.FindControl("ddlFoundation");

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

				// Retrieve "Construction" DropDownList
				DropDownList ddlConstruction;
				ddlConstruction = (DropDownList)e.Item.FindControl("ddlConstruction");

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

				// Retrieve "Roof" DropDownList
				DropDownList ddlRoof;
				ddlRoof = (DropDownList)e.Item.FindControl("ddlRoof");

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

				// Retrieve "Heating" DropDownList
				DropDownList ddlHeating;
				ddlHeating = (DropDownList)e.Item.FindControl("ddlHeating");

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

				// Retrieve "ElectricityProvider" DropDownList
				DropDownList ddlElectricityProvider;
				ddlElectricityProvider = (DropDownList)e.Item.FindControl("ddlElectricityProvider");

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

				// Retrieve "NaturalGasProvider" DropDownList
				DropDownList ddlNaturalGasProvider;
				ddlNaturalGasProvider = (DropDownList)e.Item.FindControl("ddlNaturalGasProvider");

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

				// Retrieve "Possession" DropDownList
				DropDownList ddlPossession;
				ddlPossession = (DropDownList)e.Item.FindControl("ddlPossession");

				// DataBind "Possession" DropDownList
				ddlPossession.DataSource = ds.Tables["Possession"].DefaultView;
				ddlPossession.DataValueField = "PossessionValue";
				ddlPossession.DataTextField = "PossessionValue";
				ddlPossession.DataBind();

				// Add ListItem For "Possession" DropDownList
				ListItem selectPossession = new ListItem("-- Select Possession --", "");
				// Insert ListItem Into "Possession" DropDownList
				ddlPossession.Items.Insert(0, selectPossession);

				// Retrieve "Terms" DropDownList
				DropDownList ddlTerms;
				ddlTerms = (DropDownList)e.Item.FindControl("ddlTerms");

				// DataBind "Terms" DropDownList
				ddlTerms.DataSource = ds.Tables["Terms"].DefaultView;
				ddlTerms.DataValueField = "TermsValue";
				ddlTerms.DataTextField = "TermsValue";
				ddlTerms.DataBind();

				// Add ListItem For "Terms" DropDownList
				ListItem selectTerms = new ListItem("-- Select Terms --", "");
				// Insert ListItem Into "Terms" DropDownList
				ddlTerms.Items.Insert(0, selectTerms);
			}
		}

		public void CommercialRE_Edit(Object sender, DataListCommandEventArgs e)
		{
			dlCommercialRE.EditItemIndex = e.Item.ItemIndex;
			hgcErrors.Visible = false;
			Bind();
		}

		public void CommercialRE_Cancel(Object sender, DataListCommandEventArgs e)
		{
			dlCommercialRE.EditItemIndex = -1;
			hgcErrors.Visible = false;
			Bind();
		}

		public void CommercialRE_Delete(Object sender, DataListCommandEventArgs e)
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
			SqlCommand objCommand = new SqlCommand("sp_delete_commercial_re", objConnection);
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
				Response.Redirect("browse_commercial_re.aspx", true);
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

		public void CommercialRE_Update(Object sender, DataListCommandEventArgs e)
		{
			// Create Connection String
			string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);

			// Create SqlCommand Object
			SqlCommand objCommand = new SqlCommand("sp_update_commercial_re", objConnection);
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

			// Retrieve "RPAnnualTaxes" TextBox
			TextBox tbRPAnnualTaxes;
			tbRPAnnualTaxes = (TextBox)e.Item.FindControl("txtRPAnnualTaxes");
			// Check "RPAnnualTaxes" TextBox
			if (CheckTextBox(tbRPAnnualTaxes))
			{
				// Add "RPAnnualTaxes" Parameter
				objParam = objCommand.Parameters.Add("@RPAnnualTaxes", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbRPAnnualTaxes.Text);
			}

			// Retrieve "RPAnnualTaxYear" TextBox
			TextBox tbRPAnnualTaxYear;
			tbRPAnnualTaxYear = (TextBox)e.Item.FindControl("txtRPAnnualTaxYear");
			// Check "RPAnnualTaxYear" TextBox
			if (CheckTextBox(tbRPAnnualTaxYear))
			{
				// Add "RPAnnualTaxYear" Parameter
				objParam = objCommand.Parameters.Add("@RPAnnualTaxYear", SqlDbType.Char, 4); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbRPAnnualTaxYear.Text);
			}

			// Retrieve "RPScheduleNumber" TextBox
			TextBox tbRPScheduleNumber;
			tbRPScheduleNumber = (TextBox)e.Item.FindControl("txtRPScheduleNumber");
			// Check "RPScheduleNumber" TextBox
			if (CheckTextBox(tbRPScheduleNumber))
			{
				//Add "RPScheduleNumber" Parameter
				objParam = objCommand.Parameters.Add("@RPScheduleNumber", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbRPScheduleNumber.Text);
			}

			// Retrieve "PPAnnualTaxes" TextBox
			TextBox tbPPAnnualTaxes;
			tbPPAnnualTaxes = (TextBox)e.Item.FindControl("txtPPAnnualTaxes");
			// Check "PPAnnualTaxes" TextBox
			if (CheckTextBox(tbPPAnnualTaxes))
			{
				// Add "PPAnnualTaxes" Parameter
				objParam = objCommand.Parameters.Add("@PPAnnualTaxes", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbPPAnnualTaxes.Text);
			}

			// Retrieve "PPAnnualTaxYear" TextBox
			TextBox tbPPAnnualTaxYear;
			tbPPAnnualTaxYear = (TextBox)e.Item.FindControl("txtPPAnnualTaxYear");
			// Check "PPAnnualTaxYear" TextBox
			if (CheckTextBox(tbPPAnnualTaxYear))
			{
				// Add "PPAnnualTaxYear" Parameter
				objParam = objCommand.Parameters.Add("@PPAnnualTaxYear", SqlDbType.Char, 4); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbPPAnnualTaxYear.Text);
			}

			// Retrieve "PPScheduleNumber" TextBox
			TextBox tbPPScheduleNumber;
			tbPPScheduleNumber = (TextBox)e.Item.FindControl("txtPPScheduleNumber");
			// Check "PPScheduleNumber" TextBox
			if (CheckTextBox(tbPPScheduleNumber))
			{
				//Add "PPScheduleNumber" Parameter
				objParam = objCommand.Parameters.Add("@PPScheduleNumber", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbPPScheduleNumber.Text);
			}

			// Retrieve "PropertyDescription" TextBox
			TextBox tbPropertyDescription;
			tbPropertyDescription = (TextBox)e.Item.FindControl("txtPropertyDescription");
			// Add "PropertyDescription" Parameter
			objParam = objCommand.Parameters.Add("@PropertyDescription", SqlDbType.VarChar, 500); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbPropertyDescription.Text);

			// Retrieve "BuildingLayoutDescription" TextBox
			TextBox tbBuildingLayoutDescription;
			tbBuildingLayoutDescription = (TextBox)e.Item.FindControl("txtBuildingLayoutDescription");
			if (CheckTextBox(tbBuildingLayoutDescription))
			{
				// Add "BuildingLayoutDescription" Parameter
				objParam = objCommand.Parameters.Add("@BuildingLayoutDescription", SqlDbType.VarChar, 500); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbBuildingLayoutDescription.Text);
			}

			// Retrieve "FixturesFurnitureEquipment" TextBox
			TextBox tbFixturesFurnitureEquipment;
			tbFixturesFurnitureEquipment = (TextBox)e.Item.FindControl("txtFixturesFurnitureEquipment");
			// Add "FixturesFurnitureEquipment" Parameter
			objParam = objCommand.Parameters.Add("@FixturesFurnitureEquipment", SqlDbType.VarChar, 500); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbFixturesFurnitureEquipment.Text);

			// Retrieve "Frontage" TextBox
			TextBox tbFrontage;
			tbFrontage = (TextBox)e.Item.FindControl("txtFrontage");
			if (CheckTextBox(tbFrontage))
			{
				// Add "Frontage" Parameter
				objParam = objCommand.Parameters.Add("@Frontage", SqlDbType.VarChar, 250); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbFrontage.Text);
			}

			// Retrieve "Parking" TextBox
			TextBox tbParking;
			tbParking = (TextBox)e.Item.FindControl("txtParking");
			if (CheckTextBox(tbParking))
			{
				// Add "Parking" Parameter
				objParam = objCommand.Parameters.Add("@Parking", SqlDbType.VarChar, 250); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbParking.Text);
			}

			// Retrieve "TotalSquareFt" TextBox
			TextBox tbTotalSquareFt;
			tbTotalSquareFt = (TextBox)e.Item.FindControl("txtTotalSquareFt");
			// Add "TotalSquareFt" Parameter
			objParam = objCommand.Parameters.Add("@TotalSquareFt", SqlDbType.Float); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToDouble(tbTotalSquareFt.Text);

			// Retrieve "LandSize" TextBox
			TextBox tbLandSize;
			tbLandSize = (TextBox)e.Item.FindControl("txtLandSize");
			// Add "LandSize" Parameter
			objParam = objCommand.Parameters.Add("@LandSize", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbLandSize.Text);

			// Retrieve "YearBuilt" TextBox
			TextBox tbYearBuilt;
			tbYearBuilt = (TextBox)e.Item.FindControl("txtYearBuilt");
			// Add "YearBuilt" Parameter
			objParam = objCommand.Parameters.Add("@YearBuilt", SqlDbType.Char, 4); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbYearBuilt.Text);

			// Retrieve "YearRemodeled" TextBox
			TextBox tbYearRemodeled;
			tbYearRemodeled = (TextBox)e.Item.FindControl("txtYearRemodeled");
			if (CheckTextBox(tbYearRemodeled))
			{
				// Add "YearRemodeled" Parameter
				objParam = objCommand.Parameters.Add("@YearRemodeled", SqlDbType.Char, 4); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbYearRemodeled.Text);
			}

			// Retrieve "Style" DropDownList
			DropDownList ddStyle;
			ddStyle = (DropDownList)e.Item.FindControl("ddlStyle");
			// Add "Style" Parameter
			objParam = objCommand.Parameters.Add("@Style", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddStyle.SelectedItem.Value);

			// Retrieve "Foundation" DropDownList
			DropDownList ddFoundation;
			ddFoundation = (DropDownList)e.Item.FindControl("ddlFoundation");
			// Add "Foundation" Parameter
			objParam = objCommand.Parameters.Add("@Foundation", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddFoundation.SelectedItem.Value);

			// Retrieve "Construction" DropDownList
			DropDownList ddConstruction;
			ddConstruction = (DropDownList)e.Item.FindControl("ddlConstruction");
			// Add "Construction" Parameter
			objParam = objCommand.Parameters.Add("@Construction", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddConstruction.SelectedItem.Value);

			// Retrieve "Roof" DropDownList
			DropDownList ddRoof;
			ddRoof = (DropDownList)e.Item.FindControl("ddlRoof");
			// Add "Roof" Parameter
			objParam = objCommand.Parameters.Add("@Roof", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddRoof.SelectedItem.Value);

			// Retrieve "Heating" DropDownList
			DropDownList ddHeating;
			ddHeating = (DropDownList)e.Item.FindControl("ddlHeating");
			// Add "Heating" Parameter
			objParam = objCommand.Parameters.Add("@Heating", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddHeating.SelectedItem.Value);

			// Retrieve "ElectricityProvider" DropDownList
			DropDownList ddElectricityProvider;
			ddElectricityProvider = (DropDownList)e.Item.FindControl("ddlElectricityProvider");
			// Add "ElectricityProvider" Parameter
			objParam = objCommand.Parameters.Add("@ElectricityProvider", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddElectricityProvider.SelectedItem.Value);

			// Retrieve "DomWaterProvider" TextBox
			TextBox tbDomWaterProvider;
			tbDomWaterProvider = (TextBox)e.Item.FindControl("txtDomWaterProvider");
			// Add "DomWaterProvider" Parameter
			objParam = objCommand.Parameters.Add("@DomWaterProvider", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbDomWaterProvider.Text);

			// Retrieve "NaturalGasProvider" DropDownList
			DropDownList ddNaturalGasProvider;
			ddNaturalGasProvider = (DropDownList)e.Item.FindControl("ddlNaturalGasProvider");
			// Add "NaturalGasProvider" Parameter
			objParam = objCommand.Parameters.Add("@NaturalGasProvider", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddNaturalGasProvider.SelectedItem.Value);

			// Retrieve "SewerSeptic" RadioButtonList
			RadioButtonList rblSewerSeptic;
			rblSewerSeptic = (RadioButtonList)e.Item.FindControl("rblSewerSeptic");
			// Add "SewerSeptic" Parameter
			objParam = objCommand.Parameters.Add("@SewerSeptic", SqlDbType.VarChar, 20); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(rblSewerSeptic.SelectedItem.Value);

			// Retrieve "Possession" DropDownList
			DropDownList ddPossession;
			ddPossession = (DropDownList)e.Item.FindControl("ddlPossession");
			// Add "Possession" Parameter
			objParam = objCommand.Parameters.Add("@Possession", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddPossession.SelectedItem.Value);

			// Retrieve "Terms" DropDownList
			DropDownList ddTerms;
			ddTerms = (DropDownList)e.Item.FindControl("ddlTerms");
			// Add "Terms" Parameter
			objParam = objCommand.Parameters.Add("@Terms", SqlDbType.VarChar, 30); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddTerms.SelectedItem.Value);

			// Retrieve "Features" TextBox
			TextBox tbFeatures;
			tbFeatures = (TextBox)e.Item.FindControl("txtFeatures");
			// Add "Features" Parameter
			objParam = objCommand.Parameters.Add("@Features", SqlDbType.VarChar, 750); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbFeatures.Text);

			// Retrieve "Inclusions" TextBox
			TextBox tbInclusions;
			tbInclusions = (TextBox)e.Item.FindControl("txtInclusions");
			// Add "Inclusions" Parameter
			objParam = objCommand.Parameters.Add("@Inclusions", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbInclusions.Text);

			// Retrieve "Exclusions" TextBox
			TextBox tbExclusions;
			tbExclusions = (TextBox)e.Item.FindControl("txtExclusions");
			// Add "Exclusions" Parameter
			objParam = objCommand.Parameters.Add("@Exclusions", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbExclusions.Text);

			// Retrieve "Disclosures" TextBox
			TextBox tbDisclosures;
			tbDisclosures = (TextBox)e.Item.FindControl("txtDisclosures");
			// Check "Disclosures" TextBox
			if (CheckTextBox(tbDisclosures))
			{
				// Add "Disclosures" Parameter
				objParam = objCommand.Parameters.Add("@Disclosures", SqlDbType.VarChar, 250); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbDisclosures.Text);
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
				objParam.Value = Convert.ToString(tbMapDirections.Text);
			}

			// Update database record
			// Catch any errors and display
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
				hgcErrors.InnerHtml += "<li>Error Message:" + objError.Message +
					"</li>" + "\n";
				hgcErrors.InnerHtml += "<li>Error Source:" + objError.Source +
					"</li>" + "\n";
				hgcErrors.InnerHtml += "</ul>" + "\n";
				return;
			}

			dlCommercialRE.EditItemIndex = -1;

			// Set MLS Cookie
			Session["mlsID"] = Convert.ToInt32(tbMLS.Text);

			Bind();
		}

		public void CommercialRE_DataBound(Object sender, DataListItemEventArgs e) 
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

				// Retrieve "RPAnnualTaxes" TableCell
				TableCell tcRPAnnualTaxes;
				tcRPAnnualTaxes = (TableCell)e.Item.FindControl("tcRPAnnualTaxes");
				// DataBind "RPAnnualTaxes" TableCell
				tcRPAnnualTaxes.Text = ConvertToMoney(drv["RPAnnualTaxes"]);

				// Retrieve "RPAnnualTaxYear" TableCell
				TableCell tcRPAnnualTaxYear;
				tcRPAnnualTaxYear = (TableCell)e.Item.FindControl("tcRPAnnualTaxYear");
				// DataBind "RPAnnualTaxYear" TableCell
				tcRPAnnualTaxYear.Text = ConvertToString(drv["RPAnnualTaxYear"]);

				// Retrieve "RPScheduleNumber" TableCell
				TableCell tcRPScheduleNumber;
				tcRPScheduleNumber = (TableCell)e.Item.FindControl("tcRPScheduleNumber");
				// DataBind "RPScheduleNumber" TableCell
				tcRPScheduleNumber.Text = ConvertToString(drv["RPScheduleNumber"]);

				// Retrieve "PPAnnualTaxes" TableCell
				TableCell tcPPAnnualTaxes;
				tcPPAnnualTaxes = (TableCell)e.Item.FindControl("tcPPAnnualTaxes");
				// DataBind "PPAnnualTaxes" TableCell
				tcPPAnnualTaxes.Text = ConvertToMoney(drv["PPAnnualTaxes"]);

				// Retrieve "PPAnnualTaxYear" TableCell
				TableCell tcPPAnnualTaxYear;
				tcPPAnnualTaxYear = (TableCell)e.Item.FindControl("tcPPAnnualTaxYear");
				// DataBind "PPAnnualTaxYear" TableCell
				tcPPAnnualTaxYear.Text = ConvertToString(drv["PPAnnualTaxYear"]);

				// Retrieve "PPScheduleNumber" TableCell
				TableCell tcPPScheduleNumber;
				tcPPScheduleNumber = (TableCell)e.Item.FindControl("tcPPScheduleNumber");
				// DataBind "PPScheduleNumber" TableCell
				tcPPScheduleNumber.Text = ConvertToString(drv["PPScheduleNumber"]);

				// Retrieve "PropertyDescription" TableCell
				TableCell tcPropertyDescription;
				tcPropertyDescription = (TableCell)e.Item.FindControl("tcPropertyDescription");
				// DataBind "PropertyDescription" TableCell
				tcPropertyDescription.Text = drv["PropertyDescription"].ToString();

				// Retrieve "BuildingLayoutDescription" TableCell
				TableCell tcBuildingLayoutDescription;
				tcBuildingLayoutDescription = (TableCell)e.Item.FindControl("tcBuildingLayoutDescription");
				// DataBind "BuildingLayoutDescription" TableCell
				tcBuildingLayoutDescription.Text = ConvertToString(drv["BuildingLayoutDescription"]);

				// Retrieve "FixturesFurnitureEquipment" TableCell
				TableCell tcFixturesFurnitureEquipment;
				tcFixturesFurnitureEquipment = (TableCell)e.Item.FindControl("tcFixturesFurnitureEquipment");
				// DataBind "FixturesFurnitureEquipment" TableCell
				tcFixturesFurnitureEquipment.Text = drv["FixturesFurnitureEquipment"].ToString();

				// Retrieve "Frontage" TableCell
				TableCell tcFrontage;
				tcFrontage = (TableCell)e.Item.FindControl("tcFrontage");
				// DataBind "Frontage" TableCell
				tcFrontage.Text = ConvertToString(drv["Frontage"]);

				// Retrieve "Parking" TableCell
				TableCell tcParking;
				tcParking = (TableCell)e.Item.FindControl("tcParking");
				// DataBind "Parking" TableCell
				tcParking.Text = ConvertToString(drv["Parking"]);

				// Retrieve "TotalSquareFt" TableCell
				TableCell tcTotalSquareFt;
				tcTotalSquareFt = (TableCell)e.Item.FindControl("tcTotalSquareFt");
				// DataBind "TotalSquareFt" TableCell
				tcTotalSquareFt.Text = drv["TotalSquareFt"].ToString();

				// Retrieve "LandSize" TableCell
				TableCell tcLandSize;
				tcLandSize = (TableCell)e.Item.FindControl("tcLandSize");
				// DataBind "LandSize" TableCell
				tcLandSize.Text = drv["LandSize"].ToString();

				// Retrieve "YearBuilt" TableCell
				TableCell tcYearBuilt;
				tcYearBuilt = (TableCell)e.Item.FindControl("tcYearBuilt");
				// DataBind "YearBuilt" TableCell
				tcYearBuilt.Text = drv["YearBuilt"].ToString();

				// Retrieve "YearRemodeled" TableCell
				TableCell tcYearRemodeled;
				tcYearRemodeled = (TableCell)e.Item.FindControl("tcYearRemodeled");
				// DataBind "YearRemodeled" TableCell
				tcYearRemodeled.Text = ConvertToString(drv["YearRemodeled"]);

				// Retrieve "Style" TableCell
				TableCell tcStyle;
				tcStyle = (TableCell)e.Item.FindControl("tcStyle");
				// DataBind "Style" TableCell
				tcStyle.Text = drv["Style"].ToString();

				// Retrieve "Foundation" TableCell
				TableCell tcFoundation;
				tcFoundation = (TableCell)e.Item.FindControl("tcFoundation");
				// DataBind "Foundation" TableCell
				tcFoundation.Text = drv["Foundation"].ToString();

				// Retrieve "Construction" TableCell
				TableCell tcConstruction;
				tcConstruction = (TableCell)e.Item.FindControl("tcConstruction");
				// DataBind "Construction" TableCell
				tcConstruction.Text = drv["Construction"].ToString();

				// Retrieve "Roof" TableCell
				TableCell tcRoof;
				tcRoof = (TableCell)e.Item.FindControl("tcRoof");
				// DataBind "Roof" TableCell
				tcRoof.Text = drv["Roof"].ToString();

				// Retrieve "Heating" TableCell
				TableCell tcHeating;
				tcHeating = (TableCell)e.Item.FindControl("tcHeating");
				// DataBind "Heating" TableCell
				tcHeating.Text = drv["Heating"].ToString();

				// Retrieve "ElectricityProvider" TableCell
				TableCell tcElectricityProvider;
				tcElectricityProvider = (TableCell)e.Item.FindControl("tcElectricityProvider");
				// DataBind "ElectricityProvider" TableCell
				tcElectricityProvider.Text = drv["ElectricityProvider"].ToString();

				// Retrieve "DomWaterProvider" TableCell
				TableCell tcDomWaterProvider;
				tcDomWaterProvider = (TableCell)e.Item.FindControl("tcDomWaterProvider");
				// DataBind "DomWaterProvider" TableCell
				tcDomWaterProvider.Text = drv["DomWaterProvider"].ToString();

				// Retrieve "NaturalGasProvider" TableCell
				TableCell tcNaturalGasProvider;
				tcNaturalGasProvider = (TableCell)e.Item.FindControl("tcNaturalGasProvider");
				// DataBind "NaturalGasProvider" TableCell
				tcNaturalGasProvider.Text = drv["NaturalGasProvider"].ToString();

				// Retrieve "SewerSeptic" TableCell
				TableCell tcSewerSeptic;
				tcSewerSeptic = (TableCell)e.Item.FindControl("tcSewerSeptic");
				// DataBind "SewerSeptic" TableCell
				tcSewerSeptic.Text = drv["SewerSeptic"].ToString();

				// Retrieve "Possession" TableCell
				TableCell tcPossession;
				tcPossession = (TableCell)e.Item.FindControl("tcPossession");
				// DataBind "Possession" TableCell
				tcPossession.Text = drv["Possession"].ToString();

				// Retrieve "Terms" TableCell
				TableCell tcTerms;
				tcTerms = (TableCell)e.Item.FindControl("tcTerms");
				// DataBind "Terms" TableCell
				tcTerms.Text = drv["Terms"].ToString();

				// Retrieve "Features" TableCell
				TableCell tcFeatures;
				tcFeatures = (TableCell)e.Item.FindControl("tcFeatures");
				// DataBind "Features" TableCell
				tcFeatures.Text = drv["Features"].ToString();

				// Retrieve "Inclusions" TableCell
				TableCell tcInclusions;
				tcInclusions = (TableCell)e.Item.FindControl("tcInclusions");
				// DataBind "Inclusions" TableCell
				tcInclusions.Text = drv["Inclusions"].ToString();

				// Retrieve "Exclusions" TableCell
				TableCell tcExclusions;
				tcExclusions = (TableCell)e.Item.FindControl("tcExclusions");
				// DataBind "Exclusions" TableCell
				tcExclusions.Text = drv["Exclusions"].ToString();

				// Retrieve "Disclosures" TableCell
				TableCell tcDisclosures;
				tcDisclosures = (TableCell)e.Item.FindControl("tcDisclosures");
				// DataBind "Disclosures" TableCell
				tcDisclosures.Text = ConvertToString(drv["Disclosures"]);

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

				// Retrieve "MLS" TextBox
				TextBox tbMLS;
				tbMLS = (TextBox)e.Item.FindControl("txtMLS");
				// Fill "MLS" TextBox
				tbMLS.Text = drv["MLS"].ToString();
				// Disable "MLS" TextBox
				tbMLS.Enabled = false;

				// Retrieve "AltMLS" TextBox
				TextBox tbAltMLS;
				tbAltMLS = (TextBox)e.Item.FindControl("txtAltMLS");
				// Fill "AltMLS" TextBox
				tbAltMLS.Text = drv["AltMLS"].ToString();

				// Retrieve "Price" TextBox
				TextBox tbPrice;
				tbPrice = (TextBox)e.Item.FindControl("txtPrice");
				// Fill "Price" TextBox
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

				// Retrieve "Owner" TextBox
				TextBox tbOwner;
				tbOwner = (TextBox)e.Item.FindControl("txtOwner");
				// Fill "Owner" TextBox
				tbOwner.Text = drv["Owner"].ToString();

				// Retrieve "Tagline" TextBox
				TextBox tbTagline;
				tbTagline = (TextBox)e.Item.FindControl("txtTagline");
				// Fill "Tagline" TextBox
				tbTagline.Text = drv["Tagline"].ToString();

				// Retrieve "Address1" TextBox
				TextBox tbAddress1;
				tbAddress1 = (TextBox)e.Item.FindControl("txtAddress1");
				// Fill "Address1" TextBox
				tbAddress1.Text = drv["Address1"].ToString();

				// Retrieve "Address2" TextBox
				TextBox tbAddress2;
				tbAddress2 = (TextBox)e.Item.FindControl("txtAddress2");
				// Fill "Address2" TextBox
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

				// Retrieve "RPAnnualTaxes" TextBox
				TextBox tbRPAnnualTaxes;
				tbRPAnnualTaxes = (TextBox)e.Item.FindControl("txtRPAnnualTaxes");
				// Fill "RPAnnualTaxes" TextBox
				tbRPAnnualTaxes.Text = ConvertToNumeric(drv["RPAnnualTaxes"]);	
				
				// Retrieve "RPAnnualTaxYear" TextBox
				TextBox tbRPAnnualTaxYear;
				tbRPAnnualTaxYear = (TextBox)e.Item.FindControl("txtRPAnnualTaxYear");
				// Fill "RPAnnualTaxYear" TextBox
				tbRPAnnualTaxYear.Text = drv["RPAnnualTaxYear"].ToString();

				// Retrieve "RPScheduleNumber" TextBox
				TextBox tbRPScheduleNumber;
				tbRPScheduleNumber = (TextBox)e.Item.FindControl("txtRPScheduleNumber");
				// Fill "RPScheduleNumber" TextBox
				tbRPScheduleNumber.Text = drv["RPScheduleNumber"].ToString();

				// Retrieve "PPAnnualTaxes" TextBox
				TextBox tbPPAnnualTaxes;
				tbPPAnnualTaxes = (TextBox)e.Item.FindControl("txtPPAnnualTaxes");
				// Fill "PPAnnualTaxes" TextBox
				tbPPAnnualTaxes.Text = ConvertToNumeric(drv["PPAnnualTaxes"]);	
				
				// Retrieve "PPAnnualTaxYear" TextBox
				TextBox tbPPAnnualTaxYear;
				tbPPAnnualTaxYear = (TextBox)e.Item.FindControl("txtPPAnnualTaxYear");
				// Fill "PPAnnualTaxYear" TextBox
				tbPPAnnualTaxYear.Text = drv["PPAnnualTaxYear"].ToString();

				// Retrieve "PPScheduleNumber" TextBox
				TextBox tbPPScheduleNumber;
				tbPPScheduleNumber = (TextBox)e.Item.FindControl("txtPPScheduleNumber");
				// Fill "PPScheduleNumber" TextBox
				tbPPScheduleNumber.Text = drv["PPScheduleNumber"].ToString();

				// Retrieve "PropertyDescription" TextBox
				TextBox tbPropertyDescription;
				tbPropertyDescription = (TextBox)e.Item.FindControl("txtPropertyDescription");
				// Fill "PropertyDescription" TextBox
				tbPropertyDescription.Text = drv["PropertyDescription"].ToString();

				// Retrieve "BuildingLayoutDescription" TextBox
				TextBox tbBuildingLayoutDescription;
				tbBuildingLayoutDescription = (TextBox)e.Item.FindControl("txtBuildingLayoutDescription");
				// Fill "BuildingLayoutDescription" TextBox
				tbBuildingLayoutDescription.Text = drv["BuildingLayoutDescription"].ToString();

				// Retrieve "FixturesFurnitureEquipment" TextBox
				TextBox tbFixturesFurnitureEquipment;
				tbFixturesFurnitureEquipment = (TextBox)e.Item.FindControl("txtFixturesFurnitureEquipment");
				// Fill "FixturesFurnitureEquipment" TextBox
				tbFixturesFurnitureEquipment.Text = drv["FixturesFurnitureEquipment"].ToString();

				// Retrieve "Frontage" TextBox
				TextBox tbFrontage;
				tbFrontage = (TextBox)e.Item.FindControl("txtFrontage");
				// Fill "Frontage" TextBox
				tbFrontage.Text = drv["Frontage"].ToString();

				// Retrieve "Parking" TextBox
				TextBox tbParking;
				tbParking = (TextBox)e.Item.FindControl("txtParking");
				// Fill "Parking" TextBox
				tbParking.Text = drv["Parking"].ToString();

				// Retrieve "TotalSquareFt" TextBox
				TextBox tbTotalSquareFt;
				tbTotalSquareFt = (TextBox)e.Item.FindControl("txtTotalSquareFt");
				// Fill "TotalSquareFt" TextBox
				tbTotalSquareFt.Text = drv["TotalSquareFt"].ToString();

				// Retrieve "LandSize" TextBox
				TextBox tbLandSize;
				tbLandSize = (TextBox)e.Item.FindControl("txtLandSize");
				// Fill "LandSize" TextBox
				tbLandSize.Text = drv["LandSize"].ToString();

				// Retrieve "YearBuilt" TextBox
				TextBox tbYearBuilt;
				tbYearBuilt = (TextBox)e.Item.FindControl("txtYearBuilt");
				// Fill "YearBuilt" TextBox
				tbYearBuilt.Text = drv["YearBuilt"].ToString();

				// Retrieve "YearRemodeled" TextBox
				TextBox tbYearRemodeled;
				tbYearRemodeled = (TextBox)e.Item.FindControl("txtYearRemodeled");
				// Fill "YearRemodeled" TextBox
				tbYearRemodeled.Text = drv["YearRemodeled"].ToString();

				// Retrieve "Style" DropDownList
				DropDownList ddStyle;
				ddStyle = (DropDownList)e.Item.FindControl("ddlStyle");
				// Try selecting "Style" value
				// Catch if value is not valid and set top value
				try
				{
					// Select "Style" DropDownList Choice
					ddStyle.Items.FindByText(drv["Style"].ToString()).Selected = true;
				}
				catch
				{
					// Select "Style" DropDownList Choice
					ddStyle.SelectedIndex = 0;
				}

				// Retrieve "Foundation" DropDownList
				DropDownList ddFoundation;
				ddFoundation = (DropDownList)e.Item.FindControl("ddlFoundation");
				// Try selecting "Foundation" value
				// Catch if value is not valid and set top value
				try
				{
					// Select "Foundation" DropDownList Choice
					ddFoundation.Items.FindByText(drv["Foundation"].ToString()).Selected = true;
				}
				catch
				{
					// Select "Foundation" DropDownList Choice
					ddFoundation.SelectedIndex = 0;
				}

				// Retrieve "Construction" DropDownList
				DropDownList ddConstruction;
				ddConstruction = (DropDownList)e.Item.FindControl("ddlConstruction");
				// Try selecting "Construction" value
				// Catch if value is not valid and set top value
				try
				{
					// Select "Construction" DropDownList Choice
					ddConstruction.Items.FindByText(drv["Construction"].ToString()).Selected = true;
				}
				catch
				{
					// Select "Construction" DropDownList Choice
					ddConstruction.SelectedIndex = 0;
				}

				// Retrieve "Roof" DropDownList
				DropDownList ddRoof;
				ddRoof = (DropDownList)e.Item.FindControl("ddlRoof");
				// Try selecting "Roof" value
				// Catch if value is not valid and set top value
				try
				{
					// Select "Roof" DropDownList Choice
					ddRoof.Items.FindByText(drv["Roof"].ToString()).Selected = true;
				}
				catch
				{
					// Select "Roof" DropDownList Choice
					ddRoof.SelectedIndex = 0;
				}

				// Retrieve "Heating" DropDownList
				DropDownList ddHeating;
				ddHeating = (DropDownList)e.Item.FindControl("ddlHeating");
				// Try selecting "Heating" value
				// Catch if value is not valid and set top value
				try
				{
					// Select "Heating" DropDownList Choice
					ddHeating.Items.FindByText(drv["Heating"].ToString()).Selected = true;
				}
				catch
				{
					// Select "Heating" DropDownList Choice
					ddHeating.SelectedIndex = 0;
				}

				// Retrieve "ElectricityProvider" DropDownList
				DropDownList ddElectricityProvider;
				ddElectricityProvider = (DropDownList)e.Item.FindControl("ddlElectricityProvider");
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

				// Retrieve "DomWaterProvider" TextBox
				TextBox tbDomWaterProvider;
				tbDomWaterProvider = (TextBox)e.Item.FindControl("txtDomWaterProvider");
				// Fill "DomWaterProvider" TextBox
				tbDomWaterProvider.Text = drv["DomWaterProvider"].ToString();

				// Retrieve "NaturalGasProvider" DropDownList
				DropDownList ddNaturalGasProvider;
				ddNaturalGasProvider = (DropDownList)e.Item.FindControl("ddlNaturalGasProvider");
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

				// Retrieve "SewerSeptic" RadioButtonList
				RadioButtonList rbSewerSeptic;
				rbSewerSeptic = (RadioButtonList)e.Item.FindControl("rblSewerSeptic");
				// Try selecting "SewerSeptic" value
				// Catch if value is not found
				try
				{
					// Select "SewerSeptic" RadioButtonList Choice
					rbSewerSeptic.Items.FindByText(drv["SewerSeptic"].ToString()).Selected = true;
				}
				catch
				{
					// Select 0 Index
					rbSewerSeptic.SelectedIndex = 0;
				}

				// Retrieve "Possession" DropDownList
				DropDownList ddPossession;
				ddPossession = (DropDownList)e.Item.FindControl("ddlPossession");
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

				// Retrieve "Terms" DropDownList
				DropDownList ddTerms;
				ddTerms = (DropDownList)e.Item.FindControl("ddlTerms");
				// Try selecting "Terms" value
				// Catch if value is not valid and set top value
				try
				{
					// Select "Terms" DropDownList Choice
					ddTerms.Items.FindByText(drv["Terms"].ToString()).Selected = true;
				}
				catch
				{
					// Select "Terms" DropDownList Choice
					ddTerms.SelectedIndex = 0;
				}

				// Retrieve "Features" TextBox
				TextBox tbFeatures;
				tbFeatures = (TextBox)e.Item.FindControl("txtFeatures");
				// Fill "Features" TextBox
				tbFeatures.Text = drv["Features"].ToString();

				// Retrieve "Inclusions" TextBox
				TextBox tbInclusions;
				tbInclusions = (TextBox)e.Item.FindControl("txtInclusions");
				// Fill "Inclusions" TextBox
				tbInclusions.Text = drv["Inclusions"].ToString();

				// Retrieve "Exclusions" TextBox
				TextBox tbExclusions;
				tbExclusions = (TextBox)e.Item.FindControl("txtExclusions");
				// Fill "Exclusions" TextBox
				tbExclusions.Text = drv["Exclusions"].ToString();

				// Retrieve "Disclosures" TextBox
				TextBox tbDisclosures;
				tbDisclosures = (TextBox)e.Item.FindControl("txtDisclosures");
				// Fill "Disclosures" TextBox
				tbDisclosures.Text = drv["Disclosures"].ToString();

				// Retrieve "MapDirections" TextBox
				TextBox tbMapDirections;
				tbMapDirections = (TextBox)e.Item.FindControl("txtMapDirections");
				// Fill "MapDirections" TextBox
				tbMapDirections.Text = drv["MapDirections"].ToString();
			}
		}
	}
}
