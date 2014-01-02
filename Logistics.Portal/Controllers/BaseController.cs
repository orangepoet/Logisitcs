using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Logistics.Portal.Filters;
using Logistics.Portal.Models;

namespace Logistics.Portal.Controllers {
    [Permission]
    public abstract class BaseController : System.Web.Mvc.Controller {
        protected Pager PG { get; set; }

        public struct Pager {
            public int pageNo;
            public int pageSize;
            public string sort;
            public bool asc;
            public string where;
            public string parms;
        }

        public UserInfo CurrentUser {
            get {
                if (Session["CurrentUserInfo"] == null) {
                    throw new Exception("当前用户信息丢失");
                }
                return Session["CurrentUserInfo"] as UserInfo;
            }
            set { Session["CurrentUserInfo"] = value; }
        }

        protected void InitPager() {
            PG = new Pager() {
                pageNo = Convert.ToInt32(Request["page"]),
                pageSize = Convert.ToInt32(Request["pagesize"]),
                sort = Request["sortname"],
                asc = Request["sortorder"] != "desc",
                where = Request["where"],
                parms = Request["p"],
            };
        }

        //private string GetTableName() {
        //    Type t = typeof(T);
        //    var attributes = t.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.TableAttribute), false);
        //    if (attributes.Length == 0) {
        //        return t.Name + "s";
        //    } else {
        //        return (attributes[0] as System.ComponentModel.DataAnnotations.TableAttribute).Name;
        //    }
        //}

        #region Common Methods

        protected IEnumerable<U> PagedList<U>(IEnumerable<U> source) {
            return source.Skip((PG.pageNo - 1) * PG.pageSize).Take(PG.pageSize);
        }

        protected DataTable ListToDt<T>(IEnumerable<T> value, Func<PropertyInfo, bool> propertyFilter) {
            List<PropertyInfo> pList = new List<PropertyInfo>();
            Type type = typeof(T);
            DataTable dt = new DataTable();
            dt.TableName = type.Name;
            Array.ForEach<PropertyInfo>(type.GetProperties().Where(propertyFilter).ToArray(), p => {
                pList.Add(p);
                dt.Columns.Add(p.Name, p.PropertyType);
            });
            foreach (var item in value) {
                DataRow row = dt.NewRow();
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                dt.Rows.Add(row);
            }
            return dt;
        }

        #endregion Common Methods
    }
}