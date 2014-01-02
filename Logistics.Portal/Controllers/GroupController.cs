using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Logistics.Domain.Entities;
using Logistics.Domain.Repository;
using Newtonsoft.Json;
using Ninject;
using Logistics.Portal.Models;
using Logistics.Portal.Binders;

namespace Logistics.Portal.Controllers {
    public class GroupController : BaseController {
        [Inject]
        public IGroupRep Repo { get; set; }

        public List<Group> GroupList {
            get {
                if (Repo != null)
                    return Repo.All.ToList();
                else
                    return new List<Group>();
            }
        }

        #region Group Actions

        public ActionResult Index() {
            return View();
        }

        public ActionResult Create() {
            ViewBag.Action = "Create";
            return View("Form");
        }

        [HttpPost]
        public JsonResult Delete([ModelBinder(typeof(JsonBinder<List<Group>>))] List<Group> groups) {
            try {
                string curUser = CurrentUser.UserId;
                DateTime curtime = DateTime.Now;
                //List<Group> models = JsonConvert.DeserializeObject<List<Group>>(Request["data"]);
                if (groups != null && groups.Count > 0) {
                    foreach (var g in groups) {
                        //SetValuesForModel(model, SubmitAction.Delete);
                        Repo.Delete(g);
                    }
                }
                Repo.SaveChanges();
                return Json(true);
            } catch { }
            return Json(false);
        }

        //Get group list.
        public JsonResult GetGroups() {
            var groups = GroupList;
            return Json(new { Rows = groups, Total = groups.Count }, JsonRequestBehavior.AllowGet);
        }

        //Init liger_tree.
        public JsonResult GetTree() {
            var groups = GroupList;
            List<TreeNode> root = new List<TreeNode>();
            root.Add(new TreeNode {
                id = 0, text = "全部分组", children = groups.Select(g => new TreeNode { id = g.Groupid, text = g.Groupname })
            });
            return Json(root, JsonRequestBehavior.AllowGet);
        }

        private Func<Group, object> GetOrderBy(string sort) {
            return (Group g) => {
                switch (sort) {
                    default:
                        return g.Groupid;
                }
            };
        }

        #endregion Group Actions

        #region Memeber Actions

        public JsonResult GetMembers(int id) {
            InitPager();
            var source = Repo.FindMembers(id, true);
            int count = source.Count();
            var rows = PagedList<Customer>(source).ToList();
            return Json(new { Rows = rows, Total = count }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNoMembers(int id) {
            InitPager();
            var source = Repo.FindMembers(id, false);
            int count = source.Count();
            var rows = PagedList<Customer>(source).ToList();
            return Json(new { Rows = rows, Total = count }, JsonRequestBehavior.AllowGet);
        }

        //Select memebers.
        public ActionResult MemberList() {
            return View();
        }

        [HttpPost]
        public JsonResult AddMembers([ModelBinder(typeof(JsonBinder<List<Customer>>))] List<Customer> models) {
            try {
                int groupid = Convert.ToInt32(Request["groupid"].ToString());
                //List<Customer> models = JsonConvert.DeserializeObject<List<Customer>>(Request["data"]);
                //foreach (var c in models) {
                if (models != null && models.Count > 0) {
                    foreach (var item in models) {
                        Repo.AddMember(groupid, item);
                    }
                }
                Repo.SaveChanges();
                return Json(true);
            } catch { }
            return Json(false);
        }

        [HttpPost]
        public JsonResult DeleteMembers([ModelBinder(typeof(JsonBinder<List<Customer>>))] List<Customer> models) {
            try {
                int groupid = Convert.ToInt32(Request["groupid"].ToString());
                //var group = Repo.Find(groupid);

                //string curUser = CurrentUser.UserId;
                //DateTime curtime = DateTime.Now;
                //List<Customer> models = JsonConvert.DeserializeObject<List<Customer>>(Request["data"]);
                Repo.RemoveMems(groupid, models.Select(c => c.CustomerId));
                //if (models != null && models.Count > 0) {
                //    foreach (var item in models) {
                //        group.Customers.Remove(item);
                //    }
                //}
                //service.DeleteMemebers(groupid, models);
                bool saved = Repo.SaveChanges() > 0;
                return Json(saved);
            } catch { }
            return Json(false);
        }

        #endregion Memeber Actions

        //protected override void Dispose(bool disposing) {
        //    if (disposing) {
        //        if (Repo != null)
        //            Repo.Dispose();
        //    }
        //}
    }
}