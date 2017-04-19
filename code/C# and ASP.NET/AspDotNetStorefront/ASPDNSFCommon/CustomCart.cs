// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Collections;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Text;

namespace AspDotNetStorefrontCommon
{
	/// <summary>
	/// Summary description for CustomCart.
	/// </summary>
	/// 
	public struct CustomItem
	{
		public int CustomCartRecordID;
		public int productID;
		public int variantID;
		public String productName;
		public String SKU;
		public int quantity;
		public String chosenColor;
		public String chosenColorSKUModifier;
		public String chosenSize;
		public String chosenSizeSKUModifier;
	}

	public class CustomCart
	{
		public int _packID;
		public int _customerID;
		public int _siteID;
		public int _ShoppingCartRecID;
		private bool _isEmpty;
		public ArrayList _cartItems;

		public CustomCart(int CustomerID, int PackID, int SiteID)
		{
			_customerID = CustomerID;
			_packID = PackID;
			_siteID = SiteID;
			_ShoppingCartRecID = 0;
			LoadFromDB();
		}

		public CustomCart(int CustomerID, int ShoppingCartRecID, int PackID, int SiteID)
		{
			_customerID = CustomerID;
			_packID = PackID;
			_siteID = SiteID;
			_ShoppingCartRecID = ShoppingCartRecID;
			LoadFromDB();
		}

		public void Age(int NumDays)
		{
			if(!_isEmpty)
			{
				DB.ExecuteSQL("delete from CustomCart where CustomerID=" + _customerID.ToString() + " and CreatedOn<" + DB.SQuote(Localization.ToNativeShortDateString(System.DateTime.Now.AddDays(-NumDays))));
			}
		}

		static public bool IsEmpty(int CustomerID, int PackID)
		{
			return (NumItems(CustomerID, PackID)==0);
		}

		static public int NumItems(int CustomerID, int PackID)
		{
			IDataReader rs = DB.GetRS("select sum(quantity) as NumItems from CustomCart  " + DB.GetNoLock() + " where ShoppingCartRecID=0 and CustomerID=" + CustomerID.ToString() + " and PackID=" + PackID.ToString());
			rs.Read();
			int N = DB.RSFieldInt(rs,"NumItems");
			rs.Close();
			return N;
		}

		private void LoadFromDB()
		{
			_isEmpty = true;

			IDataReader rs = DB.GetRS("select * from customcart  " + DB.GetNoLock() + " WHERE ShoppingCartRecID=" + _ShoppingCartRecID.ToString() + " and CustomCart.CustomerID = " + _customerID.ToString() + " and PackID=" + _packID.ToString() + " order by ProductName");
			_cartItems = new ArrayList(50);
			int i = 0;
			while(rs.Read())
			{
				_isEmpty = false;
				CustomItem newItem;
				newItem.CustomCartRecordID = DB.RSFieldInt(rs,"CustomCartRecID");
				newItem.productID = DB.RSFieldInt(rs,"ProductID");
				newItem.variantID = DB.RSFieldInt(rs,"VariantID");
				newItem.productName = DB.RSField(rs,"ProductName");
				newItem.SKU = DB.RSField(rs,"ProductSKU");
				newItem.quantity = DB.RSFieldInt(rs,"Quantity");
				newItem.chosenColor = DB.RSField(rs,"ChosenColor");
				newItem.chosenColorSKUModifier = DB.RSField(rs,"ChosenColorSKUModifier");
				newItem.chosenSize = DB.RSField(rs,"ChosenSize");
				newItem.chosenSizeSKUModifier = DB.RSField(rs,"ChosenSizeSKUModifier");
				_cartItems.Add(newItem);
				i = i + 1;
			}
			_cartItems.Capacity = i;
			rs.Close();
		}
		
