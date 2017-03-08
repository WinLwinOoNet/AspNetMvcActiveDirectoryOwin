using System.Web.Optimization;

namespace AspNetMvcActiveDirectoryOwin.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
#if !DEBUG
            // Set EnableOptimizations to false for debugging.
            BundleTable.EnableOptimizations = true;
#endif
            bundles.UseCdn = true;

            AddCss(bundles);
            AddJavaScript(bundles);
        }

        private static void AddCss(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.*",
                "~/Content/font-awesome.*",
                "~/Content/AdminLTE/AdminLTE.*",
                "~/Content/AdminLTE/skin-blue-light.*",
                "~/Content/AdminLTE/skin-green-light.*",
                "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
                "~/Content/kendo/kendo.bootstrap.min.css",
                "~/Content/kendo/kendo.common-bootstrap.min.css"));
        }

        private static void AddJavaScript(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.validate*",
                "~/Scripts/underscore.*",
                "~/Scripts/AdminLTE/app.*",
                "~/Scripts/AdminLTE/jquery.slimscroll.*",
                "~/Scripts/kendo/kendo.all.min.js",
                "~/Scripts/kendo/kendo.aspnetmvc.min.js",
                "~/Scripts/bootstrap.*",
                "~/Scripts/common.js"));
        }
    }
}