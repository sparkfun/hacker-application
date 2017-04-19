using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroupAdmin
{
	/// <summary>
	/// Summary description for Main.
	/// </summary>
	public class Main : Page
	{
		public Main()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public bool CheckDropDownList(DropDownList dropDownListControl)
		{
			if (dropDownListControl.SelectedItem.Value == null || dropDownListControl.SelectedItem.Value == "")
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public bool CheckTextBox(TextBox textBoxControl)
		{
			string textBox;
			textBox = textBoxControl.Text.Trim();

			if (textBox == null || textBox == "")
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public string ConvertToNumeric(Object o)
		{
			string sType = o.GetType().ToString();

			if (sType == "System.DBNull")
			{
				return o.ToString();
			}
			else
			{
				decimal dNumeric = Convert.ToDecimal(o);
				return dNumeric.ToString("n");
			}	
		}

		public string ConvertToMoney(Object o)
		{
			string sType = o.GetType().ToString();

			if (sType == "System.DBNull")
			{
				return "&nbsp;";
			}
			else
			{
				decimal dMoney = Convert.ToDecimal(o);
				return dMoney.ToString("c");
			}
		}

		public string ConvertToString(Object o)
		{
			string sType = o.GetType().ToString();

			if (sType == "System.DBNull")
			{
				return "&nbsp;";
			}
			else
			{
				return o.ToString();
			}
		}

		public string ConvertToStringPlusMinus(Object o)
		{
			string sType = o.GetType().ToString();

			if (sType == "System.DBNull")
			{
				return "&nbsp;";
			}
			else
			{
				return o.ToString() + " +/-";
			}
		}

		public string ConvertToYesNo(Object o)
		{
			bool blnYesNo = Convert.ToBoolean(o);

			if (blnYesNo == true)
			{
				return "Yes";
			}
			else
			{
				return "No";
			}
		}

		public void txtDescription_Validate750(Object sender, ServerValidateEventArgs args)
		{
			if (args.Value.Length > 750)
			{
				args.IsValid = false;
			}
			else
			{
				args.IsValid = true;
			}
		}

		public void txtDescription_Validate500(Object sender, ServerValidateEventArgs args)
		{
			if (args.Value.Length > 500)
			{
				args.IsValid = false;
			}
			else
			{
				args.IsValid = true;
			}
		}

		public void txtDescription_Validate250(Object sender, ServerValidateEventArgs args)
		{
			if (args.Value.Length > 250)
			{
				args.IsValid = false;
			}
			else
			{
				args.IsValid = true;
			}
		}

		public void txtDescription_Validate150(Object sender, ServerValidateEventArgs args)
		{
			if (args.Value.Length > 150)
			{
				args.IsValid = false;
			}
			else
			{
				args.IsValid = true;
			}
		}

		public string GetSqlExceptionDump(SqlException Ex)
		{
			StringBuilder sb = new StringBuilder("\n");
			sb.Append("<p>Database Error Details:</p>" + "\n");
			sb.Append("<ul>" + "\n");
			sb.Append(" <li>Class: " + Ex.Class + "</li>" + "\n");
			sb.Append(" <li>Error Number: " + Ex.Number + "</li>" + "\n");
			sb.Append(" <li>Errors: " + Ex.Errors + "</li>" + "\n");
			sb.Append(" <li>Help Link: " + Ex.HelpLink + "</li>" + "\n");
			sb.Append(" <li>Inner Exception: " + Ex.InnerException + "</li>" + "\n");
			sb.Append(" <li>Line Number: " + Ex.LineNumber + "</li>" + "\n");
			sb.Append(" <li>Message: " + Ex.Message + "</li>" + "\n");
			sb.Append(" <li>Procedure: " + Ex.Procedure + "</li>" + "\n");
			sb.Append(" <li>Server: " + Ex.Server + "</li>" + "\n");
			sb.Append(" <li>Source: " + Ex.Source + "</li>" + "\n");
			sb.Append(" <li>Stack Trace: " + Ex.StackTrace + "</li>" + "\n");
			sb.Append(" <li>State: " + Ex.State + "</li>" + "\n");
			sb.Append(" <li>Target Site: " + Ex.TargetSite + "</li>" + "\n");
			sb.Append("</ul>" + "\n");

			return sb.ToString();
		}
	}
}
