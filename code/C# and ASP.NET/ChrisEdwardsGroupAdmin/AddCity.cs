using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroupAdmin
{
	public class AddCity : Main
	{
		public PlaceHolder leftPanel;
		public TextBox txtCityName;
		public TextBox txtGroupID;
		public TextBox txtZipCode;
		public Label lblStatus;

		public void Page_Load(Object Source, EventArgs E)
		{
			if (Page.Controls.Contains(leftPanel))
			{
				Control myLeftPanelControl = LoadControl("includes/leftpanel.ascx");
				leftPanel.Controls.Add(myLeftPanelControl);
			}
		}

		public void AddCity_Click(Object Source, EventArgs E)
		{
			// Create Connection String
			string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);

			SqlCommand objCommand = new SqlCommand("sp_insert_city", objConnection);
			objCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter objParam;

			//Add "CityName" Parameter
			objParam = objCommand.Parameters.Add("@CityName", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtCityName.Text);

			//Add "GroupID" Parameter
			objParam = objCommand.Parameters.Add("@GroupID", SqlDbType.SmallInt); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt16(txtGroupID.Text);

			//Add "ZipCode" Parameter
			objParam = objCommand.Parameters.Add("@ZipCode", SqlDbType.Char, 10); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtZipCode.Text);

			try
			{
				objConnection.Open();
				objCommand.ExecuteNonQuery();
				objConnection.Close();
		
				lblStatus.Visible = true;
				lblStatus.Text = "Database updated sucessfully!";
			}
			catch (Exception objError)
			{
				lblStatus.Visible = true;
				lblStatus.Text = "Error while accessing data: " + objError.Message
					+ "<br />" + objError.Source;
				return;
			}

			lblStatus.Visible =	true;
			lblStatus.Text = "Database updated sucessfully!";
		}
	}
}