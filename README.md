# okta-dotnet4webform-clientapp-oidc

> Okta OpenID Connect (OIDC) integration example using ASP.NET 4 WebForms leveraging Global Login

![Version](https://img.shields.io/badge/version-v0.3.0.beta-blue.svg)


## System Requirements ##
Thsi sample was built using Visual Studio 2017 with .Net Framework 4.7.2 and Web Forms

## Usage ##
If you do not have access to an Okta Tenant you can get a free Okta Developer Tenant [here](https://developer.okta.com/signup/)
The Okta tenent requires configuration to integrate your client application.
Please refer to [OpenId Connect](https://developer.okta.com/docs/api/resources/oidc)

The web.config file needs to be edited to point to your configuration and your okta tenent.

For Example;
  <appSettings>

    <!-- Okta Config-->
    <add key="app.ApplicationName" value="Okta Integration Samples"/>
    <add key="oidc.spintweb.clientId" value="0oadt4xjwdPgIrMP00h7"/>
    <add key="oidc.spintweb.clientSecret" value="zyWA6duYVrjOoRHpCUCkGAXZdeUpl35dFFCSFHbW"/>
    <add key="oidc.issuer" value="https://dev-assurant.oktapreview.com"/>
    <add key="oidc.idp" value="0oae3xw7v1QmU2fpW0h7"/>
    <!--Note RedirectUri is being done dynamically in Startup.cs-->
    <add key="oidc.spintweb.RedirectUri" value="http://localhost:55368/signin-oidc"/>
    <add key="oidc.spintweb.PostLogoutRedirectUri" value="http://localhost:55368/PostLogOff.aspx"/>
    <add key="oidc.scopes" value="openid profile groups"/>
    <add key="oidc.tokenType" value="id_token"/>
    <add key="oidc.authError" value="AuthError.aspx"/>
  </appSettings>

## Features ##

This sample .Net WebForms application launches into a public page. There is a Login link which accesses a protected page. 
Hitting the protected page leverages the OWIN OIDC and Cookie authentication configuration. The OIDC authentication integration leverages Okta. 
The Cookie Authentication integration is the local application session. The integration automatically detects if a user as an active Okta or local session by utilizing these cookies. 
There are options within the OWIN Startup.cs file that dictate how the application will behave for various use cases. These use cases are;
* Using the built-in Okta Login UI
* Using the Global Login page via an Identity Provider configuration.
* Forcing the user to login in even if there is an active Okta session
* Only allowing user with an existing Okta session to access application ( SSO only)
* Capturing errors and redirecting to altenate locations such as a designeated Login UI.
* Checking and manipulating received and manufactured claims.
* Adding custom values to OIDC 'state' parameter.
* Configuring the app to leverage OIDC logoff workflow
* Checking and manipulating the outgoing redirect_url to help with navigating web hosting rules.


The sample application also demostrates; 
* How to access user related claims fro mmain code. 
* Mutiple methods to initiate Login
* Multiple methods to work with Logout from both the application and Okta.

## Contributing

1. Fork it
2. Create your feature branch (`git checkout -b feature/fooBar`)
3. Commit your changes (`git commit -am 'Add some fooBar'`)
4. Push to the branch (`git push origin feature/fooBar`)
5. Create a new Pull Request

**Note**: Contributing is very similar to the Github contribution process as described in detail 
[here](https://guides.github.com/activities/forking/).

## Contacts

- [Wayne Carson](https://assurhub.assurant.com/yc6235) – [wayne.carson@assurant.com](mailto:wayne.carson@assurant.com)
- [Andy Clarke](https://assurhub.assurant.com/fz6302) - [andy.clarke@assurant.com](mailto:andy.clarke@assurant.com)
