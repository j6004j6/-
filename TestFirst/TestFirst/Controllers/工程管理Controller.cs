using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestFirst.Models;
using TestFirst.Viewmodel;
using PagedList;

namespace TestFirst.Controllers
{
    // 工程管理的Controller
    public class 工程管理Controller : Controller
    {
        dbMyProjectEntitiesModel db = new dbMyProjectEntitiesModel();
        public ActionResult 管理首頁(CAdvanceQueryCondition selectdata)
        {
            dbMyProjectEntitiesModel db = new dbMyProjectEntitiesModel();
            var table = db.tMission.Select(p => p);

            ViewBag.stage = table.Select(p => p.fStage).Distinct();
            ViewBag.Year = table.Select(p => p.fEST_Time.Substring(0, 4)).Distinct();

            if (!string.IsNullOrEmpty(selectdata.txtName))  //工程名稱
            {
                table = table.Where(p => p.fName.Contains(selectdata.txtName));
            }
            if (!string.IsNullOrEmpty(selectdata.txtStage)) //工程階段
            {
                table = table.Where(p => p.fStage.Contains(selectdata.txtStage));
            }
            if (!string.IsNullOrEmpty(selectdata.txtCode))  //工程代號
            {
                table = table.Where(p => p.fCode.Contains(selectdata.txtCode));
            }
            if (!string.IsNullOrEmpty(selectdata.txtChargeMan))//負責人
            {
                table = table.Where(p => p.fChargeMan.Contains(selectdata.txtChargeMan));
            }

            if (!string.IsNullOrEmpty(selectdata.txtDaystart) && !string.IsNullOrEmpty(selectdata.txtDayEnd))//開工日期
            {

                var searchstartday = selectdata.txtDaystart.Replace("/", "");
                var searchendday = selectdata.txtDayEnd.Replace("/", "");
                /* table = table.Where(p => (p.fToday.Contains(data.projDayStart))); */ /*ok*/
                table = table.Where(p => (p.fEST_Time.CompareTo(searchstartday) >= 0 && p.fEST_Time.CompareTo(searchendday) <= 0));

            }
            if (!string.IsNullOrEmpty(selectdata.txtCompleteDayStart) && !string.IsNullOrEmpty(selectdata.txtCompleteDayEnd))//完工日期
            {
                var searchstartday = selectdata.txtCompleteDayStart.Replace("/", "");
                var searchendday = selectdata.txtCompleteDayEnd.Replace("/", "");
                table = table.Where(p => (p.fReal_Time.CompareTo(searchstartday) >= 0 && p.fReal_Time.CompareTo(searchendday) <= 0));

            }
            if (!string.IsNullOrEmpty(selectdata.txtPaymentStart) && !string.IsNullOrEmpty(selectdata.txtPaymentEnd))// 工程金額
            {
                decimal? start = Convert.ToDecimal(selectdata.txtPaymentStart);
                decimal? end = Convert.ToDecimal(selectdata.txtPaymentEnd);
                table = table.Where(p => (p.fPayment >= start && p.fPayment <= end));
            }
            return View(table);


        }

