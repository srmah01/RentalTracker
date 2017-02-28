using RentalTracker.DAL;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RentalTracker
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DataHelper.NewDb(true);
        }
    }
}
