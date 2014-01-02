using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Logistics.Domain.Entities;
using Ninject;
using Logistics.Domain.Repository;
using System.Linq;

namespace Logistics.Portal.Controllers {
    public class MemorycardController : BaseController {
        [Inject]
        public IMemoryCardRep Repo { get; set; }

        public ActionResult Index() {
            return View();
        }

        public JsonResult GetGrid() {
            InitPager();
            var list = Repo.All;
            int total = list.Count();
            IEnumerable<Memorycard> source = null;
            if (PG.asc) {
                source = list.OrderBy(GetOrderBy(PG.sort));
            } else {
                source = list.OrderByDescending(GetOrderBy(PG.sort));
            }
            return Json(new { Rows = PagedList<Memorycard>(source), Total = total });
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
        public JsonResult Create(Memorycard model) {
            try {
                //SetValuesForModel(model, SubmitAction.Create);
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
                //SetValuesForModel(model, SubmitAction.Update);
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
                List<Memorycard> models = JsonConvert.DeserializeObject<List<Memorycard>>(Request["data"]);
                foreach (var model in models) {
                    //SetValuesForModel(model, SubmitAction.Delete);
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

        private Func<Memorycard, object> GetOrderBy(string sort) {
            return m => {
                switch (sort) {
                    case "Memoryid":
                        return m.Memoryid;
                    case "Memorycard":
                        return m.MemorycardName;
                    case "Scurinrpieces":
                        return m.Scurinrpieces;
                    case "ScurinWeight":
                        return m.ScurinWeight;
                    case "ScuroutWeight":
                        return m.ScuroutWeight;
                    case "Scuroutrpieces":
                        return m.Scuroutrpieces;
                    case "Sweight":
                        return m.Sweight;
                    case "Srpieces":
                        return m.Srpieces;
                    default:
                        return m.Memoryid;
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