        public ActionResult 新增專案()
        {
          
            return View();
        }
        [HttpPost]
        public ActionResult 新增專案(tMission theproject)
        {
            dbMyProjectEntitiesModel db = new dbMyProjectEntitiesModel();
            string changedate=theproject.fEST_Time.Substring(0,4)+"/"+ theproject.fEST_Time.Substring(4,2)+"/"+theproject.fEST_Time.Substring(6,2);
           
            var countday = DateTime.Parse(changedate);

            theproject.fReal_Time = (countday.AddDays(Convert.ToDouble(theproject.fSum_Time))).ToString("yyyy/MM/dd").Replace("/","");
            db.tMission.Add(theproject);
           
            db.SaveChanges();
            return RedirectToAction("管理首頁");
        }
        public ActionResult 編輯專案(int fId)
        {
            tMission myproject = (new dbMyProjectEntitiesModel()).tMission.FirstOrDefault(p => p.fId == fId);
            if (myproject == null)
                return RedirectToAction("管理首頁");
            return View(myproject);
          
        }
        [HttpPost]
        public ActionResult 編輯專案(tMission changproject)
        {
            
            tMission olddata = db.tMission.FirstOrDefault(t => t.fId == changproject.fId);
            if (olddata != null)
            {
                olddata.fName = changproject.fName;
                //olddata.fProjectCode = changproject.fProjectCode;
                olddata.fStage = changproject.fStage;
                olddata.fCode = changproject.fCode;
                olddata.fEST_Time = changproject.fEST_Time;
                olddata.fSum_Time = changproject.fSum_Time;

                string changedate = changproject.fEST_Time.Substring(0, 4) + "/" + changproject.fEST_Time.Substring(4, 2) + "/" + changproject.fEST_Time.Substring(6, 2);
                var countday = DateTime.Parse(changedate);
                olddata.fReal_Time = changproject.fReal_Time = (countday.AddDays(Convert.ToDouble(changproject.fSum_Time))).ToString("yyyy/MM/dd").Replace("/", "");

                // olddata.fComplete = changproject.fComplete;
                olddata.fChargeMan = changproject.fChargeMan;
                //olddata.fPhone = changproject.fPhone;
                olddata.fPS = changproject.fPS;
                // olddata.fPayment = changproject.fPayment;
                db.SaveChanges();
            }
            return RedirectToAction("管理首頁");

        }


        //public ActionResult 刪除專案(int fId)
        //{
        //    dbMyProjectEntitiesModel db = new dbMyProjectEntitiesModel();
        //    tMission theproject = db.tMission.FirstOrDefault(p => p.fId == fId);
        //    if (theproject != null)
        //    {
        //        db.tMission.Remove(theproject);
        //        db.SaveChanges();
        //    }
            
        //    return RedirectToAction("管理首頁");
        //}


