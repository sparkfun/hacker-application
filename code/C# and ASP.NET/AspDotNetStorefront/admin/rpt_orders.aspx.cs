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
	/// Summary description for rpt_orders.
	/// </summary>
	public class rpt_orders : SkinBase
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Reports - Orders";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			String StartDate = Common.QueryString("StartDate");
			String EndDate = Common.QueryString("EndDate");
			String AffiliateID = Common.QueryString("AffiliateID");
			String Gender = Common.QueryString("Gender");
			String GroupBy = Common.QueryString("GroupBy");
			String CouponCode = Common.QueryString("CouponCode");
			String ShippingState = Common.QueryString("ShippingState");
			String ShowExpenses = Common.QueryString("ShowExpenses");
			String EasyRange = Common.QueryString("EasyRange");
			String Day = Common.QueryString("Day");
			String Month = Common.QueryString("Month");
			String Year = Common.QueryString("Year");
			//String ShowType = Common.QueryString("ShowType");
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
			//if(ShowType.Length == 0)
			//{
			//	ShowType = "NumberOf";
			//}
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

			writer.Write("<form method=\"GET\" action=\"rpt_orders.aspx\" id=\"ReportForm\" name=\"ReportForm\" onsubmit=\"return (validateForm(this) && ReportForm_Validator(this))\">");
			writer.Write("  <table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" width=\"100%\">");
			writer.Write("    <tr>");
			writer.Write("      <td width=\"25%\" align=\"center\" bgcolor=\"#EAEAEA\"><b><font color=\"#000000\">Date Range:</font></b></td>");
			writer.Write("      <td width=\"25%\" align=\"center\" bgcolor=\"#EAEAEA\"><b><font color=\"#000000\">Order Qualifiers:</font></b></td>");
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
			//writer.Write("            <tr>");
			//writer.Write("              <td colspan=\"2\" align=\"center\" width=\"100%\"><input type=\"radio\" value=\"UseDatesBelow\" name=\"EasyRange\" " + (EasyRange == "UseDatesBelow" , "checked" , "") + ">Use Dates Below</td>");
			//writer.Write("            </tr>");
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
			writer.Write("            <td width=\"50%\">Ship To State:</td>");
			writer.Write("            <td width=\"50%\">");
			writer.Write("            <select size=\"1\" name=\"ShippingState\">");
			writer.Write("                  <option value=\"-\" " + Common.IIF(ShippingState == "" || ShippingState == "-" , "selected" , "") + ">-</option>");
			writer.Write("                      <OPTION value=\"AL\" " + Common.IIF(ShippingState == "AL" , "selected" , "") + ">AL</option>");
			writer.Write("                      <OPTION value=\"AK\" " + Common.IIF(ShippingState == "AK" , "selected" , "") + ">AK</option>");
			writer.Write("                      <OPTION value=\"AB\" " + Common.IIF(ShippingState == "AB" , "selected" , "") + ">AB</option>");
			writer.Write("                      <OPTION value=\"AS\" " + Common.IIF(ShippingState == "AS" , "selected" , "") + ">AS</option>");
			writer.Write("                      <OPTION value=\"AZ\" " + Common.IIF(ShippingState == "AZ" , "selected" , "") + ">AZ</option>");
			writer.Write("                      <OPTION value=\"AR\" " + Common.IIF(ShippingState == "AR" , "selected" , "") + ">AR</option>");
			writer.Write("                      <OPTION value=\"BC\" " + Common.IIF(ShippingState == "BC" , "selected" , "") + ">BC</option>");
			writer.Write("                      <OPTION value=\"CA\" " + Common.IIF(ShippingState == "CA" , "selected" , "") + ">CA</option>");
			writer.Write("                      <OPTION value=\"CO\" " + Common.IIF(ShippingState == "CO" , "selected" , "") + ">CO</option>");
			writer.Write("                      <OPTION value=\"CT\" " + Common.IIF(ShippingState == "CT" , "selected" , "") + ">CT</option>");
			writer.Write("                      <OPTION value=\"DE\" " + Common.IIF(ShippingState == "DE" , "selected" , "") + ">DE</option>");
			writer.Write("                      <OPTION value=\"DC\" " + Common.IIF(ShippingState == "DC" , "selected" , "") + ">DC</option>");
			writer.Write("                      <OPTION value=\"FM\" " + Common.IIF(ShippingState == "FM" , "selected" , "") + ">FM</option>");
			writer.Write("                      <OPTION value=\"FL\" " + Common.IIF(ShippingState == "FL" , "selected" , "") + ">FL</option>");
			writer.Write("                      <OPTION value=\"GA\" " + Common.IIF(ShippingState == "GA" , "selected" , "") + ">GA</option>");
			writer.Write("                      <OPTION value=\"GU\" " + Common.IIF(ShippingState == "GU" , "selected" , "") + ">GU</option>");
			writer.Write("                      <OPTION value=\"HI\" " + Common.IIF(ShippingState == "HI" , "selected" , "") + ">HI</option>");
			writer.Write("                      <OPTION value=\"ID\" " + Common.IIF(ShippingState == "ID" , "selected" , "") + ">ID</option>");
			writer.Write("                      <OPTION value=\"IL\" " + Common.IIF(ShippingState == "IL" , "selected" , "") + ">IL</option>");
			writer.Write("                      <OPTION value=\"IN\" " + Common.IIF(ShippingState == "IN" , "selected" , "") + ">IN</option>");
			writer.Write("                      <OPTION value=\"IA\" " + Common.IIF(ShippingState == "IA" , "selected" , "") + ">IA</option>");
			writer.Write("                      <OPTION value=\"KS\" " + Common.IIF(ShippingState == "KS" , "selected" , "") + ">KS</option>");
			writer.Write("                      <OPTION value=\"KY\" " + Common.IIF(ShippingState == "KY" , "selected" , "") + ">KY</option>");
			writer.Write("                      <OPTION value=\"LA\" " + Common.IIF(ShippingState == "LA" , "selected" , "") + ">LA</option>");
			writer.Write("                      <OPTION value=\"ME\" " + Common.IIF(ShippingState == "ME" , "selected" , "") + ">ME</option>");
			writer.Write("                      <OPTION value=\"MB\" " + Common.IIF(ShippingState == "MB" , "selected" , "") + ">MB</option>");
			writer.Write("                      <OPTION value=\"MH\" " + Common.IIF(ShippingState == "MH" , "selected" , "") + ">MH</option>");
			writer.Write("                      <OPTION value=\"MD\" " + Common.IIF(ShippingState == "MD" , "selected" , "") + ">MD</option>");
			writer.Write("                      <OPTION value=\"MA\" " + Common.IIF(ShippingState == "MA" , "selected" , "") + ">MA</option>");
			writer.Write("                      <OPTION value=\"MI\" " + Common.IIF(ShippingState == "MI" , "selected" , "") + ">MI</option>");
			writer.Write("                      <OPTION value=\"MN\" " + Common.IIF(ShippingState == "MN" , "selected" , "") + ">MN</option>");
			writer.Write("                      <OPTION value=\"MS\" " + Common.IIF(ShippingState == "MS" , "selected" , "") + ">MS</option>");
			writer.Write("                      <OPTION value=\"MO\" " + Common.IIF(ShippingState == "MO" , "selected" , "") + ">MO</option>");
			writer.Write("                      <OPTION value=\"MT\" " + Common.IIF(ShippingState == "MT" , "selected" , "") + ">MT</option>");
			writer.Write("                      <OPTION value=\"NE\" " + Common.IIF(ShippingState == "NE" , "selected" , "") + ">NE</option>");
			writer.Write("                      <OPTION value=\"NV\" " + Common.IIF(ShippingState == "NV" , "selected" , "") + ">NV</option>");
			writer.Write("                      <OPTION value=\"NB\" " + Common.IIF(ShippingState == "NB" , "selected" , "") + ">NB</option>");
			writer.Write("                      <OPTION value=\"NF\" " + Common.IIF(ShippingState == "NF" , "selected" , "") + ">NF</option>");
			writer.Write("                      <OPTION value=\"NH\" " + Common.IIF(ShippingState == "NH" , "selected" , "") + ">NH</option>");
			writer.Write("                      <OPTION value=\"NJ\" " + Common.IIF(ShippingState == "NJ" , "selected" , "") + ">NJ</option>");
			writer.Write("                      <OPTION value=\"NM\" " + Common.IIF(ShippingState == "NM" , "selected" , "") + ">NM</option>");
			writer.Write("                      <OPTION value=\"NY\" " + Common.IIF(ShippingState == "NY" , "selected" , "") + ">NY</option>");
			writer.Write("                      <OPTION value=\"NC\" " + Common.IIF(ShippingState == "NC" , "selected" , "") + ">NC</option>");
			writer.Write("                      <OPTION value=\"ND\" " + Common.IIF(ShippingState == "ND" , "selected" , "") + ">ND</option>");
			writer.Write("                      <OPTION value=\"MP\" " + Common.IIF(ShippingState == "MP" , "selected" , "") + ">MP</option>");
			writer.Write("                      <OPTION value=\"NT\" " + Common.IIF(ShippingState == "NT" , "selected" , "") + ">NT</option>");
			writer.Write("                      <OPTION value=\"NS\" " + Common.IIF(ShippingState == "NS" , "selected" , "") + ">NS</option>");
			writer.Write("                      <OPTION value=\"OH\" " + Common.IIF(ShippingState == "OH" , "selected" , "") + ">OH</option>");
			writer.Write("                      <OPTION value=\"OK\" " + Common.IIF(ShippingState == "OK" , "selected" , "") + ">OK</option>");
			writer.Write("                      <OPTION value=\"ON\" " + Common.IIF(ShippingState == "ON" , "selected" , "") + ">ON</option>");
			writer.Write("                      <OPTION value=\"OR\" " + Common.IIF(ShippingState == "OR" , "selected" , "") + ">OR</option>");
			writer.Write("                      <OPTION value=\"PW\" " + Common.IIF(ShippingState == "PW" , "selected" , "") + ">PW</option>");
			writer.Write("                      <OPTION value=\"PA\" " + Common.IIF(ShippingState == "PA" , "selected" , "") + ">PA</option>");
			writer.Write("                      <OPTION value=\"PE\" " + Common.IIF(ShippingState == "PE" , "selected" , "") + ">PE</option>");
			writer.Write("                      <OPTION value=\"QC\" " + Common.IIF(ShippingState == "QC" , "selected" , "") + ">QC</option>");
			writer.Write("                      <OPTION value=\"RI\" " + Common.IIF(ShippingState == "RI" , "selected" , "") + ">RI</option>");
			writer.Write("                      <OPTION value=\"SK\" " + Common.IIF(ShippingState == "SK" , "selected" , "") + ">SK</option>");
			writer.Write("                      <OPTION value=\"SC\" " + Common.IIF(ShippingState == "SC" , "selected" , "") + ">SC</option>");
			writer.Write("                      <OPTION value=\"SD\" " + Common.IIF(ShippingState == "SD" , "selected" , "") + ">SD</option>");
			writer.Write("                      <OPTION value=\"TN\" " + Common.IIF(ShippingState == "TN" , "selected" , "") + ">TN</option>");
			writer.Write("                      <OPTION value=\"TX\" " + Common.IIF(ShippingState == "TX" , "selected" , "") + ">TX</option>");
			writer.Write("                      <OPTION value=\"UT\" " + Common.IIF(ShippingState == "UT" , "selected" , "") + ">UT</option>");
			writer.Write("                      <OPTION value=\"VT\" " + Common.IIF(ShippingState == "VT" , "selected" , "") + ">VT</option>");
			writer.Write("                      <OPTION value=\"VI\" " + Common.IIF(ShippingState == "VI" , "selected" , "") + ">VI</option>");
			writer.Write("                      <OPTION value=\"VA\" " + Common.IIF(ShippingState == "VA" , "selected" , "") + ">VA</option>");
			writer.Write("                      <OPTION value=\"WA\" " + Common.IIF(ShippingState == "WA" , "selected" , "") + ">WA</option>");
			writer.Write("                      <OPTION value=\"WV\" " + Common.IIF(ShippingState == "WV" , "selected" , "") + ">WV</option>");
			writer.Write("                      <OPTION value=\"WI\" " + Common.IIF(ShippingState == "WI" , "selected" , "") + ">WI</option>");
			writer.Write("                      <OPTION value=\"WY\" " + Common.IIF(ShippingState == "WY" , "selected" , "") + ">WY</option>");
			writer.Write("                      <OPTION value=\"YT\" " + Common.IIF(ShippingState == "YT" , "selected" , "") + ">YT</option>");
			writer.Write("                      </select>");
			writer.Write("              </td>");
			writer.Write("          </tr>");

			writer.Write("          <tr>");
			writer.Write("            <td width=\"50%\">Show Expenses:</td>");
			writer.Write("            <td width=\"50%\"><select size=\"1\" name=\"ShowExpenses\">");
			writer.Write("                  <option value=\"-\" " + Common.IIF(ShowExpenses == "" || ShowExpenses == "-" , "selected" , "") + ">No</option>");
			writer.Write("                <option value=\"Yes\"" + Common.IIF(ShowExpenses == "Yes" , "selected" , "") + ">Yes</option>");
			writer.Write("              </select></td>");
			writer.Write("          </tr>");

			writer.Write("        </table>");
			//			writer.Write("          <hr size=\"1\">");
			//			writer.Write("        <table border=\"0\" cellpadding=\"4\" cellspacing=\"0\" width=\"100%\">");
			//			writer.Write("          <tr>");
			//			writer.Write("            <td width=\"100%\"> <input type=\"radio\" value=\"NumberOf\" name=\"ShowType\"  " + (ShowType == "NumberOf" || ShowType == ""? "checked" , "") + ">Number of Orders</td>");
			//			writer.Write("          </tr>");
			//			writer.Write("          <tr>");
			//			writer.Write("            <td width=\"100%\"> <input type=\"radio\" value=\"Amounts\" name=\"ShowType\" " + (ShowType == "Amounts" , "checked" , "") + ">Amounts</td>");
			//			writer.Write("          </tr>");
			//			writer.Write("        </table>");
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
			writer.Write("        <input type=\"submit\" value=\"Submit\" name=\"B1\"><input type=\"button\" onClick=\"javascript:self.location='rpt_orders.aspx';\" value=\"Reset\" name=\"B2\">");
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
					{
						GroupBy = "Month";
					}
					if(Month.Length != 0 && Month != "0")
					{
						if(DateWhere.Length != 0)
						{
							DateWhere += " and ";
						}
						DateWhere += " month(OrderDate)=" + Month + " ";
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
			if(ShowExpenses.Length != 0 && ShowExpenses != "-")
			{
				Series2Name = "Expenses";
			}
			//switch(ShowType)
			//{
			//	case "NumberOf":
			//		Series1Name = "Number Of Orders";
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
					SelectFields = "datepart(\"dy\",OrderDate) as [Day], Year(OrderDate) as [Year]";
					GroupByFields = "Year(OrderDate), datepart(\"dy\",OrderDate)";
					OrderByFields = "Year(OrderDate) asc, datepart(\"dy\",OrderDate) asc";
					DateFormat = "mm-dd-yyyy";
					GroupByIncrement = "0";
					break;
				case "Month":
					SelectFields = "month(OrderDate) as [Month], Year(OrderDate) as [Year]";
					GroupByFields = "Year(OrderDate), month(OrderDate)";
					OrderByFields = "Year(OrderDate) asc, month(OrderDate) asc";
					DateFormat = "mm-yyyy";
					GroupByIncrement = "2";
					break;
				case "Year":
					SelectFields = "Year(OrderDate) as [Year]";
					GroupByFields = "Year(OrderDate)";
					OrderByFields = "Year(OrderDate) asc";
					DateFormat = "yyyy";
					GroupByIncrement = "3";
					break;
			}

			String GeneralWhere = String.Empty;
			if(AffiliateID != "-" && AffiliateID.Length != 0)
			{
				if(GeneralWhere.Length != 0)
				{
					GeneralWhere += " and ";
				}
				GeneralWhere += "AffiliateID=" + AffiliateID;
			}
			if(Gender != "-" && Gender.Length != 0)
			{
				if(GeneralWhere.Length != 0)
				{
					GeneralWhere += " and ";
				}
				GeneralWhere += "customerid in (select distinct customerid from customer  " + DB.GetNoLock() + " where upper(Gender)=" + DB.SQuote(Gender.ToUpper()) + ")";
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
			if(GeneralWhere.Length != 0)
			{
				GeneralWhere = "(" + GeneralWhere + ")";
			}

			if(DateWhere.Length != 0)
			{
				String DS1SQL = "select count(OrderNumber) as N, Sum(OrderSubTotal) as SubTotal, Sum(OrderTotal) as Total, Sum(OrderTax) as Tax, Sum(OrderShippingCosts) as Shipping, " + SelectFields + " from orders  " + DB.GetNoLock() + " where 1=1 " + Common.IIF(GeneralWhere.Length != 0 , " and " + GeneralWhere , "") + Common.IIF(WhereClause.Length != 0 , " and " + WhereClause , "") + " group by " + GroupByFields + " order by " + OrderByFields;
				String DS2SQL = Common.IIF(ShowExpenses.Length != 0 && ShowExpenses != "-" , "select count(ExpenseID) as N, Sum(Amount) as Total, " + SelectFields + " from expense  " + DB.GetNoLock() + " where 1=1 " + Common.IIF(WhereClause.Length != 0 , " and " + WhereClause , "") + " group by " + GroupByFields + " order by " + OrderByFields , "select * from expense  " + DB.GetNoLock() + " where 1=0");
				DS2SQL = DS2SQL.Replace("OrderDate","ExpenseDate");
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
						decimal DS1SumSubTotal = System.Decimal.Zero;
						decimal DS1SumTotal = System.Decimal.Zero;
						decimal DS1SumTax = System.Decimal.Zero;
						decimal DS1SumShipping = System.Decimal.Zero;
						int DS2SumN = 0;
						decimal DS2SumSubTotal = System.Decimal.Zero;
						decimal DS2SumTotal = System.Decimal.Zero;
						decimal DS2SumTax = System.Decimal.Zero;
						decimal DS2SumShipping = System.Decimal.Zero;
						int DS1NumRecs = ds1.Tables[0].Rows.Count;
						int DS2NumRecs = ds2.Tables[0].Rows.Count;
						int MaxNumRecs = Common.Max(DS1NumRecs,DS2NumRecs);
						foreach(DataRow row in ds1.Tables[0].Rows)
						{
							DS1SumN += DB.RowFieldInt(row,"N");
							DS1SumSubTotal += DB.RowFieldDecimal(row,"SubTotal");
							DS1SumTotal += DB.RowFieldDecimal(row,"Total");
							DS1SumTax += DB.RowFieldDecimal(row,"Tax");
							DS1SumShipping += DB.RowFieldDecimal(row,"Shipping");
						}
						foreach(DataRow row in ds2.Tables[0].Rows)
						{
							DS2SumN += DB.RowFieldInt(row,"N");
							DS2SumSubTotal += DB.RowFieldDecimal(row,"SubTotal");
							DS2SumTotal += DB.RowFieldDecimal(row,"Total");
							DS2SumTax += DB.RowFieldDecimal(row,"Tax");
							DS2SumShipping += DB.RowFieldDecimal(row,"Shipping");
						}
						// set range start date, if necessary:
						IDataReader rsd = DB.GetRS("select min(OrderDate) as RangeStartDate from orders " + DB.GetNoLock() + " ");
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
						String DS1ValuesSubTotal = String.Empty;
						String DS1ValuesTotal = String.Empty;
						String DS1ValuesTax = String.Empty;
						String DS1ValuesShipping = String.Empty;

						String DS2Dates = String.Empty;
						String DS2ValuesN = String.Empty;
						String DS2ValuesSubTotal = String.Empty;
						String DS2ValuesTotal = String.Empty;
						String DS2ValuesTax = String.Empty;
						String DS2ValuesShipping = String.Empty;

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
						//						if(Series2Name.Length != 0 && ChartType.ToLower().IndexOf("stack") == -1)
						//						{
						//							BarWidth = BarWidth / 2;
						//						}

						// COMPOSE FULL DATE and RANGE and SUM SERIES:
						int ds1_idx = 0;
						int ds2_idx = 0;
						int[] SumsN = new int[NumBuckets];
						decimal[] SumsSubTotal = new decimal[NumBuckets];
						decimal[] SumsTotal = new decimal[NumBuckets];
						decimal[] SumsTax = new decimal[NumBuckets];
						decimal[] SumsShipping = new decimal[NumBuckets];
						for(int i = SumsN.GetLowerBound(0); i <= SumsN.GetUpperBound(0); i++)
						{
							SumsN[i] = 0;
							SumsSubTotal[i] = System.Decimal.Zero;
							SumsTotal[i] = System.Decimal.Zero;
							SumsTax[i] = System.Decimal.Zero;
							SumsShipping[i] = System.Decimal.Zero;
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
										DS1ValuesSubTotal += "|";
										DS1ValuesTotal += "|";
										DS1ValuesTax += "|";
										DS1ValuesShipping += "|";
										DS2ValuesN += "|";
										DS2ValuesSubTotal += "|";
										DS2ValuesTotal += "|";
										DS2ValuesTax += "|";
										DS2ValuesShipping += "|";
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
											DS1ValuesSubTotal += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds1Row,"SubTotal"));
											DS1ValuesTotal += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds1Row,"Total"));
											DS1ValuesTax += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds1Row,"Tax"));
											DS1ValuesShipping += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds1Row,"Shipping"));
											SumsN[SumBucketIdx] += DB.RowFieldInt(ds1Row,"N");
											SumsSubTotal[SumBucketIdx] += DB.RowFieldDecimal(ds1Row,"SubTotal");
											SumsTotal[SumBucketIdx] += DB.RowFieldDecimal(ds1Row,"Total");
											SumsTax[SumBucketIdx] += DB.RowFieldDecimal(ds1Row,"Tax");
											SumsShipping[SumBucketIdx] += DB.RowFieldDecimal(ds1Row,"Shipping");
											ds1_idx++;
										}
										else
										{
											DS1ValuesN += "0";
											DS1ValuesSubTotal += "0.0";
											DS1ValuesTotal += "0.0";
											DS1ValuesTax += "0.0";
											DS1ValuesShipping += "0.0";
										}
									}
									else
									{
										DS1ValuesN += "0";
										DS1ValuesSubTotal += "0.0";
										DS1ValuesTotal += "0.0";
										DS1ValuesTax += "0.0";
										DS1ValuesShipping += "0.0";
									}
									if(Series2Name.Length != 0 && ds2_idx < DS2NumRecs)
									{
										DataRow ds2Row = ds2.Tables[0].Rows[ds2_idx];
										int dy2 = DB.RowFieldInt(ds2Row,"Day");
										DateTime dt2 = Localization.ParseUSDateTime("1/1/" + DB.RowField(ds2Row,"Year")).AddDays(dy2-1);
										if(dt2.Month == yy.Month && dt2.Day == yy.Day && dt2.Year == yy.Year)
										{
											DS2ValuesN += DB.RowFieldInt(ds2Row,"N");
											DS2ValuesSubTotal += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds2Row,"SubTotal"));
											DS2ValuesTotal += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds2Row,"Total"));
											DS2ValuesTax += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds2Row,"Tax"));
											DS2ValuesShipping += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds2Row,"Shipping"));
											SumsN[SumBucketIdx] += DB.RowFieldInt(ds2Row,"N");
											SumsSubTotal[SumBucketIdx] += DB.RowFieldDecimal(ds2Row,"SubTotal");
											SumsTotal[SumBucketIdx] += DB.RowFieldDecimal(ds2Row,"Total");
											SumsTax[SumBucketIdx] += DB.RowFieldDecimal(ds2Row,"Tax");
											SumsShipping[SumBucketIdx] += DB.RowFieldDecimal(ds2Row,"Shipping");
											ds2_idx++;
										}
										else
										{
											DS2ValuesN += "0";
											DS2ValuesSubTotal += "0.0";
											DS2ValuesTotal += "0.0";
											DS2ValuesTax += "0.0";
											DS2ValuesShipping += "0.0";
										}
									}
									else
									{
										DS2ValuesN += "0";
										DS2ValuesSubTotal += "0.0";
										DS2ValuesTotal += "0.0";
										DS2ValuesTax += "0.0";
										DS2ValuesShipping += "0.0";
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
										DS1ValuesSubTotal += "|";
										DS1ValuesTotal += "|";
										DS1ValuesTax += "|";
										DS1ValuesShipping += "|";
										DS2ValuesN += "|";
										DS2ValuesSubTotal += "|";
										DS2ValuesTotal += "|";
										DS2ValuesTax += "|";
										DS2ValuesShipping += "|";
									}
									DateSeries += yy.Month.ToString() + "-" + yy.Year.ToString();
									if(ds1_idx < DS1NumRecs)
									{
										DataRow ds1Row = ds1.Tables[0].Rows[ds1_idx];
										DateTime dt1 = Localization.ParseNativeDateTime(DB.RowField(ds1Row,"Month").ToString() + "/1/" + DB.RowField(ds1Row,"Year").ToUpper());
										if(dt1.Month == yy.Month && dt1.Year == yy.Year)
										{
											DS1ValuesN += DB.RowFieldInt(ds1Row,"N").ToString();
											DS1ValuesSubTotal += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds1Row,"SubTotal"));
											DS1ValuesTotal += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds1Row,"Total"));
											DS1ValuesTax += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds1Row,"Tax"));
											DS1ValuesShipping += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds1Row,"Shipping"));
											SumsN[SumBucketIdx] += DB.RowFieldInt(ds1Row,"N");
											SumsSubTotal[SumBucketIdx] += DB.RowFieldDecimal(ds1Row,"SubTotal");
											SumsTotal[SumBucketIdx] += DB.RowFieldDecimal(ds1Row,"Total");
											SumsTax[SumBucketIdx] += DB.RowFieldDecimal(ds1Row,"Tax");
											SumsShipping[SumBucketIdx] += DB.RowFieldDecimal(ds1Row,"Shipping");
											ds1_idx++;
										}
										else
										{
											DS1ValuesN += "0";
											DS1ValuesSubTotal += "0.0";
											DS1ValuesTotal += "0.0";
											DS1ValuesTax += "0.0";
											DS1ValuesShipping += "0.0";
										}
									}
									else
									{
										DS1ValuesN += "0";
										DS1ValuesSubTotal += "0.0";
										DS1ValuesTotal += "0.0";
										DS1ValuesTax += "0.0";
										DS1ValuesShipping += "0.0";
									}
									if(Series2Name.Length != 0 && ds2_idx < DS2NumRecs)
									{
										DataRow ds2Row = ds2.Tables[0].Rows[ds2_idx];
										DateTime dt2 = Localization.ParseNativeDateTime(DB.RowField(ds2Row,"Month").ToString() + "/1/" + DB.RowField(ds2Row,"Year").ToUpper());
										if(dt2.Month == yy.Month && dt2.Year == yy.Year)
										{
											DS2ValuesN += DB.RowFieldInt(ds2Row,"N").ToString();
											DS2ValuesSubTotal += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds2Row,"SubTotal"));
											DS2ValuesTotal += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds2Row,"Total"));
											DS2ValuesTax += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds2Row,"Tax"));
											DS2ValuesShipping += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds2Row,"Shipping"));
											SumsN[SumBucketIdx] += DB.RowFieldInt(ds2Row,"N");
											SumsSubTotal[SumBucketIdx] += DB.RowFieldDecimal(ds2Row,"SubTotal");
											SumsTotal[SumBucketIdx] += DB.RowFieldDecimal(ds2Row,"Total");
											SumsTax[SumBucketIdx] += DB.RowFieldDecimal(ds2Row,"Tax");
											SumsShipping[SumBucketIdx] += DB.RowFieldDecimal(ds2Row,"Shipping");
											ds2_idx++;
										}
										else
										{
											DS2ValuesN += "0";
											DS2ValuesSubTotal += "0.0";
											DS2ValuesTotal += "0.0";
											DS2ValuesTax += "0.0";
											DS2ValuesShipping += "0.0";
										}
									}
									else
									{
										DS2ValuesN += "0";
										DS2ValuesSubTotal += "0.0";
										DS2ValuesTotal += "0.0";
										DS2ValuesTax += "0.0";
										DS2ValuesShipping += "0.0";
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
										DS1ValuesSubTotal += "|";
										DS1ValuesTotal += "|";
										DS1ValuesTax += "|";
										DS1ValuesShipping += "|";
										DS2ValuesN += "|";
										DS2ValuesSubTotal += "|";
										DS2ValuesTotal += "|";
										DS2ValuesTax += "|";
										DS2ValuesShipping += "|";
									}
									DateSeries += yy.Year.ToString();
									if(ds1_idx < DS1NumRecs)
									{
										DataRow ds1Row = ds1.Tables[0].Rows[ds1_idx];
										DateTime dt1 = Localization.ParseUSDateTime("1/1/" + DB.RowField(ds1Row,"Year").ToUpper());
										if(dt1.Year == yy.Year)
										{
											DS1ValuesN += DB.RowFieldInt(ds1Row,"N").ToString();
											DS1ValuesSubTotal += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds1Row,"SubTotal"));
											DS1ValuesTotal += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds1Row,"Total"));
											DS1ValuesTax += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds1Row,"Tax"));
											DS1ValuesShipping += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds1Row,"Shipping"));
											SumsN[SumBucketIdx] += DB.RowFieldInt(ds1Row,"N");
											SumsSubTotal[SumBucketIdx] += DB.RowFieldDecimal(ds1Row,"SubTotal");
											SumsTotal[SumBucketIdx] += DB.RowFieldDecimal(ds1Row,"Total");
											SumsTax[SumBucketIdx] += DB.RowFieldDecimal(ds1Row,"Tax");
											SumsShipping[SumBucketIdx] += DB.RowFieldDecimal(ds1Row,"Shipping");
											ds1_idx++;
										}
										else
										{
											DS1ValuesN += "0";
											DS1ValuesSubTotal += "0.0";
											DS1ValuesTotal += "0.0";
											DS1ValuesTax += "0.0";
											DS1ValuesShipping += "0.0";
										}
									}
									else
									{
										DS1ValuesN += "0";
										DS1ValuesSubTotal += "0.0";
										DS1ValuesTotal += "0.0";
										DS1ValuesTax += "0.0";
										DS1ValuesShipping += "0.0";
									}
									if(Series2Name.Length != 0 && ds2_idx < DS2NumRecs)
									{
										DataRow ds2Row = ds2.Tables[0].Rows[ds2_idx];
										DateTime dt2 = Localization.ParseUSDateTime("1/1/" + DB.RowField(ds2Row,"Year").ToUpper());
										if(dt2.Year == yy.Year)
										{
											DS2ValuesN += DB.RowFieldInt(ds2Row,"N").ToString();
											DS2ValuesSubTotal += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds2Row,"SubTotal"));
											DS2ValuesTotal += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds2Row,"Total"));
											DS2ValuesTax += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds2Row,"Tax"));
											DS2ValuesShipping += Localization.CurrencyStringForDB(DB.RowFieldDecimal(ds2Row,"Shipping"));
											SumsN[SumBucketIdx] += DB.RowFieldInt(ds2Row,"N");
											SumsSubTotal[SumBucketIdx] += DB.RowFieldDecimal(ds2Row,"SubTotal");
											SumsTotal[SumBucketIdx] += DB.RowFieldDecimal(ds2Row,"Total");
											SumsTax[SumBucketIdx] += DB.RowFieldDecimal(ds2Row,"Tax");
											SumsShipping[SumBucketIdx] += DB.RowFieldDecimal(ds2Row,"Shipping");
											ds2_idx++;
										}
										else
										{
											DS2ValuesN += "0";
											DS2ValuesSubTotal += "0.0";
											DS2ValuesTotal += "0.0";
											DS2ValuesTax += "0.0";
											DS2ValuesShipping += "0.0";
										}
									}
									else
									{
										DS2ValuesN += "0";
										DS2ValuesSubTotal += "0.0";
										DS2ValuesTotal += "0.0";
										DS2ValuesTax += "0.0";
										DS2ValuesShipping += "0.0";
									}									
									SumBucketIdx++;
								}
								break;
						}

						writer.Write("<p align=\"left\"><b>Number of Orders: " + DS1SumN.ToString()+ "</b></p>\n");
						//						switch(ShowType)
						//						{
						//							case "NumberOf":
						//								writer.Write("<p align=\"left\"><b>Number of Orders: " + DS1SumN + "</b></p>\n");
						//								break;
						//							case "Amounts":
						//								writer.Write("<p align=\"left\"><b>Number of Orders: " + DS1SumN + "</b></p>\n");
						//								break;
						//						}

						if(DS1SumN > 0 || DS2SumN > 0)
						{
							String ReportTitle = "Orders Report|" + Localization.ToNativeShortDateString(RangeStartDate) + " - " + Localization.ToNativeShortDateString(RangeEndDate) + "|Group By: " + GroupBy;
							String ReportTitleN = "Number Of Orders Report|" + Localization.ToNativeShortDateString(RangeStartDate) + " - " + Localization.ToNativeShortDateString(RangeEndDate) + "|Group By: " + GroupBy;
							String ReportTitleSubTotal = "Order SubTotal Report|Sum=" + Localization.CurrencyStringForDB(DS1SumSubTotal) + "|" + Localization.ToNativeShortDateString(RangeStartDate) + " - " + Localization.ToNativeShortDateString(RangeEndDate) + "|Group By: " + GroupBy;
							String ReportTitleTotal = "Order Total Report|Sum=" + Localization.CurrencyStringForDB(DS1SumTotal) + "|" + Localization.ToNativeShortDateString(RangeStartDate) + " - " + Localization.ToNativeShortDateString(RangeEndDate) + "|Group By: " + GroupBy;
							String ReportTitleTax = "Tax Report|Sum=" + Localization.CurrencyStringForDB(DS1SumTax) + "|" + Localization.ToNativeShortDateString(RangeStartDate) + " - " + Localization.ToNativeShortDateString(RangeEndDate) + "|Group By: " + GroupBy;
							String ReportTitleShipping = "Shipping Report|Sum=" + Localization.CurrencyStringForDB(DS1SumShipping) + "|" + Localization.ToNativeShortDateString(RangeStartDate) + " - " + Localization.ToNativeShortDateString(RangeEndDate) + "|Group By: " + GroupBy;
							String ReportTitleExpenseN = "Number Of Expenses Report|" + Localization.ToNativeShortDateString(RangeStartDate) + " - " + Localization.ToNativeShortDateString(RangeEndDate) + "|Group By: " + GroupBy;
							String ReportTitleExpenseTotal = "Expense Total Report|Sum=" + Localization.CurrencyStringForDB(DS2SumTotal) + "|" + Localization.ToNativeShortDateString(RangeStartDate) + " - " + Localization.ToNativeShortDateString(RangeEndDate) + "|Group By: " + GroupBy;
							String ReportTitlePL = "Net Profit/Loss Report|Sum=" + Localization.CurrencyStringForDB(DS1SumTotal - DS2SumTotal) + "|" + Localization.ToNativeShortDateString(RangeStartDate) + " - " + Localization.ToNativeShortDateString(RangeEndDate) + "|Group By: " + GroupBy;
							if(ReportType == "Chart")
							{
								// Number of Orders Chart:
								Series1Name = "Number of Orders";
								writer.Write(Common.GetChart(ReportTitleN,"Date","# Orders","100%","500",ChartIs3D,ChartTypeSpec,Series1Name,String.Empty,DateSeries,DS1ValuesN,String.Empty));
								writer.Write("<br><br>");

								// Total Chart:
								Series1Name = "Order Totals";
								writer.Write(Common.GetChart(ReportTitleTotal,"Date","Totals","100%","500",ChartIs3D,ChartTypeSpec,Series1Name,String.Empty,DateSeries,DS1ValuesTotal,String.Empty));
								writer.Write("<br><br>");

								// SubTotal Chart:
								Series1Name = "Order Subtotals";
								writer.Write(Common.GetChart(ReportTitleSubTotal,"Date","SubTotals","100%","500",ChartIs3D,ChartTypeSpec,Series1Name,String.Empty,DateSeries,DS1ValuesSubTotal,String.Empty));
								writer.Write("<br><br>");

								// Tax Chart:
								Series1Name = "Tax Totals";
								writer.Write(Common.GetChart(ReportTitleTax,"Date","TaxTotals","100%","500",ChartIs3D,ChartTypeSpec,Series1Name,String.Empty,DateSeries,DS1ValuesTax,String.Empty));
								writer.Write("<br><br>");

								// Shipping Chart:
								Series1Name = "Shipping Totals";
								writer.Write(Common.GetChart(ReportTitleShipping,"Date","ShippingTotals","100%","500",ChartIs3D,ChartTypeSpec,Series1Name,String.Empty,DateSeries,DS1ValuesShipping,String.Empty));
								writer.Write("<br><br>");

								if(ShowExpenses.Length != 0 && ShowExpenses != "-")
								{
									// Number of Expenses Chart:
									Series1Name = "Number of Expenses";
									writer.Write(Common.GetChart(ReportTitleExpenseN,"Date","# Expenses","100%","500",ChartIs3D,ChartTypeSpec,Series1Name,String.Empty,DateSeries,DS2ValuesN,String.Empty));
									writer.Write("<br><br>");

									// Expense Total Chart:
									Series1Name = "Expenses";
									writer.Write(Common.GetChart(ReportTitleExpenseTotal,"Date","Expense Totals","100%","500",ChartIs3D,ChartTypeSpec,Series1Name,String.Empty,DateSeries,DS2ValuesTotal,String.Empty));
									writer.Write("<br><br>");

									// Net P/L Chart:

									// build net sum series:
									String NetValues = String.Empty;
									String[] S1Total = DS1ValuesTotal.Split('|');
									String[] S2Total = DS2ValuesTotal.Split('|');
									for(int row = 0; row < NumBuckets; row++)
									{
										if(NetValues.Length != 0)
										{
											NetValues += "|";
										}
										decimal NetAmount = System.Decimal.Parse(S1Total[row]) - System.Decimal.Parse(S2Total[row]);
										if(NetAmount < 0)
										{
											NetValues += "-" + Localization.CurrencyStringForDB( -NetAmount).Replace("$","");
										}
										else
										{
											NetValues += Localization.CurrencyStringForDB( NetAmount).Replace("$","");
										}
									}

									Series1Name = "Net P/L";
									writer.Write(Common.GetChart(ReportTitlePL,"Date","Net P/L","100%","500",ChartIs3D,ChartTypeSpec,Series1Name,String.Empty,DateSeries,NetValues,String.Empty));

									writer.Write("<br><br>");
								}

							}
							else
							{
								// WRITE OUT THE TABLE:
								String[] DD = DateSeries.Split('|');

								String[] S1N = DS1ValuesN.Split('|');
								String[] S1SubTotal = DS1ValuesSubTotal.Split('|');
								String[] S1Total = DS1ValuesTotal.Split('|');
								String[] S1Tax = DS1ValuesTax.Split('|');
								String[] S1Shipping = DS1ValuesShipping.Split('|');
								
								String[] S2N = DS2ValuesN.Split('|');
								String[] S2SubTotal = DS2ValuesSubTotal.Split('|');
								String[] S2Total = DS2ValuesTotal.Split('|');
								String[] S2Tax = DS2ValuesTax.Split('|');
								String[] S2Shipping = DS2ValuesShipping.Split('|');

								if(NumBuckets > 60)
								{
									// VERTICAL:
									writer.Write("<p align=\"center\"><b>" + ReportTitle + "</b></p>\n");
									writer.Write("<table border=\"1\" cellpadding=\"4\" cellspacing=\"0\">\n");
									writer.Write("  <tr>\n");
									writer.Write("    <td bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>Date</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b># Orders</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>SubTotal</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>Tax</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>Shipping</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>Total</b></td>\n");
									if(Series2Name.Length != 0)
									{
										writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>Expenses</b></td>\n");
										writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>Net Profit/Loss</b></td>\n");
									}
									writer.Write("  </tr>\n");

									writer.Write("  <tr>\n");
									writer.Write("    <td bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>Total</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + DS1SumN.ToString() + "</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + Localization.CurrencyStringForDB( DS1SumSubTotal) + "</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + Localization.CurrencyStringForDB( DS1SumTax) + "</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + Localization.CurrencyStringForDB( DS1SumShipping) + "</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + Localization.CurrencyStringForDB( DS1SumTotal) + "</b></td>\n");
									if(Series2Name.Length != 0)
									{
										writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + Localization.CurrencyStringForDB( DS2SumTotal) + "</b></td>\n");
										writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + Localization.CurrencyStringForDB( DS1SumTotal - DS2SumTotal) + "</b></td>\n");
									}
									writer.Write("  </tr>\n");
									
									for(int row = 0; row < NumBuckets; row++)
									{
										writer.Write("  <tr>\n");
										writer.Write("    <td>" + DD[row] + "</td>\n");
										writer.Write("    <td align=\"center\" >" + Common.IIF(S1N[row] == "0" , "&nbsp;" , S1N[row]) + "</td>\n");
										writer.Write("    <td align=\"center\" >" + Common.IIF(S1N[row] == "0" , "&nbsp;" , S1SubTotal[row]) + "</td>\n");
										writer.Write("    <td align=\"center\" >" + Common.IIF(S1N[row] == "0" , "&nbsp;" , S1Tax[row]) + "</td>\n");
										writer.Write("    <td align=\"center\" >" + Common.IIF(S1N[row] == "0" , "&nbsp;" , S1Shipping[row]) + "</td>\n");
										writer.Write("    <td align=\"center\" >" + Common.IIF(S1N[row] == "0" , "&nbsp;" , S1Total[row]) + "</td>\n");
										if(Series2Name.Length != 0)
										{
											writer.Write("    <td align=\"center\" >" + Common.IIF(S2N[row] == "0" , "&nbsp;" , S2Total[row]) + "</td>\n");
											writer.Write("    <td align=\"center\" >" + Common.IIF(S2N[row] == "0" , "&nbsp;" , Localization.CurrencyStringForDB( System.Decimal.Parse(S1Total[row]) - System.Decimal.Parse(S2Total[row]))) + "</td>\n");
										}
										writer.Write("  </tr>\n");
									}
									writer.Write("  <tr>\n");
									writer.Write("    <td bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>Total</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + DS1SumN.ToString() + "</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + Localization.CurrencyStringForDB( DS1SumSubTotal) + "</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + Localization.CurrencyStringForDB( DS1SumTax) + "</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + Localization.CurrencyStringForDB( DS1SumShipping) + "</b></td>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + Localization.CurrencyStringForDB( DS1SumTotal) + "</b></td>\n");
									if(Series2Name.Length != 0)
									{
										writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + Localization.CurrencyStringForDB( DS2SumTotal) + "</b></td>\n");
										writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>" + Localization.CurrencyStringForDB( DS1SumTotal - DS2SumTotal) + "</b></td>\n");
									}
									writer.Write("  </tr>\n");
									writer.Write("</table>\n");
								}
								else
								{
									// HORIZONTAL:

									// Number Of Orders Table:
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

									// Number of Orders
									writer.Write("  <tr>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b># Orders</b></td>\n");
									for(int row = 0; row < NumBuckets; row++)
									{
										writer.Write("    <td align=\"center\" >" + Common.IIF(S1N[row] == "0" , "&nbsp;" , S1N[row]) + "</td>\n");
									}
									writer.Write("    <td align=\"center\" bgcolor=\"#FFFFCC\"><b>" + DS1SumN.ToString() + "</b></td>\n");
									writer.Write("  </tr>\n");

									// SubTotals
									writer.Write("  <tr>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>SubTotal</b></td>\n");
									for(int row = 0; row < NumBuckets; row++)
									{
										writer.Write("    <td align=\"center\" >" + Common.IIF(S1SubTotal[row] == "0" , "&nbsp;" , S1SubTotal[row]) + "</td>\n");
									}
									writer.Write("    <td align=\"center\" bgcolor=\"#FFFFCC\"><b>" + Localization.CurrencyStringForDB( DS1SumSubTotal) + "</b></td>\n");
									writer.Write("  </tr>\n");

									// Tax
									writer.Write("  <tr>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>Tax</b></td>\n");
									for(int row = 0; row < NumBuckets; row++)
									{
										writer.Write("    <td align=\"center\" >" + Common.IIF(S1Tax[row] == "0" , "&nbsp;" , S1Tax[row]) + "</td>\n");
									}
									writer.Write("    <td align=\"center\" bgcolor=\"#FFFFCC\"><b>" + Localization.CurrencyStringForDB( DS1SumTax) + "</b></td>\n");
									writer.Write("  </tr>\n");

									// Shipping
									writer.Write("  <tr>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>Shipping</b></td>\n");
									for(int row = 0; row < NumBuckets; row++)
									{
										writer.Write("    <td align=\"center\" >" + Common.IIF(S1Shipping[row] == "0" , "&nbsp;" , S1Shipping[row]) + "</td>\n");
									}
									writer.Write("    <td align=\"center\" bgcolor=\"#FFFFCC\"><b>" + Localization.CurrencyStringForDB( DS1SumShipping) + "</b></td>\n");
									writer.Write("  </tr>\n");

									// Totals
									writer.Write("  <tr>\n");
									writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>Totals</b></td>\n");
									for(int row = 0; row < NumBuckets; row++)
									{
										writer.Write("    <td align=\"center\" >" + Common.IIF(S1Total[row] == "0" , "&nbsp;" , S1Total[row]) + "</td>\n");
									}
									writer.Write("    <td align=\"center\" bgcolor=\"#FFFFCC\"><b>" + Localization.CurrencyStringForDB( DS1SumTotal) + "</b></td>\n");
									writer.Write("  </tr>\n");

									if(Series2Name.Length != 0)
									{
										// Expense Totals
										writer.Write("  <tr>\n");
										writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>Expenses</b></td>\n");
										for(int row = 0; row < NumBuckets; row++)
										{
											writer.Write("    <td align=\"center\" >" + Common.IIF(S2Total[row] == "0" , "&nbsp;" , S2Total[row]) + "</td>\n");
										}
										writer.Write("    <td align=\"center\" bgcolor=\"#FFFFCC\"><b>" + Localization.CurrencyStringForDB( DS2SumTotal) + "</b></td>\n");
										writer.Write("  </tr>\n");

										// Net Profit/Loss Totals
										writer.Write("  <tr>\n");
										writer.Write("    <td align=\"center\" bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\"><b>Net Profit/Loss</b></td>\n");
										for(int row = 0; row < NumBuckets; row++)
										{
											writer.Write("    <td align=\"center\" >" + Common.IIF(S1Total[row] == "0"  && S2Total[row] == "0" , "&nbsp;" , Localization.CurrencyStringForDB( System.Decimal.Parse(S1Total[row]) - System.Decimal.Parse(S2Total[row]))) + "</td>\n");
										}
										writer.Write("    <td align=\"center\" bgcolor=\"#FFFFCC\"><b>" + Localization.CurrencyStringForDB( DS1SumTotal - DS2SumTotal) + "</b></td>\n");
										writer.Write("  </tr>\n");

									}

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
