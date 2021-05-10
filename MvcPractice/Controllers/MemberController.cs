using MvcPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.Security;

namespace MvcPractice.Controllers
{
    public class MemberController : Controller
    {
        MvcPracticeContext db = new MvcPracticeContext();

        //顯示會員資料
        public ActionResult Index(Member member)
        {
            return View();
        }

        //更新會員基本資料
        [HttpPost]
        public ActionResult Update([Bind(Exclude = "Password,RegisterOn")] Member member)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();

            }

            return View();
        }

        //更新會員密碼
        [HttpPost]
        public ActionResult UpdatePassword(string email, string password)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("Index");
            }

            return View();
        }

        //會員註冊畫面
        public ActionResult Register()
        {

            return View();
        }

        //寫入會員訊息
        [HttpPost]
        public ActionResult Register([Bind(Exclude = "RegisterOn,AutoCode")]Member member)
        {
            //檢查Email是否已經註冊過
            var chk_member = db.Members.Where(m => m.Email == member.Email).FirstOrDefault();
            if (chk_member != null)
            {
                ModelState.AddModelError("Email", "您輸入的Email已經有人註冊過了!");
            }
            
            if (ModelState.IsValid)
            {
                //TODO: 密碼加密
                member.Password = member.Password;
                member.RegisterOn = DateTime.Now;
                //TODO: 產生會員認證碼&寄送認證信
                member.AutoCode = "";

                db.Members.Add(member);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        //會員登入畫面
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnBag = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password, string returnUrl)
        {
            var member = db.Members.Where(m => m.Email == email && m.Password == password).FirstOrDefault();

            if (member != null)
            {
                if (member.AutoCode == null)
                { 
                    //使用表單驗證
                    FormsAuthentication.SetAuthCookie(email, false);

                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "您尚未通過會員驗證，請先收信定點擊會員驗證連結");
                }
            }
            else
            {
                ModelState.AddModelError("", "您輸入的帳號密碼錯誤");
            }

            return View();
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