
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
	/// Summary description for sections.
	/// </summary>
	public class sections : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Manage " + Common.AppConfig("SectionPromptPlural");
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(Common.QueryString("DeleteID").Length != 0)
			{
				// delete the record:
				DB.ExecuteSQL("update [Section] set deleted=1, LastUpdatedBy=" + thisCustomer._customerID.ToString() + " where SectionID=" + Common.QueryString("DeleteID"));
				Common.ClearCache();
			}

			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				for(int i = 0; i<=Request.Form.Count-1; i++)
				{
					if(Request.Form.Keys[i].IndexOf("DisplayOrder_") != -1)
					{
						String[] keys = Request.Form.Keys[i].Split('_');
						int SectionID = Localization.ParseUSInt(keys[1]);
						int DispOrd = 1;
						try
						{
							DispOrd = Localization.ParseUSInt(Request.Form[Request.Form.Keys[i]]);
						}
						catch {}
						DB.ExecuteSQL("update [Section] set DisplayOrder=" + DispOrd.ToString() + " where SectionID=" + SectionID.ToString());
					}
				}
			}
			
			writer.Write("<form method=\"POST\" action=\"sections.aspx\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("<p align=\"left\"><input type=\"button\" value=\"Add New " + Common.AppConfig("SectionPromptSingular") + "\" name=\"AddNew\" onClick=\"self.location='editSection.aspx';\"><p>");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td><b>ID</b></td>\n");
			writer.Write("      <td><b>" + Common.AppConfig("SectionPromptSingular") + "</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Parent " + Common.AppConfig("SectionPromptSingular") + "</b></td>\n");
			writer.Write("      <td align=\"center\"><b>" + Common.AppConfig("SectionPromptSingular") + " Display Order</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Edit " + Common.AppConfig("SectionPromptSingular") + "</b></td>\n");
			writer.Write("      <td align=\"center\"><b>View Products</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Set Products Display Order</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete " + Common.AppConfig("SectionPromptSingular") + "</b></td>\n");
			writer.Write("    </tr>\n");
			
			GetSections(writer, 0,1);

			writer.Write("    <tr>\n");
			writer.Write("      <td colspan=\"3\" align=\"left\"></td>\n");
			writer.Write("      <td align=\"center\" bgcolor=\"" + Common.AppConfig("LightCellColor") + "\"><input type=\"submit\" value=\"Update\" name=\"Submit\"></td>\n");
			writer.Write("      <td colspan=\"4\"></td>\n");
			writer.Write("    </tr>\n");
			writer.Write("  </table>\n");
			writer.Write("<p align=\"left\"><input type=\"button\" value=\"Add New " + Common.AppConfig("SectionPromptSingular") + "\" name=\"AddNew\" onClick=\"self.location='editSection.aspx';\"><p>");
			writer.Write("</form>\n");

			writer.Write("</center></b>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteSection(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete " + Common.AppConfig("SectionPromptSingular") + ": ' + id))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'sections.aspx?deleteid=' + id;\n");
			writer.Write("}\n");
			writer.Write("}\n");
			writer.Write("</SCRIPT>\n");
		}

		private void GetSections(System.Web.UI.HtmlTextWriter writer, int ParentSectionID, int level)
		{
			String Indent = String.Empty;
			for(int i = 1; i < level; i++)
			{
				Indent += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
			}
			DataSet ds = DB.GetDS("select * from [Section] " + DB.GetNoLock() + " where " + Common.IIF(ParentSectionID == 0 , "(parentsectionid=0 or parentsectionid is null)" , "parentsectionid=" + ParentSectionID.ToString()) + " and deleted=0 order by displayorder,name",false,System.DateTime.Now.AddDays(1));
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
				writer.Write("      <td >" + DB.RowFieldInt(row,"SectionID").ToString() + "</td>\n");
				writer.Write("<td>\n");
				String Image1URL = Common.LookupImage("Section",DB.RowFieldInt(row,"SectionID"),"icon",_siteID);
				if(Image1URL.Length == 0)
				{
					Image1URL = "../" + Common.AppConfig("NoPictureIcon");
				}
				writer.Write("<a href=\"editSection.aspx?Sectionid=" + DB.RowFieldInt(row,"SectionID").ToString() + "\">");
				writer.Write("<img src=\"" + Image1URL + "\" height=\"25\" border=\"0\" align=\"absmiddle\">");
				writer.Write("</a>&nbsp;\n");
				writer.Write("<a href=\"editSection.aspx?Sectionid=" + DB.RowFieldInt(row,"SectionID").ToString() + "\">");
				if(level == 1)
				{
					writer.Write("<b>");
				}
				writer.Write(Indent + DB.RowField(row,"Name"));
				if(level == 1)
				{
					writer.Write("</b>");
				}
				writer.Write("</a>");
				writer.Write("</td>\n");
				writer.Write("<td align=\"center\">" + Common.GetSectionName(DB.RowFieldInt(row,"ParentSectionID")) + "</td>\n");
				writer.Write("<td align=\"center\"><input size=4 type=\"text\" name=\"DisplayOrder_" + DB.RowFieldInt(row,"SectionID").ToString() + "\" value=\"" + DB.RowFieldInt(row,"DisplayOrder").ToString() + "\"></td>\n");
				writer.Write("<td align=\"center\"><input type=\"button\" value=\"Edit\" name=\"Edit_" + DB.RowFieldInt(row,"SectionID").ToString() + "\" onClick=\"self.location='editSection.aspx?Sectionid=" + DB.RowFieldInt(row,"SectionID").ToString() + "'\"></td>\n");
				writer.Write("<td align=\"center\"><input type=\"button\" value=\"Products\" name=\"Products_" + DB.RowFieldInt(row,"SectionID").ToString() + "\" onClick=\"self.location='products.aspx?Sectionid=" + DB.RowFieldInt(row,"SectionID").ToString() + "&producttypeid=0&manufacturerid=0'\"></td>\n");
				writer.Write("<td align=\"center\"><input type=\"button\" value=\"DisplayOrder\" name=\"DisplayOrder_" + DB.RowFieldInt(row,"SectionID").ToString() + "\" onClick=\"self.location='displayorder.aspx?Sectionid=" + DB.RowFieldInt(row,"SectionID").ToString() + "'\"></td>\n");
				writer.Write("<td align=\"center\"><input type=\"button\" value=\"Delete\" name=\"Delete_" + DB.RowFieldInt(row,"SectionID").ToString() + "\" onClick=\"DeleteSection(" + DB.RowFieldInt(row,"SectionID").ToString() + ")\"></td>\n");
				writer.Write("</tr>\n");
				if(Common.SectionHasSubs(DB.RowFieldInt(row,"SectionID")))
				{
					GetSections(writer, DB.RowFieldInt(row,"SectionID"),level+1);
				}
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
