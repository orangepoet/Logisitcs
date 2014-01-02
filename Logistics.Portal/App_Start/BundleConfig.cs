using System.Web.Optimization;

namespace Logistics.Portal {
    public class BundleConfig {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles) {
            #region Scripts
            //jquery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                            "~/Scripts/jquery/jquery-{version}.js"));

            //site
            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                            "~/Scripts/site.js"));

            //ligerui
            bundles.Add(new ScriptBundle("~/bundles/ligerui").Include(
                            "~/Scripts/ligerUI/ligerui.all*",
                            "~/Scripts/ligerUI/ligerui.expand.js",
                            "~/Scripts/ligerUI/LG.js"));

            //ligerui-form
            bundles.Add(new ScriptBundle("~/bundles/ligerui-form").Include(
                            "~/Scripts/jquery/jquery.form.js",
                            "~/Scripts/jquery/jquery.validate*",
                            "~/Scripts/jquery/jquery.metadata.js",
                            "~/Scripts/ligerUI/messages_cn.js"));


            ////jqueryui
            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //            "~/Scripts/jquery-ui-{version}.js"));

            ////jqueryval
            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));
            #endregion

            #region Styles

            //login
            bundles.Add(new StyleBundle("~/Content/ligerui/login").Include(
                        "~/Content/ligerui/login.css"));

            //site
            bundles.Add(new StyleBundle("~/Content/site").Include(
                        "~/Content/site.css"));

            //ligerui
            bundles.Add(new StyleBundle("~/Content/ligerui/Aqua/css/bundle").Include(
                        "~/Content/ligerui/Aqua/css/*.css"));
            bundles.Add(new StyleBundle("~/Content/ligerui/login").Include(
                        "~/Content/ligerui/login.css"));
                        //"~/Content/ligerui/ligerui-all.css"));

            ////jquery ui
            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //            "~/Content/themes/base/jquery.ui.core.css",
            //            "~/Content/themes/base/jquery.ui.resizable.css",
            //            "~/Content/themes/base/jquery.ui.selectable.css",
            //            "~/Content/themes/base/jquery.ui.accordion.css",
            //            "~/Content/themes/base/jquery.ui.autocomplete.css",
            //            "~/Content/themes/base/jquery.ui.button.css",
            //            "~/Content/themes/base/jquery.ui.dialog.css",
            //            "~/Content/themes/base/jquery.ui.slider.css",
            //            "~/Content/themes/base/jquery.ui.tabs.css",
            //            "~/Content/themes/base/jquery.ui.datepicker.css",
            //            "~/Content/themes/base/jquery.ui.progressbar.css",
            //            "~/Content/themes/base/jquery.ui.theme.css"));
            #endregion
        }
    }
}