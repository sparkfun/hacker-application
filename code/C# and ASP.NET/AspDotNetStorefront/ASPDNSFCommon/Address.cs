// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Text;
using System.Web;

namespace AspDotNetStorefrontCommon
{
	[FlagsAttribute]
	public enum AddressTypes : int
	{
		Unknown = 0,
		Billing = 1,
		Shipping = 2,
		Account = 4
	}


	/// <summary>
	/// Summary description for Address.
	/// </summary>
	public class Address
	{
		public int _siteID = 1; // caller must set this if required to be non "1"
		private int m_customerID = 0;
		public int m_addressID = 0;
		private int m_displayOrder = 0;
		private int m_shippingMethodID = 0;
    
		private AddressTypes m_addressType = AddressTypes.Unknown;

		private String m_firstName = String.Empty;
		private String m_lastName = String.Empty;
		private String m_company = String.Empty;
		private String m_address1 = String.Empty;
		private String m_address2 = String.Empty;
		private String m_suite = String.Empty;
		private String m_city = String.Empty;
		private String m_state = String.Empty;
		private String m_zip = String.Empty;
		private String m_country = String.Empty;
		private String m_phone = String.Empty;
		private String m_email = String.Empty;

		private String m_shippingMethod = String.Empty;
		private String m_paymentMethod = String.Empty;
		private String m_cardType = String.Empty;
		private String m_cardNumber = String.Empty;
		private String m_cardName = String.Empty;
		private String m_cardExtraCode = String.Empty;
		private String m_cardExpirationMonth = String.Empty;
		private String m_cardExpirationYear = String.Empty;
		private String m_eCheckBankABACode = String.Empty;
		private String m_eCheckBankAccountNumber = String.Empty;
		private String m_eCheckBankAccountType = String.Empty;
		private String m_eCheckBankName = String.Empty;
		private String m_eCheckBankAccountName = String.Empty;

		public Address()
		{
		}

		public Address(int CustomerID)
		{
			m_customerID = CustomerID;
		}

		public Address(int CustomerID, AddressTypes AddressType)
		{
			m_customerID = CustomerID;
			m_addressType = AddressType;
		}

		#region Address Properties
		/// <summary>
		/// The CustomerID associated with this address
		/// </summary>
		public int CustomerID
		{
			get 
			{
				return m_customerID;
			}
			set 
			{
				m_customerID = value;
			}
		}

		/// <summary>
		/// The AddressID associated with this address
		/// </summary>
		public int AddressID
		{
			get 
			{
				return m_addressID;
			}
			set 
			{
				m_addressID = value;
			}
		}

		/// <summary>
		/// The ShippingMethodID associated with this address
		/// </summary>
		public int ShippingMethodID
		{
			get 
			{
				return m_shippingMethodID;
			}
			set 
			{
				m_shippingMethodID = value;
			}
		}

		/// <summary>
		/// The Display order for this address
		/// </summary>
		public int DisplayOrder
		{
			get 
			{
				return m_displayOrder;
			}
			set 
			{
				m_displayOrder = value;
			}
		}

		/// <summary>
		/// The First Name for this address
		/// </summary>
		public String FirstName
		{
			get 
			{
				return m_firstName;
			}
			set 
			{
				m_firstName = value.Trim();
			}
		}

		/// <summary>
		/// The Last Name for this address
		/// </summary>
		public String LastName
		{
			get 
			{
				return m_lastName;
			}
			set 
			{
				m_lastName = value.Trim();
			}
		}

		/// <summary>
		/// The Company name for this address
		/// </summary>
		public String Company
		{
			get 
			{
				return m_company;
			}
			set 
			{
				m_company = value.Trim();
			}
		}

		/// <summary>
		/// The First address line for this address
		/// </summary>
		public String Address1
		{
			get 
			{
				return m_address1;
			}
			set 
			{
				m_address1 = value.Trim();
			}
		}

		/// <summary>
		/// The second address line for this address
		/// </summary>
		public String Address2
		{
			get 
			{
				return m_address2;
			}
			set 
			{
				m_address2 = value.Trim();
			}
		}

		/// <summary>
		/// The suite line for this address
		/// </summary>
		public String Suite
		{
			get 
			{
				return m_suite;
			}
			set 
			{
				m_suite = value.Trim();
			}
		}

		/// <summary>
		/// The City for this address
		/// </summary>
		public String City
		{
			get 
			{
				return m_city;
			}
			set 
			{
				m_city = value.Trim();
			}
		}

		/// <summary>
		/// The State for this address
		/// </summary>
		public String State
		{
			get 
			{
				return m_state;
			}
			set 
			{
				m_state = value.Trim();
			}
		}

		/// <summary>
		/// The postal code for this address
		/// </summary>
		public String Zip
		{
			get 
			{
				return m_zip;
			}
			set 
			{
				m_zip = value.Trim();
			}
		}

		/// <summary>
		/// The Country name for this address
		/// </summary>
		public String Country
		{
			get 
			{
				return m_country;
			}
			set 
			{
				m_country = value.Trim();
			}
		}

		/// <summary>
		/// The Phone number for this address
		/// </summary>
		public String Phone
		{
			get 
			{
				return m_phone;
			}
			set 
			{
				m_phone = value.Trim();
			}
		}

		/// <summary>
		/// The email for this address
		/// </summary>
		public String Email
		{
			get 
			{
				return m_email;
			}
			set 
			{
				m_email = value.Trim();
			}
		}

		/// <summary>
		/// Returns the AddressType (Unknown, Billing, Shipping, Account)
		/// Records retrieved from the Address table are Unknown
		/// Records retrieved from the Customer table are of the type the were pulled from
		/// </summary>
		public AddressTypes AddressType
		{
			get
			{
				return (m_addressType);
			}
			set
			{
				m_addressType = value;
			}
		}

		/// <summary>
		/// The shipping method for this address.
		/// </summary>
		public String ShippingMethod
		{
			get 
			{
				//string[] s = m_shippingMethod.Split('|');
				//return s[0];
				return  m_shippingMethod;
			}
			set 
			{
				m_shippingMethod = value.Trim();
			}
		}

