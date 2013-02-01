// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// $Header: /v5.0/AspDotNetStorefront/AspDotNetStorefrontAdmin/qbProduct.aspx.cs 17    6/25/05 2:22p Redwoodtree $
// ------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using AspDotNetStorefrontCommon;
using AspDotNetStorefrontQuickBooks;

namespace AspDotNetStorefrontAdmin
{

	/// <summary>
	/// Summary description for qbproduct
	/// </summary>
	public class QBProductPage : SkinBase
	{

		protected System.Web.UI.WebControls.Label lblCategoryPrompt;
		protected System.Web.UI.WebControls.Label lblRecordCount;
		protected System.Web.UI.WebControls.Button btnDownload;
		protected System.Web.UI.WebControls.Label lblSectionPrompt;
  
		private int CategoryID = -1;
		private int SectionID = -1;
		private int ProductTypeID = -1;
		private int ManufacturerID = -1;

		protected System.Web.UI.WebControls.DropDownList itemType;
		protected System.Web.UI.WebControls.TextBox txtExpenseAccount;
		protected System.Web.UI.WebControls.TextBox txtIncomeAccount;
		protected System.Web.UI.WebControls.TextBox txtPreferredVendor;
		protected System.Web.UI.WebControls.TextBox txtCOGSAccount;
		protected System.Web.UI.WebControls.TextBox txtAssetAccount;
		protected System.Web.UI.WebControls.Panel pnlExpenseAccount;
		protected System.Web.UI.WebControls.Panel pnlIncomeAccount;
		protected System.Web.UI.WebControls.Panel pnlPreferredVendor;
		protected System.Web.UI.WebControls.Panel pnlCOGSAccount;
		protected System.Web.UI.WebControls.Panel pnlAssetAccount;
		protected System.Web.UI.WebControls.RequiredFieldValidator reqExpense;
		protected System.Web.UI.WebControls.RequiredFieldValidator reqIncome;
		protected System.Web.UI.WebControls.RequiredFieldValidator reqCOGS;
		protected System.Web.UI.WebControls.RequiredFieldValidator reqAsset;
		protected System.Web.UI.WebControls.LinkButton btnResetFilters;
		protected System.Web.UI.WebControls.DropDownList categoryIDctl;
		protected System.Web.UI.WebControls.DropDownList sectionIDctl;
		protected System.Web.UI.WebControls.DropDownList manufacturerIDctl;
		protected System.Web.UI.WebControls.DropDownList productTypeIDctl;
		protected System.Web.UI.WebControls.CheckBox cbSaleAndPurchase;


