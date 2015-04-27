using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace HipstaLink
{
    public static class Config
    {
        public static void Register(HttpConfiguration config)
        {
            ConfigureDiscovery(config);
            ConfigureActivation(config);
            ConfigureFormatters(config);
            ConfigureRoutes(config);
        }

        static void ConfigureDiscovery(HttpConfiguration config)
        {
            config.Services.Replace(
                typeof(IHttpControllerTypeResolver),
                new ControllerTypeResolver());
        }

        static void ConfigureActivation(HttpConfiguration config)
        {
            var explorer = config.Services.GetApiExplorer();

            config.Services.Replace(
                typeof(IHttpControllerActivator),
                new ControllerActivator(explorer));
        }

        static void ConfigureFormatters(HttpConfiguration config)
        {
            config.Formatters.Clear();
            config.Formatters.Add(new MediaTypeFormatter());
        }

        static void ConfigureRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
            );
        }
    }
}
