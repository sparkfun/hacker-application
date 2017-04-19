// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Text;
using System.Web;
using System.Xml;
using System.Data;
using System.Xml.Serialization;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for cst_emails.
	/// </summary>
	public class cst_emails : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			// dump the order & customer info:
			Response.Write("</body></html>");
			Response.Expires = -1;
			Customer thisCustomer = new Customer();
			if(thisCustomer._isAdminUser)
			{
				Response.Write("<a href=\"cst_emails.aspx?issubmit=true&type=all\">All Customers</a>&nbsp;&nbsp;<a href=\"cst_emails.aspx?issubmit=true&type=ordersonly\">Only Customers With Orders</a><br><br>");
				if(Common.QueryString("issubmit").Length != 0)
				{
					IDataReader rs = DB.GetRS("select * from customer  " + DB.GetNoLock() + " where deleted=0 and Email not like " + DB.SQuote("Anon_%") + Common.IIF(Common.QueryString("Type").ToLower() == "all", "", " and CustomerID in (select distinct customerid from orders " + DB.GetNoLock() + ")") + " order by createdon desc");
					while(rs.Read())
					{
						Response.Write(DB.RSField(rs,"Email") + "<br>");
					}
					rs.Close();
				}
			}
			else
			{
				Response.Write("Insufficient Privilege");
			}
			Response.Write("</body></html>");
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
