// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Web;
using System.IO;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for customerratings
	/// </summary>
	public class customerratings : SkinBase
	{
		
		private Customer targetCustomer;
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			targetCustomer = new Customer(Common.QueryStringUSInt("CustomerID"));
			if(targetCustomer._customerID == 0)
			{
				Response.Redirect("customers.aspx");
			}
			if(Common.QueryString("DeleteID").Length != 0)
			{
				// delete the rating:
				Ratings.DeleteRating(Common.QueryStringUSInt("DeleteID"));
			}
			if(Common.QueryString("ClearFilthyID").Length != 0)
			{
				DB.ExecuteSQL("update rating set IsFilthy=0 where RatingID=" + Common.QueryStringUSInt("ClearFilthyID").ToString());
			}
			SectionTitle = "<a href=\"customers.aspx\">Customers</a> - Product Ratings By: <a href=\"cst_account.aspx?customerid=" + targetCustomer._customerID.ToString() + "\">" + targetCustomer.FullName() + "</a>";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write(Ratings.DisplayForCustomer(targetCustomer._customerID,_siteID));
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
