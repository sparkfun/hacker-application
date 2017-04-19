// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace AspDotNetStorefrontCommon
{

	public class RTShipping
	{

		private string[] DHLServices = {"Express","Express 10:30am","Next Afternoon","Second Day Service","Ground"};

		private string m_upsLogin;			// username, password, license
		private string m_upsServer;			// UPS server
		private string m_upsUsername;
		private string m_upsPassword;
		private string m_upsLicense;

		private string m_uspsLogin;			// username, password
		private string m_uspsServer;			// USPS server
		private string m_uspsUsername;
		private string m_uspsPassword;

		private string m_dhlAccountNumber;	 
		private string m_dhlApiSystemID;			     
		private string m_dhlApiSystemPassword;		 
		private string m_dhlShippingKey;
		private string m_dhlServer;
		//private string m_dhlTestServer;

		private string m_fedexAccountNumber;
		private string m_fedexServer;
		private string m_fedexMeter;			// Returned from Fedex after subscription
		
		private string m_originAddress;
		private string m_originAddress2;
		private string m_originCity;
		private string m_originStateProvince;
		private string m_originZipPostalCode;
		private string m_originCountry;
		private string m_destinationAddress;
		private string m_destinationAddress2;
		private string m_destinationCity;
		private string m_destinationStateProvince;
		private string m_destinationZipPostalCode;
		private string m_destinationCountry;

		private Single m_shipmentWeight;
		private decimal m_shipmentValue;
		private Single m_length;	// Length of the package in inches
		private Single m_width;	// Width of the package in inches
		private Single m_height;	// Height of the package in inches
		
		private bool m_testMode;
		
		private	ArrayList ratesValues;
		private ArrayList ratesText;

		static private String MapPickupType(String s)
		{
			s = s.Trim().ToLower();
			if(s == "upsdailypickup")
			{
				return "01";
			}
			if(s == "upscustomercounter")
			{
				return "03";
			}
			if(s == "upsonetimepickup")
			{
				return "06";
			}
			if(s == "upsoncallair")
			{
				return "07";
			}
			if(s == "upssuggestedretailrates")
			{
				return "11";
			}
			if(s == "upslettercenter")
			{
				return "19";
			}
			if(s == "upsairservicecenter")
			{
				return "20";
			}
			return "03"; // find some default
		}
		
		public string UPSLogin	// UPS Login infomration, "Username,Password,License" Please note: The login information is case sensitive
		{
			get { return m_upsLogin; }
			set
			{ 
				m_upsLogin = value;
				string[] arrUpsLogin = m_upsLogin.Split(',');
				try
				{
					m_upsUsername = arrUpsLogin[0].Trim();
					m_upsPassword = arrUpsLogin[1].Trim();
					m_upsLicense = arrUpsLogin[2].Trim();
				}
				catch {}
			}
		}

		public string DHLAccountNumber	// DHL Login infomration, "Username,Password" Please note: The login information is case sensitive
		{
			get { return m_dhlAccountNumber; }
			set { m_dhlAccountNumber = value.Trim();}
		}

		public string DHLApiSystemID	// DHL Login infomration, "Username,Password" Please note: The login information is case sensitive
		{ 
			get { return m_dhlApiSystemID; }
			set { m_dhlApiSystemID = value.Trim();}
		}

		public string DHLApiSystemPassword	// DHL Login infomration, "Username,Password" Please note: The login information is case sensitive
		{
			get { return m_dhlApiSystemPassword; }
			set { m_dhlApiSystemPassword = value.Trim();}
		}

		public string DHLServer	// URL To dhl server, either test or live
		{
			get { return m_dhlServer; }
			set { m_dhlServer = value.Trim(); }
		}

		public string DHLShippingKey	// URL To dhl server, either test or live
		{
			get { return m_dhlShippingKey; }
			set { m_dhlShippingKey = value.Trim(); }
		}

		/// FedEx login information, "Username,Password"
		public string FedexAccountNumber
		{
			get { return this.m_fedexAccountNumber; }
			set { this.m_fedexAccountNumber = value; }
		}

		/// FedEx Meter Number provided by FedEx after subscription
		public string FedexMeter
		{
			get { return this.m_fedexMeter; }
			set { this.m_fedexMeter = value; }
		}

		/// URL To FedEx server
		public string FedexServer
		{
			get { return this.m_fedexServer; }
			set { this.m_fedexServer = value; }
		}


		public string UPSServer	// URL To ups server, either test or live
		{
			get { return m_upsServer; }
			set { m_upsServer = value.Trim(); }
		}

		public string UPSUsername	// URL To ups server, either test or live
		{
			get { return m_upsUsername; }
			set { m_upsUsername = value.Trim(); }
		}

		public string UPSPassword	// URL To ups server, either test or live
		{
			get { return m_upsPassword; }
			set { m_upsPassword = value.Trim(); }
		}

		public string UPSLicense	// URL To ups server, either test or live
		{
			get { return m_upsLicense; }
			set { m_upsLicense = value.Trim(); }
		}

		public string USPSLogin	// USPS Login information, "Username,Password" Please note: The login information is case sensitive
		{
			get { return m_uspsLogin; }
			set
			{ 
				m_uspsLogin = value.Trim();
				string[] arrUSPSLogin = m_uspsLogin.Split(',');
				try
				{
					m_uspsUsername = arrUSPSLogin[0].Trim();
					m_uspsPassword = arrUSPSLogin[1].Trim();
				}
				catch {}
			}
		}

		public string USPSServer	// URL To usps server, either test or live
		{
			get { return m_uspsServer; }
			set { m_uspsServer = value.Trim(); }
		}

		public string USPSUsername	// URL To usps server, either test or live
		{
			get { return m_uspsUsername; }
			set { m_uspsUsername = value.Trim(); }
		}

		public string USPSPassword	// URL To usps server, either test or live
		{
			get { return m_uspsPassword; }
			set { m_uspsPassword = value.Trim(); }
		}

		public string DestinationAddress	// Shipment destination street address
		{
			get { return m_destinationAddress; }
			set { m_destinationAddress = value; }
		}

		public string DestinationAddress2	// Shipment destination street address continued
		{
			get { return m_destinationAddress2; }
			set { m_destinationAddress2 = value; }
		}

		public string DestinationCity	// Shipment destination city
		{
			get { return m_destinationCity; }
			set { m_destinationCity = value; }
		}

		public string DestinationStateProvince	// Shipment destination State or Province
		{
			get { return m_destinationStateProvince; }
			set { m_destinationStateProvince = value; }
		}

		public string DestinationZipPostalCode	// Shipment Destination Zip or Postal Code
		{
			get { return m_destinationZipPostalCode; }
			set { m_destinationZipPostalCode = value; }
		}

		public string DestinationCountry	// Shipment Destination Country
		{
			get { return m_destinationCountry; }
			set { m_destinationCountry = value; }
		}

		public string OriginAddress	// Shipment origin street address
		{
			get { return m_originAddress; }
			set { m_originAddress = value; }
		}

		public string OriginAddress2	// Shipment origin street address continued
		{
			get { return m_originAddress2; }
			set { m_originAddress2 = value; }
		}

		public string OriginCity	// Shipment origin city
		{
			get { return m_originCity; }
			set { m_originCity = value; }
		}

		public string OriginStateProvince	// Shipment origin State or Province
		{
			get { return m_originStateProvince; }
			set { m_originStateProvince = value; }
		}

		public string OriginZipPostalCode	// Shipment Origin Zip or Postal Code
		{
			get { return m_originZipPostalCode; }
			set { m_originZipPostalCode = value; }
		}

		public string OriginCountry	// Shipment Origin Country
		{
			get { return m_originCountry; }
			set { m_originCountry = value; }
		}

		public Single ShipmentWeight	// Shipment shipmentWeight
		{
			get { return m_shipmentWeight; }
			set { m_shipmentWeight = value; }
		}

		public decimal ShipmentValue	//  Shipment value
		{
			get { return m_shipmentValue; }
			set { m_shipmentValue = value; }
		}

		public bool TestMode	// Boolean value to set entire class into test mode. Only test servers will be used if applicable
		{
			get { return m_testMode; }
			set { m_testMode = value; }
		}

		public Single Length	// Single value representing the lenght of the package in inches
		{
			get { return m_length; }
			set { m_length = value; }
		}

		public Single Width	// Single value representing the width of the package in inches
		{
			get { return m_width; }
			set { m_width = value; }
		}

		public Single Height	// Single value representing the height of the package in inches
		{
			get { return m_height; }
			set { m_height = value; }
		}

		public RTShipping()
		{
			UPSLogin = Common.AppConfig("RTShipping.UPS.Username") + "," + Common.AppConfig("RTShipping.UPS.Password") + "," + Common.AppConfig("RTShipping.UPS.License");
			UPSServer = Common.AppConfig("RTShipping.UPS.Server");
			UPSUsername = Common.AppConfig("RTShipping.UPS.Username");
			UPSPassword = Common.AppConfig("RTShipping.UPS.Password");
			UPSLicense = Common.AppConfig("RTShipping.UPS.License");

			USPSServer = Common.AppConfig("RTShipping.USPS.Server");
			USPSLogin = Common.AppConfig("RTShipping.USPS.Username") + "," + Common.AppConfig("RTShipping.USPS.Password");
			USPSUsername = Common.AppConfig("RTShipping.USPS.Username");
			USPSPassword = Common.AppConfig("RTShipping.USPS.Password");

			DHLServer = Common.AppConfig("RTShipping.DHL.Server");
			DHLAccountNumber = Common.AppConfig("RTShipping.DHL.AccountNumber");
			DHLApiSystemID = Common.AppConfig("RTShipping.DHL.APISystemID");
			DHLApiSystemPassword = Common.AppConfig("RTShipping.DHL.APISystemPassword");
			DHLShippingKey = Common.AppConfig("RTShipping.DHL.ShippingKey");

			FedexAccountNumber = Common.AppConfig("RTShipping.FEDEX.AccountNumber");
			FedexServer = Common.AppConfig("RTShipping.FEDEX.Server");
			FedexMeter = Common.AppConfig("RTShipping.FEDEX.Meter");

			OriginAddress = Common.AppConfig("RTShipping.OriginAddress"); 
			OriginAddress2 = Common.AppConfig("RTShipping.OriginAddress2"); 
			OriginCity = Common.AppConfig("RTShipping.OriginCity"); 
			OriginStateProvince = Common.AppConfig("RTShipping.OriginState"); 
			OriginZipPostalCode = Common.AppConfig("RTShipping.OriginZip"); 
			OriginCountry = Common.AppConfig("RTShipping.OriginCountry"); 

			m_destinationAddress = string.Empty;
			m_destinationAddress2 = string.Empty;
			m_destinationCity = string.Empty;
			m_destinationStateProvince = string.Empty;
			m_destinationZipPostalCode = string.Empty;
			m_destinationCountry = string.Empty;

			m_shipmentWeight = 0.0F;

			m_shipmentValue = System.Decimal.Zero;

			m_testMode = false;

			ratesValues = new ArrayList();
			ratesText = new ArrayList();
		}

		/// <summary>
		/// Main method which retrieves rates. Returns a dropdown list, radio button list, or multiline select box
		/// </summary>
		/// <param name="Shipment">The Packages object which contains the packages to be rated</param>
		/// <param name="Carriers">The carriers to get rates from: UPS, USPS, FedEx, DHL. Use a comma separated list</param>
		/// <param name="ListFormat">The type of list you would like back: DropDown, RadioButtonList, Multiline</param>
		/// <param name="FieldName">The name of the field when returned</param>
		/// <param name="CssClass">The CSS style class name of the field when returned</param>
		/// <returns>System.String</returns>
		public object GetRates(Packages Shipment, string Carriers, ResultType ListFormat, string FieldName, string CssClass, out string RTShipRequest, out string RTShipResponse, decimal ExtraFee, decimal MarkupPercent, decimal ShipmentValue)
		{
			// Get all carriers to retrieve rates for
			string[] carriersS = Carriers.Split(',');
			
			RTShipRequest = String.Empty;
			RTShipResponse = String.Empty;

			// Loop through & get rates
			foreach(string carrier in carriersS)
			{
				switch(carrier.Trim().ToUpper())
				{
					case "UPS":
						UPSGetRates(Shipment,out RTShipRequest, out RTShipResponse, ExtraFee, MarkupPercent, ShipmentValue);
						break;
					case "USPS":
						if(Shipment.DestinationCountryCode.ToLower() == "us")
						{
							USPSGetRates(Shipment,out RTShipRequest, out RTShipResponse, ExtraFee, MarkupPercent, ShipmentValue);
						}
						else
						{
							USPSIntlGetRates(Shipment,out RTShipRequest, out RTShipResponse, ExtraFee, MarkupPercent, ShipmentValue);
						}
						break;
					case "FEDEX":
						FedExGetRates(Shipment,out RTShipRequest, out RTShipResponse, ExtraFee, MarkupPercent, ShipmentValue);
						break;
					case "DHL":
						DHLGetRates(Shipment,out RTShipRequest, out RTShipResponse, ExtraFee, MarkupPercent, ShipmentValue);
						break;
				}
			}
			
			// Check list format type, and setup appropriate 
			string output = string.Empty;
			object returnObject = null;
			//
			// TBD SHOULD SORT RETURNED RATES BY LOW COST TO HIGH COST!
			//
			switch(ListFormat)
			{
				case ResultType.PlainText:
					output = string.Format("<SPAN ID=\"{0}\" CLASS=\"{1}\">",FieldName,CssClass);
					for(int i = 0; i < ratesText.Count; i++)
					{
						output += (string)ratesText[i].ToString() + "<BR>";
					}
					output += "</SPAN>";
					returnObject = (object)output;
					break;
				case ResultType.SingleDropDownList:
					output = string.Format("<SELECT SIZE=\"1\" NAME=\"{0}\" CLASS=\"{1}\">",FieldName,CssClass);
					for(int i = 0; i < ratesText.Count; i++)
					{
						output += "<OPTION VALUE=\"" + (string)ratesValues[i].ToString() + "\">" + (string)ratesText[i].ToString() + "</OPTION>\n";
					}
					output += "</SELECT>";
					returnObject = (object)output;
					break;
				case ResultType.MultiDropDownList:
					output = string.Format("<SELECT SIZE=\"5\" NAME=\"{0}\" CLASS=\"{1}\">",FieldName,CssClass);
					for(int i = 0; i < ratesText.Count; i++)
					{
						output += "<OPTION VALUE=\"" + (string)ratesValues[i].ToString() + "\">" + (string)ratesText[i].ToString() + "</OPTION>\n";
					}
					output += "</SELECT>";
					returnObject = (object)output;
					break;
				case ResultType.RadioButtonList:
					output = string.Format("<SPAN CLASS=\"{0}\">",CssClass);
					for(int i = 0; i < ratesText.Count; i++)
					{
						string RadioId = FieldName + "_" + i.ToString();
						output += "<INPUT TYPE=\"radio\" NAME=\"" + FieldName + "\" VALUE=\""+ (string)ratesValues[i].ToString() +"\" ID=\"" + RadioId.ToString() + "\"><LABEL FOR=\"" + RadioId.ToString() + "\">" + (string)ratesText[i].ToString() + "</LABEL><BR>";
					}
					output += "</SPAN>";
					returnObject = (object)output;
					break;
				case ResultType.RawDelimited:
					for(int i = 0; i < ratesValues.Count; i++)
					{
						output += (string)ratesValues[i].ToString().Trim() + ",";
					}
					output.Remove(output.Length-1, 1);
					returnObject = (object)output;
					break;
				case ResultType.DropDownListControl:
					System.Web.UI.WebControls.DropDownList returnList = new System.Web.UI.WebControls.DropDownList();
					returnList.CssClass = CssClass;
					returnList.ID = FieldName;
					for(int i = 0; i < ratesValues.Count; i++)
					{
						System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
						item.Text = ratesText[i].ToString();
						item.Value = ratesValues[i].ToString();
						returnList.Items.Add(item);
						item = null;
					}
					returnObject = (object)returnList;
					break;
			}
			return returnObject;
		}

		private void UPSGetRates(Packages Shipment, out string RTShipRequest, out string RTShipResponse, decimal ExtraFee, decimal MarkupPercent, decimal ShipmentValue)	// Private method to retrieve UPS rates
		{
			RTShipRequest = String.Empty;
			RTShipResponse = String.Empty;
			// check all required info
			if(m_upsLogin == string.Empty || m_upsUsername == string.Empty || m_upsPassword == string.Empty || m_upsLicense == string.Empty)
			{
				ratesText.Add("Error: You must provide UPS login information\n");
				ratesValues.Add("0");
				return;
			}

			// Check for test mode
			if(m_testMode)
			{
				m_upsServer = Common.AppConfig("RTShipping.UPS.TestServer"); //ConfigurationSettings.AppSettings["UPSTestServer"].ToString();
			}

			// Check server setting
			if(m_upsServer == string.Empty)
			{
				//m_upsServer = ConfigurationSettings.AppSettings["UPSServer"].ToString();
				ratesText.Add("Error: You must provide the UPS server\n");
				ratesValues.Add("0");
				return;
			}

			// Check for m_shipmentWeight
			if(m_shipmentWeight == 0.0)
			{
				ratesText.Add("Error: Shipment Shipment Weight must be greater than 0 " + Localization.WeightUnits() + ".\n");
				ratesValues.Add("0");
				return;
			}
      
			Single maxWeight = Common.AppConfigUSSingle("RTShipping.UPS.MaxWeight");
			if (maxWeight ==0) 
			{
				maxWeight = 150;
			}

			if (m_shipmentWeight > maxWeight)
			{
				ratesText.Add("UPS " + Common.AppConfig("RTShipping.CallForShippingPrompt"));
				ratesValues.Add("UPS " + Common.AppConfig("RTShipping.CallForShippingPrompt") + "|0");
				return;
			}
			// Set the access request XML
			string accessRequest = string.Format("<?xml version=\"1.0\"?>"
				+ "<AccessRequest xml:lang=\"en-us\">"
				+ "<AccessLicenseNumber>{0}</AccessLicenseNumber>"
				+ "<UserId>{1}</UserId>"
				+ "<Password>{2}</Password>"
				+ "</AccessRequest>", this.m_upsLicense, this.m_upsUsername, this.m_upsPassword);
			
			// Set the rate request XML
			string shipmentRequest = "<?xml version=\"1.0\"?>"
				+ "<RatingServiceSelectionRequest xml:lang=\"en-US\">"
				+ "<Request>"
				+ "<RequestAction>Rate</RequestAction>"
				+ "<RequestOption>Shop</RequestOption>"
				+ "<TransactionReference>"
				+ "<CustomerContext>Rating and Service</CustomerContext>"
				+ "<XpciVersion>1.0001</XpciVersion>"
				+ "</TransactionReference>"
				+ "</Request>"
				+ "<PickupType>"
				+ "<Code>" + MapPickupType(Shipment.PickupType) + "</Code>"
				+ "</PickupType>"
				+ "<Shipment>"
				+ "<Shipper>"
				+ "<Address>"
				+ "<City>" + this.m_originCity + "</City>"
				+ "<StateProvinceCode>" + this.m_originStateProvince + "</StateProvinceCode>"
				+ "<PostalCode>" + this.m_originZipPostalCode + "</PostalCode>"
				+ "<CountryCode>" + this.m_originCountry + "</CountryCode>"
				+ "</Address>"
				+ "</Shipper>"
				+ "<ShipTo>"
				+ "<Address>"
				+ "<City>" + Shipment.DestinationCity + "</City>"
				+ "<StateProvinceCode>" + Shipment.DestinationStateProvince + "</StateProvinceCode>"
				+ "<PostalCode>" + Shipment.DestinationZipPostalCode + "</PostalCode>"
				+ "<CountryCode>" + Shipment.DestinationCountryCode + "</CountryCode>"
				+ "</Address>"
				+ "</ShipTo>"
				+ "<ShipmentWeight>"
				+ "<UnitOfMeasurement>"
				+ "<Code>" + Common.AppConfig("RTShipping.WeightUnits") + "</Code>"
				+ "</UnitOfMeasurement>"
				+ "<Weight>" + Shipment.Weight.ToString() + "</Weight>"
				+ "</ShipmentWeight>";

				
			// loop through the packages
			foreach(Package p in Shipment)
			{
				shipmentRequest += "<Package>"
					+ "<PackagingType>"
					+ "<Code>02</Code>"
					+ "</PackagingType>"
					+ "<Dimensions>"
					+ "<UnitOfMeasurement>"
					+ "<Code>IN</Code>"
					+ "</UnitOfMeasurement>"
					+ "<Length>" + p.Length.ToString() + "</Length>"
					+ "<Width>" + p.Width.ToString() + "</Width>"
					+ "<Height>" + p.Height.ToString() + "</Height>"
					+ "</Dimensions>"
					+ "<Description>" + p.PackageId.ToString() + "</Description>"
					+ "<PackageWeight>"
					+ "<UnitOfMeasure>"
					+ "<Code>" + Common.AppConfig("RTShipping.WeightUnits") + "</Code>"
					+ "</UnitOfMeasure>"
					+ "<Weight>" + p.Weight.ToString() + "</Weight>"
					+ "</PackageWeight>"
					+ "<OversizePackage />";

				if(p.Insured && (p.InsuredValue != 0))
				{
					shipmentRequest += "<AdditionalHandling />";
					shipmentRequest += "<PackageServiceOptions>";
					shipmentRequest += "<InsuredValue>"
						+ "<CurrencyCode>USD</CurrencyCode>"
						+ "<MonetaryValue>" + p.InsuredValue.ToString() + "</MonetaryValue>"
						+ "</InsuredValue>";
					shipmentRequest += "</PackageServiceOptions>";
				}

				shipmentRequest += "</Package>";
			}

			shipmentRequest += "<ShipmentServiceOptions/>"
				+ "</Shipment>"
				+ "</RatingServiceSelectionRequest>";
					
			// Concat the requests
			string fullUPSRequest = accessRequest + shipmentRequest;

			RTShipRequest = fullUPSRequest;

			// Send request & capture response
			string result = POSTandReceiveData(fullUPSRequest, m_upsServer);

			RTShipResponse = result;
			
			// Load XML into a XmlDocument object
			XmlDocument UPSResponse = new XmlDocument();
			try
			{
				UPSResponse.LoadXml(result);
			}
			catch 
			{
				ratesText.Add("UPS Gateway Did Not Respond");
				ratesValues.Add("UPS Gateway Did Not Respond|0");
				return;
			}

			// Get Response code: 0 = Fail, 1 = Success
			XmlNodeList UPSResponseCode = UPSResponse.GetElementsByTagName("ResponseStatusCode");

			if(UPSResponseCode[0].InnerText == "1") // Success
			{
				// Loop through elements & get rates
				XmlNodeList ratedShipments = UPSResponse.GetElementsByTagName("RatedShipment");
				string tempService = string.Empty;
				Single tempRate = 0.0F;
				for(int i = 0;i<ratedShipments.Count;i++)
				{
					XmlNode shipmentX = ratedShipments.Item(i);
					tempService = UPSServiceCodeDescription(shipmentX["Service"]["Code"].InnerText);
					tempRate = Single.Parse(shipmentX["TotalCharges"]["MonetaryValue"].InnerText);
					
					if(MarkupPercent != 0.0M)
					{
						tempRate = tempRate * (Single)(1.00M + (MarkupPercent/100.0M));
					}
					tempRate += (Single)ExtraFee;

					ratesText.Add(tempService + " " + tempRate.ToString("C"));
					ratesValues.Add(tempService + "|" + tempRate.ToString());
				}
			}
			else // Error
			{
				XmlNodeList UPSError = UPSResponse.GetElementsByTagName("ErrorDescription");
				ratesText.Add("UPS Error: " + UPSError[0].InnerText);
				ratesValues.Add("UPS Error: " + UPSError[0].InnerText);
				UPSError = null;
				return;
			}

			// Some clean up
			UPSResponseCode = null;
			UPSResponse = null;
		}

		private void USPSIntlGetRates(Packages Shipment, out string RTShipRequest, out string RTShipResponse, decimal ExtraFee, decimal MarkupPercent, decimal ShipmentValue) // Retrieves International rates for USPS
 		{
 			RTShipRequest = String.Empty;
			RTShipResponse = String.Empty;
			Hashtable htRates = new Hashtable();
 
			// check all required info
			if(Shipment.DestinationCountryCode.ToLower() == "us") return;
			if(m_uspsLogin == string.Empty || m_uspsUsername == string.Empty || m_uspsPassword == string.Empty)
			{
				ratesText.Add("Error: You must provide USPS login information\n");
				ratesValues.Add("0");
				return;
			}
 
			// Check server setting
 			if(m_uspsServer == string.Empty)
 
			{
 				//m_uspsServer = ConfigurationSettings.AppSettings["USPSServer"].ToString();
 				ratesText.Add("Error: You must provide the USPS server\n");
 				ratesValues.Add("0");
 				return;
 			}
 
			// Check for test mode
			if(m_testMode)
			{
				m_uspsServer = Common.AppConfig("RTShipping.USPS.TestServer");
			}
 
			// Check for m_shipmentWeight
			if(ShipmentWeight == 0.0)
			{
				ratesText.Add("Error: Shipment ShipmentWeight must be greater than 0 " + Localization.WeightUnits() + ".\n");
				ratesValues.Add("0");
				return;
			}
 
			Single maxWeight = Common.AppConfigUSSingle("RTShipping.USPS.MaxWeight");
			if (maxWeight == 0)
			{
				maxWeight = 150;
			}

			if (ShipmentWeight > maxWeight)
			{
				ratesText.Add("USPS " + Common.AppConfig("RTShipping.CallForShippingPrompt"));
				ratesValues.Add("USPS " + Common.AppConfig("RTShipping.CallForShippingPrompt") + "|0");
				return;
			}
      
			// Create the XML request (International)
			string USPSRequest = "API=IntlRate&XML=";
			string uspsReqLoop = "<IntlRateRequest USERID=\"{0}\" PASSWORD=\"{1}\">";
			foreach(Package p in Shipment)
			{
				USPSWeight w = USPSGetWeight(p.Weight);
				uspsReqLoop += "<Package ID=\"" + p.PackageId.ToString() + "\">"
					+ "<Pounds>" + w.pounds.ToString() + "</Pounds>"
					+ "<Ounces>" + w.ounces.ToString() + "</Ounces>"
					+ "<MailType>Package</MailType>";
				if (Shipment.DestinationCountryCode.ToLower() == "gb")
				{
					uspsReqLoop += "<Country>United Kingdom (Great Britain)</Country>";
				}
				else
				{
					uspsReqLoop += "<Country>" + Common.GetCountryName(Shipment.DestinationCountryCode  ) + "</Country>";
				}
				uspsReqLoop += "</Package>";
			}
			USPSRequest += uspsReqLoop + "</IntlRateRequest>";
 
			// Replace login info
			USPSRequest = string.Format(USPSRequest, USPSUsername, USPSPassword);
			RTShipRequest = USPSRequest;
 
			// Send request & capture response
			string result = GETandReceiveData(USPSRequest, USPSServer);
			RTShipResponse = result;
 
			// Load XML into a XmlDocument object
			XmlDocument USPSResponse = new XmlDocument();
			try
			{
				USPSResponse.LoadXml(result);
			}
			catch 
			{
				ratesText.Add("USPS Gateway Did Not Respond");
				ratesValues.Add("USPS Gateway Did Not Respond|0");
				return;
			}
 
			// Check for error
			XmlNodeList USPSErrors = USPSResponse.GetElementsByTagName("Error");
			if(USPSErrors.Count > 0) // Error has occured
			{
				XmlNodeList USPSError = USPSResponse.GetElementsByTagName("Error");
				XmlNode USPSErrorMessage = USPSError.Item(0);
				ratesText.Add("USPS Error: " + USPSErrorMessage["Description"].InnerText);
				ratesValues.Add("USPS Error: " + USPSErrorMessage["Description"].InnerText);
				USPSError = null;
				return;
			}
			else
			{
				XmlNodeList nodesPackages = USPSResponse.GetElementsByTagName("Package");
				foreach(XmlNode nodePackage in nodesPackages)
				{
					XmlNodeList nodesServices = nodePackage.SelectNodes("Service");
					foreach(XmlNode nodeService in nodesServices)
					{
						string rateName = nodeService.SelectSingleNode("SvcDescription").InnerText;
						if(rateName.IndexOf("Envelope")==-1 && rateName.IndexOf(" Document")==-1 && rateName.IndexOf("Letter")==-1)
						{
							decimal totalCharges = Decimal.Parse(nodeService.SelectSingleNode("Postage").InnerText);

							if(MarkupPercent != 0.0M)
							{
								totalCharges = totalCharges * (1.00M + (MarkupPercent/100.0M));
							}
							totalCharges += ExtraFee;

							if(htRates.ContainsKey(rateName))
							{
								// Get the sum of the rate(s)
								decimal myTempCharge = decimal.Parse(htRates[rateName].ToString());
								totalCharges += myTempCharge;
								// Remove the old value & add the new
								htRates.Remove(rateName);
							}
							htRates.Add(rateName,totalCharges);
						}
					}
				}
 
				// Clean up
				USPSResponse = null;
			}
 
			// Add rates from hastable into array(s)
			IDictionaryEnumerator myEnumerator = htRates.GetEnumerator();
			while(myEnumerator.MoveNext())
			{
				ratesText.Add(myEnumerator.Key.ToString() + " $" + myEnumerator.Value.ToString());
				ratesValues.Add(myEnumerator.Key.ToString() + "|" + myEnumerator.Value.ToString());
			}
		}
 
		private string USPSGetSize(Single length, Single width, Single height)
		{
 
			Single size = System.Math.Min(System.Math.Min(Length + 2* (Width + Height), Width + 2 * (Length + Height)), Height + 2 * (Length + Width));
 
			if(size > 108F) return "Oversize";
			else if(size > 84F) return "Large";
 
			return "Regular";
		}


		private void FedExGetRates(Packages Shipment, out string RTShipRequest, out string RTShipResponse, decimal ExtraFee, decimal MarkupPercent, decimal ShipmentValue)	// Retrieves FedEx rates
		{
			RTShipRequest = String.Empty;
			RTShipResponse = String.Empty;
			
			Encoding utf8 = new UTF8Encoding(false);
			string[] FedExCarrierCodes = {""}; //{"FDXE","FDXG"};
			Hashtable htRates = new Hashtable();

			Single maxWeight = Common.AppConfigUSSingle("RTShipping.Fedex.MaxWeight");
			if (maxWeight == 0)
			{
				maxWeight = 150;
			}

			if (ShipmentWeight > maxWeight)
			{
				ratesText.Add("FedEx " + Common.AppConfig("RTShipping.CallForShippingPrompt"));
				ratesValues.Add("FedEx " + Common.AppConfig("RTShipping.CallForShippingPrompt") + "|0");
				return;
			}
      
			foreach(string FedExCarrierCode in FedExCarrierCodes)
			{
				foreach(Package package in Shipment)
				{
					StringBuilder FedExRequest = new StringBuilder();
					FedExRequest.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
					FedExRequest.Append("<FDXRateAvailableServicesRequest xmlns:api=\"http://www.fedex.com/fsmapi\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"FDXRateAvailableServicesRequest.xsd\">");
					FedExRequest.Append("<RequestHeader>");
					FedExRequest.Append("<CustomerTransactionIdentifier>RatesRequest</CustomerTransactionIdentifier>");
					FedExRequest.Append("<AccountNumber>" + this.FedexAccountNumber + "</AccountNumber>");
					FedExRequest.Append("<MeterNumber>" + this.FedexMeter + "</MeterNumber>");
					FedExRequest.Append("<CarrierCode>" + FedExCarrierCode.ToString() + "</CarrierCode>");
					FedExRequest.Append("</RequestHeader>");
					System.DateTime TomorrowsDate = System.DateTime.Now.AddDays(1);
					FedExRequest.Append("<ShipDate>" + TomorrowsDate.Year.ToString() + "-" + TomorrowsDate.Month.ToString().PadLeft(2,'0') + "-" + TomorrowsDate.Day.ToString().PadLeft(2,'0') + "</ShipDate>");
					FedExRequest.Append("<DropoffType>REGULARPICKUP</DropoffType>");
					FedExRequest.Append("<Packaging>YOURPACKAGING</Packaging>");
					FedExRequest.Append("<WeightUnits>" + Common.AppConfig("RTShipping.WeightUnits") + "</WeightUnits>");
					FedExRequest.Append("<ListRate>false</ListRate>");
					FedExRequest.Append("<Weight>" + package.Weight.ToString() + "</Weight>");
					FedExRequest.Append("<OriginAddress>");
					FedExRequest.Append("<StateOrProvinceCode>" + this.OriginStateProvince + "</StateOrProvinceCode>");
					FedExRequest.Append("<PostalCode>" + this.OriginZipPostalCode + "</PostalCode>");
					FedExRequest.Append("<CountryCode>" + this.OriginCountry + "</CountryCode>");
					FedExRequest.Append("</OriginAddress>");
					FedExRequest.Append("<DestinationAddress>");
					FedExRequest.Append("<StateOrProvinceCode>" + Shipment.DestinationStateProvince + "</StateOrProvinceCode>");
					FedExRequest.Append("<PostalCode>" + Shipment.DestinationZipPostalCode + "</PostalCode>");
					String DCountry = Shipment.DestinationCountryCode;
					if(DCountry.Length == 0)
					{
						DCountry = this.OriginCountry;
					}
					FedExRequest.Append("<CountryCode>" + DCountry + "</CountryCode>");
					FedExRequest.Append("</DestinationAddress>");
					FedExRequest.Append("<Payment>");
					FedExRequest.Append("<PayorType>SENDER</PayorType>");
					FedExRequest.Append("</Payment>");
					FedExRequest.Append("<DeclaredValue>");
					FedExRequest.Append("<Value>" + Localization.CurrencyStringForGateway(ShipmentValue) + "</Value>");
					FedExRequest.Append("<CurrencyCode>USD</CurrencyCode>"); // TBD for ML
					FedExRequest.Append("</DeclaredValue>");
					FedExRequest.Append("<PackageCount>1</PackageCount>");
					FedExRequest.Append("</FDXRateAvailableServicesRequest>");
					
					// Send Fedex Request
					RTShipRequest = FedExRequest.ToString();
					string result = POSTandReceiveData(RTShipRequest,this.FedexServer);
					RTShipResponse = result;
					FedExRequest = null;
				
					// Load XML into a XmlDocument object
					XmlDocument FedExResponse = new XmlDocument();
					try
					{
						FedExResponse.LoadXml(result);
					}
					catch 
					{
						ratesText.Add("FedEx Gateway Did Not Respond");
						ratesValues.Add("FedEx Gateway Did Not Respond|0");
						return;
					}

					// Parse the response

					// Check for errors
					XmlNodeList FedExErrors = FedExResponse.SelectNodes("/FDXRateAvailableServicesReply/Error");
					
					if(FedExErrors.Count > 0)
					{
						XmlNode errorCode = FedExResponse.SelectSingleNode("/FDXRateAvailableServicesReply/Error/Code");
						XmlNode errorMessage = FedExResponse.SelectSingleNode("/FDXRateAvailableServicesReply/Error/Message");

						switch(errorCode.InnerText)
						{
							case "58660":
							{
								ratesText.Add(Common.AppConfig("RTShipping.CallForShippingPrompt"));
								ratesValues.Add(Common.AppConfig("RTShipping.CallForShippingPrompt")+"|");
								break;
							}
							default:
							{
								ratesText.Add("FedEx Error: " + errorMessage.InnerText);
								ratesValues.Add("FedEx Error: " + errorMessage.InnerText);
								break;
							}
						}
						errorCode = null;
						errorMessage = null;

						return;
					}

					FedExErrors = null;

					// Get rates
					XmlNodeList nodesEntries = FedExResponse.SelectNodes("/FDXRateAvailableServicesReply/Entry");

					// Loop through & get rates for individual packages
					foreach(XmlNode nodeEntry in nodesEntries)
					{
					
						string rateName = "FedEx " + FedExGetCodeDescription(nodeEntry.SelectSingleNode("Service").InnerText);
						decimal totalCharges = Decimal.Parse(nodeEntry.SelectSingleNode("EstimatedCharges/DiscountedCharges/NetCharge").InnerText);

						if(MarkupPercent != 0.0M)
						{
							totalCharges = totalCharges * (1.00M + (MarkupPercent/100.0M));
						}
						totalCharges += ExtraFee;

						if(htRates.ContainsKey(rateName))
						{
							// Get the sum of the rate(s)
							decimal myTempCharge = decimal.Parse(htRates[rateName].ToString());
							totalCharges += myTempCharge;

							// Remove the old value & add the new
							htRates.Remove(rateName);
						}
					
						// Temporarily add rate to hash table
						htRates.Add(rateName,totalCharges);

						// Don't add to array here, add from the hashtable
						//ratesText.Add(rateName + " " + totalCharges.ToString("C"));
						//ratesValues.Add(totalCharges);
					
					}
					// Clean up
					FedExResponse = null;
				}
			}
      
			// Add rates from hastable into array(s)
			IDictionaryEnumerator myEnumerator = htRates.GetEnumerator();
			while(myEnumerator.MoveNext())
			{
				ratesText.Add(myEnumerator.Key.ToString() + " $" + myEnumerator.Value.ToString());
				ratesValues.Add(myEnumerator.Key.ToString() + "|" + myEnumerator.Value.ToString());
			}
		}

		/// Available FedEx Rates
		public string FedExGetCodeDescription(string code)
		{
			string result = string.Empty;
			switch (code)
			{
				case "PRIORITYOVERNIGHT":
					result = "Priority";
					break;
				case "FEDEX2DAY":
					result = "2nd Day";
					break;
				case "STANDARDOVERNIGHT":
					result = "Standard Overnight";
					break;
				case "FIRSTOVERNIGHT":
					result = "First Overnight";
					break;
				case "FEDEXEXPRESSSAVER":
					result = "Express Saver";
					break;
				case "FEDEX1DAYFREIGHT":
					result = "Overnight Freight";
					break;
				case "FEDEX2DAYFREIGHT":
					result = "2nd Day Freight";
					break;
				case "FEDEX3DAYFREIGHT":
					result = "Express Saver Freight";
					break;
				case "GROUNDHOMEDELIVERY":
					result = "Home Delivery";
					break;
				case "FEDEXGROUND":
					result = "Ground Service";
					break;
			}
			return result;
		}


		/// <summary>
		/// Retrieves DHL rates
		/// </summary>
		/// <param name="Shipment"></param>
		private void DHLGetRates(Packages Shipment, out string RTShipRequest, out string RTShipResponse, decimal ExtraFee, decimal MarkupPercent, decimal ShipmentValue)
		{
			RTShipRequest = String.Empty;
			RTShipResponse = String.Empty;

			if(this.DHLApiSystemID == string.Empty || this.DHLApiSystemPassword == string.Empty || this.DHLAccountNumber == string.Empty || this.DHLShippingKey == string.Empty)
			{
				ratesText.Add("Error: You must provide DHL login information\n");
				ratesValues.Add("Error: You must provide DHL login information");
				return;
			}

			if(this.TestMode)
			{
				DHLServer = Common.AppConfig("RTShipping.DHL.TestServer");
			}

			Single maxWeight = Common.AppConfigUSSingle("RTShipping.DHL.MaxWeight");
			if (maxWeight ==0) maxWeight = 150;

			if (ShipmentWeight > maxWeight)
			{
				ratesText.Add("DHL " + Common.AppConfig("RTShipping.CallForShippingPrompt"));
				ratesValues.Add("DHL " + Common.AppConfig("RTShipping.CallForShippingPrompt") + "|0");
				return;
			}
      
			NameValueCollection RatesList = new NameValueCollection();
			foreach(string service in DHLServices)
			{
				RatesList.Add(service,"0");
			}

			foreach(Package p in Shipment)
			{
				// Create the authentication envelope
				XmlDocument dhlDoc = new XmlDocument();
				XmlNode nRoot = dhlDoc.CreateElement("eCommerce");
				dhlDoc.AppendChild(nRoot);

				XmlAttribute att = dhlDoc.CreateAttribute("version");
				att.Value = "1.1"; 
				nRoot.Attributes.Append(att);
				att = dhlDoc.CreateAttribute("action");
				att.Value = "Request";
				nRoot.Attributes.Append(att);

				XmlNode nRequestor = dhlDoc.CreateElement("Requestor");
				nRoot.AppendChild(nRequestor);

				XmlNode iNode = dhlDoc.CreateElement("ID");
				iNode.InnerText = this.DHLApiSystemID;
				nRequestor.AppendChild(iNode);
				iNode = dhlDoc.CreateElement("Password");
				iNode.InnerText = this.DHLApiSystemPassword;
				nRequestor.AppendChild(iNode);
      
				// Create the Rate request
				foreach (string service in DHLServices)
				{
					XmlNode nShipment = dhlDoc.CreateElement("Shipment");
					nRoot.AppendChild(nShipment);
      
					att = dhlDoc.CreateAttribute("action");
					att.Value = "RateEstimate";
					nShipment.Attributes.Append(att);

					att = dhlDoc.CreateAttribute("version");
					att.Value = "1.0";
					nShipment.Attributes.Append(att);

      
					// Shipping credentials

					XmlNode nShippingCredentials = dhlDoc.CreateElement("ShippingCredentials");
					nShipment.AppendChild(nShippingCredentials);

					iNode = dhlDoc.CreateElement("ShippingKey");
					iNode.InnerText = this.DHLShippingKey;
					nShippingCredentials.AppendChild(iNode);

					iNode = dhlDoc.CreateElement("AccountNbr");
					iNode.InnerText = this.DHLAccountNumber;
					nShippingCredentials.AppendChild(iNode);

					XmlNode nShipmentDetail = dhlDoc.CreateElement("ShipmentDetail");
					nShipment.AppendChild(nShipmentDetail);
      
					iNode = dhlDoc.CreateElement("ShipDate");
					iNode.InnerText = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
					nShipmentDetail.AppendChild(iNode);

					XmlNode nService = dhlDoc.CreateElement("Service");
					nShipmentDetail.AppendChild(nService);
					iNode = dhlDoc.CreateElement("Code");
					iNode.InnerText = service.Substring(0,1);  //First character of the service description
					nService.AppendChild(iNode);

					XmlNode nShipmentType = dhlDoc.CreateElement("ShipmentType");
					nShipmentDetail.AppendChild(nShipmentType);
					iNode = dhlDoc.CreateElement("Code");
					iNode.InnerText = "P";   //Package
					nShipmentType.AppendChild(iNode);

					iNode = dhlDoc.CreateElement("Weight");
					iNode.InnerText = p.Weight.ToString("###");   //Package weight integer only
					nShipmentDetail.AppendChild(iNode);

					XmlNode nBilling = dhlDoc.CreateElement("Billing");
					nShipment.AppendChild(nBilling);
					XmlNode nParty = dhlDoc.CreateElement("Party");
					nBilling.AppendChild(nParty);
					iNode = dhlDoc.CreateElement("Code");
					iNode.InnerText = "S"; //Sender
					nParty.AppendChild(iNode);

					XmlNode nReceiver = dhlDoc.CreateElement("Receiver");
					nShipment.AppendChild(nReceiver);
					XmlNode nAddress = dhlDoc.CreateElement("Address");
					nReceiver.AppendChild(nAddress);

					iNode = dhlDoc.CreateElement("City");
					iNode.InnerText = Shipment.DestinationCity;
					nAddress.AppendChild(iNode);

					iNode = dhlDoc.CreateElement("State");
					iNode.InnerText = Shipment.DestinationStateProvince;
					nAddress.AppendChild(iNode);

					iNode = dhlDoc.CreateElement("Country");
					iNode.InnerText = Shipment.DestinationCountryCode;
					nAddress.AppendChild(iNode);

					iNode = dhlDoc.CreateElement("PostalCode");
					iNode.InnerText = Shipment.DestinationZipPostalCode;
					nAddress.AppendChild(iNode);

					//Trace Name
					iNode = dhlDoc.CreateElement("TransactionTrace");
					iNode.InnerText = service;
					nShipment.AppendChild(iNode);

					if (service=="Express 10:30")
					{
						XmlNode nSpecialServices = dhlDoc.CreateElement("SpecialServices");
						nShipmentDetail.AppendChild(nSpecialServices);
						iNode = dhlDoc.CreateElement("Code");
						iNode.InnerText = "1030";
						nSpecialServices.AppendChild(iNode);
					}

					//Optional Nodes
					//Dimension of package if available
					if ((p.Length + p.Width +p.Height) !=0) //There are dimensions
					{
						XmlNode nDimensions = dhlDoc.CreateElement("Dimensions");
						nShipmentDetail.AppendChild(nDimensions);

						iNode = dhlDoc.CreateElement("Length");
						iNode.InnerText = p.Length.ToString("###");
						nDimensions.AppendChild(iNode);

						iNode = dhlDoc.CreateElement("Width");
						iNode.InnerText = p.Width.ToString("###");
						nDimensions.AppendChild(iNode);

						iNode = dhlDoc.CreateElement("Height");
						iNode.InnerText = p.Length.ToString("###");
						nDimensions.AppendChild(iNode);
					}

					// Insurance
					if (p.Insured && p.InsuredValue != 0)
					{
						XmlNode nAdditonalProtection = dhlDoc.CreateElement("AdditionalProtection");
						nShipmentDetail.AppendChild(nAdditonalProtection);

						iNode = dhlDoc.CreateElement("Code");
						iNode.InnerText = "AP";
						nAdditonalProtection.AppendChild(iNode);

						iNode = dhlDoc.CreateElement("Value");
						iNode.InnerText = p.InsuredValue.ToString("#####");
						nAdditonalProtection.AppendChild(iNode);
					}
				}

				RTShipRequest = dhlDoc.OuterXml;
				string result = POSTandReceiveData(RTShipRequest,this.DHLServer);
				RTShipResponse = result;
				dhlDoc = null;

				// Load XML into a XmlDocument object
				XmlDocument DHLResponse = new XmlDocument();
				try
				{
					DHLResponse.LoadXml(result);
				}
				catch 
				{
					ratesText.Add("DHL Gateway Did Not Respond");
					ratesValues.Add("DHL Gateway Did Not Respond");
					return;
				}

				// Check for Errors
				XmlNodeList DHLErrors = DHLResponse.SelectNodes("//Fault");
				if (DHLErrors.Count > 0)
				{
					XmlNode DHLError = DHLErrors[0].SelectSingleNode(".//Desc"); 
					if (DHLError == null)
					{
						DHLError = DHLErrors[0].SelectSingleNode(".//Description"); 
					}
					ratesText.Add("DHL Error: " + DHLError.InnerText);
					ratesValues.Add("DHL Error: " + DHLError.InnerText);
					DHLError = null;
					DHLErrors = null;
					return;
				}
				else
				{
					XmlNodeList DHLRates = DHLResponse.SelectNodes("//Shipment");
					foreach (XmlNode sNode in DHLRates)
					{
						XmlNode tNode = sNode.SelectSingleNode(".//TransactionTrace");
						XmlNode rNode = sNode.SelectSingleNode(".//TotalChargeEstimate");
						decimal total = decimal.Parse(RatesList[tNode.InnerText]) + decimal.Parse(rNode.InnerText);

						if(MarkupPercent != 0.0M)
						{
							total = total * (1.00M + (MarkupPercent/100.0M));
						}

						total += ExtraFee;

						RatesList.Set(tNode.InnerText,total.ToString());
					}
				}
      
				DHLResponse=null;
			}
			for (int i=0; i < RatesList.Count;i++)
			{
				ratesText.Add("DHL " + RatesList.GetKey(i));
				ratesValues.Add("DHL " + RatesList.GetKey(i) + "|" + RatesList[i]);
			}
		}
		
		private void USPSGetRates(Packages Shipment, out string RTShipRequest, out string RTShipResponse, decimal ExtraFee, decimal MarkupPercent, decimal ShipmentValue)	// Retrieves rates for USPS
		{
			RTShipRequest = String.Empty;
			RTShipResponse = String.Empty;
			// check all required info
			if(USPSLogin == string.Empty || USPSUsername == string.Empty || USPSPassword == string.Empty)
			{
				ratesText.Add("Error: You must provide USPS login information\n");
				ratesValues.Add("0");
				return;
			}

			// Check server setting
			if(USPSServer == string.Empty)
			{
				//uspsServer = ConfigurationSettings.AppSettings["USPSServer"].ToString();
				ratesText.Add("Error: You must provide the USPS server\n");
				ratesValues.Add("0");
				return;
			}

			// Check for test mode
			if(TestMode)
			{
				USPSServer = Common.AppConfig("RTShipping.USPS.TestServer"); //ConfigurationSettings.AppSettings["USPSTestServer"].ToString();
			}

			// Check for shipmentWeight
			if(ShipmentWeight == 0.0)
			{
				ratesText.Add("Error: Shipment Shipment Weight must be greater than 0 " + Localization.WeightUnits() + ".\n");
				ratesValues.Add("0");
				return;
			}

			Single maxWeight = Common.AppConfigUSSingle("RTShipping.USPS.MaxWeight");
			if (maxWeight ==0) maxWeight = 150;

			if (ShipmentWeight > maxWeight)
			{
				ratesText.Add("USPS " + Common.AppConfig("RTShipping.CallForShippingPrompt"));
				ratesValues.Add("USPS " + Common.AppConfig("RTShipping.CallForShippingPrompt") + "|0");
				return;
			}
      
			// Create the XML request (Domestinc)
			// 0 = Usename
			// 1 = Password
			// 2 = Service name
			// 3 = origin zip
			// 4 = dest zip
			// 5 = pounds
			// 6 = ounces (always 0)
			// 7 = Machinable? Always false
			string USPSRequest = "API=Rate&XML=";

			ArrayList USPSServices = new ArrayList();
			USPSServices.Add("Express");
			//USPSServices.Add("First Class");
			USPSServices.Add("Priority");
			USPSServices.Add("Parcel");
			//USPSServices.Add("BPM");
			USPSServices.Add("Library");
			USPSServices.Add("Media");

			//USPSWeight w = USPSGetWeight(ShipmentWeight);
				
			string uspsReqLoop = "<RateRequest USERID=\"{0}\" PASSWORD=\"{1}\">";
			foreach(Package p in Shipment)
			{
				USPSWeight w = USPSGetWeight(p.Weight);

				for(int srvcs = 0;srvcs<USPSServices.Count;srvcs++)
				{
					uspsReqLoop += "<Package ID=\"" + p.PackageId.ToString() + "-" + srvcs.ToString() + "\">"
						+ "<Service>" + USPSServices[srvcs].ToString() + "</Service>"
						+ "<ZipOrigination>" + OriginZipPostalCode + "</ZipOrigination>"
						+ "<ZipDestination>" + Shipment.DestinationZipPostalCode + "</ZipDestination>"
						+ "<Pounds>" + w.pounds.ToString() + "</Pounds>"
						+ "<Ounces>" + w.ounces.ToString() + "</Ounces>"
						+ "<Container>None</Container>"
						+ "<Size>" + USPSGetSize(p.Length, p.Width, p.Height) + "</Size>"
						+ "<Machinable>False</Machinable>"
						+ "</Package>";
				}
			}
			USPSRequest += uspsReqLoop + "</RateRequest>";

			// Replace login info
			USPSRequest = string.Format(USPSRequest, USPSUsername, USPSPassword);
			RTShipRequest = USPSRequest;

			// Send request & capture response
			string result = GETandReceiveData(USPSRequest, USPSServer);
			RTShipResponse = result;

			// Load XML into a XmlDocument object
			XmlDocument USPSResponse = new XmlDocument();
			try
			{
				USPSResponse.LoadXml(result);
			}
			catch 
			{
				ratesText.Add("USPS Gateway Did Not Respond");
				ratesValues.Add("USPS Gateway Did Not Respond|0");
				return;
			}

			// Check for error
			XmlNodeList USPSErrors = USPSResponse.GetElementsByTagName("Error");
			if(USPSErrors.Count > 0) // Error has occured
			{
				XmlNodeList USPSError = USPSResponse.GetElementsByTagName("Error");
				XmlNode USPSErrorMessage = USPSError.Item(0);
				ratesText.Add("USPS Error: " + USPSErrorMessage["Description"].InnerText);
				ratesValues.Add("USPS Error: " + USPSErrorMessage["Description"].InnerText);
				USPSError = null;
				return;
			}
			else
			{
				string tempService = string.Empty;
				string ExpressName = string.Empty, PriorityName = string.Empty, ParcelName = string.Empty, FirstClassName = string.Empty, BPMName = string.Empty, LibraryName = string.Empty, MediaName = string.Empty;
				Single tempRate = 0.0F;
				Single FirstClassRate = 0.0F, BPMRate = 0.0F, LibraryRate = 0.0F, MediaRate = 0.0F;
				Single ExpressRate = 0.0F;
				Single PriorityRate = 0.0F;
				Single ParcelRate = 0.0F;

				XmlNodeList USPSPackage = USPSResponse.GetElementsByTagName("Package");
				
				for(int i = 0;i<USPSPackage.Count;i++)
				{
					XmlNode USPSPostage = USPSPackage.Item(i);
					tempService = USPSPostage["Service"].InnerText;

					tempRate = Single.Parse(USPSPostage["Postage"].InnerText);

					if(MarkupPercent != 0.0M)
					{
						tempRate = tempRate * (Single)(1.00M + (MarkupPercent/100.0M));
					}
					tempRate += (Single)ExtraFee;

					switch(tempService)
					{
						case "Express":
							ExpressName = tempService;
							ExpressRate += tempRate;
							break;
						case "Priority":
							PriorityName = tempService;
							PriorityRate += tempRate;
							break;
						case "Parcel":
							ParcelName = tempService;
							ParcelRate += tempRate;
							break;
						case "First Class":
							FirstClassName = tempService;
							FirstClassRate += tempRate;
							break;
						case "BPM":
							BPMName = tempService;
							BPMRate += tempRate;
							break;
						case "Library":
							LibraryName = tempService;
							LibraryRate += tempRate;
							break;
						case "Media":
							MediaName = tempService;
							MediaRate += tempRate;
							break;
					}
					USPSPostage = null;
				}

				if(ExpressRate != 0.0)
				{
					ratesText.Add("U.S. Postal " + ExpressName + " " + ExpressRate.ToString("C"));
					ratesValues.Add("U.S. Postal " + ExpressName  +"|" + ExpressRate.ToString());
				}

				if(PriorityRate != 0.0)
				{
					ratesText.Add("U.S. Postal " + PriorityName + " " + PriorityRate.ToString("C"));
					ratesValues.Add("U.S. Postal " + PriorityName + "|" + PriorityRate.ToString());					
				}

				if(ParcelRate != 0.0)
				{
					ratesText.Add("U.S. Postal " + ParcelName + " " + ParcelRate.ToString("C"));
					ratesValues.Add("U.S. Postal " + ParcelName + "|" + ParcelRate.ToString());
				}

				if(FirstClassRate != 0.0)
				{
					ratesText.Add("U.S. Postal " + FirstClassName + " " + FirstClassRate.ToString("C"));
					ratesValues.Add("U.S. Postal " + FirstClassName + "|" + FirstClassRate.ToString());
				}

				if(BPMRate != 0.0)
				{
					ratesText.Add("U.S. Postal " + BPMName + " " + BPMRate.ToString("C"));
					ratesValues.Add("U.S. Postal " + BPMName + "|" + BPMRate.ToString());
				}

				if(LibraryRate != 0.0)
				{
					ratesText.Add("U.S. Postal " + LibraryName + " " + LibraryRate.ToString("C"));
					ratesValues.Add("U.S. Postal " + LibraryName + "|" + LibraryRate.ToString());
				}

				if(MediaRate != 0.0)
				{
					ratesText.Add("U.S. Postal " + MediaName + " " + MediaRate.ToString("C"));
					ratesValues.Add("U.S. Postal " + MediaName + "|" + MediaRate.ToString());
				}

				USPSPackage = null;
			}
		}

		/// <summary>
		/// Convert the input number to the textual description of the Service Code
		/// </summary>
		/// <param name="code">The Service Code number to be converted</param>
		/// <returns></returns>
		private string UPSServiceCodeDescription(string code)
		{
			string result = string.Empty;
			switch(code)
			{
				case "01": 
					result = "UPS Next Day Air";
					break;
				case "02":
					result = "UPS 2nd Day Air";
					break;
				case "03":
					result = "UPS Ground";
					break;
				case "07":
					result = "UPS Worldwide Express";
					break;
				case "08": 
					result = "UPS Worldwide Expedited";
					break;
				case "11":
					result = "UPS Standard";
					break;
				case "12": 
					result = "UPS 3-Day Select";
					break;
				case "13": 
					result = "UPS Next Day Air Saver";
					break;
				case "14": 
					result = "UPS Next Day Air Early AM";
					break;
				case "54": 
					result = "UPS Worldwide Express Plus";
					break;
				case "59": 
					result = "UPS 2nd Day Air AM";
					break;
			}

			return result;
		}


		/// <summary>
		/// Convert the decimal weight passed in to pounds and ounces
		/// </summary>
		/// <param name="weight">The decimal weight to be convert (in pounds only)</param>
		/// <returns></returns>
		USPSWeight USPSGetWeight(Single weight)
		{
			Single pounds = 0;
			Single ounces = 0;

			pounds = Convert.ToInt32(weight - weight % 1);
			decimal tempWeight = (decimal)weight * 16;
			ounces = Convert.ToInt32(Math.Ceiling((Single)tempWeight - (Single)pounds * 16.0));

			USPSWeight w = new USPSWeight();
			w.pounds = int.Parse(pounds.ToString());
			w.ounces = int.Parse(ounces.ToString());

			return w;
		}

		/// <summary>
		/// Send and capture data using GET
		/// </summary>
		/// <param name="Request">The XML Request to be sent</param>
		/// <param name="Server">The server the request should be sent to</param>
		/// <returns>String</returns>
		private string GETandReceiveData(string Request, string Server)
		{
			HttpWebRequest requestX = (HttpWebRequest)WebRequest.Create(Server + "?" + Request);
			HttpWebResponse response = (HttpWebResponse)requestX.GetResponse();
			StreamReader sr = new StreamReader(response.GetResponseStream());
			string result = sr.ReadToEnd();
			response.Close();
			sr.Close();
			return result;
		}

		/// <summary>
		/// Send and capture data using Post
		/// </summary>
		/// <param name="Request">The XML Request to be sent</param>
		/// <param name="Server">The server the request should be sent to</param>
		/// <returns>String</returns>
		private string POSTandReceiveData(string Request, string Server)
		{
			// Set encoding & get content Length
			ASCIIEncoding encoding = new ASCIIEncoding();
			byte[] data = encoding.GetBytes(Request); // Request

			// Prepare post request
			HttpWebRequest shipRequest = (HttpWebRequest)WebRequest.Create(Server); // Server
			shipRequest.Method = "POST";
			shipRequest.ContentType="application/x-www-form-urlencoded";
			shipRequest.ContentLength = data.Length;
			Stream requestStream = shipRequest.GetRequestStream();
			// Send the data
			requestStream.Write(data,0,data.Length);
			requestStream.Close();
			// get the response
			WebResponse shipResponse = null;
			string response = String.Empty;
			try
			{
				shipResponse = shipRequest.GetResponse();
				using (StreamReader sr = new StreamReader(shipResponse.GetResponseStream()) )
				{
					response = sr.ReadToEnd();
					sr.Close();
				}
			}
			catch(Exception exc)
			{
				response = exc.ToString();
			}
			finally
			{
				if (shipResponse != null) shipResponse.Close();
			}

			shipRequest = null;
			requestStream = null;
			shipResponse = null;

			return response;
		}


		public void ClearRates()	// Clears all current rates in memory
		{
			ratesText.Clear();
			ratesValues.Clear();
		}

		public class Packages:CollectionBase	// Data class which holds the multiples packages information
		{
			private string m_pickuptype;
			private string m_destinationAddress1;
			private string m_destinationAddress2;
			private string m_destinationCity;
			private string m_destinationStateProvince;
			private string m_destinationZipPostalCode;
			private string m_destinationCountryCode;
			private Single m_weight;
			private bool m_residentialAddress;


			public Packages()
			{
				m_pickuptype = string.Empty;
				m_destinationAddress1 = string.Empty;
				m_destinationAddress2 = string.Empty;
				m_destinationCity = string.Empty;
				m_destinationStateProvince = string.Empty;
				m_destinationZipPostalCode = string.Empty;
				m_destinationCountryCode = string.Empty;
				m_weight = 0.0F;
				m_residentialAddress = false;
			}

			public Single Weight
			{
				get 
				{ 
					for(int i = 0;i<this.List.Count;i++)
					{
						Package p = (Package)this.List[i];
						this.m_weight += p.Weight;
						p = null;
					}

					return this.m_weight;
				}
			}

			public string PickupType	// Shipment pickup type
			{
				get { return this.m_pickuptype; }
				set { this.m_pickuptype = value.Trim(); }
			}

			public string DestinationCity
			{
				get { return this.m_destinationCity; }
				set { this.m_destinationCity = value; }
			}

			public string DestinationAddress1
			{
				get { return this.m_destinationAddress1; }
				set { this.m_destinationAddress1 = value; }
			}

			public string DestinationZipPostalCode
			{
				get { return this.m_destinationZipPostalCode; }
				set { this.m_destinationZipPostalCode = value; }
			}

			public string DestinationAddress2
			{
				get { return this.m_destinationAddress2; }
				set { this.m_destinationAddress2 = value; }
			}

			public string DestinationStateProvince
			{
				get { return this.m_destinationStateProvince; }
				set { this.m_destinationStateProvince = value; }
			}

			public string DestinationCountryCode
			{
				get { return this.m_destinationCountryCode; }
				set { this.m_destinationCountryCode = value; }
			}

			public bool ResidentialAddress
			{
				get { return this.m_residentialAddress; }
				set { this.m_residentialAddress = value; }
			}
			
			public void AddPackage(Package package)
			{
				this.List.Add(package);
			}


			public Package this[int index]
			{
				get
				{
					return (Package)this.List[index];
				}
			}
		}

		public class Package	// Data class which holds information about a single package
		{
			private Single m_weight;
			private Single m_height;
			private Single m_length;
			private Single m_width;
			private bool m_insured;
			private Single m_insuredValue;
			private int m_packageId;

			public Single InsuredValue
			{
				get { return this.m_insuredValue; }
				set { this.m_insuredValue = value; }
			}

			public int PackageId
			{
				get { return this.m_packageId; }
				set { this.m_packageId = value; }
			}

			public bool Insured
			{
				get { return this.m_insured; }
				set { this.m_insured = value; }
			}

			public Single Width
			{
				get { return this.m_width; }
				set { this.m_width = value; }
			}

			public Single Weight
			{
				get { return this.m_weight; }
				set { this.m_weight = value; }
			}

			public Single Height
			{
				get { return this.m_height; }
				set { this.m_height = value; }
			}

			public Single Length
			{
				get { return this.m_length; }
				set { this.m_length = value; }
			}


			public Package()
			{
			}
		}

		public enum Shipper	// Enum Shipper: The currently available shipping companies
		{
			UPS,
			USPS,
			FedEx,
			DHL
		}

		public enum ResultType	// Enum ResultType: The available return types of the shipment rating(s)
		{
			PlainText = 0,	// ResultType.PlainText: Specifies the resulting output to be plain text with &lt;BR&gt; tags to separate them
			SingleDropDownList = 1,	// ResultType.SingleDropDownList: Specifies the resulting output to be a single line drop down list
			MultiDropDownList = 2,	// ResultType.MultiDropDownList: Specifies the resulting output to be a multi-line combo-box
			RadioButtonList = 3,	// ResultType.RadioButtonList: Specifies the resulting output to be a list of radio buttons with labels
			RawDelimited = 4,	// ResultType.RawDelimited: Specifes the resulting output to be a delimited string. Rates are delimited with a pipe character (|), rate names &amp; prices are delimited with a comma (,)
			DropDownListControl = 5,	// ResultType.DropDownListControl: Specifes the resulting output to be a System.Web.UI.WebControls.DropDownList control.
			RadioButtonListControl = 6	// ResultType.RadioButtonListControl: Specifes the resulting output to be a System.Web.UI.WebControls.RadioButtonList control.
		}


		public class PickupTypes
		{
			/// <summary>
			/// Specifies the pickup type as: Daily Pickup
			/// </summary>
			public static string UPSDailyPickup 
			{
				get { return "01"; }
			}
			/// <summary>
			/// Specifies the pickup type as: Customer Counter
			/// </summary>
			public static string UPSCustomerCounter 
			{
				get { return "03"; }
			}
			/// <summary>
			/// Specifies the pickup type as: One time pickup
			/// </summary>
			public static string UPSOneTimePickup
			{
				get { return "06"; }
			}
			/// <summary>
			/// Specifies the pickup type as: On Call Air
			/// </summary>
			public static string UPSOnCallAir
			{
				get { return "07"; }
			}
			/// <summary>
			/// Specifies the pickup type as: Suggested retail rates
			/// </summary>
			public static string UPSSuggestedRetailRates
			{
				get { return "11"; }
			}
			/// <summary>
			/// Specifies the pickup type as: Letter center
			/// </summary>
			public static string UPSLetterCenter
			{
				get { return "19"; }
			}
			/// <summary>
			/// Specifies the pickup type as: Air service center
			/// </summary>
			public static string UPSAirServiceCenter
			{
				get { return "20"; }
			}
		}


		public struct USPSWeight	// Struct USPSWeight: Used to hold shipment weight in pounds and ounces
		{
			public int pounds;	// USPSWeight.pounds: Holds shipment weight in pounds
			public int ounces;	// USPSWeight.pounds: Holds shipment weight in remaining ounces
		}
	}

}
