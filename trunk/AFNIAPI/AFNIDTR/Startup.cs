using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AFNIDTR.Startup))]
namespace AFNIDTR
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
