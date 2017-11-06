using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CSEBrazil.Library.Utils
{
    public static class HTTPExtension
    {
        static Action Throttled;
        
        private static async Task<TResponse> RunTaskWithAutoRetryOnQuotaLimitExceededError<TResponse>(Func<Task<TResponse>> action, int RetryCount, int RetryDelay)
        {
            int retriesLeft = RetryCount;
            int delay = RetryDelay;

            TResponse response = default(TResponse);

            while (true)
            {
                try
                {
                    response = await action();
                    break;
                }
                catch (Exception exception) when (retriesLeft > 0)
                {
                    if (retriesLeft == 1 && Throttled != null)
                    {
                        Throttled();
                    }

                    await Task.Delay(delay);
                    retriesLeft--;
                    delay *= 2;
                    continue;
                }
            }

            return response;
        }

        public static async Task<T> PostContentAsync<T>(this HttpClient client, string uri, object content, int RetryCount = 3, int DelayCount = 500) where T : class
        {
            var strContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            var result = await RunTaskWithAutoRetryOnQuotaLimitExceededError<HttpResponseMessage>(() => client.PostAsync(uri, strContent), RetryCount, DelayCount));
            result.EnsureSuccessStatusCode();
            var strResponse = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(strResponse);
        }

        public static async Task<T> PutContentAsync<T>(this HttpClient client, string uri, object content, int RetryCount = 3, int DelayCount = 500) where T : class
        {
            var strContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            var result = await RunTaskWithAutoRetryOnQuotaLimitExceededError<HttpResponseMessage>(() => client.PutAsync(uri, strContent), RetryCount, DelayCount));
            result.EnsureSuccessStatusCode();
            var strResponse = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(strResponse);
        }

        public static async Task<T> GetContentAsync<T>(this HttpClient client, string uri, int RetryCount = 3, int DelayCount = 500) where T : class
        {
            var result = await RunTaskWithAutoRetryOnQuotaLimitExceededError<HttpResponseMessage>(() => client.GetAsync(uri), RetryCount, DelayCount);
            result.EnsureSuccessStatusCode();
            var strResponse = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(strResponse);
        }
    }
}
