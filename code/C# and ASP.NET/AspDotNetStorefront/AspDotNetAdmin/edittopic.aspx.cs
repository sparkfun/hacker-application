// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
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
	/// Summary description for edittopic
	/// </summary>
	public class edittopic : SkinBase
	{
		
		int TopicID;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			TopicID = 0;

			if(Common.QueryString("TopicID").Length != 0 && Common.QueryString("TopicID") != "0") 
			{
				Editing = true;
				TopicID = Localization.ParseUSInt(Common.QueryString("TopicID"));
			} 
			else 
			{
				Editing = false;
			}
			
			IDataReader rs;
			
			//int N = 0;
			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{

//				if(Editing)
//				{
//					// see if this topic already exists:
//					rs = DB.GetRS("select count(name) as N from Topic  " + DB.GetNoLock() + " where TopicID<>" + TopicID.ToString() + " and deleted=0 and lower(Name)=" + DB.SQuote(Common.Form("Name").ToLower()));
//					rs.Read();
//					N = DB.RSFieldInt(rs,"N");
//					rs.Close();
//					if(N != 0)
//					{
//						ErrorMsg = "<p><b><font color=red>ERROR:<br><br></font><blockquote>There is already another topic with that name. Please <a href=\"javascript:history.back(-1);\">go back</a>.</b></blockquote></p>";
//					}
//				}
//				else
//				{
//					// see if this name is already there:
//					rs = DB.GetRS("select count(name) as N from Topic  " + DB.GetNoLock() + " where deleted=0 and lower(Name)=" + DB.SQuote(Common.Form("Name").ToLower()));
//					rs.Read();
//					N = DB.RSFieldInt(rs,"N");
//					rs.Close();
//					if(N != 0)
//					{
//						ErrorMsg = "<p><b><font color=red>ERROR:<br><br></font><blockquote>There is already another topic with that name. Please <a href=\"javascript:history.back(-1);\">go back</a>.</b></blockquote></p>";
//					}
//				}

				if(ErrorMsg.Length == 0)
				{
					try
					{
						StringBuilder sql = new StringBuilder(2500);
						if(!Editing)
						{
							// ok to add them:
							String NewGUID = DB.GetNewGUID();
							sql.Append("insert into Topic(TopicGUID,Name,LocaleSetting,Title,Description,[Password],RequiresSubscription,SEKeywords,SEDescription,SETitle,LastUpdatedBy) values(");
							sql.Append(DB.SQuote(NewGUID) + ",");
							sql.Append(DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							if(Common.Form("LocaleSetting").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("LocaleSetting")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							sql.Append(DB.SQuote(Common.Left(Common.Form("Title"),100)) + ",");
							if(Common.Form("Description").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("Description")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("Password").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("Password")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							sql.Append(Common.FormUSInt("RequiresSubscription").ToString() + ",");
							if(Common.Form("SEKeywords").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("SEKeywords")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("SEDescription").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("SEDescription")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("SETitle").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("SETitle")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							sql.Append(thisCustomer._customerID);
							sql.Append(")");
							DB.ExecuteSQL(sql.ToString());

							rs = DB.GetRS("select TopicID from Topic  " + DB.GetNoLock() + " where deleted=0 and TopicGUID=" + DB.SQuote(NewGUID));
							rs.Read();
							TopicID = DB.RSFieldInt(rs,"TopicID");
							Editing = true;
							rs.Close();
							DataUpdated = true;
						}
						else
						{
							// ok to update:
							sql.Append("update Topic set ");
							sql.Append("Name=" + DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							if(Common.Form("LocaleSetting").Length != 0)
							{
								sql.Append("LocaleSetting=" + DB.SQuote(Common.Form("LocaleSetting")) + ",");
							}
							else
							{
								sql.Append("LocaleSetting=NULL,");
							}
							sql.Append("Title=" + DB.SQuote(Common.Left(Common.Form("Title"),100)) + ",");
							if(Common.Form("Description").Length != 0)
							{
								sql.Append("Description=" + DB.SQuote(Common.Form("Description")) + ",");
							}
							else
							{
								sql.Append("Description=NULL,");
							}
							if(Common.Form("Password").Length != 0)
							{
								sql.Append("[Password]=" + DB.SQuote(Common.Form("Password")) + ",");
							}
							else
							{
								sql.Append("[Password]=NULL,");
							}
							sql.Append("RequiresSubscription=" + Common.FormUSInt("RequiresSubscription").ToString() + ",");
							if(Common.Form("SEKeywords").Length != 0)
							{
								sql.Append("SEKeywords=" + DB.SQuote(Common.Form("SEKeywords")) + ",");
							}
							else
							{
								sql.Append("SEKeywords=NULL,");
							}
							if(Common.Form("SEDescription").Length != 0)
							{
								sql.Append("SEDescription=" + DB.SQuote(Common.Form("SEDescription")) + ",");
							}
							else
							{
								sql.Append("SEDescription=NULL,");
							}
							if(Common.Form("SETitle").Length != 0)
							{
								sql.Append("SETitle=" + DB.SQuote(Common.Form("SETitle")) + ",");
							}
							else
							{
								sql.Append("SETitle=NULL,");
							}
							sql.Append("LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
							sql.Append("LastUpdatedBy=" + thisCustomer._customerID.ToString() + " ");
							sql.Append("where TopicID=" + TopicID.ToString());
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
			SectionTitle = "<a href=\"Topics.aspx\">Topics</a> - Manage Topics";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			IDataReader rs = DB.GetRS("select * from Topic  " + DB.GetNoLock() + " where TopicID=" + TopicID.ToString());
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


			if(ErrorMsg.Length == 0)
			{

				if(Editing)
				{
					writer.Write("<b>Editing Topic: " + DB.RSField(rs,"Name") + " (ID=" + DB.RSFieldInt(rs,"TopicID").ToString() + ")<br><br></b>\n");
				}
				else
				{
					writer.Write("<b>Adding New Topic:<br><br></b>\n");
				}

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function Form_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("submitonce(theForm);\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				if(TopicID != 0)
				{
					writer.Write("<p align=\"left\"><a href=\"edittopic.aspx\"><b>Make Another New Topic</b></a></p>");
				}

				writer.Write("<p>Please enter the following information about this topic. Fields marked with an asterisk (*) are required. All other fields are optional.</p>\n");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\">\n");
				writer.Write("<form action=\"edittopic.aspx?TopicID=" + TopicID.ToString() + "&edit=" + Editing.ToString() + "\" method=\"post\" id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
				writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"100%\" colspan=\"2\" align=\"left\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Topic Name:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"Name\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Name")) , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"Name_vldt\" value=\"[req][blankalert=Please enter the topic name]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Topic title:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"Title\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Title")) , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"Title_vldt\" value=\"[req][blankalert=Please enter the topic page title]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Locale Setting:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("<select size=\"1\" name=\"LocaleSetting\">\n");
				writer.Write(" <OPTION VALUE=\"\">SELECT ONE</option>\n");
				IDataReader rsst = DB.GetRS("select * from LocaleSetting  " + DB.GetNoLock() + " order by displayorder,name");
				while(rsst.Read())
				{
					writer.Write("<option value=\"" + DB.RSField(rsst,"Name").ToString() + "\"");
					if(Editing) 
					{
						if(DB.RSField(rs,"LocaleSetting") == DB.RSField(rsst,"Name"))
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
				writer.Write("                <td align=\"right\" valign=\"top\">Description (HTML OK):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<textarea style=\"height: 60em; width: 100%;\" cols=\"" + Common.AppConfig("Admin_TextareaWidth") + "\" rows=\"" + Common.AppConfig("Admin_TextareaHeightl") + "\" id=\"Description\" name=\"Description\">" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Description")) , "") + "</textarea>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Search Engine Page Title:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"100\" name=\"SETitle\" value=\"" + Common.IIF(Editing , DB.RSField(rs,"SETitle") , "") + "\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Search Engine Keywords:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"1000\" size=\"100\" name=\"SEKeywords\" value=\"" + Common.IIF(Editing , DB.RSField(rs,"SEKeywords") , "") + "\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Search Engine Description:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"255\" size=\"100\" name=\"SEDescription\" value=\"" + Common.IIF(Editing , DB.RSField(rs,"SEDescription") , "") + "\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Password:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input Password=\"100\" size=\"20\" name=\"Password\" value=\"" + Common.IIF(Editing , DB.RSField(rs,"Password") , "") + "\"> (Only required if you want to protect this topic content by requiring a password to be entered)\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("      <tr>");
				writer.Write("                <td align=\"right\" valign=\"top\">Requires Subscription:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"RequiresSubscription\" value=\"1\" " + Common.IIF(DB.RSFieldBool(rs,"RequiresSubscription") , " checked " , "") + ">\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"RequiresSubscription\" value=\"0\" " + Common.IIF(DB.RSFieldBool(rs,"RequiresSubscription") , "" , " checked ") + ">\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");

				
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
			}
			rs.Close();

			writer.Write(Common.GenerateHtmlEditor("Description"));
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
