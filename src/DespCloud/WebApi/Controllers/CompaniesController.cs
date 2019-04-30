using Base.Domain.Contracts;
using Base.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly UnitWork _unit;

        public CompaniesController(UnitWork unitWorkRepository)
        {
            _unit = unitWorkRepository;
        }

        [HttpGet("{companyId:int}/logo")]
        public IActionResult Logo(int companyId, [FromServices]IHostingEnvironment env)
        {
            var path = Path.Combine(env.ContentRootPath, "AppData/Companies");

            IFileProvider provider = new PhysicalFileProvider(path);
            IFileInfo fileInfo = provider.GetDirectoryContents(companyId.ToString()).Where(x => x.Name.StartsWith("logo.")).FirstOrDefault();

            if (fileInfo == null)
            {
                fileInfo = provider.GetDirectoryContents("default").Where(x => x.Name.StartsWith("logo.")).FirstOrDefault();
            }

            if (fileInfo == null)
                return NotFound();                

            var readStream = fileInfo.CreateReadStream();

            return File(readStream, "application/octet-stream", fileInfo.Name);
        }
       
        [HttpPost("this/logo")]
        public async Task<IActionResult> UploadLogo(IFormFile fileLogo, [FromServices]IHostingEnvironment env)
        {
            // File up using multpart/form-data only (Fetch body FILE not work)
            fileLogo = Request.Form.Files.FirstOrDefault();

            if (fileLogo == null && fileLogo.Length <= 0)
                throw new AppValidationException("Arquivo inválido. O arquivo está vazio");

            if (fileLogo.Length >= 1024 * 1024 * 2)
                throw new AppValidationException("O arquivo excede o tamanho máximo de 2MB");

            var path = Path.Combine(env.ContentRootPath, "AppData/Companies", _unit.Info.CompanyId.ToString());
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            else
            {
                var files = Directory.GetFiles(path, "logo.*").ToList();
                files.ForEach(x => System.IO.File.Delete(x));
            }

            path = Path.Combine(path, "logo" + Path.GetExtension(fileLogo.FileName));

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await fileLogo.CopyToAsync(stream);
            }

            return Ok();
        }

        [HttpGet("this")]
        public IActionResult GetCompany()
        {
            var company = _unit.Companies.GetUserCompany();

            return Ok(company);
        }

        [HttpPut("")]
        public IActionResult UpdateCompany(Company model)
        {
            if (ModelState.IsValid)
            {
                _unit.BeginTransaction();
                var company = _unit.Companies.Update(model);
                _unit.CommitTransaction();

                return Ok(company);
            }
            else
                return ValidationProblem();
        }
    }
}
