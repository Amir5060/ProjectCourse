using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectCourse.Models
{
    public class CheckAuthorize : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //you can change to any controller or html page.
            //filterContext.Result = new RedirectResult("UnAuthorize");
            filterContext.Result = new RedirectResult("/CheckAuthorize/UnAuthorize");
            //filterContext.Result = new RedirectToRouteResult(new
            //        RouteValueDictionary(new { controller = "CheckAuthorize", action = "AccessDenied" }));

        }
    }
}