		/// <summary>
		/// The payment method for this address.
		/// </summary>
		public String PaymentMethod
		{
			get 
			{
				if (m_paymentMethod.Length==0)
				{
					m_paymentMethod = "Credit Card";
				}
				return m_paymentMethod;
			}
			set 
			{
				m_paymentMethod = value.Trim();
			}
		}

		/// <summary>
		/// The Card Type for this address.
		/// </summary>
		public String CardType
		{
			get 
			{
				return m_cardType;
			}
			set 
			{
				m_cardType = value.Trim();
			}
		}

		/// <summary>
		/// The Card Name for this address.
		/// </summary>
		public String CardName
		{
			get 
			{
				if (m_cardName.Length == 0)
				{
					m_cardName = String.Format("{0} {1}",this.FirstName,this.LastName);
				}
				return m_cardName;
			}
			set 
			{
				m_cardName = value.Trim();
			}
		}

		/// <summary>
		/// The Card Number for this address.
		/// </summary>
		public String CardNumber
		{
			get 
			{
				return m_cardNumber;
			}
			set 
			{
				m_cardNumber = value.Trim();
			}
		}

		/// <summary>
		/// The Card Extra Code for this address (CVV).
		/// </summary>
		public String CardExtraCode
		{
			get 
			{
				return m_cardExtraCode;
			}
			set 
			{
				m_cardExtraCode = value.Trim();
			}
		}

		/// <summary>
		/// The Card Expiration Month for this address.
		/// </summary>
		public String CardExpirationMonth
		{
			get 
			{
				return m_cardExpirationMonth;
			}
			set 
			{
				m_cardExpirationMonth = value.Trim();
			}
		}

		/// <summary>
		/// The Card Expiration Month for this address.
		/// </summary>
		public String CardExpirationYear
		{
			get 
			{
				return m_cardExpirationYear;
			}
			set 
			{
				m_cardExpirationYear = value.Trim();
			}
		}

		/// <summary>
		/// The eCheck ABA Code for this address.
		/// </summary>
		public String ECheckBankABACode
		{
			get 
			{
				return m_eCheckBankABACode;
			}
			set 
			{
				m_eCheckBankABACode = value.Trim();
			}
		}

		/// <summary>
		/// The eCheck Account Number Code for this address.
		/// </summary>
		public String ECheckBankAccountNumber
		{
			get 
			{
				return m_eCheckBankAccountNumber;
			}
			set 
			{
				m_eCheckBankAccountNumber = value.Trim();
			}
		}

		/// <summary>
		/// The eCheck Account Number Code for this address.
		/// </summary>
		public String ECheckBankAccountType
		{
			get 
			{
				return m_eCheckBankAccountType;
			}
			set 
			{
				m_eCheckBankAccountType = value.Trim();
			}
		}

		/// <summary>
		/// The eCheck Account Number Code for this address.
		/// </summary>
		public String ECheckBankName
		{
			get 
			{
				return m_eCheckBankName;
			}
			set 
			{
				m_eCheckBankName = value.Trim();
			}
		}

		/// <summary>
		/// The eCheck Account Number Code for this address.
		/// </summary>
		public String ECheckBankAccountName
		{
			get 
			{
				if (m_eCheckBankAccountName.Length == 0)
				{
					m_eCheckBankAccountName = String.Format("{0} {1}",this.FirstName,this.LastName);
				}
				return m_eCheckBankAccountName;
			}
			set 
			{
				m_eCheckBankAccountName = value.Trim();
			}
		}

		/// <summary>
		/// The Card Number obscured for security for this address.
		/// </summary>
		public String SafeDisplayCardNumber
		{
			get 
			{
				if(m_cardNumber.Length > 4)
				{
					return String.Format("****{0}",m_cardNumber.Substring(m_cardNumber.Length-4,4));
				}
				else
				{
					return String.Empty;
				}
			}
		}

		/// <summary>
		/// The Card Number obscured for security for this address.
		/// </summary>
		public String SafeDisplayCardExtraCode
		{
			get 
			{
				if(m_cardExtraCode.Length != 0)
				{
					return "***";
					}
				else
				{
					return String.Empty;
				}
			}
		}

		/// <summary>
		/// The eCheck Account Number Code for this address.
		/// </summary>
		public String DisplayPaymentMethod
		{
			get 
			{
				if (this.PaymentMethod.ToUpper() == "MICROPAY")
				{
					return String.Format(Common.AppConfig("Micropay.Prompt") + " - {0}",Localization.CurrencyStringForDisplay(Common.GetMicroPayBalance(this.CustomerID)));
				}
				if (this.PaymentMethod.ToUpper().Replace(" ","") == "ECHECK")
				{
					return String.Format("eCheck - {0}: {1} {2}",this.ECheckBankName,this.ECheckBankABACode,this.ECheckBankAccountNumber);
				}
				if (this.PaymentMethod.ToUpper().Replace(" ","") == "CREDITCARD")
				{
					return String.Format("Credit Card - {0}: {1} {2}/{3}",m_cardType,Common.IIF(Common.IsAdminSite,this.CardNumber,this.SafeDisplayCardNumber),this.CardExpirationMonth,this.CardExpirationYear);
				}
				return String.Empty;
			}
		}

		/// <summary>
		/// The Card Number obscured for security for this address.
		/// </summary>
		public String SafeDisplayECheckBankAccountNumber
		{
			get 
			{
				if(m_eCheckBankAccountNumber.Length > 4)
				{
					return String.Format("****{0}",m_eCheckBankAccountNumber.Substring(m_cardNumber.Length-4,4));
				}
				else
				{
					return String.Empty;
				}
			}
		}

		#endregion


		#region Address Methods

