using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateTPT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Clients_ClientId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Properties_PropertyId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Companies_CompanyId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Owners_Companies_CompanyId",
                table: "Owners");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Contracts_ContractId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Properties_CompanyId_Status",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_Status",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ContractId_Status",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_Status",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_PropertyId_Status",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_Status",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Clients_Phone",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Bathrooms",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Method",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IdNumber",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "Floor",
                table: "Properties",
                newName: "ParkingSpots");

            migrationBuilder.RenameColumn(
                name: "Bedrooms",
                table: "Properties",
                newName: "AgeInYears");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<DateTime>(
                name: "InvitationAcceptedAt",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InvitationExpiry",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvitationToken",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsInvitationAccepted",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginAt",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetExpiry",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "PropertyMedias",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "PropertyId",
                table: "PropertyMedias",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MediaUrl",
                table: "PropertyMedias",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "MediaType",
                table: "PropertyMedias",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Image");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCover",
                table: "PropertyMedias",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "PropertyMedias",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PropertyMedias",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "PropertyMedias",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FileSizeInBytes",
                table: "PropertyMedias",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "PropertyMedias",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PropertyMedias",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Properties",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Properties",
                type: "float",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Properties",
                type: "float",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "District",
                table: "Properties",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Properties",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Properties",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Properties",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AgentId",
                table: "Properties",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeedNumber",
                table: "Properties",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacingDirection",
                table: "Properties",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MunicipalityNumber",
                table: "Properties",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentPropertyId",
                table: "Properties",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropertyCode",
                table: "Properties",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PropertyStatus",
                table: "Properties",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "Properties",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RegaLicenseNumber",
                table: "Properties",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitNumber",
                table: "Properties",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "Payments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Payments",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Payments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PaymentNumber",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "Payments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Owners",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Owners",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionRate",
                table: "Owners",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Owners",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IBAN",
                table: "Owners",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Owners",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalId",
                table: "Owners",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Owners",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerType",
                table: "Owners",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Contracts",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Contracts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "ContractType",
                table: "Contracts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "CancellationReason",
                table: "Contracts",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledAt",
                table: "Contracts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommissionStatus",
                table: "Contracts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CommissionType",
                table: "Contracts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ContractNumber",
                table: "Contracts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContractStatus",
                table: "Contracts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentCycle",
                table: "Contracts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Contracts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RenewedFromContractId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SecurityDeposit",
                table: "Contracts",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SubscriptionPlan",
                table: "Companies",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Basic");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "Companies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Companies",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Companies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "Clients",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Clients",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LeadStatus",
                table: "Clients",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "New");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Clients",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedAgentId",
                table: "Clients",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalId",
                table: "Clients",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Clients",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Appointments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualVisitAt",
                table: "Appointments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancellationReason",
                table: "Appointments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledAt",
                table: "Appointments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Appointments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "Appointments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApartmentProperties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Bedrooms = table.Column<int>(type: "int", nullable: false),
                    Bathrooms = table.Column<int>(type: "int", nullable: false),
                    LivingRooms = table.Column<int>(type: "int", nullable: true),
                    FloorNumber = table.Column<int>(type: "int", nullable: true),
                    HasMaidRoom = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasElevator = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasCentralAC = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasBalcony = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasStorage = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    FurnishedStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApartmentProperties_Properties_Id",
                        column: x => x.Id,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuildingProperties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalFloors = table.Column<int>(type: "int", nullable: true),
                    UnitsCount = table.Column<int>(type: "int", nullable: true),
                    BasementFloors = table.Column<int>(type: "int", nullable: true),
                    HasElevator = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasParkingFloor = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasMosque = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasGuard = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasGenerator = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasCCTV = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuildingProperties_Properties_Id",
                        column: x => x.Id,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cheques",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChequeNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChequeOrder = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DepositedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClearedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BouncedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BounceReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReplacedByChequeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cheques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cheques_Cheques_ReplacedByChequeId",
                        column: x => x.ReplacedByChequeId,
                        principalTable: "Cheques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cheques_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cheques_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LandProperties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StreetWidth = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    NumberOfStreets = table.Column<int>(type: "int", nullable: true),
                    ZoningType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsCornerLand = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsWalled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasElectricity = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasWater = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasSewer = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LandShape = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LandProperties_Properties_Id",
                        column: x => x.Id,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ResolutionNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContractorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ContractorPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignedToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_Users_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "OfficeProperties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FloorNumber = table.Column<int>(type: "int", nullable: true),
                    Bathrooms = table.Column<int>(type: "int", nullable: true),
                    OfficesCount = table.Column<int>(type: "int", nullable: true),
                    MeetingRooms = table.Column<int>(type: "int", nullable: true),
                    HasElevator = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasCentralAC = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasReceptionArea = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasKitchen = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasStorage = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasCCTV = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    FurnishedStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficeProperties_Properties_Id",
                        column: x => x.Id,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FileSizeInBytes = table.Column<long>(type: "bigint", nullable: true),
                    MimeType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyDocuments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropertyDocuments_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VillaProperties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Bedrooms = table.Column<int>(type: "int", nullable: false),
                    Bathrooms = table.Column<int>(type: "int", nullable: false),
                    LivingRooms = table.Column<int>(type: "int", nullable: true),
                    Floors = table.Column<int>(type: "int", nullable: true),
                    HasMaidRoom = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasDriverRoom = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasPool = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasGarden = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    GardenArea = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    HasElevator = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasMosque = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasMajlis = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasStorage = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasCCTV = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasGenerator = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    FurnishedStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VillaProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VillaProperties_Properties_Id",
                        column: x => x.Id,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseProperties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CeilingHeight = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    LoadingDocks = table.Column<int>(type: "int", nullable: true),
                    GateCount = table.Column<int>(type: "int", nullable: true),
                    ElectricityCapacity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HasOfficeSpace = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasSecurityRoom = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasCCTV = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasFireSystem = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasColdStorage = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasMosanada = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsFenced = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasTruckAccess = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseProperties_Properties_Id",
                        column: x => x.Id,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceMedias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FileSizeInBytes = table.Column<long>(type: "bigint", nullable: true),
                    MimeType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MediaType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UploadedByType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UploadedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Stage = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaintenanceRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceMedias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceMedias_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceMedias_MaintenanceRequests_MaintenanceRequestId",
                        column: x => x.MaintenanceRequestId,
                        principalTable: "MaintenanceRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId_Email",
                table: "Users",
                columns: new[] { "CompanyId", "Email" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId_IsActive",
                table: "Users",
                columns: new[] { "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId_Role",
                table: "Users",
                columns: new[] { "CompanyId", "Role" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_InvitationToken",
                table: "Users",
                column: "InvitationToken");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsActive",
                table: "Users",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PasswordResetToken",
                table: "Users",
                column: "PasswordResetToken");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role",
                table: "Users",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyMedias_CompanyId",
                table: "PropertyMedias",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyMedias_IsCover",
                table: "PropertyMedias",
                column: "IsCover");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyMedias_MediaType",
                table: "PropertyMedias",
                column: "MediaType");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyMedias_PropertyId_IsCover",
                table: "PropertyMedias",
                columns: new[] { "PropertyId", "IsCover" });

            migrationBuilder.CreateIndex(
                name: "IX_PropertyMedias_PropertyId_SortOrder",
                table: "PropertyMedias",
                columns: new[] { "PropertyId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_AgentId",
                table: "Properties",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_CompanyId_City",
                table: "Properties",
                columns: new[] { "CompanyId", "City" });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_CompanyId_PropertyCode",
                table: "Properties",
                columns: new[] { "CompanyId", "PropertyCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_CompanyId_PropertyStatus",
                table: "Properties",
                columns: new[] { "CompanyId", "PropertyStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_CompanyId_Purpose",
                table: "Properties",
                columns: new[] { "CompanyId", "Purpose" });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_IsFeatured",
                table: "Properties",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_ParentPropertyId",
                table: "Properties",
                column: "ParentPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_ParentPropertyId_UnitNumber",
                table: "Properties",
                columns: new[] { "ParentPropertyId", "UnitNumber" },
                unique: true,
                filter: "[ParentPropertyId] IS NOT NULL AND [UnitNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyStatus",
                table: "Properties",
                column: "PropertyStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_Purpose",
                table: "Properties",
                column: "Purpose");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CompanyId",
                table: "Payments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CompanyId_DueDate",
                table: "Payments",
                columns: new[] { "CompanyId", "DueDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CompanyId_PaymentStatus",
                table: "Payments",
                columns: new[] { "CompanyId", "PaymentStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ContractId",
                table: "Payments",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ContractId_PaymentNumber",
                table: "Payments",
                columns: new[] { "ContractId", "PaymentNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ContractId_PaymentStatus",
                table: "Payments",
                columns: new[] { "ContractId", "PaymentStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentMethod",
                table: "Payments",
                column: "PaymentMethod");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentStatus",
                table: "Payments",
                column: "PaymentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_CompanyId_NationalId",
                table: "Owners",
                columns: new[] { "CompanyId", "NationalId" },
                unique: true,
                filter: "[NationalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_CompanyId_Phone",
                table: "Owners",
                columns: new[] { "CompanyId", "Phone" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Owners_IsActive",
                table: "Owners",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_OwnerType",
                table: "Owners",
                column: "OwnerType");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ClientId_ContractStatus",
                table: "Contracts",
                columns: new[] { "ClientId", "ContractStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CompanyId",
                table: "Contracts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CompanyId_ContractNumber",
                table: "Contracts",
                columns: new[] { "CompanyId", "ContractNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CompanyId_ContractStatus",
                table: "Contracts",
                columns: new[] { "CompanyId", "ContractStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CompanyId_EndDate",
                table: "Contracts",
                columns: new[] { "CompanyId", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ContractStatus",
                table: "Contracts",
                column: "ContractStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_PropertyId",
                table: "Contracts",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_PropertyId_ContractStatus",
                table: "Contracts",
                columns: new[] { "PropertyId", "ContractStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_RenewedFromContractId",
                table: "Contracts",
                column: "RenewedFromContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_IsActive",
                table: "Companies",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_SubscriptionExpiry",
                table: "Companies",
                column: "SubscriptionExpiry");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_SubscriptionPlan",
                table: "Companies",
                column: "SubscriptionPlan");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_AssignedAgentId",
                table: "Clients",
                column: "AssignedAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CompanyId_AssignedAgentId",
                table: "Clients",
                columns: new[] { "CompanyId", "AssignedAgentId" });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CompanyId_LeadStatus",
                table: "Clients",
                columns: new[] { "CompanyId", "LeadStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CompanyId_Phone",
                table: "Clients",
                columns: new[] { "CompanyId", "Phone" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_IsActive",
                table: "Clients",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AgentId_ScheduledAt",
                table: "Appointments",
                columns: new[] { "AgentId", "ScheduledAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CompanyId",
                table: "Appointments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CompanyId_ScheduledAt",
                table: "Appointments",
                columns: new[] { "CompanyId", "ScheduledAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CompanyId_Status",
                table: "Appointments",
                columns: new[] { "CompanyId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ScheduledAt",
                table: "Appointments",
                column: "ScheduledAt");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Status",
                table: "Appointments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentProperties_Bedrooms",
                table: "ApartmentProperties",
                column: "Bedrooms");

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentProperties_FloorNumber",
                table: "ApartmentProperties",
                column: "FloorNumber");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingProperties_TotalFloors",
                table: "BuildingProperties",
                column: "TotalFloors");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingProperties_UnitsCount",
                table: "BuildingProperties",
                column: "UnitsCount");

            migrationBuilder.CreateIndex(
                name: "IX_Cheques_CompanyId",
                table: "Cheques",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Cheques_CompanyId_DueDate",
                table: "Cheques",
                columns: new[] { "CompanyId", "DueDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Cheques_CompanyId_Status",
                table: "Cheques",
                columns: new[] { "CompanyId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Cheques_ContractId",
                table: "Cheques",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Cheques_ContractId_ChequeOrder",
                table: "Cheques",
                columns: new[] { "ContractId", "ChequeOrder" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cheques_DueDate",
                table: "Cheques",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Cheques_ReplacedByChequeId",
                table: "Cheques",
                column: "ReplacedByChequeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cheques_Status",
                table: "Cheques",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_LandProperties_IsCornerLand",
                table: "LandProperties",
                column: "IsCornerLand");

            migrationBuilder.CreateIndex(
                name: "IX_LandProperties_StreetWidth",
                table: "LandProperties",
                column: "StreetWidth");

            migrationBuilder.CreateIndex(
                name: "IX_LandProperties_ZoningType",
                table: "LandProperties",
                column: "ZoningType");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceMedias_CompanyId",
                table: "MaintenanceMedias",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceMedias_MaintenanceRequestId",
                table: "MaintenanceMedias",
                column: "MaintenanceRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceMedias_MaintenanceRequestId_MediaType",
                table: "MaintenanceMedias",
                columns: new[] { "MaintenanceRequestId", "MediaType" });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceMedias_MaintenanceRequestId_Stage",
                table: "MaintenanceMedias",
                columns: new[] { "MaintenanceRequestId", "Stage" });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceMedias_Stage",
                table: "MaintenanceMedias",
                column: "Stage");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceMedias_UploadedByType",
                table: "MaintenanceMedias",
                column: "UploadedByType");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_AssignedToId",
                table: "MaintenanceRequests",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_AssignedToId_Status",
                table: "MaintenanceRequests",
                columns: new[] { "AssignedToId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_Category",
                table: "MaintenanceRequests",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_ClientId",
                table: "MaintenanceRequests",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_CompanyId",
                table: "MaintenanceRequests",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_CompanyId_Priority",
                table: "MaintenanceRequests",
                columns: new[] { "CompanyId", "Priority" });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_CompanyId_RequestNumber",
                table: "MaintenanceRequests",
                columns: new[] { "CompanyId", "RequestNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_CompanyId_Status",
                table: "MaintenanceRequests",
                columns: new[] { "CompanyId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_Priority",
                table: "MaintenanceRequests",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_PropertyId",
                table: "MaintenanceRequests",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_PropertyId_Status",
                table: "MaintenanceRequests",
                columns: new[] { "PropertyId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_Status",
                table: "MaintenanceRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeProperties_FloorNumber",
                table: "OfficeProperties",
                column: "FloorNumber");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeProperties_OfficesCount",
                table: "OfficeProperties",
                column: "OfficesCount");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDocuments_CompanyId",
                table: "PropertyDocuments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDocuments_CompanyId_ExpiryDate",
                table: "PropertyDocuments",
                columns: new[] { "CompanyId", "ExpiryDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDocuments_DocumentType",
                table: "PropertyDocuments",
                column: "DocumentType");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDocuments_ExpiryDate",
                table: "PropertyDocuments",
                column: "ExpiryDate");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDocuments_PropertyId",
                table: "PropertyDocuments",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDocuments_PropertyId_DocumentType",
                table: "PropertyDocuments",
                columns: new[] { "PropertyId", "DocumentType" });

            migrationBuilder.CreateIndex(
                name: "IX_VillaProperties_Bedrooms",
                table: "VillaProperties",
                column: "Bedrooms");

            migrationBuilder.CreateIndex(
                name: "IX_VillaProperties_Floors",
                table: "VillaProperties",
                column: "Floors");

            migrationBuilder.CreateIndex(
                name: "IX_VillaProperties_HasPool",
                table: "VillaProperties",
                column: "HasPool");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProperties_CeilingHeight",
                table: "WarehouseProperties",
                column: "CeilingHeight");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProperties_ElectricityCapacity",
                table: "WarehouseProperties",
                column: "ElectricityCapacity");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProperties_HasColdStorage",
                table: "WarehouseProperties",
                column: "HasColdStorage");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Clients_ClientId",
                table: "Appointments",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Companies_CompanyId",
                table: "Appointments",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Properties_PropertyId",
                table: "Appointments",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Companies_CompanyId",
                table: "Clients",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Users_AssignedAgentId",
                table: "Clients",
                column: "AssignedAgentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Companies_CompanyId",
                table: "Contracts",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Contracts_RenewedFromContractId",
                table: "Contracts",
                column: "RenewedFromContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Owners_Companies_CompanyId",
                table: "Owners",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Companies_CompanyId",
                table: "Payments",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Contracts_ContractId",
                table: "Payments",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Properties_ParentPropertyId",
                table: "Properties",
                column: "ParentPropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Users_AgentId",
                table: "Properties",
                column: "AgentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyMedias_Companies_CompanyId",
                table: "PropertyMedias",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Clients_ClientId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Companies_CompanyId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Properties_PropertyId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Companies_CompanyId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Users_AssignedAgentId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Companies_CompanyId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Contracts_RenewedFromContractId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Owners_Companies_CompanyId",
                table: "Owners");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Companies_CompanyId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Contracts_ContractId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Properties_ParentPropertyId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Users_AgentId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyMedias_Companies_CompanyId",
                table: "PropertyMedias");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ApartmentProperties");

            migrationBuilder.DropTable(
                name: "BuildingProperties");

            migrationBuilder.DropTable(
                name: "Cheques");

            migrationBuilder.DropTable(
                name: "LandProperties");

            migrationBuilder.DropTable(
                name: "MaintenanceMedias");

            migrationBuilder.DropTable(
                name: "OfficeProperties");

            migrationBuilder.DropTable(
                name: "PropertyDocuments");

            migrationBuilder.DropTable(
                name: "VillaProperties");

            migrationBuilder.DropTable(
                name: "WarehouseProperties");

            migrationBuilder.DropTable(
                name: "MaintenanceRequests");

            migrationBuilder.DropIndex(
                name: "IX_Users_CompanyId_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CompanyId_IsActive",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CompanyId_Role",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_InvitationToken",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_IsActive",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PasswordResetToken",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Role",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_PropertyMedias_CompanyId",
                table: "PropertyMedias");

            migrationBuilder.DropIndex(
                name: "IX_PropertyMedias_IsCover",
                table: "PropertyMedias");

            migrationBuilder.DropIndex(
                name: "IX_PropertyMedias_MediaType",
                table: "PropertyMedias");

            migrationBuilder.DropIndex(
                name: "IX_PropertyMedias_PropertyId_IsCover",
                table: "PropertyMedias");

            migrationBuilder.DropIndex(
                name: "IX_PropertyMedias_PropertyId_SortOrder",
                table: "PropertyMedias");

            migrationBuilder.DropIndex(
                name: "IX_Properties_AgentId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_CompanyId_City",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_CompanyId_PropertyCode",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_CompanyId_PropertyStatus",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_CompanyId_Purpose",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_IsFeatured",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_ParentPropertyId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_ParentPropertyId_UnitNumber",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_PropertyStatus",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_Purpose",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CompanyId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CompanyId_DueDate",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CompanyId_PaymentStatus",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ContractId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ContractId_PaymentNumber",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ContractId_PaymentStatus",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentMethod",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentStatus",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Owners_CompanyId_NationalId",
                table: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_Owners_CompanyId_Phone",
                table: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_Owners_IsActive",
                table: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_Owners_OwnerType",
                table: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ClientId_ContractStatus",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_CompanyId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_CompanyId_ContractNumber",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_CompanyId_ContractStatus",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_CompanyId_EndDate",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ContractStatus",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_PropertyId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_PropertyId_ContractStatus",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_RenewedFromContractId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Companies_IsActive",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_SubscriptionExpiry",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_SubscriptionPlan",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Clients_AssignedAgentId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_CompanyId_AssignedAgentId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_CompanyId_LeadStatus",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_CompanyId_Phone",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_IsActive",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_AgentId_ScheduledAt",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_CompanyId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_CompanyId_ScheduledAt",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_CompanyId_Status",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_ScheduledAt",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_Status",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "InvitationAcceptedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "InvitationExpiry",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "InvitationToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsInvitationAccepted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastLoginAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetExpiry",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "PropertyMedias");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PropertyMedias");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "PropertyMedias");

            migrationBuilder.DropColumn(
                name: "FileSizeInBytes",
                table: "PropertyMedias");

            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "PropertyMedias");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "PropertyMedias");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "DeedNumber",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "FacingDirection",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "MunicipalityNumber",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "ParentPropertyId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "PropertyCode",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "PropertyStatus",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "RegaLicenseNumber",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "UnitNumber",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentNumber",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CommissionRate",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "IBAN",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "NationalId",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "OwnerType",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "CancelledAt",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "CommissionStatus",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "CommissionType",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ContractNumber",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ContractStatus",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "PaymentCycle",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "RenewedFromContractId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "SecurityDeposit",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "AssignedAgentId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "NationalId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ActualVisitAt",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "CancelledAt",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "ParkingSpots",
                table: "Properties",
                newName: "Floor");

            migrationBuilder.RenameColumn(
                name: "AgeInYears",
                table: "Properties",
                newName: "Bedrooms");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "PropertyMedias",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "PropertyId",
                table: "PropertyMedias",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "MediaUrl",
                table: "PropertyMedias",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "MediaType",
                table: "PropertyMedias",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Image",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<bool>(
                name: "IsCover",
                table: "PropertyMedias",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Properties",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Longitude",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Latitude",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "District",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Properties",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Bathrooms",
                table: "Properties",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Properties",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Available");

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Method",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Owners",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Owners",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdNumber",
                table: "Owners",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Contracts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContractType",
                table: "Contracts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Contracts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Active");

            migrationBuilder.AlterColumn<string>(
                name: "SubscriptionPlan",
                table: "Companies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Basic",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Companies",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "Clients",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LeadStatus",
                table: "Clients",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "New",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Appointments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_CompanyId_Status",
                table: "Properties",
                columns: new[] { "CompanyId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_Status",
                table: "Properties",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ContractId_Status",
                table: "Payments",
                columns: new[] { "ContractId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Status",
                table: "Payments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_PropertyId_Status",
                table: "Contracts",
                columns: new[] { "PropertyId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_Status",
                table: "Contracts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Phone",
                table: "Clients",
                column: "Phone");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Clients_ClientId",
                table: "Appointments",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Properties_PropertyId",
                table: "Appointments",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Companies_CompanyId",
                table: "Clients",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Owners_Companies_CompanyId",
                table: "Owners",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Contracts_ContractId",
                table: "Payments",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }
    }
}
