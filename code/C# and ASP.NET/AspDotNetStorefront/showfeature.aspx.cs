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
	/// Summary description for showfeature.
	/// </summary>
	public class showfeature : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");

			Customer thisCustomer = new Customer();
			int ProductID = Common.QueryStringUSInt("ProductID");

			String sql = "select * from ProductCategory  " + DB.GetNoLock() + " where productid=" + ProductID.ToString();
			IDataReader rs = DB.GetRS(sql);
			if(rs.Read())
			{
				int CategoryID = DB.RSFieldInt(rs,"CategoryID");
				Common.LogEvent(thisCustomer._customerID,14, ProductID.ToString());
				rs.Close();
				Response.Redirect(SE.MakeProductAndCategoryLink(ProductID,CategoryID,""));
			}
			else
			{
				rs.Close();
				Response.Redirect("default.aspx");
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
