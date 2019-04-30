using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Database.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ZipCode = table.Column<string>(fixedLength: true, maxLength: 8, nullable: true),
                    Number = table.Column<string>(maxLength: 30, nullable: false),
                    Street = table.Column<string>(maxLength: 200, nullable: false),
                    DistrictId = table.Column<int>(nullable: false),
                    DistrictName = table.Column<string>(maxLength: 200, nullable: false),
                    CityId = table.Column<int>(nullable: false),
                    CityName = table.Column<string>(maxLength: 200, nullable: false),
                    CountyId = table.Column<int>(nullable: false),
                    CountyName = table.Column<string>(maxLength: 100, nullable: false),
                    CountyInitials = table.Column<string>(fixedLength: true, maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Citys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CountyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Initials = table.Column<string>(fixedLength: true, maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CityId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    CpfCnpj = table.Column<string>(maxLength: 14, nullable: false),
                    Email = table.Column<string>(maxLength: 60, nullable: false),
                    Phone = table.Column<string>(fixedLength: true, maxLength: 11, nullable: false),
                    LogoPath = table.Column<string>(maxLength: 300, nullable: true),
                    AddressId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    CreateAt = table.Column<DateTimeOffset>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    CpfCnpj = table.Column<string>(maxLength: 14, nullable: true),
                    Phone = table.Column<string>(fixedLength: true, maxLength: 11, nullable: true),
                    ContactName = table.Column<string>(maxLength: 60, nullable: true),
                    Email = table.Column<string>(maxLength: 60, nullable: true),
                    RgIE = table.Column<string>(maxLength: 20, nullable: true),
                    Org = table.Column<string>(maxLength: 20, nullable: true),
                    AddressId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => new { x.CompanyId, x.Id });
                    table.ForeignKey(
                        name: "FK_Clients_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clients_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContextSequence",
                columns: table => new
                {
                    Context = table.Column<string>(maxLength: 20, nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    Value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContextSequence", x => new { x.CompanyId, x.Context });
                    table.ForeignKey(
                        name: "FK_ContextSequence_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Honorary = table.Column<decimal>(nullable: false),
                    Rate = table.Column<decimal>(nullable: false),
                    PlateCard = table.Column<decimal>(nullable: false),
                    Other = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => new { x.CompanyId, x.Id });
                    table.ForeignKey(
                        name: "FK_Services_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 60, nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    Password = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => new { x.CompanyId, x.Id });
                    table.ForeignKey(
                        name: "FK_Users_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    CreateAt = table.Column<DateTimeOffset>(nullable: false),
                    ClosedAt = table.Column<DateTimeOffset>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Total = table.Column<decimal>(nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => new { x.CompanyId, x.Id });
                    table.ForeignKey(
                        name: "FK_Orders_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Clients_CompanyId_ClientId",
                        columns: x => new { x.CompanyId, x.ClientId },
                        principalTable: "Clients",
                        principalColumns: new[] { "CompanyId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    Plate = table.Column<string>(fixedLength: true, maxLength: 7, nullable: false),
                    Chassis = table.Column<string>(fixedLength: true, maxLength: 17, nullable: true),
                    Renavam = table.Column<string>(fixedLength: true, maxLength: 11, nullable: true),
                    Model = table.Column<string>(maxLength: 60, nullable: true),
                    Manufacturer = table.Column<string>(maxLength: 60, nullable: true),
                    YearManufacture = table.Column<int>(nullable: true),
                    ModelYear = table.Column<int>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Color = table.Column<string>(maxLength: 20, nullable: true),
                    CityName = table.Column<string>(maxLength: 200, nullable: true),
                    CountyName = table.Column<string>(maxLength: 200, nullable: true),
                    CountyInitials = table.Column<string>(fixedLength: true, maxLength: 2, nullable: true),
                    ClientId = table.Column<int>(nullable: false),
                    CityId = table.Column<int>(nullable: true),
                    CountyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => new { x.CompanyId, x.Id });
                    table.ForeignKey(
                        name: "FK_Vehicles_Citys_CityId",
                        column: x => x.CityId,
                        principalTable: "Citys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicles_Countys_CountyId",
                        column: x => x.CountyId,
                        principalTable: "Countys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_Clients_CompanyId_ClientId",
                        columns: x => new { x.CompanyId, x.ClientId },
                        principalTable: "Clients",
                        principalColumns: new[] { "CompanyId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    Honorary = table.Column<decimal>(nullable: false),
                    Rate = table.Column<decimal>(nullable: false),
                    PlateCard = table.Column<decimal>(nullable: false),
                    Other = table.Column<decimal>(nullable: false),
                    Total = table.Column<decimal>(nullable: false),
                    VehicleId = table.Column<int>(nullable: false),
                    OrderId = table.Column<int>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => new { x.CompanyId, x.Id });
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_CompanyId_OrderId",
                        columns: x => new { x.CompanyId, x.OrderId },
                        principalTable: "Orders",
                        principalColumns: new[] { "CompanyId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Services_CompanyId_ServiceId",
                        columns: x => new { x.CompanyId, x.ServiceId },
                        principalTable: "Services",
                        principalColumns: new[] { "CompanyId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Vehicles_CompanyId_VehicleId",
                        columns: x => new { x.CompanyId, x.VehicleId },
                        principalTable: "Vehicles",
                        principalColumns: new[] { "CompanyId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_AddressId",
                table: "Clients",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CompanyId_CpfCnpj",
                table: "Clients",
                columns: new[] { "CompanyId", "CpfCnpj" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CompanyId_Email",
                table: "Clients",
                columns: new[] { "CompanyId", "Email" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CompanyId_Phone",
                table: "Clients",
                columns: new[] { "CompanyId", "Phone" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CompanyId_RgIE",
                table: "Clients",
                columns: new[] { "CompanyId", "RgIE" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_AddressId",
                table: "Companies",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CpfCnpj",
                table: "Companies",
                column: "CpfCnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Email",
                table: "Companies",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Phone",
                table: "Companies",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_CompanyId_OrderId",
                table: "OrderDetails",
                columns: new[] { "CompanyId", "OrderId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_CompanyId_ServiceId",
                table: "OrderDetails",
                columns: new[] { "CompanyId", "ServiceId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_CompanyId_VehicleId",
                table: "OrderDetails",
                columns: new[] { "CompanyId", "VehicleId" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CompanyId_ClientId",
                table: "Orders",
                columns: new[] { "CompanyId", "ClientId" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId_Email",
                table: "Users",
                columns: new[] { "CompanyId", "Email" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email_Password",
                table: "Users",
                columns: new[] { "Email", "Password" });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_Chassis",
                table: "Vehicles",
                column: "Chassis",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CityId",
                table: "Vehicles",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CountyId",
                table: "Vehicles",
                column: "CountyId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_Plate",
                table: "Vehicles",
                column: "Plate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_Renavam",
                table: "Vehicles",
                column: "Renavam",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CompanyId_ClientId",
                table: "Vehicles",
                columns: new[] { "CompanyId", "ClientId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContextSequence");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Citys");

            migrationBuilder.DropTable(
                name: "Countys");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Address");
        }
    }
}
