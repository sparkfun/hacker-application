//#define SMTPDOTNET
// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// $Header: /v5.0/AspDotNetStorefront/ASPDNSFCommon/ShippingTemplate.cs 3     5/02/05 12:16p Redwoodtree $
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
	/// Summary description for ShippingTemplate: class finder for product spec description files (i.e. /shipped/...*.htm) with locale support
	/// </summary>
	public class ShippingTemplate
	{

		private String m_TemplateBaseFN;
		private String m_LocaleSetting = Localization.GetWebConfigLocale();
		private int m_SiteID = 1;
		private readonly String readonly_PathLocation = "shipped";
		private String m_Contents = String.Empty;
		private String m_ContentsRAW = String.Empty;
		private String m_FN = String.Empty;
		private String m_URL = String.Empty;

		public ShippingTemplate(int ShippingTemplateID)
			: this("template" + ShippingTemplateID.ToString(),Localization.GetWebConfigLocale(),1)
		{}

		public ShippingTemplate(int ShippingTemplateID, String LocaleSetting)
			: this("template" + ShippingTemplateID.ToString(),LocaleSetting,1)
		{}

		public ShippingTemplate(int ShippingTemplateID, String LocaleSetting, int SiteID)
			: this("template" + ShippingTemplateID.ToString(),LocaleSetting,SiteID)
		{}

		public ShippingTemplate(int ShippingTemplateID, int SiteID)
			: this("template" + ShippingTemplateID.ToString(),Localization.GetWebConfigLocale(),SiteID)
		{}

		public ShippingTemplate(String ShippingTemplate)
			: this(ShippingTemplate,Localization.GetWebConfigLocale(),1)
		{}

		public ShippingTemplate(String ShippingTemplate, String LocaleSetting)
			: this(ShippingTemplate,LocaleSetting,1)
		{}

		public ShippingTemplate(String ShippingTemplate, int SiteID)
			: this(ShippingTemplate,Localization.GetWebConfigLocale(),SiteID)
		{}

		public ShippingTemplate(String ShippingTemplate, String LocaleSetting, int SiteID)
		{
			// primary constructor
			m_TemplateBaseFN = ShippingTemplate;
			m_LocaleSetting = LocaleSetting;
			m_SiteID = SiteID;
			m_Contents = String.Empty;
			m_ContentsRAW = String.Empty;
			m_FN = String.Empty;


			// try customer locale:
			m_URL = Common.IIF(Common.IsAdminSite, "../","") + readonly_PathLocation + "/" + m_TemplateBaseFN + "." + m_LocaleSetting + ".htm";
			m_FN = HttpContext.Current.Server.MapPath(m_URL);
			if(!Common.FileExists(m_FN))
			{
				// try default store locale path:
				m_URL = Common.IIF(Common.IsAdminSite, "../","") + readonly_PathLocation + "/" + m_TemplateBaseFN + "." + Localization.GetWebConfigLocale() + ".htm";
				m_FN = HttpContext.Current.Server.MapPath(m_URL);
			}
			if(!Common.FileExists(m_FN))
			{
				// try base (NULL) locale path:
				m_URL = Common.IIF(Common.IsAdminSite, "../","") + readonly_PathLocation + "/" + m_TemplateBaseFN + ".htm";
				m_FN = HttpContext.Current.Server.MapPath(m_URL);
			}

			if(m_FN.Length != 0 && Common.FileExists(m_FN))
			{
				m_URL = Common.GetStoreHTTPLocation(false) + m_URL;
				m_Contents = Common.ReadFile(m_FN,true);
				m_ContentsRAW = m_Contents;
			}
			else
			{
				m_URL = String.Empty;
				m_FN = String.Empty;
			}
		}


		public String FN
		{
			get 
			{
				return m_FN;
			}
			set 
			{
				m_FN = value;
			}
		}

		public String URL
		{
			get 
			{
				return m_URL;
			}
			set 
			{
				m_URL = value;
			}
		}

		public String Contents
		{
			get 
			{
				return m_Contents;
			}
			set 
			{
				m_Contents = value;
			}
		}

		public String ContentsRAW
		{
			get 
			{
				return m_ContentsRAW;
			}
			set 
			{
				m_ContentsRAW = value;
			}
		}

		public String LocaleSetting
		{
			get 
			{
				return m_LocaleSetting;
			}
			set 
			{
				m_LocaleSetting = value;
			}
		}

		public String ShippingTemplateBaseFN
		{
			get 
			{
				return m_TemplateBaseFN;
			}
			set 
			{
				m_TemplateBaseFN = value;
			}
		}

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

	}
}
