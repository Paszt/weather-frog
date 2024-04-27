using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace weatherfrog.WeatherApi.Utilities
{
    public class RetryHandler(HttpMessageHandler innerHandler) : DelegatingHandler(innerHandler)
    {
        private const int MaxRetries = 3;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                     CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            for (int i = 0; i < MaxRetries; i++)
            {
                response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                if (response.IsSuccessStatusCode || !IsTransientStatusCode(response.StatusCode)) return response;
            }
            return response;
        }

        private static bool IsTransientStatusCode(HttpStatusCode code) => code switch
        {
            HttpStatusCode.RequestTimeout or
            HttpStatusCode.TooManyRequests or
            HttpStatusCode.InternalServerError or
            HttpStatusCode.NotImplemented or
            HttpStatusCode.BadGateway or
            HttpStatusCode.ServiceUnavailable or
            HttpStatusCode.GatewayTimeout or
            HttpStatusCode.HttpVersionNotSupported or
            HttpStatusCode.VariantAlsoNegotiates or
            HttpStatusCode.InsufficientStorage or
            HttpStatusCode.LoopDetected or
            HttpStatusCode.NotExtended or
            HttpStatusCode.NetworkAuthenticationRequired
            => true,
            _ => false,
        };
    }
}
