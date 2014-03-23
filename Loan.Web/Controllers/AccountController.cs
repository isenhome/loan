using Loan.Core;
using Loan.Core.Models;
using Loan.Entity.Query;
using Loan.Logic;
using Loan.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loan.Web.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

        #region 登陆相关

        #region 登陆页面
        /// <summary>
        /// 登陆页面
        /// </summary>
        /// <remarks>
        /// 创建：李真 2013-11-16
        /// </remarks>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
        #endregion

        #region 登陆
        /// <summary>
        /// 登陆
        /// </summary>
        /// <remarks>
        /// 创建：李真 2013-11-16
        /// </remarks>
        /// <param name="loginModel">登陆信息实体类</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginVModel loginModel)
        {
            if (ModelState.IsValid)
            {
                if (loginModel.ValidCode != Session[GlobalConst.SESSIONNAME_VALIDCODE_LOGIN].ToString())
                {
                    ModelState.AddModelError("", "验证码不正确！");
                }
                else
                {
                    UserQuery userQuery = new UserQuery();
                    userQuery.UserName = loginModel.UserName;
                    userQuery.Password = loginModel.Password;
                    UserLogic userLogic = new UserLogic();
                    
                    if(userLogic.CheckUser(userQuery))
                    {
                         //记录SESSION登陆信息
                    Session[GlobalConst.SESSIONNAME_USERNAME] = loginModel.UserName;

                    //跳转到系统主页
                    return RedirectToAction("Default", "ZhuYe");
                    }
                    else
                    {
                        ModelState.AddModelError("", "用户名或密码错误！");
                    }
                }                
            }
            return View(loginModel);
        }
        #endregion
        #endregion

        #region 获取验证码
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <remarks>
        /// 创建：李真 2013-11-16
        /// </remarks>
        /// <returns></returns>
        public ActionResult GetValidCode(string sessionName)
        {
            string code = ObjectExtension.CreateValidateCode(5);
            Session[sessionName] = code;
            byte[] bytes = ObjectExtension.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }
        #endregion

    }
}
