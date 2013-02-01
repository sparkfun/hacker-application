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
	/// Summary description for rpt_expenses.
	/// </summary>
	public class rpt_expenses : SkinBase
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Reports - Expenses";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			String StartDate = Common.QueryString("StartDate");
			String EndDate = Common.QueryString("EndDate");
			String GroupBy = Common.QueryString("GroupBy");
			String ExpenseCategoryID = Common.QueryString("ExpenseCategoryID");
			String EasyRange = Common.QueryString("EasyRange");
			String Day = Common.QueryString("Day");
			String Month = Common.QueryString("Month");
			String Year = Common.QueryString("Year");
			String ReportType = Common.QueryString("ReportType");
			String ChartType = Common.QueryString("ChartType");

			if(EasyRange.Length == 0)
			{
				EasyRange = "Today";
			}
			if(GroupBy.Length == 0)
			{
				GroupBy = "Day";
			}
			if(ReportType.Length == 0)
			{
				ReportType = "Table";
			}

			// make sure group by matches easyrange:
			switch(EasyRange)
			{
				case "UseDatesAbove":
					// all options ok
					break;
				case "UseDatesBelow":
					// all options ok
					break;
				case "Today":
					GroupBy = "Day";
					break;
				case "Yesterday":
					GroupBy = "Day";
					break;
				case "ThisWeek":
					GroupBy = "Day";
					break;
				case "LastWeek":
					GroupBy = "Day";
					break;
				case "ThisMonth":
					if(GroupBy == "Year")
					{
						GroupBy = "Day";
					}
					break;
				case "LastMonth":
					if(GroupBy == "Year")
					{
						GroupBy = "Day";
					}
					break;
				case "ThisYear":
					break;
				case "LastYear":
					break;
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

			writer.Write("<form method=\"GET\" action=\"rpt_expenses.aspx\" id=\"ReportForm\" name=\"ReportForm\" onsubmit=\"return (validateForm(this) && ReportForm_Validator(this))\">");
			writer.Write("  <table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" width=\"100%\">");
			writer.Write("    <tr>");
			writer.Write("      <td width=\"25%\" align=\"center\" bgcolor=\"#EAEAEA\"><b><font color=\"#000000\">Date Range:</font></b></td>");
			writer.Write("      <td width=\"25%\" align=\"center\" bgcolor=\"#EAEAEA\"><b><font color=\"#000000\">Expense Qualifiers:</font></b></td>");
			writer.Write("      <td width=\"25%\" align=\"center\" bgcolor=\"#EAEAEA\"><b><font color=\"#000000\">Report Type:</font></b></td>");
			writer.Write("      <td width=\"25%\" align=\"center\" bgcolor=\"#EAEAEA\"><b><font color=\"#000000\">Chart Type:</font></b></td>");
			writer.Write("    </tr>");
			writer.Write("    <tr>");
			writer.Write("      <td width=\"25%\" valign=\"top\" align=\"left\" bgcolor=\"#FFFFCC\">");
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
			writer.Write("      <td width=\"25%\" valign=\"top\" align=\"left\" bgcolor=\"#CCFFFF\">");
			writer.Write("        <table border=\"0\" cellpadding=\"4\" cellspacing=\"0\" width=\"100%\">");
			writer.Write("          <tr>");
			writer.Write("            <td width=\"50%\">Expense Category:</td>");
			writer.Write("            <td width=\"50%\"><select size=\"1\" name=\"ExpenseCategoryID\">");
			writer.Write("                  <option value=\"-\" " + Common.IIF(ExpenseCategoryID == "" || ExpenseCategoryID == "-" , "selected" , "") + ">-</option>");
			IDataReader rs = DB.GetRS("select * from ExpenseCategory  " + DB.GetNoLock() + " order by name");
			while(rs.Read())
			{
				writer.Write("<option value=\"" + DB.RSFieldInt(rs,"ExpenseCategoryID").ToString() + "\"" + Common.IIF(ExpenseCategoryID == DB.RSFieldInt(rs,"ExpenseCategoryID").ToString() , "selected" , "") + ">" + DB.RSField(rs,"Name") + "</option>");
			}
			rs.Close();
			writer.Write("              </select></td>");
			writer.Write("          </tr>");

			writer.Write("        </table>");
			writer.Write("        </td>");
			writer.Write("      <td width=\"25%\" valign=\"top\" align=\"left\" bgcolor=\"#CCCCFF\">");
			writer.Write("        <table border=\"0\" cellpadding=\"4\" cellspacing=\"0\" width=\"100%\">");
			writer.Write("          <tr>");
			writer.Write("            <td width=\"100%\"> <input type=\"radio\" value=\"Chart\" name=\"ReportType\"  " + Common.IIF(ReportType == "Chart" , "checked" , "") + ">Chart</td>");
			writer.Write("          </tr>");
			writer.Write("          <tr>");
			writer.Write("            <td width=\"100%\"> <input type=\"radio\" value=\"Table\" name=\"ReportType\" " + Common.IIF(ReportType == "Table" || ReportType == "" , "checked" , "") + ">Table</td>");
			writer.Write("          </tr>");
			writer.Write("        </table>");
			writer.Write("          <hr size=\"1\">");

			writer.Write("        <table border=\"0\" cellpadding=\"4\" cellspacing=\"0\" width=\"100%\">");
			writer.Write("          <tr>");
			writer.Write("            <td width=\"100%\"><b>Group Data By:</b><br> <input type=\"radio\" value=\"Day\" name=\"GroupBy\"  " + Common.IIF(GroupBy == "Day" || GroupBy == "" , "checked" , "") + ">Day</td>");
			writer.Write("          </tr>");
			writer.Write("          <tr>");
			writer.Write("            <td width=\"100%\"> <input type=\"radio\" value=\"Month\" name=\"GroupBy\" " + Common.IIF(GroupBy == "Month" , "checked" , "") + ">Month</td>");
			writer.Write("          </tr>");
			writer.Write("          <tr>");
			writer.Write("            <td width=\"100%\"> <input type=\"radio\" value=\"Year\" name=\"GroupBy\" " + Common.IIF(GroupBy == "Year" , "checked" , "") + ">Year</td>");
			writer.Write("          </tr>");
			writer.Write("        </table>");

			writer.Write("        </td>");
			writer.Write("      <td width=\"25%\" valign=\"top\" align=\"left\">");
			writer.Write("        <table border=\"0\" cellpadding=\"4\" cellspacing=\"0\" width=\"100%\">");
			writer.Write("          <tr>");
			writer.Write("            <td width=\"100%\"> <input type=\"radio\" value=\"Bar\" name=\"ChartType\" " + Common.IIF(ChartType == "Bar" , "checked" , "") + ">Bar</td>");
			writer.Write("          </tr>");
			writer.Write("          <tr>");
			writer.Write("            <td width=\"100%\"> <input type=\"radio\" value=\"StackedBar\" name=\"ChartType\" " + Common.IIF(ChartType == "StackedBar" , "checked" , "") + ">Stacked Bar</td>");
			writer.Write("          </tr>");
			writer.Write("          <tr>");
			writer.Write("            <td width=\"100%\"><input type=\"radio\" value=\"Line\" name=\"ChartType\" " + Common.IIF(ChartType == "Line" , "checked" , "") + ">Line</td>");
			writer.Write("          </tr>");
			writer.Write("          <tr>");
			writer.Write("            <td width=\"100%\"><input type=\"radio\" value=\"3DBar\" name=\"ChartType\"  " + Common.IIF(ChartType == "3DBar" || ChartType == "" , "checked" , "") + ">3D Bar</td>");
			writer.Write("          </tr>");
			writer.Write("          <tr>");
			writer.Write("            <td width=\"100%\"> <input type=\"radio\" value=\"3DStackedBar\" name=\"ChartType\" " + Common.IIF(ChartType == "3DStackedBar" , "checked" , "") + ">3D Stacked Bar</td>");
			writer.Write("          </tr>");
			writer.Write("          <tr>");
			writer.Write("            <td width=\"100%\"><input type=\"radio\" value=\"3DLine\" name=\"ChartType\" " + Common.IIF(ChartType == "3DLine" , "checked" , "") + ">3D Line</td>");
			writer.Write("          </tr>");
			writer.Write("        </table>");
			writer.Write("        <p>&nbsp;</td>");
			writer.Write("    </tr>");
			writer.Write("    <tr>");
			writer.Write("      <td width=\"100%\" valign=\"top\" align=\"center\" bgcolor=\"#EAEAEA\" colspan=\"4\">");
			writer.Write("        <input type=\"submit\" value=\"Submit\" name=\"B1\"><input type=\"button\" onClick=\"javascript:self.location='rpt_expenses.aspx';\" value=\"Reset\" name=\"B2\">");
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
						DateWhere = " ExpenseDate>=" + DB.DateQuote(StartDate + " 12:00:00.000 AM");
						RangeStartDate = Localization.ParseNativeDateTime(StartDate);
					}
					else
					{
						RangeStartDate = System.DateTime.MinValue; // will get min date returned from either query
					}
					if(EndDate.Length != 0)
					{
						DateWhere += Common.IIF(DateWhere.Length != 0 , " and " , "") + "ExpenseDate <=" + DB.DateQuote(EndDate + " 11:59:59.999 PM");
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
						DateWhere = " day(ExpenseDate)=" + Day + " ";
					}
					else
					{
						GroupBy = "Month";
					}
					if(Month.Length != 0 && Month != "0")
					{
						if(DateWhere.Length != 0)
						{
							DateWhere += " and ";
						}
						DateWhere += " month(ExpenseDate)=" + Month + " ";
					}
					else
					{
						GroupBy = "Year";
					}
					if(Year.Length != 0 && Year != "0")
					{
						if(DateWhere.Length != 0)
						{
							DateWhere += " and ";
						}
						DateWhere += " year(ExpenseDate)=" + Year + " ";
					};
					String DaySpec = Common.IIF(Day.Length == 0 || Day == "0" , "1" , Day);
					String MonthSpec = Common.IIF(Month.Length == 0 || Month == "0" , "1" , Month);
					String YearSpec = Common.IIF(Year.Length == 0 || Year == "0" , System.DateTime.Now.Year.ToString() , Year);
					RangeStartDate = Localization.ParseNativeDateTime(MonthSpec + "/" + DaySpec + "/" + YearSpec);
					RangeEndDate = RangeStartDate;
					break;
				case "Today":
					DateWhere = "day(ExpenseDate)=" + System.DateTime.Now.Day.ToString() + " and month(ExpenseDate)=" + System.DateTime.Now.Month.ToString() + " and year(ExpenseDate)=" + System.DateTime.Now.Year.ToString();
					RangeStartDate = System.DateTime.Now;
					RangeEndDate = System.DateTime.Now;
					break;
				case "Yesterday":
					DateWhere = "day(ExpenseDate)=" + System.DateTime.Now.AddDays(-1).Day.ToString() + " and month(ExpenseDate)=" + System.DateTime.Now.AddDays(-1).Month.ToString() + " and year(ExpenseDate)=" + System.DateTime.Now.AddDays(-1).Year.ToString();
					RangeStartDate = System.DateTime.Now.AddDays(-1);
					RangeEndDate = System.DateTime.Now.AddDays(-1);
					break;
				case "ThisWeek":
					int DayOfWeek = (int)System.DateTime.Now.DayOfWeek;
					System.DateTime weekstart = System.DateTime.Now.AddDays(-(DayOfWeek));
					System.DateTime weekend = weekstart.AddDays(6);
					int weekstartday = weekstart.DayOfYear;
					int weekendday = weekend.DayOfYear;
					DateWhere = "year(ExpenseDate)=" + System.DateTime.Now.Year.ToString() + " and (datepart(\"dy\",ExpenseDate)>=" + weekstartday.ToString() + " and datepart(\"dy\",ExpenseDate)<=" + weekendday.ToString() + ")";
					RangeStartDate = weekstart;
					RangeEndDate = weekend;
					break;
				case "LastWeek":
					int DayOfWeek2 = (int)System.DateTime.Now.DayOfWeek;
					System.DateTime weekstart2 = System.DateTime.Now.AddDays(-(DayOfWeek2)).AddDays(-7);
					System.DateTime weekend2 = weekstart2.AddDays(6);
					int weekstartday2 = weekstart2.DayOfYear;
					int weekendday2 = weekend2.DayOfYear;
					DateWhere = "year(ExpenseDate)=" + System.DateTime.Now.Year.ToString() + " and (datepart(\"dy\",ExpenseDate)>=" + weekstartday2.ToString() + " and datepart(\"dy\",ExpenseDate)<=" + weekendday2.ToString() + ")";
					RangeStartDate = weekstart2;
					RangeEndDate = weekend2;
					break;
				case "ThisMonth":
					DateWhere = "month(ExpenseDate)=" + System.DateTime.Now.Month.ToString() + " and year(ExpenseDate)=" + System.DateTime.Now.Year.ToString();
					RangeStartDate = Localization.ParseNativeDateTime(System.DateTime.Now.Month.ToString() + "/1/" + System.DateTime.Now.Year.ToString());
					RangeEndDate = RangeStartDate.AddMonths(1).AddDays(-1);
					break;
				case "LastMonth":
					DateWhere = "month(ExpenseDate)=" + System.DateTime.Now.AddMonths(-1).Month.ToString() + " and year(ExpenseDate)=" + System.DateTime.Now.AddMonths(-1).Year.ToString();
					RangeStartDate = Localization.ParseNativeDateTime(System.DateTime.Now.AddMonths(-1).Month.ToString() + "/1/" + System.DateTime.Now.AddMonths(-1).Year.ToString());
					RangeEndDate = RangeStartDate.AddMonths(1).AddDays(-1);
					break;
				case "ThisYear":
					DateWhere = "year(ExpenseDate)=" + System.DateTime.Now.Year.ToString();
					RangeStartDate = Localization.ParseUSDateTime("1/1/" + System.DateTime.Now.Year.ToString());
					RangeEndDate = RangeStartDate.AddYears(1).AddDays(-1);
					if(RangeEndDate > System.DateTime.Now)
					{
						RangeEndDate = System.DateTime.Now;
					}
					break;
				case "LastYear":
					DateWhere = "year(ExpenseDate)=" + System.DateTime.Now.AddYears(-1).Year.ToString();
					RangeStartDate = Localization.ParseUSDateTime("1/1/" + System.DateTime.Now.AddYears(-1).Year.ToString());
					RangeEndDate = RangeStartDate.AddYears(1).AddDays(-1);
					break;
			}
			if(DateWhere.Length != 0)
			{
				DateWhere = "(" + DateWhere + ")";
			}

			String ChartTypeSpec = String.Empty;
			String ChartTypeSpecNew = String.Empty;
			bool ChartIs3D = (ChartType.IndexOf("3D") != -1);
			switch(ChartType)
			{
				case "Bar":
				case "3DBar":
					ChartTypeSpec = "columnApp";
					ChartTypeSpecNew = "bar";
					break;
				case "StackedBar":
				case "3DStackedBar":
					ChartTypeSpec = "stackColumnApp";
					ChartTypeSpecNew = "stackbar";
					break;
				case "Line":
				case "3DLine":
					ChartTypeSpec = "lineApp";
					ChartTypeSpecNew = "line";
					break;
			}
			String WhereClause = DateWhere;

			String Series1Name = String.Empty;
			String Series2Name = String.Empty;
			//switch(ShowType)
			//{
			//	case "NumberOf":
			//		Series1Name = "Number Of Expenses";
			//		break;
			//	case "Amounts":
			//		Series1Name = "Amounts";
			//		break;
			//}

			String SelectFields = String.Empty;
			String GroupByFields = String.Empty;
			String OrderByFields = String.Empty;
			String DateFormat = String.Empty;
			String GroupByIncrement = String.Empty;
			switch(GroupBy)
			{
				case "Day":
					SelectFields = "datepart(\"dy\",ExpenseDate) as [Day], Year(ExpenseDate) as [Year]";
					GroupByFields = "Year(ExpenseDate), datepart(\"dy\",ExpenseDate)";
					OrderByFields = "Year(ExpenseDate) asc, datepart(\"dy\",ExpenseDate) asc";
					DateFormat = "mm-dd-yyyy";
					GroupByIncrement = "0";
					break;
				case "Month":
					SelectFields = "month(ExpenseDate) as [Month], Year(ExpenseDate) as [Year]";
					GroupByFields = "Year(ExpenseDate), month(ExpenseDate)";
					OrderByFields = "Year(ExpenseDate) asc, month(ExpenseDate) asc";
					DateFormat = "mm-yyyy";
					GroupByIncrement = "2";
					break;
				case "Year":
					SelectFields = "Year(ExpenseDate) as [Year]";
					GroupByFields = "Year(ExpenseDate)";
					OrderByFields = "Year(ExpenseDate) asc";
					DateFormat = "yyyy";
					GroupByIncrement = "3";
					break;
			}

			String GeneralWhere = String.Empty;
			if(ExpenseCategoryID != "-" && ExpenseCategoryID.Length != 0)
			{
				if(GeneralWhere.Length != 0)
				{
					GeneralWhere += " and ";
				}
				GeneralWhere += "ExpenseCategoryID=" + ExpenseCategoryID;
			}

			if(GeneralWhere.Length != 0)
			{
				GeneralWhere = "(" + GeneralWhere + ")";
			}
			
			if(DateWhere.Length != 0)
			{
				String DS1SQL = "select count(ExpenseID) as N, Sum(Amount) as Total, " + SelectFields + " from expense  " + DB.GetNoLock() + " where 1=1 " + Common.IIF(GeneralWhere.Length != 0 , " and " + GeneralWhere , "") + Common.IIF(WhereClause.Length != 0 , " and " + WhereClause , "") + " group by " + GroupByFields + " Order by " + OrderByFields;
				String DS2SQL = "select * from expense  " + DB.GetNoLock() + " where 1=0"; // return empty ds always, "select count(CustomerID) as N, " + SelectFields + " from customer where Email not like 'Anon_%' " + (RegOnlyWhere.Length != 0 , " and " + RegOnlyWhere , "")  + (GeneralWhere.Length != 0 , " and " + GeneralWhere , "") + (WhereClause.Length != 0 , " and " + WhereClause , "") + " group by " + GroupByFields + " Order by " + OrderByFields;
				if(Common.AppConfigBool("Admin_ShowReportSQL"))
				{
					writer.Write("<p align=\"left\">DS1SQL=" + DS1SQL + "</p>\n");
					writer.Write("<p align=\"left\">DS2SQL=" + DS2SQL + "</p>\n");
				}
				//writer.Write("<p align=\"left\">RangeStartDate=" + Localization.ToNativeShortDateString(RangeStartDate) + "</p>\n");
				//writer.Write("<p align=\"left\">RangeEndDate=" + Localization.ToNativeShortDateString(RangeEndDate) + "</p>\n");
				
				DataSet ds1;
				DataSet ds2;
				try
				{
					ds1 = DB.GetDS(DS1SQL,false,System.DateTime.Now.AddHours(1));
					ds2 = DB.GetDS(DS2SQL,false,System.DateTime.Now.AddHours(1));
					if(ds1.Tables[0].Rows.Count == 0 && ds2.Tables[0].Rows.Count == 0)
					{
						writer.Write("<p align=\"left\"><b>NO DATA FOUND</b></p>\n");
					}
					else
					{
						int DS1SumN = 0;
						decimal DS1SumTotal = System.Decimal.Zero;
						int DS2SumN = 0;
						decimal DS2SumTotal = System.Decimal.Zero;
						int DS1NumRecs = ds1.Tables[0].Rows.Count;
						int DS2NumRecs = ds2.Tables[0].Rows.Count;
						int MaxNumRecs = Common.Max(DS1NumRecs,DS2NumRecs);
						foreach(DataRow row in ds1.Tables[0].Rows)
						{
							DS1SumN += DB.RowFieldInt(row,"N");
							DS1SumTotal += DB.RowFieldDecimal(row,"Total");
						}
						foreach(DataRow row in ds2.Tables[0].Rows)
						{
							DS2SumN += DB.RowFieldInt(row,"N");
							DS2SumTotal += DB.RowFieldDecimal(row,"Total");
						}
						// set range start date, if necessary:
						IDataReader rsd = DB.GetRS("select min(ExpenseDate) as RangeStartDate from Expense " + DB.GetNoLock() + " ");
						DateTime MinCustomerDate = System.DateTime.MinValue;
						if(rsd.Read())
						{
							MinCustomerDate = DB.RSFieldDateTime(rsd,"RangeStartDate");
						}
						else
						{
							MinCustomerDate = Localization.ParseUSDateTime("1/1/2003"); // we need SOME value!
						}
						rsd.Close();
						if(RangeStartDate == System.DateTime.MinValue)
						{
							RangeStartDate = MinCustomerDate;
						}
						if(RangeStartDate < MinCustomerDate)
						{
							RangeStartDate = MinCustomerDate;
						}
						String DateSeries = String.Empty;

						String DS1Dates = String.Empty;
						String DS1ValuesN = String.Empty;
						String DS1ValuesTotal = String.Empty;

						String DS2Dates = String.Empty;
						String DS2ValuesN = String.Empty;
						String DS2ValuesTotal = String.Empty;

						bool first = true;
						foreach(DataRow row in ds1.Tables[0].Rows)
						{
							if(!first)
							{
								DS1Dates += "|";
							}
							switch(GroupBy)
							{
								case "Day":
									int dy = DB.RowFieldInt(row,"Day");
									DateTime dt = Localization.ParseUSDateTime("1/1/" + DB.RowField(row,"Year")).AddDays(dy-1);
									DS1Dates += dt.Month.ToString().PadLeft(2,'0') + '-' + dt.Day.ToString().PadLeft(2,'0') + "-" + dt.Year.ToString().PadLeft(4,'0');
									break;
								case "Month":
									DS1Dates += DB.RowField(row,"Month").PadLeft(2,'0') + "-" + DB.RowField(row,"Year").PadLeft(4,'0');
									break;
								case "Year":
									DS1Dates += DB.RowField(row,"Year").PadLeft(4,'0');
									break;
							}
							first = false;
						}
						if(Series2Name.Length != 0)
						{
							first = true;
							foreach(DataRow row in ds2.Tables[0].Rows)
							{
								if(!first)
								{
									DS2Dates += "|";
								}
								switch(GroupBy)
								{
									case "Day":
										int dy = DB.RowFieldInt(row,"Day");
										DateTime dt = Localization.ParseUSDateTime("1/1/" + DB.RowField(row,"Year")).AddDays(dy-1);
										DS2Dates += dt.Month.ToString().PadLeft(2,'0') + '-' + dt.Day.ToString().PadLeft(2,'0') + "-" + dt.Year.ToString().PadLeft(4,'0');
										break;
									case "Month":
										DS2Dates += DB.RowField(row,"Month").PadLeft(2,'0') + "-" + DB.RowField(row,"Year").PadLeft(4,'0');
										break;
									case "Year":
										DS2Dates += DB.RowField(row,"Year").PadLeft(4,'0');
										break;
								}
								first = false;
							}
						}

						int NumBuckets = 0;
						// determine how many "buckets" are in the date series:
						switch(GroupBy)
						{
							case "Day":
								for(DateTime yy = RangeStartDate; yy <= RangeEndDate; yy = yy.AddDays(1))
								{
									NumBuckets++;
								}
								break;
							case "Month":
								for(DateTime yy = Localization.ParseNativeDateTime(RangeStartDate.Month.ToString() + "/1/"+ RangeStartDate.Year.ToString()); yy <= Localization.ParseNativeDateTime(RangeEndDate.Month.ToString() + "/1/"+ RangeEndDate.Year.ToString()); yy = yy.AddMonths(1))
								{
									NumBuckets++;
								}
								break;
							case "Year":
								for(DateTime yy = Localization.ParseUSDateTime("1/1/" + RangeStartDate.Year.ToString()); yy <= Localization.ParseUSDateTime("1/1/" + RangeEndDate.Year.ToString()); yy = yy.AddYears(1))
								{
									NumBuckets++;
								}
								break;
						}
						//int ChartWidth = 800;
						int BarWidth = 800 / (NumBuckets);
						if(Series2Name.Length != 0 && ChartType.ToLower().IndexOf("stack") == -1)
						{
							BarWidth = BarWidth / 2;
						}

						// COMPOSE FULL DATE and RANGE and SUM SERIES:
						int ds1_idx = 0;
						int ds2_idx = 0;
						int[] SumsN = new int[NumBuckets];
						decimal[] SumsTotal = new decimal[NumBuckets];
						for(int i = SumsN.GetLowerBound(0); i <= SumsN.GetUpperBound(0); i++)
						{
							SumsN[i] = 0;
							SumsTotal[i] = System.Decimal.Zero;
						}
						int SumBucketIdx = 0;
						switch(GroupBy)
						{
							case "Day":
								for(DateTime yy = RangeStartDate; yy <= RangeEndDate; yy = yy.AddDays(1))
								{
									if(DateSeries.Length != 0)
									{
										DateSeries += "|";
										DS1ValuesN += "|";
										DS1ValuesTotal += "|";
										DS2ValuesN += "|";
										DS2ValuesTotal += "|";
									}
									DateSeries += yy.Month.ToString() + "-" + yy.Day.ToString() + "-" + yy.Year.ToString();
									if(ds1_idx < DS1NumRecs)
									{
										DataRow ds1Row = ds1.Tables[0].Rows[ds1_idx];
										int dy1 = DB.RowFieldInt(ds1Row,"Day");
										DateTime dt1 = Localization.ParseUSDateTime("1/1/" + DB.RowField(ds1Row,"Year")).AddDays(dy1-1);
										if(dt1.Month == yy.Month && dt1.Day == yy.Day && dt1.Year == yy.Year)
										{
											DS1ValuesN += DB.RowFieldInt(ds1Row,"N").ToString();
											DS1ValuesTotal += DB.RowFieldDecimal(ds1Row,"Total").ToString();
											SumsN[SumBucketIdx] += DB.RowFieldInt(ds1Row,"N");
											SumsTotal[SumBucketIdx] += DB.RowFieldDecimal(ds1Row,"Total");
											ds1_idx++;
										}
										else
										{
											DS1ValuesN += "0";
											DS1ValuesTotal += "0.0";
										}
									}
									else
									{
										DS1ValuesN += "0";
										DS1ValuesTotal += "0.0";
									}
									if(Series2Name.Length != 0 && ds2_idx < DS2NumRecs)
									{
										DataRow ds2Row = ds2.Tables[0].Rows[ds2_idx];
										int dy2 = DB.RowFieldInt(ds2Row,"Day");
										DateTime dt2 = Localization.ParseUSDateTime("1/1/" + DB.RowField(ds2Row,"Year")).AddDays(dy2-1);
										if(dt2.Month == yy.Month && dt2.Day == yy.Day && dt2.Year == yy.Year)
										{
											DS2ValuesN += DB.RowFieldInt(ds2Row,"N").ToString();
											DS2ValuesTotal += DB.RowFieldDecimal(ds2Row,"Total").ToString();
											SumsN[SumBucketIdx] += DB.RowFieldInt(ds2Row,"N");
											SumsTotal[SumBucketIdx] += DB.RowFieldDecimal(ds2Row,"Total");
											ds2_idx++;
										}
										else
										{
											DS2ValuesN += "0";
											DS2ValuesTotal += "0.0";
										}
									}
									else
									{
										DS2ValuesN += "0";
										DS2ValuesTotal += "0.0";
									}
									SumBucketIdx++;
								}
								break;
							case "Month":
								for(DateTime yy = Localization.ParseNativeDateTime(RangeStartDate.Month.ToString() + "/1/"+ RangeStartDate.Year.ToString()); yy <= Localization.ParseNativeDateTime(RangeEndDate.Month.ToString() + "/1/"+ RangeEndDate.Year.ToString()); yy = yy.AddMonths(1))
								{
									if(DateSeries.Length != 0)
									{
										DateSeries += "|";
										DS1ValuesN += "|";
										DS1ValuesTotal += "|";
										DS2ValuesN += "|";
										DS2ValuesTotal += "|";
									}
									DateSeries += yy.Month.ToString() + "-" + yy.Year.ToString();
									if(ds1_idx < DS1NumRecs)
									{
										DataRow ds1Row = ds1.Tables[0].Rows[ds1_idx];
										DateTime dt1 = Localization.ParseNativeDateTime(DB.RowField(ds1Row,"Month").ToString() + "/1/" + DB.RowField(ds1Row,"Year").ToUpper());
										if(dt1.Month == yy.Month && dt1.Year == yy.Year)
										{
											DS1ValuesN += DB.RowFieldInt(ds1Row,"N").ToString();
											DS1ValuesTotal += DB.RowFieldDecimal(ds1Row,"Total").ToString();
											SumsN[SumBucketIdx] += DB.RowFieldInt(ds1Row,"N");
											SumsTotal[SumBucketIdx] += DB.RowFieldDecimal(ds1Row,"Total");
											ds1_idx++;
										}
										else
										{
											DS1ValuesN += "0";
											DS1ValuesTotal += "0.0";
										}
									}
									else
									{
										DS1ValuesN += "0";
										DS1ValuesTotal += "0.0";
									}
									if(Series2Name.Length != 0 && ds2_idx < DS2NumRecs)
									{
										DataRow ds2Row = ds2.Tables[0].Rows[ds2_idx];
										DateTime dt2 = Localization.ParseNativeDateTime(DB.RowField(ds2Row,"Month").ToString() + "/1/" + DB.RowField(ds2Row,"Year").ToUpper());
										if(dt2.Month == yy.Month && dt2.Year == yy.Year)
										{
											DS2ValuesN += DB.RowFieldInt(ds2Row,"N").ToString();
											DS2ValuesTotal += DB.RowFieldDecimal(ds2Row,"Total").ToString();
											SumsN[SumBucketIdx] += DB.RowFieldInt(ds2Row,"N");
											SumsTotal[SumBucketIdx] += DB.RowFieldDecimal(ds2Row,"Total");
											ds2_idx++;
										}
										else
										{
											DS2ValuesN += "0";
											DS2ValuesTotal += "0.0";
										}
									}
									else
									{
										DS2ValuesN += "0";
										DS2ValuesTotal += "0.0";
									}								
									SumBucketIdx++;
								}
								break;
							case "Year":
								for(DateTime yy = Localization.ParseUSDateTime("1/1/" + RangeStartDate.Year.ToString()); yy <= Localization.ParseUSDateTime("1/1/" + RangeEndDate.Year.ToString()); yy = yy.AddYears(1))
								{
									if(DateSeries.Length != 0)
									{
										DateSeries += "|";
										DS1ValuesN += "|";
										DS1ValuesTotal += "|";
										DS2ValuesN += "|";
										DS2ValuesTotal += "|";
									}
									DateSeries += yy.Year.ToString();
									if(ds1_idx < DS1NumRecs)
									{
										DataRow ds1Row = ds1.Tables[0].Rows[ds1_idx];
										DateTime dt1 = Localization.ParseUSDateTime("1/1/" + DB.RowField(ds1Row,"Year").ToUpper());
										if(dt1.Year == yy.Year)
										{
											DS1ValuesN += DB.RowFieldInt(ds1Row,"N").ToString();
											DS1ValuesTotal += DB.RowFieldDecimal(ds1Row,"Total").ToString();
											SumsN[SumBucketIdx] += DB.RowFieldInt(ds1Row,"N");
											SumsTotal[SumBucketIdx] += DB.RowFieldDecimal(ds1Row,"Total");
											ds1_idx++;
										}
										else
										{
											DS1ValuesN += "0";
											DS1ValuesTotal += "0.0";
										}
									}
									else
									{
										DS1ValuesN += "0";
										DS1ValuesTotal += "0.0";
									}
									if(ds2_idx < DS2NumRecs)
									{
										DataRow ds2Row = ds2.Tables[0].Rows[ds2_idx];
										DateTime dt2 = Localization.ParseUSDateTime("1/1/" + DB.RowField(ds2Row,"Year").ToUpper());
										if(dt2.Year == yy.Year)
										{
											DS2ValuesN += DB.RowFieldInt(ds2Row,"N").ToString();
											DS2ValuesTotal += DB.RowFieldDecimal(ds2Row,"Total").ToString();
											SumsN[SumBucketIdx] += DB.RowFieldInt(ds2Row,"N");
											SumsTotal[SumBucketIdx] += DB.RowFieldDecimal(ds2Row,"Total");
											ds2_idx++;
										}
										else
										{
											DS2ValuesN += "0";
											DS2ValuesTotal += "0.0";
										}
									}
									else
									{
										DS2ValuesN += "0";
										DS2ValuesTotal += "0.0";
									}									
									SumBucketIdx++;
								}
								break;
						}

						writer.Write("<p align=\"left\"><b>Number of Expenses: " + DS1SumN.ToString() + "</b></p>\n");

						if(DS1SumN > 0 || DS2SumN > 0)
						{
							String ReportTitle = "Expenses Report|" + Localization.ToNativeShortDateString(RangeStartDate) + " - " + Localization.ToNativeShortDateString(RangeEndDate) + "|Group By: " + GroupBy;
							String ReportTitleN = "Number Of Expenses Report|" + Localization.ToNativeShortDateString(RangeStartDate) + " - " + Localization.ToNativeShortDateString(RangeEndDate) + "|Group By: " + GroupBy;
							String ReportTitleTotal = "Expense Total Report|Sum=" + Localization.DecimalStringForDB(DS1SumTotal) + "|" + Localization.ToNativeShortDateString(RangeStartDate) + " - " + Localization.ToNativeShortDateString(RangeEndDate) + "|Group By: " + GroupBy;
							if(ReportType == "Chart")
							{
								// Number of Expenses Chart:
								Series1Name = "Expenses";
								writer.Write(Common.GetChart(ReportTitleN,"Date","# Expenses","100%","500",ChartIs3D,ChartTypeSpec,Series1Name,Series2Name,DateSeries,DS1ValuesN,DS2ValuesN));
								writer.Write("<br><br>");

								// Total Chart:
								writer.Write(Common.GetChart(ReportTitleTotal,"Date","Total","100%","500",ChartIs3D,ChartTypeSpec,Series1Name,Series2Name,DateSeries,DS1ValuesTotal,DS2ValuesTotal));

							}
							else
							{
								// WRITE OUT THE TABLE:
								String[] DD = DateSeries.Split('|');

								String[] S1N = DS1ValuesN.Split('|');
								String[] S1Total = DS1ValuesTotal.Split('|');
								
								String[] S2N = DS2ValuesN.Split('|');
								String[] S2Total = DS2ValuesTotal.Split('|');

								if(NumBuckets > 60)
								{
									// VERTICAL:
									writer.Write("<p align=\"center\"><b>" + ReportTitle + "</b></p>\n");
									writer.Write("<table border=\"1\" cellpadding=\"4\" cellspacing=\"0\">\n");
									writer.Write("  <tr>\n");
									writer.Write("    <td bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>Date</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b># Expenses</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>Total</b></td>\n");
									if(Series2Name.Length != 0)
									{
										writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + Series2Name + "</b></td>\n");
									}
									//writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>Total</b></td>\n");
									writer.Write("  </tr>\n");

									writer.Write("  <tr>\n");
									writer.Write("    <td bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>Total</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + DS1SumN.ToString() + "</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + Localization.DecimalStringForDB( DS1SumTotal) + "</b></td>\n");
									if(Series2Name.Length != 0)
									{
										writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + DS2SumN.ToString() + "</b></td>\n");
									}
									//writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + (DS1SumN + DS2SumN) + "</b></td>\n");
									writer.Write("  </tr>\n");
									
									for(int row = 0; row < NumBuckets; row++)
									{
										writer.Write("  <tr>\n");
										writer.Write("    <td>" + DD[row] + "</td>\n");
										writer.Write("    <td align=\"center\" >" + Common.IIF(S1N[row] == "0" , "&nbsp;" , S1N[row]) + "</td>\n");
										writer.Write("    <td align=\"center\" >" + Common.IIF(S1N[row] == "0" , "&nbsp;" , Localization.DecimalStringForDB( System.Decimal.Parse(S1Total[row]))) + "</td>\n");
										if(Series2Name.Length != 0)
										{
											writer.Write("    <td align=\"center\" >" + Common.IIF(S2N[row] == "0" , "&nbsp;" , S2N[row]) + "</td>\n");
										}
										//writer.Write("    <td align=\"center\" >" + SumsN[row] + "</td>\n");
										writer.Write("  </tr>\n");
									}
									writer.Write("  <tr>\n");
									writer.Write("    <td bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>Total</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + DS1SumN.ToString() + "</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + Localization.DecimalStringForDB( DS1SumTotal) + "</b></td>\n");
									if(Series2Name.Length != 0)
									{
										writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + DS2SumN.ToString() + "</b></td>\n");
									}
									//writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + (DS1SumN + DS2SumN) + "</b></td>\n");
									writer.Write("  </tr>\n");
									writer.Write("</table>\n");
								}
								else
								{
									// HORIZONTAL:

									// Number Of Expenses Table:
									writer.Write("<p align=\"center\"><b>" + ReportTitle + "</b></p>\n");
									writer.Write("<table border=\"1\" cellpadding=\"4\" cellspacing=\"0\">\n");

									writer.Write("  <tr>\n");
									writer.Write("    <td bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\">&nbsp;</td>\n");
									for(int row = 0; row < NumBuckets; row++)
									{
										writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + DD[row] + "</b></td>\n");
									}
									writer.Write("    <td align=\"center\" bgcolor=\"#FFFFCC\"><b>Total</b></td>\n");
									writer.Write("  </tr>\n");

									// Number of Expenses
									writer.Write("  <tr>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b># Expenses</b></td>\n");
									for(int row = 0; row < NumBuckets; row++)
									{
										writer.Write("    <td align=\"center\" >" + Common.IIF(S1N[row] == "0" , "&nbsp;" , S1N[row]) + "</td>\n");
									}
									writer.Write("    <td align=\"center\" bgcolor=\"#FFFFCC\"><b>" + DS1SumN.ToString()+ "</b></td>\n");
									writer.Write("  </tr>\n");

									// Totals
									writer.Write("  <tr>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>Totals</b></td>\n");
									for(int row = 0; row < NumBuckets; row++)
									{
										writer.Write("    <td align=\"center\" >" + Common.IIF(S1Total[row] == "0" , "&nbsp;" , Localization.DecimalStringForDB( System.Decimal.Parse(S1Total[row]))) + "</td>\n");
									}
									writer.Write("    <td align=\"center\" bgcolor=\"#FFFFCC\"><b>" + Localization.DecimalStringForDB( DS1SumTotal) + "</b></td>\n");
									writer.Write("  </tr>\n");

									writer.Write("</table>\n");
								}
							}
						}
					}
					ds1.Dispose();
					ds2.Dispose();
				}
				catch(Exception ex)
				{
					ErrorMsg = Common.GetExceptionDetail(ex,"<br>");
					writer.Write("<p align=\"left\"><b><font color=\"red\">" + ErrorMsg + "</font></b></p>\n");
				}

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
