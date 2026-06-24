using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_ContractId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_PropertyId",
                table: "Contracts");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Properties",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_City",
                table: "Properties",
                column: "City");

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
                name: "IX_Payments_DueDate",
                table: "Payments",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Status",
                table: "Payments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_EndDate",
                table: "Contracts",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_PropertyId_Status",
                table: "Contracts",
                columns: new[] { "PropertyId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_Status",
                table: "Contracts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_LeadStatus",
                table: "Clients",
                column: "LeadStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Phone",
                table: "Clients",
                column: "Phone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Properties_City",
                table: "Properties");

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
                name: "IX_Payments_DueDate",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_Status",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_EndDate",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_PropertyId_Status",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_Status",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Clients_LeadStatus",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_Phone",
                table: "Clients");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ContractId",
                table: "Payments",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_PropertyId",
                table: "Contracts",
                column: "PropertyId");
        }
    }
}
