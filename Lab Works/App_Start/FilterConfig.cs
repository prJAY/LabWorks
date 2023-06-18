using Lab_Works.Filters;
using System.Web;
using System.Web.Mvc;

namespace Lab_Works
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomErrorHandler());
        }
    }
}
