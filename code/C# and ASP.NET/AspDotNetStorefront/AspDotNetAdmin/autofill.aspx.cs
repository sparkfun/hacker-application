// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.IO;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for autofill.
	/// </summary>
	public class autofill : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");

			Customer thisCustomer = new Customer();
			String FormImageName = Common.QueryString("FormImageName");

			if(thisCustomer._isAdminUser)
			{
				int ProductID = Common.QueryStringUSInt("ProductID");
				Response.Write("<html><head><title>AutoFill Variants</title></head><body>\n");

				if(ProductID != 0)
				{
					String Summary = String.Empty;
					IDataReader rs = DB.GetRS("Select summary from product  " + DB.GetNoLock() + " where ProductID=" + ProductID.ToString());
					if(rs.Read())
					{
						Summary = DB.RSField(rs,"Summary");
					}
					rs.Close();
					if(Summary.Length != 0)
					{
						// summary must be in format: name skusuffix price cols
						String Summary2 = Summary.Replace("&nbsp;"," ").Replace("<p>","").Replace("</p>","").Replace("<br>","|").Replace("<BR>","|");
						foreach(String s in Summary2.Split('|'))
						{
							try
							{
								String stmp = s.Trim();
								while(stmp.IndexOf("  ") != -1)
								{
									stmp = stmp.Replace("  "," ");
								}
								stmp = stmp.Trim();
								if(stmp.Length != 0)
								{
									String[] sarray = stmp.Split(' ');
									String Price = sarray[sarray.GetUpperBound(0)];
									String SKUSuffix = sarray[sarray.GetUpperBound(0)-1];
									String Name = String.Empty;
									for(int i = 0; i<= sarray.GetUpperBound(0)-2; i++)
									{
										Name += sarray[i] + " ";
									}
									Name = Name.Trim();
									if(Price.Length != 0 && Name.Length != 0)
									{
										DB.ExecuteSQL("insert into productvariant(ProductID,Name,SKUSuffix,Price,Inventory,Published) values(" + ProductID.ToString() + "," + DB.SQuote(Name) + "," + DB.SQuote(SKUSuffix) + "," + Price + ",1000000,1)");
									}
									else
									{
										Response.Write("<p><b>Bad Line Format: " + s + "</b></p>");
									}
								}
							}
							catch
							{
								Response.Write("<p><b>Error On Line: " + s + ", SKIPPING LINE</b></p>");
							}
						}
					}
					else
					{
						Response.Write("<p><b>Product Summary Is Empty</b></p>");
					}
				}
				else
				{
					Response.Write("<p><b>No Product ID Specified</b></p>");
				}

				Response.Write("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
				Response.Write("opener.document.getElementById('" + FormImageName + "').src = '../images/spacer.gif';\n");
				Response.Write("self.close();\n");
				Response.Write("</script>\n");
			}

			Response.Write("<a href=\"javascript:self.close();\">Close</a>");
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
