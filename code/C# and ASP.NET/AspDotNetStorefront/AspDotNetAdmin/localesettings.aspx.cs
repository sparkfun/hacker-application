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
	/// Summary description for localesetting.
	/// </summary>
	public class localesetting : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Manage Locale Settings (Brands)";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(Common.QueryString("DeleteID").Length != 0)
			{
				// delete the record:
				DB.ExecuteSQL("delete from localesetting where localesettingid=" + Common.QueryString("DeleteID"));
			}

			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				for(int i = 0; i<=Request.Form.Count-1; i++)
				{
					if(Request.Form.Keys[i].IndexOf("DisplayOrder_") != -1)
					{
						String[] keys = Request.Form.Keys[i].Split('_');
						int localesettingid = Localization.ParseUSInt(keys[1]);
						int DispOrd = 1;
						try
						{
							DispOrd = Localization.ParseUSInt(Request.Form[Request.Form.Keys[i]]);
						}
						catch {}
						DB.ExecuteSQL("update localesetting set DisplayOrder=" + DispOrd.ToString() + " where localesettingid=" + localesettingid.ToString());
					}
				}
			}
			
			DataSet ds = DB.GetDS("select * from localesetting  " + DB.GetNoLock() + " order by displayorder,description",false,System.DateTime.Now.AddDays(1));
			writer.Write("<form method=\"POST\" action=\"LocaleSettings.aspx\">\n");
			writer.Write("<p align=\"left\"><input type=\"button\" value=\"Add New Locale\" name=\"AddNew\" onClick=\"self.location='editlocalesetting.aspx';\"><p>");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td ><b>ID</b></td>\n");
			writer.Write("      <td ><b>Locale Setting</b></td>\n");
			writer.Write("      <td ><b>Description</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Display Order</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Edit</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete</b></td>\n");
			writer.Write("    </tr>\n");
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("<tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
				writer.Write("<td>" + DB.RowFieldInt(row,"localesettingid").ToString() + "</td>\n");
				writer.Write("<td >");
				writer.Write("<a href=\"editlocalesetting.aspx?localesettingid=" + DB.RowFieldInt(row,"localesettingid").ToString() + "\">");
				writer.Write(DB.RowField(row,"Name"));
				writer.Write("</a>");
				writer.Write("</td>\n");
				writer.Write("<td >" + DB.RowField(row,"Description") + "</td>\n");
				writer.Write("<td align=\"center\"><input size=2 type=\"text\" name=\"DisplayOrder_" + DB.RowFieldInt(row,"localesettingid").ToString() + "\" value=\"" + DB.RowFieldInt(row,"DisplayOrder").ToString() + "\"></td>\n");
				writer.Write("<td align=\"center\"><input type=\"button\" value=\"Edit\" name=\"Edit_" + DB.RowFieldInt(row,"localesettingid").ToString() + "\" onClick=\"self.location='editlocalesetting.aspx?localesettingid=" + DB.RowFieldInt(row,"localesettingid").ToString() + "'\"></td>\n");
				writer.Write("<td align=\"center\"><input type=\"button\" value=\"Delete\" name=\"Delete_" + DB.RowFieldInt(row,"localesettingid").ToString() + "\" onClick=\"Deletelocalesetting(" + DB.RowFieldInt(row,"localesettingid").ToString() + ")\"></td>\n");
				writer.Write("</tr>\n");
			}
			ds.Dispose();
			writer.Write("<tr>\n");
			writer.Write("<td colspan=\"3\" align=\"left\"></td>\n");
			writer.Write("<td align=\"center\" bgcolor=\"" + Common.AppConfig("LightCellColor") + "\"><input type=\"submit\" value=\"Update\" name=\"Submit\"></td>\n");
			writer.Write("<td colspan=\"3\"></td>\n");
			writer.Write("</tr>\n");
			writer.Write("<tr>\n");
			writer.Write("<td colspan=\"7\" height=5></td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("<p align=\"left\"><input type=\"button\" value=\"Add New Locale\" name=\"AddNew\" onClick=\"self.location='editlocalesetting.aspx';\"><p>");
			writer.Write("</form>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function Deletelocalesetting(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete Locale: ' + id))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'localesettings.aspx?deleteid=' + id;\n");
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
