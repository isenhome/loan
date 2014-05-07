using Loan.Core;
using Loan.Entity.DB;
using Loan.Logic;
using Loan.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loan.Web.Controllers
{
    public class UsersController : Controller
    {
        UserLogic userLogic = new UserLogic();

        /// <summary>
        /// add new branch company
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            ViewBag.Function = "用户管理";
            ViewBag.FunctionURL = "/Users/Add";
            ViewBag.FunctionName = "添加新用户";
            ViewBag.FunctionNameURL = "/Users/Add";
            ViewBag.FunctionDescription = "";
            ViewBag.NavShowStylePageURL = "/Users/Add_Horizontal";

            return View();
        }

        #region 新增用户
        /// <summary>
        /// 新增一个分公司
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-05-07
        /// </remarks>
        /// <param name="branchCompanyV"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(UsersVModel usersV)
        {
            if (ModelState.IsValid)
            {
                Users user = new Users();
                user.UserTypeID = usersV.UserTypeID;
                user.BranchCompanyID = usersV.BranchCompanyID;
                user.UserName = usersV.UserName;
                user.RealName = usersV.RealName;
                user.Gender = usersV.Gender;
                user.EmployeeNO = usersV.EmployeeNO;
                user.Cellphone = usersV.Cellphone;
                user.CreateTime = DateTime.Now;
                user.UpdateTime = DateTime.Now;

                int iResult = userLogic.Add(user);
                if (iResult >= 0)
                {
                    //新增成功
                    ViewBag.AlertMessage = "新增成功";
                    return View(new UsersVModel());
                }
            }
            return View(usersV);
        }
        #endregion

        #region 获取用户列表
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-05-07
        /// </remarks>
        /// <param name="id">当前页码</param>
        /// <returns></returns>
        public ActionResult GetList(int id = 1)
        {
            ViewBag.Function = "用户";
            ViewBag.FunctionURL = "/Users/GetList";
            ViewBag.FunctionName = "用户管理";
            ViewBag.FunctionNameURL = "/Users/GetList";
            ViewBag.FunctionDescription = "";
            ViewBag.NavShowStylePageURL = "/Users/GetList_Horizontal";

            int pageindex = id;
            var info = userLogic.GetList(GlobalConst.PAGESIZE, pageindex);
            return View(info.Items.ToPagedList(pageindex, GlobalConst.PAGESIZE, info.TotalItems.TryInt()));
        }
        #endregion

        #region 禁用用户
        /// <summary>
        /// 禁用用户
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-05-07
        /// </remarks>
        /// <param name="id">userID</param>
        /// <param name="pageindex">列表当前页码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Lock(int id, int pageindex)
        {
            int result = userLogic.ChangeStatus(id, GlobalConst.Status.Locked);
            if (result >= 0)
            {
                ViewBag.AlertMessage = "禁用成功";
            }
            else
            {
                ViewBag.AlertMessage = "禁用失败";
            }

            var info = userLogic.GetList(GlobalConst.PAGESIZE, pageindex);
            return View("GetList", info.Items.ToPagedList(pageindex, GlobalConst.PAGESIZE, info.TotalItems.TryInt()));
        }
        #endregion

        #region 启用用户
        /// <summary>
        /// 启用用户
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-05-07
        /// </remarks>
        /// <param name="id">userID</param>
        /// <param name="pageindex">列表当前页码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UnLock(int id, int pageindex)
        {
            int result = userLogic.ChangeStatus(id, GlobalConst.Status.Normal);
            if (result >= 0)
            {
                ViewBag.AlertMessage = "启用成功";
            }
            else
            {
                ViewBag.AlertMessage = "启用失败";
            }

            var info = userLogic.GetList(GlobalConst.PAGESIZE, pageindex);
            return View("GetList", info.Items.ToPagedList(pageindex, GlobalConst.PAGESIZE, info.TotalItems.TryInt()));
        }
        #endregion

        #region 删除用户
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-05-07
        /// </remarks>
        /// <param name="id">userID</param>
        /// <param name="pageindex">列表当前页码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id, int pageindex)
        {
            int result = userLogic.ChangeStatus(id, GlobalConst.Status.Delete);
            if (result >= 0)
            {
                ViewBag.AlertMessage = "删除成功";
            }
            else
            {
                ViewBag.AlertMessage = "删除失败";
            }

            var info = userLogic.GetList(GlobalConst.PAGESIZE, pageindex);
            if (info.TotalItems.TryInt() == 0 && pageindex > 1)
            {
                pageindex = pageindex - 1;
                info = userLogic.GetList(GlobalConst.PAGESIZE, pageindex);
            }
            return View("GetList", info.Items.ToPagedList(pageindex, GlobalConst.PAGESIZE, info.TotalItems.TryInt()));
        }
        #endregion

        #region 编辑用户
        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <remarks>
        /// 创建:李真 2014-05-07
        /// </remarks>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            ViewBag.Function = "用户管理";
            ViewBag.FunctionURL = "/Users/Edit";
            ViewBag.FunctionName = "编辑用户";
            ViewBag.FunctionNameURL = "/Users/Edit";
            ViewBag.FunctionDescription = "";
            ViewBag.NavShowStylePageURL = "/Users/Edit_Horizontal";

            Users user = userLogic.GetUserByID(id);
            UsersVModel userV = new UsersVModel();
            if (null != userV)
            {
                userV.UserID = user.UserID;
                userV.UserName = user.UserName;
                userV.UserTypeID = user.UserTypeID;
                userV.BranchCompanyID = user.BranchCompanyID;
                userV.EmployeeNO = user.EmployeeNO;
                userV.RealName = user.RealName;
                userV.Gender = user.Gender;
                userV.Cellphone = user.Cellphone;
            }

            return View("Edit", userV);
        }
        #endregion

        #region 编辑用户
        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <remarks>
        /// 创建:李真 2014-05-07
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(UsersVModel userV)
        {
            if (ModelState.IsValid)
            {
                Users user = new Users();
                user.UserID = userV.UserID;
                user.UserName = userV.UserName;
                user.UserTypeID = userV.UserTypeID;
                user.BranchCompanyID = userV.BranchCompanyID;
                user.EmployeeNO = userV.EmployeeNO;
                user.RealName = userV.RealName;
                user.Gender = userV.Gender;
                user.Cellphone = userV.Cellphone;

                int iResult = userLogic.Edit(user);
                if (iResult >= 0)
                {
                    //修改成功
                    ViewBag.AlertMessage = "修改成功";
                    return View("GetList");
                }
            }
            return View(userV);
        }
        #endregion
    }
}
