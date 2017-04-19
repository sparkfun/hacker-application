// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for showmanufacturer.
	/// </summary>
	public class showmanufacturer : SkinBase
	{

		int ManufacturerID;
		String ManufacturerName;
		String ManufacturerDescription;
		String ManufacturerPicture;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad();
			ManufacturerID = Common.QueryStringUSInt("ManufacturerID");
			
			String SEName = Common.QueryString("SEName");
			if(ManufacturerID == 0 && SEName.Length != 0)
			{
				// mapping from static url, try to find mfg id:
				ManufacturerID = SE.LookupSEManufacturer(SEName);
				if(ManufacturerID == 0)
				{
					// no match:
					Response.Redirect("default.aspx");
				}
			}
			if(ManufacturerID == 0)
			{
				Response.Redirect("default.aspx");
			}
			IDataReader rs = DB.GetRS("select * from manufacturer  " + DB.GetNoLock() + " where deleted=0 and ManufacturerID=" + ManufacturerID.ToString());
			if(rs.Read())
			{
				ManufacturerName = DB.RSField(rs,"Name");
				ManufacturerDescription = DB.RSField(rs,"Description");
				ManufacturerPicture = Common.LookupImage("Manufacturer",ManufacturerID,"",_siteID);
			}
			rs.Close();

			Common.LogEvent(thisCustomer._customerID,26,ManufacturerID.ToString());
			SectionTitle = "<a href=\"manufacturers.aspx\">Manufacturers List</a> - " + ManufacturerName;
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			String sql = "select product.*, manufacturer.manufacturerdisplayformatid, manufacturer.colwidth from product " + DB.GetNoLock() + " inner join manufacturer " + DB.GetNoLock() + " on product.manufacturerid=manufacturer.manufacturerid where manufacturer.manufacturerid=" + ManufacturerID.ToString() + " and product.deleted=0 and product.published=1 and (product.SKU IS NULL or product.sku<>" + DB.SQuote("MICROPAY") + ")";
			DataSet ds = DB.GetDS(sql,true,System.DateTime.Now.AddHours(1));
			if(Common.AppConfigBool("ForceManufacturerHeaderDisplay") || ManufacturerDescription.Length != 0)
			{
				writer.Write("<p align=\"left\">\n");
				int MaxWidth = Common.AppConfigNativeInt("MaxIconWidth");
				if(MaxWidth == 0)
				{
					MaxWidth = 125;
				}
				int MaxHeight = Common.AppConfigNativeInt("MaxIconHeight");
				if(MaxHeight == 0)
				{
					MaxHeight = 125;
				}
				if(ManufacturerPicture.Length != 0)
				{
					int w = Common.GetImageWidth(ManufacturerPicture);
					int h = Common.GetImageHeight(ManufacturerPicture);
					if(w > MaxWidth)
					{
						writer.Write("<img align=\"left\" src=\"" + ManufacturerPicture + "\" width=\"" + MaxWidth.ToString() + "\" border=\"0\">");
					}
					else if(h > MaxHeight)
					{
						writer.Write("<img align=\"left\" src=\"" + ManufacturerPicture + "\" height=\"" + MaxHeight.ToString() + "\" border=\"0\">");
					}
					else
					{
						writer.Write("<img align=\"left\" src=\"" + ManufacturerPicture + "\" border=\"0\">");
					}
				}
				writer.Write("<b>" + ManufacturerName + "</b>");
				if(ManufacturerDescription.Length != 0)
				{
					writer.Write(": " + ManufacturerDescription);
				}
				writer.Write("</p>\n");
			}

			bool empty = ds.Tables[0].Rows.Count == 0;

			if(!empty)
			{
				switch(DB.RowFieldInt(ds.Tables[0].Rows[0],"ManufacturerDisplayFormatID"))
				{
					case 1:
						// GRID FORMAT:
						int ItemNumber = 1;
						int ItemsPerRow = Common.AppConfigUSInt("DefaultManufacturerColWidth");
						if(!empty)	
						{
							int tmpItemsPerRow = DB.RowFieldInt(ds.Tables[0].Rows[0],"ColWidth");
							if (tmpItemsPerRow != 0)
							{
								ItemsPerRow = tmpItemsPerRow;
							}
						}
						writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"4\" width=\"100%\">");
						if(empty)
						{
							Topic t = new Topic("EmptyManufacturerText",thisCustomer._localeSetting,_siteID);
							writer.Write(t._contents);
						}
						else
						{
							foreach(DataRow row in ds.Tables[0].Rows)
							{
								if(ItemNumber == 1)
								{
									writer.Write("<tr>");
								}
								if(ItemNumber == ItemsPerRow+1)
								{
									writer.Write("</tr><tr><td colspan=\"" + ItemsPerRow.ToString() + "\" height=\"8\"></td></tr>");
									ItemNumber=1;
								}
								writer.Write("<td width=\"" + ((int)(100/ItemsPerRow)).ToString() + "%\" height=\"70\" align=\"center\" valign=\"top\">");
								String ImgUrl = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",_siteID);
								if(ImgUrl.Length == 0)
								{
									ImgUrl = Common.AppConfig("NoPicture");
								}
								if(ImgUrl.Length != 0)
								{
									//										Single w = (Single)Common.GetImageWidth(ImgUrl);
									//										Single h = (Single)Common.GetImageHeight(ImgUrl);
									//										Single finalW = w;
									//										Single finalH = h;
									//										if(w > 250)
									//										{
									//											finalH = h * 250/w;
									//											finalW = 250;
									//										}
									//										if(finalH > 250)
									//										{
									//											finalW = finalW * 250/finalH;
									//											finalH = 250;
									//										}
									writer.Write("<img style=\"cursor: hand;\" onClick=\"self.location='" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName")) + "';\" style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + ImgUrl + "\">");
									writer.Write("<br><br>");
								}
								writer.Write("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),String.Empty) + "\">");
								writer.Write(DB.RowField(row,"Name") + "</a>");
								writer.Write("</td>");
								ItemNumber++;
							}
							for(int i = ItemNumber; i<=ItemsPerRow; i++)
							{
								writer.Write("<td>&nbsp;</td>");
							}
							writer.Write("</tr>");
						}
						writer.Write("</table>");
						break;
			
					case 2:
						// TABLE - EXPANDED FORMAT:
						foreach(DataRow row in ds.Tables[0].Rows)
						{
							WriteProductBar(writer,row,DB.RowFieldInt(row,"ProductID"),0,_siteID,thisCustomer._localeSetting);
						}
						break;
					case 3:
						// TABLE - CONDENSED FORMAT:
						writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
						writer.Write("    <tr class=\"DarkCell\">\n");
						writer.Write("      <td align=\"center\"><font class=\"DarkCellText\"><b>Photo</b></font></td>\n");
						writer.Write("      <td><font class=\"DarkCellText\"><b>Name</b></font></td>\n");
						writer.Write("      <td><font class=\"DarkCellText\"><b>SKU</b></font></td>\n");
						writer.Write("      <td><font class=\"DarkCellText\"><b>More Info</b></font></td>\n");
						writer.Write("    </tr>\n");
						int rowi = 1;
						foreach(DataRow row in ds.Tables[0].Rows)
						{
							writer.Write("<tr " + Common.IIF(rowi % 1 == 0 , "class=\"LightCell\"" , "") + ">\n");

							writer.Write("<td valign=\"middle\" align=\"center\">");

							String Image1URL = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",_siteID);
							if(Image1URL.Length == 0)
							{
								Image1URL = Common.AppConfig("NoPictureIcon");
							}
							int HT = Common.AppConfigUSInt("CondensedTablePictureHeight");
							if(HT == 0)
							{
								HT = 50;
							}

							writer.Write("<img style=\"cursor: hand;\" OnClick=\"self.location='" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName"))  + "';\" style=\"" + Common.AppConfig("ImageFrameStyle") + "\" src=\"" + Image1URL + "\" height=\"" + HT.ToString() + "\" align=\"absmiddle\">");
							writer.Write("</td>");
					
							writer.Write("<td>");
							writer.Write("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),String.Empty) + "\">");
							writer.Write(DB.RowField(row,"Name"));
							writer.Write("</a>");
							writer.Write("</td>\n");
					
							writer.Write("<td>" + DB.RowField(row,"SKU") +  "</td>\n");

							writer.Write("<td>");
							writer.Write("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),String.Empty)  + "\">");
							writer.Write("<img src=\"skins/skin_" + _siteID.ToString() + "/images/moreinfo.gif\" border=\"0\" align=\"absmiddle\">");
							writer.Write("</a>&nbsp;\n");
							writer.Write("</td>\n");
							writer.Write("</tr>\n");
							rowi++;
						}
						writer.Write("</table>");
						break;
				}
			}
			ds.Dispose();
		}

		void WriteProductBar(System.Web.UI.HtmlTextWriter writer, DataRow row, int ProductID, int CategoryID, int SiteID, String LocaleSetting)
		{
			String url = SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),String.Empty);
			writer.Write("			<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" bgcolor=\"#FFFFFF\" >\n");
			writer.Write("			<tr>\n");
			writer.Write("				<td colspan=\"4\" align=\"left\" valign=\"middle\" height=\"20\" class=\"DarkCell\">\n");
			writer.Write("					&nbsp;<img src=\"skins/skin_" + _siteID.ToString() + "/images/whitearrow.gif\" align=\"absmiddle\">&nbsp;<a class=\"DarkCellText\" href=\"" + url + "\"><font style=\"font-size: 15px; font-weight:bold;\">" + DB.RowField(row,"Name") + "</font></a>\n");
			writer.Write("				</td>\n");
			writer.Write("			</tr>\n");
			writer.Write("			<tr>\n");
			writer.Write("				<td width=\"2%\" class=\"GreyCell\"><img src=\"/images/spacer.gif\" width=\"5\" height=\"1\">\n");
			writer.Write("				</td>\n");
			writer.Write("				<td width=\"30%\" align=\"center\" valign=\"top\" class=\"GreyCell\">\n");
			String ImgUrl = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",_siteID);
			if(ImgUrl.Length != 0)
			{
				writer.Write("<br><a href=\"" + url + "\"><img src=\"" + ImgUrl + "\" border=\"0\"></a>\n");
			}
			writer.Write("				</td>\n");
			writer.Write("				<td width=\"8%\" class=\"GreyCell\">\n");
			writer.Write("					<img src=\"/images/spacer.gif\" width=\"30\" height=\"1\">\n");
			writer.Write("				</td>\n");
			writer.Write("				<td width=\"60%\" valign=\"top\" align=\"left\" class=\"GreyCell\">\n");
			writer.Write("					<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" class=\"GreyCell\">\n");
			writer.Write("						<tr>\n");
			writer.Write("							<td width=\"20%\" align=\"left\" valign=\"top\">Description</td>\n");
			writer.Write("							<td width=\"80%\" align=\"left\" valign=\"top\">" + DB.RowField(row,"Description") + "</td>\n");
			writer.Write("						</tr>\n");
			writer.Write("						<tr>\n");
			writer.Write("							<td width=\"20%\" align=\"left\" valign=\"top\">Base SKU</td>\n");
			writer.Write("							<td width=\"80%\" align=\"left\" valign=\"top\">" + DB.RowField(row,"SKU") + "</td>\n");
			writer.Write("						</tr>\n");
			writer.Write("						<tr>\n");
			writer.Write("							<td colspan=\"2\" width=\"100%\" align=\"right\" valign=\"bottom\"><a href=\"" + url + "\"><img src=\"skins/skin_" + _siteID.ToString() + "/images/moreinfo.gif\" border=\"0\"></a></td>\n");
			writer.Write("						</tr>\n");
			writer.Write("					</table>\n");
			writer.Write("				</td>\n");
			writer.Write("			</tr>\n");
			writer.Write("			</table>\n");
			writer.Write("			<br><br>\n");
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
