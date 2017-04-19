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
	/// Summary description for editstate
	/// </summary>
	public class editstate : SkinBase
	{
		
		int StateID;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			StateID = 0;

			if(Common.QueryString("StateID").Length != 0 && Common.QueryString("StateID") != "0") 
			{
				Editing = true;
				StateID = Localization.ParseUSInt(Common.QueryString("StateID"));
			} 
			else 
			{
				Editing = false;
			}
			
			IDataReader rs;
			
			int N = 0;
			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{

				if(Editing)
				{
					// see if this State already exists:
					rs = DB.GetRS("select count(Name) as N from State  " + DB.GetNoLock() + " where StateID<>" + StateID.ToString() + " and lower(Name)=" + DB.SQuote(Common.Form("Name").ToLower()));
					rs.Read();
					N = DB.RSFieldInt(rs,"N");
					rs.Close();
					if(N != 0)
					{
						ErrorMsg = "<p><b><font color=red>ERROR:<br><br></font><blockquote>There is already another State/province with that code. Please <a href=\"javascript:history.back(-1);\">go back</a>.</b></blockquote></p>";
					}
				}
				else
				{
					// see if this name is already there:
					rs = DB.GetRS("select count(Name) as N from State  " + DB.GetNoLock() + " where lower(Name)=" + DB.SQuote(Common.Form("Name").ToLower()));
					rs.Read();
					N = DB.RSFieldInt(rs,"N");
					rs.Close();
					if(N != 0)
					{
						ErrorMsg = "<p><b><font color=red>ERROR:<br><br></font><blockquote>There is already another State/province with that name. Please <a href=\"javascript:history.back(-1);\">go back</a>.</b></blockquote></p>";
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
							sql.Append("insert into State(StateGUID,Name,Abbreviation,LastUpdatedBy) values(");
							sql.Append(DB.SQuote(NewGUID) + ",");
							sql.Append(DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							sql.Append(DB.SQuote(Common.Left(Common.Form("Abbreviation"),2)) + ",");
							sql.Append(thisCustomer._customerID);
							sql.Append(")");
							DB.ExecuteSQL(sql.ToString());

							rs = DB.GetRS("select StateID from State  " + DB.GetNoLock() + " where StateGUID=" + DB.SQuote(NewGUID));
							rs.Read();
							StateID = DB.RSFieldInt(rs,"StateID");
							Editing = true;
							rs.Close();
							DataUpdated = true;
						}
						else
						{
							// ok to update:
							sql.Append("update State set ");
							sql.Append("Name=" + DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							sql.Append("Abbreviation=" + DB.SQuote(Common.Left(Common.Form("Abbreviation"),2)) + ",");
							sql.Append("LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
							sql.Append("LastUpdatedBy=" + thisCustomer._customerID.ToString() + " ");
							sql.Append("where StateID=" + StateID.ToString());
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
			SectionTitle = "<a href=\"States.aspx\">States</a> - Manage States";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			IDataReader rs = DB.GetRS("select * from State  " + DB.GetNoLock() + " where StateID=" + StateID.ToString());
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

				writer.Write("<div align=\"left\">");
				if(Editing)
				{
					writer.Write("<b>Editing State: " + DB.RSField(rs,"Name") + " (ID=" + DB.RSFieldInt(rs,"StateID").ToString() + ")<br><br></b>\n");
				}
				else
				{
					writer.Write("<b>Adding New State:<br><br></b>\n");
				}

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function Form_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("submitonce(theForm);\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				writer.Write("<p>Please enter the following information about this State. Fields marked with an asterisk (*) are required. All other fields are optional.</p>\n");
				writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\">\n");
				writer.Write("<form enctype=\"multipart/form-data\" action=\"editState.aspx?StateID=" + StateID.ToString() + "&edit=" + Editing.ToString() + "\" method=\"post\" id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
				writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"100%\" colspan=\"2\" align=\"left\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Name:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"Name\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Name")) , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"Name_vldt\" value=\"[req][blankalert=Please enter the state/province name]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Abbreviation:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"2\" size=\"3\" name=\"Abbreviation\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Abbreviation")) , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"Abbreviation_vldt\" value=\"[req][blankalert=Please enter the 2 letter state/province abbreviation code]\">\n");
				writer.Write("                	</td>\n");
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
				writer.Write("</div>");
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
