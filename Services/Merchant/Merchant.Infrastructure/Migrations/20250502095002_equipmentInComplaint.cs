using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Merchants.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class equipmentInComplaint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EquipmentID",
                table: "tblComplaint",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EquipmentID",
                table: "tblComplaint");
        }
    }
}
