using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Logistics.Domain.Entities;
using Logistics.Domain.Repository;
using Newtonsoft.Json;
using Ninject;
using Logistics.Infrastructure;

namespace Logistics.Portal.Controllers {
    public class CustomerController : BaseController {
        [Inject]
        public ICustomerRep Repo { get; set; }

        public ActionResult Index() {
            return View();
        }

        public JsonResult GetGrid() {
            InitPager();
            var list = Repo.All;
            int total = list.Count();
            IEnumerable<Customer> source = null;
            if (PG.asc) {
                source = list.OrderBy(GetOrderBy(PG.sort));
            } else {
                source = list.OrderByDescending(GetOrderBy(PG.sort));
            }
            return Json(new { Rows = PagedList<Customer>(source), Total = total });
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
        public JsonResult Create(Customer model) {
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
                List<Customer> models = JsonConvert.DeserializeObject<List<Customer>>(Request["data"]);
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

        [HandleError(ExceptionType = typeof(FileNotFoundException), View = "NotExists.html")]
        public FileResult Export() {
            DataTable dt = ListToDt<Customer>(Repo.All.ToList(), p => {
                string[] columns = new string[] { "CustomerName", "ShortName", "MnCode", "Address", "Phones", "Contract" };
                return columns.Contains(p.Name);
            });
            byte[] fileContents = ExcelUti.ReadFileContents(dt);
            return File(fileContents, "application/vnd.ms-excel", "客户信息列表.xls");
        }

        private Func<Customer, object> GetOrderBy(string sort) {
            return c => {
                switch (sort) {
                    case "displayColumns":
                        return c.CustomerName;
                    case "ShortName":
                        return c.ShortName;
                    case "MnCode":
                        return c.MnCode;
                    case "Address":
                        return c.Address;
                    case "Phones":
                        return c.Phones;
                    case "Contract":
                        return c.Contract;
                    default:
                        return c.CustomerId;
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