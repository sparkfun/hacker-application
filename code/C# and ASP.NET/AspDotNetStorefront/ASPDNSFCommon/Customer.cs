using System;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Text;
using System.Collections;

namespace AspDotNetStorefrontCommon
{
	/// <summary>
	/// Customer object info is rebuilt off of session["CustomerID"]!!!! This is created in Common.SessionStart or other places
	/// </summary>
	public class Customer
	{
		const String SkinCookieName = "SkinID";
		
		public int _customerID;
		public bool _hasCustomerRecord;
		public String _customerGUID;
		public int _customerLevelID;
		public String _customerLevelName;
		public int _affiliateID;
		public String _localeSetting;
		public String _phone;
		public String _email;
		public bool _okToEMail;
		public String _password;
		public bool _isAnon;
		public bool _isAdminUser;
		public bool _isAdminSuperUser;
		public String _firstName;
		public String _lastName;
		public String _lastIPAddress;
		public decimal _microPayBalance;
		public bool _suppressCookies;
		public int _recurringShippingMethodID;
		public String _recurringShippingMethod;
		public DateTime _subscriptionExpiresOn;
		public int _skinID;
		
		//public String _persistList;
		//public String _updateListXML;
		//private String _updateList;
		//private Hashtable ParmTable = new Hashtable();

		public Customer()
		{
			Init(Common.SessionUSInt("CustomerID"));
		}

		public Customer(int CustomerID)
		{
			_suppressCookies = false;
			Init(CustomerID);
		}

		public String LastIPAddress
		{
			get 
			{
				return _lastIPAddress;
			}
			set 
			{
				_lastIPAddress = value;
			}
		}

		public String LocaleSetting
		{
			get 
			{
				return Localization.GetWebConfigLocale();
			}
		}

		public int SkinID
		{
			get 
			{
				return _skinID;
			}
			set 
			{
				_skinID = value;
			}
		}

		public int AffiliateID
		{
			get 
			{
				return _affiliateID;
			}
			set 
			{
				_affiliateID = value;
			}
		}

		public int CustomerID
		{
			get 
			{
				return _customerID;
			}
			set 
			{
				_customerID = value;
			}
		}

		public bool IsAnon
		{
			get 
			{
				return _isAnon;
			}
			set 
			{
				_isAnon = value;
			}
		}

		public bool IsAdminUser
		{
			get 
			{
				return _isAdminUser;
			}
			set 
			{
				_isAdminUser = value;
			}
		}

		public bool IsAdminSuperUser
		{
			get 
			{
				return _isAdminSuperUser;
			}
			set 
			{
				_isAdminSuperUser = value;
			}
		}


		public int CustomerLevelID
		{
			get 
			{
				return _customerLevelID;
			}
			set 
			{
				_customerLevelID = value;
			}
		}

		public String EMail
		{
			get 
			{
				return _email;
			}
			set 
			{
				_email = value;
			}
		}

		public String FirstName
		{
			get 
			{
				return _firstName;
			}
			set 
			{
				_firstName = value;
			}
		}

		public String LastName
		{
			get 
			{
				return _lastName;
			}
			set 
			{
				_lastName = value;
			}
		}

		public String CustomerLevelName
		{
			get 
			{
				return _customerLevelName;
			}
			set 
			{
				_customerLevelName = value;
			}
		}





		// the next call is DESIGNED for the admin site to use, with SuppressCookies=true, so that all the state saving logic IS DISABLED
		// basicaly, the admin site just wants to create a customer object to get customer info, but NOT look at QS parms, cookie parms, NOR update
		// the db record "automatically". The admin site will do explicit SQL updates as requird. So SuppressCookies turns off many of the 
		// state saving code in the customer object class
		public Customer(int CustomerID, bool SuppressCookies)
		{
			_suppressCookies = SuppressCookies;
			Init(CustomerID);
		}

