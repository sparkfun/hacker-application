// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Web;
using System.Configuration;
using System.Web.SessionState;
using System.Web.Caching;

namespace AspDotNetStorefrontCommon
{
	/// <summary>
	/// Summary description for Ratings.
	/// </summary>
	public class Ratings
	{
		public Ratings() {}

		static public int GetProductRating(int CustomerID, int ProductID)
		{
			IDataReader rs = DB.GetRS("Select rating from Rating  " + DB.GetNoLock() + " where CustomerID=" + CustomerID.ToString() + " and ProductID=" + ProductID.ToString());
			int uname = 0;
			if(rs.Read())
			{
				uname = DB.RSFieldInt(rs,"rating");
			}
			rs.Close();
			return uname;
		}

		static public bool StringHasBadWords(String s)
		{
			if(s.Length == 0)
			{
				return false;
			}
			DataSet dsBad = DB.GetDS("select * from BadWord " + DB.GetNoLock(),true,System.DateTime.Now.AddDays(1));
			s = s.ToLower();
			bool hasBad = false;
			foreach(DataRow row in dsBad.Tables[0].Rows)
			{
				if(s.IndexOf(DB.RowField(row,"Word").ToLower()) != -1)
				{
					hasBad = true;
					break;
				}
			}
			dsBad.Dispose();
			return hasBad;
		}

