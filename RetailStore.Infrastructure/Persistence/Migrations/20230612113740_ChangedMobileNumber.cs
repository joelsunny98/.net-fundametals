using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangedMobileNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Customers");

            migrationBuilder.AddColumn<long>(
                name: "MobileNumber",
                table: "Customers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MobileNumber",
                table: "Customers");

            migrationBuilder.AddColumn<int>(
                name: "PhoneNumber",
                table: "Customers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
