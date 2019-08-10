using System;
using System.Collections.Generic;
using System.Text;

namespace MasterPerform.Infrastructure.Exceptions.Entities
{
    public class ExceptionReport
    {
        public string Code { get; }
        public string Message { get; }
        public IReadOnlyCollection<ExceptionDetails> Details { get; }

        public ExceptionReport(
            string code,
            string message,
            IReadOnlyCollection<ExceptionDetails> details = null)
        {
            Code = code;
            Message = message;
            Details = details ?? Array.Empty<ExceptionDetails>();
        }
    }
}