		static public void DeleteRating(int RatingID)
		{
			if(RatingID != 0)
			{
				int CustID = 0;
				int ProdID = 0;
				IDataReader rs = DB.GetRS("select * from Rating  " + DB.GetNoLock() + " where RatingID=" + RatingID.ToString());
				if(rs.Read())
				{
					CustID = DB.RSFieldInt(rs,"CustomerID");
					ProdID = DB.RSFieldInt(rs,"ProductID");
				}
				rs.Close();
				DB.ExecuteSQL("delete from Rating where RatingID=" + Common.QueryString("DeleteID"));
				if(CustID != 0 && ProdID != 0)
				{
					DB.ExecuteSQL("delete from RatingCommentHelpfulness where RatingCustomerID=" + CustID.ToString() + " and ProductID=" + ProdID.ToString());
				}
			}
		}

		
		static public String DisplayForCustomer(int CustomerID, int _siteID)
		{
			String CustomerName = Customer.GetName(CustomerID);
			StringBuilder tmpS = new StringBuilder(50000);

			String OrderBy = "Ratings_Average desc, Ratings_Count desc, Name asc";
			String SortDescription = "Helpful to Less Helpful";
			String FilterDescription = String.Empty;
			String FieldSuffix = String.Empty;
			int OrderByIdx = Common.QueryStringUSInt("OrderBy");
			if(OrderByIdx == 0)
			{
				OrderByIdx = 3;
			}
			switch(OrderByIdx)
			{
				case 1:
					SortDescription = "Helpful to Less Helpful";
					OrderBy = "FoundHelpful desc, DateEntered desc";
					break;
				case 2:
					SortDescription = "Less Helpful to Helpful";
					OrderBy = "FoundHelpful asc, DateEntered desc";
					break;
				case 3:
					SortDescription = "New to Old";
					OrderBy = "DateEntered desc";
					break;
				case 4:
					SortDescription = "Old to New";
					OrderBy = "DateEntered asc";
					break;
				case 5:
					SortDescription = "High to Low Rating";
					OrderBy = "Rating desc, DateEntered desc";
					break;
				case 6:
					SortDescription = "Low to High Rating";
					OrderBy = "Rating asc, DateEntered desc";
					break;
			}

			tmpS.Append("<form name=\"ShowProduct\" action=\"customerratings.aspx\" method=\"GET\">\n");
			tmpS.Append("<input type=\"hidden\" name=\"CustomerID\" value=\"" + CustomerID.ToString() + "\">");
			
			DataSet ds = DB.GetDS("select * from product_comments_view  " + DB.GetNoLock() + " where CustomerID=" + CustomerID.ToString() + " order by " + OrderBy,false,System.DateTime.Now.AddMinutes(1));
			
			int NumRows = ds.Tables[0].Rows.Count;
			int PageSize = 1000000;
			int PageNum = Common.QueryStringUSInt("PageNum");
			if(PageNum == 0)
			{
				PageNum = 1;
			}
			if(Common.QueryString("show") == "all")
			{
				PageSize = 1000000;
				PageNum = 1;
			}
			int NumPages = (int)(NumRows/PageSize) + Common.IIF(NumRows % PageSize == 0 , 0 , 1);
			if(PageNum > NumPages)
			{
				if(NumRows > 0)
				{
					HttpContext.Current.Response.Redirect("customerratings.aspx?CustomerID=" + CustomerID.ToString() + "&pagenum=" + (PageNum-1).ToString());
				}
			}
			int StartRow = (PageSize*(PageNum-1)) + 1;
			int StopRow = StartRow + PageSize -1 ;
			if(StopRow > NumRows)
			{
				StopRow = NumRows;
			}
			

			if(NumRows > 0)
			{
				tmpS.Append("<br>Sort: " + SortDescription + "&nbsp;&nbsp;RE-SORT COMMENTS: ");
				tmpS.Append("<select size=\"1\" name=\"OrderBy\" onChange=\"document.ShowProduct.submit();\">\n");
				tmpS.Append("<option value=\"1\" " + Common.IIF(OrderByIdx == 1 , " selected" , "") + ">Helpful to Less Helpful</option>\n");
				tmpS.Append("<option value=\"2\" " + Common.IIF(OrderByIdx == 2 , " selected" , "") + ">Less Helpful to Helpful</option>\n");
				tmpS.Append("<option value=\"3\" " + Common.IIF(OrderByIdx == 3 , " selected" , "") + ">New to Old</option>\n");
				tmpS.Append("<option value=\"4\" " + Common.IIF(OrderByIdx == 4 , " selected" , "") + ">Old to New</option>\n");
				tmpS.Append("<option value=\"5\" " + Common.IIF(OrderByIdx == 5 , " selected" , "") + ">High to Low Rating</option>\n");
				tmpS.Append("<option value=\"6\" " + Common.IIF(OrderByIdx == 6 , " selected" , "") + ">Low to High Rating</option>\n");
				tmpS.Append("</select><br><br>\n");
			}
			if(NumRows > 0)
			{
				tmpS.Append("<TABLE class=\"GreyCell\" width=\"100%\" cellpadding=\"5\" cellspacing=\"0\" border=\"0\">\n");
				tmpS.Append("<tr>\n");
				tmpS.Append("<td align=\"left\" height=\"20\" class=\"GreyCell\">Showing comments " + StartRow.ToString() + "-" + StopRow.ToString() + " of " + NumRows.ToString());
				if(NumPages > 1)
				{
					tmpS.Append(" (");
					if(PageNum > 1)
					{
						tmpS.Append("<a href=\"customerratings.aspx?CustomerID=" + Common.QueryString("CustomerID") + "&OrderBy=" + OrderByIdx.ToString() + "&pagenum=" + (PageNum-1).ToString() + "\">Previous " + PageSize.ToString() + "</a>");
					}
					if(PageNum > 1 && PageNum < NumPages)
					{
						tmpS.Append(" | ");
					}
					if(PageNum < NumPages)
					{
						tmpS.Append("<a href=\"customerratings.aspx?CustomerID=" + Common.QueryString("CustomerID") + "&OrderBy=" + OrderByIdx.ToString() + "&pagenum=" + (PageNum+1).ToString() + "\">Next " + PageSize.ToString() + "</a>");
					}
					tmpS.Append(")");
				}
				tmpS.Append("</td>\n");
				tmpS.Append("<td align=\"right\" class=\"GreyCell\">\n");
				if(NumPages > 1)
				{
					tmpS.Append("Click <a href=\"customerratings.aspx?CustomerID=" + Common.QueryString("CustomerID") + "&show=all\">HERE</a> to see all comments");
				}
				tmpS.Append("</td>\n");
				tmpS.Append("</tr>\n");
				tmpS.Append("</table>\n");
			}
			if(NumRows == 0)
			{
				tmpS.Append("There are no comments by this customer.<br>");
			}
			else
			{
				tmpS.Append("<TABLE width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" bgcolor=\"#FFFFFF\" border=\"0\">\n");
				for(int row = StartRow; row <= StopRow; row++)
				{
					DataRow iRow = ds.Tables[0].Rows[row-1];
					tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
					tmpS.Append("<b>PRODUCT: " + DB.RowField(iRow,"ProductName") + "</b>&nbsp;");
					tmpS.Append(row.ToString() + ". <b>" + Common.IIF(DB.RowField(iRow,"FirstName").Length == 0 , "Anonymous User" , DB.RowField(iRow,"FirstName")) + "</b> " + " on " + Localization.ToNativeShortDateString(DB.RowFieldDateTime(iRow,"DateEntered")) + ", said: " + Common.BuildStarsImage(DB.RowFieldSingle(iRow,"Rating"),_siteID) + "<br>");
					tmpS.Append("<br>");
					tmpS.Append("<a href=\"customerratings.aspx?CustomerID=" + CustomerID.ToString() + "&deleteid=" + DB.RowFieldInt(iRow,"RatingID").ToString() + "\">DELETE COMMENT</a>&nbsp;&nbsp;");
					if(DB.RowFieldBool(iRow,"IsFilthy"))
					{
						tmpS.Append("<a href=\"customerratings.aspx?CustomerID=" + CustomerID.ToString() + "&clearfilthyid=" + DB.RowFieldInt(iRow,"RatingID").ToString() + "\"><b><font color=red>CLEAR FILTHY</font></b></a>&nbsp;&nbsp;");
					}
					tmpS.Append("<br><br>");
					tmpS.Append("<span class=\"CommentText\">\n");
					tmpS.Append(DB.RowField(iRow,"Comments"));
					tmpS.Append("</span><br>\n");
					tmpS.Append("<font face=\"arial,helvetica\" size=\"1\" color=\"#009999\">");
					tmpS.Append("&nbsp;&nbsp;(" + DB.RowFieldInt(iRow,"FoundHelpful").ToString() + " people found " + Common.IIF(CustomerID != DB.RowFieldInt(iRow,"CustomerID") , "this" , "your") + " comment helpful, " + DB.RowFieldInt(iRow,"FoundNotHelpful").ToString() + " did not)");
					tmpS.Append("<hr size=\"1\" color=\"#" + Common.AppConfig("GreyCellColor") + "\">\n");
					tmpS.Append("</td></tr>\n");
				}
				tmpS.Append("</table>\n");
			}
			if(NumRows > 0)
			{
				tmpS.Append("<TABLE class=\"GreyCell\" width=\"100%\" cellpadding=\"5\" cellspacing=\"0\" border=\"0\">\n");
				tmpS.Append("<tr>\n");
				tmpS.Append("<td align=\"left\" height=\"20\" class=\"GreyCell\">Showing comments " + StartRow.ToString() + "-" + StopRow.ToString() + " of " + NumRows.ToString());
				if(NumPages > 1)
				{
					tmpS.Append(" (");
					if(PageNum > 1)
					{
						tmpS.Append("<a href=\"customerratings.aspx?CustomerID=" + Common.QueryString("CustomerID") + "&OrderBy=" + OrderByIdx.ToString() + "&pagenum=" + (PageNum-1).ToString() + "\">Previous " + PageSize.ToString() + "</a>");
					}
					if(PageNum > 1 && PageNum < NumPages)
					{
						tmpS.Append(" | ");
					}
					if(PageNum < NumPages)
					{
						tmpS.Append("<a href=\"customerratings.aspx?CustomerID=" + Common.QueryString("CustomerID") + "&OrderBy=" + OrderByIdx.ToString() + "&pagenum=" + (PageNum+1).ToString() + "\">Next " + PageSize.ToString() + "</a>");
					}
					tmpS.Append(")");
				}
				tmpS.Append("</td>\n");
				tmpS.Append("<td align=\"right\" class=\"GreyCell\">\n");
				if(NumPages > 1)
				{
					tmpS.Append("Click <a href=\"customerratings.aspx?CustomerID=" + Common.QueryString("CustomerID") + "&show=all\">HERE</a> to see all comments");
				}
				tmpS.Append("</td>\n");
				tmpS.Append("</tr>\n");
				tmpS.Append("</table>\n");
			}
			ds.Dispose();

			tmpS.Append("</form");

			// END RATINGS BODY:

			return tmpS.ToString();
		}

		static public String Display(Customer thisCustomer, int ProductID, int CategoryID, int _siteID)
		{
			String ProductName = Common.GetProductName(ProductID);
			
			StringBuilder tmpS = new StringBuilder(50000);
			tmpS.Append("<form name=\"ShowProduct\" action=\"showProduct.aspx\" method=\"GET\">\n");
			tmpS.Append("<input type=\"hidden\" name=\"ProductID\" value=\"" + ProductID.ToString() + "\">");
			tmpS.Append("<input type=\"hidden\" name=\"CategoryID\" value=\"" + CategoryID.ToString() + "\">");

			// PREFIX:
			tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + Common.AppConfig("HeaderBGColor") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			tmpS.Append("<img src=\"skins/Skin_" + _siteID.ToString() + "/images/ratingsexpanded.gif\" border=\"0\"><br>");
			tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + Common.AppConfig("BoxFrameStyle") + "\">\n");
			tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
			// END PREFIX

