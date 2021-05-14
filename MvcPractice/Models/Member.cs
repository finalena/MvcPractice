using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;
using System.Web;

namespace MvcPractice.Models
{
    [DisplayName("會員訊息")]
    public class Member
    {
        public int Id { get; set; }

        [DisplayName("電子郵件")]
        [Required(ErrorMessage = "請輸入電子郵件")]
        [MaxLength(250, ErrorMessage = "電子郵件不可超過250個字")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        [MaxLength(20, ErrorMessage = "密碼長度不可超過20個字")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [DisplayName("確認密碼")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "密碼與確認密碼不符!")]
        public string ConfirmPassword { get; set; }

        [DisplayName("姓名")]
        [Required(ErrorMessage = "請輸入姓名")]
        [MaxLength(10, ErrorMessage = "姓名長度不可超過10個字")]
        public string Name { get; set; }
       
        [DisplayName("暱稱")]
        [MaxLength(20, ErrorMessage = "暱稱長度不可超過20個字")]
        public string NickName { get; set; }

        [DisplayName("性別")]
        [Required(ErrorMessage = "請選擇性別")]
        public string Sex { get; set; }
        public IEnumerable<SelectListItem> SexItems { get; set; }

        [DisplayName("出生日期")]
        [Required(ErrorMessage = "請輸入出生日期")]
        [DataType(DataType.Date)]
        //TODO: [Range(typeof(DateTime), "1921/01/01", "2011/12/31")]
        public DateTime Birthday { get; set; }

        [DisplayName("電話號碼")]
        [MaxLength(25, ErrorMessage = "電話號碼長度不可超過25字")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DisplayName("所在地")]
        [MaxLength(5, ErrorMessage = "所在地長度不可超過5字")]
        public string Location { get; set; }

        [DisplayName("個人簡介")]
        [MaxLength(200, ErrorMessage = "個人簡介長度不可超過200個字")]
        [DataType(DataType.MultilineText)]
        public string Profile { get; set; }

        [DisplayName("註冊時間")]
        public DateTime RegisterOn { get; set; }

        [DisplayName("會員啟用認證碼")]
        [MaxLength(36)]
        [Description("當AutoCode = null代表會員通過Email驗證")]
        public string AutoCode { get; set; }
    }
}