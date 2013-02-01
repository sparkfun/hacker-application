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
	/// Summary description for shipping.
	/// </summary>
	public class shipping : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Shipping Tables";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			String EditGUID = Common.QueryString("EditGUID");
			
			if(Common.Form("IsSubmitCalculationID").ToUpper() == "TRUE")
			{
				DB.ExecuteSQL("Update ShippingCalculation set Selected=0");
				DB.ExecuteSQL("Update ShippingCalculation set Selected=1 where ShippingCalculationID=" + Common.FormUSInt("ShippingCalculationID").ToString());
			}

			if(Common.Form("IsSubmitFixedRate").ToUpper() == "TRUE")
			{
				DataSet ds = DB.GetDS("select * from ShippingMethod  " + DB.GetNoLock() + " where name not like " + DB.SQuote("%Real Time%") + " and deleted=0 order by DisplayOrder,Name",false,System.DateTime.Now.AddDays(1));
				foreach(DataRow row in ds.Tables[0].Rows)
				{
					String FieldName = "FixedRate_" + DB.RowFieldInt(row,"ShippingMethodID").ToString();
					if(Common.Form(FieldName).Length != 0)
					{
						DB.ExecuteSQL("Update ShippingMethod set FixedRate=" + Localization.CurrencyStringForDB(Common.FormUSDecimal(FieldName)) + " where ShippingMethodID=" + DB.RowFieldInt(row,"ShippingMethodID").ToString());
					}
					else
					{
						DB.ExecuteSQL("Update ShippingMethod set FixedRate=NULL where ShippingMethodID=" + DB.RowFieldInt(row,"ShippingMethodID").ToString());
					}
				}
				ds.Dispose();
			}

			if(Common.Form("IsSubmitFixedPercentOfTotal").ToUpper() == "TRUE")
			{
				DataSet ds = DB.GetDS("select * from ShippingMethod  " + DB.GetNoLock() + " where name not like " + DB.SQuote("%Real Time%") + " and deleted=0 order by DisplayOrder,Name",false,System.DateTime.Now.AddDays(1));
				foreach(DataRow row in ds.Tables[0].Rows)
				{
					String FieldName = "FixedPercentOfTotal_" + DB.RowFieldInt(row,"ShippingMethodID").ToString();
					if(Common.Form(FieldName).Length != 0)
					{
						DB.ExecuteSQL("Update ShippingMethod set FixedPercentOfTotal=" + Localization.SingleStringForDB(Common.FormUSSingle(FieldName)) + " where ShippingMethodID=" + DB.RowFieldInt(row,"ShippingMethodID").ToString());
					}
					else
					{
						DB.ExecuteSQL("Update ShippingMethod set FixedPercentOfTotal=NULL where ShippingMethodID=" + DB.RowFieldInt(row,"ShippingMethodID").ToString());
					}
				}
				ds.Dispose();
			}

			if(Common.Form("IsSubmitByTotal").ToUpper() == "TRUE")
			{
				if(EditGUID.Length != 0)
				{
					DB.ExecuteSQL("delete from ShippingByTotal where RowGUID=" + DB.SQuote(EditGUID));
				}

				// check for new row addition:
				Decimal Low0 = Common.FormUSDecimal("Low_0");
				Decimal High0 = Common.FormUSDecimal("High_0");
				String NewRowGUID = DB.GetNewGUID();

				if(Low0 != System.Decimal.Zero || High0 != System.Decimal.Zero)
				{
					// add the new row if necessary:
					DataSet dsx = DB.GetDS("select * from ShippingMethod  " + DB.GetNoLock() + " where deleted=0 order by DisplayOrder,Name",false,System.DateTime.Now.AddDays(1));
					foreach(DataRow row in dsx.Tables[0].Rows)
					{
						decimal Charge = Common.FormUSDecimal("Rate_0_" + DB.RowFieldInt(row,"ShippingMethodID").ToString());
						DB.ExecuteSQL("insert into ShippingByTotal(RowGUID,LowValue,HighValue,ShippingMethodID,ShippingCharge) values(" + DB.SQuote(NewRowGUID) + "," + Localization.CurrencyStringForDB(Low0) + "," + Localization.CurrencyStringForDB(High0) + "," + DB.RowFieldInt(row,"ShippingMethodID").ToString() + "," + Localization.CurrencyStringForDB(Charge) + ")");
					}
					dsx.Dispose();
				}

				// update existing rows:
				for(int i = 0; i<=Request.Form.Count-1; i++)
				{
					String FieldName = Request.Form.Keys[i];
					if(FieldName.IndexOf("_0_") == -1  && FieldName != "Low_0" && FieldName != "High_0" && FieldName.IndexOf("_vldt") == -1 && (FieldName.IndexOf("Rate_") != -1 || FieldName.IndexOf("Low_") != -1 || FieldName.IndexOf("High_") != -1))
					{
						decimal FieldVal = Common.FormUSDecimal(FieldName);
						// this field should be processed
						String[] Parsed = FieldName.Split('_');
						if(FieldName.IndexOf("Rate_") != -1)
						{
							// update shipping costs:
							DB.ExecuteSQL("insert into ShippingByTotal(RowGUID,LowValue,HighValue,ShippingMethodID,ShippingCharge) values(" + DB.SQuote(Parsed[1]) + "," + Localization.CurrencyStringForDB(Common.FormUSDecimal("Low_" + Parsed[1])) + "," + Localization.CurrencyStringForDB(Common.FormUSDecimal("High_" + Parsed[1])) + "," + Parsed[2] + "," + Localization.CurrencyStringForDB(FieldVal) + ")");
						}
					}
				}
				DB.ExecuteSQL("Update ShippingByTotal set HighValue=99999.99 where HighValue=0.0 and LowValue<>0.0");
			}

			if(Common.Form("IsSubmitByWeight").ToUpper() == "TRUE")
			{
				if(EditGUID.Length != 0)
				{
					DB.ExecuteSQL("delete from ShippingByWeight where RowGUID=" + DB.SQuote(EditGUID));
				}

				// check for new row addition:
				Single Low0 = Common.FormUSSingle("Low_0");
				Single High0 = Common.FormUSSingle("High_0");
				String NewRowGUID = DB.GetNewGUID();

				if(Low0 != 0.0F || High0 != 0.0F)
				{
					// add the new row if necessary:
					DataSet dsx = DB.GetDS("select * from ShippingMethod  " + DB.GetNoLock() + " where deleted=0 order by DisplayOrder,Name",false,System.DateTime.Now.AddDays(1));
					foreach(DataRow row in dsx.Tables[0].Rows)
					{
						decimal Charge = Common.FormUSDecimal("Rate_0_" + DB.RowFieldInt(row,"ShippingMethodID").ToString());
						DB.ExecuteSQL("insert into ShippingByWeight(RowGUID,LowValue,HighValue,ShippingMethodID,ShippingCharge) values(" + DB.SQuote(NewRowGUID) + "," + Localization.SingleStringForDB(Low0) + "," + Localization.SingleStringForDB(High0) + "," + DB.RowFieldInt(row,"ShippingMethodID").ToString() + "," + Localization.CurrencyStringForDB(Charge) + ")");
					}
					dsx.Dispose();
				}

				// update existing rows:
				for(int i = 0; i<=Request.Form.Count-1; i++)
				{
					String FieldName = Request.Form.Keys[i];
					if(FieldName.IndexOf("_0_") == -1  && FieldName != "Low_0" && FieldName != "High_0" && FieldName.IndexOf("_vldt") == -1 && (FieldName.IndexOf("Rate_") != -1 || FieldName.IndexOf("Low_") != -1 || FieldName.IndexOf("High_") != -1))
					{
						decimal FieldVal = Common.FormUSDecimal(FieldName);
						// this field should be processed
						String[] Parsed = FieldName.Split('_');
						if(FieldName.IndexOf("Rate_") != -1)
						{
							// update shipping costs:
							DB.ExecuteSQL("insert into ShippingByWeight(RowGUID,LowValue,HighValue,ShippingMethodID,ShippingCharge) values(" + DB.SQuote(Parsed[1]) + "," + Localization.SingleStringForDB(Common.FormUSSingle("Low_" + Parsed[1])) + "," + Localization.SingleStringForDB(Common.FormUSSingle("High_" + Parsed[1])) + "," + Parsed[2] + "," + Localization.CurrencyStringForDB(FieldVal) + ")");
						}
					}
				}
				DB.ExecuteSQL("Update ShippingByTotal set HighValue=99999.99 where HighValue=0.0 and LowValue<>0.0");
			}

			if(Common.Form("IsSubmitByZone").ToUpper() == "TRUE")
			{
				if(EditGUID.Length != 0)
				{
					DB.ExecuteSQL("delete from ShippingByZone where RowGUID=" + DB.SQuote(EditGUID));
				}

				// check for new row addition:
				Single Low0 = Common.FormUSSingle("Low_0");
				Single High0 = Common.FormUSSingle("High_0");
				String NewRowGUID = DB.GetNewGUID();

				if(Low0 != 0.0F || High0 != 0.0F)
				{
					// add the new row if necessary:
					DataSet dsx = DB.GetDS("select * from ShippingZone  " + DB.GetNoLock() + " where deleted=0 order by Name",false,System.DateTime.Now.AddDays(1));
					foreach(DataRow row in dsx.Tables[0].Rows)
					{
						Single Charge = Common.FormUSSingle("Rate_0_" + DB.RowFieldInt(row,"ShippingZoneID").ToString());
						DB.ExecuteSQL("insert into ShippingByZone(RowGUID,LowValue,HighValue,ShippingZoneID,ShippingCharge) values(" + DB.SQuote(NewRowGUID) + "," + Localization.SingleStringForDB(Low0) + "," + Localization.SingleStringForDB(High0) + "," + DB.RowFieldInt(row,"ShippingZoneID").ToString() + "," + Localization.SingleStringForDB(Charge) + ")");
					}
					dsx.Dispose();
				}

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
							// update shipping costs:
							DB.ExecuteSQL("insert into ShippingByZone(RowGUID,LowValue,HighValue,ShippingZoneID,ShippingCharge) values(" + DB.SQuote(Parsed[1]) + "," + Localization.SingleStringForDB(Common.FormUSSingle("Low_" + Parsed[1])) + "," + Localization.SingleStringForDB(Common.FormUSSingle("High_" + Parsed[1])) + "," + Parsed[2] + "," + Localization.SingleStringForDB(FieldVal) + ")");
						}
					}
				}
				DB.ExecuteSQL("Update ShippingByZone set HighValue=99999.99 where HighValue=0.0 and LowValue<>0.0");
			}

			if(Common.QueryString("deletebytotalid").Length != 0)
			{
				DB.ExecuteSQL("delete from ShippingByTotal where RowGUID=" + DB.SQuote(Common.QueryString("deletebytotalid")));
			}

			if(Common.QueryString("deletebyWeightid").Length != 0)
			{
				DB.ExecuteSQL("delete from ShippingByWeight where RowGUID=" + DB.SQuote(Common.QueryString("deletebyWeightid")));
			}
			if(Common.QueryString("deletebyZoneid").Length != 0)
			{
				DB.ExecuteSQL("delete from ShippingByZone where RowGUID=" + DB.SQuote(Common.QueryString("deletebyZoneid")));
			}

			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function ShippingForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");
			writer.Write("function FixedRateForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");
			writer.Write("function FixedPercentOfTotalForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");
			writer.Write("function ByTotalForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");
			writer.Write("function ByWeightForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");
			writer.Write("function ByZoneForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");
			
			IDataReader rs = DB.GetRS("select * from ShippingCalculation  " + DB.GetNoLock() + " order by shippingcalculationid");
			writer.Write("<form action=\"shipping.aspx\" method=\"post\" id=\"ShippingForm\" name=\"ShippingForm\" onsubmit=\"return (validateForm(this) && ShippingForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmitCalculationID\" value=\"true\">\n");
			int SelectedID = 0;
			while(rs.Read())
			{
				writer.Write("<p><input type=\"radio\" name=\"ShippingCalculationID\" value=\"" + DB.RSFieldInt(rs,"ShippingCalculationID").ToString() + "\" " + Common.IIF(DB.RSFieldBool(rs,"Selected") , " checked " , "") + "><b>" + DB.RSField(rs,"Name") + "</b></p>\n");
				if(DB.RSFieldBool(rs,"Selected"))
				{
					SelectedID = DB.RSFieldInt(rs,"ShippingCalculationID");
				}
			}
			rs.Close();
			writer.Write("<input type=\"submit\" value=\"Update\" name=\"submit\">\n");
			writer.Write("</form>\n");

			DataSet dsWeight;
			switch(SelectedID)
			{
				case 1:
					// use shipping by weight:
					writer.Write("<hr size=1>");

					writer.Write("<p><b>ACTIVE RATE TABLE FOR: CALCULATE SHIPPING BY ORDER WEIGHT:</b></p>\n");

					writer.Write("<form action=\"shipping.aspx?EditGUID=" + EditGUID + "\" method=\"post\" id=\"ByWeightForm\" name=\"ByWeightForm\" onsubmit=\"return (validateForm(this) && ByWeightForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
					writer.Write("<input type=\"hidden\" name=\"IsSubmitByWeight\" value=\"true\">\n");

					dsWeight = DB.GetDS("select * from ShippingMethod  " + DB.GetNoLock() + " where deleted=0 order by DisplayOrder,Name",false,System.DateTime.Now.AddDays(1));
					if(dsWeight.Tables[0].Rows.Count == 0)
					{
						writer.Write("No shipping methods are defined. <a href=\"shippingmethods.aspx\">Click here</a> to define your shipping methods");
					}
					else
					{
						writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"1\">\n");
						writer.Write("<tr bgcolor=\"#FFFFDD\"><td colspan=2 align=\"center\"><b>Order Weight (in " + Localization.WeightUnits() + ")</b></td><td colspan=" + dsWeight.Tables[0].Rows.Count.ToString() + " align=\"center\"><b>Shipping Charge By Weight</b></td><td>&nbsp;</td><td>&nbsp;</td></tr>\n");
						writer.Write("<tr>\n");
						writer.Write("<td align=\"center\"><b>Low</b></td>\n");
						writer.Write("<td align=\"center\"><b>High</b></td>\n");
						foreach(DataRow row in dsWeight.Tables[0].Rows)
						{
							writer.Write("<td align=\"center\"><b>" + DB.RowField(row,"Name") + "</b></td>\n");
						}
						writer.Write("<td align=\"center\"><b>Edit</b></td>\n");
						writer.Write("<td align=\"center\"><b>Delete</b></td>\n");
						writer.Write("</tr>\n");

						DataSet ShippingRows = DB.GetDS("select distinct rowguid,lowvalue,highvalue from ShippingByWeight  " + DB.GetNoLock() + " order by LowValue",false,System.DateTime.Now.AddDays(1));
						foreach(DataRow shiprow in ShippingRows.Tables[0].Rows)
						{
							bool EditRow = (EditGUID == DB.RowFieldGUID(shiprow,"RowGUID"));
							writer.Write("<tr>\n");
							writer.Write("<td align=\"center\">\n");
							if(EditRow)
							{
								writer.Write("<input maxLength=\"10\" size=\"10\" name=\"Low_" + DB.RowFieldGUID(shiprow,"RowGUID") + "\" value=\"" + Localization.SingleStringForDB(DB.RowFieldSingle(shiprow,"LowValue")) + "\">\n");
								writer.Write("<input type=\"hidden\" name=\"Low_" + DB.RowFieldGUID(shiprow,"RowGUID") + "_vldt\" value=\"[number][blankalert=Please enter starting order amount][invalidalert=Please enter a weight value]\">\n");
							}
							else
							{
								writer.Write(Localization.SingleStringForDB(DB.RowFieldSingle(shiprow,"LowValue")));
							}
							writer.Write("</td>\n");
							writer.Write("<td align=\"center\">\n");
							if(EditRow)
							{
								writer.Write("<input maxLength=\"10\" size=\"10\" name=\"High_" + DB.RowFieldGUID(shiprow,"RowGUID") + "\" value=\"" + Localization.SingleStringForDB(DB.RowFieldSingle(shiprow,"HighValue")) + "\">\n");
								writer.Write("<input type=\"hidden\" name=\"High_" + DB.RowFieldGUID(shiprow,"RowGUID") + "_vldt\" value=\"[number][blankalert=Please enter ending order amount][invalidalert=Please enter a weight value]\">\n");
							}
							else
							{
								writer.Write(Localization.SingleStringForDB(DB.RowFieldSingle(shiprow,"HighValue")));
							}
							writer.Write("</td>\n");
							foreach(DataRow row in dsWeight.Tables[0].Rows)
							{
								writer.Write("<td align=\"center\">\n");
								if(EditRow)
								{
									writer.Write("<input maxLength=\"10\" size=\"10\" name=\"Rate_" + DB.RowFieldGUID(shiprow,"RowGUID") + "_" + DB.RowFieldInt(row,"ShippingMethodID").ToString() + "\" value=\"" + Localization.CurrencyStringForDB(Common.GetShipByWeightCharge(DB.RowFieldInt(row,"ShippingMethodID"),DB.RowFieldGUID(shiprow,"RowGUID"))) + "\">\n");
									writer.Write("<input type=\"hidden\" name=\"Rate_" + DB.RowFieldGUID(shiprow,"RowGUID") + "_" + DB.RowFieldInt(row,"ShippingMethodID").ToString() + "_vldt\" value=\"[number][blankalert=Please enter the shipping cost][invalidalert=Please enter a money value, WITHOUT the dollar sign]\">\n");
								}
								else
								{
									writer.Write(Localization.CurrencyStringForDB(Common.GetShipByWeightCharge(DB.RowFieldInt(row,"ShippingMethodID"),DB.RowFieldGUID(shiprow,"RowGUID"))));
								}
								writer.Write("</td>\n");
							}
							if(EditRow)
							{
								writer.Write("<td align=\"center\">");
								writer.Write("<input type=\"submit\" value=\"Update\" name=\"submit\">\n");
								writer.Write("</td>");
							}
							else
							{
								writer.Write("<td align=\"center\"><input type=\"Button\" name=\"Edit\" value=\"Edit\" onClick=\"self.location='shipping.aspx?EditGUID=" + DB.RowFieldGUID(shiprow,"RowGUID") + "'\"></td>\n");
							}
							writer.Write("<td align=\"center\"><input type=\"Button\" name=\"Delete\" value=\"Delete\" onClick=\"self.location='shipping.aspx?deleteByWeightid=" + DB.RowFieldGUID(shiprow,"RowGUID") + "'\"></td>\n");
							writer.Write("</tr>\n");
						}
						// add new row:
						writer.Write("<tr>\n");
						writer.Write("<td align=\"center\">\n");
						writer.Write("<input maxLength=\"10\" size=\"10\" name=\"Low_0\" \">\n");
						writer.Write("<input type=\"hidden\" name=\"Low_0_vldt\" value=\"[number][blankalert=Please enter starting order amount][invalidalert=Please enter a money value, WITHOUT the dollar sign]\">\n");
						writer.Write("</td>\n");
						writer.Write("<td align=\"center\">\n");
						writer.Write("<input maxLength=\"10\" size=\"10\" name=\"High_0\" >\n");
						writer.Write("<input type=\"hidden\" name=\"High_0_vldt\" value=\"[number][blankalert=Please enter ending order amount][invalidalert=Please enter a money value, WITHOUT the dollar sign]\">\n");
						writer.Write("</td>\n");
						foreach(DataRow row in dsWeight.Tables[0].Rows)
						{
							writer.Write("<td align=\"center\">\n");
							writer.Write("<input maxLength=\"10\" size=\"10\" name=\"Rate_0_" + DB.RowFieldInt(row,"ShippingMethodID").ToString() + "\">\n");
							writer.Write("<input type=\"hidden\" name=\"Rate_0_" + DB.RowFieldInt(row,"ShippingMethodID").ToString() + "_vldt\" value=\"[number][blankalert=Please enter the shipping cost][invalidalert=Please enter a money value, WITHOUT the dollar sign]\">\n");
							writer.Write("</td>\n");
						}
						writer.Write("<td align=\"center\">");
						writer.Write("<input type=\"submit\" value=\"Add New Row\" name=\"submit\">\n");
						writer.Write("</td>\n");
						writer.Write("<td>&nbsp;</td>");
						writer.Write("</tr>\n");
						writer.Write("</table>\n");
					}
					dsWeight.Dispose();

					writer.Write("</form>\n");
					break;
				case 2:
					// use shipping by Total:
					writer.Write("<hr size=1>");

					writer.Write("<p><b>ACTIVE RATE TABLE FOR: CALCULATE SHIPPING BY ORDER TOTAL:</b></p>\n");

					writer.Write("<form action=\"shipping.aspx?EditGUID=" + EditGUID + "\" method=\"post\" id=\"ByTotalForm\" name=\"ByTotalForm\" onsubmit=\"return (validateForm(this) && ByTotalForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
					writer.Write("<input type=\"hidden\" name=\"IsSubmitByTotal\" value=\"true\">\n");

					DataSet dsTotal = DB.GetDS("select * from ShippingMethod  " + DB.GetNoLock() + " where deleted=0 order by DisplayOrder,Name",false,System.DateTime.Now.AddDays(1));
					if(dsTotal.Tables[0].Rows.Count == 0)
					{
						writer.Write("No shipping methods are defined. <a href=\"shippingmethods.aspx\">Click here</a> to define your shipping methods");
					}
					else
					{
						writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"1\">\n");
						writer.Write("<tr bgcolor=\"#FFFFDD\"><td colspan=2 align=\"center\"><b>Order Total (in your currency)</b></td><td colspan=" + dsTotal.Tables[0].Rows.Count.ToString() + " align=\"center\"><b>Shipping Charge By Total</b></td><td>&nbsp;</td><td>&nbsp;</td></tr>\n");
						writer.Write("<tr>\n");
						writer.Write("<td align=\"center\"><b>Low</b></td>\n");
						writer.Write("<td align=\"center\"><b>High</b></td>\n");
						foreach(DataRow row in dsTotal.Tables[0].Rows)
						{
							writer.Write("<td align=\"center\"><b>" + DB.RowField(row,"Name") + "</b></td>\n");
						}
						writer.Write("<td align=\"center\"><b>Edit</b></td>\n");
						writer.Write("<td align=\"center\"><b>Delete</b></td>\n");
						writer.Write("</tr>\n");

						DataSet ShippingRows = DB.GetDS("select distinct rowguid,lowvalue,highvalue from ShippingByTotal  " + DB.GetNoLock() + " order by LowValue",false,System.DateTime.Now.AddDays(1));
						foreach(DataRow shiprow in ShippingRows.Tables[0].Rows)
						{
							bool EditRow = (EditGUID == DB.RowFieldGUID(shiprow,"RowGUID"));
							writer.Write("<tr>\n");
							writer.Write("<td align=\"center\">\n");
							if(EditRow)
							{
								writer.Write("<input maxLength=\"10\" size=\"10\" name=\"Low_" + DB.RowFieldGUID(shiprow,"RowGUID") + "\" value=\"" + Localization.CurrencyStringForDB(DB.RowFieldDecimal(shiprow,"LowValue")) + "\">\n");
								writer.Write("<input type=\"hidden\" name=\"Low_" + DB.RowFieldGUID(shiprow,"RowGUID") + "_vldt\" value=\"[number][blankalert=Please enter starting order amount][invalidalert=Please enter a money value, WITHOUT the dollar sign]\">\n");
							}
							else
							{
								writer.Write(Localization.CurrencyStringForDB(DB.RowFieldDecimal(shiprow,"LowValue")));
							}
							writer.Write("</td>\n");
							writer.Write("<td align=\"center\">\n");
							if(EditRow)
							{
								writer.Write("<input maxLength=\"10\" size=\"10\" name=\"High_" + DB.RowFieldGUID(shiprow,"RowGUID") + "\" value=\"" + Localization.CurrencyStringForDB(DB.RowFieldDecimal(shiprow,"HighValue")) + "\">\n");
								writer.Write("<input type=\"hidden\" name=\"High_" + DB.RowFieldGUID(shiprow,"RowGUID") + "_vldt\" value=\"[number][blankalert=Please enter ending order amount][invalidalert=Please enter a money value, WITHOUT the dollar sign]\">\n");
							}
							else
							{
								writer.Write(Localization.CurrencyStringForDB(DB.RowFieldDecimal(shiprow,"HighValue")));
							}
							writer.Write("</td>\n");
							foreach(DataRow row in dsTotal.Tables[0].Rows)
							{
								writer.Write("<td align=\"center\">\n");
								if(EditRow)
								{
									writer.Write("<input maxLength=\"10\" size=\"10\" name=\"Rate_" + DB.RowFieldGUID(shiprow,"RowGUID") + "_" + DB.RowFieldInt(row,"ShippingMethodID").ToString() + "\" value=\"" + Localization.CurrencyStringForDB(Common.GetShipByTotalCharge(DB.RowFieldInt(row,"ShippingMethodID"),DB.RowFieldGUID(shiprow,"RowGUID"))) + "\">\n");
									writer.Write("<input type=\"hidden\" name=\"Rate_" + DB.RowFieldGUID(shiprow,"RowGUID") + "_" + DB.RowFieldInt(row,"ShippingMethodID").ToString() + "_vldt\" value=\"[number][blankalert=Please enter the shipping cost][invalidalert=Please enter a money value, WITHOUT the dollar sign]\">\n");
								}
								else
								{
									writer.Write(Localization.CurrencyStringForDB(Common.GetShipByTotalCharge(DB.RowFieldInt(row,"ShippingMethodID"),DB.RowFieldGUID(shiprow,"RowGUID"))));
								}
								writer.Write("</td>\n");
							}
							if(EditRow)
							{
								writer.Write("<td align=\"center\">");
								writer.Write("<input type=\"submit\" value=\"Update\" name=\"submit\">\n");
								writer.Write("</td>");
							}
							else
							{
								writer.Write("<td align=\"center\"><input type=\"Button\" name=\"Edit\" value=\"Edit\" onClick=\"self.location='shipping.aspx?EditGUID=" + DB.RowFieldGUID(shiprow,"RowGUID") + "'\"></td>\n");
							}
							writer.Write("<td align=\"center\"><input type=\"Button\" name=\"Delete\" value=\"Delete\" onClick=\"self.location='shipping.aspx?deleteByTotalid=" + DB.RowFieldGUID(shiprow,"RowGUID") + "'\"></td>\n");
							writer.Write("</tr>\n");
						}
						// add new row:
						writer.Write("<tr>\n");
						writer.Write("<td align=\"center\">\n");
						writer.Write("<input maxLength=\"10\" size=\"10\" name=\"Low_0\" \">\n");
						writer.Write("<input type=\"hidden\" name=\"Low_0_vldt\" value=\"[number][blankalert=Please enter starting order amount][invalidalert=Please enter a money value, WITHOUT the dollar sign]\">\n");
						writer.Write("</td>\n");
						writer.Write("<td align=\"center\">\n");
						writer.Write("<input maxLength=\"10\" size=\"10\" name=\"High_0\" >\n");
						writer.Write("<input type=\"hidden\" name=\"High_0_vldt\" value=\"[number][blankalert=Please enter ending order amount][invalidalert=Please enter a money value, WITHOUT the dollar sign]\">\n");
						writer.Write("</td>\n");
						foreach(DataRow row in dsTotal.Tables[0].Rows)
						{
							writer.Write("<td align=\"center\">\n");
							writer.Write("<input maxLength=\"10\" size=\"10\" name=\"Rate_0_" + DB.RowFieldInt(row,"ShippingMethodID").ToString() + "\">\n");
							writer.Write("<input type=\"hidden\" name=\"Rate_0_" + DB.RowFieldInt(row,"ShippingMethodID").ToString() + "_vldt\" value=\"[number][blankalert=Please enter the shipping cost][invalidalert=Please enter a money value, WITHOUT the dollar sign]\">\n");
							writer.Write("</td>\n");
						}
						writer.Write("<td align=\"center\">");
						writer.Write("<input type=\"submit\" value=\"Add New Row\" name=\"submit\">\n");
						writer.Write("</td>\n");
						writer.Write("<td>&nbsp;</td>");
						writer.Write("</tr>\n");
						writer.Write("</table>\n");
					}
					dsTotal.Dispose();

					writer.Write("</form>\n");
					break;
				case 3:
					// use fixed rates:

					writer.Write("<hr size=1>");
					writer.Write("<p><b>FIXED RATE SHIPPING TABLE:</b></p>\n");

					writer.Write("<form action=\"shipping.aspx?EditGUID=" + EditGUID + "\" method=\"post\" id=\"FixedRateForm\" name=\"FixedRateForm\" onsubmit=\"return (validateForm(this) && FixedRateForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
					writer.Write("<input type=\"hidden\" name=\"IsSubmitFixedRate\" value=\"true\">\n");

					DataSet dsfixed = DB.GetDS("select * from ShippingMethod  " + DB.GetNoLock() + " where name not like " + DB.SQuote("%Real Time%") + " and deleted=0 order by DisplayOrder,Name",false,System.DateTime.Now.AddDays(1));
					if(dsfixed.Tables[0].Rows.Count == 0)
					{
						writer.Write("No shipping methods are defined. <a href=\"shippingmethods.aspx\">Click here</a> to define your shipping methods first");
					}
					else
					{
						writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"1\">\n");
						writer.Write("<tr><td bgcolor=\"#CCFFFF\"><b>Shipping Method</b></td><td bgcolor=\"#CCFFFF\"><b>Flat Rate</b></td></tr>\n");
						foreach(DataRow row in dsfixed.Tables[0].Rows)
						{
							writer.Write("<tr>\n");
							writer.Write("<td>" + DB.RowField(row,"Name") + "</td>\n");
							writer.Write("<td>\n");
							writer.Write("<input maxLength=\"10\" size=\"10\" name=\"FixedRate_" + DB.RowFieldInt(row,"ShippingMethodID").ToString() + "\" value=\"" + Localization.CurrencyStringForDB(DB.RowFieldDecimal(row,"FixedRate")) + "\"> (in x.xx format)\n");
							writer.Write("<input type=\"hidden\" name=\"FixedRate_" + DB.RowFieldInt(row,"ShippingMethodID").ToString() + "_vldt\" value=\"[number][req][blankalert=Please enter the shipping cost][invalidalert=Please enter a money value, WITHOUT the dollar sign]\">\n");
							writer.Write("</td>\n");
							writer.Write("</tr>\n");
						}
						writer.Write("<tr><td></td><td align=\"left\"><input type=\"submit\" value=\"Update\" name=\"submit\"></td></tr>\n");
						writer.Write("</table>\n");
					}
					dsfixed.Dispose();

					writer.Write("</form>\n");

					break;
				case 4:
					// all orders have free shipping, no table necessary
					break;
				case 5:
					// use fixed percent of total order:

					writer.Write("<hr size=1>");
					writer.Write("<p><b>FIXED PERCENT OF TOTAL ORDER SHIPPING TABLE:</b></p>\n");

					writer.Write("<form action=\"shipping.aspx?EditGUID=" + EditGUID + "\" method=\"post\" id=\"FixedPercentOfTotalForm\" name=\"FixedPercentOfTotalForm\" onsubmit=\"return (validateForm(this) && FixedPercentOfTotalForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
					writer.Write("<input type=\"hidden\" name=\"IsSubmitFixedPercentOfTotal\" value=\"true\">\n");

					DataSet dsfixedp = DB.GetDS("select * from ShippingMethod  " + DB.GetNoLock() + " where name not like " + DB.SQuote("%Real Time%") + "  and deleted=0 order by DisplayOrder,Name",false,System.DateTime.Now.AddDays(1));
					if(dsfixedp.Tables[0].Rows.Count == 0)
					{
						writer.Write("No shipping methods are defined. <a href=\"shippingmethods.aspx\">Click here</a> to define your shipping methods first");
					}
					else
					{
						writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"1\">\n");
						writer.Write("<tr><td bgcolor=\"#CCFFFF\"><b>Shipping Method</b></td><td bgcolor=\"#CCFFFF\"><b>Flat Percent Of Total Order Cost</b></td></tr>\n");
						foreach(DataRow row in dsfixedp.Tables[0].Rows)
						{
							writer.Write("<tr>\n");
							writer.Write("<td>" + DB.RowField(row,"Name") + "</td>\n");
							writer.Write("<td>\n");
							writer.Write("<input maxLength=\"10\" size=\"10\" name=\"FixedPercentOfTotal_" + DB.RowFieldInt(row,"ShippingMethodID").ToString() + "\" value=\"" + Localization.SingleStringForDB(DB.RowFieldSingle(row,"FixedPercentOfTotal")) + "\"> (in x.xx format)\n");
							writer.Write("<input type=\"hidden\" name=\"FixedPercentOfTotal_" + DB.RowFieldInt(row,"ShippingMethodID").ToString() + "_vldt\" value=\"[number][req][blankalert=Please enter the shipping percentage][invalidalert=Please enter a shipping percentage (percent of total order) without the leading % sign]\">\n");
							writer.Write("</td>\n");
							writer.Write("</tr>\n");
						}
						writer.Write("<tr><td></td><td align=\"left\"><input type=\"submit\" value=\"Update\" name=\"submit\"></td></tr>\n");
						writer.Write("</table>\n");
					}
					dsfixedp.Dispose();

					writer.Write("</form>\n");

					break;
				case 6:
					// use item shipping:
					writer.Write("<p>Set Your shipping costs in each product variant.</p>");
					break;
				case 7:
					// use RT shipping:
					writer.Write("<p align=\"left\">Real Time I/F will be used for rates, based on order weights. Remember to set your AppConfig:RTShipping parameters accordingly! Current settings are shown below:<br><br>");
					IDataReader rss = DB.GetRS("Select * from appconfig  " + DB.GetNoLock() + " where name like " + DB.SQuote("RTShipping%") + " order by Name");
					while(rss.Read())
					{
						writer.Write(DB.RSField(rss,"Name") + "=" + DB.RSField(rss,"ConfigValue") + "<br>");
					}
					writer.Write("</p>");
					rss.Close();
					break;
				case 8:
					// use shipping by weight by zone:
					writer.Write("<hr size=1>");

					writer.Write("<p><b>ACTIVE RATE TABLE FOR: CALCULATE SHIPPING BY ORDER WEIGHT BY ZONE:</b></p>\n");

					writer.Write("<form action=\"shipping.aspx?EditGUID=" + EditGUID + "\" method=\"post\" id=\"ByZoneForm\" name=\"ByZoneForm\" onsubmit=\"return (validateForm(this) && ByZoneForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
					writer.Write("<input type=\"hidden\" name=\"IsSubmitByZone\" value=\"true\">\n");

					dsWeight = DB.GetDS("select * from ShippingZone  " + DB.GetNoLock() + " where deleted=0 order by Name",false,System.DateTime.Now.AddDays(1));
					if(dsWeight.Tables[0].Rows.Count == 0)
					{
						writer.Write("No shipping zones are defined. <a href=\"shippingzones.aspx\">Click here</a> to define your zones");
					}
					else
					{
						writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"1\">\n");
						writer.Write("<tr bgcolor=\"#FFFFDD\"><td colspan=2 align=\"center\"><b>Order Weight (in " + Localization.WeightUnits() + ")</b></td><td colspan=" + dsWeight.Tables[0].Rows.Count.ToString() + " align=\"center\"><b>Shipping Charge By Zone</b></td><td>&nbsp;</td><td>&nbsp;</td></tr>\n");
						writer.Write("<tr>\n");
						writer.Write("<td align=\"center\"><b>Low</b></td>\n");
						writer.Write("<td align=\"center\"><b>High</b></td>\n");
						foreach(DataRow row in dsWeight.Tables[0].Rows)
						{
							writer.Write("<td align=\"center\"><b>" + DB.RowField(row,"Name") + "</b></td>\n");
						}
						writer.Write("<td align=\"center\"><b>Edit</b></td>\n");
						writer.Write("<td align=\"center\"><b>Delete</b></td>\n");
						writer.Write("</tr>\n");

						DataSet ShippingRows = DB.GetDS("select distinct rowguid,lowvalue,highvalue from ShippingByZone  " + DB.GetNoLock() + " order by LowValue",false,System.DateTime.Now.AddDays(1));
						foreach(DataRow shiprow in ShippingRows.Tables[0].Rows)
						{
							bool EditRow = (EditGUID == DB.RowFieldGUID(shiprow,"RowGUID"));
							writer.Write("<tr>\n");
							writer.Write("<td align=\"center\">\n");
							if(EditRow)
							{
								writer.Write("<input maxLength=\"10\" size=\"10\" name=\"Low_" + DB.RowFieldGUID(shiprow,"RowGUID") + "\" value=\"" + Localization.SingleStringForDB(DB.RowFieldSingle(shiprow,"LowValue")) + "\">\n");
								writer.Write("<input type=\"hidden\" name=\"Low_" + DB.RowFieldGUID(shiprow,"RowGUID") + "_vldt\" value=\"[number][blankalert=Please enter starting order amount][invalidalert=Please enter a money value, WITHOUT the dollar sign]\">\n");
							}
							else
							{
								writer.Write(Localization.SingleStringForDB(DB.RowFieldSingle(shiprow,"LowValue")));
							}
							writer.Write("</td>\n");
							writer.Write("<td align=\"center\">\n");
							if(EditRow)
							{
								writer.Write("<input maxLength=\"10\" size=\"10\" name=\"High_" + DB.RowFieldGUID(shiprow,"RowGUID") + "\" value=\"" + Localization.SingleStringForDB(DB.RowFieldSingle(shiprow,"HighValue")) + "\">\n");
								writer.Write("<input type=\"hidden\" name=\"High_" + DB.RowFieldGUID(shiprow,"RowGUID") + "_vldt\" value=\"[number][blankalert=Please enter ending order amount][invalidalert=Please enter a money value, WITHOUT the dollar sign]\">\n");
							}
							else
							{
								writer.Write(Localization.SingleStringForDB(DB.RowFieldSingle(shiprow,"HighValue")));
							}
							writer.Write("</td>\n");
							foreach(DataRow row in dsWeight.Tables[0].Rows)
							{
								writer.Write("<td align=\"center\">\n");
								if(EditRow)
								{
									writer.Write("<input maxLength=\"10\" size=\"10\" name=\"Rate_" + DB.RowFieldGUID(shiprow,"RowGUID") + "_" + DB.RowFieldInt(row,"ShippingZoneID").ToString() + "\" value=\"" + Localization.CurrencyStringForDB(Common.GetShipByZoneCharge(DB.RowFieldInt(row,"ShippingZoneID"),DB.RowFieldGUID(shiprow,"RowGUID"))) + "\">\n");
									writer.Write("<input type=\"hidden\" name=\"Rate_" + DB.RowFieldGUID(shiprow,"RowGUID") + "_" + DB.RowFieldInt(row,"ShippingZoneID").ToString() + "_vldt\" value=\"[number][blankalert=Please enter the shipping cost][invalidalert=Please enter a money value, WITHOUT the dollar sign]\">\n");
								}
								else
								{
									writer.Write(Localization.CurrencyStringForDB(Common.GetShipByZoneCharge(DB.RowFieldInt(row,"ShippingZoneID"),DB.RowFieldGUID(shiprow,"RowGUID"))));
								}
								writer.Write("</td>\n");
							}
							if(EditRow)
							{
								writer.Write("<td align=\"center\">");
								writer.Write("<input type=\"submit\" value=\"Update\" name=\"submit\">\n");
								writer.Write("</td>");
							}
							else
							{
								writer.Write("<td align=\"center\"><input type=\"Button\" name=\"Edit\" value=\"Edit\" onClick=\"self.location='shipping.aspx?EditGUID=" + DB.RowFieldGUID(shiprow,"RowGUID") + "'\"></td>\n");
							}
							writer.Write("<td align=\"center\"><input type=\"Button\" name=\"Delete\" value=\"Delete\" onClick=\"self.location='shipping.aspx?deleteByZoneid=" + DB.RowFieldGUID(shiprow,"RowGUID") + "'\"></td>\n");
							writer.Write("</tr>\n");
						}
						// add new row:
						writer.Write("<tr>\n");
						writer.Write("<td align=\"center\">\n");
						writer.Write("<input maxLength=\"10\" size=\"10\" name=\"Low_0\" \">\n");
						writer.Write("<input type=\"hidden\" name=\"Low_0_vldt\" value=\"[number][blankalert=Please enter starting order amount][invalidalert=Please enter a money value, WITHOUT the dollar sign]\">\n");
						writer.Write("</td>\n");
						writer.Write("<td align=\"center\">\n");
						writer.Write("<input maxLength=\"10\" size=\"10\" name=\"High_0\" >\n");
						writer.Write("<input type=\"hidden\" name=\"High_0_vldt\" value=\"[number][blankalert=Please enter ending order amount][invalidalert=Please enter a money value, WITHOUT the dollar sign]\">\n");
						writer.Write("</td>\n");
						foreach(DataRow row in dsWeight.Tables[0].Rows)
						{
							writer.Write("<td align=\"center\">\n");
							writer.Write("<input maxLength=\"10\" size=\"10\" name=\"Rate_0_" + DB.RowFieldInt(row,"ShippingZoneID").ToString() + "\">\n");
							writer.Write("<input type=\"hidden\" name=\"Rate_0_" + DB.RowFieldInt(row,"ShippingZoneID").ToString() + "_vldt\" value=\"[number][blankalert=Please enter the shipping cost][invalidalert=Please enter a money value, WITHOUT the dollar sign]\">\n");
							writer.Write("</td>\n");
						}
						writer.Write("<td align=\"center\">");
						writer.Write("<input type=\"submit\" value=\"Add New Row\" name=\"submit\">\n");
						writer.Write("</td>\n");
						writer.Write("<td>&nbsp;</td>");
						writer.Write("</tr>\n");
						writer.Write("</table>\n");
					}
					dsWeight.Dispose();

					writer.Write("</form>\n");
					break;
			}
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
