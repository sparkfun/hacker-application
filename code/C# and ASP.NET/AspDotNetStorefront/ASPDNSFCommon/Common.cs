//#define SMTPDOTNET
// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Web.Security;
using System.Configuration;
using System.Web.SessionState;
using System.Web.Caching;
using System.Web.Mail;
using System.Web.Util;
using System.Data;
using System.Security.Principal;
using System.Text;
using System.Collections;
using System.IO;
using System.Net;
using System.Xml;
using System.Drawing;
using System.Xml.Serialization;
using System.Globalization;
using AspDotNetStorefrontEncrypt;

#if SMTPDOTNET
using SmtpDotNet;  
#endif

namespace AspDotNetStorefrontCommon
{
	/// <summary>
	/// Summary description for Common.
	/// </summary>
	public class Common
	{

		// this MUST match the table defs in ShippingCalculation table
		public enum ShippingCalculationEnum
		{
			CalculateShippingByWeight = 1,
			CalculateShippingByTotal = 2,
			UseFixedPrice = 3,
			AllOrdersHaveFreeShipping = 4,
			UseFixedPercentageOfTotal = 5,
			UseIndividualItemShippingCosts = 6,
			UseRealTimeRates = 7,
			CalculateShippingByWeightAndZone = 8
		};

		static private Random RandomGenerator = new Random(System.DateTime.Now.Millisecond);
		
		static public bool IsAdminSite = false; // set to true only in Application_Start of admin site

		static public Hashtable AppConfigTable; // LOADED in application_start of the respective web project

		static public String GetAdvancedCategoryBrowseBox(Customer ThisCustomer, int SiteID)
		{
			XmlPackage p = new XmlPackage("CategoryTreeBox",ThisCustomer,SiteID);
			return p.TransformString();
		}

		static public int CacheDurationMinutes()
		{
			return 60;
		}

		static public String GetAdvancedSectionBrowseBox(Customer ThisCustomer, int SiteID)
		{
			// TBD:
			return GetSectionBrowseBox(SiteID);
		}

		static public String MungeString(String s)
		{
			String EncryptKey = Common.AppConfig("EncryptKey");
			if(EncryptKey.Trim().ToUpper() == "REGISTRY")
			{
				WindowsRegistry reg = new WindowsRegistry(Common.AppConfig("EncryptKey.RegistryLocation"));
				EncryptKey = reg.Read(Common.AppConfig("EncryptKey.RegistryKey"));
				reg = null;
			}
			if(EncryptKey.Trim().ToUpper() == "CONFIG")
			{
				EncryptKey = ConfigurationSettings.AppSettings[Common.AppConfig("EncryptKey.RegistryKey")];
			}
			String tmpS = Encrypt.EncryptData(EncryptKey, s.ToLower()); // lower case so not case sensitive
			return tmpS;
		}

		static public String UnmungeString(String s)
		{
			String EncryptKey = Common.AppConfig("EncryptKey");
			if(EncryptKey.Trim().ToUpper() == "REGISTRY")
			{
				WindowsRegistry reg = new WindowsRegistry(Common.AppConfig("EncryptKey.RegistryLocation"));
				EncryptKey = reg.Read(Common.AppConfig("EncryptKey.RegistryKey"));
				reg = null;
			}
			if(EncryptKey.Trim().ToUpper() == "CONFIG")
			{
				EncryptKey = ConfigurationSettings.AppSettings[Common.AppConfig("EncryptKey.RegistryKey")];
			}
			String tmpS = Encrypt.DecryptData(EncryptKey, s);
			return tmpS;
		}

		//V3_9
		/// <summary>
		/// Updates the MicroPay Balance for a given customer if there is a Micropay 
		/// purchase in the order. But only if the PaymentClearedOn field has been 
		/// set.
		/// </summary>
		public static void MicroPayBalanceUpdate(int OrderNumber)
		{
			Order order = new Order(OrderNumber);
			if (order._paymentClearedOn != System.DateTime.MinValue)
			{
				int mpProductID = Common.GetMicroPayProductID();
				int mpVariantID = Common.GetProductsFirstVariantID(mpProductID);
				decimal mpTotal = Common.GetMicroPayBalance(order._customerID);

				//Use the raw price for the amount because 
				// it may be discounted or on sale in the order
				decimal amount = Common.GetVariantPrice(mpVariantID);
				foreach (CartItem c in order._cartItems)
				{
					if (c.productID == mpProductID)
					{
						mpTotal += (amount * c.quantity);
					}
				}
				DB.ExecuteSQL(String.Format("update Customer set MicroPayBalance={0} where CustomerID={1}",Localization.CurrencyStringForDB(mpTotal),order._customerID));
			}
		}

		public static int GetSubscriptionMonths(int VariantID)
		{
			int tmp = 0;
			IDataReader rs = DB.GetRS("Select SubscriptionMonths from productvariant where variantid=" + VariantID.ToString());
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"SubscriptionMonths");
			}
			rs.Close();
			return tmp;
		}

		public static void SubscriptionUpdate(int OrderNumber)
		{
			Order order = new Order(OrderNumber);
			int numMonths = order.SubscriptionMonths();
			if (order._paymentClearedOn != System.DateTime.MinValue && numMonths != 0)
			{
				// get customer's current subscription expiration:
				IDataReader rs = DB.GetRS("Select SubscriptionExpiresOn from customer " + DB.GetNoLock() + " where CustomerID=" + order._customerID.ToString());
				if(rs.Read())
				{
					DateTime NewExpireDate = System.DateTime.Now;
					if(DB.RSFieldDateTime(rs,"SubscriptionExpiresOn") != System.DateTime.MinValue)
					{
						NewExpireDate = DB.RSFieldDateTime(rs,"SubscriptionExpiresOn");
					}
					NewExpireDate = NewExpireDate.AddMonths(numMonths);
					DB.ExecuteSQL(String.Format("update Customer set SubscriptionExpiresOn={0} where CustomerID={1}",DB.DateQuote(NewExpireDate),order._customerID));
				}
				rs.Close();
			}
		}

		
		// handles login "transformation" from one customer on the site to another, moving
		// cart items, etc...as required. This is a fairly complicated routine to get right ;)
		// this does NOT alter any session/cookie data...you should do that before/after this call
		// don't migrate their shipping info...their "old address book should take priority"
		static public void ExecuteSigninLogic(int CurrentCustomerID, int NewCustomerID)
		{
			String CurrentCustomerOrderNotes = String.Empty;
			String CurrentOrderOptions = String.Empty;
			if(CurrentCustomerID != 0)
			{
				IDataReader rsx = DB.GetRS("Select * from customer  " + DB.GetNoLock() + " where customerid=" + CurrentCustomerID.ToString());
				rsx.Read();
				CurrentCustomerOrderNotes = DB.RSField(rsx,"OrderNotes");
				CurrentOrderOptions = DB.RSField(rsx,"OrderOptions");
				rsx.Close();
			}

			if((ShoppingCart.NumItems(CurrentCustomerID,CartTypeEnum.ShoppingCart) != 0 && Common.AppConfigBool("PreserveActiveCartOnSignin")) || Common.AppConfigBool("ClearOldCartOnSignin")) // but not wishlist or recurring items!
			{
				// if preserve is on, force delete of old cart even if not set at appconfig level, since we are replacing it with the active cart:
				DB.ExecuteSQL("delete from ShoppingCart where CartType=" + ((int)CartTypeEnum.ShoppingCart).ToString() + " and customerid=" + NewCustomerID.ToString());
				DB.ExecuteSQL("delete from kitcart where CartType=" + ((int)CartTypeEnum.ShoppingCart).ToString() + " and customerid=" + NewCustomerID.ToString());
				DB.ExecuteSQL("delete from customcart where CartType=" + ((int)CartTypeEnum.ShoppingCart).ToString() + " and customerid=" + NewCustomerID.ToString());
			}
			if(Common.AppConfigBool("PreserveActiveCartOnSignin"))
			{
				if(CurrentCustomerID != 0)
				{
					DB.ExecuteSQL("update ShoppingCart set customerid=" + NewCustomerID.ToString() + " where CartType<>" + ((int)CartTypeEnum.RecurringCart).ToString() + " and customerid=" + CurrentCustomerID.ToString());
					DB.ExecuteSQL("update kitcart set customerid=" + NewCustomerID.ToString() + " where CartType<>" + ((int)CartTypeEnum.RecurringCart).ToString() + " and customerid=" + CurrentCustomerID.ToString());
					DB.ExecuteSQL("update customcart set customerid=" + NewCustomerID.ToString() + " where CartType<>" + ((int)CartTypeEnum.RecurringCart).ToString() + " and customerid=" + CurrentCustomerID.ToString());
					DB.ExecuteSQL("update customer set OrderNotes=" + DB.SQuote(CurrentCustomerOrderNotes) + ", OrderOptions=" + DB.SQuote(CurrentOrderOptions) + " where customerid=" + NewCustomerID.ToString());
				}
			}
		}

		// examines the specified option string, which should correspond to a size or color option in the product variant,
		// and returns JUST the main option text, removing any cost delta specifiers
		static public String CleanSizeColorOption(String s)
		{
			String tmp = s;
			int i = s.IndexOf("[");
			if(i > 0)
			{
				tmp = s.Substring(0,i).Trim();
			}
			return tmp;
		}

		static public String GetCategoryBreadcrumb(int CategoryID)
		{
			String CacheName = "GetCategoryBreadcrumb" + CategoryID.ToString();
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}
			String tmpS = String.Empty;
			if(CategoryID != 0)
			{
				int pid = Common.GetParentCategory(CategoryID);
				while(pid != 0)
				{
					tmpS = "<a href=\"" + Common.IIF(Common.IsAdminSite, "editcategory.aspx?categoryid=" + pid.ToString(), SE.MakeCategoryLink(pid,"")) + "\">" + Common.GetCategoryName(pid) + "</a> - " + tmpS;
					pid = Common.GetParentCategory(pid);
				}
				tmpS += "<a href=\"" + Common.IIF(Common.IsAdminSite, "editcategory.aspx?categoryid=" + CategoryID.ToString(), SE.MakeCategoryLink(CategoryID,"")) + "\">" + Common.GetCategoryName(CategoryID) + "</a>";
			}

			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS,null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS;
		}
		
		static public String GetSectionBreadcrumb(int SectionID)
		{
			String CacheName = "GetSectionBreadcrumb" + SectionID.ToString();
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}
			String tmpS = String.Empty;
			if(SectionID != 0)
			{
				int pid = Common.GetParentSection(SectionID);
				while(pid != 0)
				{
					tmpS = "<a href=\"" + Common.IIF(Common.IsAdminSite, "editSection.aspx?Sectionid=" + pid.ToString(), SE.MakeSectionLink(pid,"")) + "\">" + Common.GetSectionName(pid) + "</a> - " + tmpS;
					pid = Common.GetParentSection(pid);
				}
				tmpS += "<a href=\"" + Common.IIF(Common.IsAdminSite, "editSection.aspx?Sectionid=" + SectionID.ToString(), SE.MakeSectionLink(SectionID,"")) + "\">" + Common.GetSectionName(SectionID) + "</a>";
			}

			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS,null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS;
		}
		
		static public String GetCartContinueShoppingURL()
		{
			String tmpS = Common.AppConfig("ContinueShoppingLink");
			if(tmpS.Length == 0)
			{
				tmpS= "default.aspx";
			}
			return tmpS;
		}

		
		static public void SendOrderEMail(Customer activeCustomer, int OrderNumber, bool IsRecurring, String PaymentMethod, bool NotifyStoreAdmin)
		{
			Order order = new Order(OrderNumber);
			bool UseLiveTransactions = Common.AppConfigBool("UseLiveTransactions");
			String StoreName = Common.AppConfig("StoreName");
			String MailServer = Common.AppConfig("MailMe_Server");
			
			String SubjectReceipt = String.Empty;
			if(UseLiveTransactions)
			{
				SubjectReceipt = StoreName + " Receipt";
			}
			else
			{
				SubjectReceipt = StoreName + " Receipt (TEST)";
			}
			if(order._paymentMethod.ToUpper() == "REQUEST QUOTE")
			{
				SubjectReceipt += " (REQUEST FOR QUOTE)";
			}
			String SubjectNotification = String.Empty;
			if(UseLiveTransactions)
			{
				SubjectNotification = StoreName + " New Order Notification";
			}
			else
			{
				SubjectNotification = StoreName + " New Order Notification (TEST)";
			}
			if(order._paymentMethod.ToUpper() == "REQUEST QUOTE")
			{
				SubjectNotification += " (REQUEST FOR QUOTE)";
			}

			if(IsRecurring)
			{
				SubjectReceipt += " (RECURRING AUTO-SHIP)";
			}
			
			if(NotifyStoreAdmin)
			{
				// send E-Mail notice to store admin:
				if(order._receiptEMailSentOn == System.DateTime.MinValue)
				{
					try
					{
						Common.SendMail(SubjectNotification, order.Notification() + Common.AppConfig("MailFooter"), true, Common.AppConfig("GotOrderEMailFrom"),Common.AppConfig("GotOrderEMailFromName"),Common.AppConfig("GotOrderEMailTo"),Common.AppConfig("GotOrderEMailTo"),"",Common.AppConfig("MailMe_Server"));
					}
					catch {}
				}

				// send SMS notice to store admin:
				if(order._receiptEMailSentOn == System.DateTime.MinValue)
				{
					// SEND CELL MESSAGE NOTIFICATION:
					try
					{
						SMS.Send(order, Common.AppConfig("ReceiptEMailFrom"), MailServer);
					}
					catch {}
				}
			}
			
			//  now send customer e-mails:
			bool OkToSend = false;
			if(IsRecurring)
			{
				if(Common.AppConfigBool("Recurring.SendOrderEMailToCustomer") && MailServer.Length != 0 && MailServer != "TBD")
				{
					OkToSend = true;
				}
			}
			else
			{
				if(Common.AppConfigBool("SendOrderEMailToCustomer") && MailServer.Length != 0 && MailServer != "TBD")
				{
					OkToSend = true;
				}
			}
			if(OkToSend)
			{

				try
				{

					// NOTE: we changed this to ALWAYS send the receipt:
					//if(order._paymentClearedOn.Length != 0 && (PaymentMethod.Replace(" ","").Trim().ToUpper() == "PAYPAL" || (PaymentMethod.Replace(" ","").Trim().ToUpper() == "CREDITCARD" && order._paymentGateway != "MANUAL")))
					//{
					//	// ok to send receipt:
					if(order._receiptEMailSentOn == System.DateTime.MinValue)
					{
						try
						{
						Common.SendMail(SubjectReceipt, order.Receipt(activeCustomer._customerID,Common.GetActiveReceiptTemplate(order._siteID),"",false) + Common.AppConfig("MailFooter"), true, Common.AppConfig("ReceiptEMailFrom"),Common.AppConfig("ReceiptEMailFromName"),order._billingAddress.email,order._billingAddress.email,"",MailServer);
						DB.ExecuteSQL("update Orders set ReceiptEmailSentOn=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + " where OrderNumber=" + order._orderNumber.ToString());
						}
						catch {}
					}
					//}
					//V3_9
					if(order._paymentClearedOn != System.DateTime.MinValue && order._receiptEMailSentOn == System.DateTime.MinValue && order.HasDownloadComponents() && order.ThereAreDownloadFilesSpecified() && !Common.AppConfigBool("DelayedDownloads") && (PaymentMethod.Replace(" ","").Trim().ToUpper() == "PAYPAL" || PaymentMethod.Replace(" ","").Trim().ToUpper() == "ECHECK" || PaymentMethod.Replace(" ","").Trim().ToUpper() == "MICROPAY" || (PaymentMethod.Replace(" ","").Trim().ToUpper() == "CREDITCARD" && order._paymentGateway != "MANUAL")))
					{
						// card authorized, ok to send download e-mail or secure attachment:
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
				catch {}

			}
		}

		static public void MarkOrderAsShipped(int OrderNumber, String ShippedVIA, String ShippingTrackingNumber, bool IsRecurring)
		{
			DB.ExecuteSQL("Update orders set IsNew=0, ShippedVIA=" + DB.SQuote(ShippedVIA) + ", ShippingTrackingNumber=" + DB.SQuote(ShippingTrackingNumber) + ", ShippedOn=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + " where OrderNumber=" + OrderNumber.ToString());

			String MailServer = Common.AppConfig("MailMe_Server");
			
			bool OkToSend = false;
			if(IsRecurring)
			{
				if(Common.AppConfigBool("Recurring.SendShippedEMailToCustomer") && MailServer.Length != 0 && MailServer != "TBD")
				{
					OkToSend = true;
				}
			}
			else
			{
				if(Common.AppConfigBool("SendShippedEMailToCustomer") && MailServer.Length != 0 && MailServer != "TBD")
				{
					OkToSend = true;
				}
			}

			if(OkToSend)
			{
				Order order = new Order(OrderNumber);
				try
				{
					// try to send "shipped on" email
					String SubjectShipped = Common.AppConfig("StoreName") + " Order Shipped";
					if(IsRecurring)
					{
						SubjectShipped += "(RECURRING AUTO-SHIP)";
					}

					String BodyShipped = String.Empty;
					if(Common.AppConfig("ShippingTemplateNumber").Length != 0)
					{
						String ShippingTemplate = new ShippingTemplate(Common.AppConfigUSInt("ShippingTemplateNumber")).Contents;
						if(ShippingTemplate.Length != 0)
						{
							BodyShipped = ShippingTemplate.Replace("(!CARRIER!)",ShippedVIA)
								.Replace("(!TRACKINGNUMBER!)",ShippingTrackingNumber)
								.Replace("(!ORDERNUMBER!)",OrderNumber.ToString())
								.Replace("(!STORENAME!)",Common.AppConfig("StoreName"))
								.Replace("(!STOREURL!)",Common.GetStoreHTTPLocation(false))
								.Replace("(!DATE!)",Localization.ToNativeShortDateString(System.DateTime.Now));
						}
					}
					if(BodyShipped.Length == 0)
					{
						BodyShipped = "<html><head></head><body>Your order number " + order._orderNumber.ToString() + " has shipped on " + Localization.ToNativeShortDateString(System.DateTime.Now) + " via " + ShippedVIA + ".";
						if(ShippingTrackingNumber.Length != 0)
						{
							BodyShipped += " The tracking number is: " + ShippingTrackingNumber.ToString() + "<br><br>";
						}
						BodyShipped += " Thank you for shopping at " + Common.AppConfig("StoreName") + ", and please remember to tell a friend or two about us!<br><br>";
						BodyShipped += "</body></html>";
					}
					if(MailServer.Length != 0 && MailServer.ToUpper() != "TBD")
					{
						Common.SendMail(SubjectShipped, BodyShipped + Common.AppConfig("MailFooter"), true, Common.AppConfig("ReceiptEMailFrom"),Common.AppConfig("ReceiptEMailFromName"), order._email, order._email, String.Empty, MailServer);
					}
				}
				catch {}
			}
		}

		static public String GetRecurringCart(Customer thisCustomer, int OriginalRecurringOrderNumber, int _siteID, bool OnlyLoadRecurringItemsThatAreDue)
		{
			ShoppingCart cart = new ShoppingCart(_siteID,thisCustomer,CartTypeEnum.RecurringCart,OriginalRecurringOrderNumber,OnlyLoadRecurringItemsThatAreDue);

			// Need to find one of the CartItems that match the OriginalRecurringOrderNumber
			CartItem co = new CartItem();
			foreach (CartItem c in cart._cartItems)
			{
				if(c._originalRecurringOrderNumber == OriginalRecurringOrderNumber)
				{
					co = c;
					break;
				}
			}

			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/recurringcart.gif\" border=\"0\">");

			if(Common.IsAdminSite && (co._nextRecurringShipDate <= System.DateTime.Now))
			{
				tmpS.Append(String.Format("<img src=\"images/spacer.gif\" width=\"10\" height=\"1\"><a href=\"recurring.aspx?processCustomerID={0}&OriginalRecurringOrderNumber={1}\"><img src=\"skins/skin_{2}/images/processrecurring.gif\" border=\"0\"></a>",thisCustomer._customerID,OriginalRecurringOrderNumber,_siteID));
			}
			tmpS.Append("<br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");

			tmpS.Append(cart.DisplayRecurring(OriginalRecurringOrderNumber));

			tmpS.Append("<br><br>");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			return tmpS.ToString();
		}

		static public String UTF8ByteArrayToString(Byte[] characters) 
		{ 

			UTF8Encoding encoding = new UTF8Encoding();
			String constructedString = encoding.GetString(characters);
			return constructedString;
		}
 
		static public Byte[] StringToUTF8ByteArray(String pXmlString)
		{
			UTF8Encoding encoding = new UTF8Encoding();
			Byte[] byteArray = encoding.GetBytes(pXmlString);
			return byteArray;
		} 
		
		static public String SerializeObject(Object pObject, System.Type objectType) 
		{
			try 
			{
				String XmlizedString = null;
				MemoryStream memoryStream = new MemoryStream();
				XmlSerializer xs = new XmlSerializer(objectType);
				XmlTextWriter XmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
				xs.Serialize(XmlTextWriter, pObject);
				memoryStream = (MemoryStream)XmlTextWriter.BaseStream;
				XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
				return XmlizedString;
			}
			catch (Exception ex)
			{
				return Common.GetExceptionDetail(ex,"\n");
			}
		} 
		
		static public String FormatXml(XmlDocument inputXml)
		{
			StringWriter writer = new StringWriter();
			XmlTextWriter XmlWriter = new XmlTextWriter(writer);
			XmlWriter.Formatting = Formatting.Indented;
			XmlWriter.Indentation = 2;
			inputXml.WriteTo(XmlWriter);
			return writer.ToString();
		}
		
		static public String GetCountryBar(String currentLocaleSetting)
		{
			String CacheName = "GetCountryBar";
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			DataSet ds = DB.GetDS("select * from localesetting  " + DB.GetNoLock() + " order by displayorder,description",true,System.DateTime.Now.AddHours(1));
			if(ds.Tables[0].Rows.Count == 0)
			{
				ds.Dispose();
				return String.Empty;
			}

			StringBuilder tmpS = new StringBuilder(5000);

			tmpS.Append("<!-- COUNTRY BAR -->\n");
			tmpS.Append("<script type=\"text/javascript\">\n");

			foreach(DataRow row in ds.Tables[0].Rows)
			{
				String VarName = DB.RowField(row,"Name").Replace("-","_");
				tmpS.Append(VarName + "Off=new Image();\n");
				tmpS.Append(VarName + "Off.src='images/global/" + DB.RowField(row,"Name") + ".gif';\n");
				tmpS.Append(VarName + "On=new Image();\n");
				tmpS.Append(VarName + "On.src='images/global/" + DB.RowField(row,"Name") + "o.gif';\n");
			}

			tmpS.Append("NN6=false;\n");
			tmpS.Append("if(navigator.appVersion.indexOf('MSIE') != -1) MSIE=true; \n");
			tmpS.Append("  else MSIE=false;\n");
			tmpS.Append("if (navigator.userAgent.indexOf('Netscape6') != -1) NN6=true;\n");
			tmpS.Append("\n");
			tmpS.Append("function openInter(type)\n");
			tmpS.Append("{\n");
			tmpS.Append("  if (document.getElementById)\n");
			tmpS.Append("  if(type=='on')\n");
			tmpS.Append("    document.getElementById(\"idd\").style.visibility=\"visible\"; \n");
			tmpS.Append("      else document.getElementById(\"idd\").style.visibility=\"hidden\";\n");
			tmpS.Append("  return false;\n");
			tmpS.Append("}\n");

			tmpS.Append("function changeImg(src,identifier)\n");
			tmpS.Append("{\n");
			tmpS.Append("eval('document.'+identifier+'.src='+src+'.src;');\n");
			tmpS.Append("}\n");
			tmpS.Append("</script>\n");

			int CountryBarTopIdx = Common.AppConfigNativeInt("CountryBarTopIdx");
			tmpS.Append("<div id=\"idd\" style=\"position: absolute; top:" + CountryBarTopIdx.ToString() + "; z-index: 3001; visibility: hidden; width: 100%\">\n");
			tmpS.Append("<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td width=\"100%\" align=\"right\"><table width=\"213\" bgcolor=\"#333333\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\n");
			tmpS.Append("\n");
			
			tmpS.Append("<tr><td><a href=\"setlocale.aspx?localesetting=" + currentLocaleSetting + "&returnurl=" + HttpContext.Current.Server.UrlEncode(Common.GetThisPageName(false) + "?" + Common.ServerVariables("QUERY_STRING")) + "\"  onmouseOut=\"openInter('off')\"><img src=\"images/global/" + currentLocaleSetting + "_sel.gif\" border=\"0\" width=\"213\" height=\"21\" alt=\"" + currentLocaleSetting + "\"></a></td></tr>\n");
			tmpS.Append("<tr><td><a href=# onmouseOver=\"openInter('on')\" onmouseOut=\"openInter('off')\"><img src=\"images/global/dotdiv2.gif\" border=\"0\" width=\"213\" height=\"1\"></a></td></tr>\n");
			
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				String VarName = DB.RowField(row,"Name").Replace("-","_");
				tmpS.Append("<tr><td><a href=\"setlocale.aspx?localesetting=" + DB.RowField(row,"Name") + "&returnurl=" + HttpContext.Current.Server.UrlEncode(Common.GetThisPageName(false) + "?" + Common.ServerVariables("QUERY_STRING")) + "\" onMouseOver=\"openInter('on'); document." + VarName + ".src=" + VarName + "On.src;\" onMouseOut=\"openInter('off'); document." + VarName + ".src=" + VarName + "Off.src;\"><img src=\"images/global/" + DB.RowField(row,"Name") + ".gif\" border=\"0\" width=\"213\" height=\"22\" name=\"" + VarName + "\" alt=\"" + DB.RowField(row,"Description") + "\"></a></td></tr>\n");
				tmpS.Append("<tr><td><a href=# onmouseOver=\"openInter('on')\" onmouseOut=\"openInter('off')\"><img src=\"images/global/dotdiv2.gif\" border=\"0\" width=\"213\" height=\"1\"></a></td></tr>\n");
			}

			tmpS.Append("</table></td></tr></table></div>   \n");

			String CurVarName = currentLocaleSetting.Replace("-","_");
			
			tmpS.Append("<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" id=\"testlayer\"><tr>\n");
			tmpS.Append("<td bgcolor=\"#333333\" width=\"100%\"><img src=\"images/spacer.gif\" alt=\"\" width=\"1\" height=\"15\"></td>\n");
			tmpS.Append("<td rowspan=\"2\" bgcolor=\"#333333\"><a href=\"#\" onMouseOver=\"openInter('on');\"><img src=\"images/global/" + currentLocaleSetting + "_sel.gif?" + Common.GetRandomNumber(1,1000000).ToString() + "\" border=\"0\" width=\"213\" height=\"21\" name=\"iddimage\" alt=\"AspDotNetStorefront International\"></a></td>\n");
			tmpS.Append("</tr>\n");
			tmpS.Append("<tr>\n");
			tmpS.Append("<td align=right><img src=\"images/global/curve.gif\" width=13 height=6></td></tr>\n");
			tmpS.Append("</table>\n");

			tmpS.Append("<!-- END COUNTRY BAR -->\n");
			ds.Dispose();

			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public String GetRecurringVariantsList()
		{
			String CacheName = "GetRecurringVariantsList";
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			IDataReader rs = DB.GetRS("Select variantid from productvariant " + DB.GetNoLock() + " where IsRecurring=1 and deleted=0");
			
			StringBuilder tmpS = new StringBuilder(5000);

			while(rs.Read())
			{
				if(tmpS.Length != 0)
				{
					tmpS.Append(",");
				}
				tmpS.Append(DB.RSFieldInt(rs,"VariantID").ToString());
			}
			rs.Close();

			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public String PlainCategoryList(int ForParentCategoryID, int SkinID)
		{
			String CacheName = "PlainCategoryList" + ForParentCategoryID.ToString();
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(5000);
			String sql = String.Empty;
			if(ForParentCategoryID == 0)
			{
				sql = "Select * from category  " + DB.GetNoLock() + " where (parentCategoryid=0 or ParentCategoryID IS NULL) and deleted=0 and published=1 " + Common.IIF(Common.AppConfigBool("ShowEmptyCategories"), "", " and categoryid in (select distinct(categoryid) from productcategory  " + DB.GetNoLock() + " where productid in (select productid from product  " + DB.GetNoLock() + " where deleted=0 and published=1))") + " order by DisplayOrder";
			}
			else
			{
				sql = "Select * from category  " + DB.GetNoLock() + " where parentCategoryid=" + ForParentCategoryID.ToString() + " and deleted=0 and published=1 " + Common.IIF(Common.AppConfigBool("ShowEmptyCategories"), "", " and categoryid in (select distinct(categoryid) from productcategory  " + DB.GetNoLock() + " where productid in (select productid from product  " + DB.GetNoLock() + " where deleted=0 and published=1))") + " order by DisplayOrder";
			}
			IDataReader rs = DB.GetRS(sql);

			while(rs.Read())
			{
				tmpS.Append("&nbsp;&nbsp;");
				if(Common.AppConfigBool("ShowArrowInPlainList"))
				{
					tmpS.Append("<img src=\"skins/skin_" + SkinID.ToString() + "/images/redarrow.gif\" align=\"absmiddle\">&nbsp;");
				}
				tmpS.Append("<a href=\"" + SE.MakeCategoryLink(DB.RSFieldInt(rs,"CategoryID"),"") + "\" class=\"PlainCategoryLink\">" + DB.RSField(rs,"Name") + "</a><br>");
				if(!Common.AppConfigBool("LimitPlainCategoryListToOneLevel") && Common.CategoryHasSubs(DB.RSFieldInt(rs,"CategoryID")))
				{
					tmpS.Append(Common.PlainCategoryList(DB.RSFieldInt(rs,"CategoryID"),SkinID));
				}
			}
			rs.Close();

			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public String PlainManufacturerList(int SkinID)
		{
			String CacheName = "PlainManufacturerList";
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(5000);
			String sql = String.Empty;
			sql = "Select * from Manufacturer  " + DB.GetNoLock() + " where deleted=0 order by Name";
			IDataReader rs = DB.GetRS(sql);
			while(rs.Read())
			{
				tmpS.Append("&nbsp;&nbsp;");
				if(Common.AppConfigBool("ShowArrowInPlainList"))
				{
					tmpS.Append("<img src=\"skins/skin_" + SkinID.ToString() + "/images/redarrow.gif\" align=\"absmiddle\">&nbsp;");
				}
				tmpS.Append("<a href=\"" + SE.MakeManufacturerLink(DB.RSFieldInt(rs,"ManufacturerID"),"") + "\" class=\"PlainManufacturerLink\">" + DB.RSField(rs,"Name") + "</a><br>");
			}
			rs.Close();

			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}


		static public String PlainSectionList(int ForParentSectionID, int SkinID)
		{
			String CacheName = "PlainSectionList" + ForParentSectionID.ToString();
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(5000);
			String sql = String.Empty;
			if(ForParentSectionID == 0)
			{
				sql = "Select * from [Section]  " + DB.GetNoLock() + " where (parentSectionid=0 or ParentSectionID IS NULL) and deleted=0 and published=1 " + Common.IIF(Common.AppConfigBool("ShowEmptySections"), "" , " and Sectionid in (select distinct(Sectionid) from productSection  " + DB.GetNoLock() + " where productid in (select productid from product  " + DB.GetNoLock() + " where deleted=0 and published=1))") + " order by DisplayOrder";
			}
			else
			{
				sql = "Select * from [Section]  " + DB.GetNoLock() + " where parentSectionid=" + ForParentSectionID.ToString() + " and deleted=0 and published=1 " + Common.IIF(Common.AppConfigBool("ShowEmptySections"), "" , " and Sectionid in (select distinct(Sectionid) from productSection  " + DB.GetNoLock() + " where productid in (select productid from product  " + DB.GetNoLock() + " where deleted=0 and published=1))") + " order by DisplayOrder";
			}
			IDataReader rs = DB.GetRS(sql);

			while(rs.Read())
			{
				tmpS.Append("&nbsp;&nbsp;");
				if(Common.AppConfigBool("ShowArrowInPlainList"))
				{
					tmpS.Append("<img src=\"skins/skin_" + SkinID.ToString() + "/images/redarrow.gif\" align=\"absmiddle\">&nbsp;");
				}
				tmpS.Append("<a href=\"" + SE.MakeSectionLink(DB.RSFieldInt(rs,"SectionID"),"") + "\" class=\"PlainSectionLink\">" + DB.RSField(rs,"Name") + "</a><br>");
				if(!Common.AppConfigBool("LimitPlainSectionListToOneLevel") && Common.SectionHasSubs(DB.RSFieldInt(rs,"SectionID")))
				{
					tmpS.Append(Common.PlainSectionList(DB.RSFieldInt(rs,"SectionID"),SkinID));
				}
			}
			rs.Close();

			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}
		
		static public String OrderPanel(int OrderNumber)
		{	
			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("  <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\n");
			tmpS.Append("    <tr>\n");
			tmpS.Append("      <td width=\"10\" height=\"9\" bgcolor=\"#CCCCCC\"></td>\n");
			tmpS.Append("      <td width=\"*\" height=\"9\" bgcolor=\"#CCCCCC\">&nbsp;Order #: " + OrderNumber.ToString() + "</td>\n");
			tmpS.Append("      <td width=\"10\" height=\"9\" bgcolor=\"#CCCCCC\"></td>\n");
			tmpS.Append("    </tr>\n");
			tmpS.Append("    <tr>\n");
			tmpS.Append("      <td width=\"10\" background=\"../images/table-l.gif\"></td>\n");
			tmpS.Append("      <td width=\"*\" align=\"left\" valign=\"top\">");
					
			tmpS.Append("TBD");
					
			tmpS.Append("	 </td>\n");
			tmpS.Append("      <td width=\"10\" background=\"../images/table-r.gif\"></td>\n");
			tmpS.Append("    </tr>\n");
			tmpS.Append("    <tr>\n");
			tmpS.Append("      <td width=\"10\" height=\"9\" background=\"../images/table-bl.gif\"></td>\n");
			tmpS.Append("      <td width=\"*\" height=\"9\" background=\"../images/table-b.gif\"></td>\n");
			tmpS.Append("      <td width=\"10\" height=\"9\" background=\"../images/table-br.gif\"></td>\n");
			tmpS.Append("    </tr>\n");
			tmpS.Append("  </table>\n");
			return tmpS.ToString();
		}

		static public String GetListForStyleBrowser(int PackID, int ForParentCategoryID, bool isFull)
		{
			String CacheName = "GetListForStyleBrowser" + "_" + PackID.ToString() + "_" + ForParentCategoryID.ToString() + "_" + isFull.ToString();
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(5000);
			String sql = String.Empty;
			if(ForParentCategoryID == 0)
			{
				sql = "Select * from category  " + DB.GetNoLock() + " where (parentCategoryid=0 or ParentCategoryID IS NULL) and deleted=0 and published=1 and categoryid in (select distinct(categoryid) from productcategory  " + DB.GetNoLock() + " where productid in (select productid from product  " + DB.GetNoLock() + " where IsAPack=0 and IsAKit=0 and ShowInProductBrowser=1 and deleted=0 and published=1)) and ShowInProductBrowser=1 order by DisplayOrder";
			}
			else
			{
				sql = "Select * from category  " + DB.GetNoLock() + " where parentCategoryid=" + ForParentCategoryID.ToString() + " and deleted=0 and published=1 and categoryid in (select distinct(categoryid) from productcategory  " + DB.GetNoLock() + " where productid in (select productid from product  " + DB.GetNoLock() + " where IsAPack=0 and IsAKit=0 and ShowInProductBrowser=1 and deleted=0 and published=1)) and ShowInProductBrowser=1 order by DisplayOrder";
			}
			IDataReader rs = DB.GetRS(sql);

			while(rs.Read())
			{
				tmpS.Append("<span class=\"SBCatName\">" + DB.RSField(rs,"Name").ToUpper() + "</span><br>");
				IDataReader rsp = DB.GetRS("Select * from product  " + DB.GetNoLock() + " where productid in (select distinct productid from productcategory  " + DB.GetNoLock() + " where categoryid=" + DB.RSFieldInt(rs,"CategoryID").ToString() + ") and deleted=0 and published=1 and showinproductbrowser=1 and isapack=0 and isakit=0 order by name");
				while(rsp.Read())
				{
					String ProdURL = "pb.aspx?type=" + Common.QueryString("type") + "&PackID=" + PackID.ToString() + "&ProductID=" + DB.RSFieldInt(rsp,"ProductID").ToString() + "&CategoryID=" + DB.RSFieldInt(rs,"CategoryID").ToString() + "&isfull=" + isFull.ToString();
					tmpS.Append("&nbsp;&nbsp;<a class=\"SBProdName\" href=\"" + ProdURL + "\" target=\"pb\">" + DB.RSField(rsp,"Name") + "</a><br>");
				}
				rsp.Close();
				if(Common.CategoryHasSubs(DB.RSFieldInt(rs,"CategoryID")))
				{
					tmpS.Append(Common.GetListForStyleBrowser(PackID,DB.RSFieldInt(rs,"CategoryID"),isFull));
				}
			}
			rs.Close();

			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}
		
		static public String AffiliateEMail(int AffiliateID)
		{
			String tmpS = String.Empty;
			IDataReader rs = DB.GetRS("Select EMail from affiliate  " + DB.GetNoLock() + " where affiliateid=" + AffiliateID.ToString());
			rs.Read();
			tmpS = DB.RSField(rs,"EMail");
			rs.Close();
			return tmpS;
		}

		static public String AffiliateMailingAddress(int AffiliateID, String separator)
		{
			IDataReader rs = DB.GetRS("select * from affiliate  " + DB.GetNoLock() + " where affiliateid=" + AffiliateID.ToString());
			rs.Read();
			StringBuilder tmpS = new StringBuilder(5000);
			tmpS.Append((DB.RSField(rs,"FirstName") + " " + DB.RSField(rs,"LastName")).Trim() + separator);
			if(DB.RSField(rs,"Company").Length != 0)
			{
				tmpS.Append(DB.RSField(rs,"Company") + separator);
			}
			tmpS.Append(DB.RSField(rs,"Address1") + separator);
			if(DB.RSField(rs,"Address2").Length != 0)
			{
				tmpS.Append(DB.RSField(rs,"Address2") + separator);
			}
			if(DB.RSField(rs,"Suite").Length != 0)
			{
				tmpS.Append("Suite: " + DB.RSField(rs,"Suite") + separator);
			}
			tmpS.Append(DB.RSField(rs,"City") + ", " + DB.RSField(rs,"State") + " " + DB.RSField(rs,"Zip") + separator);
			rs.Close();
			return tmpS.ToString();
		}
		
		static public decimal AffiliateBalance(int AffiliateID)
		{
			decimal N =  AffiliateTotalEarnings(AffiliateID) - AffiliateTotalPayouts(AffiliateID);
			if(N < System.Decimal.Zero)
			{
				N = System.Decimal.Zero;
			}
			return N;
		}

		static public void AssignCardToAffiliate(int CustomerID,int AffiliateID, String CardID, decimal Amount, int NumRequested, DateTime RequestedOn, DateTime ExpiresOn)
		{
			long CardIDLong = AffiliateID;
			DB.ExecuteSQL("insert into AffiliateCardAssignment(AffiliateID,CardAmount,StartID,StopID,StartIDInt,StopIDInt,ExpiresOn,RequestedNumber,RequestedOn,IssuedBy) values(" + AffiliateID.ToString() + "," + Localization.CurrencyStringForDB(Amount) + "," + Common.SQuote(CardID) + "," + Common.SQuote(CardID) + "," + CardIDLong.ToString() + "," + CardIDLong.ToString() + "," + DB.SQuote(Localization.ToNativeDateTimeString(ExpiresOn)) + "," + NumRequested.ToString() + "," + DB.SQuote(Localization.ToNativeShortDateString(RequestedOn)) + "," + CustomerID.ToString() + ")");
			// add coupons for each card assigned!
			// add coupons for each card assigned!
			String ThisID = CardID;
			StringBuilder sql = new StringBuilder(5000);
			sql.Append("insert into coupon(CouponCode,ExpirationDate,Description,DiscountPercent,DiscountAmount,DiscountIncludesFreeShipping,LastUpdatedBy) values(");
			sql.Append(Common.SQuote(ThisID) + ",");
			sql.Append(DB.SQuote(Localization.ToNativeShortDateString(ExpiresOn) + ","));
			sql.Append(Common.SQuote("Affiliate Card ID") + ",");
			sql.Append("0.0,"); // discount percent
			sql.Append(Amount.ToString() + ",");
			sql.Append("0,"); // discount includes free shipping
			sql.Append(CustomerID.ToString());
			sql.Append(")");
			DB.ExecuteSQL(sql.ToString());
		}

		static public void AssignCardRangeToAffiliate(int CustomerID,int AffiliateID, String StartingID, String EndingID, decimal Amount, int NumRequested, DateTime RequestedOn, DateTime ExpiresOn)
		{
			String Prefix = CardExtractPrefix(StartingID);
			String StartingIDString = CardExtractTrailingNumberString(StartingID);
			long StartingIDLong = CardExtractTrailingNumberLong(StartingID);
			long EndingIDLong = CardExtractTrailingNumberLong(EndingID);
			DB.ExecuteSQL("insert into AffiliateCardAssignment(AffiliateID,CardAmount,StartID,StopID,StartIDInt,StopIDInt,ExpiresOn,RequestedNumber,RequestedOn,IssuedBy) values(" + AffiliateID.ToString() + "," + Localization.CurrencyStringForDB(Amount)  + "," + Common.SQuote(StartingID) + "," + Common.SQuote(EndingID) + "," + StartingIDLong.ToString() + "," + EndingIDLong.ToString() + "," + DB.SQuote(Localization.ToNativeDateTimeString(ExpiresOn)) + "," + NumRequested.ToString() + "," + DB.SQuote(Localization.ToNativeShortDateString(RequestedOn)) + "," + CustomerID.ToString() + ")");
			for(long l = StartingIDLong; l <= EndingIDLong; l++)
			{
				// add coupons for each card assigned!
				String ThisID = Prefix + l.ToString().PadLeft(StartingIDString.Length,'0');
				StringBuilder sql = new StringBuilder(5000);
				sql.Append("insert into coupon(CouponCode,ExpirationDate,Description,DiscountPercent,DiscountAmount,DiscountIncludesFreeShipping,LastUpdatedBy) values(");
				sql.Append(Common.SQuote(ThisID) + ",");
				sql.Append(DB.SQuote(Localization.ToNativeShortDateString(ExpiresOn)) + ",");
				sql.Append(Common.SQuote("Affiliate Card ID") + ",");
				sql.Append("0.0,"); // discount percent
				sql.Append(Amount.ToString() + ",");
				sql.Append("0,"); // discount includes free shipping
				sql.Append(CustomerID.ToString());
				sql.Append(")");
				DB.ExecuteSQL(sql.ToString());
			}
		}

		static public String CardExtractTrailingNumberString(String CardID)
		{
			// returns long integer which is all "digits" starting from RIGHT of card id until a NON digit is encountered:
			// e.g. CardID "1001004" = 1001004. CardID "AB1001004" = 1001004. CardID "AB9C1001004" = 1001004
			String digits = String.Empty;
			int len = CardID.Length;
			for(int i = len; i > 0; i--)
			{
				if("0123456789".IndexOf(CardID[i-1]) != -1)
				{
					digits = CardID[i-1] + digits;
				}
				else
				{
					break;
				}
			}
			return digits;
		}

		static public long CardExtractTrailingNumberLong(String CardID)
		{
			String digits = CardExtractTrailingNumberString(CardID);
			long tmp = 0;
			if(digits.Length > 0)
			{
				tmp = System.Int64.Parse(digits);
			}
			return tmp;
		}

		static public String CardExtractPrefix(String CardID)
		{
			// returns prefix chars of card id (everything except trailing numbers):
			// e.g. CardID "1001004" = "" (no prefix). CardID "AB1001004" = AB. CardID "AB9C1001004" = AB9C
			String digits = CardExtractTrailingNumberString(CardID);
			if(digits.Length == CardID.Length)
			{
				return String.Empty;
			}
			return CardID.Substring(0,CardID.Length - digits.Length);
		}
		
		static public decimal AffiliateTotalEarnings(int AffiliateID)
		{
			decimal tmp = System.Decimal.Zero;
			IDataReader rs = DB.GetRS("Select sum(amount) as N from affiliateactivity where amount>0 and affiliateid=" + AffiliateID.ToString());
			rs.Read();
			tmp = DB.RSFieldDecimal(rs,"N");
			rs.Close();
			return tmp;
		}

		static public decimal AffiliateTotalPayouts(int AffiliateID)
		{
			decimal tmp = System.Decimal.Zero;
			IDataReader rs = DB.GetRS("Select -sum(amount) as N from affiliateactivity where amount<0 and affiliateid=" + AffiliateID.ToString());
			rs.Read();
			tmp = DB.RSFieldDecimal(rs,"N");
			rs.Close();
			return tmp;
		}

		static public bool GiftCardCodeIsValid(String CardCode)
		{
			return true;
		}

		static public bool IsValidAffiliate(int AffiliateID)
		{
			return (DB.GetSqlN("select count(AffiliateID) as N from affiliate  " + DB.GetNoLock() + " where deleted=0 and affiliateid=" + AffiliateID.ToString()) != 0);
		}
					 		

		static public bool IsOnlineAffiliate(int AffiliateID)
		{
			bool tmp = false;
			IDataReader rs = DB.GetRS("select IsOnline from affiliate  " + DB.GetNoLock() + " where affiliateid=" + AffiliateID.ToString());
			if(rs.Read())
			{
				tmp = DB.RSFieldBool(rs,"IsOnline");
			}
			rs.Close();
			return tmp;
		}

		static public String GetSimpleProductListBox(String ForWhatTable, bool OneLineFormat, Customer thisCustomer, String PageName, int PageSize, int SiteID, int TableID, int FilterTableID, int FilterManufacturerID, int ProductTypeID, bool NoPaging, bool DisallowFeatured, String SearchFor)
		{
			if(DB.GetDBProvider() == "MSACCESS")
			{
				throw new ArgumentException("AspDotNetStorefront: The specified display format cannot be used with the Access DB");
			}

			StringBuilder tmpS = new StringBuilder(10000);
			int OrderBy = Common.QueryStringUSInt("OrderBy");
			if(OrderBy == 0)
			{
				OrderBy = Common.AppConfigUSInt("DefaultSortBy");
			}
			if(OrderBy == 0)
			{
				OrderBy = 1;
			}
			String OrderByClause = "";
			switch(OrderBy)
			{
				case 1:
					OrderByClause = " "+ ForWhatTable + "DisplayOrder.DisplayOrder asc, Product.Name asc, ProductVariant.DisplayOrder asc, ProductVariant.Name asc";
					break;
				case 2:
					OrderByClause = " Product.Name asc, " + ForWhatTable + "DisplayOrder.DisplayOrder asc, ProductVariant.DisplayOrder asc, ProductVariant.Name asc";
					break;
				case 3:
					OrderByClause = " Product.FullSKU asc, " + ForWhatTable + "DisplayOrder.DisplayOrder asc, Product.Name asc, ProductVariant.DisplayOrder asc, ProductVariant.Name asc";
					break;
				case 4:
					OrderByClause = " YourPrice asc, " + ForWhatTable + "DisplayOrder.DisplayOrder asc, Product.Name asc, ProductVariant.DisplayOrder asc, ProductVariant.Name asc";
					break;
				case 5:
					OrderByClause = " YourPrice desc, " + ForWhatTable + "DisplayOrder.DisplayOrder asc, Product.Name asc, ProductVariant.DisplayOrder asc, ProductVariant.Name asc";
					break;
				default:
					OrderByClause = " "+ ForWhatTable + "DisplayOrder.DisplayOrder asc, Product.Name asc, ProductVariant.DisplayOrder asc, ProductVariant.Name asc";
					break;
			}
			String sql =  "SELECT distinct TOP 100 PERCENT  ProductVariant.VariantID, Product.SKU, ProductVariant.SKUSuffix, sku,skusuffix, fullsku = case when skusuffix is null then sku else sku+skusuffix end, price, saleprice, yourprice = case when saleprice is null then price else saleprice end, Product.ProductID, Product.Name, ProductVariant.Name AS VariantName, Product.IsAKit, Product.IsAPack, ProductVariant.DisplayOrder as VDisplayOrder, " + ForWhatTable + "DisplayOrder.DisplayOrder FROM  ((Product  " + DB.GetNoLock() + " INNER JOIN ProductVariant  " + DB.GetNoLock() + " ON Product.ProductID = ProductVariant.ProductID) INNER JOIN " + ForWhatTable + "DisplayOrder ON Product.ProductID = " + ForWhatTable + "DisplayOrder.ProductID) WHERE Product.Published=1 " + Common.IIF(SearchFor.Length != 0, " AND (Product.Name like " + DB.SQuote("%" + SearchFor + "%") + " or Product.Keywords like " + DB.SQuote("%" + SearchFor + "%") + ")","") + " and Product.Deleted=0 AND ProductVariant.Published=1 AND ProductVariant.Deleted=0 " + Common.IIF(TableID != 0, " and product.productid in (select distinct productid from " + ForWhatTable + "displayorder  " + DB.GetNoLock() + " where " + ForWhatTable + "id=" + TableID.ToString() + ")","") +  Common.IIF(FilterManufacturerID != 0, " and product.manufacturerid=" + FilterManufacturerID.ToString(),"") + Common.IIF(ProductTypeID != 0, " and Product.producttypeid=" + ProductTypeID.ToString(),"");
			sql += " and " + ForWhatTable + "DisplayOrder." + ForWhatTable + "ID=" + TableID.ToString();
			if(FilterTableID != 0)
			{
				if(ForWhatTable.ToLower() == "category")
				{
					sql += " and product.productid in (select distinct productid from productsection  " + DB.GetNoLock() + " where sectionid=" + FilterTableID.ToString() + ") ";
				}
				else
				{
					sql += " and product.productid in (select distinct productid from productcategory  " + DB.GetNoLock() + " where categoryid=" + FilterTableID.ToString() + ") ";
				}
			}
			if(Common.AppConfigBool("FilterProductsByAffiliate"))
			{
				sql += " and Product.ProductID in (select distinct productid from productaffiliate  " + DB.GetNoLock() + " where affiliateid=" + thisCustomer._affiliateID.ToString() + ")";
			}
			if(Common.AppConfigBool("FilterProductsByCustomerLevel"))
			{
				String FilterOperator = Common.IIF(Common.AppConfigBool("FilterByCustomerLevelIsAscending"),"<=","=");
				sql += " and (Product.ProductID in (select productid from productCustomerLevel  " + DB.GetNoLock() + " where CustomerLevelid" + FilterOperator + thisCustomer._customerLevelID.ToString() + ") ";
				if(Common.AppConfigBool("FilterByCustomerLevel0SeesUnmappedProducts"))
				{
					// allow customer level 0 to see any product that is not specifically mapped to any customer levels
					sql += " or Product.ProductID not in (select productid from productcustomerlevel)";
				}
				sql += ")";
			}
			sql += " ORDER BY " + OrderByClause;
			if(Common.ApplicationBool("DumpSQL"))
			{
				tmpS.Append("SQL=" + sql + "<br>");
			}
			DataSet ds = DB.GetDS(sql,true);
			bool empty = ds.Tables[0].Rows.Count == 0;

			if(PageSize == 0)
			{
				PageSize = 1000000;
			}
			int NumRows = ds.Tables[0].Rows.Count;
			int PageNum = Common.QueryStringUSInt("PageNum");
			if(PageNum == 0)
			{
				PageNum = 1;
			}
			if(Common.QueryString("show") == "all")
			{
				PageSize = NumRows;
				PageNum = 1;
			}
			int NumPages = (NumRows/PageSize) + Common.IIF(NumRows % PageSize == 0 , 0 , 1);
			if(PageNum > NumPages)
			{
				PageNum = 1;
			}
			int StartRow = (PageSize*(PageNum-1)) + 1;
			int StopRow = StartRow + PageSize -1 ;
			if(StopRow > NumRows)
			{
				StopRow = NumRows;
			}
			
			if(!NoPaging && NumRows > 0)
			{
				tmpS.Append("<form style=\"margin-top: 0px; margin-bottom: 0px;\" method=\"GET\" id=\"SortForm1\" name=\"SortForm1\" action=\"" + PageName + "\">");
				tmpS.Append("<input type=\"hidden\" name=\"" + ForWhatTable + "ID\" value=\"" + TableID.ToString() + "\">");
				tmpS.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
				tmpS.Append("<tr>");
				tmpS.Append("<td align=\"left\">");
				tmpS.Append("<span class=\"PageNumber\">Showing " + StartRow.ToString() + "-" + StopRow.ToString() + " of " + NumRows.ToString() + " items");
				tmpS.Append("<img src=\"images/spacer.gif\" width=\"20\" height=\"1\">Page ");
				for(int u = 1; u <= NumPages; u++)
				{
					if(u % 20 == 0)
					{
						tmpS.Append("<br>");
					}
					if(u > 1)
					{
						tmpS.Append("&nbsp;|&nbsp;");
					}
					if(u == PageNum)
					{
						tmpS.Append(u.ToString());
					}
					else
					{
						tmpS.Append("<a class=\"PageNumber\" href=\"" + PageName + "?" + ForWhatTable + "ID=" + TableID.ToString() + "&pagenum=" + u.ToString() + "&orderby=" + OrderBy.ToString() + "\">" + u.ToString() + "</a>");
					}
				}
				if(NumPages > 1)
				{
					tmpS.Append("<img src=\"images/spacer.gif\" width=\"10\" height=\"1\">");
					tmpS.Append("<a class=\"PageNumber\" href=\"" + PageName + "?" + ForWhatTable + "ID=" + TableID.ToString() + "&pagenum=" + (PageNum+1).ToString() + "&orderby=" + OrderBy.ToString() + "\">Next</a>");
				}
				tmpS.Append("</td><td align=\"right\">");
				if(Common.AppConfigBool("ShowSortByPrice"))
				{
					tmpS.Append("<span class=\"PageNumber\">Sort By: </span>");
					tmpS.Append("<select id=\"OrderBy\" name=\"OrderBy\" size=\"1\" onChange=\"document.SortForm1.submit();\">");
					tmpS.Append("<option value=\"1\"" + Common.IIF(OrderBy == 1, " selected","") + ">Display Order</option>");
					tmpS.Append("<option value=\"2\"" + Common.IIF(OrderBy == 2, " selected","") + ">Name</option>");
					tmpS.Append("<option value=\"3\"" + Common.IIF(OrderBy == 3, " selected","") + ">SKU</option>");
					tmpS.Append("<option value=\"4\"" + Common.IIF(OrderBy == 4, " selected","") + ">Price: Low To High</option>");
					tmpS.Append("<option value=\"5\"" + Common.IIF(OrderBy == 5, " selected","") + ">Price: High To Low</option>");
					tmpS.Append("</select>");
					tmpS.Append("</span>\n");
				}
				tmpS.Append("</td></tr></table>");
				tmpS.Append("</form>");
				//}
			}
			
			// *******************************************


				
			if(empty)
			{
				String SS = String.Empty;
				tmpS.Append("No products were found.</td></tr>");
			}
			else
			{
				tmpS.Append("<table border=\"0\" cellpadding=\"0\" " + Common.IIF(OneLineFormat,"style=\"border-style: single; border-width: 1px; border-color=#" + Common.AppConfig("LightCellColor") + ";\"","") + " cellspacing=\"" + Common.IIF(OneLineFormat,"1","4") + "\" width=\"100%\">");
				if(OneLineFormat)
				{
					tmpS.Append("<tr bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\">");
					tmpS.Append("<td><b>SKU</b></td>");
					tmpS.Append("<td><b>Description</b></td>");
					tmpS.Append("<td><b>Price</b></td>");
					tmpS.Append("<td><b>Add</b></td>");
					tmpS.Append("</tr>");
				}
				int rowi = 1; // 1 based!
				bool first = true;
				foreach(DataRow row in ds.Tables[0].Rows)
				{
					if(rowi >= StartRow && rowi <= StopRow)
					{
						bool IsOnSale = false;
						int VariantID = DB.RowFieldInt(row,"VariantID");
						int CustomerLevelID = thisCustomer._customerLevelID;
						decimal YourPR = Common.DetermineLevelPrice(VariantID,CustomerLevelID,out IsOnSale);
						decimal RegularPR = Common.GetVariantPrice(VariantID);
						String URL = String.Empty;
						if(ForWhatTable.ToLower() == "category")
						{
							URL = SE.MakeProductAndCategoryLink(DB.RowFieldInt(row,"ProductID"),TableID,String.Empty);
						}
						else
						{
							URL = SE.MakeProductAndSectionLink(DB.RowFieldInt(row,"ProductID"),TableID,String.Empty);
						}
						if(OneLineFormat)
						{
							tmpS.Append("<tr>");
							tmpS.Append("<td valign=\"middle\" align=\"left\">" + DB.RowField(row,"FullSKU") + "</td>");
							tmpS.Append("<td valign=\"middle\" align=\"left\"><a href=\"" + URL + "\">" + Common.MakeProperProductName(DB.RowField(row,"Name"),DB.RowField(row,"VariantName")) + "</a></td>");
							tmpS.Append("<td valign=\"middle\" align=\"left\">");
							if(DB.RowFieldBool(row,"IsAPack") || DB.RowFieldBool(row,"IsAKit"))
							{
								tmpS.Append("&nbsp;");
							}
							else
							{
								tmpS.Append(Localization.CurrencyStringForDisplay(YourPR));
							}
							tmpS.Append("</td>");
							tmpS.Append("<td valign=\"middle\" align=\"left\">");
							if(DB.RowFieldBool(row,"IsAPack") || DB.RowFieldBool(row,"IsAKit"))
							{
								tmpS.Append("<a href=\"showproduct.aspx?productid=" + DB.RowFieldInt(row,"ProductID").ToString() + "\"><img src=\"skins/skin_" + SiteID.ToString() + "/images/moreinfo.gif\" border=\"0\"></a>");
							}
							else
							{
								tmpS.Append("<a href=\"addtocart.aspx?returnurl=" + HttpContext.Current.Server.UrlEncode(Common.GetThisPageName(false) + "?" + Common.ServerVariables("QUERY_STRING")) + "&productid=" + DB.RowFieldInt(row,"ProductID").ToString() + "&variantid=" + DB.RowFieldInt(row,"VariantID").ToString() + "\">" + Common.AppConfig("CartButtonPrompt") + "</a>");
							}
							tmpS.Append("</td>");
							tmpS.Append("<tr><td bgcolor=\"#" + Common.AppConfig("LightCellColor") + "\" colspan=4 height=1><img src=\"images/spacer.gif\" width=1 height=1></td></tr>");
							tmpS.Append("</tr>");
						}
						else
						{
							if(!first)
							{
								tmpS.Append("<tr><td colspan=4><hr size=1></td></tr>");
							}
							tmpS.Append("<tr>");
							tmpS.Append("<td align=\"left\" valign=\"top\">");
							String ImgUrl = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",SiteID);
							if(ImgUrl.Length == 0)
							{
								ImgUrl = Common.AppConfig("NoPicture");
							}
							tmpS.Append("<img src=\"images/spacer.gif\" width=\"25\" height=\"1\"><img alt=\"\" onClick=\"self.location='" + URL + "';\" style=\"cursor: hand;\" src=\"" + ImgUrl + "\" border=\"0\" >");
							tmpS.Append("</td>");
							tmpS.Append("<td align=\"left\" valign=\"top\">");
							tmpS.Append("<a class=\"SectionNameText\" href=\"" + URL + "\"><b>");
							tmpS.Append(Common.MakeProperProductName(DB.RowField(row,"Name"),DB.RowField(row,"VariantName")) + "</b><br><small>(" + DB.RowField(row,"FullSKU") + ")</small></a>");
							tmpS.Append("<br>");
							if(Common.AppConfigBool("ShowSummaryInTableOrderFormat"))
							{
								tmpS.Append(Common.Ellipses(Common.GetProductSummary(DB.RowFieldInt(row,"ProductID")),200,true));
							}
							else
							{
								tmpS.Append(Common.Ellipses(Common.GetProductDescription(DB.RowFieldInt(row,"ProductID")),200,true));
							}
							tmpS.Append("<br>");
							tmpS.Append("<img src=\"images/spacer.gif\" width=\"125\" height=\"5\"><br>");
							tmpS.Append("<a class=\"SectionNameText\" href=\"" + URL + "\">(read more...)</a>");
							tmpS.Append("</td>");
							tmpS.Append("<td align=\"center\" valign=\"top\">");
							if(DB.RowFieldBool(row,"IsAPack") || DB.RowFieldBool(row,"IsAKit"))
							{
								tmpS.Append("&nbsp;");
							}
							else
							{
								if(CustomerLevelID == 0)
								{
									if(IsOnSale)
									{
										tmpS.Append("<font class=\"ShowPriceRegularPrompt\"><strike>" + Common.AppConfig("ShowPriceRegularPrompt") + ": " + Localization.CurrencyStringForDisplay(RegularPR) + "</strike></font>");
										tmpS.Append("<br><font class=\"ShowPriceSalePrompt\">" + Common.AppConfig("ShowPriceSalePrompt") + ": " + Localization.CurrencyStringForDisplay(YourPR) + "</font>");
									}
									else
									{
										tmpS.Append("<font class=\"ShowPriceRegularPrompt\">" + Common.AppConfig("ShowPriceRegularPrompt") + ": " + Localization.CurrencyStringForDisplay(RegularPR) + "</font>");
									}
								}
								else
								{
									if(IsOnSale)
									{
										tmpS.Append("<font class=\"ShowPriceRegularPrompt\"><strike>" + Common.AppConfig("ShowPriceRegularPrompt") + ": " + Localization.CurrencyStringForDisplay(RegularPR) + "</strike></font>");
										tmpS.Append("<br><font class=\"ShowPriceSalePrompt\">" + Common.AppConfig("ShowPriceSalePrompt") + ": " + Localization.CurrencyStringForDisplay(YourPR) + "</font>");
									}
									else
									{
										tmpS.Append("<font class=\"ShowPriceRegularPrompt\"><strike>" + Common.AppConfig("ShowPriceRegularPrompt") + ": " + Localization.CurrencyStringForDisplay(RegularPR) + "</strike></font>");
									}
									tmpS.Append("<br><font class=\"ShowPriceExtendedPrompt\">" + Common.AppConfig("ShowPriceExtendedPrompt") + ": " + Localization.CurrencyStringForDisplay(YourPR) + "</font>");
								}
							}
							tmpS.Append("</td>");

							tmpS.Append("<td align=\"center\" valign=\"top\">");

							if(Common.AppConfigBool("ShowInventoryTable"))
							{
								tmpS.Append("<div align=\"left\"><br>");
								tmpS.Append(Common.GetInventoryTable(DB.RowFieldInt(row,"ProductID"),Common.GetProductsFirstVariantID(DB.RowFieldInt(row,"ProductID")),thisCustomer._isAdminUser));
								tmpS.Append("</div>");
								tmpS.Append("<br>");
							}

							if(DB.RowFieldBool(row,"IsAPack") || DB.RowFieldBool(row,"IsAKit"))
							{
								tmpS.Append("<a href=\"showproduct.aspx?productid=" + DB.RowFieldInt(row,"ProductID").ToString() + "\"><img src=\"skins/skin_" + SiteID.ToString() + "/images/moreinfo.gif\" border=\"0\"></a>");
							}
							else
							{
								tmpS.Append(Common.GetAddToCartForm(false,Common.AppConfigBool("ShowWishButtons"),DB.RowFieldInt(row,"ProductID"),DB.RowFieldInt(row,"VariantID"),SiteID,2,false));
							}
							tmpS.Append("</td>");
							tmpS.Append("</tr>");
						}
						first = false;
					}
					rowi++;
				}
				tmpS.Append("</table>");
				tmpS.Append("</form>");
			}


			if(!NoPaging && NumRows > 0)
			{
				tmpS.Append("<form style=\"margin-top: 0px; margin-bottom: 0px;\" method=\"GET\" id=\"SortForm2\" name=\"SortForm2\" action=\"" + PageName + "\">");
				tmpS.Append("<input type=\"hidden\" name=\"" + ForWhatTable + "ID\" value=\"" + TableID.ToString() + "\">");
				tmpS.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
				tmpS.Append("<tr>");
				tmpS.Append("<td align=\"left\">");
				tmpS.Append("<span class=\"PageNumber\">Showing " + StartRow.ToString() + "-" + StopRow.ToString() + " of " + NumRows.ToString() + " items");
				tmpS.Append("<img src=\"images/spacer.gif\" width=\"20\" height=\"1\">Page ");
				for(int u = 1; u <= NumPages; u++)
				{
					if(u % 20 == 0)
					{
						tmpS.Append("<br>");
					}
					if(u > 1)
					{
						tmpS.Append("&nbsp;|&nbsp;");
					}
					if(u == PageNum)
					{
						tmpS.Append(u.ToString());
					}
					else
					{
						tmpS.Append("<a class=\"PageNumber\" href=\"" + PageName + "?" + ForWhatTable + "ID=" + TableID.ToString() + "&pagenum=" + u.ToString() + "&orderby=" + OrderBy.ToString() + "\">" + u.ToString() + "</a>");
					}
				}
				if(NumPages > 1)
				{
					tmpS.Append("<img src=\"images/spacer.gif\" width=\"10\" height=\"1\">");
					tmpS.Append("<a class=\"PageNumber\" href=\"" + PageName + "?" + ForWhatTable + "ID=" + TableID.ToString() + "&pagenum=" + (PageNum+1).ToString() + "&orderby=" + OrderBy.ToString() + "\">Next</a>");
				}
				tmpS.Append("</td><td align=\"right\">");
				if(Common.AppConfigBool("ShowSortByPrice"))
				{
					tmpS.Append("<span class=\"PageNumber\">Sort By: </span>");
					tmpS.Append("<select id=\"OrderBy\" name=\"OrderBy\" size=\"1\" onChange=\"document.SortForm2.submit();\">");
					tmpS.Append("<option value=\"1\"" + Common.IIF(OrderBy == 1, " selected","") + ">Display Order</option>");
					tmpS.Append("<option value=\"2\"" + Common.IIF(OrderBy == 2, " selected","") + ">Name</option>");
					tmpS.Append("<option value=\"3\"" + Common.IIF(OrderBy == 3, " selected","") + ">SKU</option>");
					tmpS.Append("<option value=\"4\"" + Common.IIF(OrderBy == 4, " selected","") + ">Price: Low To High</option>");
					tmpS.Append("<option value=\"5\"" + Common.IIF(OrderBy == 5, " selected","") + ">Price: High To Low</option>");
					tmpS.Append("</select>");
					tmpS.Append("</span>\n");
				}
				tmpS.Append("</td></tr></table>");
				tmpS.Append("</form>");
				//}
			}

			ds.Dispose();

			// *********************************************


			//			tmpS.Append("</td></tr>\n");
			//			tmpS.Append("</table>\n");
			//			tmpS.Append("</td></tr>\n");
			//			tmpS.Append("</table>\n");

			return tmpS.ToString();
		}

		public static String GetProductDescription(int ProductID)
		{
			IDataReader rs = DB.GetRS("select Description from product  " + DB.GetNoLock() + " where productid=" + ProductID.ToString());
			String tmpS = String.Empty;
			if(rs.Read())
			{
				tmpS = DB.RSField(rs,"Description");
			}
			rs.Close();
			return tmpS;
		}

		public static String GetProductSummary(int ProductID)
		{
			IDataReader rs = DB.GetRS("select Summary from product  " + DB.GetNoLock() + " where productid=" + ProductID.ToString());
			String tmpS = String.Empty;
			if(rs.Read())
			{
				tmpS = DB.RSField(rs,"Summary");
			}
			rs.Close();
			return tmpS;
		}

		static public void PresetPack(Customer thisCustomer, int PackID, bool IsWish, out decimal PresetPackPrice, out String PresetPackProducts)
		{
			PresetPackProducts = String.Empty;
			PresetPackPrice = System.Decimal.Zero;
			IDataReader rs2 = DB.GetRS("select relatedproducts from product  " + DB.GetNoLock() + " where productid=" + PackID.ToString());
			if(rs2.Read())
			{
				PresetPackProducts = DB.RSField(rs2,"RelatedProducts");
			}
			rs2.Close();
			if(PresetPackProducts.Length != 0)
			{
				thisCustomer.RequireCustomerRecord();
				//try
				//{
				CustomCart cart = new CustomCart(thisCustomer._customerID,PackID,1);
				IDataReader rs = DB.GetRS("select productid,ShowInProductBrowser from product  " + DB.GetNoLock() + " where product.ProductID in (" + PresetPackProducts + ")");
				// JUST add first variant!
				while(rs.Read())
				{
					int PID = DB.RSFieldInt(rs,"ProductID");
					int VID = Common.GetProductsFirstVariantID(DB.RSFieldInt(rs,"ProductID"));
					bool ProductFound = false;
					foreach(CustomItem c in	cart._cartItems)
					{
						if(PID == c.productID && VID == c.variantID && c.chosenColor == "" && c.chosenSize == "")
						{
							ProductFound = true;
						}
					}
					if(!ProductFound)
					{
						cart.AddItem(PID,VID,1,String.Empty,String.Empty,String.Empty,String.Empty);
					}
					bool IsOnSale = false; // don't care
					decimal PR = Common.DetermineLevelPrice(VID,thisCustomer._customerLevelID,out IsOnSale);
					PresetPackPrice += PR;
				}
				rs.Close();
				//}
				//catch {}
			}
		}


		static public String GetCategorySelectList(int ForParentCategoryID, String Prefix, int FilterCategoryID)
		{
			String CacheName = "GetCategorySelectList" + ForParentCategoryID.ToString() + "_Adm";
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(5000);
			String sql = String.Empty;
			if(ForParentCategoryID == 0)
			{
				sql = "select * from Category  " + DB.GetNoLock() + " where (parentCategoryid=0 or ParentCategoryID IS NULL) and published<>0 and deleted=0 order by DisplayOrder,Name";
			}
			else
			{
				sql = "select * from Category  " + DB.GetNoLock() + " where parentCategoryid=" + ForParentCategoryID.ToString() + " and published<>0 and deleted=0 order by DisplayOrder,Name";
			}
			IDataReader rs = DB.GetRS(sql);

			if(ForParentCategoryID == 0)
			{
				Prefix = String.Empty;
			}
			else
			{
				Prefix = Prefix + Common.GetCategoryName(ForParentCategoryID) + " >> ";
			}

			while(rs.Read())
			{
				if(FilterCategoryID != DB.RSFieldInt(rs,"CategoryID"))
				{
					tmpS.Append("<option value=\"" + DB.RSFieldInt(rs,"CategoryID").ToString() + "\">" + HttpContext.Current.Server.HtmlEncode(Prefix + DB.RSField(rs,"Name")) + "</option>");
				}
				if(Common.CategoryHasSubs(DB.RSFieldInt(rs,"CategoryID")))
				{
					tmpS.Append(Common.GetCategorySelectList(DB.RSFieldInt(rs,"CategoryID"),Prefix,FilterCategoryID));
				}
			}
			rs.Close();
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public String GetSectionSelectList(int ForParentSectionID, String Prefix, int FilterSectionID)
		{
			String CacheName = "GetSectionSelectList" + ForParentSectionID.ToString() + "_Adm";
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(5000);
			String sql = String.Empty;
			if(ForParentSectionID == 0)
			{
				sql = "select * from [Section]  " + DB.GetNoLock() + " where (parentSectionid=0 or ParentSectionID IS NULL) and published<>0 and deleted=0 order by DisplayOrder,Name";
			}
			else
			{
				sql = "select * from [Section]  " + DB.GetNoLock() + " where parentSectionid=" + ForParentSectionID.ToString() + " and published<>0 and deleted=0 order by DisplayOrder,Name";
			}
			IDataReader rs = DB.GetRS(sql);

			if(ForParentSectionID == 0)
			{
				Prefix = String.Empty;
			}
			else
			{
				Prefix = Prefix + Common.GetSectionName(ForParentSectionID) + " >> ";
			}

			while(rs.Read())
			{
				if(FilterSectionID != DB.RSFieldInt(rs,"SectionID"))
				{
					tmpS.Append("<option value=\"" + DB.RSFieldInt(rs,"SectionID").ToString() + "\">" + HttpContext.Current.Server.HtmlEncode(Prefix + DB.RSField(rs,"Name")) + "</option>");
				}
				if(Common.SectionHasSubs(DB.RSFieldInt(rs,"SectionID")))
				{
					tmpS.Append(Common.GetSectionSelectList(DB.RSFieldInt(rs,"SectionID"),Prefix,FilterSectionID));
				}
			}
			rs.Close();
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public String GetCAVV(int OrderNumber)
		{
			String tmpS = String.Empty;
			IDataReader rs = DB.GetRS("Select CardinalAuthenticateResult from orders where ordernumber=" + OrderNumber.ToString());
			if(rs.Read())
			{
				tmpS = Common.ExtractToken(DB.RSField(rs,"CardinalAuthenticateResult"),"<Cavv>","</Cavv>");
			}
			rs.Close();
			return tmpS;
		}

		static public int GetNextOrderNumber()
		{
			String NewGUID = Common.GetNewGUID();
			DB.ExecuteSQL("insert into OrderNumbers(OrderNumberGUID) values(" + DB.SQuote(NewGUID) + ")");
			int tmp = 0;
			IDataReader rs = DB.GetRS("Select ordernumber from OrderNumbers where OrderNumberGUID=" + DB.SQuote(NewGUID));
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"ordernumber");
			}
			rs.Close();
			return tmp;
		}

		static public int LookupCategoryID(String Name)
		{
			int tmp = 0;
			IDataReader rs = DB.GetRS("Select CategoryID from category  " + DB.GetNoLock() + " where deleted=0 and lower(name)=" + DB.SQuote(Name.ToLower()));
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"CategoryID");
			}
			rs.Close();
			return tmp;
		}

		static public String TransactionMode()
		{
			String tmpS = Common.AppConfig("TransactionMode").ToUpper();
			if(tmpS.Length == 0)
			{
				tmpS = "AUTH CAPTURE"; // set some default
			}
			return tmpS;
		}

		static public Hashtable LoadAppConfig()
		{
			IDataReader rs = DB.GetRS("Select * from appconfig " + DB.GetNoLock());
			Hashtable ht = new Hashtable();
			while(rs.Read())
			{
				// ignore dups, first one in wins:
				if(!ht.Contains(DB.RSField(rs,"Name").ToLower()))
				{
					ht.Add(DB.RSField(rs,"Name").ToLower(),DB.RSField(rs,"ConfigValue"));
				}
			}
			rs.Close();
			return ht;
		}

		public static void SetAppConfig(String Name, String ConfigValue, bool ClearCache)
		{
			if(DB.GetSqlN("select count(name) as N from AppConfig  " + DB.GetNoLock() + " where lower(name)=" + DB.SQuote(Name.ToLower())) == 0)
			{
				DB.ExecuteSQL("insert into AppConfig(name,configvalue) values(" + DB.SQuote(Name.ToLower()) + "," + DB.SQuote(ConfigValue) + ")");
				AppConfigTable.Add(Name.ToLower(),ConfigValue);
			}
			else
			{
				DB.ExecuteSQL("update AppConfig set ConfigValue=" + DB.SQuote(ConfigValue) + " where lower(name)=" + DB.SQuote(Name.ToLower()));
				AppConfigTable[Name.ToLower()] = ConfigValue;
			}
			if(ClearCache)
			{
				Common.ClearCache();
			}
		}
		
		static public bool AnyCustomerHasUsedCoupon(String CouponCode)
		{
			return (DB.GetSqlN("select count(ordernumber) as N from orders  " + DB.GetNoLock() + " where lower(CouponCode)=" + DB.SQuote(CouponCode.ToLower())) != 0);
		}

		static public int LookupProductID(String Name)
		{
			int tmp = 0;
			IDataReader rs = DB.GetRS("Select productid from product  " + DB.GetNoLock() + " where deleted=0 and lower(name)=" + DB.SQuote(Name.ToLower()));
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"ProductID");
			}
			rs.Close();
			return tmp;
		}

		static public int GetNumberOfCouponUses(String CouponCode)
		{
			int tmp = 0;
			IDataReader rs = DB.GetRS("Select NumUses from coupon  " + DB.GetNoLock() + " where lower(CouponCode)=" + DB.SQuote(CouponCode.ToLower()));
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"NumUses");
			}
			rs.Close();
			return tmp;
		}

		static public bool IntegerIsInIntegerList(int SearchInt, String ListOfInts)
		{
			try
			{
				String target = SearchInt.ToString();
				if(ListOfInts.Length == 0)
				{
					return false;
				}
				String[] s = ListOfInts.Split(',');
				foreach(string spat in s)
				{
					if(target == spat)
					{
						return true;
					}
				}
				return false;
			}
			catch
			{
				return false;
			}
		}

		static public void RecordCouponUsage(int CustomerID, String CouponCode)
		{
			if(CouponCode.Length != 0)
			{
				try
				{
					DB.ExecuteSQL("update coupon set NumUses=NumUses+1 where lower(CouponCode)=" + DB.SQuote(CouponCode.ToLower()));
					DB.ExecuteSQL("insert into CouponUsage(CustomerID,CouponCode) values(" + CustomerID.ToString() + "," + DB.SQuote(CouponCode) + ")");
				}
				catch {}
			}
		}
		
		static public String GetInventoryTable(int ProductID, int VariantID, bool ShowActualValues)
		{
			String CacheName = "GetInventoryTable_" + ProductID.ToString() + "_" + VariantID.ToString() + "_" + ShowActualValues.ToString();
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}
			StringBuilder tmpS = new StringBuilder(10000);

			if(Common.ProductUsesAdvancedInventoryMgmt(ProductID))
			{
				bool ProductTracksInventoryBySize = Common.ProductTracksInventoryBySize(ProductID);
				bool ProductTracksInventoryByColor = Common.ProductTracksInventoryByColor(ProductID);
				IDataReader rs = DB.GetRS("select product.sizeoptionprompt,product,coloroptionprompt,productvariant.* from productvariant  " + DB.GetNoLock() + " inner join Product " + DB.GetNoLock() + " on productvariant.productid=product.productid where VariantID=" + VariantID.ToString());
				rs.Read();
				String ProductName = Common.GetProductName(ProductID);
				String ProductSKU = Common.GetProductSKU(ProductID);
				String VariantName = Common.GetVariantName(VariantID);
				String VariantSKU = Common.GetVariantSKUSuffix(VariantID);

				String Sizes = DB.RSField(rs,"Sizes");
				String Colors = DB.RSField(rs,"Colors");

				if(!ProductTracksInventoryBySize)
				{
					Sizes = String.Empty;
				}
				if(!ProductTracksInventoryByColor)
				{
					Colors = String.Empty;
				}

				String[] ColorsSplit = Colors.Split(',');
				String[] SizesSplit = Sizes.Split(',');
			
				tmpS.Append("<table border=\"0\" width=\"90%\" align=\"center\" cellpadding=\"1\" cellspacing=\"1\" style=\"border-style: solid; border-color: #EEEEEE; border-width: 1px;\">\n");
				tmpS.Append("<tr>\n");


				String SizeOptionPrompt = DB.RSField(rs,"SizeOptionPrompt");
				String ColorOptionPrompt = DB.RSField(rs,"ColorOptionPrompt");
				if(SizeOptionPrompt.Length == 0)
				{
					SizeOptionPrompt = Common.AppConfig("SizeOptionPrompt");
				}
				if(SizeOptionPrompt.Length == 0)
				{
					SizeOptionPrompt = "Size";
				}
				if(ColorOptionPrompt.Length == 0)
				{
					ColorOptionPrompt = Common.AppConfig("ColorOptionPrompt");
				}
				if(ColorOptionPrompt.Length == 0)
				{
					ColorOptionPrompt = "Color";
				}

				tmpS.Append("<td valign=\"middle\" align=\"right\"><b>" + SizeOptionPrompt + " &amp; " + ColorOptionPrompt + " In Stock</b></td>\n");
				for(int i = SizesSplit.GetLowerBound(0); i <= SizesSplit.GetUpperBound(0); i++)
				{
					tmpS.Append("<td valign=\"middle\" align=\"center\"><b>" + Common.CleanSizeColorOption(SizesSplit[i]) + "</b></td>\n");
				}
				tmpS.Append("</tr>\n");
				int FormFieldID = 1000; // arbitrary number
				for(int i = ColorsSplit.GetLowerBound(0); i <= ColorsSplit.GetUpperBound(0); i++)
				{
					tmpS.Append("<tr>\n");
					tmpS.Append("<td valign=\"middle\" align=\"right\"><b>" + Common.CleanSizeColorOption(ColorsSplit[i]) + "</b></td>\n");
					for(int j = SizesSplit.GetLowerBound(0); j <= SizesSplit.GetUpperBound(0); j++)
					{
						tmpS.Append("<td valign=\"middle\" align=\"center\">");
						if(ShowActualValues)
						{
							tmpS.Append(Common.GetInventory(ProductID,VariantID,CleanSizeColorOption(SizesSplit[j]),CleanSizeColorOption(ColorsSplit[i])));
						}
						else
						{
							tmpS.Append(Common.IIF(Common.GetInventory(ProductID,VariantID,CleanSizeColorOption(SizesSplit[j]),CleanSizeColorOption(ColorsSplit[i])) > 0 , "Yes" , "No"));
						}
						FormFieldID++;
						tmpS.Append("</td>\n");
					}
					tmpS.Append("</tr>\n");
				}

				tmpS.Append("</table>\n");
				rs.Close();
			}
			else
			{
				IDataReader rs = DB.GetRS("Select inventory from productvariant where variantid=" + VariantID.ToString());
				if(rs.Read())
				{
					if(ShowActualValues)
					{
						tmpS.Append(DB.RSFieldInt(rs,"Inventory").ToString());
					}
					else
					{
						tmpS.Append(Common.IIF(DB.RSFieldInt(rs,"Inventory") > 0 , "In Stock" , "Not In Stock"));
					}
				}
				rs.Close();
			}
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public String GetInventoryList(int ProductID, int VariantID)
		{
			String CacheName = "GetInventoryList_" + ProductID.ToString() + "_" + VariantID.ToString();
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}
			bool ProductTracksInventoryBySize = Common.ProductTracksInventoryBySize(ProductID);
			bool ProductTracksInventoryByColor = Common.ProductTracksInventoryByColor(ProductID);
			StringBuilder tmpS = new StringBuilder(10000);
			IDataReader rs = DB.GetRS("select * from productvariant  " + DB.GetNoLock() + " where VariantID=" + VariantID.ToString());
			rs.Read();
			String ProductName = Common.GetProductName(ProductID);
			String ProductSKU = Common.GetProductSKU(ProductID);
			String VariantName = Common.GetVariantName(VariantID);
			String VariantSKU = Common.GetVariantSKUSuffix(VariantID);

			String Sizes = DB.RSField(rs,"Sizes");
			String Colors = DB.RSField(rs,"Colors");

			if(!ProductTracksInventoryBySize)
			{
				Sizes = String.Empty;
			}
			if(!ProductTracksInventoryByColor)
			{
				Colors = String.Empty;
			}

			String[] ColorsSplit = Colors.Split(',');
			String[] SizesSplit = Sizes.Split(',');
			
			bool first = true;
			for(int i = ColorsSplit.GetLowerBound(0); i <= ColorsSplit.GetUpperBound(0); i++)
			{
				for(int j = SizesSplit.GetLowerBound(0); j <= SizesSplit.GetUpperBound(0); j++)
				{
					int qty = Common.GetInventory(ProductID,VariantID,CleanSizeColorOption(SizesSplit[j]),CleanSizeColorOption(ColorsSplit[i]));
					if(!first)
					{
						tmpS.Append("|");
					}
					tmpS.Append(CleanSizeColorOption(ColorsSplit[i]) + "," + CleanSizeColorOption(SizesSplit[j]) + "," + qty.ToString());
					first = false;
				}
				first = false;
			}

			rs.Close();
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}


		static public int GetProductsFirstVariantID(int ProductID)
		{
			int tmp = 0;
			IDataReader rs = DB.GetRS("select min(variantid) as VID from productvariant  " + DB.GetNoLock() + " where deleted=0 and published<>0 and productid=" + ProductID.ToString());
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"VID");
			}
			rs.Close();
			return tmp;
		}

		static public void ProcessKitForm(Customer thisCustomer, int ProductID)
		{
			thisCustomer.RequireCustomerRecord();
			int VariantID = Common.GetFirstProductVariant(ProductID);
			DB.ExecuteSQL("delete from KitCart where CustomerID=" + thisCustomer._customerID.ToString() + " and ProductID=" + ProductID.ToString() + " and ShoppingCartRecID=0");
			for(int i = 0; i<=HttpContext.Current.Request.Form.Count-1; i++)
			{
				if(HttpContext.Current.Request.Form.Keys[i].StartsWith("KitGroupID"))
				{
					int thisID = Localization.ParseUSInt(HttpContext.Current.Request.Form.Keys[i].Split('_')[1]);
					String[] thisVal = Common.Form(HttpContext.Current.Request.Form.Keys[i]).Split(',');
					foreach(String s in thisVal)
					{
						DB.ExecuteSQL("insert into kitcart(customerID,ProductID,VariantID,KitGroupID,KitItemID,CartType) values (" + thisCustomer._customerID.ToString() + "," + ProductID.ToString() + "," + VariantID.ToString() + "," + thisID.ToString() + "," + s + "," + ((int)CartTypeEnum.ShoppingCart).ToString() + ")");
					}
				}
			}
		}

		static public bool KitContainsItem(int CustomerID, int ProductID, int ShoppingCartRecID, int KitItemID)
		{
			return DB.GetSqlN("select count(*) as N from kitcart  " + DB.GetNoLock() + " where customerid="+ CustomerID.ToString() + " and productid=" + ProductID.ToString() + " and ShoppingCartrecid=" + ShoppingCartRecID.ToString() + " and kititemid=" + KitItemID.ToString()) > 0;
		}

		static public bool KitContainsAnyGroupItems(int CustomerID, int ProductID, int ShoppingCartRecID, int KitGroupID)
		{
			return DB.GetSqlN("select count(*) as N from kitcart  " + DB.GetNoLock() + " where customerid="+ CustomerID.ToString() + " and productid=" + ProductID.ToString() + " and ShoppingCartrecid=" + ShoppingCartRecID.ToString() + " and kititemid in (select kititemid from kititem  " + DB.GetNoLock() + " where kitgroupid=" + KitGroupID.ToString() + ")") > 0;
		}

		static public decimal KitPriceDelta(int CustomerID, int ProductID, int ShoppingCartRecID)
		{
			decimal tmp = System.Decimal.Zero;
			if(CustomerID != 0)
			{
				IDataReader rs = DB.GetRS("select sum(quantity*pricedelta) as PR from kitcart  " + DB.GetNoLock() + " inner join kititem  " + DB.GetNoLock() + " on kitcart.kititemid=kititem.kititemid where customerid=" + CustomerID.ToString() + " and productid=" + ProductID.ToString() + " and ShoppingCartrecid=" + ShoppingCartRecID.ToString());
				if(rs.Read())
				{
					tmp = DB.RSFieldDecimal(rs,"PR");
				}
				rs.Close();
			}
			return tmp;
		}

		static public decimal GetColorAndSizePriceDelta(String ChosenColor, String ChosenSize)
		{
			decimal price = System.Decimal.Zero;
			String ColorPriceModifier = String.Empty;
			String SizePriceModifier = String.Empty;
			if(ChosenColor.IndexOf("[") != -1)
			{
				int i1 = ChosenColor.IndexOf("[");
				int i2 = ChosenColor.IndexOf("]");
				if(i1 != -1 && i2 != -1)
				{
					ColorPriceModifier = ChosenColor.Substring(i1+1,i2-i1-1);
				}
			}
			if(ChosenSize.IndexOf("[") != -1)
			{
				int i1 = ChosenSize.IndexOf("[");
				int i2 = ChosenSize.IndexOf("]");
				if(i1 != -1 && i2 != -1)
				{
					SizePriceModifier = ChosenSize.Substring(i1+1,i2-i1-1);
				}
			}

			if(ColorPriceModifier.Length != 0)
			{
				price += Localization.ParseUSDecimal(ColorPriceModifier);
			}
			if(SizePriceModifier.Length != 0)
			{
				price += Localization.ParseUSDecimal(SizePriceModifier);
			}		
			return price;
		}

		static public decimal PackPriceDelta(int CustomerID, int CustomerLevelID, int PackID, int ShoppingCartRecID)
		{
			decimal tmp = System.Decimal.Zero;
			if(CustomerID != 0)
			{
				IDataReader rs = DB.GetRS("select * from customcart  " + DB.GetNoLock() + " where customerid=" + CustomerID.ToString() + " and packid=" + PackID.ToString() + " and ShoppingCartrecid=" + ShoppingCartRecID.ToString());
				bool isonsale = false; // not used
				while(rs.Read())
				{
					int ProductID = DB.RSFieldInt(rs,"ProductID");
					int VariantID = DB.RSFieldInt(rs,"ProductID");
					decimal PR = Common.DetermineLevelPrice(VariantID,CustomerLevelID,out isonsale);
					tmp += DB.RSFieldInt(rs,"Quantity") * PR;
				}
				rs.Close();
			}
			return tmp;
		}

		static public String GetCountryTwoLetterISOCode(String CountryName)
		{
			String tmp = "US"; // default to US just in case
			IDataReader rs = DB.GetRS("select * from country  " + DB.GetNoLock() + " where lower(name)=" + DB.SQuote(CountryName.ToLower()));
			if(rs.Read())
			{
				tmp = DB.RSField(rs,"TwoLetterISOCode");
			}
			rs.Close();
			return tmp;
		}

		static public String GetCountryThreeLetterISOCode(String CountryName)
		{
			String tmp = "US"; // default to US just in case
			IDataReader rs = DB.GetRS("select * from country  " + DB.GetNoLock() + " where lower(name)=" + DB.SQuote(CountryName.ToLower()));
			if(rs.Read())
			{
				tmp = DB.RSField(rs,"ThreeLetterISOCode");
			}
			rs.Close();
			return tmp;
		}

		static public decimal GetKitTotalPrice(int CustomerID, int ProductID, int ShoppingCartRecID)
		{
			decimal tmp = System.Decimal.Zero;
			IDataReader rs = DB.GetRS("SELECT Product.*, ProductVariant.Price, ProductVariant.SalePrice FROM Product  " + DB.GetNoLock() + " inner join productvariant  " + DB.GetNoLock() + " on product.productid=productvariant.productid where Product.ProductID=" + ProductID.ToString());
			if(rs.Read())
			{
				decimal BasePrice = System.Decimal.Zero;
				if(DB.RSFieldDecimal(rs,"SalePrice") != System.Decimal.Zero)
				{
					BasePrice = DB.RSFieldDecimal(rs,"SalePrice");
				}
				else
				{
					BasePrice = DB.RSFieldDecimal(rs,"Price");
				}
				decimal KitPriceDelta = Common.KitPriceDelta(CustomerID,ProductID,ShoppingCartRecID);
				tmp = BasePrice+KitPriceDelta;
			}
			rs.Close();
			return tmp;
		}

		static public bool KitContainsAllRequiredItems(int CustomerID, int ProductID, int ShoppingCartRecID)
		{
			bool AllRequiredFound = true;
			IDataReader rs = DB.GetRS("select * from kitgroup  " + DB.GetNoLock() + " where IsRequired=1 and kitgroup.ProductID=" + ProductID.ToString());
			while(rs.Read())
			{
				if(!Common.KitContainsAnyGroupItems(CustomerID,ProductID,ShoppingCartRecID,DB.RSFieldInt(rs,"KitGroupID")))
				{
					AllRequiredFound = false;
				}
			}
			rs.Close();
			return AllRequiredFound;
		}

		static public String GetJSPopupRoutines()
		{
			StringBuilder tmpS = new StringBuilder(2500);
			tmpS.Append("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			tmpS.Append("function popupwh(title,url,w,h)\n");
			tmpS.Append("	{\n");
			tmpS.Append("	window.open('popup.aspx?title=' + title + '&src=' + url,'Popup" + Common.GetRandomNumber(1,100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no,copyhistory=no,width=' + w + ',height=' + h + ',left=0,top=0');\n");
			tmpS.Append("	return (true);\n");
			tmpS.Append("	}\n");
			tmpS.Append("function popuptopicwh(title,topic,w,h,scrollbars)\n");
			tmpS.Append("	{\n");
			tmpS.Append("	window.open('popup.aspx?title=' + title + '&topic=' + topic,'Popup" + Common.GetRandomNumber(1,100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=' + scrollbars + ',resizable=no,copyhistory=no,width=' + w + ',height=' + h + ',left=0,top=0');\n");
			tmpS.Append("	return (true);\n");
			tmpS.Append("	}\n");
			tmpS.Append("function popuporderoptionwh(title,id,w,h,scrollbars)\n");
			tmpS.Append("	{\n");
			tmpS.Append("	window.open('popup.aspx?title=' + title + '&orderoptionid=' + id,'Popup" + Common.GetRandomNumber(1,100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=' + scrollbars + ',resizable=no,copyhistory=no,width=' + w + ',height=' + h + ',left=0,top=0');\n");
			tmpS.Append("	return (true);\n");
			tmpS.Append("	}\n");
			tmpS.Append("function popupkitgroupwh(title,kitgroupid,w,h,scrollbars)\n");
			tmpS.Append("	{\n");
			tmpS.Append("	window.open('popup.aspx?title=' + title + '&kitgroupid=' + kitgroupid,'Popup" + Common.GetRandomNumber(1,100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=' + scrollbars + ',resizable=no,copyhistory=no,width=' + w + ',height=' + h + ',left=0,top=0');\n");
			tmpS.Append("	return (true);\n");
			tmpS.Append("	}\n");
			tmpS.Append("function popupkititemwh(title,kititemid,w,h,scrollbars)\n");
			tmpS.Append("	{\n");
			tmpS.Append("	window.open('popup.aspx?title=' + title + '&kititemid=' + kititemid,'Popup" + Common.GetRandomNumber(1,100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=' + scrollbars + ',resizable=no,copyhistory=no,width=' + w + ',height=' + h + ',left=0,top=0');\n");
			tmpS.Append("	return (true);\n");
			tmpS.Append("	}\n");
			tmpS.Append("function popup(title,url)\n");
			tmpS.Append("	{\n");
			tmpS.Append("	popupwh(title,url,600,375);\n");
			tmpS.Append("	return (true);\n");
			tmpS.Append("	}\n");
			tmpS.Append("function popuptopic(title,topic,scrollbars)\n");
			tmpS.Append("	{\n");
			tmpS.Append("	popuptopicwh(title,topic,600,375,scrollbars);\n");
			tmpS.Append("	return (true);\n");
			tmpS.Append("	}\n");
			tmpS.Append("</script>\n");
			return tmpS.ToString();
		}
		
		public static bool IsAKit(int ProductID)
		{
			bool tmpS = false;
			IDataReader rs = DB.GetRS("select IsAKit from Product  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString());
			if(rs.Read())
			{
				tmpS = DB.RSFieldBool(rs,"IsAKit");
			}
			rs.Close();
			return tmpS;
		}

		public static bool IsAPack(int ProductID)
		{
			bool tmpS = false;
			IDataReader rs = DB.GetRS("select IsAPack from Product  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString());
			if(rs.Read())
			{
				tmpS = DB.RSFieldBool(rs,"IsAPack");
			}
			rs.Close();
			return tmpS;
		}

		public static int GetPackSize(int ProductID)
		{
			int tmp = 0;
			IDataReader rs = DB.GetRS("select PackSize from Product  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString());
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"PackSize");
			}
			rs.Close();
			return tmp;
		}

		public static String GetChart(String ReportTitle, String XTitle, String YTitle, String Height, String Width, bool ChartIs3D, String ChartTypeSpec, String Series1Name, String Series2Name, String DateSeries, String DS1Values, String DS2Values)
		{
			StringBuilder tmpS = new StringBuilder(10000);

			tmpS.Append("<p align=\"center\"><b><big>" + ReportTitle.Replace("|",", ") + "</big></b></p>\n");
			tmpS.Append("<APPLET CODE=\"javachart.applet." + ChartTypeSpec + "\" ARCHIVE=\"" + ChartTypeSpec + ".jar\" WIDTH=100% HEIGHT=500>\n");
			tmpS.Append("<param name=\"appletKey \" value=\"6080-632\">\n");
			tmpS.Append("<param name=\"CopyrightNotification\" value=\"KavaChart is a copyrighted work, and subject to full legal protection\">\n");
			tmpS.Append("<param name=\"delimiter\" value=\"|\">\n");
			tmpS.Append("<param name=\"labelsOn\" value=\"false\">\n");
			tmpS.Append("<param name=\"useValueLabels\" value=\"false\">\n");
			tmpS.Append("<param name=\"labelPrecision\" value=\"0\">\n");
			tmpS.Append("<param name=\"barClusterWidth\" value=\"0.58\">\n");
			tmpS.Append("<param name=\"dataset0LabelFont\" value=\"Serif|12|0\">\n");
			tmpS.Append("<param name=\"dataset0LabelColor\" value=\"000000\">\n");
			tmpS.Append("<param name=\"dataset1LabelFont\" value=\"Serif|12|0\">\n");
			tmpS.Append("<param name=\"dataset1LabelColor\" value=\"000000\">\n");
			tmpS.Append("<param name=\"backgroundColor\" value=\"ffffff\">\n");
			tmpS.Append("<param name=\"backgroundOutlining\" value=\"false\">\n");
			tmpS.Append("<param name=\"3D\" value=\"" + ChartIs3D.ToString().ToLower() + "\">\n");
			tmpS.Append("<param name=\"YDepth\" value=\"15\">\n");
			tmpS.Append("<param name=\"XDepth\" value=\"10\">\n");
			tmpS.Append("<param name=\"outlineLegend\" value=\"false\">\n");
			tmpS.Append("<param name=\"outlineColor\" value=\"000000\">\n");
			tmpS.Append("<param name=\"dataset0Name\" value=\"" + Series1Name + "\">\n");
			tmpS.Append("<param name=\"dataset0Labels\" value=\"false\">\n");
			tmpS.Append("<param name=\"dataset0Color\" value=\"" + Common.IIF(Series1Name == "Anons" , "00cccc" , "0066cc") + "\">\n");
			tmpS.Append("<param name=\"dataset0Outlining\" value=\"false\">\n");
			if(Series2Name.Length != 0)
			{
				tmpS.Append("<param name=\"dataset1Name\" value=\"" + Series2Name + "\">\n");
				tmpS.Append("<param name=\"dataset1Labels\" value=\"false\">\n");
				tmpS.Append("<param name=\"dataset1Color\" value=\"0066cc\">\n");
				tmpS.Append("<param name=\"dataset1Outlining\" value=\"false\">\n");
			}
			tmpS.Append("   <param name=\"backgroundGradient\" value=\"2\">\n");
			tmpS.Append("   <param name=\"backgroundTexture\" value=\"2\">\n");
			tmpS.Append("   <param name=\"plotAreaColor\" value=\"ffffcc\">\n");
			//tmpS.Append("   <param name=\"backgroundColor\" value=\"ffffee\">\n");
			tmpS.Append("   <param name=\"backgroundSecondaryColor\" value=\"ccccff\">\n");
			tmpS.Append("   <param name=\"backgroundGradient\" value=\"2\">\n");
			tmpS.Append("   <param name=\"yAxisTitle\" value=\"" + YTitle + "\">\n");
			tmpS.Append("<param name=\"yAxisLabelColor\" value=\"000000\">\n");
			tmpS.Append("<param name=\"yAxisLineColor\" value=\"000000\">\n");
			tmpS.Append("<param name=\"yAxisGridColor\" value=\"000000\">\n");
			tmpS.Append("<param name=\"yAxisGridWidth\" value=\"1\">\n");
			tmpS.Append("<param name=\"yAxisTickColor\" value=\"000000\">\n");
			tmpS.Append("<param name=\"yAxisOptions\" value=\"gridOn|leftAxis,\">\n");
			tmpS.Append("   <param name=\"xAxisTitle\" value=\"" + XTitle + "\">\n");
			tmpS.Append("<param name=\"xAxisLabelColor\" value=\"000000\">\n");
			tmpS.Append("<param name=\"xAxisLineColor\" value=\"000000\">\n");
			tmpS.Append("<param name=\"xAxisTickColor\" value=\"000000\">\n");
			tmpS.Append("<param name=\"xAxisOptions\" value=\"bottomAxis,\">\n");
			tmpS.Append("<param name=\"legendOn\" value=\"true\">\n");
			tmpS.Append("<param name=\"legendllX\" value=\".00\">\n");
			tmpS.Append("<param name=\"legendllY\" value=\".90\">\n");
			tmpS.Append("<param name=\"legendLabelFont\" value=\"Serif|12|0\">\n");
			tmpS.Append("<param name=\"legendLabelColor\" value=\"000000\">\n");
			tmpS.Append("<param name=\"legendColor\" value=\"ffffff\">\n");
			tmpS.Append("<param name=\"legendOutlining\" value=\"false\">\n");
			tmpS.Append("<param name=\"iconWidth\" value=\"0.03\">\n");
			tmpS.Append("<param name=\"iconHeight\" value=\"0.02\">\n");
			tmpS.Append("<param name=\"iconGap\" value=\"0.01\">\n");
			tmpS.Append("<param name=\"dwellUseDatasetName\" value=\"false\">\n");
			tmpS.Append("<param name=\"dwellUseYValue\" value=\"true\">\n");
			tmpS.Append("<param name=\"dwellYString\" value=\"Y: #\">\n");
			tmpS.Append("<param name=\"dwellUseXValue\" value=\"false\">\n");
			tmpS.Append("<param name=\"dwellUseLabelString\" value=\"false\">\n");
	
			// START DATA:
			tmpS.Append("<param name=\"xAxisLabelAngle\"  value=\"90\">\n");
			tmpS.Append("<param name=\"xAxisLabels\"  value=\"" + DateSeries + "\">\n");
			tmpS.Append("<param name=\"dataset0yValues\" value=\"" + DS1Values.Replace("$","").Replace(",","") + "\">\n");
			if(Series2Name.Length != 0)
			{
				tmpS.Append("<param name=\"dataset1yValues\" value=\"" + DS2Values.Replace("$","").Replace(",","") + "\">\n");
			}
			// END DATA
	
			tmpS.Append("</APPLET>\n");
			return tmpS.ToString();
		}
		
		public static String GetStoreHTTPLocation(bool TryToUseSSL)
		{
			String[] ScriptPathItems = Common.ServerVariables("SCRIPT_NAME").Split('/');
			String ScriptLocation = String.Empty;
			for(int i = 0; i < ScriptPathItems.GetUpperBound(0); i++)
			{
				if(ScriptPathItems[i].ToUpper() != Common.GetAdminDir().ToUpper())
				{
					ScriptLocation += ScriptPathItems[i] + "/";
				}
			}
			if(ScriptLocation.Length == 0)
			{
				ScriptLocation = "/";
			}
			if(!ScriptLocation.EndsWith("/"))
			{
				ScriptLocation = "/";
			}
			// ScriptLocation should now be everything after server name, including trailing "/", e.g. "/netstore/" or "/"
			String s = "http://" + Common.ServerVariables("HTTP_HOST") + ScriptLocation;
			if(TryToUseSSL && Common.AppConfigBool("UseSSL") && Common.OnLiveServer())
			{
				if(Common.AppConfig("SharedSSLLocation").Length == 0)
				{
					s = s.Replace("http:/","https:/");
					if(Common.AppConfigBool("RedirectLiveToWWW"))
					{
						if(s.IndexOf("https://www") == -1)
						{
							s = s.Replace("https://","https://www.");
						}
						s = s.Replace("www.www","www"); // safety check
					}
				}
				else
				{
					s = Common.AppConfig("SharedSSLLocation") + ScriptLocation;
				}
			}
			return s;
		}

		static public String GenerateHtmlEditor(String FieldID)
		{
			StringBuilder tmpS = new StringBuilder(5000);
			tmpS.Append("\n<script type=\"text/javascript\">\n");
			tmpS.Append("editor_generate('" + FieldID.ToString() + "');\n\n");
			//			tmpS.Append("var " + FieldID.ToString() + " = null;\n");
			//			tmpS.Append("function init" + FieldID.ToString() + "() {\n");
			//			tmpS.Append("  // create an editor for the \"" + FieldID.ToString() + "\" textbox\n");
			//			tmpS.Append("  " + FieldID.ToString() + " = new HTMLArea(\"" + FieldID.ToString() + "\");\n");
			//			tmpS.Append("  // register the Table plugin\n");
			//			tmpS.Append("  " + FieldID.ToString() + ".registerPlugin(TableOperations);\n");
			//			tmpS.Append("  // register the CSS plugin\n");
			//			tmpS.Append("  " + FieldID.ToString() + ".registerPlugin(CSS, {\n");
			//			tmpS.Append("    combos , [\n");
			//			tmpS.Append("      { label: \"Syntax:\",\n");
			//			tmpS.Append("                   // menu text       // CSS class\n");
			//			tmpS.Append("        options: { \"None\"           , \"\",\n");
			//			tmpS.Append("                   \"Code\" , \"code\",\n");
			//			tmpS.Append("                   \"String\" , \"string\",\n");
			//			tmpS.Append("                   \"Comment\" , \"comment\",\n");
			//			tmpS.Append("                   \"Variable name\" , \"variable-name\",\n");
			//			tmpS.Append("                   \"Type\" , \"type\",\n");
			//			tmpS.Append("                   \"Reference\" , \"reference\",\n");
			//			tmpS.Append("                   \"Preprocessor\" , \"preprocessor\",\n");
			//			tmpS.Append("                   \"Keyword\" , \"keyword\",\n");
			//			tmpS.Append("                   \"Function name\" , \"function-name\",\n");
			//			tmpS.Append("                   \"Html tag\" , \"html-tag\",\n");
			//			tmpS.Append("                   \"Html italic\" , \"html-helper-italic\",\n");
			//			tmpS.Append("                   \"Warning\" , \"warning\",\n");
			//			tmpS.Append("                   \"Html bold\" , \"html-helper-bold\"\n");
			//			tmpS.Append("                 },\n");
			//			tmpS.Append("        context: \"pre\"\n");
			//			tmpS.Append("      },\n");
			//			tmpS.Append("      { label: \"Info:\",\n");
			//			tmpS.Append("        options: { \"None\"           , \"\",\n");
			//			tmpS.Append("                   \"Quote\"          , \"quote\",\n");
			//			tmpS.Append("                   \"Highlight\"      , \"highlight\",\n");
			//			tmpS.Append("                   \"Deprecated\"     , \"deprecated\"\n");
			//			tmpS.Append("                 }\n");
			//			tmpS.Append("      }\n");
			//			tmpS.Append("    ]\n");
			//			tmpS.Append("  });\n");
			//			tmpS.Append("  // add a contextual menu\n");
			//			tmpS.Append("  " + FieldID.ToString() + ".registerPlugin(\"ContextMenu\");\n");
			//			tmpS.Append("  setTimeout(function() {\n");
			//			tmpS.Append("    " + FieldID.ToString() + ".generate();\n");
			//			tmpS.Append("  }, 500);\n");
			//			tmpS.Append("  return false;\n");
			//			tmpS.Append("}\n");
			//			tmpS.Append("init" + FieldID.ToString() + "();");
			tmpS.Append("</script>\n");
			return tmpS.ToString();
		}
		
		static public long GetImageSize(String imgname)
		{
			String imgfullpath = Common.IIF(imgname.IndexOf(":") == -1 && imgname.IndexOf("\\\\") == -1 , HttpContext.Current.Server.MapPath(imgname) , imgname);
			FileInfo fi =  new FileInfo(imgfullpath);
			long l = fi.Length;
			fi = null;
			return l;
		}
		
	
		static public bool ProductUsesAdvancedInventoryMgmt(int ProductID)
		{
			IDataReader rs = DB.GetRS("select UseAdvancedInventoryMgmt from Product  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString());
			bool tmp = false;
			if(rs.Read())
			{
				tmp = DB.RSFieldBool(rs,"UseAdvancedInventoryMgmt");
			}
			rs.Close();
			return tmp;
		}

		static public bool ProductTracksInventoryBySize(int ProductID)
		{
			IDataReader rs = DB.GetRS("select TrackInventoryBySize from Product  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString());
			bool tmp = false;
			if(rs.Read())
			{
				tmp = DB.RSFieldBool(rs,"TrackInventoryBySize");
			}
			rs.Close();
			return tmp;
		}

		static public bool ProductTracksInventoryByColor(int ProductID)
		{
			IDataReader rs = DB.GetRS("select TrackInventoryByColor from Product  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString());
			bool tmp = false;
			if(rs.Read())
			{
				tmp = DB.RSFieldBool(rs,"TrackInventoryByColor");
			}
			rs.Close();
			return tmp;
		}


		static public String GetFormInput(bool ExcludeVldtFields, String separator)
		{
			StringBuilder tmpS = new StringBuilder(10000);
			bool first = true;
			for(int i = 0; i<HttpContext.Current.Request.Form.Count; i++)
			{
				bool okField = true;
				if(ExcludeVldtFields)
				{
					if(HttpContext.Current.Request.Form.Keys[i].ToUpper().IndexOf("_VLDT") != -1)
					{
						okField = false;
					}
				}
				if(okField)
				{
					if(!first)
					{
						tmpS.Append(separator);
					}
					tmpS.Append("<b>" + HttpContext.Current.Request.Form.Keys[i] + "</b>=" + HttpContext.Current.Request.Form[HttpContext.Current.Request.Form.Keys[i]]);
					first = false;
				}
			}
			return tmpS.ToString();
		}

		// these are used for VB.NET compatibility
		static public int IIF(bool condition, int a, int b)
		{
			int x = 0;
			if(condition) 
			{
				x = a;
			}
			else
			{
				x = b;
			}
			return x;
		}

		static public Single IIF(bool condition, Single a, Single b)
		{
			float x = 0;
			if(condition) 
			{
				x = a;
			}
			else
			{
				x = b;
			}
			return x;
		}

		static public Double IIF(bool condition, double a, double b)
		{
			double x = 0;
			if(condition) 
			{
				x = a;
			}
			else
			{
				x = b;
			}
			return x;
		}

		static public decimal IIF(bool condition, decimal a, decimal b)
		{
			decimal x = 0;
			if(condition) 
			{
				x = a;
			}
			else
			{
				x = b;
			}
			return x;
		}

		static public String IIF(bool condition, String a, String b)
		{
			String x = String.Empty;
			if(condition) 
			{
				x = a;
			}
			else
			{
				x = b;
			}
			return x;
		}

		static public String GetManufacturerBrowseBox(int _siteID)
		{
			String CacheName = "GetManufacturerBrowseBox";
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<img src=\"skins/Skin_" + _siteID.ToString() +  "/images/manufacturers.gif\" border=\"0\"><br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			
			String sql = "select * from manufacturer  " + DB.GetNoLock() + " where deleted=0 order by DisplayOrder,Name";
			IDataReader rs = DB.GetRS(sql);
			bool anyFound = false;
			while(rs.Read())
			{
				tmpS.Append("<img height=\"8\" src=\"skins/skin_" + _siteID.ToString() + "/images/redarrow.gif\">&nbsp;<a href=\"" + SE.MakeManufacturerLink(DB.RSFieldInt(rs,"ManufacturerID"),DB.RSField(rs,"SEName")) + "\">" + DB.RSField(rs,"Name") + "</a><br>");
				anyFound = true;
			}
			rs.Close();
			
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return Common.IIF(anyFound , tmpS.ToString() , String.Empty);
		}

		
		static public String GetLeftCol(Customer thisCustomer, int SiteID)
		{
			StringBuilder tmpS = new StringBuilder(5000);
			tmpS.Append(Common.GetSearchBox(SiteID));
			if(DB.GetSqlN("select count(*) as N from [Section]  " + DB.GetNoLock() + " where published<>0 and (ParentSectionID=0 or ParentSectionID IS NULL) and deleted=0") > 0)
			{
				//tmpS.Append(Common.GetAdvancedSectionBrowseBox(thisCustomer,SiteID));
				tmpS.Append(Common.GetSectionBrowseBox(SiteID));
			}
			if(DB.GetSqlN("select count(*) as N from Category  " + DB.GetNoLock() + " where published<>0 and (ParentCategoryID=0 or ParentCategoryID IS NULL) and deleted=0") > 0)
			{
				//tmpS.Append(Common.GetAdvancedCategoryBrowseBox(thisCustomer,SiteID));
				tmpS.Append(Common.GetCategoryBrowseBox(SiteID));
			}
			if(Common.AppConfigBool("ShowMiniCart") && Common.GetThisPageName(false).ToLower().IndexOf("shoppingcart") == -1)
			{
				if(ShoppingCart.NumItems(thisCustomer._customerID,CartTypeEnum.ShoppingCart) > 0)
				{
					tmpS.Append(ShoppingCart.DisplayMiniCart(thisCustomer,SiteID));
				}
			}
			// don't show polls on the polls page :)
			if(Common.ServerVariables("SCRIPT_NAME").ToLower().IndexOf("polls.aspx") == -1)
			{
				String pollSQL = "select count(*) as N from Poll  " + DB.GetNoLock() + " where ExpiresOn>=" + DB.DateQuote(Localization.ToNativeShortDateString(System.DateTime.Now)) + " and published<>0 and deleted=0";
				if(DB.GetSqlN(pollSQL) > 0)
				{
					tmpS.Append(Common.GetPollBox(thisCustomer._customerID,SiteID, 0,false));
				}
			}

			tmpS.Append(Common.GetHelpBox(SiteID,true));
			return tmpS.ToString();
		}
	

		static public int GetInventory(int ProductID, int VariantID, String ChosenSize, String ChosenColor)
		{
			int tmp = 0;
			if(!Common.ProductUsesAdvancedInventoryMgmt(ProductID))
			{
				IDataReader rs = DB.GetRS("Select inventory from productvariant  " + DB.GetNoLock() + " where variantid=" + VariantID.ToString());
				if(rs.Read())
				{
					tmp = DB.RSFieldInt(rs,"Inventory");
				}
				rs.Close();
			}
			else
			{
				IDataReader rs = DB.GetRS("select * from Inventory  " + DB.GetNoLock() + " where VariantID=" + VariantID.ToString() + " and lower([size])=" + DB.SQuote(CleanSizeColorOption(ChosenSize).ToLower()) + " and lower(color)=" + DB.SQuote(CleanSizeColorOption(ChosenColor).ToLower()));
				if(rs.Read())
				{
					tmp = DB.RSFieldInt(rs,"Quan");
				}
				rs.Close();
			}
			return tmp;
		}
		
		static public void SetInventory(int VariantID,String Size,String Color,int Quan,bool assumeSafeInsert)
		{
			//if(Quan < 0)
			//{
			//	Quan = 0;
			//}
			if(!assumeSafeInsert)
			{
				DB.ExecuteSQL("delete from Inventory  where VariantID=" + VariantID.ToString() + " and lower([size])=" + DB.SQuote(CleanSizeColorOption(Size).ToLower()) + " and lower(color)=" + DB.SQuote(CleanSizeColorOption(Color).ToLower()));
			}
			DB.ExecuteSQL("insert into Inventory(InventoryGUID,VariantID,[Size],Color,Quan) values(" + DB.SQuote(DB.GetNewGUID()) + "," + VariantID.ToString() + "," + DB.SQuote(CleanSizeColorOption(Size)) + "," + DB.SQuote(CleanSizeColorOption(Color)) + "," + Quan.ToString() + ")");
		}

		static public void AdjustInventory(int VariantID,String Size,String Color,int QuanSold)
		{
			int ProductID = Common.GetVariantProductID(VariantID);
			if(!Common.ProductTracksInventoryByColor(ProductID))
			{
				Color = String.Empty;
			}
			if(!Common.ProductTracksInventoryBySize(ProductID))
			{
				Size = String.Empty;
			}
			SetInventory(VariantID,Size,Color,Common.Max(GetInventory(ProductID,VariantID,CleanSizeColorOption(Size),CleanSizeColorOption(Color))-QuanSold,0),false);
		}

		public static int Max(int a, int b)
		{
			if(a > b)
			{
				return a;
			}
			return b;
		}

		public static int Min(int a, int b)
		{
			if(a < b)
			{
				return a;
			}
			return b;
		}
		
		static public String GetCategoryPromptSingular()
		{
			String tmpS = Common.AppConfig("CategoryPromptSingular");
			if(tmpS.Length == 0)
			{
				tmpS = "Category";
			}
			return tmpS;
		}

		static public String GetCategoryPromptPlural()
		{
			String tmpS = Common.AppConfig("CategoryPromptPlural");
			if(tmpS.Length == 0)
			{
				tmpS = "Category";
			}
			return tmpS;
		}

		static public bool CustomerLevelAllowsPO(int CustomerLevelID)
		{
			if(CustomerLevelID == 0)
			{
				// consumers cannot use PO's, unless overridden by other parameters:
				return Common.AppConfigBool("CustomerLevel0AllowsPOs");
			}
			bool tmpS = false;
			IDataReader rs = DB.GetRS("Select LevelAllowsPO from CustomerLevel  " + DB.GetNoLock() + " where CustomerLevelID=" + CustomerLevelID.ToString());
			if(rs.Read())
			{
				tmpS = DB.RSFieldBool(rs,"LevelAllowsPO");
			}
			rs.Close();
			return tmpS;
		}
		
		static public String GetActiveReceiptTemplate(int SiteID)
		{
			int RTN = Common.AppConfigUSInt("ReceiptTemplateNumber");
			if(RTN == 0)
			{
				return String.Empty;
			}
			else
			{
				return new ReceiptTemplate(RTN,SiteID).Contents; 
			}
		}

		static public String GetActiveShippingTemplate(int SiteID)
		{
			int RTN = Common.AppConfigUSInt("ShippingTemplateNumber");
			if(RTN == 0)
			{
				return String.Empty;
			}
			else
			{
				return new ShippingTemplate(RTN,SiteID).Contents;
			}
		}

		static public void CheckDemoExpiration()
		{

			System.Net.IPHostEntry T = System.Net.Dns.GetHostByName("www.aspdotnetstorefront.com");
			for(int A = 0; A <= T.AddressList.Length -1; A++)
			{
				if(T.AddressList[A].ToString().StartsWith("192.168") || T.AddressList[A].ToString().StartsWith("127.0"))
				{
					throw new Exception("Cracked version not allowed.");
				}
			}

			String DemoKey = Common.Application("DemoKey");
			if(DemoKey.Length == 0)
			{
				throw(new ArgumentException("You must enter your DemoKey in your web.config and /admin/web.config files. Instructions should have been provided in your demo sign up e-mail. Please contact us if you did not receive one. http://www.aspdotnetstorefront.com. Thanks"));
			}
			String stat = Common.AspHTTP("http://www.aspdotnetstorefront.com/getdemoexpiry.aspx?demolocation=" + DemoKey + "&fromappconfig=true");
			if(stat.Length == 0)
			{
				throw(new ArgumentException("The License Server is not available. Please try again in a few minutes. We apologize for any inconvenience."));
			}
			if(stat.IndexOf("Demo License OK") == -1)
			{
				throw(new ArgumentException("Your AspDotNetStorefront trial has expired. Please register with http://www.aspdotnetstorefront.com. Thanks"));
			}
		}

		static public bool OrderHasDownloadComponents(int OrderNumber)
		{
			//V4_0 Check the Orders_CustomCart for downloadable items.
			int nCount = DB.GetSqlN("select count(*) as N from orders_customcart " + DB.GetNoLock() + " inner join ProductVariant " + DB.GetNoLock() + " on orders_customcart.VariantID=ProductVariant.VariantID where ProductVariant.IsDownload=1 and orders_customcart.OrderNumber=" + OrderNumber.ToString());
			if (nCount != 0)
			{
				return true;
			}
			//V4_0 End
			return DB.GetSqlN("select count(*) as N from orders_ShoppingCart where IsDownload=1 and OrderNumber=" + OrderNumber.ToString()) != 0;
		}

		static public bool OrderHasShippableComponents(int OrderNumber)
		{
			return DB.GetSqlN("select count(*) as N from orders_ShoppingCart  " + DB.GetNoLock() + " where IsDownload=0 and OrderNumber=" + OrderNumber.ToString()) != 0;
		}

		static public String GetRelatedProductsBoxExpanded(int forProductID, int showNum, bool showPics, String teaser, bool gridFormat, int SiteID)
		{
			String CacheName = "GetRelatedProductsBoxExpanded" + "_" + forProductID.ToString() + "_" + showNum.ToString() + "_" + showPics.ToString() + "_" + gridFormat.ToString();
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String CacheData = (String)HttpContext.Current.Cache.Get(CacheName);
				if(CacheData != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return CacheData;
				}
			}

			String RelatedProductList = String.Empty;
			IDataReader rs = DB.GetRS("Select RelatedProducts from product  " + DB.GetNoLock() + " where ProductID=" + forProductID.ToString());
			if(rs.Read())
			{
				RelatedProductList = DB.RSField(rs,"RelatedProducts");
			}
			rs.Close();
			StringBuilder tmpS = new StringBuilder(10000);
			DataSet ds = DB.GetDS("select p.* from product p  " + DB.GetNoLock() + " where p.deleted=0 and p.published=1 and p.productid in (" + RelatedProductList + ") order by name",false,System.DateTime.Now.AddDays(1));
			
			if(ds.Tables[0].Rows.Count > 0)
			{
				tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
				tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
				tmpS.Append("<img src=\"skins/Skin_" + SiteID.ToString() +  "/images/relatedproducts.gif\" border=\"0\"><br>");
				tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
				tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			
				if(teaser.Length != 0)
				{
					tmpS.Append("<p><b>" + teaser + "</b></p>\n");
				}

				try
				{
					bool empty = (ds.Tables[0].Rows.Count > 0);
					if(gridFormat)
					{
						// GRID FORMAT:
						int ItemNumber = 1;
						int ItemsPerRow = Common.AppConfigUSInt("DefaultCategoryColWidth");
						tmpS.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"4\" width=\"100%\">");
						foreach(DataRow row in ds.Tables[0].Rows)
						{
							if(ItemNumber == 1)
							{
								tmpS.Append("<tr>");
							}
							if(ItemNumber == ItemsPerRow+1)
							{
								tmpS.Append("</tr><tr><td colspan=\"" + ItemsPerRow.ToString() + "\" height=\"8\"></td></tr>");
								ItemNumber=1;
							}
							tmpS.Append("<td width=\"" + (100/ItemsPerRow).ToString() + "%\" height=\"150\" align=\"center\" valign=\"bottom\">");
							tmpS.Append("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName")) + "\">");
							if(showPics)
							{
								String ImgUrl = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",SiteID);
								if(ImgUrl.Length == 0)
								{
									ImgUrl = Common.AppConfig("NoPicture");
								}
								if(ImgUrl.Length != 0)
								{
									Single w = (Single)Common.GetImageWidth(ImgUrl);
									Single h = (Single)Common.GetImageHeight(ImgUrl);
									Single finalW = w;
									Single finalH = h;
									if(w > 150)
									{
										finalH = h * 150/w;
										finalW = 150;
									}
									if(finalH > 150)
									{
										finalW = finalW * 150/finalH;
										finalH = 150;
									}
									tmpS.Append("<img src=\"" + ImgUrl + "\" width=\"" + ((int)finalW).ToString() + " height=\"" + ((int)finalH).ToString() + "\" border=\"0\">");
									tmpS.Append("<br><br>");
								}
							}
							tmpS.Append(DB.RowField(row,"Name") + "</a>");
							tmpS.Append("</td>");
							ItemNumber++;
						}
						for(int i = ItemNumber; i<=ItemsPerRow; i++)
						{
							tmpS.Append("<td>&nbsp;</td>");
						}
						tmpS.Append("</tr>");
						tmpS.Append("</table>");
					}
					else
					{
						tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\">\n");
						int i = 1;
						foreach(DataRow row in ds.Tables[0].Rows)
						{
							if(i > showNum)
							{
								tmpS.Append("<tr><td " + Common.IIF(showPics , "colspan=\"2\"" , "") + "><hr size=1 class=\"LightCellText\"></td></tr>");
								break;
							}
							if(i > 1)
							{
								tmpS.Append("<tr><td " + Common.IIF(showPics , "colspan=\"2\"" , "") + "><hr size=1 class=\"LightCellText\"></td></tr>");
							}
							tmpS.Append("<tr>");
							String ImgUrl = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",SiteID);
							if(ImgUrl.Length == 0)
							{
								ImgUrl = Common.AppConfig("NoPicture");
							}
							if(showPics)
							{
								tmpS.Append("<td align=\"left\" valign=\"top\">\n");
								int w = Common.GetImageWidth(ImgUrl);
								int h = Common.GetImageHeight(ImgUrl);
								tmpS.Append("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName")) + "\">");
								if(w > 100)
								{
									w = 100;
								}
								//if(h > 100)
								//{
								//	h = 100;
								//}
								tmpS.Append("<img align=\"left\" src=\"" + ImgUrl + "\" width=\"" + w.ToString() + "\" border=\"0\">");
								tmpS.Append("</a>");
								tmpS.Append("</td>");
							}

							tmpS.Append("<td align=\"left\" valign=\"top\">\n");
							tmpS.Append("<b class=\"a4\">");
							tmpS.Append("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName")) + "\">" + DB.RowField(row,"Name"));
							if(DB.RowField(row,"Summary").Length != 0)
							{
								tmpS.Append(": " + DB.RowField(row,"Summary"));
							}
							tmpS.Append("</a></b><br>\n");
							if(DB.RowField(row,"Description").Length != 0)
							{
								tmpS.Append("<span class=\"a2\">" + DB.RowField(row,"Description") + "</span><br>\n");
							}
							tmpS.Append("<div class=\"a1\" style=\"PADDING-BOTTOM: 10px; PADDING-TOP: 10px;\">\n");
							tmpS.Append("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName")) + "\">");
							tmpS.Append("Click here for more information...");
							tmpS.Append("</a>");
							tmpS.Append("</div>\n");
							tmpS.Append("</td>");
							tmpS.Append("</tr>");
							i++;
						}
						tmpS.Append("</table>\n");
					}
				}
				catch
				{
					// people put all kinds of crap in relatedproducts field, so have to trap those errors, or the site fails.
				}
			
				tmpS.Append("</td></tr>\n");
				tmpS.Append("</table>\n");
				tmpS.Append("</td></tr>\n");
				tmpS.Append("</table>\n");
			}
			ds.Dispose();
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public Decimal GetUpsellProductPrice(int SourceProductID, int UpsellProductID, int CustomerLevelID)
		{
			Single UpsellProductDiscountPercentage = 0.0F;
			IDataReader rs = DB.GetRS("Select UpsellProducts,UpsellProductDiscountPercentage from product  " + DB.GetNoLock() + " where ProductID=" + SourceProductID.ToString());
			if(rs.Read())
			{
				UpsellProductDiscountPercentage = DB.RSFieldSingle(rs,"UpsellProductDiscountPercentage");
			}
			rs.Close();
			bool IsOnSale = true; // don't care really
			Decimal PR = Common.DetermineLevelPrice(Common.GetProductsFirstVariantID(UpsellProductID),CustomerLevelID,out IsOnSale);
			if(UpsellProductDiscountPercentage != 0.0F)
			{
				PR = PR * (Decimal)(1-(UpsellProductDiscountPercentage/100.0F));
			}
			return PR;
		}

		static public String GetUpsellProductsBoxExpanded(int forProductID, int showNum, bool showPics, String teaser, bool gridFormat, int SiteID, int CustomerLevelID)
		{
			String CacheName = "GetUpsellProductsBoxExpanded" + "_" + forProductID.ToString() + "_" + showNum.ToString() + "_" + showPics.ToString() + "_" + gridFormat.ToString();
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String CacheData = (String)HttpContext.Current.Cache.Get(CacheName);
				if(CacheData != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br />\n");
					}
					return CacheData;
				}
			}

			String UpsellProductList = String.Empty;
			Single UpsellProductDiscountPercentage = 0.0F;
			IDataReader rs = DB.GetRS("Select UpsellProducts,UpsellProductDiscountPercentage from product  " + DB.GetNoLock() + " where ProductID=" + forProductID.ToString());
			if(rs.Read())
			{
				UpsellProductList = DB.RSField(rs,"UpsellProducts");
				UpsellProductDiscountPercentage = DB.RSFieldSingle(rs,"UpsellProductDiscountPercentage");
			}
			rs.Close();
			StringBuilder tmpS = new StringBuilder(10000);
			DataSet ds = DB.GetDS("select p.*, pv.price, pv.saleprice from product p  " + DB.GetNoLock() + " left outer join productvariant pv " + DB.GetNoLock() + " on p.productid=pv.productid where p.deleted=0 and p.published=1 and pv.deleted=0 and pv.published=1 and p.productid in (" + UpsellProductList + ")",false,System.DateTime.Now.AddDays(1));
			
			if(ds.Tables[0].Rows.Count > 0)
			{
				tmpS.Append("\n");
				tmpS.Append("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				tmpS.Append("function UpsellClick(theItem)\n");
				tmpS.Append("	{\n");
				tmpS.Append("	var UpsellItemList = '';\n");
				tmpS.Append("	var whichitem = 0;\n");
				tmpS.Append("	var theForm = document.forms['UpsellForm'];\n");
				//tmpS.Append("alert(theForm.name);\n");
				//tmpS.Append("alert(theForm.Upsell.name);\n");
				//tmpS.Append("alert(theForm.Upsell.length);\n");
				//tmpS.Append("alert(theForm.Upsell.checked);\n");
				tmpS.Append("	while (whichitem < theForm.Upsell.length)\n");
				tmpS.Append("	{\n");
				tmpS.Append("		if (theForm.Upsell[whichitem].checked && theForm.Upsell[whichitem].value != '0')\n");
				tmpS.Append("		{\n");
				tmpS.Append("			if (UpsellItemList.length > 0)\n");
				tmpS.Append("			{\n");
				tmpS.Append("				UpsellItemList = UpsellItemList + ',';\n");
				tmpS.Append("			}\n");
				tmpS.Append("			UpsellItemList = UpsellItemList + theForm.Upsell[whichitem].value;\n");
				tmpS.Append("		}\n");
				tmpS.Append("		whichitem++;\n");
				tmpS.Append("	}\n");
				//tmpS.Append("	alert(UpsellItemList);\n");
				tmpS.Append("	if (UpsellItemList.length > 0)\n"); // set all upsell hidden fields on all addtocart forms, so they are picked up on a submit:
				tmpS.Append("	{\n");
				tmpS.Append("		var whichform = 0;\n");
				tmpS.Append("		while (whichform < document.forms.length)\n");
				tmpS.Append("		{\n");
				tmpS.Append("			if(document.forms[whichform].UpsellProducts != null)\n");
				tmpS.Append("			{\n");
				tmpS.Append("				document.forms[whichform].UpsellProducts.value = UpsellItemList;\n");
				tmpS.Append("			}\n");
				tmpS.Append("			whichform++;\n");
				tmpS.Append("		}\n");
				tmpS.Append("	}\n");
				tmpS.Append("	}\n");
				tmpS.Append("</script>\n");

				tmpS.Append("<form id=\"UpsellForm\" name=\"UpsellForm\" style=\"margin-top: 0px; margin-bottom: 0px;\">");
				
				Topic UpsellTeaser = new Topic("UpsellTeaser");
				tmpS.Append(UpsellTeaser._contents);
				
				tmpS.Append("<input style=\"visibility: hidden;\" type=\"checkbox\" id=\"Upsell\" name=\"Upsell\" value=\"0\" onClick=\"UpsellClick(this);\">"); // must have at least 2 checkboxes for stupid javascript to work!
				tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
				tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
				tmpS.Append("<img src=\"skins/Skin_" + SiteID.ToString() +  "/images/Upsellproducts.gif\" border=\"0\" /><br />");
				tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
				tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			
				if(teaser.Length != 0)
				{
					tmpS.Append("<p><b>" + teaser + "</b></p>\n");
				}

				try
				{
					bool empty = (ds.Tables[0].Rows.Count > 0);
					if(gridFormat)
					{
						// GRID FORMAT:
						int ItemNumber = 1;
						int ItemsPerRow = Common.AppConfigUSInt("DefaultCategoryColWidth");
						tmpS.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"4\" width=\"100%\">");
						foreach(DataRow row in ds.Tables[0].Rows)
						{
							if(ItemNumber == 1)
							{
								tmpS.Append("<tr>");
							}
							if(ItemNumber == ItemsPerRow+1)
							{
								tmpS.Append("</tr><tr><td colspan=\"" + ItemsPerRow.ToString() + "\" height=\"8\"></td></tr>");
								ItemNumber=1;
							}
							tmpS.Append("<td width=\"" + (100/ItemsPerRow).ToString() + "%\" height=\"150\" align=\"center\" valign=\"bottom\">");
							//tmpS.Append("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName")) + "\">");
							if(showPics)
							{
								String ImgUrl = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",SiteID);
								if(ImgUrl.Length != 0)
								{
									Single w = (Single)Common.GetImageWidth(ImgUrl);
									Single h = (Single)Common.GetImageHeight(ImgUrl);
									Single finalW = w;
									Single finalH = h;
									if(w > 150)
									{
										finalH = h * 150/w;
										finalW = 150;
									}
									if(finalH > 150)
									{
										finalW = finalW * 150/finalH;
										finalH = 150;
									}
									tmpS.Append("<img src=\"" + ImgUrl + "\" width=\"" + ((int)finalW).ToString() + " height=\"" + ((int)finalH).ToString() + "\" border=\"0\" />");
									tmpS.Append("<br /><br />");
								}
							}
							tmpS.Append(DB.RowField(row,"Name"));
							//tmpS.Append("</a>");
							tmpS.Append("<br>");
							Decimal PR = Common.GetUpsellProductPrice(forProductID,DB.RowFieldInt(row,"ProductID"),CustomerLevelID);
							tmpS.Append(Localization.CurrencyStringForDisplay(PR));
							tmpS.Append("<br>");
							tmpS.Append("<input type=\"checkbox\" id=\"Upsell\" name=\"Upsell\" value=\"" + DB.RowFieldInt(row,"ProductID") + "\" onClick=\"UpsellClick(this);\">");
							tmpS.Append("</td>");
							ItemNumber++;
						}
						for(int i = ItemNumber; i<=ItemsPerRow; i++)
						{
							tmpS.Append("<td>&nbsp;</td>");
						}
						tmpS.Append("</tr>");
						tmpS.Append("</table>");
					}
					else
					{
						tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\">\n");
						int i = 1;
						foreach(DataRow row in ds.Tables[0].Rows)
						{
							if(i > showNum)
							{
								tmpS.Append("<tr><td " + Common.IIF(showPics , "colspan=\"2\"" , "") + "><hr size=1 class=\"LightCellText\"></td></tr>");
								break;
							}
							if(i > 1)
							{
								tmpS.Append("<tr><td " + Common.IIF(showPics , "colspan=\"2\"" , "") + "><hr size=1 class=\"LightCellText\"></td></tr>");
							}
							tmpS.Append("<tr>");
							String ImgUrl = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",SiteID);
							if(showPics)
							{
								tmpS.Append("<td align=\"left\" valign=\"top\">\n");
								int w = Common.GetImageWidth(ImgUrl);
								int h = Common.GetImageHeight(ImgUrl);
								//tmpS.Append("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName")) + "\">");
								if(w > 100)
								{
									w = 100;
								}
								//if(h > 100)
								//{
								//	h = 100;
								//}
								tmpS.Append("<img align=\"left\" src=\"" + ImgUrl + "\" width=\"" + w.ToString() + "\" border=\"0\" />");
								//tmpS.Append("</a>");
								tmpS.Append("</td>");
							}

							tmpS.Append("<td align=\"left\" valign=\"top\">\n");
							tmpS.Append("<b class=\"a4\">");
							//tmpS.Append("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName")) + "\">");
							tmpS.Append(DB.RowField(row,"Name"));
							if(DB.RowField(row,"Summary").Length != 0)
							{
								tmpS.Append(": " + DB.RowField(row,"Summary"));
							}
							//tmpS.Append("</a>");
							tmpS.Append("</b><br />\n");
							if(DB.RowField(row,"Description").Length != 0)
							{
								tmpS.Append("<span class=\"a2\">" + DB.RowField(row,"Description") + "</span><br />\n");
							}

							Decimal PR = Common.GetUpsellProductPrice(forProductID,DB.RowFieldInt(row,"ProductID"),CustomerLevelID);
							tmpS.Append(Localization.CurrencyStringForDisplay(PR));
							tmpS.Append("<br>");
							tmpS.Append("<input type=\"checkbox\" id=\"Upsell\" name=\"Upsell\" value=\"" + DB.RowFieldInt(row,"ProductID") + "\" onClick=\"UpsellClick(this);\">");


							//tmpS.Append("<div class=\"a1\" style=\"PADDING-BOTTOM: 10px; PADDING-TOP: 10px;\">\n");
							//tmpS.Append("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName")) + "\">");
							//tmpS.Append(GetString("common.cs.33",SiteID,Thread.CurrentThread.CurrentUICulture.Name));
							//tmpS.Append("</a>");
							//tmpS.Append("</div>\n");
							tmpS.Append("</td>");
							tmpS.Append("</tr>");
							i++;
						}
						tmpS.Append("</table>\n");
					}
				}
				catch
				{
					// people put all kinds of crap in Upsellproducts field, so have to trap those errors, or the site fails.
				}
			
				tmpS.Append("</td></tr>\n");
				tmpS.Append("</table>\n");
				tmpS.Append("</td></tr>\n");
				tmpS.Append("</table>\n");
				tmpS.Append("</form>\n");
			}
			ds.Dispose();
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		public static String PageInvocation()
		{
			return HttpContext.Current.Request.RawUrl;
		}

		public static String PageReferrer()
		{
			try
			{
				return HttpContext.Current.Request.UrlReferrer.ToString();
			}
			catch
			{}
			return String.Empty;
		}

		public static bool ReferrerOKForSubmit()
		{
			if(Common.AppConfigBool("AllowCrossDomainSubmit"))
			{
				return true; // store manager has supressed this check
			}
			// referrer domain (location) must match this page domain:
			bool OnSSL = (Common.ServerVariables("SERVER_PORT_SECURE") == "1");
			String refDomain = Common.PageReferrer().ToLower();
			String thisDomain = "http" + Common.IIF(OnSSL , "s" , "") + "://" + Common.ServerVariables("HTTP_HOST").ToLower();

			int i = refDomain.IndexOf("?");
			if(i != -1)
			{
				refDomain = refDomain.Substring(0,i);
			}

			if(refDomain.Length == 0)
			{
				return false; // this should never happen, if our pages are called by our pages!
			}

			if(refDomain.IndexOf("file://") != -1)
			{
				//not gonna happen:
				return false;
			}

			if((OnSSL && refDomain.IndexOf("https:") == -1) || (!OnSSL && refDomain.IndexOf("https:") != -1))
			{
				// very unlikely, the form should have been secure also!
				// i.e. both form and form handler should be secure, or not secure, but not mixed
				return false;
			}

			try
			{
				return (refDomain.Substring(0,thisDomain.Length) == thisDomain);
			}
			catch {}
			// something went wrong, so fail it:
			return false;
		}

		static public String MakeSearchTags(String header,String PageName, int ManufacturerID, int CategoryID, int SectionID, int ProductID, int VariantID, int SiteID)
		{
			// make meta tags/title for page, starting at lowest level first, as priority
			String SE_Title = String.Empty;
			String SE_MetaDescription = String.Empty;
			String SE_MetaKeywords = String.Empty;
			// check variant for info:
			IDataReader rs;
			if(ManufacturerID != 0)
			{
				rs = DB.GetRS("select * from manufacturer  " + DB.GetNoLock() + " where ManufacturerID=" + VariantID.ToString());
				if(rs.Read())
				{
					SE_Title = DB.RSField(rs,"Name") + " - " + Common.AppConfig("StoreName");
					SE_MetaDescription = DB.RSField(rs,"SEDescription");
					SE_MetaKeywords = DB.RSField(rs,"SEKeywords");
					if(SE_MetaKeywords.Length == 0)
					{
						SE_MetaKeywords = SE_MetaDescription;
					}
				}
				rs.Close();
			}
			if(VariantID != 0)
			{
				rs = DB.GetRS("select * from productvariant  " + DB.GetNoLock() + " where variantid=" + VariantID.ToString());
				if(rs.Read())
				{
					SE_Title = DB.RSField(rs,"Name") + " - " + Common.AppConfig("StoreName");
					SE_MetaDescription = DB.RSField(rs,"SEDescription");
					SE_MetaKeywords = DB.RSField(rs,"SEKeywords");
					if(SE_MetaKeywords.Length == 0)
					{
						SE_MetaKeywords = SE_MetaDescription;
					}
				}
				rs.Close();
				ProductID = Common.GetVariantProductID(VariantID); // make sure it matches the variant passed in
			}
			// Check Product Level if necessary
			if((SE_Title.Length == 0 || SE_MetaDescription.Length == 0 || SE_MetaKeywords.Length == 0) && ProductID != 0)
			{
				rs = DB.GetRS("select * from product  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString());
				if(rs.Read())
				{
					if(SE_Title.Length == 0)
					{
						SE_Title = DB.RSField(rs,"SETitle");
						if(SE_Title.Length == 0)
						{
							SE_Title = Common.AppConfig("StoreName") + " - " + DB.RSField(rs,"Name");
						}
					}
					if(SE_MetaDescription.Length == 0)
					{
						SE_MetaDescription = DB.RSField(rs,"SEDescription");
					}
					if(SE_MetaKeywords.Length == 0)
					{
						SE_MetaKeywords = DB.RSField(rs,"SEKeywords");
						if(SE_MetaKeywords.Length == 0)
						{
							SE_MetaKeywords = SE_MetaDescription;
						}
						if(SE_MetaKeywords.Length == 0)
						{
							SE_MetaKeywords = SE_MetaDescription;
						}
					}
				}
				rs.Close();
			}
			// check section
			if((SE_Title.Length == 0 || SE_MetaDescription.Length == 0 || SE_MetaKeywords.Length == 0) && SectionID != 0)
			{
				rs = DB.GetRS("select * from [section]  " + DB.GetNoLock() + " where SectionID=" + SectionID.ToString());
				if(rs.Read())
				{
					if(SE_Title.Length == 0)
					{
						SE_Title = DB.RSField(rs,"SETitle");
						if(SE_Title.Length == 0)
						{
							SE_Title = Common.AppConfig("StoreName") + " - " + DB.RSField(rs,"Name");
						}
					}
					if(SE_MetaDescription.Length == 0)
					{
						SE_MetaDescription = DB.RSField(rs,"SEDescription");
					}
					if(SE_MetaKeywords.Length == 0)
					{
						SE_MetaKeywords = DB.RSField(rs,"SEKeywords");
					}
					if(SE_MetaKeywords.Length == 0)
					{
						SE_MetaKeywords = SE_MetaDescription;
					}
				}
				rs.Close();
			}
			// check category
			if((SE_Title.Length == 0 || SE_MetaDescription.Length == 0 || SE_MetaKeywords.Length == 0) && CategoryID != 0)
			{
				rs = DB.GetRS("select * from category  " + DB.GetNoLock() + " where CategoryID=" + CategoryID.ToString());
				if(rs.Read())
				{
					if(SE_Title.Length == 0)
					{
						SE_Title = DB.RSField(rs,"SETitle");
						if(SE_Title.Length == 0)
						{
							SE_Title = Common.AppConfig("StoreName") + " - " + DB.RSField(rs,"Name");
						}
					}
					if(SE_MetaDescription.Length == 0)
					{
						SE_MetaDescription = DB.RSField(rs,"SEDescription");
					}
					if(SE_MetaKeywords.Length == 0)
					{
						SE_MetaKeywords = DB.RSField(rs,"SEKeywords");
					}
					if(SE_MetaKeywords.Length == 0)
					{
						SE_MetaKeywords = SE_MetaDescription;
					}
				}
				rs.Close();
			}

			// now use PageName anything missing:
			if(SE_Title.Length == 0)
			{
				if(PageName.Length != 0)
				{
					SE_Title = Common.AppConfig("StoreName") + " - " + PageName;
				}
			}
			if(SE_MetaDescription.Length == 0)
			{
				SE_MetaDescription = Common.AppConfig("SE_MetaDescription");
			}
			if(SE_MetaKeywords.Length == 0)
			{
				SE_MetaKeywords = Common.AppConfig("SE_MetaKeywords");
			}

			// now use default values anything missing:
			if(SE_Title.Length == 0)
			{
				SE_Title = Common.AppConfig("SE_MetaTitle");
				if(SE_Title.Length == 0)
				{
					SE_Title = Common.AppConfig("StoreName");
				}
			}
			if(SE_MetaDescription.Length == 0)
			{
				SE_MetaDescription = Common.AppConfig("SE_MetaDescription");
			}
			if(SE_MetaKeywords.Length == 0)
			{
				SE_MetaKeywords = Common.AppConfig("SE_MetaKeywords");
			}

			return header.Replace("%METATITLE%",SE_Title).Replace("%METADESCRIPTION%",SE_MetaDescription).Replace("%METAKEYWORDS%",SE_MetaKeywords);
		}

		static public String GetThisPageName(bool includePath)
		{
			String s = Common.ServerVariables("SCRIPT_NAME");
			if(!includePath)
			{
				int ix = s.LastIndexOf("/");
				if(ix != -1)
				{
					s = s.Substring(ix+1);
				}
			}
			return s;
		}

		static public String GetNewsBoxExpanded(int showNum, bool useCache, String teaser, int SiteID)
		{
			String CacheName = "GetNewsBoxExpanded" + showNum.ToString();
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn && useCache)
			{
				String cachedData = (String)HttpContext.Current.Cache.Get(CacheName);
				if(cachedData != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return cachedData;
				}
			}

			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<a href=\"news.aspx\"><img src=\"skins/Skin_" + SiteID.ToString() +  "/images/newsexpanded.gif\" border=\"0\"></a><br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			
			tmpS.Append("<p><b>" + teaser + "</b></p>\n");

			DataSet ds = DB.GetDS("select * from news  " + DB.GetNoLock() + " where expireson>" + DB.DateQuote(Localization.ToNativeShortDateString(System.DateTime.Now)) + " and year(createdon) in (" + System.DateTime.Now.Year.ToString() + "," + (System.DateTime.Now.Year-1).ToString() + ") and deleted=0 and published=1 order by createdon desc",false,System.DateTime.Now.AddDays(1));

			tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\">\n");
			int i = 1;
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				if(i > showNum)
				{
					tmpS.Append("<tr><td colspan=\"2\"><hr size=1 color=\"#" + Common.AppConfig("MediumCellColor") + "\"><a href=\"news.aspx\">more...</a></td></tr>");
					break;
				}
				if(i > 1)
				{
					tmpS.Append("<tr><td colspan=\"2\"><hr size=1 color=\"#" + Common.AppConfig("MediumCellColor") + "\"></td></tr>");
				}
				tmpS.Append("<tr>");
				tmpS.Append("<td width=\"15%\" align=\"left\" valign=\"top\">\n");
				tmpS.Append("<b>" + Localization.ToNativeShortDateString(DB.RowFieldDateTime(row,"CreatedOn")) + "</b>");
				tmpS.Append("</td>");
				tmpS.Append("<td align=\"left\" valign=\"top\">\n");
				String newsItem = DB.RowField(row,"NewsCopy");
				newsItem = newsItem.Replace("%SKINID%",SiteID.ToString());
				tmpS.Append(newsItem);
				tmpS.Append("</td>");
				tmpS.Append("</tr>");
				i++;
			}
			tmpS.Append("</table>\n");
			ds.Dispose();
			
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			if(CachingOn && useCache)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public decimal GetCustomerLevelDiscountAmount(int CustomerLevelID)
		{
			if(CustomerLevelID == 0)
			{
				// consumers always have tax:
				return System.Decimal.Zero;
			}
			decimal tmpS = System.Decimal.Zero;
			IDataReader rs = DB.GetRS("Select LevelDiscountAmount from CustomerLevel  " + DB.GetNoLock() + " where CustomerLevelID=" + CustomerLevelID.ToString());
			if(rs.Read())
			{
				tmpS = DB.RSFieldDecimal(rs,"LevelDiscountAmount");
			}
			rs.Close();
			return tmpS;

		}

		static public Single GetCustomerLevelDiscountPercent(int CustomerLevelID)
		{
			if(CustomerLevelID == 0)
			{
				// consumers always have tax:
				return 0.0F;
			}
			Single tmpS = 0.0F;
			IDataReader rs = DB.GetRS("Select LevelDiscountPercent from CustomerLevel  " + DB.GetNoLock() + " where CustomerLevelID=" + CustomerLevelID.ToString());
			if(rs.Read())
			{
				tmpS = DB.RSFieldSingle(rs,"LevelDiscountPercent");
			}
			rs.Close();
			return tmpS;

		}

		static public bool CustomerLevelHasNoTax(int CustomerLevelID)
		{
			if(CustomerLevelID == 0)
			{
				// consumers always have tax:
				return false;
			}
			bool tmpS = false;
			IDataReader rs = DB.GetRS("Select LevelHasNoTax from CustomerLevel  " + DB.GetNoLock() + " where CustomerLevelID=" + CustomerLevelID.ToString());
			if(rs.Read())
			{
				tmpS = DB.RSFieldBool(rs,"LevelHasNoTax");
			}
			rs.Close();
			return tmpS;

		}

		static public bool CustomerLevelHasFreeShipping(int CustomerLevelID)
		{
			if(CustomerLevelID == 0)
			{
				// consumers always have shipping, unless overridden by other parameters:
				return false;
			}
			bool tmpS = false;
			IDataReader rs = DB.GetRS("Select LevelHasFreeShipping from CustomerLevel  " + DB.GetNoLock() + " where CustomerLevelID=" + CustomerLevelID.ToString());
			if(rs.Read())
			{
				tmpS = DB.RSFieldBool(rs,"LevelHasFreeShipping");
			}
			rs.Close();
			return tmpS;

		}
		
		static public bool CustomerLevelAllowsCoupons(int CustomerLevelID)
		{
			if(CustomerLevelID == 0)
			{
				// consumers always have this option by default, it can be overridden by product/variant settings however:
				return true;
			}
			bool tmpS = false;
			IDataReader rs = DB.GetRS("Select LevelAllowsCoupons from CustomerLevel  " + DB.GetNoLock() + " where CustomerLevelID=" + CustomerLevelID.ToString());
			if(rs.Read())
			{
				tmpS = DB.RSFieldBool(rs,"LevelAllowsCoupons");
			}
			rs.Close();
			return tmpS;

		}

		static public bool CustomerLevelAllowsQuantityDiscounts(int CustomerLevelID)
		{
			if(CustomerLevelID == 0)
			{
				// consumers always have this option by default, it can be overridden by product/variant settings however:
				return true;
			}
			bool tmpS = false;
			IDataReader rs = DB.GetRS("Select LevelAllowsQuantityDiscounts from CustomerLevel  " + DB.GetNoLock() + " where CustomerLevelID=" + CustomerLevelID.ToString());
			if(rs.Read())
			{
				tmpS = DB.RSFieldBool(rs,"LevelAllowsQuantityDiscounts");
			}
			rs.Close();
			return tmpS;

		}

		static public decimal GetVariantExtendedPrice(int VariantID, int CustomerLevelID)
		{
			decimal pr = System.Decimal.Zero;
			if(CustomerLevelID != 0)
			{
				IDataReader rs = DB.GetRS("select Price from ExtendedPrice  " + DB.GetNoLock() + " where VariantID=" + VariantID.ToString() + " and CustomerLevelID=" + CustomerLevelID.ToString());
				if(rs.Read())
				{
					pr = DB.RSFieldDecimal(rs,"Price");
				}
				rs.Close();
			}
			return pr;
		}

		static public decimal GetVariantPrice(int VariantID)
		{
			decimal pr = System.Decimal.Zero;
			IDataReader rs = DB.GetRS("select Price from productvariant  " + DB.GetNoLock() + " where VariantID=" + VariantID.ToString());
			if(rs.Read())
			{
				pr = DB.RSFieldDecimal(rs,"Price");
			}
			rs.Close();
			return pr;
		}

		static public int GetVariantPoints(int VariantID)
		{
			int pr = 0;
			IDataReader rs = DB.GetRS("select Points from productvariant  " + DB.GetNoLock() + " where VariantID=" + VariantID.ToString());
			if(rs.Read())
			{
				pr = DB.RSFieldInt(rs,"Points");
			}
			rs.Close();
			return pr;
		}

		static public decimal GetVariantSalePrice(int VariantID)
		{
			decimal pr = System.Decimal.Zero;
			IDataReader rs = DB.GetRS("select SalePrice from productvariant  " + DB.GetNoLock() + " where VariantID=" + VariantID.ToString());
			if(rs.Read())
			{
				pr = DB.RSFieldDecimal(rs,"SalePrice");
			}
			rs.Close();
			return pr;
		}

		static public decimal DetermineLevelPrice(int VariantID, int CustomerLevelID, out bool IsOnSale)
		{
			// the way the site is written, this should NOT be called with CustomerLevelID=0 but, you never know
			// if that's the case, return the sale price if any, and if not, the regular price instead:
			decimal pr = System.Decimal.Zero;
			IsOnSale = false;
			if(CustomerLevelID ==0)
			{
				IDataReader rs = DB.GetRS("select * from productvariant  " + DB.GetNoLock() + " where VariantID=" + VariantID.ToString());
				if(rs.Read())
				{
					if(DB.RSFieldDecimal(rs,"SalePrice") != System.Decimal.Zero)
					{
						pr = DB.RSFieldDecimal(rs,"SalePrice");
						IsOnSale = true;
					}
					else
					{
						pr = DB.RSFieldDecimal(rs,"Price");
					}
				}
				else
				{
					rs.Close();
					// well, this is bad, we can't return 0, and we don't have ANY valid price to return...stop the web page!
					throw(new ApplicationException("Invalid Variant Price Structure, VariantID=" + VariantID.ToString()));
				}
				rs.Close();
			}
			else
			{
				// ok, now for the hard part (e.g. the fun)
				// determine the actual price for this thing, considering everything involved!
				// If we have an extended price, get that first!
				IDataReader rs = DB.GetRS("select * from extendedprice  " + DB.GetNoLock() + " where VariantID=" + VariantID.ToString() + " and CustomerLevelID=" + CustomerLevelID.ToString());
				bool ExtendedPriceFound = false;
				if(rs.Read())
				{
					pr = DB.RSFieldDecimal(rs,"Price");
					ExtendedPriceFound = true;
				}
				rs.Close();
				if(!ExtendedPriceFound)
				{
					pr = Common.GetVariantPrice(VariantID);
				}
				// now get the "level" info:
				rs = DB.GetRS("select * from CustomerLevel  " + DB.GetNoLock() + " where CustomerLevelID=" + CustomerLevelID.ToString());
				rs.Read();
				Double DiscountPercent = DB.RSFieldSingle(rs,"LevelDiscountPercent");
				bool LevelDiscountsApplyToExtendedPrices = DB.RSFieldBool(rs,"LevelDiscountsApplyToExtendedPrices");
				rs.Close();
				if(DiscountPercent != 0.0F)
				{
					if(!ExtendedPriceFound || (ExtendedPriceFound && LevelDiscountsApplyToExtendedPrices))
					{
						pr = pr * (decimal)(1.00F - (DiscountPercent/100.0F));
					}

				}
			}
			return pr;
		}

		static public String GetCustomerLevelName(int CustomerLevelID)
		{
			String tmpS = String.Empty;
			IDataReader rs = DB.GetRS("Select * from CustomerLevel  " + DB.GetNoLock() + " where CustomerLevelID=" + CustomerLevelID.ToString());
			if(rs.Read())
			{
				tmpS = DB.RSField(rs,"Name");
			}
			rs.Close();
			return tmpS;
		}

		static public String GetQuantityDiscountName(int QuantityDiscountID)
		{
			String tmpS = String.Empty;
			IDataReader rs = DB.GetRS("Select Name from QuantityDiscount  " + DB.GetNoLock() + " where QuantityDiscountID=" + QuantityDiscountID.ToString());
			if(rs.Read())
			{
				tmpS = DB.RSField(rs,"Name");
			}
			rs.Close();
			return tmpS;
		}

		// don't return any quotes, single quotes, or carraige returns in this string!
		static public String GetQuantityDiscountDisplayTable(int DID)
		{
			String CacheName = "GetQuantityDiscountDisplayTable" + DID.ToString();
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String CacheData = (String)HttpContext.Current.Cache.Get(CacheName);
				if(CacheData != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>");
					}
					return CacheData;
				}
			}

			StringBuilder tmpS = new StringBuilder(10000);
			String sql = "select * from QuantityDiscountTable " + DB.GetNoLock() + "  where QuantityDiscountID=" + DID.ToString() + " order by LowQuantity";
			IDataReader rs = DB.GetRS(sql);
			tmpS.Append("<table border=0 cellpadding=4 cellspacing=0>");
			tmpS.Append("<tr><td align=center><b>Quantity</b></td><td align=center><b>Discount</b></td></tr>");
			while(rs.Read())
			{
				tmpS.Append("<tr>");
				tmpS.Append("<td align=center>");
				tmpS.Append(DB.RSFieldInt(rs,"LowQuantity").ToString() + Common.IIF(DB.RSFieldInt(rs,"HighQuantity") > 9999 , "+" , "-" + DB.RSFieldInt(rs,"HighQuantity").ToString()));
				tmpS.Append("</td>");
				tmpS.Append("<td align=center>");
				tmpS.Append(DB.RSFieldSingle(rs,"DiscountPercent").ToString() + "%");
				tmpS.Append("</td>");
				tmpS.Append("</tr>");
			}
			tmpS.Append("</table>");
			rs.Close();
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public bool CategoryHasActiveQuantityDiscount(int CategoryID)
		{
			return (Common.LookupActiveCategoryQuantityDiscountID(CategoryID) != 0);
		}

		static public bool ProductHasActiveQuantityDiscount(int ProductID)
		{
			return (Common.LookupActiveProductQuantityDiscountID(ProductID) != 0);
		}

		static public bool VariantHasActiveQuantityDiscount(int VariantID)
		{
			return (Common.LookupActiveVariantQuantityDiscountID(VariantID) != 0);
		}

		static public Single GetDIDPercent(int QuantityDiscountID, int Quantity)
		{
			Single tmp = 0.0F;
			if(QuantityDiscountID != 0)
			{
				IDataReader rs = DB.GetRS("select * from QuantityDiscountTable  " + DB.GetNoLock() + " where QuantityDiscountID=" + QuantityDiscountID.ToString() + " and LowQuantity<=" + Quantity.ToString() + " and HighQuantity>=" + Quantity.ToString());
				if(rs.Read())
				{
					tmp = DB.RSFieldSingle(rs,"DiscountPercent");
				}
				rs.Close();
			}
			return tmp;
		}

		static public Single LookupVariantQuantityDiscountPercent(int VariantID, int Quantity)
		{
			return GetDIDPercent(LookupActiveVariantQuantityDiscountID(VariantID),Quantity);
		}

		static public Single LookupProductQuantityDiscountPercent(int ProductID, int Quantity)
		{
			return GetDIDPercent(LookupActiveProductQuantityDiscountID(ProductID),Quantity);
		}

		static public int LookupActiveCategoryQuantityDiscountID(int CategoryID)
		{
			int DID = Common.GetProductQuantityDiscountID(CategoryID);
			if(DID == 0)
			{
				// no active discount found,try site wide default:
				DID = Common.GetQuantityDiscountID(Common.AppConfig("ActiveQuantityDiscountTable"));
			}
			return DID;
		}

		static public int LookupActiveProductQuantityDiscountID(int ProductID)
		{
			int DID = Common.GetProductQuantityDiscountID(ProductID);
			if(DID == 0)
			{
				//no active discount found, try each category level (in no particular order, first found wins):
				String[] CategoryIDs = Common.GetProductCategories(ProductID,false).Split(',');
				for(int i = CategoryIDs.GetLowerBound(0); i <= CategoryIDs.GetUpperBound(0); i++)
				{
					if(CategoryIDs[i].Length != 0)
					{
						DID = Common.GetCategoryQuantityDiscountID(Localization.ParseUSInt(CategoryIDs[i]));
					}
					if(DID != 0)
					{
						break;
					}
				}
				if(DID == 0)
				{
					// still no active discount found,try site wide default:
					DID = Common.GetQuantityDiscountID(Common.AppConfig("ActiveQuantityDiscountTable"));
				}
			}
			return DID;
		}

		static public int LookupActiveVariantQuantityDiscountID(int VariantID)
		{
			int DID = Common.GetVariantQuantityDiscountID(VariantID);
			if(DID == 0)
			{
				// no active discount at variant level, try product level:
				int ProductID = Common.GetVariantProductID(VariantID);
				DID = Common.GetProductQuantityDiscountID(ProductID);
				if(DID == 0)
				{
					//no active discount found, try each category level (in no particular order, first found wins):
					String[] CategoryIDs = Common.GetProductCategories(ProductID,false).Split(',');
					for(int i = CategoryIDs.GetLowerBound(0); i <= CategoryIDs.GetUpperBound(0); i++)
					{
						if(CategoryIDs[i].Length != 0)
						{
							DID = Common.GetCategoryQuantityDiscountID(Localization.ParseUSInt(CategoryIDs[i]));
							if(DID != 0)
							{
								break;
							}
						}
					}
					if(DID == 0)
					{
						// still no active discount found,try site wide default:
						DID = Common.GetQuantityDiscountID(Common.AppConfig("ActiveQuantityDiscountTable"));
					}
				}
			}
			return DID;
		}

		//low level accessor: no "store" logic applied:
		static public int GetVariantQuantityDiscountID(int VariantID)
		{
			int tmp = 0;
			IDataReader rs = DB.GetRS("select QuantityDiscountID from productvariant  " + DB.GetNoLock() + " where variantid=" + VariantID.ToString());
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"QuantityDiscountID");
			}
			rs.Close();
			return tmp;
		}

		//low level accessor: no "store" logic applied:
		static public int GetProductQuantityDiscountID(int ProductID)
		{
			int tmp = 0;
			IDataReader rs = DB.GetRS("select QuantityDiscountID from product  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString());
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"QuantityDiscountID");
			}
			rs.Close();
			return tmp;
		}

		//low level accessor: no "store" logic applied:
		static public int GetCategoryQuantityDiscountID(int CategoryID)
		{
			int tmp = 0;
			IDataReader rs = DB.GetRS("select QuantityDiscountID from  Category " + DB.GetNoLock() + " where CategoryID=" + CategoryID.ToString());
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"QuantityDiscountID");
			}
			rs.Close();
			return tmp;
		}

		static public int GetQuantityDiscountID(String Name)
		{
			int tmp = 0;
			IDataReader rs = DB.GetRS("select QuantityDiscountID from  QuantityDiscount " + DB.GetNoLock() + " where lower(name)=" + DB.SQuote(Name.ToLower()));
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"QuantityDiscountID");
			}
			rs.Close();
			return tmp;
		}


		static public int GetNumSlides(String inDir)
		{
			String tPath = inDir;
			if(inDir.IndexOf(":") == -1 && inDir.IndexOf("\\\\") == -1)
			{
				tPath = HttpContext.Current.Server.MapPath(inDir);
			}
			bool anyFound = false;
			for(int i=1; i <= Common.AppConfigUSInt("MaxSlides"); i++)
			{
				if(!Common.FileExists(tPath + "slide" + i.ToString().PadLeft(2,'0') + ".jpg"))
				{
					return i-1;
				}
				else
				{
					anyFound = true;
				}
			}
			return Common.IIF(anyFound , Common.AppConfigUSInt("MaxSlides") , 0);
		}
	

		static public String GetGalleryTable(int SiteID)
		{
			String CacheName = "GetGalleryTable";
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String CacheData = (String)HttpContext.Current.Cache.Get(CacheName);
				if(CacheData != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return CacheData;
				}
			}

			StringBuilder tmpS = new StringBuilder(10000);
			String sql = "select * from gallery  " + DB.GetNoLock() + " where deleted=0 order by DisplayOrder,Name";
			IDataReader rs = DB.GetRS(sql);
			tmpS.Append("<table border=\"0\" cellpadding=\"4\" cellspacing=\"0\">\n");
			tmpS.Append("<tr>\n");
			while(rs.Read())
			{
				tmpS.Append("<td valign=\"bottom\" align=\"center\">");
				String GalIcon = Common.LookupImage("Gallery",DB.RSFieldInt(rs,"GalleryID"),"",SiteID);
				if(GalIcon.Length == 0)
				{
					GalIcon = "images/spacer.gif";
				}
				tmpS.Append("<a target=\"_blank\" href=\"showgallery.aspx?galleryid=" + DB.RSFieldInt(rs,"GalleryID").ToString() + "\"><img border=\"0\" src=\"" + GalIcon + "?" + Common.GetRandomNumber(1,1000000).ToString() + "\"></a><br>\n");
				tmpS.Append("<a target=\"_blank\" href=\"showgallery.aspx?galleryid=" + DB.RSFieldInt(rs,"GalleryID").ToString() + "\">" + DB.RSField(rs,"Name") + "</a>");
				tmpS.Append("</td>\n");
			}
			tmpS.Append("</tr>\n");
			tmpS.Append("</table>\n");
			rs.Close();
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public String GetGalleryName(int GalleryID)
		{
			String tmpS = String.Empty;
			IDataReader rs = DB.GetRS("Select * from Gallery  " + DB.GetNoLock() + " where GalleryID=" + GalleryID.ToString());
			if(rs.Read())
			{
				tmpS = DB.RSField(rs,"Name");
			}
			rs.Close();
			return tmpS;
		}

		static public String GetGalleryDir(int GalleryID)
		{
			String tmpS = String.Empty;
			IDataReader rs = DB.GetRS("Select * from Gallery  " + DB.GetNoLock() + " where GalleryID=" + GalleryID.ToString());
			if(rs.Read())
			{
				tmpS = DB.RSField(rs,"DirName");
			}
			rs.Close();
			return tmpS;
		}

		public static String MakeProperPhoneFormat(String PhoneNumber)
		{
			return PhoneNumber;
			//			if(PhoneNumber.Length == 0)
			//			{
			//				return String.Empty;
			//			}
			//			if(PhoneNumber.Substring(0,1) != "1")
			//			{
			//				PhoneNumber = "1" + PhoneNumber;
			//			}
			//			String newS = String.Empty;
			//			String validDigits = "0123456789";
			//			for(int i = 1; i<= PhoneNumber.Length; i++)
			//			{
			//				if(validDigits.IndexOf(PhoneNumber[i-1]) != -1)
			//				{
			//					newS = newS + PhoneNumber[i-1];
			//				}
			//			}
			//			return newS;
		}

		public static String GetPhoneDisplayFormat(String PhoneNumber)
		{
			if(PhoneNumber.Length == 0)
			{
				return String.Empty;
			}
			if(PhoneNumber.Length != 11)
			{
				return PhoneNumber;
			}
			return "(" + PhoneNumber.Substring(1,3) + ") " + PhoneNumber.Substring(4,3) + "-" + PhoneNumber.Substring(7,4);
		}
		
		static public bool ProductHasBeenDeleted(int ProductID)
		{
			bool tmp = true;
			IDataReader rs = DB.GetRS("Select name from product  " + DB.GetNoLock() + " where deleted=0 and published<>0 and productid=" + ProductID.ToString());
			if(rs.Read())
			{
				tmp = false;
			}
			rs.Close();
			return tmp;
		}


		public static String GetAdminDir()
		{
			String AdminDir = Common.AppConfig("AdminDir");
			if(AdminDir.Length == 0)
			{
				AdminDir = "admin";
			}
			if(AdminDir.EndsWith("/"))
			{
				AdminDir = AdminDir.Substring(0,AdminDir.Length - 1);
			}
			return AdminDir;
		}
		
		public static String GetAdminHTTPLocation(bool TryToUseSSL)
		{
			return GetStoreHTTPLocation(TryToUseSSL) + Common.GetAdminDir() + "/";
		}

		public static bool OnLiveServer()
		{
			return (Common.ServerVariables("HTTP_HOST").ToLower().IndexOf(Common.AppConfig("LiveServer").ToLower()) != -1);
		}

		public static void SessionStart()
		{
			// after session has started, the Customer classes USES session["CustomerID"] to track the customer (if any)
			// theoretically, if the Customer class COULD rebuild the customer object from the CustomerGUID cookie on every page, but that
			// would be innefficient, so this session build does it ONCE on session start to map CustomerGUID to CustomerID, or to make
			// a new customer record if there are none, and one is required (and delayed customer creation is not set)
			// admin site works just a little different...no anon records should be created ever...
			
			// Also, if in a serverfarm, Application_PreBeginRequest forces session rebuild EVERY page. we have no idea if they
			// changed identities "between" server visits, where they could already have an "old" session (out of date now), so 
			// must force session update on every page

			// if not in serverfarm, session_start calls this on, well, session start ;)
			
			// since this session build from cookie works on any web server, it is farmable in terms of customer management
			// (i.e. the customer session will be rebuilt on each web server in a farm, if their session is shared between servers.

			// SPECIAL CASES: DO NOT create a session for the gateway callbacks!
			if(Common.ServerVariables("SCRIPT_NAME").ToLower().IndexOf("worldpayreturn.aspx") != -1)
			{
				return;
			}

			Customer.RecordAffiliateSessionCookie();

			int CustomerID = 0;
			// get the cookie, if any:
			String CustomerGUID = HttpContext.Current.User.Identity.Name;

			bool rebuilt = false;
			// if there was a cookie, try to get the CustomerID corresponding to it:
			if (CustomerGUID.Length > 0)
			{
				try
				{
					// found cookie, restore session from cookie:
					String sql = "select CustomerID,CustomerGUID from customer  " + DB.GetNoLock() + " where deleted=0 and CustomerGUID=" + DB.SQuote(CustomerGUID);
					IDataReader rs = DB.GetRS(sql);
					if (rs.Read())
					{
						rebuilt = true;
						HttpContext.Current.Session["CustomerID"] = DB.RSFieldInt(rs,"CustomerID").ToString();
						HttpContext.Current.Session["CustomerGUID"] = DB.RSFieldGUID(rs,"CustomerGUID");
					}
					rs.Close();
				}
				catch {}
			}

			// remember, since this code is common to BOTH projects, don't build any customer records on admin site (we don't need customer records there in session)
			if(!rebuilt && !Common.IsAdminSite && !Common.ApplicationBool("DelayedCustomerCreation"))
			{
				// no cookie or invalid cookie, so setup a new blank (anon) customer session/record:
				Customer.MakeAnonCustomerRecord(out CustomerID, out CustomerGUID); // this really cannot fail, if it does, let page DIE!

				HttpContext.Current.Session["CustomerID"] = CustomerID;
				HttpContext.Current.Session["CustomerGUID"] = CustomerGUID;
				rebuilt = true;
				// try to write them a new cookie (may fail, but it's ok), so when they visit next time, we will remember who they are:
				//V3_9    Common.SetCookie(Common.UserCookieName(),"E-" + Common.MungeString(CustomerGUID),new TimeSpan(1000,0,0,0,0));
				//V3_9 Authenticate with the temporary GUID with a persistent cookie of 7 days expiration			
				FormsAuthentication.SetAuthCookie(CustomerGUID,true);
				//V3_9 Set the expriation
				HttpCookie cookie = HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName];
				cookie.Expires = DateTime.Now.Add(new TimeSpan(1000,0,0,0));
			}
			if(rebuilt && CustomerID != 0)
			{
				// age their customersession data (in the db):
				CustomerSession.StaticAge(CustomerID);
			}
		}

		public static int SessionTimeout()
		{
			int ST = Common.AppConfigUSInt("SessionTimeoutInMinutes");
			if(ST == 0)
			{
				ST = 60;
			}
			return ST;
		}
		

		public static bool IsNumber(string expression) 
		{ 
			if(expression.Trim().Length == 0)
			{
				return false;
			}
			expression = expression.Trim();
			bool hasDecimal = false;
			int startIdx = 0;
			if(expression.StartsWith("-"))
			{
				startIdx = 1;
			}
			for(int i = startIdx;i<expression.Length;i++) 
			{ 
				// Check for decimal
				if (expression[i] == '.')
				{
					if (hasDecimal) // 2nd decimal
						return false;
					else // 1st decimal
					{
						// inform loop decimal found and continue 
						hasDecimal = true;
						continue;
					}
				}
				// check if number
				if(!char.IsNumber(expression[i])) 
					return false; 
			} 
			return true; 
		} 

		public static bool IsInteger(string expression) 
		{ 
			if(expression.Trim().Length == 0)
			{
				return false;
			}
			// leading - is ok
			expression = expression.Trim();
			int startIdx = 0;
			if(expression.StartsWith("-"))
			{
				startIdx = 1;
			}
			for(int i=startIdx;i<expression.Length;i++) 
			{ 
				if(!char.IsNumber(expression[i]))
				{
					return false; 
				}
			} 
			return true; 
		} 

		public static int GetNumCategoryProducts(int CategoryID, bool includeKits, bool includePacks)
		{
			return DB.GetSqlN("select count(*) as N from product  " + DB.GetNoLock() + " where deleted=0 and published<>0 " + Common.IIF(includeKits, "", " and IsAKit=0") + Common.IIF(includePacks, "", " and IsAPack=0") + " and productid in (select distinct productid from productCategory where CategoryID=" + CategoryID.ToString() + ")");
		}

		public static int GetNumSectionProducts(int SectionID, bool includeKits, bool includePacks)
		{
			return DB.GetSqlN("select count(*) as N from product  " + DB.GetNoLock() + " where deleted=0 and published<>0 " + Common.IIF(includeKits, "", " and IsAKit=0") + Common.IIF(includePacks, "", " and IsAPack=0") + " and productid in (select distinct productid from productSection where SectionID=" + SectionID.ToString() + ")");
		}

		public static int GetProductDisplayOrder(int ProductID, int CategoryID)
		{
			IDataReader rs = DB.GetRS("select DisplayOrder from CategoryDisplayOrder  " + DB.GetNoLock() + " where productid=" + ProductID.ToString() + " and categoryid=" + CategoryID.ToString());
			int tmp = 0;
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"DisplayOrder");
			}
			rs.Close();
			return tmp;
		}

		public static int GetProductDisplayOrderSection(int ProductID, int SectionID)
		{
			IDataReader rs = DB.GetRS("select DisplayOrder from SectionDisplayOrder  " + DB.GetNoLock() + " where productid=" + ProductID.ToString() + " and Sectionid=" + SectionID.ToString());
			int tmp = 0;
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"DisplayOrder");
			}
			rs.Close();
			return tmp;
		}


		// returns the "next" product in this category, after the specified product
		// "next" is defined as either the product that is next higher display order, or same display order and next highest alphabetical order
		// is circular also (i.e. if last, return first)
		public static int GetNextProduct(int ProductID, int CategoryID, int SectionID, int ManufacturerID, int ProductTypeID, bool SortByLooks, bool includeKits, bool includePacks)
		{
			String sql = String.Empty;
			if(CategoryID != 0)
			{
				sql = "SELECT P.ProductID FROM ((Category C  " + DB.GetNoLock() + " INNER JOIN ProductCategory PC  " + DB.GetNoLock() + " ON C.CategoryID = PC.CategoryID) INNER JOIN Product P  " + DB.GetNoLock() + " ON PC.ProductID = P.ProductID) left outer join CategoryDisplayOrder DO  " + DB.GetNoLock() + " on p.productid=DO.productid WHERE DO.categoryid=" + CategoryID.ToString() + "  and (P.Published = 1) AND (P.Deleted = 0) " + Common.IIF(includeKits, "", " and P.IsAKit=0") + Common.IIF(includePacks, "", " and P.IsAPack=0") + " and PC.categoryid=" + CategoryID.ToString();
			}
			else
			{
				sql = "SELECT P.ProductID from (([section] C  " + DB.GetNoLock() + " INNER JOIN ProductSection PC  " + DB.GetNoLock() + " ON C.SectionID = PC.SectionID) INNER JOIN Product P  " + DB.GetNoLock() + " ON PC.ProductID = P.ProductID) left outer join SectionDisplayOrder DO  " + DB.GetNoLock() + " on p.productid=DO.productid WHERE DO.Sectionid=" + SectionID.ToString() + " and (P.Published = 1) AND (P.Deleted = 0) " + Common.IIF(includeKits, "", " and P.IsAKit=0") + Common.IIF(includePacks, "", " and P.IsAPack=0") + " and PC.Sectionid=" + SectionID.ToString();
			}
			if(ManufacturerID != 0)
			{
				sql += " and p.manufacturerID =" + ManufacturerID.ToString();
			}
			if(ProductTypeID != 0)
			{
				sql += " and p.ProductTypeID=" + ProductTypeID.ToString();
			}
			if(SortByLooks)
			{
				sql += " order by looks desc, DO.displayorder, p.name";
			}
			else
			{
				sql += " order by DO.displayorder, p.name";
			}
			DataSet ds = DB.GetDS(sql,false,System.DateTime.Now.AddHours(1));
			if(ds.Tables[0].Rows.Count == 0)
			{
				ds.Dispose();
				return 0;
			}
			int i = 0;
			bool found = false;
			for(i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				if(ds.Tables[0].Rows[i]["ProductID"].ToString() == ProductID.ToString())
				{
					found = true;
					break;
				}
			}
			int id = 0;
			if(found)
			{
				if(i == ds.Tables[0].Rows.Count-1)
				{
					// if last, go to first
					id = Localization.ParseUSInt(ds.Tables[0].Rows[0]["ProductID"].ToString());
				}
				else
				{
					id = Localization.ParseUSInt(ds.Tables[0].Rows[i+1]["ProductID"].ToString());
				}
			}
			ds.Dispose();
			return id;
		}


		public static int GetCategoryParentID(int CategoryID)
		{
			IDataReader rs = DB.GetRS("Select parentcategoryid from Category  " + DB.GetNoLock() + " where CategoryID=" + CategoryID.ToString());
			int tmp = 0;
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"parentcategoryid");
			}
			rs.Close();
			return tmp;
		}

		public static int GetSectionParentID(int SectionID)
		{
			IDataReader rs = DB.GetRS("Select parentSectionID from [section]  " + DB.GetNoLock() + " where SectionID=" + SectionID.ToString());
			int tmp = 0;
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"parentSectionID");
			}
			rs.Close();
			return tmp;
		}
		
		// returns the "next" category, after the specified category
		// "next" is defined as either the category that is next higher display order, or same display order and next highest alphabetical order
		// is circular also (i.e. if last, return first)
		public static int GetNextCategory(int CategoryID)
		{
			int PID = Common.GetCategoryParentID(CategoryID);
			String sql = "SELECT C.CategoryID, C.Name AS CategoryName FROM Category C  " + DB.GetNoLock() + " where deleted=0 " + Common.IIF(PID == 0 , " and (parentcategoryid=0 or parentcategoryid IS NULL) " , " and parentcategoryid=" + PID.ToString()) + " order by displayorder, name";
			DataSet ds = DB.GetDS(sql,false,System.DateTime.Now.AddHours(1));
			if(ds.Tables[0].Rows.Count == 0)
			{
				ds.Dispose();
				return 0;
			}
			int i = 0;
			bool found = false;
			for(i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				if(ds.Tables[0].Rows[i]["CategoryID"].ToString() == CategoryID.ToString())
				{
					found = true;
					break;
				}
			}
			int id = 0;
			if(found)
			{
				if(i == ds.Tables[0].Rows.Count-1)
				{
					// if last, go to first
					id = Localization.ParseUSInt(ds.Tables[0].Rows[0]["CategoryID"].ToString());
				}
				else
				{
					id = Localization.ParseUSInt(ds.Tables[0].Rows[i+1]["CategoryID"].ToString());
				}
			}
			ds.Dispose();
			return id;
		}

		// returns the "next" Section, after the specified Section
		// "next" is defined as either the Section that is next higher display order, or same display order and next highest alphabetical order
		// is circular also (i.e. if last, return first)
		public static int GetNextSection(int SectionID)
		{
			int PID = Common.GetSectionParentID(SectionID);
			String sql = "SELECT C.SectionID, C.Name AS SectionName from [section] C  " + DB.GetNoLock() + " where deleted=0 " + Common.IIF(PID == 0 , " and (parentSectionid=0 or parentSectionid IS NULL) " , " and parentSectionid=" + PID.ToString()) + " order by displayorder, name";
			DataSet ds = DB.GetDS(sql,false,System.DateTime.Now.AddHours(1));
			if(ds.Tables[0].Rows.Count == 0)
			{
				ds.Dispose();
				return 0;
			}
			int i = 0;
			bool found = false;
			for(i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				if(ds.Tables[0].Rows[i]["SectionID"].ToString() == SectionID.ToString())
				{
					found = true;
					break;
				}
			}
			int id = 0;
			if(found)
			{
				if(i == ds.Tables[0].Rows.Count-1)
				{
					// if last, go to first
					id = Localization.ParseUSInt(ds.Tables[0].Rows[0]["SectionID"].ToString());
				}
				else
				{
					id = Localization.ParseUSInt(ds.Tables[0].Rows[i+1]["SectionID"].ToString());
				}
			}
			ds.Dispose();
			return id;
		}

		public static int GetCategoryDisplayOrder(int CategoryID)
		{
			IDataReader rs = DB.GetRS("select DisplayOrder from Category  " + DB.GetNoLock() + " where categoryid=" + CategoryID.ToString());
			int tmp = 0;
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"DisplayOrder");
			}
			rs.Close();
			return tmp;
		}

		public static int GetSectionDisplayOrder(int SectionID)
		{
			IDataReader rs = DB.GetRS("select DisplayOrder from [section]  " + DB.GetNoLock() + " where SectionID=" + SectionID.ToString());
			int tmp = 0;
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"DisplayOrder");
			}
			rs.Close();
			return tmp;
		}


		// returns the "previous" product in this category, after the specified product
		// "previous" is defined as either the product that is next lower display order, or same display order and next lowest alphabetical order
		// is circular also (i.e. if first, return last)
		public static int GetPreviousProduct(int ProductID, int CategoryID, int SectionID, int ManufacturerID, int ProductTypeID, bool SortByLooks, bool includeKits, bool includePacks)
		{
			String sql = String.Empty;
			if(CategoryID != 0)
			{
				sql = "SELECT P.ProductID FROM ((Category C  " + DB.GetNoLock() + " INNER JOIN ProductCategory PC  " + DB.GetNoLock() + " ON C.CategoryID = PC.CategoryID) INNER JOIN Product P  " + DB.GetNoLock() + " ON PC.ProductID = P.ProductID) left outer join CategoryDisplayOrder DO  " + DB.GetNoLock() + " on p.productid=DO.productid WHERE DO.categoryid=" + CategoryID.ToString() + " and (P.Published = 1) AND (P.Deleted = 0) " + Common.IIF(includeKits, "", " and P.IsAKit=0") + Common.IIF(includePacks, "", " and P.IsAPack=0") + " and PC.categoryid=" + CategoryID.ToString();
			}
			else
			{
				sql = "SELECT P.ProductID from (([section] C  " + DB.GetNoLock() + " INNER JOIN ProductSection PC  " + DB.GetNoLock() + " ON C.SectionID = PC.SectionID) INNER JOIN Product P  " + DB.GetNoLock() + " ON PC.ProductID = P.ProductID) left outer join SectionDisplayOrder DO  " + DB.GetNoLock() + " on p.productid=DO.productid WHERE DO.Sectionid=" + SectionID.ToString() + " and (P.Published = 1) AND (P.Deleted = 0) " + Common.IIF(includeKits, "", " and P.IsAKit=0") + Common.IIF(includePacks, "", " and P.IsAPack=0") + " and PC.Sectionid=" + SectionID.ToString();
			}
			if(ManufacturerID != 0)
			{
				sql += " and p.manufacturerID =" + ManufacturerID.ToString();
			}
			if(ProductTypeID != 0)
			{
				sql += " and p.ProductTypeID=" + ProductTypeID.ToString();
			}
			if(SortByLooks)
			{
				sql += " order by looks desc, DO.displayorder, p.name";
			}
			else
			{
				sql += " order by DO.displayorder, p.name";
			}
			DataSet ds = DB.GetDS(sql,false,System.DateTime.Now.AddHours(1));
			if(ds.Tables[0].Rows.Count == 0)
			{
				ds.Dispose();
				return 0;
			}
			int i = 0;
			for(i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				if(ds.Tables[0].Rows[i]["ProductID"].ToString() == ProductID.ToString())
				{
					break;
				}
			}
			int id = 0;
			if(i == 0)
			{
				// if first, go to last
				id = Localization.ParseUSInt(ds.Tables[0].Rows[ds.Tables[0].Rows.Count-1]["ProductID"].ToString());
			}
			else
			{
				id = Localization.ParseUSInt(ds.Tables[0].Rows[i-1]["ProductID"].ToString());
			}
			ds.Dispose();
			return id;
		}

		public static int GetParentSection(int SectionID)
		{
			IDataReader rs = DB.GetRS("Select ParentSectionID from [section]  " + DB.GetNoLock() + " where SectionID=" + SectionID.ToString());
			int tmp = 0;
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"ParentSectionID");
			}
			rs.Close();
			return tmp;
		}

		public static int GetParentCategory(int CategoryID)
		{
			IDataReader rs = DB.GetRS("Select ParentCategoryID from Category  " + DB.GetNoLock() + " where CategoryID=" + CategoryID.ToString());
			int tmp = 0;
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"ParentCategoryID");
			}
			rs.Close();
			return tmp;
		}

		public static String GetParentCategoryName(int CategoryID)
		{
			return GetCategoryName(GetParentCategory(CategoryID));
		}

		public static bool CategoryHasSubs(int CategoryID)
		{
			IDataReader rs = DB.GetRS("Select count(*) as N from Category  " + DB.GetNoLock() + " where published<>0 and deleted=0 and ParentCategoryID=" + CategoryID.ToString());
			int tmp = 0;
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"N");
			}
			rs.Close();
			return (tmp != 0);
		}

		public static bool SectionHasSubs(int SectionID)
		{
			IDataReader rs = DB.GetRS("Select count(*) as N from [Section]  " + DB.GetNoLock() + " where published<>0 and deleted=0 and ParentSectionID=" + SectionID.ToString());
			int tmp = 0;
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"N");
			}
			rs.Close();
			return (tmp != 0);
		}

		public static bool CategoryHasVisibleProducts(int CategoryID)
		{
			IDataReader rs = DB.GetRS("select count(*) as N from productcategory  " + DB.GetNoLock() + " where categoryid=" + CategoryID.ToString() + " and productid in (select distinct productid from product where deleted=0 and published=1)");
			rs.Read();
			bool tmp = (DB.RSFieldInt(rs,"N") != 0);
			rs.Close();
			return tmp;
		}

		public static bool SectionHasVisibleProducts(int SectionID)
		{
			IDataReader rs = DB.GetRS("select count(*) as N from productsection  " + DB.GetNoLock() + " where SectionID=" + SectionID.ToString() + " and productid in (select distinct productid from product where deleted=0 and published=1)");
			rs.Read();
			bool tmp = (DB.RSFieldInt(rs,"N") != 0);
			rs.Close();
			return tmp;
		}

		public static bool ManufacturerHasVisibleProducts(int ManufacturerID)
		{
			IDataReader rs = DB.GetRS("select count(*) as N from product " + DB.GetNoLock() + " where ManufacturerID=" + ManufacturerID.ToString() + " and deleted=0 and published=1");
			rs.Read();
			bool tmp = (DB.RSFieldInt(rs,"N") != 0);
			rs.Close();
			return tmp;
		}

		public static bool ProductTypeHasVisibleProducts(int ProductTypeID)
		{
			IDataReader rs = DB.GetRS("select count(*) as N from product  " + DB.GetNoLock() + " where ProductTypeID=" + ProductTypeID.ToString() + " and deleted=0 and published=1");
			rs.Read();
			bool tmp = (DB.RSFieldInt(rs,"N") != 0);
			rs.Close();
			return tmp;
		}

		public static String GetProductSalePrice(int ProductID)
		{
			// NOTE: IGNORE ANY EXTENDED PRICING HERE, THIS ALWAYS RETURNS NORMAL CUSTOMER PRICE AND SALE PRICE
			// YOU COULD ALTER THAT, BUT IT'S PROBABLY NOT NECESSARY, SPECIALS ARE TYPICALLY ONLY FOR "CONSUMERS"
			// return string in format: $regularprice,$saleprice (note that $saleprice could be empty), and
			// note that this proc returns the FIRST sales price of any variant found, if there are multiple sales prices
			// then you have to write a different proc if you want them returned.
			String tmpS = String.Empty;
			IDataReader rs = DB.GetRS("select * from product  " + DB.GetNoLock() + " left outer join productvariant  " + DB.GetNoLock() + " on product.productid=productvariant.productid where saleprice IS NOT NULL and saleprice<>price and product.productid=" + ProductID.ToString());
			if(rs.Read())
			{
				tmpS = Localization.CurrencyStringForDisplay(DB.RSFieldDecimal(rs,"Price")) + "|" + Localization.CurrencyStringForDisplay(DB.RSFieldDecimal(rs,"SalePrice"));
			}
			rs.Close();
			return tmpS;
		}

		static public String GetUserBox(Customer thisCustomer, int SiteID)
		{
			StringBuilder tmpS = new StringBuilder(5000);
			tmpS.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" >\n");
			tmpS.Append("<tr><td colspan=\"3\" height=20></td></tr>\n");
			tmpS.Append("<tr>\n");
			tmpS.Append("	<td colspan=\"2\" align=\"right\"><span style=\"color: white; font-size: 11px; font-weight: bold;\">You're Logged In As: <a class=\"DarkCellText\" href=\"account.aspx\"><b>" + thisCustomer.FullName() + "</b></a></span></td>\n");
			tmpS.Append("	<td width=\"25\"><img src=\"images/spacer.gif\" width=25\" height=\"1\"></td>\n");
			tmpS.Append("</tr>\n");
			tmpS.Append("<tr><td colspan=\"3\" height=4></td></tr>\n");
			tmpS.Append("<tr>\n");
			tmpS.Append("	<td colspan=\"2\" align=\"right\"><span style=\"color: white; font-size: 11px; font-weight: bold;\"><a class=\"DarkCellText\" href=\"signout.aspx\"><b>Logout</b></a></span></td>\n");
			tmpS.Append("	<td width=\"25\"><img src=\"images/spacer.gif\" width=25\" height=\"1\"></td>\n");
			tmpS.Append("</tr>\n");
			tmpS.Append("<tr>\n");
			tmpS.Append("	<td colspan=\"3\"><img src=\"skins/Skin_%SKINID%/images/spacer.gif\" height=\"2\" width=\"1\"></td></tr>\n");
			tmpS.Append("<tr>\n");
			tmpS.Append("	<td colspan=\"2\" align=\"right\"><span style=\"color: white; font-size: 11px; font-weight: bold;\"><a class=\"DarkCellText\" href=\"signin.aspx\"><b>Change User</b></a></span></td>\n");
			tmpS.Append("	<td width=\"25\"><img src=\"images/spacer.gif\" width=25\" height=\"1\"></td>\n");
			tmpS.Append("</tr>\n");
			tmpS.Append("</table>\n");
			return tmpS.ToString();
		}

		static public String GetLoginBox(int SiteID)
		{
			StringBuilder tmpS = new StringBuilder(5000);
			tmpS.Append("<form name=\"LoginForm\" method=\"POST\" action=\"signin.aspx\" onsubmit=\"return (validateForm(this) && LoginForm_Validator(this))\">\n");
			tmpS.Append("           <input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			tmpS.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\n");
			tmpS.Append("<tr>\n");
			tmpS.Append("	<td colspan=\"2\" align=\"right\"><span style=\"color: white; font-size: 11px; font-weight: bold;\">EXISTING CUSTOMER LOGIN:</span></td>\n");
			tmpS.Append("	<td width=\"15\"><img src=\"images/spacer.gif\" width=15\" height=\"1\"></td>\n");
			tmpS.Append("</tr>\n");
			//tmpS.Append("<tr><td colspan=\"3\" height=2></td></tr>\n");
			tmpS.Append("<tr>\n");
			tmpS.Append("	<td width=\"100%\" align=\"right\">\n");
			tmpS.Append("		<span style=\"color: white; font-size: 10px; font-weight: bold;\"><nobr>E-Mail:&nbsp;</nobr></span>\n");
			tmpS.Append("	</td>\n");
			tmpS.Append("	<td align=\"left\">\n");
			tmpS.Append("		<input name=\"Email\" type=\"text\" size=\"25\" maxlength=\"100\"><input name=\"Email_vldt\" type=\"hidden\" value=\"[req][email][blankalert=Please enter your e-mail address to sign in][invalidalert=Please enter a valid e-mail address]\">\n");
			tmpS.Append("	</td>\n");
			tmpS.Append("	<td width=\"15\"><img src=\"images/spacer.gif\" width=15\" height=\"1\"></td>\n");
			tmpS.Append("</tr>\n");
			tmpS.Append("<tr>\n");
			tmpS.Append("	<td colspan=\"3\"><img src=\"skins/Skin_%SKINID%/images/spacer.gif\" height=\"2\" width=\"1\"></td></tr>\n");
			tmpS.Append("<tr>\n");
			tmpS.Append("	<td width=\"100%\" align=\"right\">\n");
			tmpS.Append("		<span style=\"color: white; font-size: 10px; font-weight: bold;\"><nobr>Password:&nbsp;</nobr></span>\n");
			tmpS.Append("	</td>\n");
			tmpS.Append("	<td align=\"left\">\n");
			tmpS.Append("		<input name=\"Password\" type=\"password\" size=\"25\" maxlength=\"100\"><input name=\"Password_vldt\" type=\"hidden\" value=\"[req][blankalert=Please enter your password to sign in]\">\n");
			tmpS.Append("	</td>\n");
			tmpS.Append("	<td width=\"15\"><img src=\"images/spacer.gif\" width=15\" height=\"1\"></td>\n");
			tmpS.Append("</tr>\n");
			//tmpS.Append("<tr><td colspan=\"3\" height=2></td></tr>\n");
			tmpS.Append("<tr>\n");
			tmpS.Append("	<td width=\"100%\" align=\"right\" colspan=\"2\">\n");
			tmpS.Append("		<span style=\"color: white; font-size: 10px; font-weight: bold;\"><nobr>Remember Password:&nbsp;</nobr></span><input type=\"checkbox\" name=\"PersistLogin\" checked><input type=\"submit\" name=\"submit\" value=\"Login\">\n");
			tmpS.Append("	</td>\n");
			tmpS.Append("	<td width=\"15\"><img src=\"images/spacer.gif\" width=15\" height=\"1\"></td>\n");
			tmpS.Append("</tr>\n");
			tmpS.Append("<tr>\n");
			tmpS.Append("	<td colspan=\"2\" align=\"right\"><small><a class=\"DarkCellText\" href=\"signin.aspx\">Forgot Password?</a>&nbsp;&nbsp;&nbsp;&nbsp;<a class=\"DarkCellText\" href=\"createaccount.aspx\">Sign-up!</a></small></td>\n");
			tmpS.Append("	<td width=\"15\"><img src=\"images/spacer.gif\" width=15\" height=\"1\"></td>\n");
			tmpS.Append("</tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</form>\n");
			return tmpS.ToString();
		}

		static public String GetCategoryMenu(int ForParentCategoryID)
		{
			if(Common.IsAdminSite)
			{
				String CacheName = "GetCategoryMenu" + ForParentCategoryID.ToString() + "_Adm";
				bool CachingOn = Common.AppConfigBool("CacheMenus");
				if(CachingOn)
				{
					String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
					if(Menu != null)
					{
						if(Common.ApplicationBool("DumpSQL"))
						{
							HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
						}
						return Menu;
					}
				}

				StringBuilder tmpS = new StringBuilder(5000);
				String sql = String.Empty;
				if(ForParentCategoryID == 0)
				{
					sql = "select * from Category  " + DB.GetNoLock() + " where (parentCategoryid=0 or ParentCategoryID IS NULL) and deleted=0 order by DisplayOrder,Name";
				}
				else
				{
					sql = "select * from Category  " + DB.GetNoLock() + " where parentCategoryid=" + ForParentCategoryID.ToString() + " and deleted=0 order by DisplayOrder,Name";
				}
				IDataReader rs = DB.GetRS(sql);

				if(ForParentCategoryID == 0)
				{
					tmpS.Append("<li><a target=\"content\" href=\"categories.aspx\">" + Common.GetCategoryPromptPlural() + "</a>");
				}
				bool anyFound = false;
				while(rs.Read())
				{
					if(!anyFound)
					{
						tmpS.Append("<ul>\n");
						anyFound = true;
					}
					tmpS.Append("<li><a target=\"content\" href=\"editcategory.aspx?categoryid=" + DB.RSFieldInt(rs,"CategoryID").ToString() + "\">" + DB.RSField(rs,"Name") + "</a>");
					if(Common.CategoryHasSubs(DB.RSFieldInt(rs,"CategoryID")))
					{
						tmpS.Append(Common.GetCategoryMenu(DB.RSFieldInt(rs,"CategoryID")));
					}
					tmpS.Append("</li>\n");
				}
				if(anyFound)
				{
					tmpS.Append("</ul>\n");
				}
				rs.Close();
				if(ForParentCategoryID == 0)
				{
					tmpS.Append("</li>\n");
				}

				if(CachingOn)
				{
					HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
				}
				return tmpS.ToString();

			}
			else
			{
				String CacheName = "GetCategoryMenu" + ForParentCategoryID.ToString();
				bool CachingOn = Common.AppConfigBool("CacheMenus");
				if(CachingOn)
				{
					String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
					if(Menu != null)
					{
						if(Common.ApplicationBool("DumpSQL"))
						{
							HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
						}
						return Menu;
					}
				}

				StringBuilder tmpS = new StringBuilder(5000);
				String sql = String.Empty;
				if(ForParentCategoryID == 0)
				{
					sql = "select * from Category  " + DB.GetNoLock() + " where (parentCategoryid=0 or ParentCategoryID IS NULL) and published<>0 and deleted=0 order by DisplayOrder,Name";
				}
				else
				{
					sql = "select * from Category  " + DB.GetNoLock() + " where parentCategoryid=" + ForParentCategoryID.ToString() + " and published<>0 and deleted=0 order by DisplayOrder,Name";
				}
				IDataReader rs = DB.GetRS(sql);

				while(rs.Read())
				{
					if(tmpS.Length == 0)
					{
						if(ForParentCategoryID == 0)
						{
							tmpS.Append("<li>\n");
							tmpS.Append(Common.GetCategoryPromptPlural());
						}
						tmpS.Append("<ul>\n");
					}
					tmpS.Append("<li><a href=\"" + SE.MakeCategoryLink(DB.RSFieldInt(rs,"CategoryID"),DB.RSField(rs,"SEName")) + "\">" + DB.RSField(rs,"Name") + "</a>");
					// limit menu to 1 subcat..or it's just to damn slow for the JS menu
					if(ForParentCategoryID == 0 && Common.CategoryHasSubs(DB.RSFieldInt(rs,"CategoryID")))
					{
						tmpS.Append(Common.GetCategoryMenu(DB.RSFieldInt(rs,"CategoryID")));
					}
					tmpS.Append("</li>\n");
				}
				if(tmpS.Length != 0)
				{
					tmpS.Append("</ul>\n");
					if(ForParentCategoryID == 0)
					{
						tmpS.Append("</li>\n");
					}
				}
				rs.Close();

				if(CachingOn)
				{
					HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
				}
				return tmpS.ToString();
			}
		}

		static public String GetSectionMenu(int ForParentSectionID)
		{
			if(Common.IsAdminSite)
			{
				String CacheName = "GetSectionMenu" + ForParentSectionID.ToString() + "_Adm";
				bool CachingOn = Common.AppConfigBool("CacheMenus");
				if(CachingOn)
				{
					String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
					if(Menu != null)
					{
						if(Common.ApplicationBool("DumpSQL"))
						{
							HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
						}
						return Menu;
					}
				}

				StringBuilder tmpS = new StringBuilder(5000);
				String sql = String.Empty;
				if(ForParentSectionID == 0)
				{
					sql = "select * from [Section]  " + DB.GetNoLock() + " where (parentSectionid=0 or ParentSectionID IS NULL) and [published]<>0 and [deleted]=0 order by DisplayOrder,[Name]";
				}
				else
				{
					sql = "select * from [section]  " + DB.GetNoLock() + " where parentSectionid=" + ForParentSectionID.ToString() + " and published<>0 and deleted=0  order by DisplayOrder,Name";
				}
				IDataReader rs = DB.GetRS(sql);

				if(ForParentSectionID == 0)
				{
					tmpS.Append("<li><a target=\"content\" href=\"sections.aspx\">" + Common.AppConfig("SectionPromptPlural") + "</a>\n");
				}
				bool anyFound = false;
				while(rs.Read())
				{
					if(!anyFound)
					{
						tmpS.Append("<ul>\n");
						anyFound = true;
					}
					tmpS.Append("<li><a target=\"content\" href=\"editsection.aspx?sectionid=" + DB.RSFieldInt(rs,"SectionID").ToString() + "\">" + DB.RSField(rs,"Name") + "</a>");
					if(Common.SectionHasSubs(DB.RSFieldInt(rs,"SectionID")))
					{
						tmpS.Append(Common.GetSectionMenu(DB.RSFieldInt(rs,"SectionID")));
					}
					tmpS.Append("</li>\n");
				}
				if(anyFound)
				{
					tmpS.Append("</ul>\n");
				}
				rs.Close();
				if(ForParentSectionID == 0)
				{
					tmpS.Append("</li>\n");
				}

				if(CachingOn)
				{
					HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
				}
				return tmpS.ToString();

			}
			else
			{
				String CacheName = "GetSectionMenu" + ForParentSectionID.ToString();
				bool CachingOn = Common.AppConfigBool("CacheMenus");
				if(CachingOn)
				{
					String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
					if(Menu != null)
					{
						if(Common.ApplicationBool("DumpSQL"))
						{
							HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
						}
						return Menu;
					}
				}

				StringBuilder tmpS = new StringBuilder(5000);
				String sql = String.Empty;
				if(ForParentSectionID == 0)
				{
					sql = "select * from [Section]  " + DB.GetNoLock() + " where (parentSectionid=0 or ParentSectionID IS NULL) and published<>0 and deleted=0 order by DisplayOrder,Name";
				}
				else
				{
					sql = "select * from [Section]  " + DB.GetNoLock() + " where parentSectionid=" + ForParentSectionID.ToString() + " and published<>0 and deleted=0  order by DisplayOrder,Name";
				}
				IDataReader rs = DB.GetRS(sql);

				while(rs.Read())
				{
					if(tmpS.Length == 0)
					{
						if(ForParentSectionID == 0)
						{
							tmpS.Append("<li>\n");
							tmpS.Append(Common.AppConfig("SectionPromptPlural") + "\n");
						}
						tmpS.Append("<ul>\n");
					}
					tmpS.Append("<li><a href=\"" + SE.MakeSectionLink(DB.RSFieldInt(rs,"SectionID"),DB.RSField(rs,"SEName")) + "\">" + DB.RSField(rs,"Name") + "</a>");
					// limit menu to 1 subcat..or it's just to damn slow for the JS menu
					if(ForParentSectionID == 0 && Common.SectionHasSubs(DB.RSFieldInt(rs,"SectionID")))
					{
						tmpS.Append(Common.GetSectionMenu(DB.RSFieldInt(rs,"SectionID")));
					}
					tmpS.Append("</li>\n");
				}
				if(tmpS.Length != 0)
				{
					tmpS.Append("</ul>\n");
					if(ForParentSectionID == 0)
					{
						tmpS.Append("</li>\n");
					}
				}
				rs.Close();

				if(CachingOn)
				{
					HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
				}
				return tmpS.ToString();
			}
		}
	
		static public String GetManufacturerMenu()
		{
			if(Common.IsAdminSite)
			{
				String CacheName = "ManufacturerMenuAdm";
				bool CachingOn = Common.AppConfigBool("CacheMenus");
				if(CachingOn)
				{
					String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
					if(Menu != null)
					{
						if(Common.ApplicationBool("DumpSQL"))
						{
							HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
						}
						return Menu;
					}
				}

				StringBuilder tmpS = new StringBuilder(10000);
				DataSet ds = DB.GetDS("select * from manufacturer  " + DB.GetNoLock() + " where deleted=0 order by displayorder,name",false,System.DateTime.Now.AddDays(1));
				tmpS.Append("<li>\n");
				tmpS.Append("<a target=\"content\" href=\"manufacturers.aspx\">Manufacturers</a>\n");
				bool first = true;
				foreach(DataRow row in ds.Tables[0].Rows)
				{
					if(first)
					{
						tmpS.Append("<ul>\n");
					}
					tmpS.Append("<li><a target=\"content\" href=\"editmanufacturer.aspx?manufacturerid=" + DB.RowFieldInt(row,"ManufacturerID").ToString() + "\">" + DB.RowField(row,"Name") + "</a></li>\n");
					first = false;
				}
				ds.Dispose();
				if(!first)
				{
					tmpS.Append("</ul>\n");
				}
				tmpS.Append("</li>\n");

				if(CachingOn)
				{
					HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
				}

				return tmpS.ToString();
			}
			else
			{
				String CacheName = "ManufacturerMenu";
				bool CachingOn = Common.AppConfigBool("CacheMenus");
				if(CachingOn)
				{
					String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
					if(Menu != null)
					{
						if(Common.ApplicationBool("DumpSQL"))
						{
							HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
						}
						return Menu;
					}
				}

				StringBuilder tmpS = new StringBuilder(10000);
				IDataReader rs = DB.GetRS("select * from manufacturer  " + DB.GetNoLock() + " where deleted=0 order by displayorder,name");
				while(rs.Read())
				{
					if(tmpS.Length == 0)
					{
						tmpS.Append("<li>\n");
						tmpS.Append("<a href=\"manufacturers.aspx\">Manufacturers</a>\n");
						tmpS.Append("<ul>\n");
					}
					tmpS.Append("<li><a href=\"" + SE.MakeManufacturerLink(DB.RSFieldInt(rs,"ManufacturerID"),DB.RSField(rs,"SEName")) + "\">" + DB.RSField(rs,"Name") + "</a>");
					tmpS.Append("</li>\n");
				}
				if(tmpS.Length != 0)
				{
					tmpS.Append("</ul>\n");
					tmpS.Append("</li>\n");
				}
				rs.Close();

				if(CachingOn)
				{
					HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
				}

				return tmpS.ToString();
			}
		}
		

		static public String GetCategoryBox(int categoryID, bool subCatsOnly, int showNum, bool showPics, String teaser, int _siteID)
		{
			String CacheName = "CategoryBox" + categoryID.ToString();
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String ProductsMenu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(ProductsMenu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return ProductsMenu;
				}
			}

			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table width=\"171\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<a href=\"" + SE.MakeCategoryLink(categoryID,"") + "\"><img src=\"skins/Skin_" + _siteID.ToString() +  "/images/kits.gif\" border=\"0\"></a><br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"" + Common.IIF(showPics , "center" , "left") + "\" valign=\"top\">\n");

			tmpS.Append("<p align=\"" + Common.IIF(showPics , "center" , "left") + "\"><b>" + teaser + "</b></p>\n");
			DataSet rsf;
			if(subCatsOnly)
			{
				rsf = DB.GetDS("select * from category  " + DB.GetNoLock() + " where deleted=0 and published<>0 and parentcategoryid=" + categoryID.ToString() + " order by displayorder,name",false,System.DateTime.Now.AddDays(1));
				int N = rsf.Tables[0].Rows.Count;

				if(N != 0)
				{
					int i = 1;
					foreach(DataRow row in rsf.Tables[0].Rows)
					{
						if(i > showNum)
						{
							tmpS.Append("<img height=\"8\" src=\"skins/skin_" + _siteID.ToString() + "/images/redarrow.gif\">&nbsp;<a href=\"" + SE.MakeCategoryLink(categoryID,DB.RowField(row,"SEName")) + "\">more...</a>");
							break;
						}
						tmpS.Append("<img height=\"8\" src=\"skins/skin_" + _siteID.ToString() + "/images/redarrow.gif\">&nbsp;<a href=\"" + SE.MakeCategoryLink(DB.RowFieldInt(row,"CategoryID"),DB.RowField(row,"SEName")) + "\">");
						String ImgUrl = Common.LookupImage("Category",DB.RowFieldInt(row,"CategoryID"),"icon",_siteID);
						if(ImgUrl.Length == 0)
						{
							ImgUrl = Common.AppConfig("NoPicture");
						}
						if(ImgUrl.Length != 0)
						{
							int w = Common.GetImageWidth(ImgUrl);
							if(showPics)
							{
								tmpS.Append("<img src=\"" + ImgUrl + "\" width=\"" + Common.IIF(w >= 155 , 155 , w).ToString() + "\" border=\"0\"><br>");
							}
						}
						tmpS.Append(DB.RowField(row,"Name") + "</a>");
						tmpS.Append("<br>");
						if(showPics)
						{
							tmpS.Append("<br>");
						}
						i++;
					}
				}
				rsf.Dispose();
			}
			else
			{
				rsf = DB.GetDS("select p.*,DO.displayorder from product P  " + DB.GetNoLock() + " left outer join CategoryDisplayOrder DO  " + DB.GetNoLock() + " on p.productid=do.productid and do.categoryid=" + categoryID.ToString() + " where p.deleted=0 and p.published=1 and p.productid in (select distinct productid from productcategory  " + DB.GetNoLock() + " where categoryid=" + categoryID.ToString() + ") order by do.displayorder",false,System.DateTime.Now.AddDays(1));
				int N = rsf.Tables[0].Rows.Count;

				if(N != 0)
				{
					int i = 1;
					foreach(DataRow row in rsf.Tables[0].Rows)
					{
						if(i > showNum)
						{
							tmpS.Append("<img height=\"8\" src=\"skins/skin_" + _siteID.ToString() + "/images/redarrow.gif\">&nbsp;<a href=\"" + SE.MakeCategoryLink(categoryID,"") + "\">more...</a>");
							break;
						}
						tmpS.Append("<img height=\"8\" src=\"skins/skin_" + _siteID.ToString() + "/images/redarrow.gif\">&nbsp;<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName")) + "\">");
						String ImgUrl = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",_siteID);
						if(ImgUrl.Length == 0)
						{
							ImgUrl = Common.AppConfig("NoPicture");
						}
						if(ImgUrl.Length != 0)
						{
							int w = Common.GetImageWidth(ImgUrl);
							if(showPics)
							{
								tmpS.Append("<img src=\"" + ImgUrl + "\" width=\"" + Common.IIF(w >= 155 , 155 , w).ToString() + "\" border=\"0\"><br>");
							}
						}
						tmpS.Append(DB.RowField(row,"Name") + "</a>");
						tmpS.Append("<br>");
						if(showPics)
						{
							tmpS.Append("<br>");
						}
						i++;
					}
				}
				rsf.Dispose();
			}

			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}
		
		static public String GetSpecialsBox(int categoryID, int showNum, bool showPics, String teaser, int SiteID)
		{
			String CacheName = "SpecialsBox" + categoryID.ToString();
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String ProductsMenu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(ProductsMenu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return ProductsMenu;
				}
			}

			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table width=\"171\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<img src=\"skins/Skin_" + SiteID.ToString() +  "/images/Specials.gif\" border=\"0\"><br>");
			tmpS.Append("<table width=\"100%\" bgcolor=\"#FFFFFF\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"" + Common.IIF(Common.AppConfigBool("ShowSpecialsPics") , "center" , "left") + "\" valign=\"top\">\n");

			tmpS.Append("<p align=\"" + Common.IIF(Common.AppConfigBool("ShowSpecialsPics") , "center" , "left") + "\"><b>" + teaser + "</b></p>\n");
			DataSet rsf = DB.GetDS("select * from product  " + DB.GetNoLock() + " where deleted=0 and published=1 and productid in (select distinct productid from productcategory  " + DB.GetNoLock() + " where categoryid=" + Common.AppConfig("IsFeaturedCategoryID") + ") order by name",false,System.DateTime.Now.AddDays(1));
			int N = rsf.Tables[0].Rows.Count;

			if(N != 0)
			{
				int i = 1;
				foreach(DataRow row in rsf.Tables[0].Rows)
				{
					if(i > showNum)
					{
						tmpS.Append("<tr><td " + Common.IIF(showPics , "colspan=\"2\"" , "") + "><hr size=1 class=\"LightCellText\"><a href=\"showcategory.aspx?categoryid=" + categoryID.ToString() + "&resetfilter=true\">more...</a></td></tr>");
						break;
					}
					tmpS.Append("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName")) + "\">");
					if(showPics)
					{
						String ImgUrl = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",SiteID);
						if(ImgUrl.Length == 0)
						{
							ImgUrl = Common.AppConfig("NoPicture");
						}
						if(ImgUrl.Length != 0)
						{
							int w = Common.GetImageWidth(ImgUrl);
							if(Common.AppConfigBool("ShowSpecialsPics"))
							{
								tmpS.Append("<img src=\"" + ImgUrl + "\" width=\"" + Common.IIF(w >= 155 , 155 , w).ToString() + "\" border=\"0\"><br>");
							}
						}
					}
					tmpS.Append("<b>" + DB.RowField(row,"Name") + "</b></a>");
					tmpS.Append("<br><br>");
					i++;
				}
			}
			rsf.Dispose();
			//if(Common.AppConfig("IsFeaturedCategoryTeaser").Length != 0)
			//{
			//	tmpS.Append("<br>" + Common.AppConfig("IsFeaturedCategoryTeaser"));
			//}

			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}
		
		static public String GetCategoryBrowseBox(int _siteID)
		{
			String CacheName = "GetCategoryBrowseBox" + _siteID.ToString();
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<img src=\"skins/Skin_" + _siteID.ToString() +  "/images/browsebycategory.gif\" border=\"0\"><br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			
			String sql = "select * from category  " + DB.GetNoLock() + " where (parentcategoryid=0 or ParentCategoryID IS NULL) and published<>0 and deleted=0 order by DisplayOrder,Name";
			IDataReader rs = DB.GetRS(sql);
			bool anyFound = false;
			while(rs.Read())
			{
				tmpS.Append("<img height=\"8\" src=\"skins/skin_" + _siteID.ToString() + "/images/redarrow.gif\">&nbsp;<a href=\"" + SE.MakeCategoryLink(DB.RSFieldInt(rs,"CategoryID"),DB.RSField(rs,"SEName")) + "\">" + DB.RSField(rs,"Name") + "</a><br>");
				anyFound = true;
			}
			rs.Close();
			
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return Common.IIF(anyFound , tmpS.ToString() , String.Empty);
		}


		static public String GetPollBox(int CustomerID, int SiteID, int PollID, bool large)
		{
			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<img src=\"skins/Skin_" + SiteID.ToString() +  "/images/" + Common.IIF(large , "poll.gif" , "todayspoll.gif") + "\" border=\"0\"><br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			
			if(PollID == 0)
			{
				// try to find an active poll this user hasn't voted on yet:
				String sql = "select PollID from poll  " + DB.GetNoLock() + " where pollid not in (select distinct pollid from pollvotingrecord  " + DB.GetNoLock() + " where customerid=" + CustomerID.ToString() + ") and published<>0 and ExpiresOn>=" + DB.DateQuote(Localization.ToNativeShortDateString(System.DateTime.Now)) + " and deleted=0";
				IDataReader rs = DB.GetRS(sql);
				if(rs.Read())
				{
					PollID = DB.RSFieldInt(rs,"PollID");
				}
				rs.Close();
			}

			if(PollID == 0) // now try to find ANY active poll, voted on or not!
			{
				String sql = "select PollID from poll  " + DB.GetNoLock() + " where published<>0 and ExpiresOn>=" + DB.DateQuote(Localization.ToNativeShortDateString(System.DateTime.Now)) + " and deleted=0";
				IDataReader rs = DB.GetRS(sql);
				if(rs.Read())
				{
					PollID = DB.RSFieldInt(rs,"PollID");
				}
				rs.Close();
			}
			bool anyFound = false;
			if(PollID != 0)
			{
				anyFound = true;
				Poll p = new Poll(PollID, SiteID);
				tmpS.Append(p.Display(CustomerID,!large));
				p = null;
			}
			
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			return Common.IIF(anyFound , tmpS.ToString() , String.Empty);
		}

		static public String GetSectionBrowseBox(int _siteID)
		{
			String CacheName = "GetSectionBrowseBox_" + _siteID.ToString();
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<img src=\"skins/Skin_" + _siteID.ToString() +  "/images/browsebydept.gif\" border=\"0\"><br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			
			String sql = "select * from [section]  " + DB.GetNoLock() + " where (ParentSectionID=0 or ParentSectionID IS NULL) and published<>0 and deleted=0 order by DisplayOrder,Name";
			IDataReader rs = DB.GetRS(sql);
			bool anyFound = false;
			while(rs.Read())
			{
				tmpS.Append("<img height=\"8\" src=\"skins/skin_" + _siteID.ToString() + "/images/redarrow.gif\">&nbsp;<a href=\"" + SE.MakeSectionLink(DB.RSFieldInt(rs,"SectionID"),DB.RSField(rs,"SEName")) + "\">" + DB.RSField(rs,"Name") + "</a><br>");
				anyFound = true;
			}
			rs.Close();
			
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return Common.IIF(anyFound , tmpS.ToString() , String.Empty);
		}

		static public String GetHelpBox(int SiteID, bool includeFrame)
		{
			String CacheName = "GetHelpBox";
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(10000);

			if(includeFrame)
			{
				tmpS.Append("<table cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
				tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
				tmpS.Append("<img src=\"skins/Skin_" + SiteID.ToString() +  "/images/help.gif\" border=\"0\"><br>");
				tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
				tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			}
			
			Topic t1 = new Topic("helpbox",SiteID);
			tmpS.Append(t1._contents);

			if(includeFrame)
			{
				tmpS.Append("</td></tr>\n");
				tmpS.Append("</table>\n");
				tmpS.Append("</td></tr>\n");
				tmpS.Append("</table>\n");
			}

			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public String GetWelcomeBox(int SiteID)
		{
			String CacheName = "GetWelcomeBox";
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<img src=\"skins/Skin_" + SiteID.ToString() +  "/images/welcome.gif\" border=\"0\"><br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			
			tmpS.Append(Common.ExtractBody(Common.ReadFile("skins/Skin_" + SiteID.ToString() +  "/welcome.htm",true)));
			tmpS.Append(Common.ExtractBody(Common.ReadFile("skins/Skin_" + SiteID.ToString() +  "/homebottomteaser.htm",true)));
			
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public String GetTechTalkBox(int SiteID)
		{
			String CacheName = "GetTechTalkBox";
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table width=\"171\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<a href=\"techtalk.aspx\"><img src=\"skins/Skin_" + SiteID.ToString() +  "/images/learn.gif\" border=\"0\"></a><br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			
			tmpS.Append("--coming soon--");
			
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public String GetTechTalkBoxExpanded(int SiteID)
		{
			String CacheName = "GetTechTalkBoxExpanded";
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}


			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<a href=\"techtalk.aspx\"><img src=\"skins/Skin_" + SiteID.ToString() +  "/images/learnexpanded.gif\" border=\"0\"></a><br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td width=\"100%\" align=\"left\" valign=\"top\">\n");
			
			tmpS.Append("--coming soon--");

			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public String GetManufacturersBoxExpanded(int SiteID)
		{
			String CacheName = "GetManufacturersBoxExpanded";
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<a href=\"manufacturers.aspx\"><img src=\"skins/Skin_" + SiteID.ToString() +  "/images/manufacturersbox.gif\" border=\"0\"></a><br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			
			tmpS.Append("<p><b>We feature products from the following manufacturers:</b></p>\n");

			DataSet ds = DB.GetDS("select * from manufacturer  " + DB.GetNoLock() + " where deleted=0 order by displayorder,name",false,System.DateTime.Now.AddDays(1));

			foreach(DataRow row in ds.Tables[0].Rows)
			{
				tmpS.Append("<b class=\"a4\"><a href=\"" + SE.MakeManufacturerLink(DB.RowFieldInt(row,"ManufacturerID"),DB.RowField(row,"SEName")) + "\"><font style=\"font-size: 14px;\">" + DB.RowField(row,"Name"));
				if(DB.RowField(row,"Summary").Length != 0)
				{
					tmpS.Append(": " + DB.RowField(row,"Summary"));
				}
				tmpS.Append("</font></a></b><br>\n");
				if(DB.RowField(row,"Description").Length != 0)
				{
					tmpS.Append("<span class=\"a2\">" + DB.RowField(row,"Description") + "</span><br>\n");
				}
				tmpS.Append("<div class=\"a1\" style=\"PADDING-BOTTOM: 10px\">\n");
				if(DB.RowField(row,"URL").Length != 0)
				{
					tmpS.Append("<a href=\"" + DB.RowField(row,"URL") + "\" target=\"_blank\">" + DB.RowField(row,"URL") + "</a>");
				}
				tmpS.Append("</div><br>\n");
			}
			ds.Dispose();
			
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public String GetCategoryBoxExpanded(int categoryID, int showNum, bool showPics, String teaser, int SiteID)
		{
			String CacheName = "GetCategoryBoxExpanded" + categoryID.ToString();
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<a href=\"" + SE.MakeCategoryLink(categoryID,"") + "\"><img src=\"skins/Skin_" + SiteID.ToString() +  "/images/kitsexpanded.gif\" border=\"0\"></a><br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			
			tmpS.Append("<p><b>" + teaser + "</b></p>\n");

			DataSet ds = DB.GetDS("select p.*,DO.DisplayOrder from product  " + DB.GetNoLock() + " left outer join CategoryDisplayOrder do on p.productid=do.productid and do.categoryid=" + categoryID.ToString() + " and p.deleted=0 and p.published=1 and p.productid in (select distinct productid from productcategory  " + DB.GetNoLock() + " where categoryid=" + categoryID.ToString() + ") order by do.displayorder",false,System.DateTime.Now.AddDays(1));

			tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\">\n");
			int i = 1;
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				if(i > showNum)
				{
					tmpS.Append("<tr><td " + Common.IIF(showPics , "colspan=\"2\"" , "") + "><hr size=1 class=\"LightCellText\"><a href=\"" + SE.MakeCategoryLink(categoryID,"") + "\">more...</a></td></tr>");
					break;
				}
				if(i > 1)
				{
					tmpS.Append("<tr><td " + Common.IIF(showPics , "colspan=\"2\"" , "") + "><hr size=1 class=\"LightCellText\"></td></tr>");
				}
				tmpS.Append("<tr>");
				String ImgUrl = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",SiteID);
				if(ImgUrl.Length == 0)
				{
					ImgUrl = Common.AppConfig("NoPicture");
				}
				if(showPics)
				{
					tmpS.Append("<td align=\"left\" valign=\"top\">\n");
					int w = Common.GetImageWidth(ImgUrl);
					int h = Common.GetImageHeight(ImgUrl);
					tmpS.Append("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName")) + "\">");
					if(w > 100)
					{
						w = 100;
					}
					if(h > 100)
					{
						h = 100;
					}
					tmpS.Append("<img align=\"left\" src=\"" + ImgUrl + "\" height=\"" + h.ToString() + "\" width=\"" + w.ToString() + "\" border=\"0\">");
					tmpS.Append("</a>");
					tmpS.Append("</td>");
				}

				tmpS.Append("<td align=\"left\" valign=\"top\">\n");
				tmpS.Append("<b class=\"a4\">");
				tmpS.Append("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName")) + "\">" + DB.RowField(row,"Name"));
				if(DB.RowField(row,"Summary").Length != 0)
				{
					tmpS.Append(": " + DB.RowField(row,"Summary"));
				}
				tmpS.Append("</a></b><br>\n");
				if(DB.RowField(row,"Description").Length != 0)
				{
					tmpS.Append("<span class=\"a2\">" + DB.RowField(row,"Description") + "</span><br>\n");
				}
				tmpS.Append("<div class=\"a1\" style=\"PADDING-BOTTOM: 10px\">\n");
				tmpS.Append("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName")) + "\">");
				tmpS.Append("Click here for more information...");
				tmpS.Append("</a>");
				tmpS.Append("</div>\n");
				tmpS.Append("</td>");
				tmpS.Append("</tr>");
				i++;
			}
			tmpS.Append("</table>\n");
			ds.Dispose();
			
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public String GetRequestQuoteBoxExpanded(bool useCache, int SiteID)
		{
			String CacheName = "GetRequestQuoteBoxExpanded";
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn && useCache)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<img src=\"skins/Skin_" + SiteID.ToString() +  "/images/requestquoteexpanded.gif\" border=\"0\"><br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			
			tmpS.Append("<script Language=\"JavaScript\">\n");
			tmpS.Append("function CustomBidForm_Validator(theForm)\n");
			tmpS.Append("{\n");
			tmpS.Append("	submitonce(theForm);\n");
			tmpS.Append("	if (theForm.Summary.value.length == 0)\n");
			tmpS.Append("	{\n");
			tmpS.Append("		alert(\"Please enter a short summary of your bid requirements.\");\n");
			tmpS.Append("		theForm.Summary.focus();\n");
			tmpS.Append("		submitenabled(theForm);\n");
			tmpS.Append("		return (false);\n");
			tmpS.Append("    }\n");
			tmpS.Append("  return (true);\n");
			tmpS.Append("}\n");
			tmpS.Append("</script>\n");
			
			// CUSTOM QUOTE FORM:
			tmpS.Append("<div align=\"left\">\n");
			tmpS.Append("<form method=\"POST\" action=\"custombid.aspx\" onsubmit=\"return (validateForm(this) && CustomBidForm_Validator(this))\" id=\"CustomBidForm\" name=\"CustomBidForm\">\n");
			//tmpS.Append("<b>FOR A CUSTOM QUOTE:</b><br>\n");
			tmpS.Append("<b>We will not be undersold!</b> For a custom quote, please send us a description of your requirements, or <a href=\"" + SE.MakeDriverLink("contact") + "\">contact us by phone</a>. We'll be happy to prepare a custom bid tailored to meet your exact specifications.<br><br>\n");
			tmpS.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\n");
			tmpS.Append("<tr>\n");
			tmpS.Append("<td align=\"left\" valign=\"top\">\n");
			tmpS.Append("Summary of Need:<br><textarea rows=\"20\" name=\"Summary\" cols=\"50\"></textarea>\n");
			//tmpS.Append("<input type=\"hidden\" name=\"Summary_vldt\" value=\"[req][blankalert=Please enter a short summary of your bid requirements]\">\n");
			tmpS.Append("</td>\n");
			tmpS.Append("<td width=\"20\"></td>\n");
			tmpS.Append("<td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<br>Your Name:<br>\n");
			tmpS.Append("<input type=\"text\" name=\"Name\" size=\"35\">\n");
			tmpS.Append("<input type=\"hidden\" name=\"Name_vldt\" value=\"[req][blankalert=Please enter your name so we may contact you if we have questions]\">\n");
			tmpS.Append("<br><br>Organization:<br>\n");
			tmpS.Append("<input type=\"text\" name=\"Organization\" size=\"35\">\n");
			tmpS.Append("<input type=\"hidden\" name=\"Organization_vldt\" value=\"[req][blankalert=Please enter your organization so we can properly respond to your bid request]\">\n");
			tmpS.Append("<br><br>Your E-Mail:<br>\n");
			tmpS.Append("<input type=\"text\" name=\"EMail\" size=\"35\">\n");
			tmpS.Append("<input type=\"hidden\" name=\"EMail_vldt\" value=\"[req][email][blankalert=Please enter your e-mail address so we may contact you if we have questions][invalidalert=Please enter a valid e-mail address so we may contact you if we have questions]\">\n");
			tmpS.Append("<br><br>Your Phone:<br>\n");
			tmpS.Append("<input type=\"text\" name=\"Phone\" size=\"35\">\n");
			tmpS.Append("<input type=\"hidden\" name=\"Phone_vldt\" value=\"[req][blankalert=Please enter a valid phone number so we may contact you if we have questions]\">\n");
			tmpS.Append("<br><br><input type=\"submit\" value=\"Submit\" name=\"B1\">\n");
			tmpS.Append("</td>\n");
			tmpS.Append("</tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</form>\n");
			tmpS.Append("</div>\n");

			
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			if(CachingOn && useCache)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public String GetSpecialsBoxExpanded(int categoryID, int showNum, bool useCache, bool showPics, String teaser, int SiteID)
		{
			String CacheName = "GetSpecialsBoxExpanded" + categoryID.ToString();
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn && useCache)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<a href=\"showcategory.aspx?categoryid=" + categoryID.ToString() + "&resetfilter=true\"><img src=\"skins/Skin_" + SiteID.ToString() +  "/images/Specialsexpanded.gif\" border=\"0\"></a><br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			
			tmpS.Append("<p><b>" + teaser + "</b></p>\n");

			DataSet ds = DB.GetDS("select * from product  " + DB.GetNoLock() + " where deleted=0 and published=1 and productid in (select distinct productid from productcategory  " + DB.GetNoLock() + " where categoryid=" + Common.AppConfig("IsFeaturedCategoryID") + ") order by name",false,System.DateTime.Now.AddDays(1));

			tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\">\n");
			int i = 1;
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				if(i > showNum)
				{
					tmpS.Append("<tr><td " + Common.IIF(showPics , "colspan=\"2\"" , "") + "><hr size=1 class=\"LightCellText\"><a href=\"showcategory.aspx?categoryid=" + categoryID.ToString() + "&resetfilter=true\">more...</a></td></tr>");
					break;
				}
				if(i > 1)
				{
					tmpS.Append("<tr><td " + Common.IIF(showPics , "colspan=\"2\"" , "") + "><hr size=1 class=\"LightCellText\"></td></tr>");
				}
				tmpS.Append("<tr>");
				String ImgUrl = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",SiteID);
				if(ImgUrl.Length == 0)
				{
					ImgUrl = Common.AppConfig("NoPicture");
				}
				if(showPics)
				{
					tmpS.Append("<td align=\"left\" valign=\"top\">\n");
					int w = Common.GetImageWidth(ImgUrl);
					int h = Common.GetImageHeight(ImgUrl);
					tmpS.Append("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName")) + "\">");
					//					if(w > 100)
					//					{
					//						w = 100;
					//					}
					//					if(h > 100)
					//					{
					//						h = 100;
					//					}
					w = 100;
					tmpS.Append("<img align=\"left\" src=\"" + ImgUrl + "\" width=\"" + w.ToString() + "\" border=\"0\">");
					tmpS.Append("</a>");
					tmpS.Append("</td>");
				}

				tmpS.Append("<td align=\"left\" valign=\"top\">\n");
				tmpS.Append("<b class=\"a4\">");
				tmpS.Append("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName")) + "\">" + DB.RowField(row,"Name"));
				//if(Common.AppConfig("IsFeaturedCategoryTeaser").Length != 0)
				//{
				//	tmpS.Append(": " + Common.AppConfig("IsFeaturedCategoryTeaser"));
				//}
				tmpS.Append("</a>");
				String Prices = Common.GetProductSalePrice(DB.RowFieldInt(row,"ProductID"));
				String[] pr = Prices.Split('|');
				String RegPrice = String.Empty;
				String SalePrice = String.Empty;
				try
				{
					RegPrice = pr[0];
				}
				catch {}
				try
				{
					SalePrice = pr[1];
				}
				catch {}
				if(SalePrice.Length != 0 && SalePrice != "$0.00")
				{
					tmpS.Append("<br><strike>Regular Price: " + RegPrice + "</strike><br>On Sale For: " + SalePrice);
				}
				if(DB.RowField(row,"Summary").Length != 0)
				{
					tmpS.Append("<br>" + DB.RowField(row,"Summary"));
				}
				tmpS.Append("</b><br>\n");
				if(DB.RowField(row,"Description").Length != 0)
				{
					tmpS.Append("<span class=\"a2\">" + DB.RowField(row,"Description") + "</span><br>\n");
				}
				tmpS.Append("<div class=\"a1\" style=\"PADDING-BOTTOM: 10px\">\n");
				tmpS.Append("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName")) + "\">");
				tmpS.Append("Click here for more information...");
				tmpS.Append("</a>");
				tmpS.Append("</div>\n");
				tmpS.Append("</td>");
				tmpS.Append("</tr>");
				i++;
			}
			tmpS.Append("</table>\n");
			ds.Dispose();
			
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			if(CachingOn && useCache)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public String GetSpecialsBoxExpandedRandom(int categoryID, bool showPics, String teaser, int SiteID)
		{
			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<a href=\"showcategory.aspx?categoryid=" + categoryID.ToString() + "&resetfilter=true\"><img src=\"skins/Skin_" + SiteID.ToString() +  "/images/Specialsexpanded.gif\" border=\"0\"></a><br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			
			tmpS.Append("<p><b>" + teaser + "</b></p>\n");

			String sql = "select * from product  " + DB.GetNoLock() + " where deleted=0 and published=1 and productid in (select distinct productid from productcategory  " + DB.GetNoLock() + " where categoryid=" + Common.AppConfig("IsFeaturedCategoryID") + ") order by looks";
			DataSet ds = DB.GetDS(sql,false,System.DateTime.Now.AddDays(1));
			int NumRecs = ds.Tables[0].Rows.Count;
			int ShowRecNum = Common.GetRandomNumber(1,NumRecs);
			//tmpS.Append("Sql=" + sql + "<br>");
			//tmpS.Append("NumRecs=" + NumRecs + ", ShowRecNum=" + ShowRecNum.ToString() + "<br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\">\n");
			int i = 1;
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				if(i == ShowRecNum)
				{
					tmpS.Append("<tr>");
					String ImgUrl = Common.LookupImage("Product",DB.RowFieldInt(row,"ProductID"),"icon",SiteID);
					if(ImgUrl.Length == 0)
					{
						ImgUrl = Common.AppConfig("NoPicture");
					}
					if(showPics)
					{
						tmpS.Append("<td align=\"left\" valign=\"top\">\n");
						int w = Common.GetImageWidth(ImgUrl);
						int h = Common.GetImageHeight(ImgUrl);
						tmpS.Append("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName")) + "\">");
						//					if(w > 100)
						//					{
						//						w = 100;
						//					}
						//					if(h > 100)
						//					{
						//						h = 100;
						//					}
						w = 100;
						tmpS.Append("<img align=\"left\" src=\"" + ImgUrl + "\" width=\"" + w.ToString() + "\" border=\"0\">");
						tmpS.Append("</a>");
						tmpS.Append("</td>");
					}

					tmpS.Append("<td align=\"left\" valign=\"top\">\n");
					tmpS.Append("<b class=\"a4\">");
					tmpS.Append("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName")) + "\">" + DB.RowField(row,"Name"));
					//if(Common.AppConfig("IsFeaturedCategoryTeaser").Length != 0)
					//{
					//	tmpS.Append(": " + Common.AppConfig("IsFeaturedCategoryTeaser"));
					//}
					tmpS.Append("</a>");
					String Prices = Common.GetProductSalePrice(DB.RowFieldInt(row,"ProductID"));
					String[] pr = Prices.Split('}');
					String RegPrice = String.Empty;
					String SalePrice = String.Empty;
					try
					{
						RegPrice = pr[0];
					}
					catch {}
					try
					{
						SalePrice = pr[1];
					}
					catch {}
					if(SalePrice.Length != 0 && SalePrice != "$0.00")
					{
						tmpS.Append("<br><strike>Regular Price: " + RegPrice + "</strike><br>On Sale For: " + SalePrice);
					}
					if(DB.RowField(row,"Summary").Length != 0)
					{
						tmpS.Append("<br>" + DB.RowField(row,"Summary"));
					}
					tmpS.Append("</b><br>\n");
					if(DB.RowField(row,"Description").Length != 0)
					{
						tmpS.Append("<span class=\"a2\">" + DB.RowField(row,"Description") + "</span><br>\n");
					}
					tmpS.Append("<div class=\"a1\" style=\"PADDING-BOTTOM: 10px\">\n");
					tmpS.Append("<a href=\"" + SE.MakeProductLink(DB.RowFieldInt(row,"ProductID"),DB.RowField(row,"SEName")) + "\">");
					tmpS.Append("Click here for more information...");
					tmpS.Append("</a>");
					tmpS.Append("</div>\n");
					tmpS.Append("</td>");
					tmpS.Append("</tr>");
				}
				i++;
			}
			tmpS.Append("<tr><td " + Common.IIF(showPics , "colspan=\"2\"" , "") + "><hr size=1 class=\"LightCellText\"><a href=\"showcategory.aspx?categoryid=" + categoryID.ToString() + "&resetfilter=true\">Show me more specials...</a></td></tr>");
			tmpS.Append("</table>\n");
			ds.Dispose();
			
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			return tmpS.ToString();
		}

		static public String GetStaffBoxExpanded(bool useCache, int showNum, bool showPics, String teaser, int SiteID)
		{
			String CacheName = "GetStaffBoxExpanded" + showNum.ToString();
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn && useCache)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<a href=\"staff.aspx\"><img src=\"skins/Skin_" + SiteID.ToString() +  "/images/staffexpanded.gif\" border=\"0\"></a><br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			
			tmpS.Append("<p><b>" + teaser + "</b></p>\n");

			DataSet ds = DB.GetDS("select * from staff  " + DB.GetNoLock() + " where deleted=0 and published=1 order by displayorder,name",false,System.DateTime.Now.AddDays(1));

			tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\">\n");
			int i = 1;
			tmpS.Append("<table border=\"0\" cellpadding=\"6\" width=\"100%\">\n");
			tmpS.Append("<tr>\n");
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				if(i <= showNum)
				{
					tmpS.Append("<td valign=\"middle\" align=\"center\">\n");
					String ImgUrl = Common.LookupImage("Staff",DB.RowFieldInt(row,"StaffID"),"icon",SiteID);
					if(ImgUrl.Length == 0)
					{
						ImgUrl = Common.AppConfig("NoPicture");
					}
					if(showPics)
					{
						int w = Common.GetImageWidth(ImgUrl);
						int h = Common.GetImageHeight(ImgUrl);
						tmpS.Append("<a href=\"staff.aspx?staffid=" + DB.RowFieldInt(row,"StaffID").ToString() + "\">");
						if(w > 100)
						{
							w = 100;
						}
						if(h > 100)
						{
							h = 100;
						}
						tmpS.Append("<img alt=\"" + DB.RowField(row,"Name") + "\" src=\"" + ImgUrl + "\" width=\"" + w.ToString() + "\" border=\"0\">");
						tmpS.Append("</a>");
						tmpS.Append("<br>");
					}
					tmpS.Append("<a href=\"staff.aspx?staffid=" + DB.RowFieldInt(row,"StaffID").ToString() + "\">");
					tmpS.Append("<b>" + DB.RowField(row,"Name") + "</b>");
					tmpS.Append("<br>");
					tmpS.Append("</a>");
					tmpS.Append("<small>" + DB.RowField(row,"Title") + "</small>");
					tmpS.Append("</td>\n");
				}
				i++;
			}
			tmpS.Append("</tr>\n");
			tmpS.Append("</table>\n");
			ds.Dispose();
			
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			if(CachingOn && useCache)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public String GetManufacturersBox(int SiteID)
		{
			String CacheName = "GetManufacturersBox";
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<a href=\"manufacturers.aspx\"><img src=\"skins/Skin_" + SiteID.ToString() +  "/images/manufacturers.gif\" border=\"0\"></a><br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			
			//tmpS.Append("<center><b>FEATURING PRODUCTS FROM:</b></center><br>\n");

			tmpS.Append("<script language=\"JavaScript1.1\">\n");
			tmpS.Append("\n");
			tmpS.Append("//*****************************************\n");
			tmpS.Append("// Blending Image Slide Show Script- \n");
			tmpS.Append("// © Dynamic Drive (www.dynamicdrive.com)\n");
			tmpS.Append("// For full source code, visit http://www.dynamicdrive.com/\n");
			tmpS.Append("//*****************************************\n");
			tmpS.Append("\n");
			tmpS.Append("//specify interval between slide (in mili seconds)\n");
			tmpS.Append("var slidespeed=3000\n");
			tmpS.Append("\n");
			tmpS.Append("//specify images\n");
			String SlideImages = String.Empty;
			String SlideLinks = String.Empty;
			IDataReader rs = DB.GetRS("Select * from manufacturer  " + DB.GetNoLock() + " where deleted=0 order by displayorder,name");
			while(rs.Read())
			{
				String MfgPic = Common.LookupImage("Manufacturer",DB.RSFieldInt(rs,"ManufacturerID"),"",SiteID);
				if(MfgPic.Length != 0)
				{
					if(SlideImages.Length != 0)
					{
						SlideImages += ",";
					}
					if(SlideLinks.Length != 0)
					{
						SlideLinks += ",";
					}
					SlideImages += "'" + MfgPic + "'";
					SlideLinks += DB.SQuote(DB.RSField(rs,"Url"));
				}
			}
			rs.Close();
			tmpS.Append("var slideimages=new Array(" + SlideImages + ");");
			tmpS.Append("var slidelinks=new Array(" + SlideLinks + ");");

			//tmpS.Append("\"images/manufacturer/18.gif\",\"images/manufacturer/19.gif\",\"images/manufacturer/20.gif\",\"images/manufacturer/26.gif\",\"images/manufacturer/27.gif\",\"images/manufacturer/28.gif\",\"images/manufacturer/29.gif\",\"images/manufacturer/30.gif\",\"images/manufacturer/9.jpg\",\"images/manufacturer/17.jpg\",\"images/manufacturer/21.jpg\",\"images/manufacturer/23.jpg\",\"images/manufacturer/25.jpg\",\"images/manufacturer/31.png\")\n");
			//tmpS.Append("var slidelinks=new Array(\"url1\",\"url2\",\"url1\",\"url2\",\"url1\",\"url2\",\"url1\",\"url2\",\"url1\",\"url2\",\"url1\",\"url2\",\"url1\",\"url2\")\n");
			tmpS.Append("\n");
			tmpS.Append("var newwindow=1 //open links in new window? 1=yes, 0=no\n");
			tmpS.Append("\n");
			tmpS.Append("var imageholder=new Array()\n");
			tmpS.Append("var ie=document.all\n");
			tmpS.Append("for (i=0;i<slideimages.length;i++)\n");
			tmpS.Append("{\n");
			tmpS.Append("imageholder[i]=new Image()\n");
			tmpS.Append("imageholder[i].src=slideimages[i]\n");
			tmpS.Append("}\n");
			tmpS.Append("\n");
			tmpS.Append("function gotoshow()\n");
			tmpS.Append("{\n");
			if(Common.AppConfigBool("ManufacturersLinkToOurPage"))
			{
				tmpS.Append("window.location='manufacturers.aspx'\n");
			}
			else
			{
				tmpS.Append("if (newwindow)\n");
				tmpS.Append("window.open(slidelinks[whichlink])\n");
				tmpS.Append("else\n");
				tmpS.Append("window.location=slidelinks[whichlink]\n");
			}
			tmpS.Append("}\n");
			tmpS.Append("\n");
			tmpS.Append("</script>\n");

			tmpS.Append("<center>\n");
			tmpS.Append("<a href=\"javascript:gotoshow()\"><img src=\"image1.gif\" name=\"slide\" border=0 style=\"filter:blendTrans(duration=3)\" width=165 height=100></a>\n");
			tmpS.Append("</center>\n");
			tmpS.Append("\n");
			tmpS.Append("<script language=\"JavaScript1.1\">\n");
			tmpS.Append("<!--\n");
			tmpS.Append("\n");
			tmpS.Append("var whichlink=0\n");
			tmpS.Append("var whichimage=0\n");
			tmpS.Append("var blenddelay=(ie)? document.images.slide.filters[0].duration*1000 : 0\n");
			tmpS.Append("function slideit()\n");
			tmpS.Append("{\n");
			tmpS.Append("if (!document.images) return\n");
			tmpS.Append("if (ie) document.images.slide.filters[0].apply()\n");
			tmpS.Append("document.images.slide.src=imageholder[whichimage].src\n");
			tmpS.Append("if (ie) document.images.slide.filters[0].play()\n");
			tmpS.Append("whichlink=whichimage\n");
			tmpS.Append("whichimage=(whichimage<slideimages.length-1)? whichimage+1 : 0\n");
			tmpS.Append("setTimeout(\"slideit()\",slidespeed+blenddelay)\n");
			tmpS.Append("}\n");
			tmpS.Append("slideit()\n");
			tmpS.Append("\n");
			tmpS.Append("//-->\n");
			tmpS.Append("</script>\n");
			
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public String GetTrainingPartnersBox(int _siteID)
		{
			String CacheName = "GetTrainingPartnersBox";
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<a href=\"partners.aspx\"><img src=\"skins/Skin_" + _siteID.ToString() +  "/images/training.gif\" border=\"0\"></a><br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<font style=\"font-size: 9px;\">Our preferred training partners provide comprehensive training programs.</font><br><br>");
			IDataReader rs = DB.GetRS("select * from partner  " + DB.GetNoLock() + " where deleted=0 and published=1 order by displayorder,name");
			while(rs.Read())
			{
				tmpS.Append("<img height=\"8\" src=\"skins/skin_" + _siteID.ToString() + "/images/redarrow.gif\">&nbsp;<a href=\"partners.aspx?partnerid=" + DB.RSFieldInt(rs,"PartnerID").ToString() + "\">" + DB.RSField(rs,"Name") + "</a><br>");
			}
			rs.Close();
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public String GetSearchBox(int SiteID)
		{
			String CacheName = "GetSearchBox";
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<img src=\"skins/Skin_" + SiteID.ToString() +  "/images/search.gif\" border=\"0\"><br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");

			tmpS.Append("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			tmpS.Append("function SearchBoxForm_Validator(theForm)\n");
			tmpS.Append("{\n");
			tmpS.Append("	submitonce(theForm);\n");
			tmpS.Append("  if (theForm.SearchTerm.value.length < " + Common.AppConfig("MinSearchStringLength") + ")\n");
			tmpS.Append("  {\n");
			tmpS.Append("    alert('Please enter at least " + Common.AppConfig("MinSearchStringLength") + " characters in the Search For field.');\n");
			tmpS.Append("    theForm.SearchTerm.focus();\n");
			tmpS.Append("	submitenabled(theForm);\n");
			tmpS.Append("    return (false);\n");
			tmpS.Append("  }\n");
			tmpS.Append("  return (true);\n");
			tmpS.Append("}\n");
			tmpS.Append("</script>\n");
			
			tmpS.Append("<form name=\"SearchBoxForm\" action=\"searchadv.aspx\" method=\"GET\" onsubmit=\"return SearchBoxForm_Validator(this)\">\n");
			tmpS.Append("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">Search: <input name=\"SearchTerm\" size=\"10\"><img src=\"images/spacer.gif\" width=\"4\" height=\"4\"><INPUT NAME=\"submit\" TYPE=\"Image\" ALIGN=\"absmiddle\" src=\"skins/Skin_" + SiteID.ToString() +  "/images/go.gif\" border=0>\n");
			tmpS.Append("</form>");
			//tmpS.Append("<div align=\"center\"><a href=\"requestquote.aspx\">Request a custom quote!</a></div>");
			//tmpS.Append("<div align=\"center\"><a href=\"requestcatalog.aspx\">Request a free catalog</a><br>&nbsp;</div>");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}


		static public int GetProductTypeID(int ProductID)
		{
			int tmp = 0;
			IDataReader rs = DB.GetRS("select producttypeid from product  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString());
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"ProductTypeID");
			}
			rs.Close();
			return tmp;
		}

		static public int GetNextAppConfig(String appConfigName)
		{
			int tmp = 0;
			IDataReader rs = DB.GetRS("select * from AppConfig  " + DB.GetNoLock() + " where lower(name)>" + DB.SQuote(appConfigName.ToLower()) + " order by name");
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"AppConfigID");
			}
			rs.Close();
			if(tmp == 0)
			{
				rs = DB.GetRS("select * from AppConfig  " + DB.GetNoLock() + " order by name");
				if(rs.Read())
				{
					tmp = DB.RSFieldInt(rs,"AppConfigID");
				}
				rs.Close();
			}
			return tmp;
		}

		static public int GetPreviousAppConfig(String appConfigName)
		{
			int tmp = 0;
			IDataReader rs = DB.GetRS("select * from AppConfig  " + DB.GetNoLock() + " where lower(name)<" + DB.SQuote(appConfigName.ToLower()) + " order by name desc");
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"AppConfigID");
			}
			rs.Close();
			if(tmp == 0)
			{
				rs = DB.GetRS("select * from AppConfig  " + DB.GetNoLock() + " order by name desc");
				if(rs.Read())
				{
					tmp = DB.RSFieldInt(rs,"AppConfigID");
				}
				rs.Close();
			}
			return tmp;
		}

		public static String GetAppConfigName(int AppConfigID)
		{
			String tmp = String.Empty;
			IDataReader rs = DB.GetRS("Select name from appconfig where appconfigid=" + AppConfigID.ToString());
			if(rs.Read())
			{
				tmp = DB.RSField(rs,"name");
			}
			rs.Close();
			return tmp;
		}
				  
		// returns the "previous" category, after the specified category
		// "previous" is defined as either the category that is next lower display order, or same display order and next lowest alphabetical order
		// is circular also (i.e. if first, return last)
		public static int GetPreviousCategory(int CategoryID)
		{
			int PID = Common.GetCategoryParentID(CategoryID);
			String sql = "SELECT C.CategoryID, C.Name AS CategoryName FROM Category C  " + DB.GetNoLock() + " where deleted=0 " + Common.IIF(PID == 0 , " and (parentcategoryid=0 or parentcategoryid IS NULL) " , " and parentcategoryid=" + PID.ToString()) + " order by displayorder, name";
			DataSet ds = DB.GetDS(sql,false,System.DateTime.Now.AddHours(1));
			if(ds.Tables[0].Rows.Count == 0)
			{
				ds.Dispose();
				return 0;
			}
			int i = 0;
			for(i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				if(ds.Tables[0].Rows[i]["CategoryID"].ToString() == CategoryID.ToString())
				{
					break;
				}
			}
			int id = 0;
			if(i == 0)
			{
				// if first, go to last
				id = Localization.ParseUSInt(ds.Tables[0].Rows[ds.Tables[0].Rows.Count-1]["CategoryID"].ToString());
			}
			else
			{
				id = Localization.ParseUSInt(ds.Tables[0].Rows[i-1]["CategoryID"].ToString());
			}
			ds.Dispose();
			return id;
		}

		// returns the "previous" Section, after the specified Section
		// "previous" is defined as either the Section that is next lower display order, or same display order and next lowest alphabetical order
		// is circular also (i.e. if first, return last)
		public static int GetPreviousSection(int SectionID)
		{
			int PID = Common.GetSectionParentID(SectionID);
			String sql = "SELECT C.SectionID, C.Name AS SectionName from [section] C  " + DB.GetNoLock() + " where deleted=0 " + Common.IIF(PID == 0 , " and (parentSectionid=0 or parentSectionid IS NULL) " , " and parentSectionid=" + PID.ToString()) + " order by displayorder, name";
			DataSet ds = DB.GetDS(sql,false,System.DateTime.Now.AddHours(1));
			if(ds.Tables[0].Rows.Count == 0)
			{
				ds.Dispose();
				return 0;
			}
			int i = 0;
			for(i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				if(ds.Tables[0].Rows[i]["SectionID"].ToString() == SectionID.ToString())
				{
					break;
				}
			}
			int id = 0;
			if(i == 0)
			{
				// if first, go to last
				id = Localization.ParseUSInt(ds.Tables[0].Rows[ds.Tables[0].Rows.Count-1]["SectionID"].ToString());
			}
			else
			{
				id = Localization.ParseUSInt(ds.Tables[0].Rows[i-1]["SectionID"].ToString());
			}
			ds.Dispose();
			return id;
		}



		static public decimal GetShipByTotalCharge(int ShippingMethodID,String RowGUID)
		{
			decimal tmp = System.Decimal.Zero;
			IDataReader rs = DB.GetRS("Select ShippingCharge from ShippingByTotal  " + DB.GetNoLock() + " where RowGUID=" + DB.SQuote(RowGUID) + " and ShippingMethodID=" + ShippingMethodID.ToString());
			if(rs.Read())
			{
				tmp = DB.RSFieldDecimal(rs,"ShippingCharge");
			}
			rs.Close();
			return tmp;
		}

		static public decimal GetShipByWeightCharge(int ShippingMethodID,String RowGUID)
		{
			decimal tmp = System.Decimal.Zero;
			IDataReader rs = DB.GetRS("Select ShippingCharge from ShippingByWeight  " + DB.GetNoLock() + " where RowGUID=" + DB.SQuote(RowGUID) + " and ShippingMethodID=" + ShippingMethodID.ToString());
			if(rs.Read())
			{
				tmp = DB.RSFieldDecimal(rs,"ShippingCharge");
			}
			rs.Close();
			return tmp;
		}

		static public decimal GetShipByZoneCharge(int ShippingZoneID,String RowGUID)
		{
			decimal tmp = System.Decimal.Zero;
			IDataReader rs = DB.GetRS("Select ShippingCharge from ShippingByZone  " + DB.GetNoLock() + " where RowGUID=" + DB.SQuote(RowGUID) + " and ShippingZoneID=" + ShippingZoneID.ToString());
			if(rs.Read())
			{
				tmp = DB.RSFieldDecimal(rs,"ShippingCharge");
			}
			rs.Close();
			return tmp;
		}

		static public String GetVariantSKUSuffix(int VariantID)
		{
			String tmpS = String.Empty;
			IDataReader rs = DB.GetRS("Select * from ProductVariant  " + DB.GetNoLock() + " where VariantID=" + VariantID.ToString());
			if(rs.Read())
			{
				tmpS = DB.RSField(rs,"SKUSuffix");
			}
			rs.Close();
			return tmpS;
		}

		static public int GetFirstProductVariant(int ProductID)
		{
			int tmp = 0;
			IDataReader rs = DB.GetRS("select variantid from productvariant  " + DB.GetNoLock() + " where deleted=0 and published<>0 and productid=" + ProductID.ToString() + " order by DisplayOrder");
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"VariantID");
			}
			rs.Close();
			return tmp;
		}

		public static String GetPollCategories(int PollID)
		{
			IDataReader rs = DB.GetRS("select * from Pollcategory  " + DB.GetNoLock() + " where Pollid=" + PollID.ToString());
			StringBuilder tmpS = new StringBuilder(1000);
			while(rs.Read())
			{
				if(tmpS.Length != 0)
				{
					tmpS.Append(",");
				}
				tmpS.Append(DB.RSFieldInt(rs,"CategoryID").ToString());
			}
			rs.Close();
			return tmpS.ToString();
		}

		public static String GetPollSections(int PollID)
		{
			IDataReader rs = DB.GetRS("select * from Pollsection  " + DB.GetNoLock() + " where Pollid=" + PollID.ToString());
			StringBuilder tmpS = new StringBuilder(1000);
			while(rs.Read())
			{
				if(tmpS.Length != 0)
				{
					tmpS.Append(",");
				}
				tmpS.Append(DB.RSFieldInt(rs,"SectionID").ToString());
			}
			rs.Close();
			return tmpS.ToString();
		}

		public static String GetPollName(int PollID)
		{
			IDataReader rs = DB.GetRS("select * from Poll  " + DB.GetNoLock() + " where Pollid=" + PollID.ToString());
			String tmpS = String.Empty;
			if(rs.Read())
			{
				tmpS = DB.RSField(rs,"Name");
			}
			rs.Close();
			return tmpS;
		}
		static public decimal GetProductShippingCost(int VariantID)
		{
			decimal tmp = System.Decimal.Zero;
			IDataReader rs = DB.GetRS("select shippingcost from productvariant  " + DB.GetNoLock() + " where VariantID=" + VariantID.ToString());
			if(rs.Read())
			{
				tmp = DB.RSFieldDecimal(rs,"shippingcost");
			}
			rs.Close();
			return tmp;
		}

		static public bool ShowProductBuyButton(int ProductID)
		{
			bool tmp = true;
			IDataReader rs = DB.GetRS("select showbuybutton from product  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString());
			if(rs.Read())
			{
				tmp = DB.RSFieldBool(rs,"showbuybutton");
			}
			rs.Close();
			return tmp;
		}

		static public bool ProductIsCallToOrder(int ProductID)
		{
			bool tmp = true;
			IDataReader rs = DB.GetRS("select IsCallToOrder from product  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString());
			if(rs.Read())
			{
				tmp = DB.RSFieldBool(rs,"IsCallToOrder");
			}
			rs.Close();
			return tmp;
		}

		static public String GetAddToCartForm(bool forPack, bool showWish, int ProductID, int VariantID, int _siteID, int DisplayFormat, bool ColorChangeProductImage)
		{
			if(!Common.AppConfigBool("ShowBuyButtons") || !Common.ShowProductBuyButton(ProductID))
			{
				return "&nbsp;";
			}
			if(Common.ProductIsCallToOrder(ProductID))
			{
				return "<form style=\"margin-top: 0px; margin-bottom: 0px;\"><font class=\"CallToOrder\">CALL TO ORDER</font></form>"; // use <form></form> to give same spacing that normal add to cart would have
			}
			IDataReader rs = DB.GetRS("select PV.*,P.ColorOptionPrompt,P.SizeOptionPrompt,P.TextOptionPrompt,P.TextOptionMaxLength, P.RequiresTextOption from Product P " + DB.GetNoLock() + " , productvariant PV  " + DB.GetNoLock() + " where PV.ProductID=P.ProductID and VariantID=" + VariantID.ToString());
			if(!rs.Read())
			{
				rs.Close();
				return String.Empty;
			}
			String RestrictedQuantities = DB.RSField(rs,"RestrictedQuantities");
			int MinimumQuantity = DB.RSFieldInt(rs,"MinimumQuantity");
			String Colors = DB.RSField(rs,"Colors");
			String ColorSKUModifiers = DB.RSField(rs,"ColorSKUModifiers");
			String Sizes = DB.RSField(rs,"Sizes");
			String SizeSKUModifiers = DB.RSField(rs,"SizeSKUModifiers");
			//String Sizes2 = DB.RSField(rs,"Sizes2").Trim();
			//String SizeSKUModifiers2 = DB.RSField(rs,"SizeSKUModifiers2").Trim();
			String SizeOptionPrompt = DB.RSField(rs,"SizeOptionPrompt");
			String ColorOptionPrompt = DB.RSField(rs,"ColorOptionPrompt");
			String TextOptionPrompt = DB.RSField(rs,"TextOptionPrompt");
			int TextOptionMaxLength = DB.RSFieldInt(rs,"TextOptionMaxLength");
			if(SizeOptionPrompt.Length == 0)
			{
				SizeOptionPrompt = Common.AppConfig("SizeOptionPrompt");
			}
			if(SizeOptionPrompt.Length == 0)
			{
				SizeOptionPrompt = "Size";
			}
			if(ColorOptionPrompt.Length == 0)
			{
				ColorOptionPrompt = Common.AppConfig("ColorOptionPrompt");
			}
			if(ColorOptionPrompt.Length == 0)
			{
				ColorOptionPrompt = "Color";
			}
			if(TextOptionPrompt.Length == 0)
			{
				TextOptionPrompt = Common.AppConfig("TextOptionPrompt");
			}
			if(TextOptionPrompt.Length == 0)
			{
				TextOptionPrompt = "Customization Text";
			}
			if(TextOptionMaxLength == 0)
			{
				TextOptionMaxLength = 50;
			}
			
			bool RequiresTextOption = DB.RSFieldBool(rs,"RequiresTextOption");
			rs.Close();

			String FormName = "AddToCartForm_" + ProductID.ToString() + "_" + VariantID.ToString();

			bool ProtectInventory = Common.AppConfigBool("Inventory.LimitCartToQuantityOnHand");
			int ProtectInventoryMinQuantity = Common.AppConfigUSInt("Inventory.MinQuantity"); // if qty is below this, addtocart will not allow it
			String InventoryControlList = String.Empty;
			if(ProtectInventory)
			{
				InventoryControlList = Common.GetInventoryList(ProductID,VariantID);
			}

			StringBuilder tmpS = new StringBuilder(10000);
			tmpS.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td valign=\"middle\" align=\"" + Common.IIF(DisplayFormat == 5 , "left" , "left") + "\"><br>\n");
			tmpS.Append("\n");
			tmpS.Append("<script type=\"text/javascript\" Language=\"JavaScript\">\n");

			if(ProtectInventory && InventoryControlList.Length != 0)
			{
				bool first = true;
				foreach(String s in InventoryControlList.Split('|'))
				{
					if(first)
					{
						tmpS.Append("var board = new Array(");
					}
					else
					{
						tmpS.Append(",");
					}
					String[] ivals = s.Split(',');
					tmpS.Append("new Array('" + ivals[0].Replace("'","") + "','" + ivals[1].Replace("'","") + "','" + ivals[2].Replace("'","") + "')");
					first = false;
				}
				tmpS.Append(");\n");
			}

			tmpS.Append("function " + FormName + "_Validator(theForm)\n");
			tmpS.Append("	{\n");
			tmpS.Append("	submitonce(theForm);\n");
			
			tmpS.Append("	if ((theForm.Quantity.value*1) < 1)\n"); // convert form val to integer
			tmpS.Append("	{\n");
			tmpS.Append("		alert(\"Please specify the quantity you want to add to your cart\");\n");
			tmpS.Append("		theForm.Quantity.focus();\n");
			tmpS.Append("		submitenabled(theForm);\n");
			tmpS.Append("		return (false);\n");
			tmpS.Append("    }\n");
			
			if(RestrictedQuantities.Length == 0 && MinimumQuantity != 0)
			{
				tmpS.Append("	if ((theForm.Quantity.value*1) < " + MinimumQuantity.ToString() + ")\n"); // convert form val to integer
				tmpS.Append("	{\n");
				tmpS.Append("		alert(\"The minimum quantity for this product is: " + MinimumQuantity.ToString() + "\");\n");
				tmpS.Append("		theForm.Quantity.focus();\n");
				tmpS.Append("		submitenabled(theForm);\n");
				tmpS.Append("		return (false);\n");
				tmpS.Append("    }\n");
			}
			
			if(Colors.Length != 0)
			{
				tmpS.Append("	if (theForm.Color.selectedIndex < 1)\n");
				tmpS.Append("	{\n");
				tmpS.Append("		alert(\"Please select a " + ColorOptionPrompt.ToLower() + ".\");\n");
				tmpS.Append("		theForm.Color.focus();\n");
				tmpS.Append("		submitenabled(theForm);\n");
				tmpS.Append("		return (false);\n");
				tmpS.Append("    }\n");
			}
			if(Sizes.Length != 0)
			{
				tmpS.Append("	if (theForm.Size.selectedIndex < 1)\n");
				tmpS.Append("	{\n");
				tmpS.Append("		alert(\"Please select a " + SizeOptionPrompt.ToLower() + ".\");\n");
				tmpS.Append("		theForm.Size.focus();\n");
				tmpS.Append("		submitenabled(theForm);\n");
				tmpS.Append("		return (false);\n");
				tmpS.Append("    }\n");
			}
//			if(Sizes2.Length != 0)
//			{
//				tmpS.Append("	if (theForm.Size2.selectedIndex < 1)\n");
//				tmpS.Append("	{\n");
//				tmpS.Append("		alert(\"Please select your Bra size.\");\n");
//				tmpS.Append("		theForm.Size2.focus();\n");
//				tmpS.Append("		submitenabled(theForm);\n");
//				tmpS.Append("		return (false);\n");
//				tmpS.Append("    }\n");
//			}
			if(RequiresTextOption)
			{
				tmpS.Append("	if (theForm.TextOption.value.length == 0)\n");
				tmpS.Append("	{\n");
				tmpS.Append("		alert(\"Please enter your " + TextOptionPrompt + ".\");\n");
				tmpS.Append("		theForm.TextOption.focus();\n");
				tmpS.Append("		submitenabled(theForm);\n");
				tmpS.Append("		return (false);\n");
				tmpS.Append("    }\n");
			}
			if(ProtectInventory)
			{
				if(!Common.ProductUsesAdvancedInventoryMgmt(ProductID))
				{
					int i = Common.GetInventory(ProductID,VariantID,"","");
					tmpS.Append("	if (theForm.Quantity.value > " + i.ToString() + ")\n");
					tmpS.Append("	{\n");
					tmpS.Append("		alert(\"Your quantity exceeds stock on hand. The maximum quantity that can be added is " + i.ToString() + ". Please contact us if you need more information.\");\n");
					tmpS.Append("		theForm.Quantity.value = " + i.ToString() + ";\n");
					tmpS.Append("		theForm.Quantity.focus();\n");
					tmpS.Append("		submitenabled(theForm);\n");
					tmpS.Append("		return (false);\n");
					tmpS.Append("    }\n");
				}
				else
				{
					tmpS.Append("var sel_color = theForm.Color[theForm.Color.selectedIndex].value;\n");
					tmpS.Append("var sel_size = theForm.Size[theForm.Size.selectedIndex].value;\n");

					//tmpS.Append("alert('0 ' + sel_color + ' : ' + sel_size);\n");
					
					tmpS.Append("sel_color = sel_color.substring(0,sel_color.indexOf(',')).replace(new RegExp(\"'\", 'gi'), '');\n");
					tmpS.Append("sel_size = sel_size.substring(0,sel_size.indexOf(',')).replace(new RegExp(\"'\", 'gi'), '');\n");
					
					//tmpS.Append("alert('1 ' + sel_color + ' : ' + sel_size);\n");

					// clean price delta options if any, so match will work on inventory control list:
					tmpS.Append("var i = sel_color.indexOf(\"[\");\n");
					//tmpS.Append("alert('2 ' + sel_color + ' : ' + sel_size);\n");
					tmpS.Append("if(i != -1)\n");
					tmpS.Append("{\n");
					tmpS.Append("	sel_color = Trim(sel_color.substring(0,i));\n");
					//tmpS.Append("alert('3 ' + sel_color + ' : ' + sel_size);\n");
					tmpS.Append("}\n");
					//tmpS.Append("alert('4 ' + sel_color + ' : ' + sel_size);\n");
					tmpS.Append("var j = sel_size.indexOf(\"[\");\n");
					tmpS.Append("if(j != -1)\n");
					tmpS.Append("{\n");
					//tmpS.Append("alert('5 ' + sel_color + ' : ' + sel_size);\n");
					tmpS.Append("	sel_size = Trim(sel_size.substring(0,j));\n");
					//tmpS.Append("alert('6 ' + sel_color + ' : ' + sel_size);\n");
					tmpS.Append("}\n");
					//tmpS.Append("alert('7 ' + sel_color + ' : ' + sel_size);\n");


					tmpS.Append("var sel_qty = theForm.Quantity.value;\n");
					//tmpS.Append("alert(sel_color + ',' + sel_size + ',' + sel_qty);\n");
					tmpS.Append("for(i = 0; i < board.length; i++)\n");
					tmpS.Append("{\n");
					//tmpS.Append("	alert(board[i][0] + ',' + board[i][1] + ',' + board[i][2]);\n");
					tmpS.Append("	if(board[i][0] == sel_color && board[i][1] == sel_size)\n");
					tmpS.Append("	{\n");
					//tmpS.Append("		alert(sel_color + ',' + sel_size + ',' + sel_qty);\n");
					tmpS.Append("		if(parseInt(sel_qty) > parseInt(board[i][2]))\n");
					tmpS.Append("		{\n");
					tmpS.Append("			if(parseInt(board[i][2]) == 0)\n");
					tmpS.Append("			{\n");
					tmpS.Append("				alert('" + ColorOptionPrompt + ": ' + sel_color + ', " + SizeOptionPrompt + ": ' + sel_size + ' is currently out of stock.\\n\\nPlease select another " + ColorOptionPrompt + "/" + SizeOptionPrompt + " combination.');\n");
					tmpS.Append("			}\n");
					tmpS.Append("			else\n");
					tmpS.Append("			{\n");
					tmpS.Append("				alert('Your quantity exceeds our inventory on hand. The maximum quantity that can be added for " + ColorOptionPrompt + ": ' + sel_color + ', " + SizeOptionPrompt + ": ' + sel_size + ' is ' + board[i][2] + '.\\n\\nPlease reduce your quantity, or select another " + ColorOptionPrompt + "/" + SizeOptionPrompt + " combination.');\n");
					tmpS.Append("			}\n");
					tmpS.Append("			submitenabled(theForm);\n");
					tmpS.Append("			return (false);\n");
					tmpS.Append("		}\n");
					tmpS.Append("	}\n");
					tmpS.Append("}\n");
					tmpS.Append("submitenabled(theForm);\n");
				}
			}
			tmpS.Append("	submitenabled(theForm);\n");
			tmpS.Append("	return (true);\n");
			tmpS.Append("	}\n");
			tmpS.Append("</script>\n");
			String Action = String.Empty;
			if(!forPack)
			{
				Action = "addtocart.aspx?returnurl=" + HttpContext.Current.Server.UrlEncode(Common.GetThisPageName(false) + "?" + Common.ServerVariables("QUERY_STRING")) + "&productid=" + ProductID.ToString() + "&variantid=" + VariantID;
			}
			else
			{
				Action = "dyop.aspx?productid=" + ProductID;
			}
			
			tmpS.Append("<form  style=\"margin-top: 0px; margin-bottom: 0px;\" method=\"POST\" name=\"" + FormName + "\" id=\"" + FormName + "\" action=\"" + Action + "\" onsubmit=\"return validateForm(this) && " + FormName + "_Validator(this)\" >");
			tmpS.Append("<input type=\"hidden\" name=\"UpsellProducts\" value=\"\">\n");
			tmpS.Append("<input name=\"IsWish\" type=\"hidden\" value=\"0\">");
			if(forPack)
			{
				tmpS.Append("<input type=\"hidden\" name=\"IsSubmit\" value=\"Add\">\n");
				tmpS.Append("<input type=\"hidden\" name=\"ProductTypeID\" value=\"0\">\n");
				tmpS.Append("<input type=\"hidden\" name=\"ProductID\" value=\"" + ProductID.ToString() + "\">\n");
				tmpS.Append("<input type=\"hidden\" name=\"VariantID\" value=\"" + VariantID.ToString() + "\">\n");
				if(Colors.Length == 0)
				{
					tmpS.Append("<input type=\"hidden\" name=\"Color\" value=\"\">\n");
				}
				if(Sizes.Length == 0)
				{
					tmpS.Append("<input type=\"hidden\" name=\"Size\" value=\"\">\n");
				}
			}
			if(Common.AppConfigBool("ShowQuantityOnProductPage"))
			{
				if(RestrictedQuantities.Length == 0)
				{
					int InitialQ = 1;
					if(Common.AppConfig("DefaultAddToCartQuantity").Length != 0)
					{
						InitialQ = Common.AppConfigUSInt("DefaultAddToCartQuantity");
					}
					tmpS.Append("<small>Quantity:</small> <input type=\"text\" value=\"" + InitialQ.ToString() + "\" name=\"Quantity\" size=\"3\" maxlength=\"4\">");
					tmpS.Append("<input name=\"Quantity_vldt\" type=\"hidden\" value=\"[req][number][blankalert=Please enter the desired quantity][invalidalert=Please enter a number for your quantity, e.g. 1]\">&nbsp;");
				}
				else
				{
					tmpS.Append("<small>Quantity:</small>");
					tmpS.Append("<select name=\"Quantity\" size=\"1\">");
					foreach(String s in RestrictedQuantities.Split(','))
					{
						if(s.Trim().Length != 0)
						{
							int Q = Localization.ParseUSInt(s.Trim());
							tmpS.Append("<option value=\"" + Q.ToString() + "\">" + Q.ToString() + "</option>");
						}
					}
					tmpS.Append("</select>&nbsp;");
				}
			}
			Decimal M = 1.0M;
			String MM = Localization.CurrencyStringForDisplay(M).Substring(0,1); // get currency symbol
			if(Common.IsInteger(MM))
			{
				MM = String.Empty; // something international happened, so just leave empty, we only want currency symbol, not any digits
			}
			if(Colors.Length != 0)
			{
				String[] ColorsSplit = Colors.Split(',');
				String[] ColorSKUsSplit = ColorSKUModifiers.Split(',');
				tmpS.Append("<select size=\"1\" id=\"Color\" name=\"Color\" " + Common.IIF(ColorChangeProductImage, "onChange=\"setcolorpic(Color.value)\"", "") + ">\n");
				tmpS.Append("<option value=\"-,-\">" + ColorOptionPrompt + "</option>\n");
				for(int i = ColorsSplit.GetLowerBound(0); i <= ColorsSplit.GetUpperBound(0); i++)
				{
					String Modifier = String.Empty;
					try
					{
						Modifier = ColorSKUsSplit[i];
					}
					catch
					{}
					tmpS.Append("<option value=\"" + HttpContext.Current.Server.HtmlEncode(ColorsSplit[i].Trim() + "," + Modifier.Trim()) + "\">" + ColorsSplit[i].Trim().Replace("[","[" + MM) + "</option>\n");
				}
				tmpS.Append("</select>\n");
			}
			if(Sizes.Length != 0)
			{
				String[] SizesSplit = Sizes.Split(',');
				String[] SizeSKUsSplit = SizeSKUModifiers.Split(',');
				tmpS.Append("<select size=\"1\" name=\"Size\" class=\"SizeText\">\n");
				tmpS.Append("<option value=\"-,-\">" + SizeOptionPrompt + "</option>\n");
				for(int i = SizesSplit.GetLowerBound(0); i <= SizesSplit.GetUpperBound(0); i++)
				{
					String Modifier = String.Empty;
					try
					{
						Modifier = SizeSKUsSplit[i];
					}
					catch
					{}
					tmpS.Append("<option value=\"" + HttpContext.Current.Server.HtmlEncode(SizesSplit[i].Trim() + "," + Modifier.Trim()) + "\">" + SizesSplit[i].Trim().Replace("[","[" + MM) + "</option>\n");
				}
				tmpS.Append("</select>\n");
			}
//			if(Sizes2.Length != 0)
//			{
//				String[] SizesSplit2 = Sizes2.Split(',');
//				String[] SizeSKUsSplit2 = SizeSKUModifiers2.Split(',');
//				tmpS.Append("<select size=\"1\" name=\"Size2\" class=\"SizeText\">\n");
//				tmpS.Append("<option value=\"-,-\">Bra Size</option>\n");
//				for(int i = SizesSplit2.GetLowerBound(0); i <= SizesSplit2.GetUpperBound(0); i++)
//				{
//					String Modifier = String.Empty;
//					try
//					{
//						Modifier = SizeSKUsSplit2[i];
//					}
//					catch
//					{}
//					tmpS.Append("<option value=\"" + HttpContext.Current.Server.HtmlEncode(SizesSplit2[i] + "," + Modifier) + "\" " + (Common.Session("ChosenSize2") == SizesSplit2[i] ? " selected" : "") + ">" + SizesSplit2[i].Replace("[","[" + MM) + "</option>\n");
//				}
//				tmpS.Append("</select>\n");
//			}
			if(Colors.Length != 0 || Sizes.Length != 0)
			{
				switch(DisplayFormat)
				{
					case 1:
						// variants in right bar:
						break;
					case 2:
						// variants in grid:
						break;
					case 3:
						// variants in table - expanded
						break;
					case 4:
						// variants in table - condensed
						//tmpS.Append("<br>");
						break;
					case 5:
						// single variant format
						//tmpS.Append("<br>");
						break;
				}

			}
			if(RequiresTextOption)
			{
				tmpS.Append("<br>");
				if(TextOptionMaxLength < 50)
				{
					tmpS.Append(TextOptionPrompt + ": <input type=\"text\" size=\"20\" maxlength=\"" + TextOptionMaxLength.ToString() + "\"  name=\"TextOption\">");
				}
				else
				{
					tmpS.Append(TextOptionPrompt + ":<br><textarea rows=4 cols=50 name=\"TextOption\"></textarea><br>");
				}
			}
			if(!forPack)
			{
				tmpS.Append("<input type=\"submit\" onClick=\"document." + FormName + ".IsWish.value='0';\" value=\"" + Common.AppConfig("CartButtonPrompt") + "\" name=\"Submit\">");
			}
			else
			{
				tmpS.Append("<input type=\"button\" onClick=\"if(validateForm(document.getElementById('" + FormName + "')) && " + FormName + "_Validator(document.getElementById('" + FormName + "'))) {SendAddToCustomForm(document.getElementById('" + FormName + "'));}\" value=\"Add To Pack\" name=\"AddToCustomButton\">");
			}
			if(Common.AppConfigBool("ShowWishButtons") && showWish && !forPack)
			{
				//tmpS.Append("<input type=\"submit\" onClick=\"document.CartForm.IsWish.value='1';\" value=\"Add To Wish List\" name=\"Submit\">");
				tmpS.Append("&nbsp;<img style=\"cursor:hand;\" onClick=\"document." + FormName + ".IsWish.value='1';if(validateForm(document." + FormName + ") && " + FormName + "_Validator(document." + FormName + ")) {document." + FormName + ".submit();}\" align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/wish.gif\" border=\"0\">");
			}
			if(DisplayFormat == 5)
			{
				tmpS.Append("<img src=\"images/spacer.gif\" width=\"20\" height=\"1\">\n");
			}
			tmpS.Append("</form>\n");
			tmpS.Append("</td></tr></table>");
			return tmpS.ToString();
		}

		static public int GetVariantProductID(int VariantID)
		{
			int tmp = 0;
			IDataReader rs = DB.GetRS("select productid from productvariant  " + DB.GetNoLock() + " where variantid=" + VariantID.ToString());
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"ProductID");
			}
			rs.Close();
			return tmp;
		}

		static public int GetShippingZone(Customer thisCustomer)
		{
			int ZoneID = 0;
			String ShippingZip = thisCustomer.ShippingZip;
			if(ShippingZip.Length != 0)
			{
				ZoneID = Common.ZoneLookup(ShippingZip);
			}
			return ZoneID;
		}

		static public int ZoneLookup(String zip)
		{
			zip = zip.PadLeft(5,'0');
			String Zip3 = zip.Substring(0,3);
			int Zip3Int = 0;
			try
			{
				Zip3Int = Localization.ParseUSInt(Zip3);
			}
			catch
			{
				return Common.AppConfigUSInt("ZoneIDForNoMatch"); // something bad as input zip
			}
			int ZoneID = Common.AppConfigUSInt("ZoneIDForNoMatch");
			IDataReader rs = DB.GetRS("select * from ShippingZone " + DB.GetNoLock() + " ");
			while(rs.Read())
			{
				String[] thisZipList = DB.RSField(rs,"ZipCodes").Split(',');
				foreach(String s in thisZipList)
				{
					// is it a single 3 digit prefix, or a range:
					if(s.IndexOf("-") == -1)
					{
						// single item:
						int LowPrefix = 0;
						try
						{
							if(Common.IsInteger(s))
							{
								LowPrefix = Localization.ParseUSInt(s);
							}
						}
						catch {}
						if(LowPrefix == Zip3Int)
						{
							ZoneID = DB.RSFieldInt(rs,"ShippingZoneID");
							break;
						}
					}
					else
					{
						// range:
						String[] s2 = s.Split('-');
						int LowPrefix = 0;
						int HighPrefix = 0;
						try
						{
							if(Common.IsInteger(s2[0]))
							{
								LowPrefix = Localization.ParseUSInt(s2[0]);
							}
							if(Common.IsInteger(s2[1]))
							{
								HighPrefix = Localization.ParseUSInt(s2[1]);
							}
						}
						catch {}
						if(LowPrefix <= Zip3Int && Zip3Int <= HighPrefix)
						{
							ZoneID = DB.RSFieldInt(rs,"ShippingZoneID");
							break;
						}
					}
				}
			}
			rs.Close();
			return ZoneID;
		}

		static public Common.ShippingCalculationEnum GetActiveShippingCalculationID()
		{
			int tmp = Common.AppConfigUSInt("DefaultShippingCalculationID");
			IDataReader rs = DB.GetRS("Select * from ShippingCalculation  " + DB.GetNoLock() + " where selected=1");
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"ShippingCalculationID");
			}
			rs.Close();
			return (Common.ShippingCalculationEnum)tmp;
		}

		static public String GetShippingMethodName(int ShippingMethodID)
		{
			String tmpS = String.Empty;
			IDataReader rs = DB.GetRS("Select * from ShippingMethod  " + DB.GetNoLock() + " where ShippingMethodID=" + ShippingMethodID.ToString());
			if(rs.Read())
			{
				tmpS = DB.RSField(rs,"Name");
			}
			rs.Close();
			return tmpS;
		}

		static public Single GetStateTaxRate(int StateID)
		{
			Single taxrate = 0.0F;
			IDataReader rs = DB.GetRS("Select TaxRate from statetaxrate  " + DB.GetNoLock() + " where State=(select Abbreviation from State where stateid=" + StateID.ToString() + ")");
			rs.Read();
			taxrate = DB.RSFieldSingle(rs,"TaxRate");
			rs.Close();
			return taxrate;
		}

		static public Single GetStateTaxRate(String StateAbbrev)
		{
			Single taxrate = 0.0F;
			IDataReader rs = DB.GetRS("Select TaxRate from statetaxrate  " + DB.GetNoLock() + " where State=" + DB.SQuote(StateAbbrev).ToUpper());
			rs.Read();
			taxrate = DB.RSFieldSingle(rs,"TaxRate");
			rs.Close();
			return taxrate;
		}

		static public int GetRandomNumber(int lowerBound, int upperBound)
		{
			return new System.Random().Next(lowerBound,upperBound+1);
		}


		public static void LogEvent(int CustomerID, int EventID,String parms)
		{
			if(Common.AppConfig("EventLoggingEnabled").ToUpper() == "TRUE")
			{
				if(parms.Length == 0)
				{
					DB.ExecuteSQL("insert into LOG_CustomerEvent(CustomerID,EventID,Timestamp,Data) values(" + CustomerID.ToString() + "," + EventID.ToString() + "," + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",NULL)");
				}
				else
				{
					DB.ExecuteSQL("insert into LOG_CustomerEvent(CustomerID,EventID,Timestamp,Data) values(" + CustomerID.ToString() + "," + EventID.ToString() + "," + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + "," + DB.SQuote(parms) + ")");
				}
			}
		}

		public static bool ProductIsInCategory(int ProductID, int CategoryID)
		{
			IDataReader rs = DB.GetRS("select count(*) as N from productcategory  " + DB.GetNoLock() + " where productid=" + ProductID.ToString() + " and categoryid=" + CategoryID.ToString());
			rs.Read();
			bool IsInCat = (DB.RSFieldInt(rs,"N") != 0);
			rs.Close();
			return IsInCat;
		}

		public static String GetProductCategories(int ProductID, bool ForProductBrowser)
		{
			IDataReader rs = DB.GetRS("select * from productcategory  " + DB.GetNoLock() + " where productid=" + ProductID.ToString() + Common.IIF(ForProductBrowser," and categoryid in (select categoryid from category  " + DB.GetNoLock() + " where deleted=0 and published<>0 and ShowInProductBrowser<>0)",""));
			StringBuilder tmpS = new StringBuilder(1000);
			while(rs.Read())
			{
				if(tmpS.Length != 0)
				{
					tmpS.Append(",");
				}
				tmpS.Append(DB.RSFieldInt(rs,"CategoryID").ToString());
			}
			rs.Close();
			return tmpS.ToString();
		}

		public static String GetProductSections(int ProductID, bool ForProductBrowser)
		{
			IDataReader rs = DB.GetRS("select * from productsection  " + DB.GetNoLock() + " where productid=" + ProductID.ToString() + Common.IIF(ForProductBrowser," and sectionid in (select sectionid from [section]  " + DB.GetNoLock() + " where deleted=0 and published<>0 and ShowInProductBrowser<>0)",""));
			StringBuilder tmpS = new StringBuilder(1000);
			while(rs.Read())
			{
				if(tmpS.Length != 0)
				{
					tmpS.Append(",");
				}
				tmpS.Append(DB.RSFieldInt(rs,"SectionID").ToString());
			}
			rs.Close();
			return tmpS.ToString();
		}

		public static String GetProductAffiliates(int ProductID, bool ForProductBrowser)
		{
			IDataReader rs = DB.GetRS("select * from productaffiliate  " + DB.GetNoLock() + " where productid=" + ProductID.ToString());
			StringBuilder tmpS = new StringBuilder(1000);
			while(rs.Read())
			{
				if(tmpS.Length != 0)
				{
					tmpS.Append(",");
				}
				tmpS.Append(DB.RSFieldInt(rs,"AffiliateID").ToString());
			}
			rs.Close();
			return tmpS.ToString();
		}

		public static String GetProductCustomerLevels(int ProductID, bool ForProductBrowser)
		{
			IDataReader rs = DB.GetRS("select * from productcustomerlevel  " + DB.GetNoLock() + " where productid=" + ProductID.ToString());
			StringBuilder tmpS = new StringBuilder(1000);
			while(rs.Read())
			{
				if(tmpS.Length != 0)
				{
					tmpS.Append(",");
				}
				tmpS.Append(DB.RSFieldInt(rs,"CustomerLevelID").ToString());
			}
			rs.Close();
			return tmpS.ToString();
		}

		public static int GetFirstProduct(int CategoryID, bool AllowKits, bool AllowPacks)
		{
			String sql = "SELECT P.ProductID FROM ((((Category C  " + DB.GetNoLock() + " INNER JOIN ProductCategory PC  " + DB.GetNoLock() + " ON C.CategoryID = PC.CategoryID) INNER JOIN Product P  " + DB.GetNoLock() + " ON PC.ProductID = P.ProductID) INNER JOIN Manufacturer M  " + DB.GetNoLock() + " ON P.ManufacturerID = M.ManufacturerID) left outer join CategoryDisplayOrder DO  " + DB.GetNoLock() + " on p.productid=DO.productid) WHERE DO.categoryid=" + CategoryID.ToString() + " and (P.Published = 1) AND (P.Deleted = 0) " + Common.IIF(AllowKits, "", " and P.IsAKit=0") + Common.IIF(AllowPacks, "", " and P.IsAPack=0") + " and PC.categoryid=" + CategoryID.ToString() + " order by DO.displayorder, p.name";
			DataSet ds = DB.GetDS(sql,false,System.DateTime.Now.AddHours(1));
			if(ds.Tables[0].Rows.Count == 0)
			{
				ds.Dispose();
				return 0;
			}
			int id = Localization.ParseUSInt(ds.Tables[0].Rows[0]["ProductID"].ToString());
			ds.Dispose();
			return id;
		}

		public static String GetFirstProductCategory(int ProductID, bool ForProductBrowser)
		{
			String tmpS = GetProductCategories(ProductID,ForProductBrowser);
			if(tmpS.Length == 0)
			{
				return String.Empty;
			}
			String[] ss = tmpS.Split(',');
			String result = String.Empty;
			try
			{
				result = Common.GetCategoryName(Localization.ParseUSInt(ss[0]));
			}
			catch {}
			return result;
		}

		public static String GetZoneName(int ShippingZoneID)
		{
			IDataReader rs = DB.GetRS("select Name from ShippingZone  " + DB.GetNoLock() + " where ShippingZoneID=" + ShippingZoneID.ToString());
			String tmpS = String.Empty;
			if(rs.Read())
			{
				tmpS = DB.RSField(rs,"Name");
			}
			rs.Close();
			return tmpS;
		}

		public static String GetProductName(int ProductID)
		{
			IDataReader rs = DB.GetRS("select Name from product  " + DB.GetNoLock() + " where productid=" + ProductID.ToString());
			String tmpS = String.Empty;
			if(rs.Read())
			{
				tmpS = DB.RSField(rs,"Name");
			}
			rs.Close();
			return tmpS;
		}

		public static String GetLocaleSettingDescription(String LocaleSetting)
		{
			IDataReader rs = DB.GetRS("select Description from LocaleSetting  " + DB.GetNoLock() + " where name=" + DB.SQuote(LocaleSetting));
			String tmpS = String.Empty;
			if(rs.Read())
			{
				tmpS = DB.RSField(rs,"Description");
			}
			rs.Close();
			return tmpS;
		}

		public static String GetRequiresProducts(int ProductID)
		{
			IDataReader rs = DB.GetRS("select RequiresProducts from product  " + DB.GetNoLock() + " where productid=" + ProductID.ToString());
			String tmpS = String.Empty;
			if(rs.Read())
			{
				tmpS = DB.RSField(rs,"RequiresProducts");
			}
			rs.Close();
			return tmpS;
		}

		public static String GetVariantName(int VariantID)
		{
			IDataReader rs = DB.GetRS("select Name from productvariant  " + DB.GetNoLock() + " where VariantID=" + VariantID.ToString());
			String tmpS = String.Empty;
			if(rs.Read())
			{
				tmpS = DB.RSField(rs,"Name");
			}
			rs.Close();
			return tmpS;
		}

		public static String GetRestrictedQuantities(int VariantID, out int MinimumQuantity)
		{
			MinimumQuantity = 0;
			IDataReader rs = DB.GetRS("select RestrictedQuantities,MinimumQuantity from productvariant  " + DB.GetNoLock() + " where VariantID=" + VariantID.ToString());
			String tmpS = String.Empty;
			if(rs.Read())
			{
				tmpS = DB.RSField(rs,"RestrictedQuantities");
				MinimumQuantity = DB.RSFieldInt(rs,"MinimumQuantity");
			}
			rs.Close();
			return tmpS;
		}

		public static String GetProductSKU(int ProductID)
		{
			IDataReader rs = DB.GetRS("select SKU from product  " + DB.GetNoLock() + " where productid=" + ProductID.ToString());
			String tmpS = String.Empty;
			if(rs.Read())
			{
				tmpS = DB.RSField(rs,"SKU");
			}
			rs.Close();
			return tmpS;
		}

		public static void ClearCache()
		{
			foreach (DictionaryEntry entry in HttpContext.Current.Cache)
			{
				HttpContext.Current.Cache.Remove((string)entry.Key);
			}
			HttpContext.Current.Application.Clear();
			Common.AppConfigTable = Common.LoadAppConfig();
			if(Common.IsAdminSite)
			{
				// try to reset the store cache:
				String tmpS = Common.AspHTTP(Common.GetStoreHTTPLocation(false) + "clearcache.aspx");
			}
		}

		public static String GetImagePath(String ForWhat, String Size, bool fullPath)
		{
			String pth = String.Empty;
			String pthPrefix = String.Empty;
			if(Common.IsAdminSite)
			{
				pthPrefix = "../";
			}
			String IBP = String.Empty;
			switch(ForWhat.ToUpper())
			{
				case "MANUFACTURER":
				{
					pth = IBP + pthPrefix + "images/manufacturer/";
				}
					break;
				case "ORDEROPTION":
				{
					pth = IBP + pthPrefix + "images/orderoption/";
				}
					break;
				case "CATEGORY":
				{
					pth = IBP + pthPrefix + "images/category/";
				}
					break;
				case "SECTION":
				{
					pth = IBP + pthPrefix + "images/section/";
				}
					break;
				case "PRODUCT":
				{
					pth = IBP + pthPrefix + "images/product/" + Size.ToLower() + "/";
				}
					break;
				case "VARIANT":
				{
					pth = IBP + pthPrefix + "images/variant/" + Size.ToLower() + "/";
				}
					break;
				case "PARTNER":
				{
					pth = IBP + pthPrefix + "images/partner/";
				}
					break;
				case "STAFF":
				{
					pth = IBP + pthPrefix + "images/staff/" + Size.ToLower() + "/";
				}
					break;
				case "GALLERY":
				{
					pth = IBP + pthPrefix + "images/gallery/";
				}
					break;
				default:
				{
					pth = IBP + pthPrefix + "images/" + ForWhat + "/";
				}
					break;
			}

			//Now have a _full_ url pth which will take into account any virtual directory mappings
			if (fullPath)
			{
				pth = HttpContext.Current.Server.MapPath(pth); //Common.AppConfig("StoreFilesPath");
			}
			return pth;
		}


		static public String NoPictureImageURL(bool icon, int SiteID)
		{
			return "skins/skin_" + SiteID.ToString() + "/images/nopicture" + Common.IIF(icon, "icon","") + ".gif";
		}

		public static String LookupImage(String forWhat, int ID, String Size, int SiteID)
		{
			String FN = ID.ToString();
			if(forWhat.ToUpper() == "PRODUCT" && Common.AppConfigBool("UseSKUForProductImageName"))
			{
				IDataReader rs = DB.GetRS("select SKU from product  " + DB.GetNoLock() + " where productid=" + ID.ToString());
				if(rs.Read())
				{
					String SKU = DB.RSField(rs,"SKU").Trim();
					if(SKU.Length != 0)
					{
						FN = SKU;
					}
				}
				rs.Close();
			}
			String Image1 = Common.GetImagePath(forWhat,Size,true) + FN + ".jpg";
			String Image1URL = Common.GetImagePath(forWhat,Size,false) + FN + ".jpg";
			if(!Common.FileExists(Image1))
			{
				Image1 = Common.GetImagePath(forWhat,Size,true) + FN + ".gif";
				Image1URL = Common.GetImagePath(forWhat,Size,false) + FN + ".gif";
			}
			if(!Common.FileExists(Image1))
			{
				Image1 = Common.GetImagePath(forWhat,Size,true) + FN + ".png";
				Image1URL = Common.GetImagePath(forWhat,Size,false) + FN + ".png";
			}
			if(!Common.FileExists(Image1))
			{
				Image1 = String.Empty;
				Image1URL = String.Empty;
			}

			if(Image1URL.Length == 0 && (Size == "icon" || Size == "medium"))
			{
				Image1URL = Common.NoPictureImageURL(Size == "icon",SiteID);
				if(Image1URL.Length != 0)
				{
					Image1 = HttpContext.Current.Server.MapPath(Image1URL);
				}
			}
			return Image1URL;
		}

		public static String LookupProductImageByNumberAndColor(int ProductID, int SiteID, String LocaleSetting, int ImageNumber, String Color)
		{
			String FN = ProductID.ToString();
			String Size = "medium";
			String forWhat = "product";
			String SafeColor = Common.MakeSafeFilesystemName(Color);
			if(forWhat.ToUpper() == "PRODUCT" && Common.AppConfigBool("UseSKUForProductImageName"))
			{
				IDataReader rs = DB.GetRS("select SKU from product  " + DB.GetNoLock() + " where productid=" + ProductID.ToString());
				if(rs.Read())
				{
					String SKU = DB.RSField(rs,"SKU").Trim();
					if(SKU.Length != 0)
					{
						FN = SKU;
					}
				}
				rs.Close();
			}
			String Image1 = Common.GetImagePath(forWhat,Size,true) + FN + "_" + ImageNumber.ToString() + "_" + SafeColor + ".jpg";
			String Image1URL = Common.GetImagePath(forWhat,Size,false) + FN +  "_" + ImageNumber.ToString() + "_" + SafeColor + ".jpg";
			if(!Common.FileExists(Image1))
			{
				Image1 = Common.GetImagePath(forWhat,Size,true) + FN +  "_" + ImageNumber.ToString() + "_" + SafeColor + ".gif";
				Image1URL = Common.GetImagePath(forWhat,Size,false) + FN +  "_" + ImageNumber.ToString() + "_" + SafeColor + ".gif";
			}
			if(!Common.FileExists(Image1))
			{
				Image1 = Common.GetImagePath(forWhat,Size,true) + FN +  "_" + ImageNumber.ToString() + "_" + SafeColor + ".png";
				Image1URL = Common.GetImagePath(forWhat,Size,false) + FN +  "_" + ImageNumber.ToString() + "_" + SafeColor + ".png";
			}
			if(!Common.FileExists(Image1))
			{
				Image1 = String.Empty;
				Image1URL = String.Empty;
			}

			if(Image1URL.Length == 0)
			{
				Image1URL = Common.NoPictureImageURL(Size == "icon",SiteID);
				if(Image1URL.Length != 0)
				{
					Image1 = HttpContext.Current.Server.MapPath(Image1URL);
				}
			}
			return Image1URL;
		}


		
		static public String GetExceptionDetail(Exception ex, String LineSeparator)
		{
			String ExDetail = "Exception=" + ex.Message + LineSeparator;
			while(ex.InnerException != null)
			{
				ExDetail += ex.InnerException.Message + LineSeparator;
				ex = ex.InnerException;
			}
			return ExDetail;
		}

		static public String HighlightTerm(String InString, String Term)
		{
			int i = InString.ToUpper().IndexOf(Term.ToUpper());
			if(i != -1)
			{
				InString = InString.Substring(0,i) + "<b>" + InString.Substring(i,Term.Length) + "</b>" + InString.Substring(i+Term.Length,InString.Length-Term.Length-i);
			}
			return InString;
		}

		static public String BuildStarsImage(Single d, int _siteID)
		{
			String s = String.Empty;
			if(d < 0.25)
			{
				s = "<img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\">";
			}
			else if(d >=0.25 && d < 0.75)
			{
				s = "<img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starh.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\">";
			}
			else if(d >=0.75 && d < 1.25)
			{
				s = "<img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\">";
			}
			else if(d >=1.25 && d < 1.75)
			{
				s = "<img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starh.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\">";
			}
			else if(d >=1.75 && d < 2.25)
			{
				s = "<img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\">";
			}
			else if(d >=2.25 && d < 2.75)
			{
				s = "<img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starh.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\">";
			}
			else if(d >=2.75 && d < 3.25)
			{
				s = "<img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\">";
			}
			else if(d >=3.25 && d < 3.75)
			{
				s = "<img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starh.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\">";
			}
			else if(d >=3.75 && d < 4.25)
			{
				s = "<img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/stare.gif\">";
			}
			else if(d >=4.25 && d < 4.75)
			{
				s = "<img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starh.gif\">";
			}
			else if(d >=4.75)
			{
				s = "<img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/starf.gif\">";
			}
			return s;
		}

		static public String Left(String s, int l)
		{
			if(s.Length <= l)
			{
				return s;
			}
			return s.Substring(0,l-1);
		}

		// this really is never meant to be called with ridiculously  small l values (e.g. l < 10'ish)
		static public String Ellipses(String s, int l, bool BreakBetweenWords)
		{
			if(l < 1)
			{
				return String.Empty;
			}
			if(l >= s.Length)
			{
				return s;
			}
			String tmpS = Left(s,l-2);
			if(BreakBetweenWords)
			{
				try
				{
					tmpS = tmpS.Substring(0, tmpS.LastIndexOf(" "));
				}
				catch {}
			}
			tmpS += "...";
			return tmpS;
		}

		public static String GetCategoryName(int CategoryID)
		{
			IDataReader rs = DB.GetRS("Select * from Category  " + DB.GetNoLock() + " where CategoryID=" + CategoryID.ToString());
			String uname = String.Empty;
			if(rs.Read())
			{
				uname = DB.RSField(rs,"Name");
			}
			rs.Close();
			return uname;
		}

		public static String GetAffiliateName(int AffiliateID)
		{
			IDataReader rs = DB.GetRS("Select Name from Affiliate  " + DB.GetNoLock() + " where AffiliateID=" + AffiliateID.ToString());
			String uname = String.Empty;
			if(rs.Read())
			{
				uname = DB.RSField(rs,"Name");
			}
			rs.Close();
			return uname;
		}

		public static int GetAffiliateID(String Name)
		{
			IDataReader rs = DB.GetRS("Select AffiliateID from  affiliate " + DB.GetNoLock() + " where lower(Name)=" + DB.SQuote(Name.ToLower()));
			int tmp = 0;
			if(rs.Read())
			{
				tmp = DB.RSFieldInt(rs,"AffiliateID");
			}
			rs.Close();
			return tmp;
		}

		public static String GetManufacturerName(int ManufacturerID)
		{
			IDataReader rs = DB.GetRS("Select * from Manufacturer  " + DB.GetNoLock() + " where ManufacturerID=" + ManufacturerID.ToString());
			String uname = String.Empty;
			if(rs.Read())
			{
				uname = DB.RSField(rs,"Name");
			}
			rs.Close();
			return uname;
		}

		public static String GetSectionName(int SectionID)
		{
			IDataReader rs = DB.GetRS("Select * from [section]  " + DB.GetNoLock() + " where SectionID=" + SectionID.ToString());
			String uname = String.Empty;
			if(rs.Read())
			{
				uname = DB.RSField(rs,"Name");
			}
			rs.Close();
			return uname;
		}

		public static String AspHTTP(String url)
		{
			String result;
			try
			{
				WebResponse objResponse;
				WebRequest objRequest = System.Net.HttpWebRequest.Create(url);
				objResponse = objRequest.GetResponse();
				using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()) )
				{
					result = sr.ReadToEnd();
					// Close and clean up the StreamReader
					sr.Close();
				}
				objResponse.Close();
			}
			catch
			{
				result = String.Empty;
			}
			return result;
		}   

		public static String SelectOption(String activeValue, String oname, String fieldname)
		{
			if(activeValue == oname)
			{
				return " selected";
			}
			else
			{
				return String.Empty;
			}
		}

		public static String SelectOption(IDataReader rs, String oname, String fieldname)
		{
			return SelectOption(DB.RSField(rs,fieldname),oname,fieldname);
		}

		public static String MakeFullName(String fn, String ln)
		{
			String tmp = fn + " " + ln;
			return tmp.Trim();
		}

		public static String ExtractBody(String ss)
		{
			try
			{
				int startAt;
				int stopAt;
				startAt = ss.IndexOf("<body");
				if(startAt == -1)
				{
					startAt = ss.IndexOf("<BODY");
				}
				if(startAt == -1)
				{
					startAt = ss.IndexOf("<Body");
				}
				startAt = ss.IndexOf(">",startAt);
				stopAt = ss.IndexOf("</body>");
				if(stopAt == -1)
				{
					stopAt = ss.IndexOf("</BODY>");
				}
				if(stopAt == -1)
				{
					stopAt = ss.IndexOf("</Body>");
				}
				if(startAt == -1)
				{
					startAt = 1;
				}
				else
				{
					startAt = startAt + 1;
				}
				if(stopAt == -1)
				{
					stopAt = ss.Length;
				}
				return ss.Substring(startAt,stopAt-startAt);
			}
			catch
			{
				return String.Empty;
			}
		}

		public static void WriteFile(String fname, String contents)
		{
			if(fname.IndexOf(":\\") == -1 && fname.IndexOf("\\\\") == -1)
			{
				fname = HttpContext.Current.Server.MapPath(fname);
			}
			StreamWriter wr;
			if(Common.AppConfigBool("WriteFileInUTF8"))
			{
				wr = new StreamWriter(fname,false,System.Text.Encoding.UTF8 ,4096);
			}
			else
			{
				wr = new StreamWriter(fname,false,System.Text.Encoding.ASCII,4096);
			}
			wr.Write(contents);
			wr.Flush();
			wr.Close();
		}


		public static String ReadFile(String fname, bool ignoreErrors)
		{
			String contents;
			try
			{
				if(fname.IndexOf(@":\") == -1)
				{
					fname = HttpContext.Current.Server.MapPath(fname);
				}
				StreamReader rd = new StreamReader(fname);
				contents = rd.ReadToEnd();
				rd.Close();
				return contents;
			}
			catch (Exception e)
			{
				if(ignoreErrors)
					return String.Empty;
				else
					throw e;
			}
		}

		public static String Capitalize(String s)
		{
			if(s.Length == 0)
			{
				return String.Empty;
			}
			else if (s.Length == 1)
			{
				return s.ToUpper();
			}
			else
			{
				return s.Substring(0,1).ToUpper() + s.Substring(1,s.Length-1).ToLower();
			}
		}

		public static void SetCookie(String cookieName, String cookieVal, TimeSpan ts)
		{
			try
			{
				HttpCookie cookie = new HttpCookie(cookieName);
				cookie.Value = HttpContext.Current.Server.UrlEncode(cookieVal);
				DateTime dt = DateTime.Now;
				cookie.Expires = dt.Add(ts);
				if(Common.OnLiveServer())
				{
					cookie.Domain = Common.AppConfig("LiveServer");
				}
				HttpContext.Current.Response.Cookies.Add(cookie);
			}
			catch
			{}
		}

		public static void SetSessionCookie(String cookieName, String cookieVal)
		{
			try
			{
				HttpCookie cookie = new HttpCookie(cookieName);
				cookie.Value = HttpContext.Current.Server.UrlEncode(cookieVal);
				if(Common.OnLiveServer())
				{
					cookie.Domain = Common.AppConfig("LiveServer");
				}
				HttpContext.Current.Response.Cookies.Add(cookie);
			}
			catch
			{}
		}

		public static String ServerVariables(String paramName)
		{
			String tmpS = String.Empty;
			try
			{
				tmpS = HttpContext.Current.Request.ServerVariables[paramName].ToString();
			}
			catch
			{
				tmpS = String.Empty;
			}
			return tmpS;
		}

		public static String MakeProperProductName(String pname, String vname)
		{
			if( vname.Length == 0)
			{
				return pname;
			}
			else
			{
				return pname + " - " + vname;
			}
		}

		public static String MakeProperProductSKU(String pSKU, String vSKU, String colorMod, String sizeMod)
		{
			return pSKU + vSKU + colorMod + sizeMod;
		}

		public static bool FileExists(String fname)
		{
			if(fname.IndexOf(":") == -1 && fname.IndexOf("\\\\") == -1)
			{
				return File.Exists(HttpContext.Current.Server.MapPath(fname));
			}
			else
			{
				return File.Exists(fname);
			}
		}

		public static String ExtractToken(String ss, String t1, String t2)
		{
			if(ss.Length == 0)
			{
				return String.Empty;
			}
			int i1 = ss.IndexOf(t1);
			int i2 = ss.IndexOf(t2);
			if(i1 == -1 || i2 == -1 || i1 >= i2 || (i2 - i1) <= 0)
			{
				return String.Empty;
			}
			return ss.Substring(i1,i2-i1).Replace(t1,"");
		}

		static public void SendMail(String subject, String body, bool useHTML)
		{
			SendMail(subject,body,useHTML,Common.AppConfig("MailMe_FromAddress"),Common.AppConfig("MailMe_FromName"),Common.AppConfig("MailMe_ToAddress"),Common.AppConfig("MailMe_ToName"),String.Empty,Common.AppConfig("MailMe_Server"));
		}
		
		static public void SendMail(String subject, String body, bool useHTML, String fromaddress, String fromname, String toaddress, String toname, String bccaddresses, String server)
		{
#if SMTPDOTNET
			// SMTP.NET COMPONENT:
			SmtpServer smtp = new SmtpServer();  
			if(server.Length != 0)
			{
				smtp.ServerAddress = server;
			}
			if(Common.AppConfig("MailMe_Pwd").Length != 0 && Common.AppConfig("MailMe_User").Length != 0)
			{
				smtp.AuthLogin = Common.AppConfig("MailMe_User");
				smtp.AuthPassword = Common.AppConfig("MailMe_Pwd");
			}

			smtp.FromAddress = fromaddress; 
			if(fromname.Length != 0)
			{
				smtp.FromFriendly = fromname;
			}
			smtp.ToAddress = toaddress; 
			if(toname.Length != 0)
			{
				smtp.ToFriendly = toname;
			}
			if(bccaddresses.Length != 0)
			{
				if(bccaddresses.IndexOf(";") != -1)
				{
					String[] bcclist = bccaddresses.Split(';');
					foreach(String bccS in bcclist)
					{
						smtp.AddRecipient(bccS,bccS,SmtpDotNet.AddressTypes.BCC);
					}
				}
				else if(bccaddresses.IndexOf(",") != -1)
				{
					String[] bcclist = bccaddresses.Split(',');
					foreach(String bccS in bcclist)
					{
						smtp.AddRecipient(bccS,bccS,SmtpDotNet.AddressTypes.BCC);
					}
				}
				else
				{
					smtp.BCCAddress = bccaddresses;
				}
			}
			smtp.BodyFormat = (BodyFormatTypes)Common.IIF(useHTML , (int)BodyFormatTypes.HTML , (int)BodyFormatTypes.PLAIN);
			smtp.Subject = subject;
			smtp.Body = body;
			ReturnCodes nRC = smtp.Send(); 
			if (nRC != ReturnCodes.SUCCESS || smtp.EmailCountBad != 0 )
			{
				throw new ArgumentException("Mail Error #" + nRC + " occurred - " + smtp.LastError + " - rejected " + smtp.EmailCountBad + " Messages");
			}
#else
			// BUILT IN .NET MAIL:
			MailMessage msg = new MailMessage();
			msg.To = toaddress;
			if(bccaddresses.Length != 0)
			{
				msg.Bcc = bccaddresses;
			}
			msg.From = fromaddress;
			msg.Body = body;
			msg.Subject = subject;
			msg.BodyFormat = (MailFormat)Common.IIF(useHTML , (int)MailFormat.Html , (int)MailFormat.Text);

			if(Common.AppConfig("MailMe_User").Length != 0 && Common.AppConfig("MailMe_Pwd").Length != 0)
			{
				// for authentication, add the following lines of code 0 = None / 1 = Basic / 2 = NTLM
				msg.Fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"] = 1;
				msg.Fields["http://schemas.microsoft.com/cdo/configuration/sendusername"] =	Common.AppConfig("MailMe_User");
				msg.Fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"] =	Common.AppConfig("MailMe_Pwd");
			}

			SmtpMail.SmtpServer = server;
			try
			{
				SmtpMail.Send(msg);
			}
			catch(Exception ex)
			{
				throw new ArgumentException("Mail Error occurred - " + Common.GetExceptionDetail(ex,"<br>"));
			}
#endif
		}
								 



		static public void SetField(DataSet ds, String fieldname)
		{
			ds.Tables["Customers"].Rows[0][fieldname] = Common.Form(fieldname);
		}
		
		static public bool HasBadWords(String s)
		{
			s = s.ToUpper();
			IDataReader rs = DB.GetRS("select upper(Word) as BadWord from RatingBadWords " + DB.GetNoLock());
			while(rs.Read())
			{
				if(s.IndexOf(DB.RSField(rs,"BadWord")) != -1)
				{
					rs.Close();
					return true;
				}
			}
			rs.Close();
			return false;
		}

		static public String MakeSafeJavascriptName(String s)
		{
			String OkChars = "abcdefghijklmnopqrstuvwxyz1234567890_";
			s = s.ToLower();
			StringBuilder tmpS = new StringBuilder(s.Length);
			for(int i = 0; i < s.Length; i++)
			{
				String tok = s.Substring(i,1);
				if(OkChars.IndexOf(tok) != -1)
				{
					tmpS.Append(tok);
				}
			}
			return tmpS.ToString();
		}

		static public String MakeSafeFilesystemName(String s)
		{
			String OkChars = "abcdefghijklmnopqrstuvwxyz1234567890_";
			s = s.ToLower();
			StringBuilder tmpS = new StringBuilder(s.Length);
			for(int i = 0; i < s.Length; i++)
			{
				String tok = s.Substring(i,1);
				if(OkChars.IndexOf(tok) != -1)
				{
					tmpS.Append(tok);
				}
			}
			return tmpS.ToString();
		}

		static public String MakeSafeJavascriptString(String s)
		{
			return s.Replace("'","\\");
		}

		public static void ReadWholeArray (Stream stream, byte[] data)
		{
			/// <summary>
			/// Reads data into a complete array, throwing an EndOfStreamException
			/// if the stream runs out of data first, or if an IOException
			/// naturally occurs.
			/// </summary>
			/// <param name="stream">The stream to read data from</param>
			/// <param name="data">The array to read bytes into. The array
			/// will be completely filled from the stream, so an appropriate
			/// size must be given.</param>
			int offset=0;
			int remaining = data.Length;
			while (remaining > 0)
			{
				int read = stream.Read(data, offset, remaining);
				if (read <= 0)
				{
					return;
				}
				remaining -= read;
			}
		}

		public static byte[] ReadFully (Stream stream)
		{
			/// <summary>
			/// Reads data from a stream until the end is reached. The
			/// data is returned as a byte array. An IOException is
			/// thrown if any of the underlying IO calls fail.
			/// </summary>
			byte[] buffer = new byte[32768];
			using (MemoryStream ms = new MemoryStream())
			{
				while (true)
				{
					int read = stream.Read (buffer, 0, buffer.Length);
					if (read <= 0)
						return ms.ToArray();
					ms.Write (buffer, 0, read);
				}
			}
		}

		static public int ScreenWidth()
		{
			int SW = Common.CookieUSInt("ScreenWidth");
			if(SW == 0)
			{
				SW = 800; // default
			}
			return SW;
		}

		static public int ScreenHeight()
		{
			int SW = Common.CookieUSInt("ScreenHeight");
			if(SW == 0)
			{
				SW = 600; // default
			}
			return SW;
		}
		static public int ClientWidth()
		{
			int SW = Common.CookieUSInt("ClientWidth");
			if(SW == 0)
			{
				SW = 800; // default
			}
			return SW;
		}

		static public int ClientHeight()
		{
			int SW = Common.CookieUSInt("ClientHeight");
			if(SW == 0)
			{
				SW = 600; // default
			}
			return SW;
		}

		static public int GetImageWidth(String imgname)
		{
			//create instance of Bitmap class around specified image file
			// must use try/catch in case the image file is bogus
			try
			{
				Bitmap img = new Bitmap(HttpContext.Current.Server.MapPath(imgname), false);
				int tmp = img.Width;
				img.Dispose();
				img = null;
				return tmp;
			}
			catch
			{
				return 0;
			}
		}

		static public int GetImageHeight(String imgname)
		{
			try
			{
				//create instance of Bitmap class around specified image file
				// must use try/catch in case the image file is bogus
				Bitmap img = new Bitmap(HttpContext.Current.Server.MapPath(imgname), false);
				int tmp = img.Height;
				img.Dispose();
				img = null;
				return tmp;
			}
			catch
			{
				return 0;
			}
		}

		static public String WrapString(String s, int ColWidth, String Separator)
		{
			StringBuilder tmpS = new StringBuilder(s.Length+100);
			if(s.Length <= ColWidth || ColWidth == 0)
			{
				return s;
			}
			int start = 0;
			int length = Min(ColWidth,s.Length);
			while(start < s.Length)
			{
				if(tmpS.Length != 0)
				{
					tmpS.Append(Separator);
				}
				tmpS.Append(s.Substring(start,length));
				start += ColWidth;
				length = Min(ColWidth,s.Length-start);
			}
			return tmpS.ToString();
		}

		public static String PrettyPrintXml(String Xml)
		{
			String Result = "";

			Xml = Xml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>","");


			MemoryStream MS = new MemoryStream();
			XmlTextWriter W = new XmlTextWriter(MS, Encoding.Unicode);
			XmlDocument D   = new XmlDocument();

			// Load the XmlDocument with the Xml.
			D.LoadXml(Xml);

			W.Formatting = Formatting.Indented;

			// Write the Xml into a formatting XmlTextWriter
			D.WriteContentTo(W);
			W.Flush();
			MS.Flush();

			// Have to rewind the MemoryStream in order to read
			// its contents.
			MS.Position = 0;

			// Read MemoryStream contents into a StreamReader.
			StreamReader SR = new StreamReader(MS);

			// Extract the text from the StreamReader.
			String FormattedXml = SR.ReadToEnd();

			Result = FormattedXml;

			try
			{
				MS.Close();
				MS = null;
				W.Close();
				W = null;
			}
			catch {}

			return Result;
		}

		static public bool CommissionRecorded(int OrderNumber)
		{
			bool tmp = false;
			IDataReader rs = DB.GetRS("Select AffiliateCommissionRecorded from orders  " + DB.GetNoLock() + " where ordernumber=" + OrderNumber.ToString());
			if(rs.Read())
			{
				tmp = DB.RSFieldBool(rs,"AffiliateCommissionRecorded");
			}
			rs.Close();
			return tmp;
		}

		static public bool OrderHasCleared(int OrderNumber)
		{
			bool tmp = false;
			IDataReader rs = DB.GetRS("Select PaymentClearedOn from orders  " + DB.GetNoLock() + " where ordernumber=" + OrderNumber.ToString());
			if(rs.Read())
			{
				tmp = (DB.RSFieldDateTime(rs,"PaymentClearedOn") != System.DateTime.MinValue);
			}
			rs.Close();
			return tmp;
		}

		public static void DeleteOrder(int OrderNumber)
		{
			// should unwind any special logic here also (i.e. affiliate sales, commissions, etc that may have been added to the site!)

			DB.ExecuteSQL("delete from orders_customcart where OrderNumber=" + OrderNumber.ToString());
			DB.ExecuteSQL("delete from orders_kitcart where OrderNumber=" + OrderNumber.ToString());
			DB.ExecuteSQL("delete from orders_ShoppingCart where OrderNumber=" + OrderNumber.ToString());
			DB.ExecuteSQL("delete from orders where OrderNumber=" + OrderNumber.ToString());
		}

		public static void AdminDeleteOrphanedOrders()
		{
			DB.ExecuteSQL("delete from orders where ordernumber not in (select ordernumber from orders_ShoppingCart) and ordernumber not in (select ordernumber from orders_customcart) and ordernumber not in (select ordernumber from orders_kitcart)");
			DB.ExecuteSQL("delete from orders_customcart where ordernumber not in (select ordernumber from orders)");
			DB.ExecuteSQL("delete from orders_kitcart where ordernumber not in (select ordernumber from orders)");
			DB.ExecuteSQL("delete from orders_ShoppingCart where ordernumber not in (select ordernumber from orders)");
		}

		public static String GetNewGUID()
		{
			return System.Guid.NewGuid().ToString();
		}
		
		static public String GetNewsSummary(int ShowNum)
		{
			String CacheName = "GetNewsSummary" + ShowNum.ToString();
			bool CachingOn = Common.AppConfigBool("CacheMenus");
			if(CachingOn)
			{
				String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
				if(Menu != null)
				{
					if(Common.ApplicationBool("DumpSQL"))
					{
						HttpContext.Current.Response.Write("Cache Hit Found!<br>\n");
					}
					return Menu;
				}
			}

			StringBuilder tmpS = new StringBuilder(5000);
			String sql = "select * from news  " + DB.GetNoLock() + " where deleted=0 order by createdon desc";
			IDataReader rs = DB.GetRS(sql);
			tmpS.Append("			<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\n");
			bool anyFound = false;
			int i = 1;
			while(rs.Read() && i <= ShowNum)
			{
				tmpS.Append("				<tr><td valign=\"top\"><img align=\"absmiddle\" src=\"skins/skin_%SKINID%/images/y.gif\" hspace=\"10\"></td><td><a class=\"NewsItem\" href=\"news.aspx\">" + Common.Ellipses(DB.RSField(rs,"NewsCopy"),90,true) + "</a></td></tr>\n");
				tmpS.Append("				<tr><td height=\"5\"></td></tr>\n");
				anyFound = true;
				i++;
			}
			rs.Close();
			if(!anyFound)
			{
				tmpS.Append("				<tr><td valign=\"top\"><img align=\"absmiddle\" src=\"skins/skin_%SKINID%/images/y.gif\" hspace=\"10\"></td><td><font class=\"NewsItem\">No News Items</font></td></tr>\n");
				tmpS.Append("				<tr><td height=\"5\"></td></tr>\n");
			}
			tmpS.Append("			</table>\n");

			if(CachingOn)
			{
				HttpContext.Current.Cache.Insert(CacheName,tmpS.ToString(),null,System.DateTime.Now.AddHours(1),TimeSpan.Zero);
			}
			return tmpS.ToString();
		}

		static public String HtmlEncode(String S)
		{
			String result = String.Empty;
			for(int i = 0; i < S.Length; i++)
			{
				String c = S.Substring(i,1);
				int acode =(int)c[0];
				if(acode < 32 || acode > 127)
				{
					result += "&#" + acode.ToString() + ";";
				}
				else
				{
					switch(acode)
					{
						case 32:
							result += "&nbsp;";
							break;
						case 34:
							result += "&quot;";
							break;
						case 38:
							result += "&amp;";
							break;
						case 60:
							result += "&lt;";
							break;
						case 62:
							result += "&gt;";
							break;
						default:
							result += c;
							break;
					}
				}
			}
			return result;
		}

		
		static public String XmlEncode(String S)
		{
			String result = String.Empty;
			for(int i = 0; i < S.Length; i++)
			{
				String c = S.Substring(i,1);
				switch(c)
				{
					case "\"":
						result += "&quot;";
						break;
					case "'":
						result += "&apos;";
						break;
					case "&":
						result += "&amp;";
						break;
					case "<":
						result += "&lt;";
						break;
					case ">":
						result += "&gt;";
						break;
					default:
						result += c;
						break;
				}
			}
			return result;
		}

		// this version is NOT to be used to squote db sql stuff!
		public static String SQuote(String s)
		{
			return "'" + s.Replace("'","''") + "'";
		}

		static public String GetCountryName(String CountryTwoLetterISOCode)
		{
			String tmp = "United States"; // default to US just in case
			IDataReader rs = DB.GetRS("select * from country  " + DB.GetNoLock() + " where lower(TwoLetterISOCode)=" + DB.SQuote(CountryTwoLetterISOCode.ToLower()));
			if(rs.Read())
			{
				tmp = DB.RSField(rs,"name");
			}
			rs.Close();
			return tmp;
		}

		/// <summary>
		/// //Returns true if the Customer has previously purchased this product.
		/// </summary>
		public static bool Owns(int ProductID)
		{
			int nCount = 0;
      
			//VIP users have total access
			if (HttpContext.Current.User.IsInRole("VIP")) return true;
      
			nCount = DB.GetSqlN(String.Format("select count(*) as N from orders_ShoppingCart  " + DB.GetNoLock() + " where CustomerID={0} and ProductID={1}",Common.SessionUSInt("CustomerID"),ProductID));
			if (nCount != 0) return true;
			nCount = DB.GetSqlN(String.Format("select count(*) as N from orders_customcart  " + DB.GetNoLock() + " where CustomerID={0} and ProductID={1}",Common.SessionUSInt("CustomerID"),ProductID));
			if (nCount != 0) return true;
			return false;
		}

		/// <summary>
		/// Sets the Roles of the logged in user and adds then to their security Principal
		/// This must be called in Application_AuthenticateRequest of Global.asax
		/// </summary>
		public static void SetRoles()
		{
			if (HttpContext.Current.Request.IsAuthenticated) //We know who they are
			{
				ArrayList roleList = new ArrayList(50); //List of Role Strings
				string CustomerGUID = HttpContext.Current.User.Identity.Name;
				int CustomerID = 0;
				int CustomerLevelID = 0;
				DateTime SubscriptionExpiresOn = DateTime.Now;

				IDataReader rs = null;
				try
				{
					if (CustomerGUID.Length != 0)
					{

						rs = DB.GetRS("select * from customer  " + DB.GetNoLock() + " where deleted=0 and CustomerGUID=" + DB.SQuote(CustomerGUID));
						if (rs.Read())
						{
							CustomerID = DB.RSFieldInt(rs,"CustomerID");
							// get the CustomerLevelID for later
							CustomerLevelID = DB.RSFieldInt(rs,"CustomerLevelID");
							SubscriptionExpiresOn = DB.RSFieldDateTime(rs,"SubscriptionExpiresOn");
						}
						else
						{
							CustomerGUID = String.Empty; // some kind of error, return blank info
						}
						rs.Close();
					}
				}
				catch{}

				// Add whatever role strings required.
				// UserLevel string is a good possibility. Allow access passed on userlevel.
				// Use the SKUs of products the user has purchased as their Role strings. 
				// This way the SKU can be added to the Web.Config <authorization> section to allow acces in a protected directory.
				if (CustomerGUID.Length != 0)
				{

					// Admins and super users rule!
					if (Customer.StaticIsAdminUser(CustomerID))
					{
						roleList.Add("Admin");
					}
					if (Customer.StaticIsAdminSuperUser(CustomerID))
					{
						roleList.Add("SuperAdmin");
					}
					//Check Subscriber Expiration
					if (SubscriptionExpiresOn.CompareTo(DateTime.Now) > 0)
					{
						roleList.Add("Subscriber");
					}
          
					try
					{
						if (CustomerLevelID != 0)
						{
							rs = DB.GetRS(String.Format("select Name from CustomerLevel  " + DB.GetNoLock() + " where CustomerLevelID={0}",CustomerLevelID));
							while (rs.Read())
							{
								roleList.Add(DB.RSField(rs,"Name").Trim());
							}
							rs.Close();
						}
						//Get SKUs for orders_ShoppingCart
						rs = DB.GetRS(String.Format("select OrderedProductSKU from orders_ShoppingCart  " + DB.GetNoLock() + " where CustomerID={0}",CustomerID));
						while (rs.Read())
						{
							roleList.Add(DB.RSField(rs,"OrderedProductSKU").Trim());
						}
						rs.Close();
						//Get SKUs for orders_customcart
						rs = DB.GetRS(String.Format("select ProductSKU from orders_customcart  " + DB.GetNoLock() + " where CustomerID={0}",CustomerID));
						while (rs.Read())
						{
							roleList.Add(DB.RSField(rs,"ProductSKU").Trim());
						}
						rs.Close();
					}
					catch{}
					if (roleList.Count > 0)
					{
						string[] roles = (string[])roleList.ToArray(typeof(string));
						HttpContext.Current.User = new GenericPrincipal(HttpContext.Current.User.Identity,roles);
					}
				}
			}
		}

		//V3_9
		/// <summary>
		/// Gets the ProductID of MicroPay Product if it is being used.
		/// </summary>
		//V3_9
		public static int GetMicroPayProductID()
		{
			int result = 0;
      
			IDataReader rs = DB.GetRS("select ProductID from Product  " + DB.GetNoLock() + " where SKU='MICROPAY'"); 
			if (rs.Read())
			{
				result = DB.RSFieldInt(rs,"ProductID");
			}
			rs.Close();
      
			return result;
		}

		//V3_9
		/// <summary>
		/// Gets the the MicroPay  Balance for a given customer.
		/// </summary>
		public static decimal GetMicroPayBalance(int CustomerID)
		{
			decimal result = System.Decimal.Zero;
      
			IDataReader rs = DB.GetRS(String.Format("select MicroPayBalance from Customer  " + DB.GetNoLock() + " where CustomerID={0}",CustomerID)); 
			if (rs.Read())
			{
				result = DB.RSFieldDecimal(rs,"MicroPayBalance");
			}
			rs.Close();
      
			return result;
		}

		// ----------------------------------------------------------------
		//
		// SIMPLE APPCONFIG ROUTINES
		//
		// ----------------------------------------------------------------
		public static String AppConfig(String paramName)
		{
			String tmpS = String.Empty;
			try
			{
				tmpS = AppConfigTable[paramName.ToLower()].ToString();
			}
			catch
			{
				tmpS = String.Empty;
			}
			return tmpS;
		}
		
		public static bool AppConfigBool(String paramName)
		{
			String tmp = AppConfig(paramName).ToLower();
			if(tmp == "true" || tmp == "yes" || tmp == "1")
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static int AppConfigUSInt(String paramName)
		{
			String tmpS = AppConfig(paramName);
			return Localization.ParseUSInt(tmpS);
		}

		public static long AppConfigUSLong(String paramName)
		{
			String tmpS = AppConfig(paramName);
			return Localization.ParseUSLong(tmpS);
		}

		public static Single AppConfigUSSingle(String paramName)
		{
			String tmpS = AppConfig(paramName);
			return Localization.ParseUSSingle(tmpS);
		}

		public static Double AppConfigUSDouble(String paramName)
		{
			String tmpS = AppConfig(paramName);
			return Localization.ParseUSDouble(tmpS);
		}

		public static Decimal AppConfigUSDecimal(String paramName)
		{
			String tmpS = AppConfig(paramName);
			return Localization.ParseUSDecimal(tmpS);
		}

		public static DateTime AppConfigUSDateTime(String paramName)
		{
			return Localization.ParseUSDateTime(AppConfig(paramName));
		}

		public static int AppConfigNativeInt(String paramName)
		{
			String tmpS = AppConfig(paramName);
			return Localization.ParseNativeInt(tmpS);
		}

		public static long AppConfigNativeLong(String paramName)
		{
			String tmpS = AppConfig(paramName);
			return Localization.ParseNativeLong(tmpS);
		}

		public static Single AppConfigNativeSingle(String paramName)
		{
			String tmpS = AppConfig(paramName);
			return Localization.ParseNativeSingle(tmpS);
		}

		public static Double AppConfigNativeDouble(String paramName)
		{
			String tmpS = AppConfig(paramName);
			return Localization.ParseNativeDouble(tmpS);
		}

		public static Decimal AppConfigNativeDecimal(String paramName)
		{
			String tmpS = AppConfig(paramName);
			return Localization.ParseNativeDecimal(tmpS);
		}

		public static DateTime AppConfigNativeDateTime(String paramName)
		{
			return Localization.ParseNativeDateTime(AppConfig(paramName));
		}

		// ----------------------------------------------------------------
		//
		// SIMPLE FORM ROUTINES
		//
		// ----------------------------------------------------------------

		public static String Form(String paramName)
		{
			String tmpS = String.Empty;
			try
			{
				tmpS = HttpContext.Current.Request.Form[paramName].ToString();
			}
			catch
			{
				tmpS = String.Empty;
			}
			return tmpS;
		}
		
		public static bool FormBool(String paramName)
		{
			String tmpS = Common.Form(paramName).ToLower();
			if(tmpS == "true" || tmpS == "yes" || tmpS == "1")
			{
				return true;
			}
			return false;
		}

		public static int FormUSInt(String paramName)
		{
			String tmpS = Form(paramName);
			return Localization.ParseUSInt(tmpS);
		}

		public static long FormUSLong(String paramName)
		{
			String tmpS = Form(paramName);
			return Localization.ParseUSLong(tmpS);
		}

		public static Single FormUSSingle(String paramName)
		{
			String tmpS = Form(paramName);
			return Localization.ParseUSSingle(tmpS);
		}

		public static Double FormUSDouble(String paramName)
		{
			String tmpS = Form(paramName);
			return Localization.ParseUSDouble(tmpS);
		}

		public static decimal FormUSDecimal(String paramName)
		{
			String tmpS = Form(paramName);
			return Localization.ParseUSDecimal(tmpS);
		}

		public static DateTime FormUSDateTime(String paramName)
		{
			String tmpS = Form(paramName);
			return Localization.ParseUSDateTime(tmpS);
		}

		public static int FormNativeInt(String paramName)
		{
			String tmpS = Form(paramName);
			return Localization.ParseNativeInt(tmpS);
		}

		public static long FormNativeLong(String paramName)
		{
			String tmpS = Form(paramName);
			return Localization.ParseNativeLong(tmpS);
		}

		public static Single FormNativeSingle(String paramName)
		{
			String tmpS = Form(paramName);
			return Localization.ParseNativeSingle(tmpS);
		}

		public static Double FormNativeDouble(String paramName)
		{
			String tmpS = Form(paramName);
			return Localization.ParseNativeDouble(tmpS);
		}

		public static decimal FormNativeDecimal(String paramName)
		{
			String tmpS = Form(paramName);
			return Localization.ParseNativeDecimal(tmpS);
		}

		public static DateTime FormNativeDateTime(String paramName)
		{
			String tmpS = Form(paramName);
			return Localization.ParseNativeDateTime(tmpS);
		}

		// ----------------------------------------------------------------
		//
		// SIMPLE QUERYSTRING ROUTINES
		//
		// ----------------------------------------------------------------
		public static String QueryString(String paramName)
		{
			String tmpS = String.Empty;
			try
			{
				tmpS = HttpContext.Current.Request.QueryString[paramName].ToString();
			}
			catch
			{
				tmpS = String.Empty;
			}
			return tmpS;
		}

		public static bool QueryStringBool(String paramName)
		{
			String tmpS = Common.QueryString(paramName).ToLower();
			if(tmpS == "true" || tmpS == "yes" || tmpS == "1")
			{
				return true;
			}
			return false;
		}

		public static int QueryStringUSInt(String paramName)
		{
			String tmpS = QueryString(paramName);
			return Localization.ParseUSInt(tmpS);
		}

		public static long QueryStringUSLong(String paramName)
		{
			String tmpS = QueryString(paramName);
			return Localization.ParseUSLong(tmpS);
		}

		public static Single QueryStringUSSingle(String paramName)
		{
			String tmpS = QueryString(paramName);
			return Localization.ParseUSSingle(tmpS);
		}

		public static Double QueryStringUSDouble(String paramName)
		{
			String tmpS = QueryString(paramName);
			return Localization.ParseUSDouble(tmpS);
		}

		public static decimal QueryStringUSDecimal(String paramName)
		{
			String tmpS = QueryString(paramName);
			return Localization.ParseUSDecimal(tmpS);
		}

		public static DateTime QueryStringUSDateTime(String paramName)
		{
			String tmpS = QueryString(paramName);
			return Localization.ParseUSDateTime(tmpS);
		}

		public static int QueryStringNativeInt(String paramName)
		{
			String tmpS = QueryString(paramName);
			return Localization.ParseNativeInt(tmpS);
		}

		public static long QueryStringNativeLong(String paramName)
		{
			String tmpS = QueryString(paramName);
			return Localization.ParseNativeLong(tmpS);
		}

		public static Single QueryStringNativeSingle(String paramName)
		{
			String tmpS = QueryString(paramName);
			return Localization.ParseNativeSingle(tmpS);
		}

		public static Double QueryStringNativeDouble(String paramName)
		{
			String tmpS = QueryString(paramName);
			return Localization.ParseNativeDouble(tmpS);
		}

		public static decimal QueryStringNativeDecimal(String paramName)
		{
			String tmpS = QueryString(paramName);
			return Localization.ParseNativeDecimal(tmpS);
		}

		public static DateTime QueryStringNativeDateTime(String paramName)
		{
			String tmpS = QueryString(paramName);
			return Localization.ParseNativeDateTime(tmpS);
		}

		// ----------------------------------------------------------------
		//
		// SIMPLE SESSION ROUTINES
		//
		// ----------------------------------------------------------------
		public static String Session(String paramName)
		{
			String tmpS = String.Empty;
			try
			{
				tmpS = HttpContext.Current.Session[paramName].ToString();
			}
			catch
			{
				tmpS = String.Empty;
			}
			return tmpS;
		}

		public static bool SessionBool(String paramName)
		{
			String tmpS = Common.Session(paramName).ToLower();
			if(tmpS == "true" || tmpS == "yes" || tmpS == "1")
			{
				return true;
			}
			return false;
		}

		public static int SessionUSInt(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseUSInt(tmpS);
		}

		public static long SessionUSLong(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseUSLong(tmpS);
		}

		public static Single SessionUSSingle(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseUSSingle(tmpS);
		}

		public static Double SessionUSDouble(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseUSDouble(tmpS);
		}

		public static Decimal SessionUSDecimal(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseUSDecimal(tmpS);
		}

		public static DateTime SessionUSDateTime(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseUSDateTime(tmpS);
		}

		public static int SessionNativeInt(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseNativeInt(tmpS);
		}

		public static long SessionNativeLong(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseNativeLong(tmpS);
		}

		public static Single SessionNativeSingle(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseNativeSingle(tmpS);
		}

		public static Double SessionNativeDouble(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseNativeDouble(tmpS);
		}

		public static Decimal SessionNativeDecimal(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseNativeDecimal(tmpS);
		}

		public static DateTime SessionNativeDateTime(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseNativeDateTime(tmpS);
		}

		// ----------------------------------------------------------------
		//
		// SIMPLE APPLICATION ROUTINES
		//
		// ----------------------------------------------------------------
		
		public static String Application(String paramName)
		{
			String tmpS = String.Empty;
			try
			{
				tmpS = ConfigurationSettings.AppSettings[paramName].ToString();
			}
			catch
			{
				tmpS = String.Empty;
			}
			return tmpS;
		}

		public static bool ApplicationBool(String paramName)
		{
			String tmpS = Common.Application(paramName).ToLower();
			if(tmpS == "true" || tmpS == "yes" || tmpS == "1")
			{
				return true;
			}
			return false;
		}

		public static int ApplicationUSInt(String paramName)
		{
			String tmpS = Application(paramName);
			return Localization.ParseUSInt(tmpS);
		}

		public static long ApplicationUSLong(String paramName)
		{
			String tmpS = Application(paramName);
			return Localization.ParseUSLong(tmpS);
		}

		public static Single ApplicationUSSingle(String paramName)
		{
			String tmpS = Application(paramName);
			return Localization.ParseUSSingle(tmpS);
		}

		public static Double ApplicationUSDouble(String paramName)
		{
			String tmpS = Application(paramName);
			return Localization.ParseUSDouble(tmpS);
		}

		public static Decimal ApplicationUSDecimal(String paramName)
		{
			String tmpS = Application(paramName);
			return Localization.ParseUSDecimal(tmpS);
		}

		public static DateTime ApplicationUSDateTime(String paramName)
		{
			String tmpS = Application(paramName);
			return Localization.ParseUSDateTime(tmpS);
		}

		public static int ApplicationNativeInt(String paramName)
		{
			String tmpS = Application(paramName);
			return Localization.ParseNativeInt(tmpS);
		}

		public static long ApplicationNativeLong(String paramName)
		{
			String tmpS = Application(paramName);
			return Localization.ParseNativeLong(tmpS);
		}

		public static Single ApplicationNativeSingle(String paramName)
		{
			String tmpS = Application(paramName);
			return Localization.ParseNativeSingle(tmpS);
		}

		public static Double ApplicationNativeDouble(String paramName)
		{
			String tmpS = Application(paramName);
			return Localization.ParseNativeDouble(tmpS);
		}

		public static Decimal ApplicationNativeDecimal(String paramName)
		{
			String tmpS = Application(paramName);
			return Localization.ParseNativeDecimal(tmpS);
		}

		public static DateTime ApplicationNativeDateTime(String paramName)
		{
			String tmpS = Application(paramName);
			return Localization.ParseNativeDateTime(tmpS);
		}


		// ----------------------------------------------------------------
		//
		// SIMPLE COOKIE ROUTINES
		//
		// ----------------------------------------------------------------
		public static String Cookie(String paramName, bool decode)
		{
			try
			{
				String tmp = HttpContext.Current.Request.Cookies[paramName].Value.ToString();
				if(decode)
				{
					tmp = HttpContext.Current.Server.UrlDecode(tmp);
				}
				return tmp;
			}
			catch
			{
				return String.Empty;
			}
		}

		public static bool CookieBool(String paramName)
		{
			String tmpS = Common.Cookie(paramName,true).ToLower();
			if(tmpS == "true" || tmpS == "yes" || tmpS == "1")
			{
				return true;
			}
			return false;
		}

		public static int CookieUSInt(String paramName)
		{
			String tmpS = Cookie(paramName,true);
			return Localization.ParseUSInt(tmpS);
		}

		public static long CookieUSLong(String paramName)
		{
			String tmpS = Cookie(paramName,true);
			return Localization.ParseUSLong(tmpS);
		}

		public static Single CookieUSSingle(String paramName)
		{
			String tmpS = Cookie(paramName,true);
			return Localization.ParseUSSingle(tmpS);
		}

		public static Double CookieUSDouble(String paramName)
		{
			String tmpS = Cookie(paramName,true);
			return Localization.ParseUSDouble(tmpS);
		}

		public static Decimal CookieUSDecimal(String paramName)
		{
			String tmpS = Cookie(paramName,true);
			return Localization.ParseUSDecimal(tmpS);
		}

		public static DateTime CookieUSDateTime(String paramName)
		{
			String tmpS = Cookie(paramName,true);
			return Localization.ParseUSDateTime(tmpS);
		}

		public static int CookieNativeInt(String paramName)
		{
			String tmpS = Cookie(paramName,true);
			return Localization.ParseNativeInt(tmpS);
		}

		public static long CookieNativeLong(String paramName)
		{
			String tmpS = Cookie(paramName,true);
			return Localization.ParseNativeLong(tmpS);
		}

		public static Single CookieNativeSingle(String paramName)
		{
			String tmpS = Cookie(paramName,true);
			return Localization.ParseNativeSingle(tmpS);
		}

		public static Double CookieNativeDouble(String paramName)
		{
			String tmpS = Cookie(paramName,true);
			return Localization.ParseNativeDouble(tmpS);
		}

		public static Decimal CookieNativeDecimal(String paramName)
		{
			String tmpS = Cookie(paramName,true);
			return Localization.ParseNativeDecimal(tmpS);
		}

		public static DateTime CookieNativeDateTime(String paramName)
		{
			String tmpS = Cookie(paramName,true);
			return Localization.ParseNativeDateTime(tmpS);
		}


		// ----------------------------------------------------------------
		//
		// SIMPLE XML FIELD ROUTINES
		//
		// ----------------------------------------------------------------

		public static String XmlField(XmlNode node, String fieldName)
		{
			String fieldVal = String.Empty;
			try
			{
				fieldVal = node.SelectSingleNode(@fieldName).InnerText.Trim();
			}
			catch {} // node might not be there
			return fieldVal;
		}

		public static bool XmlFieldBool(XmlNode node, String fieldName)
		{
			String tmp = XmlField(node,fieldName).ToLower();
			if(tmp == "true" || tmp == "yes" || tmp == "1")
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static int XmlFieldUSInt(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseUSInt(tmpS);
		}

		public static long XmlFieldUSLong(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseUSLong(tmpS);
		}

		public static Single XmlFieldUSSingle(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseUSSingle(tmpS);
		}

		public static Double XmlFieldUSDouble(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseUSDouble(tmpS);
		}

		public static decimal XmlFieldUSDecimal(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseUSCurrency(tmpS);
		}

		public static DateTime XmlFieldUSDateTime(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseUSDateTime(tmpS);
		}

		public static int XmlFieldNativeInt(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseNativeInt(tmpS);
		}

		public static long XmlFieldNativeLong(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseNativeLong(tmpS);
		}

		public static Single XmlFieldNativeSingle(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseNativeSingle(tmpS);
		}

		public static Double XmlFieldNativeDouble(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseNativeDouble(tmpS);
		}

		public static decimal XmlFieldNativeDecimal(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseNativeDecimal(tmpS);
		}

		public static DateTime XmlFieldNativeDateTime(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseNativeDateTime(tmpS);
		}

		// ----------------------------------------------------------------
		//
		// SIMPLE XML ATTRIBUTE ROUTINES
		//
		// ----------------------------------------------------------------

		public static String XmlAttribute(XmlNode node, String AttributeName)
		{
			String AttributeVal = String.Empty;
			try
			{
				AttributeVal = node.Attributes[AttributeName].InnerText.Trim();
			}
			catch {} // node might not be there
			return AttributeVal;
		}

		public static bool XmlAttributeBool(XmlNode node, String AttributeName)
		{
			String tmp = XmlAttribute(node,AttributeName).ToLower();
			if(tmp == "true" || tmp == "yes" || tmp == "1")
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static int XmlAttributeUSInt(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseUSInt(tmpS);
		}

		public static long XmlAttributeUSLong(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseUSLong(tmpS);
		}

		public static Single XmlAttributeUSSingle(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseUSSingle(tmpS);
		}

		public static Double XmlAttributeUSDouble(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseUSDouble(tmpS);
		}

		public static decimal XmlAttributeUSDecimal(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseUSDecimal(tmpS);
		}

		public static DateTime XmlAttributeUSDateTime(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseUSDateTime(tmpS);
		}

		public static int XmlAttributeNativeInt(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseNativeInt(tmpS);
		}

		public static long XmlAttributeNativeLong(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseNativeLong(tmpS);
		}

		public static Single XmlAttributeNativeSingle(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseNativeSingle(tmpS);
		}

		public static Double XmlAttributeNativeDouble(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseNativeDouble(tmpS);
		}

		public static decimal XmlAttributeNativeDecimal(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseNativeDecimal(tmpS);
		}

		public static DateTime XmlAttributeNativeDateTime(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseNativeDateTime(tmpS);
		}

		// ----------------------------------------------------------------
		//
		// SIMPLE PARAMS ROUTINES Uses Request.Params[]
		//
		// ----------------------------------------------------------------

		public static String Params(String paramName)
		{
			String tmpS = String.Empty;
			try
			{
				tmpS = HttpContext.Current.Request.Params[paramName].ToString();
				string[] sAry = tmpS.Split(',');
				for (int i=0;i< sAry.Length;i++)
				{
					if (sAry[i].Length != 0) return sAry[i];
				}
			}
			catch
			{
				tmpS = String.Empty;
			}
			return tmpS;
		}
		
		public static bool ParamsBool(String paramName)
		{
			String tmpS = Common.Params(paramName).ToLower();
			if(tmpS == "true" || tmpS == "yes" || tmpS == "1")
			{
				return true;
			}
			return false;
		}

		public static int ParamsUSInt(String paramName)
		{
			String tmpS = Params(paramName);
			return Localization.ParseUSInt(tmpS);
		}

		public static long ParamsUSLong(String paramName)
		{
			String tmpS = Params(paramName);
			return Localization.ParseUSLong(tmpS);
		}

		public static Single ParamsUSSingle(String paramName)
		{
			String tmpS = Params(paramName);
			return Localization.ParseUSSingle(tmpS);
		}

		public static Double ParamsUSDouble(String paramName)
		{
			String tmpS = Params(paramName);
			return Localization.ParseUSDouble(tmpS);
		}

		public static decimal ParamsUSDecimal(String paramName)
		{
			String tmpS = Params(paramName);
			return Localization.ParseUSDecimal(tmpS);
		}

		public static DateTime ParamsUSDateTime(String paramName)
		{
			String tmpS = Params(paramName);
			return Localization.ParseUSDateTime(tmpS);
		}

		public static int ParamsNativeInt(String paramName)
		{
			String tmpS = Params(paramName);
			return Localization.ParseNativeInt(tmpS);
		}

		public static long ParamsNativeLong(String paramName)
		{
			String tmpS = Params(paramName);
			return Localization.ParseNativeLong(tmpS);
		}

		public static Single ParamsNativeSingle(String paramName)
		{
			String tmpS = Params(paramName);
			return Localization.ParseNativeSingle(tmpS);
		}

		public static Double ParamsNativeDouble(String paramName)
		{
			String tmpS = Params(paramName);
			return Localization.ParseNativeDouble(tmpS);
		}

		public static decimal ParamsNativeDecimal(String paramName)
		{
			String tmpS = Params(paramName);
			return Localization.ParseNativeDecimal(tmpS);
		}

		public static DateTime ParamsNativeDateTime(String paramName)
		{
			String tmpS = Params(paramName);
			return Localization.ParseNativeDateTime(tmpS);
		}
    


		public Common() {}

	}

}
