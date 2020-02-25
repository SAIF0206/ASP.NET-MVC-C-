using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AD_Project.Startup))]
namespace AD_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
