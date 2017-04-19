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
	/// Summary description for rpt_emails.
	/// </summary>
	public class rpt_emails : SkinBase
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Reports - Customer E-Mails";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{

//SEC4
      string SuperuserFilter = Common.IIF(thisCustomer.IsAdminSuperUser , String.Empty , String.Format(" Customer.CustomerID not in ({0}) and ",Common.AppConfig("Admin_Superuser")));
			
      IDataReader rsd = DB.GetRS("Select min(CreatedOn) as MinDate from customer " + DB.GetNoLock() + " ");
			DateTime MinCustomerDate = Localization.ParseUSDateTime("1/1/1990");
			if(rsd.Read())
			{
				MinCustomerDate = DB.RSFieldDateTime(rsd,"MinDate");
			}
			rsd.Close();
			
			String StartDate = Common.QueryString("StartDate");
			String EndDate = Common.QueryString("EndDate");
			String AffiliateID = Common.QueryString("AffiliateID");
			String Gender = Common.QueryString("Gender");
			String CouponCode = Common.QueryString("CouponCode");
			String WithOrders = Common.QueryString("WithOrders");
			String EasyRange = Common.QueryString("EasyRange");
			String Day = Common.QueryString("Day");
			String Month = Common.QueryString("Month");
			String Year = Common.QueryString("Year");
			String CustomerType = Common.QueryString("CustomerType");

			if(EasyRange.Length == 0)
			{
				EasyRange = "Today";
			}
			if(CustomerType.Length == 0)
			{
				CustomerType = "AlLCustomers";
			}

			// reset date range here, to ensure new orders are visible:
			if(StartDate.Length == 0)
			{
				StartDate = Localization.ToNativeShortDateString(MinCustomerDate);
			}
			if(EndDate.Length == 0)
			{
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

			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function ReportForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");

			writer.Write("<form method=\"GET\" action=\"rpt_emails.aspx\" id=\"ReportForm\" name=\"ReportForm\" onsubmit=\"return (validateForm(this) && ReportForm_Validator(this))\">");
			writer.Write("  <table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" width=\"100%\">");
			writer.Write("    <tr>");
			writer.Write("      <td width=\"25%\" align=\"center\" bgcolor=\"#EAEAEA\"><b><font color=\"#000000\">Date Range:</font></b></td>");
			writer.Write("      <td width=\"25%\" align=\"center\" bgcolor=\"#EAEAEA\"><b><font color=\"#000000\">Customer Qualifiers:</font></b></td>");
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
			writer.Write("            <td width=\"50%\">Gender:</td>");
			writer.Write("            <td width=\"50%\"><select size=\"1\" name=\"Gender\">");
			writer.Write("                  <option value=\"-\" " + Common.IIF(Gender == "" || Gender == "-" , "selected" , "") + ">-</option>");
			writer.Write("                <option value=\"M\"" + Common.IIF(Gender == "M" , "selected" , "") + ">Male</option>");
			writer.Write("                <option value=\"F\"" + Common.IIF(Gender == "F" , "selected" , "") + ">Female</option>");
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
			writer.Write("            <td width=\"50%\">With Orders:</td>");
			writer.Write("            <td width=\"50%\">");
			writer.Write("                <input type=\"radio\" name=\"WithOrders\" value=\"No\"" + Common.IIF(WithOrders == "No"  || WithOrders.Length == 0 , " checked " , "") + ">No&nbsp;&nbsp;&nbsp;&nbsp;");
			writer.Write("                <input type=\"radio\" name=\"WithOrders\" value=\"Yes\"" + Common.IIF(WithOrders == "Yes", " checked " , "") + ">Yes");
			writer.Write("                <input type=\"radio\" name=\"WithOrders\" value=\"Invert\"" + Common.IIF(WithOrders == "Invert", " checked " , "") + ">Without Orders");
			writer.Write("              </td>");
			writer.Write("          </tr>");
			writer.Write("        </table>");
			writer.Write("        </td>");
			writer.Write("    </tr>");
			writer.Write("    <tr>");
			writer.Write("      <td width=\"100%\" valign=\"top\" align=\"center\" bgcolor=\"#EAEAEA\" colspan=\"2\">");
			writer.Write("        <input type=\"submit\" value=\"Submit\" name=\"B1\"><input type=\"button\" onClick=\"javascript:self.location='rpt_emails.aspx';\" value=\"Reset\" name=\"B2\">");
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
						DateWhere = " CreatedOn>=" + DB.DateQuote(StartDate + " 12:00:00.000 AM");
						RangeStartDate = Localization.ParseNativeDateTime(StartDate);
					}
					else
					{
						RangeStartDate = System.DateTime.MinValue; // will get min date returned from either query
					}
					if(EndDate.Length != 0)
					{
						DateWhere += Common.IIF(DateWhere.Length != 0 , " and " , "") + "CreatedOn <=" + DB.DateQuote(EndDate + " 11:59:59.999 PM");
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
						DateWhere = " day(CreatedOn)=" + Day + " ";
					}
					if(Month.Length != 0 && Month != "0")
					{
						if(DateWhere.Length != 0)
						{
							DateWhere += " and ";
						}
						DateWhere += " month(CreatedOn)=" + Month + " ";
					}
					if(Year.Length != 0 && Year != "0")
					{
						if(DateWhere.Length != 0)
						{
							DateWhere += " and ";
						}
						DateWhere += " year(CreatedOn)=" + Year + " ";
					};
					String DaySpec = Common.IIF(Day.Length == 0 || Day == "0" , "1" , Day);
					String MonthSpec = Common.IIF(Month.Length == 0 || Month == "0" , "1" , Month);
					String YearSpec = Common.IIF(Year.Length == 0 || Year == "0" , System.DateTime.Now.Year.ToString() , Year);
					RangeStartDate = Localization.ParseNativeDateTime(MonthSpec + "/" + DaySpec + "/" + YearSpec);
					RangeEndDate = RangeStartDate;
					break;
				case "Today":
					DateWhere = "day(CreatedOn)=" + System.DateTime.Now.Day.ToString() + " and month(CreatedOn)=" + System.DateTime.Now.Month.ToString() + " and year(CreatedOn)=" + System.DateTime.Now.Year.ToString();
					RangeStartDate = System.DateTime.Now;
					RangeEndDate = System.DateTime.Now;
					break;
				case "Yesterday":
					DateWhere = "day(CreatedOn)=" + System.DateTime.Now.AddDays(-1).Day.ToString() + " and month(CreatedOn)=" + System.DateTime.Now.AddDays(-1).Month.ToString() + " and year(CreatedOn)=" + System.DateTime.Now.AddDays(-1).Year.ToString();
					RangeStartDate = System.DateTime.Now.AddDays(-1);
					RangeEndDate = System.DateTime.Now.AddDays(-1);
					break;
				case "ThisWeek":
					int DayOfWeek = (int)System.DateTime.Now.DayOfWeek;
					System.DateTime weekstart = System.DateTime.Now.AddDays(-(DayOfWeek));
					System.DateTime weekend = weekstart.AddDays(6);
					int weekstartday = weekstart.DayOfYear;
					int weekendday = weekend.DayOfYear;
					DateWhere = "year(CreatedOn)=" + System.DateTime.Now.Year.ToString() + " and (datepart(\"dy\",CreatedOn)>=" + weekstartday.ToString()+ " and datepart(\"dy\",CreatedOn)<=" + weekendday.ToString() + ")";
					RangeStartDate = weekstart;
					RangeEndDate = weekend;
					break;
				case "LastWeek":
					int DayOfWeek2 = (int)System.DateTime.Now.DayOfWeek;
					System.DateTime weekstart2 = System.DateTime.Now.AddDays(-(DayOfWeek2)).AddDays(-7);
					System.DateTime weekend2 = weekstart2.AddDays(6);
					int weekstartday2 = weekstart2.DayOfYear;
					int weekendday2 = weekend2.DayOfYear;
					DateWhere = "year(CreatedOn)=" + System.DateTime.Now.Year.ToString() + " and (datepart(\"dy\",CreatedOn)>=" + weekstartday2.ToString() + " and datepart(\"dy\",CreatedOn)<=" + weekendday2.ToString() + ")";
					RangeStartDate = weekstart2;
					RangeEndDate = weekend2;
					break;
				case "ThisMonth":
					DateWhere = "month(CreatedOn)=" + System.DateTime.Now.Month.ToString() + " and year(CreatedOn)=" + System.DateTime.Now.Year.ToString();
					RangeStartDate = Localization.ParseNativeDateTime(System.DateTime.Now.Month.ToString() + "/1/" + System.DateTime.Now.Year.ToString());
					RangeEndDate = RangeStartDate.AddMonths(1).AddDays(-1);
					break;
				case "LastMonth":
					DateWhere = "month(CreatedOn)=" + System.DateTime.Now.AddMonths(-1).Month.ToString() + " and year(CreatedOn)=" + System.DateTime.Now.AddMonths(-1).Year.ToString();
					RangeStartDate = Localization.ParseNativeDateTime(System.DateTime.Now.AddMonths(-1).Month.ToString() + "/1/" + System.DateTime.Now.AddMonths(-1).Year.ToString());
					RangeEndDate = RangeStartDate.AddMonths(1).AddDays(-1);
					break;
				case "ThisYear":
					DateWhere = "year(CreatedOn)=" + System.DateTime.Now.Year.ToString();
					RangeStartDate = Localization.ParseUSDateTime("1/1/" + System.DateTime.Now.Year.ToString());
					RangeEndDate = RangeStartDate.AddYears(1).AddDays(-1);
					if(RangeEndDate > System.DateTime.Now)
					{
						RangeEndDate = System.DateTime.Now;
					}
					break;
				case "LastYear":
					DateWhere = "year(CreatedOn)=" + System.DateTime.Now.AddYears(-1).Year.ToString();
					RangeStartDate = Localization.ParseUSDateTime("1/1/" + System.DateTime.Now.AddYears(-1).Year.ToString());
					RangeEndDate = RangeStartDate.AddYears(1).AddDays(-1);
					break;
			}
			if(DateWhere.Length != 0)
			{
				DateWhere = "(" + DateWhere + ")";
			}


			String WhereClause = DateWhere;
			String GeneralWhere = String.Empty;
			String RegOnlyWhere = String.Empty;
			if(AffiliateID != "-" && AffiliateID.Length != 0)
			{
				if(GeneralWhere.Length != 0)
				{
					GeneralWhere += " and ";
				}
				GeneralWhere += "AffiliateID=" + AffiliateID.ToString();
			}
			if(Gender != "-" && Gender.Length != 0)
			{
				if(GeneralWhere.Length != 0)
				{
					GeneralWhere += " and ";
				}
				GeneralWhere += "upper(Gender)=" + DB.SQuote(Gender.ToUpper());
			}
			if(CouponCode != "-" && CouponCode.Length != 0)
			{
				if(GeneralWhere.Length != 0)
				{
					GeneralWhere += " and ";
				}
				GeneralWhere += "upper(CouponCode)=" + DB.SQuote(CouponCode.ToUpper());
			}
			if(WithOrders == "Yes")
			{
				if(RegOnlyWhere.Length != 0)
				{
					RegOnlyWhere += " and ";
				}
				RegOnlyWhere += "customerid in (select distinct customerid from orders " + DB.GetNoLock() + " )";
			}
			if(WithOrders == "Invert")
			{
				if(RegOnlyWhere.Length != 0)
				{
					RegOnlyWhere += " and ";
				}
				RegOnlyWhere += "customerid not in (select distinct customerid from orders " + DB.GetNoLock() + " )";
			}
			if(GeneralWhere.Length != 0)
			{
				GeneralWhere = "(" + GeneralWhere + ")";
			}
			if(RegOnlyWhere.Length != 0)
			{
				RegOnlyWhere = "(" + RegOnlyWhere + ")";
			}

			if(DateWhere.Length != 0)
			{
				String sql = "select * from customer  " + DB.GetNoLock() + " where " + SuperuserFilter.ToString() + " Email not like " + DB.SQuote("Anon_%") + " " + Common.IIF(RegOnlyWhere.Length != 0 , " and " + RegOnlyWhere , "")  + Common.IIF(GeneralWhere.Length != 0 , " and " + GeneralWhere , "") + Common.IIF(WhereClause.Length != 0 , " and " + WhereClause , "") + " order by createdon desc";
				if(Common.AppConfigBool("Admin_ShowReportSQL"))
				{
					writer.Write("<p align=\"left\">SQL=" + sql + "</p>\n");
				}
				rs = DB.GetRS(sql);
				while(rs.Read())
				{
					writer.Write(DB.RSField(rs,"Email") + "<br>");
				}
				rs.Close();
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
