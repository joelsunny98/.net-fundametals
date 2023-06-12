using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenamedPhoneNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MobileNumber",
                table: "Customers",
                newName: "PhoneNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Customers",
                newName: "MobileNumber");
        }
    }
}
