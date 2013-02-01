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
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for lat_driver.
	/// </summary>
	public class lat_driver : SkinBase
	{

		Topic t;

		private void Page_Load(object sender, System.EventArgs e)
		{

			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			base._disableLeftAndRightCols = true; // page gets two wide
			t = new Topic(Common.QueryString("topic"),thisCustomer._localeSetting,_siteID);
			SectionTitle = "<a href=\"lat_account.aspx\">" + Common.AppConfig("AffiliateProgramName") + "</a> - " + t._sectionTitle;
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			int AffiliateID = Common.CookieUSInt("LATAffiliateID");
			if(AffiliateID == 0 || !Common.IsValidAffiliate(AffiliateID))
			{
				Response.Redirect("lat_signin.aspx?returnurl=" + Server.UrlEncode(Common.GetThisPageName(true) + "?" + Common.ServerVariables("QUERY_STRING")));
			}
			decimal TotalEarnings = Common.AffiliateTotalEarnings(AffiliateID);
			decimal TotalPayouts = Common.AffiliateTotalPayouts(AffiliateID);

			String FirstName = String.Empty;
			String LastName = String.Empty;
			String EMail = String.Empty;

			IDataReader rs = DB.GetRS("select * from affiliate  " + DB.GetNoLock() + " where affiliateid=" + AffiliateID.ToString());
			if(rs.Read())
			{
				FirstName = DB.RSField(rs,"FirstName");
				LastName = DB.RSField(rs,"LastName");
				EMail = DB.RSField(rs,"EMail");
			}
			rs.Close();
			
			writer.Write("<table cellSpacing=\"5\" cellPadding=\"5\" border=\"0\" width=\"100%\">\n");
			writer.Write("  <tbody>\n");
			writer.Write("    <tr>\n");
			writer.Write("      <td vAlign=\"top\" align=\"top\">\n");
			writer.Write("        <table cellSpacing=\"0\" cellPadding=\"0\" width=\"171\" bgColor=\"#AAAAAA\" border=\"0\">\n");
			writer.Write("          <tbody>\n");
			writer.Write("            <tr>\n");
			writer.Write("              <td bgcolor=\"#AAAAAA\"><b class=\"small\" style=\"COLOR: #ffffff\">&nbsp;" + Common.AppConfig("AffiliateProgramName") + " Member Sign-Out</b></td>\n");
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
			writer.Write("                              <td vAlign=\"top\" width=\"100%\" bgColor=\"#CCCCCC\">\n");
			writer.Write("                                <p><center><a href=\"lat_signout.aspx\"><img src=\"skins/skin_" + _siteID.ToString() + "/images/logout.gif\" border=\"0\" width=\"76\" height=\"22\"></a></center></td>\n");
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
			writer.Write("        <p>&nbsp;\n");
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
			writer.Write("                                • <a href=\"lat_account.aspx\">Account Home</a><br>\n");
//			writer.Write("                                • <a href=\"lat_accountstats.aspx\">Activity Stats</a><br>\n");
//			writer.Write("                                • <a href=\"lat_earnings.aspx\">Your Earnings</a><br>\n");
			writer.Write("                                • <a href=\"lat_getlinking.aspx\">Web Linking Instructions</a><br>\n");
			writer.Write("                                • <a href=\"lat_driver.aspx?topic=affiliate_faq\">FAQs</a><br>\n");
			writer.Write("                                • <a href=\"mailto:" + Common.AppConfig("AffiliateEMailAddress") + "\">Ask A Question</a><br>\n");
			writer.Write("                                • <a href=\"lat_driver.aspx?topic=affiliate_terms\">Terms &amp; Conditions</a></td>\n");
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
			writer.Write("      <td width=\"100%\" align=\"left\" valign=\"top\"><img border=\"0\" src=\"skins/skin_" + _siteID.ToString() + "/images/affiliateheader_small.jpg\" width=\"328\" height=\"70\"><br>\n");
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
