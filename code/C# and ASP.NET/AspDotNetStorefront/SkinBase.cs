using System;
using System.Web;
using System.Text;
using System.Data;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// Will indicate what mode a SkinBase page should be shown in.
	public enum Mode : int
	{
		Normal = 0, // Render normally.
		PageError = 1 // Render the error output
	}
	
	public class SkinBase : System.Web.UI.Page
	{

		const String SkinCookieName = "SkinID";
		
		public bool _designMode;
		public Customer thisCustomer;
		public String SectionTitle;
		
		public String _templateName;
		public String _templateRAW;
		public String _header;
		public String _footer;
		public int _siteID;
		public bool _noLeftCol;
		public bool _noRightCol;
		public String _SETitle;
		public String _SEDescription;
		public String _SEKeywords;
		public String _SENoScript;

		public bool _disableLeftAndRightCols;
		public bool _disableContents;

		private Mode mPageMode = Mode.Normal;
		public Mode PageMode
		{
			get { return mPageMode; }
		}

		private string mPageErrorText = "";
		public virtual string PageErrorText
		{
			get { return mPageErrorText; }
		}
		private string mErrorXRef = "";
		public string ErrorXRef
		{
			get 
			{
				// We only want to retrieve this once.
				if (mErrorXRef.Length == 0)
					mErrorXRef = ErrorXRefGenerator.NextID();
				return mErrorXRef;
			}
		}

		public Customer ThisCustomer
		{
			get 
			{
				return thisCustomer;
			}
			set 
			{
				thisCustomer = value;
			}
		}

		public int SiteID
		{
			get 
			{
				return _siteID;
			}
			set 
			{
				_siteID = value;
			}
		}


		
		public SkinBase()
		{
			_designMode = (HttpContext.Current == null);
			if(!_designMode)
			{
				SectionTitle = String.Empty;
				_siteID = 1;
				_templateName = String.Empty;
				_templateRAW = String.Empty;
				_header = "<html><head></head><body>";
				_footer = "</body></html>";
				_noLeftCol = true;
				_noRightCol = true;
				_disableLeftAndRightCols = false;
				_SETitle = String.Empty;
				_SEDescription = String.Empty;
				_SEKeywords = String.Empty;
				_SENoScript = String.Empty;
				_disableContents = false;
			}
		}

		public void RequireSecurePage()
		{
			if(Common.AppConfigBool("UseSSL"))
			{
				if(Common.OnLiveServer() && Common.ServerVariables("SERVER_PORT_SECURE") != "1")
				{
					Response.Redirect(Common.GetStoreHTTPLocation(true) + Common.GetThisPageName(false) + "?" + Common.ServerVariables("QUERY_STRING"));
				}
			}
		}

		public void RequireCustomerRecord()
		{
			if(!thisCustomer._hasCustomerRecord)
			{
				thisCustomer.RequireCustomerRecord();
			}
		}

		// called from each page_load event...we cannot figure out how to subclass this and get it called automatically :(
		public void DoOnLoad()
		{
			thisCustomer = new Customer(); // get the real customer now that session is available
		}

		public void RequiresLogin(String ReturnURL)
		{
			if(thisCustomer._isAnon)
			{
				Response.Redirect("signin.aspx?returnurl=" + Server.UrlEncode(ReturnURL));
			}
		}

		public void SetMetaTags(String SETitle, String SEKeywords, String SEDescription, String SENoScript)
		{
			_SETitle = SETitle;
			_SEDescription = SEKeywords;
			_SEKeywords = SEDescription;
			_SENoScript =  SENoScript;
		}

		public void SetTemplate(String TemplateName)
		{
			_templateName = TemplateName;
		}

		public void LoadSkinTemplate()
		{
			_siteID = 1;
			if(_templateName.Length == 0)
			{
				_templateName = "template.htm";
			}
			_templateRAW = String.Empty;
			_header = "<html><head></head><body>";
			_footer = "</body></html>";
			_noLeftCol = true;
			_noRightCol = true;
			if(_templateName.Length != 0)
			{
				_siteID = Common.QueryStringUSInt("SiteID");
				if(_siteID == 0)
				{
					_siteID = Common.QueryStringUSInt("SkinID");
				}
				if(_siteID == 0 && Common.QueryString("AffiliateID").Length != 0)
				{
					IDataReader rs = DB.GetRS("Select DefaultSkinID from Affiliate  " + DB.GetNoLock() + " where AffiliateID=" + Common.QueryString("AffiliateID"));
					if(rs.Read())
					{
						_siteID = DB.RSFieldInt(rs,"DefaultSkinID");
					}
					rs.Close();
				}
				if(_siteID == 0)
				{
					_siteID = Common.CookieUSInt(SkinCookieName);
				}
				if(_siteID == 0)
				{
					_siteID = Common.AppConfigUSInt("DefaultSkinID");
				}
				if(_siteID == 0)
				{
					_siteID = 1;
				}

				Common.SetCookie(SkinCookieName,_siteID.ToString(),new TimeSpan(365,0,0,0,0));

				_templateRAW = Common.ReadFile("skins/Skin_" + _siteID.ToString() + "/" + _templateName,true);
				if(_templateRAW.Length != 0)
				{
					int i = _templateRAW.IndexOf("%CONTENTS%");
					if(i == -1)
					{
						_header = _templateRAW;
						_footer = "";
					}
					else
					{
						_header = _templateRAW.Substring(0,i);
						_footer = _templateRAW.Substring(i+10);
					}
					_noLeftCol = (_templateRAW.IndexOf("%NOLEFTCOL%") != -1);
					_noRightCol = (_templateRAW.IndexOf("%NORIGHTCOL%") != -1);
				}
				else
				{
					_header = "<html><head></head><body>";
					_footer = "</body></html>";
				}
			}
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
	
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");

			LoadSkinTemplate();
			
			if(_disableContents)
			{
				writer.Write(SkinTop()); // Write out the top of the page.
				writer.Write(SkinBottom()); // Write out the top of the page.
			}
			else
			{
				if(_SETitle.Length == 0)
				{
					_SETitle = Common.AppConfig("SE_MetaTitle");
				}
				if(_SEDescription.Length == 0)
				{
					_SEDescription = Common.AppConfig("SE_MetaDescription");
				}
				if(_SEKeywords.Length == 0)
				{
					_SEKeywords = Common.AppConfig("SE_MetaKeywords");
				}
				if(_SENoScript.Length == 0)
				{
					_SENoScript = Common.AppConfig("SE_MetaNoScript");
				}

				writer.Write(SkinTop()); // Write out the top of the page.

				writer.Write("<table border=\"0\" cellpadding=\"4\" cellspacing=\"0\" width=\"100%\">\n");
				writer.Write("<tr>\n");

				// LEFT COL:
				if(Common.AppConfigBool("ShowLeftCol") && !_noLeftCol && !_disableLeftAndRightCols)
				{
					writer.Write("<td valign=\"top\" align=\"left\">");
					writer.Write(Common.GetLeftCol(thisCustomer,_siteID));
					writer.Write("</td>\n");
				}

				writer.Write("<td width=\"100%\" valign=\"top\" align=\"left\">");
				// CENTER COL:

				base.Render(writer);

				RenderContents(writer);

				writer.Write("</td>\n");
				// RIGHT COL:
				if(Common.AppConfigBool("ShowRightCol") && !_noRightCol && !_disableLeftAndRightCols)
				{
					writer.Write("<td width=\"\" valign=\"top\" align=\"left\">\n");
					writer.Write(Common.GetSpecialsBox(Common.AppConfigUSInt("IsFeaturedCategoryID"),5,true,Common.AppConfig("IsFeaturedCategoryTeaser"),_siteID));
					writer.Write("</td>\n");
				}

				writer.Write("</tr>\n");
				writer.Write("</table>\n");

				writer.Write(SkinBottom()); 				// Write out the bottom of the page
			}

		}
		
		protected string SkinTop()
		{
			String s = _header;
			s = s.Replace("%SECTION_TITLE%",SectionTitle);
			s = ReplaceTokens(thisCustomer,s);
			return s;
		}

		protected string SkinBottom()
		{
			String s = _footer;
			s = ReplaceTokens(thisCustomer,s);
			return s;
		}

		public String ReplaceTokens(Customer thisCustomer,String s)
		{
			bool IsAnon = thisCustomer._isAnon;
			s = s.Replace("%METATITLE%",_SETitle);
			s = s.Replace("%METADESCRIPTION%",_SEDescription);
			s = s.Replace("%METAKEYWORDS%",_SEKeywords);
			s = s.Replace("%SENOSCRIPT%",_SENoScript);
			if(!IsAnon)
			{
				int CustomerLevelID = thisCustomer._customerLevelID;
				String CustomerLevelName = String.Empty;;
				if(CustomerLevelID != 0)
				{
					CustomerLevelName = Common.GetCustomerLevelName(CustomerLevelID);
				}

				s = s.Replace("%USER_NAME%","You're logged in as: <a class=\"username\" href=\"account.aspx\">" + thisCustomer.FullName() + "</a>" + Common.IIF(CustomerLevelID != 0 , "&nbsp;(" + CustomerLevelName + ")" , ""));
				s = s.Replace("%USERNAME%","You're logged in as: <a class=\"username\" href=\"account.aspx\">" + thisCustomer.FullName() + "</a>" + Common.IIF(CustomerLevelID != 0 , "&nbsp;(" + CustomerLevelName + ")" , ""));
				s = s.Replace("%USER_INFO%",Common.GetUserBox(thisCustomer,_siteID));
			}
			else
			{
				s = s.Replace("%USER_NAME%","");
				s = s.Replace("%USERNAME%","");
				s = s.Replace("%USER_INFO%",Common.GetLoginBox(_siteID));
			}
			s = s.Replace("%USER_MENU_NAME%",Common.IIF(IsAnon , "my account" , thisCustomer.FullName()));
			if(_templateRAW.IndexOf("%USER_MENU%") != -1)
			{
				s = s.Replace("%USER_MENU%",GetUserMenu(thisCustomer._isAnon));
			}
			if(_templateRAW.IndexOf("%MICROPAY_BALANCE%") != -1)
			{
				s = s.Replace("%MICROPAY_BALANCE%","Your " + Common.AppConfig("MicroPay.Prompt") + " balance is: " + Localization.DecimalStringForDB(thisCustomer._microPayBalance));
			}
			if(_templateRAW.IndexOf("%MICROPAY_BALANCE_RAW%") != -1)
			{
				s = s.Replace("%MICROPAY_BALANCE_RAW%",Localization.DecimalStringForDB(thisCustomer._microPayBalance));
			}
			if(_templateRAW.IndexOf("%MICROPAY_BALANCE_CURRENCY%") != -1)
			{
				s = s.Replace("%MICROPAY_BALANCE_CURRENCY%",Localization.CurrencyStringForDisplay(thisCustomer._microPayBalance));
			}
			s = s.Replace("%SECTION_PROMPT%",Common.AppConfig("SectionPromptPlural"));
			s = s.Replace("%CATEGORY_PROMPT%",Common.AppConfig("CategoryPromptPlural"));
			s = s.Replace("%STORE_VERSION%",Common.AppConfig("StoreVersion"));

			if(!Common.AppConfigBool("JSMenuDisabled") || _templateRAW.IndexOf("%NOJSMENU%") == -1)
			{
				if(_templateRAW.IndexOf("%CATEGORY_MENU%") != -1)
				{
					s = s.Replace("%CATEGORY_MENU%",Common.GetCategoryMenu(0));
				}
				if(_templateRAW.IndexOf("%SECTION_MENU%") != -1)
				{
					s = s.Replace("%SECTION_MENU%",Common.GetSectionMenu(0));
				}
				if(_templateRAW.IndexOf("%MANUFACTURER_MENU%") != -1)
				{
					s = s.Replace("%MANUFACTURER_MENU%",Common.GetManufacturerMenu());
				}
			}
			if(_templateRAW.IndexOf("%NEWS_SUMMARY%") != -1)
			{
				s = s.Replace("%NEWS_SUMMARY%",Common.GetNewsSummary(3));
			}
			s = s.Replace("%RANDOM%",Common.GetRandomNumber(1,7).ToString()); // fairly skin specific ;)
			s = s.Replace("%HDRID%",Common.GetRandomNumber(1,7).ToString()); // fairly skin specific ;)
			if(Common.AppConfigBool("CardinalCommerce.Centinel.Enabled"))
			{
				s = s.Replace("%VBV%","<img src=\"skins/skin_" + _siteID.ToString() + "/images/vbv.jpg\" border=\"0\" alt=\"Store protected with Verified By Visa/MasterCard Secure Initiatives\">");
			}
			else
			{
				s = s.Replace("%VBV%","");
			}

			s = s.Replace("%SKINID%",_siteID.ToString());
			s = s.Replace("%SITENAME%",Common.AppConfig("StoreName"));
			s = s.Replace("%SITE_NAME%",Common.AppConfig("StoreName"));
			if(_templateRAW.IndexOf("%NUM_CART_ITEMS%") != -1)
			{
				s = s.Replace("%NUM_CART_ITEMS%",ShoppingCart.NumItems(thisCustomer._customerID,CartTypeEnum.ShoppingCart).ToString());
			}
			s = s.Replace("%CARTPROMPT%",Common.AppConfig("CartPrompt"));
			s = s.Replace("%NUMWISH_ITEMS%",ShoppingCart.NumItems(thisCustomer._customerID,CartTypeEnum.ShoppingCart).ToString());
			s = s.Replace("%SIGNINOUT_TEXT%",Common.IIF(IsAnon , "Login" , "Logout"));
			s = s.Replace("%SIGNINOUT_LINK%",Common.IIF(IsAnon , "signin.aspx" , "signout.aspx"));
			s = s.Replace("%INVOCATION%",HttpContext.Current.Server.HtmlEncode(Common.PageInvocation()));
			s = s.Replace("%REFERRER%",HttpContext.Current.Server.HtmlEncode(Common.PageReferrer()));

			if(_templateRAW.IndexOf("%HELPBOX%") != -1)
			{
				s = s.Replace("%HELPBOX%",Common.GetHelpBox(_siteID,false));
			}
			if(_templateRAW.IndexOf("%HELPBOX_CONTENTS%") != -1)
			{
				s = s.Replace("%HELPBOX_CONTENTS%",Common.GetHelpBox(_siteID,false));
			}
			if(_templateRAW.IndexOf("%LEFTCOL%") != -1)
			{
				s = s.Replace("%LEFTCOL%",Common.GetLeftCol(thisCustomer,_siteID));
			}
			if(_templateRAW.IndexOf("%CATEGORY_BROWSE_BOX%") != -1)
			{
				s = s.Replace("%CATEGORY_BROWSE_BOX%",Common.GetCategoryBrowseBox(_siteID));
			}
//			if(_templateRAW.IndexOf("%ADVANCED_CATEGORY_BROWSE_BOX%") != -1)
//			{
//				s = s.Replace("%ADVANCED_CATEGORY_BROWSE_BOX%",Common.GetAdvancedCategoryBrowseBox(thisCustomer,_siteID));
//			}
			if(_templateRAW.IndexOf("%SECTION_BROWSE_BOX%") != -1)
			{
				s = s.Replace("%SECTION_BROWSE_BOX%",Common.GetSectionBrowseBox(_siteID));
			}
//			if(_templateRAW.IndexOf("%ADVANCED_SECTION_BROWSE_BOX%") != -1)
//			{
//				s = s.Replace("%ADVANCED_SECTION_BROWSE_BOX%",Common.GetSectionBrowseBox(_siteID));
//			}
			if(_templateRAW.IndexOf("%SEARCH_BOX%") != -1)
			{
				s = s.Replace("%SEARCH_BOX%",Common.GetSearchBox(_siteID));
			}
			s = s.Replace("%SECTION_PROMPT_PLURAL%",Common.AppConfig("SectionPromptPlural").ToUpper());
			s = s.Replace("%CATEGORY_PROMPT_PLURAL%",Common.AppConfig("CategoryPromptPlural").ToUpper());
			s = s.Replace("%SECTION_PROMPT_SINGULAR%",Common.AppConfig("SectionPromptSingular").ToUpper());
			s = s.Replace("%CATEGORY_PROMPT_SINGULAR%",Common.AppConfig("CategoryPromptSingular").ToUpper());

			if(_templateRAW.IndexOf("%MINICART%") != -1)
			{
				String PN = Common.GetThisPageName(false).ToLower();
				if(PN.StartsWith("shoppingcart") || PN.StartsWith("checkout") || PN.StartsWith("cardinal") || PN.StartsWith("addtocart") || PN.IndexOf("_process") != -1 || PN.StartsWith("lat_"))
				{
					s = s.Replace("%MINICART%",String.Empty); // don't show on these pages
				}
				else
				{
					s = s.Replace("%MINICART%",ShoppingCart.DisplayMiniCart(thisCustomer,_siteID));
				}
			}

			if(_templateRAW.IndexOf("%CATEGORY_LIST%") != -1)
			{
				s = s.Replace("%CATEGORY_LIST%",Common.GetCategoryBrowseBox(_siteID));
			}
			if(_templateRAW.IndexOf("%SECTION_LIST%") != -1)
			{
				s = s.Replace("%SECTION_LIST%",Common.GetSectionBrowseBox(_siteID));
			}
			if(_templateRAW.IndexOf("%MANUFACTURER_LIST%") != -1)
			{
				s = s.Replace("%MANUFACTURER_LIST%",Common.GetManufacturerBrowseBox(_siteID));
			}

			if(_templateRAW.IndexOf("%PLAIN_CATEGORY_LIST%") != -1)
			{
				s = s.Replace("%PLAIN_CATEGORY_LIST%",Common.PlainCategoryList(0,_siteID));
			}
			if(_templateRAW.IndexOf("%PLAIN_SECTION_LIST%") != -1)
			{
				s = s.Replace("%PLAIN_SECTION_LIST%",Common.PlainSectionList(0,_siteID));
			}
			if(_templateRAW.IndexOf("%PLAIN_MANUFACTURER_LIST%") != -1)
			{
				s = s.Replace("%PLAIN_MANUFACTURER_LIST%",Common.PlainManufacturerList(_siteID));
			}

			//s = s.Replace("%PERSISTLIST%",thisCustomer.PersistList());
			s = s.Replace("%CUSTOMERID%",thisCustomer._customerID.ToString());
			if(_templateRAW.IndexOf("%COUNTRYBAR%") != -1)
			{
				s = s.Replace("%COUNTRYBAR%",Common.GetCountryBar(thisCustomer._localeSetting));
			}
			s = s.Replace("%STORELOCALE%",Localization.GetWebConfigLocale());
			s = s.Replace("%CUSTOMERLOCALE%",thisCustomer._localeSetting);
			s = s.Replace("%PAGEURL%",Server.UrlEncode(Common.GetThisPageName(false) + "?" + Common.ServerVariables("QUERY_STRING")));
			for(int i = 1; i <= 10; i++)
			{
				if(_templateRAW.IndexOf("%TOPIC" + i.ToString() + "%") != -1)
				{
					Topic t = new Topic("TOPIC" + i.ToString(),thisCustomer._localeSetting,_siteID);
					s = s.Replace("%TOPIC" + i.ToString() + "%",t._contents);
				}
			}return s;
		}
		
		static public String GetUserMenu(bool IsAnon)
		{
			StringBuilder tmpS = new StringBuilder(1000);
			tmpS.Append("<div id=\"userMenu\" class=\"menu\">\n");
			if(IsAnon)
			{
				tmpS.Append("<a class=\"menuItem\" href=\"signin.aspx\">Log In</a>\n");
				tmpS.Append("<a class=\"menuItem\" href=\"account.aspx\">Sign Up</a>\n");
			}
			else
			{
				tmpS.Append("<a class=\"menuItem\" href=\"account.aspx\">Update Profile</a>\n");
				tmpS.Append("<a class=\"menuItem\" href=\"signout.aspx\">Logout</a>\n");
			}
			tmpS.Append("</div>\n");
			return tmpS.ToString();
		}

		protected virtual void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
		}

		
		protected void SetError(string Text, Exception ex)
		{
			try
			{
				mPageErrorText = Text;
				mPageMode = Mode.PageError;
			}
			catch(Exception ex1)
			{
				System.Diagnostics.Debug.WriteLine(ex1.ToString());
			}

			string assemblyName = "";
			try
			{
				assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
				if (!System.Diagnostics.EventLog.SourceExists(assemblyName))
				{
					System.Diagnostics.EventLog.CreateEventSource(assemblyName, "");
				}
			}
			catch(Exception ex2)
			{
				System.Diagnostics.Debug.WriteLine(ex2.ToString());
			}

			try
			{
				System.Diagnostics.EventLog.WriteEntry(assemblyName, 
					String.Format("{0}\n{1}\n\n{2}", ErrorXRef, Text, ex.ToString()), 
					System.Diagnostics.EventLogEntryType.Error);
			}
			catch(Exception ex3)
			{
				System.Diagnostics.Debug.WriteLine(ex3.ToString());
			}
		}

		protected string ShowErrorSection()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			sb.Append("\n\n      <!-- Start End of Page -->\n\n");
			sb.Append("  </td></tr>\n");
			sb.Append("  <tr valign=top bgcolor=whitesmoke><td>\n");
			sb.Append("    An error occurred while processing your request.<br>\n");
			
			string strPageErrorText = PageErrorText;
			if (strPageErrorText.Length > 0)
			{
				sb.AppendFormat("    <br>\n{0}<br>\n<br>\n", strPageErrorText);
			}

			sb.AppendFormat("    Please reference incident {0} when contacting tech support <br>\n<br>\n<small>Error generated {1}</small><br>\n", ErrorXRef, DateTime.Now);

			return sb.ToString();
		}


	}
}
