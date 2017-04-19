using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ChrisEdwardsGroup.Website
{
	/// <summary>
	/// Summary description for FeaturedPropertyControl.
	/// </summary>
	public class FeaturedPropertyControl : Control, INamingContainer
	{
		// Public MLS Property
		private string mls;
		public string Mls
		{
			get {return mls;}
			set {mls = value;}
		}

		// Public PicturePath Property
		private string picturePath;
		public string PicturePath
		{
			get {return picturePath;}
			set {picturePath = value;}
		}

		// Public Price Property
		private double price;
		public double Price
		{
			get {return price;}
			set {price = value;}
		}

		// Public City Property
		private string city;
		public string City
		{
			get {return city;}
			set {city = value;}
		}

		protected override void CreateChildControls()
		{
			// Create DIV tag
			HtmlGenericControl divFeature = new HtmlGenericControl("div");
			divFeature.Attributes.Add("id", "feature");

			// Create Heading2 Tag
			HtmlGenericControl h2 = new HtmlGenericControl("h2");
			h2.InnerText = "Feature Property";

			// Create Anchor Tag 1
			HyperLink link1 = new HyperLink();
			link1.NavigateUrl = GetLinkPath();

			// Create Image Tag
			Image featImg = new Image();
			featImg.ImageUrl = GetPicturePath();
			featImg.Width = 150;
			featImg.Height = 120;
			featImg.AlternateText = "Click to view property.";

			// Add Image to Anchor Tag
			link1.Controls.Add(featImg);

			// Gets a NumberFormatInfo associated with the en-US culture.
			NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
			nfi.CurrencyDecimalDigits = 0;

			// Create Heading3 Tag
			HtmlGenericControl h3 = new HtmlGenericControl("h3");
			h3.InnerHtml = Price.ToString("C", nfi) + "<br />" + City + ", NC";

			// Create Paragraph Tag
			HtmlGenericControl p = new HtmlGenericControl("p");
			p.Attributes.Add("class", "center");

			// Create Anchor Tag 2
			HyperLink link2 = new HyperLink();
			link2.NavigateUrl = GetLinkPath();
			link2.Text = "(Click Here)";

			// Add Link to Paragraph Tag
			p.Controls.Add(link2);

			// Add controls to DIV tag
			divFeature.Controls.Add(h2);
			divFeature.Controls.Add(link1);
			divFeature.Controls.Add(h3);
			divFeature.Controls.Add(p);

			// Add DIV tag to controls collection
			Controls.Add(divFeature);
		}

		private string GetPicturePath()
		{
			// Get path as string
			string s = "/listings/" + Mls + "/" + PicturePath;
			// Return path
			return s;
		}

		private string GetLinkPath()
		{
			// Get path as string
			string s = "/raleigh-cary-real-estate/single-family-home.aspx?MLSID=" +
				Mls;
			// Return path
			return s;
		}
	}
}