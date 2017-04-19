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
	/// Summary description for expenses.
	/// </summary>
	public class expenses : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Expenses";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			int ExpenseCategoryID = Common.QueryStringUSInt("ExpenseCategoryID");

			if(Common.QueryString("ExpenseCategoryID").Length == 0)
			{
				if(Common.Cookie("ExpenseCategoryID",true).Length != 0)
				{
					ExpenseCategoryID = Localization.ParseUSInt(Common.Cookie("ExpenseCategoryID",true));
				}
			}

			Common.SetCookie("ExpenseCategoryID",ExpenseCategoryID.ToString(),new TimeSpan(365,0,0,0,0));

			if(Common.QueryString("DeleteID").Length != 0)
			{
				// delete the expense:
				DB.ExecuteSQL("delete from Expense where ExpenseID=" + Common.QueryString("DeleteID"));
			}

			writer.Write("<form id=\"FilterForm\" name=\"FilterForm\" method=\"GET\" action=\"expenses.aspx\">\n");

			writer.Write("Show Expenses In Category: <select size=\"1\" name=\"ExpenseCategoryID\" onChange=\"document.FilterForm.submit();\">\n");
			writer.Write("<OPTION VALUE=\"0\" " + Common.IIF(ExpenseCategoryID == 0 , " selected " , "") + ">All Categories</option>\n");
			DataSet dsst = DB.GetDS("select * from ExpenseCategory  " + DB.GetNoLock() + " where deleted=0 order by name",false,System.DateTime.Now.AddHours(3));
			foreach(DataRow row in dsst.Tables[0].Rows)
			{
				writer.Write("<option value=\"" + DB.RowFieldInt(row,"ExpenseCategoryID").ToString() + "\"");
				if(DB.RowFieldInt(row,"ExpenseCategoryID") == ExpenseCategoryID )
				{
					writer.Write(" selected");
				}
				writer.Write(">" + DB.RowField(row,"Name") + "</option>");
			}
			dsst.Dispose();
			writer.Write("</select>\n");

			writer.Write("</form>\n");

			String sql = "select * from Expense  " + DB.GetNoLock() + " left outer join ExpenseCategory  " + DB.GetNoLock() + " on Expense.ExpenseCategoryID=ExpenseCategory.ExpenseCategoryID " + Common.IIF(ExpenseCategoryID != 0 , " where Expense.ExpenseCategoryid=" + ExpenseCategoryID.ToString() , "") + " order by ExpenseDate desc";
			DataSet ds = DB.GetDS(sql,false,System.DateTime.Now.AddDays(1));

			writer.Write("<form id=\"Form1\" name=\"Form1\" method=\"POST\" action=\"expenses.aspx?ExpenseCategoryid=" + ExpenseCategoryID.ToString() + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("<p align=\"left\"><input type=\"button\" value=\"Add New Expense\" name=\"AddNew\" onClick=\"self.location='editExpense.aspx?ExpenseCategoryid=" + ExpenseCategoryID.ToString() + "';\"></p>");

			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td><b>ID</b></td>\n");
			writer.Write("      <td><b>Description</b></td>\n");
			writer.Write("      <td><b>Amount</b></td>\n");
			writer.Write("      <td><b>Category</b></td>\n");
			writer.Write("      <td><b>Date</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Edit</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete</b></td>\n");
			writer.Write("    </tr>\n");
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("<tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
				writer.Write("<td >" + DB.RowFieldInt(row,"ExpenseID").ToString() + "</td>\n");
				writer.Write("<td >");
				writer.Write("<a href=\"editExpense.aspx?Expenseid=" + DB.RowFieldInt(row,"ExpenseID").ToString() + "\">");
				writer.Write(Common.Ellipses(DB.RowField(row,"Description"),100,true));
				writer.Write("</a>");
				writer.Write("</td>\n");
				writer.Write("<td >" + Localization.CurrencyStringForDB(DB.RowFieldDecimal(row,"Amount")) + "</td>\n");
				writer.Write("<td >" + DB.RowField(row,"Name") + "</td>\n");
				writer.Write("<td >" + Localization.ToNativeShortDateString(DB.RowFieldDateTime(row,"ExpenseDate")) + "</td>\n");
				writer.Write("<td align=\"center\"><input type=\"button\" value=\"Edit\" name=\"Edit_" + DB.RowFieldInt(row,"ExpenseID").ToString() + "\" onClick=\"self.location='editExpense.aspx?Expenseid=" + DB.RowFieldInt(row,"ExpenseID").ToString() + "'\"></td>\n");
				writer.Write("<td align=\"center\"><input type=\"button\" value=\"Delete\" name=\"Delete_" + DB.RowFieldInt(row,"ExpenseID").ToString() + "\" onClick=\"DeleteExpense(" + DB.RowFieldInt(row,"ExpenseID").ToString() + ")\"></td>\n");
				writer.Write("</tr>\n");
			}
			ds.Dispose();
			writer.Write("  </table>\n");
			writer.Write("<p align=\"left\"><input type=\"button\" value=\"Add New Expense\" name=\"AddNew\" onClick=\"self.location='editExpense.aspx';\"></p>");
			writer.Write("</form>\n");

			writer.Write("</center></b>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteExpense(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete Expense: ' + id))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'Expenses.aspx?deleteid=' + id;\n");
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
