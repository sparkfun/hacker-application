// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for cst_recurring.
	/// </summary>
	public class cst_recurring : SkinBase
	{

		private Customer targetCustomer;
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			targetCustomer = new Customer(Common.QueryStringUSInt("CustomerID"),true);
			if(targetCustomer._customerID == 0)
			{
				Response.Redirect("Customers.aspx");
			}
			if(Common.QueryStringUSInt("DeleteID") != 0)
			{
				DB.ExecuteSQL("delete from ShoppingCart where CustomerID=" + targetCustomer._customerID.ToString() + " and ShoppingCartRecID=" + Common.QueryStringUSInt("DeleteID").ToString());
				DB.ExecuteSQL("delete from kitcart where CustomerID=" + targetCustomer._customerID.ToString() + " and ShoppingCartRecID=" + Common.QueryStringUSInt("DeleteID").ToString());
			}

			if(Common.FormUSInt("RecurringInterval") != 0)
			{
				int OriginalRecurringOrderNumber = Common.FormUSInt("OriginalRecurringOrderNumber");
				int NewRecurringInterval = Common.FormUSInt("RecurringInterval");
				RecurringIntervalTypeEnum NewRecurringIntervalType = (RecurringIntervalTypeEnum)Common.FormUSInt("RecurringIntervalType");

				DateTime CreatedOnDate = System.DateTime.MinValue;
				DateTime LastRecurringShipDate = System.DateTime.MinValue;
				int RecurringIndex = 1;
				int CurrentRecurringInterval = 0;
				RecurringIntervalTypeEnum CurrentRecurringIntervalType = RecurringIntervalTypeEnum.Monthly;

				IDataReader rs2 = DB.GetRS("select CreatedOn, NextRecurringShipDate,RecurringIndex,RecurringInterval,RecurringIntervalType from ShoppingCart  " + DB.GetNoLock() + " where CustomerID=" + targetCustomer._customerID.ToString() + " and CartType=" + ((int)CartTypeEnum.RecurringCart).ToString() + " and OriginalRecurringOrderNumber=" + OriginalRecurringOrderNumber.ToString()); 
				if(rs2.Read())
				{
					CurrentRecurringInterval = DB.RSFieldInt(rs2,"RecurringInterval");
					CurrentRecurringIntervalType = (RecurringIntervalTypeEnum)DB.RSFieldInt(rs2,"RecurringIntervalType");
					RecurringIndex = DB.RSFieldInt(rs2,"RecurringIndex");
					CreatedOnDate = DB.RSFieldDateTime(rs2,"CreatedOn");
					LastRecurringShipDate = DB.RSFieldDateTime(rs2,"NextRecurringShipDate"); // this must be "fixed" up below...we need the PRIOR ship date, not the date of next schedule ship
				}
				rs2.Close();

//				// figure out when the order was last shipped:
//				if(RecurringIndex == 1)
//				{
//					LastRecurringShipDate = CreatedOnDate;
//				}
//				else
//				{
//					if(LastRecurringShipDate != System.DateTime.MinValue)
//					{
//						switch(NewRecurringIntervalType)
//						{
//							case RecurringIntervalTypeEnum.Daily:
//								LastRecurringShipDate = LastRecurringShipDate.AddDays(-CurrentRecurringInterval);
//								break;
//							case RecurringIntervalTypeEnum.Weekly:
//								LastRecurringShipDate = LastRecurringShipDate.AddDays(-7 * CurrentRecurringInterval);
//								break;
//							case RecurringIntervalTypeEnum.Monthly:
//								LastRecurringShipDate = LastRecurringShipDate.AddMonths(-CurrentRecurringInterval);
//								break;
//							case RecurringIntervalTypeEnum.Yearly:
//								LastRecurringShipDate = LastRecurringShipDate.AddYears(-CurrentRecurringInterval);
//								break;
//							default:
//								LastRecurringShipDate = LastRecurringShipDate.AddMonths(-CurrentRecurringInterval);
//								break;
//						}
//					}
//				}

				LastRecurringShipDate = System.DateTime.Now;
				DateTime NewShipDate = System.DateTime.MinValue;
				if(LastRecurringShipDate != System.DateTime.MinValue)
				{
					switch(NewRecurringIntervalType)
					{
						case RecurringIntervalTypeEnum.Daily:
							NewShipDate = LastRecurringShipDate.AddDays(NewRecurringInterval);
							break;
						case RecurringIntervalTypeEnum.Weekly:
							NewShipDate = LastRecurringShipDate.AddDays(7 * NewRecurringInterval);
							break;
						case RecurringIntervalTypeEnum.Monthly:
							NewShipDate = LastRecurringShipDate.AddMonths(NewRecurringInterval);
							break;
						case RecurringIntervalTypeEnum.Yearly:
							NewShipDate = LastRecurringShipDate.AddYears(NewRecurringInterval);
							break;
						default:
							NewShipDate = LastRecurringShipDate.AddMonths(NewRecurringInterval);
							break;
					}
					DB.ExecuteSQL("update ShoppingCart set RecurringInterval=" + NewRecurringInterval.ToString() + ", RecurringIntervalType=" + ((int)NewRecurringIntervalType).ToString() + ", NextRecurringShipDate=" + DB.DateQuote(NewShipDate) + " where CustomerID=" + targetCustomer._customerID.ToString() + " and CartType=" + ((int)CartTypeEnum.RecurringCart).ToString() + " and OriginalRecurringOrderNumber=" + OriginalRecurringOrderNumber.ToString());
				}
			}		
			SectionTitle = "<a href=\"Customers.aspx?searchfor=" + targetCustomer._customerID.ToString() + "\">Customers</a> - <a href=\"cst_history.aspx?customerid=" + targetCustomer._customerID.ToString() + "\">Order History</a> - Recurring Shipments For: " + targetCustomer.FullName() + " (" + targetCustomer._email + ")";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{

			if(ShoppingCart.NumItems(targetCustomer._customerID,CartTypeEnum.RecurringCart) == 0)
			{
				writer.Write("<p align=\"left\"><b>No active recurring (auto-ship) orders found.</b></p>\n");
			}
			else
			{
				writer.Write("<p align=\"left\"><b>This customer has active recurring (auto-ship) orders.</b></p>\n");
				IDataReader rsr = DB.GetRS("Select distinct OriginalRecurringOrderNumber from ShoppingCart  " + DB.GetNoLock() + " where CartType=" + ((int)CartTypeEnum.RecurringCart).ToString() + " and CustomerID=" + targetCustomer._customerID.ToString() + " order by OriginalRecurringOrderNumber desc");
				while(rsr.Read())
				{
					writer.Write(Common.GetRecurringCart(targetCustomer,DB.RSFieldInt(rsr,"OriginalRecurringOrderNumber"),_siteID,false));
				}
				rsr.Close();
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
