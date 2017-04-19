using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Resources;
using System.Xml;
using Interop.QBFC4;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontQuickBooks
{
	public enum QBItemTypes : int
	{
		Nothing = 0,
		Service = 1,
		Inventory = 2,
		NonInventory = 3,
		OtherCharge = 4
	}

	/// <summary>
	/// Summary description for QBProduct.
	/// </summary>
	public class QBProduct
	{
		private QBItemTypes m_ItemType = QBItemTypes.Nothing;
    
		private string m_PreferredVendor = String.Empty;
		private string m_assetAccount = String.Empty;
		private string m_CogsAccount = String.Empty;
		private string m_expenseAccount = String.Empty;
		private string m_IncomeAccount = String.Empty;

		private bool m_SaleAndPurchase = false;

     
    
		public QBProduct() {}

		#region Properties

		/// <summary>
		/// Asset Account to use for Inventory products
		/// </summary>
		public string AssetAccount
		{
			get
			{
				return m_assetAccount;
			}
			set
			{
				m_assetAccount = value.Trim();
			}
		}

		/// <summary>
		/// Cost of Goods Sold Account to use for Inventory Products
		/// </summary>
		public string COGSAccount
		{
			get
			{
				return m_CogsAccount;
			}
			set
			{
				m_CogsAccount = value.Trim();
			}
		}

		/// <summary>
		/// Expense Account for SaleAndPurchase products
		/// </summary>
		public string ExpenseAccount
		{
			get
			{
				return m_expenseAccount;
			}
			set
			{
				m_expenseAccount = value.Trim();
			}
		}
    
		/// <summary>
		/// Income Account for SaleAndPurchase products
		/// </summary>
		public string IncomeAccount
		{
			get
			{
				return m_IncomeAccount;
			}
			set
			{
				m_IncomeAccount = value.Trim();
			}
		}
		/// <summary>
		/// PreferredVendor for SaleAndPurchase products
		/// </summary>
		public string PreferredVendor
		{
			get
			{
				return m_PreferredVendor;
			}
			set
			{
				m_PreferredVendor = value.Trim();
			}
		}

		/// <summary>
		/// Whether the item is handles by a subcontractor
		/// </summary>
		public bool SaleAndPurchase
		{
			get
			{
				return m_SaleAndPurchase;
			}
			set
			{
				m_SaleAndPurchase = value;
			}
		}

		/// <summary>
		/// QB Item Type for this conversion
		/// </summary>
		public QBItemTypes ItemType
		{
			get
			{
				return m_ItemType;       
			}
			set
			{
				m_ItemType = value;
			}
		}
		#endregion


		/// <summary>
		/// Creates the QB xml based on the product dataset supplied.
		/// </summary>
		public string CreateXml(DataSet data)
		{
			QBSessionManagerClass qbSession = new QBSessionManagerClass();
			short qbVersion = (short)Common.AppConfigNativeInt("QuickBooks.Version");

			IMsgSetRequest qbRequest = qbSession.CreateMsgSetRequest("US",qbVersion,0);  
			qbRequest.Attributes.OnError = ENRqOnError.roeContinue;

      
			foreach(DataRow row in data.Tables[0].Rows)
			{
				string desc = String.Format("{0} {1}",DB.RowField(row,"Name"),DB.RowField(row,"VariantName"));
				double price = double.Parse(DB.RowFieldDouble(row,"thePrice").ToString("#0.00"));
				double cost = double.Parse(DB.RowFieldDouble(row,"cost").ToString("#0.00"));

				string SKU = DB.RowField(row,"FullSKU").Trim();
        
				string Taxable = "Tax";
				if (DB.RowFieldInt(row,"IsTaxable") == 1)
				{
					Taxable="Tax";
				}
				else
				{
					Taxable="Non";
				}

				string SubItem = String.Empty;
				int divider = SKU.IndexOf(":");
				if (divider > -1)
				{
					SubItem = SKU.Substring(0,divider-1);
					SKU = SKU.Substring(divider+1);
				}


				if (cost == 0.0)
				{
					cost  = (price * 0.64);
					cost = double.Parse(cost.ToString("#0.00"));
				}

				string[] ColorsSplit = DB.RowField(row,"Colors").Split(',');
				string[] ColorSKUSplit = DB.RowField(row,"ColorSKUModifiers").Split(',');

				string[] SizesSplit = DB.RowField(row,"Sizes").Split(',');
				string[] SizeSKUSplit = DB.RowField(row,"SizeSKUModifiers").Split(',');

        
        

				for(int cNdx = 0;cNdx < ColorSKUSplit.Length;cNdx++)
				{
					for(int sNdx = 0; sNdx < SizeSKUSplit.Length; sNdx++)
					{
						double delta = (double)Common.GetColorAndSizePriceDelta(ColorsSplit[cNdx],SizesSplit[sNdx]);

						switch (ItemType)
						{
							case QBItemTypes.Service :
							{
								//Add
								IItemServiceAdd Service = qbRequest.AppendItemServiceAddRq();
								Service.Name.SetValue(String.Format("{0}{1}{2}",SKU,ColorSKUSplit[cNdx],SizeSKUSplit[sNdx]));
								Service.SalesTaxCodeRef.FullName.SetValue(Taxable);

								if (SubItem.Length !=0) Service.ParentRef.FullName.SetValue(SubItem);
								if (SaleAndPurchase)
								{
									Service.ORSalesPurchase.SalesAndPurchase.PurchaseDesc.SetValue(desc.Trim());
									Service.ORSalesPurchase.SalesAndPurchase.SalesDesc.SetValue(desc.Trim());
									Service.ORSalesPurchase.SalesAndPurchase.SalesPrice.SetValue(price+delta);
									if (PreferredVendor.Length !=0) Service.ORSalesPurchase.SalesAndPurchase.PrefVendorRef.FullName.SetValue(PreferredVendor);
									Service.ORSalesPurchase.SalesAndPurchase.ExpenseAccountRef.FullName.SetValue(ExpenseAccount);
									Service.ORSalesPurchase.SalesAndPurchase.IncomeAccountRef.FullName.SetValue(IncomeAccount);
									Service.ORSalesPurchase.SalesAndPurchase.PurchaseCost.SetValue(cost);
								}
								else
								{
									Service.ORSalesPurchase.SalesOrPurchase.Desc.SetValue(desc.Trim());
									Service.ORSalesPurchase.SalesOrPurchase.ORPrice.Price.SetValue(price+delta);
									Service.ORSalesPurchase.SalesOrPurchase.AccountRef.FullName.SetValue(IncomeAccount);
								}
								break;
							}
							case QBItemTypes.Inventory :
							{
								//Add
								IItemInventoryAdd Inventory = qbRequest.AppendItemInventoryAddRq();

								Inventory.Name.SetValue(String.Format("{0}{1}{2}",SKU,ColorSKUSplit[cNdx],SizeSKUSplit[sNdx]));
								Inventory.SalesTaxCodeRef.FullName.SetValue(Taxable);

								if (SubItem.Length !=0) Inventory.ParentRef.FullName.SetValue(SubItem);
								Inventory.PurchaseDesc.SetValue(desc.Trim());
								Inventory.SalesDesc.SetValue(desc.Trim());
								Inventory.SalesPrice.SetValue(price+delta);
								Inventory.PurchaseCost.SetValue(cost);
								Inventory.PurchaseDesc.SetValue(desc.Trim());
								if (PreferredVendor.Length !=0) Inventory.PrefVendorRef.FullName.SetValue(PreferredVendor);
								Inventory.AssetAccountRef.FullName.SetValue(AssetAccount);
								Inventory.IncomeAccountRef.FullName.SetValue(IncomeAccount);
								Inventory.COGSAccountRef.FullName.SetValue(COGSAccount);
								break;
							}
							case QBItemTypes.NonInventory :
							{
								//Add
								IItemNonInventoryAdd NonInventory = qbRequest.AppendItemNonInventoryAddRq();
								NonInventory.Name.SetValue(String.Format("{0}{1}{2}",SKU,ColorSKUSplit[cNdx],SizeSKUSplit[sNdx]));
								NonInventory.SalesTaxCodeRef.FullName.SetValue(Taxable);

								if (SubItem.Length !=0) NonInventory.ParentRef.FullName.SetValue(SubItem);
								if (SaleAndPurchase)
								{
									NonInventory.ORSalesPurchase.SalesAndPurchase.PurchaseDesc.SetValue(desc.Trim());
									NonInventory.ORSalesPurchase.SalesAndPurchase.SalesDesc.SetValue(desc.Trim());
									NonInventory.ORSalesPurchase.SalesAndPurchase.SalesPrice.SetValue(price+delta);
									if (PreferredVendor.Length !=0) NonInventory.ORSalesPurchase.SalesAndPurchase.PrefVendorRef.FullName.SetValue(PreferredVendor);
									NonInventory.ORSalesPurchase.SalesAndPurchase.ExpenseAccountRef.FullName.SetValue(ExpenseAccount);
									NonInventory.ORSalesPurchase.SalesAndPurchase.IncomeAccountRef.FullName.SetValue(IncomeAccount);
									NonInventory.ORSalesPurchase.SalesAndPurchase.PurchaseCost.SetValue(cost);
								}
								else
								{
									NonInventory.ORSalesPurchase.SalesOrPurchase.Desc.SetValue(desc.Trim());
									NonInventory.ORSalesPurchase.SalesOrPurchase.ORPrice.Price.SetValue(price+delta);
									NonInventory.ORSalesPurchase.SalesOrPurchase.AccountRef.FullName.SetValue(IncomeAccount);
								}
								break;
							}
						}
					}
				}
			}
      
			//Do the Mods 
			foreach(DataRow row in data.Tables[0].Rows)
			{
				string desc = String.Format("{0} {1}",DB.RowField(row,"Name"),DB.RowField(row,"VariantName"));
				double price = double.Parse(DB.RowFieldDouble(row,"thePrice").ToString("#0.00"));
				double cost = double.Parse(DB.RowFieldDouble(row,"cost").ToString("#0.00"));
        
				string SKU = DB.RowField(row,"FullSKU").Trim();
        
				string Taxable = "Tax";
				if (DB.RowFieldInt(row,"IsTaxable") == 1)
				{
					Taxable="Tax";
				}
				else
				{
					Taxable="Non";
				}

				string SubItem = String.Empty;
				int divider = SKU.IndexOf(":");
				if (divider > -1)
				{
					SubItem = SKU.Substring(0,divider-1);
					SKU = SKU.Substring(divider+1);
				}

				if (cost == 0.0)
				{
					cost  = (price * 0.64);
					cost = double.Parse(cost.ToString("#0.00"));
				}
				string[] ColorsSplit = DB.RowField(row,"Colors").Split(',');
				string[] ColorSKUSplit = DB.RowField(row,"ColorSKUModifiers").Split(',');

				string[] SizesSplit = DB.RowField(row,"Sizes").Split(',');
				string[] SizeSKUSplit = DB.RowField(row,"SizeSKUModifiers").Split(',');

				for(int cNdx = 0;cNdx < ColorSKUSplit.Length;cNdx++)
				{
					for(int sNdx = 0; sNdx < SizeSKUSplit.Length; sNdx++)
					{
						double delta = (double)Common.GetColorAndSizePriceDelta(ColorsSplit[cNdx],SizesSplit[sNdx]);

						switch (ItemType)
						{
							case QBItemTypes.Service :
							{
								//Mod
								IItemServiceMod ServiceMod = qbRequest.AppendItemServiceModRq();
								ServiceMod.ListID.SetValue("");
								ServiceMod.EditSequence.SetValue("");
								ServiceMod.Name.SetValue(String.Format("{0}{1}{2}",SKU,ColorSKUSplit[cNdx],SizeSKUSplit[sNdx]));
								ServiceMod.SalesTaxCodeRef.FullName.SetValue(Taxable);
								if (SubItem.Length !=0) ServiceMod.ParentRef.FullName.SetValue(SubItem);

								if (SaleAndPurchase)
								{
									ServiceMod.ORSalesPurchaseMod.SalesAndPurchaseMod.PurchaseDesc.SetValue(desc.Trim());
									ServiceMod.ORSalesPurchaseMod.SalesAndPurchaseMod.SalesDesc.SetValue(desc.Trim());
									ServiceMod.ORSalesPurchaseMod.SalesAndPurchaseMod.SalesPrice.SetValue(price+delta);
									if (PreferredVendor.Length !=0) ServiceMod.ORSalesPurchaseMod.SalesAndPurchaseMod.PrefVendorRef.FullName.SetValue(PreferredVendor);
									ServiceMod.ORSalesPurchaseMod.SalesAndPurchaseMod.PurchaseCost.SetValue(cost);
								}
								else
								{
									ServiceMod.ORSalesPurchaseMod.SalesOrPurchaseMod.Desc.SetValue(desc.Trim());
									ServiceMod.ORSalesPurchaseMod.SalesOrPurchaseMod.ORPrice.Price.SetValue(price+delta);
								}
          
								break;
							}
							case QBItemTypes.Inventory :
							{
								//Mod
								IItemInventoryMod InventoryMod = qbRequest.AppendItemInventoryModRq();
								InventoryMod.ListID.SetValue("");
								InventoryMod.EditSequence.SetValue("");
								InventoryMod.Name.SetValue(String.Format("{0}{1}{2}",SKU,ColorSKUSplit[cNdx],SizeSKUSplit[sNdx]));
								InventoryMod.SalesTaxCodeRef.FullName.SetValue(Taxable);

								if (SubItem.Length !=0) InventoryMod.ParentRef.FullName.SetValue(SubItem);

								InventoryMod.SalesDesc.SetValue(desc.Trim());
								InventoryMod.SalesPrice.SetValue(price+delta);
								InventoryMod.PurchaseDesc.SetValue(desc.Trim());
								InventoryMod.PurchaseCost.SetValue(cost);
								if (PreferredVendor.Length !=0) InventoryMod.PrefVendorRef.FullName.SetValue(PreferredVendor);
								break;
							}
							case QBItemTypes.NonInventory :
							{
								//Mod
								IItemNonInventoryMod NonInventoryMod = qbRequest.AppendItemNonInventoryModRq();
								NonInventoryMod.ListID.SetValue("");
								NonInventoryMod.EditSequence.SetValue("");
								NonInventoryMod.Name.SetValue(String.Format("{0}{1}{2}",SKU,ColorSKUSplit[cNdx],SizeSKUSplit[sNdx]));
								NonInventoryMod.SalesTaxCodeRef.FullName.SetValue(Taxable);

								if (SubItem.Length !=0) NonInventoryMod.ParentRef.FullName.SetValue(SubItem);

								if (SaleAndPurchase)
								{
									NonInventoryMod.ORSalesPurchaseMod.SalesAndPurchaseMod.PurchaseDesc.SetValue(desc.Trim());
									NonInventoryMod.ORSalesPurchaseMod.SalesAndPurchaseMod.SalesDesc.SetValue(desc.Trim());
									NonInventoryMod.ORSalesPurchaseMod.SalesAndPurchaseMod.SalesPrice.SetValue(price+delta);
									if (PreferredVendor.Length !=0) NonInventoryMod.ORSalesPurchaseMod.SalesAndPurchaseMod.PrefVendorRef.FullName.SetValue(PreferredVendor);
									NonInventoryMod.ORSalesPurchaseMod.SalesAndPurchaseMod.PurchaseCost.SetValue(cost);
								}
								else
								{
									NonInventoryMod.ORSalesPurchaseMod.SalesOrPurchaseMod.Desc.SetValue(desc.Trim());
									NonInventoryMod.ORSalesPurchaseMod.SalesOrPurchaseMod.ORPrice.Price.SetValue(price+delta);
								}
								break;
							}
						}
					}
				}
			}
			return qbRequest.ToXMLString();
		}


	}
}
