using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.Migrations
{
    /// <inheritdoc />
    public partial class addStripeChargeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripeChargeId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "74bdaebe-60a7-4c7b-aab9-814155bdea73", "AQAAAAIAAYagAAAAEPqbIrSgkG0JGSEl8JLkBPThpceTEODhcTkvQFapP3op5DcAO7e9cHkiff+jQxEgnQ==", "a730af55-d810-421c-bc6a-8062ebe5af53" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeChargeId",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5e1a528b-97c1-420c-9df6-541f96a53f07", "AQAAAAIAAYagAAAAEBwIqGJClR+7SQ51oH+YmnAoyCbf8sw6Hx/CbC1yM6qSXz/4ib9/O3lQQcrDJXIBEA==", "2bc09d2c-17a2-4b93-916b-1a5d78827e8d" });
        }
    }
}
