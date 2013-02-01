using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for lat_signup.
	/// </summary>
	public class lat_signup : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			base._disableLeftAndRightCols = true; // page gets two wide
			RequireSecurePage();
			SectionTitle = "<a href=\"lat_account.aspx\">" + Common.AppConfig("AffiliateProgramName") + "</a> - Sign Up";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
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
			writer.Write("  if (theForm.Password.value.length < 4)\n");
			writer.Write("  {\n");
			writer.Write("    alert(\"Your password must be at least 4 characters long.\");\n");
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
			writer.Write("  if (!theForm.AgreeToTermsAndConditions.checked)\n");
			writer.Write("  {\n");
			writer.Write("    alert(\"Please review and accept the " + Common.AppConfig("AffiliateProgramName").Replace("\"","'") + " terms and conditions before joining.\");\n");
			writer.Write("    theForm.AgreeToTermsAndConditions.focus();\n");
			writer.Write("    submitenabled(theForm);\n");
			writer.Write("    return (false);\n");
			writer.Write("  }\n");
			writer.Write("  return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");
			
			writer.Write(Common.GetJSPopupRoutines());
			
			writer.Write("<table cellSpacing=\"5\" cellPadding=\"5\" border=\"0\" width=\"100%\">\n");
			writer.Write("  <tbody>\n");
			writer.Write("    <tr>\n");
			writer.Write("      <td vAlign=\"top\" align=\"top\">\n");
			writer.Write("        <table cellSpacing=\"0\" cellPadding=\"0\" width=\"171\" bgColor=\"#AAAAAA\" border=\"0\">\n");
			writer.Write("          <tbody>\n");
			writer.Write("            <tr>\n");
			writer.Write("              <td bgcolor=\"#AAAAAA\"><b class=\"small\" style=\"COLOR: #ffffff\">&nbsp;" + Common.AppConfig("AffiliateProgramName") + " Member Sign-In</b></td>\n");
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
			writer.Write("                              <td vAlign=\"top\" width=\"100%\" bgColor=\"#CCCCCC\">\n");
			writer.Write("                                <p><center><a href=\"lat_signin.aspx\"><img src=\"skins/skin_" + _siteID.ToString() + "/images/login.gif\" border=\"0\" width=\"76\" height=\"22\"></a></center></td>\n");
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
			writer.Write("                              <td vAlign=\"top\" align=\"left\" width=\"100%\" bgColor=\"#ffffff\">•\n");
			writer.Write("                                <a href=\"lat_signin.aspx\">Forgot your password?</a><br>\n");
			writer.Write("                                • <a href=\"lat_account.aspx\">Your Account Page</a><br>\n");
			writer.Write("                                • <a href=\"t-affiliate_faq.aspx\">FAQs</a><br>\n");
			writer.Write("                                • <a href=\"mailto:" + Common.AppConfig("AffiliateEMailAddress") + "\">Customer\n");
			writer.Write("                                Service</a></td>\n");
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

			int AffiliateID = Common.CookieUSInt("LATAffiliateID");

			if(AffiliateID != 0)
			{
				writer.Write("<p>You are already signed in...please <a href=\"lat_signout.aspx\">sign out</a> here</p>\n");
			}
			else
			{

				Topic t = new Topic("AffiliateTeaser",thisCustomer._localeSetting,_siteID);
				writer.Write(t._contents);

				Address BillingAddress = new Address();
				BillingAddress.LoadByCustomer(thisCustomer._customerID,AddressTypes.Billing);
				
				writer.Write("        <p><b class=\"h1\">Sign Up Below</b><br>\n");
				writer.Write("        <span class=\"serif\">Please complete the sign-up form below. If you have a web site that you will be using to link to us, please complete the additional fields. If you do not have a\n");
				writer.Write("        web site, just ignore the additional fields.<br><br></span>\n");
				
				// SIGN UP FORM:
				String FieldPrefix = String.Empty;
				String act = "lat_signup_process.aspx";
				writer.Write("<form method=\"POST\" action=\"" + act + "\" name=\"LATSignupForm\" id=\"LATSignupForm\" onSubmit=\"return (validateForm(this) && LATSignupForm_Validator(this))\">");


				// ACCOUNT BOX:
				writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
				writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
				writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/accountinfo.gif\" border=\"0\"><br>");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
				writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

				writer.Write("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"2\" width=\"100%\">");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"100%\" colspan=\"2\"><b>Your account information will be used to login to your " + Common.AppConfig("AffiliateProgramName") + " account page later, so please save your password in a safe place.</b></td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"100%\" colspan=\"2\">");
				writer.Write("          <hr>");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">*Your First Name:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"FirstName\" size=\"20\" maxlength=\"50\" value=\"" + Server.HtmlEncode(BillingAddress.FirstName) + "\">");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">*Your Last Name:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"LastName\" size=\"20\" maxlength=\"50\" value=\"" + Server.HtmlEncode(BillingAddress.LastName) + "\">");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				
				String EMailField = Common.IIF(!ThisCustomer.IsAnon , ThisCustomer.EMail , String.Empty);
				if(EMailField.Length > 0 && EMailField.ToLower().IndexOf("anon_") == 1)
				{
					EMailField = String.Empty;
				}
				
				writer.Write("        <td width=\"25%\">*Your E-Mail:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"EMail\" size=\"37\" maxlength=\"100\" value=\"" + Server.HtmlEncode(EMailField) + "\">");
				writer.Write("        <input type=\"hidden\" name=\"EMail_vldt\" value=\"[req][blankalert=Please enter your e-mail address][invalidalert=Please enter a valid e-mail address]\">");
				writer.Write("        </td>");
				writer.Write("      </tr>");

				String PWD = ThisCustomer._password;
				if(PWD == "N/A")
				{
					PWD = String.Empty;
				}

				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">*Create a " + Common.AppConfig("AffiliateProgramName") + " Password:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"password\" name=\"Password\" size=\"37\" maxlength=\"100\" value=\"" + PWD + "\"> (at least 4 chars long)");
				writer.Write("        <input type=\"hidden\" name=\"Password_vldt\" value=\"[req][blankalert=Please enter a password so you can login to this site at a later time]\">\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");

				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">*Repeat Password:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"password\" name=\"Password2\" size=\"37\" maxlength=\"100\" value=\"" + PWD + "\">");
				writer.Write("        <input type=\"hidden\" name=\"Password2_vldt\" value=\"[req][blankalert=Please re-enter a password again to verify]\">\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");
		
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">Company:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"Company\" size=\"34\" maxlength=\"100\" value=\"\">");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">*Address1:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"Address1\" size=\"34\" maxlength=\"100\" value=\"" + Server.HtmlEncode(BillingAddress.Address1) + "\">");
				writer.Write("        <input type=\"hidden\" name=\"Address1_vldt\" value=\"[req][blankalert=Please enter a  address]\">\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">Address2:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"Address2\" size=\"34\" maxlength=\"100\" value=\"" + Server.HtmlEncode(BillingAddress.Address2) + "\">");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">Suite:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"Suite\" size=\"34\" maxlength=\"50\" value=\"" + Server.HtmlEncode(BillingAddress.Suite) + "\">");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">*City:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"City\" size=\"34\" maxlength=\"50\" value=\"" + Server.HtmlEncode(BillingAddress.City) + "\">");
				writer.Write("        <input type=\"hidden\" name=\"City_vldt\" value=\"[req][blankalert=Please enter a  city]\">\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">*State/Province:</td>");
				writer.Write("        <td width=\"75%\">");

				writer.Write("<select size=\"1\" name=\"State\">");
				writer.Write("<OPTION value=\"\"" + Common.IIF(BillingAddress.State.Length==0 , " selected" , String.Empty) + ">SELECT ONE</option>");
				DataSet dsstate = DB.GetDS("select * from state  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddHours(1));
				foreach(DataRow row in dsstate.Tables[0].Rows)
				{
					writer.Write("<OPTION value=\"" + DB.RowField(row,"Abbreviation") + "\"" + Common.IIF(BillingAddress.State == DB.RowField(row,"Abbreviation")," selected ","") + ">" + DB.RowField(row,"Name") + "</option>");
				}
				dsstate.Dispose();
				writer.Write("</select>");

				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">*Zip:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"Zip\" size=\"14\" maxlength=\"10\" value=\"" + Server.HtmlEncode(BillingAddress.Zip) + "\">");
				writer.Write("        <input type=\"hidden\" name=\"Zip_vldt\" value=\"[blankalert=Please enter the  zipcode][invalidalert=Please enter a valid zipcode]\">");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">*Country:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("<SELECT NAME=\"Country\" size=\"1\">");
				DataSet dscountry = DB.GetDS("select * from country  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddHours(1));
				writer.Write("<OPTION value=\"\"" + Common.IIF(BillingAddress.Country.Length==0 , " selected" , String.Empty) + ">SELECT ONE</option>");
				foreach(DataRow row in dscountry.Tables[0].Rows)
				{
					writer.Write("<OPTION value=\"" + DB.RowField(row,"Name") + "\"" + Common.IIF(BillingAddress.Country == DB.RowField(row,"Name")," selected ","") + ">" + DB.RowField(row,"Name") + "</option>");
				}
				dscountry.Dispose();
				writer.Write("</SELECT>");
				writer.Write("        </td>");
				writer.Write("      </tr>");

				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">*Phone:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"Phone\" size=\"14\" maxlength=\"20\" value=\"" + Server.HtmlEncode(BillingAddress.Phone) + "\">");
				writer.Write("        <input type=\"hidden\" name=\"Phone_vldt\" value=\"[req][blankalert=Please enter your phone number][invalidalert=Please enter a valid phone number]\">");
				writer.Write("        </td>");
				writer.Write("      </tr>");

