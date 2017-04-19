using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroup.Website
{
	/// <summary>
	/// Display single family home listing.
	/// </summary>
	public class SingleFamilyHome : Main
	{
		public Label lblOutError;
		public Repeater rprSingleFamilyHome;

		public void Page_Load(Object sender, EventArgs e)
		{
			// Bind Data to Page
			Bind();
		}

		private void Bind()
		{
			// Create Connection String
			string strConnect = GetDbConnectionString();

			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(strConnect);
			
			// Create Command Object
			SqlCommand objCommand = new SqlCommand("sp_select_residential_long_web", objConnection);
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
			objParam.Value = Convert.ToInt32(Request.QueryString["MLSID"]);

			try
			{
				sda.SelectCommand = objCommand;
				sda.Fill(ds, "SingleFamilyHome");
			}
			catch (Exception objError)
			{
				// display error details
				lblOutError.Visible = true;
				lblOutError.Text = "<b>* Error while accessing data</b>.<br />"
					+ objError.Message + "<br />" + objError.Source;
				return;  //  and stop execution
			}

			// DataBind() Residential Repeater Control
			rprSingleFamilyHome.DataSource = ds.Tables["SingleFamilyHome"].DefaultView;
			rprSingleFamilyHome.DataBind();
		}

		public void SingleFamilyHome_ItemDataBound(Object Sender, RepeaterItemEventArgs e) 
		{
			// Get the type of item being created
			ListItemType elemType = e.Item.ItemType;

			// Check for Item Type
			if (elemType == ListItemType.Item)
			{
				// Get the item data as a DataRowView object
				DataRowView drv = (DataRowView)e.Item.DataItem;

				if (drv != null)
				{
					// Display the MLS/Alternate MLS correctly by
					// determinining if an alternate MLS exists
					// and displaying both the main and alternate
					// or just the main number

					// Get the "MLS" field as an integer
					int mls = Convert.ToInt32(drv["MLS"]);

					// Get the type of the "AltMLS" field
					string typeAltMlsId = drv["AltMLS"].GetType().ToString();

					// If the "AltMLS" is null display just the main
					// MLS number
					// Otherwise display both numbers
					if (typeAltMlsId == "System.DBNull")
					{
						Literal ltlMLS = (Literal)e.Item.FindControl("ltlMLS");
						ltlMLS.Text = mls.ToString();
					}
					else
					{
						Literal ltlMLS = (Literal)e.Item.FindControl("ltlMLS");
						ltlMLS.Text = mls.ToString() + "/" +
							drv["AltMLS"].ToString();
					}

					// Display the main photo or a dummy photo if none exists

					// Get the system type of the "PicturePathFull" field
					string getType = drv["PicturePathFull"].GetType().ToString();

					// If the "PicturePathFull" field is null display
					// the dummy photo.
					// Otherwise display the main photo
					if (getType == "System.DBNull")
					{
						Image imgFull = (Image)e.Item.FindControl("imgFull");
						imgFull.ImageUrl = "/graphics/no_pic_full.jpg";
					}
					else
					{
						Image imgFull = (Image)e.Item.FindControl("imgFull");
						imgFull.ImageUrl = "/listings/" + 
							mls.ToString() + "/" + 
							drv["PicturePathFull"].ToString();
						imgFull.BorderWidth = 1;
					}

					// Create DataView to hold virtual tour data
					DataView dv = new DataView();
					dv = CreateVirtualTourDataSource(mls);

					// Check to see if any virtual tour data exists
					if (dv.Count != 0)
					{
						// Instantiate the virtual tour table
						Table virtualTour =  (Table)e.Item.FindControl("virtualTour");
						
						// Make the table visible
						virtualTour.Visible = true;

						// Create new table rows
						TableRow r1 = new TableRow();
						TableRow r2 = new TableRow();
					
						// Create new table cells
						TableCell c1 = new TableCell();
						TableCell c2 = new TableCell();

						// Set the text for cell 1
						c1.Text = "Virtual Tour (click button to view)";
						c1.CssClass = "header";

						// Create new hyperlink control using
						// the virtual tour graphic, the URL and
						// open the link in a blank window
						HyperLink link = new HyperLink();
						link.ImageUrl = "/images/featurelink01.gif";
						link.NavigateUrl = dv[0]["TourUrl"].ToString();

						// Add the hyperlink control to cell 2
						c2.Controls.Add(link);
						
						// Add cell 1 to row 1
						r1.Cells.Add(c1);

						// Add cell 2 to row 2
						r2.Cells.Add(c2);

						// Add row 1 to table
						virtualTour.Rows.Add(r1);

						// Add row 2 to table
						virtualTour.Rows.Add(r2);
					}

					// Get photo gallery data view
					dv = CreatePhotoGalleryDataSource(mls);

					// Check to see if any photo gallery data exists
					if (dv.Count != 0)
					{

						// Instantiate the photo gallery DataList
						DataList photoGallery = (DataList)e.Item.FindControl("photoGallery");

						// DataBind the photo gallery DataList
						photoGallery.DataSource = dv;
						photoGallery.DataBind();
					}
				}
			}
		}

		private DataView CreatePhotoGalleryDataSource(int mls)
		{
			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(GetDbConnectionString());

			// Create SqlCommand Object
			SqlCommand objCommand = new SqlCommand("sp_select_additional_pictures", objConnection);
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create SqlParameter Object
			SqlParameter objParam;

			// Add "MLS" Parameter
			objParam = objCommand.Parameters.Add("@MLS", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = mls;

			// Create SqlDataAdapter Object
			SqlDataAdapter sda = new SqlDataAdapter();

			// Create DataSet Object
			DataSet ds = new DataSet();

			// Fill the DataSet
			sda.SelectCommand = objCommand;
			sda.Fill(ds, "Photos");

			return ds.Tables["Photos"].DefaultView;
		}

		private DataView CreateVirtualTourDataSource(int mls)
		{
			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(GetDbConnectionString());

			// Create SqlCommand Object
			SqlCommand objCommand = new SqlCommand("sp_select_tours", objConnection);
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create SqlParameter Object
			SqlParameter objParam;

			// Add "MLS" Parameter
			objParam = objCommand.Parameters.Add("@MLS", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = mls;

			// Create SqlDataAdapter Object
			SqlDataAdapter sda = new SqlDataAdapter();

			// Create DataSet Object
			DataSet ds = new DataSet();

			// Fill the DataSet
			sda.SelectCommand = objCommand;
			sda.Fill(ds, "VirtualTours");

			return ds.Tables["VirtualTours"].DefaultView;
		}
	}
}