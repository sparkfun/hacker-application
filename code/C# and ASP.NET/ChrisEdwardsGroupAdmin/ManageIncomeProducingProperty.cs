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
	/// Summary description for ManageIncomeProducingProperty.
	/// </summary>
	public class ManageIncomeProducingProperty : Main
	{
		public Label lblListingDateTime;
		public Label lblLastEditDateTime;
		public HtmlGenericControl hgcErrors;
		public DataList dlIncomeProducingProp;

		public ManageIncomeProducingProperty()
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
			objCommand.CommandText = "sp_select_income_producing_prop";
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
			sda.Fill(ds, "IncomeProducingProp");

			// Fill ListingDateTime Label
			DateTime dtListingDateTime = Convert.ToDateTime(ds.Tables["IncomeProducingProp"].Rows[0]["ListingDateTime"]);
			lblListingDateTime.Text = dtListingDateTime.ToString("D");

			// Get Variable Type
			string getType;
			getType = ds.Tables["IncomeProducingProp"].Rows[0]["LastEditDateTime"].GetType().ToString();

			// Check to see if LastEditDateTime field is null
			// If not DataBind LastEditDateTime label
			if (getType != "System.DBNull")
			{
				DateTime dtNow = DateTime.Now;

				// Fill LastEditDateTime Label
				DateTime dtLastEdit = Convert.ToDateTime(ds.Tables["IncomeProducingProp"].Rows[0]["LastEditDateTime"]);
				lblLastEditDateTime.Text = dtLastEdit.ToString("D");

				if (dtNow.Date == dtLastEdit.Date)
				{
					lblLastEditDateTime.CssClass = "red";
				}
			}

			// DataBind() Income Producing Property DataList
			dlIncomeProducingProp.DataSource = ds.Tables["IncomeProducingProp"].DefaultView;
			dlIncomeProducingProp.DataBind();
		}

		public void IncomeProducingProp_ItemCreated(Object sender, DataListItemEventArgs e)
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
				string strVirtualPath = Request.ApplicationPath + "/config/income_producing_prop.xml";

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

		public void IncomeProducingProp_Edit(Object sender, DataListCommandEventArgs e)
		{
			dlIncomeProducingProp.EditItemIndex = e.Item.ItemIndex;
			hgcErrors.Visible = false;
			Bind();
		}

		public void IncomeProducingProp_Cancel(Object sender, DataListCommandEventArgs e)
		{
			dlIncomeProducingProp.EditItemIndex = -1;
			hgcErrors.Visible = false;
			Bind();
		}

		public void IncomeProducingProp_Delete(Object sender, DataListCommandEventArgs e)
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
			SqlCommand objCommand = new SqlCommand("sp_delete_income_producing_prop", objConnection);
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
				Response.Redirect("browse_income_producing_prop.aspx", true);
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

		public void IncomeProducingProp_Update(Object sender, DataListCommandEventArgs e)
		{
			// Create Connection String
			string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);

			// Create SqlCommand Object
			SqlCommand objCommand = new SqlCommand("sp_update_income_producing_prop", objConnection);
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

			// Retrieve "PropertyType" TextBox
			TextBox tbPropertyType;
			tbPropertyType = (TextBox)e.Item.FindControl("txtPropertyType");
			// Add "PropertyType" Parameter
			objParam = objCommand.Parameters.Add("@PropertyType", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbPropertyType.Text);

			// Retrieve "RealEstateDescription" TextBox
			TextBox tbRealEstateDescription;
			tbRealEstateDescription = (TextBox)e.Item.FindControl("txtRealEstateDescription");
			// Add "RealEstateDescription" Parameter
			objParam = objCommand.Parameters.Add("@RealEstateDescription", SqlDbType.VarChar, 500); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbRealEstateDescription.Text);

			// Retrieve "ImprovementDescription" TextBox
			TextBox tbImprovementDescription;
			tbImprovementDescription = (TextBox)e.Item.FindControl("txtImprovementDescription");
			if (CheckTextBox(tbImprovementDescription))
			{
				// Add "ImprovementDescription" Parameter
				objParam = objCommand.Parameters.Add("@ImprovementDescription", SqlDbType.VarChar, 500); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbImprovementDescription.Text);
			}

			// Retrieve "PersonalPropertyIncluded" TextBox
			TextBox tbPersonalPropertyIncluded;
			tbPersonalPropertyIncluded = (TextBox)e.Item.FindControl("txtPersonalPropertyIncluded");
			if (CheckTextBox(tbPersonalPropertyIncluded))
			{
				// Add "PersonalPropertyIncluded" Parameter
				objParam = objCommand.Parameters.Add("@PersonalPropertyIncluded", SqlDbType.VarChar, 250); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbPersonalPropertyIncluded.Text);
			}

			// Retrieve "ScheduleOfRent" TextBox
			TextBox tbScheduleOfRent;
			tbScheduleOfRent = (TextBox)e.Item.FindControl("txtScheduleOfRent");
			// Add "ScheduleOfRent" Parameter
			objParam = objCommand.Parameters.Add("@ScheduleOfRent", SqlDbType.VarChar, 500); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbScheduleOfRent.Text);

			// Retrieve "ManagementAvailableYesNo" CheckBox
			CheckBox chbManagementAvailableYesNo;
			chbManagementAvailableYesNo = (CheckBox)e.Item.FindControl("chbManagementAvailableYesNo");
			// Add "ManagementAvailableYesNo" Parameter
			objParam = objCommand.Parameters.Add("@ManagementAvailableYesNo", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = (SqlBoolean)(chbManagementAvailableYesNo.Checked);

			// Retrieve "ManagementDescription" TextBox
			TextBox tbManagementDescription;
			tbManagementDescription = (TextBox)e.Item.FindControl("txtManagementDescription");
			if (CheckTextBox(tbManagementDescription))
			{
				// Add "ManagementDescription" Parameter
				objParam = objCommand.Parameters.Add("@ManagementDescription", SqlDbType.VarChar, 500); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(tbManagementDescription.Text);
			}

			// Retrieve "DomWaterInfo" TextBox
			TextBox tbDomWaterInfo;
			tbDomWaterInfo = (TextBox)e.Item.FindControl("txtDomWaterInfo");
			// Add "DomWaterInfo" Parameter
			objParam = objCommand.Parameters.Add("@DomWaterInfo", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbDomWaterInfo.Text);

			// Retrieve "IrrWaterInfo" TextBox
			TextBox tbIrrWaterInfo;
			tbIrrWaterInfo = (TextBox)e.Item.FindControl("txtIrrWaterInfo");
			// Add "IrrWaterInfo" Parameter
			objParam = objCommand.Parameters.Add("@IrrWaterInfo", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbIrrWaterInfo.Text);

			// Retrieve "UtilityInfo" TextBox
			TextBox tbUtilityInfo;
			tbUtilityInfo = (TextBox)e.Item.FindControl("txtUtilityInfo");
			// Add "UtilityInfo" Parameter
			objParam = objCommand.Parameters.Add("@UtilityInfo", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbUtilityInfo.Text);

			// Retrieve "SewerSepticInfo" TextBox
			TextBox tbSewerSepticInfo;
			tbSewerSepticInfo = (TextBox)e.Item.FindControl("txtSewerSepticInfo");
			// Add "SewerSepticInfo" Parameter
			objParam = objCommand.Parameters.Add("@SewerSepticInfo", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbSewerSepticInfo.Text);

			// Retrieve "GrossScheduledIncomeActual" TextBox
			TextBox tbGrossScheduledIncomeActual;
			tbGrossScheduledIncomeActual = (TextBox)e.Item.FindControl("txtGrossScheduledIncomeActual");
			// Check "GrossScheduledIncomeActual" TextBox
			if (CheckTextBox(tbGrossScheduledIncomeActual))
			{
				// Add "GrossScheduledIncomeActual" Parameter
				objParam = objCommand.Parameters.Add("@GrossScheduledIncomeActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbGrossScheduledIncomeActual.Text);
			}

			// Retrieve "GrossScheduledIncomeProj" TextBox
			TextBox tbGrossScheduledIncomeProj;
			tbGrossScheduledIncomeProj = (TextBox)e.Item.FindControl("txtGrossScheduledIncomeProj");
			// Check "GrossScheduledIncomeProj" TextBox
			if (CheckTextBox(tbGrossScheduledIncomeProj))
			{
				// Add "GrossScheduledIncomeProj" Parameter
				objParam = objCommand.Parameters.Add("@GrossScheduledIncomeProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbGrossScheduledIncomeProj.Text);
			}

			// Retrieve "PlusOtherIncomeActual" TextBox
			TextBox tbPlusOtherIncomeActual;
			tbPlusOtherIncomeActual = (TextBox)e.Item.FindControl("txtPlusOtherIncomeActual");
			// Check "PlusOtherIncomeActual" TextBox
			if (CheckTextBox(tbPlusOtherIncomeActual))
			{
				// Add "PlusOtherIncomeActual" Parameter
				objParam = objCommand.Parameters.Add("@PlusOtherIncomeActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbPlusOtherIncomeActual.Text);
			}

			// Retrieve "PlusOtherIncomeProj" TextBox
			TextBox tbPlusOtherIncomeProj;
			tbPlusOtherIncomeProj = (TextBox)e.Item.FindControl("txtPlusOtherIncomeProj");
			// Check "PlusOtherIncomeProj" TextBox
			if (CheckTextBox(tbPlusOtherIncomeProj))
			{
				// Add "PlusOtherIncomeProj" Parameter
				objParam = objCommand.Parameters.Add("@PlusOtherIncomeProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbPlusOtherIncomeProj.Text);
			}

			// Retrieve "VacancyCreditLossActual" TextBox
			TextBox tbVacancyCreditLossActual;
			tbVacancyCreditLossActual = (TextBox)e.Item.FindControl("txtVacancyCreditLossActual");
			// Check "VacancyCreditLossActual" TextBox
			if (CheckTextBox(tbVacancyCreditLossActual))
			{
				// Add "VacancyCreditLossActual" Parameter
				objParam = objCommand.Parameters.Add("@VacancyCreditLossActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbVacancyCreditLossActual.Text);
			}

			// Retrieve "VacancyCreditLossProj" TextBox
			TextBox tbVacancyCreditLossProj;
			tbVacancyCreditLossProj = (TextBox)e.Item.FindControl("txtVacancyCreditLossProj");
			// Check "VacancyCreditLossProj" TextBox
			if (CheckTextBox(tbVacancyCreditLossProj))
			{
				// Add "VacancyCreditLossProj" Parameter
				objParam = objCommand.Parameters.Add("@VacancyCreditLossProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbVacancyCreditLossProj.Text);
			}

			// Retrieve "OpExpensesAdvertisingActual" TextBox
			TextBox tbOpExpensesAdvertisingActual;
			tbOpExpensesAdvertisingActual = (TextBox)e.Item.FindControl("txtOpExpensesAdvertisingActual");
			// Check "OpExpensesAdvertisingActual" TextBox
			if (CheckTextBox(tbOpExpensesAdvertisingActual))
			{
				// Add "OpExpensesAdvertisingActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesAdvertisingActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesAdvertisingActual.Text);
			}

			// Retrieve "OpExpensesAdvertisingProj" TextBox
			TextBox tbOpExpensesAdvertisingProj;
			tbOpExpensesAdvertisingProj = (TextBox)e.Item.FindControl("txtOpExpensesAdvertisingProj");
			// Check "OpExpensesAdvertisingProj" TextBox
			if (CheckTextBox(tbOpExpensesAdvertisingProj))
			{
				// Add "OpExpensesAdvertisingProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesAdvertisingProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesAdvertisingProj.Text);
			}

			// Retrieve "OpExpensesCleanMaintActual" TextBox
			TextBox tbOpExpensesCleanMaintActual;
			tbOpExpensesCleanMaintActual = (TextBox)e.Item.FindControl("txtOpExpensesCleanMaintActual");
			// Check "OpExpensesCleanMaintActual" TextBox
			if (CheckTextBox(tbOpExpensesCleanMaintActual))
			{
				// Add "OpExpensesCleanMaintActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesCleanMaintActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesCleanMaintActual.Text);
			}

			// Retrieve "OpExpensesCleanMaintProj" TextBox
			TextBox tbOpExpensesCleanMaintProj;
			tbOpExpensesCleanMaintProj = (TextBox)e.Item.FindControl("txtOpExpensesCleanMaintProj");
			// Check "OpExpensesCleanMaintProj" TextBox
			if (CheckTextBox(tbOpExpensesCleanMaintProj))
			{
				// Add "OpExpensesCleanMaintProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesCleanMaintProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesCleanMaintProj.Text);
			}

			// Retrieve "OpExpensesInsuranceActual" TextBox
			TextBox tbOpExpensesInsuranceActual;
			tbOpExpensesInsuranceActual = (TextBox)e.Item.FindControl("txtOpExpensesInsuranceActual");
			// Check "OpExpensesInsuranceActual" TextBox
			if (CheckTextBox(tbOpExpensesInsuranceActual))
			{
				// Add "OpExpensesInsuranceActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesInsuranceActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesInsuranceActual.Text);
			}

			// Retrieve "OpExpensesInsuranceProj" TextBox
			TextBox tbOpExpensesInsuranceProj;
			tbOpExpensesInsuranceProj = (TextBox)e.Item.FindControl("txtOpExpensesInsuranceProj");
			// Check "OpExpensesInsuranceProj" TextBox
			if (CheckTextBox(tbOpExpensesInsuranceProj))
			{
				// Add "OpExpensesInsuranceProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesInsuranceProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesInsuranceProj.Text);
			}

			// Retrieve "OpExpensesLegProfActual" TextBox
			TextBox tbOpExpensesLegProfActual;
			tbOpExpensesLegProfActual = (TextBox)e.Item.FindControl("txtOpExpensesLegProfActual");
			// Check "OpExpensesLegProfActual" TextBox
			if (CheckTextBox(tbOpExpensesLegProfActual))
			{
				// Add "OpExpensesLegProfActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesLegProfActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesLegProfActual.Text);
			}

			// Retrieve "OpExpensesLegProfProj" TextBox
			TextBox tbOpExpensesLegProfProj;
			tbOpExpensesLegProfProj = (TextBox)e.Item.FindControl("txtOpExpensesLegProfProj");
			// Check "OpExpensesLegProfProj" TextBox
			if (CheckTextBox(tbOpExpensesLegProfProj))
			{
				// Add "OpExpensesLegProfProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesLegProfProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesLegProfProj.Text);
			}

			// Retrieve "OpExpensesMiscRefActual" TextBox
			TextBox tbOpExpensesMiscRefActual;
			tbOpExpensesMiscRefActual = (TextBox)e.Item.FindControl("txtOpExpensesMiscRefActual");
			// Check "OpExpensesMiscRefActual" TextBox
			if (CheckTextBox(tbOpExpensesMiscRefActual))
			{
				// Add "OpExpensesMiscRefActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesMiscRefActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesMiscRefActual.Text);
			}

			// Retrieve "OpExpensesMiscRefProj" TextBox
			TextBox tbOpExpensesMiscRefProj;
			tbOpExpensesMiscRefProj = (TextBox)e.Item.FindControl("txtOpExpensesMiscRefProj");
			// Check "OpExpensesMiscRefProj" TextBox
			if (CheckTextBox(tbOpExpensesMiscRefProj))
			{
				// Add "OpExpensesMiscRefProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesMiscRefProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesMiscRefProj.Text);
			}

			// Retrieve "OpExpensesSuppliesActual" TextBox
			TextBox tbOpExpensesSuppliesActual;
			tbOpExpensesSuppliesActual = (TextBox)e.Item.FindControl("txtOpExpensesSuppliesActual");
			// Check "OpExpensesSuppliesActual" TextBox
			if (CheckTextBox(tbOpExpensesSuppliesActual))
			{
				// Add "OpExpensesSuppliesActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesSuppliesActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesSuppliesActual.Text);
			}

			// Retrieve "OpExpensesSuppliesProj" TextBox
			TextBox tbOpExpensesSuppliesProj;
			tbOpExpensesSuppliesProj = (TextBox)e.Item.FindControl("txtOpExpensesSuppliesProj");
			// Check "OpExpensesSuppliesProj" TextBox
			if (CheckTextBox(tbOpExpensesSuppliesProj))
			{
				// Add "OpExpensesSuppliesProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesSuppliesProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesSuppliesProj.Text);
			}

			// Retrieve "OpExpensesPropManActual" TextBox
			TextBox tbOpExpensesPropManActual;
			tbOpExpensesPropManActual = (TextBox)e.Item.FindControl("txtOpExpensesPropManActual");
			// Check "OpExpensesPropManActual" TextBox
			if (CheckTextBox(tbOpExpensesPropManActual))
			{
				// Add "OpExpensesPropManActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesPropManActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesPropManActual.Text);
			}

			// Retrieve "OpExpensesPropManProj" TextBox
			TextBox tbOpExpensesPropManProj;
			tbOpExpensesPropManProj = (TextBox)e.Item.FindControl("txtOpExpensesPropManProj");
			// Check "OpExpensesPropManProj" TextBox
			if (CheckTextBox(tbOpExpensesPropManProj))
			{
				// Add "OpExpensesPropManProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesPropManProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesPropManProj.Text);
			}

			// Retrieve "OpExpensesRepairsActual" TextBox
			TextBox tbOpExpensesRepairsActual;
			tbOpExpensesRepairsActual = (TextBox)e.Item.FindControl("txtOpExpensesRepairsActual");
			// Check "OpExpensesRepairsActual" TextBox
			if (CheckTextBox(tbOpExpensesRepairsActual))
			{
				// Add "OpExpensesRepairsActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesRepairsActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesRepairsActual.Text);
			}

			// Retrieve "OpExpensesRepairsProj" TextBox
			TextBox tbOpExpensesRepairsProj;
			tbOpExpensesRepairsProj = (TextBox)e.Item.FindControl("txtOpExpensesRepairsProj");
			// Check "OpExpensesRepairsProj" TextBox
			if (CheckTextBox(tbOpExpensesRepairsProj))
			{
				// Add "OpExpensesRepairsProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesRepairsProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesRepairsProj.Text);
			}

			// Retrieve "OpExpensesTaxesActual" TextBox
			TextBox tbOpExpensesTaxesActual;
			tbOpExpensesTaxesActual = (TextBox)e.Item.FindControl("txtOpExpensesTaxesActual");
			// Check "OpExpensesTaxesActual" TextBox
			if (CheckTextBox(tbOpExpensesTaxesActual))
			{
				// Add "OpExpensesTaxesActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesTaxesActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesTaxesActual.Text);
			}

			// Retrieve "OpExpensesTaxesProj" TextBox
			TextBox tbOpExpensesTaxesProj;
			tbOpExpensesTaxesProj = (TextBox)e.Item.FindControl("txtOpExpensesTaxesProj");
			// Check "OpExpensesTaxesProj" TextBox
			if (CheckTextBox(tbOpExpensesTaxesProj))
			{
				// Add "OpExpensesTaxesProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesTaxesProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesTaxesProj.Text);
			}

			// Retrieve "OpExpensesTelCableActual" TextBox
			TextBox tbOpExpensesTelCableActual;
			tbOpExpensesTelCableActual = (TextBox)e.Item.FindControl("txtOpExpensesTelCableActual");
			// Check "OpExpensesTelCableActual" TextBox
			if (CheckTextBox(tbOpExpensesTelCableActual))
			{
				// Add "OpExpensesTelCableActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesTelCableActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesTelCableActual.Text);
			}

			// Retrieve "OpExpensesTelCableProj" TextBox
			TextBox tbOpExpensesTelCableProj;
			tbOpExpensesTelCableProj = (TextBox)e.Item.FindControl("txtOpExpensesTelCableProj");
			// Check "OpExpensesTelCableProj" TextBox
			if (CheckTextBox(tbOpExpensesTelCableProj))
			{
				// Add "OpExpensesTelCableProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesTelCableProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesTelCableProj.Text);
			}

			// Retrieve "OpExpensesTrashRemActual" TextBox
			TextBox tbOpExpensesTrashRemActual;
			tbOpExpensesTrashRemActual = (TextBox)e.Item.FindControl("txtOpExpensesTrashRemActual");
			// Check "OpExpensesTrashRemActual" TextBox
			if (CheckTextBox(tbOpExpensesTrashRemActual))
			{
				// Add "OpExpensesTrashRemActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesTrashRemActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesTrashRemActual.Text);
			}

			// Retrieve "OpExpensesTrashRemProj" TextBox
			TextBox tbOpExpensesTrashRemProj;
			tbOpExpensesTrashRemProj = (TextBox)e.Item.FindControl("txtOpExpensesTrashRemProj");
			// Check "OpExpensesTrashRemProj" TextBox
			if (CheckTextBox(tbOpExpensesTrashRemProj))
			{
				// Add "OpExpensesTrashRemProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesTrashRemProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesTrashRemProj.Text);
			}

			// Retrieve "OpExpensesUtilGasActual" TextBox
			TextBox tbOpExpensesUtilGasActual;
			tbOpExpensesUtilGasActual = (TextBox)e.Item.FindControl("txtOpExpensesUtilGasActual");
			// Check "OpExpensesUtilGasActual" TextBox
			if (CheckTextBox(tbOpExpensesUtilGasActual))
			{
				// Add "OpExpensesUtilGasActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesUtilGasActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesUtilGasActual.Text);
			}

			// Retrieve "OpExpensesUtilGasProj" TextBox
			TextBox tbOpExpensesUtilGasProj;
			tbOpExpensesUtilGasProj = (TextBox)e.Item.FindControl("txtOpExpensesUtilGasProj");
			// Check "OpExpensesUtilGasProj" TextBox
			if (CheckTextBox(tbOpExpensesUtilGasProj))
			{
				// Add "OpExpensesUtilGasProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesUtilGasProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesUtilGasProj.Text);
			}

			// Retrieve "OpExpensesUtilElecActual" TextBox
			TextBox tbOpExpensesUtilElecActual;
			tbOpExpensesUtilElecActual = (TextBox)e.Item.FindControl("txtOpExpensesUtilElecActual");
			// Check "OpExpensesUtilElecActual" TextBox
			if (CheckTextBox(tbOpExpensesUtilElecActual))
			{
				// Add "OpExpensesUtilElecActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesUtilElecActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesUtilElecActual.Text);
			}

			// Retrieve "OpExpensesUtilElecProj" TextBox
			TextBox tbOpExpensesUtilElecProj;
			tbOpExpensesUtilElecProj = (TextBox)e.Item.FindControl("txtOpExpensesUtilElecProj");
			// Check "OpExpensesUtilElecProj" TextBox
			if (CheckTextBox(tbOpExpensesUtilElecProj))
			{
				// Add "OpExpensesUtilElecProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesUtilElecProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesUtilElecProj.Text);
			}

			// Retrieve "OpExpensesUtilSewerActual" TextBox
			TextBox tbOpExpensesUtilSewerActual;
			tbOpExpensesUtilSewerActual = (TextBox)e.Item.FindControl("txtOpExpensesUtilSewerActual");
			// Check "OpExpensesUtilSewerActual" TextBox
			if (CheckTextBox(tbOpExpensesUtilSewerActual))
			{
				// Add "OpExpensesUtilSewerActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesUtilSewerActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesUtilSewerActual.Text);
			}

			// Retrieve "OpExpensesUtilSewerProj" TextBox
			TextBox tbOpExpensesUtilSewerProj;
			tbOpExpensesUtilSewerProj = (TextBox)e.Item.FindControl("txtOpExpensesUtilSewerProj");
			// Check "OpExpensesUtilSewerProj" TextBox
			if (CheckTextBox(tbOpExpensesUtilSewerProj))
			{
				// Add "OpExpensesUtilSewerProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesUtilSewerProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesUtilSewerProj.Text);
			}

			// Retrieve "OpExpensesUtilIrrWaterActual" TextBox
			TextBox tbOpExpensesUtilIrrWaterActual;
			tbOpExpensesUtilIrrWaterActual = (TextBox)e.Item.FindControl("txtOpExpensesUtilIrrWaterActual");
			// Check "OpExpensesUtilIrrWaterActual" TextBox
			if (CheckTextBox(tbOpExpensesUtilIrrWaterActual))
			{
				// Add "OpExpensesUtilIrrWaterActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesUtilIrrWaterActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesUtilIrrWaterActual.Text);
			}

			// Retrieve "OpExpensesUtilIrrWaterProj" TextBox
			TextBox tbOpExpensesUtilIrrWaterProj;
			tbOpExpensesUtilIrrWaterProj = (TextBox)e.Item.FindControl("txtOpExpensesUtilIrrWaterProj");
			// Check "OpExpensesUtilIrrWaterProj" TextBox
			if (CheckTextBox(tbOpExpensesUtilIrrWaterProj))
			{
				// Add "OpExpensesUtilIrrWaterProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesUtilIrrWaterProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesUtilIrrWaterProj.Text);
			}

			// Retrieve "OpExpensesWagesSalActual" TextBox
			TextBox tbOpExpensesWagesSalActual;
			tbOpExpensesWagesSalActual = (TextBox)e.Item.FindControl("txtOpExpensesWagesSalActual");
			// Check "OpExpensesWagesSalActual" TextBox
			if (CheckTextBox(tbOpExpensesWagesSalActual))
			{
				// Add "OpExpensesWagesSalActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesWagesSalActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesWagesSalActual.Text);
			}

			// Retrieve "OpExpensesWagesSalProj" TextBox
			TextBox tbOpExpensesWagesSalProj;
			tbOpExpensesWagesSalProj = (TextBox)e.Item.FindControl("txtOpExpensesWagesSalProj");
			// Check "OpExpensesWagesSalProj" TextBox
			if (CheckTextBox(tbOpExpensesWagesSalProj))
			{
				// Add "OpExpensesWagesSalProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesWagesSalProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesWagesSalProj.Text);
			}

			// Retrieve "OpExpensesStampsPostActual" TextBox
			TextBox tbOpExpensesStampsPostActual;
			tbOpExpensesStampsPostActual = (TextBox)e.Item.FindControl("txtOpExpensesStampsPostActual");
			// Check "OpExpensesStampsPostActual" TextBox
			if (CheckTextBox(tbOpExpensesStampsPostActual))
			{
				// Add "OpExpensesStampsPostActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesStampsPostActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesStampsPostActual.Text);
			}

			// Retrieve "OpExpensesStampsPostProj" TextBox
			TextBox tbOpExpensesStampsPostProj;
			tbOpExpensesStampsPostProj = (TextBox)e.Item.FindControl("txtOpExpensesStampsPostProj");
			// Check "OpExpensesStampsPostProj" TextBox
			if (CheckTextBox(tbOpExpensesStampsPostProj))
			{
				// Add "OpExpensesStampsPostProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesStampsPostProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesStampsPostProj.Text);
			}

			// Retrieve "OpExpensesBankChargesActual" TextBox
			TextBox tbOpExpensesBankChargesActual;
			tbOpExpensesBankChargesActual = (TextBox)e.Item.FindControl("txtOpExpensesBankChargesActual");
			// Check "OpExpensesBankChargesActual" TextBox
			if (CheckTextBox(tbOpExpensesBankChargesActual))
			{
				// Add "OpExpensesBankChargesActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesBankChargesActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesBankChargesActual.Text);
			}

			// Retrieve "OpExpensesBankChargesProj" TextBox
			TextBox tbOpExpensesBankChargesProj;
			tbOpExpensesBankChargesProj = (TextBox)e.Item.FindControl("txtOpExpensesBankChargesProj");
			// Check "OpExpensesBankChargesProj" TextBox
			if (CheckTextBox(tbOpExpensesBankChargesProj))
			{
				// Add "OpExpensesBankChargesProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesBankChargesProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbOpExpensesBankChargesProj.Text);
			}

			// Retrieve "TotalAnnualDebtServiceActual" TextBox
			TextBox tbTotalAnnualDebtServiceActual;
			tbTotalAnnualDebtServiceActual = (TextBox)e.Item.FindControl("txtTotalAnnualDebtServiceActual");
			// Check "TotalAnnualDebtServiceActual" TextBox
			if (CheckTextBox(tbTotalAnnualDebtServiceActual))
			{
				// Add "TotalAnnualDebtServiceActual" Parameter
				objParam = objCommand.Parameters.Add("@TotalAnnualDebtServiceActual", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbTotalAnnualDebtServiceActual.Text);
			}

			// Retrieve "TotalAnnualDebtServiceProj" TextBox
			TextBox tbTotalAnnualDebtServiceProj;
			tbTotalAnnualDebtServiceProj = (TextBox)e.Item.FindControl("txtTotalAnnualDebtServiceProj");
			// Check "TotalAnnualDebtServiceProj" TextBox
			if (CheckTextBox(tbTotalAnnualDebtServiceProj))
			{
				// Add "TotalAnnualDebtServiceProj" Parameter
				objParam = objCommand.Parameters.Add("@TotalAnnualDebtServiceProj", SqlDbType.Money); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(tbTotalAnnualDebtServiceProj.Text);
			}

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

			dlIncomeProducingProp.EditItemIndex = -1;

			// Set MLS Cookie
			Session["mlsID"] = Convert.ToInt32(tbMLS.Text);

			Bind();
		}

		public void IncomeProducingProp_DataBound(Object sender, DataListItemEventArgs e) 
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

				// Retrieve "PropertyType" TableCell
				TableCell tcPropertyType;
				tcPropertyType = (TableCell)e.Item.FindControl("tcPropertyType");
				// DataBind "PropertyType" TableCell
				tcPropertyType.Text = drv["PropertyType"].ToString();

				// Retrieve "RealEstateDescription" TableCell
				TableCell tcRealEstateDescription;
				tcRealEstateDescription = (TableCell)e.Item.FindControl("tcRealEstateDescription");
				// DataBind "RealEstateDescription" TableCell
				tcRealEstateDescription.Text = drv["RealEstateDescription"].ToString();

				// Retrieve "ImprovementDescription" TableCell
				TableCell tcImprovementDescription;
				tcImprovementDescription = (TableCell)e.Item.FindControl("tcImprovementDescription");
				// DataBind "ImprovementDescription" TableCell
				tcImprovementDescription.Text = ConvertToString(drv["ImprovementDescription"]);

				// Retrieve "PersonalPropertyIncluded" TableCell
				TableCell tcPersonalPropertyIncluded;
				tcPersonalPropertyIncluded = (TableCell)e.Item.FindControl("tcPersonalPropertyIncluded");
				// DataBind "PersonalPropertyIncluded" TableCell
				tcPersonalPropertyIncluded.Text = ConvertToString(drv["PersonalPropertyIncluded"]);

				// Retrieve "ScheduleOfRent" TableCell
				TableCell tcScheduleOfRent;
				tcScheduleOfRent = (TableCell)e.Item.FindControl("tcScheduleOfRent");
				// DataBind "ScheduleOfRent" TableCell
				tcScheduleOfRent.Text = drv["ScheduleOfRent"].ToString();

				// Retrieve "ManagementAvailableYesNo" CheckBox
				CheckBox chbManagementAvailableYesNo;
				chbManagementAvailableYesNo = (CheckBox)e.Item.FindControl("chbManagementAvailableYesNo");
				// DataBind "ManagementAvailableYesNo" CheckBox
				chbManagementAvailableYesNo.Checked = Convert.ToBoolean(drv["ManagementAvailableYesNo"]);

				// Retrieve "ManagementDescription" TableCell
				TableCell tcManagementDescription;
				tcManagementDescription = (TableCell)e.Item.FindControl("tcManagementDescription");
				// DataBind "ManagementDescription" TableCell
				tcManagementDescription.Text = ConvertToString(drv["ManagementDescription"]);

				// Retrieve "DomWaterInfo" TableCell
				TableCell tcDomWaterInfo;
				tcDomWaterInfo = (TableCell)e.Item.FindControl("tcDomWaterInfo");
				// DataBind "DomWaterInfo" TableCell
				tcDomWaterInfo.Text = drv["DomWaterInfo"].ToString();

				// Retrieve "IrrWaterInfo" TableCell
				TableCell tcIrrWaterInfo;
				tcIrrWaterInfo = (TableCell)e.Item.FindControl("tcIrrWaterInfo");
				// DataBind "IrrWaterInfo" TableCell
				tcIrrWaterInfo.Text = drv["IrrWaterInfo"].ToString();

				// Retrieve "UtilityInfo" TableCell
				TableCell tcUtilityInfo;
				tcUtilityInfo = (TableCell)e.Item.FindControl("tcUtilityInfo");
				// DataBind "UtilityInfo" TableCell
				tcUtilityInfo.Text = drv["UtilityInfo"].ToString();

				// Retrieve "SewerSepticInfo" TableCell
				TableCell tcSewerSepticInfo;
				tcSewerSepticInfo = (TableCell)e.Item.FindControl("tcSewerSepticInfo");
				// DataBind "SewerSepticInfo" TableCell
				tcSewerSepticInfo.Text = drv["SewerSepticInfo"].ToString();

				// Retrieve "GrossScheduledIncomeActual" TableCell
				TableCell tcGrossScheduledIncomeActual;
				tcGrossScheduledIncomeActual = (TableCell)e.Item.FindControl("tcGrossScheduledIncomeActual");
				// DataBind "GrossScheduledIncomeActual" TableCell
				tcGrossScheduledIncomeActual.Text = ConvertToMoney(drv["GrossScheduledIncomeActual"]);

				// Retrieve "GrossScheduledIncomeProj" TableCell
				TableCell tcGrossScheduledIncomeProj;
				tcGrossScheduledIncomeProj = (TableCell)e.Item.FindControl("tcGrossScheduledIncomeProj");
				// DataBind "GrossScheduledIncomeProj" TableCell
				tcGrossScheduledIncomeProj.Text = ConvertToMoney(drv["GrossScheduledIncomeProj"]);

				// Retrieve "PlusOtherIncomeActual" TableCell
				TableCell tcPlusOtherIncomeActual;
				tcPlusOtherIncomeActual = (TableCell)e.Item.FindControl("tcPlusOtherIncomeActual");
				// DataBind "PlusOtherIncomeActual" TableCell
				tcPlusOtherIncomeActual.Text = ConvertToMoney(drv["PlusOtherIncomeActual"]);

				// Retrieve "PlusOtherIncomeProj" TableCell
				TableCell tcPlusOtherIncomeProj;
				tcPlusOtherIncomeProj = (TableCell)e.Item.FindControl("tcPlusOtherIncomeProj");
				// DataBind "PlusOtherIncomeProj" TableCell
				tcPlusOtherIncomeProj.Text = ConvertToMoney(drv["PlusOtherIncomeProj"]);

				// Retrieve "TotalIncomeActual" TableCell
				TableCell tcTotalIncomeActual;
				tcTotalIncomeActual = (TableCell)e.Item.FindControl("tcTotalIncomeActual");
				// DataBind "TotalIncomeActual" TableCell
				tcTotalIncomeActual.Text = ConvertToMoney(drv["TotalIncomeActual"]);

				// Retrieve "TotalIncomeProj" TableCell
				TableCell tcTotalIncomeProj;
				tcTotalIncomeProj = (TableCell)e.Item.FindControl("tcTotalIncomeProj");
				// DataBind "TotalIncomeProj" TableCell
				tcTotalIncomeProj.Text = ConvertToMoney(drv["TotalIncomeProj"]);

				// Retrieve "VacancyCreditLossActual" TableCell
				TableCell tcVacancyCreditLossActual;
				tcVacancyCreditLossActual = (TableCell)e.Item.FindControl("tcVacancyCreditLossActual");
				// DataBind "VacancyCreditLossActual" TableCell
				tcVacancyCreditLossActual.Text = ConvertToMoney(drv["VacancyCreditLossActual"]);

				// Retrieve "VacancyCreditLossProj" TableCell
				TableCell tcVacancyCreditLossProj;
				tcVacancyCreditLossProj = (TableCell)e.Item.FindControl("tcVacancyCreditLossProj");
				// DataBind "VacancyCreditLossProj" TableCell
				tcVacancyCreditLossProj.Text = ConvertToMoney(drv["VacancyCreditLossProj"]);

				// Retrieve "GrossOperatingIncomeActual" TableCell
				TableCell tcGrossOperatingIncomeActual;
				tcGrossOperatingIncomeActual = (TableCell)e.Item.FindControl("tcGrossOperatingIncomeActual");
				// DataBind "GrossOperatingIncomeActual" TableCell
				tcGrossOperatingIncomeActual.Text = ConvertToMoney(drv["GrossOperatingIncomeActual"]);

				// Retrieve "GrossOperatingIncomeProj" TableCell
				TableCell tcGrossOperatingIncomeProj;
				tcGrossOperatingIncomeProj = (TableCell)e.Item.FindControl("tcGrossOperatingIncomeProj");
				// DataBind "GrossOperatingIncomeProj" TableCell
				tcGrossOperatingIncomeProj.Text = ConvertToMoney(drv["GrossOperatingIncomeProj"]);

				// Retrieve "OpExpensesAdvertisingActual" TableCell
				TableCell tcOpExpensesAdvertisingActual;
				tcOpExpensesAdvertisingActual = (TableCell)e.Item.FindControl("tcOpExpensesAdvertisingActual");
				// DataBind "OpExpensesAdvertisingActual" TableCell
				tcOpExpensesAdvertisingActual.Text = ConvertToMoney(drv["OpExpensesAdvertisingActual"]);

				// Retrieve "OpExpensesAdvertisingProj" TableCell
				TableCell tcOpExpensesAdvertisingProj;
				tcOpExpensesAdvertisingProj = (TableCell)e.Item.FindControl("tcOpExpensesAdvertisingProj");
				// DataBind "OpExpensesAdvertisingProj" TableCell
				tcOpExpensesAdvertisingProj.Text = ConvertToMoney(drv["OpExpensesAdvertisingProj"]);

				// Retrieve "OpExpensesCleanMaintActual" TableCell
				TableCell tcOpExpensesCleanMaintActual;
				tcOpExpensesCleanMaintActual = (TableCell)e.Item.FindControl("tcOpExpensesCleanMaintActual");
				// DataBind "OpExpensesCleanMaintActual" TableCell
				tcOpExpensesCleanMaintActual.Text = ConvertToMoney(drv["OpExpensesCleanMaintActual"]);

				// Retrieve "OpExpensesCleanMaintProj" TableCell
				TableCell tcOpExpensesCleanMaintProj;
				tcOpExpensesCleanMaintProj = (TableCell)e.Item.FindControl("tcOpExpensesCleanMaintProj");
				// DataBind "OpExpensesCleanMaintProj" TableCell
				tcOpExpensesCleanMaintProj.Text = ConvertToMoney(drv["OpExpensesCleanMaintProj"]);

				// Retrieve "OpExpensesInsuranceActual" TableCell
				TableCell tcOpExpensesInsuranceActual;
				tcOpExpensesInsuranceActual = (TableCell)e.Item.FindControl("tcOpExpensesInsuranceActual");
				// DataBind "OpExpensesInsuranceActual" TableCell
				tcOpExpensesInsuranceActual.Text = ConvertToMoney(drv["OpExpensesInsuranceActual"]);

				// Retrieve "OpExpensesInsuranceProj" TableCell
				TableCell tcOpExpensesInsuranceProj;
				tcOpExpensesInsuranceProj = (TableCell)e.Item.FindControl("tcOpExpensesInsuranceProj");
				// DataBind "OpExpensesInsuranceProj" TableCell
				tcOpExpensesInsuranceProj.Text = ConvertToMoney(drv["OpExpensesInsuranceProj"]);

				// Retrieve "OpExpensesLegProfActual" TableCell
				TableCell tcOpExpensesLegProfActual;
				tcOpExpensesLegProfActual = (TableCell)e.Item.FindControl("tcOpExpensesLegProfActual");
				// DataBind "OpExpensesLegProfActual" TableCell
				tcOpExpensesLegProfActual.Text = ConvertToMoney(drv["OpExpensesLegProfActual"]);

				// Retrieve "OpExpensesLegProfProj" TableCell
				TableCell tcOpExpensesLegProfProj;
				tcOpExpensesLegProfProj = (TableCell)e.Item.FindControl("tcOpExpensesLegProfProj");
				// DataBind "OpExpensesLegProfProj" TableCell
				tcOpExpensesLegProfProj.Text = ConvertToMoney(drv["OpExpensesLegProfProj"]);

				// Retrieve "OpExpensesMiscRefActual" TableCell
				TableCell tcOpExpensesMiscRefActual;
				tcOpExpensesMiscRefActual = (TableCell)e.Item.FindControl("tcOpExpensesMiscRefActual");
				// DataBind "OpExpensesMiscRefActual" TableCell
				tcOpExpensesMiscRefActual.Text = ConvertToMoney(drv["OpExpensesMiscRefActual"]);

				// Retrieve "OpExpensesMiscRefProj" TableCell
				TableCell tcOpExpensesMiscRefProj;
				tcOpExpensesMiscRefProj = (TableCell)e.Item.FindControl("tcOpExpensesMiscRefProj");
				// DataBind "OpExpensesMiscRefProj" TableCell
				tcOpExpensesMiscRefProj.Text = ConvertToMoney(drv["OpExpensesMiscRefProj"]);

				// Retrieve "OpExpensesSuppliesActual" TableCell
				TableCell tcOpExpensesSuppliesActual;
				tcOpExpensesSuppliesActual = (TableCell)e.Item.FindControl("tcOpExpensesSuppliesActual");
				// DataBind "OpExpensesSuppliesActual" TableCell
				tcOpExpensesSuppliesActual.Text = ConvertToMoney(drv["OpExpensesSuppliesActual"]);

				// Retrieve "OpExpensesSuppliesProj" TableCell
				TableCell tcOpExpensesSuppliesProj;
				tcOpExpensesSuppliesProj = (TableCell)e.Item.FindControl("tcOpExpensesSuppliesProj");
				// DataBind "OpExpensesSuppliesProj" TableCell
				tcOpExpensesSuppliesProj.Text = ConvertToMoney(drv["OpExpensesSuppliesProj"]);

				// Retrieve "OpExpensesPropManActual" TableCell
				TableCell tcOpExpensesPropManActual;
				tcOpExpensesPropManActual = (TableCell)e.Item.FindControl("tcOpExpensesPropManActual");
				// DataBind "OpExpensesPropManActual" TableCell
				tcOpExpensesPropManActual.Text = ConvertToMoney(drv["OpExpensesPropManActual"]);

				// Retrieve "OpExpensesPropManProj" TableCell
				TableCell tcOpExpensesPropManProj;
				tcOpExpensesPropManProj = (TableCell)e.Item.FindControl("tcOpExpensesPropManProj");
				// DataBind "OpExpensesPropManProj" TableCell
				tcOpExpensesPropManProj.Text = ConvertToMoney(drv["OpExpensesPropManProj"]);

				// Retrieve "OpExpensesRepairsActual" TableCell
				TableCell tcOpExpensesRepairsActual;
				tcOpExpensesRepairsActual = (TableCell)e.Item.FindControl("tcOpExpensesRepairsActual");
				// DataBind "OpExpensesRepairsActual" TableCell
				tcOpExpensesRepairsActual.Text = ConvertToMoney(drv["OpExpensesRepairsActual"]);

				// Retrieve "OpExpensesRepairsProj" TableCell
				TableCell tcOpExpensesRepairsProj;
				tcOpExpensesRepairsProj = (TableCell)e.Item.FindControl("tcOpExpensesRepairsProj");
				// DataBind "OpExpensesRepairsProj" TableCell
				tcOpExpensesRepairsProj.Text = ConvertToMoney(drv["OpExpensesRepairsProj"]);

				// Retrieve "OpExpensesTaxesActual" TableCell
				TableCell tcOpExpensesTaxesActual;
				tcOpExpensesTaxesActual = (TableCell)e.Item.FindControl("tcOpExpensesTaxesActual");
				// DataBind "OpExpensesTaxesActual" TableCell
				tcOpExpensesTaxesActual.Text = ConvertToMoney(drv["OpExpensesTaxesActual"]);

				// Retrieve "OpExpensesTaxesProj" TableCell
				TableCell tcOpExpensesTaxesProj;
				tcOpExpensesTaxesProj = (TableCell)e.Item.FindControl("tcOpExpensesTaxesProj");
				// DataBind "OpExpensesTaxesProj" TableCell
				tcOpExpensesTaxesProj.Text = ConvertToMoney(drv["OpExpensesTaxesProj"]);

				// Retrieve "OpExpensesTelCableActual" TableCell
				TableCell tcOpExpensesTelCableActual;
				tcOpExpensesTelCableActual = (TableCell)e.Item.FindControl("tcOpExpensesTelCableActual");
				// DataBind "OpExpensesTelCableActual" TableCell
				tcOpExpensesTelCableActual.Text = ConvertToMoney(drv["OpExpensesTelCableActual"]);

				// Retrieve "OpExpensesTelCableProj" TableCell
				TableCell tcOpExpensesTelCableProj;
				tcOpExpensesTelCableProj = (TableCell)e.Item.FindControl("tcOpExpensesTelCableProj");
				// DataBind "OpExpensesTelCableProj" TableCell
				tcOpExpensesTelCableProj.Text = ConvertToMoney(drv["OpExpensesTelCableProj"]);

				// Retrieve "OpExpensesTrashRemActual" TableCell
				TableCell tcOpExpensesTrashRemActual;
				tcOpExpensesTrashRemActual = (TableCell)e.Item.FindControl("tcOpExpensesTrashRemActual");
				// DataBind "OpExpensesTrashRemActual" TableCell
				tcOpExpensesTrashRemActual.Text = ConvertToMoney(drv["OpExpensesTrashRemActual"]);

				// Retrieve "OpExpensesTrashRemProj" TableCell
				TableCell tcOpExpensesTrashRemProj;
				tcOpExpensesTrashRemProj = (TableCell)e.Item.FindControl("tcOpExpensesTrashRemProj");
				// DataBind "OpExpensesTrashRemProj" TableCell
				tcOpExpensesTrashRemProj.Text = ConvertToMoney(drv["OpExpensesTrashRemProj"]);

				// Retrieve "OpExpensesUtilGasActual" TableCell
				TableCell tcOpExpensesUtilGasActual;
				tcOpExpensesUtilGasActual = (TableCell)e.Item.FindControl("tcOpExpensesUtilGasActual");
				// DataBind "OpExpensesUtilGasActual" TableCell
				tcOpExpensesUtilGasActual.Text = ConvertToMoney(drv["OpExpensesUtilGasActual"]);

				// Retrieve "OpExpensesUtilGasProj" TableCell
				TableCell tcOpExpensesUtilGasProj;
				tcOpExpensesUtilGasProj = (TableCell)e.Item.FindControl("tcOpExpensesUtilGasProj");
				// DataBind "OpExpensesUtilGasProj" TableCell
				tcOpExpensesUtilGasProj.Text = ConvertToMoney(drv["OpExpensesUtilGasProj"]);

				// Retrieve "OpExpensesUtilElecActual" TableCell
				TableCell tcOpExpensesUtilElecActual;
				tcOpExpensesUtilElecActual = (TableCell)e.Item.FindControl("tcOpExpensesUtilElecActual");
				// DataBind "OpExpensesUtilElecActual" TableCell
				tcOpExpensesUtilElecActual.Text = ConvertToMoney(drv["OpExpensesUtilElecActual"]);

				// Retrieve "OpExpensesUtilElecProj" TableCell
				TableCell tcOpExpensesUtilElecProj;
				tcOpExpensesUtilElecProj = (TableCell)e.Item.FindControl("tcOpExpensesUtilElecProj");
				// DataBind "OpExpensesUtilElecProj" TableCell
				tcOpExpensesUtilElecProj.Text = ConvertToMoney(drv["OpExpensesUtilElecProj"]);

				// Retrieve "OpExpensesUtilSewerActual" TableCell
				TableCell tcOpExpensesUtilSewerActual;
				tcOpExpensesUtilSewerActual = (TableCell)e.Item.FindControl("tcOpExpensesUtilSewerActual");
				// DataBind "OpExpensesUtilSewerActual" TableCell
				tcOpExpensesUtilSewerActual.Text = ConvertToMoney(drv["OpExpensesUtilSewerActual"]);

				// Retrieve "OpExpensesUtilSewerProj" TableCell
				TableCell tcOpExpensesUtilSewerProj;
				tcOpExpensesUtilSewerProj = (TableCell)e.Item.FindControl("tcOpExpensesUtilSewerProj");
				// DataBind "OpExpensesUtilSewerProj" TableCell
				tcOpExpensesUtilSewerProj.Text = ConvertToMoney(drv["OpExpensesUtilSewerProj"]);

				// Retrieve "OpExpensesUtilIrrWaterActual" TableCell
				TableCell tcOpExpensesUtilIrrWaterActual;
				tcOpExpensesUtilIrrWaterActual = (TableCell)e.Item.FindControl("tcOpExpensesUtilIrrWaterActual");
				// DataBind "OpExpensesUtilIrrWaterActual" TableCell
				tcOpExpensesUtilIrrWaterActual.Text = ConvertToMoney(drv["OpExpensesUtilIrrWaterActual"]);

				// Retrieve "OpExpensesUtilIrrWaterProj" TableCell
				TableCell tcOpExpensesUtilIrrWaterProj;
				tcOpExpensesUtilIrrWaterProj = (TableCell)e.Item.FindControl("tcOpExpensesUtilIrrWaterProj");
				// DataBind "OpExpensesUtilIrrWaterProj" TableCell
				tcOpExpensesUtilIrrWaterProj.Text = ConvertToMoney(drv["OpExpensesUtilIrrWaterProj"]);

				// Retrieve "OpExpensesWagesSalActual" TableCell
				TableCell tcOpExpensesWagesSalActual;
				tcOpExpensesWagesSalActual = (TableCell)e.Item.FindControl("tcOpExpensesWagesSalActual");
				// DataBind "OpExpensesWagesSalActual" TableCell
				tcOpExpensesWagesSalActual.Text = ConvertToMoney(drv["OpExpensesWagesSalActual"]);

				// Retrieve "OpExpensesWagesSalProj" TableCell
				TableCell tcOpExpensesWagesSalProj;
				tcOpExpensesWagesSalProj = (TableCell)e.Item.FindControl("tcOpExpensesWagesSalProj");
				// DataBind "OpExpensesWagesSalProj" TableCell
				tcOpExpensesWagesSalProj.Text = ConvertToMoney(drv["OpExpensesWagesSalProj"]);

				// Retrieve "OpExpensesStampsPostActual" TableCell
				TableCell tcOpExpensesStampsPostActual;
				tcOpExpensesStampsPostActual = (TableCell)e.Item.FindControl("tcOpExpensesStampsPostActual");
				// DataBind "OpExpensesStampsPostActual" TableCell
				tcOpExpensesStampsPostActual.Text = ConvertToMoney(drv["OpExpensesStampsPostActual"]);

				// Retrieve "OpExpensesStampsPostProj" TableCell
				TableCell tcOpExpensesStampsPostProj;
				tcOpExpensesStampsPostProj = (TableCell)e.Item.FindControl("tcOpExpensesStampsPostProj");
				// DataBind "OpExpensesStampsPostProj" TableCell
				tcOpExpensesStampsPostProj.Text = ConvertToMoney(drv["OpExpensesStampsPostProj"]);

				// Retrieve "OpExpensesBankChargesActual" TableCell
				TableCell tcOpExpensesBankChargesActual;
				tcOpExpensesBankChargesActual = (TableCell)e.Item.FindControl("tcOpExpensesBankChargesActual");
				// DataBind "OpExpensesBankChargesActual" TableCell
				tcOpExpensesBankChargesActual.Text = ConvertToMoney(drv["OpExpensesBankChargesActual"]);

				// Retrieve "OpExpensesBankChargesProj" TableCell
				TableCell tcOpExpensesBankChargesProj;
				tcOpExpensesBankChargesProj = (TableCell)e.Item.FindControl("tcOpExpensesBankChargesProj");
				// DataBind "OpExpensesBankChargesProj" TableCell
				tcOpExpensesBankChargesProj.Text = ConvertToMoney(drv["OpExpensesBankChargesProj"]);

				// Retrieve "TotalOperatingExpensesActual" TableCell
				TableCell tcTotalOperatingExpensesActual;
				tcTotalOperatingExpensesActual = (TableCell)e.Item.FindControl("tcTotalOperatingExpensesActual");
				// DataBind "TotalOperatingExpensesActual" TableCell
				tcTotalOperatingExpensesActual.Text = ConvertToMoney(drv["TotalOperatingExpensesActual"]);

				// Retrieve "TotalOperatingExpensesProj" TableCell
				TableCell tcTotalOperatingExpensesProj;
				tcTotalOperatingExpensesProj = (TableCell)e.Item.FindControl("tcTotalOperatingExpensesProj");
				// DataBind "TotalOperatingExpensesProj" TableCell
				tcTotalOperatingExpensesProj.Text = ConvertToMoney(drv["TotalOperatingExpensesProj"]);

				// Retrieve "NetOperatingIncomeActual" TableCell
				TableCell tcNetOperatingIncomeActual;
				tcNetOperatingIncomeActual = (TableCell)e.Item.FindControl("tcNetOperatingIncomeActual");
				// DataBind "NetOperatingIncomeActual" TableCell
				tcNetOperatingIncomeActual.Text = ConvertToMoney(drv["NetOperatingIncomeActual"]);

				// Retrieve "NetOperatingIncomeProj" TableCell
				TableCell tcNetOperatingIncomeProj;
				tcNetOperatingIncomeProj = (TableCell)e.Item.FindControl("tcNetOperatingIncomeProj");
				// DataBind "NetOperatingIncomeProj" TableCell
				tcNetOperatingIncomeProj.Text = ConvertToMoney(drv["NetOperatingIncomeProj"]);

				// Retrieve "TotalAnnualDebtServiceActual" TableCell
				TableCell tcTotalAnnualDebtServiceActual;
				tcTotalAnnualDebtServiceActual = (TableCell)e.Item.FindControl("tcTotalAnnualDebtServiceActual");
				// DataBind "TotalAnnualDebtServiceActual" TableCell
				tcTotalAnnualDebtServiceActual.Text = ConvertToMoney(drv["TotalAnnualDebtServiceActual"]);

				// Retrieve "TotalAnnualDebtServiceProj" TableCell
				TableCell tcTotalAnnualDebtServiceProj;
				tcTotalAnnualDebtServiceProj = (TableCell)e.Item.FindControl("tcTotalAnnualDebtServiceProj");
				// DataBind "TotalAnnualDebtServiceProj" TableCell
				tcTotalAnnualDebtServiceProj.Text = ConvertToMoney(drv["TotalAnnualDebtServiceProj"]);

				// Retrieve "CashFlowBeforeTaxesActual" TableCell
				TableCell tcCashFlowBeforeTaxesActual;
				tcCashFlowBeforeTaxesActual = (TableCell)e.Item.FindControl("tcCashFlowBeforeTaxesActual");
				// DataBind "CashFlowBeforeTaxesActual" TableCell
				tcCashFlowBeforeTaxesActual.Text = ConvertToMoney(drv["CashFlowBeforeTaxesActual"]);

				// Retrieve "CashFlowBeforeTaxesProj" TableCell
				TableCell tcCashFlowBeforeTaxesProj;
				tcCashFlowBeforeTaxesProj = (TableCell)e.Item.FindControl("tcCashFlowBeforeTaxesProj");
				// DataBind "CashFlowBeforeTaxesProj" TableCell
				tcCashFlowBeforeTaxesProj.Text = ConvertToMoney(drv["CashFlowBeforeTaxesProj"]);

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

				// Retrieve "PropertyType" TextBox
				TextBox tbPropertyType;
				tbPropertyType = (TextBox)e.Item.FindControl("txtPropertyType");
				// Fill "PropertyType" TextBox
				tbPropertyType.Text = drv["PropertyType"].ToString();

				// Retrieve "RealEstateDescription" TextBox
				TextBox tbRealEstateDescription;
				tbRealEstateDescription = (TextBox)e.Item.FindControl("txtRealEstateDescription");
				// Fill "RealEstateDescription" TextBox
				tbRealEstateDescription.Text = drv["RealEstateDescription"].ToString();

				// Retrieve "ImprovementDescription" TextBox
				TextBox tbImprovementDescription;
				tbImprovementDescription = (TextBox)e.Item.FindControl("txtImprovementDescription");
				// Fill "ImprovementDescription" TextBox
				tbImprovementDescription.Text = drv["ImprovementDescription"].ToString();

				// Retrieve "PersonalPropertyIncluded" TextBox
				TextBox tbPersonalPropertyIncluded;
				tbPersonalPropertyIncluded = (TextBox)e.Item.FindControl("txtPersonalPropertyIncluded");
				// Fill "PersonalPropertyIncluded" TextBox
				tbPersonalPropertyIncluded.Text = drv["PersonalPropertyIncluded"].ToString();

				// Retrieve "ScheduleOfRent" TextBox
				TextBox tbScheduleOfRent;
				tbScheduleOfRent = (TextBox)e.Item.FindControl("txtScheduleOfRent");
				// Fill "ScheduleOfRent" TextBox
				tbScheduleOfRent.Text = drv["ScheduleOfRent"].ToString();

				// Retrieve "ManagementAvailableYesNo" CheckBox
				CheckBox chbManagementAvailableYesNo;
				chbManagementAvailableYesNo = (CheckBox)e.Item.FindControl("chbManagementAvailableYesNo");
				// DataBind "ManagementAvailableYesNo" CheckBox
				chbManagementAvailableYesNo.Checked = Convert.ToBoolean(drv["ManagementAvailableYesNo"]);

				// Retrieve "ManagementDescription" TextBox
				TextBox tbManagementDescription;
				tbManagementDescription = (TextBox)e.Item.FindControl("txtManagementDescription");
				// Fill "ManagementDescription" TextBox
				tbManagementDescription.Text = drv["ManagementDescription"].ToString();

				// Retrieve "DomWaterInfo" TextBox
				TextBox tbDomWaterInfo;
				tbDomWaterInfo = (TextBox)e.Item.FindControl("txtDomWaterInfo");
				// Fill "DomWaterInfo" TextBox
				tbDomWaterInfo.Text = drv["DomWaterInfo"].ToString();

				// Retrieve "IrrWaterInfo" TextBox
				TextBox tbIrrWaterInfo;
				tbIrrWaterInfo = (TextBox)e.Item.FindControl("txtIrrWaterInfo");
				// Fill "IrrWaterInfo" TextBox
				tbIrrWaterInfo.Text = drv["IrrWaterInfo"].ToString();

				// Retrieve "UtilityInfo" TextBox
				TextBox tbUtilityInfo;
				tbUtilityInfo = (TextBox)e.Item.FindControl("txtUtilityInfo");
				// Fill "UtilityInfo" TextBox
				tbUtilityInfo.Text = drv["UtilityInfo"].ToString();

				// Retrieve "SewerSepticInfo" TextBox
				TextBox tbSewerSepticInfo;
				tbSewerSepticInfo = (TextBox)e.Item.FindControl("txtSewerSepticInfo");
				// Fill "SewerSepticInfo" TextBox
				tbSewerSepticInfo.Text = drv["SewerSepticInfo"].ToString();

				// Retrieve "GrossScheduledIncomeActual" TextBox
				TextBox tbGrossScheduledIncomeActual;
				tbGrossScheduledIncomeActual = (TextBox)e.Item.FindControl("txtGrossScheduledIncomeActual");
				// Fill "GrossScheduledIncomeActual" TextBox
				tbGrossScheduledIncomeActual.Text = ConvertToNumeric(drv["GrossScheduledIncomeActual"]);

				// Retrieve "GrossScheduledIncomeProj" TextBox
				TextBox tbGrossScheduledIncomeProj;
				tbGrossScheduledIncomeProj = (TextBox)e.Item.FindControl("txtGrossScheduledIncomeProj");
				// Fill "GrossScheduledIncomeProj" TextBox
				tbGrossScheduledIncomeProj.Text = ConvertToNumeric(drv["GrossScheduledIncomeProj"]);

				// Retrieve "PlusOtherIncomeActual" TextBox
				TextBox tbPlusOtherIncomeActual;
				tbPlusOtherIncomeActual = (TextBox)e.Item.FindControl("txtPlusOtherIncomeActual");
				// Fill "PlusOtherIncomeActual" TextBox
				tbPlusOtherIncomeActual.Text = ConvertToNumeric(drv["PlusOtherIncomeActual"]);

				// Retrieve "PlusOtherIncomeProj" TextBox
				TextBox tbPlusOtherIncomeProj;
				tbPlusOtherIncomeProj = (TextBox)e.Item.FindControl("txtPlusOtherIncomeProj");
				// Fill "PlusOtherIncomeProj" TextBox
				tbPlusOtherIncomeProj.Text = ConvertToNumeric(drv["PlusOtherIncomeProj"]);

				// Retrieve "VacancyCreditLossActual" TextBox
				TextBox tbVacancyCreditLossActual;
				tbVacancyCreditLossActual = (TextBox)e.Item.FindControl("txtVacancyCreditLossActual");
				// Fill "VacancyCreditLossActual" TextBox
				tbVacancyCreditLossActual.Text = ConvertToNumeric(drv["VacancyCreditLossActual"]);

				// Retrieve "VacancyCreditLossProj" TextBox
				TextBox tbVacancyCreditLossProj;
				tbVacancyCreditLossProj = (TextBox)e.Item.FindControl("txtVacancyCreditLossProj");
				// Fill "VacancyCreditLossProj" TextBox
				tbVacancyCreditLossProj.Text = ConvertToNumeric(drv["VacancyCreditLossProj"]);

				// Retrieve "OpExpensesAdvertisingActual" TextBox
				TextBox tbOpExpensesAdvertisingActual;
				tbOpExpensesAdvertisingActual = (TextBox)e.Item.FindControl("txtOpExpensesAdvertisingActual");
				// Fill "OpExpensesAdvertisingActual" TextBox
				tbOpExpensesAdvertisingActual.Text = ConvertToNumeric(drv["OpExpensesAdvertisingActual"]);

				// Retrieve "OpExpensesAdvertisingProj" TextBox
				TextBox tbOpExpensesAdvertisingProj;
				tbOpExpensesAdvertisingProj = (TextBox)e.Item.FindControl("txtOpExpensesAdvertisingProj");
				// Fill "OpExpensesAdvertisingProj" TextBox
				tbOpExpensesAdvertisingProj.Text = ConvertToNumeric(drv["OpExpensesAdvertisingProj"]);

				// Retrieve "OpExpensesCleanMaintActual" TextBox
				TextBox tbOpExpensesCleanMaintActual;
				tbOpExpensesCleanMaintActual = (TextBox)e.Item.FindControl("txtOpExpensesCleanMaintActual");
				// Fill "OpExpensesCleanMaintActual" TextBox
				tbOpExpensesCleanMaintActual.Text = ConvertToNumeric(drv["OpExpensesCleanMaintActual"]);

				// Retrieve "OpExpensesCleanMaintProj" TextBox
				TextBox tbOpExpensesCleanMaintProj;
				tbOpExpensesCleanMaintProj = (TextBox)e.Item.FindControl("txtOpExpensesCleanMaintProj");
				// Fill "OpExpensesCleanMaintProj" TextBox
				tbOpExpensesCleanMaintProj.Text = ConvertToNumeric(drv["OpExpensesCleanMaintProj"]);

				// Retrieve "OpExpensesInsuranceActual" TextBox
				TextBox tbOpExpensesInsuranceActual;
				tbOpExpensesInsuranceActual = (TextBox)e.Item.FindControl("txtOpExpensesInsuranceActual");
				// Fill "OpExpensesInsuranceActual" TextBox
				tbOpExpensesInsuranceActual.Text = ConvertToNumeric(drv["OpExpensesInsuranceActual"]);

				// Retrieve "OpExpensesInsuranceProj" TextBox
				TextBox tbOpExpensesInsuranceProj;
				tbOpExpensesInsuranceProj = (TextBox)e.Item.FindControl("txtOpExpensesInsuranceProj");
				// Fill "OpExpensesInsuranceProj" TextBox
				tbOpExpensesInsuranceProj.Text = ConvertToNumeric(drv["OpExpensesInsuranceProj"]);

				// Retrieve "OpExpensesLegProfActual" TextBox
				TextBox tbOpExpensesLegProfActual;
				tbOpExpensesLegProfActual = (TextBox)e.Item.FindControl("txtOpExpensesLegProfActual");
				// Fill "OpExpensesLegProfActual" TextBox
				tbOpExpensesLegProfActual.Text = ConvertToNumeric(drv["OpExpensesLegProfActual"]);

				// Retrieve "OpExpensesLegProfProj" TextBox
				TextBox tbOpExpensesLegProfProj;
				tbOpExpensesLegProfProj = (TextBox)e.Item.FindControl("txtOpExpensesLegProfProj");
				// Fill "OpExpensesLegProfProj" TextBox
				tbOpExpensesLegProfProj.Text = ConvertToNumeric(drv["OpExpensesLegProfProj"]);

				// Retrieve "OpExpensesMiscRefActual" TextBox
				TextBox tbOpExpensesMiscRefActual;
				tbOpExpensesMiscRefActual = (TextBox)e.Item.FindControl("txtOpExpensesMiscRefActual");
				// Fill "OpExpensesMiscRefActual" TextBox
				tbOpExpensesMiscRefActual.Text = ConvertToNumeric(drv["OpExpensesMiscRefActual"]);

				// Retrieve "OpExpensesMiscRefProj" TextBox
				TextBox tbOpExpensesMiscRefProj;
				tbOpExpensesMiscRefProj = (TextBox)e.Item.FindControl("txtOpExpensesMiscRefProj");
				// Fill "OpExpensesMiscRefProj" TextBox
				tbOpExpensesMiscRefProj.Text = ConvertToNumeric(drv["OpExpensesMiscRefProj"]);

				// Retrieve "OpExpensesSuppliesActual" TextBox
				TextBox tbOpExpensesSuppliesActual;
				tbOpExpensesSuppliesActual = (TextBox)e.Item.FindControl("txtOpExpensesSuppliesActual");
				// Fill "OpExpensesSuppliesActual" TextBox
				tbOpExpensesSuppliesActual.Text = ConvertToNumeric(drv["OpExpensesSuppliesActual"]);

				// Retrieve "OpExpensesSuppliesProj" TextBox
				TextBox tbOpExpensesSuppliesProj;
				tbOpExpensesSuppliesProj = (TextBox)e.Item.FindControl("txtOpExpensesSuppliesProj");
				// Fill "OpExpensesSuppliesProj" TextBox
				tbOpExpensesSuppliesProj.Text = ConvertToNumeric(drv["OpExpensesSuppliesProj"]);

				// Retrieve "OpExpensesPropManActual" TextBox
				TextBox tbOpExpensesPropManActual;
				tbOpExpensesPropManActual = (TextBox)e.Item.FindControl("txtOpExpensesPropManActual");
				// Fill "OpExpensesPropManActual" TextBox
				tbOpExpensesPropManActual.Text = ConvertToNumeric(drv["OpExpensesPropManActual"]);

				// Retrieve "OpExpensesPropManProj" TextBox
				TextBox tbOpExpensesPropManProj;
				tbOpExpensesPropManProj = (TextBox)e.Item.FindControl("txtOpExpensesPropManProj");
				// Fill "OpExpensesPropManProj" TextBox
				tbOpExpensesPropManProj.Text = ConvertToNumeric(drv["OpExpensesPropManProj"]);

				// Retrieve "OpExpensesRepairsActual" TextBox
				TextBox tbOpExpensesRepairsActual;
				tbOpExpensesRepairsActual = (TextBox)e.Item.FindControl("txtOpExpensesRepairsActual");
				// Fill "OpExpensesRepairsActual" TextBox
				tbOpExpensesRepairsActual.Text = ConvertToNumeric(drv["OpExpensesRepairsActual"]);

				// Retrieve "OpExpensesRepairsProj" TextBox
				TextBox tbOpExpensesRepairsProj;
				tbOpExpensesRepairsProj = (TextBox)e.Item.FindControl("txtOpExpensesRepairsProj");
				// Fill "OpExpensesRepairsProj" TextBox
				tbOpExpensesRepairsProj.Text = ConvertToNumeric(drv["OpExpensesRepairsProj"]);

				// Retrieve "OpExpensesTaxesActual" TextBox
				TextBox tbOpExpensesTaxesActual;
				tbOpExpensesTaxesActual = (TextBox)e.Item.FindControl("txtOpExpensesTaxesActual");
				// Fill "OpExpensesTaxesActual" TextBox
				tbOpExpensesTaxesActual.Text = ConvertToNumeric(drv["OpExpensesTaxesActual"]);

				// Retrieve "OpExpensesTaxesProj" TextBox
				TextBox tbOpExpensesTaxesProj;
				tbOpExpensesTaxesProj = (TextBox)e.Item.FindControl("txtOpExpensesTaxesProj");
				// Fill "OpExpensesTaxesProj" TextBox
				tbOpExpensesTaxesProj.Text = ConvertToNumeric(drv["OpExpensesTaxesProj"]);

				// Retrieve "OpExpensesTelCableActual" TextBox
				TextBox tbOpExpensesTelCableActual;
				tbOpExpensesTelCableActual = (TextBox)e.Item.FindControl("txtOpExpensesTelCableActual");
				// Fill "OpExpensesTelCableActual" TextBox
				tbOpExpensesTelCableActual.Text = ConvertToNumeric(drv["OpExpensesTelCableActual"]);

				// Retrieve "OpExpensesTelCableProj" TextBox
				TextBox tbOpExpensesTelCableProj;
				tbOpExpensesTelCableProj = (TextBox)e.Item.FindControl("txtOpExpensesTelCableProj");
				// Fill "OpExpensesTelCableProj" TextBox
				tbOpExpensesTelCableProj.Text = ConvertToNumeric(drv["OpExpensesTelCableProj"]);

				// Retrieve "OpExpensesTrashRemActual" TextBox
				TextBox tbOpExpensesTrashRemActual;
				tbOpExpensesTrashRemActual = (TextBox)e.Item.FindControl("txtOpExpensesTrashRemActual");
				// Fill "OpExpensesTrashRemActual" TextBox
				tbOpExpensesTrashRemActual.Text = ConvertToNumeric(drv["OpExpensesTrashRemActual"]);

				// Retrieve "OpExpensesTrashRemProj" TextBox
				TextBox tbOpExpensesTrashRemProj;
				tbOpExpensesTrashRemProj = (TextBox)e.Item.FindControl("txtOpExpensesTrashRemProj");
				// Fill "OpExpensesTrashRemProj" TextBox
				tbOpExpensesTrashRemProj.Text = ConvertToNumeric(drv["OpExpensesTrashRemProj"]);

				// Retrieve "OpExpensesUtilGasActual" TextBox
				TextBox tbOpExpensesUtilGasActual;
				tbOpExpensesUtilGasActual = (TextBox)e.Item.FindControl("txtOpExpensesUtilGasActual");
				// Fill "OpExpensesUtilGasActual" TextBox
				tbOpExpensesUtilGasActual.Text = ConvertToNumeric(drv["OpExpensesUtilGasActual"]);

				// Retrieve "OpExpensesUtilGasProj" TextBox
				TextBox tbOpExpensesUtilGasProj;
				tbOpExpensesUtilGasProj = (TextBox)e.Item.FindControl("txtOpExpensesUtilGasProj");
				// Fill "OpExpensesUtilGasProj" TextBox
				tbOpExpensesUtilGasProj.Text = ConvertToNumeric(drv["OpExpensesUtilGasProj"]);

				// Retrieve "OpExpensesUtilElecActual" TextBox
				TextBox tbOpExpensesUtilElecActual;
				tbOpExpensesUtilElecActual = (TextBox)e.Item.FindControl("txtOpExpensesUtilElecActual");
				// Fill "OpExpensesUtilElecActual" TextBox
				tbOpExpensesUtilElecActual.Text = ConvertToNumeric(drv["OpExpensesUtilElecActual"]);

				// Retrieve "OpExpensesUtilElecProj" TextBox
				TextBox tbOpExpensesUtilElecProj;
				tbOpExpensesUtilElecProj = (TextBox)e.Item.FindControl("txtOpExpensesUtilElecProj");
				// Fill "OpExpensesUtilElecProj" TextBox
				tbOpExpensesUtilElecProj.Text = ConvertToNumeric(drv["OpExpensesUtilElecProj"]);

				// Retrieve "OpExpensesUtilSewerActual" TextBox
				TextBox tbOpExpensesUtilSewerActual;
				tbOpExpensesUtilSewerActual = (TextBox)e.Item.FindControl("txtOpExpensesUtilSewerActual");
				// Fill "OpExpensesUtilSewerActual" TextBox
				tbOpExpensesUtilSewerActual.Text = ConvertToNumeric(drv["OpExpensesUtilSewerActual"]);

				// Retrieve "OpExpensesUtilSewerProj" TextBox
				TextBox tbOpExpensesUtilSewerProj;
				tbOpExpensesUtilSewerProj = (TextBox)e.Item.FindControl("txtOpExpensesUtilSewerProj");
				// Fill "OpExpensesUtilSewerProj" TextBox
				tbOpExpensesUtilSewerProj.Text = ConvertToNumeric(drv["OpExpensesUtilSewerProj"]);

				// Retrieve "OpExpensesUtilIrrWaterActual" TextBox
				TextBox tbOpExpensesUtilIrrWaterActual;
				tbOpExpensesUtilIrrWaterActual = (TextBox)e.Item.FindControl("txtOpExpensesUtilIrrWaterActual");
				// Fill "OpExpensesUtilIrrWaterActual" TextBox
				tbOpExpensesUtilIrrWaterActual.Text = ConvertToNumeric(drv["OpExpensesUtilIrrWaterActual"]);

				// Retrieve "OpExpensesUtilIrrWaterProj" TextBox
				TextBox tbOpExpensesUtilIrrWaterProj;
				tbOpExpensesUtilIrrWaterProj = (TextBox)e.Item.FindControl("txtOpExpensesUtilIrrWaterProj");
				// Fill "OpExpensesUtilIrrWaterProj" TextBox
				tbOpExpensesUtilIrrWaterProj.Text = ConvertToNumeric(drv["OpExpensesUtilIrrWaterProj"]);

				// Retrieve "OpExpensesWagesSalActual" TextBox
				TextBox tbOpExpensesWagesSalActual;
				tbOpExpensesWagesSalActual = (TextBox)e.Item.FindControl("txtOpExpensesWagesSalActual");
				// Fill "OpExpensesWagesSalActual" TextBox
				tbOpExpensesWagesSalActual.Text = ConvertToNumeric(drv["OpExpensesWagesSalActual"]);

				// Retrieve "OpExpensesWagesSalProj" TextBox
				TextBox tbOpExpensesWagesSalProj;
				tbOpExpensesWagesSalProj = (TextBox)e.Item.FindControl("txtOpExpensesWagesSalProj");
				// Fill "OpExpensesWagesSalProj" TextBox
				tbOpExpensesWagesSalProj.Text = ConvertToNumeric(drv["OpExpensesWagesSalProj"]);

				// Retrieve "OpExpensesStampsPostActual" TextBox
				TextBox tbOpExpensesStampsPostActual;
				tbOpExpensesStampsPostActual = (TextBox)e.Item.FindControl("txtOpExpensesStampsPostActual");
				// Fill "OpExpensesStampsPostActual" TextBox
				tbOpExpensesStampsPostActual.Text = ConvertToNumeric(drv["OpExpensesStampsPostActual"]);

				// Retrieve "OpExpensesStampsPostProj" TextBox
				TextBox tbOpExpensesStampsPostProj;
				tbOpExpensesStampsPostProj = (TextBox)e.Item.FindControl("txtOpExpensesStampsPostProj");
				// Fill "OpExpensesStampsPostProj" TextBox
				tbOpExpensesStampsPostProj.Text = ConvertToNumeric(drv["OpExpensesStampsPostProj"]);

				// Retrieve "OpExpensesBankChargesActual" TextBox
				TextBox tbOpExpensesBankChargesActual;
				tbOpExpensesBankChargesActual = (TextBox)e.Item.FindControl("txtOpExpensesBankChargesActual");
				// Fill "OpExpensesBankChargesActual" TextBox
				tbOpExpensesBankChargesActual.Text = ConvertToNumeric(drv["OpExpensesBankChargesActual"]);

				// Retrieve "OpExpensesBankChargesProj" TextBox
				TextBox tbOpExpensesBankChargesProj;
				tbOpExpensesBankChargesProj = (TextBox)e.Item.FindControl("txtOpExpensesBankChargesProj");
				// Fill "OpExpensesBankChargesProj" TextBox
				tbOpExpensesBankChargesProj.Text = ConvertToNumeric(drv["OpExpensesBankChargesProj"]);

				// Retrieve "TotalAnnualDebtServiceActual" TextBox
				TextBox tbTotalAnnualDebtServiceActual;
				tbTotalAnnualDebtServiceActual = (TextBox)e.Item.FindControl("txtTotalAnnualDebtServiceActual");
				// Fill "TotalAnnualDebtServiceActual" TextBox
				tbTotalAnnualDebtServiceActual.Text = ConvertToNumeric(drv["TotalAnnualDebtServiceActual"]);

				// Retrieve "TotalAnnualDebtServiceProj" TextBox
				TextBox tbTotalAnnualDebtServiceProj;
				tbTotalAnnualDebtServiceProj = (TextBox)e.Item.FindControl("txtTotalAnnualDebtServiceProj");
				// Fill "TotalAnnualDebtServiceProj" TextBox
				tbTotalAnnualDebtServiceProj.Text = ConvertToNumeric(drv["TotalAnnualDebtServiceProj"]);

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
