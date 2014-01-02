using System;
using System.Linq;
using System.Web.Mvc;
using Logistics.Domain.Repository;
using Ninject;
using Logistics.Portal.Models;

namespace Logistics.Portal.Filters {
    /// <summary>
    /// Set Context before action result excution.
    /// </summary>
    public class PermissionAttribute : ActionFilterAttribute {
        [Inject]
        public ISystemRep SystemRep { get; set; }

        protected enum AuthorizeFailedType {
            None,
            LoseUserInfo,
            NoPermission
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            System.Diagnostics.Debug.Assert(filterContext != null);
            AuthorizeFailedType failedType;
            if (!this.AuthorizeCore(filterContext, out failedType)) {
                if (filterContext.IsChildAction) {
                    filterContext.Result = new EmptyResult();
                } else {
                    switch (failedType) {
                        case AuthorizeFailedType.LoseUserInfo:
                            filterContext.Result = new RedirectResult("~/HTML/LoseUserInfo.htm");
                            break;
                        case AuthorizeFailedType.NoPermission:
                            filterContext.Result = new RedirectResult("~/HTML/NoPermission.htm");
                            break;
                        case AuthorizeFailedType.None:
                        default:
                            break;
                    }
                }
            }
        }

        protected virtual bool AuthorizeCore(ActionExecutingContext filterContext, out AuthorizeFailedType failedType) {
            if (filterContext.HttpContext == null) {
                throw new ArgumentNullException("httpContext");
            }
            bool result = true;
            failedType = AuthorizeFailedType.None;

            if (filterContext.ActionDescriptor.GetCustomAttributes(
                        typeof(AnonymousAttribute), false).Count() == 0) {
                UserInfo userinfo = filterContext.HttpContext.Session["CurrentUserInfo"] as UserInfo;
                if (userinfo == null) {
                    result = false;
                    failedType = AuthorizeFailedType.LoseUserInfo;
                } else {
                    var controller = filterContext.RouteData.Values["controller"].ToString();
                    var action = filterContext.RouteData.Values["action"].ToString();
                    if (!SystemRep.CheckRight(userinfo.RoleId, controller, action)) {
                        result = false;
                        failedType = AuthorizeFailedType.NoPermission;
                    }
                }
            }
            return result;
        }
    }
}