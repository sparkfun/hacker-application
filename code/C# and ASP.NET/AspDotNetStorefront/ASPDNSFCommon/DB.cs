//#define MySQL
// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Configuration;
using System.Web.SessionState;
using System.Data;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.OleDb;
using System.Globalization;

#if MySQL
using MySql.Data.MySqlClient;
#endif


namespace AspDotNetStorefrontCommon
{
	/// <summary>
	/// Summary description for DB.
	/// </summary>
	public class DB
	{

		static CultureInfo USCulture = new CultureInfo("en-US");

		public DB()	{}

		static private String _activeDBProvider = SetDBProvider();
		static private String _activeDBConn = SetDBConn();

		static private String SetDBConn()
		{
			String s = Common.Application("DBConn");
			//s = s.Replace("%WEBDIR%",HttpContext.Current.Server.MapPath("./default.aspx").Replace("default.aspx","").Replace("\\" + Common.AppConfig("AdminDir") + "",""));
			return s;
		}

		static public String GetDBConn()
		{
			return _activeDBConn;
		}

		static public String GetNoLock()
		{
			String tmp = String.Empty;
			if(_activeDBProvider == "MSSQL" && (Common.ApplicationBool("UseSQLNoLock") || Common.ApplicationBool("SQLNoLock")))
			{
				tmp = " with (NOLOCK) ";
			}
			return tmp;
		}

		/// <summary>
		/// Incrementally adds tables results to a dataset
		/// </summary>
		/// <param name="ds">Dataset to add the table to</param>
		/// <param name="tableName">Name of the table to be created in the DataSet</param>
		/// <param name="sqlQuery">Query to retrieve the data for the table.</param>
		static public void FillDataSet(DataSet ds, string tableName, string sqlQuery)
		{
			switch(DB.GetDBProvider())
			{
				case "MSSQL":
					SqlConnection dbconn = new SqlConnection();
					dbconn.ConnectionString = DB.GetDBConn();
					dbconn.Open();
					SqlDataAdapter da = new SqlDataAdapter(sqlQuery,dbconn);
					da.Fill(ds,tableName);
					dbconn.Close();
					break;
#if MySQL
				case "MYSQL":
					Sql = PreprocessSQLForMySQL(Sql);
					MySqlConnection mysql_dbconn = new MySqlConnection();
					mysql_dbconn.ConnectionString = DB.GetDBConn();
					mysql_dbconn.Open();
					MyIDataAdapter mysql_da = new MyIDataAdapter(sqlQuery,mysql_dbconn);
					mysql_da.Fill(ds,tableName);
					mysql_dbconn.Close();
					break;
#endif
				case "MSACCESS":
					sqlQuery = DB.PreprocessSQLForAccess(sqlQuery);
					OleDbConnection access_dbconn = new OleDbConnection();
					access_dbconn.ConnectionString = DB.GetDBConn();
					access_dbconn.Open();
					OleDbDataAdapter access_da = new OleDbDataAdapter(sqlQuery,access_dbconn);
					access_da.Fill(ds,tableName);
					access_dbconn.Close();
					break;
			}
		}



		static private String SetDBProvider()
		{
			String Provider = Common.Application("DBProvider").ToUpper().Trim();
			if(Provider == "ACCESS")
			{
				Provider = "MSACCESS"; // because people will forget the leading MS!
			}
			if(Provider != "MSSQL" && Provider != "MYSQL" && Provider != "MSACCESS")
			{
				Provider = "MSSQL"; // default it
			}
			return Provider;
		}

		static public String GetDBProvider()
		{
			return _activeDBProvider;
		}

		public static String SQuote(String s)
		{
			switch(_activeDBProvider)
			{
				case "MSSQL":
					s = "N'" + s.Replace("'","''") + "'";
					break;
				case "MYSQL":
					s = "'" + s.Replace("'","''") + "'";
					break;
				case "MSACCESS":
					s = "'" + s.Replace("'","''") + "'";
					break;
			}		
			return s;
		}


		public static String DateQuote(String s)
		{
			switch(_activeDBProvider)
			{
				case "MSSQL":
					return "'" + s.Replace("'","''") + "'";
				case "MYSQL":
					return "'" + s.Replace("'","''") + "'";
				case "MSACCESS":
					return "#" + s.Replace("#","") + "#";
			}		
			return "'" + s.Replace("'","''") + "'";
		}

		public static String DateQuote(DateTime dt)
		{
			return DateQuote(dt.ToString());
		}


		static public String PreprocessSQLForAccess(String sql)
		{
			sql = sql.Replace(" 12:00:00.000 AM#","#");
			sql = sql.Replace(" 11:59:59.999 PM#","#");
			sql = sql.Replace("lower(","lcase(");
			sql = sql.Replace("upper(","ucase(");
			sql = sql.Replace("getdate(","Date(");
			sql = sql.Replace("datepart(\"d\",","datepart(\"d\",");
			sql = sql.Replace("SET CONCAT_NULL_YIELDS_NULL OFF;",""); // not supported
			sql = sql.Replace("set concat_null_yields_null off;",""); // not supported
			return sql;
		}

