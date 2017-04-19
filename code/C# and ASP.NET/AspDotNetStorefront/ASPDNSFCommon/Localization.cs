// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Globalization;
using System.Xml;
using System.Web;

namespace AspDotNetStorefrontCommon
{
	/// <summary>
	/// Summary description for Localization.
	/// </summary>
	public class Localization
	{
		static CultureInfo USCulture = new CultureInfo("en-US");

		public Localization() {}

		static private String MasterLocale = String.Empty;
		
		static public String GetWebConfigLocale()
		{
			if(MasterLocale.Length == 0)
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(HttpContext.Current.Server.MapPath("web.config"));
				XmlNode node = doc.DocumentElement.SelectSingleNode("//globalization");
				MasterLocale = node.Attributes["culture"].InnerText;
			}
			return MasterLocale;
		}
		
	static public String WeightUnits()
		{
			return Common.AppConfig("Localization.WeightUnits");
		}

		static public String ShortDateFormat()
		{
			String tmp = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
			tmp = tmp.Replace("M","MM").Replace("D","DD");
			tmp = tmp.Replace("MMM","MM").Replace("DDD","DD"); //.Replace("YYYY","YY");
			return tmp;
		}

		static public string JSCalendarDateFormatSpec()
		{
			// see jscalendar/calendar-setup.js for more info. Typical format would be: " + Localization.JSCalendarDateFormatSpec() + "
			String tmp = ShortDateFormat();
			tmp = tmp.Replace("MM","%m").Replace("DD","%d").Replace("YYYY","%Y").Replace("YY","%Y");
			return tmp;
		}

		static public String JSCalendarLanguageFile()
		{
			return "calendar-" + GetWebConfigLocale().Substring(0,2) + ".js";
		}

		static public String StoreCurrency()
		{
			String tmpS = Common.AppConfig("Localization.StoreCurrency").ToUpper();
			if(tmpS.Length == 0)
			{
				tmpS = "USD"; // set some default
			}
			return tmpS;
		}

		static public String StoreCurrencyNumericCode()
		{
			String tmpS = Common.AppConfig("Localization.StoreCurrencyNumericCode");
			if(tmpS.Length == 0)
			{
				tmpS = "840"; // set some default
			}
			return tmpS;
		}
		
		static public String CurrencySymbol()
		{
			String tmp = CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
			return tmp;
		}

		static public int ParseUSInt(String s)
		{
			try
			{
				return System.Int32.Parse(s,USCulture);
			}
			catch
			{
				return 0;
			}
		}

		static public int ParseNativeInt(String s)
		{
			try
			{
				return Localization.ParseUSInt(s); // use default locale setting
			}
			catch
			{
				return 0;
			}
		}

		static public long ParseUSLong(String s)
		{
			try
			{
				return System.Int64.Parse(s,USCulture);
			}
			catch
			{
				return 0;
			}
		}

		static public long ParseNativeLong(String s)
		{
			try
			{
				return System.Int64.Parse(s); // use default locale setting
			}
			catch
			{
				return 0;
			}
		}

		static public Single ParseUSSingle(String s)
		{
			try
			{
				return System.Single.Parse(s,USCulture);
			}
			catch
			{
				return 0.0F;
			}
		}

		static public Single ParseNativeSingle(String s)
		{
			try
			{
				return System.Single.Parse(s); // use default locale setting
			}
			catch
			{
				return 0.0F;
			}
		}

		static public Double ParseUSDouble(String s)
		{
			try
			{
				return System.Double.Parse(s,USCulture);
			}
			catch
			{
				return 0.0F;
			}
		}

		static public Double ParseNativeDouble(String s)
		{
			try
			{
				return System.Double.Parse(s); // use default locale setting
			}
			catch
			{
				return 0.0F;
			}
		}

		static public decimal ParseUSCurrency(String s)
		{
			s = s.Replace("$","");
			try
			{
				return System.Decimal.Parse(s,USCulture);
			}
			catch
			{
				return System.Decimal.Zero;
			}
		}

		static public decimal ParseNativeCurrency(String s)
		{
			try
			{
				return System.Decimal.Parse(s);
			}
			catch
			{
				return System.Decimal.Zero;
			}
		}

