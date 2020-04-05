using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VidlyWithLIEN.Startup))]
namespace VidlyWithLIEN
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