		/// <summary>
		/// Creates an array of Address sql parameters that can be used by String.Format to build SQL statements.
		/// </summary>
		/// <returns>object[]</returns>
		private object[] AddressValues
		{
			get
			{
				// Munge 'em for security
				string cnMunged = String.Empty;
				if(this.CardNumber.Length != 0)
				{
					cnMunged = Common.MungeString(this.CardNumber);
				}
				string cvMunged = String.Empty;
				if(this.CardExtraCode.Length != 0)
				{
					cvMunged = Common.MungeString(this.CardExtraCode);
				}
				Customer thisCustomer = new Customer();

				object[] values = new object[] 
		  {
			  this.AddressID,                         //{0}
			  this.CustomerID,                        //{1}
			  this.ShippingMethodID,                  //{2}
			  DB.SQuote(this.FirstName),              //{3}
			  DB.SQuote(this.LastName),               //{4}
			  DB.SQuote(this.Company),                //{5}
			  DB.SQuote(this.Address1),               //{6}
			  DB.SQuote(this.Address2),               //{7}
			  DB.SQuote(this.Suite),                  //{8}
			  DB.SQuote(this.City),                   //{9}
			  DB.SQuote(this.State),                  //{10}
			  DB.SQuote(this.Zip),                    //{11}
			  DB.SQuote(this.Country),                //{12}
			  DB.SQuote(this.Phone),                  //{13}
			  DB.SQuote(this.ShippingMethod),         //{14}
			  DB.SQuote(this.PaymentMethod),          //{15}
			  DB.SQuote(this.CardType),               //{16}
			  DB.SQuote(cnMunged),                    //{17}
			  DB.SQuote(cvMunged),                    //{18}
			  DB.SQuote(this.CardName),               //{19}
			  DB.SQuote(this.CardExpirationMonth),    //{20}
			  DB.SQuote(this.CardExpirationYear),     //{21}
			  DB.SQuote(this.ECheckBankABACode),      //{22}
			  DB.SQuote(this.ECheckBankAccountNumber),//{23}
			  DB.SQuote(this.ECheckBankAccountType),  //{24}
			  DB.SQuote(this.ECheckBankName),         //{25}
			  DB.SQuote(this.ECheckBankAccountName),  //{26}
			  DB.SQuote(this.Email),                  //{27}
			  thisCustomer._customerID                //{28} Last UpdatedBy
		  };
				return values;
			}
		}
    
		public void Clear()
		{
			m_customerID = 0;
			m_addressID = 0;
			m_displayOrder = 0;
    
			m_addressType = AddressTypes.Unknown;

			m_firstName = String.Empty;
			m_lastName = String.Empty;
			m_company = String.Empty;
			m_address1 = String.Empty;
			m_address2 = String.Empty;
			m_suite = String.Empty;
			m_city = String.Empty;
			m_state = String.Empty;
			m_zip = String.Empty;
			m_country = String.Empty;
			m_phone = String.Empty;
			m_shippingMethod = String.Empty;
			m_paymentMethod = String.Empty;
			m_cardType = String.Empty;
			m_cardNumber = String.Empty;
			m_cardName = String.Empty;
			m_cardExpirationMonth = String.Empty;
			m_cardExpirationYear = String.Empty;
			m_eCheckBankABACode = String.Empty;
			m_eCheckBankAccountNumber = String.Empty;
			m_eCheckBankAccountType = String.Empty;
			m_eCheckBankName = String.Empty;
			m_eCheckBankAccountName = String.Empty;
			m_email = String.Empty;
		}

		/// <summary>
		/// Adds an address to the Address Table associated with a passed CustomerID
		/// </summary>
		public void InsertDB(int aCustomerID)
		{
			this.CustomerID = aCustomerID;
			this.InsertDB();
		}

		/// <summary>
		/// Adds an address to the Address Table
		/// </summary>
		public void InsertDB()
		{
			string AddressGUID = Common.GetNewGUID();
			string sql = String.Format("insert into Address(AddressGUID,CustomerID) values({0},{1})",DB.SQuote(AddressGUID),this.CustomerID);
			DB.ExecuteSQL(sql);

			IDataReader rs = DB.GetRS(String.Format("select AddressID from Address where AddressGUID={0}",DB.SQuote(AddressGUID)));
			rs.Read();
			this.AddressID = DB.RSFieldInt(rs,"AddressID");
			rs.Close();
			UpdateDB();
		}

		public void UpdateDB()
		{
			string sql = String.Format("update Address set CustomerID={1},ShippingMethodID={2},FirstName={3},LastName={4},Company={5},Address1={6},Address2={7},Suite={8},City={9},State={10},Zip={11},Country={12},Phone={13},ShippingMethod={14},PaymentMethod={15},CardType={16},CardNumber={17},CardExtraCode={18},CardName={19},CardExpirationMonth={20},CardExpirationYear={21},eCheckBankABACode={22},eCheckBankAccountNumber={23},eCheckBankAccountType={24},eCheckBankName={25},eCheckBankAccountName={26},eMail={27},LastUpdatedBy={28} where AddressID={0}",AddressValues);
			DB.ExecuteSQL(sql);
		}

		/// <summary>
		/// Return a count of number of addresses associated with this customerID
		/// </summary>
		public int Count(int CustomerID)
		{
			return DB.GetSqlN(String.Format("select count(*) as N from Address where CustomerID={0}",CustomerID)); 
		}

		/// <summary>
		/// Deletes an address to the Address Table
		/// </summary>
		public void DeleteDB(int addressID)
		{
			this.LoadFromDB(addressID);
			int customerID = this.CustomerID;
			this.LoadByCustomer(customerID,AddressTypes.Billing);
			if (this.AddressID == addressID)
			{
				this.AddressID = 0;
				this.CopyToCustomerDB(AddressTypes.Billing);
			}
			this.LoadByCustomer(customerID,AddressTypes.Shipping);
			if (this.AddressID == addressID)
			{
				this.AddressID = 0;
				this.CopyToCustomerDB(AddressTypes.Shipping);
			}
			this.Clear();
			string sql = String.Format("delete from Address where AddressID={0}",addressID);
			DB.ExecuteSQL(sql);
		}
    
