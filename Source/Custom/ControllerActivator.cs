using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.Dispatcher;

namespace HipstaLink
{
    class ControllerActivator : IHttpControllerActivator
    {
        readonly Lazy<RouteLinker> linker;

        public ControllerActivator(IApiExplorer explorer)
        {
            linker = new Lazy<RouteLinker>(() => new RouteLinker(explorer));
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var controller = Activator.CreateInstance(controllerType);

            var custom = controller as ControllerBase;
            if (custom == null)
                return (IHttpController) controller;

            custom.Linker = linker.Value;
            
            return (IHttpController)controller;
        }
    }
}