			// RATINGS BODY:
			String sql = "select * from product  " + DB.GetNoLock() + " left outer join product_stats  " + DB.GetNoLock() + " on product.productid=product_stats.productid where Product.ProductID=" + ProductID.ToString();
			IDataReader rs = DB.GetRS(sql);
			rs.Read();
			int NumRatings = DB.RSFieldInt(rs,"NumRatings");
			int SumRatings = DB.RSFieldInt(rs,"SumRatings");
			rs.Close();

			Single TheAvg = 0.0F;
			if(NumRatings > 0)
			{
				TheAvg = SumRatings/NumRatings;
			}

			int[] ratPercents = new int[6]; // indexes 0-5, but we only use indexes 1-5
			DataSet dsbrk = DB.GetDS("select Productid,rating,count(rating) as N from Rating  " + DB.GetNoLock() + " where Productid=" + ProductID.ToString() + " group by Productid,rating order by rating",false,System.DateTime.Now.AddHours(1));
			foreach(DataRow row in dsbrk.Tables[0].Rows)
			{
				int NN = DB.RowFieldInt(row,"N");
				Single pp = ((Single)NN)/NumRatings;
				int pper = (int)(pp * 100.0);
				ratPercents[DB.RowFieldInt(row,"Rating")] = pper;
			}
			dsbrk.Dispose();
			
			String OrderBy = "Ratings_Average desc, Ratings_Count desc, Name asc";
			String SortDescription = "Helpful to Less Helpful";
			String FilterDescription = String.Empty;
			String FieldSuffix = String.Empty;
			int OrderByIdx = Common.QueryStringUSInt("OrderBy");
			if(OrderByIdx == 0)
			{
				OrderByIdx = 3;
			}
			switch(OrderByIdx)
			{
				case 1:
					SortDescription = "Helpful to Less Helpful";
					OrderBy = "FoundHelpful desc, DateEntered desc";
					break;
				case 2:
					SortDescription = "Less Helpful to Helpful";
					OrderBy = "FoundHelpful asc, DateEntered desc";
					break;
				case 3:
					SortDescription = "New to Old";
					OrderBy = "DateEntered desc";
					break;
				case 4:
					SortDescription = "Old to New";
					OrderBy = "DateEntered asc";
					break;
				case 5:
					SortDescription = "High to Low Rating";
					OrderBy = "Rating desc, DateEntered desc";
					break;
				case 6:
					SortDescription = "Low to High Rating";
					OrderBy = "Rating asc, DateEntered desc";
					break;
			}

			DataSet ds = DB.GetDS("select * from product_comments_view  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString() + " order by " + OrderBy,false,System.DateTime.Now.AddMinutes(1));
			
			int NumRows = ds.Tables[0].Rows.Count;
			int PageSize = Common.AppConfigUSInt("RatingsPageSize");
			int PageNum = Common.QueryStringUSInt("PageNum");
			if(PageNum == 0)
			{
				PageNum = 1;
			}
			if(PageSize == 0)
			{
				PageSize = 10;
			}
			if(Common.QueryString("show") == "all")
			{
				PageSize = 1000000;
				PageNum = 1;
			}
			int NumPages = (int)(NumRows/PageSize) + Common.IIF(NumRows % PageSize == 0 , 0 , 1);
			if(PageNum > NumPages)
			{
				if(NumRows > 0)
				{
					HttpContext.Current.Response.Redirect("showProduct.aspx?ProductID=" + ProductID.ToString() + "&pagenum=" + (PageNum-1).ToString());
				}
			}
			int StartRow = (PageSize*(PageNum-1)) + 1;
			int StopRow = StartRow + PageSize -1 ;
			if(StopRow > NumRows)
			{
				StopRow = NumRows;
			}
			
			tmpS.Append("Product Rating: " + Common.BuildStarsImage(TheAvg,_siteID) + "(" + String.Format("{0:f}", TheAvg) + ")&nbsp;&nbsp;&nbsp;# of Ratings: " + NumRatings.ToString() + "&nbsp;&nbsp;&nbsp;");
			
			String RateLink = "javascript:RateIt(" + ProductID.ToString() + ");";
			int urat = Ratings.GetProductRating(thisCustomer._customerID,ProductID);
			if(urat != 0)
			{
				tmpS.Append("<b>Your Rating: " + urat.ToString() + "</b>&nbsp;&nbsp;<a href=\"" + RateLink + "\">CLICK HERE</a> to change your rating\n");
			}
			else
			{
				if((Common.AppConfigBool("RatingsCanBeDoneByAnons") || !thisCustomer._isAnon) && !Common.IsAdminSite)
				{
					tmpS.Append("<a href=\"" + RateLink + "\"><img align=\"absmiddle\" src=\"skins/skin_" + _siteID.ToString() + "/images/rateit.gif\" border=\"0\"></a>&nbsp;Click <a href=\"" + RateLink + "\">HERE</a> to rate this product");
				}
				else
				{
					tmpS.Append("<b>(Only registered customers can rate)</b>");
				}
			}
			tmpS.Append("<br><br>");
			if(NumRows > 0)
			{
				tmpS.Append("<b>Ratings Breakdown for " + ProductName + "</b><br>\n");
				tmpS.Append("<table width=\"100%\" border=\"0\">\n");
				tmpS.Append("<tr>\n");
				tmpS.Append("<td align=\"right\" width=\"25%\" valign=\"top\">\n");
				tmpS.Append("<br>\n");
				tmpS.Append("	<table cellpadding=\"0\" cellspacing=\"0\" align=\"right\" border=\"0\">\n");
				tmpS.Append("	<tr><td height=\"21\"><b>1</b>&nbsp;-&nbsp;Terrible</td></tr>\n");
				tmpS.Append("	<tr><td height=\"21\"><b>2</b>&nbsp;-&nbsp;Bad</td></tr>\n");
				tmpS.Append("	<tr><td height=\"21\"><b>3</b>&nbsp;-&nbsp;OK</td></tr>\n");
				tmpS.Append("	<tr><td height=\"21\"><b>4</b>&nbsp;-&nbsp;Good</td></tr>\n");
				tmpS.Append("	<tr><td height=\"21\"><b>5</b>&nbsp;-&nbsp;Great</td></tr>\n");
				tmpS.Append("	</table>\n");
				tmpS.Append("</td>\n");
				tmpS.Append("<td width=\"100%\">\n");
				tmpS.Append("	<table border=\"0\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">\n");
				tmpS.Append("	<tr>\n");
				tmpS.Append("	<td width=\"33%\" align=\"left\"><b>0%</b></td>\n");
				tmpS.Append("	<td width=\"33%\" align=\"center\"><b>50%</b></td>\n");
				tmpS.Append("	<td width=\"33%\" align=\"right\"><b>100%</b></td>\n");
				tmpS.Append("	</tr>\n");
				tmpS.Append("	</table>\n");
				tmpS.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"#000000\" width=\"100%\">\n");
				tmpS.Append("<tr>\n");
				tmpS.Append("<td width=\"100%\">\n");
				tmpS.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"1\" width=\"100%\">\n");
				tmpS.Append("<tr>\n");
				tmpS.Append("<td bgcolor=\"#FFFFFF\" width=\"100%\">\n");
				tmpS.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"GreyCell\">\n");

