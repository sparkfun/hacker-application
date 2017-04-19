// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
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
	/// Summary description for captureorder.
	/// </summary>
	public class captureorder : System.Web.UI.Page
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
			Response.Write("<title>Capture Order</title>\n");
			Response.Write("<link rel=\"stylesheet\" href=\"skins/Skin_" + thisCustomer._skinID.ToString() + "/style.css\" type=\"text/css\">\n");
			Response.Write("<script type=\"text/javascript\" src=\"formValidate.js\"></script>\n");
			Response.Write("</head>\n");
			Response.Write("<body style=\"margin: 0px;\" bottommargin=\"0\" leftmargin=\"0\" marginheight=\"0\" marginwidth=\"0\" rightmargin=\"0\" topmargin=\"0\" bgcolor=\"#FFFFFF\" onLoad=\"self.focus()\">\n");
			Response.Write("<div align=\"left\">");
			int ONX = Common.QueryStringUSInt("OrderNumber");

			if(!thisCustomer._isAdminUser)
			{
				Response.Write("<b><font color=red>PERMISSION DENIED</b></font>");
			}
			else
			{
				Response.Write("<b>CAPTURE ORDER: " + ONX.ToString() + "</b><br><br>");
				IDataReader rs = DB.GetRS("Select * from orders  " + DB.GetNoLock() + " where ordernumber=" + ONX.ToString());
				if(rs.Read())
				{
					if(DB.RSField(rs,"PaymentMethod").Replace(" ","").ToUpper() == "CREDITCARD") 
					{
						if(DB.RSFieldDateTime(rs,"PaymentClearedOn") == System.DateTime.MinValue)
						{
							if(DB.RSField(rs,"TransactionState") == "AUTH")
							{
								String GW = DB.RSField(rs,"PaymentGateway").Replace(" ","").ToUpper();
								String Status = String.Empty;
								switch(GW)
								{
									case "EFSNET":
										Status = EFSNet.CaptureOrder(ONX);
										break;
									case "IONGATE":
										Status = IonGate.CaptureOrder(ONX);
										break;
									case "VERISIGN":
										Status = Verisign.CaptureOrder(ONX);
										break;
									case "AUTHORIZENET":
										Status = AuthorizeNet.CaptureOrder(ONX);
										break;
									case "PAYPALPRO":
										Status = PayPalPro.CaptureOrder(ONX);
										break;
									case "2CHECKOUT":
										Status = TwoCheckout.CaptureOrder(ONX);
										break;
									case "EPROCESSINGNETWORK":
										Status = eProcessingNetwork.CaptureOrder(ONX);
										break;
									case "CYBERSOURCE":
										Status = Cybersource.CaptureOrder(ONX);
										break;
									case "NETBILLING":
										Status = NetBilling.CaptureOrder(ONX);
										break;	
									case "PAYMENTECH":
										Status = Paymentech.CaptureOrder(ONX);
										break;
									case "PAYFUSE":
										Status = PayFuse.CaptureOrder(ONX);
										break;
									case "ITRANSACT":
										Status = ITransact.CaptureOrder(ONX);
										break;
									case "WORLDPAY JUNIOR":
										Status = Worldpay.CaptureOrder(ONX);
										break;
									case "WORLDPAY":
										Status = Worldpay.CaptureOrder(ONX);
										break;
									case "LINKPOINT":
										Status = Linkpoint.CaptureOrder(ONX);
										break;
									case "TRANSACTIONCENTRAL":
										Status = TransactionCentral.CaptureOrder(ONX);
										break;
									case "MERCHANTANYWHERE":
										Status = TransactionCentral.CaptureOrder(ONX);
										break;
									case "YOURPAY":
										Status = YourPay.CaptureOrder(ONX);
										break;
									case "MANUAL":
										Response.Write("The MANUAL payment gateway does not support capturing orders.");
										break;
									default:
										Response.Write("Unknown PaymentGateway in CaptureOrder.");
										break;
								}
								IDataReader rs2 = DB.GetRS("Select * from orders  " + DB.GetNoLock() + " where ordernumber=" + ONX.ToString());
								rs2.Read();
								Response.Write("<b>Capture TX Command:</b> " + Server.HtmlEncode(DB.RSField(rs2,"CaptureTXCommand")).Replace(" ","&nbsp;").Replace("\r\n","<br>") + "<br><br>");
								Response.Write("<b>Capture TX Response:</b> " + Server.HtmlEncode(DB.RSField(rs2,"CaptureTXResult")).Replace(" ","&nbsp;").Replace("\r\n","<br>") + "<br><br>");
								rs2.Close();
								Response.Write("<b>Capture Status</b>: " + Status);
								if(Status == "OK")
								{
									DB.ExecuteSQL("update orders set TransactionState=" + DB.SQuote("AUTH CAPTURE") + ", PaymentClearedOn=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + " where OrderNumber=" + ONX.ToString());
								}
							}
							else
							{
								Response.Write("The transaction state (" + DB.RSField(rs,"TransactionState") + ") is not AUTH.");
							}
						}
						else
						{
							Response.Write("The payment for this order was already cleared on " + Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs,"PaymentClearedOn")) + ".");
						}
					}
					else
					{
						Response.Write("The payment type (" + DB.RSField(rs,"PaymentMethod") + ") for this order is not credit card.");
					}	
				}
				else
				{
					Response.Write("<b><font color=red>ORDER NOT FOUND</b></font>");
				}
				rs.Close();
			}

			Response.Write("</div>");

			Response.Write("<p align=\"center\"><a href=\"javascript:self.close();\">Close</a></p>");

			// NEXT TWO LINES ONLY for auth.net certification:
			//String Statuss = AuthorizeNet.CaptureOrder(ONX);
			//Response.Write("AUTH NET CAPTURED");

			
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
