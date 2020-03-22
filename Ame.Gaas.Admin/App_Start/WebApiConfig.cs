using System.Web.Http;
using Ame.Gaas.Admin.Filters;
using System.Web.Http.Cors;
using System.Net.Http.Formatting;

namespace Ame.Gaas.Admin
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.Filters.Add(new BasicAuthenticationAttribute());
            
            config.MapHttpAttributeRoutes();

            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "GET,POST");

            config.EnableCors(cors);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var formatters = GlobalConfiguration.Configuration.Formatters;
            formatters.Remove(formatters.XmlFormatter);

            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());

        }
    }
}
