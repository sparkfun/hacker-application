using System;

namespace CRENConversion
{
	/// <summary>
	/// Main entry Class for conversion of MLS numbers from Navica
	/// to Paragon for RE/MAX Mountain West.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			// Create new conversion object
			Conversion conversion = new Conversion();

			// Set the AccessConnString property
			conversion.AccessConnString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
				@"Data Source=CRENConvert.mdb;";

			// Set the SQLConnString property
			conversion.SQLConnString = "user id=sean;password=gren76b;" +
				"initial catalog=RMW;data source=FIRESTAR1;Connect Timeout=30";

			// Set the ListingPath property
			conversion.ListingPath = @"D:\websites\rmwrealestate\listings\";

			// Update business no real estate table
			conversion.Update("BusRealEstate", "BusinessNoRealEstate");

			// Update commercial table
			conversion.Update("Commercial", "CommercialRealEstate");

			// Update farm & ranch table
			conversion.Update("FarmRanch", "FarmRanchProperties");

			// Update residential table
			conversion.Update("Residential", "ResidentialProperties");

			// Update vacant land table
			conversion.Update("VacantLand", "VacantLand");

			// Stop the console
			Console.ReadLine();
		}
	}
}