// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for offlineorder.
	/// </summary>
	public class offlineorder : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Enter Offline Order";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write("  <!-- calendar stylesheet -->\n");
			writer.Write("  <link rel=\"stylesheet\" type=\"text/css\" media=\"all\" href=\"jscalendar/calendar-win2k-cold-1.css\" title=\"win2k-cold-1\" />\n");
			writer.Write("\n");
			writer.Write("  <!-- main calendar program -->\n");
			writer.Write("  <script type=\"text/javascript\" src=\"jscalendar/calendar.js\"></script>\n");
			writer.Write("\n");
			writer.Write("  <!-- language for the calendar -->\n");
			writer.Write("  <script type=\"text/javascript\" src=\"jscalendar/lang/" + Localization.JSCalendarLanguageFile() + "\"></script>\n");
			writer.Write("\n");
			writer.Write("  <!-- the following script defines the Calendar.setup helper function, which makes\n");
			writer.Write("       adding a calendar a matter of 1 or 2 lines of code. -->\n");
			writer.Write("  <script type=\"text/javascript\" src=\"jscalendar/calendar-setup.js\"></script>\n");

			writer.Write("<p align=\"left\"><b><h3>THIS DOES NOT CREATE A CUSTOMER RECORD IN THE CUSTOMER TABLE, ONLY AN ORDER RECORD IS CREATED!<br>ALSO, PAYMENT IS SET TO CLEARED, AND SHIPPING AND DOWNLOAD RECEIPTS ARE SET TO \"ALREADY SENT\" STATUS.<br>A RECEIPT <font color=blue>WILL</font> BE E-MAILED IF THEIR E-MAIL ADDRESS IS ENTERED.</h3></p>\n");
			writer.Write("<form method=\"GET\" action=\"offlineorder.aspx\" name=\"LookupForm\" id=\"LookupForm\" onSubmit=\"return validateForm(this)\">");
			writer.Write("<table border=\"1\" cellpadding=\"2\" cellspacing=\"0\" width=\"100%\">\n");
			writer.Write("<tr>\n");
			writer.Write("<td align=\"left\" width=\"100%\"><b>Lookup Existing Customer Info</b>:<br><br>\n");
			writer.Write("First Name: <input type=\"text\" name=\"LookupFirstName\" value=\"" + Common.QueryString("LookupFirstName") + "\" size=\"20\"> Last Name: <input type=\"text\" name=\"LookupLastName\" value=\"" + Common.QueryString("LookupLastName") + "\" size=\"20\"> or E-Mail: <input type=\"text\" name=\"LookupEMail\" value=\"" + Common.QueryString("LookupEMail") + "\" size=\"20\"> <input type=\"submit\" value=\"Lookup\" name=\"B1\">\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("</form>\n");

			String WhereClause = String.Empty;
			if(Common.QueryString("LookupFirstName").Length != 0)
			{
				if(WhereClause.Length != 0)
				{
					WhereClause += " and ";
				}
				WhereClause += " lower(firstname)=" + DB.SQuote(Common.QueryString("LookupFirstName").ToLower());
			}
			if(Common.QueryString("LookupLastName").Length != 0)
			{
				if(WhereClause.Length != 0)
				{
					WhereClause += " and ";
				}
				WhereClause += " lower(lastname)=" + DB.SQuote(Common.QueryString("LookupLastName").ToLower());
			}
			if(Common.QueryString("LookupEMail").Length != 0)
			{
				if(WhereClause.Length != 0)
				{
					WhereClause += " and ";
				}
				WhereClause += " email=" + DB.SQuote(Common.QueryString("LookupEMail").ToLower());
			}

//SEC4
      string SuperuserFilter = Common.IIF(thisCustomer.IsAdminSuperUser , String.Empty , String.Format(" Customer.CustomerID not in ({0}) and ",Common.AppConfig("Admin_Superuser")));
      
      IDataReader rs = DB.GetRS("select * from customer  " + DB.GetNoLock() + " " + Common.IIF(WhereClause.Length != 0 , " where " + SuperuserFilter.ToString() +  WhereClause , " where 1=-1 "));
			if(!rs.Read())
			{
				rs.Close();
				rs = DB.GetRS("select * from orders  " + DB.GetNoLock() + " " + Common.IIF(WhereClause.Length != 0 , " where " + WhereClause , " where 1=-1 "));
				rs.Read();
			}

			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function copyaccount(theForm)\n");
			writer.Write("{\n");
			writer.Write("if (theForm.ShippingEqualsAccount.checked)\n");
			writer.Write("{\n");
			writer.Write("theForm.ShippingFirstName.value = theForm.FirstName.value;\n");
			writer.Write("theForm.ShippingLastName.value = theForm.LastName.value;\n");
			writer.Write("theForm.ShippingPhone.value = theForm.Phone.value;\n");
			writer.Write("}\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");
			writer.Write("\n");
			writer.Write("function copyshipping(theForm)\n");
			writer.Write("{\n");
			writer.Write("if (theForm.BillingEqualsShipping.checked)\n");
			writer.Write("{\n");
			writer.Write("theForm.BillingFirstName.value = theForm.ShippingFirstName.value;\n");
			writer.Write("theForm.BillingLastName.value = theForm.ShippingLastName.value;\n");
			writer.Write("theForm.BillingPhone.value = theForm.ShippingPhone.value;\n");
			writer.Write("theForm.BillingCompany.value = theForm.ShippingCompany.value;\n");
			writer.Write("theForm.BillingAddress1.value = theForm.ShippingAddress1.value;\n");
			writer.Write("theForm.BillingAddress2.value = theForm.ShippingAddress2.value;\n");
			writer.Write("theForm.BillingSuite.value = theForm.ShippingSuite.value;\n");
			writer.Write("theForm.BillingCity.value = theForm.ShippingCity.value;\n");
			writer.Write("theForm.BillingState.selectedIndex = theForm.ShippingState.selectedIndex;\n");
			writer.Write("theForm.BillingZip.value = theForm.ShippingZip.value;\n");
			writer.Write("theForm.BillingCountry.selectedIndex = theForm.ShippingCountry.selectedIndex;\n");
			writer.Write("}\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");
			writer.Write("function OfflineOrderForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("  submitonce(theForm);\n");
			writer.Write("  if (theForm.FirstName.value == \"\" && theForm.LastName.value == \"\")\n");
			writer.Write("  {\n");
			writer.Write("    alert(\"You must enter at least one of First Name or Last Name.\");\n");
			writer.Write("    theForm.FirstName.focus();\n");
			writer.Write("    submitenabled(theForm);\n");
			writer.Write("    return (false);\n");
			writer.Write("  }\n");
			writer.Write("  if (theForm.ShippingState.selectedIndex < 1)\n");
			writer.Write("  {\n");
			writer.Write("    alert(\"Please select one of the Shipping State options.\");\n");
			writer.Write("    theForm.ShippingState.focus();\n");
			writer.Write("    submitenabled(theForm);\n");
			writer.Write("    return (false);\n");
			writer.Write("  }\n");
			writer.Write("  if (theForm.BillingState.selectedIndex < 1)\n");
			writer.Write("  {\n");
			writer.Write("    alert(\"Please select one of the Billing State options.\");\n");
			writer.Write("    theForm.BillingState.focus();\n");
			writer.Write("    submitenabled(theForm);\n");
			writer.Write("    return (false);\n");
			writer.Write("  }\n");
			writer.Write("  if (theForm.PaymentMethod.value != \"Check\")\n");
			writer.Write("  {\n");
			writer.Write("    alert(\"Only the 'Check' payment method is currently supported for offline orders.\");\n");
			writer.Write("    theForm.PaymentMethod.focus();\n");
			writer.Write("    submitenabled(theForm);\n");
			writer.Write("    return (false);\n");
			writer.Write("  }\n");
			writer.Write("  if (theForm.Description_1.value.length == 0)\n");
			writer.Write("  {\n");
			writer.Write("    alert(\"Please enter at least one item description.\");\n");
			writer.Write("    theForm.Description_1.focus();\n");
			writer.Write("    submitenabled(theForm);\n");
			writer.Write("    return (false);\n");
			writer.Write("  }\n");
			writer.Write("  if (theForm.Quantity_1.value.length == 0)\n");
			writer.Write("  {\n");
			writer.Write("    alert(\"Please enter at least one item quantity.\");\n");
			writer.Write("    theForm.Quantity_1.focus();\n");
			writer.Write("    submitenabled(theForm);\n");
			writer.Write("    return (false);\n");
			writer.Write("  }\n");
			writer.Write("  if (theForm.Price_1.value.length == 0)\n");
			writer.Write("  {\n");
			writer.Write("    alert(\"Please enter at least one item price.\");\n");
			writer.Write("    theForm.Price_1.focus();\n");
			writer.Write("    submitenabled(theForm);\n");
			writer.Write("    return (false);\n");
			writer.Write("  }\n");
			writer.Write("  return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");
			
			String act = "offlineorder_process.aspx";
			writer.Write("<form method=\"POST\" action=\"" + act + "\" name=\"OfflineOrderForm\" id=\"OfflineOrderForm\" onSubmit=\"return (validateForm(this) && OfflineOrderForm_Validator(this))\">");


			// ACCOUNT BOX:
			writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			writer.Write("<img src=\"../skins/Skin_" + _siteID.ToString() + "/images/accountinfo.gif\" border=\"0\"><br>");
			writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

			writer.Write("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
			writer.Write("            <tr>");
			writer.Write("              <td width=\"25%\">Order Date:</td>");
			writer.Write("              <td width=\"75%\"><input type=\"text\" name=\"OrderDate\" size=\"11\" value=\"" + Localization.ToNativeShortDateString(System.DateTime.Now) + "\">&nbsp;<img src=\"skins/skin_" + _siteID.ToString() + "/images/calendar.gif\" style=\"cursor:hand;\" align=\"absmiddle\" id=\"f_trigger_s\">");
			writer.Write("                	<input type=\"hidden\" name=\"OrderDate_vldt\" value=\"[date][invalidalert=Please enter a valid order date in the format " + Localization.ShortDateFormat() + "]\">");
			writer.Write("</td>");
			writer.Write("            </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">First Name:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"FirstName\" size=\"20\" maxlength=\"50\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"FirstName")) + "\"> (required)");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Last Name:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"LastName\" size=\"20\" maxlength=\"50\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"LastName")) + "\"> (required)");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">E-Mail:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"EMail\" size=\"37\" maxlength=\"100\" value=\"" + Common.IIF(!thisCustomer._isAnon , DB.RSField(rs,"EMail") , String.Empty) + "\">");
			writer.Write("        <input type=\"hidden\" name=\"EMail_vldt\" value=\"[email][blankalert=Please enter the e-mail address][invalidalert=Please enter a valid e-mail address]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			
			writer.Write("</table>\n");
			
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");


			// SHIPPING BOX:
			writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			writer.Write("<img src=\"../skins/Skin_" + _siteID.ToString() + "/images/shippinginfo.gif\" border=\"0\"><br>");
			writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			
			writer.Write("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"100%\" colspan=\"2\"><b>Enter the shipping information below. (Same as account info: <input type=\"checkbox\" name=\"ShippingEqualsAccount\" value=\"ON\" onClick=\"copyaccount(OfflineOrderForm);\">)</b></td>");
			writer.Write("      </tr>");
			writer.Write("    </table>");
			writer.Write("    <hr>");
			writer.Write("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">First Name:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"ShippingFirstName\" size=\"20\" maxlength=\"50\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"ShippingFirstName")) + "\"> (required)");
			writer.Write("        <input type=\"hidden\" name=\"ShippingFirstName_vldt\" value=\"[req][blankalert=Please enter the shipping first name]\">\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Last Name:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"ShippingLastName\" size=\"20\" maxlength=\"50\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"ShippingLastName")) + "\"> (required)");
			writer.Write("        <input type=\"hidden\" name=\"ShippingLastName_vldt\" value=\"[req][blankalert=Please enter the shipping last name]\">\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Phone:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"ShippingPhone\" size=\"20\" maxlength=\"25\" value=\"" + DB.RSField(rs,"ShippingPHone") + "\"> (required)");
			writer.Write("        <input type=\"hidden\" name=\"ShippingPhone_vldt\" value=\"[req][blankalert=Please enter a phone number for the shipping address][invalidalert=Please enter a valid phone number]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Company:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"ShippingCompany\" size=\"34\" maxlength=\"100\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"ShippingCompany")) + "\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Address1:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"ShippingAddress1\" size=\"34\" maxlength=\"100\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"ShippingAddress1")) + "\"> (required)");
			writer.Write("        <input type=\"hidden\" name=\"ShippingAddress1_vldt\" value=\"[req][blankalert=Please enter a shipping address]\">\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Address2:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"ShippingAddress2\" size=\"34\" maxlength=\"100\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"ShippingAddress2")) + "\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Suite:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"ShippingSuite\" size=\"34\" maxlength=\"50\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"ShippingSuite")) + "\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">City:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"ShippingCity\" size=\"34\" maxlength=\"50\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"ShippingCity")) + "\"> (required)");
			writer.Write("        <input type=\"hidden\" name=\"ShippingCity_vldt\" value=\"[req][blankalert=Please enter a shipping city]\">\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">State/Province:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <select size=\"1\" name=\"ShippingState\">");
			writer.Write("<OPTION value=\"\"" + Common.IIF(DB.RSField(rs,"ShippingState").Length == 0 , " selected" , String.Empty) + ">SELECT ONE</option>");
			DataSet dsstate = DB.GetDS("select * from state  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddHours(1));
			foreach(DataRow row in dsstate.Tables[0].Rows)
			{
				writer.Write("<OPTION value=\"" + DB.RowField(row,"Abbreviation") + "\"" + Common.SelectOption(rs,DB.RowField(row,"Abbreviation"),"shippingstate") + ">" + DB.RowField(row,"Name") + "</option>");
			}
			dsstate.Dispose();
			writer.Write("</select> (required)");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Zip:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"ShippingZip\" size=\"14\" maxlength=\"10\" value=\"" + DB.RSField(rs,"ShippingZip") + "\"> (required)");
			writer.Write("        <input type=\"hidden\" name=\"ShippingZip_vldt\" value=\"[req][blankalert=Please enter the shipping zipcode][invalidalert=Please enter a valid zipcode]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Country:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("<SELECT NAME=\"ShippingCountry\" size=\"1\">");
			DataSet dscountry = DB.GetDS("select * from country  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddHours(1));
			foreach(DataRow row in dscountry.Tables[0].Rows)
			{
				writer.Write("<OPTION value=\"" + DB.RowField(row,"Name") + "\"" + Common.SelectOption(rs,DB.RowField(row,"Name"),"ShippingCountry") + ">" + DB.RowField(row,"Name") + "</option>");
			}
			dscountry.Dispose();
			writer.Write("</SELECT> (required)");

			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("</table>\n");

			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");


			// BILLING BOX:

			writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			writer.Write("<img src=\"../skins/Skin_" + _siteID.ToString() + "/images/billinginfo.gif\" border=\"0\"><br>");
			writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

			writer.Write("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"100%\" colspan=\"2\"><b>Enter the billing information below. (Same as shipping info: <input type=\"checkbox\" name=\"BillingEqualsShipping\" value=\"ON\" onClick=\"copyshipping(OfflineOrderForm);\">)</b></td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"100%\" colspan=\"2\">");
			writer.Write("    <hr>");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">First Name:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"BillingFirstName\" size=\"20\" maxlength=\"50\"  value=\"" + Server.HtmlEncode(DB.RSField(rs,"BillingFirstName")) + "\">");
			writer.Write("        <input type=\"hidden\" name=\"BillingFirstName_vldt\" value=\"[req][blankalert=Please enter the billing person first name]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Last Name:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"BillingLastName\" size=\"20\" maxlength=\"50\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"BillingLastName")) + "\">");
			writer.Write("        <input type=\"hidden\" name=\"BillingLastName_vldt\" value=\"[req][blankalert=Please enter the billing person last name]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Phone:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"BillingPhone\" size=\"20\" maxlength=\"25\" value=\"" + DB.RSField(rs,"BillingPhone") + "\">");
			writer.Write("        <input type=\"hidden\" name=\"BillingPhone_vldt\" value=\"[req][blankalert=Please enter the billing person phone][invalidalert=Please enter a valid phone number]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Company:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"BillingCompany\" size=\"34\"  maxlength=\"100\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"BillingCompany")) + "\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Address1:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"BillingAddress1\" size=\"34\"  maxlength=\"100\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"BillingAddress1")) + "\">");
			writer.Write("        <input type=\"hidden\" name=\"BillingAddress1_vldt\" value=\"[req][blankalert=Please enter the billing address]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Address2:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"BillingAddress2\" size=\"34\"  maxlength=\"100\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"BillingAddress2")) + "\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Suite:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"BillingSuite\" size=\"34\"  maxlength=\"50\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"BillingSuite")) + "\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">City:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"BillingCity\" size=\"34\"  maxlength=\"50\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"BillingCity")) + "\">");
			writer.Write("        <input type=\"hidden\" name=\"BillingCity_vldt\" value=\"[req][blankalert=Please enter the billing city]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">State/Province:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <select size=\"1\" name=\"BillingState\">");
			writer.Write("<OPTION value=\"\"" + Common.IIF(DB.RSField(rs,"BillingState").Length == 0 , " selected" , String.Empty) + ">SELECT ONE</option>");
			dsstate = DB.GetDS("select * from state  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddHours(1));
			foreach(DataRow row in dsstate.Tables[0].Rows)
			{
				writer.Write("<OPTION value=\"" + DB.RowField(row,"Abbreviation") + "\"" + Common.SelectOption(rs,DB.RowField(row,"Abbreviation"),"BillingState") + ">" + DB.RowField(row,"Name") + "</option>");
			}
			dsstate.Dispose();
			writer.Write("</select> (required)");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Zip:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"BillingZip\" size=\"14\" maxlength=\"10\" value=\"" + DB.RSField(rs,"BillingZip") + "\">");
			writer.Write("        <input type=\"hidden\" name=\"BillingZip_vldt\" value=\"[req][blankalert=Please enter the billing zipcode][invalidalert=Please enter a valid zipcode]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Country:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("<SELECT NAME=\"BillingCountry\" size=\"1\">");
			DataSet dscountry2 = DB.GetDS("select * from country  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddHours(1));
			foreach(DataRow row in dscountry2.Tables[0].Rows)
			{
				writer.Write("<OPTION value=\"" + DB.RowField(row,"Name") + "\"" + Common.SelectOption(rs,DB.RowField(row,"Name"),"BillingCountry") + ">" + DB.RowField(row,"Name") + "</option>");
			}
			dscountry2.Dispose();
			writer.Write("</SELECT>");

			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("</table>\n");
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");

			// PAYMENT BOX:
			writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			writer.Write("<img src=\"../skins/Skin_" + _siteID.ToString() + "/images/paymentinfo.gif\" border=\"0\"><br>");
			writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

			writer.Write("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Payment Method:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"PaymentMethod\" size=\"20\" maxlength=\"50\" value=\"Check\"> (required, at this time only 'Check' is supported)");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Order Subtotal:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"SubTotal\" size=\"20\" maxlength=\"50\" value=\"\"><input type=\"hidden\" name=\"SubTotal_vldt\" value=\"[req][number][blankalert=Please enter the order subtotal][invalidalert=Please enter a valid dollar amount, e.g. 10.00 without the leading $ sign!]\"> (required)");
			writer.Write("&nbsp;&nbsp;<a href=\"javascript:void(0);\" onClick=\"ComputeTaxFromSubTotal(this);\">Compute Tax and Total</a>");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Order Tax:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"Tax\" size=\"20\" maxlength=\"50\" value=\"\"><input type=\"hidden\" name=\"Tax_vldt\" value=\"[number][blankalert=Please enter the order tax amount, if any][invalidalert=Please enter a valid dollar amount, e.g. 10.00 without the leading $ sign!]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Order Shipping & Handling:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"Shipping\" size=\"20\" maxlength=\"50\" value=\"\"><input type=\"hidden\" name=\"Shipping_vldt\" value=\"[number][blankalert=Please enter the shipping and handling charges, if any][invalidalert=Please enter a valid dollar amount, e.g. 10.00 without the leading $ sign!]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Order Total:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"Total\" size=\"20\" maxlength=\"50\" value=\"\"><input type=\"hidden\" name=\"Total_vldt\" value=\"[req][number][blankalert=Please enter the order total][invalidalert=Please enter a valid dollar amount, e.g. 10.00 without the leading $ sign!]\"> (required)");
			writer.Write("&nbsp;&nbsp;<a href=\"javascript:void(0);\" onClick=\"ComputeTaxFromTotal(this);\">Compute Tax and SubTotal</a>");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td valign=\"top\" width=\"25%\">Order Notes:</td>");
			writer.Write("        <td valign=\"top\" width=\"75%\">");
			writer.Write("        <textarea rows=\"10\" cols=\"100\" name=\"OrderNotes\"></textarea>");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("</table>\n");
			
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");

			// ORDER ITEMS:
			writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			writer.Write("<img src=\"../skins/Skin_" + _siteID.ToString() + "/images/orderitems.gif\" border=\"0\"><br>");
			writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

			writer.Write("<p><b>The following order items are created in the order record. The values are used for documentation purposes only. The amounts/costs below are NOT used to determine the order total. The above order total is used. You must enter at least one item below.</b></p>");
			writer.Write("<table border=\"0\" cellspacing=\"0\" width=\"100%\">\n");
			writer.Write("<tr>\n");
			writer.Write("<td><b>&nbsp;</b></td>\n");
			writer.Write("<td><b>Description</b></td>\n");
			writer.Write("<td><b>SKU</b></td>\n");
			writer.Write("<td><b>Quantity</b></td>\n");
			writer.Write("<td><b>Item Cost (Each)</b></td>\n");
			writer.Write("</tr>\n");
			for(int i = 1; i <= 5; i++)
			{
				if(i > 1)
				{
					writer.Write("<tr><td colspan=5><hr size=1></td></tr>\n");
				}
				writer.Write("<tr>\n");
				writer.Write("<td valign=\"top\" align=\"left\">Item " + i.ToString() + ":</td>\n");
				writer.Write("<td valign=\"top\" align=\"left\"><textarea rows=\"4\" cols=\"100\" name=\"Description_" + i.ToString() + "\" cols=\"35\"></textarea></td>\n");
				writer.Write("<td valign=\"top\" align=\"left\"><input type=\"text\" name=\"SKU_" + i.ToString() + "\" size=\"20\"></td>\n");
				writer.Write("<td valign=\"top\" align=\"left\"><input type=\"text\" name=\"Quantity_" + i.ToString() + "\" size=\"20\"></td>\n");
				writer.Write("<td valign=\"top\" align=\"left\"><input type=\"text\" name=\"Price_" + i.ToString() + "\" size=\"20\"></td>\n");
				writer.Write("</tr>\n");
			}
			writer.Write("</table>\n");
			
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");

			writer.Write("<input type=\"submit\" value=\"Create Order\" name=\"Continue\">");
			writer.Write("</form>");

			writer.Write("\n<script type=\"text/javascript\">\n");
			writer.Write("    Calendar.setup({\n");
			writer.Write("        inputField     :    \"OrderDate\",      // id of the input field\n");
			writer.Write("        ifFormat       :    \"" + Localization.JSCalendarDateFormatSpec() + "\",       // format of the input field\n");
			writer.Write("        showsTime      :    false,            // will display a time selector\n");
			writer.Write("        button         :    \"f_trigger_s\",   // trigger for the calendar (button ID)\n");
			writer.Write("        singleClick    :    true            // Single-click mode\n");
			writer.Write("    });\n");
			writer.Write("</script>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");

			writer.Write("var States = new Array(100);\n");
			writer.Write("var Rates = new Array(100);\n");
			IDataReader rss = DB.GetRS("select * from StateTaxRate  " + DB.GetNoLock() + " where TaxRate IS NOT NULL and TaxRate<>0.0 order by State");
			int ix = 0;
			while(rss.Read())
			{
				writer.Write("States[" + ix.ToString() + "] = '" + DB.RSField(rss,"State") + "';\n");
				writer.Write("Rates[" + ix.ToString() + "] = " + DB.RSFieldSingle(rss,"TaxRate").ToString() + ";\n");
				ix++;
			}
			rss.Close();
			writer.Write("function GetShippingStateTaxRate(theForm)\n");
			writer.Write("{\n");
			writer.Write("var ShippingState = document.OfflineOrderForm.ShippingState.value;\n");
			writer.Write("for(var i = 0; i < 100; i++)\n");
			writer.Write("{\n");
			writer.Write("if(States[i] == ShippingState)\n");
			writer.Write("{\n");
			writer.Write("return Rates[i];\n");
			writer.Write("}\n");
			writer.Write("}\n");
			writer.Write("return 0.0;\n");
			writer.Write("}\n");

			writer.Write("function ComputeTaxFromSubTotal(theForm)\n");
			writer.Write("{\n");
			writer.Write("var TaxRate = 1.0 * GetShippingStateTaxRate(theForm);\n");
			writer.Write("var SubTotal = 1.0 * document.OfflineOrderForm.SubTotal.value;\n");
			writer.Write("document.OfflineOrderForm.Tax.value = Math.round(100* (SubTotal * (TaxRate/100.0)) )/100;\n");
			writer.Write("document.OfflineOrderForm.Total.value = Math.round(100* (1.0 * SubTotal + 1.0 * document.OfflineOrderForm.Tax.value) )/100;\n");
			writer.Write("}\n");

			writer.Write("function ComputeTaxFromTotal(theForm)\n");
			writer.Write("{\n");
			writer.Write("var TaxRate = 1.0 * GetShippingStateTaxRate(theForm);\n");
			writer.Write("alert(TaxRate);\n");
			writer.Write("var Total = 1.0 * document.OfflineOrderForm.Total.value;\n");
			writer.Write("document.OfflineOrderForm.SubTotal.value = Math.round(100* (1.0 * document.OfflineOrderForm.Total.value) / (1 + (TaxRate/100.0)) )/100;\n");
			writer.Write("document.OfflineOrderForm.Tax.value = Math.round(100* (1.0 * document.OfflineOrderForm.Total.value * (TaxRate/100.0)) )/100;\n");
			writer.Write("}\n");

			writer.Write("</SCRIPT>\n");
			rs.Close();
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
