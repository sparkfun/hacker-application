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
	/// Summary description for cardinalform.
	/// </summary>
	public class cardinalform : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Order - Credit Card Verification:";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(ShoppingCart.CartIsEmpty(thisCustomer._customerID,CartTypeEnum.ShoppingCart))
			{
				Response.Redirect("ShoppingCart.aspx");
			}

			if(!Common.AppConfigBool("CardinalCommerce.Centinel.IsLive"))
			{
				writer.Write("<p><b>NOTE: USING CARDINAL TEST SERVER</b></p>");
			}
			
			writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\">");
			writer.Write("<tr>");
			writer.Write("<td align=\"left\" width=\"15%\" valign=\"top\">");

			Topic t = new Topic("CardinalExplanation",thisCustomer._localeSetting,_siteID);
			writer.Write(t._contents);
			writer.Write("</td>");
			writer.Write("<td align=\"left\" width=\"85%\" valign=\"top\">");
			writer.Write("<IFRAME SRC=\"cardinalauth.aspx\" WIDTH=\"100%\" HEIGHT=\"500\" scrolling=\"auto\" frameborder=\"0\"></iframe>");
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
