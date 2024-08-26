using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.Migrations
{
    /// <inheritdoc />
    public partial class addTotalPriceInOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f7fc4112-5ca8-4444-90c5-176ad44243a0", "AQAAAAIAAYagAAAAEMK/K+3lGOza7kSoUpDEhsdcrPm13WBV68uBXnCoZY1rDWH+U5Xpq+XV8KhgReB8xA==", "568fc799-2f80-4233-a8b8-a833b8f66f03" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "OrderItems");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "911d9d44-fd62-4d39-ac5e-79b9c50093d9", "AQAAAAIAAYagAAAAEK5MywkfIFgWJk8JyLlQY3cFktZxf74ukPBDM4S+QdmLFoN7hZEIpB0cSpI+Gl+vPw==", "e3de88ad-1b7d-47b3-a37c-ee7308a21f24" });
        }
    }
}
