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
	/// Summary description for Customers
	/// </summary>
	public class Customers : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			if(Common.QueryString("DeleteID").Length != 0)
			{
				DB.ExecuteSQL("update Customer set deleted=1, Email=" + DB.SQuote("deleted_") + "+Email, LastUpdatedBy=" + thisCustomer._customerID.ToString() + " where CustomerID=" + Common.QueryString("DeleteID"));
			}
			if(Common.QueryString("NukeID").Length != 0)
			{
				// remove any download folders also:
				IDataReader rso = DB.GetRS("Select ordernumber from orders  " + DB.GetNoLock() + " where CustomerID=" + Common.QueryString("NukeID"));
				while(rso.Read())
				{
					if(DB.RSFieldDateTime(rso,"DownloadEmailSentOn") != System.DateTime.MinValue)
					{
						String dirname = DB.RSFieldInt(rso,"OrderNumber").ToString() + "_" + Common.QueryStringUSInt("NukeID").ToString();
						try
						{
							System.IO.Directory.Delete(Server.MapPath("../orderdownloads/" + dirname),true);
						}
						catch(Exception ex)
						{
							ErrorMsg = "Error nuking order directory " + dirname + ": " + Common.GetExceptionDetail(ex,"<br>");
						}
					}
				}
				rso.Close();
				DB.ExecuteSQL("delete from couponusage where CustomerID=" + Common.QueryString("NukeID"));
				DB.ExecuteSQL("delete from orders_customcart where CustomerID=" + Common.QueryString("NukeID"));
				DB.ExecuteSQL("delete from orders_kitcart where CustomerID=" + Common.QueryString("NukeID"));
				DB.ExecuteSQL("delete from orders_ShoppingCart where CustomerID=" + Common.QueryString("NukeID"));
				DB.ExecuteSQL("delete from orders where CustomerID=" + Common.QueryString("NukeID"));
				DB.ExecuteSQL("delete from ShoppingCart where CustomerID=" + Common.QueryString("NukeID"));
				DB.ExecuteSQL("delete from failedtransaction where CustomerID=" + Common.QueryString("NukeID"));
				DB.ExecuteSQL("delete from kitcart where CustomerID=" + Common.QueryString("NukeID"));
				DB.ExecuteSQL("delete from customcart where CustomerID=" + Common.QueryString("NukeID"));
				DB.ExecuteSQL("delete from log_customerevent where CustomerID=" + Common.QueryString("NukeID"));
				DB.ExecuteSQL("delete from pollvotingrecord where CustomerID=" + Common.QueryString("NukeID"));
				DB.ExecuteSQL("delete from ratingcommenthelpfulness where RatingCustomerID=" + Common.QueryString("NukeID") + " or VotingCustomerID=" + Common.QueryString("NukeID"));
				DB.ExecuteSQL("delete from rating where CustomerID=" + Common.QueryString("NukeID"));
				DB.ExecuteSQL("delete from customer where CustomerID=" + Common.QueryString("NukeID"));
			}
			if(Common.QueryString("SetAdminID").Length != 0)
			{
				DB.ExecuteSQL("update Customer set IsAdmin=1, LastUpdatedBy=" + thisCustomer._customerID.ToString() + " where CustomerID=" + Common.QueryString("SetAdminID"));
			}
			if(Common.QueryString("ClearAdminID").Length != 0)
			{
				DB.ExecuteSQL("update Customer set IsAdmin=0, LastUpdatedBy=" + thisCustomer._customerID.ToString() + " where CustomerID=" + Common.QueryString("ClearAdminID"));
			}
			if(Common.QueryString("DeletePollsID").Length != 0)
			{
				DB.ExecuteSQL("delete from PollVotingRecord where CustomerID=" + Common.QueryString("DeletePollsID"));
			}
			if(Common.QueryString("DeleteRatingsID").Length != 0)
			{
				DB.ExecuteSQL("delete from Rating where CustomerID=" + Common.QueryString("DeleteRatingsID"));
				DB.ExecuteSQL("delete from RatingCommentHelpfulness where VotingCustomerID=" + Common.QueryString("DeleteRatingsID") + " or RatingCustomerID=" + Common.QueryString("DeleteRatingsID"));
			}
			SectionTitle = "Manage Customers";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write("<p><b>There are " + DB.GetSqlN("select count(Customerid) as N from customer  " + DB.GetNoLock() + " where email not like " + DB.SQuote("Anon_%") + " and deleted=0").ToString() + " registered customers in the system</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"cst_signup.aspx\"><b>Add A New Customer</b></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"cst_export.aspx\"><b>Export to XML</b></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"rpt_emails.aspx\"><b>Get E-Mails</b></a></p>");

			String SearchFor = Common.QueryString("SearchFor");
			writer.Write("<form method=\"GET\" action=\"Customers.aspx\">");
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
					writer.Write("<a href=\"Customers.aspx?BeginsWith=" + Server.UrlEncode("" + alpha[i-1]) + "\">" + alpha[i-1] + "</a>&nbsp;");
				}
			}
			writer.Write("&nbsp;&nbsp;Search For: <input type=\"text\" name=\"SearchFor\" value=\"" + SearchFor + "\"><input type=\"submit\" name=\"search\" value=\"submit\">");
			writer.Write("</form>");

			
			String SQL = String.Empty;
			//SEC4
			string SuperuserFilter = (thisCustomer.IsAdminSuperUser) ? String.Empty : String.Format(" Customer.CustomerID not in ({0}) and ",Common.AppConfig("Admin_Superuser"));

			if(SearchFor.Length != 0)
			{
				int CID = 0;
				if(Common.IsInteger(SearchFor))
				{
					CID = Localization.ParseUSInt(SearchFor);
				}
				SQL = "select distinct Customer.CustomerID,Customer.CustomerLevelID,Customer.CreatedOn,Customer.FirstName,Customer.LastName,Customer.IsAdmin,SubscriptionExpiresOn,Customer.Email,OKToEmail,Customer.CustomerLevelID  " 
					+ " from Customer  " + DB.GetNoLock() + " Left Outer Join Address  " + DB.GetNoLock() + " ON Address.CustomerID=Customer.CustomerID "
					+ " where " + SuperuserFilter + " Customer.email not like " + DB.SQuote("Anon_%") + " and Customer.deleted=0 and (Customer.lastname like " + DB.SQuote("%" + SearchFor + "%") + Common.IIF(CID != 0," or Customer.CustomerID=" + CID.ToString(),"") + " or Address.Company like " + DB.SQuote("%" + SearchFor + "%") + " or Customer.firstname like " + DB.SQuote("%" + SearchFor + "%") + " or Customer.email like " + DB.SQuote("%" + SearchFor + "%") + ") order by Customer.lastname, Customer.firstname";
			}
			else
			{
				SQL = "select distinct Customer.CustomerID,Customer.CustomerLevelID,Customer.CreatedOn,Customer.FirstName,Customer.LastName,Customer.IsAdmin,SubscriptionExpiresOn,Customer.Email,OKToEmail,Customer.CustomerLevelID "
					+ " from Customer  " + DB.GetNoLock() + " Left Outer Join Address  " + DB.GetNoLock() + " ON Address.CustomerID=Customer.CustomerID "
					+ " where " + SuperuserFilter + " Customer.email not like " + DB.SQuote("Anon_%") + " and Customer.deleted=0 and Customer.lastname like " + DB.SQuote(BeginsWith + "%") + " order by Customer.lastname, Customer.firstname";
			}
	
			DataSet ds = DB.GetDS(SQL,false,System.DateTime.Now.AddDays(1));
			writer.Write("<form method=\"POST\" action=\"Customers.aspx?beginswith=" + BeginsWith + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"#EEEEEE\">\n");
			writer.Write("      <td width=\"100\" align=\"left\"><b>Customer ID</b></td>\n");
			writer.Write("      <td align=\"left\"><b>Name</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Order History</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Admin</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Level</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Subscription<br>Expires On</b></td>\n");
			writer.Write("      <td align=\"left\"><b>EMail</b></td>\n");
			writer.Write("      <td align=\"left\"><b>Ship To</b></td>\n");
			writer.Write("      <td align=\"left\"><b>Bill To</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Show Ratings</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete Customer Poll Votes</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete Customer Product Ratings</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete Customer</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Nuke Customer</b></td>\n");
			writer.Write("    </tr>\n");

			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("<tr bgcolor=\"#EEEEEE\">\n");
				writer.Write("<td width=\"100\" align=\"left\"><a href=\"cst_account.aspx?Customerid=" + DB.RowFieldInt(row,"CustomerID").ToString() + "\">" + DB.RowFieldInt(row,"CustomerID").ToString() + "</a><br>" + Localization.ToNativeShortDateString(DB.RowFieldDateTime(row,"CreatedOn")) + "</td>\n");
				writer.Write("<td align=\"left\"><a href=\"cst_account.aspx?Customerid=" + DB.RowFieldInt(row,"CustomerID").ToString() + "\">" + (DB.RowField(row,"FirstName") + " " + DB.RowField(row,"LastName")).Trim() + "</a></td>\n");
				if(Customer.HasOrders(DB.RowFieldInt(row,"CustomerID")))
				{
					writer.Write("<td align=\"center\"><a href=\"cst_history.aspx?Customerid=" + DB.RowFieldInt(row,"CustomerID").ToString() + "\">View</a></td>\n");
				}
				else
				{
					writer.Write("<td align=\"center\">None</td>\n");
				}
				if(DB.RowFieldBool(row,"IsAdmin"))
				{
					if(Customer.StaticIsAdminSuperUser(DB.RowFieldInt(row,"CustomerID")))
					{
						writer.Write("<td align=\"center\"><b>Yes " + Common.IIF(Customer.StaticIsAdminSuperUser(DB.RowFieldInt(row,"CustomerID")) , "(SuperUser)" , "") + "</b></td>\n");
					}
					else
					{
						writer.Write("<td align=\"center\"><b>Yes</b><br>");
						writer.Write("<input type=\"button\" value=\"ClearAdmin\" name=\"ClearAdmin_" + DB.RowFieldInt(row,"CustomerID").ToString() + "\" onClick=\"ClearAdmin(" + DB.RowFieldInt(row,"CustomerID").ToString() + ")\">");
						writer.Write("</td>\n");
					}
				}
				else
				{
					writer.Write("<td align=\"center\">No<br><input type=\"button\" value=\"SetAdmin\" name=\"SetAdmin_" + DB.RowFieldInt(row,"CustomerID").ToString() + "\" onClick=\"SetAdmin(" + DB.RowFieldInt(row,"CustomerID").ToString() + ")\"></td>\n");
				}
				writer.Write("<td align=\"center\">" + Common.GetCustomerLevelName(DB.RowFieldInt(row,"CustomerLevelID")) + "</td>\n");
				writer.Write("<td align=\"center\">" + Common.IIF(DB.RowFieldDateTime(row,"SubscriptionExpiresOn") != System.DateTime.MinValue , Localization.ToNativeShortDateString(DB.RowFieldDateTime(row,"SubscriptionExpiresOn")) , "N/A") + "</td>\n");
				writer.Write("<td align=\"left\"><a href=\"mailto:" + DB.RowField(row,"EMail") + "\">" + DB.RowField(row,"EMail") + "</a><br>OkToEmail=" + DB.RowFieldBool(row,"OkToEMail").ToString() + "</td>\n");
				writer.Write("<td align=\"left\">" + Customer.ShipToAddress(true,DB.RowFieldInt(row,"CustomerID"),"<br>") + "</td>\n");
				writer.Write("<td align=\"left\">" + Customer.BillToAddress(true,DB.RowFieldInt(row,"CustomerID"),"<br>") + "</td>\n");
				writer.Write("<td align=\"center\"><input type=\"button\" value=\"Show Ratings\" name=\"ShowRatings_" + DB.RowFieldInt(row,"CustomerID").ToString() + "\" onClick=\"self.location='customerratings.aspx?customerid=" + DB.RowFieldInt(row,"CustomerID").ToString() + "'\"></td>\n");
				writer.Write("<td align=\"center\"><input type=\"button\" value=\"Delete Polls\" name=\"DeletePolls_" + DB.RowFieldInt(row,"CustomerID").ToString() + "\" onClick=\"DeletePolls(" + DB.RowFieldInt(row,"CustomerID").ToString() + ")\"></td>\n");
				writer.Write("<td align=\"center\"><input type=\"button\" value=\"Delete Ratings\" name=\"DeleteRatings_" + DB.RowFieldInt(row,"CustomerID").ToString() + "\" onClick=\"DeleteRatings(" + DB.RowFieldInt(row,"CustomerID").ToString() + ")\"></td>\n");
				writer.Write("<td align=\"center\"><input type=\"button\" value=\"Delete\" name=\"Delete_" + DB.RowFieldInt(row,"CustomerID").ToString() + "\" onClick=\"DeleteCustomer(" + DB.RowFieldInt(row,"CustomerID").ToString() + ")\"></td>\n");
				writer.Write("<td align=\"center\"><input type=\"button\" value=\"Nuke\" name=\"Nuke_" + DB.RowFieldInt(row,"CustomerID").ToString() + "\" onClick=\"NukeCustomer(" + DB.RowFieldInt(row,"CustomerID").ToString() + ")\"></td>\n");
				writer.Write("</tr>\n");
			}
			ds.Dispose();
			writer.Write("  </table>\n");
			writer.Write("<p align=\"left\"><input type=\"button\" value=\"Add New Customer\" name=\"AddNew\" onClick=\"self.location='cst_signup.aspx';\"></p>\n");
			writer.Write("</form>\n");

			writer.Write("</center></b>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteCustomer(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete this customer?\\n\\nTheir account records, orders, and customer stats will remain in the DB, but their account will be disabled.'))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'customers.aspx?beginswith=" + BeginsWith + "&deleteid=' + id;\n");
			writer.Write("}\n");
			writer.Write("}\n");
			writer.Write("	function NukeCustomer(id)\n");
			writer.Write("	{\n");
			writer.Write("	if(confirm('Are you sure you want to nuke this customer?\\n\\nEvery trace of this customer will be completely deleted from the database, including orders. There is no undo.'))\n");
			writer.Write("		{\n");
			writer.Write("		self.location = 'customers.aspx?beginswith=" + BeginsWith + "&nukeid=' + id;\n");
			writer.Write("		}\n");
			writer.Write("	}\n");
			writer.Write("function DeletePolls(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete all poll votes by this customer?\\n\\nTheir account records, orders, and customer stats will remain in the DB unchanged.'))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'customers.aspx?beginswith=" + BeginsWith + "&deletepollsid=' + id;\n");
			writer.Write("}\n");
			writer.Write("}\n");
			writer.Write("function DeleteRatings(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete all product ratings by this customer?\\n\\nTheir account records, orders, and customer stats will remain in the DB unchanged.'))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'customers.aspx?beginswith=" + BeginsWith + "&deleteratingsid=' + id;\n");
			writer.Write("}\n");
			writer.Write("}\n");
			writer.Write("function SetAdmin(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to set admin rights for this user?'))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'customers.aspx?beginswith=" + BeginsWith + "&setadminid=' + id;\n");
			writer.Write("}\n");
			writer.Write("}\n");
			writer.Write("function ClearAdmin(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to clear admin rights for this user?'))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'customers.aspx?beginswith=" + BeginsWith + "&clearadminid=' + id;\n");
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