		static public decimal ParseUSDecimal(String s)
		{
			try
			{
				return System.Decimal.Parse(s,USCulture);
			}
			catch
			{
				return System.Decimal.Zero;
			}
		}

		static public decimal ParseNativeDecimal(String s)
		{
			try
			{
				return System.Decimal.Parse(s);
			}
			catch
			{
				return System.Decimal.Zero;
			}
		}

		static public DateTime ParseUSDateTime(String s)
		{
			try
			{
				return System.DateTime.Parse(s,USCulture);
			}
			catch
			{
				return System.DateTime.MinValue;
			}
		}

		static public DateTime ParseNativeDateTime(String s)
		{
			try
			{
				return System.DateTime.Parse(s); // use default locale setting
			}
			catch
			{
				return System.DateTime.MinValue;
			}
		}
		
		static public String ToUSShortDateString(DateTime dt)
		{
			if(dt == System.DateTime.MinValue)
			{
				return String.Empty;
			}
			return dt.ToString("MM/dd/yy"); //dt.Month.ToString().PadLeft(2,'0') + "/" + dt.Day.ToString().PadLeft(2,'0') + "/" + dt.Year.ToString().Substring(2,2);
		}

		static public String ToNativeShortDateString(DateTime dt)
		{
			if(dt == System.DateTime.MinValue)
			{
				return String.Empty;
			}
			return dt.ToShortDateString();
		}

		static public String ToUSDateTimeString(DateTime dt)
		{
			if(dt == System.DateTime.MinValue)
			{
				return String.Empty;
			}
			return dt.ToString("MM/dd/yyyy hh:mm:ss tt"); //dt.Month.ToString().PadLeft(2,'0') + "/" + dt.Day.ToString().PadLeft(2,'0') + "/" + dt.Year.ToString().Substring(2,2) + " " + dt.Hour.ToString().PadLeft(2,'0') + ":" + dt.Minute.ToString().PadLeft(2,'0') + ":" + dt.Second.ToString().PadLeft(2,'0') + "." + dt.Millisecond.ToString() + " " + dt.TimeOfDay;
		}

		static public String ToNativeDateTimeString(DateTime dt)
		{
			if(dt == System.DateTime.MinValue)
			{
				return String.Empty;
			}
			return dt.ToString();
		}

		static public String CurrencyStringForDisplay(decimal amt)
		{
			String tmpS = amt.ToString("C");
			if(tmpS.StartsWith("("))
			{
				tmpS = "-" + tmpS.Replace("(","").Replace(")","");
			}
			return tmpS;
		}

		static public String CurrencyStringForDisplayUS(decimal amt)
		{
			String tmpS = amt.ToString("C",USCulture);
			if(tmpS.StartsWith("("))
			{
				tmpS = "-" + tmpS.Replace("(","").Replace(")","");
			}
			return tmpS;
		}

		static public String CurrencyStringForGateway(decimal amt)
		{
			String tmpS = Localization.CurrencyStringForDisplayUS(amt).Replace("$","").Replace(",","");
			return tmpS;
		}

		static public String CurrencyStringForDB(decimal amt)
		{
			return Localization.CurrencyStringForDisplayUS(amt).Replace("$","").Replace(",","");
		}

		static public String IntStringForDB(int amt)
		{
			return amt.ToString("G", USCulture).Replace(",","");
		}

		static public String SingleStringForDB(Single amt)
		{
			return amt.ToString("G", USCulture).Replace(",","");
		}

		static public String DoubleStringForDB(double amt)
		{
			return amt.ToString("G", USCulture).Replace(",","");
		}

		static public String DecimalStringForDB(decimal amt)
		{
			return amt.ToString("G", USCulture).Replace(",","");
		}

		static public String CurrencyStringForDisplay(Single amt)
		{
			return CurrencyStringForDisplay((decimal)amt);
		}

		static public String CurrencyStringForDisplayUS(Single amt)
		{
			return CurrencyStringForDisplayUS((decimal)amt);
		}

		static public String CurrencyStringForGateway(Single amt)
		{
			return CurrencyStringForGateway((decimal)amt);
		}

	}
}
