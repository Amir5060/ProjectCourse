using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectCourse.Startup))]
namespace ProjectCourse
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
