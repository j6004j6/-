using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using viewmodel.Models;
using viewmodel.ViewModel;
using iii.CUser;


namespace viewmodel.Controllers
{
    public class HomeController : Controller
    {
     

        public ActionResult List()
        {
            List<CUser> myuser = (new CUserFactory()).getall();
            return View(myuser);
        }

        public ActionResult Logon(CLogin data)
        {

            CUser MyUser = (new CUserFactory()).getByAccount(data.txtAccount);
            if (MyUser != null)
            {
                if (MyUser.fAccount.Equals(data.txtAccount) && MyUser.fPassword.Equals(data.txtPassword))
                {
                    Session[CDictionary.SK_CURRENT_LOGINED_USER] = MyUser;
                 
                    return RedirectToAction("firstpage");
                }
            }
            
           
            return View();

        }
        public ActionResult newmemeber()
        {            
            return View();
        }
        [HttpPost]
        public ActionResult newmemeber(CUser newuser)
        {
            (new CUserFactory()).insertdb(newuser);

            return RedirectToAction("Logon");
        }


        public ActionResult firstpage()
        {
            if (Session[CDictionary.SK_CURRENT_LOGINED_USER] == null)
            {
                return RedirectToAction("Logon");
            }
            
            CUser now = (CUser)Session[CDictionary.SK_CURRENT_LOGINED_USER];
            ViewBag.now = now.fAccount;
            return View();
        }
    }
}