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
	/// Summary description for addtocart.
	/// </summary>
	public class addtocart : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");
			
			Customer thisCustomer = new Customer();
			thisCustomer.RequireCustomerRecord();

			String ReturnURL = Common.QueryString("ReturnURL");

			int ProductID = Common.QueryStringUSInt("ProductID");
			int VariantID = Common.QueryStringUSInt("VariantID");
			int Quantity = Common.QueryStringUSInt("Quantity");
			if(Quantity == 0)
			{
				Quantity = Common.FormNativeInt("Quantity");
			}
			if(Quantity == 0 && Common.Form("Quantity").Length == 0 && Common.QueryString("Quantity").Length == 0)
			{
				Quantity = 1;
			}

			int CustomerID = thisCustomer._customerID;

			String ChosenColor = String.Empty;
			String ChosenColorSKUModifier = String.Empty;
			String ChosenSize = String.Empty;
			String ChosenSizeSKUModifier = String.Empty;
			String ChosenSize2 = String.Empty;
			String ChosenSizeSKUModifier2 = String.Empty;
			String TextOption = Common.Form("TextOption");

			CartTypeEnum CartType = CartTypeEnum.ShoppingCart;
			if(Common.FormNativeInt("IsWish") == 1 || Common.QueryStringUSInt("IsWish") == 1)
			{
				CartType = CartTypeEnum.WishCart;
			}

			if(Common.Form("Color").Length != 0)
			{
				String[] ColorSel = Common.Form("Color").Split(',');
				try
				{
					ChosenColor = ColorSel[0];
				}
				catch {}
				try
				{
					ChosenColorSKUModifier = ColorSel[1];
				}
				catch {}
			}

			if(Common.Form("Size").Length != 0)
			{
				String[] SizeSel = Common.Form("Size").Split(',');
				try
				{
					ChosenSize = SizeSel[0];
				}
				catch {}
				try
				{
					ChosenSizeSKUModifier = SizeSel[1];
				}
				catch {}
			}

			if(Common.Form("Size2").Length != 0)
			{
				String[] SizeSel2 = Common.Form("Size2").Split(',');
				try
				{
					ChosenSize2 = SizeSel2[0];
				}
				catch {}
				try
				{
					ChosenSizeSKUModifier2 = SizeSel2[1];
				}
				catch {}
			}

			if(ChosenSize2.Length != 0)
			{
				ChosenSize += "," + ChosenSize2;
			}
			if(ChosenSizeSKUModifier2.Length != 0)
			{
				ChosenSizeSKUModifier += "," + ChosenSizeSKUModifier2;
			}

			ShoppingCart cart = new ShoppingCart(1,thisCustomer,CartType,0,false);
			if(Quantity > 0)
			{
				if(Common.IsAKit(ProductID))
				{
					String tmp = DB.GetNewGUID();
					cart.AddItem(thisCustomer,ProductID,VariantID,Quantity,tmp,ChosenColorSKUModifier,ChosenSize,ChosenSizeSKUModifier,TextOption,CartType,false);
					IDataReader rs = DB.GetRS("select ShoppingCartRecID from ShoppingCart where ChosenColor=" + DB.SQuote(tmp) + " and CustomerID=" + CustomerID.ToString());
					rs.Read();
					int NewRecID = DB.RSFieldInt(rs,"ShoppingCartRecID");
					rs.Close();
					DB.ExecuteSQL("update KitCart set ShoppingCartRecID=" + NewRecID.ToString() + " where ProductID=" + ProductID.ToString() + " and VariantID=" + VariantID.ToString() + " and ShoppingCartRecID=0 and CustomerID=" + CustomerID.ToString());
					DB.ExecuteSQL("update ShoppingCart set ProductPrice=" + Localization.CurrencyStringForGateway(Common.GetKitTotalPrice(CustomerID,ProductID,NewRecID)) + ", ChosenColor=" + DB.SQuote("") + " where ShoppingCartRecID=" + NewRecID.ToString());
				}
				else if(Common.IsAPack(ProductID))
				{
					String tmp = Common.GetNewGUID();
					cart.AddItem(thisCustomer,ProductID,VariantID,Quantity,tmp,ChosenColorSKUModifier,ChosenSize,ChosenSizeSKUModifier,TextOption,CartType,false);
					IDataReader rs = DB.GetRS("select ShoppingCartRecID from ShoppingCart where ChosenColor=" + DB.SQuote(tmp) + " and CustomerID=" + CustomerID.ToString());
					rs.Read();
					int NewRecID = DB.RSFieldInt(rs,"ShoppingCartRecID");
					rs.Close();
					DB.ExecuteSQL("update CustomCart set ShoppingCartRecID=" + NewRecID.ToString() + " where PackID=" + ProductID.ToString() + " and ShoppingCartRecID=0 and CustomerID=" + CustomerID.ToString());
					DB.ExecuteSQL("update ShoppingCart set ChosenColor=" + DB.SQuote("") + " where ShoppingCartRecID=" + NewRecID.ToString());
				}
				else
				{
					cart.AddItem(thisCustomer,ProductID,VariantID,Quantity,ChosenColor,ChosenColorSKUModifier,ChosenSize,ChosenSizeSKUModifier,TextOption,CartType,false);
				}
			}

			// handle upsell products:
			String UpsellProducts = Common.Form("UpsellProducts").Trim();
			if(UpsellProducts.Length != 0 && CartType == CartTypeEnum.ShoppingCart)
			{
				foreach(String s in UpsellProducts.Split(','))
				{
					String PID = s.Trim();
					if(PID.Length != 0)
					{
						int UpsellProductID = 0;
						try
						{
							UpsellProductID = Localization.ParseUSInt(PID);
							if(UpsellProductID != 0)
							{
								int UpsellVariantID = Common.GetProductsFirstVariantID(UpsellProductID);
								if(UpsellVariantID != 0)
								{
									cart.AddItem(thisCustomer,UpsellProductID,UpsellVariantID,1,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,CartType,false);
									Decimal PR = Common.GetUpsellProductPrice(ProductID,UpsellProductID,thisCustomer._customerLevelID);
									DB.ExecuteSQL("update shoppingcart set IsUpsell=1, ProductPrice=" + Localization.CurrencyStringForDB(PR) + " where CartType=" + ((int)CartType).ToString() + " and CustomerID=" + thisCustomer._customerID.ToString() + " and ProductID=" + UpsellProductID.ToString() + " and VariantID=" + UpsellVariantID.ToString() + " and ChosenColor=" + DB.SQuote(String.Empty) + " and ChosenSize=" + DB.SQuote(String.Empty) + " and TextOption=" + DB.SQuote(String.Empty));
								}
							}
						}
						catch {}
					}
				}
			}
			cart = null;
			
			if(Common.AppConfig("AddToCartAction").ToUpper() == "STAY" && ReturnURL.Length != 0)
			{
				Response.Redirect(ReturnURL);
			}
			else
			{
				String url = Common.IIF(CartType == CartTypeEnum.WishCart, "wishlist.aspx", "ShoppingCart.aspx?add=true");
				Response.Redirect(url);
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
