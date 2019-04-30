using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Reports.OrderServiceReceipt
{
    public class ReportModel
    {
        public string OrderId { get; set; }

        public string CompanyName { get; set; }
        public string CompanyCpfCnpj { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyAddress { get; set; }
        public byte[] CompanyLogo { get; set; }

        public string ClientName { get; set; }
        public string ClientCpfCnpj { get; set; }
        public string ClientAddress { get; set; }

        public DateTime Date { get; set; }
        public decimal Total { get; set; }

        public List<ServiceModel> Services { get; set; }       


        public class ServiceModel
        {
            public string Name { get; set; }
            public string Vehicle { get; set; }
            public decimal Honorary { get; set; }
            public decimal PlateCard { get; set; }
            public decimal Rate { get; set; }
            public decimal Other { get; set; }
        }
    }
}
