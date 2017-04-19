// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for lat_signin.
	/// </summary>
	public class lat_signin : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			base._disableLeftAndRightCols = true; // page gets two wide
			RequireSecurePage();
			SectionTitle = "<a href=\"lat_account.aspx\">" + Common.AppConfig("AffiliateProgramName") + "</a> - Sign In";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{

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

			
			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function LATSignupForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("  submitonce(theForm);\n");
			writer.Write("  if (theForm.FirstName.value == \"\" && theForm.LastName.value == \"\")\n");
			writer.Write("  {\n");
			writer.Write("    alert(\"You must enter at least one of First Name or Last Name.\");\n");
			writer.Write("    theForm.FirstName.focus();\n");
			writer.Write("    submitenabled(theForm);\n");
			writer.Write("    return (false);\n");
			writer.Write("  }\n");
			writer.Write("  if (theForm.Password.value.length < 3)\n");
			writer.Write("  {\n");
			writer.Write("    alert(\"Your password must be at least 3 characters long.\");\n");
			writer.Write("    theForm.Password.focus();\n");
			writer.Write("    submitenabled(theForm);\n");
			writer.Write("    return (false);\n");
			writer.Write("  }\n");
			writer.Write("  if (theForm.Password.value != theForm.Password2.value)\n");
			writer.Write("  {\n");
			writer.Write("    alert(\"Your passwords do not match. Please re-enter your password to confirm.\");\n");
			writer.Write("    theForm.Password2.focus();\n");
			writer.Write("    submitenabled(theForm);\n");
			writer.Write("    return (false);\n");
			writer.Write("  }\n");
			writer.Write("  if (theForm.State.selectedIndex < 1)\n");
			writer.Write("  {\n");
			writer.Write("    alert(\"Please select your state.\");\n");
			writer.Write("    theForm.State.focus();\n");
			writer.Write("    submitenabled(theForm);\n");
			writer.Write("    return (false);\n");
			writer.Write("  }\n");
			writer.Write("  return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");
			
			writer.Write("<table cellSpacing=\"5\" cellPadding=\"5\" border=\"0\" width=\"100%\">\n");
			writer.Write("  <tbody>\n");
			writer.Write("    <tr>\n");
			writer.Write("      <td vAlign=\"top\" align=\"top\">\n");

			writer.Write("        <table cellSpacing=\"0\" cellPadding=\"0\" width=\"171\" bgColor=\"#AAAAAA\" border=\"0\">\n");
			writer.Write("          <tbody>\n");
			writer.Write("            <tr>\n");
			writer.Write("              <td bgcolor=\"#AAAAAA\" height=\"18\"><b class=\"small\" style=\"COLOR: #ffffff\">&nbsp;Learn More</b></td>\n");
			writer.Write("            </tr>\n");
			writer.Write("            <tr vAlign=\"center\" align=\"middle\">\n");
			writer.Write("              <td>\n");
			writer.Write("                <table cellSpacing=\"0\" cellPadding=\"1\" width=\"171\" bgColor=\"#AAAAAA\" border=\"0\">\n");
			writer.Write("                  <tbody>\n");
			writer.Write("                    <tr>\n");
			writer.Write("                      <td width=\"100%\">\n");
			writer.Write("                        <table cellSpacing=\"0\" cellPadding=\"4\" width=\"100%\" bgColor=\"#AAAAAA\" border=\"0\">\n");
			writer.Write("                          <tbody>\n");
			writer.Write("                            <tr>\n");
			writer.Write("                              <td vAlign=\"top\" align=\"left\" width=\"100%\" bgColor=\"#ffffff\">Join our rapidly growing network of " + Common.AppConfig("AffiliateProgramName") + " -- it's simple and it works! <a href=\"t-affiliate.aspx\">Read more</a>.\n");
			writer.Write("                              </td>\n");
			writer.Write("                            </tr>\n");
			writer.Write("                          </tbody>\n");
			writer.Write("                        </table>\n");
			writer.Write("                      </td>\n");
			writer.Write("                    </tr>\n");
			writer.Write("                  </tbody>\n");
			writer.Write("                </table>\n");
			writer.Write("              </td>\n");
			writer.Write("            </tr>\n");
			writer.Write("          </tbody>\n");
			writer.Write("        </table>\n");
			writer.Write("        <p>&nbsp;\n");
			writer.Write("        <table cellSpacing=\"0\" cellPadding=\"0\" width=\"171\" bgColor=\"#AAAAAA\" border=\"0\">\n");
			writer.Write("          <tbody>\n");
			writer.Write("            <tr>\n");
			writer.Write("              <td bgcolor=\"#AAAAAA\" height=\"18\"><b style=\"color: #ffffff\" class=\"small\">&nbsp;Need Help?</b></td>\n");
			writer.Write("            </tr>\n");
			writer.Write("            <tr vAlign=\"center\" align=\"middle\">\n");
			writer.Write("              <td>\n");
			writer.Write("                <table cellSpacing=\"0\" cellPadding=\"1\" width=\"171\" bgColor=\"#AAAAAA\" border=\"0\">\n");
			writer.Write("                  <tbody>\n");
			writer.Write("                    <tr>\n");
			writer.Write("                      <td width=\"100%\">\n");
			writer.Write("                        <table cellSpacing=\"0\" cellPadding=\"4\" width=\"100%\" bgColor=\"#AAAAAA\" border=\"0\">\n");
			writer.Write("                          <tbody>\n");
			writer.Write("                            <tr>\n");
			writer.Write("                              <td vAlign=\"top\" align=\"left\" width=\"100%\" bgColor=\"#ffffff\">• <a href=\"mailto:" + Common.AppConfig("AffiliateEMailAddress") + "\">Ask A Question</a><br>\n");
			writer.Write("                            </tr>\n");
			writer.Write("                          </tbody>\n");
			writer.Write("                        </table>\n");
			writer.Write("                      </td>\n");
			writer.Write("                    </tr>\n");
			writer.Write("                  </tbody>\n");
			writer.Write("                </table>\n");
			writer.Write("              </td>\n");
			writer.Write("            </tr>\n");
			writer.Write("          </tbody>\n");
			writer.Write("        </table>\n");
			writer.Write("      </td>\n");
			writer.Write("      <td width=\"100%\" align=\"left\" valign=\"top\"><img border=\"0\" src=\"skins/skin_" + _siteID.ToString() + "/images/affiliateheader_small.jpg\" width=\"328\" height=\"70\"><br>\n");
			writer.Write("        <br>\n");

			// BEGIN SIGNIN FORMS:

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

			writer.Write("<blockquote><blockquote><b>" + Common.AppConfig("AffiliateProgramName") + " MEMBERS LOGIN BELOW (NEW MEMBERS CAN SIGNUP <a href=\"lat_signup.aspx\">HERE</a>)</b><br><br>");

			writer.Write("<br>");
			writer.Write("<table cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"490\">");
			writer.Write("  <tr>");
			writer.Write("    <td rowspan=\"2\">");
			writer.Write("	  <form method=\"POST\" action=\"lat_signin_process.aspx\" onsubmit=\"return validateForm(this)\" id=\"SigninForm\" name=\"SigninForm\">");
			writer.Write("<input type=\"hidden\" name=\"ReturnURL\" value=\"" + Server.HtmlEncode(Server.UrlEncode(Common.QueryString("returnurl"))) + "\">");
			writer.Write("        <table cellSpacing=\"0\" cellPadding=\"0\" width=\"490\" border=\"0\">");
			writer.Write("            <tr vAlign=\"top\">");
			writer.Write("              <td align=\"left\" width=\"454\" bgColor=\"#" + Common.AppConfig("MediumCellColor") + "\"><font class=\"MediumCellText\"><b>&nbsp;Login Information:</b></font></td>");
			writer.Write("              <td align=\"right\" width=\"18\" bgColor=\"#" + Common.AppConfig("MediumCellColor") + "\"></td>");
			writer.Write("            </tr>");
			writer.Write("            <tr vAlign=\"top\" bgColor=\"#" + Common.AppConfig("MediumCellColor") + "\">");
			writer.Write("              <td align=\"left\" width=\"481\" bgColor=\"#" + Common.AppConfig("LightCellColor") + "\" colspan=\"3\">");
			writer.Write("                <table cellSpacing=\"5\" cellPadding=\"0\" width=\"480\" border=\"0\">");
			writer.Write("                    <tr vAlign=\"baseline\">");
			writer.Write("                      <td colspan=\"3\"><b><font class=\"LightCellText\">Enter your " + Common.AppConfig("AffiliateProgramName") + " e-mail address and password below:</font></b></td>");
			writer.Write("                    </tr>");
			writer.Write("                    <tr vAlign=\"baseline\">");
			writer.Write("                      <td align=\"right\"><font class=\"LightCellText\">My e-mail address is:</font></td>");
			writer.Write("                      <td>");
			writer.Write("                      	<input type=\"text\" name=\"EMail\" size=\"35\">");
			writer.Write("                      	<input type=\"hidden\" name=\"EMail_vldt\" value=\"[req][blankalert=Please enter your e-mail address][invalidalert=Please enter a valid e-mail address]\">");
			writer.Write("                      </td>");
			writer.Write("                    </tr>");
			writer.Write("                    <tr vAlign=\"baseline\">");
			writer.Write("                      <td align=\"right\"><font class=\"LightCellText\">My password is:</font></td>");
			writer.Write("                      <td>");
			writer.Write("                      	<input type=\"Password\" name=\"Password\" size=\"35\">");
			writer.Write("                      	<input type=\"Hidden\" name=\"Password_vldt\" value=\"[req][blankalert=Please enter your password][invalidalert=Please enter a valid password]\">");
			writer.Write("                      </td>");
			writer.Write("                    </tr>");
			writer.Write("                    <tr vAlign=\"baseline\">");
			writer.Write("                      <td align=\"center\" colspan=\"2\">");
			writer.Write("						<font class=\"LightCellText\">Remember Password:</font> <input type=\"checkbox\" name=\"PersistLogin\" checked>");
			writer.Write("                      </td>");
			writer.Write("                    </tr>");
			writer.Write("                </table>");
			writer.Write("              </td>");
			writer.Write("            </tr>");
			writer.Write("            <tr vAlign=\"top\" bgColor=\"#" + Common.AppConfig("MediumCellColor") + "\">");
			writer.Write("              <td align=\"left\" width=\"481\" colspan=\"2\"><img height=\"2\" src=\"images/spacer.gif\" width=\"2\" border=\"0\"></td>");
			writer.Write("            </tr>");
			writer.Write("            <tr vAlign=\"top\">");
			writer.Write("              <td align=\"left\" width=\"481\" colspan=\"2\"><p align=\"right\"><input type=\"submit\" value=\"Sign In\" name=\"B1\"></td>");
			writer.Write("            </tr>");
			writer.Write("        </table>");
			writer.Write("      </form>");
			//writer.Write("	  <script type=\"text/javascript\" Language=\"JavaScript\">SigninForm.EMail.focus();</script>");
			writer.Write("    </td>");
			writer.Write("    <td width=\"100%\"><br>");
			writer.Write("    </td>");
			writer.Write("  </table>");
			
			writer.Write("<b><a name=\"lostpassword\">FORGOT YOUR PASSWORD?</a></b><br><br>");

			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function LostForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("	submitonce(theForm);\n");
			writer.Write("  if (theForm.EMail.value.length==0)\n");
			writer.Write("	{\n");
			writer.Write("	alert(\"Please enter your e-mail address or home phone number.\");\n");
			writer.Write("	theForm.EMail.focus();\n");
			writer.Write("	submitenabled(theForm);\n");
			writer.Write("	return (false);\n");
			writer.Write("	}\n");
			writer.Write("  return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");

			writer.Write("      If you can provide us with some additional information, we can e-mail your password to the e-mail address specified in your account. ");
			writer.Write("      If you are having problems, or if you can't remember any of the information, you will need to <a href=\"" + SE.MakeDriverLink("Contact") + "\">contact</a> our support staff</a>.<br><br>");
			writer.Write("<table cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"490\">");
			writer.Write("  <tr>");
			writer.Write("    <td rowspan=\"2\">");
			writer.Write("      <form action=\"lat_lostpassword.aspx\" method=\"post\" onsubmit=\"return (validateForm(this) && LostForm_Validator(this))\" id=\"LostForm\" name=\"LostForm\">");
			writer.Write("        <table cellSpacing=\"0\" cellPadding=\"0\" width=\"490\" border=\"0\">");
			writer.Write("            <tr vAlign=\"top\" bgColor=\"#" + Common.AppConfig("MediumCellColor") + "\">");
			writer.Write("              <td align=\"left\" width=\"454\"><font class=\"MediumCellText\"><b>&nbsp;User Verification:</b></font></td>");
			writer.Write("              <td align=\"right\" width=\"18\"></td>");
			writer.Write("            </tr>");
			writer.Write("            <tr vAlign=\"top\" bgColor=\"#" + Common.AppConfig("MediumCellColor") + "\">");
			writer.Write("              <td align=\"left\" width=\"481\" bgColor=\"#" + Common.AppConfig("LightCellColor") + "\" colspan=\"3\">");
			writer.Write("                <table cellSpacing=\"5\" cellPadding=\"0\" width=\"480\" border=\"0\">");
			writer.Write("                    <tr vAlign=\"baseline\">");
			writer.Write("                      <td align=\"right\"><font class=\"LightCellText\">My e-mail address is:</font></td>");
			writer.Write("                      <td>");
			writer.Write("                      	<input name=\"EMail\" size=\"35\">");
			writer.Write("                      	<input type=\"hidden\" name=\"EMail_vldt\" value=\"[blankalert=Please enter your e-mail address][invalidalert=Please enter a valid e-mail address]\">");
			writer.Write("                      	</td>");
			writer.Write("                    </tr>");
			writer.Write("                </table>");
			writer.Write("              </td>");
			writer.Write("            </tr>");
			writer.Write("            <tr vAlign=\"top\" bgColor=\"#" + Common.AppConfig("MediumCellColor") + "\">");
			writer.Write("              <td align=\"left\" width=\"481\" colspan=\"2\"><img height=\"2\" src=\"images/spacer.gif\" width=\"2\" border=\"0\"></td>");
			writer.Write("            </tr>");
			writer.Write("            <tr vAlign=\"top\">");
			writer.Write("              <td align=\"left\" width=\"481\" colspan=\"2\"><p align=\"right\"><input type=\"submit\" value=\"Request Password\" name=\"B1\"></p></td>");
			writer.Write("            </tr>");
			writer.Write("        </table>");
			writer.Write("      </form>");
			writer.Write("    </td>");
			writer.Write("    <td width=\"100%\"><br>");
			writer.Write("    </td>");
			writer.Write("  </table>");

			// END SIGNIN FORMS

			writer.Write("      </td>\n");
			writer.Write("    </tr>\n");
			writer.Write("  </tbody>\n");
			writer.Write("</table>\n");

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
