// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Text;
using System.Web;

namespace AspDotNetStorefrontCommon
{
	/// <summary>
	/// Summary description for SE.
	/// </summary>
	public class SE
	{
		public SE()	{}

		static public String MakeManufacturerLink(int ManufacturerID, String SEName)
		{
			String URL = "showmanufacturer.aspx?ManufacturerID=" + ManufacturerID.ToString();
			if(Common.ApplicationBool("UseStaticLinks") && !Common.IsAdminSite)
			{
				URL = "m-" + ManufacturerID.ToString() + "-";
				if(SEName.Length !=0)
				{
					URL += MungeName(SEName);
				}
				else
				{
					URL += SE.GetManufacturerSEName(ManufacturerID);
				}
				URL += ".aspx";
			}
			return URL.ToLower();
		}

		static public String MakeCategoryLink(int CategoryID, String SEName)
		{
			String URL = "showcategory.aspx?CategoryID=" + CategoryID.ToString();
			if(Common.ApplicationBool("UseStaticLinks") && !Common.IsAdminSite)
			{
				URL = "c-" + CategoryID.ToString() + "-";
				if(SEName.Length !=0)
				{
					URL += MungeName(SEName);
				}
				else
				{
					URL += SE.GetCategorySEName(CategoryID);
				}
				URL += ".aspx";
			}
			return URL.ToLower();
		}

		static public String MakeSectionLink(int SectionID, String SEName)
		{
			String URL = "showsection.aspx?Sectionid=" + SectionID.ToString();
			if(Common.ApplicationBool("UseStaticLinks") && !Common.IsAdminSite)
			{
				URL = "s-" + SectionID.ToString() + "-";
				if(SEName.Length !=0)
				{
					URL += MungeName(SEName);
				}
				else
				{
					URL += SE.GetSectionSEName(SectionID);
				}
				URL += ".aspx";
			}
			return URL.ToLower();
		}

		static public String MakeProductLink(int ProductID, String SEName)
		{
			String URL = "showproduct.aspx?Productid=" + ProductID.ToString();
			if(Common.ApplicationBool("UseStaticLinks") && !Common.IsAdminSite)
			{
				URL = "p-" + ProductID.ToString() + "-";
				if(SEName.Length !=0)
				{
					URL += MungeName(SEName);
				}
				else
				{
					URL += SE.GetProductSEName(ProductID);
				}
				URL += ".aspx";
			}
			return URL.ToLower();
		}

		static public String MakeProductAndCategoryLink(int ProductID, int CategoryID, String SEName)
		{
			String URL = "showproduct.aspx?Productid=" + ProductID.ToString() + "&categoryid=" + CategoryID.ToString();
			if(Common.ApplicationBool("UseStaticLinks") && !Common.IsAdminSite)
			{
				URL = "pc-" + ProductID.ToString() + "-" + CategoryID.ToString() + "-";
				if(SEName.Length !=0)
				{
					URL += MungeName(SEName);
				}
				else
				{
					URL += SE.GetProductSEName(ProductID);
				}
				URL += ".aspx";
			}
			return URL.ToLower();
		}

		static public String MakeProductAndSectionLink(int ProductID, int SectionID, String SEName)
		{
			String URL = "showproduct.aspx?Productid=" + ProductID.ToString() + "&sectionid=" + SectionID.ToString();
			if(Common.ApplicationBool("UseStaticLinks") && !Common.IsAdminSite)
			{
				URL = "ps-" + ProductID.ToString() + "-" + SectionID.ToString() + "-";
				if(SEName.Length !=0)
				{
					URL += MungeName(SEName);
				}
				else
				{
					URL += SE.GetProductSEName(ProductID);
				}
				URL += ".aspx";
			}
			return URL.ToLower();
		}

		static public String MakeDriverLink(String Topic)
		{
			String URL = "driver.aspx?topic=" + Topic;
			if(Common.ApplicationBool("UseStaticLinks") && !Common.IsAdminSite)
			{
				URL = "t-" + SE.MungeName(Topic) + ".aspx";
			}
			return URL.ToLower();
		}

		static public int LookupSECategory(String SEName)
		{
			int CategoryID = 0;
			IDataReader rs = DB.GetRS("Select * from Category  " + DB.GetNoLock() + " where Deleted=0 and Published<>0 and SEName=" + DB.SQuote(SEName));
			if(rs.Read())
			{
				CategoryID = DB.RSFieldInt(rs,"CategoryID");
			}
			rs.Close();
			return CategoryID;
		}

