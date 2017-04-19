// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using System.Text;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for lat_account.
	/// </summary>
	public class lat_account : SkinBase
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			base._disableLeftAndRightCols = true; // page gets two wide
			RequireSecurePage();
			SectionTitle = "<a href=\"lat_account.aspx\">" + Common.AppConfig("AffiliateProgramName") + "</a> - Account Summary Page";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{

			int AffiliateID = Common.CookieUSInt("LATAffiliateID");

			if(AffiliateID == 0 || !Common.IsValidAffiliate(AffiliateID))
			{
				Response.Redirect("lat_signin.aspx?returnurl=" + Server.UrlEncode(Common.GetThisPageName(true) + "?" + Common.ServerVariables("QUERY_STRING")));
			}

			decimal TotalEarnings = Common.AffiliateTotalEarnings(AffiliateID);
			decimal TotalPayouts = Common.AffiliateTotalPayouts(AffiliateID);
			
			String FirstName = String.Empty;
			String LastName = String.Empty;
			String EMail = String.Empty;

			IDataReader rs = DB.GetRS("select * from affiliate  " + DB.GetNoLock() + " where affiliateid=" + AffiliateID.ToString());
			if(rs.Read())
			{
				FirstName = DB.RSField(rs,"FirstName");
				LastName = DB.RSField(rs,"LastName");
				EMail = DB.RSField(rs,"EMail");
			}
			rs.Close();

			writer.Write("<table cellSpacing=\"5\" cellPadding=\"5\" border=\"0\" width=\"100%\">\n");
			writer.Write("  <tbody>\n");
			writer.Write("    <tr>\n");
			writer.Write("      <td vAlign=\"top\" align=\"top\">\n");
			writer.Write("        <table cellSpacing=\"0\" cellPadding=\"0\" width=\"171\" bgColor=\"#AAAAAA\" border=\"0\">\n");
			writer.Write("          <tbody>\n");
			writer.Write("            <tr>\n");
			writer.Write("              <td bgcolor=\"#AAAAAA\"><b class=\"small\" style=\"COLOR: #ffffff\">&nbsp;" + Common.AppConfig("AffiliateProgramName") + " Member Sign-Out</b></td>\n");
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
			writer.Write("                                <p><center><a href=\"lat_signout.aspx\"><img src=\"skins/skin_" + _siteID.ToString() + "/images/logout.gif\" border=\"0\" width=\"76\" height=\"22\"></a></center></td>\n");
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
			writer.Write("                              <td vAlign=\"top\" align=\"left\" width=\"100%\" bgColor=\"#ffffff\">\n");
			writer.Write("                                • <a href=\"lat_account.aspx\">Account Home</a><br>\n");
//			writer.Write("                                • <a href=\"lat_accountstats.aspx\">Activity Stats</a><br>\n");
//			writer.Write("                                • <a href=\"lat_earnings.aspx\">Your Earnings</a><br>\n");
			writer.Write("                                • <a href=\"lat_getlinking.aspx\">Web Linking Instructions</a><br>\n");
			writer.Write("                                • <a href=\"lat_driver.aspx?topic=affiliate_faq\">FAQs</a><br>\n");
			writer.Write("                                • <a href=\"mailto:" + Common.AppConfig("AffiliateEMailAddress") + "\">Ask A Question</a><br>\n");
			writer.Write("                                • <a href=\"lat_driver.aspx?topic=affiliate_terms\">Terms &amp; Conditions</a></td>\n");
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
			
//			writer.Write("        <p>&nbsp;\n");
//			writer.Write("        <table cellSpacing=\"0\" cellPadding=\"0\" width=\"171\" bgColor=\"#AAAAAA\" border=\"0\">\n");
//			writer.Write("          <tbody>\n");
//			writer.Write("            <tr>\n");
//			writer.Write("              <td bgcolor=\"#AAAAAA\" height=\"18\"><b style=\"color: #ffffff\" class=\"small\">&nbsp;Quick Stats</b></td>\n");
//			writer.Write("            </tr>\n");
//			writer.Write("            <tr vAlign=\"center\" align=\"middle\">\n");
//			writer.Write("              <td>\n");
//			writer.Write("                <table cellSpacing=\"0\" cellPadding=\"1\" width=\"171\" bgColor=\"#AAAAAA\" border=\"0\">\n");
//			writer.Write("                  <tbody>\n");
//			writer.Write("                    <tr>\n");
//			writer.Write("                      <td width=\"100%\">\n");
//			writer.Write("                        <table cellSpacing=\"0\" cellPadding=\"4\" width=\"100%\" bgColor=\"#AAAAAA\" border=\"0\">\n");
//			writer.Write("                          <tbody>\n");
//			writer.Write("                            <tr>\n");
//			writer.Write("                              <td vAlign=\"top\" align=\"left\" width=\"100%\" bgColor=\"#ffffff\">\n");
//			writer.Write("                                • Your Affiliate ID: <b>" + AffiliateID.ToString() + "</b><br>\n");
//			writer.Write("                                • Total Earnings: " + Localization.CurrencyStringForDisplay(TotalEarnings) + "<br>\n");
//			writer.Write("                                • Total Payouts : " + Localization.CurrencyStringForDisplay(TotalPayouts) + "<br>\n");
//			writer.Write("                                • Acct. Balance : " + Localization.CurrencyStringForDisplay(TotalEarnings-TotalPayouts) + "<br>\n");
//			writer.Write("								</td>");
//			writer.Write("                            </tr>\n");
//			writer.Write("                          </tbody>\n");
//			writer.Write("                        </table>\n");
//			writer.Write("                      </td>\n");
//			writer.Write("                    </tr>\n");
//			writer.Write("                  </tbody>\n");
//			writer.Write("                </table>\n");
//			writer.Write("              </td>\n");
//			writer.Write("            </tr>\n");
//			writer.Write("          </tbody>\n");
//			writer.Write("        </table>\n");
			
			writer.Write("      </td>\n");
			writer.Write("      <td width=\"100%\" align=\"left\" valign=\"top\"><img border=\"0\" src=\"skins/skin_" + _siteID.ToString() + "/images/affiliateheader_small.jpg\" width=\"328\" height=\"70\"><br>\n");
			writer.Write("        <br>\n");

			writer.Write("<p><b>Welcome to the " + Common.AppConfig("AffiliateProgramName") + " Program!</b></p>\n");
			writer.Write("<p><b>You can use this section to track your earnings, build links for your Web site, and edit your account information.</b></p>\n");
			writer.Write("<p><b>To see the " + Common.AppConfig("AffiliateProgramName") + " FAQs, <a href=\"lat_driver.aspx?topic=affiliate_faq\">click here</a>.</b></p>\n");
			writer.Write("<p><b>To update your account information, use the forms below:</b></p>");

			// START ACCOUNT INFO:

			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function LATAccountForm_Validator(theForm)\n");
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

			String FieldPrefix = String.Empty;
			rs = DB.GetRS("select * from affiliate " + DB.GetNoLock() + " where affiliateid=" + AffiliateID.ToString());
			rs.Read();

			if(Common.QueryString("Msg").Length != 0)
			{
				writer.Write("<p><font color=blue><b>" + Server.HtmlEncode(Common.QueryString("Msg")) + "</b></font></p>\n");
			}

			String act = "lat_account_process.aspx";
			writer.Write("<form method=\"POST\" action=\"" + act + "\" name=\"LATAccountForm\" id=\"LATAccountForm\" onSubmit=\"return (validateForm(this) && LATAccountForm_Validator(this))\">");


			// ACCOUNT BOX:
			writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			writer.Write("<img align=\"bottom\" src=\"skins/Skin_" + _siteID.ToString() + "/images/accountinfo.gif\" border=\"0\">");
			writer.Write("<br>");
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
			writer.Write("        <td width=\"25%\">Your First Name:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"FirstName\" size=\"20\" maxlength=\"50\" value=\"" + Server.HtmlEncode(DB.RSField(rs,FieldPrefix + "FirstName")) + "\"> (required)");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Your Last Name:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"LastName\" size=\"20\" maxlength=\"50\" value=\"" + Server.HtmlEncode(DB.RSField(rs,FieldPrefix + "LastName")) + "\"> (required)");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Your E-Mail:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"EMail\" size=\"37\" maxlength=\"100\" value=\"" + Server.HtmlEncode(DB.RSField(rs,FieldPrefix + "EMail")) + "\"> (required)");
			writer.Write("        <input type=\"hidden\" name=\"EMail_vldt\" value=\"[req][email][blankalert=Please enter your e-mail address][invalidalert=Please enter a valid e-mail address]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Create a " + Common.AppConfig("AffiliateProgramName") + " Password:</td>");
			writer.Write("        <td width=\"75%\">");
			String PWD = DB.RSField(rs,"Password");
			if(PWD == "N/A")
			{
				PWD = String.Empty;
			}
			writer.Write("        <input type=\"password\" name=\"Password\" size=\"37\" maxlength=\"100\" value=\"" + Server.HtmlEncode(PWD) + "\"> (required)");
			writer.Write("        <input type=\"hidden\" name=\"Password_vldt\" value=\"[req][blankalert=Please enter a password so you can login to this site at a later time]\">\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Repeat Password:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"password\" name=\"Password2\" size=\"37\" maxlength=\"100\" value=\"" + Server.HtmlEncode(PWD) + "\"> (required)");
			writer.Write("        <input type=\"hidden\" name=\"Password2_vldt\" value=\"[req][blankalert=Please re-enter a password again to verify]\">\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");
	
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Company:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"Company\" size=\"34\" maxlength=\"100\" value=\"" + Server.HtmlEncode(DB.RSField(rs,FieldPrefix + "Company")) + "\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Address1:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"Address1\" size=\"34\" maxlength=\"100\" value=\"" + Server.HtmlEncode(DB.RSField(rs,FieldPrefix + "Address1")) + "\"> (required)");
			writer.Write("        <input type=\"hidden\" name=\"Address1_vldt\" value=\"[req][blankalert=Please enter a  address]\">\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Address2:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"Address2\" size=\"34\" maxlength=\"100\" value=\"" + Server.HtmlEncode(DB.RSField(rs,FieldPrefix + "Address2")) + "\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Suite:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"Suite\" size=\"34\" maxlength=\"50\" value=\"" + Server.HtmlEncode(DB.RSField(rs,FieldPrefix + "Suite")) + "\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">City:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"City\" size=\"34\" maxlength=\"50\" value=\"" + Server.HtmlEncode(DB.RSField(rs,FieldPrefix + "City")) + "\"> (required)");
			writer.Write("        <input type=\"hidden\" name=\"City_vldt\" value=\"[req][blankalert=Please enter a  city]\">\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">State/Province:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("<select size=\"1\" name=\"State\">");
			writer.Write("<OPTION value=\"\"" + Common.IIF(DB.RSField(rs,FieldPrefix + "state").Length==0 , " selected" , String.Empty) + ">SELECT ONE</option>");
			DataSet dsstate = DB.GetDS("select * from state  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddHours(1));
			foreach(DataRow row in dsstate.Tables[0].Rows)
			{
				writer.Write("<OPTION value=\"" + DB.RowField(row,"Abbreviation") + "\"" + Common.SelectOption(rs,DB.RowField(row,"Abbreviation"),FieldPrefix + "state") + ">" + DB.RowField(row,"Name") + "</option>");
			}
			dsstate.Dispose();
			writer.Write("</select> (required)");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Zip:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"Zip\" size=\"14\" maxlength=\"10\" value=\"" + Server.HtmlEncode(DB.RSField(rs,FieldPrefix + "Zip")) + "\"> (required)");
			writer.Write("        <input type=\"hidden\" name=\"Zip_vldt\" value=\"[blankalert=Please enter the  zipcode][invalidalert=Please enter a valid zipcode]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Country:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("<SELECT NAME=\"Country\" size=\"1\">");
			DataSet dscountry = DB.GetDS("select * from country  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddHours(1));
			foreach(DataRow row in dscountry.Tables[0].Rows)
			{
				writer.Write("<OPTION value=\"" + DB.RowField(row,"Name") + "\"" + Common.SelectOption(rs,DB.RowField(row,"Name"),FieldPrefix+"Country") + ">" + DB.RowField(row,"Name") + "</option>");
			}
			dscountry.Dispose();
			writer.Write("</SELECT>");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Phone:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"Phone\" size=\"14\" maxlength=\"20\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"Phone")) + "\"> (required)");
			writer.Write("        <input type=\"hidden\" name=\"Phone_vldt\" value=\"[req][blankalert=Please enter your phone number][invalidalert=Please enter a valid phone number]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Date Of Birth:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"DateOfBirth\" size=\"14\" maxlength=\"20\" value=\"" + Common.IIF(DB.RSFieldDateTime(rs,"DateOfBirth") != System.DateTime.MinValue , Localization.ToNativeShortDateString(DB.RSFieldDateTime(rs,"DateOfBirth")) , "") + "\"> (in format " + Localization.ShortDateFormat() + ")");
			writer.Write("        <input type=\"hidden\" name=\"DateOfBirth_vldt\" value=\"[date][blankalert=Please enter your date of birth][invalidalert=Please enter a valid date of birth in the format " + Localization.ShortDateFormat() + "]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Gender:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("		M&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"Gender\" value=\"M\" " + Common.IIF(DB.RSField(rs,"Gender")=="M" , " checked " , "") + ">\n");
			writer.Write("		F&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"Gender\" value=\"F\" " + Common.IIF(DB.RSField(rs,"Gender")=="F" , " checked " , "") + ">\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");
		
			writer.Write("</table>\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("</table>\n");

			writer.Write("<br>");
			writer.Write("<div align=\"center\"><input type=\"submit\" value=\"Update Your Account Info\" name=\"Submit\"></div>");
			writer.Write("<br>");

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
			writer.Write("        <input type=\"text\" name=\"WebSiteName\" size=\"40\" maxlength=\"100\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"WebSiteName")) + "\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"35%\">Your Web Site Description:</td>");
			writer.Write("        <td width=\"65%\">");
			writer.Write("        <input type=\"text\" name=\"WebSiteDescription\" size=\"40\" maxlength=\"500\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"WebSiteDescription")) + "\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"35%\">Your Web Site URL:</td>");
			writer.Write("        <td width=\"65%\">");
			writer.Write("        <input type=\"text\" name=\"URL\" size=\"50\" maxlength=\"100\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"URL")) + "\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("</table>\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("</table>\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("</table>\n");

			writer.Write("<div align=\"center\"><input type=\"submit\" value=\"Update Your Account Info\" name=\"Submit\"></div>");

			// PAYEE/TAX INFO:
			writer.Write("<br>");
			writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/taxinfo.gif\" border=\"0\"><br>");
			writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

			writer.Write("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"2\" width=\"100%\">");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"100%\" colspan=\"2\"><b>In order for us to pay out your accrued funds, the following information must be supplied. This information is kept strictly confidential, and only used for tax reporting purposes where required by law. This information is never given to any third party.</b></td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"100%\" colspan=\"2\">");
			writer.Write("          <hr>");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"35%\">Tax Registration Name:</td>");
			writer.Write("        <td width=\"65%\">");
			writer.Write("        <input type=\"text\" name=\"TaxRegistrationName\" size=\"40\" maxlength=\"100\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"TaxRegistrationName")) + "\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"35%\">Tax ID Number:</td>");
			writer.Write("        <td width=\"65%\">");
			String TaxIDNumber = Common.IIF(DB.RSField(rs,"TaxIDNumber").Length == 0 , "" , Common.UnmungeString(DB.RSField(rs,"TaxIDNumber")));
			writer.Write("        <input type=\"text\" name=\"TaxIDNumber\" size=\"40\" maxlength=\"500\" value=\"" + Server.HtmlEncode(TaxIDNumber) + "\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"35%\">Tax Classification:</td>");
			writer.Write("        <td width=\"65%\">");
			writer.Write("        <input type=\"text\" name=\"TaxClassification\" size=\"50\" maxlength=\"100\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"TaxClassification")) + "\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("</table>\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("</table>\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("</table>\n");

			writer.Write("<div align=\"center\"><input type=\"submit\" value=\"Update Your Account Info\" name=\"Submit\"></div>");

			writer.Write("</form>");
			rs.Close();

			// END ACCOUNT INFO

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
