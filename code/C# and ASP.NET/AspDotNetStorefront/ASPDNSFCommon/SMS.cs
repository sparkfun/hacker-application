// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;

namespace AspDotNetStorefrontCommon
{
	/// <summary>
	/// Summary description for SMS.
	/// </summary>
	public class SMS
	{
		public SMS() {}

		public static String MakeProperPhoneFormat2(String PhoneNumber)
		{
			if(PhoneNumber.Length == 0)
			{
				return String.Empty;
			}
			if(PhoneNumber.Substring(0,1) != "1")
			{
				PhoneNumber = "1" + PhoneNumber;
			}
			String newS = String.Empty;
			String validDigits = "0123456789";
			for(int i = 1; i<= PhoneNumber.Length; i++)
			{
				if(validDigits.IndexOf(PhoneNumber[i-1]) != -1)
				{
					newS = newS + PhoneNumber[i-1];
				}
			}
			return newS;
		}

		static public void Send(Order order, String FromEMailAddress, String  MailServer)
		{
			String DispatchCellCarrier = Common.AppConfig("Dispatch_CellCarrier").Replace(" ","").ToUpper();
			if(DispatchCellCarrier.Length != 0 && DispatchCellCarrier.ToUpper() != "TBD")
			{
				String SMSTo = Common.AppConfig("Dispatch_ToPhoneNumber");
				if(SMSTo.Length != 0)
				{
					Decimal OrderThreshold = System.Decimal.Zero;
					if(Common.AppConfig("Dispatch_OrderThreshold").Length != 0)
					{
						try
						{
							OrderThreshold = System.Decimal.Parse(Common.AppConfig("Dispatch_OrderThreshold").Replace("$",""));
						}
						catch {}
					}
					if(order.Total(true) >= OrderThreshold)
					{
						SMSTo = SMS.MakeProperPhoneFormat2(SMSTo);
						switch(DispatchCellCarrier)
						{
							case "T-MOBILE":
								SMSTo = SMSTo.Substring(1,SMSTo.Length - 1) + "@tmomail.net"; // no leading 1
								break;
							case "SPRINTPCS":
								SMSTo = SMSTo.Substring(1,SMSTo.Length - 1) + "@messaging.sprintpcs.com"; // no leading 1
								break;
							case "VERIZON":
								SMSTo = SMSTo.Substring(1,SMSTo.Length - 1) + "@vtext.com"; // no leading 1
								break;
							case "AT&T":
								SMSTo = SMSTo.Substring(1,SMSTo.Length - 1) + "@mmode.com"; // no leading 1
								break;
							case "NEXTEL":
								SMSTo = SMSTo.Substring(1,SMSTo.Length - 1) + "@page.nextel.com"; // no leading 1
								break;
							case "CINGULAR":
								SMSTo = SMSTo.Substring(1,SMSTo.Length - 1) + "@mmode.com"; // no leading 1
								break;
							case "METROCALL":
								SMSTo = SMSTo.Substring(1,SMSTo.Length - 1) + "@page.metrocall.com"; // no leading 1
								break;
							default:
								break;
						}
						try
						{
							String SMSSubject = Common.AppConfig("Dispatch_SiteName");
							if(SMSSubject.Length != 0)
							{
								SMSSubject = "NEW ORDER";
							}
							String SMSBody = "Order #" + order._orderNumber.ToString() + ", Total=" + Localization.CurrencyStringForDisplay(order._total) + ", Ship=" + order._shippingMethod + ", Payment=" + order._paymentMethod + ", Date=" + Localization.ToNativeShortDateString(System.DateTime.Now);
							if(SMSBody.Length > Common.AppConfigUSInt("Dispatch_MAX_SMS_MSG_LENGTH"))
							{
								SMSBody = SMSBody.Substring(0,Common.AppConfigUSInt("Dispatch_MAX_SMS_MSG_LENGTH"));
							}
							Common.SendMail(SMSSubject, SMSBody, false, FromEMailAddress,FromEMailAddress,SMSTo,SMSTo,"",MailServer);
						}
						catch {}
					}
				}
			}
		}
	}
}
