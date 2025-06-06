using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using Roblox.ConfigurationWebsite.ViewModels;
using Logger = Roblox.EventLog.Logger;


namespace Roblox.ConfigurationWebsite.Controllers
{
    [RoutePrefix("")]
    public class AccountController : Controller
    {
        /// <summary>
        /// Shows an Error.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="returnUrl"></param>
        /// <returns>
        /// <see cref="ActionResult"/>
        /// </returns>
        private ActionResult ShowError(string errorMessage, string returnUrl) => RedirectToAction("Login", new { ReturnUrl = returnUrl, ErrorMessage = errorMessage });

        [Route("Logout")]
        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            HttpContext.User = null;

            return RedirectToAction("Login", new { ReturnUrl = "/" });
        }

        [Route("Login")]
        [HttpGet]
        public ActionResult Login(string returnUrl = "/", string errorMessage = null)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(returnUrl)) return Redirect(returnUrl);
                return RedirectToAction("Index");
            }

            if (string.IsNullOrEmpty(errorMessage)) errorMessage = null;
            if (string.IsNullOrEmpty(returnUrl)) returnUrl = "/";

            return View(
                new LoginViewModel
                {
                    ErrorMessage = errorMessage,
                    ReturnUrl = returnUrl
                }
            );
        }

        [Route("Login")]
        [HttpPost]
        public ActionResult PostLogin(string userName, string password, string returnUrl = "/")
        {
#if DEBUG
            if (userName == "testing")
            {
                FormsAuthentication.SetAuthCookie(userName.ToLower(), true);
                AuthenticateThisRequest();

                return Redirect(returnUrl);
            }
#endif

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password)) return ShowError("Please fill out all fields.", returnUrl);

            userName = GetUsername(userName);

            // Yes I know this is bad, BUT as i said this is incomplete.
            // This will get removed, Hopefully :tm:.
            if (userName == "Administrator" && password == "CB6A676E-FB46-499C-9032-8E012DE5DABB")
            {

                Logger.Singleton.Information($"Someone logged into {userName}");
                FormsAuthentication.SetAuthCookie(userName.ToLower(), true);
                AuthenticateThisRequest();

                return Redirect(returnUrl);
            }

            return ShowError("Invalid username or password.", returnUrl);
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>
        /// <see cref="System.String"/>
        /// </returns>
        private string GetUsername(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return null;

            var split = userName.Split('\\');
            if (split.Length < 2) return userName;

            // We assume it's in the format 'DOMAIN\USERNAME'
            return split[1];
        }

        /// <summary>
        /// NOTE: if the user is already logged in (e.g. under a different user account)
        /// then this will NOT reset the identity information.  Be aware of this if
        /// you allow already-logged in users to "re-login" as different accounts 
        /// without first logging out.
        /// </summary>
        private void AuthenticateThisRequest()
        {
            if (HttpContext.User.Identity.IsAuthenticated) return;

            var name = FormsAuthentication.FormsCookieName;
            var cookie = Response.Cookies[name];
            if (cookie != null)
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket != null && !ticket.Expired)
                {
                    string[] roles = (ticket.UserData ?? "").Split(',');
                    HttpContext.User = new GenericPrincipal(new FormsIdentity(ticket), roles);
                }
            }
        }
    }
}
