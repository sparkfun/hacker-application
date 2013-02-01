// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Configuration;
using System.Data;
using System.Text;
using System.Collections;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

namespace AspDotNetStorefrontCommon
{
	/// <summary>
	/// Summary description for Order.
	/// </summary>
	/// 

	public class Order
	{
		public int _customerID;
		public CartTypeEnum _cartType;
		public int _orderNumber;
		public bool _isNew;
		public bool _isEmpty;
		public Single _orderWeight;
		public String _transactionState;
		public String _AVSResult;
		public DateTime _paymentClearedOn;
		public String _paymentGateway;
		public String _paymentMethod;
		public String _orderNotes;
		public String _orderOptions;
		public String _PONumber;
		public String _localeSetting;
		public DateTime _receiptEMailSentOn;
		public DateTime _downloadEMailSentOn;
		public String _shippingTrackingNumber;
		public String _shippedVIA;
		public String _customerServiceNotes;
		public Coupon _coupon;
		public ArrayList _cartItems;
		public AddressInfo _shippingAddress;
		public AddressInfo _billingAddress;
		public Single _taxRate;
		public int _shippingMethodID;
		public String _shippingMethod;
		public int _shippingCalculationID;
		public DateTime _orderDate;
		public Decimal _total;
		public Decimal _subTotal;
		public Decimal _taxTotal;
		public Decimal _shippingTotal;
		public String _password;
		public String _cardType;
		public String _cardNumber;
		public String _cardExtraCode;
		public String _cardName;
		public String _cardExpirationMonth;
		public String _cardExpirationYear;

		public String _eCheckBankABACode;
		public String _eCheckBankAccountNumber;
		public String _eCheckBankAccountName;
		public String _eCheckBankAccountType;
		public String _eCheckBankName;

		public String _affiliateID;
		public String _email;
		public int _siteID;
		public String _storeVersion;
		public int _levelID;
		public String _levelName;
		public decimal _levelDiscountAmount;
		public Single _levelDiscountPercent;
		public bool _levelHasFreeShipping;
		public bool _levelAllowsQuantityDiscounts;
		public bool _levelHasNoTax;
		public bool _levelAllowsCoupons;
		public bool _levelDiscountsApplyToExtendedPrices;

		public Order() {} // for serialization ONLY!

		public Order(int OrderNumber)
		{
			_orderNumber = OrderNumber;
			LoadFromDB();
		}

		public Order(String OrderNumber)
		{
			_orderNumber = Localization.ParseUSInt(OrderNumber);
			LoadFromDB();
		}

