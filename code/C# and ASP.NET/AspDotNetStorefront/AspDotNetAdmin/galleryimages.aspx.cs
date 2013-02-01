// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using System.Xml;
using System.Configuration;
using System.Web.SessionState;
using System.Web.Caching;
using System.Web.Handlers;
using System.Web.Hosting;
using System.Web.Mail;
using System.Web.Security;
using System.Web.UI;
using System.Web.Util;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin
{
	/// <summary>
	/// Summary description for galleryimages.
	/// </summary>
	public class galleryimages : SkinBase
	{
		
		int GalleryID;
		String GalleryName;
		String Dir;
		String SFP;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.DoOnLoad(); // Allows SkinBase to initialize some vars (e.g. thisCustomer)
			GalleryID = Common.QueryStringUSInt("GalleryID");
			GalleryName = Common.GetGalleryName(GalleryID);
			Dir = Common.GetGalleryDir(GalleryID); //Common.QueryString("Gallery");
			SFP = HttpContext.Current.Server.MapPath("../images/spacer.gif").Replace("images\\spacer.gif","");
			if(!SFP.EndsWith("\\"))
			{
				SFP = SFP + "\\";
			}

			SFP = SFP + "images\\gallery\\" + Dir + "\\";


			if(Common.QueryString("DeleteID").Length != 0)
			{
				// delete the image:
				int DeletingSlideID = Common.QueryStringUSInt("DeleteID");
				int LastSlide = Common.GetNumSlides(SFP);
				String fndel  = SFP + "slide" + DeletingSlideID.ToString().PadLeft(2,'0') + ".jpg";
				String fndel_lg  = SFP + "slide" + DeletingSlideID.ToString().PadLeft(2,'0') + "_lg.jpg";

				System.IO.File.Delete(fndel);
				System.IO.File.Delete(fndel_lg);

				// now must renumber all "higher slides"
				for(int i = DeletingSlideID + 1; i <= Common.AppConfigUSInt("MaxSlides"); i++)
				{
					String src  = SFP + "slide" + i.ToString().PadLeft(2,'0') + ".jpg";
					String src_lg  = SFP + "slide" + i.ToString().PadLeft(2,'0') + "_lg.jpg";
					String dest  = SFP + "slide" + (i-1).ToString().PadLeft(2,'0') + ".jpg";
					String dest_lg  = SFP + "slide" + (i-1).ToString().PadLeft(2,'0') + "_lg.jpg";
					if(System.IO.File.Exists(src))
					{
						try
						{
							System.IO.File.Move(src,dest);
							System.IO.File.Move(src_lg,dest_lg);
						}
						catch {}
					}
				}
				try
				{
					String lastslide2  = SFP + "slide" + Common.AppConfigUSInt("MaxSlides").ToString().PadLeft(2,'0') + ".jpg";
					String lastslide_lg  = SFP + "slide" + Common.AppConfigUSInt("MaxSlides").ToString().PadLeft(2,'0') + "_lg.jpg";
					if(System.IO.File.Exists(lastslide2))
					{
						System.IO.File.Delete(lastslide2);
						System.IO.File.Delete(lastslide_lg);
					}
				}
				catch {}
			}

			if(Common.QueryString("MoveUpID").Length != 0)
			{
				// move the specified image up:
				int MoveUpID = Common.QueryStringUSInt("MoveUpID");
				int LastSlide = Common.GetNumSlides(SFP);
				String srcup  = SFP + "slide" + MoveUpID.ToString().PadLeft(2,'0') + ".jpg";
				String srcup_lg  = SFP + "slide" + MoveUpID.ToString().PadLeft(2,'0') + "_lg.jpg";
				String destup  = SFP + "slide" + (MoveUpID-1).ToString().PadLeft(2,'0') + ".jpg";
				String destup_lg  = SFP + "slide" + (MoveUpID-1).ToString().PadLeft(2,'0') + "_lg.jpg";
				String tmpup  = SFP + "slide00.jpg";
				String tmpup_lg  = SFP + "slide00_lg.jpg";
				if(MoveUpID > 1)
				{
					System.IO.File.Move(destup,tmpup);
					System.IO.File.Move(destup_lg,tmpup_lg);
					System.IO.File.Move(srcup,destup);
					System.IO.File.Move(srcup_lg,destup_lg);
					System.IO.File.Move(tmpup,srcup);
					System.IO.File.Move(tmpup_lg,srcup_lg);

				}
			}

			if(Common.QueryString("MoveFirstID").Length != 0)
			{
				// move the specified image to first:
				int MoveFirstID = Common.QueryStringUSInt("MoveFirstID");
				int LastSlide = Common.GetNumSlides(SFP);
				String srcup  = SFP + "slide" + MoveFirstID.ToString().PadLeft(2,'0') + ".jpg";
				String srcup_lg  = SFP + "slide" + MoveFirstID.ToString().PadLeft(2,'0') + "_lg.jpg";
				String destup  = SFP + "slide01.jpg";
				String destup_lg  = SFP + "slide01_lg.jpg";
				String tmpup  = SFP + "slide00.jpg";
				String tmpup_lg  = SFP + "slide00_lg.jpg";
				if(MoveFirstID > 1)
				{
					System.IO.File.Move(srcup,tmpup);
					System.IO.File.Move(srcup_lg,tmpup_lg);

					// now must move "up" all "lower slides"
					for(int i = MoveFirstID; i >= 2; i--)
					{
						String xsrc  = SFP + "slide" + (i-1).ToString().PadLeft(2,'0') + ".jpg";
						String xsrc_lg  = SFP + "slide" + (i-1).ToString().PadLeft(2,'0') + "_lg.jpg";
						String xdest  = SFP + "slide" + i.ToString().PadLeft(2,'0') + ".jpg";
						String xdest_lg  = SFP + "slide" + i.ToString().PadLeft(2,'0') + "_lg.jpg";
						System.IO.File.Delete(xdest);
						System.IO.File.Delete(xdest_lg);
						System.IO.File.Move(xsrc,xdest);
						System.IO.File.Move(xsrc_lg,xdest_lg);
					}
					System.IO.File.Move(tmpup,destup);
					System.IO.File.Move(tmpup_lg,destup_lg);

				}
			}

			if(Common.QueryString("MoveDownID").Length != 0)
			{
				// move the specified image down:
				int MoveDownID = Common.QueryStringUSInt("MoveDownID");
				int LastSlide = Common.GetNumSlides(SFP);
				String srcup  = SFP + "slide" + MoveDownID.ToString().PadLeft(2,'0') + ".jpg";
				String srcup_lg  = SFP + "slide" + MoveDownID.ToString().PadLeft(2,'0') + "_lg.jpg";
				String destup  = SFP + "slide" + (MoveDownID+1).ToString().PadLeft(2,'0') + ".jpg";
				String destup_lg  = SFP + "slide" + (MoveDownID+1).ToString().PadLeft(2,'0') + "_lg.jpg";
				String tmpup  = SFP + "slide00.jpg";
				String tmpup_lg  = SFP + "slide00_lg.jpg";
				if(MoveDownID < LastSlide)
				{
					System.IO.File.Move(destup,tmpup);
					System.IO.File.Move(destup_lg,tmpup_lg);
					System.IO.File.Move(srcup,destup);
					System.IO.File.Move(srcup_lg,destup_lg);
					System.IO.File.Move(tmpup,srcup);
					System.IO.File.Move(tmpup_lg,srcup_lg);

				}
			}

			if(Common.QueryString("MoveLastID").Length != 0)
			{
				// move the specified image to Last:
				int MoveLastID = Common.QueryStringUSInt("MoveLastID");
				int LastSlide = Common.GetNumSlides(SFP);
				String srcup  = SFP + "slide" + MoveLastID.ToString().PadLeft(2,'0') + ".jpg";
				String srcup_lg  = SFP + "slide" + MoveLastID.ToString().PadLeft(2,'0') + "_lg.jpg";
				String destup  = SFP + "slide" + LastSlide.ToString().PadLeft(2,'0') + ".jpg";
				String destup_lg  = SFP + "slide" + LastSlide.ToString().PadLeft(2,'0') + "_lg.jpg";
				String tmpup  = SFP + "slide00.jpg";
				String tmpup_lg  = SFP + "slide00_lg.jpg";
				if(MoveLastID < LastSlide)
				{
					System.IO.File.Move(srcup,tmpup);
					System.IO.File.Move(srcup_lg,tmpup_lg);

					// now must move "down" all "higher slides"
					for(int i = MoveLastID; i <= LastSlide-1; i++)
					{
						String xsrc  = SFP + "slide" + (i+1).ToString().PadLeft(2,'0') + ".jpg";
						String xsrc_lg  = SFP + "slide" + (i+1).ToString().PadLeft(2,'0') + "_lg.jpg";
						String xdest  = SFP + "slide" + i.ToString().PadLeft(2,'0') + ".jpg";
						String xdest_lg  = SFP + "slide" + i.ToString().PadLeft(2,'0') + "_lg.jpg";
						System.IO.File.Delete(xdest);
						System.IO.File.Delete(xdest_lg);
						System.IO.File.Move(xsrc,xdest);
						System.IO.File.Move(xsrc_lg,xdest_lg);
					}
					System.IO.File.Move(tmpup,destup);
					System.IO.File.Move(tmpup_lg,destup_lg);

				}
			}


			if(Common.Form("IsSubmit") == "true")
			{
				// handle upload if any also:
				int NextSlideNumber = Common.FormUSInt("NewSlideNumber");
				HttpPostedFile Image1File = Request.Files["Image1"];
				if(Image1File.ContentLength != 0)
				{
					String tmp = Image1File.FileName;
					if(tmp.LastIndexOf('\\') != -1)
					{
						tmp = tmp.Substring(tmp.LastIndexOf('\\') + 1);
					}
					String fn  = SFP + "slide" + NextSlideNumber.ToString().PadLeft(2,'0') + ".jpg";
					String fn_lg  = SFP + "slide" + NextSlideNumber.ToString().PadLeft(2,'0') + "_lg.jpg";
					Image1File.SaveAs(fn_lg);

					// create thumbnail:
					System.Drawing.Image g;
					System.Drawing.Bitmap g2;
					int newWidth = 0;
					int newHeight = 0;
					Single sizer = 0;
					int boxWidth=125;
					int boxHeight=125;
 
					// create a new image from file
					g = System.Drawing.Image.FromFile(fn_lg);
 
					if (g.Height > g.Width)		// portrait
					{
						sizer = (Single)boxWidth / (Single)g.Height;
					}
					else
					{
						sizer = (Single)boxHeight / (Single)g.Width;
					}
              
					newWidth = Convert.ToInt32(g.Width * sizer);
					newHeight = Convert.ToInt32(g.Height * sizer);
 
					g2 = new System.Drawing.Bitmap(g, newWidth,newHeight);
 
					// set the content type
					Response.ContentType ="image/jpeg";
 
					// send the image to the viewer
					g2.Save(fn, g.RawFormat);
 
					// tidy up
					g.Dispose();

				}			
			}
			SectionTitle = "<a href=\"galleries.aspx\">Galleries</a> - Manage Gallery Images (" + GalleryName + ")";
		}

		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
		{
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
				//if (((Convert.ToByte(myDir[i].Attributes) &	Convert.ToByte(FileAttributes.Directory) ) > 0 ))
				//{
				//sArray.Add(Path.GetFileName(myDir[i].FullName));  
				//}
				//else
				//{
				if(myDir[i].FullName.ToLower().EndsWith("jpg") && myDir[i].FullName.ToLower().IndexOf("_lg.jpg") == -1)
				{
					fArray.Add(Path.GetFileName(myDir[i].FullName));
				}
				//}
			}
			// sort the files alphabetically
			fArray.Sort(0,fArray.Count,null);

			writer.Write("<form enctype=\"multipart/form-data\" id=\"Form1\" name=\"Form1\" method=\"POST\" action=\"galleryimages.aspx?galleryid=" + GalleryID.ToString() + "\">\n");
			writer.Write("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\">\n");
			writer.Write("<input type=\"hidden\" name=\"NewSlideNumber\" value=\"" + (fArray.Count+1).ToString() + "\">\n");
			writer.Write("  <table border=\"0\" cellpadding=\"2\" border=\"0\" cellspacing=\"1\" width=\"100%\">\n");
			writer.Write("    <tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
			writer.Write("      <td><b>Slide ID</b></td>\n");
			writer.Write("      <td><b>File Name</b></td>\n");
			writer.Write("      <td><b>Img Tag Src</b></td>\n");
			writer.Write("      <td><b>Dimensions</b></td>\n");
			writer.Write("      <td><b>Size (KB)</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Image</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Move Up/Down</b></td>\n");
			writer.Write("      <td align=\"center\"><b>Delete</b></td>\n");
			writer.Write("    </tr>\n");

			if(fArray.Count != 0)
			{
				for(int i = 0; i < fArray.Count; i++)
				{
					String src = Common.GetImagePath("GALLERY","",false) + Dir + "/" + fArray[i].ToString().ToLower();
					String src_lg = src.Replace(".jpg","_lg.jpg");
					int w = Common.GetImageWidth(src_lg);
					int h = Common.GetImageHeight(src_lg);
					long s = Common.GetImageSize(src_lg);
					writer.Write("<tr bgcolor=\"" + Common.AppConfig("LightCellColor") + "\">\n");
					writer.Write("<td >" + (i+1).ToString() + "</td>\n");
					writer.Write("<td >" + fArray[i].ToString() + "</td>\n");
					writer.Write("<td >images/gallery/" + Dir + "/" + fArray[i].ToString() + "</td>\n");
					writer.Write("<td >" + w.ToString() + "x" + h.ToString() + "</td>\n");
					writer.Write("<td >" + (s/1000).ToString() + " KB</td>\n");
					writer.Write("<td align=\"center\"><a target=\"_blank\" href=\"" + src_lg + "\">\n");
					writer.Write("<img border=\"0\" src=\"" + src + "?" + Common.GetRandomNumber(1,1000000).ToString() + "\"" + Common.IIF(h > 50 , " height=\"50\"" , "") + ">\n");
					writer.Write("</a></td>\n");
					writer.Write("<td align=\"center\">");
					if(i != 0)
					{
						writer.Write("<input type=\"button\" value=\"To First\" name=\"MoveFirst_" + i.ToString() + "\" onClick=\"self.location = 'galleryimages.aspx?galleryid=" + GalleryID.ToString() + "&movefirstid=' + " + (i+1).ToString() + ";\">");
						writer.Write("<input type=\"button\" value=\"Up\" name=\"MoveUp_" + i.ToString() + "\" onClick=\"self.location = 'galleryimages.aspx?galleryid=" + GalleryID.ToString() + "&moveupid=' + " + (i+1).ToString() + ";\">");
					}
					if(i != fArray.Count-1)
					{
						writer.Write("<input type=\"button\" value=\"Down\" name=\"MoveDown_" + i.ToString() + "\" onClick=\"self.location = 'galleryimages.aspx?galleryid=" + GalleryID.ToString() + "&movedownid=' + " + (i+1).ToString() + ";\">");
						writer.Write("<input type=\"button\" value=\"To Last\" name=\"MoveLast_" + i.ToString() + "\" onClick=\"self.location = 'galleryimages.aspx?galleryid=" + GalleryID.ToString() + "&movelastid=' + " + (i+1).ToString() + ";\">");
					}
					writer.Write("</td>\n");
					writer.Write("<td align=\"center\">");
					writer.Write("<input type=\"button\" value=\"Delete\" name=\"Delete_" + i.ToString() + "\" onClick=\"DeleteImage(" + (i+1).ToString() + ")\">");
					writer.Write("</td>\n");
					writer.Write("</tr>\n");
				}
			}

			writer.Write("<tr>\n");
			writer.Write("<td colspan=\"6\" height=5></td>\n");
			writer.Write("</tr>\n");
			writer.Write("</table>\n");
			writer.Write("<p align=\"left\">Upload A New Image (JPG FORMAT ONLY): <input type=\"file\" name=\"Image1\" size=\"50\">&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"submit\" value=\"Submit\" name=\"submit\"></p>\n");
			writer.Write("</form>\n");

			writer.Write("</center></b>\n");

			writer.Write("<SCRIPT LANGUAGE=\"javascript\">\n");
			writer.Write("function DeleteImage(name)\n");
			writer.Write("{\n");
			writer.Write("if(confirm('Are you sure you want to delete gallery slide: ' + name))\n");
			writer.Write("{\n");
			writer.Write("self.location = 'galleryimages.aspx?galleryid=" + GalleryID.ToString() + "&deleteid=' + name;\n");
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
