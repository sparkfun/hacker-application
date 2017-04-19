// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for cst_selectaddress_process.
	/// </summary>
	public class cst_selectaddress_process : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");
			
			Customer thisCustomer = new Customer();

			int AddressID = Common.QueryStringUSInt("AddressID");
			int CustomerID = Common.QueryStringUSInt("CustomerID");
			int OriginalRecurringOrderNumber = Common.QueryStringUSInt("OriginalRecurringOrderNumber");
			string ReturnUrl = Common.QueryString("ReturnUrl");
			String AddressTypeString = Common.QueryString("AddressType");
      		AddressTypes AddressType = (AddressTypes)Enum.Parse(typeof(AddressTypes),AddressTypeString,true);
      
			bool AllowShipToDifferentThanBillTo = Common.AppConfigBool("AllowShipToDifferentThanBillTo");
			if(!AllowShipToDifferentThanBillTo) 
			{
				//Shipping and Billing address nust be the same so save both
				AddressType = AddressTypes.Billing | AddressTypes.Shipping;
			}

			Address thisAddress = new Address();

			if (AddressID != 0) //Users Selected an ID from the Address Grid
			{
				if (OriginalRecurringOrderNumber == 0)
				{
					thisAddress.LoadFromDB(AddressID);
					thisAddress.CopyToCustomerDB(AddressType);
				}
			}
			else  //Entered a new address to add
			{
				thisAddress.CustomerID = CustomerID;
				thisAddress.FirstName = Common.Form("AddressFirstName");
				thisAddress.LastName = Common.Form("AddressLastName");
				thisAddress.Company = Common.Form("AddressCompany");
				thisAddress.Address1 = Common.Form("AddressAddress1");
				thisAddress.Address2 = Common.Form("AddressAddress2");
				thisAddress.Suite = Common.Form("AddressSuite");
				thisAddress.City = Common.Form("AddressCity");
				thisAddress.State = Common.Form("AddressState");
				thisAddress.Zip = Common.Form("AddressZip");
				thisAddress.Country = Common.Form("AddressCountry");
				thisAddress.Phone = Common.Form("AddressPhone");
       
				thisAddress.InsertDB();
				AddressID = thisAddress.AddressID;

				if (OriginalRecurringOrderNumber ==0)
				{
					thisAddress.CopyToCustomerDB(AddressType);
				}
			}
			if (OriginalRecurringOrderNumber != 0)
			{
				//put it in the ShoppingCart record
				string sql = String.Empty;
				if ((AddressType & AddressTypes.Billing) != 0)
				{
					sql = String.Format("BillingAddressID={0}",AddressID);
				}
				if ((AddressType & AddressTypes.Shipping) != 0)
				{
					if (sql.Length != 0)
					{
					sql += ",";
					}
					sql += String.Format("ShippingAddressID={0}",AddressID);
				}
				sql = String.Format("update ShoppingCart set " + sql + " where OriginalRecurringOrderNumber={0}",OriginalRecurringOrderNumber);
				DB.ExecuteSQL(sql);
			}

			Response.Redirect(ReturnUrl);
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
