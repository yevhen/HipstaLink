using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HipstaLink
{
    public abstract class ControllerBase : ApiController
    {
        public RouteLinker Linker;

        protected Link LinkTo<TController>(string name, Expression<Action<TController>> action) where TController : ApiController
        {
            return new Link(name, Linker.Build(Request, action.Body as MethodCallExpression));
        }

        protected Link LinkSelf<TController>(Expression<Action<TController>> action) where TController : ApiController
        {
            return new Link(Linker.Build(Request, action.Body as MethodCallExpression));
        }

        protected Link LinkTo<TController>(Expression<Action<TController>> action) where TController : ApiController
        {
            return new Link(Linker.Build(Request, action.Body as MethodCallExpression));
        }

        protected Link LinkTo<TController, TResult>(string name, Expression<Func<TController, TResult>> action) where TController : ApiController
        {
            return new Link(Linker.Build(Request, action.Body as MethodCallExpression));
        }

        protected Link LinkSelf<TController, TResult>(Expression<Func<TController, TResult>> action) where TController : ApiController
        {
            return new Link(Linker.Build(Request, action.Body as MethodCallExpression));
        }

        protected Link LinkTo<TController, TResult>(Expression<Func<TController, TResult>> action) where TController : ApiController
        {
            return new Link(Linker.Build(Request, action.Body as MethodCallExpression));
        }

        protected Uri UriOf<TController, TResult>(Expression<Func<TController, TResult>> action) where TController : ApiController
        {
            return LinkTo(action).Href;
        }

        protected Uri UriOf<TController>(Expression<Action<TController>> action) where TController : ApiController
        {
            return LinkTo(action).Href;
        }

        protected IHttpActionResult Created(string location)
        {
            return Created<object>(location, null);
        }

        protected IHttpActionResult Created(Uri location)
        {
            return Created<object>(location, null); ;
        }

        protected HttpResponseMessage Moved(Uri location)
        {
            return Location(HttpStatusCode.MovedPermanently, location);
        }

        protected HttpResponseMessage Location(HttpStatusCode status, Uri location)
        {
            var response = Request.CreateResponse(status);
            response.Headers.Location = location;

            return response;
        }
    }
}