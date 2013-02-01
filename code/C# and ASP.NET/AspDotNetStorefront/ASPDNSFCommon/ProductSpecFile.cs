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
	/// Summary description for ProductSpecFile: class finder for product spec description files (i.e. /productspecs/...*.htm) with locale support
	/// </summary>
	public class ProductSpecFile
	{

		public String _productSKU;
		public int _productID;
		public String _localeSetting;
		public int _siteID;

		public String _contents;
		public String _contentsRAW;
		public String _fn;
		public String _url;

		public ProductSpecFile(String ProductSKU)
		{
			_productSKU = ProductSKU;
			_productID = 0;
			_localeSetting = String.Empty;
			_siteID = 1;
			LoadFromDB();
		}

		public ProductSpecFile(int ProductID)
		{
			_productSKU = String.Empty;
			_productID = ProductID;
			_localeSetting = Localization.GetWebConfigLocale();
			_siteID = 1;
			LoadFromDB();
		}

		public ProductSpecFile(String ProductSKU, String LocaleSetting)
		{
			_productSKU = ProductSKU;
			_productID = 0;
			_localeSetting = LocaleSetting;
			_siteID = 1;
			LoadFromDB();
		}

		public ProductSpecFile(int ProductID, String LocaleSetting)
		{
			_productSKU = String.Empty;
			_productID = ProductID;
			_localeSetting = LocaleSetting;
			_siteID = 1;
			LoadFromDB();
		}

		public ProductSpecFile(String ProductSKU, String LocaleSetting, int SiteID)
		{
			_productSKU = ProductSKU;
			_productID = 0;
			_localeSetting = LocaleSetting;
			_siteID = SiteID;
			LoadFromDB();
		}

		public ProductSpecFile(int ProductID, String LocaleSetting, int SiteID)
		{
			_productSKU = String.Empty;
			_productID = ProductID;
			_localeSetting = LocaleSetting;
			_siteID = SiteID;
			LoadFromDB();
		}

		public ProductSpecFile(String ProductSKU, int SiteID)
		{
			_productSKU = ProductSKU;
			_productID = 0;
			_localeSetting = Localization.GetWebConfigLocale();
			_siteID = SiteID;
			LoadFromDB();
		}

		public ProductSpecFile(int ProductID, int SiteID)
		{
			_productSKU = String.Empty;
			_productID = ProductID;
			_localeSetting = Localization.GetWebConfigLocale();
			_siteID = SiteID;
			LoadFromDB();
		}

		// Find the specified ProductSpec content. will be in /productspecs or some locale subdir. find by productid or SKU, whichever is provided.
		void LoadFromDB()
		{
			_contents = String.Empty;
			_contentsRAW = String.Empty;
			_fn = String.Empty;

			if(_productID != 0)
			{
				// try to locate by productid.htm
				_url = Common.IIF(Common.IsAdminSite, "../","") + "productspecs/" + _localeSetting + "/" + _productID.ToString() + ".htm";
				_fn = HttpContext.Current.Server.MapPath(_url);
				if(!Common.FileExists(_fn))
				{
					// try default store locale path:
					_url = Common.IIF(Common.IsAdminSite, "../","") + "productspecs/" + Localization.GetWebConfigLocale() + "/" + _productID.ToString() + ".htm";
					_fn = HttpContext.Current.Server.MapPath(_url);
				}
				if(!Common.FileExists(_fn))
				{
					// try skin (NULL) path:
					_url = Common.IIF(Common.IsAdminSite, "../","") + "productspecs/" + _productID.ToString() + ".htm";
					_fn = HttpContext.Current.Server.MapPath(_url);
				}
			}
			else if(_productSKU.Length != 0)
			{
				// try to locate by productsku.htm
				_url = Common.IIF(Common.IsAdminSite, "../","") + "productspecs/" + _localeSetting + "/" + _productSKU + ".htm";
				_fn = HttpContext.Current.Server.MapPath(_url);
				if(!Common.FileExists(_fn))
				{
					// try default store locale path:
					_url = Common.IIF(Common.IsAdminSite, "../","") + "productspecs/" + Localization.GetWebConfigLocale() + "/" + _productSKU + ".htm";
					_fn = HttpContext.Current.Server.MapPath(_url);
				}
				if(!Common.FileExists(_fn))
				{
					// try base (NULL) path:
					_url = Common.IIF(Common.IsAdminSite, "../","") + "productspecs/" + _productSKU + ".htm";
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
