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
	public class ManageBusinessWithRealEstate : Main
	{
		public Label lblListingDateTime;
		public Label lblLastEditDateTime;
		public HtmlGenericControl hgcErrors;
		public DataList dlBusinessWithRE;

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
			objCommand.CommandText = "sp_select_business_with_re";
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
			sda.Fill(ds, "BusinessWithRE");

			// Fill ListingDateTime Label
			DateTime dtListingDateTime = Convert.ToDateTime(ds.Tables["BusinessWithRE"].Rows[0]["ListingDateTime"]);
			lblListingDateTime.Text = dtListingDateTime.ToString("D");

			// Get Variable Type
			string getType;
			getType = ds.Tables["BusinessWithRE"].Rows[0]["LastEditDateTime"].GetType().ToString();

			// Check to see if LastEditDateTime field is null
			// If not DataBind LastEditDateTime label
			if (getType != "System.DBNull")
			{
				DateTime dtNow = DateTime.Now;

				// Fill LastEditDateTime Label
				DateTime dtLastEdit = Convert.ToDateTime(ds.Tables["BusinessWithRE"].Rows[0]["LastEditDateTime"]);
				lblLastEditDateTime.Text = dtLastEdit.ToString("D");

				if (dtNow.Date == dtLastEdit.Date)
				{
					lblLastEditDateTime.CssClass = "red";
				}
			}

			// DataBind() Residential DataList
			dlBusinessWithRE.DataSource = ds.Tables["BusinessWithRE"].DefaultView;
			dlBusinessWithRE.DataBind();
		}

		public void BusinessWithRE_ItemCreated(Object sender, DataListItemEventArgs e)
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
				string strVirtualPath = Request.ApplicationPath + "/config/business_with_re.xml";

				// Read XML Config File into DataSet
				ds.ReadXml(Request.MapPath(strVirtualPath), XmlReadMode.InferSchema);

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

		public void BusinessWithRE_Edit(Object sender, DataListCommandEventArgs e)
		{
			dlBusinessWithRE.EditItemIndex = e.Item.ItemIndex;
			hgcErrors.Visible = false;
			Bind();
		}

		public void BusinessWithRE_Cancel(Object sender, DataListCommandEventArgs e)
		{
			dlBusinessWithRE.EditItemIndex = -1;
			hgcErrors.Visible = false;
			Bind();
		}

		public void BusinessWithRE_Delete(Object sender, DataListCommandEventArgs e)
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
			SqlCommand objCommand = new SqlCommand("sp_delete_business_with_re", objConnection);
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
				Response.Redirect("browse_business_with_re.aspx", true);
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

		public void BusinessWithRE_Update(Object sender, DataListCommandEventArgs e)
		{
			// Create Connection String
			string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);

			// Create SqlCommand Object
			SqlCommand objCommand = new SqlCommand("sp_update_business_with_re", objConnection);
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

			// Retrieve Office DropDownList
			DropDownList ddOffice;
			ddOffice = (DropDownList)e.Item.FindControl("ddlOfficeID");
			//Add "OfficeID" Parameter
			objParam = objCommand.Parameters.Add("@OfficeID", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(ddOffice.SelectedItem.Value);

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

			// Retrieve "BusinessName" TextBox
			TextBox tbBusinessName;
			tbBusinessName = (TextBox)e.Item.FindControl("txtBusinessName");
			// Add "BusinessName" Parameter
			objParam = objCommand.Parameters.Add("@BusinessName", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbBusinessName.Text);

			// Retrieve "BusinessDescription" TextBox
			TextBox tbBusinessDescription;
			tbBusinessDescription = (TextBox)e.Item.FindControl("txtBusinessDescription");
			// Add "BusinessDescription" Parameter
			objParam = objCommand.Parameters.Add("@BusinessDescription", SqlDbType.VarChar, 500); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbBusinessDescription.Text);

			// Retrieve "BusinessAssetsIncluded" TextBox
			TextBox tbBusinessAssetsIncluded;
			tbBusinessAssetsIncluded = (TextBox)e.Item.FindControl("txtBusinessAssetsIncluded");
			// Add "BusinessAssetsIncluded" Parameter
			objParam = objCommand.Parameters.Add("@BusinessAssetsIncluded", SqlDbType.VarChar, 500); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbBusinessAssetsIncluded.Text);

			// Retrieve "Training" TextBox
			TextBox tbTraining;
			tbTraining = (TextBox)e.Item.FindControl("txtTraining");
			// Add "Training" Parameter
			objParam = objCommand.Parameters.Add("@Training", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbTraining.Text);

			// Retrieve "NonCompeteAgreementYesNo" CheckBox
			CheckBox chbNonCompeteAgreementYesNo;
			chbNonCompeteAgreementYesNo = (CheckBox)e.Item.FindControl("chbNonCompeteAgreementYesNo");
			// Add "NonCompeteAgreementYesNo" Parameter
			objParam = objCommand.Parameters.Add("@NonCompeteAgreementYesNo", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = (SqlBoolean)(chbNonCompeteAgreementYesNo.Checked);

			// Retrieve "NonCompeteAgreementDescription" TextBox
			TextBox tbNonCompeteAgreementDescription;
			tbNonCompeteAgreementDescription = (TextBox)e.Item.FindControl("txtNonCompeteAgreementDescription");
			// Check "NonCompeteAgreementDescription" TextBox
			if (CheckTextBox(tbNonCompeteAgreementDescription))
			{
				//Add "NonCompeteAgreementDescription" Parameter
				objParam = objCommand.Parameters.Add("@NonCompeteAgreementDescription", SqlDbType.VarChar, 250); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbNonCompeteAgreementDescription.Text);
			}

			// Retrieve "Inventory" TextBox
			TextBox tbInventory;
			tbInventory = (TextBox)e.Item.FindControl("txtInventory");
			// Add "Inventory" Parameter
			objParam = objCommand.Parameters.Add("@Inventory", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbInventory.Text);

			// Retrieve "LeaseIncludedYesNo" CheckBox
			CheckBox chbLeaseIncludedYesNo;
			chbLeaseIncludedYesNo = (CheckBox)e.Item.FindControl("chbLeaseIncludedYesNo");
			// Add "LeaseIncludedYesNo" Parameter
			objParam = objCommand.Parameters.Add("@LeaseIncludedYesNo", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = (SqlBoolean)(chbLeaseIncludedYesNo.Checked);

			// Retrieve "LeaseDescription" TextBox
			TextBox tbLeaseDescription;
			tbLeaseDescription = (TextBox)e.Item.FindControl("txtLeaseDescription");
			// Check "LeaseDescription" TextBox
			if (CheckTextBox(tbLeaseDescription))
			{
				//Add "LeaseDescription" Parameter
				objParam = objCommand.Parameters.Add("@LeaseDescription", SqlDbType.VarChar, 500); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbLeaseDescription.Text);
			}

			// Retrieve "BusinessAssetsDLA" CheckBox
			CheckBox chbBusinessAssetsDLA;
			chbBusinessAssetsDLA = (CheckBox)e.Item.FindControl("chbBusinessAssetsDLA");
			// Add "BusinessAssetsDLA" Parameter
			objParam = objCommand.Parameters.Add("@BusinessAssetsDLA", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = (SqlBoolean)(chbBusinessAssetsDLA.Checked);

			// Retrieve "FixturesFurnitureEquipmentDLA" CheckBox
			CheckBox chbFixturesFurnitureEquipmentDLA;
			chbFixturesFurnitureEquipmentDLA = (CheckBox)e.Item.FindControl("chbFixturesFurnitureEquipmentDLA");
			// Add "FixturesFurnitureEquipmentDLA" Parameter
			objParam = objCommand.Parameters.Add("@FixturesFurnitureEquipmentDLA", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = (SqlBoolean)(chbFixturesFurnitureEquipmentDLA.Checked);

			// Retrieve "InventoryDLA" CheckBox
			CheckBox chbInventoryDLA;
			chbInventoryDLA = (CheckBox)e.Item.FindControl("chbInventoryDLA");
			// Add "InventoryDLA" Parameter
			objParam = objCommand.Parameters.Add("@InventoryDLA", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = (SqlBoolean)(chbInventoryDLA.Checked);

			// Retrieve "LicenseInfoDLA" CheckBox
			CheckBox chbLicenseInfoDLA;
			chbLicenseInfoDLA = (CheckBox)e.Item.FindControl("chbLicenseInfoDLA");
			// Add "LicenseInfoDLA" Parameter
			objParam = objCommand.Parameters.Add("@LicenseInfoDLA", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = (SqlBoolean)(chbLicenseInfoDLA.Checked);

			// Retrieve "BusinessAccountsSDA" CheckBox
			CheckBox chbBusinessAccountsSDA;
			chbBusinessAccountsSDA = (CheckBox)e.Item.FindControl("chbBusinessAccountsSDA");
			// Add "BusinessAccountsSDA" Parameter
			objParam = objCommand.Parameters.Add("@BusinessAccountsSDA", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = (SqlBoolean)(chbBusinessAccountsSDA.Checked);

			// Retrieve "ProfitLossInfoSDA" CheckBox
			CheckBox chbProfitLossInfoSDA;
			chbProfitLossInfoSDA = (CheckBox)e.Item.FindControl("chbProfitLossInfoSDA");
			// Add "ProfitLossInfoSDA" Parameter
			objParam = objCommand.Parameters.Add("@ProfitLossInfoSDA", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = (SqlBoolean)(chbProfitLossInfoSDA.Checked);

			// Retrieve "SquareFt" TextBox
			TextBox tbSquareFt;
			tbSquareFt = (TextBox)e.Item.FindControl("txtSquareFt");
			//Add "SquareFt" Parameter
			objParam = objCommand.Parameters.Add("@SquareFt", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(tbSquareFt.Text);

			// Retrieve "ParcelSize" TextBox
			TextBox tbParcelSize;
			tbParcelSize = (TextBox)e.Item.FindControl("txtParcelSize");
			// Add "ParcelSize" Parameter
			objParam = objCommand.Parameters.Add("@ParcelSize", SqlDbType.VarChar, 30); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbParcelSize.Text);

			// Retrieve "RealEstateDescription" TextBox
			TextBox tbRealEstateDescription;
			tbRealEstateDescription = (TextBox)e.Item.FindControl("txtRealEstateDescription");
			// Add "RealEstateDescription" Parameter
			objParam = objCommand.Parameters.Add("@RealEstateDescription", SqlDbType.VarChar, 500); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbRealEstateDescription.Text);

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

			dlBusinessWithRE.EditItemIndex = -1;

			// Set MLS Cookie
			Session["mlsID"] = Convert.ToInt32(tbMLS.Text);

			Bind();
		}

		public void BusinessWithRE_DataBound(Object sender, DataListItemEventArgs e) 
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

				// Retrieve "BusinessName" TableCell
				TableCell tcBusinessName;
				tcBusinessName = (TableCell)e.Item.FindControl("tcBusinessName");
				// DataBind "BusinessName" TableCell
				tcBusinessName.Text = drv["BusinessName"].ToString();

				// Retrieve "BusinessDescription" TableCell
				TableCell tcBusinessDescription;
				tcBusinessDescription = (TableCell)e.Item.FindControl("tcBusinessDescription");
				// DataBind "BusinessDescription" TableCell
				tcBusinessDescription.Text = drv["BusinessDescription"].ToString();

				// Retrieve "BusinessAssetsIncluded" TableCell
				TableCell tcBusinessAssetsIncluded;
				tcBusinessAssetsIncluded = (TableCell)e.Item.FindControl("tcBusinessAssetsIncluded");
				// DataBind "BusinessAssetsIncluded" TableCell
				tcBusinessAssetsIncluded.Text = drv["BusinessAssetsIncluded"].ToString();

				// Retrieve "Training" TableCell
				TableCell tcTraining;
				tcTraining = (TableCell)e.Item.FindControl("tcTraining");
				// DataBind "Training" TableCell
				tcTraining.Text = drv["Training"].ToString();

				// Retrieve "NonCompeteAgreementYesNo" CheckBox
				CheckBox chbNonCompeteAgreementYesNo;
				chbNonCompeteAgreementYesNo = (CheckBox)e.Item.FindControl("chbNonCompeteAgreementYesNo");
				// DataBind "NonCompeteAgreementYesNo" CheckBox
				chbNonCompeteAgreementYesNo.Checked = Convert.ToBoolean(drv["NonCompeteAgreementYesNo"]);

				// Retrieve "NonCompeteAgreementDescription" TableCell
				TableCell tcNonCompeteAgreementDescription;
				tcNonCompeteAgreementDescription = (TableCell)e.Item.FindControl("tcNonCompeteAgreementDescription");
				// DataBind "NonCompeteAgreementDescription" TableCell
				tcNonCompeteAgreementDescription.Text = ConvertToString(drv["NonCompeteAgreementDescription"]);

				// Retrieve "Inventory" TableCell
				TableCell tcInventory;
				tcInventory = (TableCell)e.Item.FindControl("tcInventory");
				// DataBind "Inventory" TableCell
				tcInventory.Text = drv["Inventory"].ToString();

				// Retrieve "LeaseIncludedYesNo" CheckBox
				CheckBox chbLeaseIncludedYesNo;
				chbLeaseIncludedYesNo = (CheckBox)e.Item.FindControl("chbLeaseIncludedYesNo");
				// DataBind "LeaseIncludedYesNo" CheckBox
				chbLeaseIncludedYesNo.Checked = Convert.ToBoolean(drv["LeaseIncludedYesNo"]);

				// Retrieve "LeaseDescription" TableCell
				TableCell tcLeaseDescription;
				tcLeaseDescription = (TableCell)e.Item.FindControl("tcLeaseDescription");
				// DataBind "LeaseDescription" TableCell
				tcLeaseDescription.Text = ConvertToString(drv["LeaseDescription"]);

				// Retrieve "BusinessAssetsDLA" CheckBox
				CheckBox chbBusinessAssetsDLA;
				chbBusinessAssetsDLA = (CheckBox)e.Item.FindControl("chbBusinessAssetsDLA");
				// DataBind "BusinessAssetsDLA" CheckBox
				chbBusinessAssetsDLA.Checked = Convert.ToBoolean(drv["BusinessAssetsDLA"]);

				// Retrieve "FixturesFurnitureEquipmentDLA" CheckBox
				CheckBox chbFixturesFurnitureEquipmentDLA;
				chbFixturesFurnitureEquipmentDLA = (CheckBox)e.Item.FindControl("chbFixturesFurnitureEquipmentDLA");
				// DataBind "FixturesFurnitureEquipmentDLA" CheckBox
				chbFixturesFurnitureEquipmentDLA.Checked = Convert.ToBoolean(drv["FixturesFurnitureEquipmentDLA"]);

				// Retrieve "InventoryDLA" CheckBox
				CheckBox chbInventoryDLA;
				chbInventoryDLA = (CheckBox)e.Item.FindControl("chbInventoryDLA");
				// DataBind "InventoryDLA" CheckBox
				chbInventoryDLA.Checked = Convert.ToBoolean(drv["InventoryDLA"]);

				// Retrieve "LicenseInfoDLA" CheckBox
				CheckBox chbLicenseInfoDLA;
				chbLicenseInfoDLA = (CheckBox)e.Item.FindControl("chbLicenseInfoDLA");
				// DataBind "LicenseInfoDLA" CheckBox
				chbLicenseInfoDLA.Checked = Convert.ToBoolean(drv["LicenseInfoDLA"]);

				// Retrieve "BusinessAccountsSDA" CheckBox
				CheckBox chbBusinessAccountsSDA;
				chbBusinessAccountsSDA = (CheckBox)e.Item.FindControl("chbBusinessAccountsSDA");
				// DataBind "BusinessAccountsSDA" CheckBox
				chbBusinessAccountsSDA.Checked = Convert.ToBoolean(drv["BusinessAccountsSDA"]);

				// Retrieve "ProfitLossInfoSDA" CheckBox
				CheckBox chbProfitLossInfoSDA;
				chbProfitLossInfoSDA = (CheckBox)e.Item.FindControl("chbProfitLossInfoSDA");
				// DataBind "ProfitLossInfoSDA" CheckBox
				chbProfitLossInfoSDA.Checked = Convert.ToBoolean(drv["ProfitLossInfoSDA"]);

				// Retrieve "SquareFt" TableCell
				TableCell tcSquareFt;
				tcSquareFt = (TableCell)e.Item.FindControl("tcSquareFt");
				// DataBind "SquareFt" TableCell
				tcSquareFt.Text = ConvertToStringPlusMinus(drv["SquareFt"]);

				// Retrieve "ParcelSize" TableCell
				TableCell tcParcelSize;
				tcParcelSize = (TableCell)e.Item.FindControl("tcParcelSize");
				// DataBind "ParcelSize" TableCell
				tcParcelSize.Text = drv["ParcelSize"].ToString();

				// Retrieve "RealEstateDescription" TableCell
				TableCell tcRealEstateDescription;
				tcRealEstateDescription = (TableCell)e.Item.FindControl("tcRealEstateDescription");
				// DataBind "RealEstateDescription" TableCell
				tcRealEstateDescription.Text = drv["RealEstateDescription"].ToString();

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

				// Retrieve "BusinessName" TextBox
				TextBox tbBusinessName;
				tbBusinessName = (TextBox)e.Item.FindControl("txtBusinessName");
				// Fill "BusinessName" TextBox
				tbBusinessName.Text = drv["BusinessName"].ToString();

				// Retrieve "BusinessDescription" TextBox
				TextBox tbBusinessDescription;
				tbBusinessDescription = (TextBox)e.Item.FindControl("txtBusinessDescription");
				// Fill "BusinessDescription" TextBox
				tbBusinessDescription.Text = drv["BusinessDescription"].ToString();

				// Retrieve "BusinessAssetsIncluded" TextBox
				TextBox tbBusinessAssetsIncluded;
				tbBusinessAssetsIncluded = (TextBox)e.Item.FindControl("txtBusinessAssetsIncluded");
				// Fill "BusinessAssetsIncluded" TextBox
				tbBusinessAssetsIncluded.Text = drv["BusinessAssetsIncluded"].ToString();

				// Retrieve "Training" TextBox
				TextBox tbTraining;
				tbTraining = (TextBox)e.Item.FindControl("txtTraining");
				// Fill "Training" TextBox
				tbTraining.Text = drv["Training"].ToString();

				// Retrieve "NonCompeteAgreementYesNo" CheckBox
				CheckBox cbNonCompeteAgreementYesNo;
				cbNonCompeteAgreementYesNo = (CheckBox)e.Item.FindControl("chbNonCompeteAgreementYesNo");
				// Fill "NonCompeteAgreementYesNo" CheckBox
				cbNonCompeteAgreementYesNo.Checked = Convert.ToBoolean(drv["NonCompeteAgreementYesNo"]);

				// Retrieve "NonCompeteAgreementDescription" TextBox
				TextBox tbNonCompeteAgreementDescription;
				tbNonCompeteAgreementDescription = (TextBox)e.Item.FindControl("txtNonCompeteAgreementDescription");
				// Fill "NonCompeteAgreementDescription" TextBox
				tbNonCompeteAgreementDescription.Text = drv["NonCompeteAgreementDescription"].ToString();

				// Retrieve "Inventory" TextBox
				TextBox tbInventory;
				tbInventory = (TextBox)e.Item.FindControl("txtInventory");
				// Fill "Inventory" TextBox
				tbInventory.Text = drv["Inventory"].ToString();

				// Retrieve "LeaseIncludedYesNo" CheckBox
				CheckBox cbLeaseIncludedYesNo;
				cbLeaseIncludedYesNo = (CheckBox)e.Item.FindControl("chbLeaseIncludedYesNo");
				// Fill "LeaseIncludedYesNo" CheckBox
				cbLeaseIncludedYesNo.Checked = Convert.ToBoolean(drv["LeaseIncludedYesNo"]);

				// Retrieve "LeaseDescription" TextBox
				TextBox tbLeaseDescription;
				tbLeaseDescription = (TextBox)e.Item.FindControl("txtLeaseDescription");
				// Fill "LeaseDescription" TextBox
				tbLeaseDescription.Text = drv["LeaseDescription"].ToString();

				// Retrieve "BusinessAssetsDLA" CheckBox
				CheckBox cbBusinessAssetsDLA;
				cbBusinessAssetsDLA = (CheckBox)e.Item.FindControl("chbBusinessAssetsDLA");
				// Fill "BusinessAssetsDLA" CheckBox
				cbBusinessAssetsDLA.Checked = Convert.ToBoolean(drv["BusinessAssetsDLA"]);

				// Retrieve "FixturesFurnitureEquipmentDLA" CheckBox
				CheckBox cbFixturesFurnitureEquipmentDLA;
				cbFixturesFurnitureEquipmentDLA = (CheckBox)e.Item.FindControl("chbFixturesFurnitureEquipmentDLA");
				// Fill "FixturesFurnitureEquipmentDLA" CheckBox
				cbFixturesFurnitureEquipmentDLA.Checked = Convert.ToBoolean(drv["FixturesFurnitureEquipmentDLA"]);

				// Retrieve "InventoryDLA" CheckBox
				CheckBox cbInventoryDLA;
				cbInventoryDLA = (CheckBox)e.Item.FindControl("chbInventoryDLA");
				// Fill "InventoryDLA" CheckBox
				cbInventoryDLA.Checked = Convert.ToBoolean(drv["InventoryDLA"]);

				// Retrieve "LicenseInfoDLA" CheckBox
				CheckBox cbLicenseInfoDLA;
				cbLicenseInfoDLA = (CheckBox)e.Item.FindControl("chbLicenseInfoDLA");
				// Fill "LicenseInfoDLA" CheckBox
				cbLicenseInfoDLA.Checked = Convert.ToBoolean(drv["LicenseInfoDLA"]);

				// Retrieve "BusinessAccountsSDA" CheckBox
				CheckBox cbBusinessAccountsSDA;
				cbBusinessAccountsSDA = (CheckBox)e.Item.FindControl("chbBusinessAccountsSDA");
				// Fill "BusinessAccountsSDA" CheckBox
				cbBusinessAccountsSDA.Checked = Convert.ToBoolean(drv["BusinessAccountsSDA"]);

				// Retrieve "ProfitLossInfoSDA" CheckBox
				CheckBox cbProfitLossInfoSDA;
				cbProfitLossInfoSDA = (CheckBox)e.Item.FindControl("chbProfitLossInfoSDA");
				// Fill "ProfitLossInfoSDA" CheckBox
				cbProfitLossInfoSDA.Checked = Convert.ToBoolean(drv["ProfitLossInfoSDA"]);

				// Retrieve "SquareFt" TextBox
				TextBox tbSquareFt;
				tbSquareFt = (TextBox)e.Item.FindControl("txtSquareFt");
				// Fill "SquareFt" TextBox
				tbSquareFt.Text = drv["SquareFt"].ToString();

				// Retrieve "ParcelSize" TextBox
				TextBox tbParcelSize;
				tbParcelSize = (TextBox)e.Item.FindControl("txtParcelSize");
				// Fill "ParcelSize" TextBox
				tbParcelSize.Text = drv["ParcelSize"].ToString();

				// Retrieve "RealEstateDescription" TextBox
				TextBox tbRealEstateDescription;
				tbRealEstateDescription = (TextBox)e.Item.FindControl("txtRealEstateDescription");
				// Fill "RealEstateDescription" TextBox
				tbRealEstateDescription.Text = drv["RealEstateDescription"].ToString();

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