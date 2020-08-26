using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using log4net;

namespace WebFormClientApp_OIDC_OWIN
{
    public partial class About : Page
    {
        ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            logger.Debug("protected endpoint to initiate OIDC workflow");
            if (!Request.IsAuthenticated)
            {
                HttpContext.Current.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { },
                    //new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }

            string myLoginId = null;
            string nameIdentifier = null;

            ClaimsPrincipal principal = HttpContext.Current.User as ClaimsPrincipal;
            try
            {
                nameIdentifier = principal.Identity.Name;
                myLoginId = (from c in principal.Claims where c.Type == "loginId" select c.Value).First();
            }
            catch (Exception)
            {

                //throw;
            }

        }
    }
}