using Lab_Works.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Routing;
using System.Web.Routing;

namespace Lab_Works
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            var cR = new DefaultInlineConstraintResolver();
            cR.ConstraintMap.Add("usertype", typeof(UserTypeConstraint));
            routes.MapMvcAttributeRoutes(cR);

            routes.MapRoute(
                name: "Default",
                url: "{usertype}/{controller}/{action}/{id}",
                defaults: new { usertype = "System", controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { usertype = new UserTypeConstraint() }
            );
        }
    }
}
