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
	/// Summary description for popuptx.
	/// </summary>
	public class popuptx : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			
			Customer thisCustomer = new Customer();
			
			Response.Write("<html>\n");
			Response.Write("<head>\n");
			Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1252\">\n");
			Response.Write("<title>Transaction Details For Order #: " + Common.QueryStringUSInt("OrderNumber").ToString() + "</title>\n");
			Response.Write("<link rel=\"stylesheet\" href=\"skins/Skin_" + thisCustomer._skinID.ToString() + "/style.css\" type=\"text/css\">\n");
			Response.Write("<script type=\"text/javascript\" src=\"formValidate.js\"></script>\n");
			Response.Write("</head>\n");
			Response.Write("<body style=\"margin: 0px;\" bottommargin=\"0\" leftmargin=\"0\" marginheight=\"0\" marginwidth=\"0\" rightmargin=\"0\" topmargin=\"0\" bgcolor=\"#FFFFFF\" onLoad=\"self.focus()\">\n");

			bool IsEcheck = false;
			bool IsMicroPay = false;
			bool IsCard = false;

			if(!thisCustomer._isAdminUser)
			{
				Response.Write("<b><font color=red>PERMISSION DENIED</b></font>");
			}
			else
			{
				IDataReader rs = DB.GetRS("Select * from orders  " + DB.GetNoLock() + " where ordernumber=" + Common.QueryStringUSInt("OrderNumber").ToString());
				if(rs.Read())
				{
					IsEcheck = (DB.RSField(rs,"PaymentMethod").ToUpper().Trim() == "ECHECK");
//V3_9
          IsMicroPay = (DB.RSField(rs,"PaymentMethod").ToUpper().Trim() == "MICROPAY");
          IsCard = DB.RSField(rs,"PaymentMethod").Replace(" ","").ToUpper() == "CREDITCARD";
//V3_9
          if(IsEcheck || IsMicroPay || IsCard) 
					{
						Response.Write("<b>Order Number: </b>" + Common.QueryStringUSInt("OrderNumber").ToString() + "<br>");
						Response.Write("<b>Customer ID: </b>" + DB.RSFieldInt(rs,"CustomerID").ToString() + "<br>");
						Response.Write("<b>Order Date: </b>" + DB.RSFieldDateTime(rs,"OrderDate").ToString() + "<br>");
						Response.Write("<b>Order Total: </b>" + Localization.CurrencyStringForDB( DB.RSFieldDecimal(rs,"OrderTotal")) + "<br>");
						Response.Write("<b>Card Type: </b>" + DB.RSField(rs,"CardType") + "<br>");
						Response.Write("<b>Payment Gateway: </b>" + DB.RSField(rs,"PaymentGateway") + "<br>");
						Response.Write("<b>Transaction State: </b>" + DB.RSField(rs,"TransactionState") + "<br>");
						Response.Write("<b>Payment Cleared On: </b>" + Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs,"PaymentClearedOn")) + "<br>");

           
						String _cardNumber = DB.RSField(rs,"CardNumber");
						
						String ctmp = _cardNumber;
						if(_cardNumber.Length != 0 && _cardNumber != "XXXXXXXXXXXX")
						{
							ctmp = Common.UnmungeString(_cardNumber);
						}
						String _cardType = DB.RSField(rs,"CardType");
            
						if (IsEcheck)
						{
							Response.Write(String.Format("<b>ECheck Bank Name: </b> {0}<br>",DB.RSField(rs,"eCheckBankName")));
							Response.Write(String.Format("<b>ECheck ABA: </b> {0}<br>",DB.RSField(rs,"eCheckBankABACode")));
							Response.Write(String.Format("<b>ECheck Account: </b> {0}<br>",DB.RSField(rs,"eCheckBankAccountNumber")));
							Response.Write(String.Format("<b>ECheck Account Name: </b> {0}<br>",DB.RSField(rs,"eCheckBankAccountName")));
							Response.Write(String.Format("<b>ECheck Account Type: </b> {0}<br>",DB.RSField(rs,"eCheckBankAccountType")));
						}
//V3_9
            if (IsMicroPay)
            {
              Response.Write("<b>" + Common.AppConfig("MicroPay.Prompt") + " Transaction:</b>");
            }
//V3_9
						else
						{
							if(_cardType.ToUpper() == "PayPal".ToUpper())
							{
								Response.Write("<b>Card Number: </b>");
							}
							else
							{
								Response.Write("<b>Card Number:</b> " + ctmp + "<br>");
							}
							if(_cardNumber.Length == 0 || _cardNumber == "XXXXXXXXXXXX")
							{
								Response.Write("<b>Card Expiration:</b> N/A<br><,br>>");
							}
							else
							{
								if(_cardType.ToUpper() == "PayPal".ToUpper())
								{
									Response.Write("<b>Card Expiration:</b><br>");
								}
								else
								{
									Response.Write("<b>Card Expiration:</b> " + DB.RSField(rs,"CardExpirationMonth") + "/" + DB.RSField(rs,"cardExpirationYear") + "<br>");
								}
							}
						}
						Response.Write("<b>Transaction Command: </b>" + Server.HtmlEncode(DB.RSField(rs,"TransactionCommand")).Replace(" ","&nbsp;").Replace("\r\n","<br>") + "<br>");
						Response.Write("<b>Authorization Result: </b>" + Server.HtmlEncode(DB.RSField(rs,"AuthorizationResult")).Replace(" ","&nbsp;").Replace("\r\n","<br>") + "<br>");
						Response.Write("<b>Authorization Code: </b>" + Server.HtmlEncode(DB.RSField(rs,"AuthorizationCode")) + "<br>");
						Response.Write("<b>Authorization PNREF (Transaction ID): </b>" + DB.RSField(rs,"AuthorizationPNREF") + "<br>");
						Response.Write("<hr size=1>");
						Response.Write("<b>Capture TX Command: </b>" + Server.HtmlEncode(Common.IIF(DB.RSField(rs,"CaptureTXCommand").Length == 0, "N/A", DB.RSField(rs,"CaptureTXCommand"))).Replace(" ","&nbsp;").Replace("\r\n","<br>") + "<br>");
						Response.Write("<b>Capture TX Result: </b>" + Server.HtmlEncode(Common.IIF(DB.RSField(rs,"CaptureTXResult").Length == 0, "N/A", DB.RSField(rs,"CaptureTXResult"))).Replace(" ","&nbsp;").Replace("\r\n","<br>") + "<br>");
						Response.Write("<b>Void TX Command: </b>" + Server.HtmlEncode(Common.IIF(DB.RSField(rs,"VoidTXCommand").Length == 0, "N/A", DB.RSField(rs,"VoidTXCommand"))).Replace(" ","&nbsp;").Replace("\r\n","<br>") + "<br>");
						Response.Write("<b>Void TX Result: </b>" + Server.HtmlEncode(Common.IIF(DB.RSField(rs,"VoidTXResult").Length == 0, "N/A", DB.RSField(rs,"VoidTXResult"))).Replace(" ","&nbsp;").Replace("\r\n","<br>") + "<br>");
						Response.Write("<b>Refund TX Command: </b>" + Server.HtmlEncode(Common.IIF(DB.RSField(rs,"RefundTXCommand").Length == 0, "N/A", DB.RSField(rs,"RefundTXCommand"))).Replace(" ","&nbsp;").Replace("\r\n","<br>") + "<br>");
						Response.Write("<b>Refund TX Result: </b>" + Server.HtmlEncode(Common.IIF(DB.RSField(rs,"RefundTXResult").Length == 0, "N/A", DB.RSField(rs,"RefundTXResult"))).Replace(" ","&nbsp;").Replace("\r\n","<br>") + "<br>");

						if(Common.AppConfigBool("CardinalCommerce.Centinel.Enabled"))
						{
							Response.Write("<b>Cardinal Lookup Result: </b>" + Server.HtmlEncode(Common.IIF(DB.RSField(rs,"CardinalLookupResult").Length == 0, "N/A", DB.RSField(rs,"CardinalLookupResult"))) + "<br>");
							Response.Write("<b>Cardinal Authenticate Result: </b>" + Server.HtmlEncode(Common.IIF(DB.RSField(rs,"CardinalAuthenticateResult").Length == 0, "N/A", DB.RSField(rs,"CardinalAuthenticateResult"))) + "<br>");
							Response.Write("<b>Cardinal Gateway Parms: </b>" + Server.HtmlEncode(Common.IIF(DB.RSField(rs,"CardinalGatewayParms").Length == 0, "N/A", DB.RSField(rs,"CardinalGatewayParms"))) + "<br>");
						}
						
					}
					else
					{
						Response.Write("N/A");
					}	
				}
				else
				{
					Response.Write("<b><font color=red>ORDER NOT FOUND</b></font>");
				}
				rs.Close();
			}

			Response.Write("<p align=\"center\"><a href=\"javascript:self.close();\">Close</a></p>");
		
			
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
