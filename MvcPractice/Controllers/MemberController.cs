using MvcPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Data.Entity.Validation;

namespace MvcPractice.Controllers
{
    public class MemberController : Controller
    {
        MvcPracticeContext db = new MvcPracticeContext();
     
        //顯示會員資料
        public ActionResult Index()
        {
            var chk_member = db.Members.Where(p => p.Email == User.Identity.Name).FirstOrDefault();
            if (chk_member == null) return RedirectToAction("Login", "Member");

            return View(chk_member);
        }
        
        //更新會員基本資料
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            var member = db.Members.Where(p => p.Email == User.Identity.Name).FirstOrDefault();
            if (member == null) return RedirectToAction("Login", "Member");
           
            if (ModelState.IsValid && TryUpdateModel<Member>(member,"", form.AllKeys, new string[] {
                    "Email", "Password", "RegisterOn", "AuthCode"}))
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(member);
            }
        }
        
        //顯示會員更新密碼頁面
        public ActionResult UpdatePassword()
        {
           
            return View();
        }

        //更新會員密碼
        [HttpPost]
        public ActionResult UpdatePassword(ViewModels.MemberUpdatePasswordViewModel UpdatePasswordVM)
        {
            var member = db.Members.Where(p => p.Email == User.Identity.Name).FirstOrDefault();
            if (member == null) return RedirectToAction("Login", "Member");

            if (!ValidatePassword(UpdatePasswordVM.Password, member.Password)) ModelState.AddModelError("Password", "舊密碼輸入錯誤");

            member.Password = CreateHash(UpdatePasswordVM.NewPassword);
            if (ModelState.IsValid && TryUpdateModel<Member>(member, new string[] { "NewPassword" }))
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Member");
            }
            else
            {
                return View(UpdatePasswordVM);
            }
            
        }

        //會員註冊畫面
        public ActionResult Register()
        {
            return View();
        }

        //寫入會員訊息
        [HttpPost]
        public ActionResult Register(ViewModels.MemberRegisterViewModel registerVM)
        {
            //檢查Email是否已經註冊過
            var chk_member = db.Members.Where(m => m.Email == registerVM.Email).FirstOrDefault();
            if (chk_member != null)
            {
                ModelState.AddModelError("Email", "您輸入的Email已經有人註冊過了!");
            }
            
            if (ModelState.IsValid)
            {
                Member member = new Member
                {
                    Email = registerVM.Email, 
                    Password = CreateHash(registerVM.Password),
                    Name = registerVM.Name,
                    Gender = registerVM.Gender,
                    Birthday = registerVM.Birthday,
                    RegisterOn = DateTime.Now,
                    AuthCode = Guid.NewGuid().ToString()
                };
             
                db.Members.Add(member);
                db.SaveChanges();
              
                SendAuthCodeToMember(member);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        private void SendAuthCodeToMember(Models.Member member)
        {
            string mailBody = System.IO.File.ReadAllText(Server.MapPath("~/App_Data/MemberRefisterEMailTemplate.htm"));
            mailBody = mailBody.Replace("{{Name}}", member.Name);
            mailBody = mailBody.Replace("{{RegisterOn}}", member.RegisterOn.ToString("F"));

            var auth_url = new UriBuilder(Request.Url)
            {
                Path = Url.Action("ValidateRegister", new { id = member.AuthCode }),
                Query = ""
            };

            mailBody = mailBody.Replace("{{AUTH_URL}}", auth_url.ToString());

            try
            {
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.Port = 587;
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = true;
                SmtpServer.Credentials = new System.Net.NetworkCredential("islenaliny@gmail.com", "ldbsyqswabpzvspj");

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("islenaliny@gmail.com");
                mail.To.Add(member.Email);
                mail.Subject = "\"ASP.NET 應用程式-商務網站\"會員註冊確認信";
                mail.Body = mailBody;
                mail.IsBodyHtml = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
                //郵件寄送失敗，DB要存LOG，以免有會員無法登陸
            }
        }

        [ValidateInput(false)]
        public ActionResult ValidateRegister(string id)
        {
            if (string.IsNullOrEmpty(id)) return HttpNotFound();

            var member = db.Members.Where(p => p.AuthCode == id).FirstOrDefault();
            if (member == null)
            {
                TempData["LastTempMsg"] = "查無此會員驗證碼，您可能已經驗證過了!";
            }
            else
            {
                TempData["LastTempMsg"] = "會員驗證成功，您現在可以登陸網站了!";
                //驗證成功將清空AuthCode
                member.AuthCode = null;
                db.SaveChanges();
            }
            return RedirectToAction("Login", "Member");
        }

        //會員登入畫面
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnBag = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(ViewModels.MemberLoginViewModel LoginVM, string returnUrl)
        {
            if (ValidateUser(LoginVM.Email, LoginVM.Password))
            {
                //使用表單驗證
                FormsAuthentication.SetAuthCookie(LoginVM.Email, false);

                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }
            
            return View();
        }

        private bool ValidateUser(string email, string password)
        {
            var member = db.Members.Where(m => m.Email == email).FirstOrDefault();
            if (member == null)
            {
                ModelState.AddModelError("Email", "無此帳號，請先註冊會員");
                return false;
            }
            else if (!ValidatePassword(password, member.Password))
            {
                ModelState.AddModelError("Password", "您輸入的密碼錯誤");
                return false;
            }
            else if (member.AuthCode != null)
            {
                ModelState.AddModelError("", "您尚未通過會員驗證，請先收信定點擊會員驗證連結");
                return false;
            }
           
            return true;
        }

        private const int SALT_BYTE_SIZE = 24;
        private const int HASH_BYTE_SIZE = 24;
        private string CreateHash(string password)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_BYTE_SIZE];
            //產生鹽
            rng.GetBytes(salt);
            byte[] hash = PBKDF2(password, salt);
            //string cryptographyPassword =
            //  FormsAuthentication.HashPasswordForStoringInConfigFile(password + salt, "sha1");

            return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

        private bool ValidatePassword(string password, string correctHash)
        {
            char[] delimiter = { ':' };
            string[] split = correctHash.Split(delimiter);
            byte[] salt = Convert.FromBase64String(split[0]);
            byte[] hash = Convert.FromBase64String(split[1]);

            byte[] testHash = PBKDF2(password, salt);

            return testHash.SequenceEqual<byte>(hash);
        }

        /// <summary>
        /// 密碼加密加鹽
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private byte[] PBKDF2(string password, byte[] salt)
        {
            Rfc2898DeriveBytes rfcKey = new Rfc2898DeriveBytes(password, salt);
            rfcKey.IterationCount = 2;
            byte[] hash = rfcKey.GetBytes(HASH_BYTE_SIZE);

            return hash;
        }

        public ActionResult Logout()
        {
            //清除表單驗證Cookie
            FormsAuthentication.SignOut();
            Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}