// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
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
	/// Summary description for movevariant
	/// </summary>
	public class movevariant : SkinBase
	{
		
		int ProductID;
		int VariantID;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			ProductID = Common.QueryStringUSInt("ProductID");
			VariantID = Common.QueryStringUSInt("VariantID");

			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				DB.ExecuteSQL("Update productvariant set ProductID=" + Common.Form("NewProductID") + " where VariantID=" + VariantID.ToString());
				Response.Redirect("variants.aspx?productid=" + ProductID.ToString());
			}
			SectionTitle = "<a href=\"variants.aspx?productid=" + ProductID.ToString() + "\">Variants</a> - Move Variant";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			IDataReader rs = DB.GetRS("select * from productvariant  " + DB.GetNoLock() + " where VariantID=" + VariantID.ToString());
			rs.Read();
			writer.Write("<b>Within Product: <a href=\"editproduct.aspx?productid=" + ProductID.ToString() + "\">" + Common.GetProductName(ProductID) + "</a> (SKU=" + Common.GetProductSKU(ProductID) + ", ID=" + ProductID.ToString() + ")<br></b>\n");
			writer.Write("<b>Moving Variant: " + DB.RSField(rs,"Name") + " (SKUSuffix=" + DB.RSField(rs,"SKUSuffix") + ", ID=" + DB.RSFieldInt(rs,"VariantID").ToString() + ")<br><br></b>\n");
			rs.Close();

			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function MoveForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("if (theForm.NewProductID.selectedIndex < 1)\n");
			writer.Write("{\n");
			writer.Write("alert(\"Please select the product to which you want to move this variant.\");\n");
			writer.Write("theForm.NewProductID.focus();\n");
			writer.Write("submitenabled(theForm);\n");
			writer.Write("return (false);\n");
			writer.Write("    }\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");

			writer.Write("<p>Select the product to which you want to move this variant:</p>\n");
			writer.Write("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">\n");
			writer.Write("<form action=\"movevariant.aspx?productid=" + ProductID.ToString() + "&VariantID=" + VariantID.ToString() + "\" method=\"post\" id=\"MoveForm\" name=\"MoveForm\" onsubmit=\"return (validateForm(this) && MoveForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("              <tr valign=\"middle\">\n");
			writer.Write("                <td width=\"100%\" colspan=\"2\" align=\"left\">\n");
			writer.Write("                </td>\n");
			writer.Write("              </tr>\n");

			writer.Write("              <tr valign=\"middle\">\n");
			writer.Write("                <td align=\"right\" valign=\"middle\">*Assign To Product:&nbsp;&nbsp;</td>\n");
			writer.Write("                <td align=\"left\">\n");
			writer.Write("<select size=\"1\" name=\"NewProductID\">\n");
			writer.Write(" <OPTION VALUE=\"0\" selected>SELECT ONE</option>\n");
			IDataReader rsst = DB.GetRS("select * from Product  " + DB.GetNoLock() + " where deleted=0 and published=1 order by name");
			while(rsst.Read())
			{
				writer.Write("<option value=\"" + DB.RSFieldInt(rsst,"ProductID").ToString() + "\"");
				writer.Write(">" + DB.RSField(rsst,"Name") + "</option>");
			}
			rsst.Close();
			writer.Write("</select>\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");

			writer.Write("<tr>\n");
			writer.Write("<td></td><td align=\"left\"><br>\n");
			writer.Write("<input type=\"submit\" value=\"Move\" name=\"submit\">\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</form>\n");
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
