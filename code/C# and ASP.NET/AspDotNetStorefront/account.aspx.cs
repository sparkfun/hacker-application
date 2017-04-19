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
using System.Data.SqlTypes;
using System.Data.SqlClient;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for account.
	/// </summary>
	public class account : SkinBase
	{

		bool EMailDup;
		bool AccountUpdated;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			RequireSecurePage();
			RequiresLogin(Common.GetThisPageName(false) + "?" + Common.ServerVariables("QUERY_STRING"));

			EMailDup = false;
			AccountUpdated = false;

			if(Common.Form("IsSubmitAccount") == "true")
			{
				String EMailField = Common.Form("EMail").ToLower();
				bool EMailAlreadyTaken = false;
				if(!Common.AppConfigBool("AllowCustomerDuplicateEMailAddresses"))
				{
					int NN = DB.GetSqlN("select count(*) as N from customer  " + DB.GetNoLock() + " where EMail=" + DB.SQuote(EMailField) + " and CustomerID<>" + Common.SessionUSInt("CustomerID").ToString());
					if(NN > 0)
					{
						EMailAlreadyTaken = true;
					}
				}

				if(EMailAlreadyTaken)
				{
					EMailField = Common.Form("OriginalEMail").ToLower(); // reset their e-mail, but then update their account with other changes below
				}

				StringBuilder sql = new StringBuilder(10000);
				sql.Append("update customer set ");
				sql.Append("FirstName=" + DB.SQuote(Common.Form("FirstName")) + ",");
				sql.Append("LastName=" + DB.SQuote(Common.Form("LastName")) + ",");
				sql.Append("EMail=" + DB.SQuote(EMailField )+ ",");
				sql.Append("Phone=" + DB.SQuote(Common.Form("Phone")) + ",");
				sql.Append("[Password]=" + DB.SQuote(Common.MungeString(Common.Form("Password"))) + ",");
				sql.Append("OkToEmail=" + Common.FormNativeInt("OkToEmail").ToString());
				sql.Append(" where customerid=" + thisCustomer._customerID.ToString());
				DB.ExecuteSQL(sql.ToString());

				thisCustomer._firstName = Common.Form("FirstName");
				thisCustomer._lastName = Common.Form("LastName");
				thisCustomer._email = EMailField;
				thisCustomer._phone = Common.Form("Phone");
				thisCustomer._password = Common.Form("Password");
				thisCustomer._okToEMail = (Common.FormNativeInt("OkToEmail") == 1);

				AccountUpdated = true;
				EMailDup = EMailAlreadyTaken;
			}	
		
			//If there is a deleteid remove it from the cart
			int DeleteID = Common.QueryStringUSInt("deleteID");
			if (DeleteID != 0)
			{
				DB.ExecuteSQL(String.Format("delete from customcart where  customerid={0} and ShoppingCartRecID={1}",thisCustomer._customerID,DeleteID));
				DB.ExecuteSQL(String.Format("delete from kitcart where  customerid={0} and ShoppingCartRecID={1}",thisCustomer._customerID,DeleteID));
				DB.ExecuteSQL(String.Format("delete from ShoppingCart where  customerid={0} and ShoppingCartRecID={1}",thisCustomer._customerID,DeleteID));
			}
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(Common.QueryString("unknownerror").Length > 0)
			{
				writer.Write("<p align=\"left\"><b><font color=\"#FF0000\">There was an error saving your account profile. Please try again.</font></b></p>");
			}
			if(Common.QueryString("errormsg").Length > 0)
			{
				writer.Write("<p align=\"left\"><b><font color=\"red\">" + Server.HtmlEncode(Common.QueryString("ErrorMsg")) + "</font></b></p>");
			}

			if(AccountUpdated)
			{
				if(!EMailDup)
				{
					writer.Write("<p align=\"left\"><font color=\"#0000FF\"><b>Your account has been updated.</b></font></p>");
				}
				else
				{
					writer.Write("<p align=\"left\"><font color=\"#0000FF\"><b>Your account has been updated, but <font color=\"#FF0000\">your e-mail address was not changed</font>...there is already another registered user with that e-mail address.</b></font></p>");
				}
			}

			writer.Write("<p align=\"left\" ><b><a href=\"#OrderHistory\">View your order/billing history</a></b></p>");

			if(thisCustomer._subscriptionExpiresOn > System.DateTime.Now)
			{
				writer.Write("<p align=\"left\" ><b>You have a current site subscription. Your subscription expires on: " + Localization.ToNativeShortDateString(thisCustomer._subscriptionExpiresOn) + "</b></p>");
			}
			
			Topic hdr = new Topic("AccountPageHeader",thisCustomer._localeSetting,_siteID);
			writer.Write(hdr._contents);

			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function AccountForm_Validator(theForm)\n");
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
			writer.Write("  return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");
			
			writer.Write("<form method=\"POST\" action=\"account.aspx\" name=\"AccountForm\" id=\"AccountForm\" onSubmit=\"return (validateForm(this) && AccountForm_Validator(this)" + Common.IIF(thisCustomer._isAnon , " && AddressInputForm_Validator(this)" , "") + ")\">");
			writer.Write("<input type=\"hidden\" name=\"IsSubmitAccount\" value=\"true\">");
			writer.Write("<input type=\"hidden\" name=\"OriginalEMail\" value=\"" + thisCustomer._email + "\">");
			
			if(thisCustomer._customerLevelID != 0)
			{
				writer.Write("<p align=\"left\">NOTE: You are a member of the <b>" + Common.GetCustomerLevelName(thisCustomer._customerLevelID) + "</b> group.</p>\n");
			}
			if(Common.AppConfigBool("MicroPay.Enabled"))
			{
				if ((! thisCustomer._isAnon) && (Common.GetMicroPayProductID()!= 0))
				{
					writer.Write("<p align=\"left\">Your " + Common.AppConfig("Micropay.Prompt") + " Balance is: <b>" + Localization.CurrencyStringForDisplay(Common.GetMicroPayBalance(thisCustomer._customerID)) + "</b></p>");
				}
			}

			// ACCOUNT BOX:
			writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/accountinfo.gif\" border=\"0\"><br>");
			writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

			writer.Write("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
			writer.Write("      <tr>");
			writer.Write("        <td width=\"100%\" colspan=\"2\"><b>Your account &amp; information is used to login to the site. Please save your password in a safe place.</b></td>");
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
			writer.Write("        <input type=\"text\" name=\"EMail\" size=\"37\" maxlength=\"100\" value=\"" + Server.HtmlEncode(thisCustomer._email) + "\">");
			writer.Write("        <input type=\"hidden\" name=\"EMail_vldt\" value=\"[req][blankalert=Please enter your e-mail address][invalidalert=Please enter a valid e-mail address]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">*Create a Personal Password:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"password\" name=\"Password\" size=\"20\" maxlength=\"50\" value=\"" + thisCustomer._password + "\"> (at least 4 chars long)");
			writer.Write("        <input type=\"hidden\" name=\"Password_vldt\" value=\"[req][len=4][blankalert=Please enter a password so you can login to this site at a later time]\">\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">*Re-Enter Your Password:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"password\" name=\"Password2\" size=\"20\" maxlength=\"50\" value=\"" + thisCustomer._password + "\">");
			writer.Write("        <input type=\"hidden\" name=\"Password2_vldt\" value=\"[req][len=4][blankalert=Please re-enter your password ]\">\n");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">Phone:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("        <input type=\"text\" id=\"Phone\" name=\"Phone\" size=\"14\" maxlength=\"20\" value=\"" + Server.HtmlEncode(thisCustomer._phone) + "\">");
			writer.Write("        <input type=\"hidden\" name=\"Phone_vldt\" value=\"[blankalert=Please enter your phone number][invalidalert=Please enter a valid phone number]\">");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("      <tr>");
			writer.Write("        <td width=\"25%\">*OK To EMail:</td>");
			writer.Write("        <td width=\"75%\">");
			writer.Write("		Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"OkToEmail\" value=\"1\" " + Common.IIF(thisCustomer._okToEMail , " checked " , "") + ">\n");
			writer.Write("		No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"OkToEmail\" value=\"0\" " + Common.IIF(thisCustomer._okToEMail , "" , " checked ") + ">\n");
			writer.Write("<small>(Can we contact you with product updates/information, etc.)</small>");
			writer.Write("        </td>");
			writer.Write("      </tr>");

			writer.Write("</table>\n");

			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");

			writer.Write("<p align=\"center\"><input type=\"submit\" value=\"Update Account\" name=\"Continue\"></p>");
			writer.Write("</form>");

			// ADDRESS BOX:
			
			Address BillingAddress = new Address(); 
			Address ShippingAddress = new Address(); 

			BillingAddress.LoadByCustomer(thisCustomer._customerID,AddressTypes.Billing);
			ShippingAddress.LoadByCustomer(thisCustomer._customerID,AddressTypes.Shipping);
			
			writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/addressbook.gif\" border=\"0\"><br>");
			writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

			writer.Write("<table width=\"100%\" border=\"0\">\n");
			writer.Write("<tr>\n");
			writer.Write("<td colspan=\"3\"><b>Your currently active billing and shipping addresses are shown below:</b><br><hr></td>");
			writer.Write("</tr>\n");
			writer.Write("<tr>\n");
			writer.Write("<td  valign=\"top\" width=\"50%\"><b>");

			writer.Write("Bill To Address:&nbsp;&nbsp;&nbsp;&nbsp;</b><img style=\"cursor:hand;\" src=\"skins/skin_" + _siteID.ToString() +"/images/change.gif\" border=\"0\" align=\"absmiddle\" onClick=\"self.location='selectaddress.aspx?AddressType=billing&returnURL=account.aspx'\"><br>");
			writer.Write(BillingAddress.DisplayHTML());
			if (BillingAddress.PaymentMethod.Length != 0)
			{
				writer.Write("<b>Payment Method:</b><br>");
				writer.Write(BillingAddress.DisplayPaymentMethod);
			}
			writer.Write("</td>");

			writer.Write("<td valign=\"top\">");
			writer.Write("<b>Ship To Address:&nbsp;&nbsp;&nbsp;&nbsp;</b><img style=\"cursor:hand;\" src=\"skins/skin_" + _siteID.ToString() +"/images/change.gif\" border=\"0\" align=\"absmiddle\" onClick=\"self.location='selectaddress.aspx?AddressType=shipping&returnURL=account.aspx'\"><br>\n");
			writer.Write(ShippingAddress.DisplayHTML());
			if (ShippingAddress.ShippingMethod.Length != 0)
			{
				writer.Write("<b>Shipping Method:</b><br>");
				writer.Write(ShippingAddress.ShippingMethod.Split('|')[0]);
			}
			writer.Write("</td>");
			writer.Write("</tr></table>");
			writer.Write("</table>\n");

			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");
			writer.Write("</form>");

			// ORDER HISTORY:
			writer.Write("<p align=\"left\" ><b><a name=\"OrderHistory\"></a>ORDER/BILLING HISTORY</b></p>");
			writer.Write("<form method=\"POST\" action=\"account.aspx\" name=\"HistoryForm\" id=\"HistoryForm\" >");
			writer.Write("<input type=\"hidden\" name=\"IsSubmitRecurring\" value=\"true\">");
			if(ShoppingCart.NumItems(thisCustomer._customerID,CartTypeEnum.RecurringCart) != 0)
			{
				writer.Write("<p align=\"left\"><b>You have active recurring (auto-ship) orders:</b></p>\n");
				IDataReader rsr = DB.GetRS("Select distinct OriginalRecurringOrderNumber from ShoppingCart  " + DB.GetNoLock() + " where CartType=" + ((int)CartTypeEnum.RecurringCart).ToString() + " and CustomerID=" + thisCustomer._customerID.ToString() + " order by OriginalRecurringOrderNumber desc");
				while(rsr.Read())
				{
					writer.Write(Common.GetRecurringCart(thisCustomer,DB.RSFieldInt(rsr,"OriginalRecurringOrderNumber"),_siteID,false));
				}
				rsr.Close();
				writer.Write("<br><br>");
			}

			writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			writer.Write("<a href=\"news.aspx\"><img src=\"skins/Skin_" + _siteID.ToString() + "/images/orderhistory.gif\" border=\"0\"></a><br>");
			writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			
			int N = 0;
			IDataReader rs = DB.GetRS("Select * from orders  " + DB.GetNoLock() + " where CustomerID=" + thisCustomer._customerID.ToString() + " order by OrderDate desc");
			writer.Write("<table align=\"center\" width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"1\">\n");
			writer.Write("<tr bgcolor=\"CCCCCC\">\n");
			writer.Write("<td valign=\"top\"><b>Order Number</b><br><small>(Click For Receipt)</small></td>\n");
			writer.Write("<td valign=\"top\"><b>Order Date</b></td>\n");
			writer.Write("<td valign=\"top\"><b>Payment Status</b></td>\n");
			writer.Write("<td valign=\"top\"><b>Shipping Status</b></td>\n");
			//writer.Write("<td valign=\"top\"><b>Customer ID</b></td>\n");
			//writer.Write("<td valign=\"top\"><b>Customer Name</b></td>\n");
			//writer.Write("<td valign=\"top\"><b>Customer E-Mail</b></td>\n");
			//writer.Write("<td valign=\"top\"><b>Company</b></td>\n");
			writer.Write("<td valign=\"top\"><b>Order Total</b></td>\n");
			if(Common.AppConfigBool("ShowCustomerServiceNotesInReceipts"))
			{
				writer.Write("<td valign=\"top\"><b>Customer Service Notes</b></td>\n");
			}
			writer.Write("</tr>\n");
			while(rs.Read())
			{
				String PaymentStatus = String.Empty;
				if(DB.RSField(rs,"PaymentMethod").Length != 0)
				{
					PaymentStatus = "Payment Method: " + DB.RSField(rs,"PaymentMethod") + "<br>";
				}
				else
				{
					PaymentStatus = "Payment Method: " + Common.IIF(DB.RSField(rs,"CardNumber").ToUpper() == "PAYPAL" , "PayPal" , "Credit Card") + "<br>";
				}
				if(DB.RSFieldDateTime(rs,"PaymentClearedOn") != System.DateTime.MinValue)
				{
					PaymentStatus += " Cleared On: " + Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs,"PaymentClearedOn"));
				}
				else
				{
					PaymentStatus += " Pending";
				}

				String ShippingStatus = String.Empty;
				if(Common.OrderHasShippableComponents(DB.RSFieldInt(rs,"OrderNumber")))
				{
					if(DB.RSFieldDateTime(rs,"ShippedOn") != System.DateTime.MinValue)
					{
						ShippingStatus = "Shipped";
						if(DB.RSField(rs,"ShippedVIA").Length != 0)
						{
							ShippingStatus += " via " + DB.RSField(rs,"ShippedVIA");
						}
						ShippingStatus += " on " + Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs,"ShippedOn")) + ".";
						if(DB.RSField(rs,"ShippingTrackingNumber").Length != 0)
						{
							ShippingStatus += " Tracking Number: " + DB.RSField(rs,"ShippingTrackingNumber");
						}
					}
					else
					{
						ShippingStatus = "Not Yet Shipped";
					}
				}
				if(Common.OrderHasDownloadComponents(DB.RSFieldInt(rs,"OrderNumber")))
				{
					if(DB.RSFieldDateTime(rs,"PaymentClearedOn") != System.DateTime.MinValue && DB.RSFieldDateTime(rs,"DownloadEMailSentOn") != System.DateTime.MinValue)
					{
						Order ord = new Order(DB.RSFieldInt(rs,"OrderNumber"));
						if(ShippingStatus.Length != 0)
						{
							ShippingStatus += "<hr size=1>";
						}
						ShippingStatus += ord.GetDownloadList(false);
						ord = null;
					}
					else
					{
						if(ShippingStatus.Length == 0)
						{
							ShippingStatus += "Download List Pending Payment";
						}
					}
				}
				writer.Write("<tr>\n");
				writer.Write("<td valign=\"top\"><a href=\"" + Common.GetStoreHTTPLocation(true) + "receipt.aspx?ordernumber=" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "&customerid=" + DB.RSFieldInt(rs,"CustomerID").ToString() + "\" target=\"_blank\">" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "</a></td>");
				writer.Write("<td valign=\"top\">" + Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs,"OrderDate")));
				writer.Write("</td>");
				writer.Write("<td valign=\"top\">" + PaymentStatus + "&nbsp;" + "</td>");
				writer.Write("<td valign=\"top\">" + ShippingStatus + "&nbsp;" + "</td>");
				writer.Write("<td valign=\"top\">" + Common.IIF(DB.RSFieldBool(rs,"QuoteCheckout") || DB.RSField(rs,"PaymentMethod").Replace(" ","").ToUpper() == "REQUESTQUOTE" , "REQUEST FOR QUOTE" , Localization.CurrencyStringForDisplay(DB.RSFieldDecimal(rs,"OrderTotal"))) + "</td>");
				if(Common.AppConfigBool("ShowCustomerServiceNotesInReceipts"))
				{
					writer.Write("<td valign=\"top\">" + Common.IIF(DB.RSField(rs,"CustomerServiceNotes").Length == 0 , "None" , DB.RSField(rs,"CustomerServiceNotes")) + "</td>");
				}
				writer.Write("</tr>\n");
				N++;
			}
			writer.Write("</table>\n");
			rs.Close();
			if(N == 0)
			{
				writer.Write("<p align=\"left\">No orders found</p>\n");
			}	
			
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");
			writer.Write("</form>\n");

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