		private void Init(int CustomerID)
		{
			_customerID = CustomerID;
			_affiliateID = 0;
			_localeSetting = String.Empty;

			if(!_suppressCookies)
			{
				_affiliateID = RecordAffiliateSessionCookie(); // record any affiliate invocation information
				_localeSetting = RecordLocaleSettingCookie(); // record any locale setting 
			}
			else
			{
				// pull from db:
				IDataReader rs = DB.GetRS("select * from customer  " + DB.GetNoLock() + " where customerid=" + CustomerID.ToString());
				if(rs.Read())
				{
					_affiliateID = DB.RSFieldInt(rs,"AffiliateID");
					_localeSetting = DB.RSField(rs,"LocaleSetting");
				}
				rs.Close();
			}
			if(_localeSetting.Length == 0)
			{
				_localeSetting = Localization.GetWebConfigLocale();
			}
			_lastIPAddress = Common.ServerVariables("REMOTE_ADDR");
			_customerLevelID = 0;
			_customerLevelName = String.Empty;
			_hasCustomerRecord = false;
			_phone = String.Empty;
			_email = String.Empty;
			_okToEMail = false;
			_password = String.Empty;
			_isAnon = true;
			if(Common.IsAdminSite)
			{
				_skinID = 1;
			}
			else
			{
				_skinID = Common.CookieUSInt(SkinCookieName);
			}
			_isAdminUser = false;
			_isAdminSuperUser = false;
			_firstName = String.Empty;
			_lastName = String.Empty;
			_microPayBalance = System.Decimal.Zero;
			_subscriptionExpiresOn = System.DateTime.MinValue;

			_recurringShippingMethodID = 0;
			_recurringShippingMethod = String.Empty;

			//ParmTable.Clear();
			//_persistList = Common.AppConfig("CustomerPersistList").Replace("\\r","").Replace("\\n","");
			//_updateList = String.Empty;
			//_updateListXML = String.Empty;
			
			if(_customerID != 0)
			{
				// we have a session, so rebuild: customer info from it:
				String sql = "select * from customer  " + DB.GetNoLock() + " where Customer.deleted=0 and CustomerID=" + _customerID.ToString();
				IDataReader rs = DB.GetRS(sql);
				if (rs.Read())
				{
					_hasCustomerRecord = true;
					_phone = DB.RSField(rs,"Phone");
					_email = DB.RSField(rs,"EMail");
					_okToEMail = DB.RSFieldBool(rs,"OkToEMail");
					_password = DB.RSField(rs,"Password");
					if (Common.AppConfigBool("EncryptPassword"))
					{
						_password = Common.UnmungeString(_password);  
						if (_password.StartsWith("Error.")) //bad decryption may be in clear
						{
							_password = DB.RSField(rs,"Password");
						}
					}
					_isAnon = (_email.Trim().Length == 0 || _email.StartsWith("Anon_"));
					_firstName = DB.RSField(rs,"FirstName");
					_lastName = DB.RSField(rs,"LastName");
					_isAdminUser = DB.RSFieldBool(rs,"IsAdmin");
					_isAdminSuperUser = StaticIsAdminSuperUser(_customerID);
					_customerLevelID = DB.RSFieldInt(rs,"CustomerLevelID");
					_customerLevelName = Common.GetCustomerLevelName(_customerLevelID);
					_customerGUID = DB.RSFieldGUID(rs,"CustomerGUID");
					_subscriptionExpiresOn = DB.RSFieldDateTime(rs,"SubscriptionExpiresOn");
					_microPayBalance = DB.RSFieldDecimal(rs,"MicroPayBalance");
					if(Common.ServerVariables("QUERY_STRING").ToLower().IndexOf("localesetting=") == -1 && DB.RSField(rs,"LocaleSetting").Length != 0)
					{
						_localeSetting = DB.RSField(rs,"LocaleSetting");
					}
					_recurringShippingMethodID =  DB.RSFieldInt(rs,"RecurringShippingMethodID");
					_recurringShippingMethod =  DB.RSField(rs,"RecurringShippingMethod");
					//SEC4
					if (! this._isAnon)
					{
						// Check that the password is encrypted
						string PWD = Common.UnmungeString(DB.RSField(rs,"Password"));
						bool isClear = (PWD.StartsWith("Error."));
						if (Common.AppConfigBool("EncryptPassword"))
						{
							if (isClear)
							{
								PWD = Common.MungeString(DB.RSField(rs,"Password"));
								DB.ExecuteSQL(String.Format("update customer set [password]={0} where CustomerID={1}",DB.SQuote(PWD),_customerID));
							}
						}
						else 
						{
							if (!isClear)
							{
								DB.ExecuteSQL(String.Format("update customer set [password]={0} where CustomerID={1}",DB.SQuote(DB.RSField(rs,"Password")),_customerID));
							}
						}
					}
				}
				if(!_suppressCookies)
				{
					DB.ExecuteSQL("update customer set LastIPAddress=" + DB.SQuote(_lastIPAddress) + ", AffiliateID=" + Common.IIF(_affiliateID == 0 , "NULL", _affiliateID.ToString()) + ", LocaleSetting=" + DB.SQuote(_localeSetting) + " where CustomerID=" + _customerID.ToString());
				}
				//PersistParms(rs);
				rs.Close();
			}
			//			else
			//			{
			//				PersistParms(null);
			//			}
		}


