using System;
using System.IO;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Nexum.Zircon.Log;
using Zircon.Core.Authorization;

namespace CRM.Server.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private readonly string _tableName = "dbo.CommonWebApiLog";

        public RequestResponseLoggingMiddleware(RequestDelegate next,
                                                ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory
                      .CreateLogger<RequestResponseLoggingMiddleware>();
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }
        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);
            await LogResponse(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            string messageBody = ReadStreamInChunks(requestStream);

            _logger.LogInformation($"Http Request Information:{Environment.NewLine}" +
                                   $"Schema:{context.Request.Scheme} " +
                                   $"Host: {context.Request.Host} " +
                                   $"Path: {context.Request.Path} " +
                                   $"QueryString: {context.Request.QueryString} " +
                                   $"Request Body: {messageBody}");

            int logOid = DbLogHelper.CreateLogRecord(_tableName, messageBody, null, CommonUserContextManager.CurrentSystem == SystemOid.None ?
               (Guid?)null :
               CommonUserContextManager.CurrentUserGuid, context.Request.Path, context.Request.Host.Value, null);

            // Saving log id in the request, so that other handlers in the request processing pipeline could use it, if needed. 
            if (!context.Items.ContainsKey("LogId"))
                context.Items.Add("LogId", logOid);

            context.Request.Body.Position = 0;
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);
            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);
            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk,
                                                   0,
                                                   readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);
            return textWriter.ToString();
        }

        private async Task LogResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;
            
            await _next(context);
            
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            
            string messageBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            
            _logger.LogInformation($"Http Response Information:{Environment.NewLine}" +
                                   $"Schema:{context.Request.Scheme} " +
                                   $"Host: {context.Request.Host} " +
                                   $"Path: {context.Request.Path} " +
                                   $"QueryString: {context.Request.QueryString} " +
                                   $"Response Body: {messageBody}");

            // If haven't added LogId, then we can't get reference to a logged request
            if (!context.Items.ContainsKey("LogId"))
                return;

            int logOid = (int)context.Items["LogId"];

            
            DbLogHelper.UpdateLogRecord(_tableName, logOid, messageBody, (short)context.Response.StatusCode, CommonUserContextManager.CurrentUserGuid);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }


    public static class RequestResponseLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}
