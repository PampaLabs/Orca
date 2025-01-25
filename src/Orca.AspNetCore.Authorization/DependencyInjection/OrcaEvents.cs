using Microsoft.AspNetCore.Http;

namespace Orca
{
    public class OrcaEvents
    {
        public RequestDelegate UnauthorizedFallback { get; set; }
    }
}