				for(int jj = 1 ; jj <= 5; jj++)
				{
					tmpS.Append("<tr>\n");
					tmpS.Append("<td height=\"21\" width=\"100%\" valign=\"left\">\n");
					if(ratPercents[jj] != 0)
					{
						tmpS.Append("<img src=\"skins/skin_" + _siteID.ToString() + "/images/pollimage.gif\" height=\"15\" width=\"" + ratPercents[jj].ToString() + "%\" border=\"0\" align=\"middle\">\n");
					}
					tmpS.Append("</td>\n");
					tmpS.Append("</tr>\n");
				}
				tmpS.Append("</table>\n");
				tmpS.Append("</td>\n");
				tmpS.Append("</tr>\n");
				tmpS.Append("</table>\n");
				tmpS.Append("</td>\n");
				tmpS.Append("</tr>\n");
				tmpS.Append("</table>\n");
				tmpS.Append("</td>\n");
				tmpS.Append("</tr>\n");
				tmpS.Append("</table>\n");
			}
			if(NumRows > 0)
			{
				tmpS.Append("<br>Sort: " + SortDescription + "&nbsp;&nbsp;RE-SORT COMMENTS: ");
				tmpS.Append("<select size=\"1\" name=\"OrderBy\" onChange=\"document.ShowProduct.submit();\">\n");
				tmpS.Append("<option value=\"1\" " + Common.IIF(OrderByIdx == 1 , " selected" , "") + ">Helpful to Less Helpful</option>\n");
				tmpS.Append("<option value=\"2\" " + Common.IIF(OrderByIdx == 2 , " selected" , "") + ">Less Helpful to Helpful</option>\n");
				tmpS.Append("<option value=\"3\" " + Common.IIF(OrderByIdx == 3 , " selected" , "") + ">New to Old</option>\n");
				tmpS.Append("<option value=\"4\" " + Common.IIF(OrderByIdx == 4 , " selected" , "") + ">Old to New</option>\n");
				tmpS.Append("<option value=\"5\" " + Common.IIF(OrderByIdx == 5 , " selected" , "") + ">High to Low Rating</option>\n");
				tmpS.Append("<option value=\"6\" " + Common.IIF(OrderByIdx == 6 , " selected" , "") + ">Low to High Rating</option>\n");
				tmpS.Append("</select><br><br>\n");
			}
			
			if(NumRows > 0)
			{
				tmpS.Append("<TABLE class=\"GreyCell\" width=\"100%\" cellpadding=\"5\" cellspacing=\"0\" border=\"0\">\n");
				tmpS.Append("<tr>\n");
				tmpS.Append("<td align=\"left\" height=\"20\" class=\"GreyCell\">Showing comments " + StartRow.ToString() + "-" + StopRow.ToString() + " of " + NumRows.ToString());
				if(NumPages > 1)
				{
					tmpS.Append(" (");
					if(PageNum > 1)
					{
						tmpS.Append("<a href=\"showProduct.aspx?ProductID=" + Common.QueryString("ProductID") + "&OrderBy=" + OrderByIdx.ToString() + "&pagenum=" + (PageNum-1).ToString() + "\">Previous " + PageSize.ToString() + "</a>");
					}
					if(PageNum > 1 && PageNum < NumPages)
					{
						tmpS.Append(" | ");
					}
					if(PageNum < NumPages)
					{
						tmpS.Append("<a href=\"showProduct.aspx?ProductID=" + Common.QueryString("ProductID") + "&OrderBy=" + OrderByIdx.ToString() + "&pagenum=" + (PageNum+1).ToString() + "\">Next " + PageSize.ToString() + "</a>");
					}
					tmpS.Append(")");
				}
				tmpS.Append("</td>\n");
				tmpS.Append("<td align=\"right\" class=\"GreyCell\">\n");
				if(NumPages > 1)
				{
					tmpS.Append("Click <a href=\"showProduct.aspx?ProductID=" + Common.QueryString("ProductID") + "&show=all\">HERE</a> to see all comments");
				}
				tmpS.Append("</td>\n");
				tmpS.Append("</tr>\n");
				tmpS.Append("</table>\n");
			}