		static public int LookupSEManufacturer(String SEName)
		{
			int ManufacturerID = 0;
			IDataReader rs = DB.GetRS("Select * from Manufacturer  " + DB.GetNoLock() + " where Deleted=0 and Published<>0 and SEName=" + DB.SQuote(SEName));
			if(rs.Read())
			{
				ManufacturerID = DB.RSFieldInt(rs,"ManufacturerID");
			}
			rs.Close();
			return ManufacturerID;
		}

		static public int LookupSESection(String SEName)
		{
			int SectionID = 0;
			IDataReader rs = DB.GetRS("Select * from [section]  " + DB.GetNoLock() + " where Deleted=0 and Published<>0 and SEName=" + DB.SQuote(SEName));
			if(rs.Read())
			{
				SectionID = DB.RSFieldInt(rs,"SectionID");
			}
			rs.Close();
			return SectionID;
		}

		static public int LookupSEProduct(String SEName)
		{
			int SectionID = 0;
			IDataReader rs = DB.GetRS("Select * from Product  " + DB.GetNoLock() + " where Deleted=0 and Published<>0 and SEName=" + DB.SQuote(SEName));
			if(rs.Read())
			{
				SectionID = DB.RSFieldInt(rs,"ProductID");
			}
			rs.Close();
			return SectionID;
		}

		static public int LookupSEVariant(String SEName)
		{
			int SectionID = 0;
			IDataReader rs = DB.GetRS("Select * from Variant  " + DB.GetNoLock() + " where Deleted=0 and Published<>0 and SEName=" + DB.SQuote(SEName));
			if(rs.Read())
			{
				SectionID = DB.RSFieldInt(rs,"VariantID");
			}
			rs.Close();
			return SectionID;
		}

		static public String MungeName(String s)
		{
			String OkChars = "abcdefghijklmnopqrstuvwxyz1234567890 _-";
			s = s.Trim().ToLower();
			StringBuilder tmpS = new StringBuilder(s.Length);
			for(int i=0;i<s.Length;i++) 
			{
				if(OkChars.IndexOf(s[i]) != -1)
				{
					tmpS.Append(s[i]);
				}
			}
			tmpS = tmpS.Replace(" ","-");
			tmpS = tmpS.Replace("--","-");
			return HttpContext.Current.Server.UrlEncode(tmpS.ToString());
		}

		public static String GetCategorySEName(int CategoryID)
		{
			IDataReader rs = DB.GetRS("Select SEName from Category  " + DB.GetNoLock() + " where CategoryID=" + CategoryID.ToString());
			String uname = String.Empty;
			if(rs.Read())
			{
				uname = DB.RSField(rs,"SEName");
			}
			rs.Close();
			if(uname.Length == 0)
			{
				uname = Common.GetCategoryName(CategoryID);
			}
			return SE.MungeName(uname);
		}

		public static String GetManufacturerSEName(int ManufacturerID)
		{
			IDataReader rs = DB.GetRS("Select SEName from Manufacturer  " + DB.GetNoLock() + " where ManufacturerID=" + ManufacturerID.ToString());
			String uname = String.Empty;
			if(rs.Read())
			{
				uname = DB.RSField(rs,"SEName");
			}
			rs.Close();
			if(uname.Length == 0)
			{
				uname = Common.GetManufacturerName(ManufacturerID);
			}
			return SE.MungeName(uname);
		}

		public static String GetSectionSEName(int SectionID)
		{
			IDataReader rs = DB.GetRS("Select SEName from [section]  " + DB.GetNoLock() + " where SectionID=" + SectionID.ToString());
			String uname = String.Empty;
			if(rs.Read())
			{
				uname = DB.RSField(rs,"SEName");
			}
			rs.Close();
			if(uname.Length == 0)
			{
				uname = Common.GetSectionName(SectionID);
			}
			return SE.MungeName(uname);
		}

		public static String GetProductSEName(int ProductID)
		{
			IDataReader rs = DB.GetRS("Select SEName from Product  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString());
			String uname = String.Empty;
			if(rs.Read())
			{
				uname = DB.RSField(rs,"SEName");
			}
			rs.Close();
			if(uname.Length == 0)
			{
				uname = Common.GetProductName(ProductID);
			}
			return SE.MungeName(uname);
		}

		public static String GetVariantSEName(int VariantID)
		{
			IDataReader rs = DB.GetRS("Select SEName from Variant  " + DB.GetNoLock() + " where VariantID=" + VariantID.ToString());
			String uname = String.Empty;
			if(rs.Read())
			{
				uname = DB.RSField(rs,"SEName");
			}
			rs.Close();
			if(uname.Length == 0)
			{
				uname = Common.GetVariantName(VariantID);
			}
			return SE.MungeName(uname);
		}


	}
}
