using System;

namespace IDXUpdate
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
//			//***************************************
//			// Start Aspen MLS Update
//			//***************************************
//
//			// New Fnis constructor
//			Fnis aspenUpdate = new Fnis();
//
//			// Set default paths
//			aspenUpdate.CompressedPath = @"D:\IDX\Aspen\Compressed\";
//			aspenUpdate.DataPath = @"D:\IDX\Aspen\Data\";
//			aspenUpdate.GraphicsPath = @"D:\IDX\Aspen\Photos\";
//
//			// Set MlsName property
//			aspenUpdate.MlsName = "Aspen";
//
//			// Extract data files
//			aspenUpdate.ExtractDataFiles();
//
//			// Extract graphics files
//			aspenUpdate.ExtractGraphicsFiles();

//			//***************************************
//			// Start Glenwood Springs MLS Update
//			//***************************************
//
//			// New Fnis constructor
//			Fnis glenwoodUpdate = new Fnis();
//
//			// Set default paths
//			glenwoodUpdate.CompressedPath = @"D:\IDX\Glenwood Springs\Compressed\";
//			glenwoodUpdate.DataPath = @"D:\IDX\Glenwood Springs\Data\";
//			glenwoodUpdate.GraphicsPath = @"D:\IDX\Glenwood Springs\Photos\";
//
//			// Set MlsName property
//			glenwoodUpdate.MlsName = "Glenwood Springs";
//
//			// Extract data files
//			glenwoodUpdate.ExtractDataFiles();
//
//			// Extract graphics files
//			glenwoodUpdate.ExtractGraphicsFiles();

//			//***************************************
//			// Start CREN MLS Update
//			//***************************************
//
//			// Create new CREN object
//			Fnis crenUpdate = new Fnis();
//
//			// Set the default file paths
//			crenUpdate.CompressedPath = @"D:\IDX\CREN\Compressed\";
//			crenUpdate.DataPath = @"D:\IDX\CREN\Data\";
//			crenUpdate.GraphicsPath = @"D:\IDX\CREN\Photos";
//
//			// Set the MLS name property
//			crenUpdate.MlsName = "CREN (Colorado Real Estate Network)";
//
//			// Extract the data and graphics files
//			crenUpdate.ExtractDataFiles();
//			crenUpdate.ExtractGraphicsFiles();
			
			//***************************************
			// Start CREN MLS Update
			//***************************************

			// Create new CREN object
			Fnis crenUpdate = new Fnis();

			// Set the default file paths
			crenUpdate.CompressedPath = @"D:\IDX\CREN2\Compressed\";
			crenUpdate.DataPath = @"D:\IDX\CREN2\Data";
			crenUpdate.GraphicsPath = @"D:\IDX\CREN2\Photos";

			// Set the MLS name property
			crenUpdate.MlsName = "CREN (Colorado Real Estate Network)";

			// Extract the data and graphics files
			crenUpdate.ExtractDataFiles();
			crenUpdate.ExtractGraphicsFiles();

			//***************************************
			// Start AGS MLS Update
			//***************************************

			// Create new CREN object
			Fnis agsUpdate = new Fnis();

			// Set the default file paths
			agsUpdate.CompressedPath = @"D:\IDX\AGS\Compressed\";
			agsUpdate.DataPath = @"D:\IDX\AGS\Data";
			agsUpdate.GraphicsPath = @"D:\IDX\AGS\Photos";

			// Set the MLS name property
			agsUpdate.MlsName = "AGS (Aspen-Glenwood MLS System)";

			// Extract the data and graphics files
			agsUpdate.ExtractDataFiles();
			agsUpdate.ExtractGraphicsFiles();

//			//***************************************
//			// Start Montrose MLS Update
//			//***************************************
//
//			// New Fnis constructor
//			Fnis montroseUpdate = new Fnis();
//
//			// Set default paths
//			montroseUpdate.CompressedPath = @"D:\IDX\Montrose\Compressed\";
//			montroseUpdate.DataPath = @"D:\IDX\Montrose\Data\";
//			montroseUpdate.GraphicsPath = @"D:\IDX\Montrose\Photos\";
//
//			// Set MlsName property
//			montroseUpdate.MlsName = "Montrose";
//
//			// Extract data files
//			montroseUpdate.ExtractDataFiles();
//
//			// Extract graphics files
//			montroseUpdate.ExtractGraphicsFiles();
//
//			//***************************************
//			// Start Grand Junction MLS Update
//			//***************************************
//
//			// New Fnis constructor
//			Fnis grandJunctionUpdate = new Fnis();
//
//			// Set default paths
//			grandJunctionUpdate.CompressedPath = @"D:\IDX\Grand Junction\Compressed\";
//			grandJunctionUpdate.DataPath = @"D:\IDX\Grand Junction\Data\";
//			grandJunctionUpdate.GraphicsPath = @"D:\IDX\Grand Junction\Photos\";
//
//			// Set MlsName property
//			grandJunctionUpdate.MlsName = "Grand Junction";
//
//			// Extract data files
//			grandJunctionUpdate.ExtractDataFiles();
//
//			// Extract graphics files
//			grandJunctionUpdate.ExtractGraphicsFiles();
//
//			//***************************************
//			// Start Delta County MLS Update
//			//***************************************
//
//			// New Sei constructor.
//			Sei seiUpdate = new Sei();
//
//			// Set default paths
//			seiUpdate.CompressedPath = @"D:\IDX\Delta\Compressed\";
//			seiUpdate.DataPath = @"D:\IDX\Delta\Data\";
//			seiUpdate.GraphicsPath = @"D:\IDX\Delta\Photos\";
//
//			// Set MlsName property
//			seiUpdate.MlsName = "Delta";
//
//			// Extract data files
//			seiUpdate.ExtractDataFiles();
//
//			// Extract graphics files
//			seiUpdate.ExtractGraphicFiles();
		}
	}
}