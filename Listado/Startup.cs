using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Listado.Startup))]
namespace Listado
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
