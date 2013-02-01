using System;
using System.IO;
using System.Diagnostics;
using System.Web;

namespace AspDotNetStorefrontCommon
{
	/// <summary>
	/// Helper class to allow sending files from anywhere in the file system as HTTP downloads.
	/// </summary>
	/// 

  public struct DownloadItem
  {
    public int productID;
    public int variantID;
    public String downloadLocation;
    public String productName;
  }

	public class DownloadManager
	{

    public const int cBufferSize = 1024;


    #region Methods
    public DownloadManager()
		{
		}

    /// <summary>
    /// Downloads the file at filepath to a browser
    /// </summary>
    public static void DownloadFile(System.Web.HttpResponse response, string filePath)
    {

      Stream outputStream = response.OutputStream;
      int byteCount;
      byte[] readBytes = new byte[cBufferSize];
      FileStream fileStream = null;
      
      response.AddHeader("content-disposition","attachment; filename=" + HttpUtility.UrlEncode(Path.GetFileName(filePath)).Replace("+","%20"));
      
      try
      {
        fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read,FileShare.Read);
        do
        {
          byteCount = fileStream.Read(readBytes,0,cBufferSize);
          outputStream.Write(readBytes,0,byteCount);
        }
        while (byteCount == cBufferSize);
      }
      catch(Exception ex)
      {
        Debug.WriteLine(ex.ToString());
        throw ex;
      }
      finally
      {
        if (fileStream != null) fileStream.Close();
        fileStream=null;
      }
    }
    
    public static void DownloadXML(System.Web.HttpResponse response, string XMLString, string fileName)
    {
      response.AddHeader("content-disposition","attachment; filename=" + HttpUtility.UrlEncode(fileName));
      response.ContentType = "text/xml";
      response.Write(XMLString);
      response.Flush();
    }

    #endregion

	}
}
