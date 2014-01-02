using System;
using System.Text;
using System.Web.Mvc;

namespace Logistics.Portal.Exts {
    public enum ToolBarAction {
        Create,
        Delete,
        Update,
        Details,
        Export,
        Import,
        Gen,
        Back
    }

    public static class ExtHtmlHelper {
        private static int linkCnt = 10;

        //分页页标
        public static MvcHtmlString PageLinks(this HtmlHelper helper,
            int totalPage, int curPage, Func<int, string> Uri) {
            StringBuilder result = new StringBuilder();
            TagBuilder tag = null;
            int start = curPage % linkCnt == 0 ? ((curPage / linkCnt - 1) * linkCnt + 1) : ((curPage / linkCnt) * linkCnt + 1);
            int end = (start + linkCnt) <= totalPage ? (start + linkCnt - 1) : totalPage;

            //first
            tag = new TagBuilder("a");
            tag.MergeAttribute("href", Uri(1));
            tag.InnerHtml = "第一页";
            result.Append(tag.ToString());

            //prev
            tag = new TagBuilder("a");
            tag.MergeAttribute("href", Uri(curPage - 1));
            tag.InnerHtml = "上一页";
            if (curPage == 1)
                tag.MergeAttribute("onclick", "alert('已经是第一页了');return false;");
            result.Append(tag.ToString());

            //during start and end
            for (int i = start; i <= end; i++) {
                tag = new TagBuilder("a");
                tag.MergeAttribute("href", Uri(i));
                tag.InnerHtml = i.ToString() + " ";
                if (i == curPage) {
                    tag.AddCssClass("selected");
                }
                result.Append(tag.ToString());
            }

            //next
            tag = new TagBuilder("a");
            tag.MergeAttribute("href", Uri(curPage + 1));
            tag.InnerHtml = "下一页";
            if (curPage == totalPage)
                tag.MergeAttribute("onclick", "alert('已经是最后一页了');return false;");
            result.Append(tag.ToString());

            //last
            tag = new TagBuilder("a");
            tag.MergeAttribute("href", Uri(totalPage));
            tag.InnerHtml = "最后一页";
            result.Append(tag.ToString());
            return MvcHtmlString.Create(result.ToString());
        }

        //图片按钮
        public static MvcHtmlString ImageLink(this HtmlHelper helper,
            string href, string imgSrc, string spanText = "") {
            StringBuilder innerHtml = new StringBuilder();

            //img
            TagBuilder img = new TagBuilder("img");
            img.MergeAttribute("src", imgSrc);
            innerHtml.Append(img.ToString());

            //span
            if (spanText != null) {
                TagBuilder span = new TagBuilder("span");
                span.InnerHtml = spanText;
                innerHtml.Append(span.ToString());
            }

            //a
            TagBuilder a = new TagBuilder("a");
            a.MergeAttribute("href", href);
            a.InnerHtml = innerHtml.ToString();

            return MvcHtmlString.Create(a.ToString());
        }

        //列表工具栏
        public static MvcHtmlString ToolBar(this HtmlHelper helper, UrlHelper Url, ToolBarAction[] actions) {
            StringBuilder sb = new StringBuilder();
            foreach (ToolBarAction action in actions) {
                switch (action) {
                    case ToolBarAction.Create:
                        sb.Append(helper.ImageLink(Url.Action("Create"),
                            Url.Content("~/Content/images/icon_add.gif"), "添加") + "  ");
                        break;
                    case ToolBarAction.Delete:
                        break;
                    case ToolBarAction.Update:
                        break;
                    case ToolBarAction.Details:
                        break;
                    case ToolBarAction.Export:
                        break;
                    case ToolBarAction.Import:
                        sb.Append(helper.ImageLink(Url.Action("Import"),
                            Url.Content("~/Content/images/icon-import.gif"), "导入") + "  ");
                        break;
                    case ToolBarAction.Gen:
                        sb.Append(helper.ImageLink(Url.Action("Gen"),
                           Url.Content("~/Content/images/icon-gen.gif"), "生成") + "  ");
                        break;
                    case ToolBarAction.Back:
                        sb.Append(helper.ImageLink(Url.Action("Back"),
                            Url.Content("~/Content/images/icon-back.gif"), "回退") + "  ");
                        break;
                    default:
                        break;
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}