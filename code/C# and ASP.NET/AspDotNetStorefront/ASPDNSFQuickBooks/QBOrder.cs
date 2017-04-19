using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Xml;
using Interop.QBFC4;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontQuickBooks
{
	/// <summary>
	/// Summary description for QBOrder.
	/// </summary>
	public class QBOrder
	{
		public QBOrder() {}

		/// <summary>
		/// Creates a singles sales receipt with the aggregate of all items in the supplied orders.
		/// </summary>
		public string CreateReceiptXml(string whereClause)
		{
			return null;
		}

		/// <summary>
		/// Creates a singles sales receipt with the aggregate of all items in the supplied orders.
		/// </summary>
		public string CreateReceiptXml(DataSet ds)
		{
			double ShippingTotal = 0.0D;
			double TaxTotal = 0.0D;

			QBSessionManagerClass qbSession = new QBSessionManagerClass();

			short qbVersion = (short)Common.AppConfigNativeInt("QuickBooks.Version");
			IMsgSetRequest qbRequest = qbSession.CreateMsgSetRequest("US",qbVersion,0);  

			qbRequest.Attributes.OnError = ENRqOnError.roeContinue;
      
			string orderSet = String.Empty;
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				if (orderSet.Length > 0)
				{
					orderSet += ",";
				}
				orderSet += DB.RowFieldInt(row,"OrderNumber").ToString();
        
				ShippingTotal += DB.RowFieldDouble(row,"OrderShippingCost");
        
				TaxTotal += DB.RowFieldDouble(row,"OrderTax");
			}


			string sql = String.Format("select *,(OrderedProductPrice/Quantity) as ItemPrice from orders_shoppingcart where OrderNumber in ({0}) order by OrderedProductSKU,ItemPrice",orderSet);
			IDataReader rs = DB.GetRS(sql);

			ISalesReceiptAdd qbReceipt = qbRequest.AppendSalesReceiptAddRq();
			IORSalesReceiptLineAdd qbLineItem = null;

			if (qbVersion > 3)
			{
				IShippingLineAdd qbLineShipping = qbReceipt.ShippingLineAdd;
				qbLineShipping.Amount.SetValue(ShippingTotal); 

				ISalesTaxLineAdd qbSalesTax = qbReceipt.SalesTaxLineAdd;
				qbSalesTax.ORSalesTaxLineAdd.Amount.SetValue(TaxTotal);
			}

			string lastSKU = String.Empty;
			string currentSKU = String.Empty;
			string description = String.Empty;

			double currentQty = 0.0D;  
      
			double lineQty = 0.0D;
			double lineTotal = 0.0D;

			double lastPrice = 0.0D;
			double currentPrice = 0.0D;
 
			while (rs.Read())
			{
				currentSKU = DB.RSField(rs,"OrderedProductSKU");
				currentPrice = (double)DB.RSFieldDecimal(rs,"ItemPrice");
				currentQty = (double)DB.RSFieldInt(rs,"Quantity");
				//Detect change in line item
				if ((lastSKU != currentSKU) || (lastPrice != currentPrice))
				{
					qbLineItem = qbReceipt.ORSalesReceiptLineAddList.Append();
					lastSKU = currentSKU;
					lastPrice = currentPrice;
					lineQty = 0.0D;
					lineTotal = 0.0D;
					description = DB.RSField(rs,"OrderedProductName");
					if(DB.RSField(rs,"OrderedProductVariantName").Length != 0)
					{
						description += " - " + DB.RSField(rs,"OrderedProductVariantName");
					}
				}
				lineQty += currentQty;
				lineTotal += (double)DB.RSFieldDecimal(rs,"OrderedProductPrice"); //Extended Price

				qbLineItem.SalesReceiptLineAdd.ItemRef.FullName.SetValue(currentSKU);
				qbLineItem.SalesReceiptLineAdd.Desc.SetValue(description);
				qbLineItem.SalesReceiptLineAdd.Quantity.SetValue(lineQty);
				qbLineItem.SalesReceiptLineAdd.Amount.SetValue(lineTotal);
			}
			rs.Close();
			string result = qbRequest.ToXMLString();
			qbRequest = null;
			return result;
		}
	}
}
