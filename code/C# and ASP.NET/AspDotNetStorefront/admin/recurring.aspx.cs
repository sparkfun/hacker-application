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
using System.Xml;
using System.Xml.Serialization;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections;
using System.IO;
using AspDotNetStorefrontCommon;
using AspDotNetStorefrontGateways;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for recurring.
	/// </summary>
	public class recurring : SkinBase
	{

		private String ProcessRecurringCustomer(Customer thisCustomer, int ProcessCustomerID,int OriginalRecurringOrderNumber)
		{
			// Need to process each individual order group separately
			String status = "OK";
			if(ShoppingCart.NumItems(ProcessCustomerID,CartTypeEnum.RecurringCart) != 0)
			{
				IDataReader rsr = DB.GetRS(String.Format("Select distinct OriginalRecurringOrderNumber from ShoppingCart  " + DB.GetNoLock() + " where CartType={0} and CustomerID={1} {2} order by OriginalRecurringOrderNumber desc",(int)CartTypeEnum.RecurringCart,ProcessCustomerID,Common.IIF((OriginalRecurringOrderNumber != 0),String.Format(" and OriginalRecurringOrderNumber={0}",OriginalRecurringOrderNumber),"")));
				while(rsr.Read())
				{
					ShoppingCart c = new ShoppingCart(_siteID,new Customer(ProcessCustomerID,true),CartTypeEnum.RecurringCart,DB.RSFieldInt(rsr,"OriginalRecurringOrderNumber"),true);
					if (c._cartItems.Count != 0)
					{
						int OrderNumber = Common.GetNextOrderNumber();
						status = Gateway.MakeRecurringOrder(c,OrderNumber);
						if(status == "OK")
						{
							// make recurringshipment record:
							StringBuilder sql = new StringBuilder(2500);
							// ok to add them:
							String NewGUID = DB.GetNewGUID();
							sql.Append("insert into RecurringShipments(RecurringShipmentsGUID,CustomerID,OrderNumber,ShippingFirstName,ShippingLastName,ShippingCompany,ShippingAddress1,ShippingAddress2,ShippingSuite,ShippingCity,ShippingState,ShippingZip,ShippingCountry,ShippingPhone,ShippingMethodID,ShippingMethod,PackingList,OrderXML,LastUpdatedBy) values(");
							sql.Append(DB.SQuote(NewGUID) + ",");
							sql.Append(ProcessCustomerID.ToString() + ",");
							sql.Append(OrderNumber.ToString() + ",");
							sql.Append(DB.SQuote(Common.Left(c._shippingAddress.FirstName,50)) + ",");
							sql.Append(DB.SQuote(Common.Left(c._shippingAddress.LastName,50)) + ",");
							sql.Append(DB.SQuote(Common.Left(c._shippingAddress.Company,50)) + ",");
							sql.Append(DB.SQuote(Common.Left(c._shippingAddress.Address1,50)) + ",");
							sql.Append(DB.SQuote(Common.Left(c._shippingAddress.Address2,50)) + ",");
							sql.Append(DB.SQuote(Common.Left(c._shippingAddress.Suite,50)) + ",");
							sql.Append(DB.SQuote(Common.Left(c._shippingAddress.City,50)) + ",");
							sql.Append(DB.SQuote(Common.Left(c._shippingAddress.State,50)) + ",");
							sql.Append(DB.SQuote(Common.Left(c._shippingAddress.Zip,50)) + ",");
							sql.Append(DB.SQuote(Common.Left(c._shippingAddress.Country,50)) + ",");
							sql.Append(DB.SQuote(Common.Left(c._shippingAddress.Phone,50)) + ",");
							sql.Append(c._shippingMethodID.ToString() + ",");
							sql.Append(DB.SQuote(c._shippingMethod) + ",");

							Order ord = new Order(OrderNumber);
				
							System.Type[] extraTypes = new System.Type[1];
							extraTypes[0] = typeof(CartItem);
							MemoryStream memoryStream = new MemoryStream();
							XmlSerializer xs = new XmlSerializer(typeof(Order),extraTypes);
							XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
							xs.Serialize(xmlTextWriter, ord);
							memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
							String OrderXML = Common.UTF8ByteArrayToString(memoryStream.ToArray());

							sql.Append(DB.SQuote(ord.GetPackingList(", ","<br>")) + ",");
							sql.Append(DB.SQuote(OrderXML) + ","); // packinglist tbd
							ord = null;

							sql.Append(thisCustomer._customerID.ToString() + ")");
							DB.ExecuteSQL(sql.ToString());

							Common.SendOrderEMail(thisCustomer,OrderNumber,true, "CREDIT CARD",false); // TBD: MICROPAY ALSO could be used, but how would we know?
						}
						else
						{
							break;
						}
					}
					c = null;
				}
				rsr.Close();
			}
			return status;
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Recurring Shipments " + Common.IIF(Common.QueryString("Show").ToUpper() != "ALL", "Due Today", "All Pending");
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(Common.QueryString("clearshippinglist").ToUpper() == "TRUE")
			{
				DB.ExecuteSQL("delete from recurringshipments");
				writer.Write("<p><b>RecurringShipping Table Cleared</b></p>");
			}

			if(Common.QueryString("ProcessAll").ToUpper() == "TRUE")
			{
				IDataReader rsp = DB.GetRS("Select distinct(customerid) from ShoppingCart where CartType=" + ((int)CartTypeEnum.RecurringCart).ToString() + " and NextRecurringShipDate<" + DB.SQuote(Localization.ToNativeShortDateString(System.DateTime.Now.AddDays(1))));
				while(rsp.Read())
				{
					writer.Write("Processing Customer " + DB.RSFieldInt(rsp,"CustomerID").ToString() + "...");
					writer.Write(ProcessRecurringCustomer(thisCustomer,DB.RSFieldInt(rsp,"CustomerID"),0));
					writer.Write("...<br>");
				}
				rsp.Close();
			}
				
			if(Common.QueryString("MarkAllShipped").ToUpper() == "TRUE")
			{
				IDataReader rsp = DB.GetRS("Select * from recurringshipments " + DB.GetNoLock());
				while(rsp.Read())
				{
					writer.Write("Marking Customer Order #" + DB.RSFieldInt(rsp,"OrderNumber").ToString() + " As Shipped...");
					Common.MarkOrderAsShipped(DB.RSFieldInt(rsp,"OrderNumber"),String.Empty,String.Empty,true);
					writer.Write("...<br>");
				}
				rsp.Close();
			}
				
			int OriginalRecurringOrderNumber = Common.QueryStringUSInt("OriginalRecurringOrderNumber");
      
			if(Common.QueryStringUSInt("ProcessCustomerID") != 0)
			{
				int ProcessCustomerID = Common.QueryStringUSInt("ProcessCustomerID");
				writer.Write("Processing Customer " + ProcessCustomerID.ToString() + "...");
				writer.Write(ProcessRecurringCustomer(thisCustomer,ProcessCustomerID,OriginalRecurringOrderNumber));
				writer.Write("...<br>");
			}


			writer.Write("<br>");

			writer.Write("<ul>");
      
			bool PendingOnly = (Common.QueryString("Show").ToUpper() != "ALL");
			
			if(PendingOnly)
			{
				if(DB.GetSqlN("Select count(*) as N from ShoppingCart  " + DB.GetNoLock() + " where CartType=" + ((int)CartTypeEnum.RecurringCart).ToString() + " and NextRecurringShipDate<" + DB.SQuote(Localization.ToNativeShortDateString(System.DateTime.Now.AddDays(1)))) > 0)
				{
					writer.Write("<li><b>Step 1. <a href=\"recurring.aspx?processall=true\">PROCESS CHARGES (RUN CREDIT CARDS AND CREATE ORDERS) FOR ALL SHIPMENTS DUE TODAY</a></b> (or process them one by one below)</li>");
				}
				else
				{
					writer.Write("<li><b>Step 1. NO RECURRING SHIPMENTS ARE DUE TODAY!</b></li>");
				}
				if(DB.GetSqlN("select count(*) as N from recurringshipments " + DB.GetNoLock()) > 0)
				{
					writer.Write("<li><b>Step 2. <a href=\"recurringshippinglist.aspx\" target=\"_blank\">GET SHIPPING LIST FOR PROCESSED CUSTOMERS</a></b></li>");
					writer.Write("<li><b>Step 3. <a onClick=\"if(confirm('ARE YOU SURE YOU WANT TO MARK ALL THE PROCESSED AUTO-SHIP ORDERS AS SHIPPED?\\n\\nMAKE SURE YOU HAVE SHIPPED ALL OF THE ORDERS IN THE LIST FIRST!')) {self.location='recurring.aspx?markallshipped=true';}\" href=\"#\">MARK ALL PROCESSED ORDERS AS SHIPPED</a></b></li>");
					writer.Write("<li><b>Step 4. <a onClick=\"if(confirm('ARE YOU SURE YOU WANT TO CLEAR THE PROCESSED AUTO-SHIP LIST? IT CANNOT BE RECOVERED.\\n\\nMAKE SURE YOU HAVE SHIPPED ALL OF THE ORDERS IN THE LIST, AND MARKED THEM AS SHIPPED FIRST!')) {self.location='recurring.aspx?clearshippinglist=true';}\" href=\"#\">CLEAR SHIPPING LIST FOR ALL PROCESSED CUSTOMERS</a></b></li>");
				}
				else
				{
					writer.Write("<li><b>Step 2. NO PROCESSED RECURRING SHIPMENTS ARE WAITING TO BE SHIPPED</li>");
				}
			}
			else
			{
				if(DB.GetSqlN("Select count(*) as N from ShoppingCart  " + DB.GetNoLock() + " where CartType=" + ((int)CartTypeEnum.RecurringCart).ToString()) == 0)
				{
					writer.Write("<li><b>NO ACTIVE RECURRING ORDERS FOUND</b></li>");
				}
			}
			writer.Write("</ul>");



			String CustomerList = ",";
			String sql = "Select CustomerID,nextrecurringshipdate from ShoppingCart  " + DB.GetNoLock() + " where CartType=" + ((int)CartTypeEnum.RecurringCart).ToString() + Common.IIF(PendingOnly, " and NextRecurringShipDate<" + DB.SQuote(Localization.ToNativeShortDateString(System.DateTime.Now.AddDays(1))), "") + " order by nextrecurringshipdate desc";
			IDataReader rs = DB.GetRS(sql);
			while(rs.Read())
			{
				if(CustomerList.IndexOf("," + DB.RSFieldInt(rs,"CustomerID").ToString() + ",") == -1)
				{
					Customer targetCustomer = new Customer(DB.RSFieldInt(rs,"CustomerID"),true);
					if(ShoppingCart.NumItems(targetCustomer._customerID,CartTypeEnum.RecurringCart) != 0)
					{
						IDataReader rsr = DB.GetRS("Select distinct OriginalRecurringOrderNumber from ShoppingCart  " + DB.GetNoLock() + " where CartType=" + ((int)CartTypeEnum.RecurringCart).ToString() + Common.IIF(PendingOnly, " and NextRecurringShipDate<" + DB.SQuote(Localization.ToNativeShortDateString(System.DateTime.Now.AddDays(1))), "") + " and CustomerID=" + targetCustomer._customerID.ToString() + " order by OriginalRecurringOrderNumber desc");
						while(rsr.Read())
						{
							writer.Write(Common.GetRecurringCart(targetCustomer,DB.RSFieldInt(rsr,"OriginalRecurringOrderNumber"),_siteID,false));
						}
						rsr.Close();
					}
				}
				CustomerList += DB.RSFieldInt(rs,"CustomerID").ToString() + ",";
			}
			rs.Close();
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