		public void CopyToCustomerDB(AddressTypes aAddressType)
		{
			//An address could be both Type Shipping and Billing save both to Customer if so.
      
			string sql = String.Empty;

			if ((aAddressType & AddressTypes.Billing) != 0)
			{
				sql = "BillingAddressID={0}";
			}
			if ((aAddressType & AddressTypes.Shipping) != 0)
			{
				if (sql.Length != 0) sql += ",";
				sql += "ShippingAddressID={0}";
			}
			sql = "update Customer set " + sql + " where CustomerID={1}";
			sql = String.Format(sql,AddressValues);
			DB.ExecuteSQL(sql);
		}

		public void CopyToShoppingCartDB(int ShoppingCartID,AddressTypes aAddressType)
		{
			//An address could be both Type Shipping and Billing save both to Customer if so.
      
			string sql = String.Empty;

			if ((aAddressType & AddressTypes.Billing) != 0)
			{
				sql = "BillingAddressID={0}";
			}
			if ((aAddressType & AddressTypes.Shipping) != 0)
			{
				if (sql.Length != 0) sql += ",";
				sql += "ShippingAddressID={0}";
			}
			sql = "update ShoppingCart set " + sql + " where ShoppingCartID={1}";
			sql = String.Format(sql,this.AddressID,ShoppingCartID);
			DB.ExecuteSQL(sql);
		}

		public void LoadFromDB(int AddressID)
		{
			IDataReader rs = DB.GetRS(String.Format("select * from Address where AddressID = {0}",AddressID));
			if (rs.Read())
			{
				this.AddressID = DB.RSFieldInt(rs,"AddressID"); 
				this.CustomerID = DB.RSFieldInt(rs,"CustomerID");
				this.ShippingMethodID = DB.RSFieldInt(rs,"ShippingMethodID");
				this.FirstName = DB.RSField(rs,"FirstName");
				this.LastName = DB.RSField(rs,"LastName");
				this.Company = DB.RSField(rs,"Company");
				this.Address1 = DB.RSField(rs,"Address1");
				this.Address2 = DB.RSField(rs,"Address2");
				this.Suite = DB.RSField(rs,"Suite");
				this.City = DB.RSField(rs,"City");
				this.State = DB.RSField(rs,"State");
				this.Zip = DB.RSField(rs,"Zip");
				this.Country = DB.RSField(rs,"Country");
				this.Phone = DB.RSField(rs,"Phone");

				this.ShippingMethod = DB.RSField(rs,"ShippingMethod");
				this.PaymentMethod = DB.RSField(rs,"PaymentMethod");
        
				this.CardType = DB.RSField(rs,"CardType");
        
				this.CardNumber = DB.RSField(rs,"CardNumber");
				if (this.CardNumber.Length != 0)
				{
					this.CardNumber =  Common.UnmungeString(this.CardNumber);
				}

				this.CardExtraCode = DB.RSField(rs,"CardExtraCode");
				if (this.CardExtraCode.Length != 0)
				{
					this.CardExtraCode = Common.UnmungeString(this.CardExtraCode);
				}

				this.CardName = DB.RSField(rs,"CardName");
				this.CardExpirationMonth = DB.RSField(rs,"CardExpirationMonth");
				this.CardExpirationYear = DB.RSField(rs,"CardExpirationYear");
				this.ECheckBankABACode = DB.RSField(rs,"ECheckBankABACode");
				this.ECheckBankAccountNumber = DB.RSField(rs,"ECheckBankAccountNumber");
				this.ECheckBankAccountType = DB.RSField(rs,"ECheckBankAccountType");
				this.ECheckBankName = DB.RSField(rs,"ECheckBankName");
				this.ECheckBankAccountName = DB.RSField(rs,"ECheckBankAccountName");
				this.Email = DB.RSField(rs,"Email");
			}
			else
			{
				Clear();
			}
			rs.Close();
		}

		public void LoadByCustomer(int CustomerID, AddressTypes aAddressType)
		{
			int ShippingAddressID = 0;
			int BillingAddressID = 0;
			string CustomerEmail = String.Empty;

			//Get the address ids from Customer table
			IDataReader rs = DB.GetRS(String.Format("select ShippingAddressID,BillingAddressID,Email from Customer where CustomerID = {0}",CustomerID));
			if (rs.Read())
			{
				ShippingAddressID = DB.RSFieldInt(rs,"ShippingAddressID");
				BillingAddressID = DB.RSFieldInt(rs,"BillingAddressID");
				CustomerEmail = DB.RSField(rs,"Email");
			}
			rs.Close();
			if (aAddressType == AddressTypes.Billing)
			{
				this.AddressID = BillingAddressID;
			}
			if (aAddressType == AddressTypes.Shipping)
			{
				this.AddressID = ShippingAddressID;
			}
			if (this.AddressID != 0)
			{
				LoadFromDB(this.AddressID);
				this.AddressType |= aAddressType;
				this.Email = CustomerEmail;
			}
			else
			{
				this.Clear();
			}
		}

		public string DisplayHTML()
		{
			return DisplayString(true,"<br>");
		}
    
		public string DisplayString(bool IncludePhone,string separator)
		{
			string s = String.Empty;
      
			StringBuilder tmpS = new StringBuilder(1000);
			tmpS.Append(String.Format("{0} {1}{2}",FirstName, LastName,separator));
			tmpS.Append(Common.IIF(this.Company.Length != 0 , Company + separator , ""));
			tmpS.Append(Common.IIF(this.Address1.Length != 0 , Address1 + separator , ""));
			tmpS.Append(Common.IIF(this.Address2.Length != 0 , Address2 + separator , ""));
			tmpS.Append(Common.IIF(this.Suite.Length != 0 , Suite + separator , ""));
			s = String.Format("{0}, {1} {2}",City,State,Zip);
			if (s.Length > 3) tmpS.Append(s + separator);
			tmpS.Append(Common.IIF(this.Country.Length != 0 , Country + separator , ""));
			if (IncludePhone) tmpS.Append(Common.IIF(this.Phone.Length != 0 , Phone + separator , ""));
			return tmpS.ToString();
		}

