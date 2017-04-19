// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Text;
using System.IO;

namespace AspDotNetStorefrontCommon
{
	/// <summary>
	/// Summary description for ErrorIDGenerator.
	/// </summary>
	public class ErrorXRefGenerator
	{
		private static System.Random psRandom = null;
		/// <summary>
		/// Since this implementation will use a random
		/// number in our ID, initialize our randomizer
		/// here.
		/// </summary>
		static ErrorXRefGenerator()
		{
			psRandom = new Random();
		}

		public static string NextID()
		{
			try
			{
				// We will use the number of seconds since the
				// first day of the month plus a random number
				// to create our unique ID.  We shouldn't have
				// a real problem if this doesn't keep our ids
				// unique.

				// Get the date for today and the first day of 
				// the month.
				DateTime dtToday = DateTime.Now;
				DateTime dtFirstOfMonth = new DateTime(dtToday.Year, dtToday.Month, 1);

				// Find the number of seconds since the start
				// of the month.
				TimeSpan ts = dtToday.Subtract(dtFirstOfMonth);
				long lTotalSeconds = (long)ts.TotalSeconds;

				// Get our Random Number to add one more level
				int iRandom = psRandom.Next(32767);

				// Format and return
				return String.Format("{0:X}-{1:X}",lTotalSeconds,iRandom);
			}
			catch
			{
				// We really should never get here, but let's
				// not add insult to injury by throwing a new
				// exception in an area that will be used for
				// exception handling.

				return "AAAAAA-AAA";
			}
		}
	}
}
