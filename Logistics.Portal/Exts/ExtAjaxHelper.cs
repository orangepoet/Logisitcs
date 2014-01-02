using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Logistics.Portal.Exts {
    public static class ExtAjaxHelper {
        private static int linkCnt = 10;

        //分页页标
        public static MvcHtmlString PageLinks(this AjaxHelper helper,
            int totalPage, int curPage, Func<int, string> Uri, AjaxOptions ajaxOptions) {
            StringBuilder result = new StringBuilder();
            TagBuilder tag = null;
            int start = curPage % linkCnt == 0 ? ((curPage / linkCnt - 1) * linkCnt + 1) : ((curPage / linkCnt) * linkCnt + 1);
            int end = (start + linkCnt) <= totalPage ? (start + linkCnt - 1) : totalPage;

            //first
            tag = new TagBuilder("a");
            //ajax-option start
            if (!string.IsNullOrEmpty(ajaxOptions.UpdateTargetId))
                tag.MergeAttribute("data-ajax-update", string.Format("#{0}", ajaxOptions.UpdateTargetId));
            if (!string.IsNullOrEmpty(ajaxOptions.HttpMethod))
                tag.MergeAttribute("data-ajax-method", ajaxOptions.HttpMethod);
            if (!string.IsNullOrEmpty(ajaxOptions.Confirm))
                tag.MergeAttribute("data-ajax-confirm", ajaxOptions.Confirm);
            if (!string.IsNullOrEmpty(ajaxOptions.OnSuccess))
                tag.MergeAttribute("data-ajax-success", ajaxOptions.OnSuccess);
            if (!string.IsNullOrEmpty(ajaxOptions.OnFailure))
                tag.MergeAttribute("data-ajax-failure", ajaxOptions.OnFailure);
            tag.MergeAttribute("data-ajax-mode", ajaxOptions.InsertionMode.ToString());
            tag.MergeAttribute("data-ajax", "true");
            //ajax-option end
            tag.MergeAttribute("href", Uri(1));
            tag.InnerHtml = "第一页";
            result.Append(tag.ToString());

            //prev
            tag = new TagBuilder("a");

            //ajax-option start
            if (!string.IsNullOrEmpty(ajaxOptions.UpdateTargetId))
                tag.MergeAttribute("data-ajax-update", string.Format("#{0}", ajaxOptions.UpdateTargetId));
            if (!string.IsNullOrEmpty(ajaxOptions.HttpMethod))
                tag.MergeAttribute("data-ajax-method", ajaxOptions.HttpMethod);
            if (!string.IsNullOrEmpty(ajaxOptions.Confirm))
                tag.MergeAttribute("data-ajax-confirm", ajaxOptions.Confirm);
            if (!string.IsNullOrEmpty(ajaxOptions.OnSuccess))
                tag.MergeAttribute("data-ajax-success", ajaxOptions.OnSuccess);
            if (!string.IsNullOrEmpty(ajaxOptions.OnFailure))
                tag.MergeAttribute("data-ajax-failure", ajaxOptions.OnFailure);
            tag.MergeAttribute("data-ajax-mode", ajaxOptions.InsertionMode.ToString());
            tag.MergeAttribute("data-ajax", "true");
            //ajax-option end
            tag.MergeAttribute("href", Uri(curPage - 1));
            tag.InnerHtml = "上一页";
            if (curPage == 1)
                tag.MergeAttribute("onclick", "alert('已经是第一页了');return false;");
            result.Append(tag.ToString());

            //during start and end
            for (int i = start; i <= end; i++) {
                tag = new TagBuilder("a");
                //ajax-option start
                if (!string.IsNullOrEmpty(ajaxOptions.UpdateTargetId))
                    tag.MergeAttribute("data-ajax-update", string.Format("#{0}", ajaxOptions.UpdateTargetId));
                if (!string.IsNullOrEmpty(ajaxOptions.HttpMethod))
                    tag.MergeAttribute("data-ajax-method", ajaxOptions.HttpMethod);
                if (!string.IsNullOrEmpty(ajaxOptions.Confirm))
                    tag.MergeAttribute("data-ajax-confirm", ajaxOptions.Confirm);
                if (!string.IsNullOrEmpty(ajaxOptions.OnSuccess))
                    tag.MergeAttribute("data-ajax-success", ajaxOptions.OnSuccess);
                if (!string.IsNullOrEmpty(ajaxOptions.OnFailure))
                    tag.MergeAttribute("data-ajax-failure", ajaxOptions.OnFailure);
                tag.MergeAttribute("data-ajax-mode", ajaxOptions.InsertionMode.ToString());
                tag.MergeAttribute("data-ajax", "true");
                //ajax-option end
                tag.MergeAttribute("href", Uri(i));
                tag.InnerHtml = i.ToString() + " ";
                if (i == curPage) {
                    tag.AddCssClass("selected");
                }
                result.Append(tag.ToString());
            }

            //next
            tag = new TagBuilder("a");
            //ajax-option start
            if (!string.IsNullOrEmpty(ajaxOptions.UpdateTargetId))
                tag.MergeAttribute("data-ajax-update", string.Format("#{0}", ajaxOptions.UpdateTargetId));
            if (!string.IsNullOrEmpty(ajaxOptions.HttpMethod))
                tag.MergeAttribute("data-ajax-method", ajaxOptions.HttpMethod);
            if (!string.IsNullOrEmpty(ajaxOptions.Confirm))
                tag.MergeAttribute("data-ajax-confirm", ajaxOptions.Confirm);
            if (!string.IsNullOrEmpty(ajaxOptions.OnSuccess))
                tag.MergeAttribute("data-ajax-success", ajaxOptions.OnSuccess);
            if (!string.IsNullOrEmpty(ajaxOptions.OnFailure))
                tag.MergeAttribute("data-ajax-failure", ajaxOptions.OnFailure);
            tag.MergeAttribute("data-ajax-mode", ajaxOptions.InsertionMode.ToString());
            tag.MergeAttribute("data-ajax", "true");
            //ajax-option end
            tag.MergeAttribute("href", Uri(curPage + 1));
            tag.InnerHtml = "下一页";
            if (curPage == totalPage)
                tag.MergeAttribute("onclick", "alert('已经是最后一页了');return false;");
            result.Append(tag.ToString());

            //last
            tag = new TagBuilder("a");
            //ajax-option start
            if (!string.IsNullOrEmpty(ajaxOptions.UpdateTargetId))
                tag.MergeAttribute("data-ajax-update", string.Format("#{0}", ajaxOptions.UpdateTargetId));
            if (!string.IsNullOrEmpty(ajaxOptions.HttpMethod))
                tag.MergeAttribute("data-ajax-method", ajaxOptions.HttpMethod);
            if (!string.IsNullOrEmpty(ajaxOptions.Confirm))
                tag.MergeAttribute("data-ajax-confirm", ajaxOptions.Confirm);
            if (!string.IsNullOrEmpty(ajaxOptions.OnSuccess))
                tag.MergeAttribute("data-ajax-success", ajaxOptions.OnSuccess);
            if (!string.IsNullOrEmpty(ajaxOptions.OnFailure))
                tag.MergeAttribute("data-ajax-failure", ajaxOptions.OnFailure);
            tag.MergeAttribute("data-ajax-mode", ajaxOptions.InsertionMode.ToString());
            tag.MergeAttribute("data-ajax", "true");
            //ajax-option end
            tag.MergeAttribute("href", Uri(totalPage));
            tag.InnerHtml = "最后一页";
            result.Append(tag.ToString());
            return MvcHtmlString.Create(result.ToString());
        }

        //图片按钮
        public static MvcHtmlString ImageLink(this AjaxHelper ajaxHelper,
            string linkText, System.Web.Mvc.Ajax.AjaxOptions ajaxOptions
            , string href, string imgSrc) {
            StringBuilder innerHtml = new StringBuilder();

            //img
            TagBuilder img = new TagBuilder("img");
            img.MergeAttribute("src", imgSrc);
            innerHtml.Append(img.ToString());

            //span
            if (linkText != null) {
                TagBuilder span = new TagBuilder("span");
                span.InnerHtml = linkText;
                innerHtml.Append(span.ToString());
            }

            //a
            TagBuilder a = new TagBuilder("a");

            //AjaxOptions
            if (!string.IsNullOrEmpty(ajaxOptions.UpdateTargetId))
                a.MergeAttribute("data-ajax-update", string.Format("#{0}", ajaxOptions.UpdateTargetId));
            if (!string.IsNullOrEmpty(ajaxOptions.HttpMethod))
                a.MergeAttribute("data-ajax-method", ajaxOptions.HttpMethod);
            if (!string.IsNullOrEmpty(ajaxOptions.Confirm))
                a.MergeAttribute("data-ajax-confirm", ajaxOptions.Confirm);
            if (!string.IsNullOrEmpty(ajaxOptions.OnSuccess))
                a.MergeAttribute("data-ajax-success", ajaxOptions.OnSuccess);
            if (!string.IsNullOrEmpty(ajaxOptions.OnFailure))
                a.MergeAttribute("data-ajax-failure", ajaxOptions.OnFailure);
            a.MergeAttribute("data-ajax-mode", ajaxOptions.InsertionMode.ToString());
            a.MergeAttribute("data-ajax", "true");

            a.MergeAttribute("href", href);
            a.InnerHtml = innerHtml.ToString();
            return MvcHtmlString.Create(a.ToString());
        }
    }
}