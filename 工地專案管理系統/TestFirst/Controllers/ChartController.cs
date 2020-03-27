using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestFirst.Models;

namespace TestFirst.Controllers
{
    public class ChartController : Controller
    {
        dbMyProjectEntities1 db = new dbMyProjectEntities1();
        
        // GET: Chart
        public ActionResult chartProject()
        {
            var q = from p in db.tProject
                    select p;
            List<chartProject> cp = new List<chartProject>();
            List<string> lab2 = new List<string>();
            List<decimal> val2 = new List<decimal>();
            foreach(var item in q)
            {
                var daydiff =new TimeSpan( item.fEST_EndDate.Ticks - item.fEST_StartDate.Ticks).Days;
                chartProject c = new chartProject();
                c.fEst_StartDate = item.fEST_StartDate;
                c.fEst_EndDate = item.fEST_EndDate;
                if (item.fReal_StartDate!=null)
                {
                    c.fReal_StartDate = (DateTime)item.fReal_StartDate;
                }
                if (item.fReal_EndDate != null)
                {
                    c.fReal_EndDate = (DateTime)item.fReal_EndDate;
                }
                cp.Add(c);
                lab2.Add(item.fName);
                val2.Add((decimal)item.fMoney);
            }
            ViewBag.lab2 = lab2;
            ViewBag.val2 = val2;
            return View(cp);
        }

        public ActionResult chartMission()
        {
            var q = from p in db.tMission
                    select p;
            List<chartMission> cp = new List<chartMission>();
            foreach (var item in q)
            {
                chartMission c = new chartMission();
                c.fId = item.fId;
                c.fStage = int.Parse(item.fStage);
                c.fEst_Time = int.Parse(item.fSum_Time);
                cp.Add(c);
            }
            IQueryable<IGrouping<string, string>> model = db.tMission.GroupBy(p => p.fStage,
                                                                 p=>p.fSum_Time);
            List<int> sum_EST = new List<int>();
            List<string> lab1 = new List<string>();
            List<string> lab2 = new List<string>();
            lab2.Add("0");
            foreach (var item in model)
            {
                //Console.WriteLine("----"+item.Key+"----");//debug
                lab1.Add("Stage"+item.Key);
                lab2.Add("Stage"+item.Key);
                int sum = 0;
                foreach (var item2 in item)
                {
                    sum+= int.Parse(item2);
                    Console.WriteLine(item2);
                }
                sum_EST.Add(sum);
            }
            ViewBag.lab1 = lab1;
            ViewBag.sum1 = sum_EST;
            List<int> sum_EST2 = new List<int>
            {   
                0,
                sum_EST[0],
                sum_EST[0] + sum_EST[1],
                sum_EST[0] + sum_EST[1] + sum_EST[2],
                sum_EST[0] + sum_EST[1] + sum_EST[2] + sum_EST[3]
            };
            ViewBag.lab2 = lab2;
            ViewBag.sum2 = sum_EST2;
            return View(cp);
        }

        public ActionResult chartProject_Humancost()
        {
            var q = from p in db.tProject_HumanCost
                    select p;
            List<tProject_HumanCost> cp = new List<tProject_HumanCost>();
            foreach (var item in q)
            {
                tProject_HumanCost c = new tProject_HumanCost();
                c.fId = item.fId;
                c.fProjectCode = item.fProjectCode;
                c.fLevel = item.fLevel;
                c.fSalary = item.fSalary;
                c.fNumberOfPeople = item.fNumberOfPeople;
                c.fStartDate = item.fStartDate;
                c.fEndDate = item.fEndDate;
                cp.Add(c);
            }
            //part2
            List<string> fProjectCode = new List<string>();
            foreach (var item in db.tProject_HumanCost)
            {
                if (fProjectCode.Count == 0)
                {
                    fProjectCode.Add(item.fProjectCode);
                }
                else
                {
                    if (!fProjectCode.Contains(item.fProjectCode))//職位不重複
                    {
                        fProjectCode.Add(item.fProjectCode);
                    }
                }
            }
            ViewBag.lab2 = fProjectCode;
            List<string> fLevel = new List<string>();
            foreach (var item in db.tProject_HumanCost)
            {
                if (fLevel.Count == 0)//第一筆
                {
                    fLevel.Add(item.fLevel);
                    
                }
                else
                {
                    if (!fLevel.Contains(item.fLevel))//職位不重複
                    {
                        fLevel.Add(item.fLevel);
                    }
                }
            }
            List<double> tTpa01 = new List<double>(fProjectCode.Count);
            List<double> tTpa02 = new List<double>(fProjectCode.Count);
            List<double> tTYH01 = new List<double>(fProjectCode.Count);
            List<double> tTpa03 = new List<double>(fProjectCode.Count);
            List<double> tNWT01 = new List<double>(fProjectCode.Count);
            foreach (var item in fProjectCode)
            {
                var s = from p in db.tProject_HumanCost
                        where p.fProjectCode == item
                        select p;
                switch (item)
                {
                    case "TPA01":
                        f3(tTpa01, s);
                        break;
                    case "TPA02":
                        f3(tTpa02, s);
                        break;
                    case "TYH01":
                        f3(tTYH01, s);
                        break;
                    case "TPA03":
                        f3(tTpa03, s);
                        break;
                    case "NWT01":
                        f3(tNWT01, s);
                        break;
                    default:
                        break;
                }
                
            }
            void f3(List<double> p1,IQueryable<tProject_HumanCost> s)
            {
                foreach (var item2 in fLevel)
                {
                    var s2 = from p2 in s
                             where p2.fLevel == item2
                             select p2;
                    foreach (var item3 in s2)
                    {
                        p1.Add((double)item3.fSalary * item3.fNumberOfPeople);
                    }
                }
            }
            ViewBag.fProjectCode = fProjectCode;
            ViewBag.tTpa01 = tTpa01;
            ViewBag.tTpa02 = tTpa02;
            ViewBag.tTYH01 = tTYH01;
            ViewBag.tTpa03 = tTpa03;
            ViewBag.tNWT01 = tNWT01;

            Console.WriteLine("sss");
            return View(cp);
        }   
    }
}