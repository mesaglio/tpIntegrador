using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TP_previo.Startup))]
namespace TP_previo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
