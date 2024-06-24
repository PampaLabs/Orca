using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.TestHost
{
    public static class RequestBuilderExtensions
    {
        public static Task<HttpResponseMessage> PutAsync(this RequestBuilder builder)
        {
            return builder.SendAsync(HttpMethods.Put);
        }
    }
}
