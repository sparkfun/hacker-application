// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2004.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for createaccount.
	/// </summary>
	public class createaccount : SkinBase
	{

		String PaymentMethod = String.Empty;
		bool DoingCheckout = false;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Account Information";
			DoingCheckout = (Common.QueryString("checkout").ToLower() == "true");
			if(DoingCheckout)
			{
				thisCustomer.RequireCustomerRecord();
				SectionTitle = "Checkout - " + SectionTitle;
			}
			PaymentMethod = Common.QueryString("PaymentMethod");
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			bool AllowShipToDifferentThanBillTo = Common.AppConfigBool("AllowShipToDifferentThanBillTo");
			String ReturnURL = Common.QueryString("ReturnURL");

			Topic t = new Topic("CreateAccountPageHeader",thisCustomer._localeSetting,_siteID);
			writer.Write(t._contents);
			
			Address BillingAddress = new Address(); 
			Address ShippingAddress = new Address(); 

			BillingAddress.LoadByCustomer(thisCustomer._customerID,AddressTypes.Billing);
			ShippingAddress.LoadByCustomer(thisCustomer._customerID,AddressTypes.Shipping);

			if(Common.QueryString("errormsg").Length > 0)
			{
				writer.Write("<p align=\"left\"><b><font color=\"red\">" + Server.HtmlEncode(Common.QueryString("ErrorMsg")) + "</font></b></p>");
			}

			if(DoingCheckout)
			{
				writer.Write("<p align=\"left\">If you already have an account with us, <a href=\"signin.aspx?returnurl=ShoppingCart.aspx\"><b>sign in here</b></a>.</p>\n");
			}

			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function copyaccount(theForm)\n");
			writer.Write("{\n");
			writer.Write("if (theForm.BillingEqualsAccount.checked)\n");
			writer.Write("	{\n");
			writer.Write("	theForm.BillingFirstName.value = theForm.FirstName.value;\n");
			writer.Write("	theForm.BillingLastName.value = theForm.LastName.value;\n");
			writer.Write("	theForm.BillingPhone.value = theForm.Phone.value;\n");
			writer.Write("	}\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");
			writer.Write("\n");
			if(AllowShipToDifferentThanBillTo)
			{
				writer.Write("function copybilling(theForm)\n");
				writer.Write("{\n");
				writer.Write("if (theForm.ShippingEqualsBilling.checked)\n");
				writer.Write("	{\n");
				writer.Write("	theForm.ShippingFirstName.value = theForm.BillingFirstName.value;\n");
				writer.Write("	theForm.ShippingLastName.value = theForm.BillingLastName.value;\n");
				writer.Write("	theForm.ShippingPhone.value = theForm.BillingPhone.value;\n");
				writer.Write("	theForm.ShippingCompany.value = theForm.BillingCompany.value;\n");
				writer.Write("	theForm.ShippingAddress1.value = theForm.BillingAddress1.value;\n");
				writer.Write("	theForm.ShippingAddress2.value = theForm.BillingAddress2.value;\n");
				writer.Write("	theForm.ShippingSuite.value = theForm.BillingSuite.value;\n");
				writer.Write("	theForm.ShippingCity.value = theForm.BillingCity.value;\n");
				writer.Write("	theForm.ShippingState.selectedIndex = theForm.BillingState.selectedIndex;\n");
				writer.Write("	theForm.ShippingZip.value = theForm.BillingZip.value;\n");
				writer.Write("	theForm.ShippingCountry.selectedIndex = theForm.BillingCountry.selectedIndex;\n");
				writer.Write("	}\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");
			}
			writer.Write("function CreateAccountForm_Validator(theForm)\n");
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
			writer.Write("    alert(\"Please enter at least 4 characters for your password.\");\n");
			writer.Write("    theForm.Password.focus();\n");
			writer.Write("    submitenabled(theForm);\n");
			writer.Write("    return (false);\n");
			writer.Write("  }\n");
			writer.Write("  if (theForm.Password.value != theForm.Password2.value)\n");
			writer.Write("  {\n");
			writer.Write("    alert(\"Your passwords do not match, Please make sure your password is re-entered properly.\");\n");
			writer.Write("    theForm.Password2.focus();\n");
			writer.Write("    submitenabled(theForm);\n");
			writer.Write("    return (false);\n");
			writer.Write("  }\n");
			writer.Write("  if (theForm.BillingState.selectedIndex < 1)\n");
			writer.Write("  {\n");
			writer.Write("    alert(\"Please select one of the Billing State options. If Outside U.S, select 'Other (Non US)'\");\n");
			writer.Write("    theForm.BillingState.focus();\n");
			writer.Write("    submitenabled(theForm);\n");
			writer.Write("    return (false);\n");
			writer.Write("  }\n");
			writer.Write("  if (theForm.BillingCountry.selectedIndex < 1)\n");
			writer.Write("  {\n");
			writer.Write("    alert(\"Please select one of the Billing Country options.\");\n");
			writer.Write("    theForm.BillingCountry.focus();\n");
			writer.Write("    submitenabled(theForm);\n");
			writer.Write("    return (false);\n");
			writer.Write("  }\n");
			if(AllowShipToDifferentThanBillTo)
			{
				writer.Write("  if (theForm.ShippingState.selectedIndex < 1)\n");
				writer.Write("  {\n");
				writer.Write("    alert(\"Please select one of the Shipping State options. If Outside U.S, select 'Other (Non US)'\");\n");
				writer.Write("    theForm.ShippingState.focus();\n");
				writer.Write("    submitenabled(theForm);\n");
				writer.Write("    return (false);\n");
				writer.Write("  }\n");
				writer.Write("  if (theForm.ShippingCountry.selectedIndex < 1)\n");
				writer.Write("  {\n");
				writer.Write("    alert(\"Please select one of the Shipping Country options.\");\n");
				writer.Write("    theForm.ShippingCountry.focus();\n");
				writer.Write("    submitenabled(theForm);\n");
				writer.Write("    return (false);\n");
				writer.Write("  }\n");
			}
			writer.Write("  return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");
			
			String act = "createaccount_process.aspx?checkout=" + DoingCheckout.ToString() + "&PaymentMethod=" + Server.UrlEncode(PaymentMethod);
			writer.Write("<form method=\"POST\" action=\"" + act + "\" name=\"CreateAccountForm\" id=\"CreateAccountForm\" onSubmit=\"return (validateForm(this) && CreateAccountForm_Validator(this))\">");


			// ACCOUNT BOX:
			writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/accountinfo.gif\" border=\"0\"><br>");
			writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

			writer.Write("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"100%\" colspan=\"2\"><b>Your Account &amp; Contact Information is used to login to the site. Please save your password in a safe place.</b></td>");
			writer.Write("      </tr>");

			writer.Write("      <tr>");
			writer.Write("        <td width=\"100%\" colspan=\"2\">");
			writer.Write("          <hr>");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">*Your First Name:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" id=\"FirstName\" name=\"FirstName\" size=\"20\" maxlength=\"50\" value=\"" + Server.HtmlEncode(thisCustomer._firstName) + "\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">*Your Last Name:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" id=\"LastName\" name=\"LastName\" size=\"20\" maxlength=\"50\" value=\"" + Server.HtmlEncode(thisCustomer._lastName) + "\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">*Your E-Mail:</td>");
			writer.Write("        <td width=\"75%\">");
			String EMail = thisCustomer._email;
			if(EMail.StartsWith("Anon_"))
			{
				EMail = String.Empty;
			}
			writer.Write("        <input type=\"text\" name=\"EMail\" size=\"37\" maxlength=\"100\" value=\"" + Server.HtmlEncode(EMail) + "\">");
			writer.Write("        <input type=\"hidden\" name=\"EMail_vldt\" value=\"[req][blankalert=Please enter your e-mail address][invalidalert=Please enter a valid e-mail address]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			String PWD = thisCustomer._password;
			if(PWD == "N/A")
			{
				PWD = String.Empty;
			}
			
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">*Create a Personal Password:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"password\" name=\"Password\" size=\"20\" maxlength=\"50\" value=\"" + PWD + "\"> (at least 4 chars long)");
			writer.Write("        <input type=\"hidden\" name=\"Password_vldt\" value=\"[req][len=4][blankalert=Please enter a password so you can login to this site at a later time]\">\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">*Re-Enter Your Password:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"password\" name=\"Password2\" size=\"20\" maxlength=\"50\" value=\"" + PWD + "\">");
			writer.Write("        <input type=\"hidden\" name=\"Password2_vldt\" value=\"[req][len=4][blankalert=Please re-enter your password ]\">\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">" + Common.IIF(DoingCheckout,"*","") + "Phone:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" id=\"Phone\" name=\"Phone\" size=\"14\" maxlength=\"20\" value=\"" + Server.HtmlEncode(thisCustomer._phone) + "\">");
			writer.Write("        <input type=\"hidden\" name=\"Phone_vldt\" value=\"" + Common.IIF(DoingCheckout,"[req]","") + "[blankalert=Please enter your phone number][invalidalert=Please enter a valid phone number]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">*OK To EMail:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("		Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"OkToEmail\" value=\"1\" " + Common.IIF(!thisCustomer._isAnon , Common.IIF(thisCustomer._okToEMail , " checked " , "") , " checked ") + ">\n");
			writer.Write("		No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"OkToEmail\" value=\"0\" " + Common.IIF(!thisCustomer._isAnon , Common.IIF(thisCustomer._okToEMail , "" , " checked ") , "") + ">\n");
			writer.Write("<small>(Can we contact you with product updates/information, etc.)</small>");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("</table>\n");
			
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");

			// BILLING BOX:

			writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			if(AllowShipToDifferentThanBillTo)
			{
				writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/billinginfo.gif\" border=\"0\"><br>");
			}
			else
			{
				writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/shippingandbillinginfo.gif\" border=\"0\"><br>");
			}
			writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

			writer.Write("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
			if(AllowShipToDifferentThanBillTo)
			{
				writer.Write("      <tr>");
				writer.Write("        <td width=\"100%\" colspan=\"2\"><b>Enter your billing information below. (Same as account info: <input type=\"checkbox\" name=\"BillingEqualsAccount\" value=\"ON\" onClick=\"copyaccount(CreateAccountForm);\">)</b></td>");
				writer.Write("      </tr>");
			}
			else
			{
				writer.Write("      <tr>");
				writer.Write("        <td width=\"100%\" colspan=\"2\"><b>Enter your billing address below.  Your shipping address must be the same as your billing address.<br>Click here if this is the same as above: <input type=\"checkbox\" name=\"BillingEqualsAccount\" value=\"ON\" onClick=\"copyaccount(CreateAccountForm);\"></b></td>");
				writer.Write("      </tr>");
			}
			writer.Write("      <tr>");
			writer.Write("        <td width=\"100%\" colspan=\"2\">");
			writer.Write("    <hr>");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">*First Name:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"BillingFirstName\" size=\"20\" maxlength=\"50\"  value=\"" + Server.HtmlEncode(BillingAddress.FirstName) + "\">");
			writer.Write("        <input type=\"hidden\" name=\"BillingFirstName_vldt\" value=\"[req][blankalert=Please enter the billing person first name]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">*Last Name:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"BillingLastName\" size=\"20\" maxlength=\"50\" value=\"" + Server.HtmlEncode(BillingAddress.LastName) + "\">");
			writer.Write("        <input type=\"hidden\" name=\"BillingLastName_vldt\" value=\"[req][blankalert=Please enter the billing person last name]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">*Phone:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"BillingPhone\" size=\"20\" maxlength=\"25\" value=\"" + Server.HtmlEncode(BillingAddress.Phone) + "\">");
			writer.Write("        <input type=\"hidden\" name=\"BillingPhone_vldt\" value=\"[req][blankalert=Please enter the billing person phone][invalidalert=Please enter a valid phone number]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Company:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"BillingCompany\" size=\"34\"  maxlength=\"100\" value=\"" + Server.HtmlEncode(BillingAddress.Company) + "\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">*Address1:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"BillingAddress1\" size=\"34\"  maxlength=\"100\" value=\"" + Server.HtmlEncode(BillingAddress.Address1) + "\">");
			writer.Write("        <input type=\"hidden\" name=\"BillingAddress1_vldt\" value=\"[req][blankalert=Please enter the billing address]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Address2:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"BillingAddress2\" size=\"34\"  maxlength=\"100\" value=\"" + Server.HtmlEncode(BillingAddress.Address2) + "\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Suite:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"BillingSuite\" size=\"34\"  maxlength=\"50\" value=\"" + Server.HtmlEncode(BillingAddress.Suite) + "\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">*City or APO/AFO:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" name=\"BillingCity\" size=\"34\"  maxlength=\"50\" value=\"" + Server.HtmlEncode(BillingAddress.City) + "\">");
			writer.Write("        <input type=\"hidden\" name=\"BillingCity_vldt\" value=\"[req][blankalert=Please enter the billing city]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">*State/Province:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("<select size=\"1\" name=\"BillingState\">");
			writer.Write("<OPTION value=\"\"" + Common.IIF(BillingAddress.State.Length==0 , " selected" , String.Empty) + ">SELECT ONE</option>");
			DataSet dsstate2 = DB.GetDS("select * from state  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddMinutes(Common.CacheDurationMinutes()));
			foreach(DataRow row in dsstate2.Tables[0].Rows)
			{
				writer.Write("<OPTION value=\"" + DB.RowField(row,"Abbreviation") + "\"" + Common.IIF(BillingAddress.State == DB.RowField(row,"Abbreviation"), " selected ","") + ">" + DB.RowField(row,"Name") + "</option>");
			}
			dsstate2.Dispose();
			writer.Write("</select>");
			writer.Write("</td>");
			writer.Write("</tr>");

			writer.Write("<tr>");
			writer.Write("<td width=\"25%\">*Zip:</td>");
			writer.Write("<td width=\"75%\">");
			writer.Write("<input type=\"text\" name=\"BillingZip\" size=\"14\" maxlength=\"10\" value=\"" + BillingAddress.Zip + "\">");
			writer.Write("<input type=\"hidden\" name=\"BillingZip_vldt\" value=\"[blankalert=Please enter the billing zipcode][invalidalert=Please enter a valid zipcode]\">");
			writer.Write("</td>");
			writer.Write("</tr>");
			writer.Write("<tr>");
			writer.Write("<td width=\"25%\">*Country:</td>");
			writer.Write("<td width=\"75%\">");
			writer.Write("<SELECT NAME=\"BillingCountry\" size=\"1\">");
			writer.Write("<option value=\"0\">SELECT ONE</option>");
			DataSet dscountry2 = DB.GetDS("select * from country  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddMinutes(Common.CacheDurationMinutes()));
			foreach(DataRow row in dscountry2.Tables[0].Rows)
			{
				writer.Write("<OPTION value=\"" + DB.RowField(row,"Name") + "\"" + Common.IIF(BillingAddress.Country == DB.RowField(row,"Name"), " selected ","") + ">" + DB.RowField(row,"Name") + "</option>");
			}
			dscountry2.Dispose();
			writer.Write("</SELECT>");
			writer.Write("</td>");
			writer.Write("</tr>");

			writer.Write("</table>\n");

			if(AllowShipToDifferentThanBillTo)
			{
				// SHIPPING BOX:
				writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
				writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
				writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/shippinginfo.gif\" border=\"0\"><br>");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
				writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			
				writer.Write("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"100%\" colspan=\"2\"><b>Enter your shipping information below. (Same as billing info: <input type=\"checkbox\" name=\"ShippingEqualsBilling\" value=\"ON\" onClick=\"copybilling(CreateAccountForm);\">)</b></td>");
				writer.Write("      </tr>");
				writer.Write("    </table>");
				writer.Write("    <hr>");
				writer.Write("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">*First Name:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"ShippingFirstName\" size=\"20\" maxlength=\"50\" value=\"" + Server.HtmlEncode(ShippingAddress.FirstName) + "\">");
				writer.Write("        <input type=\"hidden\" name=\"ShippingFirstName_vldt\" value=\"[req][blankalert=Please enter the shipping first name]\">\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">*Last Name:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"ShippingLastName\" size=\"20\" maxlength=\"50\" value=\"" + Server.HtmlEncode(ShippingAddress.LastName) + "\">");
				writer.Write("        <input type=\"hidden\" name=\"ShippingLastName_vldt\" value=\"[req][blankalert=Please enter the shipping last name]\">\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">*Phone:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"ShippingPhone\" size=\"20\" maxlength=\"25\" value=\"" + Server.HtmlEncode(ShippingAddress.Phone) + "\">");
				writer.Write("        <input type=\"hidden\" name=\"ShippingPhone_vldt\" value=\"[req][blankalert=Please enter a phone number for the shipping address][invalidalert=Please enter a valid phone number]\">");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">Company:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"ShippingCompany\" size=\"34\" maxlength=\"100\" value=\"" + Server.HtmlEncode(ShippingAddress.Company) + "\">");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">*Address1:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"ShippingAddress1\" size=\"34\" maxlength=\"100\" value=\"" + Server.HtmlEncode(ShippingAddress.Address1) + "\">");
				writer.Write("        <input type=\"hidden\" name=\"ShippingAddress1_vldt\" value=\"[req][blankalert=Please enter a shipping address]\">\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">Address2:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"ShippingAddress2\" size=\"34\" maxlength=\"100\" value=\"" + Server.HtmlEncode(ShippingAddress.Address2) + "\">");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">Suite:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"ShippingSuite\" size=\"34\" maxlength=\"50\" value=\"" + Server.HtmlEncode(ShippingAddress.Suite) + "\">");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">*City or APO/AFO:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"ShippingCity\" size=\"34\" maxlength=\"50\" value=\"" + Server.HtmlEncode(thisCustomer.ShippingCity) + "\">");
				writer.Write("        <input type=\"hidden\" name=\"ShippingCity_vldt\" value=\"[req][blankalert=Please enter a shipping city]\">\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">*State/Province:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("<select size=\"1\" name=\"ShippingState\">");
				writer.Write("<OPTION value=\"\"" + Common.IIF(ShippingAddress.State.Length==0 , " selected" , String.Empty) + ">SELECT ONE</option>");
				DataSet dsstate = DB.GetDS("select * from state  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddMinutes(Common.CacheDurationMinutes()));
				foreach(DataRow row in dsstate.Tables[0].Rows)
				{
					writer.Write("<OPTION value=\"" + DB.RowField(row,"Abbreviation") + "\"" + Common.SelectOption(thisCustomer.ShippingState,DB.RowField(row,"Abbreviation"),"shippingstate") + ">" + DB.RowField(row,"Name") + "</option>");
				}
				dsstate.Dispose();
				writer.Write("</select>");
				writer.Write("        </td>");
				writer.Write("      </tr>");

				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">*Zip:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"ShippingZip\" size=\"14\" maxlength=\"10\" value=\"" + thisCustomer.ShippingZip + "\">");
				writer.Write("        <input type=\"hidden\" name=\"ShippingZip_vldt\" value=\"[blankalert=Please enter the shipping zipcode][invalidalert=Please enter a valid zipcode]\">");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">*Country:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("<SELECT NAME=\"ShippingCountry\" size=\"1\">");
				writer.Write("<option value=\"0\">SELECT ONE</option>");
				DataSet dscountry = DB.GetDS("select * from country  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddMinutes(Common.CacheDurationMinutes()));
				foreach(DataRow row in dscountry.Tables[0].Rows)
				{
					writer.Write("<OPTION value=\"" + DB.RowField(row,"Name") + "\"" + Common.SelectOption(thisCustomer.ShippingCountry,DB.RowField(row,"Name"),"ShippingCountry") + ">" + DB.RowField(row,"Name") + "</option>");
				}
				dscountry.Dispose();
				writer.Write("</SELECT>");
				writer.Write("        </td>");
				writer.Write("      </tr>");


				writer.Write("</table>\n");

				writer.Write("</td></tr>\n");
				writer.Write("</table>\n");
				writer.Write("</td></tr>\n");
				writer.Write("</table>\n");
			}

			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");
			
			writer.Write("<p align=\"center\"><input type=\"submit\" value=\"" + Common.IIF(DoingCheckout, "Create Account & Continue Checkout", "Create Account") + "\" name=\"Continue\"></p>");
			
			writer.Write("</form>");
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
