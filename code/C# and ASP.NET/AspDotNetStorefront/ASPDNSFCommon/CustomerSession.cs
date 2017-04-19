using System;
using System.Collections;
using System.Data;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontCommon
{
	/// <summary>
	/// Summary description for CustomerSession.
	/// </summary>
	public class CustomerSession
	{
		public int _customerID;
		private Hashtable SessionParms;

		public CustomerSession(int CustomerID)
		{
			_customerID = CustomerID;
			Age();
			LoadFromDB();
		}

		public void Age()
		{
			DB.ExecuteSQL("delete from CustomerSession where CustomerID=" + _customerID.ToString() + " and ExpiresOn<=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)));
		}

		public static void StaticAge(int CustomerID)
		{
			DB.ExecuteSQL("delete from CustomerSession where CustomerID=" + CustomerID.ToString() + " and ExpiresOn<=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)));
		}

		public static void StaticAgeAllCustomers(int CustomerID)
		{
			DB.ExecuteSQL("delete from CustomerSession where ExpiresOn<=" + DB.DateQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)));
		}

		public void Clear()
		{
			DB.ExecuteSQL("delete from CustomerSession where CustomerID=" + _customerID.ToString());
			LoadFromDB();
		}

		public static void StaticClear(int CustomerID)
		{
			DB.ExecuteSQL("delete from CustomerSession where CustomerID=" + CustomerID.ToString());
		}

		public static void StaticClearAllCustomers()
		{
			DB.ExecuteSQL("delete from CustomerSession");
		}

		public void ResyncToDB()
		{
			Age();
			LoadFromDB();
		}

		private void LoadFromDB()
		{
			IDataReader rs = DB.GetRS("Select * from CustomerSession where CustomerID=" + _customerID.ToString());
			SessionParms = new Hashtable();
			while(rs.Read())
			{
				SessionParms.Add(DB.RSField(rs,"SessionName").ToLower().Trim(),DB.RSField(rs,"SessionValue"));
			}
			rs.Close();
		}

		public void SetVal(String SessionName, String SessionValue, System.DateTime ExpiresOn)
		{
			SessionName = SessionName.ToLower().Trim();
			if(DB.GetSqlN("select count(SessionName) as N from CustomerSession where CustomerID=" + _customerID.ToString() + " and SessionName=" + DB.SQuote(SessionName)) == 0)
			{
				// new parm, add it:
				DB.ExecuteSQL("insert into CustomerSession(CustomerID, SessionName,SessionValue,ExpiresOn) values(" + _customerID.ToString() + "," + DB.SQuote(SessionName) + "," + DB.SQuote(SessionValue) + "," + DB.DateQuote(ExpiresOn) + ")");
				SessionParms.Add(SessionName,SessionValue);
			}
			else
			{
				DB.ExecuteSQL("update CustomerSession set SessionValue=" + DB.SQuote(SessionValue) + ", ExpiresOn=" + DB.DateQuote(ExpiresOn) + " where SessionName=" + DB.SQuote(SessionName) + " and CustomerID=" + _customerID.ToString());
				SessionParms[SessionName] = SessionValue;
			}
		}

		public void SetVal(String SessionName, int NewValue, System.DateTime ExpiresOn)
		{
			SetVal(SessionName, NewValue.ToString(), ExpiresOn);
		}

		public void SetVal(String SessionName, Single NewValue, System.DateTime ExpiresOn)
		{
			SetVal(SessionName, NewValue.ToString(), ExpiresOn);
		}
	
		public void SetVal(String SessionName, decimal NewValue, System.DateTime ExpiresOn)
		{
			SetVal(SessionName, NewValue.ToString(), ExpiresOn);
		}
	
		public void SetVal(String SessionName, System.DateTime NewValue, System.DateTime ExpiresOn)
		{
			SetVal(SessionName, NewValue.ToString(), ExpiresOn);
		}

		public String Session(String SessionName)
		{
			String tmpS = String.Empty;
			try
			{
				tmpS = SessionParms[SessionName.ToLower().Trim()].ToString();
			}
			catch
			{
				tmpS = String.Empty;
			}
			return tmpS;
		}

		public bool SessionBool(String paramName)
		{
			String tmpS = Common.Session(paramName).ToLower();
			if(tmpS == "true" || tmpS == "yes" || tmpS == "1")
			{
				return true;
			}
			return false;
		}

		public int SessionUSInt(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseUSInt(tmpS);
		}

		public long SessionUSLong(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseUSLong(tmpS);
		}

		public Single SessionUSSingle(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseUSSingle(tmpS);
		}

		public Decimal SessionUSDecimal(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseUSDecimal(tmpS);
		}

		public DateTime SessionUSDateTime(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseUSDateTime(tmpS);
		}

		public int SessionNativeInt(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseNativeInt(tmpS);
		}

		public long SessionNativeLong(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseNativeLong(tmpS);
		}

		public Single SessionNativeSingle(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseNativeSingle(tmpS);
		}

		public Decimal SessionNativeDecimal(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseNativeDecimal(tmpS);
		}

		public DateTime SessionNativeDateTime(String paramName)
		{
			String tmpS = Session(paramName);
			return Localization.ParseNativeDateTime(tmpS);
		}

	}
}
