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
			DB.ExecuteSQL("delete from CustomerSession where CustomerID=" + _customerID.ToString() + " and ExpiresOn<=" + DB.DateQuote(System.DateTime.Now.ToString()));
		}

		public static void StaticAge(int CustomerID)
		{
			DB.ExecuteSQL("delete from CustomerSession where CustomerID=" + CustomerID.ToString() + " and ExpiresOn<=" + DB.DateQuote(System.DateTime.Now.ToString()));
		}

		public static void StaticAgeAllCustomers(int CustomerID)
		{
			DB.ExecuteSQL("delete from CustomerSession where ExpiresOn<=" + DB.DateQuote(System.DateTime.Now.ToString()));
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

		public void SetVal(String SessionName, float NewValue, System.DateTime ExpiresOn)
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

		public bool SessionBool(String SessionName)
		{
			String tmp = this.Session(SessionName).ToLower();
			if(tmp == "true" || tmp == "yes" || tmp == "1")
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public int SessionInt(String SessionName)
		{
			try
			{
				return System.Int32.Parse(Session(SessionName));
			}
			catch
			{
				return 0;
			}
		}

		public float SessionFloat(String SessionName)
		{
			try
			{
				return System.Single.Parse(Session(SessionName));
			}
			catch
			{
				return 0.0F;
			}
		}

		public Decimal SessionDecimal(String SessionName)
		{
			try
			{
				return System.Decimal.Parse(Session(SessionName));
			}
			catch
			{
				return 0.0M;
			}
		}

		public DateTime SessionDateTime(String SessionName)
		{
			try
			{
				return Localization.ParseNativeDate(this.Session(SessionName));
			}
			catch
			{
				return System.DateTime.MinValue;
			}
		}

	}
}
