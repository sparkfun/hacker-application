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
using System.Web;
using System.IO;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for editmanufacturers
	/// </summary>
	public class editmanufacturers : SkinBase
	{
		
		int ManufacturerID;
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			ManufacturerID = 0;
			
			if(Common.QueryString("ManufacturerID").Length != 0 && Common.QueryString("ManufacturerID") != "0") 
			{
				Editing = true;
				ManufacturerID = Localization.ParseUSInt(Common.QueryString("ManufacturerID"));
			} 
			else 
			{
				Editing = false;
			}
			
			IDataReader rs;
			
			int N = 0;
			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{

				if(Editing)
				{
					// see if this manufacturer already exists:
					rs = DB.GetRS("select count(name) as N from manufacturer  " + DB.GetNoLock() + " where ManufacturerID<>" + ManufacturerID.ToString() + " and deleted=0 and lower(Name)=" + DB.SQuote(Common.Form("Name").ToLower()));
					rs.Read();
					N = DB.RSFieldInt(rs,"N");
					rs.Close();
					if(N != 0)
					{
						ErrorMsg = "<p><b><font color=red>ERROR:<br><br></font><blockquote>There is already another manufacturer with that name. Please <a href=\"javascript:history.back(-1);\">go back</a>.</b></blockquote></p>";
					}
				}
				else
				{
					// see if this name is already there:
					rs = DB.GetRS("select count(name) as N from Manufacturer  " + DB.GetNoLock() + " where deleted=0 and lower(Name)=" + DB.SQuote(Common.Form("Name").ToLower()));
					rs.Read();
					N = DB.RSFieldInt(rs,"N");
					rs.Close();
					if(N != 0)
					{
						ErrorMsg = "<p><b><font color=red>ERROR:<br><br></font><blockquote>There is already another manufacturer with that name. Please <a href=\"javascript:history.back(-1);\">go back</a>.</b></blockquote></p>";
					}
				}

				if(ErrorMsg.Length == 0)
				{
					try
					{
						StringBuilder sql = new StringBuilder(2500);
						if(!Editing)
						{
							// ok to add them:
							String NewGUID = DB.GetNewGUID();
							sql.Append("insert into manufacturer(ManufacturerGUID,Name,Summary,Description,ColWidth,ManufacturerDisplayFormatID,Address1,Address2,Suite,City,State,ZipCode,Country,Phone,FAX,URL,EMail,LastUpdatedBy) values(");
							sql.Append(DB.SQuote(NewGUID) + ",");
							sql.Append(DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							if(Common.Form("Summary").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("Summary")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("Description").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("Description")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							sql.Append(Common.IIF(Common.Form("ColWidth").Length == 0 , Common.AppConfig("Admin_DefaultManufacturerColWidth") , Common.Form("ColWidth")) + ",");
							sql.Append(Common.Form("ManufacturerDisplayFormatID") + ",");
							if(Common.Form("Address1").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("Address1")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("Address2").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("Address2")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("Suite").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("Suite")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("City").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("City")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("State").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("State")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("ZipCode").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("ZipCode")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("Country").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Form("Country")) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("Phone").Length != 0)
							{
								sql.Append(DB.SQuote(Common.MakeProperPhoneFormat(Common.Form("Phone"))) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("FAX").Length != 0)
							{
								sql.Append(DB.SQuote(Common.MakeProperPhoneFormat(Common.Form("FAX"))) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("URL").Length != 0)
							{
								String theUrl = Common.Left(Common.Form("URL"),80);
								if(theUrl.IndexOf("http://") == -1 && theUrl.Length != 0)
								{
									theUrl = "http://" + theUrl;
								}
								if(theUrl.Length == 0)
								{
									sql.Append("NULL,");
								}
								else
								{
									sql.Append(DB.SQuote(theUrl) + ",");
								}
							}
							else
							{
								sql.Append("NULL,");
							}
							if(Common.Form("EMail").Length != 0)
							{
								sql.Append(DB.SQuote(Common.Left(Common.Form("EMail"),100)) + ",");
							}
							else
							{
								sql.Append("NULL,");
							}
							sql.Append(thisCustomer._customerID);
							sql.Append(")");
							DB.ExecuteSQL(sql.ToString());

							rs = DB.GetRS("select ManufacturerID from manufacturer  " + DB.GetNoLock() + " where deleted=0 and ManufacturerGUID=" + DB.SQuote(NewGUID));
							rs.Read();
							ManufacturerID = DB.RSFieldInt(rs,"ManufacturerID");
							Editing = true;
							rs.Close();
							DataUpdated = true;
						}
						else
						{
							// ok to update:
							sql.Append("update manufacturer set ");
							sql.Append("Name=" + DB.SQuote(Common.Left(Common.Form("Name"),100)) + ",");
							if(Common.Form("Summary").Length != 0)
							{
								sql.Append("Summary=" + DB.SQuote(Common.Form("Summary")) + ",");
							}
							else
							{
								sql.Append("Summary=NULL,");
							}
							if(Common.Form("Description").Length != 0)
							{
								sql.Append("Description=" + DB.SQuote(Common.Form("Description")) + ",");
							}
							else
							{
								sql.Append("Description=NULL,");
							}
							sql.Append("ColWidth=" + Common.IIF(Common.Form("ColWidth").Length == 0 , Common.AppConfig("Admin_DefaultManufacturerColWidth") , Common.Form("ColWidth")) + ",");
							sql.Append("ManufacturerDisplayFormatID=" + Common.Form("ManufacturerDisplayFormatID") + ",");
							if(Common.Form("Address1").Length != 0)
							{
								sql.Append("Address1=" + DB.SQuote(Common.Form("Address1")) + ",");
							}
							else
							{
								sql.Append("Address1=NULL,");
							}
							if(Common.Form("Address2").Length != 0)
							{
								sql.Append("Address2=" + DB.SQuote(Common.Form("Address2")) + ",");
							}
							else
							{
								sql.Append("Address2=NULL,");
							}
							if(Common.Form("Suite").Length != 0)
							{
								sql.Append("Suite=" + DB.SQuote(Common.Form("Suite")) + ",");
							}
							else
							{
								sql.Append("Suite=NULL,");
							}
							if(Common.Form("City").Length != 0)
							{
								sql.Append("City=" + DB.SQuote(Common.Form("City")) + ",");
							}
							else
							{
								sql.Append("City=NULL,");
							}
							if(Common.Form("State").Length != 0)
							{
								sql.Append("State=" + DB.SQuote(Common.Form("State")) + ",");
							}
							else
							{
								sql.Append("State=NULL,");
							}
							if(Common.Form("ZipCode").Length != 0)
							{
								sql.Append("ZipCode=" + DB.SQuote(Common.Form("ZipCode")) + ",");
							}
							else
							{
								sql.Append("ZipCode=NULL,");
							}
							if(Common.Form("Country").Length != 0)
							{
								sql.Append("Country=" + DB.SQuote(Common.Form("Country")) + ",");
							}
							else
							{
								sql.Append("Country=NULL,");
							}
							if(Common.Form("Phone").Length != 0)
							{
								sql.Append("Phone=" + DB.SQuote(Common.MakeProperPhoneFormat(Common.Form("Phone"))) + ",");
							}
							else
							{
								sql.Append("Phone=NULL,");
							}
							if(Common.Form("FAX").Length != 0)
							{
								sql.Append("FAX=" + DB.SQuote(Common.MakeProperPhoneFormat(Common.Form("FAX"))) + ",");
							}
							else
							{
								sql.Append("FAX=NULL,");
							}
							if(Common.Form("URL").Length != 0)
							{
								String theUrl2 = Common.Left(Common.Form("URL"),80);
								if(theUrl2.IndexOf("http://") == -1 && theUrl2.Length != 0)
								{
									theUrl2 = "http://" + theUrl2;
								}
								if(theUrl2.Length != 0)
								{
									sql.Append("URL=" + DB.SQuote(theUrl2) + ",");
								}
								else
								{
									sql.Append("URL=NULL,");
								}
							}
							else
							{
								sql.Append("URL=NULL,");
							}
							if(Common.Form("Email").Length != 0)
							{
								sql.Append("Email=" + DB.SQuote(Common.Left(Common.Form("EMail"),100)) + ",");
							}
							else
							{
								sql.Append("Email=NULL,");
							}
							sql.Append("LastUpdated=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
							sql.Append("LastUpdatedBy=" + thisCustomer._customerID.ToString() + " ");
							sql.Append("where ManufacturerID=" + ManufacturerID.ToString());
							DB.ExecuteSQL(sql.ToString());
							DataUpdated = true;
							Editing = true;
						}
					}
					catch(Exception ex)
					{
						ErrorMsg = "<p><b>ERROR: " + Common.GetExceptionDetail(ex,"<br>") + "<br><br></b></p>";
					}

					// handle image uploaded:
					try
					{
						String Image1 = String.Empty;
						HttpPostedFile Image1File = Request.Files["Image1"];
						if(Image1File.ContentLength != 0)
						{
							// delete any current image file first
							try
							{
								System.IO.File.Delete(Common.GetImagePath("Manufacturer","",true) + ManufacturerID.ToString() + ".jpg");
								System.IO.File.Delete(Common.GetImagePath("Manufacturer","",true) + ManufacturerID.ToString() + ".gif");
								System.IO.File.Delete(Common.GetImagePath("Manufacturer","",true) + ManufacturerID.ToString() + ".png");
							}
							catch
							{}

							String s = Image1File.ContentType;
							switch(Image1File.ContentType)
							{
								case "image/gif":
									Image1 = Common.GetImagePath("Manufacturer","",true) + ManufacturerID.ToString() + ".gif";
									Image1File.SaveAs(Image1);
									break;
								case "image/x-png":
									Image1 = Common.GetImagePath("Manufacturer","",true) + ManufacturerID.ToString() + ".png";
									Image1File.SaveAs(Image1);
									break;
								case "image/jpeg":
								case "image/pjpeg":
									Image1 = Common.GetImagePath("Manufacturer","",true) + ManufacturerID.ToString() + ".jpg";
									Image1File.SaveAs(Image1);
									break;
							}
						}
					}
					catch(Exception ex)
					{
						ErrorMsg = Common.GetExceptionDetail(ex,"<br>");
					}
				}

			}
			SectionTitle = "<a href=\"manufacturers.aspx\">Manufacturers</a> - Manage Manufacturers (Brands)" + Common.IIF(DataUpdated , " (Updated)" , "");
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			IDataReader rs = DB.GetRS("select * from Manufacturer  " + DB.GetNoLock() + " where ManufacturerID=" + ManufacturerID.ToString());
			if(rs.Read())
			{
				Editing = true;
			}
			
			if(ErrorMsg.Length != 0)
			{
				writer.Write("<p><b><font color=red>" + ErrorMsg + "</font></b></p>\n");
			}


			if(ErrorMsg.Length == 0)
			{

				if(Editing)
				{
					writer.Write("<b>Editing Manufacturer: " + DB.RSField(rs,"Name") + " (ID=" + DB.RSFieldInt(rs,"ManufacturerID").ToString() + ")<br><br></b>\n");
				}
				else
				{
					writer.Write("<b>Adding New Manufacturer:<br><br></b>\n");
				}

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function Form_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("submitonce(theForm);\n");
				//			writer.Write("  if (theForm.State.selectedIndex < 1)\n");
				//			writer.Write("  {\n");
				//			writer.Write("    	alert(\"Please select the state.\");\n");
				//			writer.Write("    theForm.State.focus();\n");
				//			writer.Write("    	return (false);\n");
				//			writer.Write("  }\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				writer.Write("<p>Please enter the following information about this manufacturer (brand). Fields marked with an asterisk (*) are required. All other fields are optional.</p>\n");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\">\n");
				writer.Write("<form enctype=\"multipart/form-data\" action=\"editmanufacturer.aspx?ManufacturerID=" + ManufacturerID.ToString() + "&edit=" + Editing.ToString() + "\" method=\"post\" id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
				writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"100%\" colspan=\"2\" align=\"left\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("<tr>\n");
				writer.Write("<td></td><td align=\"left\"><br>\n");
				if(Editing) 
				{
					writer.Write("<input type=\"submit\" value=\"Update\" name=\"submit\">\n");
					writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"reset\" class=\"CPButton\" value=\"Reset\" name=\"reset\">\n");
				} 
				else 
				{
					writer.Write("<input type=\"submit\" value=\"Add New\" name=\"submit\">\n");
				}
				writer.Write("        </td>\n");
				writer.Write("      </tr>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td width=\"25%\" align=\"right\" valign=\"middle\">*Manufacturer Name:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"Name\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Name")) , "") + "\">\n");
				writer.Write("                	<input type=\"hidden\" name=\"Name_vldt\" value=\"[req][blankalert=Please enter the manufacturer name]\">\n");
				writer.Write("                	</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">*Display Format:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("<select size=\"1\" name=\"ManufacturerDisplayFormatID\">\n");
				IDataReader rsst = DB.GetRS("select * from ManufacturerDisplayFormat  " + DB.GetNoLock() + " where deleted=0 order by name");
				while(rsst.Read())
				{
					writer.Write("<option value=\"" + DB.RSFieldInt(rsst,"ManufacturerDisplayFormatID").ToString() + "\"");
					if(Editing) 
					{
						if(DB.RSFieldInt(rs,"ManufacturerDisplayFormatID") == DB.RSFieldInt(rsst,"ManufacturerDisplayFormatID"))
						{
							writer.Write(" selected");
						}
					}
					else
					{
						if(DB.RSFieldInt(rsst,"ManufacturerDisplayFormatID") == Common.AppConfigUSInt("Admin_DefaultManufacturerDisplayFormatID"))
						{
							writer.Write(" selected");
						}
					}
					writer.Write(">" + DB.RSField(rsst,"Name") + "</option>");
				}
				rsst.Close();
				writer.Write("</select>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">Grid Column Width:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"2\" size=\"2\" name=\"ColWidth\" value=\"" + Common.IIF(Editing , DB.RSFieldInt(rs,"ColWidth").ToString() , "4") + "\">\n");
				writer.Write("                </td>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">Street Address:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"Address1\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Address1")) , "") + "\">\n");
				//writer.Write("                    <input type=\"hidden\" name=\"Address1_vldt\" value=\"[req][blankalert=Please enter a street address]\">\n");
				writer.Write("                	&nbsp;&nbsp;\n");
				writer.Write("                	Apt/Suite#:\n");
				writer.Write("                	<input maxLength=\"100\" size=\"5\" name=\"Suite\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Suite")) , "") + "\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\"></td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"Address2\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Address2")) , "") + "\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">City:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"30\" name=\"City\" value=\"" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"City")) , "") + "\">\n");
				//writer.Write("                    <input type=\"hidden\" name=\"City_vldt\" value=\"[req][blankalert=Please enter a city]\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">State:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">");
				writer.Write("<select size=\"1\" name=\"State\">");
				writer.Write("<OPTION value=\"\"" + Common.IIF(DB.RSField(rs,"State").Length==0 , " selected" , String.Empty) + ">SELECT ONE</option>");
				DataSet dsstate = DB.GetDS("select * from state  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddHours(1));
				foreach(DataRow row in dsstate.Tables[0].Rows)
				{
					writer.Write("<OPTION value=\"" + Server.HtmlEncode(DB.RowField(row,"Abbreviation")) + "\"" + Common.SelectOption(rs,DB.RowField(row,"Abbreviation"),"State") + ">" + Server.HtmlEncode(DB.RowField(row,"Name")) + "</option>");
				}
				dsstate.Dispose();
				writer.Write("</select>");
				writer.Write("			</td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">Zip Code:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"16\" size=\"15\" name=\"ZipCode\" value=\"" + Common.IIF(Editing , DB.RSField(rs,"ZipCode") , "") + "\">\n");
				writer.Write("                    <input type=\"hidden\" name=\"ZipCode_vldt\" value=\"[invalidalert=Please enter a valid zipcode]\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">Country:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("<SELECT NAME=\"Country\" size=\"1\">");
				writer.Write("<option value=\"0\">SELECT ONE</option>");
				DataSet dscountry2 = DB.GetDS("select * from country  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddHours(1));
				foreach(DataRow row in dscountry2.Tables[0].Rows)
				{
					writer.Write("<OPTION value=\"" + DB.RowField(row,"Name") + "\"" + Common.IIF(DB.RSField(rs,"Country") == DB.RowField(row,"Name"), " selected ","") + ">" + DB.RowField(row,"Name") + "</option>");
				}
				dscountry2.Dispose();
				writer.Write("</SELECT>");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">Web Site:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"35\" name=\"URL\" value=\"" + Common.IIF(Editing , DB.RSField(rs,"URL") , "") + "\">&nbsp;&nbsp;<small>(e.g. http://abcd.com)</small>\n");
				//writer.Write("                    <input type=\"hidden\" name=\"URL_vldt\" value=\"[req][email][blankalert=Please enter the manufacturer's Web site address]\">\n");
				writer.Write("               	</td>\n");
				writer.Write("              </tr>\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">E-Mail Address:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"100\" size=\"35\" name=\"EMail\" value=\"" + Common.IIF(Editing , DB.RSField(rs,"EMail") , Common.QueryString("Email")) + "\">\n");
				writer.Write("                    <input type=\"hidden\" name=\"EMail_vldt\" value=\"[invalidalert=Please enter a valid e-mail address]\">\n");
				writer.Write("               	</td>\n");
				writer.Write("              </tr>\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">Phone:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"35\" size=\"35\" name=\"Phone\" value=\"" + Common.IIF(Editing , Common.GetPhoneDisplayFormat(DB.RSField(rs,"Phone")) , "") + "\">&nbsp;&nbsp;<small>(optional, including area code)</small>\n");
				writer.Write("                    <input type=\"hidden\" name=\"Phone_vldt\" value=\"[invalidalert=Please enter a valid phone number with areacode, e.g. (480) 555-1212]\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"middle\">Fax:&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<input maxLength=\"35\" size=\"35\" name=\"FAX\" value=\"" + Common.IIF(Editing , Common.GetPhoneDisplayFormat(DB.RSField(rs,"Fax")) , "") + "\">&nbsp;&nbsp;<small>(optional, including area code)</small>\n");
				writer.Write("                    <input type=\"hidden\" name=\"FAX_vldt\" value=\"[invalidalert=Please enter a valid FAX number with areacode, e.g. (480) 555-1212]\">\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				writer.Write("  <tr>\n");
				writer.Write("    <td valign=\"top\" align=\"right\">Icon:\n");
				writer.Write("</td>\n");
				writer.Write("    <td valign=\"top\" align=\"left\">");
				writer.Write("    <input type=\"file\" name=\"Image1\" size=\"30\" value=\"\">\n");
				String Image1URL = Common.LookupImage("Manufacturer",ManufacturerID,"",_siteID);
				if(Image1URL.Length != 0)
				{
					writer.Write("<a href=\"javascript:void(0);\" onClick=\"DeleteIcon();\">Click here</a> to delete the current image<br>\n");
					writer.Write("<br><img id=\"MfgPic\" name=\"MfgPic\" border=\"0\" src=\"" + Image1URL + "\">\n");
				}
				writer.Write("</td>\n");
				writer.Write(" </tr>\n");

				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Summary (HTML OK):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<textarea style=\"height: 30em; width: 100%;\" cols=\"" + Common.AppConfig("Admin_TextareaWidth") + "\" rows=\"" + Common.AppConfig("Admin_TextareaHeightSmall") + "\" id=\"Summary\" name=\"Summary\">" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Summary")) , "") + "</textarea>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("              <tr valign=\"middle\">\n");
				writer.Write("                <td align=\"right\" valign=\"top\">Description (HTML OK):&nbsp;&nbsp;</td>\n");
				writer.Write("                <td align=\"left\">\n");
				writer.Write("                	<textarea style=\"height: 60em; width: 100%;\" cols=\"" + Common.AppConfig("Admin_TextareaWidth") + "\" rows=\"" + Common.AppConfig("Admin_TextareaHeight") + "\" id=\"Description\" name=\"Description\">" + Common.IIF(Editing , Server.HtmlEncode(DB.RSField(rs,"Description")) , "") + "</textarea>\n");
				writer.Write("                </td>\n");
				writer.Write("              </tr>\n");
				
				writer.Write("<tr>\n");
				writer.Write("<td></td><td align=\"left\"><br>\n");
				if(Editing) 
				{
					writer.Write("<input type=\"submit\" value=\"Update\" name=\"submit\">\n");
					writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"reset\" class=\"CPButton\" value=\"Reset\" name=\"reset\">\n");
				} 
				else 
				{
					writer.Write("<input type=\"submit\" value=\"Add New\" name=\"submit\">\n");
				}
				writer.Write("        </td>\n");
				writer.Write("      </tr>\n");
				writer.Write("</form>\n");
				writer.Write("  </table>\n");

				writer.Write(Common.GenerateHtmlEditor("Summary"));
				writer.Write(Common.GenerateHtmlEditor("Description"));

				writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
				writer.Write("function DeleteIcon()\n");
				writer.Write("{\n");
				writer.Write("window.open('deleteicon.aspx?ManufacturerID=" + ManufacturerID.ToString() + "&FormImageName=MfgPic',\"AspDotNetStorefrontAdmin_ML\",\"height=250,width=440,top=10,left=20,status=no,toolbar=no,menubar=no,scrollbars=yes,location=no\")\n");
				writer.Write("}\n");
				writer.Write("</SCRIPT>\n");

			}
			rs.Close();
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
