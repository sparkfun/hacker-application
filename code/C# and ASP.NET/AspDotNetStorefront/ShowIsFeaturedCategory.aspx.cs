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
	/// Summary description for showisfeaturedcategory.
	/// </summary>
	public class showisfeaturedcategory : SkinBase
	{
		int CategoryID;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			CategoryID = Common.AppConfigUSInt("IsFeaturedCategoryID");
			if(CategoryID == 0)
			{
				Response.Redirect("default.aspx");
			}


			String CategoryName = String.Empty;
			String CategoryDescription = String.Empty;
			String CategoryPicture = String.Empty;

			IDataReader rs = DB.GetRS("select * from category  " + DB.GetNoLock() + " where published=1 and deleted=0 and categoryid=" + CategoryID.ToString());
			if(!rs.Read())
			{
				rs.Close();
				Response.Redirect("default.aspx");
			}
			CategoryName = DB.RSField(rs,"Name");
			CategoryDescription = DB.RSField(rs,"Description");
			CategoryPicture = Common.LookupImage("Category",CategoryID,"",_siteID);
			rs.Close();
			
			SectionTitle = "<a class=\"SectionTitleText\" href=\"default.aspx\">Home</a> - " + CategoryName;
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			Common.LogEvent(thisCustomer._customerID,9,CategoryID.ToString());
			writer.Write(Common.GetSpecialsBoxExpanded(0,100,true,true,Common.AppConfig("IsFeaturedCategoryTeaser"),_siteID));
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
