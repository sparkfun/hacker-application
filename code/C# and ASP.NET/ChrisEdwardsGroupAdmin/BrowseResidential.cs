using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroupAdmin
{
	/// <summary>
	/// Summary description for BrowseResidential.
	/// </summary>
	public class BrowseResidential : Main
	{
		public Label lblRecordCount;
		public Label lblOutError;
		public DataGrid dtgBrowse;

		public BrowseResidential()
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

			// create Command object
			SqlCommand objCommand = new SqlCommand();
			objCommand.Connection = objConnection;
			objCommand.CommandType = CommandType.StoredProcedure;
			objCommand.CommandText = "sp_select_residential_browse";

			// create SqlDataAdapter
			SqlDataAdapter sda = new SqlDataAdapter(objCommand);

			// create DataSet
			DataSet ds = new DataSet();

			try
			{
				sda.Fill(ds, "Residential");
			}
			catch (Exception objError)
			{
				// display error details
				lblOutError.Visible = true;
				lblOutError.Text = "<b>* Error while accessing data</b>.<br />"
					+ objError.Message + "<br />" + objError.Source;
				return;  //  and stop execution
			}

			// Set Record Count Label
			lblRecordCount.Text = "Found " + 
				ds.Tables["Residential"].Rows.Count.ToString() + " records.";

			// Set DataGrid DataSource
			// DataBind() DataGrid
			dtgBrowse.DataSource = ds.Tables["Residential"].DefaultView;
			dtgBrowse.DataBind();
		}

		public void BrowseResidential_ItemCreated(Object sender, DataGridItemEventArgs e)
		{
			// Get the type of item being created
			ListItemType elemType = e.Item.ItemType;

			// Check for Pager Item Type
			if (elemType == ListItemType.Pager)
			{
				TableCell pager = (TableCell)e.Item.Controls[0];

				for (int i = 0; i < pager.Controls.Count; i += 2)
				{
					Object o = pager.Controls[i];
					if (o is LinkButton)
					{
						LinkButton h = (LinkButton)o;
						h.ToolTip = "Click to see page " + h.Text + " of properties.";
						h.Text = "[ " + h.Text + " ]";
					}
					else
					{
						Label l = (Label)o;
						l.Text = "Page " + l.Text;
					}
				}
			}
		}


		public void ItemCommand_OnClick(Object sender, DataGridCommandEventArgs e)
		{
			if (e.CommandName == "editinfo")
			{
				int mlsID;
				mlsID = (int)dtgBrowse.DataKeys[e.Item.ItemIndex];

				Session.Add("mlsID", mlsID);

				string managementPage;
				managementPage = e.Item.Cells[1].Text.ToString();
				
				Response.Redirect(managementPage, true);
			}
		}

		protected void PageIndexChanged_OnClick(Object sender, DataGridPageChangedEventArgs e)
		{
			dtgBrowse.CurrentPageIndex = e.NewPageIndex;
			Bind();
		}
	}
}