		static public String PreprocessSQLForMySQL(String sql)
		{
			sql = sql.Replace("[day]","day");
			sql = sql.Replace("[Day]","Day");
			sql = sql.Replace("[DAY]","DAY");
			sql = sql.Replace("[year]","year");
			sql = sql.Replace("[Year]","Year");
			sql = sql.Replace("[YEAR]","YEAR");
			sql = sql.Replace("[section]","section");
			sql = sql.Replace("[Section]","Section");
			sql = sql.Replace("[SECTION]","SECTION");
			sql = sql.Replace("[password]","password");
			sql = sql.Replace("[Password]","Password");
			sql = sql.Replace("[PASSWORD]","PASSWORD");
			sql = sql.Replace("[deleted]","deleted");
			sql = sql.Replace("[Deleted]","Deleted");
			sql = sql.Replace("[DELETED]","DELETED");
			sql = sql.Replace("[published]","published");
			sql = sql.Replace("[Published]","Published");
			sql = sql.Replace("[PUBLISHED]","PUBLISHED");
			sql = sql.Replace("[name]","name");
			sql = sql.Replace("[Name]","Name");
			sql = sql.Replace("[NAME]","NAME");
			sql = sql.Replace("SET CONCAT_NULL_YIELDS_NULL OFF;",""); // not supported
			return sql;
		}

