// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
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

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for selectaddress.
	/// </summary>
	public class selectaddress : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			RequireSecurePage();
			RequiresLogin(Common.GetThisPageName(false) + "?" + Common.ServerVariables("QUERY_STRING"));
			SectionTitle = String.Format("Choose a {0} Address",Common.QueryString("AddressType"));
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");
			
			Customer thisCustomer = new Customer();
			thisCustomer.RequireCustomerRecord();

			bool AllowShipToDifferentThanBillTo = Common.AppConfigBool("AllowShipToDifferentThanBillTo");

			if(Common.AppConfigBool("UseSSL"))
			{
				if(Common.OnLiveServer() && Common.ServerVariables("SERVER_PORT_SECURE") != "1")
				{
					Response.Redirect(Common.GetStoreHTTPLocation(true) + "selectaddress.aspx?" + Common.ServerVariables("QUERY_STRING"));
				}
			}

			String TabImage = String.Empty;
      
			AddressTypes AddressType = AddressTypes.Unknown;
			String AddressTypeString = Common.QueryString("AddressType");

			if (AddressTypeString.Length != 0)
			{
				AddressType = (AddressTypes)Enum.Parse(typeof(AddressTypes),AddressTypeString,true);
			}

			String Prompt = String.Empty;
			Prompt = String.Format("<b>Choose a {0} address from your address book below:</b>",AddressType);
			TabImage = "addressbook.gif";
			int OriginalRecurringOrderNumber = Common.QueryStringUSInt("OriginalRecurringOrderNumber");
			String ReturnURL = Common.QueryString("ReturnURL");

			Prompt += " or <a href=\"selectaddress.aspx?add=true&addressType=" + Common.QueryString("AddressType") + "&returnURL=" + Server.UrlEncode(ReturnURL) + "\"><b>Add A New Address</b></a>";



			writer.Write("<p><b>" + Prompt + "</b></p>");

			// ADDRESS BOX:
			writer.Write("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
			writer.Write("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/"+ TabImage + "\" border=\"0\"><br>");
			writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			writer.Write("<tr><td align=\"left\" valign=\"top\">\n");

			Addresses custAddresses = new Addresses();
			custAddresses.LoadCustomer(thisCustomer._customerID);
			if(custAddresses.Count > 0)
			{
				writer.Write("<table width=\"100%\">");
				bool first = true;
				foreach(Address adr in custAddresses)
				{
					if(!first)
					{
						writer.Write("<tr><td colspan=2><hr size=1></td></tr>");
					}
					writer.Write("<tr>");
					writer.Write("<td width=\"150\" align=\"left\" valign=\"top\">");
					writer.Write(String.Format("<img src=\"images/spacer.gif\" width=\"25\" height=\"1\"><img style=\"cursor:hand;\" src=\"skins/Skin_" + _siteID.ToString() + "/images/usethisaddress.gif\" onClick=\"self.location='selectaddress_process.aspx?AddressType={0}&AddressID={1}&OriginalRecurringOrderNumber={2}&ReturnURL={3}'\"><br>",AddressType,adr.AddressID,OriginalRecurringOrderNumber,ReturnURL));
					writer.Write("</td>");
					writer.Write("<td align=\"left\" valign=\"top\">");
					writer.Write(adr.DisplayHTML());
					if (adr.PaymentMethod.Length != 0)
					{
						writer.Write("<b>Payment Method:</b><br>");
						writer.Write(adr.DisplayPaymentMethod + "<br>");
					}
					if (adr.ShippingMethod.Length != 0)
					{
						writer.Write("<b>Shipping Method:</b><br>");
						writer.Write(adr.ShippingMethod.Split('|')[0] + "<br>");
					}
					writer.Write(String.Format("<img style=\"cursor:hand;\" src=\"skins/Skin_" + _siteID.ToString() + "/images/edit2.gif\" onClick=\"self.location='editaddress.aspx?AddressType={0}&AddressID={1}&ReturnURL={2}'\">",AddressType,adr.AddressID,ReturnURL));
					writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<img style=\"cursor:hand;\" src=\"skins/Skin_" + _siteID.ToString() + "/images/delete.gif\" onClick=\"if(confirm('Are you sure you want to delete this address? This action cannot be undone!')) {self.location='editaddress_process.aspx?AddressType=" + AddressType.ToString() + "&AddressID=" + adr.AddressID.ToString() + "&DeleteAddressID=" + adr.AddressID.ToString() + "&ReturnURL=" + Server.UrlEncode(ReturnURL) + "';}\"><br><br>");
					writer.Write("</td>");
					writer.Write("</tr>");
					first = false;
				}
				writer.Write("</table>");
			}

			writer.Write("</td></tr>");

			if(Common.QueryString("add").Length != 0)
			{
				writer.Write("<tr><td align=\"left\" valign=\"top\">\n");
      
				// ADDRESS BOX:

				Address newAddress = new Address(); 
				newAddress.AddressType = AddressType;

				String act = String.Format("selectaddress_process.aspx?AddressType={0}&ReturnUrl={1}",AddressType,ReturnURL);
				if (OriginalRecurringOrderNumber != 0) {act += String.Format("OriginalRecurringOrderNumber={0}&",OriginalRecurringOrderNumber);}
				writer.Write("<form method=\"POST\" action=\"" + act + "\" name=\"SelectAddressForm\" id=\"SelectAddressForm\" onSubmit=\"return (validateForm(this) && AddressInputForm_Validator(this))\">");
 
				writer.Write(String.Format("<hr><b>Create a New {0} Address</b><hr>",AddressType));

				//Display the Address input form fields
				writer.Write(newAddress.InputHTML());

				//Button to submit the form
				writer.Write("<p align=\"center\"><input type=\"submit\" value=\"Add New Address\" name=\"Continue\"></p>");

				writer.Write("</form>");
				writer.Write("</td></tr>\n");
			}
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
