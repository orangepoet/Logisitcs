using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Logistics.Domain.Entities;
using Logistics.Domain.Repository;
using Newtonsoft.Json;
using Ninject;

namespace Logistics.Portal.Controllers {
    public class StoreController : BaseController {
        [Inject]
        public IStoreRep Service { get; set; }

        public ActionResult Index() {
            return View();
        }

        public JsonResult GetGrid() {
            InitPager();
            var list = Service.All;
            int total = list.Count();
            IEnumerable<Store> source = null;
            if (PG.asc) {
                source = list.OrderBy(GetOrderBy(PG.sort));
            } else {
                source = list.OrderByDescending(GetOrderBy(PG.sort));
            }
            return Json(new { Rows = PagedList<Store>(source), Total = total });
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
        public JsonResult Create(Store model) {
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
                List<Store> models = JsonConvert.DeserializeObject<List<Store>>(Request["data"]);
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

        private Func<Store, object> GetOrderBy(string sort) {
            return c => {
                switch (sort) {
                    case "Names":
                        return c.Names;
                    case "Shortname":
                        return c.Shortname;
                    case "MnCode":
                        return c.Mncode;
                    case "Address":
                        return c.Address;
                    case "Phones":
                        return c.Phones;
                    //case "Faxs":
                    //    return c.Faxs;
                    default:
                        return c.Storeid;
                }
            };
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                Service.Dispose();
            }
        }
    }
}