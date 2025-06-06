using System.Web.Mvc;

namespace Roblox.ConfigurationWebsite
{
    public static class HtmlUtility
    {
        /// <summary>
        /// Checks if control = routeControl and action = routeAction.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="control"></param>
        /// <param name="action"></param>
        /// <returns>
        /// <see cref="System.String"/>
        /// </returns>
        public static string IsActive(this HtmlHelper html,
                                  string control,
                                  string action)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];

            // must match both
            var returnActive = control == routeControl &&
                               action == routeAction;

            return returnActive ? "active" : "";
        }

        /// <summary>
        /// Checks if control = routeControl.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="control"></param>
        /// <returns>
        /// <see cref="System.String"/>
        /// </returns>
        public static string IsActive(this HtmlHelper html,
                                  string control)
        {
            var routeData = html.ViewContext.RouteData;
            
            var routeControl = (string)routeData.Values["controller"];

            var returnActive = control == routeControl;

            return returnActive ? "active" : "";
        }
    }
}