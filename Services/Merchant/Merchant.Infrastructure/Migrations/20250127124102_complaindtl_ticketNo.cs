using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Merchants.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class complaindtl_ticketNo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TickentNo",
                table: "tblComplaint");

            migrationBuilder.AddColumn<string>(
                name: "TickentNo",
                table: "tblComplaintDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TickentNo",
                table: "tblComplaintDetails");

            migrationBuilder.AddColumn<string>(
                name: "TickentNo",
                table: "tblComplaint",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
