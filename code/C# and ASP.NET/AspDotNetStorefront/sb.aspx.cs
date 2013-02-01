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
using AspDotNetStorefrontCommon;


namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for sb.
	/// </summary>
	public class sb : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");
			
			Customer thisCustomer = new Customer();

			Response.Write("<html>\n");
			Response.Write("<head>\n");
			Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1252\">\n");
			Response.Write("<title>Product Browser</title>\n");
			Response.Write("<link rel=\"stylesheet\" href=\"skins/Skin_" + thisCustomer._skinID.ToString() + "/style.css\" type=\"text/css\">\n");
			Response.Write("<script type=\"text/javascript\" src=\"formValidate.js\"></script>\n");
			Response.Write("</head>\n");
			Response.Write("<body style=\"margin: 0px;\" bottommargin=\"0\" leftmargin=\"0\" marginheight=\"0\" marginwidth=\"0\" rightmargin=\"0\" topmargin=\"0\" bgcolor=\"#FFFFFF\" %ONBLUR% onLoad=\"self.focus()\">\n");
			Response.Write("<!-- PAGE INVOCATION: '%INVOCATION%' -->\n");

			//			Response.Write("<script type=\"text/javascript\">\n");
			//			Response.Write("\n");
			//			Response.Write("/***********************************************\n");
			//			Response.Write("* Highlight Table Cells Script- © Dynamic Drive DHTML code library (www.dynamicdrive.com)\n");
			//			Response.Write("* Visit http://www.dynamicDrive.com for hundreds of DHTML scripts\n");
			//			Response.Write("* This notice must stay intact for legal use\n");
			//			Response.Write("***********************************************/\n");
			//			Response.Write("\n");
			//			Response.Write("//Specify highlight behavior. \"TD\" to highlight table cells, \"TR\" to highlight the entire row:\n");
			//			Response.Write("var highlightbehavior=\"TD\"\n");
			//			Response.Write("\n");
			//			Response.Write("var ns6=document.getElementById&&!document.all\n");
			//			Response.Write("var ie=document.all\n");
			//			Response.Write("\n");
			//			Response.Write("function changeto(e,highlightcolor){\n");
			//			Response.Write("source=ie? event.srcElement : e.target\n");
			//			Response.Write("if (source.tagName==\"TABLE\")\n");
			//			Response.Write("return\n");
			//			Response.Write("while(source.tagName!=highlightbehavior && source.tagName!=\"HTML\")\n");
			//			Response.Write("source=ns6? source.parentNode : source.parentElement\n");
			//			Response.Write("if (source.style.backgroundColor!=highlightcolor&&source.id!=\"ignore\")\n");
			//			Response.Write("source.style.backgroundColor=highlightcolor\n");
			//			Response.Write("}\n");
			//			Response.Write("\n");
			//			Response.Write("function contains_ns6(master, slave) { //check if slave is contained by master\n");
			//			Response.Write("while (slave.parentNode)\n");
			//			Response.Write("if ((slave = slave.parentNode) == master)\n");
			//			Response.Write("return true;\n");
			//			Response.Write("return false;\n");
			//			Response.Write("}\n");
			//			Response.Write("\n");
			//			Response.Write("function changeback(e,originalcolor){\n");
			//			Response.Write("if (ie&&(event.fromElement.contains(event.toElement)||source.contains(event.toElement)||source.id==\"ignore\")||source.tagName==\"TABLE\")\n");
			//			Response.Write("return\n");
			//			Response.Write("else if (ns6&&(contains_ns6(source, e.relatedTarget)||source.id==\"ignore\"))\n");
			//			Response.Write("return\n");
			//			Response.Write("if (ie&&event.toElement!=source||ns6&&e.relatedTarget!=source)\n");
			//			Response.Write("source.style.backgroundColor=originalcolor\n");
			//			Response.Write("}\n");
			//			Response.Write("\n");
			//			Response.Write("</script>\n");
			
			int PackID = Common.QueryStringUSInt("PackID");
			int ProductID = Common.QueryStringUSInt("ProductID");
			int CategoryID = Common.QueryStringUSInt("CategoryID");
			int SectionID = Common.QueryStringUSInt("SectionID");

			if(ProductID != 0)
			{
				try 
				{
					CategoryID = Localization.ParseUSInt(Common.GetProductCategories(ProductID,true).Split(',')[0]);
				}
				catch
				{}
			}
			else
			{
				CategoryID = Common.QueryStringUSInt("CategoryID");
				if(CategoryID == 0)
				{
					IDataReader rsc = DB.GetRS("Select * from category  " + DB.GetNoLock() + " where deleted=0 and published=1 and categoryid in (select distinct(categoryid) from productcategory  " + DB.GetNoLock() + " where productid in (select productid from product  " + DB.GetNoLock() + " where IsAPack=1 and ShowInProductBrowser=1 and deleted=0 and published=1)) and ShowInProductBrowser=1 order by DisplayOrder");
					if(rsc.Read())
					{
						CategoryID = DB.RSFieldInt(rsc,"CategoryID");
					}
					rsc.Close();
				}
				else
				{
					ProductID = Common.GetFirstProduct(CategoryID,false,false);
				}
			}
			
			Response.Write("<div align=\"left\">\n");
			Response.Write("  <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\n");
			Response.Write("    <tr>\n");
			Response.Write("      <td width=\"26%\" valign=\"top\" align=\"left\">\n");
			Response.Write("        <div align=\"left\">\n");
			//Response.Write("<b>Styles:</b><br>");
			String CatList = Common.GetListForStyleBrowser(PackID,0,Common.QueryString("IsFull").ToLower() == "true");
			bool anyFound = CatList.Length != 0;
			if(anyFound)
			{
				Response.Write(CatList);
				//			Response.Write("          <table onMouseover=\"changeto(event, '#" + Common.AppConfig("ProductBrowserHoverColor") + "')\" onMouseout=\"changeback(event, 'white')\" border=\"0\" cellpadding=\"2\" cellspacing=\"1\" width=\"100%\" style=\"border-width: 1px; border-style: solid; border-color: #" + Common.AppConfig("ProductBrowserHoverColor") + "\">\n");
				//			IDataReader rs = DB.GetRS("Select * from category  " + DB.GetNoLock() + " where deleted=0 and published=1 and categoryid in (select distinct(categoryid) from productcategory  " + DB.GetNoLock() + " where productid in (select productid from product  " + DB.GetNoLock() + " where IsAPack=0 and IsAKit=0 and ShowInProductBrowser=1 and deleted=0 and published=1)) and ShowInProductBrowser=1 order by DisplayOrder");
				//			bool anyFound = false;
				//			String BrdClr = String.Empty;
				//			while(rs.Read())
				//			{
				//				anyFound = true;
				//				BrdClr += "document.getElementById('TD_" + DB.RSFieldInt(rs,"CategoryID").ToString() + "').style.border='0 dashed transparent';\n";
				//				BrdClr += "document.getElementById('TD_" + DB.RSFieldInt(rs,"CategoryID").ToString() + "').style.background='#FFFFFF';\n";
				//				Response.Write("            <tr>\n");
				//				Response.Write("              <td id=\"TD_" + DB.RSFieldInt(rs,"CategoryID").ToString() + "\" style=\"cursor: hand; background:#" + Common.IIF(CategoryID == DB.RSFieldInt(rs,"CategoryID") , "" + Common.AppConfig("ProductBrowserHoverColor") + "" , "FFFFFF") + "; border-color: #000000; border-style: dashed; border-width: " + Common.IIF(CategoryID == DB.RSFieldInt(rs,"CategoryID") , "1" , "0") + "px;\" height=\"30\" width=\"100%\" align=\"center\" onClick=\"javascript:ClearAllBorders();document.getElementById('TD_" + DB.RSFieldInt(rs,"CategoryID").ToString() + "').style.border='1 dashed black';document.getElementById('TD_" + DB.RSFieldInt(rs,"CategoryID").ToString() + "').style.background='#" + Common.AppConfig("ProductBrowserHoverColor") + "';document.getElementById('pb').src='pb.aspx?type=" + Common.QueryString("type") + "&packid=" + PackID.ToString() + "&CategoryID=" + DB.RSFieldInt(rs,"CategoryID").ToString() + "&sectionid=" + SectionID.ToString() + "&isfull=" + Common.QueryString("isfull") + "';\"><a class=\"sb_nav\" href=\"javascript:void(0);\" onClick=\"javascript:document.getElementById('pb').src='pb.aspx?type=" + Common.QueryString("type") + "&PackID=" + PackID.ToString() + "&CategoryID=" + DB.RSFieldInt(rs,"CategoryID").ToString() + "&sectionid=" + SectionID.ToString() + "&isfull=" + Common.QueryString("isfull") + "';\">" + DB.RSField(rs,"Name") + "</a></td>\n");
				//				Response.Write("            </tr>\n");
				//			}
				//			rs.Close();
			}
			else
			{
				Response.Write("NO CATEGORIES FOUND FOR THIS PACK");
			}
			//Response.Write("          </table>\n");
			Response.Write("        </div>\n");
			Response.Write("      </td>\n");
			Response.Write("      <td width=\"1%\" valign=\"top\" align=\"left\"></td>\n");
			Response.Write("      <td width=\"73%\" valign=\"top\" align=\"left\">");
			
			// product browser
			int PBH = Common.AppConfigUSInt("ProductBrowserHeight");
			if(PBH == 0)
			{
				PBH = 500; // pixels
			}
			if(anyFound)
			{
				Response.Write("<iframe height=\"" + (PBH-10).ToString() + "\" id=\"pb\" name=\"pb\" src=\"pb.aspx?type=" + Common.QueryString("type") + "&packid=" + PackID.ToString() + "&productid=" + Common.QueryStringUSInt("ShowProductID").ToString() + "&CategoryID=" + CategoryID.ToString() + "&sectionid=" + SectionID.ToString() + "&isfull=" + Common.QueryString("isfull") + "\" scrolling=\"no\" marginwidth=\"0\" marginheight=\"0\" frameborder=\"0\" vspace=\"0\" hspace=\"0\" style=\"width:100%; display:block; border-width: 1px; border-style: solid; border-color: #" + Common.AppConfig("ProductBrowserHoverColor") + "\"></iframe>\n");
			}

			Response.Write("</td>\n");
			Response.Write("      <td width=\"1%\" valign=\"top\" align=\"left\"></td>\n");
			Response.Write("    </tr>\n");
			Response.Write("  </table>\n");
			Response.Write("</div>\n");

			//			Response.Write("<script Language=\"JavaScript\">\n");
			//			Response.Write("function ClearAllBorders()\n");
			//			Response.Write("{\n");
			//			Response.Write(BrdClr);
			//			Response.Write("  return (true);\n");
			//			Response.Write("}\n");
			//			Response.Write("</script>\n");

			Response.Write("</body>\n");
			Response.Write("</html>\n");
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
