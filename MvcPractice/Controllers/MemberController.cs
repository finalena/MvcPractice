using MvcPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

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
            //TODO: 檢查會員是否存在

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
        public ActionResult Register([Bind(Exclude = "RegisterOn")]Member member)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("Index");
            }

            return View();
        }

        //會員登入畫面
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Logout()
        {

            return View();
        }
    }
}