			if(NumRows == 0)
			{
				tmpS.Append("There are no comments for this product.");
				if(Common.AppConfigBool("RatingsCanBeDoneByAnons") || !thisCustomer._isAnon)
				{
					tmpS.Append(" Click <a href=\"" + RateLink + "\">HERE</a> to be the first one to share your opinion!");
				}
				tmpS.Append("<br>");

			}
			else
			{
				tmpS.Append("<TABLE width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" bgcolor=\"#FFFFFF\" border=\"0\">\n");
				for(int row = StartRow; row <= StopRow; row++)
				{
					DataRow iRow = ds.Tables[0].Rows[row-1];
					tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
					tmpS.Append(row.ToString() + ". <b>" + HttpContext.Current.Server.HtmlEncode(Common.IIF(DB.RowField(iRow,"FirstName").Length == 0 , "Anonymous User" , DB.RowField(iRow,"FirstName"))) + "</b> " + " on " + Localization.ToNativeShortDateString(DB.RowFieldDateTime(iRow,"DateEntered")) + ", said: " + Common.BuildStarsImage(DB.RowFieldSingle(iRow,"Rating"),_siteID) + "<br>");
					tmpS.Append("<br>");
					tmpS.Append("<span class=\"CommentText\">\n");
					tmpS.Append(HttpContext.Current.Server.HtmlEncode(DB.RowField(iRow,"Comments")));
					tmpS.Append("</span><br>\n");
					tmpS.Append("<font face=\"arial,helvetica\" size=\"1\" color=\"#009999\">");
					if(!thisCustomer._isAnon && thisCustomer._customerID != DB.RowFieldInt(iRow,"CustomerID"))
					{
						bool YouHaveVoted = false;
						bool YouVotedHelpful = false;
						IDataReader rs3 = DB.GetRS("Select * from RatingCommentHelpfulness  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString() + " and RatingCustomerID=" + DB.RowFieldInt(iRow,"CustomerID").ToString() + " and VotingCustomerID=" + Common.Session("CustomerID"));
						if(rs3.Read())
						{
							YouHaveVoted = true;
							YouVotedHelpful = DB.RSFieldBool(rs3,"Helpful");
						}
						rs3.Close();
						tmpS.Append("Was this comment helpful? ");
						tmpS.Append("<INPUT TYPE=\"RADIO\" NAME=\"helpful_" + ProductID.ToString() + "_" + DB.RowFieldInt(iRow,"CustomerID").ToString() + "\" onClick=\"return RateComment('" + ProductID.ToString() + "','" + Common.Session("CustomerID") + "','Yes','" + DB.RowFieldInt(iRow,"CustomerID").ToString() + "');\" " + Common.IIF(YouHaveVoted && YouVotedHelpful , " checked " , "") + ">\n");
						tmpS.Append("<FONT face=\"arial,helvetica\" size=1 color=\"#006600\">yes\n");
						tmpS.Append("<INPUT TYPE=\"RADIO\" NAME=\"helpful_" + ProductID.ToString() + "_" + DB.RowFieldInt(iRow,"CustomerID").ToString() + "\" onClick=\"return RateComment('" + ProductID.ToString() + "','" + Common.Session("CustomerID") + "','No','" + DB.RowFieldInt(iRow,"CustomerID").ToString() + "');\" " + Common.IIF(YouHaveVoted && !YouVotedHelpful , " checked " , "") + ">\n");
						tmpS.Append("<FONT face=\"arial,helvetica\" size=1 color=\"#006600\">no\n");
					}

					tmpS.Append("&nbsp;&nbsp;(" + DB.RowFieldInt(iRow,"FoundHelpful").ToString() + " people found " + Common.IIF(thisCustomer._customerID != DB.RowFieldInt(iRow,"CustomerID") , "this" , "your") + " comment helpful, " + DB.RowFieldInt(iRow,"FoundNotHelpful").ToString() + " did not)");
					tmpS.Append("<hr size=\"1\" color=\"#" + Common.AppConfig("GreyCellColor") + "\">\n");
					tmpS.Append("</td></tr>\n");
				}
				tmpS.Append("</table>\n");
			}
			ds.Dispose();

			if(NumRows > 0)
			{
				tmpS.Append("<TABLE class=\"GreyCell\" width=\"100%\" cellpadding=\"5\" cellspacing=\"0\" border=\"0\">\n");
				tmpS.Append("<tr>\n");
				tmpS.Append("<td align=\"left\" height=\"20\" class=\"GreyCell\">Showing comments " + StartRow.ToString() + "-" + StopRow.ToString() + " of " + NumRows.ToString());
				if(NumPages > 1)
				{
					tmpS.Append(" (");
					if(PageNum > 1)
					{
						tmpS.Append("<a href=\"showProduct.aspx?ProductID=" + Common.QueryString("ProductID") + "&OrderBy=" + OrderByIdx.ToString() + "&pagenum=" + (PageNum-1).ToString() + "\">Previous " + PageSize.ToString() + "</a>");
					}
					if(PageNum > 1 && PageNum < NumPages)
					{
						tmpS.Append(" | ");
					}
					if(PageNum < NumPages)
					{
						tmpS.Append("<a href=\"showProduct.aspx?ProductID=" + Common.QueryString("ProductID") + "&OrderBy=" + OrderByIdx.ToString() + "&pagenum=" + (PageNum+1).ToString() + "\">Next " + PageSize.ToString() + "</a>");
					}
					tmpS.Append(")");
				}
				tmpS.Append("</td>\n");
				tmpS.Append("<td align=\"right\" class=\"GreyCell\">\n");
				if(NumPages > 1)
				{
					tmpS.Append("Click <a href=\"showProduct.aspx?ProductID=" + Common.QueryString("ProductID") + "&show=all\">HERE</a> to see all comments");
				}
				tmpS.Append("</td>\n");
				tmpS.Append("</tr>\n");
				tmpS.Append("</table>\n");
			}

			// END RATINGS BODY:


			// POSTFIX
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			tmpS.Append("</td></tr>\n");
			tmpS.Append("</table>\n");
			// END POSTFIX

			tmpS.Append("</form>\n");

			tmpS.Append("<div id=\"RateCommentDiv\" name=\"RateCommentDiv\" style=\"position:absolute; left:0px; top:0px; visibility:" + Common.AppConfig("RatingsCommentFrameVisibility") + "; z-index:2000; \">\n");
			tmpS.Append("<iframe name=\"RateCommentFrm\" id=\"RateCommentFrm\" width=\"400\" height=\"100\" hspace=\"0\" vspace=\"0\" marginheight=\"0\" marginwidth=\"0\" frameborder=\"0\" noresize scrolling=\"yes\" src=\"/empty.htm\"></iframe>\n");
			tmpS.Append("</div>\n");
			tmpS.Append("<script type=\"text/javascript\" language=\"javascript\">\n");
			tmpS.Append("function RateComment(ProductID,MyCustomerID,MyVote,RatersCustomerID)\n");
			tmpS.Append("	{\n");
			tmpS.Append("	RateCommentFrm.location = 'RateComment.aspx?Productid=' + ProductID + '&VotingCustomerID=' + MyCustomerID + '&MyVote=' + MyVote + '&CustomerID=' + RatersCustomerID\n");
			tmpS.Append("	}\n");
			tmpS.Append("	function RateIt(ProductID)\n");
			tmpS.Append("	{\n");
			tmpS.Append("		window.open('rateit.aspx?Productid=' + ProductID + '&refresh=no&returnurl=" + HttpContext.Current.Server.UrlEncode(Common.PageInvocation()) + "','ASPDNSF_ML" + Common.GetRandomNumber(1,100000).ToString() + "','height=450,width=440,top=10,left=20,status=no,toolbar=no,menubar=no,scrollbars=yes,location=no')\n");
			tmpS.Append("	}\n");
			tmpS.Append("</SCRIPT>\n");

			return tmpS.ToString();
		}

