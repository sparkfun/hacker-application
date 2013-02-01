using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Resources;
using System.Threading;
using System.Globalization;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AspDotNetStorefrontCommon;
using AspDotNetStorefrontQuickBooks;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for orders.
	/// </summary>
	public class orders : SkinBase
	{
		private string OrderByFields = "IsNew desc, OrderDate desc";
    
		private int m_FirstOrderNumber = 0;

		protected DataSet dsAffiliate = null;
		protected DataSet dsCouponCode = null;
		protected DataSet dsState = null;
		protected DataSet dsSelected = null;

		protected eWorld.UI.CalendarPopup dateEnd;
		protected System.Web.UI.WebControls.RadioButtonList rbEasyRange;
		protected System.Web.UI.WebControls.RadioButton rbRange;
		protected System.Web.UI.WebControls.TextBox txtEmail;
		protected System.Web.UI.WebControls.TextBox txtCreditCardNumber;
		protected System.Web.UI.WebControls.TextBox txtCustomerPhone;
		protected System.Web.UI.WebControls.TextBox txtCustomerName;
		protected System.Web.UI.WebControls.TextBox txtCompany;
		protected System.Web.UI.WebControls.DropDownList ddPaymentMethod;
		protected System.Web.UI.WebControls.DropDownList ddAffiliate;
		protected System.Web.UI.WebControls.DropDownList ddCouponCode;
		protected System.Web.UI.WebControls.DropDownList ddShippingState;
		protected System.Web.UI.WebControls.RadioButtonList rbNewOrdersOnly;
		protected System.Web.UI.WebControls.Button btnSubmit;
		protected System.Web.UI.WebControls.Button btnReset;
		protected eWorld.UI.NumericBox txtOrderNumber;
		protected eWorld.UI.NumericBox txtCustomerID;
		protected System.Web.UI.WebControls.DataList dlSelected;
		protected System.Web.UI.WebControls.Panel pnlResults;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Image Image1;
		protected System.Web.UI.WebControls.Button btnQBExport;
		protected System.Web.UI.WebControls.Label lblCulture;
		protected eWorld.UI.CalendarPopup dateStart;
  
		public string HeaderImage
		{
			get
			{
				return String.Format("skins/Skin_{0}/images/orders.gif",_siteID);
			}
		}

		public string NewImage
		{
			get
			{
				return String.Format("skins/Skin_{0}/images/new.gif",_siteID);
			}
		}

		public int FirstOrderNumber
		{
			get
			{
				return m_FirstOrderNumber;
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad();
			DataRow dr;
			DoLocalization();

			if (! Page.IsPostBack) // Only initialize once
			{
				IDataReader rsd = DB.GetRS("Select min(OrderDate) as MinDate from orders " + DB.GetNoLock());
				DateTime MinOrderDate = Localization.ParseUSDateTime("1/1/1990");
				if(rsd.Read())
				{
					MinOrderDate = DB.RSFieldDateTime(rsd,"MinDate");
				}
				rsd.Close();
				dateStart.SelectedDate = MinOrderDate;
				dateEnd.SelectedDate = System.DateTime.Now;
				
				rbEasyRange.SelectedIndex = 0; // Set to "Today"
        
				dsAffiliate = DB.GetDS("select AffiliateID,Name from affiliate  " + DB.GetNoLock() + " order by name",false);
				dr = dsAffiliate.Tables[0].NewRow();
				dr["AffiliateID"] = 0;
				dr["Name"]="-";
				dsAffiliate.Tables[0].Rows.InsertAt(dr,0);
				ddAffiliate.DataSource = dsAffiliate;
				ddAffiliate.DataBind();
				dsAffiliate.Dispose();

				dsCouponCode = DB.GetDS("select CouponCode from Coupon  " + DB.GetNoLock() + " order by CouponCode",false);
				dr = dsCouponCode.Tables[0].NewRow();
				dr["CouponCode"] = "-";
				dsCouponCode.Tables[0].Rows.InsertAt(dr,0);
				ddCouponCode.DataSource = dsCouponCode;
				ddCouponCode.DataBind();
				dsCouponCode.Dispose();

				dsState = DB.GetDS("select Abbreviation,Name from state  " + DB.GetNoLock() + " order by displayorder,name",true,System.DateTime.Now.AddHours(1));
				dr = dsState.Tables[0].NewRow();
				dr["Abbreviation"] = "-";
				dr["Name"]="SELECT ONE";
				dsState.Tables[0].Rows.InsertAt(dr,0);
				ddShippingState.DataSource = dsState;
				ddShippingState.DataBind();
				dsState.Dispose();
			}
			MakeSearch().Dispose();
		}


		private DataSet MakeSearch()
		{
			string sql = "select * from orders  " + DB.GetNoLock() + " where " + WhereClause() + DateClause()  + " order by " + OrderByFields;
			dsSelected = DB.GetDS(sql,false);
      
			dlSelected.DataSource = dsSelected;
      
			lblError.Text = String.Empty;
			if(Common.AppConfigBool("Admin_ShowReportSQL"))
			{
				lblError.Text = "SQL=" + sql;
			}

			if (dsSelected.Tables[0].Rows.Count > 0)
			{
				m_FirstOrderNumber = (Int32)dsSelected.Tables[0].Rows[0]["OrderNumber"];
				pnlResults.Visible = true;
			}
			else
			{
				lblError.Text += "<br><br>" + "No Orders Found";
				pnlResults.Visible = false;
			}
       
			Page.DataBind();
			return dsSelected;
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
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			this.btnQBExport.Click += new System.EventHandler(this.btnQBExport_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnReset_Click(object sender, System.EventArgs e)
		{ 
			txtOrderNumber.Text = String.Empty;
			txtCustomerID.Text = String.Empty;
			txtEmail.Text = String.Empty;
			txtCreditCardNumber.Text = String.Empty;
			txtCustomerPhone.Text  = String.Empty;
			txtCustomerName.Text = String.Empty;
			txtCompany.Text = String.Empty;
			ddPaymentMethod.SelectedIndex = 0;
			ddAffiliate.SelectedIndex = 0;
			ddCouponCode.SelectedIndex = 0;
			ddShippingState.SelectedIndex = 0;
			dateStart.Clear();
			dateEnd.Clear();
			rbRange.Checked = false;
			rbEasyRange.SelectedIndex = 0;
			rbNewOrdersOnly.SelectedValue = "1";
			MakeSearch().Dispose();
		}

		/// <summary>
		/// Calculates the Where clause for the date portion of the search.
		/// </summary>
		public string DateClause()
		{
			string result = String.Empty;
			DateTime startDate = DateTime.Now;
			DateTime endDate = DateTime.Now;
      
			if (rbRange.Checked) //Use Dates above Range
			{
				if (!(dateStart.ValidDateEntered && dateEnd.ValidDateEntered))  //blank dates
				{
					return String.Empty;
				}
				if (! dateEnd.ValidDateEntered)
				{
					dateEnd.SelectedDate = DateTime.Today.AddDays(1);
				}
				if (dateStart.SelectedDate.CompareTo(dateEnd.SelectedDate) > 0) //Flip them
				{
					endDate=dateStart.SelectedDate;
					dateStart.SelectedDate = dateEnd.SelectedDate;
					dateEnd.SelectedDate = endDate;
				}
        
				startDate = dateStart.SelectedDate;
				endDate = dateEnd.SelectedDate;

			}
			else
			{
				switch (rbEasyRange.SelectedValue)
				{
					case  "Today" :
					{
						startDate = DateTime.Today;
						endDate = DateTime.Today.AddDays(1);
						break;
					}
					case "Yesterday" :
					{
						startDate = DateTime.Today.AddDays(-1);
						endDate = DateTime.Today;
						break;
					}
					case "ThisWeek" :
					{
						startDate = DateTime.Today.AddDays(-((int)DateTime.Today.DayOfWeek));
						endDate = startDate.AddDays(6);
						break;
					}
					case "LastWeek" :
					{
						startDate = DateTime.Today.AddDays(-((int)DateTime.Today.DayOfWeek) -7);
						endDate = startDate.AddDays(6);
						break;
					}
					case "ThisMonth" :
					{
						startDate = DateTime.Today.AddDays(1-DateTime.Today.Day);
						endDate = startDate.AddMonths(1);
						break;
					}
					case "LastMonth" :
					{
						startDate = DateTime.Today.AddMonths(-1);
						startDate = startDate.AddDays(1-startDate.Day);
						endDate = startDate.AddMonths(1);
						break;
					}
					case "ThisYear" :
					{
						startDate = DateTime.Today.AddMonths(1-DateTime.Today.Month);
						startDate = startDate.AddDays(1-startDate.Day);
						endDate = startDate.AddYears(1);
						break;
					}
					case "LastYear" :
					{
						startDate = DateTime.Today.AddYears(-1);
						startDate = startDate.AddMonths(1-startDate.Month);
						startDate = startDate.AddDays(1-startDate.Day);
						endDate = startDate.AddYears(1);
						break;
					}
				}
			}
			result = String.Format(" and ((OrderDate>={0}) and (OrderDate < {1}))",DB.DateQuote(startDate.ToString()),DB.DateQuote(endDate.ToString()));
			return result;
		}

		/// <summary>
		/// Creates the Where Clause based on the Qualification fields.
		/// </summary>
		public string WhereClause()
		{
			string result = "1=1";
			string sQuery = " and ({0}={1})";

			if (ddAffiliate.SelectedValue != "0" && ddAffiliate.SelectedItem.Text.Length !=0)
			{
				result += String.Format(sQuery,"AffiliateID",ddAffiliate.SelectedValue);
			}
			if (ddCouponCode.SelectedValue != "-" && ddCouponCode.SelectedItem.Text.Length !=0)
			{
				result += String.Format(sQuery,"CouponCode",DB.SQuote(ddCouponCode.SelectedValue));
			}
			if (ddShippingState.SelectedValue != "-" && ddShippingState.SelectedItem.Text.Length !=0)
			{
				result += String.Format(sQuery,"ShippingState",DB.SQuote(ddShippingState.SelectedValue));
			}
			if (rbNewOrdersOnly.SelectedValue == "1")
			{
				result += String.Format(sQuery,"IsNew",1);
			}
			if (txtEmail.Text.Trim().Length != 0)
			{
				result += String.Format(" and (email like {0})",DB.SQuote("%" + txtEmail.Text + "%"));
			}
			if (txtCustomerID.Text.Trim().Length != 0)
			{
				result += String.Format(sQuery,"CustomerID",txtCustomerID.Text);
			}
			if (txtOrderNumber.Text.Trim().Length != 0)
			{
				result += String.Format(sQuery,"OrderNumber",txtOrderNumber.Text);
			}
			if (txtCreditCardNumber.Text.Trim().Length != 0)
			{
				result += String.Format(sQuery,"CardNumber",DB.SQuote(Common.MungeString(txtCreditCardNumber.Text.Trim())));
			}
			if (txtCustomerPhone.Text.Trim().Length != 0)
			{
				result += String.Format(" and (Phone like {0} or ShippingPhone like {0} or BillingPhone like {0})","%" + txtCustomerPhone.Text.Trim() + "%");
			}
			if (txtCustomerName.Text.Length !=0)
			{
				result += String.Format(" and ((FirstName + ' ' + LastName) like {0})",DB.SQuote(txtCustomerName.Text));
			}
			if (txtCompany.Text.Length !=0)
			{
				result += String.Format(" and (ShippingCompany like {0} or BillingCompany like {0})",DB.SQuote("%" + txtCompany.Text + "%"));
			}
			if (ddPaymentMethod.SelectedValue != "-")
			{
				switch (ddPaymentMethod.SelectedValue.ToUpper().Replace(" ",String.Empty))
				{
					case "CREDITCARD" :
					{
						result += String.Format(" and (PaymentMethod={0} or (PaymentGateway is not null and upper(PaymentGateway)<>'PAYPAL'))",DB.SQuote(ddPaymentMethod.SelectedValue));
						break;
					}
					case "PAYPAL" :
					{
						result += String.Format(" and (PaymentMethod={0} or upper(PaymentGateway)={1})",DB.SQuote(ddPaymentMethod.SelectedValue),DB.SQuote("PAYPAL"));
						break;
					}
					case "PURCHASEORDER" :
					{
						result += String.Format(sQuery,"PaymentMethod",DB.SQuote(ddPaymentMethod.SelectedValue));
						break;
					}
					case "REQUESTQUOTE" :
					{
						result += String.Format(" and  (PaymentMethod={0} or QuoteCheckout<>0)",DB.SQuote(ddPaymentMethod.SelectedValue));
						break;
					}
					case "CHECK" :
					{
						result += String.Format(sQuery,"PaymentMethod",DB.SQuote(ddPaymentMethod.SelectedValue));
						break;
					}
					case "ECHECK" :
					{
						result += String.Format(sQuery,"PaymentMethod",DB.SQuote(ddPaymentMethod.SelectedValue));
						break;
					}
					case "MICROPAY" :
					{
						result += String.Format(sQuery,"PaymentMethod",DB.SQuote(ddPaymentMethod.SelectedValue));
						break;
					}
				}
			}
			return result;
		}

		private void DoLocalization()
		{
			//Localization
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(thisCustomer._localeSetting);
      
			SectionTitle = "Order Summary";
      
			btnSubmit.Text = "Submit";
			btnReset.Text = "Reset";

			dateStart.Culture = Thread.CurrentThread.CurrentUICulture; 
			dateEnd.Culture = Thread.CurrentThread.CurrentUICulture; 
			dateStart.ClearDateText = "Clear Date";
			dateEnd.ClearDateText = "Clear Date";
			dateStart.GoToTodayText = "Todays Date";
			dateEnd.GoToTodayText = "Todays Date";
      
			rbRange.Text = "Use Date Range Above";

			for (int i=0;i<rbEasyRange.Items.Count;i++)
			{
				rbEasyRange.Items[i].Text = rbEasyRange.Items[i].Text;
			}
		}

		private void btnQBExport_Click(object sender, System.EventArgs e)
		{
			DataSet ds = MakeSearch();    
			QBOrder qbo = new QBOrder();
			string qbxml = qbo.CreateReceiptXml(ds);   
			ds.Dispose();
		
			//Send the XML
			Response.AddHeader("content-disposition","attachment; filename=" + HttpUtility.UrlEncode("QBXML_"+DateTime.Now.ToString("yyyyMMddhhmmss")+".qbxml"));
			Response.ContentType = "text/qbxml";
			Response.Write(qbxml);
			Response.Flush();
			Response.End();
		
		}
	}
}
