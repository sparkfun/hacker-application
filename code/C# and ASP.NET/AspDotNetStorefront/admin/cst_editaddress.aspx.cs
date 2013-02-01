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

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for cst_editaddress.
	/// </summary>
	public class cst_editaddress : SkinBase
	{

		private Customer targetCustomer;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			targetCustomer = new Customer(Common.QueryStringUSInt("CustomerID"),true);
			if(targetCustomer._customerID == 0)
			{
				Response.Redirect("Customers.aspx");
			}
			SectionTitle = "<a href=\"Customers.aspx?searchfor=" + targetCustomer._customerID.ToString() + "\">Customers</a> - Account Info: " + (targetCustomer._firstName + " " + targetCustomer._lastName).Trim() + " (" + targetCustomer._email + ") - Edit Address";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{

			bool AllowShipToDifferentThanBillTo = Common.AppConfigBool("AllowShipToDifferentThanBillTo");
			String TabImage = "editaddress.gif";
			int AddressID = Common.QueryStringUSInt("AddressID");
			string ReturnUrl = Common.QueryString("ReturnUrl");
   
			AddressTypes AddressType = AddressTypes.Unknown;
			String AddressTypeString = Common.QueryString("AddressType");
			AddressType = (AddressTypes)Enum.Parse(typeof(AddressTypes),AddressTypeString,true);
			
      writer.Write("\n<script type=\"text/javascript\" language=\"javascript\" >\n");
      writer.Write("function ShowPaymentInput(theRadio)\n");
      writer.Write("{\n");
      writer.Write("  if (theRadio.value == 'NONE')\n");
      writer.Write("  {\n");
      writer.Write("    document.getElementById('divCheck').style.display = 'none';\n");
      writer.Write("    document.getElementById('divCard').style.display = 'none';\n");
      writer.Write("  }\n");
      writer.Write("  else if (theRadio.value == 'ECHECK')\n");
      writer.Write("  {\n");
      writer.Write("    document.getElementById('divCheck').style.display = '';\n");
      writer.Write("    document.getElementById('divCard').style.display = 'none';\n");
      writer.Write("  }\n");
      writer.Write("  else\n");
      writer.Write("  {\n");
      writer.Write("    document.getElementById('divCheck').style.display = 'none';\n");
      writer.Write("    document.getElementById('divCard').style.display = '';\n");
      writer.Write("  }\n");
      writer.Write("  return true;\n");
      writer.Write("}\n");
      writer.Write("</script>\n");
      
      // ACCOUNT BOX:
			writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/"+ TabImage + "\" border=\"0\"><br>");
			writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
      
			// ADDRESS BOX:

			Address theAddress = new Address(targetCustomer._customerID); 
			theAddress.LoadFromDB(AddressID);

			String act = String.Format("cst_editaddress_process.aspx?CustomerID={0}&AddressType={1}&AddressID={2}&ReturnUrl={3}",targetCustomer._customerID,AddressType,AddressID,ReturnUrl);
			writer.Write("<form method=\"POST\" action=\"" + act + "\" name=\"EditAddressForm\" id=\"EditAddressForm\" onSubmit=\"return (validateForm(this) && AddressInputForm_Validator(this))\">\n");
      writer.Write("<input type=\"hidden\" id=\"DeleteAddressID\" name=\"DeleteAddressID\" value=\"0\" >\n"); 
 
			writer.Write(String.Format("<b>You are Editing the {0} Address below.</b><hr>\n",AddressType));

			//Display the Address input form fields
			writer.Write(theAddress.InputHTML());

      if (AddressType == AddressTypes.Billing)
      {
        
        writer.Write("<hr>");
        writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
        writer.Write("<tr><td width=\"25%\" align=\"left\" valign=\"bottom\">\n");
        writer.Write("Preferred Payment Method:</td>\n");
        writer.Write("<td align=\"left\" valign=\"bottom\">\n");
        writer.Write(String.Format("Credit Card<input type=\"radio\" id=\"PaymentMethod\" name=\"PaymentMethod\" value=\"Credit Card\" onClick=\"ShowPaymentInput(this)\" {0}>",Common.IIF(theAddress.PaymentMethod.ToUpper().Replace(" ","") == "CREDITCARD","checked","")));
        writer.Write(String.Format(" eCheck<input type=\"radio\" id=\"PaymentMethod\" name=\"PaymentMethod\" value=\"ECHECK\" onClick=\"ShowPaymentInput(this)\" {0}>",Common.IIF(theAddress.PaymentMethod.ToUpper().Replace(" ","") == "ECHECK","checked","")));
        writer.Write("</td></tr>\n");
        writer.Write("</table>");

        writer.Write(String.Format("<div id=\"divCard\" name-\"divCard\" style=\"display:{0}\">",Common.IIF(theAddress.PaymentMethod.ToUpper().Replace(" ","") == "CREDITCARD","","none")));
        writer.Write("<p><hr><b>Enter Credit Card Account information</b><hr></p>");
        writer.Write(theAddress.InputCardHTML(false));
        writer.Write("</div>");
        
        writer.Write(String.Format("<div id=\"divCheck\" name=\"divCheck\" style=\"display:{0}\">",Common.IIF(theAddress.PaymentMethod.ToUpper().Replace(" ","") == "ECHECK","","none")));
        writer.Write("<p><hr><b>Enter Checking Account information</b><hr></p>");
        writer.Write(theAddress.InputECheckHTML(false));
        writer.Write("</div>");
      }

      bool CanDelete = (0 == DB.GetSqlN(String.Format("select count(0) as N from ShoppingCart  " + DB.GetNoLock() + " where (ShippingAddressID={0} or BillingAddressID={0}) and CartType={1}",AddressID,(int)CartTypeEnum.RecurringCart)));
      
      writer.Write("</td></tr>\n");
			writer.Write("</table>\n");
			//Button to submit the form
      writer.Write("<p align=\"center\"><input type=\"submit\" value=\"Save Edited Address\" name=\"Continue\"></p>\n");
      if (CanDelete)
      {
        writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"submit\" value=\"Delete This Address\" name=\"Delete\" onClick=\"if (confirm('Do you want to delete this address permanently?')) {EditAddressForm.DeleteAddressID.value=" + AddressID.ToString() + ";} \"></p>");
      }
      else
      {
        writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"button\" value=\"Delete This Address\" name=\"Delete\" onClick=\"alert('You may not delete this address because it is being used by an Auto-Ship order. You will need to remove it from all Auto-Ship orders before it may be deleted.')  \"></p>");
      }
			writer.Write("</form>\n");
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");

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
