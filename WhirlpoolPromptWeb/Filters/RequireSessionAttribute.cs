using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;

namespace WhirlpoolPromptWeb.Filters
{
    public class RequireSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var endpoint = context.HttpContext.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>() != null)
                return;

            if (context.HttpContext.Session.GetInt32("UserId") == null)
            {
                context.Result = new RedirectToActionResult("Login", "Login", null);
            }
        }
    }
}