// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for requestquote.
	/// </summary>
	public class requestquote : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "<a class=\"SectionTitleText\" href=\"default.aspx\">Home</a> - Request Custom Quote";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write("<script Language=\"JavaScript\">\n");
			writer.Write("function CustomBidForm_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("	submitonce(theForm);\n");
			writer.Write("	if (theForm.Summary.value.length == 0)\n");
			writer.Write("	{\n");
			writer.Write("		alert(\"Please enter a short summary of your bid requirements.\");\n");
			writer.Write("		theForm.Summary.focus();\n");
			writer.Write("		submitenabled(theForm);\n");
			writer.Write("		return (false);\n");
			writer.Write("    }\n");
			writer.Write("  return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");
			
			// CUSTOM QUOTE FORM:
			writer.Write("<div align=\"left\">\n");
			writer.Write("<form method=\"POST\" action=\"custombid.aspx\" onsubmit=\"return (validateForm(this) && CustomBidForm_Validator(this))\" id=\"CustomBidForm\" name=\"CustomBidForm\">\n");
			//writer.Write("<b>FOR A CUSTOM QUOTE:</b><br>\n");
			writer.Write("<b>We will not be undersold!</b><br><br>If you are with a government or municipal agency, and require a custom quote, please do one of the following:<br><br>1. <b>Request Quote on Checkout</b>: Add all the items and quantities that you need to your " + Common.AppConfig("CartPrompt").ToLower() + ", and then click on <a href=\"ShoppingCart.aspx\">\"Request Quote\"</a> during checkout. Instead of charging your credit card, we will respond with a custom bid for your order, or<br><br>2. <b>Submit Request Form</b>: Use the request a quote form below to send us a description of your requirements, or <a href=\"" + SE.MakeDriverLink("contact") + "\">contact us by phone</a>. We'll be happy to prepare a custom bid tailored to meet your exact specifications.<br><br>\n");
			writer.Write("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\n");
			writer.Write("<tr>\n");
			writer.Write("<td align=\"left\" valign=\"top\">\n");
			writer.Write("Summary of Need:<br><textarea rows=\"30\" name=\"Summary\" cols=\"90\"></textarea>\n");
			//writer.Write("<input type=\"hidden\" name=\"Summary_vldt\" value=\"[req][blankalert=Please enter a short summary of your bid requirements]\">\n");
			writer.Write("</td>\n");
			writer.Write("<td width=\"20\"></td>\n");
			writer.Write("<td align=\"left\" valign=\"top\">\n");
			writer.Write("<br>Your Name: \n");
			writer.Write("<input type=\"text\" name=\"Name\" size=\"35\">\n");
			writer.Write("<input type=\"hidden\" name=\"Name_vldt\" value=\"[req][blankalert=Please enter your name so we may contact you if we have questions]\">\n");
			writer.Write("<br><br>Organization: \n");
			writer.Write("<input type=\"text\" name=\"Organization\" size=\"35\">\n");
			writer.Write("<input type=\"hidden\" name=\"Organization_vldt\" value=\"[req][blankalert=Please enter your organization so we can properly respond to your bid request]\">\n");
			writer.Write("<br><br>Your E-Mail: \n");
			writer.Write("<input type=\"text\" name=\"EMail\" size=\"35\">\n");
			writer.Write("<input type=\"hidden\" name=\"EMail_vldt\" value=\"[req][email][blankalert=Please enter your e-mail address so we may contact you if we have questions][invalidalert=Please enter a valid e-mail address so we may contact you if we have questions]\">\n");
			writer.Write("<br><br>Your Phone: \n");
			writer.Write("<input type=\"text\" name=\"Phone\" size=\"35\">\n");
			writer.Write("<input type=\"hidden\" name=\"Phone_vldt\" value=\"[req][blankalert=Please enter a valid phone number so we may contact you if we have questions]\">\n");
			writer.Write("<br><br><input type=\"submit\" value=\"Submit\" name=\"B1\">\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("</form>\n");
			writer.Write("</div>\n");
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
