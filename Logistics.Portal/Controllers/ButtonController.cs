using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Logistics.Domain.Entities;
using Logistics.Domain.Repository;
using Newtonsoft.Json;
using Ninject;
using System.Linq;
using Logistics.Portal.Models;

namespace Logistics.Portal.Controllers {
    public class ButtonController : BaseController {
        [Inject]
        public IButtonRep Repo { get; set; }

        public ActionResult Index() {
            return View();
        }

        public JsonResult GetGrid() {
            InitPager();
            var list = Repo.All;
            int total = list.Count();
            IEnumerable<Button> source = null;
            if (PG.asc) {
                source = list.OrderBy(GetOrderBy(PG.sort));
            } else {
                source = list.OrderByDescending(GetOrderBy(PG.sort));
            }
            return Json(new { Rows = PagedList<Button>(source), Total = total });
        }

        public JsonResult Get(int id) {
            var model = Repo.Find(id);
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
        public JsonResult Create(Button model) {
            try {
                model.Status = "A";
                model.Modifytime = DateTime.Now;
                model.Modifyuser = CurrentUser.UserId;
                Repo.Add(model);
                Repo.SaveChanges();
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
            var model = Repo.Find(id);
            if (TryUpdateModel(model)) {
                model.Modifytime = DateTime.Now;
                model.Modifyuser = CurrentUser.UserId;
                Repo.Update(model);
                Repo.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult Delete(UserInfo user) {
            try {
                string curUser = CurrentUser.UserId;
                DateTime curtime = DateTime.Now;
                List<Button> models = JsonConvert.DeserializeObject<List<Button>>(Request["data"]);
                foreach (var model in models) {
                    model.Status = "D";
                    model.Modifytime = DateTime.Now;
                    model.Modifyuser = CurrentUser.UserId;
                    Repo.Update(model);
                }
                Repo.SaveChanges();
                return Json(true);
            } catch { }
            return Json(false);
        }

        private Func<Button, object> GetOrderBy(string sort) {
            return (Button p) => {
                switch (sort) {
                    case "Id":
                        return p.Id;
                    default:
                        return p.Id;
                }
            };
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (Repo != null)
                    Repo.Dispose();
            }
        }
    }
}