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
	/// Summary description for dyop_addtocart.
	/// </summary>
	public class dyop_addtocart : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");
			
			Customer thisCustomer = new Customer();

			int PackID = Common.QueryStringUSInt("PackID");
			int ProductID = Common.QueryStringUSInt("ProductID");
			int VariantID = Common.QueryStringUSInt("VariantID");
			int CategoryID = Common.QueryStringUSInt("CategoryID");
			int SectionID = Common.QueryStringUSInt("SectionID");
			int Quantity = Common.QueryStringUSInt("Quantity");
			if(Quantity == 0)
			{
				Quantity = 1;
			}

			String ChosenColor = String.Empty;
			String ChosenColorSKUModifier = String.Empty;
			String ChosenSize = String.Empty;
			String ChosenSizeSKUModifier = String.Empty;
			String ChosenSize2 = String.Empty;
			String ChosenSizeSKUModifier2 = String.Empty;

			if(Common.QueryString("Color").Length != 0)
			{
				String[] ColorSel = Common.QueryString("Color").Split(',');
				try
				{
					ChosenColor = ColorSel[0];
				}
				catch {}
				try
				{
					ChosenColorSKUModifier = ColorSel[1];
				}
				catch {}
			}

			if(Common.QueryString("Size").Length != 0)
			{
				String[] SizeSel = Common.QueryString("Size").Split(',');
				try
				{
					ChosenSize = SizeSel[0];
				}
				catch {}
				try
				{
					ChosenSizeSKUModifier = SizeSel[1];
				}
				catch {}
			}

			if(Common.QueryString("Size2").Length != 0)
			{
				String[] SizeSel2 = Common.QueryString("Size2").Split(',');
				try
				{
					ChosenSize2 = SizeSel2[0];
				}
				catch {}
				try
				{
					ChosenSizeSKUModifier2 = SizeSel2[1];
				}
				catch {}
			}

			if(ChosenSize2.Length != 0)
			{
				ChosenSize += "," + ChosenSize2;
			}
			if(ChosenSizeSKUModifier2.Length != 0)
			{
				ChosenSizeSKUModifier += "," + ChosenSizeSKUModifier2;
			}
			
			CustomerSession sess = new CustomerSession(thisCustomer._customerID);
			if(ChosenSize.Length != 0)
			{
				sess.SetVal("ChosenSize",ChosenSize,System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
			}

			if(ChosenSize2.Length != 0)
			{
				sess.SetVal("ChosenSize2",ChosenSize2,System.DateTime.Now.AddMinutes(Common.SessionTimeout()));
			}
			sess =  null;

			if(Quantity > 0)
			{
				// add to custom cart:
				CustomCart cart = new CustomCart(thisCustomer._customerID,PackID,1);
				cart.AddItem(ProductID,VariantID,Quantity,ChosenColor,ChosenColorSKUModifier,ChosenSize,ChosenSizeSKUModifier);
			}

			String url = "dyop.aspx?type=" + Common.QueryString("type") + "&packid=" + PackID.ToString() + "&categoryID=" + CategoryID.ToString() + "&sectionid=" + SectionID.ToString();
			Response.Redirect(url);
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
