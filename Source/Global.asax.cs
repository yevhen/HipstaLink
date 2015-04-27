using System;
using System.Linq;
using System.Web.Http;

namespace HipstaLink
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Config.Register(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configuration.EnsureInitialized();
        }
    }
}