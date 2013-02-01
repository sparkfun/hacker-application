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
	/// Summary description for editappconfigv1
	/// </summary>
	public class editappconfigv1 : SkinBase
	{


		String SearchFor;
		String GroupName;
		String BeginsWith;
		int AppConfigID;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SearchFor = Common.QueryString("SearchFor");
			GroupName = Common.QueryString("GroupName");
			BeginsWith = Common.QueryString("BeginsWith");
			AppConfigID = 0;
			
			
			if(Common.QueryString("AppConfigID").Length != 0 && Common.QueryString("AppConfigID") != "0") 
			{
				Editing = true;
				AppConfigID = Localization.ParseUSInt(Common.QueryString("AppConfigID"));
			} 
			else 
			{
				Editing = false;
			}
			
			IDataReader rs;
			
			int N = 0;
			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				Common.ClearCache();
				if(Editing)
				{
					// see if this appconfig already exists:
					rs = DB.GetRS("select count(Name) as N from appconfig  " + DB.GetNoLock() + " where AppConfigID<>" + AppConfigID.ToString() + " and lower(Name)=" + DB.SQuote(Common.Form("Name").ToLower()));
					rs.Read();
					N = DB.RSFieldInt(rs,"N");
					rs.Close();
					if(N != 0)
					{
						ErrorMsg = "<p><b><font color=red>ERROR:<br><br></font><blockquote>There is already another appconfig parameter with that name. Please <a href=\"javascript:history.back(-1);\">go back</a>.</b></blockquote></p>";
					}
				}
				else
				{
					// see if this name is already there:
					rs = DB.GetRS("select count(Name) as N from AppConfig  " + DB.GetNoLock() + " where lower(Name)=" + DB.SQuote(Common.Form("Name").ToLower()));
					rs.Read();
					N = DB.RSFieldInt(rs,"N");
					rs.Close();
					if(N != 0)
					{
						ErrorMsg = "<p><b><font color=red>ERROR:<br><br></font><blockquote>There is already another appconfig parameter with that name. Please <a href=\"javascript:history.back(-1);\">go back</a>.</b></blockquote></p>";
					}
				}

				if(ErrorMsg.Length == 0)
				{
					try
					{
						StringBuilder sql = new StringBuilder(2500);
						if(!Editing)
						{
							// ok to add them:
							String NewGUID = DB.GetNewGUID();
							//SEC4
							sql.Append("insert into appconfig(AppConfigGUID,Name,Description,ConfigValue,LastUpdatedBy,SuperOnly) values(");
							sql.Append(DB.SQuote(NewGUID) + ",");
							sql.Append(DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							sql.Append(DB.SQuote(Common.Form("Description")) + ",");
							sql.Append(DB.SQuote(Common.Left(Common.Form("ConfigValue"),1000)) + ",");
							sql.Append(thisCustomer._customerID.ToString() + ",");
							sql.Append(Common.FormUSInt("SuperOnly").ToString());
							sql.Append(")");
							DB.ExecuteSQL(sql.ToString());

							rs = DB.GetRS("select AppConfigID from appconfig  " + DB.GetNoLock() + " where AppConfigGUID=" + DB.SQuote(NewGUID));
							rs.Read();
							AppConfigID = DB.RSFieldInt(rs,"AppConfigID");
							Editing = true;
							rs.Close();
							DataUpdated = true;
						}
						else
						{
							// ok to update:
							sql.Append("update appconfig set ");
							sql.Append("Name=" + DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							sql.Append("Description=" + DB.SQuote(Common.Form("Description")) + ",");
							sql.Append("ConfigValue=" + DB.SQuote(Common.Left(Common.Form("ConfigValue"),1000)) + ",");
							sql.Append("LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
							sql.Append("LastUpdatedBy=" + thisCustomer._customerID.ToString());
							sql.Append(String.Format(",SuperOnly={0} ",Common.FormUSInt("SuperOnly"))); 
							sql.Append("where AppConfigID=" + AppConfigID.ToString());
							DB.ExecuteSQL(sql.ToString());
							DataUpdated = true;
							Editing = true;
						}
					}
					catch(Exception ex)
					{
						ErrorMsg = "<p><b>ERROR: " + Common.GetExceptionDetail(ex,"<br>") + "<br><br></b></p>";
					}
					if(ErrorMsg.Length == 0)
					{
						Common.ClearCache();
					}

				}

			}
			SectionTitle = "<a href=\"appconfig.aspx?GroupName=" + Server.UrlEncode(GroupName) + "&beginsWith=" + Server.UrlEncode(BeginsWith) + "&searchfor=" + Server.UrlEncode(SearchFor) + "\">AppConfig</a> - Manage AppConfig Parameters";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			IDataReader rs = DB.GetRS("select * from AppConfig  " + DB.GetNoLock() + " where AppConfigID=" + AppConfigID.ToString());
			if(rs.Read())
			{
				Editing = true;
			}
			
			if(ErrorMsg.Length != 0)
			{
				writer.Write("<p align=\"left\"><b><font color=red>" + ErrorMsg + "</font></b></p>\n");
			}
			if(DataUpdated)
			{
				writer.Write("<p align=\"left\"><b><font color=blue>(UPDATED)</font></b></p>\n");
			}


			if(ErrorMsg.Length == 0)
			{

				if(Editing)
				{
					writer.Write("<p align=\"left\"><b>Editing AppConfig: " + DB.RSField(rs,"Name") + " (ID=" + DB.RSFieldInt(rs,"AppConfigID").ToString() + ")&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"editappconfig.aspx?GroupName=" + Server.UrlEncode(GroupName) + "&beginsWith=" + Server.UrlEncode(BeginsWith) + "&searchfor=" + Server.UrlEncode(SearchFor) + "&appconfigid=" + Common.GetPreviousAppConfig(DB.RSField(rs,"Name")).ToString() + "\">previous</a>&nbsp;&nbsp;&nbsp;<a href=\"editappconfig.aspx?GroupName=" + Server.UrlEncode(GroupName) + "&beginsWith=" + Server.UrlEncode(BeginsWith) + "&searchfor=" + Server.UrlEncode(SearchFor) + "&appconfigid=" + Common.GetNextAppConfig(DB.RSField(rs,"Name")).ToString() + "\">next</a></b></p>\n");
				}
				else
				{
					writer.Write("<p align=\"left\"><b>Adding New AppConfig:</b></p>\n");
				}

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function Form_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("submitonce(theForm);\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				writer.Write("<p>Please enter the following information about this appconfig. Fields marked with an asterisk (*) are required. All other fields are optional.</p>\n");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\">\n");
				writer.Write("<form enctype=\"multipart/form-data\" action=\"editappconfig.aspx?GroupName=" + Server.UrlEncode(GroupName) + "&beginsWith=" + Server.UrlEncode(BeginsWith) + "&searchfor=" + Server.UrlEncode(SearchFor) + "&AppConfigID=" + AppConfigID.ToString() + "&edit=" + Editing.ToString() + "\" method=\"post\" id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
				writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"100%\" colspan=\"2\" align=\"left\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Name:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"50\" name=\"Name\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Name")) , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"Name_vldt\" value=\"[req][blankalert=Please enter the appconfig parameter name]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Description:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<textarea rows=\"10\" cols=\"80\" id=\"Description\" name=\"Description\">" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Description")) , "") + "</textarea>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Value:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"1000\" size=\"100\" name=\"ConfigValue\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"ConfigValue")) , "") + "\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				if (thisCustomer.IsAdminSuperUser)
				{
        
					writer.Write("              <tr valign=\"middle\">\n");
					writer.Write("                <td align=\"right\" valign=\"top\">SuperUser Only:&nbsp;&nbsp;</td>\n");
					writer.Write("                <td align=\"left\">\n");
					writer.Write("                <input type=\"checkbox\" name=\"SuperOnly\" value=\"1\"" + Common.IIF(DB.RSFieldBool(rs,"SuperOnly")," checked ","") + "><br>\n");
					writer.Write("                </td>\n");
					writer.Write("              </tr>\n");
				}

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
