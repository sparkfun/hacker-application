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
	/// Summary description for AddIncomeProducingProperty.
	/// </summary>
	public class AddIncomeProducingProperty : Main
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
		public TextBox txtPropertyType;
		public TextBox txtRealEstateDescription;
		public TextBox txtImprovementDescription;
		public TextBox txtPersonalPropertyIncluded;
		public TextBox txtScheduleOfRent;
		public CheckBox chbManagementAvailableYesNo;
		public TextBox txtManagementDescription;
		public TextBox txtDomWaterInfo;
		public TextBox txtIrrWaterInfo;
		public TextBox txtUtilityInfo;
		public TextBox txtSewerSepticInfo;
		public TextBox txtGrossScheduledIncomeActual;
		public TextBox txtGrossScheduledIncomeProj;
		public TextBox txtPlusOtherIncomeActual;
		public TextBox txtPlusOtherIncomeProj;
		public TextBox txtVacancyCreditLossActual;
		public TextBox txtVacancyCreditLossProj;
		public TextBox txtOpExpensesAdvertisingActual;
		public TextBox txtOpExpensesAdvertisingProj;
		public TextBox txtOpExpensesCleanMaintActual;
		public TextBox txtOpExpensesCleanMaintProj;
		public TextBox txtOpExpensesInsuranceActual;
		public TextBox txtOpExpensesInsuranceProj;
		public TextBox txtOpExpensesLegProfActual;
		public TextBox txtOpExpensesLegProfProj;
		public TextBox txtOpExpensesMiscRefActual;
		public TextBox txtOpExpensesMiscRefProj;
		public TextBox txtOpExpensesSuppliesActual;
		public TextBox txtOpExpensesSuppliesProj;
		public TextBox txtOpExpensesPropManActual;
		public TextBox txtOpExpensesPropManProj;
		public TextBox txtOpExpensesRepairsActual;
		public TextBox txtOpExpensesRepairsProj;
		public TextBox txtOpExpensesTaxesActual;
		public TextBox txtOpExpensesTaxesProj;
		public TextBox txtOpExpensesTelCableActual;
		public TextBox txtOpExpensesTelCableProj;
		public TextBox txtOpExpensesTrashRemActual;
		public TextBox txtOpExpensesTrashRemProj;
		public TextBox txtOpExpensesUtilGasActual;
		public TextBox txtOpExpensesUtilGasProj;
		public TextBox txtOpExpensesUtilElecActual;
		public TextBox txtOpExpensesUtilElecProj;
		public TextBox txtOpExpensesUtilSewerActual;
		public TextBox txtOpExpensesUtilSewerProj;
		public TextBox txtOpExpensesUtilIrrWaterActual;
		public TextBox txtOpExpensesUtilIrrWaterProj;
		public TextBox txtOpExpensesWagesSalActual;
		public TextBox txtOpExpensesWagesSalProj;
		public TextBox txtOpExpensesStampsPostActual;
		public TextBox txtOpExpensesStampsPostProj;
		public TextBox txtOpExpensesBankChargesActual;
		public TextBox txtOpExpensesBankChargesProj;
		public TextBox txtTotalAnnualDebtServiceActual;
		public TextBox txtTotalAnnualDebtServiceProj;
		public DropDownList ddlPossession;
		public DropDownList ddlTerms;
		public TextBox txtFeatures;
		public TextBox txtInclusions;
		public TextBox txtExclusions;
		public TextBox txtDisclosures;
		public TextBox txtMapDirections;

		public AddIncomeProducingProperty()
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
			string strVirtualPath = Request.ApplicationPath + "/config/income_producing_prop.xml";

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

		public void AddIncomeProducingProp_Click(Object Source, EventArgs E)
		{
			// Create Connection String
			string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);
			
			// Create SqlCommand Object
			SqlCommand objCommand = new SqlCommand("sp_insert_income_producing_prop", objConnection);
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

			// Add "PropertyType" Parameter
			objParam = objCommand.Parameters.Add("@PropertyType", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtPropertyType.Text);

			// Add "RealEstateDescription" Parameter
			objParam = objCommand.Parameters.Add("@RealEstateDescription", SqlDbType.VarChar, 500); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtRealEstateDescription.Text);

			if (CheckTextBox(txtImprovementDescription))
			{
				// Add "ImprovementDescription" Parameter
				objParam = objCommand.Parameters.Add("@ImprovementDescription", SqlDbType.VarChar, 500); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtImprovementDescription.Text);
			}

			if (CheckTextBox(txtPersonalPropertyIncluded))
			{
				// Add "PersonalPropertyIncluded" Parameter
				objParam = objCommand.Parameters.Add("@PersonalPropertyIncluded", SqlDbType.VarChar, 250); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtPersonalPropertyIncluded.Text);
			}

			// Add "ScheduleOfRent" Parameter
			objParam = objCommand.Parameters.Add("@ScheduleOfRent", SqlDbType.VarChar, 500); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtScheduleOfRent.Text);

			// Add "ManagementAvailableYesNo" Parameter
			objParam = objCommand.Parameters.Add("@ManagementAvailableYesNo", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = chbManagementAvailableYesNo.Checked;

			if (CheckTextBox(txtManagementDescription))
			{
				// Add "ManagementDescription" Parameter
				objParam = objCommand.Parameters.Add("@ManagementDescription", SqlDbType.VarChar, 500); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtManagementDescription.Text);
			}

			// Add "DomWaterInfo" Parameter
			objParam = objCommand.Parameters.Add("@DomWaterInfo", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtDomWaterInfo.Text);

			// Add "IrrWaterInfo" Parameter
			objParam = objCommand.Parameters.Add("@IrrWaterInfo", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtIrrWaterInfo.Text);

			// Add "UtilityInfo" Parameter
			objParam = objCommand.Parameters.Add("@UtilityInfo", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtUtilityInfo.Text);

			// Add "SewerSepticInfo" Parameter
			objParam = objCommand.Parameters.Add("@SewerSepticInfo", SqlDbType.VarChar, 250); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtSewerSepticInfo.Text);

			if (CheckTextBox(txtGrossScheduledIncomeActual))
			{
				// Add "GrossScheduledIncomeActual" Parameter
				objParam = objCommand.Parameters.Add("@GrossScheduledIncomeActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtGrossScheduledIncomeActual.Text);
			}

			if (CheckTextBox(txtGrossScheduledIncomeProj))
			{
				// Add "GrossScheduledIncomeProj" Parameter
				objParam = objCommand.Parameters.Add("@GrossScheduledIncomeProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtGrossScheduledIncomeProj.Text);
			}

			if (CheckTextBox(txtPlusOtherIncomeActual))
			{
				// Add "PlusOtherIncomeActual" Parameter
				objParam = objCommand.Parameters.Add("@PlusOtherIncomeActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtPlusOtherIncomeActual.Text);
			}

			if (CheckTextBox(txtPlusOtherIncomeProj))
			{
				// Add "PlusOtherIncomeProj" Parameter
				objParam = objCommand.Parameters.Add("@PlusOtherIncomeProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtPlusOtherIncomeProj.Text);
			}

			if (CheckTextBox(txtVacancyCreditLossActual))
			{
				// Add "VacancyCreditLossActual" Parameter
				objParam = objCommand.Parameters.Add("@VacancyCreditLossActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtVacancyCreditLossActual.Text);
			}

			if (CheckTextBox(txtVacancyCreditLossProj))
			{
				// Add "VacancyCreditLossProj" Parameter
				objParam = objCommand.Parameters.Add("@VacancyCreditLossProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtVacancyCreditLossProj.Text);
			}

			if (CheckTextBox(txtOpExpensesAdvertisingActual))
			{
				// Add "OpExpensesAdvertisingActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesAdvertisingActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesAdvertisingActual.Text);
			}

			if (CheckTextBox(txtOpExpensesAdvertisingProj))
			{
				// Add "OpExpensesAdvertisingProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesAdvertisingProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesAdvertisingProj.Text);
			}

			if (CheckTextBox(txtOpExpensesCleanMaintActual))
			{
				// Add "OpExpensesCleanMaintActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesCleanMaintActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesCleanMaintActual.Text);
			}

			if (CheckTextBox(txtOpExpensesCleanMaintProj))
			{
				// Add "OpExpensesCleanMaintProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesCleanMaintProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesCleanMaintProj.Text);
			}

			if (CheckTextBox(txtOpExpensesInsuranceActual))
			{
				// Add "OpExpensesInsuranceActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesInsuranceActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesInsuranceActual.Text);
			}

			if (CheckTextBox(txtOpExpensesInsuranceProj))
			{
				// Add "OpExpensesInsuranceProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesInsuranceProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesInsuranceProj.Text);
			}

			if (CheckTextBox(txtOpExpensesLegProfActual))
			{
				// Add "OpExpensesLegProfActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesLegProfActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesLegProfActual.Text);
			}

			if (CheckTextBox(txtOpExpensesLegProfProj))
			{
				// Add "OpExpensesLegProfProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesLegProfProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesLegProfProj.Text);
			}

			if (CheckTextBox(txtOpExpensesMiscRefActual))
			{
				// Add "OpExpensesMiscRefActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesMiscRefActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesMiscRefActual.Text);
			}

			if (CheckTextBox(txtOpExpensesMiscRefProj))
			{
				// Add "OpExpensesMiscRefProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesMiscRefProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesMiscRefProj.Text);
			}

			if (CheckTextBox(txtOpExpensesSuppliesActual))
			{
				// Add "OpExpensesSuppliesActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesSuppliesActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesSuppliesActual.Text);
			}

			if (CheckTextBox(txtOpExpensesSuppliesProj))
			{
				// Add "OpExpensesSuppliesProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesSuppliesProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesSuppliesProj.Text);
			}

			if (CheckTextBox(txtOpExpensesPropManActual))
			{
				// Add "OpExpensesPropManActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesPropManActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesPropManActual.Text);
			}

			if (CheckTextBox(txtOpExpensesPropManProj))
			{
				// Add "OpExpensesPropManProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesPropManProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesPropManProj.Text);
			}

			if (CheckTextBox(txtOpExpensesRepairsActual))
			{
				// Add "OpExpensesRepairsActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesRepairsActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesRepairsActual.Text);
			}

			if (CheckTextBox(txtOpExpensesRepairsProj))
			{
				// Add "OpExpensesRepairsProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesRepairsProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesRepairsProj.Text);
			}

			if (CheckTextBox(txtOpExpensesTaxesActual))
			{
				// Add "OpExpensesTaxesActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesTaxesActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesTaxesActual.Text);
			}

			if (CheckTextBox(txtOpExpensesTaxesProj))
			{
				// Add "OpExpensesTaxesProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesTaxesProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesTaxesProj.Text);
			}

			if (CheckTextBox(txtOpExpensesTelCableActual))
			{
				// Add "OpExpensesTelCableActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesTelCableActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesTelCableActual.Text);
			}

			if (CheckTextBox(txtOpExpensesTelCableProj))
			{
				// Add "OpExpensesTelCableProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesTelCableProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesTelCableProj.Text);
			}

			if (CheckTextBox(txtOpExpensesTrashRemActual))
			{
				// Add "OpExpensesTrashRemActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesTrashRemActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesTrashRemActual.Text);
			}

			if (CheckTextBox(txtOpExpensesTrashRemProj))
			{
				// Add "OpExpensesTrashRemProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesTrashRemProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesTrashRemProj.Text);
			}

			if (CheckTextBox(txtOpExpensesUtilGasActual))
			{
				// Add "OpExpensesUtilGasActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesUtilGasActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesUtilGasActual.Text);
			}

			if (CheckTextBox(txtOpExpensesUtilGasProj))
			{
				// Add "OpExpensesUtilGasProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesUtilGasProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesUtilGasProj.Text);
			}

			if (CheckTextBox(txtOpExpensesUtilElecActual))
			{
				// Add "OpExpensesUtilElecActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesUtilElecActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesUtilElecActual.Text);
			}

			if (CheckTextBox(txtOpExpensesUtilElecProj))
			{
				// Add "OpExpensesUtilElecProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesUtilElecProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesUtilElecProj.Text);
			}

			if (CheckTextBox(txtOpExpensesUtilSewerActual))
			{
				// Add "OpExpensesUtilSewerActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesUtilSewerActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesUtilSewerActual.Text);
			}

			if (CheckTextBox(txtOpExpensesUtilSewerProj))
			{
				// Add "OpExpensesUtilSewerProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesUtilSewerProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesUtilSewerProj.Text);
			}

			if (CheckTextBox(txtOpExpensesUtilIrrWaterActual))
			{
				// Add "OpExpensesUtilIrrWaterActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesUtilIrrWaterActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesUtilIrrWaterActual.Text);
			}

			if (CheckTextBox(txtOpExpensesUtilIrrWaterProj))
			{
				// Add "OpExpensesUtilIrrWaterProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesUtilIrrWaterProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesUtilIrrWaterProj.Text);
			}

			if (CheckTextBox(txtOpExpensesWagesSalActual))
			{
				// Add "OpExpensesWagesSalActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesWagesSalActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesWagesSalActual.Text);
			}

			if (CheckTextBox(txtOpExpensesWagesSalProj))
			{
				// Add "OpExpensesWagesSalProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesWagesSalProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesWagesSalProj.Text);
			}

			if (CheckTextBox(txtOpExpensesStampsPostActual))
			{
				// Add "OpExpensesStampsPostActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesStampsPostActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesStampsPostActual.Text);
			}

			if (CheckTextBox(txtOpExpensesStampsPostProj))
			{
				// Add "OpExpensesStampsPostProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesStampsPostProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesStampsPostProj.Text);
			}

			if (CheckTextBox(txtOpExpensesBankChargesActual))
			{
				// Add "OpExpensesBankChargesActual" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesBankChargesActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesBankChargesActual.Text);
			}

			if (CheckTextBox(txtOpExpensesBankChargesProj))
			{
				// Add "OpExpensesBankChargesProj" Parameter
				objParam = objCommand.Parameters.Add("@OpExpensesBankChargesProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtOpExpensesBankChargesProj.Text);
			}

			if (CheckTextBox(txtTotalAnnualDebtServiceActual))
			{
				// Add "TotalAnnualDebtServiceActual" Parameter
				objParam = objCommand.Parameters.Add("@TotalAnnualDebtServiceActual", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtTotalAnnualDebtServiceActual.Text);
			}

			if (CheckTextBox(txtTotalAnnualDebtServiceProj))
			{
				// Add "TotalAnnualDebtServiceProj" Parameter
				objParam = objCommand.Parameters.Add("@TotalAnnualDebtServiceProj", SqlDbType.Money, 8); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToDouble(txtTotalAnnualDebtServiceProj.Text);
			}

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

			// Check "DisclosuresDescription" TextBox
			if (CheckTextBox(txtDisclosures))
			{
				// Add "DisclosuresDescription" Parameter
				objParam = objCommand.Parameters.Add("@Disclosures", SqlDbType.VarChar, 250); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtDisclosures.Text);
			}

			// Check "MapDirections" TextBox
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
			Response.Redirect("manage_income_producing_prop.aspx");
		}
	}
}
