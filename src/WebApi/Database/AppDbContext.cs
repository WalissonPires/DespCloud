using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Database.Entities;
using WebApi.Database.Mapping;

namespace WebApi.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<ClientEty> Clients { get; set; }
        public DbSet<CompanyEty> Companies { get; set; }
        public DbSet<VehicleEty> Vehicles { get; set; }        
        public DbSet<ServiceEty> Services { get; set; }
        public DbSet<OrderServiceEty> Orders { get; set; }
        public DbSet<OrderDetailEty> OrderDetails { get; set; }
        public DbSet<ContextSequenceEty> ContextSequence { get; set; }
        public DbSet<UserEty> Users { get; set; }
        public DbSet<AddressCountyEty> Countys { get; set; }
        public DbSet<AddressCityEty> Citys { get; set; }
        public DbSet<AddressDistrictEty> Districts { get; set; }
        public DbSet<AddressEty> Address { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClientMap());
            modelBuilder.ApplyConfiguration(new CompanyMap());
            modelBuilder.ApplyConfiguration(new VehicleMap());
            modelBuilder.ApplyConfiguration(new ServiceMap());
            modelBuilder.ApplyConfiguration(new OrderServiceMap());
            modelBuilder.ApplyConfiguration(new OrderDetailMap());
            modelBuilder.ApplyConfiguration(new ContextSequenceMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new AddressCountyMap());
            modelBuilder.ApplyConfiguration(new AddressCityMap());
            modelBuilder.ApplyConfiguration(new AddressDistrictMap());
            modelBuilder.ApplyConfiguration(new AddressMap());

            base.OnModelCreating(modelBuilder);
        }

        public void Seed(ILogger<AppDbContext> logger, string scriptsPath)
        {
            if (Companies.Any())
                return;

            var scripts = new string [] { Path.Combine(scriptsPath, "countys.sql"), Path.Combine(scriptsPath, "citys.sql"), Path.Combine(scriptsPath, "districts.sql") };
            bool allFilesExists = scripts.Select(x => File.Exists(x) ? 0 : 1).Sum() == 0;

            if (allFilesExists)
            {
                using (var transaction = Database.BeginTransaction())
                {
                    foreach (var file in scripts)
                    {
                        var fileContent = System.IO.File.ReadAllLines(Path.Combine(scriptsPath, file));
                        foreach (var statement in fileContent)
                        {
                            this.Database.ExecuteSqlCommand(new RawSqlString(statement));
                        }
                    }

                    transaction.Commit();
                }
            }
            else
                logger.LogCritical("Scripts (Endereços) do banco de dados não encontrados");

            #region Data Test                       
            using (var transaction = Database.BeginTransaction())            
            {
                int companyId = 1;
                int clientId = 1;
                var unit = new Repository.UnitWork(new Repository.UnitWorkInfo { CompanyId = companyId }, this, null);

                Companies.Add(new CompanyEty
                {
                    Name = "Company Test",
                    CpfCnpj = "00011122299",
                    Email = "company@test.com",
                    Phone = "33900112233"
                });
                SaveChanges();

                Users.Add(new UserEty
                {
                    Id = unit.GenereteId("Users"),
                    CompanyId = companyId,
                    Name = "User Test",
                    Email = "user@test.com",
                    Password = "plantext"
                });
                SaveChanges();

                Clients.Add(new ClientEty
                {
                    Id = unit.GenereteId("Clients"),
                    CompanyId = companyId,
                    Name = "Client Test",
                    Email = "client@test.com",
                    CpfCnpj = "11122233388",
                    Phone = "33900886655",
                    ContactName = "C Test",
                    RgIE = "AA 12120000",
                    Org = "AA",
                    Address = new AddressEty
                    {
                        CountyId = 31,
                        CountyName = "Minas Gerais",
                        CountyInitials = "MG",
                        CityId = 3127701,
                        CityName = "GV",
                        DistrictId = 0,
                        DistrictName = "Centro",
                        Number = "60",
                        Street = "AV Minas Gerais",
                        ZipCode = "35060000",
                    }
                });
                SaveChanges();

                Services.Add(new ServiceEty
                {
                    Id = unit.GenereteId("Services"),
                    CompanyId = companyId,
                    Name = "Service Test",
                    Rate = 10,
                    PlateCard = 5,
                    Honorary = 30,
                    Other = 1
                });
                SaveChanges();

                Vehicles.Add(new VehicleEty
                {
                    Id = unit.GenereteId("Vehicles"),
                    CompanyId = companyId,
                    ClientId = clientId,
                    Model = "Model test",
                    Plate = "AAA0000",
                    Color = "Red",
                    Chassis = "CHS 000",
                    Renavam = "RNV 000",
                    ModelYear = 2019,
                    YearManufacture = 2019,
                    CountyId = 31,
                    CountyName = "Minas Gerais",
                    CountyInitials = "MG",
                    CityId = 3127701,
                    CityName = "Governador Valadares",
                    Type = VehicleTypeEty.MotorCycle
                });
                SaveChanges();

                transaction.Commit();
            }
            #endregion            
        }
    }
}
