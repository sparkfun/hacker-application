// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Xml;
using System.Configuration;
using System.Web.SessionState;
using System.Web.Caching;
using System.Web.Handlers;
using System.Web.Hosting;
using System.Web.Mail;
using System.Web.Security;
using System.Web.UI;
using System.Web.Util;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for mailingmgr
	/// </summary>
	public class mailingmgr : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Mailing Manager";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			if(Common.Form("RemoveEMail").Length != 0)
			{
				DB.ExecuteSQL("update customer set OkToEmail=0 where Email=" + DB.SQuote(Common.Form("RemoveEMail").ToLower()));
				writer.Write("<p align=\"left\"><b>Customer Removed...<br></b></p>");
			}

			Topic t = new Topic("MailFooter",thisCustomer._localeSetting,_siteID);
			String Footer = t._contents;
			if(Common.Form("IsSubmit").ToUpper() == "TRUE")
			{
				String Subject = Common.Form("Subject");
				String Body = Common.Form("MessageBodyText");
				int CustomerLevelID = Localization.ParseUSInt(Common.Form("CustomerLevelID"));
				String FromEMail = Common.AppConfig("ReceiptEMailFrom");
				String FromName = Common.AppConfig("ReceiptEMailFromName");
				String ToEMail = String.Empty;
				String FromServer = Common.AppConfig("MailMe_Server");

				bool testing = (Common.Form("TESTONLY")=="1");
				bool ordersonly = (Common.Form("ORDERSONLY")=="1");
				bool listonly = (Common.Form("LISTONLY")=="1");
				writer.Write("<div align=\"left\">");
				String tmpS = String.Empty;
				String ToList = String.Empty;
				if(testing)
				{
					writer.Write("Sending TEST to: " + FromEMail + "...");
					try
					{
						Common.SendMail(Subject,Body + Footer,true,FromEMail,FromName,FromEMail,FromName,"",FromServer);
					}
					catch(Exception ex)
					{
						writer.Write(Common.GetExceptionDetail(ex,"<br>"));
					}
					writer.Write("done<br>");
					Response.Flush();
				}
				else
				{
					String CustSQL = "select customerGUID,email from customer  " + DB.GetNoLock() + " where EMail not in (select ToEMail from mailingmgrlog  " + DB.GetNoLock() + " where month(senton)=" + System.DateTime.Now.Month.ToString() + " and day(senton)=" + System.DateTime.Now.Day.ToString() + " and year(SentOn)=" + System.DateTime.Now.Year.ToString() + " and Subject=" + DB.SQuote(Subject) + ") and deleted=0 and OkToEMail<>0 and EMail not like 'Anon_%' " + Common.IIF(CustomerLevelID != 0 , " and CustomerLevelID=" + CustomerLevelID.ToString() , "") + Common.IIF(ordersonly , " and CustomerID in (select distinct customerid from orders " + DB.GetNoLock() + " ) " , "") + " order by customerID";
					//writer.Write("Customer Mailing SQL=" + CustSQL + "<br>");
					DataSet ds = DB.GetDS(CustSQL,false,System.DateTime.Now.AddHours(1));
					if(ds.Tables[0].Rows.Count > 0)
					{
						int i = 0;
						String[] s = new String[ds.Tables[0].Rows.Count];
						String[] guid = new String[ds.Tables[0].Rows.Count];
						foreach(DataRow row in ds.Tables[0].Rows)
						{
							String EM = DB.RowField(row,"EMail");
							// try to check the EMail to see if it's valid:
							s[i] = EM;
							guid[i] = DB.RowField(row,"CustomerGUID");
							i++;
						}
						int BCCSize = Common.AppConfigUSInt("MailingMgr.BlockSize");
						if(BCCSize == 0)
						{
							BCCSize = 5; // default
						}
						i = 0;
						if(Common.AppConfigBool("MailingMgr.SendEachEmailSeparately"))
						{
							Footer = Footer.Replace("%REMOVEURL%",Common.GetStoreHTTPLocation(false) + "remove.aspx?id=" + guid[i]);
							// send each e-mail separately:
							while(i < ds.Tables[0].Rows.Count)
							{
								String ToThisPerson = s[i];
								writer.Write(Common.IIF(listonly , "JUST LISTING ADDRESS:<br>" , "SENDING LIVE TO:<br>") + ToThisPerson + "...<br>");
								try
								{
									if(!listonly)
									{
										Common.SendMail(Subject,Body + Footer,true,FromEMail,FromName,ToThisPerson,ToThisPerson,String.Empty,FromServer);
										DB.ExecuteSQL("insert into MailingMgrLog(ToEMail,FromEMail,Subject,Body) values(" + DB.SQuote(ToThisPerson) + "," + DB.SQuote(FromEMail) + "," + DB.SQuote(Subject) + "," + DB.SQuote(Body) + ")");
									}
								}
								catch(Exception ex)
								{
									writer.Write(Common.GetExceptionDetail(ex,"<br>"));
								}
								writer.Write("done<br>");
								Response.Flush();
								i++;
							}						
						}
						else
						{
							// send in groups of bcc's:
							while(i < ds.Tables[0].Rows.Count)
							{
								ToList = String.Empty;
								for(int j = 1; j <= BCCSize && i < ds.Tables[0].Rows.Count; j++)
								{
									if(ToList.Length != 0)
									{
										ToList += ";";
									}
									ToList += s[i];
									i++;
								}
								writer.Write(Common.IIF(listonly , "JUST LISTING ADDRESS:<br>" , "SENDING LIVE TO:<br>") + ToList.Replace(";","...<br>") + "<br>");
								try
								{
									if(!listonly)
									{
										Common.SendMail(Subject,Body + Footer,true,FromEMail,FromName,FromEMail,FromName,ToList,FromServer);
										String[] sentto = ToList.Split(';');
										foreach(String ss in sentto)
										{
											DB.ExecuteSQL("insert into MailingMgrLog(ToEMail,FromEMail,Subject,Body) values(" + DB.SQuote(ss) + "," + DB.SQuote(FromEMail) + "," + DB.SQuote(Subject) + "," + DB.SQuote(Body) + ")");
										}
									}
								}
								catch(Exception ex)
								{
									writer.Write(Common.GetExceptionDetail(ex,"<br>"));
								}
								writer.Write("done<br>");
								Response.Flush();
							}
						}
					}
					ds.Dispose();
				}
				writer.Write("</div>");
			}

			if(ErrorMsg.Length != 0)
			{
				writer.Write("<p><b><font color=red>" + ErrorMsg + "</font></b></p>\n");
			}

			writer.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
			writer.Write("function Form_Validator(theForm)\n");
			writer.Write("{\n");
			writer.Write("submitonce(theForm);\n");
			writer.Write("return (true);\n");
			writer.Write("}\n");
			writer.Write("</script>\n");

			writer.Write("<p align=\"left\">You may use this page to send e-mails to all registered customers who have chosen to accept e-mails. You can remove customers from the mailing list at the bottom of the page.</p>\n");
			writer.Write("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\">\n");
			writer.Write("<form action=\"mailingmgr.aspx\" method=\"post\" id=\"Form1\" name=\"Form1\" onsubmit=\"return (validateForm(this) && Form_Validator(this))\" onReset=\"return confirm('Do you want to reset all fields to their starting values?');\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");

			writer.Write("<tr valign=\"middle\">\n");
			writer.Write("<td width=\"25%\" align=\"right\" valign=\"middle\">*Subject:&nbsp;&nbsp;</td>\n");
			writer.Write("<td align=\"left\">\n");
			writer.Write("<input maxLength=\"100\" size=\"30\" name=\"Subject\" value=\"" + Common.Form("Subject") + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"Subject_vldt\" value=\"[req][blankalert=Please enter the subject of the e-mail]\">\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");

			writer.Write("<tr valign=\"middle\">\n");
			writer.Write("<td width=\"25%\" align=\"right\" valign=\"middle\">*TEST ONLY:&nbsp;&nbsp;</td>\n");
			writer.Write("<td align=\"left\">\n");
			writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"TESTONLY\" value=\"1\" checked>\n");
			writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"TESTONLY\" value=\"0\">\n");
			writer.Write("&nbsp;&nbsp;(if testing, only 1 e-mail will be sent to the store contact address)");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");

			writer.Write("<tr valign=\"middle\">\n");
			writer.Write("<td width=\"25%\" align=\"right\" valign=\"middle\">*CUSTOMERS WITH ORDERS ONLY:&nbsp;&nbsp;</td>\n");
			writer.Write("<td align=\"left\">\n");
			writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"ORDERSONLY\" value=\"1\" checked>\n");
			writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"ORDERSONLY\" value=\"0\">\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");

			writer.Write("<tr valign=\"middle\">\n");
			writer.Write("<td width=\"25%\" align=\"right\" valign=\"middle\">*LIST CUSTOMERS WHO WOULD BE EMAILED ONLY:&nbsp;&nbsp;</td>\n");
			writer.Write("<td align=\"left\">\n");
			writer.Write("Yes&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"LISTONLY\" value=\"1\" checked>\n");
			writer.Write("No&nbsp;<INPUT TYPE=\"RADIO\" NAME=\"LISTONLY\" value=\"0\">\n");
			writer.Write("&nbsp;&nbsp;(if list only, e-mails will not be sent)");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");

			writer.Write("<tr valign=\"middle\">\n");
			writer.Write("<td width=\"25%\" align=\"right\" valign=\"middle\">Customer Level:&nbsp;&nbsp;</td>\n");
			writer.Write("<td align=\"left\">\n");
			writer.Write("<select size=\"1\" name=\"CustomerLevelID\">\n");
			writer.Write("<OPTION VALUE=\"0\" " + Common.IIF(Common.Form("CustomerLevelID") == "0" , " selected " , "") + ">All Customers</option>\n");
			DataSet dsst = DB.GetDS("select * from CustomerLevel  " + DB.GetNoLock() + " where deleted=0 order by name",false,System.DateTime.Now.AddHours(1));
			foreach(DataRow row in dsst.Tables[0].Rows)
			{
				writer.Write("<option value=\"" + DB.RowFieldInt(row,"CustomerLevelID").ToString() + "\"");
				if(Common.FormUSInt("CustomerLevelID") == DB.RowFieldInt(row,"CustomerLevelID"))
				{
					writer.Write(" selected");
				}
				writer.Write(">" + DB.RowField(row,"Name") + "</option>");
			}
			dsst.Dispose();
			writer.Write("</select>\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");

			writer.Write("<tr valign=\"middle\">\n");
			writer.Write("<td width=\"25%\" align=\"right\" valign=\"top\">Message Body:&nbsp;&nbsp;</td>\n");
			writer.Write("<td align=\"left\">\n");
			writer.Write("<textarea style=\"height: 60em; width: 100%;\" cols=\"" + Common.AppConfig("Admin_TextareaWidth") + "\" rows=\"" + Common.AppConfig("Admin_TextareaHeight") + "\" id=\"MessageBodyText\" name=\"MessageBodyText\">" + Server.HtmlEncode(Common.Form("MessageBodyText")) + "</textarea>\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");

			writer.Write("<tr valign=\"middle\">\n");
			writer.Write("<td width=\"25%\" align=\"right\" valign=\"top\">Message Footer (From Topic: MailFooter):&nbsp;&nbsp;</td>\n");
			writer.Write("<td align=\"left\">\n");
			writer.Write(Footer);
			writer.Write("</td>\n");
			writer.Write("</tr>\n");

			writer.Write("<tr>\n");
			writer.Write("<td></td><td align=\"left\"><br>\n");
			writer.Write("<input type=\"submit\" value=\"Submit\" name=\"submit\">\n");
			writer.Write("</td>\n");
			writer.Write("</tr>\n");
			writer.Write("</form>\n");
			writer.Write("</table>\n");

			writer.Write("<hr size=1>");
			writer.Write("<div align=\"left\">\n");
			writer.Write("<b>USE THE FORM BELOW TO REMOVE SOMEONE FROM THE MAILING LIST:</b><br><br>\n");
			writer.Write("<form action=\"mailingmgr.aspx\" method=\"post\" id=\"Form2\" name=\"Form2\">\n");
			writer.Write("<b>Remove Customer E-Mail From Mailing List:</b> <input maxLength=\"100\" size=\"30\" name=\"RemoveEMail\" value=\"\">\n");
			writer.Write("&nbsp;&nbsp;<input type=\"submit\" value=\"Submit\" name=\"submit\">\n");
			writer.Write("</form>");
			writer.Write("</div>");

			writer.Write(Common.GenerateHtmlEditor("MessageBodyText"));
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
