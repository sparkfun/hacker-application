// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Configuration;
using System.Data;
using System.Text;
using System.Collections;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

namespace AspDotNetStorefrontCommon
{
	/// <summary>
	/// Summary description for ProductImageGallery.
	/// </summary>
	public class ProductImageGallery
	{

		public int _productID;
		public int _variantID;
		public int _siteID;
		public String _localeSetting;
		public int _maxImageIndex; // will be 0 if empty
		public String _colors;
		public String[] _colorsSplit;
		public String _imageNumbers = "1,2,3,4,5,6,7,8,9,10";
		public String[] _imageNumbersSplit;
		public String _imgGalIcons;
		public String _imgDHTML;
		private String[,] _imageUrls;

		public ProductImageGallery(int ProductID, int SiteID, String LocaleSetting)
		{
			_productID = ProductID;
			_variantID = Common.GetFirstProductVariant(_productID);
			_siteID = SiteID;
			_localeSetting = LocaleSetting;
			_maxImageIndex = 0;
			_colors = String.Empty;
			_imgGalIcons = String.Empty;
			_imgDHTML = String.Empty;
			LoadFromDB();
		}

		public ProductImageGallery(int ProductID, int VariantID, int SiteID, String LocaleSetting)
		{
			_productID = ProductID;
			_variantID = VariantID;
			_siteID = SiteID;
			_localeSetting = LocaleSetting;
			_maxImageIndex = 0;
			_colors = String.Empty;
			_imgGalIcons = String.Empty;
			_imgDHTML = String.Empty;
			LoadFromDB();
		}

		public bool IsEmpty()
		{
			return _maxImageIndex == 0;
		}

		public void LoadFromDB()
		{
			_imageNumbersSplit = _imageNumbers.Split(',');
			_colors = String.Empty;
			_colorsSplit = new String[1] {""};
			IDataReader rs = DB.GetRS("select * from productvariant  " + DB.GetNoLock() + " where VariantID=" + _variantID.ToString());
			if(rs.Read())
			{
				_colors = DB.RSField(rs,"Colors"); // remember to add "empty" color to front, for no color selected
				if(_colors.Length != 0)
				{
					_colorsSplit = ("," + _colors).Split(',');
				}
			}
			rs.Close();
			if(_colors.Length != 0)
			{
				for(int i = _colorsSplit.GetLowerBound(0); i <= _colorsSplit.GetUpperBound(0); i++)
				{
					_colorsSplit[i] = Common.MakeSafeFilesystemName(_colorsSplit[i]);
				}
			}
			_imageUrls = new String[_imageNumbersSplit.Length,_colorsSplit.Length];
			for(int j = _imageNumbersSplit.GetLowerBound(0); j <= _imageNumbersSplit.GetUpperBound(0); j++)
			{
				int ImgIdx = Localization.ParseUSInt(_imageNumbersSplit[j]);
				for(int i = _colorsSplit.GetLowerBound(0); i <= _colorsSplit.GetUpperBound(0); i++)
				{
					_imageUrls[j,i] = Common.LookupProductImageByNumberAndColor(_productID,_siteID,_localeSetting,ImgIdx,_colorsSplit[i]);
				}
			}
			for(int j = _imageNumbersSplit.GetLowerBound(0); j <= _imageNumbersSplit.GetUpperBound(0); j++)
			{
				int ImgIdx = Localization.ParseUSInt(_imageNumbersSplit[j]);
				if(_imageUrls[j,0].IndexOf("nopicture.") == -1)
				{
					_maxImageIndex = ImgIdx;
				}
			}
			if(!IsEmpty())
			{
				StringBuilder tmpS = new StringBuilder(5000);
				tmpS.Append("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				tmpS.Append("var ProductPicIndex = 1;\n");
				tmpS.Append("var ProductColor = '';\n");
				tmpS.Append("var boardpics = new Array();\n");
				for(int i = 1; i <= _maxImageIndex; i++)
				{
					foreach(String c in _colorsSplit)
					{
						tmpS.Append("boardpics['" + i.ToString() + "," + c + "'] = '" + ImageUrl(i,c).ToLower() + "';\n");
					}
				}
				tmpS.Append("function changecolorimg()\n");
				tmpS.Append("{\n");
				tmpS.Append("	var scidx = ProductPicIndex + ',' + ProductColor.toLowerCase();\n");
				//tmpS.Append("	alert('scidx='+scidx);\n");
				//tmpS.Append("	alert('boardpics[scidx]'+boardpics[scidx]);\n");
				tmpS.Append("	ProductPic" + _productID.ToString() + ".src=boardpics[scidx];\n");
				//tmpS.Append("	alert('done, src=' + ProductPic" + ProductID.ToString() + ".src);\n");
				tmpS.Append("}\n");
				tmpS.Append("function setcolorpicidx(idx)\n");
				tmpS.Append("{\n");
				tmpS.Append("	ProductPicIndex = idx;\n");
				tmpS.Append("	changecolorimg();\n");
				tmpS.Append("}\n");
				tmpS.Append("function setcolorpic(color)\n");
				tmpS.Append("{\n");
				tmpS.Append("	if(color == '-,-' || color == '-')\n");
				tmpS.Append("		{\n");
				tmpS.Append("		color = '';\n");
				tmpS.Append("		}\n");
				//tmpS.Append("	alert(ProductPicIndex + ',' + ProductColor);\n");
				tmpS.Append("	if(color != '' && color.indexOf(',') != -1)\n");
				tmpS.Append("		{\n");
				tmpS.Append("		color = color.substring(0,color.indexOf(',')).replace(new RegExp(\"'\", 'gi'), '');\n"); // remove sku from color select value
				tmpS.Append("		if(color != '' && color.indexOf('[') != -1)\n");
				tmpS.Append("			{\n");
				tmpS.Append("			color = color.substring(0,color.indexOf('[')).replace(new RegExp(\"'\", 'gi'), '');\n");
				tmpS.Append("			color = color.replace(/[\\s]+$/g,\"\");\n");
				tmpS.Append("			}\n");
				tmpS.Append("		}\n");
				tmpS.Append("	ProductColor = color;\n");
				//tmpS.Append("	alert(ProductPicIndex + ',' + ProductColor);\n");
				tmpS.Append("	changecolorimg();\n");
				tmpS.Append("	return (true);\n");
				tmpS.Append("}\n");
				tmpS.Append("</script>\n");
				_imgDHTML = tmpS.ToString();

				if(_maxImageIndex > 1)
				{
					tmpS.Remove(0,tmpS.Length);
					for(int i = 1; i <= _maxImageIndex; i++)
					{
						tmpS.Append("<img style=\"cursor:hand;\" onClick=\"setcolorpicidx(" + i.ToString() + ");\" alt=\"Show Picture " + i.ToString() + "\" src=\"skins/skin_" + _siteID.ToString() + "/images/im" + i.ToString() + ".gif\" border=\"0\">");
					}
					_imgGalIcons = tmpS.ToString();
				}
			}
		}

		private int GetColorIndex(String Color)
		{
			int i = 0;
			foreach(String s in _colorsSplit)
			{
				if(s == Color)
				{
					return i;
				}
				i++;
			}
			return 0;
		}

		public String ImageUrl(int Index, String Color)
		{
			try
			{
				return _imageUrls[Index-1,GetColorIndex(Color)];
			}
			catch
			{
				return String.Empty;
			}
		}

		private String GetCacheString()
		{
			return String.Empty;
		}

	}
}
