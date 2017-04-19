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
	/// Summary description for editextendedprices
	/// </summary>
	public class editextendedprices : SkinBase
	{
		
		int VariantID;
		int ProductID;
		String VariantName;
		String VariantSKUSuffix;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			VariantID = Common.QueryStringUSInt("VariantID");
			ProductID = Common.QueryStringUSInt("ProductID");
			VariantName = Common.GetVariantName(VariantID);
			VariantSKUSuffix = Common.GetVariantSKUSuffix(VariantID);
			if(VariantName.Length == 0)
			{
				VariantName = "(blank)";
			}
			if(VariantSKUSuffix.Length == 0)
			{
				VariantSKUSuffix = "(blank)";
			}
			if(ProductID == 0)
			{
				ProductID = Common.GetVariantProductID(VariantID);
			}
					
			//int N = 0;
			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				// start with clean slate, to make all adds easy:
				DB.ExecuteSQL("delete from extendedprice where VariantID=" + VariantID.ToString());
				for(int i = 0; i<=Request.Form.Count-1; i++)
				{
					String FieldName = Request.Form.Keys[i];
					if(FieldName.IndexOf("_vldt") == -1 && FieldName.IndexOf("Price_") != -1)
					{
						// this field should be processed
						decimal FieldVal = Common.FormUSDecimal(FieldName);
						String[] Parsed = FieldName.Split('_');
						int CustomerLevelID = Localization.ParseUSInt(Parsed[1]);
						if(FieldVal != System.Decimal.Zero)
						{
							DB.ExecuteSQL("insert into ExtendedPrice(ExtendedPriceGUID,VariantID,CustomerLevelID,Price,LastUpdatedBy) values(" + DB.SQuote(DB.GetNewGUID()) + "," + VariantID.ToString() + "," + CustomerLevelID.ToString() + "," + Localization.CurrencyStringForDB(FieldVal) + "," + thisCustomer._customerID.ToString() + ")");
						}
					}
				}			
			}
			SectionTitle = "<a href=\"variants.aspx?productid=" + ProductID.ToString() + "\">Variants</a> - <a href=\"editvariant.aspx?productid=" + ProductID.ToString() + "&VariantID=" + VariantID.ToString() + "\">Edit Variant</a> - Extended Prices";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			IDataReader rs = DB.GetRS("select * from CustomerLevel  " + DB.GetNoLock() + " where deleted=0 order by Name");
			
			if(ErrorMsg.Length != 0)
			{
				writer.Write("<p align=\"left\"><b><font color=red>" + ErrorMsg + "</font></b></p>\n");
			}
			if(DataUpdated)
			{
				writer.Write("<p align=\"left\"><b><font color=blue>(UPDATED)</font></b></p>\n");
			}

			writer.Write("<p align=\"left\"><b>Editing Extended Prices For <a href=\"editvariant.aspx?variantID=" + VariantID.ToString() + "\">Variant</a>: " + VariantName + " (Variant SKUSuffix=" + VariantSKUSuffix + ", VariantID=" + VariantID.ToString() + ")</b></p>\n");

			writer.Write("<p align=\"left\"><b>Within Product: <a href=\"editproduct.aspx?productid=" + ProductID.ToString() + "\">" + Common.GetProductName(ProductID) + "</a> (Product SKU=" + Common.GetProductSKU(ProductID) + ", ProductID=" + ProductID.ToString() + ")</b></p>\n");

			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function Form_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");

			writer.Write("<p align=\"left\"><br><br>Specify any extended pricing that you want for this variant. Entering a value of 0 will delete the extended price for that customer level.</p>\n");
			writer.Write("<table align=\"left\" cellpadding=\"4\" cellspacing=\"0\">\n");
			writer.Write("<form action=\"editextendedprices.aspx?productid=" + ProductID.ToString() + "&VariantID=" + VariantID.ToString() + "\" method=\"post\" id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			
			writer.Write("<tr><td><b>Customer Level</b></td><td><b>Extended Price</b></td></tr>\n");

			while(rs.Read())
			{
				decimal epr = Common.GetVariantExtendedPrice(VariantID,DB.RSFieldInt(rs,"CustomerLevelID"));
				writer.Write("<tr valign=\"middle\">\n");
				writer.Write("<td align=\"left\" valign=\"middle\">Price for Level: <b>" + DB.RSField(rs,"Name") + "</b>:&nbsp;&nbsp;</td>\n");
				writer.Write("<td align=\"left\">\n");
				writer.Write("<input maxLength=\"10\" size=\"10\" name=\"Price_" + DB.RSFieldInt(rs,"CustomerLevelID").ToString() + "\" value=\"" + (Localization.DecimalStringForDB( epr)) + "\">");
				writer.Write("<input type=\"hidden\" name=\"Price_" + DB.RSFieldInt(rs,"CustomerLevelID").ToString() + "_vldt\" value=\"[req][number][blankalert=Please enter an extended price, enter 0 to delete this extended price][invalidalert=Please enter a valid dollar amount, e.g. 10.00 without the leading $ sign!]\">\n");
				writer.Write("</td>\n");
				writer.Write("</tr>\n");
			}
			rs.Close();

			writer.Write("<tr>\n");
			writer.Write("<td align=\"left\" colspan=\"2\"><br>\n");
			writer.Write("<input type=\"submit\" value=\"Update\" name=\"submit\">\n");
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
