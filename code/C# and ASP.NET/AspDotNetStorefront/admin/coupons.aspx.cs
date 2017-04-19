// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for coupons.
	/// </summary>
	public class coupons : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			if(Common.QueryString("DeleteID").Length != 0)
			{
				DB.ExecuteSQL("Update customer set CouponCode=NULL where lower(CouponCode)=(select lower(couponcode) from coupon  " + DB.GetNoLock() + " where couponid=" + Common.QueryString("DeleteID") + ")");
				DB.ExecuteSQL("update Coupon set deleted=1, LastUpdatedBy=" + thisCustomer._customerID.ToString() + " where CouponID=" + Common.QueryString("DeleteID"));
			}
			SectionTitle = "Manage Coupons";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			DataSet ds = DB.GetDS("select * from Coupon  " + DB.GetNoLock() + " where deleted=0 order by couponcode",false,System.DateTime.Now.AddDays(1));
			writer.Write("<form method=\"POST\" action=\"coupons.aspx\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td><b>ID</b></td>\n");
			writer.Write("      <td><b>Coupon Code</b></td>\n");
			writer.Write("      <td><b>Description</b></td>\n");
			writer.Write("      <td><b>Expires On</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Edit</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete</b></td>\n");
			writer.Write("    </tr>\n");
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
				writer.Write("      <td>" + DB.RowFieldInt(row,"CouponID").ToString() + "</td>\n");
				writer.Write("      <td><a href=\"editCoupon.aspx?CouponID=" + DB.RowFieldInt(row,"CouponID").ToString() + "\">" + DB.RowField(row,"CouponCode") + "</a></td>\n");
				writer.Write("      <td>" + DB.RowField(row,"Description") + "</td>\n");
				writer.Write("      <td>" + Localization.ToNativeDateTimeString(DB.RowFieldDateTime(row,"ExpirationDate")) + "</td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Edit\" name=\"Edit_" + DB.RowFieldInt(row,"CouponID").ToString() + "\" onClick=\"self.location='editCoupon.aspx?CouponID=" + DB.RowFieldInt(row,"CouponID").ToString() + "'\"></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Delete\" name=\"Delete_" + DB.RowFieldInt(row,"CouponID").ToString() + "\" onClick=\"DeleteCoupon(" + DB.RowFieldInt(row,"CouponID").ToString() + ")\"></td>\n");
				writer.Write("    </tr>\n");
			}
			ds.Dispose();
			writer.Write("    <tr>\n");
			writer.Write("      <td align=\"left\"><input type=\"button\" value=\"Add New Coupon\" name=\"AddNew\" onClick=\"self.location='editCoupon.aspx';\"></td>\n");
			writer.Write("      <td colspan=\"6\"></td>\n");
			writer.Write("    </tr>\n");
			writer.Write("  </table>\n");
			writer.Write("</form>\n");

			writer.Write("</center></b>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteCoupon(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete Coupon: ' + id))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'Coupons.aspx?deleteid=' + id;\n");
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
