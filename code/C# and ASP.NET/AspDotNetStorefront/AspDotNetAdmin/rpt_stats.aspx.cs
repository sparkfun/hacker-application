// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
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
	/// Summary description for rpt_stats.
	/// </summary>
	public class rpt_stats : SkinBase
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Reports - Summary Stats";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			String StartDate = Common.QueryString("StartDate");
			String EndDate = Common.QueryString("EndDate");
			String EasyRange = Common.QueryString("EasyRange");
			String Day = Common.QueryString("Day");
			String Month = Common.QueryString("Month");
			String Year = Common.QueryString("Year");
			String AffiliateID = Common.QueryString("AffiliateID");

			if(EasyRange.Length == 0)
			{
				EasyRange = "Today";
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

			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function ReportForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");

			writer.Write("<div align=\"left\">\n");
			writer.Write("<form method=\"GET\" action=\"rpt_stats.aspx\" id=\"ReportForm\" name=\"ReportForm\" onsubmit=\"return (validateForm(this) && ReportForm_Validator(this))\">");
			writer.Write("  <table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" width=\"570\">");
			writer.Write("    <tr>");
			writer.Write("      <td width=\"50%\" align=\"center\" bgcolor=\"#EAEAEA\"><b><font color=\"#000000\">Date Range:</font></b></td>");
			writer.Write("      <td width=\"50%\" align=\"center\" bgcolor=\"#EAEAEA\"><b><font color=\"#000000\">Qualifiers:</font></b></td>");
			writer.Write("    </tr>");
			writer.Write("    <tr>");
			writer.Write("      <td width=\"50%\" valign=\"top\" align=\"left\" bgcolor=\"#FFFFCC\">");
			writer.Write("          <table border=\"0\" cellpadding=\"4\" cellspacing=\"0\" width=\"100%\">");
			writer.Write("            <tr>");
			writer.Write("              <td width=\"50%\">Start Date:</td>");
			writer.Write("              <td width=\"50%\"><input type=\"text\" name=\"StartDate\" size=\"11\" value=\"" + StartDate + "\">&nbsp;<button id=\"f_trigger_s\">...</button>");
			writer.Write("                	<input type=\"hidden\" name=\"StartDate_vldt\" value=\"[date][invalidalert=Please enter a valid starting date in the format " + Localization.ShortDateFormat() + "]\">");
			writer.Write("</td>");
			writer.Write("            </tr>");
			writer.Write("            <tr>");
			writer.Write("              <td width=\"50%\">End Date:</td>");
			writer.Write("              <td width=\"50%\"><input type=\"text\" name=\"EndDate\" size=\"11\" value=\"" + EndDate + "\">&nbsp;<button id=\"f_trigger_e\">...</button>");
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
			writer.Write("      <td width=\"50%\" valign=\"top\" align=\"left\" bgcolor=\"#CCFFFF\">");
			writer.Write("        <table border=\"0\" cellpadding=\"4\" cellspacing=\"0\" width=\"100%\">");
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
			writer.Write("</table>");
			writer.Write("</td>");
			writer.Write("    </tr>");
			writer.Write("    <tr>");
			writer.Write("      <td width=\"100%\" valign=\"top\" align=\"center\" bgcolor=\"#EAEAEA\" colspan=\"4\">");
			writer.Write("        <input type=\"submit\" value=\"Submit\" name=\"B1\"><input type=\"button\" onClick=\"javascript:self.location='rpt_stats.aspx';\" value=\"Reset\" name=\"B2\">");
			writer.Write("      </td>");
			writer.Write("    </tr>");
			writer.Write("  </table>");
			writer.Write("</form>");

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
					else
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
					String YearSpec = Common.IIF(Year.Length == 0 || Year == "0" , System.DateTime.Now.Year.ToString() , Year.ToString());
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

			if(DateWhere.Length != 0)
			{
				if(AffiliateID == "-")
				{
					AffiliateID = String.Empty;
				}
				int NumCustomers = DB.GetSqlN("select count(CustomerID) as N from customer  " + DB.GetNoLock() + " where " + WhereClause.Replace("OrderDate","CreatedOn") + Common.IIF(AffiliateID.Length != 0 , " and affiliateid=" + AffiliateID , ""));

				int NumAnonCustomers = DB.GetSqlN("select count(CustomerID) as N from customer  " + DB.GetNoLock() + " where email like " + DB.SQuote("Anon_%") + " and " + WhereClause.Replace("OrderDate","CreatedOn") + Common.IIF(AffiliateID.Length != 0 , " and affiliateid=" + AffiliateID , ""));
				int NumRegCustomers = DB.GetSqlN("select count(CustomerID) as N from customer  " + DB.GetNoLock() + " where email not like " + DB.SQuote("Anon_%") + " and " + WhereClause.Replace("OrderDate","CreatedOn") + Common.IIF(AffiliateID.Length != 0 , " and affiliateid=" + AffiliateID , ""));
				int NumOrderCustomers = DB.GetSqlN("select count(CustomerID) as N from customer  " + DB.GetNoLock() + " where email not like " + DB.SQuote("Anon_%") + " and customerid in (select distinct customerid from orders " + DB.GetNoLock() + " ) and " + WhereClause.Replace("OrderDate","CreatedOn") + Common.IIF(AffiliateID.Length != 0 , " and affiliateid=" + AffiliateID , ""));
				int OrderN = DB.GetSqlN("select count(ordernumber) as N from orders  " + DB.GetNoLock() + " where " + WhereClause + Common.IIF(AffiliateID.Length != 0 , " and affiliateid=" + AffiliateID , ""));
				decimal OrderTotal = DB.GetSqlNDecimal("select sum(ordertotal) as N from orders  " + DB.GetNoLock() + " where " + WhereClause + Common.IIF(AffiliateID.Length != 0 , " and affiliateid=" + AffiliateID , ""));
				decimal OrderSubTotal = DB.GetSqlNDecimal("select sum(ordersubtotal) as N from orders  " + DB.GetNoLock() + " where " + WhereClause + Common.IIF(AffiliateID.Length != 0 , " and affiliateid=" + AffiliateID , ""));
				decimal OrderTax = DB.GetSqlNDecimal("select sum(ordertax) as N from orders  " + DB.GetNoLock() + " where " + WhereClause + Common.IIF(AffiliateID.Length != 0 , " and affiliateid=" + AffiliateID , ""));
				decimal OrderShipping = DB.GetSqlNDecimal("select sum(ordershippingcosts) as N from orders  " + DB.GetNoLock() + " where " + WhereClause + Common.IIF(AffiliateID.Length != 0 , " and affiliateid=" + AffiliateID , ""));
				decimal OrderAvg = DB.GetSqlNDecimal("select avg(ordertotal) as N from orders  " + DB.GetNoLock() + " where " + WhereClause + Common.IIF(AffiliateID.Length != 0 , " and affiliateid=" + AffiliateID , ""));
				decimal ExpenseTotal = DB.GetSqlNDecimal("select sum(amount) as N from expense  " + DB.GetNoLock() + " where " + WhereClause.Replace("OrderDate","ExpenseDate"));
				decimal NetPL = (OrderTotal - ExpenseTotal);
				decimal NetPL2 = (OrderSubTotal - ExpenseTotal);
				decimal RevenuePerVisitor = System.Decimal.Zero;
				if(NumCustomers > 0)
				{
					RevenuePerVisitor = OrderTotal / NumCustomers;
				}
				decimal RevenuePerRegisteredCustomer = System.Decimal.Zero;
				if(NumRegCustomers > 0)
				{
					RevenuePerRegisteredCustomer = (OrderTotal / NumRegCustomers);
				}
				decimal RevenuePerOrderingCustomer = System.Decimal.Zero;
				if(NumOrderCustomers > 0)
				{
					RevenuePerOrderingCustomer = (OrderTotal / NumOrderCustomers);
				}
				decimal NetRevenuePerVisitor = System.Decimal.Zero;
				if(NumCustomers > 0)
				{
					NetRevenuePerVisitor = (NetPL / NumCustomers);
				}
				decimal NetRevenuePerRegisteredCustomer = System.Decimal.Zero;
				if(NumRegCustomers > 0)
				{
					NetRevenuePerRegisteredCustomer = (NetPL / NumRegCustomers);
				}
				decimal NetRevenuePerOrderingCustomer = System.Decimal.Zero;
				if(NumOrderCustomers > 0)
				{
					NetRevenuePerOrderingCustomer = (NetPL / NumOrderCustomers);
				}

				writer.Write("<table width=\"50%\" cellpadding=\"4\" cellspacing=\"0\" border=\"1\">\n");
				writer.Write("<tr bgcolor=\"#CCCCCC\"><td align=\"left\"><b>Statistic</b></td><td align=\"center\"><b>Value</b></td></tr>\n");
				writer.Write("<tr><td align=\"left\"># of Visitors</td><td align=\"center\">" + NumCustomers.ToString() + "</td></tr>\n");
				writer.Write("<tr><td align=\"left\"># of Anon Visitors</td><td align=\"center\">" + NumAnonCustomers.ToString() + "</td></tr>\n");
				writer.Write("<tr><td align=\"left\"># of Registered Customers</td><td align=\"center\">" + NumRegCustomers.ToString() + "</td></tr>\n");
				writer.Write("<tr><td align=\"left\"># of Registered Customers That Ordered</td><td align=\"center\">" + NumOrderCustomers.ToString() + "</td></tr>\n");
				if(NumCustomers > 0)
				{
					writer.Write("<tr><td align=\"left\">Percentage of All Visitors that Registered</td><td align=\"center\">" + String.Format("{0:0.00}", (((Single)NumRegCustomers/(Single)NumCustomers) * 100)) + "%</td></tr>\n");
					writer.Write("<tr><td align=\"left\">Percentage of All Visitors Who Ordered</td><td align=\"center\">" + String.Format("{0:0.00}",(((Single)NumOrderCustomers/(Single)NumCustomers) * 100)) + "%</td></tr>\n");
				}
				if(NumRegCustomers > 0)
				{
					writer.Write("<tr><td align=\"left\">Percentage of Registered Customers Who Ordered</td><td align=\"center\">" + String.Format("{0:0.00}",(((Single)NumOrderCustomers/(Single)NumRegCustomers) * 100)) + "%</td></tr>\n");
				}
				writer.Write("<tr><td align=\"left\"># Orders</td><td align=\"center\">" + OrderN.ToString() + "</td></tr>\n");
				writer.Write("<tr><td align=\"left\">Order Total</td><td align=\"center\">" + Localization.CurrencyStringForDB(OrderTotal) + "</td></tr>\n");
				writer.Write("<tr><td align=\"left\">Order SubTotal</td><td align=\"center\">" + Localization.CurrencyStringForDB(OrderSubTotal) + "</td></tr>\n");
				writer.Write("<tr><td align=\"left\">Order Tax</td><td align=\"center\">" + Localization.CurrencyStringForDB(OrderTax) + "</td></tr>\n");
				writer.Write("<tr><td align=\"left\">Order Shipping</td><td align=\"center\">" + Localization.CurrencyStringForDB(OrderShipping) + "</td></tr>\n");
				writer.Write("<tr><td align=\"left\">Expense Total</td><td align=\"center\">" + Localization.CurrencyStringForDB(ExpenseTotal) + "</td></tr>\n");
				writer.Write("<tr><td align=\"left\">Net Profit/Loss</td><td align=\"center\">" + Localization.CurrencyStringForDB(NetPL) + "</td></tr>\n");
				writer.Write("<tr><td align=\"left\">Net Profit/Loss (Minus Tax & Shipping costs)</td><td align=\"center\">" + Localization.CurrencyStringForDB(NetPL2) + "</td></tr>\n");
				writer.Write("<tr><td align=\"left\">Average Order Size</td><td align=\"center\">" + Localization.CurrencyStringForDB(OrderAvg) + "</td></tr>\n");
				writer.Write("<tr><td align=\"left\">Revenue Per Visitor</td><td align=\"center\">" + Localization.CurrencyStringForDB(RevenuePerVisitor) + "</td></tr>\n");
				writer.Write("<tr><td align=\"left\">Revenue Per Registered Customer</td><td align=\"center\">" + Localization.CurrencyStringForDB(RevenuePerRegisteredCustomer) + "</td></tr>\n");
				writer.Write("<tr><td align=\"left\">Revenue Per Ordering Customer</td><td align=\"center\">" + Localization.CurrencyStringForDB(RevenuePerOrderingCustomer) + "</td></tr>\n");
				writer.Write("<tr><td align=\"left\">Net Revenue Per Visitor</td><td align=\"center\">" + Localization.CurrencyStringForDB(NetRevenuePerVisitor) + "</td></tr>\n");
				writer.Write("<tr><td align=\"left\">Net Revenue Per Registered Customer</td><td align=\"center\">" + Localization.CurrencyStringForDB(NetRevenuePerRegisteredCustomer) + "</td></tr>\n");
				writer.Write("<tr><td align=\"left\">Net Revenue Per Ordering Customer</td><td align=\"center\">" + Localization.CurrencyStringForDB(NetRevenuePerOrderingCustomer) + "</td></tr>\n");
				writer.Write("</table>\n");
			}
			writer.Write("</div>\n");
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
