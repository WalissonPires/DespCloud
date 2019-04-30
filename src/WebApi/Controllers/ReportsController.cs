using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using WebApi.Reports.OrderServiceReceipt;
using WebApi.Repository;
using System.Linq;
using Microsoft.Extensions.FileProviders;
using System.Linq.Expressions;
using Base.Domain.Entities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IHostingEnvironment _env;
        private readonly UnitWork _unit;


        public ReportsController(IHostingEnvironment env, UnitWork unitWork)
        {
            _env = env;
            _unit = unitWork;
        }

        [HttpGet("receipt")]
        public IActionResult Receipt([FromQuery]int orderId)
        {
            var reportTemplatePath = Path.Combine(_env.ContentRootPath, "Reports/OrderServiceReceipt/receipt.frx");
            var dataSource = new Dictionary<string, IEnumerable>();
            var outStream = new MemoryStream();                                        

            var query = from order in _unit.Orders.OrdersNoTracking
                        where order.CompanyId == _unit.Info.CompanyId
                        && order.Id == orderId
                        let comp = order.Company
                        let compAddress = comp.Address
                        let clientAddress = order.Client.Address
                        select new ReportModel
                        {
                            CompanyName = comp.Name,
                            CompanyCpfCnpj = comp.CpfCnpj,
                            CompanyAddress = compAddress.Number != null && compAddress.Street != null ? String.Format("{0}, {1}, {2} - {3}/{4}", compAddress.Street, compAddress.Number, compAddress.DistrictName, compAddress.CityName, compAddress.CountyInitials) : String.Empty,
                            CompanyPhone = order.Company.Phone,

                            ClientName = order.Client.Name,
                            ClientCpfCnpj = order.Client.CpfCnpj,
                            ClientAddress = clientAddress.Number != null && clientAddress.Street != null ? String.Format("{0}, {1}, {2} - {3}/{4}", clientAddress.Street, clientAddress.Number, clientAddress.DistrictName, clientAddress.CityName, clientAddress.CountyInitials) : String.Empty,

                            Date = order.CreateAt.Date,
                            OrderId = order.Id.ToString(),
                            Total = order.Items.Sum(x => x.Total),

                            Services = order.Items.Select(x => new ReportModel.ServiceModel
                            {
                                Name = x.Service.Name,
                                Vehicle = String.Format("{0} {1} {2} {3}", x.Vehicle.Manufacturer, x.Vehicle.Model, x.Vehicle.Color, x.Vehicle.Plate),
                                Honorary = x.Honorary,
                                Rate = x.Rate,
                                PlateCard = x.PlateCard,
                                Other = x.Other
                            }).ToList()
                        };

            dataSource.Add("OSData", query.ToList());

            (dataSource["OSData"] as List<ReportModel>)[0].CompanyLogo = GetCompanyLogo();

            //Base.Report.FastReportComp.ReportBuilder.CreateDataSource(reportTemplatePath, dataSource);
            Base.Report.FastReportComp.ReportBuilder.GenereteReport(new Base.Report.FastReportComp.ReportParams
            {
                FrxPath = reportTemplatePath,
                DataSource = dataSource,                
                OutputType = 1,
                OutStream = outStream
            });
            return File(outStream, "image/png");
        }

        private byte[] GetCompanyLogo()
        {
            var path = Path.Combine(_env.ContentRootPath, "AppData/Companies", _unit.Info.CompanyId.ToString());
            if (!Directory.Exists(path))
                return null;

            IFileProvider provider = new PhysicalFileProvider(path);
            IFileInfo logoFileInfo = provider.GetDirectoryContents("").Where(x => x.Name.StartsWith("logo.")).FirstOrDefault();

            using (var companyLogoStream = new MemoryStream())
            using(var readStream = logoFileInfo.CreateReadStream())
            {
                if (logoFileInfo == null)
                    return null;

                readStream.CopyTo(companyLogoStream);

                return companyLogoStream.ToArray();
            }
        }
    }
}
