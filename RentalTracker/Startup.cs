using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RentalTracker.Startup))]
namespace RentalTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
