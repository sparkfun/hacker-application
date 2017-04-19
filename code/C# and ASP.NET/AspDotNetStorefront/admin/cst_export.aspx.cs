// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
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
	/// Summary description for cst_export.
	/// </summary>
	public class cst_export : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Customer thisCustomer = new Customer();
			if(thisCustomer._isAdminUser)
			{
				Response.Expires = -1;
				Response.ContentType = "text/xml";
				// Create a new XmlTextWriter instance
				//XmlTextWriter writer = new XmlTextWriter(Server.MapPath("userInfo.xml"), Encoding.UTF8);
				XmlTextWriter writer = new XmlTextWriter(Response.OutputStream, Encoding.UTF8);
    
				// start writing!
				writer.WriteStartDocument();
				writer.WriteStartElement("CustomerList");
				//SEC4
				string SuperuserFilter = Common.IIF(thisCustomer.IsAdminSuperUser , String.Empty , String.Format(" Customer.CustomerID not in ({0}) and ",Common.AppConfig("Admin_Superuser")));

				IDataReader rs = DB.GetRS("select * from customer  " + DB.GetNoLock() + " where " + SuperuserFilter.ToString() + " deleted=0 and Email not like " + DB.SQuote("Anon_%") + " order by createdon desc");
				while(rs.Read())
				{
					writer.WriteStartElement("Customer");
					writer.WriteElementString("FirstName", DB.RSField(rs,"FirstName"));
					writer.WriteElementString("LastName", DB.RSField(rs,"LastName"));
					writer.WriteElementString("EMail", DB.RSField(rs,"EMail"));
					writer.WriteElementString("OkToEMail", DB.RSFieldBool(rs,"OkToEMail").ToString());
					writer.WriteElementString("CreatedOn", Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs,"CreatedOn")));
					writer.WriteElementString("RegisteredOn", Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs,"RegisteredOn")));
					writer.WriteEndElement();
				}
				rs.Close();
	    
				writer.WriteEndElement();
				writer.WriteEndDocument();
				writer.Close();    
			}
			else
			{
				Response.Expires = -1;
				Response.Write("</body></html>");
				Response.Write("Insufficient Privilege");
				Response.Write("</body></html>");
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
