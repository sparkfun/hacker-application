// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Xml;
using System.Configuration;
using System.Web.SessionState;
using System.Web.Caching;
using System.Web.Handlers;
using System.Web.Hosting;
using System.Web.Mail;
using System.Web.Security;
using System.Web.UI;
using System.Web.Util;
using System.Data;
using System.Globalization;
using System.Text;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for orders.
	/// </summary>
	public class orders : SkinBase
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Order Summary";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(DB.GetSqlN("select count(*) as N from orders " + DB.GetNoLock()) == 0)
			{
				writer.Write("<h3>There are no orders in the database</h3>");
			}
			else
			{
				String StartDate = Common.QueryString("StartDate");
				String EndDate = Common.QueryString("EndDate");
				String AffiliateID = Common.QueryString("AffiliateID");
				String CouponCode = Common.QueryString("CouponCode");
				String IsNew = Common.QueryString("IsNew");
				if(Common.QueryString("OrderNumber").Length != 0)
				{
					IsNew = String.Empty; // make sure the order can be found!
				}
				String EMail = Common.QueryString("Email");
				String CreditCard = Common.QueryString("CreditCard");
				String Phone = Common.QueryString("Phone");
				String CustomerName = Common.QueryString("CustomerName");
				String Company = Common.QueryString("Company");
				String PaymentMethod = Common.QueryString("PaymentMethod");
				String OrderNumber = Common.QueryString("OrderNumber");
				String CustomerID = Common.QueryString("CustomerID");
				String ShippingState = Common.QueryString("ShippingState");
				String EasyRange = Common.QueryString("EasyRange");
				String Day = Common.QueryString("Day");
				String Month = Common.QueryString("Month");
				String Year = Common.QueryString("Year");

				if(EasyRange.Length == 0)
				{
					EasyRange = "Today";
				}

				IDataReader rsd = DB.GetRS("Select min(OrderDate) as MinDate from orders " + DB.GetNoLock());
				DateTime MinOrderDate = Localization.ParseUSDateTime("1/1/1990");
				if(rsd.Read())
				{
					MinOrderDate = DB.RSFieldDateTime(rsd,"MinDate");
				}
				rsd.Close();

				// reset date range here, to ensure new orders are visible:
				if(IsNew.Length != 0 && IsNew == "1")
				{
					EasyRange = "UseDatesAbove";
					StartDate = Localization.ToNativeShortDateString(MinOrderDate);
					EndDate = Localization.ToNativeShortDateString(System.DateTime.Now.AddDays(1));
				}

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

				writer.Write("<form method=\"GET\" action=\"orders.aspx\" id=\"ReportForm\" name=\"ReportForm\" >");
				writer.Write("<input type=\"hidden\" name=\"OrderNumberHidden\" value=\"\">\n");
				writer.Write("<input type=\"hidden\" name=\"SubmitAction\" value=\"\">\n");
				writer.Write("  <table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" width=\"100%\">");
				writer.Write("    <tr>");
				writer.Write("      <td width=\"25%\" align=\"center\" bgcolor=\"#EAEAEA\"><b><font color=\"#000000\">Date Range:</font></b></td>");
				writer.Write("      <td width=\"25%\" align=\"center\" bgcolor=\"#EAEAEA\"><b><font color=\"#000000\">Order Qualifiers:</font></b></td>");
				writer.Write("    </tr>");
				writer.Write("    <tr>");
				writer.Write("      <td width=\"25%\" valign=\"top\" align=\"left\" bgcolor=\"#FFFFCC\">");
				writer.Write("          <table border=\"0\" cellpadding=\"4\" cellspacing=\"0\" width=\"100%\">");
				writer.Write("            <tr>");
				writer.Write("              <td width=\"50%\">Start Date:</td>");
				writer.Write("              <td width=\"50%\"><input type=\"text\" name=\"StartDate\" size=\"11\" value=\"" + StartDate + "\">&nbsp;<img src=\"skins/skin_" + _siteID.ToString() + "/images/calendar.gif\" style=\"cursor:hand;\" align=\"absmiddle\" id=\"f_trigger_s\">");
				writer.Write("                	<input type=\"hidden\" name=\"StartDate_vldt\" value=\"[date][invalidalert=Please enter a valid starting date in the format " + Localization.ShortDateFormat() + "]\">");
				writer.Write("</td>");
				writer.Write("            </tr>");
				writer.Write("            <tr>");
				writer.Write("              <td width=\"50%\">End Date:</td>");
				writer.Write("              <td width=\"50%\"><input type=\"text\" name=\"EndDate\" size=\"11\" value=\"" + EndDate + "\">&nbsp;<img src=\"skins/skin_" + _siteID.ToString() + "/images/calendar.gif\" style=\"cursor:hand;\" align=\"absmiddle\" id=\"f_trigger_e\">");
				writer.Write("                	<input type=\"hidden\" name=\"EndDate_vldt\" value=\"[date][invalidalert=Please enter a valid ending date in the format " + Localization.ShortDateFormat() + "]\">");
				writer.Write("              </td>");
				writer.Write("            </tr>");
				writer.Write("          </table>");
				writer.Write("          <hr size=\"1\">");
				writer.Write("          <table border=\"0\" cellpadding=\"4\" cellspacing=\"0\" width=\"100%\">");
				writer.Write("            <tr>");
				writer.Write("              <td colspan=\"2\" align=\"center\" width=\"100%\"><input type=\"radio\" value=\"UseDatesAbove\" name=\"EasyRange\" " + Common.IIF(EasyRange == "UseDatesAbove" , "checked" , "") + ">Use Dates Above</td>");
				writer.Write("            </tr>");
				writer.Write("            <tr>");
				writer.Write("              <td width=\"50%\"><input type=\"radio\" value=\"Today\" name=\"EasyRange\" " + Common.IIF(EasyRange == "Today" || EasyRange == "" , "checked" , "") + ">Today</td>");
				writer.Write("              <td width=\"50%\"><input type=\"radio\" value=\"Yesterday\" name=\"EasyRange\" " + Common.IIF(EasyRange == "Yesterday" , "checked" , "") + ">Yesterday</td>");
				writer.Write("            </tr>");
				writer.Write("            <tr>");
				writer.Write("              <td width=\"50%\"><input type=\"radio\" value=\"ThisWeek\" name=\"EasyRange\" " + Common.IIF(EasyRange == "ThisWeek" , "checked" , "") + ">This Week</td>");
				writer.Write("              <td width=\"50%\"><input type=\"radio\" value=\"LastWeek\" name=\"EasyRange\" " + Common.IIF(EasyRange == "LastWeek" , "checked" , "") + ">Last Week</td>");
				writer.Write("            </tr>");
				writer.Write("            <tr>");
				writer.Write("              <td width=\"50%\"><input type=\"radio\" value=\"ThisMonth\" name=\"EasyRange\" " + Common.IIF(EasyRange == "ThisMonth" , "checked" , "") + ">This Month</td>");
				writer.Write("              <td width=\"50%\"><input type=\"radio\" value=\"LastMonth\" name=\"EasyRange\" " + Common.IIF(EasyRange == "LastMonth" , "checked" , "") + ">Last Month</td>");
				writer.Write("            </tr>");
				writer.Write("            <tr>");
				writer.Write("              <td width=\"50%\"><input type=\"radio\" value=\"ThisYear\" name=\"EasyRange\" " + Common.IIF(EasyRange == "ThisYear" , "checked" , "") + ">This Year</td>");
				writer.Write("              <td width=\"50%\"><input type=\"radio\" value=\"LastYear\" name=\"EasyRange\" " + Common.IIF(EasyRange == "LastYear" , "checked" , "") + ">Last Year</td>");
				writer.Write("            </tr>");
				writer.Write("          </table>");
				writer.Write("      </td>");
				writer.Write("      <td width=\"25%\" valign=\"top\" align=\"left\" bgcolor=\"#CCFFFF\">");
				writer.Write("        <table border=\"0\" cellpadding=\"4\" cellspacing=\"0\" width=\"100%\">");

				writer.Write("          <tr>");
				writer.Write("            <td width=\"50%\">Order Number:</td>");
				writer.Write("            <td width=\"50%\"><input type=\"text\" name=\"OrderNumber\" value=\"" + OrderNumber + "\"></td>");
				writer.Write("          </tr>");
			
				writer.Write("          <tr>");
				writer.Write("            <td width=\"50%\">Customer ID:</td>");
				writer.Write("            <td width=\"50%\"><input type=\"text\" name=\"CustomerID\" value=\"" + CustomerID.ToString() + "\"></td>");
				writer.Write("          </tr>");
			
				writer.Write("          <tr>");
				writer.Write("            <td width=\"50%\">Customer E-Mail:</td>");
				writer.Write("            <td width=\"50%\"><input type=\"text\" name=\"EMail\" value=\"" + EMail + "\"></td>");
				writer.Write("          </tr>");
			
				writer.Write("          <tr>");
				writer.Write("            <td width=\"50%\">Credit Card #:</td>");
				writer.Write("            <td width=\"50%\"><input type=\"text\" name=\"Card\" value=\"" + CreditCard + "\"></td>");
				writer.Write("          </tr>");
			
				writer.Write("          <tr>");
				writer.Write("            <td width=\"50%\">Customer Phone #:</td>");
				writer.Write("            <td width=\"50%\"><input type=\"text\" name=\"Phone\" value=\"" + Phone + "\"></td>");
				writer.Write("          </tr>");
			
				writer.Write("          <tr>");
				writer.Write("            <td width=\"50%\">Customer Name:</td>");
				writer.Write("            <td width=\"50%\"><input type=\"text\" name=\"CustomerName\" value=\"" + CustomerName + "\"></td>");
				writer.Write("          </tr>");
			
				writer.Write("          <tr>");
				writer.Write("            <td width=\"50%\">Company:</td>");
				writer.Write("            <td width=\"50%\"><input type=\"text\" name=\"Company\" value=\"" + Company + "\"></td>");
				writer.Write("          </tr>");
			
				writer.Write("          <tr>");
				writer.Write("            <td width=\"50%\">Payment Method:</td>");
				writer.Write("            <td width=\"50%\"><select size=\"1\" name=\"PaymentMethod\">");
				writer.Write("                <option value=\"-\"" + Common.IIF(PaymentMethod == "-" || PaymentMethod.Length == 0 , "selected" , "") + ">All Types</option>");
				writer.Write("                <option value=\"CREDIT CARD\"" + Common.IIF(PaymentMethod.ToUpper() == "CREDIT CARD" , "selected" , "") + ">Credit Card</option>");
				writer.Write("                <option value=\"PAYPAL\"" + Common.IIF(PaymentMethod.ToUpper() == "PAYPAL" , "selected" , "") + ">PayPal</option>");
				writer.Write("                <option value=\"PURCHASE ORDER\"" + Common.IIF(PaymentMethod.ToUpper() == "PURCHASE ORDER" , "selected" , "") + ">Purchase Order</option>");
				writer.Write("                <option value=\"REQUEST QUOTE\"" + Common.IIF(PaymentMethod.ToUpper() == "REQUEST QUOTE" , "selected" , "") + ">Request Quote</option>");
				writer.Write("                <option value=\"CHECK\"" + Common.IIF(PaymentMethod.ToUpper() == "CHECK" , "selected" , "") + ">Check</option>");
				writer.Write("                <option value=\"ECHECK\"" + Common.IIF(PaymentMethod.ToUpper() == "ECHECK" , "selected" , "") + ">eCheck</option>");
				writer.Write("                <option value=\"MICROPAY\"" + Common.IIF(PaymentMethod.ToUpper() == "MICROPAY" , "selected" , "") + ">MicroPay</option>");
				writer.Write("              </select></td>");
				writer.Write("          </tr>");

				writer.Write("          <tr>");
				writer.Write("            <td width=\"50%\">New Orders Only:</td>");
				writer.Write("            <td width=\"50%\">");
				writer.Write("                <input type=\"radio\" name=\"IsNew\" value=\"0\"" + Common.IIF(IsNew == "0" , " checked " , "") + ">No&nbsp;&nbsp;&nbsp;&nbsp;");
				writer.Write("                <input type=\"radio\" name=\"IsNew\" value=\"1\"" + Common.IIF(IsNew == "1" || IsNew.Length == 0, " checked " , "") + ">Yes");
				writer.Write("              </td>");
				writer.Write("          </tr>");

				writer.Write("          <tr>");
				writer.Write("            <td width=\"50%\">Affiliate:</td>");
				writer.Write("            <td width=\"50%\"><select size=\"1\" name=\"AffiliateID\">");
				writer.Write("                  <option value=\"-\" " + Common.IIF(AffiliateID == "" || AffiliateID == "-" , "selected" , "") + ">-</option>");
				IDataReader rs = DB.GetRS("select * from affiliate  " + DB.GetNoLock() + " order by name");
				while(rs.Read())
				{
					writer.Write("<option value=\"" + DB.RSFieldInt(rs,"AffiliateID").ToString() + "\"" + Common.IIF(AffiliateID == DB.RSFieldInt(rs,"AffiliateID").ToString() , "selected" , "") + ">" + DB.RSField(rs,"Name") + "</option>");
				}
				rs.Close();
				writer.Write("              </select></td>");
				writer.Write("          </tr>");
				writer.Write("          <tr>");
				writer.Write("            <td width=\"50%\">Coupon Code:</td>");
				writer.Write("            <td width=\"50%\"><select size=\"1\" name=\"CouponCode\">");
				writer.Write("                  <option value=\"-\" " + Common.IIF(CouponCode == "" || CouponCode == "-" , "selected" , "") + ">-</option>");
				rs = DB.GetRS("select * from Coupon  " + DB.GetNoLock() + " order by CouponCode");
				while(rs.Read())
				{
					writer.Write("<option value=\"" + DB.RSField(rs,"CouponCode") + "\"" + Common.IIF(CouponCode == DB.RSField(rs,"CouponCode") , "selected" , "") + ">" + DB.RSField(rs,"CouponCode") + "</option>");
				}
				rs.Close();
				writer.Write("              </select></td>");
				writer.Write("          </tr>");



				writer.Write("          <tr>");
				writer.Write("            <td width=\"50%\">Ship To State:</td>");
				writer.Write("            <td width=\"50%\">");
				writer.Write("<select size=\"1\" name=\"ShippingState\">");
				writer.Write("<OPTION value=\"\"" + Common.IIF(ShippingState.Length==0 , " selected" , String.Empty) + ">SELECT ONE</option>");
				DataSet dsstate = DB.GetDS("select * from state  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddHours(1));
				foreach(DataRow row in dsstate.Tables[0].Rows)
				{
					writer.Write("<OPTION value=\"" + DB.RowField(row,"Abbreviation") + "\"" + Common.IIF(ShippingState == DB.RowField(row,"Abbreviation") , "selected" , "") + ">" + DB.RowField(row,"Name") + "</option>");
				}
				dsstate.Dispose();
				writer.Write("</select>");

				writer.Write("              </td>");
				writer.Write("          </tr>");

				writer.Write("        </table>");
				writer.Write("        </td>");
				writer.Write("    </tr>");
				writer.Write("    <tr>");
				writer.Write("      <td width=\"100%\" valign=\"top\" align=\"center\" bgcolor=\"#EAEAEA\" colspan=\"4\">");
				writer.Write("        <input type=\"submit\" value=\"Submit\" name=\"submit2\">&nbsp;&nbsp;<input type=\"button\" onClick=\"javascript:self.location='orders.aspx?isnew=" + IsNew + "';\" value=\"Reset\" name=\"B2\">");
				writer.Write("      </td>");
				writer.Write("    </tr>");
				writer.Write("  </table>");

				writer.Write("\n<script type=\"text/javascript\">\n");
				writer.Write("    Calendar.setup({\n");
				writer.Write("        inputField     :    \"StartDate\",      // id of the input field\n");
				writer.Write("        ifFormat       :    \"" + Localization.JSCalendarDateFormatSpec() + "\",       // format of the input field\n");
				writer.Write("        showsTime      :    false,            // will display a time selector\n");
				writer.Write("        button         :    \"f_trigger_s\",   // trigger for the calendar (button ID)\n");
				writer.Write("        singleClick    :    true            // Single-click mode\n");
				writer.Write("    });\n");
				writer.Write("    Calendar.setup({\n");
				writer.Write("        inputField     :    \"EndDate\",      // id of the input field\n");
				writer.Write("        ifFormat       :    \"" + Localization.JSCalendarDateFormatSpec() + "\",       // format of the input field\n");
				writer.Write("        showsTime      :    false,            // will display a time selector\n");
				writer.Write("        button         :    \"f_trigger_e\",   // trigger for the calendar (button ID)\n");
				writer.Write("        singleClick    :    true            // Single-click mode\n");
				writer.Write("    });\n");
				writer.Write("</script>\n");

				DateTime RangeStartDate = System.DateTime.MinValue;
				DateTime RangeEndDate = System.DateTime.MaxValue;

				String DateWhere = String.Empty;
				switch(EasyRange)
				{
					case "UseDatesAbove":
						if(StartDate.Length != 0)
						{
							DateWhere = " OrderDate>=" + DB.DateQuote(StartDate + " 12:00:00.000 AM");
							RangeStartDate = Localization.ParseNativeDateTime(StartDate);
						}
						else
						{
							RangeStartDate = System.DateTime.MinValue; // will get min date returned from either query
						}
						if(EndDate.Length != 0)
						{
							DateWhere += Common.IIF(DateWhere.Length != 0 , " and " , "") + "OrderDate <=" + DB.DateQuote(EndDate + " 11:59:59.999 PM");
							RangeEndDate = Localization.ParseNativeDateTime(EndDate);
						}
						else
						{
							RangeEndDate = System.DateTime.Now;
						}
						break;
					case "UseDatesBelow":
						if(Day.Length != 0 && Day != "0")
						{
							DateWhere = " day(OrderDate)=" + Day + " ";
						}
						if(Month.Length != 0 && Month != "0")
						{
							if(DateWhere.Length != 0)
							{
								DateWhere += " and ";
							}
							DateWhere += " month(OrderDate)=" + Month + " ";
						}
						if(Year.Length != 0 && Year != "0")
						{
							if(DateWhere.Length != 0)
							{
								DateWhere += " and ";
							}
							DateWhere += " year(OrderDate)=" + Year + " ";
						};
						String DaySpec = Common.IIF(Day.Length == 0 || Day == "0" , "1" , Day);
						String MonthSpec = Common.IIF(Month.Length == 0 || Month == "0" , "1" , Month);
						String YearSpec = Common.IIF(Year.Length == 0 || Year == "0" , System.DateTime.Now.Year.ToString() , Year);
						RangeStartDate = Localization.ParseNativeDateTime(MonthSpec + "/" + DaySpec + "/" + YearSpec);
						RangeEndDate = RangeStartDate;
						break;
					case "Today":
						DateWhere = "day(OrderDate)=" + System.DateTime.Now.Day.ToString() + " and month(OrderDate)=" + System.DateTime.Now.Month.ToString() + " and year(OrderDate)=" + System.DateTime.Now.Year.ToString();
						RangeStartDate = System.DateTime.Now;
						RangeEndDate = System.DateTime.Now;
						break;
					case "Yesterday":
						DateWhere = "day(OrderDate)=" + System.DateTime.Now.AddDays(-1).Day.ToString() + " and month(OrderDate)=" + System.DateTime.Now.AddDays(-1).Month.ToString() + " and year(OrderDate)=" + System.DateTime.Now.AddDays(-1).Year.ToString();
						RangeStartDate = System.DateTime.Now.AddDays(-1);
						RangeEndDate = System.DateTime.Now.AddDays(-1);
						break;
					case "ThisWeek":
						int DayOfWeek = (int)System.DateTime.Now.DayOfWeek;
						System.DateTime weekstart = System.DateTime.Now.AddDays(-(DayOfWeek));
						System.DateTime weekend = weekstart.AddDays(6);
						int weekstartday = weekstart.DayOfYear;
						int weekendday = weekend.DayOfYear;
						DateWhere = "year(OrderDate)=" + System.DateTime.Now.Year.ToString() + " and (datepart(\"dy\",OrderDate)>=" + weekstartday.ToString() + " and datepart(\"dy\",OrderDate)<=" + weekendday.ToString() + ")";
						RangeStartDate = weekstart;
						RangeEndDate = weekend;
						break;
					case "LastWeek":
						int DayOfWeek2 = (int)System.DateTime.Now.DayOfWeek;
						System.DateTime weekstart2 = System.DateTime.Now.AddDays(-(DayOfWeek2)).AddDays(-7);
						System.DateTime weekend2 = weekstart2.AddDays(6);
						int weekstartday2 = weekstart2.DayOfYear;
						int weekendday2 = weekend2.DayOfYear;
						DateWhere = "year(OrderDate)=" + System.DateTime.Now.Year.ToString() + " and (datepart(\"dy\",OrderDate)>=" + weekstartday2.ToString() + " and datepart(\"dy\",OrderDate)<=" + weekendday2.ToString() + ")";
						RangeStartDate = weekstart2;
						RangeEndDate = weekend2;
						break;
					case "ThisMonth":
						DateWhere = "month(OrderDate)=" + System.DateTime.Now.Month.ToString() + " and year(OrderDate)=" + System.DateTime.Now.Year.ToString();
						RangeStartDate = Localization.ParseNativeDateTime(System.DateTime.Now.Month.ToString() + "/1/" + System.DateTime.Now.Year.ToString());
						RangeEndDate = RangeStartDate.AddMonths(1).AddDays(-1);
						break;
					case "LastMonth":
						DateWhere = "month(OrderDate)=" + System.DateTime.Now.AddMonths(-1).Month.ToString() + " and year(OrderDate)=" + System.DateTime.Now.AddMonths(-1).Year.ToString();
						RangeStartDate = Localization.ParseNativeDateTime(System.DateTime.Now.AddMonths(-1).Month.ToString() + "/1/" + System.DateTime.Now.AddMonths(-1).Year.ToString());
						RangeEndDate = RangeStartDate.AddMonths(1).AddDays(-1);
						break;
					case "ThisYear":
						DateWhere = "year(OrderDate)=" + System.DateTime.Now.Year.ToString();
						RangeStartDate = Localization.ParseUSDateTime("1/1/" + System.DateTime.Now.Year.ToString());
						RangeEndDate = RangeStartDate.AddYears(1).AddDays(-1);
						if(RangeEndDate > System.DateTime.Now)
						{
							RangeEndDate = System.DateTime.Now;
						}
						break;
					case "LastYear":
						DateWhere = "year(OrderDate)=" + System.DateTime.Now.AddYears(-1).Year.ToString();
						RangeStartDate = Localization.ParseUSDateTime("1/1/" + System.DateTime.Now.AddYears(-1).Year.ToString());
						RangeEndDate = RangeStartDate.AddYears(1).AddDays(-1);
						break;
				}
				if(DateWhere.Length != 0)
				{
					DateWhere = "(" + DateWhere + ")";
				}

				String WhereClause = DateWhere;

				String SelectFields = String.Empty;
				String OrderByFields = "IsNew desc, OrderDate desc";

				String GeneralWhere = String.Empty;
				if(AffiliateID != "-" && AffiliateID.Length != 0)
				{
					if(GeneralWhere.Length != 0)
					{
						GeneralWhere += " and ";
					}
					GeneralWhere += "AffiliateID=" + AffiliateID;
				}
				if(CouponCode != "-" && CouponCode.Length != 0)
				{
					if(GeneralWhere.Length != 0)
					{
						GeneralWhere += " and ";
					}
					GeneralWhere += "upper(CouponCode)=" + DB.SQuote(CouponCode.ToUpper());
				}
				if(ShippingState != "-" && ShippingState.Length != 0)
				{
					if(GeneralWhere.Length != 0)
					{
						GeneralWhere += " and ";
					}
					GeneralWhere += "upper(ShippingState)=" + DB.SQuote(ShippingState.ToUpper());
				}
				if(IsNew.Length != 0 && IsNew == "1")
				{
					if(GeneralWhere.Length != 0)
					{
						GeneralWhere += " and ";
					}
					GeneralWhere += "IsNew=1";
				}
				if(EMail.Length != 0)
				{
					if(GeneralWhere.Length != 0)
					{
						GeneralWhere += " and ";
					}
					GeneralWhere += "email like " + DB.SQuote("%" + EMail + "%");
				}
				if(CustomerID.Length != 0)
				{
					if(GeneralWhere.Length != 0)
					{
						GeneralWhere += " and ";
					}
					GeneralWhere += "CustomerID=" + CustomerID;
				}
				if(OrderNumber.Length != 0)
				{
					if(GeneralWhere.Length != 0)
					{
						GeneralWhere += " and ";
					}
					GeneralWhere += "OrderNumber=" + OrderNumber;
				}
				if(CreditCard.Length != 0)
				{
					if(GeneralWhere.Length != 0)
					{
						GeneralWhere += " and ";
					}
					GeneralWhere += "lower(CardNumber)=" + DB.SQuote(Encrypt.MungeString(CreditCard));
				}
				if(Phone.Length != 0)
				{
					if(GeneralWhere.Length != 0)
					{
						GeneralWhere += " and ";
					}
					GeneralWhere += "(Phone like " + DB.SQuote("%" + Phone + "%") + " or ShippingPhone like " + DB.SQuote("%" + Phone + "%") + " or BillingPhone like " + DB.SQuote("%" + Phone + "%") + ")";
				}
				if(CustomerName.Length != 0)
				{
					if(GeneralWhere.Length != 0)
					{
						GeneralWhere += " and ";
					}
					GeneralWhere += "(FirstName+' '+LastName) like " + DB.SQuote("%" + CustomerName + "%");
				}
				if(Company.Length != 0)
				{
					if(GeneralWhere.Length != 0)
					{
						GeneralWhere += " and ";
					}
					GeneralWhere += "(ShippingCompany like " + DB.SQuote("%" + Company + "%") + " or BillingCompany like " + DB.SQuote("%" + Company + "%") + ")";
				}
				if(PaymentMethod.Length != 0 && PaymentMethod != "-")
				{
					if(GeneralWhere.Length != 0)
					{
						GeneralWhere += " and ";
					}
					switch(PaymentMethod.Replace(" ","").ToUpper())
					{
						case "CREDITCARD":
							GeneralWhere += "(PaymentMethod=" + DB.SQuote(PaymentMethod) + " or (PaymentGateway IS NOT NULL and upper(PaymentGateway)<>'PAYPAL'))";
							break;
						case "PAYPAL":
							GeneralWhere += "(PaymentMethod=" + DB.SQuote(PaymentMethod) + " or upper(PaymentGateway)=" + DB.SQuote("PAYPAL") + ")";
							break;
						case "PURCHASEORDER":
							GeneralWhere += "PaymentMethod=" + DB.SQuote(PaymentMethod);
							break;
						case "REQUESTQUOTE":
							GeneralWhere += "(PaymentMethod=" + DB.SQuote(PaymentMethod) + " or QuoteCheckout<>0)";
							break;
						case "CHECK":
							GeneralWhere += "PaymentMethod=" + DB.SQuote(PaymentMethod);
							break;
						case "ECHECK":
							GeneralWhere += "PaymentMethod=" + DB.SQuote(PaymentMethod);
							break;
						case "MICROPAY":
							GeneralWhere += "PaymentMethod=" + DB.SQuote(PaymentMethod);
							break;
					}
				}
				if(GeneralWhere.Length != 0)
				{
					GeneralWhere = "(" + GeneralWhere + ")";
				}

				String DS1SQL = "select * from orders  " + DB.GetNoLock() + " where 1=1 " + Common.IIF(GeneralWhere.Length != 0 , " and " + GeneralWhere , "") + Common.IIF(WhereClause.Length != 0 , " and " + WhereClause , "") + " order by " + OrderByFields;

				if(Common.AppConfigBool("Admin_ShowReportSQL"))
				{
					writer.Write("<p align=\"left\">DS1SQL=" + DS1SQL + "</p>\n");
				}
				writer.Write("</form>\n");

				DataSet ds = DB.GetDS(DS1SQL,false);
				if(ds.Tables[0].Rows.Count == 0)
				{
					writer.Write("<p><b>NO ORDERS FOUND IN THAT RANGE</b></p>");
				}
				else
				{
					writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\n");
					writer.Write("<tr>\n");
					writer.Write("<td width=\"210\" valign=\"top\" align=\"left\">\n");

					writer.Write("<table width=\"171\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
					writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
					writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() +  "/images/orders.gif\" border=\"0\"></a><br>");
					writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
					writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
					int FirstOrderNumber = 0;
					foreach(DataRow row in ds.Tables[0].Rows)
					{
						if(FirstOrderNumber == 0)
						{
							FirstOrderNumber = DB.RowFieldInt(row,"OrderNumber");
						}
						writer.Write(Localization.ToNativeShortDateString(DB.RowFieldDateTime(row,"OrderDate")) + " <a href=\"orderframe.aspx?ordernumber=" + DB.RowFieldInt(row,"OrderNumber").ToString() + "\" target=\"orderframe\">" + DB.RowFieldInt(row,"OrderNumber").ToString() + "</a>");
						if(DB.RowFieldBool(row,"IsNew"))
						{
							writer.Write("&nbsp;<img src=\"skins/skin_" + _siteID.ToString() + "/images/new.gif\" align=\"absmiddle\" border=\"0\">");
						}
						writer.Write("<br>");
					}
					writer.Write("</td></tr>\n");
					writer.Write("</table>\n");
					writer.Write("</td></tr>\n");
					writer.Write("</table>\n");

					writer.Write("</td>\n");
					writer.Write("<td width=\"1%\" valign=\"top\" align=\"left\"></td>\n");
					writer.Write("<td width=\"*\" valign=\"top\" align=\"left\">");

					String DetailPage = "empty.htm";
					if(FirstOrderNumber != 0)
					{
						DetailPage = "orderframe.aspx?ordernumber=" + FirstOrderNumber.ToString();
					}
					writer.Write("<iframe height=\"1500\" id=\"orderframe\" name=\"orderframe\" src=\"" + DetailPage + "\" scrolling=\"no\" marginwidth=\"0\" marginheight=\"0\" frameborder=\"0\" vspace=\"0\" hspace=\"0\" style=\"width:100%; display:block; border-width: 1px; border-style: solid; border-color: #EEEEEE\"></iframe>\n");

					writer.Write("</td>\n");
					writer.Write("<td width=\"1%\" valign=\"top\" align=\"left\"></td>\n");
					writer.Write("</tr>\n");
					writer.Write("</table>\n");
				}
				ds.Dispose();
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
