// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for signout.
	/// </summary>
	public class signout : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");
			
			int _siteID = 1;
			
			//Common.SetCookie(Common.UserCookieName(),"",new TimeSpan(-1,0,0,0,0));
			Common.SetSessionCookie("AffiliateID","");
			Common.SetCookie("LocaleSetting",Localization.GetWebConfigLocale(),new TimeSpan(1000,0,0,0,0));

			Session.Clear();
			Session.Abandon();
			FormsAuthentication.SignOut();
		
			Response.Write("<html>\n");
			Response.Write("<head>\n");
			Response.Write("<title>AspDotNetStorefront Admin - Signout</title>\n");
			Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\">\n");
			Response.Write("<link rel=\"stylesheet\" href=\"skins/Skin_" + _siteID.ToString() +  "/style.css\" type=\"text/css\">\n");
			Response.Write("</head>\n");
			Response.Write("<body bgcolor=\"#FFFFFF\" topmargin=\"0\" marginheight=\"0\" bottommargin=\"0\" marginwidth=\"0\" rightmargin=\"0\">\n");
			Response.Write("<table width=\"100%\" height=\"100%\" cellpadding=\"0\" cellspacing=\"0\">\n");
			Response.Write("<tr><td width=\"100%\" height=\"100%\" align=\"center\" valign=\"middle\">\n");
			Response.Write("<img src=\"skins/skin_" + _siteID.ToString() + "/images/signout.jpg\">\n");
			Response.Write("</td></tr>\n");
			Response.Write("</table>\n");
			Response.Write("<script language=\"javascript\">\n");
			Response.Write("top.location='default.aspx';\n");
			Response.Write("</script>\n");
			Response.Write("</body>\n");
			Response.Write("</html>\n");

		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
