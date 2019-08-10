using System;

namespace MasterPerform.Infrastructure.Exceptions
{
    public class MasterPerformException : Exception
    {
        public MasterPerformException(string message)
            : base(message) { }
    }
}
