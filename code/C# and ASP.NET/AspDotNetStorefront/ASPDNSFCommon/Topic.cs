// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Web.Security;
using System.Configuration;
using System.Web.SessionState;
using System.Web.Caching;
using System.Web.Mail;
using System.Web.Util;
using System.Data;
using System.Security.Principal;
using System.Text;
using System.Collections;
using System.IO;
using System.Net;
using System.Xml;
using System.Drawing;
using System.Xml.Serialization;
using System.Globalization;

namespace AspDotNetStorefrontCommon
{
	/// <summary>
	/// Summary description for Topic.
	/// </summary>
	public class Topic
	{

		public String _topicName;
		public int _topicID;
		public String _localeSetting;
		public int _siteID;

		public String _contents;
		public String _contentsRAW;
		public String _sectionTitle;
		public String _SETitle;
		public String _SEKeywords;
		public String _SEDescription;
		public String _pwd;
		public bool _fromDB;
		public bool _requiresSubscription;
		public String _fn;

		public Topic(String TopicName)
		{
			_topicName = TopicName;
			_topicID = 0;
			_localeSetting = String.Empty;
			_siteID = 1;
			LoadFromDB();
		}

		public Topic(int TopicID)
		{
			_topicName = GetName(TopicID);
			_topicID = TopicID;
			_localeSetting = Localization.GetWebConfigLocale();
			_siteID = 1;
			LoadFromDB();
		}

		public Topic(String TopicName, String LocaleSetting)
		{
			_topicName = TopicName;
			_topicID = 0;
			_localeSetting = LocaleSetting;
			_siteID = 1;
			LoadFromDB();
		}

		public Topic(int TopicID, String LocaleSetting)
		{
			_topicName = GetName(TopicID);
			_topicID = TopicID;
			_localeSetting = LocaleSetting;
			_siteID = 1;
			LoadFromDB();
		}

		public Topic(String TopicName, String LocaleSetting, int SiteID)
		{
			_topicName = TopicName;
			_topicID = 0;
			_localeSetting = LocaleSetting;
			_siteID = SiteID;
			LoadFromDB();
		}

		public Topic(int TopicID, String LocaleSetting, int SiteID)
		{
			_topicName = GetName(TopicID);
			_topicID = TopicID;
			_localeSetting = LocaleSetting;
			_siteID = SiteID;
			LoadFromDB();
		}

		public Topic(String TopicName, int SiteID)
		{
			_topicName = TopicName;
			_topicID = 0;
			_localeSetting = Localization.GetWebConfigLocale();
			_siteID = SiteID;
			LoadFromDB();
		}

		public Topic(int TopicID, int SiteID)
		{
			_topicName = GetName(TopicID);
			_topicID = TopicID;
			_localeSetting = Localization.GetWebConfigLocale();
			_siteID = SiteID;
			LoadFromDB();
		}

		public String Contents
		{
			get 
			{
				return _contents;
			}
			set 
			{
				_contents = value;
			}
		}



