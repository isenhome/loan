using Loan.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loan.Web.Controllers
{
    [LoginFilter]
    public class MainPageController : Controller
    {
        /// <summary>
        /// 加载首页（竖排）
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Function = "主页";
            ViewBag.FunctionURL = "/MainPage/Index";
            ViewBag.FunctionName = "主页展示";
            ViewBag.FunctionNameURL = "/MainPage/Index";
            ViewBag.FunctionDescription = "主页展示一些常用功能信息";
            ViewBag.NavShowStylePageURL = "/MainPage/Index_Horizontal";

            return View();
        }

        /// <summary>
        /// 加载首页（横排）
        /// </summary>
        /// <returns></returns>
        public ActionResult Index_Horizontal()
        {
            ViewBag.Function = "主页";
            ViewBag.FunctionURL = "/MainPage/Index";
            ViewBag.FunctionName = "主页展示";
            ViewBag.FunctionNameURL = "/MainPage/Index";
            ViewBag.FunctionDescription = "主页展示一些常用功能信息";
            ViewBag.NavShowStylePageURL = "/MainPage/Index";

            return View();
        }
    }
}
