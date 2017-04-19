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
	/// Summary description for appconfigv1
	/// </summary>
	public class appconfigv1 : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			if(Common.QueryString("DeleteID").Length != 0)
			{
				DB.ExecuteSQL("delete from AppConfig where AppConfigID=" + Common.QueryString("DeleteID"));
				Common.ClearCache();
			}
			SectionTitle = "Manage AppConfig Parameters";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			String SearchFor = Common.QueryString("SearchFor");
			String GroupName = Common.QueryString("GroupName");
			writer.Write("<form id=\"AppConfigForm\" name=\"AppConfigForm\" method=\"GET\" action=\"appconfig.aspx\">");
			DataSet ds = DB.GetDS("select distinct groupname from appconfig  " + DB.GetNoLock() + " where groupname is not null order by groupname",true,System.DateTime.Now.AddHours(1));
			writer.Write("Config Group: <select onChange=\"document.AppConfigForm.submit()\" size=\"1\" name=\"GroupName\">\n");
			writer.Write("<OPTION VALUE=\"0\">ALL GROUPS</option>\n");
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("<option value=\"" + DB.RowField(row,"GroupName") + "\"");
				if(DB.RowField(row,"GroupName") == GroupName)
				{
					writer.Write(" selected");
				}
				writer.Write(">" + DB.RowField(row,"GroupName") + "</option>");
			}
			writer.Write("</select>&nbsp;&nbsp;&nbsp;");
			ds.Dispose();
			String BeginsWith = Common.IIF(Common.QueryString("BeginsWith").Length == 0 , "A" , Common.QueryString("BeginsWith"));
			String alpha = "%#ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			for(int i = 1; i <= alpha.Length; i++)
			{
				if(BeginsWith.Substring(0,1) == alpha.Substring(i-1,1))
				{
					writer.Write(alpha.Substring(i-1,1) + "&nbsp;");
				}
				else
				{
					writer.Write("<a href=\"appconfig.aspx?GroupName=" + Server.UrlEncode(GroupName) + "&BeginsWith=" + Server.UrlEncode("" + alpha.Substring(i-1,1)) + "\">" + alpha.Substring(i-1,1) + "</a>&nbsp;");
				}
			}
			writer.Write("&nbsp;&nbsp;Search For: <input type=\"text\" name=\"SearchFor\" value=\"" + SearchFor + "\"><input type=\"submit\" name=\"search\" value=\"submit\">");
			writer.Write("</form>");
			
			//SEC4
			string SuperuserFilter = Common.IIF(thisCustomer.IsAdminSuperUser , String.Empty , "(SuperOnly is NULL or SuperOnly=0) and ");

			String sql = String.Empty;
			if(SearchFor.Length != 0)
			{
				sql = "select * from AppConfig  " + DB.GetNoLock() + " where " + SuperuserFilter + " name like " + DB.SQuote("%" + SearchFor + "%") + " order by name";
			}
			else
			{
				sql = "select * from AppConfig  " + DB.GetNoLock() + " where " + SuperuserFilter + " name like " + DB.SQuote(BeginsWith + "%") + " order by name";
			}
			if(GroupName.Length != 0 && GroupName != "0")
			{
				sql = "select * from AppConfig  " + DB.GetNoLock() + " where " + SuperuserFilter + " groupname=" + DB.SQuote(GroupName) + " order by name";
			}
			
			//writer.Write("<p align=left><big><font color=red><b>WARNING: Consult the documentation before modifying these values, as incorrect values can cause your store site and/or administration site to stop working!</b></font></big></p>\n");
			ds = DB.GetDS(sql,false,System.DateTime.Now.AddDays(1));
			writer.Write("<form method=\"POST\" action=\"appconfig.aspx\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr>\n");
			writer.Write("      <td align=\"left\"><input type=\"button\" value=\"Add New AppConfig\" name=\"AddNew\" onClick=\"self.location='editAppConfig.aspx?GroupName=" + Server.UrlEncode(GroupName) + "&beginsWith=" + Server.UrlEncode(BeginsWith) + "&searchfor=" + Server.UrlEncode(SearchFor) + "';\"></td>\n");
			writer.Write("      <td colspan=\"6\"></td>\n");
			writer.Write("    </tr>\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td width=\"10%\"><b>ID</b></td>\n");
			writer.Write("      <td width=\"20%\"><b>Name</b></td>\n");
			writer.Write("      <td width=\"20%\"><b>Description</b></td>\n");
			writer.Write("      <td width=\"30%\"><b>ConfigValue</b></td>\n");
			writer.Write("      <td width=\"10%\" align=\"center\"><b>Edit</b></td>\n");
			writer.Write("      <td width=\"10%\" align=\"center\"><b>Delete</b></td>\n");
			writer.Write("    </tr>\n");
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				bool okToShow = true;
				if(DB.RowField(row,"Name").ToUpper() == "Admin_Superuser".ToUpper() && !thisCustomer.IsAdminSuperUser)
				{
					okToShow = false;
				}
				if(okToShow)
				{
					writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
					writer.Write("      <td valign=\"top\">" + DB.RowFieldInt(row,"AppConfigID").ToString() + "</td>\n");
					writer.Write("      <td valign=\"top\"><a href=\"editAppConfig.aspx?GroupName=" + Server.UrlEncode(GroupName) + "&beginsWith=" + Server.UrlEncode(BeginsWith) + "&searchfor=" + Server.UrlEncode(SearchFor) + "&AppConfigID=" + DB.RowFieldInt(row,"AppConfigID").ToString() + "\">" + DB.RowField(row,"Name") + "</a></td>\n");
					writer.Write("      <td valign=\"top\">" + DB.RowField(row,"Description") + "</td>\n");
					writer.Write("      <td valign=\"top\">" + DB.RowField(row,"ConfigValue") + "</td>\n");
					writer.Write("      <td  valign=\"top\" align=\"center\"><input type=\"button\" value=\"Edit\" name=\"Edit_" + DB.RowFieldInt(row,"AppConfigID").ToString() + "\" onClick=\"self.location='editAppConfig.aspx?GroupName=" + Server.UrlEncode(GroupName) + "&beginsWith=" + Server.UrlEncode(BeginsWith) + "&searchfor=" + Server.UrlEncode(SearchFor) + "&AppConfigID=" + DB.RowFieldInt(row,"AppConfigID").ToString() + "'\"></td>\n");
					writer.Write("      <td  valign=\"top\" align=\"center\"><input type=\"button\" value=\"Delete\" name=\"Delete_" + DB.RowFieldInt(row,"AppConfigID").ToString() + "\" onClick=\"DeleteAppConfig(" + DB.RowFieldInt(row,"AppConfigID").ToString() + ")\"></td>\n");
					writer.Write("    </tr>\n");
				}
			}
			ds.Dispose();
			writer.Write("    <tr>\n");
			writer.Write("      <td align=\"left\"><input type=\"button\" value=\"Add New AppConfig\" name=\"AddNew\" onClick=\"self.location='editAppConfig.aspx?GroupName=" + Server.UrlEncode(GroupName) + "&beginsWith=" + Server.UrlEncode(BeginsWith) + "&searchfor=" + Server.UrlEncode(SearchFor) + "';\"></td>\n");
			writer.Write("      <td colspan=\"6\"></td>\n");
			writer.Write("    </tr>\n");
			writer.Write("  </table>\n");
			writer.Write("</form>\n");

			writer.Write("</center></b>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteAppConfig(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete AppConfig parameter: ' + id))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'AppConfig.aspx?GroupName=" + Server.UrlEncode(GroupName) + "&beginsWith=" + Server.UrlEncode(BeginsWith) + "&searchfor=" + Server.UrlEncode(SearchFor) + "&deleteid=' + id;\n");
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