		public void SetLocale(String LocaleSetting)
		{
			if(LocaleSetting.Length == 0)
			{
				LocaleSetting = Localization.GetWebConfigLocale();
			}
			if(!_suppressCookies)
			{
				Common.SetCookie("LocaleSetting",LocaleSetting,new TimeSpan(365,0,0,0,0));
			}
			if(_hasCustomerRecord)
			{
				DB.ExecuteSQL("update customer set LocaleSetting=" + DB.SQuote(LocaleSetting) + " where customerid=" + _customerID.ToString());
				//ParmTable.Remove("LocaleSetting");
				//ParmTable.Add("LocaleSetting",LocaleSetting);
			}
			_localeSetting = LocaleSetting;
		}
		

		public String FullName()
		{
			return (_firstName + " " + _lastName).Trim();
		}

		public bool SetGiftCardCode(String CardCode)
		{
			if(Common.GiftCardCodeIsValid(CardCode))
			{
				try
				{
					DB.ExecuteSQL("update customer set CouponCode=" + Common.SQuote(CardCode) + " where CustomerID=" + _customerID.ToString());
				}
				catch
				{
					return false;
				}
				return true;
			}
			return false;
		}
		

		public int ShippingMethodID
		{
			get
			{
				CustomerSession custSession = new CustomerSession(this._customerID);
				Address thisAddress = new Address();
				thisAddress.LoadByCustomer(this._customerID,AddressTypes.Shipping);
				if (thisAddress.AddressID == 0)
				{
					return custSession.SessionUSInt("ShippingMethodID");
				}
				else
				{
					if (thisAddress.ShippingMethodID == 0) // Blank try the one in custSession
					{
						thisAddress.ShippingMethodID = custSession.SessionUSInt("ShippingMethodID");
						thisAddress.UpdateDB();
					}
					custSession.SetVal("ShippingMethodID",thisAddress.ShippingMethodID,DateTime.Now.AddYears(1));
					return thisAddress.ShippingMethodID;
				}
			}
			set
			{
				CustomerSession custSession = new CustomerSession(this._customerID);
				custSession.SetVal("ShippingMethodID",value,DateTime.Now.AddYears(1));
				Address thisAddress = new Address();
				thisAddress.LoadByCustomer(this._customerID,AddressTypes.Shipping);
				if (thisAddress.AddressID != 0)
				{
					thisAddress.ShippingMethodID = value;
					thisAddress.UpdateDB();
				}
			}
		}
		
		public string ShippingMethod
		{
			get
			{
				CustomerSession custSession = new CustomerSession(this._customerID);
				Address thisAddress = new Address();
				thisAddress.LoadByCustomer(this._customerID,AddressTypes.Shipping);
				if (thisAddress.AddressID == 0)
				{
					return custSession.Session("ShippingMethod");
				}
				else
				{
					if (thisAddress.ShippingMethod.Length == 0) // Blank try the one in custSession
					{
						thisAddress.ShippingMethod = custSession.Session("ShippingMethod");
						thisAddress.UpdateDB();
					}
					custSession.SetVal("ShippingMethod",thisAddress.ShippingMethod,DateTime.Now.AddYears(1));
					return thisAddress.ShippingMethod;
				}
			}
			set
			{
				CustomerSession custSession = new CustomerSession(this._customerID);
				custSession.SetVal("ShippingMethod",value,DateTime.Now.AddYears(1));
				Address thisAddress = new Address();
				thisAddress.LoadByCustomer(this._customerID,AddressTypes.Shipping);
				if (thisAddress.AddressID != 0)
				{
					thisAddress.ShippingMethod = value;
					thisAddress.UpdateDB();
				}
			}
		}
		
