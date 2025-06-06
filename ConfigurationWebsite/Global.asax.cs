using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Logger = Roblox.EventLog.Logger;

namespace Roblox.ConfigurationWebsite
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {

            Logger.Singleton.Information("Starting ConfigurationWebsite.");

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Logger.Singleton.Debug(@"
Started
AreaRegistration, RouteConfig, BundleConfig

Routing to public facing!
");
        }
    }
}
