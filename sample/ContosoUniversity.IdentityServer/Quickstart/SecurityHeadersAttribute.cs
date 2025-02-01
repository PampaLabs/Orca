// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityServerHost.Quickstart.UI
{
    public class SecurityHeadersAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result;
            if (result is ViewResult)
            {
                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
                context.HttpContext.Response.Headers.TryAdd("X-Content-Type-Options", "nosniff");

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
                context.HttpContext.Response.Headers.TryAdd("X-Frame-Options", "SAMEORIGIN");

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
                var csp = "default-src 'self'; object-src 'none'; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts; base-uri 'self';";
                // also consider adding upgrade-insecure-requests once you have HTTPS in place for production
                //csp += "upgrade-insecure-requests;";
                // also an example if you need client images to be displayed from twitter
                // csp += "img-src 'self' https://pbs.twimg.com;";

                // once for standards compliant browsers
                context.HttpContext.Response.Headers.TryAdd("Content-Security-Policy", csp);

                // and once again for IE
                context.HttpContext.Response.Headers.TryAdd("X-Content-Security-Policy", csp);

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
                var referrer_policy = "no-referrer";
                context.HttpContext.Response.Headers.TryAdd("Referrer-Policy", referrer_policy);
            }
        }
    }
}