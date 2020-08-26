using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security.Cookies;

namespace WebFormClientApp_OIDC_OWIN
{
    public partial class PostLogin : System.Web.UI.Page
    {
        ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            logger.Debug("PostLogin " + User.Identity.IsAuthenticated.ToString() + " " + User.Identity.Name.ToString());


            string myLoginId = null;
            string nameIdentifier = null;

            ClaimsPrincipal principal = HttpContext.Current.User as ClaimsPrincipal;
            
            nameIdentifier = principal.Identity.Name;
            myLoginId = (from c in principal.Claims where c.Type == "loginId" select c.Value).First();

            ///the following is a special integration when using Ricardo's NuGet package
            ////after successful login, get all user attributes from Ricardo's model
            ////need to logout an existing user identity if present, i dont think there was 
            //var ctx = Request.GetOwinContext();
            //var authenticationManager = ctx.Authentication;
            //authenticationManager.SignOut(CookieAuthenticationDefaults.AuthenticationType);

            ////build new set of claims
            //List<Claim> myClaims = new List<Claim>();
            //myClaims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", nameIdentifier));
            //myClaims.Add(new Claim("loginId", myLoginId));
            //myClaims.Add(new Claim("ajc", "anyValue"));

            ////build new claims identity with attributes
            //var myNewIdentity = new ClaimsIdentity(myClaims,
            //                CookieAuthenticationDefaults.AuthenticationType);

            ////login in the new user identity
            //authenticationManager.SignIn(myNewIdentity);

            ////note you will not see new user or new claims until after you exit the current method.
        }


    }
}