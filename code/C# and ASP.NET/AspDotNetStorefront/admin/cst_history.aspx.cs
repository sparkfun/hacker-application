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
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for cst_history.
	/// </summary>
	public class cst_history : SkinBase
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
			SectionTitle = "<a href=\"Customers.aspx?searchfor=" + targetCustomer._customerID.ToString() + "\">Customers</a> - Order History: " + targetCustomer.FullName() + " (" + targetCustomer._email + ")";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{

      //If there is a deleteid remove it from the cart
      int DeleteID = Common.QueryStringUSInt("deleteID");
      if (DeleteID != 0)
      {
        DB.ExecuteSQL(String.Format("delete from customcart where  customerid={0} and ShoppingCartRecID={1}",targetCustomer._customerID,DeleteID));
        DB.ExecuteSQL(String.Format("delete from kitcart where  customerid={0} and ShoppingCartRecID={1}",targetCustomer._customerID,DeleteID));
        DB.ExecuteSQL(String.Format("delete from ShoppingCart where  customerid={0} and ShoppingCartRecID={1}",targetCustomer._customerID,DeleteID));
      }
      if(ShoppingCart.NumItems(targetCustomer._customerID,CartTypeEnum.RecurringCart) != 0)
			{

				writer.Write("<p align=\"left\"><b>This customer has active recurring (auto-ship) orders.</b></p>\n");
				IDataReader rsr = DB.GetRS("Select distinct OriginalRecurringOrderNumber from ShoppingCart  " + DB.GetNoLock() + " where CartType=" + ((int)CartTypeEnum.RecurringCart).ToString() + " and CustomerID=" + targetCustomer._customerID.ToString() + " order by OriginalRecurringOrderNumber desc");
				while(rsr.Read())
				{
					writer.Write(Common.GetRecurringCart(targetCustomer,DB.RSFieldInt(rsr,"OriginalRecurringOrderNumber"),_siteID,false));
				}
				rsr.Close();
			}
			
			writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			writer.Write("<a href=\"news.aspx\"><img src=\"skins/Skin_" + _siteID.ToString() + "/images/orderhistory.gif\" border=\"0\"></a><br>");
			writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			
			writer.Write("<p align=\"left\" ><b>The customer's order/billing history is shown below. Click on any order number for details.</b></p>");
			int N = 0;
			IDataReader rs = DB.GetRS("Select * from orders  " + DB.GetNoLock() + " where CustomerID=" + targetCustomer._customerID.ToString() + " order by OrderDate desc");
			writer.Write("<table align=\"center\" width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"1\">\n");
			writer.Write("<tr bgcolor=\"CCCCCC\">\n");
			writer.Write("<td valign=\"top\"><b>Order Number</b><br><small>(Click For Receipt)</small></td>\n");
			writer.Write("<td valign=\"top\"><b>Order Date</b></td>\n");
			writer.Write("<td valign=\"top\"><b>Payment Status</b></td>\n");
			writer.Write("<td valign=\"top\"><b>Shipping Status</b></td>\n");
			//writer.Write("<td valign=\"top\"><b>Customer ID</b></td>\n");
			//writer.Write("<td valign=\"top\"><b>Customer Name</b></td>\n");
			//writer.Write("<td valign=\"top\"><b>Customer E-Mail</b></td>\n");
			//writer.Write("<td valign=\"top\"><b>Company</b></td>\n");
			writer.Write("<td valign=\"top\"><b>Order Total</b></td>\n");
			if(Common.AppConfigBool("ShowCustomerServiceNotesInReceipts"))
			{
				writer.Write("<td valign=\"top\"><b>Customer Service Notes</b></td>\n");
			}
			writer.Write("</tr>\n");
			while(rs.Read())
			{
				String PaymentStatus = String.Empty;
				if(DB.RSField(rs,"PaymentMethod").Length != 0)
				{
					PaymentStatus = "Payment Method: " + DB.RSField(rs,"PaymentMethod") + "<br>";
				}
				else
				{
					PaymentStatus = "Payment Method: " + Common.IIF(DB.RSField(rs,"CardNumber").ToUpper() == "PAYPAL" , "PayPal" , "Credit Card") + "<br>";
				}
				if(DB.RSFieldDateTime(rs,"PaymentClearedOn") != System.DateTime.MinValue)
				{
					PaymentStatus += " Cleared On: " + Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs,"PaymentClearedOn"));
				}
				else
				{
					PaymentStatus += " Pending";
				}

				String ShippingStatus = String.Empty;
				if(Common.OrderHasShippableComponents(DB.RSFieldInt(rs,"OrderNumber")))
				{
					if(DB.RSFieldDateTime(rs,"ShippedOn") != System.DateTime.MinValue)
					{
						ShippingStatus = "Shipped";
						if(DB.RSField(rs,"ShippedVIA").Length != 0)
						{
							ShippingStatus += " via " + DB.RSField(rs,"ShippedVIA");
						}
						ShippingStatus += " on " + Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs,"ShippedOn")) + ".";
						if(DB.RSField(rs,"ShippingTrackingNumber").Length != 0)
						{
							ShippingStatus += " Tracking Number: " + DB.RSField(rs,"ShippingTrackingNumber");
						}
					}
					else
					{
						ShippingStatus = "Not Yet Shipped";
					}
				}
				if(Common.OrderHasDownloadComponents(DB.RSFieldInt(rs,"OrderNumber")))
				{
					if(DB.RSFieldDateTime(rs,"PaymentClearedOn") != System.DateTime.MinValue && DB.RSFieldDateTime(rs,"DownloadEMailSentOn") != System.DateTime.MinValue)
					{
						Order ord = new Order(DB.RSFieldInt(rs,"OrderNumber"));
						if(ShippingStatus.Length != 0)
						{
							ShippingStatus += "<hr size=1>";
						}
						ShippingStatus += ord.GetDownloadList(false);
						ord = null;
					}
					else
					{
						if(ShippingStatus.Length == 0)
						{
							ShippingStatus += "Download List Pending Payment";
						}
					}
				}
				writer.Write("<tr>\n");
				writer.Write("<td valign=\"top\"><a href=\"" + Common.GetStoreHTTPLocation(true) + "receipt.aspx?ordernumber=" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "&customerid=" + DB.RSFieldInt(rs,"CustomerID").ToString() + "\" target=\"_blank\">" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "</a></td>");
				writer.Write("<td valign=\"top\">" + Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs,"OrderDate")));
				writer.Write("</td>");
				writer.Write("<td valign=\"top\">" + PaymentStatus + "&nbsp;" + "</td>");
				writer.Write("<td valign=\"top\">" + ShippingStatus + "&nbsp;" + "</td>");
				if(Common.AppConfigBool("ShowCustomerServiceNotesInReceipts"))
				{
					writer.Write("<td valign=\"top\">" + Common.IIF(DB.RSField(rs,"CustomerServiceNotes").Length == 0 , "None" , DB.RSField(rs,"CustomerServiceNotes")) + "</td>");
				}
				writer.Write("</tr>\n");
				N++;
			}
			writer.Write("</table>\n");
			rs.Close();
			if(N == 0)
			{
				writer.Write("<p align=\"left\">No orders found</p>\n");
			}	
			
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");
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
