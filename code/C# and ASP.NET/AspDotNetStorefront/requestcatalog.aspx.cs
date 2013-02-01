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
	/// Summary description for requestcatalog.
	/// </summary>
	public class requestcatalog : SkinBase
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Request a FREE Catalog";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(Common.Form("IsSubmit").Length != 0)
			{
				// process their entry:
				String Body = "<b>CATALOG REQUEST:</b><br><br>" + Common.GetFormInput(true,"<br>");
				try
				{
					Common.SendMail("Catalog Request From WebSite",Body,true,Common.AppConfig("MailMe_FromAddress"),Common.AppConfig("MailMe_FromName"),Common.AppConfig("MailMe_ToAddress"),Common.AppConfig("MailMe_ToName"),"",Common.AppConfig("MailMe_Server"));
				}
				catch {}
				writer.Write("<b><br>Your request has been received!<br><br>Thank you for your interest in " + Common.AppConfig("SE_MetaTitle") + ".<br><br>");
			}
			else
			{

				Address ShippingAddress = new Address();
				ShippingAddress.LoadByCustomer(thisCustomer._customerID,AddressTypes.Shipping);


				writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				writer.Write("function RequestCatalogForm_Validator(theForm)\n");
				writer.Write("{\n");
				writer.Write("  submitonce(theForm);\n");
				writer.Write("  if (theForm.FirstName.value == \"\" && theForm.LastName.value == \"\")\n");
				writer.Write("  {\n");
				writer.Write("    alert(\"You must enter at least one of First Name or Last Name.\");\n");
				writer.Write("    theForm.FirstName.focus();\n");
				writer.Write("    submitenabled(theForm);\n");
				writer.Write("    return (false);\n");
				writer.Write("  }\n");
				writer.Write("  if (theForm.ShippingState.selectedIndex < 1)\n");
				writer.Write("  {\n");
				writer.Write("    alert(\"Please select one of the State options.\");\n");
				writer.Write("    theForm.ShippingState.focus();\n");
				writer.Write("    submitenabled(theForm);\n");
				writer.Write("    return (false);\n");
				writer.Write("  }\n");
				writer.Write("  return (true);\n");
				writer.Write("}\n");
				writer.Write("</script>\n");
			
				String act = "requestcatalog.aspx";
				writer.Write("<form method=\"POST\" action=\"" + act + "\" name=\"RequestCatalogForm\" id=\"RequestCatalogForm\" onSubmit=\"return (validateForm(this) && RequestCatalogForm_Validator(this))\">");
				writer.Write("        <input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
				writer.Write("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"100%\" colspan=\"2\"><br><b>To receive our FREE catalog, please enter your information below:</b><br><br><br></td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">First Name:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"ShippingFirstName\" size=\"20\" maxlength=\"50\" value=\"" + Server.HtmlEncode(ShippingAddress.FirstName) + "\"> (required)");
				writer.Write("        <input type=\"hidden\" name=\"ShippingFirstName_vldt\" value=\"[req][blankalert=Please enter your first name]\">\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">Last Name:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"ShippingLastName\" size=\"20\" maxlength=\"50\" value=\"" + Server.HtmlEncode(ShippingAddress.LastName) + "\"> (required)");
				writer.Write("        <input type=\"hidden\" name=\"ShippingLastName_vldt\" value=\"[req][blankalert=Please enter your last name]\">\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">Company:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"ShippingCompany\" size=\"34\" maxlength=\"100\" value=\"" + Server.HtmlEncode(ShippingAddress.Company) + "\">");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">Address1:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"ShippingAddress1\" size=\"34\" maxlength=\"100\" value=\"" + Server.HtmlEncode(ShippingAddress.Address1) + "\"> (required)");
				writer.Write("        <input type=\"hidden\" name=\"ShippingAddress1_vldt\" value=\"[req][blankalert=Please enter your address]\">\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">Address2:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"ShippingAddress2\" size=\"34\" maxlength=\"100\" value=\"" + Server.HtmlEncode(ShippingAddress.Address2) + "\">");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">Suite:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"ShippingSuite\" size=\"34\" maxlength=\"50\" value=\"" + Server.HtmlEncode(ShippingAddress.Suite) + "\">");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">City or APO/AFO:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"ShippingCity\" size=\"34\" maxlength=\"50\" value=\"" + Server.HtmlEncode(ShippingAddress.City) + "\"> (required)");
				writer.Write("        <input type=\"hidden\" name=\"ShippingCity_vldt\" value=\"[req][blankalert=Please enter your city]\">\n");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">State/Province:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("<select size=\"1\" name=\"ShippingState\">");
				writer.Write("<OPTION value=\"\"" + Common.IIF(ShippingAddress.State.Length==0 , " selected" , String.Empty) + ">SELECT ONE</option>");
				DataSet dsstate = DB.GetDS("select * from state  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddHours(1));
				foreach(DataRow row in dsstate.Tables[0].Rows)
				{
					writer.Write("<OPTION value=\"" + DB.RowField(row,"Abbreviation") + "\"" + Common.IIF(ShippingAddress.State == DB.RowField(row,"Abbreviation")," selected ","") + ">" + DB.RowField(row,"Name") + "</option>");
				}
				dsstate.Dispose();
				writer.Write("</select> (required)");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">Zip:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("        <input type=\"text\" name=\"ShippingZip\" size=\"14\" maxlength=\"10\" value=\"" + ShippingAddress.Zip + "\"> (required)");
				writer.Write("        <input type=\"hidden\" name=\"ShippingZip_vldt\" value=\"[req][blankalert=Please enter your zip code][invalidalert=Please enter a valid US Zipcode]\">");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("      <tr>");
				writer.Write("        <td width=\"25%\">Country:</td>");
				writer.Write("        <td width=\"75%\">");
				writer.Write("<SELECT NAME=\"ShippingCountry\" size=\"1\">");
				DataSet dscountry = DB.GetDS("select * from country  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddHours(1));
				writer.Write("<OPTION value=\"\"" + Common.IIF(ShippingAddress.Country.Length==0 , " selected" , String.Empty) + ">SELECT ONE</option>");
				foreach(DataRow row in dscountry.Tables[0].Rows)
				{
					writer.Write("<OPTION value=\"" + DB.RowField(row,"Name") + "\"" + Common.IIF(ShippingAddress.Country == DB.RowField(row,"Name")," selected ","") + ">" + DB.RowField(row,"Name") + "</option>");
				}
				dscountry.Dispose();
				writer.Write("</SELECT>");
				writer.Write("        </td>");
				writer.Write("      </tr>");
				writer.Write("	  <tr><td colspan=\"2\" align=\"center\" height=\"25\"></td></tr>");
				writer.Write("	  <tr><td colspan=\"2\" align=\"center\">");
				writer.Write("        <input type=\"submit\" value=\"Request Catalog\" name=\"Continue\">");
				writer.Write("        </td></tr>");
				writer.Write("    </table>");
				writer.Write("</form>");
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
