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

    public partial class tMissionDetail_Photos
    {
        public int fId { get; set; }
        [Display(Name ="工程名")]
        public string fName { get; set; }
        [Display(Name ="照片")]
        public byte[] fPhoto { get; set; }
        [Display(Name ="日期")]
        public System.DateTime fDate { get; set; }
        [Display(Name ="內容")]
        public string fDescription { get; set; }
        [Display(Name ="專案")]
        public string fCategory { get; set; }
    }
}
