using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Helpers;

namespace WiseX.Models
{
    public class ErrorModel
    {
        public ErrorLog ErrorLog;
    }

    public class ErrorLog : EntityBase
    {
        public int Id { get; set; }
        public string Controller { get; set; }
        public string view { get; set; }
        public string Method { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string IPAddress { get; set; }
        public string URL { get; set; }

    }
    public class MyAppException : Exception
    {
        public MyAppException()
        { }

        public MyAppException(string message)
            : base(message)
        { }

        public MyAppException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
