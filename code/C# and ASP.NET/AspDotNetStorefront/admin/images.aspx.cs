// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 1995-2005.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// THE ABOVE NOTICE MUST REMAIN INTACT. ADD YOUR ADDITIONAL COPYRIGHT HERE ALSO IF DESIRED
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Web;
using System.IO;
using System.Collections;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for images.
	/// </summary>
	public class images : SkinBase
	{
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			SectionTitle = "Manage Images";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
			String SFP = HttpContext.Current.Server.MapPath("../images/spacer.gif").Replace("images\\spacer.gif","images\\upload");

			if(Common.QueryString("DeleteID").Length != 0)
			{
				// delete the image:
				System.IO.File.Delete(SFP + "/" + Common.QueryString("DeleteID"));
			}

			if(Common.Form("IsSubmit") == "true")
			{
				// handle upload if any also:
				HttpPostedFile Image1File = Request.Files["Image1"];
				if(Image1File.ContentLength != 0)
				{
					String tmp = Image1File.FileName.ToLower();
					if(tmp.EndsWith(".jpg") || tmp.EndsWith(".png") || tmp.EndsWith(".gif"))
					{
						if(tmp.LastIndexOf('\\') != -1)
						{
							tmp = tmp.Substring(tmp.LastIndexOf('\\') + 1);
						}
						String fn  = SFP + "/" + tmp;
						Image1File.SaveAs(fn);
					}
				}			
			}


			writer.Write("<form enctype=\"multipart/form-data\" id=\"Form1\" name=\"Form1\" method=\"POST\" action=\"images.aspx\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td><b>File Name</b></td>\n");
			writer.Write("      <td><b>Img Tag Src</b></td>\n");
			writer.Write("      <td><b>Dimensions</b></td>\n");
			writer.Write("      <td><b>Size (KB)</b></td>\n");
			writer.Write("      <td><b>Image</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete</b></td>\n");
			writer.Write("    </tr>\n");

			// create an array to hold the list of files
			ArrayList fArray=new ArrayList();

			// get information about our initial directory
			DirectoryInfo dirInfo=new DirectoryInfo(SFP);

			// retrieve array of files & subdirectories
			FileSystemInfo[] myDir=dirInfo.GetFileSystemInfos();
    
			for (int i=0; i<myDir.Length; i++) 
			{
				// check the file attributes

				// if a subdirectory, add it to the sArray    
				// otherwise, add it to the fArray
				if (((Convert.ToUInt32(myDir[i].Attributes) &	Convert.ToUInt32(FileAttributes.Directory) ) > 0 ))
				{
					//sArray.Add(Path.GetFileName(myDir[i].FullName));  
				}
				else
				{
					bool skipit = false;
					if(myDir[i].FullName.StartsWith("_") || (!myDir[i].FullName.ToLower().EndsWith("jpg") && !myDir[i].FullName.ToLower().EndsWith("gif") && !myDir[i].FullName.ToLower().EndsWith("png")))
					{
						skipit = true;
					}
					if(!skipit)
					{
						fArray.Add(Path.GetFileName(myDir[i].FullName));
					}
				}
			}

			if(fArray.Count != 0)
			{
				// sort the files alphabetically
				fArray.Sort(0,fArray.Count,null);
				for(int i = 0; i < fArray.Count; i++)
				{
					String src = "../images/upload/" + fArray[i].ToString();
					int w = Common.GetImageWidth(src);
					int h = Common.GetImageHeight(src);
					long s = Common.GetImageSize(src);
					writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
					writer.Write("      <td >" + fArray[i].ToString() + "</td>\n");
					writer.Write("      <td >../images/upload/" + fArray[i].ToString() + "</td>\n");
					writer.Write("      <td >" + w.ToString() + "x" + h.ToString() + "</td>\n");
					writer.Write("      <td >" + (s/1000).ToString() + " KB</td>\n");
					writer.Write("<td><a target=\"_blank\" href=\"" + src + "\">\n");
					writer.Write("<img border=\"0\" src=\"" + src + "?" + Common.GetRandomNumber(1,1000000).ToString() + "\"" + Common.IIF(h > 50 , " height=\"50\"" , "") + ">\n");
					writer.Write("</a></td>\n");
					writer.Write("      <td align=\"center\"><input type=\"button\" value=\"Delete\" name=\"Delete_" + i.ToString() + "\" onClick=\"DeleteImage(" + Common.SQuote(fArray[i].ToString()) + ")\"></td>\n");
					writer.Write("    </tr>\n");
				}
			}

			writer.Write("    <tr>\n");
			writer.Write("      <td colspan=\"6\" height=5></td>\n");
			writer.Write("    </tr>\n");
			writer.Write("  </table>\n");
			writer.Write("<p align=\"left\">Upload A New Image: <input type=\"file\" name=\"Image1\" size=\"50\"><br><input type=\"submit\" value=\"Submit\" name=\"submit\"></p>\n");
			writer.Write("</form>\n");

			writer.Write("</center></b>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteImage(name)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete image: ' + name))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'images.aspx?deleteid=' + name;\n");
			writer.Write("}\n");
			writer.Write("}\n");
			writer.Write("</SCRIPT>\n");
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
