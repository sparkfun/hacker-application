// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for pollvote.
	/// </summary>
	public class pollvote : System.Web.UI.Page
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");

			Customer thisCustomer = new Customer();
			thisCustomer.RequireCustomerRecord();

			int PollID = Common.FormNativeInt("PollID");
			int CustomerID = thisCustomer._customerID;
			int PollAnswerID = Common.FormNativeInt("Poll_" + PollID.ToString());

			if(PollID != 0 && CustomerID != 0 && PollAnswerID != 0)
			{
				// record the vote:
				try
				{
					DB.ExecuteSQL("insert into PollVotingRecord(PollID,CustomerID,PollAnswerID) values(" + PollID.ToString() + "," + CustomerID.ToString() + "," + PollAnswerID.ToString() + ")");
				}
				catch {}
			}

			Response.Redirect("polls.aspx");
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
