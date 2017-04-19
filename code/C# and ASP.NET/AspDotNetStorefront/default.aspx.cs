// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Diagnostics;
using AspDotNetStorefrontCommon;
//using AspDotNetStorefrontPatterns;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for _default.
	/// </summary>
	public class _default : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			if(Common.ServerVariables("HTTP_HOST").ToLower().IndexOf(Common.AppConfig("LiveServer").ToLower()) != -1 && Common.ServerVariables("HTTP_HOST").ToLower().IndexOf("www") == -1)
			{
				if(Common.AppConfigBool("RedirectLiveToWWW"))
				{
					Response.Redirect("http://www." + Common.AppConfig("LiveServer").ToLower() + "/default.aspx?" + Common.ServerVariables("QUERY_STRING"));
				}
			}
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SetTemplate(Common.AppConfig("HomeTemplate"));
			if(Common.AppConfigBool("HomeTemplateAsIs"))
			{
				base._disableContents = true;
			}
			SectionTitle = "Welcome to " + Common.AppConfig("StoreName");
			//System.Diagnostics.Trace.WriteLineIf(Config.TraceLevel.TraceVerbose, "Welcome to AspDotNetStorefront");
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(!Common.AppConfigBool("HomeTemplateAsIs"))
			{
				Topic t = new Topic("hometopintro",thisCustomer._localeSetting,_siteID);
				writer.Write(t._contents);
				if(DB.GetSqlN("select count(*) as N from productcategory  " + DB.GetNoLock() + " where categoryid=" + Common.AppConfigUSInt("IsFeaturedCategoryID").ToString() + " and productid in (select distinct productid from product  " + DB.GetNoLock() + " where deleted=0)") > 0)
				{
					if(Common.AppConfigUSInt("NumHomePageSpecials") <= 1)
					{
						writer.Write(Common.GetSpecialsBoxExpandedRandom(Common.AppConfigUSInt("IsFeaturedCategoryID"),true,Common.AppConfig("IsFeaturedCategoryTeaser"),_siteID));
					}
					else
					{
						writer.Write(Common.GetSpecialsBoxExpanded(Common.AppConfigUSInt("IsFeaturedCategoryID"),Common.AppConfigUSInt("NumHomePageSpecials"),false,true,Common.AppConfig("IsFeaturedCategoryTeaser"),_siteID));
					}
				}
				if(!Common.AppConfigBool("DoNotShowNewsOnHomePage") && DB.GetSqlN("select count(*) as N from News  " + DB.GetNoLock() + " where deleted=0") > 0)
				{
					writer.Write(Common.GetNewsBoxExpanded(3,false,Common.AppConfig("NewsTeaser"),_siteID));
				}
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
