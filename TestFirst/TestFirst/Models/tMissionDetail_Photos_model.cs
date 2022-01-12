using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestFirst.Models
{
    public class tMissionDetail_Photos_model
    {

        public int fId { get; set; }
        [Display(Name = "工程名稱")]
        public string fName { get; set; }
        [Display(Name = "工程照片")]
        public HttpPostedFileBase fPhoto { get; set; }
        [Display(Name = "上傳日期")]
        public System.DateTime fDate { get; set; }
        [Display(Name = "描述")]
        public string fDescription { get; set; }
        [Display(Name = "分類")]
        public string fCategory { get; set; }

    }
}