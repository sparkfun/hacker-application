using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroupAdmin
{
	public class ManageFarmRanch : Main
	{
		public Label lblListingDateTime;
		public Label lblLastEditDateTime;
		public HtmlGenericControl hgcErrors;
		public DataList dlFarmRanchProperty;

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
			objCommand.CommandText = "sp_select_farmranch";
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create SqlParameter Object
			SqlParameter objParam;

			// Add "MLS" Parameter
			objParam = objCommand.Parameters.Add("@MLS", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(Session["mlsID"]);

			// Create SqlDataAdapter Object
			SqlDataAdapter sda = new SqlDataAdapter(objCommand);

			// Create DataSet Object
			DataSet ds = new DataSet();

			// Fill DataSet
			sda.Fill(ds, "FarmRanch");

			// Fill ListingDateTime Label
			DateTime dtListingDateTime = Convert.ToDateTime(ds.Tables["FarmRanch"].Rows[0]["ListingDateTime"]);
			lblListingDateTime.Text = dtListingDateTime.ToString("D");

			// Get Variable Type
			string getType;
			getType = ds.Tables["FarmRanch"].Rows[0]["LastEditDateTime"].GetType().ToString();

			// Check to see if LastEditDateTime field is null
			// If not DataBind LastEditDateTime label
			if (getType != "System.DBNull")
			{
				DateTime dtNow = DateTime.Now;

				// Fill LastEditDateTime Label
				DateTime dtLastEdit = Convert.ToDateTime(ds.Tables["FarmRanch"].Rows[0]["LastEditDateTime"]);
				lblLastEditDateTime.Text = dtLastEdit.ToString("D");

				if (dtNow.Date == dtLastEdit.Date)
				{
					lblLastEditDateTime.CssClass = "red";
				}
			}

			// DataBind() Farm & Ranch DataList
			dlFarmRanchProperty.DataSource = ds.Tables["FarmRanch"].DefaultView;
			dlFarmRanchProperty.DataBind();
		}

		public void FarmRanch_ItemCreated(Object sender, DataListItemEventArgs e)
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
				string strVirtualPath = Request.ApplicationPath + "/config/residential_dropdown.xml";

				// Read XML into DataSet
				ds.ReadXml(Request.MapPath(strVirtualPath), XmlReadMode.InferSchema);		

				// Retrieve the Style DropDownList
				DropDownList ddStyle;
				ddStyle = (DropDownList)e.Item.FindControl("ddlStyle");
				// DataBind() Style DropDownList
				ddStyle.DataSource = ds.Tables["Style"].DefaultView;
				ddStyle.DataValueField = "StyleValue";
				ddStyle.DataTextField = "StyleText";
				ddStyle.DataBind();
				
				// Retrieve Foundation DropDownList
				DropDownList ddFoundation;
				ddFoundation = (DropDownList)e.Item.FindControl("ddlFoundation");
				// DataBind() Foundation DropDownList
				ddFoundation.DataSource = ds.Tables["Foundation"].DefaultView;
				ddFoundation.DataValueField = "FoundationValue";
				ddFoundation.DataTextField = "FoundationText";
				ddFoundation.DataBind();

				// Retrieve Construction DropDownList
				DropDownList ddConstruction;
				ddConstruction = (DropDownList)e.Item.FindControl("ddlConstruction");
				// DataBind() Construction DropDownList
				ddConstruction.DataSource = ds.Tables["Construction"].DefaultView;
				ddConstruction.DataValueField = "ConstructionValue";
				ddConstruction.DataTextField = "ConstructionText";
				ddConstruction.DataBind();

				// Retrieve Roof DropDownList
				DropDownList ddRoof;
				ddRoof = (DropDownList)e.Item.FindControl("ddlRoof");
				// DataBind() Roof DropDownList
				ddRoof.DataSource = ds.Tables["Roof"].DefaultView;
				ddRoof.DataValueField = "RoofValue";
				ddRoof.DataTextField = "RoofText";
				ddRoof.DataBind();

				// Retrieve Garage DropDownList
				DropDownList ddGarage;
				ddGarage = (DropDownList)e.Item.FindControl("ddlGarage");
				// DataBind() Garage DropDownList
				ddGarage.DataSource = ds.Tables["Garage"].DefaultView;
				ddGarage.DataValueField = "GarageValue";
				ddGarage.DataTextField = "GarageText";
				ddGarage.DataBind();

				// Retrieve Heating DropDownList
				DropDownList ddHeating;
				ddHeating = (DropDownList)e.Item.FindControl("ddlHeating");
				// DataBind() Heating DropDownList
				ddHeating.DataSource = ds.Tables["Heating"].DefaultView;
				ddHeating.DataValueField = "HeatingValue";
				ddHeating.DataTextField = "HeatingText";
				ddHeating.DataBind();

				// Retrieve ElectricityProvider DropDownList
				DropDownList ddElectricityProvider;
				ddElectricityProvider = (DropDownList)e.Item.FindControl("ddlElectricityProvider");
				// DataBind() ElectricityProvider DropDownList
				ddElectricityProvider.DataSource = ds.Tables["ElectricityProviders"].DefaultView;
				ddElectricityProvider.DataValueField = "ElectricityProviderValue";
				ddElectricityProvider.DataTextField = "ElectricityProviderText";
				ddElectricityProvider.DataBind();

				// Retrieve Possession DropDownList
				DropDownList ddPossession;
				ddPossession = (DropDownList)e.Item.FindControl("ddlPossession");
				// DataBind() Possession DropDownList
				ddPossession.DataSource = ds.Tables["Possession"].DefaultView;
				ddPossession.DataValueField = "PossessionValue";
				ddPossession.DataTextField = "PossessionText";
				ddPossession.DataBind();
			}
		}

		public void FarmRanch_Edit(Object sender, DataListCommandEventArgs e)
		{
			dlFarmRanchProperty.EditItemIndex = e.Item.ItemIndex;
			hgcErrors.Visible = false;
			Bind();
		}

		public void FarmRanch_Cancel(Object sender, DataListCommandEventArgs e)
		{
			dlFarmRanchProperty.EditItemIndex = -1;
			hgcErrors.Visible = false;
			Bind();
		}

		public void FarmRanch_Delete(Object sender, DataListCommandEventArgs e)
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
			SqlCommand objCommand = new SqlCommand("sp_delete_farmranch", objConnection);
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
				Response.Redirect("browse_farm_ranch.aspx", true);
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

		public void FarmRanch_Update(Object sender, DataListCommandEventArgs e)
		{
			// Create Connection String
			string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);

			// Create SqlCommand Object
			SqlCommand objCommand = new SqlCommand("sp_update_farmranch", objConnection);
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

			// Retrieve "TotalAcres" TextBox
			TextBox tbTotalAcres;
			tbTotalAcres = (TextBox)e.Item.FindControl("txtTotalAcres");
			// Add "TotalAcres" Parameter
			objParam = objCommand.Parameters.Add("@TotalAcres", SqlDbType.Float);
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToDouble(tbTotalAcres.Text);

			// Retrieve "ApproxIrrAcres" TextBox
			TextBox tbApproxIrrAcres;
			tbApproxIrrAcres = (TextBox)e.Item.FindControl("txtApproxIrrAcres");
			// Add "ApproxIrrAcres" Parameter
			objParam = objCommand.Parameters.Add("@ApproxIrrAcres", SqlDbType.Float);
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToDouble(tbApproxIrrAcres.Text);

			// Retrieve "IrrWaterDescription" TextBox
			TextBox tbIrrWaterDescription;
			tbIrrWaterDescription = (TextBox)e.Item.FindControl("txtIrrWaterDescription");
			// Add "IrrWaterDescription" Parameter
			objParam = objCommand.Parameters.Add("@IrrWaterDescription", SqlDbType.VarChar, 250);
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbIrrWaterDescription.Text);

			// Retrieve "IrrWaterCost" TextBox
			TextBox tbIrrWaterCost;
			tbIrrWaterCost = (TextBox)e.Item.FindControl("txtIrrWaterCost");
			// Add "IrrWaterCost" Parameter
			objParam = objCommand.Parameters.Add("@IrrWaterCost", SqlDbType.VarChar, 250);
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbIrrWaterCost.Text);

			// Retrieve "SurfaceWater" TextBox
			TextBox tbSurfaceWater;
			tbSurfaceWater = (TextBox)e.Item.FindControl("txtSurfaceWater");
			// Check "SurfaceWater" TextBox
			if (CheckTextBox(tbSurfaceWater))
			{
				// Add "SurfaceWater" Parameter
				objParam = objCommand.Parameters.Add("@SurfaceWater", SqlDbType.VarChar, 80);
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbSurfaceWater.Text);
			}

			// Retrieve "TopoFeatures" TextBox
			TextBox tbTopoFeatures;
			tbTopoFeatures = (TextBox)e.Item.FindControl("txtTopoFeatures");
			// Check "TopoFeatures" TextBox
			if (CheckTextBox(tbTopoFeatures))
			{
				// Add "TopoFeatures" Parameter
				objParam = objCommand.Parameters.Add("@TopoFeatures", SqlDbType.VarChar, 80);
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbTopoFeatures.Text);
			}

			// Retrieve "FencingDescription" TextBox
			TextBox tbFencingDescription;
			tbFencingDescription = (TextBox)e.Item.FindControl("txtFencingDescription");
			// Add "FencingDescription" Parameter
			objParam = objCommand.Parameters.Add("@FencingDescription", SqlDbType.VarChar, 80);
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbFencingDescription.Text);

			// Retrieve "Easements" TextBox
			TextBox tbEasements;
			tbEasements = (TextBox)e.Item.FindControl("txtEasements");
			// Add "Easements" Parameter
			objParam = objCommand.Parameters.Add("@Easements", SqlDbType.VarChar, 80);
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbEasements.Text);

			// Retrieve "Livestock" RadioButtonList
			RadioButtonList rbLivestock;
			rbLivestock = (RadioButtonList)e.Item.FindControl("rblLivestock");
			// Add "Livestock" Parameter
			objParam = objCommand.Parameters.Add("@Livestock", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt16(rbLivestock.SelectedItem.Value);

			// Retrieve "Crops" RadioButtonList
			RadioButtonList rbCrops;
			rbCrops = (RadioButtonList)e.Item.FindControl("rblCrops");
			// Add "Crops" Parameter
			objParam = objCommand.Parameters.Add("@Crops", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt16(rbCrops.SelectedItem.Value);

			// Retrieve "Bedrooms" TextBox
			TextBox tbBedrooms;
			tbBedrooms = (TextBox)e.Item.FindControl("txtBedrooms");
			// Check "Bedrooms"
			if (CheckTextBox(tbBedrooms))
			{
				//Add "Bedrooms" Parameter
				objParam = objCommand.Parameters.Add("@Bedrooms", SqlDbType.Float); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbBedrooms.Text);
			}

			// Retrieve "Baths" TextBox
			TextBox tbBaths;
			tbBaths = (TextBox)e.Item.FindControl("txtBaths");
			// Check "Baths" TextBox
			if (CheckTextBox(tbBaths))
			{
				//Add "Baths" Parameter
				objParam = objCommand.Parameters.Add("@Baths", SqlDbType.Float); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbBaths.Text);
			}

			// Retrieve "SquareFt" TextBox
			TextBox tbSquareFt;
			tbSquareFt = (TextBox)e.Item.FindControl("txtSquareFt");
			// Check "SquareFt" TextBox
			if (CheckTextBox(tbSquareFt))
			{
				// Add "SquareFt" Parameter
				objParam = objCommand.Parameters.Add("@SquareFt", SqlDbType.Int); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToInt32(tbSquareFt.Text);
			}

			// Retrieve "YearBuilt" TextBox
			TextBox tbYearBuilt;
			tbYearBuilt = (TextBox)e.Item.FindControl("txtYearBuilt");
			// Check "YearBuilt" TextBox
			if (CheckTextBox(tbYearBuilt))
			{
				// Add "YearBuilt" Parameter
				objParam = objCommand.Parameters.Add("@YearBuilt", SqlDbType.Char, 4); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbYearBuilt.Text);
			}

			// Retrieve "YearRemodeled" TextBox
			TextBox tbYearRemodeled;
			tbYearRemodeled = (TextBox)e.Item.FindControl("txtYearRemodeled");
			// Check "YearRemodeled" TextBox
			if (CheckTextBox(tbYearRemodeled))
			{
				//Add "YearRemodeled" Parameter
				objParam = objCommand.Parameters.Add("@YearRemodeled", SqlDbType.Char, 4); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbYearRemodeled.Text);
			}

			// Retrieve "ParcelSize" TextBox
			TextBox tbParcelSize;
			tbParcelSize = (TextBox)e.Item.FindControl("txtParcelSize");
			// Check "ParcelSize" TextBox
			if (CheckTextBox(tbParcelSize))
			{
				//Add "ParcelSize" Parameter
				objParam = objCommand.Parameters.Add("@ParcelSize", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbParcelSize.Text);
			}

			// Retrieve "Style" DropDownList
			DropDownList ddStyle;
			ddStyle = (DropDownList)e.Item.FindControl("ddlStyle");
			// Check "Style" DropDownList
			if (CheckDropDownList(ddStyle))
			{
				// Add "Style" Parameter
				objParam = objCommand.Parameters.Add("@Style", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(ddStyle.SelectedItem.Value);
			}

			// Retrieve "Foundation" DropDownList
			DropDownList ddFoundation;
			ddFoundation = (DropDownList)e.Item.FindControl("ddlFoundation");
			// Check "Foundation" DropDownList
			if (CheckDropDownList(ddFoundation))
			{
				// Add "Foundation" Parameter
				objParam = objCommand.Parameters.Add("@Foundation", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(ddFoundation.SelectedItem.Value);
			}

			// Retrieve "Construction" DropDownList
			DropDownList ddConstruction;
			ddConstruction = (DropDownList)e.Item.FindControl("ddlConstruction");
			// Check "Construction" DropDownList
			if (CheckDropDownList(ddConstruction))
			{
				// Add "Construction" Parameter
				objParam = objCommand.Parameters.Add("@Construction", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(ddConstruction.SelectedItem.Value);
			}

			// Retrieve "Roof" DropDownList
			DropDownList ddRoof;
			ddRoof = (DropDownList)e.Item.FindControl("ddlRoof");
			// Check "Roof" DropDownList
			if (CheckDropDownList(ddRoof))
			{
				// Add "Roof" Parameter
				objParam = objCommand.Parameters.Add("@Roof", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(ddRoof.SelectedItem.Value);
			}

			// Retrieve "Garage" DropDownList
			DropDownList ddGarage;
			ddGarage = (DropDownList)e.Item.FindControl("ddlGarage");
			// Check "Garage" DropDownList
			if (CheckDropDownList(ddGarage))
			{
				// Add "Garage" Parameter
				objParam = objCommand.Parameters.Add("@Garage", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(ddGarage.SelectedItem.Value);
			}

			// Retrieve "Patio" RadioButtonList
			RadioButtonList rbPatio;
			rbPatio = (RadioButtonList)e.Item.FindControl("rblPatio");
			// Add "Patio" Parameter
			objParam = objCommand.Parameters.Add("@Patio", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt16(rbPatio.SelectedItem.Value);

			// Retrieve "Deck" RadioButtonList
			RadioButtonList rbDeck;
			rbDeck = (RadioButtonList)e.Item.FindControl("rblDeck");
			// Add "Deck" Parameter
			objParam = objCommand.Parameters.Add("@Deck", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt16(rbDeck.SelectedItem.Value);

			// Retrieve "Heating" DropDownList
			DropDownList ddHeating;
			ddHeating = (DropDownList)e.Item.FindControl("ddlHeating");
			// Check "Heating" DropDownList
			if (CheckDropDownList(ddHeating))
			{
				// Add "Heating" Parameter
				objParam = objCommand.Parameters.Add("@Heating", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(ddHeating.SelectedItem.Value);
			}

			// Retrieve "Fireplace" RadioButtonList
			RadioButtonList rbFireplace;
			rbFireplace = (RadioButtonList)e.Item.FindControl("rblFireplace");
			// Add "Fireplace" Parameter
			objParam = objCommand.Parameters.Add("@Fireplace", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt16(rbFireplace.SelectedItem.Value);

			// Retrieve "Woodstove" RadioButtonList
			RadioButtonList rbWoodstove;
			rbWoodstove = (RadioButtonList)e.Item.FindControl("rblWoodstove");
			// Add "Woodstove" Parameter
			objParam = objCommand.Parameters.Add("@Woodstove", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt16(rbWoodstove.SelectedItem.Value);

			// Retrieve "ElectricityProvider" DropDownList
			DropDownList ddElectricityProvider;
			ddElectricityProvider = (DropDownList)e.Item.FindControl("ddlElectricityProvider");
			// Check "ElectricityProvider" DropDownList
			if (CheckDropDownList(ddElectricityProvider))
			{
				//Add "ElectricityProvider" Parameter
				objParam = objCommand.Parameters.Add("@ElectricityProvider", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(ddElectricityProvider.SelectedItem.Value);
			}

			// Retrieve "ElectricityMonthlyCost" TextBox
			TextBox tbElectricityMonthlyCost;
			tbElectricityMonthlyCost = (TextBox)e.Item.FindControl("txtElectricityMonthlyCost");
			// Check "ElectricityMonthlyCost" TextBox
			if (CheckTextBox(tbElectricityMonthlyCost))
			{
				// Add "ElectricityMonthlyCost" Parameter
				objParam = objCommand.Parameters.Add("@ElectricityMonthlyCost", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbElectricityMonthlyCost.Text);
			}

			// Retrieve "DomWaterProvider" TextBox
			TextBox tbDomWaterProvider;
			tbDomWaterProvider = (TextBox)e.Item.FindControl("txtDomWaterProvider");
			// Check "DomWaterProvider" TextBox
			if (CheckTextBox(tbDomWaterProvider))
			{
				// Add "DomesticWaterProvider" Parameter
				objParam = objCommand.Parameters.Add("@DomWaterProvider", SqlDbType.VarChar, 50); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbDomWaterProvider.Text);
			}

			// Retrieve "DomWaterMonthlyCost" TextBox
			TextBox tbDomWaterMonthlyCost;
			tbDomWaterMonthlyCost = (TextBox)e.Item.FindControl("txtDomWaterMonthlyCost");
			// Check "DomWaterMonthlyCost" TextBox
			if (CheckTextBox(tbDomWaterMonthlyCost))
			{
				// Add "DomWaterMonthlyCost" Parameter
				objParam = objCommand.Parameters.Add("@DomWaterMonthlyCost", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbDomWaterMonthlyCost.Text);
			}

			// Retrieve "Sewer" RadioButtonList
			RadioButtonList rbSewer;
			rbSewer = (RadioButtonList)e.Item.FindControl("rblSewer");
			// Add "Sewer" Parameter
			objParam = objCommand.Parameters.Add("@Sewer", SqlDbType.Char, 6); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(rbSewer.SelectedItem.Value);

			// Retrieve "KitchenDim" TextBox
			TextBox tbKitchenDim;
			tbKitchenDim = (TextBox)e.Item.FindControl("txtKitchenDim");
			// Check "KitchenDim" TextBox
			if (CheckTextBox(tbKitchenDim))
			{
				// Add "KitchenDim" Parameter
				objParam = objCommand.Parameters.Add("@KitchenDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbKitchenDim.Text);
			}

			// Retrieve "LivingRoomDim" TextBox
			TextBox tbLivingRoomDim;
			tbLivingRoomDim = (TextBox)e.Item.FindControl("txtLivingRoomDim");
			// Check "LivingRoomDim" TextBox
			if (CheckTextBox(tbLivingRoomDim))
			{
				// Add "LivingRoomDim" Parameter
				objParam = objCommand.Parameters.Add("@LivingRoomDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbLivingRoomDim.Text);
			}

			// Retrieve "DiningRoomDim" TextBox
			TextBox tbDiningRoomDim;
			tbDiningRoomDim = (TextBox)e.Item.FindControl("txtDiningRoomDim");
			// Check "DiningRoomDim" TextBox
			if (CheckTextBox(tbDiningRoomDim))
			{
				// Add "DiningRoomDim" Parameter
				objParam = objCommand.Parameters.Add("@DiningRoomDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbDiningRoomDim.Text);
			}

			// Retrieve "FamilyRoomDim" TextBox
			TextBox tbFamilyRoomDim;
			tbFamilyRoomDim = (TextBox)e.Item.FindControl("txtFamilyRoomDim");
			// Check "FamilyRoomDim" TextBox
			if (CheckTextBox(tbFamilyRoomDim))
			{
				// Add "FamilyRoomDim" Parameter
				objParam = objCommand.Parameters.Add("@FamilyRoomDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbFamilyRoomDim.Text);
			}

			// Retrieve "MasterBedDim" TextBox
			TextBox tbMasterBedDim;
			tbMasterBedDim = (TextBox)e.Item.FindControl("txtMasterBedDim");
			// Check "MasterBedDim" TextBox
			if (CheckTextBox(tbMasterBedDim))
			{
				// Add "MasterBedDim" Parameter
				objParam = objCommand.Parameters.Add("@MasterBedDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbMasterBedDim.Text);
			}

			// Retrieve "Bedroom2Dim" TextBox
			TextBox tbBedroom2Dim;
			tbBedroom2Dim = (TextBox)e.Item.FindControl("txtBedroom2Dim");
			// Check "Bedrooom2Dim" TextBox
			if (CheckTextBox(tbBedroom2Dim))
			{
				// Add "Bedroom2Dim" Parameter
				objParam = objCommand.Parameters.Add("@Bedroom2Dim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbBedroom2Dim.Text);
			}

			// Retrieve "Bedroom3Dim" TextBox
			TextBox tbBedroom3Dim;
			tbBedroom3Dim = (TextBox)e.Item.FindControl("txtBedroom3Dim");
			// Check "Bedroom3Dim" TextBox
			if (CheckTextBox(tbBedroom3Dim))
			{
				// Add "Bedroom3Dim" Parameter
				objParam = objCommand.Parameters.Add("@Bedroom3Dim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbBedroom3Dim.Text);
			}

			// Retrieve "Bedroom4Dim" TextBox
			TextBox tbBedroom4Dim;
			tbBedroom4Dim = (TextBox)e.Item.FindControl("txtBedroom4Dim");
			// Check "Bedroom4Dim" TextBox
			if (CheckTextBox(tbBedroom4Dim))
			{
				// Add "Bedroom4Dim" Parameter
				objParam = objCommand.Parameters.Add("@Bedroom4Dim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbBedroom4Dim.Text);
			}

			// Retrieve "Bathroom1Dim" TextBox
			TextBox tbBathroom1Dim;
			tbBathroom1Dim = (TextBox)e.Item.FindControl("txtBathroom1Dim");
			// Check "Bathroom1Dim" TextBox
			if (CheckTextBox(tbBathroom1Dim))
			{
				// Add "Bathroom1Dim" Parameter
				objParam = objCommand.Parameters.Add("@Bathroom1Dim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbBathroom1Dim.Text);
			}

			// Retrieve "Bathroom2Dim" TextBox
			TextBox tbBathroom2Dim;
			tbBathroom2Dim = (TextBox)e.Item.FindControl("txtBathroom2Dim");
			// Check "Bathroom2Dim" TextBox
			if (CheckTextBox(tbBathroom2Dim))
			{
				// Add "Bathroom2Dim" Parameter
				objParam = objCommand.Parameters.Add("@Bathroom2Dim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbBathroom2Dim.Text);
			}

			// Retrieve "Bathroom3Dim" TextBox
			TextBox tbBathroom3Dim;
			tbBathroom3Dim = (TextBox)e.Item.FindControl("txtBathroom3Dim");
			// Check "Bathroom3Dim" TextBox
			if (CheckTextBox(tbBathroom3Dim))
			{
				// Add "Bathroom3Dim" Parameter
				objParam = objCommand.Parameters.Add("@Bathroom3Dim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbBathroom3Dim.Text);
			}

			// Retrieve "Bathroom4Dim" TextBox
			TextBox tbBathroom4Dim;
			tbBathroom4Dim = (TextBox)e.Item.FindControl("txtBathroom4Dim");
			// Check "Bathroom4Dim" TextBox
			if (CheckTextBox(tbBathroom4Dim))
			{
				// Add "Bathroom4Dim" Parameter
				objParam = objCommand.Parameters.Add("@Bathroom4Dim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbBathroom4Dim.Text);
			}

			// Retrieve "BasementDim" TextBox
			TextBox tbBasementDim;
			tbBasementDim = (TextBox)e.Item.FindControl("txtBasementDim");
			// Check "BasementDim" TextBox
			if (CheckTextBox(tbBasementDim))
			{
				// Add "BasementDim" Parameter
				objParam = objCommand.Parameters.Add("@BasementDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbBasementDim.Text);
			}

			// Retrieve "GarageDim" TextBox
			TextBox tbGarageDim;
			tbGarageDim = (TextBox)e.Item.FindControl("txtGarageDim");
			// Check "GarageDim" TextBox
			if (CheckTextBox(tbGarageDim))
			{
				// Add "GarageDim" Parameter
				objParam = objCommand.Parameters.Add("@GarageDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbGarageDim.Text);
			}

			// Retrieve "PatioDim" TextBox
			TextBox tbPatioDim;
			tbPatioDim = (TextBox)e.Item.FindControl("txtPatioDim");
			// Check "PatioDim" TextBox
			if (CheckTextBox(tbPatioDim))
			{
				// Add "PatioDim" Parameter
				objParam = objCommand.Parameters.Add("@PatioDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbPatioDim.Text);
			}

			// Retrieve "DeckDim" TextBox
			TextBox tbDeckDim;
			tbDeckDim = (TextBox)e.Item.FindControl("txtDeckDim");
			// Check "DeckDim" TextBox
			if (CheckTextBox(tbDeckDim))
			{
				// Add "DeckDim" Parameter
				objParam = objCommand.Parameters.Add("@DeckDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbDeckDim.Text);
			}

			// Retrieve "ShedDim" TextBox
			TextBox tbShedDim;
			tbShedDim = (TextBox)e.Item.FindControl("txtShedDim");
			// Check "ShedDim" TextBox
			if (CheckTextBox(tbShedDim))
			{
				// Add "ShedDim" Parameter
				objParam = objCommand.Parameters.Add("@ShedDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbShedDim.Text);
			}

			// Retrieve "OfficeDim" TextBox
			TextBox tbOfficeDim;
			tbOfficeDim = (TextBox)e.Item.FindControl("txtOfficeDim");
			// Check "OfficeDim" TextBox
			if (CheckTextBox(tbOfficeDim))
			{
				// Add "OfficeDim" Parameter
				objParam = objCommand.Parameters.Add("@OfficeDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbOfficeDim.Text);
			}

			// Retrieve "MediaRoomDim" TextBox
			TextBox tbMediaRoomDim;
			tbMediaRoomDim = (TextBox)e.Item.FindControl("txtMediaRoomDim");
			// Check "MediaRoomDim" TextBox
			if (CheckTextBox(tbMediaRoomDim))
			{
				// Add "MediaRoomDim" Parameter
				objParam = objCommand.Parameters.Add("@MediaRoomDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbMediaRoomDim.Text);
			}

			// Retrieve "LaundryRoomDim" TextBox
			TextBox tbLaundryRoomDim;
			tbLaundryRoomDim = (TextBox)e.Item.FindControl("txtLaundryRoomDim");
			// Check "LaundryRoomDim" TextBox
			if (CheckTextBox(tbLaundryRoomDim))
			{
				// Add "LaundryRoomDim" Parameter
				objParam = objCommand.Parameters.Add("@LaundryRoomDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbLaundryRoomDim.Text);
			}

			// Retrieve "SunroomDim" TextBox
			TextBox tbSunroomDim;
			tbSunroomDim = (TextBox)e.Item.FindControl("txtSunroomDim");
			// Check "SunroomDim" TextBox
			if (CheckTextBox(tbSunroomDim))
			{
				// Add "SunroomDim" Parameter
				objParam = objCommand.Parameters.Add("@SunroomDim", SqlDbType.VarChar, 30); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbSunroomDim.Text);
			}

			// Retrieve "Possession" DropDownList
			DropDownList ddPossession;
			ddPossession = (DropDownList)e.Item.FindControl("ddlPossession");
			// Add "Possession" Parameter
			objParam = objCommand.Parameters.Add("@Possession", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(ddPossession.SelectedItem.Value);

			// Retrieve "EarnestMoney" TextBox
			TextBox tbEarnestMoney;
			tbEarnestMoney = (TextBox)e.Item.FindControl("txtEarnestMoney");
			// Add "EarnestMoney" Parameter
			objParam = objCommand.Parameters.Add("@EarnestMoney", SqlDbType.Money); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToDouble(tbEarnestMoney.Text);

			// Retrieve "FeaturesDescription" TextBox
			TextBox tbFeaturesDescription;
			tbFeaturesDescription = (TextBox)e.Item.FindControl("txtFeaturesDescription");
			// Add "FeaturesDescription" Parameter
			objParam = objCommand.Parameters.Add("@FeaturesDescription", SqlDbType.VarChar, 750); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbFeaturesDescription.Text);

			// Retrieve "InclusionsDescription" TextBox
			TextBox tbInclusionsDescription;
			tbInclusionsDescription = (TextBox)e.Item.FindControl("txtInclusionsDescription");
			// Add "InclusionsDescription" Parameter
			objParam = objCommand.Parameters.Add("@InclusionsDescription", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbInclusionsDescription.Text);

			// Retrieve "ExclusionsDescription" TextBox
			TextBox tbExclusionsDescription;
			tbExclusionsDescription = (TextBox)e.Item.FindControl("txtExclusionsDescription");
			// Add "ExclusionsDescription" Parameter
			objParam = objCommand.Parameters.Add("@ExclusionsDescription", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbExclusionsDescription.Text);

			// Retrieve "OutbuildingsDescription" TextBox
			TextBox tbOutbuildingsDescription;
			tbOutbuildingsDescription = (TextBox)e.Item.FindControl("txtOutbuildingsDescription");
			// Check "OutbuildingsDescription" TextBox
			if (CheckTextBox(tbOutbuildingsDescription))
			{
				// Add "OutbuildingsDescription" Parameter
				objParam = objCommand.Parameters.Add("@OutbuildingsDescription", SqlDbType.VarChar, 250); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbOutbuildingsDescription.Text);
			}

			// Retrieve "DisclosuresDescription" TextBox
			TextBox tbDisclosuresDescription;
			tbDisclosuresDescription = (TextBox)e.Item.FindControl("txtDisclosuresDescription");
			// Check "DisclosuresDescription" TextBox
			if (CheckTextBox(tbDisclosuresDescription))
			{
				// Add "DisclosuresDescription" Parameter
				objParam = objCommand.Parameters.Add("@DisclosuresDescription", SqlDbType.VarChar, 250); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbDisclosuresDescription.Text);
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

			dlFarmRanchProperty.EditItemIndex = -1;

			// Set MLS Cookie
			Session["mlsID"] = Convert.ToInt32(tbMLS.Text);

			Bind();
		}

		public void FarmRanch_DataBound(Object sender, DataListItemEventArgs e) 
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

				// Retrieve "TotalAcres" TableCell
				TableCell tcTotalAcres;
				tcTotalAcres = (TableCell)e.Item.FindControl("tcTotalAcres");
				// DataBind "TotalAcres" TableCell
				tcTotalAcres.Text = ConvertToStringPlusMinus(drv["TotalAcres"]);

				// Retrieve "ApproxIrrAcres" TableCell
				TableCell tcApproxIrrAcres;
				tcApproxIrrAcres = (TableCell)e.Item.FindControl("tcApproxIrrAcres");
				// DataBind "ApproxIrrAcres" TableCell
				tcApproxIrrAcres.Text = ConvertToStringPlusMinus(drv["ApproxIrrAcres"]);

				// Retrieve "IrrWaterDescription" TableCell
				TableCell tcIrrWaterDescription;
				tcIrrWaterDescription = (TableCell)e.Item.FindControl("tcIrrWaterDescription");
				// DataBind "IrrWaterDescription" TableCell
				tcIrrWaterDescription.Text = drv["IrrWaterDescription"].ToString();

				// Retrieve "IrrWaterCost" TableCell
				TableCell tcIrrWaterCost;
				tcIrrWaterCost = (TableCell)e.Item.FindControl("tcIrrWaterCost");
				// DataBind "IrrWaterCost" TableCell
				tcIrrWaterCost.Text = drv["IrrWaterCost"].ToString();

				// Retrieve "SurfaceWater" TableCell
				TableCell tcSurfaceWater;
				tcSurfaceWater = (TableCell)e.Item.FindControl("tcSurfaceWater");
				// DataBind "SurfaceWater" TableCell
				tcSurfaceWater.Text = ConvertToString(drv["SurfaceWater"]);

				// Retrieve "TopoFeatures" TableCell
				TableCell tcTopoFeatures;
				tcTopoFeatures = (TableCell)e.Item.FindControl("tcTopoFeatures");
				// DataBind "TopoFeatures" TableCell
				tcTopoFeatures.Text = ConvertToString(drv["TopoFeatures"]);

				// Retrieve "FencingDescription" TableCell
				TableCell tcFencingDescription;
				tcFencingDescription = (TableCell)e.Item.FindControl("tcFencingDescription");
				// DataBind "FencingDescription" TableCell
				tcFencingDescription.Text = drv["FencingDescription"].ToString();

				// Retrieve "Easements" TableCell
				TableCell tcEasements;
				tcEasements = (TableCell)e.Item.FindControl("tcEasements");
				// DataBind "Easements" TableCell
				tcEasements.Text = drv["Easements"].ToString();

				// Retrieve "Livestock" TableCell
				TableCell tcLivestock;
				tcLivestock = (TableCell)e.Item.FindControl("tcLivestock");
				// DataBind "Livestock" TableCell
				tcLivestock.Text = ConvertToYesNo(drv["Livestock"]);

				// Retrieve "Crops" TableCell
				TableCell tcCrops;
				tcCrops = (TableCell)e.Item.FindControl("tcCrops");
				// DataBind "Crops" TableCell
				tcCrops.Text = ConvertToYesNo(drv["Crops"]);

				// Retrieve "Bedrooms" TableCell
				TableCell tcBedrooms;
				tcBedrooms = (TableCell)e.Item.FindControl("tcBedrooms");
				// DataBind "Bedrooms" TableCell
				tcBedrooms.Text = ConvertToString(drv["Bedrooms"]);

				// Retrieve "Baths" TableCell
				TableCell tcBaths;
				tcBaths = (TableCell)e.Item.FindControl("tcBaths");
				// DataBind "Baths" TableCell
				tcBaths.Text = ConvertToString(drv["Baths"]);

				// Retrieve "SquareFt" TableCell
				TableCell tcSquareFt;
				tcSquareFt = (TableCell)e.Item.FindControl("tcSquareFt");
				// DataBind "SquareFt" TableCell
				tcSquareFt.Text = ConvertToStringPlusMinus(drv["SquareFt"]);

				// Retrieve "YearBuilt" TableCell
				TableCell tcYearBuilt;
				tcYearBuilt = (TableCell)e.Item.FindControl("tcYearBuilt");
				// DataBind "YearBuilt" TableCell
				tcYearBuilt.Text = ConvertToString(drv["YearBuilt"]);

				// Retrieve "YearRemodeled" TableCell
				TableCell tcYearRemodeled;
				tcYearRemodeled = (TableCell)e.Item.FindControl("tcYearRemodeled");
				// DataBind "YearRemodeled" TableCell
				tcYearRemodeled.Text = ConvertToString(drv["YearRemodeled"]);

				// Retrieve "ParcelSize" TableCell
				TableCell tcParcelSize;
				tcParcelSize = (TableCell)e.Item.FindControl("tcParcelSize");
				// DataBind "ParcelSize" TableCell
				tcParcelSize.Text = ConvertToString(drv["ParcelSize"]);

				// Retrieve "Style" TableCell
				TableCell tcStyle;
				tcStyle = (TableCell)e.Item.FindControl("tcStyle");
				// DataBind "Style" TableCell
				tcStyle.Text = ConvertToString(drv["Style"]);

				// Retrieve "Foundation" TableCell
				TableCell tcFoundation;
				tcFoundation = (TableCell)e.Item.FindControl("tcFoundation");
				// DataBind "Foundation" TableCell
				tcFoundation.Text = ConvertToString(drv["Foundation"]);

				// Retrieve "Construction" TableCell
				TableCell tcConstruction;
				tcConstruction = (TableCell)e.Item.FindControl("tcConstruction");
				// DataBind "Construction" TableCell
				tcConstruction.Text = ConvertToString(drv["Construction"]);

				// Retrieve "Roof" TableCell
				TableCell tcRoof;
				tcRoof = (TableCell)e.Item.FindControl("tcRoof");
				// DataBind "Roof" TableCell
				tcRoof.Text = ConvertToString(drv["Roof"]);

				// Retrieve "Garage" TableCell
				TableCell tcGarage;
				tcGarage = (TableCell)e.Item.FindControl("tcGarage");
				// DataBind "Garage" TableCell
				tcGarage.Text = ConvertToString(drv["Garage"]);

				// Retrieve "Patio" TableCell
				TableCell tcPatio;
				tcPatio = (TableCell)e.Item.FindControl("tcPatio");
				// DataBind "Patio" TableCell
				tcPatio.Text = ConvertToYesNo(drv["Patio"]);

				// Retrieve "Deck" TableCell
				TableCell tcDeck;
				tcDeck = (TableCell)e.Item.FindControl("tcDeck");
				// DataBind "Deck" TableCell
				tcDeck.Text = ConvertToYesNo(drv["Deck"]);

				// Retrieve "Heating" TableCell
				TableCell tcHeating;
				tcHeating = (TableCell)e.Item.FindControl("tcHeating");
				// DataBind "Heating" TableCell
				tcHeating.Text = ConvertToString(drv["Heating"]);

				// Retrieve "Fireplace" TableCell
				TableCell tcFireplace;
				tcFireplace = (TableCell)e.Item.FindControl("tcFireplace");
				// DataBind "Fireplace" TableCell
				tcFireplace.Text = ConvertToYesNo(drv["Fireplace"]);
	
				// Retrieve "Woodstove" TableCell
				TableCell tcWoodstove;
				tcWoodstove = (TableCell)e.Item.FindControl("tcWoodstove");
				// DataBind "Woodstove" TableCell
				tcWoodstove.Text = ConvertToYesNo(drv["Woodstove"]);

				// Retrieve "ElectricityProvider" TableCell
				TableCell tcElectricityProvider;
				tcElectricityProvider = (TableCell)e.Item.FindControl("tcElectricityProvider");
				// DataBind "ElectricityProvider" TableCell
				tcElectricityProvider.Text = ConvertToString(drv["ElectricityProvider"]);

				// Retrieve "ElectricityMonthlyCost" TableCell
				TableCell tcElectricityMonthlyCost;
				tcElectricityMonthlyCost = (TableCell)e.Item.FindControl("tcElectricityMonthlyCost");
				// DataBind "ElectricityMonthlyCost" TableCell
				tcElectricityMonthlyCost.Text = ConvertToMoney(drv["ElectricityMonthlyCost"]);

				// Retrieve "DomWaterProvider" TableCell
				TableCell tcDomWaterProvider;
				tcDomWaterProvider = (TableCell)e.Item.FindControl("tcDomWaterProvider");
				// DataBind "DomWaterProvider" TableCell
				tcDomWaterProvider.Text = ConvertToString(drv["DomWaterProvider"]);

				// Retrieve "DomWaterMonthlyCost" TableCell
				TableCell tcDomWaterMonthlyCost;
				tcDomWaterMonthlyCost = (TableCell)e.Item.FindControl("tcDomWaterMonthlyCost");
				// DataBind "DomWaterMonthlyCost" TableCell
				tcDomWaterMonthlyCost.Text = ConvertToMoney(drv["DomWaterMonthlyCost"]);

				// Retrieve "Sewer" TableCell
				TableCell tcSewer;
				tcSewer = (TableCell)e.Item.FindControl("tcSewer");
				// DataBind "Sewer" TableCell
				tcSewer.Text = drv["Sewer"].ToString();

				// Retrieve "KitchenDim" TableCell
				TableCell tcKitchenDim;
				tcKitchenDim = (TableCell)e.Item.FindControl("tcKitchenDim");
				// DataBind "KitchenDim" TableCell
				tcKitchenDim.Text = ConvertToString(drv["KitchenDim"]);

				// Retrieve "LivingRoomDim" TableCell
				TableCell tcLivingRoomDim;
				tcLivingRoomDim = (TableCell)e.Item.FindControl("tcLivingRoomDim");
				// DataBind "LivingRoomDim" TableCell
				tcLivingRoomDim.Text = ConvertToString(drv["LivingRoomDim"]);

				// Retrieve "DiningRoomDim" TableCell
				TableCell tcDiningRoomDim;
				tcDiningRoomDim = (TableCell)e.Item.FindControl("tcDiningRoomDim");
				// DataBind "DiningRoomDim" TableCell
				tcDiningRoomDim.Text = ConvertToString(drv["DiningRoomDim"]);

				// Retrieve "FamilyRoomDim" TableCell
				TableCell tcFamilyRoomDim;
				tcFamilyRoomDim = (TableCell)e.Item.FindControl("tcFamilyRoomDim");
				// DataBind "FamilyRoomDim" TableCell
				tcFamilyRoomDim.Text = ConvertToString(drv["FamilyRoomDim"]);

				// Retrieve "MasterBedDim" TableCell
				TableCell tcMasterBedDim;
				tcMasterBedDim = (TableCell)e.Item.FindControl("tcMasterBedDim");
				// DataBind "MasterBedDim" TableCell
				tcMasterBedDim.Text = ConvertToString(drv["MasterBedDim"]);

				// Retrieve "Bedroom2Dim" TableCell
				TableCell tcBedroom2Dim;
				tcBedroom2Dim = (TableCell)e.Item.FindControl("tcBedroom2Dim");
				// DataBind "Bedroom2Dim" TableCell
				tcBedroom2Dim.Text = ConvertToString(drv["Bedroom2Dim"]);

				// Retrieve "Bedroom3Dim" TableCell
				TableCell tcBedroom3Dim;
				tcBedroom3Dim = (TableCell)e.Item.FindControl("tcBedroom3Dim");
				// DataBind "Bedroom3Dim" TableCell
				tcBedroom3Dim.Text = ConvertToString(drv["Bedroom3Dim"]);

				// Retrieve "Bedroom4Dim" TableCell
				TableCell tcBedroom4Dim;
				tcBedroom4Dim = (TableCell)e.Item.FindControl("tcBedroom4Dim");
				// DataBind "Bedroom4Dim" TableCell
				tcBedroom4Dim.Text = ConvertToString(drv["Bedroom4Dim"]);

				// Retrieve "Bathroom1Dim" TableCell
				TableCell tcBathroom1Dim;
				tcBathroom1Dim = (TableCell)e.Item.FindControl("tcBathroom1Dim");
				// DataBind "Bathroom1Dim" TableCell
				tcBathroom1Dim.Text = ConvertToString(drv["Bathroom1Dim"]);

				// Retrieve "Bathroom2Dim" TableCell
				TableCell tcBathroom2Dim;
				tcBathroom2Dim = (TableCell)e.Item.FindControl("tcBathroom2Dim");
				// DataBind "Bathroom2Dim" TableCell
				tcBathroom2Dim.Text = ConvertToString(drv["Bathroom2Dim"]);

				// Retrieve "Bathroom3Dim" TableCell
				TableCell tcBathroom3Dim;
				tcBathroom3Dim = (TableCell)e.Item.FindControl("tcBathroom3Dim");
				// DataBind "Bathroom3Dim" TableCell
				tcBathroom3Dim.Text = ConvertToString(drv["Bathroom3Dim"]);

				// Retrieve "Bathroom4Dim" TableCell
				TableCell tcBathroom4Dim;
				tcBathroom4Dim = (TableCell)e.Item.FindControl("tcBathroom4Dim");
				// DataBind "Bathroom4Dim" TableCell
				tcBathroom4Dim.Text = ConvertToString(drv["Bathroom4Dim"]);

				// Retrieve "BasementDim" TableCell
				TableCell tcBasementDim;
				tcBasementDim = (TableCell)e.Item.FindControl("tcBasementDim");
				// DataBind "BasementDim" TableCell
				tcBasementDim.Text = ConvertToString(drv["BasementDim"]);

				// Retrieve "GarageDim" TableCell
				TableCell tcGarageDim;
				tcGarageDim = (TableCell)e.Item.FindControl("tcGarageDim");
				// DataBind "GarageDim" TableCell
				tcGarageDim.Text = ConvertToString(drv["GarageDim"]);

				// Retrieve "PatioDim" TableCell
				TableCell tcPatioDim;
				tcPatioDim = (TableCell)e.Item.FindControl("tcPatioDim");
				// DataBind "PatioDim" TableCell
				tcPatioDim.Text = ConvertToString(drv["PatioDim"]);

				// Retrieve "DeckDim" TableCell
				TableCell tcDeckDim;
				tcDeckDim = (TableCell)e.Item.FindControl("tcDeckDim");
				// DataBind "DeckDim" TableCell
				tcDeckDim.Text = ConvertToString(drv["DeckDim"]);

				// Retrieve "ShedDim" TableCell
				TableCell tcShedDim;
				tcShedDim = (TableCell)e.Item.FindControl("tcShedDim");
				// DataBind "ShedDim" TableCell
				tcShedDim.Text = ConvertToString(drv["ShedDim"]);

				// Retrieve "OfficeDim" TableCell
				TableCell tcOfficeDim;
				tcOfficeDim = (TableCell)e.Item.FindControl("tcOfficeDim");
				// DataBind "OfficeDim" TableCell
				tcOfficeDim.Text = ConvertToString(drv["OfficeDim"]);

				// Retrieve "MediaRoomDim" TableCell
				TableCell tcMediaRoomDim;
				tcMediaRoomDim = (TableCell)e.Item.FindControl("tcMediaRoomDim");
				// DataBind "MediaRoomDim" TableCell
				tcMediaRoomDim.Text = ConvertToString(drv["MediaRoomDim"]);

				// Retrieve "LaundryRoomDim" TableCell
				TableCell tcLaundryRoomDim;
				tcLaundryRoomDim = (TableCell)e.Item.FindControl("tcLaundryRoomDim");
				// DataBind "LaundryRoomDim" TableCell
				tcLaundryRoomDim.Text = ConvertToString(drv["LaundryRoomDim"]);

				// Retrieve "SunroomDim" TableCell
				TableCell tcSunroomDim;
				tcSunroomDim = (TableCell)e.Item.FindControl("tcSunroomDim");
				// DataBind "SunroomDim" TableCell
				tcSunroomDim.Text = ConvertToString(drv["SunroomDim"]);

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

				// Retrieve "InclusionsDescription" TableCell
				TableCell tcInclusionsDescription;
				tcInclusionsDescription = (TableCell)e.Item.FindControl("tcInclusionsDescription");
				// DataBind "InclusionsDescription" TableCell
				tcInclusionsDescription.Text = drv["InclusionsDescription"].ToString();

				// Retrieve "ExclusionsDescription" TableCell
				TableCell tcExclusionsDescription;
				tcExclusionsDescription = (TableCell)e.Item.FindControl("tcExclusionsDescription");
				// DataBind "ExclusionsDescription" TableCell
				tcExclusionsDescription.Text = drv["ExclusionsDescription"].ToString();

				// Retrieve "OutbuildingsDescription" TableCell
				TableCell tcOutbuildingsDescription;
				tcOutbuildingsDescription = (TableCell)e.Item.FindControl("tcOutbuildingsDescription");
				// DataBind "OutbuildingsDescription" TableCell
				tcOutbuildingsDescription.Text = ConvertToString(drv["OutbuildingsDescription"]);

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

				// Retrieve "TotalAcres" TextBox
				TextBox tbTotalAcres;
				tbTotalAcres = (TextBox)e.Item.FindControl("txtTotalAcres");
				// Fill "TotalAcres" TextBox
				tbTotalAcres.Text = drv["TotalAcres"].ToString();

				// Retrieve "ApproxIrrAcres" TextBox
				TextBox tbApproxIrrAcres;
				tbApproxIrrAcres = (TextBox)e.Item.FindControl("txtApproxIrrAcres");
				// Fill "ApproxIrrAcres" TextBox
				tbApproxIrrAcres.Text = drv["ApproxIrrAcres"].ToString();

				// Retrieve "IrrWaterDescription" TextBox
				TextBox tbIrrWaterDescription;
				tbIrrWaterDescription = (TextBox)e.Item.FindControl("txtIrrWaterDescription");
				// Fill "IrrWaterDescription" TextBox
				tbIrrWaterDescription.Text = drv["IrrWaterDescription"].ToString();

				// Retrieve "IrrWaterCost" TextBox
				TextBox tbIrrWaterCost;
				tbIrrWaterCost = (TextBox)e.Item.FindControl("txtIrrWaterCost");
				// Fill "IrrWaterCost" TextBox
				tbIrrWaterCost.Text = drv["IrrWaterCost"].ToString();

				// Retrieve "SurfaceWater" TextBox
				TextBox tbSurfaceWater;
				tbSurfaceWater = (TextBox)e.Item.FindControl("txtSurfaceWater");
				// Fill "SurfaceWater" TextBox
				tbSurfaceWater.Text = drv["SurfaceWater"].ToString();

				// Retrieve "TopoFeatures" TextBox
				TextBox tbTopoFeatures;
				tbTopoFeatures = (TextBox)e.Item.FindControl("txtTopoFeatures");
				// Fill "TopoFeatures" TextBox
				tbTopoFeatures.Text = drv["TopoFeatures"].ToString();

				// Retrieve "FencingDescription" TextBox
				TextBox tbFencingDescription;
				tbFencingDescription = (TextBox)e.Item.FindControl("txtFencingDescription");
				// Fill "FencingDescription" TextBox
				tbFencingDescription.Text = drv["FencingDescription"].ToString();

				// Retrieve "Easements" TextBox
				TextBox tbEasements;
				tbEasements = (TextBox)e.Item.FindControl("txtEasements");
				// Fill "Easements" TextBox
				tbEasements.Text = drv["Easements"].ToString();

				// Retrieve "Livestock" RadioButtonList
				RadioButtonList rbLivestock;
				rbLivestock = (RadioButtonList)e.Item.FindControl("rblLivestock");
				// Convert "Livestock" to Boolean
				bool blnLivestock = Convert.ToBoolean(drv["Livestock"]);
				// Select "Livestock" RadioButtonList Choice
				if (blnLivestock == true)
				{
					rbLivestock.Items[0].Selected = true;
				}
				else
				{
					rbLivestock.Items[1].Selected = true;
				}

				// Retrieve "Crops" RadioButtonList
				RadioButtonList rbCrops;
				rbCrops = (RadioButtonList)e.Item.FindControl("rblCrops");
				// Convert "Crops" to Boolean
				bool blnCrops = Convert.ToBoolean(drv["Crops"]);
				// Select "Crops" RadioButtonList Choice
				if (blnCrops == true)
				{
					rbCrops.Items[0].Selected = true;
				}
				else
				{
					rbCrops.Items[1].Selected = true;
				}

				// Retrieve "Bedrooms" TextBox
				TextBox tbBedrooms;
				tbBedrooms = (TextBox)e.Item.FindControl("txtBedrooms");
				// Fill "Bedrooms" TextBox
				tbBedrooms.Text = drv["Bedrooms"].ToString();

				// Retrieve "Baths" TextBox
				TextBox tbBaths;
				tbBaths = (TextBox)e.Item.FindControl("txtBaths");
				// Fill "Baths" TextBox
				tbBaths.Text = drv["Baths"].ToString();

				// Retrieve "SquareFt" TextBox
				TextBox tbSquareFt;
				tbSquareFt = (TextBox)e.Item.FindControl("txtSquareFt");
				// Fill "SquareFt" TextBox
				tbSquareFt.Text = drv["SquareFt"].ToString();

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

				// Retrieve "ParcelSize" TextBox
				TextBox tbParcelSize;
				tbParcelSize = (TextBox)e.Item.FindControl("txtParcelSize");
				// Fill "ParcelSize" TextBox
				tbParcelSize.Text = drv["ParcelSize"].ToString();

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
					// Select "Style" DropDownList Choice
					ddFoundation.SelectedIndex = 0;
				}

				// Retrieve Construction DropDownList
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

				// Retrieve "Garage" DropDownList
				DropDownList ddGarage;
				ddGarage = (DropDownList)e.Item.FindControl("ddlGarage");
				// Try selecting "Garage" value
				// Catch if value is not valid and set top value
				try
				{
					// Select "Garage" DropDownList Choice
					ddGarage.Items.FindByText(drv["Garage"].ToString()).Selected = true;
				}
				catch
				{
					// Select "Garage" DropDownList Choice
					ddGarage.SelectedIndex = 0;
				}

				// Retrieve "Patio" RadioButtonList
				RadioButtonList rbPatio;
				rbPatio = (RadioButtonList)e.Item.FindControl("rblPatio");
				// Convert "Patio" to Boolean
				bool blnPatio = Convert.ToBoolean(drv["Patio"]);
				// Select "Patio" RadioButtonList Choice
				if (blnPatio == true)
				{
					rbPatio.Items[0].Selected = true;
				}
				else
				{
					rbPatio.Items[1].Selected = true;
				}

				// Retrieve "Deck" RadioButtonList
				RadioButtonList rbDeck;
				rbDeck = (RadioButtonList)e.Item.FindControl("rblDeck");
				// Convert "Deck" to Boolean
				bool blnDeck = Convert.ToBoolean(drv["Deck"]);
				// Select "Deck" RadioButtonList Choice
				if (blnDeck == true)
				{
					rbDeck.Items[0].Selected = true;
				}
				else
				{
					rbDeck.Items[1].Selected = true;
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

				// Retrieve "Fireplace" RadioButtonList
				RadioButtonList rbFireplace;
				rbFireplace = (RadioButtonList)e.Item.FindControl("rblFireplace");
				// Convert "Fireplace" to Boolean
				bool blnFireplace = Convert.ToBoolean(drv["Fireplace"]);
				// Select "Fireplace" RadioButtonList Choice
				if (blnFireplace == true)
				{
					rbFireplace.Items[0].Selected = true;
				}
				else
				{
					rbFireplace.Items[1].Selected = true;
				}

				// Retrieve "Woodstove" RadioButtonList
				RadioButtonList rbWoodstove;
				rbWoodstove = (RadioButtonList)e.Item.FindControl("rblWoodstove");
				// Convert "Woodstove" to Boolean
				bool blnWoodstove = Convert.ToBoolean(drv["Woodstove"]);
				// Select "Woodstove" RadioButtonList Choice
				if (blnWoodstove == true)
				{
					rbWoodstove.Items[0].Selected = true;
				}
				else
				{
					rbWoodstove.Items[1].Selected = true;
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

				// Retrieve the ElectricityMonthlyCost TextBox
				TextBox tbElectricityMonthlyCost;
				tbElectricityMonthlyCost = (TextBox)e.Item.FindControl("txtElectricityMonthlyCost");
				// Fill ElectricityMonthlyCost TextBox
				tbElectricityMonthlyCost.Text = ConvertToNumeric(drv["ElectricityMonthlyCost"]);

				// Retrieve the DomWaterProvider TextBox
				TextBox tbDomWaterProvider;
				tbDomWaterProvider = (TextBox)e.Item.FindControl("txtDomWaterProvider");
				// Fill DomWaterProvider TextBox
				tbDomWaterProvider.Text = drv["DomWaterProvider"].ToString();

				// Retrieve the DomWaterMonthlyCost TextBox
				TextBox tbDomWaterMonthlyCost;
				tbDomWaterMonthlyCost = (TextBox)e.Item.FindControl("txtDomWaterMonthlyCost");
				// Fill DomWaterMonthlyCost TextBox
				tbDomWaterMonthlyCost.Text = ConvertToNumeric(drv["DomWaterMonthlyCost"]);

				// Retrieve Sewer RadioButtonList
				RadioButtonList rbSewer;
				rbSewer = (RadioButtonList)e.Item.FindControl("rblSewer");
				// Convert Sewer to String
				string strSewer = drv["Sewer"].ToString();
				// Strip Space Out of Char Returned Value
				strSewer = strSewer.Trim();	
				// Select Sewer RadioButtonList Choice
				rbSewer.Items.FindByText(strSewer).Selected = true;

				// Retrieve KitchenDim TextBox
				TextBox tbKitchenDim;
				tbKitchenDim = (TextBox)e.Item.FindControl("txtKitchenDim");
				// Fill KitchenDim TextBox
				tbKitchenDim.Text = drv["KitchenDim"].ToString();

				// Retrieve LivingRoomDim TextBox
				TextBox tbLivingRoomDim;
				tbLivingRoomDim = (TextBox)e.Item.FindControl("txtLivingRoomDim");
				// Fill LivingRoomDim TextBox
				tbLivingRoomDim.Text = drv["LivingRoomDim"].ToString();

				// Retrieve DiningRoomDim TextBox
				TextBox tbDiningRoomDim;
				tbDiningRoomDim = (TextBox)e.Item.FindControl("txtDiningRoomDim");
				// Fill DiningRoomDim TextBox
				tbDiningRoomDim.Text = drv["DiningRoomDim"].ToString();

				// Retrieve FamilyRoomDim TextBox
				TextBox tbFamilyRoomDim;
				tbFamilyRoomDim = (TextBox)e.Item.FindControl("txtFamilyRoomDim");
				// Fill FamilyRoomDim TextBox
				tbFamilyRoomDim.Text = drv["FamilyRoomDim"].ToString();

				// Retrieve MasterBedDim TextBox
				TextBox tbMasterBedDim;
				tbMasterBedDim = (TextBox)e.Item.FindControl("txtMasterBedDim");
				// Fill MasterBedDim TextBox
				tbMasterBedDim.Text = drv["MasterBedDim"].ToString();

				// Retrieve Bedroom2Dim TextBox
				TextBox tbBedroom2Dim;
				tbBedroom2Dim = (TextBox)e.Item.FindControl("txtBedroom2Dim");
				// Fill Bedroom2Dim TextBox
				tbBedroom2Dim.Text = drv["Bedroom2Dim"].ToString();

				// Retrieve Bedroom3Dim TextBox
				TextBox tbBedroom3Dim;
				tbBedroom3Dim = (TextBox)e.Item.FindControl("txtBedroom3Dim");
				// Fill Bedroom3Dim TextBox
				tbBedroom3Dim.Text = drv["Bedroom3Dim"].ToString();

				// Retrieve Bedroom4Dim TextBox
				TextBox tbBedroom4Dim;
				tbBedroom4Dim = (TextBox)e.Item.FindControl("txtBedroom4Dim");
				// Fill Bedroom4Dim TextBox
				tbBedroom4Dim.Text = drv["Bedroom4Dim"].ToString();

				// Retrieve Bathroom1Dim TextBox
				TextBox tbBathroom1Dim;
				tbBathroom1Dim = (TextBox)e.Item.FindControl("txtBathroom1Dim");
				// Fill Bathroom1Dim TextBox
				tbBathroom1Dim.Text = drv["Bathroom1Dim"].ToString();

				// Retrieve Bathroom2Dim TextBox
				TextBox tbBathroom2Dim;
				tbBathroom2Dim = (TextBox)e.Item.FindControl("txtBathroom2Dim");
				// Fill Bathroom2Dim TextBox
				tbBathroom2Dim.Text = drv["Bathroom2Dim"].ToString();

				// Retrieve Bathroom3Dim TextBox
				TextBox tbBathroom3Dim;
				tbBathroom3Dim = (TextBox)e.Item.FindControl("txtBathroom3Dim");
				// Fill Bathroom3Dim TextBox
				tbBathroom3Dim.Text = drv["Bathroom3Dim"].ToString();

				// Retrieve Bathroom4Dim TextBox
				TextBox tbBathroom4Dim;
				tbBathroom4Dim = (TextBox)e.Item.FindControl("txtBathroom4Dim");
				// Fill Bathroom4Dim TextBox
				tbBathroom4Dim.Text = drv["Bathroom4Dim"].ToString();

				// Retrieve "BasementDim" TextBox
				TextBox tbBasementDim;
				tbBasementDim = (TextBox)e.Item.FindControl("txtBasementDim");
				// Fill "BasementDim" TextBox
				tbBasementDim.Text = drv["BasementDim"].ToString();

				// Retrieve GarageDim TextBox
				TextBox tbGarageDim;
				tbGarageDim = (TextBox)e.Item.FindControl("txtGarageDim");
				// Fill GarageDim TextBox
				tbGarageDim.Text = drv["GarageDim"].ToString();

				// Retrieve PatioDim TextBox
				TextBox tbPatioDim;
				tbPatioDim = (TextBox)e.Item.FindControl("txtPatioDim");
				// Fill PatioDim TextBox
				tbPatioDim.Text = drv["PatioDim"].ToString();

				// Retrieve DeckDim TextBox
				TextBox tbDeckDim;
				tbDeckDim = (TextBox)e.Item.FindControl("txtDeckDim");
				// Fill DeckDim TextBox
				tbDeckDim.Text = drv["DeckDim"].ToString();

				// Retrieve ShedDim TextBox
				TextBox tbShedDim;
				tbShedDim = (TextBox)e.Item.FindControl("txtShedDim");
				// Fill ShedDim TextBox
				tbShedDim.Text = drv["ShedDim"].ToString();

				// Retrieve OfficeDim TextBox
				TextBox tbOfficeDim;
				tbOfficeDim = (TextBox)e.Item.FindControl("txtOfficeDim");
				// Retrieve OfficeDim TextBox
				tbOfficeDim.Text = drv["OfficeDim"].ToString();

				// Retrieve MediaRoomDim TextBox
				TextBox tbMediaRoomDim;
				tbMediaRoomDim = (TextBox)e.Item.FindControl("txtMediaRoomDim");
				// Fill MediaRoomDim TextBox
				tbMediaRoomDim.Text = drv["MediaRoomDim"].ToString();

				// Retrieve LaundryRoomDim TextBox
				TextBox tbLaundryRoomDim;
				tbLaundryRoomDim = (TextBox)e.Item.FindControl("txtLaundryRoomDim");
				// Fill LaundryRoomDim TextBox
				tbLaundryRoomDim.Text = drv["LaundryRoomDim"].ToString();

				// Retrieve SunroomDim TextBox
				TextBox tbSunroomDim;
				tbSunroomDim = (TextBox)e.Item.FindControl("txtSunroomDim");
				// Fill SunroomDim TextBox
				tbSunroomDim.Text = drv["SunroomDim"].ToString();

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

				// Retrieve "InclusionsDescription" TextBox
				TextBox tbInclusionsDescription;
				tbInclusionsDescription = (TextBox)e.Item.FindControl("txtInclusionsDescription");
				// Fill "InclusionsDescription" TextBox
				tbInclusionsDescription.Text = drv["InclusionsDescription"].ToString();

				// Retrieve "ExclusionsDescription" TextBox
				TextBox tbExclusionsDescription;
				tbExclusionsDescription = (TextBox)e.Item.FindControl("txtExclusionsDescription");
				// Fill "ExclusionsDescription" TextBox
				tbExclusionsDescription.Text = drv["ExclusionsDescription"].ToString();

				// Retrieve "OutbuildingsDescription" TextBox
				TextBox tbOutbuildingsDescription;
				tbOutbuildingsDescription = (TextBox)e.Item.FindControl("txtOutbuildingsDescription");
				// Fill "OutbuildingsDescription" TextBox
				tbOutbuildingsDescription.Text = drv["OutbuildingsDescription"].ToString();

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