using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AppSystem.Startup))]
namespace AppSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           // ConfigureAuth(app);
        }
    }
}