		public string DisplayCardHTML()
		{
			return DisplayCardString("<br>");
		}
    
		public string DisplayCardString(string separator)
		{
			StringBuilder tmpS = new StringBuilder(1000);
			tmpS.Append(this.CardName + separator);
			if (Customer.StaticIsAdminUser(Common.SessionUSInt("CustomerID")))
			{
				tmpS.Append(String.Format("{0}: {1}{2}",this.CardType,this.CardNumber,separator));
			}
			else
			{
				tmpS.Append(String.Format("{0}: ***-{1}{2}",this.CardType,this.CardNumber.Substring(10,5),separator));
			}
			tmpS.Append(String.Format("{0:0#}/{1:000#}{2}",this.CardExpirationMonth,this.CardExpirationYear,separator));
			return tmpS.ToString();
		}

		public string DisplayECheckHTML()
		{
			StringBuilder tmpS = new StringBuilder(1000);
			tmpS.Append("<b>Payment Method:</b><br>");
			tmpS.Append(this.PaymentMethod + "<br>");
			tmpS.Append(this.ECheckBankName + "<br>");
			tmpS.Append(String.Format("{0} : ***-{1}<br>",this.ECheckBankABACode,this.ECheckBankAccountNumber.Substring(this.ECheckBankAccountNumber.Length-5,5)));
			return tmpS.ToString();
		}

