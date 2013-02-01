// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for recurringshippinglist.
	/// </summary>
	public class recurringshippinglist : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			
			Customer thisCustomer = new Customer();
			
			Response.Write("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0//EN\" \"http://www.w3.org/TR/REC-html40/strict.dtd\">\n");
			Response.Write("<html>\n");
			Response.Write("<head>\n");
			Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1252\">\n");
			Response.Write("<title>Recurring Shipments List</title>\n");
			Response.Write("<link rel=\"stylesheet\" href=\"skins/Skin_" + thisCustomer._skinID.ToString() + "/style.css\" type=\"text/css\">\n");
			Response.Write("<script type=\"text/javascript\" src=\"formValidate.js\"></script>\n");
			Response.Write("</head>\n");
			Response.Write("<body style=\"margin: 0px;\" bottommargin=\"0\" leftmargin=\"0\" marginheight=\"0\" marginwidth=\"0\" rightmargin=\"0\" topmargin=\"0\" bgcolor=\"#FFFFFF\" onLoad=\"self.focus()\">\n");

			if(!thisCustomer._isAdminUser)
			{
				Response.Write("<b><font color=red>PERMISSION DENIED</b></font>");
			}
			else
			{
				Response.Write("<p><b><font color=blue>PENDING SHIPMENT LIST FOR PROCESSED RECURRING ORDERS:</b></font></p>");
				IDataReader rs = DB.GetRS("select * from recurringshipments " + DB.GetNoLock());
				Response.Write("<table cellpadding=2 cellspacing=1 border=1>");
				Response.Write("<tr>");
				Response.Write("<th>Customer ID</th>");
				Response.Write("<th>Order Number</th>");
				Response.Write("<th width=350>Shipping Summary</th>");
				Response.Write("<th>Shipping First Name</th>");
				Response.Write("<th>Shipping Last Name</th>");
				Response.Write("<th>Shipping Company</th>");
				Response.Write("<th>Shipping Address1</th>");
				Response.Write("<th>Shipping Address2</th>");
				Response.Write("<th>Shipping Suite</th>");
				Response.Write("<th>Shipping City</th>");
				Response.Write("<th>Shipping State</th>");
				Response.Write("<th>Shipping Zip</th>");
				Response.Write("<th>Shipping Country</th>");
				Response.Write("<th>Shipping Method ID</th>");
				Response.Write("<th>Shipping Method</th>");
				Response.Write("<th width=350>Packing List</th>");
				//Response.Write("<th>Order XML</th>");
				Response.Write("<th>Processed By Admin ID</th>");
				Response.Write("</tr>");
				while(rs.Read())
				{

					StringBuilder tmpS = new StringBuilder(1000);;
					
					tmpS.Append((Common.Capitalize(DB.RSField(rs,"ShippingFirstName")) + " " + Common.Capitalize(DB.RSField(rs,"ShippingLastName"))).Trim() + "<br>");
					if(DB.RSField(rs,"ShippingCompany").Length != 0)
					{
						tmpS.Append(Common.Capitalize(DB.RSField(rs,"ShippingCompany")) + "<br>");
					}
					tmpS.Append(DB.RSField(rs,"ShippingAddress1") + "<br>");
					if(DB.RSField(rs,"ShippingAddress2").Length != 0)
					{
						tmpS.Append(DB.RSField(rs,"ShippingAddress2") + "<br>");
					}
					if(DB.RSField(rs,"ShippingSuite").Length != 0)
					{
						tmpS.Append("Suite: " + DB.RSField(rs,"ShippingSuite") + "<br>");
					}
					tmpS.Append(Common.Capitalize(DB.RSField(rs,"ShippingCity")) + ", " + DB.RSField(rs,"ShippingState").ToUpper() + " " + DB.RSField(rs,"ShippingZip"));
					if(DB.RSField(rs,"ShippingCountry").Length != 0)
					{
						tmpS.Append("<br>" + DB.RSField(rs,"ShippingCountry"));
					}

					Response.Write("<tr>");
					Response.Write("<td>" + DB.RSFieldInt(rs,"CustomerID").ToString() + "&nbsp;</td>");
					Response.Write("<td>" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "&nbsp;</td>");
					Response.Write("<td>" + tmpS.ToString() + "&nbsp;</td>");
					Response.Write("<td>" + DB.RSField(rs,"ShippingFirstName") + "&nbsp;</td>");
					Response.Write("<td>" + DB.RSField(rs,"ShippingLastName") + "&nbsp;</td>");
					Response.Write("<td>" + DB.RSField(rs,"ShippingCompany") + "&nbsp;</td>");
					Response.Write("<td>" + DB.RSField(rs,"ShippingAddress1") + "&nbsp;</td>");
					Response.Write("<td>" + DB.RSField(rs,"ShippingAddress2") + "&nbsp;</td>");
					Response.Write("<td>" + DB.RSField(rs,"ShippingSuite") + "&nbsp;</td>");
					Response.Write("<td>" + DB.RSField(rs,"ShippingCity") + "&nbsp;</td>");
					Response.Write("<td>" + DB.RSField(rs,"ShippingState") + "&nbsp;</td>");
					Response.Write("<td>" + DB.RSField(rs,"ShippingZip") + "&nbsp;</td>");
					Response.Write("<td>" + DB.RSField(rs,"ShippingCountry") + "&nbsp;</td>");
					Response.Write("<td>" + DB.RSFieldInt(rs,"ShippingMethodID").ToString() + "&nbsp;</td>");
					Response.Write("<td>" + DB.RSField(rs,"ShippingMethod") + "&nbsp;</td>");
					Response.Write("<td width=350>" + DB.RSField(rs,"PackingList") + "&nbsp;</td>");
					//Response.Write("<td>" + Server.HtmlEncode(DB.RSField(rs,"OrderXML")) + "&nbsp;</td>");
					Response.Write("<td>" + DB.RSFieldInt(rs,"LastUpdatedBy").ToString() + "&nbsp;</td>");
					Response.Write("</tr>");
				}
				Response.Write("</table>");
				rs.Close();
			}

			Response.Write("<p align=\"center\"><a href=\"javascript:self.close();\">Close</a></p>");
		
			
			Response.Write("</body>\n");
			Response.Write("</html>\n");
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