		public String GetContents(bool inTable, String Separator)
		{
			StringBuilder tmpS = new StringBuilder(10000);
			if(inTable)
			{
				tmpS.Append("        <table cellSpacing=\"1\" width=\"100%\" cellpadding=\"0\" border=\"0\">\n");
				tmpS.Append("            <tr>\n");
				tmpS.Append("              <td width=\"60%\" height=\"18\" bgColor=\"#AAAAAA\"><font class=\"dyop_hdr\">&nbsp;Style</font></b></td>\n");
				tmpS.Append("              <td width=\"10%\" height=\"18\" bgColor=\"#AAAAAA\" align=\"center\"><font class=\"dyop_hdr\">Color</font></b></td>\n");
				tmpS.Append("              <td width=\"10%\" height=\"18\" bgColor=\"#AAAAAA\" align=\"center\"><font class=\"dyop_hdr\">Size</font></b></td>\n");
				tmpS.Append("              <td width=\"10%\" height=\"18\" bgColor=\"#AAAAAA\" align=\"center\"><font class=\"dyop_hdr\">Quantity</font></b></td>\n");
				tmpS.Append("            </tr>\n");
				for(int i = 0; i < _cartItems.Count; i++)
				{
					if(i > 0)
					{
						tmpS.Append("            <tr>\n");
						tmpS.Append("              <td colSpan=\"4\" height=\"5\" valign=\"middle\"><img src=\"skins/skin_" + _siteID.ToString() + "/images/spacer_pink.gif\" height=\"1\" width=\"100%\"></td>\n");
						tmpS.Append("            </tr>\n");
						tmpS.Append("            <tr>\n");
					}
					tmpS.Append("              <td valign=\"middle\" align=\"left\"><font class=\"dyop_sm\">&nbsp;" + ((CustomItem)_cartItems[i]).productName + "</font></td>\n");
					tmpS.Append("              <td valign=\"middle\" align=\"center\"><font class=\"dyop_sm\">" + ((CustomItem)_cartItems[i]).chosenColor + "</font></td>\n");
					tmpS.Append("              <td valign=\"middle\" align=\"center\"><font class=\"dyop_sm\">" + ((CustomItem)_cartItems[i]).chosenSize + "</font></td>\n");
					tmpS.Append("              <td valign=\"middle\" align=\"center\"><font class=\"dyop_sm\">" + ((CustomItem)_cartItems[i]).quantity.ToString() + "</font></td>\n");
					tmpS.Append("            </tr>\n");
				}
				tmpS.Append("</table>");
			}
			else
			{
				for(int i = 0; i < _cartItems.Count; i++)
				{
					if(i > 0)
					{
						tmpS.Append(Separator);
					}
					tmpS.Append("(" + ((CustomItem)_cartItems[i]).quantity.ToString() + ") ");
					tmpS.Append(((CustomItem)_cartItems[i]).productName + ", ");
					tmpS.Append(((CustomItem)_cartItems[i]).SKU );
					if(((CustomItem)_cartItems[i]).chosenColor.Length != 0)
					{
						tmpS.Append(", " + ((CustomItem)_cartItems[i]).chosenColor);
					}
					if(((CustomItem)_cartItems[i]).chosenSize.Length != 0)
					{
						tmpS.Append(", " + ((CustomItem)_cartItems[i]).chosenSize);
					}
				}
			}
			return tmpS.ToString();
		}

