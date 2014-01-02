using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Logistics.Domain.Entities;
using Logistics.Domain.Repository;
using Ninject;

namespace Logistics.Portal.Controllers {
    public class StockDetailController : BaseController {

        [Inject]
        public IStockDetailRep Service { get; set; }

        public ActionResult Index() {
            return View();
        }

        public JsonResult GetGrid() {
            InitPager();
            int total;
            //var list = Service.GetDetailList(PG.pageNo, PG.pageSize, PG.where, PG.sort, PG.asc, out total);
            //return Json(new { Rows = list, Total = total });
            throw new NotImplementedException();
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
        public JsonResult Create(StockDetail model) {
            try {
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
                List<StockDetail> models = JsonConvert.DeserializeObject<List<StockDetail>>(Request["data"]);
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

        private Func<StockDetail, object> GetOrderBy(string sort) {
            return sd => {
                switch (sort) {
                    case "Did":
                        return sd.Did;
                    case "Buttressno":
                        return sd.Buttressno;
                    case "Detailseqno":
                        return sd.Detailseqno;
                    case "Memorycard":
                        return sd.Memorycard;
                    case "Memoryid":
                        return sd.Memoryid;
                    case "Floors":
                        return sd.Floors;
                    case "Unit":
                        return sd.Unit;
                    case "Hottoys":
                        return sd.Hottoys;
                    case "Status":
                        return sd.Status;
                    default:
                        return sd.Did;
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