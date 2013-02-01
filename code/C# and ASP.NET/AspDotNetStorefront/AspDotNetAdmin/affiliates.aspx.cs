// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// $Header: /v5.0/AspDotNetStorefront/AspDotNetStorefrontAdmin/affiliates.aspx.cs 5     5/02/05 12:19p Redwoodtree $
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for Affiliates
	/// </summary>
	public class Affiliates : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			if(Common.QueryString("DeleteID").Length != 0)
			{
				DB.ExecuteSQL("update Affiliate set deleted=1, EMail='deleted_' + EMail, LastUpdatedBy=" + thisCustomer._customerID.ToString() + " where AffiliateID=" + Common.QueryString("DeleteID"));
			}
			SectionTitle = "Manage Affiliates";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write("<p><b>There are " + DB.GetSqlN("select count(affiliateid) as N from affiliate  " + DB.GetNoLock() + " where deleted=0").ToString() + " affiliates in the system</b></p>");

			decimal Balance = System.Decimal.Zero;
			if(Common.QueryString("Balance").Length > 0)
			{
				try
				{
					Balance = Common.QueryStringUSDecimal("Balance");
				}
				catch {}
			}
			if(Balance != System.Decimal.Zero)
			{
				writer.Write("<p><b>Showing ALL affiliates with account balances >= " + Localization.CurrencyStringForDisplay(Balance) + "</b></p>");
			}
			
			String SearchFor = Common.QueryString("SearchFor");
			writer.Write("<form method=\"GET\" action=\"Affiliates.aspx\">");
			String BeginsWith = Common.IIF(Common.QueryString("BeginsWith").Length == 0 , "A" , Common.QueryString("BeginsWith"));
			String alpha = "%#ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			for(int i = 1; i <= alpha.Length; i++)
			{
				if(BeginsWith[0] == alpha[i-1])
				{
					writer.Write(alpha[i-1] + "&nbsp;");
				}
				else
				{
					writer.Write("<a href=\"Affiliates.aspx?BeginsWith=" + Server.UrlEncode("" + alpha[i-1]) + "\">" + alpha[i-1] + "</a>&nbsp;");
				}
			}
			writer.Write("&nbsp;&nbsp;Search For: <input type=\"text\" name=\"SearchFor\" value=\"" + SearchFor + "\"><input type=\"submit\" name=\"search\" value=\"submit\">");
			writer.Write("</form>");
			
			writer.Write("<form method=\"POST\" action=\"Affiliates.aspx\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("<p align=\"left\"><input type=\"button\" value=\"Add New Affiliate\" name=\"AddNew\" onClick=\"self.location='lat_signup.aspx';\">\n");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"#EEEEEE\">\n");
			writer.Write("      <td width=\"100\" align=\"left\"><b>Affiliate ID</b></td>\n");
			writer.Write("      <td align=\"left\"><b>Name</b></td>\n");
			//writer.Write("      <td align=\"left\"><b>Account Balance</b></td>\n");
			//writer.Write("      <td align=\"left\"><b>Earnings</b></td>\n");
			writer.Write("      <td align=\"left\"><b>EMail</b></td>\n");
			writer.Write("      <td align=\"left\"><b>Ship To</b></td>\n");
			writer.Write("      <td align=\"left\"><b>URL</b></td>\n");
			//writer.Write("      <td align=\"left\"><b>Add Credit</b></td>\n");
			//writer.Write("      <td align=\"left\"><b>Make Payment</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete</b></td>\n");
			writer.Write("    </tr>\n");

			GetAffiliates(writer,0,1);

			writer.Write("  </table>\n");
			writer.Write("<p align=\"left\"><input type=\"button\" value=\"Add New Affiliate\" name=\"AddNew\" onClick=\"self.location='lat_signup.aspx';\">\n");
			writer.Write("</form>\n");

			writer.Write("</center></b>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("	function DeleteAffiliate(id)\n");
			writer.Write("	{\n");
			writer.Write("	if(confirm('Are you sure you want to delete Affiliate: ' + id + '?\\n\\nTheir account records, orders, and customer stats will remain in the DB, but their account will be disabled.'))\n");
			writer.Write("		{\n");
			writer.Write("		self.location = 'affiliates.aspx?deleteid=' + id;\n");
			writer.Write("		}\n");
			writer.Write("	}\n");
			writer.Write("</SCRIPT>\n");
		}

		private void GetAffiliates(System.Web.UI.HtmlTextWriter writer, int ParentAffiliateID, int level)
		{
			String Indent = String.Empty;
			for(int i = 1; i < level; i++)
			{
				Indent += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
			}
			DataSet ds = DB.GetDS("select * from Affiliate  " + DB.GetNoLock() + " where deleted=0 order by name",false,System.DateTime.Now.AddDays(1));
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("    <tr bgcolor=\"#EEEEEE\">\n");
				writer.Write("      <td width=\"100\" align=\"left\">" + DB.RowFieldInt(row,"AffiliateID").ToString() + "</td>\n");
				writer.Write("      <td align=\"left\">");
				writer.Write("<a href=\"lat_account.aspx?Affiliateid=" + DB.RowFieldInt(row,"AffiliateID").ToString() + "\">");
				writer.Write(Indent + Common.IIF(DB.RowField(row,"Name").Length != 0, DB.RowField(row,"Name"), "(no name)"));
				writer.Write("</a>");
				if(DB.RowFieldBool(row,"TrackingOnly"))
				{
					writer.Write("<br>(Ad Tracking Affiliate)");
				}
				writer.Write("</td>\n");
				//				if(Bal != System.Decimal.Zero && !DB.RowFieldBool(row,"TrackingOnly"))
				//				{
				//					writer.Write("      <td align=\"left\"><a href=\"lat_makepayment.aspx?Affiliateid=" + DB.RowFieldInt(row,"AffiliateID").ToString() + "\">" + Localization.CurrencyStringForDisplay( Bal) + "</a></td>\n");
				//				}
				//				else
				//				{
				//					writer.Write("      <td align=\"left\">-</td>\n");
				//				}
				//				if(!DB.RowFieldBool(row,"TrackingOnly"))
				//				{
				//					writer.Write("      <td align=\"left\"><a href=\"lat_earnings.aspx?Affiliateid=" + DB.RowFieldInt(row,"AffiliateID").ToString() + "\">View History</a></td>\n");
				//				}
				//				else
				//				{
				//					writer.Write("      <td align=\"left\">-</td>\n");
				//				}
				writer.Write("      <td align=\"left\"><a href=\"mailto:" + DB.RowField(row,"EMail") + "\">" + DB.RowField(row,"EMail") + "</a></td>\n");
				if(!DB.RowFieldBool(row,"TrackingOnly"))
				{
					writer.Write("<td align=\"left\">" + Common.AffiliateMailingAddress(DB.RowFieldInt(row,"AffiliateID"),"<br>") + "</td>\n");
				}
				else
				{
					writer.Write("<td align=\"left\">N/A</td>\n");
				}
				//				if(!DB.RowFieldBool(row,"TrackingOnly"))
				//				{
				//					writer.Write("      <td align=\"left\"><a href=\"lat_manualcredit.aspx?affiliateid=" + DB.RowFieldInt(row,"AffiliateID").ToString() + "\">Add Credit</a></td>\n");
				//				}
				//				else
				//				{
				//					writer.Write("      <td align=\"left\">-</td>\n");
				//				}
				//				if(!DB.RowFieldBool(row,"TrackingOnly"))
				//				{
				//					writer.Write("      <td align=\"left\"><a href=\"lat_makepayment.aspx?affiliateid=" + DB.RowFieldInt(row,"AffiliateID").ToString() + "\">Pay</a></td>\n");
				//				}
				//				else
				//				{
				//					writer.Write("      <td align=\"left\">-</td>\n");
				//				}
				writer.Write("<td>" + DB.RowField(row,"URL") + "&nbsp;</td>");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Delete\" name=\"Delete_" + DB.RowFieldInt(row,"AffiliateID").ToString() + "\" onClick=\"DeleteAffiliate(" + DB.RowFieldInt(row,"AffiliateID").ToString() + ")\"></td>\n");
				writer.Write("    </tr>\n");
				//if(Common.AffiliateHasSubs(DB.RowFieldInt(row,"AffiliateID")))
				//{
				//	GetAffiliates(writer,DB.RowFieldInt(row,"AffiliateID"),level+1);
				//}
			}
			ds.Dispose();
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
