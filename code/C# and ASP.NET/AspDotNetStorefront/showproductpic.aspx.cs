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
	/// Summary description for showproductpic.
	/// </summary>
	public class showproductpic : SkinBase
	{
		int ProductID;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			ProductID = Common.QueryStringUSInt("ProductID");
			IDataReader rs = DB.GetRS("select * from Product " + DB.GetNoLock() + " ,ProductCategory  " + DB.GetNoLock() + " where product.productid=productcategory.productid and product.productid=" + ProductID.ToString());
			if(!rs.Read())
			{
				rs.Close();
				Response.Redirect("default.aspx");
			}
			SectionTitle = "VIEWING PRODUCT: <a href=\"javascript:history.back(-1);\">" + rs["Name"].ToString() + "</a>";
			rs.Close();
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"4\" width=\"100%\">");
			writer.Write("<tr>");
			writer.Write("<td align=\"left\" valign=\"top\">");
			writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"4\" width=\"100%\">");
			writer.Write("<tr><td align=\"left\" valign=\"top\">");
			writer.Write("<a href=\"javascript:history.back(-1);\"><img alt=\"Back to product page...\" border=\"0\" src=\"" + Common.LookupImage("Product",ProductID,"large",_siteID) + "\"></a>");
			writer.Write("</td><td align=\"left\" valign=\"top\"><a href=\"javascript:history.back(-1)\">Back...</a></td></tr>");
			writer.Write("</table>");
			writer.Write("</td>");
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
