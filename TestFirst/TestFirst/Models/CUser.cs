using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TestFirst.Models
{
    public class CUser
    {
        public string fId { get; set; }
        [DisplayName("帳號")]
        public string fAccount { get; set; }
        [DisplayName("密碼")]
        public string fPassword { get; set; }
        [DisplayName("使用者名稱")]
        public string fName { get; set; }
        //public string fDepartment { get; set; }
        //public string fAuthorization { get; set; }
        [DisplayName("在職狀態")]
        public string fStatus { get; set; }
        [DisplayName("到職日")]
        public string fArrivingDate { get; set; }
        [DisplayName("離職日")]
        public string fLeavingDate { get; set; }

        public string fEmail { get; set; }
        [DisplayName("電話")]
        public string fPhone { get; set; }
        [DisplayName("地址")]
        public string fAddress { get; set; }
        [DisplayName("員工編號")]
        public string fEmployeeId { get; set; }
    }
}