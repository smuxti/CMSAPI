using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Merchants.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ComplaintAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Attachment",
                table: "tblComplaint",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attachment",
                table: "tblComplaint");
        }
    }
}