		public String ShippingCity
		{
			get
			{
				CustomerSession custSession = new CustomerSession(this._customerID);
				Address thisAddress = new Address();
				thisAddress.LoadByCustomer(this._customerID,AddressTypes.Shipping);
				if (thisAddress.AddressID == 0)
				{
					return custSession.Session("ShippingCity");
				}
				else
				{
					custSession.SetVal("ShippingCity",thisAddress.City,DateTime.Now.AddYears(1));
					return thisAddress.City;
				}
			}
			set
			{
				CustomerSession custSession = new CustomerSession(this._customerID);
				custSession.SetVal("ShippingCity",value,DateTime.Now.AddYears(1));
				//Don't allow change of ShippingAddress record via ShippingCity
			}
		}
    
		public String ShippingState
		{
			get
			{
				CustomerSession custSession = new CustomerSession(this._customerID);
				Address thisAddress = new Address();
				thisAddress.LoadByCustomer(this._customerID,AddressTypes.Shipping);
				if (thisAddress.AddressID == 0)
				{
					return custSession.Session("ShippingState");
				}
				else
				{
					custSession.SetVal("ShippingState",thisAddress.State,DateTime.Now.AddYears(1));
					return thisAddress.State;
				}
			}
			set
			{
				CustomerSession custSession = new CustomerSession(this._customerID);
				custSession.SetVal("ShippingState",value,DateTime.Now.AddYears(1));
				//Don't allow change of ShippingAddress record via ShippingZip
			}
		}

		public String ShippingZip
		{
			get
			{
				CustomerSession custSession = new CustomerSession(this._customerID);
				Address thisAddress = new Address();
				thisAddress.LoadByCustomer(this._customerID,AddressTypes.Shipping);
				if (thisAddress.AddressID == 0)
				{
					return custSession.Session("ShippingZip");
				}
				else
				{
					custSession.SetVal("ShippingZip",thisAddress.Zip,DateTime.Now.AddYears(1));
					return thisAddress.Zip;
				}
			}
			set
			{
				CustomerSession custSession = new CustomerSession(this._customerID);
				custSession.SetVal("ShippingZip",value,DateTime.Now.AddYears(1));
				//Don't allow change of ShippingAddress record via ShippingZip
			}
		}

		public String ShippingCountry
		{
			get
			{
				CustomerSession custSession = new CustomerSession(this._customerID);
				Address thisAddress = new Address();
				thisAddress.LoadByCustomer(this._customerID,AddressTypes.Shipping);
				if (thisAddress.AddressID == 0)
				{
					return custSession.Session("ShippingCountry");
				}
				else
				{
					custSession.SetVal("ShippingCountry",thisAddress.Country,DateTime.Now.AddYears(1));
					return thisAddress.Country;
				}
			}
			set
			{
				CustomerSession custSession = new CustomerSession(this._customerID);
				custSession.SetVal("ShippingCountry",value,DateTime.Now.AddYears(1));
				//Don't allow change of ShippingAddress record via ShippingCountry
			}
		}
    
		static public String GetName(int CustomerID)
		{
			String tmpS = String.Empty;
			IDataReader rs = DB.GetRS("Select firstname,lastname from customer  " + DB.GetNoLock() + " where CustomerID=" + CustomerID.ToString());
			if(rs.Read())
			{
				tmpS = (DB.RSField(rs,"FirstName") + " " + DB.RSField(rs,"LastName")).Trim();
			}
			rs.Close();
			return tmpS;
		}

		static public bool HasOrders(int CustomerID)
		{
			return (DB.GetSqlN("select count(ordernumber) as N from orders  " + DB.GetNoLock() + " where customerid=" + CustomerID.ToString()) > 0);
		}

		public bool HasOrders()
		{
			return Customer.HasOrders(_customerID);
		}

		public String BillingInformation(String separator)
		{
			return BillingInformation(_customerID,separator);
		}

		public String ShipToAddress(bool IncludePhone, String separator)
		{
			return ShipToAddress(IncludePhone,_customerID,separator);
		}

		public String BillToAddress(bool IncludePhone, String separator)
		{
			return BillToAddress(IncludePhone,_customerID,separator);
		}

		static public String ShipToAddress(bool IncludePhone, int CustomerID, String separator)
		{
			Address ShippingAddress = new Address();
			ShippingAddress.LoadByCustomer(CustomerID,AddressTypes.Shipping);
			return ShippingAddress.DisplayString(IncludePhone,separator);
		}

