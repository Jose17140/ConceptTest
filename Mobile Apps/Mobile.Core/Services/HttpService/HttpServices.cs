using Microsoft.Extensions.Options;
using Mobile.Core.Interface;
using Mobile.Core.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Mobile.Core.Services.HttpService
{
    public class HttpServices<T> : IHttpServices<T> where T : class
    {
        private readonly HttpClient _client;
        private readonly IAppLogger<T> _appLogger;

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        private HttpClientConfig HttpConfig { get; }

        public HttpServices(HttpClient client, IAppLogger<T> appLogger, IOptions<HttpClientConfig> httpConfig)
        {
            _client = client;
            _appLogger = appLogger;
            HttpConfig = httpConfig.Value;
        }

        /// <summary>
        /// Http Get
        /// </summary>
        /// <param name="requestString"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string requestString, string token = null)
        {
            if (string.IsNullOrEmpty(requestString))
                throw new ArgumentNullException("Url Request is null");

            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpConfig.Application));
                _client.DefaultRequestHeaders.Add(HttpConfig.Cache, HttpConfig.NoChace);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var result = await Task.FromResult(_client.GetAsync(HttpConfig.BaseAddress + requestString).Result);

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsStringAsync();
            }

            throw new HttpRequestException(result.ReasonPhrase);
        }

        /// <summary>
        /// Http Post Create model
        /// </summary>
        /// <param name="requestString"></param>
        /// <param name="body"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> PostAsync(string requestString, T body, string token = null)
        {
            if (string.IsNullOrEmpty(requestString))
                throw new ArgumentNullException("Url Request is null");

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpConfig.Application));
            _client.DefaultRequestHeaders.Add(HttpConfig.Cache, HttpConfig.NoChace);

            if (!string.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await Task.Run(() =>
                        _client.PostAsync(HttpConfig.BaseAddress + requestString,
                        new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, HttpConfig.Application)));

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsStringAsync();
            }

            throw new HttpRequestException(result.ReasonPhrase);
        }

        /// <summary>
        /// Http Put Update model
        /// </summary>
        /// <param name="requestString"></param>
        /// <param name="body"></param>
        /// <param name="token"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<string> PutAsync(string requestString, T body, string token)
        {
            if (string.IsNullOrEmpty(requestString))
                throw new ArgumentNullException("Url Request is null");

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpConfig.Application));
            _client.DefaultRequestHeaders.Add(HttpConfig.Cache, HttpConfig.NoChace);

            if (!string.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await Task.Run(() =>
                                _client.PutAsync(HttpConfig.BaseAddress + requestString,
                                new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, HttpConfig.Application)));

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsStringAsync();
            }

            throw new HttpRequestException(result.ReasonPhrase);
        }

        /// <summary>
        /// Http Delete
        /// </summary>
        /// <param name="requestString"></param>
        /// <param name="body"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> DeleteAsync(string requestString, T body, string token)
        {
            if (string.IsNullOrEmpty(requestString))
                throw new ArgumentNullException("Url Request is null");

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpConfig.Application));
            _client.DefaultRequestHeaders.Add(HttpConfig.Cache, HttpConfig.NoChace);

            if (!string.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Delete, HttpConfig.BaseAddress + requestString);

            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, HttpConfig.Application);

            var result = await Task.Run(() => _client.SendAsync(request));

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsStringAsync();
            }

            throw new HttpRequestException(result.ReasonPhrase);
        }
    }
}
