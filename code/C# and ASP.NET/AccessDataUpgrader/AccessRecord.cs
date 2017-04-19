namespace AccessDataUpgrader
{
	/// <summary>
	/// Summary description for AccessRecord.
	/// </summary>
	public class AccessRecord
	{
		// Public integer property ID.
		private int id;
		public int ID
		{
			get
			{
				return id;
			}
			set
			{
				id = value;
			}
		}
		
		// Public integer property CustID.
		private int custID;
		public int CustID
		{
			get
			{
				return custID;
			}
			set
			{
				custID = value;
			}
		}
		
		// Public string property Title.
		private string title;
		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				title = value;
			}
		}
		
		// Public string array property Photos.
		private string[] photos = new string[5];
		public string[] Photos
		{
			get
			{
				return photos;
			}
			set
			{
				photos = value;
			}
		}
		
		// Public string property File.
		private string file;
		public string File
		{
			get
			{
				return file;
			}
			set
			{
				file = value;
			}
		}
	}
}