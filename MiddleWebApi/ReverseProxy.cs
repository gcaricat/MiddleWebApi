using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using System.IO;
using Newtonsoft;



namespace MiddleWebApi
{
    public class ReverseProxy
    {
        /// <summary>
        /// defines HTTP client useds to pass requests to the targert server
        /// </summary>
        private static readonly HttpClient _httpClient = new HttpClient();
        /// <summary>
        /// represents any subsequent middleware in asp.net http pipeline
        /// </summary>
        private readonly RequestDelegate _nextMiddleware;



        /// <summary>
        /// initialze the _nextmiddleware
        /// </summary>
        /// <param name="nextMiddleware"></param>
        public ReverseProxy(RequestDelegate nextMiddleware)
        {
            _nextMiddleware = nextMiddleware;
        }

        /// <summary>
        /// 
        /// </summary>
        /// Tries to build the target URI (address of destination server).
        /// if target_uri != null the originary request must be processed
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {

            var targetUri = BuildTargetUri(context.Request);

            if (targetUri != null)
            {
                var targetRequestMessage = CreateTargetMessage(context, targetUri);

                using (var responseMessage = await _httpClient.SendAsync(targetRequestMessage, HttpCompletionOption.ResponseHeadersRead, context.RequestAborted))
                {
                    context.Response.StatusCode = (int)responseMessage.StatusCode;

                    CopyFromTargetResponseHeaders(context, responseMessage);

                    await ProcessResponseContent(context, responseMessage);
                }

                return;
            }
            

            await _nextMiddleware(context);
        }

        private async Task ProcessResponseContent(HttpContext context, HttpResponseMessage responseMessage)
        {
            var content = await responseMessage.Content.ReadAsByteArrayAsync();

            if (IsContentOfType(responseMessage, "text/html") || IsContentOfType(responseMessage, "text/javascript"))
            {
                
            }
            else
            {

                await context.Response.Body.WriteAsync(content);
            }
        }

        private bool IsContentOfType(HttpResponseMessage responseMessage, string type)
        {
            var result = false;

            if (responseMessage.Content?.Headers?.ContentType != null)
            {
                result = responseMessage.Content.Headers.ContentType.MediaType == type;
            }

            return result;
        }

        /// <summary>
        /// create a message for the destination server and sends it by using
        /// </summary>
        /// <param name="context"></param>
        /// <param name="targetUri"></param>
        /// <returns></returns>
        private HttpRequestMessage CreateTargetMessage(HttpContext context, Uri targetUri)
        {
            var requestMessage = new HttpRequestMessage();
            CopyFromOriginalRequestContentAndHeaders(context, requestMessage);

            targetUri = new Uri(QueryHelpers.AddQueryString(targetUri.OriginalString, new Dictionary<string, string>() { { "entry.1884265043", "admin" } }));

            requestMessage.RequestUri = targetUri;
            requestMessage.Headers.Host = targetUri.Host;
            requestMessage.Method = GetMethod(context.Request.Method);

            return requestMessage;
        }

        private void CopyFromOriginalRequestContentAndHeaders(HttpContext context, HttpRequestMessage requestMessage)
        {
            var requestMethod = context.Request.Method;

            if (!HttpMethods.IsGet(requestMethod) &&
                !HttpMethods.IsHead(requestMethod) &&
                !HttpMethods.IsDelete(requestMethod) &&
                !HttpMethods.IsTrace(requestMethod))
            {
                var streamContent = new StreamContent(context.Request.Body);
                requestMessage.Content = streamContent;
            }

            foreach (var header in context.Request.Headers)
            {
                requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
        }

        private void CopyFromTargetResponseHeaders(HttpContext context, HttpResponseMessage responseMessage)
        {
            foreach (var header in responseMessage.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }

            foreach (var header in responseMessage.Content.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }
            context.Response.Headers.Remove("transfer-encoding");
        }
        private static HttpMethod GetMethod(string method)
        {
            if (HttpMethods.IsDelete(method)) return HttpMethod.Delete;
            if (HttpMethods.IsGet(method)) return HttpMethod.Get;
            if (HttpMethods.IsHead(method)) return HttpMethod.Head;
            if (HttpMethods.IsOptions(method)) return HttpMethod.Options;
            if (HttpMethods.IsPost(method)) return HttpMethod.Post;
            if (HttpMethods.IsPut(method)) return HttpMethod.Put;
            if (HttpMethods.IsTrace(method)) return HttpMethod.Trace;
            return new HttpMethod(method);
        }

        /// <summary>
        /// Look for requests whose part start with specific parameter and return new uri
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private Uri BuildTargetUri(HttpRequest request)
        {
            Uri targetUri = null;
            PathString remainingPath;

            ApiNbp apiNbp = new ApiNbp();
            string strUrl = null;

            if (request.Path.StartsWithSegments("/tables", out remainingPath)) {
                strUrl = apiNbp.createApiUrl("tables");
                targetUri = new Uri(strUrl + remainingPath);
            }

            if (request.Path.StartsWithSegments("/rates", out remainingPath))
            {
                strUrl = apiNbp.createApiUrl("rates");
                targetUri = new Uri(strUrl + remainingPath);
            }

            if (request.Path.StartsWithSegments("/tables-middle-A", out remainingPath))
            {
                strUrl = apiNbp.createApiUrl("tables/A");
                targetUri = new Uri(strUrl + remainingPath);
            }

            if (request.Path.StartsWithSegments("/tables-middle-B", out remainingPath))
            {
                strUrl = apiNbp.createApiUrl("tables/B");
                targetUri = new Uri(strUrl + remainingPath);
            }

            if (request.Path.StartsWithSegments("/tables-buy-sell", out remainingPath))
            {
                strUrl = apiNbp.createApiUrl("tables/C");
                targetUri = new Uri(strUrl + remainingPath);
            }

            if (request.Path.StartsWithSegments("/rates-middle-A", out remainingPath))
            {
                strUrl = apiNbp.createApiUrl("rates/A");
                targetUri = new Uri(strUrl + remainingPath);
            }

            if (request.Path.StartsWithSegments("/rates-middle-B", out remainingPath))
            {
                strUrl = apiNbp.createApiUrl("rates/B");
                targetUri = new Uri(strUrl + remainingPath);
            }

            if (request.Path.StartsWithSegments("/rates-buy-sell", out remainingPath))
            {
                strUrl = apiNbp.createApiUrl("rates/C");
                targetUri = new Uri(strUrl + remainingPath);
            }


            return targetUri;
        }

        



    }
}
