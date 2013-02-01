using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroup.Website
{
	/// <summary>
	/// Browse all of the single family homes.
	/// </summary>
	public class BrowseSingleFamilyHome : Main
	{
		public Label lblStatus;
		public Label lblRecordCount;
		public DataList dtlBrowseSingleFamilyHome;

		public void Page_Load(Object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				// Bind DataList with Database Information
				Bind();
			}
		}

		public void BrowseSingleFamilyHome_ItemCreated(Object sender, DataListItemEventArgs e)
		{
			// Get the item data as a DataRowView object
			DataRowView drv = (DataRowView)e.Item.DataItem;

			if (drv != null)
			{
				// Declare string variable to hold type information
				string getType;

				getType = drv["PicturePathThumb"].GetType().ToString();

				if (getType == "System.DBNull")
				{
					Image imgThumbnail = (Image)e.Item.FindControl("imgThumbnail");
					imgThumbnail.ImageUrl = "/graphics/no_pic.gif";
					imgThumbnail.Height = 135;
					imgThumbnail.Width = 180;
					imgThumbnail.CssClass = "nophoto";
				}
				else
				{
					Image imgThumbnail = (Image)e.Item.FindControl("imgThumbnail");
					imgThumbnail.ImageUrl = "/listings/" + 
						drv["MLS"].ToString() + "/" + 
						drv["PicturePathThumb"].ToString();
					imgThumbnail.AlternateText = "Thumbnail picture of MLS#" + 
						drv["MLS"].ToString() + " located in " +
						drv["CityName"].ToString() + ", NC";
					imgThumbnail.Height = 135;
					imgThumbnail.Width = 180;
					imgThumbnail.CssClass = "browse";
				}
			}
		}

		private void Bind()
		{
			// Create Connection String
			string strConnect = GetDbConnectionString();

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);

			// Create SqlCommand Object
			SqlCommand objCommand = new SqlCommand("sp_select_residential_browse_web", objConnection);
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create SqlDataAdapter Object
			SqlDataAdapter sda = new SqlDataAdapter();

			// Create DataSet Object
			DataSet ds = new DataSet();

			try
			{
				sda.SelectCommand = objCommand;
				sda.Fill(ds, "BrowseSingleFamilyHome");
			}
			catch (SqlException Ex)
			{
				lblStatus.Visible = true;
				lblStatus.Text = GetSqlExceptionDump(Ex);
				return;
			}
			catch (Exception objError)
			{
				// display error details
				lblStatus.Visible = true;
				lblStatus.Text = "<b>* Error executing page source</b>.<br />"
					+ objError.Message + "<br />" + objError.Source;
				return;  //  and stop execution
			}

			DataView dv = new DataView(ds.Tables["BrowseSingleFamilyHome"]);

			int propertyCount = dv.Count;

			if (propertyCount > 0)
			{
				string propertyPlural;
				propertyPlural = (propertyCount > 1) ? " properties" : " property";

				lblRecordCount.Text = "We found " + propertyCount + 
					propertyPlural + " that might be of interest to you.";
			}
			else
			{
				lblRecordCount.Text = "We found 0 properties that match your search parameters.";
			}

			// DataBind Browse Single Family Home DataList
			dtlBrowseSingleFamilyHome.DataSource = dv;
			dtlBrowseSingleFamilyHome.DataBind();
		}
	}
}