		// Find the specified topic content. note, we try to find content even if it doesn't exactly match the input specs, by doing an ordered lookup in various areas
		// we want to show SOME topic content if it is at all possible, even if the language is not right, etc...
		// Note: site id only used for file based topic _contents
		// Search Order is (yes, other orderings are possible, but this is the one we chose, where ANY db topic match overrides file content):
		// the other option would be to match on locales in the order of DB/File (Customer Locale), DB/File (Store Locale), DB/File (Null locale)
		// DB (customer locale)
		// DB (store locale)
		// DB (null locale)
		// File (customer locale)
		// File (store locale)
		// File (null locale)
		void LoadFromDB()
		{
			_fromDB = false;
			_contents = String.Empty;
			_contentsRAW = String.Empty;
			_sectionTitle = String.Empty;
			_requiresSubscription = false;
			_pwd = String.Empty;
			_SETitle = _topicName;
			_SEKeywords = String.Empty;
			_SEDescription = String.Empty;
			_fn = String.Empty;

			String LocaleWhere = string.Empty;
			if(_localeSetting == Localization.GetWebConfigLocale())
			{
				LocaleWhere = "(LocaleSetting IS NULL or LocaleSetting=" + DB.SQuote(Localization.GetWebConfigLocale()) + ")";
			}
			else
			{
				LocaleWhere = "(LocaleSetting=" + DB.SQuote(_localeSetting) + ")";
			}

			String sql = "Select * from topic " + DB.GetNoLock() + " where " + LocaleWhere + " and deleted=0 and lower(name)=" + DB.SQuote(_topicName.ToLower());
			IDataReader rs = DB.GetRS(sql);
			bool found = false;
			_topicID = 0;
			if(rs.Read())
			{
				_fromDB = true;
				_topicID = DB.RSFieldInt(rs,"TopicID");
				_contents = DB.RSField(rs,"Description");
				_contentsRAW = _contents;
				_sectionTitle = DB.RSField(rs,"Title");
				_SETitle = DB.RSField(rs,"SETitle");
				_SEKeywords = DB.RSField(rs,"SEKeywords");
				_SEDescription = DB.RSField(rs,"SEDescription");
				_pwd = DB.RSField(rs,"Password");
				_requiresSubscription = DB.RSFieldBool(rs,"RequiresSubscription");
				found = true;
			}
			rs.Close();

			if(!found && _localeSetting != Localization.GetWebConfigLocale())
			{
				// didn't find one in the localesetting desired, try store default locale for the topic:
				LocaleWhere = "(LocaleSetting IS NULL or LocaleSetting=" + DB.SQuote(Localization.GetWebConfigLocale()) + ")";
				sql = "Select * from topic " + DB.GetNoLock() + " where " + LocaleWhere + " and deleted=0 and lower(name)=" + DB.SQuote(_topicName);
				rs = DB.GetRS(sql);
				found = false;
				if(rs.Read())
				{
					_fromDB = true;
					_topicID = DB.RSFieldInt(rs,"TopicID");
					_contents = DB.RSField(rs,"Description");
					_contentsRAW = _contents;
					_sectionTitle = DB.RSField(rs,"Title");
					_SETitle = DB.RSField(rs,"SETitle");
					_SEKeywords = DB.RSField(rs,"SEKeywords");
					_SEDescription = DB.RSField(rs,"SEDescription");
					_pwd = DB.RSField(rs,"Password");
					_requiresSubscription = DB.RSFieldBool(rs,"RequiresSubscription");
					found = true;
				}
				rs.Close();
			}

			if(!_fromDB)
			{
				_fn = HttpContext.Current.Server.MapPath("skins/Skin_" + _siteID.ToString() + "/" + _localeSetting + "/" + _topicName + ".htm");
				if(!Common.FileExists(_fn))
				{
					// try default store locale path:
					_fn = HttpContext.Current.Server.MapPath("skins/Skin_" + _siteID.ToString() + "/" + Localization.GetWebConfigLocale() + "/" + _topicName + ".htm");
				}
				if(!Common.FileExists(_fn))
				{
					// try skin (NULL) path:
					_fn = HttpContext.Current.Server.MapPath("skins/Skin_" + _siteID.ToString() + "/" + _topicName + ".htm");
				}
				if(_fn.Length != 0 && Common.FileExists(_fn))
				{
					_contents = Common.ReadFile(_fn,true);
					_contentsRAW = _contents;
					_sectionTitle = Common.ExtractToken(_contents,"<title>","</title>");
					_contents = Common.ExtractBody(_contents);
					_SETitle = Common.ExtractToken(_contents,"<PAGETITLE>","</PAGETITLE>");
					_SEKeywords = Common.ExtractToken(_contents,"<PAGEKEYWORDS>","</PAGEKEYWORDS>");
					_SEDescription = Common.ExtractToken(_contents,"<PAGEDESCRIPTION>","</PAGEDESCRIPTION>");
				}
			}
			
			if(_sectionTitle.Length == 0)
			{
				if(_SETitle.Length != 0)
				{
					_sectionTitle = _SETitle;
				}
				else
				{
					_sectionTitle = _topicName;
				}
			}

			if(_SETitle.Length == 0)
			{
				_SETitle = _sectionTitle;
			}	
			_contents = _contents.Replace("%SKINID%",_siteID.ToString());
		}

		public static String GetName(int TopicID)
		{
			IDataReader rs = DB.GetRS("select Name from Topic  " + DB.GetNoLock() + " where TopicID=" + TopicID.ToString());
			String tmpS = String.Empty;
			if(rs.Read())
			{
				tmpS = DB.RSField(rs,"Name");
			}
			rs.Close();
			return tmpS;
		}

	}
}
