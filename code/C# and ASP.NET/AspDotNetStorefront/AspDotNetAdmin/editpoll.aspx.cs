// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the Poll homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
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
	/// Summary description for editpoll
	/// </summary>
	public class editpoll : SkinBase
	{
		
		int PollID;
		String PollCategories;
		String PollSections;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			PollID = 0;

			if(Common.QueryString("PollID").Length != 0 && Common.QueryString("PollID") != "0") 
			{
				Editing = true;
				PollID = Localization.ParseUSInt(Common.QueryString("PollID"));
			} 
			else 
			{
				Editing = false;
			}
			
			IDataReader rs;

			PollCategories = Common.GetPollCategories(PollID);
			PollSections = Common.GetPollSections(PollID);

			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				if(ErrorMsg.Length == 0)
				{
			
					try
					{
						StringBuilder sql = new StringBuilder(2500);
						if(!Editing)
						{
							// ok to add:
							String NewGUID = DB.GetNewGUID();
							sql.Append("insert into Poll(PollGUID,Name,PollSortOrderID,Published,AnonsCanVote,ExpiresOn,LastUpdatedBy) values(");
							sql.Append(DB.SQuote(NewGUID) + ",");
							sql.Append(DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							sql.Append(Common.Form("PollSortOrderID") + ",");
							sql.Append(Common.Form("Published") + ",");
							sql.Append(Common.Form("AnonsCanVote") + ",");
							sql.Append(DB.SQuote(Common.Left(Common.Form("ExpiresOn"),100)) + ",");
							sql.Append(thisCustomer._customerID);
							sql.Append(")");
							DB.ExecuteSQL(sql.ToString());

							rs = DB.GetRS("select PollID from Poll  " + DB.GetNoLock() + " where deleted=0 and PollGUID=" + DB.SQuote(NewGUID));
							rs.Read();
							PollID = DB.RSFieldInt(rs,"PollID");
							Editing = true;
							rs.Close();
							DataUpdated = true;
						}
						else
						{
							// ok to update:
							sql.Append("update Poll set ");
							sql.Append("Name=" + DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							sql.Append("PollSortOrderID=" + Common.Form("PollSortOrderID") + ",");
							sql.Append("Published=" + Common.Form("Published") + ",");
							sql.Append("AnonsCanVote=" + Common.Form("AnonsCanVote") + ",");
							sql.Append("ExpiresOn=" + DB.SQuote(Common.Left(Common.Form("ExpiresOn"),100)) + ",");
							sql.Append("LastUpdatedBy=" + thisCustomer._customerID.ToString() + " ");
							sql.Append("where PollID=" + PollID.ToString());
							DB.ExecuteSQL(sql.ToString());
							DataUpdated = true;
							Editing = true;
						}

						// UPDATE CATEGORY MAPPINGS:
						if(DataUpdated)
						{
							DB.ExecuteSQL("delete from Pollcategory where Pollid=" + PollID.ToString());
							String CMap = Common.Form("CategoryMap");
							if(CMap.Length != 0)
							{
								String[] CMapArray = CMap.Split(',');
								foreach(String s in CMapArray)
								{
									DB.ExecuteSQL("insert into Pollcategory(Pollid,categoryid) values(" + PollID.ToString() + "," + s + ")");
								}
							}
						}

						// UPDATE SECTION MAPPINGS:
						if(DataUpdated)
						{
							DB.ExecuteSQL("delete from Pollsection where Pollid=" + PollID.ToString());
							String SMap = Common.Form("SectionMap");
							if(SMap.Length != 0)
							{
								String[] SMapArray = SMap.Split(',');
								foreach(String s in SMapArray)
								{
									DB.ExecuteSQL("insert into Pollsection(Pollid,sectionid) values(" + PollID.ToString() + "," + s + ")");
								}
							}
						}
					}
					catch(Exception ex)
					{
						ErrorMsg = "<p><b>ERROR: " + Common.GetExceptionDetail(ex,"<br>") + "<br><br></b></p>";
					}
				}

			}
			SectionTitle = "<a href=\"Polls.aspx\">Polls</a> - Manage Polls";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(DataUpdated)
			{
				writer.Write("<p><b><font color=blue>(UPDATED)</font></b></p>\n");
			}

			IDataReader rs = DB.GetRS("select * from Poll  " + DB.GetNoLock() + " where PollID=" + PollID.ToString());
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
					writer.Write("<p align=\"left\"><b>Editing Poll: " + DB.RSField(rs,"Name") + " (Poll SKU=" + DB.RSField(rs,"SKU") + ", PollID=" + DB.RSFieldInt(rs,"PollID").ToString() + ")&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"pollanswers.aspx?Pollid=" + PollID.ToString() + "\">Add/Edit Poll Answers</a>&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"managepoll.aspx?Pollid=" + PollID.ToString() + "\">Review Votes</a>");
					writer.Write("</b>");
					writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;");
					writer.Write("</p>\n");
				}
				else
				{
					writer.Write("<p align=\"left\"><b>Adding New Poll:</p></b>\n");
				}

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function Form_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("submitonce(theForm);\n");
				writer.Write("if (theForm.PollSortOrderID.selectedIndex < 1)\n");
				writer.Write("{\n");
				writer.Write("alert(\"Please select the poll sort order.\");\n");
				writer.Write("theForm.PollSortOrderID.focus();\n");
				writer.Write("submitenabled(theForm);\n");
				writer.Write("return (false);\n");
				writer.Write("    }\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				writer.Write("<p>Please enter the following information about this Poll. Fields marked with an asterisk (*) are required. All other fields are optional.</p>\n");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\">\n");
				writer.Write("<form action=\"editPoll.aspx?PollID=" + PollID.ToString() + "&edit=" + Editing.ToString() + "\" method=\"post\" id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
				writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"100%\" colspan=\"2\" align=\"left\">\n");
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
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Poll Name:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"50\" name=\"Name\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Name")) , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"Name_vldt\" value=\"[req][blankalert=Please enter the Poll name]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("            <tr>");
				writer.Write("              <td width=\"25%\" align=\"right\" valign=\"middle\">Expires On:</td>");
				writer.Write("              <td align=\"left\"><input type=\"text\" name=\"ExpiresOn\" size=\"11\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(Localization.ToNativeShortDateString(DB.RSFieldDateTime(rs,"ExpiresOn"))) , Localization.ToNativeShortDateString(System.DateTime.Now.AddMonths(1))) + "\">&nbsp;<img src=\"skins/skin_" + _siteID.ToString() + "/images/calendar.gif\" style=\"cursor:hand;\" align=\"absmiddle\" id=\"f_trigger_s\">");
				writer.Write("                	<input type=\"hidden\" name=\"ExpiresOn_vldt\" value=\"[date][invalidalert=Please enter a valid starting date in the format " + Localization.ShortDateFormat() + "]\">");
				writer.Write("</td>");
				writer.Write("            </tr>");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Poll Sort Order:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("<select size=\"1\" name=\"PollSortOrderID\">\n");
				writer.Write(" <OPTION VALUE=\"0\">SELECT ONE</option>\n");
				IDataReader rsst = DB.GetRS("select * from PollSortOrder " + DB.GetNoLock());
				while(rsst.Read())
				{
					writer.Write("<option value=\"" + DB.RSFieldInt(rsst,"PollSortOrderID").ToString() + "\"");
					if(Editing) 
					{
						if(DB.RSFieldInt(rs,"PollSortOrderID") == DB.RSFieldInt(rsst,"PollSortOrderID"))
						{
							writer.Write(" selected");
						}
					}
					writer.Write(">" + DB.RSField(rsst,"Name") + "</option>");
				}
				rsst.Close();
				writer.Write("</select>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\" bgcolor=\"" + Common.IIF(Editing && !DB.RSFieldBool(rs,"Published") , "#Fe5888" , "FFFFFF") + "\">*Published:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\" bgcolor=\"" + Common.IIF(Editing && !DB.RSFieldBool(rs,"Published") , "#Fe5888" , "FFFFFF") + "\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"Published\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"Published") , " checked " , "") , " checked ") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"Published\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"Published") , "" , " checked ") , "") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Anons Can Vote:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"AnonsCanVote\" value=\"1\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"AnonsCanVote") , " checked " , "") , "") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"AnonsCanVote\" value=\"0\" " + Common.IIF(Editing , Common.IIF(DB.RSFieldBool(rs,"AnonsCanVote") , "" , " checked ") , " checked ") + ">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"top\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Category(s):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");

				writer.Write("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\n");
				writer.Write("<tr>");
				writer.Write("<td align=\"left\" valign=\"top\">" + GetCategoryList(PollID, PollCategories, 0,1) + "</td>");
				writer.Write("<td align=\"right\" valign=\"top\">Section(s):&nbsp;&nbsp;</td>");
				writer.Write("<td align=\"left\" valign=\"top\">" + GetSectionList(PollID, PollSections, 0,1) + "</td>");
				writer.Write("</tr>");
				writer.Write("</table>");

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
				writer.Write("\n<script type=\"text/javascript\">\n");
				writer.Write("    Calendar.setup({\n");
				writer.Write("        inputField     :    \"ExpiresOn\",      // id of the input field\n");
				writer.Write("        ifFormat       :    \"" + Localization.JSCalendarDateFormatSpec() + "\",       // format of the input field\n");
				writer.Write("        showsTime      :    false,            // will display a time selector\n");
				writer.Write("        button         :    \"f_trigger_s\",   // trigger for the calendar (button ID)\n");
				writer.Write("        singleClick    :    true            // Single-click mode\n");
				writer.Write("    });\n");
				writer.Write("</script>\n");

			}
			rs.Close();
		}

		static public String GetCategoryList(int PollID, String PollCategories, int ForParentCategoryID, int level)
		{
			StringBuilder tmpS = new StringBuilder(5000);
			String sql = String.Empty;
			if(ForParentCategoryID == 0)
			{
				sql = "select * from category  " + DB.GetNoLock() + " where (parentcategoryid=0 or ParentCategoryID IS NULL) and published<>0 and deleted=0 order by DisplayOrder,Name";
			}
			else
			{
				sql = "select * from category  " + DB.GetNoLock() + " where parentcategoryid=" + ForParentCategoryID.ToString() + " and published<>0 and deleted=0 order by DisplayOrder,Name";
			}
			IDataReader rs = DB.GetRS(sql);

			String Indent = String.Empty;
			for(int i = 1; i < level; i++)
			{
				Indent += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
			}
			while(rs.Read())
			{
				bool PollIsMappedToThisCategory = (("," + PollCategories + ",").IndexOf("," + DB.RSFieldInt(rs,"CategoryID").ToString() + ",") != -1);
				tmpS.Append("<input type=\"checkbox\" name=\"CategoryMap\" value=\"" +  DB.RSFieldInt(rs,"CategoryID").ToString() + "\" " + Common.IIF(PollIsMappedToThisCategory , " checked " , "") + ">" + Common.IIF(level == 1 , "<b>" , "") + Indent + DB.RSField(rs,"name") + Common.IIF(level == 1 , "</b>" , "") + "<br>\n");
				if(Common.CategoryHasSubs(DB.RSFieldInt(rs,"CategoryID")))
				{
					tmpS.Append(GetCategoryList(PollID, PollCategories, DB.RSFieldInt(rs,"CategoryID"),level+1));
				}
			}
			rs.Close();
			return tmpS.ToString();
		}

		static public String GetSectionList(int PollID, String PollSections, int ForParentSectionID, int level)
		{
			StringBuilder tmpS = new StringBuilder(5000);
			String sql = String.Empty;
			if(ForParentSectionID == 0)
			{
				sql = "select * from [section]  " + DB.GetNoLock() + " where (parentSectionid=0 or ParentSectionID IS NULL) and published<>0 and deleted=0 order by DisplayOrder,Name";
			}
			else
			{
				sql = "select * from [section]  " + DB.GetNoLock() + " where parentSectionid=" + ForParentSectionID.ToString() + " and published<>0 and deleted=0 order by DisplayOrder,Name";
			}
			IDataReader rs = DB.GetRS(sql);

			String Indent = String.Empty;
			for(int i = 1; i < level; i++)
			{
				Indent += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
			}
			while(rs.Read())
			{
				bool PollIsMappedToThisSection = (("," + PollSections + ",").IndexOf("," + DB.RSFieldInt(rs,"SectionID").ToString() + ",") != -1);
				tmpS.Append("<input type=\"checkbox\" name=\"SectionMap\" value=\"" +  DB.RSFieldInt(rs,"SectionID").ToString() + "\" " + Common.IIF(PollIsMappedToThisSection , " checked " , "") + ">" + Common.IIF(level == 1 , "<b>" , "") + Indent + DB.RSField(rs,"name") + Common.IIF(level == 1 , "</b>" , "") + "<br>\n");
				if(Common.SectionHasSubs(DB.RSFieldInt(rs,"SectionID")))
				{
					tmpS.Append(GetSectionList(PollID, PollSections, DB.RSFieldInt(rs,"SectionID"),level+1));
				}
			}
			rs.Close();
			return tmpS.ToString();
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
