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
	/// Summary description for ShoppingCart.
	/// </summary>
	/// 
	public enum CartTypeEnum
	{
		ShoppingCart = 0,
		WishCart = 1,
		RecurringCart = 2
	}

	public enum RecurringIntervalTypeEnum
	{
		NotUsed = 0,
		Daily = 1,
		Weekly = 2,
		Monthly = 3,
		Yearly = 4
	}
	
	public struct CartItem
	{
		public DateTime _createdOn;
		public DateTime _nextRecurringShipDate;
		public int _recurringIndex;
		public int _originalRecurringOrderNumber;
		public int ShoppingCartRecordID;
		public CartTypeEnum cartType;
		public int productID;
		public int variantID;
		public String productName;
		public String SKU;
		public String restrictedQuantities;
		public int minimumQuantity;
		public int quantity;
		public int QuantityDiscountID;
		public String QuantityDiscountName;
		public Single QuantityDiscountPercent;
		public String chosenColor;
		public String chosenColorSKUModifier;
		public String chosenSize;
		public String chosenSizeSKUModifier;
		public String textOption;
		public Single weight;
		public String dimensions;
		public int subscriptionMonths;
		public Decimal shippingCost;
		public Decimal price; // of one item! multiply by quantity to get this item subtotal
		public bool isTaxable;
		public bool isShipSeparately;
		public bool isDownload;
		public String downloadLocation;
		public bool isSecureAttachment;
		public bool isRecurring;
		public int recurringInterval;
		public RecurringIntervalTypeEnum recurringIntervalType;
		public int shippingAddressID;
		public int billingAddressID;
		public bool isUpsell;
	}

	public struct Coupon
	{
		public String code;
		public String description;
		public DateTime expirationDate;
		public Decimal discountAmount;
		public Single discountPercent;
		public bool includesFreeShipping;
		public bool ExpiresOnFirstUseByAnyCustomer;
		public bool ExpiresAfterOneUsageByEachCustomer;
		public int ExpiresAfterNUses;
		public Decimal RequiresMinimumOrderAmount;
		public String ValidForCustomers;
		public String ValidForProducts;
		public String ValidForCategories;
		public String ValidForSections;
		public String ValidForManufacturers;
		public int NumUses;
	}

	public struct AddressInfo
	{
		public String firstName;
		public String lastName;
		public String company;
		public String address1;
		public String address2;
		public String suite;
		public String city;
		public String state;
		public String zip;
		public String country;
		public String phone;
		public String email;
	}

	public class ShoppingCart
	{
		public int _siteID;
		public CartTypeEnum _cartType;
		public Customer _thisCustomer;
		private bool _isEmpty;
		public Coupon _coupon;
		public String _email;
		public ArrayList _cartItems;
		public Address _shippingAddress;
		public Address _billingAddress;
		
		public String _cardName;
		public String _cardType;
		public String _cardNumber;
		public String _cardExpirationMonth;
		public String _cardExpirationYear;

		public Single _stateTaxRate;
		public int _shippingMethodID;
		public int _shippingZoneID;
		public String _orderNotes;
		public String _shippingMethod;
		public String _orderOptions;
		public bool InventoryTrimmed;
		public bool MinimumQuantitiesUpdated;
		public bool _onlyLoadRecurringItemsThatAreDue;
		public int _originalRecurringOrderNumber = 0;

		public ShoppingCart(int SiteID, Customer thisCustomer, CartTypeEnum CartType, int OriginalRecurringOrderNumber, bool OnlyLoadRecurringItemsThatAreDue)
		{
			_siteID = SiteID;
			_thisCustomer = thisCustomer;
			_cartType = CartType;
			this._shippingAddress = new Address(thisCustomer._customerID,AddressTypes.Shipping);
			this._billingAddress = new Address(thisCustomer._customerID,AddressTypes.Billing);
			_originalRecurringOrderNumber = OriginalRecurringOrderNumber;
			_onlyLoadRecurringItemsThatAreDue = OnlyLoadRecurringItemsThatAreDue;
			LoadFromDB(CartType);

			String ProductRequires = String.Empty;
			//V4_0 Make sure Required items are added to the cart if needed
			foreach(CartItem item in this._cartItems)
			{
				//Collect all the requires in the cart as one string.
				string tmpS = Common.GetRequiresProducts(item.productID);
        
				ProductRequires += Common.IIF(((ProductRequires.Length != 0) && (tmpS.Length != 0)),",","") + tmpS;
				string sql = String.Format("select KitCart.ProductID from KitCart  " + DB.GetNoLock() + " where ShoppingCartRecID={0} and CartType={1}",item.ShoppingCartRecordID,(int)CartType);
				IDataReader rsx = DB.GetRS(String.Format("select KitCart.ProductID from KitCart  " + DB.GetNoLock() + " where ShoppingCartRecID={0} and CartType={1}",item.ShoppingCartRecordID,(int)CartType));
				while(rsx.Read())
				{
					tmpS = Common.GetRequiresProducts(DB.RSFieldInt(rsx,"ProductID"));
					ProductRequires += Common.IIF(((ProductRequires.Length != 0) && (tmpS.Length != 0)),",","") + tmpS;
				}
				rsx.Close();
				rsx = DB.GetRS(String.Format("select CustomCart.ProductID from CustomCart  " + DB.GetNoLock() + " where ShoppingCartRecID={0} and CartType={1}",item.ShoppingCartRecordID,(int)CartType));
				while(rsx.Read())
				{
					tmpS = Common.GetRequiresProducts(DB.RSFieldInt(rsx,"ProductID"));
					ProductRequires += Common.IIF(((ProductRequires.Length != 0) && (tmpS.Length != 0)),",","") + tmpS;
				}
				rsx.Close();
			}
			if(ProductRequires.Length != 0)
			{
				String[] ssplit = ProductRequires.Split(',');
				foreach(String s in ssplit)
				{
					try
					{
						int PID = Localization.ParseUSInt(s);
						int VID = Common.GetProductsFirstVariantID(PID);
						if(!this.Contains(PID) && !Common.IsAKit(PID) && !Common.IsAPack(PID))
						{
							this.AddItem(thisCustomer,PID,VID,1,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,CartType,false);
						}
					}
					catch {}
				}
				LoadFromDB(CartType);
			}
		}

		public void GetProductInfo(int ShoppingCartRecID, out int ProductID, out int VariantID, out String ChosenColor, out String ChosenSize, out String TextOption)
		{
			ProductID = 0;
			VariantID = 0;
			ChosenColor = String.Empty;
			ChosenSize = String.Empty;
			TextOption = String.Empty;
			foreach(CartItem c in _cartItems)
			{
				if(c.ShoppingCartRecordID == ShoppingCartRecID)
				{
					ProductID = c.productID;
					VariantID = c.variantID;
					ChosenColor = c.chosenColor;
					ChosenSize = c.chosenSize;
					TextOption = c.textOption;
				}
			}
		}
		
		public void Age(int NumDays, CartTypeEnum CartType)
		{
			if(NumDays > 0)
			{
				ShoppingCart.Age(_thisCustomer._customerID,NumDays,CartType);
				LoadFromDB(CartType);
			}
		}

		public static void Age(int CustomerID, int NumDays, CartTypeEnum CartType)
		{
			if(NumDays > 0)
			{
				// remember, you can't "age" the recurring cart:
				if(CustomerID != 0 && CartType != CartTypeEnum.RecurringCart)
				{
					String AgeDate = Localization.ToNativeShortDateString(System.DateTime.Now.AddDays(-NumDays));
					DB.ExecuteSQL("delete from customcart where CartType=" + ((int)CartTypeEnum.RecurringCart).ToString() + " and CustomerID=" + CustomerID.ToString() + " and CreatedOn<" + DB.DateQuote(AgeDate));
					DB.ExecuteSQL("delete from kitcart where CartType=" + ((int)CartTypeEnum.RecurringCart).ToString() + " and CustomerID=" + CustomerID.ToString() + " and CreatedOn<" + DB.DateQuote(AgeDate));
					DB.ExecuteSQL("delete from ShoppingCart where CartType=" + ((int)CartTypeEnum.RecurringCart).ToString() + " and CustomerID=" + CustomerID.ToString() + " and CreatedOn<" + DB.DateQuote(AgeDate));
				
				}
				// now clear out any "deleted" products:
				// question, we clean out "recurring" items that are deleted? I think so...

				DB.ExecuteSQL("delete from customcart where productid not in (select productid from product  " + DB.GetNoLock() + " where deleted=0 and published=1) and CustomerID=" + CustomerID.ToString());
				DB.ExecuteSQL("delete from kitcart where productid not in (select productid from product  " + DB.GetNoLock() + " where deleted=0 and published=1) and CustomerID=" + CustomerID.ToString());
				DB.ExecuteSQL("delete from ShoppingCart where productid not in (select productid from product  " + DB.GetNoLock() + " where deleted=0 and published=1) and CustomerID=" + CustomerID.ToString());
			}
		}

		public bool IsEmpty(int CustomerID, CartTypeEnum CartType)
		{
			return (NumItems(CustomerID, CartType)==0);
		}

		public static bool CartIsEmpty(int CustomerID, CartTypeEnum CartType)
		{
			return DB.GetSqlN("select count(*) as N from ShoppingCart  " + DB.GetNoLock() + " where CartType=" + ((int)CartType).ToString() + " and customerid=" + CustomerID.ToString()) == 0;
		}

		static public int NumItems(int CustomerID, CartTypeEnum CartType)
		{
			int N = 0;
			if(CustomerID != 0)
			{
				IDataReader rs = DB.GetRS("select sum(quantity) as NumItems from ShoppingCart  " + DB.GetNoLock() + " where CartType=" + ((int)CartType).ToString() + " and CustomerID=" + CustomerID.ToString());
				rs.Read();
				N = DB.RSFieldInt(rs,"NumItems");
				rs.Close();
			}
			return N;
		}

		static public bool CheckInventory(int CustomerID)
		{
			bool ITrimmed = false;
			if(Common.AppConfigBool("Inventory.LimitCartToQuantityOnHand"))
			{
				IDataReader rsi = DB.GetRS("select * from ShoppingCart  " + DB.GetNoLock() + " where CartType=" + ((int)CartTypeEnum.ShoppingCart).ToString() + " and customerid=" + CustomerID.ToString());
				while(rsi.Read())
				{
					int ionhand = Common.GetInventory(DB.RSFieldInt(rsi,"ProductID"),DB.RSFieldInt(rsi,"VariantID"),DB.RSField(rsi,"ChosenSize"),DB.RSField(rsi,"ChosenColor"));
					if(DB.RSFieldInt(rsi,"Quantity") > ionhand)
					{
						ITrimmed = true;
						if(ionhand <= 0)
						{
							DB.ExecuteSQL("delete from ShoppingCart where ShoppingCartRecID=" + DB.RSFieldInt(rsi,"ShoppingCartRecID").ToString());
						}
						else
						{
							DB.ExecuteSQL("update ShoppingCart set Quantity=" + ionhand.ToString() + " where ShoppingCartRecID=" + DB.RSFieldInt(rsi,"ShoppingCartRecID").ToString());
						}
					}
				}
				rsi.Close();
			}
			return ITrimmed;
		}

		static public bool CheckMinimumQuantities(int CustomerID)
		{
			bool QFixed = false;
			IDataReader rsi = DB.GetRS("select ShoppingCart.*, ProductVariant.MinimumQuantity from ShoppingCart  " + DB.GetNoLock() + " left outer join productvariant " + DB.GetNoLock() + " on shoppingcart.variantid=productvariant.variantid where CartType=" + ((int)CartTypeEnum.ShoppingCart).ToString() + " and customerid=" + CustomerID.ToString());
			while(rsi.Read())
			{
				int MinQ = DB.RSFieldInt(rsi,"MinimumQuantity");
				if(MinQ > 0)
				{
					if(DB.RSFieldInt(rsi,"Quantity") < MinQ)
					{
						QFixed = true;
						DB.ExecuteSQL("update ShoppingCart set Quantity=" + MinQ.ToString() + " where ShoppingCartRecID=" + DB.RSFieldInt(rsi,"ShoppingCartRecID").ToString());
					}
				}
			}
			rsi.Close();
			return QFixed;
		}

		private void LoadFromDB(CartTypeEnum CartType)
		{
			_isEmpty = true;
			_orderOptions = String.Empty;
			InventoryTrimmed = false;
			MinimumQuantitiesUpdated = false;
			if(CartType == CartTypeEnum.ShoppingCart) // only work on the real shopping cart
			{
				InventoryTrimmed = CheckInventory(_thisCustomer._customerID);
				MinimumQuantitiesUpdated = CheckMinimumQuantities(_thisCustomer._customerID);
			}

			// Need to pull the Shipping Address from Address table
			String sql = "SELECT ShoppingCart.ProductSKU, ShoppingCart.IsUpsell, ShoppingCart.NextRecurringShipDate, ShoppingCart.RecurringIndex, ShoppingCart.OriginalRecurringOrderNumber, ShoppingCart.CartType, ShoppingCart.ProductPrice, ShoppingCart.ShippingCost, ShoppingCart.ProductWeight, ShoppingCart.ProductDimensions, ShoppingCart.SubscriptionMonths, ShoppingCart.ShoppingCartRecID, ShoppingCart.ProductID, ShoppingCart.VariantID, ShoppingCart.Quantity, ShoppingCart.IsTaxable, ShoppingCart.IsShipSeparately, ShoppingCart.ChosenColor,ShoppingCart.ChosenColorSKUModifier,ShoppingCart.ChosenSize,ShoppingCart.ChosenSizeSKUModifier, ShoppingCart.TextOption, ShoppingCart.IsDownload, ShoppingCart.DownloadLocation, ShoppingCart.IsSecureAttachment,ShoppingCart.CreatedOn, "
				+ " ShoppingCart.BillingAddressID as ShoppingCartBillingAddressID, ShoppingCart.ShippingAddressID as ShoppingCartShippingAddressID, "
				+ " Customer.EMail, Customer.OrderOptions, Customer.OrderNotes, Customer.CouponCode, "
				+ " Customer.ShippingAddressID as CustomerShippingAddressID, "
				+ " Customer.BillingAddressID as CustomerBillingAddressID, "
				+ " product.name as ProductName, productvariant.name as VariantName, "
				+ Common.IIF(CartType == CartTypeEnum.RecurringCart, " ShoppingCart.RecurringInterval, ShoppingCart.RecurringIntervalType, ", "productvariant.RecurringInterval, productvariant.RecurringIntervalType, ")
				+ " StateTaxRate.TaxRate as StateTaxRate , ZipTaxRate.TaxRate as ZipTaxRate " 
				+ " FROM (((((((Customer  " + DB.GetNoLock() + " RIGHT OUTER JOIN ShoppingCart  " + DB.GetNoLock() + " ON Customer.CustomerID = ShoppingCart.CustomerID) "
				+ " LEFT OUTER JOIN Product " + DB.GetNoLock() + " on ShoppingCart.ProductID=Product.ProductID) "
				+ " LEFT OUTER JOIN ProductVariant " + DB.GetNoLock() + " on ShoppingCart.VariantID=ProductVariant.VariantID) "
				+ " LEFT OUTER JOIN Address " + DB.GetNoLock() + " on Customer.ShippingAddressID=Address.AddressID) "
				+ " LEFT OUTER JOIN StateTaxRate " + DB.GetNoLock() + " ON Address.State = StateTaxRate.State) "
				+ " LEFT OUTER JOIN Country " + DB.GetNoLock() + " ON Address.Country = Country.Name) "
				+ " LEFT OUTER JOIN ZipTaxRate " + DB.GetNoLock() + " on Address.Zip = ZipTaxRate.ZipCode) "
				+ String.Format(" WHERE ShoppingCart.CartType={0} and Product.Deleted=0 and ProductVariant.Deleted=0 and ShoppingCart.CustomerID ={1} and customer.customerid={2}",((int)CartType),_thisCustomer._customerID,_thisCustomer._customerID);
			
//			if (_originalRecurringOrderNumber != 0)
//			{
//				sql += String.Format(" and OriginalRecurringOrderNumber={0}",_originalRecurringOrderNumber);
//			}
			if(_onlyLoadRecurringItemsThatAreDue && _cartType == CartTypeEnum.RecurringCart)
			{
				sql += " and NextRecurringShipDate<" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now.AddDays(1)));
			}
			IDataReader rs = DB.GetRS(sql);
			_cartItems = new ArrayList(50);
			int i = 0;

			Single LevelDiscountPercent = Common.GetCustomerLevelDiscountPercent(_thisCustomer._customerLevelID);
			String RecurringProducts = "," + Common.GetRecurringVariantsList() + ",";

			while(rs.Read())
			{
				_isEmpty = false;
				CartItem newItem;
				newItem._createdOn = DB.RSFieldDateTime(rs,"CreatedOn");
				newItem._nextRecurringShipDate = DB.RSFieldDateTime(rs,"NextRecurringShipDate");
				newItem._recurringIndex = DB.RSFieldInt(rs,"RecurringIndex");
				newItem._originalRecurringOrderNumber = DB.RSFieldInt(rs,"OriginalRecurringOrderNumber");
				newItem.ShoppingCartRecordID = DB.RSFieldInt(rs,"ShoppingCartRecID");
				newItem.cartType = (CartTypeEnum)DB.RSFieldInt(rs,"CartType");
				newItem.productID = DB.RSFieldInt(rs,"ProductID");
				newItem.variantID = DB.RSFieldInt(rs,"VariantID");
				newItem.productName = DB.RSField(rs,"ProductName");
				newItem.SKU = DB.RSField(rs,"ProductSKU");
				newItem.quantity = DB.RSFieldInt(rs,"Quantity");
				newItem.chosenColor = DB.RSField(rs,"ChosenColor");
				newItem.chosenColorSKUModifier = DB.RSField(rs,"ChosenColorSKUModifier");
				newItem.chosenSize = DB.RSField(rs,"ChosenSize");
				newItem.chosenSizeSKUModifier = DB.RSField(rs,"ChosenSizeSKUModifier");
				newItem.textOption = DB.RSField(rs,"TextOption");
				newItem.weight = DB.RSFieldSingle(rs,"ProductWeight");
				newItem.dimensions = DB.RSField(rs,"ProductDimensions");
				newItem.subscriptionMonths = DB.RSFieldInt(rs,"SubscriptionMonths");
				newItem.isUpsell = DB.RSFieldBool(rs,"IsUpsell");
				int minQ = 0;
				newItem.restrictedQuantities = Common.GetRestrictedQuantities(DB.RSFieldInt(rs,"VariantID"), out minQ);
				newItem.minimumQuantity = minQ;
				bool IsOnSale = false;
				newItem.price = DB.RSFieldDecimal(rs,"ProductPrice");
				if(_cartType != CartTypeEnum.RecurringCart && !newItem.isUpsell)
				{
					decimal NewPR = newItem.price;
					if(!Common.IsAKit(newItem.productID))
					{
						NewPR = Common.DetermineLevelPrice(newItem.variantID,_thisCustomer._customerLevelID, out IsOnSale);
						Decimal PrMod = Common.GetColorAndSizePriceDelta( DB.RSField(rs,"ChosenColor"),  DB.RSField(rs,"ChosenSize"));
						if(PrMod != System.Decimal.Zero)
						{
							NewPR += PrMod;
						}
						if(NewPR < System.Decimal.Zero)
						{
							NewPR = System.Decimal.Zero; // never know what people will put in the modifiers :)
						}

					}
					else
					{
						NewPR = DB.RSFieldDecimal(rs,"ProductPrice");
						if(LevelDiscountPercent != 0.0F)
						{
							NewPR = NewPR * (decimal)(1.00F - (LevelDiscountPercent/100.0F));
						}
					}
					if(Common.IsAPack(newItem.productID) && Common.GetPackSize(newItem.productID) == 0)
					{
						NewPR = DB.RSFieldDecimal(rs,"ProductPrice");
					}
					if(NewPR != newItem.price)
					{
						newItem.price = NewPR;
						// remember to update the actual db record now!
						DB.ExecuteSQL("update shoppingcart set ProductPrice=" + Localization.CurrencyStringForDB(newItem.price) + " where ShoppingCartRecID=" + newItem.ShoppingCartRecordID.ToString());
					}
				}
				newItem.shippingCost = DB.RSFieldDecimal(rs,"ShippingCost");
				newItem.QuantityDiscountID = 0;
				newItem.QuantityDiscountName = String.Empty;
				newItem.QuantityDiscountPercent = 0.0F;
				newItem.isTaxable = DB.RSFieldBool(rs,"IsTaxable");
				newItem.isShipSeparately = DB.RSFieldBool(rs,"IsShipSeparately");
				newItem.isDownload = DB.RSFieldBool(rs,"IsDownload");
				newItem.downloadLocation = DB.RSField(rs,"DownloadLocation");
				newItem.isSecureAttachment = DB.RSFieldBool(rs,"IsSecureAttachment");
				newItem.isRecurring = (RecurringProducts.IndexOf("," + newItem.variantID.ToString() + ",") != -1);
				newItem.recurringInterval = DB.RSFieldInt(rs,"RecurringInterval");
				if(newItem.recurringInterval == 0)
				{
					newItem.recurringInterval = 1; // for backwards compatability
				}
				newItem.recurringIntervalType = (RecurringIntervalTypeEnum)DB.RSFieldInt(rs,"RecurringIntervalType");
				if(newItem.recurringIntervalType == RecurringIntervalTypeEnum.NotUsed)
				{
					newItem.recurringIntervalType = RecurringIntervalTypeEnum.Monthly; // for backwards compatibility
				}
				// If the CartType = Recurring then use the ShoppingCart AddressIDs recorded at the order rather than the Customer Address IDs
				if (newItem.cartType == CartTypeEnum.RecurringCart)
				{
					newItem.billingAddressID = DB.RSFieldInt(rs,"ShoppingCartBillingAddressID");
					newItem.shippingAddressID = DB.RSFieldInt(rs,"ShoppingCartShippingAddressID");
				}
				else
				{
					newItem.billingAddressID = DB.RSFieldInt(rs,"CustomerBillingAddressID");
					newItem.shippingAddressID = DB.RSFieldInt(rs,"CustomerShippingAddressID");
				}
        
				_cartItems.Add(newItem);

				if(i == 0)
				{
					// first record, so load primary cart variables also:
					_email = DB.RSField(rs,"Email");
					bool couponSet = false;
					if(_thisCustomer._customerLevelID == 0 || Common.CustomerLevelAllowsCoupons(_thisCustomer._customerLevelID))
					{
						IDataReader rscoup = DB.GetRS("select * from coupon  " + DB.GetNoLock() + " where lower(couponcode)=(select lower(couponcode) from customer  " + DB.GetNoLock() + " where customerid=" + _thisCustomer._customerID.ToString() + ") and deleted=0");
						if(rscoup.Read())
						{
							couponSet = true;
							// either consumer level, or this level allows coupons, so load it if there are any:
							_coupon.code = DB.RSField(rscoup,"CouponCode");
							_coupon.description = DB.RSField(rscoup,"Description");
							_coupon.expirationDate = DB.RSFieldDateTime(rscoup,"ExpirationDate");
							_coupon.discountAmount = DB.RSFieldDecimal(rscoup,"DiscountAmount");
							_coupon.discountPercent = DB.RSFieldSingle(rscoup,"DiscountPercent");
							_coupon.includesFreeShipping = DB.RSFieldBool(rscoup,"DiscountIncludesFreeShipping");
							_coupon.ExpiresOnFirstUseByAnyCustomer = DB.RSFieldBool(rscoup,"ExpiresOnFirstUseByAnyCustomer");
							_coupon.ExpiresAfterOneUsageByEachCustomer = DB.RSFieldBool(rscoup,"ExpiresAfterOneUsageByEachCustomer");
							_coupon.ExpiresAfterNUses = DB.RSFieldInt(rscoup,"ExpiresAfterNUses");
							_coupon.RequiresMinimumOrderAmount = DB.RSFieldDecimal(rscoup,"RequiresMinimumOrderAmount");
							_coupon.ValidForCustomers = DB.RSField(rscoup,"ValidForCustomers");
							_coupon.ValidForProducts = DB.RSField(rscoup,"ValidForProducts");
							_coupon.ValidForCategories = DB.RSField(rscoup,"ValidForCategories");
							_coupon.ValidForSections = DB.RSField(rscoup,"ValidForSections");
							_coupon.ValidForManufacturers = DB.RSField(rscoup,"ValidForManufacturers");
							_coupon.NumUses = DB.RSFieldInt(rscoup,"NumUses");
						}
						rscoup.Close();
					}
					if(!couponSet)
					{
						// create an "empty" coupon!
						_coupon.code = String.Empty;
						_coupon.description = String.Empty;
						_coupon.expirationDate = System.DateTime.MinValue;
						_coupon.discountAmount = System.Decimal.Zero;
						_coupon.discountPercent = 0.0F;
						_coupon.includesFreeShipping = false;
						_coupon.ExpiresOnFirstUseByAnyCustomer = false;
						_coupon.ExpiresAfterOneUsageByEachCustomer = false;
						_coupon.ExpiresAfterNUses = 0;
						_coupon.RequiresMinimumOrderAmount = System.Decimal.Zero;
						_coupon.ValidForCustomers = String.Empty;
						_coupon.ValidForProducts = String.Empty;
						_coupon.ValidForCategories = String.Empty;
						_coupon.ValidForSections = String.Empty;
						_coupon.ValidForManufacturers = String.Empty;
						_coupon.NumUses = 0;
					}

					// Use the Address pointed to in the Address table.
					_shippingAddress.LoadByCustomer(_thisCustomer._customerID,AddressTypes.Shipping);
					_billingAddress.LoadByCustomer(_thisCustomer._customerID,AddressTypes.Billing);

					if(CartType == CartTypeEnum.RecurringCart)
					{
						_cardName = _billingAddress.CardName;
						_cardType = _billingAddress.CardType;
						_cardNumber = _billingAddress.CardNumber;
						_cardExpirationMonth = _billingAddress.CardExpirationMonth;
						_cardExpirationYear = _billingAddress.CardExpirationYear;
					}
					else
					{
						_cardName = String.Empty;
						_cardType = String.Empty;
						_cardNumber = String.Empty;
						_cardExpirationMonth = String.Empty;
						_cardExpirationYear = String.Empty;
					}

					_stateTaxRate = DB.RSFieldSingle(rs,"StateTaxRate");
					if(DB.RSFieldSingle(rs,"ZipTaxRate") != 0.0F)
					{
						_stateTaxRate = DB.RSFieldSingle(rs,"ZipTaxRate");
					}

					//V4_1    Use the shippingAddress if it's valid otherwise use the the Customer copy.
					if (_shippingAddress.AddressID == 0)
					{
						_shippingMethodID = _thisCustomer.ShippingMethodID;
					}
					else
					{
						_shippingMethodID = _shippingAddress.ShippingMethodID;
					}
					if(_shippingMethodID == 0)
					{
						_shippingMethodID = Common.AppConfigUSInt("Recurring.DefaultRecurringShippingMethodID");
						_shippingMethod = Common.AppConfig("Recurring.DefaultRecurringShippingMethod");
					}

					_orderOptions = DB.RSField(rs,"OrderOptions");
					_orderNotes = DB.RSField(rs,"OrderNotes");
					Common.ShippingCalculationEnum ShipCalcID = Common.GetActiveShippingCalculationID();
					if(ShipCalcID == Common.ShippingCalculationEnum.UseRealTimeRates)
					{
						_shippingZoneID = 0;
						_shippingMethod = _thisCustomer.ShippingMethod; // pulls from customersession if no shipping address, or shipping address if one exists
					}
					else if(ShipCalcID == Common.ShippingCalculationEnum.CalculateShippingByWeightAndZone)
					{
						_shippingZoneID = Common.GetShippingZone(_thisCustomer);
						_shippingMethod = "Ship To Zone " + _shippingZoneID.ToString();
					}
					else
					{
						_shippingZoneID = 0;
						_shippingMethod = Common.GetShippingMethodName(_shippingMethodID);
					}
				}

				i = i + 1;
			}
			_cartItems.Capacity = i;
			rs.Close();

		}
		
		public Coupon GetCoupon()
		{
			return _coupon;
		}

		public void ClearCoupon()
		{
			DB.ExecuteSQL("update customer set CouponCode=NULL where customerid=" + _thisCustomer._customerID.ToString());
			_coupon.code = String.Empty;
			_coupon.description = String.Empty;
			_coupon.expirationDate = System.DateTime.MinValue;
			_coupon.discountAmount = System.Decimal.Zero;
			_coupon.discountPercent = 0.0F;
			_coupon.includesFreeShipping = false;
			_coupon.ExpiresOnFirstUseByAnyCustomer = false;
			_coupon.ExpiresAfterOneUsageByEachCustomer = false;
			_coupon.ExpiresAfterNUses = 0;
			_coupon.RequiresMinimumOrderAmount = System.Decimal.Zero;
			_coupon.ValidForCustomers = String.Empty;
			_coupon.ValidForProducts = String.Empty;
			_coupon.ValidForCategories = String.Empty;
			_coupon.ValidForSections = String.Empty;
			_coupon.ValidForManufacturers = String.Empty;
			_coupon.NumUses = 0;
		}

		public void SetCoupon(String newCoupon)
		{
			if(newCoupon.Length == 0)
			{
				ClearCoupon();
			}
			else
			{
				newCoupon = newCoupon.ToUpper();
				DB.ExecuteSQL("update customer set CouponCode=" + DB.SQuote(newCoupon) + " where customerid=" + _thisCustomer._customerID.ToString());
				IDataReader rs = DB.GetRS("select * from Coupon  " + DB.GetNoLock() + " where upper(CouponCode)=" + DB.SQuote(newCoupon.ToUpper()));
				if(rs.Read())
				{
					_coupon.code = DB.RSField(rs,"CouponCode");
					_coupon.description = DB.RSField(rs,"Description");
					_coupon.expirationDate = DB.RSFieldDateTime(rs,"ExpirationDate");
					_coupon.discountAmount = DB.RSFieldDecimal(rs,"DiscountAmount");
					_coupon.discountPercent = DB.RSFieldSingle(rs,"DiscountPercent");
					_coupon.includesFreeShipping = DB.RSFieldBool(rs,"DiscountIncludesFreeShipping");
					_coupon.ExpiresOnFirstUseByAnyCustomer = DB.RSFieldBool(rs,"ExpiresOnFirstUseByAnyCustomer");
					_coupon.ExpiresAfterOneUsageByEachCustomer = DB.RSFieldBool(rs,"ExpiresAfterOneUsageByEachCustomer");
					_coupon.ExpiresAfterNUses = DB.RSFieldInt(rs,"ExpiresAfterNUses");
					_coupon.RequiresMinimumOrderAmount = DB.RSFieldDecimal(rs,"RequiresMinimumOrderAmount");
					_coupon.ValidForCustomers = DB.RSField(rs,"ValidForCustomers");
					_coupon.ValidForProducts = DB.RSField(rs,"ValidForProducts");
					_coupon.ValidForCategories = DB.RSField(rs,"ValidForCategories");
					_coupon.ValidForSections = DB.RSField(rs,"ValidForSections");
					_coupon.ValidForManufacturers = DB.RSField(rs,"ValidForManufacturers");
					_coupon.NumUses = DB.RSFieldInt(rs,"NumUses");
				}
				else
				{
					_coupon.code = newCoupon;
					_coupon.description = String.Empty;
					_coupon.expirationDate = System.DateTime.MinValue;
					_coupon.discountAmount = System.Decimal.Zero;
					_coupon.discountPercent = 0.0F;
					_coupon.includesFreeShipping = false;
					_coupon.ExpiresOnFirstUseByAnyCustomer = false;
					_coupon.ExpiresAfterOneUsageByEachCustomer = false;
					_coupon.ExpiresAfterNUses = 0;
					_coupon.RequiresMinimumOrderAmount = System.Decimal.Zero;
					_coupon.ValidForCustomers = String.Empty;
					_coupon.ValidForProducts = String.Empty;
					_coupon.ValidForCategories = String.Empty;
					_coupon.ValidForSections = String.Empty;
					_coupon.ValidForManufacturers = String.Empty;
					_coupon.NumUses = 0;
				}
				rs.Close();
			}
		}

		private int GetQForQDis(int ProductID, int VariantID)
		{
			int Q = 0;
			// ignore size & colors
			foreach(CartItem c in _cartItems)
			{
				if(c.productID == ProductID && c.variantID == VariantID)
				{
					Q += c.quantity;
				}
			}
			return Q;
		}

		//V3_9
		public int GetQuantityInCart(int ProductID, int VariantID)
		{
			int Q = 0;
			// ignore size & colors
			foreach(CartItem c in _cartItems)
			{
				if(c.productID == ProductID && c.variantID == VariantID)
				{
					Q += c.quantity;
				}
			}
			return Q;
		}

		public Decimal OptionsTotal()
		{
			Decimal sum = System.Decimal.Zero;
			if(_orderOptions.Length != 0)
			{
				IDataReader rs = DB.GetRS("Select * from orderoption  " + DB.GetNoLock() + " where orderoptionid in (" + _orderOptions + ")");
				while(rs.Read())
				{
					sum += DB.RSFieldDecimal(rs,"Cost");
				}
				rs.Close();
			}
			return sum;
		}

		public Decimal SubTotal(bool includeDiscount, bool onlyIncludeTaxableItems, bool includeDownloadItems)
		{
			Decimal sum = System.Decimal.Zero;
			foreach(CartItem c in _cartItems)
			{
				bool includeThisItem = true;
				if(onlyIncludeTaxableItems && !c.isTaxable)
				{
					includeThisItem = false;
				}
				if(c.isDownload && !includeDownloadItems)
				{
					includeThisItem = false;
				}
				if(includeThisItem)
				{
					//sum += ( c.price * c.quantity);
					int Q = c.quantity;
					decimal PR = c.price * Q;
					if(Common.CustomerLevelAllowsQuantityDiscounts(_thisCustomer._customerLevelID))
					{
						int ActiveDID = Common.LookupActiveVariantQuantityDiscountID(c.variantID);
						Single DIDPercent = Common.GetDIDPercent(ActiveDID,GetQForQDis(c.productID,c.variantID));
						if(ActiveDID != 0 && DIDPercent != 0.0)
						{
							PR = (decimal)(1.0-(DIDPercent/100.0)) * PR;
						}
					}
					sum += PR;
				}
			}
			if(_thisCustomer._customerLevelID == 0)
			{
				if(HasCoupon())
				{
					if(includeDiscount && CouponIsValid() == "OK")
					{
						sum -= _coupon.discountAmount;
						sum = (decimal)((Single)sum * (100.0 - _coupon.discountPercent)/100);
					}
				}
			}
			else
			{
				if(Common.CustomerLevelAllowsCoupons(_thisCustomer._customerLevelID))
				{
					if(HasCoupon())
					{
						if(includeDiscount && CouponIsValid() == "OK")
						{
							sum -= _coupon.discountAmount;
							sum = (decimal)((Single)sum * (100.0 - _coupon.discountPercent)/100);
						}
					}
				}
				decimal CustomerLevelDiscountAmount = Common.GetCustomerLevelDiscountAmount(_thisCustomer._customerLevelID);
				sum = sum - CustomerLevelDiscountAmount;
			}
			if(sum < System.Decimal.Zero)
			{
				sum = System.Decimal.Zero;
			}
			sum += OptionsTotal();
			return sum;
		}

		public Decimal TaxTotal(bool includeDiscount)
		{
			if(_thisCustomer._customerLevelID != 0 && Common.CustomerLevelHasNoTax(_thisCustomer._customerLevelID))
			{
				return System.Decimal.Zero;
			}
			decimal TaxableAmt = SubTotal(includeDiscount,true,true);
			if(Common.AppConfigBool("TaxOnShipping"))
			{
				TaxableAmt += ShippingTotal(true);
			}
			if(TaxableAmt == System.Decimal.Zero)
			{
				return System.Decimal.Zero;
			}
			return (decimal)(_stateTaxRate/100.0F * (Single)TaxableAmt);
		}

		public Single WeightTotal()
		{
			Single sum = 0.0F;
			foreach(CartItem c in _cartItems)
			{
				if(!c.isDownload)
				{
					Single thisW = c.weight;
					if(Common.GetActiveShippingCalculationID() == Common.ShippingCalculationEnum.UseRealTimeRates)
					{
						// RT shipping only:
						if(thisW == 0.0F)
						{
							thisW = Common.AppConfigUSSingle("RTShipping.DefaultItemWeight"); // NOTHING is weightless!
						}
					}
					sum += (c.quantity * thisW);
				}
			}
			if(sum == 0.0F)
			{
				float MinOrderWeight = 0.5F;
				if(Common.AppConfig("MinOrderWeight").Length != 0)
				{
					try
					{
						MinOrderWeight = Common.AppConfigUSSingle("MinOrderWeight");
					}
					catch {}
				}
				sum = MinOrderWeight; // must have SOMETHING to use!
			}
			return sum;
		}

		public Decimal ShippingTotal(bool includeDiscount)
		{
			if(_thisCustomer._customerLevelID != 0 && Common.CustomerLevelHasFreeShipping(_thisCustomer._customerLevelID))
			{
				return System.Decimal.Zero;
			}
			if(HasCoupon())
			{
				if(CouponIsValid() == "OK" && _coupon.includesFreeShipping)
				{
					return System.Decimal.Zero;
				}
			}
			if(this.IsAllDownloadComponents())
			{
				return System.Decimal.Zero;
			}
			decimal FreeShippingThreshold = Common.AppConfigUSDecimal("FreeShippingThreshold");
			Decimal sum = SubTotal(includeDiscount,false,false);
			if(FreeShippingThreshold != System.Decimal.Zero && sum >= FreeShippingThreshold)
			{
				return System.Decimal.Zero;
			}
			Decimal shp = System.Decimal.Zero;
			IDataReader rs;
			Common.ShippingCalculationEnum ActiveCalcID = Common.GetActiveShippingCalculationID();
			Single TotalWeight = 0.0F;
			switch(ActiveCalcID)
			{
				case Common.ShippingCalculationEnum.CalculateShippingByWeight:
					TotalWeight = WeightTotal();
					rs = DB.GetRS("select * from ShippingByWeight  " + DB.GetNoLock() + " where LowValue<=" + Localization.SingleStringForDB(TotalWeight) + " and HighValue>=" + Localization.SingleStringForDB(TotalWeight) + " and ShippingMethodID=" + _shippingMethodID.ToString());
					if(rs.Read())
					{
						shp = (decimal)DB.RSFieldDecimal(rs,"ShippingCharge");
					}
					rs.Close();
					break;
				case Common.ShippingCalculationEnum.CalculateShippingByTotal:
					rs = DB.GetRS("select * from ShippingByTotal  " + DB.GetNoLock() + " where LowValue<=" + Localization.CurrencyStringForDB(sum) + " and HighValue>=" + Localization.CurrencyStringForDB(sum) + " and ShippingMethodID=" + _shippingMethodID.ToString());
					if(rs.Read())
					{
						shp = (decimal)DB.RSFieldDecimal(rs,"ShippingCharge");
					}
					rs.Close();
					break;
				case Common.ShippingCalculationEnum.UseFixedPrice:
					rs = DB.GetRS("Select * from ShippingMethod  " + DB.GetNoLock() + " where ShippingMethodID=" + _shippingMethodID.ToString());
					shp = System.Decimal.Zero;
					if(rs.Read())
					{
						shp = (decimal)DB.RSFieldSingle(rs,"FixedRate");
					}
					rs.Close();
					break;
				case Common.ShippingCalculationEnum.AllOrdersHaveFreeShipping:
					shp = System.Decimal.Zero;
					break;
				case Common.ShippingCalculationEnum.UseFixedPercentageOfTotal:
					rs = DB.GetRS("Select * from ShippingMethod  " + DB.GetNoLock() + " where ShippingMethodID=" + _shippingMethodID.ToString());
					decimal shipPercent = System.Decimal.Zero;
					if(rs.Read())
					{
						shipPercent = (decimal)DB.RSFieldSingle(rs,"FixedPercentOfTotal");
					}
					rs.Close();
					Decimal sum2 = SubTotal(includeDiscount,false,false);
					shp = sum2 * (shipPercent/100.0M);
					break;
				case Common.ShippingCalculationEnum.UseIndividualItemShippingCosts:
					shp = System.Decimal.Zero;
					foreach(CartItem c  in _cartItems)
					{
						int Q = c.quantity;
						decimal PR = c.shippingCost * Q;
						shp += PR;
					}
					if(shp < System.Decimal.Zero)
					{
						shp = System.Decimal.Zero;
					}
					break;
				case Common.ShippingCalculationEnum.UseRealTimeRates:
					// the shipping "cost" was stuffed into the shippingmethod field of their customer record
					// not elegant, but works ok, now extract it again:
					if(_shippingMethod.Length == 0)
					{
						// no value set, so default to first in RTShipping list so site totals match what default RTShipping list says
						String ShipZip = _thisCustomer.ShippingZip;
						if(ShipZip.Length == 0)
						{
							shp = System.Decimal.Zero; // insufficent data, set to 0, customer must choose zip before checkout
						}
						else
						{
							String[] shipsplit = _thisCustomer.ShippingMethod.Split('|');
							try
							{
								shp = System.Decimal.Parse(shipsplit[1]);
							}
							catch {}
						}
					}
					else
					{
						String[] shipsplit = _shippingMethod.Split('|');
						try
						{
							shp = System.Decimal.Parse(shipsplit[1]);
						}
						catch {}
					}
					break;
				case Common.ShippingCalculationEnum.CalculateShippingByWeightAndZone:
					TotalWeight = WeightTotal();
					rs = DB.GetRS("select * from ShippingByZone  " + DB.GetNoLock() + " where LowValue<=" + Localization.SingleStringForDB(TotalWeight) + " and HighValue>=" + Localization.SingleStringForDB(TotalWeight) + " and ShippingZoneID=" + _shippingZoneID.ToString());
					if(rs.Read())
					{
						shp = (decimal)DB.RSFieldDecimal(rs,"ShippingCharge");
					}
					else
					{
						shp = Common.AppConfigUSDecimal("ShippingCostWhenNoZoneMatch");
					}
					rs.Close();
					break;
			}
			// now add fixed "handling" fee for all non-zero shipping totals
			// Note: RTShipping already has the extra fee factored in the select lists!
			if(Common.AppConfig("ShippingHandlingExtraFee").Length != 0 && ActiveCalcID != Common.ShippingCalculationEnum.UseRealTimeRates)
			{
				decimal extra = System.Decimal.Zero;
				try
				{
					extra = (decimal)Common.AppConfigUSSingle("ShippingHandlingExtraFee");
				}
				catch
				{
					extra = System.Decimal.Zero;
				}
				if(extra > System.Decimal.Zero)
				{
					shp += extra;
				}
			}
			return shp;
		}

		public Decimal Total(bool includeDiscount)
		{
			return SubTotal(includeDiscount,false,true) + TaxTotal(includeDiscount) + ShippingTotal(includeDiscount);
		}

		public String DisplayWish()
		{
			String BACKURL = Common.IIF(Common.QueryStringUSInt("ResetLinkback") == 0, "javascript:history.back();", Common.GetCartContinueShoppingURL());
			if(_isEmpty)
			{
				Topic t1 = new Topic("EmptyWishListText",_thisCustomer._localeSetting,_siteID);
				return t1._contents;
			}
			StringBuilder tmpS = new StringBuilder(50000);
			bool readOnlyVar = true;
			tmpS.Append("<div align=\"left\">");
			tmpS.Append("<table border=\"0\" cellpadding=\"3\" cellspacing=\"0\" width=\"100%\">");
			tmpS.Append("<tr>");
			tmpS.Append("<td  valign=\"bottom\" align=\"left\"><b>PRODUCT</b></td>");
			tmpS.Append("<td align=\"left\" valign=\"bottom\"><b>SKU</b></td>");
			tmpS.Append("<td align=\"center\" valign=\"bottom\"><b>" + Common.AppConfig("ColorOptionPrompt").ToUpper() + "</b></td>");
			tmpS.Append("<td align=\"center\" valign=\"bottom\"><b>" + Common.AppConfig("SizeOptionPrompt").ToUpper() + "</b></td>");
			tmpS.Append("<td align=\"center\" valign=\"bottom\"><b>QUANTITY</b></td>");
			tmpS.Append("<td width=\"150\" align=\"right\" valign=\"bottom\"><b>PRICE</b></td>");
			tmpS.Append("<td align=\"right\" valign=\"bottom\"><b>&nbsp;</b></td>");
			tmpS.Append("</tr>");
			tmpS.Append("<tr>");
			tmpS.Append("<td valign=\"top\" align=\"left\" colspan=\"7\" height=\"3\">");
			tmpS.Append("<hr noshade size=\"2\" width=\"100%\">");
			tmpS.Append("</td>");
			tmpS.Append("</tr>");
			bool ShowLinkBack = Common.AppConfigBool("LinkToProductPageInCart");
			foreach(CartItem c in _cartItems)
			{
				tmpS.Append("<tr>");
				tmpS.Append("<td  valign=\"top\" align=\"left\">");
				if(ShowLinkBack)
				{
					tmpS.Append("<a href=\"" + SE.MakeProductLink(c.productID,"") + "\">");
				}
				tmpS.Append(c.productName);
				if(c.textOption.Length != 0)
				{
					tmpS.Append("<br>(Text: " + c.textOption + ")");
				}
				if(ShowLinkBack)
				{
					tmpS.Append("</a>");
				}
				if(Common.IsAKit(c.productID))
				{
					if(!readOnlyVar)
					{
						tmpS.Append(":&nbsp;&nbsp;<a href=\"ShoppingCart_change.aspx?recid=" + c.ShoppingCartRecordID.ToString() + "\"><img src=\"skins/Skin_" + _siteID.ToString() + "/images/edit2.gif\" align=\"absmiddle\" border=\"0\" alt=\"Change This Kit\"></a>&nbsp;");
					}
					IDataReader rsx = DB.GetRS("select kititem.name, kitcart.quantity from kitcart  " + DB.GetNoLock() + " inner join kititem  " + DB.GetNoLock() + " on kitcart.kititemid=kititem.kititemid where ShoppingCartrecid=" + c.ShoppingCartRecordID.ToString());
					String tmp = String.Empty;
					while(rsx.Read())
					{
						tmp += "&nbsp;&nbsp;-&nbsp;(" + DB.RSFieldInt(rsx,"Quantity").ToString() + ")&nbsp;" + DB.RSField(rsx,"Name") + "<br>";
					}
					rsx.Close();
					tmpS.Append("<br>");
					tmpS.Append(tmp);
				}
				if(Common.IsAPack(c.productID))
				{
					CustomCart ccart = new CustomCart(_thisCustomer._customerID,c.ShoppingCartRecordID, c.productID, _siteID);
					if(!readOnlyVar)
					{
						tmpS.Append(":&nbsp;&nbsp;<a href=\"ShoppingCart_change.aspx?recid=" + c.ShoppingCartRecordID.ToString() + "\"><img src=\"skins/Skin_" + _siteID.ToString() + "/images/edit2.gif\" align=\"absmiddle\" border=\"0\" alt=\"Change This Pack\"></a>&nbsp;");
					}
					String tmp = ccart.GetContents(false,"<br>");
					tmpS.Append("<br>");
					tmpS.Append(tmp);
				}
				tmpS.Append("</td>");
				tmpS.Append("<td align=\"left\" valign=\"top\">" + c.SKU + "</td>");
				tmpS.Append("<td align=\"center\" valign=\"top\">" +(Common.IIF(c.chosenColor.Length == 0 , "--" , c.chosenColor)) + "</td>");
				tmpS.Append("<td align=\"center\" valign=\"top\">" +(Common.IIF(c.chosenSize.Length == 0 , "--" , c.chosenSize)) + "</td>");
				tmpS.Append("<td align=\"center\" valign=\"top\">");
				if(Common.AppConfigBool("ShowCartDeleteItemButton"))
				{
					tmpS.Append(c.quantity);
					tmpS.Append("&nbsp;<img style=\"cursor:hand;\" onClick=\"self.location='wishlist.aspx?deleteid=" + c.ShoppingCartRecordID.ToString() + "';\" align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/delete.gif\" width=\"32\" height=\"16\" border=\"0\">");
				}
				tmpS.Append("</td>");
				tmpS.Append("<td align=\"right\" valign=\"top\">");
				int Q = c.quantity;
				decimal PR = c.price * Q;
				int ActiveDID = 0;
				Single DIDPercent = 0.0F;
				if(Common.CustomerLevelAllowsQuantityDiscounts(_thisCustomer._customerLevelID))
				{
					ActiveDID = Common.LookupActiveVariantQuantityDiscountID(c.variantID);
					DIDPercent = Common.GetDIDPercent(ActiveDID,Q);
					if(ActiveDID != 0 && DIDPercent != 0.0)
					{
						PR = (decimal)(1.0-(DIDPercent/100.0)) * PR;
					}
				}
				tmpS.Append(Localization.CurrencyStringForDisplay(PR));
				if(Common.CustomerLevelAllowsQuantityDiscounts(_thisCustomer._customerLevelID))
				{
					if(ActiveDID != 0 && DIDPercent != 0.0F)
					{
						tmpS.Append(" <small>(" + DIDPercent.ToString() + "% Quan Dis)</small>");
					}
				}
				tmpS.Append("</td>");
				tmpS.Append("<td><input type=\"button\" name=\"Move To " + Common.AppConfig("CartPrompt") + "\" value=\"Move To " + Common.AppConfig("CartPrompt") + "\" onClick=\"self.location='wishlist.aspx?movetocartid=" + c.ShoppingCartRecordID.ToString() + "';\"></td>");
				tmpS.Append("</tr>");
			}

			tmpS.Append("</table>");
			tmpS.Append("</div>");
			return tmpS.ToString();
		}

		public String DisplayRecurring(int OriginalRecurringOrderNumber)
		{
			String BACKURL = Common.IIF(Common.QueryStringUSInt("ResetLinkback") == 0, "javascript:history.back();", Common.GetCartContinueShoppingURL());
			if(_isEmpty)
			{
				Topic t1 = new Topic("EmptyRecurringListText",_thisCustomer._localeSetting,_siteID);
				return t1._contents;
			}
			StringBuilder tmpS = new StringBuilder(50000);
			bool readOnlyVar = true;
			tmpS.Append("<div align=\"left\">");

			CartItem co = (CartItem)_cartItems[0];
      
			tmpS.Append("<form style=\"margin-top: 0px; margin-bottom: 0px;\" name=\"ChangeDay\" action=\"cst_recurring.aspx?customerid=" + _thisCustomer._customerID.ToString() + "\" method=\"POST\">");
			tmpS.Append(String.Format("<table width=\"100%\"><tr><td valign=\"top\" align=\"left\"><b>Original Recurring Order Number: {0}, RecurringIndex={1}<br>Created On {2}</b></td>",OriginalRecurringOrderNumber,co._recurringIndex,Localization.ToNativeShortDateString(co._createdOn)));
			tmpS.Append("<td align=\"right\" valign=\"top\">");
			//tmpS.Append(String.Format("<b>Next Ship Date: {0}</b>",Localization.ToNativeShortDateString(co._nextRecurringShipDate)));

			if(Common.IsAdminSite && Common.AppConfigBool("AllowRecurringIntervalEditing"))
			{
				tmpS.Append("<input type=\"hidden\" name=\"OriginalRecurringOrderNumber\" value=\"" + OriginalRecurringOrderNumber.ToString() + "\">");
				tmpS.Append("Recurring Interval: <input type=\"text\" size=\"2\" maxlength=\"4\" name=\"RecurringInterval\" id=\"RecurringInterval\" value=\"" + co.recurringInterval.ToString() + "\">&nbsp;");
//				tmpS.Append("<b>Change Ship Day For This Order:</b> <select size=1 name=\"ShipDay\"><option value=\"0\">--</option>");
//				for(int i = 1; i <= 31; i++)
//				{
//					tmpS.Append("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>");
//				}
//				tmpS.Append("</select>&nbsp;<input type=\"submit\" name=\"Submit\" value=\"Submit\">");
				tmpS.Append("Recurring Interval Type: <select name=\"RecurringIntervalType\" id=\"RecurringIntervalType\" size=\"1\">");
				tmpS.Append("<option value=\"" + ((int)RecurringIntervalTypeEnum.Daily).ToString() + "\" " + Common.IIF(co.recurringIntervalType == RecurringIntervalTypeEnum.Daily, " selected ","") + ">" + RecurringIntervalTypeEnum.Daily.ToString() + "</option>");
				tmpS.Append("<option value=\"" + ((int)RecurringIntervalTypeEnum.Weekly).ToString() + "\" " + Common.IIF(co.recurringIntervalType == RecurringIntervalTypeEnum.Weekly, " selected ","") + ">" + RecurringIntervalTypeEnum.Weekly.ToString() + "</option>");
				tmpS.Append("<option value=\"" + ((int)RecurringIntervalTypeEnum.Monthly).ToString() + "\" " + Common.IIF(co.recurringIntervalType == RecurringIntervalTypeEnum.Monthly, " selected ","") + ">" + RecurringIntervalTypeEnum.Monthly.ToString() + "</option>");
				tmpS.Append("<option value=\"" + ((int)RecurringIntervalTypeEnum.Yearly).ToString() + "\" " + Common.IIF(co.recurringIntervalType == RecurringIntervalTypeEnum.Yearly, " selected ","") + ">" + RecurringIntervalTypeEnum.Yearly.ToString() + "</option>");
				tmpS.Append("</select>");
				tmpS.Append("&nbsp;<input type=\"submit\" value=\"Go\">");
			}

			tmpS.Append("</td>");
			tmpS.Append("</tr></table>");
			if(Common.IsAdminSite)
			{
				tmpS.Append("</form>");
			}

			tmpS.Append("<table border=\"0\" cellpadding=\"2\" cellspacing=\"0\" width=\"100%\">");
			tmpS.Append("<tr>");
			tmpS.Append("<td valign=\"top\" align=\"left\" colspan=\"8\" height=\"3\">");
			tmpS.Append("<hr noshade size=\"2\" width=\"100%\">");
			tmpS.Append("</td>");
			tmpS.Append("</tr>");
			tmpS.Append("<tr>");
			tmpS.Append("<td  valign=\"bottom\" align=\"left\"><b>PRODUCT</b></td>");
			tmpS.Append("<td align=\"left\" valign=\"bottom\"><b>SKU</b></td>");
			tmpS.Append("<td align=\"center\" valign=\"bottom\"><b>" + Common.AppConfig("ColorOptionPrompt").ToUpper() + "</b></td>");
			tmpS.Append("<td align=\"center\" valign=\"bottom\"><b>" + Common.AppConfig("SizeOptionPrompt").ToUpper() + "</b></td>");
			tmpS.Append("<td align=\"center\" valign=\"bottom\"><b>QUANTITY</b></td>");
			tmpS.Append("<td align=\"right\" valign=\"bottom\"><b>PRICE</b></td>");
			tmpS.Append("<td align=\"center\" valign=\"bottom\"><b>NEXT SHIP DATE</b></td>");
			//			tmpS.Append("<td align=\"center\" valign=\"bottom\"><b>CREATED ON</b></td>");
			//			tmpS.Append("<td align=\"center\" valign=\"bottom\"><b>NEXT SHIP DATE</b></td>");
			tmpS.Append("<td align=\"center\" valign=\"bottom\"><b>CANCEL</b></td>");
			tmpS.Append("</tr>");
			tmpS.Append("<tr>");
			tmpS.Append("<td valign=\"top\" align=\"left\" colspan=\"8\" height=\"3\">");
			tmpS.Append("<hr noshade size=\"2\" width=\"100%\">");
			tmpS.Append("</td>");
			tmpS.Append("</tr>");
			bool ShowLinkBack = Common.AppConfigBool("LinkToProductPageInCart");

			int OrderShippingAddressID = 0;
			int OrderBillingAddressID = 0;

			foreach(CartItem c in _cartItems)
			{
				if(c._originalRecurringOrderNumber == OriginalRecurringOrderNumber)
				{
					OrderShippingAddressID = c.shippingAddressID;
					OrderBillingAddressID = c.billingAddressID;

					tmpS.Append("<tr>");
					tmpS.Append("<td  width=\"15%\" valign=\"top\" align=\"left\" rowspan=\"1\">");
					if(Common.IsAdminSite)
					{
						tmpS.Append("<a href=\"cst_recurring.aspx?customerid=" + _thisCustomer._customerID.ToString() + "\">");
					}
					else
					{
						if(ShowLinkBack)
						{
							tmpS.Append("<a href=\"" + SE.MakeProductLink(c.productID,"") + "\">");
						}
					}
					tmpS.Append(c.productName);
					//if(Common.IsAdminSite)
					//{
					//	tmpS.Append(" (RecurringIndex=" + c._recurringIndex.ToString() + ")");
					//}
					if(c.textOption.Length != 0)
					{
						tmpS.Append("<br>(Text: " + c.textOption + ")");
					}
					if(Common.IsAdminSite || ShowLinkBack)
					{
						tmpS.Append("</a>");
					}
					if(Common.IsAKit(c.productID))
					{
						if(!readOnlyVar)
						{
							tmpS.Append(":&nbsp;&nbsp;<a href=\"ShoppingCart_change.aspx?recid=" + c.ShoppingCartRecordID.ToString() + "\"><img src=\"skins/Skin_" + _siteID.ToString() + "/images/edit2.gif\" align=\"absmiddle\" border=\"0\" alt=\"Change This Kit\"></a>&nbsp;");
						}
						IDataReader rsx = DB.GetRS("select kititem.name, kitcart.quantity from kitcart  " + DB.GetNoLock() + " inner join kititem  " + DB.GetNoLock() + " on kitcart.kititemid=kititem.kititemid where ShoppingCartrecid=" + c.ShoppingCartRecordID.ToString());
						String tmp = String.Empty;
						while(rsx.Read())
						{
							tmp += "&nbsp;&nbsp;-&nbsp;(" + DB.RSFieldInt(rsx,"Quantity").ToString() + ")&nbsp;" + DB.RSField(rsx,"Name") + "<br>";
						}
						rsx.Close();
						tmpS.Append("<br>");
						tmpS.Append(tmp);
					}
					if(Common.IsAPack(c.productID))
					{
						CustomCart ccart = new CustomCart(_thisCustomer._customerID,c.ShoppingCartRecordID,c.productID,_siteID);
						if(!readOnlyVar)
						{
							tmpS.Append(":&nbsp;&nbsp;<a href=\"ShoppingCart_change.aspx?recid=" + c.ShoppingCartRecordID.ToString() + "\"><img src=\"skins/Skin_" + _siteID.ToString() + "/images/edit2.gif\" align=\"absmiddle\" border=\"0\" alt=\"Change This Pack\"></a>&nbsp;");
						}
						String tmp = ccart.GetContents(false,"<br>");
						tmpS.Append("<br>");
						tmpS.Append(tmp);
					}
					tmpS.Append("</td>");
					tmpS.Append("<td align=\"left\" valign=\"top\">" + c.SKU + "</td>");
					tmpS.Append("<td align=\"center\" valign=\"top\">" +(Common.IIF(c.chosenColor.Length == 0 , "--" , c.chosenColor)) + "</td>");
					tmpS.Append("<td align=\"center\" valign=\"top\">" +(Common.IIF(c.chosenSize.Length == 0 , "--" , c.chosenSize)) + "</td>");
					tmpS.Append("<td align=\"center\" valign=\"top\">");
					tmpS.Append(c.quantity);
					tmpS.Append("</td>");
					tmpS.Append("<td align=\"right\" valign=\"top\">");
					int Q = c.quantity;
					decimal PR = c.price * Q;
					int ActiveDID = 0;
					Single DIDPercent = 0.0F;
					if(Common.CustomerLevelAllowsQuantityDiscounts(_thisCustomer._customerLevelID))
					{
						ActiveDID = Common.LookupActiveVariantQuantityDiscountID(c.variantID);
						DIDPercent = Common.GetDIDPercent(ActiveDID,Q);
						if(ActiveDID != 0 && DIDPercent != 0.0)
						{
							PR = (decimal)(1.0-(DIDPercent/100.0)) * PR;
						}
					}
					tmpS.Append(Localization.CurrencyStringForDisplay(PR));
					if(Common.CustomerLevelAllowsQuantityDiscounts(_thisCustomer._customerLevelID))
					{
						if(ActiveDID != 0 && DIDPercent !=0.0)
						{
							tmpS.Append(" <small>(" + DIDPercent.ToString() + "% Quan Dis)</small>");
						}
					}
					tmpS.Append("</td>");
					
					tmpS.Append("<td align=\"center\" valign=\"top\">");
					tmpS.Append(Localization.ToNativeShortDateString(c._nextRecurringShipDate));
					tmpS.Append("</td>");

					tmpS.Append("<td align=\"center\" valign=\"top\" rowspan=\"1\">");
					tmpS.Append("<img style=\"cursor:hand;\" onClick=\"if(confirm('Are you sure you want to stop future billing & shipment for this item?')) {self.location='" + Common.IIF(Common.IsAdminSite, "cst_history.aspx?customerid=" + _thisCustomer._customerID.ToString() + "&","account.aspx?") + "deleteid=" + c.ShoppingCartRecordID.ToString() + "';}\" align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/delete.gif\" width=\"32\" height=\"16\" border=\"0\">");
					tmpS.Append("  </td>");
					tmpS.Append("  </tr>");

				}
			}

			//Show billing and shipping address for the Order group
			tmpS.Append("<tr>");
			tmpS.Append("  <td colspan=\"8\"><hr></td>");
			tmpS.Append("  </tr>");
			tmpS.Append("<tr>");
			tmpS.Append("  <td colspan=\"8\">");

			Address BillingAddress = new Address();
			BillingAddress.LoadFromDB(OrderBillingAddressID);

			Address ShippingAddress = new Address();
			ShippingAddress.LoadFromDB(OrderShippingAddressID);
        
			tmpS.Append("<div align=\"center\">");
			tmpS.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" >");
			tmpS.Append("<tr>");
			tmpS.Append(String.Format("<td valign=\"top\"><img style=\"cursor:hand;\" src=\"skins/skin_" + _siteID.ToString() +"/images/change.gif\" border=\"0\" align=\"absmiddle\" onClick=\"self.location='" + Common.IIF(Common.IsAdminSite, "cst_selectaddress.aspx?customerid={0}&","selectaddress.aspx?") + "OriginalRecurringOrderNumber={1}&AddressType=Billing&ReturnUrl={2}'\">&nbsp;</td>",_thisCustomer._customerID,OriginalRecurringOrderNumber,HttpContext.Current.Server.UrlEncode(Common.PageInvocation()))); 
			tmpS.Append("  <td valign=\"top\">");
			tmpS.Append(String.Format("<b>Bill to: </b> {0}",BillingAddress.DisplayHTML()));
			tmpS.Append(String.Format("<b>Payment Method:</b><br> {0}",BillingAddress.DisplayPaymentMethod));
			tmpS.Append("  </td>");
			tmpS.Append("  <td width=\"20\">&nbsp;");
			tmpS.Append("  </td>");
			tmpS.Append(String.Format("<td valign=\"top\"><img style=\"cursor:hand;\" src=\"skins/skin_" + _siteID.ToString() +"/images/change.gif\" border=\"0\" align=\"absmiddle\" onClick=\"self.location='" + Common.IIF(Common.IsAdminSite, "cst_selectaddress.aspx?customerid={0}&","selectaddress.aspx?") + "OriginalRecurringOrderNumber={1}&AddressType=Shipping&ReturnUrl={2}'\">&nbsp;</td>",_thisCustomer._customerID,OriginalRecurringOrderNumber,HttpContext.Current.Server.UrlEncode(Common.PageInvocation()))); 
			tmpS.Append("  <td valign=\"top\">");
			tmpS.Append(String.Format("<b>Ship to: </b> {0}",ShippingAddress.DisplayHTML()));
			tmpS.Append(String.Format("<b>Shipping Method:</b><br> {0}",ShippingAddress.ShippingMethod));
			tmpS.Append("  </td>");
			tmpS.Append("  </tr>");
			tmpS.Append("</table>");
			tmpS.Append("</div>");

      
			tmpS.Append("  </td>");
			tmpS.Append("  </tr>");
			tmpS.Append("</table>");
			tmpS.Append("</div>");

			return tmpS.ToString();
		}

		private bool OptionIsSelected(int OptionID, String Options)
		{
			return (("," + Options + ",").IndexOf("," + OptionID.ToString() + ",") != -1);
		}

		// yes, this is the BIG MAMA routine in the entire product
		// yes, we don't like HTML mixed in but it's life...
		// and yes, modify with EXTREME care...
		public String Display(bool readOnlyVar, int SiteID, bool IsAnon)
		{
			String BACKURL = Common.IIF(Common.QueryStringUSInt("ResetLinkback") == 0, "javascript:history.back();", Common.GetCartContinueShoppingURL());

			if(_isEmpty)
			{
				return "<br><b>Your " + Common.AppConfig("CartPrompt").ToLower() + " is currently empty.</b><br><br><a href=\"" + BACKURL + "\">Continue Shopping...</a><br><br><br><br>";
			}
			if(_cartType == CartTypeEnum.WishCart)
			{
				return DisplayWish();
			}
			else if(_cartType == CartTypeEnum.RecurringCart)
			{
				return DisplayRecurring(((CartItem)_cartItems[0])._originalRecurringOrderNumber);
			}
			StringBuilder tmpS = new StringBuilder(50000);
			tmpS.Append("<br clear=all>");
			tmpS.Append(Common.GetJSPopupRoutines());
			if(!readOnlyVar)
			{
				tmpS.Append("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				tmpS.Append("function Cart_Validator(theForm)\n");
				tmpS.Append("{\n");
				String cartJS = Common.ReadFile("ShoppingCart.js",true);
				int TotalQ = 0;
				foreach(CartItem c in _cartItems)
				{
					tmpS.Append(cartJS.Replace("%SKU%",c.ShoppingCartRecordID.ToString()));
					TotalQ += c.quantity;
				}
				if(TotalQ < Common.AppConfigUSInt("MinCartItemsBeforeCheckout"))
				{
					tmpS.Append("if(theForm.CheckN.value == '1')\n");
					tmpS.Append("{\n");
					tmpS.Append("	alert('Please make sure you have ordered at least " + Common.AppConfigUSInt("MinCartItemsBeforeCheckout").ToString() + " items - any " + Common.AppConfigUSInt("MinCartItemsBeforeCheckout").ToString() + " items from our site - before checking out.');\n");
					tmpS.Append("	document.CartForm.ContinueCheckout.value='0';\n");
					tmpS.Append("	submitenabled(theForm);\n");
					tmpS.Append("	return(false);\n");
					tmpS.Append("}\n");
				}

				tmpS.Append("return(true);\n");
				tmpS.Append("}\n");
				tmpS.Append("</script>\n");
			}

			String ShowShipText = _shippingMethod;
			String ShippingCity = _thisCustomer.ShippingCity;
			String ShippingState = _thisCustomer.ShippingState;
			String ShippingZip = _thisCustomer.ShippingZip;
			String ShippingCountry = _thisCustomer.ShippingCountry;

			Common.ShippingCalculationEnum ShipCalcID = Common.GetActiveShippingCalculationID();
			
			if(!readOnlyVar)
			{
				tmpS.Append("<form method=\"POST\" action=\"ShoppingCart_process.aspx\" onsubmit=\"return (validateForm(this) && Cart_Validator(this))\" name=\"CartForm\" id=\"CartForm\">");
				tmpS.Append("<input type=\"hidden\" id=\"CheckN\" name=\"CheckN\" value=\"1\">");
				tmpS.Append("<input type=\"hidden\" id=\"ContinueCheckout\" name=\"ContinueCheckout\" value=\"0\">");
				tmpS.Append("<input type=\"hidden\" id=\"GetRTShipping\" name=\"GetRTShipping\" value=\"" + Common.IIF(ShipCalcID == Common.ShippingCalculationEnum.UseRealTimeRates && (!this.IsAllDownloadComponents() && ShippingZip.Length == 0), "1", "0") + "\">"); //  MUST always do a rate get if they are entering the cart without a zip, this will force them back to the cart even if they select continue checkout, so they can see the shipping cost
				tmpS.Append("<input type=\"hidden\" id=\"GetZoneShipping\" name=\"GetZoneShipping\" value=\"" + Common.IIF(ShipCalcID == Common.ShippingCalculationEnum.CalculateShippingByWeightAndZone && (!this.IsAllDownloadComponents() && ShippingZip.Length == 0), "1", "0") + "\">"); //  MUST always do a rate get if they are entering the cart without a zip, this will force them back to the cart even if they select continue checkout, so they can see the shipping cost
			}
			tmpS.Append("<div align=\"left\">");
			tmpS.Append("<table border=\"0\" cellpadding=\"3\" cellspacing=\"0\" width=\"100%\">");
			//V3_9      

			bool AllowShipToDifferentThanBillTo = Common.AppConfigBool("AllowShipToDifferentThanBillTo");
      
			// only display address block if they are not anon and have addresses
			if(!IsAnon && (this._billingAddress.m_addressID != 0 || this._shippingAddress.m_addressID != 0))
			{
				tmpS.Append("<tr>\n");
				tmpS.Append("<td colspan=\"3\" valign=\"top\"><b>");
				if ( !AllowShipToDifferentThanBillTo)
				{
					tmpS.Append("Ship and "); // must be same
				}
				tmpS.Append("Bill To Address:&nbsp;&nbsp;&nbsp;&nbsp;</b>");
				if(!readOnlyVar)
				{
					tmpS.Append(String.Format("<img style=\"cursor:hand;\" src=\"skins/skin_" + _siteID.ToString() + "/images/change.gif\" border=\"0\" align=\"absmiddle\" onClick=\"self.location='selectaddress.aspx?AddressType=Billing&ReturnURL={0}'\">",HttpContext.Current.Server.UrlEncode(Common.PageInvocation())));
				}
				tmpS.Append("<br>");
				tmpS.Append(_billingAddress.DisplayHTML());
				tmpS.Append("</td>");
      
				tmpS.Append("<td colspan=\"3\" valign=\"top\">");
				if (AllowShipToDifferentThanBillTo)
				{
					tmpS.Append("<b>Ship To Address:&nbsp;&nbsp;&nbsp;&nbsp;</b>");
					if(!readOnlyVar)
					{
						tmpS.Append(String.Format("<img style=\"cursor:hand;\" src=\"skins/skin_" + _siteID.ToString() + "/images/change.gif\" border=\"0\" align=\"absmiddle\" onClick=\"self.location='selectaddress.aspx?AddressType=Shipping&ReturnURL={0}'\">\n",HttpContext.Current.Server.UrlEncode(Common.PageInvocation())));
					}
					tmpS.Append("<br>");
					tmpS.Append(_shippingAddress.DisplayHTML());
				}
				tmpS.Append("</td>");
				tmpS.Append("</tr>");

				tmpS.Append("<tr>");
				tmpS.Append("<td valign=\"top\" align=\"left\" colspan=\"6\" height=\"1\" style=\"padding:0;margin:0\">");
				tmpS.Append("<hr noshade size=\"2\" width=\"100%\">");
				tmpS.Append("</td>");
				tmpS.Append("</tr>");
			}
			tmpS.Append("<tr>");
			tmpS.Append("<td style=\"padding:0\" valign=\"bottom\" align=\"left\" width=\"50%\"><b>PRODUCT</b></td>");
			tmpS.Append("<td style=\"padding:0\" align=\"left\" valign=\"bottom\"><b>SKU</b></td>");
			tmpS.Append("<td style=\"padding:0\" align=\"center\" valign=\"bottom\"><b>" + Common.AppConfig("ColorOptionPrompt").ToUpper() + "</b></td>");
			tmpS.Append("<td style=\"padding:0\" align=\"center\" valign=\"bottom\"><b>" + Common.AppConfig("SizeOptionPrompt").ToUpper() + "</b></td>");
			tmpS.Append("<td style=\"padding:0\" align=\"center\" valign=\"bottom\"><b>QUANTITY</b></td>");
			tmpS.Append("<td style=\"padding:0\" align=\"right\" valign=\"bottom\"><b>PRICE</b></td>");
			tmpS.Append("</tr>");
			
			tmpS.Append("<tr>");
			tmpS.Append("<td valign=\"top\" align=\"left\" colspan=\"6\" height=\"3\">");
			tmpS.Append("<hr noshade size=\"2\" width=\"100%\">");

			//V3_9 Get the MicroPay Product ID
			int mpProductID = Common.GetMicroPayProductID();
			int mpVariantID = Common.GetProductsFirstVariantID(mpProductID);

			//V4_0 If A SKU=MICROPAY product exists make sure it is in the cart as the last item.
			if(!readOnlyVar && Common.AppConfigBool("MicroPay.Enabled"))
			{
				if (mpProductID != 0)
				{
					mpVariantID = Common.GetProductsFirstVariantID(mpProductID);
					this.AddItem(_thisCustomer, mpProductID, mpVariantID, 0, String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,CartTypeEnum.ShoppingCart,true);
					// Make sure Micropay is moved to the end of the list.
					foreach(CartItem item in this._cartItems)
					{
						if (item.productID == mpProductID)
						{
							this._cartItems.Remove(item);
							this._cartItems.Add(item);
							break;
						}
					}
				}
			}
			//V4_0

			tmpS.Append("</td>");
			tmpS.Append("</tr>");
			bool ShowLinkBack = Common.AppConfigBool("LinkToProductPageInCart") && !readOnlyVar;
			foreach(CartItem c in _cartItems)
			{

				//V3_9 If this is the MicroPay item display the Balance
				if(!readOnlyVar && Common.AppConfigBool("MicroPay.Enabled"))
				{
					if ((c.productID == mpProductID) && (c.variantID == mpVariantID))
					{
						tmpS.Append("<tr>");
						tmpS.Append("<td valign=\"top\" align=\"left\" colspan=\"6\" height=\"3\">");
						tmpS.Append("<hr noshade size=\"2\" width=\"100%\">");
						if(Common.AppConfig("Micropay.PointsPerDollar").Length == 0)
						{
							tmpS.Append("Your " + Common.AppConfig("Micropay.Prompt") + " Balance is: " + Localization.CurrencyStringForDisplay(Common.GetMicroPayBalance(_thisCustomer._customerID)));
						}
						else
						{
							tmpS.Append("Your " + Common.AppConfig("Micropay.Prompt") + " Balance is: " + ((int)Common.GetMicroPayBalance(_thisCustomer._customerID)).ToString());
						}
						tmpS.Append("</td></tr>");
					}
				}
				
				bool showIt = true;

				if(c.SKU == "MICROPAY")
				{
					if(readOnlyVar || !Common.AppConfigBool("MicroPay.Enabled") || Common.AppConfigBool("Micropay.HideOnCartPage"))
					{
						showIt = false;
					}
				}

				if(showIt)
				{
					
					
					tmpS.Append("<tr>");
					tmpS.Append("<td  valign=\"top\" align=\"left\">");
					if(ShowLinkBack)
					{
						tmpS.Append("<a href=\"" + SE.MakeProductLink(c.productID,"") + "\">");
					}
					tmpS.Append(c.productName);
					if(c.textOption.Length != 0)
					{
						tmpS.Append("<br>(Text: " + c.textOption + ")");
					}
					if(ShowLinkBack)
					{
						tmpS.Append("</a>");
					}

					string AutoShipString = Common.IIF(c.isRecurring , "<br><font size=\"1\">[Auto-Ship]</font>" , "");

					if(Common.IsAKit(c.productID))
					{
						if(!readOnlyVar)
						{
							tmpS.Append(":&nbsp;&nbsp;<a href=\"ShoppingCart_change.aspx?recid=" + c.ShoppingCartRecordID.ToString() + "\"><img src=\"skins/Skin_" + _siteID.ToString() + "/images/edit2.gif\" align=\"absmiddle\" border=\"0\" alt=\"Change This Kit\"></a>&nbsp;");
						}

						tmpS.Append(AutoShipString);
          
						IDataReader rsx = DB.GetRS("select kititem.name, kitcart.quantity from kitcart  " + DB.GetNoLock() + " inner join kititem  " + DB.GetNoLock() + " on kitcart.kititemid=kititem.kititemid where ShoppingCartrecid=" + c.ShoppingCartRecordID.ToString());
						String tmp = String.Empty;
						while(rsx.Read())
						{
							tmp += "&nbsp;&nbsp;-&nbsp;(" + DB.RSFieldInt(rsx,"Quantity").ToString() + ")&nbsp;" + DB.RSField(rsx,"Name") + "<br>";
						}
						rsx.Close();
						tmpS.Append("<br>");
						tmpS.Append(tmp);
					}
					else
						if(Common.IsAPack(c.productID))
					{
						CustomCart ccart = new CustomCart(_thisCustomer._customerID,c.ShoppingCartRecordID,c.productID,SiteID);
						if(!readOnlyVar)
						{
							tmpS.Append("&nbsp;&nbsp;<a href=\"ShoppingCart_change.aspx?recid=" + c.ShoppingCartRecordID.ToString() + "\"><img src=\"skins/Skin_" + _siteID.ToString() + "/images/edit2.gif\" align=\"absmiddle\" border=\"0\" alt=\"Change This Pack\"></a>&nbsp;");
						}
          
						tmpS.Append(AutoShipString);
          
						String tmp = ccart.GetContents(false,"<br>");
						tmpS.Append("<br>");
						tmpS.Append(tmp);
					}
					else
					{
						tmpS.Append(AutoShipString);
					}
					tmpS.Append("</td>");
					tmpS.Append("<td align=\"left\" valign=\"top\">" + c.SKU + "</td>");
					tmpS.Append("<td align=\"center\" valign=\"top\">" + (Common.IIF(c.chosenColor.Length == 0 , "--" , c.chosenColor)) + "</td>");
					tmpS.Append("<td align=\"center\" valign=\"top\">" + (Common.IIF(c.chosenSize.Length == 0 , "--" , c.chosenSize)) + "</td>");
					tmpS.Append("<td align=\"center\" valign=\"top\">");
					if(!readOnlyVar)
					{
						if(c.restrictedQuantities.Length == 0)
						{
							tmpS.Append("<input type=\"text\" id=\"Quantity_" + c.ShoppingCartRecordID.ToString() + "\" name=\"Quantity_" + c.ShoppingCartRecordID.ToString() + "\" size=\"4\" value=\"" + c.quantity.ToString() + "\" maxlength=\"4\">");
							if(Common.AppConfigBool("ShowCartDeleteItemButton"))
							{
								//V3_9  Only display delete button if quantity is > 0 (for MicroPay)
								if (c.quantity > 0)
								{
									tmpS.Append("&nbsp;<img style=\"cursor:hand;\" onClick=\"document.CartForm.Quantity_" + c.ShoppingCartRecordID.ToString() + ".value='0';document.CartForm.submit();\" align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/delete.gif\" width=\"32\" height=\"16\" border=\"0\">");
								}
								else
								{
									tmpS.Append("&nbsp;<img align=\"absmiddle\" src=\"images/spacer.gif\" width=\"32\" height=\"16\" border=\"0\">");
								}
								//V3_9
							}
						}
						else
						{
							tmpS.Append("<select id=\"Quantity_" + c.ShoppingCartRecordID.ToString() + "\" name=\"Quantity_" + c.ShoppingCartRecordID.ToString() + "\" size=\"1\">");
							if(Common.AppConfigBool("ShowCartDeleteItemButton"))
							{
								tmpS.Append("<option value=\"0\">DELETE</option>");
							}
							foreach(String s in c.restrictedQuantities.Split(','))
							{
								if(s.Trim().Length != 0)
								{
									int Qx = Localization.ParseUSInt(s.Trim());
									if(Qx != 0) // no need to show "0" on cart page, as the DELETE option above covers this!
									{
										tmpS.Append("<option value=\"" + Qx.ToString() + "\"" + Common.IIF(c.quantity == Qx, " selected ","") + ">" + Qx.ToString() + "</option>");
									}
								}
							}
							tmpS.Append("</select>&nbsp;");
						}
					}
					else
					{
						tmpS.Append(c.quantity);
					}
					tmpS.Append("</td>");
					tmpS.Append("<td align=\"right\" valign=\"top\">");
					int Q = c.quantity;
					decimal PR = c.price * Q;
					int ActiveDID = 0;
					Single DIDPercent = 0.0F;
					if(Common.CustomerLevelAllowsQuantityDiscounts(_thisCustomer._customerLevelID))
					{
						ActiveDID = Common.LookupActiveVariantQuantityDiscountID(c.variantID);
						DIDPercent = Common.GetDIDPercent(ActiveDID,Q);
						if(ActiveDID != 0 && DIDPercent != 0.0F)
						{
							PR = (decimal)(1.0-(DIDPercent/100.0)) * PR;
						}
					}
					tmpS.Append(Localization.CurrencyStringForDisplay(PR));
					if(Common.CustomerLevelAllowsQuantityDiscounts(_thisCustomer._customerLevelID))
					{
						if(ActiveDID != 0 && DIDPercent != 0.0)
						{
							tmpS.Append(" <small>(" + DIDPercent.ToString() + "% Quan Dis)</small>");
						}
					}
					tmpS.Append("</td>");
					tmpS.Append("</tr>");
				}
			}
			tmpS.Append("<tr>");
			tmpS.Append("<td valign=\"top\" align=\"left\" colspan=\"6\" height=\"3\">");
			tmpS.Append("<hr noshade width=\"100%\" size=\"2\">");
			tmpS.Append("</td>");
			tmpS.Append("</tr>");
			if(!readOnlyVar)
			{

				tmpS.Append("<tr><td valign=\"top\" align=\"left\" colspan=\"6\">");
				DataSet ds = DB.GetDS("Select * from orderoption  " + DB.GetNoLock() + " order by displayorder",true,System.DateTime.Now.AddMinutes(Common.CacheDurationMinutes()));
				if(ds.Tables[0].Rows.Count > 0)
				{
					tmpS.Append("<div align=\"center\" width=\"50%\">");
					
					tmpS.Append("<table width=\"50%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
					tmpS.Append("<tr>");
					tmpS.Append("<td align=\"left\"><span class=\"OrderOptionsTitle\">The Following Order Options Are Available:</span></td>");
					tmpS.Append("<td align=\"center\"><span class=\"OrderOptionsRowHeader\">Cost</span></td>");
					tmpS.Append("<td width=\"25\" align=\"center\"><span class=\"OrderOptionsRowHeader\">Select</span></td>");
					tmpS.Append("</tr>");
					foreach(DataRow row in ds.Tables[0].Rows)
					{
						tmpS.Append("<tr>");
						tmpS.Append("<td align=\"left\">");
						String ImgUrl = Common.LookupImage("OrderOption",DB.RowFieldInt(row,"OrderOptionID"),"icon",SiteID);
						if(ImgUrl.Length != 0)
						{
							tmpS.Append("<img src=\"" + ImgUrl + "\" border=\"0\" align=\"absmiddle\">&nbsp;");
						}
						tmpS.Append("<span class=\"OrderOptionsName\">" + DB.RowField(row,"Name") + Common.IIF(DB.RowField(row,"Description").Length != 0 , "&nbsp;<img style=\"cursor: hand;\" alt=\"More Info On This Option\" onClick=\"popuporderoptionwh('Order Option " + DB.RowFieldInt(row,"OrderOptionID").ToString() + "'," + DB.RowFieldInt(row,"OrderOptionID").ToString() + ",650,550,'yes');\" src=\"skins/skin_" + _siteID.ToString() + "/images/helpcircle.gif\" align=\"absmiddle\"></a>", "") + "</span></td>");
						tmpS.Append("<td align=\"center\"><span class=\"OrderOptionsPrice\">" + Localization.CurrencyStringForDisplay(DB.RowFieldDecimal(row,"Cost")) + "</span></td>");
						tmpS.Append("<td align=\"center\"><input type=checkbox name=\"OrderOptions\" value=\"" + DB.RowFieldInt(row,"OrderOptionID").ToString() + "\" " + Common.IIF(OptionIsSelected(DB.RowFieldInt(row,"OrderOptionID"),_orderOptions)," checked ","") + "></td>");
						tmpS.Append("</tr>");
					}
					tmpS.Append("</table>");
					tmpS.Append("</div>");
					tmpS.Append("<p>&nbsp;</p>");
					tmpS.Append("</td></tr>");
				}
				ds.Dispose();
				
				tmpS.Append("<tr>");
				tmpS.Append("<td valign=\"top\" align=\"left\" colspan=\"4\">");
				bool OkForCoupons = false;
				if(!Common.AppConfigBool("DisallowCoupons"))
				{
					if(_thisCustomer._customerLevelID == 0)
					{
						OkForCoupons = true;
					}
					else
					{
						if(Common.CustomerLevelAllowsCoupons(_thisCustomer._customerLevelID))
						{
							OkForCoupons = true;
						}
					}
				}
				if(OkForCoupons)
				{
					tmpS.Append("        Enter any special offer/discount coupon code here: <input type=\"text\" id=\"CouponCode\" name=\"CouponCode\" size=\"10\" value=\"" + HttpContext.Current.Server.HtmlEncode(_coupon.code) + "\" maxlength=\"20\">");
				}
				else
				{
					tmpS.Append("<input type=\"hidden\" id=\"CouponCode\" name=\"CouponCode\" value=\"\">");
				}
				tmpS.Append("</td>");
				tmpS.Append("<td align=\"right\" valign=\"top\" colspan=\"2\">");
				tmpS.Append("<input type=\"submit\" onClick=\"document.CartForm.ContinueCheckout.value='0';document.CartForm.CheckN.value = '0';\" name=\"btnSubmit\" value=\"&nbsp;&nbsp;Update Prices&nbsp;&nbsp;\">");
				tmpS.Append("</td>");
				tmpS.Append("</tr>");
			}
			else
			{
				// just write selected options out:
				DataSet ds = DB.GetDS("Select * from orderoption  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddMinutes(Common.CacheDurationMinutes()));
				if(ds.Tables[0].Rows.Count > 0 && _orderOptions.Length != 0)
				{
					tmpS.Append("<tr><td valign=\"top\" align=\"left\" colspan=\"6\">");
					tmpS.Append("<div align=\"center\" width=\"50%\">");
					
					tmpS.Append("<table width=\"50%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
					tmpS.Append("<tr>");
					tmpS.Append("<td align=\"left\"><span class=\"OrderOptionsTitle\">You Have Selected The Following Order Options:</span></td>");
					tmpS.Append("<td align=\"center\"><span class=\"OrderOptionsRowHeader\">Cost</span></td>");
					tmpS.Append("<td align=\"center\"><span class=\"OrderOptionsRowHeader\">Selected</span></td>");
					tmpS.Append("</tr>");
					foreach(DataRow row in ds.Tables[0].Rows)
					{
						if(OptionIsSelected(DB.RowFieldInt(row,"OrderOptionID"),_orderOptions))
						{
							tmpS.Append("<tr>");
							tmpS.Append("<td align=\"left\">");
							String ImgUrl = Common.LookupImage("OrderOption",DB.RowFieldInt(row,"OrderOptionID"),"icon",SiteID);
							if(ImgUrl.Length != 0)
							{
								tmpS.Append("<img src=\"" + ImgUrl + "\" border=\"0\" align=\"absmiddle\">&nbsp;");
							}
							tmpS.Append("<span class=\"OrderOptionsName\">" + DB.RowField(row,"Name") + Common.IIF(DB.RowField(row,"Description").Length != 0 , "&nbsp;<img style=\"cursor: hand;\" alt=\"More Info On This Option\" onClick=\"popuporderoptionwh('Order Option " + DB.RowFieldInt(row,"OrderOptionID").ToString() + "'," + DB.RowFieldInt(row,"OrderOptionID").ToString() + ",650,550,'yes');\" src=\"skins/skin_" + _siteID.ToString() + "/images/helpcircle.gif\" align=\"absmiddle\"></a>", "") + "</span></td>");
							tmpS.Append("<td align=\"center\"><span class=\"OrderOptionsPrice\">" + Localization.CurrencyStringForDisplay(DB.RowFieldDecimal(row,"Cost")) + "</span></td>");
							tmpS.Append("<td align=\"center\"><img src=\"skins/skin_" + _siteID.ToString() + "/images/selected.gif\" align=\"absmiddle\"></td>");
							tmpS.Append("</tr>");
						}
					}
					tmpS.Append("</table>");
					tmpS.Append("</div>");
					tmpS.Append("<p>&nbsp;</p>");
					tmpS.Append("</td></tr>");
				}
				ds.Dispose();
			}
			tmpS.Append("<tr>");
			tmpS.Append("<td valign=\"top\" align=\"left\" colspan=\"6\" height=\"15\">&nbsp;</td>");
			tmpS.Append("</tr>");
			
      
			//Subtotal row
			tmpS.Append("<tr>");
			tmpS.Append("<td valign=\"top\" align=\"left\" rowspan=\"2\" colspan=\"4\">&nbsp;</td>");
			tmpS.Append("<td align=\"right\" valign=\"top\" >Subtotal");
			if(HasCoupon())
			{
				if(CouponIsValid() == "OK")
				{
					if(_thisCustomer._customerLevelID == 0)
					{
						tmpS.Append(" (Includes Discounts)");
					}
					else
					{
						tmpS.Append(" (Includes all " + Common.GetCustomerLevelName(_thisCustomer._customerLevelID) + " Discounts)");
					}
				}
			}
			tmpS.Append(":</td>");
			tmpS.Append("<td align=\"right\" valign=\"top\">" + Localization.CurrencyStringForDisplay(SubTotal(true,false,true)) + "</td>");
			tmpS.Append("</tr>");

			//Tax Row
			tmpS.Append("<tr>");
			tmpS.Append("<td align=\"right\" valign=\"top\">Tax");
			if(_stateTaxRate != 0.0F && TaxTotal(true) > System.Decimal.Zero)
			{
				tmpS.Append("(");
				if(_stateTaxRate != 0.0F)
				{
					tmpS.Append(Localization.SingleStringForDB(_stateTaxRate) + "%");
				}
				tmpS.Append(")");
			}
			tmpS.Append(":</td>");
			tmpS.Append("<td align=\"right\" valign=\"top\">" + Localization.CurrencyStringForDisplay(TaxTotal(true)) + "</td>");
			tmpS.Append("</tr>");
			tmpS.Append("<tr>");

			//Shipping row
			tmpS.Append("<td align=\"right\" valign=\"top\" colspan=\"5\">");

			bool AllShippingRequiredElementsArePresent = (ShippingCity.Length != 0 && ShippingState.Length != 0 && ShippingZip.Length != 0 && ShippingCountry.Length != 0);
			String RateSelect = String.Empty;

			decimal FreeShippingThreshold = Common.AppConfigUSDecimal("FreeShippingThreshold");
			Decimal sum = SubTotal(true,false,false);

			if(!readOnlyVar)
			{
				if(this.IsAllDownloadComponents())
				{
					tmpS.Append("FREE SHIPPING/HANDLING (Download):");
					// preserve this info just in case:
					tmpS.Append("<input type=\"hidden\" id=\"ShippingZip\" name=\"ShippingZip\" value=\"" + ShippingZip + "\">");
					tmpS.Append("<input type=\"hidden\" id=\"ShippingState\" name=\"ShippingState\" value=\"" + ShippingState + "\">");
					tmpS.Append("<input type=\"hidden\" id=\"ShippingCity\" name=\"ShippingCity\" value=\"" + ShippingCity + "\">");
					tmpS.Append("<input type=\"hidden\" id=\"ShippingCountry\" name=\"ShippingCountry\" value=\"" + ShippingCountry + "\">");
					tmpS.Append("<input type=\"hidden\" id=\"ShippingMethodID\" name=\"ShippingMethodID\" value=\"" + Common.AppConfigUSInt("ShippingMethodIDIfFreeShippingIsOn").ToString() + "\">");
				}
				else if(FreeShippingThreshold != System.Decimal.Zero && sum >= FreeShippingThreshold)
				{
					tmpS.Append("FREE SHIPPING/HANDLING:");
					// preserve this info just in case:
					tmpS.Append("<input type=\"hidden\" id=\"ShippingZip\" name=\"ShippingZip\" value=\"" + ShippingZip + "\">");
					tmpS.Append("<input type=\"hidden\" id=\"ShippingState\" name=\"ShippingState\" value=\"" + ShippingState + "\">");
					tmpS.Append("<input type=\"hidden\" id=\"ShippingCity\" name=\"ShippingCity\" value=\"" + ShippingCity + "\">");
					tmpS.Append("<input type=\"hidden\" id=\"ShippingCountry\" name=\"ShippingCountry\" value=\"" + ShippingCountry + "\">");
					tmpS.Append("<input type=\"hidden\" id=\"ShippingMethodID\" name=\"ShippingMethodID\" value=\"" + Common.AppConfigUSInt("ShippingMethodIDIfFreeShippingIsOn").ToString() + "\">");
				}
				else if(ShipCalcID == Common.ShippingCalculationEnum.AllOrdersHaveFreeShipping)
				{
					tmpS.Append("FREE SHIPPING/HANDLING (" + Common.GetShippingMethodName(Common.AppConfigUSInt("ShippingMethodIDIfFreeShippingIsOn")) + ")");
					// preserve this info just in case:
					tmpS.Append("<input type=\"hidden\" id=\"ShippingZip\" name=\"ShippingZip\" value=\"" + ShippingZip + "\">");
					tmpS.Append("<input type=\"hidden\" id=\"ShippingState\" name=\"ShippingState\" value=\"" + ShippingState + "\">");
					tmpS.Append("<input type=\"hidden\" id=\"ShippingCity\" name=\"ShippingCity\" value=\"" + ShippingCity + "\">");
					tmpS.Append("<input type=\"hidden\" id=\"ShippingCountry\" name=\"ShippingCountry\" value=\"" + ShippingCountry + "\">");
					tmpS.Append("<input type=\"hidden\" id=\"ShippingMethodID\" name=\"ShippingMethodID\" value=\"" + Common.AppConfigUSInt("ShippingMethodIDIfFreeShippingIsOn").ToString() + "\">");
				}
				else if(ShipCalcID == Common.ShippingCalculationEnum.UseRealTimeRates)
				{
					// -----------------------------------------------------------------------------------
					//
					// RT shipping logic: this is the UGLY part of this routine. modify with extreme care
					//
					// -----------------------------------------------------------------------------------

					if(AllShippingRequiredElementsArePresent)
					{
						// this is ONLY valid if they have entered SOMETHING already:
						RateSelect = this.RecheckShippingRates();
						tmpS.Append("<b>Available&nbsp;Shipping&nbsp;Methods:</b><br>");
						tmpS.Append(RateSelect + "<br>");
					}

					if(this._shippingAddress.m_addressID == 0)
					{
						tmpS.Append(String.Format("<table border=\"0\" cellspacing=\"0\" cellpadding=\"2\" style=\"{0}\" >\n",Common.AppConfig("BoxFrameStyle")));
						tmpS.Append("<tr><td align=\"center\" colspan=\"2\">\n");
						if (!AllShippingRequiredElementsArePresent)
						{
							tmpS.Append("<b>Please enter your Ship To Location to get shipping cost:</b>");
						}
						else
						{
							tmpS.Append("<b>Update your shipping address below if required:</b>");
						}
						tmpS.Append("</td></tr>");
        
						//ShipCity 
						tmpS.Append("<tr><td align=\"right\">\n");
						tmpS.Append("City: </td><td><input type=\"text\" size=\"34\" maxlength=\"50\" style=\"font-size:9px;width:100%\" id=\"ShippingCity\" name=\"ShippingCity\" value=\"" + ShippingCity + "\"><input type=\"hidden\" name=\"ShippingCity_vldt\" value=\"[req][blankalert=Please enter your ship to city name]\">\n");
						tmpS.Append("</td></tr>");
						tmpS.Append("</td></tr>");
        
						//ShipState
						tmpS.Append("<tr><td align=\"right\">\n");
						tmpS.Append("State/Province: </td><td><select size=\"1\" id=\"ShipingState\" name=\"ShippingState\" style=\"font-size:9px;width:100%\">");
						tmpS.Append("        <option value=\"\"" + Common.IIF((ShippingState.Length == 0)," selected",String.Empty) + " >SELECT ONE</option>");
						DataSet dsstate = DB.GetDS("select * from state  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddMinutes(Common.CacheDurationMinutes()));
						foreach(DataRow row in dsstate.Tables[0].Rows)
						{
							tmpS.Append("      <option value=\"" + DB.RowField(row,"Abbreviation") + "\"" + Common.IIF(DB.RowField(row,"Abbreviation") == ShippingState," selected",String.Empty) + ">" + DB.RowField(row,"Name") + "</option>");
						}
						dsstate.Dispose();
						tmpS.Append("        </select>");
						tmpS.Append("</td></tr>");
        
						//ShipZip
						tmpS.Append("<tr><td align=\"right\">\n");
						tmpS.Append("Zip/Postal&nbsp;Code: </td><td><input type=\"text\" size=\"34\" maxlength=\"10\" style=\"font-size:9px;width:100%\" id=\"ShippingZip\" name=\"ShippingZip\" value=\"" + ShippingZip + "\"><input type=\"hidden\" name=\"ShippingZip_vldt\" value=\"[req][blankalert=Please enter your ship to zipcode]\">\n");
						tmpS.Append("</td></tr>");
        
						//ShipCountry
						tmpS.Append("<tr><td align=\"right\">\n");
						tmpS.Append("Country: </td><td><select id=\"ShippingCountry\" name=\"ShippingCountry\" size=\"1\" style=\"font-size:9px;\">");
						DataSet dscountry = DB.GetDS("select * from country  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddMinutes(Common.CacheDurationMinutes()));
						foreach(DataRow row in dscountry.Tables[0].Rows)
						{
							tmpS.Append("      <option value=\"" + DB.RowField(row,"Name") + "\"" + Common.IIF(DB.RowField(row,"Name") == ShippingCountry," selected",String.Empty) + ">" + DB.RowField(row,"Name") + "</option>");
						}
						dscountry.Dispose();
						tmpS.Append("        </select>");
						tmpS.Append("</td></tr>");
        
						tmpS.Append("<tr><td align=\"center\" colspan=\"2\">\n");
						tmpS.Append("<input type=\"submit\" onClick=\"document.CartForm.GetRTShipping.value='1';document.CartForm.ContinueCheckout.value='0';document.CartForm.CheckN.value = '0';\" name=\"btnSubmit\" value=\"Get Shipping Rates\">");
						tmpS.Append("<br><br><input type=\"hidden\" id=\"ShippingMethodID\" name=\"ShippingMethodID\" value=\"0\">"); // N/A Yet
						tmpS.Append("</td></tr>");
						tmpS.Append("</td></tr></table>\n");
					}
				}
				else if(ShipCalcID == Common.ShippingCalculationEnum.CalculateShippingByWeightAndZone)
				{
					if(_shippingAddress.AddressID==0 || ShippingZip.Length == 0)
					{
						// need to get zip:
						tmpS.Append("Please enter your ZipCode to get shipping cost: <br><input type=\"text\" size=\"10\" maxlength=\"10\" id=\"ShippingZip\" name=\"ShippingZip\" value=\"" + ShippingZip + "\"><input type=\"hidden\" name=\"ShippingZip_vldt\" value=\"[req][blankalert=Please enter your ship to zipcode]\">\n");
						tmpS.Append("<input type=\"submit\" onClick=\"document.CartForm.GetZoneShipping.value='1';document.CartForm.ContinueCheckout.value='0';document.CartForm.CheckN.value = '0';\" name=\"btnSubmit\" value=\"Calculate Shipping\">");
						tmpS.Append("<input type=\"hidden\" id=\"ShippingMethodID\" name=\"ShippingMethodID\" value=\"0\">"); // N/A (store chooses)
					}
					else
					{
						String ZoneName = Common.GetZoneName(this._shippingZoneID);

						tmpS.Append("Shipping/Handling To Zone (" + Common.IIF(ZoneName.Length == 0, "Unknown Zone", ZoneName) + ")");
						if(Common.AppConfigBool("AllowZipChangeAgainInCart"))
						{
							tmpS.Append("<br>Your ZipCode: <input type=\"text\" size=\"10\" maxlength=\"10\" id=\"ShippingZip\" name=\"ShippingZip\" value=\"" + ShippingZip + "\"><input type=\"hidden\" name=\"ShippingZip_vldt\" value=\"[req][blankalert=Please enter your ship to zipcode]\">\n");
						}
						else
						{
							tmpS.Append("<input type=\"hidden\" id=\"ShippingZip\" name=\"ShippingZip\" value=\"" + ShippingZip + "\">");
						}
						tmpS.Append("<input type=\"hidden\" id=\"ShippingMethodID\" name=\"ShippingMethodID\" value=\"0\">"); // N/A (store chooses)
					}
				}
				else
				{
					tmpS.Append("<input type=\"hidden\" id=\"ShippingZip\" name=\"ShippingZip\" value=\"" + ShippingZip + "\">");
					tmpS.Append("<input type=\"hidden\" id=\"ShippingState\" name=\"ShippingState\" value=\"" + ShippingState + "\">");
					tmpS.Append("<input type=\"hidden\" id=\"ShippingCity\" name=\"ShippingCity\" value=\"" + ShippingCity + "\">");
					tmpS.Append("<input type=\"hidden\" id=\"ShippingCountry\" name=\"ShippingCountry\" value=\"" + ShippingCountry + "\">");
					tmpS.Append("Shipping Method: <select size=\"1\" onChange=\"document.CartForm.submit();\" id=\"ShippingMethodID\" name=\"ShippingMethodID\">\n");
					IDataReader rsst = DB.GetRS("select * from ShippingMethod  " + DB.GetNoLock() + " where deleted=0 and name not like " + DB.SQuote("Real Time%") + " order by displayorder,name");
					while(rsst.Read())
					{
						tmpS.Append("<option value=\"" + DB.RSFieldInt(rsst,"ShippingMethodID").ToString() + "\" " + Common.IIF(_shippingMethodID == DB.RSFieldInt(rsst,"ShippingMethodID") , " selected " , "") + ">" + DB.RSField(rsst,"Name") + "</option>");
					}
					rsst.Close();
					tmpS.Append("</select>\n");
				}
			}
			else
			{
				if(this.IsAllDownloadComponents())
				{
					tmpS.Append("FREE SHIPPING/HANDLING (Download):");
				}
				else if(FreeShippingThreshold != System.Decimal.Zero && sum >= FreeShippingThreshold)
				{
					tmpS.Append("FREE SHIPPING/HANDLING (" + Common.GetShippingMethodName(Common.AppConfigUSInt("ShippingMethodIDIfFreeShippingIsOn")) + ")");
				}
				else if(ShipCalcID == Common.ShippingCalculationEnum.AllOrdersHaveFreeShipping)
				{
					tmpS.Append("FREE SHIPPING/HANDLING (" + Common.GetShippingMethodName(Common.AppConfigUSInt("ShippingMethodIDIfFreeShippingIsOn")) + ")");
				}
				else if(ShipCalcID == Common.ShippingCalculationEnum.UseRealTimeRates)
				{
					int ix = _shippingMethod.IndexOf("|");
					String dispS = _shippingMethod;
					if(ix != -1)
					{
						dispS = _shippingMethod.Substring(0,ix);
					}
					tmpS.Append("Shipping/Handling (" + dispS + "):");
				}
				else
				{
					tmpS.Append("Shipping/Handling:");
				}
			}

			tmpS.Append("</td>");

			tmpS.Append("<td align=\"right\" valign=\"top\">");
			if(readOnlyVar)
			{
				tmpS.Append(Localization.CurrencyStringForDisplay(ShippingTotal(true)));
			}
			else
			{
				if(IsAllDownloadComponents())
				{
					tmpS.Append(Localization.CurrencyStringForDisplay(ShippingTotal(true)));
				}
				else if(ShipCalcID == Common.ShippingCalculationEnum.UseRealTimeRates)
				{
					// only show "rate" if we have a rate in the select box AND we know customer has entered all required shipping info:
					if(AllShippingRequiredElementsArePresent && RateSelect.IndexOf("<select") != -1)
					{
						tmpS.Append(Localization.CurrencyStringForDisplay(ShippingTotal(true)));
					}
					else
					{
						if(this._shippingAddress.m_addressID == 0)
						{
							tmpS.Append("Need Ship To Location");
						}
						else
						{
							tmpS.Append("Please change your shipping address to provide a valid address");
						}
					}
				}
				else if(ShipCalcID == Common.ShippingCalculationEnum.CalculateShippingByWeightAndZone )
				{
					if(ShippingZip.Length == 0)
					{
						tmpS.Append("Need Ship To Location");
					}
					else
					{
						tmpS.Append(Localization.CurrencyStringForDisplay(ShippingTotal(true)));
					}
				}
				else
				{
					tmpS.Append(Localization.CurrencyStringForDisplay(ShippingTotal(true)));
				}
			}
			tmpS.Append("</td>");
			tmpS.Append("</tr>");

      
			tmpS.Append("<tr>");
			tmpS.Append("<td colspan=\"4\">&nbsp;</td>");
			tmpS.Append("<td valign=\"top\" align=\"right\" colspan=\"2\">");
			tmpS.Append("<hr noshade size=\"1\" width=\"100%\"></td>");
			tmpS.Append("</tr>");
			tmpS.Append("<tr>");
			tmpS.Append("<td colspan=\"4\">&nbsp;</td>");
			tmpS.Append("<td align=\"right\" valign=\"top\" >Total:</td>");
			tmpS.Append("<td align=\"right\" valign=\"top\">" + Localization.CurrencyStringForDisplay(Total(true)) + "</td>");
			tmpS.Append("</tr>");
			tmpS.Append("<tr>");
			tmpS.Append("<td valign=\"top\" align=\"left\" colspan=\"6\" height=\"10\"></td>");
			tmpS.Append("</tr>");
			if(!readOnlyVar)
			{
				String PaymentMethods = Common.AppConfig("PaymentMethods");

				//V3_9 Add MicroPay option if Micropay balance is greater than the OrderTotal and your not buying MicroPay $

				if(Common.AppConfigBool("MicroPay.Enabled"))
				{
					if ((mpProductID != 0) && (Common.GetMicroPayBalance(_thisCustomer._customerID) > Total(true)) && (this.GetQuantityInCart(mpProductID,mpVariantID)==0))
					{
						PaymentMethods = Common.AppConfig("Micropay.Prompt") + Common.IIF(PaymentMethods.Length == 0 , "", ",") + PaymentMethods;
					}
				}
				//V3_9
				if(PaymentMethods.Length == 0)
				{
					PaymentMethods = "Credit Card"; // default to this type of payment, in case no payment methods are found!
				}
				String[] pm = PaymentMethods.Split(',');
				int n_pm = pm.Length;
				for(int i = pm.GetLowerBound(0); i <= pm.GetUpperBound(0); i++)
				{
					pm[i] = pm[i].Trim(); // for safety
				}
				if((ShipCalcID == Common.ShippingCalculationEnum.UseRealTimeRates || ShipCalcID == Common.ShippingCalculationEnum.CalculateShippingByWeightAndZone) && (!this.IsAllDownloadComponents() && (_shippingMethod.Length == 0))) 
				{
					// don't allow proceed until zip is entered
				}
				else
				{
					tmpS.Append("<tr>");
					tmpS.Append("<td valign=\"top\" colspan=\"4\" align=\"left\"><a href=\"" + BACKURL + "\">Continue Shopping...</a></td>");
					tmpS.Append("<td align=\"right\" valign=\"top\" colspan=\"2\">");
					if(n_pm > 1)
					{
						bool hasRecurring = this.ContainsRecurring();
						tmpS.Append("Method Of Payment: <select size=\"1\" id=\"PaymentMethod\" name=\"PaymentMethod\">\n");
						for(int i = pm.GetLowerBound(0); i <= pm.GetUpperBound(0); i++)
						{
							bool okToAdd = true;
							if(hasRecurring && ((pm[i].ToUpper() != "CREDIT CARD") && (pm[i].ToUpper() != "ECHECK") && (pm[i].ToUpper() != "MICROPAY")))
							{
								okToAdd = false; // for orders with recurring items, we can ONLY have credit card!
							}
							
							if(pm[i].ToUpper() == "PURCHASE ORDER")
							{
								okToAdd = Common.CustomerLevelAllowsPO(_thisCustomer._customerLevelID);
							}

							if(okToAdd)
							{
								string selected = String.Empty;
								if ( pm[0].ToUpper() != "MICROPAY")
								{
									selected = Common.IIF(this._billingAddress.PaymentMethod.ToUpper().Replace(" ","") == pm[i].ToUpper().Replace(" ","") , "selected" , String.Empty);
								}
								tmpS.Append(String.Format("<option value=\"{0}\" {1} >{0}</option>",pm[i],selected));
							}
						}
						tmpS.Append("</select><br><br>\n");
					}
					else
					{
						tmpS.Append("<input type=\"hidden\" id=\"PaymentMethod\" name=\"PaymentMethod\" value=\"" + PaymentMethods + "\">");
					}
					tmpS.Append("<input type=\"submit\" name=\"btnSubmit\" onClick=\"document.CartForm.ContinueCheckout.value='1';\" value=\"" + Common.IIF(n_pm == 1 , "Checkout via " + PaymentMethods , "Continue Checkout") + "\">");
					tmpS.Append("</td>");
					tmpS.Append("</tr>");
				}
			}
			if(!Common.AppConfigBool("DisallowOrderNotes"))
			{
				if(!readOnlyVar)
				{
					tmpS.Append("<tr><td colspan=\"6\" align=\"left\">\n");
					tmpS.Append("<b>Enter any Special Instructions or Notes About The Order:</b><br><textarea id=\"OrderNotes\" name=\"OrderNotes\" cols=\"50\" rows=\"4\" style=\"width: 100%\">" + HttpContext.Current.Server.HtmlEncode(_orderNotes) + "</textarea>");
					tmpS.Append("</td></tr>\n");
				}
				else
				{
					if(_orderNotes.Length != 0)
					{
						tmpS.Append("<tr><td colspan=\"6\" align=\"left\">\n");
						tmpS.Append("<b>Special Instructions:</b><br>" + HttpContext.Current.Server.HtmlEncode(_orderNotes));
						tmpS.Append("</td></tr>\n");
					}
				}
			}
			tmpS.Append("</table>");
			tmpS.Append("</div>");
			if(!readOnlyVar)
			{
				tmpS.Append("</form>");
			}

			return tmpS.ToString();
		}

		// calls RT service, checks rates, updates customer table, and _shippingMethod with new rate cost based on current cart contents!
		public String RecheckShippingRates()
		{
			String CustSvc = String.Empty;
			int idx = _shippingMethod.IndexOf("|");
			if(idx == -1)
			{
				CustSvc = _shippingMethod;
			}
			else
			{
				try
				{
					CustSvc = _shippingMethod.Substring(0,idx);
				}
				catch {}
			}

			String RateList = GetRates(Common.AppConfig("ShippingHandlingExtraFee"));

			string[] strs = RateList.Split(',');
			if ((strs[0] == "0") && (strs.Length <=1))
			{
				return Common.AppConfig("CallForShippingPrompt"); 
			}
			String RateErrors = String.Empty;
      
			StringBuilder RateSelect = new StringBuilder(5000);
			RateSelect.Append("<select onChange=\"document.CartForm.submit();\"  SIZE=\"1\" NAME=\"ShippingMethod\" CLASS=\"ShippingMethod\">");
			// try to select the customer's prior rate selection (may not be valid anymore though):
      
			string weightMessage = Common.AppConfig("CallForShippingPrompt");
			string weightPrompt = String.Empty;
			
			int Count = 0;
			foreach(String s in strs)
			{
				String s2 = s.Trim();
				if(s2.Length != 0 && s2.IndexOf("|") != -1)
				{
					Count++;
					String ThisSvc =
						s2.Substring(0,s2.IndexOf("|"));
					bool thisSelected = false;
					if(CustSvc == ThisSvc)
					{
						thisSelected = true;
					}
					else if(CustSvc.Length == 0 && Count == 1)
					{
						thisSelected = true;
					}
					RateSelect.Append("<OPTION VALUE=\"" + s2 + "\" " + Common.IIF(thisSelected," selected","") + ">" + s2.Replace("|"," $") + " </OPTION>");
				
					if(thisSelected)
					{
						this._shippingAddress.ShippingMethod = s2;
						this._shippingAddress.UpdateDB();
						_shippingMethod = s2;
						_thisCustomer.ShippingMethod = _shippingMethod;
					}				

				}
				else
				{
					if (s2.IndexOf(weightMessage) != -1)
					{
						weightPrompt = "<br>" + s2;
					}
					RateErrors += "<br>" + s2;
				}
			}
      
			RateSelect.Append("</select>");
			if (Count ==0)
			{
				//No rates so clear the _shippingMethod to force recalc
				_shippingMethod = String.Empty;
				_thisCustomer.ShippingMethod = String.Empty;
				return RateErrors;
			}
			else
			{
				if(Common.AppConfigBool("RTShipping.ShowErrors")) 
				{
					RateSelect.Append(RateErrors);
				}
				else
				{
					RateSelect.Append(weightPrompt+"<br>");
				}
				return RateSelect.ToString();
			}
		}


		protected String GetRates(String ShippingHandlingExtraFee)
		{
			RTShipping realTimeShipping = new RTShipping();

			decimal ExtraFee = System.Decimal.Zero;

			if(ShippingHandlingExtraFee.Length != 0)
			{
				ExtraFee = Localization.ParseUSCurrency(ShippingHandlingExtraFee);
			}

			Single MarkupPercent = Common.AppConfigUSSingle("RTShipping.MarkupPercent");

			//realTimeShipping.TestMode = true; // just for debug
			
			// Set shipment info
			realTimeShipping.ShipmentWeight = this.WeightTotal();

			// Create Packages Collection
			RTShipping.Packages shipment = new RTShipping.Packages();
			
			// Set pickup type
			shipment.PickupType = Common.AppConfig("RTShipping.UPS.UPSPickupType"); // RTShipping.PickupTypes.UPSCustomerCounter.ToString();
			if(Common.AppConfig("RTShipping.UPS.UPSPickupType").Length == 0)
			{
				shipment.PickupType = RTShipping.PickupTypes.UPSCustomerCounter.ToString();
			}

			// Set destination address of package
			//			shipment.DestinationAddress1 = DestinationAddress.Text;
			//			shipment.DestinationAddress2 = DestinationAddress2.Text;
			shipment.DestinationCity = _thisCustomer.ShippingCity;
			shipment.DestinationStateProvince = _thisCustomer.ShippingState;
			shipment.DestinationZipPostalCode = _thisCustomer.ShippingZip;
			shipment.DestinationCountryCode = Common.GetCountryTwoLetterISOCode(_thisCustomer.ShippingCountry);

			int PackageID = 1;
			
			// Get ship separately cart Items
			foreach(CartItem c in _cartItems)
			{
				// Handle ship separately items first:
				if(!c.isDownload && c.isShipSeparately)
				{
					for(int n = 1; n <= c.quantity; n++)
					{
						// Create package object for this item
						RTShipping.Package p = new RTShipping.Package();

						p.PackageId = PackageID;
						PackageID = PackageID + 1;
			
						// Dimensions ONLY supported for ship separately items
						String Dimensions = c.dimensions.ToLower(); // MUST be in format of N.NN x N.NN x N.NN! This is Height x Length x Width
						if(Dimensions.Length != 0)
						{
							String[] dd = Dimensions.Split('x');
							try
							{
								p.Height = Localization.ParseUSSingle(dd[0]);
							}
							catch
							{}
							try
							{
								p.Length = Localization.ParseUSSingle(dd[1]);
							}
							catch
							{}
							try
							{
								p.Width = Localization.ParseUSSingle(dd[2]);
							}
							catch
							{}
						}

						p.Weight = c.weight;
						if(p.Weight == 0.0F)
						{
							p.Weight = Common.AppConfigUSSingle("RTShipping.DefaultItemWeight");
						}
						if(p.Weight == 0.0F)
						{
							p.Weight = 0.5F; // must have SOMETHING to use!
						}
			
						// Set insurance. Get from products db shipping values?
						p.Insured = Common.AppConfigBool("RTShipping.Insured"); //false;
						p.InsuredValue = (Single)(c.price * c.quantity);

						// Add package to collection
						shipment.AddPackage(p);

						p = null;
					}
				}
			}
			

			// Get all other items now
			Single remainingItemsWeight = 0.0F;
			Single remainingItemsInsuranceValue = 0.0F;
			foreach(CartItem c in _cartItems)
			{
				// Handle NON ship separately items here, but summing weight (dimensions not supported for these)
				// add one package for all of these items, summing the weight
				if(!c.isDownload && !c.isShipSeparately)
				{
					Single Weight = c.weight;
					if(Weight == 0.0F)
					{
						Weight = Common.AppConfigUSSingle("RTShipping.DefaultItemWeight");
					}
					if(Weight == 0.0F)
					{
						Weight = 0.5F; // must have SOMETHING to use!
					}
					remainingItemsWeight += (Weight * c.quantity);
					remainingItemsInsuranceValue += (Single)(c.price * c.quantity);
				}
			}
			if(remainingItemsWeight != 0.0F)
			{
				// Create package object for this item
				RTShipping.Package p = new RTShipping.Package();

				p.PackageId = PackageID;
				PackageID = PackageID + 1;
		
				p.Weight = remainingItemsWeight;
		
				// Set insurance. Get from products db shipping values?
				p.Insured = Common.AppConfigBool("RTShipping.Insured"); //false;
				p.InsuredValue = remainingItemsInsuranceValue;

				// Add package to collection
				shipment.AddPackage(p);

				p = null;
			}

			realTimeShipping.ShipmentValue = this.SubTotal(true,false,false);


			// Get carriers
			string carriers = Common.AppConfig("RTShipping.ActiveCarrier"); //string.Empty;

			// Get result type
			RTShipping.ResultType format = RTShipping.ResultType.RawDelimited;

			StringBuilder tmpS = new StringBuilder(5000);
			
			String RTShipRequest = String.Empty;
			String RTShipResponse = String.Empty;
			tmpS.Append((string)realTimeShipping.GetRates(shipment, carriers, format, "ShippingMethod", "ShippingMethod", out RTShipRequest,out RTShipResponse,ExtraFee,(decimal)MarkupPercent,this.SubTotal(true,false,false)));
			DB.ExecuteSQL("update customer set RTShipRequest=" + DB.SQuote(RTShipRequest) + ", RTShipResponse=" + DB.SQuote(RTShipResponse) + " where customerid=" + _thisCustomer._customerID.ToString());
			realTimeShipping = null;
			return tmpS.ToString();
		}
			
		public void AddItem(Customer thisCustomer, int ProductID, int VariantID, int Quantity, String ChosenColor, String ChosenColorSKUModifier, String ChosenSize, String ChosenSizeSKUModifier, String TextOption, CartTypeEnum CartType, bool UpdateCartObject)
		{
			foreach(CartItem c in _cartItems)
			{
				if (c.productID == ProductID && c.variantID == VariantID && c.chosenColor == ChosenColor && c.chosenColorSKUModifier == ChosenColorSKUModifier && c.chosenSize == ChosenSize && c.chosenSizeSKUModifier == ChosenSizeSKUModifier && c.textOption == TextOption)
				{
					String sql3 = "update ShoppingCart set Quantity = Quantity + " + Quantity.ToString() + " where ProductID=" + ProductID.ToString() + " and VariantID=" + VariantID.ToString() + " and ChosenColor=" + DB.SQuote(ChosenColor) + " and ChosenSize=" + DB.SQuote(ChosenSize) + " and TextOption like " + DB.SQuote(TextOption) + " and CustomerID=" + _thisCustomer._customerID.ToString() + " and CartType=" + ((int)CartType).ToString();
					DB.ExecuteSQL(sql3);
					_cartItems.Clear();
					LoadFromDB(CartType);
					return;
				}
			}
			String sku = String.Empty;
			String description = String.Empty;
			Decimal price = System.Decimal.Zero;
			String dimensions = String.Empty;
			Single weight = 0.0F;
			String sql = "Select m.name as ManufacturerName,p.SKU, v.SKUSuffix, p.Name, v.Name as VariantName, v.Description, v.Price, v.SalePrice, v.Weight, v.Dimensions, v.Colors, v.Sizes from product p " + DB.GetNoLock() + " ,productvariant v " + DB.GetNoLock() + " , manufacturer m  " + DB.GetNoLock() + " where p.manufacturerid=m.manufacturerid and p.productid=v.productid and v.variantid=" + VariantID.ToString() + " and p.productid=" + ProductID.ToString();
			IDataReader rs = DB.GetRS(sql);
			if(rs.Read())
			{
				sku = Common.MakeProperProductSKU(DB.RSField(rs,"SKU"),DB.RSField(rs,"SKUSuffix"),ChosenColorSKUModifier,ChosenSizeSKUModifier);
				description = DB.RSField(rs,"Name");
				if(DB.RSField(rs,"VariantName").Length != 0)
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
				else if(DB.RSField(rs,"Description").Length != 0)
				{
					description += " - " + Common.Ellipses(DB.RSField(rs,"Description"),30,true);
				}
				//description += " (" + DB.RSField(rs,"ManufacturerName") + ")";
				price = DB.RSFieldDecimal(rs,"Price");
				
				if(DB.RSFieldDecimal(rs,"SalePrice") != System.Decimal.Zero)
				{
					price = DB.RSFieldDecimal(rs,"SalePrice");
				}
				if(Common.IsAPack(ProductID) && Common.GetPackSize(ProductID) == 0)
				{
					bool IsOnSale = false;
					decimal PR = Common.DetermineLevelPrice(VariantID,thisCustomer._customerLevelID, out IsOnSale);
					PR += Common.PackPriceDelta(thisCustomer._customerID,thisCustomer._customerLevelID,ProductID,0);
					price = PR;
				}
				weight = DB.RSFieldSingle(rs,"Weight");
				dimensions = DB.RSField(rs,"Dimensions");

				// check for color & size price modifiers:
				Decimal PrMod = Common.GetColorAndSizePriceDelta(ChosenColor, ChosenSize);
				if(PrMod != System.Decimal.Zero)
				{
					price += PrMod;
				}
				if(price < System.Decimal.Zero)
				{
					price = System.Decimal.Zero; // never know what people will put in the modifiers :)
				}

			}
			rs.Close();
			if(sku.Length != 0 || Common.AppConfigBool("AllowEmptySkuAddToCart"))
			{
				IDataReader rst = DB.GetRS("select * from productvariant  " + DB.GetNoLock() + " where VariantID=" + VariantID.ToString());
				rst.Read();
				int isTaxable = Common.IIF(DB.RSFieldBool(rst,"IsTaxable") , 1 , 0);
				int isShipSeparately = Common.IIF(DB.RSFieldBool(rst,"isShipSeparately") , 1 , 0);
				int isDownload = Common.IIF(DB.RSFieldBool(rst,"isDownload") , 1 , 0);
				String downloadLocation = DB.RSField(rst,"downloadLocation");
				bool isSecureAttachment = DB.RSFieldBool(rst,"IsSecureAttachment");
				int RecurringInterval = DB.RSFieldInt(rst,"RecurringInterval");
				if(RecurringInterval == 0)
				{
					RecurringInterval = 1; // for backwards compatability
				}
				RecurringIntervalTypeEnum RecurringIntervalType = (RecurringIntervalTypeEnum)DB.RSFieldInt(rst,"RecurringIntervalType");
				if(RecurringIntervalType == RecurringIntervalTypeEnum.NotUsed)
				{
					RecurringIntervalType = RecurringIntervalTypeEnum.Monthly; // for backwards compatibility
				}
				rst.Close();
				String NewGUID = DB.GetNewGUID();
				StringBuilder sql2 = new StringBuilder("insert into ShoppingCart(CartType,ShoppingCartRecGUID,CustomerID,ProductID,SubscriptionMonths,ProductTypeID,ShippingCost,VariantID,ProductSKU,ProductName,ProductPrice,ProductWeight,ProductDimensions,Quantity,ChosenColor,ChosenColorSKUModifier,ChosenSize,ChosenSizeSKUModifier,TextOption,IsTaxable,IsShipSeparately,IsDownload,DownloadLocation,IsSecureAttachment,RecurringInterval,RecurringIntervalType) values(",10000);
				sql2.Append(((int)CartType).ToString() + ",");
				sql2.Append(DB.SQuote(NewGUID) + ",");
				sql2.Append(_thisCustomer._customerID.ToString() + ",");
				sql2.Append(ProductID.ToString() + ",");
				sql2.Append(Common.GetSubscriptionMonths(VariantID).ToString() + ",");
				sql2.Append("0,");
				sql2.Append(Common.GetProductShippingCost(VariantID).ToString() + ",");
				sql2.Append(VariantID.ToString() + ",");
				sql2.Append(DB.SQuote(sku) + ",");
				sql2.Append(DB.SQuote(description) + ",");
				sql2.Append(Localization.CurrencyStringForDB(price) + ",");
				sql2.Append(Localization.SingleStringForDB(weight) + ",");
				sql2.Append(DB.SQuote(dimensions) + ",");
				sql2.Append(Quantity.ToString() + ",");
				sql2.Append(DB.SQuote(ChosenColor) + ",");
				sql2.Append(DB.SQuote(ChosenColorSKUModifier) + ",");
				sql2.Append(DB.SQuote(ChosenSize) + ",");
				sql2.Append(DB.SQuote(ChosenSizeSKUModifier) + ",");
				sql2.Append(DB.SQuote(TextOption) + ",");
				sql2.Append(isTaxable.ToString() + ",");
				sql2.Append(isShipSeparately.ToString() + ",");
				sql2.Append(isDownload.ToString() + ",");
				sql2.Append(DB.SQuote(downloadLocation) + ",");
				sql2.Append(Common.IIF(isSecureAttachment,"1","0") + ",");
				sql2.Append(RecurringInterval.ToString() + ",");
				sql2.Append(((int)RecurringIntervalType).ToString());
				sql2.Append(")");
				DB.ExecuteSQL(sql2.ToString());
				// must update cart price in this record to be the correct kit price, NOT the base kit price
				if(Common.IsAKit(ProductID))
				{
					IDataReader rs2 = DB.GetRS("select ShoppingCartrecid from ShoppingCart  " + DB.GetNoLock() + " where ShoppingCartRecGUID=" + DB.SQuote(NewGUID));
					rs2.Read();
					int NewCartRecID = DB.RSFieldInt(rs2,"ShoppingCartRecid");
					rs2.Close();
					decimal KitPR = Common.GetKitTotalPrice(_thisCustomer._customerID,ProductID,NewCartRecID);
					DB.ExecuteSQL("update ShoppingCart set ProductPrice=" + Localization.CurrencyStringForGateway(KitPR) + " where ShoppingCartRecID=" + NewCartRecID.ToString());
				}
				if(UpdateCartObject)
				{
					_cartItems.Clear();
					LoadFromDB(CartType);
				}
			}
		}

		public void SetItemQuantity(int ProductID, int VariantID, int Quantity, CartTypeEnum CartType)
		{
			int recID;
			IDataReader rs = DB.GetRS("Select ShoppingCartRecID from ShoppingCart  " + DB.GetNoLock() + " where CartType=" + ((int)CartType).ToString() + " and VariantID=" + VariantID.ToString() + " and productID=" + ProductID.ToString() + " and CustomerID=" + _thisCustomer._customerID.ToString());
			if(rs.Read())
			{
				recID = DB.RSFieldInt(rs,"ShoppingCartRecID");
				SetItemQuantity(recID,Quantity);
			}
			rs.Close();
		}

		public void SetItemQuantity(int cartRecordID, int Quantity)
		{
			if(Quantity == 0)
			{
				RemoveItem(cartRecordID);
			}
			else
			{
				String sql = "update ShoppingCart set Quantity=" + Quantity.ToString() + " where ShoppingCartRecID=" + cartRecordID.ToString() + " and CustomerID=" + _thisCustomer._customerID.ToString();
				DB.ExecuteSQL(sql);
				//				_cartItems.Clear(); // not needed
				//				LoadFromDB(); // not needed
			}
			return;
		}

		public void RemoveItem(int ProductID, int VariantID, CartTypeEnum CartType)
		{
			int recID;
			IDataReader rs = DB.GetRS("Select ShoppingCartRecID from ShoppingCart  " + DB.GetNoLock() + " where CartType=" + ((int)CartType) + " and VariantID=" + VariantID.ToString() + " and productID=" + ProductID.ToString() + " and CustomerID=" + _thisCustomer._customerID.ToString());
			if(rs.Read())
			{
				recID = DB.RSFieldInt(rs,"ShoppingCartRecID");
				RemoveItem(recID);
			}
			rs.Close();
		}

		public void RemoveItem(int cartRecordID)
		{
			if(cartRecordID != 0)
			{
				DB.ExecuteSQL("delete from customcart where ShoppingCartRecID=" + cartRecordID.ToString() + " and CustomerID=" + _thisCustomer._customerID.ToString());
				DB.ExecuteSQL("delete from kitcart where ShoppingCartRecID=" + cartRecordID.ToString() + " and CustomerID=" + _thisCustomer._customerID.ToString());
			}
			String sql = "delete from ShoppingCart where ShoppingCartRecID=" + cartRecordID.ToString() + " and CustomerID=" + _thisCustomer._customerID.ToString();
			DB.ExecuteSQL(sql);
			//			_cartItems.Clear(); // not needed
			//			LoadFromDB(); // not needed
			return;
		}

		public bool IsEmpty()
		{
			return _isEmpty;
		}

		public void ClearContents()
		{
			_isEmpty = true;
		}

		public bool HasCoupon()
		{
			bool tmp = false;
			try
			{
				tmp = (_coupon.code.Length != 0);
			}
			catch {}
			return tmp;
		}

		// returns "OK" if coupon is valid, else returns reason why coupon is not valid
		public String CouponIsValid()
		{
			String status = "OK";
			if(_coupon.code.Length == 0)
			{
				// empty coupons are never valid:
				status = "NO COUPON SPECIFIED";
			}
			else
			{
				IDataReader rs = DB.GetRS("select * from coupon  " + DB.GetNoLock() + " where ExpirationDate>=" + DB.DateQuote(Localization.ToNativeShortDateString(System.DateTime.Now)) + " and deleted=0 and lower(CouponCode)=" + DB.SQuote(_coupon.code.ToLower()));
				if(rs.Read())
				{
					// we found a valid match for that coupon code with an expiration date greater than or equal to now, so check additional conditions on the coupon:
					// just return first reason for it not being valid, going from most obvious to least obvious:
					if(status == "OK")
					{
						if(DB.RSFieldDateTime(rs,"ExpirationDate") < System.DateTime.Now)
						{
							status = "THAT COUPON HAS EXPIRED";
						}
					}
					if(status == "OK")
					{
						if(DB.RSFieldBool(rs,"ExpiresOnFirstUseByAnyCustomer"))
						{
							if(Common.AnyCustomerHasUsedCoupon(_coupon.code))
							{
								status = "THAT COUPON HAS ALREADY BEEN USED BY SOMEONE, AND CANNOT BE RE-USED";
							}
						}
					}
					if(status == "OK")
					{
						if(DB.RSFieldBool(rs,"ExpiresAfterOneUsageByEachCustomer"))
						{
							if(Customer.HasUsedCoupon(_thisCustomer._customerID,_coupon.code))
							{
								status = "YOU CAN ONLY USE THAT COUPON ONCE";
							}
						}
					}
					if(status == "OK")
					{
						if(DB.RSFieldInt(rs,"ExpiresAfterNUses") > 0)
						{
							if(Common.GetNumberOfCouponUses(_coupon.code) > DB.RSFieldInt(rs,"ExpiresAfterNUses"))
							{
								status = "THAT COUPON HAS BEEN USED THE MAXIMUM NUMBER OF TIMES";
							}
						}
					}
					if(status == "OK")
					{
						if(DB.RSFieldDecimal(rs,"RequiresMinimumOrderAmount") > System.Decimal.Zero)
						{
							if(this.SubTotal(false,false,true) < DB.RSFieldDecimal(rs,"RequiresMinimumOrderAmount"))
							{
								status = "THAT COUPON CAN ONLY BE USED FOR ORDERS GREATER THAN " + Localization.CurrencyStringForDisplay(DB.RSFieldDecimal(rs,"RequiresMinimumOrderAmount"));
							}
						}
					}
					if(status == "OK")
					{
						if(DB.RSField(rs,"ValidForCustomers").Length != 0)
						{
							if(!Common.IntegerIsInIntegerList(_thisCustomer._customerID,DB.RSField(rs,"ValidForCustomers")))
							{
								status = "YOU ARE NOT AUTHORIZED TO USE THAT COUPON";
							}
						}
					}
					if(status == "OK")
					{
						try
						{
							if(DB.RSField(rs,"ValidForProducts").Length != 0)
							{
								if(DB.GetSqlN("select count(productid) as N from ShoppingCart  " + DB.GetNoLock() + " where productid in (" + DB.RSField(rs,"ValidForProducts") + ") and CartType=" + ((int)CartTypeEnum.ShoppingCart).ToString() + " and customerid=" + _thisCustomer._customerID.ToString()) == 0)
								{
									status = "THAT COUPON CAN ONLY BE USED ON SELECTED PRODUCTS";
								}
							}
						}
						catch
						{
							status = "ERROR CHECKING CART FOR ALLOWED COUPON PRODUCTS";
						}
					}
					if(status == "OK")
					{
						try
						{
							if(DB.RSField(rs,"ValidForCategories").Length != 0)
							{
								if(DB.GetSqlN("select count(categoryid) as N from productcategory  " + DB.GetNoLock() + " where categoryid in (" + DB.RSField(rs,"ValidForCategories") + ") and productid in (select distinct productid from ShoppingCart  " + DB.GetNoLock() + " where CartType=" + ((int)CartTypeEnum.ShoppingCart).ToString() + " and customerid=" + _thisCustomer._customerID.ToString() + ")") == 0)
								{
									status = "THAT COUPON CAN ONLY BE USED IF YOU PURCHASE PRODUCTS IN SELECTED CATEGORIES";
								}
							}
						}
						catch
						{
							status = "ERROR CHECKING CART FOR ALLOWED COUPON CATEGORIES";
						}
					}
					if(status == "OK")
					{
						try
						{
							if(DB.RSField(rs,"ValidForSections").Length != 0)
							{
								if(DB.GetSqlN("select count(sectionid) as N from productsection  " + DB.GetNoLock() + " where sectionid in (" + DB.RSField(rs,"ValidForSections") + ") and productid in (select distinct productid from ShoppingCart  " + DB.GetNoLock() + " where CartType=" + ((int)CartTypeEnum.ShoppingCart).ToString() + " and customerid=" + _thisCustomer._customerID.ToString() + ")") == 0)
								{
									status = "THAT COUPON CAN ONLY BE USED IF YOU PURCHASE PRODUCTS IN SELECTED " + Common.AppConfig("SectionPromptPlural").ToUpper();
								}
							}
						}
						catch
						{
							status = "ERROR CHECKING CART FOR ALLOWED COUPON SECTIONS";
						}
					}
					if(status == "OK")
					{
						try
						{
							if(DB.RSField(rs,"ValidForManufacturers").Length != 0)
							{
								if(DB.GetSqlN("select count(manufacturerid) as N from manufacturer  " + DB.GetNoLock() + " where manufacturerid in (" + DB.RSField(rs,"ValidForManufacturers") + ") and manufacturerid in (select distinct manufacturerid from product  " + DB.GetNoLock() + " where productid in (select distinct productid from ShoppingCart  " + DB.GetNoLock() + " where CartType=" + ((int)CartTypeEnum.ShoppingCart).ToString() + " and customerid=" + _thisCustomer._customerID.ToString() + "))") == 0)
								{
									status = "THAT COUPON CAN ONLY BE USED IF YOU PURCHASE PRODUCTS BY SELECTED MANUFACTURERS";
								}
							}
						}
						catch
						{
							status = "ERROR CHECKING CART FOR ALLOWED COUPON MANUFACTURERS";
						}
					}
				}
				else
				{
					status = "INVALID COUPON CODE";
				}
				rs.Close();
			}
			return status;
		}


		public String GetOptionsList()
		{
			StringBuilder tmpS = new StringBuilder(5000);
			if(_orderOptions.Length != 0)
			{
				IDataReader rs = DB.GetRS("Select * from orderoption  " + DB.GetNoLock() + " where orderoptionid in (" + _orderOptions + ")");
				while(rs.Read())
				{
					if(tmpS.Length != 0)
					{
						tmpS.Append("^");
					}
					tmpS.Append(DB.RSFieldInt(rs,"OrderOptionID").ToString() + "|" + DB.RSField(rs,"Name") + "|" + Localization.CurrencyStringForDisplay(DB.RSFieldDecimal(rs,"Cost")));
				}
				rs.Close();
			}
			return tmpS.ToString();
		}

		// returns true if this order has any items which are download goods:
		public bool HasDownloadComponents()
		{
			foreach(CartItem i in _cartItems)
			{
				if(i.isDownload)
				{
					return true;
				}
			}
			return false;
		}

		// returns true if this order has ONLY download goods:
		public bool IsAllDownloadComponents()
		{
			foreach(CartItem i in _cartItems)
			{
				if(!i.isDownload)
				{
					return false;
				}
			}
			return true;
		}

		public int SubscriptionMonths()
		{
			int tmp = 0;
			foreach(CartItem i in _cartItems)
			{
				tmp += (i.subscriptionMonths * i.quantity);
			}
			return tmp;
		}

		// returns true if this order has any download items that have download locations:
		public bool ThereAreDownloadFilesSpecified()
		{
			foreach(CartItem i in _cartItems)
			{
				if(i.isDownload && i.downloadLocation.Trim().Length != 0)
				{
					return true;
				}
			}
			return false;
		}

		public bool Contains(int ProductID, int VariantID)
		{
			foreach(CartItem i in _cartItems)
			{
				if(i.productID == ProductID && i.variantID == VariantID)
				{
					return true;
				}
			}
			return false;
		}

		public bool Contains(int ProductID)
		{
			foreach(CartItem i in _cartItems)
			{
				if(i.productID == ProductID)
				{
					return true;
				}
			}
			return false;
		}

		public bool ContainsRecurring()
		{
			foreach(CartItem i in _cartItems)
			{
				if(i.isRecurring)
				{
					return true;
				}
			}
			return false;
		}

		public static String DisplayMiniCart(Customer thisCustomer, int SiteID)
		{
			ShoppingCart cart = new ShoppingCart(SiteID,thisCustomer,CartTypeEnum.ShoppingCart,0,false);
			if(cart._isEmpty)
			{
				return String.Empty;
			}

			StringBuilder tmpS = new StringBuilder(10000);

			tmpS.Append("<table width=\"150\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<img src=\"skins/Skin_" + SiteID.ToString() + "/images/minicart.gif\" border=\"0\"><br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"center\" valign=\"top\">\n");

			bool ShowLinkBack = Common.AppConfigBool("LinkToProductPageInCart");
			foreach(CartItem c in cart._cartItems)
			{
				if(c.SKU != "MICROPAY")
				{
					if(ShowLinkBack)
					{
						tmpS.Append("<a href=\"" + SE.MakeProductLink(c.productID,"") + "\">");
					}
					if(Common.AppConfigBool("ShowPicsInMiniCart"))
					{
						String ProdPic = Common.LookupImage("Product",c.productID,"icon",SiteID);
						if(ProdPic.Length == 0)
						{
							ProdPic = Common.AppConfig("NoPicture");
						}

						int MaxWidth = Common.AppConfigNativeInt("MiniCartMaxIconWidth");
						if(MaxWidth == 0)
						{
							MaxWidth = 125;
						}
						int MaxHeight = Common.AppConfigNativeInt("MiniCartMaxIconHeight");
						if(MaxHeight == 0)
						{
							MaxHeight = 125;
						}
						if(ProdPic.Length != 0)
						{
							int w = Common.GetImageWidth(ProdPic);
							int h = Common.GetImageHeight(ProdPic);
							if(w > MaxWidth)
							{
								tmpS.Append("<img align=\"center\" src=\"" + ProdPic + "\" width=\"" + MaxWidth.ToString() + "\" border=\"0\"><br>");
							}
							else if(h > MaxHeight)
							{
								tmpS.Append("<img align=\"center\" src=\"" + ProdPic + "\" height=\"" + MaxHeight + "\" border=\"0\"><br>");
							}
							else
							{
								tmpS.Append("<img align=\"center\" src=\"" + ProdPic + "\" border=\"0\"><br>");
							}
						}
					}

					tmpS.Append(c.productName);
					if(ShowLinkBack)
					{
						tmpS.Append("</a>");
					}
					tmpS.Append("<br>");

					int Q = c.quantity;
					decimal PR = c.price * Q;
					int ActiveDID = 0;
					Single DIDPercent = 0.0F;
					if(Common.CustomerLevelAllowsQuantityDiscounts(thisCustomer._customerLevelID))
					{
						ActiveDID = Common.LookupActiveVariantQuantityDiscountID(c.variantID);
						DIDPercent = Common.GetDIDPercent(ActiveDID,Q);
						if(ActiveDID != 0 && DIDPercent != 0.0F)
						{
							PR = (decimal)(1.0-(DIDPercent/100.0)) * PR;
						}
					}
					tmpS.Append("Qty " + Q.ToString() + "&nbsp;" + Localization.CurrencyStringForDisplay(PR));
					tmpS.Append("<br><br>");
				}
			}

			tmpS.Append("SubTotal: " + Localization.CurrencyStringForDisplay(cart.SubTotal(true,false,true)) + "<br>");


			tmpS.Append("<a href=\"ShoppingCart.aspx\"><font color=\"BLUE\"><b>CHECKOUT</b></a>");

			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");

			return tmpS.ToString();
		}

	}
}
