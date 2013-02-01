
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for states.
	/// </summary>
	public class states : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Manage States";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(Common.QueryString("DeleteID").Length != 0)
			{
				// delete the record:
				DB.ExecuteSQL("delete from state where StateID=" + Common.QueryString("DeleteID"));
			}

			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				for(int i = 0; i<=Request.Form.Count-1; i++)
				{
					if(Request.Form.Keys[i].IndexOf("DisplayOrder_") != -1)
					{
						String[] keys = Request.Form.Keys[i].Split('_');
						int StateID = Localization.ParseUSInt(keys[1]);
						int DispOrd = 1;
						try
						{
							DispOrd = Localization.ParseUSInt(Request.Form[Request.Form.Keys[i]]);
						}
						catch {}
						DB.ExecuteSQL("update State set DisplayOrder=" + DispOrd.ToString() + " where StateID=" + StateID.ToString());
					}
				}
			}
			
			writer.Write("<form method=\"POST\" action=\"states.aspx\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("<p align=\"left\"><input type=\"button\" value=\"Add New State\" name=\"AddNew\" onClick=\"self.location='editState.aspx';\"><p>");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td><b>ID</b></td>\n");
			writer.Write("      <td><b>State/Province</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Abbreviation</b></td>\n");
			writer.Write("      <td align=\"center\"><b>State Display Order</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Edit State</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete State</b></td>\n");
			writer.Write("    </tr>\n");
			
			DataSet ds = DB.GetDS("select * from State  " + DB.GetNoLock() + " order by displayorder,name",false,System.DateTime.Now.AddDays(1));
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("<tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
				writer.Write("<td >" + DB.RowFieldInt(row,"StateID").ToString() + "</td>\n");
				writer.Write("<td align=\"center\">" + DB.RowField(row,"Name") + "</td>\n");
				writer.Write("<td align=\"center\">" + DB.RowField(row,"Abbreviation") + "</td>\n");
				writer.Write("<td align=\"center\"><input size=4 type=\"text\" name=\"DisplayOrder_" + DB.RowFieldInt(row,"StateID").ToString() + "\" value=\"" + DB.RowFieldInt(row,"DisplayOrder").ToString() + "\"></td>\n");
				writer.Write("<td align=\"center\"><input type=\"button\" value=\"Edit\" name=\"Edit_" + DB.RowFieldInt(row,"StateID").ToString() + "\" onClick=\"self.location='editState.aspx?Stateid=" + DB.RowFieldInt(row,"StateID").ToString() + "'\"></td>\n");
				writer.Write("<td align=\"center\"><input type=\"button\" value=\"Delete\" name=\"Delete_" + DB.RowFieldInt(row,"StateID").ToString() + "\" onClick=\"DeleteState(" + DB.RowFieldInt(row,"StateID").ToString() + ")\"></td>\n");
				writer.Write("</tr>\n");
			}
			ds.Dispose();

			writer.Write("    <tr>\n");
			writer.Write("      <td colspan=\"3\" align=\"left\"></td>\n");
			writer.Write("      <td align=\"center\" bgcolor=\"" + Common.AppConfig("LightCellColor") + "\"><input type=\"submit\" value=\"Update\" name=\"Submit\"></td>\n");
			writer.Write("      <td colspan=\"2\"></td>\n");
			writer.Write("    </tr>\n");
			writer.Write("  </table>\n");
			writer.Write("<p align=\"left\"><input type=\"button\" value=\"Add New State\" name=\"AddNew\" onClick=\"self.location='editState.aspx';\"><p>");
			writer.Write("</form>\n");

			writer.Write("</center></b>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteState(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete State: ' + id))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'states.aspx?deleteid=' + id;\n");
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
