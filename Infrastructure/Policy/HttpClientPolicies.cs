using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polly.Timeout;
using Polly.Retry;
using Polly;
using System.Diagnostics;

namespace Infrastructure.Policy;
public static class HttpClientPolicies
{
    public static AsyncTimeoutPolicy<HttpResponseMessage> TimeoutPolicy =>
        Polly.Policy.TimeoutAsync<HttpResponseMessage>(2, (context, timespan, task) =>
            {
                Debug.WriteLine($"Timeout fired after {timespan.Seconds} seconds");

                return Task.CompletedTask;
            });



    public static AsyncRetryPolicy<HttpResponseMessage> RetryPolicy => Polly.Retry.RetryPolicy
                   .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                   .Or<TimeoutRejectedException>()
        .OrInner<TimeoutException>((x) => x.InnerException != null)
                   .WaitAndRetryAsync(new[]
                       {
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(2),
                        TimeSpan.FromSeconds(5)
                       },
                       (delegateResult, retryCount) =>
                       {
                           Debug.WriteLine(
                               $"Retry  fired, attempt {retryCount}");
                       });

}

