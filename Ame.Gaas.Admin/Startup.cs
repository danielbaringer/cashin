using Owin;
using System.Configuration;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Ame.Gaas.Admin
{
    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(name: "DefaultApi", routeTemplate: "api/{controller}/{action}/{id}", defaults: new { id = RouteParameter.Optional });

            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());

        }

    }
}