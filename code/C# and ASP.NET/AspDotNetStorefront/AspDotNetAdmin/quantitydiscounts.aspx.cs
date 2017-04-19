// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for quantitydiscounts.
	/// </summary>
	public class quantitydiscounts : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Manage Quantity Discounts";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(Common.QueryString("DeleteID").Length != 0)
			{
				DB.ExecuteSQL("update category set QuantityDiscountID=0 where QuantityDiscountID=" + Common.QueryString("DeleteID"));
				DB.ExecuteSQL("update product set QuantityDiscountID=0 where QuantityDiscountID=" + Common.QueryString("DeleteID"));
				DB.ExecuteSQL("update productvariant set QuantityDiscountID=0 where QuantityDiscountID=" + Common.QueryString("DeleteID"));
				// delete the record:
				DB.ExecuteSQL("delete from quantitydiscounttable where quantitydiscountid=" + Common.QueryString("DeleteID"));
				DB.ExecuteSQL("delete from QuantityDiscount where QuantityDiscountID=" + Common.QueryString("DeleteID"));
				Common.ClearCache();
			}

			DataSet ds = DB.GetDS("select * from QuantityDiscount  " + DB.GetNoLock() + " order by name",false,System.DateTime.Now.AddDays(1));
			writer.Write("<form method=\"POST\" action=\"quantitydiscounts.aspx\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td width=\"5%\" align=\"center\"><b>ID</b></td>\n");
			writer.Write("      <td align=\"left\"><b>Name</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Edit</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Edit Table</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete</b></td>\n");
			writer.Write("    </tr>\n");
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
				writer.Write("      <td width=\"5%\"  align=\"center\">" + DB.RowFieldInt(row,"QuantityDiscountID").ToString() + "</td>\n");
				writer.Write("      <td align=\"left\"><a href=\"editQuantityDiscount.aspx?QuantityDiscountID=" + DB.RowFieldInt(row,"QuantityDiscountID").ToString() + "\">" + DB.RowField(row,"Name") + "</a></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Edit Name\" name=\"Edit_" + DB.RowFieldInt(row,"QuantityDiscountID").ToString() + "\" onClick=\"self.location='editquantitydiscount.aspx?QuantityDiscountID=" + DB.RowFieldInt(row,"QuantityDiscountID").ToString() + "'\"></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Edit Discount Table\" name=\"EditTable_" + DB.RowFieldInt(row,"QuantityDiscountID").ToString() + "\" onClick=\"self.location='editquantitydiscounttable.aspx?QuantityDiscountID=" + DB.RowFieldInt(row,"QuantityDiscountID").ToString() + "'\"></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Delete\" name=\"Delete_" + DB.RowFieldInt(row,"QuantityDiscountID").ToString() + "\" onClick=\"DeleteQuantityDiscount(" + DB.RowFieldInt(row,"QuantityDiscountID").ToString() + ")\"></td>\n");
				writer.Write("    </tr>\n");
			}
			ds.Dispose();
			writer.Write("</table>\n");
			writer.Write("<p align=\"left\"><input type=\"button\" value=\"Add New Quantity Discount Table\" name=\"AddNew\" onClick=\"self.location='editQuantityDiscount.aspx';\"></p>\n");
			writer.Write("</form>\n");

			writer.Write("</center></b>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteQuantityDiscount(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete this quantity discount table: ' + id))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'quantitydiscounts.aspx?deleteid=' + id;\n");
			writer.Write("}\n");
			writer.Write("}\n");
			writer.Write("</SCRIPT>\n");
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
