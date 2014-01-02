using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Logistics.Domain.Entities;
using Logistics.Domain.Repository;
using Ninject;


namespace Logistics.Portal.Controllers {
    public class BillController : BaseController {

        [Inject]
        public IBillRep Repo { get; set; }

        public ActionResult Index() {
            return View();
        }

        public JsonResult GetGrid() {
            InitPager();
            var list = Repo.All;
            int total = list.Count();
            IEnumerable<BillRecord> source = null;
            if (PG.asc) {
                source = list.OrderBy(GetOrderBy(PG.sort));
            } else {
                source = list.OrderByDescending(GetOrderBy(PG.sort));
            }
            return Json(new { Rows = PagedList<BillRecord>(source), Total = total });
        }

        [HttpPost]
        public JsonResult Build() {
            try {
                var result = Repo.ExecProcedure<SPResult>("sp_build_bill").FirstOrDefault();
                if (result != null && result.Success == 1) {
                    return Json(true);
                } else if (result != null) {
                    Debug.WriteLine("Error Message: No procedure return.");
                } else {
                    Debug.WriteLine("Error Message: {0}.", result.ErrMsg);
                }
            } catch (Exception ex) {
                Debug.WriteLine("Error Message: {0}.", ex.Message);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult Back() {
            try {
                var result = Repo.ExecProcedure<SPResult>("sp_back_bill").FirstOrDefault();
                if (result != null && result.Success == 1) {
                    return Json(true);
                } else if (result != null) {
                    Debug.WriteLine("Error Message: No procedure return.");
                } else {
                    Debug.WriteLine("Error Message: {0}.", result.ErrMsg);
                }
            } catch (Exception ex) {
                Debug.WriteLine("Error Message: {0}.", ex.Message);
            }
            return Json(false);
        }

        [HandleError(ExceptionType = typeof(FileNotFoundException), View = "NotExists.html")]
        public FileResult Export() {
            /*Construct Model for Excel Format
             *Read records from Bills && BillDetails
             * Judge each tbName
             * if the same then add sheet
             * else add new excel
             * rar all excels
             * then provider export
             */
            throw new NotFiniteNumberException();
        }

        private Func<BillRecord, object> GetOrderBy(string sort) {
            return b => {
                switch (sort) {
                    default:
                        return b.Id;
                }
            };
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (Repo != null)
                    Repo.Dispose();
            }
        }

        private class SPResult {
            public string ErrMsg { get; set; }
            public int Success { get; set; }
        }
    }
}