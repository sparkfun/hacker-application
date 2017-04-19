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
	/// Summary description for ShippingZones.
	/// </summary>
	public class ShippingZones : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Manage Shipping Zones";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(Common.QueryString("DeleteID").Length != 0)
			{
				// delete the Zone:
				DB.ExecuteSQL("delete from ShippingByZone where ShippingZoneID=" + Common.QueryString("DeleteID"));
				DB.ExecuteSQL("delete from ShippingZone where ShippingZoneID=" + Common.QueryString("DeleteID"));
				Common.ClearCache();
			}

			DataSet ds = DB.GetDS("select * from ShippingZone  " + DB.GetNoLock() + " where deleted=0 order by name",false,System.DateTime.Now.AddDays(1));
			writer.Write("<form Method=\"POST\" action=\"ShippingZones.aspx\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td width=\"5%\" align=\"center\"><b>ID</b></td>\n");
			writer.Write("      <td align=\"left\"><b>Zone</b></td>\n");
			writer.Write("      <td align=\"left\" width=\"50%\" align=\"left\"><b>ZipCodes</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Edit</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete</b></td>\n");
			writer.Write("    </tr>\n");
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
				writer.Write("      <td width=\"5%\"  align=\"center\">" + DB.RowFieldInt(row,"ShippingZoneID").ToString() + "</td>\n");
				writer.Write("      <td align=\"left\"><a href=\"editShippingZone.aspx?ShippingZoneID=" + DB.RowFieldInt(row,"ShippingZoneID").ToString() + "\">" + DB.RowField(row,"Name") + "</a></td>\n");
				writer.Write("      <td align=\"left\" width=\"50%\"  align=\"center\">" + DB.RowField(row,"ZipCodes") + "</td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Edit\" name=\"Edit_" + DB.RowFieldInt(row,"ShippingZoneID").ToString() + "\" onClick=\"self.location='editShippingZone.aspx?ShippingZoneID=" + DB.RowFieldInt(row,"ShippingZoneID").ToString() + "'\"></td>\n");
				writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Delete\" name=\"Delete_" + DB.RowFieldInt(row,"ShippingZoneID").ToString() + "\" onClick=\"DeleteShippingZone(" + DB.RowFieldInt(row,"ShippingZoneID").ToString() + ")\"></td>\n");
				writer.Write("    </tr>\n");
			}
			ds.Dispose();
			writer.Write("</table>\n");
			writer.Write("<input type=\"button\" value=\"Add New Shipping Zone\" name=\"AddNew\" onClick=\"self.location='editShippingZone.aspx';\">\n");
			writer.Write("</form>\n");

			writer.Write("</center></b>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteShippingZone(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete Shipping Zone: ' + id))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'ShippingZones.aspx?deleteid=' + id;\n");
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
