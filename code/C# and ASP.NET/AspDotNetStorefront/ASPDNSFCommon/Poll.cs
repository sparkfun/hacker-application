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

namespace AspDotNetStorefrontCommon
{
	/// <summary>
	/// Summary description for Poll.
	/// </summary>
	public class Poll
	{
		public int _pollID;
		public int _siteID;
		public String _name;
		public int _sortOrder;

		public Poll(int PollID, int SiteID)
		{
			_siteID = SiteID;
			_pollID = PollID;
			_name = String.Empty;
			_sortOrder = 3;
			IDataReader rs = DB.GetRS("select * from Poll  " + DB.GetNoLock() + " where PollID=" + PollID.ToString());
			if(rs.Read())
			{
				_name = DB.RSField(rs,"Name");
				_sortOrder = DB.RSFieldInt(rs,"PollSortOrderID");
			}
			rs.Close();
		}

		public void RecordVote(int CustomerID, int PollAnswerID)
		{
			if(!CustomerHasVoted(CustomerID))
			{
				DB.ExecuteSQL("insert into PollVotingRecord(PollID,PollAnswerID,CustomerID) values(" + _pollID.ToString() + "," + PollAnswerID.ToString() + "," + CustomerID.ToString() + ")");
			}
		}

		public bool IsExpired()
		{
			return DB.GetSqlN("select count(*) as N from Poll  " + DB.GetNoLock() + " where PollID=" + _pollID.ToString() + " and ExpiresOn<" + DB.DateQuote(Localization.ToNativeShortDateString(System.DateTime.Now))) > 0;
		}

		public String Display(int CustomerID, bool ShowPollsLink)
		{
			StringBuilder tmpS = new StringBuilder(5000);
			if(!CustomerHasVoted(CustomerID))
			{
				tmpS.Append("<form method=\"POST\" action=\"pollvote.aspx\" name=\"Poll" + _pollID.ToString() + "Form\" id=\"Poll" + _pollID.ToString() + "Form\">");
				tmpS.Append("<input type=\"hidden\" name=\"PollID\" value=\"" + _pollID.ToString() + "\">");
				tmpS.Append("<span class=\"PollTitle\">" + _name + Common.IIF(IsExpired() , " (Not Active)" , "") + "</span><br>");
				IDataReader rs = DB.GetRS("select * from PollAnswer  " + DB.GetNoLock() + " where deleted=0 and PollID=" + _pollID.ToString() + " order by displayorder,name");
				while(rs.Read())
				{
					tmpS.Append("<input class=\"PollRadio\" type=\"radio\" value=\"" + DB.RSFieldInt(rs,"PollAnswerID").ToString() + "\" name=\"Poll_" + _pollID.ToString() + "\"><span class=\"PollAnswer\">" + DB.RSField(rs,"Name") + "</span><br>");
				}
				rs.Close();
				tmpS.Append("<div align=\"center\"><input class=\"PollSubmit\" type=\"submit\" value=\"Vote\" name=\"B1\"></div>");
				tmpS.Append("</form>");
			}
			else
			{
				tmpS.Append("<span class=\"PollTitle\">" + _name + Common.IIF(IsExpired() , " (Not Active)" , "") + "</span><br>");
				tmpS.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
				String sql = "SELECT TOP 100 PERCENT Poll.PollID, PollAnswer.Name, PollAnswer.PollAnswerID, COUNT(PollVotingRecord.PollAnswerID) AS NumVotes, PollAnswer.DisplayOrder FROM (PollAnswer  " + DB.GetNoLock() + " INNER JOIN Poll  " + DB.GetNoLock() + " ON PollAnswer.PollID = Poll.PollID) LEFT OUTER JOIN PollVotingRecord  " + DB.GetNoLock() + " ON PollAnswer.PollID = PollVotingRecord.PollID AND PollAnswer.PollAnswerID = PollVotingRecord.PollAnswerID GROUP BY Poll.PollID, PollAnswer.Name, PollAnswer.PollAnswerID, PollAnswer.DisplayOrder HAVING (Poll.PollID = " + _pollID.ToString() + ") ";
				switch(_sortOrder)
				{
					case 1:
						// As Written
						sql = sql + " Order By PollAnswer.PollAnswerID";
						break;
					case 2:
						// Ascending
						sql = sql + " ORDER BY NumVotes ASC, PollAnswer.PollAnswerID";
						break;
					case 3:
						// Descending
						sql = sql + " ORDER BY NumVotes DESC, PollAnswer.PollAnswerID";
						break;
				}
				IDataReader rs = DB.GetRS(sql);
				int NV = NumVotes();
				while(rs.Read())
				{
					int AnswerNumVotes = DB.RSFieldInt(rs,"NumVotes");
					int ThisPercent = (int)((Single)AnswerNumVotes/(Single)NV*100.0);
					tmpS.Append("<tr>");
					tmpS.Append("<td width=\"40%\" align=\"right\" valign=\"middle\"><span class=\"PollAnswer\">" + DB.RSField(rs,"Name") + ":&nbsp;</span></td>");
					tmpS.Append("<td width=\"60%\" align=\"left\" valign=\"middle\"><img src=\"skins/skin_" + _siteID.ToString() + "/images/pollimage.gif\" align=\"absmiddle\" width=\"" + ((int)(ThisPercent*0.9)).ToString() + "%\" height=\"10\" border=\"0\"><span class=\"PollAnswer\"> (" + ThisPercent.ToString() + "%)</span></td>");
					tmpS.Append("</tr>");
					tmpS.Append("<tr><td colspan=\"2\"><img src=\"images/spacer.gif\" width=\"100%\" height=\"2\"></td></tr>");
				}
				rs.Close();
				tmpS.Append("</table>");
				tmpS.Append("  <div align=\"center\"><span class=\"PollLink\">Number of Votes: " + NV.ToString() + "</span></div>");
				if(ShowPollsLink)
				{
					tmpS.Append("  <div align=\"center\"><a class=\"PollLink\" href=\"polls.aspx\">View All Polls</a></div>");
				}
			}
			return tmpS.ToString();
		}

		public int NumVotes()
		{
			return DB.GetSqlN("select count(*) as N from PollVotingRecord  " + DB.GetNoLock() + " where pollanswerid in (select distinct pollanswerid from pollanswer  " + DB.GetNoLock() + " where deleted=0) and PollID=" + _pollID.ToString());
		}

		public bool CustomerHasVoted(int CustomerID)
		{
			return DB.GetSqlN("select count(*) as N from PollVotingRecord  " + DB.GetNoLock() + " where PollID=" + _pollID.ToString() + " and CustomerID=" + CustomerID.ToString()) != 0;
		}

	}
}
