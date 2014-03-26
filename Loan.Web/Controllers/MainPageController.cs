using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loan.Web.Controllers
{
    public class MainPageController : Controller
    {
        //
        // GET: /MainPage/

        public ActionResult Index()
        {
            ViewBag.Title = "青蚨信贷";
            ViewBag.PageTiTle = "页面标题";
            ViewBag.PageTiTleDescription = "页面说明";
            ViewBag.BreadcrumbItem1 = "功能模块名";
            ViewBag.BreadcrumbItem2 = "功能名称";
            return View();
        }

    }
}
