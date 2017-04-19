//#define SMTPDOTNET
// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// $Header: /v5.0/AspDotNetStorefront/ASPDNSFCommon/XmlCommon.cs 4     4/24/05 2:10p Redwoodtree $
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Configuration;
using System.Web.SessionState;
using System.Web.Caching;
using System.Web.Util;
using System.Data;
using System.Text;
using System.Collections;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.Resources;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Security;

namespace AspDotNetStorefrontCommon
{
	/// <summary>
	/// Summary description for XmlCommon.
	/// </summary>
	public class XmlCommon
	{
		public XmlCommon() {}

		static public String SerializeObject(Object pObject, System.Type objectType) 
		{
			try 
			{
				String XmlizedString = null;
				MemoryStream memoryStream = new MemoryStream();
				XmlSerializer xs = new XmlSerializer(objectType);
				XmlTextWriter XmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
				xs.Serialize(XmlTextWriter, pObject);
				memoryStream = (MemoryStream)XmlTextWriter.BaseStream;
				XmlizedString = Common.UTF8ByteArrayToString(memoryStream.ToArray());
				return XmlizedString;
			}
			catch (Exception ex)
			{
				return Common.GetExceptionDetail(ex,"\n");
			}
		} 

		static public String FormatXml(XmlDocument inputXml)
		{
			StringWriter writer = new StringWriter();
			XmlTextWriter XmlWriter = new XmlTextWriter(writer);
			XmlWriter.Formatting = Formatting.Indented;
			XmlWriter.Indentation = 2;
			inputXml.WriteTo(XmlWriter);
			return writer.ToString();
		}

		public static String PrettyPrintXml(String Xml)
		{
			String Result = "";

			Xml = Xml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>","");


			MemoryStream MS = new MemoryStream();
			XmlTextWriter W = new XmlTextWriter(MS, Encoding.Unicode);
			XmlDocument D   = new XmlDocument();

			// Load the XmlDocument with the Xml.
			D.LoadXml(Xml);

			W.Formatting = Formatting.Indented;

			// Write the Xml into a formatting XmlTextWriter
			D.WriteContentTo(W);
			W.Flush();
			MS.Flush();

			// Have to rewind the MemoryStream in order to read
			// its contents.
			MS.Position = 0;

			// Read MemoryStream contents into a StreamReader.
			StreamReader SR = new StreamReader(MS);

			// Extract the text from the StreamReader.
			String FormattedXml = SR.ReadToEnd();

			Result = FormattedXml;

			try
			{
				MS.Close();
				MS = null;
				W.Close();
				W = null;
			}
			catch {}

			return Result;
		}

		// strips illegal Xml characters:
		static public String XmlEncode(String S)
		{
			S=Regex.Replace(S,@"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]","");
			StringWriter sw = new StringWriter();
			XmlTextWriter xwr = new XmlTextWriter(sw);
			xwr.WriteString(S);
			String sTmp = sw.ToString();
			xwr.Close();
			sw.Close();
			return sTmp;
		}

		// leaves whatever data is there, and just XmlEncodes it:
		static public String XmlEncodeAsIs(String S)
		{
			StringWriter sw = new StringWriter();
			XmlTextWriter xwr = new XmlTextWriter(sw);
			xwr.WriteString(S);
			String sTmp = sw.ToString();
			xwr.Close();
			sw.Close();
			return sTmp;
		}

		static public String XmlDecode(String S)
		{
			StringBuilder tmpS = new StringBuilder(S);
			String sTmp = tmpS.Replace("&quot;","\"").Replace("&apos;","'").Replace("&lt;","<").Replace("&gt;",">").Replace("&amp;","&").ToString();
			return sTmp;
		}


		// ----------------------------------------------------------------
		//
		// SIMPLE Xml FIELD ROUTINES
		//
		// ----------------------------------------------------------------

		public static String GetLocaleEntry(String S, String LocaleSetting, bool fallBack)
		{
			String tmpS = String.Empty;
			if(S.Length == 0)
			{
				return tmpS;
			}
			if(S.StartsWith("<ml>"))
			{
				LocaleSetting = LocaleSetting;
				String WebConfigLocale = Localization.GetWebConfigLocale();
				if(Common.AppConfigBool("UseXmlDOMForLocaleExtraction"))
				{
					try
					{
						XmlDocument doc = new XmlDocument();
						doc.LoadXml(S);
						XmlNode node = doc.DocumentElement.SelectSingleNode("//locale[@name=\"" + LocaleSetting + "\"]");
						if (fallBack && (node == null))
						{
							node = doc.DocumentElement.SelectSingleNode("//locale[@name=\"" + LocaleSetting + "\"]");
						}
						if(node != null)
						{
							tmpS = node.InnerText;
						}
						if(tmpS.Length != 0)
						{
							tmpS = XmlCommon.XmlDecode(tmpS);
						}
					}
					catch {}
				}
				else
				{
					// for speed, we are using lightweight simple string token extraction here, not full Xml DOM for speed
					// return what is between <locale name=\"en-US\">...</locale>, Xml Decoded properly.
					// we have a good locale field formatted field, so try to get desired locale:
					if(S.IndexOf("<locale name=\"" + LocaleSetting + "\">") != -1)
					{
						tmpS = Common.ExtractToken(S,"<locale name=\"" + LocaleSetting + "\">","</locale>");
					}
					else if (fallBack && (S.IndexOf(LocaleSetting) != -1))
					{
						tmpS = Common.ExtractToken(S,"<locale name=\"" + LocaleSetting + "\">","</locale>");
					}
					else
					{
						tmpS = String.Empty;
					}
					if(tmpS.Length != 0)
					{
						tmpS = XmlCommon.XmlDecode(tmpS);
					}
				}
			}
			else
			{
				tmpS = S; // for backwards compatibility...they have no locale info, so just return the field.
			}
			return tmpS;
		}
		