		private void LoadFromDB()
		{
			_isEmpty = true;
			String GetOrderSQL = "SELECT Orders.IsNew, Orders.CartType, Orders_ShoppingCart.ProductID, Orders.ShippedVia, Orders.OrderWeight, Orders.eCheckBankName, Orders.ECheckBankAccountNumber, orders.ECheckBankAccountType, orders.ECheckBankABACode, Orders.ECheckBankAccountName, Orders.ShippingTrackingNumber, Orders.TransactionState, Orders.AVSResult, Orders_ShoppingCart.SubscriptionMonths, Orders_ShoppingCart.ShoppingCartRecID,Orders.PaymentClearedOn, Orders.PaymentGateway, Orders.PaymentMethod,Orders.CustomerServiceNotes,Orders.OrderNotes, Orders.OrderOptions, Orders.PONumber,Orders.LocaleSetting,Orders.DownloadEmailSentOn,Orders.ReceiptEmailSentOn,Orders_ShoppingCart.IsTaxable, Orders_ShoppingCart.IsShipSeparately, Orders_ShoppingCart.IsDownload, Orders_ShoppingCart.DownloadLocation, Orders_ShoppingCart.IsSecureAttachment, Orders.StoreVersion, Orders.LevelID, Orders.LevelName, Orders.LevelDiscountPercent, Orders.LevelDiscountAmount, Orders.LevelHasFreeShipping, Orders.LevelAllowsQuantityDiscounts, Orders.LevelHasNoTax, Orders.LevelAllowsCoupons, Orders.LevelDiscountsApplyToExtendedPrices, ";
			GetOrderSQL += " Orders_ShoppingCart.VariantID, Orders.QuoteCheckout, Orders_ShoppingCart.Quantity,Orders_ShoppingCart.ChosenColor,Orders_ShoppingCart.ChosenColorSKUModifier,Orders_ShoppingCart.ChosenSize,Orders_ShoppingCart.ChosenSizeSKUModifier,Orders_ShoppingCart.TextOption,Orders_ShoppingCart.OrderedProductQuantityDiscountID,Orders_ShoppingCart.OrderedProductQuantityDiscountName,Orders_ShoppingCart.OrderedProductQuantityDiscountPercent,Orders_ShoppingCart.OrderedProductDescription, Orders_ShoppingCart.OrderedProductSKU, Orders_ShoppingCart.OrderedProductPrice,Orders.CustomerID, Orders.OrderNumber, Orders.CustomerGUID, Orders.[Password], Orders.AuthorizationResult,Orders.LastName, Orders.SiteID, Orders.FirstName, Orders.Email, Orders.Notes, Orders.BillingEqualsShipping, Orders.BillingLastName,Orders.BillingFirstName, Orders.BillingCompany, Orders.BillingAddress1, Orders.BillingAddress2, Orders.BillingSuite, Orders.BillingCity, Orders.BillingState,Orders.BillingZip, Orders.BillingCountry, Orders.BillingPhone, Orders.ShippingLastName, Orders.ShippingFirstName, Orders.ShippingCompany,Orders.ShippingAddress1, Orders.ShippingAddress2, Orders.ShippingSuite, Orders.ShippingCity, Orders.ShippingState, Orders.ShippingZip, Orders.ShippingCountry,Orders.ShippingMethodID, Orders.ShippingMethod, Orders.ShippingCalculationID,Orders.ShippingPhone, Orders.Phone, Orders.RegisterDate, Orders.AffiliateID, Orders.CouponCode, Orders.CouponDescription, Orders.CouponDiscountAmount, Orders.CouponDiscountPercent, Orders.CouponIncludesFreeShipping, Orders.OkToEmail, Orders.Deleted,Orders.CardType, Orders.CardName, Orders.CardNumber, Orders.CardExpirationMonth, Orders.CardExpirationYear, Orders.OrderSubtotal, Orders.OrderTax, Orders.OrderShippingCosts, Orders.OrderTotal, Orders.AuthorizationCode, Orders.OrderDate, Orders.TaxRate ";
			GetOrderSQL += " FROM Orders " + DB.GetNoLock() + " left outer join orders_ShoppingCart " + DB.GetNoLock() + " ON Orders_ShoppingCart.OrderNumber = Orders.OrderNumber WHERE Orders.OrderNumber = " + _orderNumber.ToString();
			IDataReader rs = DB.GetRS(GetOrderSQL);
			_cartItems = new ArrayList(50);
			int i = 0;
			while(rs.Read())
			{
				_siteID = DB.RSFieldInt(rs,"SiteID");
				if(_siteID == 0)
				{
					_siteID = 1; // fix it up to something!
				}
				_isEmpty = false;
				_isNew = DB.RSFieldBool(rs,"IsNew");
				CartItem newItem = new CartItem();
				newItem.ShoppingCartRecordID = DB.RSFieldInt(rs,"ShoppingCartRecID");
				newItem.productID = DB.RSFieldInt(rs,"ProductID");
				newItem.variantID = DB.RSFieldInt(rs,"VariantID");
				newItem.productName = DB.RSField(rs,"OrderedProductDescription");
				newItem.SKU = DB.RSField(rs,"OrderedProductSKU");
				newItem.quantity = DB.RSFieldInt(rs,"Quantity");
				newItem.chosenColor = DB.RSField(rs,"ChosenColor");
				newItem.chosenColorSKUModifier = DB.RSField(rs,"ChosenColorSKUModifier");
				newItem.chosenSize = DB.RSField(rs,"ChosenSize");
				newItem.chosenSizeSKUModifier = DB.RSField(rs,"ChosenSizeSKUModifier");
				newItem.textOption = DB.RSField(rs,"TextOption");
				newItem.weight = 0.0F; // N/A
				newItem.subscriptionMonths = DB.RSFieldInt(rs,"SubscriptionMonths");
				newItem.price = DB.RSFieldDecimal(rs,"OrderedProductPrice");
				newItem.QuantityDiscountID = DB.RSFieldInt(rs,"OrderedProductQuantityDiscountID");
				newItem.QuantityDiscountName = DB.RSField(rs,"OrderedProductQuantityDiscountName");
				newItem.QuantityDiscountPercent = DB.RSFieldSingle(rs,"OrderedProductQuantityDiscountPercent");
				newItem.isTaxable = DB.RSFieldBool(rs,"IsTaxable");
				newItem.isShipSeparately = DB.RSFieldBool(rs,"IsShipSeparately");
				newItem.isDownload = DB.RSFieldBool(rs,"IsDownload");
				newItem.downloadLocation = DB.RSField(rs,"DownloadLocation");
				newItem.isSecureAttachment = DB.RSFieldBool(rs,"IsSecureAttachment");
				_cartItems.Add(newItem);

				if(i == 0)
				{
					_total = DB.RSFieldDecimal(rs,"OrderTotal");
					_subTotal = DB.RSFieldDecimal(rs,"OrderSubtotal");
					_taxTotal = DB.RSFieldDecimal(rs,"OrderTax");
					_shippingTotal = DB.RSFieldDecimal(rs,"OrderShippingCosts");
					_orderDate = DB.RSFieldDateTime(rs,"OrderDate");
					_password = DB.RSField(rs,"Password");
					_paymentGateway = DB.RSField(rs,"PaymentGateway");
					_cardType = DB.RSField(rs,"CardType");
					_cardNumber = DB.RSField(rs,"CardNumber");
					_cardExtraCode = String.Empty; // NOT ALLOWED TO STORE
					_cardName = DB.RSField(rs,"CardName");
					_cardExpirationMonth = DB.RSField(rs,"CardExpirationMonth");
					_cardExpirationYear = DB.RSField(rs,"CardExpirationYear");

					_eCheckBankAccountName = DB.RSField(rs,"eCheckBankAccountName");
					_eCheckBankAccountType = DB.RSField(rs,"eCheckBankAccountType");
					_eCheckBankName = DB.RSField(rs,"eCheckBankName");
					_eCheckBankABACode = DB.RSField(rs,"eCheckBankABACode");
					_eCheckBankAccountNumber = DB.RSField(rs,"eCheckBankAccountNumber");

					_affiliateID = DB.RSFieldInt(rs,"AffiliateID").ToString();
					_customerID = DB.RSFieldInt(rs,"CustomerID");
					_email = DB.RSField(rs,"Email");
					_cartType = (CartTypeEnum)DB.RSFieldInt(rs,"CartType");

					_orderWeight = DB.RSFieldSingle(rs,"OrderWeight");
					_transactionState = DB.RSField(rs,"TransactionState");
					_AVSResult = DB.RSField(rs,"AVSResult");
					_paymentClearedOn = DB.RSFieldDateTime(rs,"PaymentClearedOn");
					_paymentMethod = DB.RSField(rs,"PaymentMethod");
					_orderNotes = DB.RSField(rs,"OrderNotes");
					_orderOptions = DB.RSField(rs,"OrderOptions");
					_PONumber = DB.RSField(rs,"PONumber");
					_localeSetting = DB.RSField(rs,"LocaleSetting");
					_receiptEMailSentOn = DB.RSFieldDateTime(rs,"ReceiptEMailSentOn");
					_downloadEMailSentOn = DB.RSFieldDateTime(rs,"DownloadEMailSentOn");
					_shippingTrackingNumber = DB.RSField(rs,"ShippingTrackingNumber");
					_shippedVIA = DB.RSField(rs,"ShippedVIA");
					_customerServiceNotes = DB.RSField(rs,"CustomerServiceNotes");

					_coupon.code = DB.RSField(rs,"CouponCode");
					_coupon.description = DB.RSField(rs,"CouponDescription");
					_coupon.expirationDate = System.DateTime.MinValue; // DB.RSFieldDateTime(rs,"CouponExpirationDate");
					_coupon.discountAmount = DB.RSFieldDecimal(rs,"CouponDiscountAmount");
					_coupon.discountPercent = DB.RSFieldSingle(rs,"CouponDiscountPercent");
					_coupon.includesFreeShipping = DB.RSFieldBool(rs,"CouponIncludesFreeShipping");

					_shippingAddress.firstName = DB.RSField(rs,"ShippingFirstName");
					_shippingAddress.lastName = DB.RSField(rs,"ShippingLastName");
					_shippingAddress.company = DB.RSField(rs,"ShippingCompany");
					_shippingAddress.address1 = DB.RSField(rs,"ShippingAddress1");
					_shippingAddress.address2 = DB.RSField(rs,"ShippingAddress2");
					_shippingAddress.suite = DB.RSField(rs,"ShippingSuite");
					_shippingAddress.city = DB.RSField(rs,"ShippingCity");
					_shippingAddress.state = DB.RSField(rs,"ShippingState");
					_shippingAddress.zip = DB.RSField(rs,"ShippingZip");
					_shippingAddress.country = DB.RSField(rs,"ShippingCountry");
					_shippingAddress.phone = DB.RSField(rs,"ShippingPhone");
					_shippingAddress.email = DB.RSField(rs,"EMail");

					_billingAddress.firstName = DB.RSField(rs,"BillingFirstName");
					_billingAddress.lastName = DB.RSField(rs,"BillingLastName");
					_billingAddress.company = DB.RSField(rs,"BillingCompany");
					_billingAddress.address1 = DB.RSField(rs,"BillingAddress1");
					_billingAddress.address2 = DB.RSField(rs,"BillingAddress2");
					_billingAddress.suite = DB.RSField(rs,"BillingSuite");
					_billingAddress.city = DB.RSField(rs,"BillingCity");
					_billingAddress.state = DB.RSField(rs,"BillingState");
					_billingAddress.zip = DB.RSField(rs,"BillingZip");
					_billingAddress.country = DB.RSField(rs,"BillingCountry");
					_billingAddress.phone = DB.RSField(rs,"BillingPhone");
					_billingAddress.email = DB.RSField(rs,"EMail");

					_taxRate = DB.RSFieldSingle(rs,"TaxRate");
					_shippingCalculationID = DB.RSFieldInt(rs,"ShippingCalculationID");
					_shippingMethodID = DB.RSFieldInt(rs,"ShippingMethodID");
					_shippingMethod = DB.RSField(rs,"ShippingMethod");
					if(_shippingMethod.Length == 0)
					{
						_shippingMethod = Common.GetShippingMethodName(_shippingMethodID); // for old order compatibility
					}
					_storeVersion = DB.RSField(rs,"StoreVersion");
					_levelID = DB.RSFieldInt(rs,"LevelID");
					_levelName = DB.RSField(rs,"LevelName");
					_levelDiscountAmount = DB.RSFieldDecimal(rs,"LevelDiscountAmount");
					_levelDiscountPercent = DB.RSFieldSingle(rs,"LevelDiscountPercent");
					_levelHasFreeShipping = DB.RSFieldBool(rs,"LevelHasFreeShipping");
					_levelAllowsQuantityDiscounts = DB.RSFieldBool(rs,"LevelAllowsQuantityDiscounts");
					_levelHasNoTax = DB.RSFieldBool(rs,"LevelHasNoTax");
					_levelAllowsCoupons = DB.RSFieldBool(rs,"LevelAllowsCoupons");
					_levelDiscountsApplyToExtendedPrices = DB.RSFieldBool(rs,"LevelDiscountsApplyToExtendedPrices");

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

		public Decimal SubTotal(bool includeDiscount)
		{
			// NOTE: when the order record was created the total fields already reflect all discounts, quantities, coupons, and levels:
			return _subTotal;
		}

		public Decimal TaxTotal(bool includeDiscount)
		{
			// NOTE: when the order record was created the total fields already reflect all discounts, quantities, coupons, and levels:
			return _taxTotal;
		}

		public Decimal ShippingTotal(bool includeDiscount)
		{
			// NOTE: when the order record was created the total fields already reflect all discounts, quantities, coupons, and levels:
			return _shippingTotal;
		}

		public Decimal Total(bool includeDiscount)
		{
			return _total;
		}

		private String ProcessReceiptTokens(String R, String ShowCCPWD, bool nocc)
		{
			R = R.Replace("%STOREFRONT_VERSION%",_storeVersion);
			R = R.Replace("%AFFILIATE_ID%",_affiliateID);
			R = R.Replace("%STORE_NAME%",Common.AppConfig("StoreName"));
			R = R.Replace("%STORE_URL%",Common.GetStoreHTTPLocation(false));
			R = R.Replace("%ORDER_NUMBER%",_orderNumber.ToString());
			R = R.Replace("%CUSTOMER_ID%",_customerID.ToString());
			R = R.Replace("%ORDER_DATE%",_orderDate.ToString());
			R = R.Replace("%ORDER_STATUS%",""); // TBD
			String st = "Will Quote";
			if(_paymentMethod.ToUpper() != "REQUEST QUOTE")
			{
				st = Localization.CurrencyStringForDisplay(SubTotal(true));
			}
			R = R.Replace("%ORDER_SUBTOTAL%",st);

			if(_taxRate != 0.0F && TaxTotal(true) > System.Decimal.Zero)
			{
				R = R.Replace("%TAX_PERCENT%","(" + String.Format("{0:P}",_taxRate/100.0F) + ")");
			}
			else
			{
				R = R.Replace("%TAX_PERCENT%","");
			}
			st = "Will Quote";
			if(_paymentMethod.ToUpper() != "REQUEST QUOTE")
			{
				st = Localization.CurrencyStringForDisplay(TaxTotal(true));
			}
			R = R.Replace("%ORDER_TAX%",st);


			String ShowShipText = _shippingMethod;
			// strip out RT shipping cost, if any:
			if(ShowShipText.IndexOf("|") != -1)
			{
				String[] ss = ShowShipText.Split('|');
				try
				{
					ShowShipText = ss[0].Trim();
				}
				catch {}
			}
			if(this.IsAllDownloadComponents())
			{
				ShowShipText = "FREE SHIPPING (Download)";
			}
			if(_shippingCalculationID == 4)
			{
				ShowShipText = "FREE SHIPPING";
			}

			R = R.Replace("%SHIPPING_METHOD%",ShowShipText);
			st = "Will Quote";
			if(_paymentMethod.ToUpper() != "REQUEST QUOTE")
			{
				st = Localization.CurrencyStringForDisplay(ShippingTotal(true));
			}
			R = R.Replace("%ORDER_SHIPPING%",st);
			st = "Will Quote";
			if(_paymentMethod.ToUpper() != "REQUEST QUOTE")
			{
				st = Localization.CurrencyStringForDisplay(Total(true));
			}
			R = R.Replace("%ORDER_TOTAL%",st);

			R = R.Replace("%ORDER_OPTIONS%",_orderOptions);
	
			st = "None";
			if(_orderNotes.Length != 0)
			{
				st = HttpContext.Current.Server.HtmlEncode(_orderNotes);
			}
			R = R.Replace("%ORDER_NOTES%",st);
			if(Common.AppConfigBool("ShowCustomerServiceNotesInReceipts"))
			{
				R = R.Replace("%CUSTOMER_SERVICE_NOTES%",Common.IIF(_customerServiceNotes.Length == 0 , "None" , _customerServiceNotes));
			}
			else
			{
				R = R.Replace("%CUSTOMER_SERVICE_NOTES%","None");
			}

			if(_levelID != 0)
			{
				StringBuilder LevelInfo = new StringBuilder(1000);
				LevelInfo.Append(_levelName);
				if(ShowCCPWD == Common.AppConfig("OrderShowCCPwd"))
				{
					LevelInfo.Append(": ");
					LevelInfo.Append("DiscountPercent=" + _levelDiscountPercent.ToString() + "%, ");
					LevelInfo.Append("DiscountAmount=" + Localization.CurrencyStringForDisplay(_levelDiscountAmount) + ", ");
					LevelInfo.Append("HasFreeShipping=" + _levelHasFreeShipping.ToString() + ", ");
					LevelInfo.Append("AllowsQuantityDiscounts=" + _levelAllowsQuantityDiscounts.ToString() + ", ");
					LevelInfo.Append("HasNoTax=" + _levelHasNoTax.ToString() + ", ");
					LevelInfo.Append("AllowsCoupons=" + _levelAllowsCoupons.ToString() + ", ");
					LevelInfo.Append("DiscountsApplyToExtendedPrices=" + _levelDiscountsApplyToExtendedPrices.ToString());
				}
				R = R.Replace("%CUSTOMER_LEVEL_INFO%",LevelInfo.ToString());
			}
			else
			{
				R = R.Replace("%CUSTOMER_LEVEL_INFO%","");
			}

			if(_coupon.code.Length != 0)
			{
				R = R.Replace("%COUPON_INFO%",HttpContext.Current.Server.HtmlEncode(_coupon.code) + " (" + _coupon.description + "), Free Shipping: " + Common.IIF(_coupon.includesFreeShipping , "Yes" , "No") + ", Discount Amount: " + Common.IIF(_coupon.discountAmount != System.Decimal.Zero , Localization.CurrencyStringForDisplay(_coupon.discountAmount),"N/A") + ", Discount Percent: " + Common.IIF(_coupon.discountPercent != 0 , _coupon.discountPercent.ToString() + "%" , "N/A"));
			}
			else
			{
				R = R.Replace("%COUPON_INFO%","None");
			}

			R = R.Replace("%EMAIL%",HttpContext.Current.Server.HtmlEncode(_email));

			String PMT = _paymentMethod;
			if(_paymentMethod == "MICROPAY")
			{
				PMT = Common.AppConfig("Micropay.Prompt");
			}

			R = R.Replace("%PAYMENT_METHOD%",HttpContext.Current.Server.HtmlEncode(PMT));
			if(_paymentMethod.ToUpper() == "REQUEST QUOTE")
			{
				R = R.Replace("%PAYMENT_STATUS%","Request For Quote");
			}
			else
			{
				R = R.Replace("%PAYMENT_STATUS%",Common.IIF(_paymentClearedOn != System.DateTime.MinValue , "Payment Cleared" , "Pending"));
			}

			R = R.Replace("%SHIPPING_FIRST_NAME%",HttpContext.Current.Server.HtmlEncode(_shippingAddress.firstName));
			R = R.Replace("%SHIPPING_LAST_NAME%",HttpContext.Current.Server.HtmlEncode(_shippingAddress.lastName));
			R = R.Replace("%SHIPPING_FULL_NAME%",HttpContext.Current.Server.HtmlEncode((_shippingAddress.firstName + " " + _shippingAddress.lastName).Trim()));
			String ShipAddr = _shippingAddress.address1;
			if(_shippingAddress.address2.Length != 0)
			{
				ShipAddr += "<br>" + _shippingAddress.address2;
			}
			if(_shippingAddress.suite.Length != 0)
			{
				ShipAddr += ", " + _shippingAddress.suite;
			}
			R = R.Replace("%SHIPPING_ADDRESS%",HttpContext.Current.Server.HtmlEncode(ShipAddr));
			R = R.Replace("%SHIPPING_COMPANY%",HttpContext.Current.Server.HtmlEncode(_shippingAddress.company));
			R = R.Replace("%SHIPPING_CITY%",HttpContext.Current.Server.HtmlEncode(_shippingAddress.city));
			R = R.Replace("%SHIPPING_STATE%",HttpContext.Current.Server.HtmlEncode(_shippingAddress.state));
			R = R.Replace("%SHIPPING_ZIP%",HttpContext.Current.Server.HtmlEncode(_shippingAddress.zip));
			R = R.Replace("%SHIPPING_COUNTRY%",HttpContext.Current.Server.HtmlEncode(_shippingAddress.country));
			R = R.Replace("%SHIPPING_PHONE%",HttpContext.Current.Server.HtmlEncode(_shippingAddress.phone));

			R = R.Replace("%BILLING_FIRST_NAME%",HttpContext.Current.Server.HtmlEncode(_billingAddress.firstName));
			R = R.Replace("%BILLING_LAST_NAME%",HttpContext.Current.Server.HtmlEncode(_billingAddress.lastName));
			R = R.Replace("%BILLING_FULL_NAME%",HttpContext.Current.Server.HtmlEncode((_billingAddress.firstName + " " + _billingAddress.lastName).Trim()));
			String BilAddr = _billingAddress.address1;
			if(_billingAddress.address2.Length != 0)
			{
				BilAddr += "<br>" + _billingAddress.address2;
			}
			if(_billingAddress.suite.Length != 0)
			{
				BilAddr += ", " + _billingAddress.suite;
			}
			R = R.Replace("%BILLING_ADDRESS%",HttpContext.Current.Server.HtmlEncode(BilAddr));
			R = R.Replace("%BILLING_COMPANY%",HttpContext.Current.Server.HtmlEncode(_billingAddress.company));
			R = R.Replace("%BILLING_CITY%",HttpContext.Current.Server.HtmlEncode(_billingAddress.city));
			R = R.Replace("%BILLING_STATE%",HttpContext.Current.Server.HtmlEncode(_billingAddress.state));
			R = R.Replace("%BILLING_ZIP%",HttpContext.Current.Server.HtmlEncode(_billingAddress.zip));
			R = R.Replace("%BILLING_COUNTRY%",HttpContext.Current.Server.HtmlEncode(_billingAddress.country));
			R = R.Replace("%BILLING_PHONE%",HttpContext.Current.Server.HtmlEncode(_billingAddress.phone));
			R = R.Replace("%LOCALESETTING%",_localeSetting);

			switch(_paymentMethod.Replace(" ","").Trim().ToUpper())
			{
				case "CREDITCARD":
					R = R.Replace("%CARD_TYPE%",_cardType);
					R = R.Replace("%CARD_NAME%",_cardName);
					String ctmp = _cardNumber;
					if(_cardNumber.Length != 0 && _cardNumber != "XXXXXXXXXXXX")
					{
						ctmp = Common.UnmungeString(_cardNumber);
					}
					if(ShowCCPWD == Common.AppConfig("OrderShowCCPwd") && !nocc)
					{
						// authorized to view this info:
						if(_cardType.ToUpper() == "PayPal".ToUpper())
						{
							ctmp = "PayPal";
						}
						else
						{
							ctmp = ctmp;
						}
					}
					else
					{
						// not authorized to view this info:
						if(_cardNumber.Length == 0 || _cardNumber == "XXXXXXXXXXXX")
						{
							ctmp = "";
						}
						else
						{
							if(_cardType.ToUpper() == "PayPal".ToUpper())
							{
								ctmp = "PayPal";
							}
							else
							{
								String CardSuffix = "XXXX";
								try
								{
									CardSuffix = ctmp.Substring(ctmp.Length-4,4);
								}
								catch {}
								ctmp = "XXXXXXXX" + CardSuffix;
							}
						}
					}
					R = R.Replace("%CARD_NUMBER%",ctmp);
					if(_cardNumber.Length == 0 || _cardNumber == "XXXXXXXXXXXX")
					{
						R = R.Replace("%CARD_EXPIRATION%","");
					}
					else
					{
						if(_cardType.ToUpper() == "PayPal".ToUpper())
						{
							R = R.Replace("%CARD_EXPIRATION%","");
						}
						else
						{
							R = R.Replace("%CARD_EXPIRATION%",_cardExpirationMonth + "/" + _cardExpirationYear);
						}
					}
					break;
				case "PURCHASEORDER":
					R = R.Replace("%CARD_TYPE%","N/A (Purchase Order: " + _PONumber + ")");
					R = R.Replace("%CARD_NUMBER%","N/A");
					R = R.Replace("%CARD_NAME%","N/A");
					R = R.Replace("%CARD_EXPIRATION%","N/A");
					break;
				case "ECHECK":
					R = R.Replace("%BANK_ABA_CODE%","N/A (Purchase Order: " + _PONumber + ")");
					R = R.Replace("%BANK_ACCOUNT_NUM%","N/A");
					R = R.Replace("%BANK_ACCOUNT_TYPE%","N/A");
					R = R.Replace("%BANK_NAME%","N/A");
					R = R.Replace("%BANK_ACCOUNT_NAME%","N/A");
					break;
				case "PAYPAL":
					R = R.Replace("%CARD_TYPE%","N/A (PayPal)");
					R = R.Replace("%CARD_NUMBER%","N/A");
					R = R.Replace("%CARD_NAME%","N/A");
					R = R.Replace("%CARD_EXPIRATION%","N/A");
					break;
				case "REQUESTQUOTE":
					R = R.Replace("%CARD_TYPE%","N/A (Request For Quote)");
					R = R.Replace("%CARD_NUMBER%","N/A");
					R = R.Replace("%CARD_NAME%","N/A");
					R = R.Replace("%CARD_EXPIRATION%","N/A");
					break;
				case "CHECK":
					R = R.Replace("%CARD_TYPE%","N/A (Check)");
					R = R.Replace("%CARD_NUMBER%","N/A");
					R = R.Replace("%CARD_NAME%","N/A");
					R = R.Replace("%CARD_EXPIRATION%","N/A");
					break;
			}

			String ItemList = String.Empty;
			String Tok1 = "<!-- ITEM_START -->";
			String Tok2 = "<!-- ITEM_END -->";
			String IR = Common.ExtractToken(R,Tok1,Tok2);
			foreach(CartItem c in _cartItems)
			{
				//if(c.SKU != "MICROPAY")
				//{
					String IRX = IR;
					IRX = IRX.Replace("%ITEM_SKU%",c.SKU);
					IRX = IRX.Replace("%ITEM_COLOR%",(Common.IIF(c.chosenColor.Length == 0 , "--" , c.chosenColor)));
					IRX = IRX.Replace("%ITEM_SIZE%",(Common.IIF(c.chosenSize.Length == 0 , "--" , c.chosenSize)));
					IRX = IRX.Replace("%ITEM_TEXTOPTION%",(Common.IIF(c.textOption.Length == 0 , "--" , c.textOption)));
					IRX = IRX.Replace("%ITEM_QUANTITY%",c.quantity.ToString());

					StringBuilder tmpS = new StringBuilder(1000);
					tmpS.Append(c.productName);
					if(c.textOption.Length != 0)
					{
						tmpS.Append(" (Text: " + c.textOption + ") ");
					}

					if(this.IsAKit(c.productID))
					{
						tmpS.Append(":");
						IDataReader rscust = DB.GetRS("select * from orders_kitcart where ShoppingCartRecID=" + c.ShoppingCartRecordID.ToString() + " and OrderNumber=" + _orderNumber.ToString() + " and CustomerID=" + _customerID.ToString() + " order by KitItemName");
						while(rscust.Read())
						{
							tmpS.Append("<br>");
							tmpS.Append("<small>");
							tmpS.Append("&nbsp;&nbsp;-&nbsp;(" + DB.RSFieldInt(rscust,"Quantity").ToString() + ")&nbsp;");
							tmpS.Append(DB.RSField(rscust,"KitItemName") + ", ");
							tmpS.Append("</small>");
						}
						rscust.Close();
					}

					int PackSize = this.PackSize(c.productID,c.ShoppingCartRecordID);
					bool IsAPack = this.IsAPack(c.productID,c.ShoppingCartRecordID);
					if(IsAPack)
					{
						IDataReader rscust = DB.GetRS("select * from orders_customcart where ShoppingCartRecID=" + c.ShoppingCartRecordID.ToString() + " and OrderNumber=" + _orderNumber.ToString() + " and CustomerID=" + _customerID.ToString() + " order by ProductName");
						while(rscust.Read())
						{
							tmpS.Append("<br>");
							tmpS.Append("<small>");
							tmpS.Append("(" + DB.RSFieldInt(rscust,"Quantity").ToString() + ") ");
							tmpS.Append(DB.RSField(rscust,"ProductName") + ", ");
							tmpS.Append(DB.RSField(rscust,"ProductSKU") + ", ");
							tmpS.Append(DB.RSField(rscust,"ChosenColor") + ", ");
							tmpS.Append(DB.RSField(rscust,"ChosenSize"));
							tmpS.Append("</small>");
						}
						rscust.Close();
					}

					IRX = IRX.Replace("%ITEM_NAME%",tmpS.ToString());

					IRX = IRX.Replace("%ITEM_STATUS%","");

					// note: order shopping cart records "price" field already reflects the total including all discounts, quantities, levels, etc, so just display it (this is DIFFERENT than the shopping cart)
					IRX = IRX.Replace("%ITEM_PRICE%",Localization.CurrencyStringForDisplay(c.price / c.quantity));
					IRX = IRX.Replace("%ITEM_SUBTOTAL%",Localization.CurrencyStringForDisplay(c.price));
					ItemList += IRX;
				//}
			}

			int ix1 = R.IndexOf(Tok1);
			int ix2 = R.IndexOf(Tok2);
			R = R.Substring(0,ix1 + Tok1.Length) + ItemList + R.Substring(ix2,R.Length - ix2);

			return R;
		}

		private bool IsAKit(int ProductID) // remember, we CANNOT use anything except from order tables in the order object!
		{
			return DB.GetSqlN("select count(*) as N from orders_kitcart  " + DB.GetNoLock() + " where CustomerID=" + _customerID.ToString() + " and ProductID=" + ProductID.ToString()) > 0;
		}

		private bool IsAPack(int ProductID, int ShoppingCartRecID) // remember, we CANNOT use anything except from order tables in the order object!
		{
			return DB.GetSqlN("select count(*) as N from orders_customcart  " + DB.GetNoLock() + " where ShoppingCartRecID=" + ShoppingCartRecID.ToString() + " and CustomerID=" + _customerID.ToString() + " and PackID=" + ProductID.ToString()) > 0;
		}

		private int PackSize(int ProductID, int ShoppingCartRecID) // remember, we CANNOT use anything except from order tables in the order object!
		{
			return DB.GetSqlN("select sum(quantity) as N from orders_customcart  " + DB.GetNoLock() + " where ShoppingCartRecID=" + ShoppingCartRecID.ToString() + " and CustomerID=" + _customerID.ToString() + " and PackID=" + ProductID.ToString());
		}

		public String Display(String ShowCCPWD, bool nocc)
		{
			StringBuilder tmpS = new StringBuilder(10000);

			String SS = String.Empty;
			tmpS.Append("<table cellSpacing=\"0\" cellPadding=\"0\" width=\"100%\">");
			tmpS.Append("<tr><td width=\"20%\">Order Number:</td><td width=\"80%\">" + _orderNumber.ToString() + "</td></tr>");
			tmpS.Append("<tr><td width=\"20%\">Customer ID:</td><td width=\"80%\">" + _customerID.ToString() + "</td></tr>");
			tmpS.Append("<tr><td width=\"20%\">EMail:</td><td width=\"80%\">" + HttpContext.Current.Server.HtmlEncode(_email) + "</td></tr>");
			//tmpS.Append("<tr><td width=\"20%\">Password:</td><td width=\"80%\">" + HttpContext.Current.Server.HtmlEncode(_password) + "</td></tr>");
			tmpS.Append("<tr><td width=\"20%\">Date of purchase:</td><td width=\"80%\">" + _orderDate.ToString() + "</td></tr>");
			tmpS.Append("<tr><td width=\"20%\">Store Version:</td><td width=\"80%\">" + _storeVersion + "</td></tr>");
			if(_levelID != 0)
			{
				StringBuilder LevelInfo = new StringBuilder(1000);
				LevelInfo.Append(_levelName);
				if(ShowCCPWD == Common.AppConfig("OrderShowCCPwd"))
				{
					LevelInfo.Append(": ");
					LevelInfo.Append("DiscountPercent=" + _levelDiscountPercent.ToString() + "%, ");
					LevelInfo.Append("DiscountAmount=" + Localization.CurrencyStringForDisplay(_levelDiscountAmount) + ", ");
					LevelInfo.Append("HasFreeShipping=" + _levelHasFreeShipping.ToString() + ", ");
					LevelInfo.Append("AllowsQuantityDiscounts=" + _levelAllowsQuantityDiscounts.ToString() + ", ");
					LevelInfo.Append("HasNoTax=" + _levelHasNoTax.ToString() + ", ");
					LevelInfo.Append("AllowsCoupons=" + _levelAllowsCoupons.ToString() + ", ");
					LevelInfo.Append("DiscountsApplyToExtendedPrices=" + _levelDiscountsApplyToExtendedPrices.ToString());
				}
				tmpS.Append("<tr><td width=\"20%\">Customer Level:</td><td width=\"80%\">" + LevelInfo.ToString() + "</td></tr>");
			}
			if(_coupon.code.Length != 0)
			{
				tmpS.Append("<tr><td width=\"20%\">Coupon Used:</td><td width=\"80%\">" + HttpContext.Current.Server.HtmlEncode(_coupon.code) + " (" + _coupon.description + "), Free Shipping: " + Common.IIF(_coupon.includesFreeShipping , "Yes" , "No") + ", Discount Amount: " + Common.IIF(_coupon.discountAmount != System.Decimal.Zero , Localization.CurrencyStringForDisplay(_coupon.discountAmount),"N/A") + ", Discount Percent: " + Common.IIF(_coupon.discountPercent != 0 , _coupon.discountPercent.ToString() + "%" , "N/A") + "</td></tr>");
			}
			else
			{
				tmpS.Append("<tr><td width=\"20%\">Coupon Used:</td><td width=\"80%\">None</td></tr>");
			}
			tmpS.Append("<tr><td width=\"20%\">Locale Setting:</td><td width=\"80%\">" + this._localeSetting + "</td></tr>");
			String PMT = _paymentMethod;
			if(_paymentMethod == "MICROPAY")
			{
				PMT = Common.AppConfig("Micropay.Prompt");
			}
			tmpS.Append("<tr><td width=\"20%\">Payment Method:</td><td width=\"80%\">" + PMT + "</td></tr>");
			switch(_paymentMethod.Replace(" ","").Trim().ToUpper())
			{
				case "CREDITCARD":
					tmpS.Append("<tr><td width=\"20%\">Card Type:</td><td width=\"80%\">" + _cardType + "</td></tr>");
					tmpS.Append("<tr><td width=\"20%\">Card Name:</td><td width=\"80%\">" + HttpContext.Current.Server.HtmlEncode(_cardName) + "</td></tr>");
					String ctmp = _cardNumber;
					if(_cardNumber.Length != 0 && _cardNumber != "XXXXXXXXXXXX")
					{
						ctmp = Common.UnmungeString(_cardNumber);
					}
					if(ShowCCPWD == Common.AppConfig("OrderShowCCPwd") && !nocc)
					{
						// authorized to view this info:
						if(_cardType.ToUpper() == "PayPal".ToUpper())
						{
							tmpS.Append("<tr><td width=\"20%\">Card Number:</td><td width=\"80%\"></td></tr>");
						}
						else
						{
							tmpS.Append("<tr><td width=\"20%\">Card Number:</td><td width=\"80%\">" + HttpContext.Current.Server.HtmlEncode(ctmp) + "</td></tr>");
						}
					}
					else
					{
						// not authorized to view this info:
						if(_cardNumber.Length == 0 || _cardNumber == "XXXXXXXXXXXX")
						{
							tmpS.Append("<tr><td width=\"20%\">Card Number:</td><td width=\"80%\"></td></tr>");
						}
						else
						{
							if(_cardType.ToUpper() == "PayPal".ToUpper())
							{
								tmpS.Append("<tr><td width=\"20%\">Card Number:</td><td width=\"80%\"></td></tr>");
							}
							else
							{
								String CardSuffix = "XXXX";
								try
								{
									CardSuffix = ctmp.Substring(ctmp.Length-4,4);
								}
								catch {}
								tmpS.Append("<tr><td width=\"20%\">Card Number:</td><td width=\"80%\">XXXXXXXX" + CardSuffix + "</td></tr>");
							}
						}
					}
					if(_cardNumber.Length == 0 || _cardNumber == "XXXXXXXXXXXX")
					{
						tmpS.Append("<tr><td width=\"20%\">Card Expiration:</td><td width=\"80%\"></td></tr>");
					}
					else
					{
						if(_cardType.ToUpper() == "PayPal".ToUpper())
						{
							tmpS.Append("<tr><td width=\"20%\">Card Expiration:</td><td width=\"80%\"></td></tr>");
						}
						else
						{
							tmpS.Append("<tr><td width=\"20%\">Card Expiration:</td><td width=\"80%\">" + _cardExpirationMonth + "/" + _cardExpirationYear + "</td></tr>");
						}
					}
					break;

				
				case "ECHECK":
					tmpS.Append("<tr><td width=\"20%\">Bank Account Name:</td><td width=\"80%\">" + HttpContext.Current.Server.HtmlEncode(_eCheckBankAccountName) + "</td></tr>");
					tmpS.Append("<tr><td width=\"20%\">Bank Account Type:</td><td width=\"80%\">" + HttpContext.Current.Server.HtmlEncode(_eCheckBankAccountType) + "</td></tr>");
					tmpS.Append("<tr><td width=\"20%\">Bank Name:</td><td width=\"80%\">" + HttpContext.Current.Server.HtmlEncode(_eCheckBankName) + "</td></tr>");
					if(ShowCCPWD == Common.AppConfig("OrderShowCCPwd") && !nocc)
					{
						// authorized to view this info:
						tmpS.Append("<tr><td width=\"20%\">Bank ABA Code:</td><td width=\"80%\">" + HttpContext.Current.Server.HtmlEncode(_eCheckBankABACode) + "</td></tr>");
						tmpS.Append("<tr><td width=\"20%\">Bank Account #:</td><td width=\"80%\">" + HttpContext.Current.Server.HtmlEncode(_eCheckBankAccountNumber) + "</td></tr>");
					}
					else
					{
						// don't show anything else
					}
					break;
				
				
				case "PURCHASEORDER":
					tmpS.Append("<tr><td width=\"20%\">PO Number:</td><td width=\"80%\">" + HttpContext.Current.Server.HtmlEncode(_PONumber) + "</td></tr>");
					break;
				case "PAYPAL":
					break;
				case "REQUESTQUOTE":
					break;
				case "CHECK":
					break;
			}
			tmpS.Append("<tr><td width=\"20%\" valign=\"top\" >Order Notes:</td><td width=\"80%\" valign=\"top\" >" + HttpContext.Current.Server.HtmlEncode(_orderNotes) + "</td></tr>");
			if(Common.AppConfigBool("ShowCustomerServiceNotesInReceipts"))
			{
				tmpS.Append("<tr><td width=\"20%\" valign=\"top\" >Customer Service Notes:</td><td width=\"80%\" valign=\"top\" >" + _customerServiceNotes + "</td></tr>");
			}
			tmpS.Append("<tr><td width=\"20%\">Affiliate ID:</td><td width=\"80%\">" + _affiliateID.ToString() + "</td></tr>");
			tmpS.Append("<tr><td colspan=\"2\" height=\"30\"></td></tr></table>");

			tmpS.Append("<div align=\"left\">");
			tmpS.Append("<table border=\"0\" cellpadding=\"3\" cellspacing=\"0\" width=\"100%\">");
			tmpS.Append("<tr>");
			tmpS.Append("<td width=\"43%\" valign=\"bottom\" align=\"left\"><b>PRODUCT</b></td>");
			tmpS.Append("<td width=\"13%\" align=\"left\" valign=\"bottom\"><b>SKU</b></td>");
			tmpS.Append("<td width=\"6%\" align=\"center\" valign=\"bottom\"><b>" + Common.AppConfig("ColorOptionPrompt").ToUpper() + "</b></td>");
			tmpS.Append("<td width=\"6%\" align=\"center\" valign=\"bottom\"><b>" + Common.AppConfig("SizeOptionPrompt").ToUpper() + "</b></td>");
			tmpS.Append("<td width=\"20%\" align=\"center\" valign=\"bottom\"><b>QUANTITY</b></td>");
			tmpS.Append("<td width=\"12%\" align=\"right\" valign=\"bottom\"><b>PRICE</b></td>");
			tmpS.Append("</tr>");
			tmpS.Append("<tr>");
			tmpS.Append("<td valign=\"top\" align=\"left\" colspan=\"6\"height=\"3\">");
			tmpS.Append("<hr noshade size=\"2\" width=\"100%\">");
			tmpS.Append("</td>");
			tmpS.Append("</tr>");
			foreach(CartItem c in _cartItems)
			{
				string AutoShipString = Common.IIF(c.isRecurring , "<br><font size=\"1\">[Auto-Ship]</font>" , "");
				//if(c.SKU != "MICROPAY")
				//{
					tmpS.Append("<tr>");
					tmpS.Append("<td  valign=\"top\" align=\"left\">");
					tmpS.Append(c.productName);
					if(c.textOption.Length != 0)
					{
						tmpS.Append(" (Text: " + c.textOption + ") ");
					}

					if(this.IsAKit(c.productID))
					{
						tmpS.Append(":");
						IDataReader rscust = DB.GetRS("select * from orders_kitcart where ShoppingCartRecID=" + c.ShoppingCartRecordID.ToString() + " and OrderNumber=" + _orderNumber.ToString() + " and CustomerID=" + _customerID.ToString() + " order by KitItemName");
						while(rscust.Read())
						{
							tmpS.Append("<br>");
							tmpS.Append("<small>");
							tmpS.Append("&nbsp;&nbsp;-&nbsp;(" + DB.RSFieldInt(rscust,"Quantity").ToString() + ")&nbsp;");
							tmpS.Append(DB.RSField(rscust,"KitItemName") + ", ");
							tmpS.Append("</small>");
						}
						rscust.Close();
					}

					int PackSize = this.PackSize(c.productID,c.ShoppingCartRecordID);
					bool IsAPack = this.IsAPack(c.productID,c.ShoppingCartRecordID);
					if(IsAPack)
					{
						IDataReader rscust = DB.GetRS("select * from orders_customcart where ShoppingCartRecID=" + c.ShoppingCartRecordID.ToString() + " and OrderNumber=" + _orderNumber.ToString() + " and CustomerID=" + _customerID.ToString() + " order by ProductName");
						while(rscust.Read())
						{
							tmpS.Append("<br>");
							tmpS.Append("<small>");
							tmpS.Append("(" + DB.RSFieldInt(rscust,"Quantity").ToString() + ") ");
							tmpS.Append(DB.RSField(rscust,"ProductName") + ", ");
							tmpS.Append(DB.RSField(rscust,"ProductSKU") + ", ");
							tmpS.Append(DB.RSField(rscust,"ChosenColor") + ", ");
							tmpS.Append(DB.RSField(rscust,"ChosenSize"));
							tmpS.Append("</small>");
						}
						rscust.Close();
						tmpS.Append("<br>");
					}
					tmpS.Append(AutoShipString);


					tmpS.Append("</td>");
					tmpS.Append("<td align=\"left\" valign=\"top\">" + c.SKU + "</td>");
					tmpS.Append("<td align=\"center\" valign=\"top\">" +(Common.IIF(c.chosenColor.Length == 0 , "--" , c.chosenColor)) + "</td>");
					tmpS.Append("<td align=\"center\" valign=\"top\">" +(Common.IIF(c.chosenSize.Length == 0 , "--" , c.chosenSize)) + "</td>");
					tmpS.Append("<td align=\"center\" valign=\"top\">");
					if(IsAPack)
					{
						tmpS.Append(c.quantity.ToString() + " (" + (PackSize*c.quantity).ToString() + " items)");
					}
					else
					{
						tmpS.Append(c.quantity);
					}
					tmpS.Append("</td>");
					tmpS.Append("<td align=\"right\" valign=\"top\">");
				
					//tmpS.Append(Localization.CurrencyStringForDisplay(c.price * c.quantity);

					// note: order shopping cart records "price" field already reflects the total including all discounts, quantities, levels, etc, so just display it (this is DIFFERENT than the shopping cart)
					int Q = c.quantity;
					decimal PR = c.price;
					int ActiveDID = c.QuantityDiscountID;
					Single DIDPercent = c.QuantityDiscountPercent;
					tmpS.Append(Localization.CurrencyStringForDisplay(PR));
					if(_levelAllowsQuantityDiscounts)
					{
						if(ActiveDID != 0 && DIDPercent != 0.0)
						{
							tmpS.Append(" <small>(" + DIDPercent.ToString() + "% Quan Dis)</small>");
						}
					}
					tmpS.Append("</td>");
					tmpS.Append("</tr>");
				//}
			}
			tmpS.Append("<tr>");
			tmpS.Append("<td valign=\"top\" align=\"left\" colspan=\"6\"height=\"3\">");
			tmpS.Append("<hr noshade width=\"100%\" size=\"2\">");
			tmpS.Append("</td>");
			tmpS.Append("</tr>");



			// just write selected options out:
			if(_orderOptions.Length != 0)
			{
				tmpS.Append("<tr><td valign=\"top\" align=\"left\" colspan=\"6\">");

				tmpS.Append("<div align=\"center\" width=\"50%\">");
					
				//tmpS.Append("<span class=\"OrderOptionsTitle\">You Have Selected The Following Order Options:</span><br><br>");
				tmpS.Append("<table width=\"50%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
				tmpS.Append("<tr>");
				tmpS.Append("<td align=\"left\"><span class=\"OrderOptionsTitle\">You Have Selected The Following Order Options:</span></td>");
				tmpS.Append("<td align=\"center\"><span class=\"OrderOptionsRowHeader\">Cost</span></td>");
				tmpS.Append("<td align=\"center\"><span class=\"OrderOptionsRowHeader\">Selected</span></td>");
				tmpS.Append("</tr>");
				foreach(String s in _orderOptions.Split('^'))
				{
					String[] flds = s.Split('|');
					tmpS.Append("<tr>");
					tmpS.Append("<td align=\"left\">");
					String ImgUrl = Common.LookupImage("OrderOption",Localization.ParseUSInt(flds[0]),"icon",_siteID);
					if(ImgUrl.Length != 0)
					{
						tmpS.Append("<img src=\"" + ImgUrl + "\" border=\"0\" align=\"absmiddle\">&nbsp;");
					}
					tmpS.Append("<span class=\"OrderOptionsName\">" + flds[1] + "</span></td>");
					tmpS.Append("<td align=\"center\"><span class=\"OrderOptionsPrice\">" + flds[2] + "</span></td>");
					tmpS.Append("<td align=\"center\"><img src=\"skins/skin_" + _siteID.ToString() + "/images/selected.gif\" align=\"absmiddle\"></td>");
					tmpS.Append("</tr>");
				}
				tmpS.Append("</table>");

				tmpS.Append("<br>&nbsp;<br>");
				tmpS.Append("</div>");

				tmpS.Append("</td></tr>");
			}

			tmpS.Append("<tr>");
			tmpS.Append("<td valign=\"top\" align=\"left\" rowspan=\"3\" colspan=\"4\">");
			tmpS.Append("</td>");
			tmpS.Append("<td align=\"right\" valign=\"top\">Subtotal");
			tmpS.Append(":</td>");
			String st = "Will Quote";
			if(_paymentMethod.ToUpper() != "REQUEST QUOTE")
			{
				st = Localization.CurrencyStringForDisplay(SubTotal(true));
			}
			tmpS.Append("<td align=\"right\" valign=\"top\">" + st + "</td>");
			tmpS.Append("</tr>");
			tmpS.Append("<tr>");
			tmpS.Append("<td align=\"right\" valign=\"top\">Tax");
			if(_taxRate != 0.0F && TaxTotal(true) > System.Decimal.Zero)
			{
				tmpS.Append("(" + String.Format("{0:P}",_taxRate/100.0F) + ")");
			}
			tmpS.Append(":</td>");
			st = "Will Quote";
			if(_paymentMethod.ToUpper() != "REQUEST QUOTE")
			{
				st = Localization.CurrencyStringForDisplay(TaxTotal(true));
			}
			tmpS.Append("<td align=\"right\" valign=\"top\">" + st + "</td>");
			tmpS.Append("</tr>");
			tmpS.Append("<tr>");
			String ShowShipText = _shippingMethod;
			// strip out RT shipping cost, if any:
			if(this.IsAllDownloadComponents())
			{
				ShowShipText = "FREE SHIPPING (Download)";
			}
			else if(ShowShipText.IndexOf("|") != -1)
			{
				String[] ss2 = ShowShipText.Split('|');
				try
				{
					ShowShipText = ss2[0].Trim();
				}
				catch {}
			}
			if(_shippingCalculationID == 4)
			{
				ShowShipText = "FREE SHIPPING";
			}

			tmpS.Append("<td align=\"right\" valign=\"top\" >Shipping (" + ShowShipText + "):</td>");
			st = "Will Quote";
			if(_paymentMethod.ToUpper() != "REQUEST QUOTE")
			{
				st = Localization.CurrencyStringForDisplay(ShippingTotal(true));
			}
			tmpS.Append("<td align=\"right\" valign=\"top\">" + st + "</td>");
			tmpS.Append("</tr>");
			tmpS.Append("<tr>");
			tmpS.Append("<td valign=\"top\" align=\"right\" colspan=\"6\">");
			tmpS.Append("<hr noshade size=\"1\" width=\"100%\"></td>");
			tmpS.Append("</tr>");
			tmpS.Append("<tr>");
			tmpS.Append("<td align=\"right\" valign=\"top\" colspan=\"5\">Total:</td>");
			st = "Will Quote";
			if(_paymentMethod.ToUpper() != "REQUEST QUOTE")
			{
				st = Localization.CurrencyStringForDisplay(Total(true));
			}
			tmpS.Append("<td align=\"right\" valign=\"top\">" + st + "</td>");
			tmpS.Append("</tr>");
			tmpS.Append("<tr>");
			tmpS.Append("<td valign=\"top\" align=\"left\" colspan=\"6\"height=\"10\"></td>");
			tmpS.Append("</tr>");
			tmpS.Append("</table>");
			tmpS.Append("</div>");

			tmpS.Append("<table cellSpacing=\"0\" cellPadding=\"0\" width=\"100%\">");
			tmpS.Append("<tr>");
			tmpS.Append("<td>&nbsp;</td>");
			tmpS.Append("<td>&nbsp;</td>");
			tmpS.Append("<td><b>Ship To</b></td>");
			tmpS.Append("<td>&nbsp;</td>");
			tmpS.Append("<td><b>Bill To</b></td>");
			tmpS.Append("</tr>");
			tmpS.Append("<tr>");
			tmpS.Append("<td>Name:</td>");
			tmpS.Append("<td>&nbsp;</td>");
			tmpS.Append("<td>" + HttpContext.Current.Server.HtmlEncode((_shippingAddress.firstName + " " + _shippingAddress.lastName).Trim()) + "</td>");
			tmpS.Append("<td>&nbsp;</td>");
			tmpS.Append("<td>" + HttpContext.Current.Server.HtmlEncode((_billingAddress.firstName + " " + _billingAddress.lastName).Trim()) + "</td>");
			tmpS.Append("</tr>");
			tmpS.Append("<tr>");
			tmpS.Append("<td>Company:</td>");
			tmpS.Append("<td>&nbsp;</td>");
			tmpS.Append("<td>" + HttpContext.Current.Server.HtmlEncode(_shippingAddress.company) + "</td>");
			tmpS.Append("<td>&nbsp;</td>");
			tmpS.Append("<td>" + HttpContext.Current.Server.HtmlEncode(_billingAddress.company) + "</td>");
			tmpS.Append("</tr>");
			tmpS.Append("<tr>");
			tmpS.Append("<td>Address:</td>");
			tmpS.Append("<td>&nbsp;</td>");
			tmpS.Append("<td>" + HttpContext.Current.Server.HtmlEncode(_shippingAddress.address1) + "</td>");
			tmpS.Append("<td>&nbsp;</td>");
			tmpS.Append("<td>" + HttpContext.Current.Server.HtmlEncode(_billingAddress.address1) + "</td>");
			tmpS.Append("</tr>");
			if(_shippingAddress.address2.Length != 0 || _billingAddress.address2.Length != 0)
			{
				tmpS.Append("<tr>");
				tmpS.Append("<td></td>");
				tmpS.Append("<td>&nbsp;</td>");
				tmpS.Append("<td>" + HttpContext.Current.Server.HtmlEncode(_shippingAddress.address2) + "</td>");
				tmpS.Append("<td>&nbsp;</td>");
				tmpS.Append("<td>" + HttpContext.Current.Server.HtmlEncode(_billingAddress.address2) + "</td>");
				tmpS.Append("</tr>");
			}
			if(_shippingAddress.suite.Length != 0 || _billingAddress.suite.Length != 0)
			{
				tmpS.Append("<tr>");
				tmpS.Append("<td>Suite:</td>");
				tmpS.Append("<td>&nbsp;</td>");
				tmpS.Append("<td>" + HttpContext.Current.Server.HtmlEncode(_shippingAddress.suite) + "</td>");
				tmpS.Append("<td>&nbsp;</td>");
				tmpS.Append("<td>" + HttpContext.Current.Server.HtmlEncode(_billingAddress.suite) + "</td>");
				tmpS.Append("</tr>");
			}
			tmpS.Append("<tr>");
			tmpS.Append("<td>City:</td>");
			tmpS.Append("<td>&nbsp;</td>");
			tmpS.Append("<td>" + HttpContext.Current.Server.HtmlEncode(_shippingAddress.city) + "</td>");
			tmpS.Append("<td>&nbsp;</td>");
			tmpS.Append("<td>" + HttpContext.Current.Server.HtmlEncode(_billingAddress.city) + "</td>");
			tmpS.Append("</tr>");
			tmpS.Append("<tr>");
			tmpS.Append("<td>State:</td>");
			tmpS.Append("<td>&nbsp;</td>");
			tmpS.Append("<td>" + HttpContext.Current.Server.HtmlEncode(_shippingAddress.state) + "</td>");
			tmpS.Append("<td>&nbsp;</td>");
			tmpS.Append("<td>" + HttpContext.Current.Server.HtmlEncode(_billingAddress.state) + "</td>");
			tmpS.Append("</tr>");
			tmpS.Append("<tr>");
			tmpS.Append("<td>Zip:</td>");
			tmpS.Append("<td>&nbsp;</td>");
			tmpS.Append("<td>" + HttpContext.Current.Server.HtmlEncode(_shippingAddress.zip) + "</td>");
			tmpS.Append("<td>&nbsp;</td>");
			tmpS.Append("<td>" + HttpContext.Current.Server.HtmlEncode(_billingAddress.zip) + "</td>");
			tmpS.Append("</tr>");
			tmpS.Append("<tr>");
			tmpS.Append("<td>Country:</td>");
			tmpS.Append("<td>&nbsp;</td>");
			tmpS.Append("<td>" + HttpContext.Current.Server.HtmlEncode(_shippingAddress.country) + "</td>");
			tmpS.Append("<td>&nbsp;</td>");
			tmpS.Append("<td>" + HttpContext.Current.Server.HtmlEncode(_billingAddress.country) + "</td>");
			tmpS.Append("</tr>");
			tmpS.Append("<tr>");
			tmpS.Append("<td>Phone:</td>");
			tmpS.Append("<td>&nbsp;</td>");
			tmpS.Append("<td>" + HttpContext.Current.Server.HtmlEncode(_shippingAddress.phone) + "</td>");
			tmpS.Append("<td>&nbsp;</td>");
			tmpS.Append("<td>" + HttpContext.Current.Server.HtmlEncode(_billingAddress.phone) + "</td>");
			tmpS.Append("</tr>");
			//			tmpS.Append("<tr>");
			//			tmpS.Append("<td>Email:</td>");
			//			tmpS.Append("<td>&nbsp;</td>");
			//			tmpS.Append("<td colspan=\"3\">" + HttpContext.Current.Server.HtmlEncode(_email) + "</td>");
			//			tmpS.Append("</tr>");
			tmpS.Append("<tr>");
			tmpS.Append("<td>Shipping Method:</td>");
			tmpS.Append("<td>&nbsp;</td>");
			tmpS.Append("<td colspan=\"3\">" + HttpContext.Current.Server.HtmlEncode(_shippingMethod) + "</td>");
			tmpS.Append("</tr>");
			tmpS.Append("<tr>");
			tmpS.Append("<td colspan=\"5\">&nbsp;</td>");
			tmpS.Append("</tr>");
			tmpS.Append("</table>");

			return tmpS.ToString();
		}

		public String Receipt(int ViewingCustomerID, String ReceiptTemplate, String ShowCCPWD, bool nocc)
		{
			String TMP = String.Empty;
			if(ReceiptTemplate.Length != 0)
			{
				TMP = ReceiptTemplate;
			}
			String ErrorMsg = String.Empty;
			if(_isEmpty)
			{
				// no order
				ErrorMsg = "<b><font color=\"#0000FF\">Order Number: " + _orderNumber.ToString() + " appears to be empty.";
			}
			bool OkToView = false;
			if(ViewingCustomerID == _customerID || Customer.StaticIsAdminUser(ViewingCustomerID))
			{
				OkToView = true;
			}
			else
			{
				if(ShowCCPWD == Common.AppConfig("OrderShowCCPwd"))
				{
					OkToView = true;
				}
			}
			if(!OkToView)
			{
				// not authorized to view the order
				ErrorMsg = "<b><font color=\"#0000FF\">You are not authorized to view this order.";
			}

			if(ReceiptTemplate.Length == 0 || TMP.Length == 0 || ErrorMsg.Length != 0)
			{
				// use built in default receipt format:
				StringBuilder tmpS = new StringBuilder(10000);
				tmpS.Append("<html>");
				tmpS.Append("<head>");
				tmpS.Append("<meta http-equiv=\"Content-Language\" content=\"en-us\">");
				tmpS.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1252\">");
				tmpS.Append("<title>" + Common.AppConfig("StoreName").ToUpper() + " - RECEIPT" + Common.IIF(_paymentMethod.ToUpper() == "REQUEST QUOTE" , " (REQUEST FOR QUOTE)" , "") + "</title>");
				tmpS.Append("<link rel=\"stylesheet\" href=\"skins/Skin_" + _siteID.ToString() + "/style.css\" type=\"text/css\">");
				tmpS.Append("</head>");
				tmpS.Append("<body>");
				tmpS.Append("<p align=\"center\"><b><font size=\"3\">" + Common.AppConfig("StoreName").ToUpper() + " RECEIPT" + Common.IIF(_paymentMethod.ToUpper() == "REQUEST QUOTE" , " (REQUEST FOR QUOTE)" , "") + "<br>");
				tmpS.Append("</font></b></p>");
				tmpS.Append("<p align=\"center\"><b><font size=\"1\">*** PLEASE PRINT RECEIPT OUT AND RETAIN IT FOR FUTURE REFERENCE ***</font></b><br><br></p>");
				String RHdr = String.Empty;
				Topic t1 = new Topic("ReceiptHeader",this._siteID);
				RHdr = t1._contents;
				if(RHdr.Length == 0)
				{
					RHdr = Common.AppConfig("ReceiptHeader");
				}
				if(RHdr.Length == 0)
				{
					tmpS.Append(RHdr);
				}
				tmpS.Append("<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
				tmpS.Append("<tr><td align=\"left\" valign=\"top\">");
				if(ErrorMsg.Length != 0)
				{
					tmpS.Append(ErrorMsg);
				}
				else
				{
					tmpS.Append(Display(ShowCCPWD,nocc));
				}
				tmpS.Append("</td></tr></table>");
				String RFtr = String.Empty;
				Topic t2 = new Topic("ReceiptFooter",this._siteID);
				RFtr = t2._contents;
				if(RFtr.Length == 0)
				{
					RFtr = Common.AppConfig("ReceiptFooter");
				}
				if(RFtr.Length == 0)
				{
					tmpS.Append(RFtr);
				}
				tmpS.Append("<p>Thank you for your purchase.</p>");
				tmpS.Append("<p>" + Common.AppConfig("StoreName") + "<br>");
				tmpS.Append("<a href=\"" + Common.GetStoreHTTPLocation(false) + "\">" + Common.GetStoreHTTPLocation(false) + "</a></p>");
				tmpS.Append("</body>");
				tmpS.Append("</html>");
				return tmpS.ToString();
			}
			else
			{
				return ProcessReceiptTokens(TMP,ShowCCPWD,nocc);	
			}
		}

		public String Notification()
		{
			StringBuilder tmpS = new StringBuilder(1000);

			tmpS.Append("<html><head><title>" + Common.AppConfig("StoreName") + " Order Notification, Order #" + _orderNumber.ToString() + "</title></head><body>");
			tmpS.Append("<b>" + Common.AppConfig("StoreName") + " Order Notification, Order #" + _orderNumber.ToString() + "</b><br>");
			tmpS.Append("<b>Order Date: " + Localization.ToNativeDateTimeString(DateTime.Now) + "<br><br>");

			String ReceiptURL = Common.GetStoreHTTPLocation(true) + "receipt.aspx?ordernumber=" + _orderNumber.ToString() + "&customerid=" + _customerID.ToString();
			String XMLURL = Common.GetStoreHTTPLocation(true) + Common.AppConfig("AdminDir") + "/orderXML.aspx?ordernumber=" + _orderNumber.ToString() + "&customerid=" + _customerID.ToString();

			tmpS.Append("For Order Receipt: <a href=\"" + ReceiptURL + "\">click here</a><br>");
			tmpS.Append("For Order XML: <a href=\"" + XMLURL + "\">click here</a><br>");
			tmpS.Append("</body></html>");
			return tmpS.ToString();
		}

		public bool IsEmpty()
		{
			return _isEmpty;
		}

		public String GetPackingList(String FieldSeparator, String LineBreak)
		{
			StringBuilder tmpS = new StringBuilder(5000);
			int i = 0;
			foreach(CartItem c in _cartItems)
			{
				if(i > 0)
				{
					tmpS.Append(LineBreak);
				}
				tmpS.Append(c.productName);
				if(c.textOption.Length != 0)
				{
					tmpS.Append(" (Text: " + c.textOption + ") ");
				}

				if(this.IsAKit(c.productID))
				{
					tmpS.Append(":");
					IDataReader rscust = DB.GetRS("select * from orders_kitcart where ShoppingCartRecID=" + c.ShoppingCartRecordID.ToString() + " and OrderNumber=" + _orderNumber.ToString() + " and CustomerID=" + _customerID.ToString() + " order by KitItemName");
					while(rscust.Read())
					{
						tmpS.Append("<br>");
						tmpS.Append("<small>");
						tmpS.Append("&nbsp;&nbsp;-&nbsp;(" + DB.RSFieldInt(rscust,"Quantity").ToString() + ")&nbsp;");
						tmpS.Append(DB.RSField(rscust,"KitItemName") + ", ");
						tmpS.Append("</small>");
					}
					rscust.Close();
				}

				int PackSize = this.PackSize(c.productID,c.ShoppingCartRecordID);
				bool IsAPack = this.IsAPack(c.productID,c.ShoppingCartRecordID);
				if(IsAPack)
				{
					IDataReader rscust = DB.GetRS("select * from orders_customcart where ShoppingCartRecID=" + c.ShoppingCartRecordID.ToString() + " and OrderNumber=" + _orderNumber.ToString() + " and CustomerID=" + _customerID.ToString() + " order by ProductName");
					while(rscust.Read())
					{
						tmpS.Append("<br>");
						tmpS.Append("<small>");
						tmpS.Append("(" + DB.RSFieldInt(rscust,"Quantity").ToString() + ") ");
						tmpS.Append(DB.RSField(rscust,"ProductName") + ", ");
						tmpS.Append(DB.RSField(rscust,"ProductSKU") + ", ");
						tmpS.Append(DB.RSField(rscust,"ChosenColor") + ", ");
						tmpS.Append(DB.RSField(rscust,"ChosenSize"));
						tmpS.Append("</small>");
					}
					rscust.Close();
					tmpS.Append("<br>");
				}


				tmpS.Append(FieldSeparator);
				tmpS.Append(c.SKU + FieldSeparator);
				tmpS.Append((Common.IIF(c.chosenColor.Length == 0 , "--" , c.chosenColor)) + FieldSeparator);
				tmpS.Append((Common.IIF(c.chosenSize.Length == 0 , "--" , c.chosenSize)) + FieldSeparator);
				if(IsAPack)
				{
					tmpS.Append(c.quantity.ToString() + " (" + (PackSize*c.quantity).ToString() + " items)");
				}
				else
				{
					tmpS.Append(c.quantity);
				}
				i++;
			}
			return tmpS.ToString();
		}

		// returns true if this order has any items which are download goods:
		public bool HasDownloadComponents()
		{
			//v3_9 Check the packs as well
			if (Common.OrderHasDownloadComponents(this._orderNumber))
			{
				return true;
			}
			foreach(CartItem c in _cartItems)
			{
				if(c.isDownload)
				{
					return true;
				}
			}
			return false;
		}

		// NOTE: This routine ALSO copies the download files to their order directory IF includeFullHTMLBody is true
		public String GetDownloadList(bool includeFullHTMLBody)
		{
			bool DumpDownloadInfo = Common.AppConfigBool("DumpDownloadInfo");

			// prepare for downloads, by copying targets to order download dirs:
			String ThisOrderDownloadDir = "orderdownloads/" + _orderNumber.ToString() + "_" + _customerID.ToString();
			if(DumpDownloadInfo)
			{
				HttpContext.Current.Response.Write("ThisOrderDownloadDir=" + ThisOrderDownloadDir + "<br>");
			}
			String ThisOrderDownloadPath = HttpContext.Current.Server.MapPath(Common.IIF(Common.IsAdminSite,"../","") + ThisOrderDownloadDir);
			if(DumpDownloadInfo)
			{
				HttpContext.Current.Response.Write("ThisOrderDownloadPath=" + ThisOrderDownloadPath + "<br>");
			}
			if(includeFullHTMLBody)
			{
				if(!Directory.Exists(ThisOrderDownloadPath))
				{
					// don't let a failure here stop the order! customer service will have to clean up the issue later:
					try
					{
						Directory.CreateDirectory(ThisOrderDownloadPath);
					}
					catch(Exception ex)
					{
						if(DumpDownloadInfo)
						{
							HttpContext.Current.Response.Write("Exception=" + Common.GetExceptionDetail(ex,"<br>") + "<br>");
						}
					}
				}
			}

			//V3_9 Create a list of downloadable items that includes anything in the Orders_CustomCart
			ArrayList DownloadItems = new ArrayList(50);
			foreach(CartItem c in _cartItems)
			{
				if(c.isDownload && !c.isSecureAttachment && c.quantity != 0)
				{
					DownloadItem newDownload = new DownloadItem();
					newDownload.productID = c.productID;
					newDownload.variantID = c.variantID;
					newDownload.productName = c.productName;
					newDownload.downloadLocation = c.downloadLocation;
					DownloadItems.Add(newDownload);
				}
			}
			if(DB.GetDBProvider() != "MSACCESS") // this SQL is not supported for MS ACCESS
			{
				String getPackDownloads = "select C.ProductID,C.VariantID,C.ProductName,V.DownloadLocation";
				getPackDownloads += " from orders_customcart as C join ProductVariant as V on (C.ProductID=V.ProductID and C.VariantID=V.VariantID)";
				getPackDownloads += " where (V.IsDownload = 1) and C.OrderNumber = " + this._orderNumber.ToString();
				IDataReader rs = DB.GetRS(getPackDownloads);
				while(rs.Read())
				{
					DownloadItem newDownload = new DownloadItem();
					newDownload.productID = DB.RSFieldInt(rs,"ProductID");
					newDownload.variantID = DB.RSFieldInt(rs,"VariantID");
					newDownload.productName = DB.RSField(rs,"ProductName");
					newDownload.downloadLocation = DB.RSField(rs,"DownloadLocation");
					DownloadItems.Add(newDownload);
				}
				rs.Close();
			}
			//V3_9 End

			StringBuilder DownloadList = new StringBuilder(1000);
			DownloadList.Append("<b>Your download files for order #" + _orderNumber.ToString() + " are specified below:</b><br><br>");

			//V3_9 Use the DownloadItems list to create the DownloadList string
			// copy the distribution items:
			for(int i = 0; i< DownloadItems.Count; i++)
			{
				String DownloadLocation = ((DownloadItem)DownloadItems[i]).downloadLocation;
        
				// Is it in the downloads directory?
				bool InDownloads = DownloadLocation.ToLower().StartsWith("download");

				if(DumpDownloadInfo)
				{
					HttpContext.Current.Response.Write("DownloadLocation=" + DownloadLocation + "<br>");
				}
				String TargetFile = DownloadLocation;
				if(DumpDownloadInfo)
				{
					HttpContext.Current.Response.Write("TargetFile=" + TargetFile + "<br>");
				}
				if(TargetFile.IndexOf("/") != -1)
				{
					String[] tmpTF = TargetFile.Split('/');
					TargetFile = tmpTF[tmpTF.GetUpperBound(0)];
				}
				if(DumpDownloadInfo)
				{
					HttpContext.Current.Response.Write("TargetFile=" + TargetFile + "<br>");
				}
        
				if(includeFullHTMLBody && InDownloads)
				{
					// don't let a failure here stop the order! customer service will have to clean up the issue later:
					try
					{
						String DownloadLocationSource = HttpContext.Current.Server.MapPath(Common.IIF(Common.IsAdminSite,"../","") + DownloadLocation);
						if(DumpDownloadInfo)
						{
							HttpContext.Current.Response.Write("DownloadLocationSource=" + DownloadLocationSource + "<br>");
						}
						File.Copy(DownloadLocationSource,ThisOrderDownloadPath + "\\" + TargetFile);
					}
					catch (Exception ex)
					{
						if(DumpDownloadInfo)
						{
							HttpContext.Current.Response.Write("Exception=" + Common.GetExceptionDetail(ex,"<br>") + "<br>");
						}
					}
				}

				String TargetURL;
				if (InDownloads)
				{
					TargetURL = Common.GetStoreHTTPLocation(false) + ThisOrderDownloadDir + "/" + TargetFile;
				}
				else
				{
					TargetURL = Common.GetStoreHTTPLocation(false) + DownloadLocation;
				}
				if(DumpDownloadInfo)
				{
					HttpContext.Current.Response.Write("TargetURL=" + TargetURL + "<br>");
				}
				if(includeFullHTMLBody)
				{
					DownloadList.Append("<b>" + ((DownloadItem)DownloadItems[i]).productName + ":</b><br>");
					if (DownloadLocation.Length != 0)
					{
						DownloadList.Append("<a href=\"" + TargetURL + "\">" + TargetURL + "</a><br>");
						DownloadList.Append("<small>URL=" + TargetURL + "</small><br><br>");
					}
				}
				else
				{
					if (DownloadLocation.Length != 0)
					{
						DownloadList.Append("<a href=\"" + TargetURL + "\"><b>" + ((DownloadItem)DownloadItems[i]).productName + "</b></a><br>");
					}
					else
					{
						DownloadList.Append("<b>" + ((DownloadItem)DownloadItems[i]).productName + "</b><br>");
					}
				}
			}
			//V3_9 END making DownloadList

			if(includeFullHTMLBody)
			{
				StringBuilder tmpS = new StringBuilder(10000);
				tmpS.Append("<html>");
				tmpS.Append("<head>");
				tmpS.Append("<meta http-equiv=\"Content-Language\" content=\"en-us\">");
				tmpS.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1252\">");
				tmpS.Append("<title>" + Common.AppConfig("StoreName").ToUpper() + " - DOWNLOAD INSTRUCTIONS" + Common.IIF(_paymentMethod.ToUpper() == "REQUEST QUOTE" , " (REQUEST FOR QUOTE)" , "") + "</title>");
				tmpS.Append("</head>");
				tmpS.Append("<body>");
				tmpS.Append("<p align=\"center\"><b><font size=\"3\">" + Common.AppConfig("StoreName").ToUpper() + " DOWNLOAD INSTRUCTIONS" + Common.IIF(_paymentMethod.ToUpper() == "REQUEST QUOTE" , " (REQUEST FOR QUOTE)" , "") + "<br>");
				tmpS.Append("</font></b></p>");
				tmpS.Append("<p align=\"center\"><b><font size=\"1\">*** PLEASE PRINT THESE DOWNLOAD INSTRUCTIONS AND RETAIN IT FOR FUTURE REFERENCE ***</font></b><br><br></p>");
				tmpS.Append("<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
				tmpS.Append("<tr><td align=\"left\" valign=\"top\">");
				tmpS.Append(DownloadList.ToString());
				tmpS.Append("</td></tr></table>");
				tmpS.Append("<p>Thank you for your purchase.</p>");
				tmpS.Append("<p>" + Common.AppConfig("StoreName") + "<br>");
				tmpS.Append("<a href=\"" + Common.GetStoreHTTPLocation(false) + "\">" + Common.GetStoreHTTPLocation(false) + "</a></p>");
				tmpS.Append("</body>");
				tmpS.Append("</html>");
				if(DumpDownloadInfo)
				{
					HttpContext.Current.Response.Write("EMailBody=" + tmpS.ToString() + "<br>");
				}
				return tmpS.ToString();
			}
			else
			{
				return DownloadList.ToString();
			}

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


		public String ToXml()
		{
			System.Type[] extraTypes = new System.Type[1];
			extraTypes[0] = typeof(CartItem);
			
			String XmlizedString = null;
			MemoryStream memoryStream = new MemoryStream();
			XmlSerializer xs = new XmlSerializer(typeof(Order),extraTypes);
			XmlTextWriter XmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
			xs.Serialize(XmlTextWriter, this);
			memoryStream = (MemoryStream)XmlTextWriter.BaseStream;
			XmlizedString = Common.UTF8ByteArrayToString(memoryStream.ToArray());
			XmlizedString = XmlizedString.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>","");
			return XmlizedString.Substring(1,XmlizedString.Length - 1); // don't know why there is a crap character at position 0! - RJB
		}


	}
}
