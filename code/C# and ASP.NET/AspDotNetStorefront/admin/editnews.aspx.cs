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
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary NewsCopy for editnews
	/// </summary>
	public class editnews : SkinBase
	{
		
		int NewsID;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			NewsID = 0;
			if(Common.QueryString("NewsID").Length != 0 && Common.QueryString("NewsID") != "0") 
			{
				Editing = true;
				NewsID = Localization.ParseUSInt(Common.QueryString("NewsID"));
			} 
			else 
			{
				Editing = false;
			}
						
			IDataReader rs;
			
			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				try
				{
					StringBuilder sql = new StringBuilder(2500);
					if(!Editing)
					{
						// ok to add them:
						String NewGUID = DB.GetNewGUID();
						sql.Append("insert into news(NewsGUID,ExpiresOn,NewsCopy,Published,LastUpdatedBy) values(");
						sql.Append(DB.SQuote(NewGUID) + ",");
						sql.Append(DB.DateQuote(Common.Form("ExpiresOn")) + ",");
						sql.Append(DB.SQuote(Common.Form("NewsCopy")) + ",");
						sql.Append(Common.Form("Published") + ",");
						sql.Append(thisCustomer._customerID);
						sql.Append(")");
						DB.ExecuteSQL(sql.ToString());

						rs = DB.GetRS("select NewsID from news   " + DB.GetNoLock() + " where deleted=0 and NewsGUID=" + DB.SQuote(NewGUID));
						rs.Read();
						NewsID = DB.RSFieldInt(rs,"NewsID");
						Editing = true;
						rs.Close();
						DataUpdated = true;
					}
					else
					{
						// ok to update:
						sql.Append("update news set ");
						sql.Append("NewsCopy=" + DB.SQuote(Common.Form("NewsCopy")) + ",");
						sql.Append("ExpiresOn=" + DB.DateQuote(Common.Form("ExpiresOn")) + ",");
						sql.Append("Published=" + Common.Form("Published") + ",");
						sql.Append("LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
						sql.Append("LastUpdatedBy=" + thisCustomer._customerID.ToString() + " ");
						sql.Append("where NewsID=" + NewsID.ToString());
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
			SectionTitle = "<a href=\"news.aspx\">News</a> - Manage News" + Common.IIF(DataUpdated , " (Updated)" , "");
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			IDataReader rs = DB.GetRS("select * from News  " + DB.GetNoLock() + " where NewsID=" + NewsID.ToString());
			if(rs.Read())
			{
				Editing = true;
			}
			
			if(ErrorMsg.Length != 0)
			{
				writer.Write("<p><b><font color=red>" + ErrorMsg + "</font></b></p>\n");
			}


			if(ErrorMsg.Length == 0)
			{

				if(Editing)
				{
					writer.Write("<b>Editing News: (ID=" + DB.RSFieldInt(rs,"NewsID").ToString() + ")<br><br></b>\n");
				}
				else
				{
					writer.Write("<b>Adding New News Item:<br><br></b>\n");
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
				writer.Write("function Form_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("submitonce(theForm);\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				writer.Write("<p>Please enter the following information about this news item. Fields marked with an asterisk (*) are required. All other fields are optional.</p>\n");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\">\n");
				writer.Write("<form enctype=\"multipart/form-data\" action=\"editnews.aspx?NewsID=" + NewsID.ToString() + "&edit=" + Editing.ToString() + "\" method=\"post\" id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
				writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"100%\" colspan=\"2\" align=\"left\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">News Copy (HTML OK):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<textarea cols=\"" + Common.AppConfig("Admin_TextareaWidth") + "\" rows=\"" + Common.AppConfig("Admin_TextareaHeight") + "\" name=\"NewsCopy\">" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"NewsCopy")) , "") + "</textarea>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Expiration Date:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"ExpiresOn\" value=\"" + Common.IIF(Editing , Localization.ToNativeShortDateString(DB.RSFieldDateTime(rs,"ExpiresOn")) , Localization.ToNativeShortDateString(System.DateTime.Now.AddMonths(1))) + "\">&nbsp;<img src=\"skins/skin_" + _siteID.ToString() + "/images/calendar.gif\" style=\"cursor:hand;\" align=\"absmiddle\" id=\"f_trigger_s\">&nbsp; <small>(" + Localization.ShortDateFormat() + ")</small>\n");
				writer.Write("                	<input type=\"hidden\" name=\"ExpiresOn_vldt\" value=\"[req][blankalert=Please enter the expiration date (e.g. " + Localization.ShortDateFormat() + ")]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Published:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"Published\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"Published") , " checked " , "") , " checked ") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"Published\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"Published") , "" , " checked ") , "") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");


				writer.Write("<tr>\n");
				writer.Write("<td></td><td align=\"left\"><br>\n");
				if(Editing) 
				{
					writer.Write("<input type=\"submit\" value=\"Update\" name=\"submit\">\n");
					writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"reset\" class=\"CPButton\" value=\"Reset\" name=\"reset\">\n");
				} 
				else 
				{
					writer.Write("<input type=\"submit\" value=\"Add New\" name=\"submit\">\n");
				}
				writer.Write("        </td>\n");
				writer.Write("      </tr>\n");
				writer.Write("</form>\n");
				writer.Write("  </table>\n");
				writer.Write("\n<script type=\"text/javascript\">\n");
				writer.Write("    Calendar.setup({\n");
				writer.Write("        inputField     :    \"ExpiresOn\",      // id of the input field\n");
				writer.Write("        ifFormat       :    \"" + Localization.JSCalendarDateFormatSpec() + "\",       // format of the input field\n");
				writer.Write("        showsTime      :    false,            // will display a time selector\n");
				writer.Write("        button         :    \"f_trigger_s\",   // trigger for the calendar (button ID)\n");
				writer.Write("        singleClick    :    true            // double-click mode\n");
				writer.Write("    });\n");
				writer.Write("</script>\n");
				writer.Write(Common.GenerateHtmlEditor("NewsCopy"));
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
