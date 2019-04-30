using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.Contracts
{
    public class AppValidationException : Exception
    {
        public Dictionary<string, string> Erros { get; private set; }

        public AppValidationException(string messsage, Dictionary<string, string> erros = null) : base(messsage)
        {
            Erros = erros;
        }
    }
}
