using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SueldosYjornales.Startup))]
namespace SueldosYjornales
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
