using Microsoft.AspNetCore.Http;
using Microsoft.IO;
using System.Diagnostics;

namespace Common.Shared
{
    public class RequestAndResponseActivityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RecyclableMemoryStreamManager _memoryStream;
        public RequestAndResponseActivityMiddleware(RequestDelegate next)
        {
            _next = next;
            _memoryStream = new RecyclableMemoryStreamManager();
        }
        public async Task InvokeAsync(HttpContext context)
        {
            await AddReqBodyContentToActivityTag(context);
            await AddResponseBodyContentToActivityTag(context);
        }
        private async Task AddReqBodyContentToActivityTag(HttpContext context)
        {
            context.Request.EnableBuffering();
            var reqBodyStreamReader = new StreamReader(context.Request.Body);
            var requestBody = await reqBodyStreamReader.ReadToEndAsync();

            Activity.Current?.SetTag("http.request.body",requestBody);
            context.Request.Body.Position = 0;
        }
        private async Task AddResponseBodyContentToActivityTag(HttpContext context)
        {
            var originalResponse = context.Response.Body;
            await using var responseBodyMemoryStream = _memoryStream.GetStream();
            context.Response.Body = responseBodyMemoryStream;

            await _next(context);

            responseBodyMemoryStream.Position = 0;
            var resBodyStreamReader = new StreamReader(responseBodyMemoryStream);
            var responseBody = await resBodyStreamReader.ReadToEndAsync();

            Activity.Current?.SetTag("http.response.body", responseBody);
            responseBodyMemoryStream.Position = 0;
            await responseBodyMemoryStream.CopyToAsync(originalResponse);
        }
    }
}