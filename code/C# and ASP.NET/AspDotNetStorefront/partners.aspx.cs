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
	/// Summary description for partners.
	/// </summary>
	public class partners : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Training & Instruction";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write("<p>&nbsp;</p>\n");

			IDataReader rs = DB.GetRS("select * from partner  " + DB.GetNoLock() + " where deleted=0 and published<>0 order by displayorder,name");
			writer.Write("<p>\n");
			while(rs.Read())
			{
				String ImgUrl = Common.LookupImage("Partner",DB.RSFieldInt(rs,"PartnerID"),"icon",_siteID);
				if(ImgUrl.Length == 0)
				{
					ImgUrl = Common.AppConfig("NoPicture");
				}
				if(ImgUrl.Length != 0)
				{
					writer.Write("<a href=\"partners.aspx?partnerid=" + DB.RSFieldInt(rs,"PartnerID").ToString() + "\"><img src=\"" + ImgUrl + "\" alt=\"" + DB.RSField(rs,"Name") + "\" border=\"0\"></a>&nbsp;\n");
				}
			}
			writer.Write("</p>\n");
			rs.Close();

			int PartnerID = Common.QueryStringUSInt("PartnerID");
			if(PartnerID != 0)
			{
				rs = DB.GetRS("select * from partner  " + DB.GetNoLock() + " where partnerid=" + PartnerID.ToString());
				if(rs.Read())
				{
					writer.Write("  <table border=\"0\" cellpadding=\"2\" cellspacing=\"0\" align=\"center\" width=\"99%\">\n");
					writer.Write("    <tr>\n");
					writer.Write("      <td width=\"100%\" bgcolor=\"#C0C0C0\"><b>");
					if(DB.RSField(rs,"URL").Length != 0 && DB.RSFieldBool(rs,"LinkToSite"))
					{
						writer.Write("<a href=\"" + DB.RSField(rs,"URL") + "\" " + Common.IIF(DB.RSFieldBool(rs,"LinkInNewWindow") , " target=\"_blank\"" , "") + ">");
					}
					writer.Write(DB.RSField(rs,"Name"));
					if(DB.RSField(rs,"URL").Length != 0 && DB.RSFieldBool(rs,"LinkToSite"))
					{
						writer.Write("</a>");
					}
					writer.Write("</b></td>\n");
					writer.Write("    </tr>\n");
					writer.Write("  </table>\n");
					writer.Write("  <table border=\"0\" cellpadding=\"8\" cellspacing=\"0\" align=\"center\" width=\"99%\" style=\"border-style: inset; border-width: 1px;\">\n");
					writer.Write("    <tr>\n");
					writer.Write("      <td width=\"130\" valign=\"top\" align=\"right\">Summary:</td>\n");
					writer.Write("      <td valign=\"top\" align=\"left\">\n");
					writer.Write(DB.RSField(rs,"Summary"));
					writer.Write("      </td>\n");
					writer.Write("    </tr>\n");
					writer.Write("    <tr>\n");
					writer.Write("      <td width=\"130\" valign=\"top\" align=\"right\">Areas of Specialty:</td>\n");
					writer.Write("      <td valign=\"top\" align=\"left\">\n");
					writer.Write(DB.RSField(rs,"Specialty"));
					writer.Write("        </td>\n");
					writer.Write("    </tr>\n");
					writer.Write("    <tr>\n");
					writer.Write("      <td width=\"130\" valign=\"top\" align=\"right\">Instructors:</td>\n");
					writer.Write("      <td valign=\"top\" align=\"left\">\n");
					writer.Write(DB.RSField(rs,"Instructors"));
					writer.Write("       </td>\n");
					writer.Write("    </tr>\n");
					writer.Write("    <tr>\n");
					writer.Write("      <td width=\"130\" valign=\"top\" align=\"right\">Upcoming Schedule:</td>\n");
					writer.Write("      <td valign=\"top\" align=\"left\">\n");
					writer.Write(DB.RSField(rs,"Schedule"));
					writer.Write("        </td>\n");
					writer.Write("    </tr>\n");
					if(DB.RSFieldBool(rs,"LinkToSite"))
					{
						writer.Write("    <tr>\n");
						writer.Write("      <td width=\"130\" valign=\"top\" align=\"right\">Web Site:</td>\n");
						writer.Write("      <td valign=\"top\" align=\"left\">");
						if(DB.RSField(rs,"URL").Length != 0 && DB.RSFieldBool(rs,"LinkToSite"))
						{
							writer.Write("<a href=\"" + DB.RSField(rs,"URL") + "\" " + Common.IIF(DB.RSFieldBool(rs,"LinkInNewWindow") , " target=\"_blank\"" , "") + ">");
						}
						writer.Write(DB.RSField(rs,"URL"));
						if(DB.RSField(rs,"URL").Length != 0 && DB.RSFieldBool(rs,"LinkToSite"))
						{
							writer.Write("</a>");
						}
						writer.Write("</td>\n");
					}
					writer.Write("    </tr>\n");
					writer.Write("    <tr>\n");
					writer.Write("      <td width=\"130\" valign=\"top\" align=\"right\">Customer Feedback:</td>\n");
					writer.Write("      <td valign=\"top\" align=\"left\">\n");
					writer.Write(DB.RSField(rs,"Testimonials"));
					writer.Write("       </td>\n");
					writer.Write("    </tr>\n");
					writer.Write("  </table>\n");
				}
				else
				{
					writer.Write("<p><b>That partner information cannot be found</b></p>\n");
				}
				rs.Close();
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
