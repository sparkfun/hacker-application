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
	/// Summary description for managepoll.
	/// </summary>
	public class managepoll : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "<a href=\"Polls.aspx\">Manage Polls</a> - Review Customer Votes";
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
				DB.ExecuteSQL("delete from PollVotingRecord where PollVotingRecordID=" + Common.QueryString("DeleteID"));
				Common.ClearCache();
			}

			if(Common.QueryString("BanID").Length != 0)
			{
				DB.ExecuteSQL("delete from PollVotingRecord where CustomerID=" + Common.QueryString("BanID"));
			}

			writer.Write("<p align=\"left\"<b>Reviewing Votes for Poll: <a href=\"editPoll.aspx?Pollid=" + PollID.ToString() + "\">" + PollName + "</a> (PollID=" + PollID.ToString() + ")</b></p>\n");

			DataSet ds = DB.GetDS("SELECT  Customer.Email, Customer.FirstName AS CFN, Customer.LastName AS CLN, Customer.FirstName + ' ' + Customer.LastName AS Name, PollVotingRecord.PollVotingRecordID,PollVotingRecord.AnsweredOn, PollVotingRecord.PollID, PollAnswer.Name AS AnswerName, PollVotingRecord.PollAnswerID, PollVotingRecord.CustomerID FROM (PollVotingRecord LEFT OUTER JOIN Customer ON PollVotingRecord.CustomerID = Customer.CustomerID) LEFT OUTER JOIN PollAnswer ON PollVotingRecord.PollAnswerID = PollAnswer.PollAnswerID where PollVotingRecord.PollID=" + PollID.ToString() + " order by AnsweredOn desc",false,System.DateTime.Now.AddDays(1));


			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td><b>Record ID</b></td>\n");
			writer.Write("      <td><b>Answered On</b></td>\n");
			writer.Write("      <td><b>Customer ID</b></td>\n");
			writer.Write("      <td><b>Customer EMail</b></td>\n");
			writer.Write("      <td><b>Customer Name</b></td>\n");
			writer.Write("      <td><b>Answer Picked</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete</b></td>\n");
			writer.Write("      <td align=\"center\"><b>BAN Customer</b></td>\n");
			writer.Write("    </tr>\n");
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
				writer.Write("      <td>" + DB.RowFieldInt(row,"PollVotingRecordID").ToString() + "</td>\n");
				writer.Write("      <td>" + DB.RowFieldDateTime(row,"AnsweredOn").ToString() + "</td>\n");
				writer.Write("      <td>" + DB.RowFieldInt(row,"CustomerID").ToString() + "</td>\n");
				writer.Write("      <td>" + DB.RowField(row,"EMail") + "</td>\n");
				writer.Write("      <td>" + (DB.RowField(row,"CFN") + " " + DB.RowField(row,"CLN")).Trim() + "</td>\n");
				writer.Write("      <td>" + DB.RowField(row,"AnswerName") + "</td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Delete\" name=\"Delete_" + DB.RowFieldInt(row,"PollVotingRecordID").ToString() + "\" onClick=\"DeleteVote(" + DB.RowFieldInt(row,"PollVotingRecordID").ToString() + ")\"></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"BAN\" name=\"BanCustomer_" + DB.RowFieldInt(row,"CustomerID").ToString() + "\" onClick=\"BanCustomer(" + DB.RowFieldInt(row,"CustomerID").ToString() + ")\"></td>\n");
				writer.Write("    </tr>\n");
			}
			ds.Dispose();
			writer.Write("  </table>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteVote(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete this vote?'))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'managepoll.aspx?Pollid=" + PollID.ToString() + "&deleteid=' + id;\n");
			writer.Write("}\n");
			writer.Write("}\n");
			writer.Write("function BanCustomer(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to remove this customers votes from ALL polls?'))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'managepoll.aspx?Pollid=" + PollID.ToString() + "&banid=' + id;\n");
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
