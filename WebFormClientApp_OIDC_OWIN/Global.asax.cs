using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using log4net;

namespace WebFormClientApp_OIDC_OWIN
{
    public class Global : HttpApplication
    {
        ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        void Application_Start(object sender, EventArgs e)
        {
            logger.Debug("Application_Start");
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));

            //System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            logger.Debug("Session_Start");
            // event is raised each time a new session is created     
        }

        protected void Application_BeginRequest()
        {
            logger.Debug("Application_BeginRequest");
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            logger.Debug("Application_AuthenticateRequest");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            logger.Debug("Application_Error");
        }

        protected void Session_End(object sender, EventArgs e)
        {
            logger.Debug("Session_End");
        }

        protected void Application_End(object sender, EventArgs e)
        {
            logger.Debug("Application_End");
        }




    }
}