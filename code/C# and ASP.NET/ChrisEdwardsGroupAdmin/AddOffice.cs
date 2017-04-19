using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroupAdmin
{
	/// <summary>
	/// Summary description for AddOffice.
	/// </summary>
	public class AddOffice : Page
	{
		public PlaceHolder leftPanel;
		public Label lblStatus;
		public TextBox txtAddress1;
		public TextBox txtAddress2;
		public TextBox txtCity;
		public TextBox txtState;
		public TextBox txtZipCode;
		public TextBox txtOfficeTel1;
		public TextBox txtOfficeTel2;
		public TextBox txtOfficeTel3;
		public TextBox txtOfficeFax1;
		public TextBox txtOfficeFax2;
		public TextBox txtOfficeFax3;

		public AddOffice()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public void Page_Load(Object Source, EventArgs E)
		{
			if (Page.Controls.Contains(leftPanel))
			{
				Control myLeftPanelControl = LoadControl("includes/leftpanel.ascx");
				leftPanel.Controls.Add(myLeftPanelControl);
			}
		}

		public void AddOffice_Click(Object Source, EventArgs E)
		{
			//create connection object
			string strConnect = "user id=sean;password=gren76b;initial catalog=RMW;";
			SqlConnection objConnection = new SqlConnection(strConnect);

			SqlCommand objCommand = new SqlCommand("sp_insert_office", objConnection);
			objCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter objParam;

			//Add "Address1" Parameter
			objParam = objCommand.Parameters.Add("@Address1", SqlDbType.VarChar, 35); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtAddress1.Text);

			if (txtAddress2.Text != "" && txtAddress2.Text != null)
			{
				//Add "Address2" Parameter
				objParam = objCommand.Parameters.Add("@Address2", SqlDbType.VarChar, 35); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToString(txtAddress2.Text);
			}

			//Add "City" Parameter
			objParam = objCommand.Parameters.Add("@City", SqlDbType.VarChar, 35); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtCity.Text);

			//Add "State" Parameter
			objParam = objCommand.Parameters.Add("@State", SqlDbType.Char, 2); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtState.Text);

			//Add "ZipCode" Parameter
			objParam = objCommand.Parameters.Add("@ZipCode", SqlDbType.VarChar, 10); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtZipCode.Text);

			string officeTelephone, officeFax;

			officeTelephone = "(" + txtOfficeTel1.Text + ")" + " " +
				txtOfficeTel2.Text + "-" + txtOfficeTel3.Text;

			officeFax = "(" + txtOfficeFax1.Text + ")" + " " +
				txtOfficeFax2.Text + "-" + txtOfficeFax3.Text;

			//Add "OfficeTelephone" Parameter
			objParam = objCommand.Parameters.Add("@OfficeTelephone", SqlDbType.Char, 14); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(officeTelephone);

			//Add "OfficeFax" Parameter
			objParam = objCommand.Parameters.Add("@OfficeFax", SqlDbType.Char, 14); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(officeFax);

			try
			{
				objConnection.Open();
				objCommand.ExecuteNonQuery();
				objConnection.Close();
			}
			catch (Exception objError)
			{
				lblStatus.Visible = true;
				lblStatus.Text = "Error while accessing data: " + objError.Message
					+ "<br />" + objError.Source;
				return;
			}

			Server.Transfer("add_office.aspx");
		}
	}
}
