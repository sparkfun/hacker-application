// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using AspDotNetStorefrontCommon;


namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for dyop_delete.
	/// </summary>
	public class dyop_delete : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");
			
			Customer thisCustomer = new Customer();

			int PackID = Common.QueryStringUSInt("PackID");
			int ProductID = Common.QueryStringUSInt("ProductID");
			int CategoryID = Common.QueryStringUSInt("CategoryID");
			int SectionID = Common.QueryStringUSInt("SectionID");
			int DeleteID = Common.QueryStringUSInt("DeleteID");

			if(DeleteID != 0)
			{
				DB.ExecuteSQL("delete from CustomCart where ShoppingCartRecID=0 and CustomCartRecID=" + Common.QueryStringUSInt("DeleteID").ToString() + " and CustomerID=" + thisCustomer._customerID.ToString());
			}

			String url = "dyop.aspx?packid=" + PackID.ToString() + "&productid=" + ProductID.ToString() + "&categoryID=" + CategoryID.ToString() + "&sectionid=" + SectionID.ToString();
			Response.Redirect(url);
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
