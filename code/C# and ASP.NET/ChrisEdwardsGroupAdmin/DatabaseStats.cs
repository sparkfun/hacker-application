using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroupAdmin
{
	public class DatabaseStats : Main
	{
		public Label lblUser;
		public Label lblOutError;
		public Literal litBusinessNoREQty;
		public Literal litBusinessWithREQty;
		public Literal litCommercialREQty;
		public Literal litFarmRanchQty;
		public Literal litIncomeProducingPropQty;
		public Literal litResidentialQty;
		public Literal litVacantLandQty;
		public Literal litTotalQty;
		public Literal litTotalPicQty;
		public Literal litPicPerListingAvg;

		public void Page_Load(Object Source, EventArgs E)
		{
			if (!Page.IsPostBack)
			{
				Bind();
			}
		}	

		protected void Bind()
		{
			// Create Connection String
			string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);

			// Create SqlCommand Object
			SqlCommand objCommand = new SqlCommand();

			// Stored Procedure "sp_select_business_no_re_qty"
			objCommand.CommandText = "sp_select_business_no_re_qty";
			objCommand.Connection = objConnection;
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create Storage Variable
			int businessNoREQty;

			try
			{
				objConnection.Open();
				businessNoREQty = (int)objCommand.ExecuteScalar();
				objConnection.Close();
				litBusinessNoREQty.Text = businessNoREQty.ToString();
			}
			catch (Exception objError)
			{
				// display error details
				lblOutError.Visible = true;
				lblOutError.Text = "<b>* Error while accessing data</b>.<br />"
					+ objError.Message + "<br />" + objError.Source;
				return;  //  and stop execution
			}

			// Stored Procedure "sp_select_business_with_re_qty"
			objCommand.CommandText = "sp_select_business_with_re_qty";
			objCommand.Connection = objConnection;
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create Storage Variable
			int businessWithREQty;

			try
			{
				objConnection.Open();
				businessWithREQty = (int)objCommand.ExecuteScalar();
				objConnection.Close();
				litBusinessWithREQty.Text = businessWithREQty.ToString();
			}
			catch (Exception objError)
			{
				// display error details
				lblOutError.Visible = true;
				lblOutError.Text = "<b>* Error while accessing data</b>.<br />"
					+ objError.Message + "<br />" + objError.Source;
				return;  //  and stop execution
			}

			// Stored Procedure "sp_select_commercial_re_qty"
			objCommand.CommandText = "sp_select_commercial_re_qty";
			objCommand.Connection = objConnection;
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create Storage Variable
			int commercialREQty;

			try
			{
				objConnection.Open();
				commercialREQty = (int)objCommand.ExecuteScalar();
				objConnection.Close();
				litCommercialREQty.Text = commercialREQty.ToString();
			}
			catch (Exception objError)
			{
				// display error details
				lblOutError.Visible = true;
				lblOutError.Text = "<b>* Error while accessing data</b>.<br />"
					+ objError.Message + "<br />" + objError.Source;
				return;  //  and stop execution
			}

			// Stored Procedure "sp_select_farmranch_qty"
			objCommand.CommandText = "sp_select_farmranch_qty";
			objCommand.Connection = objConnection;
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create Storage Variable
			int farmRanchQty;

			try
			{
				objConnection.Open();
				farmRanchQty = (int)objCommand.ExecuteScalar();
				objConnection.Close();
				litFarmRanchQty.Text = farmRanchQty.ToString();
			}
			catch (Exception objError)
			{
				// display error details
				lblOutError.Visible = true;
				lblOutError.Text = "<b>* Error while accessing data</b>.<br />"
					+ objError.Message + "<br />" + objError.Source;
				return;  //  and stop execution
			}

			// Stored Procedure "sp_select_income_producing_prop_qty"
			objCommand.CommandText = "sp_select_income_producing_prop_qty";
			objCommand.Connection = objConnection;
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create Storage Variable
			int incomeProducingPropQty;

			try
			{
				objConnection.Open();
				incomeProducingPropQty = (int)objCommand.ExecuteScalar();
				objConnection.Close();
				litIncomeProducingPropQty.Text = incomeProducingPropQty.ToString();
			}
			catch (Exception objError)
			{
				// display error details
				lblOutError.Visible = true;
				lblOutError.Text = "<b>* Error while accessing data</b>.<br />"
					+ objError.Message + "<br />" + objError.Source;
				return;  //  and stop execution
			}

			// Stored Procedure "sp_select_residential_qty"
			objCommand.CommandText = "sp_select_residential_qty";
			objCommand.Connection = objConnection;
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create Storage Variable
			int residentialQty;

			try
			{
				objConnection.Open();
				residentialQty = (int)objCommand.ExecuteScalar();
				objConnection.Close();
				litResidentialQty.Text = residentialQty.ToString();
			}
			catch (Exception objError)
			{
				// display error details
				lblOutError.Visible = true;
				lblOutError.Text = "<b>* Error while accessing data</b>.<br />"
					+ objError.Message + "<br />" + objError.Source;
				return;  //  and stop execution
			}

			// Stored Procedure "sp_select_vacant_land_qty"
			objCommand.CommandText = "sp_select_vacant_land_qty";
			objCommand.Connection = objConnection;
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create Storage Variable
			int vacantLandQty;

			try
			{
				objConnection.Open();
				vacantLandQty = (int)objCommand.ExecuteScalar();
				objConnection.Close();
				litVacantLandQty.Text = vacantLandQty.ToString();
			}
			catch (Exception objError)
			{
				// display error details
				lblOutError.Visible = true;
				lblOutError.Text = "<b>* Error while accessing data</b>.<br />"
					+ objError.Message + "<br />" + objError.Source;
				return;  //  and stop execution
			}

			// Calculate Total
			int totalQty = businessNoREQty + businessWithREQty + 
				commercialREQty + farmRanchQty + incomeProducingPropQty + 
				residentialQty + vacantLandQty;
			// DataBind() Total to Literal Control
			litTotalQty.Text = totalQty.ToString();

			// Stored Procedure "sp_select_pictures_qty"
			objCommand.CommandText = "sp_select_pictures_qty";
			objCommand.Connection = objConnection;
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create Storage Variable
			int picturesQty;

			try
			{
				objConnection.Open();
				picturesQty = (int)objCommand.ExecuteScalar();
				objConnection.Close();
				litTotalPicQty.Text = picturesQty.ToString();
			}
			catch (Exception objError)
			{
				// display error details
				lblOutError.Visible = true;
				lblOutError.Text = "<b>* Error while accessing data</b>.<br />"
					+ objError.Message + "<br />" + objError.Source;
				return;  //  and stop execution
			}

			// Calculate Average Number of Pictures Per Listing
			// DataBind() Value to Literal Control
			double dPicPerListingAvg = ((double)picturesQty / (double)totalQty);
			litPicPerListingAvg.Text = dPicPerListingAvg.ToString("n2");
		}
	}
}
