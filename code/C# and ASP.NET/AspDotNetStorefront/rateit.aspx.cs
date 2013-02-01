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
	/// Summary description for rateit.
	/// </summary>
	public class rateit : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");

			Customer thisCustomer = new Customer();
			thisCustomer.RequireCustomerRecord();
			int _siteID = Common.CookieUSInt("SkinID");

			int ProductID = Common.QueryStringUSInt("ProductID");
			String ProductName = Common.GetProductName(ProductID);
			String ReturnURL = Common.QueryString("ReturnURL");

			Response.Write("<html><head><title>Rate Product</title></head><body bgcolor=\"#336699\">\n");

			Response.Write("<!-- " + Common.PageInvocation() + " -->\n");

			Response.Write("<script type=\"text/javascript\" src=\"formValidate.js\"></script> \n");
			Response.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			Response.Write("function FormValidator(theForm)\n");
			Response.Write("	{\n");
			Response.Write("	submitonce(theForm);\n");
			Response.Write("	if (theForm.Rating.selectedIndex < 1)\n");
			Response.Write("	{\n");
			Response.Write("		alert(\"Please select your rating before submitting.\");\n");
			Response.Write("		theForm.Rating.focus();\n");
			Response.Write("		submitenabled(theForm);\n");
			Response.Write("		return (false);\n");
			Response.Write("    }\n");
			Response.Write("	if (theForm.Comments.value.length > 5000)\n");
			Response.Write("	{\n");
			Response.Write("		alert(\"Please limit your comment to 5000 characters.\");\n");
			Response.Write("		theForm.Comments.focus();\n");
			Response.Write("		submitenabled(theForm);\n");
			Response.Write("		return (false);\n");
			Response.Write("    }\n");
			Response.Write("	return (true);\n");
			Response.Write("	}\n");
			Response.Write("\n");
			Response.Write("</script>\n");

			Response.Write("<SCRIPT language=\"javascript\">\n");
			Response.Write("	var ImgArray = new Array(new Image(),new Image())\n");
			Response.Write("	ImgArray[0].src = \"skins/skin_" + _siteID.ToString() + "/images/bigstar-blu.gif\"\n");
			Response.Write("	ImgArray[1].src = \"skins/skin_" + _siteID.ToString() + "/images/bigstar-whi.gif\"\n");
			Response.Write("	\n");
			Response.Write("	function document_onreadystatechange()\n");
			Response.Write("	{\n");
			Response.Write("		newRatingEntered(document.RateItForm.Rating.selectedIndex);\n");
			Response.Write("	}\n");
			Response.Write("	\n");
			Response.Write("	function newRatingEntered(RV)\n");
			Response.Write("	{\n");
			Response.Write("		if (RV >= 1)\n");
			Response.Write("			{document.RateItForm.Star1.src = ImgArray[0].src}\n");
			Response.Write("		else\n");
			Response.Write("			{document.RateItForm.Star1.src = ImgArray[1].src}\n");
			Response.Write("		if (RV >= 2)\n");
			Response.Write("			{document.RateItForm.Star2.src = ImgArray[0].src}\n");
			Response.Write("		else\n");
			Response.Write("			{document.RateItForm.Star2.src = \"skins/skin_" + _siteID.ToString() + "/images/bigstar-whi.gif\"}\n");
			Response.Write("		if (RV >= 3)\n");
			Response.Write("			{document.RateItForm.Star3.src = ImgArray[0].src}\n");
			Response.Write("		else\n");
			Response.Write("			{document.RateItForm.Star3.src = \"skins/skin_" + _siteID.ToString() + "/images/bigstar-whi.gif\"}\n");
			Response.Write("		if (RV >= 4)\n");
			Response.Write("			{document.RateItForm.Star4.src = ImgArray[0].src}\n");
			Response.Write("		else\n");
			Response.Write("			{document.RateItForm.Star4.src = \"skins/skin_" + _siteID.ToString() + "/images/bigstar-whi.gif\"}\n");
			Response.Write("		if (RV >= 5)\n");
			Response.Write("			{document.RateItForm.Star5.src = ImgArray[0].src}\n");
			Response.Write("		else\n");
			Response.Write("			{document.RateItForm.Star5.src =\"skins/skin_" + _siteID.ToString() + "/images/bigstar-whi.gif\"}\n");
			Response.Write("		document.RateItForm.Rating.selectedIndex = RV\n");
			Response.Write("		return false\n");
			Response.Write("	}\n");
			Response.Write("</script>\n");

			bool Editing = false;
			int TheirCurrentRating = 0;
			String TheirCurrentComment = String.Empty;
			bool SecondTry = (Common.Form("SecondTry").ToLower() == "true") || (Common.QueryString("SecondTry").ToLower() == "true");

			IDataReader rs = DB.GetRS("select * from Rating  " + DB.GetNoLock() + " where CustomerID=" + thisCustomer._customerID.ToString() + " and ProductID=" + ProductID.ToString());
			if(rs.Read())
			{
				TheirCurrentRating = DB.RSFieldInt(rs,"Rating");
				TheirCurrentComment = DB.RSField(rs,"Comments");
				Editing = true;
			}
			rs.Close();

			if(SecondTry)
			{
				TheirCurrentRating = Common.SessionUSInt("LastRatingEntered");
				TheirCurrentComment = Common.Session("LastCommentEntered");
			}

			if(Common.Form("IsSubmit").Length > 0)
			{
				StringBuilder sql = new StringBuilder(2500);
				String theCmts = Common.Left(Common.Form("Comments"),5000);
				String theRating = Common.Form("Rating");
				
				CustomerSession sess = new CustomerSession(thisCustomer._customerID);
				sess.SetVal("LastCommentEntered",theCmts,System.DateTime.Now.AddMinutes(Common.SessionTimeout())); // instead of passing via querystring due to length
				sess.SetVal("LastRatingEntered",theRating,System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
				sess = null;

				bool HasBadWords = Ratings.StringHasBadWords(theCmts);
				if(!HasBadWords || SecondTry)
				{
					if(!Editing)
					{
						sql.Append("insert into Rating(ProductID,IsFilthy,CustomerID,DateEntered,Rating,HasComment,Comments) values(");
						sql.Append(ProductID.ToString() + ",");
						sql.Append(Common.IIF(HasBadWords , "1" , "0") + ",");
						sql.Append(thisCustomer._customerID.ToString() + ",");
						//sql.Append(DB.SQuote(Common.ServerVariables("REMOTE_ADDR")) + ",");
						sql.Append(DB.DateQuote(Localization.ToNativeShortDateString(System.DateTime.Now)) + ",");
						sql.Append(theRating + ",");
						sql.Append(Common.IIF(theCmts.Length != 0 , "1" , "0") + ",");
						if(theCmts.Length != 0)
						{
							//HasComment = true;
							sql.Append(DB.SQuote(theCmts));
						}
						else
						{
							sql.Append("NULL");
						}
						sql.Append(")");
						DB.ExecuteSQL(sql.ToString());
					} 
					else 
					{
						sql.Append("update Rating set ");
						sql.Append("IsFilthy=" + Common.IIF(HasBadWords , "1" , "0") + ",");
						sql.Append("Rating=" + theRating + ",");
						//sql.Append("CustomerIPAddress=" + DB.SQuote(Common.ServerVariables("REMOTE_ADDR")) + ",");
						sql.Append("DateEntered=" + DB.DateQuote(Localization.ToNativeShortDateString(System.DateTime.Now)) + ",");
						sql.Append("HasComment=" + Common.IIF(theCmts.Length != 0 , "1" , "0") + ",");
						if(theCmts.Length != 0)
						{
							//HasComment = true;
							sql.Append("Comments=" + DB.SQuote(theCmts));
						}
						else
						{
							sql.Append("Comments=NULL");
						}
						sql.Append(" where ProductID=" + ProductID.ToString() + " and CustomerID=" + thisCustomer._customerID.ToString());
						DB.ExecuteSQL(sql.ToString());
					}
				}
				Response.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				if(!HasBadWords || SecondTry)
				{
					Response.Write("opener.window.location.reload();"); 
					Response.Write("self.close();");
				}
				else
				{
					Response.Redirect("rateit.aspx?" + Common.ServerVariables("QUERY_STRING") + "&secondtry=true&returnURL=" + Server.UrlEncode(ReturnURL));
				}
				Response.Write("</script>\n");
			}
            
			Response.Write("<form name=\"RateItForm\" action=\"rateit.aspx?refresh=" + Common.QueryString("refresh") + "&Productid=" + ProductID.ToString() + "&oldrating=" + TheirCurrentRating.ToString() + "&OldCommentLength=" + TheirCurrentComment.Length.ToString() + "&returnurl=" + Server.UrlEncode(ReturnURL) + "\" method=\"POST\" onsubmit=\"return (validateForm(this) && FormValidator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
			Response.Write("<input type=\"hidden\" name=\"secondtry\" value=\"" + SecondTry.ToString() + "\">\n");
			Response.Write("<table width=\"100%\" cellpadding=\"5\" cellspacing=\"0\" border=\"0\" bgcolor=\"#336699\">\n");
			Response.Write("<tr><td align=\"center\" valign=\"middle\"><font face=\"arial,helvetica\" color=white><b>You are rating:<br></b></font></td></tr>\n");
			if(SecondTry)
			{
				// their last attempt had BadWords:
				Response.Write("<tr><td align=\"center\" valign=\"middle\"><font style=\"font-size: 10px; font-weight: bold; color: yellow; font-family: Verdana, Geneva, Arial, Helvetica;\">Please clean up your language. You may submit your comment as is, but it may be deleted if it doesn't comply with site rules.</b></font></td></tr>\n");
			}
			Response.Write("<tr><td align=\"center\" valign=\"top\">\n");
			Response.Write("<table width=\"100%\" cellpadding=\"6\" cellspacing=\"0\" border=\"0\">\n");
			Response.Write("<tr><td width=\"10%\"></td><td align=\"center\" valign=\"top\"  bgcolor=\"#FFFFCC\">\n");
			Response.Write("<font face=\"arial,helvetica\" size=2><b>" + ProductName + "</b></font>\n");
			Response.Write("</td><td width=\"10%\"></td></tr>\n");
			Response.Write("</table>\n");
			Response.Write("</td></tr>\n");
			Response.Write("<tr><td align=\"center\" valign=\"middle\" ><font face=\"arial,helvetica\" color=white><b>This is your rating:<br></b></font></td></tr>\n");
			Response.Write("<tr><td align=\"center\" valign=\"top\">\n");
			Response.Write("<table width=\"100%\" cellpadding=\"10\" cellspacing=\"0\" border=\"0\">\n");
			Response.Write("<tr><td width=\"25%\"></td><td align=\"center\" valign=\"top\" bgcolor=\"#FFFFFF\">\n");

			Response.Write("<TABLE border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\n");
			Response.Write("<TR>\n");
			Response.Write("<TD align=\"CENTER\" valign=\"MIDDLE\">\n");
			Response.Write("<A HREF=\"javascript://\" onclick=\"return newRatingEntered(1);\">\n");
			Response.Write("<img name=\"Star1\" src=\"skins/skin_" + _siteID.ToString() + "/images/bigstar-whi.gif\" width=\"30\" height=\"30\" border=\"0\" alt=\"\" hspace=\"2\">\n");
			Response.Write("</A>\n");
			Response.Write("</TD>\n");
			Response.Write("<TD align=\"CENTER\" valign=\"MIDDLE\">\n");
			Response.Write("<A HREF=\"javascript://\" onclick=\"return newRatingEntered(2);\">\n");
			Response.Write("<img name=\"Star2\" src=\"skins/skin_" + _siteID.ToString() + "/images/bigstar-whi.gif\" width=\"30\" height=\"30\" border=\"0\" alt=\"\" hspace=\"2\">\n");
			Response.Write("</A>\n");
			Response.Write("</TD>\n");
			Response.Write("<TD align=\"CENTER\" valign=\"MIDDLE\">\n");
			Response.Write("<A HREF=\"javascript://\" onclick=\"return newRatingEntered(3);\">\n");
			Response.Write("<img name=\"Star3\" src=\"skins/skin_" + _siteID.ToString() + "/images/bigstar-whi.gif\" width=\"30\" height=\"30\" border=\"0\" alt=\"\" hspace=\"2\">\n");
			Response.Write("</A>\n");
			Response.Write("</TD>\n");
			Response.Write("<TD align=\"CENTER\" valign=\"MIDDLE\">\n");
			Response.Write("<A HREF=\"javascript://\" onclick=\"return newRatingEntered(4);\">\n");
			Response.Write("<img name=\"Star4\" src=\"skins/skin_" + _siteID.ToString() + "/images/bigstar-whi.gif\" width=\"30\" height=\"30\" border=\"0\" alt=\"\" hspace=\"2\">\n");
			Response.Write("</A>\n");
			Response.Write("</TD>\n");
			Response.Write("<TD align=\"CENTER\" valign=\"MIDDLE\">\n");
			Response.Write("<A HREF=\"javascript://\" onclick=\"return newRatingEntered(5);\">\n");
			Response.Write("<img name=\"Star5\" src=\"skins/skin_" + _siteID.ToString() + "/images/bigstar-whi.gif\" width=\"30\" height=\"30\" border=\"0\" alt=\"\" hspace=\"2\">\n");
			Response.Write("</A>\n");
			Response.Write("</TD>\n");
			Response.Write("</TR>\n");
			Response.Write("</TABLE>\n");

			
			Response.Write("<select name=\"Rating\" style=\"font-size: 10px;\" onChange=\"newRatingEntered(document.RateItForm.Rating.selectedIndex)\">\n");
			Response.Write("<OPTION value=\"0\" selected>Select Rating</OPTION>\n");
			Response.Write("<OPTION value=\"1\"" + Common.IIF(TheirCurrentRating == 1 , " selected " , "") + ">1-Terrible!</OPTION>\n");
			Response.Write("<OPTION value=\"2\"" + Common.IIF(TheirCurrentRating == 2 , " selected " , "") + ">2-Bad</OPTION>\n");
			Response.Write("<OPTION value=\"3\"" + Common.IIF(TheirCurrentRating == 3 , " selected " , "") + ">3-OK</OPTION>\n");
			Response.Write("<OPTION value=\"4\"" + Common.IIF(TheirCurrentRating == 4 , " selected " , "") + ">4-Good</OPTION>\n");
			Response.Write("<OPTION value=\"5\"" + Common.IIF(TheirCurrentRating == 5 , " selected " , "") + ">5-Great!</OPTION>\n");
			Response.Write("</SELECT>\n");

			Response.Write("<br><font face=\"arial,helvetica\" size=1>(change rating above if desired></font>\n");
			Response.Write("</td><td width=\"25%\"></tr>\n");
			Response.Write("</table>\n");
			Response.Write("</td></tr>\n");
			Response.Write("<tr><td align=\"center\" valign=\"middle\" ><font face=\"arial,helvetica\" color=white size=2><b>ADD A COMMENT:</b></font></td></tr>\n");
			Response.Write("<tr><td align=\"center\" valign=\"middle\"><textarea name=\"Comments\" rows=\"6\" cols=\"40\">" + TheirCurrentComment + "</textarea></td>\n");
			Response.Write("<tr><td valign=\"top\" align=\"center\"><input type=\"hidden\" value=\"Yes\" name=\"IsSubmit\"><input type=\"submit\" value=\"Submit\" name=\"B1\">&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"button\" onClick=\"javascript:self.close();\" value=\"Cancel\" name=\"B2\"></td></tr>\n");
			Response.Write("</table>\n");

			Response.Write("</form>\n");

			Response.Write("<SCRIPT LANGUAGE=javascript FOR=document EVENT=onreadystatechange>\n");
			Response.Write(" document_onreadystatechange()\n");
			Response.Write("</SCRIPT>\n");

			Response.Write("</body></html>\n");

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
