using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace HipstaLink
{
    public class ControllerTypeResolver : DefaultHttpControllerTypeResolver
    {
        public ControllerTypeResolver()
            : base(IsController)
        {}

        static bool IsController(Type type)
        {
            return
                type.IsClass &&
                type.IsVisible &&
                !type.IsAbstract &&
                typeof(ApiController).IsAssignableFrom(type);
        }
    }
}