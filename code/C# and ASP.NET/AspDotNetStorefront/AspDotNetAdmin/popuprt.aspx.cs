// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for popuprt.
	/// </summary>
	public class popuprt : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			
			Customer thisCustomer = new Customer();
			
			Response.Write("<html>\n");
			Response.Write("<head>\n");
			Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1252\">\n");
			Response.Write("<title>Admin Popup</title>\n");
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
				IDataReader rs = DB.GetRS("Select * from orders  " + DB.GetNoLock() + " where ordernumber=" + Common.QueryStringUSInt("OrderNumber").ToString());
				if(rs.Read())
				{
					String r1 = DB.RSField(rs,"RTShipRequest");
					String r2 = DB.RSField(rs,"RTShipResponse");
					String rqst = String.Empty;
					try
					{
						rqst = Common.PrettyPrintXml(r1);
					}
					catch
					{
						rqst = r1;
					}
					String resp = String.Empty;
					try
					{
						resp = Common.PrettyPrintXml(r2);
					}
					catch
					{
						resp = r2;
					}
					Response.Write("<b>RT Shipping Request: </b><br><br><textarea cols=120 rows=70>" + Server.HtmlEncode(r1) + "</textarea><br><br>");
					Response.Write("<hr size=1>");
					Response.Write("<b>RT Shipping Response: </b><br><br><textarea cols=120 rows=70>" + Server.HtmlEncode(r2) + "</textarea><br><br>");
				}
				else
				{
					Response.Write("<b><font color=red>ORDER NOT FOUND</b></font>");
				}
				rs.Close();
			}

		
			
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
