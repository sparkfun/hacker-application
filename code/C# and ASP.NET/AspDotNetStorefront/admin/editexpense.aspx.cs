// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Web;
using System.IO;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for editexpense
	/// </summary>
	public class editexpense : SkinBase
	{
		
		int ExpenseID;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			ExpenseID = 0;

			if(Common.QueryString("ExpenseID").Length != 0 && Common.QueryString("ExpenseID") != "0") 
			{
				Editing = true;
				ExpenseID = Localization.ParseUSInt(Common.QueryString("ExpenseID"));
			} 
			else 
			{
				Editing = false;
			}

			
			IDataReader rs;
			

			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				if(ErrorMsg.Length == 0)
				{
			
					try
					{
						StringBuilder sql = new StringBuilder(2500);
						if(!Editing)
						{
							// ok to add them:
							String NewGUID = DB.GetNewGUID();
							sql.Append("insert into expense(ExpenseGUID,Description,ExpenseCategoryID,ExpenseDate,Amount,LastUpdatedBy) values(");
							sql.Append(DB.SQuote(NewGUID) + ",");
							sql.Append(DB.SQuote(Common.Left(Common.Form("Description"),5000)) + ",");
							sql.Append(Common.Form("ExpenseCategoryID") + ",");
							sql.Append(DB.SQuote(Common.Form("ExpenseDate")) + ",");
							sql.Append(Localization.CurrencyStringForDB(Common.FormUSDecimal("Amount")) + ",");
							sql.Append(thisCustomer._customerID);
							sql.Append(")");
							DB.ExecuteSQL(sql.ToString());

							rs = DB.GetRS("select ExpenseID from expense  " + DB.GetNoLock() + " where ExpenseGUID=" + DB.SQuote(NewGUID));
							rs.Read();
							ExpenseID = DB.RSFieldInt(rs,"ExpenseID");
							Editing = true;
							rs.Close();
							DataUpdated = true;
						}
						else
						{
							// ok to update:
							sql.Append("update expense set ");
							sql.Append("Description=" + DB.SQuote(Common.Left(Common.Form("Description"),5000)) + ",");
							sql.Append("ExpenseDate=" + DB.SQuote(Common.Form("ExpenseDate")) + ",");
							sql.Append("ExpenseCategoryID=" + Common.Form("ExpenseCategoryID") + ",");
							sql.Append("Amount=" + Localization.CurrencyStringForDB(Common.FormUSDecimal("Amount")) + ",");
							sql.Append("LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
							sql.Append("LastUpdatedBy=" + thisCustomer._customerID.ToString() + " ");
							sql.Append("where ExpenseID=" + ExpenseID.ToString());
							DB.ExecuteSQL(sql.ToString());
							DataUpdated = true;
							Editing = true;
						}
					}
					catch(Exception ex)
					{
						ErrorMsg = "<p><b>ERROR: " + Common.GetExceptionDetail(ex,"<br>") + "<br><br></b></p>";
					}
				}
			}
			SectionTitle = "<a href=\"expenses.aspx\">Expenses</a> - Manage Expenses";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			IDataReader rs = DB.GetRS("select * from Expense  " + DB.GetNoLock() + " where ExpenseID=" + ExpenseID.ToString());
			if(rs.Read())
			{
				Editing = true;
			}
			

			if(ErrorMsg.Length != 0)
			{
				writer.Write("<p><b><font color=red>" + ErrorMsg + "</font></b></p>\n");
			}
			if(DataUpdated)
			{
				writer.Write("<p align=\"left\"><b><font color=blue>(UPDATED)</font></b></p>\n");
			}

			int ExpenseCategoryID = Common.QueryStringUSInt("ExpenseCategoryID");
			if(Editing)
			{
				ExpenseCategoryID = DB.RSFieldInt(rs,"ExpenseCategoryID");
			}

			if(ErrorMsg.Length == 0)
			{

				if(Editing)
				{
					writer.Write("<p align=\"left\"><b>Editing Expense: " + DB.RSFieldInt(rs,"ExpenseID").ToString() + "</b></p>\n");
				}
				else
				{
					writer.Write("<p align=\"left\"><b>Adding New Expense:</p></b>\n");
				}

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function Form_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("submitonce(theForm);\n");
				writer.Write("if (theForm.ExpenseCategoryID.selectedIndex < 1)\n");
				writer.Write("{\n");
				writer.Write("alert(\"Please select an expense category.\");\n");
				writer.Write("theForm.ExpenseCategoryID.focus();\n");
				writer.Write("submitenabled(theForm);\n");
				writer.Write("return (false);\n");
				writer.Write("    }\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				writer.Write("<p align=\"left\">Please enter the following information about this expense. Fields marked with an asterisk (*) are required. All other fields are optional.</p>\n");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\">\n");
				writer.Write("<form action=\"editexpense.aspx?ExpenseID=" + ExpenseID.ToString() + "&edit=" + Editing.ToString() + "&ExpenseCategoryID=" + ExpenseCategoryID.ToString() + "\" method=\"post\" id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
				writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
				writer.Write("<tr valign=\"middle\">\n");
				writer.Write("<td width=\"100%\" colspan=\"2\" align=\"left\">\n");
				writer.Write("</td>\n");
				writer.Write("</tr>\n");

				writer.Write("<tr>\n");
				writer.Write("<td></td><td align=\"left\"><br>\n");
				if(Editing) 
				{
					writer.Write("<input type=\"submit\" value=\"Update\" name=\"submit\">\n");
					writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"reset\" value=\"Reset\" name=\"reset\">\n");
					writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"button\" onClick=\"self.location='editexpense.aspx?ExpenseCategoryID=" + ExpenseCategoryID.ToString() + "';\" value=\"Add Another Expense\" name=\"another\">\n");
				} 
				else 
				{
					writer.Write("<input type=\"submit\" value=\"Add\" name=\"submit\">\n");
				}
				writer.Write("</td>\n");
				writer.Write("</tr>\n");
				
				writer.Write("<tr valign=\"middle\">\n");
				writer.Write("<td align=\"right\" valign=\"middle\">*Expense Category:&nbsp;&nbsp;</td>\n");
				writer.Write("<td align=\"left\">\n");
				writer.Write("<select size=\"1\" name=\"ExpenseCategoryID\">\n");
				writer.Write(" <OPTION VALUE=\"0\">SELECT ONE</option>\n");
				IDataReader rsst = DB.GetRS("select * from ExpenseCategory  " + DB.GetNoLock() + " order by name");
				while(rsst.Read())
				{
					writer.Write("<option value=\"" + DB.RSFieldInt(rsst,"ExpenseCategoryID").ToString() + "\"");
					if(Editing) 
					{
						if(DB.RSFieldInt(rs,"ExpenseCategoryID") == DB.RSFieldInt(rsst,"ExpenseCategoryID") || ExpenseCategoryID == DB.RSFieldInt(rsst,"ExpenseCategoryID"))
						{
							writer.Write(" selected");
						}
					}
					else if(ExpenseCategoryID == DB.RSFieldInt(rsst,"ExpenseCategoryID"))
					{
						writer.Write(" selected");
					}
					writer.Write(">" + DB.RSField(rsst,"Name") + "</option>");
				}
				rsst.Close();
				writer.Write("</select>\n");
				writer.Write("</td>\n");
				writer.Write("</tr>\n");

				writer.Write("<tr valign=\"middle\">\n");
				writer.Write("<td width=\"25%\" align=\"right\" valign=\"middle\">*Expense Date:</td>\n");
				writer.Write("<td align=\"left\">\n");
				writer.Write("<input maxLength=\"10\" size=\"15\" name=\"ExpenseDate\" value=\"" + Common.IIF(Editing , Localization.ToNativeShortDateString(DB.RSFieldDateTime(rs,"ExpenseDate")) , Localization.ToNativeShortDateString(System.DateTime.Now)) + "\">\n");
				writer.Write("<input type=\"hidden\" name=\"ExpenseDate_vldt\" value=\"[req][date][blankalert=Please enter the expense date][invalidalert=Please enter a date, format: " + Localization.ShortDateFormat() + "!]\">\n");
				writer.Write("<button id=\"f_trigger_d\">...</button>\n");
				writer.Write("</td>\n");
				writer.Write("</tr>\n");

				writer.Write("<tr valign=\"middle\">\n");
				writer.Write("<td width=\"25%\" align=\"right\" valign=\"middle\">*Amount:&nbsp;&nbsp;</td>\n");
				writer.Write("<td align=\"left\">\n");
				writer.Write("<input maxLength=\"10\" size=\"10\" name=\"Amount\" value=\"" + Common.IIF(Editing , Localization.CurrencyStringForDB( DB.RSFieldDecimal(rs,"Amount")) , "") + "\">\n");
				writer.Write("<input type=\"hidden\" name=\"Amount_vldt\" value=\"[req][number][blankalert=Please enter the expense amount][invalidalert=Please enter a valid dollar amount, e.g. 10.00 without the leading $ sign!]\">\n");
				writer.Write("</td>\n");
				writer.Write("</tr>\n");
				
				writer.Write("<tr valign=\"middle\">\n");
				writer.Write("<td align=\"right\" valign=\"top\">*Description:</td>\n");
				writer.Write("<td align=\"left\">\n");
				writer.Write("<textarea cols=\"" + Common.AppConfig("Admin_TextareaWidth") + "\" rows=\"" + Common.AppConfig("Admin_TextareaHeightSmall") + "\" id=\"Description\" name=\"Description\">" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Description")) , "") + "</textarea>\n");
				writer.Write("</td>\n");
				writer.Write("</tr>\n");


				writer.Write("<tr>\n");
				writer.Write("<td></td><td align=\"left\"><br>\n");
				if(Editing) 
				{
					writer.Write("<input type=\"submit\" value=\"Update\" name=\"submit\">\n");
					writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"reset\" class=\"CPButton\" value=\"Reset\" name=\"reset\">\n");
				} 
				else 
				{
					writer.Write("<input type=\"submit\" value=\"Add\" name=\"submit\">\n");
				}
				writer.Write("        </td>\n");
				writer.Write("      </tr>\n");


				writer.Write("</form>\n");
				writer.Write("  </table>\n");

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

				writer.Write("<script type=\"text/javascript\">\n");
				writer.Write("    Calendar.setup({\n");
				writer.Write("        inputField     :    \"ExpenseDate\",      // id of the input field\n");
				writer.Write("        ifFormat       :    \"" + Localization.JSCalendarDateFormatSpec() + "\",       // format of the input field\n");
				writer.Write("        showsTime      :    false,            // will display a time selector\n");
				writer.Write("        button         :    \"f_trigger_d\",   // trigger for the calendar (button ID)\n");
				writer.Write("        singleClick    :    true            // Single-click mode\n");
				writer.Write("    });\n");
				writer.Write("</script>\n");
			
			}
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
