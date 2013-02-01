using System;
using System.IO;
using nsoftware.IPWorksZip;

namespace IDXUpdate
{
	/// <summary>
	/// Summary description for Fnis.
	/// </summary>
	public class Fnis
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
			// Get main directory and files.
			DirectoryInfo compressedDir = new DirectoryInfo(CompressedPath);
			FileInfo[] items = compressedDir.GetFiles();
			
			// Get the data directory.
			DirectoryInfo dataDir = new DirectoryInfo(DataPath);

			// Create a new folder if one doesn't exist.
			if (!dataDir.Exists)
			{
				dataDir.Create();
			}
			
			// Output status to console
			Console.WriteLine("*** {0} MLS System ***", MlsName);
			Console.WriteLine("Start extracting data files...");

			foreach (FileInfo item in items)
			{
				if (item.Extension == ".gz")
				{
					// Split file into name and extension.
					string[] fileParts = item.Name.Split(new Char [] {'.'});
					
					// Initialize Gzip.
					Gzip gzip = new Gzip();
					
					// Extract file.
					gzip.ArchiveFile = item.FullName;
					gzip.FileDecompressedName = DataPath + "\\" + fileParts[0] + "." + fileParts[1];
					gzip.Extract();
					
					// Update the screen.
					Console.WriteLine("Finished extracting data file: {0}", item.Name);
				}
			}

			// Output status to console
			Console.WriteLine("Done extracting data files.");
		}

		public void ExtractGraphicsFiles()
		{
			DirectoryInfo mainDir = new DirectoryInfo(CompressedPath);
			FileInfo[] items = mainDir.GetFiles();
			
			// Get the graphics directory.
			DirectoryInfo graphicsDir = new DirectoryInfo(GraphicsPath);

			// Create a new folder if one doesn't exist.
			if (!graphicsDir.Exists)
			{
				graphicsDir.Create();
			}

			// Output status to console
			Console.WriteLine("*** {0} MLS System ***", MlsName);
			Console.WriteLine("Start extracting graphics files...");

			foreach (FileInfo item in items)
			{
				if (item.Extension == ".tar")
				{
					// Initialize Tar.
					Tar tar = new Tar();
					
					// Set file name.
					tar.ArchiveFile = item.FullName;
					
					// Set extract path to graphics path.
					tar.ExtractToPath = GraphicsPath;
					
					// Extract all files.
					tar.ExtractAll();
					
					// Update the console.
					Console.WriteLine("Finished extracting graphic file: {0}", item.Name);
				}
			}

			// Output status to console
			Console.WriteLine("Done extracting graphics files.");
		}

		public void ListDataFiles()
		{
			DirectoryInfo mainDir = new DirectoryInfo(CompressedPath);
			FileInfo[] items = mainDir.GetFiles();
			foreach (FileInfo item in items)
			{
				if (item.Extension == ".gz")
				{
					Console.WriteLine("Found {0} data file.", item.Name);
				}
			}
		}

		public void ListGraphicFiles()
		{
			DirectoryInfo mainDir = new DirectoryInfo(CompressedPath);
			FileInfo[] items = mainDir.GetFiles();
			foreach (FileInfo item in items)
			{
				if (item.Extension == ".tar")
				{
					Console.WriteLine("Found {0} data file.", item.Name);
				}
			}
		}
	}
}