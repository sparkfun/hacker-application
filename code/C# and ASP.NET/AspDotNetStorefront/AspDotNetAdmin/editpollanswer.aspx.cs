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
using System.Web;
using System.IO;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for editpollanswer
	/// </summary>
	public class editpollanswer : SkinBase
	{
		
		int PollID;
		int PollAnswerID;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			PollID = Common.QueryStringUSInt("PollID");
			PollAnswerID = 0;
			

			if(Common.QueryString("PollAnswerID").Length != 0 && Common.QueryString("PollAnswerID") != "0") 
			{
				Editing = true;
				PollAnswerID = Localization.ParseUSInt(Common.QueryString("PollAnswerID"));
			} 
			else 
			{
				Editing = false;
			}
			if(PollID == 0)
			{
				Response.Redirect("polls.aspx");
			}
			
			
			IDataReader rs;
			
			//int N = 0;
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
							sql.Append("insert into PollAnswer(PollAnswerGUID,PollID,Name,LastUpdatedBy) values(");
							sql.Append(DB.SQuote(NewGUID) + ",");
							sql.Append(PollID.ToString() + ",");
							sql.Append(DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							sql.Append(thisCustomer._customerID);
							sql.Append(")");
							DB.ExecuteSQL(sql.ToString());

							rs = DB.GetRS("select PollAnswerID from PollAnswer  " + DB.GetNoLock() + " where deleted=0 and PollAnswerGUID=" + DB.SQuote(NewGUID));
							rs.Read();
							PollAnswerID = DB.RSFieldInt(rs,"PollAnswerID");
							Editing = true;
							rs.Close();
							DataUpdated = true;
						}
						else
						{
							// ok to update:
							sql.Append("update PollAnswer set ");
							sql.Append("PollID=" + PollID.ToString() + ",");
							sql.Append("Name=" + DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							sql.Append("LastUpdatedBy=" + thisCustomer._customerID.ToString() + " ");
							sql.Append("where PollAnswerID=" + PollAnswerID.ToString());
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
			SectionTitle = "<a href=\"PollAnswers.aspx?Pollid=" + PollID.ToString() + "\">PollAnswers</a> - Manage PollAnswers";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(DataUpdated)
			{
				writer.Write("<p><b><font color=blue>(UPDATED)</font></b></p>\n");
			}
			
			IDataReader rs = DB.GetRS("select * from PollAnswer  " + DB.GetNoLock() + " where PollAnswerID=" + PollAnswerID.ToString());
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

				writer.Write("<p align=\"left\"><b>Within Poll: <a href=\"editPoll.aspx?Pollid=" + PollID.ToString() + "\">" + Common.GetPollName(PollID) + "</a> (PollID=" + PollID.ToString() + ")</b</p>\n");
				if(Editing)
				{
					writer.Write("<p align=\"left\"><b>Editing Poll Answer: " + DB.RSField(rs,"Name") + " (PollAnswerID=" + DB.RSFieldInt(rs,"PollAnswerID").ToString() + ")</b></p>\n");
				}
				else
				{
					writer.Write("<p align=\"left\"><b>Adding New Poll Answer:</p></b>\n");
				}

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function Form_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("submitonce(theForm);\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				writer.Write("<p align=\"left\">Please enter the following information about this Poll Answer. Fields marked with an asterisk (*) are required. All other fields are optional.</p>\n");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\">\n");
				writer.Write("<form action=\"editPollAnswer.aspx?Pollid=" + PollID.ToString() + "&PollAnswerID=" + PollAnswerID.ToString() + "&edit=" + Editing.ToString() + "\" method=\"post\" id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
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
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">Answer Text:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"Name\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Name")) , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"Name_vldt\" value=\"[req][blankalert=Please enter the answer text]\">\n");
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
