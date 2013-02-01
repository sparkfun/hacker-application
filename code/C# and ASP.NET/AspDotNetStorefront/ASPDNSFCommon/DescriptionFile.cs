//#define SMTPDOTNET
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
	/// Summary description for DescriptionFile: class finder for section/category description files (i.e. /{descriptiontype}descriptions/...*.htm) with locale support
	/// </summary>
	public class DescriptionFile
	{

		public String _descriptionType; // should be "section" or "category"
		public int _ID;
		public String _localeSetting;
		public int _siteID;

		public String _contents;
		public String _contentsRAW;
		public String _fn;
		public String _url;

		public DescriptionFile(String DescriptionType, int ID)
		{
			_descriptionType = DescriptionType;
			_ID = ID;
			_localeSetting = Localization.GetWebConfigLocale();
			_siteID = 1;
			LoadFromDB();
		}

		public DescriptionFile(String DescriptionType, int ID, String LocaleSetting)
		{
			_descriptionType = DescriptionType;
			_ID = ID;
			_localeSetting = LocaleSetting;
			_siteID = 1;
			LoadFromDB();
		}

		public DescriptionFile(String DescriptionType, int ID, String LocaleSetting, int SiteID)
		{
			_descriptionType = DescriptionType;
			_ID = ID;
			_localeSetting = LocaleSetting;
			_siteID = SiteID;
			LoadFromDB();
		}

		public DescriptionFile(String DescriptionType, int ID, int SiteID)
		{
			_descriptionType = DescriptionType;
			_ID = ID;
			_localeSetting = Localization.GetWebConfigLocale();
			_siteID = SiteID;
			LoadFromDB();
		}

		// Find the description file content. will be in /{descriptiontype}descriptions or some locale subdir. find by ID
		void LoadFromDB()
		{
			_contents = String.Empty;
			_contentsRAW = String.Empty;
			_fn = String.Empty;

			if(_ID != 0)
			{
				// try to locate by ID.htm
				_url = Common.IIF(Common.IsAdminSite, "../","") + _descriptionType.ToLower() + "descriptions/" + _localeSetting + "/" + _ID.ToString() + ".htm";
				_fn = HttpContext.Current.Server.MapPath(_url);
				if(!Common.FileExists(_fn))
				{
					// try default store locale path:
					_url = Common.IIF(Common.IsAdminSite, "../","") + _descriptionType.ToLower() + "descriptions/" + Localization.GetWebConfigLocale() + "/" + _ID.ToString() + ".htm";
					_fn = HttpContext.Current.Server.MapPath(_url);
				}
				if(!Common.FileExists(_fn))
				{
					// try skin (NULL) path:
					_url = Common.IIF(Common.IsAdminSite, "../","") + _descriptionType.ToLower() + "descriptions/" + _ID.ToString() + ".htm";
					_fn = HttpContext.Current.Server.MapPath(_url);
				}
			}
			if(_fn.Length != 0 && Common.FileExists(_fn))
			{
				_url = Common.GetStoreHTTPLocation(false) + _url;
				_contents = Common.ReadFile(_fn,true);
				_contentsRAW = _contents;
				_contents = Common.ExtractBody(_contents);
				_contents = _contents.Replace("%SKINID%",_siteID.ToString());
			}
			else
			{
				_url = String.Empty;
				_fn = String.Empty;
			}
		}

	}
}
