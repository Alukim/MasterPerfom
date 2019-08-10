using FluentAssertions;
using MasterPerform.Infrastructure.Exceptions.Entities;
using MasterPerform.Infrastructure.Tests;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
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

        public static async Task<HttpResponseMessage> Post(this HttpClient client, string url, object content)
        {
            var response = await client.SendRequest(HttpMethod.Post, url, content);
            return await response.AssertCreated();
        }

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
            await response.AssertSuccess();
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(), SerializerSettings);
        }

        public static async Task<HttpResponseMessage> Put(this HttpClient client, string url, object content)
        {
            var response = await client.SendRequest(HttpMethod.Put, url, content);
            return await response.AssertSuccess();
        }

        public static async Task<HttpResponseMessage> AssertCreated(this HttpResponseMessage httpResponse)
        {
            await httpResponse.AssertSuccess();
            httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            return httpResponse;
        }

        public static async Task<HttpResponseMessage> AssertSuccess(this HttpResponseMessage httpResponse)
        {
            return httpResponse.IsSuccessStatusCode
                ? httpResponse
                : throw new TestRequestFailed(JsonConvert.DeserializeObject<ExceptionReport>(await httpResponse.Content.ReadAsStringAsync(), SerializerSettings), (int)httpResponse.StatusCode);
        }

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

        public static void ActionShouldFailWithCode()
        {

        }

    }
}
