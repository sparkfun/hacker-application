// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2004.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for createaccount_process.
	/// </summary>
	public class createaccount_process : System.Web.UI.Page
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!Common.ReferrerOKForSubmit())
			{
				Response.Redirect("default.aspx?acds=failed");
			}
			Customer thisCustomer = new Customer();
			thisCustomer.RequireCustomerRecord();
			
			bool AllowShipToDifferentThanBillTo = Common.AppConfigBool("AllowShipToDifferentThanBillTo");
			bool acds = Common.AppConfigBool("AllowCrossDomainSubmit");

			bool DoingCheckout = (Common.QueryString("checkout").ToLower() == "true");

			String EMailField = Common.Form("EMail").ToLower();
			bool EMailAlreadyTaken = false;

			if(!Common.AppConfigBool("AllowCustomerDuplicateEMailAddresses"))
			{
				int NN = DB.GetSqlN("select count(*) as N from customer  " + DB.GetNoLock() + " where EMail=" + DB.SQuote(EMailField) + " and CustomerID<>" + Common.SessionUSInt("CustomerID").ToString());
				if(NN > 0)
				{
					EMailAlreadyTaken = true;
				}
			}

			String PaymentMethod = Common.QueryString("PaymentMethod");

			string ReturnURL = Common.QueryString("ReturnURL");
			string AddressTypeString = Common.QueryString("AddressType");
      
			AddressTypes AddressType = AddressTypes.Unknown;
			if (AddressTypeString.Length != 0)
			{
				AddressType = (AddressTypes)Enum.Parse(typeof(AddressTypes),AddressTypeString,true);
			}

			StringBuilder sql = new StringBuilder(10000);
			sql.Append("update customer set ");
			if(Common.Form("BillingEqualsShipping").Length != 0)
			{
				sql.Append("BillingEqualsShipping=1,");
			}
			if(thisCustomer._isAnon)
			{
				sql.Append("RegisterDate=" + DB.SQuote(Localization.ToNativeDateTimeString(DateTime.Now)) + ",");
			}
			sql.Append("FirstName=" + DB.SQuote(Common.Form("FirstName")) + ",");
			sql.Append("LastName=" + DB.SQuote(Common.Form("LastName")) + ",");
			sql.Append("EMail=" + DB.SQuote(EMailField )+ ",");
			sql.Append("Phone=" + DB.SQuote(Common.Form("Phone")) + ",");
			sql.Append("[Password]=" + DB.SQuote(Common.MungeString(Common.Form("Password"))) + ",");
			sql.Append("OkToEmail=" + Common.FormNativeInt("OkToEmail").ToString());
			sql.Append(" where customerid=" + thisCustomer._customerID.ToString());

			DB.ExecuteSQL(sql.ToString());

			Address BillingAddress = new Address();
			Address ShippingAddress  = new Address();
      
			BillingAddress.LastName=Common.Form("BillingLastName");
			BillingAddress.FirstName=Common.Form("BillingFirstName");
			BillingAddress.Phone=Common.Form("BillingPhone");
			BillingAddress.Company=Common.Form("BillingCompany");
			BillingAddress.Address1=Common.Form("BillingAddress1");
			BillingAddress.Address2=Common.Form("BillingAddress2");
			BillingAddress.Suite=Common.Form("BillingSuite");
			BillingAddress.City=Common.Form("BillingCity");
			BillingAddress.State=Common.Form("BillingState");
			BillingAddress.Zip=Common.Form("BillingZip");
			BillingAddress.Country=Common.Form("BillingCountry");
			BillingAddress.Email=EMailField;
      
			BillingAddress.InsertDB(thisCustomer._customerID);
			BillingAddress.CopyToCustomerDB(AddressTypes.Billing);
      
         	if(AllowShipToDifferentThanBillTo)
			{
				ShippingAddress.LastName= Common.Form("ShippingLastName");
				ShippingAddress.FirstName=Common.Form("ShippingFirstName");
				ShippingAddress.Phone=Common.Form("ShippingPhone");
				ShippingAddress.Company=Common.Form("ShippingCompany");
				ShippingAddress.Address1=Common.Form("ShippingAddress1");
				ShippingAddress.Address2=Common.Form("ShippingAddress2");
				ShippingAddress.Suite=Common.Form("ShippingSuite");
				ShippingAddress.City=Common.Form("ShippingCity");
				ShippingAddress.State=Common.Form("ShippingState");
				ShippingAddress.Zip=Common.Form("ShippingZip");
				ShippingAddress.Country=Common.Form("ShippingCountry");
				ShippingAddress.Email=EMailField;
        
				ShippingAddress.InsertDB(thisCustomer._customerID);
				ShippingAddress.CopyToCustomerDB(AddressTypes.Shipping);
			}
			else
			{
				BillingAddress.CopyToCustomerDB(AddressTypes.Shipping);
			}

			if(Common.AppConfigBool("SendEMailOnCustomerSignup"))
			{
				try
				{
					Common.SendMail("New Site User Signup", thisCustomer._customerID + "(" + thisCustomer.FullName(), true, Common.AppConfig("GotOrderEMailFrom"),Common.AppConfig("GotOrderEMailFromName"),Common.AppConfig("GotOrderEMailTo"),Common.AppConfig("GotOrderEMailTo"),"",Common.AppConfig("MailMe_Server"));
				}
				catch {}
			}

			if(DoingCheckout)
			{
				if(EMailAlreadyTaken)
				{
					DB.ExecuteSQL("update customer set EMail=" + DB.SQuote("Anon_" + Common.GetNewGUID()) + " where customerid=" + thisCustomer._customerID);
					Response.Redirect("createaccount.aspx?errormsg=" + Server.UrlEncode("That EMail Address is Already Used By Another Customer"));
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
								Response.Redirect("checkoutcard.aspx?acds=" + acds.ToString());
							}
							break;
						default:
							Response.Redirect("checkoutcard.aspx?acds=" + acds.ToString());
							break;
					}
				}
			}
			else
			{
				Response.Redirect("account.aspx");
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
