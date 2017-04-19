using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroup.Website
{
	/// <summary>
	/// Display page footer.
	/// </summary>
	public class Footer : UserControl
	{
		public Label lblCurrentYear;

		public void Page_Load(Object sender, EventArgs e)
		{
			// Get current year as string.
			string year = DateTime.Now.Year.ToString();

			// Set label text to year value.
			lblCurrentYear.Text = year;
		}
	}
}