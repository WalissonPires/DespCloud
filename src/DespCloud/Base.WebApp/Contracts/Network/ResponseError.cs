using System;
using System.Collections.Generic;
using System.Text;

namespace Base.WebApp.Contracts.Network
{
    public class ResponseError
    {
        public string TraceId { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public Dictionary<string, string> Erros { get; set; }
    }
}
