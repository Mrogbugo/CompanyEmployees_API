using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyEmployees.Migrations
{
    /// <inheritdoc />
    public partial class AddedRoleToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "743f8174-b99c-4216-9c88-ca74fd316e83", "5cfdeca8-8fa1-4e14-af11-e66aa4f39c16", "Manager", "MANAGER" },
                    { "92f2f77a-9112-4688-828e-26fae6fb26bd", "87aa5b94-0d3e-4e71-bd0e-85d790dade63", "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "743f8174-b99c-4216-9c88-ca74fd316e83");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92f2f77a-9112-4688-828e-26fae6fb26bd");
        }
    }
}
