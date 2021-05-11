using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcPractice.ViewModels
{
    public class MemberUpdatePasswordViewModel
    {
        [DisplayName("舊密碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("新密碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [MaxLength(20, ErrorMessage = "密碼長度不可超過20個字")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DisplayName("確認新密碼")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "密碼與確認新密碼不符")]
        public string ConfirmNewPassword { get; set; }
    }
}