		static public String BillToAddress(bool IncludePhone, int CustomerID, String separator)
		{
			Address BillingAddress = new Address();
			BillingAddress.LoadByCustomer(CustomerID,AddressTypes.Billing);
			return BillingAddress.DisplayString(IncludePhone,separator);
		}

		static public String BillingInformation(int CustomerID, String separator)
		{

			Address BillingAddress = new Address();
			BillingAddress.LoadByCustomer(CustomerID,AddressTypes.Billing);
			return BillingAddress.DisplayCardString(separator);
		}

		static public int GetIDFromEMail(String EMail)
		{
			int tmpS = 0;
			IDataReader rs = DB.GetRS("Select * from customer  " + DB.GetNoLock() + " where deleted=0 and Email=" + DB.SQuote(EMail.ToLower()));
			if(rs.Read())
			{
				tmpS = DB.RSFieldInt(rs,"CustomerID");
			}
			rs.Close();
			return tmpS;
		}

		static public String GetGUID(int customerID)
		{
			String tmpS = String.Empty;
			IDataReader rs = DB.GetRS("Select CustomerGUID from customer  " + DB.GetNoLock() + " where customerid=" + customerID.ToString());
			if(rs.Read())
			{
				tmpS = DB.RSFieldGUID(rs,"CustomerGUID");
			}
			rs.Close();
			return tmpS;
		}

		static public String GetEMail(int customerID)
		{
			String tmpS = String.Empty;
			IDataReader rs = DB.GetRS("Select EMail from customer  " + DB.GetNoLock() + " where customerid=" + customerID.ToString());
			if(rs.Read())
			{
				tmpS = DB.RSField(rs,"EMail");
			}
			rs.Close();
			return tmpS;
		}

		static public String GetOrderNotes(int customerID)
		{
			String tmpS = String.Empty;
			IDataReader rs = DB.GetRS("Select OrderNotes from customer  " + DB.GetNoLock() + " where customerid=" + customerID.ToString());
			if(rs.Read())
			{
				tmpS = DB.RSField(rs,"OrderNotes");
			}
			rs.Close();
			return tmpS;
		}

		public bool HasUsedCoupon(String CouponCode)
		{
			return (DB.GetSqlN("select count(ordernumber) as N from orders  " + DB.GetNoLock() + " where customerid=" + _customerID.ToString() + " and lower(CouponCode)=" + DB.SQuote(CouponCode.ToLower())) != 0);
		}
		
		static public bool HasUsedCoupon(int CustomerID, String CouponCode)
		{
			return (DB.GetSqlN("select count(ordernumber) as N from orders  " + DB.GetNoLock() + " where customerid=" + CustomerID.ToString() + " and lower(CouponCode)=" + DB.SQuote(CouponCode.ToLower())) != 0);
		}
		

		static public int GetCustomerLevelID(int CustomerID)
		{
			int tmpS = 0;
			IDataReader rs = DB.GetRS("Select CustomerLevelID from customer where customerlevelid in (select customerlevelid from customerlevel where deleted=0) and deleted=0 and CustomerID=" + CustomerID.ToString());
			if(rs.Read())
			{
				tmpS = DB.RSFieldInt(rs,"CustomerLevelID");
			}
			rs.Close();
			return tmpS;
		}
		
		public static String GetUsername(int CustomerID)
		{
			IDataReader rs = DB.GetRS("Select * from customer where customerid=" + CustomerID.ToString());
			String uname = String.Empty;
			if(rs.Read())
			{
				uname = (DB.RSField(rs,"FirstName") + "" + DB.RSField(rs,"LastName")).Trim();
			}
			rs.Close();
			return uname;
		}
		
		static public bool StaticIsAdminSuperUser(int CustomerID)
		{
			String[] su = Common.AppConfig("Admin_Superuser").Split(',');
			for(int i = su.GetLowerBound(0); i <= su.GetUpperBound(0); i++)
			{
				if(Common.IsInteger(su[i]))
				{
					if(Localization.ParseUSInt(su[i]) == CustomerID)
					{
						return true;
					}
				}
			}
			return false;
		}

		static public bool StaticIsAdminUser(int CustomerID)
		{
			if(CustomerID == 0)
			{
				return false;
			}
			bool tmp = false;
			IDataReader rs = DB.GetRS("select IsAdmin from customer " + DB.GetNoLock() + " where CustomerID=" + CustomerID.ToString());
			if(rs.Read())
			{
				tmp = DB.RSFieldBool(rs,"IsAdmin");
			}
			rs.Close();
			return tmp;
		}