		static public String DisplayMatching(Customer thisCustomer,String st, int _siteID)
		{
			// if st, then match on that, else show recent

			StringBuilder tmpS = new StringBuilder(50000);

			int NumDays = Common.AppConfigUSInt("RecentCommentHistoryDays");
			if(NumDays == 0)
			{
				NumDays = 7;
			}

			String OrderBy = "DateEntered desc";
			String SortDescription = "Helpful to Less Helpful";
			String FilterDescription = String.Empty;
			String FieldSuffix = String.Empty;
			int OrderByIdx = Common.QueryStringUSInt("OrderBy");
			if(OrderByIdx == 0)
			{
				OrderByIdx = 3;
			}
			switch(OrderByIdx)
			{
				case 1:
					SortDescription = "Helpful to Less Helpful";
					OrderBy = "FoundHelpful desc, DateEntered desc";
					break;
				case 2:
					SortDescription = "Less Helpful to Helpful";
					OrderBy = "FoundHelpful asc, DateEntered desc";
					break;
				case 3:
					SortDescription = "New to Old";
					OrderBy = "DateEntered desc";
					break;
				case 4:
					SortDescription = "Old to New";
					OrderBy = "DateEntered asc";
					break;
				case 5:
					SortDescription = "High to Low Rating";
					OrderBy = "Rating desc, DateEntered desc";
					break;
				case 6:
					SortDescription = "Low to High Rating";
					OrderBy = "Rating asc, DateEntered desc";
					break;
			}

			tmpS.Append("<form name=\"ShowProduct\" action=\"manageratings.aspx\" method=\"GET\">\n");
			tmpS.Append("<input type=\"hidden\" name=\"SearchTerm\" value=\"" + st + "\">");

			DataSet ds = DB.GetDS("select * from product_comments_view  " + DB.GetNoLock() + " where " + Common.IIF(st.Length != 0 , " Comments like " + DB.SQuote("%" + st + "%") , " DateEntered>=" + DB.DateQuote(Localization.ToNativeShortDateString(System.DateTime.Now.AddDays(-NumDays)))) + " order by " + OrderBy,false,System.DateTime.Now.AddMinutes(1));
			
			int NumRows = ds.Tables[0].Rows.Count;
			int PageSize = 1000000;
			int PageNum = Common.QueryStringUSInt("PageNum");
			if(PageNum == 0)
			{
				PageNum = 1;
			}
			if(Common.QueryString("show") == "all")
			{
				PageSize = 1000000;
				PageNum = 1;
			}
			int NumPages = (int)(NumRows/PageSize) + Common.IIF(NumRows % PageSize == 0 , 0 , 1);
			if(PageNum > NumPages)
			{
				if(NumRows > 0)
				{
					HttpContext.Current.Response.Redirect("manageratings.aspx?st=" + st + "&pagenum=" + (PageNum-1).ToString());
				}
			}
			int StartRow = (PageSize*(PageNum-1)) + 1;
			int StopRow = StartRow + PageSize -1 ;
			if(StopRow > NumRows)
			{
				StopRow = NumRows;
			}
			

			if(NumRows > 0)
			{
				tmpS.Append("<br>Sort: " + SortDescription + "&nbsp;&nbsp;RE-SORT COMMENTS: ");
				tmpS.Append("<select size=\"1\" name=\"OrderBy\" onChange=\"document.ShowProduct.submit();\">\n");
				tmpS.Append("<option value=\"1\" " + Common.IIF(OrderByIdx == 1 , " selected" , "") + ">Helpful to Less Helpful</option>\n");
				tmpS.Append("<option value=\"2\" " + Common.IIF(OrderByIdx == 2 , " selected" , "") + ">Less Helpful to Helpful</option>\n");
				tmpS.Append("<option value=\"3\" " + Common.IIF(OrderByIdx == 3 , " selected" , "") + ">New to Old</option>\n");
				tmpS.Append("<option value=\"4\" " + Common.IIF(OrderByIdx == 4 , " selected" , "") + ">Old to New</option>\n");
				tmpS.Append("<option value=\"5\" " + Common.IIF(OrderByIdx == 5 , " selected" , "") + ">High to Low Rating</option>\n");
				tmpS.Append("<option value=\"6\" " + Common.IIF(OrderByIdx == 6 , " selected" , "") + ">Low to High Rating</option>\n");
				tmpS.Append("</select><br><br>\n");
			}
			
			if(NumRows > 0)
			{
				tmpS.Append("<TABLE class=\"GreyCell\" width=\"100%\" cellpadding=\"5\" cellspacing=\"0\" border=\"0\">\n");
				tmpS.Append("<tr>\n");
				tmpS.Append("<td align=\"left\" height=\"20\" class=\"GreyCell\">Showing comments " + StartRow.ToString() + "-" + StopRow.ToString() + " of " + NumRows.ToString());
				if(NumPages > 1)
				{
					tmpS.Append(" (");
					if(PageNum > 1)
					{
						tmpS.Append("<a href=\"manageratings.aspx?st=" + st + "&OrderBy=" + OrderByIdx.ToString() + "&pagenum=" + (PageNum-1).ToString() + "\">Previous " + PageSize.ToString() + "</a>");
					}
					if(PageNum > 1 && PageNum < NumPages)
					{
						tmpS.Append(" | ");
					}
					if(PageNum < NumPages)
					{
						tmpS.Append("<a href=\"manageratings.aspx?st=" + st + "&OrderBy=" + OrderByIdx.ToString() + "&pagenum=" + (PageNum+1).ToString() + "\">Next " + PageSize.ToString() + "</a>");
					}
					tmpS.Append(")");
				}
				tmpS.Append("</td>\n");
				tmpS.Append("<td align=\"right\" class=\"GreyCell\">\n");
				if(NumPages > 1)
				{
					tmpS.Append("Click <a href=\"manageratings.aspx?st=" + st + "&show=all\">HERE</a> to see all comments");
				}
				tmpS.Append("</td>\n");
				tmpS.Append("</tr>\n");
				tmpS.Append("</table>\n");
			}
			if(NumRows == 0)
			{
				if(st.Length != 0)
				{
					tmpS.Append("There are no comments matching " + DB.SQuote(st) + ".<br>");
				}
				else
				{
					tmpS.Append("There are no comments in the last " + NumDays.ToString() + " days.<br>");
				}
			}
			else
			{
				tmpS.Append("<TABLE width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" bgcolor=\"#FFFFFF\" border=\"0\">\n");
				for(int row = StartRow; row <= StopRow; row++)
				{
					DataRow iRow = ds.Tables[0].Rows[row-1];
					tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
					tmpS.Append("<b>PRODUCT: " + DB.RowField(iRow,"ProductName") + "</b>&nbsp;");
					tmpS.Append(row.ToString() + ". <b><a href=\"customerratings.aspx?customerid=" + DB.RowFieldInt(iRow,"CustomerID").ToString() + "\">" + Common.IIF(DB.RowField(iRow,"FirstName").Length == 0 , "Anonymous User" , (DB.RowField(iRow,"FirstName") + " " + DB.RowField(iRow,"LastName")).Trim()) + "</a></b> " + " on " + ds.Tables[0].Rows[row-1]["DateEntered"].ToString() + ", said: " + Common.BuildStarsImage(DB.RowFieldSingle(iRow,"Rating"),_siteID) + "<br>");
					tmpS.Append("<br>");
					tmpS.Append("<a href=\"manageratings.aspx?st=" + st + "&deleteid=" + DB.RowFieldInt(iRow,"RatingID").ToString() + "\">DELETE COMMENT</a>&nbsp;&nbsp;");
					if(DB.RowFieldBool(iRow,"IsFilthy"))
					{
						tmpS.Append("<a href=\"manageratings.aspx?CustomerID=" + DB.RowFieldInt(iRow,"CustomerID").ToString() + "&clearfilthyid=" + DB.RowFieldInt(iRow,"RatingID").ToString()+ "\"><b><font color=red>CLEAR FILTHY</font></b></a>&nbsp;&nbsp;");
					}
					tmpS.Append("<br><br>");
					tmpS.Append("<span class=\"CommentText\">\n");
					tmpS.Append(DB.RowField(iRow,"Comments"));
					tmpS.Append("</span><br>\n");
					tmpS.Append("<font face=\"arial,helvetica\" size=\"1\" color=\"#009999\">");
					tmpS.Append("&nbsp;&nbsp;(" + DB.RowFieldInt(iRow,"FoundHelpful").ToString() + " people found " + Common.IIF(thisCustomer._customerID != DB.RowFieldInt(iRow,"CustomerID") , "this" , "your") + " comment helpful, " + DB.RowFieldInt(iRow,"FoundNotHelpful").ToString() + " did not)");
					tmpS.Append("<hr size=\"1\" color=\"#" + Common.AppConfig("GreyCellColor") + "\">\n");
					tmpS.Append("</td></tr>\n");
				}
				tmpS.Append("</table>\n");
			}
			if(NumRows > 0)
			{
				tmpS.Append("<TABLE class=\"GreyCell\" width=\"100%\" cellpadding=\"5\" cellspacing=\"0\" border=\"0\">\n");
				tmpS.Append("<tr>\n");
				tmpS.Append("<td align=\"left\" height=\"20\" class=\"GreyCell\">Showing comments " + StartRow.ToString() + "-" + StopRow.ToString() + " of " + NumRows.ToString());
				if(NumPages > 1)
				{
					tmpS.Append(" (");
					if(PageNum > 1)
					{
						tmpS.Append("<a href=\"manageratings.aspx?st=" + st + "&OrderBy=" + OrderByIdx.ToString() + "&pagenum=" + (PageNum-1).ToString() + "\">Previous " + PageSize.ToString() + "</a>");
					}
					if(PageNum > 1 && PageNum < NumPages)
					{
						tmpS.Append(" | ");
					}
					if(PageNum < NumPages)
					{
						tmpS.Append("<a href=\"manageratings.aspx?st=" + st + "&OrderBy=" + OrderByIdx.ToString() + "&pagenum=" + (PageNum+1).ToString() + "\">Next " + PageSize.ToString() + "</a>");
					}
					tmpS.Append(")");
				}
				tmpS.Append("</td>\n");
				tmpS.Append("<td align=\"right\" class=\"GreyCell\">\n");
				if(NumPages > 1)
				{
					tmpS.Append("Click <a href=\"manageratings.aspx?st=" + st + "&show=all\">HERE</a> to see all comments");
				}
				tmpS.Append("</td>\n");
				tmpS.Append("</tr>\n");
				tmpS.Append("</table>\n");
			}
			ds.Dispose();

			tmpS.Append("</form");

			// END RATINGS BODY:

			return tmpS.ToString();
		}

