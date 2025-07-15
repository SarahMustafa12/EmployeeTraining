using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InternalTraining.Test_Cookie
{

    public class CookieOrRoleAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _cookieName;
        private readonly string _role;

        public CookieOrRoleAuthorizeAttribute(string cookieName, string role)
        {
            _cookieName = cookieName;
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var hasRole = user.Identity?.IsAuthenticated == true && user.IsInRole(_role);
            var hasCookie = context.HttpContext.Request.Cookies.ContainsKey(_cookieName);

            if (!hasRole && !hasCookie)
            {
                // Redirect to AccessDenied page
                context.Result = new RedirectToActionResult("AccessDenied", "Account", new { area = "Identity" });
            }
        }
    }

}