		public void RequireCustomerRecord()
		{
			if(!_hasCustomerRecord)
			{
				MakeAnonCustomerRecord(out _customerID,out _customerGUID);
				HttpContext.Current.Session["CustomerID"] = _customerID.ToString();
				HttpContext.Current.Session["CustomerGUID"] = _customerGUID;

				// try to write them a new cookie (may fail, but it's ok), so when they visit next time, we will remember who they are:
				//				Common.SetCookie(Common.UserCookieName(),"E-" + Common.MungeString(_customerGUID),new TimeSpan(1000,0,0,0,0));
				//V3_9 Authenticate with the temporary GUID with a persistent cookie of 			
				FormsAuthentication.SetAuthCookie(_customerGUID,true);
				//V3_9 Set the expriation
				HttpCookie cookie = HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName];
				cookie.Expires = DateTime.Now.Add(new TimeSpan(1000,0,0,0));

				Init(_customerID); // must reload customer data now that we have a record for it
			}
		}

		// check QS for an affiliateid= or affid=, and if so, set session cookie
		// returns resulting active customer affiliateid (from QS or prior session cookie)
		static public int RecordAffiliateSessionCookie()
		{
			int AffiliateID = 0;
			String QS = Common.ServerVariables("QUERY_STRING").ToLower();
			if(QS.IndexOf("affiliateid=") != -1 || QS.IndexOf("affid=") != -1)
			{
				AffiliateID = Common.QueryStringUSInt("AffiliateID");
				if(AffiliateID == 0)
				{
					AffiliateID = Common.QueryStringUSInt("AffID");
				}
				Common.SetSessionCookie("AffiliateID",AffiliateID.ToString());
			}
			else
			{
				// noting in QS, just return what their "prior" affiliateid is:
				AffiliateID = Common.CookieUSInt("AffiliateID");
			}
			return AffiliateID;
		}

		// check QS for an affiliateid= or affid=, and if so, set session cookie
		// returns resulting active customer affiliateid (from QS or prior session cookie)
		static public String RecordLocaleSettingCookie()
		{
			String LocaleSetting = String.Empty;
			String QS = Common.ServerVariables("QUERY_STRING").ToLower();
			if(QS.IndexOf("localesetting=") != -1)
			{
				LocaleSetting = Common.QueryString("LocaleSetting");
				if(LocaleSetting.Length == 0)
				{
					LocaleSetting = Localization.GetWebConfigLocale();
				}
				Common.SetCookie("LocaleSetting",LocaleSetting,new TimeSpan(1000,0,0,0,0));
			}
			else
			{
				// noting in QS, just return what their "prior" LocaleSetting is:
				LocaleSetting = Common.Cookie("LocaleSetting",true);
			}
			return LocaleSetting;
		}

		public bool IsLicensedUser()
		{
			int N = DB.GetSqlN("select count(*) as N from orders  " + DB.GetNoLock() + " where email=" + DB.SQuote(_email));
			return (N != 0);
		}

		// make a new customer record and set SESSION Parms
		public static void MakeAnonCustomerRecord(out int CustomerID, out String CustomerGUID)
		{
			CustomerID = 0;
			CustomerGUID = String.Empty;
			String NewGUID = DB.GetNewGUID();
			int AffiliateID = Customer.RecordAffiliateSessionCookie();
			String sql = "insert into Customer(CustomerGUID,EMail,[Password],AffiliateID,LastIPAddress) values(" + DB.SQuote(NewGUID) + "," + DB.SQuote("Anon_" + NewGUID) + "," + DB.SQuote("N/A") + "," + Common.IIF(AffiliateID == 0, "NULL", AffiliateID.ToString()) + "," + DB.SQuote(Common.ServerVariables("REMOTE_ADDR")) + ")";
			DB.ExecuteSQL(sql);
			IDataReader rs = DB.GetRS("select * from customer where CustomerGUID=" + DB.SQuote(NewGUID));
			if (rs.Read())
			{
				CustomerID = DB.RSFieldInt(rs,"CustomerID");
				CustomerGUID = DB.RSFieldGUID(rs,"CustomerGUID");
			}
			rs.Close();
		}



	}
}
