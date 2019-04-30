using FastReport.Export;
using FastReport.Export.Html;
using FastReport.Export.Image;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Base.Report.FastReportComp
{
    /// <summary>
    /// FastReport Core Utils
    /// </summary>
    public static class ReportBuilder
    {
        public static void CreateDataSource(string frxPath, Dictionary<string, IEnumerable> datasource, int maxNestingLevel = 3)
        {
            var rel = new FastReport.Report();
            rel.Load(frxPath);

            foreach (var pair in datasource)
            {
                rel.Dictionary.RegisterBusinessObject(pair.Value, pair.Key, maxNestingLevel, true);
            }

            rel.Save(frxPath);
        }

        public static void GenereteReport(ReportParams reportParams)
        {
            if (reportParams == null)
                throw new ArgumentNullException($"{nameof(reportParams)} cannot be null.");

            // create report instance
            using (FastReport.Report report = new FastReport.Report())
            {
                // load the existing report                
                report.Load(reportParams.FrxPath);

                // register datasource's           
                if (reportParams.DataSource != null)
                {
                    foreach (var pair in reportParams.DataSource)
                        report.RegisterData(pair.Value, pair.Key);
                }

                if (reportParams.Parameters != null)
                {
                    foreach (var pair in reportParams.Parameters)
                        report.SetParameterValue(pair.Key, pair.Value);
                }

                // prepare the report
                report.Prepare();
                                
                ExportBase export = null;
                switch (reportParams.OutputType)
                {
                    case 0: // export to html
                        export = new HTMLExport();
                        (export as HTMLExport).Format = HTMLExportFormat.HTML;
                        break;
                    case 1:
                        export = new ImageExport();
                        (export as ImageExport).ImageFormat = ImageExportFormat.Png;
                        break;                    
                    default:
                        throw new ArgumentException($"O parametro {reportParams.OutputType} é inválido");
                }
               
                report.Export(export, reportParams.OutStream);
            }

            reportParams.OutStream.Position = 0;            
        }
    }
}
