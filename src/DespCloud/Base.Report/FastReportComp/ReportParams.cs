using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Base.Report.FastReportComp
{
    public class ReportParams
    {
        public string FrxPath { get; set; }
        public Dictionary<string, IEnumerable> DataSource { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public int OutputType { get; set; }
        public Stream OutStream { get; set; }
    }
}
