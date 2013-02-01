//#define MySQL
//#defene DBXML
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
using System.Xml;
using System.IO;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.OleDb;
using System.Globalization;
#if DBXML
using Microsoft.Data.SqlXml;
#endif
#if MySQL
using MySql.Data.MySqlClient;
#endif

namespace AspDotNetStorefrontCommon
{
	/// <summary>
	/// Summary description for DBXml.
	/// </summary>
	public class DBXml
	{
		public DBXml()	{}

#if DBXML

		/// <summary>
		/// Resets the Provider paramter in a connection string
		/// </summary>
		/// <param name="sConn">original connection string</param>
		/// <param name="sProvider">new provider</param>
		/// <returns></returns>
		static private string SetProvider(string sConn, string sProvider)
		{
			string[] sPhrases; // phrases in the string
			char[] sDelimiters = new char[] {';'};
			StringBuilder sConnOut = new StringBuilder(); // working copy of the result
			
			// Start the string builder with the provider
			sConnOut.Append ("Provider=");
			sConnOut.Append (sProvider);

			sPhrases = sConn.Split(sDelimiters);

			foreach (string sPhrase in sPhrases)
			{
				// If it's not one of the clauses that are handeled automatically, add it.					
				if ( ! (sPhrase.ToLower().Trim().StartsWith ("provider=")
					|| sPhrase.ToLower().Trim().StartsWith("data provider=")))
				{
					sConnOut.Append (';');
					sConnOut.Append (sPhrase); 	// keep the clause
				}
			}

			// If the provider is SQLXMLOLEDB, we must add the Data provider
			if (sProvider.ToLower().Trim().Equals("sqlxmloledb"))
			{
				sConnOut.Append (';');
				sConnOut.Append ("Data Provider=SQLOLEDB.1");
			}

			return sConnOut.ToString();
		}

		static public DataSet GetXmlDSFromTemplate(String TemplateURL)
		{
			DataSet ds = new  DataSet();
			ds.ReadXml(TemplateURL, XmlReadMode.InferSchema);
			return ds;
		}

		static public String GetXml(String XmlSql, SqlXmlCommandType cmdType, int InitialBufferSize, String DBConn)
		{
			String result = String.Empty;
			if(Common.ApplicationBool("DumpSQL"))
			{
				HttpContext.Current.Response.Write("XmlSQL=" + XmlSql + "<br>\n");
			}
			switch(DB.GetDBProvider())
			{
				case "MSSQL":
					SqlXmlCommand cmd = new SqlXmlCommand(DBConn);
					cmd.CommandType = cmdType;
					cmd.RootTag = "root";
					cmd.CommandText = XmlSql;
					//cmd.OutputEncoding = "utf-8";
					MemoryStream objResultStream = (MemoryStream)cmd.ExecuteStream();
					// The reader is needed for Dot Net Streams to retrieve the output
					StreamReader objResultStreamReader = new StreamReader(objResultStream);
					// Send all the xml to the console.
					result = objResultStreamReader.ReadToEnd();
					break;
#if MySQL
				case "MYSQL":
					result = "Xml DB Queries No Supported";
					break;
#endif
				case "MSACCESS":
					result = "Xml DB Queries No Supported";
					break;
			}
			return result;
		}

		static public String GetXMLDBConn()
		{
			return SetProvider(DB.GetDBConn(),"SQLOLEDB.1");
		}
		
		static public String GetXml(String XmlSql)
		{
			return GetXml(XmlSql,SqlXmlCommandType.Sql,8*1024,GetXMLDBConn());
		}

		static public String GetXml(String XmlSql, SqlXmlCommandType cmdType)
		{
			return GetXml(XmlSql,cmdType,8*1024,GetXMLDBConn());
		}

		static public String GetXml(String XmlSql, String DBConn)
		{
			return GetXml(XmlSql,SqlXmlCommandType.Sql,8*1024, DBConn);
		}

		static public String GetXml(String XmlSql, SqlXmlCommandType cmdType, String DBConn)
		{
			return GetXml(XmlSql,cmdType,8*1024, DBConn);
		}

		static public String GetXml(String XmlSql, int InitialBufferSize)
		{
			return GetXml(XmlSql,SqlXmlCommandType.Sql, InitialBufferSize,GetXMLDBConn());
		}

		static public String GetXml(String XmlSql, SqlXmlCommandType cmdType, int InitialBufferSize)
		{
			return GetXml(XmlSql,cmdType, InitialBufferSize, GetXMLDBConn());
		}

		static public String GetXml(String XmlSql, int InitialBufferSize, String DBConn)
		{
			return GetXml(XmlSql,SqlXmlCommandType.Sql, InitialBufferSize, DBConn);
		}
#endif
		
	}
}
