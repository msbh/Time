using System.Web;
using System.Web.Optimization;

namespace TimeAtten
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryLowAdmin").Include(
                  "~/Content/themes/MetTheme/js/jquery-1.8.3.min.js",
                      "~/Content/themes/MetTheme/breakpoints/breakpoints.js",
                          "~/Content/themes/MetTheme/bootstrap/js/bootstrap.min.js",
                    "~/Content/themes/MetTheme/bootstrap-fileupload/bootstrap-fileupload.js",
                    "~/Content/themes/MetTheme/js/jquery.blockui.js",
                    "~/Content/themes/MetTheme/js/jquery.cookie.js",
                    "~/Content/themes/MetTheme/ckeditor/ckeditor.js",                    
                        "~/Content/themes/MetTheme/chosen-bootstrap/chosen/chosen.jquery.min.js",
                        "~/Content/themes/MetTheme/bootstrap-wysihtml5/wysihtml5-0.3.0.js",
                        "~/Content/themes/MetTheme/bootstrap-wysihtml5/bootstrap-wysihtml5.js",
                        "~/Content/themes/MetTheme/jquery-tags-input/jquery.tagsinput.min.js",
                        "~/Content/themes/MetTheme/bootstrap-toggle-buttons/static/js/jquery.toggle.buttons.js",
                        "~/Content/themes/MetTheme/bootstrap-datepicker/js/bootstrap-datepicker.js",
                        "~/Content/themes/MetTheme/clockface/js/clockface.js",
                        "~/Content/themes/MetTheme/bootstrap-daterangepicker/date.js",
                        "~/Content/themes/MetTheme/bootstrap-daterangepicker/daterangepicker.js",
                        "~/Content/themes/MetTheme/bootstrap-colorpicker/js/bootstrap-colorpicker.js",
                        "~/Content/themes/MetTheme/bootstrap-timepicker/js/bootstrap-timepicker.js",
                       
                        "~/Content/themes/MetTheme/chosen-bootstrap/chosen/chosen.jquery.min.js"
                  ));


            bundles.Add(new ScriptBundle("~/bundles/jqueryAdmin").Include(
                        "~/Content/themes/Theme/js/jquery.min.js",
                        "~/Content/themes/Theme/js/jquery.ui.custom.js",
                        "~/Content/themes/Theme/js/bootstrap.min.js",
                        "~/Content/themes/Theme/js/jquery.flot.min.js",
                        "~/Content/themes/Theme/js/jquery.flot.resize.min.js",
                        "~/Content/themes/Theme/js/jquery.peity.min.js",
                        "~/Content/themes/Theme/js/fullcalendar.min.js",

                         "~/Content/themes/Theme/js/matrix.js",
                        "~/Content/themes/Theme/js/matrix.dashboard.js",
                        "~/Content/themes/Theme/js/jquery.gritter.min.js",
                        "~/Content/themes/Theme/js/matrix.interface.js",
                        "~/Content/themes/Theme/js/matrix.chat.js",


                          "~/Content/themes/Theme/js/matrix.form_validation.js",
                        "~/Content/themes/Theme/js/matrix.dashboard.js",
                        "~/Content/themes/Theme/js/jquery.wizard.js",
                        "~/Content/themes/Theme/js/jquery.uniform.js",
                        "~/Content/themes/Theme/js/select2.min.js",


                        "~/Content/themes/Theme/js/matrix.popover.js",
                        "~/Content/themes/Theme/js/jquery.dataTables.min.js",
                        "~/Content/themes/Theme/js/matrix.tables.js",



                        "~/Content/themes/Theme/js/excanvas.min.js"
                        ));


            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/themes/Theme/css").Include(
          "~/Content/themes/Theme/css/bootstrap-responsive.min.css",
          "~/Content/themes/Theme/css/bootstrap-wysihtml5.css",
          "~/Content/themes/Theme/css/bootstrap.min.css",
          "~/Content/themes/Theme/css/colorpicker.css",
          "~/Content/themes/Theme/css/datepicker.css",
          "~/Content/themes/Theme/css/fullcalender.css",
          "~/Content/themes/Theme/css/jquery.gritter.css",
          "~/Content/themes/Theme/css/matrix-login.css",
          "~/Content/themes/Theme/css/matrix-media.css",
          "~/Content/themes/Theme/css/matrix-style.css",
          "~/Content/themes/Theme/css/select2.css",
          "~/Content/themes/Theme/css/uniform.css"));

            bundles.Add(new StyleBundle("~/Content/themes/MetTheme/").Include(
                        "~/Content/themes/MetTheme/bootstrap/css/bootstrap.min.css",
                        "~/Content/themes/MetTheme/css/metro.css",
                        "~/Content/themes/MetTheme/bootstrap/css/bootstrap-responsive.min.css",
                        "~/Content/themes/MetTheme/font-awesome/css/font-awesome.css",
                        "~/Content/themes/MetTheme/css/style.css",
                        "~/Content/themes/MetTheme/css/style_responsive.css",
                        "~/Content/themes/MetTheme/css/style_default.css",
                        "~/Content/themes/MetTheme/uniform/css/uniform.default.css",
                        "~/Content/themes/MetTheme/fancybox/source/jquery.fancybox.css",
                        "~/Content/themes/MetTheme/chosen-bootstrap/chosen/chosen.css",
                        "~/Content/themes/MetTheme/data-tables/DT_bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/themes/MetThemeBoard/").Include(
                    "~/Content/themes/MetTheme/gritter/css/jquery.gritter.css",
                     "~/Content/themes/MetTheme/bootstrap-daterangepicker/daterangepicker.css",
                     "~/Content/themes/MetTheme/fullcalendar/fullcalendar/bootstrap-fullcalendar.css",
                     "~/Content/themes/MetTheme/chosen-bootstrap/chosen/chosen.css",
                     "~/Content/themes/MetTheme/jquery-tags-input/jquery.tagsinput.css",
                     "~/Content/themes/MetTheme/clockface/css/clockface.css",
                     "~/Content/themes/MetTheme/bootstrap-wysihtml5/bootstrap-wysihtml5.css",
                     "~/Content/themes/MetTheme/bootstrap-datepicker/css/datepicker.css",
                      "~/Content/themes/MetTheme/bootstrap-timepicker/compiled/timepicker.css",
                      "~/Content/themes/MetTheme/bootstrap-toggle-buttons/static/stylesheets/bootstrap-toggle-buttons.css",
                      "~/Content/themes/MetTheme/data-tables/DT_bootstrap.css",
                      "~/Content/themes/MetTheme/bootstrap-daterangepicker/daterangepicker.css",

                      "~/Content/themes/MetTheme/bootstrap-fileupload/bootstrap-fileupload.css",
                      "~/Content/themes/MetTheme/bootstrap-colorpicker/css/colorpicker.css",
                       "~/Content/themes/MetTheme/data-tables/DT_bootstrap.css",
                     "~/Content/themes/MetTheme/jqvmap/jqvmap/jqvmap.css"));

        }
    }
}