		public string InputHTML()
		{
      
			bool AllowShipToDifferentThanBillTo = Common.AppConfigBool("AllowShipToDifferentThanBillTo");
      
			StringBuilder tmpS = new StringBuilder(1000);

			tmpS.Append("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			tmpS.Append("function AddressInputForm_Validator(theForm)\n");
			tmpS.Append("{\n");
			tmpS.Append("  submitonce(theForm);\n");
			tmpS.Append("  if (theForm.AddressState.selectedIndex < 1)\n");
			tmpS.Append("  {\n");
			tmpS.Append("    alert(\"Please select one of the State options. If Outside U.S, select 'Other (Non US)'\");\n");
			tmpS.Append("    theForm.AddressState.focus();\n");
			tmpS.Append("    submitenabled(theForm);\n");
			tmpS.Append("    return (false);\n");
			tmpS.Append("  }\n");
			tmpS.Append("  return (true);\n");
			tmpS.Append("}\n");
			tmpS.Append("</script>\n");

			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" >\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			
			tmpS.Append("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
			tmpS.Append("      <tr>");
			tmpS.Append("        <td width=\"25%\">First Name:</td>");
			tmpS.Append("        <td width=\"75%\">");
			tmpS.Append("        <input type=\"text\" name=\"AddressFirstName\" size=\"20\" maxlength=\"50\" value=\"" + this.FirstName + "\"> (required)");
			tmpS.Append("        <input type=\"hidden\" name=\"AddressFirstName_vldt\" value=\"[req][blankalert=Please enter the first name]\">\n");
			tmpS.Append("        </td>");
			tmpS.Append("      </tr>");
			tmpS.Append("      <tr>");
			tmpS.Append("        <td width=\"25%\">Last Name:</td>");
			tmpS.Append("        <td width=\"75%\">");
			tmpS.Append("        <input type=\"text\" name=\"AddressLastName\" size=\"20\" maxlength=\"50\" value=\"" + this.LastName + "\"> (required)");
			tmpS.Append("        <input type=\"hidden\" name=\"AddressLastName_vldt\" value=\"[req][blankalert=Please enter the last name]\">\n");
			tmpS.Append("        </td>");
			tmpS.Append("      </tr>");
			tmpS.Append("      <tr>");
			tmpS.Append("        <td width=\"25%\">Phone:</td>");
			tmpS.Append("        <td width=\"75%\">");
			tmpS.Append("        <input type=\"text\" name=\"AddressPhone\" size=\"20\" maxlength=\"25\" value=\"" + this.Phone + "\"> (required)");
			tmpS.Append("        <input type=\"hidden\" name=\"AddressPhone_vldt\" value=\"[req][blankalert=Please enter a phone number for the address][invalidalert=Please enter a valid phone number]\">");
			tmpS.Append("        </td>");
			tmpS.Append("      </tr>");
			tmpS.Append("      <tr>");
			tmpS.Append("        <td width=\"25%\">Company:</td>");
			tmpS.Append("        <td width=\"75%\">");
			tmpS.Append("        <input type=\"text\" name=\"AddressCompany\" size=\"34\" maxlength=\"100\" value=\"" + this.Company + "\">");
			tmpS.Append("        </td>");
			tmpS.Append("      </tr>");
			tmpS.Append("      <tr>");
			tmpS.Append("        <td width=\"25%\">Address1:</td>");
			tmpS.Append("        <td width=\"75%\">");
			tmpS.Append("        <input type=\"text\" name=\"AddressAddress1\" size=\"34\" maxlength=\"100\" value=\"" + this.Address1 + "\"> (required)");
			tmpS.Append("        <input type=\"hidden\" name=\"AddressAddress1_vldt\" value=\"[req][blankalert=Please enter an address]\">\n");
			tmpS.Append("        </td>");
			tmpS.Append("      </tr>");
			tmpS.Append("      <tr>");
			tmpS.Append("        <td width=\"25%\">Address2:</td>");
			tmpS.Append("        <td width=\"75%\">");
			tmpS.Append("        <input type=\"text\" name=\"AddressAddress2\" size=\"34\" maxlength=\"100\" value=\"" + this.Address2 + "\">");
			tmpS.Append("        </td>");
			tmpS.Append("      </tr>");
			tmpS.Append("      <tr>");
			tmpS.Append("        <td width=\"25%\">Suite:</td>");
			tmpS.Append("        <td width=\"75%\">");
			tmpS.Append("        <input type=\"text\" name=\"AddressSuite\" size=\"34\" maxlength=\"50\" value=\"" + this.Suite + "\">");
			tmpS.Append("        </td>");
			tmpS.Append("      </tr>");
			tmpS.Append("      <tr>");
			tmpS.Append("        <td width=\"25%\">City or APO/AFO:</td>");
			tmpS.Append("        <td width=\"75%\">");
			tmpS.Append("        <input type=\"text\" name=\"AddressCity\" size=\"34\" maxlength=\"50\" value=\"" + this.City + "\"> (required)");
			tmpS.Append("        <input type=\"hidden\" name=\"AddressCity_vldt\" value=\"[req][blankalert=Please enter a city]\">\n");
			tmpS.Append("        </td>");
			tmpS.Append("      </tr>");
			tmpS.Append("      <tr>");
			tmpS.Append("        <td width=\"25%\">State/Province:</td>");
			tmpS.Append("        <td width=\"75%\">");
      
			tmpS.Append("        <select size=\"1\" name=\"AddressState\">");
			tmpS.Append("        <OPTION value=\"\"" + Common.IIF((this.State.Length == 0)," selected",String.Empty) + " >SELECT ONE</option>");

			DataSet dsstate = DB.GetDS("select * from state  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddHours(1));
			foreach(DataRow row in dsstate.Tables[0].Rows)
			{
				tmpS.Append("<OPTION value=\"" + DB.RowField(row,"Abbreviation") + "\"" + Common.IIF(DB.RowField(row,"Abbreviation") == this.State," selected",String.Empty) + ">" + DB.RowField(row,"Name") + "</option>");
			}
			dsstate.Dispose();
			tmpS.Append("        </select> (required)");
			//      tmpS.Append("        <input type=\"hidden\" name=\"State_vldt\" value=\"[req][blankalert=Please select a  state]\">\n");
			tmpS.Append("        </td>");
			tmpS.Append("      </tr>");

			tmpS.Append("      <tr>");
			tmpS.Append("        <td width=\"25%\">Zip:</td>");
			tmpS.Append("        <td width=\"75%\">");
			tmpS.Append("        <input type=\"text\" name=\"AddressZip\" size=\"14\" maxlength=\"10\" value=\"" + this.Zip + "\"> (required)");
			tmpS.Append("        <input type=\"hidden\" name=\"AddressZip_vldt\" value=\"[req][blankalert=Please enter the zipcode][invalidalert=Please enter a valid zipcode]\">");
			tmpS.Append("        </td>");
			tmpS.Append("      </tr>");
			tmpS.Append("      <tr>");
			tmpS.Append("        <td width=\"25%\">Country:</td>");
			tmpS.Append("        <td width=\"75%\">");
			tmpS.Append("        <SELECT NAME=\"AddressCountry\" size=\"1\">");
      
			DataSet dscountry = DB.GetDS("select * from country  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddHours(1));
			foreach(DataRow row in dscountry.Tables[0].Rows)
			{
				tmpS.Append("<OPTION value=\"" + DB.RowField(row,"Name") + "\"" + Common.IIF(DB.RowField(row,"Name") == this.Country," selected",String.Empty) + ">" + DB.RowField(row,"Name") + "</option>");
			}
			dscountry.Dispose();
			tmpS.Append("        </SELECT>");
			tmpS.Append("        </td>");
			tmpS.Append("      </tr>");
			tmpS.Append("    </table>\n");
			tmpS.Append("   </td>");
			tmpS.Append(" </tr>");
			tmpS.Append("</table>\n");

      
			return tmpS.ToString();
		}

		public String InputCardHTML(bool Validate)
		{
			StringBuilder tmpS = new StringBuilder(1000);

			tmpS.Append(Common.ReadFile("tip2.js",false));
      
			//CardInputForm_Validator
			tmpS.Append("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			tmpS.Append("function CardInputForm_Validator(theForm)\n");
			tmpS.Append("	{\n");
			tmpS.Append("	submitonce(theForm);\n");
			if (Validate)
			{
				tmpS.Append("	if (theForm.CardType.selectedIndex < 1)\n");
				tmpS.Append("	{\n");
				tmpS.Append("		alert(\"Please select the type of credit card you are using.\");\n");
				tmpS.Append("		theForm.CardType.focus();\n");
				tmpS.Append("		submitenabled(theForm);\n");
				tmpS.Append("		return (false);\n");
				tmpS.Append("    }\n");
				tmpS.Append("	if (theForm.CardExpirationMonth.selectedIndex < 1)\n");
				tmpS.Append("	{\n");
				tmpS.Append("		alert(\"Please select the expiration month.\");\n");
				tmpS.Append("		theForm.CardExpirationMonth.focus();\n");
				tmpS.Append("		submitenabled(theForm);\n");
				tmpS.Append("		return (false);\n");
				tmpS.Append("    }\n");
				tmpS.Append("	if (theForm.CardExpirationYear.selectedIndex < 1)\n");
				tmpS.Append("	{\n");
				tmpS.Append("		alert(\"Please select the expiration year.\");\n");
				tmpS.Append("		theForm.CardExpirationYear.focus();\n");
				tmpS.Append("		submitenabled(theForm);\n");
				tmpS.Append("		return (false);\n");
				tmpS.Append("    }\n");

				tmpS.Append("	if (typeof(theForm.TermsAndConditionsRead) != 'undefined')\n");
				tmpS.Append("	{\n");
				tmpS.Append("	  if (!theForm.TermsAndConditionsRead.checked)\n");
				tmpS.Append("	  {\n");
				tmpS.Append("	  	alert(\"Please indicate your acceptance of the Terms and Conditions before proceeding. If you need assistance, please contact us.\");\n");
				tmpS.Append("		  theForm.TermsAndConditionsRead.focus();\n");
				tmpS.Append("		  submitenabled(theForm);\n");
				tmpS.Append("		  return (false);\n");
				tmpS.Append("   }\n");
				tmpS.Append("  }\n");
			}
			tmpS.Append("	return (true);\n");
			tmpS.Append("	}\n");
			tmpS.Append("</script>\n");
      
			// Credit Card fields

			tmpS.Append("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
			tmpS.Append("      <tr><td colspan=2 height=10></td></tr>");
			tmpS.Append("      <tr>");
			tmpS.Append("        <td width=\"25%\">Name On Card:</td>\n");
			tmpS.Append("        <td width=\"75%\">\n");
			tmpS.Append("        <input type=\"text\" name=\"CardName\" size=\"20\" maxlength=\"100\" value=\"" + HttpContext.Current.Server.HtmlEncode(this.CardName) + "\">\n");
			if (Validate)
			{
				tmpS.Append("        <input type=\"hidden\" name=\"CardName_vldt\" value=\"[req][blankalert=Please enter the name on the credit card]\">\n");
			}
			tmpS.Append("        </td>");
			tmpS.Append("      </tr>");
			tmpS.Append("     <tr><td colspan=2 height=2></td></tr>");
			tmpS.Append("      <tr>");
			tmpS.Append("        <td width=\"25%\">Credit Card Number:</td>");
			tmpS.Append("        <td width=\"75%\">\n");
				
			tmpS.Append("        <input type=\"text\" autocomplete=\"off\" name=\"CardNumber\" size=\"20\" maxlength=\"20\" value=\"" + Common.IIF(Common.IsAdminSite,this.CardNumber,this.SafeDisplayCardNumber) + "\"> (no spaces)\n");
			if (Validate)
			{
				tmpS.Append("        <input type=\"hidden\" name=\"CardNumber_vldt\" value=\"[req][len=8][blankalert=Please enter credit card number with no spaces][invalidalert=Please enter a valid credit card number]\">\n");
			}
			tmpS.Append("        </td>");
			tmpS.Append("      </tr>");
			tmpS.Append("     <tr><td colspan=2 height=2></td></tr>");
			tmpS.Append("      <tr>");
			tmpS.Append("        <td width=\"25%\">Credit Card Verfication Code:</td>");
			tmpS.Append("        <td width=\"75%\">\n");
			tmpS.Append("        <input type=\"text\" autocomplete=\"off\" name=\"CardExtraCode\" size=\"5\" maxlength=\"10\" value=\""+ Common.IIF(Common.IsAdminSite,this.CardExtraCode,SafeDisplayCardExtraCode) +"\">\n");
			tmpS.Append("(<a href=\"javascript:void(0);\" style=\"cursor: normal;\" onMouseover=\"ddrivetip('<iframe width=400 height=370 frameborder=0 marginheight=2 marginwidth=2 scrolling=no src=skins/skin_" + _siteID.ToString() + "/images/verificationnumber.gif></iframe>','" + Common.AppConfig("LightCellColor") + "', 300)\" onMouseout=\"hideddrivetip()\">what's this?</a>)");
			if (Validate)
			{
				tmpS.Append("        <input type=\"hidden\" name=\"CardExtraCode_vldt\" value=\"" + Common.IIF(!Common.AppConfigBool("CardExtraCodeIsOptional") , "[req]" , "") + "[len=3][blankalert=Please enter the credit card verification code with no spaces. This number can be found printed on the back side of your card][invalidalert=Please enter a valid credit card validation code]\">\n");
			}
			tmpS.Append("        </td>");
			tmpS.Append("      </tr>");
			tmpS.Append("      <tr><td width=\"100%\" height=\"10\" colspan=\"2\"></td></tr>");
			tmpS.Append("      <tr>");
			tmpS.Append("        <td width=\"25%\">Card Type:</td>");
			tmpS.Append("        <td width=\"75%\">\n");
			tmpS.Append("        <select size=\"1\" name=\"CardType\">");
			tmpS.Append("				<OPTION VALUE=\"\">CARD TYPE");

			IDataReader rsCard = DB.GetRS("select * from creditcardtype  " + DB.GetNoLock() + " order by CardType");
			while(rsCard.Read())
			{
				tmpS.Append("<OPTION " + Common.IIF(this.CardType == DB.RSField(rsCard,"CardType"), " selected ","") + ">" + DB.RSField(rsCard,"CardType") + "</option>\n");
			}
			rsCard.Close();

			tmpS.Append("              </select>\n");
			tmpS.Append("        </td>");
			tmpS.Append("      </tr>");
			tmpS.Append("      <tr><td width=\"100%\" height=\"2\" colspan=\"2\"></td></tr>");
			tmpS.Append("      <tr>");
			tmpS.Append("        <td width=\"25%\">Expiration Date:</td>");
			tmpS.Append("        <td width=\"75%\">\n");
			tmpS.Append("        <select size=\"1\" name=\"CardExpirationMonth\">");
			tmpS.Append("<OPTION VALUE=\"\">MONTH");
			for(int i = 1; i <= 12; i++)
			{
				tmpS.Append("<OPTION " + Common.IIF(this.CardExpirationMonth == i.ToString().PadLeft(2,'0'), " selected ","") + ">" + i.ToString().PadLeft(2,'0') + "</option>");
			}
			tmpS.Append("</select>    <select size=\"1\" name=\"CardExpirationYear\">");
			tmpS.Append("<OPTION VALUE=\"\" SELECTED>YEAR");
			for(int y = 0; y <= 10; y++)
			{
				tmpS.Append("<OPTION " + Common.IIF(this.CardExpirationYear == (System.DateTime.Now.Year + y).ToString(), " selected ","") + ">" + (System.DateTime.Now.Year + y).ToString() + "</option>");
			}
			tmpS.Append("</select></td></tr>\n");
			tmpS.Append("</table>\n");
     
			return tmpS.ToString();
		}

		public string InputECheckHTML(bool Validate)
		{
			StringBuilder tmpS = new StringBuilder(1000);

			tmpS.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
				
			tmpS.Append("  <tr><td width=\"100%\" height=\"10\" colspan=\"2\"></td></tr>");
			tmpS.Append("  <tr>");
			tmpS.Append("    <td width=\"25%\">*Name On Account:&nbsp;</td>\n");
			tmpS.Append("    <td width=\"75%\">\n");
			tmpS.Append("    <input type=\"text\" name=\"eCheckBankAccountName\" size=\"20\" maxlength=\"50\" value=\"" + HttpContext.Current.Server.HtmlEncode(this.FirstName + " " + this.LastName) + "\">\n");
			if (Validate)
			{
				tmpS.Append("    <input type=\"hidden\" name=\"eCheckBankAccountName_vldt\" value=\"[req][blankalert=Please enter the name on your checking account]\">\n");
			}
			tmpS.Append("        </td>");
			tmpS.Append("      </tr>");
				
			tmpS.Append("      <tr><td width=\"100%\" height=\"10\" colspan=\"2\"></td></tr>");
				
			tmpS.Append("      <tr>");
			tmpS.Append("        <td width=\"25%\">*Enter the name of your bank&nbsp;</td>\n");
			tmpS.Append("        <td width=\"75%\">\n");
			tmpS.Append("        <input type=\"text\" name=\"eCheckBankName\" size=\"20\" maxlength=\"50\" value=\"" + HttpContext.Current.Server.HtmlEncode(this.ECheckBankName) + "\"> e.g. Bank of America, Wells Fargo, etc.\n");
			if (Validate)
			{
				tmpS.Append("        <input type=\"hidden\" name=\"eCheckBankName_vldt\" value=\"[req][blankalert=Please enter the name of your bank, e.g. Bank of America, Wells Fargo, etc.]\">\n");
			}
			tmpS.Append("        </td>");
			tmpS.Append("      </tr>");
				
			tmpS.Append("      <tr><td width=\"100%\" height=\"10\" colspan=\"2\"></td></tr>");

			tmpS.Append("      <tr>");
			tmpS.Append("        <td width=\"25%\">*Enter the series of nine numbers&nbsp;<br>on your check <i>between</i> these symbols:&nbsp;</td>\n");
			tmpS.Append("        <td width=\"75%\">\n");
			tmpS.Append("        &nbsp;<img src=\"skins/skin_" + _siteID.ToString() + "/images/check_aba.gif\"><input type=\"text\" autocomplete=\"off\" name=\"eCheckBankABACode\" size=\"9\" maxlength=\"9\" value=\""+ this.ECheckBankABACode +"\"><img src=\"skins/Skin_" + _siteID.ToString() + "/images/check_aba.gif\"> (This is your Bank ABA Number)\n");
			if (Validate)
			{
				tmpS.Append("        <input type=\"hidden\" name=\"eCheckBankABACode_vldt\" value=\"[req][blankalert=Please enter your 9 digit bank routing number, with no spaces, and all leading zeros]\">\n");
			}
			tmpS.Append("        </td>");
			tmpS.Append("      </tr>");
				
			tmpS.Append("      <tr><td width=\"100%\" height=\"10\" colspan=\"2\"></td></tr>");
				
			tmpS.Append("      <tr>");
			tmpS.Append("        <td width=\"25%\">*Enter the series of numbers found&nbsp;<br>on your check <i>before</i> this symbol:&nbsp;</td>\n");
			tmpS.Append("        <td width=\"75%\">\n");
			tmpS.Append("&nbsp;<input type=\"text\" autocomplete=\"off\" name=\"eCheckBankAccountNumber\" size=\"15\" maxlength=\"25\" value=\""+ this.ECheckBankAccountNumber +"\"><img src=\"skins/skin_" + _siteID.ToString() + "/images/check_account.gif\"> (This is your Account Number)\n");
			if (Validate)
			{
				tmpS.Append("        <input type=\"hidden\" name=\"eCheckBankAccountNumber_vldt\" value=\"[req][blankalert=lease enter your checking or savings account number, with no spaces and all leading zeros]\">\n");
			}
			tmpS.Append("        </td>");
			tmpS.Append("      </tr>");
				
			tmpS.Append("      <tr><td width=\"100%\" height=\"10\" colspan=\"2\"></td></tr>");
				
			tmpS.Append("      <tr>");
			tmpS.Append("        <td width=\"25%\">*Account Type:&nbsp;</td>");
			tmpS.Append("        <td width=\"75%\">\n");
			tmpS.Append("        <select size=\"1\" name=\"eCheckBankAccountType\">");
			tmpS.Append("          <OPTION VALUE=\"CHECKING\" selected>CHECKING</OPTION>");
			tmpS.Append("		  		<OPTION VALUE=\"SAVINGS\">SAVINGS</OPTION>");
			tmpS.Append("			  	<OPTION VALUE=\"BUSINESS CHECKING\">BUSINESS CHECKING</OPTION>");
			tmpS.Append("        </select>\n");
			tmpS.Append("        </td>");
			tmpS.Append("      </tr>");

			tmpS.Append("      <tr><td width=\"100%\" height=\"10\" colspan=\"2\"></td></tr>");

			tmpS.Append("	  <tr><td colspan=\"2\">Note: Some banks use a non-standard format in this series of numbers. If your account number includes this symbol <img src=\"skins/skin_" + _siteID.ToString() + "/images/check_micr.gif\" align=\"absmiddle\">, simply disregard the symbols and enter this section of numbers.</td></tr>");
			tmpS.Append("</table>");
      
			return tmpS.ToString();
		}
		#endregion

	}

	public class Addresses : ArrayList
	{
		public Addresses()
		{
		}

		public new Address this [int index]
		{
			get
			{
				return (Address)base[index];
			}
			set
			{
				base[index] = value;
			}
		}

		public void LoadCustomer(int CustomerID)
		{
			string sql = String.Format("select AddressID from Address where CustomerID={0}",CustomerID);
			IDataReader rs = DB.GetRS(sql);
			while (rs.Read())
			{
				int AddressID = DB.RSFieldInt(rs,"AddressID");
				Address newAddress = new Address();
				newAddress.LoadFromDB(AddressID);
				this.Add(newAddress);
			}
			rs.Close();
		}
	}
}