        public ActionResult 日報列表(CForreport data)
        {
            var table = (new dbMyProjectEntitiesModel()).tMissionDetail.Select(p => p);
            ViewBag.Year = table.Select(p => p.fToday.Substring(0, 4)).Distinct();
            ViewBag.Mouth = table.Select(p => p.fToday.Substring(4, 2)).Distinct();
            if (!string.IsNullOrEmpty(data.projName)) //工程名稱
            {
                table = table.Where(p => p.fName.Contains(data.projName));
            }
            if (!string.IsNullOrEmpty(data.txtcode))  //工程代碼
            {
                table = table.Where(p => p.fCode.Contains(data.txtcode));
            }
            if (!string.IsNullOrEmpty(data.projChargeMan))
            {
                table = table.Where(p => p.fChargeMan.Contains(data.projChargeMan));
            }
            if (!string.IsNullOrEmpty(data.projYear)) //年份搜索
            {

                table = table.Where(p => (p.fToday.Substring(0, 4).Contains(data.projYear)));
            }
            if (!string.IsNullOrEmpty(data.projMouth))//月份搜索
            {

                table = table.Where(p => (p.fToday.Substring(4, 2).Contains(data.projMouth)));
            }
            if (!string.IsNullOrEmpty(data.projYear) && !string.IsNullOrEmpty(data.projMouth) && !string.IsNullOrEmpty(data.projWeekend))//週期搜索
            {
                if (data.projWeekend == "第一週")
                {
                    var searchstartday = data.projYear + data.projMouth + "01";
                    var searchendday = data.projYear + data.projMouth + "07";
                    table = table.Where(p => (p.fToday.CompareTo(searchstartday) >= 0 && p.fToday.CompareTo(searchendday) <= 0));
                }
                if (data.projWeekend == "第二週")
                {
                    var searchstartday = data.projYear + data.projMouth + "08";
                    var searchendday = data.projYear + data.projMouth + "14";
                    table = table.Where(p => (p.fToday.CompareTo(searchstartday) >= 0 && p.fToday.CompareTo(searchendday) <= 0));
                }
                if (data.projWeekend == "第三週")
                {
                    var searchstartday = data.projYear + data.projMouth + "15";
                    var searchendday = data.projYear + data.projMouth + "21";
                    table = table.Where(p => (p.fToday.CompareTo(searchstartday) >= 0 && p.fToday.CompareTo(searchendday) <= 0));
                }
                if (data.projWeekend == "第四週")
                {
                    var searchstartday = data.projYear + data.projMouth + "22";
                    var searchendday = data.projYear + data.projMouth + "28";
                    table = table.Where(p => (p.fToday.CompareTo(searchstartday) >= 0 && p.fToday.CompareTo(searchendday) <= 0));
                }
                if (data.projWeekend == "第五週")
                {
                    var searchstartday = data.projYear + data.projMouth + "29";
                    var searchendday = data.projYear + data.projMouth + "31";
                    table = table.Where(p => (p.fToday.CompareTo(searchstartday) >= 0 && p.fToday.CompareTo(searchendday) <= 0));
                }
            }
            if (!string.IsNullOrEmpty(data.projDayStart) && !string.IsNullOrEmpty(data.projDayEnd))  //日期搜索
            {
                var searchstartday = data.projDayStart.Replace("/", "");
                var searchendday = data.projDayEnd.Replace("/", "");
                /* table = table.Where(p => (p.fToday.Contains(data.projDayStart))); */ /*ok*/
                table = table.Where(p => (p.fToday.CompareTo(searchstartday) >= 0 && p.fToday.CompareTo(searchendday) <= 0));

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
            theReport.fToday = DateTime.Now.ToShortDateString().Replace("/", "0");
            CUser ChargeMan = new CUser();
            ChargeMan = (CUser)Session[CDictionary.SK_CURRENT_LOGINED_USER];
            theReport.fChargeMan = ChargeMan.fAccount;
            db.tMissionDetail.Add(theReport);
            db.SaveChanges();
            var thecode = theReport.fCode;

            return RedirectToAction("日報列表", new { txtcode = thecode });

        }
        public ActionResult 編輯日報(int fId)
        {
            tMissionDetail Olddata = (new dbMyProjectEntitiesModel()).tMissionDetail.FirstOrDefault(t => t.fId == fId);

            return View(Olddata);
        }
        [HttpPost]
        public ActionResult 編輯日報(tMissionDetail changereport)
        {
            dbMyProjectEntitiesModel db = new dbMyProjectEntitiesModel();
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
            var thecode = changereport.fCode;

            return RedirectToAction("日報列表", new { txtcode = thecode });
          
        }
        
        public ActionResult 刪除日報(int fId)
        {
            
            tMissionDetail theproject = db.tMissionDetail.FirstOrDefault(p => p.fId == fId);
            if (theproject != null)
            {
                dbMyProjectEntitiesModel db = new dbMyProjectEntitiesModel();
                db.tMissionDetail.Remove(theproject);
                db.SaveChanges();
            }
            var thecode = theproject.fCode;

            return RedirectToAction("日報列表", new { txtcode = thecode });
           
        }
        public ActionResult PostdatatoDB(string MyfCode, string MyfPayment)
        {
            try
            {
                tMission_SignOff Newdata = new tMission_SignOff();
                Newdata.fMissionCode = MyfCode;
                Newdata.fMoney = Convert.ToDecimal(MyfPayment);
                db.tMission_SignOff.Add(Newdata);
                db.SaveChanges();
                return Json(Newdata,JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message,JsonRequestBehavior.AllowGet);

            }
                        
        }
        public ActionResult AJAX_Check()
        {
            List<List<string>> list = new List<List<string>>();
            list.Add(db.tMission_SignOff.Where(p => p.fStatus == "簽核完畢").Select(p => p.fMissionCode).ToList());
            list.Add(db.tMission_SignOff.Where(p => p.fStatus == "已駁回").Select(p => p.fMissionCode).ToList());
            list.Add(db.tMission_SignOff.Where(p => p.fStatus == "待主管核准").Select(p => p.fMissionCode).ToList());
            list.Add(db.tMission_SignOff.Where(p => p.fStatus == "完工待審").Select(p => p.fMissionCode).ToList());
            return Json(list, JsonRequestBehavior.AllowGet);
        }


    }

}