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
	/// Summary description for customerlevels.
	/// </summary>
	public class customerlevels : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			if(Common.QueryString("DeleteID").Length != 0)
			{
				// delete the level:
				DB.ExecuteSQL("delete from ExtendedPrice where CustomerLevelID=" + Common.QueryString("DeleteID"));
				DB.ExecuteSQL("update Customer set CustomerLevelID=0 where CustomerLevelID=" + Common.QueryString("DeleteID"));
				DB.ExecuteSQL("update CustomerLevel set deleted=1, LastUpdatedBy=" + thisCustomer._customerID.ToString() + " where CustomerLevelID=" + Common.QueryString("DeleteID"));
				Common.ClearCache();
			}
			SectionTitle = "Manage Customer Levels";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			DataSet ds = DB.GetDS("select * from CustomerLevel  " + DB.GetNoLock() + " where deleted=0 order by Name",false,System.DateTime.Now.AddDays(1));
			writer.Write("<form method=\"POST\" action=\"CustomerLevels.aspx\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("<table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("<tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("<td width=\"10%\"><b>ID</b></td>\n");
			writer.Write("<td ><b>Description</b></td>\n");
			writer.Write("<td width=\"20%\" align=\"center\"><b>View Customers Of This Level</b></td>\n");
			writer.Write("<td width=\"10%\" align=\"center\"><b>Edit</b></td>\n");
			writer.Write("<td width=\"10%\" align=\"center\"><b>Delete</b></td>\n");
			writer.Write("</tr>\n");
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("<tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
				writer.Write("<td width=\"10%\">" + DB.RowFieldInt(row,"CustomerLevelID").ToString() + "</td>\n");
				writer.Write("<td><a href=\"editCustomerLevel.aspx?CustomerLevelID=" + DB.RowFieldInt(row,"CustomerLevelID").ToString() + "\">" + DB.RowField(row,"Name") + "</a></td>\n");
				writer.Write("<td width=\"20%\" align=\"center\"><input type=\"button\" value=\"Show Customers\" name=\"Edit_" + DB.RowFieldInt(row,"CustomerLevelID").ToString() + "\" onClick=\"self.location='showCustomerLevel.aspx?CustomerLevelID=" + DB.RowFieldInt(row,"CustomerLevelID").ToString() + "'\"></td>\n");
				writer.Write("<td width=\"10%\" align=\"center\"><input type=\"button\" value=\"Edit\" name=\"Edit_" + DB.RowFieldInt(row,"CustomerLevelID").ToString() + "\" onClick=\"self.location='editCustomerLevel.aspx?CustomerLevelID=" + DB.RowFieldInt(row,"CustomerLevelID").ToString() + "'\"></td>\n");
				writer.Write("<td width=\"10%\" align=\"center\"><input type=\"button\" value=\"Delete\" name=\"Delete_" + DB.RowFieldInt(row,"CustomerLevelID").ToString() + "\" onClick=\"DeleteCustomerLevel(" + DB.RowFieldInt(row,"CustomerLevelID").ToString() + ")\"></td>\n");
				writer.Write("</tr>\n");
			}
			ds.Dispose();
			writer.Write(" </table>\n");
			writer.Write("<p align=\"left\"><input type=\"button\" value=\"Add New Customer Level\" name=\"AddNew\" onClick=\"self.location='editCustomerLevel.aspx';\"></p>\n");
			writer.Write("</form>\n");

			writer.Write("</center></b>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteCustomerLevel(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete Customer Level: ' + id))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'CustomerLevels.aspx?deleteid=' + id;\n");
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
