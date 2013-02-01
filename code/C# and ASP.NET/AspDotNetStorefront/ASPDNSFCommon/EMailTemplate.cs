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
	/// Summary description for EMailTemplate: class finder for product spec description files (i.e. /emailtemplates/...*.htm) with locale support
	/// </summary>
	public class EMailTemplate
	{

		public String _templateName;
		public String _localeSetting;
		public int _siteID;

		public String _contents;
		public String _contentsRAW;
		public String _fn;
		public String _url;

		public EMailTemplate(String TemplateName)
		{
			_templateName = TemplateName;
			_localeSetting = Localization.GetWebConfigLocale();
			_siteID = 1;
			LoadFromDB();
		}

		public EMailTemplate(String TemplateName, String LocaleSetting)
		{
			_templateName = TemplateName;
			_localeSetting = LocaleSetting;
			_siteID = 1;
			LoadFromDB();
		}

		public EMailTemplate(String TemplateName, String LocaleSetting, int SiteID)
		{
			_templateName = TemplateName;
			_localeSetting = LocaleSetting;
			_siteID = SiteID;
			LoadFromDB();
		}

		public EMailTemplate(String TemplateName, int SiteID)
		{
			_templateName = TemplateName;
			_localeSetting = Localization.GetWebConfigLocale();
			_siteID = SiteID;
			LoadFromDB();
		}

		// Find the specified EMail Template content. will be in /emailtemplates or some locale subdir. find by TemplateName.htm.
		void LoadFromDB()
		{
			_contents = String.Empty;
			_contentsRAW = String.Empty;
			_fn = String.Empty;

			if(_templateName.Length != 0)
			{
				// try to locate by productid.htm
				_url = Common.IIF(Common.IsAdminSite, "../","") + "emailtemplates/" + _localeSetting + "/template" + _templateName + ".htm";
				_fn = HttpContext.Current.Server.MapPath(_url);
				if(!Common.FileExists(_fn))
				{
					// try default store locale path:
					_url = Common.IIF(Common.IsAdminSite, "../","") + "emailtemplates/" + Localization.GetWebConfigLocale() + "/template" + _templateName + ".htm";
					_fn = HttpContext.Current.Server.MapPath(_url);
				}
				if(!Common.FileExists(_fn))
				{
					// try base (NULL) path:
					_url = Common.IIF(Common.IsAdminSite, "../","") + "emailtemplates/template" + _templateName + ".htm";
					_fn = HttpContext.Current.Server.MapPath(_url);
				}
			}
			if(_fn.Length != 0 && Common.FileExists(_fn))
			{
				_url = Common.GetStoreHTTPLocation(false) + _url;
				_contents = Common.ReadFile(_fn,true);
				_contentsRAW = _contents;
			}
			else
			{
				_url = String.Empty;
				_fn = String.Empty;
			}
		}

	}
}
