using System;
using System.Data;
using System.Data.SqlClient;

namespace ChrisEdwardsGroup.Website
{
	/// <summary>
	/// Summary description for FeaturedProperty.
	/// </summary>
	public class FeaturedProperty : UserControlMain
	{
		public FeaturedPropertyControl featuredProp;

		public void Page_Load(Object sender, EventArgs e)
		{
			// If page is not being posted back
			if (!Page.IsPostBack)
			{
				// Bind the page controls
				Bind();
			}
		}

		private void Bind()
		{
			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(GetDbConnectionString());
			
			// Create Command Object
			SqlCommand objCommand = new SqlCommand("sp_select_featured_properties", objConnection);
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create SqlDataAdapter Object
			SqlDataAdapter sda = new SqlDataAdapter(objCommand);

			// Create DataSet Object
			DataSet ds = new DataSet();

			// Fill DataSet
			sda.Fill(ds, "FeaturedProp");

			// Create DataView and bind default view from table
			DataView dv = new DataView();
			dv = ds.Tables["FeaturedProp"].DefaultView;

			// Create integer to hold the selected 
			// row value
			int selectedRow = 0;

			// Check to see if more than one featured property exists
			if (dv.Count > 1)
			{
				// Random gets a random number based
				// on the total number of rows in DataView
				Random rand = new Random();
				selectedRow = rand.Next(dv.Count);
			}

			// Set featured property values
			featuredProp.Mls = dv[selectedRow]["MLS"].ToString();
			featuredProp.PicturePath = dv[selectedRow]["PicturePath"].ToString();
			featuredProp.Price = Convert.ToDouble(dv[selectedRow]["Price"]);
			featuredProp.City = dv[selectedRow]["City"].ToString();
		}
	}
}