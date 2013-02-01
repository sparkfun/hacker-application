// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Configuration;
using System.Text;
using System.IO;
using System.Security;
using System.Security.Cryptography;

namespace AspDotNetStorefrontEncrypt
{
	/// <summary>
	/// Summary description for Encrypt.
	/// </summary>
	public class Encrypt
	{
		static private Byte[] m_Key = new Byte[8]; 
		static private Byte[] m_IV = new Byte[8]; 

		public Encrypt()
		{}

		//////////////////////////
		//Function to encrypt data
		static public string EncryptData(String strKey, String strData)
		{
			string strResult;		//Return Result

			//1. String Length cannot exceed 90Kb. Otherwise, buffer will overflow. See point 3 for reasons
			if (strData.Length > 92160)
			{
				strResult="Error. Data String too large. Keep within 90Kb.";
				return strResult;
			}
	
			//2. Generate the Keys
			if (!InitKey(strKey))
			{
				strResult="Error. Fail to generate key for encryption";
				return strResult;
			}

			//3. Prepare the String
			//	The first 5 character of the string is formatted to store the actual length of the data.
			//	This is the simplest way to remember to original length of the data, without resorting to complicated computations.
			strData = String.Format("{0,5:00000}"+strData, strData.Length);


			//4. Encrypt the Data
			byte[] rbData = new byte[strData.Length];
			ASCIIEncoding aEnc = new ASCIIEncoding();
			aEnc.GetBytes(strData, 0, strData.Length, rbData, 0);
		
			DESCryptoServiceProvider descsp = new DESCryptoServiceProvider(); 
		
			ICryptoTransform desEncrypt = descsp.CreateEncryptor(m_Key, m_IV); 


			//5. Perpare the streams:
			//	mOut is the output stream. 
			//	mStream is the input stream.
			//	cs is the transformation stream.
			MemoryStream mStream = new MemoryStream(rbData); 
			CryptoStream cs = new CryptoStream(mStream, desEncrypt, CryptoStreamMode.Read);        
			MemoryStream mOut = new MemoryStream();
		
			//6. Start performing the encryption
			int bytesRead; 
			byte[] output = new byte[1024]; 
			do 
			{ 
				bytesRead = cs.Read(output,0,1024);
				if (bytesRead != 0) 
					mOut.Write(output,0,bytesRead); 
			} while (bytesRead > 0); 
		
			//7. Returns the encrypted result after it is base64 encoded
			//	In this case, the actual result is converted to base64 so that it can be transported over the HTTP protocol without deformation.
			if (mOut.Length == 0)		
				strResult = "";
			else
				strResult = Convert.ToBase64String(mOut.GetBuffer(), 0, (int)mOut.Length);
	
			return strResult;
		}

		//////////////////////////
		//Function to decrypt data
		static public string DecryptData(String strKey, String strData)
		{
			string strResult;

			//1. Generate the Key used for decrypting
			if (!InitKey(strKey))
			{
				strResult="Error. Fail to generate key for decryption";
				return strResult;
			}

			//2. Initialize the service provider
			int nReturn = 0;
			DESCryptoServiceProvider descsp = new DESCryptoServiceProvider(); 
			ICryptoTransform desDecrypt = descsp.CreateDecryptor(m_Key, m_IV); 
		
			//3. Prepare the streams:
			//	mOut is the output stream. 
			//	cs is the transformation stream.
			MemoryStream mOut = new MemoryStream();
			CryptoStream cs = new CryptoStream(mOut, desDecrypt, CryptoStreamMode.Write);        
		
			//4. Remember to revert the base64 encoding into a byte array to restore the original encrypted data stream
			byte[] bPlain = new byte[strData.Length];
			try 
			{
				bPlain = Convert.FromBase64CharArray(strData.ToCharArray(), 0, strData.Length);
			}
			catch (Exception) 
			{ 
				strResult = "Error. Input Data is not base64 encoded.";
				return strResult;
			}
		
			long lRead = 0;
			long lTotal = strData.Length;
		
			try
			{
				//5. Perform the actual decryption
				while (lTotal >= lRead)
				{ 
					cs.Write(bPlain,0,(int)bPlain.Length); 
					//descsp.BlockSize=64
					lRead = mOut.Length + Convert.ToInt64(Convert.ToUInt32((bPlain.Length / descsp.BlockSize) * descsp.BlockSize));
				};
			
				ASCIIEncoding aEnc = new ASCIIEncoding();
				strResult = aEnc.GetString(mOut.GetBuffer(), 0, (int)mOut.Length);
			
				//6. Trim the string to return only the meaningful data
				//	Remember that in the encrypt function, the first 5 character holds the length of the actual data
				//	This is the simplest way to remember to original length of the data, without resorting to complicated computations.
				String strLen = strResult.Substring(0,5);
				int nLen = Convert.ToInt32(strLen);
				strResult = strResult.Substring(5, nLen);
				nReturn = (int)mOut.Length;
			
				return strResult;
			}
			catch (Exception)
			{
				strResult = "Error. Decryption Failed. Possibly due to incorrect Key or corrputed data";
				return strResult;
			}
		}

		/////////////////////////////////////////////////////////////
		//Private function to generate the keys into member variables
		static private bool InitKey(String strKey)
		{
			try
			{
				// Convert Key to byte array
				byte[] bp = new byte[strKey.Length];
				ASCIIEncoding aEnc = new ASCIIEncoding();
				aEnc.GetBytes(strKey, 0, strKey.Length, bp, 0);
			
				//Hash the key using SHA1
				SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
				byte[] bpHash = sha.ComputeHash(bp);
			
				int i;
				// use the low 64-bits for the key value
				for (i=0; i<8; i++) 
					m_Key[i] = bpHash[i];

				for (i=8; i<16; i++) 
					m_IV[i-8] = bpHash[i];
			
				return true;
			}
			catch (Exception)
			{
				//Error Performing Operations
				return false;
			}
		}
	}
}
