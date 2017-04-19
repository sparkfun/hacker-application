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
	/// Summary description for salesprompts.
	/// </summary>
	public class salesprompts : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Manage Sales Prompts";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(Common.QueryString("DeleteID").Length != 0)
			{
				// delete the mfg:
				DB.ExecuteSQL("update salesprompt set deleted=1, LastUpdatedBy=" + thisCustomer._customerID.ToString() + " where SalesPromptID=" + Common.QueryString("DeleteID"));
				Common.ClearCache();
			}

			DataSet ds = DB.GetDS("select * from salesprompt  " + DB.GetNoLock() + " where deleted=0 order by name",false,System.DateTime.Now.AddDays(1));
			writer.Write("<form method=\"POST\" action=\"salesprompts.aspx\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td><b>ID</b></td>\n");
			writer.Write("      <td><b>SalesPrompt</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Edit</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete</b></td>\n");
			writer.Write("    </tr>\n");
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
				writer.Write("      <td>" + DB.RowFieldInt(row,"SalesPromptID").ToString() + "</td>\n");
				writer.Write("      <td><a href=\"editSalesPrompt.aspx?SalesPromptid=" + DB.RowFieldInt(row,"SalesPromptID").ToString() + "\">" + DB.RowField(row,"Name") + "</a></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Edit\" name=\"Edit_" + DB.RowFieldInt(row,"SalesPromptID").ToString() + "\" onClick=\"self.location='editSalesPrompt.aspx?SalesPromptid=" + DB.RowFieldInt(row,"SalesPromptID").ToString() + "'\"></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Delete\" name=\"Delete_" + DB.RowFieldInt(row,"SalesPromptID").ToString() + "\" onClick=\"DeleteSalesPrompt(" + DB.RowFieldInt(row,"SalesPromptID").ToString() + ")\"></td>\n");
				writer.Write("    </tr>\n");
			}
			ds.Dispose();
			writer.Write("    <tr>\n");
			writer.Write("      <td align=\"left\"><input type=\"button\" value=\"Add New Sales Prompt\" name=\"AddNew\" onClick=\"self.location='editSalesPrompt.aspx';\"></td>\n");
			writer.Write("      <td colspan=\"3\"></td>\n");
			writer.Write("    </tr>\n");
			writer.Write("  </table>\n");
			writer.Write("</form>\n");

			writer.Write("</center></b>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteSalesPrompt(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete SalesPrompt: ' + id))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'salesprompts.aspx?deleteid=' + id;\n");
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
