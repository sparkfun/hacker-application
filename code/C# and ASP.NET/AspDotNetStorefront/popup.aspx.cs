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
	/// Summary description for popup.
	/// </summary>
	public class popup : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");

			String PageTitle = Common.QueryString("Title");
			Customer thisCustomer = new Customer();

			Response.Write("<html>\n");
			Response.Write("<head>\n");
			Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1252\">\n");
			Response.Write("<title>Popup Window " + Common.GetRandomNumber(1,1000000).ToString() + "</title>\n");
			Response.Write("<link rel=\"stylesheet\" href=\"skins/Skin_" + thisCustomer._skinID.ToString() + "/style.css\" type=\"text/css\">\n");
			Response.Write("<script type=\"text/javascript\" src=\"formValidate.js\"></script>\n");
			Response.Write("</head>\n");


			if(Common.QueryString("src").Length != 0)
			{
				// IMAGE POPUP:
				Response.Write("<body style=\"margin: 0px;\" bottommargin=\"0\" leftmargin=\"0\" marginheight=\"0\" marginwidth=\"0\" rightmargin=\"0\" topmargin=\"0\" bgcolor=\"#FFFFFF\" ONBLUR=\"self.close();\" onClick=\"self.close();\" onLoad=\"self.focus()\">\n");
				Response.Write("<center>\n");
				Response.Write("<img name=\"Image1\" onClick=\"javascript:self.close();\" style=\"cursor:hand;\" alt=\"Close this window\" border=\"0\" src=\"" + Server.HtmlEncode(Common.QueryString("src")) + "\">\n");
				Response.Write("<br>");
			}
			else if(Common.QueryString("orderoptionid").Length != 0)
			{
				// kit group info popoup:
				Response.Write("<body style=\"margin: 0px;\" bottommargin=\"0\" leftmargin=\"0\" marginheight=\"0\" marginwidth=\"0\" rightmargin=\"0\" topmargin=\"0\" bgcolor=\"#FFFFFF\" %ONBLUR% onLoad=\"self.focus()\">\n");
				IDataReader rs = DB.GetRS("Select * from orderoption  " + DB.GetNoLock() + " where orderoptionid=" + Common.QueryStringUSInt("orderoptionid").ToString());
				if(rs.Read())
				{
					Response.Write("<p align=\"left\"><b>" + DB.RSField(rs,"Name") + "</b>:</p>");
					Response.Write("<p align=\"left\">" + DB.RSField(rs,"Description") + "</p>");
				}
				else
				{
					Response.Write("<p align=\"left\"><b><font color=red>ORDER OPTION NOT FOUND!</font></b>:</p>");
				}
				rs.Close();
			}
			else if(Common.QueryString("kitgroupid").Length != 0)
			{
				// kit group info popoup:
				Response.Write("<body style=\"margin: 0px;\" bottommargin=\"0\" leftmargin=\"0\" marginheight=\"0\" marginwidth=\"0\" rightmargin=\"0\" topmargin=\"0\" bgcolor=\"#FFFFFF\" %ONBLUR% onLoad=\"self.focus()\">\n");
				IDataReader rs = DB.GetRS("Select * from kitgroup  " + DB.GetNoLock() + " where kitgroupid=" + Common.QueryStringUSInt("kitgroupid").ToString());
				if(rs.Read())
				{
					Response.Write("<p align=\"left\"><b>" + DB.RSField(rs,"Name") + "</b>:</p>");
					Response.Write("<p align=\"left\">" + DB.RSField(rs,"Description") + "</p>");
				}
				else
				{
					Response.Write("<p align=\"left\"><b><font color=red>KIT GROUP NOT FOUND!</font></b>:</p>");
				}
				rs.Close();
			}
			else if(Common.QueryString("KitItemID").Length != 0)
			{
				// kit group info popoup:
				Response.Write("<body style=\"margin: 0px;\" bottommargin=\"0\" leftmargin=\"0\" marginheight=\"0\" marginwidth=\"0\" rightmargin=\"0\" topmargin=\"0\" bgcolor=\"#FFFFFF\" %ONBLUR% onLoad=\"self.focus()\">\n");
				IDataReader rs3 = DB.GetRS("Select * from kititem  " + DB.GetNoLock() + " where KitItemID=" + Common.QueryStringUSInt("KitItemID").ToString());
				if(rs3.Read())
				{
					Response.Write("<p align=\"left\"><b>" + DB.RSField(rs3,"Name") + "</b>:</p>");
					Response.Write("<p align=\"left\">" + DB.RSField(rs3,"Description") + "</p>");
				}
				else
				{
					Response.Write("<p align=\"left\"><b><font color=red>KIT ITEM NOT FOUND!</font></b>:</p>");
				}
				rs3.Close();
			}
			else
			{
				// CONTENT POPUP:
				Response.Write("<body style=\"margin: 0px;\" bottommargin=\"0\" leftmargin=\"0\" marginheight=\"0\" marginwidth=\"0\" rightmargin=\"0\" topmargin=\"0\" bgcolor=\"#FFFFFF\" %ONBLUR% onLoad=\"self.focus()\">\n");

				Topic t = new Topic(Common.QueryString("Topic"),thisCustomer._localeSetting);
				
				if(t._contents.Length == 0)
				{
					Response.Write("<img src=\"images/spacer.gif\" border=\"0\" height=\"100\" width=\"1\"><br>\n");
					Response.Write("<p align=\"center\"><font class=\"big\"><b>This page is currently empty. Please check back again for an update.</b></font></p>");
				}
				else
				{	
					Response.Write("\n");
					Response.Write("<!-- READ FROM " + Common.IIF(t._fromDB , "DB", "FILE: " + t._fn) + ": " + " -->");
					Response.Write("\n");
					Response.Write(t._contents);
					Response.Write("\n");
					Response.Write("<!-- END OF " + Common.IIF(t._fromDB , "DB", "FILE: " + t._fn) + ": " + " -->");
					Response.Write("\n");
				}

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