		static public IDataReader GetRS(String sql)
		{
			if(Common.ApplicationBool("DumpSQL"))
			{
				HttpContext.Current.Response.Write("SQL=" + sql + "<br>\n");
			}
			switch(_activeDBProvider)
			{
				case "MSSQL":
					SqlConnection dbconn = new SqlConnection();
					dbconn.ConnectionString = DB.GetDBConn();
					dbconn.Open();
					SqlCommand cmd = new SqlCommand(sql,dbconn);
					return cmd.ExecuteReader(CommandBehavior.CloseConnection);
#if MySQL
				case "MYSQL":
					sql = PreprocessSQLForMySQL(sql);
					MySqlConnection mysql_dbconn = new MySqlConnection();
					mysql_dbconn.ConnectionString = DB.GetDBConn();
					mysql_dbconn.Open();
					MySqlCommand mysql_cmd = new MySqlCommand(sql,mysql_dbconn);
					return mysql_cmd.ExecuteReader(CommandBehavior.CloseConnection);
#endif
				case "MSACCESS":
					// pre-process SQL to convert to use supported MS ACCESS functions:
					sql = PreprocessSQLForAccess(sql);
					OleDbConnection access_dbconn = new OleDbConnection();
					access_dbconn.ConnectionString = DB.GetDBConn();
					access_dbconn.Open();
					OleDbCommand access_cmd = new OleDbCommand(sql,access_dbconn);
					return access_cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
			// JUST to make the compiler happy!
			SqlConnection dbconn4 = new SqlConnection();
			dbconn4.ConnectionString = DB.GetDBConn();
			dbconn4.Open();
			SqlCommand cmd4 = new SqlCommand(sql,dbconn4);
			return cmd4.ExecuteReader(CommandBehavior.CloseConnection);
		}

		static public IDataReader SPGetRS(String procCall)
		{
			if(Common.ApplicationBool("DumpSQL"))
			{
				HttpContext.Current.Response.Write("SP=" + procCall + "<br>\n");
			}
			switch(_activeDBProvider)
			{
				case "MSSQL":
					SqlConnection dbconn = new SqlConnection();
					dbconn.ConnectionString = DB.GetDBConn();
					dbconn.Open();
					SqlCommand cmd = new SqlCommand(procCall, dbconn);
					cmd.CommandType = CommandType.StoredProcedure;
					return cmd.ExecuteReader(CommandBehavior.CloseConnection);
#if MySQL
				case "MYSQL":
					MySqlConnection mysql_dbconn = new MySqlConnection();
					mysql_dbconn.ConnectionString = DB.GetDBConn();
					mysql_dbconn.Open();
					MySqlCommand mysql_cmd = new MySqlCommand(procCall, mysql_dbconn);
					mysql_cmd.CommandType = CommandType.StoredProcedure;
					return mysql_cmd.ExecuteReader(CommandBehavior.CloseConnection);
#endif
				case "MSACCESS":
					OleDbConnection access_dbconn = new OleDbConnection();
					access_dbconn.ConnectionString = DB.GetDBConn();
					access_dbconn.Open();
					OleDbCommand access_cmd = new OleDbCommand(procCall, access_dbconn);
					access_cmd.CommandType = CommandType.StoredProcedure;
					return access_cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
			// just to make the compiler happy:
			SqlConnection dbconn4 = new SqlConnection();
			dbconn4.ConnectionString = DB.GetDBConn();
			dbconn4.Open();
			SqlCommand cmd4 = new SqlCommand(procCall, dbconn4);
			cmd4.CommandType = CommandType.StoredProcedure;
			return cmd4.ExecuteReader(CommandBehavior.CloseConnection);
		}

		static public void ExecuteSQL(String sql)
		{
			if(Common.ApplicationBool("DumpSQL"))
			{
				HttpContext.Current.Response.Write("SQL=" + sql + "<br>\n");
			}
			switch(_activeDBProvider)
			{
				case "MSSQL":
					SqlConnection dbconn = new SqlConnection();
					dbconn.ConnectionString = DB.GetDBConn();
					dbconn.Open();
					SqlCommand cmd = new SqlCommand(sql,dbconn);
					try
					{
						cmd.ExecuteNonQuery();
						cmd.Dispose();
						dbconn.Close();
						dbconn.Dispose();
					}
					catch(Exception ex)
					{
						cmd.Dispose();
						dbconn.Close();
						dbconn.Dispose();
						throw(ex);
					}
					break;
#if MySQL
				case "MYSQL":
					sql = PreprocessSQLForMySQL(sql);
					MySqlConnection mysql_dbconn = new MySqlConnection();
					mysql_dbconn.ConnectionString = DB.GetDBConn();
					mysql_dbconn.Open();
					MySqlCommand mysql_cmd = new MySqlCommand(sql,mysql_dbconn);
					try
					{
						mysql_cmd.ExecuteNonQuery();
						mysql_cmd.Dispose();
						mysql_dbconn.Close();
						mysql_dbconn.Dispose();
					}
					catch(Exception ex)
					{
						mysql_cmd.Dispose();
						mysql_dbconn.Close();
						mysql_dbconn.Dispose();
						throw(ex);
					}
					break;
#endif
				case "MSACCESS":
					sql = PreprocessSQLForAccess(sql);
					OleDbConnection access_dbconn = new OleDbConnection();
					access_dbconn.ConnectionString = DB.GetDBConn();
					access_dbconn.Open();
					OleDbCommand access_cmd = new OleDbCommand(sql,access_dbconn);
					try
					{
						access_cmd.ExecuteNonQuery();
						access_cmd.Dispose();
						access_dbconn.Close();
						access_dbconn.Dispose();
					}
					catch(Exception ex)
					{
						access_cmd.Dispose();
						access_dbconn.Close();
						access_dbconn.Dispose();
						throw(ex);
					}
					break;
			}
			
		}

		// NOTE FOR DB ACCESSOR FUNCTIONS: AdminSite try/catch block is needed until
		// we convert to the new admin page styles. Our "old" db accessors handled empty
		// recordset conditions, so we need to preserve that for the admin site to add 
		// new products/categories/etc...
		//
		// We do not use try/catch on the store site for speed

		// ----------------------------------------------------------------
		//
		// SIMPLE ROW FIELD ROUTINES
		//
		// ----------------------------------------------------------------

		public static String RowField(DataRow row, String fieldname)
		{
			if(_activeDBProvider == "MSSQL")
			{
				if(Common.IsAdminSite)
				{
					try
					{
						if(Convert.IsDBNull(row[fieldname]))
						{
							return String.Empty;
						}
						return Convert.ToString(row[fieldname]);
					}
					catch
					{
						return String.Empty;
					}
				}
				else
				{
					if(Convert.IsDBNull(row[fieldname]))
					{
						return String.Empty;
					}
					return Convert.ToString(row[fieldname]);
				}
			}
			else
			{
				String tmpS = row[fieldname].ToString();
				if(tmpS == null)
				{
					tmpS = String.Empty;
				}
				return tmpS;
			}
		}
		
		public static bool RowFieldBool(DataRow row, String fieldname)
		{
			if(_activeDBProvider == "MSSQL")
			{
				if(Common.IsAdminSite)
				{
					try
					{
						if(Convert.IsDBNull(row[fieldname]))
						{
							return false;
						}
						String s = row[fieldname].ToString().ToLower();
						return (s == "true" || s == "yes" || s == "1");
					}
					catch
					{
						return false;
					}
				}
				else
				{
					if(Convert.IsDBNull(row[fieldname]))
					{
						return false;
					}
					String s = row[fieldname].ToString().ToLower();
					return (s == "true" || s == "yes" || s == "1");
				}
			}
			else
			{
				String tmpS = RowField(row,fieldname).ToLower();
				if(tmpS == "true" || tmpS == "yes" || tmpS == "1")
				{
					return true;
				}
				return false;
			}
		}
		
		public static Byte RowFieldByte(DataRow row, String fieldname)
		{
			if(_activeDBProvider == "MSSQL")
			{
				if(Common.IsAdminSite)
				{
					try
					{
						if(Convert.IsDBNull(row[fieldname]))
						{
							return 0;
						}
						return Convert.ToByte(row[fieldname]);
					}
					catch
					{
						return 0;
					}
				}
				else
				{
					if(Convert.IsDBNull(row[fieldname]))
					{
						return 0;
					}
					return Convert.ToByte(row[fieldname]);
				}
			}
			else
			{
				try
				{
					return System.Byte.Parse(RowField(row,fieldname));
				}
				catch
				{
					return 0;
				}
			}
		}

		public static String RowFieldGUID(DataRow row, String fieldname)
		{
			if(_activeDBProvider == "MSSQL")
			{
				if(Common.IsAdminSite)
				{
					try
					{
						if(Convert.IsDBNull(row[fieldname]))
						{
							return String.Empty;
						}
						return Convert.ToString(row[fieldname]);
					}
					catch
					{
						return String.Empty;
					}
				}
				else
				{
					if(Convert.IsDBNull(row[fieldname]))
					{
						return String.Empty;
					}
					return Convert.ToString(row[fieldname]);
				}
			}
			else
			{
				String tmpS = row[fieldname].ToString();
				if(tmpS == null)
				{
					tmpS = String.Empty;
				}
				return tmpS;
			}
		}

		public static int RowFieldInt(DataRow row, String fieldname)
		{
			if(_activeDBProvider == "MSSQL")
			{
				if(Common.IsAdminSite)
				{
					try
					{
						if(Convert.IsDBNull(row[fieldname]))
						{
							return 0;
						}
						return Convert.ToInt32(row[fieldname]);
					}
					catch
					{
						return 0;
					}
				}
				else
				{
					if(Convert.IsDBNull(row[fieldname]))
					{
						return 0;
					}
					return Convert.ToInt32(row[fieldname]);
				}
			}
			else
			{
				try
				{
					return System.Int32.Parse(RowField(row,fieldname));
				}
				catch
				{
					return 0;
				}
			}
		}

		public static long RowFieldLong(DataRow row, String fieldname)
		{
			if(_activeDBProvider == "MSSQL")
			{
				if(Common.IsAdminSite)
				{
					try
					{
						if(Convert.IsDBNull(row[fieldname]))
						{
							return 0;
						}
						return Convert.ToInt64(row[fieldname]);
					}
					catch
					{
						return 0;
					}
				}
				else
				{
					if(Convert.IsDBNull(row[fieldname]))
					{
						return 0;
					}
					return Convert.ToInt64(row[fieldname]);
				}
			}
			else
			{
				try
				{
					return System.Int64.Parse(RowField(row,fieldname));
				}
				catch
				{
					return 0;
				}
			}
		}

		public static Single RowFieldSingle(DataRow row, String fieldname)
		{
			if(_activeDBProvider == "MSSQL")
			{
				if(Common.IsAdminSite)
				{
					try
					{
						if(Convert.IsDBNull(row[fieldname]))
						{
							return 0.0F;
						}
						return Convert.ToSingle(row[fieldname]);
					}
					catch
					{
						return 0.0F;
					}
				}
				else
				{
					if(Convert.IsDBNull(row[fieldname]))
					{
						return 0.0F;
					}
					return Convert.ToSingle(row[fieldname]);
				}
			}
			else
			{
				try
				{
					return System.Single.Parse(RowField(row,fieldname));
				}
				catch
				{
					return 0.0F;
				}
			}
		}

		public static Double RowFieldDouble(DataRow row, String fieldname)
		{
			if(_activeDBProvider == "MSSQL")
			{
				if(Common.IsAdminSite)
				{
					try
					{
						if(Convert.IsDBNull(row[fieldname]))
						{
							return 0.0F;
						}
						return Convert.ToDouble(row[fieldname]);
					}
					catch
					{
						return 0.0F;
					}
				}
				else
				{
					if(Convert.IsDBNull(row[fieldname]))
					{
						return 0.0F;
					}
					return Convert.ToDouble(row[fieldname]);
				}
			}
			else
			{
				try
				{
					return System.Double.Parse(RowField(row,fieldname));
				}
				catch
				{
					return 0.0F;
				}
			}
		}

		public static Decimal RowFieldDecimal(DataRow row, String fieldname)
		{
			if(_activeDBProvider == "MSSQL")
			{
				if(Common.IsAdminSite)
				{
					try
					{
						if(Convert.IsDBNull(row[fieldname]))
						{
							return System.Decimal.Zero;
						}
						return Convert.ToDecimal(row[fieldname]);
					}
					catch
					{
						return System.Decimal.Zero;
					}
				}
				else
				{
					if(Convert.IsDBNull(row[fieldname]))
					{
						return System.Decimal.Zero;
					}
					return Convert.ToDecimal(row[fieldname]);
				}
			}
			else
			{
				try
				{
					return System.Decimal.Parse(RowField(row,fieldname));
				}
				catch
				{
					return 0.0M;
				}
			}
		}

		
		public static DateTime RowFieldDateTime(DataRow row, String fieldname)
		{
			if(_activeDBProvider == "MSSQL")
			{
				if(Common.IsAdminSite)
				{
					try
					{
						if(Convert.IsDBNull(row[fieldname]))
						{
							return System.DateTime.MinValue;
						}
						return Convert.ToDateTime(row[fieldname]);
					}
					catch
					{
						return System.DateTime.MinValue;
					}
				}
				else
				{
					if(Convert.IsDBNull(row[fieldname]))
					{
						return System.DateTime.MinValue;
					}
					return Convert.ToDateTime(row[fieldname]);
				}
			}
			else
			{
				try
				{
					return Localization.ParseNativeDateTime(row[fieldname].ToString());
				}
				catch
				{
					return System.DateTime.MinValue;
				}
			}
		}

//		public static int RowFieldNativeInt(DataRow row, String fieldname)
//		{
//			String tmpS = RowField(row,fieldname);
//			return Localization.ParseNativeInt(tmpS);
//		}
//
//		public static long RowFieldNativeLong(DataRow row, String fieldname)
//		{
//			String tmpS = RowField(row,fieldname);
//			return Localization.ParseNativeLong(tmpS);
//		}
//
//		public static Single RowFieldNativeSingle(DataRow row, String fieldname)
//		{
//			String tmpS = RowField(row,fieldname);
//			return Localization.ParseNativeSingle(tmpS);
//		}
//
//		public static Decimal RowFieldNativeDecimal(DataRow row, String fieldname)
//		{
//			String tmpS = RowField(row,fieldname);
//			return Localization.ParseNativeDecimal(tmpS);
//		}
//
//		
//		public static DateTime RowFieldNativeDateTime(DataRow row, String fieldname)
//		{
//			String tmpS = RowField(row,fieldname);
//			return Localization.ParseNativeDateTime(tmpS);
//		}

		// ----------------------------------------------------------------
		//
		// SIMPLE RS FIELD ROUTINES
		//
		// ----------------------------------------------------------------

		public static String RSField(IDataReader rs, String fieldname)
		{
			if(_activeDBProvider == "MSSQL")
			{
				if(Common.IsAdminSite)
				{
					try
					{
						int idx = rs.GetOrdinal(fieldname);
						if(rs.IsDBNull(idx))
						{
							return String.Empty;
						}
						return rs.GetString(idx);
					}
					catch
					{
						return String.Empty;
					}
				}
				else
				{
					int idx = rs.GetOrdinal(fieldname);
					if(rs.IsDBNull(idx))
					{
						return String.Empty;
					}
					return rs.GetString(idx);
				}
			}
			else
			{
				String tmpS = String.Empty;
				try
				{
					tmpS = rs[fieldname].ToString();
				}
				catch
				{
					tmpS = String.Empty;
				}
				return tmpS;
			}
		}
		
		public static bool RSFieldBool(IDataReader rs, String fieldname)
		{
			if(_activeDBProvider == "MSSQL")
			{
				if(Common.IsAdminSite)
				{
					try
					{
						int idx = rs.GetOrdinal(fieldname);
						if(rs.IsDBNull(idx))
						{
							return false;
						}
						String s = rs[fieldname].ToString().ToLower();
						return (s == "true" || s == "yes" || s == "1");
					}
					catch
					{
						return false;
					}
				}
				else
				{
					int idx = rs.GetOrdinal(fieldname);
					if(rs.IsDBNull(idx))
					{
						return false;
					}
					String s = rs[fieldname].ToString().ToLower();
					return (s == "true" || s == "yes" || s == "1");
				}
			}
			else
			{
				String tmpS = RSField(rs,fieldname).ToLower();
				if(tmpS == "true" || tmpS == "yes" || tmpS == "1")
				{
					return true;
				}
				return false;
			}
		}

		public static String RSFieldGUID(IDataReader rs, String fieldname)
		{
			if(_activeDBProvider == "MSSQL")
			{
				if(Common.IsAdminSite)
				{
					try
					{
						int idx = rs.GetOrdinal(fieldname);
						if(rs.IsDBNull(idx))
						{
							return String.Empty;
						}
						if(_activeDBProvider == "MSSQL") // MS SQL is the only db to support GUIDS properly
						{
							return rs.GetGuid(idx).ToString();
						}
						else
						{
							return rs.GetString(idx).ToString();
						}
					}
					catch
					{
						return String.Empty;
					}
				}
				else
				{
					int idx = rs.GetOrdinal(fieldname);
					if(rs.IsDBNull(idx))
					{
						return String.Empty;
					}
					if(_activeDBProvider == "MSSQL") // MS SQL is the only db to support GUIDS properly
					{
						return rs.GetGuid(idx).ToString();
					}
					else
					{
						return rs.GetString(idx).ToString();
					}
				}
			}
			else
			{
				String tmpS = String.Empty;
				try
				{
					tmpS = rs[fieldname].ToString();
				}
				catch
				{
					tmpS = String.Empty;
				}
				return tmpS;
			}
		}

		public static Byte RSFieldByte(IDataReader rs, String fieldname)
		{
			if(_activeDBProvider == "MSSQL")
			{
				if(Common.IsAdminSite)
				{
					try
					{
						int idx = rs.GetOrdinal(fieldname);
						if(rs.IsDBNull(idx))
						{
							return 0;
						}
						return rs.GetByte(idx);
					}
					catch
					{
						return 0;
					}
				}
				else
				{
					int idx = rs.GetOrdinal(fieldname);
					if(rs.IsDBNull(idx))
					{
						return 0;
					}
					return rs.GetByte(idx);
				}
			}
			else
			{
				try
				{
					return System.Byte.Parse(RSField(rs,fieldname));
				}
				catch
				{
					return 0;
				}
			}
		}

		public static int RSFieldInt(IDataReader rs, String fieldname)
		{
			if(_activeDBProvider == "MSSQL")
			{
				if(Common.IsAdminSite)
				{
					try
					{
						int idx = rs.GetOrdinal(fieldname);
						if(rs.IsDBNull(idx))
						{
							return 0;
						}
						return rs.GetInt32(idx);
					}
					catch
					{
						return 0;
					}
				}
				else
				{
					int idx = rs.GetOrdinal(fieldname);
					if(rs.IsDBNull(idx))
					{
						return 0;
					}
					return rs.GetInt32(idx);
				}
			}
			else
			{
				try
				{
					return System.Int32.Parse(RSField(rs,fieldname));
				}
				catch
				{
					return 0;
				}
			}
		}

		public static long RSFieldLong(IDataReader rs, String fieldname)
		{
			if(_activeDBProvider == "MSSQL")
			{
				if(Common.IsAdminSite)
				{
					try
					{
						int idx = rs.GetOrdinal(fieldname);
						if(rs.IsDBNull(idx))
						{
							return 0;
						}
						return rs.GetInt64(idx);
					}
					catch
					{
						return 0;
					}
				}
				else
				{
					int idx = rs.GetOrdinal(fieldname);
					if(rs.IsDBNull(idx))
					{
						return 0;
					}
					return rs.GetInt64(idx);
				}
			}
			else
			{
				try
				{
					return System.Int64.Parse(RSField(rs,fieldname));
				}
				catch
				{
					return 0;
				}
			}
		}

		public static Single RSFieldSingle(IDataReader rs, String fieldname)
		{
			if(_activeDBProvider == "MSSQL")
			{
				if(Common.IsAdminSite)
				{
					try
					{
						int idx = rs.GetOrdinal(fieldname);
						if(rs.IsDBNull(idx))
						{
							return 0.0F;
						}
						return (Single)rs.GetDouble(idx); // SQL server seems to fail the GetFloat calls, so we have to do this
					}
					catch
					{
						return 0.0F;
					}
				}
				else
				{
					int idx = rs.GetOrdinal(fieldname);
					if(rs.IsDBNull(idx))
					{
						return 0.0F;
					}
					return (Single)rs.GetDouble(idx); // SQL server seems to fail the GetFloat calls, so we have to do this
				}
			}
			else
			{
				try
				{
					return System.Single.Parse(RSField(rs,fieldname));
				}
				catch
				{
					return 0.0F;
				}
			}
		}

		public static Double RSFieldDouble(IDataReader rs, String fieldname)
		{
			if(_activeDBProvider == "MSSQL")
			{
				if(Common.IsAdminSite)
				{
					try
					{
						int idx = rs.GetOrdinal(fieldname);
						if(rs.IsDBNull(idx))
						{
							return 0.0F;
						}
						return rs.GetDouble(idx);
					}
					catch
					{
						return 0.0F;
					}
				}
				else
				{
					int idx = rs.GetOrdinal(fieldname);
					if(rs.IsDBNull(idx))
					{
						return 0.0F;
					}
					return rs.GetDouble(idx);
				}
			}
			else
			{
				try
				{
					return System.Double.Parse(RSField(rs,fieldname));
				}
				catch
				{
					return 0.0F;
				}
			}
		}

		public static Decimal RSFieldDecimal(IDataReader rs, String fieldname)
		{
			if(_activeDBProvider == "MSSQL")
			{
				if(Common.IsAdminSite)
				{
					try
					{
						int idx = rs.GetOrdinal(fieldname);
						if(rs.IsDBNull(idx))
						{
							return System.Decimal.Zero;
						}
						return rs.GetDecimal(idx);
					}
					catch
					{
						return System.Decimal.Zero;
					}
				}
				else
				{
					int idx = rs.GetOrdinal(fieldname);
					if(rs.IsDBNull(idx))
					{
						return System.Decimal.Zero;
					}
					return rs.GetDecimal(idx);
				}
			}
			else
			{
				try
				{
					return System.Decimal.Parse(RSField(rs,fieldname));
				}
				catch
				{
					return 0.0M;
				}
			}
		}

		public static DateTime RSFieldDateTime(IDataReader rs, String fieldname)
		{
			if(_activeDBProvider == "MSSQL")
			{
				if(Common.IsAdminSite)
				{
					try
					{
						int idx = rs.GetOrdinal(fieldname);
						if(rs.IsDBNull(idx))
						{
							return System.DateTime.MinValue;
						}
						return rs.GetDateTime(idx);
					}
					catch
					{
						return System.DateTime.MinValue;
					}
				}
				else
				{
					int idx = rs.GetOrdinal(fieldname);
					if(rs.IsDBNull(idx))
					{
						return System.DateTime.MinValue;
					}
					return rs.GetDateTime(idx);
				}
			}
			else
			{
				try
				{
					return Localization.ParseNativeDateTime(rs[fieldname].ToString());
				}
				catch
				{
					return System.DateTime.MinValue;
				}
			}
		}

//		public static int RSFieldNativeInt(IDataReader rs, String fieldname)
//		{
//			String tmpS = RSField(rs,fieldname);
//			return Localization.ParseNativeInt(tmpS);
//		}
//
//		public static long RSFieldNativeLong(IDataReader rs, String fieldname)
//		{
//			String tmpS = RSField(rs,fieldname);
//			return Localization.ParseNativeLong(tmpS);
//		}
//
//		public static Single RSFieldNativeSingle(IDataReader rs, String fieldname)
//		{
//			String tmpS = RSField(rs,fieldname);
//			return Localization.ParseNativeSingle(tmpS);
//		}
//
//		public static Decimal RSFieldNativeDecimal(IDataReader rs, String fieldname)
//		{
//			String tmpS = RSField(rs,fieldname);
//			return Localization.ParseNativeDecimal(tmpS);
//		}
//
//		public static DateTime RSFieldNativeDateTime(IDataReader rs, String fieldname)
//		{
//			String tmpS = RSField(rs,fieldname);
//			return Localization.ParseNativeDateTime(tmpS);
//		}



		public static DataSet GetTable(String tablename, String orderBy, String cacheName, bool useCache)
		{
			if(useCache)
			{
				DataSet cacheds = (DataSet)HttpContext.Current.Cache.Get(cacheName);
				if(cacheds != null)
				{
					return cacheds;
				}
			}
			DataSet ds = new DataSet();
			String sql = String.Empty;
			switch(_activeDBProvider)
			{
				case "MSSQL":
					SqlConnection dbconn = new SqlConnection();
					dbconn.ConnectionString = DB.GetDBConn();
					dbconn.Open();
					sql = "select * from " + tablename + " order by " + orderBy;
					SqlDataAdapter da = new SqlDataAdapter(sql,dbconn);
					da.Fill(ds,tablename);
					dbconn.Close();
					break;
#if MySQL
				case "MYSQL":
					MySqlConnection mysql_dbconn = new MySqlConnection();
					mysql_dbconn.ConnectionString = DB.GetDBConn();
					mysql_dbconn.Open();
					sql = "select * from " + tablename + " order by " + orderBy;
					MyIDataAdapter mysql_da = new MyIDataAdapter(sql,mysql_dbconn);
					mysql_da.Fill(ds,tablename);
					mysql_dbconn.Close();
					break;
#endif
				case "MSACCESS":
					OleDbConnection access_dbconn = new OleDbConnection();
					access_dbconn.ConnectionString = DB.GetDBConn();
					access_dbconn.Open();
					sql = "select * from " + tablename + " order by " + orderBy;
					OleDbDataAdapter access_da = new OleDbDataAdapter(sql,access_dbconn);
					access_da.Fill(ds,tablename);
					access_dbconn.Close();
					break;
			}
			if(useCache)
			{
				HttpContext.Current.Cache.Insert(cacheName,ds);
			}
			return ds;
		}

		public static DataSet GetDS(String sql, String cacheName, bool useCache, System.DateTime expiresAt)
		{
			if(Common.ApplicationBool("DumpSQL"))
			{
				HttpContext.Current.Response.Write("SQL=" + sql + "<br>\n");
			}
			if(useCache)
			{
				DataSet cacheds = (DataSet)HttpContext.Current.Cache.Get(cacheName);
				if(cacheds != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return cacheds;
				}
			}
			DataSet ds = new DataSet();
			switch(_activeDBProvider)
			{
				case "MSSQL":
					SqlConnection dbconn = new SqlConnection();
					dbconn.ConnectionString = DB.GetDBConn();
					dbconn.Open();
					SqlDataAdapter da = new SqlDataAdapter(sql,dbconn);
					da.Fill(ds,"Table1");
					dbconn.Close();
					break;
#if MySQL
				case "MYSQL":
					sql = PreprocessSQLForMySQL(sql);
					MySqlConnection mysql_dbconn = new MySqlConnection();
					mysql_dbconn.ConnectionString = DB.GetDBConn();
					mysql_dbconn.Open();
					MyIDataAdapter mysql_da = new MyIDataAdapter(sql,mysql_dbconn);
					mysql_da.Fill(ds,"Table1");
					mysql_dbconn.Close();
					break;
#endif
				case "MSACCESS":
					sql = PreprocessSQLForAccess(sql);
					OleDbConnection access_dbconn = new OleDbConnection();
					access_dbconn.ConnectionString = DB.GetDBConn();
					access_dbconn.Open();
					OleDbDataAdapter access_da = new OleDbDataAdapter(sql,access_dbconn);
					access_da.Fill(ds,"Table1");
					access_dbconn.Close();
					break;
			}
			if(useCache)
			{
				HttpContext.Current.Cache.Insert(cacheName,ds,null,expiresAt,TimeSpan.Zero);
			}
			return ds;
		}

		public static DataSet GetDS(String sql, String cacheName, bool useCache)
		{
			return GetDS(sql,cacheName,useCache,System.DateTime.Now.AddHours(1));
		}

		public static DataSet GetDS(String sql, bool useCache, System.DateTime expiresAt)
		{
			return GetDS(sql,sql.ToUpper(),useCache,expiresAt);
		}

		public static DataSet GetDS(String sql, bool useCache)
		{
			return GetDS(sql,useCache,System.DateTime.Now.AddHours(1));
		}

		public static String GetNewGUID()
		{
			return System.Guid.NewGuid().ToString();
		}

		static public int GetSqlN(String sql)
		{
			int N = 0;
			IDataReader rs = DB.GetRS(sql);
			if(rs.Read())
			{
				N = DB.RSFieldInt(rs,"N");
			}
			rs.Close();
			return N;
		}

		static public Single GetSqlNSingle(String sql)
		{
			Single N = 0.0F;
			IDataReader rs = DB.GetRS(sql);
			if(rs.Read())
			{
				N = DB.RSFieldSingle(rs,"N");
			}
			rs.Close();
			return N;
		}

		static public decimal GetSqlNDecimal(String sql)
		{
			decimal N = System.Decimal.Zero;
			IDataReader rs = DB.GetRS(sql);
			if(rs.Read())
			{
				N = DB.RSFieldDecimal(rs,"N");
			}
			rs.Close();
			return N;
		}

		static public void ExecuteLongTimeSQL(String sql, int TimeoutSecs)
		{
			ExecuteLongTimeSQL(sql,DB.GetDBConn(),TimeoutSecs);
		}

		static public void ExecuteLongTimeSQL(String sql, String DBConnString, int TimeoutSecs)
		{
			if(Common.ApplicationBool("DumpSQL"))
			{
				HttpContext.Current.Response.Write("SQL=" + sql + "<br>\n");
			}
			switch(_activeDBProvider)
			{
				case "MSSQL":
					SqlConnection dbconn = new SqlConnection();
					dbconn.ConnectionString = DB.GetDBConn();
					dbconn.Open();
					SqlCommand cmd = new SqlCommand(sql,dbconn);
					cmd.CommandTimeout = TimeoutSecs;
					try
					{
						cmd.ExecuteNonQuery();
						cmd.Dispose();
						dbconn.Close();
						dbconn.Dispose();
					}
					catch(Exception ex)
					{
						cmd.Dispose();
						dbconn.Close();
						dbconn.Dispose();
						throw(ex);
					}
					break;
#if MySQL
				case "MYSQL":
					sql = PreprocessSQLForMySQL(sql);
					MySqlConnection mysql_dbconn = new MySqlConnection();
					mysql_dbconn.ConnectionString = DB.GetDBConn();
					mysql_dbconn.Open();
					MySqlCommand mysql_cmd = new MySqlCommand(sql,mysql_dbconn);
					mysql_cmd.CommandTimeout = TimeoutSecs;
					try
					{
						mysql_cmd.ExecuteNonQuery();
						mysql_cmd.Dispose();
						mysql_dbconn.Close();
						mysql_dbconn.Dispose();
					}
					catch(Exception ex)
					{
						mysql_cmd.Dispose();
						mysql_dbconn.Close();
						mysql_dbconn.Dispose();
						throw(ex);
					}
					break;
#endif
				case "MSACCESS":
					sql = PreprocessSQLForAccess(sql);
					OleDbConnection access_dbconn = new OleDbConnection();
					access_dbconn.ConnectionString = DB.GetDBConn();
					access_dbconn.Open();
					OleDbCommand access_cmd = new OleDbCommand(sql,access_dbconn);
					access_cmd.CommandTimeout = TimeoutSecs;
					try
					{
						access_cmd.ExecuteNonQuery();
						access_cmd.Dispose();
						access_dbconn.Close();
						access_dbconn.Dispose();
					}
					catch(Exception ex)
					{
						access_cmd.Dispose();
						access_dbconn.Close();
						access_dbconn.Dispose();
						throw(ex);
					}
					break;
			}
			
		}
		
	}

	// currently only supported for SQL Server:
	public class DBTransaction
	{
		private ArrayList sqlCommands = new ArrayList(10);

		public DBTransaction()	{}

		public void AddCommand(String sql)
		{
			sqlCommands.Add(sql);
		}

		public void ClearCommands()
		{
			sqlCommands.Clear();
		}

		// returns true if no errors, or false if ANY Exception is found:
		public bool Commit()
		{
			SqlConnection conn = new SqlConnection(); 
			conn.ConnectionString = DB.GetDBConn();
			conn.Open();
			SqlTransaction trans = conn.BeginTransaction();
			bool status = false;
			try
			{	
	
				foreach(String s in sqlCommands)
				{
					SqlCommand comm = new SqlCommand(s,conn);
					comm.Transaction = trans;
					comm.ExecuteNonQuery();
				}
				trans.Commit();
				status = true;
			}
			catch //(SqlException ex)
			{
				trans.Rollback();
				status = false;
			}
			finally
			{
				conn.Close();
			}
			return status;
		}

	}
}
