// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for cst_account_process.
	/// </summary>
	public class cst_account_process : System.Web.UI.Page
	{

    private void Page_Load(object sender, System.EventArgs e)
    {
      Response.CacheControl="private";
      Response.Expires=0;
      Response.AddHeader("pragma", "no-cache");

      Customer thisCustomer = new Customer();

      Customer targetCustomer = new Customer(Common.QueryStringUSInt("CustomerID"),true);
      if(targetCustomer._customerID == 0)
      {
        Response.Redirect("Customers.aspx");
      }

      String EMailField = Common.Form("EMail").ToLower();
      bool EMailAlreadyTaken = false;
      if(!Common.AppConfigBool("AllowCustomerDuplicateEMailAddresses"))
      {
        int NN = DB.GetSqlN("select count(*) as N from customer  " + DB.GetNoLock() + " where EMail=" + DB.SQuote(EMailField) + " and CustomerID<>" + targetCustomer._customerID.ToString());
        if(NN > 0)
        {
          EMailAlreadyTaken = true;
        }
      }

      String PaymentMethod = Common.QueryString("PaymentMethod");

      // if discount code was entered, see if it's valid, If not put up error message
      String DiscountValid = String.Empty;
      if(Common.Form("CouponCode").Length != 0)
      {
        DiscountValid = "&discounterror=1";
        IDataReader rsdis = DB.GetRS("select * from Coupon  " + DB.GetNoLock() + " where lower(CouponCode)=" + DB.SQuote(Common.Form("CouponCode").ToLower()) + " and expirationdate>=getdate() and deleted=0");
        if(rsdis.Read())
        {
          DiscountValid = String.Empty;
        }
        rsdis.Close();
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
      if(DiscountValid.Length == 0) // meaning no error
      {
        sql.Append("CouponCode=" + DB.SQuote(Common.Form("CouponCode")) + ",");
      }
      else
      {
        sql.Append("CouponCode=NULL,");
      }

      sql.Append("FirstName=" + DB.SQuote(Common.Form("FirstName")) + ",");
      sql.Append("LastName=" + DB.SQuote(Common.Form("LastName")) + ",");
      if(!EMailAlreadyTaken)
      {
        sql.Append("EMail=" + DB.SQuote(EMailField )+ ",");
      }
      else
      {
        sql.Append("EMail=" + DB.SQuote(thisCustomer._email.ToLower()) + ",");
      }
      sql.Append("Phone=" + DB.SQuote(Common.Form("Phone")) + ",");
      sql.Append("[Password]=" + DB.SQuote(Common.Form("Password")) + ",");

      if(Common.AppConfigBool("MicroPay.Enabled"))
      {
        sql.Append("MicroPayBalance=" + Common.FormUSSingle("MicroPayBalance") + ",");
      }
      sql.Append("OkToEmail=" + Common.FormUSSingle("OkToEmail").ToString());
      sql.Append(" where customerid=" + targetCustomer._customerID.ToString());

      try
      {
        DB.ExecuteSQL(sql.ToString());
      }
      catch(Exception ex)
      {
        Response.Redirect("cst_account.aspx?customerid=" + targetCustomer._customerID.ToString() + "&errormsg=" + Server.UrlEncode(ex.Message));
      }

	  	if(Common.Form("SubscriptionExpiresOn").Length == 0)
			{
				DB.ExecuteSQL("update customer set SubscriptionExpiresOn=NULL where CustomerID=" + targetCustomer._customerID.ToString());
			}
			else
			{
				DB.ExecuteSQL("update customer set SubscriptionExpiresOn=" + DB.DateQuote(Common.Form("SubscriptionExpiresOn")) + " where CustomerID=" + targetCustomer._customerID.ToString());
			}

      Address BillingAddress = new Address(targetCustomer._customerID);

      if (thisCustomer._isAnon) // Save address in both Shipping and Billing
      {
        BillingAddress.AddressType = AddressTypes.Billing | AddressTypes.Shipping;
        BillingAddress.FirstName = Common.Form("AddressFirstName");
        BillingAddress.LastName = Common.Form("AddressLastName");
        BillingAddress.Company = Common.Form("AddressCompany");
        BillingAddress.Address1 = Common.Form("AddressAddress1");
        BillingAddress.Address2 = Common.Form("AddressAddress2");
        BillingAddress.Suite = Common.Form("AddressSuite");
        BillingAddress.City = Common.Form("AddressCity");
        BillingAddress.State = Common.Form("AddressState");
        BillingAddress.Zip = Common.Form("AddressZip");
        BillingAddress.Country = Common.Form("AddressCountry");
        BillingAddress.Phone = Common.Form("AddressPhone");
          
        BillingAddress.Email = targetCustomer._email.ToLower();
        
        BillingAddress.InsertDB();
        BillingAddress.CopyToCustomerDB(BillingAddress.AddressType);
      }

		if(!EMailAlreadyTaken)
      {
        Response.Redirect("cst_account.aspx?customerid=" + targetCustomer._customerID.ToString() + "&msg=Updated");
      }
      else
      {
        Response.Redirect("cst_account.aspx?customerid=" + targetCustomer._customerID.ToString() + "&errormsg=That+E-Mail+Address+is+already+Taken!+Please+use+another+e-mail+address.");
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
