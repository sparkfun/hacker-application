// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
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
	/// Summary description for cst_selectaddress.
	/// </summary>
	public class cst_selectaddress : SkinBase
	{

		private Customer targetCustomer;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			targetCustomer = new Customer(Common.QueryStringUSInt("CustomerID"));
			if(targetCustomer._customerID == 0)
			{
				Response.Redirect("Customers.aspx");
			}
			SectionTitle = "<a href=\"Customers.aspx?searchfor=" + targetCustomer._customerID.ToString() + "\">Customers</a> - Account Info: " + (targetCustomer._firstName + " " + targetCustomer._lastName).Trim() + " (" + targetCustomer._email + ") - " + SectionTitle;
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
      
			bool AllowShipToDifferentThanBillTo = Common.AppConfigBool("AllowShipToDifferentThanBillTo");

			String TabImage = String.Empty;
      
			AddressTypes AddressType = AddressTypes.Unknown;
			String AddressTypeString = Common.QueryString("AddressType");

			AddressType = (AddressTypes)Enum.Parse(typeof(AddressTypes),AddressTypeString,true);
      
			if (AddressType == AddressTypes.Billing) 
			{
				SectionTitle = "Choose a Billing Address";
				TabImage = "selectbillingaddress.gif";
			}
			if (AddressType == AddressTypes.Shipping) 
			{
				SectionTitle = "Choose a Shipping Address";
				TabImage = "selectshippingaddress.gif";
			}

			int OriginalRecurringOrderNumber = Common.QueryStringUSInt("OriginalRecurringOrderNumber");
			string ReturnUrl = Common.QueryString("ReturnUrl");

			// ACCOUNT BOX:
			writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/"+ TabImage + "\" border=\"0\"><br>");
			writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

			writer.Write("</td><tr><td>\n");
			writer.Write("<table width=\"100%\" border=\"0\">");
			writer.Write("<tr>");

			Addresses custAddresses = new Addresses();
			custAddresses.LoadCustomer(targetCustomer._customerID);
			int pos = 0;
			foreach (Address adr in custAddresses)
			{
				writer.Write("<td align=\"left\" valign=\"top\">\n");
				writer.Write(String.Format("<img style=\"cursor:hand;\" src=\"skins/Skin_" + _siteID.ToString() + "/images/usethisaddress.gif\" onClick=\"self.location='cst_selectaddress_process.aspx?CustomerID={0}&AddressType={1}&AddressID={2}&OriginalRecurringOrderNumber={3}&ReturnUrl={4}'\"><br>",targetCustomer._customerID,AddressType,adr.AddressID,OriginalRecurringOrderNumber,ReturnUrl));
				writer.Write(adr.DisplayHTML());
				if (adr.PaymentMethod.Length != 0)
				{
					writer.Write("<b>Payment Method:</b><br>");
					writer.Write(adr.DisplayPaymentMethod + "<br>");
				}
				if (adr.ShippingMethod.Length != 0)
				{
					writer.Write("<b>Shipping Method:</b><br>");
					writer.Write(adr.ShippingMethod + "<br>");
				}
				writer.Write(String.Format("<img style=\"cursor:hand;\" src=\"skins/Skin_" + _siteID.ToString() + "/images/edit2.gif\" onClick=\"self.location='cst_editaddress.aspx?CustomerID={0}&AddressType={1}&AddressID={2}&ReturnUrl={3}'\"><br><br>",targetCustomer._customerID,AddressType,adr.AddressID,ReturnUrl));
				writer.Write("</td>");
        
				pos++;
				if ((pos % 2) == 0)
				{
					writer.Write("</tr><tr>");
				}
			}
      
			writer.Write("</tr></table>");

			writer.Write("</td></tr>");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
      
			// ADDRESS BOX:

			Address newAddress = new Address(); 
			newAddress.AddressType = AddressType;

			String act = String.Format("cst_selectaddress_process.aspx?CustomerID={0}&AddressType={1}&ReturnUrl={2}",targetCustomer._customerID,AddressType,ReturnUrl);
			if (OriginalRecurringOrderNumber != 0)
			{
				act += String.Format("OriginalRecurringOrderNumber={0}&",OriginalRecurringOrderNumber);
			}

			writer.Write("<form method=\"POST\" action=\"" + act + "\" name=\"SelectAddressForm\" id=\"SelectAddressForm\" onSubmit=\"return (validateForm(this) && AddressInputForm_Validator(this))\">");
 
			writer.Write(String.Format("<hr><b>Or Enter a New {0} Address</b><hr>",AddressType));

			//Display the Address input form fields
			writer.Write(newAddress.InputHTML());

//			if ((AddressType & AddressTypes.Billing) != 0)
//			{
//				if (newAddress.PaymentMethod.ToUpper() == "ECHECK")
//				{
//					writer.Write(newAddress.InputECheckHTML(false));
//				}
//				else
//				{
//					writer.Write(newAddress.InputCardHTML(false));
//				}
//			}

			//Button to submit the form
			writer.Write("<p align=\"center\"><input type=\"submit\" value=\"Add New Address\" name=\"Continue\"></p>");

			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");
			writer.Write("</form>");
			writer.Write("</td></tr>\n");
			writer.Write("</table>\n");
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
