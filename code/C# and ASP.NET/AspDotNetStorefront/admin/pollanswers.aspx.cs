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
	/// Summary description for pollanswers.
	/// </summary>
	public class pollanswers : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "<a href=\"Polls.aspx\">Manage Polls</a> - Add/Edit Answers";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			int PollID = Common.QueryStringUSInt("PollID");
			if(PollID == 0)
			{
				Response.Redirect("Polls.aspx");
			}

			String PollName = Common.GetPollName(PollID);

			if(Common.QueryString("DeleteID").Length != 0)
			{
				// delete the mfg:
				DB.ExecuteSQL("update PollAnswer set deleted=1, LastUpdatedBy=" + thisCustomer._customerID.ToString() + " where PollAnswerID=" + Common.QueryString("DeleteID"));
				Common.ClearCache();
			}

			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				for(int i = 0; i<=Request.Form.Count-1; i++)
				{
					if(Request.Form.Keys[i].IndexOf("DisplayOrder_") != -1)
					{
						String[] keys = Request.Form.Keys[i].Split('_');
						int PollAnswerID = Localization.ParseUSInt(keys[1]);
						int DispOrd = 1;
						try
						{
							DispOrd = Localization.ParseUSInt(Request.Form[Request.Form.Keys[i]]);
						}
						catch {}
						DB.ExecuteSQL("update PollAnswer set DisplayOrder=" + DispOrd.ToString() + " where PollAnswerID=" + PollAnswerID.ToString());
					}
				}
			}
			
			writer.Write("<p align=\"left\"<b>Editing Answers for Poll: <a href=\"editPoll.aspx?Pollid=" + PollID.ToString() + "\">" + PollName + "</a> (PollID=" + PollID.ToString() + ")</b></p>\n");

			DataSet ds = DB.GetDS("select * from PollAnswer  " + DB.GetNoLock() + " where deleted=0 and PollID=" + PollID.ToString() + " order by displayorder,name",false,System.DateTime.Now.AddDays(1));

			writer.Write("<form id=\"Form1\" name=\"Form1\" method=\"POST\" action=\"pollanswers.aspx?Pollid=" + PollID.ToString() + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td><b>ID</b></td>\n");
			writer.Write("      <td><b>Answer</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Display Order</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Edit</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete</b></td>\n");
			writer.Write("    </tr>\n");
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
				writer.Write("      <td >" + DB.RowFieldInt(row,"PollAnswerID").ToString() + "</td>\n");
				writer.Write("      <td >");
				writer.Write("<a href=\"editpollanswer.aspx?Pollid=" + PollID.ToString() + "&PollAnswerid=" + DB.RowFieldInt(row,"PollAnswerID").ToString() + "\">" + Common.IIF(DB.RowField(row,"Name").Length == 0 , "(Blank)" , DB.RowField(row,"Name")) + "</a>");
				writer.Write("</td>\n");
				writer.Write("      <td align=\"center\"><input size=2 type=\"text\" name=\"DisplayOrder_" + DB.RowFieldInt(row,"PollAnswerID").ToString() + "\" value=\"" + DB.RowFieldInt(row,"DisplayOrder").ToString() + "\"></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Edit\" name=\"Edit_" + DB.RowFieldInt(row,"PollAnswerID").ToString() + "\" onClick=\"self.location='editpollanswer.aspx?Pollid=" + PollID.ToString() + "&PollAnswerid=" + DB.RowFieldInt(row,"PollAnswerID").ToString() + "'\"></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Delete\" name=\"Delete_" + DB.RowFieldInt(row,"PollAnswerID").ToString() + "\" onClick=\"DeleteAnswer(" + DB.RowFieldInt(row,"PollAnswerID").ToString() + ")\"></td>\n");
				writer.Write("    </tr>\n");
			}
			ds.Dispose();
			writer.Write("    <tr>\n");
			writer.Write("      <td colspan=\"2\" align=\"left\"></td>\n");
			writer.Write("      <td align=\"center\" bgcolor=\"" + Common.AppConfig("LightCellColor") + "\"><input type=\"submit\" value=\"Update\" name=\"Submit\"></td>\n");
			writer.Write("      <td colspan=\"2\"></td>\n");
			writer.Write("    </tr>\n");
			writer.Write("    <tr>\n");
			writer.Write("      <td colspan=\"5\" height=5></td>\n");
			writer.Write("    </tr>\n");
			writer.Write("  </table>\n");
			writer.Write(" <input type=\"button\" value=\"Add New Answer\" name=\"AddNew\" onClick=\"self.location='editpollanswer.aspx?Pollid=" + PollID.ToString() + "';\">\n");
			writer.Write("</form>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteAnswer(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete Answer: ' + id))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'pollanswers.aspx?Pollid=" + PollID.ToString() + "&deleteid=' + id;\n");
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
