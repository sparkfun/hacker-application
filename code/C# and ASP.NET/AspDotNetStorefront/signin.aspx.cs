// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Web.Security;
using System.Data;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for signin.
	/// </summary>
	public class signin : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "<a class=\"SectionTitleText\" href=\"default.aspx\">Home</a> - Login";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(Common.Form("IsSubmit").Length != 0)
			{
				String EMailField = Common.Form("EMail").ToLower();
				String PasswordField = Common.Form("Password");

				//SEC4
				//Try it both ways
				bool ClearLogin = false;
				bool EncryptLogin = false;
        
				IDataReader rs = DB.GetRS(String.Format("select * from customer {0} where deleted=0 and email={1}  and [password]={2}",DB.GetNoLock(),DB.SQuote(EMailField),DB.SQuote(PasswordField)));
        
				ClearLogin = rs.Read();
				if (!ClearLogin)
				{
					rs = DB.GetRS(String.Format("select * from customer {0} where deleted=0 and email={1}  and [password]={2}",DB.GetNoLock(),DB.SQuote(EMailField),DB.SQuote(Common.MungeString(PasswordField))));
					EncryptLogin = rs.Read();  
				}
				if (ClearLogin && Common.AppConfigBool("EncryptPassword"))
				{
					//Logged in but the password was not encrypted so encrypt it.
					DB.ExecuteSQL(String.Format("update customer set [password]={0} where IsAdmin=1 and deleted=0 and email={1}",DB.SQuote(Common.MungeString(PasswordField)),DB.SQuote(EMailField)));
				}

				//Logged in but encrypted when it shouldn't be so update to clear.
				if (EncryptLogin && (! Common.AppConfigBool("EncryptPassword")))
				{
					//Logged in but encrypted when it shouldn't be so update to clear.
					DB.ExecuteSQL(String.Format("update customer set [password]={0} where IsAdmin=1 and deleted=0 and email={1}",DB.SQuote(PasswordField),DB.SQuote(EMailField)));
				}
				if (!(ClearLogin || EncryptLogin))
				{
					rs.Close();
					Response.Redirect("signin.aspx?returnURL=" + Server.UrlEncode(Common.QueryString("ReturnURL")) + "&errormsg=Invalid+Login&checkout=" + Common.QueryString("checkout").ToLower());
				}

				int CurrentCustomerID = thisCustomer._customerID;
				int NewCustomerID = DB.RSFieldInt(rs,"CustomerID");

				Common.ExecuteSigninLogic(CurrentCustomerID,NewCustomerID);

				// we've got a good login:
				Session["CustomerID"] = DB.RSFieldInt(rs,"CustomerID").ToString();
				Session["CustomerGUID"] = DB.RSFieldGUID(rs,"CustomerGUID");
			
				String CustomerGUID = DB.RSFieldGUID(rs,"CustomerGUID").Replace("{","").Replace("}","");
				rs.Close();

				writer.Write("<img src=\"../images/spacer.gif\" width=\"1\" height=\"100\"><br><b><center>");
				writer.Write("Sign-in complete, please wait...");
				writer.Write("<br><img src=\"../images/spacer.gif\" width=\"1\" height=\"100\"><br>");
				writer.Write("</center></b>");

				bool bPersist = (Common.Form("PersistLogin").Length != 0);
				string ReturnURL = FormsAuthentication.GetRedirectUrl(CustomerGUID,bPersist);

				FormsAuthentication.SetAuthCookie(CustomerGUID,bPersist);
                
				if (bPersist)
				{
					HttpCookie cookie = Response.Cookies[FormsAuthentication.FormsCookieName];
					cookie.Expires = DateTime.Now.Add(new TimeSpan(365,0,0,0));
				}
      
				if (ReturnURL.Length == 0)
				{
					ReturnURL = "default.aspx";
				}
				Response.AddHeader("REFRESH","1; URL=" + Server.UrlDecode(ReturnURL));
			}
			else
			{
				Topic t = new Topic("SigninPageHeader",thisCustomer._localeSetting,_siteID);
				writer.Write(t._contents);

				writer.Write("<script Language=\"JavaScript\">\n");
				writer.Write("function FrontPage_SigninForm_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("	submitonce(theForm);\n");
				writer.Write("  if (theForm.EMail.value == \"\")\n");
				writer.Write("  {\n");
				writer.Write("    alert(\"Please enter your e-mail address.\");\n");
				writer.Write("    theForm.EMail.focus();\n");
				writer.Write("	submitenabled(theForm);\n");
				writer.Write("    return (false);\n");
				writer.Write("  }\n");
				writer.Write("  if (theForm.Password.value == \"\")\n");
				writer.Write("  {\n");
				writer.Write("    alert(\"Please enter your password.\");\n");
				writer.Write("    theForm.Password.focus();\n");
				writer.Write("	submitenabled(theForm);\n");
				writer.Write("    return (false);\n");
				writer.Write("  }\n");
				writer.Write("  return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				if(Common.QueryString("errormsg").Length != 0)
				{
					writer.Write("<center><font color=\"red\"><b>" + Server.HtmlEncode(Common.QueryString("errormsg")) + " Please Try Again:</b></font><br><br></center>");
				}
				else
				{
					if(Common.QueryString("notemsg").Length != 0)
					{
						writer.Write("<center><font color=\"#0000FF\"><b>" + Server.HtmlEncode(Common.QueryString("notemsg")) + "</b></font><br><br></center>");
					}
				}

				writer.Write("<b>MEMBERS LOGIN BELOW (NEW MEMBERS CAN SIGNUP <a href=\"createaccount.aspx\">HERE</a>)</b><br><br>");

				writer.Write("<div align=\"center\">\n");
				writer.Write("<form method=\"POST\" action=\"signin.aspx?checkout=" + Common.QueryString("checkout").ToLower() + "\" onsubmit=\"return validateForm(this)\" id=\"SigninForm\" name=\"SigninForm\">");
				//V3_9			Response.Write("<input type=\"hidden\" name=\"ReturnURL\" value=\"" + Server.HtmlEncode(Server.UrlEncode(Common.QueryString("returnurl"))) + "\">");
				Response.Write("<input type=\"hidden\" name=\"ReturnURL\" value=\"" + Common.QueryString("returnurl") + "\">");
				Response.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
				writer.Write("<table width=\"80%\">\n");
				writer.Write("  <tr vAlign=\"top\">\n");
				writer.Write("    <td align=\"left\" width=\"90%\" class=\"MediumCell\"><font class=\"MediumCellText\"><b>Login\n");
				writer.Write("      Information:</b></font></td>\n");
				writer.Write("  </tr>\n");
				writer.Write("  <tr vAlign=\"top\" class=\"MediumCell\">\n");
				writer.Write("    <td align=\"left\" width=\"90%\" class=\"LightCell\">\n");
				writer.Write("      <table cellSpacing=\"5\" cellPadding=\"0\" width=\"100%\" border=\"0\">\n");
				writer.Write("        <tbody>\n");
				writer.Write("          <tr vAlign=\"baseline\">\n");
				writer.Write("            <td colSpan=\"2\"><b><font class=\"LightCellText\">Enter your e-mail\n");
				writer.Write("              address and password below:</font></b></td>\n");
				writer.Write("          </tr>\n");
				writer.Write("          <tr vAlign=\"baseline\">\n");
				writer.Write("            <td align=\"right\"><font class=\"LightCellText\">My e-mail address is:</font></td>\n");
				writer.Write("            <td><input size=\"35\" name=\"EMail\"> <input type=\"hidden\" value=\"[req][blankalert=Please enter your e-mail address][invalidalert=Please enter a valid e-mail address]\" name=\"EMail_vldt\"></td>\n");
				writer.Write("          </tr>\n");
				writer.Write("          <tr>\n");
				writer.Write("            <td align=\"right\"><font class=\"LightCellText\">My password is:</font></td>\n");
				writer.Write("            <td><input type=\"password\" size=\"35\" value name=\"Password\"> </td>\n");
				writer.Write("          </tr>\n");
				writer.Write("          <tr vAlign=\"baseline\">\n");
				writer.Write("            <td align=\"right\"><font class=\"LightCellText\">Remember Password:</font></td>\n");
				writer.Write("            <td>&nbsp;<input type=\"checkbox\" CHECKED name=\"PersistLogin\" value=\"ON\"><input type=\"hidden\" value=\"[req][blankalert=Please enter your password][invalidalert=Please enter a valid password]\" name=\"Password_vldt\"></td>\n");
				writer.Write("          </tr>\n");
				writer.Write("        </tbody>\n");
				writer.Write("      </table>\n");
				writer.Write("    </td>\n");
				writer.Write("  </tr>\n");
				writer.Write("  <tr vAlign=\"top\">\n");
				writer.Write("    <td align=\"left\" width=\"90%\">\n");
				writer.Write("      <p align=\"right\"><input type=\"submit\" value=\"Login\" name=\"B1\"></p>\n");
				writer.Write("    </td>\n");
				writer.Write("  </tr>\n");
				writer.Write("</table>\n");
				writer.Write("</form>\n");
				//writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">document.GetElementById('EMail').focus();</script>");
			
				writer.Write("<p align=\"left\"><b>FORGOT YOUR PASSWORD?</b></p><br>");

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function LostForm_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("  return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				writer.Write("<form action=\"lostpassword.aspx\" method=\"post\" onsubmit=\"return (validateForm(this) && LostForm_Validator(this))\" id=\"LostForm\" name=\"LostForm\">");
				writer.Write("<p align=\"left\">If you can provide us with some additional information, we can e-mail your password to the e-mail address specified in your account. If you are having problems, or if you can't remember any of the information, you will need to <a href=\"" + SE.MakeDriverLink("Contact") + "\">contact</a> our support staff</a>.</p><br>");
				writer.Write("<table width=\"80%\">\n");
				writer.Write("  <tr vAlign=\"top\" class=\"MediumCell\">\n");
				writer.Write("    <td align=\"left\" width=\"80%\"><font class=\"MediumCellText\"><b>User\n");
				writer.Write("      Verification:</b></font></td>\n");
				writer.Write("  </tr>\n");
				writer.Write("  <tr vAlign=\"top\" class=\"MediumCell\">\n");
				writer.Write("    <td align=\"left\" width=\"80%\" class=\"LightCell\">\n");
				writer.Write("      <table cellSpacing=\"5\" cellPadding=\"0\" width=\"80%\" border=\"0\">\n");
				writer.Write("        <tbody>\n");
				writer.Write("          <tr vAlign=\"baseline\">\n");
				writer.Write("            <td align=\"right\"><font class=\"LightCellText\">My e-mail address is:</font></td>\n");
				writer.Write("            <td><input size=\"35\" name=\"EMail\"> <input type=\"hidden\" value=\"[req][blankalert=Please enter your e-mail address]\" name=\"EMail_vldt\"></td>\n");
				writer.Write("          </tr>\n");
				writer.Write("        </tbody>\n");
				writer.Write("      </table>\n");
				writer.Write("    </td>\n");
				writer.Write("  </tr>\n");
				writer.Write("  <tr vAlign=\"top\" class=\"MediumCell\">\n");
				writer.Write("    <td align=\"left\" width=\"80%\"><img height=\"2\" src=\"images/spacer.gif\" width=\"2\" border=\"0\"></td>\n");
				writer.Write("  </tr>\n");
				writer.Write("  <tr vAlign=\"top\">\n");
				writer.Write("    <td align=\"left\" width=\"80%\">\n");
				writer.Write("      <p align=\"right\"><input type=\"submit\" value=\"Request Password\" name=\"B1\"></p>\n");
				writer.Write("    </td>\n");
				writer.Write("  </tr>\n");
				writer.Write("</table>\n");
				writer.Write("</form>\n");
				writer.Write("</div>\n");

				writer.Write("\n<script type=\"text/javascript\">\n");
				writer.Write("document.SigninForm.EMail.focus();\n");
				writer.Write("</script>\n");
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