//				writer.Write("      <tr>");
//				writer.Write("        <td width=\"25%\">Date Of Birth:</td>");
//				writer.Write("        <td width=\"75%\">");
//				writer.Write("        <input type=\"text\" name=\"DateOfBirth\" size=\"14\" maxlength=\"20\" value=\"\"> (in format " + Localization.ShortDateFormat() + ")");
//				writer.Write("        <input type=\"hidden\" name=\"DateOfBirth_vldt\" value=\"[date][blankalert=Please enter your date of birth][invalidalert=Please enter a valid date of birth in the format " + Localization.ShortDateFormat() + "]\">");
//				writer.Write("        </td>");
//				writer.Write("      </tr>");
//
//				writer.Write("      <tr>");
//				writer.Write("        <td width=\"25%\">Gender:</td>");
//				writer.Write("        <td width=\"75%\">");
//				writer.Write("		M&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"Gender\" value=\"M\" >\n");
//				writer.Write("		F&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"Gender\" value=\"F\" >\n");
//				writer.Write("        </td>");
//				writer.Write("      </tr>");
			
				writer.Write("</table>\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("</table>\n");


				// ONLINE AFFILIATE INFO:
				writer.Write("<br>");
				writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
				writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
				writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/onlineinfo.gif\" border=\"0\"><br>");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
				writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

				writer.Write("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"2\" width=\"100%\">");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"100%\" colspan=\"2\"><b>Online affiliates must also complete the following fields.<br><br>You only need to enter these fields if you will be using a web site to link to us.</b></td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"100%\" colspan=\"2\">");
				writer.Write("          <hr>");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"35%\">Your Web Site Name:</td>");
				writer.Write("        <td width=\"65%\">");
				writer.Write("        <input type=\"text\" name=\"WebSiteName\" size=\"40\" maxlength=\"100\" value=\"\">");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"35%\">Your Web Site Description:</td>");
				writer.Write("        <td width=\"65%\">");
				writer.Write("        <input type=\"text\" name=\"WebSiteDescription\" size=\"40\" maxlength=\"500\" value=\"\">");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"35%\">Your Web Site URL:</td>");
				writer.Write("        <td width=\"65%\">");
				writer.Write("        <input type=\"text\" name=\"URL\" size=\"50\" maxlength=\"100\" value=\"\">");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("</table>\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("</table>\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("</table>\n");

				writer.Write("<div align=\"center\">");
				writer.Write("<br><input type=\"checkbox\" value=\"yes\" name=\"AgreeToTermsAndConditions\">By selecting this box and the &quot;Join&quot; button, I agree to these <a onClick=\"popuptopicwh('" + Common.AppConfig("AffiliateProgramName").Replace("'","") + " Terms & Conditions','affiliate_terms',650,550,'yes')\" href=\"javascript:void(0);\">Terms and Conditions</a><br><br>");
				writer.Write("<input type=\"submit\" value=\"Join\" name=\"Continue\">");
				writer.Write("</div>");

				writer.Write("</form>");

				// END SIGNUP FORM
			}

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
