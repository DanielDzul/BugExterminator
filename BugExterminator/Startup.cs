using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BugExterminator.Startup))]
namespace BugExterminator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