		public void AddItem(int ProductID, int VariantID, int Quantity, String ChosenColor, String ChosenColorSKUModifier, String ChosenSize, String ChosenSizeSKUModifier)
		{
			for(int i = 0; i<_cartItems.Count; i++)
			{
				if (((CustomItem)_cartItems[i]).productID == ProductID && ((CustomItem)_cartItems[i]).variantID == VariantID && ((CustomItem)_cartItems[i]).chosenColor == ChosenColor && ((CustomItem)_cartItems[i]).chosenColorSKUModifier == ChosenColorSKUModifier && ((CustomItem)_cartItems[i]).chosenSize == ChosenSize && ((CustomItem)_cartItems[i]).chosenSizeSKUModifier == ChosenSizeSKUModifier)
				{
					String sql3 = "update CustomCart set Quantity = Quantity + " + Quantity.ToString() + " where ShoppingCartRecID=" + _ShoppingCartRecID.ToString() + " and ProductID=" + ProductID.ToString() + " and VariantID=" + VariantID.ToString() + " and ChosenColor=" + DB.SQuote(ChosenColor) + " and ChosenSize=" + DB.SQuote(ChosenSize) + " and CustomerID=" + _customerID.ToString();
					DB.ExecuteSQL(sql3);
					_cartItems.Clear();
					LoadFromDB();
					return;
				}
			}
			String sku = String.Empty;
			String description = String.Empty;
			String price = String.Empty;
			String weight = String.Empty;
			String sql = "Select m.name as ManufacturerName,p.SKU, v.SKUSuffix, p.Name, v.Name as VariantName, v.Description, v.Price, v.SalePrice, v.Weight from product p " + DB.GetNoLock() + " ,productvariant v " + DB.GetNoLock() + " , manufacturer m  " + DB.GetNoLock() + " where p.manufacturerid=m.manufacturerid and p.productid=v.productid and v.variantid=" + VariantID.ToString() + " and p.productid=" + ProductID.ToString();
			IDataReader rs = DB.GetRS(sql);
			if(rs.Read())
			{
				sku = Common.MakeProperProductSKU(DB.RSField(rs,"SKU"),DB.RSField(rs,"SKUSuffix"),ChosenColorSKUModifier,ChosenSizeSKUModifier);
				description = DB.RSField(rs,"Name");
				if(DB.RSField(rs,"VariantName").Trim().Length != 0)
				{
					if(DB.RSField(rs,"VariantName") != DB.RSField(rs,"Name"))
					{
						description += " - " + DB.RSField(rs,"VariantName");
					}
					else
					{
						description += " - " + Common.Ellipses(DB.RSField(rs,"Description"),30,true);
					}
				}
				else if(DB.RSField(rs,"Description").Trim().Length != 0)
				{
					description += " - " + Common.Ellipses(DB.RSField(rs,"Description").Trim(),30,true);
				}
			}
			rs.Close();
			if(sku.Length != 0 || Common.AppConfigBool("AllowEmptySkuAddToCart"))
			{
				StringBuilder sql2 = new StringBuilder("insert into CustomCart(CustomerID,PackID,ProductID,VariantID,ProductSKU,ProductName,Quantity,ChosenColor,ChosenColorSKUModifier,ChosenSize,ChosenSizeSKUModifier) values(",10000);
				sql2.Append(_customerID.ToString() + ",");
				sql2.Append(_packID.ToString() + ",");
				sql2.Append(ProductID.ToString() + ",");
				sql2.Append(VariantID.ToString() + ",");
				sql2.Append(DB.SQuote(sku) + ",");
				sql2.Append(DB.SQuote(description) + ",");
				sql2.Append(Quantity.ToString() + ",");
				sql2.Append(DB.SQuote(ChosenColor) + ",");
				sql2.Append(DB.SQuote(ChosenColorSKUModifier) + ",");
				sql2.Append(DB.SQuote(ChosenSize) + ",");
				sql2.Append(DB.SQuote(ChosenSizeSKUModifier));
				sql2.Append(")");
				DB.ExecuteSQL(sql2.ToString());
				_cartItems.Clear();
				LoadFromDB();
			}
		}


		public bool IsEmpty()
		{
			return _isEmpty;
		}

		public void ClearContents()
		{
			DB.ExecuteSQL("delete from customcart where ShoppingCartRecID=" + _ShoppingCartRecID.ToString() + " and customerid=" + _customerID.ToString() + " and packid=" + _packID.ToString());
			_isEmpty = true;
			LoadFromDB();
		}

	}
}
