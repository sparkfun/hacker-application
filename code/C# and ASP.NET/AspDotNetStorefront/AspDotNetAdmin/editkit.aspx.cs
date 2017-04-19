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
	/// Summary description for editkit
	/// </summary>
	public class editkit : SkinBase
	{
		
		int ProductID;
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			ProductID = Common.QueryStringUSInt("ProductID");

			if(Common.QueryString("DeleteGroupID").Length != 0)
			{
				// delete the group, and any items it contains:
				DB.ExecuteSQL("delete from kitcart where kitgroupid=" +  Common.QueryString("DeleteGroupID"));
				DB.ExecuteSQL("delete from kititem where kitgroupid=" +  Common.QueryString("DeleteGroupID"));
				DB.ExecuteSQL("delete from kitgroup where kitgroupid=" +  Common.QueryString("DeleteGroupID") + " and ProductID=" + ProductID.ToString());
			}

			if(Common.QueryString("DeleteItemID").Length != 0)
			{
				// delete the item:
				DB.ExecuteSQL("delete from kitcart where kititemid=" +  Common.QueryString("DeleteItemID"));
				DB.ExecuteSQL("delete from kititem where kititemid=" +  Common.QueryString("DeleteItemID"));
			}

			if(Common.QueryString("DeleteItemID").Length != 0)
			{
				// delete the item:
				DB.ExecuteSQL("delete from kititem where kititemid=" +  Common.QueryString("DeleteItemID"));
			}

			
			
			//int N = 0;
			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				try
				{
					// are we adding a new group:
					if(Common.Form("NewGroupName").Length != 0)
					{
						String NewGUID = DB.GetNewGUID();
						DB.ExecuteSQL("insert into KitGroup(KitGroupGUID,Name,Description,ProductID,DisplayOrder,KitGroupTypeID,IsRequired) values(" + DB.SQuote(NewGUID) + "," + DB.SQuote(Common.Form("NewGroupName")) + "," + DB.SQuote(Common.Form("NewGroupDescription")) + "," + ProductID.ToString() + "," + Common.FormUSInt("NewGroupDisplayOrder").ToString() + "," + Common.FormUSInt("NewGroupType").ToString() + "," + Common.FormUSInt("NewGroupIsRequired").ToString() + ")");
					}

					// add new group items:
					IDataReader rsg = DB.GetRS("select * from KitGroup  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString());
					while(rsg.Read())
					{
						int ThisGroupID = DB.RSFieldInt(rsg,"KitGroupID");
						if(Common.Form("NewItemName_" + DB.RSFieldInt(rsg,"KitGroupID").ToString()).Length != 0)
						{
							int DDDO = Common.FormUSInt("NewItemDisplayOrder_" + ThisGroupID.ToString());
							decimal Price = System.Decimal.Zero;
							if(Common.Form("NewItemPriceDelta_" + ThisGroupID.ToString()).Length != 0)
							{
								try
								{
									Price = Common.FormUSDecimal("NewItemPriceDelta_" + ThisGroupID.ToString());
								}
								catch {}
							}
							String KIGUID = DB.GetNewGUID();
							String sql = "insert into KitItem(KitItemGUID,KitGroupID,Name,Description,PriceDelta,DisplayOrder,IsDefault) values(";
							sql += DB.SQuote(KIGUID) + ",";
							sql += ThisGroupID.ToString() + ",";
							sql += DB.SQuote(Common.Form("NewItemName_" + ThisGroupID.ToString())) + ",";
							sql += DB.SQuote(Common.Form("NewItemDescription_" + ThisGroupID.ToString())) + ",";
							sql += Localization.CurrencyStringForDB(Price) + ",";
							sql += Common.FormUSInt("NewItemDisplayOrder_" + ThisGroupID.ToString()).ToString() + ",";
							sql += Common.FormUSInt("NewItemIsDefault_" + ThisGroupID.ToString()).ToString();
							sql += ")";
							DB.ExecuteSQL(sql);
						}
					}
					rsg.Close();

					// update Groups:
					for(int i = 0; i<=Request.Form.Count-1; i++)
					{
						if(Request.Form.Keys[i].StartsWith("GroupName"))
						{
							int thisID = Localization.ParseUSInt(Request.Form.Keys[i].Split('_')[1]);
							DB.ExecuteSQL("update KitGroup set Name=" + DB.SQuote(Common.Form("GroupName_" + thisID.ToString())) + ",Description=" + DB.SQuote(Common.Form("GroupDescription_" + thisID.ToString())) + ",DisplayOrder=" + Common.FormUSInt("GroupDisplayOrder_" + thisID.ToString()).ToString() + ",KitGroupTypeID=" + Common.Form("GroupType_" + thisID.ToString()) + ",IsRequired=" + Common.Form("GroupIsRequired_" + thisID.ToString()) + " where KitGroupID=" + thisID.ToString());
						}
					}


					// update Items:
					for(int i = 0; i<=Request.Form.Count-1; i++)
					{
						if(Request.Form.Keys[i].StartsWith("ItemName"))
						{
							int thisID = Localization.ParseUSInt(Request.Form.Keys[i].Split('_')[1]);
							decimal Price = System.Decimal.Zero;
							try
							{
								Price = Common.FormUSDecimal("ItemPriceDelta_" + thisID.ToString());
							}
							catch {}
							DB.ExecuteSQL("update KitItem set Name=" + DB.SQuote(Common.Form("ItemName_" + thisID.ToString())) + ",Description=" + DB.SQuote(Common.Form("ItemDescription_" + thisID.ToString())) + ",DisplayOrder=" + Common.FormUSInt("ItemDisplayOrder_" + thisID.ToString()).ToString() + ",IsDefault=" + Common.Form("ItemIsDefault_" + thisID.ToString()) + ",PriceDelta=" + Localization.CurrencyStringForDB(Price) + " where KitItemID=" + thisID.ToString());
						}
					}
				}
				catch(Exception ex)
				{
					ErrorMsg = "<p><b>ERROR: " + Common.GetExceptionDetail(ex,"<br>") + "<br><br></b></p>";
				}
			}
			SectionTitle = "<a href=\"editproduct.aspx?productid=" + ProductID.ToString() + "\">Product</a> - Edit Kit" + Common.IIF(DataUpdated , " (Updated)" , "");
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(ErrorMsg.Length != 0)
			{
				writer.Write("<p align=\"left\"><b><font color=red>" + ErrorMsg + "</font></b></p>\n");
			}

			if(ErrorMsg.Length == 0)
			{

				writer.Write("<p align=\"left\"><b>Within Product: <a href=\"editproduct.aspx?productid=" + ProductID.ToString() + "\">" + Common.GetProductName(ProductID) + "</a> (Product SKU=" + Common.GetProductSKU(ProductID) + ", ProductID=" + ProductID.ToString() + ")</b</p>\n");
				writer.Write("<p align=\"left\"><b>Editing Kit: " + Common.GetProductName(ProductID) + ", ProductID=" + ProductID.ToString() + ")</b></p>\n");

				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function Form_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("submitonce(theForm);\n");
				writer.Write("return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");

				writer.Write("<p align=\"left\">Please enter the following information about this kit. Kits are composed of groups, and groups are composed of items. Each item can have a price delta applied to the base kit (product) price.</p>\n");
				writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\">\n");
				writer.Write("<form enctype=\"multipart/form-data\" action=\"editkit.aspx?productid=" + ProductID.ToString() + "\" method=\"post\" id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
				writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
				writer.Write("<tr valign=\"middle\">\n");
				writer.Write("<td width=\"100%\" colspan=\"2\" align=\"left\">\n");
				writer.Write("</td>\n");
				writer.Write("</tr>\n");
				
				writer.Write("<tr>\n");
				writer.Write("<td></td><td align=\"left\"><br>\n");
				writer.Write("<input type=\"submit\" value=\"Update\" name=\"submit\">\n");
				writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"reset\" value=\"Reset\" name=\"reset\">\n");
				writer.Write("</td>\n");
				writer.Write("</tr>\n");

				IDataReader rs = DB.GetRS("select * from KitGroup  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString() + " order by DisplayOrder,Name");
				int i = 1;
				while(rs.Read())
				{
					writer.Write("<tr><td colspan=\"2\" bgcolor=\"" + Common.IIF(i % 2 == 0 , "#FFFFFF", "#EEEEEE") + "\"><font style=\"font-size: 14px; font-weight: bold\">Group: " + Server.HtmlEncode(DB.RSField(rs,"Name")) + "</font></td></tr>");
					writer.Write("<tr><td colspan=\"2\" bgcolor=\"" + Common.IIF(i % 2 == 0 , "#FFFFFF", "#EEEEEE") + "\">");
					int ThisGroupID = DB.RSFieldInt(rs,"KitGroupID");
					writer.Write("Name: <input maxLength=\"100\" size=\"40\" name=\"GroupName_" + ThisGroupID.ToString() + "\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"Name")) + "\">&nbsp;&nbsp;\n");
					writer.Write("Description: <input maxLength=\"1000\" size=\"40\" name=\"GroupDescription_" + ThisGroupID.ToString() + "\" value=\"" + Server.HtmlEncode(DB.RSField(rs,"Description")) + "\">&nbsp;&nbsp;\n");
					writer.Write("Display Order: <input maxLength=\"3\" size=\"5\" name=\"GroupDisplayOrder_" + ThisGroupID.ToString() + "\" value=\"" + DB.RSFieldInt(rs,"DisplayOrder").ToString() + "\"><input type=\"hidden\" name=\"GroupDisplayOrder_" + ThisGroupID.ToString() + "_vldt\" value=\"[number][blankalert=Please enter an integer number\">&nbsp;&nbsp;");
					writer.Write("<select size=\"1\" name=\"GroupType_" + ThisGroupID.ToString() + "\">\n");
					IDataReader rsst = DB.GetRS("select * from KitGroupType  " + DB.GetNoLock() + " order by KitGroupTypeID");
					while(rsst.Read())
					{
						writer.Write("<option value=\"" + DB.RSFieldInt(rsst,"KitGroupTypeID").ToString() + "\"" + Common.IIF(DB.RSFieldInt(rs,"KitGroupTypeID") == DB.RSFieldInt(rsst,"KitGroupTypeID") , " selected " , "") + ">" + DB.RSField(rsst,"Name") + "</option>");
					}
					writer.Write("</select>&nbsp;&nbsp;");
					rsst.Close();
					writer.Write("Is Required: Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"GroupIsRequired_" + ThisGroupID.ToString() + "\" value=\"1\" " + Common.IIF(DB.RSFieldBool(rs,"IsRequired"), " checked ", "") + ">\n");
					writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"GroupIsRequired_" + ThisGroupID.ToString() + "\" value=\"0\" " + Common.IIF(DB.RSFieldBool(rs,"IsRequired"), "", " checked ") + ">&nbsp;&nbsp;\n");
					writer.Write("<input type=\"button\" value=\"Delete This Group\" name=\"DeleteGroup_" + ThisGroupID.ToString() + "\" onClick=\"DeleteGroup(" + ThisGroupID.ToString() + ")\">\n");
					writer.Write("</td></tr>");

					// ITEMS:
					writer.Write("<tr><td colspan=2 bgcolor=\"" + Common.IIF(i % 2 == 0 , "#FFFFFF", "#EEEEEE") + "\">Items In This Group:</td></tr>");
					writer.Write("<tr><td colspan=2 bgcolor=\"" + Common.IIF(i % 2 == 0 , "#FFFFFF", "#EEEEEE") + "\">");
					writer.Write("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
					writer.Write("<tr><td><b>KitItemID</b></td><td><b>Name</b></td><td><b>Description</b></td><td><b>Delta Price</b></td><td><b>Display order</b></td><td><b>Is Default</b></td><td><b>Delete Item</b></td></tr>");
					IDataReader rsi = DB.GetRS("select * from KitItem  " + DB.GetNoLock() + " where KitGroupID=" + ThisGroupID.ToString() + " order by displayorder,name");
					while(rsi.Read())
					{
						writer.Write("<tr>");
						writer.Write("<td>" + DB.RSFieldInt(rsi,"KitItemID").ToString() + "</td>");
						writer.Write("<td>");
						writer.Write("<input maxLength=\"100\" size=\"40\" name=\"ItemName_" + DB.RSFieldInt(rsi,"KitItemID").ToString() + "\" value=\"" + Server.HtmlEncode(DB.RSField(rsi,"Name")) + "\">");
						writer.Write("<input type=\"hidden\" name=\"ItemName_" + DB.RSFieldInt(rsi,"KitItemID").ToString() + "_vldt\" value=\"[req][blankalert=Each kit item must have a name!]\">");
						writer.Write("</td>");
						writer.Write("<td>");
						writer.Write("<input maxLength=\"500\" size=\"40\" name=\"ItemDescription_" + DB.RSFieldInt(rsi,"KitItemID").ToString() + "\" value=\"" + Server.HtmlEncode(DB.RSField(rsi,"Description")) + "\">");
						writer.Write("</td>");
						writer.Write("<td>");
						writer.Write("<input maxLength=\"10\" size=\"10\" name=\"ItemPriceDelta_" + DB.RSFieldInt(rsi,"KitItemID").ToString() + "\" value=\"" + Localization.CurrencyStringForDB( DB.RSFieldDecimal(rsi,"PriceDelta")) + "\"> (in x.xx format)");
						writer.Write("<input type=\"hidden\" name=\"ItemPriceDelta_" + DB.RSFieldInt(rsi,"KitItemID").ToString() + "_vldt\" value=\"[req][number][blankalert=Please enter the item delta price][invalidalert=Please enter a valid dollar amount, e.g. 10.00 without the leading $ sign!]\">\n");
						writer.Write("</td>");
						writer.Write("<td><input maxLength=\"5\" size=\"10\" name=\"ItemDisplayOrder_" + DB.RSFieldInt(rsi,"KitItemID").ToString() + "\" value=\"" + DB.RSFieldInt(rsi,"DisplayOrder").ToString() + "\"></td>");
						writer.Write("<td>");
						writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"ItemIsDefault_" + DB.RSFieldInt(rsi,"KitItemID").ToString() + "\" value=\"1\" " + Common.IIF(DB.RSFieldBool(rsi,"IsDefault"), " checked ", "") + ">\n");
						writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"ItemIsDefault_" + DB.RSFieldInt(rsi,"KitItemID").ToString() + "\" value=\"0\" " + Common.IIF(DB.RSFieldBool(rsi,"IsDefault"), "", " checked ") + ">&nbsp;&nbsp;\n");
						writer.Write("</td>");
						writer.Write("<td>");
						writer.Write("<input type=\"button\" value=\"Delete This Item\" name=\"DeleteItem_" + DB.RSFieldInt(rsi,"KitItemID").ToString() + "\" onClick=\"DeleteItem(" + DB.RSFieldInt(rsi,"KitItemID").ToString() + ")\">\n");
						writer.Write("</td>");
						writer.Write("<tr>");
					}
					rsi.Close();
					// new item row:
					writer.Write("<tr>");
					writer.Write("<td>Add New Item:</td>");
					writer.Write("<td>");
					writer.Write("<input maxLength=\"100\" size=\"40\" name=\"NewItemName_" + ThisGroupID.ToString() + "\">");
					writer.Write("</td>");
					writer.Write("<td>");
					writer.Write("<input maxLength=\"500\" size=\"40\" name=\"NewItemDescription_" + ThisGroupID.ToString() + "\">");
					writer.Write("</td>");
					writer.Write("<td>");
					writer.Write("<input maxLength=\"10\" size=\"10\" name=\"NewItemPriceDelta_" + ThisGroupID.ToString() + "\"> (in x.xx format)");
					writer.Write("<input type=\"hidden\" name=\"NewItemPriceDelta_" + ThisGroupID.ToString() + "_vldt\" value=\"[number][blankalert=Please enter the item delta price][invalidalert=Please enter a valid dollar amount, e.g. 10.00 without the leading $ sign!]\">\n");
					writer.Write("</td>");
					writer.Write("<td><input maxLength=\"5\" size=\"10\" name=\"NewItemDisplayOrder_" + ThisGroupID.ToString() + "\" value=\"1\"></td>");
					writer.Write("<td>");
					writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"NewItemIsDefault_" + ThisGroupID.ToString() + "\" value=\"1\">\n");
					writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"NewItemIsDefault_" + ThisGroupID.ToString() + "\" value=\"0\" checked >\n");
					writer.Write("</td>");
					writer.Write("<td>N/A</td>");
					writer.Write("<tr>");

					writer.Write("</table>");
					writer.Write("</td></tr>");
					writer.Write("<tr><td colspan=2 height=10 bgcolor=\"" + Common.IIF(i % 2 == 0 , "#FFFFFF", "#EEEEEE") + "\"><img src=\"images/spacer.gif\" width=\"1\" height=\"20\"></td></tr>");

					i++;
				}
				rs.Close();


				writer.Write("<tr><td colspan=\"2\" bgcolor=\"" + Common.IIF(i % 2 == 0 , "#FFFFFF", "#EEEEEE") + "\">");
				writer.Write("<hr size=1>");
				writer.Write("<p align=\"left\"><b>ADD NEW GROUP:</b></p>");
				writer.Write("Group Name: <input maxLength=\"100\" size=\"50\" name=\"NewGroupName\">&nbsp;&nbsp;\n");
				writer.Write("Description: <input maxLength=\"1000\" size=\"50\" name=\"NewGroupDescription\">&nbsp;&nbsp;\n");
				writer.Write("Display Order: <input maxLength=\"3\" size=\"5\" name=\"NewGroupDisplayOrder\" value=\"1\"><input type=\"hidden\" name=\"NewDisplayOrder_vldt\" value=\"[number][blankalert=Please enter an integer number\">&nbsp;&nbsp;");
				writer.Write("<select size=\"1\" name=\"NewGroupType\">\n");
				IDataReader rsst3 = DB.GetRS("select * from KitGroupType  " + DB.GetNoLock() + " order by KitGroupTypeID");
				while(rsst3.Read())
				{
					writer.Write("<option value=\"" + DB.RSFieldInt(rsst3,"KitGroupTypeID").ToString() + "\">" + DB.RSField(rsst3,"Name") + "</option>");
				}
				writer.Write("</select>&nbsp;&nbsp;");
				rsst3.Close();
				writer.Write("Is Required: Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"NewGroupIsRequired\" value=\"1\" checked>\n");
				writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"NewGroupIsRequired\" value=\"0\" >\n");
				writer.Write("</td></tr>");

				writer.Write("<tr>\n");
				writer.Write("<td></td><td align=\"left\"><br>\n");
				writer.Write("<input type=\"submit\" value=\"Update\" name=\"submit\">\n");
				writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"reset\" value=\"Reset\" name=\"reset\">\n");
				writer.Write("</td>\n");
				writer.Write("</tr>\n");
				writer.Write("</form>\n");
				writer.Write("</table>\n");

			}

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteGroup(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete Group: ' + id + ' from this kit? This will also delete any items that are in this group!'))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'editkit.aspx?deletegroupid=' + id + '&productid=" + ProductID.ToString() + "';\n");
			writer.Write("}\n");
			writer.Write("}\n");
			writer.Write("function DeleteItem(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete Item: ' + id + ' from this group?'))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'editkit.aspx?deleteitemid=' + id + '&productid=" + ProductID.ToString() + "';\n");
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
