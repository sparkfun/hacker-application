using System;
using System.Data.SqlClient;

namespace CRENConversion
{
	/// <summary>
	/// Summary description for Checks.
	/// </summary>
	public class Checks
	{
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

		public bool MLSExists(string tableName, int MLS)
		{
			// Create SqlConnection
			SqlConnection sqlCn = new SqlConnection(SQLConnString);

			// Open SqlConnection
			sqlCn.Open();

			// Create command text
			string sqlCommand = "SELECT COUNT(*) " +
				"FROM " + tableName + " " +
				"WHERE MLS = '" + MLS.ToString() + "'";

			// Create SqlCommand
			SqlCommand sqlCmd = new SqlCommand(sqlCommand, sqlCn);

			// Use ExecuteScalar to determine if record exists or not
			bool recordFound = Convert.ToBoolean(sqlCmd.ExecuteScalar());

			// Close connection
			sqlCn.Close();

			// Return the result
			return recordFound;
		}

		public bool AltMLSExists(string tableName, int MLS)
		{
			// Create SqlConnection
			SqlConnection sqlCn = new SqlConnection(SQLConnString);

			// Open SqlConnection
			sqlCn.Open();

			// Create command text
			string sqlCommand = "SELECT COUNT(AltMLS) " +
				"FROM " + tableName + " " +
				"WHERE MLS = '" + MLS.ToString() + "'";

			// Create SqlCommand
			SqlCommand sqlCmd = new SqlCommand(sqlCommand, sqlCn);

			// Use ExecuteScalar to determine if record exists or not
			bool recordFound = Convert.ToBoolean(sqlCmd.ExecuteScalar());

			// Close connection
			sqlCn.Close();

			// Return the result
			return recordFound;
		}

		public bool MLSAvailable(string tableName, int MLS)
		{
			// Create SqlConnection
			SqlConnection sqlCn = new SqlConnection(SQLConnString);

			// Open SqlConnection
			sqlCn.Open();

			// Create command text
			string sqlCommand = "SELECT COUNT(*) " +
				"FROM " + tableName + " " +
				"WHERE MLS = '" + MLS.ToString() + "'";

			// Create SqlCommand
			SqlCommand sqlCmd = new SqlCommand(sqlCommand, sqlCn);

			// Use ExecuteScalar to determine if record exists or not
			bool recordFound = Convert.ToBoolean(sqlCmd.ExecuteScalar());

			// Close connection
			sqlCn.Close();

			// If a record is found return not available
			if (recordFound)
			{
				return false;
			}
			else
			{
				// Otherwise return true if the record is 
				// available for use
				return true;
			}
		}
	}
}