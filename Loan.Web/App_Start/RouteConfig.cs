using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Loan.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "ValidCode", // 获取验证码
                "Account/GetValidCode/{sessionName}", // URL with parameters
                new { controller = "Account", action = "GetValidCode" } // Parameter defaults
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "MainPage", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}