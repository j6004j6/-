using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestFirst.Models;

namespace TestFirst.Controllers
{
    public class 工程管理Controller : Controller
    {
        dbMyProjectEntities db = new dbMyProjectEntities();
        // GET: 工程管理
        public ActionResult FirstView()
        {
            var q = from p in db.tMission
                    select p;

            return View(q.ToList());
        }
    }
}