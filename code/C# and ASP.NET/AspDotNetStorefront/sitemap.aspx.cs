using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Drawing;
using System.Web;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for sitemap.
	/// </summary>
	public class sitemap : SkinBase
	{

		private String GetCategoryProductList(int CategoryID)
		{
			StringBuilder tmpS = new StringBuilder(1000);
			String sql = "select * from product  " + DB.GetNoLock() + " where deleted=0 and published<>0 and ProductID in (select productid from productcategory  " + DB.GetNoLock() + " where categoryid=" + CategoryID.ToString() + ")";
			IDataReader rs = DB.GetRS(sql);
			tmpS.Append("<ul>");
			while(rs.Read())
			{
				tmpS.Append("<li><a href=\"" + SE.MakeProductLink(DB.RSFieldInt(rs,"ProductID"),DB.RSField(rs,"SEName")) + "\">" + Server.HtmlEncode(DB.RSField(rs,"Name")) + "</a></li>");
			}
			tmpS.Append("</ul>");
			rs.Close();
			return tmpS.ToString();
		}

		private String GetSectionProductList(int SectionID)
		{
			StringBuilder tmpS = new StringBuilder(1000);
			String sql = "select * from product  " + DB.GetNoLock() + " where deleted=0 and published<>0 and ProductID in (select productid from productSection  " + DB.GetNoLock() + " where sectionid=" + SectionID.ToString() + ")";
			IDataReader rs = DB.GetRS(sql);
			tmpS.Append("<ul>");
			while(rs.Read())
			{
				tmpS.Append("<li><a href=\"" + SE.MakeProductLink(DB.RSFieldInt(rs,"ProductID"),DB.RSField(rs,"SEName")) + "\">" + Server.HtmlEncode(DB.RSField(rs,"Name")) + "</a></li>");
			}
			tmpS.Append("</ul>");
			rs.Close();
			return tmpS.ToString();
		}

		private String GetManufacturerProductList(int ManufacturerID)
		{
			StringBuilder tmpS = new StringBuilder(1000);
			String sql = "select * from product  " + DB.GetNoLock() + " where deleted=0 and published<>0 and Manufacturerid=" + ManufacturerID.ToString();
			IDataReader rs = DB.GetRS(sql);
			tmpS.Append("<ul>");
			while(rs.Read())
			{
				tmpS.Append("<li><a href=\"" + SE.MakeProductLink(DB.RSFieldInt(rs,"ProductID"),DB.RSField(rs,"SEName")) + "\">" + Server.HtmlEncode(DB.RSField(rs,"Name")) + "</a></li>");
			}
			tmpS.Append("</ul>");
			rs.Close();
			return tmpS.ToString();
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Site Map";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			DataSet ds;
			// Categories:
			writer.Write("<ul>\n");
			ds = DB.GetDS("select * from category  " + DB.GetNoLock() + " where published<>0 and deleted=0",true,System.DateTime.Now.AddHours(1));
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("<li><a href=\"" + SE.MakeCategoryLink(DB.RowFieldInt(row,"CategoryID"),DB.RowField(row,"SEName")) + "\">" + Server.HtmlEncode(DB.RowField(row,"Name")) + "</a></li>\n");
				writer.Write(GetCategoryProductList(DB.RowFieldInt(row,"CategoryID")));
			}
			ds.Dispose();
			writer.Write("</ul>\n");

			// Sections:
			writer.Write("<ul>\n");
			ds = DB.GetDS("select * from [section]  " + DB.GetNoLock() + " where published<>0 and deleted=0",true,System.DateTime.Now.AddHours(1));
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("<li><a href=\"" + SE.MakeSectionLink(DB.RowFieldInt(row,"SectionID"),DB.RowField(row,"SEName")) + "\">" + Server.HtmlEncode(DB.RowField(row,"Name")) + "</a></li>\n");
				writer.Write(GetSectionProductList(DB.RowFieldInt(row,"SectionID")));
			}
			ds.Dispose();
			writer.Write("</ul>\n");

			if(Common.AppConfigBool("Search_ShowManufacturersInResults"))
			{
				// Manufacturers:
				writer.Write("<ul>\n");
				ds = DB.GetDS("select * from manufacturer  " + DB.GetNoLock() + " where deleted=0",true,System.DateTime.Now.AddHours(1));
				foreach(DataRow row in ds.Tables[0].Rows)
				{
					writer.Write("<li><a href=\"" + SE.MakeManufacturerLink(DB.RowFieldInt(row,"ManufacturerID"),DB.RowField(row,"SEName")) + "\">" + Server.HtmlEncode(DB.RowField(row,"Name")) + "</a></li>\n");
					writer.Write(GetManufacturerProductList(DB.RowFieldInt(row,"ManufacturerID")));
				}
				ds.Dispose();
				writer.Write("</ul>\n");
			}
			if(Common.AppConfigBool("ShowTopicsInSiteMap"))
			{
				// Topics:
				writer.Write("<ul>\n");
				writer.Write("<li><a href=\"default.aspx\">Topics</a></li>");
				writer.Write("<ul>\n");
				ds = DB.GetDS("select * from topic  " + DB.GetNoLock() + " where lower(name) not in ('cardinalexplanation','cartpagefooter','checkoutcardauth','downloadfooter','emptycarttext','emptycategorytext','emptymanufacturertext','emptysectiontext','mailfooter','secureattachment.emailbody','worldpaycancel','worldpaysuccess') and deleted=0",true,System.DateTime.Now.AddHours(1));
				foreach(DataRow row in ds.Tables[0].Rows)
				{
					writer.Write("<li><a href=\"" + SE.MakeDriverLink(DB.RowField(row,"Name")) + "\">" + Server.HtmlEncode(DB.RowField(row,"Title")) + "</a></li>\n");
				}
				ds.Dispose();

				// File Topics:
				// create an array to hold the list of files
				ArrayList fArray=new ArrayList();

				// get information about our initial directory
				String SFP = HttpContext.Current.Server.MapPath("skins/skin_" + _siteID.ToString() + "/template.htm").Replace("template.htm","");

				DirectoryInfo dirInfo=new DirectoryInfo(SFP);

				// retrieve array of files & subdirectories
				FileSystemInfo[] myDir=dirInfo.GetFileSystemInfos();
    
				for (int i=0; i<myDir.Length; i++) 
				{
					// check the file attributes

					// if a subdirectory, add it to the sArray    
					// otherwise, add it to the fArray
					if (((Convert.ToUInt32(myDir[i].Attributes) &	Convert.ToUInt32(FileAttributes.Directory) ) > 0 ))
					{
						//sArray.Add(Path.GetFileName(myDir[i].FullName));  
					}
					else
					{
						bool skipit = false;
						if(!myDir[i].FullName.ToLower().EndsWith("htm") || (myDir[i].FullName.ToLower().IndexOf("template") != -1))
						{
							skipit = true;
						}
						if(!skipit)
						{
							fArray.Add(Path.GetFileName(myDir[i].FullName));
						}
					}
				}

				if(fArray.Count != 0)
				{
					// sort the files alphabetically
					fArray.Sort(0,fArray.Count,null);
					for(int i = 0; i < fArray.Count; i++)
					{
						writer.Write("<li><a href=\"" + SE.MakeDriverLink(fArray[i].ToString().Replace(".htm","")) + "\">" + Server.HtmlEncode(Common.Capitalize(fArray[i].ToString().Replace(".htm",""))) + "</a></li>\n");
					}
				}
				writer.Write("</ul>\n");
				writer.Write("</ul>\n");
			}
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
