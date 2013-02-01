using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroupAdmin
{
	/// <summary>
	/// Summary description for ManageTours.
	/// </summary>
	public class ManageTours : Main
	{
		public Label lblMLS;
		public Label lblTotalTours;
		public Label lblOutError;
		public TextBox txtTourUrl;
		public DataList dtlManageTours;

		public ManageTours()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public void Page_Load(Object source, EventArgs e)
		{

			// Check "mlsID" Session
			// If Null or Empty Redirect to Home Page
			if (Session["mlsID"] == null || Session["mlsID"].ToString() == "")
			{
				Response.Redirect("default.aspx", true);
			}

			if (!Page.IsPostBack)
			{
				lblMLS.Text = Session["mlsID"].ToString();

				Bind();
			}
		}

		private void Bind()
		{
			// Create Connection String
			string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);

			// Create Command Object
			SqlCommand objCommand = new SqlCommand("sp_select_tours", objConnection);
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create SqlParameter
			SqlParameter objParam;

			// Add "MLS" Parameter
			objParam = objCommand.Parameters.Add("@MLS", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(Session["mlsID"].ToString());

			// Create SqlDataAdapter
			SqlDataAdapter sda = new SqlDataAdapter(objCommand);

			// Create DataSet
			DataSet ds = new DataSet();

			// Populate DataSet
			sda.Fill(ds, "ManageTours");

			// Bind Total Tours Label
			lblTotalTours.Text = ds.Tables["ManageTours"].Rows.Count.ToString();

			// DataBind() DataList
			dtlManageTours.DataSource = ds.Tables["ManageTours"].DefaultView;
			dtlManageTours.DataBind();
		}

		public void ManageTours_ItemCreated(Object sender, DataListItemEventArgs e)
		{
			ListItemType lit = e.Item.ItemType;

			if (lit != ListItemType.EditItem)
			{
				// Get the item data as a DataRowView object
				DataRowView drv = (DataRowView)e.Item.DataItem;

				if (drv != null)
				{
					// Retrieve "hlTourUrl" HyperLink
					HyperLink hlTourUrl;
					hlTourUrl = (HyperLink)e.Item.FindControl("hlTourUrl");

					// DataBind() "hlTourUrl" HyperLink
					hlTourUrl.NavigateUrl = drv["TourUrl"].ToString();
					hlTourUrl.Text = drv["TourUrl"].ToString();
				}
			}

			if (lit == ListItemType.EditItem)
			{
				// Get the item data as a DataRowView object
				DataRowView drv = (DataRowView)e.Item.DataItem;

				if (drv != null)
				{
					// Retrieve "txtTourUrl" TextBox
					TextBox txtTourUrl;
					txtTourUrl = (TextBox)e.Item.FindControl("txtTourUrl");
					// DataBind() "txtTourUrl" TextBox
					txtTourUrl.Text = drv["TourUrl"].ToString();

					// Retrieve "ddlTourProvider" DropDownList
					DropDownList ddlTourProvider;
					ddlTourProvider = (DropDownList)e.Item.FindControl("ddlTourProvider");
				}
			}
		}

		public void ManageTours_EditCommand(Object sender, DataListCommandEventArgs e)
		{
			// Set the current item to edit mode
			dtlManageTours.EditItemIndex = e.Item.ItemIndex;

			// Re-Bind the DataGrid
			Bind();
		}

		public void ManageTours_CancelCommand(Object sender, DataListCommandEventArgs e)
		{
			// Reset the edit mode for the current item
			dtlManageTours.EditItemIndex = -1;

			// Re-Bind the DataGrid
			Bind();
		}

		public void ManageTours_UpdateCommand(Object sender, DataListCommandEventArgs e)
		{
			// Create Connection String
			string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);
			
			// Create Command object
			SqlCommand objCommand = new SqlCommand("sp_update_tour", objConnection);
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create SqlParameter
			SqlParameter objParam;

			// Put tourID into string
			string tourID = dtlManageTours.DataKeys[e.Item.ItemIndex].ToString();

			// Add "TourID" Parameter
			objParam = objCommand.Parameters.Add("@TourID", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(tourID);

			// Retrieve "txtTourUrl" TextBox
			TextBox txtTourUrl;
			txtTourUrl = (TextBox)e.Item.FindControl("txtTourUrl");

			// Add "TourUrl" Parameter
			objParam = objCommand.Parameters.Add("@TourUrl", SqlDbType.VarChar, 80); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtTourUrl.Text);

			try
			{
				objConnection.Open();
				objCommand.ExecuteNonQuery();
				objConnection.Close();
			}
			catch (Exception objError)
			{
				lblOutError.Visible = true;
				lblOutError.Text = "Error while accessing data: " + objError.Message
					+ "<br />" + objError.Source;
				return;
			}

			// Reset the edit mode for the current item
			dtlManageTours.EditItemIndex = -1;

			// Re-Bind the DataGrid
			Bind();
		}

		public void ManageTours_DeleteCommand(Object sender, DataListCommandEventArgs e)
		{
			// Create Connection String
			string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);
			
			// Create Command object
			SqlCommand objCommand = new SqlCommand("sp_delete_tour", objConnection);
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create SqlParameter
			SqlParameter objParam;

			// Put "tourID" into string
			string tourID = dtlManageTours.DataKeys[e.Item.ItemIndex].ToString();

			// Add "TourID" Parameter
			objParam = objCommand.Parameters.Add("@TourID", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(tourID);

			try
			{
				objConnection.Open();
				objCommand.ExecuteNonQuery();
				objConnection.Close();
			}
			catch (Exception objError)
			{
				lblOutError.Visible = true;
				lblOutError.Text = "Error while accessing data: " + objError.Message
					+ "<br />" + objError.Source;
				return;
			}

			// Re-Bind the DataGrid
			Bind();
		}

		public void AddTourUrl_Click(Object sender, EventArgs e)
		{
			// Create Connection String
			string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);
			
			// Create Command object
			SqlCommand objCommand = new SqlCommand("sp_add_tour", objConnection);
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create SqlParameter
			SqlParameter objParam;

			// Add "MLS" Parameter
			objParam = objCommand.Parameters.Add("@MLS", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(Session["mlsID"].ToString());

			// Add "TourUrl" Parameter
			objParam = objCommand.Parameters.Add("@TourUrl", SqlDbType.VarChar, 80); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(txtTourUrl.Text);

			try
			{
				objConnection.Open();
				objCommand.ExecuteNonQuery();
				objConnection.Close();
			}
			catch (Exception objError)
			{
				lblOutError.Visible = true;
				lblOutError.Text = "Error while accessing data: " + objError.Message
					+ "<br />" + objError.Source;
				return;
			}

			// Re-Bind the DataGrid
			Bind();

		}
	}
}
