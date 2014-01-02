using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Logistics.Domain.Entities;
using Ninject;
using Logistics.Domain.Repository;
using System.Linq;
using Logistics.Portal.Models;
using Logistics.Portal.Binders;
using System.Xml.Linq;

namespace Logistics.Portal.Controllers {
    public class MenuController : BaseController {
        [Inject]
        public IMenuRep Repo { get; set; }

        public ActionResult Index() {
            return View();
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
        public JsonResult Create(Menu model) {
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
        public JsonResult Delete([ModelBinder(typeof(JsonBinder<List<Menu>>))]List<Menu> menus) {
            try {
                string curUser = CurrentUser.UserId;
                DateTime curtime = DateTime.Now;
                //List<Menu> models = JsonConvert.DeserializeObject<List<Menu>>(Request["data"]);
                if (menus != null && menus.Count > 0) {
                    foreach (var m in menus) {
                        //SetValuesForModel(model, SubmitAction.Delete);
                        m.Status = "D";
                        m.Modifytime = DateTime.Now;
                        m.Modifyuser = CurrentUser.UserId;
                        Repo.Update(m);
                    }
                }
                Repo.SaveChanges();
                return Json(true);
            } catch { }
            return Json(false);
        }

        public JsonResult GetMenus(int id) {
            InitPager();
            var list = Repo.All.Where(m => m.Status != "D" && m.ParentId == id);
            int total = list.Count();
            IEnumerable<Menu> source = null;
            if (PG.asc) {
                source = list.OrderBy(GetOrderBy(PG.sort));
            } else {
                source = list.OrderByDescending(GetOrderBy(PG.sort));
            }
            return Json(new { Rows = PagedList<Menu>(source), Total = total });
        }

        public JsonResult GetTree() {
            List<TreeNode> treeNodes = new List<TreeNode>();
            var menus = Repo.All.Where(m => m.Status != "D").OrderBy(m => m.Sort).ToList();
            treeNodes.Add(new TreeNode {
                id = 1,
                text = "顶级菜单",
                children = menus.Where(m => m.ParentId == 1)
                                  .OrderBy(m => m.Sort)
                                  .Select(m =>
                                        new TreeNode {
                                            id = m.Id,
                                            text = m.DisplayName,
                                            icon = Url.Content(m.DisplayIcon)
                                        }).ToList<TreeNode>()
            });
            return Json(treeNodes, JsonRequestBehavior.AllowGet);
        }

        private Func<Menu, object> GetOrderBy(string sort) {
            return (Menu m) => {
                switch (sort) {
                    case "DisplayName":
                        return m.DisplayName;
                    case "MenuLinkUrl":
                    case "MenuNo":
                        return m.MenuNo;
                    case "Sort":
                        return m.Sort;
                    default:
                        return m.Id;
                }
            };
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (Repo != null)
                    Repo.Dispose();
            }
        }


        //System Menu
        public PartialViewResult GetMenu(bool frmDB) {
            IEnumerable<Menu> menus = null;
            if (frmDB) {
                UserInfo userinfo = Session["CurrentUserInfo"] as UserInfo;
                menus = Repo.GetMenusByRole(userinfo.RoleId).ToList();
            } else {
                menus = LoadMenuFromConfigXml();
            }
            return PartialView(menus);
        }

        private IEnumerable<Menu> LoadMenuFromConfigXml() {
            XDocument xdoc = XDocument.Load(Server.MapPath("~/menu.xml"));
            return xdoc
                    .Root
                    .Elements("menu")
                    .Select(m => new Menu {
                        DisplayName = m.Attribute("title").Value,
                        ChildMenus = m.Elements("tab")
                                    .Select(t => new Menu {
                                        DisplayIcon = t.Attribute("icon").Value,
                                        MenuNo = t.Attribute("controller").Value,
                                        DisplayName = t.Attribute("title").Value,
                                    }).ToList()
                    });
        }
    }
}