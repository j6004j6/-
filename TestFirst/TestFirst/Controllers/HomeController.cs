using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TestFirst.Models;
using TestFirst.Viewmodel;

namespace TestFirst.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult 首頁()
        {
            if (Session[CDictionary.SK_CURRENT_LOGINED_USER] == null)
            {
                return RedirectToAction("登入系統");
            }
            else
            {
                CUser now = (CUser)Session[CDictionary.SK_CURRENT_LOGINED_USER];
                ViewBag.user = now.fAccount;
                return View();
            }
        }
        [HttpGet]
        public ActionResult 登入系統()
        {
            if (Session[CDictionary.SK_CURRENT_LOGINED_USER]!=null)
            {
                return RedirectToAction("首頁");
            }
            else
            {
                return View();
            }
            
        }
        [HttpPost]
        public ActionResult 登入系統(CLogin data)
        {
            CUser MyUser = (new CUserFactory()).getByAccount(data.txtAccount);
            if (MyUser != null)
            {
                string Mypassword = data.txtPassword;

                Mypassword = FormsAuthentication.HashPasswordForStoringInConfigFile(Mypassword, "Sha1");
                if (MyUser.fAccount.Equals(data.txtAccount) && MyUser.fPassword.Equals(Mypassword))
                {
                    Session[CDictionary.SK_CURRENT_LOGINED_USER] = MyUser;
                    return RedirectToAction("首頁");
                }
                else
                {
                    ViewBag.log_result = "密碼錯誤，請再試一次";
                    return View();
                }
            }
            else
            {
                ViewBag.log_result = "登入失敗";
                return View();
            }
        }
        
        public ActionResult 工程款簽核 ()
        {
            return View();
        }
        public ActionResult 照片管理()
        {
            

            return View();
        }
        public ActionResult 資料分析()
        {
           

            return View();
        }
        public ActionResult 後台管理()
        {
           

            return View();
        }
        public ActionResult 留言板()
        {
            

            return View();
        }


        // 測試看User List
        public ActionResult 會員資料列表()
        {
            List<CUser> myuser = (new CUserFactory()).getall();
            return View(myuser);
        }
        // 測試用新增User
        public ActionResult 新增會員()
        {
            return View();
        }
        [HttpPost]
        public ActionResult 新增會員(CUser newuser)
        {
            (new CUserFactory()).insertdb(newuser);
            return RedirectToAction("登入系統");
        }

        // 登出
        public ActionResult 登出()
        {
            System.Web.HttpContext.Current.Session[CDictionary.SK_CURRENT_LOGINED_USER] = null;
            Session[CDictionary.SK_CURRENT_LOGINED_USER] = null;
            return RedirectToAction("登入系統");
        }

    }
}
