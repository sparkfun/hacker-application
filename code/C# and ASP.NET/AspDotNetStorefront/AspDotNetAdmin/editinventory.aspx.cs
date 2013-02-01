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
	/// Summary description for editinventory
	/// </summary>
	public class editinventory : SkinBase
	{
		
		int ProductID;
		int VariantID;
		bool ProductTracksInventoryBySize;
		bool ProductTracksInventoryByColor;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			ProductID = Common.QueryStringUSInt("ProductID");
			VariantID = Common.QueryStringUSInt("VariantID");
			if(VariantID == 0)
			{
				VariantID = Common.GetFirstProductVariant(ProductID);
			}
			if(VariantID == 0)
			{
				Response.Redirect("default.aspx"); // should never get here, but...
			}
			if(ProductID == 0)
			{
				ProductID = Common.GetVariantProductID(ProductID);
			}
			
			ProductTracksInventoryBySize = Common.ProductTracksInventoryBySize(ProductID);
			ProductTracksInventoryByColor = Common.ProductTracksInventoryByColor(ProductID);
			
			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				if(ErrorMsg.Length == 0)
				{
					try
					{

						DB.ExecuteSQL("delete from Inventory  where VariantID=" + VariantID.ToString());
						for(int i = 0; i<=Request.Form.Count-1; i++)
						{
							String FieldName = Request.Form.Keys[i];
							if(FieldName.IndexOf("Key_") != -1)
							{
								String KeyVal = Common.Form(FieldName);
								// this field should be processed
								String[] KeyValSplit = KeyVal.Split('|');
								int TheFieldID = Localization.ParseUSInt(KeyValSplit[0]);
								int TheProductID = Localization.ParseUSInt(KeyValSplit[1]);
								int TheVariantID = Localization.ParseUSInt(KeyValSplit[2]);
								String Size = Common.CleanSizeColorOption(KeyValSplit[3]);
								String Color = Common.CleanSizeColorOption(KeyValSplit[4]);
								Common.SetInventory(TheVariantID,Size,Color,Common.FormUSInt("Field_" + TheFieldID.ToString()),true);
							}
						}
						DB.ExecuteSQL("Update Inventory set Quan=0 where Quan<0"); // safety check

						DataUpdated = true;
					}
					catch(Exception ex)
					{
						ErrorMsg = "<p><b>ERROR: " + Common.GetExceptionDetail(ex,"<br>") + "<br><br></b></p>";
					}
				}

			}
			SectionTitle = "<a href=\"editvariant.aspx?productid=" + ProductID.ToString() + "&variantid=" + VariantID.ToString() + "\">Back To Variant</a> - Manage Inventory" + Common.IIF(DataUpdated , " (Updated)" , "");
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			IDataReader rs = DB.GetRS("select * from productvariant  " + DB.GetNoLock() + " where VariantID=" + VariantID.ToString());
			if(!rs.Read())
			{
				rs.Close();
				Response.Redirect("default.aspx"); // should not happen, but...
			}
			
			if(ErrorMsg.Length != 0)
			{
				writer.Write("<p align=\"left\"><b><font color=red>" + ErrorMsg + "</font></b></p>\n");
			}

			if(ErrorMsg.Length == 0)
			{

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function InventoryForm_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("submitonce(theForm);\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				String ProductName = Common.GetProductName(ProductID);
				String ProductSKU = Common.GetProductSKU(ProductID);
				String VariantName = Common.GetVariantName(VariantID);
				String VariantSKU = Common.GetVariantSKUSuffix(VariantID);

				String Sizes = DB.RSField(rs,"Sizes");
				String Colors = DB.RSField(rs,"Colors");

				if(!ProductTracksInventoryBySize)
				{
					Sizes = String.Empty;
				}
				if(!ProductTracksInventoryByColor)
				{
					Colors = String.Empty;
				}

				String[] ColorsSplit = Colors.Split(',');
				String[] SizesSplit = Sizes.Split(',');
				
				writer.Write("<p align=\"left\">Please enter the following inventory data for this product variant.</p>\n");

				writer.Write("<p align=\"left\"><b>PRODUCT: <a href=\"editproduct.aspx?productid=" + ProductID.ToString() + "\">" + ProductName + " (ProductID=" + ProductID.ToString() + ")</a><br>VARIANT: <a href=\"editvariant.aspx?productid=" + ProductID.ToString() + "&variantID=" + VariantID.ToString() + "\">" + VariantID.ToString() + " (VariantID=" + VariantID.ToString() + ")</a></b></p>");

				writer.Write("<div align=\"left\">");
				writer.Write("<form action=\"editinventory.aspx?productid=" + ProductID.ToString() + "&VariantID=" + VariantID.ToString() + "\" method=\"post\" id=\"InventoryForm\" name=\"InventoryForm\" onsubmit=\"return (validateForm(this) && InventoryForm_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
				writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");

				writer.Write("<table border=\"0\" cellspacing=\"0\">\n");
				writer.Write("<tr>\n");
				writer.Write("<td valign=\"middle\" align=\"right\"><b>Inventory</b></td>\n");
				for(int i = SizesSplit.GetLowerBound(0); i <= SizesSplit.GetUpperBound(0); i++)
				{
					writer.Write("<td valign=\"middle\" align=\"center\"><b>" + Common.CleanSizeColorOption(SizesSplit[i]) + "</b></td>\n");
				}
				writer.Write("</tr>\n");
				int FormFieldID = 1000; // arbitrary number
				for(int i = ColorsSplit.GetLowerBound(0); i <= ColorsSplit.GetUpperBound(0); i++)
				{
					writer.Write("<tr>\n");
					writer.Write("<td valign=\"middle\" align=\"right\"><b>" + Common.CleanSizeColorOption(ColorsSplit[i]) + "</b></td>\n");
					for(int j = SizesSplit.GetLowerBound(0); j <= SizesSplit.GetUpperBound(0); j++)
					{
						writer.Write("<td valign=\"middle\" align=\"center\">");
						writer.Write("<input type=\"text\" name=\"Field_" + FormFieldID.ToString() + "\" size=\"8\" value=\"" + Common.GetInventory(ProductID,VariantID,Common.CleanSizeColorOption(SizesSplit[j]),Common.CleanSizeColorOption(ColorsSplit[i])).ToString() + "\">");
						writer.Write("<input type=\"hidden\" name=\"Field_" + FormFieldID.ToString() + "_vldt\" value=\"[number][invalidalert=Please enter a number, without any commas, e.g. 100]\">");
						writer.Write("<input type=\"hidden\" name=\"Key_" + FormFieldID.ToString() + "\" value=\"" + FormFieldID.ToString() + "|" + ProductID.ToString() + "|" + VariantID.ToString() + "|" + Common.CleanSizeColorOption(SizesSplit[j]) + "|" + Common.CleanSizeColorOption(ColorsSplit[i]) + "\">");
						FormFieldID++;
						writer.Write("</td>\n");
					}
					writer.Write("</tr>\n");
				}

				writer.Write("</table>\n");
				writer.Write("<p align=\"left\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"submit\" value=\"Update\" name=\"submit\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"reset\" value=\"Reset\" name=\"reset\"></p>\n");
				writer.Write("</form>\n");
				writer.Write("</div>");
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
