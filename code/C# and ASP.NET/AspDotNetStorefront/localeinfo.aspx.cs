using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Globalization;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for localeinfo.
	/// </summary>
	public class localeinfo : SkinBase
	{
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label CustomerLocale;
		protected System.Web.UI.WebControls.Label StoreLocale;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label WebDateFormat;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label ToNativeShortDateString;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label ToUSShortDateString;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.Label Label19;
		protected System.Web.UI.WebControls.Label Label20;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label22;
		protected System.Web.UI.WebControls.Label Label21;
		protected System.Web.UI.WebControls.Label Label23;
		protected System.Web.UI.WebControls.Label Label25;
		protected System.Web.UI.WebControls.Label Label26;
		protected System.Web.UI.WebControls.Label Label27;
		protected System.Web.UI.WebControls.Label Label28;
		protected System.Web.UI.WebControls.Label Label29;
		protected System.Web.UI.WebControls.Label Label30;
		protected System.Web.UI.WebControls.Label Label31;
		protected System.Web.UI.WebControls.Label Label32;
		protected System.Web.UI.WebControls.Label Label33;
		protected System.Web.UI.WebControls.Label Label34;
		protected System.Web.UI.WebControls.Label Label35;
		protected System.Web.UI.WebControls.Label Label36;
		protected System.Web.UI.WebControls.Label Label37;
		protected System.Web.UI.WebControls.Label Label38;
		protected System.Web.UI.WebControls.Label Label39;
		protected System.Web.UI.WebControls.Label Label24;
		protected System.Web.UI.WebControls.Label Label41;
		protected System.Web.UI.WebControls.Label Label42;
		protected System.Web.UI.WebControls.Label Label43;
		protected System.Web.UI.WebControls.Label Label44;
		protected System.Web.UI.WebControls.Label Label45;
		protected System.Web.UI.WebControls.Label Label46;
		protected System.Web.UI.WebControls.Label Label40;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			base._disableLeftAndRightCols = true;
			SectionTitle = "Locale Test Page";

			StoreLocale.Text = Localization.GetWebConfigLocale();
			CustomerLocale.Text = thisCustomer._localeSetting;
			WebDateFormat.Text = System.DateTime.Now.ToShortDateString();
			ToNativeShortDateString.Text = Localization.ToNativeShortDateString(System.DateTime.Now);
			ToUSShortDateString.Text = Localization.ToUSShortDateString(System.DateTime.Now);
			Label9.Text = Localization.ToNativeDateTimeString(System.DateTime.Now);
			Label10.Text = Localization.ToUSDateTimeString(System.DateTime.Now);
			Label12.Text = System.DateTime.Now.ToString();

			int i = 123456;
			Label14.Text = i.ToString(); 
			Label16.Text = Localization.IntStringForDB(i); 

			Single f = 123456.7890F;
			Label18.Text = f.ToString();
			Label20.Text = Localization.SingleStringForDB(f);

			Double g = 123456.7890F;
			Label22.Text = g.ToString();
			Label24.Text = Localization.DoubleStringForDB(g);

			Decimal h = 123456.7890M;
			Label26.Text = h.ToString();
			Label28.Text = Localization.DecimalStringForDB(h);

			Label30.Text = Localization.CurrencyStringForDisplay(h);
			Label32.Text = Localization.CurrencyStringForDisplay(h);
			Label34.Text = Localization.CurrencyStringForDisplayUS(h);
			Label36.Text = Localization.CurrencyStringForGateway(h);
			Label46.Text = Localization.CurrencyStringForDB(h);

			Label38.Text = Localization.ShortDateFormat();
			Label40.Text = Localization.CurrencySymbol();

			IDataReader rs = DB.GetRS("select cast(@@langid as varchar(5)) as L, convert(varchar(20),getdate(),1) as M");
			rs.Read();
			Label42.Text = DB.RSField(rs,"M");
			Label44.Text = DB.RSField(rs,"L");
			rs.Close();

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
