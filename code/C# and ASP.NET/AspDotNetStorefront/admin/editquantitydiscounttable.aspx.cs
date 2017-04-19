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
	/// Summary description for editquantitydiscounttable.
	/// </summary>
	public class editquantitydiscounttable : SkinBase
	{
		
		int QuantityDiscountID;
		String QuantityDiscountName;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			QuantityDiscountID = Common.QueryStringUSInt("QuantityDiscountID");
			QuantityDiscountName = Common.GetQuantityDiscountName(QuantityDiscountID);
			if(Common.Form("IsSubmitByCount").ToUpper() == "TRUE")
			{
				// check for new row addition:
				Single Low0 = Common.FormUSSingle("Low_0");
				Single High0 = Common.FormUSSingle("High_0");
				String NewGUID = DB.GetNewGUID();
				int NewRowID = 0;

				if(Low0 != 0.0F || High0 != 0.0F)
				{
					// add the new row if necessary:
					Single Discount = Common.FormUSSingle("Rate_0_" + QuantityDiscountID.ToString());
					DB.ExecuteSQL("insert into QuantityDiscountTable(QuantityDiscountTableGUID,QuantityDiscountID,LowQuantity,HighQuantity,DiscountPercent) values(" + DB.SQuote(NewGUID) + "," + QuantityDiscountID.ToString() + "," + Localization.SingleStringForDB(Low0) + "," + Localization.SingleStringForDB(High0) + "," + Localization.SingleStringForDB(Discount) + ")");
				}
				IDataReader rs = DB.GetRS("Select QuantityDiscountTableID from QuantityDiscountTable  " + DB.GetNoLock() + " where QuantityDiscountTableGUID=" + DB.SQuote(NewGUID));
				rs.Read();
				NewRowID = DB.RSFieldInt(rs,"QuantityDiscountTableID");
				rs.Close();

				// update existing rows:
				for(int i = 0; i<=Request.Form.Count-1; i++)
				{
					String FieldName = Request.Form.Keys[i];
					if(FieldName.IndexOf("_0_") == -1  && FieldName != "Low_0" && FieldName != "High_0" && FieldName.IndexOf("_vldt") == -1 && (FieldName.IndexOf("Rate_") != -1 || FieldName.IndexOf("Low_") != -1 || FieldName.IndexOf("High_") != -1))
					{
						Single FieldVal = Common.FormUSSingle(FieldName);
						// this field should be processed
						String[] Parsed = FieldName.Split('_');
						if(FieldName.IndexOf("Rate_") != -1)
						{
							// update discount:
							DB.ExecuteSQL("Update QuantityDiscountTable set DiscountPercent=" + Localization.SingleStringForDB(FieldVal) + ", LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ", LastUpdatedBy=" + thisCustomer._customerID.ToString() + " where QuantityDiscountTableID=" + Parsed[1]);
						}
						if(FieldName.IndexOf("Low_") != -1)
						{
							// update low value:
							DB.ExecuteSQL("Update QuantityDiscountTable set LowQuantity=" + FieldVal.ToString() + ", LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ", LastUpdatedBy=" + thisCustomer._customerID.ToString() + " where QuantityDiscountTableID=" + DB.SQuote(Parsed[1]));
						}
						if(FieldName.IndexOf("High_") != -1)
						{
							// update high value:
							DB.ExecuteSQL("Update QuantityDiscountTable set HighQuantity=" + FieldVal.ToString() + ", LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ", LastUpdatedBy=" + thisCustomer._customerID.ToString() + "  where QuantityDiscountTableID=" + DB.SQuote(Parsed[1]));
						}
					}
				}
				DB.ExecuteSQL("Update QuantityDiscountTable set HighQuantity=999999 where HighQuantity=0.0 and LowQuantity<>0.0");
			}

			if(Common.QueryString("deleteByCountid").Length != 0)
			{
				DB.ExecuteSQL("delete from QuantityDiscountTable where QuantityDiscountTableID=" + Common.QueryString("deleteByCountid"));
			}
			SectionTitle = "<a href=\"quantitydiscounts.aspx\">Quantity Discounts</a> - Manage Quantity Discounts Table: " + QuantityDiscountName;
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function ByCountForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");
			
			writer.Write("<p align=\"left\"><b>DISCOUNT QUANTITY TABLE: " + QuantityDiscountName.ToUpper() + "</b></p>\n");

			writer.Write("<form action=\"editquantitydiscounttable.aspx?quantitydiscountid=" + QuantityDiscountID.ToString() + "\" method=\"post\" id=\"ByCountForm\" name=\"ByCountForm\" onsubmit=\"return (validateForm(this) && ByCountForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmitByCount\" value=\"true\">\n");

			writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"1\">\n");
			writer.Write("<tr bgcolor=\"#FFFFDD\"><td colspan=3 align=\"center\"><b>Order Quantity</b></td><td align=\"center\"><b>Percent Discount</b></td></tr>\n");
			writer.Write("<tr>\n");
			writer.Write("<td align=\"center\" bgcolor=\"#CCFFFF\"><b>Delete</b></td>\n");
			writer.Write("<td align=\"center\" bgcolor=\"#CCFFFF\"><b>Low</b></td>\n");
			writer.Write("<td align=\"center\" bgcolor=\"#CCFFFF\"><b>High</b></td>\n");
			writer.Write("<td align=\"center\" bgcolor=\"#CCFFFF\"><b>Discount Percentage</b></td>\n");
			writer.Write("</tr>\n");

			DataSet QuantityDiscountRows = DB.GetDS("select * from QuantityDiscountTable  " + DB.GetNoLock() + " where QuantityDiscountID=" + QuantityDiscountID.ToString() + " order by LowQuantity",false,System.DateTime.Now.AddDays(1));
			foreach(DataRow discountrow in QuantityDiscountRows.Tables[0].Rows)
			{
				writer.Write("<tr>\n");
				writer.Write("<td align=\"center\" bgcolor=\"#CCFFFF\"><input type=\"Button\" name=\"Delete\" value=\"X\" onClick=\"self.location='editquantitydiscounttable.aspx?quantitydiscountid=" + QuantityDiscountID.ToString() + "&deleteByCountid=" + DB.RowFieldInt(discountrow,"QuantityDiscountTableID").ToString() + "'\"></td>\n");
				writer.Write("<td align=\"center\" bgcolor=\"#CCFFFF\">\n");
				writer.Write("<input maxLength=\"10\" size=\"10\" name=\"Low_" + DB.RowFieldInt(discountrow,"QuantityDiscountTableID").ToString() + "\" value=\"" + DB.RowFieldSingle(discountrow,"LowQuantity").ToString() + "\">\n");
				writer.Write("<input type=\"hidden\" name=\"Low_" + DB.RowFieldInt(discountrow,"QuantityDiscountTableID").ToString() + "_vldt\" value=\"[number][blankalert=Please enter starting order amount][invalidalert=Please enter a money value, WITHOUT the dollar sign]\">\n");
				writer.Write("</td>\n");
				writer.Write("<td align=\"center\" bgcolor=\"#CCFFFF\">\n");
				writer.Write("<input maxLength=\"10\" size=\"10\" name=\"High_" + DB.RowFieldInt(discountrow,"QuantityDiscountTableID").ToString() + "\" value=\"" + DB.RowFieldSingle(discountrow,"HighQuantity").ToString() + "\">\n");
				writer.Write("<input type=\"hidden\" name=\"High_" + DB.RowFieldInt(discountrow,"QuantityDiscountTableID").ToString() + "_vldt\" value=\"[number][blankalert=Please enter ending order amount][invalidalert=Please enter a money value, WITHOUT the dollar sign]\">\n");
				writer.Write("</td>\n");
				writer.Write("<td align=\"center\" bgcolor=\"#CCFFFF\">\n");
				writer.Write("<input maxLength=\"10\" size=\"10\" name=\"Rate_" + DB.RowFieldInt(discountrow,"QuantityDiscountTableID").ToString() + "\" value=\"" + Localization.SingleStringForDB( DB.RowFieldSingle(discountrow,"DiscountPercent")) + "\">\n");
				writer.Write("<input type=\"hidden\" name=\"Rate_" + DB.RowFieldInt(discountrow,"QuantityDiscountTableID").ToString() + "_vldt\" value=\"[number][blankalert=Please enter the discount percent][invalidalert=Please enter a money value, WITHOUT the dollar sign]\">\n");
				writer.Write("</td>\n");
				writer.Write("</tr>\n");
			}
			// add new row:
			writer.Write("<tr>\n");
			writer.Write("<td align=\"center\" bgcolor=\"#CCFFFF\">Add New Row Here:</td>\n");
			writer.Write("<td align=\"center\" bgcolor=\"#CCFFFF\">\n");
			writer.Write("<input maxLength=\"10\" size=\"10\" name=\"Low_0\" value=\"\">\n");
			writer.Write("<input type=\"hidden\" name=\"Low_0_vldt\" value=\"[int][blankalert=Please enter starting order quantity][invalidalert=Please enter an integer]\">\n");
			writer.Write("</td>\n");
			writer.Write("<td align=\"center\" bgcolor=\"#CCFFFF\">\n");
			writer.Write("<input maxLength=\"10\" size=\"10\" name=\"High_0\" value=\"\">\n");
			writer.Write("<input type=\"hidden\" name=\"High_0_vldt\" value=\"[int][blankalert=Please enter ending order quantity][invalidalert=Please enter an integer]\">\n");
			writer.Write("</td>\n");
			writer.Write("<td align=\"center\" bgcolor=\"#CCFFFF\">\n");
			writer.Write("<input maxLength=\"10\" size=\"10\" name=\"Rate_0_" + QuantityDiscountID.ToString() + "\" value=\"\">\n");
			writer.Write("<input type=\"hidden\" name=\"Rate_0_" + QuantityDiscountID.ToString() + "_vldt\" value=\"[number][blankalert=Please enter the desired percent discount][invalidalert=Please enter a percentage value, WITHOUT the percent sign]\">\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("<p align=\"left\"><input type=\"submit\" value=\"Update\" name=\"submit\"></p>\n");

			writer.Write("</form>\n");
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
