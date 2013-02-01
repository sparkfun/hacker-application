// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for ShoppingCart_process.
	/// </summary>
	public class ShoppingCart_process : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");

			if(!Common.ReferrerOKForSubmit())
			{
				Response.Redirect("default.aspx?acds=failed");
			}

			Customer thisCustomer = new Customer();
			thisCustomer.RequireCustomerRecord();
			
			bool acds = Common.AppConfigBool("AllowCrossDomainSubmit");

			String ShippingCity = Common.Form("ShippingCity");
			String ShippingState = Common.Form("ShippingState");
			String ShippingZip = Common.Form("ShippingZip");
			String ShippingCountry = Common.Form("ShippingCountry");

			ShoppingCart cart = new ShoppingCart(1,thisCustomer,CartTypeEnum.ShoppingCart,0,false);
			if(cart.IsEmpty())
			{
				Response.Redirect("ShoppingCart.aspx");
			}

			for(int i = 0; i<=Request.Form.Count-1; i++)
			{
				String fld = Request.Form.Keys[i];
				String fldval = Request.Form[Request.Form.Keys[i]];
				int recID;
				String quantity;
				if(fld.StartsWith("Quantity"))
				{
					recID = Localization.ParseUSInt(fld.Substring(9));
					quantity = fldval;
					int iquan = 0;
					try
					{
						iquan = Localization.ParseUSInt(quantity);
					}
					catch
					{
						iquan = 0;
					}
					if(iquan < 0)
					{
						iquan = 0;
					}
					cart.SetItemQuantity(recID,iquan);
				}
			}
			cart.SetCoupon(Common.Form("CouponCode"));

			if(ShoppingCart.CheckInventory(thisCustomer._customerID))
			{
				Response.Redirect("ShoppingCart.aspx?resetlinkback=1&acds=" + acds.ToString() + "&errormsg=" + Server.UrlEncode("Some of your item quantities were reduced, as they exceeded inventory in stock."));
			}
			
			bool DoingFullCheckout = (Common.Form("ContinueCheckout") == "1");
			Common.ShippingCalculationEnum ShipCalcID = Common.GetActiveShippingCalculationID();

			if(DoingFullCheckout)
			{
				// reload to do final cost checking!
				cart = null;
				cart = new ShoppingCart(1,thisCustomer,CartTypeEnum.ShoppingCart,0,false);
			}

			int ShippingMethodID = Common.FormNativeInt("ShippingMethodID");
			String ShippingMethod = String.Empty;
			if(ShipCalcID == Common.ShippingCalculationEnum.UseRealTimeRates)
			{
				ShippingMethodID = 0;
				ShippingMethod = Common.Form("ShippingMethod");

// this should no longer be needed RJB:
//				if(DoingFullCheckout)
//				{
//					cart.RecheckShippingRates(); // recalc shipping rates, because user is going to checkout, if going back to cart, no need to do this, as cart does it on display after load
//					ShippingMethod = cart._shippingMethod;
//				}
//				else
//				{
//					ShippingMethod = Common.Form("ShippingMethod");
//				}

			}
			else if(ShipCalcID == Common.ShippingCalculationEnum.CalculateShippingByWeightAndZone)
			{
				ShippingMethodID = 0;
				ShippingMethod = "Ship To Zone";
			}
			else
			{
				ShippingMethod = Common.GetShippingMethodName(ShippingMethodID);
			}
			String OrderNotes = Common.Form("OrderNotes");

			//V4_1
			thisCustomer.ShippingMethodID = ShippingMethodID;
			thisCustomer.ShippingMethod = ShippingMethod;
			thisCustomer.ShippingCity = ShippingCity;
			thisCustomer.ShippingState = ShippingState;
			thisCustomer.ShippingZip = ShippingZip;
			thisCustomer.ShippingCountry = ShippingCountry;
			//V4_1

			DB.ExecuteSQL("update customer set OrderOptions=" + DB.SQuote(Common.Form("OrderOptions")) + ", OrderNotes=" + DB.SQuote(OrderNotes) + " where CustomerID=" + thisCustomer._customerID.ToString());

			if(DoingFullCheckout)
			{

				Decimal MinOrderAmount = Common.AppConfigUSDecimal("CartMinOrderAmount");
				if(MinOrderAmount != System.Decimal.Zero && cart.SubTotal(false,false,true) < MinOrderAmount)
				{
					Response.Redirect("ShoppingCart.aspx?resetlinkback=1");
				}

				if(cart.HasCoupon())
				{
					String CValid = cart.CouponIsValid();
					if(CValid != "OK")
					{
						Response.Redirect("ShoppingCart.aspx?resetlinkback=1&discountvalid=false&invalidreason=" + Server.UrlEncode(CValid) + "&acds=" + acds.ToString());
					}
				}

				Address BillingAddress = new Address();
				Address ShippingAddress = new Address();
				BillingAddress.LoadByCustomer(thisCustomer._customerID,AddressTypes.Billing);
				ShippingAddress.LoadByCustomer(thisCustomer._customerID,AddressTypes.Shipping);

				String PaymentMethod = Common.Form("PaymentMethod");
				if(Common.AppConfig("Micropay.Prompt") == Common.Form("PaymentMethod"))
				{
					PaymentMethod = "MICROPAY";
				}
				if (thisCustomer._isAnon || BillingAddress.m_addressID == 0 || ShippingAddress.m_addressID == 0)
				{
					Response.Redirect("createaccount.aspx?checkout=true&paymentmethod=" + Server.UrlEncode(PaymentMethod) + "&acds=" + acds.ToString());
				}
				else
				{
					switch(PaymentMethod.Replace(" ","").Trim().ToUpper())
					{
						case "CREDITCARD":
							if(Common.AppConfig("PaymentGateway").Trim().ToUpper() == "WORLDPAY JUNIOR" || Common.AppConfig("PaymentGateway").Trim().ToUpper() == "WORLDPAY")
							{
								Response.Redirect("checkoutworldpay.aspx?acds=" + acds.ToString());
							}
							else if(Common.AppConfig("PaymentGateway").Trim().ToUpper() == "2CHECKOUT")
							{
								Response.Redirect("checkouttwocheckout.aspx?acds=" + acds.ToString());
							}
							else
							{
								Response.Redirect("checkoutcard.aspx?acds=" + acds.ToString());
							}
							break;
						case "PURCHASEORDER":
							Response.Redirect("checkoutpo.aspx?acds=" + acds.ToString());
							break;
						case "PAYPAL":
							Response.Redirect("checkoutpaypal.aspx?acds=" + acds.ToString());
							break;
						case "REQUESTQUOTE":
							Response.Redirect("checkoutquote.aspx?acds=" + acds.ToString());
							break;
						case "CHECK":
							Response.Redirect("checkoutcheck.aspx?acds=" + acds.ToString());
							break;
						case "ECHECK":
							Response.Redirect("checkoutecheck.aspx?acds=" + acds.ToString());
							break;
						case "MICROPAY":
							if(Common.AppConfigBool("MicroPay.Enabled"))
							{
								Response.Redirect("checkoutmicropay.aspx?acds=" + acds.ToString());
							}
							else
							{
								// try cc checkout...user should never get here:
								if(Common.AppConfig("PaymentGateway").Trim().ToUpper() == "WORLDPAY JUNIOR" || Common.AppConfig("PaymentGateway").Trim().ToUpper() == "WORLDPAY")
								{
									Response.Redirect("checkoutworldpay.aspx?acds=" + acds.ToString());
								}
								else if(Common.AppConfig("PaymentGateway").Trim().ToUpper() == "2CHECKOUT")
								{
									Response.Redirect("checkouttwocheckout.aspx?acds=" + acds.ToString());
								}
								else
								{
									Response.Redirect("checkoutcard.aspx?acds=" + acds.ToString());
								}
							}
							break;
						default:
							if(Common.AppConfig("PaymentGateway").Trim().ToUpper() == "WORLDPAY JUNIOR" || Common.AppConfig("PaymentGateway").Trim().ToUpper() == "WORLDPAY")
							{
								Response.Redirect("checkoutworldpay.aspx?acds=" + acds.ToString());
							}
							else if(Common.AppConfig("PaymentGateway").Trim().ToUpper() == "2CHECKOUT")
							{
								Response.Redirect("checkouttwocheckout.aspx?acds=" + acds.ToString());
							}
							else
							{
								Response.Redirect("checkoutcard.aspx?acds=" + acds.ToString());
							}
							break;
					}
				}
			}
			else
			{
				if(cart.HasCoupon())
				{
					String CValid = cart.CouponIsValid();
					if(CValid != "OK")
					{
						Response.Redirect("ShoppingCart.aspx?resetlinkback=1&discountvalid=false&invalidreason=" + Server.UrlEncode(CValid) + "&acds=" + acds.ToString());

					}
				}
				Response.Redirect("ShoppingCart.aspx?resetlinkback=1&acds=" + acds.ToString());
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
