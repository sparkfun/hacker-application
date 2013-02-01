using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SAImageGen_Interop;

namespace ChrisEdwardsGroup.ListManage
{
	/// <summary>
	/// Summary description for ManagePhotos.
	/// </summary>
	public class ManagePictures : Main
	{
		public Label lblMLS;
		public Label lblTotalPics;
		public Label lblOutError;
		public HtmlInputFile iptFile;
		public DataList dtlManagePictures;

		public void Page_Load(Object sender, EventArgs e)
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
			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(GetDbConnectionString());

			// Create Command object
			SqlCommand objCommand = new SqlCommand("sp_select_pictures", objConnection);
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
			sda.Fill(ds, "ManagePictures");

			// DataBind() DataList
			dtlManagePictures.DataSource = ds.Tables["ManagePictures"].DefaultView;
			dtlManagePictures.DataBind();

			// Bind Total Pics Label
			lblTotalPics.Text = ds.Tables["ManagePictures"].Rows.Count.ToString();
		}

		public void ManagePics_ItemCreated(Object sender, DataListItemEventArgs e)
		{
			ListItemType lit = e.Item.ItemType;

			string picturePath;
			picturePath = "../listings/" + Session["mlsID"].ToString() + "/";

			if (lit != ListItemType.EditItem)
			{
				// Get the item data as a DataRowView object
				DataRowView drv = (DataRowView)e.Item.DataItem;

				if (drv != null)
				{
					// Retrieve "PicturePathThumb" Image
					Image imgPicturePathThumb;
					imgPicturePathThumb = (Image)e.Item.FindControl("imgPicturePathThumb");
					// DataBind() "PicturePathThumb Image
					imgPicturePathThumb.ImageUrl = picturePath + drv["PicturePathThumb"].ToString();

					// Retrieve "PictureComments" TableCell
					TableCell tcPictureComments;
					tcPictureComments = (TableCell)e.Item.FindControl("tcPictureComments");
					// DataBind() "PictureComments" TableCell
					tcPictureComments.Text = drv["PictureComments"].ToString();

					// Retrieve "DefaultPicture" TableCell
					TableCell tcDefaultPicture;
					tcDefaultPicture = (TableCell)e.Item.FindControl("tcDefaultPicture");

					string yesNo;
					yesNo = (Convert.ToString(drv["DefaultPicture"]) == "True") ? "Yes" : "No";

					// DataBind() "DefaultPicture" TableCell
					tcDefaultPicture.Text = yesNo;
				}
			}

			if (lit == ListItemType.EditItem)
			{
				// Get the item data as a DataRowView object
				DataRowView drv = (DataRowView)e.Item.DataItem;

				if (drv != null)
				{
					// Retrieve "PicturePathThumb" Image
					Image imgPicturePathThumb;
					imgPicturePathThumb = (Image)e.Item.FindControl("imgPicturePathThumb");
					// DataBind() "PicturePathThumb Image
					imgPicturePathThumb.ImageUrl = picturePath + drv["PicturePathThumb"].ToString();

					// Retrieve "PictureComments" TextBox
					TextBox tbPictureComments;
					tbPictureComments = (TextBox)e.Item.FindControl("txtPictureComments");
					// DataBind() "PictureComments" TextBox
					tbPictureComments.Text = drv["PictureComments"].ToString();

					// Retrieve "DefaultPicture" CheckBox
					CheckBox chbDefaultPicture;
					chbDefaultPicture = (CheckBox)e.Item.FindControl("chbDefaultPicture");

					// Create Boolean to hold "DefaultPicture" value
					bool defaultPicture;
					defaultPicture = Convert.ToBoolean(drv["DefaultPicture"]);

					// DataBind() "DefaultPicture" CheckBox
					chbDefaultPicture.Checked = defaultPicture;
				}
			}
		}

		public void ManagePics_EditCommand(Object sender, DataListCommandEventArgs e)
		{
			// Set the current item to edit mode
			dtlManagePictures.EditItemIndex = e.Item.ItemIndex;

			// Re-Bind the DataGrid
			Bind();
		}

		public void ManagePics_CancelCommand(Object sender, DataListCommandEventArgs e)
		{
			// Reset the edit mode for the current item
			dtlManagePictures.EditItemIndex = -1;

			// Re-Bind the DataGrid
			Bind();
		}

