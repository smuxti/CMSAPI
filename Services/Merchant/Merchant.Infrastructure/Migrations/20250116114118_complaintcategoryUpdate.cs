using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Merchants.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class complaintcategoryUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "tblComplaintCategory");

            migrationBuilder.DropColumn(
                name: "ResponeType",
                table: "tblComplaintCategory");

            migrationBuilder.DropColumn(
                name: "ResponseTime",
                table: "tblComplaintCategory");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "tblEscalation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponeType",
                table: "tblEscalation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ResponseTime",
                table: "tblEscalation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MerchantName",
                table: "tblComplaint",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "tblEscalation");

            migrationBuilder.DropColumn(
                name: "ResponeType",
                table: "tblEscalation");

            migrationBuilder.DropColumn(
                name: "ResponseTime",
                table: "tblEscalation");

            migrationBuilder.DropColumn(
                name: "MerchantName",
                table: "tblComplaint");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "tblComplaintCategory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponeType",
                table: "tblComplaintCategory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ResponseTime",
                table: "tblComplaintCategory",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
