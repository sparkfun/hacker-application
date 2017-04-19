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
using System.Data.SqlTypes;
using System.Data.SqlClient;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for lat_signup.
	/// </summary>
	public class lat_signup : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "<a href=\"affiliates.aspx\">Affiliates</a> - Add new Affiliate";
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
			writer.Write("  return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");
			
			writer.Write("<table cellSpacing=\"5\" cellPadding=\"5\" border=\"0\" width=\"100%\">\n");
			writer.Write("  <tbody>\n");
			writer.Write("    <tr>\n");
			writer.Write("      <td vAlign=\"top\" align=\"top\">\n");
			writer.Write("      </td>\n");
			writer.Write("      <td width=\"100%\" align=\"left\" valign=\"top\">\n");
			writer.Write("        <br>\n");

			Topic t = new Topic("AffiliateTeaser",thisCustomer._localeSetting,_siteID);
			writer.Write(t._contents);

			writer.Write("        <p><b class=\"h1\">Sign Up Below</b><br>\n");
			writer.Write("        <span class=\"serif\">Please complete the sign-up form below. If you have\n");
			writer.Write("        a web site that you will be using to link to us, please complete the additional fields. If you do not have a\n");
			writer.Write("        web site, just ignore the additional fields.<br><br></span>\n");
			
			// SIGN UP FORM:
			String FieldPrefix = String.Empty;
			IDataReader rs = DB.GetRS("select * from affiliate where affiliateid=-1"); // produce no match :)
			rs.Read();

			String act = "lat_signup_process.aspx";
			writer.Write("<form method=\"POST\" action=\"" + act + "\" name=\"LATSignupForm\" id=\"LATSignupForm\" onSubmit=\"return (validateForm(this) && LATSignupForm_Validator(this))\">");


			// ACCOUNT BOX:
			writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			writer.Write("<img src=\"../skins/Skin_" + _siteID.ToString() + "/images/accountinfo.gif\" border=\"0\"><br>");
			writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

			writer.Write("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"2\" width=\"100%\">");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"100%\" colspan=\"2\"><b>Your account information will be used to login to your Rewards Program account page later, so please save your password in a safe place.</b></td>");
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
			String EMailField = Common.IIF(!thisCustomer._isAnon , DB.RSField(rs,"EMail") , String.Empty);
			if(EMailField.Length > 0 && EMailField.ToLower().IndexOf("anon_") == 1)
			{
				EMailField = String.Empty;
			}
			writer.Write("        <input type=\"text\" name=\"EMail\" size=\"37\" maxlength=\"100\" value=\"" + Server.HtmlEncode(EMailField) + "\"> (required)");
			writer.Write("        <input type=\"hidden\" name=\"EMail_vldt\" value=\"[req][email][blankalert=Please enter your e-mail address][invalidalert=Please enter a valid e-mail address]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Create a Rewards Program Password:</td>");
			writer.Write("        <td width=\"75%\">");
			String PWD = DB.RSField(rs,"Password");
			if(PWD == "N/A")
			{
				PWD = String.Empty;
			}
			writer.Write("        <input type=\"text\" name=\"Password\" size=\"37\" maxlength=\"100\" value=\"" + PWD + "\"> (required, a least 4 chars long)");
			writer.Write("        <input type=\"hidden\" name=\"Password_vldt\" value=\"[req][blankalert=Please enter a password so you can login to this site at a later time]\">\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Repeat Password:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"Password2\" size=\"37\" maxlength=\"100\" value=\"" + PWD + "\"> (required)");
			writer.Write("        <input type=\"hidden\" name=\"Password2_vldt\" value=\"[req][blankalert=Please re-enter a password again to verify]\">\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");
	
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Default Skin ID:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"DefaultSkinID\" size=\"3\" maxlength=\"3\" value=\"1\"> (required)");
			writer.Write("        <input type=\"hidden\" name=\"DefaultSkinID_vldt\" value=\"[number][req][blankalert=Please enter the default skin ID for this affiliate][invalidalert=Please enter a number, e.g. 1]\">\n");
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
			writer.Write("        <select size=\"1\" name=\"State\">");
			writer.Write("						<OPTION value=\"\"" + Common.IIF(DB.RSField(rs,FieldPrefix + "state").Length == 0 , " selected" , String.Empty) + ">SELECT ONE</option>");
			writer.Write("   					  <OPTION value=\"--\"" + Common.SelectOption(rs,"--",FieldPrefix + "state") + ">Non US/Canada</option>");
			writer.Write("                      <OPTION value=\"AL\"" + Common.SelectOption(rs,"AL",FieldPrefix + "state") + ">AL</option>");
			writer.Write("                      <OPTION value=\"AK\"" + Common.SelectOption(rs,"AK",FieldPrefix + "state") + ">AK</option>");
			writer.Write("                      <OPTION value=\"AB\"" + Common.SelectOption(rs,"AB",FieldPrefix + "state") + ">AB</option>");
			writer.Write("                      <OPTION value=\"AS\"" + Common.SelectOption(rs,"AS",FieldPrefix + "state") + ">AS</option>");
			writer.Write("                      <OPTION value=\"AZ\"" + Common.SelectOption(rs,"AZ",FieldPrefix + "state") + ">AZ</option>");
			writer.Write("                      <OPTION value=\"AR\"" + Common.SelectOption(rs,"AR",FieldPrefix + "state") + ">AR</option>");
			writer.Write("                      <OPTION value=\"BC\"" + Common.SelectOption(rs,"BC",FieldPrefix + "state") + ">BC</option>");
			writer.Write("                      <OPTION value=\"CA\"" + Common.SelectOption(rs,"CA",FieldPrefix + "state") + ">CA</option>");
			writer.Write("                      <OPTION value=\"CO\"" + Common.SelectOption(rs,"CO",FieldPrefix + "state") + ">CO</option>");
			writer.Write("                      <OPTION value=\"CT\"" + Common.SelectOption(rs,"CT",FieldPrefix + "state") + ">CT</option>");
			writer.Write("                      <OPTION value=\"DE\"" + Common.SelectOption(rs,"DE",FieldPrefix + "state") + ">DE</option>");
			writer.Write("                      <OPTION value=\"DC\"" + Common.SelectOption(rs,"DC",FieldPrefix + "state") + ">DC</option>");
			writer.Write("                      <OPTION value=\"FM\"" + Common.SelectOption(rs,"FM",FieldPrefix + "state") + ">FM</option>");
			writer.Write("                      <OPTION value=\"FL\"" + Common.SelectOption(rs,"FL",FieldPrefix + "state") + ">FL</option>");
			writer.Write("                      <OPTION value=\"GA\"" + Common.SelectOption(rs,"GA",FieldPrefix + "state") + ">GA</option>");
			writer.Write("                      <OPTION value=\"GU\"" + Common.SelectOption(rs,"GU",FieldPrefix + "state") + ">GU</option>");
			writer.Write("                      <OPTION value=\"HI\"" + Common.SelectOption(rs,"HI",FieldPrefix + "state") + ">HI</option>");
			writer.Write("                      <OPTION value=\"ID\"" + Common.SelectOption(rs,"ID",FieldPrefix + "state") + ">ID</option>");
			writer.Write("                      <OPTION value=\"IL\"" + Common.SelectOption(rs,"IL",FieldPrefix + "state") + ">IL</option>");
			writer.Write("                      <OPTION value=\"IN\"" + Common.SelectOption(rs,"IN",FieldPrefix + "state") + ">IN</option>");
			writer.Write("                      <OPTION value=\"IA\"" + Common.SelectOption(rs,"IA",FieldPrefix + "state") + ">IA</option>");
			writer.Write("                      <OPTION value=\"KS\"" + Common.SelectOption(rs,"KS",FieldPrefix + "state") + ">KS</option>");
			writer.Write("                      <OPTION value=\"KY\"" + Common.SelectOption(rs,"KY",FieldPrefix + "state") + ">KY</option>");
			writer.Write("                      <OPTION value=\"LA\"" + Common.SelectOption(rs,"LA",FieldPrefix + "state") + ">LA</option>");
			writer.Write("                      <OPTION value=\"ME\"" + Common.SelectOption(rs,"ME",FieldPrefix + "state") + ">ME</option>");
			writer.Write("                      <OPTION value=\"MB\"" + Common.SelectOption(rs,"MB",FieldPrefix + "state") + ">MB</option>");
			writer.Write("                      <OPTION value=\"MH\"" + Common.SelectOption(rs,"MH",FieldPrefix + "state") + ">MH</option>");
			writer.Write("                      <OPTION value=\"MD\"" + Common.SelectOption(rs,"MD",FieldPrefix + "state") + ">MD</option>");
			writer.Write("                      <OPTION value=\"MA\"" + Common.SelectOption(rs,"MA",FieldPrefix + "state") + ">MA</option>");
			writer.Write("                      <OPTION value=\"MI\"" + Common.SelectOption(rs,"MI",FieldPrefix + "state") + ">MI</option>");
			writer.Write("                      <OPTION value=\"MN\"" + Common.SelectOption(rs,"MN",FieldPrefix + "state") + ">MN</option>");
			writer.Write("                      <OPTION value=\"MS\"" + Common.SelectOption(rs,"MS",FieldPrefix + "state") + ">MS</option>");
			writer.Write("                      <OPTION value=\"MO\"" + Common.SelectOption(rs,"MO",FieldPrefix + "state") + ">MO</option>");
			writer.Write("                      <OPTION value=\"MT\"" + Common.SelectOption(rs,"MT",FieldPrefix + "state") + ">MT</option>");
			writer.Write("                      <OPTION value=\"NE\"" + Common.SelectOption(rs,"NE",FieldPrefix + "state") + ">NE</option>");
			writer.Write("                      <OPTION value=\"NV\"" + Common.SelectOption(rs,"NV",FieldPrefix + "state") + ">NV</option>");
			writer.Write("                      <OPTION value=\"NB\"" + Common.SelectOption(rs,"NB",FieldPrefix + "state") + ">NB</option>");
			writer.Write("                      <OPTION value=\"NF\"" + Common.SelectOption(rs,"NF",FieldPrefix + "state") + ">NF</option>");
			writer.Write("                      <OPTION value=\"NH\"" + Common.SelectOption(rs,"NH",FieldPrefix + "state") + ">NH</option>");
			writer.Write("                      <OPTION value=\"NJ\"" + Common.SelectOption(rs,"NJ",FieldPrefix + "state") + ">NJ</option>");
			writer.Write("                      <OPTION value=\"NM\"" + Common.SelectOption(rs,"NM",FieldPrefix + "state") + ">NM</option>");
			writer.Write("                      <OPTION value=\"NY\"" + Common.SelectOption(rs,"NY",FieldPrefix + "state") + ">NY</option>");
			writer.Write("                      <OPTION value=\"NC\"" + Common.SelectOption(rs,"NC",FieldPrefix + "state") + ">NC</option>");
			writer.Write("                      <OPTION value=\"ND\"" + Common.SelectOption(rs,"ND",FieldPrefix + "state") + ">ND</option>");
			writer.Write("                      <OPTION value=\"MP\"" + Common.SelectOption(rs,"MP",FieldPrefix + "state") + ">MP</option>");
			writer.Write("                      <OPTION value=\"NT\"" + Common.SelectOption(rs,"NT",FieldPrefix + "state") + ">NT</option>");
			writer.Write("                      <OPTION value=\"NS\"" + Common.SelectOption(rs,"NS",FieldPrefix + "state") + ">NS</option>");
			writer.Write("                      <OPTION value=\"OH\"" + Common.SelectOption(rs,"OH",FieldPrefix + "state") + ">OH</option>");
			writer.Write("                      <OPTION value=\"OK\"" + Common.SelectOption(rs,"OK",FieldPrefix + "state") + ">OK</option>");
			writer.Write("                      <OPTION value=\"ON\"" + Common.SelectOption(rs,"ON",FieldPrefix + "state") + ">ON</option>");
			writer.Write("                      <OPTION value=\"OR\"" + Common.SelectOption(rs,"OR",FieldPrefix + "state") + ">OR</option>");
			writer.Write("                      <OPTION value=\"PW\"" + Common.SelectOption(rs,"PW",FieldPrefix + "state") + ">PW</option>");
			writer.Write("                      <OPTION value=\"PA\"" + Common.SelectOption(rs,"PA",FieldPrefix + "state") + ">PA</option>");
			writer.Write("                      <OPTION value=\"PE\"" + Common.SelectOption(rs,"PE",FieldPrefix + "state") + ">PE</option>");
			writer.Write("                      <OPTION value=\"QC\"" + Common.SelectOption(rs,"QC",FieldPrefix + "state") + ">QC</option>");
			writer.Write("                      <OPTION value=\"RI\"" + Common.SelectOption(rs,"RI",FieldPrefix + "state") + ">RI</option>");
			writer.Write("                      <OPTION value=\"SK\"" + Common.SelectOption(rs,"SK",FieldPrefix + "state") + ">SK</option>");
			writer.Write("                      <OPTION value=\"SC\"" + Common.SelectOption(rs,"SC",FieldPrefix + "state") + ">SC</option>");
			writer.Write("                      <OPTION value=\"SD\"" + Common.SelectOption(rs,"SD",FieldPrefix + "state") + ">SD</option>");
			writer.Write("                      <OPTION value=\"TN\"" + Common.SelectOption(rs,"TN",FieldPrefix + "state") + ">TN</option>");
			writer.Write("                      <OPTION value=\"TX\"" + Common.SelectOption(rs,"TX",FieldPrefix + "state") + ">TX</option>");
			writer.Write("                      <OPTION value=\"UT\"" + Common.SelectOption(rs,"UT",FieldPrefix + "state") + ">UT</option>");
			writer.Write("                      <OPTION value=\"VT\"" + Common.SelectOption(rs,"VT",FieldPrefix + "state") + ">VT</option>");
			writer.Write("                      <OPTION value=\"VI\"" + Common.SelectOption(rs,"VI",FieldPrefix + "state") + ">VI</option>");
			writer.Write("                      <OPTION value=\"VA\"" + Common.SelectOption(rs,"VA",FieldPrefix + "state") + ">VA</option>");
			writer.Write("                      <OPTION value=\"WA\"" + Common.SelectOption(rs,"WA",FieldPrefix + "state") + ">WA</option>");
			writer.Write("                      <OPTION value=\"WV\"" + Common.SelectOption(rs,"WV",FieldPrefix + "state") + ">WV</option>");
			writer.Write("                      <OPTION value=\"WI\"" + Common.SelectOption(rs,"WI",FieldPrefix + "state") + ">WI</option>");
			writer.Write("                      <OPTION value=\"WY\"" + Common.SelectOption(rs,"WY",FieldPrefix + "state") + ">WY</option>");
			writer.Write("                      <OPTION value=\"YT\"" + Common.SelectOption(rs,"YT",FieldPrefix + "state") + ">YT</option>");
			writer.Write("                      </select> (required)");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Zip:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"Zip\" size=\"14\" maxlength=\"10\" value=\"" + DB.RSField(rs,FieldPrefix + "Zip") + "\"> (required)");
			writer.Write("        <input type=\"hidden\" name=\"Zip_vldt\" value=\"[req][blankalert=Please enter the  zipcode][invalidalert=Please enter a valid zipcode]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Country:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("<SELECT NAME=\"Country\" size=\"1\">");
			writer.Write("<OPTION value=\"US\"" + Common.SelectOption(rs,"US",FieldPrefix + "Country") + ">United States</option>");
			writer.Write("<OPTION value=\"AF\"" + Common.SelectOption(rs,"AF",FieldPrefix + "Country") + ">Afghanistan</option>");
			writer.Write("<OPTION value=\"AU\"" + Common.SelectOption(rs,"AR",FieldPrefix + "Country") + ">Argentina</option>");
			writer.Write("<OPTION value=\"AU\"" + Common.SelectOption(rs,"AU",FieldPrefix + "Country") + ">Australia</option>");
			writer.Write("<OPTION value=\"AT\"" + Common.SelectOption(rs,"AT",FieldPrefix + "Country") + ">Austria</option>");
			writer.Write("<OPTION value=\"BE\"" + Common.SelectOption(rs,"BE",FieldPrefix + "Country") + ">Belgium</option>");
			writer.Write("<OPTION value=\"BR\"" + Common.SelectOption(rs,"BR",FieldPrefix + "Country") + ">Brazil</option>");
			writer.Write("<OPTION value=\"CA\"" + Common.SelectOption(rs,"CA",FieldPrefix + "Country") + ">Canada</option>");
			writer.Write("<OPTION value=\"CA\"" + Common.SelectOption(rs,"CL",FieldPrefix + "Country") + ">Chile</option>");
			writer.Write("<OPTION value=\"CA\"" + Common.SelectOption(rs,"CN",FieldPrefix + "Country") + ">China</option>");
			writer.Write("<OPTION value=\"DK\"" + Common.SelectOption(rs,"DK",FieldPrefix + "Country") + ">Denmark</option>");
			writer.Write("<OPTION value=\"FR\"" + Common.SelectOption(rs,"FR",FieldPrefix + "Country") + ">France</option>");
			writer.Write("<OPTION value=\"DE\"" + Common.SelectOption(rs,"DE",FieldPrefix + "Country") + ">Germany</option>");
			writer.Write("<OPTION value=\"GR\"" + Common.SelectOption(rs,"GR",FieldPrefix + "Country") + ">Greece</option>");
			writer.Write("<OPTION value=\"HK\"" + Common.SelectOption(rs,"HK",FieldPrefix + "Country") + ">Hong Kong</option>");
			writer.Write("<OPTION value=\"IN\"" + Common.SelectOption(rs,"IN",FieldPrefix + "Country") + ">India</option>");
			writer.Write("<OPTION value=\"ID\"" + Common.SelectOption(rs,"ID",FieldPrefix + "Country") + ">Indonesia</option>");
			writer.Write("<OPTION value=\"IE\"" + Common.SelectOption(rs,"IE",FieldPrefix + "Country") + ">Ireland</option>");
			writer.Write("<OPTION value=\"IT\"" + Common.SelectOption(rs,"IT",FieldPrefix + "Country") + ">Italy</option>");
			writer.Write("<OPTION value=\"JP\"" + Common.SelectOption(rs,"JP",FieldPrefix + "Country") + ">Japan</option>");
			writer.Write("<OPTION value=\"KR\"" + Common.SelectOption(rs,"KR",FieldPrefix + "Country") + ">Korea, Republic Of</option>");
			writer.Write("<OPTION value=\"MX\"" + Common.SelectOption(rs,"MX",FieldPrefix + "Country") + ">Mexico</option>");
			writer.Write("<OPTION value=\"NL\"" + Common.SelectOption(rs,"NL",FieldPrefix + "Country") + ">Netherlands</option>");
			writer.Write("<OPTION value=\"NZ\"" + Common.SelectOption(rs,"NZ",FieldPrefix + "Country") + ">New Zealand</option>");
			writer.Write("<OPTION value=\"NO\"" + Common.SelectOption(rs,"NO",FieldPrefix + "Country") + ">Norway</option>");
			writer.Write("<OPTION value=\"PT\"" + Common.SelectOption(rs,"PT",FieldPrefix + "Country") + ">Portugal</option>");
			writer.Write("<OPTION value=\"PR\"" + Common.SelectOption(rs,"PR",FieldPrefix + "Country") + ">Puerto Rico</option>");
			writer.Write("<OPTION value=\"SA\"" + Common.SelectOption(rs,"SA",FieldPrefix + "Country") + ">Saudi Arabia</option>");
			writer.Write("<OPTION value=\"SG\"" + Common.SelectOption(rs,"SG",FieldPrefix + "Country") + ">Singapore</option>");
			writer.Write("<OPTION value=\"ZA\"" + Common.SelectOption(rs,"ZA",FieldPrefix + "Country") + ">South Africa</option>");
			writer.Write("<OPTION value=\"ES\"" + Common.SelectOption(rs,"ES",FieldPrefix + "Country") + ">Spain</option>");
			writer.Write("<OPTION value=\"SE\"" + Common.SelectOption(rs,"SE",FieldPrefix + "Country") + ">Sweden</option>");
			writer.Write("<OPTION value=\"CH\"" + Common.SelectOption(rs,"CH",FieldPrefix + "Country") + ">Switzerland</option>");
			writer.Write("<OPTION value=\"TW\"" + Common.SelectOption(rs,"TW",FieldPrefix + "Country") + ">Taiwan</option>");
			writer.Write("<OPTION value=\"TH\"" + Common.SelectOption(rs,"TH",FieldPrefix + "Country") + ">Thailand</option>");
			writer.Write("<OPTION value=\"TR\"" + Common.SelectOption(rs,"TR",FieldPrefix + "Country") + ">Turkey</option>");
			writer.Write("<OPTION value=\"AE\"" + Common.SelectOption(rs,"AE",FieldPrefix + "Country") + ">United Arab Emirates</option>");
			writer.Write("<OPTION value=\"GB\"" + Common.SelectOption(rs,"GB",FieldPrefix + "Country") + ">United Kingdom</option>");
			writer.Write("<OPTION value=\"VE\"" + Common.SelectOption(rs,"VE",FieldPrefix + "Country") + ">Venezuela</option>");
			writer.Write("<OPTION value=\"OTHER\"" + Common.SelectOption(rs,"OTHER",FieldPrefix + "Country") + ">Other Country</option>");
			writer.Write("                    </SELECT> (required)");
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
			writer.Write("		M&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"Gender\" value=\"M\" " + Common.IIF(DB.RSField(rs,"Gender") == "M" , " checked " , "") + ">\n");
			writer.Write("		F&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"Gender\" value=\"F\" " + Common.IIF(DB.RSField(rs,"Gender") == "F" , " checked " , "") + ">\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");
		
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">For Ad Tracking Only:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("		False&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"TrackingOnly\" value=\"0\" " + Common.IIF(DB.RSFieldBool(rs,"TrackingOnly") , "" , " checked ") + ">\n");
			writer.Write("		True&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"TrackingOnly\" value=\"1\" " + Common.IIF(DB.RSFieldBool(rs,"TrackingOnly") , " checked " , "") + ">\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");
		
			writer.Write("</table>\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("</table>\n");


			// ONLINE AFFILIATE INFO:
			writer.Write("<br>");
			writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			writer.Write("<img src=\"../skins/Skin_" + _siteID.ToString() + "/images/onlineinfo.gif\" border=\"0\"><br>");
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

			writer.Write("<div align=\"center\"><input type=\"submit\" value=\"Submit\" name=\"Continue\"></div>");

			writer.Write("</form>");
			rs.Close();

			// END SIGNUP FORM

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
