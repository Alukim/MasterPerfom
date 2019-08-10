using MasterPerform.Infrastructure.Exceptions;
using MasterPerform.Infrastructure.Exceptions.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MasterPerform.Infrastructure.WebApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private static readonly JsonSerializerSettings SerializerSettings;

        static ExceptionMiddleware()
        {
            SerializerSettings = JsonSerializerSettingsProvider.CreateSerializerSettings();
        }

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogError("Response has started - cannot handle exception.");
                    throw;
                }

                try
                {
                    var (report, statusCode) = BuildExceptionReport(ex);
                    context.Response.StatusCode = (int) statusCode;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(report, SerializerSettings));
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Exception on handling exception.");
                    throw;
                }
            }
        }

        private (ExceptionReport report, HttpStatusCode) BuildExceptionReport(Exception exception)
        {
            switch (exception)
            {
                case EntityNotFound e:
                    _logger.LogDebug(e, e.Message);
                    var report = new ExceptionReport(
                        code: typeof(EntityNotFound).Name,
                        message: "Entity not found exception occured",
                        details: new List<ExceptionDetails>
                        {
                            new ExceptionDetails(
                                code: e.GetType().Name,
                                message: e.Message)
                        });
                    return (report, HttpStatusCode.NotFound);

                default:
                    _logger.LogError(exception, exception.Message);
                    var reportDefault = new ExceptionReport(
                        code: "UnhandledException",
                        message: "Unhandled exception occured",
                        details: new List<ExceptionDetails>
                        {
                            new ExceptionDetails(
                                code: exception.GetType().Name,
                                message: exception.Message)
                        });
                    return (reportDefault, HttpStatusCode.InternalServerError);
            }
        }
    }
}
