using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Database;
using WebApi.Repository;

namespace WebApi.StartupConfig
{
    public static class DatabaseConfig
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("SqliteConnectionString");
            services.AddDbContext<AppDbContext>(x => x.UseSqlite(connectionString));

            services.AddScoped<UnitWorkInfo>(x => new UnitWorkInfo { CompanyId = 1, UserId = 1 }); /*TESTE*/
            services.AddScoped<UnitWork>();
        }

        public static void Initialize(IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            using(var context = serviceProvider.GetService<AppDbContext>())
            {
                context.Database.Migrate();

                var scriptsPath = Path.Combine(env.ContentRootPath, "Database/Scripts");
                context.Seed(serviceProvider.GetService<ILoggerFactory>().CreateLogger<AppDbContext>(), scriptsPath);
            }
        }
    }
}
