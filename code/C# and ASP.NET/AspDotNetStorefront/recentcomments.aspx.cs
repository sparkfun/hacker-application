// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for recentcomments.
	/// </summary>
	public class recentcomments : SkinBase
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "<span class=\"BreadCrumbText\"><a HREF=\"default.aspx\" class=\"BreadCrumbText\">AspDotNetStorefront.com</a> &gt; Recent Comments</span>";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			int CategoryID = Common.QueryStringUSInt("CategoryID"); // used only if creating a new list for a specific category
			String OrderBy = "Ratings_Average desc, Ratings_Count desc, Name asc";
			String SortDescription = "Ratings, High to Low";
			switch(Common.QueryString("OrderBy"))
			{
				case "":
				case "0":
					OrderBy = "DateEntered desc";
					SortDescription = "New to Old";
					break;
				case "1":
					OrderBy = "DateEntered asc";
					SortDescription = "Old to New";
					break;
				case "2":
					OrderBy = "Rating desc, DateEntered desc";
					SortDescription = "High to Low Rating";
					break;
				case "3":
					OrderBy = "Rating asc, DateEntered desc";
					SortDescription = "Low to High Rating";
					break;
			}


			DataSet ds = DB.GetDS("select * from product_comments_view  " + DB.GetNoLock() + " where DateEntered>=" + DB.DateQuote(Localization.ToNativeShortDateString(System.DateTime.Now.AddDays(-1)) + " 12:00:00.000 AM") + " order by " + OrderBy,true,System.DateTime.Now.AddMinutes(15));
			
			int NumRows = ds.Tables[0].Rows.Count;
			int PageSize = Common.AppConfigUSInt("ShowCommentsPageSize");
			if(PageSize == 0)
			{
				PageSize = 50;
			}
			int PageNum = Common.QueryStringUSInt("PageNum");
			if(PageNum == 0)
			{
				PageNum = 1;
			}
			int NumPages = (NumRows/PageSize) + Common.IIF(NumRows % PageSize == 0 , 0 , 1);
			if(PageNum > NumPages)
			{
				if(NumRows > 0)
				{
					Response.Redirect("recentcomments.aspx?pagenum=" + (PageNum-1).ToString());
				}
			}
			int StartRow = (PageSize*(PageNum-1)) + 1;
			int StopRow = StartRow + PageSize -1 ;
			if(StopRow > NumRows)
			{
				StopRow = NumRows;
			}

			// ------------------------

			writer.Write("<form name=\"recentcomments\" action=\"recentcomments.aspx\" method=\"GET\">\n");

			writer.Write("<table BORDER=\"0\" WIDTH=\"770\" CELLSPACING=\"5\" CELLPADDING=\"0\" ALIGN=\"CENTER\">\n");
			writer.Write("<tr>\n");
			writer.Write("<td width=\"100%\" valign=\"top\" align=\"center\">\n");
			writer.Write("<table border=\"0\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\">\n");
			writer.Write("<tr>\n");
			writer.Write("<td width=\"50%\">\n");
			writer.Write("<table border=\"0\" width=\"100%\" height=\"110\" cellspacing=\"0\" cellpadding=\"5\" bgcolor=\"#FFFFCC\">\n");
			writer.Write("<tr>\n");
			writer.Write("<td width=\"100%\">\n");
			writer.Write("<span class=\"CommentHeaderText\"><FONT color=\"#000000\">Recent Comments</b></font></span>\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("<tr>\n");
			writer.Write("<td width=\"100%\">\n");
			writer.Write("<FONT color=\"#FF0000\">Sort:</font><FONT color=\"#000000\">&nbsp;&nbsp;" + SortDescription + "</font>\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("<tr>\n");
			writer.Write("<td width=\"100%\" valign=\"middle\">\n");
			writer.Write("<FONT color=\"#000000\"><b>Re-Sort List:</b>&nbsp;&nbsp;</font>\n");
			writer.Write("<select size=\"1\" name=\"OrderBy\" onChange=\"document.recentcomments.submit();\">\n");
			writer.Write("<option value=\"0\" " + Common.IIF(Common.QueryString("OrderBy")=="" || Common.QueryString("OrderBy")=="0" , " selected" , "") + ">New to Old</option>\n");
			writer.Write("<option value=\"1\" " + Common.IIF(Common.QueryString("OrderBy")=="1" , " selected" , "") + ">Old to New</option>\n");
			writer.Write("<option value=\"2\" " + Common.IIF(Common.QueryString("OrderBy")=="2" , " selected" , "") + ">High to Low Rating</option>\n");
			writer.Write("<option value=\"3\" " + Common.IIF(Common.QueryString("OrderBy")=="3" , " selected" , "") + ">Low to High Rating</option>\n");
			writer.Write("</select>\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("</td>\n");
			writer.Write("<td width=\"1%\" align=\"Center\">&nbsp;\n");
			writer.Write("</td>\n");
			writer.Write("<td width=\"50%\" valign=\"top\">\n");
			writer.Write("<table border=\"0\" width=\"100%\" bgcolor=\"#009999\" cellspacing=\"0\" cellpadding=\"0\">\n");
			writer.Write("<tr>\n");
			writer.Write("<td width=\"100%\">\n");
			writer.Write("<table border=\"0\" width=\"100%\" cellspacing=\"1\" cellpadding=\"9\">\n");
			writer.Write("<tr>\n");
			writer.Write("<td colspan=\"2\" bgcolor=\"#009999\" align=\"center\" height=\"30\">\n");
			writer.Write("<FONT color=\"#FFFFFF\"><b>Isolate Comments by Category</b></font>\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("<tr>\n");
			writer.Write("<td width=\"50%\" bgcolor=\"#FFFFFF\" valign=\"Middle\" align=\"center\">\n");
			writer.Write("<FONT color=\"#000000\">Use this drop-down menu to isolate the comments this Customer left in different categories.</font>\n");
			writer.Write("</td>\n");
			writer.Write("<td width=\"50%\" bgcolor=\"#FFFFFF\" valign=\"Middle\">\n");
			writer.Write("<table border=\"0\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\">\n");
			writer.Write("<tr>\n");
			writer.Write("<td width=\"40%\" bgcolor=\"#FFFFFF\" align=\"right\">\n");
			writer.Write("<FONT color=\"#000000\">Category:&nbsp;</font>\n");
			writer.Write("</td>\n");
			writer.Write("<td width=\"40%\" bgcolor=\"#FFFFFF\">\n");
			writer.Write("<select size=\"1\" name=\"CategoryID\" onChange=\"document.recentcomments.submit();\">\n");
			writer.Write("<OPTION VALUE=\"\" " + Common.IIF(CategoryID==0 , " selected " , "") + ">All Categories</option>\n");
			DataSet dsst = DB.GetDS("select * from Category  " + DB.GetNoLock() + " order by displayorder,Name",true,System.DateTime.Now.AddHours(3));
			foreach(DataRow row in dsst.Tables[0].Rows)
			{
				writer.Write("<option value=\"" + DB.RowFieldInt(row,"CategoryID").ToString() + "\"");
				if(DB.RowFieldInt(row,"CategoryID") == CategoryID )
				{
					writer.Write(" selected");
				}
				writer.Write(">" + DB.RowField(row,"Name") + "</option>");
			}
			dsst.Dispose();
			writer.Write("</select>\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			//			writer.Write("<tr>\n");
			//			writer.Write("<td colspan=\"3\">\n");
			//			writer.Write("<img src=\"images/spacer.gif\" height=10 hspace=0 vspace=0 border=0>\n");
			//			writer.Write("</td>\n");
			//			writer.Write("</tr>\n");
			writer.Write("</table>\n");

			writer.Write("<table width=\"100%\" bgcolor=\"#CCCCCC\" border=\"0\" cellpadding=\"5\" cellspacing=\"0\">\n");
			writer.Write("<tr>\n");
			writer.Write("<td align=\"left\" width=\"60%\" height=\"25\" bgcolor=\"#FFFFFF\">\n");
			writer.Write("Showing comments ");
			writer.Write(StartRow);
			writer.Write("-");
			writer.Write(StopRow);
			writer.Write(" of ");
			writer.Write(NumRows);
			if(NumPages > 1)
			{
				writer.Write(" (");
				if(PageNum > 1)
				{
					writer.Write("<a href=\"recentcomments.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + (PageNum-1).ToString() + "\">Previous " + PageSize.ToString() + "</a>");
				}
				if(PageNum > 1 && PageNum < NumPages)
				{
					writer.Write(" | ");
				}
				if(PageNum < NumPages)
				{
					writer.Write("<a href=\"recentcomments.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + (PageNum+1).ToString() + "\">Next " + PageSize.ToString() + "</a>");
				}
				writer.Write(")");
			}
			writer.Write("</TD>\n");
			writer.Write("<td align=\"right\" width=\"40%\" height=\"25\" bgcolor=\"#FFFFFF\">\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");

			writer.Write("<table border=\"0\" width=\"100%\" bgcolor=\"#CCCCCC\" cellspacing=\"0\" cellpadding=\"1\">\n");
			writer.Write("<tr>\n");
			writer.Write("<TD>\n");
			writer.Write("<table border=\"0\" width=\"100%\" bgcolor=\"#FFFFFF\" cellpadding=\"5\" cellspacing=\"1\">\n");
			writer.Write("<tr>\n");
			writer.Write("<td align=\"center\" width=\"10%\" height=\"25\" bgcolor=\"#CCCCCC\">\n");
			writer.Write("<b>\n");
			writer.Write("<FONT color=\"#000000\">DATE LEFT</font>\n");
			writer.Write("</b>\n");
			writer.Write("</td>\n");
			writer.Write("<td align=\"center\" width=\"25%\" height=\"25\" bgcolor=\"#CCCCCC\">\n");
			writer.Write("<b>\n");
			writer.Write("<FONT color=\"#000000\">Product &amp; RATING</font>\n");
			writer.Write("</b>\n");
			writer.Write("</td>\n");
			writer.Write("<td align=\"center\" width=\"65%\" height=\"25\" bgcolor=\"#CCCCCC\">\n");
			writer.Write("<b>\n");
			writer.Write("<FONT color=\"#000000\">COMMENT</font>\n");
			writer.Write("</b>\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("\n");

			if(NumRows == 0)
			{
				writer.Write("<p>No comments to display</p>\n");
			}
			else
			{
				writer.Write("<table border=\"0\" width=\"100%\" bgcolor=\"#CCCCCC\" cellpadding=\"5\" cellspacing=\"1\">\n");
				for(int row = StartRow; row <= StopRow; row++)
				{
					DataRow iRow = ds.Tables[0].Rows[row-1];
					int ProductID = DB.RowFieldInt(iRow,"ProductID");
					String ProductName = DB.RowField(iRow,"ProductName");
					String ProductSEName = DB.RowField(iRow,"ProductSEName");

					writer.Write("<TR>\n");
					writer.Write("<TD valign=\"top\" align=\"center\" width=\"10%\" bgcolor=\"#FFFFFF\">\n");
					writer.Write("<FONT color=\"#000000\">"+ Localization.ToNativeShortDateString(DB.RowFieldDateTime(iRow,"DateEntered")) + "</font>\n");
					writer.Write("</TD>\n");
					writer.Write("<TD valign=\"top\"  align=\"left\" width=\"25%\" bgcolor=\"#FFFFFF\">\n");
					writer.Write("<a HREF=\"" + SE.MakeProductLink(ProductID,ProductSEName) + "\">\n");
					writer.Write("<FONT color=\"#0000FF\"><b>" + ProductName + "</b></font>");
					writer.Write("</a>\n");
					writer.Write("<br>\n");


					writer.Write("<br>\n");
					writer.Write(Common.BuildStarsImage(DB.RowFieldSingle(iRow,"Rating"),_siteID));
					writer.Write("<a href=\"javascript:RateIt(" + ProductID.ToString() + ",'" + DB.RowFieldGUID(iRow,"ProductGUID") + "');\">\n");
					int urat = Ratings.GetProductRating(thisCustomer._customerID,(int)ProductID);
					if(urat != 0)
					{
						writer.Write("<br><small>Your Rating: " + urat.ToString() + "</small>\n");
					}
					else
					{
						writer.Write("<br><img src=\"skins/skin_" + _siteID.ToString() + "/images/rateit.gif\" border=\"0\" width=\"46\" height=\"15\">\n");
					}
					writer.Write("</a>");
					writer.Write("</TD>\n");
					writer.Write("<TD valign=\"top\"  align=\"left\" width=\"65%\" bgcolor=\"#FFFFFF\">\n");
					writer.Write("<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\n");
					writer.Write("<tr>\n");
					writer.Write("<td>\n");
					writer.Write("<span class=\"CommentText\">" + DB.RowField(iRow,"Comments") + "<br> - <small><i>Rated By:</i>&nbsp;<a href=\"Customer.aspx?RI=" + DB.RowFieldInt(iRow,"CustomerID").ToString() + "\">" + DB.RowField(iRow,"RatingCustomerName") + "</a>&nbsp;<a href=\"Customer.aspx?RI=" + DB.RowFieldInt(iRow,"CustomerID").ToString() + "\">" + "</a></small></span>\n");
					writer.Write("</td>\n");
					writer.Write("</tr>\n");
					writer.Write("<tr>\n");
					writer.Write("<td height=\"5\"> \n");
					writer.Write("</td>\n");
					writer.Write("</tr>\n");
					writer.Write("<tr>\n");
					writer.Write("<td>\n");

					writer.Write("<font face=\"arial,helvetica\" size=\"1\" color=\"#009999\">");

					if(!thisCustomer._isAnon && thisCustomer._customerID != DB.RowFieldInt(iRow,"CustomerID"))
					{
						bool YouHaveVoted = false;
						bool YouVotedHelpful = false;
						IDataReader rs3 = DB.GetRS("Select * from CommentHelpfulness  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString() + " and RatingCustomerID=" + DB.RowFieldInt(iRow,"CustomerID").ToString() + " and VotingCustomerID=" + thisCustomer._customerID.ToString());
						if(rs3.Read())
						{
							YouHaveVoted = true;
							YouVotedHelpful = DB.RSFieldBool(rs3,"Helpful");
						}
						rs3.Close();
						writer.Write("Was this comment helpful? ");
						writer.Write("<INPUT TYPE=\"RADIO\" NAME=\"helpful_" + ProductID.ToString() + "_" + DB.RowFieldInt(iRow,"CustomerID").ToString() + "\" onClick=\"return RateComment('" + ProductID.ToString() + "','" + DB.RowFieldGUID(iRow,"ProductGUID") + "','" + thisCustomer._customerID.ToString() + "','Yes','" + DB.RowFieldInt(iRow,"CustomerID").ToString() + "');\" " + Common.IIF(YouHaveVoted && YouVotedHelpful , " checked " , "") + ">\n");
						writer.Write("<FONT face=\"arial,helvetica\" size=1 color=\"#006600\">yes</FONT>\n");
						writer.Write("<INPUT TYPE=\"RADIO\" NAME=\"helpful_" + ProductID.ToString() + "_" + DB.RowFieldInt(iRow,"CustomerID").ToString() + "\" onClick=\"return RateComment('" + ProductID.ToString() + "','" + DB.RowFieldGUID(iRow,"ProductGUID") + "','" + thisCustomer._customerID.ToString() + "','No','" + DB.RowFieldInt(iRow,"CustomerID").ToString() + "');\" " + Common.IIF(YouHaveVoted && !YouVotedHelpful , " checked " , "") + ">\n");
						writer.Write("<FONT face=\"arial,helvetica\" size=1 color=\"#006600\">no</FONT>\n");
					}

					writer.Write("&nbsp;&nbsp;(" + DB.RowFieldInt(iRow,"FoundHelpful").ToString() + " people found " + Common.IIF(thisCustomer._customerID != DB.RowFieldInt(iRow,"CustomerID") , "this" , "your") + " comment helpful, " + DB.RowFieldInt(iRow,"FoundNotHelpful").ToString() + " did not)</font>");
					
					writer.Write("</TD>\n");
					writer.Write("</TR>\n");
					writer.Write("</TABLE>\n");
					writer.Write("</TD>\n");
					writer.Write("</TR>\n");
				}
				writer.Write("</TABLE>\n");
			}

			writer.Write("<table width=\"100%\" bgcolor=\"#CCCCCC\" border=\"0\" cellpadding=\"5\" cellspacing=\"0\">\n");
			writer.Write("<tr>\n");
			writer.Write("<td align=\"left\" width=\"60%\" height=\"25\" bgcolor=\"#CCCCCC\">\n");
			writer.Write("Showing comments ");
			writer.Write(StartRow);
			writer.Write("-");
			writer.Write(StopRow);
			writer.Write(" of ");
			writer.Write(NumRows);
			if(NumPages > 1)
			{
				writer.Write(" (");
				if(PageNum > 1)
				{
					writer.Write("<a href=\"recentcomments.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + (PageNum-1).ToString() + "\">Previous " + PageSize.ToString() + "</a>");
				}
				if(PageNum > 1 && PageNum < NumPages)
				{
					writer.Write(" | ");
				}
				if(PageNum < NumPages)
				{
					writer.Write("<a href=\"recentcomments.aspx?categoryid=" + CategoryID.ToString() + "&pagenum=" + (PageNum+1).ToString() + "\">Next " + PageSize.ToString() + "</a>");
				}
				writer.Write(")");
			}
			writer.Write("</TD>\n");
			writer.Write("<td align=\"right\" width=\"40%\" height=\"25\" bgcolor=\"#CCCCCC\">\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");

			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("</form>\n");

			ds.Dispose();

			writer.Write("<div id=\"RateCommentDiv\" name=\"RateCommentDiv\" style=\"position:absolute; left:0px; top:0px; visibility:" + Common.Application("RateCommentFrameVisibility") + "; z-index:2000; \">\n");
			writer.Write("<iframe name=\"RateCommentFrm\" id=\"RateCommentFrm\" width=\"300\" height=\"300\" hspace=\"0\" vspace=\"0\" marginheight=\"0\" marginwidth=\"0\" frameborder=\"0\" noresize scrolling=\"yes\"></iframe>\n");
			writer.Write("</div>\n");
			writer.Write("<script type=\"text/javascript\" language=\"javascript\">\n");
			writer.Write("function RateComment(ProductID,MyCustomerID,MyVote,RatersCustomerID)\n");
			writer.Write("	{\n");
			writer.Write("	RateCommentFrm.location = 'RateComment.aspx?Productid=' + ProductID + '&VotingCustomerID=' + MyCustomerID + '&MyVote=' + MyVote + '&CustomerID=' + RatersCustomerID\n");
			writer.Write("	}\n");
			writer.Write("function MakeROTDComment(RatingID)\n");
			writer.Write("	{\n");
			writer.Write("	RateCommentFrm.location = 'ROTDComment.aspx?RatingID=' + RatingID;\n");
			writer.Write("	}\n");
			writer.Write("</script>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("	function RateIt(ProductID,ProductGUID)\n");
			writer.Write("	{\n");
			writer.Write("		window.open('rateit.aspx?Productid=' + ProductID + '&refresh=no&returnurl=" + Server.UrlEncode(Common.PageInvocation()) + "','AspDotNetStorefront_ML','height=450,width=440,top=10,left=20,status=no,toolbar=no,menubar=no,scrollbars=yes,location=no');\n");
			writer.Write("	}\n");
			writer.Write("</SCRIPT>\n");
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
