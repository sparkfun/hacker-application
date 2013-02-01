// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;
using System.Web.SessionState;
using System.Web.Caching;
using System.Web.Handlers;
using System.Web.Hosting;
using System.Web.Mail;
using System.Web.Security;
using System.Web.UI;
using System.Web.Util;
using System.Data;
using System.Globalization;
using System.Text;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for orderframe.
	/// </summary>
	public class orderframe : SkinBase
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Order Summary";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			int OrderNumber = Common.QueryStringUSInt("OrderNumber");
			writer.Write("<html>\n");
			writer.Write("<head>\n");
			writer.Write("<title>AspDotNetStorefront Order Detail</title>\n");
			writer.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\">\n");
			writer.Write("<link rel=\"stylesheet\" href=\"skins/Skin_" + _siteID.ToString() +  "/style.css\" type=\"text/css\">\n");
			writer.Write("</head>\n");
			writer.Write("<body bgcolor=\"#FFFFFF\" topmargin=\"0\" marginheight=\"0\" bottommargin=\"0\" marginwidth=\"0\" rightmargin=\"0\">\n");
			writer.Write("<script type=\"text/javascript\" src=\"formValidate.js\"></script>\n");

			String InitialTab = Common.QueryString("InitialTab");
			if(InitialTab.Length == 0)
			{
				InitialTab = "General";
			}

			if(!thisCustomer._isAdminUser)
			{
				writer.Write("<p><b>INSUFFICIENT PERMISSIONS</b></p>");
			}
			else
			{

				if(Common.Form("SubmitAction").Length != 0)
				{
					String MailServer = Common.AppConfig("MailMe_Server");
							
					if(Common.Form("SubmitAction").ToUpper() == "UPDATENOTES")
					{
						DB.ExecuteSQL("Update orders set CustomerServiceNotes=" + DB.SQuote(Common.Form("CustomerServiceNotes")) + " where OrderNumber=" + OrderNumber.ToString());
						InitialTab = "Notes";
					}
					
					if(Common.Form("SubmitAction").ToUpper() == "CLEARNEW")
					{
						DB.ExecuteSQL("Update orders set IsNew=0 where OrderNumber=" + OrderNumber.ToString());
					}
						
					if(Common.Form("SubmitAction").ToUpper() == "MARKASSHIPPED")
					{
						String ShippedVIA = Common.Form("ShippedVIA").Trim(", ".ToCharArray());
						String ShippingTrackingNumber = Common.Form("ShippingTrackingNumber").Trim(",".ToCharArray());
						Common.MarkOrderAsShipped(OrderNumber,ShippedVIA,ShippingTrackingNumber,false);
						InitialTab = "Shipping";
					}

						
					if(Common.Form("SubmitAction").ToUpper() == "DELETEORDER")
					{
						Common.DeleteOrder(OrderNumber);
					}
						
					if(Common.AppConfigBool("ShipRush.Enabled"))
					{
						if(Common.Form("SubmitAction").ToUpper() == "SHIPRUSH")
						{
							// make sure this AspDotNetStorefront "administrator" is defined as a ShipRush User:
							DBTransaction trans = new DBTransaction();
							String ShipRushLoginID = Common.AppConfig("ShipRush.LoginID").Trim();
							if(ShipRushLoginID.Length == 0)
							{
								if(DB.GetSqlN("select count(*) as N from OR_Users  " + DB.GetNoLock() + " where Login=" + DB.SQuote(thisCustomer._customerID.ToString())) == 0)
								{
									IDataReader rsc = DB.GetRS("Select * from customer  " + DB.GetNoLock() + " where customerid=" + DB.SQuote(thisCustomer._customerID.ToString()));
									rsc.Read();
									trans.AddCommand("insert into OR_Users(Login,FirstName,LastName,EMail,Phone,FaxNo) values(" + thisCustomer._customerID.ToString() + "," + DB.SQuote(Common.Left(DB.RSField(rsc,"FirstName"),25)) + "," + DB.SQuote(Common.Left(DB.RSField(rsc,"LastName"),25)) + "," + DB.SQuote(Common.Left(DB.RSField(rsc,"EMail"),60)) + "," + DB.SQuote(DB.RSField(rsc,"Phone").Replace(" ","").Replace("-","").Replace(".","").Replace("(","").Replace(")","").Replace("+","")) + ",NULL)");
									rsc.Close();
								}
								ShipRushLoginID = thisCustomer._customerID.ToString();
							}
							Order ord = new Order(OrderNumber);
							// create the ShipRush Contact record for this "shipment":
							String ContactID = "OrderNumber_" + OrderNumber.ToString(); //DB.GetNewGUID().Replace("{","").Replace("}","").Replace("-","");
							String JobID = "OrderNumber_" + OrderNumber.ToString(); //DB.GetNewGUID().Replace("{","").Replace("}","").Replace("-","");
							String sql = "insert into OR_Contacts(ContactID,Company,FirstName,MiddleName,LastName,EMail,Phone,FaxNo,Address1,Address2,City,State,ZIP,Country) values(";
							sql += DB.SQuote(Common.Left(DB.SQuote(ContactID),40)) + ",";
							sql += DB.SQuote(Common.Left(ord._shippingAddress.company,25)) + ",";
							sql += DB.SQuote(Common.Left(ord._shippingAddress.firstName,25)) + ",";
							sql += "NULL,";
							sql += DB.SQuote(Common.Left(ord._shippingAddress.lastName,25)) + ",";
							sql += DB.SQuote(Common.Left(ord._shippingAddress.email,60)) + ",";
							sql += DB.SQuote(Common.Left(ord._shippingAddress.phone,30)) + ",";
							sql += "NULL,";
							sql += DB.SQuote(Common.Left(ord._shippingAddress.address1,60)) + ",";
							sql += DB.SQuote(Common.Left(ord._shippingAddress.address2,60)) + ",";
							sql += DB.SQuote(Common.Left(ord._shippingAddress.city,30)) + ",";
							sql += DB.SQuote(Common.Left(ord._shippingAddress.state,20)) + ",";
							sql += DB.SQuote(Common.Left(ord._shippingAddress.zip,10)) + ",";
							sql += DB.SQuote(Common.Left(ord._shippingAddress.country,20));
							sql += ")";
							trans.AddCommand(sql);

							// create the ShipRush JOB record for this shipment:
							String ShipRushTemplate = String.Empty;
							IDataReader rssm = DB.GetRS("Select ShipRushTemplate from shippingmethod  " + DB.GetNoLock() + " where shippingmethodid=" + ord._shippingMethodID.ToString());
							if(rssm.Read())
							{
								ShipRushTemplate = DB.RSField(rssm,"ShipRushTemplate");
							}
							rssm.Close();
							if(ShipRushTemplate.Length == 0)
							{
								ShipRushTemplate = Common.AppConfig("ShipRush.DefaultTemplate");
							}
							String ShipRushReference = ShipRushTemplate;
							if(Common.AppConfigBool("ShipRush.ProvideExtraTemplateInfo"))
							{
								ShipRushReference += " WGT:" + ord._orderWeight.ToString() + " VAL:" + Localization.CurrencyStringForGateway(ord.Total(true)) + " // Order #: " + OrderNumber.ToString();
							}
																
							sql = "insert into OR_JOBS(JobID,ContactID,Reference,Notes,TrackRef,UserLastBy) values(";
							sql += DB.SQuote(Common.Left(DB.SQuote(JobID),40)) + ",";
							sql += DB.SQuote(Common.Left(DB.SQuote(ContactID),40)) + ",";
							sql += DB.SQuote(Common.Left(ShipRushReference,60)) + ",";
							sql += DB.SQuote("Order Number: " + OrderNumber.ToString()) + ",";
							sql += DB.SQuote(Common.Left(Common.AppConfig("ShipRush.TrackRef"),15)) + ",";
							sql += DB.SQuote(Common.Left(ShipRushLoginID,15));
							sql += ")";
							trans.AddCommand(sql);
							ord = null;

							if(trans.Commit())
							{
								DB.ExecuteSQL("Update orders set IsNew=0, ShippedVIA=" + DB.SQuote("ShipRush") + ", ShippingTrackingNumber=" + DB.SQuote("See ShipRush") + ", ShippedOn=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + " where OrderNumber=" + OrderNumber.ToString());
							}
							trans = null;
						}
						InitialTab = "Shipping";
					}
								
					if(Common.Form("SubmitAction").ToUpper() == "CLEARORDER")
					{
						Order order = new Order(OrderNumber);
						// mark payment cleared:
						DB.ExecuteSQL("update orders set PaymentClearedOn=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + " where OrderNumber=" + OrderNumber.ToString());

						//Check if they bought MicroPay
						Common.MicroPayBalanceUpdate(OrderNumber); 

						//Check if they have any subscription extension products
						Common.SubscriptionUpdate(OrderNumber); 

						//Common.SendOrderEMail(thisCustomer,order._orderNumber,false,order._paymentMethod,false);

						InitialTab = "Billing";
						if(order.HasDownloadComponents() && !Common.AppConfigBool("DelayedDownloads"))
						{
							bool downloadListSent = false;
							foreach(CartItem i in order._cartItems)
							{
								if(i.isDownload)
								{
									if(i.isSecureAttachment)
									{
#if SECUREATTACHMENT
									try
									{
										SecureAttachment.Credentials c = new SecureAttachment.Credentials();
										c.Username = Common.AppConfig("SecureAttachment.Username"); 
										c.Password = Common.AppConfig("SecureAttachment.Password");
						
										String inputFileName = Server.MapPath(i.downloadLocation);
										// First, set our credentials

										//Create and set the document information object
										SecureAttachment.DocumentInfo di = new SecureAttachment.DocumentInfo();
										di.Filename = i.productName + ".pdf";
										di.NotifyWhenAllLinksUsed = true;
										di.DocumentFormat = Common.AppConfig("SecureAttachment.DocumentFormat");;

										// try opening the document file
										System.IO.FileStream inFile = new System.IO.FileStream(inputFileName,System.IO.FileMode.Open,System.IO.FileAccess.Read);
										// allocate a buffer for the document and fill with file content
										di.DocumentBytes = new byte[inFile.Length];
										inFile.Read(di.DocumentBytes, 0, Convert.ToInt32(inFile.Length));
										inFile.Close();

										// Define PDFM options
										SecureAttachment.PDFMOptions Options = new SecureAttachment.PDFMOptions();
										Options.CanCopyPaste = false;
										Options.CanPrint = false;

										// Create a document submission object
										SecureAttachment.SADocument doc = new SecureAttachment.SADocument();
										doc.AllowAutoRedirect = true;
										doc.UserAgent = Common.AppConfig("SecureAttachment.UserAgent");

										// This will upload the document to the server:
										string DocID = doc.SAUserRegisterDocumentPDFM(c, di, Options);

										// Now create a distribution request for sender;
										SecureAttachment.DistributionRequest dr = new SecureAttachment.DistributionRequest();
										dr.EMailFormat = SecureAttachment.EMailFormatEnum.EMailFormat_HTML;
										dr.Request = new SecureAttachment.HyperlinkRequest();
										dr.Request.NotifyWhenOneUsed = true;
										dr.EMailSubject = Common.AppConfig("SecureAttachment.EMailSubject");
										String SS = String.Empty;
										dr.EMailBody = Common.GetTopicWithoutSite("SecureAttachment.EMailBody", out SS);

										// Now create a list of recipients:
										string[] Recipients = new string[1];
										Recipients[0] = order._shippingAddress.email;

										// Submit request to SecureAttachment Server
										SecureAttachment.DistributionInfo[] disti = new SecureAttachment.DistributionInfo[1];
										disti = doc.SAUserDistributeDocumentSimple(c, DocID, Recipients, dr, true);
									}
									catch (System.Exception ex)
									{
										Response.Write("FILE ERROR: " + Common.GetExceptionDetail(ex,"<br>") + "<br><br>");
									}
#endif
									}
									else
									{
										// only send download list once for non SA items, as order.getdownloadlist(true) sends a links to ALL non SA items at one time:
										if(!downloadListSent)
										{
											try
											{
												Topic t1 = new Topic("DownloadFooter");
												String SubjectReceipt = String.Format("{0} Receipt",Common.AppConfig("StoreName"));
												Common.SendMail(SubjectReceipt.Replace("Receipt","Download Instructions"), order.GetDownloadList(true) + t1._contents + "<br><br>" + Common.AppConfig("MailFooter"), true, Common.AppConfig("ReceiptEMailFrom"),Common.AppConfig("ReceiptEMailFromName"),order._billingAddress.email,order._billingAddress.email,"",MailServer);
											}
											catch {}
											downloadListSent = true;
										}
									}
								}
							}
							// update db so we know this order download list(s) were sent:
							if(order.HasDownloadComponents())
							{
								DB.ExecuteSQL("update Orders set DownloadEmailSentOn=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + " where OrderNumber=" + order._orderNumber.ToString());
							}						
						}
					}
						
					if(Common.Form("SubmitAction").ToUpper() == "SENDRECEIPTEMAIL")
					{
						Order order = new Order(OrderNumber);
						if(MailServer.Length != 0 && MailServer.ToUpper() != "TBD")
						{
							String SubjectReceipt = String.Empty;
							if(Common.AppConfig("UseLiveTransactions").ToUpper() == "TRUE")
							{
								SubjectReceipt = Common.AppConfig("StoreName") + " Receipt";
							}
							else
							{
								SubjectReceipt = Common.AppConfig("StoreName") + " Receipt (TEST)";
							}
							if(order._paymentMethod.ToUpper() == "REQUEST QUOTE")
							{
								SubjectReceipt += " (REQUEST FOR QUOTE)";
							}
							try
							{
								Common.SendMail(SubjectReceipt, order.Receipt(thisCustomer._customerID,Common.GetActiveReceiptTemplate(order._siteID),"",true) + Common.AppConfig("MailFooter"), true, Common.AppConfig("ReceiptEMailFrom"),Common.AppConfig("ReceiptEMailFromName"),order._email,order._email,String.Empty,MailServer);
							}
							catch {}
							DB.ExecuteSQL("update Orders set ReceiptEmailSentOn=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + " where OrderNumber=" + OrderNumber.ToString());
						}
						InitialTab = "General";
					}

					if(Common.Form("SubmitAction").ToUpper() == "SENDDOWNLOADEMAIL")
					{
						Order order = new Order(OrderNumber);
						if(order.HasDownloadComponents() && order.ThereAreDownloadFilesSpecified())
						{
							String SubjectReceipt = String.Empty;
							if(Common.AppConfig("UseLiveTransactions").ToUpper() == "TRUE")
							{
								SubjectReceipt = Common.AppConfig("StoreName") + " Receipt";
							}
							else
							{
								SubjectReceipt = Common.AppConfig("StoreName") + " Receipt (TEST)";
							}
							if(order._paymentMethod.ToUpper() == "REQUEST QUOTE")
							{
								SubjectReceipt += " (REQUEST FOR QUOTE)";
							}
							if(MailServer.Length != 0 && MailServer.ToUpper() != "TBD")
							{
								try
								{
									Common.SendMail(SubjectReceipt.Replace("Receipt","Download Instructions"), order.GetDownloadList(true) + Common.AppConfig("MailFooter"), true, Common.AppConfig("ReceiptEMailFrom"),Common.AppConfig("ReceiptEMailFromName"),order._email,order._email,String.Empty,MailServer);
								}
								catch {}
								DB.ExecuteSQL("update Orders set DownloadEmailSentOn=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + " where OrderNumber=" + OrderNumber.ToString());
							}
						}
						else
						{
							writer.Write("<p><b>NO DOWNLOAD ITEMS OR NO DOWNLOAD FILES SPECIFIED. DOWNLOAD E-MAIL NOT SENT.</b></p>");
						}
						InitialTab = "General";
					}
				}
					
				if(DB.GetSqlN("select count(*) as N from orders  " + DB.GetNoLock() + " where OrderNumber=" + OrderNumber.ToString()) == 0)
				{
					writer.Write("<p><b>ORDER NOT FOUND or ORDER HAS BEEN DELETED</b></p>");
				}
				else
				{
					IDataReader rs = DB.GetRS("Select * from orders  " + DB.GetNoLock() + " where ordernumber=" + OrderNumber.ToString());
					rs.Read();
					bool IsNew = DB.RSFieldBool(rs,"IsNew");
					bool HasShippableComponents = Common.OrderHasShippableComponents(DB.RSFieldInt(rs,"OrderNumber"));
					bool PaymentCleared = (DB.RSFieldDateTime(rs,"PaymentClearedOn") != System.DateTime.MinValue);
					String GW = DB.RSField(rs,"PaymentGateway").Trim().ToUpper();
					String TXState = DB.RSField(rs,"TransactionState");
					String PMethod = DB.RSField(rs,"PaymentMethod").Replace(" ","").ToUpper();
					bool IsCard = (PMethod == "CREDITCARD");
					bool IsCheck = (PMethod == "CHECK");
					bool IsMicroPay = (PMethod == "MICROPAY");
					bool IsEcheck = (PMethod == "ECHECK");
					String bgColor = "FFFFFF";
					//			if(DB.RSFieldBool(rs,"IsNew"))
					//			{
					//				bgColor = "FFFF44";
					//			}
					//if(IsCheck && !PaymentCleared)
					//{
					//	bgColor = "EEAAAA";
					//}
					Order ord = new Order(OrderNumber);

					String ShipAddr = (ord._shippingAddress.firstName + " " + ord._shippingAddress.lastName).Trim() + "<br>";
					ShipAddr += ord._shippingAddress.address1;
					if(ord._shippingAddress.address2.Length != 0)
					{
						ShipAddr += "<br>" + ord._shippingAddress.address2;
					}
					if(ord._shippingAddress.suite.Length != 0)
					{
						ShipAddr += ", " + ord._shippingAddress.suite;
					}
					ShipAddr += "<br>" + ord._shippingAddress.city + ", " + ord._shippingAddress.state + " " + ord._shippingAddress.zip;
					ShipAddr += "<br>" + ord._shippingAddress.country.ToUpper();
					ShipAddr += "<br>" + ord._shippingAddress.phone;


					String BillAddr = (ord._billingAddress.firstName + " " + ord._billingAddress.lastName).Trim() + "<br>";
					BillAddr += ord._billingAddress.address1;
					if(ord._billingAddress.address2.Length != 0)
					{
						BillAddr += "<br>" + ord._billingAddress.address2;
					}
					if(ord._billingAddress.suite.Length != 0)
					{
						BillAddr += ", " + ord._billingAddress.suite;
					}
					BillAddr += "<br>" + ord._billingAddress.city + ", " + ord._billingAddress.state + " " + ord._billingAddress.zip;
					BillAddr += "<br>" + ord._billingAddress.country.ToUpper();
					BillAddr += "<br>" + ord._billingAddress.phone;

					String ReceiptURL = "../receipt.aspx?ordernumber=" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "&customerid=" + DB.RSFieldInt(rs,"CustomerID").ToString() + "&pwd=" + HttpContext.Current.Server.UrlEncode(Common.AppConfig("OrderShowCCPwd"));
					String ReceiptURLPrintable = ReceiptURL + "&nocc=true";

					writer.Write("<b>Order # " + OrderNumber.ToString());
					if(IsNew)
					{
						writer.Write("&nbsp;<img style=\"cursor:hand;\" alt=\"Clear IsNew Flag\" onClick=\"ClearNew(OrderDetailForm," + OrderNumber.ToString() + "," + Common.IIF(DB.RSFieldDateTime(rs,"PaymentClearedOn") != System.DateTime.MinValue, "1", "0") + ");\" src=\"skins/skin_" + _siteID.ToString() + "/images/new.gif\" align=\"absmiddle\" border=\"0\"></a>");
					}

					writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<input style=\"font-size: 9px;\" type=\"button\" value=\"Delete\" name=\"Delete" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "\" onClick=\"DeleteOrder(OrderDetailForm," + DB.RSFieldInt(rs,"OrderNumber").ToString() + ");\">");


					writer.Write("<form method=\"POST\" action=\"orderframe.aspx?ordernumber=" + OrderNumber.ToString() + "\" id=\"OrderDetailForm\" name=\"OrderDetailForm\" >");
					writer.Write("<input type=\"hidden\" name=\"SubmitAction\" value=\"\">\n");

					writer.Write("\n<script type=\"text/javascript\" Language=\"JavaScript\">\n");
					writer.Write("function PopupTX(ordernumber)\n");
					writer.Write("{\n");
					writer.Write("window.open('popuptx.aspx?ordernumber=' + ordernumber,'PopupTX" + Common.GetRandomNumber(1,100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,copyhistory=no,width=600,height=500,left=0,top=0');\n");
					writer.Write("return (true);\n");
					writer.Write("}\n");
					writer.Write("</script>\n");

					writer.Write("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\n");
					writer.Write("<tr>\n");
					writer.Write("<td id=\"GeneralTD_" + OrderNumber.ToString() + "\" width=90 height=22 align=\"center\" valign=\"bottom\"><a href=\"javascript:void(0);\" onClick=\"ShowGeneralDiv_" + OrderNumber.ToString() + "();\" >General</a><br><img src=\"images/spacer.gif\" height=\"2\" width=\"2\" border=\"0\"></td>\n");
					writer.Write("<td id=\"BillingTD_" + OrderNumber.ToString() + "\" width=90 height=22 align=\"center\" valign=\"bottom\"><a href=\"javascript:void(0);\" onClick=\"ShowBillingDiv_" + OrderNumber.ToString() + "();\" >Billing</a><br><img src=\"images/spacer.gif\" height=\"2\" width=\"2\" border=\"0\"></td>\n");
					writer.Write("<td id=\"ShippingTD_" + OrderNumber.ToString() + "\" width=90 height=22 align=\"center\" valign=\"bottom\"><a href=\"javascript:void(0);\" onClick=\"ShowShippingDiv_" + OrderNumber.ToString() + "();\" >Shipping</a><br><img src=\"images/spacer.gif\" height=\"2\" width=\"2\" border=\"0\"></td>\n");
					writer.Write("<td id=\"CustomerTD_" + OrderNumber.ToString() + "\" width=90 height=22 align=\"center\" valign=\"bottom\"><a href=\"javascript:void(0);\" onClick=\"ShowCustomerDiv_" + OrderNumber.ToString() + "();\" >Customer</a><br><img src=\"images/spacer.gif\" height=\"2\" width=\"2\" border=\"0\"></td>\n");
					writer.Write("<td id=\"NotesTD_" + OrderNumber.ToString() + "\" width=90 height=22 align=\"center\" valign=\"bottom\"><a href=\"javascript:void(0);\" onClick=\"ShowNotesDiv_" + OrderNumber.ToString() + "();\" >Notes</a><br><img src=\"images/spacer.gif\" height=\"2\" width=\"2\" border=\"0\"></td>\n");
					writer.Write("<td id=\"ReceiptTD_" + OrderNumber.ToString() + "\" width=90 height=22 align=\"center\" valign=\"bottom\"><a href=\"javascript:void(0);\" onClick=\"ShowReceiptDiv_" + OrderNumber.ToString() + "();\" >Receipt</a><br><img src=\"images/spacer.gif\" height=\"2\" width=\"2\" border=\"0\"></td>\n");
					writer.Write("<td id=\"XMLTD_" + OrderNumber.ToString() + "\" width=90 height=22 align=\"center\" valign=\"bottom\"><a href=\"javascript:void(0);\" onClick=\"ShowXMLDiv_" + OrderNumber.ToString() + "();\" >XML</a><br><img src=\"images/spacer.gif\" height=\"2\" width=\"2\" border=\"0\"></td>\n");
					writer.Write("<td width=\"*\"></td>\n");
					writer.Write("</tr>\n");
					writer.Write("<td bgcolor=\"#" + bgColor + "\" colspan=\"7\" align=\"left\" valign=\"top\">\n");
						
						
					writer.Write("<div id=\"GeneralDiv_" + OrderNumber.ToString() + "\" name=\"GeneralDiv_" + OrderNumber.ToString() + "\" style=\"width: 100%; display:none;\">\n");
					bool HasDownload = Common.OrderHasDownloadComponents(DB.RSFieldInt(rs,"OrderNumber"));
					writer.Write("<table align=\"left\" valign=\"top\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\n");
					writer.Write("<tr><td align=\"right\" valign=\"top\">Order Number:&nbsp;</td><td align=\"left\" valign=\"top\">" + OrderNumber.ToString() + "</td>\n");
					writer.Write("<tr><td align=\"right\" valign=\"top\">Order Date:&nbsp;</td><td align=\"left\" valign=\"top\">" + Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs,"OrderDate")) + "</td></tr>\n");
					writer.Write("<tr><td align=\"right\" valign=\"top\">Customer ID:&nbsp;</td><td align=\"left\" valign=\"top\">" + DB.RSFieldInt(rs,"CustomerID").ToString() + "</td></tr>\n");
					int NumOrders = DB.GetSqlN("select count(ordernumber) as N from orders  " + DB.GetNoLock() + " where customerid=" + DB.RSFieldInt(rs,"CustomerID").ToString());
					if(NumOrders > 0)
					{
						writer.Write("<tr><td align=\"right\" valign=\"top\">Order History:&nbsp;</td><td align=\"left\" valign=\"top\">");
						writer.Write("<a target=\"content\" href=\"cst_history.aspx?customerid=" + DB.RSFieldInt(rs,"CustomerID").ToString() + "\">");
						for(int i = 1; i<= NumOrders; i++)
						{
							writer.Write("<img src=\"skins/skin_" + _siteID.ToString() + "/images/smile.gif\" border=\"0\" align=\"absmiddle\">");
							if(i % 25 == 0)
							{
								writer.Write("<br>");
							}
						}
						writer.Write("</td></tr>\n");
					}
					writer.Write("<tr><td align=\"right\" valign=\"top\">Order Total:&nbsp;</td><td align=\"left\" valign=\"top\">" + Common.IIF(DB.RSFieldBool(rs,"QuoteCheckout") , "REQUEST FOR QUOTE" , Localization.CurrencyStringForDB( DB.RSFieldDecimal(rs,"OrderTotal"))) + "</td></tr>\n");
					writer.Write("<tr><td align=\"right\" valign=\"top\">Subscription Months:&nbsp;</td><td align=\"left\" valign=\"top\">" + ord.SubscriptionMonths().ToString() + "</td></tr>\n");
					writer.Write("<tr><td align=\"right\" valign=\"top\">Locale Setting:&nbsp;</td><td align=\"left\" valign=\"top\">" + ord._localeSetting + "</td></tr>\n");
					writer.Write("<tr><td colspan=2>&nbsp;</td></tr>");
					writer.Write("<tr><td align=\"right\" valign=\"top\">Payment Method:&nbsp;</td><td align=\"left\" valign=\"top\">" + DB.RSField(rs,"PaymentMethod") + "</td></tr>\n");
					writer.Write("<tr><td align=\"right\" valign=\"top\">Payment Cleared:&nbsp;</td><td align=\"left\" valign=\"top\">" + Common.IIF(DB.RSFieldDateTime(rs,"PaymentClearedOn") != System.DateTime.MinValue, Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs,"PaymentClearedOn")), "No") + "</td></tr>\n");
					writer.Write("<tr><td align=\"right\" valign=\"top\">Payment Gateway:&nbsp;</td><td align=\"left\" valign=\"top\">" + DB.RSField(rs,"PaymentGateway") + "</td></tr>\n");
					
					writer.Write("<tr><td align=\"right\" valign=\"middle\">TXState:&nbsp;</td><td align=\"left\" valign=\"top\">");
					if((IsCard || IsMicroPay) && (GW != "MANUAL" && GW != "WORLDPAY JUNIOR" && GW != "WORLDPAY")) // junior doesn't support void, delayed capture, or refunds via API
					{
						if(TXState == "AUTH")
						{
							writer.Write(TXState);
							writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"button\" value=\"Capture\" name=\"CaptureOrder" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "\" onClick=\"CaptureOrder(OrderDetailForm," + DB.RSFieldInt(rs,"OrderNumber").ToString() + ");\">");
							//writer.Write("<input type=\"button\" value=\"Void\" name=\"VoidOrder" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "\" onClick=\"VoidOrder(OrderDetailForm," + DB.RSFieldInt(rs,"OrderNumber").ToString() + ");\">");
						}
						else if(TXState == "AUTH CAPTURE")
						{
							writer.Write(TXState);
							writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"button\" value=\"Void\" name=\"VoidOrder" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "\" onClick=\"VoidOrder(OrderDetailForm," + DB.RSFieldInt(rs,"OrderNumber").ToString() + ");\">");
							writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"button\" value=\"Refund\" name=\"RefundOrder" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "\" onClick=\"RefundOrder(OrderDetailForm," + DB.RSFieldInt(rs,"OrderNumber").ToString() + ");\">");
						}
						else
						{
							writer.Write("None");
						}
					}
					else
					{
						writer.Write("N/A");
					}
					writer.Write("</td></tr>");
					
					writer.Write("<tr><td colspan=2>&nbsp;</td></tr>");
					writer.Write("<tr><td align=\"right\" valign=\"middle\">Receipt E-Mail Sent On:&nbsp;</td><td align=\"left\" valign=\"middle\">" + Common.IIF(DB.RSFieldDateTime(rs,"ReceiptEMailSentOn") != System.DateTime.MinValue, Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs,"ReceiptEMailSentOn")), "Not Sent"));
					writer.Write("&nbsp;<input style=\"font-size: 9px;\" type=\"button\" value=\"Re-Send Receipt E-Mail\" name=\"Clear" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "\" onClick=\"SendReceiptEMail(OrderDetailForm," + DB.RSFieldInt(rs,"OrderNumber").ToString() + ");\">");
					writer.Write("</td></tr>\n");
					writer.Write("</td></tr>\n");
					writer.Write("<tr><td colspan=2>&nbsp;</td></tr>");

					writer.Write("<tr><td align=\"right\" valign=\"top\">Shipping Method:&nbsp;</td><td align=\"left\" valign=\"top\">" + Common.IIF(HasShippableComponents, DB.RSField(rs,"ShippingMethod"), "All Download Items") + "</td></tr>\n");

					String ShippingStatus = String.Empty;
					if(HasShippableComponents)
					{
						if(DB.RSFieldDateTime(rs,"ShippedOn") != System.DateTime.MinValue)
						{
							ShippingStatus = "Shipped";
							if(DB.RSField(rs,"ShippedVIA").Length != 0)
							{
								ShippingStatus += " via " + DB.RSField(rs,"ShippedVIA");
							}
							ShippingStatus += " on " + Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs,"ShippedOn")) + ".";

							if(DB.RSField(rs,"ShippedVIA") == "ShipRush" && Common.AppConfigBool("ShipRush.Enabled"))
							{
								IDataReader rsp = DB.GetRS("select * from OR_JOBS  " + DB.GetNoLock() + " where Notes like " + DB.SQuote("Order Number: " + DB.RSFieldInt(rs,"OrderNumber").ToString()));
								if(rsp.Read())
								{
									ShippingStatus += " Tracking Number: " + DB.RSField(rsp,"TrackRef");
								}
								rsp.Close();
							}
							else
							{
								if(DB.RSField(rs,"ShippingTrackingNumber").Length != 0)
								{
									ShippingStatus += " Tracking Number: " + DB.RSField(rs,"ShippingTrackingNumber");
								}
							}
						}
						else
						{
							ShippingStatus += "Carrier: <input maxlength=\"50\" size=\"10\" type=\"text\" name=\"ShippedVIA\">&nbsp;&nbsp;";
							ShippingStatus += "Tracking#:<input maxlength=\"50\" size=\"10\" type=\"text\" name=\"ShippingTrackingNumber\"><br>";
							if(PaymentCleared)
							{
								ShippingStatus += "<input type=\"button\" value=\"Mark As Shipped\" onClick=\"document.OrderDetailForm.SubmitAction.value='MARKASSHIPPED';document.OrderDetailForm.submit();\">\n";
							}
							else
							{
								ShippingStatus += "<input type=\"button\" value=\"Mark As Shipped\" onClick=\"if(confirm('Are you sure you want to proceed. The payment for this order has not yet cleared, and this will close the order, and remove the IsNew status from the order!')) {document.OrderDetailForm.SubmitAction.value='MARKASSHIPPED';document.OrderDetailForm.submit();}\">\n";
							}
						}
						writer.Write("<tr><td align=\"right\" valign=\"top\">Shipping Status:&nbsp;</td><td align=\"left\" valign=\"top\">" + ShippingStatus + "</td></tr>\n");
					}
					writer.Write("<tr><td colspan=2>&nbsp;</td></tr>");
					if(ord.HasDownloadComponents())
					{
						writer.Write("<tr><td align=\"right\" valign=\"top\">Has Download Items:&nbsp;</td><td align=\"left\" valign=\"top\">Yes</td></tr>\n");
						writer.Write("<tr><td align=\"right\" valign=\"top\">Download E-Mail Sent On:&nbsp;</td><td align=\"left\" valign=\"top\">" + Common.IIF(DB.RSFieldDateTime(rs,"DownloadEMailSentOn") != System.DateTime.MinValue, Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs,"DownloadEMailSentOn")), "N/A") + "<input type=\"button\" value=\"Re-Send Download E-Mail\" name=\"Download" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "\" onClick=\"SendDownloadEMail(OrderDetailForm," + DB.RSFieldInt(rs,"OrderNumber").ToString() + ");\"></td></tr>\n");

					}
					else
					{
						writer.Write("<tr><td align=\"right\" valign=\"top\">Has Download Items:&nbsp;</td><td align=\"left\" valign=\"top\">No</td></tr>\n");
					}

					if(Common.AppConfigBool("ShipRush.Enabled"))
					{
						writer.Write("<tr><td colspan=2>&nbsp;</td></tr>");
						writer.Write("<tr><td align=\"right\" valign=\"top\">ShipRush:&nbsp;</td><td align=\"left\" valign=\"top\">");
						if(DB.RSFieldDateTime(rs,"ShippedOn") == System.DateTime.MinValue)
						{
							writer.Write("<input style=\"font-size: 9px;\" type=\"button\" value=\"ShipRush\" name=\"ShipRush" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "\" onClick=\"ShipRushOrder(OrderDetailForm," + DB.RSFieldInt(rs,"OrderNumber").ToString() + "," + Common.IIF(DB.RSFieldDateTime(rs,"PaymentClearedOn") != System.DateTime.MinValue, "1","0") + ");\">");
						}
						else
						{
							writer.Write("Sent To ShipRush on " + Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs,"ShippedOn")));
						}
						writer.Write("</td></tr>");
					}


					writer.Write("<tr><td colspan=2>&nbsp;</td></tr>");
					writer.Write("<tr><td align=\"right\" valign=\"top\">Bill To:&nbsp;</td><td align=\"left\" valign=\"top\">" + BillAddr + "</td></tr>\n");
					writer.Write("<tr><td colspan=2>&nbsp;</td></tr>");
					writer.Write("<tr><td align=\"right\" valign=\"top\">Ship To:&nbsp;</td><td align=\"left\" valign=\"top\">" + ShipAddr + "</td></tr>\n");
					writer.Write("</table>\n");
					writer.Write("</div>\n");

					writer.Write("<div id=\"BillingDiv_" + OrderNumber.ToString() + "\" name=\"BillingDiv_" + OrderNumber.ToString() + "\" style=\"width: 100%; display:none;\">\n");

					String PaymentStatus = String.Empty;
					if(PaymentCleared)
					{
						PaymentStatus += " Cleared On: " + DB.RSFieldDateTime(rs,"PaymentClearedOn").ToString() + "<br>";
						PaymentStatus += " Receipt E-Mail Sent On: " + DB.RSFieldDateTime(rs,"ReceiptEMailSentOn").ToString() + "<br>";
						PaymentStatus += "<input style=\"font-size: 9px;\" type=\"button\" value=\"Re-Send Receipt E-Mail\" name=\"Clear" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "\" onClick=\"SendReceiptEMail(OrderDetailForm," + DB.RSFieldInt(rs,"OrderNumber").ToString() + ");\">";
					}
					else
					{
						PaymentStatus += " Pending<br>";
						PaymentStatus += "<input style=\"font-size: 9px;\" type=\"button\" value=\"Mark Payment Cleared\" name=\"Clear" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "\" onClick=\"ClearPayment(OrderDetailForm," + DB.RSFieldInt(rs,"OrderNumber").ToString() + ");\">";
					}
					writer.Write("<p>" + PaymentStatus + "</p>");

					if((IsCard || IsMicroPay) && (GW != "MANUAL" && GW != "WORLDPAY JUNIOR" && GW != "WORLDPAY")) // junior doesn't support void, delayed capture, or refunds via API
					{
						if(TXState == "AUTH")
						{
							writer.Write("TXState:&nbsp;" + TXState);
							writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"button\" value=\"Capture\" name=\"CaptureOrder" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "\" onClick=\"CaptureOrder(OrderDetailForm," + DB.RSFieldInt(rs,"OrderNumber").ToString() + ");\">");
							//writer.Write("<input type=\"button\" value=\"Void\" name=\"VoidOrder" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "\" onClick=\"VoidOrder(OrderDetailForm," + DB.RSFieldInt(rs,"OrderNumber").ToString() + ");\">");
						}
						else if(TXState == "AUTH CAPTURE")
						{
							writer.Write("TXState:&nbsp;" + TXState);
							writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"button\" value=\"Void\" name=\"VoidOrder" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "\" onClick=\"VoidOrder(OrderDetailForm," + DB.RSFieldInt(rs,"OrderNumber").ToString() + ");\">");
							writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"button\" value=\"Refund\" name=\"RefundOrder" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "\" onClick=\"RefundOrder(OrderDetailForm," + DB.RSFieldInt(rs,"OrderNumber").ToString() + ");\">");
						}
						else
						{
							writer.Write("TXState: None");
						}
					}
					else
					{
						writer.Write("TXState: N/A");
					}
					writer.Write("<p></p>");


					writer.Write("<table align=\"left\" valign=\"top\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\n");
					if(IsEcheck || IsMicroPay || IsCard) 
					{
						writer.Write("<tr><td width=\"200\" align=\"right\" valign=\"top\">Order Number:&nbsp;</td><td align=\"left\" valign=\"top\">" + OrderNumber.ToString() + "</td></tr>");
						writer.Write("<tr><td align=\"right\" valign=\"top\">Customer ID:&nbsp;</td><td align=\"left\" valign=\"top\">" + DB.RSFieldInt(rs,"CustomerID").ToString() + "</td></tr>");
						writer.Write("<tr><td align=\"right\" valign=\"top\">Order Date:&nbsp;</td><td align=\"left\" valign=\"top\">" + DB.RSFieldDateTime(rs,"OrderDate").ToString() + "</td></tr>");
						writer.Write("<tr><td align=\"right\" valign=\"top\">Order Total:&nbsp;</td><td align=\"left\" valign=\"top\">" + Localization.CurrencyStringForDB( DB.RSFieldDecimal(rs,"OrderTotal")) + "</td></tr>");
						writer.Write("<tr><td align=\"right\" valign=\"top\">Card Type:&nbsp;</td><td align=\"left\" valign=\"top\">" + DB.RSField(rs,"CardType") + "</td></tr>");
						writer.Write("<tr><td align=\"right\" valign=\"top\">Payment Gateway:&nbsp;</td><td align=\"left\" valign=\"top\">" + DB.RSField(rs,"PaymentGateway") + "</td></tr>");
						writer.Write("<tr><td align=\"right\" valign=\"top\">Transaction State:&nbsp;</td><td align=\"left\" valign=\"top\">" + DB.RSField(rs,"TransactionState") + "</td></tr>");
						writer.Write("<tr><td align=\"right\" valign=\"top\">Payment Cleared On:&nbsp;</td><td align=\"left\" valign=\"top\">" + Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs,"PaymentClearedOn")) + "</td></tr>");
						writer.Write("<tr><td align=\"right\" valign=\"top\">AVS Result:&nbsp;</td><td align=\"left\" valign=\"top\">" + DB.RSField(rs,"AVSResult") + "</td></tr>");

    
						String _cardNumber = DB.RSField(rs,"CardNumber");
				
						String ctmp = _cardNumber;
						if(_cardNumber.Length != 0 && _cardNumber != "XXXXXXXXXXXX")
						{
							ctmp = Common.UnmungeString(_cardNumber);
						}
						String _cardType = DB.RSField(rs,"CardType");
    
						if (IsEcheck)
						{
							writer.Write(String.Format("<tr><td align=\"right\" valign=\"top\">ECheck Bank Name:&nbsp;</td><td align=\"left\" valign=\"top\">{0}</td></tr>",DB.RSField(rs,"eCheckBankName")));
							writer.Write(String.Format("<tr><td align=\"right\" valign=\"top\">ECheck ABA:&nbsp;</td><td align=\"left\" valign=\"top\">{0}</td></tr>",DB.RSField(rs,"eCheckBankABACode")));
							writer.Write(String.Format("<tr><td align=\"right\" valign=\"top\">ECheck Account:&nbsp;</td><td align=\"left\" valign=\"top\">{0}</td></tr>",DB.RSField(rs,"eCheckBankAccountNumber")));
							writer.Write(String.Format("<tr><td align=\"right\" valign=\"top\">ECheck Account Name:&nbsp;</td><td align=\"left\" valign=\"top\">{0}</td></tr>",DB.RSField(rs,"eCheckBankAccountName")));
							writer.Write(String.Format("<tr><td align=\"right\" valign=\"top\">ECheck Account Type:&nbsp;</td><td align=\"left\" valign=\"top\">{0}</td></tr>",DB.RSField(rs,"eCheckBankAccountType")));
						}
						if (IsMicroPay)
						{
							writer.Write("<tr><td align=\"right\" valign=\"top\">" + Common.AppConfig("MicroPay.Prompt") + " Transaction:&nbsp;</td><td align=\"left\" valign=\"top\"></td></tr>");
						}
						else
						{
							if(_cardType.ToUpper() == "PayPal".ToUpper())
							{
								writer.Write("<tr><td align=\"right\" valign=\"top\">Card Number:&nbsp;</td><td align=\"left\" valign=\"top\">PayPal</td></tr>");
							}
							else
							{
								writer.Write("<tr><td align=\"right\" valign=\"top\">Card Number:&nbsp;</td><td align=\"left\" valign=\"top\">" + ctmp + "</td></tr>");
							}
							if(_cardNumber.Length == 0 || _cardNumber == "XXXXXXXXXXXX")
							{
								writer.Write("<tr><td align=\"right\" valign=\"top\">Card Expiration:&nbsp;</td><td align=\"left\" valign=\"top\"></td></tr>");
							}
							else
							{
								if(_cardType.ToUpper() == "PayPal".ToUpper())
								{
									writer.Write("<tr><td align=\"right\" valign=\"top\">Card Expiration:&nbsp;</td><td align=\"left\" valign=\"top\"></td></tr>");
								}
								else
								{
									writer.Write("<tr><td align=\"right\" valign=\"top\">Card Expiration:&nbsp;</td><td align=\"left\" valign=\"top\">" + DB.RSField(rs,"CardExpirationMonth") + "/" + DB.RSField(rs,"cardExpirationYear") + "</td></tr>");
								}
							}
						}
						writer.Write("<tr><td align=\"right\" valign=\"top\">Transaction Command:&nbsp;</td><td align=\"left\" valign=\"top\">" + Common.WrapString(DB.RSField(rs,"TransactionCommand"),80,"<br>") + "</td></tr>");
						writer.Write("<tr><td align=\"right\" valign=\"top\">Authorization Result:&nbsp;</td><td align=\"left\" valign=\"top\">" + Common.WrapString(DB.RSField(rs,"AuthorizationResult"),80,"<br>") + "</td></tr>");
						writer.Write("<tr><td align=\"right\" valign=\"top\">Authorization Code:&nbsp;</td><td align=\"left\" valign=\"top\">" + DB.RSField(rs,"AuthorizationCode") + "</td></tr>");
						writer.Write("<tr><td align=\"right\" valign=\"top\">Authorization PNREF (Transaction ID):&nbsp;</td><td align=\"left\" valign=\"top\">" + DB.RSField(rs,"AuthorizationPNREF") + "</td></tr>");
						writer.Write("<tr><td align=\"right\" valign=\"top\">Capture TX Command:&nbsp;</td><td align=\"left\" valign=\"top\">" + Common.WrapString(Common.IIF(DB.RSField(rs,"CaptureTXCommand").Length == 0, "N/A", DB.RSField(rs,"CaptureTXCommand")),80,"<br>") + "</td></tr>");
						writer.Write("<tr><td align=\"right\" valign=\"top\">Capture TX Result:&nbsp;</td><td align=\"left\" valign=\"top\">" + Common.WrapString(Common.IIF(DB.RSField(rs,"CaptureTXResult").Length == 0, "N/A", DB.RSField(rs,"CaptureTXResult")),80,"<br>") + "</td></tr>");
						writer.Write("<tr><td align=\"right\" valign=\"top\">Void TX Command:&nbsp;</td><td align=\"left\" valign=\"top\">" + Common.WrapString(Common.IIF(DB.RSField(rs,"VoidTXCommand").Length == 0, "N/A", DB.RSField(rs,"VoidTXCommand")),80,"<br>") + "</td></tr>");
						writer.Write("<tr><td align=\"right\" valign=\"top\">Void TX Result:&nbsp;</td><td align=\"left\" valign=\"top\">" + Common.WrapString(Common.IIF(DB.RSField(rs,"VoidTXResult").Length == 0, "N/A", DB.RSField(rs,"VoidTXResult")),80,"<br>") + "</td></tr>");
						writer.Write("<tr><td align=\"right\" valign=\"top\">Refund TX Command:&nbsp;</td><td align=\"left\" valign=\"top\">" + Common.WrapString(Common.IIF(DB.RSField(rs,"RefundTXCommand").Length == 0, "N/A", DB.RSField(rs,"RefundTXCommand")),80,"<br>") + "</td></tr>");
						writer.Write("<tr><td align=\"right\" valign=\"top\">Refund TX Result:&nbsp;</td><td align=\"left\" valign=\"top\">" + Common.WrapString(Common.IIF(DB.RSField(rs,"RefundTXResult").Length == 0, "N/A", DB.RSField(rs,"RefundTXResult")),80,"<br>") + "</td></tr>");

						if(Common.AppConfigBool("CardinalCommerce.Centinel.Enabled"))
						{
							writer.Write("<tr><td align=\"right\" valign=\"top\">Cardinal Lookup Result:&nbsp;</td><td align=\"left\" valign=\"top\">" + Common.WrapString(Common.IIF(DB.RSField(rs,"CardinalLookupResult").Length == 0, "N/A", DB.RSField(rs,"CardinalLookupResult")),80,"<br>") + "</td></tr>");
							writer.Write("<tr><td align=\"right\" valign=\"top\">Cardinal Authenticate Result:&nbsp;</td><td align=\"left\" valign=\"top\">" + Common.WrapString(Common.IIF(DB.RSField(rs,"CardinalAuthenticateResult").Length == 0, "N/A", DB.RSField(rs,"CardinalAuthenticateResult")),80,"<br>") + "</td></tr>");
							writer.Write("<tr><td align=\"right\" valign=\"top\">Cardinal Gateway Parms:&nbsp;</td><td align=\"left\" valign=\"top\">" + Common.WrapString(Common.IIF(DB.RSField(rs,"CardinalGatewayParms").Length == 0, "N/A", DB.RSField(rs,"CardinalGatewayParms")),80,"<br>") + "</td></tr>");
						}
					}
					else
					{
						writer.Write("N/A");
					}
					writer.Write("</table>\n");
					writer.Write("</div>\n");

					writer.Write("<div id=\"ShippingDiv_" + OrderNumber.ToString() + "\" name=\"ShippingDiv_" + OrderNumber.ToString() + "\" style=\"width: 100%; display:none;\">\n");
					
					writer.Write("<table align=\"left\" valign=\"top\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\n");
					writer.Write("<tr><td align=\"right\" valign=\"top\">Ship To:&nbsp;</td><td align=\"left\" valign=\"top\">" + ShipAddr + "</td></tr>\n");
					writer.Write("<tr><td colspan=2>&nbsp;</td></tr>");
					writer.Write("<tr><td align=\"right\" valign=\"top\">Shipping Method:&nbsp;</td><td align=\"left\" valign=\"top\">" + Common.IIF(HasShippableComponents, DB.RSField(rs,"ShippingMethod"), "All Download Items") + "</td></tr>\n");

					if(HasShippableComponents)
					{
						writer.Write("<tr><td colspan=2>&nbsp;</td></tr>");
						writer.Write("<tr><td align=\"right\" valign=\"top\">Shipping Status:&nbsp;</td><td align=\"left\" valign=\"top\">" + ShippingStatus + "</td></tr>\n");
						writer.Write("<tr><td colspan=2>&nbsp;</td></tr>");
						if(DB.RSField(rs,"RTShipRequest").Length != 0)
						{
							writer.Write("<tr><td align=\"right\" valign=\"top\">RTShipping:&nbsp;</td><td align=\"left\" valign=\"top\"><a target=\"_blank\" href=\"popuprt.aspx?ordernumber=" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "\">RT Shipping Info</a></td></tr>\n");
						}
					}
					if(ord.HasDownloadComponents())
					{
						writer.Write("<tr><td align=\"right\" valign=\"top\">Has Download Items:&nbsp;</td><td align=\"left\" valign=\"top\">Yes</td></tr>\n");
						writer.Write("<tr><td align=\"right\" valign=\"top\">Download E-Mail Sent On:&nbsp;</td><td align=\"left\" valign=\"top\">" + Common.IIF(DB.RSFieldDateTime(rs,"DownloadEMailSentOn") != System.DateTime.MinValue, Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs,"DownloadEMailSentOn")), "N/A") + "<input type=\"button\" value=\"Re-Send Download E-Mail\" name=\"Download" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "\" onClick=\"SendDownloadEMail(OrderDetailForm," + DB.RSFieldInt(rs,"OrderNumber").ToString() + ");\"></td></tr>\n");

					}
					else
					{
						writer.Write("<tr><td align=\"right\" valign=\"top\">Has Download Items:&nbsp;</td><td align=\"left\" valign=\"top\">No</td></tr>\n");
					}

					if(Common.AppConfigBool("ShipRush.Enabled"))
					{
						writer.Write("<tr><td align=\"right\" valign=\"top\">ShipRush:&nbsp;</td><td align=\"left\" valign=\"top\">");
						if(DB.RSFieldDateTime(rs,"ShippedOn") == System.DateTime.MinValue)
						{
							writer.Write("<input style=\"font-size: 9px;\" type=\"button\" value=\"ShipRush\" name=\"ShipRush" + DB.RSFieldInt(rs,"OrderNumber").ToString() + "\" onClick=\"ShipRushOrder(OrderDetailForm," + DB.RSFieldInt(rs,"OrderNumber").ToString() + "," + Common.IIF(DB.RSFieldDateTime(rs,"PaymentClearedOn") != System.DateTime.MinValue, "1","0") + ");\">");
						}
						else
						{
							writer.Write("Sent To ShipRush on " + Localization.ToNativeDateTimeString(DB.RSFieldDateTime(rs,"ShippedOn")));
						}
						writer.Write("</td></tr>");
					}

					writer.Write("</table>\n");
					writer.Write("</div>\n");

					writer.Write("<div id=\"CustomerDiv_" + OrderNumber.ToString() + "\" name=\"CustomerDiv_" + OrderNumber.ToString() + "\" style=\"width: 100%; display:none;\">\n");
					writer.Write("<table align=\"left\" valign=\"top\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\n");
					writer.Write("<tr><td align=\"right\" valign=\"top\">Customer ID:&nbsp;</td><td align=\"left\" valign=\"top\">" + DB.RSFieldInt(rs,"CustomerID").ToString() + "&nbsp;&nbsp;<a href=\"cst_account.aspx?customerid=" + DB.RSFieldInt(rs,"CustomerID").ToString() + "\" target=\"content\">Edit</a></td></tr>");
					writer.Write("<tr><td align=\"right\" valign=\"top\">Customer Name:&nbsp;</td><td align=\"left\" valign=\"top\">" + (DB.RSField(rs,"FirstName") + " " + DB.RSField(rs,"LastName")).Trim() + "</td></tr>");
					writer.Write("<tr><td align=\"right\" valign=\"top\">Customer Phone:&nbsp;</td><td align=\"left\" valign=\"top\">" + DB.RSField(rs,"Phone") + "</td></tr>");
					writer.Write("<tr><td align=\"right\" valign=\"top\">Customer E-Mail:&nbsp;</td><td align=\"left\" valign=\"top\"><a href=\"mailto:" + DB.RSField(rs,"Email") + "\">" + DB.RSField(rs,"Email") + "</a></td></tr>");
					if(NumOrders > 1)
					{
						writer.Write("<tr><td colspan=2>&nbsp;</td></tr>");
						writer.Write("<tr><td align=\"right\" valign=\"top\">Order History:&nbsp;</td><td align=\"left\" valign=\"top\">");
						writer.Write("<a target=\"content\" href=\"cst_history.aspx?customerid=" + DB.RSFieldInt(rs,"CustomerID").ToString() + "\">");
						for(int i = 1; i<= NumOrders; i++)
						{
							writer.Write("<img src=\"skins/skin_" + _siteID.ToString() + "/images/smile.gif\" border=\"0\" align=\"absmiddle\">");
							if(i % 10 == 0)
							{
								writer.Write("<br>");
							}
						}
						writer.Write("</a>");
						writer.Write("</td></tr>");
					}
					if(DB.RSField(rs,"Referrer").Length != 0)
					{
						writer.Write("<tr><td align=\"right\" valign=\"top\">Referrer:&nbsp;</td><td align=\"left\" valign=\"top\">");
						if(DB.RSField(rs,"Referrer").StartsWith("http://"))
						{
							writer.Write("<a href=\"" + DB.RSField(rs,"Referrer") + "\" target=\"_blank\">");
							writer.Write(DB.RSField(rs,"Referrer"));
							writer.Write("</a>");
						}
						writer.Write("</td></tr>");
					}
					writer.Write("<tr><td align=\"right\" valign=\"top\">Affiliate:&nbsp;</td><td align=\"left\" valign=\"top\">" + Common.GetAffiliateName(DB.RSFieldInt(rs,"AffiliateID")) + "</td></tr>");
					writer.Write("</table>");
					writer.Write("</div>\n");

					writer.Write("<div id=\"NotesDiv_" + OrderNumber.ToString() + "\" name=\"NotesDiv_" + OrderNumber.ToString() + "\" style=\"width: 100%; display:none;\">\n");
					writer.Write("<p><b>CUSTOMER NOTES:</b></p>");
					String Notes = DB.RSField(rs,"OrderNotes");
					if(Notes.Length == 0)
					{
						Notes = "None";
					}
					writer.Write("<p>" + Server.HtmlEncode(Notes) + "</p>");
					writer.Write("<p></p>");
					writer.Write("<p><b>CUSTOMER SERVICE NOTES:</b></p>");
					writer.Write("<textarea rows=\"20\" name=\"CustomerServiceNotes\" cols=\"80\">" + DB.RSField(rs,"CustomerServiceNotes") + "</textarea><br>\n");
					writer.Write("<input type=\"button\" value=\"Submit\" onClick=\"document.OrderDetailForm.SubmitAction.value='UPDATENOTES';document.OrderDetailForm.submit();\">\n");
					writer.Write("</div>\n");

					writer.Write("<div id=\"ReceiptDiv_" + OrderNumber.ToString() + "\" name=\"ReceiptDiv_" + OrderNumber.ToString() + "\" style=\"width: 100%; display:none;\">\n");
					writer.Write("<p><b>For Printable Receipt: <a href=\"" + ReceiptURLPrintable + "\" target=\"_blank\">CLICK HERE</a></b></p>");
					writer.Write(Common.ExtractBody(ord.Receipt(thisCustomer._customerID,Common.GetActiveReceiptTemplate(DB.RSFieldInt(rs,"SiteID")),Common.AppConfig("ShowCCPWD"),false)));
					writer.Write("</div>\n");

					writer.Write("<div id=\"XMLDiv_" + OrderNumber.ToString() + "\" name=\"XMLDiv_" + OrderNumber.ToString() + "\" style=\"width: 100%; display:none;\">\n");
					writer.Write("<br><br><a href=\"orderxml.aspx?ordernumber=" + ord._orderNumber.ToString() + "\" target=\"_blank\">Click Here For Xml</a>");
					writer.Write("</div>\n");

					writer.Write("</td></tr>\n");
					writer.Write("</table>\n");


					writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");

					writer.Write("function ShowDiv_" + OrderNumber.ToString() + "(name)\n");
					writer.Write("{\n");
					writer.Write("	document.getElementById(name + 'Div_" + OrderNumber.ToString() + "').style.display='block';\n");
					writer.Write("	document.getElementById(name + 'TD_" + OrderNumber.ToString() + "').className = 'LightTab';\n");
					writer.Write("	return (true);\n");
					writer.Write("}\n");

					writer.Write("function HideDiv_" + OrderNumber.ToString() + "(name)\n");
					writer.Write("{\n");
					writer.Write("	document.getElementById(name + 'Div_" + OrderNumber.ToString() + "').style.display='none';\n");
					writer.Write("	return (true);\n");
					writer.Write("}\n");


					writer.Write("function ShowGeneralDiv_" + OrderNumber.ToString() + "()\n");
					writer.Write("{\n");
					writer.Write("	ShowDiv_" + OrderNumber.ToString() + "('General');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Billing');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Shipping');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Customer');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Notes');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Receipt');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('XML');\n");
					writer.Write("	return (true);\n");
					writer.Write("}\n");

					writer.Write("function ShowBillingDiv_" + OrderNumber.ToString() + "()\n");
					writer.Write("{\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('General');\n");
					writer.Write("	ShowDiv_" + OrderNumber.ToString() + "('Billing');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Shipping');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Customer');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Notes');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Receipt');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('XML');\n");
					writer.Write("	return (true);\n");
					writer.Write("}\n");

					writer.Write("function ShowShippingDiv_" + OrderNumber.ToString() + "()\n");
					writer.Write("{\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('General');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Billing');\n");
					writer.Write("	ShowDiv_" + OrderNumber.ToString() + "('Shipping');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Customer');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Notes');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Receipt');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('XML');\n");
					writer.Write("	return (true);\n");
					writer.Write("}\n");

					writer.Write("function ShowCustomerDiv_" + OrderNumber.ToString() + "()\n");
					writer.Write("{\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('General');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Billing');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Shipping');\n");
					writer.Write("	ShowDiv_" + OrderNumber.ToString() + "('Customer');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Notes');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Receipt');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('XML');\n");
					writer.Write("	return (true);\n");
					writer.Write("}\n");

					writer.Write("function ShowNotesDiv_" + OrderNumber.ToString() + "()\n");
					writer.Write("{\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('General');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Billing');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Shipping');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Customer');\n");
					writer.Write("	ShowDiv_" + OrderNumber.ToString() + "('Notes');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Receipt');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('XML');\n");
					writer.Write("	return (true);\n");
					writer.Write("}\n");

					writer.Write("function ShowReceiptDiv_" + OrderNumber.ToString() + "()\n");
					writer.Write("{\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('General');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Billing');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Shipping');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Customer');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Notes');\n");
					writer.Write("	ShowDiv_" + OrderNumber.ToString() + "('Receipt');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('XML');\n");
					writer.Write("	return (true);\n");
					writer.Write("}\n");

					writer.Write("function ShowXMLDiv_" + OrderNumber.ToString() + "()\n");
					writer.Write("{\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('General');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Billing');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Shipping');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Customer');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Notes');\n");
					writer.Write("	HideDiv_" + OrderNumber.ToString() + "('Receipt');\n");
					writer.Write("	ShowDiv_" + OrderNumber.ToString() + "('XML');\n");
					writer.Write("	return (true);\n");
					writer.Write("}\n");

					writer.Write("Show" + InitialTab + "Div_" + OrderNumber.ToString() + "();\n");

					writer.Write("</script>\n");

					rs.Close();
					ord = null;

					writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");

					writer.Write("function MarkAsShipped(theForm,id,paymentcleared)\n");
					writer.Write("{\n");
					writer.Write("var oktoproceed = true;\n");
					writer.Write("if(paymentcleared == 0)\n");
					writer.Write("{\n");
					writer.Write("oktoproceed = confirm('Are you sure you want to proceed. The payment for this order has not yet cleared, and this will close the order, and remove the IsNew status from the order!');\n");
					writer.Write("}\n");
					writer.Write("if(oktoproceed)\n");
					writer.Write("{\n");
					writer.Write("document.OrderDetailForm.SubmitAction.value = 'MARKASSHIPPED';\n");
					writer.Write("theForm.submit();\n");
					writer.Write("}\n");
					writer.Write("}\n");

					writer.Write("function ClearNew(theForm,id,paymentcleared)\n");
					writer.Write("{\n");
					writer.Write("var oktoproceed = true;\n");
					writer.Write("if(paymentcleared == 0)\n");
					writer.Write("{\n");
					writer.Write("oktoproceed = confirm('Are you sure you want to proceed. This will remove the IsNew status from the order!');\n");
					writer.Write("}\n");
					writer.Write("if(oktoproceed)\n");
					writer.Write("{\n");
					writer.Write("document.OrderDetailForm.SubmitAction.value = 'CLEARNEW';\n");
					writer.Write("document.OrderDetailForm.submit();\n");
					writer.Write("}\n");
					writer.Write("}\n");

					writer.Write("function CaptureOrder(theForm,id)\n");
					writer.Write("{\n");
					writer.Write("window.open('captureorder.aspx?ordernumber=' + id,'CaptureOrder" + Common.GetRandomNumber(1,100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,copyhistory=no,width=600,height=500,left=0,top=0');\n");
					writer.Write("}\n");

					writer.Write("function VoidOrder(theForm,id)\n");
					writer.Write("{\n");
					//writer.Write("if(confirm('Are you sure you want to void order: ' + id + '. This action cannot be undone! The credit card capture will be voided and the order record will be deleted!'))\n");
					//writer.Write("{\n");
					writer.Write("window.open('voidorder.aspx?ordernumber=' + id,'VoidOrder" + Common.GetRandomNumber(1,100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,copyhistory=no,width=600,height=500,left=0,top=0');\n");
					//writer.Write("}\n");
					writer.Write("}\n");

					writer.Write("function RefundOrder(theForm,id)\n");
					writer.Write("{\n");
					//writer.Write("if(confirm('Are you sure you want to refund order: ' + id + '. This action cannot be undone! The credit card will be refunded and the order record will be deleted!'))\n");
					//writer.Write("{\n");
					writer.Write("window.open('refundorder.aspx?ordernumber=' + id,'RefundOrder" + Common.GetRandomNumber(1,100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,copyhistory=no,width=600,height=500,left=0,top=0');\n");
					//writer.Write("}\n");
					writer.Write("}\n");

					if(Common.AppConfigBool("ShipRush.Enabled"))
					{

						writer.Write("function ShipRushOrder(theForm,id,paymentcleared)\n");
						writer.Write("{\n");
						writer.Write("var oktoproceed = true;\n");
						writer.Write("if(paymentcleared == 0)\n");
						writer.Write("{\n");
						writer.Write("oktoproceed = confirm('Are you sure you want to proceed. The payment for this order has not yet cleared, and this will close the order, and remove the IsNew status from the order!');\n");
						writer.Write("}\n");
						writer.Write("if(oktoproceed)\n");
						writer.Write("{\n");
						writer.Write("document.OrderDetailForm.SubmitAction.value = 'SHIPRUSH';\n");
						writer.Write("theForm.submit();\n");
						writer.Write("}\n");
						writer.Write("}\n");
					}
							
					writer.Write("function DeleteOrder(theForm,id)\n");
					writer.Write("{\n");
					writer.Write("if(confirm('Are you sure you want to delete order: ' + id + '. This action cannot be undone, and the order will be permanently deleted!'))\n");
					writer.Write("{\n");
					writer.Write("document.OrderDetailForm.SubmitAction.value = 'DELETEORDER';\n");
					writer.Write("document.OrderDetailForm.submit();\n");
					writer.Write("}\n");
					writer.Write("}\n");

					writer.Write("function ClearPayment(theForm,id)\n");
					writer.Write("{\n");
					writer.Write("if(confirm('This will  mark order ' + id + ' as payment cleared. This action cannot be undone. The customer will be e-mailed a receipt, and also e-mailed a download instruction e-mail if the order contains downloadable items.'))\n");
					writer.Write("{\n");
					writer.Write("document.OrderDetailForm.SubmitAction.value = 'CLEARORDER';\n");
					writer.Write("document.OrderDetailForm.submit();\n");
					writer.Write("}\n");
					writer.Write("}\n");

					writer.Write("function SendDownloadEMail(theForm,id)\n");
					writer.Write("{\n");
					writer.Write("if(confirm('Are you sure you want to send the download e-mail for order: ' + id + '. This action will send an e-mail with download instructions to the customer, so make sure you have cleared the order payment first!'))\n");
					writer.Write("{\n");
					writer.Write("document.OrderDetailForm.SubmitAction.value = 'SENDDOWNLOADEMAIL';\n");
					writer.Write("document.OrderDetailForm.submit();\n");
					writer.Write("}\n");
					writer.Write("}\n");

					writer.Write("function SendReceiptEMail(theForm,id)\n");
					writer.Write("{\n");
					writer.Write("if(confirm('Are you sure you want to send the receipt e-mail for order: ' + id + '. This action will send an e-mail with to the customer, so make sure you have cleared the order payment first!'))\n");
					writer.Write("{\n");
					writer.Write("document.OrderDetailForm.SubmitAction.value = 'SENDRECEIPTEMAIL';\n");
					writer.Write("document.OrderDetailForm.submit();\n");
					writer.Write("}\n");
					writer.Write("}\n");

					writer.Write("function SetOrderNotes(theForm,id)\n");
					writer.Write("{\n");
					writer.Write("window.open('editordernotes.aspx?OrderNumber=' + id,\"AspDotNetStorefront_ML" + Common.GetRandomNumber(1,10000).ToString() + "\",\"height=300,width=630,top=10,left=20,status=no,toolbar=no,menubar=no,scrollbars=yes,location=no\")\n");
					writer.Write("}\n");

					writer.Write("</SCRIPT>\n");
				}	
			}
			
			writer.Write("</body></html>");
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
