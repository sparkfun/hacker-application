using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroupAdmin
{
	public class AddAgent : Main
	{
		public PlaceHolder leftPanel;
		public TextBox txtFirstName;
		public TextBox txtLastName;
		public TextBox txtOfficeTelephone;
		public TextBox txtHomeTelephone;
		public TextBox txtTollFreeTelephone;
		public TextBox txtFax;
		public TextBox txtEmail;
		public TextBox txtWebsiteURL;
		public Label lblStatus;

		public void Page_Load(Object Source, EventArgs E)
		{
			if (Page.Controls.Contains(leftPanel))
			{
				Control myLeftPanelControl = LoadControl("includes/leftpanel.ascx");
				leftPanel.Controls.Add(myLeftPanelControl);
			}
		}

		public void AddAgent_Click(Object Source, EventArgs E)
		{
			// Create Connection String
			string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);

			SqlCommand objCommand = new SqlCommand("sp_insert_agent", objConnection);
			objCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter objParam;

			//Add "FirstName" Parameter
			objParam = objCommand.Parameters.Add("@FirstName", SqlDbType.VarChar, 30); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtFirstName.Text);

			//Add "LastName" Parameter
			objParam = objCommand.Parameters.Add("@LastName", SqlDbType.VarChar, 30); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtLastName.Text);

			//Add "OfficeTelephone" Parameter
			objParam = objCommand.Parameters.Add("@OfficeTelephone", SqlDbType.VarChar, 14); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtOfficeTelephone.Text);

			//Add "HomeTelephone" Parameter
			objParam = objCommand.Parameters.Add("@HomeTelephone", SqlDbType.VarChar, 14); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtHomeTelephone.Text);

			//Add "TollFreeTelephone" Parameter
			objParam = objCommand.Parameters.Add("@TollFreeTelephone", SqlDbType.VarChar, 14); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtTollFreeTelephone.Text);

			//Add "Fax" Parameter
			objParam = objCommand.Parameters.Add("@Fax", SqlDbType.VarChar, 14); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtFax.Text);

			//Add "Email" Parameter
			objParam = objCommand.Parameters.Add("@Email", SqlDbType.VarChar, 50); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtEmail.Text);

			//Add "WebsiteURL" Parameter
			objParam = objCommand.Parameters.Add("@WebsiteURL", SqlDbType.VarChar, 100); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtWebsiteURL.Text);

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
