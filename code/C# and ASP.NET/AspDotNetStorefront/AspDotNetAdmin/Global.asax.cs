// ------------------------------------------------------------------------------------------
// Copyright AspDotNetStorefront.com, 2005-2012.  All Rights Reserved.
// http://www.aspdotnetstorefront.com
// For details on this license please visit  the product homepage at the URL above.
// Copyright Firestar Design Co.  2008-2012.  All Rights Reserved.  
// ------------------------------------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Web;
using AspDotNetStorefrontCommon;

namespace AspDotNetStorefrontAdmin 
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{
			Common.IsAdminSite = true;
			Common.AppConfigTable = Common.LoadAppConfig();
			if(Common.AppConfig("EncryptKey") == "WIZARD")
			{
				Common.SetAppConfig("EncryptKey",Common.GetRandomNumber(1000,1000000).ToString() + Common.GetRandomNumber(1000,1000000).ToString() + Common.GetRandomNumber(1000,1000000).ToString(),false);
			}
		}
 
		public void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
		{
			//Common.CheckDemoExpiration();
			if(Common.AppConfigBool("UseSSL"))
			{
				if(Common.OnLiveServer() && Common.ServerVariables("SERVER_PORT_SECURE") != "1")
				{
					Response.Redirect("https://" + Common.ServerVariables("HTTP_HOST") + Common.GetThisPageName(true) + "?" + Common.ServerVariables("QUERY_STRING"));
				}
			}
			if(Common.ApplicationBool("ServerFarm"))
			{
				Common.SessionStart();
			}
		}
		
		protected void Session_Start(Object sender, EventArgs e)
		{
			if(!Common.ApplicationBool("ServerFarm"))
			{
			Common.SessionStart();
			}
		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
			if (Request.Path.IndexOf('\\') >= 0 ||
				System.IO.Path.GetFullPath(Request.PhysicalPath) != Request.PhysicalPath) 
			{
				throw new HttpException(404, "not found");
			}
		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{
			Common.SetRoles();
		}

		protected void Application_Error(Object sender, EventArgs e)
		{

		}

		protected void Session_End(Object sender, EventArgs e)
		{

		}

		protected void Application_End(Object sender, EventArgs e)
		{

		}
			
		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.components = new System.ComponentModel.Container();
		}
		#endregion
	}
}