		public void ManagePics_UpdateCommand(Object sender, DataListCommandEventArgs e)
		{
			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(GetDbConnectionString());
			
			// Create Command object
			SqlCommand objCommand = new SqlCommand("sp_update_picture", objConnection);
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create SqlParameter
			SqlParameter objParam;

			// Put PictureID into string
			string pictureID = dtlManagePictures.DataKeys[e.Item.ItemIndex].ToString();

			//Add "PictureID" Parameter
			objParam = objCommand.Parameters.Add("@PictureID", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(pictureID);

			//Add "MLS" Parameter
			objParam = objCommand.Parameters.Add("@MLS", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(Session["mlsID"]);

			// Retrieve PictureComments TextBox
			TextBox tbPictureComments;
			tbPictureComments = (TextBox)e.Item.FindControl("txtPictureComments");
			//Add "PictureComments" Parameter
			objParam = objCommand.Parameters.Add("@PictureComments", SqlDbType.VarChar, 200); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToString(tbPictureComments.Text);

			// Retrieve DefaultPicture CheckBox
			CheckBox chbDefaultPicture;
			chbDefaultPicture = (CheckBox)e.Item.FindControl("chbDefaultPicture");
			//Add "PictureComments" Parameter
			objParam = objCommand.Parameters.Add("@DefaultPicture", SqlDbType.Bit); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = chbDefaultPicture.Checked;

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
			dtlManagePictures.EditItemIndex = -1;

			// Re-Bind the DataGrid
			Bind();
		}

		public void ManagePics_DeleteCommand(Object sender, DataListCommandEventArgs e)
		{
			// Create SqlConnection Object
			SqlConnection objConnection = new SqlConnection(GetDbConnectionString());
			
			// Create Command object
			SqlCommand objCommand = new SqlCommand("sp_select_picturepath", objConnection);
			objCommand.CommandType = CommandType.StoredProcedure;

			// Create SqlParameter
			SqlParameter objParam;

			// Put "PictureID" into string
			string pictureID = dtlManagePictures.DataKeys[e.Item.ItemIndex].ToString();

			//Add "PictureID" Parameter
			objParam = objCommand.Parameters.Add("@PictureID", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(pictureID);

			// create data adapter
			SqlDataAdapter sda = new SqlDataAdapter(objCommand);

			DataSet ds = new DataSet();

			// populate DataSet
			sda.Fill(ds, "PicturePaths");

			string dirPath, removePicturePathThumb, removePicturePathFull;
			dirPath = @"D:\websites\chrisedwardsgroup\listings\" + Session["mlsID"].ToString() + @"\";

			removePicturePathThumb = dirPath + 
				ds.Tables["PicturePaths"].Rows[0]["PicturePathThumb"].ToString();

			removePicturePathFull = dirPath + 
				ds.Tables["PicturePaths"].Rows[0]["PicturePathFull"].ToString();

			// Delete the thumbnail file
			if (File.Exists(removePicturePathThumb))
			{
				File.Delete(removePicturePathThumb);
			}

			// Delete the full-sized file
			if (File.Exists(removePicturePathFull))
			{
				File.Delete(removePicturePathFull);
			}

			objCommand.CommandText = "sp_delete_picture";
			objCommand.CommandType = CommandType.StoredProcedure;
			objCommand.Parameters.Clear();

			//Add "PictureID" Parameter
			objParam = objCommand.Parameters.Add("@PictureID", SqlDbType.Int); 
			objParam.Direction = ParameterDirection.Input;
			objParam.Value = Convert.ToInt32(pictureID);

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

		public void UploadPicture_Click(Object source, EventArgs e)
		{
			string contentType;
			contentType = iptFile.PostedFile.ContentType.ToString();

			if (iptFile.PostedFile == null)
			{
				lblOutError.Visible = true;
				lblOutError.Text = "Error: You must select a file to upload.";
				return;
			}
			else
			{
				if (contentType == "image/pjpeg")
				{
					string dirPath;

					dirPath = @"D:\websites\chrisedwardsgroup\listings\" + Session["mlsID"].ToString() + @"\";

					if (!Directory.Exists(dirPath))
					{
						Directory.CreateDirectory(dirPath);
					}

					string tempFileName, fileName, fileNameThumb, savedFile, savedFileThumb;
					int lastBackSlash;

					// parse out file name
					tempFileName = iptFile.PostedFile.FileName;
					// find last backslash character
					lastBackSlash = tempFileName.LastIndexOf("\\", tempFileName.Length);

					// put file name into variable
					fileName = tempFileName.Remove(0, lastBackSlash + 1);
					// make lowercase
					fileName = fileName.ToLower();
					// remove empty chars
					fileName = fileName.Replace(" ", "_");
					// remove pound character
					fileName = fileName.Replace("#", "");

					// put thumbnail file name into variable
					fileNameThumb = fileName.Replace(".jpg", "_thumb.jpg");

					savedFile = dirPath + fileName;
					savedFileThumb = dirPath + fileNameThumb;

					int i = 1;

					if (File.Exists(savedFile))
					{
						while (File.Exists(savedFile))
						{
							int findPeriod = 0;
							findPeriod = savedFile.LastIndexOf(".", savedFile.Length);

							if (i > 1 && i < 11)
							{
								savedFile = savedFile.Remove(findPeriod - 1, 1);
								savedFile = savedFile.Insert(findPeriod - 1, i.ToString());
							}
							else if (i >= 11 && i < 100)
							{
								savedFile = savedFile.Remove(findPeriod - 2, 2);
								savedFile = savedFile.Insert(findPeriod - 2, i.ToString());
							}
							else
							{
								savedFile = savedFile.Insert(findPeriod, i.ToString());
							}

							// Increment i
							i++;
						}
					}

					i = 1;

					if (File.Exists(savedFileThumb))
					{
						while (File.Exists(savedFileThumb))
						{
							int findPeriod = 0;
							findPeriod = savedFileThumb.LastIndexOf(".", savedFileThumb.Length);

							if (i > 1 && i < 11)
							{
								savedFileThumb = savedFileThumb.Remove(findPeriod - 1, 1);
								savedFileThumb = savedFileThumb.Insert(findPeriod - 1, i.ToString());
							}
							else if (i >= 11 && i < 100)
							{
								savedFileThumb = savedFileThumb.Remove(findPeriod - 2, 2);
								savedFileThumb = savedFileThumb.Insert(findPeriod - 2, i.ToString());
							}
							else
							{
								savedFileThumb = savedFileThumb.Insert(findPeriod, i.ToString());
							}

							// Increment i
							i++;
						}
					}

					try
					{
						iptFile.PostedFile.SaveAs(savedFile);
						iptFile.PostedFile.SaveAs(savedFileThumb);
					}
					catch
					{
						lblOutError.Visible = true;
						lblOutError.Text = "Unable to save files to disk.";
					}

					// parse out file name
					tempFileName = savedFile;
					// find last backslash character
					lastBackSlash = tempFileName.LastIndexOf("\\", tempFileName.Length);

					// put file name into variable
					fileName = tempFileName.Remove(0, lastBackSlash + 1);
					// make lowercase
					fileName = fileName.ToLower();
					// remove empty chars
					fileName = fileName.Replace(" ", "_");
					// remove pound character
					fileName = fileName.Replace("#", "");

					// put thumbnail file name into variable
					fileNameThumb = fileName.Replace(".jpg", "_thumb.jpg");

					// Create SqlConnection Object
					SqlConnection objConnection = new SqlConnection(GetDbConnectionString());

					// Create Command object
					SqlCommand objCommand = new SqlCommand("sp_insert_picture", objConnection);
					objCommand.CommandType = CommandType.StoredProcedure;

					// Create SqlParameter
					SqlParameter objParam;

					// Add "MLS" Parameter
					objParam = objCommand.Parameters.Add("@MLS", SqlDbType.Int); 
					objParam.Direction = ParameterDirection.Input;
					objParam.Value = Convert.ToInt32(Session["mlsID"].ToString());

					// Add "PicturePathThumb" Parameter
					objParam = objCommand.Parameters.Add("@PicturePathThumb", SqlDbType.VarChar, 50); 
					objParam.Direction = ParameterDirection.Input;
					objParam.Value = Convert.ToString(fileNameThumb);

					// Add "PicturePathFull" Parameter
					objParam = objCommand.Parameters.Add("@PicturePathFull", SqlDbType.VarChar, 50); 
					objParam.Direction = ParameterDirection.Input;
					objParam.Value = Convert.ToString(fileName);

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

					try
					{
						ImageGenClass saImgGen = new ImageGenClass();

						saImgGen.LoadImage(savedFileThumb);
						saImgGen.ImageQuality = 80;
						saImgGen.ResizeFilter = 8;
						saImgGen.ResizeImage(186, 141, SAImageGen_Interop.SAImageResizeAlgorithm.saiFilterImage, 0);
						saImgGen.SaveImage(SAImageGen_Interop.SAImageSaveMethod.saiFile, SAImageGen_Interop.SAImageFormat.saiJPG, savedFileThumb);

						saImgGen.LoadImage(savedFile);
						saImgGen.ImageQuality = 80;
						saImgGen.ResizeFilter = 8;
						saImgGen.ResizeImage(360, 270, SAImageGen_Interop.SAImageResizeAlgorithm.saiFilterImage, 0);
						saImgGen.SaveImage(SAImageGen_Interop.SAImageSaveMethod.saiFile, SAImageGen_Interop.SAImageFormat.saiJPG, savedFile);

					}
					catch (Exception exc)
					{
						lblOutError.Visible = true;
						lblOutError.Text = "Error re-sizing file.  Error is: " + exc.Message.ToString();
					}

					Bind();
				}
			}
		}
	}
}