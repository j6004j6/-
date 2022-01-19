//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestFirst
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class tProject
    {
        public int fId { get; set; }
        [Display(Name = "工程名")]
        public string fName { get; set; }
        [Display(Name = "開工日期")]
        public System.DateTime fEST_StartDate { get; set; }
        [Display(Name = "完工日期")]
        public System.DateTime fEST_EndDate { get; set; }
        [Display(Name = "實際開工日期")]
        public Nullable<System.DateTime> fReal_StartDate { get; set; }
        [Display(Name = "實際完工日期")]
        public Nullable<System.DateTime> fReal_EndDate { get; set; }
        [Display(Name = "專案負責人")]
        public string fChargeMan { get; set; }
        [Display(Name = "地點")]
        public string fLocation { get; set; }
        [Display(Name = "委託人")]
        public string fBoss { get; set; }
        [Display(Name = "專案款項")]
        public Nullable<decimal> fMoney { get; set; }
        [Display(Name ="詳細資料")]
        public string fDetail { get; set; }
        [Display(Name ="工程代碼")]
        public string fCode { get; set; }
    }
}
