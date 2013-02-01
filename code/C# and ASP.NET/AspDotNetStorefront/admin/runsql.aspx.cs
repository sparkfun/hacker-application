// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for runsql.
	/// </summary>
	public class runsql : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Run SQL Command";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			String SQL = Common.Form("SQL");
			if(Common.Form("IsSubmit") == "true")
			{
				try
				{
					if(SQL.Length != 0)
					{
						DB.ExecuteLongTimeSQL(SQL,1000);
						writer.Write("<b>COMMAND EXECUTED OK<br><br></b>");
					}
					else
					{
						writer.Write("<b>NO SQL INPUT<br><br></b>");
					}
				}
				catch(Exception ex)
				{
					writer.Write("<b><font color=red>ERROR: " + Common.GetExceptionDetail(ex,"<br>") + "</font></b><br><br>");
				}
			}

			writer.Write("Enter SQL Command Text Below:<br>");
			writer.Write("<form method=\"POST\" action=\"runsql.aspx\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">");
			writer.Write("  <p><textarea rows=\"50\" name=\"SQL\" cols=\"120\">" + SQL + "</textarea></p>\n");
			writer.Write("  <p><input type=\"submit\" value=\"Submit\" name=\"B1\"><input type=\"reset\" value=\"Reset\" name=\"B2\"></p>\n");
			writer.Write("</form>\n");
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
