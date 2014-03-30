using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loan.Web.Controllers
{
    public class MainPageController : Controller
    {
        /// <summary>
        /// 加载首页（竖排）
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 加载首页（横排）
        /// </summary>
        /// <returns></returns>
        public ActionResult Index_Horizontal()
        {
            return View();
        }
    }
}
