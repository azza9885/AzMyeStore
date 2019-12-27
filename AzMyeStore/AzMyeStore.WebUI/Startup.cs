using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AzMyeStore.WebUI.Startup))]
namespace AzMyeStore.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
