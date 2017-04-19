using System;

namespace AccessDataUpgrader
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class MainEntry
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			// Create new Upgrader object.
			Upgrader upg = new Upgrader();

			// Set the AccessConnString property to the RVParkStore.mdb
			// database.
			upg.AccessConnString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
				@"Data Source=RVParkStore.mdb;";

			// Set the SQLConnString property to the RVParkStore database.
			upg.SQLConnString = "user id=rvps;password=3O9Q1af89;" +
				"initial catalog=RVParkStore;data source=HOMESTAR;Connect Timeout=30";
			
			// Upgrade all of the records.
			upg.ProcessRecords();
			
//			// Test File Extension Algorithm.
//			string fileName = "RVPKAd-Photo-636-5.jpg.pdf ";
//			Upgrader.FileType fileType = upg.GetFileExtension(fileName);
//			Console.WriteLine("File Type: {0}", fileType.ToString());
			
			// Wait for response.
			Console.ReadLine();
		}
	}
}