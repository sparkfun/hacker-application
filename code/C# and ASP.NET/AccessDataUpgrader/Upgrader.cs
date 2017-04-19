using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace AccessDataUpgrader
{
	/// <summary>
	/// Summary description for Upgrader.
	/// </summary>
	public class Upgrader
	{
		private enum QueryStringType
		{
			CountOnly = 1,
			Values = 2
		}
		
		private enum FileType
		{
			DOC = 1,
			PDF = 2
		}
		
		// Public property Access database connection string.
		private string accessConnString;
		public string AccessConnString
		{
			get
			{
				return accessConnString;
			}
			set
			{
				accessConnString = value;
			}
		}

		// Public property SQL Server database connection string.
		private string sqlConnString;
		public string SQLConnString
		{
			get
			{
				return sqlConnString;
			}
			set
			{
				sqlConnString = value;
			}
		}
		
		public void ProcessRecords()
		{
			// Create OleDbConnection getting the connection string
			// from the AccessConnString property.
			OleDbConnection cn = new OleDbConnection(AccessConnString);
			
			// Open the connection.
			cn.Open();

			// Create SQL command string to get the records.
			string sqlCommand = "SELECT ID, CustId, Title, Photo1, Photo2, Photo3, Photo4, Photo5, File1 " +
								"FROM RVParkClassifieds " +
								"ORDER BY ID ASC;";
			
			// Create OleDbCommand.
			OleDbCommand cmd = new OleDbCommand(sqlCommand, cn);

			// Create OleDbDataReader.
			OleDbDataReader rdr = cmd.ExecuteReader();
			
			// Create the new log file
			FileStream fs = new FileStream("UpgradeLog.txt", FileMode.Create);

			// Create a writer and specify the encoding
			// The default (UTF-8) supports special unicode characters,
			// but encodes all standard characters in the same way as
			// ASCII encoding
			StreamWriter w = new StreamWriter(fs, Encoding.UTF8);
			
			// Read through records and display results.
			while (rdr.Read())
			{
				// Create a new AccessRecord object.
				AccessRecord aRecord = new AccessRecord();
				aRecord.ID = rdr.GetInt32(0);
				aRecord.CustID = rdr.GetInt16(1);
				aRecord.Title = rdr.GetString(2);
				
				// Assign values to the photo array.  There
				// are five photo fields total.
				for (int i = 0; i <= 4; i++)
				{
					// Check for null values.
					if (!rdr.IsDBNull(i + 3))
					{
						aRecord.Photos[i] = rdr.GetString(i + 3);
					}
				}
				// Check for null values.
				if (!rdr.IsDBNull(8))
				{
					aRecord.File = rdr.GetString(8);
				}
				
				// Check to see if a matching record exits. A matching
				// record is determined by comparing CustId and Title.
				if (MatchingRecordExists(aRecord))
				{
					// Update the record.
					SQLRecord sRecord = UpdateRecord(aRecord);
					
					// Display and log progress.
					Console.WriteLine("Successfully updated matching ad '{0}' with title {1}.", 
						sRecord.AdID, sRecord.Headline);
					w.WriteLine("Successfully updated matching ad '{0}' with title {1}.", 
						sRecord.AdID, sRecord.Headline);
				}
				else
				{
					Console.WriteLine("No matching record for ad ID#{0} titled '{1}'.", aRecord.ID, aRecord.Title);
					w.WriteLine("No matching record for ad ID#{0} titled '{1}'.", aRecord.ID, aRecord.Title);
					
				}
			}
			
			// Make sure all data is written from the internal buffer.
			w.Flush();

			// Close the file stream.
			w.Close();
			fs.Close();
			
			// Close the reader and connection
			rdr.Close();
			cn.Close();
			
			Console.WriteLine("***Finished****");
		}
		
		private SQLRecord UpdateRecord(AccessRecord aRecord)
		{
			// Create SQLRecord object from AccessRecord object.
			SQLRecord sRecord = GetSQLRecord(aRecord);
			
			// Create SqlConnection Object.
			SqlConnection sqlCn = new SqlConnection(SQLConnString);
			
			// Create SqlCommand Object.
			SqlCommand sqlCm = new SqlCommand("InsertPhoto", sqlCn);
			sqlCm.CommandType = CommandType.StoredProcedure;
			
			// Add a photo record for each photo associated
			// with record.
			foreach (string s in aRecord.Photos)
			{
				if (s != null && s != String.Empty)
				{
					// Create SqlParameter Object.
					SqlParameter sqlParam;
			
					// Add "AdID" SQL parameter.
					sqlParam = sqlCm.Parameters.Add("@AdID", SqlDbType.Int);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = sRecord.AdID;
					
					// Add "PhotoName" SQL parameter.
					sqlParam = sqlCm.Parameters.Add("@PhotoName", SqlDbType.VarChar, 50);
					sqlParam.Direction = ParameterDirection.Input;
					sqlParam.Value = s;
			
					// Add database record.
					// Catch and display any errors.
					try
					{
						// Execute query.
						sqlCn.Open();
						sqlCm.ExecuteNonQuery();
						sqlCn.Close();
					}
					catch (SqlException sqlEx)
					{
						Console.WriteLine("Caught SqlException: {0}", sqlEx.Message);
					}
					catch (Exception Ex)
					{
						Console.WriteLine("Caught General Exception: {0}", Ex.Message);
					}
					
					Console.WriteLine("Added photo '{0}' for AD ID# '{1}'.", s, sRecord.AdID);
				}
			}
			
			// Change command.
			sqlCm.CommandText = "InsertFile";
			sqlCm.CommandType = CommandType.StoredProcedure;
			
			// Add a file record if one exists.
			if (aRecord.File != null && aRecord.File != String.Empty)
			{
				// Get file extension.
				FileType fileExt = GetFileExtension(aRecord.File);
				
				// Create SqlParameter Object.
				SqlParameter sqlParam;
			
				// Add "AdID" SQL parameter.
				sqlParam = sqlCm.Parameters.Add("@AdID", SqlDbType.Int);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = sRecord.AdID;
					
				// Add "FileName" SQL parameter.
				sqlParam = sqlCm.Parameters.Add("@FileName", SqlDbType.VarChar, 100);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = aRecord.File;
				
				// Add "FileTypeID" SQL parameter.
				sqlParam = sqlCm.Parameters.Add("@FileTypeID", SqlDbType.SmallInt);
				sqlParam.Direction = ParameterDirection.Input;
				sqlParam.Value = fileExt;
			
				// Add database record.
				// Catch and display any errors.
				try
				{
					// Execute query.
					sqlCn.Open();
					sqlCm.ExecuteNonQuery();
					sqlCn.Close();
				}
				catch (SqlException sqlEx)
				{
					Console.WriteLine("Caught SqlException: {0}", sqlEx.Message);
				}
				catch (Exception Ex)
				{
					Console.WriteLine("Caught General Exception: {0}", Ex.Message);
				}
				
				Console.WriteLine("Added file '{0}' for AD ID# '{1}'.", aRecord.File, sRecord.AdID);
			}
			
			// Return SQLRecord object.
			return sRecord;
		}
		
		private SQLRecord GetSQLRecord(AccessRecord record)
		{
			// Create SqlConnection.
			SqlConnection sqlCn = new SqlConnection(SQLConnString);
			
			// Create SqlCommand.
			SqlCommand sqlCmd = new SqlCommand(GetSQLQueryString(QueryStringType.Values), sqlCn);
			
			// CustID parameter.
			SqlParameter pCustID = sqlCmd.Parameters.Add("@CustID", SqlDbType.Int);
			pCustID.Value = record.CustID;
			
			// Title parameter.
			SqlParameter pTitle = sqlCmd.Parameters.Add("@Headline", SqlDbType.VarChar, 50);
			pTitle.Value = record.Title;
			
			// Create new SQLRecord object.
			SQLRecord sRecord = new SQLRecord();
			
			try
			{
				// Open SqlConnection.
				sqlCn.Open();
				
				// Create SqlDataReader.
				SqlDataReader rdr = sqlCmd.ExecuteReader();
				
				// Open reader.
				rdr.Read();
				
				// Read first (and only) record and initialize
				// SQLRecord object values.
				sRecord.AdID = rdr.GetInt32(0);
				sRecord.CustomerID = rdr.GetInt32(1);
				sRecord.Headline = rdr.GetString(2);
				
				// Close reader.
				rdr.Close();

				// Close connection.
				sqlCn.Close();
				
			}
			catch(SqlException sqlEx)
			{
				Console.WriteLine("Caught SqlException: {0}", sqlEx.Message);
			}
			
			// Return the SQLRecord object.
			return sRecord;
		}
		
		private bool MatchingRecordExists(AccessRecord aRecord)
		{
			// Create SqlConnection.
			SqlConnection sqlCn = new SqlConnection(SQLConnString);
			
			// Create SqlCommand.
			SqlCommand sqlCmd = new SqlCommand(GetSQLQueryString(QueryStringType.CountOnly), sqlCn);
			
			// Create SqlParameter Object.
			SqlParameter sqlParam;
			
			// Add "CustID" SQL parameter.
			sqlParam = sqlCmd.Parameters.Add("@CustID", SqlDbType.Int);
			sqlParam.Direction = ParameterDirection.Input;
			sqlParam.Value = aRecord.CustID;
			
			// Add "Headline" SQL parameter.
			sqlParam = sqlCmd.Parameters.Add("@Headline", SqlDbType.VarChar, 50);
			sqlParam.Direction = ParameterDirection.Input;
			sqlParam.Value = aRecord.Title;
			
			// Initialize and set bool return value.
			bool recordFound = true;
			
			// Try to find the matching record.
			try
			{
				// Open SqlConnection.
				sqlCn.Open();

				// Use ExecuteScalar to determine if record exists or not.
				recordFound = Convert.ToBoolean(sqlCmd.ExecuteScalar());

				// Close connection.
				sqlCn.Close();
				
			}
			catch(SqlException sqlEx)
			{
				Console.WriteLine("Caught SqlException: {0}", sqlEx.Message);
			}

			// Return the result.
			return recordFound;
		}
		
		private string GetSQLQueryString(QueryStringType qsType)
		{
			// Query string variable.
			string qryString;
			
			if (qsType == QueryStringType.CountOnly)
			{
				// SQL query string.
				qryString =	"SELECT COUNT(*) " +
					"FROM " +
					"RVParkAds AS pa " +
					"INNER JOIN Ads as a " +
					"ON a.AdID = pa.AdId " +
					"WHERE CustomerID = @CustID AND Headline = @Headline";
			}
			else
			{
				// SQL query string.
				qryString =	"SELECT a.AdID, CustomerID, Headline" +
					"FROM " +
					"RVParkAds AS pa " +
					"INNER JOIN Ads as a " +
					"ON a.AdID = pa.AdId " +
					"WHERE CustomerID = @CustID AND Headline = @Headline";
				
			}
			
			// Return query string.
			return qryString;
		}
		
		private FileType GetFileExtension(string fileName)
		{
			// Get file extension.
			string tempFileName = fileName.Trim();
			string ext = fileName.Substring(tempFileName.Length - 3, 3).ToUpper();
			if (ext == "DOC")
			{
				return FileType.DOC;
			}
			else
			{
				return FileType.PDF;
			}
		}
		
		public void TestGetFileExtension(string fileName)
		{
			FileType fileType = GetFileExtension(fileName);
			Console.WriteLine("File Type: {0}", fileType.ToString());
		}
	}
}