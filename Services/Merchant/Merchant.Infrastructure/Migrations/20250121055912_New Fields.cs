using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Merchants.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "tblComplaintDetails");

            migrationBuilder.AddColumn<int>(
                name: "EscalationId",
                table: "tblComplaintDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "tblComplaintDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ManagementId",
                table: "tblComplaintDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EscalationId",
                table: "tblComplaintDetails");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "tblComplaintDetails");

            migrationBuilder.DropColumn(
                name: "ManagementId",
                table: "tblComplaintDetails");

            migrationBuilder.AddColumn<string>(
                name: "AssignedTo",
                table: "tblComplaintDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
