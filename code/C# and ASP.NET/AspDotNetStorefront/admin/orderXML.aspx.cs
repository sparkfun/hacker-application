// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for orderXML.
	/// </summary>
	public class orderXML : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
//			XmlTextWriter _xml = new XmlTextWriter(HttpContext.Current.Response.Output);
//			_xml.Formatting = Formatting.None; //Formatting.Indented;
//			_xml.WriteStartDocument();
//			_xml.WriteStartElement("OrderXML");

			// dump the order & customer info:
			Customer thisCustomer = new Customer();

			if(thisCustomer._isAdminUser)
			{
				Order ord = new Order(Common.QueryString("OrderNumber"));
				System.Type[] extraTypes = new System.Type[1];
				extraTypes[0] = typeof(CartItem);
				XmlSerializer serializer =	new XmlSerializer(typeof(Order),extraTypes);
				serializer.Serialize(HttpContext.Current.Response.OutputStream, ord);
			}
			else
			{
				Response.Write("<html><head><title>" + Common.AppConfig("StoreName") + "</title></head><body>Insufficient Privilege</body></html>");
			}

//			_xml.WriteEndElement(); // docname
//			_xml.WriteEndDocument();
//			_xml.Flush();
//			_xml.Close();

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