		static public String DisplayFilthy(Customer thisCustomer, int _siteID)
		{
			StringBuilder tmpS = new StringBuilder(50000);

			String OrderBy = "DateEntered desc";
			String SortDescription = "Helpful to Less Helpful";
			String FilterDescription = String.Empty;
			String FieldSuffix = String.Empty;
			int OrderByIdx = Common.QueryStringUSInt("OrderBy");
			if(OrderByIdx == 0)
			{
				OrderByIdx = 3;
			}
			switch(OrderByIdx)
			{
				case 1:
					SortDescription = "Helpful to Less Helpful";
					OrderBy = "FoundHelpful desc, DateEntered desc";
					break;
				case 2:
					SortDescription = "Less Helpful to Helpful";
					OrderBy = "FoundHelpful asc, DateEntered desc";
					break;
				case 3:
					SortDescription = "New to Old";
					OrderBy = "DateEntered desc";
					break;
				case 4:
					SortDescription = "Old to New";
					OrderBy = "DateEntered asc";
					break;
				case 5:
					SortDescription = "High to Low Rating";
					OrderBy = "Rating desc, DateEntered desc";
					break;
				case 6:
					SortDescription = "Low to High Rating";
					OrderBy = "Rating asc, DateEntered desc";
					break;
			}

			tmpS.Append("<form name=\"ShowProduct\" action=\"manageratings.aspx\" method=\"GET\">\n");
			tmpS.Append("<input type=\"hidden\" name=\"Filthy\" value=\"1\">");

			DataSet ds = DB.GetDS("select * from product_comments_view  " + DB.GetNoLock() + " where IsFilthy<>0 order by " + OrderBy,false,System.DateTime.Now.AddMinutes(1));
			
			int NumRows = ds.Tables[0].Rows.Count;
			int PageSize = 1000000;
			int PageNum = Common.QueryStringUSInt("PageNum");
			if(PageNum == 0)
			{
				PageNum = 1;
			}
			if(Common.QueryString("show") == "all")
			{
				PageSize = 1000000;
				PageNum = 1;
			}
			int NumPages = (int)(NumRows/PageSize) + Common.IIF(NumRows % PageSize == 0 , 0 , 1);
			if(PageNum > NumPages)
			{
				if(NumRows > 0)
				{
					HttpContext.Current.Response.Redirect("manageratings.aspx?filthy=1&pagenum=" + (PageNum-1).ToString());
				}
			}
			int StartRow = (PageSize*(PageNum-1)) + 1;
			int StopRow = StartRow + PageSize -1 ;
			if(StopRow > NumRows)
			{
				StopRow = NumRows;
			}
			

			if(NumRows > 0)
			{
				tmpS.Append("<br>Sort: " + SortDescription + "&nbsp;&nbsp;RE-SORT COMMENTS: ");
				tmpS.Append("<select size=\"1\" name=\"OrderBy\" onChange=\"document.ShowProduct.submit();\">\n");
				tmpS.Append("<option value=\"1\" " + Common.IIF(OrderByIdx == 1 , " selected" , "") + ">Helpful to Less Helpful</option>\n");
				tmpS.Append("<option value=\"2\" " + Common.IIF(OrderByIdx == 2 , " selected" , "") + ">Less Helpful to Helpful</option>\n");
				tmpS.Append("<option value=\"3\" " + Common.IIF(OrderByIdx == 3 , " selected" , "") + ">New to Old</option>\n");
				tmpS.Append("<option value=\"4\" " + Common.IIF(OrderByIdx == 4 , " selected" , "") + ">Old to New</option>\n");
				tmpS.Append("<option value=\"5\" " + Common.IIF(OrderByIdx == 5 , " selected" , "") + ">High to Low Rating</option>\n");
				tmpS.Append("<option value=\"6\" " + Common.IIF(OrderByIdx == 6 , " selected" , "") + ">Low to High Rating</option>\n");
				tmpS.Append("</select><br><br>\n");
			}
			
			if(NumRows > 0)
			{
				tmpS.Append("<TABLE class=\"GreyCell\" width=\"100%\" cellpadding=\"5\" cellspacing=\"0\" border=\"0\">\n");
				tmpS.Append("<tr>\n");
				tmpS.Append("<td align=\"left\" height=\"20\" class=\"GreyCell\">Showing comments " + StartRow.ToString() + "-" + StopRow.ToString() + " of " + NumRows.ToString());
				if(NumPages > 1)
				{
					tmpS.Append(" (");
					if(PageNum > 1)
					{
						tmpS.Append("<a href=\"manageratings.aspx?filthy=1&OrderBy=" + OrderByIdx.ToString() + "&pagenum=" + (PageNum-1).ToString() + "\">Previous " + PageSize.ToString() + "</a>");
					}
					if(PageNum > 1 && PageNum < NumPages)
					{
						tmpS.Append(" | ");
					}
					if(PageNum < NumPages)
					{
						tmpS.Append("<a href=\"manageratings.aspx?filthy=1&OrderBy=" + OrderByIdx.ToString() + "&pagenum=" + (PageNum+1).ToString() + "\">Next " + PageSize.ToString() + "</a>");
					}
					tmpS.Append(")");
				}
				tmpS.Append("</td>\n");
				tmpS.Append("<td align=\"right\" class=\"GreyCell\">\n");
				if(NumPages > 1)
				{
					tmpS.Append("Click <a href=\"manageratings.aspx?filthy=1&show=all\">HERE</a> to see all comments");
				}
				tmpS.Append("</td>\n");
				tmpS.Append("</tr>\n");
				tmpS.Append("</table>\n");
			}
			if(NumRows == 0)
			{
				tmpS.Append("There are no filthy comments.<br>");
			}
			else
			{
				tmpS.Append("<TABLE width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" bgcolor=\"#FFFFFF\" border=\"0\">\n");
				for(int row = StartRow; row <= StopRow; row++)
				{
					DataRow iRow = ds.Tables[0].Rows[row-1];
					tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
					tmpS.Append("<b>PRODUCT: " + DB.RowField(iRow,"ProductName") + "</b>&nbsp;");
					tmpS.Append(row.ToString() + ". <b><a href=\"customerratings.aspx?customerid=" + DB.RowFieldInt(iRow,"CustomerID").ToString() + "\">" + Common.IIF(DB.RowField(iRow,"FirstName").Length == 0 , "Anonymous User" , (DB.RowField(iRow,"FirstName") + " " + DB.RowField(iRow,"LastName")).Trim()) + "</a></b> " + " on " + Localization.ToNativeShortDateString(DB.RowFieldDateTime(iRow,"DateEntered")) + ", said: " + Common.BuildStarsImage(DB.RowFieldSingle(iRow,"Rating"),_siteID) + "<br>");
					tmpS.Append("<br>");
					tmpS.Append("<a href=\"manageratings.aspx?filthy=1&deleteid=" + DB.RowFieldInt(iRow,"RatingID").ToString() + "\">DELETE COMMENT</a>&nbsp;&nbsp;");
					if(DB.RowFieldBool(iRow,"IsFilthy"))
					{
						tmpS.Append("<a href=\"manageratings.aspx?CustomerID=" + DB.RowFieldInt(iRow,"CustomerID").ToString() + "&clearfilthyid=" + DB.RowFieldInt(iRow,"RatingID").ToString() + "\"><b><font color=red>CLEAR FILTHY</font></b></a>&nbsp;&nbsp;");
					}
					tmpS.Append("<br><br>");
					tmpS.Append("<span class=\"CommentText\">\n");
					tmpS.Append(DB.RowField(iRow,"Comments"));
					tmpS.Append("</span><br>\n");
					tmpS.Append("<font face=\"arial,helvetica\" size=\"1\" color=\"#009999\">");
					tmpS.Append("&nbsp;&nbsp;(" + DB.RowFieldInt(iRow,"FoundHelpful").ToString() + " people found " + Common.IIF(thisCustomer._customerID != DB.RowFieldInt(iRow,"CustomerID") , "this" , "your") + " comment helpful, " + DB.RowFieldInt(iRow,"FoundNotHelpful").ToString() + " did not)");
					tmpS.Append("<hr size=\"1\" color=\"#" + Common.AppConfig("GreyCellColor") + "\">\n");
					tmpS.Append("</td></tr>\n");
				}
				tmpS.Append("</table>\n");
			}
			if(NumRows > 0)
			{
				tmpS.Append("<TABLE class=\"GreyCell\" width=\"100%\" cellpadding=\"5\" cellspacing=\"0\" border=\"0\">\n");
				tmpS.Append("<tr>\n");
				tmpS.Append("<td align=\"left\" height=\"20\" class=\"GreyCell\">Showing comments " + StartRow.ToString() + "-" + StopRow.ToString() + " of " + NumRows.ToString());
				if(NumPages > 1)
				{
					tmpS.Append(" (");
					if(PageNum > 1)
					{
						tmpS.Append("<a href=\"manageratings.aspx?filthy=1&OrderBy=" + OrderByIdx.ToString() + "&pagenum=" + (PageNum-1).ToString() + "\">Previous " + PageSize.ToString() + "</a>");
					}
					if(PageNum > 1 && PageNum < NumPages)
					{
						tmpS.Append(" | ");
					}
					if(PageNum < NumPages)
					{
						tmpS.Append("<a href=\"manageratings.aspx?filthy=1&OrderBy=" + OrderByIdx.ToString() + "&pagenum=" + (PageNum+1).ToString() + "\">Next " + PageSize.ToString() + "</a>");
					}
					tmpS.Append(")");
				}
				tmpS.Append("</td>\n");
				tmpS.Append("<td align=\"right\" class=\"GreyCell\">\n");
				if(NumPages > 1)
				{
					tmpS.Append("Click <a href=\"manageratings.aspx?filthy=1&show=all\">HERE</a> to see all comments");
				}
				tmpS.Append("</td>\n");
				tmpS.Append("</tr>\n");
				tmpS.Append("</table>\n");
			}
			ds.Dispose();

			tmpS.Append("</form");

			// END RATINGS BODY:

			return tmpS.ToString();
		}

	}
}
