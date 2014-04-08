using Loan.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loan.Web.Controllers
{
    [LoginFilter]
    public class CollectingController : Controller
    {
        //
        // GET: /Collecting/
        public ActionResult Index()
        {
            return View();
        }

    }
}
