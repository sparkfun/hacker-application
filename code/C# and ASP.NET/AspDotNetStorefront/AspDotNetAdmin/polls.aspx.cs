// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for polls.
	/// </summary>
	public class polls : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Manage Polls";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(Common.QueryString("DeleteID").Length != 0)
			{
				// delete the record:
				DB.ExecuteSQL("update Poll set deleted=1, LastUpdatedBy=" + thisCustomer._customerID.ToString() + " where PollID=" + Common.QueryString("DeleteID"));
				Common.ClearCache();
			}

			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				for(int i = 0; i<=Request.Form.Count-1; i++)
				{
					if(Request.Form.Keys[i].IndexOf("DisplayOrder_") != -1)
					{
						String[] keys = Request.Form.Keys[i].Split('_');
						int PollID = Localization.ParseUSInt(keys[1]);
						int DispOrd = 1;
						try
						{
							DispOrd = Localization.ParseUSInt(Request.Form[Request.Form.Keys[i]]);
						}
						catch {}
						DB.ExecuteSQL("update Poll set DisplayOrder=" + DispOrd.ToString() + " where PollID=" + PollID.ToString());
					}
				}
			}
			
			DataSet ds = DB.GetDS("select * from Poll  " + DB.GetNoLock() + " where deleted=0 order by displayorder,name",false,System.DateTime.Now.AddDays(1));
			writer.Write("<form method=\"POST\" action=\"polls.aspx\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("<p align=\"left\"><input type=\"button\" value=\"Add New Poll\" name=\"AddNew\" onClick=\"self.location='editPoll.aspx';\"><p>");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td><b>ID</b></td>\n");
			writer.Write("      <td><b>Poll</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Expires On</b></td>\n");
			writer.Write("      <td align=\"center\"><b>NumVotes</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Display Order</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Manage Answers</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Review Votes</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete Poll</b></td>\n");
			writer.Write("    </tr>\n");
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
				writer.Write("      <td >" + DB.RowFieldInt(row,"PollID").ToString() + "</td>\n");
				writer.Write("<td>\n");
				writer.Write("<a href=\"editPoll.aspx?Pollid=" + DB.RowFieldInt(row,"PollID").ToString() + "\">");
				writer.Write(DB.RowField(row,"Name"));
				writer.Write("</a>");
				writer.Write("</td>\n");
				writer.Write("<td align=\"center\">" + Localization.ToNativeShortDateString(DB.RowFieldDateTime(row,"ExpiresOn")) + "</td>");
				writer.Write("<td align=\"center\">" + DB.GetSqlN("select count(*) as N from PollVotingRecord  " + DB.GetNoLock() + " where pollanswerid in (select distinct pollanswerid from pollanswer where deleted=0) and PollID=" + DB.RowFieldInt(row,"PollID").ToString()).ToString() + "</td>");
				writer.Write("      <td align=\"center\"><input size=2 type=\"text\" name=\"DisplayOrder_" + DB.RowFieldInt(row,"PollID").ToString() + "\" value=\"" + DB.RowFieldInt(row,"DisplayOrder").ToString() + "\"></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Manage Answers\" name=\"ManageAnswers_" + DB.RowFieldInt(row,"PollID").ToString() + "\" onClick=\"self.location='pollanswers.aspx?Pollid=" + DB.RowFieldInt(row,"PollID").ToString() + "'\"></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Review Votes\" name=\"ReviewVotes_" + DB.RowFieldInt(row,"PollID").ToString() + "\" onClick=\"self.location='managepoll.aspx?Pollid=" + DB.RowFieldInt(row,"PollID").ToString() + "'\"></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Delete\" name=\"Delete_" + DB.RowFieldInt(row,"PollID").ToString() + "\" onClick=\"DeletePoll(" + DB.RowFieldInt(row,"PollID").ToString() + ")\"></td>\n");
				writer.Write("    </tr>\n");
			}
			ds.Dispose();
			writer.Write("    <tr>\n");
			writer.Write("      <td colspan=\"4\" align=\"left\"></td>\n");
			writer.Write("      <td align=\"center\" bgcolor=\"" + Common.AppConfig("LightCellColor") + "\"><input type=\"submit\" value=\"Update\" name=\"Submit\"></td>\n");
			writer.Write("      <td colspan=\"3\"></td>\n");
			writer.Write("    </tr>\n");
			writer.Write("  </table>\n");
			writer.Write("<p align=\"left\"><input type=\"button\" value=\"Add New Poll\" name=\"AddNew\" onClick=\"self.location='editPoll.aspx';\"><p>");
			writer.Write("</form>\n");

			writer.Write("</center></b>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeletePoll(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete Poll: ' + id))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'polls.aspx?deleteid=' + id;\n");
			writer.Write("}\n");
			writer.Write("}\n");
			writer.Write("</SCRIPT>\n");
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