		private void Page_Load(object sender, System.EventArgs e)
		{
			SectionTitle = "Quick Books Product Export";

			if (Page.IsPostBack)
			{
				CategoryID = Int32.Parse(categoryIDctl.Items[categoryIDctl.SelectedIndex].Value);
				SectionID = Int32.Parse(sectionIDctl.Items[sectionIDctl.SelectedIndex].Value);
				ProductTypeID = Int32.Parse(productTypeIDctl.Items[productTypeIDctl.SelectedIndex].Value);
				ManufacturerID = Int32.Parse(manufacturerIDctl.Items[manufacturerIDctl.SelectedIndex].Value);
			}

			if(CategoryID < 0)
			{
				CategoryID = Common.CookieUSInt("CategoryID");
			}
			if(SectionID < 0)
			{
				SectionID = Common.CookieUSInt("SectionID");
			}
			if(ProductTypeID < 0)
			{
				ProductTypeID = Common.CookieUSInt("ProductTypeID");
			}
			if(ManufacturerID < 0)
			{
				ManufacturerID = Common.CookieUSInt("ManufacturerID");
			}
      
			//Show all the fields
			this.pnlAssetAccount.Visible=true;
			this.pnlCOGSAccount.Visible=true;
			this.pnlExpenseAccount.Visible=true;
			this.pnlIncomeAccount.Visible=true;
			this.pnlPreferredVendor.Visible=true;

			//Then turn off the ones you don't want
			switch (itemType.SelectedValue)
			{
				case "Service" :
				{
					cbSaleAndPurchase.Visible = true;
					cbSaleAndPurchase.Text = "ServiceSaleAndPurchasePrompt";
					if (! cbSaleAndPurchase.Checked)
					{
						pnlPreferredVendor.Visible = false;
						this.pnlExpenseAccount.Visible=false;
					}
					pnlCOGSAccount.Visible = false;
					pnlAssetAccount.Visible = false;
					break;
				}
				case "Inventory" :
				{
					cbSaleAndPurchase.Visible = false;
					this.pnlExpenseAccount.Visible=false;
					break;
				}
				case "NonInventory" :
				{
					cbSaleAndPurchase.Visible = true;
					cbSaleAndPurchase.Text = "NonInventorySaleAndPurchasePrompt";
					if (! cbSaleAndPurchase.Checked)
					{
						pnlPreferredVendor.Visible = false;
						this.pnlExpenseAccount.Visible=false;
					}
					pnlCOGSAccount.Visible = false;
					pnlAssetAccount.Visible = false;
					break;
				}
			}

			if (!Page.IsPostBack)
			{
				string tmpS = "<root>" + Common.GetCategorySelectList(0,String.Empty,0) + "</root>";

				XmlDocument doc = new XmlDocument();
				doc.LoadXml(tmpS);
  
				categoryIDctl.Items.Add(new ListItem("All Categories","0"));
     
				XmlNodeList cNodes = doc.SelectNodes("//option");
				foreach (XmlNode node in cNodes)
				{
					int val = Int32.Parse(node.Attributes["value"].Value);
					ListItem item = new ListItem(node.InnerText,node.Attributes["value"].Value);
					categoryIDctl.Items.Add(item);
					if (val == CategoryID) categoryIDctl.SelectedIndex = categoryIDctl.Items.IndexOf(item);
				}

				tmpS = "<root>" + Common.GetSectionSelectList(0,String.Empty,0) + "</root>";

				doc.LoadXml(tmpS);
       
				sectionIDctl.Items.Add(new ListItem("All Departments","0"));
				cNodes = doc.SelectNodes("//option");
				foreach (XmlNode node in cNodes)
				{
					int val = Int32.Parse(node.Attributes["value"].Value);
					ListItem item = new ListItem(node.InnerText,node.Attributes["value"].Value);
					sectionIDctl.Items.Add(item);
					if (val == SectionID) sectionIDctl.SelectedIndex = sectionIDctl.Items.IndexOf(item);
				}

				manufacturerIDctl.Items.Add(new ListItem("All Manufacturers","0"));
				DataSet dsst = DB.GetDS("select * from manufacturer where deleted=0 order by displayorder,name",false,DateTime.Now.AddHours(1));
				foreach (DataRow row in dsst.Tables[0].Rows)
				{
					int val = DB.RowFieldInt(row,"ManufacturerID");
					ListItem item = new ListItem(DB.RowField(row,"Name"),val.ToString());
					manufacturerIDctl.Items.Add(item);
					if (val==ManufacturerID) manufacturerIDctl.SelectedIndex = manufacturerIDctl.Items.IndexOf(item);  
				}

				dsst.Dispose();

				productTypeIDctl.Items.Add(new ListItem("All Product Types","0"));
				dsst = DB.GetDS("select * from producttype where deleted=0 order by name",false,DateTime.Now.AddHours(1));
				foreach (DataRow row in dsst.Tables[0].Rows)
				{
					int val = DB.RowFieldInt(row,"ProductTypeID");
					ListItem item = new ListItem(DB.RowField(row,"Name"),val.ToString());
					productTypeIDctl.Items.Add(item);
					if (val==ProductTypeID) productTypeIDctl.SelectedIndex = productTypeIDctl.Items.IndexOf(item);  
				}

				dsst.Dispose();
				MakeSearch().Dispose();

			}
		}

