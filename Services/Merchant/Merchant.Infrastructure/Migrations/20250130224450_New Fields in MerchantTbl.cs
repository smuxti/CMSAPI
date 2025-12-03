using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Merchants.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewFieldsinMerchantTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "tblMerchant",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "tblMerchant",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherEmail",
                table: "tblMerchant",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherNumber",
                table: "tblMerchant",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "tblMerchant");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "tblMerchant");

            migrationBuilder.DropColumn(
                name: "OtherEmail",
                table: "tblMerchant");

            migrationBuilder.DropColumn(
                name: "OtherNumber",
                table: "tblMerchant");
        }
    }
}
