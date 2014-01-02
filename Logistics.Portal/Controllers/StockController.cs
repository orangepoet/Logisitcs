using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Logistics.Domain.Entities;
using Logistics.Domain.Repository;
using Newtonsoft.Json;
using Ninject;

namespace Logistics.Portal.Controllers {
    public class StockController : BaseController {
        [Inject]
        public IStockRep Repo { get; set; }

        public ActionResult Index() {
            return View();
        }

        public JsonResult GetGrid() {
            InitPager();
            var list = Repo.All;
            int total = list.Count();
            IEnumerable<Stock> source = null;
            if (PG.asc) {
                source = list.OrderBy(GetOrderBy(PG.sort));
            } else {
                source = list.OrderByDescending(GetOrderBy(PG.sort));
            }
            return Json(new { Rows = PagedList<Stock>(source), Total = total });
        }

        public JsonResult Get(string id) {
            var model = Repo.GetStockView(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(string id) {
            ViewBag.Action = "Detail";
            ViewBag.Id = id;
            return View("Form");
        }

        public ActionResult Create() {
            ViewBag.Action = "Create";
            return View("Form");
        }

        [HttpPost]
        public JsonResult Create(Stock model) {
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

        public ActionResult Update(string id) {
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
                List<Stock> models = JsonConvert.DeserializeObject<List<Stock>>(Request["data"]);
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

        private Func<Stock, object> GetOrderBy(string sort) {
            return s => {
                switch (sort) {
                    case "Customername":
                        return s.Customername;
                    case "Memorycard":
                        return s.Memorycard;
                    case "Proname":
                        return s.Proname;
                    case "Brandname":
                        return s.Brandname;
                    case "Moname":
                        return s.Moname;
                    case "Maname":
                        return s.Maname;
                    case "Weight":
                        return s.Weight;
                    case "Carno":
                        return s.Carno;
                    case "Status":
                        return s.Status;
                    default:
                        return s.Did;
                }
            };
        }

        public ActionResult FilterCustomer() {
            return View();
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (Repo != null)
                    Repo.Dispose();
            }
        }
    }
}