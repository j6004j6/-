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

    public partial class tMission_SignOff
    {
        public int fId { get; set; }
        [Display(Name ="工程代碼")]
        public string fMissionCode { get; set; }
        [Display(Name ="合約")]
        public string fContract { get; set; }
        [Display(Name ="工程款")]
        public Nullable<decimal> fMoney { get; set; }
        [Display(Name ="經辦")]
        public string fChargeMan { get; set; }
        [Display(Name ="主管")]
        public string fManager { get; set; }
        [Display(Name ="狀態")]
        public string fStatus { get; set; }
        [Display(Name ="詳細資料")]
        public string fDetails { get; set; }
        [Display(Name ="核准日")]
        public Nullable<System.DateTime> fConfirmDate { get; set; }
    }
}
