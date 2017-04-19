// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Web;
using System.Web.Security;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for signin.
	/// </summary>
	public class signin : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			if(Common.ServerVariables("HTTP_HOST").ToLower().IndexOf(Common.AppConfig("LiveServer").ToLower()) != -1 && Common.ServerVariables("HTTP_HOST").ToLower().IndexOf("www") == -1)
			{
				if(Common.AppConfigBool("RedirectLiveToWWW"))
				{
					Response.Redirect("http://www." + Common.AppConfig("LiveServer").ToLower() + "/" + Common.GetAdminDir() + "/signin.aspx?" + Common.ServerVariables("QUERY_STRING"));
				}
			}
			if(Common.AppConfigBool("UseSSL"))
			{
				if(Common.OnLiveServer() && Common.ServerVariables("SERVER_PORT_SECURE") != "1")
				{
					Response.Redirect(Common.GetStoreHTTPLocation(true) + Common.GetThisPageName(false) + "?" + Common.ServerVariables("QUERY_STRING"));
				}
			}
			
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");

			if(Common.AppConfig("EncryptKey") == "WIZARD")
			{
				Common.SetAppConfig("EncryptKey",Common.GetRandomNumber(1000,1000000).ToString() + Common.GetRandomNumber(1000,1000000).ToString() + Common.GetRandomNumber(1000,1000000).ToString(),false);
			}
			
			int _siteID = 1;

			if(Common.Form("IsSubmit").Length != 0)
			{
				String EMailField = Common.Form("EMail").ToLower();
				String PasswordField = Common.Form("Password");

				//SEC4
				//Try login both ways
				bool ClearLogin = false;
				bool EncryptLogin = false;
        
				IDataReader rs = DB.GetRS(String.Format("select * from customer {0} where IsAdmin=1 and deleted=0 and email={1} and [password]={2}",DB.GetNoLock(),DB.SQuote(EMailField),DB.SQuote(PasswordField)));
        
				ClearLogin = rs.Read();
				if (!ClearLogin)
				{
					rs = DB.GetRS(String.Format("select * from customer {0} where IsAdmin=1 and deleted=0 and email={1} and [password]={2}",DB.GetNoLock(),DB.SQuote(EMailField),DB.SQuote(Common.MungeString(PasswordField))));
					EncryptLogin = rs.Read();  
				}

				if (ClearLogin && Common.AppConfigBool("EncryptPassword"))
				{
					//Logged in but the password was not encrypted so encrypt it.
					DB.ExecuteSQL(String.Format("update customer set [password]={0} where IsAdmin=1 and deleted=0 and email={1}",DB.SQuote(Common.MungeString(PasswordField)),DB.SQuote(EMailField)));
				}

				if (EncryptLogin && (! Common.AppConfigBool("EncryptPassword")))
				{
					//Logged in but encrypted when it shouldn't be so update to clear.
					DB.ExecuteSQL(String.Format("update customer set [password]={0} where IsAdmin=1 and deleted=0 and email={1}",DB.SQuote(PasswordField),DB.SQuote(EMailField)));
				}
				if (! (ClearLogin || EncryptLogin))
				{
					rs.Close();
					DB.ExecuteSQL("insert into FailedAdminLoginAttempts(FailedEMail,FailedPassword,IPAddress) values(" + DB.SQuote(EMailField) + "," + DB.SQuote(PasswordField) + "," + DB.SQuote(Common.ServerVariables("REMOTE_ADDR")) + ")");
					Response.Redirect("signin.aspx?errormsg=Invalid+Login&returnurl=" + Server.UrlEncode(Common.Form("ReturnURL")));
				}

				// we've got a good login:
				String LocaleSetting = DB.RSField(rs,"LocaleSetting");
				if(LocaleSetting.Length == 0)
				{
					LocaleSetting = Localization.GetWebConfigLocale();
				}
				Session["CustomerID"] = DB.RSFieldInt(rs,"CustomerID").ToString();
				Session["CustomerGUID"] = DB.RSFieldGUID(rs,"CustomerGUID");
				String CustomerGUID = DB.RSFieldGUID(rs,"CustomerGUID");
				CustomerGUID = CustomerGUID.Replace("{","").Replace("}","");
				rs.Close();

				Common.SetSessionCookie("AffiliateID","");
				Common.SetCookie("LocaleSetting",LocaleSetting,new TimeSpan(1000,0,0,0,0));
			
				bool bPersist = (Common.Form("PersistLogin").Length != 0);
				string ReturnURL = "default.aspx"; // FORCE THIS so frames get reloaed // FormsAuthentication.GetRedirectUrl(CustomerGUID,bPersist);

				FormsAuthentication.SetAuthCookie(CustomerGUID,bPersist);
                
				if (bPersist)
				{
					HttpCookie cookie = Response.Cookies[FormsAuthentication.FormsCookieName];
					cookie.Expires = DateTime.Now.Add(new TimeSpan(365,0,0,0));
				}

				Response.Write("<html>\n");
				Response.Write("<head>\n");
				Response.Write("<title>AspDotNetStorefront Admin - Signin</title>\n");
				Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\">\n");
				Response.Write("<link rel=\"stylesheet\" href=\"skins/Skin_" + _siteID.ToString() +  "/style.css\" type=\"text/css\">\n");
				Response.Write("</head>\n");
				Response.Write("<body bgcolor=\"#FFFFFF\" topmargin=\"0\" marginheight=\"0\" bottommargin=\"0\" marginwidth=\"0\" rightmargin=\"0\">\n");
				Response.Write("<script type=\"text/javascript\" src=\"formValidate.js\"></script>\n");
				Response.Write("<table width=\"100%\" height=\"100%\" cellpadding=\"0\" cellspacing=\"0\">\n");
				Response.Write("<tr><td width=\"100%\" height=\"100%\" align=\"center\" valign=\"middle\">\n");
				Response.Write("<img src=\"skins/skin_" + _siteID.ToString() + "/images/signin.jpg\">\n");
				Response.Write("</td></tr>\n");
				Response.Write("</table>\n");
				Response.Write("<script language=\"javascript\">\n");
				Response.Write("top.location='" + ReturnURL + "';\n");
				Response.Write("</script>\n");
				Response.Write("</body>\n");
				Response.Write("</html>\n");
			}
			else
			{
			
				Response.Write("<html>\n");
				Response.Write("<head>\n");
				Response.Write("<title>AspDotNetStorefront Admin for Store: " + Common.AppConfig("StoreName") + "</title>\n");
				Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\">\n");
				Response.Write("<link rel=\"stylesheet\" href=\"skins/Skin_" + _siteID.ToString() +  "/style.css\" type=\"text/css\">\n");
				Response.Write("</head>\n");
				Response.Write("<body bgcolor=\"#FFFFFF\" topmargin=\"0\" marginheight=\"0\" bottommargin=\"0\" marginwidth=\"0\" rightmargin=\"0\">\n");
				Response.Write("<script type=\"text/javascript\" src=\"formValidate.js\"></script>\n");


				Response.Write("<center>\n");
				Response.Write("<table width=564 cellpadding=0 cellspacing=0 border=0>\n");
				Response.Write("	<tr>\n");
				Response.Write("		<td><img src=\"images/spacer.gif\" height=20 width=1></td>\n");
				Response.Write("	</tr>\n");
				Response.Write("	<tr>\n");
				Response.Write("		<td><table width=564 cellpadding=0 cellspacing=0>\n");
				Response.Write("			<tr>\n");
				Response.Write("				<td colspan=4><img alt=\"Site Login\" src=\"skins/skin_" + _siteID.ToString() + "/images/title.gif\" width=564 height=116></td>\n");
				//Response.Write("				<td style=\"background: url(skins/skin_" + _siteID.ToString() + "/images/title_bg.gif) repeat-x;\" width=100%></td>\n");
				//Response.Write("				<td><img src=\"skins/skin_" + _siteID.ToString() + "/images/fox1.gif\" width=105 height=116></td>\n");
				//Response.Write("				<td><img src=\"skins/skin_" + _siteID.ToString() + "/images/fox2.gif\" width=19 height=85></td>\n");
				Response.Write("			</tr>\n");
				Response.Write("			</table><td>\n");
				Response.Write("	</tr>\n");
				Response.Write("	<tr>\n");
				Response.Write("		<td><table cellpadding=0 cellspacing=0 width=\"100%\" border=0>\n");
				Response.Write("<tr>\n");
				Response.Write("<td bgcolor=\"#CED6EA\"><img src=\"images/spacer.gif\" height=1 width=1></td>\n");
				Response.Write("<td align=center>");

				Response.Write("<script Language=\"JavaScript\">\n");
				Response.Write("function FrontPage_SigninForm_Validator(theForm)\n");
				Response.Write("{\n");
				Response.Write("submitonce(theForm);\n");
				Response.Write("  if (theForm.EMail.value == \"\")\n");
				Response.Write("  {\n");
				Response.Write("    alert(\"Please enter your e-mail address.\");\n");
				Response.Write("    theForm.EMail.focus();\n");
				Response.Write("	submitenabled(theForm);\n");
				Response.Write("    return (false);\n");
				Response.Write("  }\n");
				Response.Write("  if (theForm.Password.value == \"\")\n");
				Response.Write("  {\n");
				Response.Write("    alert(\"Please enter your password.\");\n");
				Response.Write("    theForm.Password.focus();\n");
				Response.Write("	submitenabled(theForm);\n");
				Response.Write("    return (false);\n");
				Response.Write("  }\n");
				Response.Write("  return (true);\n");
				Response.Write("}\n");
				Response.Write("</script>\n");

				if(Common.QueryString("errormsg").Length != 0)
				{
					Response.Write("<center><font color=\"#FF0000\"><b>" + Common.QueryString("errormsg") + " Please Try Again:</b></font><br><br></center>");
				}
				else
				{
					if(Common.QueryString("notemsg").Length != 0)
					{
						Response.Write("<center><font color=\"#0000FF\"><b>" + Common.QueryString("notemsg") + "</b></font><br><br></center>");
					}
				}

				Response.Write("        <table cellSpacing=\"0\" cellPadding=\"0\" width=\"80%\" border=\"0\">");
				Response.Write("<form method=\"POST\" action=\"signin.aspx\" onsubmit=\"return validateForm(this)\" id=\"SigninForm\" name=\"SigninForm\">");
				Response.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
				Response.Write("<input type=\"hidden\" name=\"ReturnURL\" value=\"" + Common.QueryString("ReturnURL") + "\">\n");
				Response.Write("            <tr vAlign=\"top\">");
				Response.Write("              <td align=\"left\" height=\"18\" valign=\"middle\" bgcolor=\"#6487DB\"><font class=\"DarkCellText\" size=\"2\"><b>&nbsp;Login Information:</b></font></td>");
				Response.Write("            </tr>");
				Response.Write("            <tr vAlign=\"top\" bgcolor=\"#6487DB\">");
				Response.Write("              <td align=\"left\" width=\"100%\" bgColor=\"#e0e0e0\">");
				Response.Write("                <table cellSpacing=\"5\" cellPadding=\"0\" width=\"100%\" border=\"0\">");
				Response.Write("                    <tr vAlign=\"baseline\">");
				Response.Write("                      <td colspan=2><b>Enter your e-mail address and password below:</b></td>");
				Response.Write("                    </tr>");
				Response.Write("                    <tr vAlign=\"baseline\">");
				Response.Write("                      <td align=\"right\">My e-mail address is:</td>");
				Response.Write("                      <td><input type=\"text\" name=\"EMail\" size=\"35\"><input type=\"hidden\" name=\"EMail_vldt\" value=\"[req][blankalert=Please enter your e-mail address][invalidalert=Please enter a valid e-mail address]\"></td>");
				Response.Write("                    </tr>");
				Response.Write("                    <tr vAlign=\"baseline\">");
				Response.Write("                      <td align=\"right\">My password is:</td>");
				Response.Write("                      <td><input type=\"Password\" name=\"Password\" size=\"35\"><input type=\"Hidden\" name=\"Password_vldt\" value=\"[req][blankalert=Please enter your password][invalidalert=Please enter a valid password]\"></td>");
				Response.Write("                    </tr>");
				Response.Write("                    <tr vAlign=\"baseline\">");
				Response.Write("                      <td colspan=2 align=\"center\">Remember Password: <input type=\"checkbox\" name=\"PersistLogin\" checked></td>");
				Response.Write("                    </tr>");
				Response.Write("                </table>");
				Response.Write("              </td>");
				Response.Write("            </tr>");
				Response.Write("            <tr vAlign=\"top\" bgcolor=\"#6487DB\">");
				Response.Write("              <td align=\"left\"><img height=\"2\" src=\"images/spacer.gif\" width=\"2\" border=\"0\"></td>");
				Response.Write("            </tr>");
				Response.Write("            <tr vAlign=\"top\">");
				Response.Write("              <td align=\"left\"><p align=\"right\"><input type=\"submit\" value=\"Sign In\" name=\"B1\"></td>");
				Response.Write("            </tr>");
				Response.Write("      </form>");
				Response.Write("        </table>");
			

				Response.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				Response.Write("function LostForm_Validator(theForm)\n");
				Response.Write("{\n");
				Response.Write("  return (true);\n");
				Response.Write("}\n");
				Response.Write("</script>\n");

				Response.Write("        <table cellSpacing=\"0\" cellPadding=\"0\" width=\"80%\" border=\"0\">");
				Response.Write("      <form action=\"lostpassword.aspx\" method=\"post\" onsubmit=\"return (validateForm(this) && LostForm_Validator(this))\" id=\"LostForm\" name=\"LostForm\">");
				Response.Write("            <tr vAlign=\"top\">");
				Response.Write("              <td align=\"left\"><b>FORGOT YOUR PASSWORD?</b><br><br>If you forgot your password, enter your e-mail address below. Your password will be sent to the e-mail address specified in your account profile.<p></p></td>");
				Response.Write("            </tr>");
				Response.Write("            <tr vAlign=\"top\" bgcolor=\"#6487DB\">");
				Response.Write("              <td align=\"left\" height=\"18\" valign=\"middle\"><font class=\"DarkCellText\" size=\"2\"><b>&nbsp;User Verification:</b></font></td>");
				Response.Write("            </tr>");
				Response.Write("            <tr vAlign=\"top\" bgcolor=\"#6487DB\">");
				Response.Write("              <td align=\"left\" bgColor=\"#e0e0e0\" colSpan=\"3\">");
				Response.Write("                <table cellSpacing=\"5\" cellPadding=\"0\" width=\"100%\" border=\"0\">");
				Response.Write("                    <tr vAlign=\"baseline\">");
				Response.Write("                      <td align=\"right\">My e-mail address is:</td>");
				Response.Write("                      <td>");
				Response.Write("                      	<input name=\"EMail\" size=\"35\">");
				Response.Write("                        <input type=\"hidden\" name=\"EMail_vldt\" value=\"[req][blankalert=Please enter your e-mail address]\">");
				Response.Write("                      	</td>");
				Response.Write("                    </tr>");
				Response.Write("                </table>");
				Response.Write("              </td>");
				Response.Write("            </tr>");
				Response.Write("            <tr vAlign=\"top\" bgcolor=\"#6487DB\">");
				Response.Write("              <td align=\"left\"><img height=\"2\" src=\"images/spacer.gif\" width=\"2\" border=\"0\"></td>");
				Response.Write("            </tr>");
				Response.Write("            <tr vAlign=\"top\">");
				Response.Write("              <td align=\"left\"><p align=\"right\"><input type=\"submit\" value=\"Request Password\" name=\"B1\"></p></td>");
				Response.Write("            </tr>");
				Response.Write("</form>");
				Response.Write("        </table>");

				Response.Write("</td>");
				Response.Write("<td width=20 background=\"skins/skin_" + _siteID.ToString() + "/images/righttableline.gif\" height=\"100\"></td>\n");
				Response.Write("</tr>\n");
				Response.Write("</table>\n");

				Response.Write("		</td>\n");
				Response.Write("	</tr>\n");

				Response.Write("<tr>\n");
				Response.Write("<td><table cellpadding=0 cellspacing=0>\n");
				Response.Write("<tr>\n");
				Response.Write("<td valign=top><img src=\"skins/skin_" + _siteID.ToString() + "/images/footer1.gif\" height=\"34\" width=\"48\"></td>\n");
				Response.Write("<td background=\"skins/skin_" + _siteID.ToString() + "/images/footer2.gif\" width=100%></td>\n");
				Response.Write("<td valign=top><img src=\"skins/skin_" + _siteID.ToString() + "/images/footer3.gif\" height=\"34\" width=\"497\"></td>\n");
				Response.Write("<td><img src=\"images/spacer.gif\" height=1 width=19></td>\n");
				Response.Write("</tr>\n");
				Response.Write("</table></td></tr>\n");

				Response.Write("</table>\n");
				Response.Write("</center>\n");

				Response.Write("\n<script type=\"text/javascript\">\n");
				Response.Write("document.SigninForm.EMail.focus();\n");
				Response.Write("</script>\n");


				Response.Write("</body>\n");
				Response.Write("</html>\n");
			}
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
