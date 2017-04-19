// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Text;
using AspDotNetStorefrontCommon;


namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for lat_driver.
	/// </summary>
	public class lat_driver : SkinBase
	{
		private int AffiliateID;
		private decimal TotalEarnings;
		private decimal TotalPayouts;
		private String FirstName;
		private String LastName;
		private String EMail;
		private bool TrackingOnly;
		private Topic t;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			AffiliateID = Common.QueryStringUSInt("AffiliateID");
			if(AffiliateID == 0 || !Common.IsValidAffiliate(AffiliateID))
			{
				Response.Redirect("affiliates.aspx");
			}

			TotalEarnings = Common.AffiliateTotalEarnings(AffiliateID);
			TotalPayouts = Common.AffiliateTotalPayouts(AffiliateID);
			
			FirstName = String.Empty;
			LastName = String.Empty;
			EMail = String.Empty;
			TrackingOnly = false;

			IDataReader rs = DB.GetRS("select * from affiliate where affiliateid=" + AffiliateID.ToString());
			if(rs.Read())
			{
				FirstName = DB.RSField(rs,"FirstName");
				LastName = DB.RSField(rs,"LastName");
				EMail = DB.RSField(rs,"EMail");
				TrackingOnly = DB.RSFieldBool(rs,"TrackingOnly");
			}
			rs.Close();

			t = new Topic(Common.QueryString("topic"),thisCustomer._localeSetting,_siteID);
			SectionTitle = "<a href=\"affiliates.aspx?searchfor=" + AffiliateID.ToString() + "\">Affiliates</a> - " + t._sectionTitle + ": " + (FirstName + " " + LastName).Trim() + " (" + EMail + ")" + ")";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{

			writer.Write("<table cellSpacing=\"5\" cellPadding=\"5\" border=\"0\" width=\"100%\">\n");
			writer.Write("  <tbody>\n");
			writer.Write("    <tr>\n");
			writer.Write("      <td vAlign=\"top\" align=\"top\">\n");
			writer.Write("        <table cellSpacing=\"0\" cellPadding=\"0\" width=\"171\" bgColor=\"#AAAAAA\" border=\"0\">\n");
			writer.Write("          <tbody>\n");
			writer.Write("            <tr>\n");
			writer.Write("              <td bgcolor=\"#AAAAAA\" height=\"18\"><b style=\"color: #ffffff\" class=\"small\">&nbsp;Need Help?</b></td>\n");
			writer.Write("            </tr>\n");
			writer.Write("            <tr vAlign=\"center\" align=\"middle\">\n");
			writer.Write("              <td>\n");
			writer.Write("                <table cellSpacing=\"0\" cellPadding=\"1\" width=\"171\" bgColor=\"#AAAAAA\" border=\"0\">\n");
			writer.Write("                  <tbody>\n");
			writer.Write("                    <tr>\n");
			writer.Write("                      <td width=\"100%\">\n");
			writer.Write("                        <table cellSpacing=\"0\" cellPadding=\"4\" width=\"100%\" bgColor=\"#AAAAAA\" border=\"0\">\n");
			writer.Write("                          <tbody>\n");
			writer.Write("                            <tr>\n");
			writer.Write("                              <td vAlign=\"top\" align=\"left\" width=\"100%\" bgColor=\"#ffffff\">\n");
			writer.Write("                                • <a href=\"lat_account.aspx?affiliateid=" + AffiliateID.ToString() + "\">Account Home</a><br>\n");
//			writer.Write("                                • <a href=\"lat_accountstats.aspx?affiliateid=" + AffiliateID.ToString() + "\">Activity Stats</a><br>\n");
//			if(!TrackingOnly)
//			{
//				writer.Write("                                • <a href=\"lat_earnings.aspx?affiliateid=" + AffiliateID.ToString() + "\">Your Earnings</a><br>\n");
//			}
			writer.Write("                                • <a href=\"lat_getlinking.aspx?affiliateid=" + AffiliateID.ToString() + "\">Web Linking Instructions</a><br>\n");
			writer.Write("                                • <a href=\"lat_driver.aspx?affiliateid=" + AffiliateID.ToString() + "&topic=affiliate_faq\">FAQs</a><br>\n");
			writer.Write("                                • <a href=\"mailto:" + Common.AppConfig("RewardsEmailAddress") + "\">Ask A Question</a><br>\n");
			writer.Write("                                • <a href=\"lat_driver.aspx?affiliateid=" + AffiliateID.ToString() + "&topic=affiliate_terms\">Terms &amp; Conditions</a></td>\n");
			writer.Write("                            </tr>\n");
			writer.Write("                          </tbody>\n");
			writer.Write("                        </table>\n");
			writer.Write("                      </td>\n");
			writer.Write("                    </tr>\n");
			writer.Write("                  </tbody>\n");
			writer.Write("                </table>\n");
			writer.Write("              </td>\n");
			writer.Write("            </tr>\n");
			writer.Write("          </tbody>\n");
			writer.Write("        </table>\n");
						
//			writer.Write("        <p>&nbsp;\n");
//			writer.Write("        <table cellSpacing=\"0\" cellPadding=\"0\" width=\"171\" bgColor=\"#AAAAAA\" border=\"0\">\n");
//			writer.Write("          <tbody>\n");
//			writer.Write("            <tr>\n");
//			writer.Write("              <td bgcolor=\"#AAAAAA\" height=\"18\"><b style=\"color: #ffffff\" class=\"small\">&nbsp;Quick Stats</b></td>\n");
//			writer.Write("            </tr>\n");
//			writer.Write("            <tr vAlign=\"center\" align=\"middle\">\n");
//			writer.Write("              <td>\n");
//			writer.Write("                <table cellSpacing=\"0\" cellPadding=\"1\" width=\"171\" bgColor=\"#AAAAAA\" border=\"0\">\n");
//			writer.Write("                  <tbody>\n");
//			writer.Write("                    <tr>\n");
//			writer.Write("                      <td width=\"100%\">\n");
//			writer.Write("                        <table cellSpacing=\"0\" cellPadding=\"4\" width=\"100%\" bgColor=\"#AAAAAA\" border=\"0\">\n");
//			writer.Write("                          <tbody>\n");
//			writer.Write("                            <tr>\n");
//			writer.Write("                              <td vAlign=\"top\" align=\"left\" width=\"100%\" bgColor=\"#ffffff\">\n");
//			writer.Write("                                • Your Affiliate ID: <b>" + AffiliateID.ToString() + "</b><br>\n");
//			writer.Write("                                • Total Earnings: " + Localization.CurrencyStringForDisplay(TotalEarnings) + "<br>\n");
//			writer.Write("                                • Total Payouts : " + Localization.CurrencyStringForDisplay(TotalPayouts) + "<br>\n");
//			writer.Write("                                • Acct. Balance : " + Localization.CurrencyStringForDisplay(TotalEarnings-TotalPayouts) + "<br>\n");
//			writer.Write("								</td>");
//			writer.Write("                            </tr>\n");
//			writer.Write("                          </tbody>\n");
//			writer.Write("                        </table>\n");
//			writer.Write("                      </td>\n");
//			writer.Write("                    </tr>\n");
//			writer.Write("                  </tbody>\n");
//			writer.Write("                </table>\n");
//			writer.Write("              </td>\n");
//			writer.Write("            </tr>\n");
//			writer.Write("          </tbody>\n");
//			writer.Write("        </table>\n");
			
			writer.Write("      </td>\n");
			writer.Write("      <td width=\"100%\" align=\"left\" valign=\"top\">\n");
			writer.Write("        <br>\n");

			// START TOPIC:

			if(t._contents.Length == 0)
			{
				writer.Write("<img src=\"images/spacer.gif\" border=\"0\" height=\"100\" width=\"1\"><br>\n");
				writer.Write("<p align=\"center\"><font class=\"big\"><b>This page is currently empty. Please check back again for an update.</b></font></p>");
			}
			else
			{	
				writer.Write("\n");
				writer.Write("<!-- READ FROM " + Common.IIF(t._fromDB , "DB", "FILE: " + t._fn) + ": " + " -->");
				writer.Write("\n");
				writer.Write(t._contents.Replace("%AFFILIATEID%",AffiliateID.ToString()));
				writer.Write("\n");
				writer.Write("<!-- END OF " + Common.IIF(t._fromDB , "DB", "FILE: " + t._fn) + ": " + " -->");
				writer.Write("\n");
			}

			// END TOPIC

			writer.Write("      </td>\n");
			writer.Write("    </tr>\n");
			writer.Write("  </tbody>\n");
			writer.Write("</table>\n");
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
