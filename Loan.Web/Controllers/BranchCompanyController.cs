using Loan.Entity.DB;
using Loan.Logic;
using Loan.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using Loan.Core;

namespace Loan.Web.Controllers
{
    public class BranchCompanyController : Controller
    {
        BranchCompanyLogic branchCompanyLogic = new BranchCompanyLogic();

        /// <summary>
        /// add new branch company
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            ViewBag.Function = "分公司管理";
            ViewBag.FunctionURL = "/BranchCompany/Add";
            ViewBag.FunctionName = "添加分公司";
            ViewBag.FunctionNameURL = "/BranchCompany/Add";
            ViewBag.FunctionDescription = "";
            ViewBag.NavShowStylePageURL = "/BranchCompany/Add_Horizontal";

            return View();
        }

        #region 新增一个分公司
        /// <summary>
        /// 新增一个分公司
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-04-13
        /// </remarks>
        /// <param name="branchCompanyV"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(BranchCompanyVModel branchCompanyV)
        {
            if (ModelState.IsValid)
            {                 
                BranchCompany branchCompany = new BranchCompany();
                branchCompany.Name = branchCompanyV.Name;
                branchCompany.Address = branchCompanyV.Address;
                branchCompany.PostCode = branchCompanyV.PostCode;
                branchCompany.Description = branchCompanyV.Description;

                int iResult = branchCompanyLogic.Add(branchCompany);
                if (iResult >= 0)
                { 
                    //新增成功

                }
            }
            return View(branchCompanyV);
        }
        #endregion

        #region 获取分公司列表
        /// <summary>
        /// 获取分公司列表
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-04-14
        /// </remarks>
        /// <param name="id">当前页码</param>
        /// <returns></returns>
        public ActionResult GetList(int id = 1)
        {
            ViewBag.Function = "分公司";
            ViewBag.FunctionURL = "/BranchCompany/GetList";
            ViewBag.FunctionName = "分公司管理";
            ViewBag.FunctionNameURL = "/BranchCompany/GetList";
            ViewBag.FunctionDescription = "";
            ViewBag.NavShowStylePageURL = "/BranchCompany/GetList_Horizontal";
            
            int pageindex = id;
            var info = branchCompanyLogic.GetList(GlobalConst.PAGESIZE, pageindex);
            return View(info.Items.ToPagedList(pageindex, GlobalConst.PAGESIZE, info.TotalItems.TryInt()));
        }
        #endregion
    }
}
