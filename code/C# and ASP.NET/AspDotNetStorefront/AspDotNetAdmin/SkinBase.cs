using System;
using System.Web;
using System.Text;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// Will indicate what mode a SkinBase page should be shown in.
	public enum Mode : int
	{
		Normal = 0, // Render normally.
		PageError = 1 // Render the error output
	}

	
	public class SkinBase : System.Web.UI.Page
	{

		public bool _designMode;
		public Customer thisCustomer;
		public String SectionTitle;
		public String ErrorMsg;
		public bool DataUpdated;
		public bool Editing;
		
		public String _templateName;
		public String _templateRAW;
		public String _header;
		public String _footer;
		public int _siteID;

		private Mode mPageMode = Mode.Normal;
		public Mode PageMode
		{
			get { return mPageMode; }
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
				ErrorMsg = String.Empty;
				DataUpdated = false;
				Editing = false;
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
			ErrorMsg = String.Empty;
			DataUpdated = false;
			Editing = false;
			if(_templateName.Length != 0)
			{
				_siteID = 1;

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

			// We do each write in a separate try/catch block to 
			// try to make sure we get as many of the writes as
			// possible.  It also allows us to make sure that any
			// exceptions thrown during the early writes will put
			// us into "Error" mode and include the standard 
			// exception mode.

			LoadSkinTemplate();
			
			//try
			//{
				writer.Write(SkinTop()); // Write out the top of the page.
			//}
			//catch(Exception ex)
			//{
			//	SetError("Error rendering page", ex);
			//}

			//try
			//{
			//	// Show the normal page mode if we are in normal mode.  
			//	if (PageMode == Mode.Normal)
			//	{
					base.Render(writer);
			//	}
			//}
			//catch(Exception ex)
			//{
			//	SetError("Error rendering page (AAB)", ex);
			//}

			//try
			//{
			//	if (PageMode == Mode.PageError)
			//	{
			//		writer.Write(ShowErrorSection()); 				// Show the error page mode
			//	}
			//}
			//catch(Exception ex)
			//{
			//	SetError("Error rendering page (AAC)", ex);
			//}

			RenderContents(writer);

			//try
			//{
				writer.Write(SkinBottom()); 				// Write out the bottom of the page
			//}
			//catch(Exception ex)
			//{
			//	SetError("Error rendering page (AAD)", ex);
			//}
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
			s = s.Replace("%INVOCATION%",HttpContext.Current.Server.HtmlEncode(Common.PageInvocation()));
			s = s.Replace("%REFERRER%",HttpContext.Current.Server.HtmlEncode(Common.PageReferrer()));
			s = s.Replace("%SKINID%",_siteID.ToString());
			s = s.Replace("%STORE_VERSION%",Common.AppConfig("StoreVersion"));
			s = s.Replace("%SITENAME%",Common.AppConfig("StoreName"));
			s = s.Replace("%SITE_NAME%",Common.AppConfig("StoreName"));
			s = s.Replace("%SECTION_PROMPT%",Common.AppConfig("SectionPromptPlural"));
			s = s.Replace("%SIGNINOUT_TEXT%",Common.IIF(thisCustomer._isAnon , "Signin" , "Signout"));
			s = s.Replace("%SIGNINOUT_LINK%",Common.IIF(thisCustomer._isAnon , "signin.aspx" , "signout.aspx"));
			s = s.Replace("%CUSTOMERID%",thisCustomer._customerID.ToString());
			s = s.Replace("%USERNAME%",thisCustomer.FullName());
			if(s.IndexOf("%COUNTRYBAR%") != -1)
			{
				s = s.Replace("%COUNTRYBAR%",Common.GetCountryBar(thisCustomer._localeSetting));
			}
			return s;
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
