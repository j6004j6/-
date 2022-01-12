using System;
using System.Collections.Generic;
using System.IO;
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
        dbMyProjectEntitiesModel db = new dbMyProjectEntitiesModel();
        // 加入Cache快取，讓客戶端加載速度變快，也減少Server端的負擔 TODO...
        [OutputCache(Duration =3600,Location =System.Web.UI.OutputCacheLocation.Client)]
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
                if (now.fEmployeeId.Substring(1,1)=="5")
                {
                    var data = db.tMission_SignOff.Where(p => p.fStatus == "待主管核准").Select(p => p);
                    ViewBag.todoCount = data.Count();
                }
                else if (now.fEmployeeId.Substring(1, 1) == "7")
                {
                    var data=  db.tMission_SignOff.Where(p => p.fStatus == "完工待審").Select(p => p);
                    ViewBag.todoCount = data.Count();
                }
                else
                {
                    ViewBag.todoCount = 0;
                }
                
                return View();
            }
        }
        // AJAX_content 回傳現在的提醒字串
        public ContentResult AJAX_Home(string id)
        {
            string Notification = "無最新消息";
            if (id.Substring(0, 1) == "4")
            {
                if (id.Substring(1,1)=="7")
                {                  
                    int done = db.tMission_SignOff.Where(p => p.fStatus == "完工待審").Select(p => p).Count();
                    int deny =  db.tMission_SignOff.Where(p => p.fStatus == "已駁回").Select(p => p).Count();
                    Notification = $"有{done}個項目待您審核；有{deny}個駁回項目待您檢查";
                    return Content(Notification);
                }
                else if (id.Substring(1,1)=="5"||id.Substring(1,1)=="3")
                {
                    int done = db.tMission_SignOff.Where(p => p.fStatus == "待主管核准").Select(p => p).Count();
                    Notification = $"有{done}個項目待您審核";
                    return Content(Notification);
                }

            }
            else 
            {
            
            }
            return Content(Notification);
        }
        [HttpGet]
        public ActionResult 登入系統()
        {
            if (Session[CDictionary.SK_AUTHENTICATED_CODE] == null)
            {
                Random myrandom = new Random();
                Session[CDictionary.SK_AUTHENTICATED_CODE] = myrandom.Next(0, 10).ToString() +
                myrandom.Next(0, 10).ToString() + myrandom.Next(0, 10).ToString() + myrandom.Next(0, 10).ToString();
            }
            if (Session[CDictionary.SK_CURRENT_LOGINED_USER] != null)
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
                    if (Session[CDictionary.SK_AUTHENTICATED_CODE].ToString().Equals(data.txtCode))
                    {
                        Session[CDictionary.SK_CURRENT_LOGINED_USER] = MyUser;
                        Session["UserName"] = MyUser.fName;
                        Session["UserId"] = MyUser.fEmployeeId;
                        return RedirectToAction("首頁");
                    }
                    else
                    {
                        ViewBag.log_result = "驗證碼錯誤，請再試一次";
                        Random myrandom = new Random();
                        Session[CDictionary.SK_AUTHENTICATED_CODE] = myrandom.Next(0, 10).ToString() +
                        myrandom.Next(0, 10).ToString() + myrandom.Next(0, 10).ToString() + myrandom.Next(0, 10).ToString();
                        return View();
                    }

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
        public ActionResult 工程管理()
        {
          

            return View();
        }
        // 簽核系統
        public ActionResult goInvoice(string select_type = "",string select_search = "")
        {
            if (Session[CDictionary.SK_CURRENT_LOGINED_USER] != null)
            {
                if (Session["UserId"].ToString().Substring(0, 1) == "4"|| Session["UserId"].ToString().Substring(0, 1) == "1")
                {
                    if (select_search != "")
                    {
                        var q = from p in db.tMission_SignOff
                                where p.fMissionCode.Contains(select_search) || p.fManager.Contains(select_search) || p.fConfirmDate.ToString().Contains(select_search) || p.fChargeMan.Contains(select_search) || p.fStatus.Contains(select_search)
                                select p;
                        return View(q.ToList());
                    }
                    if (select_type == "")
                    {
                        var q = from p in db.tMission_SignOff
                                select p;
                        return View(q.ToList());
                    }
                    else
                    {
                        var q = from p in db.tMission_SignOff
                                where p.fStatus == select_type
                                select p;
                        return View(q.ToList());
                    }
                }
                else
                {
                    
                    return RedirectToAction("首頁");
                }

            }
            else
            {
                return RedirectToAction("登入系統");
            }
        }
        // 簽核系統 >>> 單項核准Action
        public ActionResult Invoice_OK(string id)
        {
            var q = from p in db.tMission_SignOff
                    where p.fMissionCode == id
                    select p;
            string username = Session["UserName"] as string;
            string c1 =(Session["UserId"] as string).Substring(0,1);
            string c2 = (Session["UserId"] as string).Substring(1, 1);
            foreach (var signoff in q)
            {
                if (c1=="4"&&c2=="7")
                {
                    signoff.fChargeMan = username;
                    signoff.fStatus = "待主管核准";
                    signoff.fConfirmDate = DateTime.Now;
                }
                else if (c1=="4"&&c2=="5")
                {
                    signoff.fManager = username;
                    signoff.fStatus = "簽核完畢";
                    signoff.fConfirmDate = DateTime.Now;
                }
                else if (c1 == "4" && c2 == "3")
                {
                    signoff.fManager = username;
                    signoff.fStatus = "簽核完畢";
                    signoff.fConfirmDate = DateTime.Now;
                }

            }
            db.SaveChanges();
            return RedirectToAction("goInvoice");
        }
        // 簽核系統 >>> 駁回Action
        public ActionResult Invoice_NO(string id,string reason)
        {
            
            var q = from p in db.tMission_SignOff
                    where p.fMissionCode == id
                    select p;
            foreach (var item in q)
            {
                item.fDetails = reason;
                item.fStatus = "已駁回";
                item.fConfirmDate = DateTime.Now;
            }
            db.SaveChanges();
            return RedirectToAction("goInvoice");
        }
        // 專案人力系統
        public ActionResult HumanResource()
        {
            if (Session[CDictionary.SK_CURRENT_LOGINED_USER] != null)
            {
                if (Session["UserId"].ToString().Substring(0, 1) == "4")
                {
                    
                    var q = db.tProject_HumanCost.Select(p => p).ToList();
                    var q2 = from p in db.tProject
                             select new myProject() { Code = p.fCode, Name = p.fName };
                    ViewBag.Project = q2.ToList();
                    return View(q);
                }
                else
	            {
                    return RedirectToAction("首頁");
                }

            }
            else
            {
                return RedirectToAction("登入系統");
            }
        }
        [HttpGet]
        public ActionResult HumanResource_alt(int pro_code)
        {
            
            tProject_HumanCost q = db.tProject_HumanCost.FirstOrDefault(p => p.fId == pro_code);
            return View(q);
        }
        // 專案人力系統 >>> 修改單項人力系統資料(POST)
        [HttpPost]
        public ActionResult HumanResource_alt(tProject_HumanCost humancost)
        {
           
            tProject_HumanCost q = db.tProject_HumanCost.FirstOrDefault(p => p.fId == humancost.fId);
            q.fEndDate = humancost.fEndDate;
            q.fLevel = humancost.fLevel;
            q.fNumberOfPeople = humancost.fNumberOfPeople;
            q.fProjectCode = humancost.fProjectCode;
            q.fSalary = humancost.fSalary;
            q.fStartDate = humancost.fStartDate;
            db.SaveChanges();
            return RedirectToAction("HumanResource_alt");
        }
        // 專案人力系統 >>> 刪除單項人力系統資料
        public ActionResult HumanResource_del(int pro_code)
        {
           
            tProject_HumanCost q = db.tProject_HumanCost.FirstOrDefault(p => p.fId == pro_code);
            db.tProject_HumanCost.Remove(q);
            db.SaveChanges();
            return RedirectToAction("HumanResource");
        }
        // 專案人力系統 >>> 新增單項人力系統資料
        public ActionResult HumanResource_new()
        {
            return View();
        }
        [HttpPost]
        public ActionResult HumanResource_new(tProject_HumanCost human)
        {
           
            db.tProject_HumanCost.Add(human);
            db.SaveChanges();
            return RedirectToAction("HumanResource");
        }
        // 標單與合約
        [OutputCache(Duration =3600,Location =System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult BiddingandContract(string select_search="")
        {
            if (Session[CDictionary.SK_CURRENT_LOGINED_USER] != null)
            {
                if (Session["UserId"].ToString().Substring(0, 1) == "4")
                {
                    if (select_search != "")
                    {
                        var q = db.tBidding.Where(p => p.fName.Contains(select_search) ||
                                                    p.fCategory.Contains(select_search) ||
                                                    p.fChargeMan.Contains(select_search) ||
                                                    p.fBidder.Contains(select_search) ||
                                                    p.fDate.ToString().Contains(select_search)
                                                  ).Select(p => p);
                        ViewBag.BiddingCategory = q.Select(p => p.fCategory).Distinct();
                        return View(q);
                    }
                    else
                    {
                        var q = db.tBidding.Select(p => p);
                        ViewBag.BiddingCategory = q.Select(p => p.fCategory).Distinct();
                        return View(q);
                    }
                }
                else
                {
                    return RedirectToAction("首頁");
                }
                    
            
            }
            else
            {
                return RedirectToAction("登入系統");
            }
            
        }
        // 標單與合約 >>> 單項修改
        public ActionResult Bidding_Edit(int id)
        {
            tBidding q = db.tBidding.FirstOrDefault(p => p.fId == id);
            return View(q);
        }
        [HttpPost]
        public ActionResult Bidding_Edit(tBidding bid)
        {
            tBidding q = db.tBidding.FirstOrDefault(p => p.fId == bid.fId);
            q.fAmount = bid.fAmount;
            q.fBidder = bid.fBidder;
            q.fCategory = bid.fCategory;
            q.fChargeMan = bid.fChargeMan;
            q.fContract = bid.fContract;
            q.fDate = bid.fDate;
            q.fName = bid.fName;
            q.fUnit = bid.fUnit;
            q.fUnitPrice = bid.fUnitPrice;
            db.SaveChanges();
            return RedirectToAction("BiddingandContract");
        }
        // 標單與合約 >>> 單項刪除
        public ActionResult Bidding_Delete(int id)
        {
            tBidding q = db.tBidding.FirstOrDefault(p => p.fId == id);
            db.tBidding.Remove(q);
            db.SaveChanges();
            return RedirectToAction("BiddingandContract");
        }
        // 標單與合約 >>> 新增
        public ActionResult Bidding_new()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Bidding_new(tBidding tbid)
        {
            db.tBidding.Add(tbid);
            db.SaveChanges();
            return RedirectToAction("BiddingandContract");
        }
        // 照片功能
        public ActionResult Photos()
        {
            var q = db.tMissionDetail_Photos.Select(p => p);
            ViewBag.Project = db.tProject.Select(p => p).ToList();
            return View(q.ToList());
        }
        public ActionResult Photos_new( )
        {
            ViewBag.Project = db.tProject.Select(p => p).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Photos_new(tMissionDetail_Photos_model data)
        {

            //存到資料夾
            var FileName = Path.GetFileName(data.fPhoto.FileName);
            var FilePath = Path.Combine(Server.MapPath("~/Images/"), FileName);
            data.fPhoto.SaveAs(FilePath);
            //轉成byte 直接轉
            byte[] FileBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                data.fPhoto.InputStream.CopyTo(ms);
                FileBytes = ms.GetBuffer();
            }
            var q = new tMissionDetail_Photos();
            q.fDate = data.fDate;
            q.fDescription = data.fDescription;
            q.fName = data.fName;
            q.fPhoto = FileBytes;
            q.fCategory = data.fCategory;
            db.tMissionDetail_Photos.Add(q);
            db.SaveChanges();
            return RedirectToAction("Photos");
        }
        
        public ActionResult 資料分析()
        {
            return View();
        }
        // QandA機器人
        public ActionResult QandA()
        {//TODO...新增專案相關問題
            return View();
        }
        // 公告與留言區
        public ActionResult 留言板()
        {
            if (Session["UserId"]==null)
            {
                return RedirectToAction("登入系統");
            }
            else
            {
                string s = (Session["UserId"]as string).Substring(0,1);
                var m = from mb in db.tMessagBoard
                        where mb.fdepartment == 10 || mb.fdepartment.ToString() == s
                        orderby mb.fid descending
                        select mb;
                var q = from p in db.tUser
                        select p.fName;
                List<string> Namelist = q.ToList();
                ViewBag.NameList = Namelist;
                ViewBag.NameList1 = db.tUser.Where(p => p.fEmployeeId.Substring(0, 1) == "1").Select(p => p.fName).ToList();
                ViewBag.NameList2 = db.tUser.Where(p => p.fEmployeeId.Substring(0, 1) == "2").Select(p => p.fName).ToList();
                ViewBag.NameList3 = db.tUser.Where(p => p.fEmployeeId.Substring(0, 1) == "3").Select(p => p.fName).ToList();
                ViewBag.NameList4 = db.tUser.Where(p => p.fEmployeeId.Substring(0, 1) == "4").Select(p => p.fName).ToList();
                
               
                return View(m.ToList());
            }
            
        }
        // AJAX回傳此訊息是否有被登入之人讀過
        public ActionResult CheckRead(string messid)
        {
            string userid = Session["UserId"].ToString();
            List<string> q = db.tMessageConfirm.Where(p => p.fuser_id.ToString() == userid).Select(p => p.fmssage_id.ToString()).ToList();
            bool ans = q.Contains(messid);
            return Content(ans.ToString());
        }
        // AJAX_把已讀寫進資料庫
        public ActionResult AddRead(string messageid, string userid)
        {
            try
            {
                tMessageConfirm con = new tMessageConfirm();
                con.fmssage_id = Convert.ToInt32(messageid.Substring(3));
                con.fuser_id = Convert.ToInt32(userid);
                db.tMessageConfirm.Add(con);
                db.SaveChanges();
                return Content("已讀完畢");
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
           
            
        }
        // AJAX讀取發公告者的名字
        public ActionResult AJAX_PostMan(string id)
        {
            string[] empid = id.Split(',');
            List<string> name = new List<string>() { };
            for (int i = 0; i < empid.Length; i++)
            {
                string x = empid[i];

                var q = from p in db.tUser
                           where p.fEmployeeId == x
                           select p.fName;
                name.Add(q.FirstOrDefault());
            }
            return Json(name,JsonRequestBehavior.AllowGet);
        }
        // AJAX取得已讀特定公告的名單
        public ActionResult getList(int id)
        {
            List<string> list = new List<string>() { };
            List<int?> read = db.tMessageConfirm.Where(p => p.fmssage_id == id).Select(p =>  p.fuser_id ).ToList();
            if (read.Count!=0)
            {
                for (int i = 0; i < read.Count; i++)
                {
                    var user = from p in db.tUser.AsEnumerable()
                               where p.fEmployeeId == read[i].ToString()
                               select p.fName;
                    list.Add(user.ToList()[0]);
                }
            }
            else
            {
                
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        } 
        // 公告與留言區>>>新增公告
        public ActionResult SaveMessage1()
        {
            tMessagBoard mb = new tMessagBoard();
            return View(mb);
        }
        [HttpPost]
        public ActionResult SaveMessage1(tMessagBoard c)
        {
            tMessagBoard t = new tMessagBoard();
            t.ftitle = c.ftitle;
            t.fdatetime = DateTime.Now;
            t.fmessage = c.fmessage;
            t.fdepartment = c.fdepartment;
            int userid = Convert.ToInt32 (Session["UserId"]);
            t.fuser_id = userid;
            db.tMessagBoard.Add(t);
            db.SaveChanges();
            return RedirectToAction("留言板");
        }
        // 公告與留言區>>>刪除公告
        public ActionResult DeletMessage(int fid)
        {
            var t = db.tMessagBoard.FirstOrDefault(m => m.fid == fid);
            db.tMessagBoard.Remove(t);
            var c = from p in db.tMessageConfirm
                    where p.fmssage_id == fid
                    select p;
            foreach (var item in c)
            {
                db.tMessageConfirm.Remove(item);
            }

            db.SaveChanges();
            return RedirectToAction("留言板");
        }

        public ActionResult EditMessage(int fid)
        {
            tMessagBoard t = db.tMessagBoard.FirstOrDefault(m => m.fid == fid);
            if (t == null)
            {
                return RedirectToAction("留言板");
            }
            return View(t);
        }
        [HttpPost]
        public ActionResult EditMessage(tMessagBoard t)
        {
            tMessagBoard message = db.tMessagBoard.FirstOrDefault(p => p.fid == t.fid);
            message.fmessage = t.fmessage;
            message.fdatetime = DateTime.Now;
            message.ftitle = t.ftitle;
            db.SaveChanges();
            return RedirectToAction("留言板");
        }

        public ActionResult AdvanceQueryMessage1(QueryMessage m)
        {
            var list = db.tMessagBoard.Select(p => p);
            if (m.textmessage != null)
            {
                list = list.Where(p => p.fmessage.Contains(m.textmessage));
            }
            if (m.textuserid != null)
            {
                list = list.Where(p => p.fuser_id == Convert.ToInt32(m.textuserid));
            }
            if (m.textyear1 != null && m.textyear2 != null)
            {
                int str1 = Convert.ToInt32(m.textyear1);
                int str2 = Convert.ToInt32(m.textyear2);
                list = list.Where(p => p.fdatetime.Year >= str1 && p.fdatetime.Year <= str2);
            }
            if (m.textmonth1 != null && m.textmonth2 != null)
            {
                int str3 = Convert.ToInt32(m.textmonth1);
                int str4 = Convert.ToInt32(m.textmonth2);
                list = list.Where(p => p.fdatetime.Month >= str3 && p.fdatetime.Month <= str4);
            }

            return View(list);
        }
        public ActionResult Confirm(int fid)
        {
            tMessageConfirm m = new tMessageConfirm();
            m.fmssage_id = fid;
            CUser user = (CUser)Session[CDictionary.SK_CURRENT_LOGINED_USER];
            m.fuser_id = Convert.ToInt32(user.fId);
            db.tMessageConfirm.Add(m);
            db.SaveChanges();
            return RedirectToAction("留言板");
        }
        public ActionResult Chat()
        {
            return View();
        }
        public ActionResult 信箱()
        {
            CUser u = (CUser)Session[CDictionary.SK_CURRENT_LOGINED_USER];
            var mail = from m in db.tmail
                       where m.fsendid.ToString() == u.fId || m.frecieveid.ToString() == u.fId
                       orderby m.fid descending
                       select m;
            int count = 0;
            var mail1 = from tm in db.tmail
                        where tm.frecieveid.ToString() == u.fId
                        select tm;
            foreach (var item in mail1)
            {
                if (!item.fisreaded)
                {
                    count++;

                }
            }
            ViewBag.mailcount = count;
            return View(mail);
        }
        public ActionResult SendMail()
        {
            tmail t = new tmail();
            return View(t);
        }
        [HttpPost]
        public ActionResult SendMail(tmail tmail)
        {
            tmail t = new tmail();
            t.ftitle = tmail.ftitle;
            t.fmessage = tmail.fmessage;
            t.frecieveid = tmail.frecieveid;
            t.fdate = DateTime.Now;
            CUser u = (CUser)Session[CDictionary.SK_CURRENT_LOGINED_USER];
            t.fsendid = int.Parse(u.fId);
            db.tmail.Add(t);
            db.SaveChanges();
            return View();
        }
        public ActionResult DeletMail(int fid)
        {
            var m = db.tmail.FirstOrDefault(t => t.fid == fid);
            CUser u = (CUser)Session[CDictionary.SK_CURRENT_LOGINED_USER];
            if (m.frecieveid.ToString() == u.fId)
            {
                m.freceievedelete = true;
            }
            if (m.fsendid.ToString() == u.fId)
            {
                m.fsenddelete = true;
            }
            db.SaveChanges();
            return RedirectToAction("信箱");
        }

        public ActionResult OpenMail(int fid)
        {
            CUser u = (CUser)Session[CDictionary.SK_CURRENT_LOGINED_USER];
            var m = db.tmail.FirstOrDefault(t => t.fid == fid);
            if (m.isreplied != true && m.frecieveid.ToString() == u.fId)
            {
                m.fisreaded = true;
                db.SaveChanges();
                int count = 0;
                var mail = from t in db.tmail
                           where t.frecieveid.ToString() == u.fId
                           select t;
                foreach (var item in mail)
                {
                    if (!item.fisreaded)
                    {
                        count++;

                    }
                }
                ViewBag.mailcount = count;
            }


            return View(m);
        }

        public ActionResult ReplyMail(int fid)
        {

            var m = db.tmail.FirstOrDefault(t => t.fid == fid);
            m.fisreaded = true;
            db.SaveChanges();
            return View(m);
        }
        [HttpPost]
        public ActionResult ReplyMail(tmail tmail)
        {
            tmail t = new tmail();
            // int fid = tmail.fid;
            tmail t1 = db.tmail.FirstOrDefault(m => m.fid == tmail.fid);
            t.ftitle = "r:" + t1.ftitle;
            t.fsendid = t1.frecieveid;
            t.frecieveid = t1.fsendid;
            t.fmessage = tmail.fmessage;
            t.fdate = DateTime.Now;
            CUser u = (CUser)Session[CDictionary.SK_CURRENT_LOGINED_USER];
            t.fsendid = int.Parse(u.fId);
            db.tmail.Add(t);
            db.SaveChanges();
            return RedirectToAction("信箱");
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
