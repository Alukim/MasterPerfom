using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MasterPerform.Tests.Utilities
{
    public static class HttpClientExtensions
    {
        private static readonly JsonSerializerSettings SerializerSettings;

        static HttpClientExtensions()
        {
            SerializerSettings = JsonSerializerSettingsProvider.CreateSerializerSettings();
        }

        public static Task<HttpResponseMessage> Post(this HttpClient client, string url, object content)
            => client.SendRequest(HttpMethod.Post, url, content);

        public static async Task<T> Get<T>(this HttpClient client, string url, object queryParams = null)
        {
            var address = url;

            if (queryParams != null)
            {
                var queryString = string.Join("&",
                    new RouteValueDictionary(queryParams).Where(x => x.Value != null).Select(x => $"{x.Key}={x.Value}"));
                address = $"{url}/?{queryString}";
            }

            var response = await client.SendRequest(HttpMethod.Get, address);
            return response.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(), SerializerSettings)
                : throw new Exception();
        }

        public static Task<HttpResponseMessage> Put(this HttpClient client, string url, object content)
            => client.SendRequest(HttpMethod.Put, url, content);

        public static Guid GetCreatedId(this HttpResponseMessage response)
        {
            var headerValue = response.Headers.GetValues("Location").FirstOrDefault();
            if (string.IsNullOrEmpty(headerValue))
                throw new ArgumentException($"Location header is null or empty");
            var lastToken = headerValue.Substring(headerValue.LastIndexOf('/') + 1);
            return Guid.Parse(lastToken);
        }

        private static async Task<HttpResponseMessage> SendRequest(this HttpClient client, HttpMethod method, string url, object content = null)
        {
            var request = new HttpRequestMessage(method, url);

            if (content != null)
            {
                var json = content is string s ? s : JsonConvert.SerializeObject(content, SerializerSettings);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return await client.SendAsync(request);
        }
    }
}
