using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestFirst.Models;
using TestFirst.Viewmodel;

namespace TestFirst.Controllers
{
    public class 工程管理Controller : Controller
    {
       
        // GET: 工程管理
       
        public ActionResult 管理首頁(CAdvanceQueryCondition selectdata)  
        {
            var table = (new dbMyProjectEntities()).tMission.Select(p => p);
            if (!string.IsNullOrEmpty(selectdata.txtName))
            {
                table = table.Where(p => p.fName.Contains(selectdata.txtName));
            }
            if (!string.IsNullOrEmpty(selectdata.txtStage))
            {
                table = table.Where(p => p.fStage.Contains(selectdata.txtStage));
            }
            if (!string.IsNullOrEmpty(selectdata.txtCode))
            {
                table = table.Where(p => p.fCode.Contains(selectdata.txtCode));
            }
            if (!string.IsNullOrEmpty(selectdata.txtChargeMan))
            {
                table = table.Where(p => p.fChargeMan.Contains(selectdata.txtChargeMan));
            }
            if (!string.IsNullOrEmpty(selectdata.txtConfirmer))
            {
                table = table.Where(p => p.fConfirmer.Contains(selectdata.txtConfirmer));
            }
            //if (!string.IsNullOrEmpty(selectdata.日期))
            //{
               
            //}
            //if (!string.IsNullOrEmpty(selectdata.完成狀態))
            //{
            //    table = table.Where(p => p.fConfirmer.Contains(selectdata.txtConfirmer));
            //}
            if (!string.IsNullOrEmpty(selectdata.txtPaymentStart) && !string.IsNullOrEmpty(selectdata.txtPaymentEnd))
            {
                decimal? start = Convert.ToDecimal(selectdata.txtPaymentStart);
                decimal? end = Convert.ToDecimal(selectdata.txtPaymentEnd);
                table = table.Where(p => (p.fPayment >= start && p.fPayment <= end));
            }
            return View(table);



            //============================
            //dbMyProjectEntities db = new dbMyProjectEntities();
            //var table = from project in db.tMission
            //            select project;
            //return View(table);
        }
        public ActionResult 新增專案()
        {
          
            return View();
        }
        [HttpPost]
        public ActionResult 新增專案(tMission theproject)
        {
            dbMyProjectEntities db = new dbMyProjectEntities();
            db.tMission.Add(theproject);
            db.SaveChanges();
            return RedirectToAction("管理首頁");
        }
        public ActionResult 編輯專案(int fId)
        {
            tMission myproject = (new dbMyProjectEntities()).tMission.FirstOrDefault(p => p.fId == fId);
            if (myproject == null)
                return RedirectToAction("List");
            return View(myproject);
          
        }
        [HttpPost]
        public ActionResult 編輯專案(tMission changproject)
        {
            dbMyProjectEntities db = new dbMyProjectEntities();
            tMission olddata = db.tMission.FirstOrDefault(t => t.fId == changproject.fId);
            if (olddata != null)
            {
                olddata.fName = changproject.fName;
                olddata.fProjectCode = changproject.fProjectCode;
                olddata.fStage = changproject.fStage;
                olddata.fCode = changproject.fCode;
                olddata.fEST_Time = changproject.fEST_Time;
                olddata.fSum_Time = changproject.fSum_Time;
                olddata.fReal_Time = changproject.fReal_Time;
                olddata.fComplete = changproject.fComplete;
                olddata.fChargeMan = changproject.fChargeMan;
                olddata.fConfirmer = changproject.fConfirmer;
                olddata.fPS = changproject.fPS;
                olddata.fPayment = changproject.fPayment;
                db.SaveChanges();
            }
            return RedirectToAction("管理首頁");
            
        }


        public ActionResult 刪除專案(int fId)
        {
            dbMyProjectEntities db = new dbMyProjectEntities();
            tMission theproject = db.tMission.FirstOrDefault(p => p.fId == fId);
            if (theproject != null)
            {
                db.tMission.Remove(theproject);
                db.SaveChanges();
            }
            
            return RedirectToAction("管理首頁");
        }


        public ActionResult 日報列表(CForreport data)
        {
            var table = (new dbMyProjectEntities()).tMissionDetail.Select(p=>p);
            
                       
            if (!string.IsNullOrEmpty(data.txtcode))
            {
                table = table.Where(p => p.fCode.Contains(data.txtcode));
            }
         
            return View(table);
                       
           
        }
        public ActionResult 新增日報()
        {
           return View();
        }
        [HttpPost]
        public ActionResult 新增日報(tMissionDetail theReport)
        {
            dbMyProjectEntities db = new dbMyProjectEntities();
            db.tMissionDetail.Add(theReport);
            db.SaveChanges();

           
            return RedirectToAction("日報列表");

        }
        public ActionResult 編輯日報(int fId)
        {
            tMissionDetail Olddata = (new dbMyProjectEntities()).tMissionDetail.FirstOrDefault(t => t.fId == fId);

            return View(Olddata);
        }
        [HttpPost]
        public ActionResult 編輯日報(tMissionDetail changereport)
        {
            dbMyProjectEntities db = new dbMyProjectEntities();
            tMissionDetail olddata = db.tMissionDetail.FirstOrDefault(t => t.fId == changereport.fId);
            if (olddata != null)
            {
                olddata.fCode = changereport.fCode;
                olddata.fName = changereport.fName;
                olddata.fMaterial = changereport.fMaterial;
                olddata.fChargeMan = changereport.fChargeMan;
                olddata.fWorkers = changereport.fWorkers;
                olddata.fWeather = changereport.fWeather;
                olddata.fToday = changereport.fToday;
                olddata.fPhoto = changereport.fPhoto;
                olddata.fPS = changereport.fPS;
                db.SaveChanges();
            }

            return RedirectToAction("日報列表");
        }
        public ActionResult 刪除日報(int fId)
        {
            dbMyProjectEntities db = new dbMyProjectEntities();
            tMissionDetail theproject = db.tMissionDetail.FirstOrDefault(p => p.fId == fId);
            if (theproject != null)
            {
                db.tMissionDetail.Remove(theproject);
                db.SaveChanges();
            }

            return RedirectToAction("管理首頁");
        }

    }
}