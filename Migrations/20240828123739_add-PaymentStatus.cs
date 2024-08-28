using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.Migrations
{
    /// <inheritdoc />
    public partial class addPaymentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "efc0295f-1247-4605-81c3-b5c10d8bc9b7", "AQAAAAIAAYagAAAAEKh+SnNtOLlsX0ntmOkMqIwKHUrHd1qTpPcNxR5nHLwxvqgvqeQcfu818UzWc4p7pA==", "408a1c36-654c-4e72-9308-e2737d7cde74" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "74bdaebe-60a7-4c7b-aab9-814155bdea73", "AQAAAAIAAYagAAAAEPqbIrSgkG0JGSEl8JLkBPThpceTEODhcTkvQFapP3op5DcAO7e9cHkiff+jQxEgnQ==", "a730af55-d810-421c-bc6a-8062ebe5af53" });
        }
    }
}
