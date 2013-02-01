// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for staff.
	/// </summary>
	public class staff : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Staff";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			Topic t = new Topic("StaffPageHeader",thisCustomer._localeSetting,_siteID);
			writer.Write(t._contents);
			
			writer.Write("<p>&nbsp;</p>\n");

			IDataReader rs = DB.GetRS("select * from staff  " + DB.GetNoLock() + " where deleted=0 and published<>0 order by displayorder,name");
			while(rs.Read())
			{
				writer.Write("<p align=\"left\">\n");
				String ImgUrl = Common.LookupImage("Staff",DB.RSFieldInt(rs,"StaffID"),"Medium",_siteID);
				if(ImgUrl.Length == 0)
				{
					ImgUrl = Common.LookupImage("Staff",DB.RSFieldInt(rs,"StaffID"),"Icon",_siteID);
				}
				if(ImgUrl.Length == 0)
				{
					ImgUrl = Common.AppConfig("NoPicture");
				}
				if(ImgUrl.Length != 0)
				{
					writer.Write("<img hspace=\"10\" src=\"" + ImgUrl + "\" alt=\"" + DB.RSField(rs,"Name") + "\" border=\"0\" align=\"right\">");
				}
				writer.Write("<font class=\"StaffNameText\">");
				if(DB.RSField(rs,"Email").Length != 0)
				{
					writer.Write("<a href=\"mailto:" + DB.RSField(rs,"Email") + "\">");
				}
				writer.Write(DB.RSField(rs,"Name"));
				if(DB.RSField(rs,"Email").Length != 0)
				{
					writer.Write("</a>");
				}
				writer.Write("</font>");
				if(DB.RSField(rs,"Title").Length != 0)
				{
					writer.Write("<br>");
					writer.Write("<font class=\"StaffTitleText\">");
					writer.Write(DB.RSField(rs,"Title"));
					writer.Write("</font>");
				}
				writer.Write("</p>");
				writer.Write("<p align=\"justify\">");
				writer.Write(DB.RSField(rs,"Bio"));
				writer.Write("</p>");
				writer.Write("<br clear=\"all\"><hr size=\"1\" class=\"LightCellText\" width=\"100%\">");
			}
			rs.Close();
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
