using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using log4net;

namespace WebFormClientApp_OIDC_OWIN
{
    public partial class Login : Page
    {
        ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            logger.Debug("Login; protected page to intiate OIDC workflow");
            //if (!Request.IsAuthenticated || HttpContext.Current.User.Identity.AuthenticationType == "Forms")
            if (!Request.IsAuthenticated)
                {
                HttpContext.Current.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { },
                    //new AuthenticationProperties { RedirectUri = "/PostLogin.aspx" },
                    CookieAuthenticationDefaults.AuthenticationType,
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
                }

            logger.Debug("completed Login page");



        }
    }
}