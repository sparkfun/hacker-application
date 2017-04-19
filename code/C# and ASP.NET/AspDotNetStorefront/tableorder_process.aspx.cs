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
	/// Summary description for tableorder_process.
	/// </summary>
	public class tableorder_process : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");

			Customer thisCustomer = new Customer();
			thisCustomer.RequireCustomerRecord();

			CartTypeEnum CartType = CartTypeEnum.ShoppingCart;

			int CustomerID = thisCustomer._customerID;
			ShoppingCart cart = new ShoppingCart(1,thisCustomer,CartType,0,false);
			for(int i = 0; i < Request.Form.Count; i++)
			{
				String FieldName = Request.Form.Keys[i];
				String FieldVal = Request.Form[Request.Form.Keys[i]];
				if(FieldName.ToUpper().IndexOf("_VLDT") == -1 && FieldName.ToUpper().StartsWith("QTY_") && FieldVal.Trim().Length != 0)
				{
					try // ignore errors, just add what items we can:
					{
						String[] flds = FieldName.Split('_');
						int ProductID = Localization.ParseUSInt(flds[1]);
						int VariantID = Localization.ParseUSInt(flds[2]);
						int ColorIdx = Localization.ParseUSInt(flds[3]);
						int SizeIdx = Localization.ParseUSInt(flds[4]);
						int Qty = Localization.ParseUSInt(FieldVal);

						IDataReader rs = DB.GetRS("select * from productvariant where VariantID=" + VariantID.ToString());
						rs.Read();
						String ChosenColor = DB.RSField(rs,"Colors").Split(',')[ColorIdx].Trim();
						String ChosenColorSKUModifier = String.Empty;
						if(DB.RSField(rs,"ColorSKUModifiers").Length != 0)
						{
							ChosenColorSKUModifier = DB.RSField(rs,"ColorSKUModifiers").Split(',')[ColorIdx].Trim();
						}
						String ChosenSize = DB.RSField(rs,"Sizes").Split(',')[SizeIdx].Trim();
						String ChosenSizeSKUModifier = String.Empty;
						if(DB.RSField(rs,"SizeSKUModifiers").Length != 0)
						{
							ChosenSizeSKUModifier = DB.RSField(rs,"SizeSKUModifiers").Split(',')[SizeIdx].Trim();
						}
						rs.Close();

						String TextOption = String.Empty;
						cart.AddItem(thisCustomer,ProductID,VariantID,Qty,ChosenColor,ChosenColorSKUModifier,ChosenSize,ChosenSizeSKUModifier,TextOption,CartType,false);


					}
					catch {}
				}
			}
			cart = null;

			String url = Common.IIF(CartType == CartTypeEnum.WishCart, "wishlist.aspx", "ShoppingCart.aspx?add=true");
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
