namespace CopelandRealEstate
{
	/// <summary>
	/// Match all of the index values from different databases.
	/// </summary>
	public class DataMatcher : ConverterForm
	{
		// Match AgentID Numbers.
		public int MatchAgent(int oldAgentID)
		{
			// New AgentID number.
			int newAgentID;
			
			// Check old number and find matching new number.
			switch (oldAgentID)
			{
				case 9:
					newAgentID= 7;
					break;
				case 11:
					newAgentID = 6;
					break;
				case 13:
					newAgentID = 2;
					break;
				case 15:
					newAgentID = 4;
					break;
				case 16:
					newAgentID = 5;
					break;
				case 17:
					newAgentID = 8;
					break;
				default:
					newAgentID = 7;
					break;
			}
			
			// Return new number.
			return newAgentID;
		}
		
		public ClassPropertyType MatchClassListingType(string oldPropertyType)
		{
			// New ClassPropertyType.
			ClassPropertyType newCst;
			
			// Initialize.
			newCst.classID = ListingClass.ResidentialHomes;
			newCst.propertyTypeID = 7;

			// Check old property type.
			switch (oldPropertyType)
			{
				case "BoatSlip":
					newCst.classID = ListingClass.BoatSlipOther;
					newCst.propertyTypeID = 11;
					break;
				case "Commercial":
					newCst.classID = ListingClass.Commercial;
					newCst.propertyTypeID = 8;
					break;
				case "Condo":
					newCst.classID = ListingClass.ResidentialHomes;
					newCst.propertyTypeID = 1;
					break;
				case "Condo/Townhouse":
					newCst.classID = ListingClass.ResidentialHomes;
					newCst.propertyTypeID = 2;
					break;
				case "Lots/Land":
					newCst.classID = ListingClass.LotsLand;
					newCst.propertyTypeID = 9;
					break;
				case "Mobile Home":
					newCst.classID = ListingClass.ResidentialHomes;
					newCst.propertyTypeID = 5;
					break;
				case "Mobile Home w/Land":
					newCst.classID = ListingClass.ResidentialHomes;
					newCst.propertyTypeID = 6;
					break;
				case "Single Family":
					newCst.classID = ListingClass.ResidentialHomes;
					newCst.propertyTypeID = 7;
					break;
				default :
					newCst.classID = ListingClass.ResidentialHomes;
					newCst.propertyTypeID = 7;
					break;
			}
			
			// Return value.
			return newCst;
		}
		
		public int MatchLocation(string oldLocation)
		{
			// New location ID.
			int newLocationID;
			
			// Check old location string.
			switch (oldLocation)
			{
				case "Mainland":
					newLocationID = 5;
					break;
				case "Oceanview":
					newLocationID = 3;
					break;
				case "Soundfront":
					newLocationID = 4;
					break;
				case "Waterfront":
					newLocationID = 2;
					break;
				default:
					newLocationID = 5;
					break;
			}
			
			// Return value.
			return newLocationID;
		}
		
		public int MatchCity(string oldCity)
		{
			// New city ID.
			int newCityID;
			
			// Check old city string.
			switch (oldCity)
			{
				case "Atlantic Beach":
					newCityID = 3;
					break;
				case "Beaufort":
					newCityID= 1;
					break;
				case "Gloucester":
					newCityID = 5;
					break;
				case "Harkers Island":
					newCityID = 6;
					break;
				case "Marshallberg":
					newCityID = 4;
					break;
				case "Morehead City":
					newCityID = 2;
					break;
				case "Newport":
					newCityID = 7;
					break;
				case "Williston":
					newCityID = 8;
					break;
				default:
					newCityID = 1;
					break;
			}
			
			// Return value.
			return newCityID;
		}
		
		public int MatchStatus(string oldStatus)
		{
			// New status ID.
			int statusID;
			
			// Check old status string.
			switch (oldStatus)
			{
				case "Active":
					statusID = 1;
					break;
				case "Under Contract":
					statusID = 2;
					break;
				default:
					statusID = 1;
					break;
			}
			
			// Return value.
			return statusID;
		}
	}
}