using System.IO;
using System.Text;

namespace AccessDataUpgrader
{
	/// <summary>
	/// Summary description for LogBuilder.
	/// </summary>
	public class LogBuilder
	{
		// StringBuilder to save log file data
		// to before saving.
		protected StringBuilder sbLog;
		
		public LogBuilder()
		{
			sbLog = new StringBuilder("****Starting Log****");
		}
		
		public FileStream CreateLogFile(string logFileName)
		{
			FileStream fileStream = new FileStream(logFileName,
			                                       FileMode.Create,
			                                       FileAccess.ReadWrite,
			                                       FileShare.None);
			return (fileStream);
		}
		
		public void WriteToLog(FileStream fileStream, StringBuilder sbLog)
		{
			// Make sure we can write to the stream.
			if (!fileStream.CanWrite)
			{
				// Close it and re-open for append.
				string fileName = fileStream.Name;
				fileStream.Close();
				fileStream = new FileStream(fileName, FileMode.Append);
			}
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.WriteLine(sbLog.ToString());
			streamWriter.Close();
		}
		
		public void WriteLineToLog(string data)
		{
			sbLog.Append(data + "\n\r");
		}
	}
}