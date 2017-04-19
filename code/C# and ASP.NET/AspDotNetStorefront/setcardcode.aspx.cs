using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for setcardcode.
	/// </summary>
	public class setcardcode : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");

			Customer thisCustomer = new Customer();
			thisCustomer.RequireCustomerRecord();

			String PageTitle = Common.QueryString("Title");

			Response.Write("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0//EN\" \"http://www.w3.org/TR/REC-html40/strict.dtd\">\n");
			Response.Write("<html>\n");
			Response.Write("<head>\n");
			Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1252\">\n");
			Response.Write("<title>%METATITLE%</title>\n");
			Response.Write("<link rel=\"stylesheet\" href=\"skins/Skin_" + thisCustomer._skinID.ToString() + "/style.css\" type=\"text/css\">\n");
			Response.Write("<script type=\"text/javascript\" src=\"formValidate.js\"></script>\n");
			Response.Write("</head>\n");
			Response.Write("<body style=\"margin: 0px;\" bottommargin=\"0\" leftmargin=\"0\" marginheight=\"0\" marginwidth=\"0\" rightmargin=\"0\" topmargin=\"0\" bgcolor=\"#FFFFFF\" ONBLUR=\"self.close();\" onClick=\"self.close();\" onLoad=\"self.focus()\">\n");

			// IMAGE POPUP:
			Response.Write("<center>\n");

			if(thisCustomer.SetGiftCardCode(Common.Form("CardCode")))
			{
				Response.Write("<br><br><br><br><br><br><br><p>Your gift card code has been entered.<br><br>Your discount will be applied when you check out.<br><br>Thanks</p>");
			}
			else
			{
				Response.Write("<blockquote><br><br><br><br><br><br><br><p><b><font color=red>The gift card code you entered is not valid.</b></font><br><br>Please <a href=\"javascript:history.back(-1);\">click here</a> to go back and enter a new gift card code. If you need assistance, please contact our customer service department.</p></blockquote>");
			}

			Response.Write("</center>\n");
						
			Response.Write("<p align=\"center\"><a href=\"javascript:self.close();\">close</a></p>\n");
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
