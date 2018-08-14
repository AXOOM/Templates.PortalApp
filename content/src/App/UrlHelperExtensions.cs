using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyVendor.MyService
{
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Generates a URL with an absolute path for an action method.
        /// Uses the ambient <see cref="HttpContext"/> to get the appropriate URI schema and host name.
        /// </summary>
        /// <param name="helper">The <see cref="IUrlHelper"/>.</param>
        /// <param name="action">The name of the action method.</param>
        /// <param name="controller">The name of the controller.</param>
        /// <param name="values">An object that contains route values.</param>
        /// <returns>The generated URL.</returns>
        public static string ActionAbsolute(this IUrlHelper helper, [AspMvcAction] string action, [AspMvcController] string controller, object values)
        {
            var request = helper.ActionContext?.HttpContext?.Request;
            return helper.Action(action, controller, values, request?.Scheme, request?.Host.Value);
        }
    }
}
