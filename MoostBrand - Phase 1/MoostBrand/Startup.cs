using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MoostBrand.Startup))]
namespace MoostBrand
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
