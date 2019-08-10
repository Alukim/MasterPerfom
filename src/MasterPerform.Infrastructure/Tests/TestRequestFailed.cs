using MasterPerform.Infrastructure.Exceptions.Entities;
using System;

namespace MasterPerform.Infrastructure.Tests
{
    public class TestRequestFailed : Exception
    {
        public TestRequestFailed(ExceptionReport report, int statusCode)
        {
            Report = report;
            StatusCode = statusCode;
        }

        public ExceptionReport Report { get; }
        public int StatusCode { get; }
    }
}
