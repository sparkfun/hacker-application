using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for galleries.
	/// </summary>
	public class galleries : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			Common.LogEvent(thisCustomer._customerID,27,"");
			SectionTitle = "<img src=\"skins/skin_" + _siteID.ToString() + "/images/downarrow.gif\" align=\"absmiddle\" border=\"0\"> Photo Galleries".ToLower();
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			int ItemsPerRow = 3;
			int ItemNumber = 1;

			String sql = "select * from gallery  " + DB.GetNoLock() + " where deleted=0 order by DisplayOrder,Name";
			IDataReader rs = DB.GetRS(sql);			

			writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"4\" width=\"100%\">");
			while(rs.Read())
			{
				if(ItemNumber == 1)
				{
					writer.Write("<tr>");
				}
				if(ItemNumber == ItemsPerRow+1)
				{
					writer.Write("</tr><tr><td colspan=\"" + ItemsPerRow.ToString() + "\" height=\"8\"></td></tr>");
					ItemNumber=1;
				}
				writer.Write("<td width=\"" + (100/ItemsPerRow).ToString() + "%\" align=\"center\" valign=\"top\">");

				String GalIcon = Common.LookupImage("Gallery",DB.RSFieldInt(rs,"GalleryID"),"",_siteID);
				if(GalIcon.Length == 0)
				{
					GalIcon = "images/spacer.gif";
				}
				writer.Write("<a target=\"_blank\" href=\"showgallery.aspx?galleryid=" + DB.RSFieldInt(rs,"GalleryID").ToString() + "\"><img border=\"0\" width=\"175\" src=\"" + GalIcon + "?" + Common.GetRandomNumber(1,1000000).ToString() + "\"></a><br>\n");
				//writer.Write("<a target=\"_blank\" href=\"showgallery.aspx?siteid=" + thisSite._siteGUID + "&galleryid=" + Common.RSField(rs,"GalleryID") + "\">" + Common.RSField(rs,"Name") + "</a>");
				writer.Write(DB.RSField(rs,"Description"));

				writer.Write("</td>");
				ItemNumber++;
				
			} 
			rs.Close();

//			while (rssub.Read());
//			rssub.Close();
			for(int i = ItemNumber; i<=ItemsPerRow; i++)
			{
				writer.Write("<td>&nbsp;</td>");
			}
			writer.Write("</tr>");
			writer.Write("</table>");
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
