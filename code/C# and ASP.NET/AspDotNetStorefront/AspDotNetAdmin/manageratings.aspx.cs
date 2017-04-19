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
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for manageratings.
	/// </summary>
	public class manageratings : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Manage Ratings";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(Common.QueryString("DeleteID").Length != 0)
			{
				// delete the rating:
				Ratings.DeleteRating(Common.QueryStringUSInt("DeleteID"));
			}

			if(Common.QueryString("ClearFilthyID").Length != 0)
			{
				DB.ExecuteSQL("update rating set IsFilthy=0 where RatingID=" + Common.QueryStringUSInt("ClearFilthyID").ToString());
			}

			writer.Write("<form method=\"GET\" action=\"manageratings.aspx\" name=\"SearchForm2\">\n");
			writer.Write("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\n");
			writer.Write("      <tr align=\"left\">\n");
			writer.Write("        <td width=\"25%\">Search For Comment:</td>\n");
			writer.Write("        <td width=\"75%\">\n");
			writer.Write("          <input type=\"text\" name=\"SearchTerm\" size=\"25\" maxlength=\"70\" value=\"" + Server.HtmlEncode(Common.QueryString("SearchTerm")) + "\">\n");
			writer.Write("          <input type=\"hidden\" name=\"SearchTerm_vldt\" value=\"[req][blankalert=Please enter something to search for!]\">\n");
			writer.Write("          &nbsp;<input type=\"submit\" value=\"Search\" name=\"B1\">&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"manageratings.aspx?filthy=1\">SHOW FILTHY</a></td>\n");
			writer.Write("      </tr>\n");
			writer.Write("    </table>\n");
			writer.Write("</form>\n");

			String st = Common.QueryString("SearchTerm").Trim();

			if(st.Length != 0)
			{
				writer.Write("<p align=\"left\"><b>Product Ratings Matching: " + st + "</b></p>");
			}
			if(Common.QueryString("filthy").Length != 0)
			{
				writer.Write(Ratings.DisplayFilthy(thisCustomer,_siteID));
			}
			else
			{
				writer.Write(Ratings.DisplayMatching(thisCustomer,st,_siteID));
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
