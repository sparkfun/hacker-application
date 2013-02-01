using System;
using nsoftware.IPWorksZip;

namespace IDXUpdate
{
	/// <summary>
	/// Summary description for Sei.
	/// </summary>
	public class Sei
	{
		// Define Compressed Path Property
		string compressedPath;
		public string CompressedPath
		{
			get
			{
				return compressedPath;
			}
			set
			{
				compressedPath = value;
			}
		}

		// Define Data Path Property
		string dataPath;
		public string DataPath
		{
			get
			{
				return dataPath;
			}
			set
			{
				dataPath = value;
			}
		}

		// Define Graphics Path Property
		string graphicsPath;
		public string GraphicsPath
		{
			get
			{
				return graphicsPath;
			}
			set
			{
				graphicsPath = value;
			}
		}

		// Define MLS Name Property
		string mlsName;
		public string MlsName
		{
			get
			{
				return mlsName;
			}
			set
			{
				mlsName = value;
			}
		}

		public void ExtractDataFiles()
		{
			// Path to data file
			string dataFile = "IDX_Custom.zip";

			// Initialize zip component
			Zip zipFile = new Zip();

			// Output startup status to console
			Console.WriteLine("*** {0} MLS System ***", MlsName);
			Console.WriteLine("Start extracting data files...");

			// Extract data file
			zipFile.ArchiveFile = CompressedPath + dataFile;
			zipFile.ExtractToPath = DataPath;
			zipFile.OverwriteFiles = true;
			zipFile.ExtractAll();

			// Output status to console
			Console.WriteLine("Done extracting data files.");
		}

		public void ExtractGraphicFiles()
		{
			// Path to graphic files
			string daily = "daily.zip";
			string weeklyA = "weeklya.zip";
			string weeklyB = "weeklyb.zip";

			// Initialize zip component
			Zip zipFile = new Zip();

			// Output startup status to console
			Console.WriteLine("*** {0} MLS System ***", MlsName);
			Console.WriteLine("Start extracting graphics files...");

			// Extract daily photo file
			zipFile.ArchiveFile = CompressedPath + daily;
			zipFile.ExtractToPath = GraphicsPath;
			zipFile.OverwriteFiles = true;
			zipFile.ExtractAll();
			Console.WriteLine("Finished extracting daily photo file.");

			// Reset zip component
			zipFile.Reset();

			// Extract daily weekly A photo file
			zipFile.ArchiveFile = CompressedPath + weeklyA;
			zipFile.ExtractToPath = GraphicsPath;
			zipFile.OverwriteFiles = true;
			zipFile.ExtractAll();
			Console.WriteLine("Finished extracting weekly photo file A.");

			// Reset zip component
			zipFile.Reset();

			// Extract daily weekly B photo file
			zipFile.ArchiveFile = CompressedPath + weeklyB;
			zipFile.ExtractToPath = GraphicsPath;
			zipFile.OverwriteFiles = true;
			zipFile.ExtractAll();
			Console.WriteLine("Finished extracting weekly photo file B.");

			// Output status to console
			Console.WriteLine("Done extracting graphics files.");
		}
	}
}