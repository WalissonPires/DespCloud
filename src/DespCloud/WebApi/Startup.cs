using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApi.Database;
using WebApi.Repository;
using WebApi.Services.AddressSearch;
using WebApi.Services.AddressSearch.Postmon;
using WebApi.StartupConfig;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddServiceMVC();

            services.AddAutoMapper();
            services.AddTransient<IServiceZipCodeQuery, PostmonService>();

            DatabaseConfig.ConfigureServices(services, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {           
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            app.UseMiddleware<TryCatchMiddleware>();

            Mapper.Initialize(x => x.AddProfile(new AutoMapperProfile()));
            
            app.UseCors("CorsPolicy");

            //app.UseHttpsRedirection();
            app.UseMvc();

            using (var scope = app.ApplicationServices.CreateScope())
            {
                DatabaseConfig.Initialize(env, scope.ServiceProvider);
            }
        }
    }
}
