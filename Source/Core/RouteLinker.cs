using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.Routing;

namespace HipstaLink
{
    public class RouteLinker
    {
        readonly Dictionary<MethodInfo, IHttpRoute> methods;

        public RouteLinker(IApiExplorer explorer)
        {
            methods = explorer.ApiDescriptions.ToDictionary(
                x => ((ReflectedHttpActionDescriptor)x.ActionDescriptor).MethodInfo,
                x => x.Route);
        }

        public RouteLink Build(HttpRequestMessage request, MethodCallExpression call)
        {
            if (call == null)
                throw new InvalidOperationException();

            var arguments = BuildArguments(call);
            var route = LocateRoute(call);

            if (route == null)
                throw new InvalidOperationException("Can't find route for: " + call.Method);

            var routePath = route.GetVirtualPath(request,
                new HttpRouteValueDictionary(arguments) {{ HttpRoute.HttpRouteKey, true }});

            if (routePath == null)
                throw new InvalidOperationException("Can't map virtual path for this route: " + route.RouteTemplate);

            var rootPath = request.GetRequestContext().VirtualPathRoot;
            var scheme = request.RequestUri.GetLeftPart(UriPartial.Authority);

            var relativeUri = routePath.VirtualPath;
            var absoluteUri = new Uri(new Uri(new Uri(scheme), rootPath), relativeUri);

            return new RouteLink(route, call.Method, arguments, relativeUri, absoluteUri);
        }

        static Dictionary<string, object> BuildArguments(MethodCallExpression call)
        {
            var arguments = new Dictionary<string, object>();

            for (var i = 0; i < call.Method.GetParameters().Length; i++)
            {
                var value = Expression.Lambda(call.Arguments[i]).Compile().DynamicInvoke();
                var name = call.Method.GetParameters()[i].Name;

                arguments[name] = value;
            }

            return arguments;
        }

        IHttpRoute LocateRoute(MethodCallExpression call)
        {
            var route = Find(call.Method);

            if (route == null)
                throw new ArgumentException(
                    string.Format("Can't find route for the action method: {0}", call.Method));

            return route;
        }

        IHttpRoute Find(MethodInfo method)
        {
            IHttpRoute result;
            return methods.TryGetValue(method, out result) ? result : null;
        }
    }

    public struct RouteLink
    {
        public readonly IHttpRoute Route;
        public readonly MethodInfo Method;
        public readonly Dictionary<string, object> Arguments;
        public readonly string RelativeUri;
        public readonly Uri AbsoluteUri;

        public RouteLink(
            IHttpRoute route, 
            MethodInfo method, 
            Dictionary<string, object> arguments, 
            string relativeUri, 
            Uri absoluteUri)
        {
           Route = route;
           Method = method;
           Arguments = arguments;
           RelativeUri = relativeUri;
           AbsoluteUri = absoluteUri;
        }
    }
}
