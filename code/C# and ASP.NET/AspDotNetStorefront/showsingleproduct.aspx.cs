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
	/// Summary description for showsingleproduct.
	/// </summary>
	public class showsingleproduct : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");

			int ProductID = Common.QueryStringUSInt("ProductID");
			int CategoryID = Common.QueryStringUSInt("CategoryID");
			String SKUOnly = Common.QueryString("SKU");
			if(ProductID == 0)
			{
				String sql = "select distinct productid,categoryid from productcategory  " + DB.GetNoLock() + " where productid in (select distinct productid from productvariant  " + DB.GetNoLock() + " where upper(skusuffix)=" + DB.SQuote(SKUOnly).ToUpper() + ")";
				IDataReader rs = DB.GetRS(sql);
				if(!rs.Read())
				{
					rs.Close();
					Response.Redirect("default.aspx");
				}
				ProductID = DB.RSFieldInt(rs,"ProductID");
				CategoryID = DB.RSFieldInt(rs,"CategoryID");
				rs.Close();
			}
			Response.Redirect(SE.MakeProductAndCategoryLink(ProductID,CategoryID,""));
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
