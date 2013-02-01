using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace CopelandRealEstate
{
	/// <summary>
	/// Convert all of the Copeland Real Estate listings from
	/// one database format to another.
	/// </summary>
	public class ConverterForm : Form
	{
		private Label ResultsLabel;
		private Button ConvButton;
		private Label ErrorsLabel;
		private GroupBox ConvOptions;
		private RadioButton ConvertAllOption;
		private RadioButton TestOneOption;
		private ListBox ConvResults;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		public ConverterForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ConvButton = new System.Windows.Forms.Button();
			this.ResultsLabel = new System.Windows.Forms.Label();
			this.ErrorsLabel = new System.Windows.Forms.Label();
			this.ConvOptions = new System.Windows.Forms.GroupBox();
			this.ConvertAllOption = new System.Windows.Forms.RadioButton();
			this.TestOneOption = new System.Windows.Forms.RadioButton();
			this.ConvResults = new System.Windows.Forms.ListBox();
			this.ConvOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// ConvButton
			// 
			this.ConvButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ConvButton.Location = new System.Drawing.Point(8, 16);
			this.ConvButton.Name = "ConvButton";
			this.ConvButton.Size = new System.Drawing.Size(100, 23);
			this.ConvButton.TabIndex = 0;
			this.ConvButton.Text = "Run Conversion";
			this.ConvButton.Click += new System.EventHandler(this.ConvButton_Click);
			// 
			// ResultsLabel
			// 
			this.ResultsLabel.Location = new System.Drawing.Point(8, 50);
			this.ResultsLabel.Name = "ResultsLabel";
			this.ResultsLabel.Size = new System.Drawing.Size(125, 15);
			this.ResultsLabel.TabIndex = 2;
			this.ResultsLabel.Text = "Conversion Results";
			// 
			// ErrorsLabel
			// 
			this.ErrorsLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ErrorsLabel.Location = new System.Drawing.Point(136, 16);
			this.ErrorsLabel.Name = "ErrorsLabel";
			this.ErrorsLabel.Size = new System.Drawing.Size(272, 30);
			this.ErrorsLabel.TabIndex = 3;
			this.ErrorsLabel.Visible = false;
			// 
			// ConvOptions
			// 
			this.ConvOptions.Controls.Add(this.ConvertAllOption);
			this.ConvOptions.Controls.Add(this.TestOneOption);
			this.ConvOptions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ConvOptions.Location = new System.Drawing.Point(16, 296);
			this.ConvOptions.Name = "ConvOptions";
			this.ConvOptions.Size = new System.Drawing.Size(400, 64);
			this.ConvOptions.TabIndex = 4;
			this.ConvOptions.TabStop = false;
			this.ConvOptions.Text = "Conversion Options";
			// 
			// ConvertAllOption
			// 
			this.ConvertAllOption.Location = new System.Drawing.Point(184, 24);
			this.ConvertAllOption.Name = "ConvertAllOption";
			this.ConvertAllOption.Size = new System.Drawing.Size(125, 30);
			this.ConvertAllOption.TabIndex = 1;
			this.ConvertAllOption.Text = "Convert All Records";
			// 
			// TestOneOption
			// 
			this.TestOneOption.Checked = true;
			this.TestOneOption.Location = new System.Drawing.Point(16, 24);
			this.TestOneOption.Name = "TestOneOption";
			this.TestOneOption.Size = new System.Drawing.Size(110, 30);
			this.TestOneOption.TabIndex = 0;
			this.TestOneOption.TabStop = true;
			this.TestOneOption.Text = "Test One Record";
			// 
			// ConvResults
			// 
			this.ConvResults.ColumnWidth = 340;
			this.ConvResults.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ConvResults.HorizontalScrollbar = true;
			this.ConvResults.Location = new System.Drawing.Point(16, 72);
			this.ConvResults.Name = "ConvResults";
			this.ConvResults.Size = new System.Drawing.Size(400, 212);
			this.ConvResults.TabIndex = 3;
			// 
			// ConverterForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(432, 374);
			this.Controls.Add(this.ConvOptions);
			this.Controls.Add(this.ErrorsLabel);
			this.Controls.Add(this.ResultsLabel);
			this.Controls.Add(this.ConvButton);
			this.Controls.Add(this.ConvResults);
			this.Name = "ConverterForm";
			this.Text = "Database Converter - Copeland Real Estate";
			this.ConvOptions.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			Application.Run(new ConverterForm());
		}

		private const string CopelandDBString =
			"Data Source=HOMESTAR;user id=copeland;password=G4rYiW0OI;Initial Catalog=CopelandRealEstate;";

		private const string OldDBString =
			"Data Source=HOMESTAR;user id=copeland;password=G4rYiW0OI;Initial Catalog=copeland;";

		// Enumeration of Listing Classes.
		public enum ListingClass
		{
			ResidentialHomes = 1,
			Commercial = 2,
			LotsLand = 3,
			BoatSlipOther = 4,
			RentalHomes = 5
		}
		
		// Enumeration of image sizes.
		protected enum ImageSize : int
		{
			Small = 0,
			Medium = 1,
			Large = 2
		}

		public struct ClassPropertyType
		{
			public ListingClass classID;
			public int propertyTypeID;

			public ClassPropertyType(ListingClass listClass, int pID)
			{
				classID = listClass;
				propertyTypeID = pID;
			}
		}

		public struct PropertyID
		{
			public int oldListingID, newPropertyID;

			public PropertyID(int oldID, int newID)
			{
				oldListingID = oldID;
				newPropertyID = newID;
			}
		}

		protected DataSet GetData()
		{
			// Create SqlConnection Object.
			SqlConnection sqlConnection = new SqlConnection(OldDBString);

			// Create SqlCommand Object and call
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = "GetAllListings";

			// Create SqlDataAdapter Object
			SqlDataAdapter sda = new SqlDataAdapter(sqlCommand);

			// Create DataSet Object
			DataSet ds = new DataSet();

			// Fill DataSet.
			// Catch and display any errors
			try
			{
				sda.Fill(ds);
			}
			catch (SqlException sqlEx)
			{
				// Display error.
				ErrorsLabel.Visible = true;
				ErrorsLabel.Text = sqlEx.Message;
			}
			catch (Exception ex)
			{
				// Display error.
				ErrorsLabel.Visible = true;
				ErrorsLabel.Text = ex.Message;
			}

			// Return DataSet.
			return ds;
		}
		
		protected DataSet GetPhotoData(int listingID)
		{
			// Create SqlConnection Object.
			SqlConnection sqlConnection = new SqlConnection(OldDBString);

			// Create SqlCommand Object and call
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = "GetPhotoData";

			// Create SqlDataAdapter Object
			SqlDataAdapter sda = new SqlDataAdapter(sqlCommand);
			
			// Create SqlParameter Object
			SqlParameter sqlParam;

			// Add "listing_id" SQL parameter.
			sqlParam = sqlCommand.Parameters.Add("@listing_id", SqlDbType.Int);
			sqlParam.Direction = ParameterDirection.Input;
			sqlParam.Value = listingID;

			// Create DataSet Object
			DataSet ds = new DataSet();

			// Fill DataSet.
			// Catch and display any errors
			try
			{
				sda.Fill(ds);
			}
			catch (SqlException sqlEx)
			{
				// Display error.
				ErrorsLabel.Visible = true;
				ErrorsLabel.Text = sqlEx.Message;
			}
			catch (Exception ex)
			{
				// Display error.
				ErrorsLabel.Visible = true;
				ErrorsLabel.Text = ex.Message;
			}

			// Return DataSet.
			return ds;
		}

		protected PropertyID InsertRecord(DataRow dr)
		{
			ClassPropertyType cpt;

			DataMatcher matcher = new DataMatcher();
			cpt = matcher.MatchClassListingType(dr["listtype"].ToString());

			// Initialize property ID.
			PropertyID propID;
			propID.newPropertyID = 0;
			propID.oldListingID = 0;

			if (cpt.classID == ListingClass.ResidentialHomes)
			{
				// Create SqlConnection Object
				SqlConnection objConnection = new SqlConnection(CopelandDBString);

				// Create SqlCommand Object
				SqlCommand objCommand = new SqlCommand("adm_InsertResidentialHomesProperty", objConnection);
				objCommand.CommandType = CommandType.StoredProcedure;

				// Create SqlParameter Object
				SqlParameter sqlParam;

				// Add "StatusID" SQL parameter.
				int status = matcher.MatchStatus(dr["status"].ToString());
				sqlParam = objCommand.Parameters.Add("@StatusID", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = status;

				// Add "PropertyTypeID" SQL parameter.
				byte propertyTypeID = Convert.ToByte(cpt.propertyTypeID);
				sqlParam = objCommand.Parameters.Add("@PropertyTypeID", SqlDbType.TinyInt);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = propertyTypeID;

				// Add "PropertyIDOut" value
				sqlParam = objCommand.Parameters.Add("@PropertyIDOut", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Output;

				// Add "MLS1" SQL parameter, if field is filled in.
				if (dr["mls"].ToString() != String.Empty)
				{
					sqlParam = objCommand.Parameters.Add("@MLS1", SqlDbType.VarChar, 20);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = dr["mls"].ToString();
				}

				// Add "Price" SQL parameter.
				decimal price = Convert.ToDecimal(dr["price"]);
				sqlParam = objCommand.Parameters.Add("@Price", SqlDbType.Money);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = price;

				// Add "AgentID1" SQL parameter.
				int agentID1 = matcher.MatchAgent(Convert.ToInt32(dr["agent_id"]));
				sqlParam = objCommand.Parameters.Add("@AgentID1", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = agentID1;

				// Add "Tagline" SQL parameter.
				string tagline = "Click to see property.";
				sqlParam = objCommand.Parameters.Add("@Tagline", SqlDbType.VarChar, 100);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = tagline;

				// Add "Address1" SQL parameter.
				string address1 = Convert.ToString(dr["address"]);
				sqlParam = objCommand.Parameters.Add("@Address1", SqlDbType.VarChar, 100);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = address1;

				// Add "CityID" SQL parameter.
				int cityID = Convert.ToInt32(matcher.MatchCity(dr["city"].ToString()));
				sqlParam = objCommand.Parameters.Add("@CityID", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = cityID;

				// Add "PropertyName" SQL parameter.
				string propertyName = Convert.ToString(dr["propname"].ToString());
				sqlParam = objCommand.Parameters.Add("@PropertyName", SqlDbType.VarChar, 150);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = propertyName;

				// Add "LocationID" SQL parameter.
				int locationID = matcher.MatchLocation(dr["location"].ToString());
				sqlParam = objCommand.Parameters.Add("@LocationID", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = locationID;

				// Add "Bedrooms" SQL parameter.
				int bedrooms = Convert.ToInt32(dr["bedrooms"]);
				sqlParam = objCommand.Parameters.Add("@Bedrooms", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = bedrooms;

				// Add "BathsFull" SQL parameter.
				int bathsFull = Convert.ToInt32(dr["baths_full"]);
				sqlParam = objCommand.Parameters.Add("@BathsFull", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = bathsFull;

				// Add "BathsHalf" SQL parameter.
				int bathsHalf = Convert.ToInt32(dr["baths_half"]);
				sqlParam = objCommand.Parameters.Add("@BathsHalf", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = bathsHalf;

				// Add "YearBuilt" SQL parameter.
				string yearBuilt = Convert.ToString(dr["built"]);
				sqlParam = objCommand.Parameters.Add("@YearBuilt", SqlDbType.Char, 4);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = yearBuilt;

				// Add "EstYearlyTaxes" SQL parameter, if field is filled in.
				if (dr["taxes"].ToString() != String.Empty)
				{
					decimal estYearlyTaxes = Convert.ToDecimal(dr["taxes"]);
					sqlParam = objCommand.Parameters.Add("@EstYearlyTaxes", SqlDbType.Money);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = estYearlyTaxes;
				}

				// Add "HOAFees" SQL parameter, if field is filled in.
				if (dr["hoa"].ToString() != String.Empty)
				{
					decimal hOAFees = Convert.ToDecimal(dr["hoa"]);
					sqlParam = objCommand.Parameters.Add("@HOAFees", SqlDbType.Money);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = hOAFees;
				}

				// Add "HOAPeriod" SQL parameter.
				string hOAPeriod = Convert.ToString(dr["hoa_period"]);
				sqlParam = objCommand.Parameters.Add("@HOAPeriod", SqlDbType.VarChar, 50);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = hOAPeriod;

				// Add "SqFt" SQL parameter.
				int sqFt = Convert.ToInt32(dr["sqft"]);
				sqlParam = objCommand.Parameters.Add("@SqFt", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = sqFt;

				// Add "LotSize" SQL parameter, if field is filled in.
				if (dr["lotsize"].ToString() != String.Empty)
				{
					string lotSize = Convert.ToString(dr["lotsize"]);
					sqlParam = objCommand.Parameters.Add("@LotSize", SqlDbType.VarChar, 50);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = lotSize;
				}

				// Add "HeatingType" SQL parameter.
				string heatingType = Convert.ToString(dr["heating"]);
				if (heatingType == String.Empty || heatingType == null)
				{
					heatingType = "Other";
				}
				sqlParam = objCommand.Parameters.Add("@HeatingType", SqlDbType.VarChar, 150);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = heatingType;

				// Add "CoolingType" SQL parameter.
				string coolingType = Convert.ToString(dr["cooling"]);
				if (coolingType == String.Empty || coolingType == null)
				{
					coolingType = "Other";
				}
				sqlParam = objCommand.Parameters.Add("@CoolingType", SqlDbType.VarChar, 150);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = coolingType;

				// Add "SidingType" SQL parameter.
				string sidingType = Convert.ToString(dr["siding"]);
				if (sidingType == String.Empty || sidingType == null)
				{
					sidingType = "Other";
				}
				sqlParam = objCommand.Parameters.Add("@SidingType", SqlDbType.VarChar, 150);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = sidingType;

				// Add "ElemSchool" SQL parameter, if field is filled in.
				string elemSchool = Convert.ToString(dr["school1"]);
				if (elemSchool == String.Empty || elemSchool == null)
				{
					sqlParam = objCommand.Parameters.Add("@ElemSchool", SqlDbType.VarChar, 100);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = elemSchool;
				}

				// Add "MiddleSchool" SQL parameter, if field is filled in.
				string middleSchool = Convert.ToString(dr["school2"]);
				if (middleSchool == String.Empty || middleSchool == null)
				{
					sqlParam = objCommand.Parameters.Add("@MiddleSchool", SqlDbType.VarChar, 100);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = middleSchool;
				}

				// Add "HighSchool" SQL parameter, if field is filled in.
				string highSchool = Convert.ToString(dr["school3"]);
				if (highSchool == String.Empty || highSchool == null)
				{
					sqlParam = objCommand.Parameters.Add("@HighSchool", SqlDbType.VarChar, 100);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = highSchool;
				}

				// Add "Garage" SQL parameter.
				string garage = Convert.ToString(dr["garage"]);
				if (garage == String.Empty || garage == null)
				{
					garage = "Other";
				}
				sqlParam = objCommand.Parameters.Add("@Garage", SqlDbType.VarChar, 50);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = garage;

				// Add "Sewer" SQL parameter.
				string sewer = Convert.ToString(dr["sewer"]);
				if (sewer == String.Empty || sewer == null)
				{
					sewer = "Public";
				}
				sqlParam = objCommand.Parameters.Add("@Sewer", SqlDbType.VarChar, 50);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = sewer;

				// Add "Water" SQL parameter.
				string water = "Public";
				sqlParam = objCommand.Parameters.Add("@Water", SqlDbType.VarChar, 50);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = water;

				// Add "Description1" SQL parameter.
				string description1 = Convert.ToString(dr["description"]);
				sqlParam = objCommand.Parameters.Add("@Description1", SqlDbType.VarChar, 1000);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = description1;

				// Add database record.
				// Catch and display any errors
				try
				{
					// Execute query
					objConnection.Open();
					objCommand.ExecuteNonQuery();
					objConnection.Close();
				}
				catch (SqlException SqlEx)
				{
					// Show error.
					ErrorsLabel.Visible = true;
					ErrorsLabel.Text = SqlEx.Message;
				}
				catch (Exception Ex)
				{
					// Show error.
					ErrorsLabel.Visible = true;
					ErrorsLabel.Text = Ex.Message;
				}

				// Set values to PropertyID struct by getting output value
				// from database.
				propID.newPropertyID = (int) objCommand.Parameters["@PropertyIDOut"].Value;
				propID.oldListingID = Convert.ToInt32(dr["listing_id"]);

				// Create listing folder using propID.
				CreatePropertyFolder(propID.newPropertyID);
				
				// Insert photos.
				InsertPhotos(propID);
			}
			else if (cpt.classID == ListingClass.Commercial)
			{
				// Create SqlConnection Object
				SqlConnection objConnection = new SqlConnection(CopelandDBString);

				// Create SqlCommand Object
				SqlCommand objCommand = new SqlCommand("adm_InsertCommercialProperty", objConnection);
				objCommand.CommandType = CommandType.StoredProcedure;

				// Create SqlParameter Object
				SqlParameter sqlParam;

				// Add "StatusID" SQL parameter.
				int status = matcher.MatchStatus(dr["status"].ToString());
				sqlParam = objCommand.Parameters.Add("@StatusID", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = status;

				// Add "PropertyTypeID" SQL parameter.
				byte propertyTypeID = Convert.ToByte(cpt.propertyTypeID);
				sqlParam = objCommand.Parameters.Add("@PropertyTypeID", SqlDbType.TinyInt);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = propertyTypeID;

				// Add "PropertyIDOut" value
				sqlParam = objCommand.Parameters.Add("@PropertyIDOut", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Output;

				// Add "MLS1" SQL parameter, if field is filled in.
				if (dr["mls"].ToString() != String.Empty)
				{
					sqlParam = objCommand.Parameters.Add("@MLS1", SqlDbType.VarChar, 20);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = dr["mls"].ToString();
				}

				// Add "Price" SQL parameter.
				decimal price = Convert.ToDecimal(dr["price"]);
				sqlParam = objCommand.Parameters.Add("@Price", SqlDbType.Money);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = price;

				// Add "AgentID1" SQL parameter.
				int agentID1 = matcher.MatchAgent(Convert.ToInt32(dr["agent_id"]));
				sqlParam = objCommand.Parameters.Add("@AgentID1", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = agentID1;

				// Add "Tagline" SQL parameter.
				string tagline = "Click to see property.";
				sqlParam = objCommand.Parameters.Add("@Tagline", SqlDbType.VarChar, 100);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = tagline;

				// Add "Address1" SQL parameter.
				string address1 = Convert.ToString(dr["address"]);
				sqlParam = objCommand.Parameters.Add("@Address1", SqlDbType.VarChar, 100);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = address1;

				// Add "CityID" SQL parameter.
				int cityID = Convert.ToInt32(matcher.MatchCity(dr["city"].ToString()));
				sqlParam = objCommand.Parameters.Add("@CityID", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = cityID;

				// Add "PropertyName" SQL parameter.
				string propertyName = Convert.ToString(dr["propname"].ToString());
				sqlParam = objCommand.Parameters.Add("@PropertyName", SqlDbType.VarChar, 150);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = propertyName;

				// Add "LocationID" SQL parameter.
				int locationID = matcher.MatchLocation(dr["location"].ToString());
				sqlParam = objCommand.Parameters.Add("@LocationID", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = locationID;

				// Add "BathsFull" SQL parameter.
				int bathsFull = Convert.ToInt32(dr["baths_full"]);
				sqlParam = objCommand.Parameters.Add("@BathsFull", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = bathsFull;

				// Add "BathsHalf" SQL parameter.
				int bathsHalf = Convert.ToInt32(dr["baths_half"]);
				sqlParam = objCommand.Parameters.Add("@BathsHalf", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = bathsHalf;

				// Add "YearBuilt" SQL parameter.
				string yearBuilt = Convert.ToString(dr["built"]);
				sqlParam = objCommand.Parameters.Add("@YearBuilt", SqlDbType.Char, 4);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = yearBuilt;

				// Add "EstYearlyTaxes" SQL parameter, if field is filled in.
				if (dr["taxes"].ToString() != String.Empty)
				{
					decimal estYearlyTaxes = Convert.ToDecimal(dr["taxes"]);
					sqlParam = objCommand.Parameters.Add("@EstYearlyTaxes", SqlDbType.Money);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = estYearlyTaxes;
				}

				// Add "SqFt" SQL parameter.
				int sqFt = Convert.ToInt32(dr["sqft"]);
				sqlParam = objCommand.Parameters.Add("@SqFt", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = sqFt;

				// Add "LotSize" SQL parameter, if field is filled in.
				if (dr["lotsize"].ToString() != String.Empty)
				{
					string lotSize = Convert.ToString(dr["lotsize"]);
					sqlParam = objCommand.Parameters.Add("@LotSize", SqlDbType.VarChar, 50);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = lotSize;
				}

				// Add "Parking" SQL parameter.
				short parking = 0;
				sqlParam = objCommand.Parameters.Add("@Parking", SqlDbType.Bit);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = parking;

				// Add "HeatingType" SQL parameter.
				string heatingType = Convert.ToString(dr["heating"]);
				if (heatingType == String.Empty || heatingType == null)
				{
					heatingType = "Other";
				}
				sqlParam = objCommand.Parameters.Add("@HeatingType", SqlDbType.VarChar, 150);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = heatingType;

				// Add "CoolingType" SQL parameter.
				string coolingType = Convert.ToString(dr["cooling"]);
				if (coolingType == String.Empty || coolingType == null)
				{
					coolingType = "Other";
				}
				sqlParam = objCommand.Parameters.Add("@CoolingType", SqlDbType.VarChar, 150);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = coolingType;

				// Add "SidingType" SQL parameter.
				string sidingType = Convert.ToString(dr["siding"]);
				if (sidingType == String.Empty || sidingType == null)
				{
					sidingType = "Other";
				}
				sqlParam = objCommand.Parameters.Add("@SidingType", SqlDbType.VarChar, 150);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = sidingType;

				// Add "Garage" SQL parameter.
				string garage = Convert.ToString(dr["garage"]);
				if (garage == String.Empty || garage == null)
				{
					garage = "Other";
				}
				sqlParam = objCommand.Parameters.Add("@Garage", SqlDbType.VarChar, 50);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = garage;

				// Add "Sewer" SQL parameter.
				string sewer = Convert.ToString(dr["sewer"]);
				if (sewer == String.Empty || sewer == null)
				{
					sewer = "Public";
				}
				sqlParam = objCommand.Parameters.Add("@Sewer", SqlDbType.VarChar, 50);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = sewer;

				// Add "Water" SQL parameter.
				string water = "Public";
				sqlParam = objCommand.Parameters.Add("@Water", SqlDbType.VarChar, 50);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = water;

				// Add "Description1" SQL parameter.
				string description1 = Convert.ToString(dr["description"]);
				sqlParam = objCommand.Parameters.Add("@Description1", SqlDbType.VarChar, 1000);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = description1;

				// Add database record
				// Catch and display any errors
				try
				{
					// Execute query
					objConnection.Open();
					objCommand.ExecuteNonQuery();
					objConnection.Close();
				}
				catch (SqlException SqlEx)
				{
					// Show error.
					ErrorsLabel.Visible = true;
					ErrorsLabel.Text = SqlEx.Message;
				}
				catch (Exception Ex)
				{
					// Show error.
					ErrorsLabel.Visible = true;
					ErrorsLabel.Text = Ex.Message;
				}

				// Set values to PropertyID struct by getting output value
				// from database.
				propID.newPropertyID = (int) objCommand.Parameters["@PropertyIDOut"].Value;
				propID.oldListingID = Convert.ToInt32(dr["listing_id"]);

				// Create listing folder using propID.
				CreatePropertyFolder(propID.newPropertyID);
				
				// Insert photos.
				InsertPhotos(propID);
			}
			else if (cpt.classID == ListingClass.LotsLand)
			{
				// Create SqlConnection Object
				SqlConnection objConnection = new SqlConnection(CopelandDBString);

				// Create SqlCommand Object
				SqlCommand objCommand = new SqlCommand("adm_InsertLotsLandProperty", objConnection);
				objCommand.CommandType = CommandType.StoredProcedure;

				// Create SqlParameter Object
				SqlParameter sqlParam;

				// Add "StatusID" SQL parameter.
				int status = matcher.MatchStatus(dr["status"].ToString());
				sqlParam = objCommand.Parameters.Add("@StatusID", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = status;

				// Add "PropertyTypeID" SQL parameter.
				byte propertyTypeID = Convert.ToByte(cpt.propertyTypeID);
				sqlParam = objCommand.Parameters.Add("@PropertyTypeID", SqlDbType.TinyInt);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = propertyTypeID;

				// Add "PropertyIDOut" value
				sqlParam = objCommand.Parameters.Add("@PropertyIDOut", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Output;

				// Add "MLS1" SQL parameter, if field is filled in.
				if (dr["mls"].ToString() != String.Empty)
				{
					sqlParam = objCommand.Parameters.Add("@MLS1", SqlDbType.VarChar, 20);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = dr["mls"].ToString();
				}

				// Add "Price" SQL parameter.
				decimal price = Convert.ToDecimal(dr["price"]);
				sqlParam = objCommand.Parameters.Add("@Price", SqlDbType.Money);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = price;

				// Add "AgentID1" SQL parameter.
				int agentID1 = matcher.MatchAgent(Convert.ToInt32(dr["agent_id"]));
				sqlParam = objCommand.Parameters.Add("@AgentID1", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = agentID1;

				// Add "Tagline" SQL parameter.
				string tagline = "Click to see property.";
				sqlParam = objCommand.Parameters.Add("@Tagline", SqlDbType.VarChar, 100);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = tagline;

				// Add "Address1" SQL parameter.
				string address1 = Convert.ToString(dr["address"]);
				sqlParam = objCommand.Parameters.Add("@Address1", SqlDbType.VarChar, 100);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = address1;

				// Add "CityID" SQL parameter.
				int cityID = Convert.ToInt32(matcher.MatchCity(dr["city"].ToString()));
				sqlParam = objCommand.Parameters.Add("@CityID", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = cityID;

				// Add "PropertyName" SQL parameter.
				string propertyName = Convert.ToString(dr["propname"].ToString());
				sqlParam = objCommand.Parameters.Add("@PropertyName", SqlDbType.VarChar, 150);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = propertyName;

				// Add "LocationID" SQL parameter.
				int locationID = matcher.MatchLocation(dr["location"].ToString());
				sqlParam = objCommand.Parameters.Add("@LocationID", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = locationID;

				// Add "EstYearlyTaxes" SQL parameter, if field is filled in.
				if (dr["taxes"].ToString() != String.Empty)
				{
					decimal estYearlyTaxes = Convert.ToDecimal(dr["taxes"]);
					sqlParam = objCommand.Parameters.Add("@EstYearlyTaxes", SqlDbType.Money);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = estYearlyTaxes;
				}

				// Add "HOAFees" SQL parameter, if field is filled in.
				if (dr["hoa"].ToString() != String.Empty)
				{
					decimal hOAFees = Convert.ToDecimal(dr["hoa"]);
					sqlParam = objCommand.Parameters.Add("@HOAFees", SqlDbType.Money);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = hOAFees;
				}

				// Add "HOAPeriod" SQL parameter.
				string hOAPeriod = Convert.ToString(dr["hoa_period"]);
				sqlParam = objCommand.Parameters.Add("@HOAPeriod", SqlDbType.VarChar, 50);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = hOAPeriod;

				// Add "LotSize" SQL parameter, if field is filled in.
				if (dr["lotsize"].ToString() != String.Empty)
				{
					string lotSize = Convert.ToString(dr["lotsize"]);
					sqlParam = objCommand.Parameters.Add("@LotSize", SqlDbType.VarChar, 50);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = lotSize;
				}

				// Add "ElemSchool" SQL parameter, if field is filled in.
				string elemSchool = Convert.ToString(dr["school1"]);
				if (elemSchool == String.Empty || elemSchool == null)
				{
					sqlParam = objCommand.Parameters.Add("@ElemSchool", SqlDbType.VarChar, 100);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = elemSchool;
				}

				// Add "MiddleSchool" SQL parameter, if field is filled in.
				string middleSchool = Convert.ToString(dr["school2"]);
				if (middleSchool == String.Empty || middleSchool == null)
				{
					sqlParam = objCommand.Parameters.Add("@MiddleSchool", SqlDbType.VarChar, 100);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = middleSchool;
				}

				// Add "HighSchool" SQL parameter, if field is filled in.
				string highSchool = Convert.ToString(dr["school3"]);
				if (highSchool == String.Empty || highSchool == null)
				{
					sqlParam = objCommand.Parameters.Add("@HighSchool", SqlDbType.VarChar, 100);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = highSchool;
				}

				// Add "Cable" SQL parameter.
				string cable = "Available";
				sqlParam = objCommand.Parameters.Add("@Cable", SqlDbType.VarChar, 50);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = cable;

				// Add "Electric" SQL parameter.
				string electric = "Available";
				sqlParam = objCommand.Parameters.Add("@Electric", SqlDbType.VarChar, 50);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = electric;

				// Add "NaturalGas" SQL parameter.
				string naturalGas = "Available";
				sqlParam = objCommand.Parameters.Add("@NaturalGas", SqlDbType.VarChar, 50);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = naturalGas;

				// Add "Phone" SQL parameter.
				string phone = "Available";
				sqlParam = objCommand.Parameters.Add("@Phone", SqlDbType.VarChar, 50);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = phone;

				// Add "Sewer" SQL parameter.
				string sewer = Convert.ToString(dr["sewer"]);
				if (sewer == String.Empty || sewer == null)
				{
					sewer = "Public Available";
				}
				sqlParam = objCommand.Parameters.Add("@Sewer", SqlDbType.VarChar, 50);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = sewer;

				// Add "Water" SQL parameter.
				string water = "Public Available";
				sqlParam = objCommand.Parameters.Add("@Water", SqlDbType.VarChar, 50);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = water;

				// Add "Description1" SQL parameter.
				string description1 = Convert.ToString(dr["description"]);
				sqlParam = objCommand.Parameters.Add("@Description1", SqlDbType.VarChar, 1000);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = description1;

				// Add database record
				// Catch and display any errors
				try
				{
					// Execute query
					objConnection.Open();
					objCommand.ExecuteNonQuery();
					objConnection.Close();
				}
				catch (SqlException SqlEx)
				{
					// Show error.
					ErrorsLabel.Visible = true;
					ErrorsLabel.Text = SqlEx.Message;
				}
				catch (Exception Ex)
				{
					// Show error.
					ErrorsLabel.Visible = true;
					ErrorsLabel.Text = Ex.Message;
				}

				// Set values to PropertyID struct by getting output value
				// from database.
				propID.newPropertyID = (int) objCommand.Parameters["@PropertyIDOut"].Value;
				propID.oldListingID = Convert.ToInt32(dr["listing_id"]);

				// Create listing folder using propID.
				CreatePropertyFolder(propID.newPropertyID);
				
				// Insert photos.
				InsertPhotos(propID);
			}
			else if (cpt.classID == ListingClass.BoatSlipOther)
			{
				// Create SqlConnection Object
				SqlConnection objConnection = new SqlConnection(CopelandDBString);

				// Create SqlCommand Object
				SqlCommand objCommand = new SqlCommand("adm_InsertBoatSlipOtherProperty", objConnection);
				objCommand.CommandType = CommandType.StoredProcedure;

				// Create SqlParameter Object
				SqlParameter sqlParam;

				// Add "StatusID" SQL parameter.
				int status = matcher.MatchStatus(dr["status"].ToString());
				sqlParam = objCommand.Parameters.Add("@StatusID", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = status;

				// Add "PropertyTypeID" SQL parameter.
				byte propertyTypeID = Convert.ToByte(cpt.propertyTypeID);
				sqlParam = objCommand.Parameters.Add("@PropertyTypeID", SqlDbType.TinyInt);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = propertyTypeID;

				// Add "PropertyIDOut" value
				sqlParam = objCommand.Parameters.Add("@PropertyIDOut", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Output;

				// Add "MLS1" SQL parameter, if field is filled in.
				if (dr["mls"].ToString() != String.Empty)
				{
					sqlParam = objCommand.Parameters.Add("@MLS1", SqlDbType.VarChar, 20);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = dr["mls"].ToString();
				}

				// Add "Price" SQL parameter.
				decimal price = Convert.ToDecimal(dr["price"]);
				sqlParam = objCommand.Parameters.Add("@Price", SqlDbType.Money);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = price;

				// Add "AgentID1" SQL parameter.
				int agentID1 = matcher.MatchAgent(Convert.ToInt32(dr["agent_id"]));
				sqlParam = objCommand.Parameters.Add("@AgentID1", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = agentID1;

				// Add "Tagline" SQL parameter.
				string tagline = "Click to see property.";
				sqlParam = objCommand.Parameters.Add("@Tagline", SqlDbType.VarChar, 100);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = tagline;

				// Add "Address1" SQL parameter.
				string address1 = Convert.ToString(dr["address"]);
				sqlParam = objCommand.Parameters.Add("@Address1", SqlDbType.VarChar, 100);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = address1;

				// Add "CityID" SQL parameter.
				int cityID = Convert.ToInt32(matcher.MatchCity(dr["city"].ToString()));
				sqlParam = objCommand.Parameters.Add("@CityID", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = cityID;

				// Add "PropertyName" SQL parameter.
				string propertyName = Convert.ToString(dr["propname"].ToString());
				sqlParam = objCommand.Parameters.Add("@PropertyName", SqlDbType.VarChar, 150);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = propertyName;

				// Add "HOAFees" SQL parameter, if field is filled in.
				if (dr["hoa"].ToString() != String.Empty)
				{
					decimal hOAFees = Convert.ToDecimal(dr["hoa"]);
					sqlParam = objCommand.Parameters.Add("@HOAFees", SqlDbType.Money);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = hOAFees;
				}

				// Add "HOAPeriod" SQL parameter.
				string hOAPeriod = Convert.ToString(dr["hoa_period"]);
				sqlParam = objCommand.Parameters.Add("@HOAPeriod", SqlDbType.VarChar, 50);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = hOAPeriod;

				// Add "Parking" SQL parameter.
				short parking = 0;
				sqlParam = objCommand.Parameters.Add("@Parking", SqlDbType.Bit);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = parking;

				// Add "PhoneConnectionAvail" SQL parameter.
				short phoneConnectionAvail = 0;
				sqlParam = objCommand.Parameters.Add("@PhoneConnectionAvail", SqlDbType.Bit);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = phoneConnectionAvail;

				// Add "ElectricConnectionAvail" SQL parameter.
				short electricConnectionAvail = 0;
				sqlParam = objCommand.Parameters.Add("@ElectricConnectionAvail", SqlDbType.Bit);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = electricConnectionAvail;

				// Add "WaterConnectionAvail" SQL parameter.
				short waterConnectionAvail = 0;
				sqlParam = objCommand.Parameters.Add("@WaterConnectionAvail", SqlDbType.Bit);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = waterConnectionAvail;

				// Add "Description1" SQL parameter.
				string description1 = Convert.ToString(dr["description"]);
				sqlParam = objCommand.Parameters.Add("@Description1", SqlDbType.VarChar, 1000);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = description1;

				// Add database record
				// Catch and display any errors
				try
				{
					// Execute query
					objConnection.Open();
					objCommand.ExecuteNonQuery();
					objConnection.Close();
				}
				catch (SqlException SqlEx)
				{
					// Show error.
					ErrorsLabel.Visible = true;
					ErrorsLabel.Text = SqlEx.Message;
				}
				catch (Exception Ex)
				{
					// Show error.
					ErrorsLabel.Visible = true;
					ErrorsLabel.Text = Ex.Message;
				}

				// Set values to PropertyID struct by getting output value
				// from database.
				propID.newPropertyID = (int) objCommand.Parameters["@PropertyIDOut"].Value;
				propID.oldListingID = Convert.ToInt32(dr["listing_id"]);

				// Create listing folder using propID.
				CreatePropertyFolder(propID.newPropertyID);
				
				// Insert photos.
				InsertPhotos(propID);
			}

			return propID;
		}
		
		protected void InsertPhotos(PropertyID propertyID)
		{
			// Create new DataSet to hold photo data.
			DataSet photoData = GetPhotoData(propertyID.oldListingID);

			foreach (DataRow dr in photoData.Tables[0].Rows)
			{
				// Create SqlConnection Object.
				SqlConnection sqlConnection = new SqlConnection(CopelandDBString);

				// Create SqlCommand Object.
				SqlCommand sqlCommand = new SqlCommand("adm_InsertPicture", sqlConnection);

				// Set CommandType to StoredProcedure.
				sqlCommand.CommandType = CommandType.StoredProcedure;

				// Create SqlParameter Object.
				SqlParameter sqlParam;

				// Add "PropertyID" Parameter
				sqlParam = sqlCommand.Parameters.Add("@PropertyID", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = propertyID.newPropertyID;

				// Add "PhotoIDOut" Parameter
				sqlParam = sqlCommand.Parameters.Add("@PhotoIDOut", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Output;

				// Add "MainPhoto" Parameter
				sqlParam = sqlCommand.Parameters.Add("@MainPhoto", SqlDbType.Bit);
				sqlParam.Direction = ParameterDirection.Input;

				// Execute query.
				sqlConnection.Open();
				sqlCommand.ExecuteNonQuery();
				sqlConnection.Close();

				// Get "PhotoIDOut" from database output parameter.
				int photoID = (int) sqlCommand.Parameters["@PhotoIDOut"].Value;
				
				ConvResults.Items.Add("Added photo #" + photoID.ToString());

				// Get the photo data as byte arrays.
				byte[] thumbData = (byte[])dr["thumb_data"];
				byte[] lgPhotoData = (byte[])dr["photo_data"];
					
				// Save the physical images to file.
				WriteImageToFile(thumbData, propertyID.newPropertyID, photoID, ImageSize.Small);
				WriteImageToFile(lgPhotoData, propertyID.newPropertyID, photoID, ImageSize.Large);
			}
		}

		protected string CreatePropertyFolder(int propID)
		{
			// Set directory path variable.
			string dirPath = @"\\Homestar\websites\copelandrealestatenc\listings\" + propID + "\\";

			// If directory does not exist.
			if (!Directory.Exists(dirPath))
			{
				// Create directory.
				Directory.CreateDirectory(dirPath);
			}

			// Return path.
			return dirPath;
		}
		
		public string GetPropertyFolderPath(int propID)
		{
			// Set directory path variable.
			string dirPath = @"\\Homestar\websites\copelandrealestatenc\listings\" + propID + "\\";
			
			// Return path.
			return dirPath;
		}

		private void ConvButton_Click(object sender, EventArgs e)
		{
			// If the test one record option is checked
			// try inserting one record into database.
			if (TestOneOption.Checked)
			{
				// Get old records as DataSet.
				DataSet OldCopelandRecords = GetData();
				
				// Clear results and notify user of startup.
				ConvResults.Items.Clear();
				ConvResults.Items.Add("Starting conversion..." + " Table count: " + OldCopelandRecords.Tables.Count.ToString());

				// Initialize PropertyID value and flag.
				PropertyID propertyIDSuccessFlag;

				// Insert record using first row of data.
				propertyIDSuccessFlag = InsertRecord(OldCopelandRecords.Tables[0].Rows[0]);

				if (propertyIDSuccessFlag.newPropertyID == 0)
				{
					ConvResults.Items.Add("Single record conversion failed.");
				}
				else
				{
					ConvResults.Items.Add("Single conversion succeeded!");
					ConvResults.Items.Add("Added new record number: " +
					                      propertyIDSuccessFlag.newPropertyID.ToString() +
					                      " from old record " +
					                      propertyIDSuccessFlag.oldListingID.ToString() +
					                      ".");
				}
			}

			// If convert all records option is checked
			// insert all records.
			if (ConvertAllOption.Checked)
			{
				// Get old records as DataSet.
				DataSet OldCopelandRecords = GetData();
				
				// Clear results and notify user of startup.
				ConvResults.Items.Clear();
				ConvResults.Items.Add("Starting conversion..." + " Table count: " + OldCopelandRecords.Tables.Count.ToString());

				// Initialize PropertyID value and flag.
				PropertyID propertyIDSuccessFlag;

				// Loop through all records.
				for (int i = 0; i < OldCopelandRecords.Tables[0].Rows.Count; i++)
				{
					// Insert record using first row of data.
					propertyIDSuccessFlag = InsertRecord(OldCopelandRecords.Tables[0].Rows[i]);

					// Display conversion results.
					if (propertyIDSuccessFlag.newPropertyID == 0)
					{
						ConvResults.Items.Add("Conversion #" + (i + 1).ToString() + " failed.");
					}
					else
					{
						ConvResults.Items.Add("Conversion #" + (i + 1).ToString() + " succeeded!");
						ConvResults.Items.Add("Added new record number: " +
						                    propertyIDSuccessFlag.newPropertyID.ToString() +
						                    " from old record " +
						                    propertyIDSuccessFlag.oldListingID.ToString());
					}
				}
			}
		}
		
		protected void WriteImageToFile(byte[] photoData, int propID, int photoID, ImageSize size)
		{
			// Determine string file name extension based
			// on file size.
			string sizeStringExt;
			if (size == ImageSize.Small)
			{
				sizeStringExt = "sm.jpg";
			}
			else if (size == ImageSize.Medium)
			{
				sizeStringExt = "md.jpg";
			}
			else
			{
				sizeStringExt = "lg.jpg";
			}
			
			// File name.
			string FILE_NAME = GetPropertyFolderPath(propID) + 
				propID.ToString() + photoID.ToString() + sizeStringExt;
					
			// Create the new, empty data file.
			if (File.Exists(FILE_NAME)) 
			{
				ErrorsLabel.Visible = true;
				ErrorsLabel.Text = String.Format("{0} already exists!", FILE_NAME);
				return;
			}
			FileStream fs = new FileStream(FILE_NAME, FileMode.CreateNew);
					
			// Create the writer for data.
			BinaryWriter w = new BinaryWriter(fs);
					
			// Write data to file.
			w.Write(photoData);
					
			// Close up shop.
			w.Close();
			fs.Close();
			
			ConvResults.Items.Add("Create file name:" + FILE_NAME);
		}
	}
}