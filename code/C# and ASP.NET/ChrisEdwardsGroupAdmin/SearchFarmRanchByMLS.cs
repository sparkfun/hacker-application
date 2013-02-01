using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace ChrisEdwardsGroupAdmin
{
	/// <summary>
	/// Summary description for SearchFarmRanchByMLS.
	/// </summary>
	public class SearchFarmRanchByMLS : Main
	{
		public HtmlGenericControl hgcErrors;
		public TextBox txtMLS;
		public TextBox txtAltMLS;
		public Table tblRecordCount;
		public TableCell tcRecordCount;
		public DataGrid dtgSearchByMLS;

		public SearchFarmRanchByMLS()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public void Page_Load(Object sender, EventArgs e)
		{

		}

		public void SearchByMLS_ItemCreated(Object sender, DataGridItemEventArgs e)
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
				mlsID = (int)dtgSearchByMLS.DataKeys[e.Item.ItemIndex];

				Session.Add("mlsID", mlsID);

				string managementPage;
				managementPage = e.Item.Cells[1].Text.ToString();
				
				Response.Redirect(managementPage, true);
			}
		}

		public void SearchByMLS_Click(Object sender, EventArgs e)
		{
			// Create Connection String
			string strConnect = System.Configuration.ConfigurationSettings.AppSettings["dbConnectString"];

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);
			
			// Create SqlCommand Object
			SqlCommand objCommand = new SqlCommand("sp_select_farm_ranch_search_by_mls", objConnection);
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create SqlDataAdapter Object
			SqlDataAdapter sda = new SqlDataAdapter();

			// Create DataSet Object
			DataSet ds = new DataSet();

			// Create SqlParameter Object
			SqlParameter objParam;

			// Add "MLS" Parameter
			objParam = objCommand.Parameters.Add("@MLS", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(txtMLS.Text);

			// Check "AltMLS" TextBox
			if (CheckTextBox(txtAltMLS))
			{
				// Add "AltMLS" Parameter
				objParam = objCommand.Parameters.Add("@AltMLS", SqlDbType.Int); 
				objParam.Direction = ParameterDirection.Input;
				objParam.Value = Convert.ToInt32(txtAltMLS.Text);
			}

			// Add Database Record
			// Return RecordSet
			// Catch and Report Any Errors
			try
			{
				sda.SelectCommand = objCommand;
				sda.Fill(ds, "FarmRanch");
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

			DataView dv = new DataView(ds.Tables["FarmRanch"]);

			int propertyCount = (int)dv.Count;

			if (propertyCount > 0)
			{
				string propertyPlural, matchPlural;
				propertyPlural = (propertyCount > 1) ? " properties" : " property";
				matchPlural = (propertyCount > 1) ? " match" : " matches";


				tblRecordCount.Visible = true;
				tcRecordCount.Text = "We found " + propertyCount + 
					propertyPlural + " that" + matchPlural + " your search parameters.";
			}
			else
			{
				tblRecordCount.Visible = true;
				tcRecordCount.Text = "We found 0 properties that match your search parameters.";
			}

			// Make Search Table Visible
			dtgSearchByMLS.Visible = true;

			// DataBind "Residential Search" DataGrid
			dtgSearchByMLS.DataSource = dv;
			dtgSearchByMLS.DataBind();
		}	
	}
}
