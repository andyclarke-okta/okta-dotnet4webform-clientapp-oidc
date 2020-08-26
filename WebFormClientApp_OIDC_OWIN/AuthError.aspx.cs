using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

namespace WebFormClientApp_OIDC_OWIN
{
    public partial class AuthError : System.Web.UI.Page
    {
        ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            logger.Debug("optional error message display");
            String msg = Request.QueryString["message"];

            Label1.Text = msg;
        }
    }
}