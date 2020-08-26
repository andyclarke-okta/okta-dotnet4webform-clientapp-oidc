using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Web.Configuration;
using System.Threading.Tasks;
//using System.IdentityModel.Claims;
using System.Security.Claims;
using log4net;
using Microsoft.IdentityModel.Tokens;

[assembly: OwinStartupAttribute(typeof(WebFormClientApp_OIDC_OWIN.Startup))]
namespace WebFormClientApp_OIDC_OWIN
{
    public partial class Startup
    {
        string idp = WebConfigurationManager.AppSettings["oidc.idp"];
        ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Configuration(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            //app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseCookieAuthentication(new CookieAuthenticationOptions { CookieName = "AssurantPolarisApp" });


            app.UseOpenIdConnectAuthentication(

                                new OpenIdConnectAuthenticationOptions
                                {
                                    ClientId = WebConfigurationManager.AppSettings["oidc.spintweb.clientId"],
                                    Authority = WebConfigurationManager.AppSettings["oidc.issuer"],
                                    //Note: this is done dynamically below
                                    RedirectUri = WebConfigurationManager.AppSettings["oidc.spintweb.RedirectUri"],
                                    ResponseType = WebConfigurationManager.AppSettings["oidc.tokenType"],
                                    Scope = WebConfigurationManager.AppSettings["oidc.scopes"],
                                    PostLogoutRedirectUri = WebConfigurationManager.AppSettings["oidc.spintweb.PostLogoutRedirectUri"],
                                    //StateDataFormat = new AuthenticationPropertiesFormaterKeyValue(),


            //Required to validate the signature
            TokenValidationParameters = new TokenValidationParameters
                                    {
                                        ValidAudience = WebConfigurationManager.AppSettings["oidc.spintweb.clientId"],
                                        ValidIssuer = WebConfigurationManager.AppSettings["oidc.issuer"],
                                        ValidateIssuerSigningKey = true,
                                        ValidateAudience = true,
                                        ValidateIssuer = true,
                                        ValidateLifetime = true
                                    },

                                    //Must be placed after 'TokenValidationParameters'
                                    SignInAsAuthenticationType = "Cookies",

                                    
                                    Notifications = new OpenIdConnectAuthenticationNotifications
                                    {

                                        MessageReceived = (context) =>
                                        {
                                            logger.Debug("MessageReceived");
                                            string assurantCustomParameter;
                                            var protectedState = context.ProtocolMessage.State.Split('=')[1];
                                            var state = context.Options.StateDataFormat.Unprotect(protectedState);
                                            state.Dictionary.TryGetValue("AssurantCustomParameter", out assurantCustomParameter);
                                            return Task.FromResult(0);
                                        },


                                        AuthorizationCodeReceived = (context) =>
                                        {
                                            logger.Debug("AuthorizationCodeReceived");
                                            return Task.FromResult(0);
                                        },
                                        AuthenticationFailed = (context) =>
                                        {
                                            logger.Debug("AuthenticationFailed");
                                            context.HandleResponse();                                    
                                            logger.Debug(context.Exception.Message);

                                            if (context.Exception.Message == "access_denied")
                                            {
                                                context.Response.Redirect(WebConfigurationManager.AppSettings["oidc.authError"]);
                                                //context.Response.Redirect("/AuthError?message=" + context.Exception.Message);
                                            }
                                            if (context.Exception.Message == "login_required")
                                            {
                                                context.Response.Redirect(WebConfigurationManager.AppSettings["oidc.authError"]);
                                                //context.Response.Redirect("/AuthError?message=" + context.Exception.Message);
                                            }
                                            else
                                            {
                                                context.Response.Redirect(WebConfigurationManager.AppSettings["oidc.authError"]);
                                                //context.Response.Redirect("/AuthError?message=" + context.Exception.Message);
                                            }


                                            return Task.FromResult(0);
                                        },
                                        SecurityTokenReceived = (context) =>
                                        {
                                            logger.Debug("SecurityTokenReceived");
                                            return Task.FromResult(0);
                                        },
                                        SecurityTokenValidated = (context) =>
                                        {
                                            logger.Debug("SecurityTokenValidated");
                                            string nameIdent = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                                            string loginId = context.AuthenticationTicket.Identity.FindFirst("loginId").Value;

                                            var id_token = context.ProtocolMessage.IdToken;
                                            context.AuthenticationTicket.Identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", loginId));
                                            context.AuthenticationTicket.Identity.AddClaim(new Claim("id_token", id_token));
                                            // Add roles based on Okta groups 
                                            foreach (var group in context.AuthenticationTicket.Identity.Claims.Where(x => x.Type == "groups"))
                                            {
                                                context.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Role, group.Value));
                                            }
                                            //context.AuthenticationTicket.Properties.RedirectUri = "/PostLogin.aspx";
                                            return Task.FromResult(0);
                                        },
                                        //added to support idp parameter on authorize endpoint
                                        RedirectToIdentityProvider = (context) =>
                                        {
                                            logger.Debug("RedirectToIdentityProvider type " + context.ProtocolMessage.RequestType);

                                            if (context.ProtocolMessage.RequestType == Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectRequestType.Authentication)
                                            {
                                                //use this if only allowed access from entry application
                                                //(context).ProtocolMessage.SetParameter("prompt", "none"); //does not allow a login UI if no session
                                                //use this to supercede an ACTIVE user
                                                //(context).ProtocolMessage.SetParameter("prompt", "login"); //forces login even if there is a ACTIVE Okta session

                                                //Gobal Login Integration
                                                //context.ProtocolMessage.SetParameter("idp", idp);

                                                //create redirect_url parameter dynamically
                                                //all variants MUST be included in Okta integration
                                                string absoluteUri = context.Request.Uri.AbsoluteUri;
                                                string myRedirectUrl = (absoluteUri.Substring(0, absoluteUri.IndexOf(context.Request.Uri.LocalPath))) + "/signin-oidc";
                                                string httpRule = context.Request.Headers["Front-End-Https"];
                                                if (!string.IsNullOrEmpty(httpRule) && httpRule == "On")
                                                {
                                                    int index = myRedirectUrl.IndexOf("http://");
                                                    if (myRedirectUrl.IndexOf("http://") != -1)
                                                    {
                                                        myRedirectUrl = myRedirectUrl.Replace("http://", "https://");
                                                    }
                                                }
                                                context.ProtocolMessage.RedirectUri = myRedirectUrl.ToLower();

                                                //add state parameter to OIDC
                                                //var stateQueryString = context.ProtocolMessage.State.Split('=');
                                                //var protectedState = stateQueryString[1];
                                                //var state = context.Options.StateDataFormat.Unprotect(protectedState);
                                                //state.Dictionary.Add("AssurantCustomParameter", "myClientAppStateInfo");
                                                //context.ProtocolMessage.State = stateQueryString[0] + "=" + context.Options.StateDataFormat.Protect(state);

                                            }
                                         
                                            if (context.ProtocolMessage.RequestType == Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectRequestType.Logout)
                                            {
                                                var idToken = context.OwinContext.Authentication.User.Claims
                                                    .FirstOrDefault(c => c.Type == "id_token")?.Value;
                                                context.ProtocolMessage.IdTokenHint = idToken;
                                                context.ProtocolMessage.State = "Optional_myApplicationState";
                                            }


                                            return Task.FromResult(0);
                                        }

                                    }
                                }



                );



        }
    }
}