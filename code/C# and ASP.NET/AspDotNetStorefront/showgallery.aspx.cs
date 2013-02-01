// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Web;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefront
{
	/// <summary>
	/// Summary description for showgallery.
	/// </summary>
	public class showgallery : System.Web.UI.Page
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			Response.CacheControl="private";
			Response.Expires=0;
			Response.AddHeader("pragma", "no-cache");

			Customer thisCustomer = new Customer();

			int GalleryID = Common.QueryStringUSInt("GalleryID");
			String Dir = Common.GetGalleryDir(GalleryID); //Common.QueryString("Gallery");
			String SFP = HttpContext.Current.Server.MapPath("images/spacer.gif").Replace("images\\spacer.gif",""); //Common.AppConfig("StoreFilesPath");
			if(!SFP.EndsWith("\\"))
			{
				SFP = SFP + "\\";
			}

			String tPath = SFP + "images\\gallery\\" + Dir + "\\";
			int NumSlides = Common.GetNumSlides(tPath);
			int SlideIdx = Common.QueryStringUSInt("SlideIDX");
			String ThisSlide = tPath + "slide" + SlideIdx.ToString().PadLeft(2,'0') + "_lg.jpg";
			String ThisSlideRel = "images/gallery/" + Dir + "/slide" + SlideIdx.ToString().PadLeft(2,'0') + "_lg.jpg";
			bool InShow = Common.QueryString("show").ToLower() == "true";
			bool ShowGallery = (SlideIdx == 0);
			if(ShowGallery)
			{
				InShow = false; // can't do slide show in a gallery view
			}

			String GalleryName = Common.ExtractToken(Common.ReadFile(tPath + "gallery.xml",true),"<Name>","</Name>");

			Response.Write("<html>\n");
			Response.Write("<head>\n");
			Response.Write("<title>" + Common.AppConfig("StoreName") + " Gallery - " + GalleryName + "</title>\n");
			Response.Write("<script language=\"javascript\">\n");
			Response.Write("if ((screen.width > 800) && (screen.height > 600))\n");
			Response.Write("{\n");
			Response.Write("	var isNav4, isIE4;\n");
			Response.Write("	if (parseInt(navigator.appVersion.charAt(0)) >= 4)\n");
			Response.Write("	{\n");
			Response.Write("		isNav4 = (navigator.appName == \"Netscape\") ? 1 : 0;\n");
			Response.Write("		isIE4 = (navigator.appName.indexOf(\"Microsoft\") != -1) ? 1 : 0;\n");
			Response.Write("		isWin = (navigator.platform == \"Win32\") ? 1 : 0;\n");
			Response.Write("	}\n");
			Response.Write("\n");
			Response.Write("function fitWindowSize()\n");
			Response.Write("{\n");
			Response.Write("	if (isNav4)\n");
			Response.Write("	{\n");
			Response.Write("		if (isWin)\n");
			Response.Write("		{\n");
			Response.Write("			setWidth = (document.layers[0].document.images[0].width)+33;\n");
			Response.Write("			setHeight = (document.layers[0].document.images[0].height)+90;\n");
			Response.Write("		}\n");
			Response.Write("		else\n");
			Response.Write("		{\n");
			Response.Write("			setWidth = (document.layers[0].document.images[0].width)+33;\n");
			Response.Write("			setHeight = (document.layers[0].document.images[0].height)+90;\n");
			Response.Write("		}\n");
			Response.Write("		if (setWidth < 440)\n");
			Response.Write("		{\n");
			Response.Write("			setWidth = 440;\n");
			Response.Write("		}\n");
			Response.Write("		window.innerWidth = setWidth;\n");
			Response.Write("		window.innerHeight = setHeight;\n");
			Response.Write("	}\n");
			Response.Write("	if (isIE4)\n");
			Response.Write("	{\n");
			Response.Write("		if (isWin)\n");
			Response.Write("		{\n");
			Response.Write("			window.resizeTo(800, 800);\n");
			Response.Write("			width = (800 - (document.body.clientWidth -  document.images[6].width) +20);\n");
			Response.Write("			height = (800 - (document.body.clientHeight -  document.images[6].height) +90);\n");
			Response.Write("		}\n");
			Response.Write("		else\n");
			Response.Write("		{\n");
			Response.Write("			window.resizeTo(800, 800);\n");
			Response.Write("			width = (800 - (document.body.clientWidth -  document.images[6].width) + 25);\n");
			Response.Write("			height = (800 - (document.body.clientHeight -  document.images[6].height) + 105);\n");
			Response.Write("		}\n");
			Response.Write("		if (width < 440)\n");
			Response.Write("		{\n");
			Response.Write("			width = 440;\n");
			Response.Write("		}\n");
			Response.Write("		window.resizeTo(width, height);\n");
			Response.Write("	}\n");
			Response.Write("}\n");
			Response.Write("\n");
			Response.Write("}\n");
			Response.Write("\n");
			Response.Write("var timer1 = null;\n");
			Response.Write("var inShow = " + Common.IIF(InShow , "1" , "0") + ";\n");
			Response.Write("function toggleShow()\n");
			Response.Write("{\n");
			Response.Write("	if(inShow == 1)\n");
			Response.Write("	{\n");
			Response.Write("		clearTimeout(timer1);\n");
			Response.Write("		inShow = 0;\n");
			Response.Write("	}\n");
			Response.Write("	else\n");
			Response.Write("	{\n");
			Response.Write("		setTimeout('go()'," + (Common.AppConfigUSInt("SlideShowInterval")*1000).ToString() + ");\n");
			Response.Write("		inShow = 0;\n");
			Response.Write("	}\n");
			Response.Write("}\n");
			Response.Write("function go()\n");
			Response.Write("{\n");
			Response.Write("	location.href='showgallery.aspx?show=true&galleryid=" + GalleryID.ToString() + "&slideidx=" + Common.IIF(SlideIdx == NumSlides , 1 , (SlideIdx + 1)).ToString() + "';\n");
			Response.Write("}\n");
			Response.Write("\n");
			Response.Write("</script>\n");
			Response.Write("\n");
			Response.Write("<style type=\"text/css\">\n");
			Response.Write(".picnr { font-family: Arial, Helvetica, sans-serif; font-size: 10px; color: #000000; text-decoration: none; font-weight: bold; }\n");
			Response.Write("</style>\n");
			Response.Write("\n");
			Response.Write("</head>\n");
			if(Common.AppConfigBool("ResizeSlideWindow"))
			{
				Response.Write("<BODY BGCOLOR=\"#000000\" onload=\"fitWindowSize();\">\n");
			}
			else
			{
				Response.Write("<BODY BGCOLOR=\"#000000\">\n");
			}
			if(InShow)
			{
				Response.Write("<script language=\"javascript\">\n");
				Response.Write("timer1 = setTimeout('go()'," + (Common.AppConfigUSInt("SlideShowInterval")*1000).ToString() + ");\n");
				Response.Write("</script>\n");
			}

			Response.Write("<center>\n");
			Response.Write("<form name=\"form\">\n");
			Response.Write("<TABLE WIDTH=\"100%\" BORDER=0 CELLPADDING=0 CELLSPACING=0  height=19>\n");
			Response.Write("<TR>\n");
			Response.Write("<TD width=\"162\" height=19><a href=\"showgallery.aspx?galleryid=" + GalleryID.ToString() + "\"><IMG SRC=\"images/gallery/pictorial_top_01.gif\" border=\"0\" WIDTH=162 HEIGHT=19></TD>\n");
			Response.Write("<TD bgcolor=\"#000000\" width=\"100%\">&nbsp;</TD>\n");
			Response.Write("<TD width=\"15\" height=19><IMG SRC=\"images/gallery/pictorial_top_03.gif\" WIDTH=15 HEIGHT=19></TD>\n");
			Response.Write("<TD width=\"180\" bgcolor=\"#969696\" class=\"picnr\" align=\"right\" nowrap height=19>picture " + SlideIdx.ToString() + " of " + NumSlides.ToString() + "    &nbsp;&nbsp;&nbsp;&nbsp;I&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"javascript:window.close()\"><font class=\"picnr\">close window</font></a></TD>\n");
			Response.Write("<TD width=\"1\" bgcolor=\"#969696\" height=19>&nbsp; </TD>\n");
			Response.Write("<TD width=\"28\" height=19><a href=\"javascript:window.close()\"><IMG SRC=\"images/gallery/pictorial_top_06.gif\" border=\"0\" WIDTH=28 HEIGHT=19></a></TD>\n");
			Response.Write("</TR>\n");
			Response.Write("</TABLE>\n");
			Response.Write("<TABLE WIDTH=\"100%\" BORDER=0 CELLPADDING=0 CELLSPACING=0>\n");
			Response.Write("<tr>\n");
			Response.Write("<td width=\"86\">\n");
			Response.Write("<a href=\"showgallery.aspx?galleryid=" + GalleryID.ToString() + "&slideidx=" + Common.IIF(SlideIdx == 1 || SlideIdx == 0 , NumSlides , (SlideIdx - 1)).ToString() + "\">\n");
			Response.Write("<img src=\"images/gallery/pictorial_bot_01.gif\" border=\"0\" width=86 height=36></a>");
			Response.Write("</td>\n");
			Response.Write("<td width=\"94\">\n");
			Response.Write("<a href=\"showgallery.aspx?galleryid=" + GalleryID.ToString() + "&slideidx=" + Common.IIF(SlideIdx == NumSlides , 1 , (SlideIdx + 1)).ToString() + "\">\n");
			Response.Write("<img src=\"images/gallery/pictorial_bot_02.gif\" border=\"0\" width=94 height=36></a>");
			Response.Write("</td>\n");
			Response.Write("<td height=\"36\" align=\"center\" background=\"images/gallery/pictorial_bot_03.gif\" width=\"100%\"> \n");
			Response.Write("<select name=\"url\" onchange=\"javascript:location.href=form.url.options[form.url.selectedIndex].value\">\n");
			Response.Write("<option value=\"showgallery.aspx?galleryid=" + GalleryID.ToString() + "\"" +  Common.IIF(SlideIdx == 0 , " selected " , "") + ">Gallery</option>\n");
			for(int i = 1; i <= NumSlides; i++)
			{
				Response.Write("<option value=\"showgallery.aspx?galleryid=" + GalleryID.ToString() + "&slideidx=" + i.ToString() + "\"" +  Common.IIF(SlideIdx == i , " selected " , "").ToString() + ">" + i.ToString() + "</option>\n");
			}
			Response.Write("</select>\n");
			Response.Write("</td>\n");
			Response.Write("<td width=\"181\"><a href=\"#\" onclick=\"toggleShow();\">\n");
			Response.Write("<img name=\"slideshow\" src=\"images/gallery/pictorial_bot_04.gif\" border=\"0\"></a></td>\n");
			Response.Write("</tr>\n");
			Response.Write("</table>\n");
			Response.Write("</form>\n");
			Response.Write("</center>\n");
			Response.Write("<div style=\"position:absolute; left:10px; top:72px\">\n");
			if(ShowGallery)
			{
				for(int i = 1; i <= NumSlides; i++)
				{
					Response.Write("<a href=\"showgallery.aspx?galleryid=" + GalleryID.ToString() + "&slideidx=" + i.ToString() + "\">");
					Response.Write("<img src=\"" + "images/gallery/" + Dir + "/slide" + i.ToString().PadLeft(2,'0') + ".jpg\" border=\"0\">");
					Response.Write("</a>&nbsp;");
					//					col++;
					//					if(col > Common.AppConfigUSInt("SlideShowGalleryCols"))
					//					{
					//						Response.Write("<br>");
					//						col = 1;
					//					}
				}
			}
			else
			{
				Response.Write("<a href=\"showgallery.aspx?galleryid=" + GalleryID.ToString() + "&slideidx=" + Common.IIF(SlideIdx == NumSlides , 1 , (SlideIdx + 1)).ToString() + "\">\n");
				Response.Write("<img src=\"" + ThisSlideRel + "\" border=\"0\">");
				Response.Write("</a>\n");
			}
			Response.Write("</div>\n");
			Response.Write("</BODY>\n");
			Response.Write("</HTML>\n");

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
