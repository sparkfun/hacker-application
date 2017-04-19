// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on cart license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Text;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontGateways
{
	/// <summary>
	/// Summary description for Gateway.
	/// </summary>
	public class Gateway
	{
		public Gateway() {}

		public static String MakeRecurringOrder(ShoppingCart cart, int OrderNumber)
		{
			return MakeOrder(cart,cart._billingAddress.PaymentMethod,String.Empty,cart._billingAddress.CardName,cart._billingAddress.CardType,cart._billingAddress.CardNumber,String.Empty,cart._billingAddress.CardExpirationMonth,cart._billingAddress.CardExpirationYear,OrderNumber,String.Empty,String.Empty,String.Empty,cart._billingAddress.ECheckBankABACode,cart._billingAddress.ECheckBankAccountNumber,cart._billingAddress.ECheckBankAccountType,cart._billingAddress.ECheckBankName,cart._billingAddress.ECheckBankAccountName);
		}

		public static String MakeQuoteOrder(ShoppingCart cart, int OrderNumber, String PaymentMethod,String PONumber)
		{
			String status = MakeOrder(cart, PaymentMethod,PONumber,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,OrderNumber,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty,String.Empty);
			DB.ExecuteSQL("update orders set PONumber=" + DB.SQuote(PONumber) + " where OrderNumber=" + OrderNumber.ToString());
			return status;
		}

		public static int CreateOrderRecord(ShoppingCart cart, int OrderNumber, String PaymentMethod, String PONumber, String CardName, String CardType, String CardNumber, String CardExtraCode, String CardExpirationMonth, String CardExpirationYear, String eCheckBankABACode, String eCheckBankAccountNumber, String eCheckBankAccountType, String eCheckBankName, String eCheckBankAccountName)
		{
			StringBuilder sql = new StringBuilder(5000);
			String orderGUID = Common.GetNewGUID();

			if(OrderNumber == 0)
			{
				OrderNumber = Common.GetNextOrderNumber();
			}

			// must create new order record, so we can get an OrderNumber in case it's good
			IDataReader rsCustomer = DB.GetRS("select * from customer  " + DB.GetNoLock() + " where customerid=" + cart._thisCustomer._customerID.ToString());
			rsCustomer.Read();

			sql.Append("insert into Orders(OrderNumber, OrderGUID,TaxRate,CartType,LocaleSetting,OrderWeight,TransactionState,PONumber,eCheckBankABACode,eCheckBankAccountNumber,eCheckBankAccountType,eCheckBankName,eCheckBankAccountName,StoreVersion,CustomerID,Referrer,OrderNotes,CustomerServiceNotes,OrderOptions,PaymentMethod,LastIPAddress,CustomerGUID,[Password],SiteID,LastName,FirstName,EMail,Phone,Notes,RegisterDate,AffiliateID,CouponCode,CouponDescription,CouponDiscountAmount,CouponDiscountPercent,CouponIncludesFreeShipping,OkToEMail,Deleted,BillingEqualsShipping,BillingLastName,BillingFirstName,BillingCompany,BillingAddress1,BillingAddress2,BillingSuite,BillingCity,BillingState,BillingZip,BillingCountry,BillingPhone,ShippingLastName,ShippingFirstName,ShippingCompany,ShippingAddress1,ShippingAddress2,ShippingSuite,ShippingCity,ShippingState,ShippingZip,ShippingCountry,ShippingPhone,ShippingMethodID,ShippingMethod,ShippingCalculationID,RTShipRequest,RTShipResponse,CardType,CardName,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderSubtotal,OrderTax,OrderShippingCosts,OrderTotal,AuthorizationResult,AuthorizationCode,AuthorizationPNREF,TransactionCommand) values (");
			sql.Append( OrderNumber.ToString() + ",");
			sql.Append( DB.SQuote(orderGUID) + ",");
			sql.Append(Localization.SingleStringForDB(cart._stateTaxRate) + ",");
			sql.Append(((int)cart._cartType).ToString() + ",");
			sql.Append(DB.SQuote(cart._thisCustomer._localeSetting) + ",");
			sql.Append( Localization.SingleStringForDB(cart.WeightTotal()) + ",");

			if (PaymentMethod.ToUpper().Replace(" ","").Trim() == "CREDITCARD" || PaymentMethod.ToUpper() == "MICROPAY")
			{
				sql.Append( DB.SQuote(Common.TransactionMode()) + ",");
			}
			else
			{
				sql.Append( "NULL,");
			}
			sql.Append( DB.SQuote(PONumber) + ",");
			sql.Append( DB.SQuote(eCheckBankABACode) + ",");
			sql.Append( DB.SQuote(eCheckBankAccountNumber) + ",");
			sql.Append( DB.SQuote(eCheckBankAccountType) + ",");
			sql.Append( DB.SQuote(eCheckBankName) + ",");
			sql.Append( DB.SQuote(eCheckBankAccountName) + ",");
			sql.Append( DB.SQuote(Common.AppConfig("StoreVersion")) + ",");
			sql.Append( cart._thisCustomer._customerID.ToString() + ",");
			sql.Append( DB.SQuote(DB.RSField(rsCustomer,"Referrer")) + ",");
			sql.Append( DB.SQuote(cart._orderNotes) + ",");
			sql.Append( DB.SQuote(Common.IIF(cart._cartType == CartTypeEnum.RecurringCart, "Recurring Auto-Ship, Sequence #" + ((CartItem)cart._cartItems[0])._recurringIndex.ToString(),"")) + ",");
			sql.Append( DB.SQuote(cart.GetOptionsList()) + ",");
			sql.Append( DB.SQuote(PaymentMethod) + ",");
			sql.Append( DB.SQuote(DB.RSField(rsCustomer,"LastIPAddress")) + ",");
			sql.Append( DB.SQuote(DB.RSFieldGUID(rsCustomer,"CustomerGUID")) + ",");
			sql.Append( DB.SQuote(DB.RSField(rsCustomer,"Password")) + ",");
			sql.Append( cart._siteID.ToString() + ",");
			sql.Append( DB.SQuote(DB.RSField(rsCustomer,"LastName")) + ",");
			sql.Append( DB.SQuote(DB.RSField(rsCustomer,"FirstName")) + ",");
			sql.Append( DB.SQuote(DB.RSField(rsCustomer,"EMail")) + ",");
			sql.Append( DB.SQuote(DB.RSField(rsCustomer,"Phone")) + ",");
			sql.Append( DB.SQuote(DB.RSField(rsCustomer,"Notes")) + ",");
			sql.Append( DB.DateQuote(DB.RSFieldDateTime(rsCustomer,"RegisterDate").ToString("G")) + ",");
			sql.Append( cart._thisCustomer._affiliateID.ToString() + ",");
			if(cart._coupon.code.Length == 0)
			{
				sql.Append( "NULL,");
			}
			else
			{
				sql.Append( DB.SQuote(cart._coupon.code) + ",");
			}
			if(cart.HasCoupon() && cart.CouponIsValid() == "OK")
			{
				sql.Append( DB.SQuote(cart._coupon.description) + ",");
				sql.Append( cart._coupon.discountAmount.ToString() + ",");
				sql.Append( cart._coupon.discountPercent.ToString() + ",");
				if(cart._coupon.includesFreeShipping)
				{
					sql.Append( "1,");
				}
				else
				{
					sql.Append( "0,");
				}
			}
			else
			{
				sql.Append( "NULL,");
				sql.Append( "0,");
				sql.Append( "0,");
				sql.Append( "0,");
			}
			if(DB.RSFieldBool(rsCustomer,"OkToEmail"))
			{
				sql.Append( "1,");
			}
			else
			{
				sql.Append( "0,");		
			}
			sql.Append( "0,");
			if(DB.RSFieldBool(rsCustomer,"BillingEqualsShipping"))
			{
				sql.Append( "1,");
			}
			else
			{
				sql.Append( "0,");		
			}
      
			//Update the Billing record with card information
			Address shippingAddress = new Address();
			Address billingAddress = new Address();
			billingAddress.LoadByCustomer(cart._thisCustomer._customerID,AddressTypes.Billing);
			if (billingAddress.AddressID != 0)
			{
				cart._billingAddress = billingAddress;
				cart._billingAddress.PaymentMethod = PaymentMethod;
				if (PaymentMethod.ToUpper().Replace(" ","") == "CREDITCARD")
				{
					cart._billingAddress.CardType = CardType;
					cart._billingAddress.CardName = CardName;
					cart._billingAddress.CardExtraCode = CardExtraCode;
					cart._billingAddress.CardNumber = CardNumber;
					cart._billingAddress.CardExpirationMonth = CardExpirationMonth;
					cart._billingAddress.CardExpirationYear = CardExpirationYear;
				}
				if (PaymentMethod.ToUpper().Replace(" ","") == "ECHECK")
				{
					cart._billingAddress.ECheckBankABACode = eCheckBankABACode;
					cart._billingAddress.ECheckBankAccountName = eCheckBankAccountName;
					cart._billingAddress.ECheckBankAccountNumber = eCheckBankAccountNumber;
					cart._billingAddress.ECheckBankAccountType = eCheckBankAccountType;
					cart._billingAddress.ECheckBankName = eCheckBankName;
				}
				cart._billingAddress.UpdateDB();
			}

			//Save the Shipping Methods info.
			//Reload the shipping address in case it's being shared with the billing info above
			shippingAddress.LoadByCustomer(cart._thisCustomer._customerID,AddressTypes.Shipping);
			if (shippingAddress.AddressID != 0)
			{
				cart._shippingAddress = shippingAddress;
				cart._shippingAddress.ShippingMethod = cart._shippingMethod;
				cart._shippingAddress.ShippingMethodID = cart._shippingMethodID;
				cart._shippingAddress.UpdateDB();
			}

			shippingAddress = null;
			billingAddress = null;

			sql.Append( DB.SQuote(cart._billingAddress.LastName) + ",");
			sql.Append( DB.SQuote(cart._billingAddress.FirstName) + ",");
			sql.Append( DB.SQuote(cart._billingAddress.Company) + ",");
			sql.Append( DB.SQuote(cart._billingAddress.Address1) + ",");
			sql.Append( DB.SQuote(cart._billingAddress.Address2) + ",");
			sql.Append( DB.SQuote(cart._billingAddress.Suite) + ",");
			sql.Append( DB.SQuote(cart._billingAddress.City) + ",");
			sql.Append( DB.SQuote(cart._billingAddress.State) + ",");
			sql.Append( DB.SQuote(cart._billingAddress.Zip) + ",");
			sql.Append( DB.SQuote(cart._billingAddress.Country) + ",");
			sql.Append( DB.SQuote(cart._billingAddress.Phone) + ",");
			
			sql.Append( DB.SQuote(cart._shippingAddress.LastName) + ",");
			sql.Append( DB.SQuote(cart._shippingAddress.FirstName) + ",");
			sql.Append( DB.SQuote(cart._shippingAddress.Company) + ",");
			sql.Append( DB.SQuote(cart._shippingAddress.Address1) + ",");
			sql.Append( DB.SQuote(cart._shippingAddress.Address2) + ",");
			sql.Append( DB.SQuote(cart._shippingAddress.Suite) + ",");
			sql.Append( DB.SQuote(cart._shippingAddress.City) + ",");
			sql.Append( DB.SQuote(cart._shippingAddress.State) + ",");
			sql.Append( DB.SQuote(cart._shippingAddress.Zip) + ",");
			sql.Append( DB.SQuote(cart._shippingAddress.Country) + ",");
			sql.Append( DB.SQuote(cart._shippingAddress.Phone) + ",");

			sql.Append( cart._shippingMethodID.ToString() + ",");
			sql.Append( DB.SQuote(cart._shippingMethod) + ",");
			sql.Append( ((int)Common.GetActiveShippingCalculationID()).ToString() + ",");
			sql.Append( DB.SQuote(DB.RSField(rsCustomer,"RTShipRequest")) + ",");
			sql.Append( DB.SQuote(DB.RSField(rsCustomer,"RTShipResponse")) + ",");
			sql.Append( DB.SQuote(CardType) + ",");
			sql.Append( DB.SQuote(CardName) + ",");
			String CC = String.Empty;
			if(Common.AppConfigBool("StoreCCInDB"))
			{
				CC = Common.MungeString(CardNumber);
			}
			else
			{
				CC = "XXXXXXXXXXXX";
			}
			sql.Append( DB.SQuote(CC) + ",");
			sql.Append( "NULL,"); // not allowed to store cardextracode
			sql.Append( DB.SQuote(CardExpirationMonth) + ",");
			sql.Append( DB.SQuote(CardExpirationYear) + ",");
			sql.Append( Localization.CurrencyStringForDB(cart.SubTotal(true,false,true)) + ",");
			sql.Append( Localization.CurrencyStringForDB(cart.TaxTotal(true)) + ",");
			sql.Append( Localization.CurrencyStringForDB(cart.ShippingTotal(true)) + ",");
			sql.Append( Localization.CurrencyStringForDB(cart.Total(true)) + ",");
			sql.Append( DB.SQuote("TBD") + ","); // must update later to RawResponseString if TX is ok!
			sql.Append( DB.SQuote("TBD") + ",");   // must update later if TX is ok!
			sql.Append( DB.SQuote("TBD") + ",");  // must update later if TX is ok!
			sql.Append( DB.SQuote("TBD") + ")");  // must update later if TX is ok!

			DB.ExecuteSQL(sql.ToString());
			rsCustomer.Close();

			return OrderNumber;
		}


		// returns "OK" or error Msg.
		// if "OK" then order was created successfully, and the cart is now empty and _newOrderNumber is the new order number created
		// if error msg, then shopping cart remains as it was before call
		public static String MakeOrder(ShoppingCart cart, String PaymentMethod, String PONumber, String CardName, String CardType, String CardNumber, String CardExtraCode, String CardExpirationMonth, String CardExpirationYear, int OrderNumber, String CAVV, String ECI, String XID, String eCheckBankABACode, String eCheckBankAccountNumber, String eCheckBankAccountType, String eCheckBankName, String eCheckBankAccountName)
		{

			if(OrderNumber == 0)
			{
				OrderNumber = Common.GetNextOrderNumber();
			}
			//_newOrderNumber = 0;
			
			bool PaymentCleared = false;
			String AVSResult = String.Empty;
			String AuthorizationResult = String.Empty;
			String AuthorizationCode = String.Empty;
			String AuthorizationTransID = String.Empty;
			String TransactionCommand = String.Empty;

			String GW = Common.AppConfig("PaymentGateway");

			String status = "OK";
			Decimal OrderTotal = cart.Total(true);
			
			String PM = PaymentMethod.Replace(" ","").ToUpper().Trim();
			if(OrderTotal == System.Decimal.Zero)
			{
				PaymentCleared = true;
				AuthorizationTransID = "0";
				status = "OK"; // nothing to charge!
			}
			else
			{
				switch(PM)
				{
					case "CREDITCARD":
						status = ProcessCard(cart,OrderNumber,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,Common.AppConfigBool("UseLiveTransactions"), CAVV, ECI, XID, out PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					case "PAYPAL":
						status = ProcessCard(cart,OrderNumber,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,Common.AppConfigBool("UseLiveTransactions"), CAVV, ECI, XID, out PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					case "ECHECK":
						status = ProcessECheck(cart,OrderNumber,eCheckBankABACode,eCheckBankAccountNumber,eCheckBankAccountType,eCheckBankName,eCheckBankAccountName,OrderTotal,Common.AppConfigBool("UseLiveTransactions"), CAVV, ECI, XID, out PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
						//V3_9        
					case "MICROPAY":
						status = Micropay.ProcessMicropay(OrderNumber, cart, OrderTotal,cart._billingAddress, out PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						GW = "MicroPay";
						break;
					case "BYPASSGATEWAY": // this is a completely unsupported/undocumented feature. do not use.
						status = "OK";
						PaymentCleared = true;
						AVSResult = String.Empty;
						AuthorizationResult = String.Empty;
						AuthorizationCode = String.Empty;
						AuthorizationTransID = String.Empty;
						TransactionCommand = "Bypass Gateway";
						break;
				}
			}
			if(status == "OK")
			{
				CreateOrderRecord(cart,OrderNumber, PaymentMethod, PONumber, CardName, CardType, CardNumber, CardExtraCode, CardExpirationMonth, CardExpirationYear,eCheckBankABACode,eCheckBankAccountNumber,eCheckBankAccountType,eCheckBankName,eCheckBankAccountName);

				String TransCMD = String.Empty;
				if(TransactionCommand.Length != 0 && CardNumber.Length != 0)
				{
					String CC = String.Empty;
					if(Common.AppConfigBool("StoreCCInDB"))
					{
						CC = Common.MungeString(CardNumber);
					}
					else
					{
						CC = "XXXXXXXXXXXX";
					}
					TransCMD = TransactionCommand.Replace(CardNumber,CC);
				}
				if(TransactionCommand.Length != 0 && eCheckBankAccountNumber.Length != 0)
				{
					TransCMD = TransactionCommand;
				}
				String sql2 = "update orders set " +
					"PaymentClearedOn=" + Common.IIF(PM == "PAYPAL" || (PaymentCleared && Common.TransactionMode() == "AUTH CAPTURE"), DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)), "NULL") + ", " +
					"PaymentGateway=" + DB.SQuote(GW) + ", " +
					"AVSResult=" + DB.SQuote(AVSResult) + ", " +
					"AuthorizationResult=" + DB.SQuote(AuthorizationResult) + ", " +
					"AuthorizationCode=" + DB.SQuote(AuthorizationCode) + ", " + 
					"AuthorizationPNREF=" + DB.SQuote(AuthorizationTransID) + ", " + 
					"TransactionCommand=" + DB.SQuote(TransCMD) +
					" where OrderNumber=" + OrderNumber.ToString();
				DB.ExecuteSQL(sql2);

				
				// order was ok, clean up shopping cart!
				// move cart to order cart:
				if(cart.HasCoupon())
				{
					Common.RecordCouponUsage(cart._thisCustomer._customerID,cart._coupon.code);
				}

				//When processing Recurring order need to limit to the current _originalRecurringOrderNumber
				String RecurringOrderSql = String.Empty;
				if (cart._originalRecurringOrderNumber != 0)
				{
					RecurringOrderSql = String.Format(" and OriginalRecurringOrderNumber={0}",cart._originalRecurringOrderNumber);
				}

				String sql4 = "insert into orders_ShoppingCart(OrderNumber,CartType,ShoppingCartRecID,CustomerID,ProductID,SubscriptionMonths,ProductTypeID,ShippingCost,VariantID,Quantity,ChosenColor,ChosenColorSKUModifier,ChosenSize,ChosenSizeSKUModifier,TextOption,OrderedProductDescription,OrderedProductSKU,OrderedProductPrice,IsTaxable,IsShipSeparately,IsDownload,DownloadLocation,IsSecureAttachment)"
					+ String.Format(" select {0},CartType,ShoppingCartRecID,CustomerID,ProductID,SubscriptionMonths,ProductTypeID,ShippingCost,VariantID,Quantity,ChosenColor,ChosenColorSKUModifier,ChosenSize,ChosenSizeSKUModifier,TextOption,ProductName,ProductSKU,ProductPrice,IsTaxable,IsShipSeparately,IsDownload,DownloadLocation,IsSecureAttachment",OrderNumber)
					+               " from ShoppingCart " + DB.GetNoLock() + " "
					+ String.Format(" where (Quantity IS NOT NULL and Quantity > 0) and CartType={0} and customerid={1}",(int)cart._cartType,cart._thisCustomer._customerID)
					+ RecurringOrderSql;
				DB.ExecuteSQL(sql4);
				
				String sql5 = "insert into orders_kitcart(OrderNumber,CartType,KitCartRecID,CustomerID,ShoppingCartRecID,ProductID,VariantID,ProductName,KitGroupID,KitGroupName,KitGroupIsRequired,KitItemID,KitItemName,KitItemPriceDelta,Quantity)"
					+ String.Format(" select {0},CartType,KitCartRecID,CustomerID,ShoppingCartRecID,KitCart.ProductID,KitCart.VariantID,Product.Name,KitCart.KitGroupID,KitGroup.Name,KitGroup.IsRequired,KitCart.KitItemID,KitItem.Name,KitItem.PriceDelta,Quantity",OrderNumber) 
					+ " FROM ((KitCart  " + DB.GetNoLock() + " INNER JOIN KitGroup  " + DB.GetNoLock() + " ON KitCart.KitGroupID = KitGroup.KitGroupID)"
					+ " INNER JOIN KitItem " + DB.GetNoLock() + " ON KitCart.KitItemID = KitItem.KitItemID)"
					+ " INNER JOIN Product  " + DB.GetNoLock() + " ON KitCart.ProductID = Product.ProductID"
					+ String.Format(" WHERE CartType={0} and customerid={1}",(int)cart._cartType,cart._thisCustomer._customerID)
					+ RecurringOrderSql;
				DB.ExecuteSQL(sql5);

				String sql6 = "insert into orders_customcart(OrderNumber,CartType,CustomCartRecID,CustomerID,PackID,ShoppingCartRecID,ProductSKU,ProductName,ProductWeight,ProductID,ProductTypeID,VariantID,Quantity,ChosenColor,ChosenColorSKUModifier,ChosenSize,ChosenSizeSKUModifier)"
					+ String.Format(" select {0},CartType,CustomCartRecID,CustomerID,PackID,ShoppingCartRecID,ProductSKU,ProductName,ProductWeight,ProductID,ProductTypeID,VariantID,Quantity,ChosenColor,ChosenColorSKUModifier,ChosenSize,ChosenSizeSKUModifier",OrderNumber) 
					+ String.Format(" from customcart where CartType={0} and customerid={1}",(int)cart._cartType,cart._thisCustomer._customerID)
					+ RecurringOrderSql;
				DB.ExecuteSQL(sql6);

				// update inventory:
				foreach(CartItem c in cart._cartItems)
				{
					if(Common.IsAPack(c.productID))
					{
						CustomCart ccart = new CustomCart(cart._thisCustomer._customerID,c.productID,cart._siteID);
						foreach(CustomItem ix in ccart._cartItems)
						{
							bool UseAdvancedInventoryMgmt = Common.ProductUsesAdvancedInventoryMgmt(ix.productID);
							if(!UseAdvancedInventoryMgmt)
							{
								String isql = "update productvariant set inventory=inventory-" + ix.quantity.ToString() + " where variantid=" + ix.variantID.ToString();
								DB.ExecuteSQL(isql);
							}
							else
							{
								Common.AdjustInventory(ix.variantID,ix.chosenSize,ix.chosenColor,ix.quantity);
							}
						}
					}
					else
					{
						bool UseAdvancedInventoryMgmt = Common.ProductUsesAdvancedInventoryMgmt(c.productID);
						if(!UseAdvancedInventoryMgmt)
						{
							String isql = "update productvariant set inventory=inventory-" + c.quantity.ToString() + " where variantid=" + c.variantID.ToString();
							DB.ExecuteSQL(isql);
						}
						else
						{
							Common.AdjustInventory(c.variantID,c.chosenSize,c.chosenColor,c.quantity);
						}
					}
				}

				// now set extended pricing info in the order cart to take into account all levels, quantities, etc...so the order object doesn't have to recompute cart stuff
				foreach(CartItem c in cart._cartItems)
				{
					if(!Common.IsAKit(c.productID) && !Common.IsAPack(c.productID))
					{
						int Q = c.quantity;
						bool IsOnSale = false;
						decimal pr = 0.0M;
						if(cart._cartType == CartTypeEnum.RecurringCart)
						{
							pr = c.price; // price is grandfathered
						}
						else
						{
							pr = Common.DetermineLevelPrice(c.variantID,cart._thisCustomer._customerLevelID,out IsOnSale);
						}
						pr = pr * Q;
						int ActiveDID = 0;
						Single DIDPercent = 0.0F;
						if(Common.CustomerLevelAllowsQuantityDiscounts(cart._thisCustomer._customerLevelID))
						{
							ActiveDID = Common.LookupActiveVariantQuantityDiscountID(c.variantID);
							DIDPercent = Common.GetDIDPercent(ActiveDID,Q);
							if(ActiveDID != 0 && DIDPercent != 0.0F)
							{
								pr = (decimal)(1.0-(DIDPercent/100.0)) * pr;
							}
						}
						decimal regular_pr = System.Decimal.Zero;
						decimal sale_pr = System.Decimal.Zero;
						decimal extended_pr = System.Decimal.Zero;
						if(cart._cartType != CartTypeEnum.RecurringCart)
						{
							regular_pr = Common.GetVariantPrice(c.variantID);
							sale_pr = Common.GetVariantSalePrice(c.variantID);
							extended_pr = Common.GetVariantExtendedPrice(c.variantID,cart._thisCustomer._customerLevelID);
						
							// adjust for color & size price modifirers:
							Decimal PrMod = Common.GetColorAndSizePriceDelta(c.chosenColor, c.chosenSize);
							if(PrMod != System.Decimal.Zero)
							{
								pr += (PrMod * Q);
							}
							if(pr < System.Decimal.Zero)
							{
								pr = System.Decimal.Zero; // never know what people will put in the modifiers :)
							}
						}
						else
						{
							regular_pr = c.price;
							sale_pr = System.Decimal.Zero;
							extended_pr = System.Decimal.Zero;
						}

						DB.ExecuteSQL("update orders_ShoppingCart set OrderedProductPrice=" + Localization.CurrencyStringForDB(pr) + ", OrderedProductRegularPrice=" + Localization.CurrencyStringForDB(regular_pr) + ", OrderedProductSalePrice=" + Localization.CurrencyStringForDB(sale_pr) + ", OrderedProductExtendedPrice=" + Localization.CurrencyStringForDB(extended_pr) + " where OrderNumber=" + OrderNumber.ToString() + " and ShoppingCartRecID=" + c.ShoppingCartRecordID.ToString());
					}
					else
					{
						int Q = c.quantity;
						decimal pr = c.price * Q;
						DB.ExecuteSQL("update orders_ShoppingCart set OrderedProductPrice=" + Localization.CurrencyStringForDB(pr) + " where OrderNumber=" + OrderNumber.ToString() + " and ShoppingCartRecID=" + c.ShoppingCartRecordID.ToString());
					}
				}

				// clear cart
				String RecurringVariantsList = Common.GetRecurringVariantsList();

				//Check for a Micropay purchase and upadte the Balance
				// Will _only_ update if PaymentClearedOn is set.
				Common.MicroPayBalanceUpdate(OrderNumber);

				//Check if they have any subscription extension products
				Common.SubscriptionUpdate(OrderNumber); 

				if(cart._cartType == CartTypeEnum.ShoppingCart)
				{
					// clear "normal" items out of cart, but leave any recurring items there:
					DB.ExecuteSQL("delete from customcart where CartType=" + ((int)CartTypeEnum.ShoppingCart).ToString() + Common.IIF(RecurringVariantsList.Length != 0, " and VariantID not in (" + RecurringVariantsList + ")","") + " and customerid=" + cart._thisCustomer._customerID.ToString());
					DB.ExecuteSQL("delete from kitcart where CartType=" + ((int)CartTypeEnum.ShoppingCart).ToString() + Common.IIF(RecurringVariantsList.Length != 0, " and VariantID not in (" + RecurringVariantsList + ")","") + " and customerid=" + cart._thisCustomer._customerID.ToString());
					DB.ExecuteSQL("delete from ShoppingCart where CartType=" + ((int)CartTypeEnum.ShoppingCart).ToString() + Common.IIF(RecurringVariantsList.Length != 0, " and VariantID not in (" + RecurringVariantsList + ")","") + " and customerid=" + cart._thisCustomer._customerID.ToString());
				}

				if(RecurringVariantsList.Length != 0)
				{
					// WE HAVE RECURRING ITEMS! They should be left in the cart, so the next recurring process will still find them.

					DateTime NextRecurringShipDate = System.DateTime.Now.AddMonths(1); // default just for safety, should never be used
					if (cart._originalRecurringOrderNumber == 0)
					{
						// this is a completely NEW recurring order, so set the recurring master parameters:
						String ThisOrderDate = Localization.ToNativeDateTimeString(System.DateTime.Now);
						foreach(CartItem c in cart._cartItems)
						{
							switch(c.recurringIntervalType)
							{
								case RecurringIntervalTypeEnum.Daily:
									NextRecurringShipDate = System.DateTime.Now.AddDays(c.recurringInterval);
									break;
								case RecurringIntervalTypeEnum.Weekly:
									NextRecurringShipDate = System.DateTime.Now.AddDays(7*c.recurringInterval);
									break;
								case RecurringIntervalTypeEnum.Monthly:
									NextRecurringShipDate = System.DateTime.Now.AddMonths(c.recurringInterval);
									break;
								case RecurringIntervalTypeEnum.Yearly:
									NextRecurringShipDate = System.DateTime.Now.AddYears(c.recurringInterval);
									break;
								default:
									NextRecurringShipDate = System.DateTime.Now.AddMonths(c.recurringInterval);
									break;
							}
							DB.ExecuteSQL("update ShoppingCart set UpdatedOn=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ", BillingAddressID=" + cart._billingAddress.AddressID.ToString() + ",ShippingAddressID=" + cart._shippingAddress.AddressID.ToString() + ", RecurringIndex=1, CartType=" + ((int)CartTypeEnum.RecurringCart).ToString() + ", CreatedOn=" + DB.DateQuote(ThisOrderDate) + ", NextRecurringShipDate=" + DB.DateQuote(Localization.ToNativeShortDateString(NextRecurringShipDate)) + ", OriginalRecurringOrderNumber=" + OrderNumber.ToString() + " where (OriginalRecurringOrderNumber is null or OriginalRecurringOrderNumber=0) and VariantID=" + c.variantID.ToString() + " and CartType=" + ((int)CartTypeEnum.ShoppingCart).ToString() + " and customerid=" + cart._thisCustomer._customerID.ToString());
						}
						DB.ExecuteSQL("update customcart set UpdatedOn=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ", CartType=" + ((int)CartTypeEnum.RecurringCart).ToString() + ", CreatedOn=" + DB.DateQuote(ThisOrderDate) + ", OriginalRecurringOrderNumber=" + OrderNumber.ToString() + " where (OriginalRecurringOrderNumber is null or OriginalRecurringOrderNumber=0) and VariantID in (" + RecurringVariantsList + ") and CartType=" + ((int)CartTypeEnum.ShoppingCart).ToString() + " and customerid=" + cart._thisCustomer._customerID.ToString());
						DB.ExecuteSQL("update kitcart set UpdatedOn=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ", CartType=" + ((int)CartTypeEnum.RecurringCart).ToString() + ", CreatedOn=" + DB.DateQuote(ThisOrderDate) + ", OriginalRecurringOrderNumber=" + OrderNumber.ToString() + " where (OriginalRecurringOrderNumber is null or OriginalRecurringOrderNumber=0) and VariantID in (" + RecurringVariantsList + ") and CartType=" + ((int)CartTypeEnum.ShoppingCart).ToString() + " and customerid=" + cart._thisCustomer._customerID.ToString());
					}
					else
					{
						// this is a REPEAT recurring order process:
						NextRecurringShipDate = System.DateTime.Now.AddMonths(1); // default just for safety, should never be used, as it should be reset below!

						// don't reset their ship dates to today plus interval, use what "would" have been the proper order date
						// for this order, and then add the interval (in case the store administrator is porcessing this order early!)
						DateTime ProperNextRecurringShipDateStartsOn = ((CartItem)cart._cartItems[0])._nextRecurringShipDate;
						if(ProperNextRecurringShipDateStartsOn == System.DateTime.MinValue)
						{
							// safety check:
							ProperNextRecurringShipDateStartsOn = System.DateTime.Now;
						}

						String ThisOrderDate = Localization.ToNativeDateTimeString(System.DateTime.Now);
						foreach(CartItem c in cart._cartItems)
						{
							switch(c.recurringIntervalType)
							{
								case RecurringIntervalTypeEnum.Daily:
									NextRecurringShipDate = ProperNextRecurringShipDateStartsOn.AddDays(c.recurringInterval);
									break;
								case RecurringIntervalTypeEnum.Weekly:
									NextRecurringShipDate = ProperNextRecurringShipDateStartsOn.AddDays(7*c.recurringInterval);
									break;
								case RecurringIntervalTypeEnum.Monthly:
									NextRecurringShipDate = ProperNextRecurringShipDateStartsOn.AddMonths(c.recurringInterval);
									break;
								case RecurringIntervalTypeEnum.Yearly:
									NextRecurringShipDate = ProperNextRecurringShipDateStartsOn.AddYears(c.recurringInterval);
									break;
								default:
									NextRecurringShipDate = ProperNextRecurringShipDateStartsOn.AddMonths(c.recurringInterval);
									break;
							}
							DB.ExecuteSQL("update ShoppingCart set UpdatedOn=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ", BillingAddressID=" + cart._billingAddress.AddressID.ToString() + ",ShippingAddressID=" + cart._shippingAddress.AddressID.ToString() + ", RecurringIndex=RecurringIndex+1, NextRecurringShipDate=" + DB.DateQuote(Localization.ToNativeDateTimeString(NextRecurringShipDate)) + " where originalrecurringordernumber=" + cart._originalRecurringOrderNumber.ToString() + " and VariantID=" + c.variantID.ToString() + " and CartType=" + ((int)CartTypeEnum.RecurringCart).ToString() + " and customerid=" + cart._thisCustomer._customerID.ToString());
						}
					}
				}

				// clear CouponCode
				if(Common.AppConfigBool("ClearCouponAfterOrdering"))
				{
					DB.ExecuteSQL("update customer set CouponCode=NULL where customerid=" + cart._thisCustomer._customerID.ToString());
				}

				//  now we have to update their quantity discount fields in their "order cart", so we have them available for later
				// receipts (e.g. you may delete that quantity discount table tomorrow, but the customer wants to get their receipt again
				// next month, and we would have to reproduce the exact order conditions that they had on order, and we couldn't do that
				// if the discount table has been deleted, unless we store the discount info along with the order)
				if(Common.CustomerLevelAllowsQuantityDiscounts(cart._thisCustomer._customerLevelID))
				{
					IDataReader rsq = DB.GetRS("select * from orders_ShoppingCart  " + DB.GetNoLock() + " where OrderNumber=" + OrderNumber.ToString());
					while(rsq.Read())
					{
						int RecID = DB.RSFieldInt(rsq,"ShoppingCartRecID");
						int ProductID = DB.RSFieldInt(rsq,"ProductID");
						int VariantID = DB.RSFieldInt(rsq,"VariantID");
						int ActiveDID = Common.LookupActiveVariantQuantityDiscountID(VariantID);
						String ActiveDIDName = Common.GetQuantityDiscountName(ActiveDID);
						int Q = DB.RSFieldInt(rsq,"Quantity");
						Single DIDPercent = Common.LookupVariantQuantityDiscountPercent(VariantID,Q);
						DB.ExecuteSQL("update orders_ShoppingCart set OrderedProductQuantityDiscountName=" + DB.SQuote(ActiveDIDName) + ", OrderedProductQuantityDiscountID=" + ActiveDID.ToString() + ", OrderedProductQuantityDiscountPercent=" + Localization.SingleStringForDB(DIDPercent) + " where OrderNumber=" + OrderNumber.ToString() + " and ProductID=" + ProductID.ToString() + " and VariantID=" + VariantID.ToString() + " and ShoppingCartRecID=" + RecID.ToString());
					}
					rsq.Close();
				}

				// now update their CustomerLevel info in the order record, if necessary:
				if(cart._thisCustomer._customerLevelID != 0)
				{
					IDataReader rs_l = DB.GetRS("select * from CustomerLevel  " + DB.GetNoLock() + " where CustomerLevelID=" + cart._thisCustomer._customerLevelID.ToString());
					if(rs_l.Read())
					{
						StringBuilder sql_l = new StringBuilder(5000);
						sql_l.Append("update orders set ");
						sql_l.Append("LevelID=" + cart._thisCustomer._customerLevelID.ToString() + ",");
						sql_l.Append("LevelName=" + DB.SQuote(Common.GetCustomerLevelName(cart._thisCustomer._customerLevelID)) + ",");
						sql_l.Append("LevelDiscountPercent=" + DB.RSFieldSingle(rs_l,"LevelDiscountPercent").ToString() + ",");
						sql_l.Append("LevelDiscountAmount=" + DB.RSFieldDecimal(rs_l,"LevelDiscountAmount").ToString() + ",");
						sql_l.Append("LevelHasFreeShipping=" + Common.IIF(DB.RSFieldBool(rs_l,"LevelHasFreeShipping") , 1 , 0).ToString() + ",");
						sql_l.Append("LevelAllowsQuantityDiscounts=" + Common.IIF(DB.RSFieldBool(rs_l,"LevelAllowsQuantityDiscounts") , 1 , 0).ToString() + ",");
						sql_l.Append("LevelHasNoTax=" + Common.IIF(DB.RSFieldBool(rs_l,"LevelHasNoTax") , 1 , 0).ToString() + ",");
						sql_l.Append("LevelAllowsCoupons=" + Common.IIF(DB.RSFieldBool(rs_l,"LevelAllowsCoupons") , 1 , 0).ToString() + ",");
						sql_l.Append("LevelDiscountsApplyToExtendedPrices=" + Common.IIF(DB.RSFieldBool(rs_l,"LevelDiscountsApplyToExtendedPrices") , 1 ,0).ToString() + " ");
						sql_l.Append("where OrderNumber=" + OrderNumber.ToString());
						DB.ExecuteSQL(sql_l.ToString());
					}
					rs_l.Close();
				}
			}
			return status;
		}

		private static String ProcessCard(ShoppingCart cart, int OrderNumber, String CardName, String CardType, String CardNumber, String CardExtraCode, String CardExpirationMonth, String CardExpirationYear, Decimal OrderTotal, bool useLiveTransactions, String CAVV, String ECI, String XID, out bool PaymentCleared, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommand)
		{
			String GW = Common.AppConfig("PaymentGateway").Trim().ToUpper();
			String Status = "NO GATEWAY SET, GATEWAY='" + GW + "'";

			if(CardType.ToUpper() == "PAYPAL")
			{
				Status = PayPal.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID,  out PaymentCleared,  out AVSResult,  out AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
			}
			else
			{
				switch(GW)
				{
					case "EFSNET":
						Status = EFSNet.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					case "IONGATE":
						Status = IonGate.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					case "VERISIGN":
						Status = Verisign.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					case "AUTHORIZENET":
						Status = AuthorizeNet.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					case "PAYPALPRO":
						Status = PayPalPro.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					case "2CHECKOUT":
						Status = TwoCheckout.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					case "EPROCESSINGNETWORK":
						Status = eProcessingNetwork.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					case "CYBERSOURCE":
						Status = Cybersource.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					case "NETBILLING":
						Status = NetBilling.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					case "PAYMENTECH":
						Status = Paymentech.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					case "PAYFUSE":
						Status = PayFuse.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					case "ITRANSACT":
						Status = ITransact.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					case "WORLDPAY JUNIOR":
						Status = Worldpay.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					case "WORLDPAY":
						Status = Worldpay.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					case "LINKPOINT":
						Status = Linkpoint.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					case "TRANSACTIONCENTRAL":
						Status = TransactionCentral.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					case "MERCHANTCENTRAL":
						Status = TransactionCentral.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					case "YOURPAY":
						Status = YourPay.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					case "MANUAL":
						Status = ManualAuth.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
					default:
						Status = ManualAuth.ProcessCard(OrderNumber,cart,CardName,CardType,CardNumber,CardExtraCode,CardExpirationMonth,CardExpirationYear,OrderTotal,useLiveTransactions,cart._billingAddress,cart._shippingAddress,CAVV,ECI,XID, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
				}
			}
			return Status;
		}

		private static String ProcessECheck(ShoppingCart cart, int OrderNumber, String eCheckBankABACode,String eCheckBankAccountNumber,String eCheckBankAccountType,String eCheckBankName,String eCheckBankAccountName, Decimal OrderTotal, bool useLiveTransactions, String CAVV, String ECI, String XID, out bool PaymentCleared, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommand)
		{
			String GW = Common.AppConfig("PaymentGateway").Trim().ToUpper();
			String Status = "NO GATEWAY SET OR A ECHECKS NOT IMPLEMENTED FOR THAT GATEWAY (GATEWAY=" + GW + ")";

			PaymentCleared = false;
			AVSResult = String.Empty;
			AuthorizationResult = String.Empty;
			AuthorizationCode = String.Empty;
			AuthorizationTransID = String.Empty;
			TransactionCommand = String.Empty;

			if(Common.AppConfig("TransactionMode").ToUpper() != "AUTH CAPTURE")
			{
				Status = "ECHECKS CAN ONLY BE USED IN AUTH CAPTURE MODE. Consult your AppConfig:TransactionMode Settings";
			}
			else
			{
				switch(GW)
				{
					case "AUTHORIZENET":
						Status = AuthorizeNet.ProcessECheck(OrderNumber,cart,eCheckBankABACode,eCheckBankAccountNumber,eCheckBankAccountType,eCheckBankName,eCheckBankAccountName,OrderTotal,cart._billingAddress,cart._shippingAddress, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
						//V3_9
					case "EPROCESSINGNETWORK":
						Status = eProcessingNetwork.ProcessECheck(OrderNumber,cart,eCheckBankABACode,eCheckBankAccountNumber,eCheckBankAccountType,eCheckBankName,eCheckBankAccountName,OrderTotal,cart._billingAddress,cart._shippingAddress, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
						//V3_9
					case "ITRANSACT":
						Status = ITransact.ProcessECheck(OrderNumber,cart,eCheckBankABACode,eCheckBankAccountNumber,eCheckBankAccountType,eCheckBankName,eCheckBankAccountName,OrderTotal,cart._billingAddress,cart._shippingAddress, out  PaymentCleared, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand);
						break;
				}
			}
			return Status;
		}

	}
}
