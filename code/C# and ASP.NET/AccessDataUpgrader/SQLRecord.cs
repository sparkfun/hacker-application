namespace AccessDataUpgrader
{
	/// <summary>
	/// Summary description for SQLRecord.
	/// </summary>
	public class SQLRecord
	{
		// Public integer property AdID.
		private int adID;
		public int AdID
		{
			get
			{
				return adID;
			}
			set
			{
				adID = value;
			}
		}
		
		// Public integer property CustomerID.
		private int customerID;
		public int CustomerID
		{
			get
			{
				return customerID;
			}
			set
			{
				customerID = value;
			}
		}
		
		// Public string property Headline.
		private string headline;
		public string Headline
		{
			get
			{
				return headline;
			}
			set
			{
				headline = value;
			}
		}
	}
}