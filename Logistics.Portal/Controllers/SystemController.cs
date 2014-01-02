using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Logistics.Domain.Entities;
using Logistics.Domain.Repository;
using Logistics.Portal.Filters;
using Logistics.Portal.Models;
using Logistics.Web.Infrastructure;
using Ninject;

namespace Logistics.Portal.Controllers {
    public class SystemController : Controller {
        [Inject]
        public ISystemRep SysRep { get; set; }

        public UserInfo CurrentUser {
            get { return Session["CurrentUserInfo"] as UserInfo; }
            set { Session["CurrentUserInfo"] = value; }
        }

        public ActionResult Index() {
            if (CurrentUser == null) {
                return RedirectToAction("Login");
            }
            return View();
        }

        [Permission]
        public JsonResult GetButtons() {
            string menuNo = Request["MenuNo"];
            var data = SysRep.GetButtons(CurrentUser.RoleId, menuNo).ToList();
            return Json(data.Select(d => new { id = d.SysController, text = d.About, icon = d.Icon }));
        }

        [Permission]
        public JsonResult GetCurrentUser() {
            return Json(CurrentUser, JsonRequestBehavior.AllowGet);
        }

        [Permission]
        public ActionResult ChangePassword() {
            return View();
        }

        [Permission]
        [HttpPost]
        public JsonResult ChangePassword(FormCollection forms) {
            string OldPassword = forms["OldPassword"].ToString();
            string NewPassword = forms["NewPassword"].ToString();
            string ConfirmPassword = forms["ConfirmPassword"].ToString();
            if (NewPassword == ConfirmPassword) {
                LoginUser user = SysRep.GetUser(CurrentUser.UserId, OldPassword);
                if (user != null) {
                    user.UserPassword = NewPassword;
                    SysRep.SaveChanges();
                    return Json(true);
                }
            }
            return Json(false);
        }

        public ActionResult Login() {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection forms) {
            string UserName = forms["UserName"].ToString();
            string Password = forms["Password"].ToString();
            string CheckCode = forms["CheckCode"].ToString();

            if (!string.Equals(CheckCode, Session["CheckCode"])) {
                ModelState.AddModelError("", "验证码不正确");
            } else {
                LoginUser user = SysRep.GetUser(UserName, Password);
                if (user == null) {
                    ModelState.AddModelError("", "用户名或密码不正确");
                } else {
                    CurrentUser = SetCurrentUserInfo(user);
                    //FormsAuthentication.SetAuthCookie(UserName, false);
                    //FormsAuthentication.RedirectFromLoginPage(UserName, false);
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        private UserInfo SetCurrentUserInfo(LoginUser user) {
            ////return SysRep.SetCurrentUserInfo(user);
            //throw new NotImplementedException();
            return new UserInfo { UserId = user.UserId, RoleId = user.RoleId };
        }

        public void LogOff() {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            FormsAuthentication.RedirectToLoginPage();
        }

        public ActionResult GetImgVerifyChars() {
            string rndNum = Rnd.GetRandNum(4);
            Session["CheckCode"] = rndNum;

            Bitmap img = rndNum.ToImg();
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Png);
            img.Dispose();
            return File(ms.ToArray(), @"image/jpeg");
        }
    }
}