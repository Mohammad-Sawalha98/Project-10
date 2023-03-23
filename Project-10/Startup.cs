using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Project_10.Startup))]
namespace Project_10
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
