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
	/// Summary description for taxzips.
	/// </summary>
	public class taxzips : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Tax Tables By ZipCode";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				for(int i = 0; i<=Request.Form.Count-1; i++)
				{
					if(Request.Form.Keys[i].IndexOf("_New") == -1 && Request.Form.Keys[i].IndexOf("Rate_") != -1 && Request.Form.Keys[i].IndexOf("_vldt") == -1)
					{
						String[] keys = Request.Form.Keys[i].Split('_');
						String ZipCode = keys[1];
						Single TaxRate = 0.0F;
						try
						{
							if(Common.Form("Rate_" + ZipCode).Length != 0)
							{
								TaxRate = Localization.ParseUSSingle(Common.Form("Rate_" + ZipCode));
							}
							DB.ExecuteSQL("update ZipTaxRate set TaxRate=" + Common.IIF(TaxRate != 0.0 , Localization.SingleStringForDB(TaxRate) , "NULL") + ", LastUpdatedBy=" + thisCustomer._customerID.ToString() + ", LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + "  where ZipCode=" + DB.SQuote(ZipCode));
						}
						catch {}
					}
					try
					{
						DB.ExecuteSQL("insert into ZipTaxRate(ZipCode,TaxRate) values(" + Common.Form("ZipCode_New") + "," + Common.Form("Rate_New") + ")");
					}
					catch {}
					DB.ExecuteSQL("delete from ZipTaxRate where TaxRate IS NULL or TaxRate=0.0");
				}
			}

			DataSet ds = DB.GetDS("select * from ZipTaxRate  " + DB.GetNoLock() + " order by ZipCode",false,System.DateTime.Now.AddDays(1));
			
			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function TaxForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");

			writer.Write("<p><b>Leave all ZipCodes blank EXCEPT for those which you want to charge sales tax. For those ZipCodes, enter the total tax rate for your ZipCode (city, county, state, etc.)</b></p>");
			writer.Write("<p>To remove an existing tax rate for any zip code, just set it's tax rate to blank or 0</b>");
			writer.Write("<form id=\"TaxForm\" name=\"TaxForm\" method=\"POST\" action=\"taxzips.aspx\" onsubmit=\"return (validateForm(this) && TaxForm_Validator(this))\" >\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"40%\">\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td><b>ZipCode</b></td>\n");
			writer.Write("      <td><b>Tax Rate (%)</b></td>\n");
			writer.Write("    </tr>\n");
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("<tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
				writer.Write("<td >" + DB.RowField(row,"ZipCode") + "</td>\n");
				writer.Write("<td >\n");
				writer.Write("<input name=\"Rate_" + DB.RowField(row,"ZipCode") + "\" type=\"text\" size=\"3\" value=\"" + Localization.SingleStringForDB(DB.RowFieldSingle(row,"TaxRate")) + "\"> (in x.xx format)");
				writer.Write("<input name=\"Rate_" + DB.RowField(row,"ZipCode") + "_vldt\" type=\"hidden\" value=\"[number][invalidalert=Please enter a tax rate percentage, e.g. 8.1]\">");
				writer.Write("</td >\n");
				writer.Write("</tr>\n");
			}
			ds.Dispose();
			writer.Write("<tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("<td >New ZipCode:");
			writer.Write("<input type=\"text\" name=\"ZipCode_New\" size=\"14\" maxlength=\"10\">");
			writer.Write("<input type=\"hidden\" name=\"ZipCode_New_vldt\" value=\"[zip][invalidalert=Please enter a valid ZipCode]\">");
			writer.Write("</td>\n");
			writer.Write("<td >\n");
			writer.Write("<input name=\"Rate_New\" type=\"text\" size=\"3\">");
			writer.Write("<input name=\"Rate_New_vldt\" type=\"hidden\" value=\"[number][invalidalert=Please enter a tax rate percentage, e.g. 8.1]\">");
			writer.Write("</td >\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("<p align=\"center\"><input type=\"submit\" value=\"Update\" name=\"Submit\"></p>\n");
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