		public DataSet MakeSearch()
		{
			string LocaleSetting = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
			String sql = String.Empty;
			if(CategoryID !=0 && SectionID != 0)
			{
				// section has priority over category for DO
				sql = "select p.*,do.DisplayOrder" 
					+ " from Product P " + DB.GetNoLock() 
					+ " left outer join sectiondisplayorder DO  " + DB.GetNoLock() + " on p.productid=DO.productid"
					+ " where deleted=0"
					+ " and do.sectionID=" + SectionID.ToString() + " " + Common.IIF(SectionID != 0 , " and p.ProductID in (select distinct productid from productsection  " + DB.GetNoLock() + " where SectionID=" + SectionID.ToString() + ")" , "") 
					+ Common.IIF(CategoryID != 0 , " and p.ProductID in (select distinct productid from productcategory  " + DB.GetNoLock() + " where categoryid=" + CategoryID.ToString() + ")" , "") 
					+ Common.IIF(ProductTypeID != 0 , " and ProductTypeID=" + ProductTypeID.ToString() , "") 
					+ Common.IIF(ManufacturerID != 0 , " and ManufacturerID=" + ManufacturerID.ToString() , "") 
					+ " order by do.displayorder";
			}
			if(CategoryID != 0 && SectionID == 0)
			{
				sql = "select p.*,do.DisplayOrder"
					+ " from Product P  " + DB.GetNoLock() 
					+ " left outer join categorydisplayorder DO  " + DB.GetNoLock() 
					+ " on p.productid=DO.productid"
					+ " where deleted=0"
					+ " and do.categoryID=" + CategoryID.ToString() + " " 
					+ Common.IIF(CategoryID != 0 , " and p.ProductID in (select distinct productid from productcategory  " + DB.GetNoLock() + " where categoryid=" + CategoryID.ToString() + ")" , "") 
					+ Common.IIF(ProductTypeID != 0 , " and ProductTypeID=" + ProductTypeID.ToString() , "") 
					+ Common.IIF(ManufacturerID != 0 , " and ManufacturerID=" + ManufacturerID.ToString() , "") 
					+ " order by do.displayorder";
			}
			if(CategoryID == 0 && SectionID != 0)
			{
				sql = "select p.*,do.DisplayOrder"
					+ " from Product P  " + DB.GetNoLock() 
					+ " left outer join sectiondisplayorder DO  " + DB.GetNoLock() 
					+ " on p.productid=DO.productid"
					+ " where deleted=0" 
					+ " and do.sectionID=" + SectionID.ToString() + " " 
					+ Common.IIF(SectionID != 0 , " and p.ProductID in (select distinct productid from productsection " + DB.GetNoLock() + " where SectionID=" + SectionID.ToString() + ")" , "") 
					+ Common.IIF(ProductTypeID != 0 , " and ProductTypeID=" + ProductTypeID.ToString() , "") 
					+ Common.IIF(ManufacturerID != 0 , " and ManufacturerID=" + ManufacturerID.ToString() , "") 
					+ " order by do.displayorder";
			}
			if(CategoryID == 0 && SectionID == 0)
			{
				sql = "select p.*" 
					+ " from Product P  " + DB.GetNoLock() 
					+ " where deleted=0 " 
					+ Common.IIF(ProductTypeID != 0 , " and ProductTypeID=" + ProductTypeID.ToString() , "") 
					+ Common.IIF(ManufacturerID != 0 , " and ManufacturerID=" + ManufacturerID.ToString() , "");
			}

			string sectionQuery = (SectionID != 0) ? String.Format(" and Product.ProductID in (select distinct productid from productsection where SectionID={0} )",SectionID):String.Empty;
			string categoryQuery = (CategoryID != 0) ? String.Format(" and Product.ProductID in (select distinct productid from productcategory where CategoryID={0} )",CategoryID):String.Empty;
			string productTypeQuery = (ProductTypeID != 0) ? String.Format(" and Product.ProductTypeID={0}",ProductTypeID):String.Empty;
			string manufacturerQuery = (ManufacturerID != 0) ? String.Format(" and Product.ManufacturerID={0}",ManufacturerID):String.Empty;

			sql = "select ProductVariant.VariantID, SKU, SKUSuffix,Colors,ColorSKUModifiers,Sizes,SizeSKUModifiers,"
				+ " FullSKU= case when SKUSuffix is null then SKU else SKU+SKUSuffix end, "
				+ " price,saleprice,"
				+ " thePrice = case when saleprice is null then price else saleprice end,"
				+ " cost,Product.ProductID, Product.Name, ProductVariant.name as VariantName"
				+ " from Product inner join ProductVariant on Product.ProductID = ProductVariant.ProductID"
				+ String.Format(" where Product.deleted=0 and ProductVariant.deleted=0 {0} {1} {2} {3}",sectionQuery,categoryQuery,productTypeQuery,manufacturerQuery); 
        

			Common.SetCookie("CategoryID",CategoryID.ToString(),new TimeSpan(365,0,0,0,0));
			Common.SetCookie("SectionID",SectionID.ToString(),new TimeSpan(365,0,0,0,0));
			Common.SetCookie("ProductTypeID",ProductTypeID.ToString(),new TimeSpan(365,0,0,0,0));
			Common.SetCookie("ManufacturerID",ManufacturerID.ToString(),new TimeSpan(365,0,0,0,0));

			DataSet ds = DB.GetDS(sql,false);
			lblRecordCount.Text = ds.Tables[0].Rows.Count.ToString();
			return ds;
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
			this.btnResetFilters.Click += new System.EventHandler(this.btnResetFilters_Click);
			this.categoryIDctl.SelectedIndexChanged += new System.EventHandler(this.IndexChanged);
			this.sectionIDctl.SelectedIndexChanged += new System.EventHandler(this.IndexChanged);
			this.manufacturerIDctl.SelectedIndexChanged += new System.EventHandler(this.IndexChanged);
			this.productTypeIDctl.SelectedIndexChanged += new System.EventHandler(this.IndexChanged);
			this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnDownload_Click(object sender, System.EventArgs e)
		{
			QBProduct qbp = new QBProduct();

			qbp.ItemType = (QBItemTypes)Enum.Parse(typeof(QBItemTypes),itemType.SelectedValue,true);
			qbp.AssetAccount = txtAssetAccount.Text;
			qbp.COGSAccount = txtCOGSAccount.Text;
			qbp.ExpenseAccount = txtExpenseAccount.Text;
			qbp.IncomeAccount = txtIncomeAccount.Text;
			qbp.PreferredVendor = txtPreferredVendor.Text;
			qbp.SaleAndPurchase = (cbSaleAndPurchase.Checked);

			DataSet ds = MakeSearch();
			string qbxml = qbp.CreateXml(ds);
      
			ds.Dispose(); 
      
			//Send the XML
			Response.AddHeader("content-disposition","attachment; filename=" + HttpUtility.UrlEncode("QBXML_Product_"+DateTime.Now.ToString("yyyyMMddhhmmss")+".qbxml"));
			Response.ContentType = "text/qbxml";
			Response.Write(qbxml);
			Response.Flush();
			Response.End();
		}

		private void IndexChanged(object sender, System.EventArgs e)
		{
			MakeSearch().Dispose();
		}

		private void btnResetFilters_Click(object sender, System.EventArgs e)
		{
			CategoryID = 0;
			categoryIDctl.SelectedIndex = 0;
			SectionID = 0;
			sectionIDctl.SelectedIndex = 0;
			ProductTypeID = 0;
			productTypeIDctl.SelectedIndex = 0;
			ManufacturerID = 0;
			manufacturerIDctl.SelectedIndex = 0;
			MakeSearch().Dispose();
		}

	}
}
