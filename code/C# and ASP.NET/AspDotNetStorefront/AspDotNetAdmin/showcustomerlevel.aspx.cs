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
	/// Summary description for showcustomerlevel.
	/// </summary>
	public class showcustomerlevel : SkinBase
	{
		
		private int CustomerLevelID;
		private String CustomerLevelName;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			CustomerLevelID = Common.QueryStringUSInt("CustomerLevelID");
			CustomerLevelName = Common.GetCustomerLevelName(CustomerLevelID);
			if(Common.QueryString("DeleteID").Length != 0)
			{
				// remove this level from this customer:
				DB.ExecuteSQL("update Customer set CustomerLevelID=0 where CustomerID=" + Common.QueryString("DeleteID"));
			}

			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				String EMail = Common.Form("EMail");
				if(EMail.Length != 0)
				{
					int CustomerID = Customer.GetIDFromEMail(EMail);
					if(CustomerID != 0)
					{
						DB.ExecuteSQL("Update customer set CustomerLevelID=" + CustomerLevelID.ToString() + " where CustomerID=" + CustomerID.ToString());
					}
					else
					{
						ErrorMsg = "That customer e-mail was not found in the database";
					}
				}
			}
			SectionTitle = "<a href=\"CustomerLevels.aspx\">CustomerLevels</a> - Show Customer Level: " + CustomerLevelName;
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(ErrorMsg.Length != 0)
			{
				writer.Write("<p><b><font color=red>" + ErrorMsg + "</font></b></p>\n");
			}
			DataSet ds = DB.GetDS("select * from Customer  " + DB.GetNoLock() + " where deleted=0 and CustomerLevelID=" + CustomerLevelID.ToString() + " order by email",false,System.DateTime.Now.AddDays(1));

			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function Form_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");
			
			writer.Write("<form method=\"POST\" action=\"showCustomerLevel.aspx?customerlevelid=" + CustomerLevelID.ToString() + "\"  id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("<table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("<tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("<td width=\"10%\"><b>Customer ID</b></td>\n");
			writer.Write("<td ><b>Name</b></td>\n");
			writer.Write("<td ><b>EMail</b></td>\n");
			writer.Write("<td align=\"center\"><b>Clear Level for this Customer</b></td>\n");
			writer.Write("</tr>\n");
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				writer.Write("<tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
				writer.Write("<td width=\"10%\">" + DB.RowFieldInt(row,"CustomerID").ToString() + "</td>\n");
				writer.Write("<td >" + (DB.RowField(row,"FirstName") + " " + DB.RowField(row,"LastName")).Trim() + "</td>\n");
				writer.Write("<td >" + DB.RowField(row,"EMail") + "</td>\n");
				writer.Write("<td width=\"10%\" align=\"center\"><input type=\"button\" value=\"Clear Level\" name=\"Delete_" + DB.RowFieldInt(row,"CustomerID").ToString() + "\" onClick=\"DeleteCustomerLevel(" + DB.RowFieldInt(row,"CustomerID").ToString() + ")\"></td>\n");
				writer.Write("</tr>\n");
			}
			ds.Dispose();
			writer.Write(" </table>\n");
			writer.Write("<p align=\"left\">");
			writer.Write("<input type=\"text\" name=\"EMail\" size=\"37\" maxlength=\"100\">");
			writer.Write("<input type=\"hidden\" name=\"EMail_vldt\" value=\"[req][email][blankalert=Please enter a valid customer e-mail address.  You must know this in advance, and type it in here][invalidalert=Please enter a valid customer e-mail address]\">");
			writer.Write("<input type=\"submit\" value=\"Add Customer To This Level\" name=\"Submit\">");
			writer.Write("</p>\n");
			writer.Write("</form>\n");

			writer.Write("</center></b>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteCustomerLevel(id)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you clear the level for customer id: ' + id + '. NOTE: this will NOT delete their customer record'))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'showCustomerLevel.aspx?customerlevelid=" + CustomerLevelID.ToString() + "&deleteid=' + id;\n");
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
