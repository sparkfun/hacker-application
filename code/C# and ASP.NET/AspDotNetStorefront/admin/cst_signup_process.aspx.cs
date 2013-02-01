// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Text;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for cst_signup_process.
	/// </summary>
	public class cst_signup_process : SkinBase
	{

		int CustomerID;
		String EMailField;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			EMailField = Common.Form("EMail").ToLower();
			if(!Common.AppConfigBool("AllowCustomerDuplicateEMailAddresses"))
			{
				int NN = DB.GetSqlN("select count(*) as N from Customer  " + DB.GetNoLock() + " where EMail=" + DB.SQuote(EMailField) + " and CustomerID<>" + thisCustomer._customerID.ToString());
				if(NN > 0)
				{
					ErrorMsg = "That E-Mail Address is already Taken! Please use another E-mail address!";
				}
			}
			CustomerID = 0;
			

			if(ErrorMsg.Length == 0)
			{
				IDataReader rs;

				try
				{

					//					Address BillingAddress = new Address();
					//
					//					BillingAddress.FirstName = Common.Form("AddressFirstName");
					//					BillingAddress.LastName = Common.Form("AddressLastName");
					//					BillingAddress.Company = Common.Form("AddressCompany");
					//					BillingAddress.Address1 = Common.Form("AddressAddress1");
					//					BillingAddress.Address2 = Common.Form("AddressAddress2");
					//					BillingAddress.Suite = Common.Form("AddressSuite");
					//					BillingAddress.City = Common.Form("AddressCity");
					//					BillingAddress.State = Common.Form("AddressState");
					//					BillingAddress.Zip = Common.Form("AddressZip");
					//					BillingAddress.Country = Common.Form("AddressCountry");
					//					BillingAddress.Phone = Common.Form("AddressPhone");
					//          
					//					BillingAddress.Email = Common.Left(EMailField,100).ToLower();
        
					StringBuilder sql = new StringBuilder(2500);

					if(!Editing)
					{
						// ok to add them:
						String NewGUID = DB.GetNewGUID();
						sql.Append("insert into Customer(CustomerGUID,EMail,[Password],DateOfBirth,SubscriptionExpiresOn,Gender,OkToEmail,FirstName,LastName,Phone,CouponCode) values(");
						sql.Append(DB.SQuote(NewGUID) + ",");
						sql.Append(DB.SQuote(Common.Left(EMailField,100)) + ",");
						sql.Append(DB.SQuote(Common.Form("Password")) + ",");

						if(Common.Form("DateOfBirth").Length != 0)
						{
							try
							{
								DateTime dob = Localization.ParseNativeDateTime(Common.Form("DateOfBirth"));
								sql.Append(DB.DateQuote(Localization.ToNativeShortDateString(dob)) + ",");
							}
							catch
							{
								sql.Append("NULL,");
							}
						}
						else
						{
							sql.Append("NULL,");
						}
						if(Common.Form("SubscriptionExpiresOn").Length != 0)
						{
							try
							{
								DateTime seo = Localization.ParseNativeDateTime(Common.Form("SubscriptionExpiresOn"));
								sql.Append(DB.DateQuote(Localization.ToNativeShortDateString(seo)) + ",");
							}
							catch
							{
								sql.Append("NULL,");
							}
						}
						else
						{
							sql.Append("NULL,");
						}
						sql.Append(DB.SQuote(Common.Left(Common.Form("Gender"),1)) + ",");
						sql.Append(Common.Form("OkToEMail") + ",");
						sql.Append(DB.SQuote(Common.Left(Common.Form("FirstName"),50)) + ",");
						sql.Append(DB.SQuote(Common.Left(Common.Form("LastName"),50)) + ",");
						sql.Append(DB.SQuote(Common.Left(Common.Form("Phone"),25)) + ",");
						sql.Append(DB.SQuote(Common.Left(Common.Form("CouponCode"),50)));
						
						sql.Append(")");
						DB.ExecuteSQL(sql.ToString());

						rs = DB.GetRS("select CustomerID from Customer  " + DB.GetNoLock() + " where deleted=0 and CustomerGUID=" + DB.SQuote(NewGUID));
						rs.Read();
						CustomerID = DB.RSFieldInt(rs,"CustomerID");
						Editing = true;
						rs.Close();

//						// Finish adding the Billing and Shipping Address
//						BillingAddress.CustomerID = CustomerID;
//						BillingAddress.AddressType = AddressTypes.Billing;
//						BillingAddress.InsertDB();
//						BillingAddress.CopyToCustomerDB(BillingAddress.AddressType);
					}
					else
					{
						// ok to update:
						sql.Append("update Customer set ");
						sql.Append("EMail=" + DB.SQuote(Common.Left(EMailField,100)) + ",");
						sql.Append("[Password]=" + DB.SQuote(Common.Form("Password")) + ",");

						if(Common.Form("DateOfBirth").Length != 0)
						{
							try
							{
								DateTime dob = Localization.ParseNativeDateTime(Common.Form("DateOfBirth"));
								sql.Append("DateOfBirth=" + DB.DateQuote(Localization.ToNativeShortDateString(dob)) + ",");
							}
						catch
							{
								sql.Append("DateOfBirth=NULL,");
							}
						}
						else
						{
							sql.Append("DateOfBirth=NULL,");
						}
						if(Common.Form("SubscriptionExpiresOn").Length != 0)
						{
							try
							{
								DateTime seo = Localization.ParseNativeDateTime(Common.Form("SubscriptionExpiresOn"));
								sql.Append("SubscriptionExpiresOn=" + DB.DateQuote(Localization.ToNativeShortDateString(seo)) + ",");
							}
							catch
							{
								sql.Append("SubscriptionExpiresOn=NULL,");
							}
						}
						else
						{
							sql.Append("SubscriptionExpiresOn=NULL,");
						}
						//sql.Append("Gender=" + DB.SQuote(Common.Left(Common.Form("Gender"),1)) + ",");
						sql.Append("FirstName=" + DB.SQuote(Common.Left(Common.Form("FirstName"),50)) + ",");
						sql.Append("LastName=" + DB.SQuote(Common.Left(Common.Form("LastName"),50)) + ",");
						sql.Append("Phone=" + DB.SQuote(Common.Left(Common.Form("Phone"),25)) + ",");
						sql.Append("CouponCode=" + DB.SQuote(Common.Left(Common.Form("CouponCode"),50)) + ",");
						sql.Append("LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
						sql.Append("LastUpdatedBy=" + thisCustomer._customerID.ToString() + " ");
						sql.Append("where CustomerID=" + CustomerID.ToString());
						DB.ExecuteSQL(sql.ToString());
						Editing = true;
//						// Update the Billing Address 
//						BillingAddress.CustomerID = CustomerID;
//						BillingAddress.UpdateDB();
					}
				}
				catch(Exception ex)
				{
					ErrorMsg = "<p><b>ERROR: " + Common.GetExceptionDetail(ex,"<br>") + "<br><br></b></p>";
				}

			}
			SectionTitle = "<a href=\"Customers.aspx\">Customers</a> - Add new Customer";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(ErrorMsg.Length > 0)
			{
				writer.Write("<p align=\"left\"><b><font color=\"red\">" + ErrorMsg + "</font></b></p>");
				writer.Write("<p align=\"left\">Please <a href=\"javascript:history.back(-1);\">go back</a> and correct the error. Thanks.</p>");
			}
			else
			{
//				try
//				{
//					// send welcome e-mail:
//					String WelcomeMsg = new EMailTemplate("newmember",thisCustomer._localeSetting,_siteID)._contents;
//					WelcomeMsg = WelcomeMsg.Replace("%CustomerID%",CustomerID.ToString());
//					WelcomeMsg = WelcomeMsg.Replace("%PASSWORD%",Common.Form("Password"));
//					WelcomeMsg = WelcomeMsg.Replace("%EMAIL%",Common.Form("EMail"));
//					WelcomeMsg = WelcomeMsg.Replace("%STORENAME%",Common.AppConfig("StoreName"));
//					WelcomeMsg = WelcomeMsg.Replace("%STOREURL%",Common.GetStoreHTTPLocation(false));
//					Common.SendMail(Common.AppConfig("StoreName") + " Welcome",WelcomeMsg,true,Common.AppConfig("AffiliateEMailAddress"),Common.AppConfig("AffiliateEMailAddress"),EMailField,EMailField,"",Common.AppConfig("MailMe_Server"));
//				}
//				catch {}
				writer.Write("<p align=\"left\"><b>Customer add was successful.<br><br><a href=\"cst_account.aspx?Customerid=" + CustomerID.ToString() + "\">Click here</a> to go to their account page.</p>");
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
