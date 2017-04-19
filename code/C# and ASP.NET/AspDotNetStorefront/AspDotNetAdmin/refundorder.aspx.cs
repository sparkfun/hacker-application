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
using AspDotNetStorefrontGateways;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for refundorder.
	/// </summary>
	public class refundorder : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");
			
			Customer thisCustomer = new Customer();
			
			Response.Write("<html>\n");
			Response.Write("<head>\n");
			Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1252\">\n");
			Response.Write("<title>Refund Order</title>\n");
			Response.Write("<link rel=\"stylesheet\" href=\"skins/Skin_" + thisCustomer._skinID.ToString() + "/style.css\" type=\"text/css\">\n");
			Response.Write("<script type=\"text/javascript\" src=\"formValidate.js\"></script>\n");
			Response.Write("</head>\n");
			Response.Write("<body style=\"margin: 0px;\" bottommargin=\"0\" leftmargin=\"0\" marginheight=\"0\" marginwidth=\"0\" rightmargin=\"0\" topmargin=\"0\" bgcolor=\"#FFFFFF\" onLoad=\"self.focus()\">\n");
			Response.Write("<div align=\"left\">");

			if(!thisCustomer._isAdminUser)
			{
				Response.Write("<b><font color=red>PERMISSION DENIED</b></font>");
			}
			else
			{
				int ONX = Common.QueryStringUSInt("OrderNumber");
				Response.Write("<b>REFUND ORDER: " + ONX.ToString() + "</b><br><br>");
				if(Common.QueryString("Confirm") == "yes")
				{
					IDataReader rs = DB.GetRS("Select * from orders  " + DB.GetNoLock() + " where ordernumber=" + ONX.ToString());
					if(rs.Read())
					{
						string PMethod = DB.RSField(rs,"PaymentMethod").Replace(" ","").ToUpper();
						if((PMethod == "CREDITCARD") || (PMethod == "MICROPAY")) 
						{
							if(DB.RSFieldDateTime(rs,"PaymentClearedOn") != System.DateTime.MinValue)
							{
								if(DB.RSField(rs,"TransactionState") == "AUTH CAPTURE")
								{
									String GW = DB.RSField(rs,"PaymentGateway").Replace(" ","").ToUpper();
									String Status = String.Empty;
									switch(GW)
									{
										case "EFSNET":
											Status = EFSNet.RefundOrder(ONX);
											break;
										case "IONGATE":
											Status = IonGate.RefundOrder(ONX);
											break;
										case "VERISIGN":
											Status = Verisign.RefundOrder(ONX);
											break;
										case "AUTHORIZENET":
											Status = AuthorizeNet.RefundOrder(ONX);
											break;
										case "PAYPALPRO":
											Status = PayPalPro.RefundOrder(ONX);
											break;
										case "2CHECKOUT":
											Status = TwoCheckout.RefundOrder(ONX);
											break;
										case "EPROCESSINGNETWORK":
											Status = eProcessingNetwork.RefundOrder(ONX);
											break;
										case "CYBERSOURCE":
											Status = Cybersource.RefundOrder(ONX);
											break;
										case "NETBILLING":
											Status = NetBilling.RefundOrder(ONX);
											break;
										case "PAYMENTECH":
											Status = Paymentech.RefundOrder(ONX);
											break;
										case "PAYFUSE":
											Status = PayFuse.RefundOrder(ONX);
											break;
										case "ITRANSACT":
											Status = ITransact.RefundOrder(ONX);
											break;
										case "MICROPAY":
											Status = Micropay.RefundOrder(ONX);
											break;
										case "WORLDPAY JUNIOR":
											Status = Worldpay.RefundOrder(ONX);
											break;
										case "WORLDPAY":
											Status = Worldpay.RefundOrder(ONX);
											break;
										case "LINKPOINT":
											Status = Linkpoint.RefundOrder(ONX);
											break;
										case "TRANSACTIONCENTRAL":
											Status = TransactionCentral.RefundOrder(ONX);
											break;
										case "MERCHANTANYWHERE":
											Status = TransactionCentral.RefundOrder(ONX);
											break;
										case "YOURPAY":
											Status = YourPay.RefundOrder(ONX);
											break;
										case "MANUAL":
											Response.Write("The MANUAL payment gateway does not support refunding orders.");
											break;
										default:
											Response.Write("Unknown PaymentGateway in RefundOrder.");
											break;
									}
									Response.Write("Refund Status: " + Status);
									if(Status == "OK")
									{
										DB.ExecuteSQL("update orders set transactionstate=" + DB.SQuote("REFUNDED") + ", PaymentClearedOn=NULL where ordernumber=" + ONX.ToString());
									}

								}
								else
								{
									Response.Write("The transaction state (" + DB.RSField(rs,"TransactionState") + ") is not AUTH CAPTURE.");
								}
							}
							else
							{
								Response.Write("The payment for this order has not yet been cleared.");
							}
						}
						else
						{
							Response.Write("The payment type (" + DB.RSField(rs,"PaymentMethod") + ") for this order is not Credit Card or " + Common.AppConfig("MicroPay.Prompt") + ".");
						}	
					}
					else
					{
						Response.Write("<b><font color=red>ORDER NOT FOUND</b></font>");
					}
					rs.Close();
				}
				else
				{
					Response.Write("<p align=\"center\">Are you sure you want to refund this order?<br><br></p>");
					Response.Write("<p align=\"center\"><a href=\"refundorder.aspx?ordernumber=" + ONX.ToString() + "&confirm=yes\">Yes</a><img src=\"images/spacer.gif\" width=\"100\" height=\"1\"><a href=\"javascript:self.close();\">No</a></p>");
				}
			}

			Response.Write("</div>");

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
