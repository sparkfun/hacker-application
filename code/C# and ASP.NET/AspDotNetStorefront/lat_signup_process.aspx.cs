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
	/// Summary description for lat_signup_process.
	/// </summary>
	public class lat_signup_process : SkinBase
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			base._disableLeftAndRightCols = true; // page gets two wide
			SectionTitle = "<a href=\"lat_account.aspx\">" + Common.AppConfig("AffiliateProgramName") + "</a> - Sign Up";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			int AffiliateID = Common.CookieUSInt("LATAffiliateID");
		
			String ErrorMsg = String.Empty;
			String EMailField = Common.Form("EMail").ToLower();
//			int NN = DB.GetSqlN("select count(*) as N from Affiliate  " + DB.GetNoLock() + " where EMail=" + Common.SQuote(EMailField) + " and AffiliateID<>" + AffiliateID.ToString());
//			if(NN > 0)
//			{
//				ErrorMsg = "That E-Mail Address is already Taken! Please use another E-mail address!";
//			}
			bool Editing = false;

			if(ErrorMsg.Length == 0)
			{
				IDataReader rs;

				try
				{
					StringBuilder sql = new StringBuilder(2500);
					String Name = Common.Form("Name");
					if(Name.Length == 0)
					{
						if(Common.Form("FirstName").Length != 0)
						{
							Name = (Common.Form("FirstName") + " " + Common.Form("FirstName")).Trim();
						}
						else
						{
							Name = Common.Form("LastName");
						}
					}
					if(!Editing)
					{
						// ok to add them:
						String NewGUID = Common.GetNewGUID();
						sql.Append("insert into Affiliate(AffiliateGUID,EMail,[Password],IsOnline,FirstName,LastName,Name,Company,Address1,Address2,Suite,City,State,Zip,Country,Phone,WebSiteName,WebSiteDescription,URL) values(");
						sql.Append(Common.SQuote(NewGUID) + ",");
						sql.Append(Common.SQuote(Common.Left(EMailField,100)) + ",");
						sql.Append(DB.SQuote(Common.Form("Password")) + ",");
//						try
//						{
//							if(Common.Form("DateOfBirth").Length != 0)
//							{
//								DateTime dob = Localization.ParseNativeDateTime(Common.Form("DateOfBirth"));
//								sql.Append(DB.SQuote(Localization.ToNativeShortDateString(dob)) + ",");
//							}
//							else
//							{
//								sql.Append("NULL,");
//							}
//						}
//						catch
//						{
//							sql.Append("NULL,");
//						}
//						sql.Append(Common.SQuote(Common.Left(Common.Form("Gender"),1)) + ",");
						if(Common.Form("URL").Length != 0)
						{
							sql.Append("1,");
						}
						else
						{
							sql.Append("0,");
						}
						sql.Append(Common.SQuote(Common.Left(Common.Form("FirstName"),50)) + ",");
						sql.Append(Common.SQuote(Common.Left(Common.Form("LastName"),50)) + ",");
						sql.Append(Common.SQuote(Common.Left(Name,100)) + ",");
						sql.Append(Common.SQuote(Common.Left(Common.Form("Company"),50)) + ",");
						if(Common.Form("Address1").Length != 0)
						{
							sql.Append(Common.SQuote(Common.Form("Address1").Replace("\x0D\x0A","")) + ",");
						}
						else
						{
							sql.Append("NULL,");
						}
						if(Common.Form("Address2").Length != 0)
						{
							sql.Append(Common.SQuote(Common.Form("Address2").Replace("\x0D\x0A","")) + ",");
						}
						else
						{
							sql.Append("NULL,");
						}
						if(Common.Form("Suite").Length != 0)
						{
							sql.Append(Common.SQuote(Common.Form("Suite")) + ",");
						}
						else
						{
							sql.Append("NULL,");
						}
						if(Common.Form("City").Length != 0)
						{
							sql.Append(Common.SQuote(Common.Form("City")) + ",");
						}
						else
						{
							sql.Append("NULL,");
						}
						if(Common.Form("State").Length != 0)
						{
							sql.Append(Common.SQuote(Common.Form("State")) + ",");
						}
						else
						{
							sql.Append("NULL,");
						}
						if(Common.Form("Zip").Length != 0)
						{
							sql.Append(Common.SQuote(Common.Form("Zip")) + ",");
						}
						else
						{
							sql.Append("NULL,");
						}
						if(Common.Form("Country").Length != 0)
						{
							sql.Append(Common.SQuote(Common.Form("Country")) + ",");
						}
						else
						{
							sql.Append("NULL,");
						}
						if(Common.Form("Phone").Length != 0)
						{
							sql.Append(Common.SQuote(Common.Form("Phone")) + ",");
						}
						else
						{
							sql.Append("NULL,");
						}
						if(Common.Form("WebSiteName").Length != 0)
						{
							sql.Append(Common.SQuote(Common.Form("WebSiteName")) + ",");
						}
						else
						{
							sql.Append("NULL,");
						}
						if(Common.Form("WebSiteDescription").Length != 0)
						{
							sql.Append(Common.SQuote(Common.Form("WebSiteDescription")) + ",");
						}
						else
						{
							sql.Append("NULL,");
						}
						if(Common.Form("URL").Length != 0)
						{
							String theUrl = Common.Left(Common.Form("URL"),80);
							if(theUrl.IndexOf("http://") == -1 && theUrl.Length != 0)
							{
								theUrl = "http://" + theUrl;
							}
							if(theUrl.Length == 0)
							{
								sql.Append("NULL,");
							}
							else
							{
								sql.Append(Common.SQuote(theUrl));
							}
						}
						else
						{
							sql.Append("NULL");
						}
						sql.Append(")");
						DB.ExecuteSQL(sql.ToString());

						rs = DB.GetRS("select AffiliateID from Affiliate  " + DB.GetNoLock() + " where deleted=0 and AffiliateGUID=" + Common.SQuote(NewGUID));
						rs.Read();
						AffiliateID = DB.RSFieldInt(rs,"AffiliateID");
						Editing = true;
						rs.Close();
					}
					else
					{
						// ok to update:
						sql.Append("update Affiliate set ");
						sql.Append("EMail=" + Common.SQuote(Common.Left(EMailField,100)) + ",");
						sql.Append("[Password]=" + DB.SQuote(Common.Form("Password")) + ",");
//						try
//						{
//							if(Common.Form("DateOfBirth").Length != 0)
//							{
//								DateTime dob = Localization.ParseNativeDateTime(Common.Form("DateOfBirth"));
//								sql.Append("DateOfBirth=" + DB.SQuote(Localization.ToNativeShortDateString(dob)) + ",");
//							}
//						}
//						catch {}
//						sql.Append("Gender=" + Common.SQuote(Common.Left(Common.Form("Gender"),1)) + ",");
						sql.Append("IsOnline=" + Common.IIF(Common.Form("URL").Length == 0 , "0" , "1") + ",");
						sql.Append("FirstName=" + Common.SQuote(Common.Left(Common.Form("FirstName"),50)) + ",");
						sql.Append("LastName=" + Common.SQuote(Common.Left(Common.Form("LastName"),50)) + ",");
						sql.Append("Name=" + Common.SQuote(Common.Left(Name,100)) + ",");
						if(Common.Form("Company").Length != 0)
						{
							sql.Append("Company=" + Common.SQuote(Common.Form("Company")) + ",");
						}
						else
						{
							sql.Append("Company=NULL,");
						}
						if(Common.Form("Address1").Length != 0)
						{
							sql.Append("Address1=" + Common.SQuote(Common.Form("Address1").Replace("\x0D\x0A","")) + ",");
						}
						else
						{
							sql.Append("Address1=NULL,");
						}
						if(Common.Form("Address2").Length != 0)
						{
							sql.Append("Address2=" + Common.SQuote(Common.Form("Address2").Replace("\x0D\x0A","")) + ",");
						}
						else
						{
							sql.Append("Address2=NULL,");
						}
						if(Common.Form("Suite").Length != 0)
						{
							sql.Append("Suite=" + Common.SQuote(Common.Form("Suite")) + ",");
						}
						else
						{
							sql.Append("Suite=NULL,");
						}
						if(Common.Form("City").Length != 0)
						{
							sql.Append("City=" + Common.SQuote(Common.Form("City")) + ",");
						}
						else
						{
							sql.Append("City=NULL,");
						}
						if(Common.Form("State").Length != 0)
						{
							sql.Append("State=" + Common.SQuote(Common.Form("State")) + ",");
						}
						else
						{
							sql.Append("State=NULL,");
						}
						if(Common.Form("Zip").Length != 0)
						{
							sql.Append("Zip=" + Common.SQuote(Common.Form("Zip")) + ",");
						}
						else
						{
							sql.Append("Zip=NULL,");
						}
						if(Common.Form("Country").Length != 0)
						{
							sql.Append("Country=" + Common.SQuote(Common.Form("Country")) + ",");
						}
						else
						{
							sql.Append("Country=NULL,");
						}
						if(Common.Form("Phone").Length != 0)
						{
							sql.Append("Phone=" + Common.SQuote(Common.MakeProperPhoneFormat(Common.Form("Phone"))) + ",");
						}
						else
						{
							sql.Append("Phone=NULL,");
						}
						if(Common.Form("WebSiteName").Length != 0)
						{
							sql.Append("WebSiteName=" + Common.SQuote(Common.Form("WebSiteName")) + ",");
						}
						else
						{
							sql.Append("WebSiteName=NULL,");
						}
						if(Common.Form("WebSiteDescription").Length != 0)
						{
							sql.Append("WebSiteDescription=" + Common.SQuote(Common.Form("WebSiteDescription")) + ",");
						}
						else
						{
							sql.Append("WebSiteDescription=NULL,");
						}
						if(Common.Form("URL").Length != 0)
						{
							String theUrl2 = Common.Left(Common.Form("URL"),80);
							if(theUrl2.IndexOf("http://") == -1 && theUrl2.Length != 0)
							{
								theUrl2 = "http://" + theUrl2;
							}
							if(theUrl2.Length != 0)
							{
								sql.Append("URL=" + Common.SQuote(theUrl2) + ",");
							}
							else
							{
								sql.Append("URL=NULL,");
							}
						}
						else
						{
							sql.Append("URL=NULL,");
						}
						sql.Append("LastUpdated=" + DB.SQuote(Localization.ToNativeDateTimeString(System.DateTime.Now)) + ",");
						sql.Append("LastUpdatedBy=" + thisCustomer._customerID.ToString() + " ");
						sql.Append("where AffiliateID=" + AffiliateID.ToString());
						DB.ExecuteSQL(sql.ToString());
						Editing = true;
					}
				}
				catch
				{
					ErrorMsg = "<p><b>ERROR: There was an unknown error in adding your new account record. Please <a href=\"t-contact.aspx\">contact a service representative</a> for assistance.<br><br></b></p>";
				}

			}

			String AffiliateGUID = String.Empty;
			IDataReader rsx = DB.GetRS("select AffiliateGUID from affiliate  " + DB.GetNoLock() + " where AffiliateID=" + AffiliateID.ToString());
			rsx.Read();
			Common.SetSessionCookie("AffiliateID",AffiliateID.ToString());
			rsx.Close();

			writer.Write("<table cellSpacing=\"5\" cellPadding=\"5\" border=\"0\" width=\"100%\">\n");
			writer.Write("  <tbody>\n");
			writer.Write("    <tr>\n");
			writer.Write("      <td vAlign=\"top\" align=\"left\">\n");
			writer.Write("        <table cellSpacing=\"0\" cellPadding=\"0\" width=\"171\" bgColor=\"#AAAAAA\" border=\"0\">\n");
			writer.Write("          <tbody>\n");
			writer.Write("            <tr>\n");
			writer.Write("              <td bgcolor=\"#AAAAAA\" height=\"18\"><b style=\"color: #ffffff\" class=\"small\">&nbsp;Program Links</b></td>\n");
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
			writer.Write("      </td>\n");
			writer.Write("      <td width=\"100%\" align=\"left\" valign=\"top\"><img border=\"0\" src=\"skins/skin_" + _siteID.ToString() + "/images/affiliateheader_small.jpg\" width=\"328\" height=\"70\"><br>\n");
			writer.Write("        <br>\n");

			// SIGNUP STATUS
			if(ErrorMsg.Length > 0)
			{
				writer.Write("<p align=\"left\"><b><font color=\"red\">" + ErrorMsg + "</font></b></p>");
				writer.Write("<p align=\"left\">Please <a href=\"javascript:history.back(-1);\">go back</a> and correct the error. Thanks.</p>");
			}
			else
			{
				try
				{
					// send admin notification:
					String FormContents = String.Empty;
					for(int i = 0; i<=Request.Form.Count-1; i++)
					{
						FormContents += "<b>" + Request.Form.Keys[i] + "</b>=" + Request.Form[Request.Form.Keys[i]] + "<br>";
					}
					Common.SendMail("" + Common.AppConfig("AffiliateProgramName") + " New Member Notification",FormContents,true,Common.AppConfig("MailMe_FromAddress"),Common.AppConfig("MailMe_FromName"),Common.AppConfig("AffiliateEMailAddress"),Common.AppConfig("AffiliateEMailAddress"),String.Empty,Common.AppConfig("MailMe_Server"));
				}
				catch {}

				try
				{
					// send welcome e-mail:
					String WelcomeMsg = new EMailTemplate("affiliatesignup",thisCustomer._localeSetting,_siteID)._contents;
					WelcomeMsg = WelcomeMsg.Replace("%AFFILIATEID%",AffiliateID.ToString());
					WelcomeMsg = WelcomeMsg.Replace("%PASSWORD%",Common.Form("Password"));
					WelcomeMsg = WelcomeMsg.Replace("%STORENAME%",Common.AppConfig("StoreName"));
					WelcomeMsg = WelcomeMsg.Replace("%AFFILIATEPROGRAMNAME%",Common.AppConfig("AffiliateProgramName"));
					WelcomeMsg = WelcomeMsg.Replace("%AFFILIATEPAGEURL%",Common.GetStoreHTTPLocation(false) + "lat_account.aspx");
					WelcomeMsg = WelcomeMsg.Replace("%STOREURL%",Common.GetStoreHTTPLocation(false));
					Common.SendMail("" + Common.AppConfig("AffiliateProgramName") + " Welcome",WelcomeMsg,true,Common.AppConfig("AffiliateEMailAddress"),Common.AppConfig("AffiliateEMailAddress"),EMailField,EMailField,"",Common.AppConfig("MailMe_Server"));
				}
				catch {}
				
				// add a "gift card ID" equal to their affiliateID:
				//Common.AssignCardToAffiliate(thisCustomer._customerID,AffiliateID,AffiliateID.ToString(),(decimal)Common.AppConfigUSSingle("GiftCardAmount"),1,System.DateTime.Now,Localization.ParseUSDateTime("12/31/2050"));

				//DB.ExecuteSQL("insert into AffiliateCardRequest(AffiliateID,RequestedNumber) values(" + AffiliateID.ToString() + ",20)");

				writer.Write("<p align=\"left\"><b>CONGRATULATIONS AND WELCOME TO THE " + Common.AppConfig("AffiliateProgramName").ToUpper() + " PROGRAM!<br><br>Your sign-up was successful.<br><br><a href=\"lat_account.aspx?affiliateid=" + AffiliateID.ToString() + "\">Click here</a> to go to your " + Common.AppConfig("AffiliateProgramName") + " Account Page.</p>");
			}


			// END SIGNUP STATUS

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
