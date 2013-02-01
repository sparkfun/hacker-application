using System;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace CRENConversion
{
	/// <summary>
	/// Summary description for Conversion.
	/// </summary>
	public class Conversion
	{
		// Declare public property for setting the path
		// to the listings directory
		private string listingPath;
		public string ListingPath
		{
			get
			{
				return listingPath;
			}
			set
			{
				listingPath = value;
			}
		}

		// Declare public property for setting the Access database
		// connection string
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

		// Declare public property for setting the SQL Server database
		// connection string
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

		public void Update(string accessTable, string sqlTable)
		{
			// Create OleDbConnection getting the connection string
			// from the AccessConnString property
			OleDbConnection cn = new OleDbConnection(AccessConnString);
			
			// Open the connection
			cn.Open();

			// Create SQL command string to grab
			// all of the records
			string sqlCommand = "SELECT NavicaMLS, ParagonMLS " +
				"FROM " + accessTable + " " +
				"ORDER BY NavicaMLS ASC;";

			// Create OleDbCommand
			OleDbCommand cmd = new OleDbCommand(sqlCommand, cn);

			// Create OleDbDataReader
			OleDbDataReader rdr = cmd.ExecuteReader();

			// Create Check
			Checks check1 = new Checks();
			check1.SQLConnString = SQLConnString;

			// Create the new log file
			FileStream fs = new FileStream(sqlTable + "Log.txt", FileMode.Create);

			// Create a writer and specify the encoding
			// The default (UTF-8) supports special unicode characters,
			// but encodes all standard characters in the same way as
			// ASCII encoding
			StreamWriter w = new StreamWriter(fs, Encoding.UTF8);

			// Write startup line to console
			Console.WriteLine("Updating {0} database....", sqlTable);

			// Read through records and display results
			while (rdr.Read()) 
			{
				// Step 1: Check to see if the MLS# exists in SQL database
				// Step 2: Check to see if the MLS# is available
				if (check1.MLSExists(sqlTable, Convert.ToInt32(rdr[0])) &&
					check1.MLSAvailable(sqlTable, Convert.ToInt32(rdr[1])))
				{
					// Step 3: Update the MLS/Alternate MLS number
					UpdateMLS(sqlTable, Convert.ToInt32(rdr[0]), Convert.ToInt32(rdr[1]));

					// Step 4: Write success to console
					Console.WriteLine("Success: Updated database MLS number {0} to {1}.", 
						Convert.ToString(rdr[0]), 
						Convert.ToString(rdr[1]));
					w.WriteLine("Success: Updated database MLS number {0} to {1}.", 
						Convert.ToString(rdr[0]), 
						Convert.ToString(rdr[1]));

					// Path to individual listing (old)
					string sourcePath = ListingPath + 
						Convert.ToString(rdr[0]) + @"\";
					// Path to individual listing (new)
					string destinationPath = ListingPath + 
						Convert.ToString(rdr[1]) + @"\";

					// Create boolean to hold value
					bool movedDir;

					// Move the directory
					movedDir = MoveDirectory(sourcePath, destinationPath);

					// If the directory was successfully moved
					if (movedDir)
					{
						// Step 4: Write success to console and log file
						Console.WriteLine("Success: Updated file directory {0} to {1}.", 
							Convert.ToString(rdr[0]), 
							Convert.ToString(rdr[1]));
						w.WriteLine("Success: Updated file directory {0} to {1}.", 
							Convert.ToString(rdr[0]), 
							Convert.ToString(rdr[1]));
					}
					else
					{
						// Step 4: Write failure to console and log file
						Console.WriteLine("Failure: Could not update directory {0}.", 
							Convert.ToString(rdr[0]));
						w.WriteLine("Failure: Could not update directory {0}.", 
							Convert.ToString(rdr[0]));
					}
					
				}
				else
				{
					// Step 4: Write failure to console and log file
					Console.WriteLine("Failure: Could not update MLS number {0}.", 
						Convert.ToString(rdr[0]));
					w.WriteLine("Failure: Could not update MLS number {0}.", 
						Convert.ToString(rdr[0]));
				}
			}

			// Make sure all data is written from the internal buffer.
			w.Flush();

			// Close the file
			w.Close();
			fs.Close();

			// Close the reader and connection
			rdr.Close();
			cn.Close();

			// Write ending line to console
			Console.WriteLine("Finished updating {0} database....", sqlTable);
		}

		public void UpdateMLS(string tableName, int oldMLS, int newMLS)
		{
			// Create SqlConnection
			SqlConnection sqlCn = new SqlConnection(SQLConnString);

			// Create Checks object to check whether or not an alternate
			// mls number exits for the listing
			Checks check = new Checks();
			check.SQLConnString = SQLConnString;
			bool altMlsExists = check.AltMLSExists(tableName.ToString(), oldMLS);

			// Open SqlConnection
			sqlCn.Open();

			// Create command text
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DECLARE" + "\r\n");
			sqlCommand.Append("@OldMLS AS int," + "\r\n");
			sqlCommand.Append("@NewMLS AS int" + "\r\n");
			sqlCommand.Append("SET" + "\r\n");
			sqlCommand.Append("@OldMLS = '" + oldMLS.ToString() + "'" + "\r\n");
			sqlCommand.Append("SET" + "\r\n");
			sqlCommand.Append("@NewMLS = '" + newMLS.ToString() + "'" + "\r\n");
			sqlCommand.Append("UPDATE " + tableName.ToString() + "\r\n");

			// If an alternate mls number does exist just update
			// the new mls number
			// But if no alternate mls number exists update both
			// the new mls number with the new number
			// and the alternate mls number with the old number
			if (altMlsExists)
			{
				sqlCommand.Append("SET MLS = @NewMLS" + "\r\n");
				
			}
			else
			{
				sqlCommand.Append("SET MLS = @NewMLS, AltMLS = @OldMLS" + "\r\n");
			}

			sqlCommand.Append("WHERE MLS = @OldMLS" + "\r\n");
			sqlCommand.Append("UPDATE Pictures" + "\r\n");
			sqlCommand.Append("SET MLS = @NewMLS" + "\r\n");
			sqlCommand.Append("WHERE MLS = @OldMLS" + "\r\n");
			sqlCommand.Append("UPDATE Tours" + "\r\n");
			sqlCommand.Append("SET MLS = @NewMLS" + "\r\n");
			sqlCommand.Append("WHERE MLS = @OldMLS" + "\r\n");

			// Create SqlCommand
			SqlCommand sqlCmd = new SqlCommand(sqlCommand.ToString(), sqlCn);

			// Execute query
			sqlCmd.ExecuteNonQuery();

			// Close connection
			sqlCn.Close();
		}

		public void TestCommandText(string tableName, int oldMLS, int newMLS)
		{
			// Create command text
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DECLARE" + "\r\n");
			sqlCommand.Append("@OldMLS AS int," + "\r\n");
			sqlCommand.Append("@NewMLS AS int" + "\r\n");
			sqlCommand.Append("SET" + "\r\n");
			sqlCommand.Append("@OldMLS = '" + oldMLS.ToString() + "'" + "\r\n");
			sqlCommand.Append("SET" + "\r\n");
			sqlCommand.Append("@NewMLS = '" + newMLS.ToString() + "'" + "\r\n");
			sqlCommand.Append("UPDATE " + tableName.ToString() + "\r\n");
			sqlCommand.Append("SET MLS = @NewMLS" + "\r\n");
			sqlCommand.Append("WHERE MLS = @OldMLS" + "\r\n");
			sqlCommand.Append("UPDATE Pictures" + "\r\n");
			sqlCommand.Append("SET MLS = @NewMLS" + "\r\n");
			sqlCommand.Append("WHERE MLS = @OldMLS" + "\r\n");
			sqlCommand.Append("UPDATE Tours" + "\r\n");
			sqlCommand.Append("SET MLS = @NewMLS" + "\r\n");
			sqlCommand.Append("WHERE MLS = @OldMLS" + "\r\n");

			// Test the command text
			Console.WriteLine(sqlCommand.ToString());
		}

		public bool TestPathMove(string oldMls, string newMls)
		{
			// Set path to listings
			ListingPath = @"C:\CRENConversion\";

			// Path to individual listing (old)
			string sourcePath = ListingPath + oldMls + @"\";
			// Path to individual listing (new)
			string destinationPath = ListingPath + newMls + @"\";
			
			// Return result of 
			return MoveDirectory(sourcePath, destinationPath);
		}

		public bool MoveDirectory(string sourcePath, string destinationPath)
		{
			// Rename picture directory
			// if the directory exists
			// and the destination directory does not exist
			if (Directory.Exists(sourcePath) && !Directory.Exists(destinationPath))
			{
				Directory.Move(sourcePath, destinationPath);
				return true;
			}

			return false;
		}
	}
}