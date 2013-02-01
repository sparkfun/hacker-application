// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// $Header: /v5.0/AspDotNetStorefront/ASPDNSFCommon/XmlPackage.cs 30    4/24/05 5:28p Administrator $
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.IO;
using System.Collections;
using System.Security;
using System.Security.Policy;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontCommon
{

	/// <summary>
	/// Summary description for XmlPackage.
	/// </summary>
	public class XmlPackage
	{

		// Parses tokens of the form "(!command attribute="value" attribute2='value' ..!)"
		private Regex m_CmdMatch = new Regex("\\(\\!(\\w+)(?:\\s(?:(\\w*)=(?:'|\")(.*?)(?:\"|'))?)*\\!\\)");
		private MatchEvaluator m_CmdMatchEval;
    
		private static readonly string XslNameSpace = "http://www.w3.org/1999/XSL/Transform";
		private static readonly string AspdnsfNameSpace = "http://www.aspdotnetstorefront.com";

		string m_xslName = String.Empty;
		string m_xslUrl = String.Empty;
		Customer m_ThisCustomer = null;
		int m_SiteID = 1;

		XmlDocument m_Document = null;
		string m_DocumentSource = String.Empty;

		XmlDocument m_TransformDocument = null;
		//XmlNamespaceManager m_TransformNameSpaceManager = null;
		XslTransform m_Transform = null;
		XsltArgumentList m_TransformArgumentList = null;
		string m_TransformSource = String.Empty;

		DataSet m_Data = null;

		string m_SectionTitle = String.Empty;

		//CONSTRUCTORS
		public XmlPackage(string XslName, Customer ThisCustomer, int SiteID)
		{
			m_CmdMatchEval = new MatchEvaluator(CommandMatchEvaluator);
			m_xslName = XslName;
			m_ThisCustomer = ThisCustomer;
			m_SiteID = SiteID;
			m_xslUrl = FullXslUrl(m_xslName,m_SiteID);
			TransformSource = HttpContext.Current.Server.MapPath(m_xslUrl);
		}

		//PROPERTIES

		/// <summary>
		/// Gets or Sets by an XmlDocument for this XmlPackage
		/// </summary>
		public XmlDocument Document
		{
			get
			{
				if (m_Document == null)
				{
					m_Document = new XmlDocument();
				}
				return m_Document;
			}
			set
			{
				m_Document = value;
				m_DocumentSource = String.Empty;
			}
		}

		/// <summary>
		/// Gets or Sets by an Xml string the Xml document for this XmlPackage
		/// </summary>
		public string DocumentContent
		{
			get
			{
				return Document.OuterXml;
			}
			set
			{
				Document.LoadXml(value);
			}
		}

		/// <summary>
		/// gets or Sets the Path to the Xml document for this XmlPackage
		/// </summary>
		public string DocumentSource
		{
			get
			{
				return m_DocumentSource;
			}
			set
			{
				m_DocumentSource = value;
				Document.Load(m_DocumentSource);
			}
		}

		/// <summary>
		/// gets or Sets the XslTransform info for this XmlPackage
		/// </summary>
		public XslTransform Transform
		{
			get
			{
				if (m_Transform == null)
				{
					m_Transform = new XslTransform();
					m_TransformSource = String.Empty;
				}
				return m_Transform;
			}
			set
			{
				m_Transform = value;
				m_TransformSource = String.Empty;
			}
		}

		/// <summary>
		/// Gets or Sets by an XmlDocument for this XmlPackage
		/// </summary>
		public XmlDocument TransformDocument
		{
			get
			{
				if (m_TransformDocument == null)
				{
					m_TransformDocument = new XmlDocument();
				}
				return m_TransformDocument;
			}
			set
			{
				m_TransformDocument = value;
			}
		}

		/// <summary>
		/// gets or Sets the XsltArgumentList info for this XmlPackage
		/// </summary>
		public XsltArgumentList TransformArgumentList
		{
			get
			{
				if (m_TransformArgumentList == null)
				{
					m_TransformArgumentList = new XsltArgumentList();
				}
				return m_TransformArgumentList;
			}
			set
			{
				m_TransformArgumentList = value;
			}
		}

		/// <summary>
		/// gets or Sets the Path to the Xml document for this XmlPackage
		/// </summary>
		public string TransformSource
		{
			get
			{
				return m_TransformSource;
			}
			set
			{
				m_TransformSource = value;
				if (File.Exists(m_TransformSource))
				{
					TransformDocument.Load(m_TransformSource);
				}
				else
				{
					TransformDocument = null;
				}
			}
		}

		/// <summary>
		/// gets or Sets the Customer info for this XmlPackage
		/// </summary>
		public Customer Customer
		{
			get
			{
				return m_ThisCustomer;
			}
			set
			{
				m_ThisCustomer = value;
			}
		}

		/// <summary>
		/// Gets or sets the SiteID for this package
		/// </summary>
		public int SiteID
		{
			get
			{
				return m_SiteID;
			}
			set
			{
				m_SiteID = value;
			}
		}
    
		/// <summary>
		/// gets or Sets the XsltArgumentList info for this XmlPackage
		/// </summary>
		public DataSet Data
		{
			get
			{
				if (m_Data == null)
				{
					m_Data = new DataSet("root");
				}
				return m_Data;
			}
		}

		public string SectionTitle
		{
			get
			{
				return m_SectionTitle;
			}
			set
			{
				m_SectionTitle = value;
			}
		}

		//METHODS
    
		public string FullXslUrl(string XslName, int SkinID)
		{
			string rootUrl =  String.Format("skins/Skin_{0}/XmlPackages/", SkinID);
			string url = String.Empty;
			url = Path.Combine(rootUrl,String.Format("{0}.{1}.xslt",XslName,Customer.LocaleSetting));
			if (!Common.FileExists(url))
			{
				url = Path.Combine(rootUrl,String.Format("{0}.{1}.xslt",XslName,Localization.GetWebConfigLocale()));
			}
			if (!Common.FileExists(url))
			{
				url = Path.Combine(rootUrl,String.Format("{0}.xslt",XslName));
			}
			return url.Replace(@"\","/");
		}

		/// <summary>
		/// Evaluates (!!) tokens and replaces them with correct command output
		/// </summary>
		protected String CommandMatchEvaluator(Match match)
		{
			string cmd = match.Groups[1].Value; // The command string

			Hashtable parameters = new Hashtable(new CaseInsensitiveHashCodeProvider(),new CaseInsensitiveComparer());

			for (int i=0;i < match.Groups[2].Captures.Count;i++)
			{
				string attr = match.Groups[2].Captures[i].Value;
				if (attr == null) attr=String.Empty;
        
				string val = match.Groups[3].Captures[i].Value;
				if (val == null) val = String.Empty;

				parameters.Add(attr,val);   
			}
			return DispatchCommand(cmd,parameters);
		}

		/// <summary>
		/// Takes command string and parameters and returns the result string of the command.
		/// </summary>
		public string DispatchCommand(string command, Hashtable parameters)
		{
			string result = "(!" + command + "!)";
			command = command.ToLower().Replace("username","user_name");
			switch (command.ToLower())
			{
					// Strings
				case "skinid":
				{
					result = this.SiteID.ToString();
					break;
				}
				case "customerid":
				{
					result = this.Customer.CustomerID.ToString(); 
					break;
				}
				case "user_name":
				{
					if (this.Customer.IsAnon)
					{
						result = String.Empty;
					}
					else
					{
						result = this.Customer.FullName(); 
					}
					break;
				}
				case "user_info":
				{
					if (this.Customer.IsAnon)
					{
						result = Common.GetLoginBox(this.SiteID);
					}
					else
					{
						result = Common.GetUserBox(Customer,this.SiteID); 
					}
					break;
				}
				case "user_menu_name":
				{
					if (this.Customer.IsAnon)
					{
						result = "myAccount";
					}
					else
					{
						result = Customer.FullName(); 
					}
					break;
				}
				case "store_version":
				{
					result = Common.AppConfig("StoreVersion");
					break;
				}


					//Links
				case "manufacturerlink":
				{
					result =  SE.MakeManufacturerLink(Int32.Parse((string)parameters["ManufacturerID"]),(string)parameters["SEName"]);
					break;
				}
				case "categorylink":
				{
					result =  SE.MakeCategoryLink(Int32.Parse((string)parameters["CategoryID"]),(string)parameters["SEName"]);
					break;
				}
				case "sectionlink":
				{
					result =  SE.MakeSectionLink(Int32.Parse((string)parameters["SectionID"]),(string)parameters["SEName"]);
					break;
				}
				case "productlink":
				{
					result =  SE.MakeProductLink(Int32.Parse((string)parameters["ProductID"]),(string)parameters["SEName"]);
					break;
				}
				case "productandcategorylink":
				{
					result =  SE.MakeProductAndCategoryLink(Int32.Parse((string)parameters["ProductID"]),Int32.Parse((string)parameters["CategoryID"]),(string)parameters["SEName"]);
					break;
				}
				case "productandsectionlink":
				{
					result =  SE.MakeProductAndSectionLink(Int32.Parse((string)parameters["ProductID"]),Int32.Parse((string)parameters["SectionID"]),(string)parameters["SEName"]);
					break;
				}
				case "topiclink":
				{
					string sTID = (string)parameters["id"];
					string sTName = (string)parameters["Name"];

					if (sTID != null && sTID.Length!=0)
					{
						int tid = Int32.Parse(sTName);
						Topic t = new Topic(tid);
						result = t.Contents;
					}
					else
					{
						if (sTName != null && sTName.Length !=0)
						{
							Topic t = new Topic(sTName);
							result = t.Contents;
						}
					}
					break;
				}
					//Forms
				case "loginoutprompt":
				{
					result = Common.GetLoginBox(SiteID);
					break;
				}

				case "searchbox":
				{
					result = Common.GetSearchBox(SiteID);
					break;
				}

				case "helpbox":
				{
					result = Common.GetHelpBox(SiteID,true);
					break;
				}
				case "addtocartform":
				{
					result = Common.GetAddToCartForm(false,Common.AppConfigBool("ShowWishButtons"),Int32.Parse((string)parameters["ProductID"]),Int32.Parse((string)parameters["VariantID"]),SiteID,Int32.Parse((string)parameters["DisplayFormat"]),true);
					break;
				}
			}
			return result;
		}

 
		/// <summary>
		/// Performs the Transform and returns the resulting string.
		/// </summary>
		public string TransformString()
		{
			InitializeSystemData();
			ParseTransformDocument();


			if (Common.AppConfigBool("Xml.DumpTransform"))
			{
				StreamWriter sw = File.CreateText(HttpContext.Current.Server.MapPath(String.Format("images/{0}.xml",this.m_xslName))); 
				sw.WriteLine(Data.GetXml());
				sw.Close();
				//doc.Save(HttpContext.Current.Server.MapPath(String.Format("images/{0}.xml",this.m_xslName)));
			}
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(Data.GetXml());
			//      XPathDocument doc = new XPathDocument(new StringReader(Common.XmlDecode(Data.GetXml())));  
      
			//      XmlTextReader txtReader = new XmlTextReader(new StringReader(XslString));
			//      txtReader.WhitespaceHandling = WhitespaceHandling.None;

			//      XslTransform xsl = new XslTransform();
			StringWriter writer = new StringWriter();
			Evidence evidence = this.GetType().Assembly.Evidence;
			if (TransformDocument.FirstChild == null)
			{
				return String.Format("<div>Unable to load XmlPackage: {0}</div>",TransformSource);
			}
			try
			{
				Transform.Load(TransformDocument,null,evidence);
				Transform.Transform(doc,null,writer,null);
			}
			catch
			{
			}
			string result = writer.ToString(); 
			if (Common.AppConfigBool("Xml.DumpTransform"))
			{
				//doc.Save(HttpContext.Current.Server.MapPath(String.Format("images/{0}.xml",this.m_xslName)));
				StreamWriter sw = File.CreateText(HttpContext.Current.Server.MapPath(String.Format("images/{0}.xfrm.xml",this.m_xslName))); 
				sw.WriteLine(result);
				sw.Close();
			}
			result = ReplaceTokens(result);
			if (Common.AppConfigBool("Xml.DumpTransform"))
			{
				StreamWriter sw = File.CreateText(HttpContext.Current.Server.MapPath(String.Format("images/{0}.tkn.xml",this.m_xslName))); 
				sw.WriteLine(result);
				sw.Close();
			}        
			return result;
		}

		/// <summary>
		/// Replace the (!!) tokens in a string
		/// </summary>
		/// <param name="aString">String have tokens replaced in.</param>
		/// <returns>The modified string</returns>
		public string ReplaceTokens(string aString)
		{
			return m_CmdMatch.Replace(aString,m_CmdMatchEval);
		}

		/// <summary>
		/// Initializes the Dataset for XML by Adding the System table values to an empty DataSet
		/// </summary>
		public void InitializeSystemData()
		{
			Data.Clear();
			DataTable system = new DataTable("System");
			Data.Tables.Add(system);
			system.Columns.Add("CustomerID",typeof(Int32));
			system.Columns.Add("CustomerLevelID",typeof(Int32));
			system.Columns.Add("CustomerFirstName",typeof(String));
			system.Columns.Add("CustomerLastName",typeof(String));
			system.Columns.Add("CustomerLevelName",typeof(String));
			system.Columns.Add("IsAdminUser",typeof(bool));
			system.Columns.Add("IsSuperUser",typeof(bool));
			system.Columns.Add("LocaleSetting",typeof(String));
			system.Columns.Add("WebConfigLocaleSetting",typeof(String));
			system.Columns.Add("Date",typeof(String));
			system.Columns.Add("Time",typeof(String));
			system.Columns.Add("SkinID",typeof(Int32));
			system.Columns.Add("AffiliateID",typeof(Int32));
			system.Columns.Add("IPAddress",typeof(String));
			system.Columns.Add("QueryString",typeof(String));
			system.Columns.Add("UseStaticLinks",typeof(bool));

			DataRow dr = system.NewRow();
			dr["CustomerID"] = Customer.CustomerID;
			dr["CustomerLevelID"] = m_ThisCustomer.CustomerLevelID;
			dr["CustomerFirstName"] = SecurityElement.Escape(m_ThisCustomer.FirstName);
			dr["CustomerLastName"] = SecurityElement.Escape(m_ThisCustomer.LastName);
			dr["CustomerLevelName"] = SecurityElement.Escape(m_ThisCustomer.CustomerLevelName);
			dr["IsAdminUser"] = m_ThisCustomer.IsAdminUser;
			dr["IsSuperUser"] = m_ThisCustomer.IsAdminSuperUser;
			dr["LocaleSetting"] = m_ThisCustomer.LocaleSetting;
			dr["WebConfigLocaleSetting"] = Localization.GetWebConfigLocale();
			dr["Date"] = DateTime.Now.ToShortDateString();
			dr["Time"] = DateTime.Now.ToShortTimeString();
			dr["SkinID"] = SiteID;
			dr["AffiliateID"] = m_ThisCustomer.AffiliateID;
			dr["IPAddress"] = m_ThisCustomer.LastIPAddress;
			dr["QueryString"] = SecurityElement.Escape(HttpContext.Current.Request.QueryString.ToString());
			dr["UseStaticLinks"] = Common.AppConfigBool("UseStaticLinks");
			system.Rows.Add(dr);
		}

		/// <summary>
		/// Parses the Transform Document to extract queries and parameters and perform them.
		/// </summary>
		public void ParseTransformDocument()
		{
			XmlNamespaceManager nsmgr = new XmlNamespaceManager(TransformDocument.NameTable);
			nsmgr.AddNamespace("xsl",XslNameSpace);
			nsmgr.AddNamespace("aspdnsf",AspdnsfNameSpace);

			XmlNodeList packageNodes = TransformDocument.SelectNodes("//aspdnsf:xmlpackage",nsmgr);
			foreach (XmlNode xNode in packageNodes)
			{
				AddPackageNode(xNode);
			}

			XmlNodeList aspdnsfNodes = TransformDocument.SelectNodes("//aspdnsf:*",nsmgr);
			foreach (XmlNode xNode in aspdnsfNodes)
			{
				AddAspdnsfNode(xNode);
			}

			XmlNodeList queryNodes = TransformDocument.SelectNodes("//aspdnsf:sqlquery",nsmgr);
			XmlNodeList paramNodes = TransformDocument.SelectNodes("//xsl:param[@aspdnsf:*]",nsmgr);

			foreach (XmlNode qNode in queryNodes)
			{
				String sqlQuery = qNode.InnerText;
				String tableName = qNode.Attributes["name"].InnerText;
      
				foreach (XmlNode pNode in paramNodes)
				{
					sqlQuery = AddParameterNode(pNode,sqlQuery);
				}
				DB.FillDataSet(Data,tableName,sqlQuery);
			}
		}

		public void AddPackageNode(XmlNode xNode)
		{
			string packageName = xNode.Attributes["name"].InnerText;
			XmlPackage xp = new XmlPackage(packageName,Customer,SiteID);
			XmlDocument xNew = new XmlDocument();
			xNew.LoadXml(xp.TransformString());
			XmlNode xNewChild = xNode.OwnerDocument.ImportNode(xNew.DocumentElement,true);
			xNode.ParentNode.ReplaceChild(xNewChild,xNode);
		}

		public void AddAspdnsfNode(XmlNode aNode)
		{
			string cmd = aNode.LocalName;

			Hashtable parameters = new Hashtable(new CaseInsensitiveHashCodeProvider(),new CaseInsensitiveComparer());
      
			for (int i=0;i < aNode.Attributes.Count;i++)
			{
				string attr = aNode.Attributes[i].LocalName;
				if (attr == null) attr=String.Empty;
        
				string val = aNode.Attributes[i].Value;
				if (val == null) val = String.Empty;

				parameters.Add(attr,val);   
			}
			string result = DispatchCommand(cmd,parameters);
			XmlDocument aNew = new XmlDocument();
			try
			{
				aNew.LoadXml(result);
				XmlNode aNewChild = aNode.OwnerDocument.ImportNode(aNew.DocumentElement,true);
				aNode.ParentNode.ReplaceChild(aNewChild,aNode);
			}
			catch
			{
			}
		}

		public string AddParameterNode(XmlNode pNode, string sqlQuery)
		{
			string paramName = String.Empty;
			string paramValue = String.Empty;

			if (pNode.Attributes["aspdnsf:appconfig"] != null)
			{
				paramName = pNode.Attributes["aspdnsf:appconfig"].InnerText; //Get the parameter Name
				paramValue = Common.AppConfig(pNode.Attributes["aspdnsf:appconfig"].InnerText);
				pNode.InnerText = paramValue;
			}

			if (pNode.Attributes["aspdnsf:params"] != null)
			{
				paramName = pNode.Attributes["aspdnsf:params"].InnerText; //Get the parameter Name
				paramValue = Common.Params(pNode.Attributes["aspdnsf:params"].InnerText);
				pNode.InnerText = paramValue;
			}

			sqlQuery = sqlQuery.Replace("{"+paramName+"}",paramValue); //Get the value from Params
			return sqlQuery;
		}

	}
  
	//  /// <summary>
	//  /// A generic XSL Transformation Class for use in ASP.NET pages
	//  /// </summary>
	//  public class XsltTransform 
	//  {
	//
	//    public static string TransformXml(String xmlPath, String xsltPath, Hashtable xsltParams, Hashtable xsltObjects) 
	//    {
	//      StringBuilder sb = new StringBuilder();
	//      StringWriter sw = new StringWriter(sb);
	//      try 
	//      {
	//        XPathDocument doc = new XPathDocument(xmlPath);
	//        XsltArgumentList args = new XsltArgumentList();
	//        XslTransform xslDoc = new XslTransform();
	//        xslDoc.Load(xsltPath);
	//
	//        //Fill XsltArgumentList if necessary
	//        if (xsltParams != null) 
	//        {
	//          IDictionaryEnumerator pEnumerator = 
	//            xsltParams.GetEnumerator();
	//          while (pEnumerator.MoveNext()) 
	//          {
	//            args.AddParam(pEnumerator.Key.ToString(),"",
	//              pEnumerator.Value);
	//          }
	//        }
	//        if (xsltObjects != null) 
	//        {
	//          IDictionaryEnumerator pEnumerator =
	//            xsltObjects.GetEnumerator();
	//          while (pEnumerator.MoveNext()) 
	//          {
	//            args.AddExtensionObject(pEnumerator.Key.ToString(),
	//              pEnumerator.Value);
	//          }
	//        }
	//        // TBD!! xslDoc.Transform(doc,args,sw);
	//        return sb.ToString();
	//      } 
	//      catch (Exception exp) 
	//      {
	//        return exp.ToString();
	//      } 
	//      finally 
	//      {
	//        sw.Close();
	//      }
	//    }
	//  }
}
