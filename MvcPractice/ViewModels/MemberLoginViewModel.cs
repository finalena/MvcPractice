using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcPractice.ViewModels
{
    public class MemberLoginViewModel
    {
        [DisplayName("會員帳號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [DataType(DataType.EmailAddress, ErrorMessage = "請輸入您的Email地址")]
        public string Email { get; set; }

        [DisplayName("會員密碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


    }
}