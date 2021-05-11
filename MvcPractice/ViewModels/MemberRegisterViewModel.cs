using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcPractice.ViewModels
{
    public class MemberRegisterViewModel
    {
        [DisplayName("電子郵件")]
        [Required(ErrorMessage = "請輸入{0}")]
        [MaxLength(250, ErrorMessage = "電子郵件不可超過250個字")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [MaxLength(20, ErrorMessage = "密碼長度不可超過20個字")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("確認密碼")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "密碼與確認密碼不符!")]
        public string ConfirmPassword { get; set; }

        [DisplayName("姓名")]
        [Required(ErrorMessage = "請輸入{0}")]
        [MaxLength(10, ErrorMessage = "姓名長度不可超過10個字")]
        public string Name { get; set; }

        [DisplayName("性別")]
        [Required(ErrorMessage = "請選擇{0}")]
        public int Sex { get; set; }

        [DisplayName("出生日期")]
        [Required(ErrorMessage = "請輸入{0}")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1921/01/01", "2011/12/31")]
        public DateTime Birthday { get; set; }
    }
}