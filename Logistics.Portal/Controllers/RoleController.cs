﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Logistics.Domain.Entities;
using Logistics.Domain.Repository;
using Ninject;
using System.Linq;

namespace Logistics.Portal.Controllers {
    public class RoleController : BaseController {
        [Inject]
        public IRoleRep Repo { get; set; }

        public ActionResult Index() {
            return View();
        }

        public JsonResult GetGrid() {
            InitPager();
            var list = Repo.All;
            int total = list.Count();
            IEnumerable<Role> source = null;
            if (PG.asc) {
                source = list.OrderBy(GetOrderBy(PG.sort));
            } else {
                source = list.OrderByDescending(GetOrderBy(PG.sort));
            }
            return Json(new { Rows = PagedList<Role>(source), Total = total });
        }

        public JsonResult Get(int id) {
            var model = Repo.Find(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult Detail(int id) {
        //    ViewBag.Action = "Detail";
        //    ViewBag.Id = id;
        //    return View("Form");
        //}

        public ActionResult Create() {
            ViewBag.Action = "Create";
            return View("Form");
        }

        [HttpPost]
        public JsonResult Create(Role model) {
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
        public JsonResult Delete() {
            try {
                string curUser = CurrentUser.UserId;
                DateTime curtime = DateTime.Now;
                List<Role> models = JsonConvert.DeserializeObject<List<Role>>(Request["data"]);
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

        private Func<Role, object> GetOrderBy(string sort) {
            return c => {
                switch (sort) {
                    default:
                        return c.Id;
                }
            };
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (Repo != null)
                    Repo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}