		public static String XmlField(XmlNode node, String fieldName)
		{
			String fieldVal = String.Empty;
			try
			{
				fieldVal = node.SelectSingleNode(@fieldName).InnerText.Trim();
			}
			catch {} // node might not be there
			return fieldVal;
		}

		public static String XmlFieldByLocale(XmlNode node, String fieldName, String LocaleSetting)
		{
			return GetLocaleEntry(XmlField(node,fieldName),LocaleSetting,true);
		}

		public static bool XmlFieldBool(XmlNode node, String fieldName)
		{
			String tmp = XmlField(node,fieldName).ToLower();
			if(tmp == "true" || tmp == "yes" || tmp == "1")
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static int XmlFieldUSInt(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseUSInt(tmpS);
		}

		public static long XmlFieldUSLong(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseUSLong(tmpS);
		}

		public static Single XmlFieldUSSingle(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseUSSingle(tmpS);
		}

		public static Double XmlFieldUSDouble(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseUSDouble(tmpS);
		}

		public static decimal XmlFieldUSDecimal(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseUSCurrency(tmpS);
		}

		public static DateTime XmlFieldUSDateTime(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseUSDateTime(tmpS);
		}

		public static int XmlFieldNativeInt(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseNativeInt(tmpS);
		}

		public static long XmlFieldNativeLong(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseNativeLong(tmpS);
		}

		public static Single XmlFieldNativeSingle(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseNativeSingle(tmpS);
		}

		public static Double XmlFieldNativeDouble(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseNativeDouble(tmpS);
		}

		public static decimal XmlFieldNativeDecimal(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseNativeDecimal(tmpS);
		}

		public static DateTime XmlFieldNativeDateTime(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseNativeDateTime(tmpS);
		}

		// ----------------------------------------------------------------
		//
		// SIMPLE Xml ATTRIBUTE ROUTINES
		//
		// ----------------------------------------------------------------

		public static String XmlAttribute(XmlNode node, String AttributeName)
		{
			String AttributeVal = String.Empty;
			try
			{
				AttributeVal = node.Attributes[AttributeName].InnerText.Trim();
			}
			catch {} // node might not be there
			return AttributeVal;
		}

		public static bool XmlAttributeBool(XmlNode node, String AttributeName)
		{
			String tmp = XmlAttribute(node,AttributeName).ToLower();
			if(tmp == "true" || tmp == "yes" || tmp == "1")
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static int XmlAttributeUSInt(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseUSInt(tmpS);
		}

		public static long XmlAttributeUSLong(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseUSLong(tmpS);
		}

		public static Single XmlAttributeUSSingle(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseUSSingle(tmpS);
		}

		public static Double XmlAttributeUSDouble(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseUSDouble(tmpS);
		}

		public static decimal XmlAttributeUSDecimal(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseUSDecimal(tmpS);
		}

		public static DateTime XmlAttributeUSDateTime(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseUSDateTime(tmpS);
		}

		public static int XmlAttributeNativeInt(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseNativeInt(tmpS);
		}

		public static long XmlAttributeNativeLong(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseNativeLong(tmpS);
		}

		public static Single XmlAttributeNativeSingle(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseNativeSingle(tmpS);
		}

		public static Double XmlAttributeNativeDouble(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseNativeDouble(tmpS);
		}

		public static decimal XmlAttributeNativeDecimal(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseNativeDecimal(tmpS);
		}

		public static DateTime XmlAttributeNativeDateTime(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseNativeDateTime(tmpS);
		}

		public static String GetXPathEntry(String S, String XPath)
		{
			String tmpS = String.Empty;
			if(S.Length == 0)
			{
				return tmpS;
			}
			try
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(S);
				XmlNode node = doc.DocumentElement.SelectSingleNode(XPath);
				if(node != null)
				{
					tmpS = node.InnerText;
				}
				if(tmpS.Length != 0)
				{
					tmpS = XmlCommon.XmlDecode(tmpS);
				}
			}
			catch {}
			return tmpS;
		}

	}
}
