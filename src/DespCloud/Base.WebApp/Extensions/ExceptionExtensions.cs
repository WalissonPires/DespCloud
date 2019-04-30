using System;
using System.Collections.Generic;
using System.Text;

namespace Base.WebApp.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetRelevantMessage(this Exception exception)
        {
            Exception excp = exception;

            while(excp.InnerException != null)
            {
                excp = excp.InnerException;
            }

            return excp.Message;
        }
    }
}
