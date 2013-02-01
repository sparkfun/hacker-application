// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for ratecomment.
	/// </summary>
	public class ratecomment : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");

			Customer thisCustomer = new Customer();
			thisCustomer.RequireCustomerRecord();

			int ProductID = Common.QueryStringUSInt("ProductID");
			int VotingCustomerID = Common.QueryStringUSInt("VotingCustomerID");
			int CustomerID = Common.QueryStringUSInt("CustomerID");
			String MyVote = Common.QueryString("MyVote").ToUpper();
			int HelpfulVal = Common.IIF(MyVote == "YES" , 1 , 0);
			bool IsProduct = (Common.QueryString("IsProduct").ToUpper() == "TRUE");

			bool AlreadyVoted = false;
			IDataReader rs = DB.GetRS("select * from RatingCommentHelpfulness  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString() + " and RatingCustomerID=" + CustomerID.ToString() + " and VotingCustomerID=" + VotingCustomerID.ToString());
			if(rs.Read())
			{
				AlreadyVoted = true;
				// they have already voted on this comment, and are changing their minds perhaps, so adjust totals, and reapply vote:
				if(DB.RSFieldBool(rs,"Helpful"))
				{
					DB.ExecuteSQL("update Rating set FoundHelpful = FoundHelpful-1 where ProductID=" + ProductID.ToString() + " and CustomerID=" + CustomerID.ToString());
				}
				else
				{
					DB.ExecuteSQL("update Rating set FoundNotHelpful = FoundNotHelpful-1 where ProductID=" + ProductID.ToString() + " and CustomerID=" + CustomerID.ToString());
				}
			}
			rs.Close();
			if(AlreadyVoted)
			{
				DB.ExecuteSQL("delete from RatingCommentHelpfulness where ProductID=" + ProductID.ToString() + " and RatingCustomerID=" + CustomerID.ToString() + " and VotingCustomerID=" + VotingCustomerID.ToString());
			}

			DB.ExecuteSQL("insert into RatingCommentHelpfulness(ProductID,RatingCustomerID,VotingCustomerID,Helpful) values(" + ProductID.ToString() + "," + CustomerID.ToString() + "," + VotingCustomerID.ToString() + "," + HelpfulVal.ToString() + ")");
			if(MyVote == "YES")
			{
				DB.ExecuteSQL("update Rating set FoundHelpful = FoundHelpful+1 where ProductID=" + ProductID.ToString() + " and CustomerID=" + CustomerID.ToString());
			}
			else
			{
				DB.ExecuteSQL("update Rating set FoundNotHelpful = FoundNotHelpful+1 where ProductID=" + ProductID.ToString() + " and CustomerID=" + CustomerID.ToString());
			}

			Response.Write("<html>\n");
			Response.Write("<head>\n");
			Response.Write("<title>Rate Comment</title>\n");
			Response.Write("</head>\n");
			Response.Write("<body>\n");
			Response.Write("<!-- INVOCATION: " + Common.PageInvocation() + " -->\n");
			Response.Write("</body>\n");
			Response.Write("</html>\n");

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
