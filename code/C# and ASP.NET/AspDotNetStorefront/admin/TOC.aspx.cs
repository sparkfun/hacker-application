using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Microsoft.Web.UI.WebControls;
using System.Text;
using System.Xml;
using System.Data.OleDb;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for TOC
	/// </summary>
	public class TOC : System.Web.UI.Page
	{
		protected TreeView TreeView1;
		protected System.Web.UI.WebControls.ImageButton ImageButton1;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlModel;
		public String JavaScriptCode;
		public String TopSellers;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				int CategoryID  = Common.QueryStringUSInt("CategoryID");
				int SectionID  = Common.QueryStringUSInt("SectionID");
				int ManufacturerID = Common.QueryStringUSInt("ManufacturerID");

				RenderTree();

				if(CategoryID != 0)
				{
					SelectNode(TreeView1.Nodes, "C" + CategoryID.ToString());
				}
				else if(SectionID != 0)
				{
					SelectNode(TreeView1.Nodes, "S" + SectionID.ToString());
				}
				else if(ManufacturerID != 0)
				{
					SelectNode(TreeView1.Nodes, "M" + ManufacturerID.ToString());
				}
			}					
		}

		void RenderTree()
		{

			//'TreeView1.CssClass = "MenuUnselected"
			TreeView1.AutoSelect = true;
			TreeView1.ShowLines = true;
			TreeView1.ShowPlus = true;
			TreeView1.ShowToolTip = false;
			TreeView1.SelectExpands = true;
			TreeView1.Nodes.Clear();
			TreeView1.BackColor =  System.Drawing.Color.FromArgb(225,225,225);

			TreeNode HomeNode = new TreeNode();
			HomeNode.Text = "Home";
			HomeNode.NavigateUrl = Common.IIF(Common.IsAdminSite, "splash.aspx", "default.aspx");
			HomeNode.Target = "content";
			HomeNode.ImageUrl = "images/icons/home.gif";
			//RootNode.Expanded = true;
			TreeView1.Nodes.Add(HomeNode);

			if(Common.IsAdminSite)
			{
				TreeNode OrdersNode = new TreeNode();
				OrdersNode.Text = "Orders";
				OrdersNode.NavigateUrl = "orders.aspx";
				OrdersNode.Target = "content";
				OrdersNode.ImageUrl = "images/icons/folderclosed.gif";
				OrdersNode.ExpandedImageUrl = "images/icons/folderopen.gif";
				OrdersNode.SelectedImageUrl = "images/icons/folderopen.gif";
				TreeView1.Nodes.Add(OrdersNode);
			}
			
			TreeNode CategoryNode = new TreeNode();
			CategoryNode.Text = Common.IIF(Common.IsAdminSite,"","Browse By ") + Common.AppConfig("CategoryPromptPlural");
			CategoryNode.NavigateUrl = Common.IIF(Common.IsAdminSite,"categories.aspx",String.Empty);
			CategoryNode.Target = "content";
			CategoryNode.ImageUrl = "images/icons/folderclosed.gif";
			CategoryNode.ExpandedImageUrl = "images/icons/folderopen.gif";
			CategoryNode.SelectedImageUrl = "images/icons/folderopen.gif";
			TreeView1.Nodes.Add(CategoryNode);
			LoadCategoryTree(CategoryNode.Nodes,0);

			TreeNode SectionNode = new TreeNode();
			SectionNode.Text = Common.IIF(Common.IsAdminSite,"","Browse By ") + Common.AppConfig("SectionPromptPlural");
			SectionNode.NavigateUrl = Common.IIF(Common.IsAdminSite,"sections.aspx",String.Empty);
			SectionNode.Target = "content";
			SectionNode.ImageUrl = "images/icons/folderclosed.gif";
			SectionNode.ExpandedImageUrl = "images/icons/folderopen.gif";
			SectionNode.SelectedImageUrl = "images/icons/folderopen.gif";
			TreeView1.Nodes.Add(SectionNode);
			LoadSectionTree(SectionNode.Nodes,0);

			if(Common.AppConfigBool("ShowManufacturerTree") || Common.IsAdminSite)
			{
				TreeNode ManufacturerNode = new TreeNode();
				ManufacturerNode.Text = "Manufacturers";
				ManufacturerNode.NavigateUrl = Common.IIF(Common.IsAdminSite,"manufacturers.aspx","manufacturers.aspx");
				ManufacturerNode.Target = "content";
				ManufacturerNode.ImageUrl = "images/icons/folderclosed.gif";
				ManufacturerNode.ExpandedImageUrl = "images/icons/folderopen.gif";
				ManufacturerNode.SelectedImageUrl = "images/icons/folderopen.gif";
				TreeView1.Nodes.Add(ManufacturerNode);
				LoadManufacturerTree(ManufacturerNode.Nodes);
			}

			if(!Common.IsAdminSite)
			{

				Topic t = new Topic("CustomerServiceTreeMenu");
				String CustomerServiceTreeMenu = t._contents.Replace("<BR>","<br>").Replace("<p>",String.Empty).Replace("</p>",String.Empty).Trim().Replace("<br>","^");

				if(CustomerServiceTreeMenu.Length != 0)
				{
					TreeNode CustomerServiceNode = new TreeNode();
					CustomerServiceNode.Text = "Customer Service";
					CustomerServiceNode.NavigateUrl = "t-service.aspx";
					CustomerServiceNode.Target = "content";
					CustomerServiceNode.ImageUrl = "images/icons/folderclosed.gif";
					CustomerServiceNode.ExpandedImageUrl = "images/icons/folderopen.gif";
					CustomerServiceNode.SelectedImageUrl = "images/icons/folderopen.gif";
					//RootNode.Expanded = true;
					TreeView1.Nodes.Add(CustomerServiceNode);
					LoadCustomerServiceTree(CustomerServiceNode.Nodes,CustomerServiceTreeMenu);
				}
			}

		}


		static private void LoadCategoryTree(TreeNodeCollection Nodes, int ForParentCategoryID)
		{
			if(Common.IsAdminSite)
			{
				String sql = String.Empty;
				if(ForParentCategoryID == 0)
				{
					sql = "select * from Category  " + DB.GetNoLock() + " where (parentCategoryid=0 or ParentCategoryID IS NULL) and published<>0 and deleted=0 order by DisplayOrder,Name";
				}
				else
				{
					sql = "select * from Category  " + DB.GetNoLock() + " where parentCategoryid=" + ForParentCategoryID.ToString() + " and published<>0 and deleted=0 order by DisplayOrder,Name";
				}
				IDataReader rs = DB.GetRS(sql);

				while(rs.Read())
				{
					TreeNode Node = new TreeNode();
					Node.Text = DB.RSField(rs,"Name");
					Node.NavigateUrl = "editcategory.aspx?categoryid=" + DB.RSFieldInt(rs,"CategoryID").ToString();
					Node.Target = "content";
					Node.ImageUrl = "images/icons/folderclosed.gif";
					Node.ExpandedImageUrl = "images/icons/folderopen.gif";
					Node.SelectedImageUrl = "images/icons/folderopen.gif";
					Nodes.Add(Node);
					if(Common.CategoryHasSubs(DB.RSFieldInt(rs,"CategoryID")))
					{
						LoadCategoryTree(Node.Nodes,DB.RSFieldInt(rs,"CategoryID"));
					}
				}
				rs.Close();
			}
			else
			{
				String sql = String.Empty;
				if(ForParentCategoryID == 0)
				{
					sql = "select * from Category  " + DB.GetNoLock() + " where (parentCategoryid=0 or ParentCategoryID IS NULL) and published<>0 and deleted=0 order by DisplayOrder,Name";
				}
				else
				{
					sql = "select * from Category  " + DB.GetNoLock() + " where parentCategoryid=" + ForParentCategoryID.ToString() + " and published<>0 and deleted=0 order by DisplayOrder,Name";
				}
				IDataReader rs = DB.GetRS(sql);

				while(rs.Read())
				{
					TreeNode Node = new TreeNode();
					Node.Text = DB.RSField(rs,"Name");
					Node.NavigateUrl = SE.MakeCategoryLink(DB.RSFieldInt(rs,"CategoryID"),String.Empty);
					Node.Target = "content";
					Node.ImageUrl = "images/icons/folderclosed.gif";
					Node.ExpandedImageUrl = "images/icons/folderopen.gif";
					Node.SelectedImageUrl = "images/icons/folderopen.gif";
					Nodes.Add(Node);
					if(Common.CategoryHasSubs(DB.RSFieldInt(rs,"CategoryID")))
					{
						LoadCategoryTree(Node.Nodes,DB.RSFieldInt(rs,"CategoryID"));
					}
				}
				rs.Close();
			}
		}
			
		static private void LoadSectionTree(TreeNodeCollection Nodes, int ForParentSectionID)
		{
			if(Common.IsAdminSite)
			{
				String sql = String.Empty;
				if(ForParentSectionID == 0)
				{
					sql = "select * from [Section]  " + DB.GetNoLock() + " where (parentSectionid=0 or ParentSectionID IS NULL) and deleted=0 order by DisplayOrder,Name";
				}
				else
				{
					sql = "select * from [Section]  " + DB.GetNoLock() + " where parentSectionid=" + ForParentSectionID.ToString() + " and deleted=0 order by DisplayOrder,Name";
				}
				IDataReader rs = DB.GetRS(sql);

				while(rs.Read())
				{
					TreeNode Node = new TreeNode();
					Node.Text = DB.RSField(rs,"Name");
					Node.NavigateUrl = "editsection.aspx?sectionid=" + DB.RSFieldInt(rs,"SectionID").ToString();
					Node.Target = "content";
					Node.ImageUrl = "images/icons/folderclosed.gif";
					Node.ExpandedImageUrl = "images/icons/folderopen.gif";
					Node.SelectedImageUrl = "images/icons/folderopen.gif";
					Nodes.Add(Node);
					if(Common.SectionHasSubs(DB.RSFieldInt(rs,"SectionID")))
					{
						LoadSectionTree(Node.Nodes,DB.RSFieldInt(rs,"SectionID"));
					}
				}
				rs.Close();
			}
			else
			{
				String sql = String.Empty;
				if(ForParentSectionID == 0)
				{
					sql = "select * from [Section]  " + DB.GetNoLock() + " where (parentSectionid=0 or ParentSectionID IS NULL) and published<>0 and deleted=0 order by DisplayOrder,Name";
				}
				else
				{
					sql = "select * from [Section]  " + DB.GetNoLock() + " where parentSectionid=" + ForParentSectionID.ToString() + " and published<>0 and deleted=0 order by DisplayOrder,Name";
				}
				IDataReader rs = DB.GetRS(sql);

				while(rs.Read())
				{
					TreeNode Node = new TreeNode();
					Node.Text = DB.RSField(rs,"Name");
					Node.NavigateUrl = SE.MakeSectionLink(DB.RSFieldInt(rs,"SectionID"),String.Empty);
					Node.Target = "content";
					Node.ImageUrl = "images/icons/folderclosed.gif";
					Node.ExpandedImageUrl = "images/icons/folderopen.gif";
					Node.SelectedImageUrl = "images/icons/folderopen.gif";
					Nodes.Add(Node);
					if(Common.SectionHasSubs(DB.RSFieldInt(rs,"SectionID")))
					{
						LoadSectionTree(Node.Nodes,DB.RSFieldInt(rs,"SectionID"));
					}
				}
				rs.Close();
			}
		}
	
		static private void LoadManufacturerTree(TreeNodeCollection Nodes)
		{
			if(Common.IsAdminSite)
			{
				StringBuilder tmpS = new StringBuilder(5000);
				String sql = String.Empty;
				sql = "select * from Manufacturer  " + DB.GetNoLock() + " where deleted=0 order by DisplayOrder,Name";
				IDataReader rs = DB.GetRS(sql);

				while(rs.Read())
				{
					TreeNode Node = new TreeNode();
					Node.Text = DB.RSField(rs,"Name");
					Node.NavigateUrl = "editmanufacturer.aspx?manufacturerid=" + DB.RSFieldInt(rs,"ManufacturerID").ToString();
					Node.Target = "content";
					Node.ImageUrl = "images/icons/folderclosed.gif";
					Node.ExpandedImageUrl = "images/icons/folderopen.gif";
					Node.SelectedImageUrl = "images/icons/folderopen.gif";
					Nodes.Add(Node);
				}
				rs.Close();
			}
			else
			{
				StringBuilder tmpS = new StringBuilder(5000);
				String sql = String.Empty;
				sql = "select * from Manufacturer  " + DB.GetNoLock() + " where deleted=0 order by DisplayOrder,Name";
				IDataReader rs = DB.GetRS(sql);

				while(rs.Read())
				{
					TreeNode Node = new TreeNode();
					Node.Text = DB.RSField(rs,"Name");
					Node.NavigateUrl = SE.MakeManufacturerLink(DB.RSFieldInt(rs,"ManufacturerID"),String.Empty);
					Node.Target = "content";
					Node.ImageUrl = "images/icons/folderclosed.gif";
					Node.ExpandedImageUrl = "images/icons/folderopen.gif";
					Node.SelectedImageUrl = "images/icons/folderopen.gif";
					Nodes.Add(Node);
				}
				rs.Close();
			}
		}

		static private void LoadCustomerServiceTree(TreeNodeCollection Nodes, String ServiceMenuOptions)
		{

			if(ServiceMenuOptions.Length != 0)
			{
				try
				{
					foreach(String s in ServiceMenuOptions.Split('^'))
					{
						String[] s2 = s.Split('|');
						//XmlDocument xmlDoc = new XmlDocument();
						//xmlDoc.LoadXml(CustomerServiceTreeMenu);
						//XmlNodeList nodeList = xmlDoc.SelectNodes(@"//node");
						//foreach (XmlNode node in nodeList)
						//{
						TreeNode NewNode = new TreeNode();
						NewNode.Text = s2[0];
						NewNode.NavigateUrl = s2[1];
						NewNode.Target = "content";
						NewNode.ImageUrl = "images/icons/folderclosed.gif";
						NewNode.ExpandedImageUrl = "images/icons/folderopen.gif";
						NewNode.SelectedImageUrl = "images/icons/folderopen.gif";
						Nodes.Add(NewNode);
						//}
					}

				}
				catch(Exception ex)
				{
					TreeNode NewNode = new TreeNode();
					NewNode.Text = Common.XmlEncode("Error: " + ex.Message);
					NewNode.NavigateUrl = String.Empty;
					NewNode.Target = "content";
					NewNode.ImageUrl = "images/icons/folderclosed.gif";
					NewNode.ExpandedImageUrl = "images/icons/folderopen.gif";
					NewNode.SelectedImageUrl = "images/icons/folderopen.gif";
					Nodes.Add(NewNode);
				}
			}
		}

		bool SelectNode(TreeNodeCollection Nodes, String ID)
		{
			foreach (TreeNode Node in Nodes)
			{
				if (ID.Equals(Node.ID))
				{
					TreeView1.SelectedNodeIndex = Node.GetNodeIndex();
					return true;
				}
				else if (SelectNode(Node.Nodes, ID))
				{
					Node.Expanded = true;
					return true;
				}
			}
			return false;
		}


		TreeNode ExpandNode(TreeNodeCollection Nodes, String s)
		{
			TreeNode Node;
			if (!s.Equals("")) 
			{
				int i = s.IndexOf(".");
				if (i > 0)      
				{
					String s1 = s.Substring(0, i);
					String s2 = s.Substring(i + 1);
					Node = Nodes[Convert.ToInt32(s1)];
					Node.Expanded = true;
					return ExpandNode(Node.Nodes, s2);
				}
				else
				{
					Node = Nodes[Convert.ToInt32(s)];
					Node.Expanded = true;
					return Node;
				}
			}
			return null;
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

		private void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			Response.Redirect("selectmodel.aspx");
		}

	}
}
