using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Logistics.Domain.Entities;
using Logistics.Domain.Repository;
using Ninject;
using Logistics.Infrastructure;

namespace Logistics.Portal.Controllers {
    public class LoginUserController : BaseController {

        [Inject]
        public ILoginUserRep Service { get; set; }

        public ActionResult Index() {
            return View();
        }

        public JsonResult GetGrid() {
            InitPager();
            var list = Service.GetUserView();
            int total = list.Count();
            IEnumerable<dynamic> source = null;
            if (PG.asc) {
                source = list.OrderBy(GetOrderBy(PG.sort));
            } else {
                source = list.OrderByDescending(GetOrderBy(PG.sort));
            }
            var tmp = source.ToList();
            return Json(new { Rows = PagedList<dynamic>(tmp), Total = total });
        }

        public JsonResult Get(int id) {
            var model = Service.Find(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(int id) {
            ViewBag.Action = "Detail";
            ViewBag.Id = id;
            return View("Form");
        }

        public ActionResult Create() {
            ViewBag.Action = "Create";
            return View("Form");
        }

        [HttpPost]
        public JsonResult Create(LoginUser model) {
            try {
                //SetValuesForModel(model, SubmitAction.Create);
                model.Status = "A";
                model.Modifytime = DateTime.Now;
                model.Modifyuser = CurrentUser.UserId;
                Service.Add(model);
                Service.SaveChanges();
                return Json(true);
            } catch { }
            return Json(false);
        }

        public ActionResult Update(int id) {
            ViewBag.Action = "Update";
            ViewBag.Id = id;
            return View("Form");
        }

        [HttpPost]
        public JsonResult Update(int id, FormCollection forms) {
            var model = Service.Find(id);
            if (TryUpdateModel(model)) {
                //SetValuesForModel(model, SubmitAction.Update);
                model.Modifytime = DateTime.Now;
                model.Modifyuser = CurrentUser.UserId;
                Service.Update(model);
                Service.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult Delete() {
            try {
                string curUser = CurrentUser.UserId;
                DateTime curtime = DateTime.Now;
                List<LoginUser> models = JsonConvert.DeserializeObject<List<LoginUser>>(Request["data"]);
                foreach (var model in models) {
                    //SetValuesForModel(model, SubmitAction.Delete);
                    model.Status = "D";
                    model.Modifytime = DateTime.Now;
                    model.Modifyuser = CurrentUser.UserId;
                    Service.Update(model);
                }
                Service.SaveChanges();
                return Json(true);
            } catch { }
            return Json(false);
        }

        [HandleError(ExceptionType = typeof(FileNotFoundException), View = "NotExists.html")]
        public FileResult Export() {
            DataTable dt = ListToDt<LoginUser>(Service.All.ToList(), p => {
                string[] columns = new string[] { "LoginUserName", "ShortName", "MnCode", "Address", "Phones", "Contract" };
                return columns.Contains(p.Name);
            });
            byte[] fileContents = ExcelUti.ReadFileContents(dt);
            return File(fileContents, "application/vnd.ms-excel", "客户信息列表.xls");
        }

        private Func<dynamic, object> GetOrderBy(string sort) {
            return u => {
                switch (sort) {
                    default:
                        return u.UserId;
                }
            };
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                Service.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}