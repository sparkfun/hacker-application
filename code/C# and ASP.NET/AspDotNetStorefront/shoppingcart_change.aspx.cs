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
using System.Data.SqlTypes;
using System.Data.SqlClient;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for ShoppingCart_change.
	/// </summary>
	public class ShoppingCart_change : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");

			Customer thisCustomer = new Customer();
			thisCustomer.RequireCustomerRecord();

			int ShoppingCartRecID = Common.QueryStringUSInt("RecID");
			if(ShoppingCartRecID == 0)
			{
				Response.Redirect("ShoppingCart.aspx");
			}
			// move this item back to the customcart (only one of the following two updates will actually do anything) depending on if the product is a pack or a kit:
			DB.ExecuteSQL("update kitcart set ShoppingCartRecID=0 where ShoppingCartRecID=" + ShoppingCartRecID.ToString() + " and CustomerID=" + thisCustomer._customerID.ToString());
			DB.ExecuteSQL("update customcart set ShoppingCartRecID=0 where ShoppingCartRecID=" + ShoppingCartRecID.ToString() + " and CustomerID=" + thisCustomer._customerID.ToString());

			IDataReader rs = DB.GetRS("select * from ShoppingCart where ShoppingCartrecid=" + ShoppingCartRecID.ToString());
			rs.Read();
			int ProductID = DB.RSFieldInt(rs,"ProductID");
			rs.Close();
			DB.ExecuteSQL("delete from ShoppingCart where ShoppingCartrecid=" + ShoppingCartRecID.ToString());
			Response.Redirect("showproduct.aspx?productid=" + ProductID.ToString());
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
