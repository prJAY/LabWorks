using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Lab_Works.Constraints
{
    public class UserTypeConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            object value;
            string[] types = { "admin", "technician", "faculty", "student" , "system"};
            if (values.TryGetValue(parameterName, out value) && value != null)
            {
                return types.Contains(value.ToString().ToLower());
            }
            return true;
        }
    }
}