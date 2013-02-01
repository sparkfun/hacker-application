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

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for editaddress_process.
	/// </summary>
	public class editaddress_process : System.Web.UI.Page
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

			int AddressID = Common.QueryStringUSInt("AddressID");
			//      String ReturnURL = Common.QueryString("ReturnURL");
  
			String AddressTypeString = Common.QueryString("AddressType");
			AddressTypes AddressType = (AddressTypes)Enum.Parse(typeof(AddressTypes),AddressTypeString,true);
      
			int DeleteAddressID = Common.FormNativeInt("DeleteAddressID");
      if (DeleteAddressID == 0) 
				{
        DeleteAddressID = Common.QueryStringUSInt("DeleteAddressID");
				}
      if (DeleteAddressID != 0) 
				{
        Address theAddress = new Address();
        theAddress.DeleteDB(DeleteAddressID);
				Response.Redirect(String.Format("selectaddress.aspx?AddressType={0}",AddressType));
			}

			bool AllowShipToDifferentThanBillTo = Common.AppConfigBool("AllowShipToDifferentThanBillTo");

			Address thisAddress = new Address(thisCustomer._customerID);
			thisAddress.AddressID = AddressID;
			thisAddress.LoadFromDB(AddressID); 

			thisAddress.AddressType = AddressType;

			thisAddress.PaymentMethod = Common.Form("PaymentMethod");

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
			if ((thisAddress.AddressType & AddressTypes.Billing) != 0)
			{
				if (thisAddress.PaymentMethod.ToUpper().Replace(" ","") == "ECHECK")
				{
					thisAddress.ECheckBankABACode = Common.Form("ECheckBankABACode");
					thisAddress.ECheckBankAccountNumber = Common.Form("ECheckBankAccountNumber");
					thisAddress.ECheckBankName = Common.Form("ECheckBankName");
					thisAddress.ECheckBankAccountName = Common.Form("ECheckBankAccountName");
					thisAddress.ECheckBankAccountType = Common.Form("ECheckBankAccountType");
				}
				if (thisAddress.PaymentMethod.ToUpper().Replace(" ","") == "CREDITCARD")
				{
					thisAddress.CardName = Common.Form("CardName");    
					thisAddress.CardType = Common.Form("CardType");    

					string tmpS = Common.Form("CardNumber");
					if (!tmpS.StartsWith("***")) thisAddress.CardNumber = tmpS;
      
					tmpS = Common.Form("CardExtraCode");
					if (!tmpS.StartsWith("***")) thisAddress.CardExtraCode = tmpS;

					thisAddress.CardExpirationMonth = Common.Form("CardExpirationMonth");    
					thisAddress.CardExpirationYear = Common.Form("CardExpirationYear");    
				}
			}
			thisAddress.UpdateDB();

			Response.Redirect("selectaddress.aspx?" + Common.ServerVariables("QUERY_STRING"));
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
