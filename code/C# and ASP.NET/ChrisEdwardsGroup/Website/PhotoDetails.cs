using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroup.Website
{
	/// <summary>
	/// Summary description for PhotoDetails.
	/// </summary>
	public class PhotoDetails : Main
	{
		public Label lblOutError;
		public Literal ltlMLS;
		public Image imgPicturePathFull;
		public Label lblPictureComments;

		public void Page_Load(Object sender, EventArgs e)
		{
			Bind();
		}

		private void Bind()
		{
			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(GetDbConnectionString());
			
			// Create Command Object
			SqlCommand objCommand = new SqlCommand("sp_select_picture_web", objConnection);
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create SqlDataAdapter Object
			SqlDataAdapter sda = new SqlDataAdapter();

			// Create DataSet Object
			DataSet ds = new DataSet();
			
			// Create SqlParameter Object
			SqlParameter objParam;

			// Add "PictureID" Parameter
			objParam = objCommand.Parameters.Add("@PictureID", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(Request.QueryString["PictureID"]);

			try
			{
				sda.SelectCommand = objCommand;
				sda.Fill(ds, "Picture");
			}
			catch (Exception objError)
			{
				// display error details
				lblOutError.Visible = true;
				lblOutError.Text = "<b>* Error while accessing data</b>.<br />"
					+ objError.Message + "<br />" + objError.Source;
				return;  //  and stop execution
			}

			// Set Literal Text Property to MLS Number
			ltlMLS.Text = ds.Tables["Picture"].Rows[0]["MLS"].ToString();

			// Set ImageUrl Property to Image Path
			imgPicturePathFull.ImageUrl = "/listings/" +
				ds.Tables["Picture"].Rows[0]["MLS"].ToString() + "/" +
				ds.Tables["Picture"].Rows[0]["PicturePathFull"].ToString();

			// Set AlternateText Property to Picture Comments
			imgPicturePathFull.AlternateText = ds.Tables["Picture"].Rows[0]["PictureComments"].ToString();

			// Set Label Text Property to Picture Comments
			lblPictureComments.Text = ds.Tables["Picture"].Rows[0]["PictureComments"].ToString();
		}
	}
}