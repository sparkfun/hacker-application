// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for wishlist.
	/// </summary>
	public class wishlist : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Wish List";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			String BACKURL = Common.IIF(Common.QueryStringUSInt("ResetLinkback") == 0, "javascript:history.back();", Common.GetCartContinueShoppingURL());
			
			if(!thisCustomer._hasCustomerRecord)
			{
				writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
				writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
				writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/wishlist.gif\" border=\"0\"><br>");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
				writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

				writer.Write("<b>Your wish list is currently empty.</b>");
				
				writer.Write("<br><br><a href=\"" + BACKURL + "\">Continue Shopping...</a>");
				
				writer.Write("</td></tr>\n");
				writer.Write("</table>\n");
				writer.Write("</td></tr>\n");
				writer.Write("</table>\n");
			}
			else
			{
				if(Common.QueryStringUSInt("MoveToCartID") != 0)
				{
					DB.ExecuteSQL("update ShoppingCart set CartType=" + ((int)CartTypeEnum.ShoppingCart).ToString() + " where ShoppingCartRecID=" + Common.QueryStringUSInt("MoveToCartID").ToString());
					Response.Redirect("ShoppingCart.aspx");
				}

				if(Common.QueryStringUSInt("DeleteID") != 0)
				{
					//				int ProductID = 0;
					//				IDataReader rsp = DB.GetRS("select * from ShoppingCart  " + DB.GetNoLock() + " where ShoppingCartRecID=" + Common.QueryStringUSInt("MoveToCartID"));
					//				if(rsp.Read())
					//				{
					//					ProductID = DB.RSFieldInt(rsp,"ProductID");
					//				}
					//				rsp.Close();
					//				bool IsAKit = Common.IsAKit(ProductID);
					DB.ExecuteSQL("delete from ShoppingCart where CustomerID=" + thisCustomer._customerID.ToString() + " and ShoppingCartRecID=" + Common.QueryStringUSInt("DeleteID").ToString());
					DB.ExecuteSQL("delete from customcart where CustomerID=" + thisCustomer._customerID.ToString() + " and ShoppingCartRecID=" + Common.QueryStringUSInt("DeleteID").ToString());
					DB.ExecuteSQL("delete from kitcart where CustomerID=" + thisCustomer._customerID.ToString() + " and ShoppingCartRecID=" + Common.QueryStringUSInt("DeleteID").ToString());
				}

				ShoppingCart cart = new ShoppingCart(_siteID,thisCustomer,CartTypeEnum.WishCart,0,false);

				writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
				writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
				writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/wishlist.gif\" border=\"0\"><br>");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
				writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

				int AgeWishListDays = Common.AppConfigUSInt("AgeWishListDays");
				if(AgeWishListDays == 0)
				{
					AgeWishListDays = 30;
				}
				cart.Age(AgeWishListDays,CartTypeEnum.WishCart);
				writer.Write(cart.DisplayWish());

				writer.Write("<br><br><a href=\"" + BACKURL + "\">Continue Shopping...</a>");
				
				writer.Write("</td></tr>\n");
				writer.Write("</table>\n");
				writer.Write("</td></tr>\n");
				writer.Write("</table>\n");
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
