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
	/// Summary description for ShoppingCartPage.
	/// </summary>
	public class ShoppingCartPage : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			this.RequireCustomerRecord();
			RequireSecurePage();
			SectionTitle = Common.AppConfig("CartPrompt");
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(!thisCustomer._hasCustomerRecord)
			{
				writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
				writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
				writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/orderinfo.gif\" border=\"0\"><br>");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
				writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

				writer.Write("<b>Your shopping cart is currently empty.</b>");
				writer.Write("<br><br>");
				String BACKURL = Common.IIF(Common.QueryStringUSInt("ResetLinkback") == 0, "javascript:history.back();", Common.GetCartContinueShoppingURL());
				writer.Write("<a href=\"" + BACKURL + "\">Continue Shopping...</a>");

				writer.Write("</td></tr>\n");
				writer.Write("</table>\n");
				writer.Write("</td></tr>\n");
				writer.Write("</table>\n");
			}
			else
			{
				int AgeCartDays = Common.AppConfigUSInt("AgeCartDays");
				if(AgeCartDays == 0)
				{
					AgeCartDays = 7;
				}
				ShoppingCart.Age(thisCustomer._customerID,AgeCartDays,CartTypeEnum.ShoppingCart);

				ShoppingCart cart = new ShoppingCart(_siteID,thisCustomer,CartTypeEnum.ShoppingCart,0,false);

				if(Common.QueryString("discountvalid").ToUpper() == "FALSE")
				{
					writer.Write("<p align=\"left\"><b><font color=\"red\">" + Server.HtmlEncode(Common.QueryString("invalidreason")) + ".</font></b></p>");
				}

				if(Common.QueryString("ErrorMsg").Length != 0)
				{
					writer.Write("<p align=\"left\"><b><font color=\"red\">" + Server.HtmlEncode(Common.QueryString("ErrorMsg")) + "</font></b></p>");
				}

				if(cart.InventoryTrimmed)
				{
					writer.Write("<p align=\"left\"><b><font color=\"red\">Some of your item quantities were reduced, as they exceeded stock on hand.</font></b></p>");
				}

				if(cart.MinimumQuantitiesUpdated)
				{
					writer.Write("<p align=\"left\"><b><font color=\"red\">Some of your item quantities were below the minimum allowed orderable quantity, so they were increased.</font></b></p>");
				}

				Decimal MinOrderAmount = Common.AppConfigUSDecimal("CartMinOrderAmount");
				if(!cart.IsEmpty() && MinOrderAmount != System.Decimal.Zero && cart.SubTotal(false,false,true) < MinOrderAmount)
				{
					writer.Write("<p align=\"left\"><b><font color=\"red\">The minimum allowed order amount is " + Localization.CurrencyStringForDisplay(MinOrderAmount) + " Please add additional items to your cart, or increase item quantities.</font></b></p>");
				}

				Topic t = new Topic("CartPageHeader",thisCustomer._localeSetting,_siteID);
				writer.Write(t._contents);
				
				writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
				writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
				writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/ShoppingCart.gif\" border=\"0\"><br>");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
				writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

				Response.Write(cart.Display(false,_siteID,thisCustomer._isAnon));

				writer.Write("</td></tr>\n");
				writer.Write("</table>\n");
				writer.Write("</td></tr>\n");
				writer.Write("</table>\n");

				Topic t2 = new Topic("CartPageFooter",thisCustomer._localeSetting,_siteID);
				writer.Write(t2._contents);
				
				if(Common.AppConfigBool("RTShipping.DumpXMLOnCartPage") && Common.GetActiveShippingCalculationID() == Common.ShippingCalculationEnum.UseRealTimeRates)
				{
					writer.Write("<hr break=\"all\">");
					IDataReader rs = DB.GetRS("Select RTShipRequest,RTShipResponse from customer where CustomerID=" + thisCustomer._customerID.ToString());
					if(rs.Read())
					{
						String s = DB.RSField(rs,"RTShipRequest");
						s = s.Replace("<?xml version=\"1.0\"?>","");
						try
						{
							s = Common.PrettyPrintXml("<roottag_justaddedfordisplayonthispage>" + s + "</roottag_justaddedfordisplayonthispage>"); // the RTShipRequest may have "two" XML docs in it :)
						}
						catch
						{
							s = DB.RSField(rs,"RTShipRequest");
						}
						writer.Write("<b>RTShipRequest:</b><br><textarea rows=40 cols=80>" + s + "</textarea><br><br>");
						try
						{
							s = Common.PrettyPrintXml(DB.RSField(rs,"RTShipResponse"));
						}
						catch
						{
							s = DB.RSField(rs,"RTShipResponse");
						}
						writer.Write("<b>RTShipResponse:</b><br><textarea rows=40 cols=80>" + s + "</textarea><br><br>");
					}
					rs.Close();
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
