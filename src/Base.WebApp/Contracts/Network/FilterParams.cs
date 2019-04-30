using System;
using System.Collections.Generic;
using System.Text;

namespace Base.WebApp.Contracts.Network
{
    public class FilterParams
    {
        public string Filter { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
    }
}
